//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Xml;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MReporting.Gui.MPartner;
//using Ict.Petra.Client.MReporting.Gui.MPersonnel;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using System.Collections.Specialized;
using Ict.Petra.Shared.Interfaces.MPartner;

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

            System.Int64 FamilyKey = GetLastUsedFamilyKey();
            TLocationPK LocationSiteKey = TRemote.MPartner.Partner.WebConnectors.DetermineBestAddress(FamilyKey);

            frm.SetParameters(TScreenMode.smNew, "PERSON", -1, -1, "", "", false,
                FamilyKey, LocationSiteKey.LocationKey, LocationSiteKey.SiteKey);

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
                    String doc = TRemote.MPartner.ImportExport.WebConnectors.ExportAllPartnersExt();
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
        /// Makes a server call to get the key of the last used family
        /// <returns>FamilyKey of the last accessed family</returns>
        /// </summary>
        private static System.Int64 GetLastUsedFamilyKey()
        {
            bool LastFamilyFound = false;

            System.Int64 FamilyKey = 0000000000;
            Dictionary <long, string>RecentlyUsedPartners;
            ArrayList PartnerClasses = new ArrayList();

            PartnerClasses.Add("*");

            int MaxPartnersCount = 7;
            TServerLookup.TMPartner.GetRecentlyUsedPartners(MaxPartnersCount, PartnerClasses, out RecentlyUsedPartners);

            foreach (KeyValuePair <long, string>CurrentEntry in RecentlyUsedPartners)
            {
                //search for the last FamilyKey
                //assign it only to FamilyKey if there hasn't been yet found another Family

                //fe. CurrentEntry.Key= 43005007 CurrentEntry.Value= Test, alex (type PERSON)
                //TLogging.Log("CurrentEntry.Key= " + CurrentEntry.Key + " CurrentEntry.Value= " + CurrentEntry.Value);
                if (CurrentEntry.Value.Contains("FAMILY") && !LastFamilyFound)
                {
                    FamilyKey = CurrentEntry.Key;
                    LastFamilyFound = true;
                }
            }

            return FamilyKey;
        }
    }
}