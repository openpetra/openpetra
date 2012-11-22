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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.DataGrid;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonForms;
using System.Threading;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.Interfaces.MPartner;

namespace Ict.Petra.Client.MCommon.Gui
{
    /// <summary>
    /// UserControl for editing Partner Addresses (List/Detail view).
    ///
    /// Consists of a Grid that contains a list of records, three buttons that allow
    /// manipulation of the records and a Detail UserControl to show and edit the
    /// details of the selected record.
    ///
    /// </summary>
    public partial class TUCPartnerAddresses : System.Windows.Forms.UserControl, IPetraEditUserControl
    {
        #region Resourcestrings

        private static readonly string StrSimilarLocation1stLine = Catalog.GetString("A similar address already exists in the database:");

        private static readonly string StrSimilarLocationUseQuestionPart1 = Catalog.GetString("Use the existing address record?");

        private static readonly string StrSimilarLocationUseQuestionPart2 = Catalog.GetString("(Choose 'No' to create a new address record.)");

        private static readonly string StrSimilarLocationTitle = Catalog.GetString("Similar Address Exists");

        private static readonly string StrSimilarLocationUsedByN1 = Catalog.GetString("NOTE: this is used by {0} ");

        private static readonly string StrAddressAddedFamilyPromotion1stLine = Catalog.GetString(
            "You have added the following address to this family:");

        private static readonly string StrAddressAddedFamilyPromotionQuestion = Catalog.GetString(
            "Do you want to add this address to all members\r\nof this family?");

        private static readonly string StrAddressAddedFamilyPromotionQuestionTitle = Catalog.GetString("Add Address to Family Members?");

        private static readonly string StrExpireAllCurrentAddressesTitle = Catalog.GetString("Expire All Current Addresses");

        private static readonly string StrExpireAllCurrentAddressesNone = Catalog.GetString(
            "There are no Current Addresses, therefore none need to be expired.");

        private static readonly string StrExpireAllCurrentAddressesNone2 = Catalog.GetString("There are no Current Addresses that can be expired.");

        private static readonly string StrExpireAllCurrentAddressesDoneTitle = Catalog.GetString("All Addresses Expired");

        private static readonly string StrExpireAllCurrentAddressesDone = Catalog.GetString(
            "The following {0} Address(es) was/were expired:\r\n{1}\r\n" +
            "The Partner has no Current Addresses left.");

        #endregion

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        protected IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary>Object that holds the logic for this screen</summary>
        protected TUCPartnerAddressesLogic FLogic;

        /// <summary>holds the DataSet that contains most data that is used on the screen</summary>
        protected PartnerEditTDS FMainDS;

        /// <summary>DataView that the SourceDataGrid is bound to</summary>
        protected DataView FDataGridDV;

        /// <summary>tells whether the currently selected record is beeing edited, or not</summary>
        protected Boolean FIsEditingRecord;

        /// <summary>tells whether the Grid has been expanded vertically with btnMaximiseMinimiseGrid</summary>
        protected Boolean FIsGridMaximised;

        /// <summary>tells whether the 'New' button got initially disabled (for an autocreated Address), or not</summary>
        protected Boolean FDisabledNewButtonOnAutoCreatedAddress;

        /// <summary>holds the height of the Grid before it got expanded vertically with btnMaximiseMinimiseGrid</summary>
        protected Int32 FGridMinimisedSize;

        /// <summary>holds a LocationKey if an Address that was just found using 'Menu Edit>Find Address...' should be added as a new Address, otherwise 0</summary>
        protected Int32 FFoundNewAddressLocationKey;

        /// <summary>holds the LocationKey of the Address record that was last selected in the Grid</summary>
        protected Int32 FLastKey;

        /// <summary>DataSet that contains most data that is used on the screen</summary>
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

        /// <summary>Record that is currently selected (p_location DataTable)</summary>
        public PLocationRow LocationDataRowOfCurrentlySelectedRecord
        {
            get
            {
                return FLogic.LocationDataRowOfCurrentlySelectedRecord;
            }
        }

        /// <summary>Record that is currently selected (p_partner_location DataTable)</summary>
        public PartnerEditTDSPPartnerLocationRow PartnerLocationDataRowOfCurrentlySelectedRecord
        {
            get
            {
                return FLogic.PartnerLocationDataRowOfCurrentlySelectedRecord;
            }
        }

        /// <summary>true if a record is beeing added, otherwise false</summary>
        public Boolean IsRecordBeingAdded
        {
            get
            {
                return FLogic.IsRecordBeingAdded;
            }
        }

        /// <summary>Used for passing through the Clientside Proxy for the UIConnector</summary>
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

        /// <summary>Custom Event for recalculation of the Tab Header</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>Custom Event for hooking up data change events</summary>
        public event THookupDataChangeEventHandler HookupDataChange;

        #region TUCPartnerAddresses

        /// <summary>
        /// Default Constructor.
        ///
        /// Define the screen's logic.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TUCPartnerAddresses() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnDeleteRecord.Text = Catalog.GetString("      &Delete");
            this.btnNewRecord.Text = Catalog.GetString("       &New");
            #endregion

            // define the screen's logic
            FLogic = new TUCPartnerAddressesLogic();
        }

        /// <summary>
        /// Required for OpePetra UserControls.
        /// </summary>
        public void InitUserControl()
        {
        }

        private TFrmPetraEditUtils FPetraUtilsObject;

        /// <summary>
        /// this provides general functionality for edit screens
        /// </summary>
        public TFrmPetraEditUtils PetraUtilsObject
        {
            get
            {
                return FPetraUtilsObject;
            }
            set
            {
                FPetraUtilsObject = value;
                FPetraUtilsObject.SetStatusBarText(this.btnMaximiseMinimiseGrid, Catalog.GetString("List expand button"));
                FPetraUtilsObject.SetStatusBarText(this.grdRecordList, Catalog.GetString("Address list"));
                FPetraUtilsObject.SetStatusBarText(this.btnDeleteRecord, Catalog.GetString("Delete currently selected Address"));
                FPetraUtilsObject.SetStatusBarText(this.btnNewRecord, Catalog.GetString("Create new address"));
            }
        }

        /// <summary>
        /// Determines and selects the 'Best Address' in the Grid.
        /// Optionally it can DataBind the Details UserControl.
        ///
        /// </summary>
        /// <param name="APerformDataBinding">Set this to true if the Details UserControl should
        /// be databound, otherwise set to false.
        /// </param>
        /// <returns>void</returns>
        private void SelectBestAddressRow(Boolean APerformDataBinding)
        {
            Int32 BestAddressRowNumber;
            Int32 BestAddressLocationKey;
            Int64 BestAddressSiteKey;

            // Determine the row that contains 'Best Address' > this row should be initially selected
            FLogic.DetermineInitiallySelectedRecord(
                out BestAddressRowNumber,
                out BestAddressLocationKey,
                out BestAddressSiteKey);
            FLogic.LocationKey = BestAddressLocationKey;

            // MessageBox.Show('BestAddressRowNumber: ' + BestAddressRowNumber.ToString +
            // #13#10'BestAddressLocationKey: ' + BestAddressLocationKey.ToString);
            // Select row that contains 'Best Address'
            grdRecordList.Selection.ResetSelection(false);
            grdRecordList.Selection.SelectRow(BestAddressRowNumber, true);

            // Scroll grid to line where the 'Best Address' is displayed
            grdRecordList.ShowCell(BestAddressRowNumber);

            if (APerformDataBinding)
            {
                // DataBind Detail UserControl
                ucoDetails.PerformDataBinding(FMainDS, BestAddressLocationKey);
            }

            // Determine current Location Key and enable/disable buttons,
            // update Detail UserControl
            DataGrid_FocusRowEntered(this, new RowEventArgs(BestAddressRowNumber));
        }

