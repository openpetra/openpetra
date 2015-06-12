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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Conversion;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Server.MPartner.ImportExport
{
    /// <summary>
    /// Helper Class that aids in the conversion of certain p_partner_location data columns' content into Contact Details.
    /// </summary>
    public static class TPartnerContactDetails_LocationConversionHelper
    {
        /// <summary>
        /// Func Delegate for the getting of a Sequence value from the DB.
        /// </summary>
        public static Func <TSequenceNames, Int64>SequenceGetter;

        /// <summary>
        /// Func Delegate for loading of PPartnerAttribute data from the DB using a Template Record.
        /// </summary>
        public static Func <PPartnerAttributeRow, StringCollection, TDBTransaction,
                            PPartnerAttributeTable>PartnerAttributeLoadUsingTemplate;

        /// <summary>
        /// Parses certain p_partner_location data columns' content into a data structure that is p_parnter_attribute
        /// representation.
        /// </summary>
        /// <remarks>Similar to code found in \csharp\ICT\BuildTools\DataDumpPetra2\FixData.cs, Method 'FixData',
        /// in the code section that starts with the comment 'Process p_partner_location records and migrate certain values
        /// of p_partner_location records to 'Contact Detail' records'.</remarks>
        /// <param name="AMainDS">Typed DataSet that holds the p_partner_location records that are to be parsed.</param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        public static void ParsePartnerLocationsForContactDetails(PartnerImportExportTDS AMainDS, TDBTransaction ATransaction)
        {
            DataTable PartnerLocationsDT;
            DataRow NewPartnerLocationDR;
            string TelephoneNumber = String.Empty;
            string FaxNumber = String.Empty;
            string PhoneExtension;
            string FaxExtension;

            // collect the partner classes
            foreach (PPartnerRow PartnerDR in AMainDS.PPartner.Rows)
            {
                TPartnerContactDetails.PartnerClassInformation[PartnerDR.PartnerKey] = PartnerDR.PartnerClass;
            }

            SortedList <long, DataTable>PartnerLocationsTables = new SortedList <long, DataTable>();

            for (int counter = 0; counter < TPartnerContactDetails.NumberOfTables; counter++)
            {
                PartnerLocationsTables[counter] = TPartnerContactDetails.BestAddressHelper.GetNewPPartnerLocationTableInstance();
            }

            TPartnerContactDetails.PartnerLocationRecords = PartnerLocationsTables;

            foreach (PPartnerLocationRow PartnerLocationDR in AMainDS.PPartnerLocation.Rows)
            {
                PartnerLocationsDT = PartnerLocationsTables[Math.Abs(PartnerLocationDR.PartnerKey) % TPartnerContactDetails.NumberOfTables];

                // Phone Extension: Ignore if value in the dumped data is either null or 0
                if (PartnerLocationDR.IsExtensionNull())
                {
                    PhoneExtension = String.Empty;
                }

                PhoneExtension = PartnerLocationDR.Extension.ToString();

                if (PhoneExtension == "0")
                {
                    PhoneExtension = String.Empty;
                }

                // Fax Extension: Ignore if value in the dumped data is either null or 0
                if (PartnerLocationDR.IsFaxExtensionNull())
                {
                    FaxExtension = String.Empty;
                }

                FaxExtension = PartnerLocationDR.FaxExtension.ToString();

                if (FaxExtension == "0")
                {
                    FaxExtension = String.Empty;
                }

                if (!PartnerLocationDR.IsTelephoneNumberNull())
                {
                    // Concatenate Phone Number and Phone Extension ONLY if both of them aren't null and Phone Extension isn't 0 either.
                    TelephoneNumber = PartnerLocationDR.TelephoneNumber + PhoneExtension;
                }

                if (!PartnerLocationDR.IsFaxNumberNull())
                {
                    // Concatenate Fax Number and Fax Extension ONLY if both of them aren't null and Fax Extension isn't 0 either.
                    FaxNumber = PartnerLocationDR.FaxNumber + FaxExtension;
                }

                // Create representation of key data of the p_partner_location row and add it to the TPartnerContactDetails.PartnerLocationRecords Data Structure
                NewPartnerLocationDR = PartnerLocationsDT.NewRow();
                NewPartnerLocationDR["p_partner_key_n"] = PartnerLocationDR.PartnerKey;
                NewPartnerLocationDR["p_site_key_n"] = PartnerLocationDR.SiteKey;
                NewPartnerLocationDR["p_location_key_i"] = PartnerLocationDR.LocationKey;

                if (!PartnerLocationDR.IsDateEffectiveNull())
                {
                    NewPartnerLocationDR["p_date_effective_d"] = PartnerLocationDR.DateEffective;
                }
                else
                {
                    PartnerLocationDR.SetDateEffectiveNull();
                }

                if (!PartnerLocationDR.IsDateGoodUntilNull())
                {
                    NewPartnerLocationDR["p_date_good_until_d"] = PartnerLocationDR.DateGoodUntil;
                }
                else
                {
                    PartnerLocationDR.SetDateGoodUntilNull();
                }

                NewPartnerLocationDR["p_location_type_c"] = PartnerLocationDR.LocationType;
                NewPartnerLocationDR["p_send_mail_l"] = PartnerLocationDR.SendMail;
                NewPartnerLocationDR["p_telephone_number_c"] = TelephoneNumber;
                NewPartnerLocationDR["p_fax_number_c"] = FaxNumber;
                NewPartnerLocationDR["p_mobile_number_c"] = PartnerLocationDR.MobileNumber;
                NewPartnerLocationDR["p_alternate_telephone_c"] = PartnerLocationDR.AlternateTelephone;
                NewPartnerLocationDR["p_email_address_c"] = PartnerLocationDR.EmailAddress;
                NewPartnerLocationDR["p_url_c"] = PartnerLocationDR.Url;

                PartnerLocationsDT.Rows.Add(NewPartnerLocationDR);
            }

            TPartnerContactDetails.CreateContactDetailsRow = CreatePartnerContactDetailRecord;
            TPartnerContactDetails.EmptyStringIndicator = String.Empty;
            TPartnerContactDetails.PartnerAttributeHoldingDataSet = AMainDS;
            TPartnerContactDetails.PopulatePPartnerAttribute();

            Ict.Petra.Shared.MPartner.Calculations.DeterminePartnerContactDetailAttributes(AMainDS.PPartnerAttribute);
        }

        /// <summary>
        /// Creates a PPartnerAttribute Record out of data that is held in a data structure that is a p_parnter_attribute
        /// representation.
        /// </summary>
        /// <param name="APPARec">Data structure that is p_parnter_attribute representation.</param>
        /// <param name="AMainDS">DataSet to which the PPartnerAttribute Record should be added to.</param>
        public static void CreatePartnerContactDetailRecord(TPartnerContactDetails.PPartnerAttributeRecord APPARec,
            DataSet AMainDS)
        {
            var MainDS = (PartnerImportExportTDS)AMainDS;

            PPartnerAttributeRow PartnerAttributeDR = MainDS.PPartnerAttribute.NewRowTyped(false);

            PartnerAttributeDR.PartnerKey = APPARec.PartnerKey;
            PartnerAttributeDR.Sequence = (Int32)SequenceGetter(TSequenceNames.seq_partner_attribute_index);
            PartnerAttributeDR.AttributeType = APPARec.AttributeType;
            PartnerAttributeDR.Index = APPARec.Index;
            PartnerAttributeDR.Value = APPARec.Value;
            PartnerAttributeDR.Comment = APPARec.Comment;
            PartnerAttributeDR.Primary = APPARec.Primary;
            PartnerAttributeDR.WithinOrganisation = APPARec.WithinOrganisation;
            PartnerAttributeDR.Specialised = APPARec.Specialised;
            PartnerAttributeDR.Confidential = APPARec.Confidential;
            PartnerAttributeDR.Current = APPARec.Current;
            PartnerAttributeDR.NoLongerCurrentFrom = APPARec.NoLongerCurrentFrom;

            MainDS.PPartnerAttribute.Rows.Add(PartnerAttributeDR);
        }

        /// <summary>
        /// This adds DataColumns to the p_partner_location Table that will no longer be defined in the DB Schema
        /// once the Contact Details schema has been implemented in full. These Columns are needed because Petra 2.x's DB
        /// Scheme has them and we need to be able to store that data (temporarily only - for conversion into
        /// p_partner_attribute records).
        /// </summary>
        /// <param name="APartnerLocationDT">An instance of the p_partner_location DataTable.</param>
        public static void AddOldDBTableColumnsToPartnerLocation(DataTable APartnerLocationDT)
        {
            if (!APartnerLocationDT.Columns.Contains("p_telephone_number_c"))
            {
                APartnerLocationDT.Columns.Add(new System.Data.DataColumn("p_telephone_number_c", typeof(string)));
                APartnerLocationDT.Columns.Add(new System.Data.DataColumn("p_fax_number_c", typeof(string)));
                APartnerLocationDT.Columns.Add(new System.Data.DataColumn("p_extension_i", typeof(int)));
                APartnerLocationDT.Columns.Add(new System.Data.DataColumn("p_fax_extension_i", typeof(int)));
                APartnerLocationDT.Columns.Add(new System.Data.DataColumn("p_mobile_number_c", typeof(string)));
                APartnerLocationDT.Columns.Add(new System.Data.DataColumn("p_email_address_c", typeof(string)));
            }
        }

        /// <summary>
        /// Checks whether Partner Attributes of the same Attribute Type exist in the database.
        /// </summary>
        /// <param name="AImportedPartnerAttribRow"><see cref="PPartnerAttributeRow" /> that holds the imported data.</param>
        /// <param name="AFoundPartnerAttribDR"><see cref="PPartnerAttributeRow" /> that holds the record that was found -
        /// if one was found, otherwise this is null.</param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        /// <returns>False if the row should be imported as found in the import file, otherwise true.</returns>
        public static bool ExistingPartnerAttributes(PPartnerAttributeRow AImportedPartnerAttribRow,
            out PPartnerAttributeRow AFoundPartnerAttribDR, TDBTransaction ATransaction)
        {
            PPartnerAttributeTable ExistingPartnerAttributeDT;

            AFoundPartnerAttribDR = null;

            // Find existing Partner Attribute(s) of the same AttributeType (ignoring sequence!)
            PPartnerAttributeRow TmpPartnerAttributeRow = new PPartnerAttributeTable().NewRowTyped(false);
            TmpPartnerAttributeRow.PartnerKey = AImportedPartnerAttribRow.PartnerKey;
            TmpPartnerAttributeRow.AttributeType = AImportedPartnerAttribRow.AttributeType;

            ExistingPartnerAttributeDT = PartnerAttributeLoadUsingTemplate(TmpPartnerAttributeRow, null, ATransaction);

            if (ExistingPartnerAttributeDT.Count == 0)
            {
                // No existing Partner Attribute(s) of the same AttributeType -->
                // we want to import the row as found in the import file!
                return false;
            }

            // Existing Partner Attribute(s) of the same AttributeType --> Check to see whether we want to import
            // that Row, or whether we want to overwrite an existing Row with some imported data!
            for (int Counter = 0; Counter < ExistingPartnerAttributeDT.Rows.Count; Counter++)
            {
                if (ExistingPartnerAttributeDT[Counter].Value == AImportedPartnerAttribRow.Value)
                {
                    // This *is* the same Partner Attribute!
                    AFoundPartnerAttribDR = ExistingPartnerAttributeDT[Counter];

                    break;
                }
                else if (AImportedPartnerAttribRow.AttributeType == Ict.Petra.Shared.MPartner.Calculations.ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD)
                {
                    // This *is* the same Partner Attribute!
                    AFoundPartnerAttribDR = ExistingPartnerAttributeDT[Counter];

                    break;
                }
            }

            if (AFoundPartnerAttribDR != null)
            {
                // Same Partner Attribute --> overwrite the Row that is existing in the DB with some imported data!
                return true;
            }

            // No existing Partner Attribute(s) of the same AttributeType with the same Value -->
            // we want to import the row as found in the import file!
            return false;
        }

        /// <summary>
        /// Takes an PPartnerAttribute Row from the DB and modifies it with data from a PPartnerAttribute Row that got
        /// populated from imported data.
        /// </summary>
        /// <param name="AImportedPartnerAttributeDR">PPartnerAttribute Row that got populated from imported data.</param>
        /// <param name="AExistingPartnerAttributeDR">PPartnerAttribute Row from the DB.</param>
        public static void TakeExistingPartnerAttributeRecordAndModifyIt(PPartnerAttributeRow AImportedPartnerAttributeDR,
            PPartnerAttributeRow AExistingPartnerAttributeDR)
        {
            // First step: Store some details of the to-be-imported record in Variables
            var ImportDRValue = AImportedPartnerAttributeDR.Value;  // only needed for Attribute Type 'PARTNERS_PRIMARY_CONTACT_METHOD'
            var ImportDRComment = AImportedPartnerAttributeDR.Comment;
            var ImportDRPrimary = AImportedPartnerAttributeDR.Primary;
            var ImportDRWithinOrganisation = AImportedPartnerAttributeDR.WithinOrganisation;
            var ImportDRSpecialised = AImportedPartnerAttributeDR.Specialised;
            var ImportDRConfidential = AImportedPartnerAttributeDR.Confidential;
            var ImportDRCurrent = AImportedPartnerAttributeDR.Current;
            var ImportDRNoLongerCurrentFrom = AImportedPartnerAttributeDR.NoLongerCurrentFrom;

            // Second step: Copy all data from the record as it exists in the DB over into the to-be-imported record.
            // This is necessary so that the overwriting of some details of the record (see below) creates DataColumns
            // whose 'Modified' data is different than their 'Current' data (otherwise an 'empty' UPDATE command will
            // be issued against the DB!)
            DataUtilities.CopyAllColumnValues(AExistingPartnerAttributeDR, AImportedPartnerAttributeDR);

            // Make sure the Imported DataRows' DataRowState is no longer 'Added' as we don't want to add it to the
            // database, but want to update the existing row in the database!
            // (All the DataColumns hold 'Current' Data after this!)
            AImportedPartnerAttributeDR.AcceptChanges();

            // Third step: Overwrite some details of the record as it exists in the DB with details of the
            // to-be-imported record. (All these DataColumns will hold 'Modified' Data after this!)
            AImportedPartnerAttributeDR.Value = ImportDRValue;  // only needed for Attribute Type 'PARTNERS_PRIMARY_CONTACT_METHOD'
            AImportedPartnerAttributeDR.Comment = ImportDRComment;
            AImportedPartnerAttributeDR.Primary = ImportDRPrimary;
            AImportedPartnerAttributeDR.WithinOrganisation = ImportDRWithinOrganisation;
            AImportedPartnerAttributeDR.Specialised = ImportDRSpecialised;
            AImportedPartnerAttributeDR.Confidential = ImportDRConfidential;
            AImportedPartnerAttributeDR.Current = ImportDRCurrent;
            AImportedPartnerAttributeDR.NoLongerCurrentFrom = ImportDRNoLongerCurrentFrom;

            // Note: The DataRows' DataRowState is now 'Modified', and that causes the DB Access layer to issue an
            // UPDATE statement later. That statement will update data in each of the DB Tables' columns where a
            // DataColumn holds different data in the 'Current' and 'Modified' versions of a DataColum, i.e. where
            // there is a real difference between the to-be-imported data and the data in the record that is already
            // in the DB.
        }
    }
}