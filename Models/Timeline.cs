namespace FinTracer.Models
{
    public class Timeline
    {
        public Guid TimeLineId { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string Username { get; set; }
        public required string Title { get; set; }
        public required string Notes { get; set; }
        public required string ChartType { get; set; }
        public required string Settings { get; set; }
        public bool Enabled { get; set; }
    }
}
