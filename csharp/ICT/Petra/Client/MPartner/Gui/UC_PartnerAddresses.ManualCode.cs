//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2014 by OM International
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
using System.IO;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MCommon.Gui;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_PartnerAddresses
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary>needed as temporary variables between PreDeleteManual and PostDeleteManual methods</summary>
        Boolean FSharedLocationPartnerLocation;
        Int64 DeletedRowSiteKey = -1;
        Int32 DeletedRowLocationKey = -1;

        /// <summary>Array holding LocationKeys of records that need to be cleaned up (deleted) before Merging</summary>
        private Int32[] FCleanupAddressesLocationKeys;
//TODOWB        private Boolean FRecordBeingAddedIsFoundRecord;
//TODOWB        private TLocationPK FRecordKeyBeforeFinding;
//TODOWB        private PLocationRow FLocationRowAfterCopying;
        private int FSelectedRowIndexBeforeSaving = -1;

        /// <summary>Copy of the PartnerLocation record that is being deleted</summary>
        private DataRow FJustDeletedPartnerLocationsRow;

        /// <summary>Current Address Order (used for optimising the number of TabIndex changes of certain Controls)</summary>
        private Int32 FCurrentAddressOrder;
        private Int32 FLastNonChangedAddressFieldTabIndex;

        /// <summary>Holds a reference to an ImageList containing Icons that can be shown in Grid Rows</summary>
        private ImageList FGridRowIconsImageList;

        #region Public Methods

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

        /// <summary>Record that is currently selected (p_location DataTable)</summary>
        public PartnerEditTDSPPartnerLocationRow PartnerLocationDataRowOfCurrentlySelectedAddress
        {
            get
            {
                object[] SelectedRows = grdDetails.SelectedDataRows;

                if (SelectedRows.Length > 0)
                {
                    return (PartnerEditTDSPPartnerLocationRow)((DataRowView)SelectedRows[0]).Row;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
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
        private PLocationRow FindLocationRowWithOriginalKey(TLocationPK ALocationPK)
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

        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// called when combo box value for country code is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CountryCodeChanged(object sender, EventArgs e)
        {
            SetAddressFieldOrder();
        }

        /// <summary>
        /// Sets the TabIndexes of certain address fields depending on the Country's
        /// AddressOrder.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetAddressFieldOrder()
        {
            Int32 AddressOrder = 0;

            try
            {
                AddressOrder = TAddressHandling.GetAddressOrder(cmbLocationCountryCode.GetSelectedString());
            }
            catch (Exception)
            {
            }

            if (AddressOrder == 0)
            {
                // MessageBox.Show('Address Order = 0');
                if (FCurrentAddressOrder != AddressOrder)
                {
                    // MessageBox.Show('Peforming Address Order change');
                    lblLocationCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 1;
                    txtLocationCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 2;
                    lblLocationPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 3;
                    txtLocationPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 4;
                    lblLocationCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 5;
                    txtLocationCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 6;
                    lblLocationCountryCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 7;
                    cmbLocationCountryCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 8;
                    FCurrentAddressOrder = AddressOrder;
                }
            }
            else if (AddressOrder == 1)
            {
                // MessageBox.Show('Address Order = 1');
                if (FCurrentAddressOrder != AddressOrder)
                {
                    // MessageBox.Show('Peforming Address Order change');
                    lblLocationPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 1;
                    txtLocationPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 2;
                    lblLocationCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 3;
                    txtLocationCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 4;
                    lblLocationCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 5;
                    txtLocationCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 6;
                    lblLocationCountryCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 7;
                    cmbLocationCountryCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 8;
                    FCurrentAddressOrder = AddressOrder;
                }
            }
            else
            {
                if (AddressOrder == 2)
                {
                    if (FCurrentAddressOrder != AddressOrder)
                    {
                        // MessageBox.Show('Peforming Address Order change');
                        lblLocationCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 1;
                        txtLocationCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 2;
                        lblLocationCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 3;
                        txtLocationCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 4;
                        lblLocationPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 5;
                        txtLocationPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 6;
                        lblLocationCountryCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 7;
                        cmbLocationCountryCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 8;
                        FCurrentAddressOrder = AddressOrder;
                    }
                }
            }
        }

        /// <summary>
        /// This Procedure will get called from the SaveChanges procedure before it
        /// actually performs any saving operation.
        /// </summary>
        /// <param name="sender">The Object that throws this Event</param>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        private void DataSavingStarted(System.Object sender, System.EventArgs e)
        {
            DataRow TempRow = null;
            PLocationRow LocationRow;

            PartnerEditTDSPPartnerLocationRow DeletedPartnerLocation0Row = null;
            PLocationRow DeletedLocation0Row = null;

            DataRow[] DeletedLocation0Rows;

            // make sure latest screen modifications are saved to FMainDS
            GetDataFromControls();

            FSelectedRowIndexBeforeSaving = grdDetails.GetFirstHighlightedRowIndex();

            foreach (PartnerEditTDSPPartnerLocationRow PartnerLocationRow in FMainDS.PPartnerLocation.Rows)
            {
                // Find the location record and modify data on the client as only modified data records will
                // be sent over to the server for saving.
                if (PartnerLocationRow.RowState == DataRowState.Added)
                {
                    TempRow = FMainDS.PLocation.Rows.Find(new Object[] { PartnerLocationRow.SiteKey, PartnerLocationRow.LocationKey });

                    if (TempRow == null)
                    {
                        LocationRow = FMainDS.PLocation.NewRowTyped();

                        LocationRow.SiteKey = PartnerLocationRow.SiteKey;
                        LocationRow.LocationKey = PartnerLocationRow.LocationKey;
                    }
                    else
                    {
                        LocationRow = (PLocationRow)TempRow;
                    }

                    LocationRow.Locality = PartnerLocationRow.LocationLocality;
                    LocationRow.StreetName = PartnerLocationRow.LocationStreetName;
                    LocationRow.Address3 = PartnerLocationRow.LocationAddress3;
                    LocationRow.City = PartnerLocationRow.LocationCity;
                    LocationRow.County = PartnerLocationRow.LocationCounty;
                    LocationRow.PostalCode = PartnerLocationRow.LocationPostalCode;
                    LocationRow.CountryCode = PartnerLocationRow.LocationCountryCode;

                    if (TempRow == null)
                    {
                        FMainDS.PLocation.Rows.Add(LocationRow);
                    }
                }
                else if (PartnerLocationRow.RowState == DataRowState.Modified)
                {
                    TempRow = FMainDS.PLocation.Rows.Find(new Object[] { PartnerLocationRow.SiteKey, PartnerLocationRow.LocationKey });

                    if (TempRow != null)
                    {
                        LocationRow = (PLocationRow)TempRow;

                        LocationRow.Locality = PartnerLocationRow.LocationLocality;
                        LocationRow.StreetName = PartnerLocationRow.LocationStreetName;
                        LocationRow.Address3 = PartnerLocationRow.LocationAddress3;
                        LocationRow.City = PartnerLocationRow.LocationCity;
                        LocationRow.County = PartnerLocationRow.LocationCounty;
                        LocationRow.PostalCode = PartnerLocationRow.LocationPostalCode;
                        LocationRow.CountryCode = PartnerLocationRow.LocationCountryCode;
                    }
                }
                else if (PartnerLocationRow.RowState == DataRowState.Deleted)
                {
                    // this case is handled by PreDeleteManual and PostDeleteManual methods

                    // make sure that Location Rows with Location Key 0 do not get transferred to server
                    // if they are deleted (only needed for temporary use on client)
                    // (do only remove from collection once loop is finished)
                    if (((int)PartnerLocationRow[2, DataRowVersion.Original]) == 0)
                    {
                        DeletedPartnerLocation0Row = PartnerLocationRow;

                        DeletedLocation0Rows = FMainDS.PLocation.Select(PLocationTable.GetLocationKeyDBName() + " = 0", "", DataViewRowState.Deleted);

                        if (DeletedLocation0Rows.Length != 0)
                        {
                            DeletedLocation0Row = (PLocationRow)DeletedLocation0Rows[0];
                        }
                    }
                }
            }

            if (DeletedPartnerLocation0Row != null)
            {
                FMainDS.PPartnerLocation.Rows.Remove(DeletedPartnerLocation0Row);
            }

            if (DeletedLocation0Row != null)
            {
                FMainDS.PLocation.Rows.Remove(DeletedLocation0Row);
            }


        }

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
            PartnerEditTDSPPartnerLocationRow PartnerLocationRow = null;
            Int32 ImageIndex = 0;

            DataRowView rowView = (DataRowView)grdDetails.Rows.IndexToDataSourceRow(ARow + 1);

            if (rowView != null)
            {
                PartnerLocationRow = (PartnerEditTDSPPartnerLocationRow)(rowView.Row);
            }

            if ((PartnerLocationRow != null)
                && (PartnerLocationRow.RowState != DataRowState.Deleted)
                && (PartnerLocationRow.RowState != DataRowState.Detached))
            {
                ImageIndex = PartnerLocationRow.Icon - 1;

                if (PartnerLocationRow.BestAddress)
                {
                    // 3 Icons x two states
                    ImageIndex = ImageIndex + (6 / 2);
                }
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
            System.Drawing.Image ReturnValue = null;
            PartnerEditTDSPPartnerLocationRow PartnerLocationRow = null;

            DataRowView rowView = (DataRowView)grdDetails.Rows.IndexToDataSourceRow(ARow + 1);

            if (rowView != null)
            {
                PartnerLocationRow = (PartnerEditTDSPPartnerLocationRow)(rowView.Row);
            }

            if ((PartnerLocationRow != null)
                && (PartnerLocationRow.RowState != DataRowState.Deleted)
                && (PartnerLocationRow.RowState != DataRowState.Detached))
            {
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
            }
            else
            {
                ReturnValue = FGridRowIconsImageList.Images[0];
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
            Int32 IconType;
            PartnerEditTDSPPartnerLocationRow PartnerLocationRow = null;
            String TooltipText = "";

            DataRowView rowView = (DataRowView)grdDetails.Rows.IndexToDataSourceRow(ARow + 1);

            if (rowView != null)
            {
                PartnerLocationRow = (PartnerEditTDSPPartnerLocationRow)(rowView.Row);
            }

            if ((PartnerLocationRow != null)
                && (PartnerLocationRow.RowState != DataRowState.Deleted)
                && (PartnerLocationRow.RowState != DataRowState.Detached))
            {
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
            }

            return TooltipText;
        }

        #endregion

        /// <summary>todoComment</summary>
        public void SpecialInitUserControl()
        {
            int RowNumber;
            TLocationPK BestLocationPK;
            Boolean FoundBestLocation = false;
            String ResourceDirectory = TAppSettingsManager.GetValue("Resource.Dir", true);

            // initialize Image List
            FGridRowIconsImageList = new System.Windows.Forms.ImageList();
            FGridRowIconsImageList.ImageSize = new System.Drawing.Size(16, 16);
            FGridRowIconsImageList.Images.Add(TIconCache.IconCache.AddOrGetExistingIcon(ResourceDirectory + Path.DirectorySeparatorChar +
                    "Address.ico", TIconCache.TIconSize.is16by16));
            FGridRowIconsImageList.Images.Add(TIconCache.IconCache.AddOrGetExistingIcon(ResourceDirectory + Path.DirectorySeparatorChar +
                    "Address_Best.ico", TIconCache.TIconSize.is16by16));
            FGridRowIconsImageList.Images.Add(TIconCache.IconCache.AddOrGetExistingIcon(ResourceDirectory + Path.DirectorySeparatorChar +
                    "Address_Future.ico", TIconCache.TIconSize.is16by16));
            FGridRowIconsImageList.Images.Add(TIconCache.IconCache.AddOrGetExistingIcon(ResourceDirectory + Path.DirectorySeparatorChar +
                    "Address_Future_Best.ico", TIconCache.TIconSize.is16by16));
            FGridRowIconsImageList.Images.Add(TIconCache.IconCache.AddOrGetExistingIcon(ResourceDirectory + Path.DirectorySeparatorChar +
                    "Address_Past.ico", TIconCache.TIconSize.is16by16));
            FGridRowIconsImageList.Images.Add(TIconCache.IconCache.AddOrGetExistingIcon(ResourceDirectory + Path.DirectorySeparatorChar +
                    "Address_Past_Best.ico", TIconCache.TIconSize.is16by16));
            FGridRowIconsImageList.Images.Add(TIconCache.IconCache.AddOrGetExistingIcon(ResourceDirectory + Path.DirectorySeparatorChar +
                    "Address_Mailing_Indicator.ico", TIconCache.TIconSize.is16by16));
            FGridRowIconsImageList.Images.Add(TIconCache.IconCache.AddOrGetExistingIcon(ResourceDirectory + Path.DirectorySeparatorChar +
                    "Completeley_Empty.ico", TIconCache.TIconSize.is16by16));
            FGridRowIconsImageList.TransparentColor = System.Drawing.Color.Transparent;

            LoadDataOnDemand();

            // Determination of the Grid icons and the 'Best Address' (these calls change certain columns in some rows!)
            Calculations.DeterminePartnerLocationsDateStatus((DataSet)FMainDS);
            BestLocationPK = Calculations.DetermineBestAddress((DataSet)FMainDS);

            grdDetails.Columns.Clear();
            grdDetails.AddImageColumn(@GetAddressKindIconForGridRow);
            grdDetails.AddImageColumn(@GetMailingAddressIconForGridRow);
            grdDetails.AddTextColumn("Address-1", FMainDS.PPartnerLocation.Columns[PartnerEditTDSPPartnerLocationTable.GetLocationLocalityDBName()]);
            grdDetails.AddTextColumn("Street-2", FMainDS.PPartnerLocation.Columns[PartnerEditTDSPPartnerLocationTable.GetLocationStreetNameDBName()]);
            grdDetails.AddTextColumn("Address-3", FMainDS.PPartnerLocation.Columns[PartnerEditTDSPPartnerLocationTable.GetLocationAddress3DBName()]);
            grdDetails.AddTextColumn("City", FMainDS.PPartnerLocation.Columns[PartnerEditTDSPPartnerLocationTable.GetLocationCityDBName()]);
            grdDetails.AddTextColumn("Location Type", FMainDS.PPartnerLocation.ColumnLocationType);

            grdDetails.ToolTipTextDelegate = @GetToolTipTextForGridRow;

            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpAddresses));

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);

            // Initialise Array
            FCleanupAddressesLocationKeys = new int[0];

            // check if any specific address is to be selected (e.g. when opened from PartnerFind), otherwise select best one
            if ((FMainDS.MiscellaneousData != null)
                && (FMainDS.MiscellaneousData.Count > 0))
            {
                PartnerEditTDSMiscellaneousDataTable MiscellaneousDataDT = (PartnerEditTDSMiscellaneousDataTable)FMainDS.MiscellaneousData;
                PartnerEditTDSMiscellaneousDataRow MiscellaneousDataDR = (PartnerEditTDSMiscellaneousDataRow)MiscellaneousDataDT.Rows[0];

                if (MiscellaneousDataDR.SelectedLocationKey != 0)
                {
                    BestLocationPK.SiteKey = MiscellaneousDataDR.SelectedSiteKey;
                    BestLocationPK.LocationKey = MiscellaneousDataDR.SelectedLocationKey;
                }
            }

            if (grdDetails.Rows.Count > 1)
            {
                FoundBestLocation = false;

                for (RowNumber = 0; RowNumber < grdDetails.DataSource.Count; RowNumber++)
                {
                    if ((Convert.ToInt64((grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[RowNumber][PPartnerLocationTable.
                                                                                                                            GetSiteKeyDBName()]) ==
                         BestLocationPK.SiteKey)
                        && (Convert.ToInt32((grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[RowNumber][PPartnerLocationTable.
                                                                                                                               GetLocationKeyDBName()
                                ]) == BestLocationPK.LocationKey))
                    {
                        FoundBestLocation = true;
                        break;
                    }
                }

                if (FoundBestLocation)
                {
                    grdDetails.SelectRowInGrid(RowNumber + 1);
                    ShowDetails(RowNumber + 1); // do this as for some reason details are not automatically show here at the moment
                }
                else
                {
                    grdDetails.SelectRowInGrid(1);
                    ShowDetails(1); // do this as for some reason details are not automatically show here at the moment
                }
            }

            // initialize address order variables
            FCurrentAddressOrder = 0;
            FLastNonChangedAddressFieldTabIndex = txtLocationAddress3.TabIndex;
            SetAddressFieldOrder();

            ApplySecurity();
        }

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
            // Make sure selected row in grid is reinitialized after save in case 
            // it got replaced during merge process.
            if (FSelectedRowIndexBeforeSaving >= 0)
            {
                grdDetails.SelectRowInGrid(FSelectedRowIndexBeforeSaving);
            }

            Calculations.DeterminePartnerLocationsDateStatus((DataSet)FMainDS);
            Calculations.DetermineBestAddress((DataSet)FMainDS);
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
            if (!FMainDS.Tables.Contains(PartnerEditTDSPPartnerLocationTable.GetTableName()))
            {
                FMainDS.Tables.Add(new PartnerEditTDSPPartnerLocationTable());
            }

            FMainDS.InitVars();

            SpecialInitUserControl();
        }

        /// <summary>
        /// Loads Partner Location Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue = true;
            PLocationTable LocationTable = new PLocationTable();

            // Load Partner Locations, if not already loaded
            try
            {
                // initialization only needed when Partner Record already exists
                if (FPetraUtilsObject.ScreenMode != TScreenMode.smNew)
                {
                    // Make sure that Typed DataTables are already there at Client side
                    if (FMainDS.PPartnerLocation == null)
                    {
                        FMainDS.Tables.Add(new PartnerEditTDSPPartnerLocationTable());
                        FMainDS.InitVars();
                    }

                    if (FMainDS.PLocation == null)
                    {
                        FMainDS.Tables.Add(new PLocationTable());
                        FMainDS.InitVars();
                    }

                    if (TClientSettings.DelayedDataLoading)
                    {
                        FMainDS.Merge(FPartnerEditUIConnector.GetDataPartnerLocations(ref LocationTable));
                        FMainDS.Merge(LocationTable);

                        // Make DataRows unchanged
                        if (FMainDS.PPartnerLocation.Rows.Count > 0)
                        {
                            FMainDS.PPartnerLocation.AcceptChanges();
                        }

                        if (FMainDS.PLocation.Rows.Count > 0)
                        {
                            FMainDS.PLocation.AcceptChanges();
                        }
                    }
                }

                if (FMainDS.PPartnerLocation.Rows.Count != 0)
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

        private void ShowDataManual()
        {
        }

        private void ShowDetailsManual(PPartnerLocationRow ARow)
        {
            // Determine current Location Key and enable/disable buttons
            if ((ARow != null)
                && (ARow.RowState != DataRowState.Deleted))
            {
                if (ARow.LocationKey == 0)
                {
                    pnlDetails.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    pnlDetails.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }

            ApplySecurity();
            SetAddressFieldOrder();
        }

        private void GetDetailDataFromControlsManual(PPartnerLocationRow ARow)
        {
        }

        /// <summary>
        /// adding a new partner relationship record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRecord(System.Object sender, EventArgs e)
        {
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            if (CreateNewPPartnerLocation())
            {
                txtLocationLocality.Focus();
            }

            // Determination of the Grid icons and the 'Best Address' (these calls change certain columns in some rows!)
            Calculations.DeterminePartnerLocationsDateStatus((DataSet)FMainDS);
            Calculations.DetermineBestAddress((DataSet)FMainDS);

            ApplySecurity();
            SetAddressFieldOrder();

            // Fire OnRecalculateScreenParts event: reset counter in tab header
            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }

        /// <summary>
        /// manual code when adding new row
        /// </summary>
        /// <param name="ARow"></param>
        private void NewRowManual(ref PartnerEditTDSPPartnerLocationRow ARow)
        {
            PLocationRow NewLocationRow = FMainDS.PLocation.NewRowTyped(true);
            Int32 LocationKey = -1;

            while (FMainDS.PLocation.Rows.Find(new object[] { ARow.SiteKey, LocationKey }) != null)
            {
                LocationKey = LocationKey - 1;
            }

            NewLocationRow.LocationKey = LocationKey;
            FMainDS.PLocation.Rows.Add(NewLocationRow);

            ARow.PartnerKey = ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey;

            // LocationKey has to be set properly later on server side
            ARow.LocationKey = LocationKey;

            // initialize "valid from" date with today's date
            ARow.DateEffective = DateTime.Today;

            // initialize country code, email address, URL and mobile number from last (currently) selected address
            if ((FPreviouslySelectedDetailRow != null)
                && (FPreviouslySelectedDetailRow.RowState != DataRowState.Detached))
            {
                ARow.LocationCountryCode = FPreviouslySelectedDetailRow.LocationCountryCode;

                ARow.EmailAddress = FPreviouslySelectedDetailRow.EmailAddress;
                ARow.Url = FPreviouslySelectedDetailRow.Url;
                ARow.MobileNumber = FPreviouslySelectedDetailRow.MobileNumber;
            }

            // initialize location type with default value depending on partner class
            ARow.LocationType =
                TSharedAddressHandling.GetDefaultLocationType(SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass));

            // make sure this is initialized as otherwise initial drawing of cell gives problems
            ARow.BestAddress = false;
            ARow.Icon = 1;

            RemoveDefaultRecord();
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(PartnerEditTDSPPartnerLocationRow ARowToDelete, ref string ADeletionQuestion)
        {
            Boolean DeleteSecurityOK;

            /*
             * Check security
             */
            DeleteSecurityOK = CheckDeleteSecurityGeneral(true);

            if (!DeleteSecurityOK)
            {
                return false;
            }

            // Determine the Text of the question that is shown to the user
            FSharedLocationPartnerLocation =
                FPartnerEditUIConnector.HasPartnerLocationOtherPartnerReferences(ARowToDelete.SiteKey,
                    ARowToDelete.LocationKey);
            DeletedRowSiteKey = ARowToDelete.SiteKey;
            DeletedRowLocationKey = ARowToDelete.LocationKey;

            if (FSharedLocationPartnerLocation)
            {
                ADeletionQuestion = Catalog.GetString("Are you sure you want to remove this address from this partner?");
            }
            else
            {
                // Check security if Location is not shared (needs to be deletable then)
                if (!CheckDeleteSecurityNonSharedLocation(true))
                {
                    return false;
                }

                ADeletionQuestion = Catalog.GetString("Are you sure you want to remove this address from the database (it is not used elsewhere)?");
            }

            ADeletionQuestion += String.Format("{0}{0}({1} {2},{0}{3} {4},{0}{5} {6})",
                Environment.NewLine,
                lblLocationStreetName.Text,
                txtLocationStreetName.Text,
                lblLocationCity.Text,
                txtLocationCity.Text,
                lblLocationCountryCode.Text,
                cmbLocationCountryCode.GetSelectedString());

            // Keep a copy of the current PartnerLocation record in case the
            // deleted record was the last one and a new default record needs to be
            // created afterwards
            FJustDeletedPartnerLocationsRow = FMainDS.PPartnerLocation.NewRow();
            FJustDeletedPartnerLocationsRow.ItemArray = ARowToDelete.ItemArray;

            return true;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(PartnerEditTDSPPartnerLocationRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            DataRow LocationRow;

            if (ADeletionPerformed)
            {
                if (!FSharedLocationPartnerLocation)
                {
                    // delete location row if it was the last one
                    LocationRow = FMainDS.PLocation.Rows.Find(new object[] { DeletedRowSiteKey, DeletedRowLocationKey });

                    if (LocationRow != null)
                    {
                        LocationRow.Delete();
                    }
                }

                // reset temporary variables
                DeletedRowSiteKey = -1;
                DeletedRowLocationKey = -1;

                // Determination of the Grid icons and the 'Best Address' (these calls change certain columns in some rows!)
                Calculations.DeterminePartnerLocationsDateStatus((DataSet)FMainDS);
                Calculations.DetermineBestAddress((DataSet)FMainDS);

                if (grdDetails.Rows.Count <= 1)
                {
                    AddDefaultRecord();
                    grdDetails.SelectRowInGrid(1);
                }

                ApplySecurity();
                DoRecalculateScreenParts();
            }
            else
            {
                FJustDeletedPartnerLocationsRow = null;
            }
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        /// <summary>
        /// Adds one LocationKey to the FCleanupAddressesLocationKeys array.
        ///
        /// </summary>
        /// <param name="ALocationKey">Location Key that is to be added
        /// </param>
        /// <returns>void</returns>
        private void AddCleanupAddressesLocationKey(Int32 ALocationKey)
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
        private void AddDefaultRecord()
        {
            PLocationRow Location0Row;
            PartnerEditTDSPPartnerLocationRow PartnerLocation0Row;

            DataRow[] Location0Rows;
            DataRow[] PartnerLocation0Rows;

            Location0Rows = FMainDS.PLocation.Select(PLocationTable.GetLocationKeyDBName() + " = 0", "");

            if (Location0Rows.Length == 0)
            {
                // Add new Locations row
                Location0Row = (PLocationRow)FMainDS.PLocation.NewRow();

                // TODO: use s_system_parameter.s_site_key_n once p_partner_location actually uses the Petra System SiteKey in the PrimaryKey (instead of 0, which is used currently)
                Location0Row.SiteKey = SharedConstants.FIXED_SITE_KEY;
                Location0Row.LocationKey = 0;
                Location0Row.Locality = "";

                // TODO 1 oChristianK cData : Fetch values from p_location record 0 and use these instead of the following hardcoded values!
                Location0Row.StreetName = "No Valid Address On File";
                Location0Row.Address3 = "";
                Location0Row.PostalCode = "";
                Location0Row.City = "";
                Location0Row.County = "";
                Location0Row.CountryCode = "99";
                FMainDS.PLocation.Rows.Add(Location0Row);

                // the location is already in the database
                Location0Row.AcceptChanges();
            }
            else
            {
                Location0Rows[0].RejectChanges();

                Location0Row = (PLocationRow)Location0Rows[0];
            }

            PartnerLocation0Rows = FMainDS.PPartnerLocation.Select(
                PartnerEditTDSPPartnerLocationTable.GetPartnerKeyDBName() + " = " + FMainDS.PPartner[0].PartnerKey.ToString() + " AND " +
                PartnerEditTDSPPartnerLocationTable.GetLocationKeyDBName() + " = 0", "");

            if (PartnerLocation0Rows.Length == 0)
            {
                // Add new PartnerLocations row
                PartnerLocation0Row = (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.NewRow();

                if (FJustDeletedPartnerLocationsRow != null)
                {
                    PartnerLocation0Row.ItemArray = FJustDeletedPartnerLocationsRow.ItemArray;
                }
                else
                {
                    // MessageBox.Show('FJustDeletedPartnerLocationsRow <> nil');
                    PartnerLocation0Row.LocationType =
                        TSharedAddressHandling.GetDefaultLocationType(SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass));
                }

                PartnerLocation0Row.LocationKey = 0;

                // TODO: use s_system_parameter.s_site_key_n once p_partner_location actually uses the Petra System SiteKey in the PrimaryKey (instead of 0, which is used currently)
                PartnerLocation0Row.SiteKey = SharedConstants.FIXED_SITE_KEY;
                PartnerLocation0Row.PartnerKey = FMainDS.PPartner[0].PartnerKey;

                PartnerLocation0Row.SendMail = false;
                PartnerLocation0Row.Icon = 1;
                PartnerLocation0Row.BestAddress = false;

                // initialize values related to location
                PartnerLocation0Row.LocationLocality = Location0Row.Locality;
                PartnerLocation0Row.LocationStreetName = Location0Row.StreetName;
                PartnerLocation0Row.LocationAddress3 = Location0Row.Address3;
                PartnerLocation0Row.LocationPostalCode = Location0Row.PostalCode;
                PartnerLocation0Row.LocationCity = Location0Row.City;
                PartnerLocation0Row.LocationCounty = Location0Row.County;
                PartnerLocation0Row.LocationCountryCode = Location0Row.CountryCode;

                FMainDS.PPartnerLocation.Rows.Add(PartnerLocation0Row);
            }
            else
            {
                PartnerLocation0Rows[0].RejectChanges();
            }
        }

        /// <summary>
        /// Remove 'default' record from PartnerLocationTable
        ///
        /// This is run when the first real location record is created for grid
        ///
        /// </summary>
        /// <returns>void</returns>
        private void RemoveDefaultRecord()
        {
            DataRow[] Location0Row;
            DataRow[] PartnerLocation0Row;

            PartnerLocation0Row = FMainDS.PPartnerLocation.Select(
                PartnerEditTDSPPartnerLocationTable.GetPartnerKeyDBName() + " = " + FMainDS.PPartner[0].PartnerKey.ToString() + " AND " +
                PartnerEditTDSPPartnerLocationTable.GetLocationKeyDBName() + " = 0", "");

            if ((PartnerLocation0Row.Length != 0)
                && (PartnerLocation0Row[0].RowState != DataRowState.Deleted))
            {
                PartnerLocation0Row[0].Delete();
            }

            Location0Row = FMainDS.PLocation.Select(PLocationTable.GetLocationKeyDBName() + " = 0", "");

            if ((Location0Row.Length != 0)
                && (Location0Row[0].RowState != DataRowState.Deleted))
            {
                Location0Row[0].Delete();
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
                                             String.Format(Catalog.GetString(
                                "NOTE: this is used by {0} "), SimilarLocationParametersRow.UsedByNOtherPartners) +
                                             Catalog.GetPluralString(" other partner.",
                            " other partners.",
                            SimilarLocationParametersRow.UsedByNOtherPartners);
                    }
                    else
                    {
                        AlreadyUsedMessage = "";
                    }

                    SimilarLocationDialogResult = MessageBox.Show(
                        Catalog.GetString("A similar address already exists in the database:") + "\r\n" + "    " +
                        SimilarLocationParametersRow.Locality + "\r\n" + "    " +
                        SimilarLocationParametersRow.StreetName + "\r\n" + "    " +
                        SimilarLocationParametersRow.Address3 + "\r\n" + "    " +
                        SimilarLocationParametersRow.City + ' ' + SimilarLocationParametersRow.PostalCode + "\r\n" + "    " +
                        SimilarLocationParametersRow.County + ' ' + SimilarLocationParametersRow.CountryCode + "\r\n" +
                        AlreadyUsedMessage + "\r\n" + "\r\n" +
                        Catalog.GetString("Use the existing address record?") + "\r\n" +
                        Catalog.GetString("(Choose 'No' to create a new address record.)"),
                        Catalog.GetString("Similar Address Exists"),
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

                    AddCleanupAddressesLocationKey((Int32)SimilarLocationParametersRow.LocationKey);
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

            string FilterCriteria;
#if TODO
            DataView PersonsLocationsDV;
#endif
            DataView PartnerSharingLocationDV;
            TFrmPartnerAddressChangePropagationDialog AddressChangedDialog;
            string UserAnswer;
#if TODO
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
                                Catalog.GetString("You have added the following address to this family:") + "\r\n" + "    " +
                                TSaveConvert.StringColumnToString(LocationDT.ColumnLocality,
                                    LocationRow) + "\r\n" + "    " + TSaveConvert.StringColumnToString(LocationDT.ColumnStreetName,
                                    LocationRow) + "\r\n" + "    " + TSaveConvert.StringColumnToString(LocationDT.ColumnAddress3,
                                    LocationRow) + "\r\n" + "    " + TSaveConvert.StringColumnToString(LocationDT.ColumnCity,
                                    LocationRow) + ' ' + TSaveConvert.StringColumnToString(LocationDT.ColumnPostalCode,
                                    LocationRow) + "\r\n" + "    " + TSaveConvert.StringColumnToString(LocationDT.ColumnCounty,
                                    LocationRow) + ' ' + TSaveConvert.StringColumnToString(LocationDT.ColumnCountryCode,
                                    LocationRow) + "\r\n" + "\r\n" +
                                Catalog.GetString("Do you want to add this address to all members\r\nof this family?"),
                                Catalog.GetString("Add Address to Family Members?"),
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
                            FilterCriteria = PartnerAddressAggregateTDSChangePromotionParametersTable.GetSiteKeyDBName() + " = " +
                                             AddressAddedOrChangedPromotionRow.SiteKey.ToString() + " AND " +
                                             PartnerAddressAggregateTDSChangePromotionParametersTable.GetLocationKeyDBName() + " = " +
                                             AddressAddedOrChangedPromotionRow.LocationKey.ToString();

                            // MessageBox.Show('FilterCriteria: ' + FilterCriteria);
#if TODO
#endif
                            LocationRow = (PLocationRow)FMainDS.PLocation.Rows.Find(new Object[] { AddressAddedOrChangedPromotionRow.SiteKey,
                                                                                                   AddressAddedOrChangedPromotionRow.LocationKey });

                            if (LocationRow != null)
                            {
                                PartnerSharingLocationDV = new DataView(AParameterDT,
                                    FilterCriteria,
                                    PartnerAddressAggregateTDSChangePromotionParametersTable.GetPartnerKeyDBName() + " ASC",
                                    DataViewRowState.CurrentRows);

                                AddressChangedDialog = new TFrmPartnerAddressChangePropagationDialog(FindForm());
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
#if TODO
#endif

                                        if (AddressAddedOrChangedPromotionRow.UserAnswer.StartsWith("CHANGE"))
                                        {
                                            /*
                                             * The LocationRow gets deleted from the LocationTable on the
                                             * Server side, but there a AcceptChanges is done so that the
                                             * DataRow doesn't actually get deleted from the DB. The Client
                                             * would then no longer know that it needs to delete it, so we
                                             * need do remember to do it later!
                                             */
                                            AddCleanupAddressesLocationKey((Int32)AddressAddedOrChangedPromotionRow.LocationKey);
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
#if TODO
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
        /// Returns a DataView that contains only Current DataRows.
        ///
        /// </summary>
        /// <returns>DataView that contains only Current DataRows.
        /// </returns>
        private DataView GetCurrentRecords()
        {
            return new DataView(FMainDS.PPartnerLocation, "", "", DataViewRowState.CurrentRows);
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
                    string StrSecurityViolationExplanation = Catalog.GetString(
                        "You tried to delete an Address that isn't used by any other Partner.\r\n" +
                        "For this operation you need delete permission on Location records,\r\nwhich you do not have.");

                    TMessages.MsgSecurityException(new ESecurityDBTableAccessDeniedException("", "delete",
                            PLocationTable.GetTableDBName()), this.GetType());
                    MessageBox.Show(StrSecurityViolationExplanation,
                        Catalog.GetString("Security Violation - Explanation"),
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
        private Boolean CheckDeleteSecurityGeneral(Boolean AShowMessages)
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
                        string StrSecurityViolationExplanation = Catalog.GetString(
                            "You tried to delete the last Address that the Partner had.\r\n" +
                            "For this operation you need modify permission on PartnerLocation records,\r\nwhich you do not have.");

                        TMessages.MsgSecurityException(new ESecurityDBTableAccessDeniedException("", "modify",
                                PPartnerLocationTable.GetTableDBName()), this.GetType());
                        MessageBox.Show(StrSecurityViolationExplanation,
                            Catalog.GetString("Security Violation - Explanation"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }

                    ReturnValue = false;
                }
            }

            return ReturnValue;
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
                btnNew.Enabled = false;
            }

            // Check security for Edit record operations
            if ((!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY,
                     PLocationTable.GetTableDBName()))
                || (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY,
                        PPartnerLocationTable.GetTableDBName()))
                || (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerTable.GetTableDBName())))
            {
                // needed for setting p_partner.s_date_modified_d = TODAY
                btnDelete.Enabled = false;
            }

            // Check security for Delete record operations
            // do not allow deletion of record, if this is location 0 (this is done in DataGrid_FocusRowEntered)
            if (CheckDeleteSecurityGeneral(false) && btnDelete.Enabled)
            {
                btnDelete.Enabled = true;

                // Note: more checks are done only on clicking the Delete button
                // due to the need for a server roundtrip.
            }
            else
            {
                btnDelete.Enabled = false;
            }

            // Extra security check for LocationType SECURITY_CAN_LOCATIONTYPE
            // MessageBox.Show('Current LocationType: ' + this.PartnerLocationDataRowOfCurrentlySelectedRecord.LocationType);
            if ((GetSelectedDetailRow() != null)
                && (GetSelectedDetailRow().RowState != DataRowState.Deleted)
                && (!GetSelectedDetailRow().IsLocationTypeNull()))
            {
                if (GetSelectedDetailRow().LocationType.EndsWith(SharedConstants.SECURITY_CAN_LOCATIONTYPE)
                    && (!UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_ADDRESSCAN)))
                {
                    btnDelete.Enabled = false;
                }
            }
        }

        private void ValidateDataDetailsManual(PartnerEditTDSPPartnerLocationRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidatePartnerAddressManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);
        }

        #endregion
    }
}