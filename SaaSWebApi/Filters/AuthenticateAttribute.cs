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

namespace SaaSWebAPI
{
    public class AuthenticateAttribute : ActionFilterAttribute
    {
        private static string url = string.Empty;

        public AuthenticateAttribute()
        {
            url = ConfigurationManager.AppSettings["WebApiURL"].ToString();
        }

        public static string CalculateHash(string user, string sharedkey, string url)
        {
            byte[] secretBytes = ASCIIEncoding.ASCII.GetBytes(sharedkey);
            HMACMD5 hmac = new HMACMD5(secretBytes);
            byte[] dataBytes = ASCIIEncoding.ASCII.GetBytes(url);
            byte[] computedHash = hmac.ComputeHash(dataBytes);
            return user + ":" + Convert.ToBase64String(computedHash);
        }

        private static string CalculateHash(string user)
        {
            LoginService objLogin = new LoginService();
            string sharedKey = objLogin.GetSharedkeybyUser(user);
            byte[] secretBytes = ASCIIEncoding.ASCII.GetBytes(sharedKey);
            HMACMD5 hmac = new HMACMD5(secretBytes);
            byte[] dataBytes = ASCIIEncoding.ASCII.GetBytes(url);
            byte[] computedHash = hmac.ComputeHash(dataBytes);
            return user + ":" + Convert.ToBase64String(computedHash);
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
            if (header.Equals(verifiedHash))
                return true;

            return false;
        }

        private bool IsAuthenticated(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;

            var authenticationString = GetHttpRequestHeader(headers, "Autherization");
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