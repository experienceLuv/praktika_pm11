using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeperProWpf.Models
{
    [Table("documents", Schema = "keeperpro")]
    public class DocumentEntity
    {
        [Key]
        [Column("document_id")]
        public int DocumentId { get; set; }

        [Column("application_id")]
        public int ApplicationId { get; set; }

        [Column("visitor_id")]
        public int? VisitorId { get; set; }

        [Column("document_type_id")]
        public int DocumentTypeId { get; set; }

        [Column("file_name")]
        public string FileName { get; set; } = "";

        [Column("file_path")]
        public string FilePath { get; set; } = "";

        [Column("uploaded_at")]
        public DateTime UploadedAt { get; set; }
    }
}