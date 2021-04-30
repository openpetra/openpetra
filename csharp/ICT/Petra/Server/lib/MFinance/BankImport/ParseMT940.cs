//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2021 by OM International
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
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Threading;
using System.Text;
using Ict.Common;

namespace Ict.Petra.Server.MFinance.BankImport.Logic
{
    /// <summary>
    /// parses bank statement files (Swift MT940) in Germany;
    /// for the structure of the file see
    /// https://www.db-bankline.deutsche-bank.com/download/MT940_Deutschland_Structure2002.pdf
    /// </summary>
    public class TSwiftParser
    {
        /// <summary>
        /// the parsed bank statements
        /// </summary>
        public List <TStatement>statements;
        private TStatement currentStatement = null;

        private static string WithoutLeadingZeros(string ACode)
        {
            // cut off leading zeros
            try
            {
                return Convert.ToInt64(ACode).ToString();
            }
            catch (Exception)
            {
                // IBAN or BIC
                return ACode;
            }
        }

        private void HandleSwiftData(string swiftTag, string swiftData)
        {
            if (swiftTag == "OS")
            {
                // ignore
            }
            else if (swiftTag == "20")
            {
                // 20 is used for each "page" of the statement; but we want to put all transactions together
                // the whole statement closes with 62F
                if (currentStatement == null)
                {
                    currentStatement = new TStatement();
                    // currentStatement.lines.Add(new TLine(swiftTag, swiftData));
                }
            }
            else if (swiftTag == "25")
            {
                int posSlash = swiftData.IndexOf("/");
                currentStatement.bankCode = swiftData.Substring(0, posSlash);
                currentStatement.accountCode = WithoutLeadingZeros(swiftData.Substring(posSlash + 1));
            }
            else if (swiftTag.StartsWith("60"))
            {
                // 60M is the start balance on each page of the statement.
                // 60F is the start balance of the whole statement.

                // first character is D or C
                int DebitCreditIndicator = (swiftData[0] == 'D' ? -1 : +1);

                // next 6 characters: YYMMDD
                // next 3 characters: currency
                // last characters: balance with comma for decimal point
                currentStatement.currency = swiftData.Substring(7, 3);
                decimal balance = DebitCreditIndicator * Convert.ToDecimal(swiftData.Substring(10).Replace(",",
                        Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));

                // we only want to use the first start balance
                if (swiftTag == "60F")
                {
                    currentStatement.startBalance = balance;
                    currentStatement.endBalance = balance;
                }
                else
                {
                    // check if the balance inside the statement is ok
                    // ie it fits the balance of the previous page
                    if (Convert.ToDecimal(Math.Round(currentStatement.endBalance, 2)) != Convert.ToDecimal(balance))
                    {
                        throw new Exception("start balance does not match current balance");
                    }
                }
            }
            else if (swiftTag == "28C")
            {
                // this contains the number of the statement and the number of the page
                // only use for first page
                if (currentStatement.transactions.Count == 0)
                {
                    if (swiftData.IndexOf("/") != -1)
                    {
                        currentStatement.id = swiftData.Substring(0, swiftData.IndexOf("/"));
                    }
                    else
                    {
                        // realtime statement. do not use statement number 0, because Sparkasse has 0/1 for valid statements
                        currentStatement.id = string.Empty;
                    }
                }
            }
            else if (swiftTag == "61")
            {
                TTransaction transaction = new TTransaction();

                // valuta date (YYMMDD)
                try
                {
                    transaction.valueDate = new DateTime(2000 + Convert.ToInt32(swiftData.Substring(0, 2)),
                        Convert.ToInt32(swiftData.Substring(2, 2)),
                        Convert.ToInt32(swiftData.Substring(4, 2)));
                }
                catch (ArgumentOutOfRangeException)
                {
                    // we have had the situation in the bank file with a date 30 Feb 2010.
                    // probably because the instruction by the donor is to transfer the money on the 30 day each month
                    // use the last day of the month
                    int year = 2000 + Convert.ToInt32(swiftData.Substring(0, 2));
                    int month = Convert.ToInt32(swiftData.Substring(2, 2));
                    int day = DateTime.DaysInMonth(year, month);
                    transaction.valueDate = new DateTime(year, month, day);
                }

                swiftData = swiftData.Substring(6);

                if (char.IsDigit(swiftData[0]))
                {
                    // posting date (MMDD)
                    transaction.inputDate = new DateTime(transaction.valueDate.Year,
                        Convert.ToInt32(swiftData.Substring(0, 2)),
                        Convert.ToInt32(swiftData.Substring(2, 2)));
                    swiftData = swiftData.Substring(4);
                }
                else
                {
                    transaction.inputDate = transaction.valueDate;
                }

                // debit or credit, or storno debit or credit
                int debitCreditIndicator = 0;

                if (swiftData[0] == 'R')
                {
                    // storno means: reverse the debit credit flag
                    debitCreditIndicator = (swiftData[1] == 'D' ? 1 : -1);

                    swiftData = swiftData.Substring(2);
                }
                else
                {
                    debitCreditIndicator = (swiftData[0] == 'D' ? -1 : 1);
                    swiftData = swiftData.Substring(1);
                }

                // sometimes there is something about currency
                if (Char.IsLetter(swiftData[0]))
                {
                    // just skip it for the moment
                    swiftData = swiftData.Substring(1);
                }

                // the amount, finishing with N
                transaction.amount =
                    debitCreditIndicator * Convert.ToDecimal(swiftData.Substring(0, swiftData.IndexOf("N")).Replace(",",
                            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                currentStatement.endBalance += transaction.amount;
                swiftData = swiftData.Substring(swiftData.IndexOf("N"));

                // Geschaeftsvorfallcode
                // transaction.typecode = swiftData.Substring(1, 3);
                swiftData = swiftData.Substring(4);

                // the following sub fields are ignored
                // optional: customer reference; ends with //
                // optional: bank reference; ends with CR/LF
                // something else about original currency and transaction fees

                currentStatement.transactions.Add(transaction);
            }
            else if (swiftTag == "86")
            {
                TTransaction transaction = currentStatement.transactions[currentStatement.transactions.Count - 1];

                // Geschaeftsvorfallcode
                transaction.typecode = swiftData.Substring(0, 3);
                swiftData = swiftData.Substring(3);
                char separator = swiftData[0];
                swiftData = swiftData.Substring(1);
                string[] elements = swiftData.Split(new char[] { separator });

                foreach (string element in elements)
                {
                    int key = 0;
                    string value = element;

                    try
                    {
                        key = Convert.ToInt32(element.Substring(0, 2));
                        value = element.Substring(2);
                    }
                    catch (Exception)
                    {
                        // if there is a question mark in the description, then we get here
                    }

                    if (key == 0)
                    {
                        // Buchungstext
                        transaction.text = value;
                    }
                    else if (key == 10)
                    {
                        // Primanotennummer; ignore at the moment
                    }
                    else if ((key >= 11) && (key <= 19))
                    {
                        // ignore, unknown meaning
                    }
                    else if ((key >= 20) && (key <= 29))
                    {
                        // do not insert a space between description lines
                        transaction.description += value;
                    }
                    else if (key == 30)
                    {
                        transaction.bankCode = value;
                    }
                    else if (key == 31)
                    {
                        // could use WithoutLeadingZeros, but then this must be stored in p_banking_details also without leading zeros
                        // which is not the case at the moment, and would increase complexity of sql queries
                        transaction.accountCode = value;
                    }
                    else if ((key == 32) || (key == 33))
                    {
                        transaction.partnerName += value;
                    }
                    else if (key == 34)
                    {
                        // Textschlüsselergänzung; ignore
                    }
                    else if ((key == 35) || (key == 36))
                    {
                        // Empfängername
                        transaction.description += value;
                    }
                    else if ((key >= 60) && (key <= 63))
                    {
                        transaction.description += value;
                    }
                    else
                    {
                        throw new Exception("unknown key " + key.ToString());
                    }
                }
            }
            else if (swiftTag.StartsWith("62"))
            {
                // 62M: finish page
                // 62F: finish statement
                int debitCreditIndicator = (swiftData[0] == 'D' ? -1 : 1);
                swiftData = swiftData.Substring(1);

                // posting date YYMMDD
                DateTime postingDate = new DateTime(2000 + Convert.ToInt32(swiftData.Substring(0, 2)),
                    Convert.ToInt32(swiftData.Substring(2, 2)),
                    Convert.ToInt32(swiftData.Substring(4, 2)));
                swiftData = swiftData.Substring(6);

                // currency
                swiftData = swiftData.Substring(3);

                // sometimes, this line is the last line, and it has -NULNULNUL at the end
                if (swiftData.Contains("-\0"))
                {
                    swiftData = swiftData.Substring(0, swiftData.IndexOf("-\0"));
                }

                // end balance
                decimal shouldBeBalance = debitCreditIndicator * Convert.ToDecimal(swiftData.Replace(",",
                        Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));

                currentStatement.endBalance = Math.Round(currentStatement.endBalance, 2);

                if (Convert.ToDecimal(Math.Round(currentStatement.endBalance, 2)) != Convert.ToDecimal(shouldBeBalance))
                {
                    throw new Exception("end balance does not match" +
                        " last transaction was: " + currentStatement.transactions[currentStatement.transactions.Count - 1].partnerName +
                        " balance is: " + currentStatement.endBalance.ToString() +
                        " but should be: " + shouldBeBalance.ToString());
                }

                if (swiftTag == "62F")
                {
                    currentStatement.date = postingDate;
                    statements.Add(currentStatement);
                    currentStatement = null;
                }
            }
            else if (swiftTag == "64")
            {
                // valutensaldo; ignore
            }
            else if (swiftTag == "65")
            {
                // future valutensaldo; ignore
            }
            else
            {
                Console.WriteLine("swiftTag " + swiftTag + " is unknown");
            }
        }

        private static string ReadLine(ref string AFileContent)
        {
            Int32 counter;

            for (counter = 0; counter < AFileContent.Length; counter++)
            {
                if ((AFileContent[counter] == (char)10) || (AFileContent[counter] == (char)13))
                {
                    break;
                }
            }

            string line = AFileContent.Substring(0, counter);

            if ((counter < AFileContent.Length) && (AFileContent[counter] == (char)13))
            {
                counter++;
            }

            if ((counter < AFileContent.Length) && (AFileContent[counter] == (char)10))
            {
                counter++;
            }

            if (counter < AFileContent.Length)
            {
                AFileContent = AFileContent.Substring(counter);
            }
            else
            {
                AFileContent = String.Empty;
            }

            line = line.Trim();
            line = line.Replace("™", "Ö");
            line = line.Replace("š", "Ü");
            line = line.Replace("Ž", "Ä");
            line = line.Replace("á", "ß");
            line = line.Replace("\\", "Ö");
            line = line.Replace("]", "Ü");
            line = line.Replace("[", "Ä");
            line = line.Replace("~", "ß");
            return line;
        }

        /// <summary>
        /// processing MT940 file
        /// </summary>
        public void ProcessFileContent(string AContent)
        {
            Int32 LineCounter = 0;

            try
            {
                string swiftTag = "";
                string swiftData = "";
                statements = new List <TStatement>();
                currentStatement = null;

                while (AContent.Length > 0)
                {
                    string line = ReadLine(ref AContent);
                    LineCounter++;

                    if ((line.Length > 0) && (!line.StartsWith("-")))
                    {
                        // a swift chunk starts with a swiftTag, which is between colons
                        if (line.StartsWith(":"))
                        {
                            // process previously read swift chunk
                            if (swiftTag.Length > 0)
                            {
                                // Console.WriteLine(" Tag " + swiftTag + " Data " + swiftData);
                                HandleSwiftData(swiftTag, swiftData);
                            }

                            int posColon = line.IndexOf(":", 2);
                            swiftTag = line.Substring(1, posColon - 1);
                            swiftData = line.Substring(posColon + 1);
                        }
                        else
                        {
                            // the swift chunk is spread over several lines
                            swiftData = swiftData + line;
                        }
                    }
                }

                if (swiftTag.Length > 0)
                {
                    HandleSwiftData(swiftTag, swiftData);
                }
            }
            catch (Exception e)
            {
                throw new Exception(
                    "problem with MT940 file; line: " + LineCounter.ToString() + "; " + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        /// <summary>
        /// write xml file with data imported from MT940 file
        /// </summary>
        /// <param name="filename"></param>
        public void ExportToXML(string filename)
        {
            XmlTextWriter textWriter = new XmlTextWriter(filename, Encoding.UTF8);

            textWriter.Formatting = System.Xml.Formatting.Indented;
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("BankStatements");

            foreach (TStatement stmt in statements)
            {
                textWriter.WriteStartElement("BankStatement");
                textWriter.WriteElementString("id", stmt.id);
                textWriter.WriteElementString("date", "20" + stmt.date.ToShortDateString());
                textWriter.WriteElementString("accountcode", stmt.accountCode);
                textWriter.WriteElementString("banksortcode", stmt.bankCode);
                textWriter.WriteElementString("currency", stmt.currency);
                textWriter.WriteElementString("startBalance", (Math.Floor(stmt.startBalance * 100)).ToString());
                textWriter.WriteElementString("endBalance", (Math.Floor(stmt.endBalance * 100)).ToString());

                foreach (TTransaction t in stmt.transactions)
                {
                    textWriter.WriteStartElement("Transaction");
                    textWriter.WriteElementString("partnerName", t.partnerName);
                    textWriter.WriteElementString("accountCode", t.accountCode);
                    textWriter.WriteElementString("bankCode", t.bankCode);
                    textWriter.WriteElementString("description", t.description);
                    textWriter.WriteElementString("text", t.text);
                    textWriter.WriteElementString("typecode", t.typecode);
                    textWriter.WriteElementString("amount", (Math.Floor(t.amount * 100)).ToString());
                    textWriter.WriteEndElement();
                }

                textWriter.WriteEndElement();
            }

            textWriter.WriteEndElement();
            textWriter.WriteEndDocument();
            textWriter.Close();
        }
    }
}
