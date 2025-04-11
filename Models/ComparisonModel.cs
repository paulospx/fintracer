using System.ComponentModel.DataAnnotations;

namespace FinTracer.Models
{
    public class ComparisonModel
    {
        [Key]
        public Guid ComparisonId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? Period { get; set; }
        public string? CachedResult { get; set; }
        public string? CacheRmse { get; set; }
        public string? Mad { get; set; }
        public string? Area { get; set; }
        public DateTime CreateAt {get;set;}
        public DateTime LastUpdated { get; set; }
        public string? GenerateBy { get; set; }
    }
}
