/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank, timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Windows.Forms;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Gui;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// the manually written part of TFrmPartnerMain
    /// </summary>
    public partial class TFrmPartnerMain
    {
        /// <summary>
        /// Initializes a new instance of the TPartnerEditDSWinForm class (called by constructor)
        /// </summary>
        public void InitializeManualCode()
        {
        }

        /// <summary>
        /// create a new partner
        /// </summary>
        public void NewPartner()
        {
            TPartnerEditDSWinForm frm = new TPartnerEditDSWinForm(this.Handle);

            frm.SetParameters(TScreenMode.smNew, "FAMILY", -1, -1, "");
            frm.Show();
        }

        /// <summary>
        /// open partner find screen
        /// </summary>
        public void PartnerFind()
        {
            TPartnerFindScreen frm = new TPartnerFindScreen(this.Handle);

            frm.SetParameters(false, -1);
            frm.Show();
        }
    }
}