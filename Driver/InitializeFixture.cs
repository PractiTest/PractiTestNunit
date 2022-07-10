using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace XUnitDemo.Driver;

/// <summary>
///     InitializeFixture code for XUnit to handle
///     Selenium WebDriver and PractiTest
/// </summary>
public class InitializeFixture : IDisposable
{
    public InitializeFixture()
    {
        //WebDriverManager
        new DriverManager().SetUpDriver(new ChromeConfig());
        ChromeDriver = new ChromeDriver();
    }

    public ChromeDriver ChromeDriver { get; }

    public void Dispose()
    {
        ChromeDriver.Quit();
        ChromeDriver.Dispose();
    }
}