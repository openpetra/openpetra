//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.GL.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Tools.SampleDataConstructor
{
    /// <summary>
    /// tools for generating bank partners
    /// </summary>
    public class SampleDataBankPartners
    {
        /// <summary>
        /// generate the banks
        /// </summary>
        public static void GenerateBanks(string ABankCSVFile)
        {
            if (!File.Exists(ABankCSVFile))
            {
                TLogging.Log("there is no bank file " + ABankCSVFile);
                return;
            }

            TLogging.Log("creating banks from file " + ABankCSVFile);

            XmlDocument doc = TCsv2Xml.ParseCSVFile2Xml(ABankCSVFile, ",");

            XmlNode RecordNode = doc.FirstChild.NextSibling.FirstChild;

            PartnerImportExportTDS PartnerDS = new PartnerImportExportTDS();

            Int32 NumberOfPartnerKeysReserved = 100;
            Int64 NextPartnerKey = TNewPartnerKey.ReservePartnerKeys(-1, ref NumberOfPartnerKeysReserved);

            while (RecordNode != null)
            {
                PBankRow BankRow = PartnerDS.PBank.NewRowTyped();

                if (NumberOfPartnerKeysReserved == 0)
                {
                    NumberOfPartnerKeysReserved = 100;
                    NextPartnerKey = TNewPartnerKey.ReservePartnerKeys(-1, ref NumberOfPartnerKeysReserved);
                }

                BankRow.PartnerKey = NextPartnerKey;
                NextPartnerKey++;
                NumberOfPartnerKeysReserved--;

                BankRow.BranchName = TXMLParser.GetAttribute(RecordNode, "Branchname");
                BankRow.BranchCode = TXMLParser.GetAttribute(RecordNode, "Branchcode");
                BankRow.Bic = TXMLParser.GetAttribute(RecordNode, "Bic");
                PartnerDS.PBank.Rows.Add(BankRow);

                if (PartnerDS.PBank.Rows.Count % 1000 == 0)
                {
                    TLogging.Log("created bank " + PartnerDS.PBank.Rows.Count.ToString() + " " + BankRow.BranchName);
                }

                PPartnerRow PartnerRow = PartnerDS.PPartner.NewRowTyped();
                PartnerRow.PartnerKey = BankRow.PartnerKey;
                PartnerRow.PartnerShortName = BankRow.BranchName;
                PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_BANK;
                PartnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;
                PartnerDS.PPartner.Rows.Add(PartnerRow);

                // add empty location so that the partner can be found in the Partner Find screen
                PPartnerLocationRow PartnerLocationRow = PartnerDS.PPartnerLocation.NewRowTyped();
                PartnerLocationRow.PartnerKey = BankRow.PartnerKey;
                PartnerLocationRow.LocationKey = 0;
                PartnerLocationRow.SiteKey = 0;
                PartnerDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

                RecordNode = RecordNode.NextSibling;
            }

            PartnerDS.ThrowAwayAfterSubmitChanges = true;
            PartnerImportExportTDSAccess.SubmitChanges(PartnerDS);

            TLogging.Log("after saving banks");
        }
    }
}