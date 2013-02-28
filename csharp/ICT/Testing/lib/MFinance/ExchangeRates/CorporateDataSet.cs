//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       AlanP
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
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics;

using NUnit.Extensions.Forms;
using NUnit.Framework;

using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;
using Ict.Testing.NUnitTools;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;

namespace Tests.MFinance.Client.ExchangeRates
{
    public partial class TCorporateExchangeRateTest
    {
        #region FMainDS Class Definition

        private class FMainDS : SerialisableDS
        {
            public static ACorporateExchangeRateTable ACorporateExchangeRate;

            public static void LoadAll()
            {
                ACorporateExchangeRate = new ACorporateExchangeRateTable();
                SerialisableDS.LoadAll(ACorporateExchangeRate, ACorporateExchangeRateTable.GetTableDBName());
            }

            public static bool SaveChanges()
            {
                TTypedDataTable TableChanges = ACorporateExchangeRate.GetChangesTyped();

                return SerialisableDS.SaveChanges(ACorporateExchangeRate, TableChanges, ACorporateExchangeRateTable.GetTableDBName());
            }

            public static void DeleteAllRows()
            {
                DataView view = FMainDS.ACorporateExchangeRate.DefaultView;

                for (int i = view.Count - 1; i >= 0; i--)
                {
                    view[i].Delete();
                }
            }

            public static void InsertStandardRows()
            {
                for (int i = 0; i <= StandardData.GetUpperBound(0); i++)
                {
                    AddARow(StandardData[i, 0].ToString(),
                        StandardData[i, 1].ToString(),
                        DateTime.Parse(StandardData[i, 2].ToString(), CultureInfo.InvariantCulture),
                        Convert.ToDecimal(StandardData[i, 3]));
                    Console.WriteLine("Inserted standard data to row {0}: {1}->{2} {3} @ {4}",
                        i + 1,
                        StandardData[i, FFromCurrencyId],
                        StandardData[i, FToCurrencyId],
                        StandardData[i, FDateEffectiveId],
                        StandardData[i, FRateOfExchangeId]);
                }
            }

            private static void AddARow(String FromCurrency, String ToCurrency, DateTime EffectiveDate, decimal Rate)
            {
                ACorporateExchangeRateRow newRow = FMainDS.ACorporateExchangeRate.NewRowTyped();

                newRow.FromCurrencyCode = FromCurrency;
                newRow.ToCurrencyCode = ToCurrency;
                newRow.DateEffectiveFrom = EffectiveDate;
                newRow.TimeEffectiveFrom = 0;
                newRow.RateOfExchange = Rate;
                FMainDS.ACorporateExchangeRate.Rows.Add(newRow);
            }
        }

        #endregion

        #region Standard Exchange Rate Data stored in FMainDS

        /// <summary>
        /// The standard data creates 1 pair of currencies both ways
        /// There are two rates for 1900 and 2 rates for 2999 (when I won't be here!)
        /// The array items are specified oldest first, but on screen they will be newest first
        /// Also USD as To is specified before GBP but on screen it will be the other way round
        /// This is IMPORTANT for the test
        /// </summary>
        private static object[, ] StandardData =
        {
            { "GBP", "USD", "1900-06-01", 0.50m },
            { "GBP", "USD", "1900-07-01", 0.55m },
            { "GBP", "USD", "2999-06-01", 0.40m },
            { "GBP", "USD", "2999-07-01", 0.45m },
            { "USD", "GBP", "1900-06-01", 2.00m },
            { "USD", "GBP", "1900-07-01", 1.8181818182m },
            { "USD", "GBP", "2999-06-01", 2.50m },
            { "USD", "GBP", "2999-07-01", 2.2222222222m }
        };

        private const int FFromCurrencyId = 0;
        private const int FToCurrencyId = 1;
        private const int FDateEffectiveId = 2;
        private const int FRateOfExchangeId = 3;
        private const int FAllRowCount = 8;
        private const int FHiddenRowCount = 4;

        private int FCurrentDataId = 7;

        private int Row2DataId(int AGridRow)
        {
            // Based on our standard 8 rows: grid row 1->data row 7, grid row 2->data row 6 etc...
            return StandardData.GetUpperBound(0) - AGridRow + 1;
        }

        private void SelectRowInGrid(int AGridRow, int ADataRow)
        {
            TSgrdDataGridPagedTester grdTester = new TSgrdDataGridPagedTester("grdDetails");

            grdTester.SelectRow(AGridRow);
            FCurrentDataId = ADataRow;
        }

        private void SelectRowInGrid(int AGridRow)
        {
            SelectRowInGrid(AGridRow, Row2DataId(AGridRow));
        }

        private string EffectiveCurrency(int ACurrencyId)
        {
            return StandardData[FCurrentDataId, ACurrencyId].ToString();
        }

        private DateTime EffectiveDate()
        {
            return DateTime.Parse(StandardData[FCurrentDataId, FDateEffectiveId].ToString(), CultureInfo.InvariantCulture);
        }

        private decimal EffectiveRate()
        {
            return Convert.ToDecimal(StandardData[FCurrentDataId, FRateOfExchangeId]);
        }

        #endregion
    }
}