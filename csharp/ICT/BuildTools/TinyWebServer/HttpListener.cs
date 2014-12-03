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
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Threading;
using System.Diagnostics;

namespace Ict.Tools.TinyWebServer
{
/// <summary>
/// this class is a wrapper for HttpListener.
/// it is able to deal with multiple requests, starting a thread for each request
/// </summary>
    public class ThreadedHttpListenerWrapper : MarshalByRefObject
    {
        private HttpListener FListener;
        private string FVirtualDir;
        private string FPhysicalDir;

        /// <summary>
        /// configure the listener
        /// </summary>
        public void Configure(string[] APrefixes, string AVirtualDir, string APhysicalDir)
        {
            FVirtualDir = AVirtualDir;
            FPhysicalDir = APhysicalDir;
            FListener = new HttpListener();

            foreach (string prefix in APrefixes)
            {
                FListener.Prefixes.Add(prefix);
            }
        }

        /// <summary>
        /// start the listener
        /// </summary>
        public void Start()
        {
            FListener.Start();
        }

        /// <summary>
        /// stop the listener
        /// </summary>
        public void Stop()
        {
            FListener.Stop();
        }

        /// <summary>
        /// to be called by a new thread per request
        /// </summary>
        private void ProcessRequest(TMyHttpWorkerRequest r)
        {
            HttpRuntime.ProcessRequest(r);
        }

        /// <summary>
        /// ProcessRequest
        /// </summary>
        public void ProcessRequest()
        {
            HttpListenerContext ctx = null;

            try
            {
                ctx = FListener.GetContext();
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("Server stopped probably because a file in the application directory was changed");
                return;
            }
            TMyHttpWorkerRequest workerRequest =
                new TMyHttpWorkerRequest(ctx, FVirtualDir, FPhysicalDir);

            (new Thread(() => ProcessRequest(workerRequest))).Start();
        }
    }

/// <summary>
/// extend HttpWorkerRequest so that we can handle asmx requests
/// </summary>
    public class TMyHttpWorkerRequest : HttpWorkerRequest
    {
        private HttpListenerContext FContext;
        private string FVirtualDir;
        private string FPhysicalDir;

        /// <summary>
        /// constructor
        /// </summary>
        public TMyHttpWorkerRequest(
            HttpListenerContext context, string vdir, string pdir)
        {
            if (null == context)
            {
                throw new ArgumentNullException("context");
            }

            if ((null == vdir) || vdir.Equals(""))
            {
                throw new ArgumentException("vdir");
            }

            if ((null == pdir) || pdir.Equals(""))
            {
                throw new ArgumentException("pdir");
            }

            FContext = context;
            FVirtualDir = vdir;
            FPhysicalDir = pdir;
        }

        /// <summary>
        /// EndOfRequest
        /// </summary>
        public override void EndOfRequest()
        {
            FContext.Response.OutputStream.Close();
            FContext.Response.Close();
        }

        /// <summary>
        /// FlushResponse
        /// </summary>
        /// <param name="finalFlush"></param>
        public override void FlushResponse(bool finalFlush)
        {
            FContext.Response.OutputStream.Flush();
        }

        /// <summary>
        /// GetHttpVerbName
        /// </summary>
        /// <returns></returns>
        public override string GetHttpVerbName()
        {
            return FContext.Request.HttpMethod;
        }

        /// <summary>
        /// GetHttpVersion
        /// </summary>
        /// <returns></returns>
        public override string GetHttpVersion()
        {
            return string.Format("HTTP/{0}.{1}",
                FContext.Request.ProtocolVersion.Major,
                FContext.Request.ProtocolVersion.Minor);
        }

        /// <summary>
        /// GetLocalAddress
        /// </summary>
        /// <returns></returns>
        public override string GetLocalAddress()
        {
            return FContext.Request.LocalEndPoint.Address.ToString();
        }

        /// <summary>
        /// GetLocalPort
        /// </summary>
        /// <returns></returns>
        public override int GetLocalPort()
        {
            return FContext.Request.LocalEndPoint.Port;
        }

        /// <summary>
        /// GetQueryString
        /// </summary>
        /// <returns></returns>
        public override string GetQueryString()
        {
            string queryString = "";
            string rawUrl = FContext.Request.RawUrl;
            int index = rawUrl.IndexOf('?');

            if (index != -1)
            {
                queryString = rawUrl.Substring(index + 1);
            }

            return queryString;
        }

        /// <summary>
        /// GetRawUrl
        /// </summary>
        /// <returns></returns>
        public override string GetRawUrl()
        {
            return FContext.Request.RawUrl;
        }

        /// <summary>
        /// GetRemoteAddress
        /// </summary>
        /// <returns></returns>
        public override string GetRemoteAddress()
        {
            return FContext.Request.RemoteEndPoint.Address.ToString();
        }

        /// <summary>
        /// GetRemotePort
        /// </summary>
        /// <returns></returns>
        public override int GetRemotePort()
        {
            return FContext.Request.RemoteEndPoint.Port;
        }

