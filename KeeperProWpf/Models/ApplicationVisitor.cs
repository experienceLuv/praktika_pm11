using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeperProWpf.Models
{
    [Table("application_visitors", Schema = "keeperpro")]
    public class ApplicationVisitor
    {
        [Key]
        [Column("application_visitor_id")]
        public int ApplicationVisitorId { get; set; }

        [Column("application_id")]
        public int ApplicationId { get; set; }

        [Column("visitor_id")]
        public int VisitorId { get; set; }

        [Column("visitor_order")]
        public int VisitorOrder { get; set; }
    }
}