using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestEGallery
{
    [TestFixture]
    public class UnitTest
    {
        private string EGalleryUrl = "http://localhost:49917/";

        public ChromeDriver getDriver()
        {
            var driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(EGalleryUrl);
            return driver;
        }

        [Test]
        public void TestLoginAndLogoutValidate()
        {
            var driver = getDriver();
            driver.FindElement(By.Name("Email")).SendKeys("kama@gmail.com");
            driver.FindElement(By.Name("Password")).SendKeys("kama1489s");
            driver.FindElementByClassName("form-login-user").Submit();
            Assert.AreEqual(EGalleryUrl + "Gallery/Home/000000001", driver.Url, "Test personal page user");
            driver.Navigate().GoToUrl(EGalleryUrl + "/Account/Login");
            Assert.AreEqual(EGalleryUrl + "Gallery/Home/000000001", driver.Url, "Test personal page user");
            var links = driver.FindElements(By.ClassName("link-page"));
            links.Last().FindElement(By.TagName("a")).Click();
            Assert.AreEqual(EGalleryUrl, driver.Url, "Test logout user");
            driver.Quit();
        }

        [Test]
        public void TestLoginNoValidate()
        {
            var driver = getDriver();
            driver.FindElement(By.Name("Email")).SendKeys("111111");
            driver.FindElement(By.Name("Password")).SendKeys("111111");
            driver.FindElementByClassName("form-login-user").Submit();
            Assert.AreEqual(EGalleryUrl + "?enctype=multipart%2Fform-data", driver.Url, "Test No such user");
            var divValidationSummaryErrors = driver.FindElementByClassName("validation-summary-errors");
            var message = divValidationSummaryErrors.FindElements(By.TagName("li")).First().Text;
            Assert.AreEqual("There is no such user", message, "Test message");
            driver.Quit();
        }

        [Test]
        public void TestRegistration()
        {
            var driver = getDriver();
            driver.FindElementByClassName("button-form").FindElement(By.TagName("a")).Click(); //go to Registration page
            Assert.AreEqual(EGalleryUrl + "Account/Registration", driver.Url, "Test rigistration url page");
            driver.FindElementByClassName("form-registration-user").Submit();  //test validation field
            var errorSpan = driver.FindElements(By.ClassName("field-validation-error"));
            Assert.AreEqual(4, errorSpan.Count, "Test count error message");
            driver.FindElement(By.Name("Email")).SendKeys("kama@gmail.com");
            driver.FindElement(By.Name("Username")).SendKeys("");
            driver.FindElement(By.Name("Password")).SendKeys("1qazxsw2");
            driver.FindElement(By.Name("ConfirmPassword")).SendKeys("1qazxsw2");

            driver.FindElementByClassName("form-registration-user").Submit();
            errorSpan = driver.FindElements(By.ClassName("field-validation-error"));
            Assert.AreEqual(2, errorSpan.Count, "Test count error message");
            Assert.AreEqual("Username is required.", errorSpan.First().Text, "Test error message Nick");
            Assert.AreEqual("The password must contain the numbers and letters of the two registers.", errorSpan.Last().Text, "Test error message Password");

            driver.FindElement(By.Name("Email")).Clear();

            driver.FindElement(By.Name("Email")).SendKeys("kama@gmail.com");
            driver.FindElement(By.Name("Username")).SendKeys("Test Nick");
            driver.FindElement(By.Name("Password")).SendKeys("1qazxsW2");
            driver.FindElement(By.Name("ConfirmPassword")).SendKeys("1qazxsw2");
            driver.FindElementByClassName("form-registration-user").Submit();
            errorSpan = driver.FindElements(By.ClassName("field-validation-error"));
            Assert.AreEqual(1, errorSpan.Count, "Test count error message");
            Assert.AreEqual("Please confirm your password.", errorSpan.First().Text, "Test error message Confirm Password");

            driver.FindElement(By.Name("Email")).Clear();
            driver.FindElement(By.Name("Username")).Clear();

            driver.FindElement(By.Name("Email")).SendKeys("kama@gmail.com");
            driver.FindElement(By.Name("Username")).SendKeys("Test Nick");
            driver.FindElement(By.Name("Password")).SendKeys("1qazxsW2");
            driver.FindElement(By.Name("ConfirmPassword")).SendKeys("1qazxsW2");
            driver.FindElementByClassName("form-registration-user").Submit();
            errorSpan = driver.FindElements(By.ClassName("field-validation-error"));
            Assert.AreEqual(1, errorSpan.Count, "Test count error message");
            Assert.AreEqual("User with this email exist.", errorSpan.First().Text, "Test error message Email");

            driver.FindElement(By.Name("Email")).Clear();
            driver.FindElement(By.Name("Username")).Clear();

            driver.FindElement(By.Name("Email")).SendKeys("kama");
            driver.FindElement(By.Name("Username")).SendKeys("Test Nick");
            driver.FindElement(By.Name("Password")).SendKeys("1qazxsW2");
            driver.FindElement(By.Name("ConfirmPassword")).SendKeys("1qazxsW2");
            driver.FindElementByClassName("form-registration-user").Submit();
            errorSpan = driver.FindElements(By.ClassName("field-validation-error"));
            Assert.AreEqual(1, errorSpan.Count, "Test count error message");
            Assert.AreEqual("Invalid email address.", errorSpan.First().Text, "Test error message Email");

            driver.FindElement(By.Name("Email")).Clear();
            driver.FindElement(By.Name("Username")).Clear();

            driver.FindElement(By.Name("Email")).SendKeys("test@test.test");
            driver.FindElement(By.Name("Username")).SendKeys("Test Nick");
            driver.FindElement(By.Name("Password")).SendKeys("1qazxsW2");
            driver.FindElement(By.Name("ConfirmPassword")).SendKeys("1qazxsW2");
            driver.FindElementByClassName("form-registration-user").Submit();

            Assert.AreEqual(EGalleryUrl + "Account/Verification", driver.Url, "Test registration user");
            driver.Quit();
        }
        
        [Test]
        public void TestGalleryUser()
        {
            var driver = getDriver();
            var links = driver.FindElements(By.ClassName("link-page"));
            links[1].FindElement(By.TagName("a")).Click();

            Thread.Sleep(1000);

            var userDiv = driver.FindElement(By.ClassName("user"));
            var url = userDiv.FindElement(By.TagName("a")).GetAttribute("href");

            userDiv.FindElement(By.ClassName("avatar-user")).Click();

            Assert.AreEqual(url, driver.Url, "Test personal page user");

            Thread.Sleep(1000);
            var images = driver.FindElement(By.ClassName("block-gallery")).FindElements(By.TagName("img"));

            foreach (var img in images)
            {
                img.Click();
                driver.FindElement(By.Id("select-image")).Click();
            }
            var countClickButton = 0;
            images.First().Click();

            while (driver.FindElementByClassName("next-image").Displayed)
            {
                driver.FindElementByClassName("next-image").Click();
                countClickButton++;
            }
            Assert.AreEqual(images.Count - 1, countClickButton, "Test count click");
            
            while (driver.FindElementByClassName("prev-image").Displayed)
            {
                driver.FindElementByClassName("prev-image").Click();
                countClickButton--;
            }
            Assert.AreEqual(0, countClickButton, "Test count click");
            driver.Quit();
        }
    }
}