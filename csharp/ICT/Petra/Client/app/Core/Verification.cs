//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.App.Core
{

	/// <summary>
	/// Contains OpenPetra application-wide verification Methods.
	/// </summary>
    public class TAppCoreVerification
    {
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
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null). (Default: null).</param>    	
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
	    				PetraErrorCodes.ERR_PARTNERKEY_INVALID_NOZERO, new string[] {APartnerKey.ToString()}));   				
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
	    		bool VerificationOK = TServerLookup.TMPartner.VerifyPartner(APartnerKey, AValidPartnerClasses, out PartnerExists,
	    		    out ShortName, out PartnerClass, out IsMergedPartner);
	    		
	    		if ((!VerificationOK)
	    		    || (IsMergedPartner))
	    		{
	    			if (AErrorMessageText == String.Empty) 
	    			{
		    			ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
		    				PetraErrorCodes.ERR_PARTNERKEY_INVALID, new string[] {APartnerKey.ToString()}));   				
	    			}
	    			else
	    			{
	    				ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
		    				PetraErrorCodes.ERR_PARTNERKEY_INVALID, new string[] {APartnerKey.ToString()}));    				
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
							
							if(!PartnerClassValid)
							{
								ValidPartnerClassesStr = ValidPartnerClassesStr.Substring(0, ValidPartnerClassesStr.Length - PartnerClassConcatStr.Length - 1);   // strip off "' or "
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
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null). (Default: null).</param>
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
    		return IsValidPartner(APartnerKey, new TPartnerClass[] {TPartnerClass.UNIT}, AZeroPartnerKeyIsValid, 
        	    AErrorMessageText, AResultContext, AResultColumn, AResultControl);
    	}
    }
}