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

using Ict.Common.IO;

namespace PetraMultiStart
{
    /// <summary>
    /// Description of TestGroup.
    /// </summary>
    public class TestGroup
    {
        private XmlNode curGroup;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ACurGroup"></param>
        public TestGroup(XmlNode ACurGroup)
        {
            curGroup = ACurGroup;
        }

        /// <summary>
        /// Drives a Test Group.
        /// </summary>
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
