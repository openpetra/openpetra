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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces;
using Ict.Petra.Shared.Interfaces.MFinance.ICH.UIConnectors;
using Ict.Petra.Server.MFinance.ICH;
using System.Data;

namespace Ict.Petra.Server.MFinance.ICH.UIConnectors
{
    /// <summary>
    /// Stewardship Calculation UIConnector.
    /// </summary>
    /// <remarks>
    /// UIConnector Objects are instantiated by the Client's User Interface via the
    /// Instantiator classes.
    /// <para>
    /// These Objects would usually not be instantiated by other Server
    /// Objects, but only by the Client UI via the Instantiator classes.
    /// However, Server Objects that derive from these objects and that
    /// are also UIConnectors are feasible.
    /// </para>
    /// </remarks>
    public class TStewardshipCalculationUIConnector : TConfigurableMBRObject, IICHUIConnectorsStewardshipCalculation
    {
        private int FLedgerNumber;
        private int FPeriodNumber;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <returns>void</returns>
        public TStewardshipCalculationUIConnector(int ALedgerNumber, int APeriodNumber)
        {
            FLedgerNumber = ALedgerNumber;
            FPeriodNumber = APeriodNumber;
        }

        /// <summary>
        /// Performs the ICH Stewardship Calculation.
        /// </summary>
        /// <returns>True if calculation succeeded, otherwise false.</returns>
        public bool PerformStewardshipCalculation(out TVerificationResultCollection AVerificationResult)
        {
            TStewardshipCalculation StewardshipCalc = new TStewardshipCalculation();
            
            return StewardshipCalc.PerformStewardshipCalculation(FLedgerNumber, FPeriodNumber, out AVerificationResult);
        }
   }
}