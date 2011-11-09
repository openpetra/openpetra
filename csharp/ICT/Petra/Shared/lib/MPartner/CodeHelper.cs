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
    /// Gets descriptions for codes in the Partner Module.
    /// </summary>
    public class PartnerCodeHelper
    {
    	/// <summary>
    	/// Returns the description of a Marital Status Code.
    	/// </summary>
    	/// <param name="ACacheRetriever">Delegate that returns the specified DataTable from the data cache (client- or serverside).</param>
    	/// <param name="AMaritalStatusCode">Marital Status Code.</param>
    	/// <returns>The description of a Marital Status Code, or empty string if the Marital Status Code could not be identified.</returns>
		public static string GetMaritalStatusDescription(TGetCacheableDataTableFromCache ACacheRetriever, string AMaritalStatusCode)
		{
			DataTable MaritalStatusDT;
			DataRow MaritalStatusDR;
			string ReturnValue = "";
			Type tmp;
			
			MaritalStatusDT = ACacheRetriever(Enum.GetName(typeof(TCacheablePartnerTablesEnum), TCacheablePartnerTablesEnum.MaritalStatusList), out tmp);
			MaritalStatusDR = MaritalStatusDT.Rows.Find(new object[] { AMaritalStatusCode });
			
			if (MaritalStatusDR != null) 
			{
				ReturnValue = MaritalStatusDR[PtMaritalStatusTable.ColumnDescriptionId].ToString();
			}
			
			return ReturnValue;
		}    	
    }
}