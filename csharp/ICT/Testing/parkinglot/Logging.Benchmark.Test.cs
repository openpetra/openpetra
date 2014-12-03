//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Wolfgang Uhr
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
using System.IO;
using System.Configuration;
using System.Diagnostics;
using Ict.Common;
using NUnit.Framework;
using log4net;
using log4net.Config;

namespace Tests.Common
{
    /// <summary>
    /// This class provides a benchmark test and compares the old TLogging-System
    /// with the logging.apache.org - log4net - System. The test itself shall run
    /// inside of NUnit but it is not realy a NUNIT-Test.
    /// Using a stopwatch you can read the time which is used for logging from the logfiles
    /// itself.
    ///
    /// The Routine requires file
    ///   csharp\ICT\Testing\_bin\Debug\Tests.Common.xml
    /// in order to set the relevant logging parameters.
    /// </summary>
    [TestFixture]
    public class Logging_Benchmark_Test
    {
        String fileName = "../../Common/Tests.Common.xml";

        Stopwatch stopwatch = new Stopwatch();


        // Standard-Initilisation of the log4net
        private static readonly ILog log = LogManager.GetLogger(typeof(Logging_Benchmark_Test));


        /// <summary>
        /// Standard-Initilisation of TLogging
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            new TLogging("../../log/test.log");
        }

        /// <summary>
        /// Standard-Test of the TLogging-System in order to get a reference time
        /// </summary>
        [Test]
        public void Test_01_TLogging()
        {
            stopwatch.Reset();
            stopwatch.Start();

            for (int i = 0; i < 10000; i++)
            {
                TLogging.Log("Entry: " + i);
            }

            stopwatch.Stop();
            TLogging.Log("Zeit: " + stopwatch.ElapsedMilliseconds);
            TLogging.Log("Freq: " + Stopwatch.Frequency);
        }

        /// <summary>
        /// First Version of log4net. In this case log.Debug is used directly.
        /// </summary>
        [Test]
        public void Test_02_Log4Net_DirectLog()
        {
            XmlConfigurator.Configure(
                new FileInfo(fileName));

            stopwatch.Reset();
            stopwatch.Start();

            for (int i = 0; i < 10000; i++)
            {
                log.Debug("Entry: " + i);
            }

            stopwatch.Stop();
            log.Info("Zeit: " + stopwatch.ElapsedMilliseconds);
            log.Info("Freq: " + Stopwatch.Frequency);
        }

        /// <summary>
        /// Optimized Version of the log4net-Test.
        /// if (log.IsDebugEnabled) will avoid a string construction in the
        /// local routine
        /// </summary>
        [Test]
        public void Test_03_Log4Net_If_CastedLog()
        {
            XmlConfigurator.Configure(
                new FileInfo(fileName));
            stopwatch.Reset();
            stopwatch.Start();
            log.Info("HRes: " + Stopwatch.IsHighResolution);

            for (int i = 0; i < 10000; i++)
            {
                if (log.IsDebugEnabled)
                {
                    log.Debug("Entry: " + i);
                }
            }

            stopwatch.Stop();
            log.Info("Zeit: " + stopwatch.ElapsedMilliseconds);
            log.Info("Ticks: " + stopwatch.ElapsedTicks);
            log.Info("Freq: " + Stopwatch.Frequency);
        }

        /// <summary>
        /// A Reference Test using System.Console directly.
        /// </summary>
        [Test]
        public void Test_99_WriteConsoleOnly()
        {
            stopwatch.Reset();
            stopwatch.Start();

            for (int i = 0; i < 10000; i++)
            {
                System.Console.WriteLine("Entry: " + i);
            }

            stopwatch.Stop();
            TLogging.Log("Zeit: " + stopwatch.ElapsedMilliseconds);
        }
    }
}