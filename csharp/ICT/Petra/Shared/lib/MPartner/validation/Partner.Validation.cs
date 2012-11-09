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
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Shared.MPartner.Validation
{
    /// <summary>
    /// Contains functions for the validation of MPartner Partner DataTables.
    /// </summary>
    public static partial class TSharedPartnerValidation_Partner
    {
        /// <summary>todoComment</summary>
        private static readonly string StrBICSwiftCodeInvalid = Catalog.GetString(
            "The BIC / Swift code you entered for this bank is invalid!" + "\r\n" + "\r\n" +
            "  Here is the format of a valid BIC: 'BANKCCLL' or 'BANKCCLLBBB'." + "\r\n" +
            "    BANK = Bank Code. This code identifies the bank world wide." + "\r\n" +
            "    CC   = Country Code. This is the ISO country code of the country the bank is in.\r\n" +
            "    LL   = Location Code. This code gives the town where the bank is located." + "\r\n" +
            "    BBB  = Branch Code. This code denotes the branch of the bank." + "\r\n" +
            "  BICs have either 8 or 11 characters." + "\r\n");

        /// <summary>
        /// Validates the Partner Detail data of a Partner of PartnerClass BANK.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidatePartnerBankManual(object AContext, PBankRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'BIC' (Bank Identifier Code) must be valid
            ValidationColumn = ARow.Table.Columns[PBankTable.ColumnBicId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (CommonRoutines.CheckBIC(ARow.Bic) == false)
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_BANKBICSWIFTCODEINVALID, StrBICSwiftCodeInvalid)),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }
                else
                {
                    VerificationResult = null;
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // For information only: 'Branch Code' format matches the format of a BIC
            ValidationColumn = ARow.Table.Columns[PBankTable.ColumnBranchCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if ((ARow.BranchCode != null)
                    && (ARow.BranchCode != String.Empty))
                {
                    if (CommonRoutines.CheckBIC(ARow.BranchCode) == true)
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_BRANCHCODELIKEBIC, String.Empty,
                                    new String[] {
                                        ValidationControlsData.ValidationControlLabel,
                                        ValidationControlsData.SecondValidationControlLabel,
                                        ValidationControlsData.ValidationControlLabel,
                                        ValidationControlsData.ValidationControlLabel
                                    },
                                    new String[] { ValidationControlsData.ValidationControlLabel })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                    else
                    {
                        VerificationResult = null;
                    }

                    // Handle addition/removal to/from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }
        }

        /// <summary>
        /// Validates the Partner data of a Partner.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidatePartnerManual(object AContext, PPartnerRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TScreenVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'PartnerStatus' must not be set to MERGED
            ValidationColumn = ARow.Table.Columns[PPartnerTable.ColumnStatusCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (ARow.StatusCode == SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscMERGED))
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PARTNERSTATUSMERGEDCHANGEUNDONE)),
                        ValidationColumn, ValidationControlsData.ValidationControl);

                    // Note: The error code 'ERR_PARTNERSTATUSMERGEDCHANGEUNDONE' sets VerificationResult.ControlValueUndoRequested = true!
                }
                else
                {
                    VerificationResult = null;
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Subscription data of a Partner.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidateSubscriptionManual(object AContext, PSubscriptionRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TScreenVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'SubscriptionStatus' must not be null or empty
            ValidationColumn = ARow.Table.Columns[PSubscriptionTable.ColumnSubscriptionStatusId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (((!ARow.IsSubscriptionStatusNull())
                     && (ARow.SubscriptionStatus == String.Empty))
                    || (ARow.IsSubscriptionStatusNull()))
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_SUBSCRIPTION_STATUSMANDATORY)),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }
                else
                {
                    VerificationResult = null;
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Relationship data of a Partner.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <param name="AValidateForNewPartner">true if validation is run for a new partner record</param>
        /// <param name="APartnerKey">main partner key this validation is run for</param>
        /// <returns>void</returns>
        public static void ValidateRelationshipManual(object AContext, PPartnerRelationshipRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict,
            bool AValidateForNewPartner = false, Int64 APartnerKey = 0)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Partner' must have a valid partner key and must not be 0
            ValidationColumn = ARow.Table.Columns[PPartnerRelationshipTable.ColumnPartnerKeyId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = null;

                // don't complain if this is done for a new partner record (partner key not yet saved in db)
                if (!(AValidateForNewPartner
                      && (APartnerKey == ARow.PartnerKey)))
                {
                    VerificationResult = TSharedPartnerValidation_Partner.IsValidPartner(
                        ARow.PartnerKey, new TPartnerClass[] { }, false, "",
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                    // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                    // ResultText!
                    AVerificationResultCollection.Remove(ValidationColumn);
                    AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
                }

                // 'Partner Key' and 'Another Partner Key'must not be the same
                // (Partner Key 0 will be dealt with by other checks)
                if ((ARow.PartnerKey != 0)
                    && (ARow.PartnerKey == ARow.RelationKey))
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUESIDENTICAL_ERROR,
                                new string[] { ARow.PartnerKey.ToString(), ARow.RelationKey.ToString() })),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }

                // Handle addition to/removal from TVerificationResultCollection
                //if (AVerificationResultCollection.Contains(ValidationColumn))
                //{xxx
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
                //}
            }

            // 'Another Partner' must have a valid partner key and must not be 0
            ValidationColumn = ARow.Table.Columns[PPartnerRelationshipTable.ColumnRelationKeyId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                // don't complain if this is done for a new partner record (partner key not yet saved in db)
                if (!(AValidateForNewPartner
                      && (APartnerKey == ARow.RelationKey)))
                {
                    VerificationResult = TSharedPartnerValidation_Partner.IsValidPartner(
                        ARow.RelationKey, new TPartnerClass[] { }, false, "",
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                    // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                    // ResultText!
                    AVerificationResultCollection.Remove(ValidationColumn);
                    AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
                }
            }

            // 'Relation' must be valid and have a value
            ValidationColumn = ARow.Table.Columns[PPartnerRelationshipTable.ColumnRelationNameId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                PRelationTable RelationTable;
                PRelationRow RelationRow;

                VerificationResult = null;

                if ((!ARow.IsRelationNameNull())
                    && (ARow.RelationName != String.Empty))
                {
                    RelationTable = (PRelationTable)TSharedDataCache.TMPartner.GetCacheablePartnerTable(
                        TCacheablePartnerTablesEnum.RelationList);
                    RelationRow = (PRelationRow)RelationTable.Rows.Find(ARow.RelationName);

                    // 'Relation' must be valid
                    if ((RelationRow != null)
                        && !RelationRow.ValidRelation)
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING, new string[] { ARow.RelationName })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                }
                else
                {
                    VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.RelationName,
                        ValidationControlsData.ValidationControlLabel,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Checks whether a Partner with a certain PartnerKey and a range of valid PartnerClasses exists.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey.</param>
        /// <param name="AValidPartnerClasses">An array of PartnerClasses. If the Partner exists, but its
        /// PartnerClass isn't in the array, a TVerificationResult is still returned.</param>
        /// <param name="AZeroPartnerKeyIsValid">Set to true if <paramref name="APartnerKey" /> 0 should be considered
        /// as valid (Default: false)</param>
        /// <param name="AErrorMessageText">Text that should be prepended to the ResultText. (Default: empty string)</param>
        /// <param name="AResultContext">ResultContext (Default: null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null). (Default: null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null). (Default: null).</param>
        /// <returns>Null if the Partner exists and its PartnerClass is in the <paramref name="AValidPartnerClasses" />
        /// array. If the Partner exists, but its PartnerClass isn't in the array, a TVerificationResult
        /// with details about the error is returned. This is also the case if the Partner doesn't exist at all
        /// or got merged into another Partner, or if <paramref name="APartnerKey" /> is 0 and <paramref name="AZeroPartnerKeyIsValid" />
        /// is false.
        /// </returns>
        public static TVerificationResult IsValidPartner(Int64 APartnerKey, TPartnerClass[] AValidPartnerClasses,
            bool AZeroPartnerKeyIsValid = false, string AErrorMessageText = "", object AResultContext = null,
            System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            string ShortName;
            TPartnerClass PartnerClass;
            bool PartnerExists;
            bool IsMergedPartner;
            string ValidPartnerClassesStr = String.Empty;
            bool PartnerClassValid = false;
            string PartnerClassInvalidMessageStr = Catalog.GetString("The Partner Class of the Partner needs to be '{0}', but it is '{1}'.");
            string PartnerClassConcatStr = Catalog.GetString(" or ");

            if ((AZeroPartnerKeyIsValid)
                && (APartnerKey == 0))
            {
                return null;
            }
            else if ((!AZeroPartnerKeyIsValid)
                     && (APartnerKey == 0))
            {
                if (AErrorMessageText == String.Empty)
                {
                    ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                            PetraErrorCodes.ERR_PARTNERKEY_INVALID_NOZERO, new string[] { APartnerKey.ToString() }));
                }
                else
                {
                    ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                            PetraErrorCodes.ERR_PARTNERKEY_INVALID_NOZERO));
                    ReturnValue.OverrideResultText(AErrorMessageText + Environment.NewLine + ReturnValue.ResultText);
                }
            }
            else
            {
                bool VerificationOK = TSharedPartnerValidationHelper.VerifyPartner(APartnerKey, AValidPartnerClasses, out PartnerExists,
                    out ShortName, out PartnerClass, out IsMergedPartner);

                if ((!VerificationOK)
                    || (IsMergedPartner))
                {
                    if (AErrorMessageText == String.Empty)
                    {
                        ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                                PetraErrorCodes.ERR_PARTNERKEY_INVALID, new string[] { APartnerKey.ToString() }));
                    }
                    else
                    {
                        ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                                PetraErrorCodes.ERR_PARTNERKEY_INVALID, new string[] { APartnerKey.ToString() }));
                        ReturnValue.OverrideResultText(AErrorMessageText + Environment.NewLine + ReturnValue.ResultText);
                    }

                    if ((PartnerExists)
                        && (!IsMergedPartner))
                    {
                        if ((AValidPartnerClasses.Length == 1)
                            && (AValidPartnerClasses[0] != PartnerClass))
                        {
                            ReturnValue.OverrideResultText(ReturnValue.ResultText + " " +
                                String.Format(PartnerClassInvalidMessageStr, AValidPartnerClasses[0], PartnerClass));
                        }
                        else if (AValidPartnerClasses.Length > 1)
                        {
                            for (int Counter = 0; Counter < AValidPartnerClasses.Length; Counter++)
                            {
                                ValidPartnerClassesStr += "'" + AValidPartnerClasses[Counter] + "'" + PartnerClassConcatStr;

                                if (AValidPartnerClasses[Counter] == PartnerClass)
                                {
                                    PartnerClassValid = true;
                                }
                            }

                            if (!PartnerClassValid)
                            {
                                ValidPartnerClassesStr = ValidPartnerClassesStr.Substring(0,
                                    ValidPartnerClassesStr.Length - PartnerClassConcatStr.Length - 1);                                                                                            // strip off "' or "
                                ReturnValue.OverrideResultText(ReturnValue.ResultText + " " +
                                    String.Format(PartnerClassInvalidMessageStr, ValidPartnerClassesStr, PartnerClass));
                            }
                        }
                    }
                }
            }

            if ((ReturnValue != null)
                && (AResultColumn != null))
            {
                ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks that a Partner with a certain PartnerKey exists and is a Partner of PartnerClass UNIT.
        /// </summary>
        /// <param name="APartnerKey">PartneKey.</param>
        /// <param name="AZeroPartnerKeyIsValid">Set to true if <paramref name="APartnerKey" /> 0 should be considered
        /// as valid (Default: false)</param>
        /// <param name="AErrorMessageText">Text that should be prepended to the ResultText. (Default: empty string)</param>
        /// <param name="AResultContext">ResultContext (optional).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null). (Default: null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null). (Default: null).</param>
        /// <returns>Null if the Partner exists and its PartnerClass is UNIT. If the Partner exists,
        /// but its PartnerClass isn't UNIT, a TVerificationResult with details about the error is
        /// returned. This is also the case if the Partner doesn't exist at all or got merged
        /// into another Partner, or if <paramref name="APartnerKey" /> is 0 and <paramref name="AZeroPartnerKeyIsValid" />
        /// is false.
        /// </returns>
        public static TVerificationResult IsValidUNITPartner(Int64 APartnerKey, bool AZeroPartnerKeyIsValid = false,
            string AErrorMessageText = "", object AResultContext = null, System.Data.DataColumn AResultColumn = null,
            System.Windows.Forms.Control AResultControl = null)
        {
            return IsValidPartner(APartnerKey, new TPartnerClass[] { TPartnerClass.UNIT }, AZeroPartnerKeyIsValid,
                AErrorMessageText, AResultContext, AResultColumn, AResultControl);
        }

        /// <summary>
        /// Validates the MPartner Mailing Setup screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateMailingSetup(object AContext, PMailingRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'MailingDate' must not be empty
            ValidationColumn = ARow.Table.Columns[PMailingTable.ColumnMailingDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.MailingDate,
                    ValidationControlsData.ValidationControlLabel, AVerificationResultCollection, true, 
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }
    }
}