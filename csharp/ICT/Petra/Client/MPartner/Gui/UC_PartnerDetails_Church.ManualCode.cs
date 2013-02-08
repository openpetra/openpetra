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

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_PartnerDetails_Church
    {
        #region Public Methods

        /// <summary>
        /// Gets the data from all controls on this UserControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls2()
        {
            GetDataFromControls(FMainDS.PChurch[0]);
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
        
        private void InitializeManualCode()
        {
            this.SuspendLayout();
            
            // The following Controls are inside a Panel, but we don't want that to be noticed on the screen
            txtAccomodationSize.Top = txtAccomodationSize.Top - 6;
            cmbAccomodationType.Top = cmbAccomodationType.Top - 7;
            lblAccomodationSize.Top = lblAccomodationSize.Top - 7;
            lblAccomodationType.Top = lblAccomodationType.Top - 7;
            pnlAccomodationDetails.Top = pnlAccomodationDetails.Top - 3;
            pnlAccomodationDetails.Height = pnlAccomodationDetails.Height - 5;
            grpMisc.Height = grpMisc.Height - 7;
            
            ShowHideAccomodationData(null, null);            
            
            this.ResumeLayout();
        }
        
        private void ShowHideAccomodationData(object sender, EventArgs e)
        {
            pnlAccomodationDetails.Visible = chkAccomodation.Checked;
        }

        #endregion
    }
}