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
using Ict.Common.Controls;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using SourceGrid;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>Delegate declaration</summary>
    public delegate PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable TDelegatePartnerTypePropagationSelection(String APartnerTypeCode,
        String AAction);

    /// <summary>
    /// Contains logic for the UC_PartnerTypes UserControl.
    /// </summary>
    public class TUCPartnerTypesLogic
    {
        #region Resourcestrings

        private static readonly string StrPartnerHasCostCentreLink = Catalog.GetString(
            "This Partner is linked to a Cost Centre ({0}) in the\r\nFinance Module.  Remove the link before deleting\r\n" +
            "this Special Type.");
        private static readonly string StrPartnerHasCostCentreLinkTitle = Catalog.GetString("Cannot remove Partner Type");
        private static readonly string StrTheCodeIsNoLongerActive = Catalog.GetString(
            "The code '{0}' is no longer active.\r\nDo you still want to use it?");
        private static readonly string StrSecurityPreventsRemoval = Catalog.GetString(
            "You are not allowed to remove this Partner Type from the Partner\r\n" +
            "because of the security warning you have just received.");
        private static readonly string StrSecurityPreventsRemovalTitle = Catalog.GetString("Partner Type Removal Denied");

        #endregion

        private delegate void CheckChangedArgs (int ChangedRow);
        private event CheckChangedArgs ChangedRowEvent;

        private PartnerEditTDS FMainDS;
        private TFrmPetraEditUtils FPetraUtilsObject;
        private DataTable FPartnerTypesGridTable;
        private PTypeTable FDataCache_PartnerTypeListTable;
        private DataView FPartnerTypesGridTableDV;
        private DataView FDataCache_PartnerTypeListDV;
        private TSgrdDataGrid FDataGrid;
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
// TODO PartnerTypeFamilyMembersPropagationSelectionWinForm Dialog still missing
#if TODO
        private TDelegatePartnerTypePropagationSelection FDelegatePartnerTypePropagationSelection;
#endif
        private CustomValueChangedEvent FGridValueChangedEvent;

        #region Properties

        /// <summary>todoComment</summary>
        public PartnerEditTDS MultiTableDS
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

        /// <summary>todoComment</summary>
        public TFrmPetraEditUtils PetraUtilsObject
        {
            set
            {
                FPetraUtilsObject = value;
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
        #endregion

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;


        /// <summary>
        /// Loads Partner Types Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        public Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;


            // Load Partner Types, if not already loaded
            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PPartnerType == null)
                {
                    FMainDS.Tables.Add(new PPartnerTypeTable());
                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading)
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataPartnerTypes());

                    // Make DataRows unchanged - but only if this isn't a new PERSON
                    // where PartnerTypes were copied over from its FAMILY!
                    if (FMainDS.PPartnerType.Rows.Count > 0)
                    {
                        if (FMainDS.PPartnerType.Rows[0].RowState != DataRowState.Added)
                        {
                            FMainDS.PPartnerType.AcceptChanges();
                        }
                    }
                }

                if (FMainDS.PPartnerType.Rows.Count != 0)
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
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void LoadTypes()
        {
            FDataCache_PartnerTypeListTable = (PTypeTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.PartnerTypeList);
            FDataCache_PartnerTypeListDV = FDataCache_PartnerTypeListTable.DefaultView;
            FDataCache_PartnerTypeListDV.Sort = PTypeTable.GetTypeCodeDBName();
        }

        private void PartnerTypesGridTableColumnChanged(ref DataRow AChangingPartnerTypeRow)
        {
            Int32 TmpRowIndex;
            String TmpTypeCode;
            DataRowView TmpDataRowView;
            Boolean IsRemoval;

            TmpTypeCode = AChangingPartnerTypeRow["TypeCode"].ToString();

            if (PerformPartnerTypeAddOrRemoval(AChangingPartnerTypeRow, out IsRemoval))
            {
                FPetraUtilsObject.SetChangedFlag();
            }
            else
            {
                // Uncheck row in the Grid
                TmpDataRowView = DetermineRowToSelect(TmpTypeCode);
                TmpRowIndex = FDataGrid.DataSourceRowToIndex2(TmpDataRowView);
                FPartnerTypesGridTableDV[TmpRowIndex]["Checked"] = false;
            }

            // Give Focus back to the Grid and the Cells again so that the Selection can be moved with the Cursor keys
            FDataGrid.Focus();
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
        /// todoComment
        /// </summary>
        /// <param name="AChangingPartnerTypeRow"></param>
        /// <param name="AIsRemoval"></param>
        /// <returns></returns>
        public Boolean PerformPartnerTypeAddOrRemoval(DataRow AChangingPartnerTypeRow, out Boolean AIsRemoval)
        {
            const String TYPECODE_COSTCENTRE = "COSTCENTRE";

            Boolean ReturnValue = false;

            AIsRemoval = false;
            String TypeCode;
            DataRow ExistingMatchingDataRow;
            PPartnerTypeTable PartnerTypeTable;
            PPartnerTypeRow TheNewRow;
            PTypeRow CheckTypeRow;
            DataRowView[] CheckTypeRows;
            DialogResult CheckTypeRowsAnswer;
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;
            String CostCentreLink;

            PartnerTypeTable = FMainDS.PPartnerType;

            try
            {
                TypeCode = AChangingPartnerTypeRow["TypeCode"].ToString();

                ExistingMatchingDataRow = PartnerTypeTable.Rows.Find(new Object[] { ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey, TypeCode });

                if (ExistingMatchingDataRow == null)
                {
                    /*
                     * Add Special Type
                     */

                    // Check security permission
                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PPartnerTypeTable.GetTableDBName()))
                    {
                        TMessages.MsgSecurityException(new ESecurityDBTableAccessDeniedException("", "create",
                                PPartnerTypeTable.GetTableDBName()), this.GetType());

                        AChangingPartnerTypeRow.CancelEdit();   // reset to unchecked
                        return false;
                    }

                    // Check: is this Partner Type assignable?

//                  MessageBox.Show("Perform check: is PartnerType assignable?  TypeCode: " + TypeCode);
                    CheckTypeRows = FDataCache_PartnerTypeListDV.FindRows(TypeCode);

                    if (CheckTypeRows.Length > 0)
                    {
                        CheckTypeRow = (PTypeRow)CheckTypeRows[0].Row;

                        if (!CheckTypeRow.ValidType)
                        {
                            /*CheckTypeRowsAnswer = TMessages.MsgQuestion(
                             *  ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE, TypeCode),
                             *  this.GetType(), false);*/

                            CheckTypeRowsAnswer = MessageBox.Show(
                                string.Format(StrTheCodeIsNoLongerActive, TypeCode),
                                Catalog.GetString("Invalid Data Entered"),
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2);

                            if (CheckTypeRowsAnswer == DialogResult.No)
                            {
                                // reset to unchecked
                                AChangingPartnerTypeRow.CancelEdit();
                                return false;
                            }
                        }
                    }

                    // add new row to PartnerType table
                    PartnerTypeTable = FMainDS.PPartnerType;
                    TheNewRow = PartnerTypeTable.NewRowTyped();
                    TheNewRow.PartnerKey = FMainDS.PPartner[0].PartnerKey;
                    TheNewRow.TypeCode = TypeCode;
                    TheNewRow.CreatedBy = UserInfo.GUserInfo.UserID;
                    TheNewRow.DateCreated = DateTime.Now.Date;
                    PartnerTypeTable.Rows.Add(TheNewRow);

                    // Fire OnRecalculateScreenParts event
                    RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
                    RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
                    OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);

                    ReturnValue = true;
                    AIsRemoval = false;
                }
                else
                {
                    /*
                     * Remove Special Type
                     */

                    // Check security permission
                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapDELETE, PPartnerTypeTable.GetTableDBName()))
                    {
                        TMessages.MsgSecurityException(new ESecurityDBTableAccessDeniedException("", "delete",
                                PPartnerTypeTable.GetTableDBName()), this.GetType());

                        // reset to checked
                        AChangingPartnerTypeRow.CancelEdit();
                        return false;
                    }

                    // perform check: If COSTCENTRE is to be removed then check whether Partner has a link to costcentre set up
                    if (TypeCode == TYPECODE_COSTCENTRE)
                    {
                        try
                        {
                            if (FPartnerEditUIConnector.HasPartnerCostCentreLink(out CostCentreLink))
                            {
                                MessageBox.Show(String.Format(StrPartnerHasCostCentreLink, CostCentreLink,
                                        StrPartnerHasCostCentreLinkTitle));

                                // reset to checked
                                AChangingPartnerTypeRow.CancelEdit();
                                return false;
                            }
                        }
                        catch (ESecurityAccessDeniedException Exp)
                        {
                            TMessages.MsgSecurityException(Exp, this.GetType());
                            MessageBox.Show(StrSecurityPreventsRemoval, StrSecurityPreventsRemovalTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            // reset to checked
                            AChangingPartnerTypeRow.CancelEdit();
                            return false;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }

                    // Delete row from PartnerType table
                    ExistingMatchingDataRow.Delete();

                    // Fire OnRecalculateScreenParts event
                    RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
                    RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
                    OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);

                    ReturnValue = true;
                    AIsRemoval = true;
                }

                /*
                 * Check if this change could be applied to Family Members
                 */