        /// <summary>
        /// GetUriPath
        /// </summary>
        /// <returns></returns>
        public override string GetUriPath()
        {
            return FContext.Request.Url.LocalPath;
        }

        /// <summary>
        /// SendKnownResponseHeader
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public override void SendKnownResponseHeader(int index, string value)
        {
            FContext.Response.Headers[
                HttpWorkerRequest.GetKnownResponseHeaderName(index)] = value;
        }

        /// <summary>
        /// SendResponseFromMemory
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public override void SendResponseFromMemory(byte[] data, int length)
        {
            FContext.Response.OutputStream.Write(data, 0, length);
        }

        /// <summary>
        /// SendStatus
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="statusDescription"></param>
        public override void SendStatus(int statusCode, string statusDescription)
        {
            FContext.Response.StatusCode = statusCode;
            FContext.Response.StatusDescription = statusDescription;
        }

        /// <summary>
        /// SendUnknownResponseHeader
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public override void SendUnknownResponseHeader(string name, string value)
        {
            FContext.Response.Headers[name] = value;
        }

        /// <summary>
        /// SendResponseFromFile
        /// </summary>
        public override void SendResponseFromFile(
            IntPtr handle, long offset, long length)
        {
            // not implemented
        }

        /// <summary>
        /// SendResponseFromFile
        /// </summary>
        public override void SendResponseFromFile(string filename, long offset, long length)
        {
            // not implemented
        }

        /// <summary>
        /// CloseConnection
        /// </summary>
        public override void CloseConnection()
        {
            // not implemented
        }

        /// <summary>
        /// GetAppPath
        /// </summary>
        /// <returns></returns>
        public override string GetAppPath()
        {
            return FVirtualDir;
        }

        /// <summary>
        /// GetAppPathTranslated
        /// </summary>
        /// <returns></returns>
        public override string GetAppPathTranslated()
        {
            return FPhysicalDir;
        }

        /// <summary>
        /// ReadEntityBody
        /// </summary>
        public override int ReadEntityBody(byte[] buffer, int size)
        {
            return FContext.Request.InputStream.Read(buffer, 0, size);
        }

        /// <summary>
        /// GetUnknownRequestHeader
        /// </summary>
        public override string GetUnknownRequestHeader(string name)
        {
            return FContext.Request.Headers[name];
        }

        /// <summary>
        /// GetUnknownRequestHeaders
        /// </summary>
        public override string[][] GetUnknownRequestHeaders()
        {
            string[][] unknownRequestHeaders;
            System.Collections.Specialized.NameValueCollection headers = FContext.Request.Headers;
            int count = headers.Count;
            List <string[]>headerPairs = new List <string[]>(count);

            for (int i = 0; i < count; i++)
            {
                string headerName = headers.GetKey(i);

                if (GetKnownRequestHeaderIndex(headerName) == -1)
                {
                    string headerValue = headers.Get(i);
                    headerPairs.Add(new string[] { headerName, headerValue });
                }
            }

            unknownRequestHeaders = headerPairs.ToArray();
            return unknownRequestHeaders;
        }

        /// <summary>
        /// GetKnownRequestHeader
        /// </summary>
        public override string GetKnownRequestHeader(int index)
        {
            switch (index)
            {
                case HeaderUserAgent:
                    return FContext.Request.UserAgent;

                default:
                    return FContext.Request.Headers[GetKnownRequestHeaderName(index)];
            }
        }

        /// <summary>
        /// GetServerVariable
        /// </summary>
        public override string GetServerVariable(string name)
        {
            switch (name)
            {
                case "HTTPS":
                    return FContext.Request.IsSecureConnection ? "on" : "off";

                case "HTTP_USER_AGENT":
                    return FContext.Request.Headers["UserAgent"];

                default:
                    return null;
            }
        }

        /// <summary>
        /// GetFilePath
        /// </summary>
        public override string GetFilePath()
        {
            string s = FContext.Request.Url.LocalPath;

            if (s.IndexOf(".aspx") != -1)
            {
                s = s.Substring(0, s.IndexOf(".aspx") + 5);
            }
            else if (s.IndexOf(".asmx") != -1)
            {
                s = s.Substring(0, s.IndexOf(".asmx") + 5);
            }

            return s;
        }

        /// <summary>
        /// GetFilePathTranslated
        /// </summary>
        public override string GetFilePathTranslated()
        {
            string s = GetFilePath();

            s = s.Substring(FVirtualDir.Length);
            s = s.Replace('/', '\\');
            return FPhysicalDir + s;
        }

        /// <summary>
        /// GetPathInfo
        /// </summary>
        public override string GetPathInfo()
        {
            string s1 = GetFilePath();
            string s2 = FContext.Request.Url.LocalPath;

            if (s1.Length == s2.Length)
            {
                return "";
            }
            else
            {
                return s2.Substring(s1.Length);
            }
        }
    }
}