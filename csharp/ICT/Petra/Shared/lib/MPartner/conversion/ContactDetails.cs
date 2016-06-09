//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK, timop, PeterS
//
// Copyright 2004-2015 by OM International
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
using System.IO;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.Verification;

namespace Ict.Petra.Shared.MPartner.Conversion
{
    /// <summary>
    /// This class generates Partner Contact Detail records (=p_partner_attribute table records)
    /// out of Petra 2.x p_partner_location records
    /// </summary>
    public class TPartnerContactDetails
    {
        #region Fields

        /// <summary>Copied from Ict.Petra.Shared.MPartner.Calculations!</summary>
        private const String PARTNERLOCATION_BESTADDR_COLUMN = "BestAddress";

        /// <summary>Copied from Ict.Petra.Shared.MPartner.Calculations!</summary>
        private const String PARTNERLOCATION_ICON_COLUMN = "Icon";

        /// <summary>Copied from Ict.Petra.Shared.SharedConstants!</summary>
        private const String SECURITY_CAN_LOCATIONTYPE = "-CAN";

        /// <summary>As specified in the 'Base Data'.</summary>
        private const String ATTR_TYPE_PHONE = "Phone";

        /// <summary>As specified in the 'Base Data'.</summary>
        private const String ATTR_TYPE_FAX = "Fax";

        /// <summary>As specified in the 'Base Data'.</summary>
        private const String ATTR_TYPE_MOBILE_PHONE = "Mobile Phone";

        /// <summary>As specified in the 'Base Data'.</summary>
        private const String ATTR_TYPE_EMAIL = "E-Mail";

        /// <summary>As specified in the 'Base Data'.</summary>
        private const String ATTR_TYPE_WEBSITE = "Web Site";

        /// <summary>As specified in the 'Base Data'.</summary>
        private const String ATTR_TYPE_SKYPE = "Skype";

        /// <summary>Possible prefix that denotes that a Skype ID is to follow.</summary>
        private const String PREFIX_SKYPE1 = "Skype:";

        /// <summary>Possible prefix that denotes that a Skype ID is to follow.</summary>
        private const String PREFIX_SKYPE2 = "Skype";

        private const String CONT_DETAIL_MIGRATION_LOG_PREFIX = "Contact Details ";

        private const String LOG_PREF_INFO = "[INFO";

        private const String LOG_PREF_ADVICE = "[ADVICE";

        private const String LOG_PREF_WARN = "[WARNING";

        private const String LOG_PREF_CHECK = "[CHECK";

        /// <summary>
        /// Holds the p_partner.p_partner_class_c information of each Partner.
        /// </summary>
        private static Dictionary <Int64, string>FPartnerClassInformation = new Dictionary <Int64, string>();

        /// <summary>
        /// Holds the p_partner_location records of each Partner.
        /// </summary>
        private static SortedList <long, DataTable>FPartnerLocationRecords = null;

        #region Variables for gathering PartnerKey information for various Warnings

        private static SortedSet <string>FWarnings100Collector = new SortedSet <string>();
        private static SortedSet <string>FWarnings110Collector = new SortedSet <string>();
        private static SortedSet <string>FWarnings310CollectorFieldCurrentPartnerKeys = new SortedSet <string>();
        private static SortedSet <string>FWarnings310CollectorFieldNonCurrentPartnerKeys = new SortedSet <string>();
        private static SortedSet <string>FFWarnings310CollectorNonFieldCurrentPartnerKeys = new SortedSet <string>();
        private static SortedSet <string>FFWarnings310CollectorNonFieldNonCurrentPartnerKeys = new SortedSet <string>();
        private static SortedSet <string>FWarnings400Collector = new SortedSet <string>();

        #endregion

        /// <summary>
        /// we split all partner locations by modulo of the partner key, to improve the speed of the search
        /// </summary>
        public const int NumberOfTables = 20;

        /// <summary>
        /// Number for the p_sequence_i Column. Gets increased with every p_partner_attribute record that gets produced!
        /// </summary>
        private static int FSequenceNo = 0;

        /// <summary>
        /// The order of the p_partner_location record (according to how they are sorted) that the p_partner_attribute
        /// got genereated from.
        /// </summary>
        private static int FInsertionOrderPerPartner = 0;

        private static string FEmptyStringIndicator = null;

        private static string FSiteCountryCode = null;

        private static string FSiteInternatAccessCode = null;

        private static DataTable FCountryTable = null;

        private static DataSet FPartnerAttributeHoldingDataSet;

        #endregion

        #region Properties

        /// <summary>
        /// Holds the p_partner.p_partner_class_c information of each Partner.
        /// </summary>
        public static Dictionary <Int64, string>PartnerClassInformation
        {
            get
            {
                return FPartnerClassInformation;
            }
        }

        /// <summary>
        /// Holds the p_partner_location records of each Partner.
        /// </summary>
        public static SortedList <long, DataTable>PartnerLocationRecords
        {
            get
            {
                return FPartnerLocationRecords;
            }

            set
            {
                FPartnerLocationRecords = value;
            }
        }

        /// <summary>
        /// String that denotes an empty strings (not necessarily <see cref="String.Empty" />!)
        /// </summary>
        public static string EmptyStringIndicator
        {
            get
            {
                return FEmptyStringIndicator;
            }

            set
            {
                FEmptyStringIndicator = value;
            }
        }

        /// <summary>
        /// Country Code the site country
        /// </summary>
        public static string SiteCountryCode
        {
            set
            {
                FSiteCountryCode = value;
            }
        }

        /// <summary>
        /// International Access (Exit) Code for dialling out of the site country
        /// </summary>
        public static string SiteInternatAccessCode
        {
            set
            {
                FSiteInternatAccessCode = value;
            }
        }

        /// <summary>
        /// DataTable containing all data in a_country
        /// </summary>
        public static DataTable CountryTable
        {
            set
            {
                FCountryTable = value;
            }
        }

        /// <summary>
        /// DataSet in which newly created Partner Attributes should be added to.
        /// </summary>
        /// <remarks>May be null, depending on what the Method that is set for the
        /// <see cref="CreateContactDetailsRow" /> Action expects.</remarks>
        public static DataSet PartnerAttributeHoldingDataSet
        {
            get
            {
                return FPartnerAttributeHoldingDataSet;
            }

            set
            {
                FPartnerAttributeHoldingDataSet = value;
            }
        }

        #endregion

        /// <summary>
        /// Action Delegate for the serialisation of a newly created Contact Details Row.
        /// </summary>
        public static Action <TPartnerContactDetails.PPartnerAttributeRecord,
                              DataSet>CreateContactDetailsRow;

        #region Inner Classes

        /// <summary>
        /// Helper Class for 'Best Address' handling.
        /// </summary>
        /// <remarks>Utilises .NET Reflection for the loading of an OpenPetra DLL that we can't make a
        /// direct Reference to in the C# Project as the DLL gets build later!</remarks>
        public static class BestAddressHelper
        {
            private static object FInstantiator = null;
            private static Type FRemoteClass;

            /// <summary>Static Constructor.</summary>
            static BestAddressHelper()
            {
                const string AssemblyDLLName = "Ict.Petra.Shared.lib.MPartner";
                const string RemoteType = "Ict.Petra.Shared.MPartner.Calculations";
                Assembly LoadedAssembly = null;

                LoadedAssembly = Assembly.Load(AssemblyDLLName);

                FRemoteClass = LoadedAssembly.GetType(RemoteType);

                if (FRemoteClass == null)
                {
                    const string msg = "cannot find type " + RemoteType + " in " + AssemblyDLLName;
                    TLogging.Log(msg);
                    throw new Exception(msg);
                }

                FInstantiator = Activator.CreateInstance(FRemoteClass,
                    (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                    null,
                    null,
                    null);
            }

            /// <summary>
            /// Creates an instance of a 'cut-down' PPartnerLocation Table that will be used for storing partial
            /// to-be-imported p_partner_location record information. It will also be used for the 'Best Address'
            /// determination, which is important for the Contact Details migration.
            /// </summary>
            /// <returns>Instance of a 'cut-down' PPartnerLocation Table that will be used for 'Best Address' determination.</returns>
            public static DataTable GetNewPPartnerLocationTableInstance()
            {
                DataTable ReturnValue = new DataTable("Partners_PPartnerLocation");
                DataColumn IconForSortingCol;

                ReturnValue.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_site_key_n", typeof(Int64)));
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_location_key_i", typeof(Int32)));
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_date_effective_d", typeof(System.DateTime)));
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_date_good_until_d", typeof(System.DateTime)));
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_location_type_c", typeof(string)));
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_send_mail_l", typeof(Boolean)));
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_telephone_number_c", typeof(string)));
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_fax_number_c", typeof(string)));
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_mobile_number_c", typeof(string)));
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_alternate_telephone_c", typeof(string)));
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_email_address_c", typeof(string)));
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_url_c", typeof(string)));

                // Add special DataColumns that are needed for the 'Best Address' calculation
                ReturnValue.Columns.Add(new System.Data.DataColumn(PARTNERLOCATION_ICON_COLUMN, typeof(Int32)));

                // Add a 'Calculated Column' for sorting
                IconForSortingCol = new DataColumn();
                IconForSortingCol.DataType = System.Type.GetType("System.Int32");
                IconForSortingCol.ColumnName = "Icon_For_Sorting";
                IconForSortingCol.Expression = "IIF(Icon = 1, 2, IIF(Icon = 2, 1, 3))"; // exchanges instead of Icon=1 we get Icon=2
                ReturnValue.Columns.Add(IconForSortingCol);

                // Add extra column for Country Code
                ReturnValue.Columns.Add(new System.Data.DataColumn("p_value_country_c", typeof(string)));

                // Specify the Primary Key of the new DataTabe
                ReturnValue.PrimaryKey = new DataColumn[] {
                    ReturnValue.Columns["p_partner_key_n"], ReturnValue.Columns["p_site_key_n"], ReturnValue.Columns["p_location_key_i"]
                };

                return ReturnValue;
            }

            /// <summary>
            /// Determines which address is the 'Best Address' of a Partner, and marks it in the DataColumn 'BestAddress'.
            /// </summary>
            /// <remarks>This method calls into an OpenPetra .DLL via .NET Reflection!</remarks>
            /// <param name="APartnerLocationsDT">DataTable containing all the addresses of a Partner.</param>
            /// <param name="ASiteKey">Site Key of the 'Best Address'.</param>
            /// <param name="ALocationKey">Location Key of the 'Best Address'.</param>
            /// <returns>True if a 'Best Address' was found,
            /// otherwise false. In the latter case ASiteKey and ALocationKey will be both -1, too.</returns>
            public static bool DetermineBestAddress(DataTable APartnerLocationsDT, out Int64 ASiteKey, out int ALocationKey)
            {
                bool ReturnValue;

                ASiteKey = -1;
                ALocationKey = -1;

                object[] MethodArguments = new object[] {
                    APartnerLocationsDT, ASiteKey, ALocationKey
                };

                ReturnValue =
                    Convert.ToBoolean(FRemoteClass.InvokeMember("DetermineBestAddress",
                            (BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod),
                            null, FInstantiator, MethodArguments, null));

                // Assign the values of the 'out' Arguments to this Methods' out Arguments!
                ASiteKey = (Int64)MethodArguments[1];
                ALocationKey = (int)MethodArguments[2];

                return ReturnValue;
            }

            /// <summary>
            /// Determines whether a p_partner_location record is the one that constitutes the Partners' 'Best Address'.
            /// </summary>
            /// <param name="AParterLocationDR">p_partner_location DataRow.</param>
            /// <returns>True if it is, false if it isn't the Partners' 'Best Address'.</returns>
            public static bool IsBestAddressPartnerLocationRecord(DataRow AParterLocationDR)
            {
                return ((bool)AParterLocationDR[PARTNERLOCATION_BESTADDR_COLUMN]) == true;
            }
        }

