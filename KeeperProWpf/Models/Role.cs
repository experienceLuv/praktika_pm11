using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeperProWpf.Models
{
    [Table("roles", Schema = "keeperpro")]
    public class Role
    {
        [Key]
        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; } = "";
    }
}