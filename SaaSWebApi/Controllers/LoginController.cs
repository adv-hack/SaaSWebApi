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

        [HttpPost]
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
    }
}