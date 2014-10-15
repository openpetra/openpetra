//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK
//
// Copyright 2004-2014 by OM International
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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// A utility class that centralises aspects of 'SaveChanges' Methods in OpenPetra. Those aspects perform (nearly) 
    /// the same operations everywhere; where they differ, Arguments of these central Methods make those different 
    /// behaviours possible.
    /// </summary>
    public static class TCommonSaveChangesFunctions
    {
        /// <summary>
        /// Processes the result of a data submission to the Server where the result of that operation is
        /// <see cref="TSubmitChangesResult.scrOK" />. (Overload for DataTables.)
        /// </summary>
        /// <param name="ACallingFormOrUserControl"></param>
        /// <param name="ALocalDT"></param>
        /// <param name="ASubmitDT"></param>
        /// <param name="APetraUtilsObject"></param>
        /// <param name="AVerificationResults"></param>
        /// <param name="ASetPrimaryKeyOnlyMethod"></param>
        /// <param name="AMasterDataTableSaveCall"></param>
        /// <param name="ACalledFromUserControl"></param>
        /// <param name="ACallAcceptChangesOnReturnedDataBeforeMerge"></param>
        public static void ProcessSubmitChangesResultOK(IFrmPetra ACallingFormOrUserControl, DataTable ALocalDT, 
            DataTable ASubmitDT, TFrmPetraEditUtils APetraUtilsObject, TVerificationResultCollection AVerificationResults,
            Action<bool> ASetPrimaryKeyOnlyMethod, bool AMasterDataTableSaveCall, bool ACalledFromUserControl, 
            bool ACallAcceptChangesOnReturnedDataBeforeMerge = false)
        {
            if (AMasterDataTableSaveCall) 
            {            
                // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                ALocalDT.AcceptChanges();
    
                // Merge back with data from the Server (eg. for getting Sequence values)
                if (ACallAcceptChangesOnReturnedDataBeforeMerge) 
                {
                    ASubmitDT.AcceptChanges();    
                }
                
                ALocalDT.Merge(ASubmitDT, false);
    
                // Need to accept any new modification ID's
                ALocalDT.AcceptChanges();
            
                if (ASetPrimaryKeyOnlyMethod != null) 
                {
                    // Ensure the Primary-Key(s)-containing Controls are disabled to prevent further modification of Primary Key values
                    ASetPrimaryKeyOnlyMethod(true);                                    
                }
            }
            
            CommonPostMergeOperations(ACallingFormOrUserControl, APetraUtilsObject, 
                AVerificationResults, ACalledFromUserControl);
        }
        
        /// <summary>
        /// Processes the result of a data submission to the Server where the result of that operation is
        /// <see cref="TSubmitChangesResult.scrOK" />. (Overload for Typed DataSets.)
        /// </summary>
        /// <param name="ACallingFormOrUserControl"></param>
        /// <param name="ALocalTDS"></param>
        /// <param name="ASubmitTDS"></param>
        /// <param name="APetraUtilsObject"></param>
        /// <param name="AVerificationResults"></param>
        /// <param name="ASetPrimaryKeyOnlyMethod"></param>
        /// <param name="AMasterDataTableSaveCall"></param>
        /// <param name="ACalledFromUserControl"></param>
        /// <param name="ACallAcceptChangesOnReturnedDataBeforeMerge"></param>
        public static void ProcessSubmitChangesResultOK(IFrmPetra ACallingFormOrUserControl, TTypedDataSet ALocalTDS, 
            TTypedDataSet ASubmitTDS, TFrmPetraEditUtils APetraUtilsObject, TVerificationResultCollection AVerificationResults,
            Action<bool> ASetPrimaryKeyOnlyMethod, bool AMasterDataTableSaveCall, bool ACalledFromUserControl, 
            bool ACallAcceptChangesOnReturnedDataBeforeMerge = false)
        {
            if (AMasterDataTableSaveCall) 
            {            
                // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                ALocalTDS.AcceptChanges();
    
                // Merge back with data from the Server (eg. for getting Sequence values)
                if (ACallAcceptChangesOnReturnedDataBeforeMerge) 
                {
                    ASubmitTDS.AcceptChanges();    
                }
                
                ALocalTDS.Merge(ASubmitTDS, false);
    
                // Need to accept any new modification ID's
                ALocalTDS.AcceptChanges();
            
                if (ASetPrimaryKeyOnlyMethod != null) 
                {
                    // Ensure the Primary-Key(s)-containing Controls are disabled to prevent further modification of Primary Key values
                    ASetPrimaryKeyOnlyMethod(true);                                    
                }
            }
            
            CommonPostMergeOperations(ACallingFormOrUserControl, APetraUtilsObject, 
                AVerificationResults, ACalledFromUserControl);
        }        

        /// <summary>
        /// Processes the result of a data submission to the Server where the result of that operation is
        /// <see cref="TSubmitChangesResult.scrNothingToBeSaved" />.
        /// </summary>
        /// <param name="ACallingFormOrUserControl"></param>
        /// <param name="APetraUtilsObject"></param>
        /// <param name="ACalledFromUserControl"></param>
        public static void ProcessSubmitChangesResultNothingToBeSaved(IFrmPetra ACallingFormOrUserControl, TFrmPetraEditUtils APetraUtilsObject, bool ACalledFromUserControl = false)
        {
            CommonUIUpdatesNoChangesAnymore(ACallingFormOrUserControl, APetraUtilsObject, 
                MCommonResourcestrings.StrSavingDataNothingToSave, ACalledFromUserControl);
        }

		private static void CommonPostMergeOperations(IFrmPetra ACallingFormOrUserControl, TFrmPetraEditUtils APetraUtilsObject, TVerificationResultCollection AVerificationResults, bool ACalledFromUserControl = false)
		{
		    CommonUIUpdatesSavingSuccessful(ACallingFormOrUserControl, APetraUtilsObject, ACalledFromUserControl);
						
			if ((AVerificationResults != null) 
			    && (AVerificationResults.HasCriticalOrNonCriticalErrors))
			{
				TDataValidation.ProcessAnyDataValidationErrors(false, AVerificationResults, ACallingFormOrUserControl.GetType(), null);
			}
		}

		private static void CommonUIUpdatesSavingSuccessful(IFrmPetra ACallingFormOrUserControl, TFrmPetraEditUtils APetraUtilsObject,
            bool ACalledFromUserControl = false)
		{
		    CommonUIUpdatesNoChangesAnymore(ACallingFormOrUserControl, APetraUtilsObject, 
		      MCommonResourcestrings.StrSavingDataSuccessful, ACalledFromUserControl);
		}        

		private static void CommonUIUpdatesNoChangesAnymore(IFrmPetra ACallingFormOrUserControl, TFrmPetraEditUtils APetraUtilsObject, string AStatusBarMessage,
            bool ACalledFromUserControl = false)
		{
			APetraUtilsObject.WriteToStatusBar(AStatusBarMessage);
			APetraUtilsObject.ShowDefaultCursor();
			
			// We don't have unsaved changes anymore
			APetraUtilsObject.DisableSaveButton();

            
			if (!ACalledFromUserControl) 
			{
				APetraUtilsObject.OnDataSaved(ACallingFormOrUserControl, new TDataSavedEventArgs(true));
			}			
		}
    }
}