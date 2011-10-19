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
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Person;

namespace Ict.Petra.Client.MPartner.Gui
{        
    public partial class TUC_IndividualData_Summary
    {   
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        
        #region Properties
        
        /// <summary>used for passing through the Clientside Proxy for the UIConnector</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }    
        
        #endregion
        
        /// <summary>
        /// todoComment
        /// </summary>
        public void SpecialInitUserControl(IndividualDataTDS AMainDS)
        {
            FMainDS = AMainDS;
        
			LoadDataOnDemand();
            
            ShowData((PPersonRow)FMainDS.PPerson.Rows[0]);  
            
            if (FMainDS.SummaryData.Rows.Count > 0)
            {
//                MessageBox.Show("FMainDS.SummaryData.Rows.Count: " + FMainDS.SummaryData.Rows.Count.ToString() + Environment.NewLine +
//                                ((IndividualDataTDSSummaryDataRow)FMainDS.SummaryData.Rows[0]).FamilyName);
            }
            else
            {
                MessageBox.Show("FMainDS.SummaryData holds NO ROWS!");
            }
            
            dtpDateOfBirth.Enabled = true;
        }
        
        private void GetDataFromControlsManual(PPersonRow ARow)
        {
            
        }
        
        private void ShowDataManual(PPersonRow ARow)
        {
            
        }
        
        /// <summary>
        /// Loads Summary Data from Petra Server into FMainDS, if not already loaded.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.SummaryData == null)
                {
                    FMainDS.Tables.Add(new IndividualDataTDSSummaryDataTable());
                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading && 
                    (FMainDS.SummaryData.Rows.Count == 0))
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataPersonnelIndividualData(TIndividualDataItemEnum.idiSummary));

                    // Make DataRows unchanged
                    if (FMainDS.SummaryData.Rows.Count > 0)
                    {
                        if (FMainDS.SummaryData.Rows[0].RowState != DataRowState.Added)
                        {
                            FMainDS.SummaryData.AcceptChanges();
                        }
                    }
                }

                if (FMainDS.SummaryData.Rows.Count != 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            
            return ReturnValue;
        }        
    }
}
