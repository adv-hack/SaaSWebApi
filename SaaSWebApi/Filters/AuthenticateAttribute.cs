using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using SaaSService;
using System.Web;
using SaaSDAL;

namespace SaaSWebAPI
{
    public class AuthenticateAttribute : ActionFilterAttribute
    {
        private static string url = string.Empty;

        public AuthenticateAttribute()
        {
            url = ConfigurationManager.AppSettings["WebApiURL"].ToString();
        }

        public static string CalculateHash(string user, string sharedkey)
        {
            byte[] secretBytes = ASCIIEncoding.ASCII.GetBytes(sharedkey);
            HMACMD5 hmac = new HMACMD5(secretBytes);
            var urllocal = HttpContext.Current.Request;
            var updatedurl = GetBaseUrl(urllocal);
            byte[] dataBytes = ASCIIEncoding.ASCII.GetBytes(updatedurl);
            byte[] computedHash = hmac.ComputeHash(dataBytes);
            return user + ":" + Convert.ToBase64String(computedHash);
        }

        private static string CalculateHash(string user)
        {
            LoginService objLogin = new LoginService();
            UserInfo userInfo = objLogin.GetSharedkeybyUser(user);
            byte[] secretBytes = ASCIIEncoding.ASCII.GetBytes(user+":"+userInfo.Token);
            HMACMD5 hmac = new HMACMD5(secretBytes);
            byte[] dataBytes = ASCIIEncoding.ASCII.GetBytes(userInfo.URL);
            byte[] computedHash = hmac.ComputeHash(dataBytes);
            return user + ":" + Convert.ToBase64String(computedHash);
        }

        public static string GetBaseUrl(HttpRequest request)
        {
            return string.Format("{0}://{1}", request.UrlReferrer.Scheme, request.UrlReferrer.Authority);
        }

        private static string GetHttpRequestHeader(HttpHeaders headers, string headerName)
        {
            if (!headers.Contains(headerName))
                return string.Empty;

            return headers.GetValues(headerName)
                            .SingleOrDefault();
        }

        private static bool IsAuthenticated(string userName, string header)
        {
            var verifiedHash = CalculateHash(userName);
            var getUserHashKeyFromHeader = CalculateHash(userName, header);
            if (getUserHashKeyFromHeader.Equals(verifiedHash))
                return true;

            return false;
        }

        private bool IsAuthenticated(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;

            var authenticationString = GetHttpRequestHeader(headers, "Authorization");
            if (string.IsNullOrEmpty(authenticationString))
                return false;

            var authenticationParts = authenticationString.Split(new[] { ":" },
                    StringSplitOptions.RemoveEmptyEntries);

            if (authenticationParts.Length != 2)
                return false;

            var username = authenticationParts[0];
            var signature = authenticationParts[1];

            return IsAuthenticated(username, authenticationString);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            bool isAuthenticated = IsAuthenticated(actionContext);

            if (!isAuthenticated)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                actionContext.Response = response;
            }
        }
    }
}