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
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using SourceGrid;
using SourceGrid.Cells;
using Ict.Common.Data;
using Ict.Common.Controls;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;

namespace Ict.Petra.Client.MCommon
{
    /// <summary>
    /// Contains logic for the UC_PartnerAddresses UserControl.
    ///
    /// </summary>
    public class TUCPartnerAddressesLogic : System.Object
    {
        #region Resourcestrings

        /// <summary>todoComment</summary>
        private static readonly string StrAnotherAddressIsMailingAddress = Catalog.GetString(
            "Another Address is already set to be a Mailing Address.\r\n\r\n" +
            "This could lead to confusion as to which Address is the 'Best' Address (=the default Address OpenPetra will use).\r\n" +
            "OpenPetra has already marked the 'Best' Address in the list of Addresses (green icon background).\r\n" +
            "Please check if this is the correct one!");

        /// <summary>todoComment</summary>
        private static readonly string StrAnotherAddressIsMailingAddressTitle = Catalog.GetString("Multiple Mailing Addresses");

        /// <summary>todoComment</summary>
        private static readonly string StrSecurityViolationExplanation1 = Catalog.GetString(
            "You tried to delete the last Address that the Partner had.\r\n" +
            "For this operation you need modify permission on PartnerLocation records,\r\nwhich you do not have.");

        /// <summary>todoComment</summary>
        private static readonly string StrSecurityViolationExplanation2 = Catalog.GetString(
            "You tried to delete an Address that isn't used by any other Partner.\r\n" +
            "For this operation you need delete permission on Location records,\r\nwhich you do not have.");

        /// <summary>todoComment</summary>
        private static readonly string StrSecurityViolationExplanationTitle = Catalog.GetString("Security Violation - Explanation");

        /// <summary>todoComment</summary>
        private static readonly string StrDeleteQuestionLine1 = Catalog.GetString("Are you sure you want to remove this address");

        /// <summary>todoComment</summary>
        private static readonly string StrDeleteQuestionShared = Catalog.GetString("from this partner?");

        /// <summary>todoComment</summary>
        private static readonly string StrDeleteQuestionNotShared = Catalog.GetString("from the database (it is not used elsewhere)?");

        /// <summary>todoComment</summary>
        private static readonly string StrDeleteQuestionTitle = Catalog.GetString("Delete Address?");

        /// <summary>todoComment</summary>
        private static readonly string StrFoundAddressIsDuplicate = Catalog.GetString(
            "The found Address cannot be used because the" +
            " Partner already has an Address record with this address!");

        /// <summary>todoComment</summary>
        private static readonly string StrFoundAddressIsDuplicateTitle = Catalog.GetString("Duplicate Address");

        /// <summary>todoComment</summary>
        private static readonly string StrPartnerReActivationBecauseOfNewAddr = Catalog.GetString(" because you have added\r\na new Address!");

        /// <summary>todoComment</summary>
        private static readonly string StrAddressCannotBeExpired = Catalog.GetString(
            "The Address\r\n\r\n   {0}\r\n\r\n" +
            "has a Valid-From date that lies after the date that you entered for\r\nexpiration.\r\n\r\n" +
            "This Address will not be expired because the Valid-To date cannot lie\r\nbefore the Valid-From date!");

        /// <summary>todoComment</summary>
        private static readonly string StrAddressCannotBeExpiredTitle = Catalog.GetString("Address Cannot be Expired");

        #endregion

        /// <summary>Holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary>Holds a reference to the DataSet that contains most data that is used on the screen</summary>
        private PartnerEditTDS FMainDS;

        /// <summary>DataView that the SourceDataGrid is bound to</summary>
        private DataView FDataGridDV;

        /// <summary>DataRow of the record we are currently working with</summary>
        private PLocationRow FCurrentDataRow;

        /// <summary>DataTable Key value for the record we are currently working with</summary>
        private Int32 FCurrentLocationKey;

        /// <summary>DataTable Key value for the record we are currently working with</summary>
        private Int64 FCurrentSiteKey;

        /// <summary>Holds a reference to the DataGrid that is used to display the records</summary>
        private TSgrdDataGrid FDataGrid;

        /// <summary>Holds a reference to an ImageList containing Icons that can be shown in Grid Rows</summary>
        private ImageList FGridRowIconsImageList;

        /// <summary>Fictive DataTable Key value for new records that is used on the Client side  until the record is saved by the PetraServer and gets a proper Key value</summary>
        private Int32 FClientSideNewDataRowKey;

        /// <summary>true if a record is beeing added, otherwise false</summary>
        private Boolean FIsRecordBeingAdded;

        /// <summary>DataTable Key value of an existing record that is used in creating a new record</summary>
        private Int32 FNewRecordFromRecordKey;

        /// <summary>Value of DateEffective DataColumn before the user edits the current record</summary>
        private DateTime? FValidFromBeforeEditing;

        /// <summary>Value of DateGoodUntil DataColumn before the user edits the current record</summary>
        private DateTime? FValidUntilBeforeEditing;

        /// <summary>Value of SendMail DataColumn before the user edits the current record</summary>
        private Boolean FSendMailBeforeEditing;

        /// <summary>Copy of the PartnerLocation record that is being deleted</summary>
        private DataRow FJustDeletedPartnerLocationsRow;

        /// <summary>Array holding LocationKeys of records that need to be cleaned up (deleted) before Merging</summary>
        private Int32[] FCleanupAddressesLocationKeys;
        private Boolean FRecordBeingAddedIsFoundRecord;
        private TLocationPK FRecordKeyBeforeFinding;
        private PLocationRow FLocationRowAfterCopying;

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

        /// <summary>DataTable Key value for the record we are working with</summary>
        public Int32 LocationKey
        {
            get
            {
                return FCurrentLocationKey;
            }

            set
            {
                FCurrentLocationKey = value;
            }
        }

        /// <summary>DataTable Key value for the record we are working with</summary>
        public Int64 SiteKey
        {
            get
            {
                return FCurrentSiteKey;
            }

            set
            {
                FCurrentSiteKey = value;
            }
        }

        /// <summary>DataGrid that is used to display the records</summary>
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

        /// <summary>ImageList containing Icons that can be shown in Grid Rows</summary>
        public ImageList GridRowIconsImageList
        {
            get
            {
                return FGridRowIconsImageList;
            }

            set
            {
                FGridRowIconsImageList = value;
            }
        }

        /// <summary>true if a record is beeing added, otherwise false</summary>
        public Boolean IsRecordBeingAdded
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

        /// <summary>Record that is currently selected (p_location DataTable)</summary>
        public PLocationRow LocationDataRowOfCurrentlySelectedRecord
        {
            get
            {
                return FCurrentDataRow;
            }
        }

