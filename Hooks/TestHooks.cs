using OpenQA.Selenium;
using PractiTestNunit.PractiTestUtil;
using PractiTestNunit.PractiTestUtil.Model;
using XUnitDemo.Driver;

namespace PractiTestNunit.Hooks;

[Binding]
public class TestHooks
{
    private static List<TestRunModel.StepData>? _steps;
    private static PractiTestDetails? _practiTestDetails;
    private readonly ScenarioContext _scenarioContext;

    public TestHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static void InitializeTestRunDetails()
    {
        //This details should come from either configuration file or via ENVIRONMENT VARIABLE
        _practiTestDetails = new PractiTestDetails
        {
            TestName = "EA UI Test",
            TestSetName = "Testing 3.6 UI features",
            TestPriority = "highest",
            TestSetVersion = "3.6"
        };
    }


    [BeforeFeature]
    public static void InitializePractiTest()
    {
        var practiTest = new PractiTest();
        practiTest.CreatePractiTestSession(_practiTestDetails.TestName, _practiTestDetails.TestSetName,
            _practiTestDetails.TestPriority,
            _practiTestDetails.TestSetVersion);

        //Initialize steps
        _steps = new List<TestRunModel.StepData>();
    }

    [AfterStep]
    public void InsertTestRunSteps()
    {
        if (ScenarioContext.Current.TestError == null)
            _steps.Add(new TestRunModel.StepData
            {
                Name = ScenarioStepContext.Current.StepInfo.Text,
                ExpectedResults = ScenarioStepContext.Current.StepInfo.Text,
                Status = "PASSED"
            });
        else if (ScenarioContext.Current.TestError != null)
            _steps.Add(new TestRunModel.StepData
            {
                Name = ScenarioStepContext.Current.StepInfo.Text,
                ExpectedResults = ScenarioStepContext.Current.StepInfo.Text,
                Status = "FAILED"
            });
    }


    [BeforeScenario]
    public void InitializeWebDriver()
    {
        //initialize Selenium WebDriver
        _scenarioContext.Set<IWebDriver>(new InitializeFixture().ChromeDriver, "driver");
    }

    [AfterFeature]
    public static void PublishTestRunSteps()
    {
        var practiTest = new PractiTest();
        var testInstanceId = practiTest.GetInstanceId(_practiTestDetails.TestName);
        practiTest.CreateTestSteps(_steps, testInstanceId);
    }


    [AfterScenario]
    public void Cleanup()
    {
        _scenarioContext.Get<IWebDriver>("driver").Quit();
    }
}

public class PractiTestDetails
{
    public string TestName { get; set; }
    public string TestSetName { get; set; }
    public string TestPriority { get; set; }
    public string TestSetVersion { get; set; }
}