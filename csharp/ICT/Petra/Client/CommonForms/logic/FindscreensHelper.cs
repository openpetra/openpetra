//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2015 by OM International
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

using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using SourceGrid;

namespace Ict.Petra.Client.CommonForms.Logic
{
    /// <summary>
    /// Helper Class for Find Screens.
    /// </summary>
    public static class TFindscreensHelper
    {
        #region Wildcard processing-related Helper Methods

        /// <summary>
        /// Replaces any * character(s) in the middle of a Search Criteria's text with % character(s)
        /// to make the SQL-92 'LIKE' operator do what the user intended. The only case when this isn't done is
        /// when the Search Criteria's text starts with || AND ends with ||. This signalises that the
        /// Search Criteria's text is to be taken absolutely literally, that is, wild card characters are
        /// to be taken as the characters they really are, and not as wildcards. Also removes any leading or
        /// trailing || characters ('stops').
        /// </summary>
        /// <param name="ASearchCriteria">Text value of a Search Criteria.</param>
        /// <returns>Text value of a Search Criteria with all necessary replacements applied.</returns>
        public static string ReplaceWildCardsInMiddleOfSearchCriteriaAndRemoveStops(string ASearchCriteria)
        {
            // In case the Search Criteria's text is not to be taken absolutely literally:
            if (!(ASearchCriteria.StartsWith("||")
                  && ASearchCriteria.EndsWith("||")))
            {
                for (int Counter = 1; Counter < ASearchCriteria.Length - 1; Counter++)
                {
                    if (ASearchCriteria[Counter] == '*')
                    {
                        ASearchCriteria = ASearchCriteria.Substring(0, Counter) +
                                          '%' + ASearchCriteria.Substring(Counter + 1, ASearchCriteria.Length - (Counter + 1));
                    }
                }
            }

            // Remove any leading || character sequence occurence (used only when the Search Criteria's text starts with || AND ends with ||)
            if (ASearchCriteria.StartsWith("||"))
            {
                ASearchCriteria = ASearchCriteria.Substring(2);
            }

            // Remove any trailing || character sequence occurence (used to signalise that no % should be appended automatically to the Search Criteria's text)
            if (ASearchCriteria.EndsWith("||"))
            {
                ASearchCriteria = ASearchCriteria.Substring(0, ASearchCriteria.Length - 2);
            }

            return ASearchCriteria;
        }

        /// <summary>
        /// Leave Handler for a Critera TextBox Control.
        /// </summary>
        /// <param name="AFindCriteriaDataTable"></param>
        /// <param name="ATextBox"></param>
        /// <param name="ACriteriaControl"></param>
        public static void CriteriaTextBoxLeaveHandler(DataTable AFindCriteriaDataTable, TextBox ATextBox, SplitButton ACriteriaControl)
        {
            TMatches NewMatchValue = TMatches.BEGINS;
            string TextBoxText = ATextBox.Text;
            string CriteriaValue;

            //            TLogging.Log("GeneralLeaveHandler for " + ATextBox.Name + ". SplitButton: " + ACriteriaControl.Name);

            if (TextBoxText.Contains("*")
                || (TextBoxText.Contains("%"))
                || (TextBoxText.EndsWith("||")))
            {
                if (TextBoxText.EndsWith("||")
                    && !(TextBoxText.StartsWith("||")))
                {
//                    TLogging.Log(ATextBox.Name + " ends with ||  = ENDS");
                    NewMatchValue = TMatches.ENDS;
                }
                else if (TextBoxText.EndsWith("||")
                         && (TextBoxText.StartsWith("||")))
                {
//                        TLogging.Log(ATextBox.Name + " begins with || and ends with ||  = EXACT");
                    NewMatchValue = TMatches.EXACT;
                }
                else if (TextBoxText.EndsWith("*")
                         && !(TextBoxText.StartsWith("*")))
                {
//                    TLogging.Log(ATextBox.Name + " ends with *  = BEGINS");
                    NewMatchValue = TMatches.BEGINS;
                }
                else if ((TextBoxText.EndsWith("*")
                          && (TextBoxText.StartsWith("*")))
                         || (TextBoxText.StartsWith("*")))
                {
//                    TLogging.Log(ATextBox.Name + " begins and ends with *, or begins with *  = CONTAINS");
                    NewMatchValue = TMatches.CONTAINS;
                }

                /*
                 * See what the Criteria Value would be without any 'joker' characters
                 * ( * and % ).
                 */
                CriteriaValue = TextBoxText.Replace("*", String.Empty);
                CriteriaValue = CriteriaValue.Replace("%", String.Empty);

                if (CriteriaValue != String.Empty)
                {
                    // There is still a valid CriteriaValue
                    PutNewMatchValueIntoFindCriteriaDT(AFindCriteriaDataTable, ACriteriaControl, NewMatchValue, ATextBox, TextBoxText);
                }
                else
                {
                    // No valid Criteria Value, therefore empty the TextBox's Text.
                    ATextBox.Text = String.Empty;
                }
            }
            else
            {
                // Ensure that 'BEGINS' is restored in case the user used the '*' joker before but
                // has cleared it now!
                PutNewMatchValueIntoFindCriteriaDT(AFindCriteriaDataTable, ACriteriaControl, NewMatchValue, ATextBox, TextBoxText);
            }
        }

