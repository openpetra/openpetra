//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.IO;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using Ict.Common;

namespace Ict.Testing.BrowserTests.OnlineRegistration
{
    class TestOnlineRegistration
    {
        static private void ScreenshotsOfOnlineRegistration(IWebDriver ADriver, string ABaseURL, string ACountryID, string ARole, string AOutPath)
        {
            ADriver.Navigate().GoToUrl(
                String.Format(ABaseURL + "/Apps/OnlineRegistration/index.aspx?country-id={0}&role-id={1}&validate=false",
                    ACountryID, ARole));

            int step = 1;

            while (true)
            {
                string pngfile = AOutPath + Path.DirectorySeparatorChar + string.Format("test-{0}-{1}-{2}.png", ACountryID, ARole, step++);

                ((FirefoxDriver)ADriver).GetScreenshot().SaveAsFile(
                    pngfile, ImageFormat.Png);

                Console.WriteLine("writing " + pngfile);

                IWebElement btnNext = ADriver.FindElement(By.Id("btn-next-btnEl"));

                if ((btnNext != null) && btnNext.Displayed)
                {
                    btnNext.Click();
                }
                else
                {
                    IWebElement btnFinish = ADriver.FindElement(By.Id("btn-finish-btnEl"));

                    if ((btnFinish != null) && btnFinish.Displayed)
                    {
                        // btnFinish.Click();
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            IWebDriver driver = new FirefoxDriver();

            new TAppSettingsManager(false);

            string baseURL = TAppSettingsManager.GetValue("base-url", "http://localhost:8081");
            string countryID = TAppSettingsManager.GetValue("country-id", "*");
            string roleID = TAppSettingsManager.GetValue("role-id", "*");
            string pathGen = TAppSettingsManager.GetValue("path-generated");
            string pathOutput = TAppSettingsManager.GetValue("path-out");

            try
            {
                string[] files = Directory.GetFiles(pathGen, "*.js");
                Regex FileRegex = new Regex("main\\.(.*)\\.(.*)\\.js");

                foreach (string filename in files)
                {
                    Match match = FileRegex.Match(filename);

                    if (match.Success)
                    {
                        if (((countryID == "*") || (match.Groups[1].Value == countryID))
                            && ((roleID == "*") || (match.Groups[2].Value == roleID)))
                        {
                            ScreenshotsOfOnlineRegistration(driver, baseURL, match.Groups[1].Value, match.Groups[2].Value, pathOutput);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}