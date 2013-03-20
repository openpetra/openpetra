//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Tools.Mantis.CloseResolvedBugs
{
    /// <summary>
    /// Program for automatic mass closing of Bugs in Mantis.
    /// </summary>
    /// <remarks>Usage: (1) Filter the desired Bugs in Mantis as follows: status=resolved, 'Fixed in Version': select all versions before a certain version (e.g. Alpha 0.2.20).
    /// (2) Export the list of Bugs as CSV file.
    /// (3) Run this tool with the export file as an input and the Version that you chose to not be included anymore (e.g. Alpha 0.2.20.) The tool will set all those bugs to closed and
    /// add a Note to each Bug that this was done by an automatic process.</remarks>
    class MantisCloseResolvedBugs
    {
        static private void LoginToSourceforge(IWebDriver ADriver, string ALoginURL, string AUsername, string APassword)
        {
            ADriver.Navigate().GoToUrl(ALoginURL);

            IWebElement txtLoginName = ADriver.FindElement(By.Name("form_loginname"));
            txtLoginName.SendKeys(AUsername);

            IWebElement txtPassword = ADriver.FindElement(By.Name("form_pw"));
            txtPassword.SendKeys(APassword);

            IWebElement chkRememberMe = ADriver.FindElement(By.Name("form_rememberme"));
            chkRememberMe.SendKeys(" ");

            IWebElement btnLogin = ADriver.FindElement(By.Name("login"));
            btnLogin.Click();
        }

        static private void SetResolvedBugToClosed(IWebDriver ADriver, string AEditBugURL, string AVersion)
        {
            TLogging.Log(AEditBugURL);
            ADriver.Navigate().GoToUrl(AEditBugURL);

            IWebElement cmbResolution = ADriver.FindElement(By.Name("status"));
            ReadOnlyCollection <IWebElement>options = cmbResolution.FindElements(By.TagName("option"));

            foreach (IWebElement option in options)
            {
                if ((option.Text == "resolved") && (option.Selected == false))
                {
                    TLogging.Log("*status* is not 'resolved', so we are not setting status to 'closed' for Bug #" + AEditBugURL + "!");
                    return;
                }
            }

            // Set 'status' to 'closed'
            foreach (IWebElement option in options)
            {
                if ("closed" == option.Text)
                {
                    option.Click();
                    break;
                }
            }

            // Add a note
            IWebElement txtBugNote = ADriver.FindElement(By.Name("bugnote_text"));
            txtBugNote.SendKeys("[Bug got automatically closed because it was resolved and it was fixed earlier than Version '" + AVersion + "'.]");


            // Submit the Form
            IWebElement form = ((IWebElement)cmbResolution).FindElement(By.XPath("../../../../.."));

            if (form.TagName == "form")
            {
                form.Submit();
            }
        }

        static void Main(string[] args)
        {
            string BugIDs = String.Empty;

            new TAppSettingsManager(false);

            if ((!TAppSettingsManager.HasValue("sf-username"))
                || (!TAppSettingsManager.HasValue("sf-pwd"))
                || (!TAppSettingsManager.HasValue("bugs-csv-file"))
                || (!TAppSettingsManager.HasValue("version-fixed-in-earlier-than"))
                )
            {
                Console.WriteLine(
                    "call: MantisCloseResolvedBugs.exe -sf-username:pokorra -sf-pwd:xyz -bugs-csv-file:resolvedbugs.csv -version-fixed-in-earlier-than:\"Alpha 0.2.20\"");
                return;
            }

            // Process CSV file: turn it into an XmlDocument for ease of use
            string bugsCSVFile = TAppSettingsManager.GetValue("bugs-csv-file");
            XmlDocument bugsXmlDoc = TCsv2Xml.ParseCSV2Xml(bugsCSVFile, ",");

            XmlNode RecordNode = bugsXmlDoc.FirstChild.NextSibling.FirstChild;

            // Extract all Bug ID's from the XmlDocument
            while (RecordNode != null)
            {
                BugIDs += TXMLParser.GetAttribute(RecordNode, "Id") + ",";

                RecordNode = RecordNode.NextSibling;
            }

            BugIDs = BugIDs.Substring(0, BugIDs.Length - 1);   // remove last comma ','


            // Start the processing in the web browser
            string loginURL = TAppSettingsManager.GetValue("login-url", "http://sourceforge.net/account/login.php");
            string mantisURL = TAppSettingsManager.GetValue("mantis-url", "https://sourceforge.net/apps/mantisbt/openpetraorg/");

            IWebDriver driver = new FirefoxDriver();

            try
            {
                LoginToSourceforge(driver, loginURL, TAppSettingsManager.GetValue("sf-username"), TAppSettingsManager.GetValue("sf-pwd"));

                string[] bugids = BugIDs.Split(new char[] { ',' });

                // Process each Bug
                foreach (string bugid in bugids)
                {
                    SetResolvedBugToClosed(
                        driver,
                        mantisURL + "bug_update_page.php?bug_id=" + bugid,
                        TAppSettingsManager.GetValue("version-fixed-in-earlier-than"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                StreamWriter sw = new StreamWriter("error.html");
                sw.WriteLine(driver.PageSource.Replace("a0:", "").Replace(":a0", ""));
                sw.Close();
                Console.WriteLine("please check " + Path.GetFullPath("error.html"));
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}