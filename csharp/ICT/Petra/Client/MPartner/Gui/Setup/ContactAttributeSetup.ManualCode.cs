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
            ucContactDetail.MainDS = null;
            ucContactDetail.PetraUtilsObject = FPetraUtilsObject;

            // The auto-generator does not dock our user control correctly
            grpExtraDetails.Dock = System.Windows.Forms.DockStyle.Bottom;

            // We need to capture the 'DataSaved' event, so we can save our Extra DataSet
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_DataSavingStarted);

            // we also want to know if the number of rows in our user control changes
            ucContactDetail.CountChanged += new CountChangedEventHandler(ucContactDetail_CountChanged);


            txtDetailContactAttributeCode.LostFocus += new EventHandler(txtDetailContactAttributeCode_LostFocus);
        }

        private void RunOnceOnActivationManual()
        {
            // Initialise the GUI of the user control
            ucContactDetail.InitUserControl();

            // Set up the correct filter for the bottom grid, based on our initial contact attribute
            if (FMainDS.PContactAttribute.Rows.Count > 0)
            {
                ucContactDetail.SetContactAttribute(txtDetailContactAttributeCode.Text);
            }

            // Add an extra column to our main data set that contains the number of sub-details for a given code
            NumDetailCodesColumnOrdinal = FMainDS.PContactAttribute.Columns.Add("NumDetails", typeof(int)).Ordinal;

            for (int i = 0; i < FMainDS.PContactAttribute.Rows.Count; i++)
            {
                string code = FMainDS.PContactAttribute.Rows[i][FMainDS.PContactAttribute.ColumnContactAttributeCode.Ordinal].ToString();
                FMainDS.PContactAttribute.Rows[i][NumDetailCodesColumnOrdinal] = ucContactDetail.NumberOfDetails(code);
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
            CreateNewPContactAttribute();

            // Update our extra column for the new row
            FPreviouslySelectedDetailRow[NumDetailCodesColumnOrdinal] = 0;
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
                    MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            // Now we need to remove all the detail attributes associated with this contact attribute.
            // (If we can delete the current row, it must also be the case that we can delete all the detail attributes for this row)
            // Then we can delete the contact attribute itself...
            ucContactDetail.DeleteAll();

            // Get the selected grid row
            int nSelectedRow = grdDetails.DataSourceRowToIndex2(grdDetails.SelectedDataRowsAsDataRowView[0]) + 1;
            FPreviouslySelectedDetailRow.Delete();
            FPetraUtilsObject.SetChangedFlag();

            // Select the next row to show
            int maxRow = grdDetails.Rows.Count - 1;

            if (nSelectedRow > maxRow)
            {
                nSelectedRow = maxRow;
            }

            if (nSelectedRow > 0)
            {
                grdDetails.SelectRowInGrid(nSelectedRow);
            }

            // Check the enabled states now that we have fewer rows
            btnDelete.Enabled = nSelectedRow > 0 && !txtDetailContactAttributeCode.ReadOnly;
            pnlDetails.Enabled = maxRow > 0;
        }

        private void ShowDetailsManual(PContactAttributeRow ARow)
        {
            if (ARow == null)
            {
                pnlDetails.Enabled = false;
                ucContactDetail.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                pnlDetails.Enabled = true;
                ucContactDetail.Enabled = true;
                btnDelete.Enabled = !txtDetailContactAttributeCode.ReadOnly;

                // Pass the contact attribute to the user control - it will then update itself
                ucContactDetail.SetContactAttribute(ARow.ContactAttributeCode);
            }
        }

        private void GetDetailDataFromControlsManual(PContactAttributeRow ARow)
        {
            // Tell the user control to get its data too
            ucContactDetail.GetDataFromControls();
        }

        private void txtDetailContactAttributeCode_LostFocus(object sender, EventArgs e)
        {
            // If the user has changed the content of the code we have some checking to do
            int NumDetails = Convert.ToInt32(FPreviouslySelectedDetailRow[NumDetailCodesColumnOrdinal]);

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
            if (FMainDS.PContactAttribute.Rows.Find(new object[] { newCode }) != null)
            {
                // It is the same as an existing code.
                // On most screens this would normally get trapped later but we want to
                // trap it now so we don't change the detail attributes unnecessarily
                MessageBox.Show(String.Format(Catalog.GetString(
                            "'{0}' has already been used for a Contact Attribute Code."), newCode), Catalog.GetString("Contact Attribute"));
                txtDetailContactAttributeCode.Text = FPreviouslySelectedDetailRow.ContactAttributeCode;
                txtDetailContactAttributeCode.Focus();
                txtDetailContactAttributeCode.SelectAll();
                return;
            }

            // So it is safe to modify the detail attribute
            ucContactDetail.ModifyAttributeCode(newCode);
        }

        private void FPetraUtilsObject_DataSavingStarted(object Sender, EventArgs e)
        {
            // We need to check that there is at least one detail attribute in the user control.
            // This is because, when an attribute is used to apply to a partner,
            //   the attribute and detail attribute are both required for the primary key
            // We will go through all the rows in the table making sure that we have non-zero values in our extra column
            bool bFoundError = false;
            string msg = String.Empty;

            for (int i = 0; i < FMainDS.PContactAttribute.Rows.Count; i++)
            {
                int NumDetailAttributes = Convert.ToInt32(FMainDS.PContactAttribute.Rows[i][NumDetailCodesColumnOrdinal]);

                if (NumDetailAttributes == 0)
                {
                    msg = String.Format(
                        Catalog.GetString("There are no detail codes associated with the '{0}' contact attribute.  No data has been saved."),
                        FMainDS.PContactAttribute.Rows[i][0]);
                    TVerificationResult result = new TVerificationResult(FMainDS.PContactAttribute.ColumnContactAttributeCode.ColumnName,
                        msg,
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.VerificationResultCollection.Add(result);
                    bFoundError = true;
                    break;
                }
            }

            if (bFoundError)
            {
                MessageBox.Show(msg, Catalog.GetString("Error Saving Data"), MessageBoxButtons.OK);
            }
        }

        private void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
        {
            // Save the changes in the user control
            if (e.Success)
            {
                FPetraUtilsObject.SetChangedFlag();
                ucContactDetail.SaveChanges();
            }
        }

        private void ucContactDetail_CountChanged(object Sender, CountEventArgs e)
        {
            // something has changed in our user control (add/delete rows)
            FPreviouslySelectedDetailRow[NumDetailCodesColumnOrdinal] = e.NewCount;
        }
    }
}