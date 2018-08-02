namespace CrossCarePatientPortalPOCDAL
{
    public class Enum
    {
        public enum LoginStatus
        {
            None = 0,
            FirstTime = 1,
            AlreadyLogin = 2,
            Locked = 3,
            Expired = 4
        }

        public enum PatientAppointmentStatus
        {
            None = 0,
            Booked = 1,
            Cancelled = 2
        }
    }
}