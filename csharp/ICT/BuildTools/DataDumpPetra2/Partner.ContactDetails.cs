//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK
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
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Specialized;

using Ict.Common;
using Ict.Petra.Shared.MPartner.Conversion;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// This class generates Partner Contact Detail records (=p_partner_attribute table records)
    /// out of Petra 2.x p_partner_location records. It utilises the shared
    /// 'Ict.Petra.Shared.MPartner.Conversion.TPartnerContactDetails' Class for most of its work.
    /// </summary>
    public class TPartnerContactDetails_DataDump : TFixData
    {
        private static StringCollection FColumnNames;
        private static string[] FNewRow;
        private static StreamWriter FWriter;
        private static StreamWriter FWriterTest;

        static TPartnerContactDetails_DataDump()
        {
            TPartnerContactDetails.CreateContactDetailsRow = WriteOutContactDetails;
        }

        /// <summary>
        /// Initialises the population of PPartnerAttributes.
        /// </summary>
        /// <param name="AColumnNames"></param>
        /// <param name="ANewRow"></param>
        /// <param name="AWriter"></param>
        /// <param name="AWriterTest"></param>
        public static void PopulatePPartnerAttribute_Init(StringCollection AColumnNames,
            ref string[] ANewRow,
            StreamWriter AWriter,
            StreamWriter AWriterTest)
        {
            FColumnNames = AColumnNames;
            FNewRow = ANewRow;
            FWriter = AWriter;
            FWriterTest = AWriterTest;

            // default for all new records
            SetValue(AColumnNames, ref FNewRow, "s_date_created_d", "\\N");
            SetValue(AColumnNames, ref FNewRow, "s_created_by_c", "\\N");
            SetValue(AColumnNames, ref FNewRow, "s_date_modified_d", "\\N");
            SetValue(AColumnNames, ref FNewRow, "s_modified_by_c", "\\N");
            SetValue(AColumnNames, ref FNewRow, "s_modification_id_t", "\\N");
        }

        /// <summary>
        /// Kicks off the population of PPartnerAttributes.
        /// </summary>
        /// /// <returns>Number of created PPartnerAttribute Rows.</returns>
        public static int PopulatePPartnerAttribute()
        {
            TPartnerContactDetails.EmptyStringIndicator = @"\N";
            return TPartnerContactDetails.PopulatePPartnerAttribute();
        }

        /// <summary>
        /// Writes out a PPartnerAttribute Record out of data that is held in a data structure that is a p_parnter_attribute
        /// representation.
        /// </summary>
        /// <param name="APPARec">Data structure that is p_parnter_attribute representation.</param>
        /// <param name="ANotUsed">IGNORED - Pass in null.</param>
        public static void WriteOutContactDetails(TPartnerContactDetails.PPartnerAttributeRecord APPARec, DataSet ANotUsed)
        {
            SetValue(FColumnNames, ref FNewRow, "p_partner_key_n", APPARec.PartnerKey.ToString());
            SetValue(FColumnNames, ref FNewRow, "p_sequence_i", APPARec.Sequence.ToString());
            SetValue(FColumnNames, ref FNewRow, "p_attribute_type_c", APPARec.AttributeType);
            SetValue(FColumnNames, ref FNewRow, "p_index_i", APPARec.Index.ToString());
            SetValue(FColumnNames, ref FNewRow, "p_value_c", APPARec.Value.Replace(";", @"\;"));
            SetValue(FColumnNames, ref FNewRow, "p_comment_c", APPARec.Comment != String.Empty ? APPARec.Comment : "\\N");
            SetValue(FColumnNames, ref FNewRow, "p_primary_l", APPARec.Primary ? "1" : "0");
            SetValue(FColumnNames, ref FNewRow, "p_within_organsiation_l", APPARec.WithinOrganisation ? "1" : "0");
            SetValue(FColumnNames, ref FNewRow, "p_specialised_l", APPARec.Specialised ? "1" : "0");
            SetValue(FColumnNames, ref FNewRow, "p_confidential_l", APPARec.Confidential ? "1" : "0");
            SetValue(FColumnNames, ref FNewRow, "p_current_l", APPARec.Current ? "1" : "0");
            SetValue(FColumnNames, ref FNewRow, "p_no_longer_current_from_d",
                APPARec.NoLongerCurrentFrom.HasValue ? APPARec.NoLongerCurrentFrom.Value.ToString("yyyy-dd-mm") : "\\N");

            FWriter.WriteLine(StringHelper.StrMerge(FNewRow, '\t').Replace("\\\\N", "\\N").ToString());

            if (FWriterTest != null)
            {
                FWriterTest.WriteLine("BEGIN; " + "COPY p_partner_attribute FROM stdin;");
                FWriterTest.WriteLine(StringHelper.StrMerge(FNewRow, '\t').Replace("\\\\N", "\\N").ToString());
                FWriterTest.WriteLine("\\.");
                FWriterTest.WriteLine("ROLLBACK;");
            }
        }
    }
}