        /// <summary>
        /// Disables the 'New' button.
        ///
        /// This would be called when an Address was automatically added. In this
        /// situation it is the only Address and the user should be guided to edit this
        /// Address and to not create another new Address.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DisableNewButtonOnAutoCreatedAddress()
        {
            btnNewRecord.Enabled = false;
            FDisabledNewButtonOnAutoCreatedAddress = true;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ACancelDate"></param>
        public void ExpireAllCurrentAddresses(DateTime ACancelDate)
        {
            int TotalAddresses;
            int CurrentAddresses;
            ArrayList AddrExpired;
            int UpdateCounter;
            String AddrExpiredString;
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            AddrExpiredString = "";

            // Check whether there are any (Current) Addresses that can be expired
            Calculations.CalculateTabCountsAddresses(FMainDS.PPartnerLocation, out TotalAddresses, out CurrentAddresses);

            if (CurrentAddresses > 0)
            {
                // Expire all Current Addresses
                AddrExpired = FLogic.ExpireAllCurrentAddresses(ACancelDate);

                /*
                 * Build a String to tell the user what Addresses were expired.
                 */
                for (UpdateCounter = 0; UpdateCounter <= AddrExpired.Count - 1; UpdateCounter += 1)
                {
                    AddrExpiredString = AddrExpiredString + "   " + AddrExpired[UpdateCounter].ToString() + Environment.NewLine;
                }

                if (AddrExpiredString != "")
                {
                    // Now refresh the Grid with the changed data
                    RefreshRecordsAfterMerge();
                    grdRecordList.Refresh();

                    // Finally, select the first record in the Grid and update the Detail
                    // UserControl (this one might have been Expired)
                    grdRecordList.Selection.ResetSelection(false);
                    grdRecordList.Selection.SelectRow(1, true);

                    // Scroll grid to line where the selected record is displayed
                    grdRecordList.ShowCell(new Position(1, 0), true);
                    DataGrid_FocusRowEntered(this, new RowEventArgs(1));

                    // Make sure the Detail UserControl is updated, even if the Row wasn't
                    // changed because there was only one row...
                    ucoDetails.SetMode(TDataModeEnum.dmBrowse);

                    // Fire OnRecalculateScreenParts event to update the Tab Counters
                    RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
                    RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
                    OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);

                    // Tell the user that Expriring of Addresses was succesful
                    MessageBox.Show(String.Format(StrExpireAllCurrentAddressesDone,
                            AddrExpired.Count,
                            AddrExpiredString), StrExpireAllCurrentAddressesDoneTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Tell the user that there are no (Current) Addresses that can be expired.
                    MessageBox.Show(StrExpireAllCurrentAddressesNone2,
                        StrExpireAllCurrentAddressesTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else
            {
                // Tell the user that there are no (Current) Addresses that can be expired.
                MessageBox.Show(StrExpireAllCurrentAddressesNone, StrExpireAllCurrentAddressesTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Iterates through all PPartnerLocation DataRows and checks whether any has a
        /// SiteKey and LocationKey with the Original values that we are looking for to
        /// be able to return the corresponding PLocation DataRow.
        ///
        /// </summary>
        /// <param name="ALocationPK">Primary Key that the searched for PLocation DataRow should
        /// have had orignally</param>
        /// <returns>PLocation DataRow if found, otherwise nil
        /// </returns>
        protected PLocationRow FindLocationRowWithOriginalKey(TLocationPK ALocationPK)
        {
            PLocationRow ReturnValue;

            ReturnValue = null;

            /*
             * Iterate through all PPartnerLocation DataRows and check whether any has a PK with
             * the Original values that we are looking for.
             */
            foreach (PPartnerLocationRow PartnerLocationRow in FMainDS.PPartnerLocation.Rows)
            {
                // MessageBox.Show('FindLocationRowWithOriginalKey: SiteKey: ' + Convert.ToInt64(PartnerLocationRow[PPartnerLocationTable.GetSiteKeyDBName(),
                // DataRowVersion.Original]).ToString + '; LocationKey: ' +
                // Convert.ToInt32(PartnerLocationRow[PPartnerLocationTable.GetLocationKeyDBName(),
                // DataRowVersion.Original]).ToString);
                if ((Convert.ToInt64(PartnerLocationRow[PLocationTable.GetSiteKeyDBName(),
                                                        DataRowVersion.Original]) == ALocationPK.SiteKey)
                    && (Convert.ToInt32(PartnerLocationRow[PLocationTable.GetLocationKeyDBName(),
                                                           DataRowVersion.Original]) == ALocationPK.LocationKey))
                {
                    // Find the PLocationRow that has the same SiteKey and LocationKey than the
                    // found PPartnerLocationRow and return it!
                    ReturnValue = (PLocationRow)FMainDS.PLocation.Rows.Find(new Object[] { PartnerLocationRow.SiteKey, PartnerLocationRow.LocationKey });
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Raises Event HookupDataChange.
        ///
        /// </summary>
        /// <param name="e">Event parameters (nothing in particluar for this Event)
        /// </param>
        /// <returns>void</returns>
        protected void OnHookupDataChange(System.EventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        /// <summary>
        /// Raises Event RecalculateScreenParts.
        ///
        /// </summary>
        /// <param name="e">Event parameters
        /// </param>
        /// <returns>void</returns>
        protected void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        #region Public Methods

        /// <summary>
        /// Sets up the screen logic, retrieves data, databinds the Grid and the Detail
        /// UserControl.
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseUserControl()
        {
            DataColumn ForeignTableColumn;
            DataTable LocationTable;

            // Set up screen logic
            FLogic.MainDS = FMainDS;
            FLogic.DataGrid = grdRecordList;
            FLogic.GridRowIconsImageList = imlRecordIcons;
            FLogic.PartnerEditUIConnector = FPartnerEditUIConnector;
            FLogic.LoadDataOnDemand();

            // FLastKey: just make it something that is not a LocationKey that will be in the screen...
            FLastKey = -1000;
            LocationTable = FMainDS.PLocation;

            // Create DataColumns that contain data from the PartnerLocations DataTable
            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.DateTime");
            ForeignTableColumn.ColumnName = "Parent_" + PPartnerLocationTable.GetDateEffectiveDBName();
            ForeignTableColumn.Expression = "Parent." + PPartnerLocationTable.GetDateEffectiveDBName();
            LocationTable.Columns.Add(ForeignTableColumn);
            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.String");
            ForeignTableColumn.ColumnName = "Parent_" + PPartnerLocationTable.GetLocationTypeDBName();
            ForeignTableColumn.Expression = "Parent." + PPartnerLocationTable.GetLocationTypeDBName();
            LocationTable.Columns.Add(ForeignTableColumn);
            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.String");
            ForeignTableColumn.ColumnName = "Parent_BestAddress";
            ForeignTableColumn.Expression = "Parent.BestAddress";
            LocationTable.Columns.Add(ForeignTableColumn);
            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.String");
            ForeignTableColumn.ColumnName = "Parent_" + PPartnerLocationTable.GetLocationKeyDBName();
            ForeignTableColumn.Expression = "Parent." + PPartnerLocationTable.GetLocationKeyDBName();
            LocationTable.Columns.Add(ForeignTableColumn);

            // The following column allows sorting of the addresses by date within the Icons
            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.Int16");
            ForeignTableColumn.ColumnName = "Parent_Icon";

            // exchanges instead of Icon=1 we get Icon=2
            ForeignTableColumn.Expression = "IIF(Parent.Icon = 1, 2, IIF(Parent.Icon = 2, 1, 3))";
            LocationTable.Columns.Add(ForeignTableColumn);

            // Specify the DataView that the SourceDataGrid will be bound to
            FDataGridDV = LocationTable.DefaultView;
            FDataGridDV.Sort = "Parent_Icon ASC, Parent_" + PPartnerLocationTable.GetDateEffectiveDBName() + " DESC";
            FDataGridDV.AllowDelete = true;
            FDataGridDV.AllowNew = false;
            grdRecordList.ToolTipTextDelegate = @FLogic.GetToolTipTextForGridRow;

            // Initialise dependent UserControl
            ucoDetails.PetraUtilsObject = FPetraUtilsObject;
            ucoDetails.InitUserControl();
            ucoDetails.DisabledControlClickHandler = @HandleDisabledControlClick;

            // Create SourceDataGrid columns
            FLogic.CreateColumns((PLocationTable)FDataGridDV.Table);

            // DataBindingrelated stuff
            SetupDataGridDataBinding();

            // Setup the DataGrid's visual appearance
            SetupDataGridVisualAppearance();

            ApplySecurity();
            OnHookupDataChange(new System.EventArgs());

            // initial state of buttons. show edit and delete button
            btnDeleteRecord.Text = "     " + MCommonResourcestrings.StrBtnTextDelete;
            btnDeleteRecord.ImageIndex = 2;
        }

        /// <summary>
        /// Checks whether there any Tips to show to the User; if there are, they will be
        /// shown.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CheckForUserTips()
        {
            Rectangle SelectedTabHeaderRectangle;

            // Calculate where the middle of the TabHeader of the Tab lies at this moment
            SelectedTabHeaderRectangle = ((TTabVersatile) this.Parent.Parent.Parent).SelectedTabHeaderRectangle;
            pnlBalloonTipAnchor.Left = SelectedTabHeaderRectangle.X + Convert.ToInt16(SelectedTabHeaderRectangle.Width / 2.0);

            // Check for BalloonTips to display, and show them
            System.Threading.ThreadPool.QueueUserWorkItem(@TThreaded.ThreadedCheckForUserTips, pnlBalloonTipAnchor);
        }

        /// <summary>
        /// Performs necessary actions to make the Merging of rows that were changed on
        /// the Server side into the Client-side DataSet possible.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CleanupRecordsBeforeMerge()
        {
            // Do the following only if the UserControl was properly initialised
            if (FMainDS != null)
            {
                FLogic.CleanupRecordsBeforeMerge();
            }
        }

        /// <summary>
        /// Allows copying of an Address that the user has found (using Menu 'Edit'->'Find
        /// New Address...') into the currently edited Address.
        ///
        /// </summary>
        /// <param name="AFoundAddressLocationRow">DataRow containing the Location information
        /// for the found Address.
        /// </param>
        /// <returns>void</returns>
        public void CopyFoundAddressData(PLocationRow AFoundAddressLocationRow)
        {
            // Make sure that DataBinding writes the value of any active Control in
            // ucoDetails that is DataBound to PPartnerLocation to the underlying
            // DataSource (for taking over current PartnerLocation changes made by the
            // user!
            ucoDetails.SetFocusToFirstField();
            FLogic.CopyFoundAddressData(AFoundAddressLocationRow);
            ucoDetails.Key = (Int32)AFoundAddressLocationRow.LocationKey;
            ucoDetails.RefreshDataBoundFields();
        }

        /// <summary>
        /// Performs necessary actions after the Merging of rows that were changed on
        /// the Server side into the Client-side DataSet.
        ///
        /// Determination of the Grid icons and the 'Best Address' (this changes certain
        /// DataColumns in some DataRows!)
        ///
        /// </summary>
        /// <returns>void</returns>
        public void RefreshRecordsAfterMerge()
        {
            // Do the following only if the UserControl was properly initialised
            if (FMainDS != null)
            {
                FLogic.RefreshRecordsAfterMerge();
                SelectBestAddressRow(false);
            }
        }

        /// <summary>
        /// Called if the PetraServer responds on added or edited Location data that was
        /// submitted in a call to the UIConnector's SubmitChanges function.
        ///
        /// The PetraServer found out that for one or more Locations which were
        /// added or edited similar Location(s) already exist in the DB. The user needs
        /// to choose what to do with the changed Location(s).
        /// The user's answer is stored in the AParameterDT DataTable for each Address.
        /// Eventually this DataTable is sent back to the PetraServer for further
        /// evaluation and action.
        ///
        /// </summary>
        /// <param name="AParameterDT">DataTable containing parameter data that needs to be
        /// processed
        /// </param>
        /// <returns>void</returns>
        public void ProcessServerResponseSimilarLocations(PartnerAddressAggregateTDSSimilarLocationParametersTable AParameterDT)
        {
            PartnerAddressAggregateTDSSimilarLocationParametersRow SimilarLocationParametersRow;
            String AlreadyUsedMessage;
            int Counter;

            System.Windows.Forms.DialogResult SimilarLocationDialogResult;

            for (Counter = 0; Counter <= AParameterDT.Rows.Count - 1; Counter += 1)
            {
                SimilarLocationParametersRow = (PartnerAddressAggregateTDSSimilarLocationParametersRow)AParameterDT.Rows[Counter];

                /* MessageBox.Show('ProcessServerResponseSimilarLocations:  SimilarLocationParametersRow.SiteKey: ' + SimilarLocationParametersRow.SiteKey.ToString + '; SimilarLocationParametersRow.LocationKey: ' +
                 *SimilarLocationParametersRow.LocationKey.ToString); */
                if (!SimilarLocationParametersRow.AnswerProcessedClientSide)
                {
                    if (SimilarLocationParametersRow.UsedByNOtherPartners > 0)
                    {
                        AlreadyUsedMessage = "\r\n\r\n" +
                                             String.Format(StrSimilarLocationUsedByN1, SimilarLocationParametersRow.UsedByNOtherPartners) +
                                             Catalog.GetPluralString(" other partner.",
                            " other partners.",
                            SimilarLocationParametersRow.UsedByNOtherPartners);
                    }
                    else
                    {
                        AlreadyUsedMessage = "";
                    }

                    SimilarLocationDialogResult = MessageBox.Show(
                        StrSimilarLocation1stLine + "\r\n" + "    " + SimilarLocationParametersRow.Locality + "\r\n" + "    " +
                        SimilarLocationParametersRow.StreetName + "\r\n" + "    " + SimilarLocationParametersRow.Address3 + "\r\n" + "    " +
                        SimilarLocationParametersRow.City + ' ' + SimilarLocationParametersRow.PostalCode + "\r\n" + "    " +
                        SimilarLocationParametersRow.County + ' ' + SimilarLocationParametersRow.CountryCode + "\r\n" + AlreadyUsedMessage + "\r\n" +
                        "\r\n" + StrSimilarLocationUseQuestionPart1 + "\r\n" + StrSimilarLocationUseQuestionPart2,
                        StrSimilarLocationTitle,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (SimilarLocationDialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        SimilarLocationParametersRow.AnswerReuse = true;
                    }
                    else
                    {
                        SimilarLocationParametersRow.AnswerReuse = false;
                    }

                    FLogic.AddCleanupAddressesLocationKey((Int32)SimilarLocationParametersRow.LocationKey);
                }
            }
        }

        /// <summary>
        /// Called if the PetraServer responds on added Address(es) that was/were
        /// submitted in a call to the UIConnector's SubmitChanges function.
        ///
        /// The PetraServer found out that the Address(es) is/are added to a Partner of
        /// Partner Class FAMILY and that this FAMILY has got Family Members. The user
        /// needs to decide whether the Address(es) should be added to all members of
        /// the family.
        /// The user's answer is stored in the AAddedOrChangedPromotionDT DataTable for
        /// each Address. Eventually this DataTable is sent back to the PetraServer for
        /// further evaluation and action.
        ///
        /// </summary>
        /// <param name="AAddedOrChangedPromotionDT">DataTable containing parameter data that
        /// needs to be processed</param>
        /// <param name="AParameterDT">DataTable containing detail data for each record in
        /// AAddedOrChangedPromotionDT
        /// </param>
        /// <returns>void</returns>
        public void ProcessServerResponseAddressAddedOrChanged(
            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddedOrChangedPromotionDT,
            PartnerAddressAggregateTDSChangePromotionParametersTable AParameterDT)
        {
            PLocationRow LocationRow;
            PLocationTable LocationDT;

            System.Windows.Forms.DialogResult AddressAddedPromotionDialogResult;
            int Counter;
            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow AddressAddedOrChangedPromotionRow;

#if TODO
            string FilterCriteria;
            DataView PersonsLocationsDV;
            DataView PartnerSharingLocationDV;
            TPartnerAddressChangePropagationSelectionWinForm AddressChangedDialog;
            string UserAnswer;
            TPartnerLocationChangePropagationSelectionWinForm LocationChangedDialog;
#endif

            for (Counter = 0; Counter <= AAddedOrChangedPromotionDT.Rows.Count - 1; Counter += 1)
            {
                AddressAddedOrChangedPromotionRow =
                    (PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow)AAddedOrChangedPromotionDT.Rows[Counter];

                if (!AddressAddedOrChangedPromotionRow.AnswerProcessedClientSide)
                {
                    if (AddressAddedOrChangedPromotionRow.LocationAdded)
                    {
                        LocationRow = (PLocationRow)FMainDS.PLocation.Rows.Find(new Object[] { AddressAddedOrChangedPromotionRow.SiteKey,
                                                                                               AddressAddedOrChangedPromotionRow.LocationKey });

                        if (LocationRow != null)
                        {
                            LocationDT = (PLocationTable)LocationRow.Table;
                            AddressAddedPromotionDialogResult = MessageBox.Show(
                                StrAddressAddedFamilyPromotion1stLine + "\r\n" + "    " +
                                TSaveConvert.StringColumnToString(LocationDT.ColumnLocality,
                                    LocationRow) + "\r\n" + "    " + TSaveConvert.StringColumnToString(LocationDT.ColumnStreetName,
                                    LocationRow) + "\r\n" + "    " + TSaveConvert.StringColumnToString(LocationDT.ColumnAddress3,
                                    LocationRow) + "\r\n" + "    " + TSaveConvert.StringColumnToString(LocationDT.ColumnCity,
                                    LocationRow) + ' ' + TSaveConvert.StringColumnToString(LocationDT.ColumnPostalCode,
                                    LocationRow) + "\r\n" + "    " + TSaveConvert.StringColumnToString(LocationDT.ColumnCounty,
                                    LocationRow) + ' ' + TSaveConvert.StringColumnToString(LocationDT.ColumnCountryCode,
                                    LocationRow) + "\r\n" + "\r\n" + StrAddressAddedFamilyPromotionQuestion,
                                StrAddressAddedFamilyPromotionQuestionTitle,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (AddressAddedPromotionDialogResult == System.Windows.Forms.DialogResult.Yes)
                            {
                                AddressAddedOrChangedPromotionRow.UserAnswer = "YES";
                            }
                            else
                            {
                                AddressAddedOrChangedPromotionRow.UserAnswer = "NO";
                            }
                        }
                        else
                        {
                            MessageBox.Show(
                                "Error in " + this.GetType().FullName + ".AddressAddedOrChangedProcessing (LocationAdded): " +
                                "Location with SiteKey " +
                                AddressAddedOrChangedPromotionRow.SiteKey.ToString() + " and LocationKey " +
                                AddressAddedOrChangedPromotionRow.LocationKey.ToString() + " could not be found on the Client side!");
                            AddressAddedOrChangedPromotionRow.UserAnswer = "CANCEL";
                        }
                    }
                    else if (AddressAddedOrChangedPromotionRow.LocationChange)
                    {
                        if (AParameterDT != null)
                        {
#if TODO
                            FilterCriteria = PartnerAddressAggregateTDSChangePromotionParametersTable.GetSiteKeyDBName() + " = " +
                                             AddressAddedOrChangedPromotionRow.SiteKey.ToString() + " AND " +
                                             PartnerAddressAggregateTDSChangePromotionParametersTable.GetLocationKeyDBName() + " = " +
                                             AddressAddedOrChangedPromotionRow.LocationKey.ToString();

                            // MessageBox.Show('FilterCriteria: ' + FilterCriteria);
#endif
                            LocationRow = (PLocationRow)FMainDS.PLocation.Rows.Find(new Object[] { AddressAddedOrChangedPromotionRow.SiteKey,
                                                                                                   AddressAddedOrChangedPromotionRow.LocationKey });

                            if (LocationRow != null)
                            {
#if TODO
                                PartnerSharingLocationDV = new DataView(AParameterDT,
                                    FilterCriteria,
                                    PartnerAddressAggregateTDSChangePromotionParametersTable.GetPartnerKeyDBName() + " ASC",
                                    DataViewRowState.CurrentRows);

                                AddressChangedDialog = new TPartnerAddressChangePropagationSelectionWinForm();
                                AddressChangedDialog.SetParameters(AddressAddedOrChangedPromotionRow, PartnerSharingLocationDV, LocationRow, "", "");

                                if (AddressChangedDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                                {
                                    AddressAddedOrChangedPromotionRow.UserAnswer = "CANCEL";

                                    // get AddressChangedDialog out of memory
                                    AddressChangedDialog.Dispose();

                                    // MessageBox.Show('AddressChangedDialog: pressed Cancel.');
                                }
                                else
                                {
                                    if (AddressChangedDialog.GetReturnedParameters(out UserAnswer))
                                    {
                                        AddressAddedOrChangedPromotionRow.UserAnswer = UserAnswer;
#endif
                                AddressAddedOrChangedPromotionRow.UserAnswer = "CHANGE-NONE";          // TODO Remove this assignment once the code lines immediately above are no longer in compiler directive '#if Todo'!

                                if (AddressAddedOrChangedPromotionRow.UserAnswer.StartsWith("CHANGE"))
                                {
                                    /*
                                     * The LocationRow gets deleted from the LocationTable on the
                                     * Server side, but there a AcceptChanges is done so that the
                                     * DataRow doesn't actually get deleted from the DB. The Client
                                     * would then no longer know that it needs to delete it, so we
                                     * need do remember to do it later!
                                     */
                                    FLogic.AddCleanupAddressesLocationKey((Int32)AddressAddedOrChangedPromotionRow.LocationKey);
#if TODO
                                }
                            }
                            else
                            {
                                throw new System.Exception(
                                    "GetReturnedParameters called, but Form '" + AddressChangedDialog.Name +
                                    "' is not finished yet with initialisation");
                            }

                            // get NewPartnerDialog out of memory
                            AddressChangedDialog.Dispose();
#endif
                                }
                            }
                            else
                            {
                                MessageBox.Show(
                                    "Error in " + this.GetType().FullName + ".AddressAddedOrChangedProcessing (LocationChange): " +
                                    "Location with SiteKey " + AddressAddedOrChangedPromotionRow.SiteKey.ToString() + " and LocationKey " +
                                    AddressAddedOrChangedPromotionRow.LocationKey.ToString() + " could not be found on the Client side!");
                                AddressAddedOrChangedPromotionRow.UserAnswer = "CANCEL";
                            }
                        }
                        else
                        {
                            throw new System.ArgumentException("AParameterDT must not be nil when LocationChange = true");
                        }
                    }
                    else if (AddressAddedOrChangedPromotionRow.PartnerLocationChange)
                    {
                        if (AParameterDT != null)
                        {
#if TODO
                            FilterCriteria = PartnerAddressAggregateTDSChangePromotionParametersTable.GetSiteKeyOfEditedRecordDBName() + " = " +
                                             AddressAddedOrChangedPromotionRow.SiteKey.ToString() + " AND " +
                                             PartnerAddressAggregateTDSChangePromotionParametersTable.GetLocationKeyOfEditedRecordDBName() + " = " +
                                             AddressAddedOrChangedPromotionRow.LocationKey.ToString();

                            // MessageBox.Show('FilterCriteria: ' + FilterCriteria);
#endif
                            LocationRow = (PLocationRow)FMainDS.PLocation.Rows.Find(new Object[] { AddressAddedOrChangedPromotionRow.SiteKey,
                                                                                                   AddressAddedOrChangedPromotionRow.LocationKey });

                            if (LocationRow == null)
                            {
                                /*
                                 * Location not found with PK -> check whether any Location has a PK
                                 * with the Original PK values that we are looking for (this is needed
                                 * just in the case the Address was Edited, a different Address was
                                 * found by the user (therefore the PK changes) and a PartnerLocation
                                 * field was changed).
                                 */
                                LocationRow =
                                    FindLocationRowWithOriginalKey(new TLocationPK(AddressAddedOrChangedPromotionRow.SiteKey,
                                            (Int32)AddressAddedOrChangedPromotionRow.LocationKey));
                            }

                            if (LocationRow != null)
                            {
#if TODO
                                PersonsLocationsDV = new DataView(AParameterDT,
                                    FilterCriteria,
                                    PartnerAddressAggregateTDSChangePromotionParametersTable.GetPartnerKeyDBName() + " ASC",
                                    DataViewRowState.CurrentRows);

                                LocationChangedDialog = new TPartnerLocationChangePropagationSelectionWinForm();
                                LocationChangedDialog.SetParameters(AddressAddedOrChangedPromotionRow, PersonsLocationsDV, LocationRow, "", "");

                                if (LocationChangedDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                                {
                                    AddressAddedOrChangedPromotionRow.UserAnswer = "CANCEL";

                                    // get LocationChangedDialog out of memory
                                    LocationChangedDialog.Dispose();

                                    // MessageBox.Show('LocationChangedDialog: pressed Cancel.');
                                }
                                else
                                {
                                    if (LocationChangedDialog.GetReturnedParameters(out UserAnswer))
                                    {
                                        AddressAddedOrChangedPromotionRow.UserAnswer = UserAnswer;
#endif
                                AddressAddedOrChangedPromotionRow.UserAnswer = "NO";          // TODO Remove this assignment once the code lines immediately above are no longer in compiler directive '#if Todo'!
#if TODO
                            }
                            else
                            {
                                throw new System.Exception(
                                    "GetReturnedParameters called, but Form '" + LocationChangedDialog.Name +
                                    "' is not finished yet with initialisation");
                            }

                            // get NewPartnerDialog out of memory
                            LocationChangedDialog.Dispose();
                        }
#endif
                            }
                            else
                            {
                                MessageBox.Show(
                                    "Error in " + this.GetType().FullName + ".AddressAddedOrChangedProcessing (PartnerLocationChange): " +
                                    "Location with SiteKey " + AddressAddedOrChangedPromotionRow.SiteKey.ToString() + " and LocationKey " +
                                    AddressAddedOrChangedPromotionRow.LocationKey.ToString() + " could not be found on the Client side!");
                                AddressAddedOrChangedPromotionRow.UserAnswer = "CANCEL";
                            }
                        }
                        else
                        {
                            throw new System.ArgumentException("AParameterDT must not be nil when PartnerLocationChange = true");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applies Petra Security to restrict functionality, if needed.
        /// </summary>
        /// <returns>void</returns>
        protected void ApplySecurity()
        {
            // Check security for New record operations
            if ((!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE,
                     PLocationTable.GetTableDBName()))
                || (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE,
                        PPartnerLocationTable.GetTableDBName()))
                || (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerTable.GetTableDBName())))
            {
                // needed for setting p_partner.s_date_modified_d = TODAY
                btnNewRecord.Enabled = false;
            }

            // Check security for Edit record operations
            if ((!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY,
                     PLocationTable.GetTableDBName()))
                || (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY,
                        PPartnerLocationTable.GetTableDBName()))
                || (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerTable.GetTableDBName())))
            {
                // needed for setting p_partner.s_date_modified_d = TODAY
                btnDeleteRecord.Enabled = false;
            }

            // Check security for Delete record operations
            // do not allow deletion of record, if this is location 0 (this is done in DataGrid_FocusRowEntered)
            if (FLogic.CheckDeleteSecurityGeneral(false) && btnDeleteRecord.Enabled)
            {
                btnDeleteRecord.Enabled = true;

                // Note: more checks are done only on clicking the Delete button
                // due to the need for a server roundtrip.
            }
            else
            {
                btnDeleteRecord.Enabled = false;
            }

            // Extra security check for LocationType SECURITY_CAN_LOCATIONTYPE
            // MessageBox.Show('Current LocationType: ' + this.PartnerLocationDataRowOfCurrentlySelectedRecord.LocationType);
            if ((this.PartnerLocationDataRowOfCurrentlySelectedRecord != null)
                && (!this.PartnerLocationDataRowOfCurrentlySelectedRecord.IsLocationTypeNull()))
            {
                if (this.PartnerLocationDataRowOfCurrentlySelectedRecord.LocationType.EndsWith(SharedConstants.SECURITY_CAN_LOCATIONTYPE)
                    && (!UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_ADDRESSCAN)))
                {
                    btnDeleteRecord.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <remarks>May be called by the Form that hosts this UserControl to invoke the data validation of
        /// the UserControl.</remarks>
        /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing
        /// to another record, otherwise set it to false.</param>
        /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
        /// user, otherwise set it to false.</param>
        /// <param name="AValidateSpecificControl">Pass in a Control to restrict Data Validation error checking to a
        /// specific Control for which Data Validation errors might have been recorded. (Default=this.ActiveControl).
        /// <para>
        /// This is useful for restricting Data Validation error checking to the current TabPage of a TabControl in order
        /// to only display Data Validation errors that pertain to the current TabPage. To do this, pass in a TabControl in
        /// this Argument.
        /// </para>
        /// </param>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        public bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors, Control AValidateSpecificControl = null)
        {
            bool ReturnValue = true;

// TODO
//            bool ReturnValue = false;
//            Control ControlToValidate;
//            PSubscriptionRow CurrentRow;
//
//            CurrentRow = GetSelectedDetailRow();
//
//            if (CurrentRow != null)
//            {
//                if (AValidateSpecificControl != null)
//                {
//                    ControlToValidate = AValidateSpecificControl;
//                }
//                else
//                {
//                    ControlToValidate = this.ActiveControl;
//                }
//
//                GetDetailsFromControls(CurrentRow);
//
//                // TODO Generate automatic validation of data, based on the DB Table specifications (e.g. 'not null' checks)
//                ValidateDataDetailsManual(CurrentRow);
//
//                if (AProcessAnyDataValidationErrors)
//                {
//                    // Only process the Data Validations here if ControlToValidate is not null.
//                    // It can be null if this.ActiveControl yields null - this would happen if no Control
//                    // on this UserControl has got the Focus.
//                    if(ControlToValidate.FindUserControlOrForm(true) == this)
//                    {
//                        ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(false, FPetraUtilsObject.VerificationResultCollection,
//                            this.GetType(), ControlToValidate.FindUserControlOrForm(true).GetType());
//                    }
//                    else
//                    {
//                        ReturnValue = true;
//                    }
//                }
//            }
//            else
//            {
//                ReturnValue = true;
//            }
//
//            if(ReturnValue)
//            {
//                // Remove a possibly shown Validation ToolTip as the data validation succeeded
//                FPetraUtilsObject.ValidationToolTip.RemoveAll();
//            }

            return ReturnValue;
        }

        #endregion

        #region Helper functions

        /// <summary>
        /// Switches between 'Read Only Mode' and 'Edit Mode' of the Detail UserControl.
        ///
        /// In 'Read Only Mode' all controls in the Detail UserControl are disabled and
        /// the three Buttons next to the Grid are set up to perform Add, Edit and Delete
        /// operations. In 'Edit Mode' the controls in the Detail UserControl are enabled
        /// and the three Buttons next to the Grid are set up to perform Done and Cancel
        /// operations.
        ///
        /// Only location 0 is read only!!!
        /// </summary>
        /// <param name="ASelectCurrentRow">Set this to true to select the DataRow that is deemed
        /// to be the 'current' one in FLogic in the Grid and to DataBind the Details
        /// UserControl to this row, otherwise set to false.
        /// </param>
        /// <returns>void</returns>
        protected void SwitchDetailReadOnlyModeOrEditMode(Boolean ASelectCurrentRow)
        {
            Int32 TmpRowIndex;
            DataRowView TmpDataRowView;

            // MessageBox.Show('Executing SwitchDetailReadOnlyModeOrEditMode');
            if (!(FIsEditingRecord))
            {
                FIsEditingRecord = true;
                ucoDetails.SetMode(TDataModeEnum.dmEdit);
            }
            else
            {
                FIsEditingRecord = false;
                ucoDetails.SetMode(TDataModeEnum.dmBrowse);
            }

            if (ASelectCurrentRow)
            {
                // Select current row in the Grid
                TmpDataRowView = FLogic.DetermineRecordToSelect((grdRecordList.DataSource as DevAge.ComponentModel.BoundDataView).DataView);
                TmpRowIndex = grdRecordList.Rows.DataSourceRowToIndex(TmpDataRowView);

                // MessageBox.Show('Selecting TmpRowIndex: ' + TmpRowIndex.ToString);
                grdRecordList.Selection.ResetSelection(false);
                grdRecordList.Selection.SelectRow(TmpRowIndex + 1, true);

                // Scroll grid to line where the new record is now displayed
                grdRecordList.ShowCell(new Position(TmpRowIndex + 1, 0), true);
                FLogic.DetermineCurrentKey(TmpRowIndex + 1);

                // DataBind the Detail UserControl to different record
                ucoDetails.UpdateDataBinding(FLogic.LocationKey);
            }

            ApplySecurity();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void AdjustAfterResizing()
        {
            ucoDetails.AdjustLabelControlsAfterResizing();
            SetupDataGridVisualAppearance();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void AdjustLabelControlsAfterResizing()
        {
            ucoDetails.AdjustLabelControlsAfterResizing();
            SetupDataGridVisualAppearance();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleDisabledControlClick(System.Object sender, System.EventArgs e)
        {
            //          MessageBox.Show("UC_PartnerAddressES: HandleDisabledControlClick called from \"" + sender.ToString() + "\"");
            BtnEditRecord_Click(this, null);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ALocationPK"></param>
        /// <returns></returns>
        public Boolean IsAddressRowPresent(TLocationPK ALocationPK)
        {
            return FLogic.IsAddressRowPresent(ALocationPK);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for Button Click Event
        /// </summary>
        /// <returns>void</returns>
        private void BtnMaximiseMinimiseGrid_Click(System.Object sender, System.EventArgs e)
        {
            if (!FIsGridMaximised)
            {
                FIsGridMaximised = true;
                FGridMinimisedSize = pnlRecordList.Height;
                pnlRecordList.Height = 324;
            }
            else
            {
                FIsGridMaximised = false;
                pnlRecordList.Height = FGridMinimisedSize;
            }
        }

        /// <summary>
        /// Event Handler for Button Click Event
        /// </summary>
        /// <returns>void</returns>
        private void BtnNewRecord_Click(System.Object sender, System.EventArgs e)
        {
            ActionNewRecord();
        }

        /// <summary>
        /// Adds a new record.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void ActionNewRecord()
        {
            Int32 AddedLocationsRowKey;
            DataRowView TmpDataRowView;
            Int32 TmpRowIndex;
            Int64 SiteKey;
            Int32 LocationKey;

            try
            {
                // MessageBox.Show('Running ActionNewRecord...');
                if (btnNewRecord.Enabled)
                {
                    // Determine current row in the Grid
                    TmpDataRowView = FLogic.DetermineRecordToSelect((grdRecordList.DataSource as DevAge.ComponentModel.BoundDataView).DataView);
                    TmpRowIndex = grdRecordList.Rows.DataSourceRowToIndex(TmpDataRowView);

                    //                  MessageBox.Show("TmpRowIndex: " + TmpRowIndex.ToString());

                    // Determine PrimaryKey of the current row in the Grid
                    LocationKey = FLogic.DetermineCurrentKey(TmpRowIndex + 1);

                    // using the sideeffect of the previous DetermineCurrentSiteKey call
                    SiteKey = FLogic.SiteKey;

                    // Create new record (based on either the selected record or a found record)
                    AddedLocationsRowKey = FLogic.AddRecord(SiteKey, LocationKey, FFoundNewAddressLocationKey);

                    // DataBind the Detail UserControl to new record
                    ucoDetails.Key = FFoundNewAddressLocationKey;
                    ucoDetails.RefreshDataBoundFields();
                    FLastKey = AddedLocationsRowKey;

                    // Enable Details UserControl's edit fields
                    SwitchDetailReadOnlyModeOrEditMode(true);
                    ucoDetails.SetAddressFieldOrder();
                    ucoDetails.Focus();

                    btnDeleteRecord.Enabled = true;
                }

                ApplySecurity();
            }
            catch (Exception Exp)
            {
                TLogging.Log("Exception occured in ActionNewRecord: " + Exp.ToString());
            }
        }

        /// <summary>
        /// Event Handler for Button Click Event
        /// </summary>
        /// <returns>void</returns>
        private void BtnEditRecord_Click(System.Object sender, System.EventArgs e)
        {
            ActionEditRecord();
        }

        /// <summary>
        /// Either edits the currently selected record or ends the editing of the
        /// currently selected record.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void ActionEditRecord()
        {
            PartnerEditTDSPPartnerLocationRow ErroneousRow;
            String ErrorMessages;
            Control FirstErrorControl;
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            // only allow edit if deletion is allowed
            if (btnDeleteRecord.Enabled)
            {
                // MessageBox.Show('Running ActionEditRecord...');
                if (!FIsEditingRecord)
                {
                    //
                    // Record should be edited
                    //
                    FLogic.EditRecord();

                    // MessageBox.Show('After FLogic.EditRecord.');
                    SwitchDetailReadOnlyModeOrEditMode(false);

                    // MessageBox.Show('SwitchDetailReadOnlyModeOrEditMode(false).');
                    ucoDetails.SetAddressFieldOrder();
                    ucoDetails.Focus();

                    // MessageBox.Show('Finished running ActionEditRecord.');
                    return;
                }
                else
                {
                    //
                    // User is submitting an edited record
                    //
                    if (!FPetraUtilsObject.VerificationResultCollection.Contains(ucoDetails))
                    {
                        // Data Validation OK
                        ucoDetails.SaveUnboundControlData();
                        FLogic.ProcessEditedRecord(out ErroneousRow);
                    }
                    else
                    {
                        // Data Validation failed
                        FPetraUtilsObject.VerificationResultCollection.BuildScreenVerificationResultList(ucoDetails,
                            out ErrorMessages,
                            out FirstErrorControl);

                        TMessages.MsgRecordChangeVerificationError(ErrorMessages, this.GetType());

                        FirstErrorControl.Focus();
                        return;
                    }
                }

                // Enable 'New' Address Button if it was initially disabled
                if (FDisabledNewButtonOnAutoCreatedAddress)
                {
                    FDisabledNewButtonOnAutoCreatedAddress = false;
                    btnNewRecord.Enabled = true;
                    ApplySecurity();
                }

                SwitchDetailReadOnlyModeOrEditMode(true);

                // Fire OnRecalculateScreenParts event
                RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
                RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
                OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);

                // Make the Grid respond on updown keys
                grdRecordList.Focus();

                // MessageBox.Show('Finished running ActionEditRecord.');
            }
        }

        /// <summary>
        /// Allows adding an Address that the user has found (using Menu 'Edit'->'Find
        /// New Address').
        ///
        /// @comment The found Address must be merged into the PLocation DataTable in the
        /// FMultiTableDS before this function can be called! The record that was merged
        /// gets deleted by a call to this procedure!
        ///
        /// </summary>
        /// <param name="ASiteKey">SiteKey of the found Location</param>
        /// <param name="ALocationKey">LocationKey of the found Location
        /// </param>
        /// <returns>void</returns>
        public void AddNewFoundAddress(Int64 ASiteKey, Int32 ALocationKey)
        {
            FFoundNewAddressLocationKey = ALocationKey;

            // The 'New' button needs to be enabled in order for ActionNewAddress to be run!
            btnNewRecord.Enabled = true;
            ActionNewRecord();
            FFoundNewAddressLocationKey = 0;
        }

        /// <summary>
        /// Event Handler for Button Click Event
        /// </summary>
        /// <returns>void</returns>
        private void BtnDeleteRecord_Click(System.Object sender, System.EventArgs e)
        {
            Int32 NewlySelectedRow;
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            // MessageBox.Show('btnDeleteRecord_Click');
            if (btnDeleteRecord.Enabled)
            {
                if (FLogic.DeleteRecord())
                {
                    // Ensure that following Selection.ActivePosition inquiries will work!
                    grdRecordList.Focus();

                    // Update the details section to show the details of the now selected row
                    if (grdRecordList.Selection.ActivePosition != Position.Empty)
                    {
                        NewlySelectedRow = grdRecordList.Selection.ActivePosition.Row;
                    }
                    else
                    {
                        NewlySelectedRow = grdRecordList.Rows.Count - 1;
                    }

//                      MessageBox.Show("NewlySelectedRow: " + NewlySelectedRow.ToString());
                    if (grdRecordList.Rows.Count > 1)
                    {
                        // Deleted any row but the last one

                        grdRecordList.Selection.SelectRow(NewlySelectedRow, true);
                        grdRecordList.ShowCell(new Position(NewlySelectedRow - 1, 0), true);

                        // Make the Grid respond on updown keys
                        grdRecordList.Selection.Focus(new Position(NewlySelectedRow, 1), true);
                        DataGrid_FocusRowEntered(this, new RowEventArgs(NewlySelectedRow));
                    }
                    else
                    {
                        // Deleted last row, adding default one
                        FLogic.AddDefaultRecord();
                        grdRecordList.Selection.SelectRow(1, true);
                        grdRecordList.ShowCell(new Position(1, 0), true);

                        // Make the Grid respond on updown keys
                        grdRecordList.Selection.Focus(new Position(1, 1), true);
                        DataGrid_FocusRowEntered(this, new RowEventArgs(1));

                        // Enable 'New' Address Button if it was initially disabled
                        if (FDisabledNewButtonOnAutoCreatedAddress)
                        {
                            FDisabledNewButtonOnAutoCreatedAddress = false;
                            btnNewRecord.Enabled = true;
                            ApplySecurity();
                        }
                    }

                    FPetraUtilsObject.SetChangedFlag();

                    // Fire OnRecalculateScreenParts event
                    RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
                    RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
                    OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
                }
            }
        }

        #endregion

        #region SourceGrid Event Handlers

        /// <summary>
        /// Event Handler for FocusRowEntered Event of the Grid
        ///
        /// </summary>
        /// <param name="ASender">The Grid</param>
        /// <param name="AEventArgs">RowEventArgs as specified by the Grid (use Row property to
        /// get the Grid row for which this Event fires)
        /// </param>
        /// <returns>void</returns>
        protected void DataGrid_FocusRowEntered(System.Object ASender, RowEventArgs AEventArgs)
        {
            // Determine current Location Key and enable/disable buttons
            if (FLogic.DetermineCurrentKey(AEventArgs.Row) == 0)
            {
                btnDeleteRecord.Enabled = false;
            }
            else
            {
                btnDeleteRecord.Enabled = true;
            }

            ApplySecurity();

            if (btnDeleteRecord.Enabled == true)
            {
                FIsEditingRecord = false;
                ActionEditRecord();
            }

            // Update detail section only if user actually changed to a different record
            // (this Event also fires if the user just (double)clicks on the same record again)
            if (FLogic.LocationKey != FLastKey)
            {
                // DataBind the Detail UserControl to different record
                ucoDetails.UpdateDataBinding(FLogic.LocationKey);
                ucoDetails.SetMode(TDataModeEnum.dmBrowse);
                FLastKey = ucoDetails.Key;
            }
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void GrdRecordList_EnterKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            BtnEditRecord_Click(this, null);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void GrdRecordList_DoubleClickCell(System.Object Sender, SourceGrid.CellContextEventArgs e)
        {
            // MessageBox.Show('grdRecordList_DoubleClickCell:  Column: ' + e.CellContext.Position.Column.ToString  + '; Row: ' + e.CellContext.Position.Row.ToString);
            ActionEditRecord();
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void GrdRecordList_InsertKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            BtnNewRecord_Click(this, null);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void GrdRecordList_DeleteKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            if (e.Row != -1)
            {
                BtnDeleteRecord_Click(this, null);
            }
        }

        #endregion

        #region Setup SourceDataGrid

        /// <summary>
        /// Sets up the DataBinding of the Grid. Also selects the row containing the
        /// 'Best Address'.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetupDataGridDataBinding()
        {
            // DataBind the DataGrid
            grdRecordList.DataSource = new DevAge.ComponentModel.BoundDataView(FDataGridDV);

            // Hook up event that fires when a different row is selected
            grdRecordList.Selection.FocusRowEntered += new RowEventHandler(this.DataGrid_FocusRowEntered);

            SelectBestAddressRow(true);
        }

        /// <summary>
        /// Sets up the visual appearance of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetupDataGridVisualAppearance()
        {
            grdRecordList.AutoSizeCells();

            /*
             * HACK ***Temporary*** solution to make the Grid appear
             * correctly on "Large Fonts (120DPI)" display setting:
             * Do not reassign the width; on 120DPI it causes the Grid to
             * become very wide and this pushes the Add, Edit and Delete
             * buttons off the screen once the user switches to another Tab
             * and then back to this Tab!
             */
            if (!TClientSettings.GUIRunningOnNonStandardDPI)
            {
                // It is necessary to reassign the width because the columns don't take up the maximum width
                grdRecordList.Width = btnNewRecord.Left - grdRecordList.Left - 6;
            }

            // It is necessary to change the automatically calculated Height of the Rows to make them as
            // high as they used to be with SourceGrid3 (it seems that SourceGrid4 takes the Icons' Height
            // into consideration, whereas SourceGrid3 only looked at the Font size)
            for (int Counter = 0; Counter <= grdRecordList.Rows.Count; Counter++)
            {
                grdRecordList.Rows.SetHeight(Counter, 19);
            }
        }

        #endregion
        #endregion
    }

    #region TThreaded

    /// <summary>
    /// todoComment
    /// </summary>
    public class TThreaded
    {
#if BALLOONTIP
// TODO
        private static TBalloonTip UBalloonTip;

        public static void PrepareBalloonTip()
        {
            // Ensure that we have an instance of TBalloonTip
            if (UBalloonTip == null)
            {
                UBalloonTip = new TBalloonTip();
            }
        }
#endif



        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControl"></param>
        public static void ThreadedCheckForUserTips(System.Object AControl)
        {
            /*
             * Check for specific TipStatus: '!' means 'show the Tip, but then set it'
             *
             * This is done to pick up Tips only for the Users that they were specifically
             * set for, and not for users where the TipStatus would be un-set ('-')!
             * This is helfpful eg. for Patch Notices - they should be shown to all current
             * users (the patch program would set the TipStatus to '!' for all current users),
             * but not for users that get created after the Patch was applied (they don't
             * need to know what was new/has changed in a certain Patch that was applied in
             * the past!)
             */
            if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersAddresses) == '!')
            {
                // Set Tip Status so it doesn't get picked up again!
                TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatNewTabCountersAddresses);
#if BALLOONTIP
// TODO Balloontip
                PrepareBalloonTip();
                UBalloonTip.ShowBalloonTipNewFunction(
                    "Petra Version 2.2.7 Change: Counter in Addresses Tab Header",
                    "The number in the tab header no longer shows just the number of records - from now on" + "\r\n" +
                    "it takes only current Addresses (non-past and non-future Addresses) into account." + "\r\n" +
                    "If there is no current Address on file, the number will be 0, and an exclamation mark will" + "\r\n" + "be shown as well." +
                    "\r\n" +
                    "The Subscriptions tab and Notes tab also have changes to the counters in the tab headers.",
                    (Control)AControl);
                UBalloonTip = null;
#endif
            }
        }
    }
    #endregion
}