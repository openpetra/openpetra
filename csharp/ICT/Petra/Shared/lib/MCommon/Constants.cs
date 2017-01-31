//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2010 by OM International
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

using Ict.Common;

namespace Ict.Petra.Shared.MCommon
{
    /// <summary>
    /// this class defines some data types that can be used for Office Specific Data Labels
    /// </summary>
    public class MCommonConstants
    {
        /// <summary>Cacheable DataTables: Isolation Level used when reading them into memory</summary>
        public const System.Data.IsolationLevel CACHEABLEDT_ISOLATIONLEVEL = IsolationLevel.ReadCommitted;

        #region Importing

        /// <summary>todoComment</summary>
        public const String USERDEFAULT_IMPORTEDDATESMAYBEINTEGERS = "ImportedDatesMayBeIntegers";

        /// <summary>'Import Information'</summary>
        public static readonly string StrImportInformation = Catalog.GetString("Import Information");

        /// <summary>'An exception occurred while parsing line {0}'</summary>
        public static readonly string StrExceptionWhileParsingLine = Catalog.GetString("An exception occurred while parsing line {0}");

        /// <summary>'An exception occurred while saving the transactions'</summary>
        public static readonly string StrExceptionWhileSavingTransactions = Catalog.GetString("An exception occurred while saving the transactions");

        /// <summary>'An exception occurred while saving the batch number {0}'</summary>
        public static readonly string StrExceptionWhileSavingBatch = Catalog.GetString(
            "An exception occurred while saving the batch with description: '{0}'");

        /// <summary>'DuplicateKey exception while saving a batch' - part 1</summary>
        public static readonly string StrDuplicateKeyExceptionWhileSavingBatch1 = Catalog.GetString(
            "The data could not be saved because the Batch Number that was available at the start of the import had already been used.  ");

        /// <summary>'DuplicateKey exception while saving a batch' - part 2</summary>
        public static readonly string StrDuplicateKeyExceptionWhileSavingBatch2 = Catalog.GetString(
            "This could be because another user was importing batches at the same time as you.  ");

        /// <summary>'DuplicateKey exception while saving a batch' - part 3</summary>
        public static readonly string StrDuplicateKeyExceptionWhileSavingBatch3 = Catalog.GetString(
            "Please try to import the same file again, because another attempt may well be successful.");

        /// <summary>'Parsing error in Line {0}'</summary>
        public static readonly string StrParsingErrorInLine = Catalog.GetString("Parsing error in Line {0}");

        /// <summary>'Parsing error in line {0} - column '{1}''</summary>
        public static readonly string StrParsingErrorInLineColumn = Catalog.GetString("Parsing error in line {0} - column '{1}'");

        /// <summary>'Import information for Line {0}'</summary>
        public static readonly string StrImportInformationForLine = Catalog.GetString("Import information for Line {0}");

        /// <summary>'Import validation warning in Line {0}'</summary>
        public static readonly string StrImportValidationWarningInLine = Catalog.GetString("Import validation warning in Line {0}");

        /// <summary>'Import validation error in Line {0}'</summary>
        public static readonly string StrImportValidationErrorInLine = Catalog.GetString("Import validation error in Line {0}");

        /// <summary>'Validation error in line {0}'</summary>
        public static readonly string StrValidationErrorInLine = Catalog.GetString("Validation error in line {0}");

        /// <summary>'Error adding transactions'</summary>
        public static readonly string StrErrorAddingTransactions = Catalog.GetString("Error adding transactions");

        /// <summary>'Error adding transactions'</summary>
        public static readonly string StrHintReviewJournalContent = Catalog.GetString("Please review the current content of this Journal");

        #endregion

        #region Office Specific Data Types

        /// <summary>string</summary>
        public const String OFFICESPECIFIC_DATATYPE_CHAR = "char";

        /// <summary>decimal numbers</summary>
        public const String OFFICESPECIFIC_DATATYPE_FLOAT = "float";

        /// <summary>date</summary>
        public const String OFFICESPECIFIC_DATATYPE_DATE = "date";