// TODO PartnerTypeFamilyMembersPropagationSelectionWinForm Dialog still missing
#if TODO
                if (SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass) == TPartnerClass.FAMILY)
                {
                    if (HasFamilyFamilyMembers())
                    {
                        if (FDelegatePartnerTypePropagationSelection != null)
                        {
                            if (!AIsRemoval)
                            {
                                FMainDS.Merge(FDelegatePartnerTypePropagationSelection(TypeCode, "ADD"));
                            }
                            else
                            {
                                FMainDS.Merge(FDelegatePartnerTypePropagationSelection(TypeCode, "DELETE"));
                            }
                        }
                    }
                }
#endif
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATypeCode"></param>
        /// <returns></returns>
        public DataRowView DetermineRowToSelect(String ATypeCode)
        {
            DataView PartnerTypesTableView;
            DataRowView TmpDataRowView;
            String RowFilterCriteria;

            PartnerTypesTableView = new DataView(FPartnerTypesGridTable);
            RowFilterCriteria = "TypeCode = '" + ATypeCode + "'";

            // MessageBox.Show('PartnerTypesTableView.RowFilter: ' + RowFilterCriteria);
            PartnerTypesTableView.RowFilter = RowFilterCriteria;

            // MessageBox.Show('PartnerTypesTableView.RowFilter applied!  Count: ' + PartnerTypesTableView.Count.ToString);
            TmpDataRowView = PartnerTypesTableView[0];
            PartnerTypesTableView.RowFilter = "";

            // MessageBox.Show(TmpDataRowView['TypeCode'].ToString);
            return TmpDataRowView;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void CreateTempPartnerTypesTable()
        {
            FPartnerTypesGridTable = new DataTable("PartnerTypesList");
            FPartnerTypesGridTable.Columns.Add("Checked", System.Type.GetType("System.Boolean"));
            FPartnerTypesGridTable.Columns.Add("TypeCode", System.Type.GetType("System.String"));
            FPartnerTypesGridTable.Columns.Add("TypeDescription", System.Type.GetType("System.String"));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void FillTempPartnerTypesTable()
        {
            DataTable SelectedPartnerTypeTable;
            DataView SelectedPartnerTypeTableDV;
            DataView UnselectedPartnerTypeTableDV;
            DataRow TheNewRow;
            Int16 RowCounter;
            String TypeDescription;
            Int32 TypeDescriptionInCachePosition;

            // TODO 2 oChristianK cList Sorting : Sort both the checked and not checked List items so that invalid Types are sorted to the end of both lists, respectively.

            FPartnerTypesGridTable.Rows.Clear();
            FPartnerTypesGridTable.AcceptChanges();

            // Sort data
            SelectedPartnerTypeTable = FMainDS.PPartnerType;
            SelectedPartnerTypeTableDV = SelectedPartnerTypeTable.DefaultView;
            SelectedPartnerTypeTableDV.Sort = PTypeTable.GetTypeCodeDBName();

            UnselectedPartnerTypeTableDV = FDataCache_PartnerTypeListTable.DefaultView;
            UnselectedPartnerTypeTableDV.Sort = PTypeTable.GetTypeCodeDBName();

            // first add Special Types which are already selected for partner
            for (RowCounter = 0; RowCounter <= SelectedPartnerTypeTableDV.Count - 1; RowCounter += 1)
            {
                #region Determine Type Description
                TypeDescriptionInCachePosition =
                    FDataCache_PartnerTypeListDV.Find(SelectedPartnerTypeTableDV[RowCounter][PTypeTable.GetTypeCodeDBName()]);

                if (TypeDescriptionInCachePosition != -1)
                {
                    TypeDescription = FDataCache_PartnerTypeListDV[TypeDescriptionInCachePosition][PTypeTable.GetTypeDescriptionDBName()].ToString();

                    // If this Type is inactive, show it.
                    if (!Convert.ToBoolean(FDataCache_PartnerTypeListDV[TypeDescriptionInCachePosition][PTypeTable.GetValidTypeDBName()]))
                    {
                        TypeDescription = TypeDescription + MCommonResourcestrings.StrGenericInactiveCode;
                    }
                }
                else
                {
                    TypeDescription = "";
                }

                #endregion

                TheNewRow = FPartnerTypesGridTable.NewRow();
                TheNewRow["Checked"] = (System.Object)true;
                TheNewRow["TypeCode"] = SelectedPartnerTypeTableDV[RowCounter][PTypeTable.GetTypeCodeDBName()];
                TheNewRow["TypeDescription"] = TypeDescription;
                FPartnerTypesGridTable.Rows.Add(TheNewRow);
            }

            // second add the rest of the Special Types in db
            for (RowCounter = 0; RowCounter <= UnselectedPartnerTypeTableDV.Count - 1; RowCounter += 1)
            {
                // only add row if it has not already been added as a checked row
                if (SelectedPartnerTypeTableDV.Find(
                        new object[] { UnselectedPartnerTypeTableDV[RowCounter][PTypeTable.GetTypeCodeDBName()] }) == -1)
                {
                    #region Determine Type Description
                    TypeDescription = UnselectedPartnerTypeTableDV[RowCounter][PTypeTable.GetTypeDescriptionDBName()].ToString();

                    // If this Type is inactive, show it.
                    if (!Convert.ToBoolean(UnselectedPartnerTypeTableDV[RowCounter][PTypeTable.GetValidTypeDBName()]))
                    {
                        TypeDescription = TypeDescription + MCommonResourcestrings.StrGenericInactiveCode;
                    }

                    #endregion

                    TheNewRow = FPartnerTypesGridTable.NewRow();
                    TheNewRow["Checked"] = (System.Object)false;
                    TheNewRow["TypeCode"] = UnselectedPartnerTypeTableDV[RowCounter][PTypeTable.GetTypeCodeDBName()];
                    TheNewRow["TypeDescription"] = TypeDescription;
                    FPartnerTypesGridTable.Rows.Add(TheNewRow);
                }
            }
        }

        private Boolean HasFamilyFamilyMembers()
        {
            Boolean ReturnValue;
            DataView TmpDV;

            ReturnValue = false;

            if (FMainDS.Tables.Contains(PartnerEditTDSFamilyMembersTable.GetTableName()))
            {
                TmpDV = new DataView(FMainDS.FamilyMembers, "", "", DataViewRowState.CurrentRows);

                if (TmpDV.Count > 0)
                {
                    ReturnValue = true;
                }
            }
            else
            {
                if (FMainDS.MiscellaneousData[0].ItemsCountFamilyMembers > 0)
                {
                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }

// TODO PartnerTypeFamilyMembersPropagationSelectionWinForm Dialog still missing
#if TODO
        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADelegate"></param>
        public void InitialiseDelegatePartnerTypePropagationSelection(TDelegatePartnerTypePropagationSelection ADelegate)
        {
            FDelegatePartnerTypePropagationSelection = ADelegate;
        }

#endif


        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ARow"></param>
        public void ChangeCheckedStateForRow(Int32 ARow)
        {
            if (ARow >= 0)
            {
                FPartnerTypesGridTableDV[ARow]["Checked"] = (System.Object)((!(Boolean)(FPartnerTypesGridTableDV[ARow]["Checked"])));

                DataRow TmpDR = FPartnerTypesGridTableDV[ARow].Row;
                PartnerTypesGridTableColumnChanged(ref TmpDR);
            }
        }

        #region Setup SourceDataGrid

        /// <summary>
        /// todoComment
        /// </summary>
        public void CreateColumns()
        {
            // CheckBox
            FDataGrid.AddCheckBoxColumn("", FPartnerTypesGridTable.Columns["Checked"], 17, false);

            // Special Type
            FDataGrid.AddTextColumn("Special Type", FPartnerTypesGridTable.Columns["TypeCode"]);

            // Description
            FDataGrid.AddTextColumn("Description", FPartnerTypesGridTable.Columns["TypeDescription"]);
        }

        /// <summary>
        /// Sets up the DataBinding of the Grid.
        /// </summary>
        /// <returns>void</returns>
        public void DataBindGrid()
        {
            FPartnerTypesGridTableDV = FPartnerTypesGridTable.DefaultView;
            FPartnerTypesGridTableDV.AllowNew = false;
            FPartnerTypesGridTableDV.AllowEdit = true;
            FPartnerTypesGridTableDV.AllowDelete = false;

            // DataBind the DataGrid
            FDataGrid.DataSource = new DevAge.ComponentModel.BoundDataView(FPartnerTypesGridTableDV);

            // Hook Grid event that allows popping up a question whether to check the CheckBox
            FGridValueChangedEvent = new CustomValueChangedEvent(this);
            FDataGrid.Controller.AddController(FGridValueChangedEvent);
            this.ChangedRowEvent += new CheckChangedArgs(ChangedRowEventHandler);
        }

        // this is only called when the user clicks on the 'CheckBox' column and the grid automatically checks or unchecks a CheckBox
        private void ChangedRowEventHandler(int ChangedRow)
        {
//          MessageBox.Show("ChangedRowEventHandler: " + FPartnerTypesGridTableDV[ChangedRow].Row[0].ToString() + " / " + FPartnerTypesGridTableDV[ChangedRow].Row[1].ToString());

            // Our code also checks/uncheck a CheckBox when the user clicks in the 'CheckBox' column so we have a double check.
            // I.e. the checkbox is returned to the original value. We need to check/uncheck again to redo this.
            DataRow TmpDR = FPartnerTypesGridTableDV[ChangedRow].Row;

            FPartnerTypesGridTableDV[ChangedRow]["Checked"] = (System.Object)((!(Boolean)(FPartnerTypesGridTableDV[ChangedRow]["Checked"])));
        }

        private class CustomValueChangedEvent : SourceGrid.Cells.Controllers.ControllerBase
        {
            TUCPartnerTypesLogic FParentClass;

            public CustomValueChangedEvent(TUCPartnerTypesLogic AParentClass)
            {
                FParentClass = AParentClass;
            }

            public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
            {
                base.OnValueChanged(sender, e);

                FParentClass.ChangedRowEvent(sender.Position.Row - 1);
            }
        }
        #endregion
    }
}