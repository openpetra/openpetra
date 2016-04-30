//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK, timop
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
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Specialized;

using Ict.Common;
using Ict.Common.Exceptions;

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

        /// <summary>
        /// Holds the p_partner.p_partner_class_c information of each Partner.
        /// </summary>
        private static Dictionary <Int64, string>FPartnerClassInformation = new Dictionary <Int64, string>();

        /// <summary>
        /// Holds the p_partner_location records of each Partner.
        /// </summary>
        private static SortedList <long, DataTable>FPartnerLocationRecords = null;

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

                    for (int Counter = 0; Counter < SortedRecordsDV.Count; Counter++)
                    {
                        List <PPartnerAttributeRecord>PPARecordsSingleLocation =
                            ConstructPPartnerAttributeRecords(PartnerKey, SortedRecordsDV[Counter].Row);

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

            return RowCounter;
        }

        #endregion

        #region Private Methods

        private static List <PPartnerAttributeRecord>ConstructPPartnerAttributeRecords(Int64 APartnerKey, DataRow APartnerLocationDR)
        {
            var ReturnValue = new List <TPartnerContactDetails.PPartnerAttributeRecord>();
            var PPARecordList = new List <PPartnerAttributeRecord>();
            PPartnerAttributeRecord PPARecord;
            bool IsBestAddr = BestAddressHelper.IsBestAddressPartnerLocationRecord(APartnerLocationDR);
            bool AnyTelephoneNumberSetAsPrimary = false;
            // Variables that hold record information
            string TelephoneNumberString = (string)APartnerLocationDR["p_telephone_number_c"];
            string FaxNumberString = (string)APartnerLocationDR["p_fax_number_c"];
            string MobileNumberString = (string)APartnerLocationDR["p_mobile_number_c"];
            string AlternatePhoneNumberString = (string)APartnerLocationDR["p_alternate_telephone_c"];
            string UrlString = (string)APartnerLocationDR["p_url_c"];
            string EmailAddressString = (string)APartnerLocationDR["p_email_address_c"];
            string PartnerClass;

            FInsertionOrderPerPartner++;

            //
            // Work on the various p_partner_location DataColumns that hold data and that will each be migrated to
            // a Contact Detail record - if they hold data.
            //

            if ((TelephoneNumberString != FEmptyStringIndicator)
                || (EmailAddressString != FEmptyStringIndicator))
            {
                // Set data parts that depend on certain conditions
                if (TelephoneNumberString != FEmptyStringIndicator)
                {
                    // There could be a number of phone numbers seperated by a semi colon. We need to split these up and add them seperately.
                    string[] TelephoneNumbers = TelephoneNumberString.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < TelephoneNumbers.Length; i++)
                    {
                        string TelephoneNumber = TelephoneNumbers[i].Trim();

                        PPARecord = GetNewPPartnerAttributeRecord(APartnerKey, APartnerLocationDR);

                        PPARecord.Value = TelephoneNumber;
                        PPARecord.AttributeType = ATTR_TYPE_PHONE;

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

                        PPARecordList.Add(PPARecord);
                    }
                }

                if (EmailAddressString != FEmptyStringIndicator)
                {
                    // Do not yet split up email addresses that could seperated by a semi colon!
                    PPARecord = GetNewPPartnerAttributeRecord(APartnerKey, APartnerLocationDR);

                    PPARecord.Value = EmailAddressString;
                    PPARecord.AttributeType = ATTR_TYPE_EMAIL;

                    if ((IsBestAddr)
                        && (PPARecord.Current))
                    {
                        // Mark this Contact Detail as being 'Primary' - but only if the Contact Detail is current!
                        PPARecord.Primary = true;

                        // Mark this Contact Detail as being 'WithinOrganisation' as it has an 'organisation-internal' e-mail-address!
                        // - but only if the Partner is a PERSON!
                        if (FPartnerClassInformation.TryGetValue(APartnerKey, out PartnerClass))
                        {
                            if (PartnerClass == "PERSON")
                            {
                                if (EmailAddressString.EndsWith("@om.org", StringComparison.InvariantCulture))
                                {
                                    PPARecord.WithinOrganisation = true;

                                    //                                    TLogging.Log(String.Format(
                                    //                                            "Made email address '{0}' a 'WithinOrganisation' e-mail address (PartnerKey: {1}, LocationKey: {2})",
                                    //                                            EmailAddress, APartnerKey, APartnerLocationDR["p_location_key_i"]));
                                }
                            }
                        }
                    }

                    PPARecordList.Add(PPARecord);
                }
            }

            if (FaxNumberString != FEmptyStringIndicator)
            {
                // There could be a number of fax numbers seperated by a semi colon. We need to split these up and add them seperately.
                string[] FaxNumbers = FaxNumberString.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < FaxNumbers.Length; i++)
                {
                    string FaxNumber = FaxNumbers[i].Trim();

                    PPARecord = GetNewPPartnerAttributeRecord(APartnerKey, APartnerLocationDR);
                    // TODO_LOW - PERHAPS: check if the Value is an email address and in case it is, record it as an e-mail address instead of this Attribute Type! [would need to use TStringChecks.ValidateEmail(xxxx, true)]
                    PPARecord.Value = FaxNumber;
                    PPARecord.AttributeType = ATTR_TYPE_FAX;

                    PPARecordList.Add(PPARecord);
                }
            }

            if (MobileNumberString != FEmptyStringIndicator)
            {
                // There could be a number of mobile numbers seperated by a semi colon. We need to split these up and add them seperately.
                string[] MobileNumbers = MobileNumberString.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < MobileNumbers.Length; i++)
                {
                    string MobileNumber = MobileNumbers[i].Trim();

                    PPARecord = GetNewPPartnerAttributeRecord(APartnerKey, APartnerLocationDR);
                    // TODO_LOW - PERHAPS: check if the Value is an email address and in case it is, record it as an e-mail address instead of this Attribute Type! [would need to use TStringChecks.ValidateEmail(xxxx, true)]
                    PPARecord.Value = MobileNumber;
                    PPARecord.AttributeType = ATTR_TYPE_MOBILE_PHONE;

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

                    PPARecordList.Add(PPARecord);
                }
            }

            if (AlternatePhoneNumberString != FEmptyStringIndicator)
            {
                // There could be a number of mobile numbers seperated by a semi colon. We need to split these up and add them seperately.
                string[] AlternatePhoneNumbers = AlternatePhoneNumberString.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < AlternatePhoneNumbers.Length; i++)
                {
                    string AlternatePhoneNumber = AlternatePhoneNumbers[i].Trim();

                    PPARecord = GetNewPPartnerAttributeRecord(APartnerKey, APartnerLocationDR);
                    // TODO_LOW - PERHAPS: check if the Value is an email address and in case it is, record it as an e-mail address instead of this Attribute Type! [would need to use TStringChecks.ValidateEmail(xxxx, true)]
                    PPARecord.Value = AlternatePhoneNumber;
                    PPARecord.AttributeType = ATTR_TYPE_PHONE;

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

                    PPARecordList.Add(PPARecord);
                }
            }

            if (UrlString != FEmptyStringIndicator)
            {
                // There could be a number of urls seperated by a semi colon. We need to split these up and add them seperately.
                string[] Urls = UrlString.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < Urls.Length; i++)
                {
                    string Url = Urls[i].Trim();

                    PPARecord = GetNewPPartnerAttributeRecord(APartnerKey, APartnerLocationDR);
                    // TODO_LOW - PERHAPS: check if the Value is an email address and in case it is, record it as an e-mail address instead of this Attribute Type! [would need to use TStringChecks.ValidateEmail(xxxx, true)]
                    PPARecord.Value = Url;
                    PPARecord.AttributeType = ATTR_TYPE_WEBSITE;

                    PPARecordList.Add(PPARecord);
                }
            }

            // Now add all created records to the ReturnValue
            foreach (var PPARec in PPARecordList)
            {
                ReturnValue.Add(PPARec);
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
                        "Please set this Contact Detail record to 'Valid' on, or after, this date!",
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
                       NoLongerCurrentFrom = NoLongerCurrentFromDate
            };
        }

        #endregion
    }
}