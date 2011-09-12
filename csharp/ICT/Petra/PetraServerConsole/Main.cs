//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;
using System.Threading;
using GNU.Gettext;

using Ict.Common;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Main;
using Ict.Petra.Shared.Interfaces.ServerAdminInterface;

using Ict.Petra.Server.MFinance.GL.WebConnectors;

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

    /// <summary>
    /// Starts the Petra Server.
    ///
    /// </summary>
    /// <returns>void</returns>
    public void Startup()
    {
        TServerManager TheServerManager;
        bool ReadLineLoopEnd;
        bool EntryParsedOK;

        System.Int16 ClientID = 0;
        System.Int16 ClientTaskPriority = 1;
        String ServerCommand;
        String ConsoleInput;
        String ClientTaskCode;
        String ClientTaskGroup;
        bool RunWithoutMenu;

//              Assembly tmpAssembly;
        String CantDisconnectReason;

        try
        {
            //
            // Uncomment the following lines to see which DLL's are loaded into the Default AppDomain at application start.
            // It can help in identifying which DLL's are loaded later in addition to those that were loaded at application start.
            //
            // $IFDEF DEBUGMODE
            // Console.WriteLine('Loaded Assemblies in AppDomain ' + Thread.GetDomain.FriendlyName + ' (at Server start):');
            // for tmpAssembly in Thread.GetDomain.GetAssemblies() do
            // begin
            // Console.WriteLine(tmpAssembly.FullName);
            // end;
            // $ENDIF

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
                TLogging.Log("May be a nant initConfigFile helps ...");
                throw new ApplicationException();
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
                // iServerPort := 9000;
                //
                // tChannel := new TcpServerChannel(iServerPort);
                // ChannelServices.RegisterChannel(tChannel);
                // RemotingConfiguration.RegisterWellKnownServiceType(TypeOf(TheServerManager.TTheServerManager),
                // 'Servermanager', WellKnownObjectMode.Singleton);
                TLogging.Log(Catalog.GetString("Reading server remote configuration from config file..."));

                if (TheServerManager.ConfigurationFileName == "")
                {
                    RemotingConfiguration.Configure(Environment.GetCommandLineArgs()[0] + ".config", false);
                }
                else
                {
                    RemotingConfiguration.Configure(TheServerManager.ConfigurationFileName, false);
                }
            }
            catch (RemotingException rex)
            {
                if (rex.Message.IndexOf("SocketException") > 1)
                {
                    TLogging.Log("A SocketException has been thrown.");
                    TLogging.Log("Most probably problem is that the adress port is used twice!");
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
#if  RUNWITHOUTMENU
            RunWithoutMenu = true;
#else
            RunWithoutMenu = TAppSettingsManager.ToBoolean(TAppSettingsManager.GetValue("RunWithoutMenu", "false"), false);

            if ((!RunWithoutMenu))
            {
                Console.WriteLine(Environment.NewLine + Catalog.GetString("-> Press \"m\" for menu."));
                WriteServerPrompt();
            }
#endif
            ReadLineLoopEnd = false;

            if ((!RunWithoutMenu))
            {
                do
                {
                    ServerCommand = (Console.ReadLine());

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
#if DEBUGMODE
                                Console.WriteLine("     l: load AppDomain for a fake Client (for debugging purposes only!)");
#endif
                                Console.WriteLine("     q: queue a Client Task for a certain Client");
                                Console.WriteLine("     s: Server Status");
#if DEBUGMODE
                                Console.WriteLine("     y: show Server memory");
                                Console.WriteLine("     g: perform Server garbage collection (for debugging purposes only!)");
#endif

                                Console.WriteLine("     u: unconditional Server shutdown (forces disconnection of all Clients!)");
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
                                    ConsoleInput = Console.ReadLine();
                                    try
                                    {
                                        ClientID = System.Int16.Parse(ConsoleInput);

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

                            case 's':
                            case 'S':
                                Console.WriteLine(Environment.NewLine + "-> Server Status <-");
                                Console.WriteLine();

                                DisplayPetraServerInformation(TheServerManager);

                                WriteServerPrompt();

                                break;

                            case 'l':
                            case 'L':
                                Console.Write("     Enter fake UserName for which an AppDomain should be loaded: ");
                                ConsoleInput = Console.ReadLine();

                                if (TheServerManager.LoadClientAppDomain(ConsoleInput))
                                {
                                    TLogging.Log("AppDomain for User " + ConsoleInput + " loaded on admin request.");
                                }
                                else
                                {
                                    TLogging.Log("AppDomain for User " + ConsoleInput + " could not be loaded on admin request!");
                                }

                                // queue a Client Task for a certain Client
                                break;

                            case 'q':
                            case 'Q':
                                Console.WriteLine(Environment.NewLine + "-> Queue a Client Task for a certain Client <-");

                                if (TheServerManager.ClientList.Count > 0)
                                {
                                    Console.WriteLine(TheServerManager.FormatClientList(false));
ReadClientID:
                                    Console.Write("     Enter ClientID: ");
                                    ConsoleInput = Console.ReadLine();
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
                                    ClientTaskGroup = Console.ReadLine();
                                    Console.Write("     Enter Client Task Code: ");
                                    ClientTaskCode = Console.ReadLine();
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

                            case 'u':
                            case 'U':
                                Console.WriteLine(Environment.NewLine + "-> UNCONDITIONAL SHUTDOWN   (force disconnection of all Clients) <-");
                                Console.Write("     Enter YES to perform shutdown (anything else to leave command): ");

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
            else
            {
                do
                {
                    /*
                     * Infinite loop - ON PURPOSE !
                     * Server can only be shutdown from PetraServerAdminConsole...
                     */

                    /*
                     * Access a Property of TTheServerManager - without doing anything
                     * with the result value.   Stupid ? - NO !!!
                     *
                     * If this is not done, .NET is 'clever' and figures out that the Instance
                     * of TTheServerManager is no longer needed in the whole lifetime of the
                     * PetraServer Process - which is correct when looking a further lines
                     * down, where the Process will end after this loop, but is wrong in our
                     * case, because TTheServerManager can be accessed using the
                     * PetraServerAdminConsole via .NET Remoting.
                     *
                     * Therefore reading the Property is needed to keep the Instance of
                     * TTheServerManager from being GC'ed, and therefore from running its
                     * Finalizer, which would Log 'SERVER STOPPED!' as soon as GC kicks in
                     * (eg. when a Client connects)!
                     *
                     */
                    int TmpNeededForCheatingOnGarbageCollection = TheServerManager.IPPort;

                    /*
                     * Server main Thread goes to sleep and never needs to wake up again -
                     * the PetraServer is only accessed through .NET Remoting and has no
                     * interaction with the Console anymore.
                     */
                    Thread.Sleep(Timeout.Infinite);
                } while (!(false));
            }

            // All exceptions that are raised from various parts of the Server are handled here
            // Note: The Server stops after handling these exceptions!!!
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

        // THE VERY END OF THE SERVER :(
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