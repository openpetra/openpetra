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
using System.Collections;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Common.Controls;
using SourceGrid;
using SourceGrid.Cells;
using Ict.Common;
using System.Collections.Specialized;
using System.Windows.Forms;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// Contains logic for the UC_PartnerSubscriptions UserControl.
    /// </summary>
    public class TUCPartnerSubscriptionsLogic : IDisposable
    {
        #region Resourcestrings

        private static readonly string StrDeleteQuestionLine = Catalog.GetString(
            "Are you sure you want to remove this Subscription from the database?");

        private static readonly string StrDeleteQuestionTitle = Catalog.GetString("Delete Subscription?");

        private static readonly string StrPartnerReActivationBecauseOfNewSubscr = Catalog.GetString(" because you have added\r\na new Subscription!");

        private static readonly string StrSubscriptionValueCancelled = Catalog.GetString("CANCELLED");

        private static readonly string StrSubscriptionValueExpired = Catalog.GetString("EXPIRED");

        private static readonly string StrInactive = Catalog.GetString(" (inactive)");

        #endregion

        private PartnerEditTDS FMultiTableDS;

        /// <summary>Holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private DataTable FDataCachePublicationListTable;
        private DataTable FPartnerSubscriptionsGridTable;
        private PPublicationCostTable FPublicationCostDT;
        private DataView FPartnerSubscriptionsGridTableDV;
        private DataView FPublicationsDV;
        private DataRow FSubscriptionRow;
        private IPartnerUIConnectorsExtractsAddSubscriptions FPartnerUIConnectorsExtracts;
        private String FPublicationCode;

        /// <summary>true if a record is beeing added, otherwise false</summary>
        private Boolean FIsRecordBeingAdded;
        private DataRow FLastRowAdded;

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
        public String PublicationCode
        {
            get
            {
                return FPublicationCode;
            }

            set
            {
                FPublicationCode = value;
            }
        }

        /// <summary>todoComment</summary>
        public IAsynchronousExecutionProgress AsyncExecProgress
        {
            get
            {
                IAsynchronousExecutionProgress ReturnValue;

                if (FPartnerUIConnectorsExtracts != null)
                {
                    ReturnValue = FPartnerUIConnectorsExtracts.AsyncExecProgress;
                }
                else
                {
                    ReturnValue = null;
                }

                return ReturnValue;
            }
        }

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #region TUCPartnerSubscriptionsLogic

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <param name="YesNo"></param>
        public void AllowDelete(TSgrdDataGrid AGrid, Boolean YesNo)
        {
            FPartnerSubscriptionsGridTableDV.AllowDelete = YesNo;

            // DataBind the DataGrid
            AGrid.DataSource = new DevAge.ComponentModel.BoundDataView(FPartnerSubscriptionsGridTableDV);
        }

        /// <summary>
        /// Counts all cancelable Subscriptions.
        ///
        /// </summary>
        /// <returns>Number of cancelable Subscriptions
        /// </returns>
        public Int32 CancelAllSubscriptionsCount()
        {
            return CancelAllSubscriptions("", DateTime.MinValue).Count;
        }

        /// <summary>
        /// Cancels all Subscriptions (that are not already CANCELED or EXPIRED).
        ///
        /// </summary>
        /// <param name="AReasonEnded">Text that gives the reason for ending the Subscriptions</param>
        /// <param name="ADateEnded">Date when the Subscriptions should end (can be empty)</param>
        /// <returns>ArrayList holding Publication Codes of the Subscriptions that were
        /// canceled
        /// </returns>
        public ArrayList CancelAllSubscriptions(String AReasonEnded, System.DateTime ADateEnded)
        {
            ArrayList ReturnValue;
            Boolean CountOnly;
            DataView CurrentSubsDV;
            PSubscriptionRow SubscriptionRow;
            Int16 RowCounter;

            ReturnValue = new ArrayList();

            // Determine whether we should only count the cancelable Subscriptions, or
            // actually cancel them (ADateEnded will be DateTime.MinValue when called
            // from CancelAllSubscriptionsCount).
            CountOnly = ADateEnded == DateTime.MinValue;

            // Loop over all nondeleted Subscriptions and check whether they should be
            // Canceled.
            CurrentSubsDV = new DataView(FMultiTableDS.PSubscription, "", "", DataViewRowState.CurrentRows);

            for (RowCounter = 0; RowCounter <= CurrentSubsDV.Count - 1; RowCounter += 1)
            {
                SubscriptionRow = (PSubscriptionRow)CurrentSubsDV[RowCounter].Row;

                if (SubscriptionRow.IsSubscriptionStatusNull() || (SubscriptionRow.SubscriptionStatus != StrSubscriptionValueCancelled)
                    && (SubscriptionRow.SubscriptionStatus != StrSubscriptionValueExpired))
                {
                    // this should not happen, but one never knows...

                    if (!CountOnly)
                    {
                        SubscriptionRow.BeginEdit();
                        SubscriptionRow.SubscriptionStatus = StrSubscriptionValueCancelled;
                        SubscriptionRow.DateCancelled = ADateEnded;
                        SubscriptionRow.ReasonSubsCancelledCode = AReasonEnded;
                        SubscriptionRow.GiftFromKey = 0;
                        SubscriptionRow.EndEdit();
                    }

                    ReturnValue.Add(SubscriptionRow.PublicationCode);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TUCPartnerSubscriptionsLogic() : base()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void CreateTempSubscriptionsTable()
        {
            FPartnerSubscriptionsGridTable = new DataTable("SubscriptionsList");
            FPartnerSubscriptionsGridTable.Columns.Add("Status", System.Type.GetType("System.String"));
            FPartnerSubscriptionsGridTable.Columns.Add("Publication", System.Type.GetType("System.String"));
            FPartnerSubscriptionsGridTable.Columns.Add("PublicationDescription", System.Type.GetType("System.String"));
            FPartnerSubscriptionsGridTable.Columns.Add("FreeOfCost", System.Type.GetType("System.Boolean"));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void FillTempSubscriptionsTable()
        {
            PSubscriptionTable PartnerSubsTable;
            DataRow TheNewRow;

            System.Int16 RowCounter;
            String PublicationDescription;
            Int32 PublicationDescriptionInCachePosition;

            // TODO 2 oChristianK cList Sorting : Sort both the subscribed and not subscribed List items so that invalid Publications are sorted to the end of both lists, respectively.
            PartnerSubsTable = FMultiTableDS.PSubscription;

            for (RowCounter = 0; RowCounter <= PartnerSubsTable.Rows.Count - 1; RowCounter += 1)
            {
                #region Determine Publication Description
                PublicationDescriptionInCachePosition =
                    FPublicationsDV.Find(PartnerSubsTable.Rows[RowCounter][PPublicationTable.GetPublicationCodeDBName()]);

                // MessageBox.Show('FillTempSubscriptionsTable');
                if (PublicationDescriptionInCachePosition != -1)
                {
                    PublicationDescription =
                        FPublicationsDV[PublicationDescriptionInCachePosition][PPublicationTable.GetPublicationDescriptionDBName()].ToString();

                    // If this Publication is inactive, show it.
                    if (!(Convert.ToBoolean(FPublicationsDV[PublicationDescriptionInCachePosition][PPublicationTable.GetValidPublicationDBName()])))
                    {
                        PublicationDescription = PublicationDescription + StrInactive;
                    }
                }
                else
                {
                    PublicationDescription = "";
                }

                #endregion
                TheNewRow = FPartnerSubscriptionsGridTable.NewRow();
                TheNewRow["Status"] = (object)PartnerSubsTable[RowCounter].SubscriptionStatus;
                TheNewRow["Publication"] = (object)PartnerSubsTable[RowCounter].PublicationCode;
                TheNewRow["PublicationDescription"] = (System.Object)PublicationDescription;
                TheNewRow["FreeOfCost"] = (object)PartnerSubsTable[RowCounter].GratisSubscription;
                FPartnerSubscriptionsGridTable.Rows.Add(TheNewRow);
            }

            // ######### Next code is not necessary ###########
            // ######### Shows the unsubscribed publications also ####
            // for PublicationRow in FDataCachePublicationListTable.Rows do
            // begin
            // if PartnerSubsTable.Rows.Find(
            // PublicationRow[PPublicationTable.GetPublicationCodeDBName()].ToString ) = nil then
            // begin
            // $REGION 'Determine Publication Description'
            // PublicationDescription := PublicationRow
            // [PPublicationTable.GetPublicationDescriptionDBName()].ToString();
            //
            // If this Publication is inactive, show it.
            // if not Convert.ToBoolean(PublicationRow
            // [PPublicationTable.GetValidPublicationDBName()]) then
            // begin
            // PublicationDescription := PublicationDescription + StrInactive;
            // end;
            // $ENDREGION
            //
            // TheNewRow := FPartnerSubscriptionsGridTable.NewRow();
            // TheNewRow['Status'] := '';
            // TheNewRow['Publication'] := PublicationRow
            // [PPublicationTable.GetPublicationCodeDBName()].ToString();
            // TheNewRow['PublicationDescription'] := PublicationDescription;
            // FPartnerSubscriptionsGridTable.Rows.Add(TheNewRow);
            // end;
            // end;
            // ############ end of unecessary code ###############
            // Get Publication Cost cacheable DataTable
            FPublicationCostDT = (PPublicationCostTable)TDataCache.TMPartner.GetCacheableSubscriptionsTable(
                TCacheableSubscriptionsTablesEnum.PublicationCostList);
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
        /// <param name="PublicationCode"></param>
        /// <returns></returns>
        public String GetDescriptionForPublication(String PublicationCode)
        {
            DataRowView[] Ar;
            Ar = this.FPublicationsDV.FindRows(PublicationCode);
            return Ar[0][PPublicationTable.GetPublicationDescriptionDBName()].ToString();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ExtractID"></param>
        /// <param name="ASubscriptionTable"></param>
        public void AddSubscriptions(Int32 ExtractID, PSubscriptionTable ASubscriptionTable)
        {
            ApplicationException SubmitException;

            try
            {
                if (FPartnerUIConnectorsExtracts == null)
                {
                    FPartnerUIConnectorsExtracts = TRemote.MPartner.Extracts.UIConnectors.ExtractsAddSubscriptions(ExtractID);

                    // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
                    TEnsureKeepAlive.Register(FPartnerUIConnectorsExtracts);
                }

                FPartnerUIConnectorsExtracts.SubmitChangesAsync(ASubscriptionTable);
            }
            catch (Exception Exp)
            {
                SubmitException = new ApplicationException(
                    "An error occured when OpenPetra tried adding Subscription '" + ASubscriptionTable[0].PublicationCode +
                    "' to Partners in Extract with ID '" + ExtractID.ToString() + "'!",
                    Exp);
                throw SubmitException;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APublicationCode"></param>
        /// <returns></returns>
        public DataRow GetSelectedDataRow(String APublicationCode)
        {
            return FMultiTableDS.PSubscription.Rows.Find(new Object[] { APublicationCode, FMultiTableDS.PPartner[0].PartnerKey });
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AlreadySubscribedPartnersDT"></param>
        /// <param name="AVerificationResult"></param>
        /// <param name="ASubmitChangesResult"></param>
        public void GetSubscriptionAddingResults(out DataTable AlreadySubscribedPartnersDT,
            out TVerificationResultCollection AVerificationResult,
            out TSubmitChangesResult ASubmitChangesResult)
        {
            FPartnerUIConnectorsExtracts.SubmitChangesAsyncResult(out AlreadySubscribedPartnersDT, out AVerificationResult, out ASubmitChangesResult);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public Boolean IsPublicationValid()
        {
            Boolean ReturnValue;
            PPublicationRow Row;

            Row = (PPublicationRow)FDataCachePublicationListTable.Rows.Find(new Object[] { FPublicationCode });

            if (Row.ValidPublication)
            {
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Loads Subscription Data from Petra Server into FMultiTableDS.
        ///
        /// </summary>
        /// <returns>true if successful, otherwise false.
        /// </returns>
        public Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            // Load Subscriptions, if not already loaded
            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMultiTableDS.PSubscription == null)
                {
                    FMultiTableDS.Tables.Add(new PSubscriptionTable());
                    FMultiTableDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading)
                {
                    FMultiTableDS.Merge(FPartnerEditUIConnector.GetDataSubscriptions());
                    FMultiTableDS.PSubscription.AcceptChanges();
                }

                if (FMultiTableDS.PSubscription.Rows.Count != 0)
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
        public void LoadPublications()
        {
            // Load Publications
            FDataCachePublicationListTable = TDataCache.TMPartner.GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum.PublicationList);

            // Specify the DataView that the SourceDataGrid will be bound to
            // FMultiTableDS.Tables['Subscriptions']
            FPublicationsDV = FDataCachePublicationListTable.DefaultView;
            FPublicationsDV.Sort = "p_publication_code_c";
        }

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
        /// <param name="ASubscriptionRow"></param>
        public void ProcessEditedRecord(out PSubscriptionRow ASubscriptionRow)
        {
            // this is the same always????
            if (FIsRecordBeingAdded)
            {
                ASubscriptionRow =
                    ((PSubscriptionRow)FMultiTableDS.PSubscription.Rows.Find(new Object[] { FPublicationCode, FMultiTableDS.PPartner[0].PartnerKey }));
            }
            else
            {
                ASubscriptionRow =
                    ((PSubscriptionRow)FMultiTableDS.PSubscription.Rows.Find(new Object[] { FPublicationCode, FMultiTableDS.PPartner[0].PartnerKey }));
            }

            // Call EndEdit on all DataRows that are editable in the Detail UserControl.
            // THIS IS ESSENTIAL!!! It makes the changes that the user has made permanent.
            if (ASubscriptionRow == null)
            {
                MessageBox.Show("TUCPartnerSubscriptionsLogic.ProcessEditedRecord:  ASubscriptionRow is nil" + FPublicationCode.ToString());
            }
            else
            {
                ASubscriptionRow.EndEdit();

                // New Subscription?
                if ((ASubscriptionRow.RowState == DataRowState.Added) && FIsRecordBeingAdded)
                {
                    FIsRecordBeingAdded = false;

                    if (FMultiTableDS.PPartner[0].StatusCode != SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE))
                    {
                        // Business Rule: if a new Subscription is added and the Partner's StatusCode
                        // isn't ACTIVE, set it to ACTIVE automatically.
                        // Note: The 4GL Petra screen didn't set the StatusCode to active when a
                        // new Subscription was added.
                        MessageBox.Show(String.Format(MCommonResourcestrings.StrPartnerStatusChange + StrPartnerReActivationBecauseOfNewSubscr,
                                FMultiTableDS.PPartner[0].StatusCode,
                                SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE)),
                            MCommonResourcestrings.StrPartnerReActivationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FMultiTableDS.PPartner[0].StatusCode = SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE);
                    }
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataView"></param>
        /// <param name="tmpPublicationCode"></param>
        /// <returns></returns>
        public DataRowView DetermineRecordToSelect(DataView ADataView, String tmpPublicationCode)
        {
            DataRowView ReturnValue = null;
            DataRowView TmpDataRowView;

            ADataView.RowFilter = "Publication" + " = '" + tmpPublicationCode + "'";
            TmpDataRowView = ADataView[0];
            ADataView.RowFilter = "";
            try
            {
                ReturnValue = TmpDataRowView;
            }
            catch (Exception)
            {
            }
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void Dispose()
        {
            if (FPartnerUIConnectorsExtracts != null)
            {
                // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
                TEnsureKeepAlive.UnRegister(FPartnerUIConnectorsExtracts);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ApublicationCode"></param>
        /// <returns></returns>
        public Boolean PublicationAlreadyExcist(String ApublicationCode)
        {
            Boolean ReturnValue;

            // MessageBox.Show('Pcode:' +APublicationcode);
            ReturnValue = true;

            foreach (DataRow Row in FPartnerSubscriptionsGridTable.Rows)
            {
                // MessageBox.Show('pub from grid: '+Row['Publication'].ToString);
                if (Row["Publication"].ToString() == ApublicationCode)
                {
                    // if the publication is already there, returns false
                    MessageBox.Show("A Subscription for that Publication already exists.\r\nPlease select another Publication, or press Cancel.");
                    ReturnValue = false;
                    break;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// function DeleteSubscriptionRow(AGrid: TSgrdDataGrid): Boolean;
        /// </summary>
        /// <returns>void</returns>
        public void RefreshDataGrid(TSgrdDataGrid AGrid, DataTable tmpDT, Boolean IsNew)
        {
            DataRow TheNewRow;
            PSubscriptionRow tmpSubscriptionRow;
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;
            Int32 PublicationDescriptionInCachePosition;
            Int32 counter;
            String PublicationDescription;
            StringCollection ACollection;
            StringCollection SortedCollection;

            ACollection = new StringCollection();
            SortedCollection = new StringCollection();

            // if not IsNew then
            // begin
            // MessageBox.Show(' is this ever entered?');
            // AGrid.SelectedDataRows[0].Delete();
            // end;
            tmpSubscriptionRow = (PSubscriptionRow)tmpDT.Rows[tmpDT.Rows.Count - 1];
            #region Determine Publication Description
            PublicationDescriptionInCachePosition = FPublicationsDV.Find(
                tmpDT.Rows[tmpDT.Rows.Count - 1][PPublicationTable.GetPublicationCodeDBName()]);

            // PublicationDescriptionInCachePosition := FSubscriptionsTableDV.Find(PartnerSubsTable.Rows[RowCounter][PPublicationTable.GetPublicationCodeDBName()]);
            if (PublicationDescriptionInCachePosition != -1)
            {
                PublicationDescription =
                    FPublicationsDV[PublicationDescriptionInCachePosition][PPublicationTable.GetPublicationDescriptionDBName()].ToString();
            }
            else
            {
                PublicationDescription = "";
            }

            #endregion
            tmpSubscriptionRow = (PSubscriptionRow)tmpDT.Rows[tmpDT.Rows.Count - 1];
            TheNewRow = FPartnerSubscriptionsGridTable.NewRow();
            TheNewRow["Status"] = (System.Object)tmpSubscriptionRow.SubscriptionStatus;
            TheNewRow["Publication"] = (System.Object)tmpSubscriptionRow.PublicationCode;
            TheNewRow["PublicationDescription"] = (System.Object)PublicationDescription;
            TheNewRow["FreeOfCost"] = (System.Object)tmpSubscriptionRow.GratisSubscription;
            FPartnerSubscriptionsGridTable.Rows.Add(TheNewRow);
            AGrid.DataSource = new DevAge.ComponentModel.BoundDataView(FPartnerSubscriptionsGridTable.DefaultView);

            // next 2 for loops are to get the right position on newly inserted row.
            // First all publication values are added to a StringCollection, which then is sorted,
            // And the Publication just added is compared to the sorted list, so that that the right row
            // in the DataGrid can be selected.
            counter = 0;

            foreach (DataRow tmpRow in FPartnerSubscriptionsGridTable.Rows)
            {
                ACollection.Add(tmpRow["Publication"].ToString());
            }

            SortedCollection = StringHelper.StrSort(ACollection);

            foreach (String APublicationString in SortedCollection)
            {
                counter = counter + 1;

                if (APublicationString == tmpSubscriptionRow.PublicationCode.ToString())
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
        /// <param name="AGrid"></param>
        /// <param name="pub"></param>
        /// <param name="SB"></param>
        /// <param name="isFreeSubscription"></param>
        public void RefreshDataGridEdit(TSgrdDataGrid AGrid, String pub, String SB, Boolean isFreeSubscription)
        {
            // Change the Status of edited Publication.
            foreach (DataRow Row in FPartnerSubscriptionsGridTable.Rows)
            {
                if (Row["publication"].ToString().ToUpper() == pub.ToUpper())
                {
                    Row["Status"] = SB;
                    Row["FreeOfCost"] = (System.Object)isFreeSubscription;
                    break;
                }
            }

            AGrid.DataSource = new DevAge.ComponentModel.BoundDataView(FPartnerSubscriptionsGridTable.DefaultView);
            AGrid.Refresh();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APublicationCost"></param>
        /// <param name="ACurrencyCode"></param>
        public void DeterminePublicationCost(out decimal APublicationCost, out String ACurrencyCode)
        {
            DataRow[] PublicationCostRows;
            PublicationCostRows = FPublicationCostDT.Select(PPublicationTable.GetPublicationCodeDBName() + " = '" + FPublicationCode + "'");

            if (PublicationCostRows.Length > 0)
            {
                APublicationCost = ((PPublicationCostRow)PublicationCostRows[0]).PublicationCost;
                ACurrencyCode = ((PPublicationCostRow)PublicationCostRows[0]).CurrencyCode;
            }
            else
            {
                APublicationCost = 0;
                ACurrencyCode = "";
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void EditRecord()
        {
            PSubscriptionRow Row;

            Row = ((PSubscriptionRow)FMultiTableDS.PSubscription.Rows.Find(new Object[] { FPublicationCode, FMultiTableDS.PPartner[0].PartnerKey }));
            Row.BeginEdit();
        }

        /// <summary>
        /// DataBinds the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DataBindGrid(TSgrdDataGrid AGrid)
        {
            FPartnerSubscriptionsGridTableDV = FPartnerSubscriptionsGridTable.DefaultView;
            FPartnerSubscriptionsGridTableDV.AllowNew = false;
            FPartnerSubscriptionsGridTableDV.AllowEdit = false;
            FPartnerSubscriptionsGridTableDV.AllowDelete = false;
            FPartnerSubscriptionsGridTableDV.Sort = "publication";

            // DataBind the DataGrid
            AGrid.DataSource = new DevAge.ComponentModel.BoundDataView(FPartnerSubscriptionsGridTableDV);
        }

        /// <summary>
        /// </summary>
        /// <returns>void</returns>
        public String DetermineCurrentPublicationCode(TSgrdDataGrid AGrid, Int32 ARow)
        {
            DataRowView TheDataRowView;

            // MessageBox.Show(ARow.ToString);
            TheDataRowView = (DataRowView)AGrid.Rows.IndexToDataSourceRow(ARow);
            try
            {
                FSubscriptionRow = TheDataRowView.Row;
                FPublicationCode = FSubscriptionRow["Publication"].ToString();
            }
            catch (NullReferenceException)
            {
            }

            // no subscriptions: to do?
            // MessageBox.Show('FPublicationCode of currently selected Grid Row: ' + FPublicationCode);
            return FPublicationCode;
        }

        // function TUCPartnerSubscriptionsLogic.DeleteSubscriptionRow( out AGrid: TSgrdDataGrid);
        // begin
        /// <summary>
        /// AGrid.SelectedDataRows[0].Delete(); */
        /// </summary>
        /// <returns>void</returns>
        public Boolean DeleteRecord(String PublicationCode, Int64 PPartnerKey)
        {
            Boolean ReturnValue;
            DataRow SubscriptionRow = null;
            DialogResult Chosen;
            String DeleteQuestion;

            try
            {
                SubscriptionRow = FMultiTableDS.PSubscription.Rows.Find(new Object[] { PublicationCode, FMultiTableDS.PPartner[0].PartnerKey });
            }
            catch (Exception)
            {
            }
            DeleteQuestion = StrDeleteQuestionLine;
            Chosen = MessageBox.Show(DeleteQuestion,
                StrDeleteQuestionTitle,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (Chosen == DialogResult.Yes)
            {
                SubscriptionRow.Delete();
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public Int64 DetermineCurrentPartnerKey()
        {
            Int64 ReturnValue = -1;

            try
            {
                ReturnValue = FMultiTableDS.PPartner[0].PartnerKey;
            }
            catch (Exception)
            {
                MessageBox.Show("Exception at DetermineCurrentPartnerKey()");
            }
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <returns></returns>
        public String DetermineCurrentPublicationCode(TSgrdDataGrid AGrid)
        {
            DataRowView[] TheDataRowViewArray;
            TheDataRowViewArray = AGrid.SelectedDataRowsAsDataRowView;
            FPublicationCode = TheDataRowViewArray[0].Row["Publication"].ToString();
            return FPublicationCode;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <param name="ARowNumber"></param>
        /// <param name="APublicationCode"></param>
        public void DetermineInitiallySelectedPublication(TSgrdDataGrid AGrid, out Int32 ARowNumber, out String APublicationCode)
        {
            APublicationCode = "";
            ARowNumber = -1;
            DataRow SubscriptionRow;
            ARowNumber = 1;
            try
            {
                SubscriptionRow = ((DataRowView)AGrid.Rows.IndexToDataSourceRow(1)).Row;
                APublicationCode = SubscriptionRow["Publication"].ToString();
            }
            catch (NullReferenceException)
            {
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="Partnerkey"></param>
        /// <param name="PublicationCode"></param>
        public void DetermineNewPrimaryKeys(out Int64 Partnerkey, out String PublicationCode)
        {
            DataRow tmpRow;

            PublicationCode = "";
            Partnerkey = FMultiTableDS.PPartner[0].PartnerKey;

            foreach (PPublicationRow ARow in FPublicationsDV.Table.Rows)
            {
                // Find the first Publication, that the Partner is not yet subscribing.
                tmpRow = FMultiTableDS.PSubscription.Rows.Find(new Object[] { ARow.PublicationCode, Partnerkey });

                if (tmpRow == null)
                {
                    PublicationCode = ARow.PublicationCode;
                    break;
                }
                else
                {
                    // MessageBox.Show('TUCPartnerSubscriptionsLogic.DetermineNewPrimaryKeys: key determined');
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        public void CreateColumns(TSgrdDataGrid AGrid)
        {
            // Publication Code
            AGrid.AddTextColumn("Publication Code", FPartnerSubscriptionsGridTable.Columns["Publication"]);

            // Publication Description
            AGrid.AddTextColumn("Description", FPartnerSubscriptionsGridTable.Columns["PublicationDescription"]);

            // Subscription Status
            AGrid.AddTextColumn("Subscription Status", FPartnerSubscriptionsGridTable.Columns["Status"]);

            // Free Subscription
            AGrid.AddCheckBoxColumn("Free Subscription", FPartnerSubscriptionsGridTable.Columns["FreeOfCost"]);
            AGrid.Columns[3].DataCell.Editor = null;

            // Agrid.Columns[3].DataCell.Editor.EnableEdit := false;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="Partnerkey"></param>
        /// <param name="PublicationCode"></param>
        public void CreateNewSubscriptionRow(Int64 Partnerkey, String PublicationCode)
        {
            PSubscriptionRow newPSubscriptionRow;

            FIsRecordBeingAdded = true;

            /*
             * Add new Locations row
             */
            newPSubscriptionRow = FMultiTableDS.PSubscription.NewRowTyped(true);

            // Assign Primary Key columns
            newPSubscriptionRow.PartnerKey = Partnerkey;
            newPSubscriptionRow.PublicationCode = PublicationCode;
            FMultiTableDS.PSubscription.Rows.Add(newPSubscriptionRow);
            FLastRowAdded = newPSubscriptionRow;

            // MessageBox.Show('TUCPartnerSubscriptionsLogic.CreateNewSubscriptionRow: new row added to FMultiTableDS');
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RemoveLastRowAdded()
        {
            if (FLastRowAdded != null)
            {
                FMultiTableDS.PSubscription.Rows.Remove(FLastRowAdded);
            }
        }

        #endregion
    }
}