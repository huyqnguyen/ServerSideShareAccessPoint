using System;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using System.Text;

namespace Boshi.Tests
{
    [TestFixture]
    class LoginTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private IJavaScriptExecutor javaScriptExecutor;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            javaScriptExecutor = (IJavaScriptExecutor)driver;
            baseURL = "http://localhost:52057/";
            verificationErrors = new StringBuilder();
            wait = new WebDriverWait(driver, new TimeSpan(0, 3, 0));
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheIngNewApplicationWithEdittingChildSInformationsTest()
        {
            driver.Navigate().GoToUrl(baseURL + "/login");
            wait.Until(ExpectedConditions.ElementExists(By.LinkText("会員登録・ログインはこちら"))).Click();
            wait.Until(ExpectedConditions.ElementExists(By.LinkText("メールアドレスで使う"))).Click();
            wait.Until(ExpectedConditions.ElementExists(By.Id("Password"))).Clear();
            driver.FindElement(By.Id("Password")).SendKeys("Anhbinpro2");
            driver.FindElement(By.Id("Email")).Clear();
            driver.FindElement(By.Id("Email")).SendKeys("huynq@mti-tech.vn");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.button__move.button__block"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("span.fs8"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("編集"))).Click();
            wait.Until(ExpectedConditions.ElementExists(By.Id("Name"))).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Dan");
            //driver.FindElement(By.Id("Sex")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("li > label"))).Click();
            //driver.FindElement(By.CssSelector("li > label")).Click();
            new SelectElement(driver.FindElement(By.Id("BirthDay"))).SelectByText("3");
            new SelectElement(driver.FindElement(By.Id("Rh"))).SelectByText("＋");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Forward"))).Click();
            Thread.Sleep(15000);
            driver.FindElement(By.Name("Forward")).Click();
            //javaScriptExecutor.ExecuteScript("$(\"[name=\'Forward\']\").click()");
            //javaScriptExecutor.ExecuteScript("alert('12345678')");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a.link-02 > div"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("i.glyphicon.glyphicon-icon_hamburger"))).Click();
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText("ログアウト"))).Click();
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
