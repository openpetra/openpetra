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
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MPartner;
using Ict.Petra.Server.MPartner.DataAggregates;

namespace Ict.Petra.Server.MPartner.Common
{
    /// <summary>
    /// Contains Methods that looks up miscellaneous Partner data.
    /// Used by TPartnerServerLookups.PartnerInfo Method.
    /// </summary>
    public static class TServerLookups_PartnerInfo
    {
        /// <summary>
        /// Retrieves Location and PartnerLocation information and the rest of the PartnerInfo data,
        /// but not the 'Head' data.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the PartnerInfo data for</param>
        /// <param name="ALocationKey" >Location Key of the Location that the information should be
        /// retrieved for.</param>
        /// <param name="APartnerInfoDS">Typed DataSet that contains the requested data.</param>
        /// <returns>True if Partner exists, otherwise false.</returns>
        public static bool LocationPartnerLocationAndRestOnly(Int64 APartnerKey, TLocationPK ALocationKey,
            ref PartnerInfoTDS APartnerInfoDS)
        {
            return LocationPartnerLocationInternal(APartnerKey, ALocationKey, ref APartnerInfoDS, true);
        }

        /// <summary>
        /// Retrieves Location and PartnerLocation information and the rest of the PartnerInfo data,
        /// but not the 'Head' data.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the PartnerInfo data for</param>
        /// <param name="ALocationKey" >Location Key of the Location that the information should be
        /// retrieved for.</param>
        /// <param name="APartnerInfoDS">Typed DataSet that contains the requested data.</param>
        /// <returns>True if Partner exists, otherwise false.</returns>
        public static bool LocationPartnerLocationOnly(Int64 APartnerKey, TLocationPK ALocationKey,
            ref PartnerInfoTDS APartnerInfoDS)
        {
            return LocationPartnerLocationInternal(APartnerKey, ALocationKey, ref APartnerInfoDS, false);
        }

        /// <summary>
        /// Retrieves PartnerLocation information and the rest of the PartnerInfo data, but not the
        /// 'Head' data.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the PartnerInfo data for</param>
        /// <param name="ALocationKey" >Location Key of the Location that the information should be
        /// retrieved for.</param>
        /// <param name="APartnerInfoDS">Typed DataSet that contains the requested data.</param>
        /// <returns>True if Partner exists, otherwise false.</returns>
        public static bool PartnerLocationAndRestOnly(Int64 APartnerKey, TLocationPK ALocationKey,
            ref PartnerInfoTDS APartnerInfoDS)
        {
            return PartnerLocationInternal(APartnerKey, ALocationKey, ref APartnerInfoDS, true);
        }

        /// <summary>
        /// Retrieves PartnerLocation information and the rest of the PartnerInfo data, but not the
        /// 'Head' data.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the PartnerInfo data for</param>
        /// <param name="ALocationKey" >Location Key of the Location that the information should be
        /// retrieved for.</param>
        /// <param name="APartnerInfoDS">Typed DataSet that contains the requested data.</param>
        /// <returns>True if Partner exists, otherwise false.</returns>
        public static bool PartnerLocationOnly(Int64 APartnerKey, TLocationPK ALocationKey,
            ref PartnerInfoTDS APartnerInfoDS)
        {
            return PartnerLocationInternal(APartnerKey, ALocationKey, ref APartnerInfoDS, false);
        }

