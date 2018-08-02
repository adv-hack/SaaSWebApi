using System;
using System.Linq;
using SaaSDTO;
using SaaSDAL;

namespace SaaSService
{
    public class UserService
    {
        /// <summary>
        /// Function for Getting Login User Details by UserID
        /// </summary>
        /// <param name="userID">int</param>
        /// <returns>UserDTO</returns>
        public SaaSLogin GetUserDetails(int userID)
        {
            SaaSLogin objUserDTO = new SaaSLogin();

            //Check for Passing Argument
            if (userID <= 0)
                throw new Exception("Invalid Argument");

            using (HackSaaSEntities Context = new HackSaaSEntities())
            {
                try
                {
                    var user = Context.UserInfo.Where(x => x.UserID == userID).FirstOrDefault();
                    if (user == null || user.UserID <= 0)
                        throw new Exception("User does not Exists.");

                    //Converting User Entity to UserDTO
                    objUserDTO = ConvertUserEntitytoUserDTO(user);
                }
                catch
                {
                    throw new Exception("Error on Getting User Details,Try again later");
                }
            }
            return objUserDTO;
        }

        //Method to convert Table User Entity to UserDTO
        public SaaSLogin ConvertUserEntitytoUserDTO(UserInfo objUser)
        {
            SaaSLogin objUserDTO = new SaaSLogin();
            objUserDTO.UserID = objUser.UserID;
            objUserDTO.FullName = objUser.FullName;            
            objUserDTO.CreatedDtm = objUser.RegisteredDate;
            objUserDTO.UpdatedDtm = objUser.AmendedOn;            
            objUserDTO.Email = objUser.Email;
            return objUserDTO;
        }
    }
}