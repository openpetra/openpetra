//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    /// <summary>
    /// Description of TFrmSelectCampaignOption.ManualCode.
    /// </summary>
    public partial class TFrmSelectCampaignOption
    {
        private static Int64 FUnitKey;
        private static PUnitTable FUnitTable;

        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitUserControlsManually()
        {
            FUnitTable = TRemote.MConference.WebConnectors.GetCampaignOptions(FUnitKey);
            FUnitTable.Columns.Add("Is Selected", Type.GetType("System.Boolean"));

            for (int Counter = 0; Counter < FUnitTable.Rows.Count; ++Counter)
            {
                FUnitTable.Rows[Counter]["Is Selected"] = false;
            }

            grdCampaignOption.AddCheckBoxColumn("", FUnitTable.Columns["Is Selected"]);
            grdCampaignOption.AddTextColumn("Campaign Code", FUnitTable.ColumnXyzTbdCode);
            grdCampaignOption.AddTextColumn("Unit Name", FUnitTable.ColumnUnitName);
            grdCampaignOption.AddTextColumn("Unit Key", FUnitTable.ColumnPartnerKey);

            FUnitTable.DefaultView.AllowNew = false;
            FUnitTable.DefaultView.AllowEdit = true;
            FUnitTable.DefaultView.AllowDelete = false;

            grdCampaignOption.DataSource = new DevAge.ComponentModel.BoundDataView(FUnitTable.DefaultView);
        }

        private void grdCampaignOptionDoubleClick(System.Object sender, EventArgs e)
        {
            AcceptSelection(sender, e);
        }

        private void AcceptSelection(System.Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelClick(System.Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SelectCampaignOptions(Boolean AValue)
        {
            foreach (DataRow Row in FUnitTable.Rows)
            {
                Row["Is Selected"] = AValue;
            }
        }

        private void SelectAll(System.Object sender, EventArgs e)
        {
            SelectCampaignOptions(true);
        }

        private void DeselectAll(System.Object sender, EventArgs e)
        {
            SelectCampaignOptions(false);
        }

        /// <summary>
        /// Returns the number of campaign options available for this conference
        /// </summary>
        /// <returns></returns>
        public static int GetCampaignOptionsCount()
        {
            return FUnitTable.Rows.Count;
        }

        /// <summary>
        /// Returns all or only the selected campaign options
        /// </summary>
        /// <param name="ASelection">List with Unit key and Unit name of the selected campaign options</param>
        /// <param name="AAllOptionsInTable">True if all campaign options should be returned</param>
        public static void GetSelectedCampaignOptions(ref List <KeyValuePair <long, String>>ASelection,
            bool AAllOptionsInTable)
        {
            foreach (DataRow Row in FUnitTable.Rows)
            {
                if (AAllOptionsInTable || (bool)Row["Is Selected"])
                {
                    long UnitKey = (long)Row[PUnitTable.GetPartnerKeyDBName()];
                    String UnitName = (String)Row[PUnitTable.GetUnitNameDBName()];

                    ASelection.Add(new KeyValuePair <long, String>(UnitKey, UnitName));
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="EventKey">The UnitKey of the conference from which the campaign options are displayed</param>
        /// <param name="AOwner">The parent form</param>
        /// <param name="AShowOnlyIfSeveralOptions">Inidcator if the dialog is to be shown if there
        /// is only one campaign option. True: dialog is shown only if there are more than one option.
        /// False: dialog is shown always, indipendent of how many campaign options there are</param>
        /// <param name="ConferenceList">The selected campaign options. It holds the Unit keys
        /// and the unit names.</param>
        /// <returns></returns>
        public static DialogResult OpenSelectCampaignOptionDialog(Int64 EventKey, IWin32Window AOwner,
            bool AShowOnlyIfSeveralOptions, out List <KeyValuePair <long, string>>ConferenceList)
        {
            TFrmSelectCampaignOption SelectCampaignOptionDialog;

            FUnitKey = EventKey;
            DialogResult DlgResult = DialogResult.Cancel;

            ConferenceList = new List <KeyValuePair <long, String>>();
// TODO check the handle...
            SelectCampaignOptionDialog = new TFrmSelectCampaignOption(AOwner.Handle);

            if ((AShowOnlyIfSeveralOptions && (GetCampaignOptionsCount() > 1))
                || !AShowOnlyIfSeveralOptions)
            {
                DlgResult = SelectCampaignOptionDialog.ShowDialog(AOwner);

                if (DlgResult == DialogResult.OK)
                {
                    GetSelectedCampaignOptions(ref ConferenceList, false);
                }
            }

            if (AShowOnlyIfSeveralOptions && (GetCampaignOptionsCount() == 1))
            {
                // We don't show the dialog and we have only one campaign option, then use
                // this campaign option as result
                GetSelectedCampaignOptions(ref ConferenceList, true);
                DlgResult = DialogResult.OK;
            }

            return DlgResult;
        }
    }
}