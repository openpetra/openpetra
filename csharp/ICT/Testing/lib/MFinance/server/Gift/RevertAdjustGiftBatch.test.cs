//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2020 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using NUnit.Framework;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.Setup.WebConnectors;
using Ict.Petra.Server.MPartner.Partner.UIConnectors;
using Ict.Petra.Server.MPartner.Partner.WebConnectors;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Testing.NUnitPetraServer;
using Ict.Testing.NUnitTools;

using Tests.MPartner.shared.CreateTestPartnerData;

namespace Tests.MFinance.Server.Gift
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TRevertAdjustGiftBatchTest
    {
        Int32 FLedgerNumber = -1;

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            //new TLogging("TestServer.log");
            TPetraServerConnector.Connect("../../etc/TestServer.config");
            FLedgerNumber = TAppSettingsManager.GetInt32("LedgerNumber", 43);
            TLogging.Log("Ledger Number = " + FLedgerNumber);
        }

        /// <summary>
        /// cleaning up everything that was set up for this test
        /// </summary>
        [OneTimeTearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        private int ImportAndPostGiftBatch()
        {
            TGiftImporting importer = new TGiftImporting();

            string testFile = TAppSettingsManager.GetValue("GiftBatch.file", "../../csharp/ICT/Testing/lib/MFinance/SampleData/sampleGiftBatch.csv");
            StreamReader sr = new StreamReader(testFile);
            string FileContent = sr.ReadToEnd();

            FileContent = FileContent.Replace("{ledgernumber}", FLedgerNumber.ToString());
            FileContent = FileContent.Replace("{thisyear}", DateTime.Today.Year.ToString());

            sr.Close();

            Hashtable parameters = new Hashtable();
            parameters.Add("Delimiter", ",");
            parameters.Add("ALedgerNumber", FLedgerNumber);
            parameters.Add("DateFormatString", "yyyy-MM-dd");
            parameters.Add("DatesMayBeIntegers", false);
            parameters.Add("NumberFormat", "American");
            parameters.Add("NewLine", Environment.NewLine);

            TVerificationResultCollection VerificationResult = null;
            GiftBatchTDSAGiftDetailTable NeedRecipientLedgerNumber;
            bool refreshRequired;

            if (!importer.ImportGiftBatches(parameters, FileContent, out NeedRecipientLedgerNumber, out refreshRequired, out VerificationResult))
            {
                Assert.Fail("Gift Batch was not imported: " + VerificationResult.BuildVerificationResultString());
            }

            int BatchNumber = importer.GetLastGiftBatchNumber();

            Assert.AreNotEqual(-1, BatchNumber, "Should have imported the gift batch and returned a valid batch number");
            Int32 generatedGlBatchNumber;

            if (!TGiftTransactionWebConnector.PostGiftBatch(FLedgerNumber, BatchNumber, out generatedGlBatchNumber, out VerificationResult))
            {
                Assert.Fail("Gift Batch was not posted: " + VerificationResult.BuildVerificationResultString());
            }

            return BatchNumber;
        }

        /// <summary>
        /// This will import a test gift batch, and post it, and then create an adjustment batch
        /// </summary>
        [Test]
        public void TestAdjustGiftBatch()
        {
            int GiftBatchNumber = ImportAndPostGiftBatch();

            TGet_GLM_Info getGLM_InfoBeforeTest73 = new TGet_GLM_Info(FLedgerNumber, "0200", "7300");
            TGet_GLM_Info getGLM_InfoBeforeTest35 = new TGet_GLM_Info(FLedgerNumber, "0200", "3500");

            string formletterTemplateFile = TAppSettingsManager.GetValue("ReceiptTemplate.file",
                "../../csharp/ICT/Testing/lib/MFinance/SampleData/AnnualReceiptTemplate.html");
            Encoding encodingOfHTMLfile = TTextFile.GetFileEncoding(formletterTemplateFile);
            StreamReader sr = new StreamReader(formletterTemplateFile, encodingOfHTMLfile, false);
            string FileContent = sr.ReadToEnd();
            sr.Close();

            string receiptsBefore;
            string receiptsPDF;
            TReceiptingWebConnector.CreateAnnualGiftReceipts(FLedgerNumber, "Annual",
                new DateTime(DateTime.Today.Year, 1, 1), new DateTime(DateTime.Today.Year, 12, 31),
                FileContent, null, String.Empty, null, String.Empty, "de-DE",
                out receiptsPDF, out receiptsBefore);
            Assert.AreNotEqual(0, receiptsBefore.Trim().Length, "old receipt must not be empty");

            int AdjustBatchNumber;
            TAdjustmentWebConnector.GiftRevertAdjust(FLedgerNumber,
                GiftBatchNumber, -1, -1, false, -1,
                DateTime.Today, GiftAdjustmentFunctionEnum.AdjustGift,
                false, -1.0m, out AdjustBatchNumber);

            bool BatchIsUnposted;
            string CurrencyCode;
            GiftBatchTDS BatchTDS = TGiftTransactionWebConnector.LoadGiftTransactionsForBatch(
                FLedgerNumber, AdjustBatchNumber, out BatchIsUnposted, out CurrencyCode);

            // find the transaction to modify
            Int32 ToModify = (BatchTDS.AGiftDetail[1].GiftTransactionNumber == 2)?1:0;
            
            // change the amount from 20 to 25
            BatchTDS.AGiftDetail[ToModify].GiftTransactionAmount = 25;
            // the money should go to field 35 instead of field 73
            BatchTDS.AGiftDetail[ToModify].RecipientKey = 35000000;
            // TODO change of donor
            // BatchTDS.Gift[1].DonorKey = 

            TVerificationResultCollection VerificationResult;
            if (TGiftTransactionWebConnector.SaveGiftBatchTDS(ref BatchTDS, out VerificationResult) != TSubmitChangesResult.scrOK)
            {
                Assert.Fail("Adjustment Gift Batch was not saved: " + VerificationResult.BuildVerificationResultString());
            }

            int generatedGlBatchNumber;
            if (!TGiftTransactionWebConnector.PostGiftBatch(FLedgerNumber, AdjustBatchNumber, out generatedGlBatchNumber, out VerificationResult))
            {
                Assert.Fail("Adjustment Gift Batch was not posted: " + VerificationResult.BuildVerificationResultString());
            }

            TGet_GLM_Info getGLM_InfoAfterTest73 = new TGet_GLM_Info(FLedgerNumber, "0200", "7300");
            TGet_GLM_Info getGLM_InfoAfterTest35 = new TGet_GLM_Info(FLedgerNumber, "0200", "3500");

            // Test balance on the related account/Costcentre
            Assert.AreEqual(getGLM_InfoBeforeTest73.YtdActual - 20, getGLM_InfoAfterTest73.YtdActual, "The amount of 20 should be derived from the 73 costcentre");
            Assert.AreEqual(getGLM_InfoBeforeTest35.YtdActual + 25, getGLM_InfoAfterTest35.YtdActual, "The amount of 25 should be added to the 35 costcentre");

            // Test the number of rows on the gift receipt.
            // the difference should be 3 lines removed, 3 lines added. no double donations.
            string receiptsAfter;
            TReceiptingWebConnector.CreateAnnualGiftReceipts(FLedgerNumber, "Annual",
                new DateTime(DateTime.Today.Year, 1, 1), new DateTime(DateTime.Today.Year, 12, 31),
                FileContent, null, String.Empty, null, String.Empty, "de-DE",
                out receiptsPDF, out receiptsAfter);
            receiptsBefore = THttpBinarySerializer.DeserializeFromBase64(receiptsBefore);
            receiptsAfter = THttpBinarySerializer.DeserializeFromBase64(receiptsAfter);

            TLogging.Log("TestAdjustGiftBatch Diff:");
            TLogging.Log(TTextFile.Diff(receiptsBefore, receiptsAfter));
            string[] diff = TTextFile.Diff(receiptsBefore, receiptsAfter).Trim().Split(Environment.NewLine);
            Assert.AreEqual(6, diff.Length, "difference on receipts are 6 lines");
        }
    }
}
