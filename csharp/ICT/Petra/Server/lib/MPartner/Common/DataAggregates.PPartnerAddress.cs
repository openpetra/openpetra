//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;

namespace Ict.Petra.Server.MPartner.DataAggregates
{
    /// <summary>
    /// The TPPartnerAddressAggregate Class contains logic to create, edit and delete
    /// addresses, involving both p_location and p_partner_location tables.
    /// </summary>
    public class TPPartnerAddressAggregate
    {
        private static void DebugLoadedDataset(DataSet ADataSet)
        {
            TLogging.Log("Select done in TPPartnerAddressAggregate.GetData(). Result: ");

            foreach (DataRow TheRow in ADataSet.Tables[PLocationTable.GetTableName()].Rows)
            {
                TLogging.Log(PLocationTable.GetTableName() + ": Processing rows...");
                TLogging.Log(TheRow[0].ToString() + " | " + TheRow[1].ToString());
            }

            foreach (DataRow TheRow in ADataSet.Tables[PPartnerLocationTable.GetTableName()].Rows)
            {
                TLogging.Log(PPartnerLocationTable.GetTableName() + ": Processing rows...");
                TLogging.Log(TheRow[0].ToString() + " | " + TheRow[1].ToString());
            }
        }

        /// <summary>
        /// Passes Partner Address data as a DataSet to the caller. Loads all available
        /// Addresses for the Partner.
        ///
        /// </summary>
        /// <param name="ADataSet">DataSet that holds a DataSet with a DataTable for the
        /// Person</param>
        /// <param name="APartnerKey">PartnerKey of the Partner for which Address data is to be
        /// loaded</param>
        /// <param name="AReadTransaction">Transaction for the SELECT statement
        /// </param>
        /// <returns>void</returns>
        public static void LoadAll(DataSet ADataSet, Int64 APartnerKey, TDBTransaction AReadTransaction)
        {
            PLocationTable LocationDT;
            PPartnerLocationTable PartnerLocationDT;

            PPartnerLocationAccess.LoadViaPPartner(ADataSet, APartnerKey, AReadTransaction);
            PLocationAccess.LoadViaPPartner(ADataSet, APartnerKey, AReadTransaction);

            // Apply security
            LocationDT = (PLocationTable)ADataSet.Tables[PLocationTable.GetTableName()];
            PartnerLocationDT = (PPartnerLocationTable)ADataSet.Tables[PPartnerLocationTable.GetTableName()];
            ApplySecurity(ref PartnerLocationDT, ref LocationDT);

            if (TLogging.DL >= 9)
            {
                DebugLoadedDataset(ADataSet);
            }
        }

        /// <summary>
        /// check the location change; validate and take other required action
        /// eg. change the location of family members, promote address changes
        /// </summary>
        /// <param name="ALocationRow"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AResponseDS"></param>
        /// <param name="ASubmitChangesTransaction"></param>
        /// <param name="AAddressAddedPromotionDT"></param>
        /// <param name="AChangeLocationParametersDT"></param>
        /// <param name="APartnerLocationTable"></param>
        /// <param name="AVerificationResult"></param>
        /// <param name="ACreateLocation"></param>
        /// <param name="AOriginalLocationKey"></param>
        /// <returns></returns>
        private static TSubmitChangesResult PerformLocationChangeChecks(PLocationRow ALocationRow,
            Int64 APartnerKey,
            ref PartnerAddressAggregateTDS AResponseDS,
            TDBTransaction ASubmitChangesTransaction,
            ref PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddressAddedPromotionDT,
            ref PartnerAddressAggregateTDSChangePromotionParametersTable AChangeLocationParametersDT,
            ref PPartnerLocationTable APartnerLocationTable,
            ref TVerificationResultCollection AVerificationResult,
            out Boolean ACreateLocation,
            out TLocationPK AOriginalLocationKey)
        {
            TSubmitChangesResult ReturnValue;
            DataView PropagateLocationParametersDV;
            DataView PropagateLocationParametersDV2;
            Boolean UpdateLocation;

            Int64[] CreateLocationOtherPartnerKeys;
            PartnerAddressAggregateTDSChangePromotionParametersTable ChangePromotionParametersDT;
            PLocationTable NewLocationTable;
            PLocationRow NewLocationRowSaved;

            TVerificationResultCollection SingleVerificationResultCollection;
            Int32 NewLocationLocationKey;
            PPartnerLocationRow PartnerLocationRowForChangedLocation;

            DataSet PartnerLocationModifyDS;
            int Counter;
            Int64 OldLocationKey;
            OdbcParameter[] ParametersArray;
            String OtherPartnerKeys = "";

            AOriginalLocationKey = null;
//          TLogging.LogAtLevel(9, "PerformLocationChangeChecks: AAddressAddedPromotionDT.Rows.Count: " + AAddressAddedPromotionDT.Rows.Count.ToString());

            if (CheckLocationChange(ALocationRow, APartnerKey, ref AAddressAddedPromotionDT, ASubmitChangesTransaction, out UpdateLocation,
                    out ACreateLocation, out CreateLocationOtherPartnerKeys, out ChangePromotionParametersDT))
            {
                // Check if there is a Parameter Row for the LocationKey we are looking at
                PropagateLocationParametersDV = new DataView(AAddressAddedPromotionDT,
                    PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName() + " = " + ALocationRow.SiteKey.ToString() +
                    " AND " +
                    PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName() + " = " +
                    ALocationRow.LocationKey.ToString() +
                    " AND " + PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationChangeDBName() + " = true AND " +
                    PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetAnswerProcessedClientSideDBName() + " = false",
                    "",
                    DataViewRowState.CurrentRows);

                if (PropagateLocationParametersDV.Count > 0)
                {
//                  TLogging.LogAtLevel(9, "PerformLocationChangeChecks: Location " + ALocationRow.LocationKey.ToString() +
//                          ": Location has been changed, decision on propagation is needed.");

                    /*
                     * More information is needed (usually via user interaction)
                     * -> stop processing here and return parameters
                     * (usually used for UI interaction)
                     */
                    if (AResponseDS == null)
                    {
//                      TLogging.LogAtLevel(9, TLogging.Log("PerformLocationChangeChecks: Creating AResponseDS.");
                        AResponseDS = new PartnerAddressAggregateTDS(MPartnerConstants.PARTNERADDRESSAGGREGATERESPONSE_DATASET);
                    }

//                  TLogging.LogAtLevel(9, "PerformLocationChangeChecks: AAddressAddedPromotionDT.Rows.Count: " + AAddressAddedPromotionDT.Rows.Count.ToString());
                    AResponseDS.Merge(AAddressAddedPromotionDT);
//                  TLogging.LogAtLevel(9, "PerformLocationChangeChecks: Merged AAddressAddedPromotionDT into AResponseDS.");
                    AResponseDS.Merge(ChangePromotionParametersDT);
//                  TLogging.LogAtLevel(9, "PerformLocationChangeChecks: Merged ChangePromotionParametersDT into AResponseDS.");
//                  TLogging.LogAtLevel(9, "PerformLocationChangeChecks: AResponseDS.Tables[" + MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME +
//                      "].Rows.Count: " + AResponseDS.Tables[MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME].Rows.Count.ToString());
                    return TSubmitChangesResult.scrInfoNeeded;
                }
                else
                {
//                  TLogging.LogAtLevel(9, "PerformLocationChangeChecks: User made his/her choice regarding Location Change promotion; now processing...");

                    /*
                     * User made his/her choice regarding Location Change promotion; now process it
                     */
                    if (ACreateLocation)
                    {
                        OldLocationKey = ALocationRow.LocationKey;
                        AOriginalLocationKey = new TLocationPK(
                            Convert.ToInt64(ALocationRow[PLocationTable.GetSiteKeyDBName(),
                                                         DataRowVersion.Original]),
                            Convert.ToInt32(ALocationRow[PLocationTable.GetLocationKeyDBName(),
                                                         DataRowVersion.Original]));

                        // ALocationRow.LocationKey;
//                      TLogging.LogAtLevel(9, "PerformLocationChangeChecks: Location " + AOriginalLocationKey.LocationKey.ToString() + ": should be created.");

                        /*
                         * Create and save NEW Location that holds the same data than the changed
                         * Location.
                         */
                        NewLocationTable = new PLocationTable();
                        NewLocationRowSaved = NewLocationTable.NewRowTyped(false);
                        NewLocationRowSaved.ItemArray = DataUtilities.DestinationSaveItemArray(NewLocationRowSaved, ALocationRow);
                        NewLocationRowSaved.LocationKey = -1;
                        NewLocationTable.Rows.Add(NewLocationRowSaved);

                        // Submit the NEW Location to the DB
                        if (!PLocationAccess.SubmitChanges(NewLocationTable, ASubmitChangesTransaction, out SingleVerificationResultCollection))
                        {
                            AVerificationResult.AddCollection(SingleVerificationResultCollection);
                            return TSubmitChangesResult.scrError;
                        }

                        // The DB gives us a LocationKey from a Sequence. Remember this one.
                        NewLocationLocationKey = (Int32)NewLocationRowSaved.LocationKey;
//                      TLogging.LogAtLevel(9, "PerformLocationChangeChecks: New Location created! Its Location Key is: " + NewLocationLocationKey.ToString());

                        // Add the new row to the LocationTable that is beeing processed as well
                        // NewLocationCurrentTableRow := (ALocationRow.Table as PLocationTable).NewRowTyped(false);
                        // NewLocationCurrentTableRow.ItemArray := NewLocationRowSaved.ItemArray;
                        // ALocationRow.Table.Rows.Add(NewLocationCurrentTableRow);
                        // Make the row unchanged so that it isn't picked up as a 'new Address'
                        // and that it doesn't get saved later. Will be sent back to the Partner
                        // Edit screen lateron.
                        // NewLocationCurrentTableRow.AcceptChanges;

                        /*
                         * Update the reference from the changed Location to the new Location in
                         * the Partner's PartnerLocation DataTable. This will be saved later in
                         * the call to SubmitChanges in the main loop of the SubmitData function.
                         */
                        PartnerLocationRowForChangedLocation =
                            (PPartnerLocationRow)APartnerLocationTable.Rows.Find(new object[] { APartnerKey, ALocationRow.SiteKey,
                                                                                                ALocationRow.LocationKey });
                        PartnerLocationRowForChangedLocation.LocationKey = NewLocationLocationKey;

                        // Now delete the changed Location so that it doesn't get saved!
                        // ALocationRow.Delete;
                        // ALocationRow.AcceptChanges;
                        // Overwrite the Location that should be replaced with the data of the new Location
                        ALocationRow.ItemArray = NewLocationRowSaved.ItemArray;
                        PropagateLocationParametersDV2 = new DataView(AAddressAddedPromotionDT,
                            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName() + " = " +
                            NewLocationRowSaved.SiteKey.ToString() + " AND " +
                            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName() + " = " +
                            OldLocationKey.ToString() +
                            " AND " + PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationChangeDBName() + " = true AND " +
                            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetAnswerProcessedClientSideDBName() + " = true",
                            "",
                            DataViewRowState.CurrentRows);
                        ((PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow)(PropagateLocationParametersDV2[0].Row)).LocationKey =
                            ALocationRow.LocationKey;

                        if (CreateLocationOtherPartnerKeys.Length > 0)
                        {
//                          TLogging.LogAtLevel(9, "PerformLocationChangeChecks: Created Location " + NewLocationLocationKey.ToString() +
//                              ": should be assigned to " + Convert.ToInt32(CreateLocationOtherPartnerKeys.Length).ToString() + " Partners...");

                            // Build list of PartnerKeys for IN (x,y) clause in the SQL statement
                            for (Counter = 0; Counter <= CreateLocationOtherPartnerKeys.Length - 1; Counter += 1)
                            {
                                OtherPartnerKeys = OtherPartnerKeys + CreateLocationOtherPartnerKeys[Counter].ToString() + ',';
                            }

                            // remove last ','
                            OtherPartnerKeys = OtherPartnerKeys.Substring(0, OtherPartnerKeys.Length - 1);

                            // Load data for all the other selected Partners that reference
                            // the PartnerLocation
                            PartnerLocationModifyDS = new DataSet();
                            PartnerLocationModifyDS.Tables.Add(new PPartnerLocationTable());
                            ParametersArray = new OdbcParameter[2];
                            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
                            ParametersArray[0].Value = (System.Object)(NewLocationRowSaved.SiteKey);
                            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
                            ParametersArray[1].Value = (System.Object)(AOriginalLocationKey.LocationKey);
                            PartnerLocationModifyDS = DBAccess.GDBAccessObj.Select(PartnerLocationModifyDS,
                                "SELECT * " + "FROM PUB_" + PPartnerLocationTable.GetTableDBName() + ' ' + "WHERE " +
                                PPartnerLocationTable.GetPartnerKeyDBName() + " IN (" + OtherPartnerKeys + ") " + "AND " +
                                PPartnerLocationTable.GetSiteKeyDBName() + " = ? " + "AND " + PPartnerLocationTable.GetLocationKeyDBName() + " = ?",
                                PPartnerLocationTable.GetTableName(),
                                ASubmitChangesTransaction,
                                ParametersArray);

                            // Change the LocationKey for every one of those PartnerLocation
                            // DataRows to point to the NEW Location
                            for (Counter = 0; Counter <= CreateLocationOtherPartnerKeys.Length - 1; Counter += 1)
                            {
                                ((PPartnerLocationTable)PartnerLocationModifyDS.Tables[0])[Counter].LocationKey = NewLocationLocationKey;
                            }

                            // Submit the changes to those PartnerLocations to the DB
                            if (!PPartnerLocationAccess.SubmitChanges((PPartnerLocationTable)PartnerLocationModifyDS.Tables[0],
                                    ASubmitChangesTransaction, out SingleVerificationResultCollection))
                            {
                                AVerificationResult.AddCollection(SingleVerificationResultCollection);
                                return TSubmitChangesResult.scrError;
                            }
                        }
                        else
                        {
//                          TLogging.LogAtLevel(9, "PerformLocationChangeChecks: Created Location " + NewLocationLocationKey.ToString() + ": should not be assigned to any other Partners...");

                            /*
                             * Don't need to do anything here - the just created Location got already
                             * assigned to the Partner we are currently working with.
                             */
                        }
                    }
                    else if (UpdateLocation)
                    {
//                      TLogging.LogAtLevel(9, "PerformLocationChangeChecks: Location " + ALocationRow.LocationKey.ToString() +
//                              ": should simply get updated; therefore the Locations of ALL Partners will be changed...");

                        /*
                         * Don't need to do anything here - the changed Location will be saved
                         * in the call to SubmitChanges in the main loop of the SubmitData function.
                         */
                    }
                }

                ReturnValue = TSubmitChangesResult.scrOK;
            }
            else
            {
                TLogging.LogAtLevel(9, "PerformLocationChangeChecks: Location " + ALocationRow.LocationKey.ToString() +
                    ": User cancelled the selection - stopping the whole saving process!");

                /*
                 * User cancelled the selection - stop the whole saving process!
                 */
                AVerificationResult.Add(new TVerificationResult("Location Change Promotion: Information",
                        "No changes were saved because the Location Change Promotion dialog was cancelled by the user.", "Saving cancelled by user",
                        "",
                        TResultSeverity.Resv_Noncritical));
                ReturnValue = TSubmitChangesResult.scrError;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Applies Address Security by inspecting the submitted DataTable for the
        /// PartnerLocation records' LocationType DataColumn and the user's presence in
        /// Security Group ADDRESSCAN.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the PartnerLocation records' LocationType DataColumn exists and the
        /// user isn't in the mentioned Security Group, the LocationTypes of all records
        /// are checked.
        /// For any records where the security check fails, the information in all
        /// DataColumns of the Location and PartnerLocation Tables that are present in
        /// the submitted DataTable gets stripped out, Strings get replaced with
        /// StrRestrictedInformation.
        /// </para>
        /// <para>
        /// <em>The performance of the other overload
        /// <see cref="M:ApplySecurity(ref PPartnerLocationTable, ref PLocationTable)" />
        /// is better. Use it whenever the Partner Location data is available in the
        /// form of the Typed DataTable PPartnerLocation!</em>
        /// </para>
        /// </remarks>
        /// <param name="AInspectDT">DataTable that holds DataColumns from the
        /// PartnerLocation (and optionally Location) DB Table(s). <em>Address Security can
        /// only be applied if the LocationType DataColumn of the PartnerLocation DB Table
        /// is present in the DataTable!</em></param>
        /// <returns>void</returns>
        public static void ApplySecurity(ref DataTable AInspectDT)
        {
            string LocationTypeDBName;

            LocationTypeDBName = PPartnerLocationTable.GetLocationTypeDBName();

            if (!UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_ADDRESSCAN))
            {
//              TLogging.LogAtLevel(9, "ApplySecurity (1): User isn't in Security Group ADDRESSCAN.");
                if (AInspectDT.Columns.Contains(LocationTypeDBName))
                {
//                  TLogging.LogAtLevel(9, "ApplySecurity (1): LocationType is present.");

                    foreach (DataRow TmpDR in AInspectDT.Rows)
                    {
                        if (TLogging.DL >= 9)
                        {
                            TLogging.Log("LocationType: " + TmpDR[LocationTypeDBName].ToString());
                        }

                        if (TmpDR[LocationTypeDBName].ToString().EndsWith(SharedConstants.SECURITY_CAN_LOCATIONTYPE))
                        {
//                          TLogging.LogAtLevel(9, "ApplySecurity (1): LocationType is ending with '-CAN', applying Address Security...");

                            /*
                             * Check for existance of PPartnerLocation DataTable columns and
                             * strip out information from PPartnerLocation DataTable Columns;
                             * replace Strings with StrRestrictedInformation.
                             */
                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetDateEffectiveDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetDateEffectiveDBName()] = DBNull.Value;
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetDateGoodUntilDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetDateGoodUntilDBName()] = DBNull.Value;
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetSendMailDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetSendMailDBName()] = false;
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetFaxNumberDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetFaxNumberDBName()] = false;
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetTelexDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetTelexDBName()] = 0;
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetTelephoneNumberDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetTelephoneNumberDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetExtensionDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetExtensionDBName()] = 0;
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetEmailAddressDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetEmailAddressDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetLocationDetailCommentDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetLocationDetailCommentDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetFaxExtensionDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetFaxExtensionDBName()] = 0;
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetMobileNumberDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetMobileNumberDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetAlternateTelephoneDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetAlternateTelephoneDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetUrlDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetUrlDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetDateCreatedDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetDateCreatedDBName()] = DBNull.Value;
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetCreatedByDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetCreatedByDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetDateModifiedDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetDateModifiedDBName()] = DBNull.Value;
                            }

                            if (AInspectDT.Columns.Contains(PPartnerLocationTable.GetModifiedByDBName()))
                            {
                                TmpDR[PPartnerLocationTable.GetModifiedByDBName()] = Catalog.GetString("** restricted **");
                            }

                            /*
                             * Check for existance of PLocation DataTable columns and
                             * strip out information from PLocation DataTable Columns;
                             * replace Strings with StrRestrictedInformation.
                             */
                            if (AInspectDT.Columns.Contains(PLocationTable.GetStreetNameDBName()))
                            {
                                TmpDR[PLocationTable.GetStreetNameDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PLocationTable.GetLocalityDBName()))
                            {
                                TmpDR[PLocationTable.GetLocalityDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PLocationTable.GetCityDBName()))
                            {
                                TmpDR[PLocationTable.GetCityDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PLocationTable.GetCountyDBName()))
                            {
                                TmpDR[PLocationTable.GetCountyDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PLocationTable.GetPostalCodeDBName()))
                            {
                                TmpDR[PLocationTable.GetPostalCodeDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PLocationTable.GetCountryCodeDBName()))
                            {
                                TmpDR[PLocationTable.GetCountryCodeDBName()] = Catalog.GetString("** restricted **");
                            }

                            if (AInspectDT.Columns.Contains(PLocationTable.GetAddress3DBName()))
                            {
                                TmpDR[PLocationTable.GetAddress3DBName()] = Catalog.GetString("** restricted **");
                            }

                            // Don't need to check for DateCreated, CreatedBy, DateModified, ModifiedBy here
                            // because that is already done earlier on in this Method for the PPartnerLocation
                            // DB Table Fields, and the Fields are named the same for the PLocation DB Table.

                            TmpDR.AcceptChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applies Address Security by inspecting all PartnerLocation records'
        /// LocationTypes and the user's presence in security Group ADDRESSCAN.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the user isn't in the mentioned Security Group, the LocationTypes of all
        /// records are checked.
        /// For any records where the security check fails, the information in all
        /// DataColumns of the Location and PartnerLocation Tables that are present in
        /// the submitted DataTables gets stripped out, Strings get replaced with
        /// StrRestrictedInformation. For the LocationTable this is only true if it is
        /// submitted and holds records that are restricted in the PPartnerLocation
        /// DataTable.
        /// </para>
        /// <para>
        /// <em>Another overload <see cref="M:ApplySecurity(ref DataTable)" /> is available which
        /// can be used if the Partner Location (and/or Location) data isn't available
        /// in the form of the Typed DataTables PPartnerLocation and PLocation!</em>
        /// </para>
        /// </remarks>
        /// <param name="ALocationDT">Location Typed DataTable (optional, set to nil if there is
        /// no Location Typed DataTable that goes with the PartnerLocation Typed
        /// DataTable that is submitted in APartnerLocationDT.</param>
        /// <param name="APartnerLocationDT">PartnerLocation Typed DataTable
        /// </param>
        /// <returns>void</returns>
        public static void ApplySecurity(ref PPartnerLocationTable APartnerLocationDT, ref PLocationTable ALocationDT)
        {
            PPartnerLocationRow PartnerLocationDR;
            PLocationRow LocationDR;

            if (!UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_ADDRESSCAN))
            {
//              TLogging.LogAtLevel(9, "ApplySecurity (2): User isn't in Security Group ADDRESSCAN.");

                foreach (DataRow TmpDR in APartnerLocationDT.Rows)
                {
                    PartnerLocationDR = (PPartnerLocationRow)TmpDR;

                    if (PartnerLocationDR.LocationType.EndsWith(SharedConstants.SECURITY_CAN_LOCATIONTYPE))
                    {
                        /*
                         * Need to strip out information from PPartnerLocation DataTable;
                         * replace Strings with StrRestrictedInformation.
                         */
                        PartnerLocationDR.SetDateEffectiveNull();
                        PartnerLocationDR.SetDateGoodUntilNull();
                        PartnerLocationDR.SendMail = false;
                        PartnerLocationDR.FaxNumber = Catalog.GetString("** restricted **");
                        PartnerLocationDR.Telex = 0;
                        PartnerLocationDR.TelephoneNumber = Catalog.GetString("** restricted **");
                        PartnerLocationDR.Extension = 0;
                        PartnerLocationDR.EmailAddress = Catalog.GetString("** restricted **");
                        PartnerLocationDR.LocationDetailComment = Catalog.GetString("** restricted **");
                        PartnerLocationDR.FaxExtension = 0;
                        PartnerLocationDR.MobileNumber = Catalog.GetString("** restricted **");
                        PartnerLocationDR.AlternateTelephone = Catalog.GetString("** restricted **");
                        PartnerLocationDR.Url = Catalog.GetString("** restricted **");
                        PartnerLocationDR.SetDateCreatedNull();
                        PartnerLocationDR.CreatedBy = Catalog.GetString("** restricted **");
                        PartnerLocationDR.SetDateModifiedNull();
                        PartnerLocationDR.ModifiedBy = Catalog.GetString("** restricted **");
                        PartnerLocationDR.AcceptChanges();

                        // Check if we can find this PartnerLocation Row also in the Location DataTable
                        if (ALocationDT != null)
                        {
                            LocationDR = (PLocationRow)ALocationDT.Rows.Find(new System.Object[] { PartnerLocationDR.SiteKey,
                                                                                                   PartnerLocationDR.LocationKey });

                            if (LocationDR != null)
                            {
                                /*
                                 * Location DataTable holds the Row that matches the PartnerLocation Row
                                 * --> Need to strip out information from PLocation DataTable;
                                 * replace Strings with StrRestrictedInformation.
                                 */
                                LocationDR.StreetName = Catalog.GetString("** restricted **");
                                LocationDR.Locality = Catalog.GetString("** restricted **");
                                LocationDR.City = Catalog.GetString("** restricted **");
                                LocationDR.County = Catalog.GetString("** restricted **");
                                LocationDR.PostalCode = Catalog.GetString("** restricted **");

                                // TRANSLATORS: this is used for the restricted country codes
                                LocationDR.CountryCode = Catalog.GetString("99");
                                LocationDR.Address3 = Catalog.GetString("** restricted **");
                                LocationDR.SetDateCreatedNull();
                                LocationDR.CreatedBy = Catalog.GetString("** restricted **");
                                LocationDR.SetDateModifiedNull();
                                LocationDR.ModifiedBy = Catalog.GetString("** restricted **");
                                LocationDR.AcceptChanges();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="APartnerLocationRow"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAddressAddedOrChangedPromotionDT"></param>
        /// <param name="AReadTransaction"></param>
        /// <param name="AUpdatePartnerLocations"></param>
        /// <param name="AUpdatePartnerLocationOtherPersons"></param>
        /// <param name="AChangePromotionParametersDT"></param>
        /// <returns></returns>
        private static Boolean CheckPartnerLocationChange(PPartnerLocationRow APartnerLocationRow,
            Int64 APartnerKey,
            ref PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddressAddedOrChangedPromotionDT,
            TDBTransaction AReadTransaction,
            out Boolean AUpdatePartnerLocations,
            out Int64[, ] AUpdatePartnerLocationOtherPersons,
            ref PartnerAddressAggregateTDSChangePromotionParametersTable AChangePromotionParametersDT)
        {
            Boolean ReturnValue;
            DataView AddressAddedOrChangedPromotionDV;
            StringCollection ChangedDetails;

            string[] ChangeSomeArray;
            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow AddressAddedOrChangedRow;
            OdbcParameter[] ParametersArray;
            DataSet PersonsLocationReferencesDS;
            DataRow OtherPartnerLocationReferenceRow;
            Int32 Counter;
            Int32 Counter2;
            Int32 Counter3;
            PartnerAddressAggregateTDSChangePromotionParametersRow PartnerLocationChangePromotionRow;
            DataView AChangePromotionParametersDV;

            AUpdatePartnerLocations = false;
            AUpdatePartnerLocationOtherPersons = new Int64[0, 0];

//          TLogging.LogAtLevel(9, "CheckPartnerLocationChange for Location " + APartnerLocationRow.LocationKey.ToString() +
//                  ": AAddressAddedOrChangedPromotionDT.Rows.Count: " + AAddressAddedOrChangedPromotionDT.Rows.Count.ToString());

            // Check if there is a Parameter Row for the LocationKey we are looking at
            AddressAddedOrChangedPromotionDV = new DataView(AAddressAddedOrChangedPromotionDT,
                PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName() + " = " +
                APartnerLocationRow[PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName(),
                                    DataRowVersion.Original].ToString() + " AND " +
                PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName() +
                " = " +
                APartnerLocationRow[PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName(),
                                    DataRowVersion.Original].ToString() + " AND " +
                PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetPartnerLocationChangeDBName() + " = true",
                "",
                DataViewRowState.CurrentRows);

            // APartnerLocationRow.SiteKey.ToString
            // APartnerLocationRow.LocationKey.ToString
            // No, there isn't one  therefore create one (if necessary)
            if (AddressAddedOrChangedPromotionDV.Count == 0)
            {
                /*
                 * Check if one ore more of specific PartnerLocation Data-holding fields
                 * were changed
                 */
                if (CheckHasPartnerLocationPromotionDetailChanged(APartnerLocationRow, out ChangedDetails))
                {
                    if (PPersonAccess.CountViaPFamily(APartnerKey, AReadTransaction) > 0)
                    {
//                      TLogging.LogAtLevel(9, "CheckPartnerLocationChange: PartnerLocation with LocationKey " + APartnerLocationRow.LocationKey.ToString() +
//                          ": certain fields have been changed and there are Family Members to which they can be promoted!");
                        #region Build AddressAddedOrChangedPromotion DataTable
                        AAddressAddedOrChangedPromotionDT = new PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable(
                            MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME);
                        AddressAddedOrChangedRow = AAddressAddedOrChangedPromotionDT.NewRowTyped(false);
                        AddressAddedOrChangedRow.SiteKey =
                            Convert.ToInt64(APartnerLocationRow[PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName(),
                                                                DataRowVersion.Original]);

                        // APartnerLocationRow.SiteKey;
                        AddressAddedOrChangedRow.LocationKey =
                            Convert.ToInt32(APartnerLocationRow[PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName(),
                                                                DataRowVersion.Original]);

                        // APartnerLocationRow.LocationKey;
                        AddressAddedOrChangedRow.PartnerKey = APartnerKey;
                        AddressAddedOrChangedRow.LocationChange = false;
                        AddressAddedOrChangedRow.PartnerLocationChange = true;
                        AddressAddedOrChangedRow.LocationAdded = false;
                        AddressAddedOrChangedRow.ChangedFields = StringHelper.StrMerge(ChangedDetails, '|');
//                      TLogging.LogAtLevel(9, "CheckPartnerLocationChange: ChangedFields String: " + AddressAddedOrChangedRow.ChangedFields.ToString() +
//                                " (ChangedDetails.Count /4: " + Convert.ToInt16(ChangedDetails.Count / 4.0).ToString() + ')');
                        AddressAddedOrChangedRow.AnswerProcessedClientSide = false;
                        AddressAddedOrChangedRow.AnswerProcessedServerSide = false;
                        AAddressAddedOrChangedPromotionDT.Rows.Add(AddressAddedOrChangedRow);
                        #endregion
                        #region Build ChangePromotionParameters DataTable
                        AChangePromotionParametersDT = new PartnerAddressAggregateTDSChangePromotionParametersTable(
                            MPartnerConstants.ADDRESSCHANGEPROMOTIONPARAMETERS_TABLENAME);

                        // Load data for all PERSONs of the FAMILY
                        ParametersArray = new OdbcParameter[1];
                        ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
                        ParametersArray[0].Value = (System.Object)(APartnerKey);
                        PersonsLocationReferencesDS = DBAccess.GDBAccessObj.Select(
                            "SELECT PUB_" + PPartnerLocationTable.GetTableDBName() + '.' + PPartnerLocationTable.GetPartnerKeyDBName() + ", " +
                            PPartnerLocationTable.GetSiteKeyDBName() + ", " + PPartnerLocationTable.GetLocationKeyDBName() + ", " +
                            PPartnerTable.GetPartnerShortNameDBName() + ", " +
                            PPartnerTable.GetPartnerClassDBName() + ", " + PPartnerLocationTable.GetTelephoneNumberDBName() + ", " +
                            PPartnerLocationTable.GetExtensionDBName() + ", " + PPartnerLocationTable.GetFaxNumberDBName() + ", " +
                            PPartnerLocationTable.GetFaxExtensionDBName() + ", " + PPartnerLocationTable.GetAlternateTelephoneDBName() + ", " +
                            PPartnerLocationTable.GetMobileNumberDBName() + ", " + PPartnerLocationTable.GetEmailAddressDBName() + ", " +
                            PPartnerLocationTable.GetUrlDBName() + ", " + PPartnerLocationTable.GetSendMailDBName() + ", " +
                            PPartnerLocationTable.GetDateEffectiveDBName() + ", " + PPartnerLocationTable.GetDateGoodUntilDBName() + ", " +
                            PPartnerLocationTable.GetLocationTypeDBName() + ' ' + "FROM PUB_" + PPersonTable.GetTableDBName() + ", PUB_" +
                            PPartnerLocationTable.GetTableDBName() + ", PUB_" + PPartnerTable.GetTableDBName() + ' ' + "WHERE PUB_" +
                            PPersonTable.GetTableDBName() + '.' + PPersonTable.GetFamilyKeyDBName() + " = ? " + "AND PUB_" +
                            PPartnerLocationTable.GetTableDBName() + '.' + PPartnerLocationTable.GetPartnerKeyDBName() + " = PUB_" +
                            PPersonTable.GetTableDBName() + '.' + PPersonTable.GetPartnerKeyDBName() + ' ' + "AND PUB_" +
                            PPartnerTable.GetTableDBName() +
                            '.' + PPartnerTable.GetPartnerKeyDBName() + " = PUB_" + PPersonTable.GetTableDBName() + '.' +
                            PPersonTable.GetPartnerKeyDBName(
                                ) + ' ' + "ORDER BY PUB_" + PPersonTable.GetTableDBName() + '.' + PPersonTable.GetFamilyIdDBName() +
                            " ASC, PUB_" +
                            PPartnerLocationTable.GetTableDBName() + '.' + PPartnerLocationTable.GetSendMailDBName() + " DESC",
                            "PersonsLocationReferences", AReadTransaction, ParametersArray);

                        // Insert data into the ChangePromotionParameters DataTable
                        for (Counter = 0; Counter <= PersonsLocationReferencesDS.Tables[0].Rows.Count - 1; Counter += 1)
                        {
                            OtherPartnerLocationReferenceRow = PersonsLocationReferencesDS.Tables[0].Rows[Counter];
                            PartnerLocationChangePromotionRow = AChangePromotionParametersDT.NewRowTyped(false);
                            PartnerLocationChangePromotionRow.PartnerKey =
                                Convert.ToInt64(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetPartnerKeyDBName()]);
                            PartnerLocationChangePromotionRow.SiteKey =
                                Convert.ToInt64(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetSiteKeyDBName()]);
                            PartnerLocationChangePromotionRow.LocationKey =
                                Convert.ToInt32(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetLocationKeyDBName()]);
                            PartnerLocationChangePromotionRow.PartnerShortName =
                                OtherPartnerLocationReferenceRow[PPartnerTable.GetPartnerShortNameDBName()].ToString();
                            PartnerLocationChangePromotionRow.PartnerClass =
                                OtherPartnerLocationReferenceRow[PPartnerTable.GetPartnerClassDBName()].ToString();
                            PartnerLocationChangePromotionRow.TelephoneNumber =
                                OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetTelephoneNumberDBName()].ToString();
                            PartnerLocationChangePromotionRow.Extension =
                                Convert.ToInt32(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetExtensionDBName()]);
                            PartnerLocationChangePromotionRow.FaxNumber =
                                OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetFaxNumberDBName()].ToString();
                            PartnerLocationChangePromotionRow.FaxExtension =
                                Convert.ToInt32(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetFaxExtensionDBName()]);
                            PartnerLocationChangePromotionRow.AlternateTelephone =
                                OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetAlternateTelephoneDBName()].ToString();
                            PartnerLocationChangePromotionRow.MobileNumber =
                                OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetMobileNumberDBName()].ToString();
                            PartnerLocationChangePromotionRow.EmailAddress =
                                OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetEmailAddressDBName()].ToString();
                            PartnerLocationChangePromotionRow.Url = OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetUrlDBName()].ToString();
                            PartnerLocationChangePromotionRow.SendMail =
                                Convert.ToBoolean(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetSendMailDBName()]);

                            if (!OtherPartnerLocationReferenceRow.IsNull(PPartnerLocationTable.GetDateEffectiveDBName()))
                            {
                                PartnerLocationChangePromotionRow.DateEffective =
                                    Convert.ToDateTime(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetDateEffectiveDBName()]);
                            }

                            if (!OtherPartnerLocationReferenceRow.IsNull(PPartnerLocationTable.GetDateGoodUntilDBName()))
                            {
                                PartnerLocationChangePromotionRow.DateGoodUntil =
                                    Convert.ToDateTime(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetDateGoodUntilDBName()]);
                            }

                            PartnerLocationChangePromotionRow.LocationType =
                                OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetLocationTypeDBName()].ToString();

                            // Add the SiteKey and LocationKey of the currently processed
                            // PartnerLocation record. This is necessary to be able to filter on the
                            // rows at a later stage on Client side and Server side!
                            PartnerLocationChangePromotionRow.SiteKeyOfEditedRecord =
                                Convert.ToInt64(APartnerLocationRow[PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName(),
                                                                    DataRowVersion.Original]);

                            // APartnerLocationRow.SiteKey;
                            PartnerLocationChangePromotionRow.LocationKeyOfEditedRecord =
                                Convert.ToInt32(APartnerLocationRow[PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.
                                                                    GetLocationKeyDBName(),
                                                                    DataRowVersion.Original]);

                            // APartnerLocationRow.LocationKey;
                            AChangePromotionParametersDT.Rows.Add(PartnerLocationChangePromotionRow);
                        }

