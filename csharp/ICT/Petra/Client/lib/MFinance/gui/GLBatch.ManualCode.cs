/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
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
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TFrmGLBatch
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// AP is opened in this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
            }
        }

        /// <summary>
        ///  TODOComment
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            // TODO
            return false;
        }

        /// <summary>
        /// TODOComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FileSave(System.Object sender, EventArgs e)
        {
            // TODO
        }

        public void InitializeManualCode()
        {
            Ict.Petra.Shared.MFinance.Account.Data.ALedgerTable LedgerTable;
            todo
            TRemote.MCommon.DataReader.GetData("ALedger",
                new TSearchCriteria[] { new TSearchCriteria("LedgerNumber", FLedgerNumber) },
                out LedgerTable);
            FMainDS.Merge(LedgerTable);
        }
    }
}