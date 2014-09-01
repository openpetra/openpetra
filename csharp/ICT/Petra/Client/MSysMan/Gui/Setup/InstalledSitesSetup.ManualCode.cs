//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2014 by OM International
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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MSysMan.Gui.Setup
{
    public partial class TFrmInstalledSitesSetup
    {
        List<Int64> OriginallyCheckedSites;
        DataTable AvailableSitesTable;
        DataTable CmbDataTable;
        string CmbDisplayMember = "Display";
        string CmbValueMember = "Value";

        /// <summary>
        /// todoComment
        /// </summary>
        public void FileSave(System.Object sender, EventArgs e)
        {
            SaveChanges();
        }
        
        /// <summary>
        /// save the changes
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            Boolean Result = false;
            String CheckedStringList = clbSites.GetCheckedStringList();
            String[] SiteKeyArray = CheckedStringList.Split(',');
            Int32 Counter = 0;
            List<Int64> AddedSiteKeyList = new List<Int64>();
            List<Int64> RemovedSiteKeyList = new List<Int64>();
            Int64 RemovedSiteKey;
            String RemovedSiteName;
            Boolean AnySiteRemoved = false;
            String UserMessage = Catalog.GetString("Are you sure you want to remove access to following sites? You will not be able to create Partner Keys for them any longer! \r\n");

            TVerificationResultCollection VerificationResultCollection = new TVerificationResultCollection();
            TVerificationResult VerificationResult = TStringChecks.StringMustNotBeEmpty(cmbDefaultSite.Text,
                lblDefaultSite.Text);

            // Handle addition/removal to/from TVerificationResultCollection
            VerificationResultCollection.Auto_Add_Or_AddOrRemove(null, VerificationResult, null);

            if (!TDataValidation.ProcessAnyDataValidationErrors(false, VerificationResultCollection, this.GetType()))
            {
                return false;
            }

            // save site keys selected for possible use
            for (Counter = 0; Counter < SiteKeyArray.Length; Counter++)
            {
                AddedSiteKeyList.Add(Convert.ToInt64(SiteKeyArray[Counter]));
            }

            // create list of site keys that have been removed and double check with user
            foreach (DataRow SiteRow in AvailableSitesTable.Rows)
            {
                if (!Convert.ToBoolean(SiteRow[SharedConstants.SYSMAN_AVAILABLE_SITES_COLUMN_IS_PARTNER_LEDGER]))
                {
                    // check if previously checked site is now no longer checked
                    RemovedSiteKey = Convert.ToInt64(SiteRow[PUnitTable.GetPartnerKeyDBName()]);
                    if (OriginallyCheckedSites.Contains(RemovedSiteKey))
                    {
                        AnySiteRemoved = true;
                        RemovedSiteKeyList.Add(RemovedSiteKey);
                        RemovedSiteName = SiteRow[PUnitTable.GetUnitNameDBName()].ToString();
                        UserMessage += "\r\n" + String.Format("{0:0000000000}", RemovedSiteKey) + " - " + RemovedSiteName;
                    }
                }
            }

            if (AnySiteRemoved)
            {
                if (MessageBox.Show(UserMessage, Catalog.GetString("Remove access to Sites"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return false;
                }
            }


            // save default site key
            if (cmbDefaultSite.SelectedValue != null)
            {
                TSystemDefaults.SetSystemDefault(SharedConstants.SYSDEFAULT_SITEKEY, cmbDefaultSite.SelectedValue.ToString());
            }

            Result = TRemote.MSysMan.WebConnectors.SaveSiteKeys(AddedSiteKeyList, RemovedSiteKeyList);

            if (Result)
            {
                // reload data from server here so the list is sorted by check box value first and then site name (consider doing this
                // on client in the future if performance issues)
                LoadSitesData();

                FPetraUtilsObject.DisableSaveButton();

                // We don't have unsaved changes anymore
                FPetraUtilsObject.HasChanges = false;
            }

            return Result;
        }

        /// <summary>
        /// only run this code once during activation
        /// </summary>
        private void RunOnceOnActivationManual()
        {
            // enable autofind in list for first character (so the user can press character to find list entry)
            this.clbSites.AutoFindColumn = ((Int16)(1));
            this.clbSites.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;

            clbSites.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            // populate list with data to be loaded
            this.LoadSitesData();

            CmbDataTable = new DataTable();
            CmbDataTable.Columns.Add(CmbDisplayMember);
            CmbDataTable.Columns.Add(CmbValueMember);

            cmbDefaultSite.DisplayMember = CmbDisplayMember;
            cmbDefaultSite.ValueMember = CmbValueMember;

            cmbDefaultSite.DataSource = CmbDataTable.DefaultView;

            UpdateDefaultCombobox();

            // reset after initialization
            FPetraUtilsObject.DisableSaveButton();
            FPetraUtilsObject.HasChanges = false;
        }

        /// <summary>
        /// load data for sites grid from server
        /// </summary>
        private void LoadSitesData()
        {
            string CheckedMember = SharedConstants.SYSMAN_AVAILABLE_SITES_COLUMN_IS_PARTNER_LEDGER;
            string SiteKey = PUnitTable.GetPartnerKeyDBName();
            string SiteShortName = PUnitTable.GetUnitNameDBName();

            AvailableSitesTable = TRemote.MSysMan.WebConnectors.GetAvailableSites();

            clbSites.Columns.Clear();
            clbSites.AddCheckBoxColumn("", AvailableSitesTable.Columns[CheckedMember], 17, false);
            clbSites.AddPartnerKeyColumn(Catalog.GetString("Partner Key"), AvailableSitesTable.Columns[SiteKey], 100);
            clbSites.AddTextColumn(Catalog.GetString("Site Name"), AvailableSitesTable.Columns[SiteShortName], 400);

            clbSites.DataBindGrid(AvailableSitesTable, "", CheckedMember, SiteKey, false, true, false);

            // create list of site keys that have been checked when screen opens
            OriginallyCheckedSites = new List<Int64>();
            OriginallyCheckedSites.Clear();
            foreach (DataRow SiteRow in AvailableSitesTable.Rows)
            {
                if (Convert.ToBoolean(SiteRow[SharedConstants.SYSMAN_AVAILABLE_SITES_COLUMN_IS_PARTNER_LEDGER]))
                {
                    OriginallyCheckedSites.Add(Convert.ToInt64(SiteRow[PUnitTable.GetPartnerKeyDBName()]));
                }
            }
        }

        private void ClbSites_ValueChanged(System.Object sender, System.EventArgs e)
        {
            UpdateDefaultCombobox();
        }

        private void UpdateDefaultCombobox()
        {
            // update default combobox
            String SelectedSites = clbSites.GetCheckedStringList();
            Int64 SelectedDefaultSite = 0;
            String SiteName;

            // remember original selection for default site
            if (cmbDefaultSite.SelectedValue != null)
            {
                SelectedDefaultSite = Convert.ToInt64(cmbDefaultSite.SelectedValue);
            }

            String[] SiteKeyArray = SelectedSites.Split(',');
            Int32 Counter = 0;
            List<Int64> SiteKeyList = new List<Int64>();

            // initialize data table for combobox
            CmbDataTable.Clear();
            for (Counter = 0; Counter < SiteKeyArray.Length; Counter++)
            {
                if (SiteKeyArray[Counter] != "")
                {
                    // find site name for given key ("Find" does not work on DataTable as it does not have primary key)
                    SiteName = "";
                    foreach (DataRow SiteRow in AvailableSitesTable.Rows)
                    {
                        if (Convert.ToInt64(SiteRow[PUnitTable.GetPartnerKeyDBName()]) == Convert.ToInt64(SiteKeyArray[Counter]))
                        {
                            SiteName = SiteRow[PUnitTable.GetUnitNameDBName()].ToString();
                            break;
                        }
                    }
                    CmbDataTable.Rows.Add(string.Format("{0:0000000000}", Convert.ToInt64(SiteKeyArray[Counter])) + " - " + SiteName, Convert.ToInt64(SiteKeyArray[Counter]));
                }
            }

            // now reselected the original selected default site
            if (SelectedDefaultSite > 0)
            {
                cmbDefaultSite.SelectedValue = SelectedDefaultSite;
            }

        }

    }
}