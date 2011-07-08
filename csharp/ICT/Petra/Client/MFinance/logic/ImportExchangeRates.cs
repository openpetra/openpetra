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
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using Ict.Petra.Shared.Interfaces; // Implicit reference
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Common.Data;

namespace Ict.Petra.Client.MFinance.Logic
{

    /// <summary>
    /// this provides some static functions that import
    /// daily and corporate exchange rates
    /// </summary>

    public class TImportExchangeRates
    {   

    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode">Determines whether corporate or daily exchange rates specified</param>
        public static void Import (TTypedDataTable exchangeRateDT, string mode)
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
                        ImportFromCSVFile(exchangeRateDT, dialog.FileName, RootNode, mode);
                    }
                }
            }
            
        }
        
        private static void ImportFromCSVFile(TTypedDataTable exchangeRDT, string ADataFilename, XmlNode ARootNode, string mode)
        {
            int x, y;
            
            if (mode != "Corporate" && mode != "Daily")
            {
                throw new ArgumentException("Invalid value '" + mode + "' for mode argument: Valid values are Corporate and Daily");
            }
            else if (mode == "Corporate" && exchangeRDT.GetType() !=  typeof(ACorporateExchangeRateTable))
            {
                throw new ArgumentException("Invalid type of exchangeRateDT argument for mode: 'Corporate'. Needs to be: ACorporateExchangeRateTable");
            }
            else if (mode == "Daily" && exchangeRDT.GetType() !=  typeof(ADailyExchangeRateTable))
            {
                throw new ArgumentException("Invalid type of exchangeRateDT argument for mode: 'Daily'. Needs to be: ADailyExchangeRateTable");
            }

            
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

                //TODO: Date parsing as in Petra 2.x instead of using XML date format!!!
                DateTime dateEffective = XmlConvert.ToDateTime(StringHelper.GetNextCSV(ref line, Separator, false), DateFormat);
                string ExchangeRateString = StringHelper.GetNextCSV(ref line, Separator, false).Replace(ThousandsSeparator, "").Replace(DecimalSeparator, ".");

                decimal ExchangeRate = Convert.ToDecimal(ExchangeRateString, System.Globalization.CultureInfo.InvariantCulture);

                if (mode == "Corporate" && exchangeRDT is ACorporateExchangeRateTable)
                {
                    ACorporateExchangeRateTable exchangeRateDT = (ACorporateExchangeRateTable)exchangeRDT;
    
                    // run this code in the loop twice to get ExchangeRate value and its inverse
                    for (int i = 0; i <= 1; i++)
                    {
                        //this will cause x and y to go from 0 to 1 and 1 to 0 respectively
                        x = i;
                        y = Math.Abs(i - 1);
                        
                        ACorporateExchangeRateRow exchangeRow = (ACorporateExchangeRateRow)exchangeRateDT.Rows.
                                                                    Find(new object[] { Currencies[x], Currencies[y], dateEffective});
    
                        if (exchangeRow == null)                                                                                    // remove 0 for Corporate
                        {
                            exchangeRow = (ACorporateExchangeRateRow)exchangeRateDT.NewRowTyped();
                            exchangeRow.FromCurrencyCode = Currencies[x];
                            exchangeRow.ToCurrencyCode = Currencies[y];
                            exchangeRow.DateEffectiveFrom = dateEffective;
                            exchangeRateDT.Rows.Add(exchangeRow);
                        }
        
                        if (i == 0)
                            exchangeRow.RateOfExchange = ExchangeRate;
                        else
                            exchangeRow.RateOfExchange = 1 / ExchangeRate;
                    }
                    
                }
                else if (mode == "Daily" && exchangeRDT is ADailyExchangeRateTable)
                {               
                    ADailyExchangeRateTable exchangeRateDT = (ADailyExchangeRateTable)exchangeRDT;
    
                    // run this code in the loop twice to get ExchangeRate value and its inverse
                    for (int i = 0; i <= 1; i++)
                    {
                        //this will cause x and y to go from 0 to 1 and 1 to 0 respectively
                        x = i;
                        y = Math.Abs(i - 1);
                        
                        ADailyExchangeRateRow exchangeRow = (ADailyExchangeRateRow)exchangeRateDT.Rows.
                                                                Find(new object[] { Currencies[x], Currencies[y], dateEffective, 0});
    
                        if (exchangeRow == null)                                                                                    // remove 0 for Corporate
                        {
                            exchangeRow = (ADailyExchangeRateRow)exchangeRateDT.NewRowTyped();
                            exchangeRow.FromCurrencyCode = Currencies[x];
                            exchangeRow.ToCurrencyCode = Currencies[y];
                            exchangeRow.DateEffectiveFrom = dateEffective;
                            exchangeRateDT.Rows.Add(exchangeRow);
                        }
        
                        if (i == 0)
                            exchangeRow.RateOfExchange = ExchangeRate;
                        else
                            exchangeRow.RateOfExchange = 1 / ExchangeRate;
                    }
    
                }

            }

            dataFile.Close();
        }
        
    }
}
