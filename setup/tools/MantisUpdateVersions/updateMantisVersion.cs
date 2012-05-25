//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using Ict.Common;

namespace Ict.Tools.Mantis.UpdateVersion
{
    class UpdateMantisVersion
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
        
        static private SortedList<int, string> GetAllProjects(IWebDriver ADriver, string AProjectPageURL)
        {
            ADriver.Navigate().GoToUrl(AProjectPageURL);
            ReadOnlyCollection<IWebElement> links = ADriver.FindElements(By.TagName("a"));
            
            SortedList<int, string> result = new SortedList<int, string>();
            
            foreach (IWebElement link in links)
            {
                // Console.WriteLine(link.Text + " " + link.GetAttribute("href"));
                string href = link.GetAttribute("href");
                if (href.Contains("manage_proj_edit_page.php?project_id="))
                {
                    result.Add(Convert.ToInt32(
                        href.Substring(href.IndexOf("project_id=") + "project_id=".Length)),
                        link.Text);
                }
            }
            
            return result;
        }

        static private void EditVersion(IWebDriver ADriver, string AVersionName, string ADateReleased, 
                                        string ADescription, bool AHasBeenReleased)
        {
            IWebElement txtVersionName = ADriver.FindElement(By.Name("new_version"));
            txtVersionName.Clear();
            txtVersionName.SendKeys(AVersionName);

            IWebElement txtDescription = ADriver.FindElement(By.Name("description"));
            txtDescription.Clear();
            txtDescription.SendKeys(ADescription);
            
            IWebElement txtDate = ADriver.FindElement(By.Name("date_order"));
            txtDate.Clear();
            txtDate.SendKeys(ADateReleased);

            IWebElement chkReleased = ADriver.FindElement(By.Name("released"));
            if (chkReleased.Selected != AHasBeenReleased)
            {
                chkReleased.SendKeys(" ");
            }
            
            IWebElement form = txtDate.FindElement(By.XPath("../../../../.."));

            if (form.TagName == "form")
            {
                form.Submit();
            }
        }
        
        /// find the version that should be edited
        static private void OpenVersionEdit(IWebDriver ADriver, string AEditProjectURL, string AVersionID)
        {
            ADriver.Navigate().GoToUrl(AEditProjectURL);
            
            ReadOnlyCollection<IWebElement> elements = ADriver.FindElements(By.XPath("//form"));
            foreach (IWebElement form in elements)
            {
                if (form.GetAttribute("action").Contains("manage_proj_ver_edit_page.php?version_id="))
                {
                    if (form.FindElement(By.XPath("../../td[1]")).Text.Trim() == AVersionID)
                    {
                        Console.WriteLine("edit version " + AVersionID);
                        form.Submit();
                        break;
                    }
                }
            }
        }
    
        static private void UpdateVersionsOfProject(IWebDriver ADriver, string AEditProjectURL, 
                                                    string AVersionReleased, string AVersionDev, string AVersionNext)
        {
            DateTime DateReleased = DateTime.Today;
            
            // if the next version already exists, then do not continue on this project
            ADriver.Navigate().GoToUrl(AEditProjectURL);
            
            ReadOnlyCollection<IWebElement> elements = ADriver.FindElements(By.XPath("//form"));
            foreach (IWebElement form in elements)
            {
                if (form.GetAttribute("action").Contains("manage_proj_ver_edit_page.php?version_id="))
                {
                    if (form.FindElement(By.XPath("../../td[1]")).Text.Trim() == AVersionNext)
                    {
                        return;
                    }
                }
            }
            
            // find the version that should be released now
            // edit that version
            // set released flag, set released date
            // save
            OpenVersionEdit(ADriver, AEditProjectURL, AVersionReleased);
            EditVersion(ADriver, AVersionReleased, DateReleased.ToString("yyyy-MM-dd"), "", true);

            // go back
            ADriver.Navigate().GoToUrl(AEditProjectURL);
            // add a new development version
            ADriver.FindElement(By.Name("version")).SendKeys(AVersionDev);
            ADriver.FindElement(By.Name("add_version")).Click();
            OpenVersionEdit(ADriver, AEditProjectURL, AVersionDev);
            EditVersion(ADriver, AVersionDev, DateReleased.ToString("yyyy-MM-dd") + " 00:01:00", "for fixing development bugs", false);
            
            // go back
            ADriver.Navigate().GoToUrl(AEditProjectURL);
            // add a new future release version
            ADriver.FindElement(By.Name("version")).SendKeys(AVersionNext);
            ADriver.FindElement(By.Name("add_version")).Click();
            OpenVersionEdit(ADriver, AEditProjectURL, AVersionNext);
            EditVersion(ADriver, AVersionNext, DateReleased.AddMonths(1).ToString("yyyy-MM-dd"), "next planned release", false);
        }

