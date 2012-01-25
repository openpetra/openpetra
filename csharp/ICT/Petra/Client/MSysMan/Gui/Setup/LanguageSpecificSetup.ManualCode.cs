//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MSysMan.Gui.Setup
{
    public partial class TFrmLanguageSpecificSetup
    {
        private void NewRowManual(ref SLanguageSpecificRow ARow)
        {
            // Deal with primary key.  LanguageCode is unique and is 10 characters.
            // We use the first unused language code from our language code list
            Type DataTableType;

            // Load Data
            PLanguageTable allLanguages = new PLanguageTable();
            DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("LanguageCodeList", String.Empty, null, out DataTableType);

            allLanguages.Merge(CacheDT);

            // Because of the checking code in NewRecord, we know there is at least one spare language we can use
            int rowIndex = 1;         // first row is invalid 99 code

            while (FMainDS.SLanguageSpecific.Rows.Find(new object[] { allLanguages.Rows[rowIndex][0].ToString() }) != null)
            {
                rowIndex++;
            }

            ARow.LanguageCode = allLanguages.Rows[rowIndex][0].ToString();
            ARow.MonthName1 = Catalog.GetString("January");
            ARow.MonthName2 = Catalog.GetString("February");
            ARow.MonthName3 = Catalog.GetString("March");
            ARow.MonthName4 = Catalog.GetString("April");
            ARow.MonthName5 = Catalog.GetString("May");
            ARow.MonthName6 = Catalog.GetString("June");
            ARow.MonthName7 = Catalog.GetString("July");
            ARow.MonthName8 = Catalog.GetString("August");
            ARow.MonthName9 = Catalog.GetString("September");
            ARow.MonthName10 = Catalog.GetString("October");
            ARow.MonthName11 = Catalog.GetString("November");
            ARow.MonthName12 = Catalog.GetString("December");

            ARow.MonthNameShort1 = Catalog.GetString("Jan");
            ARow.MonthNameShort2 = Catalog.GetString("Feb");
            ARow.MonthNameShort3 = Catalog.GetString("Mar");
            ARow.MonthNameShort4 = Catalog.GetString("Apr");
            ARow.MonthNameShort5 = Catalog.GetString("May");
            ARow.MonthNameShort6 = Catalog.GetString("Jun");
            ARow.MonthNameShort7 = Catalog.GetString("Jul");
            ARow.MonthNameShort8 = Catalog.GetString("Aug");
            ARow.MonthNameShort9 = Catalog.GetString("Sep");
            ARow.MonthNameShort10 = Catalog.GetString("Oct");
            ARow.MonthNameShort11 = Catalog.GetString("Nov");
            ARow.MonthNameShort12 = Catalog.GetString("Dec");
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            // We need to check that we do have a language that we do not yet have a calendar definition for
            Type DataTableType;

            // Load Data
            PLanguageTable allLanguages = new PLanguageTable();
            DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("LanguageCodeList", String.Empty, null, out DataTableType);

            allLanguages.Merge(CacheDT);

            Int32 languageCount = allLanguages.Rows.Count - 1;  // exclude the '99' row
            languageCount = 3;

            if (grdDetails.Rows.Count <= languageCount)
            {
                CreateNewSLanguageSpecific();
            }
            else
            {
                MessageBox.Show(Catalog.GetString("You already have a row for every specified language in the database."),
                    Catalog.GetString("Add new Month Names"));
            }
        }
    }
}