//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       andreww
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
using Ict.Petra.Client.MCommon;
using Ict.Common.Data;
using Ict.Petra.Client.CommonForms;

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

        /// <summary>
        /// Selects the given contact log.
        /// </summary>
        /// <param name="AContactLogID">Contact Log identifier.</param>
        public void SelectContactLogID(string AContactLogID)
        {
            foreach (DataRowView RowView in FMainDS.PContactLog.DefaultView)
            {
                if (RowView[PContactLogTable.GetContactLogIdDBName()].ToString() == AContactLogID)
                {
                    grdDetails.SelectRowInGrid(grdDetails.Rows.DataSourceRowToIndex(RowView) + 1);
                    return;
                }
            }
        }

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;
        /// <summary>
        /// This Method is needed for UserControls who get dynamically loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        #region Private Methods
        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        private void InitializeManualCode()
        {
            if (!FMainDS.Tables.Contains(PContactLogTable.GetTableName()))
            {
                FMainDS.Merge(TRemote.MPartner.Partner.WebConnectors.GetPartnerContactLogData(FMainDS.PPartner[0].PartnerKey));
                FMainDS.PContactLog.DefaultView.AllowNew = false;
            }

            FMainDS.InitVars();

            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpContacts));
            //Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);
            ucoDetails.SpecialInitUserControl();
        }

        private void ShowDataManual()
        {
            grdDetails.AutoResizeGrid();
        }

        private void NewRecord(object sender, EventArgs e)
        {
            if (CreateNewPContactLog())
            {
                ucoDetails.Contactor = UserInfo.GUserInfo.UserID;
                ucoDetails.Focus();
            }
        }

        private void NewRowManual(ref PContactLogRow ARow)
        {
            ARow.ContactLogId = TRemote.MCommon.WebConnectors.GetNextSequence(TSequenceNames.seq_contact);

            PPartnerContactRow PartnerContact = FMainDS.PPartnerContact.NewRowTyped(true);
            PartnerContact.ContactLogId = ARow.ContactLogId;
            PartnerContact.PartnerKey = ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey;
            FMainDS.PPartnerContact.Rows.Add(PartnerContact);

            RecalculateTabHeaderCounter();
        }

        /// <summary>
        /// This Procedure will get called from the SaveChanges procedure before it
        /// actually performs any saving operation.
        /// </summary>
        private void DataSavingStarted(System.Object sender, System.EventArgs e)
        {
        }

        private void ShowDetailsManual(PContactLogRow ARow)
        {
            if (ARow != null)
            {
                ucoDetails.ShowDetails(ARow);

                if (TRemote.MPartner.Partner.WebConnectors.IsContactLogAssociatedWithMoreThanOnePartner((long)ARow.ContactLogId))
                {
                    lblRelatedLogs.Text = Catalog.GetString(
                        "Note that this Contact Log associated with more than one Partner.  Changes here will affect all Partners using this Contact Log.");
                }
                else
                {
                    lblRelatedLogs.Text = Catalog.GetString("Note that this Contact Log is only associated with this Partner.");
                }
            }
        }

        private void DeleteRecord(object sender, EventArgs e)
        {
            DeletePContactLog();
        }

        private bool PreDeleteManual(PContactLogRow pContactLogRow, ref string ADeletionQuestion)
        {
            ADeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");

            if (TRemote.MPartner.Partner.WebConnectors.IsContactLogAssociatedWithMoreThanOnePartner((long)pContactLogRow.ContactLogId))
            {
                ADeletionQuestion = Catalog.GetString(
                    Environment.NewLine + "Other Partners share this contact log." +
                    Environment.NewLine + "Deleting this record will not affect other Partners" +
                    Environment.NewLine + "To delete this contact for everyone, use \"Find and Delete Contacts\"");
            }

            ADeletionQuestion += String.Format("{0}{0}({1} {2})",
                Environment.NewLine, ucoDetails.ContactDate, ucoDetails.ContactCode);

            return true;
        }

        private bool DeleteRowManual(PContactLogRow ARowToDelete, ref String ACompletionMessage)
        {
            foreach (DataRowView ContactLogRow in grdDetails.SelectedDataRows)
            {
                DataView PartnerContactLogs = new DataView(FMainDS.PPartnerContact);
                PartnerContactLogs.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                    PPartnerContactTable.GetPartnerKeyDBName(),
                    FMainDS.PPartner[0].PartnerKey,
                    PPartnerContactTable.GetContactLogIdDBName(),
                    ContactLogRow.Row[PContactLogTable.ColumnContactLogIdId]);

                // Delete the PartnerContact records
                foreach (DataRowView row in PartnerContactLogs)
                {
                    row.Delete();
                }

                // Actually delete the ContactLog if it's the last
                if (!TRemote.MPartner.Partner.WebConnectors.IsContactLogAssociatedWithMoreThanOnePartner((long)ContactLogRow.Row[PContactLogTable.
                                                                                                                                 ColumnContactLogIdId
                        ]))
                {
                    // make sure to delete any contact attributes existing for this contact
                    DataView PartnerContactAttribute = new DataView(FMainDS.PPartnerContactAttribute);
                    PartnerContactAttribute.RowFilter = String.Format("{0} = {1}",
                        PPartnerContactAttributeTable.GetContactIdDBName(),
                        ContactLogRow.Row[PContactLogTable.ColumnContactLogIdId]);

                    // Delete the Contact Attribute records
                    foreach (DataRowView attributeRow in PartnerContactAttribute)
                    {
                        attributeRow.Delete();
                    }

                    ContactLogRow.Row.Delete();
                }
            }

            grdDetails.Refresh();

            return true;
        }

        private void PostDeleteManual(PContactLogRow pContactLogRow, bool AAllowDeletion, bool ADeletionPerformed, string ACompletionMessage)
        {
            if (ADeletionPerformed)
            {
                RecalculateTabHeaderCounter();
            }
        }

        private void GetDetailDataFromControlsManual(PContactLogRow ARow)
        {
            ucoDetails.GetDetails(ARow);
        }

        private void ValidateDataDetailsManual(PContactLogRow ARow)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private void RecalculateTabHeaderCounter()
        {
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            /* Fire OnRecalculateScreenParts event to update the Tab Counters */
            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }

        #endregion
    }
}