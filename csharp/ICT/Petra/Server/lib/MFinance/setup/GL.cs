/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2010 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.GL.Data.Access;

namespace Ict.Petra.Server.MFinance.Setup.WebConnectors
{
    /// <summary>
    /// Description of GL.
    /// </summary>
    public static class TGLSetupWebConnector
    {
        /// <summary>
        /// save modified account hierarchy etc; does not support moving accounts;
        /// also used for saving cost centre hierarchy and cost centre details
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static TSubmitChangesResult SaveGLSetupTDS(ref GLSetupTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            if (AInspectDS == null)
            {
                return TSubmitChangesResult.scrNothingToBeSaved;
            }

            if ((AInspectDS.AAccount != null) && (AInspectDS.AAccount.Count > 0))
            {
                // load all account properties, there are not so many anyways
                AInspectDS.Merge(AAccountPropertyAccess.LoadViaALedger(AInspectDS.AAccount[0].LedgerNumber, null));
                AInspectDS.AAccountProperty.AcceptChanges();

                // check AAccount, if BankAccountFlag is not null, then create AAccountProperty or delete it
                foreach (GLSetupTDSAAccountRow acc in AInspectDS.AAccount.Rows)
                {
                    // if the flag has been changed by the client, it will not be null
                    if (!acc.IsBankAccountFlagNull())
                    {
                        AInspectDS.AAccountProperty.DefaultView.RowFilter =
                            String.Format("{0}='{1}' and {2}='{3}'",
                                AAccountPropertyTable.GetAccountCodeDBName(),
                                acc.AccountCode,
                                AAccountPropertyTable.GetPropertyCodeDBName(),
                                MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT);

                        if ((AInspectDS.AAccountProperty.DefaultView.Count == 0) && acc.BankAccountFlag)
                        {
                            AAccountPropertyRow accProp = AInspectDS.AAccountProperty.NewRowTyped(true);
                            accProp.LedgerNumber = acc.LedgerNumber;
                            accProp.AccountCode = acc.AccountCode;
                            accProp.PropertyCode = MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT;
                            accProp.PropertyValue = "true";
                            AInspectDS.AAccountProperty.Rows.Add(accProp);
                        }
                        else if (AInspectDS.AAccountProperty.DefaultView.Count == 1)
                        {
                            AAccountPropertyRow accProp = (AAccountPropertyRow)AInspectDS.AAccountProperty.DefaultView[0].Row;

                            if (!acc.BankAccountFlag)
                            {
                                accProp.Delete();
                            }
                            else
                            {
                                accProp.PropertyValue = "true";
                            }
                        }
                    }
                }

                AInspectDS.AAccountProperty.DefaultView.RowFilter = "";
            }

            return GLSetupTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);
        }
    }
}