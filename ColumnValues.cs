using System.Text.Json.Serialization;

namespace FinTracer
{
    public class ColumnValues
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("data")]
        public required object[] Data { get; set; }
    }
}
