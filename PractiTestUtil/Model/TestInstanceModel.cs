using Newtonsoft.Json;

namespace PractiTestNunit.PractiTestUtil.Model;

public class TestInstanceModel
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
    }

    public class Attributes
    {
        [JsonProperty("test-id")] public int TestId { get; set; }

        [JsonProperty("set-id")] public int SetId { get; set; }
    }
}