        /// <summary>
        /// Holds data of a p_partner_attribute Record.
        /// </summary>
        /// <remarks>This can't be a struct because we need to be able to assign
        /// values to the Index Property in an foreach loop.</remarks>
        public class PPartnerAttributeRecord
        {
            /// <summary>
            /// Insertion order of p_partner_attribute Records.
            /// </summary>
            public int InsertionOrderPerPartner
            {
                get; set;
            }

            /// <summary>
            /// Partner Key Column data.
            /// </summary>
            public Int64 PartnerKey
            {
                get; set;
            }

            /// <summary>
            /// Attribute Type Column data.
            /// </summary>
            public string AttributeType
            {
                get; set;
            }

            /// <summary>
            /// Sequence  Column data.
            /// </summary>
            public int Sequence
            {
                get; set;
            }

            /// <summary>
            /// Index Column data.
            /// </summary>
            public int Index
            {
                get; set;
            }

            /// <summary>
            /// Value Column data.
            /// </summary>
            public string Value
            {
                get; set;
            }

            /// <summary>
            /// Value Country Column data.
            /// </summary>
            public string ValueCountry
            {
                get; set;
            }

            /// <summary>
            /// Comment Column data.
            /// </summary>
            public string Comment
            {
                get; set;
            }

            /// <summary>
            /// Primary Column data.
            /// </summary>
            public bool Primary
            {
                get; set;
            }

            /// <summary>
            /// WithinOrganisation Column data.
            /// </summary>
            public bool WithinOrganisation
            {
                get; set;
            }

            /// <summary>
            /// Specialised Column data.
            /// </summary>
            public bool Specialised
            {
                get; set;
            }

            /// <summary>
            /// Confidential Column data.
            /// </summary>
            public bool Confidential
            {
                get; set;
            }

            /// <summary>
            /// Current Column data.
            /// </summary>
            public bool Current
            {
                get; set;
            }

