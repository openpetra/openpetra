//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb, andreww
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
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_Contacts
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        #region Public Methods

        /// <summary>used for passing through the Clientside Proxy for the UIConnector</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
        }

        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// This Procedure will get called from the SaveChanges procedure before it
        /// actually performs any saving operation.
        /// </summary>
        /// <param name="sender">The Object that throws this Event</param>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        private void DataSavingStarted(System.Object sender, System.EventArgs e)
        {
        }

        public void SpecialInitUserControl()
        {
            LoadDataOnDemand();

            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Category", FMainDS.PPartnerInterest.ColumnInterestCategory);
            grdDetails.AddTextColumn("Interest", FMainDS.PPartnerInterest.ColumnInterest);
            grdDetails.AddTextColumn("Country", FMainDS.PPartnerInterest.ColumnCountry);
            grdDetails.AddPartnerKeyColumn("Field", FMainDS.PPartnerInterest.ColumnFieldKey);
            grdDetails.AddTextColumn("Level", FMainDS.PPartnerInterest.ColumnLevel);
            grdDetails.AddTextColumn("Comment", FMainDS.PPartnerInterest.ColumnComment);

            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpInterests));

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);

            if (grdDetails.Rows.Count > 1)
            {
                grdDetails.SelectRowInGrid(1);
                ShowDetails(1); // do this as for some reason details are not automatically show here at the moment
            }
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        #region Private Methods


        private void InitializeManualCode()
        {
            if (!FMainDS.Tables.Contains(Ict.Petra.Shared.MPartner.Mailroom.Data.PPartnerContactTable.GetTableName()))
            {
                FMainDS.Merge(TRemote.MPartner.Partner.WebConnectors.FindContactsForPartner(FMainDS.PPartner[0].PartnerKey));

                FMainDS.PPartnerContact.DefaultView.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.PPartnerContact.DefaultView);
            }

            FMainDS.InitVars();
        }

        private Boolean LoadDataOnDemand()
        {
            return false;
        }

        private void ShowDataManual()
        {
        }

        private void ShowDetailsManual(PPartnerInterestRow ARow)
        {
        }

        private void GetDetailDataFromControlsManual(PPartnerInterestRow ARow)
        {
        }

        private void FilterInterestCombo(object sender, EventArgs e)
        {
        }

        private void NewRecord(System.Object sender, EventArgs e)
        {
        }

        private void NewRowManual(ref PartnerEditTDSPPartnerInterestRow ARow)
        {
        }

        private bool PreDeleteManual(PartnerEditTDSPPartnerInterestRow ARowToDelete, ref string ADeletionQuestion)
        {
            return true;
        }

        private void PostDeleteManual(PartnerEditTDSPPartnerInterestRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
        }

        private void DoRecalculateScreenParts()
        {
        }

        private void ValidateDataDetailsManual(PPartnerInterestRow ARow)
        {
        }

        private void NewRowManual(ref PPartnerContactRow foo)
        {
        }

        private void ShowDetailsManual(PPartnerContactRow ARow)
        {   
        }

        private bool PreDeleteManual(PPartnerContactRow pPartnerContactRow, ref string ADeletionQuestion)
        {
            return true;
        }

        private void GetDetailDataFromControlsManual(PPartnerContactRow ARow)
        {   
        }

        private void PostDeleteManual(PPartnerContactRow pPartnerContactRow, bool AAllowDeletion, bool ADeletionPerformed, string ACompletionMessage)
        {   
        }

        private void ValidateDataDetailsManual(PPartnerContactRow FPreviouslySelectedDetailRow)
        {
        }
        #endregion
    }
}