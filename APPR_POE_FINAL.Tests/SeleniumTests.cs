using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace APPR_POE_FINAL.Tests;

public class SeleniumTests : IDisposable
{

    private const string BaseUrl = "https://giftofthegivers-poe-final-2026-dddneyetc4cjfpc7.southafricanorth-01.azurewebsites.net";

    private const string EmployeeEmail = "employee2@test.com";
    private const string EmployeePassword = "Test@1234";

    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public SeleniumTests()
    {
        var options = new ChromeOptions();
        options.AddArgument("--window-size=1366,900");
        _driver = new ChromeDriver(options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
    }

    [Fact]
    public void Test_Home_Page_Loads()
    {
        _driver.Navigate().GoToUrl(BaseUrl);
        _wait.Until(d => d.FindElements(By.TagName("nav")).Count > 0);

        Assert.Contains("Gift of the Givers", _driver.PageSource, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Test_Employee_Login()
    {
        _driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");

        _wait.Until(d => d.FindElement(By.Id("Input_Email"))).SendKeys(EmployeeEmail);
        _driver.FindElement(By.Id("Input_Password")).SendKeys(EmployeePassword);
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        // After login the nav shows the greeting and the Employee Portal link
        _wait.Until(d => d.PageSource.Contains("Employee Portal"));
        Assert.Contains("Employee Portal", _driver.PageSource);
    }

    [Fact]
    public void Test_Donation_Form_Anonymous_Option()
    {
        // Donation page needs a logged-in user, so log in first
        _driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");
        _wait.Until(d => d.FindElement(By.Id("Input_Email"))).SendKeys(EmployeeEmail);
        _driver.FindElement(By.Id("Input_Password")).SendKeys(EmployeePassword);
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        _wait.Until(d => d.PageSource.Contains("Logout"));

        _driver.Navigate().GoToUrl($"{BaseUrl}/Donation");
        _wait.Until(d => d.FindElement(By.Id("Amount"))).SendKeys("250");

        var anonymous = _driver.FindElement(By.Id("IsAnonymous"));
        // The toggle is styled, so click it with JavaScript
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", anonymous);

        Assert.True(anonymous.Selected);
        Assert.Equal("250", _driver.FindElement(By.Id("Amount")).GetAttribute("value"));
    }

    public void Dispose() => _driver.Quit();
}