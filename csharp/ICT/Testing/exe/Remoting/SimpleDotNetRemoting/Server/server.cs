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

namespace Ict.Testing.SimpleDotNetRemoting.Server
{
    class Server
    {
        static void Main(string[] args)
        {
            try
            {
                TcpChannel tcpChannel = new TcpChannel(9000);
                ChannelServices.RegisterChannel(tcpChannel, false);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(MyService), "MyService", WellKnownObjectMode.SingleCall);

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

    /// <summary>
    /// a simple service for testing purposes
    /// </summary>
    public class MyService : MarshalByRefObject, IMyService
    {
        /// <summary>
        /// print hello world
        /// </summary>
        /// <param name="msg"></param>
        public void HelloWorld(string msg)
        {
            Console.WriteLine(msg);
        }

        /// <summary>
        /// some tests for remoting DateTime objects
        /// </summary>
        /// <param name="date"></param>
        /// <param name="outDate"></param>
        /// <returns></returns>
        public DateTime TestDateTime(DateTime date, out DateTime outDate)
        {
            Console.WriteLine("ToShortDateString(): " + date.ToShortDateString());
            Console.WriteLine("ToUniversalTime(): " + date.ToUniversalTime());
            Console.WriteLine("ToLocalTime(): " + date.ToLocalTime());

            date = new DateTime(date.Year, date.Month, date.Day);
            Console.WriteLine("ToShortDateString(): " + date.ToShortDateString());
            Console.WriteLine("ToUniversalTime(): " + date.ToUniversalTime());
            Console.WriteLine("ToLocalTime(): " + date.ToLocalTime());

            outDate = date;
            return date;
        }
    }
}