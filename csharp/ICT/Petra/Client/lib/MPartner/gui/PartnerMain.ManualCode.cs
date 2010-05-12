//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2010 by OM International
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
using System.Xml;
using System.Windows.Forms;
using Mono.Unix;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MReporting.Gui.MPartner;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// the manually written part of TFrmPartnerMain
    /// </summary>
    public partial class TFrmPartnerMain
    {
        /// <summary>
        /// create a new partner (default to family ie. household)
        /// </summary>
        public static void NewPartner(IntPtr AParentFormHandle)
        {
            TPartnerEditDSWinForm frm = new TPartnerEditDSWinForm(AParentFormHandle);

            frm.SetParameters(TScreenMode.smNew, "FAMILY", -1, -1, "");
            frm.Show();
        }

        /// create a new organisation (eg. supplier)
        public static void NewOrganisation(IntPtr AParentFormHandle)
        {
            TPartnerEditDSWinForm frm = new TPartnerEditDSWinForm(AParentFormHandle);

            frm.SetParameters(TScreenMode.smNew, "ORGANISATION", -1, -1, "");
            frm.Show();
        }

        /// create a new person
        public static void NewPerson(IntPtr AParentFormHandle)
        {
            TPartnerEditDSWinForm frm = new TPartnerEditDSWinForm(AParentFormHandle);

            frm.SetParameters(TScreenMode.smNew, "PERSON", -1, -1, "");
            frm.Show();
        }

        /// import partners from file
        public static void ImportPartners(IntPtr AParentFormHandle)
        {
            XmlDocument doc = TImportExportDialogs.ImportWithDialog(Catalog.GetString("Load Partners from File"));

            if (doc == null)
            {
                return;
            }

            TVerificationResultCollection VerificationResult;

            if (!TRemote.MPartner.ImportExport.WebConnectors.ImportPartners(TXMLParser.XmlToString(doc), out VerificationResult))
            {
                string ErrorMessages = String.Empty;

                if (VerificationResult != null)
                {
                    foreach (TVerificationResult verif in VerificationResult)
                    {
                        ErrorMessages += "[" + verif.ResultContext + "] " +
                                         verif.ResultTextCaption + ": " +
                                         verif.ResultText + Environment.NewLine;
                    }
                }

                MessageBox.Show(ErrorMessages, Catalog.GetString("Import of partners failed!"));
            }
            else
            {
                MessageBox.Show(Catalog.GetString("Partner(s) have been imported successfully"), Catalog.GetString(
                        "Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// export partners into file
        public static void ExportPartners(IntPtr AParentFormHandle)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(TRemote.MPartner.ImportExport.WebConnectors.ExportPartners());
            TImportExportDialogs.ExportWithDialog(doc, Catalog.GetString("Save Partners into File"));
        }

        /// <summary>
        /// open partner find screen
        /// </summary>
        public static void FindPartner(IntPtr AParentFormHandle)
        {
            TPartnerFindScreen frm = new TPartnerFindScreen(AParentFormHandle);

            frm.SetParameters(false, -1);
            frm.Show();
        }
    }
}