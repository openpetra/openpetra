//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
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
using Mono.Unix;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TUC_SetupAnalysisValues
    {
        private Int32 FLedgerNumber;


        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
            }
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadValues(Int32 ALedgerNumber)
        {
            // TODO
        }

        private void NewRow(System.Object sender, EventArgs e)
        {
            // TODO
        }

        private void NewRowManual(ref AFreeformAnalysisRow ARow)
        {
            // TODO
        }

        private void DeleteRow(System.Object sender, EventArgs e)
        {
            // TODO
        }

        private void GetDetailDataFromControlsManual(AFreeformAnalysisRow ARow)
        {
            // TODO
        }
    }
}