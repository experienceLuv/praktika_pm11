namespace KeeperProWpf.Security.ViewModels
{
    public class SecurityRequestItemViewModel
    {
        public int ApplicationId { get; set; }
        public string TypeName { get; set; } = "";
        public string DepartmentName { get; set; } = "";
        public string EmployeeName { get; set; } = "";
        public string DateStart { get; set; } = "";
        public string DateEnd { get; set; } = "";
        public string VisitorNames { get; set; } = "";
        public string PassportNumbers { get; set; } = "";
        public string StatusName { get; set; } = "";
    }
}