        /// <summary>time</summary>
        public const String OFFICESPECIFIC_DATATYPE_TIME = "time";

        /// <summary>integer</summary>
        public const String OFFICESPECIFIC_DATATYPE_INTEGER = "integer";

        /// <summary>currency values</summary>
        public const String OFFICESPECIFIC_DATATYPE_CURRENCY = "currency";

        /// <summary>logical</summary>
        public const String OFFICESPECIFIC_DATATYPE_BOOLEAN = "boolean";

        /// <summary>partner key</summary>
        public const String OFFICESPECIFIC_DATATYPE_PARTNERKEY = "partnerkey";

        /// <summary>lookup, refering to another table</summary>
        public const String OFFICESPECIFIC_DATATYPE_LOOKUP = "lookup";

        #endregion

        #region Common Form Design Codes

        /// <summary>System</summary>
        public const String FORM_CODE_SYSTEM = "SYSTEM";

        /// <summary>Partner</summary>
        public const String FORM_CODE_PARTNER = "PARTNER";

        /// <summary>Personnel</summary>
        public const String FORM_CODE_PERSONNEL = "PERSONNEL";

        /// <summary>Conference</summary>
        public const String FORM_CODE_CONFERENCE = "CONFERENCE";

        /// <summary>Cheque</summary>
        public const String FORM_CODE_CHEQUE = "CHEQUE";

        /// <summary>Receipt</summary>
        public const String FORM_CODE_RECEIPT = "RECEIPT";

        /// <summary>Remittance</summary>
        public const String FORM_CODE_REMITTANCE = "REMITTANCE";

        /// <summary>Form Design Type Code</summary>
        public const string FORM_TYPE_CODE_STANDARD = "STANDARD";

        /// <summary>Form Design Type Code</summary>
        public const string FORM_TYPE_CODE_STANDARD_DESCRIPTION = "Standard Form or Letter";

        /// <summary>Form Design Type Code</summary>
        public const string FORM_TYPE_CODE_LABEL = "LABEL";

        /// <summary>Form Design Type Code</summary>
        public const string FORM_TYPE_CODE_LABEL_DESCRIPTION = "Special Form for Label Printing";

        /// <summary>Form Design Gift Options</summary>
        public const string FORM_OPTION_ALL = "All";

        /// <summary>Form Design Gift Options</summary>
        public const string FORM_OPTION_GIFT_IN_KIND_ONLY = "Gift in Kind Only";

        /// <summary>Form Design Gift Options</summary>
        public const string FORM_OPTION_GIFTS_ONLY = "Gifts Only";

        /// <summary>Form Design Gift Options</summary>
        public const string FORM_OPTION_OTHER = "Other";

        /// <summary>Form Design Adjustment Options</summary>
        public const string FORM_OPTION_ADJUSTMENTS_ONLY = "Adjustments Only";

        /// <summary>Form Design Adjustment Options</summary>
        public const string FORM_OPTION_EXCLUDE_ADJUSTMENTS = "Exclude Adjustments";

        /// <summary>Form Design Email Options</summary>
        public const string FORM_OPTION_BEST_EMAIL = "Best";

        /// <summary>Form Design Email Options</summary>
        public const string FORM_OPTION_SPLIT_EMAIL = "Split";


        #endregion

        #region Form Letter Contexts

        /// <summary>Form Letter Contexts</summary>
        public const string FORM_LETTER_CONTEXT_PARTNER_LETTER = "PARTNERLETTER";

        /// <summary>Form Letter Contexts</summary>
        public const string FORM_LETTER_CONTEXT_PARTNER_LABEL = "PARTNERLABEL";

        /// <summary>Form Letter Contexts</summary>
        public const string FORM_LETTER_CONTEXT_PERSONNEL = "PERSONNEL";

        /// <summary>Form Letter Contexts</summary>
        public const string FORM_LETTER_CONTEXT_CONFERENCE = "CONFERENCE";

        #endregion
    }
}