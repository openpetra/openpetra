//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using NUnit.Framework;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using Ict.Common;
using Ict.Common.Data;

namespace Ict.Common.Testing
{
    ///  This is a testing program for Ict.Common.dll
    [TestFixture]
    public class TTestCommon
    {
        /// init the test
        [SetUp]
        public void Init()
        {
            new TLogging("test.log");
        }

        /// find matching quote in a string. considers nested quotes
        public static int FindMatchingQuote(String s)
        {
            string localstr;

            localstr = s.Substring(1);

            while (localstr.IndexOf("\"\"") != -1)
            {
                localstr = localstr.Replace("\"\"", "  ");
            }

            return localstr.IndexOf('"') + 1;
        }

        static Int16 tstNr;

        /// create a name for a test
        public static string FormatTestName(string s)
        {
            string ReturnValue;

            ReturnValue = CultureInfo.CurrentCulture.Name + ' ' + tstNr.ToString() + ' ' + s;
            tstNr++;
            return ReturnValue;
        }

        /// test dates
        [Test]
        public void TestDates()
        {
            DateTime d;
            DateTime d1;
            DateTime d2;

            d = new DateTime(2004, 1, 1);
            d1 = new DateTime(d.Year, d.Month, 1).AddMonths(-1);
            d2 = new DateTime(d.Year, d.Month, 1).AddDays(-1);
            Assert.AreEqual(new DateTime(2003, 12, 1), d1, "start date");
            Assert.AreEqual(new DateTime(2003, 12, 31), d2, "end date");
        }

        /// test formatting currency values
        [Test]
        public void TestStringHelperFormatCurrency()
        {
            String DecimalSeparator;
            String ThousandsOperator;
            ArrayList cultures;
            TVariant v;

            cultures = new ArrayList();
            cultures.Add("en-GB");
            cultures.Add("en-US");
            cultures.Add("de-DE");

            /* opposite meaning of , and . */
            cultures.Add("ru-RU");

            /* funny thousand separator: space */
            foreach (string s in cultures)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(s, false);

                /* Console.WriteLine('currently testing: ' + CultureInfo.CurrentCulture.Name); */
                DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
                ThousandsOperator = CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                tstNr = 1;
                Assert.AreEqual("2" + DecimalSeparator + "23",
                    StringHelper.FormatCurrency(new TVariant(2.23), "Currency"), FormatTestName("variant parameter; positive; format: Currency"));
                Assert.AreEqual("0" + DecimalSeparator + "00",
                    StringHelper.FormatCurrency(new TVariant(0.00), "Currency"), FormatTestName("variant parameter; 0; format: Currency"));
                Assert.AreEqual("(2" + DecimalSeparator + "23)",
                    StringHelper.FormatCurrency(new TVariant(-2.23), "Currency"), FormatTestName("variant parameter; negative; format: Currency"));
                Assert.AreEqual('2' + DecimalSeparator + "23", StringHelper.FormatCurrency(new TVariant(
                            2.23), "#,##0.00;-#,##0.00;0.00; "), FormatTestName("variant parameter; positive; format: Access Style"));
                Assert.AreEqual('0' + DecimalSeparator + "00", StringHelper.FormatCurrency(new TVariant(
                            0.00), "#,##0.00;-#,##0.00;0.00; "), FormatTestName("variant parameter; 0; format: Access Style"));
                Assert.AreEqual("-2" + DecimalSeparator + "23", StringHelper.FormatCurrency(new TVariant(
                            -2.23), "#,##0.00;-#,##0.00; ; "), FormatTestName("variant parameter; negative; format: Access Style"));
                Assert.AreEqual("",
                    StringHelper.FormatCurrency(new TVariant(-2.23),
                        "#,##0.00; ; ; "), FormatTestName("variant parameter; negative; format: Access Style; only display positive values"));
                Assert.AreEqual("-2" + DecimalSeparator + "23",
                    StringHelper.FormatCurrency(new TVariant(-2.23),
                        ";-#,##0.00; ; "), FormatTestName("variant parameter; negative; format: Access Style; only display negative values"));
                Assert.AreEqual('2' + DecimalSeparator + "00",
                    StringHelper.FormatCurrency(new TVariant(-2.00),
                        ";#,##0.00; ; "),
                    FormatTestName(
                        "variant parameter; negative; format: Access Style; only display negative values, but without sign; also test if 0 are added"));
                Assert.AreEqual('2' + DecimalSeparator + "00",
                    StringHelper.FormatCurrency(new TVariant(-2),
                        ";#,##0.00; ; "),
                    FormatTestName(
                        "variant parameter; negative; format: Access Style; only display negative values, but without sign; also test if 0 are added; integer value"));
                Assert.AreEqual('2' + DecimalSeparator + "23",
                    StringHelper.FormatCurrency(new TVariant(2.23),
                        "->>>,>>>,>>>,>>9.99"), FormatTestName("variant parameter; positive; format: Progress Style"));
                Assert.AreEqual('0' + DecimalSeparator + "00",
                    StringHelper.FormatCurrency(new TVariant(0.00),
                        "->>>,>>>,>>>,>>9.99"), FormatTestName("variant parameter; 0; format: Progress Style"));
                Assert.AreEqual("-2" + DecimalSeparator + "23", StringHelper.FormatCurrency(new TVariant(
                            -2.23), "->>>,>>>,>>>,>>9.99"), FormatTestName("variant parameter; negative; format: Progress Style"));
                Assert.AreEqual("2",
                    StringHelper.FormatCurrency(new TVariant(2.23),
                        "->>>,>>>,>>>,>>9"), FormatTestName("variant parameter; positive; format: Progress Style, no decimals"));
                Assert.AreEqual("0",
                    StringHelper.FormatCurrency(new TVariant(0.00),
                        "->>>,>>>,>>>,>>9"), FormatTestName("variant parameter; 0; format: Progress Style, no decimals"));
                Assert.AreEqual("-2",
                    StringHelper.FormatCurrency(new TVariant(-2.23),
                        "->>>,>>>,>>>,>>9"), FormatTestName("variant parameter; negative; format: Progress Style, no decimals"));
                Assert.AreEqual("002" + DecimalSeparator + "23", StringHelper.FormatCurrency(new TVariant(
                            2.23), "->>>,>>>,>>>,999.99"), FormatTestName("variant parameter; positive; format: Progress Style, leading zeros"));
                Assert.AreEqual("000" + DecimalSeparator + "00", StringHelper.FormatCurrency(new TVariant(
                            0.00), "->>>,>>>,>>>,999.99"), FormatTestName("variant parameter; 0; format: Progress Style, leading zeros"));
                Assert.AreEqual("-002" + DecimalSeparator + "23", StringHelper.FormatCurrency(new TVariant(
                            -2.23), "->>>,>>>,>>>,999.99"), FormatTestName("variant parameter; negative; format: Progress Style, leading zeros"));
                Assert.AreEqual("2" + ThousandsOperator + "345" + DecimalSeparator + "23",
                    StringHelper.FormatCurrency(new TVariant(2345.23),
                        "->>>,>>>,>>>,>>9.99"), FormatTestName("variant parameter; positive; format: Progress Style, thousand separator"));
                Assert.AreEqual('2' + DecimalSeparator + "23",
                    StringHelper.FormatCurrency(2.23M, "->>>,>>>,>>>,>>9.99"), FormatTestName("decimal parameter"));
                Assert.AreEqual("223", StringHelper.FormatCurrency(223234, "#;(#);0;"), FormatTestName("just display the thousands"));
                Assert.AreEqual("0027045678", StringHelper.FormatCurrency(27045678, "0000000000;;;"), FormatTestName("partner key"));
                Assert.AreEqual("3%", StringHelper.FormatCurrency(3.0M, "0%;-0%;0;"), FormatTestName("percentage"));
                Assert.AreEqual("0029112233", StringHelper.FormatCurrency(29112233, "partnerkey"), FormatTestName("partnerkey"));
                Assert.AreEqual("31" + DecimalSeparator + "31%",
                    StringHelper.FormatCurrency(31.3053682781472M, "percentage2decimals"), FormatTestName("percentage2decimals"));
                Assert.AreEqual("31" + DecimalSeparator + "31%", StringHelper.FormatCurrency(new TVariant(
                            31.3053682781472), "percentage2decimals"), FormatTestName("percentage2decimals TVariant"));
                Assert.AreEqual("31" + DecimalSeparator + "31%",
                    StringHelper.FormatCurrency(TVariant.DecodeFromString(new TVariant(
                                31.3053682781472).EncodeToString()), "percentage2decimals"), FormatTestName("percentage2decimals TVariant Encoded"));
                Assert.AreEqual("31" + DecimalSeparator + "31%",
                    StringHelper.FormatCurrency(TVariant.DecodeFromString(new TVariant(31.3053682781472M,
                                "percentage2decimals").EncodeToString()),
                        "percentage2decimals"), FormatTestName("percentage2decimals TVariant Encoded 2"));
                v = new TVariant(31.3053682781472);
                v.FormatString = "percentage2decimals";
                Assert.AreEqual("eDecimal:percentage2decimals:4629504895489138888", v.EncodeToString(),
                    FormatTestName("percentage2decimals encodetostring"));
                Assert.AreEqual("31" + DecimalSeparator + "31%", StringHelper.FormatCurrency(TVariant.DecodeFromString(
                            v.EncodeToString()), ""), FormatTestName("percentage2decimals TVariant Encoded 3"));
                v = new TVariant(31);
                v.FormatString = "percentage2decimals";
                Assert.AreEqual("eInteger:percentage2decimals:31", v.EncodeToString(), FormatTestName("percentage2decimals encodetostring integer"));
            }
        }

