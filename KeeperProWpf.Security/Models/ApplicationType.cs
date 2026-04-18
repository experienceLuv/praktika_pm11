using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeperProWpf.Security.Models
{
    [Table("application_types", Schema = "keeperpro")]
    public class ApplicationType
    {
        [Key]
        [Column("application_type_id")]
        public int ApplicationTypeId { get; set; }

        [Column("type_name")]
        public string TypeName { get; set; } = "";
    }
}