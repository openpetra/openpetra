//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, christophert
//
// Copyright 2004-2011 by OM International
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
using Ict.Common.DB;
using Ict.Common.Verification;

namespace Ict.Petra.Server.MFinance.ICH
{
    /// <summary>
    /// ICH Stewardship Calculation functionality.
    /// </summary>
    public class TStewardshipCalculation 
    {
        /// <summary>
        /// Performs the ICH Stewardship Calculation.
        /// </summary>
        /// <returns>True if calculation succeeded, otherwise false.</returns>
        public bool PerformStewardshipCalculation(int ALedgerNumber, int APeriodNumber,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction DBTransaction;
            bool FirstCallResult = false;
            bool SecondCallResult = false;
            bool ThirdCallResult = false;
            
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": PerformStewardshipCalculation called.");
            }
#endif

            AVerificationResult = null;
            
            DBTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            
            try
            {
                FirstCallResult = GenerateAdminFeeBatch(ALedgerNumber, APeriodNumber, DBTransaction, AVerificationResult);

                if (FirstCallResult)
                {
                    SecondCallResult = true;   // TODO: Second call in sequence  - e,g,   '= SecondCall(DBTransaction, AVerificationResult)'
                }

                // TODO: 0..n other calls in sequence, e.g.:
                if (SecondCallResult)
                {
                    ThirdCallResult = true;   // TODO: Third call in sequence  - e,g,   '= ThirdCall(DBTransaction, AVerificationResult)'
                }
                
                
                if (FirstCallResult && SecondCallResult && ThirdCallResult)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                    if (TLogging.DL >= 8)
                    {
                        Console.WriteLine(this.GetType().FullName + ".PerformStewardshipCalculation: Transaction committed!");
                    }
#endif
                    return true;
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
#if DEBUGMODE
                    if (TLogging.DL >= 8)
                    {
                        Console.WriteLine(this.GetType().FullName + ".PerformStewardshipCalculation: Transaction ROLLED BACK because of an error!");
                    }
#endif
                    return false;
                }
            }
            catch (Exception Exp)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
#if DEBUGMODE
                if (TLogging.DL >= 8)
                {
                    Console.WriteLine(this.GetType().FullName + ".PerformStewardshipCalculation: Transaction ROLLED BACK because an exception occured!");
                }
#endif
                TLogging.Log(Exp.Message);
                TLogging.Log(Exp.StackTrace);

                throw Exp;
            }
        }
       
        /// <summary>
        /// Reads from the table holding all the fees charged for this month and generates a GL batch from it.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private bool GenerateAdminFeeBatch(int ALedgerNumber, int APeriodNumber, TDBTransaction ADBTransaction,
            TVerificationResultCollection AVerificationResult)
        {
            bool ReturnValue = true;  // TODO: change this to false once the actual implementation is in place!


            // TODO

                
            
            return ReturnValue;
        }
    }
}