            /// <summary>
            /// NoLongerCurrentFrom Column data.
            /// </summary>
            public DateTime ? NoLongerCurrentFrom
            {
                get; set;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Populate the empty table p_partner_attribute using selected data from p_partner_location.
        /// </summary>
        /// <returns>Number of created PPartnerAttribute Rows.</returns>
        public static int PopulatePPartnerAttribute()
        {
            int RowCounter = 0;
            Int64 BestPartnerLocSiteKey;
            int BestPartnerLocLocationKey;

            List <PPartnerAttributeRecord>PPARecords;

            if (FEmptyStringIndicator == null)
            {
                throw new EOPException("'FEmptyStringIndicator' must be set before Method 'PopulatePPartnerAttribute' gets called!");
            }

            // FPartnerLocationRecords got created when the p_partner_location table got processed; it holds selected Columns of all
            // p_partner_location records of each Partner that gets loaded.
//            TLogging.Log(String.Format("We have entries for {0} Partners.", FPartnerClassInformation.Count));

            // Process each Partner and its p_partner_location records
            foreach (Int64 PartnerKey in FPartnerClassInformation.Keys)
            {
                FInsertionOrderPerPartner = -1;

                // Get that Partner's p_partner_location records from PPartnerLocationRecords
                DataRow[] CurrentRows = FPartnerLocationRecords[Math.Abs(PartnerKey) % NumberOfTables].Select(
                    "p_partner_key_n = " + PartnerKey.ToString());

                if (CurrentRows.Length == 0)
                {
                    continue;
                }

                DataTable PPartnersLocationsDT = TPartnerContactDetails.BestAddressHelper.GetNewPPartnerLocationTableInstance();

                foreach (DataRow r in CurrentRows)
                {
                    PPartnersLocationsDT.Rows.Add(r.ItemArray);
                }

                // We hazard a guess that we will possibly create 2 times as many p_partner_attribute records as there are
                // p_partner_location records - but in some cases there will be more (if we need to create many 'primary'
                // p_partner_attribute records for Contact Details that come out of the 'Best Address' p_partner_location records,
                // or less (if there are many p_partner_location records that don't have details that will become a Contact Detail).
                // The guess is done so that PPARecords doesn't need to continually get expanded by .NET as we are adding to it.
                // (The 2.0 factor got determined by checking the average number of records that got created by running this
                // over actual data from the SA DB (1.3) and then rounding the number up, as often there will only be one
                // p_partner_location record, and so we create at least 2 entries in that case.)
                PPARecords = new List <PPartnerAttributeRecord>(PPartnersLocationsDT.Rows.Count * 2);

                // Determine the p_partner_location record that constitutes the 'Best Address' of that Partner
                if (!BestAddressHelper.DetermineBestAddress(PPartnersLocationsDT,
                        out BestPartnerLocSiteKey, out BestPartnerLocLocationKey))
                {
                    throw new Exception(String.Format(
                            "Problem determining the 'Best Address' for the p_partner_location records of Partner with PartnerKey {0}! Number of p_partner_location records: {1}",
                            PartnerKey, PPartnersLocationsDT.Rows.Count));
                }
                else
                {
                    if (PPartnersLocationsDT.Rows.Count > 1)
                    {
//                        TLogging.Log(
//                            String.Format(
//                                "'Best Address' for the p_parter_location records of Partner with PartnerKey {0} which has {1} p_partner_locations:   p_parter_location SiteKey: {2}; p_parter_location LocationKey: {3}",
//                                PartnerKey, PPartnersLocationsDT.Rows.Count, BestPartnerLocSiteKey, BestPartnerLocLocationKey));
                    }

                    //
                    // Create p_partner_attribute records for each p_partner_location records'
                    // columns that hold 'Contact Detail' data and have values, and are unique,
                    //

                    // We want to create the p_partner_attribute records in the same order that the user
                    // would see the p_location records (that we lift them from) on the Address Tab.
                    DataView SortedRecordsDV = PPartnersLocationsDT.DefaultView;
                    SortedRecordsDV.Sort = "Icon_For_Sorting ASC, p_date_effective_d DESC";

                    string CurrentFieldEmail = string.Empty;

                    // Look for current field address with an email address. If exists then it will be primary email.
                    for (int Counter = 0; Counter < SortedRecordsDV.Count; Counter++)
                    {
                        if ((Convert.ToInt32(SortedRecordsDV[Counter].Row[PARTNERLOCATION_ICON_COLUMN]) == 1)
                            && (SortedRecordsDV[Counter].Row["p_location_type_c"].ToString() == "FIELD")
                            && !string.IsNullOrEmpty(SortedRecordsDV[Counter].Row["p_email_address_c"].ToString())
                            && (SortedRecordsDV[Counter].Row["p_email_address_c"].ToString().Contains("@om.org")
                                || SortedRecordsDV[Counter].Row["p_email_address_c"].ToString().Contains("@gbaships.org"))
                            && (SortedRecordsDV[Counter].Row["p_email_address_c"].ToString() != "firstname.lastname@om.org"))
                        {
                            CurrentFieldEmail = SortedRecordsDV[Counter].Row["p_email_address_c"].ToString();
                            break;
                        }
                    }

                    for (int Counter = 0; Counter < SortedRecordsDV.Count; Counter++)
                    {
                        List <PPartnerAttributeRecord>PPARecordsSingleLocation =
                            ConstructPPartnerAttributeRecords(PartnerKey, SortedRecordsDV[Counter].Row, CurrentFieldEmail);

                        for (int SingleLocCounter = 0; SingleLocCounter < PPARecordsSingleLocation.Count; SingleLocCounter++)
                        {
                            // LINQ Query for de-duplication: Check if there are already PPARecords of a Partner that have
                            // the same Attribute Type and Value as a record from PPARecordsSingleLocation. If so, don't add
                            // the record from PPARecordsSingleLocation to PPARecords as it would create a duplicate Contact Detail!
                            // (it is often the case that several p_partner_location records of a Partner hold the same data!)
                            var PPARecordsContainsQuery =
                                from PPARecord in PPARecords
                                where PPARecord.AttributeType == PPARecordsSingleLocation[SingleLocCounter].AttributeType
                                && PPARecord.Value == PPARecordsSingleLocation[SingleLocCounter].Value
                                select PPARecord;

                            if (!PPARecordsContainsQuery.Any())
                            {
                                PPARecords.Add(PPARecordsSingleLocation[SingleLocCounter]);
                            }
                            else if (PPARecordsSingleLocation[SingleLocCounter].Primary)
                            {
                                var PPARecordsContainsQueryList = PPARecordsContainsQuery.ToList();

                                foreach (var ListItem in PPARecordsContainsQueryList)
                                {
                                    if (((ListItem.AttributeType == PPARecordsSingleLocation[SingleLocCounter].AttributeType)
                                         && (ListItem.Value == PPARecordsSingleLocation[SingleLocCounter].Value))
                                        && !ListItem.Primary)
                                    {
                                        TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_CHECK + "-100" +
                                            String.Format(
                                                "]: Primary {2} NOT SET for PartnerKey {0}: Based on the data we have we believe it is likely that the {2} '{1}' should become the Primary {2} of this Partner, but human decision is needed on that.",
                                                PartnerKey, PPARecordsSingleLocation[SingleLocCounter].Value,
                                                PPARecordsSingleLocation[SingleLocCounter].AttributeType));

                                        FWarnings100Collector.Add(PartnerKey.ToString());

                                        //ListItem.Primary = true;
                                        // // Primary items must be Current, too
                                        //ListItem.Current = true;
                                        // // Set comment to empty, too, as it might have contained 'In Petra 2.x this Contact Detail was set to become effective on...'
                                        //ListItem.Comment = String.Empty;

                                        break;
                                    }
                                }

                                if (PPARecordsSingleLocation[SingleLocCounter].WithinOrganisation)
                                {
                                    var PPARecordsContainsQueryList2 = PPARecordsContainsQuery.ToList();

                                    foreach (var ListItem in PPARecordsContainsQueryList2)
                                    {
                                        if (((ListItem.AttributeType == PPARecordsSingleLocation[SingleLocCounter].AttributeType)
                                             && (ListItem.Value == PPARecordsSingleLocation[SingleLocCounter].Value))
                                            && !ListItem.WithinOrganisation)
                                        {
                                            TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_CHECK + "-100" +
                                                String.Format(
                                                    "]: Within-Organisation E-mail NOT SET for PartnerKey {0}: Based on the data we have we believe it is likely that the E-Mail address '{1}' should become the Within-Organisation E-mail of this Partner, but human decision is needed on that.",
                                                    PartnerKey, PPARecordsSingleLocation[SingleLocCounter].Value));

                                            FWarnings100Collector.Add(PartnerKey.ToString());

                                            //ListItem.Primary = true;
                                            // // Primary items must be Current, too
                                            //ListItem.Current = true;
                                            // // Set comment to empty, too, as it might have contained 'In Petra 2.x this Contact Detail was set to become effective on...'
                                            //ListItem.Comment = String.Empty;

                                            break;
                                        }
                                    }
                                }
                            }
                            else if (PPARecordsSingleLocation[SingleLocCounter].Current)
                            {
                                var PPARecordsContainsQueryList3 = PPARecordsContainsQuery.ToList();

                                foreach (var ListItem in PPARecordsContainsQueryList3)
                                {
                                    if (((ListItem.AttributeType == PPARecordsSingleLocation[SingleLocCounter].AttributeType)
                                         && (ListItem.Value == PPARecordsSingleLocation[SingleLocCounter].Value))
                                        && !ListItem.Current)
                                    {
                                        TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_CHECK + "-110" +
                                            String.Format(
                                                "]: Non-Current {2} for PartnerKey {0}: Based on the data we have we believe it is likely that the {2} '{1}' should become Current, but human decision is needed on that.",
                                                PartnerKey, PPARecordsSingleLocation[SingleLocCounter].Value,
                                                PPARecordsSingleLocation[SingleLocCounter].AttributeType));

                                        FWarnings110Collector.Add(PartnerKey.ToString());

                                        //ListItem.Current = true;
                                        // // Set comment to empty, too, as it might have contained 'In Petra 2.x this Contact Detail was set to become effective on...'
                                        //ListItem.Comment = String.Empty;

                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // LINQ Query to establish Index values: Group all PPARecords of a Partner by
                    // Attribute Type and have the Primary record come first in each Group, followed by
                    // the other PPARecords in their insertion order.
                    var PPARecordsGroupedQuery =
                        from PPARecord in PPARecords
                        orderby PPARecord.Primary descending, PPARecord.InsertionOrderPerPartner
                    group PPARecord by PPARecord.AttributeType into grouping
                    orderby grouping.Key
                    select grouping;

                    // Iterate over each Group and determine the Index values of their individual members.
                    // The first member always starts with Index 0.
                    // Reason for this: The Contact Details Tab displays Contact Details grouped by
                    // Contact Category, and the ones of the same Contact Category are sorted by Index
                    // within each Contact Category. With this approach, 'Primary' PPARecords will be
                    // listed first, followed by other records in the order that a user would have seen
                    // them on the 'Addresses' Tab in Petra 2.x!
                    foreach (var PPARecordGroup in PPARecordsGroupedQuery)
                    {
                        int IndexInGroup = 0;

                        foreach (var PPARecGroupMember in PPARecordGroup)
                        {
                            PPARecGroupMember.Index = IndexInGroup++;
                        }
                    }

                    // Create new Rows and write them out
                    foreach (var PPARec in PPARecords)
                    {
                        CreateContactDetailsRow(PPARec, PartnerAttributeHoldingDataSet);
                    }

                    RowCounter += PPARecords.Count;
                }
            }

            // Get rid of the Data Structure that we don't need anymore!
            FPartnerLocationRecords = null;
            // ... and kindly ask .NET's Garbage Collector to really get it out of memory, if it's convenient.
            GC.Collect(0, GCCollectionMode.Optimized);

            CustomLoggingOfPartnerKeysForWarning();

            return RowCounter;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Puts PartnerKeys that were collected for various Warnings into separate files and logs an overview
        /// about those Warnings to a file, pointing to the invididual files.
        /// </summary>
        private static void CustomLoggingOfPartnerKeysForWarning()
        {
            Assembly asm = Assembly.GetCallingAssembly();
            String ExePath = Path.GetDirectoryName(new Uri(asm.EscapedCodeBase).LocalPath);
            string FulldumpPath = ExePath + Path.DirectorySeparatorChar + TAppSettingsManager.GetValue("fulldumpPath", "fulldump");
            string StatisticsPath = FulldumpPath + Path.DirectorySeparatorChar + "statistics";

            // Create the 'statistics' sub-directory if it isn't there yet (doesn't throw an error if it exists already!)
            System.IO.Directory.CreateDirectory(StatisticsPath);

            string PartnerKeysWherePrimaryWasntSetFileName = StatisticsPath + Path.DirectorySeparatorChar +
                                                             "Warning CHECK-100 - PrimaryNotSet" + ".txt";
            string PartnerKeysWhereCurrentWasntSetFileName = StatisticsPath + Path.DirectorySeparatorChar +
                                                             "Warning CHECK-110 - CurrentNotSet" + ".txt";

            string FieldCurrentPartnerKeysAffectedByWarningFileName = StatisticsPath + Path.DirectorySeparatorChar +
                                                                      "Warning CHECK-310 - FieldCurrentPartnerKeys" + ".txt";
            string FieldNonCurrentPartnerKeysAffectedByWarningFileName = StatisticsPath + Path.DirectorySeparatorChar +
                                                                         "Warning CHECK-310 - FieldNonCurrentPartnerKeys" + ".txt";
            string NonFieldCurrentPartnerKeysAffectedByWarningFileName = StatisticsPath + Path.DirectorySeparatorChar +
                                                                         "Warning CHECK-310 - NonFieldCurrentPartnerKeys" + ".txt";
            string NonFieldNonCurrentPartnerKeysAffectedByWarningFileName = StatisticsPath + Path.DirectorySeparatorChar +
                                                                            "Warning CHECK-310 - NonFieldNonCurrentPartnerKeys" + ".txt";

            string PartnerKeysWhereEmailSeemsNotToBeValidFileName = StatisticsPath + Path.DirectorySeparatorChar +
                                                                    "Warning CHECK-400 - EmailSeemsNotToBeValid" + ".txt";

            System.IO.File.WriteAllLines(PartnerKeysWherePrimaryWasntSetFileName, FWarnings100Collector.ToArray());
            System.IO.File.WriteAllLines(PartnerKeysWhereCurrentWasntSetFileName, FWarnings110Collector.ToArray());

            System.IO.File.WriteAllLines(FieldCurrentPartnerKeysAffectedByWarningFileName, FWarnings310CollectorFieldCurrentPartnerKeys.ToArray());
            System.IO.File.WriteAllLines(FieldNonCurrentPartnerKeysAffectedByWarningFileName, FWarnings310CollectorFieldNonCurrentPartnerKeys.ToArray());

            System.IO.File.WriteAllLines(NonFieldCurrentPartnerKeysAffectedByWarningFileName,
                FFWarnings310CollectorNonFieldCurrentPartnerKeys.ToArray());
            System.IO.File.WriteAllLines(NonFieldNonCurrentPartnerKeysAffectedByWarningFileName,
                FFWarnings310CollectorNonFieldNonCurrentPartnerKeys.ToArray());

            System.IO.File.WriteAllLines(PartnerKeysWhereEmailSeemsNotToBeValidFileName, FWarnings400Collector.ToArray());


            // Build a string that contains and overview that we will write to a file
            string StatisticFileLines =
                "PartnerKeys Affected By Warnings - Statistics" + Environment.NewLine +
                "=============================================" + Environment.NewLine + Environment.NewLine +
                String.Format("* Warning 'CHECK-100': Number of Contact Details that weren't set to Primary/Within Organisation: {0}",
                    FWarnings100Collector.Count) + Environment.NewLine +
                "  -> Found in file '" + PartnerKeysWherePrimaryWasntSetFileName + "'" + Environment.NewLine + Environment.NewLine +
                String.Format("* Warning 'CHECK-110': Number of Contact Details that weren't set to Current: {0}",
                    FWarnings110Collector.Count) + Environment.NewLine +
                "  -> Found in file '" + PartnerKeysWhereCurrentWasntSetFileName + "'" + Environment.NewLine + Environment.NewLine +
                String.Format("* Warning 'CHECK-310': Number of Current Partners whose PartnerKeys start with {0}: {1}",
                    35,
                    FWarnings310CollectorFieldCurrentPartnerKeys.Count) + Environment.NewLine +
                "  -> Found in file '" + FieldCurrentPartnerKeysAffectedByWarningFileName + "'" + Environment.NewLine +
                String.Format("* Warning 'CHECK-310': Number of non-Current Contact Details whose PartnerKeys start with {0}: {1}",
                    35,
                    FWarnings310CollectorFieldNonCurrentPartnerKeys.Count) + Environment.NewLine +
                "  -> Found in file '" + FieldNonCurrentPartnerKeysAffectedByWarningFileName + "'" + Environment.NewLine +
                String.Format("* Warning 'CHECK-310': Number of Current Contact Details whose PartnerKeys don't start with {0}: {1}",
                    35,
                    FFWarnings310CollectorNonFieldCurrentPartnerKeys.Count) + Environment.NewLine +
                "  -> Found in file '" + NonFieldCurrentPartnerKeysAffectedByWarningFileName + "'" + Environment.NewLine +
                String.Format(
                    "* Warning 'CHECK-310': Number of non-Current Contact Details whose PartnerKeys don't start with {0}: {1}",
                    35,
                    FFWarnings310CollectorNonFieldNonCurrentPartnerKeys.Count + Environment.NewLine +
                    "  -> Found in file '" + NonFieldNonCurrentPartnerKeysAffectedByWarningFileName + "'" + Environment.NewLine +
                    Environment.NewLine +
                    String.Format("* Warning 'WARNING-400': Number of E-Mail Contact Details where the E-Mail seems not to be valid: {0}",
                        FWarnings400Collector.Count) + Environment.NewLine +
                    "  -> Found in file '" + PartnerKeysWhereEmailSeemsNotToBeValidFileName + "'");

            // Write the string to a file
            using (var StatisticsFile = new System.IO.StreamWriter(StatisticsPath + Path.DirectorySeparatorChar +
                       "PartnerKeys Affected By Warnings - Statistics.txt"))
            {
                StatisticsFile.WriteLine(StatisticFileLines);

                StatisticsFile.Close();
            }
        }

        private static List <PPartnerAttributeRecord>ConstructPPartnerAttributeRecords(
            Int64 APartnerKey, DataRow APartnerLocationDR, string ACurrentFieldEmail)
        {
            var ReturnValue = new List <TPartnerContactDetails.PPartnerAttributeRecord>();
            var PPARecordList = new List <PPartnerAttributeRecord>();
            PPartnerAttributeRecord PPARecord;
            PPartnerAttributeRecord PPARecordEmail = null;
            bool IsBestAddr = BestAddressHelper.IsBestAddressPartnerLocationRecord(APartnerLocationDR);
            bool AnyTelephoneNumberSetAsPrimary = false;
            bool AnyEmailSetAsPrimary = false;
            bool AnyEmailSetAsWithinOrganisation = false;
            bool IsAnEmailAddressInAWrongField = false;
            bool IsHoldingASkypeID = false;

            List <Tuple <string, string>>SplitValues;
            bool SplitValuesContainOnlyDefaultType;
            // Variables that hold record information
            string TelephoneNumberString = (string)APartnerLocationDR["p_telephone_number_c"];
            string FaxNumberString = (string)APartnerLocationDR["p_fax_number_c"];
            string MobileNumberString = (string)APartnerLocationDR["p_mobile_number_c"];
            string AlternatePhoneNumberString = (string)APartnerLocationDR["p_alternate_telephone_c"];
            string UrlString = (string)APartnerLocationDR["p_url_c"];
            string EmailAddressString = (string)APartnerLocationDR["p_email_address_c"];
            string CountryCode = GetCountryCode(APartnerLocationDR);
            string CountryCodeOrig = (CountryCode != null ? String.Copy(CountryCode) : CountryCode);

            ACurrentFieldEmail.Trim().Replace("\t", " ");  // The last statement replaces any <TAB> characters inside the string with a single space character each, see Bugs #4620, #4625!

            FInsertionOrderPerPartner++;

            //
            // Work on the various p_partner_location DataColumns that hold data and that will each be migrated to
            // a Contact Detail record - if they hold data.
            //

            if ((TelephoneNumberString != FEmptyStringIndicator)
                || (EmailAddressString != FEmptyStringIndicator))
            {
                // EmailAddressString processing needs to come FIRST as any email address that was really the email address
                // in the Partner's p_partner_location record ought to take precedence over any other email addresses
                // that might have been entered into other p_partner_location columns (e.g. the phone number)
                // (Important for for determining the Primary and WithinOrganisation flags).
                if (EmailAddressString != FEmptyStringIndicator)
                {
                    EmailAddressString = EmailAddressString.Trim().Replace("\t", " ");  // The last statement replaces any <TAB> characters inside the string with a single space character each, see Bugs #4620, #4625!
                    bool LogRemark = false;
                    bool LogWarning = false;

                    // Do not yet split up email addresses that could be seperated by a colon or semi colon...
                    PPARecordEmail = GetNewPPartnerAttributeRecord(APartnerKey, APartnerLocationDR);

                    string[] EmailAddresses = StringHelper.SplitEmailAddresses(EmailAddressString);

                    if (EmailAddresses.Length > 1)
                    {
                        // ...but log the fact that we didn't do that if there are multiple email addresses!
                        LogRemark = true;
                    }

                    if (TStringChecks.ValidateEmail(EmailAddressString, true) != null)
                    {
                        LogWarning = true;
                    }

                    IsHoldingASkypeID = IsStringHoldingASkypeID(EmailAddressString);

                    PPARecordEmail.Value = EmailAddressString;
                    PPARecordEmail.AttributeType = !IsHoldingASkypeID ? ATTR_TYPE_EMAIL : ATTR_TYPE_SKYPE;

                    if (!IsHoldingASkypeID)
                    {
                        SpecialEmailProcessing(EmailAddressString,
                            ACurrentFieldEmail,
                            APartnerKey,
                            IsBestAddr,
                            ref PPARecordEmail,
                            ref AnyEmailSetAsPrimary,
                            ref AnyEmailSetAsWithinOrganisation);

                        if (LogRemark)
                        {
                            TLogging.Log(String.Format(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_INFO + "-410" +
                                    "]: Email Address '{1}' for PartnerKey {0} contained {2} E-mail Addresses. However, we didn't split those up " +
                                    "but migrated them as a single E-Mail Contact Detail record (for compatibility reasons). (Cont.Detail Record Current: {3})",
                                    APartnerKey, EmailAddressString, EmailAddresses.Length, PPARecordEmail.Current));
                        }

                        if (LogWarning)
                        {
                            TLogging.Log(String.Format(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_WARN + "-400" +
                                    ((EmailAddresses.Length ==
                                      1) ?
                                     "]: Email Address '{1}' for PartnerKey {0} seems not to be a valid E-mail Address! (Cont.Detail Record Current: {2})"
                                     :
                                     "]: Email Address '{1}' seems to contain at least one invalid E-mail Address! (Cont.Detail Record Current: {2})"),
                                    APartnerKey, EmailAddressString, PPARecordEmail.Current));

                            FWarnings400Collector.Add(APartnerKey.ToString());
                        }
                    }
                    else
                    {
                        SpecialSkypeIDProcessing(ref PPARecordEmail);
                    }

                    // Caveat: Adding to PPARecordList happens AFTER any potential Telephone got processed
                    // (important only for helping in comparing various outputs of the data conversion)
                }

                // Set data parts that depend on certain conditions
                if (TelephoneNumberString != FEmptyStringIndicator)
                {
                    PPARecord = null;

                    // There could be a number of phone numbers seperated by a semi colon. We need to split these up and add them seperately.
                    string[] TelephoneNumbers = TelephoneNumberString.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    SplitValues = EmtpySplitValuesList(TelephoneNumbers);

                    for (int i = 0; i < TelephoneNumbers.Length; i++)
                    {
                        string TelephoneNumber = TelephoneNumbers[i].Trim().Replace("\t", " ");  // The last statement replaces any <TAB> characters inside the string with a single space character each, see Bugs #4620, #4625!;

                        if (TelephoneNumber == String.Empty)
                        {
                            // If value is empty after trimming don't process it (ie. if it was only made up of space/TAB characters)
                            continue;
                        }

                        PPARecord = GetNewPPartnerAttributeRecord(APartnerKey, APartnerLocationDR);

                        IsAnEmailAddressInAWrongField = TStringChecks.ValidateEmail(TelephoneNumber, true) == null;
                        IsHoldingASkypeID = IsStringHoldingASkypeID(TelephoneNumber);

                        if ((!IsAnEmailAddressInAWrongField)
                            && (!IsHoldingASkypeID))
                        {
                            TelephoneNumber = RemoveInternationalCodeFromTelephoneNumber(TelephoneNumber,
                                ref CountryCode, ATTR_TYPE_PHONE, APartnerKey, PPARecord);
                        }

                        PPARecord.Value = TelephoneNumber;
                        PPARecord.ValueCountry = CountryCode;
                        PPARecord.AttributeType = GetAttributeType(ATTR_TYPE_PHONE, IsAnEmailAddressInAWrongField, IsHoldingASkypeID);

                        if ((!IsAnEmailAddressInAWrongField)
                            && (!IsHoldingASkypeID))
                        {
                            if ((i == 0)
                                && (IsBestAddr)
                                && (PPARecord.Current))
                            {
                                // Mark this Contact Detail as being 'Primary' - but only if the Contact Detail is current!
                                PPARecord.Primary = true;

                                AnyTelephoneNumberSetAsPrimary = true;

                                //                        TLogging.Log(String.Format(
                                //                                "Made Telephone Number '{0}' the 'Primary Phone' (PartnerKey: {1}, LocationKey: {2})",
                                //                                TelephoneNumber, APartnerKey, APartnerLocationDR["p_location_key_i"]));
                            }

                            SplitValues.Add(new Tuple <string, string>(BuildTypeWithPrimaryPrefix("Phone", PPARecord.Primary), PPARecord.Value));
                        }
                        else
                        {
                            if (IsAnEmailAddressInAWrongField)
                            {
                                SpecialEmailProcessing(TelephoneNumber,
                                    ACurrentFieldEmail,
                                    APartnerKey,
                                    IsBestAddr,
                                    ref PPARecord,
                                    ref AnyEmailSetAsPrimary,
                                    ref AnyEmailSetAsWithinOrganisation,
                                    "Telephone Number");

                                SplitValues.Add(new Tuple <string, string>(
                                        BuildTypeWithPrimaryPrefix("E-Mail (!)", PPARecord.Primary), PPARecord.Value));
                            }
                            else if (IsHoldingASkypeID)
                            {
                                SpecialSkypeIDProcessing(ref PPARecord);

                                SplitValues.Add(new Tuple <string, string>("Skype (!)", PPARecord.Value));
                            }
                        }

                        PPARecordList.Add(PPARecord);
                    }

                    if (PPARecord != null)
                    {
                        // Detect if there was more than one Contact Detail processed -> write out log message if this is the case
                        if (SplitValues.Count > 1)
                        {
                            TLogging.Log(String.Format(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_INFO + "-501" +
                                    "]: Telephone Number '" + TelephoneNumberString +
                                    "' held more than one item and hence got split into the " +
                                    "following {0}Contact Details: " + BuildSplitValuesListStr(SplitValues,
                                        "Phone", out SplitValuesContainOnlyDefaultType) + " (Cont.Detail Record Current: {1})",
                                    SplitValuesContainOnlyDefaultType ? "Phone " : "", PPARecord.Current));
                        }
                    }
                }

                if (EmailAddressString != FEmptyStringIndicator)
                {
                    PPARecordList.Add(PPARecordEmail);
                }
            }

            if (FaxNumberString != FEmptyStringIndicator)
            {
                PPARecord = null;

                // There could be a number of fax numbers seperated by a semi colon. We need to split these up and add them seperately.
                string[] FaxNumbers = FaxNumberString.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                SplitValues = EmtpySplitValuesList(FaxNumbers);

                for (int i = 0; i < FaxNumbers.Length; i++)
                {
                    string FaxNumber = FaxNumbers[i].Trim().Replace("\t", " ");  // The last statement replaces any <TAB> characters inside the string with a single space character each, see Bugs #4620, #4625!

                    if (FaxNumber == String.Empty)
                    {
                        // If value is empty after trimming don't process it (ie. if it was only made up of space/TAB characters)
                        continue;
                    }

                    IsAnEmailAddressInAWrongField = TStringChecks.ValidateEmail(FaxNumber, true) == null;
                    IsHoldingASkypeID = IsStringHoldingASkypeID(FaxNumber);

                    PPARecord = GetNewPPartnerAttributeRecord(APartnerKey, APartnerLocationDR);

                    if ((!IsAnEmailAddressInAWrongField)
                        && (!IsHoldingASkypeID))
                    {
                        // In case the CountryCode variable got set to something else above: set it up again.
                        CountryCode = CountryCodeOrig;

                        FaxNumber = RemoveInternationalCodeFromTelephoneNumber(FaxNumber, ref CountryCode, ATTR_TYPE_FAX, APartnerKey,
                            PPARecord);
                    }

                    PPARecord.Value = FaxNumber;
                    PPARecord.ValueCountry = CountryCode;
                    PPARecord.AttributeType = GetAttributeType(ATTR_TYPE_FAX, IsAnEmailAddressInAWrongField, IsHoldingASkypeID);

                    if ((!IsAnEmailAddressInAWrongField)
                        && (!IsHoldingASkypeID))
                    {
                        SplitValues.Add(new Tuple <string, string>(BuildTypeWithPrimaryPrefix("Fax", PPARecord.Primary), PPARecord.Value));
                    }

                    if (IsAnEmailAddressInAWrongField)
                    {
                        SpecialEmailProcessing(FaxNumber,
                            ACurrentFieldEmail,
                            APartnerKey,
                            IsBestAddr,
                            ref PPARecord,
                            ref AnyEmailSetAsPrimary,
                            ref AnyEmailSetAsWithinOrganisation,
                            "Fax Number");

                        SplitValues.Add(new Tuple <string,
                                                   string>(BuildTypeWithPrimaryPrefix("E-Mail (!)", PPARecord.Primary), PPARecord.Value));
                    }
                    else if (IsHoldingASkypeID)
                    {
                        SpecialSkypeIDProcessing(ref PPARecord);

                        SplitValues.Add(new Tuple <string, string>("Skype (!)", PPARecord.Value));
                    }

                    PPARecordList.Add(PPARecord);
                }

                if (PPARecord != null)
                {
                    // Detect if there was more than one Contact Detail processed -> write out log message if this is the case
                    if (SplitValues.Count > 1)
                    {
                        TLogging.Log(String.Format(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_INFO + "-502" +
                                "]: Fax Number '" + FaxNumberString +
                                "' held more than one item and hence got split into the " +
                                "following {0}Contact Details: " + BuildSplitValuesListStr(SplitValues,
                                    "Fax", out SplitValuesContainOnlyDefaultType) + " (Cont.Detail Record Current: {1})",
                                SplitValuesContainOnlyDefaultType ? "Fax " : "", PPARecord.Current));
                    }
                }
            }

            if (MobileNumberString != FEmptyStringIndicator)
            {
                PPARecord = null;

                // There could be a number of mobile numbers seperated by a semi colon. We need to split these up and add them seperately.
                string[] MobileNumbers = MobileNumberString.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                SplitValues = EmtpySplitValuesList(MobileNumbers);

                for (int i = 0; i < MobileNumbers.Length; i++)
                {
                    string MobileNumber = MobileNumbers[i].Trim().Replace("\t", " ");  // The last statement replaces any <TAB> characters inside the string with a single space character each, see Bugs #4620, #4625!

                    if (MobileNumber == String.Empty)
                    {
                        // If value is empty after trimming don't process it (ie. if it was only made up of space/TAB characters)
                        continue;
                    }

                    IsAnEmailAddressInAWrongField = TStringChecks.ValidateEmail(MobileNumber, true) == null;
                    IsHoldingASkypeID = IsStringHoldingASkypeID(MobileNumber);

                    PPARecord = GetNewPPartnerAttributeRecord(APartnerKey, APartnerLocationDR);

                    if ((!IsAnEmailAddressInAWrongField)
                        && (!IsHoldingASkypeID))
                    {
                        // In case the CountryCode variable got set to something else above: set it up again.
                        CountryCode = CountryCodeOrig;

                        MobileNumber = RemoveInternationalCodeFromTelephoneNumber(MobileNumber, ref CountryCode, ATTR_TYPE_MOBILE_PHONE, APartnerKey,
                            PPARecord);
                    }

                    PPARecord.Value = MobileNumber;
                    PPARecord.ValueCountry = CountryCode;
                    PPARecord.AttributeType = GetAttributeType(ATTR_TYPE_MOBILE_PHONE, IsAnEmailAddressInAWrongField, IsHoldingASkypeID);

                    if ((!IsAnEmailAddressInAWrongField)
                        && (!IsHoldingASkypeID))
                    {
                        SplitValues.Add(new Tuple <string, string>(BuildTypeWithPrimaryPrefix("Mobile Phone", PPARecord.Primary), PPARecord.Value));
                    }

                    if (!IsAnEmailAddressInAWrongField)
                    {
                        if ((!AnyTelephoneNumberSetAsPrimary)
                            && (IsBestAddr)
                            && (PPARecord.Current))
                        {
                            // Mark this Contact Detail as being 'Primary' - but only if no other telephone number has been set as primary already and
                            // when the Contact Detail is current!
                            PPARecord.Primary = true;

                            AnyTelephoneNumberSetAsPrimary = true;

                            //                    TLogging.Log(String.Format(
                            //                            "Made MOBILE Number '{0}' the 'Primary Phone' (PartnerKey: {1}, LocationKey: {2})",
                            //                            MobileNumber, APartnerKey, APartnerLocationDR["p_location_key_i"]));
                        }
                    }
                    else
                    {
                        if (IsAnEmailAddressInAWrongField)
                        {
                            SpecialEmailProcessing(MobileNumber,
                                ACurrentFieldEmail,
                                APartnerKey,
                                IsBestAddr,
                                ref PPARecord,
                                ref AnyEmailSetAsPrimary,
                                ref AnyEmailSetAsWithinOrganisation,
                                "Mobile Number");

                            SplitValues.Add(new Tuple <string,
                                                       string>(BuildTypeWithPrimaryPrefix("E-Mail (!)", PPARecord.Primary), PPARecord.Value));
                        }
                        else if (IsHoldingASkypeID)
                        {
                            SpecialSkypeIDProcessing(ref PPARecord);

                            SplitValues.Add(new Tuple <string, string>("Skype (!)", PPARecord.Value));
                        }
                    }

                    PPARecordList.Add(PPARecord);
                }

                if (PPARecord != null)
                {
                    // Detect if there was more than one Contact Detail processed -> write out log message if this is the case
                    if (SplitValues.Count > 1)
                    {
                        TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_INFO + "-503" +
                            String.Format("]: Mobile Phone Number '" + MobileNumberString +
                                "' held more than one item and hence got split into the " +
                                "following {0}Contact Details: " + BuildSplitValuesListStr(SplitValues,
                                    "Mobile Phone", out SplitValuesContainOnlyDefaultType) + " (Cont.Detail Record Current: {1})",
                                SplitValuesContainOnlyDefaultType ? "Mobile Phone " : "", PPARecord.Current));
                    }
                }
            }

            if (AlternatePhoneNumberString != FEmptyStringIndicator)
            {
                PPARecord = null;

                // There could be a number of mobile numbers seperated by a semi colon. We need to split these up and add them seperately.
                string[] AlternatePhoneNumbers = AlternatePhoneNumberString.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                SplitValues = EmtpySplitValuesList(AlternatePhoneNumbers);

                for (int i = 0; i < AlternatePhoneNumbers.Length; i++)
                {
                    string AlternatePhoneNumber = AlternatePhoneNumbers[i].Trim().Replace("\t", " ");  // The last statement replaces any <TAB> characters inside the string with a single space character each, see Bugs #4620, #4625!

                    if (AlternatePhoneNumber == String.Empty)
                    {
                        // If value is empty after trimming don't process it (ie. if it was only made up of space/TAB characters)
                        continue;
                    }

                    IsAnEmailAddressInAWrongField = TStringChecks.ValidateEmail(AlternatePhoneNumber, true) == null;
                    IsHoldingASkypeID = IsStringHoldingASkypeID(AlternatePhoneNumber);

                    PPARecord = GetNewPPartnerAttributeRecord(APartnerKey, APartnerLocationDR);

                    if ((!IsAnEmailAddressInAWrongField)
                        && (!IsHoldingASkypeID))
                    {
                        // In case the CountryCode variable got set to something else above: set it up again.
                        CountryCode = CountryCodeOrig;

                        AlternatePhoneNumber = RemoveInternationalCodeFromTelephoneNumber(AlternatePhoneNumber,
                            ref CountryCode, ATTR_TYPE_PHONE, APartnerKey, PPARecord);
                    }

                    PPARecord.Value = AlternatePhoneNumber;
                    PPARecord.ValueCountry = CountryCode;
                    PPARecord.AttributeType = GetAttributeType(ATTR_TYPE_PHONE, IsAnEmailAddressInAWrongField, IsHoldingASkypeID);

                    if ((!IsAnEmailAddressInAWrongField)
                        && (!IsHoldingASkypeID))
                    {
                        if ((!AnyTelephoneNumberSetAsPrimary)
                            && (IsBestAddr)
                            && (PPARecord.Current))
                        {
                            // Mark this Contact Detail as being 'Primary' - but only if no other telephone number has been set as primary already and
                            // when the Contact Detail is current!
                            PPARecord.Primary = true;

                            AnyTelephoneNumberSetAsPrimary = true;

                            //                    TLogging.Log(String.Format(
                            //                            "Made ALTERNATE Phone Number '{0}' the 'Primary Phone' (PartnerKey: {1}, LocationKey: {2})",
                            //                            AlternatePhoneNumber, APartnerKey, APartnerLocationDR["p_location_key_i"]));
                        }

                        SplitValues.Add(new Tuple <string, string>(BuildTypeWithPrimaryPrefix("Phone", PPARecord.Primary), PPARecord.Value));
                    }
                    else
                    {
                        if (IsAnEmailAddressInAWrongField)
                        {
                            SpecialEmailProcessing(AlternatePhoneNumber,
                                ACurrentFieldEmail,
                                APartnerKey,
                                IsBestAddr,
                                ref PPARecord,
                                ref AnyEmailSetAsPrimary,
                                ref AnyEmailSetAsWithinOrganisation,
                                "Alternate Number");

                            SplitValues.Add(new Tuple <string, string>(
                                    BuildTypeWithPrimaryPrefix("E-Mail (!)", PPARecord.Primary), PPARecord.Value));
                        }
                        else if (IsHoldingASkypeID)
                        {
                            SpecialSkypeIDProcessing(ref PPARecord);

                            SplitValues.Add(new Tuple <string, string>("Skype (!)", PPARecord.Value));
                        }
                    }

                    PPARecordList.Add(PPARecord);
                }

                if (PPARecord != null)
                {
                    // Detect if there was more than one Contact Detail processed -> write out log message if this is the case
                    if (SplitValues.Count > 1)
                    {
                        TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_INFO + "-504" +
                            String.Format("]: Alternate Phone Number '" + AlternatePhoneNumberString +
                                "' held more than one item and hence got split into the " +
                                "following {0}Contact Details: " + BuildSplitValuesListStr(SplitValues,
                                    "Phone", out SplitValuesContainOnlyDefaultType) + " (Cont.Detail Record Current: {1})",
                                SplitValuesContainOnlyDefaultType ? "Phone " : "", PPARecord.Current));
                    }
                }
            }

            if (UrlString != FEmptyStringIndicator)
            {
                PPARecord = null;

                // There could be a number of urls seperated by a semi colon. We need to split these up and add them seperately.
                string[] Urls = UrlString.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                SplitValues = EmtpySplitValuesList(Urls);

                for (int i = 0; i < Urls.Length; i++)
                {
                    string Url = Urls[i].Trim().Replace("\t", " ");  // The last statement replaces any <TAB> characters inside the string with a single space character each, see Bugs #4620, #4625!

                    if (Url == String.Empty)
                    {
                        // If value is empty after trimming don't process it (ie. if it was only made up of space/TAB characters)
                        continue;
                    }

                    IsAnEmailAddressInAWrongField = TStringChecks.ValidateEmail(Url, true) == null;
                    IsHoldingASkypeID = IsStringHoldingASkypeID(Url);

                    PPARecord = GetNewPPartnerAttributeRecord(APartnerKey, APartnerLocationDR);

                    PPARecord.Value = Url;
                    PPARecord.AttributeType = GetAttributeType(ATTR_TYPE_WEBSITE, IsAnEmailAddressInAWrongField, IsHoldingASkypeID);

                    if ((!IsAnEmailAddressInAWrongField)
                        && (!IsHoldingASkypeID))
                    {
                        SplitValues.Add(new Tuple <string, string>(BuildTypeWithPrimaryPrefix("Web Site", PPARecord.Primary), PPARecord.Value));
                    }

                    if (IsAnEmailAddressInAWrongField)
                    {
                        SpecialEmailProcessing(Url,
                            ACurrentFieldEmail,
                            APartnerKey,
                            IsBestAddr,
                            ref PPARecord,
                            ref AnyEmailSetAsPrimary,
                            ref AnyEmailSetAsWithinOrganisation,
                            "Web Site/URL");

                        SplitValues.Add(new Tuple <string, string>(
                                BuildTypeWithPrimaryPrefix("E-Mail (!)", PPARecord.Primary), PPARecord.Value));
                    }
                    else if (IsHoldingASkypeID)
                    {
                        SpecialSkypeIDProcessing(ref PPARecord);

                        SplitValues.Add(new Tuple <string, string>("Skype (!)", PPARecord.Value));
                    }

                    PPARecordList.Add(PPARecord);
                }

                if (PPARecord != null)
                {
                    // Detect if there was more than one Contact Detail processed -> write out log message if this is the case
                    if (SplitValues.Count > 1)
                    {
                        TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_INFO + "-505" +
                            String.Format("]: Web Site (URL) '" + UrlString +
                                "' held more than one item and hence got split into the " +
                                "following {0}Contact Details: " + BuildSplitValuesListStr(SplitValues,
                                    "Web Site", out SplitValuesContainOnlyDefaultType) + " (Cont.Detail Record Current: {1})",
                                SplitValuesContainOnlyDefaultType ? "Web Site " : "", PPARecord.Current));
                    }
                }
            }

            // Now add all created records to the ReturnValue
            foreach (var PPARec in PPARecordList)
            {
                ReturnValue.Add(PPARec);
            }

            return ReturnValue;
        }

        private static List <Tuple <string, string>>EmtpySplitValuesList(string[] AValueArray)
        {
            return new List <Tuple <string, string>>(AValueArray.Length);
        }

        private static void SpecialEmailProcessing(string AEmailAddressString, string ACurrentFieldEmail, Int64 APartnerKey,
            bool AIsBestAddr, ref PPartnerAttributeRecord APPARecord, ref bool AAnyEmailSetAsPrimary, ref bool AAnyEmailSetAsWithinOrganisation,
            string AOriginatingField = null)
        {
            string PartnerClass;
            bool PrimaryEmailBecauseOfCurrentFieldEmail = (!string.IsNullOrEmpty(ACurrentFieldEmail)
                                                           && (ACurrentFieldEmail == AEmailAddressString));
            bool PotentiallyFlagUpAdditionalWarning = false;

            // If this is the predetermined current field email address...
            PrimaryEmailBecauseOfCurrentFieldEmail = (!string.IsNullOrEmpty(ACurrentFieldEmail)
                                                      && (ACurrentFieldEmail == AEmailAddressString));

            // ...or if there is no current field email but this is the Best Address and it is Current...
            if (PrimaryEmailBecauseOfCurrentFieldEmail
                || ((string.IsNullOrEmpty(ACurrentFieldEmail)) && (AIsBestAddr) && (APPARecord.Current)))
            {
                // ...then make this E-mail Contact Detail the 'Primary E-Mail'
                if (!AAnyEmailSetAsPrimary)
                {
                    APPARecord.Primary = true;
                    AAnyEmailSetAsPrimary = true;

                    if (AOriginatingField != null)
                    {
                        TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_ADVICE + "-230" +
                            String.Format("]: The E-Mail Address '{1}' of Partner {0} " +
                                "was set as the Primary E-Mail address of this Partner; however, it wasn't taken from the 'E-Mail' field " +
                                "of the Partner's Address, but from the '{2}' field (the 'E-Mail' field held no e-mail address). " +
                                "(Cont.Detail Record Current: true)", APartnerKey, APPARecord.Value, AOriginatingField));
                    }
                }

                if (PrimaryEmailBecauseOfCurrentFieldEmail
                    && (!APPARecord.Current))
                {
                    APPARecord.Current = true;

                    PotentiallyFlagUpAdditionalWarning = true;

                    TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_CHECK + "-200" +
                        String.Format("]: The E-Mail Address '{1}' of Partner {0} " +
                            "was set as the Primary E-Mail address of this Partner because a Current Field E-Mail address exists, although " +
                            "the E-Mail address was also contained in an Address that isn't current. (This should be the right thing to do, " +
                            "but please check.) (Cont.Detail Record Current: {2})", APartnerKey, APPARecord.Value, APPARecord.Current));

                    // Set comment to empty, too, as it might have contained 'In Petra 2.x this Contact Detail was set to become effective on...'
                    APPARecord.Comment = String.Empty;
                }

                // Mark this Contact Detail as being 'WithinOrganisation' as it has an 'organisation-internal' e-mail-address!
                // - but only if the Partner is a PERSON!
                if (FPartnerClassInformation.TryGetValue(APartnerKey, out PartnerClass))
                {
                    if (PartnerClass == "PERSON")
                    {
                        if (AEmailAddressString.EndsWith("@om.org", StringComparison.InvariantCulture))
                        {
                            if (!AAnyEmailSetAsWithinOrganisation)
                            {
                                APPARecord.WithinOrganisation = true;
                                AAnyEmailSetAsWithinOrganisation = true;
                            }

                            if (PotentiallyFlagUpAdditionalWarning)
                            {
                                TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_CHECK + "-210" +
                                    String.Format("]: The E-Mail Address '{1}' of Partner {0} " +
                                        "was set as the 'Within Organsiation' E-Mail address of this Partner, too, because a Current Field E-Mail address exists, although "
                                        +
                                        "the E-Mail address was also contained in an Address that isn't current. (This should be the right thing to do, "
                                        +
                                        "but please check.) (Cont.Detail Record Current: {2})", APartnerKey, APPARecord.Value, APPARecord.Current));
                            }

                            //                                    TLogging.Log(String.Format(
                            //                                            "Made email address '{0}' a 'WithinOrganisation' e-mail address (PartnerKey: {1}, LocationKey: {2})",
                            //                                            EmailAddress, APartnerKey, APartnerLocationDR["p_location_key_i"]));
                        }
                    }
                }
            }

