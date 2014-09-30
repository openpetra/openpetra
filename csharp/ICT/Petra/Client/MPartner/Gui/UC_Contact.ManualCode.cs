//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb, andreww
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
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_Contact
    {
        private PPartnerContactRow FContactDR = null;
        private bool FInitializationRunning { get; set; }

        private PPartnerContactRow GetSelectedMasterRow()
        {
            return FContactDR;
        }

        /// <summary>
        /// Display data in control based on data from ARow
        /// </summary>
        /// <param name="ARow"></param>
        public void ShowDetails(PPartnerContactRow ARow)
        {
            FInitializationRunning = true;

            // show controls if not visible yet
            //MakeScreenInvisible(false);

            // set member
            FContactDR = ARow;

            //ShowData(ARow);
            
            // for every record initially disable issue group box, but enable button in the same group box
            //if (FAllowEditIssues)
            //{
            //    this.btnEditIssues.Visible = true;
            //    this.btnEditIssues.Enabled = true;
            //}

            //EnableDisableIssuesGroupBox(false);

            //txtPublicationCost.ReadOnly = true;

            // make sure initialization happens
            //PublicationStatusChanged(null, null);

            FInitializationRunning = false;
        }

        /// <summary>
        /// Read data from controls into ARow parameter
        /// </summary>
        /// <param name="ARow"></param>
        public void GetDetails(PPartnerContactRow ARow)
        {
            //ValidateAllData(false);
            //GetDataFromControls(ARow);
        }
        
    }
}