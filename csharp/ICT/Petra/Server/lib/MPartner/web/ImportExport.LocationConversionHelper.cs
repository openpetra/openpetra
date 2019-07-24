//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK, timop
//
// Copyright 2004-2019 by OM International
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
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Server.MPartner.ImportExport
{
    /// <summary>
    /// Helper Class that aids in the conversion of certain p_partner_location data columns' content into Contact Details.
    /// </summary>
    public class TPartnerContactDetails_LocationConversionHelper
    {
        /// <summary>
        /// Func Delegate for loading of PPartnerAttribute data from the DB using a Template Record.
        /// </summary>
        public Func <PPartnerAttributeRow, StringCollection, TDBTransaction,
                            PPartnerAttributeTable>PartnerAttributeLoadUsingTemplate;

        /// <summary>
        /// Creates a PPartnerAttribute Record out of data that is held in a data structure that is a p_partner_attribute
        /// representation.
        /// </summary>
        /// <param name="APPARec">Data structure that is p_partner_attribute representation.</param>
        /// <param name="AMainDS">DataSet to which the PPartnerAttribute Record should be added to.</param>
        /// <param name="ADataBase"></param>
        public void CreatePartnerContactDetailRecord(PPartnerAttributeRecord APPARec,
            DataSet AMainDS, TDataBase ADataBase = null)
        {
            var MainDS = (PartnerImportExportTDS)AMainDS;

            PPartnerAttributeRow PartnerAttributeDR = MainDS.PPartnerAttribute.NewRowTyped(false);

            TDataBase db = DBAccess.Connect("CreatePartnerContactDetailRecord", ADataBase);
            TDBTransaction Transaction = new TDBTransaction();

            PartnerAttributeDR.PartnerKey = APPARec.PartnerKey;
    
            bool SubmitOK = false;
            db.WriteTransaction(ref Transaction,
                ref SubmitOK,
                delegate
                {
                    PartnerAttributeDR.Sequence = Convert.ToInt32(db.GetNextSequenceValue(TSequenceNames.seq_partner_attribute_index.ToString(), Transaction));
                    SubmitOK = true;
                });

            PartnerAttributeDR.AttributeType = APPARec.AttributeType;
            PartnerAttributeDR.Index = APPARec.Index;
            PartnerAttributeDR.Value = APPARec.Value;
            PartnerAttributeDR.ValueCountry = APPARec.ValueCountry;
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
        public void AddOldDBTableColumnsToPartnerLocation(DataTable APartnerLocationDT)
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
        public bool ExistingPartnerAttributes(PPartnerAttributeRow AImportedPartnerAttribRow,
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
        public void TakeExistingPartnerAttributeRecordAndModifyIt(PPartnerAttributeRow AImportedPartnerAttributeDR,
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

            /// <summary>
            /// CreatedByUser Column data.
            /// </summary>
            public String CreatedByUser
            {
                get; set;
            }

            /// <summary>
            /// CreatedDate Column data.
            /// </summary>
            public DateTime ? DateCreated
            {
                get; set;
            }

            /// <summary>
            /// ModifiedByUser Column data.
            /// </summary>
            public String ModifiedByUser
            {
                get; set;
            }

            /// <summary>
            /// ModifiedDate Column data.
            /// </summary>
            public DateTime ? DateModified
            {
                get; set;
            }
        }
    }
}
