//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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

using System;
using System.Collections;
using System.IO;
using System.Data.Odbc;
using NUnit.Framework;
using Ict.Testing.NUnitForms;
using Ict.Petra.Server.MFinance.GL;
using Ict.Common.Verification;

using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;


using Ict.Common.DB;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Testing.Petra.Server.MFinance.GL
{
    [TestFixture]
    public partial class TestGLImport : CommonNUnitFunctions
    {
        private const int intLedgerNumber = 43;


        [Test]
        public void Test_01_GL_Import()
        {
            Hashtable requestParams = new Hashtable();

            requestParams.Add("ALedgerNumber", intLedgerNumber);
            requestParams.Add("Delimiter", ",");
            requestParams.Add("DateFormatString", "dd.mm.yyyy");
            requestParams.Add("NumberFormat", "American");
            requestParams.Add("NewLine", Environment.NewLine);

            string strContent = LoadCSVFileToString("csharp\\ICT\\Testing\\lib\\MFinance\\GL-Test\\" +
                "test-csv\\glbatch-import.csv");
            // FileStream fs = new FileStream(

            System.Diagnostics.Debug.WriteLine(strContent);
            TVerificationResultCollection verificationResult;

            Assert.IsTrue(
                TTransactionWebConnector.ImportGLBatches(requestParams, strContent, out verificationResult),
                "Import glbatch-import.csv done well ....");
        }

        [TestFixtureSetUp]
        public void Init()
        {
            InitServerConnection();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            DisconnectServerConnection();
        }
    }
}