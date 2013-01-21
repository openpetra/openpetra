//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Tests.SimpleDotNetRemoting.Interface;

namespace Ict.Testing.SimpleDotNetRemoting.Client
{
    class Client
    {
        static void Main(string[] args)
        {
            try
            {
                TcpChannel tcpChannel = new TcpChannel();
                ChannelServices.RegisterChannel(tcpChannel, false);

                IMyService remoteObject = (IMyService)Activator.GetObject(typeof(IMyService), "tcp://localhost:9000/MyService");

                remoteObject.HelloWorld("test");

                DateTime outDate;
                DateTime date = new DateTime(2010, 1, 1);
                DateTime result = remoteObject.TestDateTime(date, out outDate);

                Console.WriteLine("ToShortDateString(): " + result.ToShortDateString());
                Console.WriteLine("ToUniversalTime(): " + result.ToUniversalTime());
                Console.WriteLine("ToLocalTime(): " + result.ToLocalTime());

                Console.WriteLine("ToShortDateString(): " + outDate.ToShortDateString());
                Console.WriteLine("ToUniversalTime(): " + outDate.ToUniversalTime());
                Console.WriteLine("ToLocalTime(): " + outDate.ToLocalTime());

                Console.WriteLine("Press ENTER to exit ...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
    }
}