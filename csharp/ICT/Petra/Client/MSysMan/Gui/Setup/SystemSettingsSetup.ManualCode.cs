//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2016 by OM International
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

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Controls;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;

namespace Ict.Petra.Client.MSysMan.Gui.Setup
{
    public partial class TFrmSystemSettingsSetup
    {
        private const string FControlsCountName = "ControlsCount";      // Column header name
        private int FColumnControlCountOrdinal = -1;                    // Ordinal for this column
        private TGuiControlsDataTable FControlsDataTable = null;        // The table that holds gui layout data

        // Display Class eg Partner, Finance
        private bool FSuppressClassChange = true;
        private string FActiveClass = null;

        // SysAdmin Mode
        private bool FLaunchedInSysAdminMode = false;
        private bool FShowingSysAdminCodes = false;
        private string FPrevDefaultValue = null;

        #region Initialisation and Setup

        private void InitializeManualCode()
        {
            // Fix up the screen.  Set the text boxes to look like labels and choose different fonts, where necessary.
            txtDetailDefaultCodeLocal.BorderStyle = BorderStyle.None;
            txtDetailDefaultCodeLocal.BackColor = System.Drawing.SystemColors.Control;

            txtDetailDefaultDescription.BorderStyle = BorderStyle.None;
            txtDetailDefaultDescription.BackColor = System.Drawing.SystemColors.Control;
            txtDetailDefaultDescription.Font = new System.Drawing.Font(txtDetailDefaultDescription.Font, System.Drawing.FontStyle.Regular);

            txtDetailDefaultCodeIntl.BorderStyle = BorderStyle.None;
            txtDetailDefaultCodeIntl.BackColor = System.Drawing.SystemColors.Control;
            txtDetailDefaultCodeIntl.Font = new System.Drawing.Font(txtDetailDefaultCodeIntl.Font, System.Drawing.FontStyle.Regular);

            txtDetailDefaultCode.BorderStyle = BorderStyle.None;
            txtDetailDefaultCode.BackColor = System.Drawing.SystemColors.Control;
            txtDetailDefaultCode.Font = new System.Drawing.Font(txtDetailDefaultCode.Font.FontFamily.Name, txtDetailDefaultCode.Font.Size * 0.75f);

            // We can save a bit of height
            pnlRow2.Height -= 15;
            grpDetails.Height -= 15;

            TTypedDataTable controlsTable;

            // Get the Gui Controls table that describes the controls for each displayed default code
            TRemote.MCommon.DataReader.WebConnectors.GetData(SSystemDefaultsGuiTable.GetTableDBName(), null, out controlsTable);
            controlsTable.DefaultView.Sort = string.Format("{0} ASC, {1} ASC",
                SSystemDefaultsGuiTable.GetDefaultCodeDBName(), SSystemDefaultsGuiTable.GetControlIdDBName());

            // Add a column to the main system defaults table that will hold the number of controls required for the code value
            FColumnControlCountOrdinal = FMainDS.SSystemDefaults.Columns.Add(FControlsCountName, typeof(System.Int32)).Ordinal;

            foreach (SSystemDefaultsRow dr in FMainDS.SSystemDefaults.Rows)
            {
                if (dr.IsCategoryNull() || dr.IsDefaultCodeLocalNull())
                {
                    // We cannot display settings where we do not have these values
                    dr[FColumnControlCountOrdinal] = 0;
                }
                else
                {
                    controlsTable.DefaultView.RowFilter = string.Format("{0}='{1}'",
                        SSystemDefaultsGuiTable.GetDefaultCodeDBName(), ((SSystemDefaultsRow)dr).DefaultCode);
                    dr[FColumnControlCountOrdinal] = controlsTable.DefaultView.Count;
                }
            }

            FMainDS.SSystemDefaults.AcceptChanges();

            // Now pass the controls table to our special controls data table class
            FControlsDataTable = new TGuiControlsDataTable(
                controlsTable,
                SSystemDefaultsGuiTable.GetDefaultCodeDBName(),
                SSystemDefaultsGuiTable.GetControlIdDBName(),
                SSystemDefaultsGuiTable.GetControlLabelDBName(),
                SSystemDefaultsGuiTable.GetControlTypeDBName(),
                SSystemDefaultsGuiTable.GetControlOptionalValuesDBName(),
                SSystemDefaultsGuiTable.GetControlAttributesDBName());

            // Add our event handlers for forms messaging after a successful save
            FPetraUtilsObject.DataSavingValidated += FPetraUtilsObject_DataSavingValidated;
            FPetraUtilsObject.DataSaved += FPetraUtilsObject_DataSaved;
        }

