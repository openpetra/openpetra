//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MConference.Gui
{
    /// <summary>
    /// methods for deletion of a conference
    /// </summary>
    public class TDeleteConference
    {
        private static void ProcessDeletion(Form AMainWindow, Int64 AConferenceKey, string AConferenceName)
        {
            TVerificationResultCollection VerificationResult;
            MethodInfo method;

            if (!TRemote.MConference.Conference.WebConnectors.DeleteConference(AConferenceKey, out VerificationResult))
            {
                MessageBox.Show(
                    string.Format(Catalog.GetString("Deletion of Conference '{0}' failed"), AConferenceName) + "\r\n\r\n" +
                    VerificationResult.BuildVerificationResultString(),
                    Catalog.GetString("Deletion failed"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(
                    string.Format(Catalog.GetString("Conference '{0}' has been deleted"), AConferenceName),
                    Catalog.GetString("Deletion successful"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            method = AMainWindow.GetType().GetMethod("ShowCurrentConferenceInfoInStatusBar");

            if (method != null)
            {
                method.Invoke(AMainWindow, new object[] { });
            }
        }

        /// delete the complete conference including all conference data
        public static void DeleteThisConference(Form AMainWindow)
        {
            // Get conference key
            long ConferenceKey = TUserDefaults.GetInt64Default("LASTCONFERENCEWORKEDWITH");

            if (ConferenceKey == 0)
            {
                MessageBox.Show(Catalog.GetString("There is no conference selected."),
                    Catalog.GetString("Delete Conference"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Get conference name
                string ConferenceName;
                TPartnerClass PartnerClass;
                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(ConferenceKey, out ConferenceName, out PartnerClass);

                DeleteConference(AMainWindow, ConferenceKey, ConferenceName);
            }
        }

        /// delete the complete conference including all conference data
        public static void DeleteConference(Form AMainWindow, Int64 AConferenceKey, string AConferenceName)
        {
            if (MessageBox.Show(Catalog.GetString("Please save a backup of your database first!!!") + Environment.NewLine +
                    string.Format(Catalog.GetString("Do you REALLY want to delete conference '{0}'?"),
                        AConferenceName),
                    Catalog.GetString("Delete Conference"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2
                    ) == DialogResult.Yes)
            {
                Thread t = new Thread(() => ProcessDeletion(AMainWindow, AConferenceKey, AConferenceName));

                using (TProgressDialog dialog = new TProgressDialog(t))
                {
                    dialog.ShowDialog();
                }

                if (AConferenceKey == TUserDefaults.GetInt64Default("LASTCONFERENCEWORKEDWITH"))
                {
                    // update user defaults table
                    TUserDefaults.SetDefault(TUserDefaults.CONFERENCE_LASTCONFERENCEWORKEDWITH, 0);

                    // reload navigation
                    PropertyInfo CurrentConferenceProperty = AMainWindow.GetType().GetProperty("SelectedConferenceKey");
                    CurrentConferenceProperty.SetValue(AMainWindow, 0, null);

                    MethodInfo method = AMainWindow.GetType().GetMethod("LoadNavigationUI");

                    if (method != null)
                    {
                        method.Invoke(AMainWindow, new object[] { false });
                        method = AMainWindow.GetType().GetMethod("SelectConferenceFolder");
                        method.Invoke(AMainWindow, new object[] { });
                    }
                }
            }
        }
    }
}