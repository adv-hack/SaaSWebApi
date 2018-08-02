using System;
namespace SaaSDTO
{
    public class SaaSLogin
    {
        public int ID { get; set; }

        public long UserID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool Login { get; set; }       

        public string FullName { get; set; }

        public string Email { get; set; }       

        public DateTime? CreatedDtm { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDtm { get; set; }

        public string UpdatedBy { get; set; }

        public enum LoginStatus
        {
            None = 0,
            FirstTime = 1,
            AlreadyLogin = 2,
            Locked = 3,
            Expired = 4
        }
    }
}