        private static void PutNewMatchValueIntoFindCriteriaDT(DataTable AFindCriteriaDataTable, SplitButton ACriteriaControl, TMatches NewMatchValue,
            TextBox ATextBox, string ATextBoxText)
        {
            AFindCriteriaDataTable.Rows[0].BeginEdit();
            ACriteriaControl.SelectedValue = Enum.GetName(typeof(TMatches), NewMatchValue);
            AFindCriteriaDataTable.Rows[0].EndEdit();

            //TODO: It seems databinding is broken on this control
            // this needs to happen in the SplitButton control really
            string fieldname = ((SplitButton)ACriteriaControl).DataBindings[0].BindingMemberInfo.BindingMember;
            AFindCriteriaDataTable.Rows[0][fieldname] = Enum.GetName(typeof(TMatches), NewMatchValue);

            //TODO: DataBinding is really doing strange things here; we have to
            //assign the just entered Text again, otherwise it is lost!!!
            ATextBox.Text = ATextBoxText;
        }

        /// <summary>
        /// Removes any 'Joker' characters from a Criteria TextBox
        /// </summary>
        /// <param name="AFindCriteriaDataTable"></param>
        /// <param name="ASplitButton"></param>
        /// <param name="AAssociatedTextBox"></param>
        /// <param name="ALastSelection"></param>
        public static void RemoveJokersFromTextBox(DataTable AFindCriteriaDataTable, SplitButton ASplitButton,
            TextBox AAssociatedTextBox, TMatches ALastSelection)
        {
            string NewText;

            try
            {
                if (AAssociatedTextBox != null)
                {
                    // Remove * Joker character(s)
                    NewText = AAssociatedTextBox.Text.Replace("*", String.Empty);

                    // If an EXACT search is wanted, we need to remove the % Joker character(s) as well
                    if (ALastSelection == TMatches.EXACT)
                    {
                        NewText = NewText.Replace("%", String.Empty);
                    }

//                    TLogging.Log(
//                        "RemoveJokersFromTextBox:  Associated TextBox's (" + AAssociatedTextBox.Name + ") Text (1): " + AAssociatedTextBox.Text);
//                    AFindCriteriaDataTable.Rows[0].BeginEdit();
//                    AAssociatedTextBox.Text = NewText;
//                    AFindCriteriaDataTable.Rows[0].EndEdit();

                    string fieldname = ((TextBox)AAssociatedTextBox).DataBindings[0].BindingMemberInfo.BindingMember;
                    AFindCriteriaDataTable.Rows[0][fieldname] = NewText;
                    fieldname = ((SplitButton)ASplitButton).DataBindings[0].BindingMemberInfo.BindingMember;
                    AFindCriteriaDataTable.Rows[0][fieldname] = Enum.GetName(typeof(TMatches), ALastSelection);
//                    AFindCriteriaDataTable.Rows[0].EndEdit();

//
//                    AAssociatedTextBox.Text = NewText;
//
//                    TLogging.Log(
//                        "RemoveJokersFromTextBox:  Associated TextBox's (" + AAssociatedTextBox.Name + ") Text (2): " + AAssociatedTextBox.Text);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception in RemoveJokersFromTextBox: " + exp.ToString());
            }
        }

        #endregion

        #region SourceGrid-related Helper Methods

        /// <summary>
        /// Sets up the DataBinding of the Grid.
        /// </summary>
        public static void SetupDataGridDataBinding(TSgrdDataGridPaged ADataGrid, DataTable APagedDataTable,
            RowEventHandler AFocusRowEnteredHandler = null)
        {
            DataView FindResultPagedDV;

            FindResultPagedDV = APagedDataTable.DefaultView;
            FindResultPagedDV.AllowNew = false;
            FindResultPagedDV.AllowDelete = false;
            FindResultPagedDV.AllowEdit = false;
            ADataGrid.DataSource = new DevAge.ComponentModel.BoundDataView(FindResultPagedDV);

            if (AFocusRowEnteredHandler != null)
            {
                // Hook up event that fires when a different Row is selected
                ADataGrid.Selection.FocusRowEntered += new RowEventHandler(AFocusRowEnteredHandler);
            }
        }

        /// <summary>
        /// Sets up the visual appearance of the Grid.
        /// </summary>
        /// <remarks><em>Caution:</em>Do not call this Method with <paramref name="AAutoSizeCells" /> set to true
        /// if the Grid holds more than a few hundred Rows, as the Grid will take quite a time for the auto-sizing
        /// calculation!</remarks>
        public static void SetupDataGridVisualAppearance(TSgrdDataGridPaged ADataGrid, bool AAutoSizeCells = true)
        {
            // make the border to the right of the fixed columns bold
            ((TSgrdTextColumn)ADataGrid.Columns[2]).BoldRightBorder = true;

// TLogging.Log("ADataGrid.Rows.Count: " + ADataGrid.Rows.Count.ToString());

            if (AAutoSizeCells)
            {
                ADataGrid.AutoResizeGrid();
            }
        }

        #endregion
    }
}