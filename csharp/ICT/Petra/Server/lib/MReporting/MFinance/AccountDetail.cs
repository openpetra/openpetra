//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2018 by OM International
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

namespace Ict.Petra.Server.MReporting.MFinance
{
    /// calculate the Account Detail report
    public class AccountDetail
    {
        /// calculate the report
        public static string Calculate(
            string AHTMLReportDefinition,
            TParameterList parameterlist,
            out TResultList resultlist)
        {
            resultlist = new TResultList();

            HTMLTemplateProcessor templateProcessor = new HTMLTemplateProcessor(AHTMLReportDefinition, parameterlist);

            // get all the transactions

            // get all the balances

            // render the report

            return templateProcessor.GetHTML();
        }
    }
}