//                      TLogging.LogAtLevel(9, "CheckPartnerLocationChange: Location " + APartnerLocationRow.LocationKey.ToString() +
//                              ": inserted PartnerLocation data of " + AChangePromotionParametersDT.Rows.Count.ToString() +
//                              " PERSON''s of that FAMILY into AChangePromotionParametersDT!");
                        #endregion
                    }
                }

                ReturnValue = true;
            }
            else
            {
                // AAddressAddedOrChangedPromotionDT was passed in, holding parameters for the LocationKey we are looking at
                AddressAddedOrChangedRow = (PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow)AddressAddedOrChangedPromotionDV[0].Row;

                if (AddressAddedOrChangedRow.UserAnswer == "CHANGE-NONE")
                {
//                  TLogging.LogAtLevel(9, "CheckPartnerLocationChange: AAddressAddedOrChangedPromotionDT tells me to UPDATE NONE of the Persons with the changes that were made to the PartnerLocation.");

                    // No processing necessary!
                    AddressAddedOrChangedRow.AnswerProcessedClientSide = true;
                    AddressAddedOrChangedRow.AcceptChanges();
                    ReturnValue = true;
                }
                else if (AddressAddedOrChangedRow.UserAnswer.StartsWith("CHANGE-SOME"))
                {
//                  TLogging.LogAtLevel(9, "CheckPartnerLocationChange: AAddressAddedOrChangedPromotionDT tells me to UPDATE SOME Persons with the changes that were made to the PartnerLocation.");
                    AUpdatePartnerLocations = true;

                    // Parse the UserAnswer. It's format is 'CHANGESOME:PartnerKey1,SiteKey1,LocationKey1;PartnerKey2,SiteKey2,LocationKey2;PartnerKeyN,SiteKeyN,LocationKeyN'
                    ChangeSomeArray = AddressAddedOrChangedRow.UserAnswer.Split(":,;".ToCharArray());

                    /*
                     * Build the AUpdatePartnerLocationOtherPersons array from the UserAnswer
                     * to signal to calling procedure that the changes that were made to the
                     * PartnerLocation that we are processing should be
                     * taken over to all the Person's PartnerLocations contained in the Array.
                     */
                    AUpdatePartnerLocationOtherPersons = new Int64[Convert.ToInt32((ChangeSomeArray.Length - 1) / 3.0), 3];

                    // Counter: ' 1': don't include 'CHANGESOME' array entry, '/ 3' each entry consists of three strings:
                    Counter = 1;
                    Counter2 = 0;

                    while (Counter < AUpdatePartnerLocationOtherPersons.GetLength(0) * 3)
                    {
                        // store PartnerKey
                        AUpdatePartnerLocationOtherPersons[Counter2, 0] = Convert.ToInt64(ChangeSomeArray[Counter]);
//                      TLogging.LogAtLevel(9, "CheckPartnerLocationChange: PartnerKey[" + Counter2.ToString() + "]: " +
//                              AUpdatePartnerLocationOtherPersons[Counter2, 0].ToString());

                        // store SiteKey
                        AUpdatePartnerLocationOtherPersons[Counter2, 1] = Convert.ToInt32(ChangeSomeArray[Counter + 1]);
//                      TLogging.LogAtLevel(9, "CheckPartnerLocationChange: SiteKey[" + Counter2.ToString() + "]: " +
//                            AUpdatePartnerLocationOtherPersons[Counter2, 1].ToString());

                        // store LocationKey
                        AUpdatePartnerLocationOtherPersons[Counter2, 2] = Convert.ToInt32(ChangeSomeArray[Counter + 2]);
//                      TLogging.LogAtLevel(9, "CheckPartnerLocationChange: LocationKey[" + Counter2.ToString() + "]: " +
//                            AUpdatePartnerLocationOtherPersons[Counter2, 2].ToString());
                        // position Counter to next 'record' of PartnerKey, SiteKey and LocationKey
                        Counter = Counter + 3;
                        Counter2 = Counter2 + 1;
                    }

                    AddressAddedOrChangedRow.AnswerProcessedClientSide = true;
                    AddressAddedOrChangedRow.AcceptChanges();
                    ReturnValue = true;
                }
                else if (AddressAddedOrChangedRow.UserAnswer == "CHANGE-ALL")
                {
//                  TLogging.LogAtLevel(9,  "CheckPartnerLocationChange: AAddressAddedOrChangedPromotionDT tells me to UPDATE ALL Persons with the changes that were made to the PartnerLocation.");
                    AUpdatePartnerLocations = true;

                    /*
                     * Build the AUpdatePartnerLocationOtherPersons array from
                     * AChangePromotionParametersDT to signal to calling procedure that the
                     * changes that were made to the PartnerLocation that we are processing
                     * should be taken over to all the Person's PartnerLocations contained in
                     * the Array.
                     */

                    // Process only AChangePromotionParametersDT rows that are for the current
                    // SiteKey and LocationKey!
//                  TLogging.LogAtLevel(9, "CheckPartnerLocationChange: AChangePromotionParametersDT.Rows.Count: " +
//                        AChangePromotionParametersDT.Rows.Count.ToString());
                    AChangePromotionParametersDV = new DataView(AChangePromotionParametersDT,
                        PartnerAddressAggregateTDSChangePromotionParametersTable.GetSiteKeyOfEditedRecordDBName() + " = " +
                        Convert.ToInt64(APartnerLocationRow[PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName(),
                                                            DataRowVersion.Original]).ToString() + " AND " +
                        PartnerAddressAggregateTDSChangePromotionParametersTable.GetLocationKeyOfEditedRecordDBName() + " = " +
                        Convert.ToInt32(APartnerLocationRow[PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName(),
                                                            DataRowVersion.Original]).ToString(),
                        "",
                        DataViewRowState.CurrentRows);

                    // APartnerLocationRow.SiteKey.ToString
                    // APartnerLocationRow.LocationKey.ToString
//                  TLogging.LogAtLevel(9, "CheckPartnerLocationChange: AChangePromotionParametersDV.Count: " + AChangePromotionParametersDV.Count.ToString());
                    AUpdatePartnerLocationOtherPersons = new Int64[AChangePromotionParametersDV.Count, 3];

                    for (Counter3 = 0; Counter3 <= AChangePromotionParametersDV.Count - 1; Counter3 += 1)
                    {
                        // store PartnerKey
                        AUpdatePartnerLocationOtherPersons[Counter3,
                                                           0] =
                            ((PartnerAddressAggregateTDSChangePromotionParametersRow)(AChangePromotionParametersDV[Counter3].Row)).PartnerKey;

                        // store SiteKey
                        AUpdatePartnerLocationOtherPersons[Counter3,
                                                           1] =
                            ((PartnerAddressAggregateTDSChangePromotionParametersRow)(AChangePromotionParametersDV[Counter3].Row)).SiteKey;

                        // store LocationKey
                        AUpdatePartnerLocationOtherPersons[Counter3,
                                                           2] =
                            ((PartnerAddressAggregateTDSChangePromotionParametersRow)(AChangePromotionParametersDV[Counter3].Row)).LocationKey;
                    }

                    AddressAddedOrChangedRow.AnswerProcessedClientSide = true;
                    AddressAddedOrChangedRow.AcceptChanges();
                    ReturnValue = true;
                }
                else if (AddressAddedOrChangedRow.UserAnswer == "CANCEL")
                {
//                  TLogging.LogAtLevel(9, "CheckPartnerLocationChange: AAddressAddedOrChangedPromotionDT tells me to CANCEL the changing of the PartnerLocation.");
                    AddressAddedOrChangedRow.AnswerProcessedClientSide = true;
                    AddressAddedOrChangedRow.AcceptChanges();
                    ReturnValue = false;
                }
                else
                {
//                  TLogging.LogAtLevel(9, "CheckPartnerLocationChange: AAddressAddedOrChangedPromotionDT holds unexpected UserAnswer: " +
//                        AddressAddedOrChangedRow.UserAnswer + "! Aborting operation!!!");
                    AddressAddedOrChangedRow.AnswerProcessedClientSide = true;
                    AddressAddedOrChangedRow.AcceptChanges();
                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerLocationRow"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="AAddressAddedOrChangedPromotionDT"></param>
        /// <param name="ALocationPK"></param>
        /// <param name="AReadTransaction"></param>
        /// <returns></returns>
        private static Boolean CheckFamilyMemberPropagation(PPartnerLocationRow APartnerLocationRow,
            Int64 APartnerKey,
            String APartnerClass,
            ref PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddressAddedOrChangedPromotionDT,
            TLocationPK ALocationPK,
            TDBTransaction AReadTransaction)
        {
            Boolean ReturnValue;
            DataView AddressAddedOrChangedPromotionDV;
            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow AddressAddedOrChangedRow;
            Boolean FoundFamilyMembers;

//              TLogging.LogAtLevel(9, "CheckFamilyMemberPropagation for Location " + APartnerLocationRow.LocationKey.ToString() +
//                    ": AAddressAddedOrChangedPromotionDT.Rows.Count: " + AAddressAddedOrChangedPromotionDT.Rows.Count.ToString());
//              TLogging.LogAtLevel(9, "CheckFamilyMemberPropagation: ALocationPK.LocationKey: " + ALocationPK.LocationKey.ToString());

            // Check if there is a Parameter Row for the LocationKey we are looking at
            AddressAddedOrChangedPromotionDV = new DataView(AAddressAddedOrChangedPromotionDT,
                PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName() + " = " + ALocationPK.SiteKey.ToString() + " AND " +
                PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName() + " = " + ALocationPK.LocationKey.ToString() +
                " AND " + PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationAddedDBName() + " = true AND " +
                PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetUserAnswerDBName() + " <> ''''",
                "",
                DataViewRowState.CurrentRows);

            // No, there isn't one: perform DB check
            if (AddressAddedOrChangedPromotionDV.Count == 0)
            {
                if (PPersonAccess.CountViaPFamily(APartnerKey, AReadTransaction) > 0)
                {
                    FoundFamilyMembers = true;
                }
                else
                {
                    FoundFamilyMembers = false;
                }

                if (FoundFamilyMembers)
                {
                    // Create a Parameter Row
//                  TLogging.LogAtLevel(9, "CheckFamilyMemberPropagation: Location " + APartnerLocationRow.LocationKey.ToString() + ": Partner is Family and has Family Members!");
                    AAddressAddedOrChangedPromotionDT = new PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable(
                        MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME);
                    AddressAddedOrChangedRow = AAddressAddedOrChangedPromotionDT.NewRowTyped(false);
                    AddressAddedOrChangedRow.SiteKey = ALocationPK.SiteKey;
                    AddressAddedOrChangedRow.LocationKey = ALocationPK.LocationKey;
                    AddressAddedOrChangedRow.LocationAdded = true;
                    AddressAddedOrChangedRow.LocationChange = false;
                    AddressAddedOrChangedRow.PartnerLocationChange = false;
                    AddressAddedOrChangedRow.AnswerProcessedClientSide = false;
                    AddressAddedOrChangedRow.AnswerProcessedServerSide = false;
                    AAddressAddedOrChangedPromotionDT.Rows.Add(AddressAddedOrChangedRow);
                    ReturnValue = true;
                }
                else
                {
//                  TLogging.LogAtLevel(9, "CheckFamilyMemberPropagation: Location " + APartnerLocationRow.LocationKey.ToString() + ": found no Family Members.");
                    ReturnValue = false;
                }
            }
            else
            {
                // AAddressAddedOrChangedPromotionDT was passed in, holding parameters for the LocationKey we are looking at
                AddressAddedOrChangedRow = (PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow)AddressAddedOrChangedPromotionDV[0].Row;

                if (AddressAddedOrChangedRow.UserAnswer == "YES")
                {
//                  TLogging.LogAtLevel(9, "CheckFamilyMemberPropagation: AAddressAddedOrChangedPromotionDT tells me to propagate the new Location to all Family Members.");
                    AddressAddedOrChangedRow.AnswerProcessedClientSide = true;
                    AddressAddedOrChangedRow.AcceptChanges();
                    ReturnValue = true;
                }
                else
                {
//                  TLogging.LogAtLevel(9, "CheckFamilyMemberPropagation: AAddressAddedOrChangedPromotionDT tells me NOT to propagate the new Location.");
                    AddressAddedOrChangedRow.AnswerProcessedClientSide = true;
                    AddressAddedOrChangedRow.AcceptChanges();
                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ALocationRow"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAddressAddedOrChangedPromotionDT"></param>
        /// <param name="AReadTransaction"></param>
        /// <param name="AUpdateLocation"></param>
        /// <param name="ACreateLocation"></param>
        /// <param name="ACreateLocationOtherPartnerKeys"></param>
        /// <param name="AChangePromotionParametersDT"></param>
        /// <returns></returns>
        private static Boolean CheckLocationChange(PLocationRow ALocationRow,
            Int64 APartnerKey,
            ref PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddressAddedOrChangedPromotionDT,
            TDBTransaction AReadTransaction,
            out Boolean AUpdateLocation,
            out Boolean ACreateLocation,
            out Int64[] ACreateLocationOtherPartnerKeys,
            out PartnerAddressAggregateTDSChangePromotionParametersTable AChangePromotionParametersDT)
        {
            Boolean ReturnValue;

            PartnerAddressAggregateTDSChangePromotionParametersRow AddressAddedPromotionRow;

            ACreateLocation = false;
            AUpdateLocation = false;

            ACreateLocationOtherPartnerKeys = null;
            AChangePromotionParametersDT = null;

//          TLogging.LogAtLevel(9, "CheckLocationChange for Location " + ALocationRow.LocationKey.ToString() + ": AAddressAddedOrChangedPromotionDT.Rows.Count: " +
//              AAddressAddedOrChangedPromotionDT.Rows.Count.ToString());

            // Check if there is a Parameter Row for the LocationKey we are looking at
            DataView AddressAddedOrChangedPromotionDV = new DataView(AAddressAddedOrChangedPromotionDT,
                PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName() + " = " + ALocationRow.SiteKey.ToString() +
                " AND " +
                PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName() + " = " + ALocationRow.LocationKey.ToString() +
                " AND " + PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationChangeDBName() + " = true",
                "",
                DataViewRowState.CurrentRows);

            // No, there isn't one  therefore create one
            if (AddressAddedOrChangedPromotionDV.Count == 0)
            {
//              TLogging.LogAtLevel(9, "CheckLocationChange: Location " + ALocationRow.LocationKey.ToString() +
//                  ": Location has been changed and is referenced by other Partners!");
                AAddressAddedOrChangedPromotionDT = new PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable(
                    MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME);
                PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow AddressAddedOrChangedRow =
                    AAddressAddedOrChangedPromotionDT.NewRowTyped(false);
                AddressAddedOrChangedRow.SiteKey = ALocationRow.SiteKey;
                AddressAddedOrChangedRow.LocationKey = ALocationRow.LocationKey;
                AddressAddedOrChangedRow.PartnerKey = APartnerKey;
                AddressAddedOrChangedRow.LocationChange = true;
                AddressAddedOrChangedRow.PartnerLocationChange = false;
                AddressAddedOrChangedRow.LocationAdded = false;
                AddressAddedOrChangedRow.AnswerProcessedClientSide = false;
                AddressAddedOrChangedRow.AnswerProcessedServerSide = false;
                AAddressAddedOrChangedPromotionDT.Rows.Add(AddressAddedOrChangedRow);
                #region Build ChangePromotionParameters DataTable
                AChangePromotionParametersDT = new PartnerAddressAggregateTDSChangePromotionParametersTable(
                    MPartnerConstants.ADDRESSCHANGEPROMOTIONPARAMETERS_TABLENAME);

                // Load data for all other Partners that reference the PartnerLocation
                OdbcParameter[] ParametersArray = new OdbcParameter[3];
                ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
                ParametersArray[0].Value = (System.Object)(APartnerKey);
                ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
                ParametersArray[1].Value = (System.Object)(ALocationRow.SiteKey);
                ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[2].Value = (System.Object)(ALocationRow.LocationKey);
                DataTable OtherPartnerLocationReferencesDT = DBAccess.GDBAccessObj.SelectDT(
                    "SELECT PUB_" + PPartnerLocationTable.GetTableDBName() + '.' + PPartnerLocationTable.GetPartnerKeyDBName() + ", " +
                    PPartnerTable.GetPartnerShortNameDBName() + ", " +
                    PPartnerTable.GetPartnerClassDBName() + ", " + PPartnerLocationTable.GetTelephoneNumberDBName() + ", " +
                    PPartnerLocationTable.GetLocationTypeDBName() + ' ' + "FROM PUB_" + PPartnerTable.GetTableDBName() + " INNER JOIN PUB_" +
                    PPartnerLocationTable.GetTableDBName() + " ON PUB_" + PPartnerTable.GetTableDBName() + '.' +
                    PPartnerTable.GetPartnerKeyDBName() +
                    " = PUB_" + PPartnerLocationTable.GetTableDBName() + '.' + PPartnerLocationTable.GetPartnerKeyDBName() + ' ' + "WHERE    PUB_" +
                    PPartnerLocationTable.GetTableDBName() + '.' + PPartnerLocationTable.GetPartnerKeyDBName() + " <> ? " + "AND    " +
                    PPartnerLocationTable.GetSiteKeyDBName() + " = ? " + "AND    " + PPartnerLocationTable.GetLocationKeyDBName() + " = ?",
                    "OtherPartnerLocationReferencesDT", AReadTransaction, ParametersArray);

                // Don't need these columns for the moment, but it would be nice to have them later on
                // PPartnerLocationTable.GetExtensionDBName + ', ' +
                // PPartnerLocationTable.GetFaxNumberDBName + ', ' +
                // PPartnerLocationTable.GetFaxExtensionDBName + ', ' +
                // PPartnerLocationTable.GetAlternateTelephoneDBName + ', ' +
                // PPartnerLocationTable.GetMobileNumberDBName + ', ' +
                // PPartnerLocationTable.GetEmailAddressDBName + ', ' +
                // PPartnerLocationTable.GetUrlDBName + ', ' +
                // PPartnerLocationTable.GetSendMailDBName + ', ' +
                // PPartnerLocationTable.GetDateEffectiveDBName + ', ' +
                // PPartnerLocationTable.GetDateGoodUntilDBName + ', ' +
                // Insert data into the ChangePromotionParameters DataTable
                for (int Counter = 0; Counter <= OtherPartnerLocationReferencesDT.Rows.Count - 1; Counter += 1)
                {
                    DataRow OtherPartnerLocationReferenceRow = OtherPartnerLocationReferencesDT.Rows[Counter];
                    AddressAddedPromotionRow = AChangePromotionParametersDT.NewRowTyped(false);
                    AddressAddedPromotionRow.SiteKey = ALocationRow.SiteKey;
                    AddressAddedPromotionRow.LocationKey = (Int32)ALocationRow.LocationKey;
                    AddressAddedPromotionRow.PartnerKey = Convert.ToInt64(
                        OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetPartnerKeyDBName()]);
                    AddressAddedPromotionRow.PartnerShortName = OtherPartnerLocationReferenceRow[PPartnerTable.GetPartnerShortNameDBName()].ToString();
                    AddressAddedPromotionRow.PartnerClass = OtherPartnerLocationReferenceRow[PPartnerTable.GetPartnerClassDBName()].ToString();
                    AddressAddedPromotionRow.TelephoneNumber =
                        OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetTelephoneNumberDBName()].ToString();

                    // Don't need these columns for the moment, but it would be nice to have them later on
                    // AddressAddedPromotionRow.Extension :=
                    // Convert.ToInt32(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetExtensionDBName]);
                    // AddressAddedPromotionRow.FaxNumber :=
                    // OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetFaxNumberDBName].ToString;
                    // AddressAddedPromotionRow.FaxExtension :=
                    // Convert.ToInt32(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetFaxExtensionDBName]);
                    // AddressAddedPromotionRow.AlternateTelephone :=
                    // OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetAlternateTelephoneDBName].ToString;
                    // AddressAddedPromotionRow.MobileNumber :=
                    // OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetMobileNumberDBName].ToString;
                    // AddressAddedPromotionRow.EmailAddress :=
                    // OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetEmailAddressDBName].ToString;
                    // AddressAddedPromotionRow.Url :=
                    // OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetUrlDBName].ToString;
                    // AddressAddedPromotionRow.SendMail :=
                    // Convert.ToBoolean(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetSendMailDBName]);
                    // if not OtherPartnerLocationReferenceRow.IsNull(
                    // PPartnerLocationTable.GetDateEffectiveDBName) then
                    // begin
                    // AddressAddedPromotionRow.DateEffective :=
                    // Convert.ToDateTime(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetDateEffectiveDBName]);
                    // end;
                    // if not OtherPartnerLocationReferenceRow.IsNull(
                    // PPartnerLocationTable.GetDateGoodUntilDBName) then
                    // begin
                    // AddressAddedPromotionRow.DateGoodUntil :=
                    // Convert.ToDateTime(OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetDateGoodUntilDBName]);
                    // end;
                    AddressAddedPromotionRow.LocationType = OtherPartnerLocationReferenceRow[PPartnerLocationTable.GetLocationTypeDBName()].ToString();
                    AChangePromotionParametersDT.Rows.Add(AddressAddedPromotionRow);
                }

//             TLogging.LogAtLevel(9, "CheckLocationChange: Location " + ALocationRow.LocationKey.ToString() + ": inserted PartnerLocation data of " +
//                  AChangePromotionParametersDT.Rows.Count.ToString() + " other Partners that reference this Location into AChangePromotionParametersDT!");
                #endregion
                ReturnValue = true;
            }
            else
            {
                // AAddressAddedOrChangedPromotionDT was passed in, holding parameters for the LocationKey we are looking at
                PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow AddressAddedOrChangedRow =
                    (PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow)AddressAddedOrChangedPromotionDV[0].Row;

                if (AddressAddedOrChangedRow.UserAnswer == "CHANGE-NONE")
                {
//                  TLogging.LogAtLevel(9, "CheckLocationChange: AAddressAddedOrChangedPromotionDT tells me to CREATE the Location.");
                    ACreateLocation = true;

                    // Signal to calling procedure that the created location should not be
                    // assigned to any Partner other than the one we are currently processing.
                    ACreateLocationOtherPartnerKeys = new Int64[0];
                    AddressAddedOrChangedRow.AnswerProcessedClientSide = true;
                    AddressAddedOrChangedRow.AcceptChanges();
                    ReturnValue = true;
                }
                else if (AddressAddedOrChangedRow.UserAnswer.StartsWith("CHANGE-SOME"))
                {
//                  TLogging.LogAtLevel(9, "CheckLocationChange: AAddressAddedOrChangedPromotionDT tells me to CREATE the Location and assign it to selected partners.");
                    ACreateLocation = true;

                    // Parse the UserAnswer. It's format is 'CHANGESOME:PartnerKey1;PartnerKey2;PartnerKeyN'
                    string[] ChangeSomeArray = AddressAddedOrChangedRow.UserAnswer.Split(":;".ToCharArray());

                    // Build the ACreateLocationOtherPartnerKeys array from it to
                    // signal to calling procedure that the created location should be
                    // assigned to all the Partners contained in the Array.
                    ACreateLocationOtherPartnerKeys = new Int64[ChangeSomeArray.Length - 1];

                    for (int Counter = 1; Counter <= ChangeSomeArray.Length - 1; Counter += 1)
                    {
                        ACreateLocationOtherPartnerKeys[Counter - 1] = Convert.ToInt64(ChangeSomeArray[Counter]);
                    }

                    AddressAddedOrChangedRow.AnswerProcessedClientSide = true;
                    AddressAddedOrChangedRow.AcceptChanges();
                    ReturnValue = true;
                }
                else if (AddressAddedOrChangedRow.UserAnswer == "CHANGE-ALL")
                {
//                  TLogging.LogAtLevel(9, "CheckLocationChange: AAddressAddedOrChangedPromotionDT tells me to UPDATE the Location.");
                    AUpdateLocation = true;
                    AddressAddedOrChangedRow.AnswerProcessedClientSide = true;
                    AddressAddedOrChangedRow.AcceptChanges();
                    ReturnValue = true;
                }
                else
                {
//                  TLogging.LogAtLevel(9, "CheckLocationChange: AAddressAddedOrChangedPromotionDT tells me to CANCEL the changing of the Location.");
                    AddressAddedOrChangedRow.AnswerProcessedClientSide = true;
                    AddressAddedOrChangedRow.AcceptChanges();
                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }

        private static bool SameValueOriginalCurrent(DataRow ADataRow, string AColumnName)
        {
            object obj1 = ADataRow[AColumnName, DataRowVersion.Original];
            object obj2 = ADataRow[AColumnName, DataRowVersion.Current];

            if ((obj1 == null) && (obj2 == null))
            {
                return true;
            }

            if ((obj1 == null) || (obj2 == null))
            {
                return false;
            }

            return obj1.ToString() == obj2.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="ALocationRow"></param>
        /// <returns></returns>
        private static Boolean CheckHasLocationChanged(PLocationRow ALocationRow)
        {
            Boolean ReturnValue;

            ReturnValue = false;

            // We only check Modified DataRows here
            if (ALocationRow.RowState == DataRowState.Modified)
            {
                // Check if any of the Addressholding fields were changed
                if (!SameValueOriginalCurrent(ALocationRow, PLocationTable.GetLocalityDBName())
                    || !SameValueOriginalCurrent(ALocationRow, PLocationTable.GetStreetNameDBName())
                    || !SameValueOriginalCurrent(ALocationRow, PLocationTable.GetAddress3DBName())
                    || !SameValueOriginalCurrent(ALocationRow, PLocationTable.GetCityDBName())
                    || !SameValueOriginalCurrent(ALocationRow, PLocationTable.GetCountyDBName())
                    || !SameValueOriginalCurrent(ALocationRow, PLocationTable.GetCountryCodeDBName())
                    || !SameValueOriginalCurrent(ALocationRow, PLocationTable.GetPostalCodeDBName()))
                {
                    ReturnValue = true;
//                  TLogging.LogAtLevel(9, "CheckHasLocationChanged: Location has changed.");
                }
                else
                {
//                  TLogging.LogAtLevel(9, "CheckHasLocationChanged: Location has NOT changed.");
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataRow"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AReadTransaction"></param>
        /// <returns></returns>
        public static Boolean CheckHasPartnerLocationOtherPartnerReferences(DataRow ADataRow, Int64 APartnerKey, TDBTransaction AReadTransaction)
        {
            Int64 SiteKey;
            Int32 LocationKey;

            // Determine LocationKey
            if (ADataRow.RowState != DataRowState.Deleted)
            {
                if (ADataRow is PLocationRow)
                {
                    SiteKey = Convert.ToInt64(ADataRow[PLocationTable.GetSiteKeyDBName(), DataRowVersion.Original]);
                    LocationKey = Convert.ToInt32(ADataRow[PLocationTable.GetLocationKeyDBName(), DataRowVersion.Original]);

                    // SiteKey := (ADataRow as PLocationRow).SiteKey;
                    // LocationKey := (ADataRow as PLocationRow).LocationKey;
                }
                else if (ADataRow is PPartnerLocationRow)
                {
                    SiteKey = ((PPartnerLocationRow)ADataRow).SiteKey;
                    LocationKey = ((PPartnerLocationRow)ADataRow).LocationKey;
                }
                else
                {
                    throw new ApplicationException(
                        "Expected a PLocationRow or a PPartnerLocationRow for ADataRow parameter, but received other DataRow");
                }
            }
            else
            {
                if (ADataRow is PLocationRow)
                {
                    SiteKey = Convert.ToInt64(ADataRow[PLocationTable.GetSiteKeyDBName(), DataRowVersion.Original]);
                    LocationKey = Convert.ToInt32(ADataRow[PLocationTable.GetLocationKeyDBName(), DataRowVersion.Original]);
                }
                else if (ADataRow is PPartnerLocationRow)
                {
                    SiteKey = Convert.ToInt64(ADataRow[PPartnerLocationTable.GetSiteKeyDBName(), DataRowVersion.Original]);
                    LocationKey = Convert.ToInt32(ADataRow[PPartnerLocationTable.GetLocationKeyDBName(), DataRowVersion.Original]);
                }
                else
                {
                    throw new ApplicationException(
                        "Expected a PLocationRow or a PPartnerLocationRow for ADataRow parameter, but received other DataRow");
                }
            }

            return CheckHasPartnerLocationOtherPartnerReferences(APartnerKey, SiteKey, LocationKey, AReadTransaction);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="ALocationKey"></param>
        /// <param name="AReadTransaction"></param>
        /// <returns></returns>
        public static Boolean CheckHasPartnerLocationOtherPartnerReferences(Int64 APartnerKey,
            Int64 ASiteKey,
            Int32 ALocationKey,
            TDBTransaction AReadTransaction)
        {
            Boolean ReturnValue;
            PPartnerLocationTable TemplateTable;
            PPartnerLocationRow TemplateRow;
            StringCollection TemplateOperators;
            int OtherPartnerLocationReferences;

//          TLogging.LogAtLevel(9, "CheckHasPartnerLocationOtherPartnerReferences for Location " + ALocationKey.ToString());

            if (ALocationKey != 0)
            {
                // Look for other Partners that reference the specified PartnerLocation
                TemplateTable = new PPartnerLocationTable();
                TemplateRow = TemplateTable.NewRowTyped(false);
                TemplateRow.PartnerKey = APartnerKey;
                TemplateRow.SiteKey = ASiteKey;
                TemplateRow.LocationKey = ALocationKey;
                TemplateOperators = new StringCollection();
                TemplateOperators.Add("<>");
                OtherPartnerLocationReferences = PPartnerLocationAccess.CountUsingTemplate(TemplateRow, TemplateOperators, AReadTransaction);
//              TLogging.LogAtLevel(9, "CheckHasPartnerLocationOtherPartnerReferences: Location " + ALocationKey.ToString() + ": is used by " +
//                  OtherPartnerLocationReferences.ToString() + " other Partners.");
                ReturnValue = (OtherPartnerLocationReferences > 0);
            }
            else
            {
                /*
                 * Special case: for Location 0 we always return 'is referenced' - it is a
                 * dummy record (signalising 'Partner has no address') that is seen as
                 * beeing referenced all the time (even if no Partner is referencing it).
                 */
//              TLogging.LogAtLevel(9, "CheckHasPartnerLocationOtherPartnerReferences: Location " + ALocationKey.ToString() +
//                  ": is Location 0, therefore it is seen as beeing used by other Partners.");
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataRow"></param>
        /// <param name="AChangedDetails"></param>
        /// <returns></returns>
        private static Boolean CheckHasPartnerLocationPromotionDetailChanged(PPartnerLocationRow ADataRow, out StringCollection AChangedDetails)
        {
            Boolean ReturnValue;

            AChangedDetails = new StringCollection();

            if (ADataRow.HasVersion(DataRowVersion.Original))
            {
                /*
                 * Check if one ore more of specific PartnerLocation Data-holding fields
                 * values were changed.
                 * If a field's value was changed, record the Field's DB Name, the Label of
                 * the Field (as shown on the screen), the Original value and the Current
                 * (new) value.
                 */

                // Check 'TelephoneNumber'
                if ((TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetTelephoneNumberDBName(), DataRowVersion.Original)).ToString()
                    != (TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetTelephoneNumberDBName(), DataRowVersion.Current)).ToString())
                {
                    AChangedDetails.Add(PPartnerLocationTable.GetTelephoneNumberDBName());
                    AChangedDetails.Add(TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnTelephoneNumberId));
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetTelephoneNumberDBName(),
                            DataRowVersion.Original).ToString());
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetTelephoneNumberDBName(),
                            DataRowVersion.Current).ToString());
                }

                // Check 'Extension'
                if ((TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetExtensionDBName(), DataRowVersion.Original)).ToString()
                    != (TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetExtensionDBName(), DataRowVersion.Current)).ToString())
                {
                    AChangedDetails.Add(PPartnerLocationTable.GetExtensionDBName());
                    AChangedDetails.Add(TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnExtensionId));
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow,
                            PPartnerLocationTable.GetExtensionDBName(), DataRowVersion.Original).ToString());
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow,
                            PPartnerLocationTable.GetExtensionDBName(), DataRowVersion.Current).ToString());
                }

                // Check 'FaxNumber'
                if ((TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetFaxNumberDBName(), DataRowVersion.Original)).ToString()
                    != (TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetFaxNumberDBName(), DataRowVersion.Current)).ToString())
                {
                    AChangedDetails.Add(PPartnerLocationTable.GetFaxNumberDBName());
                    AChangedDetails.Add(TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnFaxNumberId));
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow,
                            PPartnerLocationTable.GetFaxNumberDBName(), DataRowVersion.Original).ToString());
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow,
                            PPartnerLocationTable.GetFaxNumberDBName(), DataRowVersion.Current).ToString());
                }

                // Check 'FaxExtension'
                if ((TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetFaxExtensionDBName(), DataRowVersion.Original)).ToString()
                    != (TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetFaxExtensionDBName(), DataRowVersion.Current)).ToString())
                {
                    AChangedDetails.Add(PPartnerLocationTable.GetFaxExtensionDBName());
                    AChangedDetails.Add(TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnFaxExtensionId));
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetFaxExtensionDBName(),
                            DataRowVersion.Original).ToString());
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow,
                            PPartnerLocationTable.GetFaxExtensionDBName(), DataRowVersion.Current).ToString());
                }

                // Check 'AlternateTelephone'
                if ((TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetAlternateTelephoneDBName(), DataRowVersion.Original)).ToString()
                    != (TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetAlternateTelephoneDBName(),
                            DataRowVersion.Current)).ToString())
                {
                    AChangedDetails.Add(PPartnerLocationTable.GetAlternateTelephoneDBName());
                    AChangedDetails.Add(TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnAlternateTelephoneId));
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetAlternateTelephoneDBName(),
                            DataRowVersion.Original).ToString());
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetAlternateTelephoneDBName(),
                            DataRowVersion.Current).ToString());
                }

                // Check 'MobileNumber'
                if ((TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetMobileNumberDBName(), DataRowVersion.Original)).ToString()
                    != (TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetMobileNumberDBName(), DataRowVersion.Current)).ToString())
                {
                    AChangedDetails.Add(PPartnerLocationTable.GetMobileNumberDBName());
                    AChangedDetails.Add(TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnMobileNumberId));
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetMobileNumberDBName(),
                            DataRowVersion.Original).ToString());
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow,
                            PPartnerLocationTable.GetMobileNumberDBName(), DataRowVersion.Current).ToString());
                }

                // Check 'EmailAddress'
                if ((TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetEmailAddressDBName(), DataRowVersion.Original)).ToString()
                    != (TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetEmailAddressDBName(), DataRowVersion.Current)).ToString())
                {
                    AChangedDetails.Add(PPartnerLocationTable.GetEmailAddressDBName());
                    AChangedDetails.Add(TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnEmailAddressId));
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetEmailAddressDBName(),
                            DataRowVersion.Original).ToString());
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow,
                            PPartnerLocationTable.GetEmailAddressDBName(), DataRowVersion.Current).ToString());
                }

                // Check 'Url'
                if ((TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetUrlDBName(), DataRowVersion.Original)).ToString()
                    != (TTypedDataAccess.GetSafeValue(ADataRow, PPartnerLocationTable.GetUrlDBName(), DataRowVersion.Current)).ToString())
                {
                    AChangedDetails.Add(PPartnerLocationTable.GetUrlDBName());
                    AChangedDetails.Add(TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnUrlId));
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow,
                            PPartnerLocationTable.GetUrlDBName(), DataRowVersion.Original).ToString());
                    AChangedDetails.Add(TTypedDataAccess.GetSafeValue(ADataRow,
                            PPartnerLocationTable.GetUrlDBName(), DataRowVersion.Current).ToString());
                }

                // Result will be true if at least one field's value was changed
                ReturnValue = AChangedDetails.Count != 0;

