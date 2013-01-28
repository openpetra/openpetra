//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Collections;
using System.IO;
using System.Windows.Forms;

using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using GNU.Gettext;
using Ict.Common;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Manual code for the GL Info screen
    /// </summary>
    public partial class TFrmGLInfo
    {
        /// <summary>
        /// Initialize values
        /// </summary>
        public void InitializeManualCode()
        {
        }

        /// the ledger that the user is currently working with
        public Int32 LedgerNumber
        {
            set
            {
                ucoLedgerInfo.LedgerNumber = value;
                this.Text = String.Format(Catalog.GetString("Ledger Info - Ledger {0}"), value.ToString());
            }
        }

    }
}