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
using System.Xml;
using System.IO;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.MFinance.Logic;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupDailyExchangeRate
    {
        private void NewRow(System.Object sender, EventArgs e)
        {
            // TODO
        }

        private void DeleteRow(System.Object sender, EventArgs e)
        {
            // TODO
        }

        private void GetDetailDataFromControlsManual(ADailyExchangeRateRow ARow)
        {
            TExchangeRateCache.ResetCache();
        }

        private void Import(System.Object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = Catalog.GetString("Import exchange rates from spreadsheet file");
            dialog.Filter = Catalog.GetString("Spreadsheet files (*.csv)|*.csv");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string directory = Path.GetDirectoryName(dialog.FileName);
                string[] ymlFiles = Directory.GetFiles(directory, "*.yml");
                string definitionFileName = String.Empty;

                if (ymlFiles.Length == 1)
                {
                    definitionFileName = ymlFiles[0];
                }
                else
                {
                    // show another open file dialog for the description file
                    OpenFileDialog dialogDefinitionFile = new OpenFileDialog();
                    dialogDefinitionFile.Title = Catalog.GetString("Please select a yml file that describes the content of the spreadsheet");
                    dialogDefinitionFile.Filter = Catalog.GetString("Data description files (*.yml)|*.yml");

                    if (dialogDefinitionFile.ShowDialog() == DialogResult.OK)
                    {
                        definitionFileName = dialogDefinitionFile.FileName;
                    }
                }

                if (File.Exists(definitionFileName))
                {
                    TYml2Xml parser = new TYml2Xml(definitionFileName);
                    XmlDocument dataDescription = parser.ParseYML2XML();
                    XmlNode RootNode = TXMLParser.FindNodeRecursive(dataDescription.DocumentElement, "RootNode");

                    if (Path.GetExtension(dialog.FileName).ToLower() == ".csv")
                    {
                        ImportFromCSVFile(dialog.FileName, RootNode);
                    }
                }
            }
        }

        private void ImportFromCSVFile(string ADataFilename, XmlNode ARootNode)
        {
            StreamReader dataFile = new StreamReader(ADataFilename, System.Text.Encoding.Default);

            string Separator = TXMLParser.GetAttribute(ARootNode, "Separator");
            string DateFormat = TXMLParser.GetAttribute(ARootNode, "DateFormat");
            string ThousandsSeparator = TXMLParser.GetAttribute(ARootNode, "ThousandsSeparator");
            string DecimalSeparator = TXMLParser.GetAttribute(ARootNode, "DecimalSeparator");

            // assumes date in first column, exchange rate in second column
            // picks the names of the currencies from the file name: CUR1_CUR2.csv
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(ADataFilename);

            if (!fileNameWithoutExtension.Contains("_"))
            {
                MessageBox.Show(Catalog.GetString("Cannot import exchange rates, please name the file after the currencies involved, eg. KES_EUR.csv"));
            }

            string[] Currencies = fileNameWithoutExtension.Split(new char[] { '_' });

            // TODO: check for valid currency codes? at the moment should fail on foreign key
            // TODO: disconnect the grid from the datasource to avoid flickering?

            while (!dataFile.EndOfStream)
            {
                string line = dataFile.ReadLine();

                DateTime dateEffective = XmlConvert.ToDateTime(StringHelper.GetNextCSV(ref line, Separator, false), DateFormat);
                string ExchangeRateString = StringHelper.GetNextCSV(ref line, Separator, false).
                                            Replace(ThousandsSeparator, "").
                                            Replace(DecimalSeparator, ".");

                decimal ExchangeRate = Convert.ToDecimal(ExchangeRateString, System.Globalization.CultureInfo.InvariantCulture);

                ADailyExchangeRateRow exchangeRow =
                    (ADailyExchangeRateRow)FMainDS.ADailyExchangeRate.Rows.Find(new object[] { Currencies[0], Currencies[1], dateEffective,
                                                                                               0 });

                if (exchangeRow == null)
                {
                    exchangeRow = FMainDS.ADailyExchangeRate.NewRowTyped();
                    exchangeRow.FromCurrencyCode = Currencies[0];
                    exchangeRow.ToCurrencyCode = Currencies[1];
                    exchangeRow.DateEffectiveFrom = dateEffective;
                    FMainDS.ADailyExchangeRate.Rows.Add(exchangeRow);
                }

                exchangeRow.RateOfExchange = ExchangeRate;
            }

            dataFile.Close();

            FPetraUtilsObject.SetChangedFlag();
        }
    }
}