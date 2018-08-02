using System.Net;
using System.Web.Http;
using SaaSDTO;
using SaaSDAL;
using SaaSService;
using System.Web;
using System.Text;

namespace SaaSWebAPI
{
    [Authenticate]
    public class LoginController : ApiController
    {
        //API for Login Credentials check for UserSSSSS

        [HttpGet]
        public SaaSResponse AuthenticateUser(string uname, string pin)
        {
            bool IsValidUser = false;
            SaaSLogin objLoginDTO = new SaaSLogin();
            SaaSResponse objResponseXML = new SaaSResponse();

            try
            {
                if (!string.IsNullOrEmpty(uname))
                {
                    objLoginDTO.UserName = uname;
                }
                if (!string.IsNullOrEmpty(pin))
                {
                    objLoginDTO.Password = pin;
                }

                LoginService objLoginBL = new LoginService();
                IsValidUser = objLoginBL.ChkForLoginUserValidated(objLoginDTO);

                //objResponseXML.CrosscareData.Result = objLoginDTO.ToString();
                objResponseXML._saasData = Common.GetXMLFromObject<SaaSLogin>(objLoginDTO);
            }
            catch
            {
                objResponseXML._fault = HttpStatusCode.Unauthorized.ToString();
                // throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            return objResponseXML;
            //return new HttpResponseMessage() { Content = new StringContent(, Encoding.UTF8, "application/xml") };
        }

        [HttpPost]
        public string HMACSharedKey()
        {
            string hMACDetails = FormBufferToString();
            string user = string.Empty, sharedkey = string.Empty, url = string.Empty;
            string strHmacString = string.Empty;
            HMACDetails objHMACDetails = new HMACDetails();
            objHMACDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<HMACDetails>(hMACDetails);
            strHmacString = AuthenticateAttribute.CalculateHash(objHMACDetails.user, objHMACDetails.sharedkey, objHMACDetails.url);
            return strHmacString;
        }

        public class HMACDetails
        {
            public string user { get; set; }

            public string sharedkey { get; set; }

            public string url { get; set; }
        }

        public static string FormBufferToString()
        {
            HttpRequest Request = HttpContext.Current.Request;

            if (Request.TotalBytes > 0)
                return Encoding.Default.GetString(Request.BinaryRead(Request.TotalBytes));

            return string.Empty;
        }
    }
}