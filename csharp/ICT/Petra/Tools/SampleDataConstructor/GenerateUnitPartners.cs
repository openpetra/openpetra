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
    /// tools for generating unit partners (fields and key ministries)
    /// </summary>
    public class SampleDataUnitPartners
    {
        /// LedgerNumber to be set from outside
        public static int FLedgerNumber = 43;

        /// <summary>
        /// generate the units
        /// </summary>
        /// <param name="AFieldCSVFile"></param>
        public static void GenerateFields(string AFieldCSVFile)
        {
            XmlDocument doc = TCsv2Xml.ParseCSV2Xml(AFieldCSVFile, ",");

            XmlNode RecordNode = doc.FirstChild.NextSibling.FirstChild;

            PartnerImportExportTDS PartnerDS = new PartnerImportExportTDS();
            GLSetupTDS GLSetupDS = new GLSetupTDS();

            PCountryTable countryTable = PCountryAccess.LoadAll(null);

            while (RecordNode != null)
            {
                PUnitRow UnitRow = PartnerDS.PUnit.NewRowTyped();
                long id = 100 + Convert.ToInt64(TXMLParser.GetAttribute(RecordNode, "id"));
                UnitRow.PartnerKey = id * 1000000;
                string CountryCode = TXMLParser.GetAttribute(RecordNode, "Name");
                UnitRow.UnitName = ((PCountryRow)countryTable.Rows.Find(CountryCode)).CountryName;
                UnitRow.UnitTypeCode = "F";
                PartnerDS.PUnit.Rows.Add(UnitRow);

                PPartnerRow PartnerRow = PartnerDS.PPartner.NewRowTyped();
                PartnerRow.PartnerKey = UnitRow.PartnerKey;
                PartnerRow.PartnerShortName = UnitRow.UnitName;
                PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_UNIT;
                PartnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;
                PartnerDS.PPartner.Rows.Add(PartnerRow);

                // add empty location so that the partner can be found in the Partner Find screen
                PPartnerLocationRow PartnerLocationRow = PartnerDS.PPartnerLocation.NewRowTyped();
                PartnerLocationRow.PartnerKey = UnitRow.PartnerKey;
                PartnerLocationRow.LocationKey = 0;
                PartnerLocationRow.SiteKey = 0;
                PartnerDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

                // create unit hierarchy
                UmUnitStructureRow UnitStructureRow = PartnerDS.UmUnitStructure.NewRowTyped();
                UnitStructureRow.ParentUnitKey = 1000000;
                UnitStructureRow.ChildUnitKey = UnitRow.PartnerKey;
                PartnerDS.UmUnitStructure.Rows.Add(UnitStructureRow);

                // create special type
                PPartnerTypeRow PartnerTypeRow = PartnerDS.PPartnerType.NewRowTyped();
                PartnerTypeRow.PartnerKey = UnitRow.PartnerKey;
                PartnerTypeRow.TypeCode = MPartnerConstants.PARTNERTYPE_LEDGER;
                PartnerDS.PPartnerType.Rows.Add(PartnerTypeRow);

                // create cost centre
                ACostCentreRow CostCentreRow = GLSetupDS.ACostCentre.NewRowTyped();
                CostCentreRow.LedgerNumber = FLedgerNumber;
                CostCentreRow.CostCentreCode = (id * 100).ToString("0000");
                CostCentreRow.CostCentreName = UnitRow.UnitName;
                CostCentreRow.CostCentreToReportTo = MFinanceConstants.INTER_LEDGER_HEADING;
                CostCentreRow.CostCentreType = MFinanceConstants.FOREIGN_CC_TYPE;
                GLSetupDS.ACostCentre.Rows.Add(CostCentreRow);

                // create foreign ledger, cost centre link validledgernumber
                AValidLedgerNumberRow ValidLedgerNumber = GLSetupDS.AValidLedgerNumber.NewRowTyped();
                ValidLedgerNumber.LedgerNumber = FLedgerNumber;
                ValidLedgerNumber.PartnerKey = UnitRow.PartnerKey;
                ValidLedgerNumber.CostCentreCode = CostCentreRow.CostCentreCode;
                ValidLedgerNumber.IltProcessingCentre = Convert.ToInt64(MFinanceConstants.ICH_COST_CENTRE) * 10000;
                GLSetupDS.AValidLedgerNumber.Rows.Add(ValidLedgerNumber);

                RecordNode = RecordNode.NextSibling;
            }

            if(PartnerImportExportTDSAccess.SubmitChanges(PartnerDS) != TSubmitChangesResult.scrOK)
            {
                throw new Exception("A problem occured during a call to PartnerImportExportTDSAccess.SubmitChanges(AInspectDS)");
            }

            if(GLSetupTDSAccess.SubmitChanges(GLSetupDS) != TSubmitChangesResult.scrOK)
            {
                throw new Exception("A problem occured during a call to GLSetupTDSAccess.SubmitChanges(AInspectDS)");
            }
        }

        /// <summary>
        /// link the fields in the current ledger
        /// </summary>
        /// <param name="AFieldCSVFile"></param>
        public static void GenerateFieldsFinanceOnly(string AFieldCSVFile)
        {
            XmlDocument doc = TCsv2Xml.ParseCSV2Xml(AFieldCSVFile, ",");

            XmlNode RecordNode = doc.FirstChild.NextSibling.FirstChild;

            GLSetupTDS GLSetupDS = new GLSetupTDS();

            PCountryTable countryTable = PCountryAccess.LoadAll(null);

            while (RecordNode != null)
            {
                long id = 100 + Convert.ToInt64(TXMLParser.GetAttribute(RecordNode, "id"));
                string CountryCode = TXMLParser.GetAttribute(RecordNode, "Name");
                string UnitName = ((PCountryRow)countryTable.Rows.Find(CountryCode)).CountryName;
                Int64 PartnerKey = id * 1000000;

                // create cost centre
                ACostCentreRow CostCentreRow = GLSetupDS.ACostCentre.NewRowTyped();
                CostCentreRow.LedgerNumber = FLedgerNumber;
                CostCentreRow.CostCentreCode = (id * 100).ToString("0000");
                CostCentreRow.CostCentreName = UnitName;
                CostCentreRow.CostCentreToReportTo = MFinanceConstants.INTER_LEDGER_HEADING;
                CostCentreRow.CostCentreType = MFinanceConstants.FOREIGN_CC_TYPE;
                GLSetupDS.ACostCentre.Rows.Add(CostCentreRow);

                // create foreign ledger, cost centre link validledgernumber
                AValidLedgerNumberRow ValidLedgerNumber = GLSetupDS.AValidLedgerNumber.NewRowTyped();
                ValidLedgerNumber.LedgerNumber = FLedgerNumber;
                ValidLedgerNumber.PartnerKey = PartnerKey;
                ValidLedgerNumber.CostCentreCode = CostCentreRow.CostCentreCode;
                ValidLedgerNumber.IltProcessingCentre = Convert.ToInt64(MFinanceConstants.ICH_COST_CENTRE) * 10000;
                GLSetupDS.AValidLedgerNumber.Rows.Add(ValidLedgerNumber);

                RecordNode = RecordNode.NextSibling;
            }

            if(GLSetupTDSAccess.SubmitChanges(GLSetupDS) != TSubmitChangesResult.scrOK)
            {
                throw new Exception("A problem occured during a call to GLSetupTDSAccess.SubmitChanges(AInspectDS)");
            }
        }

        /// <summary>
        /// generate the key ministries
        /// </summary>
        /// <param name="AKeyMinCSVFile"></param>
        public static void GenerateKeyMinistries(string AKeyMinCSVFile)
        {
            XmlDocument doc = TCsv2Xml.ParseCSV2Xml(AKeyMinCSVFile, ",");

            XmlNode RecordNode = doc.FirstChild.NextSibling.FirstChild;

            PartnerImportExportTDS PartnerDS = new PartnerImportExportTDS();

            // get a list of fields (all class UNIT, with unit type F)
            string sqlGetFieldPartnerKeys = "SELECT p_partner_key_n, p_unit_name_c FROM PUB_p_unit WHERE u_unit_type_code_c = 'F'";
            DataTable FieldKeys = DBAccess.GDBAccessObj.SelectDT(sqlGetFieldPartnerKeys, "keys", null);

            Int32 NumberOfPartnerKeysReserved = 100;
            Int64 NextPartnerKey = TNewPartnerKey.ReservePartnerKeys(-1, ref NumberOfPartnerKeysReserved);

            while (RecordNode != null)
            {
                int FieldID =
                    Convert.ToInt32(TXMLParser.GetAttribute(RecordNode, "field")) % FieldKeys.Rows.Count;
                long FieldPartnerKey = Convert.ToInt64(FieldKeys.Rows[FieldID].ItemArray[0]);

                PUnitRow UnitRow = PartnerDS.PUnit.NewRowTyped();

                if (NumberOfPartnerKeysReserved == 0)
                {
                    NumberOfPartnerKeysReserved = 100;
                    NextPartnerKey = TNewPartnerKey.ReservePartnerKeys(-1, ref NumberOfPartnerKeysReserved);
                }

                long UnitPartnerKey = NextPartnerKey;
                NextPartnerKey++;
                NumberOfPartnerKeysReserved--;

                UnitRow.PartnerKey = UnitPartnerKey;
                UnitRow.UnitName = FieldKeys.Rows[FieldID].ItemArray[1].ToString() + " - " + TXMLParser.GetAttribute(RecordNode, "KeyMinName");
                UnitRow.UnitTypeCode = "KEY-MIN";
                PartnerDS.PUnit.Rows.Add(UnitRow);

                PPartnerRow PartnerRow = PartnerDS.PPartner.NewRowTyped();
                PartnerRow.PartnerKey = UnitRow.PartnerKey;
                PartnerRow.PartnerShortName = UnitRow.UnitName;
                PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_UNIT;
                PartnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;
                PartnerDS.PPartner.Rows.Add(PartnerRow);

                // add empty location so that the partner can be found in the Partner Find screen
                PPartnerLocationRow PartnerLocationRow = PartnerDS.PPartnerLocation.NewRowTyped();
                PartnerLocationRow.PartnerKey = UnitRow.PartnerKey;
                PartnerLocationRow.LocationKey = 0;
                PartnerLocationRow.SiteKey = 0;
                PartnerDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

                // create unit hierarchy
                UmUnitStructureRow UnitStructureRow = PartnerDS.UmUnitStructure.NewRowTyped();
                UnitStructureRow.ParentUnitKey = FieldPartnerKey;
                UnitStructureRow.ChildUnitKey = UnitRow.PartnerKey;
                PartnerDS.UmUnitStructure.Rows.Add(UnitStructureRow);

                RecordNode = RecordNode.NextSibling;
            }

            if(PartnerImportExportTDSAccess.SubmitChanges(PartnerDS) != TSubmitChangesResult.scrOK)
            {
                throw new Exception("A problem occured during a call to PartnerImportExportTDSAccess.SubmitChanges(AInspectDS)");
            }
        }
    }
}