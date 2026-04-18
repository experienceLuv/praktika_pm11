using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace KeeperProWpf.Department.Models
{
    [Table("users", Schema = "keeperpro")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("email")]
        public string Email { get; set; } = "";

        [Column("password_hash")]
        public string PasswordHash { get; set; } = "";

        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        public Role? Role { get; set; }
    }
}