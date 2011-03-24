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


namespace Ict.Testing.Petra.Server.MFinance.GL
{
	[TestFixture]
	public partial class TestGLRevaluation :CommonNUnitFunctions
	{

		/// <summary>
        /// Normally I plan to move out this routine to CommonNUnitFunctions but this is not coorect
        /// for all types of test. So I need a set of CommonNUnitFunctions
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            //new TLogging("TestServer.log");
            //TPetraServerConnector.Connect("../../etc/TestServer.config");
            InitServerConnection();
        }

        /// <summary>
        /// Normally I plan to move out this routine to CommonNUnitFunctions but this is not coorect
        /// for all types of test. So I need a set of CommonNUnitFunctions
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            DisconnectServerConnection();
        }
	}
}
