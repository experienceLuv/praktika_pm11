using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeperProWpf.Models
{
    [Table("application_statuses", Schema = "keeperpro")]
    public class ApplicationStatus
    {
        [Key]
        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("status_name")]
        public string StatusName { get; set; } = "";
    }
}