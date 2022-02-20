namespace Events.Core.Models.General
{
    public enum Constants
    {
        UserId,
        RoleId,
    }
    public class VULNERABILITY
    {
        public static readonly long WAITING_ORG_INFORMING = 30;
        public static readonly long WAITING_HEAD_DIRECTIONS = 29;
        public static readonly long WAITING_FIX = 31;
        public static readonly long WAITING_FIX_AUTHENTICATION = 32;
        public static readonly long FIX_DONE = 33;
        public static readonly long IGNORED_CASE = 34;
        public static readonly long WAITING_DOC = 35;
        public static readonly long WAITING_VARIFY = 10035;
    }


    public class NOTIFICATION
    {
        public static readonly long OPEN_NOTIFICATION = 10;
        public static readonly long CLOSED_NOTIFICATION = 12;
        public static readonly long IGNORED_NOTIFICATION = 11;
        public static readonly long CLOSED_INCIDENT = 9;
        public static readonly long INCIDENT = 8;
        public static readonly long EDIT_INCIDENT = 16;
        public static readonly long ADD_COMMENT = 6;
        public static readonly long ASSIGN_INCIDENT = 7;
        public static readonly long REQUEST_RESPONSE = 13;
        public static readonly long SEND_TO_VARIFY = 24;
        public static long ASSIGN_REPORT = 27;
        public static long ASSIGN_TASK = 28;
    }

    public class GENERAL_REPORT
    {
        public static readonly long OPEN_REPORT = 17;
        public static readonly long CLOSE_REPORT = 18;
    }
    public class TASK
    {           
        public static readonly long OPEN = 5;
        public static readonly long IN_PROGRESS = 3;
        public static readonly long CLOSED = 4;
        public static readonly long ADD_COMMENT = 2;
        public static readonly long REQUEST_RESPONSE = 19;
    }
    
    public class APT_STATUS
    {
        public static readonly long OPEN = 25;
        public static readonly long CLOSED = 26;

    }



}
