//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Xml;

namespace Ict.Petra.Shared.Interfaces.Plugins.MFinance
{
    /// <summary>
    /// This interface defines which methods need to be implemented
    /// by a plugin for exporting data to a bank for electronic payment
    /// e.g. see Ict.Plugin.EP.Germany for the DTAUS text file export
    /// </summary>
    public interface IElectronicPaymentBankExport
    {
        /// <summary>
        /// Write a file for the bank using the data defined in XML
        /// </summary>
        /// <param name="AXMLFilename"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        bool WriteFile(string AXMLFilename, XmlNode data);
    }

    /// <summary>
    /// this should be implemented for all sorts of bank file formats that contain the bank statements
    /// </summary>
    public interface IImportBankStatement
    {
        /// <summary>
        /// this can ask the user to select a file, or could connect directly to the bank, etc;
        /// it should get the data of one or several bank statements and store the transactions in the tables
        /// a_ep_statement and a_ep_transaction in the database
        /// </summary>
        /// <param name="AStatementKey">this returns the first key of a statement that was imported. depending on the implementation, several statements can be created from one file</param>
        /// <param name="ALedgerNumber">the current ledger number</param>
        /// <param name="ABankAccountCode">the bank account against which the statement should be stored</param>
        /// <returns></returns>
        bool ImportBankStatement(out Int32 AStatementKey, Int32 ALedgerNumber, string ABankAccountCode);
    }
}