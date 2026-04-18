using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeperProWpf.Security.Models
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

        public override string ToString() => DepartmentName;
    }
}