        /// test csv operations (comma separated value lists)
        [Test]
        public void TestStringHelperCSVList()
        {
            const String EMPTY = "";
            const String UMLAUT = "B�cker";
            const String QUOTEKOMMA = "test\",";
            const String QUOTE = "test\"";
            const String SIMPLE = "test";
            const String KOMMA = "test, hallo";
            const String QUOTES = "test\"tst";
            const String DATE = "24/03/1999";
            const String ACCOUNTCODE = "0400";

            String[] myTest;
            Int16 i;
            string s;
            string s2;
            string longStringBug672;
            Assert.AreEqual("eString:test", StringHelper.AddCSV("", "eString:test", "|"), "Add to empty csv string");
            Assert.AreEqual("a, test ", StringHelper.AddCSV(StringHelper.AddCSV("", "a", ","), " test "), "add string with spaces");
            s = " test ,a";
            Assert.AreEqual(" test ", StringHelper.GetNextCSV(ref s), "get string with spaces");

            s = "shortdesc=\"Equipment, General\",longdesc=Equipment";
            Assert.AreEqual("shortdesc=\"Equipment, General\"", StringHelper.GetNextCSV(ref s), "get string with quotes that are not at the start");
            Assert.AreEqual("longdesc=Equipment", StringHelper.GetNextCSV(ref s), "after string with quotes that are not at the start");

            /* ,B�cker,"test"",","test""",test,"test, hallo","test""tst","24/03/1999","0400" */
            myTest = new String[] {
                EMPTY, UMLAUT, QUOTEKOMMA, QUOTE, SIMPLE, KOMMA, QUOTES, DATE, ACCOUNTCODE
            };
            s = "";

            for (i = 0; i <= myTest.Length - 1; i += 1)
            {
                s = StringHelper.AddCSV(s, myTest[i]);
            }

            Assert.AreEqual(" ," + UMLAUT + ",\"" +
                QUOTEKOMMA.Replace("\"", "\"\"") + "\",\"" +
                QUOTE.Replace("\"", "\"\"") + "\"," + SIMPLE + ",\"" + KOMMA + "\",\"" +
                QUOTES.Replace("\"", "\"\"") + "\"," + DATE + ",\"" + ACCOUNTCODE + "\"", s, "full list not correctly built");
            i = 0;

            while (s.Length > 0)
            {
                Assert.AreEqual(myTest[i].Trim(), StringHelper.GetNextCSV(ref s).Trim(), "problem with reading one element");
                i++;
            }

            s = "eDateTime:29/03/2004";
            Assert.AreEqual("eDateTime", StringHelper.GetNextCSV(ref s, ":"), "read CSV 1");
            Assert.AreEqual("29/03/2004", StringHelper.GetNextCSV(ref s, ":"), "read CSV 2");
            s = "";
            s = StringHelper.AddCSV(s, "test", ",");
            s = StringHelper.AddCSV(s, "\" )  ))", ",");
            Assert.AreEqual("test,\"\"\" )  ))\"", s, "quote at beginning of csv value");
            s2 = "test";
            s2 = StringHelper.AddCSV(s2, s);
            Assert.AreEqual("test,\"test,\"\"\"\"\"\" )  ))\"\"\"", s2, "cascading csv lists");
            Assert.AreEqual("test", StringHelper.GetNextCSV(ref s2, ","), "cascading csv lists 2");
            Assert.AreEqual(s, StringHelper.GetNextCSV(ref s2, ","), "cascading csv lists 3");
            Assert.AreEqual(1, FindMatchingQuote("\"\""), "Test 1");
            Assert.AreEqual(4, FindMatchingQuote("\"   \""), "Test 2");
            Assert.AreEqual(8, FindMatchingQuote("\" \"\" \"\" \""), "Test 3");
            Assert.AreEqual(8, FindMatchingQuote("\" \"\" \"\" \""), "Test 3");
            Assert.AreEqual(19, FindMatchingQuote("\"eString:\"\" test \"\"\"|\"eDateTime:29/03/2004\""), "Test 4");
            s = "\"eComposite::eString:test|\"\"eString:\"\"\"\" test \"\"\"\"\"\"|\"\"eDateTime:29/03/2004\"\"\"";
            Assert.AreEqual("eComposite::eString:test|\"eString:\"\" test \"\"\"|\"eDateTime:29/03/2004\"", StringHelper.GetNextCSV(ref s,
                    "|"), "split composite CSV 1a");
            longStringBug672 =
                "Actual vs Budget with % Variance\\, (Quarter)3 even longer and longer and longer and longer\\, testActual vs Budget with % VarianceActual vs Budget with % Variance,"
                +
                "Actual vs Budget with % Variance\\, (Quarter)3 even longer and longer and longer and longer\\, testActual vs Budget with % Variance,"
                +
                "Actual vs Budget with % Variance\\, (Quarter)3 even longer and longer and longer and longer\\, test," + "My office budget report," +
                "Actual vs Budget with % Variance (Month)";
            Assert.AreEqual(5, StringHelper.StrSplit(longStringBug672, ",").Count, "Test bug 672, strsplit, number of elements");
            Assert.AreEqual(159, StringHelper.StrSplit(longStringBug672, ",")[0].Length, "Test bug 672, strsplit, length of first element");

            string testAccountCodeLeadingZero = StringHelper.AddCSV("test", "0100");
            Assert.AreEqual("0100", StringHelper.GetCSVValue(testAccountCodeLeadingZero,
                    1), "Integers with leading zero should be treated as string; for account codes");

            string testListSeparator2Spaces = "\"Test  Hallo\"  0  \"\"  0";
            Assert.AreEqual("Test  Hallo", StringHelper.GetNextCSV(ref testListSeparator2Spaces, "  "));
            Assert.AreEqual("0", StringHelper.GetNextCSV(ref testListSeparator2Spaces, "  "));
            Assert.AreEqual("", StringHelper.GetNextCSV(ref testListSeparator2Spaces, "  "));
            Assert.AreEqual("0", StringHelper.GetNextCSV(ref testListSeparator2Spaces, "  "));
        }