            APPARecord.ValueCountry = String.Empty;
        }

        private static void SpecialSkypeIDProcessing(ref PPartnerAttributeRecord APPARecord)
        {
            if (APPARecord.Value.StartsWith(PREFIX_SKYPE1, true, CultureInfo.InvariantCulture))
            {
                APPARecord.Value = APPARecord.Value.Substring(PREFIX_SKYPE1.Length).Trim();  // Trim removes any spaces between the position of the prefix and the Skype ID
                APPARecord.ValueCountry = String.Empty;
            }
            else if (APPARecord.Value.StartsWith(PREFIX_SKYPE2, true, CultureInfo.InvariantCulture))
            {
                APPARecord.Value = APPARecord.Value.Substring(PREFIX_SKYPE2.Length).Trim();  // Trim removes any spaces between the position of the prefix and the Skype ID
                APPARecord.ValueCountry = String.Empty;
            }
            else
            {
                // We can't recognise the prefix that ought to denote that a Skype ID is to follow: no change
            }
        }

        private static bool IsStringHoldingASkypeID(string AString)
        {
            if ((AString.StartsWith(PREFIX_SKYPE1, true, CultureInfo.InvariantCulture))
                || (AString.StartsWith(PREFIX_SKYPE2, true, CultureInfo.InvariantCulture)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string GetAttributeType(string ADefaultAttributeTypeForThisCase, bool AIsAnEmailAddressInAWrongField, bool AIsSkypeID)
        {
            if (!AIsAnEmailAddressInAWrongField
                && !AIsSkypeID)
            {
                return ADefaultAttributeTypeForThisCase;
            }
            else if (AIsAnEmailAddressInAWrongField)
            {
                return ATTR_TYPE_EMAIL;
            }
            else if (AIsSkypeID)
            {
                return ATTR_TYPE_SKYPE;
            }

            // Fallback...
            return ATTR_TYPE_PHONE;
        }

        private static string BuildSplitValuesListStr(List <Tuple <string, string>>ASplitValues, string ADefaultType,
            out bool AContainsOnlyDefaultType)
        {
            string ReturnValue = String.Empty;
            string Postfix;

            AContainsOnlyDefaultType = true;

            foreach (var SplittedValue in ASplitValues)
            {
                if (!((SplittedValue.Item1 == ADefaultType)
                      || (SplittedValue.Item1 == "Primary " + ADefaultType)))
                {
                    AContainsOnlyDefaultType = false;

                    break;
                }
            }

            foreach (var SplittedValue in ASplitValues)
            {
                Postfix = String.Empty;

                if (!AContainsOnlyDefaultType)
                {
                    ReturnValue += SplittedValue.Item1 + ": ";
                }
                else
                {
                    Postfix = (SplittedValue.Item1 == "Primary " + ADefaultType) ?
                              " (" + "Primary " + ADefaultType + ")" : String.Empty;
                }

                ReturnValue += "'" + SplittedValue.Item2 + "'" + Postfix + "; ";
            }

            // Strip off last Separator
            ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 2);

            return ReturnValue;
        }

        private static string BuildTypeWithPrimaryPrefix(string AType, bool AIsPrimary)
        {
            return AIsPrimary ? "Primary " + AType : AType;
        }

        /// <summary>
        /// Returns the Country Code as set in the p_partner_location Row. (If that did not exist in p_location
        /// then this will be null. (This may be changed later in RemoveInternationalCodeFromTelephoneNumber.)
        /// </summary>
        /// <param name="APartnerLocationDR">PartnerLocation DataRow.</param>
        /// <returns>Country Code, or null if p_value_country_c is null.</returns>
        private static string GetCountryCode(DataRow APartnerLocationDR)
        {
            return (!APartnerLocationDR.IsNull("p_value_country_c")) ?
                   ((string)APartnerLocationDR["p_value_country_c"]) : null;
        }

        // This removes the international calling code from the start of a phone/fax/mobile number.
        // It also ensures that the p_partner_attribute country code is correct.
        private static string RemoveInternationalCodeFromTelephoneNumber(string ATelephoneNumber, ref string ACountryCode,
            string AAttributeType, Int64 APartnerKey, PPartnerAttributeRecord APPARecord)
        {
            const string CheckIntlAccessCodeWarningStr = "]: Please check International Access Code for {0} '{1}' for Partner {2}.";

            string ReturnValue = null;
            string TelephoneNumberOrig = String.Copy(ATelephoneNumber);
            string CountryCallingCodeString;
            DataRow CountryRow = FCountryTable.Rows.Find(ACountryCode);
            bool FSiteInternatAccessCodeIsANumber = true;

            // Sanity Check: Check if the country we get passed is in the p_country Table
            if (CountryRow == null)
            {
                // This should never happen, but it case it ever does:
                // Ensure we don't set a Country Code for the Telephone Number as it would be invalid and we would get an Exception
                // thrown when the p_partner_attribute Row would get inserted into the DB due to referential integrity checks.
                // (Telephone Number gets returned unchanged.)
                ACountryCode = null;

                TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_WARN + "-990" +
                    string.Format(Catalog.GetString(CheckIntlAccessCodeWarningStr) +
                        Catalog.GetString(" Country Code could not get interpreted hence the " +
                            "International Access Code could not be determined: it got set to nothing (Cont.Detail Record Current: {3})"),
                        AAttributeType, TelephoneNumberOrig, APartnerKey, APPARecord.Current));

                return TelephoneNumberOrig;
            }
            else
            {
                CountryCallingCodeString = CountryRow["p_internat_telephone_code_i"].ToString();
            }

            //
            // Preparations
            //

            // First strip off any leading whitespace characters (e.g. Space, Tab, etc) to ensure we can always detect a leading + or
            // FSiteInternatAccessCode, and any trailing whitespace characters in case we need to move any prefixing textual characters
            // to the end of the Telephone Number string
            ATelephoneNumber = ATelephoneNumber.Trim();

            Int64 Tmp1;

            if (!Int64.TryParse(FSiteInternatAccessCode, out Tmp1))
            {
                FSiteInternatAccessCodeIsANumber = false;
            }

            // Turn any textual prefixes into textual postfixes and clad them in parenthesis [e.g. 'xxx 01234 56789' becomes '01234 56789 (xxx)']
            MoveAnyTexualPrefixesToEndOfTelephoneNumber(ref ATelephoneNumber, FSiteInternatAccessCode,
                FSiteInternatAccessCodeIsANumber);

            //
            // Checks
            //

            // First check if Telephone Number starts with + and CountryCallingCode (e.g. '+44' for UK)
            // or with the FSiteInternatAccessCode and CountryCallingCode (e.g. '0044' for UK if the to-be-converted Site is Switzerland)
            if (!string.IsNullOrEmpty(CountryCallingCodeString))
            {
                int CountryCallingCode = Convert.ToInt32(CountryCallingCodeString);

                if (ATelephoneNumber.StartsWith("+" + CountryCallingCode, StringComparison.InvariantCulture))  // e.g. '+44' for UK
                {
                    ReturnValue = ATelephoneNumber.Remove(0, 1 + CountryCallingCode.ToString().Length);
                }
                else if (ATelephoneNumber.StartsWith(FSiteInternatAccessCode + CountryCallingCode,
                             StringComparison.InvariantCulture)) // e.g. '0044' for UK
                {
                    ReturnValue = ATelephoneNumber.Remove(0, FSiteInternatAccessCode.Length + CountryCallingCode.ToString().Length);
                }
            }

            // If no success so far...
            if (ReturnValue == null)
            {
                // Check if Telephone Number starts with + or with FSiteInternatAccessCode
                if (ATelephoneNumber.StartsWith("+", StringComparison.InvariantCulture)
                    || ATelephoneNumber.StartsWith(FSiteInternatAccessCode, StringComparison.InvariantCulture))
                {
                    bool Found = false;

                    // If number's country calling code does not match the country calling code corresponding to the country code in p_location record
                    // then find out if one of the p_internat_telephone_code_i in FCountryTable matches the country calling code.
                    // Country calling codes are either 2 or 3 digits and a 2 digit code is never contained in a 3 digit code.
                    foreach (DataRow Row in FCountryTable.Rows)
                    {
                        int n;
                        string InternatTelephoneCode = Row["p_internat_telephone_code_i"].ToString();

                        // Ignore country codes that are numbers
                        if (int.TryParse(Row["p_country_code_c"].ToString(), out n))
                        {
                            continue;
                        }

                        // Don't try to match against a Country that has got 0 for its Int'l Telephone Code as that is an invalid code
                        if (InternatTelephoneCode == "0")
                        {
                            continue;
                        }

                        if (ATelephoneNumber.StartsWith("+", StringComparison.InvariantCulture)
                            && ATelephoneNumber.Substring(1).StartsWith(InternatTelephoneCode,
                                StringComparison.InvariantCulture))
                        {
                            ACountryCode = Row["p_country_code_c"].ToString();
                            ReturnValue = ATelephoneNumber.Substring(1 + InternatTelephoneCode.Length);

                            DetectAndLogIfMulitpleCountriesHaveSameIntlAccessCode(Row, AAttributeType, APartnerKey,
                                TelephoneNumberOrig, Catalog.GetString(CheckIntlAccessCodeWarningStr), APPARecord);

                            DetectAndLogExtraDigitAfterInternatTelephoneCode(ATelephoneNumber, InternatTelephoneCode,
                                1, AAttributeType, APartnerKey, TelephoneNumberOrig, Catalog.GetString(CheckIntlAccessCodeWarningStr),
                                APPARecord);

                            Found = true;
                            break;
                        }
                        else if (ATelephoneNumber.StartsWith(FSiteInternatAccessCode, StringComparison.InvariantCulture)
                                 && ATelephoneNumber.Substring(FSiteInternatAccessCode.Length).StartsWith(
                                     InternatTelephoneCode, StringComparison.InvariantCulture))
                        {
                            ACountryCode = Row["p_country_code_c"].ToString();
                            ReturnValue = ATelephoneNumber.Substring(
                                FSiteInternatAccessCode.Length + InternatTelephoneCode.Length);

                            DetectAndLogIfMulitpleCountriesHaveSameIntlAccessCode(Row, AAttributeType, APartnerKey,
                                TelephoneNumberOrig, Catalog.GetString(CheckIntlAccessCodeWarningStr), APPARecord);

                            DetectAndLogExtraDigitAfterInternatTelephoneCode(ATelephoneNumber, InternatTelephoneCode,
                                FSiteInternatAccessCode.Length, AAttributeType, APartnerKey, TelephoneNumberOrig,
                                Catalog.GetString(CheckIntlAccessCodeWarningStr), APPARecord);

                            Found = true;
                            break;
                        }
                    }

                    if (!Found)
                    {
                        ACountryCode = null;
                        ReturnValue = TelephoneNumberOrig;

                        TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_CHECK + "-300" +
                            string.Format(Catalog.GetString(CheckIntlAccessCodeWarningStr) +
                                Catalog.GetString(" No country could be found for (what got recognised as) the International Access Code. " +
                                    "Because of that the International Access Code could not be determined: it got set to nothing. (Cont.Detail Record Current: {3})"),
                                AAttributeType, TelephoneNumberOrig, APartnerKey, APPARecord.Current));
                    }
                }
                else
                {
                    // Telephone number doesn't start with + or FSiteInternatAccessCode = no need to remove Int'l Telephone Code.
                    ReturnValue = ATelephoneNumber;

                    if (ACountryCode != FSiteCountryCode)
                    {
                        // If this is the case we issue a warning to the user as we can't be totally sure that the phone number is
                        // really inside the Country of the ACountryCode...
                        TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_CHECK + "-310" +
                            string.Format(Catalog.GetString(CheckIntlAccessCodeWarningStr) +
                                Catalog.GetString(" Telephone Number doesn't start with an International Access Code " +
                                    "and the Country of the Address that it is recorded against ({3}) isn't the Site's Country ({4}) - " +
                                    "please check whether the International Access Code that was assigned is indeed correct. (Cont.Detail Record Current: {5})"),
                                AAttributeType,
                                TelephoneNumberOrig, APartnerKey, ACountryCode, FSiteCountryCode, APPARecord.Current));

                        // TODO: Make hard-coded number '35' (first two digits of a PartnerKey for Switzerland) variable!
                        if (!APartnerKey.ToString().StartsWith("35", StringComparison.InvariantCulture))
                        {
                            if (APPARecord.Current)
                            {
                                FFWarnings310CollectorNonFieldCurrentPartnerKeys.Add(APartnerKey.ToString());
                            }
                            else
                            {
                                FFWarnings310CollectorNonFieldNonCurrentPartnerKeys.Add(APartnerKey.ToString());
                            }
                        }
                        else
                        {
                            if (APPARecord.Current)
                            {
                                FWarnings310CollectorFieldCurrentPartnerKeys.Add(APartnerKey.ToString());
                            }
                            else
                            {
                                FWarnings310CollectorFieldNonCurrentPartnerKeys.Add(APartnerKey.ToString());
                            }
                        }
                    }
                }
            }

