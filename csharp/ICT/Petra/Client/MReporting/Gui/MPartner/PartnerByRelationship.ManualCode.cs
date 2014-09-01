// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//   awebster
//
// Copyright 2004-2013 by OM International
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
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Owf.Controls;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    public partial class TFrmPartnerByRelationship
    {
        private void InitializeManualCode(){}

        private void PartnerChanged(Int64 APartnerKey, String APartnerShortName, bool AValidSelection)
        {
            if (AValidSelection)
            {
                //TPartnerClass foo;
                //TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(APartnerKey, out APartnerShortName, out foo);

                //string AllowedPartnerClasses = FFromPartnerClass.ToString();
                //AllowedDifferentPartnerClasses(ref AllowedPartnerClasses, FFromPartnerClass);

                //// restrict the choice of partner class for txtMergeTo
                //txtMergeTo.PartnerClass = AllowedPartnerClasses;

                //if ((txtMergeTo.Text != "0000000000") && (APartnerKey != Convert.ToInt64(txtMergeTo.Text)))
                //{
                //    btnOK.Enabled = true;
                //}
                //else
                //{
                //    btnOK.Enabled = false;
                //}
            }
        }
    }
}