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
using System.Data;
using System.Data.Odbc;
using System.Collections;
using Ict.Petra.Server.App.Core.Security;


using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;


using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;


namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
    public partial class TSingletonConnector
    {
        /// <summary>
        /// Test routine only
        /// </summary>
        /// <param name="AInput">new integer value</param>
        /// <returns>old integer value</returns>
        [RequireModulePermission("FINANCE-1")]
        public static int TTestInfo_1(int AInput)
        {
            int ih = TTestMonthEnd.Instance.TestObject.ITest;

            TTestMonthEnd.Instance.TestObject.ITest = AInput;
            return ih;
        }
    }
}

namespace Ict.Petra.Server.MFinance.GL
{
    public class TTestMonthEnd
    {
        private static volatile TTestMonthEnd instance;
        private static object smallTestObject = new SmallTestObject();

        private TTestMonthEnd()
        {
        }

        public static TTestMonthEnd Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (smallTestObject)
                    {
                        if (instance == null)
                        {
                            instance = new TTestMonthEnd();
                        }
                    }
                }

                return instance;
            }
        }

        public SmallTestObject TestObject
        {
            get
            {
                return (SmallTestObject)smallTestObject;
            }
        }
    }

    public class SmallTestObject
    {
        int local = 0;
        public SmallTestObject()
        {
        }

        public int ITest
        {
            get
            {
                return local;
            }
            set
            {
                local = value;
            }
        }
    }
}