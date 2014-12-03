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
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Net;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Ict.Tools.TinyWebServer
{
/// <summary>
/// this is a simple ASMX Server, for use if XSP from mono is not available
/// </summary>
    class TTinyASMXServer
    {
        static string FLogFile = "../Ict.Tools.WebServer.log";
        static TimeSpan? FMaxRunTime = new Nullable <TimeSpan>();
        static ThreadedHttpListenerWrapper Fthlw;
        static DateTime FTimeStarted = DateTime.Now;
        static bool FStopServer = false;

        static void Log(string AMessage)
        {
            Console.WriteLine(AMessage);

            using (StreamWriter sw = new StreamWriter(FLogFile.Replace('/', Path.DirectorySeparatorChar), true))
            {
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + AMessage);
                sw.Close();
            }
        }

        static void ThreadCheckMaxRunTime()
        {
            while (!FStopServer)
            {
                if (FTimeStarted.Add(FMaxRunTime.Value).CompareTo(DateTime.Now) < 0)
                {
                    Log("Stopping the server after maximum run time defined in command line parameter: " +
                        FMaxRunTime.Value.ToString());
                    FStopServer = true;
                    Fthlw.Stop();
                    break;
                }

                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        static void Main(string[] args)
        {
            try
            {
                string port = "8888";
                string[] parameters = Environment.GetCommandLineArgs();

                if (parameters.Length > 1)
                {
                    port = parameters[1];
                }

                if (parameters.Length > 2)
                {
                    FLogFile = parameters[2];
                }

                if (parameters.Length > 3)
                {
                    FMaxRunTime = TimeSpan.FromMinutes(Convert.ToInt32(parameters[3]));
                }

                string physicalDir = Directory.GetCurrentDirectory();

                if (!(physicalDir.EndsWith(Path.DirectorySeparatorChar.ToString())))
                {
                    physicalDir = physicalDir + Path.DirectorySeparatorChar;
                }

                // Copy this hosting DLL into the /bin directory of the application
                string FileName = Assembly.GetExecutingAssembly().Location;

                try
                {
                    if (!Directory.Exists(physicalDir + "bin" + Path.DirectorySeparatorChar))
                    {
                        Directory.CreateDirectory(physicalDir + "bin" + Path.DirectorySeparatorChar);
                    }

                    File.Copy(FileName, physicalDir + "bin" + Path.DirectorySeparatorChar + Path.GetFileName(FileName), true);
                }
                catch
                {
                    ;
                }

                Fthlw = (ThreadedHttpListenerWrapper)ApplicationHost.CreateApplicationHost(
                    typeof(ThreadedHttpListenerWrapper), "/", physicalDir);

                Log("trying to listen on port " + port);
                string[] prefixes = new string[] {
                    "http://+:" + port + "/"
                };

                Fthlw.Configure(prefixes, "/", Directory.GetCurrentDirectory());

                try
                {
                    Fthlw.Start();
                }
                catch (HttpListenerException)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Log("we cannot listen on this port. perhaps you need to run as administrator once: ");
                    Log(
                        "  netsh http add urlacl url=http://+:" + port + "/ user=" + Environment.MachineName + "\\" + Environment.UserName);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();

                    throw;
                }

                string Message = "Listening for requests on http://127.0.0.1:" + port + "/";

                if (FMaxRunTime.HasValue)
                {
                    Message += " for a maximum run time: " + FMaxRunTime.Value.ToString();
                }

                Log(Message);

                if (FMaxRunTime.HasValue)
                {
                    (new Thread(() => ThreadCheckMaxRunTime())).Start();
                }

                while (!FStopServer)
                {
                    try
                    {
                        Fthlw.ProcessRequest();
                    }
                    catch (AppDomainUnloadedException e)
                    {
                        Log(e.Message);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Log(e.ToString());
            }
        }
    }
}