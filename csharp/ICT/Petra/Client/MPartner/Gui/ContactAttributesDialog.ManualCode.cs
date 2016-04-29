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
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TFrmContactAttributesDialog
    {
        private delegate void CheckChangedArgs(int ChangedRow);
        private event CheckChangedArgs ChangedRowEvent;
        private CustomValueChangedEvent FGridValueChangedEvent;

        private DataView FContactAttributeTableDV;
        private DataView FContactAttributeDetailTableDV;
        private PPartnerContactAttributeTable FSelectedContactAttributeTable = new PPartnerContactAttributeTable();
        private DataView FSelectedContactAttributeTableDV;
        private Int64 FContactID;
        private DataTable FGridTable;
        private DataView FGridTableDV;
        private Boolean FAddedAttributeDeleted = false;

        /// <summary>
        /// Sets the contact identifier.
        /// </summary>
        public Int64 ContactID
        {
            set
            {
                FContactID = value;
            }
        }

        /// <summary>
        /// True if a previously added attribute has now been deleted
        /// </summary>
        public bool AddedAttributeDeleted
        {
            get
            {
                return FAddedAttributeDeleted;
            }
        }

        /// <summary>
        /// Gets or sets the PartnerContactAttributeTable
        /// </summary>
        public PPartnerContactAttributeTable SelectedContactAttributeTable
        {
            set
            {
                FSelectedContactAttributeTable.Merge(value);

                // create dataview with a filter to only show contact attributes for current contact log
                FSelectedContactAttributeTableDV = FSelectedContactAttributeTable.DefaultView;
                FSelectedContactAttributeTableDV.RowFilter = PPartnerContactAttributeTable.GetContactIdDBName() + " = " + FContactID;
                FSelectedContactAttributeTableDV.Sort = PPartnerContactAttributeTable.GetContactAttributeCodeDBName() + " ASC, " +
                                                        PPartnerContactAttributeTable.GetContactAttrDetailCodeDBName() + " ASC";

                // populate the grid
                FillTempTable();
                CreateGrid();
            }
            get
            {
                return FSelectedContactAttributeTable;
            }
        }

        #region Setup methods

        private void InitializeManualCode()
        {
            this.AcceptButton = btnOK;

            FContactAttributeTableDV =
                TDataCache.TMPartner.GetCacheableMailingTable(TCacheableMailingTablesEnum.ContactAttributeList).DefaultView;
            FContactAttributeTableDV.Sort = PContactAttributeTable.GetContactAttributeCodeDBName() + " ASC";

            FContactAttributeDetailTableDV =
                TDataCache.TMPartner.GetCacheableMailingTable(TCacheableMailingTablesEnum.ContactAttributeDetailList).DefaultView;
            FContactAttributeDetailTableDV.Sort = PContactAttributeDetailTable.GetContactAttributeCodeDBName() + " ASC, " +
                                                  PContactAttributeDetailTable.GetContactAttrDetailCodeDBName() + " ASC";

            CreateTempTable();

            grdContactAttributes.MouseClick += new MouseEventHandler(this.GrdContactAttributes_Click);
            grdContactAttributes.SpaceKeyPressed += new TKeyPressedEventHandler(this.GrdContactAttributes_SpaceKeyPressed);
            grdContactAttributes.EnterKeyPressed += new TKeyPressedEventHandler(this.GrdContactAttributes_EnterKeyPressed);
        }

        private void CreateTempTable()
        {
            FGridTable = new DataTable("ContactAttributes");
            FGridTable.Columns.Add("Checked", System.Type.GetType("System.Boolean"));
            FGridTable.Columns.Add("AttributeCode", System.Type.GetType("System.String"));
            FGridTable.Columns.Add("AttributeDescription", System.Type.GetType("System.String"));
            FGridTable.Columns.Add("AttributeDetailCode", System.Type.GetType("System.String"));
            FGridTable.Columns.Add("AttributeDetailDescription", System.Type.GetType("System.String"));
        }

        private void FillTempTable()
        {
            DataView UnselectedContactAttributeTableDV;
            DataRow TheNewRow;
            Int16 RowCounter;

            FGridTable.Rows.Clear();
            FGridTable.AcceptChanges();

            UnselectedContactAttributeTableDV = FContactAttributeDetailTableDV;

            // first add Contact Attributes which are already selected for partner
            for (RowCounter = 0; RowCounter < FSelectedContactAttributeTableDV.Count; RowCounter++)
            {
                TheNewRow = FGridTable.NewRow();
                TheNewRow["Checked"] = (System.Object)true;
                TheNewRow["AttributeCode"] =
                    FSelectedContactAttributeTableDV[RowCounter][PPartnerContactAttributeTable.GetContactAttributeCodeDBName()];
                TheNewRow["AttributeDescription"] = ContactAttributesLogic.GetContactAttributeDesciption(
                    FSelectedContactAttributeTableDV[RowCounter][PPartnerContactAttributeTable.GetContactAttributeCodeDBName()].ToString(),
                    FContactAttributeTableDV);
                TheNewRow["AttributeDetailCode"] =
                    FSelectedContactAttributeTableDV[RowCounter][PPartnerContactAttributeTable.GetContactAttrDetailCodeDBName()];
                TheNewRow["AttributeDetailDescription"] = ContactAttributesLogic.GetContactAttributeDetailDesciption(
                    FSelectedContactAttributeTableDV[RowCounter][PPartnerContactAttributeTable.GetContactAttributeCodeDBName()].ToString(),
                    FSelectedContactAttributeTableDV[RowCounter][PPartnerContactAttributeTable.GetContactAttrDetailCodeDBName()].ToString(),
                    FContactAttributeDetailTableDV);
                FGridTable.Rows.Add(TheNewRow);
            }

            // second add the rest of the Special Types in db
            for (RowCounter = 0; RowCounter < UnselectedContactAttributeTableDV.Count; RowCounter++)
            {
                // only add row if it has not already been added as a checked row
                if (FSelectedContactAttributeTableDV.Find(new object[]
                        { UnselectedContactAttributeTableDV[RowCounter][PContactAttributeDetailTable.GetContactAttributeCodeDBName()],
                          UnselectedContactAttributeTableDV[RowCounter][PContactAttributeDetailTable.GetContactAttrDetailCodeDBName()] }) == -1)
                {
                    TheNewRow = FGridTable.NewRow();
                    TheNewRow["Checked"] = (System.Object)false;
                    TheNewRow["AttributeCode"] =
                        UnselectedContactAttributeTableDV[RowCounter][PContactAttributeDetailTable.GetContactAttributeCodeDBName()];
                    TheNewRow["AttributeDescription"] = ContactAttributesLogic.GetContactAttributeDesciption(
                        UnselectedContactAttributeTableDV[RowCounter][PContactAttributeDetailTable.GetContactAttributeCodeDBName()].ToString(),
                        FContactAttributeTableDV);
                    TheNewRow["AttributeDetailCode"] =
                        UnselectedContactAttributeTableDV[RowCounter][PContactAttributeDetailTable.GetContactAttrDetailCodeDBName()];
                    TheNewRow["AttributeDetailDescription"] = ContactAttributesLogic.GetContactAttributeDetailDesciption(
                        UnselectedContactAttributeTableDV[RowCounter][PContactAttributeDetailTable.GetContactAttributeCodeDBName()].ToString(),
                        UnselectedContactAttributeTableDV[RowCounter][PContactAttributeDetailTable.GetContactAttrDetailCodeDBName()].ToString(),
                        FContactAttributeDetailTableDV);
                    FGridTable.Rows.Add(TheNewRow);
                }
            }
        }

        private void CreateGrid()
        {
            grdContactAttributes.AddCheckBoxColumn("", FGridTable.Columns["Checked"], 17, false);
            grdContactAttributes.AddTextColumn("Attribute Code", FGridTable.Columns["AttributeCode"]);
            grdContactAttributes.AddTextColumn("Description", FGridTable.Columns["AttributeDescription"]);
            grdContactAttributes.AddTextColumn("Detail Code", FGridTable.Columns["AttributeDetailCode"]);
            grdContactAttributes.AddTextColumn("Description", FGridTable.Columns["AttributeDetailDescription"]);

            FGridTableDV = FGridTable.DefaultView;
            FGridTableDV.AllowNew = false;
            FGridTableDV.AllowEdit = true;
            FGridTableDV.AllowDelete = false;

            // DataBind the DataGrid
            grdContactAttributes.DataSource = new DevAge.ComponentModel.BoundDataView(FGridTableDV);
            grdContactAttributes.SelectRowInGrid(1);

            // Hook Grid event that allows popping up a question whether to check the CheckBox
            FGridValueChangedEvent = new CustomValueChangedEvent(this);
            grdContactAttributes.Controller.AddController(FGridValueChangedEvent);
            this.ChangedRowEvent += new CheckChangedArgs(ChangedRowEventHandler);
        }

        #endregion

        // this is only called when the user clicks on the 'CheckBox' column and the grid automatically checks or unchecks a CheckBox
        private void ChangedRowEventHandler(int ChangedRow)
        {
            // Our code also checks/uncheck a CheckBox when the user clicks in the 'CheckBox' column so we have a double check.
            // I.e. the checkbox is returned to the original value. We need to check/uncheck again to redo this.
            DataRow TmpDR = FGridTableDV[ChangedRow].Row;

            FGridTableDV[ChangedRow]["Checked"] = (System.Object)((!(Boolean)(FGridTableDV[ChangedRow]["Checked"])));
        }

        private void ChangeCheckedStateForRow(Int32 ARow)
        {
            if (ARow >= 0)
            {
                FGridTableDV[ARow]["Checked"] = (System.Object)((!(Boolean)(FGridTableDV[ARow]["Checked"])));

                DataRow TmpDR = FGridTableDV[ARow].Row;

                if (!GridTableColumnChanged(ref TmpDR))
                {
                    FGridTableDV[ARow]["Checked"] = (System.Object)((!(Boolean)(FGridTableDV[ARow]["Checked"])));
                }
            }
        }

        private bool GridTableColumnChanged(ref DataRow AChangingRow)
        {
            Boolean IsRemoval;
            Boolean ReturnValue = false;

            ReturnValue = PerformContactAttributeAddOrRemoval(AChangingRow, out IsRemoval);

            // Give Focus back to the Grid and the Cells again so that the Selection can be moved with the Cursor keys
            grdContactAttributes.Focus();

            return ReturnValue;
        }

        private Boolean PerformContactAttributeAddOrRemoval(DataRow AChangingRow, out Boolean AIsRemoval)
        {
            Boolean ReturnValue = false;

            AIsRemoval = false;

            try
            {
                String AttributeCode = AChangingRow["AttributeCode"].ToString();
                String AttributeDetailCode = AChangingRow["AttributeDetailCode"].ToString();

                DataRow ExistingDataRow = FSelectedContactAttributeTable.Rows.Find(new object[] { FContactID, AttributeCode, AttributeDetailCode });

                if (ExistingDataRow == null)
                {
                    /*
                     * Add Contact Attribute
                     */

                    DataRow DeletedRow = null;

                    // check that this contact attribute hasn't previously been deleted
                    foreach (DataRow Row in FSelectedContactAttributeTable.Rows)
                    {
                        if ((Row.RowState == DataRowState.Deleted)
                            && (Convert.ToInt64(Row[PPartnerContactAttributeTable.GetContactIdDBName(), DataRowVersion.Original]) == FContactID)
                            && (Row[PPartnerContactAttributeTable.GetContactAttributeCodeDBName(),
                                    DataRowVersion.Original].ToString() == AttributeCode)
                            && (Row[PPartnerContactAttributeTable.GetContactAttrDetailCodeDBName(),
                                    DataRowVersion.Original].ToString() == AttributeDetailCode))
                        {
                            DeletedRow = Row;
                        }
                    }

                    if (DeletedRow != null)
                    {
                        // undelete the previously deleted row
                        DeletedRow.RejectChanges();
                    }
                    else
                    {
                        // Check: is this Contact Attribute assignable
                        PContactAttributeRow ContactAttribute =
                            (PContactAttributeRow)FContactAttributeTableDV.Table.Rows.Find(new Object[] { AttributeCode });
                        PContactAttributeDetailRow ContactAttributeDetail =
                            (PContactAttributeDetailRow)FContactAttributeDetailTableDV.Table.Rows.Find(new Object[] { AttributeCode,
                                                                                                                      AttributeDetailCode });

                        // -1 means this is being used in a find screen and we can select inactive attributes
                        if ((FContactID != -1) && (!ContactAttribute.Active || !ContactAttributeDetail.Active))
                        {
                            MessageBox.Show(
                                string.Format(Catalog.GetString("This Contact Attribute is inactive and cannot be added to this Contact Log."),
                                    AttributeCode),
                                Catalog.GetString("Inactive Contact Attribute"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return false;
                        }

                        // add new row to PartnerType table
                        PPartnerContactAttributeRow TheNewRow = FSelectedContactAttributeTable.NewRowTyped();
                        TheNewRow.ContactId = FContactID;
                        TheNewRow.ContactAttributeCode = AttributeCode;
                        TheNewRow.ContactAttrDetailCode = AttributeDetailCode;
                        FSelectedContactAttributeTable.Rows.Add(TheNewRow);
                    }

                    ReturnValue = true;
                    AIsRemoval = false;
                }
                else
                {
                    /*
                     * Remove Special Type
                     */

                    // Delete row

                    if (ExistingDataRow.RowState == DataRowState.Added)
                    {
                        FAddedAttributeDeleted = true;
                    }

                    ExistingDataRow.Delete();

                    ReturnValue = true;
                    AIsRemoval = true;
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
                ReturnValue = false;
            }

            return ReturnValue;
        }

        #region Events

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void GrdContactAttributes_Click(System.Object Sender, System.EventArgs e)
        {
            ChangeCheckedStateForRow(grdContactAttributes.MouseCellPosition.Row - 1);
        }

        private void GrdContactAttributes_EnterKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            ChangeCheckedStateForRow(e.Row);
        }

        private void GrdContactAttributes_SpaceKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            ChangeCheckedStateForRow(e.Row);
        }

        #endregion

        private class CustomValueChangedEvent : SourceGrid.Cells.Controllers.ControllerBase
        {
            TFrmContactAttributesDialog FParentClass;

            public CustomValueChangedEvent(TFrmContactAttributesDialog AParentClass)
            {
                FParentClass = AParentClass;
            }

            public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
            {
                base.OnValueChanged(sender, e);

                FParentClass.ChangedRowEvent(sender.Position.Row - 1);
            }
        }
    }
}