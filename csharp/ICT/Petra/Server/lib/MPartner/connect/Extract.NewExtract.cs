//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
//
// Copyright 2004-2012 by OM International
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
using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.MPartner.Extracts;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;

namespace Ict.Petra.Server.MPartner.Extracts.UIConnectors
{
    /// <summary>
    /// New Extract User Interface Connector
    /// It contains methods to create new extracts in m_extract_master data table
    /// </summary>
    public class TPartnerNewExtractUIConnector : IPartnerUIConnectorsPartnerNewExtract
    {
        Int32 FNewExtractID = -1;


        /// <summary>
        /// Constructor
        /// </summary>
        public TPartnerNewExtractUIConnector() : base()
        {
            TLogging.LogAtLevel(7, "TPartnerNewExtractUIConnector created.");
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~TPartnerNewExtractUIConnector()
        {
            TLogging.LogAtLevel(7, "TPartnerNewExtractUIConnector FINALIZE called!");
        }

        /// <summary>
        /// Creates a new extract with the given extract name and extract descriptionin the
        /// m_extract_master data table. The extract name must be unique.
        /// </summary>
        /// <param name="AExtractName">Name of the extract to be created</param>
        /// <param name="AExtractDescription">Description of the extract to be created</param>
        /// <param name="AExtractID">Extract Id of the new created extract. -1 if not successfull</param>
        /// <param name="AExtractAlreadyExists">True if there is already an extract with the given name. Otherwise false</param>
        /// <returns>true if the new extract was created. Otherwise false</returns>
        public bool CreateNewExtract(String AExtractName,
            String AExtractDescription,
            out Int32 AExtractID,
            out Boolean AExtractAlreadyExists)
        {
            bool Success;

            Success = TExtractsHandling.CreateNewExtract(AExtractName,
                AExtractDescription,
                out AExtractID,
                out AExtractAlreadyExists);

            if (Success)
            {
                FNewExtractID = AExtractID;
            }

            return Success;
        }

        /// <summary>
        /// create an extract from a list of best addresses
        /// </summary>
        /// <param name="AExtractName">Name of the Extract to be created.</param>
        /// <param name="AExtractDescription">Description of the Extract to be created.</param>
        /// <param name="ANewExtractId">Extract Id of the created Extract, or -1 if the
        /// creation of the Extract was not successful.</param>
        /// <param name="APartnerKeysTable"></param>
        /// <param name="APartnerKeyColumn">number of the column that contains the partner keys</param>
        /// <param name="AAddressFilterAdded">true if location key fields exist in APartnerKeysTable</param>
        /// <param name="ACommitTransaction">true if transaction should committed at end of method</param>
        /// <returns>True if the new Extract was created, otherwise false.</returns>
        public bool CreateExtractFromListOfPartnerKeys(
            String AExtractName,
            String AExtractDescription,
            out Int32 ANewExtractId,
            DataTable APartnerKeysTable,
            Int32 APartnerKeyColumn,
            bool AAddressFilterAdded,
            bool ACommitTransaction)
        {
            bool Success;

            Success = TExtractsHandling.CreateExtractFromListOfPartnerKeys(
                AExtractName,
                AExtractDescription,
                out ANewExtractId,
                APartnerKeysTable,
                APartnerKeyColumn,
                AAddressFilterAdded,
                ACommitTransaction);

            if (Success)
            {
                FNewExtractID = ANewExtractId;
            }

            return Success;
        }

        /// <summary>
        /// Deletes the Extract again that was created by this UIConnector.
        /// This is needed if a client-side process that involved creating a
        /// new Extract failed somehow and the created Extract needs to be
        /// deleted again.
        /// </summary>
        public void DeleteExtractAgain()
        {
            bool ExtractNotDeletable;
            TVerificationResult VerificationResult;

            if (FNewExtractID != -1)
            {
                if (!TExtractsHandling.DeleteExtract(FNewExtractID, out ExtractNotDeletable, out VerificationResult))
                {
                    if (ExtractNotDeletable)
                    {
                        throw new EOPAppException("Cannot delete Extract because it is not deletable");
                    }
                    else
                    {
                        throw new EOPAppException("Cannot delete Extract. Reason: " + VerificationResult.ResultText);
                    }
                }
            }
            else
            {
                throw new EOPAppException("Cannot delete Extract that wasn't yet created by " +
                    "this UIConnector!");
            }
        }
    }
}