//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2004-2015 by OM International
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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SourceGrid;
using GNU.Gettext;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using System.Resources;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Collections.Generic;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    /// <summary>
    /// auto generated class for report
    /// </summary>
    public partial class TFrmEsrInpaymentSlip
    {
        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
        }

        private void rbtSelectionChange(object sender, EventArgs e)
        {
        }

        private void AddRowToTable(DataTable ReportTable, Int64 DonorKey, Int64 RecipientKey, Int32 MailingCode, Int32 Copies)
        {
            DataRow Row = ReportTable.NewRow();

            Row["DonorKey"] = DonorKey;
            Row["RecipientKey"] = RecipientKey;
            string PartnerShortName;
            TPartnerClass PartnerClass;

            TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(
                DonorKey, out PartnerShortName, out PartnerClass);
            String[] PartnerNamePart = PartnerShortName.Split(new Char[] { ',' });
            Row["DonorShortName"] = PartnerNamePart[0] + " " + PartnerNamePart[1];

            TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(
                RecipientKey, out PartnerShortName, out PartnerClass);

            PartnerNamePart = PartnerShortName.Split(new Char[] { ',' });
            Row["RecipientShortName"] = PartnerNamePart[0] + " " + PartnerNamePart[1];

            PLocationTable LocationTbl;
            String CountryName;

            TRemote.MPartner.Mailing.WebConnectors.GetBestAddress(DonorKey,
                out LocationTbl, out CountryName);

            if (LocationTbl.Rows.Count > 0)
            {
                Row["DonorAddress"] = Calculations.DetermineLocationString(LocationTbl[0],
                    Calculations.TPartnerLocationFormatEnum.plfLineBreakSeparated,
                    1,
                    " "
                    );
            }

            TRemote.MPartner.Mailing.WebConnectors.GetBestAddress(RecipientKey,
                out LocationTbl, out CountryName);

            if (LocationTbl.Rows.Count > 0)
            {
                Row["RecipientAddress"] = Calculations.DetermineLocationString(LocationTbl[0],
                    Calculations.TPartnerLocationFormatEnum.plfLineBreakSeparated,
                    1,
                    " "
                    );
            }

            String MailingCodeString = MailingCode.ToString();
            MailingCodeString = (MailingCodeString + "8888888").Substring(0, 7);

            String LongCode = DonorKey.ToString("D10") + RecipientKey.ToString("D10") + MailingCodeString;
            Row["CodeWithSpaces"] = String.Format("{0} {1} {2} {3} {4} {5}",
                LongCode.Substring(0, 2),
                LongCode.Substring(2, 5),
                LongCode.Substring(7, 5),
                LongCode.Substring(12, 5),
                LongCode.Substring(17, 5),
                LongCode.Substring(22)
                );
            Row["CodeForOcr"] = "042>" + LongCode + "+ 010467502>";

            for (Int32 copy = 0; copy < Copies; copy++)
            {
                var newRow = ReportTable.NewRow();
                newRow.ItemArray = Row.ItemArray;
                ReportTable.Rows.Add(newRow);
            }
        }

        //
        // This will be called if the Fast Reports Wrapper loaded OK.
        // Returns True if the data apparently loaded OK and the report should be printed.
        private bool LoadReportData(TRptCalculator ACalc)
        {
            ArrayList reportParam = ACalc.GetParameters().Elems;

            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation") && (!paramsDictionary.ContainsKey(p.name)))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            DataTable ReportTable = new DataTable();
            ReportTable.Columns.Add("DonorKey", typeof(Int64));
            ReportTable.Columns.Add("DonorShortName", typeof(String));
            ReportTable.Columns.Add("DonorAddress", typeof(String));

            ReportTable.Columns.Add("RecipientKey", typeof(Int64));
            ReportTable.Columns.Add("RecipientShortName", typeof(String));
            ReportTable.Columns.Add("RecipientAddress", typeof(String));

            ReportTable.Columns.Add("MailingCode", typeof(Int32));
            ReportTable.Columns.Add("CodeWithSpaces", typeof(String));
            ReportTable.Columns.Add("CodeForOcr", typeof(String));

            Int64 DonorKey;
            Int64 RecipientKey;

            Int32 Copies;
            Boolean numbersOk = true;

            numbersOk &= Int64.TryParse(txtDonorKey.Text, out DonorKey);
            numbersOk &= Int64.TryParse(txtRecipientKey.Text, out RecipientKey);
            numbersOk &= Int32.TryParse(txtCopies.Text, out Copies);

            if (!numbersOk)
            {
                MessageBox.Show(
                    Catalog.GetString("Please ensure that Donor, Recipient, and Copies are correctly set."),
                    Catalog.GetString("ESR Inpayment Slip")
                    );
                return false;
            }

            Int32 MailingCode = 8; // If the mailing code box is left empty, I'll accept that.

            if (txtMailingCode.Text.Length > 0)
            {
                numbersOk = Int32.TryParse(txtMailingCode.Text, out MailingCode);

                if (!numbersOk)
                {
                    MessageBox.Show(
                        Catalog.GetString("For ESR, Mailing Code must be numeric."),
                        Catalog.GetString("ESR Inpayment Slip")
                        );
                    return false;
                }
            }

            AddRowToTable(ReportTable, DonorKey, RecipientKey, MailingCode, Copies);
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "EsrInpaymentSlip");
            return true;
        }
    }
}