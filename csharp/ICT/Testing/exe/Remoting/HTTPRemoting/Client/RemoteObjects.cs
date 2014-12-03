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
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Tests.HTTPRemoting.Interface;

namespace Tests.HTTPRemoting.Client
{
    /// <summary>
    /// Holds all references to instantiated Serverside objects that are remoted by the Server.
    /// These objects can get remoted either statically (the same Remoting URL all
    /// the time) or dynamically (on-the-fly generation of Remoting URL).
    ///
    /// The TRemote class is used by the Client for all communication with the Server
    /// after the initial communication at Client start-up is done.
    ///
    /// The properties MPartner, MFinance, etc. represent the top-most level of the
    /// Petra Partner, Finance, etc. Petra Module Namespaces in the PetraServer.
    /// </summary>
    public class TRemoteTest
    {
        /// <summary>
        /// my service class
        /// </summary>
        public class TMyService
        {
            /// print hello world
            public string HelloWorld(string msg)
            {
                SortedList <string, object>parameters = new SortedList <string, object>();
                parameters.Add("msg", "hello " + Environment.UserName);
                return (string)THttpConnector.CallWebConnector("Sample", "HelloWorld", parameters, "System.String")[0];
            }

            /// some tests for remoting DateTime objects
            public DateTime TestDateTime(DateTime date, out DateTime outDateTomorrow)
            {
                SortedList <string, object>parameters = new SortedList <string, object>();
                parameters.Add("date", date);
                List <object>Result = THttpConnector.CallWebConnector("Sample", "TestDateTime", parameters, "list");
                outDateTomorrow = (DateTime)Result[0];
                return (DateTime)Result[1];
            }

            /// sample webconnector method that takes a long time and uses the ProgressTracker
            public string LongRunningJob()
            {
                SortedList <string, object>parameters = new SortedList <string, object>();
                return (string)THttpConnector.CallWebConnector("Sample", "LongRunningJob", parameters, "System.String")[0];
            }

            private TMySubNamespace FMySubNamespace = new TMySubNamespace();

            /// get a subnamespace
            public TMySubNamespace SubNamespace
            {
                get
                {
                    return FMySubNamespace;
                }
            }

            /// <summary> namespace definition </summary>
            public class TMySubNamespace
            {
                /// get the UIConnector
                public TMyUIConnector MyUIConnector()
                {
                    return new TMyUIConnector();
                }

                /// print hello sub world
                public string HelloSubWorld(string msg)
                {
                    SortedList <string, object>ActualParameters = new SortedList <string, object>();
                    ActualParameters.Add("msg", msg);
                    List <object>Result = THttpConnector.CallWebConnector("Sample",
                        "TMySubNamespace.HelloSubWorld",
                        ActualParameters,
                        "System.String");
                    return Result[0].ToString();
                }

                /// the implementation of the UIConnector for the client
                public class TMyUIConnector : IMyUIConnector, IDisposable
                {
                    private string FObjectID = String.Empty;

                    /// constructor, create the object on the server
                    public TMyUIConnector()
                    {
                        SortedList <string, object>ActualParameters = new SortedList <string, object>();
                        FObjectID = THttpConnector.CreateUIConnector("Sample", "TMyUIConnector", ActualParameters);
                    }

                    /// desctructor
                    ~TMyUIConnector()
                    {
                        Dispose();
                    }

                    /// dispose the object on the server as well
                    public void Dispose()
                    {
                        THttpConnector.DisconnectUIConnector("Sample", FObjectID);
                    }

                    /// access the UIConnector Method
                    public string HelloWorldUIConnector()
                    {
                        SortedList <string, object>ActualParameters = new SortedList <string, object>();
                        return THttpConnector.CallUIConnectorMethod(FObjectID,
                            "Sample",
                            "TMyUIConnector",
                            "HelloWorldUIConnector",
                            ActualParameters,
                            "System.String")[0].ToString();
                    }
                }
            }
        }


        /// <summary>Reference to the TMyService object</summary>
        public static TMyService MyService
        {
            get
            {
                return UMyServiceObject;
            }
        }

        private static TMyService UMyServiceObject;

        /// <summary>
        ///
        /// </summary>
        public TRemoteTest()
        {
            UMyServiceObject = new TMyService();
        }
    }
}