//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Threading;
using System.Runtime.Remoting;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.ServiceModel.Web;
using System.ServiceModel;
using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Tests.HTTPRemoting.Interface;
using Ict.Petra.Server.App.Core;

namespace Tests.HTTPRemoting.Service
{
    /// <summary>
    /// sample webconnector
    /// </summary>
    public class TMyServiceWebConnector
    {
        /// <summary>
        /// sample webconnector method
        /// </summary>
        static public string HelloWorld(string msg)
        {
            TLogging.Log(msg);
            return "Hello from the server!!!";
        }

        /// <summary>
        /// sample webconnector method
        /// </summary>
        static public DateTime TestDateTime(DateTime date, out DateTime outDateTomorrow)
        {
            Console.WriteLine("ToShortDateString(): " + date.ToShortDateString());
            Console.WriteLine("ToUniversalTime(): " + date.ToUniversalTime());
            Console.WriteLine("ToLocalTime(): " + date.ToLocalTime());

            date = new DateTime(date.Year, date.Month, date.Day);
            Console.WriteLine("ToShortDateString(): " + date.ToShortDateString());
            Console.WriteLine("ToUniversalTime(): " + date.ToUniversalTime());
            Console.WriteLine("ToLocalTime(): " + date.ToLocalTime());

            outDateTomorrow = DateTime.Today.AddDays(1);
            return date;
        }

        /// <summary>
        /// sample webconnector method
        /// </summary>
        static public string HelloSubWorld()
        {
            return "HelloSubWorld from the server!!!";
        }

        /// <summary>
        /// sample webconnector method that takes a long time and uses the ProgressTracker
        /// </summary>
        static public string LongRunningJob()
        {
            string ClientID = DomainManager.GClientID.ToString();

            // enable DebugLevel to show progress state on console
            TLogging.DebugLevel = 1;
            TProgressTracker.InitProgressTracker(ClientID,
                "LongRunningJob",
                100);

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500);
                TProgressTracker.SetCurrentState(ClientID, "working", Convert.ToDecimal(i * 10));
            }

            TProgressTracker.FinishJob(ClientID);

            return "done";
        }
    }

    /// <summary>
    /// test of UIConnector
    /// </summary>
    public class TMyUIConnector : IMyUIConnector
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TMyUIConnector()
        {
        }

        private int FCounter = 0;

        /// <summary>
        /// test
        /// </summary>
        public string HelloWorldUIConnector()
        {
            FCounter++;
            string s = "HelloWorldUIConnector called this many times: " + FCounter.ToString();
            TLogging.Log(s);
            return s;
        }
    }
}