using FluentDate;
using FluentDateTime;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;

namespace AutoGymLogin
{
    public class GymLogin
    {
        public readonly ChromeDriver _driver;
        private readonly KeyValuePair<string, SecureString> _creds;

       

        public GymLogin(KeyValuePair<string, SecureString> creds)
        {
            _driver = new ChromeDriver(GetBrowserExecutablePath());
            _creds = creds;
        }

        public void NavigateToGymWebsite()
        {
            _driver.Navigate().GoToUrl("https://www.myiclubonline.com/iclub/members#account/checkinhistory");
        }

        public void InputUserCredentials()
        {
            var userID = "j_username";
            new WebDriverWait(_driver, TimeSpan.FromSeconds(.5)).Until(ExpectedConditions.ElementExists((By.Id(userID))));
            _driver.FindElementById("j_username").SendKeys(_creds.Key);
            _driver.FindElementById("j_password").SendKeys(new System.Net.NetworkCredential(string.Empty, _creds.Value).Password);
            
            new WebDriverWait(_driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementToBeClickable(By.Id("signIn")));
            _driver.FindElementById("signIn").SendKeys(Keys.Space);
        }

        public void InputDate()
        {
            var lastMonth = 1.Months().Ago().Date;
            var firstDayOfMonth = lastMonth.FirstDayOfMonth();
            var lastDayOfMonth = lastMonth.LastDayOfMonth();

            new WebDriverWait(_driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementExists((By.CssSelector("#checkinhistoryDateFilter > input.date.lowDate"))));

            var firstDay = _driver.FindElementByCssSelector("#checkinhistoryDateFilter > input.date.lowDate");
            firstDay.Clear();
            firstDay.SendKeys(firstDayOfMonth.ToShortDateString());

            var lastDay = _driver.FindElementByCssSelector("#checkinhistoryDateFilter > input.date.highDate");
            lastDay.Clear();
            lastDay.SendKeys(lastDayOfMonth.ToShortDateString());

            _driver.FindElementByClassName("showButton").Click();
        }

        public void DownloadPdf()
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementIsVisible(By.ClassName("downloadLink")));
            new WebDriverWait(_driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementToBeClickable(By.ClassName("downloadLink")));
            _driver.FindElementByClassName("downloadLink").SendKeys(Keys.Space);
        }

        public string GetBrowserExecutablePath()
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))));
        }
    }
}
