//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// general methods for use in partner module
    /// </summary>
    public class TPartnerWebConnector
    {
        /// <summary>
        /// performs database changes to move person from current (old) family to new family record
        /// </summary>
        /// <param name="APersonKey"></param>
        /// <param name="AOldFamilyKey"></param>
        /// <param name="ANewFamilyKey"></param>
        /// <param name="AProblemMessage"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>true if change of family completed successfully</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool ChangeFamily(Int64 APersonKey, Int64 AOldFamilyKey, Int64 ANewFamilyKey, 
			out String AProblemMessage, out TVerificationResultCollection AVerificationResult)
        {
            Boolean ResultValue = false;

            ResultValue = Server.MPartner.Partner.TFamilyHandling.ChangeFamily(APersonKey, 
                                                                               AOldFamilyKey, 
                                                                               ANewFamilyKey,
                                                                               out AProblemMessage,
                                                                               out AVerificationResult);
            
            return ResultValue;
        }
    }
}