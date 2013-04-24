//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Net;
using Ict.Common;
using Futureware.MantisConnect;
using Futureware.MantisConnect.MantisConnectWebservice;

namespace Ict.Tools.Mantis.UpdateVersion
{
    class UpdateMantisVersion
    {
        static private Session LoginToMantis(string ALoginURL, string AUsername, string APassword)
        {
            Session session = new Session(ALoginURL, AUsername, APassword, null);

            session.Connect();

            return session;
        }

        static private SortedList <int, string>GetAllProjects(Session ASession)
        {
            SortedList <int, string>result = new SortedList <int, string>();
            ProjectData[] projects = ASession.Request.UserGetDetailedAccessibleProjects();

            foreach (ProjectData p in projects)
            {
                result.Add(Convert.ToInt32(p.id), p.name);

                foreach (ProjectData sp in p.subprojects)
                {
                    result.Add(Convert.ToInt32(sp.id), sp.name);
                }
            }

            return result;
        }

        static private void UpdateVersionsOfProject(Session ASession, int AProjectID,
            string AVersionReleased, string AVersionDev, string AVersionNext)
        {
            DateTime DateReleased = DateTime.Today;

            Console.WriteLine(AProjectID.ToString() + " " + AVersionReleased);

            ProjectVersion[] ProjectVersions = ASession.Request.ProjectGetVersions(AProjectID);

            foreach (var element in ProjectVersions)
            {
                if (element.Name == AVersionReleased)
                {
                    element.IsReleased = true;
                    element.DateOrder = DateReleased;
                    element.Description = "";

                    ASession.Request.ProjectVersionUpdate(element);
                }
                else if (element.Name == AVersionDev)
                {
                    AVersionDev = string.Empty;
                }
                else if (element.Name == AVersionNext)
                {
                    AVersionNext = string.Empty;
                }
            }

            // add a new development version
            if (AVersionDev != string.Empty)
            {
                Console.WriteLine("adding version " + AVersionDev);
                ProjectVersion v = new ProjectVersion();
                v.ProjectId = AProjectID;
                v.IsReleased = false;
                v.Name = AVersionDev;
                v.DateOrder = new DateTime(DateReleased.Year, DateReleased.Month, DateReleased.Day, 0, 1, 0);
                v.Description = "for fixing development bugs";
                ASession.Request.ProjectVersionAdd(v);
            }

            // add a new future release version
            if (AVersionNext != string.Empty)
            {
                Console.WriteLine("adding version " + AVersionNext);
                ProjectVersion v = new ProjectVersion();
                v.ProjectId = AProjectID;
                v.IsReleased = false;
                v.Name = AVersionNext;
                v.DateOrder = DateReleased.AddMonths(1);
                v.Description = "next planned release";
                ASession.Request.ProjectVersionAdd(v);
            }
        }

        static private void SetVersionFixedInForResolvedBug(Session ASession, int bugid, string AVersionFixedIn)
        {
            Issue issue = ASession.Request.IssueGet(bugid);

            if (issue.Resolution.Id != 20) // 20 means fixed
            {
                TLogging.Log("*resolution* is not a fix, so we are not setting *version fixed in* for bug " + bugid.ToString());
                return;
            }

            issue.FixedInVersion = AVersionFixedIn;
            ASession.Request.IssueUpdate(issue);
        }

        static void Main(string[] args)
        {
            new TAppSettingsManager(false);

            ServicePointManager.ServerCertificateValidationCallback = delegate {
                return true;
            };

            if (!TAppSettingsManager.HasValue("sf-username"))
            {
                Console.WriteLine("call: MantisUpdateVersions.exe -sf-username:pokorra -sf-pwd:xyz -release-version:0.2.16.0");
                Console.WriteLine(
                    "or: MantisUpdateVersions.exe -sf-username:pokorra -sf-pwd:xyz -bug-id:abc,def,ghi -version-fixed-in:\"Alpha 0.2.20\"");
                return;
            }

            string mantisURL = TAppSettingsManager.GetValue("mantis-url", "https://tracker.openpetra.org/api/soap/mantisconnect.php");

            try
            {
                Session session = LoginToMantis(mantisURL, TAppSettingsManager.GetValue("sf-username"), TAppSettingsManager.GetValue("sf-pwd"));

                if (TAppSettingsManager.HasValue("version-fixed-in"))
                {
                    string[] bugids = TAppSettingsManager.GetValue("bug-id").Split(new char[] { ',' });

                    foreach (string bugid in bugids)
                    {
                        SetVersionFixedInForResolvedBug(
                            session,
                            Convert.ToInt32(bugid),
                            TAppSettingsManager.GetValue("version-fixed-in"));
                    }
                }
                else
                {
                    Version releaseVersion = new Version(TAppSettingsManager.GetValue("release-version"));
                    Version devVersion = new Version(releaseVersion.Major, releaseVersion.Minor, releaseVersion.Build + 1, releaseVersion.Revision);
                    Version nextVersion = new Version(releaseVersion.Major, releaseVersion.Minor, releaseVersion.Build + 2, releaseVersion.Revision);

                    SortedList <int, string>projectIDs = GetAllProjects(session);

                    foreach (int id in projectIDs.Keys)
                    {
                        Console.WriteLine("project " + projectIDs[id]);
                        UpdateVersionsOfProject(session,
                            id,
                            "Alpha " + releaseVersion.ToString(3),
                            "Alpha " + devVersion.ToString(3) + " Dev",
                            "Alpha " + nextVersion.ToString(3));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}