/*
 *              if (ReturnValue)
 *              {
 *                  TLogging.LogAtLevel(9, "CheckHasPartnerLocationPromotionDetailChanged: " + Convert.ToInt16(AChangedDetails.Count / 4.0).ToString() + " promotable Location Detail(s) has/have got changed.");
 *              }
 *              else
 *              {
 *                  TLogging.LogAtLevel(9, "CheckHasPartnerLocationPromotionDetailChanged: NO promotable Location Detail has got changed.");
 *              }
 */
            }
            else
            {
                /*
                 * There is no Original data to access. This means it is a new DataRow.
                 * -> report it as *not* being changed
                 */
                ReturnValue = false;
//              TLogging.LogAtLevel(9, "CheckHasPartnerLocationPromotionDetailChanged: PPartnerLocationRow is new and not being seen as changed.");
            }

            return ReturnValue;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ALocationKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AReadTransaction"></param>
        /// <returns></returns>
        private static Boolean CheckHasPartnerOtherPartnerLocations(Int32 ALocationKey, Int64 APartnerKey, TDBTransaction AReadTransaction)
        {
            Int32[] OtherPartnerLocationKeys = new Int32[1];

            OtherPartnerLocationKeys[0] = ALocationKey;
            return CheckHasPartnerOtherPartnerLocations(OtherPartnerLocationKeys, APartnerKey, AReadTransaction);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ALocationKeys"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AReadTransaction"></param>
        /// <returns></returns>
        private static Boolean CheckHasPartnerOtherPartnerLocations(Int32[] ALocationKeys, Int64 APartnerKey, TDBTransaction AReadTransaction)
        {
            OdbcParameter[] ParametersArray;
            int Counter;
            Int16 OtherLocations;
            String LocationKeyInString = "";

            // Look if Partners has other PartnerLocations
            ParametersArray = new OdbcParameter[1 + ALocationKeys.Length];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = (System.Object)(APartnerKey);

            for (Counter = 0; Counter <= ALocationKeys.Length - 1; Counter += 1)
            {
                ParametersArray[Counter + 1] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[Counter + 1].Value = (System.Object)(ALocationKeys[Counter]);
                LocationKeyInString = LocationKeyInString + "?,";
            }

            // remove last comma
            LocationKeyInString = LocationKeyInString.Substring(0, LocationKeyInString.Length - 1);

            // Can't use the DataStore here since the query contains an IN operator...
            OtherLocations = Convert.ToInt16(DBAccess.GDBAccessObj.ExecuteScalar(
                    "SELECT COUNT(*) " + "FROM PUB_" + PPartnerLocationTable.GetTableDBName() + ' ' +
                    "WHERE " + PPartnerLocationTable.GetPartnerKeyDBName() + " = ? " +
                    "AND " + PPartnerLocationTable.GetLocationKeyDBName() + " NOT IN " +
                    "(" + LocationKeyInString + ")", AReadTransaction, false, ParametersArray));
//          TLogging.LogAtLevel(9, "CheckHasPartnerOtherPartnerLocations: Partner " + APartnerKey.ToString() + ": has " + OtherLocations.ToString() + " other PartnerLocations.");
            return OtherLocations != 0;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AExistingLocationParametersDT"></param>
        /// <param name="AChangeLocationParametersDT"></param>
        /// <param name="AAddressAddedOrChangedPromotionParametersDT"></param>
        private static void CheckParameterTables(PartnerAddressAggregateTDS AInspectDS,
            out PartnerAddressAggregateTDSSimilarLocationParametersTable AExistingLocationParametersDT,
            out PartnerAddressAggregateTDSChangePromotionParametersTable AChangeLocationParametersDT,
            out PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddressAddedOrChangedPromotionParametersDT)
        {
            if (AInspectDS != null)
            {
                if (AInspectDS.Tables.Contains(MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME))
                {
                    if (AInspectDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME] is
                        PartnerAddressAggregateTDSSimilarLocationParametersTable)
                    {
                        AExistingLocationParametersDT =
                            (PartnerAddressAggregateTDSSimilarLocationParametersTable)AInspectDS.Tables[MPartnerConstants.
                                                                                                        EXISTINGLOCATIONPARAMETERS_TABLENAME];

/* if DEBUGMODE
 *                      if (TLogging.DL >= 9)
 *                      {
 *                          TLogging.Log(
 *                              "CheckParameterTables: Passed in ParameterTable ''" + MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME +
 *                              "''; Rows.Count: " + AExistingLocationParametersDT.Rows.Count.ToString());
 *                      }
 *                      if (TLogging.DL >= 8)
 *                      {
 *                          for (Int16 TmpRowCounter = 0; TmpRowCounter <= AExistingLocationParametersDT.Rows.Count - 1; TmpRowCounter += 1)
 *                          {
 *                              TLogging.Log(
 *                                  "CheckParameterTables: AExistingLocationParametersDT: Row[" + TmpRowCounter.ToString() + "]: PLocationKey: " +
 *                                  AExistingLocationParametersDT[TmpRowCounter][PartnerAddressAggregateTDSSimilarLocationParametersTable.
 *                                                                               GetLocationKeyDBName(),
 *                                                                               DataRowVersion.Original].ToString() + "; PSiteKey: " +
 *                                  AExistingLocationParametersDT[TmpRowCounter][PartnerAddressAggregateTDSSimilarLocationParametersTable.
 *                                                                               GetSiteKeyDBName(),
 *                                                                               DataRowVersion.Original].ToString() + "; RowState: " +
 *                                  (Enum.GetName(typeof(DataRowState), AExistingLocationParametersDT.Rows[TmpRowCounter].RowState)));
 *                          }
 *                      }
 */
                    }
                    else
                    {
                        throw new ApplicationException("Expected Typed DataTable, received normal DataTable");
                    }
                }
                else
                {
                    AExistingLocationParametersDT = new PartnerAddressAggregateTDSSimilarLocationParametersTable(
                        MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME);
                }

                if (AInspectDS.Tables.Contains(MPartnerConstants.ADDRESSCHANGEPROMOTIONPARAMETERS_TABLENAME))
                {
                    if (AInspectDS.Tables[MPartnerConstants.ADDRESSCHANGEPROMOTIONPARAMETERS_TABLENAME] is
                        PartnerAddressAggregateTDSChangePromotionParametersTable)
                    {
                        AChangeLocationParametersDT =
                            (PartnerAddressAggregateTDSChangePromotionParametersTable)AInspectDS.Tables[MPartnerConstants.
                                                                                                        ADDRESSCHANGEPROMOTIONPARAMETERS_TABLENAME];
//                      TLogging.LogAtLevel(9, "CheckParameterTables: Passed in ParameterTable ''" + MPartnerConstants.ADDRESSCHANGEPROMOTIONPARAMETERS_TABLENAME +
//                          "''; Rows.Count: " + AChangeLocationParametersDT.Rows.Count.ToString());
                    }
                    else
                    {
                        throw new ApplicationException("Expected Typed DataTable, received normal DataTable");
                    }
                }
                else
                {
                    AChangeLocationParametersDT = new PartnerAddressAggregateTDSChangePromotionParametersTable(
                        MPartnerConstants.ADDRESSCHANGEPROMOTIONPARAMETERS_TABLENAME);
                }

                if (AInspectDS.Tables.Contains(MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME))
                {
                    if (AInspectDS.Tables[MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME] is
                        PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable)
                    {
                        AAddressAddedOrChangedPromotionParametersDT =
                            (PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable)AInspectDS.Tables
                            [MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME];
//                      TLogging.LogAtLevel(9, "CheckParameterTables: Passed in ParameterTable ''" + MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME +
//                                "''; Rows.Count: " + AAddressAddedOrChangedPromotionParametersDT.Rows.Count.ToString());
                    }
                    else
                    {
                        throw new ApplicationException("Expected Typed DataTable, received normal DataTable");
                    }
                }
                else
                {
                    AAddressAddedOrChangedPromotionParametersDT = new PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable(
                        MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME);
                }
            }
            else
            {
//              TLogging.LogAtLevel(9, "CheckParameterTables: Passed in ParameterDataSet is nil.");
                AExistingLocationParametersDT = new PartnerAddressAggregateTDSSimilarLocationParametersTable(
                    MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME);
                AChangeLocationParametersDT = new PartnerAddressAggregateTDSChangePromotionParametersTable(
                    MPartnerConstants.ADDRESSCHANGEPROMOTIONPARAMETERS_TABLENAME);
                AAddressAddedOrChangedPromotionParametersDT = new PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable(
                    MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ALocationRow"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AExistingLocationParametersDT"></param>
        /// <param name="AReadTransaction"></param>
        /// <param name="AExistingSiteKey"></param>
        /// <param name="AExistingLocationKey"></param>
        /// <returns></returns>
        private static Boolean CheckReUseExistingLocation(PLocationRow ALocationRow,
            Int64 APartnerKey,
            ref PartnerAddressAggregateTDSSimilarLocationParametersTable AExistingLocationParametersDT,
            TDBTransaction AReadTransaction,
            out Int64 AExistingSiteKey,
            out Int32 AExistingLocationKey)
        {
            Boolean ReturnValue;
            PLocationTable LocationTable;
            PLocationTable MatchingLocationsDT;
            PLocationRow LocationTemplateRow;
            Boolean FoundSimilarLocation;
            PartnerAddressAggregateTDSSimilarLocationParametersRow SimilarLocationRow;
            PartnerAddressAggregateTDSSimilarLocationParametersRow SimilarLocationParameterRow;
            DataView ExistingLocationParametersDV;
            int LocationUsedByNPartners;
            int Counter;
            PLocationRow MatchingLocationRow;

            AExistingSiteKey = 0;
            AExistingLocationKey = 0;

//              TLogging.LogAtLevel(9,  "CheckReUseExistingLocation for Location " + ALocationRow.LocationKey.ToString() +
//                  ": AExistingLocationParametersDT.Rows.Count: " + AExistingLocationParametersDT.Rows.Count.ToString());

            // Check if there is a Parameter Row for the LocationKey we are looking at
            ExistingLocationParametersDV = new DataView(AExistingLocationParametersDT,
                PartnerAddressAggregateTDSSimilarLocationParametersTable.GetSiteKeyDBName() + " = " + ALocationRow.SiteKey.ToString() + " AND " +
                PartnerAddressAggregateTDSSimilarLocationParametersTable.GetLocationKeyDBName() + " = " + ALocationRow.LocationKey.ToString(),
                "",
                DataViewRowState.CurrentRows);

            // No, there isn't one: perform DB check
            if (ExistingLocationParametersDV.Count == 0)
            {
                FoundSimilarLocation = false;
                LocationTable = ((PLocationTable)ALocationRow.Table);
                #region Look in the DB for *similar* Locations
                LocationTemplateRow = LocationTable.NewRowTyped(false);
                LocationTemplateRow.Locality = TSaveConvert.StringColumnToString(LocationTable.ColumnLocality, ALocationRow);
                LocationTemplateRow.StreetName = TSaveConvert.StringColumnToString(LocationTable.ColumnStreetName, ALocationRow);
                LocationTemplateRow.City = TSaveConvert.StringColumnToString(LocationTable.ColumnCity, ALocationRow);
                LocationTemplateRow.PostalCode = TSaveConvert.StringColumnToString(LocationTable.ColumnPostalCode, ALocationRow);
                LocationTemplateRow.CountryCode = TSaveConvert.StringColumnToString(LocationTable.ColumnCountryCode, ALocationRow);

                /*
                 * Note: County and Address3 are not searched for - we are looking for a
                 * Location that is *similar*!
                 */
                MatchingLocationsDT = PLocationAccess.LoadUsingTemplate(LocationTemplateRow, AReadTransaction);
                MatchingLocationRow = null;  // to avoid compiler warning

                if (MatchingLocationsDT.Rows.Count != 0)
                {
                    // check if any of the returned Rows is not the current Row
                    for (Counter = 0; Counter <= MatchingLocationsDT.Rows.Count - 1; Counter += 1)
                    {
                        if (MatchingLocationsDT[Counter].LocationKey != ALocationRow.LocationKey)
                        {
                            FoundSimilarLocation = true;
                            AExistingSiteKey = MatchingLocationsDT[Counter].SiteKey;
                            AExistingLocationKey = (int)MatchingLocationsDT[Counter].LocationKey;
                            MatchingLocationRow = (PLocationRow)MatchingLocationsDT[Counter];
                            break;
                        }
                    }
                }
                else
                {
                    FoundSimilarLocation = false;
                }

                #endregion

                if (FoundSimilarLocation)
                {
//                  TLogging.LogAtLevel(9,  "CheckReUseExistingLocation: Location " + ALocationRow.LocationKey.ToString() + ": found a similar Location (" + AExistingLocationKey.ToString() + ")!");
                    AExistingLocationParametersDT = new PartnerAddressAggregateTDSSimilarLocationParametersTable(
                        MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME);
                    LocationUsedByNPartners =
                        (Int16)(PPartnerLocationAccess.CountViaPLocation(AExistingSiteKey, AExistingLocationKey, AReadTransaction));
//                  TLogging.LogAtLevel(9, "CheckReUseExistingLocation: LocationUsedByNPartners: " + LocationUsedByNPartners.ToString());
                    SimilarLocationRow = AExistingLocationParametersDT.NewRowTyped(false);
                    SimilarLocationRow.SiteKey = ALocationRow.SiteKey;
                    SimilarLocationRow.LocationKey = ALocationRow.LocationKey;
                    SimilarLocationRow.Locality = TSaveConvert.StringColumnToString(MatchingLocationsDT.ColumnLocality, MatchingLocationRow);
                    SimilarLocationRow.StreetName = TSaveConvert.StringColumnToString(MatchingLocationsDT.ColumnStreetName, MatchingLocationRow);
                    SimilarLocationRow.Address3 = TSaveConvert.StringColumnToString(MatchingLocationsDT.ColumnAddress3, MatchingLocationRow);
                    SimilarLocationRow.City = TSaveConvert.StringColumnToString(MatchingLocationsDT.ColumnCity, MatchingLocationRow);
                    SimilarLocationRow.PostalCode = TSaveConvert.StringColumnToString(MatchingLocationsDT.ColumnPostalCode, MatchingLocationRow);
                    SimilarLocationRow.County = TSaveConvert.StringColumnToString(MatchingLocationsDT.ColumnCounty, MatchingLocationRow);
                    SimilarLocationRow.CountryCode = TSaveConvert.StringColumnToString(MatchingLocationsDT.ColumnCountryCode, MatchingLocationRow);

                    if (LocationUsedByNPartners > 0)
                    {
                        SimilarLocationRow.UsedByNOtherPartners = LocationUsedByNPartners;
                    }
                    else
                    {
                        SimilarLocationRow.UsedByNOtherPartners = 0;
                    }

                    SimilarLocationRow.SiteKeyOfSimilarLocation = AExistingSiteKey;
                    SimilarLocationRow.LocationKeyOfSimilarLocation = AExistingLocationKey;
                    SimilarLocationRow.AnswerProcessedClientSide = false;
                    SimilarLocationRow.AnswerProcessedServerSide = false;
                    AExistingLocationParametersDT.Rows.Add(SimilarLocationRow);
                    SimilarLocationRow.AcceptChanges();
                    ReturnValue = true;
                }
                else
                {
//                  TLogging.LogAtLevel(9,  "CheckReUseExistingLocation: Location " + ALocationRow.LocationKey.ToString() + ": found no similar Location.");
                    ReturnValue = false;
                }
            }
            else
            {
                // AExistingLocationParametersDT was passed in, holding parameters for the LocationKey we are looking at
                SimilarLocationParameterRow = (PartnerAddressAggregateTDSSimilarLocationParametersRow)ExistingLocationParametersDV[0].Row;

                if (SimilarLocationParameterRow.AnswerReuse)
                {
                    AExistingSiteKey = SimilarLocationParameterRow.SiteKeyOfSimilarLocation;
                    AExistingLocationKey = SimilarLocationParameterRow.LocationKeyOfSimilarLocation;
//                  TLogging.LogAtLevel(9, "CheckReUseExistingLocation: AExistingLocationParametersDT tells me to re-use existing Location " +
//                      AExistingLocationParametersDT[0].LocationKeyOfSimilarLocation.ToString() + '.');
                    SimilarLocationParameterRow.AnswerProcessedClientSide = true;

                    // SimilarLocationParameterRow.AcceptChanges;
                    ReturnValue = true;
                }
                else
                {
//                  TLogging.LogAtLevel(9, "CheckReUseExistingLocation: AExistingLocationParametersDT tells me NOT to re-use existing Location.");
                    SimilarLocationParameterRow.AnswerProcessedClientSide = true;

                    // SimilarLocationParameterRow.AcceptChanges;
                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Passes Location data as a Typed DataTable to the caller.
        ///
        /// </summary>
        /// <param name="ASiteKey">SiteKey of the Location for which Address data is to be loaded</param>
        /// <param name="ALocationKey">LocationKey of the Location for which Address data is to
        /// be loaded</param>
        /// <param name="AReadTransaction"></param>
        /// <returns>Typed DataTable PLocationTable containing one DataRow
        /// </returns>
        public static PLocationTable LoadByPrimaryKey(Int64 ASiteKey, Int32 ALocationKey, TDBTransaction AReadTransaction)
        {
            return PLocationAccess.LoadByPrimaryKey(ASiteKey, ALocationKey, AReadTransaction);
        }

        /// <summary>
        /// Passes PartnerLocation data as a Typed DataTable to the caller.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner for which PartnerLocation data is to be loaded</param>
        /// <param name="ASiteKey">SiteKey of the Location for which PartnerLocation data is to be loaded</param>
        /// <param name="ALocationKey">LocationKey of the Location for which PartnerLocation data is to
        /// be loaded</param>
        /// <param name="AReadTransaction"></param>
        /// <returns>Typed DataTable PPartnerLocation containing one DataRow
        /// </returns>
        public static PPartnerLocationTable LoadByPrimaryKey(Int64 APartnerKey, Int64 ASiteKey, Int32 ALocationKey, TDBTransaction AReadTransaction)
        {
            PPartnerLocationTable PPartnerLocationDT = PPartnerLocationAccess.LoadByPrimaryKey(APartnerKey, ASiteKey, ALocationKey, AReadTransaction);

            PLocationTable Dummy = null;

            ApplySecurity(ref PPartnerLocationDT, ref Dummy);

            return PPartnerLocationDT;
        }

        /// <summary>
        /// Passes Partner Address data as a DataSet to the caller. Loads the Address
        /// which is specified through ALocationKey.
        ///
        /// </summary>
        /// <param name="ADataSet">DataSet that holds a DataSet with a DataTable for the
        /// Person</param>
        /// <param name="APartnerKey">PartnerKey of the Partner for which Address data is to be
        /// loaded</param>
        /// <param name="ASiteKey">SiteKey of the Location for which Address data is to
        /// be loaded</param>
        /// <param name="ALocationKey">LocationKey of the Location for which Address data is to
        /// be loaded</param>
        /// <param name="AReadTransaction">Transaction for the SELECT statement
        /// </param>
        /// <returns>void</returns>
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 APartnerKey, Int64 ASiteKey, Int32 ALocationKey, TDBTransaction AReadTransaction)
        {
            PLocationTable LocationDT;
            PPartnerLocationTable PartnerLocationDT;

            PPartnerLocationAccess.LoadByPrimaryKey(ADataSet, APartnerKey, ASiteKey, ALocationKey, AReadTransaction);
            PLocationAccess.LoadByPrimaryKey(ADataSet, ASiteKey, ALocationKey, AReadTransaction);

            // Apply security
            LocationDT = (PLocationTable)ADataSet.Tables[PLocationTable.GetTableName()];
            PartnerLocationDT = (PPartnerLocationTable)ADataSet.Tables[PPartnerLocationTable.GetTableName()];
            ApplySecurity(ref PartnerLocationDT, ref LocationDT);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerLocationTable"></param>
        /// <param name="ATransaction"></param>
        private static void MakeSureLocation0IsNotPresent(Int64 APartnerKey,
            Int64 ASiteKey,
            PPartnerLocationTable APartnerLocationTable,
            TDBTransaction ATransaction)
        {
            PPartnerLocationTable TemplateDT;
            PPartnerLocationRow TemplateRow;

            if (APartnerLocationTable.Select(PPartnerLocationTable.GetPartnerKeyDBName() + " = " + APartnerKey.ToString() + " AND " +
                    PPartnerLocationTable.GetSiteKeyDBName() + " = " + ASiteKey.ToString() + " AND " + PPartnerLocationTable.GetSiteKeyDBName() +
                    " = 0", "",
                    DataViewRowState.Deleted).Length == 0)
            {
                TemplateDT = new PPartnerLocationTable();
                TemplateRow = TemplateDT.NewRowTyped(false);
                TemplateRow.PartnerKey = APartnerKey;
                TemplateRow.SiteKey = ASiteKey;
                TemplateRow.LocationKey = 0;

                /*
                 * Currently we need to do a count before issuing the delete command.
                 * If we don't do this, the DataStore raises a System.Data.Odbc.OdbcException
                 * if such a row doesn't exist. The DataStore should really be fixed, then
                 * counting before execution should not be necessary, which would be faster.
                 */
                if (Convert.ToInt32(PPartnerLocationAccess.CountUsingTemplate(TemplateRow, null, ATransaction)) > 0)
                {
                    PPartnerLocationAccess.DeleteUsingTemplate(TemplateRow, null, ATransaction);
//                  TLogging.LogAtLevel(9, "MakeSureLocation0IsNotPresent: Deleted PPartnerLoction that referenced Location 0.");
                }
            }
            else
            {
//              TLogging.LogAtLevel(9, "MakeSureLocation0IsNotPresent: Submitted DataSet contains a Deleted PPartnerLoction Location 0; will get deleted later.");
            }
        }

        /// Debugging - what comes into the function in Location and PartnerLocation
        private static void DebugLocationsBeforeSaving(DataSet AInspectDS)
        {
            string TmpDebugString = "";

            if (AInspectDS.Tables.Contains(PLocationTable.GetTableName()))
            {
                for (Int16 TmpRowCounter = 0;
                     TmpRowCounter <= AInspectDS.Tables[PLocationTable.GetTableName()].Rows.Count - 1;
                     TmpRowCounter += 1)
                {
                    if (AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpRowCounter].RowState != DataRowState.Deleted)
                    {
                        TmpDebugString = TmpDebugString + PLocationTable.GetTableName() + ".Row[" + TmpRowCounter.ToString() +
                                         "]: PLocationKey: " +
                                         AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpRowCounter][PLocationTable.
                                                                                                              GetLocationKeyDBName()].
                                         ToString() +
                                         "; PSiteKey: " +
                                         AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpRowCounter][PLocationTable.GetSiteKeyDBName()
                                         ].ToString() +
                                         "; RowState: " +
                                         (Enum.GetName(typeof(DataRowState),
                                              AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpRowCounter].RowState)) + "\r\n";
                    }
                    else
                    {
                        TmpDebugString = TmpDebugString + PLocationTable.GetTableName() + ".Row[" + TmpRowCounter.ToString() +
                                         "]: PLocationKey: " +
                                         AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpRowCounter][1,
                                                                                                              DataRowVersion.Original].
                                         ToString() + "; RowState: DELETED" + "\r\n";
                    }
                }
            }

            if (AInspectDS.Tables.Contains(PPartnerLocationTable.GetTableName()))
            {
                TmpDebugString = TmpDebugString + Environment.NewLine;

                for (Int16 TmpRowCounter = 0;
                     TmpRowCounter <= AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows.Count - 1;
                     TmpRowCounter += 1)
                {
                    if (AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter].RowState != DataRowState.Deleted)
                    {
                        TmpDebugString = TmpDebugString + PPartnerLocationTable.GetTableName() + ".Row[" + TmpRowCounter.ToString() +
                                         "]: PLocationKey: " +
                                         AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter][PPartnerLocationTable.
                                                                                                                     GetLocationKeyDBName()
                                         ].ToString() + "; PSiteKey: " +
                                         AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter][PPartnerLocationTable.
                                                                                                                     GetSiteKeyDBName()].
                                         ToString() + "; PPartnerKey: " +
                                         AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter][PPartnerLocationTable.
                                                                                                                     GetPartnerKeyDBName()
                                         ].ToString() + "; RowState: " +
                                         (Enum.GetName(typeof(DataRowState),
                                              AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter].RowState) + "\r\n");
                    }
                    else
                    {
                        TmpDebugString = TmpDebugString + PPartnerLocationTable.GetTableName() + ".Row[" + TmpRowCounter.ToString() +
                                         "]: PLocationKey: " +
                                         AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter][2,
                                                                                                                     DataRowVersion.Original]
                                         .ToString() + "; RowState: DELETED" + "\r\n";
                    }
                }

                TLogging.Log(
                    "SubmitChanges: PLocation / PPartnerLocation local contents: " + Environment.NewLine + TmpDebugString +
                    Environment.NewLine);
            }
        }

        /// <summary>
        /// Check each Location DataRow before calling SubmitChanges
        /// to enforce Business Rules:
        /// - Added or changed Location: check for a similar Location record
        /// - if no similar Location record exists, save this Location record
        /// - if a similar Location record exists: allow choosing whether the
        ///    existing one should be used, or this Location record should be saved
        /// - Changed Location: don't save Location record if the data is actually
        ///     the same than before
        /// - Deleted Location: delete Location only if no other PartnerLocation
        ///      is referencing it
        /// - Deleted Location: remove references from any Extracts
        /// </summary>
        private static TSubmitChangesResult ProcessLocationChanges(
            PLocationTable ALocationTable,
            PPartnerLocationTable APartnerLocationTable,
            ref PartnerAddressAggregateTDS AResponseDS,
            TDBTransaction ASubmitChangesTransaction,
            Int64 APartnerKey,
            ref PartnerAddressAggregateTDSSimilarLocationParametersTable AExistingLocationParametersDT,
            ref TLocationPK[, ] ASimilarLocationReUseKeyMapping,
            ref PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddressAddedOrChangedPromotionParametersDT,
            ref PartnerAddressAggregateTDSChangePromotionParametersTable AChangeLocationParametersDT,
            ref TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult Result = TSubmitChangesResult.scrOK;
            TSubmitChangesResult TmpResult;

            for (Int16 LocationCounter = 0; LocationCounter < ALocationTable.Rows.Count; LocationCounter++)
            {
                if ((ALocationTable.Rows[LocationCounter].RowState == DataRowState.Added)
                    || (ALocationTable.Rows[LocationCounter].RowState == DataRowState.Modified))
                {
                    if (ALocationTable[LocationCounter].LocationKey == 0)
                    {
                        throw new Exception("TPPartnerAddress.ProcessLocationChanges: must not add or modify the empty location");
                    }
                }

                if (ALocationTable.Rows[LocationCounter].RowState == DataRowState.Deleted)
                {
                    if (Convert.ToInt32(ALocationTable[LocationCounter][PLocationTable.GetLocationKeyDBName(),
                                                                        DataRowVersion.Original]) == 0)
                    {
                        throw new Exception("TPPartnerAddress.ProcessLocationChanges: must not delete the empty location");
                    }
                }

                if (ALocationTable.Rows[LocationCounter].RowState == DataRowState.Added)
                {
                    bool ReUseSimilarLocation = false;

                    // Check for reuse of a similar location in the DB
                    PLocationRow TmpRow = ALocationTable[LocationCounter];
                    TmpResult = PerformSimilarLocationReUseChecks(
                        ref TmpRow,
                        ref AResponseDS,
                        ASubmitChangesTransaction,
                        APartnerKey,
                        ref AExistingLocationParametersDT,
                        ref APartnerLocationTable,
                        ref ASimilarLocationReUseKeyMapping,
                        out ReUseSimilarLocation,
                        ref AVerificationResult);
//                  TLogging.LogAtLevel(8, "SubmitChanges: TmpRow.LocationKey after PerformSimilarLocationReUseChecks (1): " + TmpRow.LocationKey.ToString());

                    if (TmpResult != TSubmitChangesResult.scrOK)
                    {
                        // Stop processing here - we need a decision whether to re-use
                        // an existing Location or not (or the user tried to re-use a
                        // Location that is already used by this Partner, which is a
                        // user error)
                        return TmpResult;
                    }
                } // DataRowState.Added
                else if (ALocationTable.Rows[LocationCounter].RowState == DataRowState.Modified)
                {
                    if (CheckHasLocationChanged(ALocationTable[LocationCounter]))
                    {
                        bool ReUseSimilarLocation = false;

                        // Check for reuse of a similar location in the DB
                        PLocationRow TmpRow = ALocationTable[LocationCounter];
                        TmpResult = PerformSimilarLocationReUseChecks(ref TmpRow,
                            ref AResponseDS,
                            ASubmitChangesTransaction,
                            APartnerKey,
                            ref AExistingLocationParametersDT,
                            ref APartnerLocationTable,
                            ref ASimilarLocationReUseKeyMapping,
                            out ReUseSimilarLocation,
                            ref AVerificationResult);

//                      TLogging.LogAtLevel(9, "SubmitChanges: TmpRow.LocationKey after PerformSimilarLocationReUseChecks (2): " + TmpRow.LocationKey.ToString());

                        if (TmpResult != TSubmitChangesResult.scrOK)
                        {
                            // Stop processing here - we need a decision whether to re-use
                            // an existing Location or not (or the user tried to re-use a
                            // Location that is already used by this Partner, which is a
                            // user error)
                            return TmpResult;
                        }

                        if (!ReUseSimilarLocation)
                        {
                            // No similar Location exists, or an existing similar Location
                            // should not be reused
                            if (CheckHasPartnerLocationOtherPartnerReferences(ALocationTable[LocationCounter], APartnerKey,
                                    ASubmitChangesTransaction))
                            {
//                              TLogging.LogAtLevel(9, "SubmitChanges: Location " + ALocationTable[LocationCounter].LocationKey.ToString() + ": is used by other Partners as well.");

                                bool CreateLocationFlag;
                                TLocationPK OriginalLocationKey;

                                TmpResult =
                                    PerformLocationChangeChecks(ALocationTable[LocationCounter],
                                        APartnerKey,
                                        ref AResponseDS,
                                        ASubmitChangesTransaction,
                                        ref AAddressAddedOrChangedPromotionParametersDT,
                                        ref AChangeLocationParametersDT,
                                        ref APartnerLocationTable,
                                        ref AVerificationResult,
                                        out CreateLocationFlag,
                                        out OriginalLocationKey);

                                if (TmpResult != TSubmitChangesResult.scrOK)
                                {
                                    Result = TmpResult;

                                    if (Result == TSubmitChangesResult.scrError)
                                    {
                                        return Result;
                                    }
                                }

                                if (CreateLocationFlag)
                                {
                                    ModifyExistingLocationParameters(ALocationTable[LocationCounter],
                                        OriginalLocationKey,
                                        ref AExistingLocationParametersDT);

                                    // Make this location's DataRow undmodified because it should not be submitted to the database
                                    ALocationTable.Rows[LocationCounter].AcceptChanges();
                                    LocationCounter--;
                                }
                            } // if CheckHasPartnerLocationOtherPartnerReferences ... then

                        }
                    } // if CheckHasLocationChanged ... then
                    else
                    {
//                      TLogging.LogAtLevel(9, "Location " + ALocationTable[LocationCounter].LocationKey.ToString() + ": data has NOT changed -> will not be saved.");

                        // remove this location because it should not be submitted to the database
                        ALocationTable.Rows.RemoveAt(LocationCounter);
                        LocationCounter--;
                    }
                } // DataRowState.Modified
                else if (ALocationTable.Rows[LocationCounter].RowState == DataRowState.Deleted)
                {
//                  TLogging.LogAtLevel(9, "SubmitChanges: Location " + ALocationTable[LocationCounter]
//                      [PLocationTable.GetLocationKeyDBName(), DataRowVersion.Original].ToString() + ": has been marked for deletion.");

                    // Handle deletion of Location row: delete it only if no other PartnerLocation is referencing it
                    if (CheckHasPartnerLocationOtherPartnerReferences(ALocationTable[LocationCounter], APartnerKey, ASubmitChangesTransaction))
                    {
//                      TLogging.LogAtLevel(9,  TLogging.Log("SubmitChanges: Location " +
//                          ALocationTable[LocationCounter][PLocationTable.GetLocationKeyDBName(), DataRowVersion.Original].ToString() +
//                          ": has been marked for deletion and is used by others, so it won''t get deleted.");

                        // remove this location because it should not be submitted to the database
                        ALocationTable.Rows.RemoveAt(LocationCounter);
                        LocationCounter--;
                    }
                    else
                    {
//                      TLogging.LogAtLevel(9, "SubmitChanges: Location " + ALocationTable[LocationCounter][PLocationTable.GetLocationKeyDBName(),
//                          DataRowVersion.Original].ToString() + ": has been marked for deletion and will get deleted.");

                        // Any Extract in Petra that references this Location must no longer
                        // reference this Location since it will get deleted
                        if (!RemoveLocationFromExtracts(ALocationTable[LocationCounter], ASubmitChangesTransaction, ref AVerificationResult))
                        {
                            return TSubmitChangesResult.scrError;
                        }
                    }
                } // if LocationTable.Rows[LocationCounter].RowState = DataRowState.Deleted
                else if (ALocationTable.Rows[LocationCounter].RowState != DataRowState.Unchanged)
                {
                    throw new ArgumentException(
                        "SubmitChanges can only deal with Locations of DataRowState Added, Modified or Deleted, but not with " +
                        (Enum.GetName(typeof(DataRowState), ALocationTable.Rows[LocationCounter].RowState)));
                }
            }

            return Result;
        }

        /// <summary>
        ///  Check each PartnerLocation DataRow before calling SubmitChanges
        ///  to enforce Business Rules:
        ///  - Added PartnerLocation:
        ///  - if working with a PartnerLocation of a FAMILY:
        ///  - Added PartnerLocation: if working with a Location of a FAMILY: allow
        ///  choosing whether this PartnerLocation should be added to all PERSONs
        ///  in the FAMILY
        ///  - make sure that Location 0 is no longer mapped to this Partner.
        ///  - Modified Location:
        ///  - if working with a PartnerLocation of a FAMILY:
        ///  - check whether other Partners are referencing it, and if so,
        ///  allow choosing which of the Partners (or none or all) should be
        ///  affected by the change
        ///  - if the value in the DateGoodUntil column has changed, silently
        ///  update it for all PERSONs of a FAMILY that have the same LocationKey.
        ///  - Deleted PartnerLocation: check whether this is the last
        ///  PartnerLocation that is left for this Partner. If this is the case,
        ///  don't delete the PartnerLocation, but set it's LocationKey to 0.
        /// </summary>
        private static TSubmitChangesResult ProcessPartnerLocationChanges(
            PPartnerLocationTable PartnerLocationTable,
            ref PartnerAddressAggregateTDS AResponseDS,
            TDBTransaction ASubmitChangesTransaction,
            Int64 APartnerKey,
            String APartnerClass,
            ref TLocationPK[, ] ASimilarLocationReUseKeyMapping,
            ref PartnerAddressAggregateTDSSimilarLocationParametersTable AExistingLocationParametersDT,
            ref PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddressAddedOrChangedPromotionParametersDT,
            ref PartnerAddressAggregateTDSChangePromotionParametersTable AChangeLocationParametersDT,
            ref TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult Result = TSubmitChangesResult.scrOK;
            TSubmitChangesResult TmpResult;

            for (Int16 PartnerLocationCounter = 0; PartnerLocationCounter <= PartnerLocationTable.Rows.Count - 1; PartnerLocationCounter += 1)
            {
                switch (PartnerLocationTable.Rows[PartnerLocationCounter].RowState)
                {
                    case DataRowState.Added:

                        if (PartnerLocationTable[PartnerLocationCounter].LocationKey != 0)
                        {
                            /*
                             * PartnerLocation of a FAMILY: Family Members promotion
                             */
                            if (APartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
                            {
                                bool PerformPropagation = false;

                                TmpResult = PerformLocationFamilyMemberPropagationChecks(
                                    PartnerLocationTable[PartnerLocationCounter],
                                    ref AResponseDS,
                                    ASubmitChangesTransaction,
                                    APartnerKey,
                                    APartnerClass,
                                    ref AAddressAddedOrChangedPromotionParametersDT,
                                    ref PartnerLocationTable,
                                    AExistingLocationParametersDT,
                                    ASimilarLocationReUseKeyMapping,
                                    out PerformPropagation,
                                    ref AVerificationResult);

                                if (TmpResult != TSubmitChangesResult.scrOK)
                                {
                                    Result = TmpResult;

                                    if (TmpResult == TSubmitChangesResult.scrError)
                                    {
                                        return TmpResult;
                                    }
                                }

                                if (PerformPropagation)
                                {
                                    ModifyAddressAddedOrChangedParameters(PartnerLocationTable[PartnerLocationCounter],
                                        ref AAddressAddedOrChangedPromotionParametersDT);
                                }
                            }

                            /*
                             * Since a new Location has been added, we need to make sure that
                             * Location 0 is no longer mapped to this Partner!
                             */
                            MakeSureLocation0IsNotPresent(APartnerKey,
                                PartnerLocationTable[PartnerLocationCounter].SiteKey,
                                PartnerLocationTable,
                                ASubmitChangesTransaction);
                        }
                        else
                        {
                            MakeSureLocation0SavingIsAllowed(PartnerLocationTable[PartnerLocationCounter],
                                APartnerKey,
                                ASubmitChangesTransaction);
                        }

                        break;

                    case DataRowState.Modified:

                        /*
                         * PartnerLocation of a FAMILY: Family Members promotion
                         */
                        if (APartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
                        {
                            /*
                             * If certain PartnerLocation details are changed, give user the
                             * option to apply this change to some or all PERSONs of this FAMILY
                             * (independent of them having the same LocationKey!).
                             */
                            TSubmitChangesResult PerformPartnerLocationChangeChecksResult =
                                PerformPartnerLocationChangeChecks(PartnerLocationTable[PartnerLocationCounter],
                                    APartnerKey,
                                    ref AResponseDS,
                                    ASubmitChangesTransaction,
                                    ref AAddressAddedOrChangedPromotionParametersDT,
                                    ref AChangeLocationParametersDT,
                                    ref PartnerLocationTable,
                                    ref AVerificationResult);

                            if (PerformPartnerLocationChangeChecksResult != TSubmitChangesResult.scrOK)
                            {
                                if (PerformPartnerLocationChangeChecksResult == TSubmitChangesResult.scrError)
                                {
                                    return PerformPartnerLocationChangeChecksResult;
                                }
                            }

                            /*
                             * If the value in the DateGoodUntil column has changed, silently
                             * update it for all PERSONs of this FAMILY that have the same
                             * LocationKey.
                             */
                            if (TSaveConvert.ObjectToDate(PartnerLocationTable[PartnerLocationCounter][PPartnerLocationTable.
                                                                                                       GetDateGoodUntilDBName(),
                                                                                                       DataRowVersion.Original]) !=
                                TSaveConvert.ObjectToDate(PartnerLocationTable[PartnerLocationCounter][PPartnerLocationTable.
                                                                                                       GetDateGoodUntilDBName(),
                                                                                                       DataRowVersion.Current]))
                            {
//                              TLogging.LogAtLevel(8, "SubmitChanges: PartnerLocation of a FAMILY: DateGoodUntil has changed -> promoting change to FAMILY members...");

                                TVerificationResultCollection SingleVerificationResultCollection;

                                if (!PromoteToFamilyMembersDateGoodUntilChange(APartnerKey, PartnerLocationTable[PartnerLocationCounter],
                                        ASubmitChangesTransaction, out SingleVerificationResultCollection))
                                {
                                    AVerificationResult.AddCollection(SingleVerificationResultCollection);
                                    return TSubmitChangesResult.scrError;
                                }
                            }
                        }

                        break;

                    case DataRowState.Deleted:

                        /*
                         * PPartnerLocation must not get deleted if it is the last one of the
                         * Partner, but must get mapped to Location 0 instead!
                         */

                        // Make sure that Location 0 can never get deleted!
                        if (Convert.ToInt32(PartnerLocationTable[PartnerLocationCounter][PPartnerLocationTable.GetLocationKeyDBName(),
                                                                                         DataRowVersion.Original]) != 0)
                        {
                            // Some other Location than Location 0 is about to be deleted!
                            // Check in the in-memory PartnerLocation Table first...
                            DataRow[] ChangePartnerLocationKeyRows = PartnerLocationTable.Select(
                                PPartnerLocationTable.GetPartnerKeyDBName() + " = " + APartnerKey.ToString() + " AND " +
                                PPartnerLocationTable.GetLocationKeyDBName() + " <> " +
                                PartnerLocationTable[PartnerLocationCounter][PPartnerLocationTable.GetLocationKeyDBName(),
                                                                             DataRowVersion.Original].ToString(), "",
                                DataViewRowState.CurrentRows);

                            if (ChangePartnerLocationKeyRows.Length == 0)
                            {
                                // No PPartnerLocation that is not deleted is left in
                                // PartnerLocationTable > now check for deleted ones
                                DataView DeletedPartnerLocationsDV = new DataView(PartnerLocationTable, "", "", DataViewRowState.Deleted);
                                int[] DeletedPartnerLocationKeys = new int[DeletedPartnerLocationsDV.Count];

                                for (Int16 DeletedPartnerLocationsCounter = 0;
                                     DeletedPartnerLocationsCounter <= DeletedPartnerLocationsDV.Count - 1;
                                     DeletedPartnerLocationsCounter += 1)
                                {
                                    DeletedPartnerLocationKeys[DeletedPartnerLocationsCounter] =
                                        Convert.ToInt32(DeletedPartnerLocationsDV[DeletedPartnerLocationsCounter].Row[PPartnerLocationTable.
                                                                                                                      GetLocationKeyDBName(),
                                                                                                                      DataRowVersion.Original
                                            ]);
                                }

                                // now check in the DB as well
                                if (!CheckHasPartnerOtherPartnerLocations(DeletedPartnerLocationKeys, APartnerKey, ASubmitChangesTransaction))
                                {
                                    // 'Undelete' DataRow and make it point to Location 0
                                    // (dummy Location) > will get submitted lateron!
                                    PartnerLocationTable[PartnerLocationCounter].RejectChanges();
                                    PartnerLocationTable[PartnerLocationCounter].LocationKey = 0;
//                                  TLogging.LogAtLevel(8, "SubmitChanges: PPartnerLocation " +
//                                      PartnerLocationTable[PartnerLocationCounter][PPartnerLocationTable.GetLocationKeyDBName(),
//                                      DataRowVersion.Original].ToString() + ": was last PartnerLocation, so its LocationKey got set to 0 (will be submitted lateron)!");
                                }
                            }
                            else
                            {
                                // There is at least one PPartnerLocation that is not deleted
                                // left in PartnerLocationTable, so the current PPartnerLocation
                                // can't be the last one > nothing to do.
                            }
                        }
                        else
                        {
                            DataRow[] ChangePartnerLocationKeyRows = PartnerLocationTable.Select(
                                PPartnerLocationTable.GetPartnerKeyDBName() + " = " + APartnerKey.ToString() + " AND " +
                                PPartnerLocationTable.GetLocationKeyDBName() + " = 0 ", "", DataViewRowState.CurrentRows);
//                          TLogging.LogAtLevel(8, "SubmitChanges: ChangePartnerLocationKeyRows Length: " + Convert.ToInt16(ChangePartnerLocationKeyRows.Length).ToString());

                            if (ChangePartnerLocationKeyRows.Length != 0)
                            {
                                // remove this location because it should not be submitted to the database
                                PartnerLocationTable.Rows.RemoveAt(PartnerLocationCounter);
                                PartnerLocationCounter--;

//                              TLogging.LogAtLevel(8, "SubmitChanges: Extra Location 0 row won''t be submitted lateron");
                            }
                        }

                        break;

                    case DataRowState.Unchanged:
                        break;

                    default:
                        throw new ArgumentException(
                        "SubmitChanges can only deal with PartnerLocations of DataRowState Added, Modified or Deleted, but not with " +
                        (Enum.GetName(typeof(DataRowState), PartnerLocationTable.Rows[PartnerLocationCounter].RowState)));
                }
            }

            return Result;
        }

        /// <summary>
        /// Prepare the address changes. Check for the rules regarding locations
        /// </summary>
        public static TSubmitChangesResult PrepareChanges(PartnerEditTDS AInspectDS,
            Int64 APartnerKey,
            String APartnerClass,
            TDBTransaction ASubmitChangesTransaction,
            ref PartnerAddressAggregateTDS AResponseDS,
            out TVerificationResultCollection AVerificationResult)
        {
            PartnerAddressAggregateTDSSimilarLocationParametersTable ExistingLocationParametersDT;
            PartnerAddressAggregateTDSChangePromotionParametersTable ChangeLocationParametersDT;
            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AddressAddedOrChangedPromotionParametersDT;

            AVerificationResult = null;

            if (AInspectDS == null)
            {
//              TLogging.LogAtLevel(9, "SubmitChanges: AInspectDS = nil!");
                return TSubmitChangesResult.scrNothingToBeSaved;
            }

            AVerificationResult = new TVerificationResultCollection();
            TLocationPK[, ] SimilarLocationReUseKeyMapping = new TLocationPK[1, 2];

            if (AInspectDS.PLocation != null)
            {
//              TLogging.LogAtLevel(9, "SubmitChanges: PLocation Rows: " + AInspectDS.PLocation.Rows.Count.ToString());
            }

            if (AInspectDS.PPartnerLocation != null)
            {
//              TLogging.LogAtLevel(9, "SubmitChanges: PPartnerLocation Rows: " + AInspectDS.PPartnerLocation.Rows.Count.ToString());
            }

            if (TLogging.DL >= 8)
            {
                DebugLocationsBeforeSaving(AInspectDS);
            }

            // Check if Parameter Tables are passed in
            CheckParameterTables(AResponseDS,
                out ExistingLocationParametersDT,
                out ChangeLocationParametersDT,
                out AddressAddedOrChangedPromotionParametersDT);

            if (AInspectDS.PLocation != null)
            {
                TSubmitChangesResult result = ProcessLocationChanges(
                    AInspectDS.PLocation,
                    AInspectDS.PPartnerLocation,
                    ref AResponseDS,
                    ASubmitChangesTransaction,
                    APartnerKey,
                    ref ExistingLocationParametersDT,
                    ref SimilarLocationReUseKeyMapping,
                    ref AddressAddedOrChangedPromotionParametersDT,
                    ref ChangeLocationParametersDT,
                    ref AVerificationResult);

                if (result != TSubmitChangesResult.scrOK)
                {
                    // Stop processing here, we need more information!

                    return result;
                }
            }

            if (AInspectDS.PPartnerLocation != null)
            {
                TSubmitChangesResult result = ProcessPartnerLocationChanges(
                    AInspectDS.PPartnerLocation,
                    ref AResponseDS,
                    ASubmitChangesTransaction,
                    APartnerKey,
                    APartnerClass,
                    ref SimilarLocationReUseKeyMapping,
                    ref ExistingLocationParametersDT,
                    ref AddressAddedOrChangedPromotionParametersDT,
                    ref ChangeLocationParametersDT,
                    ref AVerificationResult);

                if (result != TSubmitChangesResult.scrOK)
                {
                    // Stop processing here, we need more information!

                    return result;
                }
            }

            /*
             * Actual saving of data
             */
            if (AInspectDS.PLocation != null)
            {
//              TLogging.LogAtLevel(9, "SubmitChanges: Length(SimilarLocationReUseKeyMapping): " + Convert.ToInt16(SimilarLocationReUseKeyMapping.GetLength(0)).ToString());

                if ((SimilarLocationReUseKeyMapping.GetLength(0) - 1) > 0)
                {
                    for (Int16 LocationReUseCounter = 1;
                         LocationReUseCounter <= SimilarLocationReUseKeyMapping.GetLength(0) - 1;
                         LocationReUseCounter += 1)
                    {
/* if DEBUGMODE
 *                      if (TLogging.DL >= 9)
 *                      {
 *                          TLogging.Log("LocationReUseCounter: " + LocationReUseCounter.ToString());
 *                          TLogging.Log(
 *                              "SubmitChanges: LocationReUseKeyMapping[" + LocationReUseCounter.ToString() +
 *                              ", 0].LocationKey: " +
 *                              SimilarLocationReUseKeyMapping[LocationReUseCounter, 0].LocationKey.ToString());
 *                          TLogging.Log(
 *                              "SubmitChanges: LocationReUseKeyMapping[" + LocationReUseCounter.ToString() +
 *                              ", 1].LocationKey: " +
 *                              SimilarLocationReUseKeyMapping[LocationReUseCounter, 1].LocationKey.ToString());
 *                      }
 */
                        PLocationRow ReUsedLocationDR =
                            (PLocationRow)AInspectDS.PLocation.Rows.Find(
                                new System.Object[] { SimilarLocationReUseKeyMapping[LocationReUseCounter, 0].SiteKey,
                                                      SimilarLocationReUseKeyMapping[LocationReUseCounter, 0].LocationKey });

                        if (ReUsedLocationDR != null)
                        {
                            // Overwrite the originally submitted Key with the one that
                            // replaces it. This is needed to have the correct Key on
                            // the Client side!
                            ReUsedLocationDR.SiteKey = SimilarLocationReUseKeyMapping[LocationReUseCounter, 1].SiteKey;
                            ReUsedLocationDR.LocationKey = SimilarLocationReUseKeyMapping[LocationReUseCounter, 1].LocationKey;

                            // Make the DataRow 'unchanged' so that it doesn't get saved in the
                            // SubmitChanges call for PLocation!
                            ReUsedLocationDR.AcceptChanges();

                            // Remember that the row should not be submitted lateron
                            // NotToBeSubmittedLocationRows.Add(ReUsedLocationDR);
                        }
                        else
                        {
                            throw new ApplicationException("ReUsedLocationDR for SiteKey " +
                                SimilarLocationReUseKeyMapping[LocationReUseCounter,
                                                               0].SiteKey.ToString() + " and LocationKey " +
                                SimilarLocationReUseKeyMapping[LocationReUseCounter, 0].LocationKey.ToString() + " could not be found!");
                        }
                    }
                }
            }

            if (AInspectDS.PPartnerLocation != null)
            {
//              TLogging.LogAtLevel(9, "SubmitChanges: Length(SimilarLocationReUseKeyMapping): " + Convert.ToInt16(SimilarLocationReUseKeyMapping.GetLength(0)).ToString());

                if ((SimilarLocationReUseKeyMapping.GetLength(0) - 1) > 0)
                {
                    for (Int16 LocationReUseCounter = 1;
                         LocationReUseCounter <= SimilarLocationReUseKeyMapping.GetLength(0) - 1;
                         LocationReUseCounter += 1)
                    {
//                      TLogging.LogAtLevel(9, "LocationReUseCounter: " + LocationReUseCounter.ToString());
//                      TLogging.LogAtLevel(9, "SubmitChanges: LocationReUseKeyMapping[" + LocationReUseCounter.ToString() + ", 0].LocationKey: " +
//                                SimilarLocationReUseKeyMapping[LocationReUseCounter, 0].LocationKey.ToString());
                        PPartnerLocationRow ReUsedPartnerLocationDR = (PPartnerLocationRow)AInspectDS.PPartnerLocation.Rows.Find(
                            new System.Object[] { APartnerKey, SimilarLocationReUseKeyMapping[LocationReUseCounter, 0].SiteKey,
                                                  SimilarLocationReUseKeyMapping[LocationReUseCounter, 0].LocationKey });

                        if (ReUsedPartnerLocationDR != null)
                        {
                            // Overwrite the originally submitted Key with the one that
                            // replaces it. This is needed to have the correct Key on
                            // the Client side!
                            ReUsedPartnerLocationDR.SiteKey = SimilarLocationReUseKeyMapping[LocationReUseCounter, 1].SiteKey;
                            ReUsedPartnerLocationDR.LocationKey = SimilarLocationReUseKeyMapping[LocationReUseCounter, 1].LocationKey;
                        }
                        else
                        {
                            throw new ApplicationException("ReUsedPartnerLocationDR for SiteKey " +
                                SimilarLocationReUseKeyMapping[LocationReUseCounter,
                                                               0].SiteKey.ToString() + " and LocationKey " +
                                SimilarLocationReUseKeyMapping[LocationReUseCounter, 0].LocationKey.ToString() + " could not be found!");
                        }
                    }
                }
            }

            return TSubmitChangesResult.scrOK;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerLocationRow"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ASubmitChangesTransaction"></param>
        private static void MakeSureLocation0SavingIsAllowed(PPartnerLocationRow APartnerLocationRow,
            Int64 APartnerKey,
            TDBTransaction ASubmitChangesTransaction)
        {
            DataRow[] ChangePartnerLocationKeyRows;
            PPartnerLocationTable PartnerLocationTable;

            // PPartnerLocation with LocationKey 0 must only be allowed to get
            // saved if the Partner indeed has no other PartnerLocations!
            // Check in the inmemory PartnerLocation Table first...
            PartnerLocationTable = (PPartnerLocationTable)APartnerLocationRow.Table;
            ChangePartnerLocationKeyRows = PartnerLocationTable.Select(
                PPartnerLocationTable.GetPartnerKeyDBName() + " = " + APartnerKey.ToString() + " AND " +
                PPartnerLocationTable.GetLocationKeyDBName() +
                " <> 0", "", DataViewRowState.CurrentRows);
//          TLogging.LogAtLevel(8, "MakeSureLocation0SavingIsAllowed: ChangePartnerLocationKeyRows Length: " + Convert.ToInt16(ChangePartnerLocationKeyRows.Length).ToString());

            if (ChangePartnerLocationKeyRows.Length == 0)
            {
                // now check in the DB as well
                if (!CheckHasPartnerOtherPartnerLocations(0, APartnerKey, ASubmitChangesTransaction))
                {
//                  TLogging.LogAtLevel(8, "MakeSureLocation0SavingIsAllowed: CheckHasPartnerOtherPartnerLocations found no other Locations.");

                    if (PPartnerLocationAccess.Exists(APartnerKey, APartnerLocationRow.SiteKey, 0, ASubmitChangesTransaction))
                    {
                        throw new ApplicationException(
                            "A new PPartnerLocation DataRow with LocationKey 0 was added, but the Partner already has a PPartnerLocation with LocationKey 0");
                    }
                    else
                    {
                        // PPartnerLocation with LocationKey 0 may get saved!
//                      TLogging.LogAtLevel(8, "MakeSureLocation0SavingIsAllowed: PPartnerLocation with LocationKey 0 was added and will be saved.");
                    }
                }
            }
            else
            {
                throw new ApplicationException(
                    "A new PPartnerLocation DataRow with LocationKey 0 was added, but the Partner already has a PPartnerLocation with LocationKey 0");
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerLocationRow"></param>
        /// <param name="AAddressAddedPromotionDT"></param>
        private static void ModifyAddressAddedOrChangedParameters(PPartnerLocationRow APartnerLocationRow,
            ref PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddressAddedPromotionDT)
        {
            DataView AddressAddedOrChangedParametersDV;
            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow AddressAddedOrChangedRow;

//          TLogging.LogAtLevel(9, "ModifyAddressAddedOrChangedParameters: Looking for ExistingLocationParameters with LocationKey " + APartnerLocationRow.LocationKey.ToString());

/* if DEBUGMODE
 *          if (TLogging.DL >= 8)
 *          {
 *              for (int TmpRowCounter = 0; TmpRowCounter <= AAddressAddedPromotionDT.Rows.Count - 1; TmpRowCounter += 1)
 *              {
 *                  TLogging.Log("Checking Row: " + TmpRowCounter.ToString());
 *                  TLogging.Log(
 *                      "ModifyAddressAddedOrChangedParameters: AAddressAddedPromotionDT.Row[" + TmpRowCounter.ToString() + ".RowState: " +
 *                      (Enum.GetName(typeof(DataRowState), AAddressAddedPromotionDT.Rows[TmpRowCounter].RowState)));
 *                  TLogging.Log(
 *                      "ModifyAddressAddedOrChangedParameters: before searching: Row[" + TmpRowCounter.ToString() + "]: PLocationKey: " +
 *                      AAddressAddedPromotionDT[TmpRowCounter][PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName(),
 *                                                              DataRowVersion.Current].ToString() + "; PSiteKey: " +
 *                      AAddressAddedPromotionDT[TmpRowCounter][PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName(),
 *                                                              DataRowVersion.Current].ToString() + "; RowState: " +
 *                      (Enum.GetName(typeof(DataRowState), AAddressAddedPromotionDT.Rows[TmpRowCounter].RowState)));
 *              }
 *          }
 */

            // Check if there is a Parameter Row for the LocationKey we are looking at
            AddressAddedOrChangedParametersDV = new DataView(AAddressAddedPromotionDT,
                PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName() + " = " +
                Convert.ToInt64(APartnerLocationRow[PLocationTable.GetSiteKeyDBName(),
                                                    DataRowVersion.Current]).ToString() + " AND " +
                PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName() +
                " = " + Convert.ToInt32(APartnerLocationRow[PLocationTable.GetLocationKeyDBName(), DataRowVersion.Current]).ToString(),
                "",
                DataViewRowState.CurrentRows);

            // There is a row like that: replace SiteKey and LocationKey!
            if (AddressAddedOrChangedParametersDV.Count != 0)
            {
                AddressAddedOrChangedRow = (PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow)AddressAddedOrChangedParametersDV[0].Row;
//              TLogging.LogAtLevel(9, "ModifyAddressAddedOrChangedParameters: Exchanging LocationKey " + AddressAddedOrChangedRow.LocationKey.ToString() +
//                  " with LocationKey " + APartnerLocationRow.LocationKey.ToString());
                AddressAddedOrChangedRow.SiteKey = APartnerLocationRow.SiteKey;
                AddressAddedOrChangedRow.LocationKey = APartnerLocationRow.LocationKey;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ALocationRow"></param>
        /// <param name="AOriginalLocationKey"></param>
        /// <param name="AExistingLocationParametersDT"></param>
        private static void ModifyExistingLocationParameters(PLocationRow ALocationRow,
            TLocationPK AOriginalLocationKey,
            ref PartnerAddressAggregateTDSSimilarLocationParametersTable AExistingLocationParametersDT)
        {
            DataView ExistingLocationParametersDV;
            PartnerAddressAggregateTDSSimilarLocationParametersRow SimilarLocationParameterRow;

//          TLogging.LogAtLevel(9, "ModifyExistingLocationParameters: Looking for ExistingLocationParameters with LocationKey " +
//              AOriginalLocationKey.LocationKey.ToString() + "; AExistingLocationParametersDT.Rows.Count: " + AExistingLocationParametersDT.Rows.Count.ToString());

/* if DEBUGMODE
 *          if (TLogging.DL >= 8)
 *          {
 *              for (int TmpRowCounter = 0; TmpRowCounter <= AExistingLocationParametersDT.Rows.Count - 1; TmpRowCounter += 1)
 *              {
 *                  TLogging.Log("Checking Row: " + TmpRowCounter.ToString());
 *                  TLogging.Log("ModifyExistingLocationParameters: SimilarLocationParameterRow[" + TmpRowCounter.ToString() + ".RowState: " +
 *                      (Enum.GetName(typeof(DataRowState), AExistingLocationParametersDT.Rows[TmpRowCounter].RowState)));
 *                  TLogging.Log(
 *                      "ModifyExistingLocationParameters: before searching: Row[" + TmpRowCounter.ToString() + "]: PLocationKey: " +
 *                      AExistingLocationParametersDT[TmpRowCounter][PartnerAddressAggregateTDSSimilarLocationParametersTable.GetLocationKeyDBName(),
 *                                                                   DataRowVersion.Current].ToString() + "; PSiteKey: " +
 *                      AExistingLocationParametersDT[TmpRowCounter][PartnerAddressAggregateTDSSimilarLocationParametersTable.GetSiteKeyDBName(),
 *                                                                   DataRowVersion.Current].ToString() + "; RowState: " +
 *                      (Enum.GetName(typeof(DataRowState), AExistingLocationParametersDT.Rows[TmpRowCounter].RowState)));
 *              }
 *          }
 */

            if (AExistingLocationParametersDT.Rows.Count != 0)
            {
                // Check if there is a Parameter Row for the LocationKey we are looking at
                // ExistingLocationParametersDV := new DataView(
                // AExistingLocationParametersDT,
                // PartnerAddressAggregateTDSSimilarLocationParametersTable.GetSiteKeyDBName +
                // ' = ' + Convert.ToInt64(ALocationRow[PLocationTable.GetSiteKeyDBName,
                // DataRowVersion.Original]).ToString +
                // ' AND ' +
                // PartnerAddressAggregateTDSSimilarLocationParametersTable.GetLocationKeyDBName +
                // ' = ' + Convert.ToInt32(ALocationRow[PLocationTable.GetLocationKeyDBName,
                // DataRowVersion.Original]).ToString, '', DataViewRowState.OriginalRows);
                ExistingLocationParametersDV = new DataView(AExistingLocationParametersDT,
                    PartnerAddressAggregateTDSSimilarLocationParametersTable.GetSiteKeyDBName() + " = " + AOriginalLocationKey.SiteKey.ToString() +
                    " AND " + PartnerAddressAggregateTDSSimilarLocationParametersTable.GetLocationKeyDBName() + " = " +
                    AOriginalLocationKey.LocationKey.ToString(),
                    "",
                    DataViewRowState.ModifiedOriginal);

                // There is a row like that: replace SiteKey and LocationKey!
                if (ExistingLocationParametersDV.Count != 0)
                {
                    SimilarLocationParameterRow = (PartnerAddressAggregateTDSSimilarLocationParametersRow)ExistingLocationParametersDV[0].Row;
//                  TLogging.LogAtLevel(9, "ModifyExistingLocationParameters: Exchanging LocationKey " + SimilarLocationParameterRow.LocationKey.ToString() +
//                      " with LocationKey " + ALocationRow.LocationKey.ToString());

/* if DEBUGMODE
 *                  if (TLogging.DL >= 8)
 *                  {
 *                      TLogging.Log("ModifyExistingLocationParameters: SimilarLocationParameterRow.RowState: " +
 *                          (Enum.GetName(typeof(DataRowState), SimilarLocationParameterRow.RowState)));
 *
 *                      if (SimilarLocationParameterRow.RowState == DataRowState.Added)
 *                      {
 *                          TLogging.Log("ModifyExistingLocationParameters (before modification): PLocationKey: " +
 *                              SimilarLocationParameterRow[PartnerAddressAggregateTDSSimilarLocationParametersTable.GetLocationKeyDBName()].ToString(
 *                                  ) +
 *                              "; PSiteKey: " +
 *                              SimilarLocationParameterRow[PartnerAddressAggregateTDSSimilarLocationParametersTable.GetSiteKeyDBName()].ToString() +
 *                              "; RowState: " + (Enum.GetName(typeof(DataRowState), SimilarLocationParameterRow.RowState)));
 *                      }
 *                      else if ((SimilarLocationParameterRow.RowState == DataRowState.Modified)
 || (SimilarLocationParameterRow.RowState == DataRowState.Unchanged))
 ||                     {
 ||                         TLogging.Log("ModifyExistingLocationParameters (before modification): PLocationKey: " +
 ||                             SimilarLocationParameterRow[PartnerAddressAggregateTDSSimilarLocationParametersTable.GetLocationKeyDBName(),
 ||                                                         DataRowVersion.Original].ToString() + "; PSiteKey: " +
 ||                             SimilarLocationParameterRow[PartnerAddressAggregateTDSSimilarLocationParametersTable.GetSiteKeyDBName(),
 ||                                                         DataRowVersion.Original].ToString() + "; RowState: " +
 ||                             (Enum.GetName(typeof(DataRowState), SimilarLocationParameterRow.RowState) + " (ORIGINAL)"));
 ||                         TLogging.Log("ModifyExistingLocationParameters (before modification): PLocationKey: " +
 ||                             SimilarLocationParameterRow[PartnerAddressAggregateTDSSimilarLocationParametersTable.GetLocationKeyDBName(),
 ||                                                         DataRowVersion.Current].ToString() + "; PSiteKey: " +
 ||                             SimilarLocationParameterRow[PartnerAddressAggregateTDSSimilarLocationParametersTable.GetSiteKeyDBName(),
 ||                                                         DataRowVersion.Current].ToString() + "; RowState: " +
 ||                             (Enum.GetName(typeof(DataRowState), SimilarLocationParameterRow.RowState) + " (CURRENT)"));
 ||                     }
 ||                 }
 */

                    // Now modify it so that it can be found later by function DetermineReplacedLocationPK!
                    SimilarLocationParameterRow.SiteKey = ALocationRow.SiteKey;
                    SimilarLocationParameterRow.LocationKey = ALocationRow.LocationKey;

/* if DEBUGMODE
 *                  if (TLogging.DL >= 8)
 *                  {
 *                      TLogging.Log("ModifyExistingLocationParameters: SimilarLocationParameterRow.RowState: " +
 *                          (Enum.GetName(typeof(DataRowState), SimilarLocationParameterRow.RowState)));
 *
 *                      if (SimilarLocationParameterRow.RowState == DataRowState.Added)
 *                      {
 *                          TLogging.Log("ModifyExistingLocationParameters (after modification): PLocationKey: " +
 *                              SimilarLocationParameterRow[PartnerAddressAggregateTDSSimilarLocationParametersTable.GetLocationKeyDBName()].ToString(
 *                                  ) +
 *                              "; PSiteKey: " +
 *                              SimilarLocationParameterRow[PartnerAddressAggregateTDSSimilarLocationParametersTable.GetSiteKeyDBName()].ToString() +
 *                              "; RowState: " + (Enum.GetName(typeof(DataRowState), SimilarLocationParameterRow.RowState)));
 *                      }
 *                      else if ((SimilarLocationParameterRow.RowState == DataRowState.Modified)
 || (SimilarLocationParameterRow.RowState == DataRowState.Unchanged))
 ||                     {
 ||                         TLogging.Log("ModifyExistingLocationParameters (after modification): PLocationKey: " +
 ||                             SimilarLocationParameterRow[PartnerAddressAggregateTDSSimilarLocationParametersTable.GetLocationKeyDBName(),
 ||                                                         DataRowVersion.Original].ToString() + "; PSiteKey: " +
 ||                             SimilarLocationParameterRow[PartnerAddressAggregateTDSSimilarLocationParametersTable.GetSiteKeyDBName(),
 ||                                                         DataRowVersion.Original].ToString() + "; RowState: " +
 ||                             (Enum.GetName(typeof(DataRowState), SimilarLocationParameterRow.RowState) + " (ORIGINAL)"));
 ||                         TLogging.Log("ModifyExistingLocationParameters (after modification): PLocationKey: " +
 ||                             SimilarLocationParameterRow[PartnerAddressAggregateTDSSimilarLocationParametersTable.GetLocationKeyDBName(),
 ||                                                         DataRowVersion.Current].ToString() + "; PSiteKey: " +
 ||                             SimilarLocationParameterRow[PartnerAddressAggregateTDSSimilarLocationParametersTable.GetSiteKeyDBName(),
 ||                                                         DataRowVersion.Current].ToString() + "; RowState: " +
 ||                             (Enum.GetName(typeof(DataRowState), SimilarLocationParameterRow.RowState) + " (CURRENT)"));
 ||                     }
 ||                 }
 */
                }
            }
            else
            {
//              TLogging.LogAtLevel(9, "ModifyExistingLocationParameters: No ExistingLocationParameters with LocationKey " + AOriginalLocationKey.LocationKey.ToString() + " found --> creating new one!");

                /*
                 * No such parameter row found -> create a 'fake' one!
                 *
                 * NOTE: This is a bit of a 'Hack', since normally only function
                 * CheckReUseExistingLocation creates such a DataRow. However, currently
                 * this 'Hack' is needed to make function DetermineReplacedLocationPK work
                 * even if function CheckReUseExistingLocation was never executed (because
                 * no similar Location was found)...
                 */
                SimilarLocationParameterRow = AExistingLocationParametersDT.NewRowTyped(false);
                SimilarLocationParameterRow.SiteKey = AOriginalLocationKey.SiteKey;
                SimilarLocationParameterRow.LocationKey = AOriginalLocationKey.LocationKey;
                SimilarLocationParameterRow.UsedByNOtherPartners = 0;
                SimilarLocationParameterRow.SiteKeyOfSimilarLocation = ALocationRow.SiteKey;
                SimilarLocationParameterRow.LocationKeyOfSimilarLocation = (int)ALocationRow.LocationKey;
                SimilarLocationParameterRow.AnswerProcessedClientSide = true;
                SimilarLocationParameterRow.AnswerProcessedServerSide = true;
                SimilarLocationParameterRow.AnswerReuse = false;
                AExistingLocationParametersDT.Rows.Add(SimilarLocationParameterRow);
                SimilarLocationParameterRow.AcceptChanges();

                // Now modify it so that it can be found later by function DetermineReplacedLocationPK!
                SimilarLocationParameterRow.SiteKey = ALocationRow.SiteKey;
                SimilarLocationParameterRow.LocationKey = ALocationRow.LocationKey;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AFamilyPartnerKey"></param>
        /// <param name="AReadTransaction"></param>
        /// <returns></returns>
        private static PPersonTable GetFamilyMemberPartnerKeys(Int64 AFamilyPartnerKey, TDBTransaction AReadTransaction)
        {
            PPersonTable TemplateDT;
            PPersonRow TemplateRow;
            StringCollection RequiredColumns;

//          TLogging.LogAtLevel(9, "GetFamilyMemberPartnerKeys for Family with PartnerKey " + AFamilyPartnerKey.ToString());
            TemplateDT = new PPersonTable();
            TemplateRow = TemplateDT.NewRowTyped(false);
            TemplateRow.FamilyKey = AFamilyPartnerKey;

            //
            // Retrieve the PartnerKeys of the Persons of the Family
            //
            RequiredColumns = new StringCollection();
            RequiredColumns.Add(PPersonTable.GetPartnerKeyDBName());
            return PPersonAccess.LoadUsingTemplate(TemplateRow, RequiredColumns, AReadTransaction);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ARow"></param>
        /// <param name="ALocationReUseKeyMapping"></param>
        /// <returns></returns>
        private static TLocationPK DetermineReplacedLocationPK(DataRow ARow, TLocationPK[, ] ALocationReUseKeyMapping)
        {
            TLocationPK ReturnValue;
            TLocationPK SubmittedLocationPK;
            Int64 SiteKey;
            Int32 LocationKey;
            int Counter;

            ReturnValue = null;
            SubmittedLocationPK = null;

            if (!((ARow is PLocationRow) || (ARow is PPartnerLocationRow)))
            {
                throw new System.ArgumentException("ARow Argument must be either of Type PLocationRow or of Type PPartnerLocationRow");
            }

            if (ARow is PLocationRow)
            {
                SiteKey = ((PLocationRow)ARow).SiteKey;
                LocationKey = (int)((PLocationRow)ARow).LocationKey;
            }
            else
            {
                SiteKey = ((PPartnerLocationRow)ARow).SiteKey;
                LocationKey = ((PPartnerLocationRow)ARow).LocationKey;
            }

            // Check if passed in Key can be found in the KeyMapping Array
            if ((ALocationReUseKeyMapping.GetLength(0) - 1) > 0)
            {
//              TLogging.LogAtLevel(9, "(Length(ALocationReUseKeyMapping): " + Convert.ToInt16(ALocationReUseKeyMapping.GetLength(0)).ToString());
                for (Counter = 1; Counter <= ALocationReUseKeyMapping.GetLength(0) - 1; Counter += 1)
                {
                    SubmittedLocationPK = ALocationReUseKeyMapping[Counter, 0];

                    if ((SubmittedLocationPK.LocationKey == LocationKey) && (SubmittedLocationPK.SiteKey == SiteKey))
                    {
                        // found passed in Key in the KeyMapping Array
                        ReturnValue = ALocationReUseKeyMapping[Counter, 1];
//                      TLogging.LogAtLevel(9, "DetermineReplacedLocationPK: Key found in Key Mapping.");
                        continue;
                    }
                }
            }

            if (ReturnValue == null)
            {
                // passed in Key not found in the KeyMapping Array
//              TLogging.LogAtLevel(9, "DetermineReplacedLocationPK: Key *not* found in Key Mapping");
                ReturnValue = new TLocationPK(SiteKey, LocationKey);
            }

//          TLogging.LogAtLevel(9, "DetermineReplacedLocationPK: ReturnValue.SiteKey: " + ReturnValue.SiteKey.ToString() + "; ReturnValue.LocationKey: " + ReturnValue.LocationKey.ToString());
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ARow"></param>
        /// <param name="AExistingLocationParametersDT"></param>
        /// <returns></returns>
        private static TLocationPK DetermineReplacedLocationPK(DataRow ARow,
            PartnerAddressAggregateTDSSimilarLocationParametersTable AExistingLocationParametersDT)
        {
            TLocationPK ReturnValue;
            Int64 SiteKey;
            Int32 LocationKey;
            PartnerAddressAggregateTDSSimilarLocationParametersRow ExistingLocationParametersDR;
            DataView ExistingLocationParametersDV;

            ReturnValue = null;

            if (!((ARow is PLocationRow) || (ARow is PPartnerLocationRow)))
            {
                throw new System.ArgumentException("ARow Argument must be either of Type PLocationRow or of Type PPartnerLocationRow");
            }

            if (ARow is PLocationRow)
            {
                SiteKey = ((PLocationRow)ARow).SiteKey;
                LocationKey = (int)((PLocationRow)ARow).LocationKey;
            }
            else
            {
                SiteKey = ((PPartnerLocationRow)ARow).SiteKey;
                LocationKey = ((PPartnerLocationRow)ARow).LocationKey;
            }

            if ((AExistingLocationParametersDT != null) && (AExistingLocationParametersDT.Rows.Count > 0))
            {
//              TLogging.LogAtLevel(9, "DetermineReplacedLocationPK: checking for LocationKey: " + LocationKey.ToString());

                // Check if there is a Parameter Row for the LocationKey we are looking at
                ExistingLocationParametersDV = new DataView(AExistingLocationParametersDT,
                    PartnerAddressAggregateTDSSimilarLocationParametersTable.GetSiteKeyDBName() + " = " + SiteKey.ToString() + " AND " +
                    PartnerAddressAggregateTDSSimilarLocationParametersTable.GetLocationKeyDBName() + " = " + LocationKey.ToString(),
                    "",
                    DataViewRowState.CurrentRows);

                if (ExistingLocationParametersDV.Count != 0)
                {
//                  TLogging.LogAtLevel(9, "DetermineReplacedLocationPK: Key found in AExistingLocationParametersDT");
                    ExistingLocationParametersDR = (PartnerAddressAggregateTDSSimilarLocationParametersRow)ExistingLocationParametersDV[0].Row;
                    ReturnValue =
                        new TLocationPK(Convert.ToInt64(ExistingLocationParametersDR[PartnerAddressAggregateTDSSimilarLocationParametersTable.
                                                                                     GetSiteKeyDBName(),
                                                                                     DataRowVersion.Original]),
                            Convert.ToInt32(ExistingLocationParametersDR[PartnerAddressAggregateTDSSimilarLocationParametersTable.
                                                                         GetLocationKeyDBName(),
                                                                         DataRowVersion.Original]));
                }
            }

            if (ReturnValue == null)
            {
                // passed in Key not found in AExistingLocationParametersDT
//              TLogging.LogAtLevel(9, "DetermineReplacedLocationPK: Key *not* found in AExistingLocationParametersDT");
                ReturnValue = new TLocationPK(SiteKey, LocationKey);
            }

//          TLogging.LogAtLevel(9, "DetermineReplacedLocationPK: Result.SiteKey: " + ReturnValue.SiteKey.ToString() + "; ReturnValue.LocationKey: " + ReturnValue.LocationKey.ToString());
            return ReturnValue;
        }

        /// <summary>
        /// Allows adding of a new Location of a FAMILY Partner to all PERSONS of that
        /// FAMILY.
        ///
        /// @comment Must only be called for Partners of Partner Class FAMILY - the
        /// function does no checks on that and will fail for other Partner Classes!
        /// </summary>
        /// <returns>void</returns>
        private static TSubmitChangesResult PerformLocationFamilyMemberPropagationChecks(PPartnerLocationRow APartnerLocationRow,
            ref PartnerAddressAggregateTDS AResponseDS,
            TDBTransaction ASubmitChangesTransaction,
            Int64 APartnerKey,
            String APartnerClass,
            ref PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddressAddedPromotionDT,
            ref PPartnerLocationTable APartnerLocationTable,
            PartnerAddressAggregateTDSSimilarLocationParametersTable AExistingLocationParametersDT,
            TLocationPK[, ] ALocationReUseKeyMapping,
            out Boolean APerformPropagation,
            ref TVerificationResultCollection AVerificationResult)
        {
//          TLogging.LogAtLevel(9, "PerformLocationFamilyMemberPropagationChecks for LocationKey: " + APartnerLocationRow.LocationKey.ToString() +
//                    "; AAddressAddedPromotionDT.Rows.Count: " + AAddressAddedPromotionDT.Rows.Count.ToString());

            APerformPropagation = false;
            TLocationPK SubmittedLocationPK = DetermineReplacedLocationPK(APartnerLocationRow, ALocationReUseKeyMapping);
            TLocationPK LocationPK = DetermineReplacedLocationPK(APartnerLocationRow, AExistingLocationParametersDT);

            if (CheckFamilyMemberPropagation(APartnerLocationRow, APartnerKey, APartnerClass, ref AAddressAddedPromotionDT, LocationPK,
                    ASubmitChangesTransaction))
            {
                // Check if there is a Parameter Row for the LocationKey we are looking at
                DataView PropagateLocationParametersDV = new DataView(AAddressAddedPromotionDT,
                    PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName() + " = " + LocationPK.SiteKey.ToString() +
                    " AND " +
                    PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName() + " = " +
                    LocationPK.LocationKey.ToString() +
                    " AND " + PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationAddedDBName() + " = true AND " +
                    PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetAnswerProcessedClientSideDBName() + " = false",
                    "",
                    DataViewRowState.CurrentRows);

                if (PropagateLocationParametersDV.Count > 0)
                {
//                  TLogging.LogAtLevel(9, "PerformLocationFamilyMemberPropagationChecks: Location " + APartnerLocationRow.LocationKey.ToString() + ": found Family Members, decision on propagation is needed.");

                    /*
                     * More information is needed (usually via user interaction)
                     * -> stop processing here and return parameters
                     * (usually used for UI interaction)
                     */
                    if (AResponseDS == null)
                    {
//                      TLogging.LogAtLevel(9, "PerformLocationFamilyMemberPropagationChecks: Creating AResponseDS.");
                        AResponseDS = new PartnerAddressAggregateTDS(MPartnerConstants.PARTNERADDRESSAGGREGATERESPONSE_DATASET);
                    }

//                  TLogging.LogAtLevel(9, "PerformLocationFamilyMemberPropagationChecks: AAddressAddedPromotionDT.Rows.Count: " + AAddressAddedPromotionDT.Rows.Count.ToString());
                    AResponseDS.Merge(AAddressAddedPromotionDT);
//                  TLogging.LogAtLevel(9, "PerformLocationFamilyMemberPropagationChecks: Merged AAddressAddedPromotionDT into AResponseDS.");
//                  TLogging.LogAtLevel(9, "PerformLocationFamilyMemberPropagationChecks: PerformLocationFamilyMemberPropagationChecks: AResponseDS.Tables[" +
//                            MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME + "].Rows.Count: " +
//                            AResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].Rows.Count.ToString());
                    return TSubmitChangesResult.scrInfoNeeded;
                }
                else
                {
//                  TLogging.LogAtLevel(9, "PerformLocationFamilyMemberPropagationChecks: Location " + APartnerLocationRow.LocationKey.ToString() + ": found Family Members and new Location should be propagated to them!");

                    /*
                     * Family Members were found and the new Location should be added to all
                     * of them!
                     */
                    APerformPropagation = true;

                    // Load all Persons of the Family
                    PPersonTable FamilyPersonsDT = PPersonAccess.LoadViaPFamily(APartnerKey, ASubmitChangesTransaction);

                    // Find PPartnerLocation row of the Family that we should process
                    PPartnerLocationRow FamilyPartnerLocationRow = (PPartnerLocationRow)APartnerLocationTable.Rows.Find(
                        new System.Object[] { APartnerKey, APartnerLocationRow.SiteKey,
                                              APartnerLocationRow.LocationKey });

                    if (FamilyPartnerLocationRow != null)
                    {
                        for (int Counter = 0; Counter <= FamilyPersonsDT.Rows.Count - 1; Counter += 1)
                        {
                            PPersonRow ProcessedPersonRow = FamilyPersonsDT[Counter];
//                          TLogging.LogAtLevel(9,  "PerformLocationFamilyMemberPropagationChecks: Person  " + ProcessedPersonRow.PartnerKey.ToString() + ": checking...");

                            // Check if Person doesn't already have the Location
                            if (PPartnerLocationAccess.Exists(ProcessedPersonRow.PartnerKey, SubmittedLocationPK.SiteKey,
                                    SubmittedLocationPK.LocationKey, ASubmitChangesTransaction))
                            {
//                              TLogging.LogAtLevel(9, "PerformLocationFamilyMemberPropagationChecks: Person  " + ProcessedPersonRow.PartnerKey.ToString() +
//                                  ": adding Location " + SubmittedLocationPK.LocationKey.ToString() + "...");

                                // Add a copy of the PartnerLocation data to the Person
                                PPartnerLocationRow AddPartnerLocationRow = APartnerLocationTable.NewRowTyped(false);
                                AddPartnerLocationRow.ItemArray = DataUtilities.DestinationSaveItemArray(AddPartnerLocationRow,
                                    FamilyPartnerLocationRow);
                                AddPartnerLocationRow.PartnerKey = ProcessedPersonRow.PartnerKey;
                                AddPartnerLocationRow.SiteKey = SubmittedLocationPK.SiteKey;
                                AddPartnerLocationRow.LocationKey = SubmittedLocationPK.LocationKey;
                                APartnerLocationTable.Rows.Add(AddPartnerLocationRow);

                                /*
                                 * If this Person has an PartnerLocation with LocationKey 0 (this
                                 * means that this was the only PartnerLocation so far), delete the
                                 * PartnerLocation with LocationKey 0.
                                 */
                                if (PPartnerLocationAccess.Exists(ProcessedPersonRow.PartnerKey, SubmittedLocationPK.SiteKey, 0,
                                        ASubmitChangesTransaction))
                                {
//                                  TLogging.LogAtLevel(9, "PerformLocationFamilyMemberPropagationChecks: Person  " + ProcessedPersonRow.PartnerKey.ToString() + ": had Location 0 assigned, deleting it.");
                                    PPartnerLocationAccess.DeleteByPrimaryKey(ProcessedPersonRow.PartnerKey,
                                        APartnerLocationRow.SiteKey,
                                        0,
                                        ASubmitChangesTransaction);
                                }
                            }
                            else
                            {
//                              TLogging.LogAtLevel(9, "PerformLocationFamilyMemberPropagationChecks: Person  " + ProcessedPersonRow.PartnerKey.ToString() +
//                                      ": already has Location " + SubmittedLocationPK.LocationKey.ToString() + " assigned.");
                            }
                        }
                    }
                    else
                    {
                        throw new ApplicationException(
                            "TPPartnerAddressAggregate.PerformLocationFamilyMemberPropagationChecks: PPartnerLocation record for Family is missing");
                    }
                }
            }
            else
            {
//              TLogging.LogAtLevel(9, "PerformLocationFamilyMemberPropagationChecks: Location " + SubmittedLocationPK.LocationKey.ToString() +
//                  ": Family either has no Family Members, or no propagation of the new Location is wanted. New Location will therefore only be added to the FAMILY.");

                /*
                 * Family either has no Family Members, or it has Members, but the decision
                 * was made that the new PartnerLocation should not be added to the Family
                 * Members.
                 * The new Location will therefore only be added to the FAMILY.
                 */
            }

            return TSubmitChangesResult.scrOK;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        private static TSubmitChangesResult PerformSimilarLocationReUseChecks(ref PLocationRow ALocationRow,
            ref PartnerAddressAggregateTDS AResponseDS,
            TDBTransaction ASubmitChangesTransaction,
            Int64 APartnerKey,
            ref PartnerAddressAggregateTDSSimilarLocationParametersTable AExistingLocationParametersDT,
            ref PPartnerLocationTable APartnerLocationTable,
            ref TLocationPK[, ] ALocationReUseKeyMapping,
            out Boolean AReUseSimilarLocation,
            ref TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult ReturnValue;
            PPartnerLocationRow PartnerLocationCheckRow;
            DataView ExistingLocationParametersDV;

            PLocationRow ExistingLocationRow;
            Int64 CurrentSiteKey;
            Int64 ExistingSiteKey;
            Int32 CurrentLocationKey;
            Int32 ExistingLocationKey;

            PLocationTable SimilarLocationDT;

//          TLogging.LogAtLevel(9, "PerformSimilarLocationReUseChecks: AExistingLocationParametersDT.Rows.Count: " +
//              AExistingLocationParametersDT.Rows.Count.ToString());
            AReUseSimilarLocation = false;

            if (CheckReUseExistingLocation(ALocationRow, APartnerKey, ref AExistingLocationParametersDT, ASubmitChangesTransaction,
                    out ExistingSiteKey, out ExistingLocationKey))
            {
                // Check if there is a Parameter Row for the LocationKey we are looking at
                ExistingLocationParametersDV = new DataView(AExistingLocationParametersDT,
                    PartnerAddressAggregateTDSSimilarLocationParametersTable.GetSiteKeyDBName() + " = " + ALocationRow.SiteKey.ToString() + " AND " +
                    PartnerAddressAggregateTDSSimilarLocationParametersTable.GetLocationKeyDBName() + " = " + ALocationRow.LocationKey.ToString() +
                    " AND " + PartnerAddressAggregateTDSSimilarLocationParametersTable.GetAnswerProcessedClientSideDBName() + " = false",
                    "",
                    DataViewRowState.CurrentRows);

                if (ExistingLocationParametersDV.Count > 0)
                {
//                  TLogging.LogAtLevel(9, "PerformSimilarLocationReUseChecks: Location " + ALocationRow.LocationKey.ToString() + ": found similar Location, decision is needed.");

                    /*
                     * More information is needed (usually via user interaction)
                     * -> stop processing here and return parameters
                     * (usually used for UI interaction)
                     */
                    if (AResponseDS == null)
                    {
//                      TLogging.LogAtLevel(9, "PerformSimilarLocationReUseChecks: Creating AResponseDS.");
                        AResponseDS = new PartnerAddressAggregateTDS(MPartnerConstants.PARTNERADDRESSAGGREGATERESPONSE_DATASET);
                    }

//                  TLogging.LogAtLevel(9, "PerformSimilarLocationReUseChecks: AExistingLocationParametersDT.Rows.Count: " + AExistingLocationParametersDT.Rows.Count.ToString());
                    AResponseDS.Merge(AExistingLocationParametersDT);
//                  TLogging.LogAtLevel(9, "PerformSimilarLocationReUseChecks: Merged ExistingLocationParametersDT into AResponseDS.");
//                  TLogging.LogAtLevel(9, "PerformSimilarLocationReUseChecks: AResponseDS.Tables[" + MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME +
//                            "].Rows.Count: " + AResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].Rows.Count.ToString());
                    return TSubmitChangesResult.scrInfoNeeded;
                }
                else
                {
//                  TLogging.LogAtLevel(9, "PerformSimilarLocationReUseChecks: Location " + ALocationRow.LocationKey.ToString() +
//                      ": found similar Location and this one (" + ExistingLocationKey.ToString() + ") should be used instead of creating a new one!");

                    /*
                     * Location with the same data already exists and it should be
                     * re-used instead of creating a new one!
                     */

                    // Keep a mapping of the initially submitted LocationKey to the newly assigned one
                    ALocationReUseKeyMapping = new TLocationPK[ALocationReUseKeyMapping.GetLength(0) + 1, 2];
                    ALocationReUseKeyMapping[(ALocationReUseKeyMapping.GetLength(0)) - 1, 0] = new TLocationPK(ALocationRow.SiteKey,
                        (int)ALocationRow.LocationKey);
                    ALocationReUseKeyMapping[(ALocationReUseKeyMapping.GetLength(0)) - 1, 1] = new TLocationPK(ExistingSiteKey, ExistingLocationKey);
                    AReUseSimilarLocation = true;

                    if (!AExistingLocationParametersDT[0].AnswerProcessedServerSide)
                    {
                        AExistingLocationParametersDT[0].AnswerProcessedServerSide = true;

                        // Preserve Key of current Location
                        CurrentSiteKey = ALocationRow.SiteKey;
                        CurrentLocationKey = (int)ALocationRow.LocationKey;

                        /*
                         * Make sure that the Partner hasn't already got a PartnerLocation with
                         * the same Key (neither in memory nor in the DB)
                         */
//                      TLogging.LogAtLevel(9, "PerformSimilarLocationReUseChecks: Finding PartnerLocation Row in APartnerLocationTable with LocationKey " + ALocationRow.LocationKey.ToString());
                        PartnerLocationCheckRow =
                            (PPartnerLocationRow)APartnerLocationTable.Rows.Find(new object[] { APartnerKey, ALocationRow.SiteKey,
                                                                                                ALocationRow.LocationKey });

                        if (PartnerLocationCheckRow != null)
                        {
                            /*
                             * Checks in Memory: look for Current (ie. unchanged, new or edited)
                             * rows first whether they are the Location that is about to being
                             * reused. Secondly, check if there is a deleted Location with the same
                             * LocationKey that is about to being reused; only if this is not the
                             * case:
                             * Check in the DB whether the Partner hasn't got the Location
                             * with the same LocationKey that is about to being reuse.
                             */
                            if ((APartnerLocationTable.Select(PPartnerLocationTable.GetPartnerKeyDBName() + " = " + APartnerKey.ToString() +
                                     " AND " + PPartnerLocationTable.GetSiteKeyDBName() + " = " + ExistingSiteKey.ToString() + " AND " +
                                     PPartnerLocationTable.GetLocationKeyDBName() + " = " + ExistingLocationKey.ToString(), "",
                                     DataViewRowState.CurrentRows).Length != 0)
                                || ((APartnerLocationTable.Select(PPartnerLocationTable.GetPartnerKeyDBName() + " = " + APartnerKey.ToString() +
                                         " AND " +
                                         PPartnerLocationTable.GetSiteKeyDBName() + " = " + ExistingSiteKey.ToString() + " AND " +
                                         PPartnerLocationTable.GetLocationKeyDBName() + " = " + ExistingLocationKey.ToString(), "",
                                         DataViewRowState.Deleted).Length == 0)
                                    && (PPartnerLocationAccess.Exists(APartnerKey, ExistingSiteKey, ExistingLocationKey,
                                            ASubmitChangesTransaction))))
                            {
                                AVerificationResult.Add(new TVerificationResult("[Partner Address Save]", "Partner " + APartnerKey.ToString() +
                                        " already has a " + "record linked with Location " + ExistingLocationKey.ToString() + Environment.NewLine +
                                        "Unable to save.", "Duplicate Address Entered", "", TResultSeverity.Resv_Critical));
                                ReturnValue = TSubmitChangesResult.scrError;
                                return ReturnValue;
                            }
                            else
                            {
//                              TLogging.LogAtLevel(9, "PerformSimilarLocationReUseChecks: LocationKey: " + ExistingLocationKey.ToString() + " will later get assigned to PPartnerLocation.");
                            }
                        }
                        else
                        {
                            throw new ApplicationException(
                                "PerformSimilarLocationReUseChecks: PartnerLocationCheckRow with SiteKey " + ALocationRow.SiteKey.ToString() +
                                " and LocationKey " +
                                ALocationRow.LocationKey.ToString() + " not found!");
                        }

                        /*
                         * Copy all fields from the existing Location to the current Location
                         */
                        SimilarLocationDT = PLocationAccess.LoadByPrimaryKey(ExistingSiteKey, ExistingLocationKey, null, ASubmitChangesTransaction);

                        if (SimilarLocationDT.Rows.Count != 0)
                        {
                            ExistingLocationRow = (PLocationRow)SimilarLocationDT.Rows[0];

                            ALocationRow.ItemArray = ExistingLocationRow.ItemArray;

                            /*
                             * NOTE: The SiteKey and LocationKey are re-assigned to the ones of the
                             * current Location. This is done to have the current SiteKey and
                             * LocationKey preserved throughout the whole process of working with the
                             * Locations. The SiteKey and LocationKey are exchanged with the ones of
                             * the existing Location before the DataRow gets sent back to the Client!
                             */
                            ALocationRow.SiteKey = CurrentSiteKey;
                            ALocationRow.LocationKey = CurrentLocationKey;
//                          TLogging.LogAtLevel(9, "CheckReUseExistingLocation: Location " + ALocationRow.LocationKey.ToString() +
//                                ": data got replaced with data from the existing Location (" + ExistingLocationKey.ToString() + ")!");
                        }
                        else
                        {
                            throw new ApplicationException(
                                "Couldn''t find existing Similar Location with SiteKey " + ALocationRow.SiteKey.ToString() + " and LocationKey " +
                                ALocationRow.LocationKey.ToString() + '!');
                        }
                    }
                }
            }
            else
            {
//              TLogging.LogAtLevel(9, "CheckReUseExistingLocation: Location " + ALocationRow.LocationKey.ToString() +
//                  ": Location does not exist yet (or an existing Location should not be re-used) -> will get saved later.");

                /*
                 * No similar Location exists, or an existing similar Location should
                 * not be re-used: Save this Location
                 * -> will get saved later in call to SubmitChanges
                 */
            }

            return TSubmitChangesResult.scrOK;
        }

        /// <summary>
        /// todoComment
        ///
        /// @comment Must only be called for Partners of Partner Class FAMILY - the
        /// function does no checks on that and will fail for other Partner Classes!
        /// </summary>
        /// <param name="APartnerLocationRow"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AResponseDS"></param>
        /// <param name="ASubmitChangesTransaction"></param>
        /// <param name="AAddressChangedPromotionDT"></param>
        /// <param name="AChangeLocationParametersDT"></param>
        /// <param name="APartnerLocationTable"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private static TSubmitChangesResult PerformPartnerLocationChangeChecks(PPartnerLocationRow APartnerLocationRow,
            Int64 APartnerKey,
            ref PartnerAddressAggregateTDS AResponseDS,
            TDBTransaction ASubmitChangesTransaction,
            ref PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddressChangedPromotionDT,
            ref PartnerAddressAggregateTDSChangePromotionParametersTable AChangeLocationParametersDT,
            ref PPartnerLocationTable APartnerLocationTable,
            ref TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult ReturnValue;
            DataView PropagateLocationParametersDV;
            DataView PropagateLocationParametersProcessedDV;
            Boolean UpdatePartnerLocations;

            Int64[, ] UpdatePartnerLocationOtherPersons;
            OdbcParameter[] ParametersArray;
            DataSet PartnerLocationModifyDS;
            TVerificationResultCollection SingleVerificationResultCollection;
            StringCollection ChangedFieldsColl;
            String[] ChangedFieldsArr;
            Int32 Counter;
            Int32 Counter2;
            Int32 Counter3;
            Int32 Counter4;
            PPartnerLocationTable PartnerLocationModificationDT;
//          TLogging.LogAtLevel(9, "PerformPartnerLocationChangeChecks: AAddressChangedPromotionDT.Rows.Count: " + AAddressChangedPromotionDT.Rows.Count.ToString());

            UpdatePartnerLocations = false;

            if (CheckPartnerLocationChange(APartnerLocationRow, APartnerKey, ref AAddressChangedPromotionDT, ASubmitChangesTransaction,
                    out UpdatePartnerLocations, out UpdatePartnerLocationOtherPersons, ref AChangeLocationParametersDT))
            {
                // Check if there is a Parameter Row for the LocationKey we are looking at
                PropagateLocationParametersDV = new DataView(AAddressChangedPromotionDT,
                    PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName() + " = " +
                    APartnerLocationRow[PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName(),
                                        DataRowVersion.Original].ToString() + " AND " +
                    PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName() +
                    " = " +
                    APartnerLocationRow[PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName(),
                                        DataRowVersion.Original].ToString() + " AND " +
                    PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetPartnerLocationChangeDBName() + " = true AND " +
                    PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetAnswerProcessedClientSideDBName() + " = false",
                    "",
                    DataViewRowState.CurrentRows);

                // APartnerLocationRow.SiteKey.ToString
                // APartnerLocationRow.LocationKey.ToString
                if (PropagateLocationParametersDV.Count > 0)
                {
//                  TLogging.LogAtLevel(9, "PerformPartnerLocationChangeChecks: PartnerLocation " + APartnerLocationRow.LocationKey.ToString() +
//                          ": PartnerLocation has been changed, decision on propagation is needed.");

                    /*
                     * More information is needed (usually via user interaction)
                     * -> stop processing here and return parameters
                     * (usually used for UI interaction)
                     */
                    if (AResponseDS == null)
                    {
//                      TLogging.LogAtLevel(9, "PerformPartnerLocationChangeChecks: Creating AResponseDS.");
                        AResponseDS = new PartnerAddressAggregateTDS(MPartnerConstants.PARTNERADDRESSAGGREGATERESPONSE_DATASET);
                    }

//                  TLogging.LogAtLevel(9, "PerformPartnerLocationChangeChecks: AAddressAddedPromotionDT.Rows.Count: " + AAddressChangedPromotionDT.Rows.Count.ToString());
                    AResponseDS.Merge(AAddressChangedPromotionDT);

//                  TLogging.LogAtLevel(9, "PerformPartnerLocationChangeChecks: Merged AAddressAddedPromotionDT into AResponseDS.");
                    AResponseDS.Merge(AChangeLocationParametersDT);
//                  TLogging.LogAtLevel(9, "PerformPartnerLocationChangeChecks: Merged ChangePromotionParametersDT into AResponseDS.");
//                  TLogging.LogAtLevel(9, "PerformPartnerLocationChangeChecks: AResponseDS.Tables[" + MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME +
//                        "].Rows.Count: " + AResponseDS.Tables[MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME].Rows.Count.ToString());
                    ReturnValue = TSubmitChangesResult.scrInfoNeeded;
                    return ReturnValue;
                }
                else
                {
                    /*
                     * NOTE: If there are no Family Members, CheckPartnerLocationChange will
                     * return true, but the PropagateLocationParametersDV.Count will be 0, so
                     * the processing will also go here and find UpdatePartnerLocations false
                     * (default assigned at beginning), so nothing will happen.
                     */
                    if (UpdatePartnerLocations)
                    {
//                      TLogging.LogAtLevel(9, "PerformPartnerLocationChangeChecks: User made his/her choice regarding PartnerLocation Change promotion; now processing...");

                        /*
                         * User made his/her choice regarding PartnerLocation Change promotion;
                         * now process it
                         */
//                      TLogging.LogAtLevel(9,  "PerformPartnerLocationChangeChecks: Updated FAMILY PartnerLocation " + APartnerLocationRow.LocationKey.ToString() +
//                            ": changes should be assigned to " + Convert.ToInt32(UpdatePartnerLocationOtherPersons.GetLength(0)).ToString() + " PartnerLocations of PERSONs...");

                        PartnerLocationModifyDS = null;

                        // Find associated Parameter Row
                        PropagateLocationParametersProcessedDV = new DataView(
                            AAddressChangedPromotionDT, PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName() + " = " +
                            APartnerLocationRow[PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetSiteKeyDBName(),
                                                DataRowVersion.Original].ToString() + " AND " +
                            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName() + " = " +
                            APartnerLocationRow[PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetLocationKeyDBName(),
                                                DataRowVersion.Original].ToString() + " AND " +
                            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetPartnerLocationChangeDBName() +
                            " = true AND " +
                            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetAnswerProcessedClientSideDBName() +
                            " = true", "",
                            DataViewRowState.CurrentRows);

                        // APartnerLocationRow.SiteKey.ToString
                        // APartnerLocationRow.LocationKey.ToString

                        /*
                         * Create a Collection by splitting String 'ChangedFields' from the
                         * Parameter Row.
                         * The String contains: DBName, Label, OriginalValue, CurrentValue for
                         * each changed DataColumn.
                         */
                        ChangedFieldsColl =
                            StringHelper.StrSplit(((PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow)
                                                   PropagateLocationParametersProcessedDV[0].
                                                   Row).ChangedFields, "|");
                        ChangedFieldsArr = new String[(Convert.ToInt16(ChangedFieldsColl.Count / 4.0))];
//                      TLogging.LogAtLevel(9, "PerformPartnerLocationChangeChecks: Length(ChangedFieldsArr): " + Convert.ToInt16(ChangedFieldsArr.Length).ToString());

                        Counter = 0;
                        Counter2 = 0;

                        // Build array that contains just the DB names of the changed fields
                        while (Counter2 <= ChangedFieldsArr.Length - 1)
                        {
                            ChangedFieldsArr[Counter2] = ChangedFieldsColl[Counter];

                            // position Counter to next DB Field name
                            Counter = Counter + 4;
                            Counter2 = Counter2 + 1;
                        }

                        /*
                         * Load data for each Person's PartnerLocation to which the changes to
                         * the Family's PartnerLocation should be taken over and apply the change.
                         */
                        PartnerLocationModifyDS = new DataSet();
                        PartnerLocationModifyDS.Tables.Add(new PPartnerLocationTable(PPartnerLocationTable.GetTableName()));

                        for (Counter3 = 0; Counter3 <= UpdatePartnerLocationOtherPersons.GetLength(0) - 1; Counter3 += 1)
                        {
                            ParametersArray = new OdbcParameter[3];
                            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
                            ParametersArray[0].Value = (System.Object)(UpdatePartnerLocationOtherPersons[Counter3, 0]);
                            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
                            ParametersArray[1].Value = (System.Object)(UpdatePartnerLocationOtherPersons[Counter3, 1]);
                            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
                            ParametersArray[2].Value = (System.Object)(UpdatePartnerLocationOtherPersons[Counter3, 2]);
                            PartnerLocationModifyDS = DBAccess.GDBAccessObj.Select(PartnerLocationModifyDS,
                                "SELECT * " + "FROM PUB_" + PPartnerLocationTable.GetTableDBName() + ' ' + "WHERE " +
                                PPartnerLocationTable.GetPartnerKeyDBName() + " = ? " + "AND " + PPartnerLocationTable.GetSiteKeyDBName() + " = ? " +
                                "AND " + PPartnerLocationTable.GetLocationKeyDBName() + " = ?",
                                PPartnerLocationTable.GetTableName(),
                                ASubmitChangesTransaction,
                                ParametersArray);
                            PartnerLocationModificationDT = ((PPartnerLocationTable)PartnerLocationModifyDS.Tables[0]);

                            // Take over defined Columns' contents (only those Columns containing
                            // changes) of the FAMILY's PartnerLocation to the loaded
                            // PartnerLocation
                            for (Counter4 = 0; Counter4 <= ChangedFieldsArr.Length - 1; Counter4 += 1)
                            {
                                PartnerLocationModificationDT[0][ChangedFieldsArr[Counter4]] = APartnerLocationRow[ChangedFieldsArr[Counter4]];
//                              TLogging.LogAtLevel(9, "PerformPartnerLocationChangeChecks: Changes to Column " + ChangedFieldsArr[Counter4].ToString() +
//                                      " taken over for LocationKey " + PartnerLocationModificationDT[0][PPartnerLocationTable.GetLocationKeyDBName()].ToString());
                            }

                            // Submit the changes of the processed Person's PartnerLocation record to the DB
                            if (!PPartnerLocationAccess.SubmitChanges(PartnerLocationModificationDT, ASubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                AVerificationResult.AddCollection(SingleVerificationResultCollection);
                                ReturnValue = TSubmitChangesResult.scrError;
                                return ReturnValue;
                            }

                            // Don't keep the Person's PPartnerLocation in memory that we just processed!
                            PartnerLocationModifyDS.Tables[0].Rows.Clear();
                        }
                    }
                }

                ReturnValue = TSubmitChangesResult.scrOK;
            }
            else
            {
//              TLogging.LogAtLevel(9, "PerformPartnerLocationChangeChecks: Location " + APartnerLocationRow.LocationKey.ToString() + ": User cancelled the selection - stopping the whole saving process!");

                /*
                 * User cancelled the selection - stop the whole saving process!
                 */
                AVerificationResult.Add(new TVerificationResult("Partner-specific Address Data Change Promotion: Information",
                        "No changes were saved because the Partner-specific Address Data Promotion dialog was cancelled by the user.",
                        "Saving cancelled by user", "", TResultSeverity.Resv_Noncritical));
                ReturnValue = TSubmitChangesResult.scrError;
            }

            return ReturnValue;
        }

        /// <summary>
        /// For all members of the Family: if they have a PartnerLocation mapped to the
        /// same Location as the Family then update the DateGoodUntil column.
        ///
        /// @comment Must only be called for Partners of Partner Class FAMILY - the
        /// function does no checks on that and will fail for other Partner Classes!
        ///
        /// </summary>
        /// <param name="AFamilyPartnerKey">PartnerKey of the Family</param>
        /// <param name="APartnerLocationDR">PartnerLocation DataRow of the Family</param>
        /// <param name="ASubmitChangesTransaction">Running transaction in which the DB commands
        /// will be enlisted</param>
        /// <param name="AVerificationResult">Nil if DB update call succeded, otherwise filled
        /// with 1..n TVerificationResult objects (contains DB call exceptions)</param>
        /// <returns>true if processing was successful, otherwise false
        /// </returns>
        private static Boolean PromoteToFamilyMembersDateGoodUntilChange(Int64 AFamilyPartnerKey,
            PPartnerLocationRow APartnerLocationDR,
            TDBTransaction ASubmitChangesTransaction,
            out TVerificationResultCollection AVerificationResult)
        {
            Boolean ReturnValue;
            StringCollection RequiredColumns;
            PPersonTable FamilyPersonsDT;
            PPartnerLocationTable PartnerLocationDT;
            PPartnerLocationTable PartnerLocationSubmitDT;
            PPartnerLocationRow PartnerLocationSubmitDR;
            Int16 Counter;
            TVerificationResultCollection SingleVerificationResultCollection;

            ReturnValue = true;
            AVerificationResult = null;

            FamilyPersonsDT = GetFamilyMemberPartnerKeys(AFamilyPartnerKey, ASubmitChangesTransaction);
            PartnerLocationSubmitDT = new PPartnerLocationTable();
            RequiredColumns = new StringCollection();
            RequiredColumns.Add(PPartnerLocationTable.GetModificationIdDBName());

            /*
             * For all members of the Family: if they have a PartnerLocation mapped to the
             * same Location as the Family then update the DateGoodUntil column
             */
//          TLogging.LogAtLevel(9, "PromoteToFamilyMembersDateGoodUntilChange for Location " + APartnerLocationDR.LocationKey.ToString() + ": Family has " + FamilyPersonsDT.Rows.Count.ToString() + " members.");

            for (Counter = 0; Counter <= FamilyPersonsDT.Rows.Count - 1; Counter += 1)
            {
                // Load ModificationId for the PartnerLocation of each Person (otherwise we
                // will run into optimistic locking problems!)
                PartnerLocationDT = PPartnerLocationAccess.LoadByPrimaryKey(
                    FamilyPersonsDT[Counter].PartnerKey,
                    APartnerLocationDR.SiteKey,
                    APartnerLocationDR.LocationKey,
                    RequiredColumns,
                    ASubmitChangesTransaction);

                /*
                 * Check that found Person has PartnerLocation that is mapped to the same
                 * Location than the one that the PartnerLocation of the Family that we are
                 * processing is mapped to
                 */
                if (PartnerLocationDT.Rows.Count != 0)
                {
                    // Build row to be able to update that PartnerLocation
                    PartnerLocationSubmitDR = PartnerLocationSubmitDT.NewRowTyped(false);
                    PartnerLocationSubmitDR.PartnerKey = FamilyPersonsDT[Counter].PartnerKey;
                    PartnerLocationSubmitDR.SiteKey = APartnerLocationDR.SiteKey;
                    PartnerLocationSubmitDR.LocationKey = APartnerLocationDR.LocationKey;
                    PartnerLocationSubmitDR.ModificationId = PartnerLocationDT[0].ModificationId;
                    PartnerLocationSubmitDT.Rows.Add(PartnerLocationSubmitDR);

                    // Change its DateGoodUntil column to match the Family's PartnerLocation DateGoodUntil column
                    if (!APartnerLocationDR.IsDateGoodUntilNull())
                    {
                        // Make this row unchanged so that SubmitChanges later picks up a changed
                        // DataRow, and not a new DataRow.
                        PartnerLocationSubmitDR.AcceptChanges();

                        // Now change DateGoodUntil
                        PartnerLocationSubmitDR.DateGoodUntil = APartnerLocationDR.DateGoodUntil;
                    }
                    else
                    {
                        // Change DataRow to a dummy value to make Null different to what was
                        // there before
                        PartnerLocationSubmitDR.DateGoodUntil = DateTime.MinValue;
                        PartnerLocationSubmitDR.AcceptChanges();

                        // Now change DateGoodUntil
                        PartnerLocationSubmitDR.SetDateGoodUntilNull();
                    }

                    // Get PartnerLocation data of just processed Person out of memory!
                    PartnerLocationDT.Rows.Clear();
                }
            }

            /*
             * Save changes to DateGoodUntil columns if PartnerLocation of any FamilyMember got changed.
             */
            if (PartnerLocationSubmitDT.Rows.Count > 0)
            {
                // Submit the changes to the DB
                if (!PPartnerLocationAccess.SubmitChanges(PartnerLocationSubmitDT, ASubmitChangesTransaction, out SingleVerificationResultCollection))
                {
                    AVerificationResult.AddCollection(SingleVerificationResultCollection);
                    ReturnValue = false;
                    return ReturnValue;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Removes a Location from all Extracts that reference it.
        /// It does that by changing these Extract records to reference Location 0
        /// (dummy Location) instead of referencing the Location that is passed in.
        ///
        /// @comment WARNING: must only be called if a Location is deleted
        /// and it is no longer referenced by any other Partner (ie. it will get
        /// deleted from the DB) - this procedure does no check on that!
        ///
        /// </summary>
        /// <param name="ALocationRow">Location DataRow which should be processed</param>
        /// <param name="ASubmitChangesTransaction">Running transaction in which the DB commands
        /// will be enlisted</param>
        /// <param name="AVerificationResult">Nil if DB update call succeded, otherwise filled
        /// with 1..n TVerificationResult objects (contains DB call exceptions)</param>
        /// <returns>true if processing was successful, otherwise false
        /// </returns>
        private static Boolean RemoveLocationFromExtracts(PLocationRow ALocationRow,
            TDBTransaction ASubmitChangesTransaction,
            ref TVerificationResultCollection AVerificationResult)
        {
            Boolean ReturnValue;
            MExtractTable ExtractsDT;
            Int32 Counter;
            StringCollection RequiredColumns;
            TVerificationResultCollection SingleVerificationResultCollection;

//          TLogging.LogAtLevel(9, "RemoveLocationFromExtracts for Location " +
//              Convert.ToInt32(ALocationRow[MExtractTable.GetLocationKeyDBName(), DataRowVersion.Original]).ToString());
            ReturnValue = true;
            Counter = 0;

            // Load all Extracts that contain the Location
            RequiredColumns = new StringCollection();
            RequiredColumns.Add(MExtractTable.GetExtractIdDBName());
            RequiredColumns.Add(MExtractTable.GetPartnerKeyDBName());
            RequiredColumns.Add(MExtractTable.GetSiteKeyDBName());
            RequiredColumns.Add(MExtractTable.GetLocationKeyDBName());
            ExtractsDT = MExtractAccess.LoadViaPLocation(Convert.ToInt64(
                    ALocationRow[MExtractTable.GetSiteKeyDBName(),
                                 DataRowVersion.Original]), Convert.ToInt32(
                    ALocationRow[MExtractTable.GetLocationKeyDBName(), DataRowVersion.Original]), RequiredColumns, ASubmitChangesTransaction);

            if (ExtractsDT.Rows.Count != 0)
            {
                /*
                 * Change these Extract records to reference Location 0 (dummy Location)
                 * instead of referencing the current Location.
                 */
                while (Counter != ExtractsDT.Rows.Count)
                {
//                  TLogging.LogAtLevel(8,  "RemoveLocationFromExtracts: Removing Location with LocationKey " + ExtractsDT[Counter].LocationKey.ToString() +
//                       " from Extract with ExtractID ''" + ExtractsDT[Counter].ExtractId.ToString() + "''.");
                    ExtractsDT[Counter].SiteKey = 0;
                    ExtractsDT[Counter].LocationKey = 0;
                    Counter = Counter + 1;
                }

                // Submit the changes to these Extract records to the DB
                if (!MExtractAccess.SubmitChanges(ExtractsDT, ASubmitChangesTransaction, out SingleVerificationResultCollection))
                {
                    AVerificationResult.AddCollection(SingleVerificationResultCollection);
                    ReturnValue = false;
                    return ReturnValue;
                }
            }
            else
            {
//              TLogging.LogAtLevel(9, "RemoveLocationFromExtracts: Location with LocationKey " +
//                  ALocationRow[MExtractTable.GetLocationKeyDBName(), DataRowVersion.Original].ToString() + " was not referenced in any Extract -> nothing to do.");
            }

            return ReturnValue;
        }
    }
}