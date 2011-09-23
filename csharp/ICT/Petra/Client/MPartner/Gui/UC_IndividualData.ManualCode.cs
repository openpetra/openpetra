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
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_IndividualData
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        
        IndividualDataTDS FMainDS;          // FMainDS is NOT of Type 'PartnerEditTDS' in this UserControl!!!
        PartnerEditTDS FPartnerEditTDS;
        
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
        
        public PartnerEditTDS MainDS
        {
            get
            {
                return FPartnerEditTDS;
            }
            
            set
            {
                FPartnerEditTDS = value;
            }
        }
        #endregion
        #region Events
        
        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;
        
        #endregion
        
        #region Public Methods

        /// <summary>
        /// Sets up the screen logic, retrieves data, databinds the Grid and the Detail
        /// UserControl.
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseUserControl()
        {
            FMainDS = new IndividualDataTDS();
            FMainDS.Merge(FPartnerEditTDS);
//MessageBox.Show(FMainDS.SummaryData.Rows.Count.ToString());

            ucoSummaryData.PartnerEditUIConnector = FPartnerEditUIConnector;
            ucoSummaryData.SpecialInitUserControl(FMainDS);
        }
        
//        /// <summary>
//        /// Gets the data from all controls on this UserControl.
//        /// The data is stored in the DataTables/DataColumns to which the Controls
//        /// are mapped.
//        /// </summary>
//        public void GetDataFromControls2()
//        {
//            GetDataFromControls(FMainDS.PFamily[0]);
//        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }        
                           
        #endregion
                         
        #region Event Handlers
        
        private void IndividualDataItemSelected(object Sender, EventArgs e)
        {
            MessageBox.Show("IndividualDataItemSelected clicked.  Sender: " + Sender.ToString());
            
//            if (Sender == btnOverview)
//            {
//                
//            }
//            else if (Sender == btnSpecialNeeds)
//            {
//                
//            }
//            else if (Sender == btnLanguages)
//            {
//                
//            }
        }
        
        private void OpenBasicDataShepherd(object Sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void OpenIntranetRegistrationShepherd(object Sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ShowEmergencyContacts(object Sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