        /// <summary>Record that is currently selected (p_partner_location DataTable)</summary>
        public PartnerEditTDSPPartnerLocationRow PartnerLocationDataRowOfCurrentlySelectedRecord
        {
            get
            {
                /* MessageBox.Show('get_PartnerLocationDataRowOfCurrentlySelectedRecord: looking for Row with Key: ' + FMainDS.PPartner[0].PartnerKey.ToString + ', ' + FCurrentDataRow.SiteKey.ToString + ', ' + FCurrentDataRow.LocationKey.ToString);
                **/
                return (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.Rows.Find(new Object[] { FMainDS.PPartner[0].PartnerKey,
                                                                                                            FCurrentDataRow.SiteKey,
                                                                                                            FCurrentDataRow.LocationKey });

                // if result = nil then
                // begin
                // MessageBox.Show('get_PartnerLocationDataRowOfCurrentlySelectedRecord: Row now found!');
                // end;
            }
        }

        /// <summary>Key value of an existing record that is used to create a new record</summary>
        public Int32 NewRecordFromRecordKey
        {
            get
            {
                return FNewRecordFromRecordKey;
            }
        }

        /// <summary>Holds the PK of the PLocationTable before the PK was changed to the PK of the found Address if a Find Address command was issued by the user, otherwilse this is nil</summary>
        public TLocationPK RecordKeyBeforeFinding
        {
            get
            {
                return FRecordKeyBeforeFinding;
            }

            set
            {
                FRecordKeyBeforeFinding = value;
            }
        }


        #region TUCPartnerAddressesLogic

        /// <summary>
        /// Performs necessary actions to make the Merging of rows that were changed on
        /// the Server side into the Client-side DataSet possible.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CleanupRecordsBeforeMerge()
        {
            DataView NewLocationsDV;
            DataView NewPartnerLocationsDV;
            Int16 LocationsCounter;
            Int16 PartnerLocationsCounter;
            int Counter;
            String AdditionalCleanupWhereClause;
            ArrayList PLocationDeleteRows;

            DataRow[] PLocationCleanupRows;
            ArrayList PPartnerLocationDeleteRows;
            DataRow[] PPartnerLocationCleanupRows;
            int Counter2;

            PLocationDeleteRows = new ArrayList();
            PPartnerLocationDeleteRows = new ArrayList();

            /*
             * Check if Locations/PartnerLocations have been added
             * -> remove them on the Client side, otherwise we will end up with these rows
             *   (having LocationKeys below 0) plus the rows coming from the Server
             *   (having LocationKeys that were determined by a Sequence) after merging
             */
            NewLocationsDV = new DataView(FMainDS.PLocation, "", "", DataViewRowState.Added);
            NewPartnerLocationsDV = new DataView(FMainDS.PPartnerLocation, "", "", DataViewRowState.Added);

            // First check and remember affected DataRows
            for (LocationsCounter = 0; LocationsCounter <= NewLocationsDV.Count - 1; LocationsCounter += 1)
            {
                PLocationDeleteRows.Add(NewLocationsDV[LocationsCounter].Row);

                //        NewLocationsDV.Table.Rows.Remove(NewLocationsDV[0].Row);
            }

            for (PartnerLocationsCounter = 0; PartnerLocationsCounter <= NewPartnerLocationsDV.Count - 1; PartnerLocationsCounter += 1)
            {
                PPartnerLocationDeleteRows.Add(NewPartnerLocationsDV[PartnerLocationsCounter].Row);

                //        NewPartnerLocationsDV.Table.Rows.Remove(NewPartnerLocationsDV[0].Row);
            }

            // Now remove affected DataRows
            foreach (DataRow PLocationDeleteRow in PLocationDeleteRows)
            {
                NewLocationsDV.Table.Rows.Remove(PLocationDeleteRow);
            }

            foreach (DataRow PPartnerLocationDeleteRow in PPartnerLocationDeleteRows)
            {
                NewPartnerLocationsDV.Table.Rows.Remove(PPartnerLocationDeleteRow);
            }

            /*
             * Check if PartnerLocation 0 is present
             *   -> remove it on the Client side, otherwise we will get a Merge Exception
             *      when the data coming from Location 0 on the Server will be merged
             */

            // This causes bug http:bugs.om.org/petra/show_bug.cgi?id=757, when deleting the last address;
            // we need the location 0, otherwise there is no location at all, and it seems there is no location 0 coming from the server anyways
            // NewPartnerLocationsDV := new DataView(FMainDS.PPartnerLocation,
            // PPartnerLocationTable.GetLocationKeyDBName() +  ' = 0', '',
            // DataViewRowState.CurrentRows);
            //
            // if NewPartnerLocationsDV.Count = 1 then
            // begin
            // /MessageBox.Show('Location 0 found, deleting it.');
            // NewPartnerLocationsDV.Table.Rows.Remove(NewPartnerLocationsDV[0].Row);
            // end;

            /*
             *
             * Additional cleanup to be able to cope with re-using of addresses also in
             * the case where an edited address is changed to an existing address and the
             * existing address should be re-used. In this case we need to delete the
             * edited record (that had a certain LocationKey <> 0) before merging with the
             * DataSet from the PetraServer.
             */
            if (FCleanupAddressesLocationKeys.Length != 0)
            {
                AdditionalCleanupWhereClause = "";

                // built WHERE criteria for selecting the rows that need cleanup
                for (Counter = 0; Counter <= FCleanupAddressesLocationKeys.Length - 1; Counter += 1)
                {
                    AdditionalCleanupWhereClause = AdditionalCleanupWhereClause + FCleanupAddressesLocationKeys[Counter].ToString() + ',';
                }

                // remove last ','
                AdditionalCleanupWhereClause = AdditionalCleanupWhereClause.Substring(0, AdditionalCleanupWhereClause.Length - 1);

                // MessageBox.Show('Cleaning additional rows with LocationKeys: ' + AdditionalCleanupWhereClause);
                // Select the rows that need cleanup
                PLocationCleanupRows = FMainDS.PLocation.Select(PLocationTable.GetLocationKeyDBName() + " IN (" + AdditionalCleanupWhereClause + ')');
                PPartnerLocationCleanupRows = FMainDS.PPartnerLocation.Select(
                    PPartnerLocationTable.GetLocationKeyDBName() + " IN (" + AdditionalCleanupWhereClause + ')');

                // Delete the rows that need cleanup
                for (Counter2 = 0; Counter2 <= PLocationCleanupRows.Length - 1; Counter2 += 1)
                {
                    FMainDS.PLocation.Rows.Remove(PLocationCleanupRows[Counter2]);
                }

                for (Counter2 = 0; Counter2 <= PPartnerLocationCleanupRows.Length - 1; Counter2 += 1)
                {
                    FMainDS.PPartnerLocation.Rows.Remove(PPartnerLocationCleanupRows[Counter2]);
                }

                // Reset array for next use!
                FCleanupAddressesLocationKeys = new int[0];
            }
        }

        /// <summary>
        /// Allows copying of an Address that the user has found (using Menu 'Edit'->'Find
        /// New Address') into the currently edited Address.
        ///
        /// </summary>
        /// <param name="AFoundAddressLocationRow">DataRow containing the Location information
        /// for the found Address.
        /// </param>
        /// <returns>void</returns>
        public void CopyFoundAddressData(PLocationRow AFoundAddressLocationRow)
        {
            PLocationRow CurrentLocationRow;
            Int32 FindLocationKey;
            Int64 FindSiteKey;
            PPartnerLocationRow CurrentPartnerLocationRow;

            if (FIsRecordBeingAdded)
            {
                FindLocationKey = FClientSideNewDataRowKey;
                FindSiteKey = FCurrentSiteKey;
                FRecordBeingAddedIsFoundRecord = true;
            }
            else
            {
                FindLocationKey = FCurrentLocationKey;
                FindSiteKey = FCurrentSiteKey;
            }

            // Get a reference to the DataRow that is currently selected in the Grid
            CurrentLocationRow = (PLocationRow)FMainDS.PLocation.Rows.Find(new Object[] { FindSiteKey, FindLocationKey });
            CurrentPartnerLocationRow =
                (PPartnerLocationRow)FMainDS.PPartnerLocation.Rows.Find(new Object[] { FMainDS.PPartner[0].PartnerKey, FindSiteKey,
                                                                                       FindLocationKey });

            // Remember what the Key was before we copy over data (needed for Cancelling)!
            FRecordKeyBeforeFinding = new TLocationPK(FindSiteKey, FindLocationKey);

            if (FIsRecordBeingAdded)
            {
                // MessageBox.Show('Before BeginEdit:' + "\r\n" + 'CurrentLocationRow RowState: ' + (Enum.GetName(typeof(DataRowState),
                // CurrentLocationRow.RowState)) + "\r\n" +
                // 'CurrentPartnerLocationRow RowState: ' + (Enum.GetName(typeof(DataRowState),
                // CurrentPartnerLocationRow.RowState)));
                CurrentLocationRow.BeginEdit();
                CurrentPartnerLocationRow.BeginEdit();

                // MessageBox.Show('After BeginEdit:' + "\r\n" + 'CurrentLocationRow RowState: ' + (Enum.GetName(typeof(DataRowState),
                // CurrentLocationRow.RowState)) + "\r\n" +
                // 'CurrentPartnerLocationRow RowState: ' + (Enum.GetName(typeof(DataRowState),
                // CurrentPartnerLocationRow.RowState)));
            }

            // update current PLocation record data
            TAddressHandling.CopyFoundAddressLocationData(AFoundAddressLocationRow, CurrentLocationRow);
            CurrentLocationRow.SiteKey = AFoundAddressLocationRow.SiteKey;
            CurrentLocationRow.LocationKey = AFoundAddressLocationRow.LocationKey;
            FCurrentDataRow = CurrentLocationRow;

            if (FLocationRowAfterCopying == null)
            {
                FLocationRowAfterCopying = FMainDS.PLocation.NewRowTyped();
            }

            FLocationRowAfterCopying.ItemArray = CurrentLocationRow.ItemArray;

            // MessageBox.Show('Before changing  CurrentPartnerLocationRow.LocationKey: ' + CurrentPartnerLocationRow.LocationKey.ToString + '; CurrentPartnerLocationRow.SiteKey: ' + CurrentPartnerLocationRow.SiteKey.ToString);
            // update current PPartnerLocation record data
            CurrentPartnerLocationRow.SiteKey = AFoundAddressLocationRow.SiteKey;
            CurrentPartnerLocationRow.LocationKey = (Int32)AFoundAddressLocationRow.LocationKey;

            // update the date that this change is effective
            CurrentPartnerLocationRow.SetDateGoodUntilNull();

            // needs to be set before DateEffective to prevent date check trigger to run!
            CurrentPartnerLocationRow.DateEffective = DateTime.Now.Date;
            FCurrentSiteKey = AFoundAddressLocationRow.SiteKey;
            FCurrentLocationKey = (Int32)AFoundAddressLocationRow.LocationKey;

            /* MessageBox.Show('CopyFoundAddressData: FCurrentLocationKey: ' + FCurrentLocationKey.ToString + ';  FCurrentSiteKey: ' + FCurrentSiteKey.ToString + '; CurrentPartnerLocationRow.LocationKey: ' +
             *CurrentPartnerLocationRow.LocationKey.ToString + '; CurrentPartnerLocationRow.SiteKey: ' + CurrentPartnerLocationRow.SiteKey.ToString + '; FRecordBeingAddedIsFoundRecord: ' + FRecordBeingAddedIsFoundRecord.ToString); */
        }

        /// <summary>
        /// Default Constructor.
        ///
        /// Initialises Fields.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TUCPartnerAddressesLogic() : base()
        {
            // Start FClientSideNewDataRowKey at 1, so that the first new Address gets
            // 2. This is necessary because in case a new Partner was created, the
            // first Address gets 1.
            FClientSideNewDataRowKey = -1;

            // TODO: use s_system_parameter.s_site_key_n once p_partner_location actually uses the Petra System SiteKey in the PrimaryKey (instead of 0, which is used currently)
            FCurrentSiteKey = SharedConstants.FIXED_SITE_KEY;

            // Initialise Array
            FCleanupAddressesLocationKeys = new int[0];
        }

        /// <summary>
        /// Checks security for deletion of non-shared Locations.
        ///
        /// </summary>
        /// <param name="AShowMessages">Set to true to show a message if the check fails,
        /// otherwise to false</param>
        /// <returns>true if the user has the necessary security privileges, otherwise
        /// false
        /// </returns>
        private Boolean CheckDeleteSecurityNonSharedLocation(Boolean AShowMessages)
        {
            Boolean ReturnValue;

            ReturnValue = true;

            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapDELETE, PLocationTable.GetTableDBName()))
            {
                if (AShowMessages)
                {
                    TMessages.MsgSecurityException(new ESecurityDBTableAccessDeniedException("", "delete",
                            PLocationTable.GetTableDBName()), this.GetType());
                    MessageBox.Show(StrSecurityViolationExplanation2,
                        StrSecurityViolationExplanationTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks security for deletion of Locations.
        ///
        /// </summary>
        /// <param name="AShowMessages">Set to true to show a message if the check fails,
        /// otherwise to false</param>
        /// <returns>true if the user has the necessary security privileges, otherwise
        /// false
        /// </returns>
        public Boolean CheckDeleteSecurityGeneral(Boolean AShowMessages)
        {
            Boolean ReturnValue;
            DataView ActiveLocationsDV;

            ReturnValue = true;
            ActiveLocationsDV = GetCurrentRecords();

            if (ActiveLocationsDV.Count > 1)
            {
                /*
                 * There is more than one record left, so p_partner_location records
                 * will need to be deletable!
                 */
                if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapDELETE, PPartnerLocationTable.GetTableDBName()))
                {
                    if (AShowMessages)
                    {
                        TMessages.MsgSecurityException(new ESecurityDBTableAccessDeniedException("", "delete",
                                PPartnerLocationTable.GetTableDBName()), this.GetType());
                    }

                    ReturnValue = false;
                }
            }
            else
            {
                /*
                 * There is only one record left, so p_partner_location records
                 * will need to be modifyable since LocationKey = 0 will get assigned
                 * to it!
                 */
                if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerLocationTable.GetTableDBName()))
                {
                    if (AShowMessages)
                    {
                        TMessages.MsgSecurityException(new ESecurityDBTableAccessDeniedException("", "modify",
                                PPartnerLocationTable.GetTableDBName()), this.GetType());
                        MessageBox.Show(StrSecurityViolationExplanation1,
                            StrSecurityViolationExplanationTitle,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }

                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns a DataView that contains only Current DataRows.
        ///
        /// </summary>
        /// <returns>DataView that contains only Current DataRows.
        /// </returns>
        private DataView GetCurrentRecords()
        {
            return new DataView(FMainDS.PLocation, "", "", DataViewRowState.CurrentRows);
        }

        /// <summary>
        /// Creates DataBound columns for the Grid control.
        ///
        /// </summary>
        /// <param name="ASourceTable">DataTable containing DataColumns which should be
        /// DataBound to columns of the DataGrid
        /// </param>
        /// <returns>void</returns>
        public void CreateColumns(PLocationTable ASourceTable)
        {
            FDataGrid.AddImageColumn(@GetAddressKindIconForGridRow);
            FDataGrid.AddImageColumn(@GetMailingAddressIconForGridRow);

            //
            // Address1
            FDataGrid.AddTextColumn("Address-1", ASourceTable.ColumnLocality);

            // Street2
            FDataGrid.AddTextColumn("Street-2", ASourceTable.ColumnStreetName);

            // Address3
            FDataGrid.AddTextColumn("Address-3", ASourceTable.ColumnAddress3);

            // City
            FDataGrid.AddTextColumn("City", ASourceTable.ColumnCity);

            // p_location_key_i (for testing purposes only...)
            // FDataGrid.AddTextColumn("Location Key", ASourceTable.ColumnLocationKey);

            // p_location_key_i (for testing purposes only...)
            // FDataGrid.AddTextColumn("PartnerLocation Key", ASourceTable.Columns["Parent_" + PPartnerLocationTable.GetLocationKeyDBName()]);

            // Location Type
            FDataGrid.AddTextColumn("Location Type", ASourceTable.Columns["Parent_" + PPartnerLocationTable.GetLocationTypeDBName()]);

            // Modification TimeStamp (for testing purposes only...)
            // FDataGrid.AddTextColumn("Modification TimeStamp", ASourceTable.ColumnModificationId);
        }

        #region Editing

        /// <summary>
        /// Edits the currently selected record.
        ///
        /// Note: a record actually consists of two records (one for the p_location table
        /// and one for the p_partner_location table).
        ///
        /// </summary>
        /// <returns>void</returns>
        public void EditRecord()
        {
            PartnerEditTDSPPartnerLocationRow PPartnerLocationRow;

            // MessageBox.Show('FCurrentLocationKey: ' + FCurrentLocationKey.ToString);
            FIsRecordBeingAdded = false;
            FRecordBeingAddedIsFoundRecord = false;

            // Find record of second DataTable that corresponds with the record of our first DataTable
            PPartnerLocationRow =
                (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.Rows.Find(new Object[] { FMainDS.PPartner[0].PartnerKey, FCurrentSiteKey,
                                                                                                     FCurrentLocationKey });

            // MessageBox.Show('FLocationRow: Proposed? ' + FLocationRow.HasVersion(DataRowVersion.Proposed).ToString +
            // 'PPartnerLocationRow: Proposed? ' + PPartnerLocationRow.HasVersion(DataRowVersion.Proposed).ToString);
            // MessageBox.Show('FLocationRow: HasVersion ? ' + FLocationRow.HasVersion(DataRowVersion.Proposed).ToString +
            // 'PPartnerLocationRow: Proposed? ' + PPartnerLocationRow.HasVersion(DataRowVersion.Proposed).ToString);
            // Call BeginEdit on all DataRows that are editable in the Detail UserControl.
            // THIS IS ESSENTIAL!!! It allows cancelling of editing by calling
            // CancelCurrentEdit on the associated CurrencyManager (done in the Detail
            // UserControl)!
            FCurrentDataRow.BeginEdit();
            PPartnerLocationRow.BeginEdit();

            // MessageBox.Show('FLocationRow: Proposed? ' + FLocationRow.HasVersion(DataRowVersion.Proposed).ToString +
            // 'PPartnerLocationRow: Proposed? ' + PPartnerLocationRow.HasVersion(DataRowVersion.Proposed).ToString);
            // Store values of some DataColumns before the user can edit them (used later for comparisons)
            FValidFromBeforeEditing = PPartnerLocationRow.DateEffective;
            FValidUntilBeforeEditing = PPartnerLocationRow.DateGoodUntil;
            FSendMailBeforeEditing = PPartnerLocationRow.SendMail;
        }

        /// <summary>
        /// Ensures that a 'default' record is always present.
        ///
        /// This makes it possible to have a Grid that always has at least 1 record (i.e.
        /// never has zero records).
        ///
        /// </summary>
        /// <returns>void</returns>
        public void EnsureDefaultRecordIsPresentIfNeeded()
        {
            DataView CurrentRecordsDV;

            CurrentRecordsDV = GetCurrentRecords();

            // MessageBox.Show('CurrentRecordsDV.Count: ' + CurrentRecordsDV.Count.ToString);
            if (CurrentRecordsDV.Count == 0)
            {
                AddDefaultRecord();

                // MessageBox.Show('Default Records Added!');
            }
        }

        /// <summary>
        /// Expires all Current Addresses (that is, non-Past and non-Future Addresses).
        ///
        /// </summary>
        /// <param name="ACancelDate">Date when the Subscriptions should end (can be empty)</param>
        /// <returns>ArrayList holding Addresses that were Expired
        /// </returns>
        public ArrayList ExpireAllCurrentAddresses(DateTime ACancelDate)
        {
            ArrayList ReturnValue;
            DataView CurrentAddresses;
            int Counter;
            PPartnerLocationRow PartnerLocationRow;

            PPartnerLocationRow[] PartnerLocationRows;
            PLocationRow LocationRow;
            int Counter2;
            ReturnValue = new ArrayList();
            CurrentAddresses = Calculations.DetermineCurrentAddresses(FMainDS.PPartnerLocation);
            PartnerLocationRows = new PPartnerLocationRow[CurrentAddresses.Count];

            for (Counter = 0; Counter <= CurrentAddresses.Count - 1; Counter += 1)
            {
                PartnerLocationRows[Counter] = (PPartnerLocationRow)CurrentAddresses[Counter].Row;
            }

            for (Counter2 = 0; Counter2 <= PartnerLocationRows.Length - 1; Counter2 += 1)
            {
                PartnerLocationRow = PartnerLocationRows[Counter2];
                LocationRow = (PLocationRow)FMainDS.PLocation.Rows.Find(new object[] { PartnerLocationRow.SiteKey, PartnerLocationRow.LocationKey });

                if (PartnerLocationRow.IsDateGoodUntilNull())
                {
                    if ((PartnerLocationRow.IsDateEffectiveNull()) || (PartnerLocationRow.DateEffective <= ACancelDate))
                    {
                        PartnerLocationRow.BeginEdit();
                        PartnerLocationRow.DateGoodUntil = ACancelDate;
                        PartnerLocationRow.EndEdit();
                        ReturnValue.Add(Calculations.DetermineLocationString(LocationRow,
                                Calculations.TPartnerLocationFormatEnum.plfCommaSeparated));
                    }
                    else
                    {
                        MessageBox.Show(String.Format(StrAddressCannotBeExpired, Calculations.DetermineLocationString(
                                    LocationRow, Calculations.TPartnerLocationFormatEnum.plfCommaSeparated)),
                            StrAddressCannotBeExpiredTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Ends the editing of the currently selected record.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void ProcessEditedRecord(out PartnerEditTDSPPartnerLocationRow APartnerLocationRow)
        {
            PLocationRow LocationRow;
            PLocationRow ActualLocationRow;

            DataRow[] FoundPartnerLocationRows;
            DataRow[] SendMailDataRows;

            // MessageBox.Show('FIsRecordBeingAdded: ' + FIsRecordBeingAdded.ToString + '; FRecordBeingAddedIsFoundRecord: ' + FRecordBeingAddedIsFoundRecord.ToString);
            if ((FIsRecordBeingAdded) && (!FRecordBeingAddedIsFoundRecord))
            {
                APartnerLocationRow = (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.Rows.Find(
                    new object[] { FMainDS.PPartner[0].PartnerKey, FCurrentSiteKey, FClientSideNewDataRowKey });
                LocationRow = (PLocationRow)FMainDS.PLocation.Rows.Find(new object[] { FCurrentSiteKey, FClientSideNewDataRowKey });

                // MessageBox.Show('ProcessEditedRecord:  FClientSideNewDataRowKey: ' + FClientSideNewDataRowKey.ToString);
            }
            else
            {
                // MessageBox.Show('ProcessEditedRecord: FCurrentLocationKey: ' + FCurrentLocationKey.ToString +'; FCurrentSiteKey: ' + FCurrentSiteKey.ToString);
                if (FRecordKeyBeforeFinding == null)
                {
                    APartnerLocationRow = (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.Rows.Find(
                        new object[] { FMainDS.PPartner[0].PartnerKey, FCurrentSiteKey, FCurrentLocationKey });
                }
                else
                {
                    // MessageBox.Show(PPartnerLocationTable.GetSiteKeyDBName() + ' = ' +
                    // FRecordKeyBeforeFinding.SiteKey.ToString + ' AND ' +
                    // PPartnerLocationTable.GetLocationKeyDBName() + ' = ' +
                    // FRecordKeyBeforeFinding.LocationKey.ToString);
                    FoundPartnerLocationRows = FMainDS.PPartnerLocation.Select(
                        PPartnerLocationTable.GetSiteKeyDBName() + " = " + FRecordKeyBeforeFinding.SiteKey.ToString() + " AND " +
                        PPartnerLocationTable.GetLocationKeyDBName() + " = " + FRecordKeyBeforeFinding.LocationKey.ToString());

                    if (FoundPartnerLocationRows.Length != 0)
                    {
                        APartnerLocationRow = (PartnerEditTDSPPartnerLocationRow)FoundPartnerLocationRows[0];
                    }
                    else
                    {
                        APartnerLocationRow = (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.Rows.Find(
                            new object[] { FMainDS.PPartner[0].PartnerKey, FCurrentSiteKey, FCurrentLocationKey });
                    }
                }

                LocationRow = LocationDataRowOfCurrentlySelectedRecord;
            }

            // if LocationRow = nil then
            // begin
            // MessageBox.Show('LocationRow = nil');
            // end;
            // if APartnerLocationRow = nil then
            // begin
            // MessageBox.Show('APartnerLocationRow = nil');
            // end;
            // MessageBox.Show('Before EndEdit:' + "\r\n" + 'LocationRow RowState: ' + (Enum.GetName(typeof(DataRowState),
            // LocationRow.RowState)) + "\r\n" +
            // 'APartnerLocationRow RowState: ' + (Enum.GetName(typeof(DataRowState),
            // APartnerLocationRow.RowState)));
            // Call EndEdit on all DataRows that are editable in the Detail UserControl.
            // THIS IS ESSENTIAL!!! It makes the changes that the user has made permanent.
            if (FLocationRowAfterCopying == null)
            {
                LocationRow.EndEdit();
            }
            else
            {
                // Location got copied  make it unchanged to prevent it from being saved
                LocationRow.AcceptChanges();

                // ...but: if the user has changed the Location after copying we need to save it!
                if (!DataUtilities.HaveDataRowsIdenticalValues(FLocationRowAfterCopying, LocationRow))
                {
                    // User has changed the Location after copying

                    /*
                     * Tricky bit: at this stage we have got a DataRow that is *Added* (the copy function does that)
                     * and it is modified by the user. However, to get the
                     * PetraServer to not save it as a new Address, we need to make
                     * it unchanged and then change it again so that the PetraServer
                     * sees a 'Modified' DataRow and acts appropriately (eg. look
                     * for duplicates, offer the user to change other Partner's
                     * addresses too if applicable)!!!
                     */

                    // (1) preserve values of the relevant datacolums (one or more of which the user has changed)
                    ActualLocationRow = FMainDS.PLocation.NewRowTyped(false);
                    TAddressHandling.CopyLocationData(LocationRow, ActualLocationRow);
                    LocationRow.BeginEdit();

                    // (2) overwrite current values of relevant datacolums with values after copying
                    TAddressHandling.CopyLocationData(FLocationRowAfterCopying, LocationRow);
                    LocationRow.EndEdit();

                    if (!FIsRecordBeingAdded)
                    {
                        // (3) make the DataRow 'unchanged' instead of 'added'
                        LocationRow.AcceptChanges();
                        LocationRow.BeginEdit();

                        // (4) overwrite relevant datacolums with the values that the user entered
                        TAddressHandling.CopyLocationData(ActualLocationRow, LocationRow);

                        // (5) After executing the following command, we have a DataRow with
                        // state 'Modified' instead of 'Added'  with the same content than
                        // the 'Added' DataRow had before!
                        LocationRow.EndEdit();
                    }

                    // MessageBox.Show('not HaveDataRowsIdenticalValues: After EndEdit:' + "\r\n" + 'LocationRow RowState: ' + (Enum.GetName(typeof(DataRowState),
                    // LocationRow.RowState)));
                }

                FLocationRowAfterCopying = null;
            }

            APartnerLocationRow.EndEdit();

            // MessageBox.Show('After EndEdit:' + "\r\n" + 'LocationRow RowState: ' + (Enum.GetName(typeof(DataRowState),
            // LocationRow.RowState)) + "\r\n" +
            // 'APartnerLocationRow RowState: ' + (Enum.GetName(typeof(DataRowState),
            // APartnerLocationRow.RowState)));
            if ((APartnerLocationRow.DateEffective != FValidFromBeforeEditing)
                || (APartnerLocationRow.DateGoodUntil != FValidUntilBeforeEditing) || (APartnerLocationRow.SendMail != FSendMailBeforeEditing))
            {
                // MessageBox.Show('PartnerLocation Date(s) or Mailing Address CheckBox have changed!');
                FValidFromBeforeEditing = APartnerLocationRow.DateEffective;
                FValidUntilBeforeEditing = APartnerLocationRow.DateGoodUntil;

                // Determination of the Grid icons and the 'Best Address' (these calls change certain columns in some rows!)
                Calculations.DeterminePartnerLocationsDateStatus((DataSet)FMainDS);
                Calculations.DetermineBestAddress((DataSet)FMainDS);
            }

            // Perform check whether another Address has already the SendMail flag set
            if ((APartnerLocationRow.SendMail != FSendMailBeforeEditing) && (APartnerLocationRow.SendMail == true))
            {
                SendMailDataRows = FMainDS.PPartnerLocation.Select(
                    PPartnerLocationTable.GetSendMailDBName() + " = true" + " AND " + PPartnerLocationTable.GetLocationKeyDBName() + " <> " +
                    APartnerLocationRow.LocationKey.ToString());

                if (SendMailDataRows.Length != 0)
                {
                    MessageBox.Show(StrAnotherAddressIsMailingAddress,
                        StrAnotherAddressIsMailingAddressTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }

            // New Address?
            if ((APartnerLocationRow.RowState == DataRowState.Added) && FIsRecordBeingAdded)
            {
                FIsRecordBeingAdded = false;

                if (FRecordBeingAddedIsFoundRecord)
                {
                    FRecordBeingAddedIsFoundRecord = false;
                }
                else
                {
                    FCurrentLocationKey = FClientSideNewDataRowKey;
                }

                if (FMainDS.PPartner[0].StatusCode != SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE))
                {
                    // Business Rule: if a new Address is added and the Partner's StatusCode
                    // isn't ACTIVE, set it to ACTIVE automatically.
                    MessageBox.Show(String.Format(MCommonResourcestrings.StrPartnerStatusChange + StrPartnerReActivationBecauseOfNewAddr,
                            FMainDS.PPartner[0].StatusCode,
                            SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE)),
                        MCommonResourcestrings.StrPartnerReActivationTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FMainDS.PPartner[0].StatusCode = SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE);
                }
            }

            FRecordKeyBeforeFinding = null;

            // A Partner is seen as beeing modified if an Address has been changed or
            // added > set Partner's DateModified
            FMainDS.PPartner[0].DateModified = DateTime.Today;
        }

        /// <summary>
        /// Performs necessary actions after the Merging of rows that were changed on
        /// the Server side into the Client-side DataSet.
        ///
        /// Determination of the Grid icons and the 'Best Address' (this changes
        /// certain DataColumns in some DataRows!)
        ///
        /// </summary>
        /// <returns>void</returns>
        public void RefreshRecordsAfterMerge()
        {
            Calculations.DeterminePartnerLocationsDateStatus((DataSet)FMainDS);
            Calculations.DetermineBestAddress((DataSet)FMainDS);
        }

        /// <summary>
        /// Adds a new record (based on either the selected record or a found record),
        ///
        /// Note: a new record actually consists of two new records (one for the
        /// p_location table and one for the p_partner_location table).
        ///
        /// </summary>
        /// <param name="ANewFromSiteKey">The SiteKey of the currently selected record</param>
        /// <param name="ANewFromLocationKey">The LocationKey of the currently selected record</param>
        /// <param name="AFoundNewLocationKey">Must be 0, except if a new record has been found
        /// by the user and the data of this record should be copied over into the new
        /// record.</param>
        /// <returns>Key of new record.
        /// </returns>
        public Int32 AddRecord(Int64 ANewFromSiteKey, Int32 ANewFromLocationKey, Int32 AFoundNewLocationKey)
        {
            Int32 ReturnValue;
            PLocationRow NewLocationRecord;
            PartnerEditTDSPPartnerLocationRow NewPartnerLocationRecord;
            DataRow PartnerLocationRecordZero;
            DataRow LocationRecordZero;

            FNewRecordFromRecordKey = ANewFromLocationKey;

            // New records use a unique, internal, fictive negative DataTable Key on the
            // Client side  until the record is saved by the PetraServer and gets a
            // proper Key value.
            FClientSideNewDataRowKey = FClientSideNewDataRowKey - 1;

            // TODO: use s_system_parameter.s_site_key_n once p_partner_location actually uses the Petra System SiteKey in the PrimaryKey (instead of 0, which is used currently)
            FCurrentSiteKey = SharedConstants.FIXED_SITE_KEY;
            FIsRecordBeingAdded = true;

            if (AFoundNewLocationKey == 0)
            {
                /*
                 * Create new record by copying over just the PartnerLocation data from the
                 * currently selected record.
                 */
                TAddressHandling.CreateNewAddress(FMainDS.PLocation,
                    FMainDS.PPartnerLocation,
                    FMainDS.PPartner[0].PartnerKey,
                    SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass),
                    FCurrentDataRow.CountryCode,
                    FClientSideNewDataRowKey,
                    FCurrentLocationKey,
                    ANewFromSiteKey);
                NewPartnerLocationRecord = (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.Rows.Find(
                    new object[] { FMainDS.PPartner[0].PartnerKey, FCurrentSiteKey, FClientSideNewDataRowKey });
            }
            else
            {
                /*
                 * Create new record by copying over most of the data from the found
                 * Location as well as the PartnerLocation data from the currently selected
                 * record. The data of the selected Location itself gets deleted after
                 * that!!!
                 */
                TAddressHandling.CreateNewAddress(FMainDS.PLocation,
                    FMainDS.PPartnerLocation,
                    FMainDS.PPartner[0].PartnerKey,
                    SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass),
                    "",
                    FClientSideNewDataRowKey,
                    FCurrentLocationKey,
                    FCurrentSiteKey,
                    FMainDS.PPartner[0].PartnerKey,
                    AFoundNewLocationKey,
                    FCurrentSiteKey,
                    true,
                    true);
                NewPartnerLocationRecord = (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.Rows.Find(
                    new object[] { FMainDS.PPartner[0].PartnerKey, FCurrentSiteKey, FClientSideNewDataRowKey });
                NewLocationRecord = (PLocationRow)FMainDS.PLocation.Rows.Find(new object[] { FCurrentSiteKey, FClientSideNewDataRowKey });
                NewPartnerLocationRecord.LocationKey = AFoundNewLocationKey;
                NewLocationRecord.LocationKey = AFoundNewLocationKey;
                NewLocationRecord.AcceptChanges();
                FCurrentLocationKey = AFoundNewLocationKey;
                FRecordBeingAddedIsFoundRecord = true;
            }

            // Set this Address to be a current Address that isn't the 'Best Address'
            NewPartnerLocationRecord.Icon = 1;
            NewPartnerLocationRecord.BestAddress = false;

            // Check if record with PartnerLocation.LocationKey = 0 is around > delete it
            PartnerLocationRecordZero =
                FMainDS.PPartnerLocation.Rows.Find(new object[] { FMainDS.PPartner[0].PartnerKey, SharedConstants.FIXED_SITE_KEY,
                                                                  0 });

            if (PartnerLocationRecordZero != null)
            {
                LocationRecordZero = FMainDS.PLocation.Rows.Find(new object[] { SharedConstants.FIXED_SITE_KEY, 0 });
                FMainDS.PLocation.Rows.Remove(LocationRecordZero);
                PartnerLocationRecordZero.Delete();

                // LocationRecordZero.AcceptChanges();
                // PartnerLocationRecordZero.AcceptChanges();
                // MessageBox.Show('Deleted Location 0');
            }

            if (AFoundNewLocationKey == 0)
            {
                ReturnValue = FClientSideNewDataRowKey;
            }
            else
            {
                ReturnValue = AFoundNewLocationKey;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether the Partner has an Address with the specified PK.
        ///
        /// @comment Displays an error message to the user if it will return false!
        ///
        /// </summary>
        /// <param name="ALocationPK">PK of a DataRow in the PLocation table</param>
        /// <returns>true if the Address was found, otherwise false.
        /// </returns>
        public Boolean IsAddressRowPresent(TLocationPK ALocationPK)
        {
            Boolean ReturnValue;
            PLocationRow DuplicateLocationRow;

            DuplicateLocationRow = (PLocationRow)FMainDS.PLocation.Rows.Find(new object[] { ALocationPK.SiteKey, ALocationPK.LocationKey });

            if (DuplicateLocationRow == null)
            {
                ReturnValue = false;
            }
            else
            {
                MessageBox.Show(StrFoundAddressIsDuplicate, StrFoundAddressIsDuplicateTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Adds one LocationKey to the FCleanupAddressesLocationKeys array.
        ///
        /// </summary>
        /// <param name="ALocationKey">Location Key that is to be added
        /// </param>
        /// <returns>void</returns>
        public void AddCleanupAddressesLocationKey(Int32 ALocationKey)
        {
            // MessageBox.Show('Current Length of FCleanupAddressesLocationKeys: ' + Integer(Length(FCleanupAddressesLocationKeys)).ToString);
            FCleanupAddressesLocationKeys = new int[FCleanupAddressesLocationKeys.Length + 1];

            // MessageBox.Show('Adding LocationKey ' + ALocationKey.ToString + ' to FCleanupAddressesLocationKeys[' + Integer(Integer(Length(FCleanupAddressesLocationKeys))  1).ToString + ']');
            FCleanupAddressesLocationKeys[FCleanupAddressesLocationKeys.Length - 1] = ALocationKey;
        }

        /// <summary>
        /// Adds a 'default' record to each of the DataTables.
        ///
        /// This makes it possible to have a Grid that always has at least 1 record (i.e.
        /// never has zero records).
        ///
        /// </summary>
        /// <returns>void</returns>
        public void AddDefaultRecord()
        {
            PLocationRow NewLocationsRow;
            PartnerEditTDSPPartnerLocationRow NewPartnerLocationRow;

            DataRow[] DeletedPartnerLocation0Row;
            FCurrentLocationKey = 0;
            DeletedPartnerLocation0Row = FMainDS.PLocation.Select(PLocationTable.GetLocationKeyDBName() + " = 0", "", DataViewRowState.Deleted);

            if (DeletedPartnerLocation0Row.Length == 0)
            {
                // Add new Locations row
                NewLocationsRow = (PLocationRow)FMainDS.PLocation.NewRow();

                // TODO: use s_system_parameter.s_site_key_n once p_partner_location actually uses the Petra System SiteKey in the PrimaryKey (instead of 0, which is used currently)
                NewLocationsRow.SiteKey = SharedConstants.FIXED_SITE_KEY;
                NewLocationsRow.LocationKey = 0;
                NewLocationsRow.Locality = "";

                // TODO 1 oChristianK cData : Fetch values from p_location record 0 and use these instead of the following hardcoded values!
                NewLocationsRow.StreetName = "No Valid Address On File";
                NewLocationsRow.Address3 = "";
                NewLocationsRow.PostalCode = "";
                NewLocationsRow.City = "";
                NewLocationsRow.County = "";
                NewLocationsRow.CountryCode = "99";
                FMainDS.PLocation.Rows.Add(NewLocationsRow);

                // the location is already in the database
                NewLocationsRow.AcceptChanges();

                // Add new PartnerLocations row
                NewPartnerLocationRow = (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.NewRow();

                if (FJustDeletedPartnerLocationsRow != null)
                {
                    NewPartnerLocationRow.ItemArray = FJustDeletedPartnerLocationsRow.ItemArray;
                }
                else
                {
                    // MessageBox.Show('FJustDeletedPartnerLocationsRow <> nil');
                    NewPartnerLocationRow.LocationType =
                        TSharedAddressHandling.GetDefaultLocationType(SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass));
                }

                NewPartnerLocationRow.LocationKey = 0;

                // TODO: use s_system_parameter.s_site_key_n once p_partner_location actually uses the Petra System SiteKey in the PrimaryKey (instead of 0, which is used currently)
                NewPartnerLocationRow.SiteKey = SharedConstants.FIXED_SITE_KEY;
                NewPartnerLocationRow.PartnerKey = FMainDS.PPartner[0].PartnerKey;
                NewPartnerLocationRow.SendMail = false;
                NewPartnerLocationRow.Icon = 1;
                NewPartnerLocationRow.BestAddress = false;
                FMainDS.PPartnerLocation.Rows.Add(NewPartnerLocationRow);

                /* MessageBox.Show('FMainDS.PPartnerLocation.Rows.Count: ' + FMainDS.PPartnerLocation.Rows.Count.ToString + '; FMainDS.PPartnerLocation.Row[1].LocationKey: ' + FMainDS.PPartnerLocation.Row[1].LocationKey.ToString + ';
                 *FMainDS.PPartnerLocation.Row[1].LocationType: ' + FMainDS.PPartnerLocation.Row[1].LocationType); */

                // MessageBox.Show('NewLocationsRow: Proposed? ' + NewLocationsRow.HasVersion(DataRowVersion.Proposed).ToString +
                // 'NewPartnerLocationRow: Proposed? ' + NewPartnerLocationRow.HasVersion(DataRowVersion.Proposed).ToString);
                // Make rows unchanged
                // NewLocationsRow.AcceptChanges();
                // NewPartnerLocationRow.AcceptChanges();
            }
            else
            {
                DeletedPartnerLocation0Row[0].RejectChanges();

                // MessageBox.Show('UnDeleted PLocation row with LocationKey 0.');
                DeletedPartnerLocation0Row = FMainDS.PPartnerLocation.Select(
                    PPartnerLocationTable.GetPartnerKeyDBName() + " = " + FMainDS.PPartner[0].PartnerKey.ToString() + " AND " +
                    PPartnerLocationTable.GetLocationKeyDBName() + " = 0", "", DataViewRowState.Deleted);

                if (DeletedPartnerLocation0Row.Length != 0)
                {
                    // Delete PPartnerLocation row
                    DeletedPartnerLocation0Row[0].RejectChanges();

                    // MessageBox.Show('UnDeleted PPartnerLocation row with LocationKey 0.');
                }
            }

            //
            // Make sure there is no deleted Default Record left...
            //
            // DeletedPartnerLocation0Row := FMainDS.PLocation.Select(
            // PLocationTable.GetLocationKeyDBName() + ' = 0',
            // '', DataViewRowState.Deleted);
            //
            // if Length(DeletedPartnerLocation0Row) <> 0 then
            // begin
            // Delete PLocation row
            // DeletedPartnerLocation0Row[0].AcceptChanges();
            // MessageBox.Show('Deleted PLocation row with LocationKey 0.');
            // end;
            //
            // DeletedPartnerLocation0Row = new SomeType[0];
            //
            // DeletedPartnerLocation0Row := FMainDS.PPartnerLocation.Select(
            // PPartnerLocationTable.GetPartnerKeyDBName() + ' = ' +
            // FMainDS.PPartner[0].PartnerKey.ToString + ' AND ' +
            // PPartnerLocationTable.GetLocationKeyDBName() + ' = 0',
            // '', DataViewRowState.Deleted);
            //
            // if Length(DeletedPartnerLocation0Row) <> 0 then
            // begin
            // Delete PPartnerLocation row
            // DeletedPartnerLocation0Row[0].AcceptChanges();
            // MessageBox.Show('Deleted PPartnerLocation row with LocationKey 0.');
            // end;
        }

        /// <summary>
        /// Deletes the currently selected record.
        ///
        /// Displays a MessageBox to the user where he/she needs to choose 'Yes' or 'No'.
        /// The record is only deleted when 'Yes' is chosen and security checks were OK.
        ///
        /// Note: a record actually consists of two records (one for the p_location table
        /// and one for the p_partner_location table).
        ///
        /// </summary>
        /// <returns>void</returns>
        public Boolean DeleteRecord()
        {
            Boolean ReturnValue;
            DialogResult Chosen;
            DataRow PartnerLocationRow;
            DataRow LocationRow;
            Boolean SharedLocationPartnerLocation;
            String DeleteQuestion;
            Boolean DeleteSecurityOK;

            // Find records in both DataTables that correspond with the current key; those will be deleted.
            LocationRow = FMainDS.PLocation.Rows.Find(new object[] { FCurrentSiteKey, FCurrentLocationKey });
            PartnerLocationRow = FMainDS.PPartnerLocation.Rows.Find(new object[] { FMainDS.PPartner[0].PartnerKey, FCurrentSiteKey,
                                                                                   FCurrentLocationKey });

            /*
             * Check security
             */
            DeleteSecurityOK = CheckDeleteSecurityGeneral(true);

            if (DeleteSecurityOK)
            {
                /*
                 * Determine the Text of the question that is shown to the user
                 */
                SharedLocationPartnerLocation =
                    FPartnerEditUIConnector.HasPartnerLocationOtherPartnerReferences(((PLocationRow)LocationRow).SiteKey,
                        (Int32)((PLocationRow)LocationRow).LocationKey);

                if (SharedLocationPartnerLocation)
                {
                    DeleteQuestion = StrDeleteQuestionLine1 + "\r\n" + StrDeleteQuestionShared;
                }
                else
                {
                    // Check security if Location is not shared (needs to be deletable then)
                    if (!CheckDeleteSecurityNonSharedLocation(true))
                    {
                        return false;
                    }

                    DeleteQuestion = StrDeleteQuestionLine1 + "\r\n" + StrDeleteQuestionNotShared;
                }

                Chosen = MessageBox.Show(DeleteQuestion,
                    StrDeleteQuestionTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                /*
                 * Delete records
                 */
                if (Chosen == DialogResult.Yes)
                {
                    // Keep a copy of the current PartnerLocation record in case the
                    // deleted record was the last one and a new default record needs to be
                    // created afterwards
                    FJustDeletedPartnerLocationsRow = FMainDS.PPartnerLocation.NewRow();
                    FJustDeletedPartnerLocationsRow.ItemArray = PartnerLocationRow.ItemArray;
                    LocationRow.Delete();
                    PartnerLocationRow.Delete();

                    // Determination of the Grid icons and the 'Best Address' (these calls change certain columns in some rows!)
                    Calculations.DeterminePartnerLocationsDateStatus((DataSet)FMainDS);
                    Calculations.DetermineBestAddress((DataSet)FMainDS);
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

        #endregion

        #region Helper functions

        /// <summary>
        /// Determines the record that should be initially be the selected record.
        ///
        /// </summary>
        /// <param name="ARowNumber">Grid row number for the initially selected record</param>
        /// <param name="ALocationKey">DataTable Key value for the initially selected record</param>
        /// <param name="ASiteKey">DataTable Key value for the initially selected record
        /// </param>
        /// <returns>void</returns>
        public void DetermineInitiallySelectedRecord(out Int32 ARowNumber, out System.Int32 ALocationKey, out System.Int64 ASiteKey)
        {
            System.Int16 CurrentRow;
            Boolean FoundSelectedLocation;
            ASiteKey = FMainDS.MiscellaneousData[0].SelectedSiteKey;
            ALocationKey = FMainDS.MiscellaneousData[0].SelectedLocationKey;
            ARowNumber = 0;
            FDataGridDV = (FDataGrid.DataSource as DevAge.ComponentModel.BoundDataView).DataView;
            FoundSelectedLocation = false;

//            MessageBox.Show("ASiteKey: " + ASiteKey.ToString() + "; ALocationKey: " + ALocationKey.ToString());
            for (CurrentRow = 0; CurrentRow <= FDataGridDV.Count - 1; CurrentRow += 1)
            {
                ARowNumber = ARowNumber + 1;

                if ((Convert.ToInt64(FDataGridDV[CurrentRow].Row[PPartnerLocationTable.GetSiteKeyDBName()]) == ASiteKey)
                    && (Convert.ToInt32(FDataGridDV[CurrentRow].Row[PPartnerLocationTable.GetLocationKeyDBName()]) == ALocationKey))
                {
                    FoundSelectedLocation = true;
                    break;
                }
            }

            if (!FoundSelectedLocation)
            {
                ARowNumber = 1;
            }
        }

        /// <summary>
        /// Determines the row that needs to be selected in the Grid (depending on
        /// whether its an existing or a new row).
        ///
        /// </summary>
        /// <param name="ADataView">DataView that the Grid is DataBound to</param>
        /// <returns>DataRowView containing the row that needs to be selected
        /// </returns>
        public DataRowView DetermineRecordToSelect(DataView ADataView)
        {
            DataRowView TmpDataRowView;

            // MessageBox.Show('DetermineRecordToSelect:  FCurrentLocationKey: ' + FCurrentLocationKey.ToString);
            if ((FIsRecordBeingAdded) && (!FRecordBeingAddedIsFoundRecord))
            {
                // MessageBox.Show('DetermineRecordToSelect:  FIsRecordBeingAdded: ' + FIsRecordBeingAdded.ToString);
                ADataView.RowFilter = PLocationTable.GetLocationKeyDBName() + " = " + FClientSideNewDataRowKey.ToString() + " AND " +
                                      PLocationTable.GetSiteKeyDBName() + " = " + FCurrentSiteKey.ToString();
            }
            else
            {
                ADataView.RowFilter = PLocationTable.GetLocationKeyDBName() + " = " + FCurrentLocationKey.ToString() + " AND " +
                                      PLocationTable.GetSiteKeyDBName() + " = " + FCurrentSiteKey.ToString();
            }

            // MessageBox.Show('ADataView.RowFilter: ' + ADataView.RowFilter.ToString);
            TmpDataRowView = ADataView[0];
            ADataView.RowFilter = "";

            // MessageBox.Show(TmpDataRowView['p_location_key_i'].ToString);
            return TmpDataRowView;
        }

        /// <summary>
        /// Determines the DataTable Key of the currently selected record.
        ///
        /// Besides returning the DataTable Key, this also sets important internal
        /// variables!
        ///
        /// </summary>
        /// <param name="ARow">Grid row of the currently selected record.</param>
        /// <returns>DataTable Key of the currently selected record.
        /// </returns>
        public Int32 DetermineCurrentKey(Int32 ARow)
        {
            DataRowView TheDataRowView;

//TLogging.Log("DetermineCurrentKey:  ARow: " + ARow.ToString());
            if (FDataGrid != null)
            {
                TheDataRowView = (DataRowView)FDataGrid.Rows.IndexToDataSourceRow(ARow);
                FCurrentDataRow = (PLocationRow)TheDataRowView.Row;
            }
            else
            {
                // ONLY needed for Unit Testing: we don't have access to the DataGrid then!
                FCurrentDataRow = FMainDS.PLocation[ARow];
            }

            FCurrentLocationKey = (Int32)FCurrentDataRow.LocationKey;
            FCurrentSiteKey = FCurrentDataRow.SiteKey;

//TLogging.Log("DetermineCurrentKey:  FCurrentLocationKey: " + FCurrentLocationKey.ToString());
            return FCurrentLocationKey;
        }

        /// <summary>
        /// Loads Data from the PetraServer into FMainDS.
        ///
        /// Data comes from the p_location and p_partner_location tables in the DB,
        ///
        /// </summary>
        /// <returns>true if successful, otherwise false.
        /// </returns>
        public Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            // Load data if not already loaded
            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PLocation == null)
                {
                    FMainDS.Tables.Add(new PLocationTable());
                    FMainDS.Tables.Add(new PartnerEditTDSPPartnerLocationTable());
                    FMainDS.InitVars();
                }

                if (FMainDS.PLocation.Rows.Count == 0)
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataAddresses());
                    FMainDS.EnableRelations();

                    // Determination of the Grid icons and the 'Best Address' (these calls change certain columns in some rows!)
                    Calculations.DeterminePartnerLocationsDateStatus((DataSet)FMainDS);
                    Calculations.DetermineBestAddress((DataSet)FMainDS);
                    FMainDS.PLocation.AcceptChanges();
                    FMainDS.PPartnerLocation.AcceptChanges();
                }

                if (FMainDS.PLocation.Rows.Count != 0)
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

        #endregion

        #region Callback procedures used for Icon column

        /// <summary>
        /// Determines the Address Kind Icon (displayed in the first Column of the Grid).
        ///
        /// </summary>
        /// <param name="ARow">Grid Row</param>
        /// <returns>Address Kind Icon
        /// </returns>
        public System.Drawing.Image GetAddressKindIconForGridRow(int ARow)
        {
            PLocationRow CurrentGridDataRow;

            System.Int32 LocationKey;
            System.Int64 SiteKey;
            PartnerEditTDSPPartnerLocationRow PartnerLocationRow;
            Int32 ImageIndex;

            // Find record in second DataTable that correspond with the current key
            CurrentGridDataRow = (PLocationRow)FDataGridDV[ARow].Row;

            if (CurrentGridDataRow.HasVersion(DataRowVersion.Original))
            {
                // MessageBox.Show('GetAddressKindIconForGridRow: DataRowVersion.Original');
                LocationKey = Convert.ToInt32(CurrentGridDataRow[PLocationTable.GetLocationKeyDBName(), DataRowVersion.Original]);
                SiteKey = Convert.ToInt64(CurrentGridDataRow[PLocationTable.GetSiteKeyDBName(), DataRowVersion.Original]);
            }
            else
            {
                LocationKey = (Int32)CurrentGridDataRow.LocationKey;
                SiteKey = CurrentGridDataRow.SiteKey;
            }

            // MessageBox.Show('Row: ' + ARow.ToString + '; p_location_key_i: ' + LocationKey.ToString + '; p_site_key_n: ' + SiteKey.ToString);
            PartnerLocationRow =
                (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.Rows.Find(new object[] { FMainDS.PPartner[0].PartnerKey, SiteKey,
                                                                                                     LocationKey });
            ImageIndex = PartnerLocationRow.Icon - 1;

            if (PartnerLocationRow.BestAddress)
            {
                // 3 Icons x two states
                ImageIndex = ImageIndex + (6 / 2);
            }

            return FGridRowIconsImageList.Images[ImageIndex];
        }

        /// <summary>
        /// Determines the Mailing Address Icon (displayed in the second Column of the
        /// Grid).
        ///
        /// </summary>
        /// <param name="ARow">Grid Row</param>
        /// <returns>Mailing Address Icon
        /// </returns>
        public System.Drawing.Image GetMailingAddressIconForGridRow(int ARow)
        {
            System.Drawing.Image ReturnValue;
            PLocationRow CurrentGridDataRow;
            System.Int32 LocationKey;
            System.Int64 SiteKey;
            PartnerEditTDSPPartnerLocationRow PartnerLocationRow;

            // Find record in second DataTable that correspond with the current key
            CurrentGridDataRow = (PLocationRow)FDataGridDV[ARow].Row;

            if (CurrentGridDataRow.HasVersion(DataRowVersion.Original))
            {
                LocationKey = Convert.ToInt32(CurrentGridDataRow[PLocationTable.GetLocationKeyDBName(), DataRowVersion.Original]);
                SiteKey = Convert.ToInt64(CurrentGridDataRow[PLocationTable.GetSiteKeyDBName(), DataRowVersion.Original]);
            }
            else
            {
                LocationKey = (Int32)CurrentGridDataRow.LocationKey;
                SiteKey = CurrentGridDataRow.SiteKey;
            }

            // MessageBox.Show('ARow: ' + ARow.ToString + '; p_location_key_i: ' + LocationKey.ToString);
            PartnerLocationRow =
                (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.Rows.Find(new object[] { FMainDS.PPartner[0].PartnerKey, SiteKey,
                                                                                                     LocationKey });

            if (PartnerLocationRow.SendMail)
            {
                // Mailing Address Icon
                ReturnValue = FGridRowIconsImageList.Images[6];
            }
            else
            {
                // Empty Icon
                ReturnValue = FGridRowIconsImageList.Images[7];
            }

            return ReturnValue;
        }

        /// <summary>
        /// Determines the Text that should be shown as a ToolTip for a certain Grid Row.
        ///
        /// </summary>
        /// <param name="AColumn">IGNORED (but needed because this will be called from a
        /// callback that has this parameter)</param>
        /// <param name="ARow">Grid Row</param>
        /// <returns>ToolTip Text
        /// </returns>
        public String GetToolTipTextForGridRow(Int16 AColumn, Int16 ARow)
        {
            PLocationRow CurrentGridDataRow;
            Int32 IconType;

            System.Int32 LocationKey;
            System.Int64 SiteKey;
            PartnerEditTDSPPartnerLocationRow PartnerLocationRow;
            String TooltipText;

            // Find record in second DataTable that correspond with the current key
            CurrentGridDataRow = (PLocationRow)FDataGridDV[ARow].Row;

            if (CurrentGridDataRow.HasVersion(DataRowVersion.Original))
            {
                LocationKey = Convert.ToInt32(CurrentGridDataRow[PLocationTable.GetLocationKeyDBName(), DataRowVersion.Original]);
                SiteKey = Convert.ToInt64(CurrentGridDataRow[PLocationTable.GetSiteKeyDBName(), DataRowVersion.Original]);
            }
            else
            {
                LocationKey = (Int32)CurrentGridDataRow.LocationKey;
                SiteKey = CurrentGridDataRow.SiteKey;
            }

            // MessageBox.Show('ARow: ' + ARow.ToString + '; p_location_key_i: ' + LocationKey.ToString);
            PartnerLocationRow =
                (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.Rows.Find(new object[] { FMainDS.PPartner[0].PartnerKey, SiteKey,
                                                                                                     LocationKey });
            IconType = PartnerLocationRow.Icon;

            switch (IconType)
            {
                case 1:
                    TooltipText = "Current Address";
                    break;

                case 2:
                    TooltipText = "Address will become active in the future  (Valid from " + StringHelper.DateToLocalizedString(
                    PartnerLocationRow.DateEffective.Value) + ')';
                    break;

                case 3:
                    TooltipText = "Address was active in the past  (Valid until " + StringHelper.DateToLocalizedString(
                    PartnerLocationRow.DateGoodUntil.Value) + ')';
                    break;

                default:
                    TooltipText = "[UNKNOWN]";
                    break;
            }

            if (PartnerLocationRow.BestAddress)
            {
                TooltipText = TooltipText + " " + Catalog.GetString(
                    "'Best' Address - this is the default address OpenPetra will use [eg. for Mailings])");
            }

            return TooltipText;
        }

        #endregion
        #endregion
    }
}