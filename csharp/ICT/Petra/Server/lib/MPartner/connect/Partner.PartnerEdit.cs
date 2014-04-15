//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Server.MCommon.Data.Cascading;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Petra.Server.MPartner.Partner;
using Ict.Petra.Shared.MPersonnel.Person;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MCommon.UIConnectors;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner;
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Server.MSysMan.Maintenance.UserDefaults.WebConnectors;
using Ict.Petra.Server.MPersonnel.Person.DataElements.WebConnectors;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MPartner.Partner.UIConnectors
{
    /// <summary>
    /// Partner Edit Screen UIConnector working with a Typed DataSet
    ///
    /// UIConnector Objects are instantiated by the Client's User Interface via the
    /// Instantiator classes.
    /// They handle requests for data retrieval and saving of data (including data
    /// verification).
    ///
    /// Their role is to
    ///   - retrieve (and possibly aggregate) data using Business Objects,
    ///   - put this data into///one* DataSet that is passed to the Client and make
    ///     sure that no unnessary data is transferred to the Client,
    ///   - optionally provide functionality to retrieve additional, different data
    ///     if requested by the Client (for Client screens that load data initially
    ///     as well as later, eg. when a certain tab on the screen is clicked),
    ///   - save data using Business Objects.
    ///
    /// @Comment These Objects would usually not be instantiated by other Server
    ///          Objects, but only by the Client UI via the Instantiator classes.
    ///          However, Server Objects that derive from these objects and that
    ///          are also UIConnectors are feasible.
    ///
    /// </summary>
    public partial class TPartnerEditUIConnector : TConfigurableMBRObject, IPartnerUIConnectorsPartnerEdit
    {
        private const String DATASETNAME = "PartnerEditScreen";

        /// <summary>Holds DataTables which are created by the instantiated Partner Business Objects</summary>
        private PartnerEditTDS FPartnerEditScreenDS;
        private Int64 FPartnerKey;
        private Int64 FNewPartnerPartnerKey;
        private TLocationPK FKeyForSelectingPartnerLocation;
        private TPartnerClass FPartnerClass;
        private TPartnerClass FNewPartnerPartnerClass;
        private PartnerEditTDS FSubmissionDS;

        #region TPartnerEditUIConnector

        /// <summary>
        /// Constructor which automatically loads data for the Partner.
        ///
        /// </summary>
        /// <param name="APartnerKey">PartnerKey for the Partner to instantiate this object with
        /// </param>
        /// <returns>void</returns>
        public TPartnerEditUIConnector(System.Int64 APartnerKey) : base()
        {
            FPartnerKey = APartnerKey;
            FKeyForSelectingPartnerLocation = new TLocationPK(0, 0);
        }

        /// <summary>
        /// Constructor which automatically loads data for the Partner.
        ///
        /// @Comment Use this overload to specify which Address should be selected when
        /// the Partner Edit screen is shown.
        ///
        /// </summary>
        /// <param name="APartnerKey">PartnerKey for the Partner to instantiate this object with</param>
        /// <param name="ASiteKey">SiteKey of the PartnerLocation record of the Partner</param>
        /// <param name="ALocationKey">LocationKey of the PartnerLocation record of the Partner
        /// </param>
        /// <returns>void</returns>
        public TPartnerEditUIConnector(Int64 APartnerKey, Int64 ASiteKey, Int32 ALocationKey) : base()
        {
            FPartnerKey = APartnerKey;
            FKeyForSelectingPartnerLocation = new TLocationPK(ASiteKey, ALocationKey);
        }

        /// <summary>
        /// Constructor for creating a new Partner.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TPartnerEditUIConnector() : base()
        {
            FKeyForSelectingPartnerLocation = new TLocationPK(0, 0);
        }

        #region BankingDetails

        private void PrepareBankingDetailsForSaving(ref PartnerEditTDS AInspectDS, ref TVerificationResultCollection AVerificationResult,
            TDBTransaction ATransaction)
        {
            if ((AInspectDS.PBankingDetails != null) && (AInspectDS.PBankingDetails.Rows.Count > 0))
            {
                AInspectDS.Merge(new PBankingDetailsUsageTable());
                AInspectDS.InitVars();
                PBankingDetailsUsageAccess.LoadViaPPartner(AInspectDS, FPartnerKey, ATransaction);
                AInspectDS.PBankingDetailsUsage.AcceptChanges();

                // make sure there is at least one main account, or no account at all
                // make sure there are no multiple main accounts

                PartnerEditTDS LocalDS = new PartnerEditTDS();
                PBankingDetailsAccess.LoadViaPPartner(LocalDS, FPartnerKey, ATransaction);

                // there are problems with Mono on merging deleted rows into the LocalDS, they become modified rows
                // see https://tracker.openpetra.org/view.php?id=1871
                LocalDS.Merge(AInspectDS.PBankingDetails);

                // workaround for bug 1871
                List <Int32>DeletedBankingDetails = new List <Int32>();

                foreach (DataRow bdrow in AInspectDS.PBankingDetails.Rows)
                {
                    if (bdrow.RowState == DataRowState.Deleted)
                    {
                        DeletedBankingDetails.Add(Convert.ToInt32(bdrow[AInspectDS.PBankingDetails.ColumnBankingDetailsKey, DataRowVersion.Original]));
                    }
                }

                bool ThereIsAtLeastOneAccount = false;

                // get the main account
                Int32 MainAccountBankingDetails = Int32.MinValue;

                foreach (PartnerEditTDSPBankingDetailsRow detailRow in LocalDS.PBankingDetails.Rows)
                {
                    if ((detailRow.RowState != DataRowState.Deleted) && !DeletedBankingDetails.Contains(detailRow.BankingDetailsKey))
                    {
                        ThereIsAtLeastOneAccount = true;

                        if (!detailRow.IsMainAccountNull() && detailRow.MainAccount)
                        {
                            if ((MainAccountBankingDetails != Int32.MinValue) && (detailRow.BankingDetailsKey != MainAccountBankingDetails))
                            {
                                AVerificationResult.Add(new TVerificationResult(
                                        String.Format("Banking Details"),
                                        string.Format("there are multiple main accounts"),
                                        TResultSeverity.Resv_Critical));
                                return;
                            }
                            else
                            {
                                // if the main account has been changed
                                MainAccountBankingDetails = detailRow.BankingDetailsKey;
                            }
                        }
                    }
                }

                bool alreadyExists = false;

                foreach (PBankingDetailsUsageRow usageRow in AInspectDS.PBankingDetailsUsage.Rows)
                {
                    if (usageRow.Type == MPartnerConstants.BANKINGUSAGETYPE_MAIN)
                    {
                        DataRow bdrow = LocalDS.PBankingDetails.Rows.Find(usageRow.BankingDetailsKey);

                        if ((bdrow == null) || (bdrow.RowState == DataRowState.Deleted)
                            || DeletedBankingDetails.Contains(usageRow.BankingDetailsKey))
                        {
                            // deleting the account
                            usageRow.Delete();
                        }
                        else if (MainAccountBankingDetails == Int32.MinValue)
                        {
                            MainAccountBankingDetails = usageRow.BankingDetailsKey;
                            alreadyExists = true;
                        }
                        else if ((MainAccountBankingDetails != Int32.MinValue) && (usageRow.BankingDetailsKey != MainAccountBankingDetails))
                        {
                            usageRow.Delete();
                        }
                        else
                        {
                            alreadyExists = true;
                        }
                    }
                }

                if (!alreadyExists && ThereIsAtLeastOneAccount)
                {
                    if (MainAccountBankingDetails == Int32.MinValue)
                    {
                        AVerificationResult.Add(new TVerificationResult(
                                "Banking Details",
                                "One Bank Account of a Partner must be set as the 'Main Account'. Please select the record that should become the 'Main Account' and choose 'Set Main Account'.",
                                TResultSeverity.Resv_Critical));
                        return;
                    }
                    else
                    {
                        PBankingDetailsUsageRow newUsageRow = AInspectDS.PBankingDetailsUsage.NewRowTyped(true);
                        newUsageRow.PartnerKey = FPartnerKey;
                        newUsageRow.BankingDetailsKey = MainAccountBankingDetails;
                        newUsageRow.Type = MPartnerConstants.BANKINGUSAGETYPE_MAIN;
                        AInspectDS.PBankingDetailsUsage.Rows.Add(newUsageRow);
                    }
                }
            }
        }

        /// <summary>
        /// get the banking details of the current partner
        /// </summary>
        public PartnerEditTDS GetBankingDetails()
        {
            Boolean NewTransaction;

            PartnerEditTDS localDS = new PartnerEditTDS();

            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            // Get hold of the two tables needed for the banking information:
            // p_partner_banking_details
            // p_banking_details
            try
            {
                PBankingDetailsAccess.LoadViaPPartner(localDS, FPartnerKey, ReadTransaction);
                PPartnerBankingDetailsAccess.LoadViaPPartner(localDS, FPartnerKey, ReadTransaction);
                PBankingDetailsUsageAccess.LoadViaPPartner(localDS, FPartnerKey, ReadTransaction);
            }
            catch (Exception)
            {
                TLogging.Log("An exception happened while retrieving data.", TLoggingType.ToLogfile);
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetBankingDetails: committed own transaction.");
                }
            }

            foreach (PartnerEditTDSPBankingDetailsRow bd in localDS.PBankingDetails.Rows)
            {
                bd.MainAccount =
                    (localDS.PBankingDetailsUsage.Rows.Find(
                         new object[] { FPartnerKey, bd.BankingDetailsKey, MPartnerConstants.BANKINGUSAGETYPE_MAIN }) != null);
            }

            localDS.RemoveEmptyTables();

            return localDS;
        }

        #endregion

        /// <summary>
        /// Passes data as a Typed DataSet to the Partner Edit Screen, containing multiple
        /// DataTables.
        ///
        /// </summary>
        /// <param name="ADelayedDataLoading">Tells whether Delayed Data Loading is activated on
        /// the Client side, or not</param>
        /// <param name="ATabPage">Tab Page the Client wants to display initially.
        /// </param>
        /// <returns>void</returns>
        [RequireModulePermission("CONFERENCE")]
        public PartnerEditTDS GetData(Boolean ADelayedDataLoading, TPartnerEditTabPageEnum ATabPage)
        {
            LoadData(ADelayedDataLoading, ATabPage);
            return FPartnerEditScreenDS;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ADelayedDataLoading"></param>
        /// <returns></returns>
        public PartnerEditTDS GetData(Boolean ADelayedDataLoading)
        {
            return GetData(ADelayedDataLoading, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <returns></returns>
        public PartnerEditTDS GetData()
        {
            return GetData(false, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public PartnerEditTDS GetDataAddresses()
        {
            PartnerEditTDS ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

            TLogging.LogAtLevel(9, "TPartnerEditUIConnector.GetDataAddresses called!");
            ReturnValue = new PartnerEditTDS(DATASETNAME);
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            // Load data
            TPPartnerAddressAggregate.LoadAll(ReturnValue, FPartnerKey, ReadTransaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetDataAddresses: committed own transaction.");
            }

            // Remove any unused tables from the Typed DataSet
            ReturnValue.RemoveEmptyTables();
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ABaseTableOnly"></param>
        /// <returns></returns>
        public PartnerEditTDS GetDataFoundation(Boolean ABaseTableOnly)
        {
            PartnerEditTDS ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

            TLogging.LogAtLevel(9, "TPartnerEditUIConnector.GetDataFoundation called!");
            ReturnValue = new PartnerEditTDS(DATASETNAME);
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            // Foundation Table
            PFoundationAccess.LoadByPrimaryKey(ReturnValue, FPartnerKey, ReadTransaction);

            if (!ABaseTableOnly)
            {
                // Foundation Proposal Deadline
                PFoundationDeadlineAccess.LoadViaPFoundation(ReturnValue, FPartnerKey, ReadTransaction);

                // Proposal Table
                PFoundationProposalAccess.LoadViaPFoundation(ReturnValue, FPartnerKey, ReadTransaction);

                // Proposal Detail Table
                PFoundationProposalDetailAccess.LoadViaPFoundation(ReturnValue, FPartnerKey, ReadTransaction);
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetDataFoundation: committed own transaction.");
            }

            // Remove any unused tables from the Typed DataSet
            ReturnValue.RemoveEmptyTables();
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public PartnerEditTDSPPartnerInterestTable GetDataPartnerInterests()
        {
            Int32 PartnerInterestsCount;

            return GetPartnerInterestsInternal(out PartnerInterestsCount, false);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public PInterestTable GetDataInterests()
        {
            Int32 InterestsCount;

            return GetInterestsInternal(out InterestsCount, false);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASiteKey"></param>
        /// <param name="ALocationKey"></param>
        /// <returns></returns>
        public PLocationTable GetDataLocation(Int64 ASiteKey, Int32 ALocationKey)
        {
            PLocationTable ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            ReturnValue = TPPartnerAddressAggregate.LoadByPrimaryKey(ASiteKey, ALocationKey, ReadTransaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetDataLocation: committed own transaction.");
            }

            return ReturnValue;
        }

        private void LoadData(Boolean ADelayedDataLoading, TPartnerEditTabPageEnum ATabPage)
        {
            PartnerEditTDSMiscellaneousDataTable MiscellaneousDataDT;
            PartnerEditTDSMiscellaneousDataRow MiscellaneousDataDR;
            TDBTransaction ReadTransaction;
            DateTime LastGiftDate;
            DateTime LastContactDate;
            String LastGiftInfo;
            TLocationPK LocationPK;
            Boolean OfficeSpecificDataLabelsAvailable = false;
            Int32 ItemsCountAddresses = 0;
            Int32 ItemsCountAddressesActive = 0;
            Int32 ItemsCountSubscriptions = 0;
            Int32 ItemsCountSubscriptionsActive = 0;
            Int32 ItemsCountPartnerTypes = 0;
            Int32 ItemsCountPartnerRelationships = 0;
            Int32 ItemsCountFamilyMembers = 0;
            Int32 ItemsCountPartnerInterests = 0;
            Int32 ItemsCountInterests = 0;
            Int32 ItemsCountPartnerBankingDetails = 0;
            Int64 FoundationOwner1Key = 0;
            Int64 FoundationOwner2Key = 0;
            bool HasEXWORKERPartnerType = false;

//          TLogging.LogAtLevel(7, "TPartnerEditUIConnector.LoadData called. ADelayedDataLoading: " + ADelayedDataLoading.ToString() + "; ATabPage: " +
//               Enum.GetName(typeof(TPartnerEditTabPageEnum), ATabPage));

            // create the FPartnerEditScreenDS DataSet that will later be passed to the Client
            FPartnerEditScreenDS = new PartnerEditTDS(DATASETNAME);

            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.RepeatableRead, 5);
                try
                {
                    #region Load data using the DataStore

                    // Partner
                    PPartnerAccess.LoadByPrimaryKey(FPartnerEditScreenDS, FPartnerKey, ReadTransaction);

                    if (FPartnerEditScreenDS.PPartner.Rows.Count != 0)
                    {
                        FPartnerClass = SharedTypes.PartnerClassStringToEnum(FPartnerEditScreenDS.PPartner[0].PartnerClass);
                    }
                    else
                    {
                        throw new EPartnerNotExistantException();
                    }

                    /*
                     * For Partners that aren't ORGANISATIONs we can check access to the Partner here.
                     * For Partners that *are* ORGANISATIONS we need to do a more elaborate check
                     * further down the code of this Method!
                     */
                    if (FPartnerClass != TPartnerClass.ORGANISATION)
                    {
                        /*
                         * Check if access to Partner is granted; if not, an
                         * ESecurityPartnerAccessDeniedException will be thrown by the called Method!
                         */
                        Ict.Petra.Shared.MPartner.TSecurity.CanAccessPartnerExc(FPartnerEditScreenDS.PPartner[0], false, null);
                    }

                    // Partner Types
                    FPartnerEditScreenDS.Merge(GetPartnerTypesInternal(out ItemsCountPartnerTypes, false));

                    // Determine whether the Partner has a 'EX-WORKER*' Partner Type
                    HasEXWORKERPartnerType = Ict.Petra.Shared.MPartner.Checks.HasPartnerType(MPartnerConstants.PARTNERTYPE_EX_WORKER,
                        FPartnerEditScreenDS.PPartnerType,
                        false);

                    if ((ADelayedDataLoading) && (ATabPage != TPartnerEditTabPageEnum.petpPartnerTypes))
                    {
                        // Empty Tables again, we don't want to transfer the data contained in them
                        FPartnerEditScreenDS.PPartnerType.Rows.Clear();
                    }

                    // Subscriptions
                    FPartnerEditScreenDS.Merge(GetSubscriptionsInternal(out ItemsCountSubscriptions, false));

                    if ((ADelayedDataLoading) && (ATabPage != TPartnerEditTabPageEnum.petpSubscriptions))
                    {
                        // Only count Subscriptions
                        Calculations.CalculateTabCountsSubscriptions(FPartnerEditScreenDS.PSubscription,
                            out ItemsCountSubscriptions,
                            out ItemsCountSubscriptionsActive);

                        // Empty Tables again, we don't want to transfer the data contained in them
                        FPartnerEditScreenDS.PSubscription.Rows.Clear();
                    }

                    // Partner Relationships
                    FPartnerEditScreenDS.Merge(GetPartnerRelationshipsInternal(out ItemsCountPartnerRelationships, false));

                    if ((ADelayedDataLoading) && (ATabPage != TPartnerEditTabPageEnum.petpPartnerRelationships))
                    {
                        // Only count Relationships
                        Calculations.CalculateTabCountsPartnerRelationships(FPartnerEditScreenDS.PPartnerRelationship,
                            out ItemsCountPartnerRelationships);

                        // Empty Tables again, we don't want to transfer the data contained in them
                        FPartnerEditScreenDS.PPartnerRelationship.Rows.Clear();
                    }

                    // Locations and PartnerLocations
                    TLogging.LogAtLevel(9, "TPartnerEditUIConnector.LoadData: Before TPPartnerAddressAggregate.LoadAll");
                    TPPartnerAddressAggregate.LoadAll(FPartnerEditScreenDS, FPartnerKey, ReadTransaction);

                    if (FKeyForSelectingPartnerLocation.LocationKey > 0)
                    {
                        if (FPartnerEditScreenDS.PPartnerLocation.Rows.Find(new Object[] { FPartnerKey, FKeyForSelectingPartnerLocation.SiteKey,
                                                                                           FKeyForSelectingPartnerLocation.LocationKey }) == null)
                        {
                            throw new EPartnerLocationNotExistantException(
                                "PartnerLocation SiteKey " + FKeyForSelectingPartnerLocation.SiteKey.ToString() + " and LocationKey " +
                                FKeyForSelectingPartnerLocation.LocationKey.ToString());
                        }
                    }

                    if (((!ADelayedDataLoading)) || (ATabPage == TPartnerEditTabPageEnum.petpAddresses))
                    {
                        // Determination of the address list icons and the 'best' address (these calls change certain columns in some rows!)
                        Calculations.DeterminePartnerLocationsDateStatus((DataSet)FPartnerEditScreenDS);
                        LocationPK = Calculations.DetermineBestAddress((DataSet)FPartnerEditScreenDS);
                    }
                    else
                    {
                        // Only count
                        Calculations.CalculateTabCountsAddresses(FPartnerEditScreenDS.PPartnerLocation,
                            out ItemsCountAddresses,
                            out ItemsCountAddressesActive);

                        // Empty Tables again, we don't want to transfer the data contained in them
                        FPartnerEditScreenDS.PLocation.Rows.Clear();
                        FPartnerEditScreenDS.PPartnerLocation.Rows.Clear();

                        // location will be determined correctly on the Client side...
                        LocationPK = new TLocationPK(0, 0);
                    }

                    // PartnerInterests
                    if (((!ADelayedDataLoading)) || (ATabPage == TPartnerEditTabPageEnum.petpInterests))
                    {
                        DataRow InterestRow;

                        // Load data for Interests
                        FPartnerEditScreenDS.Merge(GetPartnerInterestsInternal(out ItemsCountPartnerInterests, false));
                        FPartnerEditScreenDS.Merge(GetInterestsInternal(out ItemsCountInterests, false));

                        // fill field for interest category in PartnerInterest table in dataset
                        foreach (PartnerEditTDSPPartnerInterestRow row in FPartnerEditScreenDS.PPartnerInterest.Rows)
                        {
                            InterestRow = FPartnerEditScreenDS.PInterest.Rows.Find(new object[] { row.Interest });

                            if (InterestRow != null)
                            {
                                row.InterestCategory = ((PInterestRow)InterestRow).Category;
                            }
                        }
                    }
                    else
                    {
                        // Only count Interests
                        GetPartnerInterestsInternal(out ItemsCountPartnerInterests, true);
                    }

                    #region Partner Details data according to PartnerClass

                    switch (FPartnerClass)
                    {
                        case TPartnerClass.PERSON:

                            // Disable some constraints that relate to other tables in the DataSet that are not filled with data.
                            // This applies for the DataTables that exist for a certain Partner Class, eg. Person, where all other
                            // DataTables that represent certain Partner Classes are not filled.
                            FPartnerEditScreenDS.DisableConstraint("FKPerson2");

                            // points to PFamily
                            FPartnerEditScreenDS.DisableConstraint("FKPerson4");

                            // points to PUnit
                            TLogging.LogAtLevel(9, "Disabled Constraints in Typed DataSet PartnerEditTDS.");
                            PPersonAccess.LoadByPrimaryKey(FPartnerEditScreenDS, FPartnerKey, ReadTransaction);

                            if (((!ADelayedDataLoading)) || (ATabPage == TPartnerEditTabPageEnum.petpFamilyMembers))
                            {
                                // Load data for Family Members
                                FPartnerEditScreenDS.Merge(GetFamilyMembersInternal(FPartnerEditScreenDS.PPerson[0].FamilyKey, "",
                                        out ItemsCountFamilyMembers, false));
                            }
                            else
                            {
                                // Only count Family Members
                                GetFamilyMembersInternal(FPartnerEditScreenDS.PPerson[0].FamilyKey, "", out ItemsCountFamilyMembers, true);
                            }

                            break;

                        case TPartnerClass.FAMILY:
                            PFamilyAccess.LoadByPrimaryKey(FPartnerEditScreenDS, FPartnerKey, ReadTransaction);

                            if (((!ADelayedDataLoading)) || (ATabPage == TPartnerEditTabPageEnum.petpFamilyMembers))
                            {
                                // Load data for Family Members
                                FPartnerEditScreenDS.Merge(GetFamilyMembersInternal(FPartnerEditScreenDS.PFamily[0].PartnerKey, "",
                                        out ItemsCountFamilyMembers, false));
                            }
                            else
                            {
                                // Only count Family Members
                                GetFamilyMembersInternal(FPartnerEditScreenDS.PFamily[0].PartnerKey, "", out ItemsCountFamilyMembers, true);
                            }

                            break;

                        case TPartnerClass.CHURCH:
                            PChurchAccess.LoadByPrimaryKey(FPartnerEditScreenDS, FPartnerKey, ReadTransaction);
                            break;

                        case TPartnerClass.ORGANISATION:
                            POrganisationAccess.LoadByPrimaryKey(FPartnerEditScreenDS, FPartnerKey, ReadTransaction);

                            if (FPartnerEditScreenDS.POrganisation[0].Foundation)
                            {
                                if (!((UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVUSER))
                                      || (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVADMIN))))
                                {
                                    throw new ESecurityScreenAccessDeniedException(
                                        // Some users won't have access to edit partners that are foundations. Foundations are a special type of organisation.
                                        // They usually are important donors, and they should not be approached by any user in the
                                        // office, but only by the person that has been assigned to do that job
                                        Catalog.GetString(
                                            "You do not have access to Partners of Partner Class 'ORGANISATION' that are 'Foundations'!"));
                                }
                                else
                                {
                                    // User has access to open the screen, based on Module Security
                                    if (((!ADelayedDataLoading)) || (ATabPage == TPartnerEditTabPageEnum.petpFoundationDetails))
                                    {
                                        // Load all data that there is for a Foundation (across several DataTables)
                                        FPartnerEditScreenDS.Merge(GetDataFoundation(false));
                                    }
                                    else
                                    {
                                        // Load just p_foundation record (needed for security, see below)
                                        FPartnerEditScreenDS.Merge(GetDataFoundation(true));
                                    }

                                    if (FPartnerEditScreenDS.PFoundation.Rows.Count != 0)
                                    {
                                        // Store Foundation Owner1 and Foundation Owner2 from the
                                        // p_foundation record. This information will be evaluated in the
                                        // Partner Edit screen to allow/disallow access to the Foundation
                                        // Details Tab.
                                        if (FPartnerEditScreenDS.PFoundation[0].IsOwner1KeyNull())
                                        {
                                            FoundationOwner1Key = 0;
                                        }
                                        else
                                        {
                                            FoundationOwner1Key = FPartnerEditScreenDS.PFoundation[0].Owner1Key;
                                        }

                                        if (FPartnerEditScreenDS.PFoundation[0].IsOwner2KeyNull())
                                        {
                                            FoundationOwner2Key = 0;
                                        }
                                        else
                                        {
                                            FoundationOwner2Key = FPartnerEditScreenDS.PFoundation[0].Owner2Key;
                                        }

                                        if ((ADelayedDataLoading) && (ATabPage != TPartnerEditTabPageEnum.petpFoundationDetails))
                                        {
                                            // We don't want to transfer the data of the table until it is
                                            // needed on the Client side (also for security reasons).
                                            FPartnerEditScreenDS.Tables.Remove(FPartnerEditScreenDS.PFoundation);
                                        }

                                        /*
                                         * Check if access to Partner is granted; if not, an
                                         * ESecurityPartnerAccessDeniedException will be thrown by the called Method!
                                         */
                                        Ict.Petra.Shared.MPartner.TSecurity.CanAccessPartnerExc(
                                            FPartnerEditScreenDS.PPartner[0], true,
                                            FPartnerEditScreenDS.PFoundation[0]);
                                    }
                                    else
                                    {
                                        // Althought the Organisation is marked to be a Foundation, no
                                        // p_foundation record exists. This should of course not happen,
                                        // but in case it does: mark the Organisation to be NO Foundation.
                                        FPartnerEditScreenDS.POrganisation[0].Foundation = false;

                                        /*
                                         * Check if access to Partner is granted; if not, an
                                         * ESecurityPartnerAccessDeniedException will be thrown by the called Method!
                                         */
                                        Ict.Petra.Shared.MPartner.TSecurity.CanAccessPartnerExc(
                                            FPartnerEditScreenDS.PPartner[0], false, null);
                                    }
                                }
                            }
                            else
                            {
                                /*
                                 * Check if access to Partner is granted; if not, an
                                 * ESecurityPartnerAccessDeniedException will be thrown by the called Method!
                                 */
                                Ict.Petra.Shared.MPartner.TSecurity.CanAccessPartnerExc(FPartnerEditScreenDS.PPartner[0], false, null);
                            }

                            break;

                        case TPartnerClass.BANK:

                            // Get the main information about a BANK partner
                            PBankAccess.LoadByPrimaryKey(FPartnerEditScreenDS, FPartnerKey, ReadTransaction);

                            // Gain information about the BANK partner's banking details
                            // When dealing with this grid one has to consider that the Netherland do not
                            // know bank branches and that Credit Cards do not have branches as well.
                            // Last time I checked how many accounts one bank in the Netherlands had
                            // I ended up with some 9756. I was also puzzled what use this grid may have.
                            // It is very tedious to find the right bank account from some 9000 bank
                            // accounts. Therefore the paged grid would be needed. But Rob decided that
                            // we currently do not build this grid into this screen, since this sreen
                            // was hardly used anyway. We are happy to wait for the first complaints.
                            //
                            // FPartnerEditScreenDS := GetBankingDetailsInternal(ReadTransaction, FPartnerKey);
                            break;

                        case TPartnerClass.UNIT:
                            PUnitAccess.LoadByPrimaryKey(FPartnerEditScreenDS, FPartnerKey, ReadTransaction);
                            UmUnitStructureAccess.LoadViaPUnitChildUnitKey(FPartnerEditScreenDS, FPartnerKey, ReadTransaction);
                            break;

                        case TPartnerClass.VENUE:
                            PVenueAccess.LoadByPrimaryKey(FPartnerEditScreenDS, FPartnerKey, ReadTransaction);
                            break;
                    }

                    #endregion

                    // financial details
                    if ((!ADelayedDataLoading) || (ATabPage == TPartnerEditTabPageEnum.petpFinanceDetails))
                    {
                        FPartnerEditScreenDS.Merge(GetBankingDetails());
                    }

                    ItemsCountPartnerBankingDetails = PPartnerBankingDetailsAccess.CountViaPPartner(FPartnerKey, ReadTransaction);

                    // Office Specific Data
                    if ((!ADelayedDataLoading) || (ATabPage == TPartnerEditTabPageEnum.petpOfficeSpecific))
                    {
                        FPartnerEditScreenDS.Merge(GetDataLocalPartnerDataValuesInternal(out OfficeSpecificDataLabelsAvailable, false));
                    }
                    else
                    {
                        FPartnerEditScreenDS.Merge(GetDataLocalPartnerDataValuesInternal(out OfficeSpecificDataLabelsAvailable, true));
                    }

                    // Console.WriteLine('FPartnerEditScreenDS.PDataLabelValuePartner.Rows.Count: ' + FPartnerEditScreenDS.PDataLabelValuePartner.Rows.Count.ToString);

                    #region Individual Data (Personnel Tab)

                    if (((!ADelayedDataLoading)) || (ATabPage == TPartnerEditTabPageEnum.petpPersonnelIndividualData))
                    {
                        FPartnerEditScreenDS.Merge(TIndividualDataWebConnector.GetData(FPartnerKey, TIndividualDataItemEnum.idiSummary));
//                      Console.WriteLine("FPartnerEditScreenDS.PDataLabelValuePartner.Rows.Count: " + FPartnerEditScreenDS.Tables["SummaryData"].Rows.Count.ToString());
                    }

                    #endregion

                    #endregion

                    #region Process data

                    // Determination of Last Gift information
                    TGift.GetLastGiftDetails(FPartnerKey, out LastGiftDate, out LastGiftInfo);

                    // Determination of Last Contact Date
                    TMailroom.GetLastContactDate(FPartnerKey, out LastContactDate);

                    // Create 'miscellaneous' DataRow
                    MiscellaneousDataDT = FPartnerEditScreenDS.MiscellaneousData;
                    MiscellaneousDataDR = MiscellaneousDataDT.NewRowTyped(false);
                    MiscellaneousDataDR.PartnerKey = FPartnerKey;

                    if (FKeyForSelectingPartnerLocation.LocationKey == 0)
                    {
                        MiscellaneousDataDR.SelectedSiteKey = LocationPK.SiteKey;
                        MiscellaneousDataDR.SelectedLocationKey = LocationPK.LocationKey;
                    }
                    else
                    {
//                      TLogging.LogAtLevel(6, "Passed in FKeyForSelectingPartnerLocation.SiteKey and FKeyForSelectingPartnerLocation.LocationKey: " +
//                          FKeyForSelectingPartnerLocation.SiteKey.ToString() + "/" + FKeyForSelectingPartnerLocation.LocationKey.ToString());

                        MiscellaneousDataDR.SelectedSiteKey = FKeyForSelectingPartnerLocation.SiteKey;
                        MiscellaneousDataDR.SelectedLocationKey = FKeyForSelectingPartnerLocation.LocationKey;
                    }

                    if (LastGiftDate != DateTime.MinValue)
                    {
                        MiscellaneousDataDR.LastGiftDate = LastGiftDate;
                    }
                    else
                    {
                        MiscellaneousDataDR.SetLastGiftDateNull();
                    }

                    if (LastContactDate != DateTime.MinValue)
                    {
                        MiscellaneousDataDR.LastContactDate = LastContactDate;
                    }
                    else
                    {
                        MiscellaneousDataDR.SetLastContactDateNull();
                    }

                    MiscellaneousDataDR.LastGiftInfo = LastGiftInfo;
                    MiscellaneousDataDR.ItemsCountAddresses = ItemsCountAddresses;
                    MiscellaneousDataDR.ItemsCountAddressesActive = ItemsCountAddressesActive;
                    MiscellaneousDataDR.ItemsCountSubscriptions = ItemsCountSubscriptions;
                    MiscellaneousDataDR.ItemsCountSubscriptionsActive = ItemsCountSubscriptionsActive;
                    MiscellaneousDataDR.ItemsCountPartnerTypes = ItemsCountPartnerTypes;
                    MiscellaneousDataDR.ItemsCountPartnerRelationships = ItemsCountPartnerRelationships;
                    MiscellaneousDataDR.ItemsCountFamilyMembers = ItemsCountFamilyMembers;
                    MiscellaneousDataDR.ItemsCountInterests = ItemsCountPartnerInterests;
                    MiscellaneousDataDR.ItemsCountPartnerBankingDetails = ItemsCountPartnerBankingDetails;
                    MiscellaneousDataDR.OfficeSpecificDataLabelsAvailable = OfficeSpecificDataLabelsAvailable;
                    MiscellaneousDataDR.FoundationOwner1Key = FoundationOwner1Key;
                    MiscellaneousDataDR.FoundationOwner2Key = FoundationOwner2Key;
                    MiscellaneousDataDR.HasEXWORKERPartnerType = HasEXWORKERPartnerType;
                    MiscellaneousDataDT.Rows.Add(MiscellaneousDataDR);
                    #endregion

                    // Add this partner key to the list of recently used partners.
                    TRecentPartnersHandling.AddRecentlyUsedPartner(FPartnerKey, FPartnerClass, false, TLastPartnerUse.lpuMailroomPartner);
                }
                catch (EPartnerLocationNotExistantException)
                {
                    // don't log this exception  this is thrown on purpose here and the Client deals with it.
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    throw;
                }
                catch (ESecurityPartnerAccessDeniedException)
                {
                    // don't log this exception  this is thrown on purpose here and the Client deals with it.
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    throw;
                }
                catch (Exception Exp)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.Log("TPartnerEditUIConnector.LoadData exception: " + Exp.ToString(), TLoggingType.ToLogfile);
                    TLogging.Log(Exp.StackTrace, TLoggingType.ToLogfile);
                    throw;
                }
            }
            finally
            {
                if (DBAccess.GDBAccessObj.Transaction != null)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }

            // Accept row changes here so that the Client gets 'unmodified' rows
            FPartnerEditScreenDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            // Examples for such DataTables are the ones that exist for a certain Partner
            // Class, eg. Person  only one of those Tables will be filled, the other ones
            // are not needed at the Client side.
            FPartnerEditScreenDS.RemoveEmptyTables();
        }

        private void LoadData(Boolean ADelayedDataLoading)
        {
            LoadData(ADelayedDataLoading, TPartnerEditTabPageEnum.petpDefault);
        }

        private void LoadData()
        {
            LoadData(false, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Passes data as a Typed DataSet to the Partner Edit Screen, containing multiple
        /// DataTables. The DataSets tables are with default data for a new Partner.
        ///
        /// </summary>
        /// <param name="ASiteKey">SiteKey for which the Partner should be created (optional)</param>
        /// <param name="APartnerKey">PartnerKey that the Partner should have (optional)</param>
        /// <param name="APartnerClass">PartnerClass that the Partner should have (optional)</param>
        /// <param name="ADesiredCountryCode">Country Code that the Partner's first address should have
        /// (if &lt;&gt; '', this overrides the Country Code that would be found out from the
        /// Site Key)</param>
        /// <param name="AAcquisitionCode">AcquisitionCode that the Partner should have (optional)</param>
        /// <param name="APrivatePartner">Set to true if this should become a Private Partner
        /// of the current user</param>
        /// <param name="AFamilyPartnerKey">PartnerKey of the Family (only needed if new Partner is of
        /// Partner Class PERSON)</param>
        /// <param name="AFamilySiteKey"></param>
        /// <param name="AFamilyLocationKey">LocationKey of the selected Location of the Family
        /// (only needed if new Partner is of Partner Class PERSON)
        /// </param>
        /// <param name="ASiteCountryCode"></param>
        /// <returns>void</returns>
        public PartnerEditTDS GetDataNewPartner(System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ADesiredCountryCode,
            String AAcquisitionCode,
            Boolean APrivatePartner,
            Int64 AFamilyPartnerKey,
            Int64 AFamilySiteKey,
            Int32 AFamilyLocationKey,
            out String ASiteCountryCode)
        {
            TDBTransaction ReadTransaction;
            PPartnerRow PartnerRow;
            PartnerEditTDSPPersonRow PersonRow;
            PartnerEditTDSPFamilyRow FamilyRow;
            PChurchRow ChurchRow;
            POrganisationRow OrganisationRow;
            PBankRow BankRow;
            PUnitRow UnitRow;
            PVenueRow VenueRow;
            PPartnerRelationshipRow PartnerRelationshipRow;
            PartnerEditTDSMiscellaneousDataTable MiscellaneousDataDT;
            PartnerEditTDSMiscellaneousDataRow MiscellaneousDataDR;
            PPartnerTable PersonFamilyPartnerDT;
            PLocationTable SiteLocationDT;
            StringCollection SiteLocationRequiredColumns;
            DateTime CreationDate;
            String CreationUserID;
            String GiftReceiptingDefaults;
            String ReceiptLetterFrequency;
            String LanguageCode;

            String[] GiftReceiptingDefaultsSplit;
            Boolean ReceiptEachGift;
            TPartnerFamilyIDHandling FamilyIDHandling;
            int FamilyID;
            String ProblemMessage;
            Int32 ItemsCountAddresses = 0;
            Int32 ItemsCountAddressesActive = 0;
            Int32 ItemsCountSubscriptions = 0;
            Int32 ItemsCountSubscriptionsActive = 0;
            Int32 ItemsCountPartnerTypes = 0;
            Int32 ItemsCountPartnerRelationships = 0;
            Int32 ItemsCountFamilyMembers = 0;
            Int32 ItemsCountInterests = 0;
            Int64 FoundationOwner1Key = 0;
            Int64 FoundationOwner2Key = 0;
            bool HasEXWORKERPartnerType = false;
            PFamilyTable PersonFamilyDT;
            TOfficeSpecificDataLabelsUIConnector OfficeSpecificDataLabelsUIConnector;
            Boolean OfficeSpecificDataLabelsAvailable;

            ASiteCountryCode = "";
            FNewPartnerPartnerKey = APartnerKey;
            FNewPartnerPartnerClass = APartnerClass;
            CreationDate = DateTime.Today;
            CreationUserID = UserInfo.GUserInfo.UserID;

            // create the FPartnerEditScreenDS DataSet that will later be passed to the Client
            FPartnerEditScreenDS = new PartnerEditTDS(DATASETNAME);
            FPartnerKey = FNewPartnerPartnerKey;
            FPartnerClass = FNewPartnerPartnerClass;

            ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                /*
                 * Create DataRow for PPartner
                 */
                #region Calculations

                // Determine Gift Processing settings
                GiftReceiptingDefaults = TSystemDefaultsCache.GSystemDefaultsCache.GetStringDefault(
                    TSystemDefaultsCache.PARTNER_GIFTRECEIPTINGDEFAULTS);

                if (GiftReceiptingDefaults != "")
                {
                    GiftReceiptingDefaultsSplit = GiftReceiptingDefaults.Split(',');
                    ReceiptLetterFrequency = GiftReceiptingDefaultsSplit[0];

                    if (GiftReceiptingDefaultsSplit[1] == "no")
                    {
                        ReceiptEachGift = false;
                    }
                    else
                    {
                        ReceiptEachGift = true;
                    }
                }
                else
                {
                    ReceiptLetterFrequency = "Annual";
                    ReceiptEachGift = false;
                }

                // Determine LanguageCode
                if (FPartnerClass != TPartnerClass.PERSON)
                {
                    LanguageCode = TUserDefaults.GetStringDefault(MSysManConstants.PARTNER_LANGUAGE, "99");
                }
                else
                {
                    PersonFamilyPartnerDT = PPartnerAccess.LoadByPrimaryKey(AFamilyPartnerKey, ReadTransaction);

                    if (PersonFamilyPartnerDT[0].LanguageCode == "99")
                    {
                        LanguageCode = TUserDefaults.GetStringDefault(MSysManConstants.PARTNER_LANGUAGE, "99");
                    }
                    else
                    {
                        LanguageCode = PersonFamilyPartnerDT[0].LanguageCode;
                    }
                }

                #endregion

                // We get the default values for all DataColumns
                // and then modify some.
                PartnerRow = FPartnerEditScreenDS.PPartner.NewRowTyped(true);
                PartnerRow.PartnerKey = FPartnerKey;
                PartnerRow.DateCreated = CreationDate;
                PartnerRow.CreatedBy = CreationUserID;
                PartnerRow.PartnerClass = SharedTypes.PartnerClassEnumToString(APartnerClass);
                PartnerRow.AcquisitionCode = AAcquisitionCode;

                // logical DataColumns must be initialised for DataBinding to work
                PartnerRow.NoSolicitations = false;
                PartnerRow.DeletedPartner = false;
                PartnerRow.ChildIndicator = false;
                PartnerRow.ReceiptLetterFrequency = ReceiptLetterFrequency;
                PartnerRow.ReceiptEachGift = ReceiptEachGift;
                PartnerRow.LanguageCode = LanguageCode;

                if (!APrivatePartner)
                {
                    PartnerRow.StatusCode = SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE);
                }
                else
                {
                    PartnerRow.StatusCode = SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscPRIVATE);
                    PartnerRow.Restricted = SharedConstants.PARTNER_PRIVATE_USER;
                    PartnerRow.UserId = CreationUserID;
                }

                PartnerRow.StatusChange = CreationDate;
                #region Partner Types
                TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetDataNewPartner: before PartnerClass switch");

                switch (APartnerClass)
                {
                    case TPartnerClass.PERSON:
                        // Load p_family record of the FAMILY that the PERSON will belong to
                        PersonFamilyDT = PFamilyAccess.LoadByPrimaryKey(AFamilyPartnerKey, ReadTransaction);

                        // Create DataRow for PPerson using the default values for all DataColumns
                        TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetDataNewPartner: before adding Person DataRow");

                        PersonRow = FPartnerEditScreenDS.PPerson.NewRowTyped(true);
                        PersonRow.PartnerKey = FPartnerKey;
                        PersonRow.DateCreated = CreationDate;
                        PersonRow.CreatedBy = CreationUserID;
                        PersonRow.FamilyKey = AFamilyPartnerKey;
                        PersonRow.FamilyName = PersonFamilyDT[0].FamilyName;
                        FamilyIDHandling = new TPartnerFamilyIDHandling();

                        if (FamilyIDHandling.GetNewFamilyID(AFamilyPartnerKey, out FamilyID, out ProblemMessage) == TFamilyIDSuccessEnum.fiError)
                        {
                            // this should not really happen  but we cannot continue if it does
                            throw new EPartnerFamilyIDException(ProblemMessage);
                        }

                        PersonRow.FamilyId = FamilyID;
                        PersonRow.OccupationCode = MPartnerConstants.DEFAULT_CODE_UNKNOWN;
                        FPartnerEditScreenDS.PPerson.Rows.Add(PersonRow);
                        GetFamilyMembersInternal(AFamilyPartnerKey, "", out ItemsCountFamilyMembers, true);

                        /*
                         * Remove other Partner Class Tables.
                         * This is needed for correct working of the creation of new Partners on the Client side (the ShowData Method of the 'Top Part'
                         * relies on null DataTables when determining which data of which DataTables to put into which Controls).
                         */
                        FPartnerEditScreenDS.Tables.Remove(PFamilyTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(POrganisationTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PChurchTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PBankTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PUnitTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PVenueTable.GetTableName());

                        break;

                    case TPartnerClass.FAMILY:
                        TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetDataNewPartner: before adding Family DataRow");

                        // Create DataRow for PFamily using the default values for all DataColumns
                        FamilyRow = FPartnerEditScreenDS.PFamily.NewRowTyped(true);
                        FamilyRow.PartnerKey = FPartnerKey;
                        FamilyRow.DateCreated = CreationDate;
                        FamilyRow.CreatedBy = CreationUserID;
                        FPartnerEditScreenDS.PFamily.Rows.Add(FamilyRow);

                        /*
                         * Remove other Partner Class Tables.
                         * This is needed for correct working of the creation of new Partners on the Client side (the ShowData Method of the 'Top Part'
                         * relies on null DataTables when determining which data of which DataTables to put into which Controls).
                         */
                        FPartnerEditScreenDS.Tables.Remove(PPersonTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(POrganisationTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PChurchTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PBankTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PUnitTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PVenueTable.GetTableName());

                        break;

                    case TPartnerClass.CHURCH:
                        // Create DataRow for PChurch using the default values for all DataColumns
                        ChurchRow = FPartnerEditScreenDS.PChurch.NewRowTyped(true);
                        ChurchRow.PartnerKey = FPartnerKey;
                        ChurchRow.DateCreated = CreationDate;
                        ChurchRow.CreatedBy = CreationUserID;

                        // logical DataColumns must be initialised for DataBinding to work
                        ChurchRow.Accomodation = false;
                        ChurchRow.PrayerGroup = false;
                        ChurchRow.MapOnFile = false;
                        FPartnerEditScreenDS.PChurch.Rows.Add(ChurchRow);

                        /*
                         * Remove other Partner Class Tables.
                         * This is needed for correct working of the creation of new Partners on the Client side (the ShowData Method of the 'Top Part'
                         * relies on null DataTables when determining which data of which DataTables to put into which Controls).
                         */
                        FPartnerEditScreenDS.Tables.Remove(PPersonTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PFamilyTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(POrganisationTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PBankTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PUnitTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PVenueTable.GetTableName());

                        break;

                    case TPartnerClass.ORGANISATION:
                        // Create DataRow for POrganisation using the default values for all DataColumns
                        OrganisationRow = FPartnerEditScreenDS.POrganisation.NewRowTyped(true);
                        OrganisationRow.PartnerKey = FPartnerKey;
                        OrganisationRow.DateCreated = CreationDate;
                        OrganisationRow.CreatedBy = CreationUserID;

                        // logical DataColumns must be initialised for DataBinding to work
                        OrganisationRow.Religious = false;
                        OrganisationRow.Foundation = false;
                        FPartnerEditScreenDS.POrganisation.Rows.Add(OrganisationRow);

                        /*
                         * Remove other Partner Class Tables.
                         * This is needed for correct working of the creation of new Partners on the Client side (the ShowData Method of the 'Top Part'
                         * relies on null DataTables when determining which data of which DataTables to put into which Controls).
                         */
                        FPartnerEditScreenDS.Tables.Remove(PPersonTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PFamilyTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PChurchTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PBankTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PUnitTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PVenueTable.GetTableName());

                        break;

                    case TPartnerClass.BANK:
                        // Create DataRow for PBank using the default values for all DataColumns
                        BankRow = FPartnerEditScreenDS.PBank.NewRowTyped(true);
                        BankRow.PartnerKey = FPartnerKey;
                        BankRow.DateCreated = CreationDate;
                        BankRow.CreatedBy = CreationUserID;
                        FPartnerEditScreenDS.PBank.Rows.Add(BankRow);

                        /*
                         * Remove other Partner Class Tables.
                         * This is needed for correct working of the creation of new Partners on the Client side (the ShowData Method of the 'Top Part'
                         * relies on null DataTables when determining which data of which DataTables to put into which Controls).
                         */
                        FPartnerEditScreenDS.Tables.Remove(PPersonTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PFamilyTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(POrganisationTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PChurchTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PUnitTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PVenueTable.GetTableName());

                        break;

                    case TPartnerClass.UNIT:
                        // Create DataRow for PUnit using the default values for all DataColumns
                        UnitRow = FPartnerEditScreenDS.PUnit.NewRowTyped(true);
                        UnitRow.PartnerKey = FPartnerKey;
                        UnitRow.DateCreated = CreationDate;
                        UnitRow.CreatedBy = CreationUserID;
                        FPartnerEditScreenDS.PUnit.Rows.Add(UnitRow);

                        /*
                         * Remove other Partner Class Tables.
                         * This is needed for correct working of the creation of new Partners on the Client side (the ShowData Method of the 'Top Part'
                         * relies on null DataTables when determining which data of which DataTables to put into which Controls).
                         */
                        FPartnerEditScreenDS.Tables.Remove(PPersonTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PFamilyTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PChurchTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PBankTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(POrganisationTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PVenueTable.GetTableName());

                        break;

                    case TPartnerClass.VENUE:
                        // Create DataRow for PVenue using the default values for all DataColumns
                        VenueRow = FPartnerEditScreenDS.PVenue.NewRowTyped(true);
                        VenueRow.PartnerKey = FPartnerKey;
                        VenueRow.DateCreated = CreationDate;
                        VenueRow.CreatedBy = CreationUserID;

                        // Makeup a VenueCode for now, as there is now way to let the user spec one quickly...
                        VenueRow.VenueCode = 'V' + FPartnerKey.ToString().Substring(1);
                        FPartnerEditScreenDS.PVenue.Rows.Add(VenueRow);

                        /*
                         * Remove other Partner Class Tables.
                         * This is needed for correct working of the creation of new Partners on the Client side (the ShowData Method of the 'Top Part'
                         * relies on null DataTables when determining which data of which DataTables to put into which Controls).
                         */
                        FPartnerEditScreenDS.Tables.Remove(PPersonTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PFamilyTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PChurchTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PBankTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(POrganisationTable.GetTableName());
                        FPartnerEditScreenDS.Tables.Remove(PUnitTable.GetTableName());

                        break;
                }

                PartnerRow.AddresseeTypeCode = TSharedAddressHandling.GetDefaultAddresseeType(APartnerClass);
                TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetDataNewPartner: before adding new Partner DataRow");

                // Add DataRow for PPartner
                FPartnerEditScreenDS.PPartner.Rows.Add(PartnerRow);

                if (APartnerClass == TPartnerClass.PERSON)
                {
                    /*
                     * Copy specified Family Address into the PLocation and PPartnerLocation
                     * table (is needed on the Client side to copy this address as the default
                     * address of the new Person and gets deleted there as soon as copying is
                     * done)
                     */
//                  TLogging.LogAtLevel(7, "Getting Family Address - AFamilyPartnerKey: " + AFamilyPartnerKey.ToString() + "; AFamilyLocationKey: " + AFamilyLocationKey.ToString());
                    TPPartnerAddressAggregate.LoadByPrimaryKey(FPartnerEditScreenDS,
                        AFamilyPartnerKey,
                        AFamilySiteKey,
                        AFamilyLocationKey,
                        ReadTransaction);

                    // Copy Special Types from the PERSON'S FAMILY to the PERSON
                    TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetDataNewPartner: before loading Special Types from FAMILY");

                    GetPartnerTypesForNewPartnerFromFamily(AFamilyPartnerKey, ReadTransaction);

                    /*
                     * Create Relationship between Family and Person
                     */
                    PartnerRelationshipRow = FPartnerEditScreenDS.PPartnerRelationship.NewRowTyped(true);
                    PartnerRelationshipRow.PartnerKey = AFamilyPartnerKey;
                    PartnerRelationshipRow.RelationKey = FPartnerKey;
                    PartnerRelationshipRow.RelationName = "FAMILY";
                    PartnerRelationshipRow.Comment = SharedConstants.ROW_IS_SYSTEM_GENERATED;
                    PartnerRelationshipRow.DateCreated = CreationDate;
                    PartnerRelationshipRow.CreatedBy = CreationUserID;
                    FPartnerEditScreenDS.PPartnerRelationship.Rows.Add(PartnerRelationshipRow);
                }

                #endregion

                // Note: Location and PartnerLocation for a new Partner are set up on the Client side!
                // Determine CountryCode for first Address of the Partner, if it is not overwritten
                if (ADesiredCountryCode == "")
                {
                    // TODO 1 oChristianK cPartner Edit / New Partner : Use a proper function to determine the 'Best' Address of the Unit...
                    SiteLocationRequiredColumns = new StringCollection();
                    SiteLocationRequiredColumns.Add(PLocationTable.GetLocationKeyDBName());
                    SiteLocationRequiredColumns.Add(PLocationTable.GetCountryCodeDBName());
                    SiteLocationDT = PLocationAccess.LoadViaPPartner(ASiteKey, SiteLocationRequiredColumns, ReadTransaction, null, 0, 0);

                    // For the moment just take the CountryCode that we find in the (random) first record...
                    ASiteCountryCode = SiteLocationDT[0].CountryCode;
                }

                /*
                 * Office Specific Data
                 */
                OfficeSpecificDataLabelsUIConnector = new TOfficeSpecificDataLabelsUIConnector(FPartnerKey,
                    MCommonTypes.PartnerClassEnumToOfficeSpecificDataLabelUseEnum(FPartnerClass));
                OfficeSpecificDataLabelsAvailable =
                    (OfficeSpecificDataLabelsUIConnector.CountLabelUse(SharedTypes.PartnerClassEnumToString(FPartnerClass), ReadTransaction) != 0);


                MiscellaneousDataDT = FPartnerEditScreenDS.MiscellaneousData;
                MiscellaneousDataDR = MiscellaneousDataDT.NewRowTyped(false);
                MiscellaneousDataDR.PartnerKey = FPartnerKey;

                if (AFamilyLocationKey != 0)
                {
                    MiscellaneousDataDR.SelectedSiteKey = AFamilySiteKey;
                    MiscellaneousDataDR.SelectedLocationKey = AFamilyLocationKey;
                }
                else
                {
                    MiscellaneousDataDR.SelectedSiteKey = -1;
                    MiscellaneousDataDR.SelectedLocationKey = -1;
                }

                MiscellaneousDataDR.SetLastGiftDateNull();
                MiscellaneousDataDR.LastGiftInfo = "";
                MiscellaneousDataDR.ItemsCountAddresses = ItemsCountAddresses;
                MiscellaneousDataDR.ItemsCountAddressesActive = ItemsCountAddressesActive;
                MiscellaneousDataDR.ItemsCountSubscriptions = ItemsCountSubscriptions;
                MiscellaneousDataDR.ItemsCountSubscriptionsActive = ItemsCountSubscriptionsActive;
                MiscellaneousDataDR.ItemsCountPartnerTypes = ItemsCountPartnerTypes;
                MiscellaneousDataDR.ItemsCountPartnerRelationships = ItemsCountPartnerRelationships;
                MiscellaneousDataDR.ItemsCountFamilyMembers = ItemsCountFamilyMembers;
                MiscellaneousDataDR.ItemsCountInterests = ItemsCountInterests;
                MiscellaneousDataDR.OfficeSpecificDataLabelsAvailable = OfficeSpecificDataLabelsAvailable;
                MiscellaneousDataDR.FoundationOwner1Key = FoundationOwner1Key;
                MiscellaneousDataDR.FoundationOwner2Key = FoundationOwner2Key;
                MiscellaneousDataDR.HasEXWORKERPartnerType = HasEXWORKERPartnerType;
                MiscellaneousDataDT.Rows.Add(MiscellaneousDataDR);
                MiscellaneousDataDT.AcceptChanges();
            }
            catch (Exception Exp)
            {
                TLogging.Log("Exception occured in GetDataNewPartner: " + Exp.ToString());
            }
            DBAccess.GDBAccessObj.CommitTransaction();

            // Remove all Tables that were not filled with data before remoting them
            // This will be the DataTables that exist for a certain Partner Class,
            // eg. Person  only one of those Tables will be filled, the other ones are
            // not needed at the Client side.
            // FPartnerEditScreenDS.RemoveEmptyTables;
            // For the moment we only remove the following table so that the Partner
            // Edit screen can discover the value of MiscellaneousDataDR.ItemsCountFamilyMembers
            FPartnerEditScreenDS.Tables.Remove(PartnerEditTDSFamilyMembersTable.GetTableName());

            return FPartnerEditScreenDS;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public IndividualDataTDS GetDataPersonnelIndividualData(TIndividualDataItemEnum AIndividualDataItem)
        {
            return GetDataPersonnelIndividualDataInternal(AIndividualDataItem);
        }

        private IndividualDataTDS GetDataPersonnelIndividualDataInternal(TIndividualDataItemEnum AIndividualDataItem)
        {
            return TIndividualDataWebConnector.GetData(FPartnerKey, AIndividualDataItem);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public PPartnerTypeTable GetDataPartnerTypes()
        {
            Int32 PartnerTypesCount;

            return GetPartnerTypesInternal(out PartnerTypesCount, false);
        }

        private PPartnerTypeTable GetPartnerTypesInternal(out Int32 ACount, Boolean ACountOnly)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            PPartnerTypeTable PartnerTypesDT;

            PartnerTypesDT = new PPartnerTypeTable();
            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                if (ACountOnly)
                {
                    ACount = PPartnerTypeAccess.CountViaPPartner(FPartnerKey, ReadTransaction);
                }
                else
                {
//                  TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetDataPartnerTypesInternal: loading Partner Types for Partner " + FPartnerKey.ToString() + "...");
                    try
                    {
                        PartnerTypesDT = PPartnerTypeAccess.LoadViaPPartner(FPartnerKey, ReadTransaction);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    ACount = PartnerTypesDT.Rows.Count;
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetDataPartnerTypesInternal: committed own transaction.");
                }
            }
            return PartnerTypesDT;
        }

        private void GetPartnerTypesForNewPartnerFromFamily(Int64 AFamilyPartnerKey, TDBTransaction AReadTransaction)
        {
            PPartnerTypeTable PartnerTypeDT;
            PPartnerTypeRow NewDR;

            // Load PartnerTypes that the FAMILY has
            PartnerTypeDT = PPartnerTypeAccess.LoadViaPPartner(AFamilyPartnerKey, AReadTransaction);

            // Copy over all PartnerTypes from the FAMILY to the new PERSON
            for (int Counter = 0; Counter < PartnerTypeDT.Rows.Count; Counter++)
            {
                NewDR = FPartnerEditScreenDS.PPartnerType.NewRowTyped();
                NewDR.PartnerKey = FPartnerKey;
                NewDR.TypeCode = PartnerTypeDT[Counter].TypeCode;

                FPartnerEditScreenDS.PPartnerType.Rows.Add(NewDR);
            }
        }

        private PPartnerTypeTable GetPartnerTypesInternal(out Int32 ACount)
        {
            return GetPartnerTypesInternal(out ACount, false);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public PSubscriptionTable GetDataSubscriptions()
        {
            Int32 SubscriptionsCount;

            return GetSubscriptionsInternal(out SubscriptionsCount, false);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public PartnerEditTDSPPartnerRelationshipTable GetDataPartnerRelationships()
        {
            Int32 RelationshipsCount;

            return GetPartnerRelationshipsInternal(out RelationshipsCount, false);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public PDataLabelValuePartnerTable GetDataLocalPartnerDataValues()
        {
            Boolean LabelsAvailable;

            return GetDataLocalPartnerDataValuesInternal(out LabelsAvailable, false);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AFamilyPartnerKey"></param>
        /// <param name="AWorkWithSpecialType"></param>
        /// <returns></returns>
        public PartnerEditTDSFamilyMembersTable GetDataFamilyMembers(Int64 AFamilyPartnerKey, String AWorkWithSpecialType)
        {
            Int32 FamilyMembersCount;

            return GetFamilyMembersInternal(AFamilyPartnerKey, AWorkWithSpecialType, out FamilyMembersCount, false);
        }

        private PartnerEditTDSFamilyMembersTable GetFamilyMembersInternal(Int64 AFamilyPartnerKey,
            String AWorkWithSpecialType,
            out Int32 ACount,
            Boolean ACountOnly)
        {
            TDBTransaction ReadTransaction;

            OdbcParameter[] ParametersArray;
            Boolean NewTransaction = false;
            PartnerEditTDSFamilyMembersTable PartnerTypeFamilyMembersDT;
            PPersonTable FamilyPersonsDT;
            PPartnerTypeTable PartnerTypesDT;
            DataRow[] PartnerTypesFoundRows;
            PartnerEditTDSFamilyMembersRow NewRow;
            StringCollection RequiredColumns;
            int Counter;
            Boolean TypeCodePresent;
            DataSet TmpDS;


            PartnerTypeFamilyMembersDT = new PartnerEditTDSFamilyMembersTable();

            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                    TEnforceIsolationLevel.eilMinimum, out NewTransaction);

                ParametersArray = new OdbcParameter[1];
                ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
                ParametersArray[0].Value = (System.Object)AFamilyPartnerKey;

                if (ACountOnly)
                {
                    ACount = Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(
                            "SELECT COUNT(*) " +
                            "FROM PUB_" + PPersonTable.GetTableDBName() +
                            " INNER JOIN " + "PUB_" + PPartnerTable.GetTableDBName() +
                            " ON " + "PUB_" + PPersonTable.GetTableDBName() + '.' +
                            PPartnerTable.GetPartnerKeyDBName() + " = " +
                            "PUB_" + PPartnerTable.GetTableDBName() + '.' +
                            PPartnerTable.GetPartnerKeyDBName() + ' ' +
                            "WHERE " + PPersonTable.GetFamilyKeyDBName() + " = ? " +
                            "AND " + PPartnerTable.GetStatusCodeDBName() + " <> '" +
                            SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscMERGED) + "'", ReadTransaction,
                            ParametersArray));

                    // Make sure we don't count MERGED Partners (shouldn't have a p_family_key_n, but just in case.)
                }
                else
                {
                    // Find all Persons that belong to the Family
//                  TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetFamilyMembersInternal: loading Persons for Family " + AFamilyPartnerKey.ToString() + "...");

                    TmpDS = new DataSet();
                    FamilyPersonsDT = new PPersonTable();
                    TmpDS.Tables.Add(FamilyPersonsDT);
                    DBAccess.GDBAccessObj.Select(TmpDS,
                        "SELECT " + "PUB_" + PPartnerTable.GetTableDBName() + '.' +
                        PPartnerTable.GetPartnerKeyDBName() + ", " +
                        PPersonTable.GetFamilyNameDBName() + ", " +
                        PPersonTable.GetTitleDBName() + ", " +
                        PPersonTable.GetFirstNameDBName() + ", " +
                        PPersonTable.GetMiddleName1DBName() + ", " +
                        PPersonTable.GetFamilyIdDBName() + ", " +
                        PPersonTable.GetGenderDBName() + ", " +
                        PPersonTable.GetDateOfBirthDBName() + ' ' +
                        "FROM PUB_" + PPersonTable.GetTableDBName() +
                        " INNER JOIN " + "PUB_" + PPartnerTable.GetTableDBName() + " ON " +
                        "PUB_" + PPersonTable.GetTableDBName() + '.' +
                        PPartnerTable.GetPartnerKeyDBName() + " = " +
                        "PUB_" + PPartnerTable.GetTableDBName() + '.' +
                        PPartnerTable.GetPartnerKeyDBName() + ' ' +
                        "WHERE " + PPersonTable.GetFamilyKeyDBName() + " = ? " +
                        "AND " + PPartnerTable.GetStatusCodeDBName() + " <> '" +
                        SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscMERGED) + "'",    // Make sure we don't load MERGED Partners (shouldn't have a p_family_key_n, but just in case.)
                        PPersonTable.GetTableName(), ReadTransaction, ParametersArray, 0, 0);

                    ACount = FamilyPersonsDT.Rows.Count;

                    RequiredColumns = new StringCollection();
                    RequiredColumns.Add(PPartnerTypeTable.GetTypeCodeDBName());

                    TypeCodePresent = false;

                    // Add Persons to Table
                    for (Counter = 0; Counter <= FamilyPersonsDT.Rows.Count - 1; Counter += 1)
                    {
                        // Load a Person's SpecialTypes if requested
                        if (AWorkWithSpecialType != "")
                        {
                            PartnerTypesDT = PPartnerTypeAccess.LoadViaPPartner(
                                FamilyPersonsDT[Counter].PartnerKey,
                                RequiredColumns,
                                ReadTransaction,
                                null,
                                0,
                                0);

                            if (PartnerTypesDT.Rows.Count != 0)
                            {
                                // check if the searched for Special Type is present
                                PartnerTypesFoundRows = PartnerTypesDT.Select(
                                    PPartnerTypeTable.GetTypeCodeDBName() + " = '" + AWorkWithSpecialType + "'");

                                if ((PartnerTypesFoundRows != null) && (PartnerTypesFoundRows.Length > 0))
                                {
                                    TypeCodePresent = true;
                                }
                                else
                                {
                                    TypeCodePresent = false;
                                }
                            }
                            else
                            {
                                TypeCodePresent = false;
                            }
                        }

                        NewRow = PartnerTypeFamilyMembersDT.NewRowTyped(false);
                        NewRow.PartnerKey = FamilyPersonsDT[Counter].PartnerKey;
                        NewRow.PartnerShortName =
                            Calculations.DeterminePartnerShortName(TSaveConvert.StringColumnToString(FamilyPersonsDT.ColumnFamilyName,
                                    FamilyPersonsDT[Counter]), TSaveConvert.StringColumnToString(FamilyPersonsDT.ColumnTitle,
                                    FamilyPersonsDT[Counter]), TSaveConvert.StringColumnToString(FamilyPersonsDT.ColumnFirstName,
                                    FamilyPersonsDT[Counter]),
                                TSaveConvert.StringColumnToString(FamilyPersonsDT.ColumnMiddleName1, FamilyPersonsDT[Counter]));
                        NewRow.FamilyId = FamilyPersonsDT[Counter].FamilyId;

                        if (AWorkWithSpecialType != "")
                        {
                            NewRow.TypeCodePresent = TypeCodePresent;
                            NewRow.TypeCodeModify = false;

                            // TODO 2 oChristianK cSpecial Types / Family Members : Add all other Special Types of each Person
                            NewRow.OtherTypeCodes = "#NOT YET RETRIEVED FROM DB!#";
                        }

                        NewRow.Gender = FamilyPersonsDT[Counter].Gender;

                        try
                        {
                            NewRow.DateOfBirth = FamilyPersonsDT[Counter].DateOfBirth;
                        }
                        catch (System.Data.StrongTypingException)
                        {
                            NewRow.DateOfBirth = System.DateTime.MinValue;
                        }

                        PartnerTypeFamilyMembersDT.Rows.Add(NewRow);
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetFamilyMembersInternal: committed own transaction.");
                }
            }
            return PartnerTypeFamilyMembersDT;
        }

        private PartnerEditTDSPPartnerInterestTable GetPartnerInterestsInternal(out Int32 ACount, Boolean ACountOnly)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            PartnerEditTDSPPartnerInterestTable PartnerInterestDT;
            PInterestTable InterestDT;
            DataRow InterestRow;
            int ItemsCountInterests;

            PartnerInterestDT = new PartnerEditTDSPPartnerInterestTable();
            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                if (ACountOnly)
                {
                    ACount = PPartnerInterestAccess.CountViaPPartner(FPartnerKey, ReadTransaction);
                }
                else
                {
//                  TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetPartnerInterestsInternal: loading Interests for Partner " + FPartnerKey.ToString() + "...");
                    try
                    {
                        PartnerInterestDT.Merge(PPartnerInterestAccess.LoadViaPPartner(FPartnerKey, ReadTransaction));
                        InterestDT = GetInterestsInternal(out ItemsCountInterests, false);

                        // fill field for interest category in PartnerInterest table in dataset
                        foreach (PartnerEditTDSPPartnerInterestRow row in PartnerInterestDT.Rows)
                        {
                            InterestRow = InterestDT.Rows.Find(new object[] { row.Interest });

                            if (InterestRow != null)
                            {
                                row.InterestCategory = ((PInterestRow)InterestRow).Category;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    ACount = PartnerInterestDT.Rows.Count;
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetPartnerInterestsInternal: committed own transaction.");
                }
            }
            return PartnerInterestDT;
        }

        private PartnerEditTDSPPartnerInterestTable GetPartnerInterestsInternal(out Int32 ACount)
        {
            return GetPartnerInterestsInternal(out ACount, false);
        }

        private PInterestTable GetInterestsInternal(out Int32 ACount, Boolean ACountOnly)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            PInterestTable InterestDT;

            ACount = 0;
            InterestDT = new PInterestTable();

            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                if (ACountOnly)
                {
                }
                // ACount := PInterestAccess.LoadAll(
                // FPartnerKey, ReadTransaction);
                else
                {
//                  TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetInterestsInternal: loading Interests for Partner " + FPartnerKey.ToString() + "...");
                    try
                    {
                        InterestDT = PInterestAccess.LoadAll(ReadTransaction);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    ACount = InterestDT.Rows.Count;
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetInterestsInternal: committed own transaction.");
                }
            }
            return InterestDT;
        }

        private PInterestTable GetInterestsInternal(out Int32 ACount)
        {
            return GetInterestsInternal(out ACount, false);
        }

        private void LogAfterSaving(PartnerEditTDS AInspectDS)
        {
            for (Int16 TmpCounter = 0; TmpCounter <= AInspectDS.Tables.Count - 1; TmpCounter += 1)
            {
                Console.WriteLine(
                    "AInspectDS.Tables[" + TmpCounter.ToString() + "].TableName: " + AInspectDS.Tables[TmpCounter].TableName);
            }

            if (AInspectDS.Tables.Contains(PLocationTable.GetTableName()))
            {
                for (Int16 TmpCounter = 0; TmpCounter <= AInspectDS.Tables[PLocationTable.GetTableName()].Rows.Count - 1; TmpCounter += 1)
                {
                    if (AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpCounter].RowState != DataRowState.Deleted)
                    {
                        Console.WriteLine(
                            PLocationTable.GetTableName() + "[" + TmpCounter.ToString() + "]: PLocationKey: " +
                            AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpCounter][PLocationTable.GetLocationKeyDBName()].
                            ToString() +
                            "(); PSiteKey: " +
                            AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpCounter][PLocationTable.GetSiteKeyDBName()].ToString(
                                ) +
                            Environment.NewLine);
                    }
                    else
                    {
                        Console.WriteLine(
                            PLocationTable.GetTableName() + "[" + TmpCounter.ToString() + "]: DELETED ROW! PLocationKey: " +
                            AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpCounter][PLocationTable.GetLocationKeyDBName(),
                                                                                              DataRowVersion.Original].ToString() +
                            "(); PSiteKey: " +
                            AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpCounter][PLocationTable.GetSiteKeyDBName(),
                                                                                              DataRowVersion.Original].ToString() +
                            Environment.NewLine);
                    }
                }
            }

            if (AInspectDS.Tables.Contains(PPartnerLocationTable.GetTableName()))
            {
                Console.WriteLine("");

                for (Int16 TmpCounter = 0;
                     TmpCounter <= AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows.Count - 1;
                     TmpCounter += 1)
                {
                    if (AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpCounter].RowState != DataRowState.Deleted)
                    {
                        Console.WriteLine(
                            PPartnerLocationTable.GetTableName() + "[" + TmpCounter.ToString() + "]: PLocationKey: " +
                            AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpCounter][PPartnerLocationTable.
                                                                                                     GetLocationKeyDBName()].ToString() +
                            "(); PSiteKey: " +
                            AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpCounter][PPartnerLocationTable.
                                                                                                     GetSiteKeyDBName()]
                            .ToString() + "(); PPartnerKey: " +
                            AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpCounter][PPartnerLocationTable.
                                                                                                     GetPartnerKeyDBName(
                                                                                                         )].ToString() +
                            Environment.NewLine);
                    }
                    else
                    {
                        Console.WriteLine(
                            PPartnerLocationTable.GetTableName() + "[" + TmpCounter.ToString() + "]: DELETED ROW! PLocationKey: " +
                            AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpCounter][PPartnerLocationTable.
                                                                                                     GetLocationKeyDBName(),
                                                                                                     DataRowVersion.Original].ToString()
                            +
                            "(); PSiteKey: " +
                            AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpCounter][PPartnerLocationTable.
                                                                                                     GetSiteKeyDBName(),
                                                                                                     DataRowVersion.Original].ToString()
                            +
                            Environment.NewLine);
                    }
                }
            }
        }

        /// <summary>
        /// Saves data from the Partner Edit Screen (contained in a Typed DataSet).
        ///
        /// All DataTables contained in the Typed DataSet are inspected for added,
        /// changed or deleted rows by submitting them to the DataStore.
        ///
        /// </summary>
        /// <param name="AInspectDS">Typed DataSet that needs to contain known DataTables</param>
        /// <param name="AResponseDS">DataSet containing data to be used on the Client side
        /// (eg. for displaying Address Change dialogs with data in it)</param>
        /// <param name="AVerificationResult">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions)</param>
        /// <returns>true if all verifications are OK and all DB calls succeeded, false if
        /// any verification or DB call failed
        /// </returns>
        public TSubmitChangesResult SubmitChanges(ref PartnerEditTDS AInspectDS,
            ref DataSet AResponseDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult;
            PartnerAddressAggregateTDS TmpResponseDS = null;

            AVerificationResult = null;

            if (AInspectDS != null)
            {
                #region Work with AResponseDS if present

                if (AResponseDS != null)
                {
//                  TLogging.LogAtLevel(8, "AResponseDS.Tables.Count: " + AResponseDS.Tables.Count.ToString());

                    if (AResponseDS.Tables.Contains(MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME))
                    {
//                      TLogging.LogAtLevel(8, MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME + " Type: " +
//                          AResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].GetType().ToString() + "; Rows.Count: " +
//                          AResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].Rows.Count.ToString());
                    }

                    // AResponseDS is present: make a local copy and set AResponseDS to nil
                    TmpResponseDS = new PartnerAddressAggregateTDS(MPartnerConstants.PARTNERADDRESSAGGREGATERESPONSE_DATASET);
                    TmpResponseDS.Merge(AResponseDS);
                    AResponseDS = null;
//                  TLogging.LogAtLevel(8, "TmpResponseDS.Tables.Count: " + TmpResponseDS.Tables.Count.ToString());

                    if (TmpResponseDS.Tables.Contains(MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME))
                    {
//                      TLogging.LogAtLevel(8, MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME + " Type: " +
//                          TmpResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].GetType().ToString() +
//                          "; Rows.Count: " + TmpResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].Rows.Count.ToString());
                    }
                }

                #endregion

                TVerificationResultCollection SingleVerificationResultCollection;
                AVerificationResult = new TVerificationResultCollection();
                TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                PrepareBankingDetailsForSaving(ref AInspectDS, ref AVerificationResult, SubmitChangesTransaction);

                if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    return TSubmitChangesResult.scrError;
                }

                FSubmissionDS = AInspectDS;

                try
                {
                    if (SubmitChangesOther(ref FSubmissionDS, SubmitChangesTransaction, out SingleVerificationResultCollection))
                    {
                        SubmissionResult = TSubmitChangesResult.scrOK;
                    }
                    else
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }

                    AVerificationResult.AddCollection(SingleVerificationResultCollection);

                    if (SubmissionResult != TSubmitChangesResult.scrError)
                    {
                        TSubmitChangesResult SubmitChangesAddressResult = SubmitChangesAddresses(ref FSubmissionDS,
                            SubmitChangesTransaction,
                            ref TmpResponseDS,
                            out SingleVerificationResultCollection);

                        if (SubmitChangesAddressResult == TSubmitChangesResult.scrOK)
                        {
                            // don't need to do anything here; SubmissionResult is set already
                        }
                        else
                        {
                            SubmissionResult = SubmitChangesAddressResult;

                            if (SubmitChangesAddressResult == TSubmitChangesResult.scrInfoNeeded)
                            {
                                AResponseDS = (DataSet)TmpResponseDS;
//                              TLogging.LogAtLevel(8, "AResponseDS.Tables.Count: " + AResponseDS.Tables.Count.ToString());

                                if (AResponseDS.Tables.Contains(MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME))
                                {
//                                  TLogging.LogAtLevel(7, MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME + " Type: " +
//                                      AResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].GetType().ToString());
                                    if (TLogging.DL >= 8)
                                    {
                                        if (AResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].Rows.Count > 0)
                                        {
                                            Console.WriteLine(
                                                MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME + "[0].AnswerProcessedClientSide: " +
                                                ((PartnerAddressAggregateTDSSimilarLocationParametersTable)(AResponseDS.Tables[MPartnerConstants.
                                                                                                                               EXISTINGLOCATIONPARAMETERS_TABLENAME
                                                                                                            ]))[0].AnswerProcessedClientSide.ToString());
                                        }
                                    }
                                }

                                if (AResponseDS.Tables.Contains(MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME))
                                {
//                                  TLogging.LogAtLevel(8,  MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME + " Type: " +
//                                      AResponseDS.Tables[MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME].GetType().ToString());
                                    if (TLogging.DL >= 8)
                                    {
                                        if (AResponseDS.Tables[MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME].Rows.Count > 0)
                                        {
                                            Console.WriteLine(
                                                MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME + "[0].AnswerProcessedClientSide: " +
                                                ((PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable)(AResponseDS.Tables[MPartnerConstants
                                                                                                                                    .
                                                                                                                                    ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME
                                                                                                                 ]))[0].AnswerProcessedClientSide.
                                                ToString());
                                        }
                                    }
                                }
                            }
                        }

                        AVerificationResult.AddCollection(SingleVerificationResultCollection);
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        // all tables in the dataset will be stored.
                        // there are exceptions: for example cascading delete of foundations, change of unique key of family id
                        // those tables need to have run AcceptChanges
                        PartnerEditTDSAccess.SubmitChanges(AInspectDS);
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        // Save data from the Personnel Data part (needs to be done here towards the end of saving
                        // as p_person record needs to be saved earlier in the process and is referenced from data saved here.
                        if (SubmitChangesPersonnelData(ref FSubmissionDS, SubmitChangesTransaction, out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                            AVerificationResult.AddCollection(SingleVerificationResultCollection);
                        }
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        TLogging.LogAtLevel(6, "TPartnerEditUIConnector.SubmitChanges: Before check for new Partner for Recent Partner handling...");

                        // Check if this is a new Partner
                        if ((AInspectDS.Tables.Contains(PPartnerTable.GetTableName()))
                            && ((AInspectDS.PPartner.Rows.Count != 0) && ((!AInspectDS.PPartner[0].HasVersion(DataRowVersion.Original)))))
                        {
                            // Partner is new Partner > add to list of recent partners. (If the
                            // Partner was not new then this was already done in LoadData.)
                            TRecentPartnersHandling.AddRecentlyUsedPartner(FPartnerKey, FPartnerClass, true, TLastPartnerUse.lpuMailroomPartner);
                            TLogging.LogAtLevel(6, "TPartnerEditUIConnector.SubmitChanges: Set Partner as Recent Partner.");
                        }

                        if (TLogging.DebugLevel >= 4)
                        {
                            LogAfterSaving(AInspectDS);
                        }

                        // Must call AcceptChanges so that DataSet.Merge on Client side works
                        // properly if Primary Keys have been changed!
                        AInspectDS.AcceptChanges();
                        DBAccess.GDBAccessObj.CommitTransaction();

                        /* $IFDEF DEBUGMODE if TLogging.DL >= 9 then Console.WriteLine('Location[0] LocationKey: ' + FSubmissionDS.PLocation[0].LocationKey.ToString + '; PartnerLocation[0] LocationKey: ' +
                         *FSubmissionDS.PPartnerLocation[0].LocationKey.ToString);$ENDIF */
                        TLogging.LogAtLevel(8, "TPartnerEditUIConnector.SubmitChanges: Transaction committed!");
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                        TLogging.LogAtLevel(8, "TPartnerEditUIConnector.SubmitChanges: Transaction ROLLED BACK!");
                    }
                }
                catch (Exception e)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();

                    TLogging.Log(e.Message);
                    TLogging.Log(e.StackTrace);

                    throw;
                }
            }
            else
            {
                TLogging.LogAtLevel(8, "TPartnerEditUIConnector.SubmitChanges: AInspectDS = nil!");
                SubmissionResult = TSubmitChangesResult.scrNothingToBeSaved;
            }

            AInspectDS = FSubmissionDS;
            return SubmissionResult;
        }

        private TSubmitChangesResult SubmitChangesAddresses(ref PartnerEditTDS AInspectDS,
            TDBTransaction ASubmitChangesTransaction,
            ref PartnerAddressAggregateTDS AResponseDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult;

            AVerificationResult = null;
//          TLogging.LogAtLevel(7, "TPartnerEditUIConnector.SubmitChangesAddresses: Instance hash is " + this.GetHashCode().ToString());

            if (AInspectDS != null)
            {
                SubmissionResult =
                    TPPartnerAddressAggregate.PrepareChanges(
                        AInspectDS,
                        FPartnerKey,
                        SharedTypes.PartnerClassEnumToString(FPartnerClass),
                        ASubmitChangesTransaction,
                        ref AResponseDS,
                        out AVerificationResult);

                /*
                 * Business Rule: Ensure that the DateModified of the Partner record is
                 * changed to today's date if a new PPartnerLocation is added.
                 */
                if (SubmissionResult == TSubmitChangesResult.scrOK)
                {
                    if (AInspectDS.PPartnerLocation != null)
                    {
                        DataView AddedPartnerLocationsDV = new DataView(AInspectDS.PPartnerLocation, "", "", DataViewRowState.Added);

                        if (AddedPartnerLocationsDV.Count > 0)
                        {
                            // New PPartnerLocation exists
                            TLogging.LogAtLevel(7,
                                "TPartnerEditUIConnector.SubmitChangesAddresses: New PPartnerLocation or changed PPartnerLocation exists.");

                            if (AInspectDS.PPartner != null)
                            {
                                if (AInspectDS.PPartner.Rows.Count > 0)
                                {
                                    // AInspectDS contains a PPartner DataRow > simply update the
                                    // DateModified there (strictly speaking this should not be necessary
                                    // because the DataStore should do that anyway when the PPartner
                                    // record is saved, but just to be sure it happens...)
                                    AInspectDS.PPartner[0].DateModified = DateTime.Today;
//                                  TLogging.LogAtLevel(7,  "TPartnerEditUIConnector.SubmitChangesAddresses: updated PPartner's DateModified to today (PPartner record was present).");
                                }
                            }
                            else
                            {
                                // AInspectDS doesn't contain a PPartner DataRow: load that PPartner
                                // record, change DateModified and save the PPartner record.
                                // must use AddedPartnerLocationsDV because AInspectDS.PPartnerLocation[0] could be a deleted row; see bug 759
                                PPartnerTable PartnerDT = PPartnerAccess.LoadByPrimaryKey(
                                    ((PPartnerLocationRow)(AddedPartnerLocationsDV[0].Row)).PartnerKey,
                                    ASubmitChangesTransaction);
                                PartnerDT[0].DateModified = DateTime.Today;
                                AInspectDS.Merge(PartnerDT);
//                              TLogging.LogAtLevel(7, "TPartnerEditUIConnector.SubmitChangesAddresses: updated PPartner's DateModified to today (PPartner record wasn't present).");
                            }
                        }
                    }
                }

                if (SubmissionResult == TSubmitChangesResult.scrError)
                {
//                  TLogging.LogAtLevel(9, Messages.BuildMessageFromVerificationResult("TPartnerEditUIConnector.SubmitChangesAddresses AVerificationResult: ", AVerificationResult));
                }
            }
            else
            {
                TLogging.LogAtLevel(8, "TPartnerEditUIConnector.SubmitChangesAddresses:AInspectDS = nil!");
                SubmissionResult = TSubmitChangesResult.scrNothingToBeSaved;
            }

            return SubmissionResult;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AResponseDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public TSubmitChangesResult SubmitChangesContinue(out PartnerEditTDS AInspectDS,
            ref DataSet AResponseDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult ReturnValue;

//          TLogging.LogAtLevel(7, this.GetType().FullName + ".SubmitChangesContinue: Instance hash is " + this.GetHashCode().ToString());
//          TLogging.LogAtLevel(8, "AResponseDS.Tables.Count: " + AResponseDS.Tables.Count.ToString());

/*
 *          if (AResponseDS.Tables.Contains(MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME))
 *          {
 *              TLogging.LogAtLevel(7, MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME + " Type: " +
 *                      AResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].GetType().ToString() + "; Rows.Count: " +
 *                      AResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].Rows.Count.ToString());
 *          }
 */
            ReturnValue = SubmitChanges(ref FSubmissionDS, ref AResponseDS, out AVerificationResult);

            if (AResponseDS == null)
            {
                /*
                 * Send AInspectDS back to Client (it may contain changes such as PrimaryKeys
                 * set by a Sequence) if we don't need any more information from the Client
                 * side (the SubmitChanges process is completed then).
                 */
                TLogging.LogAtLevel(8, "TPartnerEditUIConnector.SubmitChangesContinue:  AResponseDS = nil");
                AInspectDS = FSubmissionDS;
            }
            else
            {
                /*
                 * Don't send AInspectDS back to the Client - we are just requesting more
                 * information which is specified in AResponseDS.
                 */
                TLogging.LogAtLevel(8, "TPartnerEditUIConnector.SubmitChangesContinue:  AResponseDS <> nil");
                AInspectDS = null;
            }

            return ReturnValue;
        }

        private bool SubmitChangesOther(ref PartnerEditTDS AInspectDS,
            TDBTransaction ASubmitChangesTransaction,
            out TVerificationResultCollection AVerificationResult)
        {
            TOfficeSpecificDataLabelsUIConnector OfficeSpecificDataLabelsUIConnector;
            PartnerEditTDSFamilyMembersTable FamilyMembersTableSubmit;

            AVerificationResult = null;

//          TLogging.LogAtLevel(7, "TPartnerEditUIConnector.SubmitChanges: Instance hash is " + this.GetHashCode().ToString());
            bool AllSubmissionsOK = true;

            if (AInspectDS != null)
            {
                AVerificationResult = new TVerificationResultCollection();

                #region Partner

                if (AInspectDS.Tables.Contains(PPartnerTable.GetTableName()))
                {
                    SpecialPreSubmitProcessingPartner(AInspectDS.PPartner);

                    if (AInspectDS.PPartner.Rows.Count > 0)
                    {
                        ValidatePPartner(ref AVerificationResult, AInspectDS.PPartner);
                        ValidatePPartnerManual(ref AVerificationResult, AInspectDS.PPartner);

                        if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                        {
                            AllSubmissionsOK = false;
                        }

                        // Business Rule: if the Partner's StatusCode changes, give the user the
                        // option to promote the change to all Family Members (if the Partner is
                        // a FAMILY and has Family Members).
                        if (AInspectDS.PPartner.Rows[0].HasVersion(DataRowVersion.Original)
                            && (AInspectDS.PPartner.Rows != AInspectDS.PPartner.Rows[0][PPartnerTable.GetStatusCodeDBName(), DataRowVersion.Current]))
                        {
//                          TLogging.LogAtLevel(7, this.GetType().FullName + ".SubmitChanges: StatusCode has changed");

                            // StatusCode has changed, now perform FamilyMembers promotion
                            // (the FamilyMembersInfoForStatusChange DataTable won't be nil if
                            // the user indicated so)
                            if (AInspectDS.FamilyMembersInfoForStatusChange != null)
                            {
                                if (!SpecialSubmitProcessingPartnerStatusChange(
                                        AInspectDS,
                                        AInspectDS.PPartner[0].StatusCode,
                                        AInspectDS.FamilyMembersInfoForStatusChange,
                                        ASubmitChangesTransaction))
                                {
                                    AllSubmissionsOK = false;
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Partner Types

                if (AInspectDS.Tables.Contains(PPartnerTypeTable.GetTableName()))
                {
                    if (!SpecialSubmitProcessingPartnerTypes(AInspectDS, ASubmitChangesTransaction))
                    {
                        AllSubmissionsOK = false;
                    }
                }

                #endregion

                #region Partner Details according to PartnerClass

                if (FPartnerClass == TPartnerClass.FAMILY)
                {
                    if (AInspectDS.Tables.Contains(PartnerEditTDSFamilyMembersTable.GetTableName()))
                    {
                        FamilyMembersTableSubmit = AInspectDS.FamilyMembers;
//                      TLogging.LogAtLevel(7, "FamilyMembersTableSubmit.Rows.Count: " + FamilyMembersTableSubmit.Rows.Count.ToString());

                        SpecialSubmitProcessingFamilyMembers(FamilyMembersTableSubmit, ASubmitChangesTransaction);
                    }
                }

                if (FPartnerClass == TPartnerClass.BANK)
                {
                    if (AInspectDS.Tables.Contains(PBankTable.GetTableName()))
                    {
                        if (AInspectDS.PBank.Rows.Count > 0)
                        {
                            ValidatePBank(ref AVerificationResult, AInspectDS.PBank);
                            ValidatePBankManual(ref AVerificationResult, AInspectDS.PBank);

                            if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                            {
                                AllSubmissionsOK = false;
                            }
                        }
                    }
                }

                #endregion

                #region Foundations

                if (AInspectDS.Tables.Contains(PFoundationTable.GetTableName()))
                {
                    DataView DeletedFoundationsDV = new DataView(AInspectDS.PFoundation, "", "", DataViewRowState.Deleted);

                    if (DeletedFoundationsDV.Count > 0)
                    {
                        /*
                         * Special handling in case a foundation got deleted: do a cascading
                         * delete (deletes p_foundation record and also existing records
                         * in p_foundation_deadline, p_foundation_proposal,
                         * p_foundation_proposal_detail)
                         */
                        TLogging.LogAtLevel(7, "TPartnerEditUIConnector.SubmitChanges: performing cascading delete for Foundation!");
                        PFoundationCascading.DeleteByPrimaryKey(FPartnerKey, ASubmitChangesTransaction, true);

                        // Now delete this DataRow to prevent SubmitChanges from trying to
                        // delete it and failing at it (for simplicity we suppose there is only
                        // one DataRow that got deleted)
                        DeletedFoundationsDV[0].Row.Delete();
                        DeletedFoundationsDV[0].Row.AcceptChanges();
                    }
                }

                #endregion

                #region Office Specific Data Labels

                if (AInspectDS.Tables.Contains(PDataLabelValuePartnerTable.GetTableName()))
                {
                    OfficeSpecificDataLabelsUIConnector = new TOfficeSpecificDataLabelsUIConnector(FPartnerKey,
                        MCommonTypes.PartnerClassEnumToOfficeSpecificDataLabelUseEnum(FPartnerClass));

                    OfficeSpecificDataLabelsUIConnector.PrepareChangesServerSide(
                        AInspectDS.PDataLabelValuePartner,
                        ASubmitChangesTransaction);
                }

                #endregion

                #region Partner Interests

                if (AInspectDS.Tables.Contains(PPartnerInterestTable.GetTableName()))
                {
                    foreach (PPartnerInterestRow row in AInspectDS.PPartnerInterest.Rows)
                    {
                        if (row.RowState != DataRowState.Deleted)
                        {
                            if (!row.IsFieldKeyNull())
                            {
                                if (row.FieldKey == 0)
                                {
                                    row.SetFieldKeyNull();
                                }
                            }
                        }
                    }
                }

                #endregion

                // Note: Locations and PartnerLocations are done sepearately in SubmitChangesAddresses!
                if (AllSubmissionsOK == false)
                {
                    TLogging.LogAtLevel(9,
                        Messages.BuildMessageFromVerificationResult("TPartnerEditUIConnector.SubmitChanges AVerificationResult: ",
                            AVerificationResult));
                }
            }
            else
            {
                TLogging.LogAtLevel(8, "TPartnerEditUIConnector.SubmitChanges AInspectDS = nil!");
                AllSubmissionsOK = false;
            }

            if (AVerificationResult.Count > 0)
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
            }

            return AllSubmissionsOK;
        }

        private bool SubmitChangesPersonnelData(ref PartnerEditTDS AInspectDS,
            TDBTransaction ASubmitChangesTransaction,
            out TVerificationResultCollection AVerificationResult)
        {
            TVerificationResultCollection SingleVerificationResultCollection;

            AVerificationResult = null;

//          TLogging.LogAtLevel(7, "TPartnerEditUIConnector.SubmitChangesPersonnelData: Instance hash is " + this.GetHashCode().ToString());
            bool AllSubmissionsOK = true;

            if (AInspectDS != null)
            {
                AVerificationResult = new TVerificationResultCollection();

                #region Individual Data (Personnel Tab)

                IndividualDataTDS TempDS = new IndividualDataTDS();
                TempDS.Merge(AInspectDS);
                TSubmitChangesResult IndividualDataResult;

                // can remove tables PPerson, PDataLabelValuePartner and PDataLabelValueApplication here
                // as this is part of both PartnerEditTDS and IndividualDataTDS and
                // so the relevant data was already saved when PartnerEditTDS was saved
                TempDS.RemoveTable(PPersonTable.GetTableName());
                TempDS.RemoveTable(PDataLabelValuePartnerTable.GetTableName());
                TempDS.RemoveTable(PDataLabelValueApplicationTable.GetTableName());
                TempDS.InitVars();

                IndividualDataResult = TIndividualDataWebConnector.SubmitChangesServerSide(ref TempDS, ref AInspectDS, ASubmitChangesTransaction,
                    out SingleVerificationResultCollection);

                if ((IndividualDataResult != TSubmitChangesResult.scrOK)
                    && (IndividualDataResult != TSubmitChangesResult.scrNothingToBeSaved))
                {
                    AllSubmissionsOK = false;
                    AVerificationResult.AddCollection(SingleVerificationResultCollection);
                }

                #endregion

                // Note: Locations and PartnerLocations are done sepearately in SubmitChangesAddresses!
                if (AllSubmissionsOK == false)
                {
//                  TLogging.LogAtLevel(9, Messages.BuildMessageFromVerificationResult(
//                      "TPartnerEditUIConnector.SubmitChangesPersonnelData AVerificationResult: ", AVerificationResult));
                }
            }
            else
            {
                TLogging.LogAtLevel(8, "TPartnerEditUIConnector.SubmitChangesPersonnelData AInspectDS = null!");
                AllSubmissionsOK = false;
            }

            return AllSubmissionsOK;
        }

        private void SpecialPreSubmitProcessingPartner(PPartnerTable APartnerTableSubmitDT)
        {
            String CurrentPartnerStatus;

            CurrentPartnerStatus = APartnerTableSubmitDT[0].StatusCode;

            // New Partner or Partner Status changed?
            if ((APartnerTableSubmitDT.Rows[0].RowState == DataRowState.Added)
                || (CurrentPartnerStatus != APartnerTableSubmitDT.Rows[0][PPartnerTable.GetStatusCodeDBName(), DataRowVersion.Original].ToString()))
            {
                if (CurrentPartnerStatus == SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscPRIVATE))
                {
                    // Partner Status is PRIVATE > update related fields
                    APartnerTableSubmitDT[0].Restricted = SharedConstants.PARTNER_PRIVATE_USER;
                    APartnerTableSubmitDT[0].UserId = UserInfo.GUserInfo.UserID;
                }
                else
                {
                    // Partner Status is not PRIVATE > update related fields
                    APartnerTableSubmitDT[0].Restricted = 0;
                    APartnerTableSubmitDT[0].UserId = "";
                }

                // Business Rule: StatusChange needs to be set if this is a new Partner,
                // or an existing Partner's PartnerStatus has changed
                APartnerTableSubmitDT[0].StatusChange = DateTime.Today;
            }
        }

        /// <summary>
        /// Performs Partner Status change promotion to Family Members.
        ///
        /// </summary>
        /// <param name="AInspectDS">the dataset that will be saved later to the database</param>
        /// <param name="ANewPartnerStatusCode">The new Partner StatusCode</param>
        /// <param name="APartnerTypeChangeFamilyMembersDT">DataTable holding the PartnerKeys of
        /// Family Members (Note: These could be retrieved from the DB on-the-fly, but
        /// we really want to be sure that the promotion is done to those Family
        /// Members that we presented on the UI)</param>
        /// <param name="ASubmitChangesTransaction">Running transaction in which the DB commands
        /// will be enlisted</param>
        /// <returns>true if processing was successful, otherwise false
        /// </returns>
        private Boolean SpecialSubmitProcessingPartnerStatusChange(
            PartnerEditTDS AInspectDS,
            String ANewPartnerStatusCode,
            PartnerEditTDSFamilyMembersInfoForStatusChangeTable APartnerTypeChangeFamilyMembersDT,
            TDBTransaction ASubmitChangesTransaction)
        {
            PPartnerTable PartnerSaveDT = new PPartnerTable();

//          TLogging.LogAtLevel(7, "TPartnerEditUIConnector.SpecialSubmitProcessingPartnerStatusChange: processing " + APartnerTypeChangeFamilyMembersDT.Rows.Count.ToString() + " Partners...");

            // Loop over all Family Members that were presented to the user
            for (Int16 Counter = 0; Counter <= APartnerTypeChangeFamilyMembersDT.Rows.Count - 1; Counter += 1)
            {
                PartnerEditTDSFamilyMembersInfoForStatusChangeRow PartnerDR = APartnerTypeChangeFamilyMembersDT[Counter];

                // Load Family Member's Partner record
                PPartnerTable PartnerDT = PPartnerAccess.LoadByPrimaryKey(PartnerDR.PartnerKey, ASubmitChangesTransaction);

                if (PartnerDT[0].StatusCode != ANewPartnerStatusCode)
                {
                    // StatusCode of the Partner is different to the new StatusCode > change it
                    PPartnerRow PartnerSaveDR = PartnerSaveDT.NewRowTyped(false);
                    PartnerSaveDR.ItemArray = PartnerDT[0].ItemArray;

                    // Add Partner DataRow to DataTable that contains the Partners that will be saved
                    PartnerSaveDT.Rows.Add(PartnerSaveDR);

                    // Change just added PartnerSaveDR's DataRowState from 'Added' to 'Unchanged'
                    PartnerSaveDR.AcceptChanges();

                    // Apply new StatusCode (DataRowState changes to 'Modified')
                    PartnerSaveDR.StatusCode = ANewPartnerStatusCode;
                    PartnerSaveDR.StatusChange = DateTime.Now.Date;
                }
            }

            AInspectDS.PPartner.Merge(PartnerSaveDT);

            return true;
        }

        /// <summary>
        /// Performs Special Types promotion to Family Members.
        ///
        /// </summary>
        /// <param name="AInspectDS">The Main DataSet of the UIConnector</param>
        /// <param name="ASubmitChangesTransaction">Running transaction in which the DB commands
        /// will be enlisted</param>
        /// <returns>true if processing was successful, otherwise false
        /// </returns>
        private bool SpecialSubmitProcessingPartnerTypes(PartnerEditTDS AInspectDS,
            TDBTransaction ASubmitChangesTransaction)
        {
            bool ReturnValue = true;
            PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable FamilyChangePromotionTable;
            PPartnerTypeTable PPartnerTypeSubmitTable;
            PPartnerTypeRow PPartnerTypeSubmitRow;
            bool PartnerTypeDBExists;
            PPartnerTypeTable PartnerType;

            if (AInspectDS.Tables.Contains(PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable.GetTableName()))
            {
//              TLogging.LogAtLevel(7, "TPartnerEditUIConnector.SpecialSubmitProcessingPartnerTypes: " + PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable.GetTableName() + " passed in.");
                FamilyChangePromotionTable = AInspectDS.PartnerTypeChangeFamilyMembersPromotion;

                if (FamilyChangePromotionTable.Rows.Count > 0)
                {
//                  TLogging.LogAtLevel(7, "SpecialSubmitProcessingPartnerTypes: " + PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable.GetTableName() +
//                            ".Rows.Count: " + FamilyChangePromotionTable.Rows.Count.ToString());
                    PPartnerTypeSubmitTable = new PPartnerTypeTable();

                    for (int Counter = 0; Counter <= FamilyChangePromotionTable.Rows.Count - 1; Counter += 1)
                    {
                        PartnerTypeDBExists =
                            PPartnerTypeAccess.Exists(FamilyChangePromotionTable[Counter].PartnerKey,
                                FamilyChangePromotionTable[Counter].TypeCode, ASubmitChangesTransaction);
//                      TLogging.LogAtLevel(7,  "SpecialSubmitProcessingPartnerTypes: Row[" + Counter.ToString() + "]: DB Exists: " + PartnerTypeDBExists.ToString());

                        if ((!PartnerTypeDBExists
                             && (FamilyChangePromotionTable[Counter].AddTypeCode))
                            || (PartnerTypeDBExists && (FamilyChangePromotionTable[Counter].RemoveTypeCode)))
                        {
                            /*
                             * Action is needed since the PartnerType is either or is either not
                             * in the DB when it should be.
                             */
                            PPartnerTypeSubmitRow = PPartnerTypeSubmitTable.NewRowTyped(false);
                            PPartnerTypeSubmitRow.PartnerKey = FamilyChangePromotionTable[Counter].PartnerKey;
                            PPartnerTypeSubmitRow.TypeCode = FamilyChangePromotionTable[Counter].TypeCode;

                            if (PartnerTypeDBExists)
                            {
                                // get the latest modificationID, otherwise it is emtpy, and that would cause trouble
                                // when deleting the partner type.
                                PartnerType = PPartnerTypeAccess.LoadByPrimaryKey(
                                    FamilyChangePromotionTable[Counter].PartnerKey,
                                    FamilyChangePromotionTable[Counter].TypeCode,
                                    ASubmitChangesTransaction);
                                PPartnerTypeSubmitRow.ModificationId = PartnerType[0].ModificationId;
                            }

                            PPartnerTypeSubmitTable.Rows.Add(PPartnerTypeSubmitRow);

                            if (FamilyChangePromotionTable[Counter].RemoveTypeCode)
                            {
//                              TLogging.LogAtLevel(7, "SpecialSubmitProcessingPartnerTypes: Row[" + Counter.ToString() + "]: Adding to PPartnerTypeSubmitTable - for deletion.");

                                // Mark Type for deletion so that it will be deleted in the DB
                                PPartnerTypeSubmitRow.AcceptChanges();
                                PPartnerTypeSubmitRow.Delete();
                            }
                            else
                            {
//                              TLogging.LogAtLevel(7, "SpecialSubmitProcessingPartnerTypes: Row[" + Counter.ToString() + "]: Adding to PPartnerTypeSubmitTable - for addition.");

                                // Type is added, so it will be added in the DB
                            }
                        }
                    }

                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// </summary>
        /// <param name="AFieldPartnerKey"></param>
        /// <returns></returns>
        public Int64 GetPartnerKeyForNewPartner(System.Int64 AFieldPartnerKey)
        {
            // TODO 2 oChristianK cPartner Edit / Security : Check permissions for creating a new Partner
            return TNewPartnerKey.GetNewPartnerKey(AFieldPartnerKey);
        }

        /// <summary>
        /// Get short name for a partner.
        ///
        /// @Comment Calling this function should soon be no longer necessary since it
        /// can be replaced with a call to Ict.Petra.Client.App.Core.ServerLookups!
        ///
        /// </summary>
        /// <param name="APartnerKey">PartnerKey to find the short name for</param>
        /// <param name="APartnerShortName">short name found for partner key</param>
        /// <param name="APartnerClass">Partner Class for partner that was found</param>
        /// <returns>true if Partner was found in DB, otherwise false
        /// </returns>
        public Boolean GetPartnerShortName(Int64 APartnerKey, out String APartnerShortName, out TPartnerClass APartnerClass)
        {
            TStdPartnerStatusCode PartnerStatus;

            return MCommonMain.RetrievePartnerShortName(APartnerKey, out APartnerShortName, out APartnerClass, out PartnerStatus);
        }

        private PSubscriptionTable GetSubscriptionsInternal(out Int32 ACount, Boolean ACountOnly)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            PSubscriptionTable SubscriptionDT;

            SubscriptionDT = new PSubscriptionTable();
            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                if (ACountOnly)
                {
                    ACount = PSubscriptionAccess.CountViaPPartnerPartnerKey(FPartnerKey, ReadTransaction);
                }
                else
                {
//                  TLogging.LogAtLevel(7,  "TPartnerEditUIConnector.GetSubscriptionsInternal: loading Subscriptions for Partner " + FPartnerKey.ToString() + "...");
                    try
                    {
                        SubscriptionDT = PSubscriptionAccess.LoadViaPPartnerPartnerKey(FPartnerKey, ReadTransaction);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    ACount = SubscriptionDT.Rows.Count;
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetSubscriptionsInternal: committed own transaction.");
                }
            }
            return SubscriptionDT;
        }

        private PSubscriptionTable GetSubscriptionsInternal(out Int32 ACount)
        {
            return GetSubscriptionsInternal(out ACount, false);
        }

        private PartnerEditTDSPPartnerRelationshipTable GetPartnerRelationshipsInternal(out Int32 ACount, Boolean ACountOnly)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            PartnerEditTDSPPartnerRelationshipTable RelationshipDT;
            PPartnerTable PartnerDT;

            RelationshipDT = new PartnerEditTDSPPartnerRelationshipTable();
            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                if (ACountOnly)
                {
                    // count relationships where partner is involved with partner key or reciprocal
                    ACount = PPartnerRelationshipAccess.CountViaPPartnerPartnerKey(FPartnerKey, ReadTransaction) +
                             PPartnerRelationshipAccess.CountViaPPartnerRelationKey(FPartnerKey, ReadTransaction);
                }
                else
                {
//                  TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetRelationshipsInternal: loading Relationships for Partner " + FPartnerKey.ToString() + "...");
                    try
                    {
                        // load relationships where partner is involved with partner key or reciprocal
                        RelationshipDT.Merge(PPartnerRelationshipAccess.LoadViaPPartnerPartnerKey(FPartnerKey, ReadTransaction));
                        RelationshipDT.Merge(PPartnerRelationshipAccess.LoadViaPPartnerRelationKey(FPartnerKey, ReadTransaction));

                        foreach (PartnerEditTDSPPartnerRelationshipRow RelationshipRow in RelationshipDT.Rows)
                        {
                            // find partner name and class depending on relation and add it to data set
                            if (RelationshipRow.PartnerKey == FPartnerKey)
                            {
                                PartnerDT = PPartnerAccess.LoadByPrimaryKey(RelationshipRow.RelationKey, ReadTransaction);
                            }
                            else
                            {
                                PartnerDT = PPartnerAccess.LoadByPrimaryKey(RelationshipRow.PartnerKey, ReadTransaction);
                            }

                            // set extended fields for partner data if record exists
                            if (PartnerDT.Rows[0] != null)
                            {
                                RelationshipRow.PartnerShortName = ((PPartnerRow)PartnerDT.Rows[0]).PartnerShortName;
                                RelationshipRow.PartnerClass = ((PPartnerRow)PartnerDT.Rows[0]).PartnerClass;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    ACount = RelationshipDT.Rows.Count;
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetRelationshipsInternal: committed own transaction.");
                }
            }
            return RelationshipDT;
        }

        private PDataLabelValuePartnerTable GetDataLocalPartnerDataValuesInternal(out Boolean ALabelsAvailable, Boolean ACountOnly)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            TOfficeSpecificDataLabelsUIConnector OfficeSpecificDataLabelsUIConnector;
            PDataLabelValuePartnerTable DataLabelValuePartnerDT;
            OfficeSpecificDataLabelsTDS OfficeSpecificDataLabels;

            ALabelsAvailable = false;

            DataLabelValuePartnerDT = new PDataLabelValuePartnerTable();

            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);


                OfficeSpecificDataLabelsUIConnector = new TOfficeSpecificDataLabelsUIConnector(FPartnerKey,
                    MCommonTypes.PartnerClassEnumToOfficeSpecificDataLabelUseEnum(FPartnerClass));
                OfficeSpecificDataLabels = OfficeSpecificDataLabelsUIConnector.GetData();
                ALabelsAvailable =
                    (OfficeSpecificDataLabelsUIConnector.CountLabelUse(SharedTypes.PartnerClassEnumToString(FPartnerClass), ReadTransaction) != 0);

                if (!ACountOnly)
                {
                    DataLabelValuePartnerDT.Merge(OfficeSpecificDataLabels.PDataLabelValuePartner);
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerEditUIConnector.GetDataLocalPartnerDataValuesInternal: committed own transaction.");
                }
            }

            return DataLabelValuePartnerDT;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ACostCentreCode"></param>
        /// <returns></returns>
        public Boolean HasPartnerCostCentreLink(out String ACostCentreCode)
        {
            return Ict.Petra.Server.MFinance.Common.Common.HasPartnerCostCentreLink(FPartnerKey, out ACostCentreCode);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ACostCentreCode"></param>
        /// <returns></returns>
        public Boolean HasPartnerCostCentreLink(System.Int64 APartnerKey, out String ACostCentreCode)
        {
            return Ict.Petra.Server.MFinance.Common.Common.HasPartnerCostCentreLink(APartnerKey, out ACostCentreCode);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASiteKey"></param>
        /// <param name="ALocationKey"></param>
        /// <returns></returns>
        public Boolean HasPartnerLocationOtherPartnerReferences(Int64 ASiteKey, Int32 ALocationKey)
        {
            Boolean ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

//          TLogging.LogAtLevel(6, "Called HasPartnerLocationOtherPartnerReferences", TLoggingType.ToLogfile);
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            try
            {
                ReturnValue = TPPartnerAddressAggregate.CheckHasPartnerLocationOtherPartnerReferences(FPartnerKey,
                    ASiteKey,
                    ALocationKey,
                    ReadTransaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerEditUIConnector.HasPartnerLocationOtherPartnerReferences: committed own transaction.");
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AFieldPartnerKey"></param>
        /// <param name="AOriginalDefaultKey"></param>
        /// <param name="ANewPartnerKey"></param>
        /// <returns></returns>
        public bool SubmitPartnerKeyForNewPartner(System.Int64 AFieldPartnerKey, System.Int64 AOriginalDefaultKey, ref System.Int64 ANewPartnerKey)
        {
            return TNewPartnerKey.SubmitNewPartnerKey(AFieldPartnerKey, AOriginalDefaultKey, ref ANewPartnerKey);
        }

        private void SpecialSubmitProcessingFamilyMembers(
            PartnerEditTDSFamilyMembersTable AFamilyMembersTable,
            TDBTransaction ASubmitChangesTransaction)
        {
            Int32 DummyCounter = 100;

            /*
             * Load the Persons of a Family
             */
            PPersonTable FamilyPersonsDT = PPersonAccess.LoadViaPFamily(FPartnerEditScreenDS.PFamily[0].PartnerKey, ASubmitChangesTransaction);

            /*
             * Now change the FamilyID of those rows that have been modified on the
             * Client side (first to a dummy value to prevent uniqueness constraint
             * violations)
             */
            for (int Counter = 0; Counter <= AFamilyMembersTable.Rows.Count - 1; Counter += 1)
            {
                PPersonRow ChangePersonRow = (PPersonRow)FamilyPersonsDT.Rows.Find(new Object[] { AFamilyMembersTable[Counter].PartnerKey });
                ChangePersonRow.FamilyId = DummyCounter;
//              TLogging.LogAtLevel(7, "Person " + ChangePersonRow.PartnerKey.ToString() + ": changed OldFamilyID to Dummy " + DummyCounter.ToString());
                DummyCounter = DummyCounter - 1;
            }

            // Save the dummy values
            PPersonAccess.SubmitChanges(FamilyPersonsDT, ASubmitChangesTransaction);

            FamilyPersonsDT.AcceptChanges();

            // Now change it to the real values
            for (int Counter = 0; Counter <= AFamilyMembersTable.Rows.Count - 1; Counter += 1)
            {
                PPersonRow ChangePersonRow = (PPersonRow)FamilyPersonsDT.Rows.Find(new Object[] { AFamilyMembersTable[Counter].PartnerKey });
                ChangePersonRow.FamilyId = AFamilyMembersTable[Counter].FamilyId;
//              TLogging.LogAtLevel(7, "Person " + ChangePersonRow.PartnerKey.ToString() + ": changed OldFamilyID to " + ChangePersonRow.FamilyId.ToString());
            }

            // Save the changes
            PPersonAccess.SubmitChanges(FamilyPersonsDT, ASubmitChangesTransaction);
        }

        #endregion

        #region Data Validation

        static partial void ValidatePPartner(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidatePPartnerManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidatePBank(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidatePBankManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        #endregion Data Validation
    }
}