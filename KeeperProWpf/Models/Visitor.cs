using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeperProWpf.Models
{
    [Table("visitors", Schema = "keeperpro")]
    public class Visitor
    {
        [Key]
        [Column("visitor_id")]
        public int VisitorId { get; set; }

        [Column("last_name")]
        public string LastName { get; set; } = "";

        [Column("first_name")]
        public string FirstName { get; set; } = "";

        [Column("middle_name")]
        public string? MiddleName { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("email")]
        public string Email { get; set; } = "";

        [Column("organization")]
        public string? Organization { get; set; }

        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        [Column("passport_series")]
        public string PassportSeries { get; set; } = "";

        [Column("passport_number")]
        public string PassportNumber { get; set; } = "";

        [Column("photo_path")]
        public string? PhotoPath { get; set; }
    }
}