using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FinTracer.Models
{
    public class FileTransfer
    {
        [Key]
        public Guid TransferId { get; set; }
        public required string SourceFile { get; set; }
        public DateTime SourceCreatedAt { get; set; }
        public long SourceSize { get; set; }
        public string? SourceMd5 { get; set; }

        public required string TargetFile { get; set; }
        public DateTime TargetCreatedAt { get; set; }
        public long TargetSize { get; set; }
        public string? TargetMd5 { get; set; }

        public DateTime LastCopied { get; set; }
        public bool IsSelected { get; set; }
        public string? Status { get; set; }
        public DateTime ScheduledToCopy { get; set; }
    }
}
