using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeperProWpf.Models
{
    [Table("employees", Schema = "keeperpro")]
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Column("last_name")]
        public string LastName { get; set; } = "";

        [Column("first_name")]
        public string FirstName { get; set; } = "";

        [Column("middle_name")]
        public string? MiddleName { get; set; }

        [Column("position")]
        public string? Position { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("employee_code")]
        public string? EmployeeCode { get; set; }

        public Department? Department { get; set; }

        [NotMapped]
        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();

        public override string ToString() => FullName;
    }
}