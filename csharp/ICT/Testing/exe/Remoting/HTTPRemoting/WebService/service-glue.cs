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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Runtime.Remoting;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.ServiceModel.Web;
using System.ServiceModel;
using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MCommon.WebConnectors;
using Tests.HTTPRemoting.Interface;

namespace Tests.HTTPRemoting.Service
{
    /// <summary>
    /// the test service
    /// </summary>
    [WebService(Namespace = "http://www.openpetra.org/webservices/Sample")]
    [ScriptService]
    public class TMyService : WebService
    {
        private static SortedList <string, object>FUIConnectors = new SortedList <string, object>();

        /// <summary>
        /// constructor, which is called for each http request
        /// </summary>
        public TMyService() : base()
        {
            TOpenPetraOrgSessionManagerTest.Init();
        }
        
        /// disconnect an UIConnector object
        [WebMethod(EnableSession = true)]
        public void DisconnectUIConnector(string UIConnectorObjectID)
        {
            string ObjectID = String.Empty;
            
            try 
            {
                ObjectID = UIConnectorObjectID + " " + DomainManager.GClientID;    
            }
            catch (EOPDBInvalidSessionException)
            {
                // Don't do anything in this scenario as in this case the request to disconnect an UIConnector 
                // has likely come after the Client has disconnected: UIConnector Objects' Finalizers in the 
                // 'Client Glue' call the UIConnector Objects' Dispose Methods, which call the present Method. 
                // Those UIConnector Object Finalizers are not only executed when a screen gets closed by the user  
                // (where that screen uses UIConnectors) and the Garbage Collector executes the Finalizer (whenever 
                // the GC gets to that!), but also when a Client gets closed while screens that use UIConnectors 
                // were still open when the Client gets closed...
                TLogging.Log("DisconnectUIConnector for Sample for UIConnectorObjectID '" + UIConnectorObjectID + "' got called, but there is no Client Session anymore for that client...");
                
                return;
            }
            catch (Exception Exc) 
            {           
                TLogging.Log("DisconnectUIConnector for Sample for UIConnectorObjectID '" + UIConnectorObjectID + "': encountered Exception:\r\n" + Exc.ToString());
                throw;
            }         

            TLogging.Log("DisconnectUIConnector for Sample: ObjectID is '" + ObjectID + "' for UIConnectorObjectID '" + ObjectID +"'!");
            
            if (FUIConnectors.ContainsKey(ObjectID))
            {
                // FUIConnectors[ObjectID].Dispose();
                FUIConnectors.Remove(ObjectID);
                TLogging.Log("DisconnectUIConnector for Sample: removed UIConnectorObjectID '" + ObjectID +"'!");               
            }
        }
        
        /// <summary>
        /// print hello world
        /// </summary>
        /// <param name="msg"></param>
        [WebMethod(EnableSession = true)]
        public string HelloWorld(string msg)
        {
            return TMyServiceWebConnector.HelloWorld(msg);
        }

        /// <summary>
        /// sample webconnector method that takes a long time and uses the ProgressTracker
        /// </summary>
        [WebMethod(EnableSession = true)]
        public string LongRunningJob()
        {
            return TMyServiceWebConnector.LongRunningJob();
        }

        /// <summary>
        /// some tests for remoting DateTime objects
        /// </summary>
        [WebMethod(EnableSession = true)]
        public string TestDateTime(string date)
        {
            DateTime outDateTomorrow;

            DateTime result = TMyServiceWebConnector.TestDateTime((DateTime)THttpBinarySerializer.DeserializeObject(date,
                    "binary"), out outDateTomorrow);

            return THttpBinarySerializer.SerializeObjectWithType(outDateTomorrow) + "," + THttpBinarySerializer.SerializeObjectWithType(result);
        }

        /// create a new UIConnector
        [WebMethod(EnableSession = true)]
        public System.String Create_TMyUIConnector()
        {
            // CHECKUSERMODULEPERMISSIONS

            System.Guid ObjectID = Guid.NewGuid();
            FUIConnectors.Add(ObjectID.ToString() + " " + DomainManager.GClientID, new TMyUIConnector());

            return ObjectID.ToString();
        }

        /// access a UIConnector method
        [WebMethod(EnableSession = true)]
        public string TMyUIConnector_HelloWorldUIConnector(string UIConnectorObjectID)
        {
            string ObjectID = UIConnectorObjectID + " " + DomainManager.GClientID;

            if (!FUIConnectors.ContainsKey(ObjectID))
            {
                TLogging.Log(
                    "Trying to call TMyUIConnector_HelloWorldUIConnector, but the object with this ObjectID " + ObjectID + " does not exist");
                throw new Exception("this object does not exist anymore!");
            }

            try
            {
                return THttpBinarySerializer.SerializeObjectWithType(((TMyUIConnector)FUIConnectors[ObjectID]).HelloWorldUIConnector());
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                throw new Exception("Please check server log file");
            }
        }

        /// web connector method call
        [WebMethod(EnableSession = true)]
        public string TMySubNamespace_HelloSubWorld(System.String msg)
        {
            // TModuleAccessManager.CheckUserPermissionsForMethod();
            try
            {
                TLogging.Log(msg);
                return THttpBinarySerializer.SerializeObjectWithType(TMyServiceWebConnector.HelloSubWorld());
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                throw new Exception("Please check server log file");
            }
        }
    }

    /// <summary>
    /// server manager for testing purposes
    /// </summary>
    public class TOpenPetraOrgSessionManagerTest
    {
        private static string TheServerManager = null;

        /// <summary>
        /// initialise the server once for everyone
        /// </summary>
        public static bool Init()
        {
            if (TheServerManager == null)
            {
                // make sure the correct config file is used
                new TAppSettingsManager(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "web.config");
                new TSrvSetting();
                new TLogging(TSrvSetting.ServerLogFile);
                TLogging.DebugLevel = TAppSettingsManager.GetInt16("Server.DebugLevel", 0);

                Catalog.Init();

                TheServerManager = "test";

                TLogging.Log("Server has been initialised");

                return true;
            }

            return false;
        }
    }
}