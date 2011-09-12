//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Remoting.Shared;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Creates a Tread which regularly calls into remoted server-side Objects that
    /// are registered with this Class. This call increases the remoted Objects'
    /// ILease CurrentLeaseTime so that they don't get marked for GC.
    ///
    /// If this signal is no longer received by a remoted server-side Object, the
    /// Object is marked for GC by the LeaseManager in the PetraServer as soon as its
    /// CurrentLeaseTime gets to zero!
    ///
    /// @comment Usage: Call Register for registering remoted Objects and UnRegister for
    /// releasing remoted Objects. Objects that are not UnRegistered will live on
    /// the PetraServer until the AppDomain for the Client is destroyed!!!
    ///
    /// </summary>
    public class TEnsureKeepAlive
    {
        /// <summary>todoComment</summary>
        public const String StrConnectionBroken =
            "The connection to the Petra Server has broken.\r\n\r\n==> Unfortunately you will need to close Petra and log in again. <==";

        /// <summary>todoComment</summary>
        public const String StrConnectionBrokenTitle = "SERVER CONNECTION BROKEN!";

        /// <summary>todoComment</summary>
        public const String StrConnectionClosed =
            "The connection to the Petra Server has been closed by the Petra Server.\r\n\r\n==> Unfortunately you will need to close Petra and log in again. <==";

        /// <summary>todoComment</summary>
        public const String StrConnectionClosedTitle = "SERVER CONNECTION CLOSED BY PETRA SERVER!";

        /// <summary>todoComment</summary>
        public const String StrConnectionUnavailableCause = "\r\n\r\nDEBUG INFORMATION: Actual cause for the problem: \r\n";

        /// <summary>Keeps the Registered Objects</summary>
        private static SortedList UKeepAliveObjects;

        /// <summary>Needs to be true as long as the Thread should still execute</summary>
        private static bool UKeepRemotedObjectsAlive;

        #region TEnsureKeepAlive

        /// <summary>
        /// Starts the KeepAliveThread.
        ///
        /// </summary>
        public TEnsureKeepAlive() : base()
        {
            Thread TheThread;

            UKeepRemotedObjectsAlive = true;
            UKeepAliveObjects = SortedList.Synchronized(new SortedList());

            // Start KeepAliveThread
            TheThread = new Thread(new ThreadStart(KeepAliveThread));
            TheThread.Start();
        }

        /// <summary>
        /// Stops the KeepAliveThread.
        ///
        /// @comment The only way to start the KeepAliveThread again is to create a new
        /// TEnsureKeepAlive object. However, since there should be only one such
        /// Thread throughout the Client's lifetime, this should not be necessary.
        ///
        /// </summary>
        /// <returns>void</returns>
        public static void StopKeepAlive()
        {
            // Through this KeepAliveThread will stop when it awakes next time
            UKeepRemotedObjectsAlive = false;
        }

        /// <summary>
        /// Registers a remoted Object with the KeepAlive mechanism of this Class.
        ///
        /// @comment Once a remoted Object is Registered, it will be not be GC'ed on the
        /// PetraServer until it is UnRegistered.
        ///
        /// </summary>
        /// <param name="ARemotedObject">The Remoted Object as an Interface
        /// </param>
        /// <returns>void</returns>
        public static void Register(MarshalByRefObject ARemotedObject)
        {
            try
            {
                try
                {
                    if (Monitor.TryEnter(UKeepAliveObjects.SyncRoot, 10000))
                    {
                        // Add remoted Object to the SortedList
                        UKeepAliveObjects.Add(ARemotedObject.GetHashCode().ToString(), ARemotedObject);

                        // TLogging.Log("TEnsureKeepAlive.Register: Added Object '" + ARemotedObject.ToString() + "' (HashCode: " + ARemotedObject.GetHashCode().ToString() + ")", TLoggingType.ToLogfile);

                        ARemotedObject.GetLifetimeService();
                    }
                }
                finally
                {
                    // $IFDEF DEBUGMODE TLogging.Log('TEnsureKeepAlive.Register: Finally Clause.', [ToLogFile]); $ENDIF
                    Monitor.PulseAll(UKeepAliveObjects.SyncRoot);
                    Monitor.Exit(UKeepAliveObjects.SyncRoot);
                }
            }
            catch (Exception Exp)
            {
                TLogging.Log("Exception in TEnsureKeepAlive.Register: " + Exp.ToString(), TLoggingType.ToLogfile);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ARemotedObject"></param>
        public static void Register(IInterface ARemotedObject)
        {
            Register((MarshalByRefObject)ARemotedObject);
        }

        /// <summary>
        /// Unregisters an remoted Object with the KeepAlive mechanism of this Class.
        ///
        /// @comment Once an Object is UnRegistered, it will be marked for GC by the
        /// LeaseManager in the PetraServer as soon as its CurrentLeaseTime gets to
        /// zero!
        ///
        /// </summary>
        /// <param name="ARemotedObject">The Remoted Object as an Interface
        /// </param>
        /// <returns>void</returns>
        public static void UnRegister(MarshalByRefObject ARemotedObject)
        {
            String ObjectName;
            String ObjectHashCode;

            ObjectName = "";
            ObjectHashCode = "";
#if DEBUGMODE
            if (ARemotedObject == null)
            {
                TLogging.Log("TEnsureKeepAlive.UnRegister: Object cannot be unregistered because it is nil!", TLoggingType.ToLogfile);
                throw new System.ArgumentException(
                    "ARemotedObject must not be nil. You must have Registered an Object that was nil, or the Object must has become nil since Registering it");
            }
#endif

            if (ARemotedObject != null)
            {
                // this is just to not create an Exception when not compiled with DEBUGMODE...
                try
                {
                    ObjectHashCode = ARemotedObject.GetHashCode().ToString();
                    ObjectName = ARemotedObject.ToString();
                }
                catch (System.Runtime.Remoting.RemotingException)
                {
                    // ignore this Exception: it is thrown if one tries to UnRegister a remoted object that was already UnRegistered and doesn't exist anymore on the PetraServer
#if DEBUGMODE
                    MessageBox.Show(
                        "Message from TEnsureKeepAlive.UnRegister Method:\r\nAn attempt was made to UnRegister an Object that was already UnRegistered\r\nand doesn't exist anymore on the PetraServer!",
                        "DEVELOPER DEBUGGING INFORMATION");
#endif
                }
                catch (Exception Exp)
                {
#if DEBUGMODE
                    TLogging.Log("TEnsureKeepAlive.UnRegister: Exception: " + Exp.ToString(), TLoggingType.ToLogfile);
#endif
                }

                if (ObjectHashCode != "")
                {
                    try
                    {
                        if (Monitor.TryEnter(UKeepAliveObjects.SyncRoot, 10000))
                        {
                            if (UKeepAliveObjects.Contains(ObjectHashCode))
                            {
                                // Remove remoted Object to the SortedList
                                UKeepAliveObjects.Remove(ObjectHashCode);

                                // $IFDEF DEBUGMODE TLogging.Log('TEnsureKeepAlive.UnRegister: Removed Object ''' + ObjectName + '''', [ToLogFile]); $ENDIF
                            }
                            else
                            {
                                if (ObjectName != "")
                                {
#if DEBUGMODE
                                    TLogging.Log(
                                        "TEnsureKeepAlive.UnRegister: Object '" + ObjectName +
                                        "' cannot be unregistered because it is not Registered!",
                                        TLoggingType.ToLogfile);
#endif
                                }
                                else
                                {
#if DEBUGMODE
                                    TLogging.Log("TEnsureKeepAlive.UnRegister: Object cannot be unregistered because it is not Registered!",
                                        TLoggingType.ToLogfile);
#endif
                                }
                            }
                        }
                    }
                    finally
                    {
                        // $IFDEF DEBUGMODE TLogging.Log('TEnsureKeepAlive.UnRegister: Finally Clause.', [ToLogFile]); $ENDIF
                        Monitor.PulseAll(UKeepAliveObjects.SyncRoot);
                        Monitor.Exit(UKeepAliveObjects.SyncRoot);
                    }
                }
            }
            else
            {
                // if the code is not compiled with DEBUGMODE, we don't care about UnRegistering an Object that was nil...
                TLogging.Log("TEnsureKeepAlive.UnRegister: Object cannot be unregistered because it is nil!", TLoggingType.ToLogfile);
            }
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ARemotedObject"></param>
        public static void UnRegister(IInterface ARemotedObject)
        {
            UnRegister((MarshalByRefObject)ARemotedObject);
        }

        /// <summary>
        /// Thread that calls in regular intervals into registered remoted server-side
        /// Objects.
        ///
        /// @comment The Thread is started at Class instantiation and can be stopped by
        /// calling the StopKeepAlive method.
        ///
        /// @comment The interval can be configured with the ClientSetting
        /// 'ServerObjectKeepAliveIntervalInSeconds'.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void KeepAliveThread()
        {
            IDictionaryEnumerator ObjectEnum;

            // Check whether this Thread should still execute
            while (UKeepRemotedObjectsAlive)
            {
                try
                {
                    // $IFDEF DEBUGMODE TLogging.Log('KeepAliveThread: Checking objects (' + UKeepAliveObjects.Count.ToString + ' Objects to keep alive in SortedList)', [ToLogFile]); $ENDIF
                    ObjectEnum = UKeepAliveObjects.GetEnumerator();
                    try
                    {
                        if (Monitor.TryEnter(UKeepAliveObjects.SyncRoot, 10000))
                        {
                            // Iterate over all Objects in the SortedList and keep them alive
                            while (ObjectEnum.MoveNext())
                            {
                                try
                                {
                                    // TLogging.Log("KeepAliveThread: Keeping Object " + ObjectEnum.Key.ToString() + " alive", TLoggingType.ToLogfile);

                                    /*
                                     * The following call is the key to the whole concept of keeping
                                     * the remoted server-side Objects alive:
                                     * Calling 'GetLifeTimeService' is sufficient to 'tickle' the
                                     * server-side Object and for its Lease to be renewed!
                                     */
                                    ((MarshalByRefObject)ObjectEnum.Value).GetLifetimeService();

                                    // TLogging.Log("KeepAliveThread: Kept Object " + ObjectEnum.Value.ToString() + " alive", TLoggingType.ToLogfile);
                                }
                                catch (Exception Exp)
                                {
#if DEBUGMODE
                                    TLogging.Log(
                                        "KeepAliveThread: " + ObjectEnum.Key.ToString() + " Could not contact PetraServer!\r\n" + Exp.ToString(),
                                        TLoggingType.ToLogfile);
#endif
                                }
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Exit(UKeepAliveObjects.SyncRoot);
                    }
                }
                catch (System.Runtime.Remoting.RemotingException Exp)
                {
                    // string DebugInfo = StrConnectionUnavailableCause + Exp.ToString();
                    // MessageBox.Show(StrConnectionBroken + DebugInfo, StrConnectionBrokenTitle,
                    // MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TLogging.Log("RemotingException in TEnsureKeepAlive.KeepAliveThread: " + Exp.ToString(), TLoggingType.ToLogfile);
                }
                catch (System.Net.Sockets.SocketException Exp)
                {
                    // string DebugInfo = StrConnectionUnavailableCause + Exp.ToString() +
                    //            "\r\n\r\nSocketException.ErrorCode: " + Exp.ErrorCode.ToString();
                    // MessageBox.Show(StrConnectionClosed + DebugInfo, StrConnectionClosedTitle,
                    // MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TLogging.Log("SocketException in TEnsureKeepAlive.KeepAliveThread: " + Exp.ToString(), TLoggingType.ToLogfile);
                }
                catch (Exception Exp)
                {
                    TLogging.Log("Exception in TEnsureKeepAlive.KeepAliveThread: " + Exp.ToString(), TLoggingType.ToLogfile);
                }

                // Sleep for some time. After that, this function is called again automatically.
                Thread.Sleep(TClientSettings.ServerObjectKeepAliveIntervalInSeconds * 1000);
            }

            // Thread stops here and doesn't get called again automatically.
        }

        #endregion
    }
}