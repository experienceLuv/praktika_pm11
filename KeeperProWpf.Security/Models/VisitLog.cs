using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeperProWpf.Security.Models
{
    [Table("visit_logs", Schema = "keeperpro")]
    public class VisitLog
    {
        [Key]
        [Column("visit_log_id")]
        public int VisitLogId { get; set; }

        [Column("application_id")]
        public int ApplicationId { get; set; }

        [Column("visitor_id")]
        public int VisitorId { get; set; }

        [Column("security_employee_id")]
        public int? SecurityEmployeeId { get; set; }

        [Column("department_employee_id")]
        public int? DepartmentEmployeeId { get; set; }

        [Column("entry_time")]
        public DateTime? EntryTime { get; set; }

        [Column("exit_time")]
        public DateTime? ExitTime { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }
    }
}