        /// test TVariant and dates
        [Test]
        public void TestVariantDates()
        {
            CultureInfo oldCulture;

            oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB", false);

            /* console.writeLine(CultureInfo.CurrentCulture.TwoLetterISOLanguageName); */
            Assert.AreEqual("29/03/2004", new TVariant(new DateTime(2004, 03, 29)).ToString(), "Problem A date GB");
            Assert.AreEqual("29-MAR", new TVariant(new DateTime(2004, 03, 29)).ToFormattedString("dayofyear"), "Problem A2 day of year GB");
            Assert.AreEqual("29-MAR", new TVariant(new DateTime(2004, 03, 29), "dayofyear").ToFormattedString(
                    ""), "Problem A2 day of year formatstring GB");
            Assert.AreEqual("29/03/" + DateTime.Now.Year.ToString(), new TVariant(new DateTime(2004, 03, 29)).ToFormattedString("dayofyear",
                    "CSV"), "Problem A2 day of year CSV GB");
            Assert.AreEqual(new TVariant("#20040329#").ToString(), new TVariant(new DateTime(2004, 03, 29)).ToString(), "Problem B date GB");
            Assert.AreEqual(new TVariant("#20040731#").ToString(), new TVariant(new DateTime(2004, 07, 31)).ToString(), "Problem C date GB");
            Assert.AreEqual("29-MAR-2004", StringHelper.DateToLocalizedString(new TVariant(new DateTime(2004, 03, 29)).ToDate()), "Problem D date GB");
            Assert.AreEqual("eDateTime:31/07/2004", new TVariant("#20040731#").EncodeToString(), "EncodeToString GB");
            Assert.AreEqual("29-MAR-2004", new TVariant(new DateTime(2004, 03, 29), "formatteddate").ToFormattedString(
                    ""), "Problem formatting dates");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE", false);

            /* console.writeLine(CultureInfo.CurrentCulture.TwoLetterISOLanguageName); */
            Assert.AreEqual("29.03.2004", new TVariant(new DateTime(2004, 03, 29)).ToString(), "Problem A date DE");
            Assert.AreEqual("29-MRZ", new TVariant(new DateTime(2004, 03, 29)).ToFormattedString("dayofyear"), "Problem A2 day of year DE");
            Assert.AreEqual("29.03." + DateTime.Now.Year.ToString(), new TVariant(new DateTime(2004, 03, 29)).ToFormattedString("dayofyear",
                    "CSV"), "Problem A2 day of year CSV DE");
            Assert.AreEqual(new TVariant("#20040329#").ToString(), new TVariant(new DateTime(2004, 03, 29)).ToString(), "Problem B date DE");
            Assert.AreEqual(new TVariant("#20040731#").ToString(), new TVariant(new DateTime(2004, 07, 31)).ToString(), "Problem C date DE");

            /* To make this work, we should use short month names from local array, similar to GetLongMonthName; see the comment in Ict.Common.StringHelper, DateToLocalizedString */
            /* Assert.AreEqual('29M�R2004', DateToLocalizedString(TVariant.Create(DateTime.Create(2004,03,29)).ToDate()),'Problem D date DE'); */
            Assert.AreEqual("29-MRZ-2004", StringHelper.DateToLocalizedString(new TVariant(new DateTime(2004, 03, 29)).ToDate()), "Problem D date DE");
            Assert.AreEqual("eDateTime:31/07/2004", new TVariant("#20040731#").EncodeToString(), "EncodeToString DE");
            Assert.AreEqual("29-MRZ-2004", StringHelper.DateToLocalizedString(new TVariant("2004-03-29 00:00:00").ToDate()), "sqlite date value");
            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        /// test TVariant composites
        [Test]
        public void TestVariantComposite()
        {
            CultureInfo oldCulture;
            TVariant v;
            TVariant v2;
            TVariant v3;
            String encodedString;
            String s;

            oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB", false);
            v = new TVariant("test");
            Assert.AreEqual("eString:test", v.EncodeToString(), "before EncodeToString1");
            v.Add(new TVariant(true));
            Assert.AreEqual("eComposite::\"eString:test|eBoolean:true\"", v.EncodeToString(), "EncodeToString1");
            v.Add(new TVariant(2.23M, "Currency"));
            v.Add(new TVariant(2.23M));
            v.Add(new TVariant(2));
            Assert.AreEqual("eComposite::\"eString:test|eBoolean:true|eCurrency:Currency:4612203932384535511|eDecimal:4612203932384535511|eInteger:2\"",
                v.EncodeToString(),
                "EncodeToString2");
            v.Add(new TVariant(" test "));
            Assert.AreEqual(
                "eComposite::\"eString:test|eBoolean:true|eCurrency:Currency:4612203932384535511|eDecimal:4612203932384535511|eInteger:2|eString: test \"",
                v.EncodeToString(),
                "EncodeToString3");
            v.Add(new TVariant(new DateTime(2004, 03, 29)));
            v2 = new TVariant(v); // copy constructor
            Assert.AreEqual(
                "eComposite::\"eString:test|eBoolean:true|eCurrency:Currency:4612203932384535511|eDecimal:4612203932384535511|eInteger:2|eString: test |eDateTime:29/03/2004\"",
                v2.EncodeToString(),
                "EncodeToString4");
            Assert.AreEqual("eComposite::\"eString:test|eBoolean:true\"", TVariant.DecodeFromString(
                    "eComposite::\"eString:test|eBoolean:true\"").EncodeToString(), "DecodeFromString");
            Assert.AreEqual("eComposite::\"eString: test |eBoolean:true\"", TVariant.DecodeFromString(
                    "eComposite::\"eString: test |eBoolean:true\"").EncodeToString(), "DecodeFromString with spaces in string");
            Assert.AreEqual(v2.EncodeToString(), TVariant.DecodeFromString(v2.EncodeToString()).EncodeToString(), "DecodeFromString2");
            v = new TVariant();
            v.Add(new TVariant(2));
            Assert.AreEqual(2.00, v.ToDouble(), "Variant Composite Double");
            v.Add(new TVariant(" test "));
            v3 = new TVariant();
            v3.Add(new TVariant(1));
            v3.Add(v);
            v3.Add(v2);
            s =
                "eInteger:1|\"eComposite::\"\"eInteger:2|\"\"\"\"eString:\"\"\"\"\"\"\"\" test \"\"\"\"\"\"\"\"\"\"\"\"\"\"\"|\"eComposite::\"\"eString:test|eBoolean:true|eCurrency:Currency:4612203932384535511|eDecimal:4612203932384535511|eInteger:2|\"\"\"\"eString:\"\"\"\"\"\"\"\" test \"\"\"\"\"\"\"\"\"\"\"\"|eDateTime:29/03/2004\"\"\"";
            Assert.AreEqual("eInteger:1", StringHelper.GetNextCSV(ref s, "|"), "split composite CSV 1");
            Assert.AreEqual("eComposite::\"eInteger:2|\"\"eString:\"\"\"\" test \"\"\"\"\"\"\"", StringHelper.GetNextCSV(ref s, "|"), "split composite CSV 2");
            Assert.AreEqual(
                "\"eComposite::\"\"eString:test|eBoolean:true|eCurrency:Currency:4612203932384535511|eDecimal:4612203932384535511|eInteger:2|\"\"\"\"eString:\"\"\"\"\"\"\"\" test \"\"\"\"\"\"\"\"\"\"\"\"|eDateTime:29/03/2004\"\"\"",
                s,
                "split composite CSV 4");
            Assert.AreEqual(
                "eComposite::\"eString:test|eBoolean:true|eCurrency:Currency:4612203932384535511|eDecimal:4612203932384535511|eInteger:2|\"\"eString:\"\"\"\" test \"\"\"\"\"\"|eDateTime:29/03/2004\"",
                StringHelper.GetNextCSV(ref s, "|"),
                "split composite CSV 6");
            Assert.AreEqual(
                "eComposite::\"eInteger:1|\"\"eComposite::\"\"\"\"eInteger:2|eString: test \"\"\"\"\"\"|\"\"eComposite::\"\"\"\"eString:test|eBoolean:true|eCurrency:Currency:4612203932384535511|eDecimal:4612203932384535511|eInteger:2|eString: test |eDateTime:29/03/2004\"\"\"\"\"\"\"",
                v3.EncodeToString(),
                "EncodeToString1 with Composite containing Composite");
            Assert.AreEqual(v3.EncodeToString(), TVariant.DecodeFromString(
                    v3.EncodeToString()).EncodeToString(), "DecodeFromString with Composite containing Composite");
            v = new TVariant();
            v.Add(new TVariant(2));
            v.Add(new TVariant(" test"));
            v3 = new TVariant();
            v3.Add(v);
            v3.Add(v2);
            Assert.AreEqual(
                "eComposite::\"eInteger:2|eString: test|\"\"eComposite::\"\"\"\"eString:test|eBoolean:true|eCurrency:Currency:4612203932384535511|eDecimal:4612203932384535511|eInteger:2|eString: test |eDateTime:29/03/2004\"\"\"\"\"\"\"",
                v3.EncodeToString(),
                "EncodeToString2 with Composite containing Composite");
            Assert.AreEqual(v3.EncodeToString(), TVariant.DecodeFromString(
                    v3.EncodeToString()).EncodeToString(), "DecodeFromString with Composite containing Composite");
            v = new TVariant();
            v2 = new TVariant();
            v2.Add(new TVariant(""), "", false);
            v.Add(v2);
            v2 = new TVariant();
            v2.Add(new TVariant(""), "", false);
            v.Add(v2);
            Assert.AreEqual("eComposite::\"eComposite::\"\"eString:\"\"|eComposite::\"\"eString:\"\"\"",
                            v.EncodeToString(), "Composite containing two Composites encode");
            Assert.AreEqual(v.EncodeToString(), TVariant.DecodeFromString(v.EncodeToString()).EncodeToString(),
                            "Composite containing two Composites encode decode encode");
            v = new TVariant("test\"1");
            v.Add(new TVariant(" test\"2"));
            v2 = new TVariant("test\"3");
            v2.Add(new TVariant("test\"4"));
            Assert.AreEqual("eString:\"test\"\"3test\"\"4\"", v2.EncodeToString(), "Test Cascading Composite 1");
            Assert.AreEqual("eString:\"test\"\"3test\"\"4\"", TVariant.DecodeFromString(
                    v2.EncodeToString()).EncodeToString(), "Test Cascading Composite 2");
            v.Add(v2);
            Assert.AreEqual("eString:\"test\"\"1 test\"\"2test\"\"3test\"\"4\"", v.EncodeToString(), "Test Cascading Composite 3");
            Assert.AreEqual("eString:\"test\"\"1 test\"\"2test\"\"3test\"\"4\"", TVariant.DecodeFromString(
                    v.EncodeToString()).EncodeToString(), "Test Cascading Composite 4");
            v = new TVariant("test1\"");
            v.Add(new TVariant(" test2\""));
            v2 = new TVariant();
            v2.Add(new TVariant("2900"));
            v.Add(v2);
            v.Add(new TVariant("test3"));
            Assert.AreEqual("eComposite::\"\"\"eString:\"\"\"\"test1\"\"\"\"\"\"\"\" test2\"\"\"\"\"\"\"\"\"\"\"\"\"\"|eInteger:2900|eString:test3\"",
                v.EncodeToString(), "Test Cascading Composite2");
            Assert.AreEqual("eString:\"\"\" )  ))\"", TVariant.DecodeFromString("eString:\"\"\" )  ))\"").EncodeToString(), "problem decoding string");
            v = new TVariant();
            v.Add(new TVariant("test\""));
            v.Add(new TVariant("2900"));
            v.Add(new TVariant("test"));
            Assert.AreEqual("eComposite::\"\"\"eString:\"\"\"\"test\"\"\"\"\"\"\"\"\"\"\"\"\"\"|eInteger:2900|eString:test\"", v.EncodeToString(), "problem encoding 1b");
            Assert.AreEqual(v.EncodeToString(), TVariant.DecodeFromString(v.EncodeToString()).EncodeToString(), "problem encoding1a");
            v = new TVariant();
            v.Add(new TVariant("SELECT DISTINCT  WHERE (  ( cc.a_cost_centre_code_c = \""));
            v.Add(new TVariant("2900"));
            v.Add(new TVariant("\" )  ))"));
            encodedString = "eComposite::\"\"\"eString:\"\"\"\"SELECT DISTINCT " +
                            " WHERE (  ( cc.a_cost_centre_code_c = \"\"\"\"\"\"\"\"\"\"\"\"\"\"|eInteger:2900|\"\"eString:\"\"\"\"\"\"\"\"\"\"\"\" )  ))\"\"\"\"\"\"\"";
            Assert.AreEqual(encodedString, v.EncodeToString(), "problem encoding1");
            Assert.AreEqual(v.EncodeToString(), TVariant.DecodeFromString(v.EncodeToString()).EncodeToString(), "problem encoding2");
            Assert.AreEqual(encodedString, TVariant.DecodeFromString(encodedString).EncodeToString(), "problem encoding3");
            v = new TVariant();
            v.Add(new TVariant("29001234", "partnerkey"));
            v.Add(new TVariant("29001235", "partnerkey"));
            v.Add(new TVariant("29001236", "partnerkey"));
            v.FormatString = "csvlistslash";
            Assert.AreEqual("eComposite:csvlistslash:\"eInteger:partnerkey:29001234|eInteger:partnerkey:29001235|eInteger:partnerkey:29001236\"",
                v.EncodeToString(),
                "format list with slash separator 1");
            Assert.AreEqual("0029001234/0029001235/0029001236", v.ToFormattedString(""), "format list with slash separator 2");
            v = new TVariant(435082450);
            Assert.AreEqual("eInteger:435082450", v.EncodeToString(), "test integer with text format 1");
            v.ApplyFormatString("text");
            Assert.AreEqual("eString:text:435082450", v.EncodeToString(), "test integer with text format 2");
            Assert.AreEqual("eString:text:435082450", TVariant.DecodeFromString(
                    v.EncodeToString()).EncodeToString(), "test integer with text format 3");
            v2 = new TVariant(v);
            Assert.AreEqual("eString:text:435082450", v2.EncodeToString(), "test integer with text format 4");
            v = new TVariant();
            v.Add(new TVariant((Object)(29015041.0)));
            v.Add(new TVariant((Object)(29017453.0)));
            Assert.AreEqual("eComposite:Currency:\"eCurrency:Currency:4718553875991232512|eCurrency:Currency:4718554523457552384\"",
                v.EncodeToString(), "test composite partner key");
            Assert.AreEqual("eComposite:Currency:\"eCurrency:Currency:4718553875991232512|eCurrency:Currency:4718554523457552384\"",
                TVariant.DecodeFromString(v.EncodeToString()).EncodeToString(),
                "test composite partner key 4");
            v.ApplyFormatString("csvlistslash:partnerkey");
            Assert.AreEqual("eComposite:csvlistslash:\"eInt64:partnerkey:29015041|eInt64:partnerkey:29017453\"",
                v.EncodeToString(), "test composite partner key 2");
            Assert.AreEqual("0029015041/0029017453", v.ToFormattedString(" "), "test composite partner key 3");
            v = TVariant.DecodeFromString("eComposite:csvlistslash:\"eString:text:de Vries, Rianne|eString:text:Visser, W. and K.K.J.\"");
            Assert.AreEqual("de Vries, Rianne/Visser, W. and K.K.J.", v.ToFormattedString(""));
            Assert.AreEqual("de Vries, Rianne/Visser, W. and K.K.J.", v.ToFormattedString("", "CSV"));
            v = new TVariant();
            v.Add(new TVariant("From Gift-Batch#: "));
            v.Add(new TVariant(8351));
            Assert.AreEqual("From Gift-Batch#: 8351", v.ToFormattedString(""), "colon in text");
            Assert.AreEqual("eComposite::\"\"\"eString:\"\"\"\"From Gift-Batch#: \"\"\"\"\"\"|eInteger:8351\"", v.EncodeToString(), "colon in text2");
            Assert.AreEqual("From Gift-Batch#: 8351", TVariant.DecodeFromString(v.EncodeToString()).ToFormattedString(""), "colon in text3");
            v = new TVariant();
            v.Add(new TVariant("Total for Account "));
            v.Add(new TVariant(1000));
            v.Add(new TVariant(':'));
            Assert.AreEqual("Total for Account 1000:", v.ToFormattedString(), "colon in text on its own");
            Assert.AreEqual(v.EncodeToString(), TVariant.DecodeFromString(v.EncodeToString()).EncodeToString(), "colon in text on its own 2");
            v = new TVariant();
            v.Add(new TVariant("", "", false));
            v.Add(new TVariant("", "", false));
            Assert.AreEqual("eComposite::\"eString:|eString:\"", v.EncodeToString(), "Composite with two empty strings encoding");
            Assert.AreEqual(TVariant.DecodeFromString(v.EncodeToString()).EncodeToString(),
                            v.EncodeToString(), "Composite with two empty strings encoding decoding encoding");
            v.Add(new TVariant("|", "", false));
            Assert.AreEqual("eComposite::\"eString:|eString:|\"\"eString:|\"\"\"",
                            v.EncodeString(), "Composite with pipe encoding");
            Assert.AreEqual(TVariant.DecodeFromString(v.EncodeToString()).EncodeToString(),
                            v.EncodeToString(), "Composite with pipe encoding decoding encoding");
            v = new TVariant("", true);
            Assert.AreEqual("eString:text:", v.EncodeToString(), "empty string encoding");
            Assert.AreEqual(v.EncodeToString(), TVariant.DecodeFromString(v.EncodeToString()).EncodeToString(), "empty string decoding");
            Assert.AreEqual(v.ToString(), "", "empty string identity");
            Assert.AreEqual("", TVariant.DecodeFromString(v.EncodeToString()).ToString(), "empty string decoding identity");
            Assert.AreEqual(eVariantTypes.eString, TVariant.DecodeFromString(v.EncodeToString()).TypeVariant, "empty string decoding type");
            v = new TVariant(" ", true);
            Assert.AreEqual(v.ToString(), " ", "one-space string identity");
            Assert.AreEqual("eString:text: ", v.EncodeToString(), "one-space string encoding");
            Assert.AreEqual(v.EncodeToString(), TVariant.DecodeFromString(v.EncodeToString()).EncodeToString(), "one-space string decoding");
            Assert.AreEqual(" ", TVariant.DecodeFromString(v.EncodeToString()).ToString(), "one-space string decoding identity");
            Assert.AreEqual(eVariantTypes.eString, TVariant.DecodeFromString(v.EncodeToString()).TypeVariant, "one-space string decoding type");
            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        /// test TVariant and currencies
        [Test]
        public void TestVariantCurrencies()
        {
            CultureInfo oldCulture;
            ArrayList cultures;
            String DecimalSeparator;
            String ThousandsOperator;
            TVariant v;

            oldCulture = Thread.CurrentThread.CurrentCulture;
            cultures = new ArrayList();
            cultures.Add("en-GB");
            cultures.Add("en-US");
            cultures.Add("de-DE");

            /* opposite meaning of , and . */
            cultures.Add("ru-RU");

            /* funny thousand separator: space */
            foreach (string s in cultures)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(s, false);
                DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
                ThousandsOperator = CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                Assert.AreEqual("#,##0;(#,##0);0;0", StringHelper.GetFormatString("Currency",
                        "CurrencyWithoutDecimals"), "find correct format string, CurrencyWithoutDecimals");
                Assert.AreEqual("#,#;(#,#);0;0", StringHelper.GetFormatString("Currency",
                        "CurrencyThousands"), "find correct format string thousands");
                Assert.AreEqual("#,##0.00;(#,##0.00);0.00;0", StringHelper.GetFormatString("", "Currency"), "Problem currency string");

                /* console.writeLine(CultureInfo.CurrentCulture.TwoLetterISOLanguageName); */
                Assert.AreEqual("0", new TVariant(0.00M, "Currency").ToFormattedString(
                        "CurrencyWithoutDecimals"), "format currency 0 without decimals");
                Assert.AreEqual("0", new TVariant(0.00M, "Currency").ToFormattedString("CurrencyThousands"), "format currency 0 thousands");
                Assert.AreEqual("0", new TVariant(0.00M, "->>>,>>>,>>>,>>9.99").ToFormattedString(
                        "CurrencyWithoutDecimals"), "format progress currency 0 without decimals");
                Assert.AreEqual("0", new TVariant(0.00M, "->>>,>>>,>>>,>>9.99").ToFormattedString(
                        "CurrencyThousands"), "format progress currency 0 thousands");
                Assert.AreEqual("", new TVariant("", true).ToFormattedString(
                        "Currency"), "format empty string in the column heading of a column that has currency values");
                Assert.AreEqual("eString:text:", new TVariant(new TVariant("",
                            true)).EncodeToString(), "format empty string in the column heading of a column that has currency values 2");
                Assert.AreEqual("", new TVariant(new TVariant("", true)).ToFormattedString(
                        "Currency"), "format empty string in the column heading of a column that has currency values 3");
                Assert.AreEqual("eString:text:", new TVariant("",
                        true).EncodeToString(), "format empty string in the column heading of a column that has currency values 4");
                v = new TVariant("", true);
                Assert.AreEqual("eString:text:",
                    v.EncodeToString(), "format empty string in the column heading of a column that has currency values 5");
                v = new TVariant();
                v.Add(new TVariant("", true));
                Assert.AreEqual("eEmpty:text:",
                    v.EncodeToString(), "format empty string in the column heading of a column that has currency values 6");
                Assert.AreEqual("", v.ToFormattedString(
                        "Currency"), "format empty string in the column heading of a column that has currency values 7");
                v = new TVariant();
                v.ApplyFormatString("partnerkey");
                Assert.AreEqual("eInt64:partnerkey:-1", v.EncodeToString(), "format empty partnerkey 1");
                Assert.AreEqual("0000000000", v.ToFormattedString(), "format empty partnerkey 2");
                v = new TVariant(0.00M, "Currency");
                v.ApplyFormatString("partnerkey");
                Assert.AreEqual("eInt64:partnerkey:0", v.EncodeToString(), "format empty partnerkey 3");
                Assert.AreEqual("0000000000", v.ToFormattedString(), "format empty partnerkey 4");
                Assert.AreEqual("100" + DecimalSeparator + "00%", TVariant.DecodeFromString(
                        "eDecimal:percentage2decimals:4636737291354636288").ToFormattedString(), "format percentage");
                v = new TVariant(-1003.25M, "Currency");
                Assert.AreEqual("eCurrency:Currency:-4571336140711264256",
                    v.EncodeToString(), "format negative number with format that only prints negative values 1");
                Assert.AreEqual("(1" + ThousandsOperator + "003" + DecimalSeparator + "25)",
                    v.ToFormattedString(), "format negative number with format that only prints negative values 2");
                v.ApplyFormatString("#,##0.00; ; ; ;");
                Assert.AreEqual("eEmpty:#,##0.00; ; ; ;:",
                    v.EncodeToString(), "format negative number with format that only prints negative values 3");
                Assert.AreEqual("", v.ToFormattedString(), "format negative number with format that only prints negative values 4");
                Assert.AreEqual("12" + ThousandsOperator + "346", new TVariant(12345.67M, "Currency").ToFormattedString(
                        "CurrencyWithoutDecimals"), "Problem D format currency");
                Assert.AreEqual("eCurrency:Currency:4668012718187306025", new TVariant(12345.67M,
                        "Currency").EncodeToString(), "Problem E format currency");
                Assert.AreEqual("12" + ThousandsOperator + "345" + DecimalSeparator + "67",
                    StringHelper.FormatCurrency(new TVariant(12345.67), "Currency"), "Problem F format currency");
                Assert.AreEqual("12345" + DecimalSeparator + "67", new TVariant(12345.67).ToString(), "Problem G format currency");
                Assert.AreEqual("12" + ThousandsOperator + "345" + DecimalSeparator + "67",
                    TVariant.DecodeFromString(new TVariant(12345.67M, "Currency").EncodeToString()).ToFormattedString(
                        "Currency"), "Problem H format currency");
                Assert.AreEqual("12",
                    TVariant.DecodeFromString(new TVariant(12345.67M, "Currency").EncodeToString()).ToFormattedString(
                        "CurrencyThousands"), "Problem I format currency");
                Assert.AreEqual("12", TVariant.DecodeFromString(new TVariant(12345.67M, "CurrencyThousands").EncodeToString()).ToFormattedString(
                        ""), "Problem J format currency");
                Assert.AreEqual("12", TVariant.DecodeFromString(new TVariant(12345.67M,
                            "#,##0.00;(#,##0.00);0.00;0").EncodeToString()).ToFormattedString(
                        "CurrencyThousands"), "Problem J2 access format currency thousands");

                /* we don't support thousands only with the progress format, too complicated */
                /* Assert.AreEqual('12', TVariant.DecodeFromString(TVariant.CreateCurrency(12345.67,'>>>,>>>,>>>,>>9.99').EncodeToString("CurrencyThousands")).toFormattedString(''), 'Problem J3 progress format currency thousands'); */
                Assert.AreEqual("12" + ThousandsOperator + "346",
                    TVariant.DecodeFromString(new TVariant(12345.67M,
                            "#,##0.00;(#,##0.00);0.00;0").EncodeToString()).ToFormattedString(
                        "CurrencyWithoutDecimals"), "Problem J2 access format currency w/o decimals");
                Assert.AreEqual("12" + ThousandsOperator + "346",
                    TVariant.DecodeFromString(new TVariant(12345.67M, "->>>,>>>,>>>,>>9.99").EncodeToString()).ToFormattedString(
                        "CurrencyWithoutDecimals"), "Problem J3 progress format currency w/o decimals");
                Assert.AreEqual("12" + ThousandsOperator + "346",
                    TVariant.DecodeFromString(new TVariant(12345.67M, "CurrencyWithoutDecimals").EncodeToString()).ToFormattedString(
                        ""), "Problem K format currency");
            }

            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        /// test currentculture and list separators
        [Test]
        public void TestListSeparator()
        {
            CultureInfo oldCulture;
            string separator;

            oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB", false);
            separator = Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator;
            Assert.AreEqual(",", separator);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE", false);
            separator = Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator;
            Assert.AreEqual(";", separator);
            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        /// test printing amounts as words
        [Test]
        public void TestAmountToWords()
        {
            CultureInfo oldCulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB", false);

            // see also Java GPL code: NumericalChameleon: http://www.jonelo.de/java/nc/
            // and use the NumericalChameleon executable to get the right words?

            Assert.AreEqual("one hundred and twenty-three Euro", NumberToWords.AmountToWords(123, "Euro", "Cent"));
            Assert.AreEqual("one thousand two hundred and thirty Euro", NumberToWords.AmountToWords(1230, "Euro", "Cent"));
            Assert.AreEqual("three hundred and eighty Euro", NumberToWords.AmountToWords(380, "Euro", "Cent"));
            Assert.AreEqual("twelve thousand three hundred Euro", NumberToWords.AmountToWords(12300, "Euro", "Cent"));
            Assert.AreEqual("one hundred and twenty-three thousand Euro", NumberToWords.AmountToWords(123000, "Euro", "Cent"));
            Assert.AreEqual("one hundred and twenty-three thousand one Euro", NumberToWords.AmountToWords(123001, "Euro", "Cent"));
            Assert.AreEqual("one hundred and twenty-three thousand one hundred and one Euro", NumberToWords.AmountToWords(123101, "Euro", "Cent"));
            Assert.AreEqual("one hundred and twenty-three thousand one hundred and thirty-one Euro",
                NumberToWords.AmountToWords(123131, "Euro", "Cent"));
            Assert.AreEqual("one hundred and twenty-three thousand two hundred and thirteen Euro", NumberToWords.AmountToWords(123213, "Euro", "Cent"));
            Assert.AreEqual("three Euro twenty-three Cent", NumberToWords.AmountToWords(3.23M, "Euro", "Cent"));
            Assert.AreEqual("three Euro seventy-five Cent", NumberToWords.AmountToWords(3.75M, "Euro", "Cent"));
            Assert.AreEqual("zero Euro one Cent", NumberToWords.AmountToWords(0.01M, "Euro", "Cent"));

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE", false);

            Assert.AreEqual("Einhundertdreiundzwanzig Euro", NumberToWords.AmountToWords(123, "Euro", "Cent"));
            Assert.AreEqual("Eintausendzweihundertdreißig Euro", NumberToWords.AmountToWords(1230, "Euro", "Cent"));
            Assert.AreEqual("Dreihundertachtzig Euro", NumberToWords.AmountToWords(380, "Euro", "Cent"));
            Assert.AreEqual("Zwölftausenddreihundert Euro", NumberToWords.AmountToWords(12300, "Euro", "Cent"));
            Assert.AreEqual("Einhundertdreiundzwanzigtausend Euro", NumberToWords.AmountToWords(123000, "Euro", "Cent"));
            Assert.AreEqual("Einhundertdreiundzwanzigtausendeins Euro", NumberToWords.AmountToWords(123001, "Euro", "Cent"));
            Assert.AreEqual("Einhundertdreiundzwanzigtausendeinhunderteins Euro", NumberToWords.AmountToWords(123101, "Euro", "Cent"));
            Assert.AreEqual("Einhundertdreiundzwanzigtausendeinhunderteinunddreißig Euro", NumberToWords.AmountToWords(123131, "Euro", "Cent"));
            Assert.AreEqual("Einhundertdreiundzwanzigtausendzweihundertdreizehn Euro", NumberToWords.AmountToWords(123213, "Euro", "Cent"));
            Assert.AreEqual("Drei Euro Dreiundzwanzig Cent", NumberToWords.AmountToWords(3.23M, "Euro", "Cent"));
            Assert.AreEqual("Drei Euro Fünfundsiebzig Cent", NumberToWords.AmountToWords(3.75M, "Euro", "Cent"));
            Assert.AreEqual("Null Euro Ein Cent", NumberToWords.AmountToWords(0.01M, "Euro", "Cent"));

            Thread.CurrentThread.CurrentCulture = oldCulture;
        }
    }
}