//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       martaj
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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Common.Controls;
using SourceGrid;
using SourceGrid.Cells;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared;
using Ict.Common.Remoting.Client;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// Contains logic for the UC_PartnerInterests UserControl.
    /// </summary>
    public class TUCPartnerInterestsLogic
    {
        #region Resourcestrings

        /// <summary>todoComment</summary>
        private static readonly string StrDeleteQuestion = Catalog.GetString("Are you sure you want to remove the Interest {0} from the database?");

        /// <summary>todoComment</summary>
        private static readonly string StrDeleteQuestionTitle = Catalog.GetString("Delete Interest?");

        #endregion

        private PartnerEditTDS FMultiTableDS;

        /// <summary>Holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private DataView FPartnerInterestsGridTableDV;
        private DataView FInterestDV;
        private DataTable FDataCacheInterestListTable;
        private DataTable FPartnerInterestsGridTable;
        private DataRow FInterestRow;
        private String FInterest;
        private Int32 FInterestNumber;

        /// <summary>true if a record is being added, otherwise false</summary>
        private Boolean FIsRecordBeingAdded;

        /// <summary>todoComment</summary>
        public PartnerEditTDS MultiTableDS
        {
            get
            {
                return FMultiTableDS;
            }

            set
            {
                FMultiTableDS = value;
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
        public Boolean IsRecordBeingAdded3
        {
            get
            {
                return FIsRecordBeingAdded;
            }

            set
            {
                FIsRecordBeingAdded = value;
            }
        }

        /// <summary>todoComment</summary>
        public String Interest
        {
            get
            {
                return FInterest;
            }

            set
            {
                FInterest = value;
            }
        }

        /// <summary>todoComment</summary>
        public Int32 InterestNumber
        {
            get
            {
                return FInterestNumber;
            }

            set
            {
                FInterestNumber = value;
            }
        }

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #region UCPartnerInterestsLogic

        /// <summary>
        /// constructor
        /// </summary>
        public TUCPartnerInterestsLogic() : base()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <param name="YesNo"></param>
        public void AllowDelete(TSgrdDataGrid AGrid, Boolean YesNo)
        {
            FPartnerInterestsGridTableDV.AllowDelete = YesNo;

            // DataBind the DataGrid
            AGrid.DataSource = new DevAge.ComponentModel.BoundDataView(FPartnerInterestsGridTableDV);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void FireRecalculateScreenPartsEventArgs()
        {
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        public String GetInterestCategory(String Category)
        {
            DataRowView[] Ar;
            Ar = this.FInterestDV.FindRows(Category);
            return Ar[0][PInterestTable.GetCategoryDBName()].ToString();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public Int32 GetInterestNumber()
        {
            return FInterestNumber;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AInterest"></param>
        /// <returns></returns>
        public DataRow GetSelectedDataRow(String AInterest)
        {
            return FMultiTableDS.PPartnerInterest.Rows.Find(new Object[] { AInterest, FMultiTableDS.PPartner[0].PartnerKey });
        }

        /// <summary>
        /// Loads Interest Data from Petra Server into FMultiTableDS.
        ///
        /// </summary>
        /// <returns>true if successful, otherwise false.
        /// </returns>
        public Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            // Load Interests, if not already loaded
            try
            {
                // Make sure that Typed DataTables are already there on Client side
                if (FMultiTableDS.PPartnerInterest == null)
                {
                    FMultiTableDS.Tables.Add(new PPartnerInterestTable());
                    FMultiTableDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading)
                {
                    FMultiTableDS.PPartnerInterest.AcceptChanges();
                }

                if (FMultiTableDS.PPartnerInterest.Rows.Count != 0)
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
        public void LoadInterests()
        {
            // Load Interests
            FDataCacheInterestListTable = (PInterestTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.InterestList);

            // Specify the DataView that the SourceDataGrid will be bound to
            FInterestDV = FDataCacheInterestListTable.DefaultView;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
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
        /// <param name="APartnerInterestRow"></param>
        public void ProcessEditedRecord(out PPartnerInterestRow APartnerInterestRow)
        {
            // Exampel from EditRecord
            // InterestRow := (FMultiTableDS.PPartnerInterest.Rows.Find(
            // [FMultiTableDS.PPartner.Row[0].PartnerKey, FInterestNumber]) as PPartnerInterestRow);
            // [Finterest,FMulti...
            // this is the same always????
            APartnerInterestRow =
                ((PPartnerInterestRow)FMultiTableDS.PPartnerInterest.Rows.Find(new Object[] { FMultiTableDS.PPartner[0].PartnerKey, FInterestNumber }));

            // Call EndEdit on all DataRows that are editable in the Detail UserControl.
            // THIS IS ESSENTIAL!!! It makes the changes that the user has made permanent.
            if (APartnerInterestRow == null)
            {
                MessageBox.Show("TUCPartnerInterestsLogic.ProcessEditedRecord:  APartnerInterestRow is nil" + FInterest.ToString());
            }
            else
            {
                APartnerInterestRow.EndEdit();

                // New Address?
                if ((APartnerInterestRow.RowState == DataRowState.Added) && FIsRecordBeingAdded)
                {
                    FIsRecordBeingAdded = false;

                    // The 4GL Petra screen didn't set the StatusCode to active when a new Subscription was added.
                    // FMultiTableDS.PPartner.Row[0].StatusCode := 'ACTIVE';
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataView"></param>
        /// <param name="tmpInterest"></param>
        /// <returns></returns>
        public DataRowView DetermineRecordToSelect(DataView ADataView, String tmpInterest)
        {
            DataRowView ReturnValue = null;
            DataRowView TmpDataRowView;

            ADataView.RowFilter = PInterestTable.GetInterestDBName() + " = '" + tmpInterest + "'";
            TmpDataRowView = ADataView[0];
            try
            {
                ReturnValue = TmpDataRowView;
            }
            catch (Exception)
            {
            }
            ADataView.RowFilter = "";
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <param name="tmpDT"></param>
        /// <param name="IsNew"></param>
        public void RefreshDataGrid(ref TSgrdDataGrid AGrid, DataTable tmpDT, Boolean IsNew)
        {
            DataRow TheNewRow;
            PPartnerInterestRow tmpPartnerInterestRow;
            PInterestRow tmpInterestRow = null; // todo: this needs to be assigned? says Timo while converting from delphi
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;
            Int32 counter;
            StringCollection ACollection = new StringCollection();
            StringCollection SortedCollection;

            // AInterestRow : PInterestRow;
            MessageBox.Show("TmpDT count:" + tmpDT.Rows.Count.ToString());
            tmpPartnerInterestRow =
                ((PPartnerInterestRow)FMultiTableDS.PPartnerInterest.Rows.Find(new Object[] { FMultiTableDS.PPartner[0].PartnerKey, FInterestNumber }));

            // tmpPartnerInterestRow := tmpDT.rows[tmpDT.rows.count1] as PPartnerInterestRow;
            // tmpInterestRow := tmpDT.rows[tmpDT.rows.count1] as PInterestRow;
            // MessageBox.Show('Refreshgrid: '+tmpPartnerInterestRow.Interest);
            // AInterestRow := (FMultiTableDS.PInterest.Rows.Find(new Object[]{[tmpPartnerInterestRow.Interest]}) as PinterestRow);
            FPartnerInterestsGridTable = new DataTable();
            TheNewRow = FPartnerInterestsGridTable.NewRow();

            // TheNewRow['Category'] := AInterestROw.Category as System.Object;
            // TheNewRow['Category'] := 'DOG';
            TheNewRow["Interest"] = (System.Object)tmpPartnerInterestRow.Interest;
            TheNewRow["Country"] = (System.Object)tmpPartnerInterestRow.Country;
            TheNewRow["Field"] = (System.Object)tmpPartnerInterestRow.FieldKey;
            TheNewRow["Level"] = (System.Object)tmpPartnerInterestRow.Level;
            TheNewRow["Comment"] = (System.Object)tmpPartnerInterestRow.Comment;
            FPartnerInterestsGridTable.Rows.Add(TheNewRow);
            AGrid.DataSource = new DevAge.ComponentModel.BoundDataView(FPartnerInterestsGridTable.DefaultView);
            counter = 0;

            foreach (DataRow tmpRow in FPartnerInterestsGridTable.Rows)
            {
                ACollection.Add(tmpRow[PInterestTable.GetCategoryDBName()].ToString());
            }

            SortedCollection = StringHelper.StrSort(ACollection);

            foreach (String AInterestString in SortedCollection)
            {
                counter = counter + 1;

                if (AInterestString == tmpInterestRow.Interest.ToString())
                {
                    break;
                }
            }

            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
            AGrid.Selection.Focus(new Position(1, 1), true);
            AGrid.Selection.ResetSelection(false);
            AGrid.Selection.SelectRow(counter, true);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void EditRecord()
        {
            PPartnerInterestRow InterestRow;

            InterestRow =
                ((PPartnerInterestRow)FMultiTableDS.PPartnerInterest.Rows.Find(new Object[] { FMultiTableDS.PPartner[0].PartnerKey, FInterestNumber }));
            InterestRow.BeginEdit();
        }

        /// <summary>
        /// DataBinds the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DataBindGrid(TSgrdDataGrid AGrid)
        {
            FPartnerInterestsGridTableDV = FMultiTableDS.PPartnerInterest.DefaultView;
            FPartnerInterestsGridTableDV.AllowNew = false;
            FPartnerInterestsGridTableDV.AllowEdit = false;
            FPartnerInterestsGridTableDV.AllowDelete = false;

            // DataBind the DataGrid
            AGrid.DataSource = new DevAge.ComponentModel.BoundDataView(FPartnerInterestsGridTableDV);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <param name="ARow"></param>
        /// <returns></returns>
        public String DetermineCurrentInterest(TSgrdDataGrid AGrid, Int32 ARow)
        {
            DataRowView TheDataRowView;

            TheDataRowView = (DataRowView)AGrid.Rows.IndexToDataSourceRow(ARow);
            try
            {
                FInterestRow = TheDataRowView.Row;
                FInterest = FInterestRow[PPartnerInterestTable.GetInterestDBName()].ToString();
                FInterestNumber = Convert.ToInt32(FInterestRow[PPartnerInterestTable.GetInterestNumberDBName()]);
            }
            catch (NullReferenceException)
            {
            }

            // no interests: to do?
            // MessageBox.Show('FInterest of currently selected Grid Row: ' + FInterest);
            return FInterest;
        }

        /// <summary>
        /// This function probably deletes the record in the database
        /// </summary>
        /// <returns>void</returns>
        public Boolean DeleteRecord(Int64 PPartnerKey, Int32 InterestNumber)
        {
            Boolean ReturnValue;
            DataRow InterestRow = null;
            DialogResult Chosen;

            try
            {
                InterestRow = FMultiTableDS.PPartnerInterest.Rows.Find(new Object[] { FMultiTableDS.PPartner[0].PartnerKey, FInterestNumber });
            }
            catch (Exception)
            {
            }

            Chosen = MessageBox.Show(StrDeleteQuestion,
                StrDeleteQuestionTitle,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (Chosen == DialogResult.Yes)
            {
                InterestRow.Delete();
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// old function DetermineCurrentPartnerInterestKey(AGrid: TSgrdDataGrid; ARow: Int32): int64;
        /// </summary>
        /// <returns>void</returns>
        public void DetermineCurrentPartnerInterestKey(out Int64 Partnerkey, out Int32 InterestNumber, out String Interest)
        {
            Partnerkey = FMultiTableDS.PPartnerInterest[0].PartnerKey;
            InterestNumber = FMultiTableDS.PPartnerInterest[0].InterestNumber;
            Interest = FMultiTableDS.PPartnerInterest[0].Interest;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <returns></returns>
        public String DetermineCurrentInterest(TSgrdDataGrid AGrid)
        {
            DataRowView[] TheDataRowViewArray;
            TheDataRowViewArray = AGrid.SelectedDataRowsAsDataRowView;

            // MessageBox.Show(TUCPartnerInterestsLogic.DetermineCurrentInterest.TheDataRowViewArray);
            FInterest = TheDataRowViewArray[0].Row[PPartnerInterestTable.GetInterestDBName()].ToString();
            return FInterest;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <param name="ARowNumber"></param>
        /// <param name="AInterest"></param>
        public void DetermineInitiallySelectedInterest(TSgrdDataGrid AGrid, out Int32 ARowNumber, out String AInterest)
        {
            AInterest = "";
            PPartnerInterestRow InterestRow;
            ARowNumber = 1;
            try
            {
                InterestRow = (PPartnerInterestRow)((DataRowView)AGrid.Rows.IndexToDataSourceRow(1)).Row;
                AInterest = InterestRow.Interest;
            }
            catch (NullReferenceException)
            {
            }
        }

        /// <summary>
        /// Useful if the user happens to add an interest number that is already listed */
        /// </summary>
        /// <returns>void</returns>
        public void DetermineNewPrimaryKeys(out Int64 Partnerkey, out Int32 InterestNumber)
        {
            InterestNumber = -1;

            // tmpRow : DataRow;
            Partnerkey = FMultiTableDS.PPartner[0].PartnerKey;

            foreach (PPartnerInterestRow ARow in FPartnerInterestsGridTableDV.Table.Rows)
            {
                if ((ARow.InterestNumber.ToString() == PPartnerInterestTable.GetInterestNumberDBName()) || (ARow.InterestNumber == 0))
                {
                    MessageBox.Show("MJ " + PPartnerInterestTable.GetInterestNumberDBName());
                    MessageBox.Show("Marta BB " + ARow.InterestNumber.ToString());

                    // No interests exists for this partner (partner doesn't exist in the database)
                    InterestNumber = 0;
                }
                else
                {
                    // This partner has interests already (exists in the database)
                    InterestNumber = InterestNumber + 1;
                    MessageBox.Show("Marta BBB " + ARow.InterestNumber.ToString());

                    // Find the first Interest that the Partner does not have yet.
                    // tmpRow := FMultiTableDS.PPartnerInterest.Rows.Find(new Object[]{[PartnerKey, InterestNumber]});
                    // if tmpRow = nil then
                    // begin
                    // InterestNumber := ARow.InterestNumber;
                    // break;
                    // end;
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="InterestNumber"></param>
        /// <param name="Interest"></param>
        /// <param name="Category"></param>
        public void DetermineNextInterest(Int32 InterestNumber, out String Interest, out String Category)
        {
            DataRow tmpRow;

            Interest = "";
            Category = "";

            if (InterestNumber.ToString() == PPartnerInterestTable.GetInterestNumberDBName())
            {
                // This partner has no interests and therefore the first one is picked up
                // as a starting point. Will be edited later.
                foreach (PInterestRow ARow in FInterestDV.Table.Rows)
                {
                    tmpRow = FMultiTableDS.PInterest.Rows.Find(new Object[] { ARow.Interest });

                    if (tmpRow == null)
                    {
                    }
                    else
                    {
                        Interest = ARow.Interest;
                        Category = ARow.Category;
                        break;
                    }
                }

                // MessageBox.Show('TUCPartnerInterestsLogic.DetermineNewInterest: interest determined');
            }
            else
            {
            }

            // This partner has interests already.
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public Int32 DetermineNextInterestKey()
        {
            InterestNumber = -1;

            // go through all interests the partner has
            foreach (PPartnerInterestRow APartnerInterestRow in FPartnerInterestsGridTableDV)
            {
                try
                {
                    // note the interest number
                    InterestNumber = APartnerInterestRow.InterestNumber;
                }
                catch (Exception)
                {
                }

                // MessageBox.Show(Exp.ToString + 'No more interests exist');
            }

            // add one so we know it is a new one
            InterestNumber = InterestNumber + 1;

            // MessageBox.Show('Interest Number = ' + InterestNumber.ToString);
            // return the answer
            return InterestNumber;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        public void CreateColumns(TSgrdDataGrid AGrid)
        {
            // var
            // ForeignTableColumn: DataColumn;
            // Add column Category in FMultiTableDS. Info is taken from the Interest table.
            // Rest of info comes from PartnerInterest table.
            // ForeignTableColumn := new DataColumn();
            // ForeignTableColumn.DataType := System.Type.GetType('System.String');
            // ForeignTableColumn.ColumnName := 'Parent_' + PInterestTable.GetCategoryDBName();
            // ForeignTableColumn.Expression := 'Parent.' + PInterestTable.GetCategoryDBName();
            // FMultiTableDS.EnableRelation('PartnerInterestCategory');
            // FMultiTableDS.PPartnerInterest.Columns.Add(ForeignTableColumn);
            // Agrid.AddTextColumn('Category', FMultiTableDS.PInterest.ColumnCategory);
            // AGrid.AddTextColumn('Category', FMultiTableDS.PPartnerInterest.Columns[
            // 'Parent_' + PInterestTable.GetCategoryDBName()]);
            // ????
            // AGrid.AddTextColumn('Category', FMultiTableDS.Tables. );
            // ????
            AGrid.AddTextColumn("Interest", FMultiTableDS.PPartnerInterest.ColumnInterest);
            AGrid.AddTextColumn("Country", FMultiTableDS.PPartnerInterest.ColumnCountry);
            AGrid.AddTextColumn("Field", FMultiTableDS.PPartnerInterest.ColumnFieldKey);
            AGrid.AddTextColumn("Level", FMultiTableDS.PPartnerInterest.ColumnLevel);
            AGrid.AddTextColumn("Comment", FMultiTableDS.PPartnerInterest.ColumnComment);

            // Agrid.Columns[5].DataCell.Editor := nil;
        }

        /// <summary>
        /// Creates a new row in the grid
        /// </summary>
        /// <returns>void</returns>
        public void CreateNewInterestRow(Int64 PartnerKey, Int32 InterestNumber, String Interest, String Category)
        {
            PPartnerInterestRow newPPartnerInterestRow;

            // When Category below has been solved FIsRecordBeingAdded will be true.Until then
            // below line is marked off!!!
            FIsRecordBeingAdded = true;

            // Add new Locations row
            newPPartnerInterestRow = FMultiTableDS.PPartnerInterest.NewRowTyped(true);

            // Assign Primary Key columns
            newPPartnerInterestRow.PartnerKey = PartnerKey;
            newPPartnerInterestRow.InterestNumber = InterestNumber;
            newPPartnerInterestRow.Interest = Interest;

            // newPPartnerInterestRow.Category := Category;
            FMultiTableDS.PPartnerInterest.Rows.Add(newPPartnerInterestRow);

            // MessageBox.Show('TUCPartnerInterestsLogic.CreateNewInterestRow: new row added to FMultiTableDS');
        }

        #endregion
    }
}