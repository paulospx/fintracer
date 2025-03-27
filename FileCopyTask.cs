namespace FinTracer
{
    public class FileCopyTask
    {
        public string SourceFile { get; set; }
        public string TargetFile { get; set; }
        public DateTime? CopiedAt { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Period { get; set; } // Daily, Weekly, Monthly, Quarterly
        public string Warning { get; set; }
        public string Stakeholders { get; set; }
        public string Comment { get; set; }
    }
}
