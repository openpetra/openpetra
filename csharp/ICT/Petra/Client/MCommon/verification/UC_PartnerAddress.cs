//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner.Partner.Data;
using System.Diagnostics;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;

namespace Ict.Petra.Client.MCommon.Verification
{
    /// <summary>
    /// Contains verification logic for the UC_PartnerAddress UserControl.
    /// </summary>
    public class TPartnerAddressVerification
    {
        #region Resoucestrings

        private static readonly string StrErrorTheCodeIsNotAssignableSecurity = Catalog.GetString(
            "The code {0} cannot be assigned because you are not\r\nin Security Group '{1}'!");

        #endregion

        #region TPartnerAddressVerification

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AVerificationResultCollection"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static Boolean VerifyPartnerLocationData(DataColumnChangeEventArgs e,
            TVerificationResultCollection AVerificationResultCollection,
            out TVerificationResult AVerificationResult)
        {
            Boolean ReturnValue;

            AVerificationResult = null;

            // MessageBox.Show('Verifying DataRow...');
            if ((e.Column.ColumnName == PPartnerLocationTable.GetDateEffectiveDBName())
                || (e.Column.ColumnName == PPartnerLocationTable.GetDateGoodUntilDBName()))
            {
                VerifyDates(e, AVerificationResultCollection, out AVerificationResult);
            }

            if (e.Column.Ordinal == ((PPartnerLocationTable)e.Column.Table).ColumnEmailAddress.Ordinal)
            {
                VerifyEmailAddress(e, out AVerificationResult);
            }

            if (e.Column.Ordinal == ((PPartnerLocationTable)e.Column.Table).ColumnLocationType.Ordinal)
            {
                VerifyLocationType(e, out AVerificationResult);
            }

            // any verification errors?
            if (AVerificationResult == null)
            {
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;

                // MessageBox.Show('VerifyPartnerLocationData: There was an error!');
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AVerificationResultCollection"></param>
        /// <param name="AVerificationResult"></param>
        public static void VerifyDates(DataColumnChangeEventArgs e,
            TVerificationResultCollection AVerificationResultCollection,
            out TVerificationResult AVerificationResult)
        {
            AVerificationResult = null;
            DateTime DateGoodUntil = new DateTime();
            DateTime DateEffective = new DateTime();
            try
            {
                if (e.Column.ColumnName == PPartnerLocationTable.GetDateEffectiveDBName())
                {
                    // MessageBox.Show('p_date_effective_d: ' + e.Row['p_date_effective_d', DataRowVersion.Current].ToString);
                    DateEffective = TSaveConvert.ObjectToDate(e.ProposedValue);
                    DateGoodUntil = TSaveConvert.ObjectToDate(e.Row[PPartnerLocationTable.GetDateGoodUntilDBName()]);
                }
                else if (e.Column.ColumnName == PPartnerLocationTable.GetDateGoodUntilDBName())
                {
                    DateEffective = TSaveConvert.ObjectToDate(e.Row[PPartnerLocationTable.GetDateEffectiveDBName()]);
                    DateGoodUntil = TSaveConvert.ObjectToDate(e.ProposedValue);
                }

                // MessageBox.Show('p_date_effective_d: ' + DateEffective.ToString + Environment.NewLine +
                // 'p_date_good_until_d: ' + DateGoodUntil.ToString);
                if (e.Column.ColumnName == PPartnerLocationTable.GetDateEffectiveDBName())
                {
                    AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateEffective, DateGoodUntil, "Valid From", "Valid To");
                }
                else
                {
                    AVerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(DateGoodUntil, DateEffective, "Valid To", "Valid From");
                }

                if (e.Column.ColumnName == PPartnerLocationTable.GetDateEffectiveDBName())
                {
                    // delete any error that might have been there from a verification of the other column
                    AVerificationResultCollection.Remove(PPartnerLocationTable.GetDateGoodUntilDBName());
                }
                else
                {
                    // delete any error that might have been there from a verification of the other column
                    AVerificationResultCollection.Remove(PPartnerLocationTable.GetDateEffectiveDBName());
                }
            }
            catch (Exception Exp)
            {
                MessageBox.Show("Exception occured in TPartnerAddressVerification.VerifyDates: " + Exp.ToString());
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AVerificationResult"></param>
        public static void VerifyEmailAddress(DataColumnChangeEventArgs e, out TVerificationResult AVerificationResult)
        {
            AVerificationResult = (TVerificationResult)TStringChecks.ValidateEmail(e.ProposedValue.ToString(), true);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AVerificationResult"></param>
        public static void VerifyLocationType(DataColumnChangeEventArgs e, out TVerificationResult AVerificationResult)
        {
            AVerificationResult = null;
            PLocationTypeTable DataCache_ListTable;
            PLocationTypeRow FoundRow;
            DialogResult UseAlthoughUnassignable;
            DataCache_ListTable = (PLocationTypeTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.LocationTypeList);
            FoundRow = (PLocationTypeRow)DataCache_ListTable.Rows.Find(new Object[] { e.ProposedValue.ToString() });

            if (FoundRow != null)
            {
                if (!FoundRow.Assignable)
                {
                    UseAlthoughUnassignable = TMessages.MsgQuestion(
                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE, e.ProposedValue.ToString()),
                        new StackTrace(false).GetFrame(2).GetMethod().DeclaringType, false);

                    if (UseAlthoughUnassignable == System.Windows.Forms.DialogResult.No)
                    {
                        AVerificationResult = new TVerificationResult("",
                            "",
                            "",
                            PetraErrorCodes.ERR_VALUEUNASSIGNABLE,
                            TResultSeverity.Resv_Noncritical);
                    }
                }
                else
                {
                    if (e.ProposedValue.ToString().EndsWith(SharedConstants.SECURITY_CAN_LOCATIONTYPE)
                        && (!UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_ADDRESSCAN)))
                    {
                        TMessages.MsgGeneralError(
                            String.Format(StrErrorTheCodeIsNotAssignableSecurity, e.ProposedValue, SharedConstants.PETRAGROUP_ADDRESSCAN),
                            MCommonResourcestrings.StrValueUnassignable,
                            PetraErrorCodes.ERR_VALUEUNASSIGNABLE,
                            new StackTrace(false).GetFrame(2).GetMethod().DeclaringType);
                        AVerificationResult = new TVerificationResult("", "", "",
                            PetraErrorCodes.ERR_VALUEUNASSIGNABLE,
                            TResultSeverity.Resv_Noncritical);
                    }
                }
            }
        }

        #endregion
    }
}