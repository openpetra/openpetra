//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2019 by OM International
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
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.WebConnectors;

namespace Ict.Petra.Server.MReporting.MPartner
{
    /// calculate the Partner By City report
    public class PartnerByCity
    {
        /// calculate the report
        public static HtmlDocument Calculate(
            string AHTMLReportDefinition,
            TParameterList parameterlist,
            TDBTransaction ATransaction)
        {
            if (parameterlist.Get("param_city").ToString().Replace("*","").Replace("%","").Trim() == String.Empty)
            {
                throw new Exception("invalid parameters");
            }

            HTMLTemplateProcessor templateProcessor = new HTMLTemplateProcessor(AHTMLReportDefinition, parameterlist);

            // get all the partners
            string sql = templateProcessor.GetSQLQuery("SelectPartners");

            DataTable partners = ATransaction.DataBaseObj.SelectDT(sql, "transactions", ATransaction);

            // TODO: get best address

            // ensure consents
            string needed_consent = parameterlist.Get("param_consent").ToString().Replace("*", "").Replace("%", "").Trim();
            partners = Utils.PartnerRemoveUnconsentReportData(partners, needed_consent);

            // generate the report from the HTML template
            HtmlDocument html = templateProcessor.GetHTML();
            CalculateData(ref html, partners, templateProcessor);

            return html;
        }


        private static void CalculateData(ref HtmlDocument html, DataTable partners, HTMLTemplateProcessor templateProcessor)
        {
            var partnerTemplate = HTMLTemplateProcessor.SelectSingleNode(html.DocumentNode, "//div[@id='partner_template']");
            var partnerParentNode = partnerTemplate.ParentNode;

            int countRow = 0;
            foreach (DataRow partner in partners.Rows)
            {
                templateProcessor.AddParametersFromRow(partner);

                var newPartnerRow = partnerTemplate.Clone();
                string partnerId = "partner" + countRow.ToString();
                newPartnerRow.SetAttributeValue("id", partnerId);
                partnerParentNode.AppendChild(newPartnerRow);
                newPartnerRow.InnerHtml = templateProcessor.InsertParameters("{", "}", newPartnerRow.InnerHtml,
                    HTMLTemplateProcessor.ReplaceOptions.NoQuotes);

                countRow++;
            }
            partnerTemplate.Remove();
        }
    }
}
