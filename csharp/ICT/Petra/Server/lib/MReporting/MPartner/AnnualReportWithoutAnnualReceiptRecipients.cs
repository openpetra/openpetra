//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2022 by OM International
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
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MReporting;
using System.Data.Odbc;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO; // Implicit reference
using Ict.Common.Remoting.Server;
using System.IO;
using HtmlAgilityPack;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Common;

namespace Ict.Petra.Server.MReporting.MPartner
{
    /// report for sending an annual report to supporters that did not receive an annual receipt
    public class AnnualReportWithoutAnnualReceiptRecipients
    {
        /// calculate the report
        public static HtmlDocument Calculate(
            string AHTMLReportDefinition,
            TParameterList parameterlist,
            TDBTransaction ATransaction
            )
        {
            HTMLTemplateProcessor templateProcessor = new HTMLTemplateProcessor(AHTMLReportDefinition, parameterlist);

            DataTable recipients = null;

            // get all the recipients
            string sql = templateProcessor.GetSQLQuery("SelectRecipients");
            recipients = ATransaction.DataBaseObj.SelectDT(sql, "recipients", ATransaction);

            // ensure consents
            string needed_consent = parameterlist.Get("param_consent").ToString().Replace("*", "").Replace("%", "").Trim();
            recipients = Utils.PartnerRemoveUnconsentReportData(recipients, needed_consent);

            // generate the report from the HTML template
            HtmlDocument html = templateProcessor.GetHTML();
            CalculateData(ref html, recipients, templateProcessor);

            return html;
        }


        private static void CalculateData(ref HtmlDocument html, DataTable partners, HTMLTemplateProcessor templateProcessor)
        {
            var partnerTemplate = HTMLTemplateProcessor.SelectSingleNode(html.DocumentNode, "//div[@id='partner_template']");
            var partnerParentNode = partnerTemplate.ParentNode;

            int countRow = 0;
            foreach (DataRow partner in partners.Rows)
            {
                if (partner["p_partner_class_c"].ToString() == "FAMILY")
                {
                    partner["PartnerName"] = Calculations.FormatShortName(partner["PartnerName"].ToString(), eShortNameFormat.eReverseShortname);
                }

                templateProcessor.AddParametersFromRow(partner);

                var newPartnerRow = partnerTemplate.Clone();
                string partnerId = "partner" + countRow.ToString();
                newPartnerRow.SetAttributeValue("id", partnerId);
                partnerParentNode.AppendChild(newPartnerRow);
                newPartnerRow.InnerHtml = templateProcessor.InsertParameters("{", "}", newPartnerRow.InnerHtml,
                    HTMLTemplateProcessor.ReplaceOptions.NoQuotes);

                // fix empty email column
                newPartnerRow.InnerHtml = newPartnerRow.InnerHtml.Replace(">,</div>", "></div>");

                countRow++;
            }
            partnerTemplate.Remove();
        }
    }
}
