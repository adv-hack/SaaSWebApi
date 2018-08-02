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
using SaaSDTO;

namespace SaaSWebAPI
{
    public class AuthenticateAttribute : ActionFilterAttribute
    {

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
            byte[] secretBytes = ASCIIEncoding.ASCII.GetBytes(user + ":" + userInfo.Token);
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
            SaaSResponse objResponseXML = new SaaSResponse();            
            bool isAuthenticated = IsAuthenticated(actionContext);

            if (!isAuthenticated)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                actionContext.Response = response;
            }
            else
            {
                SaaSLogin objLoginDTO = new SaaSLogin();
                LoginService objLoginBL = new LoginService();
                string hMACDetails = FormBufferToString();
                string user = string.Empty, sharedkey = string.Empty, url = string.Empty;
                string strHmacString = string.Empty;
                HMACDetails objHMACDetails = new HMACDetails();
                objHMACDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<HMACDetails>(hMACDetails);
                if (!string.IsNullOrEmpty(objHMACDetails.user))
                {
                    objLoginDTO.UserName = objHMACDetails.user;
                }
                if (!string.IsNullOrEmpty(objHMACDetails.password))
                {
                    objLoginDTO.Password = objHMACDetails.password;
                }
                bool IsValidUser = objLoginBL.ChkForLoginUserValidated(objLoginDTO);
                if (!IsValidUser)
                {
                    var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    actionContext.Response = response;
                }
            }
        }

        public static string FormBufferToString()
        {
            HttpRequest Request = HttpContext.Current.Request;

            if (Request.TotalBytes > 0)
                return Encoding.Default.GetString(Request.BinaryRead(Request.TotalBytes));

            return string.Empty;
        }

        public class HMACDetails
        {
            public string user { get; set; }

            public string sharedkey { get; set; }

            public string password { get; set; }
        }
    }
}