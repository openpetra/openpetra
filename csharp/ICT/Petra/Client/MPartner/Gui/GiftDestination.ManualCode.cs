//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.IO;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Printing;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MPartner.Verification;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TFrmGiftDestination
    {
        private Int64 FPartnerKey;

        /// constructor (use this one!)
        public TFrmGiftDestination(Form AParentForm, long APartnerKey) : base()
        {
            FPartnerKey = APartnerKey;

            TSearchCriteria[] Search = new TSearchCriteria[1];
            Search[0] = new TSearchCriteria(PPartnerGiftDestinationTable.GetPartnerKeyDBName(), FPartnerKey);

            Initialize(AParentForm, Search);
        }

        private void InitializeManualCode()
        {
            txtPartnerKey.Text = FPartnerKey.ToString();

            // display the partner's name
            PPartnerTable Table = TRemote.MPartner.Partner.WebConnectors.GetPartnerDetails(FPartnerKey, false, false, false).PPartner;

            if (Table != null)
            {
                PPartnerRow Row = Table[0];
                txtName.Text = Row.PartnerShortName;
            }

            SetupGrid();

            // add event for when 'save' is clicked and an event for when the data is successfully saved
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_BeforeDataSave);
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);
        }

        private void SetupGrid()
        {
            // manually add a forth column which displays the fields partner short name
            DataColumn FieldName = new DataColumn("FieldName", Type.GetType("System.String"));

            FMainDS.PPartnerGiftDestination.Columns.Add(FieldName);

            grdDetails.Columns.Clear();
            grdDetails.AddDateColumn("Date Effective From", FMainDS.PPartnerGiftDestination.ColumnDateEffective);
            grdDetails.AddDateColumn("Date of Expiry", FMainDS.PPartnerGiftDestination.ColumnDateExpires);
            grdDetails.AddPartnerKeyColumn("Field Key", FMainDS.PPartnerGiftDestination.ColumnFieldKey);
            grdDetails.AddPartnerKeyColumn("Field Name", FieldName);

            foreach (DataRow Row in FMainDS.PPartnerGiftDestination.Rows)
            {
                string PartnerShortName;
                TPartnerClass PartnerClass;

                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(
                    Convert.ToInt64(Row[PPartnerGiftDestinationTable.ColumnFieldKeyId]), out PartnerShortName, out PartnerClass);
                Row["FieldName"] = PartnerShortName;
            }

            // create a new column so we can to sort the table with null DateExpires values at the top
            DataColumn NewColumn = new DataColumn("Order", Type.GetType("System.DateTime"));
            FMainDS.PPartnerGiftDestination.Columns.Add(NewColumn);

            foreach (DataRow Row in FMainDS.PPartnerGiftDestination.Rows)
            {
                if (Row["p_date_expires_d"] == DBNull.Value)
                {
                    Row["Order"] = DateTime.MaxValue;
                }
                else
                {
                    Row["Order"] = Row["p_date_expires_d"];
                }
            }

            DataView myDataView = FMainDS.PPartnerGiftDestination.DefaultView;
            myDataView.Sort = "Order DESC, p_date_effective_d DESC";
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            grdDetails.Columns.AutoSize(false);

            //Prepare grid to highlight inactive Gift Destinations
            SourceGrid.Cells.Views.Cell strikeoutCell = new SourceGrid.Cells.Views.Cell();
            strikeoutCell.Font = new System.Drawing.Font(grdDetails.Font, FontStyle.Strikeout);

            // Create a condition, apply the view when true, and assign a delegate to handle it
            SourceGrid.Conditions.ConditionView condition = new SourceGrid.Conditions.ConditionView(strikeoutCell);
            condition.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                DataRowView row = (DataRowView)itemRow;

                if (row[PPartnerGiftDestinationTable.ColumnDateEffectiveId].ToString() ==
                    row[PPartnerGiftDestinationTable.ColumnDateExpiresId].ToString())
                {
                    return true;
                }

                return false;
            };

            // add condtion to grid columns
            grdDetails.Columns[0].Conditions.Add(condition);
            grdDetails.Columns[1].Conditions.Add(condition);
            grdDetails.Columns[2].Conditions.Add(condition);
            grdDetails.Columns[3].Conditions.Add(condition);
        }

        private void NewRow(Object Sender, EventArgs e)
        {
            // Generated code will automatically focus on dtpDetailDateEffective rather than txtDetailFieldKey.
            // Disable dtpDetailDateEffective to avoid verification error and manually focus on txtDetailFieldKey.

            this.dtpDetailDateEffective.Validated -= new System.EventHandler(this.ControlValidatedHandler);

            if (CreateNewPPartnerGiftDestination())
            {
                txtDetailFieldKey.Focus();
            }

            this.dtpDetailDateEffective.Validated += new System.EventHandler(this.ControlValidatedHandler);
        }

        private void NewRowManual(ref PPartnerGiftDestinationRow ANewRow)
        {
            // create a key that is at least one more that all the (unsaved) records in the grid AND all the records in the database
            int Max = 0;

            foreach (PPartnerGiftDestinationRow Row in FMainDS.PPartnerGiftDestination.Rows)
            {
                if ((Row.RowState != DataRowState.Deleted) && (Row.Key >= Max))
                {
                    Max = Row.Key + 1;
                }
            }

            ANewRow.Key = Math.Max(Max, TRemote.MPartner.Partner.WebConnectors.GetNewKeyForPartnerGiftDestination());
            ANewRow.PartnerKey = FPartnerKey;
            ANewRow.FieldKey = 0;
            ANewRow["Order"] = DateTime.MaxValue;
        }

        private void ShowDetailsManual(PPartnerGiftDestinationRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            // dtpDetailDateEffective and txtDetailFieldKey cannot be changed once saved
            if (ARow.RowState == DataRowState.Added)
            {
                dtpDetailDateEffective.Enabled = true;
                txtDetailFieldKey.Enabled = true;
            }
            else
            {
                dtpDetailDateEffective.Enabled = false;
                txtDetailFieldKey.Enabled = false;
            }

            if (ARow.DateEffective == ARow.DateExpires)
            {
                btnDeactivate.Enabled = false;
            }
            else
            {
                btnDeactivate.Enabled = true;
            }
        }

        private void GetDetailDataFromControlsManual(PPartnerGiftDestinationRow ARow)
        {
            // updates the custom field to display field name in the grid
            ARow["FieldName"] = txtDetailFieldKey.LabelText;
        }

        private void DateExpiresEntered(object sender, EventArgs e)
        {
            // order depends on dates
            if (string.IsNullOrEmpty(dtpDetailDateExpires.Text))
            {
                FPreviouslySelectedDetailRow["Order"] = DateTime.MaxValue;
            }
            else
            {
                try
                {
                    // If a user enters a date command (e.g. -100) this will fail. Hence the try-catch.
                    FPreviouslySelectedDetailRow["Order"] = Convert.ToDateTime(dtpDetailDateExpires.Text);
                }
                catch
                {
                    // Do nothing.
                }
            }
        }

        private void UpdateFieldName(object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow.RowState != DataRowState.Unchanged)
            {
                string PartnerShortName;
                TPartnerClass PartnerClass;

                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(
                    Convert.ToInt64(txtDetailFieldKey.Text), out PartnerShortName, out PartnerClass);

                FPreviouslySelectedDetailRow["FieldName"] = PartnerShortName;
            }
        }

        private void DeactivateRecord(object sender, EventArgs e)
        {
            dtpDetailDateExpires.Text = dtpDetailDateEffective.Text;

            FPetraUtilsObject.SetChangedFlag();
            btnDeactivate.Enabled = false;
        }

        private TVerificationResultCollection FVerificationResultCollection = null;

        private void FPetraUtilsObject_BeforeDataSave(object Sender, EventArgs e)
        {
            GetDetailsFromControls(FPreviouslySelectedDetailRow);

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            FVerificationResultCollection = new TVerificationResultCollection();

            // validates all the data in the datatable
            TSharedPartnerValidation_Partner.ValidateGiftDestinationManual(this, FMainDS.PPartnerGiftDestination, ref FVerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        private void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
        {
            if (e.Success)
            {
                // Reload details on successful save. This is so dtpDetailDateEffective can be made readonly
                if (!FPetraUtilsObject.HasChanges)
                {
                    ShowDetailsManual(FPreviouslySelectedDetailRow);
                }

                if (!this.Modal)
                {
                    // Broadcast message to update partner's Partner Edit screen if open
                    TFormsMessage BroadcastMessage;

                    BroadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcGiftDestinationChanged);

                    BroadcastMessage.SetMessageDataGiftDestination(
                        FPartnerKey,
                        FMainDS.PPartnerGiftDestination);

                    TFormsList.GFormsList.BroadcastFormMessage(BroadcastMessage);
                }
            }
        }

        private void ValidateDataDetailsManual(PPartnerGiftDestinationRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidateGiftDestinationRowManual(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);

            // add VerificationResults from validating on all data (FPetraUtilsObject_DataSaved)
            VerificationResultCollection.AddCollection(FVerificationResultCollection);
            FVerificationResultCollection = null;
        }

        #region Forms Messaging Interface Implementation

        /// <summary>
        /// Will be called by TFormsList to inform any Form that is registered in TFormsList
        /// about any 'Forms Messages' that are broadcasted.
        /// </summary>
        /// <remarks>Theis screen 'listens' to such 'Forms Message' broadcasts by
        /// implementing this virtual Method. This Method will be called each time a
        /// 'Forms Message' broadcast occurs.
        /// </remarks>
        /// <param name="AFormsMessage">An instance of a 'Forms Message'. This can be
        /// inspected for parameters in the Method Body and the Form can use those to choose
        /// to react on the Message, or not.</param>
        /// <returns>Returns True if the Form reacted on the specific Forms Message,
        /// otherwise false.</returns>
        public bool ProcessFormsMessage(TFormsMessage AFormsMessage)
        {
            bool MessageProcessed = false;

            if (((AFormsMessage.MessageClass == TFormsMessageClassEnum.mcPersonnelCommitmentChanged))
                && (((IFormsMessagePartnerInterface)AFormsMessage.MessageObject).PartnerKey == FPartnerKey))
            {
                TSearchCriteria[] Search = new TSearchCriteria[1];
                Search[0] = new TSearchCriteria(PPartnerGiftDestinationTable.GetPartnerKeyDBName(), FPartnerKey);

                FMainDS = new TLocalMainTDS();
                Ict.Common.Data.TTypedDataTable TypedTable;
                TRemote.MCommon.DataReader.WebConnectors.GetData(PPartnerGiftDestinationTable.GetTableDBName(), Search, out TypedTable);
                FMainDS.PPartnerGiftDestination.Merge(TypedTable);

                SetupGrid();
                UpdateRecordNumberDisplay();
                SelectRowInGrid(1);

                MessageProcessed = true;
            }

            return MessageProcessed;
        }

        #endregion
    }
}