//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timotheusp, ChristianK (C# translation, adaption to OpenPetra)
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
using System.Threading;
using System.Xml;

using Ict.Common;
using Ict.Common.IO;

namespace PetraMultiStart
{
    /// <summary>
    /// Description of TestClient.
    /// </summary>
    public class TestClient
    {
        private XmlNode curGroup;
        private Int32 Id;
        private bool SingleClient;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ACurGroup"></param>
        /// <param name="AId"></param>
        /// <param name="ASingleClient"></param>
        public TestClient(XmlNode ACurGroup, Int32 AId, bool ASingleClient)
        {
            curGroup = ACurGroup;
            Id = AId;
            SingleClient = ASingleClient;
        }

        /// <summary>
        /// Drives a Test Client.
        /// </summary>
        public void Run()
        {
            XmlNode curEvent;
            XmlNode oldEvent;
            String action;
            Int64 breakTime;
            Int32 NetClientPort;
            Int32 APClientPort;
            String parameters;
            System.Diagnostics.Process UProgressProcess;
            DateTime TimeFinished;
            
            curEvent = curGroup.FirstChild;
            breakTime = 0;

            if ((curEvent != null) && (curEvent.Name.ToLower() == "event"))
            {
                breakTime = main.RandomBreak(curEvent);
            }

            while ((curEvent != null) && (curEvent.Name.ToLower() == "event") && main.ServerStillRunning())
            {
                if (breakTime > 0)
                {
                    System.Console.WriteLine("{0}: Client {1} is sleeping for about " + Convert.ToString(
                            breakTime / 1000) + " seconds ...", DateTime.Now.ToLongTimeString(), Id);
                    Thread.Sleep((int)breakTime);
                }

                // System.Console.WriteLine('continue client ' + Id.Tostring() + ' at time ' + DateTime.Now.ToString());
                // we need the breaktime already, to tell the client how long it should run
                oldEvent = curEvent;
                curEvent = curEvent.NextSibling;

                if (curEvent == null)
                {
                    // start again
                    curEvent = curGroup.FirstChild;

                    if (TXMLParser.GetBoolAttribute(curGroup, "loop", true) == false)
                    {
                        curEvent = null;
                    }
                }

                if (curEvent != null)
                {
                    breakTime = main.RandomBreak(curEvent);
                }

                action = TXMLParser.GetAttribute(oldEvent, "action");

                if (action == "connect")
                {
                    System.Console.WriteLine("{0}: connecting client {1}", DateTime.Now.ToLongTimeString(), Id);
                    
                    NetClientPort = 2080 + Id * 2;
                    APClientPort = 2081 + Id * 2;
                    
                    TimeFinished = DateTime.Now.AddMilliseconds(breakTime);
                    
                    parameters = "-C:" + Global.Configfile + ' ' + "-NetClientPort:" + NetClientPort.ToString() + ' ' + "-APClientPort:" +
                                 APClientPort.ToString() + ' ' + "-AutoLogin:TESTUSER" + Id.ToString() + ' ' + "-AutoLoginPasswd:test" + ' ' +
                                 "-DisconnectTime:" + new TVariant(TimeFinished).EncodeToString() + ' ' + "-RunAutoTests:true" + ' ' +
                                 "-AutoTestConfigFile:" +
                                 TXMLParser.GetAttribute(oldEvent, "testing") + ' ' + 
                                 "-AutoTestParameters:" +
                                 TXMLParser.GetAttribute(oldEvent, "params") + ' ' + 
                                 "-SingleClient:" +
                                 SingleClient.ToString();

                    // add fixed disconnection datetime; or create a testing file?
                    // add testing file
                    try
                    {
                        System.Console.WriteLine("{0}:   starting {1}", DateTime.Now.ToLongTimeString(), parameters);
                        
                        UProgressProcess = new System.Diagnostics.Process();
                        UProgressProcess.EnableRaisingEvents = false;
                        UProgressProcess.StartInfo.FileName = Global.Filename;
                        UProgressProcess.StartInfo.Arguments = parameters;
                        UProgressProcess.EnableRaisingEvents = true;

                        if ((!UProgressProcess.Start()))
                        {
                            return;
                        }
                    }
                    catch (Exception Exp)
                    {
                        System.Console.WriteLine("{0}: Trouble in TestClient.Run", DateTime.Now.ToLongTimeString());
                        System.Console.WriteLine("{0}: {1}", DateTime.Now.ToLongTimeString(), Exp.Message);
                        
                        return;
                    }
                }
                else if (action == "disconnect")
                {
                    System.Console.WriteLine("{0}: disconnecting client {1}", DateTime.Now.ToLongTimeString(), Id);
                }
            }
        }
    }
}
