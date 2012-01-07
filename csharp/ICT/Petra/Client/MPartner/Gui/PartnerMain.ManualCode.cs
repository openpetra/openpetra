//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Xml;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MReporting.Gui.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.MPartner.Gui.Extracts;
using System.Collections.Specialized;

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
        public static void NewPartner(Form AParentForm)
        {
            TFrmPartnerEdit frm = new TFrmPartnerEdit(AParentForm);

            frm.SetParameters(TScreenMode.smNew, "FAMILY", -1, -1, "");
            frm.Show();
        }

        /// create a new organisation (eg. supplier)
        public static void NewOrganisation(Form AParentForm)
        {
            TFrmPartnerEdit frm = new TFrmPartnerEdit(AParentForm);

            frm.SetParameters(TScreenMode.smNew, "ORGANISATION", -1, -1, "");
            frm.Show();
        }

        /// create a new person
        public static void NewPerson(Form AParentForm)
        {
            TFrmPartnerEdit frm = new TFrmPartnerEdit(AParentForm);

            frm.SetParameters(TScreenMode.smNew, "PERSON", -1, -1, "");
            frm.Show();
        }

        /// export partners into file
        public static void ExportPartners(Form AParentForm)
        {
            String FileName = TImportExportDialogs.GetExportFilename(Catalog.GetString("Save Partners into File"));

            if (FileName.Length > 0)
            {
                if (FileName.EndsWith("ext"))
                {
                    Int64 PartnerKey = 10000026;
                    StringCollection ASpecificBuildingInfo = null;
                    String doc = TRemote.MPartner.ImportExport.WebConnectors.GetExtFileHeader();
                    Int32 SiteKey = 0;
                    Int32 LocationKey = 0;

                    doc += TRemote.MPartner.ImportExport.WebConnectors.ExportPartnerExt(
                        PartnerKey, SiteKey, LocationKey, false, ASpecificBuildingInfo);

                    doc += TRemote.MPartner.ImportExport.WebConnectors.GetExtFileFooter();
                    TImportExportDialogs.ExportTofile(doc, FileName);
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(TRemote.MPartner.ImportExport.WebConnectors.ExportPartners());
                    TImportExportDialogs.ExportTofile(doc, FileName);
                }
            }
        }

        /// <summary>
        /// open partner find screen
        /// </summary>
        public static void FindPartner(Form AParentForm)
        {
            TPartnerFindScreen frm = new TPartnerFindScreen(AParentForm);

            frm.SetParameters(false, -1);
            frm.Show();
        }
        
        /// <summary>
        /// open screen to create Publication Extract
        /// </summary>
        public static void PublicationExtract(Form AParentForm)
        {
            TFrmPartnerBySubscription frm = new TFrmPartnerBySubscription(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// open screen to create "Partner by City" Extract
        /// </summary>
        public static void PartnerByCityExtract(Form AParentForm)
        {
            TFrmPartnerByCityExtract frm = new TFrmPartnerByCityExtract(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }
    }
}