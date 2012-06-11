//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_Application_Event
    {
        // <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        //private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private IndividualDataTDS FMainDS;          // FMainDS is NOT of Type 'PartnerEditTDS' in this UserControl!!!
        private ApplicationTDS FApplicationDS;

        #region Properties
        
        /// dataset for the whole screen
        public IndividualDataTDS MainDS
        {
            get
            {
                return FMainDS;
            }

            set
            {
                FMainDS = value;
            }
        }

        #endregion
        
        #region Public Methods

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        /// <summary>
        /// Sets up the screen logic, retrieves data, databinds the Grid and the Detail
        /// UserControl.
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseUserControl()
        {
            FApplicationDS = new ApplicationTDS();
            //FApplicationDS.Tables.Add(new PmGeneralApplicationTable());
           	//FApplicationDS.Tables.Add(new PmShortTermApplicationTable());
            FApplicationDS.InitVars();
            
			ucoEvent.PetraUtilsObject = FPetraUtilsObject;
			ucoApplicant.PetraUtilsObject = FPetraUtilsObject;
			ucoTravel.PetraUtilsObject = FPetraUtilsObject;
        }
        
        /// <summary>
        /// Display data in control based on data from Rows
        /// </summary>
        /// <param name="AGeneralAppRow"></param>
        /// <param name="AEventAppRow"></param>
        public void ShowDetails(PmGeneralApplicationRow AGeneralAppRow, PmShortTermApplicationRow AEventAppRow)
        {
            ShowData(AGeneralAppRow, AEventAppRow);
        }

        /// <summary>
        /// Read data from controls into Row parameters
        /// </summary>
        /// <param name="ARow"></param>
        /// <param name="AEventAppRow"></param>
        public void GetDetails(PmGeneralApplicationRow ARow, PmShortTermApplicationRow AEventAppRow)
        {
            GetDataFromControls(ARow, AEventAppRow);
        }
        
        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        #region Private Methods

        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
        }

        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }
        
        private void InitializeManualCode()
        {
        }

        private void GetDataFromControlsManual(PmGeneralApplicationRow ARow)
        {
        }

        private void ValidateDataDetailsManual(PmGeneralApplicationRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            //TODO
        }

        private void ShowData(PmGeneralApplicationRow AGeneralAppRow, PmShortTermApplicationRow AEventAppRow)
        {
        	// clear dataset and create a copy of the row to be displayed so Dataset contains only one set of records
            FApplicationDS.PmShortTermApplication.Rows.Clear();
            FApplicationDS.PmGeneralApplication.Rows.Clear();
            
            PmGeneralApplicationRow GeneralAppRowCopy = (PmGeneralApplicationRow)FApplicationDS.PmGeneralApplication.NewRow();
            PmShortTermApplicationRow EventAppRowCopy = (PmShortTermApplicationRow)FApplicationDS.PmShortTermApplication.NewRow();
            
            DataUtilities.CopyAllColumnValues(AGeneralAppRow, GeneralAppRowCopy);
            DataUtilities.CopyAllColumnValues(AEventAppRow, EventAppRowCopy);
            
            FApplicationDS.PmGeneralApplication.Rows.Add(GeneralAppRowCopy);
            FApplicationDS.PmShortTermApplication.Rows.Add(EventAppRowCopy);
            
            ucoEvent.MainDS = FApplicationDS;
            ucoApplicant.MainDS = FApplicationDS;
            ucoTravel.MainDS = FApplicationDS;
            
            ucoEvent.ShowDetails(GeneralAppRowCopy);
            ucoApplicant.ShowDetails(GeneralAppRowCopy);
            ucoTravel.ShowDetails(GeneralAppRowCopy);
        }        
        
 	    private void GetDataFromControls(PmGeneralApplicationRow ARow, PmShortTermApplicationRow AEventAppRow)
        {
 	    	ucoEvent.GetDetails(FApplicationDS.PmGeneralApplication[0]);
 	    	ucoApplicant.GetDetails(FApplicationDS.PmGeneralApplication[0]);
 	    	ucoTravel.GetDetails(FApplicationDS.PmGeneralApplication[0]);
 	    	
            DataUtilities.CopyAllColumnValues(FApplicationDS.PmGeneralApplication[0], ARow);
            DataUtilities.CopyAllColumnValues(FApplicationDS.PmShortTermApplication[0], AEventAppRow);
 	    }
       #endregion
    }
}