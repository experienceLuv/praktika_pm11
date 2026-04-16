namespace KeeperProWpf.ViewModels
{
    public class ApplicationListItem
    {
        public int ApplicationId { get; set; }
        public string ApplicationType { get; set; } = "";
        public string DepartmentName { get; set; } = "";
        public string EmployeeName { get; set; } = "";
        public string DateStart { get; set; } = "";
        public string DateEnd { get; set; } = "";
        public string VisitPurpose { get; set; } = "";
        public string StatusName { get; set; } = "";
        public string RejectionReason { get; set; } = "";
        public string CreatedAt { get; set; } = "";
    }
}