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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MReporting.Gui.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
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
        public static void NewPartner(IntPtr AParentFormHandle)
        {
            TFrmPartnerEdit frm = new TFrmPartnerEdit(AParentFormHandle);

            frm.SetParameters(TScreenMode.smNew, "FAMILY", -1, -1, "");
            frm.Show();
        }

        /// create a new organisation (eg. supplier)
        public static void NewOrganisation(IntPtr AParentFormHandle)
        {
            TFrmPartnerEdit frm = new TFrmPartnerEdit(AParentFormHandle);

            frm.SetParameters(TScreenMode.smNew, "ORGANISATION", -1, -1, "");
            frm.Show();
        }

        /// create a new person
        public static void NewPerson(IntPtr AParentFormHandle)
        {
            TFrmPartnerEdit frm = new TFrmPartnerEdit(AParentFormHandle);

            frm.SetParameters(TScreenMode.smNew, "PERSON", -1, -1, "");
            frm.Show();
        }

            /// export partners into file
        public static void ExportPartners(IntPtr AParentFormHandle)
        {

            String FileName = TImportExportDialogs.GetExportFilename (Catalog.GetString("Save Partners into File"));
            if (FileName.Length > 0)
            {
                if (FileName.EndsWith("ext"))
                {
                    Int64 APartnerKey = 10000026;
                    Int32 ASiteKey = 0;
                    Int32 ALocationKey = 0;
                    StringCollection ASpecificBuildingInfo = null;
                    String doc = TRemote.MPartner.ImportExport.WebConnectors.GetExtFileHeader ();
                    

                    doc += TRemote.MPartner.ImportExport.WebConnectors.ExportPartnerExt(
                        APartnerKey, ASiteKey, ALocationKey, 
                        ASpecificBuildingInfo);
                        
                    TImportExportDialogs.ExportTofile(doc, FileName);
                
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(TRemote.MPartner.ImportExport.WebConnectors.ExportPartners());
                    TImportExportDialogs.ExportTofile(doc, FileName);
                }
            }
//            TImportExportDialogs.ExportWithDialog(doc, Catalog.GetString("Save Partners into File"));
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