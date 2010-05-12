//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using System.Windows.Forms;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Gui;

namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// Contains routines that help with DataBinding.
    ///
    /// </summary>
    public class TDataBinding : System.Object
    {
        /// <summary>
        /// Sets the Focus to a databound control within a given Form or a UserControl by
        /// specifying the column to which it is bound.
        ///
        /// </summary>
        /// <param name="AContainerControl">Either a Form or a UserControl.</param>
        /// <param name="ABindingManagerBase">BindingManagerBase where the data binding
        /// information is stored</param>
        /// <param name="AColumnName">Name of the column whose databound control should get the
        /// focus.</param>
        /// <returns>Name of the control, or empty string if not found.
        /// </returns>
        public static String SetFocusOnDataBoundControlInternal(ContainerControl AContainerControl,
            BindingManagerBase ABindingManagerBase,
            String AColumnName)
        {
            Int16 Counter1;
            String ControlName;

            ControlName = "";

            // MessageBox.Show('SetFocusOnDataBoundControlInternal: looking for control that belongs to DataColumn ' + AColumnName + '...');
            // MessageBox.Show('Number of Bindings: ' + ABindingManagerBase.Bindings.Count.ToString);
            for (Counter1 = 0; Counter1 <= ABindingManagerBase.Bindings.Count - 1; Counter1 += 1)
            {
                // MessageBox.Show('ABindingManagerBase.Bindings.Item[Counter1].BindingMemberInfo.BindingField: ' + ABindingManagerBase.Bindings.Item[Counter1].BindingMemberInfo.BindingField);
                if (ABindingManagerBase.Bindings[Counter1].BindingMemberInfo.BindingField == AColumnName)
                {
                    // MessageBox.Show('BmbPartnerLocation.Bindings.Item[Counter1].Control.Name: ' + ABindingManagerBase.Bindings.Item[Counter1].Control.Name);
                    ControlName = TFocusing.SetFocusOnControlInFormOrUserControl(AContainerControl,
                        ABindingManagerBase.Bindings[Counter1].Control.Name);
                    break;
                }
            }

            return ControlName;
        }

        /// <summary>
        /// Sets the Focus to a databound control within a given Form or a UserControl by
        /// specifying the column to which it is bound.
        ///
        /// Internally it uses SetFocusOnDataBoundControlInternal to set the focus.
        ///
        /// </summary>
        /// <param name="AContainer">Either a Form or a UserControl.</param>
        /// <param name="ADataView">DataView in which the column is to be found.</param>
        /// <param name="AColumnName">Name of the column whose databound control should get the
        /// focus.</param>
        /// <returns>Name of the control, or empty string if not found.
        /// </returns>
        public static String SetFocusOnDataBoundControl(ContainerControl AContainer, DataView ADataView, String AColumnName)
        {
            BindingManagerBase Tmp;

            Tmp = AContainer.BindingContext[ADataView];
            return TDataBinding.SetFocusOnDataBoundControlInternal(AContainer, Tmp, AColumnName);
        }

        /// <summary>
        /// Sets the text of ColumnError.
        ///
        /// </summary>
        /// <param name="AEventArgs">Event arguments of DataColumnChange event.</param>
        /// <param name="AVerificationResultEntry">A VerificationResultEntry that holds
        /// information about the error.</param>
        /// <param name="AControlName">Name of the control on the form that is associated with
        /// the errenous DataColumn.
        /// </param>
        /// <returns>void</returns>
        public static void SetColumnErrorText(DataColumnChangeEventArgs AEventArgs, TVerificationResult AVerificationResultEntry, string AControlName)
        {
            AEventArgs.Row.SetColumnError(AEventArgs.Column, AVerificationResultEntry.ResultText + "//[[" + AControlName + "]]");
        }

        /// <summary>
        /// Sets the Focus to a databound control within a given Form or a UserControl by
        /// specifying the column to which it is bound.
        ///
        /// Internally it uses SetFocusOnDataBoundControlInternal to set the focus.
        ///
        /// </summary>
        /// <param name="AContainer">Either a Form or a UserControl.</param>
        /// <param name="ADataTable">DataTable in which the column is to be found.</param>
        /// <param name="AColumnName">Name of the column whose databound control should get the
        /// focus.</param>
        /// <returns>Name of the control, or empty string if not found.
        /// </returns>
        public static String SetFocusOnDataBoundControl(ContainerControl AContainer, DataTable ADataTable, String AColumnName)
        {
            BindingManagerBase Tmp;

            Tmp = AContainer.BindingContext[ADataTable];
            return TDataBinding.SetFocusOnDataBoundControlInternal(AContainer, Tmp, AColumnName);
        }

        /// <summary>
        /// Makes sure that DataBinding writes the value of the active Control to the
        /// underlying DataSource - without leaving the Control!
        ///
        /// @comment The active Control must be DataBound for this to work, of course. If
        /// it isn't, nothing happens and no error is given.
        ///
        /// </summary>
        /// <param name="AContainerControl">A ContainterControl (eg. Form, UserControl, Panel)
        /// that contains controls
        /// </param>
        /// <returns>void</returns>
        public static void EnsureDataChangesAreStored(ContainerControl AContainerControl)
        {
            Control InspectedControl;

            while (true)
            {
                InspectedControl = AContainerControl.ActiveControl;

                // MessageBox.Show('Inspecting control: ' + InspectedControl.Name + '; Type: ' + InspectedControl.GetType().ToString);
                if (!(InspectedControl == null))
                {
                    if (!(InspectedControl is ContainerControl))
                    {
                        // MessageBox.Show('Found nonContainerControl: ' + InspectedControl.Name);
                        foreach (Binding TheBinding in InspectedControl.DataBindings)
                        {
                            // this ensures each control updates its datasource
                            // even if the user is "IN" the control
                            TheBinding.BindingManagerBase.EndCurrentEdit();
                        }

                        return;
                    }
                    else
                    {
                        // recursive call!
                        EnsureDataChangesAreStored((ContainerControl)InspectedControl);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Returns the Control that is bound to the specified DataColumn.
        ///
        /// </summary>
        /// <param name="ABindingManagerBase">A BindingManagerBase. Such an Object can be
        /// retrieved e.g. by such a code: BindingContext[FMainDS.PPartner]</param>
        /// <param name="AColumn">The DataColumn to which the Control should be found</param>
        /// <returns>The Control, if it could be found, otherwise nil.
        /// </returns>
        public static Control GetBoundControlForColumn(BindingManagerBase ABindingManagerBase, DataColumn AColumn)
        {
            int Counter;

            // MessageBox.Show('ABindingManagerBase.Bindings.Count: ' + ABindingManagerBase.Bindings.Count.ToSTring);
            for (Counter = 0; Counter <= ABindingManagerBase.Bindings.Count - 1; Counter += 1)
            {
                // MessageBox.Show('Bound Control # ' + Counter.ToString + ': ' + ABindingManagerBase.Bindings[Counter].Control.ToString + '; bound to field: ' + ABindingManagerBase.Bindings[Counter].BindingMemberInfo.BindingField.ToString);
                if (ABindingManagerBase.Bindings[Counter].BindingMemberInfo.BindingField == AColumn.ToString())
                {
                    return ABindingManagerBase.Bindings[Counter].Control;
                }
            }

            return null;
        }

        /// <summary>
        /// Builds a string out of DataColumns that have errors and gives the name of the
        /// column where the first error is stored.
        ///
        /// </summary>
        /// <param name="ADataRow">DataRow in which the DataColumn errors should be iterated.</param>
        /// <param name="AErrorMessages">String containing all DataColumns that have errors,
        /// separated by two CR+LF's.</param>
        /// <param name="AFirstErrorControlName">Name of the DataColumn where the first error is
        /// stored.
        /// </param>
        /// <returns>void</returns>
        public static void IterateErrorsInData(DataRow ADataRow, out String AErrorMessages, out String AFirstErrorControlName)
        {
            string ErrorMessage;
            int ErrorCounter;

            DataColumn[] ErrorColumns;
            AErrorMessages = "";
            AFirstErrorControlName = "";

            ErrorColumns = ADataRow.GetColumnsInError();

            for (ErrorCounter = 0; ErrorCounter <= ErrorColumns.Length; ErrorCounter += 1)
            {
                ErrorMessage = ADataRow.GetColumnError(ErrorColumns[ErrorCounter]);

                if (ErrorCounter == 0)
                {
                    AFirstErrorControlName =
                        ErrorMessage.Substring(ErrorMessage.IndexOf("//[[") + 4, ErrorMessage.IndexOf("]]") - (ErrorMessage.IndexOf("//[[") + 4));
                }

                ErrorMessage = ErrorMessage.Substring(0, ErrorMessage.IndexOf("//[["));
                AErrorMessages = AErrorMessages + ErrorMessage + Environment.NewLine + Environment.NewLine;
            }
        }

        /// <summary>
        /// Builds a string out of DataColumns that have errors and gives the name of the
        /// column where the first error is stored.
        ///
        /// </summary>
        /// <param name="ADataTable">DataTable in which the DataColumn errors or all DataRows
        /// should be iterated.</param>
        /// <param name="AErrorMessages">String containing all DataColumns that have errors,
        /// separated by two CR+LF's.</param>
        /// <param name="AFirstErrorControlName">Name of the DataColumn where the first error is
        /// stored.
        /// </param>
        /// <returns>void</returns>
        public static void IterateErrorsInData(DataTable ADataTable, out String AErrorMessages, out String AFirstErrorControlName)
        {
            DataRow[] ErrorRows;
            int ErrorCounter;
            String ErrorMessages;
            String FirstErrorControlName;
            AErrorMessages = "";
            AFirstErrorControlName = "";
            ErrorRows = ADataTable.GetErrors();

            for (ErrorCounter = 0; ErrorCounter <= ErrorRows.Length; ErrorCounter += 1)
            {
                IterateErrorsInData(ErrorRows[ErrorCounter], out ErrorMessages, out FirstErrorControlName);

                // MessageBox.Show('TDataBinding.IterateErrorsInData(DataTable).FirstErrorControlName: ' + FirstErrorControlName);
                AErrorMessages = AErrorMessages + ErrorMessages;

                if (ErrorCounter == 0)
                {
                    AFirstErrorControlName = FirstErrorControlName;
                }
            }
        }

        /// <summary>
        /// Builds a string out of DataColumns that have errors and gives the name of the
        /// column where the first error is stored.
        ///
        /// </summary>
        /// <param name="ADataSet">DataSet in which the DataColumn errors of all DataTables
        /// should be iterated.</param>
        /// <param name="AErrorMessages">String containing all DataColumns that have errors,
        /// separated by two CR+LF's.</param>
        /// <param name="AFirstErrorControlName">Name of the DataColumn where the first error is
        /// stored.
        /// </param>
        /// <returns>void</returns>
        public static void IterateErrorsInData(DataSet ADataSet, out String AErrorMessages, out String AFirstErrorControlName)
        {
            int ErrorCounter;
            String ErrorMessages;
            String FirstErrorControlName;

            AErrorMessages = "";
            AFirstErrorControlName = "";

            for (ErrorCounter = 0; ErrorCounter <= ADataSet.Tables.Count - 1; ErrorCounter += 1)
            {
                IterateErrorsInData(ADataSet.Tables[ErrorCounter], out ErrorMessages, out FirstErrorControlName);

                // MessageBox.Show('TDataBinding.IterateErrorsInData(DataSet).FirstErrorControlName: ' + FirstErrorControlName);
                AErrorMessages = AErrorMessages + ErrorMessages;

                if ((FirstErrorControlName != "") && (AFirstErrorControlName == ""))
                {
                    AFirstErrorControlName = FirstErrorControlName;
                }
            }
        }
    }
}