using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeperProWpf.Department.Models
{
    [Table("departments", Schema = "keeperpro")]
    public class Department
    {
        [Key]
        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Column("department_name")]
        public string DepartmentName { get; set; } = "";

        [Column("description")]
        public string? Description { get; set; }

        [Column("transfer_minutes_limit")]
        public int TransferMinutesLimit { get; set; }
    }
}