        private void OnSettingsClassChange(object sender, EventArgs e)
        {
            // We get called here when the settings class combo has changed
            if ((FFilterAndFindObject == null) || (FSuppressClassChange == true))
            {
                return;
            }

            if (sender != null)
            {
                // We don't do this at startup, but after that we need to set the class back to what it was before if validation fails
                if (!ValidateAllData(false, TErrorProcessingMode.Epm_All))
                {
                    FSuppressClassChange = true;
                    ((TCmbAutoComplete)sender).SetSelectedString(FActiveClass);
                    FSuppressClassChange = false;
                    return;
                }

                FActiveClass = ((TCmbAutoComplete)sender).GetSelectedString();
            }

            // We need to know if we are displaying a sysAdmin row or not
            string sysAdminRowFilter = "12345";
            string rowFilter = string.Format("{0}>0 AND ", FControlsCountName);

            if (cmbClass.SelectedValue == null)
            {
                rowFilter += "1=0";
            }
            else if (FLaunchedInSysAdminMode && (cmbClass.SelectedIndex == 0))
            {
                sysAdminRowFilter = string.Format("{0}=0 OR {1} IS NULL", FControlsCountName, SSystemDefaultsTable.GetCategoryDBName());
                rowFilter = sysAdminRowFilter;
            }
            else
            {
                rowFilter += string.Format("{0}='{1}'", SSystemDefaultsTable.GetCategoryDBName(), cmbClass.SelectedValue.ToString());
            }

            FFilterAndFindObject.FilterPanelControls.SetBaseFilter(rowFilter, true);

            // Both these will cause validation to run, but it should be ok because we checked it above
            FFilterAndFindObject.ApplyFilter();
            SelectRowInGrid(1);

            // Depending on how validation worked out we now know whether we are displaying the sysAdmin codes or not
            FShowingSysAdminCodes = FMainDS.SSystemDefaults.DefaultView.RowFilter.StartsWith(sysAdminRowFilter);

            // Now we can show/hide columns 0 and 1
            grdDetails.Columns[0].Visible = FShowingSysAdminCodes;
            grdDetails.Columns[1].Visible = !FShowingSysAdminCodes;
            grdDetails.AutoResizeGrid();
        }

        private void RunOnceOnActivationManual()
        {
            // Start by dealing with the settings classes that will be displayed in the combo box filter
            DataTable settingsClassDT = FMainDS.SSystemDefaults.DefaultView.ToTable("SettingsClass", true, SSystemDefaultsTable.GetCategoryDBName());

            if (Form.ModifierKeys == (Keys.Alt | Keys.Shift))
            {
                // This is the back-door to showing all the system defaults
                FLaunchedInSysAdminMode = true;

                DataRow dr = settingsClassDT.NewRow();
                dr[0] = "!SysAdmin!";
                settingsClassDT.Rows.Add(dr);
            }

            cmbClass.DisplayMember = SSystemDefaultsTable.GetCategoryDBName();
            cmbClass.ValueMember = SSystemDefaultsTable.GetCategoryDBName();
            settingsClassDT.DefaultView.RowFilter = string.Format("{0} IS NOT NULL", SSystemDefaultsTable.GetCategoryDBName());
            settingsClassDT.DefaultView.Sort = string.Format("{0}", cmbClass.DisplayMember);
            cmbClass.DataSource = settingsClassDT.DefaultView;

            // Select the first item in the class filter
            if (settingsClassDT.DefaultView.Count > 0)
            {
                cmbClass.SelectedIndex = 0;
                FActiveClass = cmbClass.GetSelectedString();
            }

            // This will select the first row in the grid now that everything has been activated
            FSuppressClassChange = false;
            OnSettingsClassChange(null, null);
        }

        #endregion

        #region Show and Get Details

        private void ShowDetailsManual(SSystemDefaultsRow ARow)
        {
            if (ARow == null)
            {
                FPetraUtilsObject.ClearControls(pnlBottom);
                FControlsDataTable.RemoveAllControls();
                pnlBottom.Enabled = false;
                return;
            }

            // Display the controls required for the system default code in this row
            pnlBottom.Enabled = true;
            FPrevDefaultValue = ARow.DefaultValue;
            FControlsDataTable.ShowControls(ARow.DefaultCode, pnlValues, ARow.DefaultValue, ARow.ReadOnly, ControlValidatedHandler);
        }

        private void GetDetailDataFromControlsManual(SSystemDefaultsRow ARow)
        {
            if (FShowingSysAdminCodes)
            {
                ARow.DefaultValue = FPrevDefaultValue;
            }
            else
            {
                ARow.DefaultValue = FControlsDataTable.GetCurrentValue();
            }
        }

