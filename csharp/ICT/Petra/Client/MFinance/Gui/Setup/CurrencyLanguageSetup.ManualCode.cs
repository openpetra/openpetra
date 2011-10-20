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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmCurrencyLanguageSetup
    {
        private void NewRowManual(ref ACurrencyLanguageRow ARow)
        {
            // Deal with primary key.  CurrencyCode (varchar(16)) and LanguageCode (varchar(20)) are unique.
            // Initially we try a few popular currencies with a few popular languages
            // Then we try the same currencies with much more unlikely languages
            // In all we can try 11 x 17 = 187 possibilities
            // We will only fail if you actually have chosen to store all our possibilities
            // It is much more likely that you will want to store other combinations than all of these, so we will
            // find something that has not been actually used.
            string[] tryCurrencies = {"USD", "GBP", "EUR", "INR", "JPY", "AUD", "NZD", "CHF", "RUR", "SEK", "ZAR"};
            string[] tryLanguages = {"EN", "DE", "ES", "FR", "HI", "IT", "JA", "MAN", "NO", "SV", "AF", "AR", "BG", "BN", "CAN", "CRE", "CY"};
            int nTryLanguage = 0;
            int nTryCurrency = 0;
            while (FMainDS.ACurrencyLanguage.Rows.Find(new object[] { tryCurrencies[nTryCurrency], tryLanguages[nTryLanguage] }) != null)
            {
                if (++nTryCurrency == tryCurrencies.Length)
                {
                    if (++nTryLanguage == tryLanguages.Length)
                    {
                        break;
                    }
                    nTryCurrency = 0;
                }
            }
            
            ARow.CurrencyCode = tryCurrencies[nTryCurrency];
            ARow.LanguageCode = tryLanguages[nTryLanguage];
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewACurrencyLanguage();
        }
    }
}