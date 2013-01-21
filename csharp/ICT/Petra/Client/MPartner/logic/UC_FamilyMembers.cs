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
using System.Drawing;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Controllers;
using Ict.Common.Controls;
using System.Collections;
using DevAge;
using DevAge.ComponentModel.Validator;
using System.ComponentModel;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using System.Globalization;
using DevAge.ComponentModel.Converter;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// FamilyID logic for the UC_FamilyMembers UserControl.
    /// </summary>
    public class TUCFamilyMembersLogic
    {
        #region Resourcetexts

        /// <summary>todoComment</summary>
        private static readonly string StrFamilyIDChangeDone = Catalog.GetString(
            "The Family ID of\r\n" +
            "    {0} was changed from {1} to {2}\r\n" +
            "    {3} was changed from {4} to {5}");

        /// <summary>todoComment</summary>
        private static readonly string StrFamilyIDChangeDoneTitle = Catalog.GetString("Family ID Change Done");

        #endregion

        private PartnerEditTDS FMainDS;
        private PartnerEditTDSFamilyMembersTable FFamilyMembersDT;
        private System.Data.DataView FFamilyMembersDV;
        private TSgrdDataGrid FDataGrid;
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private SourceGrid.Cells.Editors.ComboBox FFamilyIDEditor;
        private ControllerBase FSpecialCellController = null;
        private Int32[] FamilyIDDropDownValues;

        /// <summary>isEdited: Boolean;</summary>
        private Boolean FGridEdited;
        private object PreviousPartnerMemory;

        /// <summary>todoComment</summary>
        public PartnerEditTDS MainDS
        {
            get
            {
                return FMainDS;
            }

            set
            {
                FMainDS = value;
            }
        }

        /// <summary>todoComment</summary>
        public TSgrdDataGrid DataGrid
        {
            get
            {
                return FDataGrid;
            }

            set
            {
                FDataGrid = value;
            }
        }

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

        /// <summary>todoComment</summary>
        public Boolean GridEdited
        {
            get
            {
                Boolean ReturnValue;
                DataView ChangesDV;

                if (DataGridExist())
                {
                    ChangesDV = new DataView(((DevAge.ComponentModel.BoundDataView)FDataGrid.DataSource).DataView.Table,
                        "",
                        "",
                        DataViewRowState.ModifiedCurrent);

                    if (ChangesDV.Count > 0)
                    {
                        ReturnValue = true;
                    }
                    else
                    {
                        ReturnValue = false;
                    }
                }
                else
                {
                    ReturnValue = false;
                }

                return ReturnValue;
            }

            set
            {
                MessageBox.Show("FGridEdited: was: " + FGridEdited.ToString() + ", getting changed to: " + value.ToString());
                FGridEdited = value;
            }
        }

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #region TUCFamilyMembersLogic

        /// <summary>
        /// Loads FamilyMembers Data from Petra Server into FMainDS.
        ///
        /// </summary>
        /// <returns>true if successful and Family has Family Members, otherwise false.
        /// //DevAge.ComponentModel.Validator;</returns>
        public Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;
            Int64 FamilyPartnerKey;

            if (FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
            {
                FamilyPartnerKey = FMainDS.PFamily[0].PartnerKey;
            }
            else
            {
                FamilyPartnerKey = FMainDS.PPerson[0].FamilyKey;
            }

            // retrieve Family Members from PetraServer
            // If family has no members, returns false
            try
            {
                // Make sure that Typed DataTable is already there at Client side
                if (FMainDS.FamilyMembers == null)
                {
                    FMainDS.Tables.Add(new PartnerEditTDSFamilyMembersTable(PartnerEditTDSFamilyMembersTable.GetTableName()));
                    FMainDS.InitVars();
                }

                FMainDS.FamilyMembers.Rows.Clear();
                FMainDS.Merge(FPartnerEditUIConnector.GetDataFamilyMembers(FamilyPartnerKey, ""));
                FMainDS.FamilyMembers.AcceptChanges();

                if (FMainDS.FamilyMembers.Rows.Count > 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                ReturnValue = false;
                return false;
            }
            catch (Exception)
            {
                ReturnValue = false;

                // raise;
            }

            // Fire OnRecalculateScreenParts event
            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
            FDataGrid.Selection.Focus(new Position(1, 1), true);
            return ReturnValue;
        }

        /// <summary>
        /// Checks if the selected FamilyID is maximum.
        /// </summary>
        /// <returns>void</returns>
        public Boolean IsMaximum()
        {
            Boolean ReturnValue = false;
            Int32 Counter;
            Int32 FamilyIDint;

            FamilyIDint = Convert.ToInt32(((DataRowView)FDataGrid.SelectedDataRows[0])[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]);

            // Goes throuht the FamilyID:s, If finds larger than selected, breaks and returns false. Otherwice true.
            for (Counter = 0; Counter <= (this.GetNumberOfRows() - 1); Counter += 1)
            {
                if (Convert.ToInt64(FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]) > FamilyIDint)
                {
                    ReturnValue = false;
                    break;
                }

                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks if the selected FamilyID is minimun.
        /// Checks, if the Family ID of selectedrow is the smallest of FamilyID:s.
        /// </summary>
        /// <returns>void</returns>
        public Boolean IsMinimum()
        {
            Boolean ReturnValue = false;
            Int32 Counter;
            Int32 FamilyIDint;

            FamilyIDint = Convert.ToInt32(
                ((DataRowView)FDataGrid.SelectedDataRows[0]).Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]);

            // Goes throuht the FamilyID:s, If finds smaller than selected, breaks and returns false. Otherwice true.
            for (Counter = 0; Counter <= (this.GetNumberOfRows() - 1); Counter += 1)
            {
                if (Convert.ToInt64(FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]) < FamilyIDint)
                {
                    ReturnValue = false;
                    break;
                }

                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// returns true if Family has members (in the datagrid)
        /// </summary>
        /// <returns>void</returns>
        public Boolean MembersInFamilyExist()
        {
            Boolean ReturnValue;

            try
            {
                if (FDataGrid.Rows.Count > 1)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                // MessageBox.show(' DO NOT PRESS THE BUTTONS, IF THERE IS NOTHING TO REFRESH'+#10+' If you continue your antisocial behaviour, Petra Team will punish you','STOP THAT IMMEDIATELY');
                ReturnValue = false;
            }
            return ReturnValue;
        }

        /// <summary>
        /// Custom Event
        /// </summary>
        /// <returns>void</returns>
        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// enables the FamilyID edit combobox. This causes no errors, but uses the default list for FamilyID:s (1,2,3,4,5,6,7,8,9,0)
        /// </summary>
        /// <returns>void</returns>
        public void OpenComboBox()
        {
            Int32 RowNumber;

            RowNumber = this.GetRowSelected();
            FFamilyIDEditor.EnableEdit = true;
            FFamilyIDEditor.EditableMode = EditableMode.Focus;
            FDataGrid.Selection.Focus(new Position(RowNumber, FDataGrid.Columns.Count - 1), true);

            // Int64 PartnerKeyMemory = this.GetPartnerKeySelected();
        }

        /// <summary>
        /// returns Arraylist that has FamilyID and PartnerID pairs. For testing purposes.
        /// </summary>
        /// <returns>void</returns>
        public ArrayList PrintOrder()
        {
            Int32 i;
            ArrayList List;

            List = new ArrayList();

            // Goes through the FamilyMembers, and list FamilyID and PartnerKey
            for (i = 0; i <= (GetNumberOfRows() - 1); i += 1)
            {
                List.Add(Convert.ToString(FFamilyMembersDV[i].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()]) + ':' +
                    Convert.ToString(FFamilyMembersDV[i].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]));
            }

            return List;
        }

        /// <summary>
        /// Promotes selected ID (and demotes the FamilyID next (up) to selected FamilyID
        /// </summary>
        /// <returns>void</returns>
        public void PromoteFamilyID()
        {
            Int32 buttonvalue = -1;
            Int32 Counter1;
            Int32 NumberOfRows;
            Int32 Counter2;
            Int32 Counter2ToMax;
            Int32 FamilyIDint;

            System.Object PartnerKey;
            System.Object FamilyID = -1;
            System.Object PersonName1 = "";
            System.Object PersonName2 = "";
            System.Object NextFamilyID = "";
            Boolean MemberFind;
            MessageBoxButtons Button;

            // Get the PartnerKey of the selected Row
            PartnerKey = ((DataRowView)FDataGrid.SelectedDataRows[0]).Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()];
            NumberOfRows = GetNumberOfRows();

            // for loop to get the selected partners ID
            for (Counter2 = 0; Counter2 <= (NumberOfRows - 1); Counter2 += 1)
            {
                if (FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()].ToString() == PartnerKey.ToString())
                {
                    FamilyID = FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()];
                    PersonName1 = FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetPartnerShortNameDBName()];
                    break;
                }
            }

            // loop to find the nearest  FamilyID compared to selected FamilyID.
            // sets the next FamilyID to correct.
            MemberFind = true;
            FamilyIDint = Convert.ToInt32(FamilyID);

            // If Family ID 1 (parent) is about to be promoted
            if (FamilyIDint == 1)
            {
                Button = MessageBoxButtons.YesNo;
                buttonvalue = 0;

                // if pressed No to question below, does nothing.
                if (MessageBox.Show("Parents should be Family ID 0 or 1" + "\r\nAre you sure you want to change this Family ID?", "Family ID Change",
                        Button) == DialogResult.No)
                {
                    buttonvalue = 1;
                }
            }

            // Executes this loop, if Family ID to be promoted is not 1.
            if (buttonvalue != 1)
            {
                Counter2ToMax = 100;

                // goes through all values from selected +1 to 100+selected.
                for (Counter1 = 1; Counter1 <= Counter2ToMax; Counter1 += 1)
                {
                    // goes through all FamilyID:s
                    for (Counter2 = 0; Counter2 <= (NumberOfRows - 1); Counter2 += 1)
                    {
                        // When finds FamilyID that's next (above) to selected FamilyID, Replaces the Found Family members ID with selected ID
                        if (Convert.ToInt32(FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()].ToString())
                            == FamilyIDint + Counter1)
                        {
                            // saves the FamilyIF just found
                            NextFamilyID = FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()];
                            FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()] = FamilyID;
                            PersonName2 = FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetPartnerShortNameDBName()];
                            MemberFind = false;
                            break;
                        }
                    }

                    if (!MemberFind)
                    {
                        break;
                    }
                }

                // end;
                // loop to set the selected FamilyID to the PreviousFamilyID
                for (Counter2 = 0; Counter2 <= (NumberOfRows - 1); Counter2 += 1)
                {
                    if (FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()].ToString() == PartnerKey.ToString())
                    {
                        FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()] = (object)(FamilyIDint + 1);
                        break;
                    }
                }

                // button := MessageBoxButtons.OK;

                MessageBox.Show(String.Format(StrFamilyIDChangeDone, PersonName1, FamilyID, (FamilyIDint + 1),
                        PersonName2, NextFamilyID, FamilyID),
                    StrFamilyIDChangeDoneTitle);
            }
            else
            {
            }
        }

        /// <summary>
        /// Shows MessageBox, that has several lines on it.
        /// </summary>
        /// <returns>void</returns>
        public void ShowStrMessage(String Caption, string[] Lines)
        {
            String Text;

            Text = "";

            foreach (string s in Lines)
            {
                Text = Text + s + "\r\n";
            }

            Text = Text.Substring(0, Text.Length - 2);
            MessageBox.Show(Text, Caption);
        }

        /// <summary>
        /// Shows the arraulist members in a combobox
        /// </summary>
        /// <returns>void</returns>
        public void ShowArraylistMembers(ArrayList A)
        {
            String Apu;

            Apu = "";

            foreach (object o in A)
            {
                Apu = Apu + o.ToString();
            }

            MessageBox.Show(Apu);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="Lines"></param>
        public void ShowStrMessage(string[] Lines)
        {
            ShowStrMessage("", Lines);
        }

        #region Setup SourceDataGrid

        /// <summary>
        /// Sets the order of colums
        /// </summary>
        /// <returns>void</returns>
        public void CreateColumns()
        {
            SourceGrid.Cells.Editors.TextBoxUITypeEditor l_editorDt2;
            Ict.Common.TypeConverter.TDateConverter DateTypeConverter;
            FFamilyMembersDT = FMainDS.FamilyMembers;

            // Editor for Date of Birth column (Petra Date format)
            l_editorDt2 = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(DateTime));
            l_editorDt2.EditableMode = EditableMode.None;
            DateTypeConverter = new Ict.Common.TypeConverter.TDateConverter();

            // DateTypeConverter.
            l_editorDt2.TypeConverter = DateTypeConverter;
            try
            {
                this.FDataGrid.AddTextColumn("Person Name",
                    FFamilyMembersDT.Columns[PartnerEditTDSFamilyMembersTable.GetPartnerShortNameDBName()], -1, FSpecialCellController, null, null,
                    null);
                this.FDataGrid.AddTextColumn("Gender",
                    FFamilyMembersDT.Columns[PartnerEditTDSFamilyMembersTable.GetGenderDBName()], -1, FSpecialCellController, null, null, null);
                this.FDataGrid.AddTextColumn("Date of Birth",
                    FFamilyMembersDT.Columns[PartnerEditTDSFamilyMembersTable.GetDateOfBirthDBName()], -1, FSpecialCellController, l_editorDt2, null,
                    null);
                this.FDataGrid.AddTextColumn("Partner Key",
                    FFamilyMembersDT.Columns[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()], -1, FSpecialCellController, null, null, null);
                FamilyIDDropDownValues = new Int32[] {
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 0
                };
                FFamilyIDEditor = new SourceGrid.Cells.Editors.ComboBox(typeof(Int32), FamilyIDDropDownValues, false);
                this.FDataGrid.AddTextColumn("Family ID",
                    FFamilyMembersDT.Columns[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()], 80, FFamilyIDEditor);
                DisableEditing();
                FFamilyIDEditor.EnableEdit = false;
                FFamilyIDEditor.Control.Validating += new CancelEventHandler(this.FamilyID_Validating);
                // DevAge.ComponentModel.Validator.ValueMapping FamilyIDDropDownMapping =
                new DevAge.ComponentModel.Validator.ValueMapping();
            }
            catch (System.NullReferenceException)
            {
            }

            // to do if no lines.
        }

        /// <summary>
        /// Sets up the DataBinding of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DataBindGrid()
        {
            FFamilyMembersDV = FFamilyMembersDT.DefaultView;
            FFamilyMembersDV.AllowNew = false;
            FFamilyMembersDV.AllowEdit = true;
            FFamilyMembersDV.AllowDelete = false;
            FFamilyMembersDV.Sort = PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName() + " ASC";

            // DataBind the DataGrid
            this.FDataGrid.DataSource = new DevAge.ComponentModel.BoundDataView(FFamilyMembersDV);
            this.FDataGrid.Selection.SelectRow(1, true);
        }

        /// <summary>
        /// returns true, if DataGrid is created
        /// </summary>
        /// <returns>void</returns>
        public Boolean DataGridExist()
        {
            Boolean ReturnValue;

            try
            {
                if (FDataGrid.Columns.Count > 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (NullReferenceException)
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }

        /// <summary>
        /// Changes the state of checked row
        /// </summary>
        /// <returns>void</returns>
        public void ChangeCheckedStateForRow(Int32 ARow)
        {
            FFamilyMembersDV[ARow]["Checked"] = (System.Object)((!(Boolean)(FFamilyMembersDV[ARow]["Checked"])));
        }

        /// <summary>
        /// gets asd sets the combox values. Redraws the grids last column. causes errors.
        /// </summary>
        /// <returns>void</returns>
        public void GetComboBoxValues(out Int32[] ComboBoxValues)
        {
            Int32 Counter;
            ArrayList List;
            String epo;

            // fills the list with numbers 0,1,...9
            ComboBoxValues = new Int32[(10 - GetNumberOfRows())];
            List = new ArrayList();

            for (Counter = 0; Counter <= 9; Counter += 1)
            {
                List.Add(Counter.ToString());
            }

            // removes the existing familyID:s from the list
            for (Counter = 0; Counter <= this.GetNumberOfRows() - 1; Counter += 1)
            {
                epo = Convert.ToString(FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]);
                List.Remove(epo);

                // ShowArraylistMembers(List);
            }

            // creates the array of int 32 of the ArrayList members
            for (Counter = 0; Counter <= List.Count - 1; Counter += 1)
            {
                try
                {
                    ComboBoxValues[Counter] = Convert.ToInt32(List[Counter]);
                }
                catch (System.ArgumentOutOfRangeException e)
                {
                    MessageBox.Show("Again a error:" + e.ToString());
                }
            }
        }

        /// <summary>
        /// returns the selected Family ID
        /// </summary>
        /// <returns>void</returns>
        public Int32 GetFamilyID()
        {
            return Convert.ToInt32(((DataRowView)FDataGrid.SelectedDataRows[0]).Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]);
        }

        /// <summary>
        /// Returns the number of rows in datagrid (number of Family members)
        /// </summary>
        /// <returns>void</returns>
        public Int32 GetNumberOfRows()
        {
            return this.FDataGrid.DataSource.Count;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public Int64 GetPartnerKeySelected()
        {
            return Convert.ToInt64(((DataRowView)FDataGrid.SelectedDataRows[0]).Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()]);
        }

        /// <summary>
        /// Returns the PartnerKey that's selected
        /// </summary>
        /// <returns>void</returns>
        public Int32 GetRowSelected()
        {
            System.Int32 ARowNumber;
            System.Int64 ASiteKey;
            this.GetRowSelected(out ARowNumber, out ASiteKey);
            return ARowNumber;
        }

        /// <summary>
        /// Finds out the number of row, and it's Partnerkey in datagrid that's selected.
        /// </summary>
        /// <returns>void</returns>
        public void GetRowSelected(out Int32 ARowNumber, out Int64 ASiteKey)
        {
            System.Int32 CurrentRow;
            DataView AGridDataView;
            AGridDataView = ((DevAge.ComponentModel.BoundDataView)FDataGrid.DataSource).DataView;
            ARowNumber = 0;
            ASiteKey = Convert.ToInt64(((DataRowView)FDataGrid.SelectedDataRows[0]).Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()]);

            // goes throuhg the FamilyID:s in datagrid, break when comes to selected.
            for (CurrentRow = 0; CurrentRow <= AGridDataView.Count - 1; CurrentRow += 1)
            {
                ARowNumber = ARowNumber + 1;

                if (Convert.ToInt64(AGridDataView[CurrentRow].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()]) == ASiteKey)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Demotes selected ID (and promotes the FamilyID next (lower) to selected FamilyID
        /// </summary>
        /// <returns>void</returns>
        public void DemoteFamilyID()
        {
            Int32 Buttonvalue = -1;
            Int32 NumberOfRows;
            Int32 Counter;
            Int32 CounterToMax;
            Int32 FamilyIDint;

            System.Object PartnerKey;
            System.Object FamilyID = -1;
            System.Object PreviousFamilyID = -1;
            System.Object PersonName1 = "";
            System.Object PersonName2 = "";
            Boolean MemberFind;
            MessageBoxButtons Button;

            // Get the PartnerKey of the selected Row
            PartnerKey = ((DataRowView)FDataGrid.SelectedDataRows[0]).Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()];
            NumberOfRows = GetNumberOfRows();

            // for loop to get the selected partners ID
            for (Counter = 0; Counter <= (NumberOfRows - 1); Counter += 1)
            {
                if (FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()].ToString() == PartnerKey.ToString())
                {
                    FamilyID = FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()];
                    PersonName1 = FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetPartnerShortNameDBName()];
                    break;
                }
            }

            if ((this.IsMinimum()) && (Convert.ToInt32(FamilyID) > 0))
            {
                FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()] = (object)0;
                Buttonvalue = 1;
                FamilyID = (object)0;
            }

            // loop to find the nearest smaller FamilyID compared to selected FamilyID.
            // sets the previous FamilyID to correct.
            MemberFind = true;
            FamilyIDint = Convert.ToInt32(FamilyID);

            // If Family ID is 2, or A parent is to be replaced with child, gives warning.
            if (FamilyIDint == 2)
            {
                Button = MessageBoxButtons.YesNo;

                if (MessageBox.Show("Parents should be Family ID 0 or 1" + "\r\nAre you sure you want to change this Family ID?", "Family ID Change",
                        Button) == DialogResult.No)
                {
                    Buttonvalue = 1;
                }
            }

            // if FamilyID to be demoted is 2 and cancel is selected from warnind messagebox, does nothing. otherwice goes through the  for loop
            if (Buttonvalue != 1)
            {
                // Goes through the FamilyID:s from 1 to selected FamilyID
                for (CounterToMax = 1; CounterToMax <= FamilyIDint; CounterToMax += 1)
                {
                    // Goes through all the FamilyID:s
                    for (Counter = 0; Counter <= (NumberOfRows - 1); Counter += 1)
                    {
                        // when finds FamilyID next to selected (below), Sets the found Family ID to selected
                        if (Convert.ToInt32(FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()].ToString()) ==
                            (FamilyIDint - CounterToMax))
                        {
                            // saves the found FamilyID
                            PreviousFamilyID = FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()];
                            FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()] =
                                (object)(Convert.ToInt32(PreviousFamilyID) + 1);
                            PersonName2 = FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetPartnerShortNameDBName()];

                            if (PersonName2 == PersonName1)
                            {
                                PersonName1 = PreviousPartnerMemory;
                            }

                            MemberFind = false;
                            break;
                        }
                    }

                    if (!MemberFind)
                    {
                        break;
                    }
                }

                // loop to set the selected FamilyID to the PreviousFamilyID
                for (Counter = 0; Counter <= (NumberOfRows - 1); Counter += 1)
                {
                    if (FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()].ToString() == PartnerKey.ToString())
                    {
                        FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()] = PreviousFamilyID;
                        break;
                    }
                }

                MessageBox.Show(String.Format(StrFamilyIDChangeDone, PersonName1, FamilyID, PreviousFamilyID,
                        PersonName2, PreviousFamilyID, (Convert.ToInt32(PreviousFamilyID) + 1)),
                    StrFamilyIDChangeDoneTitle);
                PreviousPartnerMemory = PersonName1;
            }
            else
            {
            }
        }

        /// <summary>
        /// Disables the editing mode of FamilyID column
        /// </summary>
        /// <returns>void</returns>
        public void DisableEditing()
        {
            FFamilyIDEditor.EnableEdit = false;
        }

        private void FamilyID_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            const int NEWFAMILYID_DEFAULT = -1;
            Int32 Counter;
            Int64 PartnerKey;
            Boolean IsInUse;

            IsInUse = false;
            int NewFamilyID = NEWFAMILYID_DEFAULT;
            bool ValidFormat = true;

            try
            {
                try
                {
                    NewFamilyID = Convert.ToInt32((sender as Control).Text);
                }
                catch (System.FormatException)
                {
                    ValidFormat = false;
                }
                catch (Exception exp)
                {
                    e.Cancel = true;
                    MessageBox.Show("Exception in FamilyID_Validating: " + exp.ToString());
                    throw;
                }

                if (ValidFormat)
                {
                    if ((NewFamilyID < 0) || (NewFamilyID > 99))
                    {
                        MessageBox.Show("Family ID needs to be a number between 0 and 99!");
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Only numbers are allowed as Family IDs!");
                    e.Cancel = true;
                    return;
                }

                PartnerKey = this.GetPartnerKeySelected();

                // checks if the FamilyID selected from the Combobox is already in use
                for (Counter = 0; Counter <= (GetNumberOfRows() - 1); Counter += 1)
                {
                    if (FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()].ToString() == NewFamilyID.ToString())
                    {
                        if (FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()].ToString() ==
                            this.GetFamilyID().ToString())
                        {
                            break;
                        }

                        IsInUse = true;

                        // the validating event is cancelled. (for saving the old FamilyID
                        e.Cancel = true;
                        break;
                    }
                }

                // if The Family ID selected from the Combobox is already in use, copies the old FamilyID to that Person edited.
                // This needs to be done, because the combobox (or FFamilyIDEditor) is databinded to the database.
                if (IsInUse)
                {
                    for (Counter = 0; Counter <= (GetNumberOfRows() - 1); Counter += 1)
                    {
                        if (FFamilyMembersDV[Counter].Row == (object)PartnerKey)
                        {
                            FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()] = (object)this.GetFamilyID();

                            // Int32 LocationMemory = Counter;

                            break;
                        }
                    }

                    MessageBox.Show("Please, select another Family ID, the one you selected (" + NewFamilyID.ToString() + ") is already in use!");
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
        #endregion
    }
}