        #endregion

        #region Manual Validation

        private void ValidateDataDetailsManual(SSystemDefaultsRow ARow)
        {
            if (FShowingSysAdminCodes)
            {
                return;
            }

            TVerificationResultCollection verificationResults = FPetraUtilsObject.VerificationResultCollection;
            DataColumn validationColumn = ARow.Table.Columns[SSystemDefaultsTable.ColumnDefaultValueId];
            TVerificationResult verificationResult = null;

            // First we need to validate that there were no errors setting up the controls for this row
            if (!FControlsDataTable.Validate(ARow.DefaultValue, validationColumn, this, verificationResults))
            {
                // No point in carrying on with other validations because the controls are all messed up
                return;
            }

            // Individual rows can be validated to check that settings code values are ok.

            // This shows how to check SYSDEFAULT_LOCALISEDCOUNTYLABEL is not an empty string
            // We no longer check for this because Petra databases have empty string here and our main OP code takes care of empty string
            //  by substituting our default County/State

            //if (string.Compare(ARow.DefaultCode, SharedConstants.SYSDEFAULT_LOCALISEDCOUNTYLABEL, true) == 0)
            //{
            //    Control[] validationControl = pnlValues.Controls.Find("cValue_0", true);

            //    if (validationControl.Length > 0)
            //    {
            //        verificationResult = TStringChecks.StringMustNotBeEmpty(ARow.DefaultValue, "",
            //            this, validationColumn, validationControl[0]);

            //        if (verificationResult != null)
            //        {
            //            verificationResult.OverrideResultText(CommonResourcestrings.StrSettingCannotBeEmpty);
            //        }
            //    }

            //    verificationResults.Auto_Add_Or_AddOrRemove(this, verificationResult, validationColumn);
            //}
        }

        #endregion

        #region Forms messaging after save

        private SSystemDefaultsRow FIncludesLocalisedCountyLabel = null;
        private SSystemDefaultsRow FIncludesDonorZero = null;
        private SSystemDefaultsRow FIncludesRecipientZero = null;

        private void FPetraUtilsObject_DataSavingValidated(object Sender, System.ComponentModel.CancelEventArgs e)
        {
            // Capture any rows that are going to be saved that we are interested in
            SSystemDefaultsTable submitDT = FMainDS.SSystemDefaults.GetChangesTyped();

            FIncludesLocalisedCountyLabel = (SSystemDefaultsRow)submitDT.Rows.Find(SharedConstants.SYSDEFAULT_LOCALISEDCOUNTYLABEL);
            FIncludesDonorZero = (SSystemDefaultsRow)submitDT.Rows.Find(SharedConstants.SYSDEFAULT_DONORZEROISVALID);
            FIncludesRecipientZero = (SSystemDefaultsRow)submitDT.Rows.Find(SharedConstants.SYSDEFAULT_RECIPIENTZEROISVALID);
        }

        private void FPetraUtilsObject_DataSaved(object Sender, Common.TDataSavedEventArgs e)
        {
            if (!e.Success)
            {
                return;
            }

            // Data was saved successfully so do we need to broadcast anything??
            if (FIncludesLocalisedCountyLabel != null)
            {
                TFormsMessage broadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcLocalisedCountyLabelChanged);
                broadcastMessage.SetMessageDataName(FIncludesLocalisedCountyLabel.DefaultValue);
                TFormsList.GFormsList.BroadcastFormMessage(broadcastMessage);
            }

            if (FIncludesDonorZero != null)
            {
                TFormsMessage broadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcGiftPartnerZeroChanged);
                Dictionary <string, object>data = new Dictionary <string, object>();
                data.Add(FIncludesDonorZero.DefaultCode, StringHelper.StrToBool(FIncludesDonorZero.DefaultValue));
                broadcastMessage.SetMessageDataSimpleDictionary(data);
                TFormsList.GFormsList.BroadcastFormMessage(broadcastMessage);
            }

            if (FIncludesRecipientZero != null)
            {
                TFormsMessage broadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcGiftPartnerZeroChanged);
                Dictionary <string, object>data = new Dictionary <string, object>();
                data.Add(FIncludesRecipientZero.DefaultCode, StringHelper.StrToBool(FIncludesRecipientZero.DefaultValue));
                broadcastMessage.SetMessageDataSimpleDictionary(data);
                TFormsList.GFormsList.BroadcastFormMessage(broadcastMessage);
            }
        }

        #endregion
    }
}