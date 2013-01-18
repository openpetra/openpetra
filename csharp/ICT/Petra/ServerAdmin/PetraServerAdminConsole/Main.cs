//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2012 by OM International
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
using System.Net.Sockets;
using System.Runtime.Remoting;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Petra.ServerAdmin.App.Core;
using System.Reflection;
using System.Diagnostics;
using System.IO;


namespace PetraServerAdminConsole
{
/// <summary>
///  Petra Server Admin Command Line Application
/// </summary>
public class TAdminConsole
{
    /// <summary>
    /// the command prompt
    /// </summary>
    public const String ServerAdminPrompt = "SERVERADMIN> ";

    /// <summary>
    /// todoComment
    /// </summary>
    /// <param name="remexp"></param>
    public static void HandleConnectionError(Exception remexp)
    {
        String ServerAdminCommand;

        TLogging.Log("PETRAServer is not running or cannot be reached!");
        Console.WriteLine();
        Console.WriteLine("Press ENTER to end PETRAServerADMIN, or 'D' for details...");
        Console.Write(ServerAdminPrompt);
        ServerAdminCommand = (Console.ReadLine().ToLower());

        if (ServerAdminCommand == "d")
        {
            Console.WriteLine(remexp);
        }
    }

    /// <summary>
    /// shut down the server (gets all connected clients to disconnect)
    /// </summary>
    /// <param name="TRemote"></param>
    /// <param name="AWithUserInteraction"></param>
    /// <returns>true if shutdown was completed</returns>
    public static bool ShutDownControlled(IServerAdminInterface TRemote, bool AWithUserInteraction)
    {
        bool ReturnValue;
        bool ack;

        ack = false;

        if (AWithUserInteraction == true)
        {
            Console.WriteLine(Environment.NewLine + "-> CONTROLLED SHUTDOWN  (gets all connected clients to disconnect) <-");
            Console.Write("     Enter YES to perform shutdown (anything else to leave command): ");

            if (Console.ReadLine() == "YES")
            {
                Console.WriteLine();
                ack = true;
            }
        }
        else
        {
            ack = true;
        }

        if (ack == true)
        {
            TLogging.Log("CONTROLLED SHUTDOWN PROCEDURE INITIATED...");
            try
            {
                if (!TRemote.StopServerControlled(true)) 
                {
                    Console.WriteLine("     Shutdown cancelled!");
                    Console.Write(ServerAdminPrompt);
                    ReturnValue = false;
                }
            }
            catch (SocketException)
            {
                if (AWithUserInteraction == true)
                {
                    Console.WriteLine();
                    TLogging.Log("SERVER STOPPED!");
                    Console.WriteLine();
                    Console.Write("Press ENTER to end PETRAServerADMIN...");
                    Console.ReadLine();
                }
            }
            ReturnValue = true;
        }
        else
        {
            Console.WriteLine("     Shutdown cancelled!");
            Console.Write(ServerAdminPrompt);
            ReturnValue = false;
        }

        return ReturnValue;
    }
    
    /// <summary>
    /// shut down the server
    /// </summary>
    /// <param name="TRemote"></param>
    /// <param name="AWithUserInteraction"></param>
    /// <returns>true if shutdown was completed</returns>
    public static bool ShutDown(IServerAdminInterface TRemote, bool AWithUserInteraction)
    {
        bool ReturnValue;
        bool ack;

        ack = false;

        if (AWithUserInteraction == true)
        {
            Console.WriteLine(Environment.NewLine + "-> UNCONDITIONAL SHUTDOWN   (force disconnection of all Clients) <-");
            Console.Write("     Enter YES to perform shutdown (anything else to leave command): ");

            if (Console.ReadLine() == "YES")
            {
                Console.WriteLine();
                ack = true;
            }
        }
        else
        {
            ack = true;
        }

        if (ack == true)
        {
            TLogging.Log("SHUTDOWN PROCEDURE INITIATED...");
            try
            {
                TRemote.StopServer();
            }
            catch (SocketException)
            {
                if (AWithUserInteraction == true)
                {
                    Console.WriteLine();
                    TLogging.Log("SERVER STOPPED!");
                    Console.WriteLine();
                    Console.Write("Press ENTER to end PETRAServerADMIN...");
                    Console.ReadLine();
                }
            }
            ReturnValue = true;
        }
        else
        {
            Console.WriteLine("     Shutdown cancelled!");
            Console.Write(ServerAdminPrompt);
            ReturnValue = false;
        }

        return ReturnValue;
    }

