using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PractiTestNunit.PractiTestUtil.Model;

namespace PractiTestNunit.PractiTestUtil;

public class PractiTest
{
    public const string apiToken = "f776c8c861b27701e4f8790f24a1209b0931225a";
    public const string developerEmail = "karthik@techgeek.co.in";
    public const string projectId = "13052";

    private readonly HttpClient _client;

    public PractiTest()
    {
        _client = new HttpClient();
        //For US Prod, the URL is https://api.practitest.com
        _client.BaseAddress = new Uri("https://eu1-prod-api.practitest.app/api/v2/");
        var byteArray = Encoding.ASCII.GetBytes($"{developerEmail}:{apiToken}");
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }

    public void CreatePractiTestSession(string testName, string testSetName, string priority, string version)
    {
        var testId = IsTestExist(testName);
        var setId = IsTestSetExist(testSetName);
        var instanceId = IsTestInstanceExist(testId, setId);

        //Step 1: Create New Test
        if (testId == 0)
            testId = CreateNewTest(testName, priority);

        //Step 2: Create New Test Set
        if (setId == 0)
            setId = CreateTestSet(testSetName, priority, version).Result;

        if (testId != 0 && setId != 0)
        {
            var instanceAttribute = new TestInstanceModel.Attributes
            {
                SetId = setId,
                TestId = testId
            };

            //Step 3:Create Test Instance
            if (instanceId == 0)
                CreateTestInstance(instanceAttribute);

        }
    }


    private int CreateNewTest(string testName, string priority)
    {
        var data = @"{
                            ""data"": {
                                ""type"": ""tests"",
                                ""attributes"": {
                                    ""name"": """ + testName + @""",
                                    ""author-id"": 27809,
                                    ""priority"": """ + priority + @"""
                                }
                            }
                        }";

        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
        var response = _client
            .PostAsync($"projects/{projectId}/tests.json", content)
            .Result;
        var result = response.Content.ReadAsStringAsync().Result;

        dynamic res = JObject.Parse(result);

        return int.Parse(res.data.id.ToString());
    }

    private int IsTestExist(string testName)
    {
        var response = _client.GetAsync($"projects/{projectId}/tests.json?name_exact={testName}").Result;

        var result = response.Content.ReadAsStringAsync().Result;

        dynamic res = JObject.Parse(result);

        return res.data.Count > 0 ? (int)res.data[0].id : 0;
    }

    private async Task<int> CreateTestSet(string testSetName, string priority, string version)
    {
        var data = @"{
                            ""data"": {
                                ""type"": ""set"",
                                ""attributes"": {
                                    ""name"": """ + testSetName + @""",
                                    ""author-id"": 27809,
                                    ""priority"": """ + priority + @""",
                                    ""version"": """ + version + @"""
                                }
                            }
                        }";
        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
        var response =
            await _client.PostAsync($"projects/{projectId}/sets.json",
                content);


        var result = response.Content.ReadAsStringAsync().Result;

        dynamic res = JObject.Parse(result);

        return int.Parse(res.data.id.ToString());
    }

    private int IsTestSetExist(string testSetName)
    {
        var response = _client.GetAsync($"projects/{projectId}/sets.json?name_exact={testSetName}").Result;

        var result = response.Content.ReadAsStringAsync().Result;

        dynamic res = JObject.Parse(result);

        return res.data.Count > 0 ? (int)res.data[0].id : 0;
    }

    private int CreateTestInstance(TestInstanceModel.Attributes attributes)
    {
        var testInstanceData = new TestInstanceModel.RootData
        {
            Data = new List<TestInstanceModel.RunData>
            {
                new()
                {
                    Attributes = attributes,
                    Type = "Instance"
                }
            }
        };

        var data = JsonConvert.SerializeObject(testInstanceData);

        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
        var response = _client
            .PostAsync($"projects/{projectId}/instances.json" , content)
            .Result;

        var result = response.Content.ReadAsStringAsync().Result;

        dynamic res = JObject.Parse(result);

        return int.Parse(res.data.id.ToString());
    }

    private int IsTestInstanceExist(int testId, int testSetId)
    {
        var response = _client.GetAsync($"projects/{projectId}/instances.json?set-ids={testSetId}&test-ids={testId}").Result;

        var result = response.Content.ReadAsStringAsync().Result;

        dynamic res = JObject.Parse(result);

        return res.data.Count > 0 ? (int)res.data[0].id : 0;
    }


    public void CreateTestSteps(List<TestRunModel.StepData>? stepData, int instanceId)
    {
        var testRunData = new TestRunModel.RootData
        {
            Data = new List<TestRunModel.RunData>
            {
                new()
                {
                    Attributes = new TestRunModel.Attributes
                    {
                        InstanceId = instanceId
                    },
                    Steps = new TestRunModel.Steps
                    {
                        StepData = stepData
                    },
                    Type = "instances"
                }
            }
        };

        var data = JsonConvert.SerializeObject(testRunData);

        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
        try
        {
            var response =
                _client.PostAsync($"projects/{projectId}/runs.json",
                    content);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public int GetInstanceId(string testName)
    {
        var response = _client.GetAsync($"projects/{projectId}/instances.json?name_exact={testName}").Result;

        var result = response.Content.ReadAsStringAsync().Result;

        dynamic res = JObject.Parse(result);

        return res.data.Count > 0 ? (int)res.data[0].id : 0;
    }
}