        /// <summary>
        /// Retrieves all of the PartnerInfo data.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the PartnerInfo data for</param>
        /// <param name="APartnerInfoDS">Typed DataSet that contains the requested data.</param>
        /// <returns>True if Partner exists, otherwise false.</returns>
        public static bool AllPartnerInfoData(Int64 APartnerKey, ref PartnerInfoTDS APartnerInfoDS)
        {
            bool ReturnValue = false;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            PPartnerRow PartnerDR;

            TLocationPK BestLocationPK;
            PLocationRow LocationDR;
            PLocationRow LocationDR2;
            PPartnerLocationRow PartnerLocationDR;
            PPartnerLocationRow PartnerLocationDR2;
            PLocationTable LocationDT;
            PPartnerLocationTable PartnerLocationDT;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                /*
                 * Check for existance of Partner
                 */
                PartnerDR = MCommonMain.CheckPartnerExists2(APartnerKey, true);

                if (PartnerDR != null)
                {
                    /*
                     * Perform security checks; these throw ESecurityPartnerAccessDeniedException
                     * in case access isn't granted.
                     */
                    TSecurity.CanAccessPartnerExc(PartnerDR);

                    /*
                     * Get the Partner's Address data of its 'Best' Address
                     */
                    if (TMailing.GetPartnersBestLocationData(APartnerKey, out BestLocationPK,
                            out LocationDR, out PartnerLocationDR))
                    {
                        #region Process Address

                        /*
                         * Check for existance of PLocation and PPartnerLocation Tables in the passed in
                         * DataSet APartnerInfoDS.
                         */
                        if (!APartnerInfoDS.Tables.Contains(PLocationTable.GetTableName()))
                        {
                            // Need to create Table here
                            APartnerInfoDS.Tables.Add(new PLocationTable());
                        }

                        if (!APartnerInfoDS.Tables.Contains(PPartnerLocationTable.GetTableName()))
                        {
                            // Need to create Table here
                            APartnerInfoDS.Tables.Add(new PPartnerLocationTable());
                        }

                        // Add copies of the Location and PartnerLocation DataRows of the 'Best Address'

                        /*
                         * Remove DataColumns that might have been added by the call to
                         * TMailing.GetPartnersBestLocationData - otherwise PartnerLocationDR2.ItemArray
                         * assignment will fail.
                         */
                        if (PartnerLocationDR.Table.Columns.Contains(Calculations.PARTNERLOCATION_BESTADDR_COLUMN))
                        {
                            PartnerLocationDR.Table.Columns.Remove(Calculations.PARTNERLOCATION_BESTADDR_COLUMN);
                        }

                        if (PartnerLocationDR.Table.Columns.Contains(Calculations.PARTNERLOCATION_ICON_COLUMN))
                        {
                            PartnerLocationDR.Table.Columns.Remove(Calculations.PARTNERLOCATION_ICON_COLUMN);
                        }

                        LocationDR2 = APartnerInfoDS.PLocation.NewRowTyped(false);
                        LocationDR2.ItemArray = LocationDR.ItemArray;
                        PartnerLocationDR2 = APartnerInfoDS.PPartnerLocation.NewRowTyped(false);
                        PartnerLocationDR2.ItemArray = PartnerLocationDR.ItemArray;

                        APartnerInfoDS.PLocation.Rows.Add(LocationDR2);
                        APartnerInfoDS.PPartnerLocation.Rows.Add(PartnerLocationDR2);

                        #endregion

                        // Apply Address Security
                        LocationDT = APartnerInfoDS.PLocation;
                        PartnerLocationDT = APartnerInfoDS.PPartnerLocation;
                        TPPartnerAddressAggregate.ApplySecurity(ref PartnerLocationDT,
                            ref LocationDT);

                        // Process 'Head' data and rest of data for the Partner
                        HeadInternal(PartnerDR, ref APartnerInfoDS);
                        RestInternal(PartnerDR, ReadTransaction, ref APartnerInfoDS);

                        ReturnValue = true;
                    }
                }
            }
            catch (ESecurityPartnerAccessDeniedException)
            {
                // don't log this exception - this is thrown on purpose here and the Client needs to deal with it.
                throw;
            }
            catch (Exception Exp)
            {
                TLogging.Log("TServerLookups_PartnerInfo.AllPartnerInfoData exception: " + Exp.ToString(), TLoggingType.ToLogfile);
                TLogging.Log(Exp.StackTrace, TLoggingType.ToLogfile);
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TServerLookups_PartnerInfo.AllPartnerInfoData: committed own transaction.");
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Retrieves Location and PartnerLocation information and the rest of the PartnerInfo data,
        /// but not the 'Head' data.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
        /// <param name="ALocationKey">Location Key of the Location that the information should be
        /// retrieved for.</param>
        /// <param name="APartnerInfoDS">Typed DataSet that contains the requested data.</param>
        /// <param name="AIncludeRest">Include 'Rest' data as well</param>
        private static bool LocationPartnerLocationInternal(Int64 APartnerKey, TLocationPK ALocationKey,
            ref PartnerInfoTDS APartnerInfoDS,
            bool AIncludeRest)
        {
            bool ReturnValue = false;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            PPartnerRow PartnerDR;
            PLocationTable LocationDT;
            PPartnerLocationTable PartnerLocationDT;


            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                /*
                 * Check for existance of Partner
                 */
                PartnerDR = MCommonMain.CheckPartnerExists2(APartnerKey, true);

                if (PartnerDR != null)
                {
                    /*
                     * Perform security checks; these throw ESecurityPartnerAccessDeniedException
                     * in case access isn't granted.
                     */
                    TSecurity.CanAccessPartnerExc(PartnerDR);

                    /*
                     * Load Partner Location data and rest of data first
                     */
                    PartnerLocationInternal(APartnerKey, ALocationKey, ReadTransaction, ref APartnerInfoDS);

                    /*
                     * Load Location Information; this gets merged into the already retrieved
                     * information in APartnerInfoDS (eg. Partner Location data and rest of data)
                     */
                    APartnerInfoDS.Merge(TPPartnerAddressAggregate.LoadByPrimaryKey(
                            ALocationKey.SiteKey, ALocationKey.LocationKey, ReadTransaction));

                    // Apply Address Security
                    LocationDT = APartnerInfoDS.PLocation;
                    PartnerLocationDT = APartnerInfoDS.PPartnerLocation;
                    TPPartnerAddressAggregate.ApplySecurity(ref PartnerLocationDT,
                        ref LocationDT);

                    if (AIncludeRest)
                    {
                        RestInternal(PartnerDR, ReadTransaction, ref APartnerInfoDS);
                    }

                    ReturnValue = true;
                }
            }
            catch (ESecurityPartnerAccessDeniedException)
            {
                // don't log this exception  this is thrown on purpose here and the Client needs to deal with it.
                throw;
            }
            catch (Exception Exp)
            {
                TLogging.Log("TServerLookups_PartnerInfo.PartnerLocationInternal exception: " + Exp.ToString(), TLoggingType.ToLogfile);
                TLogging.Log(Exp.StackTrace, TLoggingType.ToLogfile);
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TServerLookups_PartnerInfo.LocationPartnerLocationAndRestOnly: committed own transaction.");
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Retrieves PartnerLocation information and the rest of the PartnerInfo data, but not the
        /// 'Head' data.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
        /// <param name="ALocationKey" >Location Key of the Location that the information should be
        /// retrieved for.</param>
        /// <param name="APartnerInfoDS">Typed DataSet that contains the requested data.</param>
        /// <param name="AIncludeRest">Include 'Rest' data as well</param>
        private static bool PartnerLocationInternal(Int64 APartnerKey, TLocationPK ALocationKey,
            ref PartnerInfoTDS APartnerInfoDS,
            bool AIncludeRest)
        {
            bool ReturnValue = false;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            PPartnerRow PartnerDR;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                /*
                 * Check for existance of Partner
                 */
                PartnerDR = MCommonMain.CheckPartnerExists2(APartnerKey, true);

                if (PartnerDR != null)
                {
                    /*
                     * Perform security checks; these throw ESecurityPartnerAccessDeniedException
                     * in case access isn't granted.
                     */
                    TSecurity.CanAccessPartnerExc(PartnerDR);

                    /*
                     * Partner exists --> we can go ahead with data gathering
                     */
                    PartnerLocationInternal(APartnerKey, ALocationKey, ReadTransaction, ref APartnerInfoDS);

                    if (AIncludeRest)
                    {
                        RestInternal(PartnerDR, ReadTransaction, ref APartnerInfoDS);
                    }

                    ReturnValue = true;
                }
            }
            catch (ESecurityPartnerAccessDeniedException)
            {
                // don't log this exception - this is thrown on purpose here and the Client needs to deal with it.
                throw;
            }
            catch (Exception Exp)
            {
                TLogging.Log("TServerLookups_PartnerInfo.PartnerLocationInternal exception: " + Exp.ToString(), TLoggingType.ToLogfile);
                TLogging.Log(Exp.StackTrace, TLoggingType.ToLogfile);
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TServerLookups_PartnerInfo.LocationPartnerLocationAndRestOnly: committed own transaction.");
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Retrieves PartnerLocation information.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
        /// <param name="ALocationKey" >Location Key of the Location that the information should be
        /// retrieved for.</param>
        /// <param name="AReadTransaction">Open Database Transaction.</param>
        /// <param name="APartnerInfoDS">Typed DataSet that contains the requested data.</param>
        private static void PartnerLocationInternal(Int64 APartnerKey, TLocationPK ALocationKey,
            TDBTransaction AReadTransaction,
            ref PartnerInfoTDS APartnerInfoDS)
        {
            /*
             * Load PartnerLocation Information; this gets merged into the already retrieved
             * information in APartnerInfoDS (eg. 'Head' data)
             */
            APartnerInfoDS.Merge(TPPartnerAddressAggregate.LoadByPrimaryKey(
                    APartnerKey, ALocationKey.SiteKey, ALocationKey.LocationKey, AReadTransaction));
        }

        /// <summary>
        /// Retrieves 'Rest' of Partner Information data.
        /// </summary>
        /// <param name="APartnerDR">DataRow that contains the Partner data.</param>
        /// <param name="AReadTransaction" >Open DB Transaction.</param>
        /// <param name="APartnerInfoDS" >Typed PartnerInfoTDS DataSet</param>
        private static void RestInternal(PPartnerRow APartnerDR,
            TDBTransaction AReadTransaction,
            ref PartnerInfoTDS APartnerInfoDS)
        {
            PartnerInfoTDSPartnerAdditionalInfoRow PartnerInfoDR;
            TPartnerClass PartnerClass;
            DateTime LastContactDate;
            PPersonTable PersonDT;
            PPartnerTable FamilyPartnerDT;
            Int64 PartnerKey = APartnerDR.PartnerKey;

            /*
             * Load Special Types
             */
            PPartnerTypeAccess.LoadViaPPartner(APartnerInfoDS, PartnerKey, AReadTransaction);


            /*
             * Load Subscriptions
             */
            PSubscriptionAccess.LoadViaPPartnerPartnerKey(APartnerInfoDS, PartnerKey, AReadTransaction);


            #region Populate PartnerAdditionalInfo Table

            if (APartnerInfoDS.PartnerAdditionalInfo.Rows.Count == 0)
            {
                PartnerInfoDR = APartnerInfoDS.PartnerAdditionalInfo.NewRowTyped(false);
            }
            else
            {
                PartnerInfoDR = APartnerInfoDS.PartnerAdditionalInfo[0];
            }

            if (!APartnerDR.IsCommentNull())
            {
                PartnerInfoDR.Notes = APartnerDR.Comment.Trim();
            }

            if (!APartnerDR.IsDateCreatedNull())
            {
                PartnerInfoDR.DateCreated = APartnerDR.DateCreated;
            }

            if (!APartnerDR.IsDateModifiedNull())
            {
                PartnerInfoDR.DateModified = APartnerDR.DateModified;
            }

            if (!APartnerDR.IsLanguageCodeNull())
            {
                PartnerInfoDR.MainLanguages = APartnerDR.LanguageCode;
            }

            if (!APartnerDR.IsPreviousNameNull())
            {
                PartnerInfoDR.PreviousName = APartnerDR.PreviousName;
            }

            // Determination of Last Contact Date
            TMailroom.GetLastContactDate(PartnerKey, out LastContactDate);
            PartnerInfoDR.LastContact = LastContactDate;


            /*
             * Special Data according to Partner Class
             */
            PartnerClass = SharedTypes.PartnerClassStringToEnum(APartnerDR.PartnerClass);

            switch (PartnerClass)
            {
                case TPartnerClass.PERSON:
                    PersonDT = PPersonAccess.LoadByPrimaryKey(APartnerDR.PartnerKey, AReadTransaction);

                    if (PersonDT != null)
                    {
                        if (!PersonDT[0].IsDateOfBirthNull())
                        {
                            PartnerInfoDR.DateOfBirth = PersonDT[0].DateOfBirth;
                        }

                        // Get Family Members info
                        APartnerInfoDS.Merge(GetFamilyMembers(PersonDT[0].FamilyKey, AReadTransaction));


                        // Get Family Partner info
                        FamilyPartnerDT = PPartnerAccess.LoadByPrimaryKey(PersonDT[0].FamilyKey, AReadTransaction);

                        if (FamilyPartnerDT != null)
                        {
                            PartnerInfoDR.Family = FamilyPartnerDT[0].PartnerShortName;
                            PartnerInfoDR.FamilyKey = FamilyPartnerDT[0].PartnerKey;
                        }

                        // Get the Languages of a Person from Personnel
                        PartnerInfoDR.AdditionalLanguages = GetPersonLanguagesFromPersonnel(
                            APartnerDR.PartnerKey, PartnerInfoDR.MainLanguages, AReadTransaction);
                    }

                    break;

                case TPartnerClass.FAMILY:

                    // Get Family Members info
                    APartnerInfoDS.Merge(GetFamilyMembers(PartnerKey, AReadTransaction));
                    break;

                case TPartnerClass.UNIT:

                    // Get Unit structure info
                    APartnerInfoDS.Merge(GetUnitStructure(PartnerKey, AReadTransaction));

                    break;
            }

            if (APartnerInfoDS.PartnerAdditionalInfo.Rows.Count == 0)
            {
                APartnerInfoDS.PartnerAdditionalInfo.Rows.Add(PartnerInfoDR);
            }

            #endregion
        }

        /// <summary>
        /// Puts 'Head' data from a Partner DataRow into Partner Information data.
        /// </summary>
        /// <param name="APartnerDR">DataRow that contains the Partner data.</param>
        /// <param name="APartnerInfoDS" >Typed PartnerInfoTDS DataSet</param>
        private static void HeadInternal(PPartnerRow APartnerDR,
            ref PartnerInfoTDS APartnerInfoDS)
        {
            PartnerInfoTDSPartnerHeadInfoRow PartnerHeadInfoDR;

            if (APartnerInfoDS.PartnerHeadInfo.Rows.Count == 0)
            {
                PartnerHeadInfoDR = APartnerInfoDS.PartnerHeadInfo.NewRowTyped(false);
            }
            else
            {
                PartnerHeadInfoDR = APartnerInfoDS.PartnerHeadInfo[0];
            }

            PartnerHeadInfoDR.PartnerKey = APartnerDR.PartnerKey;
            PartnerHeadInfoDR.PartnerShortName = APartnerDR.PartnerShortName;
            PartnerHeadInfoDR.PartnerClass = APartnerDR.PartnerClass;
            PartnerHeadInfoDR.StatusCode = APartnerDR.StatusCode;
            PartnerHeadInfoDR.AcquisitionCode = APartnerDR.AcquisitionCode;
            PartnerHeadInfoDR.PrivatePartnerOwner = APartnerDR.UserId;

            if (APartnerInfoDS.PartnerHeadInfo.Rows.Count == 0)
            {
                APartnerInfoDS.PartnerHeadInfo.Rows.Add(PartnerHeadInfoDR);
            }
        }

        /// <summary>
        /// Returns the Languages of a Person from Personnel. It excludes the MainLanguage.
        /// </summary>
        /// <remarks>The Language Codes are sorted by Language Level so that the Language
        /// with the best level comes first, then the other Languages in descending Language
        /// Level order.</remarks>
        /// <param name="APartnerKey">PartnerKey of the PERSON.</param>
        /// <param name="AMainLanguage">MainLanguage of the PERSON.</param>
        /// <param name="AReadTransaction">Open DB Transaction.</param>
        /// <returns>A String containing all the Language Codes from Personnel, excluding the
        /// MainLanguage. The Language Codes are separated with a '|' (pipe) character.</returns>
        private static String GetPersonLanguagesFromPersonnel(Int64 APartnerKey, string AMainLanguage, TDBTransaction AReadTransaction)
        {
            const string LANG_SEPARATOR = "|";
            String AdditionalLanguages = "";

            PmPersonLanguageTable PersonLangDT = PmPersonLanguageAccess.LoadViaPPerson(APartnerKey, AReadTransaction);

            if (PersonLangDT != null)
            {
                // We want the list of Languages sorted by Language Level so that the Language with the
                // best level comes first, then the other Languages in descending Language Level order.
                DataView PersonLangDV = new DataView(PersonLangDT, "",
                    PmPersonLanguageTable.GetLanguageLevelDBName() + " DESC", DataViewRowState.CurrentRows);

                for (int Counter = 0; Counter < PersonLangDV.Count; Counter++)
                {
                    PmPersonLanguageRow PersonLangDR = (PmPersonLanguageRow)PersonLangDV[Counter].Row;

                    if (PersonLangDR.LanguageCode != AMainLanguage)
                    {
                        AdditionalLanguages = AdditionalLanguages + PersonLangDR.LanguageCode + LANG_SEPARATOR;
                    }
                }

                if (AdditionalLanguages.Length > 0)
                {
                    AdditionalLanguages = AdditionalLanguages.Substring(0, AdditionalLanguages.Length - LANG_SEPARATOR.Length);
                }
            }

            return AdditionalLanguages;
        }

        /// <summary>
        /// Returns the Unit Structure Info for a Partner of Partner Class UNIT.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the UNIT.</param>
        /// <param name="AReadTransaction">Open DB Transaction.</param>
        /// <returns>An Instance of <see cref="PartnerInfoTDSUnitInfoTable" />.
        /// Contains one DataRow if Unit Structure information could be retrieved, otherwise no DataRow.</returns>
        private static PartnerInfoTDSUnitInfoTable GetUnitStructure(Int64 APartnerKey, TDBTransaction AReadTransaction)
        {
            PartnerInfoTDSUnitInfoTable UnitInfoDT = new PartnerInfoTDSUnitInfoTable();

            UmUnitStructureTable UnitStructureDT = UmUnitStructureAccess.LoadViaPUnitChildUnitKey(APartnerKey, AReadTransaction);

            if (UnitStructureDT.Rows.Count > 0)
            {
                PUnitTable UnitDT = PUnitAccess.LoadByPrimaryKey(UnitStructureDT[0].ParentUnitKey, AReadTransaction);

                if (UnitDT != null)
                {
                    PartnerInfoTDSUnitInfoRow UnitInfoDR = UnitInfoDT.NewRowTyped(false);
                    UnitInfoDR.ParentUnitKey = UnitDT[0].PartnerKey;
                    UnitInfoDR.ParentUnitName = UnitDT[0].UnitName;

                    UnitInfoDT.Rows.Add(UnitInfoDR);
                }
            }

            return UnitInfoDT;
        }

        /// <summary>
        /// Returns the Family Members of a Family.
        /// </summary>
        /// <param name="AFamilyPartnerKey">PartnerKey of the FAMILY.</param>
        /// <param name="AReadTransaction">Open DB Transaction.</param>
        /// <returns>An Instance of <see cref="PartnerInfoTDSFamilyMembersTable" />.
        /// If there were Family Members, there will be one DataRow for each Family Member.</returns>
        private static PartnerInfoTDSFamilyMembersTable GetFamilyMembers(Int64 AFamilyPartnerKey,
            TDBTransaction AReadTransaction)
        {
            OdbcParameter[] ParametersArray;
            DataSet TmpDS;
            PPersonTable FamilyPersonsDT;
            PartnerInfoTDSFamilyMembersRow NewRow;
            PartnerInfoTDSFamilyMembersTable FamilyMembersDT;

            FamilyMembersDT = new PartnerInfoTDSFamilyMembersTable();

            ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = (System.Object)AFamilyPartnerKey;

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
                PPersonTable.GetFamilyIdDBName() + ' ' +
                "FROM PUB_" + PPersonTable.GetTableDBName() +
                " INNER JOIN " + "PUB_" + PPartnerTable.GetTableDBName() + " ON " +
                "PUB_" + PPersonTable.GetTableDBName() + '.' +
                PPartnerTable.GetPartnerKeyDBName() + " = " +
                "PUB_" + PPartnerTable.GetTableDBName() + '.' +
                PPartnerTable.GetPartnerKeyDBName() + ' ' +
                "WHERE " + PPersonTable.GetFamilyKeyDBName() + " = ? " +
                "AND " + PPartnerTable.GetStatusCodeDBName() + " <> " + '"' +
                SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscMERGED) + "\" " +                            // Make sure we don't load MERGED Partners (shouldn't have a p_family_key_n, but just in case.)
                "ORDER BY " + PPersonTable.GetFamilyIdDBName() + " ASC",
                PPersonTable.GetTableName(), AReadTransaction, ParametersArray, 0, 0);

            // Add Persons to Table
            for (Int32 Counter = 0; Counter <= FamilyPersonsDT.Rows.Count - 1; Counter += 1)
            {
                NewRow = FamilyMembersDT.NewRowTyped(false);
                NewRow.PartnerKey = FamilyPersonsDT[Counter].PartnerKey;
                NewRow.PartnerShortName =
                    Calculations.DeterminePartnerShortName(TSaveConvert.StringColumnToString(FamilyPersonsDT.ColumnFamilyName,
                            FamilyPersonsDT[Counter]), TSaveConvert.StringColumnToString(FamilyPersonsDT.ColumnTitle,
                            FamilyPersonsDT[Counter]), TSaveConvert.StringColumnToString(FamilyPersonsDT.ColumnFirstName,
                            FamilyPersonsDT[Counter]),
                        TSaveConvert.StringColumnToString(FamilyPersonsDT.ColumnMiddleName1, FamilyPersonsDT[Counter]));
                NewRow.FamilyId = FamilyPersonsDT[Counter].FamilyId;

                FamilyMembersDT.Rows.Add(NewRow);
            }

            return FamilyMembersDT;
        }
    }
}