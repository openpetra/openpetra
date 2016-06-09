//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonControls;


namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// A utility class which handles the creation of controls for a Form where the control information
    /// is stored in a Open Petra data table.
    /// </summary>
    public class TGuiControlsDataTable
    {
        private const string CONTROL_DEFINITION_ERROR = "## CONTROL DEFINITION ERROR ##";

        private DataTable FDataTable = null;
        private string FCodeColumn = null;
        private string FControlIndexColumn = null;
        private string FLabelColumn = null;
        private string FControlTypeColumn = null;
        private string FOptionalValuesColumn = null;
        private string FAttributesColumn = null;

        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Panel FCurrentHostPanel = null;

        /// <summary>
        /// Main constructor for the class
        /// </summary>
        /// <param name="ADataTable">The data table containing the data that describes the control(s) required</param>
        /// <param name="ACodeColumn">The column name for the primamry key code associated with the array of controls</param>
        /// <param name="AControlIndexColumn">The column name for the index into the array of controls.  Can be 0 for a single control.  The values do not have to be contiguous.</param>
        /// <param name="ALabelColumn">The column name for the information about the text label.</param>
        /// <param name="AControlTypeColumn">The column name for the control type - e.g. cmb, txt etc</param>
        /// <param name="AOptionalValuesColumn">The column name for optional values in a combo box</param>
        /// <param name="AAttributesColumn">The column name for the control attributes column</param>
        public TGuiControlsDataTable(DataTable ADataTable,
            string ACodeColumn,
            string AControlIndexColumn,
            string ALabelColumn,
            string AControlTypeColumn,
            string AOptionalValuesColumn,
            string AAttributesColumn)
        {
            FDataTable = ADataTable;
            FCodeColumn = ACodeColumn;
            FControlIndexColumn = AControlIndexColumn;
            FLabelColumn = ALabelColumn;
            FControlTypeColumn = AControlTypeColumn;
            FAttributesColumn = AAttributesColumn;
            FOptionalValuesColumn = AOptionalValuesColumn;
        }

        /// <summary>
        /// Removes all the controls on the current panel
        /// </summary>
        public void RemoveAllControls()
        {
            if (FCurrentHostPanel != null)
            {
                FCurrentHostPanel.Controls.Clear();
            }
        }

        /// <summary>
        /// Method to create and show the controls required for a particular code on the specified panel.
        /// Currently we support the following controls:
        ///    txt -> TextBox,
        ///    txt -> TTxtNumericTextBox with Format=Integer as the attribute,
        ///    cmb -> TCmbAutoComplete: content is defined in OptionalValues column: Format=yes/no turns the combo into a tri-state choice
        ///                where the OptionalValues do not have to be False and True because the result is yes or no based on SelectedIndex.
        ///    dtp -> TtxtPetraDate
        /// </summary>
        /// <param name="AControlCode">The code that is a primary key into the controls table</param>
        /// <param name="AHostPanel">The panel onto which the GUI controls will be placed</param>
        /// <param name="AInitialValue">The initial value for this 'setting'.  The value may be a comma separated list of values.</param>
        /// <param name="AReadOnly">Set to true to disable the control(s) or make them read-only as appropriate.</param>
        /// <param name="AValidationEventHandler">An event handler that will be called on validation</param>
        public void ShowControls(string AControlCode, Panel AHostPanel, string AInitialValue, bool AReadOnly, EventHandler AValidationEventHandler)
        {
            // Work out the PetraUtilsObject for the form that called us
            TFrmPetraUtils petraBase = ((IFrmPetra)AHostPanel.TopLevelControl).GetPetraUtilsObject();

            FPetraUtilsObject = (petraBase is TFrmPetraEditUtils) ? (TFrmPetraEditUtils)petraBase : null;

            // Set up a default view on the controls table that will tell us the controls we need for this code
            FDataTable.DefaultView.RowFilter = string.Format("{0}='{1}'", FCodeColumn, AControlCode);
            FDataTable.DefaultView.Sort = string.Format("{0} ASC, {1} ASC", FCodeColumn, FControlIndexColumn);

            AHostPanel.AutoScroll = (FDataTable.DefaultView.Count > 1);
            AHostPanel.Controls.Clear();

            // Work out the value(s) that have been passed in as a comma separated list
            StringCollection values = StringHelper.GetCSVList(AInitialValue, ",", true);

            if (values.Count != FDataTable.DefaultView.Count)
            {
                return;
            }

            FCurrentHostPanel = AHostPanel;
            int id = 0;
            int vPos = 5;
            int tabIndex = 10;

            foreach (DataRowView drv in FDataTable.DefaultView)
            {
                // get the attributes as a dictionary
                Dictionary <string, string>attributes = new Dictionary <string, string>();
                object rowValue = drv.Row[FAttributesColumn];

                if ((rowValue != null) && rowValue.ToString().Contains("="))
                {
                    string[] attPairs = drv.Row[FAttributesColumn].ToString().Split(',');

                    foreach (string attPair in attPairs)
                    {
                        string[] att = attPair.Split('=');

                        if (att.Length == 2)
                        {
                            attributes.Add(att[0].Trim(), att[1].Trim());
                        }
                    }
                }

                // See if some of the popular attributes are defined and store their values in local variables
                string formatAttribute = string.Empty;
                string allowBlankAttribute = string.Empty;
                string stretchAttribute = string.Empty;
                int widthAttribute = -1;
                int labelWidthAttribute = -1;

                if (attributes.ContainsKey("Format"))
                {
                    // Use lower case
                    formatAttribute = attributes["Format"].ToString().ToLower();
                }

                if (attributes.ContainsKey("Width"))
                {
                    widthAttribute = Convert.ToInt32(attributes["Width"]);
                }

                if (attributes.ContainsKey("LabelWidth"))
                {
                    labelWidthAttribute = Convert.ToInt32(attributes["LabelWidth"]);
                }

                if (attributes.ContainsKey("AllowBlankValue"))
                {
                    allowBlankAttribute = attributes["AllowBlankValue"].ToString().ToLower();
                }

                if (attributes.ContainsKey("Stretch"))
                {
                    stretchAttribute = attributes["Stretch"].ToString().ToLower();
                }

                // create the control label
                Label label = new Label();
                label.Name = "cLabel_" + id.ToString();
                label.Text = drv.Row[FLabelColumn].ToString() + ":";
                label.Location = new Point(5, vPos);
                label.Size = new Size((labelWidthAttribute > 0) ? labelWidthAttribute : (AHostPanel.Width / 2) - 10, 17);
                label.TextAlign = ContentAlignment.MiddleRight;
                label.Anchor = AnchorStyles.Top | AnchorStyles.Left;

                AHostPanel.Controls.Add(label);

                // Create the control
                // We support text boxes, numeric (integer) text boxes, combo boxes and date boxes
                switch (drv.Row[FControlTypeColumn].ToString().Substring(0, 3))
                {
                    case "txt":

                        if (formatAttribute == "integer")
                        {
                            TTxtNumericTextBox txt = new TTxtNumericTextBox();
                            txt.Context = AHostPanel;
                            txt.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Integer;
                            txt.Name = "cValue_" + id.ToString();
                            txt.Location = new Point(label.Right + 10, vPos);
                            txt.Size = new Size((widthAttribute > 0) ? widthAttribute : 100, 22);
                            txt.TabStop = true;
                            txt.TabIndex = tabIndex;

                            int iValue;

                            if (int.TryParse(values[id], out iValue))
                            {
                                txt.NumberValueInt = iValue;
                            }

                            txt.ReadOnly = AReadOnly;

                            txt.Validated += AValidationEventHandler;
                            txt.TextChanged += new EventHandler(MultiChangeEventHandler);

                            AHostPanel.Controls.Add(txt);
                        }
                        else
                        {
                            TextBox txt = new TextBox();
                            txt.Name = "cValue_" + id.ToString();
                            txt.Location = new Point(label.Right + 10, vPos);
                            txt.Size = new Size((widthAttribute > 0) ? widthAttribute : AHostPanel.Width - label.Width - 25, 22);
                            txt.TabStop = true;
                            txt.TabIndex = tabIndex;
                            txt.Text = values[id];
                            txt.ReadOnly = AReadOnly;

                            if (stretchAttribute == "horizontally")
                            {
                                txt.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                            }

                            txt.Validated += AValidationEventHandler;
                            txt.TextChanged += new EventHandler(MultiChangeEventHandler);

                            AHostPanel.Controls.Add(txt);
                        }

                        break;

                    case "cmb":

                        if (attributes.ContainsKey("List"))
                        {
                            TCmbAutoPopulated cmb = new TCmbAutoPopulated();
                            cmb.Name = "cValue_" + id.ToString();
                            cmb.Location = new Point(label.Right + 10, vPos);
                            cmb.Size = new Size((widthAttribute > 0) ? widthAttribute : AHostPanel.Width - label.Width - 25, 22);
                            cmb.TabStop = true;
                            cmb.TabIndex = tabIndex;
                            cmb.ListTable = (TCmbAutoPopulated.TListTableEnum)Enum.Parse(typeof(TCmbAutoPopulated.TListTableEnum),
                                attributes["List"].ToString(),
                                true);
                            cmb.InitialiseUserControl();
                            cmb.SetSelectedString(values[id], -1);

                            cmb.Enabled = !AReadOnly;

                            cmb.Validated += AValidationEventHandler;
                            cmb.SelectedValueChanged += new EventHandler(MultiChangeEventHandler);

                            AHostPanel.Controls.Add(cmb);
                        }
                        else
                        {
                            TCmbAutoComplete cmb = new TCmbAutoComplete();
                            cmb.Name = "cValue_" + id.ToString();
                            cmb.Location = new Point(label.Right + 10, vPos);
                            cmb.Size = new Size((widthAttribute > 0) ? widthAttribute : AHostPanel.Width - label.Width - 25, 22);
                            cmb.TabStop = true;
                            cmb.TabIndex = tabIndex;

                            if (drv.Row[FOptionalValuesColumn] != null)
                            {
                                string[] optValues = drv.Row[FOptionalValuesColumn].ToString().Split(',');

                                for (int i = 0; i < optValues.Length; i++)
                                {
                                    cmb.Items.Add(optValues[i].ToString().Trim());
                                }
                            }

                            if (formatAttribute == "yes/no")
                            {
                                cmb.SetSelectedYesNo(values[id]);
                                cmb.Tag = "yes/no";
                            }
                            else
                            {
                                cmb.Text = values[id];
                            }

                            if (stretchAttribute == "horizontally")
                            {
                                cmb.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                            }

                            cmb.AllowBlankValue = (allowBlankAttribute == "true");
                            cmb.Enabled = !AReadOnly;

                            cmb.Validated += AValidationEventHandler;
                            cmb.SelectedIndexChanged += new EventHandler(MultiChangeEventHandler);
                            cmb.TextChanged += new EventHandler(ComboBoxTextChangeEventHandler);

                            AHostPanel.Controls.Add(cmb);
                        }

                        break;

                    case "dtp":
                        TtxtPetraDate dtp = new TtxtPetraDate();
                        dtp.Name = "cValue_" + id.ToString();
                        dtp.Location = new Point(label.Right + 10, vPos);
                        dtp.Size = new Size(94, 22);
                        dtp.TabStop = true;
                        dtp.TabIndex = tabIndex;

                        DateTime dt;

                        if (DateTime.TryParse(values[id], out dt))
                        {
                            dtp.Date = Convert.ToDateTime(values[id]);
                        }
                        else
                        {
                            dtp.Date = null;
                        }

                        dtp.ReadOnly = AReadOnly;

                        dtp.Validated += AValidationEventHandler;
                        dtp.DateChanged += new TPetraDateChangedEventHandler(MultiChangeEventHandler);

                        AHostPanel.Controls.Add(dtp);
                        break;

                    default:
                        break;
                }

                id++;
                tabIndex += 10;
                vPos += 30;
            }
        }

        /// <summary>
        /// Gets the value of the current state of the controls.  If there are multiple controls the value will be a comma separated list.
        /// </summary>
        /// <returns>The value formatted as a string.  If there are no controls the result will be an empty string.
        /// If there was an error in the database specification of the controls, the result will be an empty string
        /// If a control value is null we set the string representation to ? which can be validated by the client.</returns>
        public string GetCurrentValue()
        {
            string ReturnValue = "";

            if (FCurrentHostPanel == null)
            {
                return ReturnValue;
            }

            if (FCurrentHostPanel.Controls.Count == 0)
            {
                return CONTROL_DEFINITION_ERROR;
            }

            for (int i = 0; i < FCurrentHostPanel.Controls.Count; i++)
            {
                Control control = FCurrentHostPanel.Controls[i];

                if (control is Label)
                {
                    continue;
                }

                if (control is TTxtNumericTextBox)
                {
                    // Numeric text box returns ? if the entry is null
                    TTxtNumericTextBox txt = (TTxtNumericTextBox)control;

                    switch (txt.ControlMode)
                    {
                        case TTxtNumericTextBox.TNumericTextBoxMode.Integer:
                            ReturnValue += ((txt.NumberValueInt.HasValue ? txt.NumberValueInt.ToString() : "?") + ",");
                            break;

                        default:
                            break;
                    }
                }
                else if (control is TextBox)
                {
                    ReturnValue += (((TextBox)control).Text + ",");
                }

                if (control is TCmbAutoPopulated)
                {
                    // Returns ? if the selected index is -1
                    TCmbAutoPopulated cmb = (TCmbAutoPopulated)control;
                    string s = cmb.GetSelectedString();
                    ReturnValue += ((s.Length == 0 ? "?" : s) + ",");
                }

                if (control is TCmbAutoComplete)
                {
                    TCmbAutoComplete cmb = (TCmbAutoComplete)control;

                    if (Convert.ToString(control.Tag) == "yes/no")
                    {
                        ReturnValue += (cmb.GetSelectedYesNo() + ",");
                    }
                    else
                    {
                        // Returns ? if the selected index is -1
                        string s = cmb.GetSelectedString();
                        ReturnValue += ((s.Length == 0 ? "?" : s) + ",");
                    }
                }

                if (control is TtxtPetraDate)
                {
                    // Returns ? where the date value is null
                    TtxtPetraDate txt = (TtxtPetraDate)control;
                    ReturnValue += ((txt.Date.HasValue ? txt.Date.Value.ToString("yyyy-MM-dd") : "?") + ",");
                }
            }

            // Remove the trailing comma we added
            return ReturnValue.Trim(',');
        }

        /// <summary>
        /// A public validation method that can be called from the screen in ManualValidation.
        /// It checks that there were no errors in displaying the controls on the screen.
        /// </summary>
        /// <param name="ARowValue">The current value for the setting</param>
        /// <param name="ADataColum">The data column that holds the value</param>
        /// <param name="AContext">A validation context</param>
        /// <param name="AVerificationResults">The verification result collection to add any errors to</param>
        /// <returns>True if there are no errors, false otherwise</returns>
        public bool Validate(string ARowValue, DataColumn ADataColum, object AContext, TVerificationResultCollection AVerificationResults)
        {
            TVerificationResult verificationResult = null;

            if (ARowValue == TGuiControlsDataTable.CONTROL_DEFINITION_ERROR)
            {
                // This message should not occur in production because any control definition errors should have been discovered in testing.
                // They arise if the content of the s_system_defaults_gui_controls table is not correct or if
                //   the default value contains an array of values that does not match the array size of the controls in the controls table.
                string message = Catalog.GetString("There is an error in the database relating to the set-up of this row.  ");
                message += Catalog.GetString(
                    "In order to avoid saving badly constructed data to the System Settings table you must close the screen.  ");
                message += Catalog.GetString(
                    "You can re-open it and continue to change the settings in other rows, but do not select the current row until the problem is fixed.");
                verificationResult = new TScreenVerificationResult(AContext, ADataColum, message, null, TResultSeverity.Resv_Critical);
            }

            AVerificationResults.Auto_Add_Or_AddOrRemove(AContext, verificationResult, ADataColum);
            return verificationResult == null;
        }

        private void MultiChangeEventHandler(object sender, EventArgs e)
        {
            if (FPetraUtilsObject != null)
            {
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void ComboBoxTextChangeEventHandler(object sender, EventArgs e)
        {
            if (FPetraUtilsObject != null)
            {
                if (sender is TCmbAutoComplete)
                {
                    TCmbAutoComplete cmb = (TCmbAutoComplete)sender;

                    if ((cmb.Text.Length == 0) && (cmb.SelectedIndex == -1) && cmb.AllowBlankValue)
                    {
                        FPetraUtilsObject.SetChangedFlag();
                    }
                }
            }
        }
    }
}