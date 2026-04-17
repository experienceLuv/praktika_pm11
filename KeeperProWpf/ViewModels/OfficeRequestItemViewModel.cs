namespace KeeperProWpf.ViewModels
{
    public class OfficeRequestItemViewModel
    {
        public int ApplicationId { get; set; }
        public string TypeName { get; set; } = "";
        public string DepartmentName { get; set; } = "";
        public string EmployeeName { get; set; } = "";
        public string DateStart { get; set; } = "";
        public string DateEnd { get; set; } = "";
        public string VisitPurpose { get; set; } = "";
        public string StatusName { get; set; } = "";
        public string VisitorNames { get; set; } = "";
    }
}