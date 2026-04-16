using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeperProWpf.Models
{
    [Table("applications", Schema = "keeperpro")]
    public class ApplicationEntity
    {
        [Key]
        [Column("application_id")]
        public int ApplicationId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("application_type_id")]
        public int ApplicationTypeId { get; set; }

        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Column("date_start")]
        public DateTime DateStart { get; set; }

        [Column("date_end")]
        public DateTime DateEnd { get; set; }

        [Column("visit_purpose")]
        public string VisitPurpose { get; set; } = "";

        [Column("note")]
        public string Note { get; set; } = "";

        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("rejection_reason")]
        public string? RejectionReason { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public Department? Department { get; set; }
        public Employee? Employee { get; set; }
        public ApplicationStatus? Status { get; set; }
        public ApplicationType? ApplicationType { get; set; }
    }
}