    /// <summary>
    /// disconnect a client
    /// </summary>
    /// <param name="TRemote"></param>
    /// <param name="ConsoleInput"></param>
    public static void DisconnectClient(IServerAdminInterface TRemote, String ConsoleInput)
    {
        Int16 ClientID;
        String CantDisconnectReason;

        try
        {
            ClientID = System.Int16.Parse(ConsoleInput);

            if (TRemote.DisconnectClient(ClientID, out CantDisconnectReason))
            {
                TLogging.Log("Client #" + ClientID.ToString() + ": disconnection will take place shortly.");
            }
            else
            {
                TLogging.Log("Client #" + ClientID.ToString() + " could not be disconnected on admin request.  Reason: " + CantDisconnectReason);
            }
        }
        catch (System.FormatException)
        {
            Console.WriteLine("  Entered ClientID is not numeric!");
        }
        catch (Exception exp)
        {
            TLogging.Log(
                Environment.NewLine + "Exception occured while trying to disconnect a Client on admin request:" + Environment.NewLine + exp.ToString());
        }
    }

    private static void ExportDatabase(IServerAdminInterface TRemote)
    {
        Console.Write("     Please enter filename of yml.gz file: ");
        string backupFile = Path.GetFullPath(Console.ReadLine());

        if (!backupFile.EndsWith(".yml.gz"))
        {
            Console.WriteLine("filename has to end with .yml.gz. Please try again");
            return;
        }

        string YmlGZData = TRemote.BackupDatabaseToYmlGZ();

        FileStream fs = new FileStream(backupFile, FileMode.Create);
        byte[] buffer = Convert.FromBase64String(YmlGZData);
        fs.Write(buffer, 0, buffer.Length);
        fs.Close();
        TLogging.Log("backup has been written to " + backupFile);
    }

