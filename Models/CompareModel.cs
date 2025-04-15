using System.ComponentModel.DataAnnotations;

namespace FinTracer.Models
{
    public class CompareModel
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Comments { get; set; }
        public string? Description { get; set; }
        public string? Period { get; set; }
        public required string SourceFile { get; set; }
        public required string TargetFile { get; set; } 
        public required string SourceCurve { get; set; }
        public required string TargetCurve { get; set; }
        public string? Maturities { get; set; }
        public string? Delta { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
