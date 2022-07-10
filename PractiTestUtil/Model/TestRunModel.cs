using Newtonsoft.Json;

namespace PractiTestNunit.PractiTestUtil.Model;

public class TestRunModel
{
    public RootData Data { get; set; }

    public class RootData
    {
        [JsonProperty("data")] public List<RunData> Data { get; set; } = new();
    }

    public class RunData
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("attributes")] public Attributes Attributes { get; set; }

        [JsonProperty("steps")] public Steps Steps { get; set; }
    }

    public class Attributes
    {
        [JsonProperty("instance-id")] public long InstanceId { get; set; }
    }

    public class Steps
    {
        [JsonProperty("data")] public List<StepData>? StepData { get; set; }
    }

    public class StepData
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("expected-results")] public string ExpectedResults { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}