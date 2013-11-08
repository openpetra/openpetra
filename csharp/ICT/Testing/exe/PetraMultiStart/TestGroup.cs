/*
 * >>>> Describe the functionality of this file. <<<<
 *
 * Comment: >>>> Optional comment. <<<<
 *
 * Author:  Timotheus Pokorra, Christian Kendel (C# translation)
 *
 * Version: $Revision: 1.3 $ / $Date: 2009/07/10 15:38:07 $
 */

using System;
using System.Threading;
using System.Xml;

using Ict.Common.IO;

namespace PetraMultiStart
{
    /// <summary>
    /// Description of TestGroup.
    /// </summary>
    public class TestGroup
    {
        protected XmlNode curGroup;

        public TestGroup(XmlNode ACurGroup)
        {
            curGroup = ACurGroup;
        }

        public void Run()
        {
            Thread ClientThread;
            TestClient MyClient;
            int StartId;
            int EndId;
            
            
            Thread.Sleep((int)main.RandomBreak(curGroup));
            
            Console.WriteLine("{0}: starting group {1}", DateTime.Now.ToLongTimeString(), TXMLParser.GetAttribute(curGroup, "name"));

            StartId = TXMLParser.GetIntAttribute(curGroup, "startid");
            EndId = TXMLParser.GetIntAttribute(curGroup, "endid");
            
            for (int Counter = StartId; Counter <= EndId; Counter += 1)
            {
                MyClient = new TestClient(curGroup, Counter + Global.StartClientID, StartId == EndId);
                
                ClientThread = new Thread(MyClient.Run);
                ClientThread.Start();
            }
        }
    }
}
