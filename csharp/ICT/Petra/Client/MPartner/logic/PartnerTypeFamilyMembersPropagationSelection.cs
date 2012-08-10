//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, markusm
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
using Ict.Common.Data; // Implicit reference
using Ict.Common.Controls;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Interfaces.MPartner;
using DevAge.Drawing;
using DevAge.ComponentModel;
using DevAge.ComponentModel.Converter;
using DevAge.ComponentModel.Validator;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Views;
using System.ComponentModel;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// Contains logic for the PartnerType Add/Remove Family Members Propagation Selection Dialog.
    /// </summary>
    public class TPartnerTypeFamilyMembersPropagationSelectionLogic
    {
        #region Resourcestrings

        private static readonly string StrWarningOnColumnChangingAdd = Catalog.GetString(
            "It is not possible to add this Special Type to this person because\r\n" +
            "this person already has got this Special Type assigned!");

        private static readonly string StrWarningOnColumnChangingRemove = Catalog.GetString(
            "It is not possible to remove this Special Type from this person\r\n" +
            "because this person doesn't have this Special Type assigned!");

        private static readonly string StrMessageBoxTitleWarning = Catalog.GetString("Action Not Possible");

        private static readonly string StrPartnerHasCostCentreLink = Catalog.GetString(
            "This partner is linked to a Cost Centre ({0}) in the\r\n" +
            "Finance Module.  Remove the link before deleting\r\nthis Special Type.");

        private static readonly string StrPartnerHasCostCentreLinkTitle = Catalog.GetString("Cannot remove Special Type");

        #endregion

        private ControllerBase FSpecialCellController = null;
        private PartnerEditTDSFamilyMembersTable FFamilyMembersDT;
        private System.Data.DataView FFamilyMembersDV;
        private PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable FFamilyMembersResultDT;
        private String FAction;
        private String FTypeCode;

        /// <summary>Column names for the FFamilyMembersDT table. They get initialised in InitialisePartnerTypeFamilyMembers.</summary>
        private System.String FTypeCodeModifyName;
        private String FTypeCodePresentName;
        private String FPartnerKeyName;

        /// <summary>Column names for the FFamilyMembersResultDT table. They get initialised in InitialisePartnerTypeFamilyMembers.</summary>
        private String FResultPartnerKeyName;
        private String FResultTypeCodeName;
        private String FResultAddTypeCodeName;
        private String FResultRemoveTypeCodeName;
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private TSgrdDataGrid FDataGrid;

        /// <summary>
        /// This property handles the datasource of this dialogue
        ///
        /// </summary>
        public System.Data.DataView FamilyMembersDV
        {
            get
            {
                return FFamilyMembersDV;
            }

            set
            {
                FFamilyMembersDV = value;
            }
        }

        /// <summary>
        /// This property handles the TypeCode property
        ///
        /// </summary>
        public String TypeCode
        {
            get
            {
                return this.FTypeCode;
            }

            set
            {
                this.FTypeCode = value;
            }
        }

        /// <summary>
        /// This property handles the TypeCode property
        ///
        /// </summary>
        public TSgrdDataGrid DataGrid
        {
            get
            {
                return this.FDataGrid;
            }

            set
            {
                this.FDataGrid = value;
            }
        }

        /// <summary>
        /// This property handles the PartnerEditUIConnector property
        ///
        /// </summary>
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


        #region TPartnerTypeFamilyMembersPropagationSelectionLogic

        /// <summary>
        /// This procedure adds a new datarow to the result table if the row with the
        /// specified values cannot be found in it.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void AddRowToFamilyMembersPromotionTable(System.Int64 APartnerKey)
        {
            DataRow[] mDataRows;
            System.Data.DataRow mNewRow;
            String mFilter;

            // Assemble Filterexpression
            mFilter = this.FResultPartnerKeyName + " = " + APartnerKey.ToString();

            // Messagebox.Show('AddRowToFamilyMembersPromotionTable FilterString: ' + mFilter);
            // Check whether the datarow exists.
            mDataRows = this.FFamilyMembersResultDT.Select(mFilter);

            if (mDataRows.Length > 0)
            {
            }
            // mInt := Length(mDataRows);
            // messagebox.show('At least one Row exists!!! Number of Rows: ' + mInt.ToString);
            else
            {
                // messagebox.show('Row does not exist!!!');
                mNewRow = this.FFamilyMembersResultDT.NewRowTyped(false);
                mNewRow[this.FResultPartnerKeyName] = APartnerKey;
                mNewRow[this.FResultTypeCodeName] = this.TypeCode;

                if (this.FAction.Equals("ADD"))
                {
                    mNewRow[this.FResultAddTypeCodeName] = true;
                    mNewRow[this.FResultRemoveTypeCodeName] = false;
                }
                else
                {
                    mNewRow[this.FResultAddTypeCodeName] = false;
                    mNewRow[this.FResultRemoveTypeCodeName] = true;
                }

                this.FFamilyMembersResultDT.Rows.Add(mNewRow);
                #region Final Check
                mDataRows = this.FFamilyMembersResultDT.Select(mFilter);

                // mInt := Length(mDataRows);
                // messagebox.show(mInt.ToString + ' Rows.');
                // foreach (DataRow mDataRow in mDataRows)
                // {
                //     string mWhatsInIt = mDataRow[0].ToString() + "; " + mDataRow[1].ToString() + "; " + mDataRow[2].ToString() + "; " + mDataRow[3].ToString();
                //     messagebox.show(mWhatsInIt);
                // }

                #endregion
            }
        }

        /// <summary>
        /// This procedure changes the State of the datarow.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void ChangeCheckedStateForAllRows(bool AMasterState)
        {
            DataRow[] mDataRows;
            String mFilter;
            bool mCurrentState;
            System.Int64 mPartnerKey;
            String mCostCentreLink;
            bool mHasCostCentreLink;

            // mRowNums:      System.Int32;
            // Initialize
            // Build filter string;
            if (this.FAction.Equals("ADD"))
            {
                mFilter = this.FTypeCodePresentName + " = false";
            }
            else
            {
                mFilter = this.FTypeCodePresentName + " = true";
            }

            // messagebox.Show(this.FFamilyMembersDT.TableName);
            mDataRows = this.FFamilyMembersDT.Select(mFilter);

            // mRowNums := Length(mDataRows);
            // Build table of relevant rows
            foreach (DataRow mDataRow in mDataRows)
            {
                // messagebox.Show(mDataRow['p_partner_short_name_c'].ToString);
                // messagebox.Show('Checkbox Value: ' + AMasterState.ToString);
                mCurrentState = (Boolean)mDataRow[this.FTypeCodeModifyName];
                mPartnerKey = (System.Int64)mDataRow[this.FPartnerKeyName];

                // messagebox.Show('PartnerKey: ' + mPartnerKey.ToString);
                mHasCostCentreLink = FPartnerEditUIConnector.HasPartnerCostCentreLink(mPartnerKey, out mCostCentreLink);

                if (AMasterState == true)
                {
                    if (mCurrentState == false)
                    {
                        if (mHasCostCentreLink == false)
                        {
                            mDataRow[this.FTypeCodeModifyName] = (System.Object)((!((Boolean)mDataRow[this.FTypeCodeModifyName])));
                        }
                    }
                }
                else
                {
                    if (mCurrentState == true)
                    {
                        if (mHasCostCentreLink == false)
                        {
                            mDataRow[this.FTypeCodeModifyName] = (System.Object)((!((Boolean)mDataRow[this.FTypeCodeModifyName])));
                        }
                    }
                }
            }

            // messagebox.Show('ChangeCheckedStateForAllRows Rows: ' + mRowNums.ToString);
        }

        /// <summary>
        /// This procedure changes the State of the datarow. This function is called
        /// from the UserControl. It contains several tests to determine whether the
        /// state of a CheckBox in the UserContro should be changed or not. If the state
        /// should be changed this routine marks it for change. If the state should not
        /// be changed a message is displayed. It basically draws a tick mark into the
        /// checkbox if necessary.
        /// </summary>
        /// <param name="ARow">row number.
        /// </param>
        /// <param name="AChanged"></param>
        /// <returns>void</returns>
        public void ChangeCheckedStateForRow(Int32 ARow, out Boolean AChanged)
        {
            // mTypeCodePresent:     System.Boolean;
            // CostCentreLink:       System.String;
            // APartnerKey:          System.Int64;
            // mHasCostCentreLink:   System.Boolean;
            // mChecked:             System.Boolean;
            // Initialization
            // TLogging.Log('Begin of ChangeCheckedStateForRow, ARow: ' + ARow.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('Begin of initialization section of ChangeCheckedStateForRow', [TLoggingType.ToLogfile]);
            // System.Data.DataRow mDataRow = this.FamilyMembersDV[ARow].Row;

            // mTypeCodePresent := System.Convert.ToBoolean(mDataRow[this.FTypeCodePresentName]);
            AChanged = false;

            // APartnerKey := this.DeterminePartnerKeyFromRowNumber(ARow);
            // TLogging.Log('mTypeCodePresent: ' + mTypeCodePresent.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('APartnerKey:      ' + APartnerKey.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('End of initialization section of ChangeCheckedStateForRow', [TLoggingType.ToLogfile]);
            // Apply the changes necessary
            if (this.FAction.Equals("ADD"))
            {
                // TLogging.Log('Row ' + ARow.ToString() + ' marking for adding!', [TLoggingType.ToLogfile]);
                this.FamilyMembersDV[ARow][this.FTypeCodeModifyName] =
                    (System.Object)((!(Boolean)(this.FamilyMembersDV[ARow][this.FTypeCodeModifyName])));

                // TLogging.Log('Row ' + ARow.ToString() + ' marked for adding!', [TLoggingType.ToLogfile]);
                // if (mTypeCodePresent = true) then
                // begin
                // This case is ADD / TypeCodePresent = true. In this case we have to display a warning.
                // TLogging.Log('This case is ADD / TypeCodePresent = true. In this case we have to display a warning. ' + StrWarningOnColumnChangingAdd, [TLoggingType.ToLogfile]);
                // Messagebox.Show(StrWarningOnColumnChangingAdd,
                // StrMessageBoxTitleWarning, MessageBoxButtons.OK,
                // MessageBoxIcon.Information);
                // end
                // else
                // begin
                // This case is ADD / TypeCodePresent = false. In this case we should make the changes.
                // TLogging.Log('This case is ADD / TypeCodePresent = false. In this case we should make the changes.', [TLoggingType.ToLogfile]);
                // this.FamilyMembersDV[ARow][this.FTypeCodeModifyName] := (not (this.FamilyMembersDV[ARow][this.FTypeCodeModifyName] as Boolean)) as System.Object;
                // Messagebox.show('Hello from ChangeCheckedStateForRow: Add special type!');
                // AChanged := true;
                // end;
            }
            else
            {
                // TLogging.Log('Row ' + ARow.ToString() + ' marking for removing!', [TLoggingType.ToLogfile]);
                this.FamilyMembersDV[ARow][this.FTypeCodeModifyName] =
                    (System.Object)((!(Boolean)(this.FamilyMembersDV[ARow][this.FTypeCodeModifyName])));

                // TLogging.Log('Row ' + ARow.ToString() + ' marked for removing!', [TLoggingType.ToLogfile]);
                // if (mTypeCodePresent = false) then
                // begin
                // This case is REMOVE / TypeCodePresent = false. In this case we have to display a warning.
                // TLogging.Log('This case is REMOVE / TypeCodePresent = false. In this case we have to display a warning. ' + StrWarningOnColumnChangingRemove, [TLoggingType.ToLogfile]);
                // Messagebox.Show(StrWarningOnColumnChangingRemove, StrMessageBoxTitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // end
                // else
                // begin
                // TLogging.Log('TypeCode: ' + Typecode, [TLoggingType.ToLogfile]);
                // perform check: If COSTCENTRE is to be removed then check whether Partner has a link to costcentre set up
                // if TypeCode = "COSTCENTRE" then
                // begin
                // mHasCostCentreLink := FPartnerEditUIConnector.HasPartnerCostCentreLink(APartnerKey, CostCentreLink);
                // TLogging.Log('Boolean: ' + mHasCostCentreLink.ToString + '; A CostCentreLink: ' + CostCentreLink, [TLoggingType.ToLogfile]);
                // if (mHasCostCentreLink = true) then
                // begin
                // In this case we are not allowed to make any changes and the check needs to be removed!!!
                // TLogging.Log(StrPartnerHasCostCentreLink1, [TLoggingType.ToLogfile]);
                // mChecked := this.FamilyMembersDV[ARow][this.FTypeCodeModifyName] as Boolean;
                // if (mChecked = true) then
                // begin
                // this.FamilyMembersDV[ARow][this.FTypeCodeModifyName] := (not (this.FamilyMembersDV[ARow][this.FTypeCodeModifyName] as Boolean)) as System.Object;
                // end;
                // MessageBox.Show(StrPartnerHasCostCentreLink1 + CostCentreLink +
                // StrPartnerHasCostCentreLink2, StrPartnerHasCostCentreLinkTitle);
                // end
                // else
                // begin
                // This case is REMOVE / TypeCodePresent = true. In this case we should make the changes.
                // TLogging.Log('Checked?: ' + this.FamilyMembersDV[ARow][this.FTypeCodeModifyName].ToString, [TLoggingType.ToLogfile]);
                // this.FamilyMembersDV[ARow][this.FTypeCodeModifyName] := (not (this.FamilyMembersDV[ARow][this.FTypeCodeModifyName] as Boolean)) as System.Object;
                // TLogging.Log('Checked after tampering?: ' + this.FamilyMembersDV[ARow][this.FTypeCodeModifyName].ToString, [TLoggingType.ToLogfile]);
                // AChanged := true;
                // end;
                // end;
                // end;
            }

            // TLogging.Log('End of ChangeCheckedStateForRow', [TLoggingType.ToLogfile]);
        }

        /// <summary>
        /// This procedure does the actual changing of the CheckBox state.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CheckedStateForRowChanging(ref DataColumnChangeEventArgs e)
        {
            System.Data.DataRow mDataRow;
            bool mTypeCodeModify;
            bool mTypeCodePresent;
            bool mHasCostCentreLink;
            System.Int64 mPartnerKey;
            String CostCentreLink;

            // Initialization
            // TLogging.Log('Begin of CheckedStateForRowChanging', [TLoggingType.ToLogfile]);
            mDataRow = e.Row;
            mTypeCodeModify = System.Convert.ToBoolean(mDataRow[this.FTypeCodeModifyName]);
            mTypeCodePresent = System.Convert.ToBoolean(mDataRow[this.FTypeCodePresentName]);
            mPartnerKey = System.Convert.ToInt64(mDataRow[this.FPartnerKeyName]);

            // TLogging.Log('PartnerKey: ' + mPartnerKey.ToString, [TLoggingType.ToLogfile]);
            // Prevent the changes
            if (this.FAction.Equals("ADD"))
            {
                if (mTypeCodePresent == true)
                {
                    // This case is ADD / TypeCodePresent = true. In this case we have to display a warning.
                    MessageBox.Show(StrWarningOnColumnChangingAdd, StrMessageBoxTitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // TLogging.Log('Proposed Value before changing:' + e.ProposedValue.ToString, [TLoggingType.ToLogfile]);
                    // TLogging.Log('mTypeCodeModify:' + mTypeCodeModify.ToString, [TLoggingType.ToLogfile]);
                    e.ProposedValue = mTypeCodeModify;

                    // TLogging.Log('Proposed Value after changing:' + e.ProposedValue.ToString, [TLoggingType.ToLogfile]);
                    // mChecked := mDataRow[this.FTypeCodeModifyName] as Boolean;
                    // TLogging.Log('mChecked:' + mChecked.ToString, [TLoggingType.ToLogfile]);
                }
                else
                {
                    // This case is ADD / TypeCodePresent = false. In this case we should make the changes.
                    if (mTypeCodeModify == false)
                    {
                        // TLogging.Log('AddRowToFamilyMembersPromotionTable', [TLoggingType.ToLogfile]);
                        // If the box is not checked jet we have to do that
                        // Messagebox.Show('Add Row');
                        this.AddRowToFamilyMembersPromotionTable(mPartnerKey);
                    }
                    else
                    {
                        // Messagebox.Show('Remove Row');
                        // TLogging.Log('RemoveRowFromFamilyMembersPromotionTable', [TLoggingType.ToLogfile]);
                        this.RemoveRowFromFamilyMembersPromotionTable(mPartnerKey);
                    }
                }
            }
            else
            {
                // TLogging.Log('CheckedStateForRowChanging.This case is REMOVE', [TLoggingType.ToLogfile]);
                if (mTypeCodePresent == false)
                {
                    // This case is REMOVE / TypeCodePresent = false. In this case we have to display a warning.
                    // TLogging.Log('CheckedStateForRowChanging.This case is REMOVE / TypeCodePresent = false. In this case we have to display a warning. '+ StrWarningOnColumnChangingRemove, [TLoggingType.ToLogfile]);
                    MessageBox.Show(StrWarningOnColumnChangingRemove, StrMessageBoxTitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.ProposedValue = mTypeCodeModify;
                }
                else
                {
                    // TLogging.Log('CheckedStateForRowChanging.This case is REMOVE / TypeCodePresent = true. In this case we have to apply the changes. ', [TLoggingType.ToLogfile]);
                    // TLogging.Log('CheckedStateForRowChanging.TypeCode: ' + Typecode, [TLoggingType.ToLogfile]);
                    // perform check: If COSTCENTRE is to be removed then check whether Partner has a link to costcentre set up
                    if (TypeCode == "COSTCENTRE")
                    {
                        mHasCostCentreLink = FPartnerEditUIConnector.HasPartnerCostCentreLink(mPartnerKey, out CostCentreLink);

                        // TLogging.Log('CheckedStateForRowChanging.Boolean: ' + mHasCostCentreLink.ToString + '; A CostCentreLink: ' + CostCentreLink, [TLoggingType.ToLogfile]);
                        if (mHasCostCentreLink == true)
                        {
                            // In this case we are not allowed to make any changes and the check needs to be removed!!!
                            // TLogging.Log(StrPartnerHasCostCentreLink1, [TLoggingType.ToLogfile]);
                            e.ProposedValue = mTypeCodeModify;
                            MessageBox.Show(String.Format(StrPartnerHasCostCentreLink, CostCentreLink),
                                StrPartnerHasCostCentreLinkTitle);
                        }
                        else
                        {
                            // This case is REMOVE / TypeCodePresent = true. In this case we should make the changes.
                            // TLogging.Log('CheckedStateForRowChanging.This case is REMOVE / TypeCodePresent = true', [TLoggingType.ToLogfile]);
                            // TLogging.Log('CheckedStateForRowChanging.Checked?: ' + mDataRow[this.FTypeCodeModifyName].ToString, [TLoggingType.ToLogfile]);
                            if (mTypeCodeModify == false)
                            {
                                // If the box is not checked yet we have to do that
                                // Messagebox.Show('Add Row');
                                // TLogging.Log('CheckedStateForRowChanging.AddRowToFamilyMembersPromotionTable', [TLoggingType.ToLogfile]);
                                this.AddRowToFamilyMembersPromotionTable(mPartnerKey);
                            }
                            else
                            {
                                // Messagebox.Show('Remove Row');
                                // TLogging.Log('CheckedStateForRowChanging.RemoveRowFromFamilyMembersPromotionTable', [TLoggingType.ToLogfile]);
                                this.RemoveRowFromFamilyMembersPromotionTable(mPartnerKey);
                            }

                            // TLogging.Log('CheckedStateForRowChanging.Checked after tampering?: ' + mDataRow[this.FTypeCodeModifyName].ToString, [TLoggingType.ToLogfile]);
                        }
                    }
                    else
                    {
                        // This case is REMOVE / TypeCodePresent = true. In this case we should make the changes.
                        // TLogging.Log('CheckedStateForRowChanging.This case is REMOVE / TypeCodePresent = true', [TLoggingType.ToLogfile]);
                        // TLogging.Log('CheckedStateForRowChanging.Checked?: ' + mDataRow[this.FTypeCodeModifyName].ToString, [TLoggingType.ToLogfile]);
                        if (mTypeCodeModify == false)
                        {
                            // If the box is not checked yet we have to do that
                            // Messagebox.Show('Add Row');
                            // TLogging.Log('CheckedStateForRowChanging.AddRowToFamilyMembersPromotionTable', [TLoggingType.ToLogfile]);
                            this.AddRowToFamilyMembersPromotionTable(mPartnerKey);
                        }
                        else
                        {
                            // Messagebox.Show('Remove Row');
                            // TLogging.Log('CheckedStateForRowChanging.RemoveRowFromFamilyMembersPromotionTable', [TLoggingType.ToLogfile]);
                            this.RemoveRowFromFamilyMembersPromotionTable(mPartnerKey);
                        }

                        // TLogging.Log('CheckedStateForRowChanging.Checked after tampering?: ' + mDataRow[this.FTypeCodeModifyName].ToString, [TLoggingType.ToLogfile]);
                    }
                }
            }

            // TLogging.Log('End of CheckedStateForRowChanging', [TLoggingType.ToLogfile]);
        }

        /// <summary>
        /// This procedure creates the colums of the DataGrid displayed
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CreateColumns(TSgrdDataGrid AGrid, System.Data.DataTable ASourceTable, String AAction)
        {
            String FAddRemoveHeaderText;

            SourceGrid.Cells.Editors.TextBoxUITypeEditor l_editor;
            Ict.Common.TypeConverter.TBooleanToYesNoConverter BooleanToYesNoConverter;
            l_editor = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(Boolean));
            l_editor.EditableMode = EditableMode.None;
            BooleanToYesNoConverter = new Ict.Common.TypeConverter.TBooleanToYesNoConverter();

            // PetraDtConverter.
            l_editor.TypeConverter = BooleanToYesNoConverter;
            this.DataGrid = AGrid;

            // Assemble column heading
            if (AAction == "ADD")
            {
                FAddRemoveHeaderText = "Add?";
                this.FAction = "ADD";
            }
            else
            {
                FAddRemoveHeaderText = "Remove?";
                this.FAction = "REMOVE";
            }

            // TLogging.Log('Hello Test Markusm: ', [TLoggingType.ToLogfile]);
            this.FDataGrid.AddCheckBoxColumn(FAddRemoveHeaderText, ASourceTable.Columns[PartnerEditTDSFamilyMembersTable.GetTypeCodeModifyDBName()]);
            this.FDataGrid.AddTextColumn("Currently assigned",
                ASourceTable.Columns[PartnerEditTDSFamilyMembersTable.GetTypeCodePresentDBName()], -1, FSpecialCellController, l_editor, null, null);
            this.FDataGrid.AddTextColumn("Person Name", ASourceTable.Columns[PartnerEditTDSFamilyMembersTable.GetPartnerShortNameDBName()]);
            this.FDataGrid.AddTextColumn("Person PartnerKey", ASourceTable.Columns[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()]);
            this.FDataGrid.AddTextColumn("Family ID", ASourceTable.Columns[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]);

            // Add controller to each line
            // mNumberRows := AGrid.Rows.Count;
            // messagebox.show('Number of rows in  the grid: ' + mNumberRows.ToString);
        }

        /// <summary>
        /// This procedure initializes this System.Object.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InitialisePartnerTypeFamilyMembers(PartnerEditTDSFamilyMembersTable AFamilyMembersDT)
        {
            // Create the result table
            if (FFamilyMembersResultDT == null)
            {
                FFamilyMembersResultDT = new PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable();
            }

            // Logger := new Ict.Common.Logging.TLogging('U:\delphi.net\ICT\Petra\Client\_bin\Debug\Propagation.log');
            // Set the column names for the FFamilyMembersResultDT table.
            this.FResultPartnerKeyName = PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable.GetPartnerKeyDBName();
            this.FResultTypeCodeName = PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable.GetTypeCodeDBName();
            this.FResultAddTypeCodeName = PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable.GetAddTypeCodeDBName();
            this.FResultRemoveTypeCodeName = PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable.GetRemoveTypeCodeDBName();

            // Set the column names for the FFamilyMembersDT table.
            this.FFamilyMembersDT = AFamilyMembersDT;
            this.FTypeCodeModifyName = PartnerEditTDSFamilyMembersTable.GetTypeCodeModifyDBName();
            this.FTypeCodePresentName = PartnerEditTDSFamilyMembersTable.GetTypeCodePresentDBName();
            this.FPartnerKeyName = PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName();
        }

        /// <summary>
        /// This procedure adds a new datarow to the result table if the row with the
        /// specified values cannot be found in it.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void RemoveRowFromFamilyMembersPromotionTable(System.Int64 APartnerKey)
        {
            DataRow[] mDataRows;
            System.Data.DataRow mDataRow;
            String mFilter;
            int mNumRows;

            // Assemble Filterexpression
            mFilter = this.FResultPartnerKeyName + " = " + APartnerKey.ToString();

            // Messagebox.Show('AddRowToFamilyMembersPromotionTable FilterString: ' + mFilter);
            // Check whether the datarow exists.
            mDataRows = this.FFamilyMembersResultDT.Select(mFilter);

            if (mDataRows.Length > 0)
            {
                mNumRows = mDataRows.Length;

                // messagebox.show('At least one Row exists!!! Number of Rows: ' + mNumRows.ToString);
                if (mNumRows > 1)
                {
                    MessageBox.Show(Catalog.GetString(
                            "Please contact your OpenPetra Support team. Forward this message: TPartnerTypeFamilyMembersPropagationSelectionLogic.RemoveRowFromFamilyMembersPromotionTable"));
                }
                else
                {
                    mDataRow = mDataRows[0];
                    this.FFamilyMembersResultDT.Rows.Remove(mDataRow);
                }
            }

            #region Final Check
            mDataRows = this.FFamilyMembersResultDT.Select(mFilter);

            // mNumRows := Length(mDataRows);
            // messagebox.show(mNumRows.ToString + ' Rows.');
            // for mDataRow in mDataRows do
            // begin
            // mWhatsInIt := mDataRow[0].ToString + '; ' + mDataRow[1].ToString + '; ' + mDataRow[2].ToString + '; ' + mDataRow[3].ToString;
            // messagebox.show(mWhatsInIt);
            // end;
            #endregion
        }

        /// <summary>
        /// This procedure resets the Checked status immedieatly after the Single Click
        /// event.
        /// </summary>
        /// <param name="ARow">row number.
        /// </param>
        /// <returns>void</returns>
        public void ResetCheckedStatusAfterSingleClick(System.Int32 ARow)
        {
            // var
            // mRowView: System.Data.DataRowView;
            // mRow:     System.Data.DataRow;
            if ((Boolean)(this.FamilyMembersDV[ARow][this.FTypeCodeModifyName]) == true)
            {
                this.FamilyMembersDV[ARow][this.FTypeCodeModifyName] =
                    (System.Object)((!(Boolean)(this.FamilyMembersDV[ARow][this.FTypeCodeModifyName])));

                // mRowView := this.FDataGrid.Rows.IndexToDataSourceRow(ARow +1);  Grid starts counting with 1
                // mRow := mRowView.Row;
                // mRow[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()] := (false as System.Object);
                // messagebox.Show('true');
            }
            else
            {
                // messagebox.Show('false');
            }
        }

        /// <summary>
        /// This function determines the current PartnerKey
        ///
        /// </summary>
        /// <returns>void</returns>
        public Int64 DetermineCurrentFamilyMemberPartnerKey(TSgrdDataGrid AGrid)
        {
            DataRowView[] TheDataRowViewArray;
            Int64 PersonPartnerKey;

            // MessageBox.Show(ARow.ToString);
            TheDataRowViewArray = AGrid.SelectedDataRowsAsDataRowView;

            // get PartnerKey of current DataRow
            try
            {
                PersonPartnerKey = Convert.ToInt64(TheDataRowViewArray[0].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()]);
            }
            catch (Exception)
            {
                throw;
            }

            // MessageBox.Show(PersonPartnerKey.ToString);
            return PersonPartnerKey;
        }

        /// <summary>
        /// This function determines the current PartnerKey
        ///
        /// </summary>
        /// <returns>void</returns>
        public Int64 DeterminePartnerKeyFromRowNumber(System.Int32 ARowNumber)
        {
            System.Data.DataRowView mRowView;
            System.Data.DataRow mRow;
            System.Object mObject;

            // TLogging.Log('Begin of DeterminePartnerKeyFromRowNumber: ' + ARowNumber.ToString, [TLoggingType.ToLogfile]);
            // Grid starts counting with 1
            mRowView = (DataRowView) this.FDataGrid.Rows.IndexToDataSourceRow(ARowNumber + 1);
            mRow = mRowView.Row;
            mObject = mRow[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()];
            return (System.Int64)(mObject);

            // TLogging.Log('PartnerKey: ' + Result.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('End of DeterminePartnerKeyFromRowNumber: ' + ARowNumber.ToString, [TLoggingType.ToLogfile]);
        }

        /// <summary>
        /// This function returns the Result table.
        ///
        /// </summary>
        /// <returns>void</returns>
        public PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable GetResultTable()
        {
            return this.FFamilyMembersResultDT;
        }

        #endregion
    }
}