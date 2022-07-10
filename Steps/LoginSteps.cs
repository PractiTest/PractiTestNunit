using OpenQA.Selenium;
using TechTalk.SpecFlow.Assist;

namespace PractiTestNunit.Steps;

[Binding]
public class LoginSteps
{
    private readonly IWebDriver _driver;

    public LoginSteps(ScenarioContext scenarioContext)
    {
        _driver = scenarioContext.Get<IWebDriver>("driver");
    }


    [Given(@"I navigate to the application")]
    public void GivenINavigateToTheApplication()
    {
        _driver.Navigate().GoToUrl("http://eaapp.somee.com");
    }

    [Given(@"I click Login")]
    public void GivenIClickLogin()
    {
        _driver.FindElement(By.LinkText("Login")).Click();
    }

    [Given(@"I login with following details")]
    public void GivenILoginWithFollowingDetails(Table table)
    {
        dynamic data = table.CreateDynamicInstance();
        _driver.FindElement(By.Id("UserName")).SendKeys(data.UserName);
        _driver.FindElement(By.Id("Password")).SendKeys(data.Password);
        _driver.FindElement(By.CssSelector(".btn-default")).Click();
    }

    [Then(@"I should see the ""([^""]*)""")]
    public void GivenIShouldSeeThe(string link)
    {
        var element = _driver.FindElement(By.LinkText(link));
        element.Should().NotBeNull();
    }
}