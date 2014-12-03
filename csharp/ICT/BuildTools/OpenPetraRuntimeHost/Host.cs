/* **********************************************************************************
*
* Copyright (c) Microsoft Corporation. All rights reserved.
*
* This source code is subject to terms and conditions of the Microsoft Public
* License (Ms-PL). A copy of the license can be found in the license.htm file
* included in this distribution.
*
* You must not remove this notice, or any other, from this software.
*
* **********************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Security.Permissions;
using System.Security.Principal;

namespace Ict.Tools.OpenPetraRuntimeHost
{
    /// <summary>
    /// Host class
    /// </summary>
    public class Host : MarshalByRefObject, IRegisteredObject
    {
        Server _server;

        int _port;
        volatile int _pendingCallsCount;
        string _virtualPath;
        string _lowerCasedVirtualPath;
        string _lowerCasedVirtualPathWithTrailingSlash;
        string _physicalPath;
        string _defaultPage;
        bool _allowRemoteConnection;
        string _installPath;
        string _physicalClientScriptPath;
        string _lowerCasedClientScriptPathWithTrailingSlash;

        string _logfilePath = String.Empty;
        StreamWriter _logfileWriter = null;

        /// <summary>
        /// Returns true if remote connection is allowed
        /// </summary>
        public bool AllowRemoteConnection {
            get
            {
                return _allowRemoteConnection;
            }
        }

        /// <summary>
        /// Returns the default page, if any
        /// </summary>
        public string DefaultPage {
            get
            {
                return _defaultPage;
            }
        }

        /// <summary>
        /// Public override
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            // never expire the license
            return null;
        }

        /// <summary>
        /// Main constructor for host
        /// </summary>
        public Host()
        {
            HostingEnvironment.RegisterObject(this);
        }

        /// <summary>
        /// The streamwriter object used for logging page requests
        /// </summary>
        public StreamWriter LogfileWriter
        {
            get
            {
                return _logfileWriter;
            }
        }

        /// <summary>
        /// Main method for configuring the host
        /// </summary>
        public void Configure(Server server,
            int port,
            string virtualPath,
            string physicalPath,
            string defaultPage,
            bool allowRemoteConnection,
            bool logPageRequests)
        {
            _server = server;

            _port = port;
            _installPath = null;
            _virtualPath = virtualPath;
            _defaultPage = defaultPage;
            _allowRemoteConnection = allowRemoteConnection;

            _lowerCasedVirtualPath = CultureInfo.InvariantCulture.TextInfo.ToLower(_virtualPath);
            _lowerCasedVirtualPathWithTrailingSlash = virtualPath.EndsWith("/", StringComparison.Ordinal) ? virtualPath : virtualPath + "/";
            _lowerCasedVirtualPathWithTrailingSlash = CultureInfo.InvariantCulture.TextInfo.ToLower(_lowerCasedVirtualPathWithTrailingSlash);
            _physicalPath = physicalPath;
            _physicalClientScriptPath = HttpRuntime.AspClientScriptPhysicalPath + "\\";
            _lowerCasedClientScriptPathWithTrailingSlash = CultureInfo.InvariantCulture.TextInfo.ToLower(HttpRuntime.AspClientScriptVirtualPath + "/");

            if (logPageRequests)
            {
                // Make a log folder beneath the web site's root folder and store our request log there
                _logfilePath = Path.Combine(HttpRuntime.AppDomainAppPath, "httpLog", "HttpRequest.log");

                string logFileFolder = Path.GetDirectoryName(_logfilePath);

                if (!Directory.Exists(logFileFolder))
                {
                    Directory.CreateDirectory(logFileFolder);
                }

                _logfileWriter = new StreamWriter(_logfilePath);
            }
        }

        /// <summary>
        /// Main method to process a request
        /// </summary>
        public void ProcessRequest(Connection conn)
        {
            // Add a pending call to make sure our thread doesn't get killed
            AddPendingCall();

            try
            {
                Request request = new Request(_server, this, conn);
                request.Process();
            }
            finally
            {
                RemovePendingCall();
            }
        }

        void WaitForPendingCallsToFinish()
        {
            for (;; )
            {
                if (_pendingCallsCount <= 0)
                {
                    break;
                }

                Thread.Sleep(250);
            }
        }

        void AddPendingCall()
        {
#pragma warning disable 0420
            Interlocked.Increment(ref _pendingCallsCount);
#pragma warning restore 0420
        }

        void RemovePendingCall()
        {
#pragma warning disable 0420
            Interlocked.Decrement(ref _pendingCallsCount);
#pragma warning restore 0420
        }

        /// <summary>
        /// Shuts down the hosting environment
        /// </summary>
        public void Shutdown()
        {
            HostingEnvironment.InitiateShutdown();
        }

        void IRegisteredObject.Stop(bool immediate)
        {
            // Unhook the Host so Server will process the requests in the new appdomain.
            if (_server != null)
            {
                _server.HostStopped();
            }

            // Make sure all the pending calls complete before this Object is unregistered.
            WaitForPendingCallsToFinish();

            if (_logfileWriter != null)
            {
                _logfileWriter.Close();
                _logfileWriter.Dispose();
            }

            HostingEnvironment.UnregisterObject(this);
        }

        /// <summary>
        /// Returns the install path of the application
        /// </summary>
        public string InstallPath {
            get
            {
                return _installPath;
            }
        }

        /// <summary>
        /// Returns the lower case script path with trailing slash
        /// </summary>
        public string NormalizedClientScriptPath {
            get
            {
                return _lowerCasedClientScriptPathWithTrailingSlash;
            }
        }

        /// <summary>
        /// Returns the lower case virtual path with trailing slash
        /// </summary>
        public string NormalizedVirtualPath {
            get
            {
                return _lowerCasedVirtualPathWithTrailingSlash;
            }
        }

        /// <summary>
        /// returns the physical client script path
        /// </summary>
        public string PhysicalClientScriptPath {
            get
            {
                return _physicalClientScriptPath;
            }
        }

        /// <summary>
        /// returns the physical path
        /// </summary>
        public string PhysicalPath {
            get
            {
                return _physicalPath;
            }
        }

        /// <summary>
        /// Returns the port number
        /// </summary>
        public int Port {
            get
            {
                return _port;
            }
        }

        /// <summary>
        /// Returns the virtual path
        /// </summary>
        public string VirtualPath {
            get
            {
                return _virtualPath;
            }
        }

        /// <summary>
        /// True if the virtual path is in the app path
        /// </summary>
        public bool IsVirtualPathInApp(String path)
        {
            bool isClientScriptPath;

            return IsVirtualPathInApp(path, out isClientScriptPath);
        }

        /// <summary>
        /// Returns true if path is in app and also returns true/false if path is client script path
        /// </summary>
        public bool IsVirtualPathInApp(string path, out bool isClientScriptPath)
        {
            isClientScriptPath = false;

            if (path == null)
            {
                return false;
            }

            if ((_virtualPath == "/") && path.StartsWith("/", StringComparison.Ordinal))
            {
                if (path.StartsWith(_lowerCasedClientScriptPathWithTrailingSlash, StringComparison.Ordinal))
                {
                    isClientScriptPath = true;
                }

                return true;
            }

            path = CultureInfo.InvariantCulture.TextInfo.ToLower(path);

            if (path.StartsWith(_lowerCasedVirtualPathWithTrailingSlash, StringComparison.Ordinal))
            {
                return true;
            }

            if (path == _lowerCasedVirtualPath)
            {
                return true;
            }

            if (path.StartsWith(_lowerCasedClientScriptPathWithTrailingSlash, StringComparison.Ordinal))
            {
                isClientScriptPath = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the path is the app path
        /// </summary>
        public bool IsVirtualPathAppPath(string path)
        {
            if (path == null)
            {
                return false;
            }

            path = CultureInfo.InvariantCulture.TextInfo.ToLower(path);
            return path == _lowerCasedVirtualPath || path == _lowerCasedVirtualPathWithTrailingSlash;
        }
    }
}