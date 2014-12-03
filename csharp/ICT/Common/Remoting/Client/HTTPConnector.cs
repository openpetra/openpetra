//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.Threading;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;

namespace Ict.Common.Remoting.Client
{
    /// connect to the server and return a response
    public class THttpConnector
    {
        private static string ServerURL = string.Empty;

        private static string FServerAdminSecurityToken = string.Empty;

        /// <summary>
        /// security token for access to serveradmin webconnector methods
        /// </summary>
        public static string ServerAdminSecurityToken
        {
            get
            {
                return FServerAdminSecurityToken;
            }
            set
            {
                FServerAdminSecurityToken = value;
            }
        }


        /// <summary>
        /// initialise the name of the server
        /// </summary>
        /// <param name="AServerURL"></param>
        public static void InitConnection(string AServerURL)
        {
            ServerURL = AServerURL;
        }

        private static NameValueCollection ConvertParameters(SortedList <string, object>parameters)
        {
            NameValueCollection result = new NameValueCollection();

            if (parameters == null)
            {
                return result;
            }

            foreach (string param in parameters.Keys)
            {
                object o = parameters[param];
                result.Add(param, THttpBinarySerializer.SerializeObject(o));
            }

            return result;
        }

        private static string TrimResult(string result)
        {
            // returns <string xmlns="...">someresulttext</string>
            TLogging.LogAtLevel(1, "returned from server (unmodified): " + 
                                (result.Length > 600 ? result.Substring(0, 600) + " ...":result));

            string OrigResult = result;
            try
            {
                result = result.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", string.Empty).Substring(result.IndexOf("<"));
                result = result.Substring(result.IndexOf(">") + 1);

                if (result.Length == 0)
                {
                    return string.Empty;
                }

                result = result.Substring(0, result.IndexOf("<"));
            }
            catch (Exception e)
            {
                TLogging.Log("THttpConnector.TrimResult: problems processing result from server");
                TLogging.Log(OrigResult);
                TLogging.Log(e.ToString());
                throw new Exception("invalid response from server");
            }

            // TLogging.Log("returned from server: " + result);
            return result;
        }

        /// <summary>
        /// call a webconnector
        /// </summary>
        public static List <object>CallWebConnector(
            string AModuleName,
            string methodname,
            SortedList <string, object>parameters, string expectedReturnType)
        {
            NameValueCollection Parameters = ConvertParameters(parameters);

            string result;

            try
            {
                result = THTTPUtils.PostRequest(ServerURL + "/server" + AModuleName + ".asmx/" + methodname.Replace(".", "_"), Parameters);
            }
            catch (Exception e)
            {
                if (e.Message == THTTPUtils.SESSION_ALREADY_CLOSED)
                {
                    TLogging.Log("session has already been closed!");
                }

                throw;
            }

            if (expectedReturnType == "void")
            {
                // did we get a positive response at all? yes, otherwise we would have gotten an exception
                return null;
            }

            if ((result == null) || (result.Length == 0))
            {
                throw new Exception("invalid response from the server");
            }

            result = TrimResult(result);

            List <object>resultObjects = new List <object>();

            if (expectedReturnType == "list")
            {
                string[] resultlist = result.Split(new char[] { ',' });

                foreach (string o in resultlist)
                {
                    string[] typeAndVal = o.Split(new char[] { ':' });
                    resultObjects.Add(THttpBinarySerializer.DeserializeObject(typeAndVal[0], typeAndVal[1]));
                }
            }
            else
            {
                resultObjects.Add(THttpBinarySerializer.DeserializeObject(result, expectedReturnType));
            }

            return resultObjects;
        }

        /// <summary>
        /// call a method of a UIConnector
        /// </summary>
        public static List <object>CallUIConnectorMethod(
            string ObjectID,
            string AModuleName,
            string UIConnectorClass,
            string methodname,
            SortedList <string, object>parameters, string expectedReturnType)
        {
            parameters.Add("UIConnectorObjectID", ObjectID.ToString());

            return CallWebConnector(AModuleName, UIConnectorClass + "." + methodname, parameters, expectedReturnType);
        }

        /// <summary>
        /// read a property of a UIConnector
        /// </summary>
        public static object ReadUIConnectorProperty(
            string ObjectID,
            string AModuleName,
            string UIConnectorClass,
            string propertyname,
            string expectedReturnType)
        {
            SortedList <string, object>parameters = new SortedList <string, object>();
            parameters.Add("UIConnectorObjectID", ObjectID);

            return CallWebConnector(AModuleName, UIConnectorClass + ".Get" + propertyname, parameters, expectedReturnType)[0];
        }

        /// <summary>
        /// write to a property of a UIConnector
        /// </summary>
        public static void WriteUIConnectorProperty(
            string ObjectID,
            string AModuleName,
            string UIConnectorClass,
            string propertyname,
            object value)
        {
            SortedList <string, object>parameters = new SortedList <string, object>();
            parameters.Add("UIConnectorObjectID", ObjectID);
            parameters.Add("value", value);

            CallWebConnector(AModuleName, UIConnectorClass + ".Set" + propertyname, parameters, "void");
        }

        /// <summary>
        /// create a UIConnector on the server
        /// </summary>
        public static string CreateUIConnector(
            string AModuleName,
            string classname,
            SortedList <string, object>parameters)
        {
            NameValueCollection Parameters =
                ConvertParameters(parameters);

            string result = THTTPUtils.PostRequest(ServerURL + "/server" + AModuleName + ".asmx/Create_" + classname, Parameters);

            result = TrimResult(result);

            TLogging.LogAtLevel(4, String.Format("CreateUIConnector called for Module '{0}' and Class '{1}'",
                AModuleName, classname));

            return THttpBinarySerializer.DeserializeObject(result, "System.String").ToString();
        }

        /// <summary>
        /// disconnect an object from the server, freeing up memory on the server
        /// </summary>
        public static void DisconnectUIConnector(string AModuleName, string ObjectID)
        {
            SortedList <string, object>Parameters = new SortedList <string, object>();
            Parameters.Add("UIConnectorObjectID", ObjectID.ToString());

            TLogging.LogAtLevel(4, String.Format("DisconnectUIConnector called for Module '{0}' with ObjectID '{1}'",
                AModuleName, ObjectID));
            
            CallWebConnector(AModuleName, "DisconnectUIConnector", Parameters, "void");
        }
    }
}

