using System;
using System.Linq;
using System.Web;
using SaaSDAL;
using SaaSDTO;

namespace SaaSService
{
    public class LoginService
    {
        /// <summary>
        /// Function for checking Login User Credentials is valid or not
        /// </summary>
        /// <param name="loginDTO">LoginDTO</param>
        /// <returns>bool</returns>
        public bool ChkForLoginUserValidated(SaaSLogin loginDTO)
        {
            bool isValidUser = false;
            //Check for Passing Argument
            if (string.IsNullOrEmpty(loginDTO.UserName) || string.IsNullOrEmpty(loginDTO.Password))
                throw new Exception("Invalid Argument");

            using (HackSaaSEntities Context = new HackSaaSEntities())
            {
                try
                {
                    var login = Context.UserInfo.Where(x => x.UserName == loginDTO.UserName && x.Password == loginDTO.Password).FirstOrDefault();
                    if (login == null || login.UserID <= 0)
                        throw new Exception("User is Not Valid");
                    //'login.Status = (int)CrossCarePatientPortalPOCDAL.Enum.LoginStatus.AlreadyLogin;
                    //'HttpContext.Current.Session["PatientID"] = loginDTO.UserID;
                    //'login.SessionID = HttpContext.Current.Session.SessionID;
                    //Context.SaveChanges();
                    //'loginDTO.loginStatus = SaaSDTO.SaaSLogin.LoginStatus.AlreadyLogin;
                    //loginDTO.Login = true;
                    //'loginDTO.SessionId = login.SessionID;
                    //loginDTO.UserID = login.UserID;
                    isValidUser = true;
                }
                catch(Exception ex)
                {
                    throw new Exception("Error on Check Valid User,Try again later");
                }
            }
            return isValidUser;
        }

        /// <summary>
        /// Function for Getting User SharedKey
        /// </summary>
        /// <param name="username">string</param>
        /// <returns>string</returns>
        public UserInfo GetSharedkeybyUser(string username)
        {
            UserInfo User = new UserInfo() ;

            //Check for Passing Argument
            if (string.IsNullOrEmpty(username))
                throw new Exception("Invalid Argument");

            using (HackSaaSEntities Context = new HackSaaSEntities())
            {
                try
                {
                    var login = Context.UserInfo.Where(x => x.UserName == username).FirstOrDefault();
                    if (login == null || login.UserID <= 0)
                        throw new Exception("User Name is Not Valid");
                    User = login;
                }
                catch
                {
                    throw new Exception("Error on Get User Sharedkey ,Try again later");
                }
            }
            return User;
        }

        /// <summary>
        /// User Status Update by userID
        /// </summary>
        /// <param name="userID">int</param>
        /// <param name="status">CrossCarePatientPortalPOCDAL.Enum.LoginStatus</param>
        /// <returns></returns>
        public void UserLoginStatusUpdate(int userID, CrossCarePatientPortalPOCDAL.Enum.LoginStatus status)
        {
            //Check for Passing Argument
            if (userID <= 0)
                throw new Exception("Invalid Argument");

            using (HackSaaSEntities Context = new HackSaaSEntities())
            {
                try
                {
                    var login = Context.UserInfo.Where(x => x.UserID == userID).FirstOrDefault();
                    if (login == null || login.UserID <= 0)
                        throw new Exception("User is Not Valid");
                    //'login.Status = (int)status;

                    Context.SaveChanges();
                }
                catch
                {
                    throw new Exception("Error on Update Status by User,Try again later");
                }
            }
        }
    }
}