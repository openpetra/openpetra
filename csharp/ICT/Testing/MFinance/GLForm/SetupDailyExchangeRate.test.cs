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
using NUnit.Framework;
using Ict.Testing.NUnitForms;
using Ict.Common;
using Ict.Common.IO;
using System.Collections;
using System.Resources;
using Ict.Testing.NUnitPetraClient;

namespace Tests.MFinance.GLBatches
{
    [TestFixture]
    public class SetupDailyExchangeRate_test : CommonNUnitFunctions
    {
        // Each account is defined by its LedgerNumber ...
        // Actually the value is read by the TAppSettingsManager
        private Int32 fLedgerNumber;

        [Test]
        public void T01_CreateRessourceFile()
        {
            ResourceWriter rw = new ResourceWriter("sample.resources");

            rw.AddResource("btnCancel", "Cancel");
            rw.AddResource("btnTest", "Cancel");
            rw.Close();
        }

        [Test]
        public void T02_CreateRessourceFile()
        {
            ResourceReader rr = new ResourceReader("sample.resources");
        }

        [TestFixtureSetUp]
        public void Init()
        {
            new TLogging("PetraClient.log");
            TPetraConnector.Connect("../../../../../etc/TestClient.config");
            fLedgerNumber = Convert.ToInt32(TAppSettingsManager.GetValueStatic("LedgerNumber"));
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
            TPetraConnector.Disconnect();
        }
    }
}