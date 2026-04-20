using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeperProWpf.Office.Models
{
    [Table("black_list_entries", Schema = "keeperpro")]
    public class BlackListEntry
    {
        [Key]
        [Column("black_list_entry_id")]
        public int BlackListEntryId { get; set; }

        [Column("visitor_id")]
        public int VisitorId { get; set; }

        [Column("reason")]
        public string Reason { get; set; } = "";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("added_by_employee_id")]
        public int? AddedByEmployeeId { get; set; }
    }
}