using System.Text.Json.Serialization;

namespace FinTracer
{
    public class ColumnValues
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("data")]
        public required object[] Data { get; set; }
        public string[] Category {get;set;} = new List<string>().ToArray();
        public double Sum { get; set; }
    }
}
