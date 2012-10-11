//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SourceGrid;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmContactAttributeSetup
    {
        // A local variable that saves the column ordinal for our additional column in the main data table
        private int NumDetailCodesColumnOrdinal = 0;

        private void InitializeManualCode()
        {
            // Initialise the user control variables
            ucoContactDetail.PetraUtilsObject = FPetraUtilsObject;

            // The auto-generator does not dock our user control correctly
            grpExtraDetails.Dock = System.Windows.Forms.DockStyle.Bottom;

            // We need to capture the 'DataSaved' event, so we can save our Extra DataSet
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);

            // we also want to know if the number of rows in our user control changes
            ucoContactDetail.CountChanged += new CountChangedEventHandler(ucoContactDetail_CountChanged);


            txtDetailContactAttributeCode.LostFocus += new EventHandler(txtDetailContactAttributeCode_LostFocus);
        }

        private void RunOnceOnActivationManual()
        {
            // Set up the correct filter for the bottom grid, based on our initial contact attribute
            if (FMainDS.PContactAttribute.Rows.Count > 0)
            {
                ucoContactDetail.SetContactAttribute(txtDetailContactAttributeCode.Text);
            }

            // Add an extra column to our main data set that contains the number of sub-details for a given code
            NumDetailCodesColumnOrdinal = FMainDS.PContactAttribute.Columns.Add("NumDetails", typeof(int)).Ordinal;

            for (int i = 0; i < FMainDS.PContactAttribute.Rows.Count; i++)
            {
                string code = FMainDS.PContactAttribute.Rows[i][FMainDS.PContactAttribute.ColumnContactAttributeCode.Ordinal].ToString();
                FMainDS.PContactAttribute.Rows[i][NumDetailCodesColumnOrdinal] = ucoContactDetail.NumberOfDetails(code);
            }

            // add a column to the grid and bind it to our new data set column
            grdDetails.AddTextColumn(Catalog.GetString("Number of Detail Codes"), FMainDS.PContactAttribute.Columns[NumDetailCodesColumnOrdinal]);
        }

        private void NewRowManual(ref PContactAttributeRow ARow)
        {
            string newCode = Catalog.GetString("NEWCODE");
            Int32 countNewCode = 1;

            if (FMainDS.PContactAttribute.Rows.Find(new object[] { newCode }) != null)
            {
                while (FMainDS.PContactAttribute.Rows.Find(new object[] { newCode + countNewCode.ToString() }) != null)
                {
                    countNewCode++;
                }

                newCode += countNewCode.ToString();
            }

            ARow.ContactAttributeCode = newCode;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            int nRowCount = grdDetails.Rows.Count;

            CreateNewPContactAttribute();

            // Did we actually create one??
            if (nRowCount == grdDetails.Rows.Count)
            {
                return;
            }

            // Create the required initial detail attribute.  This will automatically fire the event that updates our details count column
            ucoContactDetail.CreateFirstAttributeDetail(txtDetailContactAttributeCode.Text);
            txtDetailContactAttributeCode.Focus();
        }

        private void DeleteRecord(Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if (MessageBox.Show(Catalog.GetString(
                        "Are you sure that you want to delete the current Contact Attribute?  If you choose 'Yes', all the detail attributes for this Contact Attribute will be deleted as well."),
                    Catalog.GetString("Delete Row"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            // Now we need to remove all the detail attributes associated with this contact attribute.
            // (If we can delete the current row, it must also be the case that we can delete all the detail attributes for this row)
            // Then we can delete the contact attribute itself...
            ucoContactDetail.DeleteAll();

            // Get the selected grid row
            int nSelectedRow = grdDetails.SelectedRowIndex();
            FPreviouslySelectedDetailRow.Delete();
            FPetraUtilsObject.SetChangedFlag();

            // Select the next row to show
            SelectRowInGrid(nSelectedRow);
        }

        private void ShowDetailsManual(PContactAttributeRow ARow)
        {
            if (ARow == null)
            {
                pnlDetails.Enabled = false;
                ucoContactDetail.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                pnlDetails.Enabled = true;
                ucoContactDetail.Enabled = true;
                btnDelete.Enabled = grdDetails.Rows.Count > 1 && !txtDetailContactAttributeCode.ReadOnly;

                // Pass the contact attribute to the user control - it will then update itself
                ucoContactDetail.SetContactAttribute(ARow.ContactAttributeCode);
            }
        }

        private void GetDetailDataFromControlsManual(PContactAttributeRow ARow)
        {
            // Tell the user control to get its data too
            ucoContactDetail.GetDetailsFromControls();
        }

        private void txtDetailContactAttributeCode_LostFocus(object sender, EventArgs e)
        {
            if (NumDetailCodesColumnOrdinal == 0)
            {
                return;                                                 // No problem if we have no details yet
            }

            string newCode = txtDetailContactAttributeCode.Text;

            if (newCode.CompareTo(FPreviouslySelectedDetailRow[0]) == 0)
            {
                return;                                                                 // same as before
            }

            // ooops!  The user has edited the attribute code and we have some detail codes that depended on it!
            // We have to update the detail codes provided the new code is good (and passed validation)
            if (newCode == String.Empty)
            {
                return;
            }

            if (FMainDS.PContactAttribute.Rows.Find(new object[] { newCode }) != null)
            {
                // It is the same as an existing code.
                // On most screens this would normally get trapped later but we want to
                // trap it now so we don't change the detail attributes unnecessarily
                MessageBox.Show(String.Format(
                        Catalog.GetString("'{0}' has already been used for a Contact Attribute Code."), newCode),
                    Catalog.GetString("Contact Attribute"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtDetailContactAttributeCode.Text = FPreviouslySelectedDetailRow.ContactAttributeCode;
                txtDetailContactAttributeCode.Focus();
                txtDetailContactAttributeCode.SelectAll();
                return;
            }

            // So it is safe to modify the detail attribute
            ucoContactDetail.ModifyAttributeCode(newCode);
            grdDetails.Focus();
        }

        private void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
        {
            // Save the changes in the user control
            if (e.Success)
            {
                FPetraUtilsObject.SetChangedFlag();
                ucoContactDetail.SaveChanges();
            }
        }

        private void ucoContactDetail_CountChanged(object Sender, CountEventArgs e)
        {
            // something has changed in our user control (add/delete rows)
            FPreviouslySelectedDetailRow[NumDetailCodesColumnOrdinal] = e.NewCount;
        }

        private void ValidateDataDetailsManual(PContactAttributeRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult = null;

            // The added column at the end of the table, which is the number of detail codes for this attribute, must not be zero
            // Our problem is that the control that is really the correct one to verify is the details grid of the user control.
            // This control is not one that can be verified.
            // We can do the verification here - either by using the Active checkbox as a proxy (the nearest control) in which case we get a tooltip
            // but when we tab away from the checkbox, which is not quite right...
            // or we can just use this code without the Validation=true in the YAML - in which case we just get the validation dialog
            ValidationColumn = ARow.Table.Columns[PContactAttributeTable.ColumnActiveId];

            if (ARow[NumDetailCodesColumnOrdinal] != System.DBNull.Value)
            {
                VerificationResult = TNumericalChecks.IsPositiveInteger(Convert.ToInt32(ARow[NumDetailCodesColumnOrdinal]),
                    "Contact Detail",
                    this, ValidationColumn, null);

                if (VerificationResult != null)
                {
                    VerificationResult.OverrideResultText(Catalog.GetString(
                            "You must create at least one 'Attribute Detail Code' for each 'Contact Attribute'."));
                }

                // Handle addition to/removal from TVerificationResultCollection.
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, false);
            }
        }
    }
}