            // Sanity Check: Did the algorithm of this method somehow fail to remove the Int'l Code from the Telephone Number?
            if (ReturnValue == null)
            {
                throw new Exception("RemoveInternationalCodeFromTelephoneNumber Method attempted to clear the Telephone Number " +
                    TelephoneNumberOrig + " due to a problem in the Methods' algorithm, but that must not happen");
            }

            return ReturnValue.Trim().Trim('-').Trim('/').Replace("\t", " ");  // The last statement replaces any <TAB> characters inside the string with a single space character each, see Bugs #4620, #4625!
        }

        private static void DetectAndLogExtraDigitAfterInternatTelephoneCode(string ATelephoneNumber, string AInternatTelephoneCode,
            int APrefixDenominatorLength, string AAttributeType, Int64 APartnerKey, string ATelephoneNumberOrig, string AGenericWarningString,
            PPartnerAttributeRecord APPARecord)
        {
            char ExtraDigitCheckChar = ATelephoneNumber[APrefixDenominatorLength + AInternatTelephoneCode.Length];

            if ((ExtraDigitCheckChar >= '0')
                && (ExtraDigitCheckChar <= '9'))
            {
                TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_CHECK + "-320" +
                    string.Format(Catalog.GetString(AGenericWarningString) +
                        Catalog.GetString(" What got recognised as the International Access Code +{3} is immediately followed by a number, {4}. " +
                            "Because of that we can't be totally sure of the International Access Code that we recognised: please check it manually. (Cont.Detail Record Current: {5})"),
                        AAttributeType, ATelephoneNumberOrig, APartnerKey, AInternatTelephoneCode, ExtraDigitCheckChar, APPARecord.Current));
            }
        }

        /// <summary>
        /// Turns any textual prefixes into textual postfixes and clads them in parenthesis [e.g. 'xxx 01234 56789' becomes '01234 56789 (xxx)'].
        /// </summary>
        /// <remarks>
        /// The algorithm first establishes where the Telephone Number starts 'for real' in the <paramref name="ATelephoneNumberStr" />
        /// by looking for:
        ///   * any digit (0-9);
        ///   * + sign;
        ///   * Code needed to dial *out* of the country of the Site that we are converting.
        /// It then takes everything that comes *before* that starting position, removes it from the start and appends it to the Telephone Number in
        /// the format of ' (___)' where '___' is what was the postfix. Any trailing colon ( : ) or space also gets removed from the postfix before
        /// it is appended.
        /// </remarks>
        /// <param name="ATelephoneNumberStr">Telephone Number string (this instance gets directly modified)!</param>
        /// <param name="AInternatAccessCodeOfCountryOfSite">Code needed to dial *out* of the country of the Site that we are converting (often this will be '00',
        /// but not always, and that code can be a text, not just numbers!).</param>
        /// <param name="ASiteInternatAccessCodeIsANumber">Pass true if <paramref name="AInternatAccessCodeOfCountryOfSite" /> is a number,
        /// otherwise false.</param>
        private static void MoveAnyTexualPrefixesToEndOfTelephoneNumber(ref string ATelephoneNumberStr,
            string AInternatAccessCodeOfCountryOfSite, bool ASiteInternatAccessCodeIsANumber)
        {
            string ATelephoneNumberStrOrig = String.Copy(ATelephoneNumberStr);
            string PrefixStr;
            string RemainingTelephoneStr;
            int CharCounter = 0;
            int CharCounterReverse;

            List <int>PositionsOfPlusSign = new List <int>();
            bool PlusPrefixUtilised = false;

            // Find anything that isn't the first digit ( 0..9 ) in the Telephone Number
            while ((CharCounter < ATelephoneNumberStr.Length)
                   && !(ATelephoneNumberStr[CharCounter] >= '0' && ATelephoneNumberStr[CharCounter] <= '9'))
            {
                CharCounter++;
            }

            if (CharCounter == 0)
            {
                // Telephone Number starts with a digit straight away --> we are done!
                return;
            }
            else if ((CharCounter == 1)
                     && (ATelephoneNumberStr[0] == '('))
            {
                // Telephone Number starts with '(' plus a digit straight away --> we are done as we are assuming a local number prefix then, such as (0)xxx
                return;
            }

            // Let our prefix string (for the moment) begin one character before the first digit
            PrefixStr = ATelephoneNumberStr.Substring(0, CharCounter);
            RemainingTelephoneStr = ATelephoneNumberStr.Substring(CharCounter);

            // Remove any whitespace from beginning and end of prefix
            PrefixStr = PrefixStr.Trim();

            //
            // Handle + sign prefix
            //

            // Should there be any occurrence of '++', replace it with a single plus sign to make for easier parsing.
            // We assume that ++ will *always* be a typo because...
            //   (1) ++ isn't a valid indicator for signalising an Int'l Telephone Code;
            //   (2) We can't think of any reason why users might have used ++ deliberately in a prefix string!
            PrefixStr = PrefixStr.Replace("++", "+");

            // Find plus sign (if it's there) starting from the end of the prefix string (and also spot potential multiple occurrences of it!)
            CharCounterReverse = PrefixStr.Length - 1;

            while (CharCounterReverse >= 0)
            {
                if (PrefixStr.Substring(CharCounterReverse, 1) == "+")
                {
                    PositionsOfPlusSign.Add(CharCounterReverse);
                }

                CharCounterReverse--;
            }

            if (PositionsOfPlusSign.Count == 0)
            {
                // No plus sign found in prefix string - prefix string doesn't need to be stripped of any plus sign
            }
            else if (PositionsOfPlusSign.Count == 1)
            {
                //
                // Single plus sign found in prefix string
                //

                if (PositionsOfPlusSign[0] == PrefixStr.Length - 1)
                {
                    // Telephone Number starts with + and a digit straight away:
                    // We let the Telephone Number start with + and anything that starts off with the first digit.
                    PrefixStr = PrefixStr.Substring(0, PrefixStr.Length - 1);
                    RemainingTelephoneStr = "+" + RemainingTelephoneStr;

                    PlusPrefixUtilised = true;
                }
            }
            else
            {
                //
                // Multiple plus signs found in the prefix string, starting from the end of the prefix string
                //

                // The Telephone Number contains a second (or more) plus sign(s) but they can't form the string '++' (as we already got rid of ++
                // earlier).
                // Ignore those as they will have a different meaning

                if (PositionsOfPlusSign[0] == PrefixStr.Length - 1)
                {
                    // Telephone Number starts with + and a digit straight away:
                    // We let the Telephone Number start with + and anything that starts off with the first digit.
                    // (Example: 'A+S: +01234 5678900')
                    PrefixStr = PrefixStr.Substring(0, PrefixStr.Length - 1);
                    RemainingTelephoneStr = "+" + RemainingTelephoneStr;

                    PlusPrefixUtilised = true;
                }
            }

            //
            // Handle AInternatAccessCodeOfCountryOfSite prefix
            //
            if (!PlusPrefixUtilised)
            {
                if (ASiteInternatAccessCodeIsANumber)  // e.g. '00'
                {
                    // Parsing will aleady be OK as AInternatAccessCodeOfCountryOfSite will be at the start of RemainingTelephoneStr
                    // (if it is indeed entered by for the Telephone Number)! Examples: '0049 1234 5678900', 'A+S: 0049 1234 5678900'
                }
                else                                  // e.g. '0~0' for Poland, '8~10' for Azerbaijan, '00~' for Algeria, ...
                {
                    throw new Exception("MoveAnyTexualPrefixesToEndOfTelephoneNumber: Can't yet handle conversion of Telephone Numbers " +
                        "for a Site that hasn't got a numeric Int' Access Code!");

                    // TODO: Implement checks for a non-numeric AInternatAccessCodeOfCountryOfSite prefixes!
                }
            }

            if (PrefixStr != String.Empty)
            {
                // Finally: Strip the prefix off the Telephone Number, remove any whitespace characters from the beginning and end
                // of the prefix and append it to the Telephone Number (removing any trailing colon ( : ) character, if there is one)
                ATelephoneNumberStr = RemainingTelephoneStr + " (" + PrefixStr.Trim().Trim(new char[] { ':' }) + ")";
            }
            else
            {
                ATelephoneNumberStr = RemainingTelephoneStr;
            }
        }

        private static void DetectAndLogIfMulitpleCountriesHaveSameIntlAccessCode(DataRow ACountryRow,
            string AAttributeType, Int64 APartnerKey, string ATelephoneNumberOrig, string AGenericWarningString,
            PPartnerAttributeRecord APPARecord)
        {
            const string MultipleMatchingCountriesWarningStr = " Multiple countries ({3}) have the same International Access Code {4}. " +
                                                               "Out of those, the country {5} (Country Code '{6}') got chosen.";

            string MultipleMatchingCoutries = MultipleCountriesMatchingSameIntlAccessCode(ACountryRow);

            if (MultipleMatchingCoutries != String.Empty)
            {
                TLogging.Log(CONT_DETAIL_MIGRATION_LOG_PREFIX + LOG_PREF_CHECK + "-330" +
                    string.Format(AGenericWarningString + Catalog.GetString(MultipleMatchingCountriesWarningStr) +
                        " (Cont.Detail Record Current: {7})",
                        AAttributeType, ATelephoneNumberOrig, APartnerKey, MultipleMatchingCoutries,
                        ACountryRow["p_internat_telephone_code_i"].ToString(),
                        ACountryRow["p_country_name_c"].ToString(), ACountryRow["p_country_code_c"].ToString(),
                        APPARecord.Current));
            }
        }

        private static string MultipleCountriesMatchingSameIntlAccessCode(DataRow ACountryRow)
        {
            string ReturnValue = String.Empty;

            DataRow[] MatchingCountyDataRows;

            MatchingCountyDataRows = FCountryTable.Select(
                "p_internat_telephone_code_i = '" + ACountryRow["p_internat_telephone_code_i"].ToString() + "'");

            if (MatchingCountyDataRows.Length > 1)
            {
                foreach (DataRow Row in MatchingCountyDataRows)
                {
                    ReturnValue += "'" + ACountryRow["p_country_code_c"].ToString() + "' - " + Row["p_country_name_c"].ToString() + "; ";
                }

                // Strip off trailing separator
                ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 2);
            }

            return ReturnValue;
        }

        private static PPartnerAttributeRecord GetNewPPartnerAttributeRecord(Int64 APartnerKey, DataRow APartnerLocationDR)
        {
            DateTime? EffectiveDate = null;
            DateTime? GoodUntilDate = null;
            DateTime? NoLongerCurrentFromDate = null;
            int Icon = ((int)APartnerLocationDR[PARTNERLOCATION_ICON_COLUMN]);  // determined by 'Ict.Petra.Shared.MPartner.Calculations.DeterminePartnerLocationsDateStatus'
            bool CurrentFlag = Icon == 1;                                       // 1 = 'Current Address'
            bool SpecialisedFlag = false;
            string CommentStr = String.Empty;

            if (!APartnerLocationDR.IsNull("p_date_effective_d"))
            {
                EffectiveDate = ((DateTime)APartnerLocationDR["p_date_effective_d"]);
            }

            if (!APartnerLocationDR.IsNull("p_date_good_until_d"))
            {
                GoodUntilDate = ((DateTime)APartnerLocationDR["p_date_good_until_d"]);
            }

//            TLogging.Log(String.Format("Effective and GoodUntil Dates for Partner {0}: {1} = {2}; {3} = {4}",
//                APartnerKey, "Effective", EffectiveDate ?? DateTime.MinValue, "GoodUntil", GoodUntilDate ?? DateTime.MinValue));

            if ((Icon == 3)                                                     // 'Expired Address'
                && (GoodUntilDate.HasValue))
            {
                NoLongerCurrentFromDate = GoodUntilDate.Value;
            }
            else if ((Icon == 2)                                               // 'Future Address'
                     && (EffectiveDate.HasValue))
            {
                CommentStr = Catalog.GetString(
                    String.Format("In Petra 2.x this Contact Detail was set to become effective on {0}. " +
                        "Please set this Contact Detail record to 'Current' on, or after, this date!",
                        StringHelper.DateToLocalizedString(EffectiveDate)));
            }

            if (((string)APartnerLocationDR["p_location_type_c"] == "BUSINESS")
                || ((string)APartnerLocationDR["p_location_type_c"] == "FIELD"))
            {
                SpecialisedFlag = true;
            }

            return new TPartnerContactDetails.PPartnerAttributeRecord() {
                       InsertionOrderPerPartner = FInsertionOrderPerPartner,
                       PartnerKey = APartnerKey,
                       Sequence = FSequenceNo++,
                       Comment = CommentStr,
                       Specialised = SpecialisedFlag,
                       Current = CurrentFlag,
                       Confidential = ((string)APartnerLocationDR["p_location_type_c"]).EndsWith(SECURITY_CAN_LOCATIONTYPE,
                           StringComparison.InvariantCulture),
                       NoLongerCurrentFrom = NoLongerCurrentFromDate,
                       ValueCountry = null
            };
        }

        #endregion
    }
}