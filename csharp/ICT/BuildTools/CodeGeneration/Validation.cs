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
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration
{
    /// <summary>
    /// functions for data validation
    /// </summary>
    public class TDataValidation
    {
        /// <summary>
        /// Spefies the scopes of automatic data validation.
        /// </summary>
        public enum TAutomDataValidationScope
        {
            /// <summary>All checks</summary>
            advsAll,

            /// <summary>Only NOT NULL checks</summary>
            advsNotNullChecks,

            /// <summary>Only Date checks</summary>
            advsDateChecks,

            /// <summary>Only String Lenght Checks</summary>
            advsStringLengthChecks,

            /// <summary>Only Number Range Checks</summary>
            advsNumberRangeChecks
        }

        /// <summary>
        /// Determines whether automatic Data Validation code should be created for a certain Control in a YAML file.
        /// </summary>
        /// <param name="AControl">Control in YAML file.</param>
        /// <param name="AHasDataField"></param>
        /// <param name="AMasterOrDetailTable">Pass in 'true' if the YAML file has got a 'MasterTable' or 'DetailTable' Element. </param>
        /// <param name="AIncludeMasterOrDetailTableControl"></param>
        /// <param name="AScope">Scope of the Data Validation that should be checked for. Specify <see cref="TAutomDataValidationScope.advsAll"/>
        /// to find out if any of the scopes should be checked against, or use any other value of that enum to specifiy a specific scope.</param>
        /// <param name="AReasonForAutomValidation">Contains the reason why automatic data validation code needs to be generated.</param>
        /// <returns>True if automatic Data Validation code should be created for the Control in a YAML that was passed in in <paramref name="AControl" /> for
        /// the scope that was specified with <paramref name="AScope" />, otherwise false. This Method also returns false if the Control specified in
        /// <paramref name="AControl" /> isn't linked to a DB Table Field.</returns>
        public static bool GenerateAutoValidationCodeForControl(TControlDef AControl, bool AHasDataField, bool AMasterOrDetailTable,
            bool AIncludeMasterOrDetailTableControl, TAutomDataValidationScope AScope, out string AReasonForAutomValidation)
        {
            TTableField DBField = null;
            bool IsDetailNotMaster;

            AReasonForAutomValidation = String.Empty;

            if (AHasDataField)
            {
                DBField = TDataBinding.GetTableField(AControl, AControl.GetAttribute("DataField"), out IsDetailNotMaster, true);
            }
            else if (AMasterOrDetailTable && AIncludeMasterOrDetailTableControl)
            {
                DBField = TDataBinding.GetTableField(AControl, AControl.controlName.Substring(
                        AControl.controlTypePrefix.Length), out IsDetailNotMaster, false);
            }

            if (DBField != null)
            {
                return GenerateAutoValidationCodeForDBTableField(DBField, AScope, out AReasonForAutomValidation);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether automatic Data Validation code should be created for a certain DB Table Field.
        /// </summary>
        /// <param name="ADBField">DB Field.</param>
        /// <param name="AScope">Scope of the Data Validation that should be checked for. Specify <see cref="TAutomDataValidationScope.advsAll"/>
        /// to find out if any of the scopes should be checked against, or use any other value of that enum to specifiy a specific scope.</param>
        /// <param name="AReasonForAutomValidation">Contains the reason why automatic data validation code needs to be generated.</param>
        /// <returns>True if automatic Data Validation code should be created for the DB Table Field passed in in <paramref name="ADBField" /> for
        /// the scope that was specified with <paramref name="AScope" />, otherwise false.</returns>
        public static bool GenerateAutoValidationCodeForDBTableField(TTableField ADBField,
            TAutomDataValidationScope AScope,
            out string AReasonForAutomValidation)
        {
            AReasonForAutomValidation = String.Empty;

            // NOT NULL checks
            if ((AScope == TAutomDataValidationScope.advsNotNullChecks)
                || (AScope == TAutomDataValidationScope.advsAll))
            {
                if (ADBField.bPartOfPrimKey)
                {
                    AReasonForAutomValidation = "must have a value (it is part of the Primary Key)";

                    if ((ADBField.strType == "varchar") || (ADBField.strType == "text"))
                    {
                        AReasonForAutomValidation += " and must not be an empty string";
                    }

                    return true;
                }
                else if (ADBField.bNotNull)
                {
                    AReasonForAutomValidation = "must have a value (NOT NULL constraint)";

                    return true;
                }
            }

            // Date checks
            if ((AScope == TAutomDataValidationScope.advsDateChecks)
                || (AScope == TAutomDataValidationScope.advsAll))
            {
                if (ADBField.strType == "date")
                {
                    AReasonForAutomValidation = "must represent a valid date";

                    return true;
                }
            }

            // String Length checks
            if ((AScope == TAutomDataValidationScope.advsStringLengthChecks)
                || (AScope == TAutomDataValidationScope.advsAll))
            {
                if (((ADBField.strType == "varchar") || (ADBField.strType == "text"))
                    && ((ADBField.strName != "s_created_by_c")
                        && (ADBField.strName != "s_modified_by_c")))
                {
                    AReasonForAutomValidation = "must not contain more than " + (ADBField.iCharLength * 2).ToString() + " characters";

                    return true;
                }
            }

            // Number Range checks
            if ((AScope == TAutomDataValidationScope.advsNumberRangeChecks)
                || (AScope == TAutomDataValidationScope.advsAll))
            {
                if (ADBField.strType == "number")
                {
                    if (ADBField.iDecimals > 0)
                    {
                        AReasonForAutomValidation = "must not have more than " +
                                                    (ADBField.iLength -
                                                     ADBField.iDecimals).ToString() + " digits before the decimal point and not more than " +
                                                    ADBField.iDecimals.ToString() +
                                                    " after the decimal point";
                    }
                    else
                    {
                        AReasonForAutomValidation = "must not have more than " + ADBField.iLength.ToString() + " digits and no decimals";
                    }

                    return true;
                }
            }

            return false;
        }
    }
}