    private static bool RestoreDatabase(IServerAdminInterface TRemote, string ARestoreFile)
    {
        string restoreFile = Path.GetFullPath(ARestoreFile);

        if (!File.Exists(restoreFile) || !restoreFile.EndsWith(".yml.gz"))
        {
            Console.WriteLine("invalid filename, please try again");
            return false;
        }

        string YmlGZData = string.Empty;

        try
        {
            FileStream fs = new FileStream(restoreFile, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            YmlGZData = Convert.ToBase64String(buffer);
        }
        catch (Exception e)
        {
            TLogging.Log("cannot open file " + restoreFile);
            TLogging.Log(e.ToString());
            return false;
        }

        if (TRemote.RestoreDatabaseFromYmlGZ(YmlGZData))
        {
            TLogging.Log("backup has been restored from " + restoreFile);
            return true;
        }
        else
        {
            TLogging.Log("there have been problems with the restore");
            return false;
        }
    }

    private static void RestoreDatabase(IServerAdminInterface TRemote)
    {
        Console.WriteLine(Environment.NewLine + "-> DELETING YOUR DATABASE <-");
        Console.Write("     Enter YES to import the new database (anything else to leave command): ");

        if (Console.ReadLine() == "YES")
        {
            Console.Write("     Please enter filename of yml.gz file: ");
            string restoreFile = Console.ReadLine();

            RestoreDatabase(TRemote, restoreFile);
        }
        else
        {
            Console.WriteLine("     Reset of database cancelled!");
        }
    }

    private static void AddUser(IServerAdminInterface TRemote, string AUserId)
    {
        TRemote.AddUser(AUserId);
    }

    /// <summary>
    /// shows the menu and processes the selections of the administrator
    /// </summary>
    /// <param name="TRemote"></param>
    public static void Menu(IServerAdminInterface TRemote)
    {
        bool ReadLineLoopEnd;
        bool EntryParsedOK;
        String ServerAdminCommand;
        String ConsoleInput;
        String ClientTaskCode;
        String ClientTaskGroup;

        System.Int16 ClientID = 0;                                          // assignment only to make code compile; has no functional implication
        System.Int16 ClientTaskPriority = 1;                        // assignment only to make code compile; has no functional implication

        // label
        // ReadClientID,               used only for repeating invalid command line input
        // ReadClientTaskPriority;     used only for repeating invalid command line input

        DisplayPetraServerInformation(TRemote);

        //
        // Startup done.
        // From now on just listen on menu commands...
        //
        Console.WriteLine(Environment.NewLine + "-> Press \"m\" for menu.");
        Console.Write(ServerAdminPrompt);

        // ServerAdmin stops after leaving the following loop...!
        ReadLineLoopEnd = false;

        do
        {
            ServerAdminCommand = (Console.ReadLine());

            if (ServerAdminCommand.Length > 0)
            {
                ServerAdminCommand = ServerAdminCommand.Substring(0, 1);

                switch (Convert.ToChar(ServerAdminCommand))
                {
                    case 'm':
                    case 'M':
                        Console.WriteLine(Environment.NewLine + "-> Available commands <-");
                        Console.WriteLine("     c: list connected Clients / C: list disconnected Clients");
                        Console.WriteLine("     d: disconnect a certain Client");
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
                        
                        Console.WriteLine("     x: exit PETRAServerADMIN");
                        Console.Write(ServerAdminPrompt);
                        break;

                    case 'c':
                        Console.WriteLine(Environment.NewLine + "-> Connected Clients <-");
                        Console.WriteLine(TRemote.FormatClientList(false));
                        Console.Write(ServerAdminPrompt);
                        break;

                    case 'C':
                        Console.WriteLine(Environment.NewLine + "-> Disconnected Clients <-");
                        Console.WriteLine(TRemote.FormatClientList(true));
                        Console.Write(ServerAdminPrompt);
                        break;

                    case 'd':
                    case 'D':
                        Console.WriteLine(Environment.NewLine + "-> Disconnect a certain Client <-");

                        if (TRemote.ClientList.Count > 0)
                        {
                            Console.WriteLine(TRemote.FormatClientList(false));
                            Console.Write("     Enter ClientID: ");
                            ConsoleInput = Console.ReadLine();
                            DisconnectClient(TRemote, ConsoleInput);
                        }
                        else
                        {
                            Console.WriteLine("  * no Clients connected *");
                        }

                        Console.Write(ServerAdminPrompt);

                        // queue a Client Task for a certain Client
                        break;

                    case 'e':
                    case 'E':
                        Console.WriteLine(Environment.NewLine + "-> Export the database to yml.gz file <-");

                        ExportDatabase(TRemote);

                        Console.Write(ServerAdminPrompt);

                        // queue a Client Task for a certain Client
                        break;

                    case 'i':
                    case 'I':
                        Console.WriteLine(Environment.NewLine + "-> Restore the database from yml.gz file <-");

                        RestoreDatabase(TRemote);

                        Console.Write(ServerAdminPrompt);

                        // queue a Client Task for a certain Client
                        break;

                    case 's':
                    case 'S':
                        Console.WriteLine(Environment.NewLine + "-> Server Status <-");
                        Console.WriteLine();

                        DisplayPetraServerInformation(TRemote);

                        Console.Write(ServerAdminPrompt);

                        break;

                    case 'q':
                    case 'Q':
                        Console.WriteLine(Environment.NewLine + "-> Queue a Client Task for a certain Client <-");

                        if (TRemote.ClientList.Count > 0)
                        {
                            Console.WriteLine(TRemote.FormatClientList(false));

                            // ReadClientID:
                            Console.Write("     Enter ClientID: ");
                            ConsoleInput = Console.ReadLine();
                            EntryParsedOK = false;
                            try
                            {
                                ClientID = System.Int16.Parse(ConsoleInput);
                                EntryParsedOK = true;
                            }
                            catch (System.FormatException)
                            {
                                Console.WriteLine("  Entered ClientID is not numeric!");
                            }

                            if (!EntryParsedOK)
                            {
                            }

                            // goto ReadClientID;
                            Console.Write("     Enter Client Task Group: ");
                            ClientTaskGroup = Console.ReadLine();
                            Console.Write("     Enter Client Task Code: ");
                            ClientTaskCode = Console.ReadLine();

                            // ReadClientTaskPriority:
                            Console.Write("     Enter Client Task Priority: ");
                            ConsoleInput = Console.ReadLine();
                            ClientTaskPriority = -1;
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
                            }

                            // goto ReadClientTaskPriority;
                            try
                            {
                                if (TRemote.QueueClientTask(ClientID, ClientTaskGroup, ClientTaskCode, ClientTaskPriority))
                                {
                                    TLogging.Log("Client Task queued for Client #" + ClientID.ToString() + " on admin request.");
                                }
                                else
                                {
                                    TLogging.Log("Client Task for Client #" + ClientID.ToString() + " could not be queued on admin request.");
                                }
                            }
                            catch (Exception exp)
                            {
                                TLogging.Log(
                                    Environment.NewLine + "Exception occured while queueing a Client Task on admin request:" + Environment.NewLine +
                                    exp.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("  * no Clients connected *");
                        }

                        Console.Write(ServerAdminPrompt);
                        break;

                    case 'y':
                    case 'Y':
                        Console.WriteLine("Server memory: " + TRemote.ServerInfoMemory.ToString());
                        Console.Write(ServerAdminPrompt);
                        break;

                    case 'g':
                    case 'G':
                        GC.Collect();
                        Console.WriteLine("GarbageCollection performed. Server memory: " + TRemote.PerformGC().ToString());
                        Console.Write(ServerAdminPrompt);
                        break;

                    case 'o':
                    case 'O':
                        ReadLineLoopEnd = ShutDownControlled(TRemote, true);
                        break;

                    case 'u':
                    case 'U':
                        ReadLineLoopEnd = ShutDown(TRemote, true);
                        break;

                    case 'x':
                    case 'X':

                        // exit loop, ServerAdmin stops then
                        ReadLineLoopEnd = true;
                        break;

                    default:
                        Console.WriteLine(Environment.NewLine + "-> Unrecognised command '" + ServerAdminCommand + "' <-   (Press 'm' for menu)");
                        Console.Write(ServerAdminPrompt);
                        break;
                }

                // case Convert.ToChar( ServerAdminCommand )
            }
            // if ServerAdminCommand.Length > 0
            else
            {
                Console.Write(ServerAdminPrompt);
            }
        } while (!(ReadLineLoopEnd == true));
    }

    /// <summary>
    /// Retrieve information about connected Clients of the Server we are
    /// connected to.
    /// </summary>
    /// <param name="ARemote">Instance of remote ServerManager</param>
    /// <param name="ATotalConnectedClients">Total number of Clients that
    /// have been connected while the PetraServer has been running.</param>
    /// <param name="ACurrentlyConnectedClients">Number of currently
    /// connected Clients.</param>
    static void RetrieveConnectedClients(IServerAdminInterface ARemote, out int ATotalConnectedClients, out int ACurrentlyConnectedClients)
    {
        ATotalConnectedClients = -1;
        ACurrentlyConnectedClients = -1;

        try
        {
            ATotalConnectedClients = ARemote.ClientsConnectedTotal;
            ACurrentlyConnectedClients = ARemote.ClientsConnected;
        }
        catch (RemotingException remexp)
        {
            HandleConnectionError(remexp);

            Environment.Exit(0);

            // PetraServerAdminConsole application stops here !!!
        }
        catch (System.Net.Sockets.SocketException remexp)
        {
            HandleConnectionError(remexp);
            
            Environment.Exit(0);

            // PetraServerAdminConsole application stops here !!!
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Displays information about the Server we are connected to.
    /// </summary>
    /// <param name="AServerManager">Instance of remote ServerManager</param>
    static void DisplayPetraServerInformation(IServerAdminInterface AServerManager)
    {
        int TotalConnectedClients;
        int CurrentlyConnectedClients;

        RetrieveConnectedClients(AServerManager, out TotalConnectedClients, out CurrentlyConnectedClients);

        TLogging.Log(AServerManager.ServerInfoVersion);
        TLogging.Log("  Clients connections since Server start: " + TotalConnectedClients.ToString());
        TLogging.Log("  Clients currently connected: " + CurrentlyConnectedClients.ToString());

        TLogging.Log(AServerManager.ServerInfoState);
    }

    /// <summary>
    /// Connects to the PetraServer and provides a menu with a number of functions,
    /// including stopping the PetraServer.
    /// </summary>
    /// <returns>void</returns>
    public static void Start()
    {
        Ict.Petra.ServerAdmin.App.Core.TConnector TheConnector;
        IServerAdminInterface TRemote;
        String ClientID;
        Boolean SilentSysadm;
        SilentSysadm = false;

        try
        {
            new TLogging();
            new TAppSettingsManager();
            SilentSysadm = true;

            if (TAppSettingsManager.HasValue("DebugLevel"))
            {
                TLogging.DebugLevel = TAppSettingsManager.GetInt32("DebugLevel");
            }

            if ((!TAppSettingsManager.HasValue("Command") || (TAppSettingsManager.GetValue("Command") == "Stop")))
            {
                SilentSysadm = false;
            }

            if (TAppSettingsManager.HasValue("ServerAdmin.LogFile"))
            {
                new TLogging(TAppSettingsManager.GetValue("ServerAdmin.LogFile"));
            }

            if ((!SilentSysadm))
            {
                Console.WriteLine();
                TLogging.Log(
                    "PETRAServerADMIN " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString() + ' ' + "Build " +
                    System.IO.File.GetLastWriteTime(
                        Process.GetCurrentProcess().MainModule.FileName).ToString() + " (OS: " +
                    CommonTypes.ExecutingOSEnumToString(Utilities.DetermineExecutingOS()) + ')');

                // System.Reflection.Assembly.GetEntryAssembly.FullName does not return the file path
                TLogging.Log("Connecting to PETRAServer...");
                Console.WriteLine();
            }

            // Instantiate a TServerManager object, which has all logic for Server
            // startup and shutdown
            TheConnector = new Ict.Petra.ServerAdmin.App.Core.TConnector();
            TheConnector.GetServerConnection(TAppSettingsManager.ConfigFileName, out TRemote);

            if (TAppSettingsManager.HasValue("Command"))
            {
                if (TAppSettingsManager.GetValue("Command") == "Stop")
                {
                    ShutDown(TRemote, false);
                }
                else if (TAppSettingsManager.GetValue("Command") == "StopAndCloseClients")
                {
                    ShutDownControlled(TRemote, false);
                }                
                else if (TAppSettingsManager.GetValue("Command") == "ConnectedClients")
                {
                    System.Console.WriteLine(TRemote.FormatClientList(false));
                }
                else if (TAppSettingsManager.GetValue("Command") == "ConnectedClientsSysadm")
                {
                    System.Console.WriteLine(TRemote.FormatClientListSysadm(false));
                }
                else if (TAppSettingsManager.GetValue("Command") == "DisconnectedClients")
                {
                    System.Console.WriteLine(TRemote.FormatClientList(true));
                }
                else if (TAppSettingsManager.GetValue("Command") == "DisconnectClient")
                {
                    ClientID = TAppSettingsManager.GetValue("ClientID");
                    DisconnectClient(TRemote, ClientID);
                }
                else if (TAppSettingsManager.GetValue("Command") == "LoadYmlGz")
                {
                    RestoreDatabase(TRemote, TAppSettingsManager.GetValue("YmlGzFile"));
                }
                else if (TAppSettingsManager.GetValue("Command") == "AddUser")
                {
                    AddUser(TRemote, TAppSettingsManager.GetValue("UserId"));
                }
            }
            else
            {
                Menu(TRemote);
            }

            // All exceptions that are raised are handled here
            // Note: ServerAdmin stops after handling these exceptions!!!
        }
        catch (RemotingException remexp)
        {
            if ((!SilentSysadm))
            {
                Console.WriteLine("RemotingException occured while connecting/communicating to PETRAServer: " + remexp.Message);
            }

            return;
        }
        catch (Exception exp)
        {
            if ((!SilentSysadm))
            {
                Console.WriteLine("Exception occured while connecting/communicating to PETRAServer: " + exp.ToString());
            }

            return;
        }

        // THE VERY END OF SERVERADMIN :(
    }
}
}