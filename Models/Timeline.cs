namespace FinTracer.Models
{
    public class Timeline
    {
        public Guid TimeLineId { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string Username { get; set; }
        public required string Title { get; set; }
        public required string SubTitle { get; set; }
        public required string XAxis { get; set; }
        public required string YAxis { get; set; }
        public required string Tooltip { get; set; }
        public required string Notes { get; set; }
        public required string ChartType { get; set; }
        public required string Category { get; set; }
        public required string Series { get; set; }
        public required string Settings { get; set; }
        public bool Enabled { get; set; }
    }
}
