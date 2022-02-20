namespace Events.Api
{
    public class SysConfiguration
    {
        public const string Name = "SysConfiguration";
        public static string Http = "Http";
        public static string Environment = "Environment";
        public static string WindowsIdentity = "WindowsIdentity";
        

        public string MethodName { get; set; }
        public long InternalSec { get; set; }
        public long ExternalSec { get; set; }
        public long NonSpacifiedOrgId { get; set; }
        public long SystemUserId { get; set; }
        public long TCDepartmentId { get; set; }
        public long HeadDepartment { get; set; }
    }
}