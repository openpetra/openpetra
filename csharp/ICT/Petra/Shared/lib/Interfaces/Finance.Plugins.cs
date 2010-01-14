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
        /// the file extensions that should be used for the file open dialog
        /// </summary>
        string GetFileFilter();

        /// <summary>
        /// should return the text for the filter for AEpTransactionTable to get all the gifts, by transaction type
        /// </summary>
        /// <returns></returns>
        string GetFilterGifts();

        /// <summary>
        /// parse the file contents and store the statement in the database
        /// </summary>
        /// <param name="ABankStatement">the content of the bank statement file, can be binary or text</param>
        /// <param name="ABankStatementFilename">to keep track if a file is loaded again</param>
        /// <param name="ABankPartnerKey">the partner key of the bank that issued the statement. this can be overwritten by the plugin if the bank name is specified on the statement</param>
        /// <param name="ABankAccountKey">the key of the bank account that this statement belongs to. this can be overwritten by the plugin if the bank account is specified in the bank statement</param>
        /// <param name="AStatementKey">this returns the first key of a statement that was imported. depending on the implementation, several statements can be created from one file</param>
        /// <returns></returns>
        bool ImportFromStatement(byte[] ABankStatement,
            string ABankStatementFilename,
            Int64 ABankPartnerKey,
            Int32 ABankAccountKey,
            out Int32 AStatementKey);
    }
}