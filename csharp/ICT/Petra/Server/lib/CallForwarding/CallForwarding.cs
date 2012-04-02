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

using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Server.MCommon.DataReader;
using Ict.Petra.Server.MPartner.Partner.ServerLookups;

namespace Ict.Petra.Server.CallForwarding
{
    /// <summary>
    /// Sets up Delegates that allow arbitrary code to be called in various server-side
    /// DLL's, avoiding 'circular dependencies' between DLL's that need to call Methods in
    /// other DLL's (which would also reference the DLL that the call would originate from).
    /// </summary>
    public class TCallForwarding
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        static TCallForwarding()
        {
            // Set up Error Codes and Data Validation Delegates for a Client's AppDomain.
            // This setting-up makes use of the fact that this Method is called only once,
            // namely directly after the Client logged in successfully.
            ErrorCodeInventory.RegisteredTypes.Add(new Ict.Petra.Shared.PetraErrorCodes().GetType());
            TSharedValidationHelper.SharedGetDataDelegate = @TCommonDataReader.GetData;

            TSharedPartnerValidationHelper.VerifyPartnerDelegate = @TPartnerServerLookups.VerifyPartner;
            Console.WriteLine("TCallForwading: static constructor ran!");
        }
    }
}