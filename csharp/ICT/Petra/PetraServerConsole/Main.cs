//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using GNU.Gettext;

using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Sinks.Encryption;
using Ict.Petra.Server.App.Core;

using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Server.MSysMan.ImportExport.WebConnectors;

namespace PetraServerConsole
{
#region TServer

/// <summary>
/// The TServer class starts the PetraServer and provides a menu with a number
/// of functions, including stopping the PetraServer.
///
/// </summary>
public class TServer
{
    private static void WriteServerPrompt()
    {
        Console.Write(Catalog.GetString("SERVER>") + " ");
    }

    private TServerManager TheServerManager;

    /// <summary>
    /// Starts the Petra Server.
    ///
    /// </summary>
    /// <returns>void</returns>
    public void Startup()
    {
        try
        {
            //
            // Uncomment the following lines to see which DLL's are loaded into the Default AppDomain at application start.
            // It can help in identifying which DLL's are loaded later in addition to those that were loaded at application start.

            // Console.WriteLine('Loaded Assemblies in AppDomain ' + Thread.GetDomain.FriendlyName + ' (at Server start):');
            // foreach (Assembly tmpAssembly in Thread.GetDomain.GetAssemblies())
            // {
            // Console.WriteLine(tmpAssembly.FullName);
            // }

            new TAppSettingsManager();

            TLanguageCulture.Init();

            TheServerManager = new TServerManager();

            // Ensure Logging and an 'ordered cooperative shutdown' in case of an Unhandled Exception
            TheServerManager.HookupProperShutdownProcessing();

            Console.WriteLine();
            TLogging.Log(TheServerManager.ServerInfoVersion);

            //
            // Connect to main Database
            //
            try
            {
                TheServerManager.EstablishDBConnection();
            }
            catch (FileNotFoundException ex)
            {
                TLogging.Log(ex.Message);
                TLogging.Log("Please check your OpenPetra.build.config file ...");
                TLogging.Log("Maybe a nant initConfigFile helps ...");
                throw new ApplicationException();
            }
            catch (Exception)
            {
                throw;
            }

            // Setup Server Timed Processing
            try
            {
                TheServerManager.SetupServerTimedProcessing();
            }
            catch (Exception)
            {
                throw;
            }

            //
            // Remote the remoteable objects
            //
            try
            {
                if (TAppSettingsManager.HasValue("LifetimeServices.LeaseTimeInSeconds"))
                {
                    TLogging.Log(Catalog.GetString("Reading parameters for server remote configuration from config file..."));

                    BinaryServerFormatterSinkProvider TCPSink = new BinaryServerFormatterSinkProvider();
                    TCPSink.TypeFilterLevel = TypeFilterLevel.Low;
                    IServerChannelSinkProvider EncryptionSink = TCPSink;

                    if (TAppSettingsManager.GetValue("Server.ChannelEncryption.PrivateKeyfile", "", false).Length > 0)
                    {
                        EncryptionSink = new EncryptionServerSinkProvider();
                        EncryptionSink.Next = TCPSink;
                    }

                    Hashtable ChannelProperties = new Hashtable();
                    ChannelProperties.Add("port", TAppSettingsManager.GetValue("Server.Port"));

                    string SpecificIPAddress = TAppSettingsManager.GetValue("ListenOnIPAddress", "", false);

                    if (SpecificIPAddress.Length > 0)
                    {
                        ChannelProperties.Add("machineName", SpecificIPAddress);
                    }

                    TcpChannel Channel = new TcpChannel(ChannelProperties, null, EncryptionSink);
                    ChannelServices.RegisterChannel(Channel, false);

                    RemotingConfiguration.RegisterWellKnownServiceType(typeof(Ict.Petra.Server.App.Core.TServerManager),
                        "Servermanager", WellKnownObjectMode.Singleton);
                    RemotingConfiguration.RegisterWellKnownServiceType(typeof(Ict.Common.Remoting.Server.TClientManager),
                        "Clientmanager", WellKnownObjectMode.Singleton);
                    RemotingConfiguration.RegisterWellKnownServiceType(typeof(TCrossDomainMarshaller),
                        TClientManager.CROSSDOMAINURL, WellKnownObjectMode.Singleton);

                    LifetimeServices.LeaseTime = TimeSpan.FromSeconds(TAppSettingsManager.GetDouble("LifetimeServices.LeaseTimeInSeconds", 5.0f));
                    LifetimeServices.RenewOnCallTime = TimeSpan.FromSeconds(TAppSettingsManager.GetDouble("LifetimeServices.RenewOnCallTime", 5.0f));
                    LifetimeServices.LeaseManagerPollTime =
                        TimeSpan.FromSeconds(TAppSettingsManager.GetDouble("LifetimeServices.LeaseManagerPollTime", 1.0f));
                }
                else
                {
                    TLogging.Log(Catalog.GetString("Reading server remote configuration from config file..."));

                    if (TheServerManager.ConfigurationFileName == "")
                    {
                        RemotingConfiguration.Configure(Environment.GetCommandLineArgs()[0] + ".config", false);
                    }
                    else
                    {
                        RemotingConfiguration.Configure(TheServerManager.ConfigurationFileName, false);
                    }

                    RemotingConfiguration.RegisterWellKnownServiceType(typeof(TCrossDomainMarshaller),
                        TClientManager.CROSSDOMAINURL, WellKnownObjectMode.Singleton);
                }
            }
            catch (RemotingException rex)
            {
                if (rex.Message.IndexOf("SocketException") > 1)
                {
                    TLogging.Log("A SocketException has been thrown.");
                    TLogging.Log("Most probably problem is that the address port is used twice!");
                    throw new ApplicationException();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }

            Thread.Sleep(50);
            TrackingServices.RegisterTrackingHandler(new TRemotingTracker());

            // Display information that the Server is ready to accept .NET Remoting requests
            TLogging.Log(TheServerManager.ServerInfoState);

            //
            // Server startup done.
            // From now on just listen on .NET Remoting Framework object invocations or on
            // menu commands...
            //

            bool RunWithoutMenu = TAppSettingsManager.GetBoolean("RunWithoutMenu", false);

            if ((!RunWithoutMenu))
            {
                Console.WriteLine(Environment.NewLine + Catalog.GetString("-> Press \"m\" for menu."));
                WriteServerPrompt();
            }

            // All exceptions that are raised from various parts of the Server are handled below.
            // Note: The Server stops after handling these exceptions!!!
            if (RunWithoutMenu)
            {
                RunInBackground();
            }
            else
            {
                RunMenu();
            }

            // THE VERY END OF THE SERVER :(
        }
        catch (System.Net.Sockets.SocketException exp)
        {
            TLogging.Log(
                Environment.NewLine + "Unable to start the Server: The IP Port " + TSrvSetting.IPBasePort.ToString() +
                " is being used by a different instance of the Server or some other application." + Environment.NewLine + exp.ToString());
        }
        catch (System.Runtime.Remoting.RemotingException exp)
        {
            System.Diagnostics.Debug.WriteLine(exp.ToString());
            TLogging.Log(Environment.NewLine + "Exception occured while setting up Remoting Framework:" + Environment.NewLine + exp.ToString());
        }
        catch (ApplicationException)
        {
            // This Exception is used if no more messages shall be done ...
        }
        catch (Exception exp)
        {
            TLogging.Log(Environment.NewLine + "Exception occured:" + Environment.NewLine + exp.ToString());
        }
    }

    private void RunInBackground()
    {
        do
        {
            /*
             * Infinite loop - ON PURPOSE !
             * Server can only be shutdown from PetraServerAdminConsole...
             */

            /*
             * Server main Thread goes to sleep and never needs to wake up again -
             * the PetraServer is only accessed through .NET Remoting and has no
             * interaction with the Console anymore.
             */
            Thread.Sleep(Timeout.Infinite);
        } while (!(false));
    }

    private void RunMenu()
    {
        bool ReadLineLoopEnd = false;
        bool EntryParsedOK = false;

        System.Int16 ClientID = 0;
        System.Int16 ClientTaskPriority = 1;

        do
        {
            string ServerCommand = (Console.ReadLine());

            if (ServerCommand.Length > 0)
            {
                ServerCommand = ServerCommand.Substring(0, 1);

                switch (Convert.ToChar(ServerCommand))
                {
                    case 'm':
                    case 'M':
                        Console.WriteLine(Environment.NewLine + "-> Available commands <-");
                        Console.WriteLine("     c: list connected Clients / C: list disconnected Clients");
                        Console.WriteLine("     d: disconnect a certain Client");

                        if (TLogging.DebugLevel > 0)
                        {
                            Console.WriteLine("     l: load AppDomain for a fake Client (for debugging purposes only!)");
                        }

                        Console.WriteLine("     p: perform timed server processing manually now");
                        Console.WriteLine("     q: queue a Client Task for a certain Client");
                        Console.WriteLine("     s: Server Status");

                        if (TLogging.DebugLevel > 0)
                        {
                            Console.WriteLine("     y: show Server memory");
                            Console.WriteLine("     g: perform Server garbage collection (for debugging purposes only!)");
                        }

                        Console.WriteLine("     e: export the database to yml.gz");
                        Console.WriteLine("     i: import a yml.gz, which will overwrite the database");

                        Console.WriteLine("     o: controlled Server shutdown (gets all connected clients to disconnect)");
                        Console.WriteLine("     u: unconditional Server shutdown (forces 'hard' disconnection of all Clients!)");
                        WriteServerPrompt();

                        // list connected Clients
                        break;

                    case 'c':
                        Console.WriteLine(Environment.NewLine + "-> Connected Clients <-");
                        Console.WriteLine(TheServerManager.FormatClientList(false));
                        WriteServerPrompt();

                        // list disconnected Clients
                        break;

                    case 'C':
                        Console.WriteLine(Environment.NewLine + "-> Disconnected Clients <-");
                        Console.WriteLine(TheServerManager.FormatClientList(true));
                        WriteServerPrompt();

                        // disconnect a certain Client
                        break;

                    case 'd':
                    case 'D':
                        Console.WriteLine(Environment.NewLine + "-> Disconnect a certain Client <-");

                        if (TheServerManager.ClientList.Count > 0)
                        {
                            Console.WriteLine(TheServerManager.FormatClientList(false));
                            Console.Write("     Enter ClientID: ");
                            string ConsoleInput = Console.ReadLine();
                            try
                            {
                                ClientID = System.Int16.Parse(ConsoleInput);

                                String CantDisconnectReason;

                                if (TheServerManager.DisconnectClient(ClientID, out CantDisconnectReason))
                                {
                                    TLogging.Log("Client #" + ClientID.ToString() + ": disconnection will take place shortly.");
                                }
                                else
                                {
                                    TLogging.Log(
                                        "Client #" + ClientID.ToString() + " could not be disconnected on admin request.  Reason: " +
                                        CantDisconnectReason);
                                }
                            }
                            catch (System.FormatException)
                            {
                                Console.WriteLine("  Entered ClientID is not numeric!");
                            }
                            catch (Exception exp)
                            {
                                TLogging.Log(
                                    Environment.NewLine + "Exception occured while trying to disconnect a Client on admin request:" +
                                    Environment.NewLine + exp.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("  * no Clients connected *");
                        }

                        WriteServerPrompt();

                        // load AppDomain for a fake Client (for debugging purposes only!)
                        break;


                    case 'e':
                    case 'E':
                        string YmlGZData = TImportExportWebConnector.ExportAllTables();
                        Console.Write("     Please enter filename of yml.gz file: ");
                        string backupFile = Path.GetFullPath(Console.ReadLine());

                        if (!backupFile.EndsWith(".yml.gz"))
                        {
                            Console.WriteLine("filename has to end with .yml.gz. Please try again");
                        }
                        else
                        {
                            FileStream fs = new FileStream(backupFile, FileMode.Create);
                            byte[] buffer = Convert.FromBase64String(YmlGZData);
                            fs.Write(buffer, 0, buffer.Length);
                            fs.Close();
                            TLogging.Log("backup has been written to " + backupFile);
                        }

                        WriteServerPrompt();
                        break;

                    case 'i':
                    case 'I':
                        Console.WriteLine(Environment.NewLine + "-> DELETING YOUR DATABASE <-");
                        Console.Write("     Enter YES to import the new database (anything else to leave command): ");

                        if (Console.ReadLine() == "YES")
                        {
                            Console.Write("     Please enter filename of yml.gz file: ");
                            string restoreFile = Path.GetFullPath(Console.ReadLine());

                            if (!File.Exists(restoreFile) || !restoreFile.EndsWith(".yml.gz"))
                            {
                                Console.WriteLine("invalid filename, please try again");
                            }
                            else
                            {
                                FileStream fsRead = new FileStream(restoreFile, FileMode.Open);
                                byte[] bufferRead = new byte[fsRead.Length];
                                fsRead.Read(bufferRead, 0, bufferRead.Length);
                                fsRead.Close();
                                YmlGZData = Convert.ToBase64String(bufferRead);

                                if (TImportExportWebConnector.ResetDatabase(YmlGZData))
                                {
                                    TLogging.Log("backup has been restored from " + restoreFile);
                                }
                                else
                                {
                                    TLogging.Log("there have been problems with the restore");
                                }
                            }

                            WriteServerPrompt();
                        }
                        else
                        {
                            Console.WriteLine("     Reset of database cancelled!");
                            WriteServerPrompt();
                        }

                        break;

                    case 'p':
                    case 'P':
                        string resp = "";

                        Console.WriteLine("  Server Timed Processing Status: " +
                        "runs daily at " + TheServerManager.TimedProcessingDailyStartTime24Hrs + ".");
                        Console.WriteLine("    Partner Reminders: " +
                        (TheServerManager.TimedProcessingJobEnabled("TProcessPartnerReminders") ? "On" : "Off"));
                        Console.WriteLine("    Automatic Intranet Export: " +
                        (TheServerManager.TimedProcessingJobEnabled("TProcessAutomatedIntranetExport") ? "On" : "Off"));
                        Console.WriteLine("    Data Checks: " + (TheServerManager.TimedProcessingJobEnabled("TProcessDataChecks") ? "On" : "Off"));

                        Console.WriteLine("  SMTP Server used for sending e-mails: " + TheServerManager.SMTPServer);

                        if (TheServerManager.TimedProcessingJobEnabled("TProcessPartnerReminders"))
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Do you want to run Reminder Processing now?");
                            Console.Write("Type YES to continue, anything else to skip:");
                            resp = Console.ReadLine();

                            if (resp == "YES")
                            {
                                TheServerManager.PerformTimedProcessingNow("TProcessPartnerReminders");
                            }
                        }

                        if (TheServerManager.TimedProcessingJobEnabled("TProcessAutomatedIntranetExport"))
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Do you want to run Intranet Export Processing now?");
                            Console.Write("Type YES to continue, anything else to skip:");
                            resp = Console.ReadLine();

                            if (resp == "YES")
                            {
                                TheServerManager.PerformTimedProcessingNow("TProcessAutomatedIntranetExport");
                            }
                        }

                        if (TheServerManager.TimedProcessingJobEnabled("TProcessDataChecks"))
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Do you want to run Data Checks Processing now?");
                            Console.Write("Type YES to continue, anything else to skip:");
                            resp = Console.ReadLine();

                            if (resp == "YES")
                            {
                                TheServerManager.PerformTimedProcessingNow("TProcessDataChecks");
                            }
                        }

                        WriteServerPrompt();
                        break;

                    case 's':
                    case 'S':
                        Console.WriteLine(Environment.NewLine + "-> Server Status <-");
                        Console.WriteLine();

                        DisplayPetraServerInformation(TheServerManager);

                        WriteServerPrompt();

                        break;

                    case 'q':
                    case 'Q':
                        Console.WriteLine(Environment.NewLine + "-> Queue a Client Task for a certain Client <-");

                        if (TheServerManager.ClientList.Count > 0)
                        {
                            Console.WriteLine(TheServerManager.FormatClientList(false));
ReadClientID:
                            Console.Write("     Enter ClientID: ");
                            string ConsoleInput = Console.ReadLine();
                            try
                            {
                                ClientID = System.Int16.Parse(ConsoleInput);
                                EntryParsedOK = true;
                            }
                            catch (System.FormatException)
                            {
                                Console.WriteLine("  Entered ClientID is not numeric!");
                                EntryParsedOK = false;
                            }

                            if (!EntryParsedOK)
                            {
                                goto ReadClientID;
                            }

                            Console.Write("     Enter Client Task Group: ");
                            string ClientTaskGroup = Console.ReadLine();
                            Console.Write("     Enter Client Task Code: ");
                            string ClientTaskCode = Console.ReadLine();
ReadClientTaskPriority:
                            Console.Write("     Enter Client Task Priority: ");
                            ConsoleInput = Console.ReadLine();
                            try
                            {
                                ClientTaskPriority = System.Int16.Parse(ConsoleInput);
                                EntryParsedOK = true;
                            }
                            catch (System.FormatException)
                            {
                                Console.WriteLine("  Entered Client Task Priority is not numeric!");
                                EntryParsedOK = false;
                            }

                            if (!EntryParsedOK)
                            {
                                goto ReadClientTaskPriority;
                            }

                            try
                            {
                                if (TheServerManager.QueueClientTask(ClientID, ClientTaskGroup, ClientTaskCode, ClientTaskPriority))
                                {
                                    TLogging.Log("Client Task queued for Client #" + ClientID.ToString() + " on admin request.");
                                }
                                else
                                {
                                    TLogging.Log(
                                        "Client Task for Client #" + ClientID.ToString() + " could not be queued on admin request.");
                                }
                            }
                            catch (Exception exp)
                            {
                                TLogging.Log(
                                    Environment.NewLine + "Exception occured while queueing a Client Task on admin request:" +
                                    Environment.NewLine + exp.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("  * no Clients connected *");
                        }

                        WriteServerPrompt();

                        // show Server memory
                        break;

                    case 'y':
                    case 'Y':
                        Console.WriteLine("Server memory: " + TheServerManager.ServerInfoMemory.ToString());
                        WriteServerPrompt();

                        // perform Server garbage collection
                        break;

                    case 'g':
                    case 'G':
                        Console.WriteLine("GarbageCollection performed. Server memory: " + TheServerManager.PerformGC().ToString());
                        WriteServerPrompt();

                        // unconditional Server shutdown
                        break;

                    case 'o':
                    case 'O':
                        Console.WriteLine(Environment.NewLine + "-> CONTROLLED SHUTDOWN  (gets all connected clients to disconnect) <-");
                        Console.Write("     Enter YES to perform controlled shutdown (anything else to leave command): ");

                        if (Console.ReadLine() == "YES")
                        {
                            if (!TheServerManager.StopServerControlled(false))
                            {
                                Console.WriteLine("     Shutdown cancelled!");
                                WriteServerPrompt();
                            }
                        }
                        else
                        {
                            Console.WriteLine("     Shutdown cancelled!");
                            WriteServerPrompt();
                        }

                        break;

                    case 'u':
                    case 'U':
                        Console.WriteLine(Environment.NewLine + "-> UNCONDITIONAL SHUTDOWN   (force disconnection of all Clients) <-");
                        Console.Write("     Enter YES to perform UNCONDITIONAL shutdown (anything else to leave command): ");

                        if (Console.ReadLine() == "YES")
                        {
                            TheServerManager.StopServer();
                            ReadLineLoopEnd = true;
                        }
                        else
                        {
                            Console.WriteLine("     Shutdown cancelled!");
                            WriteServerPrompt();
                        }

                        break;

                    default:
                        Console.WriteLine(
                        Environment.NewLine + "-> Unrecognised command '" + ServerCommand + "' <-   (Press 'm' for menu)");
                        WriteServerPrompt();
                        break;
                }

                // case Convert.ToChar( ServerCommand )
            }
            else
            {
                WriteServerPrompt();
            }
        } while (!(ReadLineLoopEnd == true));
    }

    /// <summary>
    /// Displays information about the Server we are connected to.
    /// </summary>
    /// <param name="AServerManager">Instance of remote ServerManager</param>
    void DisplayPetraServerInformation(IServerAdminInterface AServerManager)
    {
        Console.WriteLine(AServerManager.ServerInfoVersion);
        Console.WriteLine("  " +
            String.Format(Catalog.GetString("Client connections since Server start: {0}"), AServerManager.ClientsConnectedTotal));
        Console.WriteLine("  " + String.Format(Catalog.GetString("Clients currently connected: "), AServerManager.ClientsConnected));

        Console.WriteLine(AServerManager.ServerInfoState);
    }
}
#endregion

/// <summary>
/// Tracks all .NET Remoting events (for debugging purposes only)
///
/// </summary>
public class TRemotingTracker : object, ITrackingHandler
{
    #region TRemotingTracker

    /// <summary>
    /// Gets called whenever a remoted object has been disconnected from its proxy.
    ///
    /// </summary>
    /// <param name="AObj">The disconnected object
    /// </param>
    /// <returns>void</returns>
    public void DisconnectedObject(System.Object AObj)
    {
        Console.WriteLine(("Disconnected " + AObj.GetType().Name));
    }

    /// <summary>
    /// Gets called whenever .NET Remoting 'unmarshals' (un-remotes) an object.
    ///
    /// </summary>
    /// <param name="AObj">The object that has been unmarshaled</param>
    /// <param name="AOr">The ObjRef that results from marshaling and represents the
    /// specified object
    /// </param>
    /// <returns>void</returns>
    public void UnmarshaledObject(System.Object AObj, ObjRef AOr)
    {
        // Console.WriteLine(((('Unmarshaled ' + AObj.GetType.Name) + ' local:') + AOr.IsFromThisAppDomain.ToString));
    }

    /// <summary>
    /// Gets called whenever .NET Remoting 'marshals' (remotes) an object.
    ///
    /// </summary>
    /// <param name="AObj">The object that has been marshaled</param>
    /// <param name="AOr">The ObjRef that results from marshaling and represents the
    /// specified object
    /// </param>
    /// <returns>void</returns>
    public void MarshaledObject(System.Object AObj, ObjRef AOr)
    {
        // Console.WriteLine(('Marshaled ' + AObj.GetType.Name));
    }

    #endregion
}
}