/*
 * >>>> Describe the functionality of this file. <<<<
 *
 * Comment: >>>> Optional comment. <<<<
 *
 * Author:  Timotheus Pokorra, Christian Kendel (C# translation)
 *
 * Version: $Revision: 1.3 $ / $Date: 2009/07/10 15:41:06 $
 */

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
        protected XmlNode curGroup;
        protected Int32 Id;
        private bool SingleClient;

        public TestClient(XmlNode ACurGroup, Int32 AId, bool ASingleClient)
        {
            curGroup = ACurGroup;
            Id = AId;
            SingleClient = ASingleClient;
        }

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
