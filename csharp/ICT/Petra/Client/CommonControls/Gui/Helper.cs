//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// Contains Extension Methods that add functionality to Controls.
    /// </summary>
    public static class TControlExtensions
    {
        /// <summary>
        /// Finds the UserControl that hosts <paramref name="AControl" />.
        /// </summary>
        /// <param name="AControl">Control to find the hosting UserControl for.</param>
        /// <param name="AIfControlIsUserControlReturnThis">Set to true if the Control that is
        /// passed is a UserControl and this Method should return it instead of searching for
        /// a UserControl that this UserControl is hosted in.</param>
        /// <param name="AIfControlIsTabControlReturnChildUserControl">Set to true if the Control that is
        /// passed is a TabPage and this Method should return the UserControl that is placed on it
        /// instead of searching for a UserControl that this TabPage is hosted in.</param>
        /// <returns>The UserControl that hosts <paramref name="AControl" /> or null
        /// in case no hosting UserControl could be found.</returns>
        public static object FindUserControlOrForm(this Control AControl, bool AIfControlIsUserControlReturnThis = false,
            bool AIfControlIsTabControlReturnChildUserControl = false)
        {
            Control ControlSoughtFor;

            if (AControl == null)
            {
                return null;
            }

            ControlSoughtFor = AControl.Parent;

            if ((AControl is UserControl)
                && (!(AControl is TtxtAutoPopulatedButtonLabel))
                && AIfControlIsUserControlReturnThis)
            {
                return AControl;
            }

            if (!((AControl is System.Windows.Forms.TabControl)
                  && (AIfControlIsTabControlReturnChildUserControl)))
            {
                // Iterate through all Parent Controls to find a UserControl, failing that a Form
                while ((!(ControlSoughtFor is UserControl))
                       && (!(ControlSoughtFor is Form)))
                {
                    ControlSoughtFor = ControlSoughtFor.Parent;
                }
            }
            else
            {
                // Check if there is only one Child Control on the currently selected TabPage and
                // that this Child Control is indeed a UserControl
                if ((((TabControl)AControl).SelectedTab.Controls.Count == 1)
                    && (((TabControl)AControl).SelectedTab.Controls[0] is UserControl))
                {
                    ControlSoughtFor = ((TabControl)AControl).SelectedTab.Controls[0];
                }
                else
                {
                    ControlSoughtFor = null;
                }
            }

            if (ControlSoughtFor != AControl)
            {
                return ControlSoughtFor;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the UserControl that hosts <paramref name="AControl" />.
        /// </summary>
        /// <param name="AControl">Control to find the hosting UserControl for.</param>
        /// <returns>The UserControl that hosts <paramref name="AControl" /> or null
        /// in case no hosting UserControl could be found.</returns>
        public static UserControl FindUserControl(this Control AControl)
        {
            object FindResult = FindUserControlOrForm(AControl);

            if (FindResult != null)
            {
                if (FindResult is UserControl)
                {
                    return (UserControl)FindResult;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a friendly string from the column name of a data column (eg pt_internat_postal_code_c).
        /// If AIncludeSpaces is true it would return 'Internat Postal Code', otherwise 'internatpostalcode'
        /// </summary>
        /// <param name="ADataColumn">The data column to work with (often a primary key column)</param>
        /// <param name="AIncludeSpaces">If true returs eg 'Internat Postal Code'.  If false returns eg 'internatpostalcode'</param>
        /// <returns>The appropriate string</returns>
        public static string DataColumnNameToFriendlyName(System.Data.DataColumn ADataColumn, Boolean AIncludeSpaces)
        {
            if (ADataColumn == null)
            {
                return String.Empty;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // for example pt_internat_postal_code_c
            string primaryKeyName = ADataColumn.ToString();

            // strip off the leading p_
            string work = primaryKeyName.Substring(primaryKeyName.IndexOf('_') + 1);

            // strip off the trailing _c
            work = work.Substring(0, work.LastIndexOf('_'));

            if (AIncludeSpaces)
            {
                // Swap _ for space and capitalise the next character
                work = work.Replace("_", " ");
                sb.Append(work.Substring(0, 1).ToUpper());
                work = work.Substring(1);
                int pos = work.IndexOf(' ');

                while (pos >= 0)
                {
                    sb.Append(work.Substring(0, pos + 1));
                    sb.Append(work.Substring(pos + 1, 1).ToUpper());
                    work = work.Substring(pos + 2);
                    pos = work.IndexOf(' ');
                }

                sb.Append(work);
            }
            else
            {
                //just replace _ with blank
                sb.Append(work.Replace("_", String.Empty));
            }

            return sb.ToString();
        }

        /// <summary>
        /// A method that returns the Label and Data controls corresponding to the specified Primary Key Data Column.  This method is used
        /// by ValidateNonDuplicateRecord but also on each screen to locate a pair of controls for a 'prime' primary key.
        /// </summary>
        /// <param name="APrimaryKey">The Data Column that is one of the primary keys</param>
        /// <param name="AHostControl">The control (often a details panel) that hosts the controls to be searched for.</param>
        /// <param name="ALabelControl">The Label control associated with the primary key data column</param>
        /// <param name="ADataControl">The data control associated with the primary key data column</param>
        /// <returns>True if both the label and data control were found</returns>
        public static bool GetControlsForPrimaryKey(System.Data.DataColumn APrimaryKey, System.Windows.Forms.Control AHostControl,
            out System.Windows.Forms.Label ALabelControl, out System.Windows.Forms.Control ADataControl)
        {
            bool bFound = false;
            string primaryKeyColName = /*"detail" + */ DataColumnNameToFriendlyName(APrimaryKey, false);

            ALabelControl = null;
            ADataControl = null;

            for (int i = 0; i < AHostControl.Controls.Count; i++)
            {
                string controlName = AHostControl.Controls[i].Name;

                if (controlName.StartsWith("pnl") || controlName.StartsWith("grp"))
                {
                    // Call ourself recursively to see if the desired control is on a sub-panel
                    Label subLabelControl = null;
                    Control subDataControl = null;

                    if (GetControlsForPrimaryKey(APrimaryKey, AHostControl.Controls[i], out subLabelControl, out subDataControl))
                    {
                        ALabelControl = subLabelControl;
                        ADataControl = subDataControl;
                        bFound = true;
                        break;
                    }
                }
                else if (controlName.ToLower().EndsWith(primaryKeyColName))
                {
                    if (controlName.StartsWith("lbl") && (AHostControl.Controls[i].GetType() == typeof(System.Windows.Forms.Label)))
                    {
                        ALabelControl = (System.Windows.Forms.Label)AHostControl.Controls[i];
                    }
                    else
                    {
                        ADataControl = AHostControl.Controls[i];
                    }

                    if ((ALabelControl != null) && (ADataControl != null))
                    {
                        bFound = true;
                        break;
                    }
                }
            }

            return bFound;
        }

        /// <summary>
        /// Gets the display value that is in the specified control
        /// </summary>
        /// <param name="AControl">The control</param>
        /// <returns>The display text, or an empty string if the text cannot be determined</returns>
        public static string GetDisplayTextForControl(Control AControl)
        {
            string ret = String.Empty;

            if (AControl == null)
            {
                return ret;
            }

            if (AControl.GetType() == typeof(TtxtPetraDate))
            {
                if (((TtxtPetraDate)AControl).Date.HasValue)
                {
                    return ((TtxtPetraDate)AControl).Date.Value.ToString("dd-MMM-yyyy").ToUpper();
                }
            }
            else if (AControl.GetType() == typeof(TCmbAutoComplete))
            {
                return ((TCmbAutoComplete)AControl).GetSelectedString();
            }
            else if (AControl.GetType() == typeof(TCmbAutoPopulated))
            {
                return ((TCmbAutoPopulated)AControl).GetSelectedString();
            }
            else if (AControl.GetType() == typeof(TCmbLabelled))
            {
                return ((TCmbLabelled)AControl).GetSelectedString();
            }
            else if (AControl.GetType() == typeof(TCmbVersatile))
            {
                return ((TCmbVersatile)AControl).GetSelectedString();
            }
            else if (AControl.GetType() == typeof(TTxtPartnerKeyTextBox))
            {
                return ((TTxtPartnerKeyTextBox)AControl).PartnerKey.ToString();
            }
            else if (AControl.GetType() == typeof(TTxtNumericTextBox))
            {
                if (((TTxtNumericTextBox)AControl).ControlMode == TTxtNumericTextBox.TNumericTextBoxMode.Integer)
                {
                    return ((TTxtNumericTextBox)AControl).NumberValueInt.ToString();
                }
                else if (((TTxtNumericTextBox)AControl).ControlMode == TTxtNumericTextBox.TNumericTextBoxMode.LongInteger)
                {
                    return ((TTxtNumericTextBox)AControl).NumberValueLongInt.ToString();
                }
                else if (((TTxtNumericTextBox)AControl).ControlMode == TTxtNumericTextBox.TNumericTextBoxMode.Decimal)
                {
                    return ((TTxtNumericTextBox)AControl).NumberValueDecimal.ToString();
                }
            }
            else if (AControl.GetType() == typeof(TTxtCurrencyTextBox))
            {
                return ((TTxtNumericTextBox)AControl).NumberValueDecimal.ToString();
            }
            else if (AControl.GetType() == typeof(TextBox))
            {
                return ((TextBox)AControl).Text;
            }

            return ret;
        }

        /// <summary>
        /// Handles validation of a duplicate or non-duplicate record entered in the GUI
        /// </summary>
        /// <param name="AHostContext">Context that describes where the data validation occurs (usually specified as 'this').</param>
        /// <param name="AConstraintExceptionOccurred">Set to True if a constraint exception occurred when saving the data or False otherwise</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="APrimaryKeyColumn">The data column that will be used to identify the error (usually the first of the primary key columns)</param>
        /// <param name="APrimaryKeyControl">The control corresponding to the Primary Key column</param>
        /// <param name="APrimaryKeys">The array of primary key data columns that define the unique constraint being validated</param>
        public static void ValidateNonDuplicateRecord(object AHostContext,
            bool AConstraintExceptionOccurred,
            TVerificationResultCollection AVerificationResultCollection,
            System.Data.DataColumn APrimaryKeyColumn,
            System.Windows.Forms.Control APrimaryKeyControl,
            System.Data.DataColumn[] APrimaryKeys)
        {
            TVerificationResult verificationResult = null;
            string resultText = String.Empty;

            if (AConstraintExceptionOccurred)
            {
                // Work out what the current user input values are for the primary keys
                ErrCodeInfo errInfo = ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_DUPLICATE_RECORD);
                resultText = errInfo.ErrorMessageText;

                string hintText = String.Empty;
                bool bFoundOne = false;

                foreach (System.Data.DataColumn column in APrimaryKeys)
                {
                    // Look at each primary key name and find its control.  It is quite common for one key (eg Ledger number) to not have a control.
                    System.Windows.Forms.Label label;
                    System.Windows.Forms.Control control;
                    string controlText = String.Empty;

                    if (GetControlsForPrimaryKey(column, (System.Windows.Forms.Control)AHostContext, out label, out control))
                    {
                        bFoundOne = true;
                        hintText += Environment.NewLine;
                        hintText += label.Text.Replace("&", String.Empty);
                        controlText = GetDisplayTextForControl(control);

                        if (controlText != String.Empty)
                        {
                            // Note from Alan:  I may not have implemented getting control text for all control types
                            // If you find a missing type, please add it to GetDisplayTextForControl()
                            // In the meantime we have to ignore empty text and just display the label text...
                            if (!hintText.EndsWith(":"))
                            {
                                hintText += ":";
                            }

                            hintText += " ";
                            hintText += controlText;
                        }
                    }
                }

                if (!bFoundOne)
                {
                    // See Alan's note above.  This will occur on a form that has no control type that has GetDisplayTextForControl()
                    hintText += Environment.NewLine;
                    hintText += Environment.NewLine;
                    hintText +=
                        String.Format(Catalog.GetString(
                                "No hint text is available for the following screen:{0}{1}.{0}Please inform the OpenPetra team if you see this message."),
                            Environment.NewLine, AHostContext.ToString());
                }

                resultText += hintText;
            }

            verificationResult = TGuiChecks.ValidateNonDuplicateRecord(AHostContext,
                AConstraintExceptionOccurred,
                resultText,
                APrimaryKeyColumn,
                APrimaryKeyControl);

            // Add or remove the error from the collection
            AVerificationResultCollection.AddOrRemove(verificationResult, APrimaryKeyColumn);
        }

        /// <summary>
        /// Retrieves the list of currencies from the Cache.
        /// </summary>
        /// <returns></returns>
        public static DataTable RetrieveCurrencyList()
        {
            return TDataCache.TMCommon.GetCacheableCommonTable(TCacheableCommonTablesEnum.CurrencyCodeList);
        }

        /// <summary>
        /// Retrieves a boolean user default value from the TUserDefaults.
        /// </summary>
        /// <returns></returns>
        public static Boolean RetrieveUserDefaultBoolean(String AKey, Boolean ADefault)
        {
            return TUserDefaults.GetBooleanDefault(AKey, ADefault);
        }
    }
}