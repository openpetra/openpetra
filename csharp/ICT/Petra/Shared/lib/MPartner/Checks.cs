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
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Shared.MPartner
{
    /// <summary>
    /// Contains functions to be used by the Server and the Client that perform
    /// certain checks - specific for the Partner Module.
    /// </summary>
    public class Checks
    {
        /// <summary>
        /// Determines whether a specified Partner Type code is contained in the PPartnerTypeTable
        /// that is passed in. The AExactMatch parameter determines whether the check is done on an
        /// exact string match, or a 'starts with' string match.
        /// </summary>
        /// <param name="APartnerType"></param>Partner Type code that should be checked for
        /// <param name="APPartnerTypeDT"></param>PPartnerTypeTable containing rows of Partner Types
        /// <param name="AExactMatch"></param>Exact string match for APartnerType if true,
        /// otherwise 'starts with' string match
        /// <returns>true if APartnerType was found in APPartnerTypeDT, otherwise false</returns>
        public static bool HasPartnerType(string APartnerType, PPartnerTypeTable APPartnerTypeDT, bool AExactMatch)
        {
            DataRow[] foundRows;
            PPartnerTypeRow Row;

            if (APPartnerTypeDT == null)
            {
                throw new ArgumentException("APPartnerTypeDT must not be null");
            }

            if (APartnerType == "")
            {
                return false;
            }

            if (APPartnerTypeDT.Rows.Count != 0)
            {
                if (AExactMatch)
                {
                    foundRows = APPartnerTypeDT.Select(PPartnerTypeTable.GetTypeCodeDBName() + " = '" + APartnerType + "'");
                }
                else
                {
                    foundRows = APPartnerTypeDT.Select(PPartnerTypeTable.GetTypeCodeDBName() + " LIKE '" + APartnerType + "%'");
                }

                foreach (DataRow untypedRow in foundRows)
                {
                    Row = (PPartnerTypeRow)untypedRow;

                    if ((Row.IsValidFromNull()
                         || (Row.ValidFrom.Value.CompareTo(DateTime.Now) <= 0))
                        && (Row.IsValidUntilNull() || (Row.ValidUntil.Value.CompareTo(DateTime.Now) >= 0)))
                    {
                        return true;
                    }
                }

                // if APartnerType hasn't been found, false will be returned from this method because of the next statement!
            }

            return false;
        }

        /// <summary>
        /// Determines whether DataLabels exist for a specified Partner Class.
        /// </summary>
        /// <param name="APartnerClass">Partner Class that should be checked for.</param>
        /// <param name="ACacheableDataLabelsForPartnerClassesList">Instance of the Cacheable DataTable 'DataLabelsForPartnerClassesList'
        /// that has data in it (ie. the Cacheable DataTable must have been retrieved by the caller).</param>
        /// <returns>True if at least one DataLabel exists for the Partner Class specified in <paramref name="APartnerClass"></paramref>,
        /// otherwise false.</returns>
        public static bool HasPartnerClassLocalPartnerDataLabels(TPartnerClass APartnerClass, DataTable ACacheableDataLabelsForPartnerClassesList)
        {
            bool ReturnValue = false;

            DataRow[] PartnerClassDR =
                ACacheableDataLabelsForPartnerClassesList.Select("PartnerClass = '" + SharedTypes.PartnerClassEnumToString(APartnerClass) + "'");

            if (PartnerClassDR.Length > 0)
            {
                if (Convert.ToBoolean(PartnerClassDR[0]["DataLabelsAvailable"]) == true)
                {
                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }
        
        /// <summary>
        /// check if the partner has a valid Gift Destination in the past but not a currently active Gift Destination
        /// </summary>
        /// <param name="AGiftDestinationTable"></param>
        /// <returns></returns>
        public static bool PartnerIsExWorker(PPartnerGiftDestinationTable AGiftDestinationTable)
        {
        	if (AGiftDestinationTable == null || AGiftDestinationTable.Rows.Count == 0)
        	{
        		return false;
        	}
        	
        	bool ReturnValue = false;
        	
        	foreach (PPartnerGiftDestinationRow Row in AGiftDestinationTable.Rows)
        	{
        		// if currently active
        		if (Row.DateEffective <= DateTime.Today &&
        		   (Row.IsDateExpiresNull() || Row.DateExpires >= DateTime.Today) &&
        		   Row.DateEffective != Row.DateExpires)
        		{
        			return false;
        		}
        		
        		// if a previous gift destination exists
        		if (Row.DateEffective < DateTime.Today && Row.DateEffective != Row.DateExpires)
        		{
        			ReturnValue = true;
        		}
        	}
        	
        	return ReturnValue;
        }
    }
}