        static private void SetVersionFixedInForResolvedBug(IWebDriver ADriver, string AEditBugAdvancedURL, string AVersionFixedIn)
        {
            TLogging.Log(AEditBugAdvancedURL);
            ADriver.Navigate().GoToUrl(AEditBugAdvancedURL);

            IWebElement cmbResolution = ADriver.FindElement(By.Name("resolution"));
            ReadOnlyCollection<IWebElement> options = cmbResolution.FindElements(By.TagName("option"));
            foreach (IWebElement option in options) 
            {
                if (option.Text == "fixed" && option.Selected == false)
                {
                    TLogging.Log("*resolution* is not a fix, so we are not setting *version fixed in* for " + AEditBugAdvancedURL);
                    return;
                }
            } 

            IWebElement cmbFixedInVersion = ADriver.FindElement(By.Name("fixed_in_version"));
            options = cmbFixedInVersion.FindElements(By.TagName("option"));

            foreach (IWebElement option in options) 
            {
                if (AVersionFixedIn == option.Text)
                {
                    option.Click();
                    break; 
                }
            } 

            IWebElement form = ((IWebElement)cmbFixedInVersion).FindElement(By.XPath("../../../../.."));

            if (form.TagName == "form")
            {
                form.Submit();
            }
        }

        static void Main(string[] args)
        {
            new TAppSettingsManager(false);

            if (!TAppSettingsManager.HasValue("sf-username"))
            {
                Console.WriteLine("call: MantisUpdateVersions.exe -sf-username:pokorra -sf-pwd:xyz -release-version:0.2.16.0");
                Console.WriteLine("or: MantisUpdateVersions.exe -sf-username:pokorra -sf-pwd:xyz -bug-id:abc,def,ghi -version-fixed-in:\"Alpha 0.2.20\"");
                return;
            }

            string loginURL = TAppSettingsManager.GetValue("login-url", "http://sourceforge.net/account/login.php");
            string mantisURL = TAppSettingsManager.GetValue("mantis-url", "https://sourceforge.net/apps/mantisbt/openpetraorg/");
            
            IWebDriver driver = new FirefoxDriver();
            
            try
            {
                LoginToSourceforge(driver, loginURL, TAppSettingsManager.GetValue("sf-username"), TAppSettingsManager.GetValue("sf-pwd"));

                if (TAppSettingsManager.HasValue("version-fixed-in"))
                {
                    string[] bugids = TAppSettingsManager.GetValue("bug-id").Split(new char[]{','});
                    
                    foreach (string bugid in bugids)
                    {
                        SetVersionFixedInForResolvedBug(
                            driver,
                            mantisURL + "bug_update_advanced_page.php?bug_id=" + bugid,
                            TAppSettingsManager.GetValue("version-fixed-in"));
                    }
                }
                else
                {
                    Version releaseVersion = new Version(TAppSettingsManager.GetValue("release-version"));
                    Version devVersion = new Version(releaseVersion.Major, releaseVersion.Minor, releaseVersion.Build+1, releaseVersion.Revision);
                    Version nextVersion = new Version(releaseVersion.Major, releaseVersion.Minor, releaseVersion.Build+2, releaseVersion.Revision);
                    
                    SortedList<int, string> projectIDs = GetAllProjects(driver, mantisURL + "manage_proj_page.php");
                    
                    foreach(int id in projectIDs.Keys)
                    {
                        Console.WriteLine("project " + projectIDs[id]);
                        UpdateVersionsOfProject(driver,
                                                mantisURL + "manage_proj_edit_page.php?project_id=" + id.ToString(),
                                                "Alpha " + releaseVersion.ToString(3),
                                                "Alpha " + devVersion.ToString(3) + " Dev",
                                                "Alpha " + nextVersion.ToString(3));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                
                StreamWriter sw = new StreamWriter("error.html");
                sw.WriteLine(driver.PageSource.Replace("a0:","").Replace(":a0",""));
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