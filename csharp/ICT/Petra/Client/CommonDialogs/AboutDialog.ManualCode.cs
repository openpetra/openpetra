//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Drawing;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Remoting.Client;

namespace Ict.Petra.Client.CommonDialogs
{
    /// manual methods for the generated window
    public partial class TFrmAboutDialog
    {
        private void InitializeManualCode()
        {
            ucoPetraLogoAndVersionInfo.PetraVersion = TClientInfo.ClientAssemblyVersion;
            ucoPetraLogoAndVersionInfo.InstallationKind = string.Empty;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            this.Close();
        }
    }
}