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
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.BankImport
{
    /// <summary>
    /// this should be implemented for all sorts of bank file formats that contain the bank statements
    /// </summary>
    public interface IImportBankStatement
    {
        /// <summary>
        /// the file extensions that should be used for the file open dialog
        /// </summary>
        string GetFileFilter();

        /// <summary>
        /// open the file and return a typed datatable
        /// </summary>
        bool ImportFromFile(string AFilename, ref BankImportTDS AMainDS, out double ATotalAmountStatement, out Int32 ANumberAllTransactions);
    }
}