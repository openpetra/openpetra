//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.MFinance.AP.UIConnectors;
using Ict.Petra.Server.MConference.Applications;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.AP.Data;
using Jayrock.Json;
using Ict.Petra.Server.MPartner.Import;
using Ict.Petra.Shared;

namespace PetraWebService
{
/// <summary>
/// this publishes the SOAP web services of OpenPetra.org
/// sqlite: will not work at the moment, because running with xsp2 from mono: requires CRT; otherwise you get the error:
///             Microsoft Visual C++ Runtime Library
///             Runtime Error!
///             Program: C:\Programme\Mono-2.4\bin\mono.exe
///             R6030
///             - CRT not initialized
/// Solution: run xsp with ms.net, or use sqlite with managed version only and native sqlite
///
/// TODO: generate soap functions with nant generateGlue from interfaces/instantiators?
/// </summary>
[WebService(Namespace = "http://www.openpetra.org/webservices/")]
[ScriptService]
public class TOpenPetraOrg : WebService
{
    /// <summary>
    /// static: only initialised once for the whole server
    /// </summary>
    static TServerManager TheServerManager = null;

    /// <summary>
    /// constructor, which is called for each http request
    /// </summary>
    public TOpenPetraOrg() : base()
    {
        // make sure the correct config file is used
        new TAppSettingsManager(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "web.config");
        new TSrvSetting();
        new TLogging(TSrvSetting.ServerLogFile);
        TLogging.DebugLevel = TAppSettingsManager.GetInt16("Server.DebugLevel", 0);

        DBAccess.SetFunctionForRetrievingCurrentObjectFromWebSession(SetDatabaseForSession,
            GetDatabaseFromSession);

        UserInfo.SetFunctionForRetrievingCurrentObjectFromWebSession(SetUserInfoForSession,
            GetUserInfoFromSession);
    }

    /// <summary>Initialise the server; this can only be called once, after that it will have no effect;
    /// it will be called automatically by Login</summary>
    [WebMethod(EnableSession = true)]
    public bool InitServer()
    {
        if (TheServerManager == null)
        {
            Catalog.Init();

            TheServerManager = new TServerManager();
            try
            {
                TheServerManager.EstablishDBConnection();

                TSystemDefaultsCache.GSystemDefaultsCache = new TSystemDefaultsCache();
                DomainManager.GSiteKey = TSystemDefaultsCache.GSystemDefaultsCache.GetInt64Default(
                    Ict.Petra.Shared.SharedConstants.SYSDEFAULT_SITEKEY);
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                throw;
            }
        }

        return true;
    }

    private bool LoginInternal(string username, string password)
    {
        Int32 ProcessID;
        bool ASystemEnabled;

        try
        {
            InitServer();

            TClientManager.PerformLoginChecks(
                username.ToUpper(), password.Trim(), "WEB", "127.0.0.1", out ProcessID, out ASystemEnabled);
            Session["LoggedIn"] = true;

            DBAccess.GDBAccessObj.UserID = username.ToUpper();

            TheServerManager.AddDBConnection(DBAccess.GDBAccessObj);

            return true;
        }
        catch (Exception e)
        {
            TLogging.Log(e.Message);
            TLogging.Log(e.StackTrace);
            Session["LoggedIn"] = false;
            Ict.Common.DB.DBAccess.GDBAccessObj.RollbackTransaction();
            DBAccess.GDBAccessObj.CloseDBConnection();
            return false;
        }
    }

    /// <summary>Login a user</summary>
    [WebMethod(EnableSession = true)]
    public bool Login(string username, string password)
    {
        bool loggedIn = LoginInternal(username, password);

        return loggedIn;
    }

    private TDataBase GetDatabaseFromSession()
    {
        if (HttpContext.Current.Session["DBAccessObj"] == null)
        {
            if (TheServerManager == null)
            {
                TLogging.Log("GetDatabaseFromSession : TheServerManager is null");
                InitServer();
            }
            else
            {
                // disconnect web user after 2 minutes of inactivity. should disconnect itself already earlier
                TheServerManager.DisconnectTimedoutDatabaseConnections(2 * 60, "ANONYMOUS");

                // disconnect normal users after 3 hours of inactivity
                TheServerManager.DisconnectTimedoutDatabaseConnections(3 * 60 * 60, "");

                TheServerManager.EstablishDBConnection();
            }
        }

        return (TDataBase)HttpContext.Current.Session["DBAccessObj"];
    }

    private void SetDatabaseForSession(TDataBase database)
    {
        HttpContext.Current.Session["DBAccessObj"] = database;
    }

    private TPetraPrincipal GetUserInfoFromSession()
    {
        return (TPetraPrincipal)HttpContext.Current.Session["UserInfo"];
    }

    private void SetUserInfoForSession(TPetraPrincipal userinfo)
    {
        HttpContext.Current.Session["UserInfo"] = userinfo;
    }

    /// <summary>check if the user has logged in successfully</summary>
    [WebMethod(EnableSession = true)]
    public bool IsUserLoggedIn()
    {
        object loggedIn = Session["LoggedIn"];

        if ((null != loggedIn) && ((bool)loggedIn == true))
        {
            return true;
        }

        return false;
    }

    /// <summary>log the user out</summary>
    [WebMethod(EnableSession = true)]
    public bool Logout()
    {
        TLogging.Log("Logout from a session", TLoggingType.ToLogfile | TLoggingType.ToConsole);

        if (DBAccess.GDBAccessObj != null)
        {
            DBAccess.GDBAccessObj.CloseDBConnection();
        }

        // Session Abandon causes problems in Mono 2.10.x see https://bugzilla.novell.com/show_bug.cgi?id=669807
        // TODO Session.Abandon();
        Session.Clear();

        return true;
    }

    /// <summary>
    /// check if there is already a supplier record for the given partner
    /// </summary>
    /// <param name="APartnerKey"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public bool CanFindSupplier(Int64 APartnerKey)
    {
        // TODO check permissions
        if (IsUserLoggedIn())
        {
            TSupplierEditUIConnector uiconnector = new TSupplierEditUIConnector();
            return uiconnector.CanFindSupplier(APartnerKey);
        }

        return false;
    }

    /// <summary>
    /// Passes data as a Typed DataSet to the Supplier Edit Screen
    /// </summary>
    [WebMethod(EnableSession = true)]
    public AccountsPayableTDS GetSupplierData(Int64 APartnerKey)
    {
        // TODO check permissions
        if (IsUserLoggedIn())
        {
            TSupplierEditUIConnector uiconnector = new TSupplierEditUIConnector();
            return uiconnector.GetData(APartnerKey);
        }

        return new AccountsPayableTDS();
    }

    /// <summary>
    /// Passes data as a Typed DataSet (Converted to a JSON string) to the Supplier Edit Screen
    /// </summary>
    [WebMethod(EnableSession = true)]
    public string GetSupplierDataJSON(Int64 APartnerKey)
    {
        // TODO check permissions
        if (IsUserLoggedIn())
        {
            TSupplierEditUIConnector uiconnector = new TSupplierEditUIConnector();
            AccountsPayableTDS dataset = uiconnector.GetData(APartnerKey);
            string myJson = Jayrock.Json.Conversion.JsonConvert.ExportToString(dataset);
            return myJson;
        }

        return "";
    }

    /// <summary>
    /// experiment to check how SubmitChanges could be done via web interface
    /// </summary>
    /// <param name="DatasetInJSON"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string SubmitChangesToSupplierJSON(string DatasetInJSON)
    {
        // TODO check permissions
        if (IsUserLoggedIn())
        {
            // pass the dataset as a JSON string, then deserialize the dataset
            AccountsPayableTDS AInspectDS = (AccountsPayableTDS)Jayrock.Json.Conversion.JsonConvert.Import(typeof(AccountsPayableTDS), DatasetInJSON);
            TSupplierEditUIConnector uiconnector = new TSupplierEditUIConnector();

            TSubmitChangesResult changesResult = uiconnector.SubmitChanges(ref AInspectDS);
            return new TCombinedSubmitChangesResult(changesResult, AInspectDS, new TVerificationResultCollection()).ToJSON();
        }

        return "not enough permissions";
    }

    /// <summary>
    /// combine all results into one struct; it seems out and ref is not supported by web services?
    /// </summary>
    public struct TCombinedSubmitChangesResult
    {
        private TSubmitChangesResult SubmitChangesResult;
        private DataSet UntypedDataSet;
        private TVerificationResultCollection VerificationResultCollection;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ASubmitChangesResult"></param>
        /// <param name="AUntypedDataSet"></param>
        /// <param name="AVerificationResultCollection"></param>
        public TCombinedSubmitChangesResult(TSubmitChangesResult ASubmitChangesResult,
            DataSet AUntypedDataSet,
            TVerificationResultCollection AVerificationResultCollection)
        {
            SubmitChangesResult = ASubmitChangesResult;
            UntypedDataSet = AUntypedDataSet;
            VerificationResultCollection = AVerificationResultCollection;
        }

        /// the only purpose of this function is to avoid the compiler warning on Mono:
        /// The private field `xyz' is assigned but its value is never used
        private void DummyFunction(out TSubmitChangesResult ASubmitChangesResult,
            out DataSet AUntypedDataSet,
            out TVerificationResultCollection AVerificationResultCollection)
        {
            ASubmitChangesResult = SubmitChangesResult;
            AUntypedDataSet = UntypedDataSet;
            AVerificationResultCollection = VerificationResultCollection;
        }

        /// <summary>
        /// encode the value in JSON
        /// </summary>
        public string ToJSON()
        {
            return Jayrock.Json.Conversion.JsonConvert.ExportToString(this);
        }
    }

    /// <summary>
    /// experiment to check how SubmitChanges could be done via web interface
    /// </summary>
    /// <param name="AInspectDS"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public TCombinedSubmitChangesResult SubmitChangesToSupplier(AccountsPayableTDS AInspectDS)
    {
        // TODO check permissions
        if (IsUserLoggedIn())
        {
            TSupplierEditUIConnector uiconnector = new TSupplierEditUIConnector();

            TSubmitChangesResult changesResult = uiconnector.SubmitChanges(ref AInspectDS);
            return new TCombinedSubmitChangesResult(changesResult, AInspectDS, new TVerificationResultCollection());
        }

        return new TCombinedSubmitChangesResult(TSubmitChangesResult.scrError, new DataSet(), new TVerificationResultCollection());
    }

    /// <summary>
    /// import data from a web form, ie partners are entering their own data
    /// </summary>
    /// <param name="AFormID"></param>
    /// <param name="AJSONFormData"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string DataImportFromForm(string AFormID, string AJSONFormData)
    {
        try
        {
            // user ANONYMOUS, can only write, not read
            if (!IsUserLoggedIn())
            {
                InitServer();

                if (!LoginInternal("ANONYMOUS", TAppSettingsManager.GetValue("AnonymousUserPasswd")))
                {
                    string message =
                        "In order to process anonymous submission of data from the web, we need to have a user ANONYMOUS which does not have any read permissions";
                    TLogging.Log(message);

                    // do not disclose errors on production version
                    message = "There is a problem on the Server";

                    return "{\"failure\":true, \"data\":{\"result\":\"" + message + "\"}}";
                }
            }
        }
        catch (Exception e)
        {
            TLogging.Log(e.Message);
            TLogging.Log(e.StackTrace);

            Logout();

            return "{\"failure\":true, \"data\":{\"result\":\"Unexpected failure\"}}";
        }

        // remove ext-comp controls, for multi-page forms
        TLogging.Log(AJSONFormData);

        try
        {
            string RequiredCulture = string.Empty;
            AJSONFormData = TJsonTools.RemoveContainerControls(AJSONFormData, ref RequiredCulture);

            AJSONFormData = AJSONFormData.Replace("\"txt", "\"").
                            Replace("\"chk", "\"").
                            Replace("\"rbt", "\"").
                            Replace("\"cmb", "\"").
                            Replace("\"hid", "\"").
                            Replace("\"dtp", "\"").
                            Replace("\n", " ").Replace("\r", "");

            TLogging.Log(AJSONFormData);
            string result = Ict.Petra.Server.MPartner.Import.TImportPartnerForm.DataImportFromForm(AFormID, AJSONFormData);

            Logout();

            return result;
        }
        catch (Exception e)
        {
            TLogging.Log(e.Message);
            TLogging.Log(e.StackTrace);

            Logout();

            return "{\"failure\":true, \"data\":{\"result\":\"Unexpected failure\"}}";
        }
    }

    /// <summary>
    /// testing function for web forms
    /// </summary>
    /// <param name="AFormID"></param>
    /// <param name="AJSONFormData"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string TestingForms(string AFormID, string AJSONFormData)
    {
        // remove ext-comp controls, for multi-page forms
        TLogging.Log(AJSONFormData);

        try
        {
            string RequiredCulture = string.Empty;
            AJSONFormData = TJsonTools.RemoveContainerControls(AJSONFormData, ref RequiredCulture);

            AJSONFormData = AJSONFormData.Replace("\"txt", "\"").
                            Replace("\"chk", "\"").
                            Replace("\"rbt", "\"").
                            Replace("\"cmb", "\"").
                            Replace("\"hid", "\"").
                            Replace("\"dtp", "\"").
                            Replace("\n", " ").Replace("\r", "");

            TLogging.Log(AJSONFormData);
            return "{\"failure\":true, \"data\":{\"result\":\"Nothing happened, just a test\"}}";
        }
        catch (Exception e)
        {
            TLogging.Log(e.Message);
            TLogging.Log(e.StackTrace);
            return "{\"failure\":true, \"data\":{\"result\":\"Unexpected failure\"}}";
        }
    }

    /// <summary>
    /// for the partner that wants to edit/update his/her own data.
    /// this also allows to enter new forms (eg. during application process) without having to reenter some data
    /// </summary>
    /// <param name="AFormID"></param>
    /// <param name="AEmailAddress"></param>
    /// <param name="APassword"></param>
    /// <returns></returns>
    public string GetPreviouslyEnteredData(string AFormID, string AEmailAddress, string APassword)
    {
        // TODO: validate email and password. if true, generate a temporary user, which has read access only to his own partner data, identified by the email?
        // TODO use that temporary user to login to the database. clear user afterwards?
        // TODO or easier: just get the data of the user and return it as JSON string?

        return "error";
    }

    /// <summary>
    /// reset the password of the partner so that he/she can edit their own data
    /// </summary>
    /// <param name="AEmailAddress"></param>
    /// <returns></returns>
    public string RequestNewPasswordForEmail(string AEmailAddress)
    {
        // TODO create a new password and send to the email address, if a partner exists with that email address
        return "error";
    }

    /// <summary>
    /// returns all partner data for records that have been modified on the given date or later.
    /// this should be called by the office OpenPetra instance, to import the data of the OpenPetra instance running on the web.
    /// </summary>
    /// <param name="AUsername">authentication of the user</param>
    /// <param name="APassword"></param>
    /// <param name="AEarliestModifiedDate"></param>
    /// <returns>yml partner export with all data of modified partners</returns>
    public string SyncModifiedPartnerData(string AUsername, string APassword, DateTime AEarliestModifiedDate)
    {
        return "error";
    }

    /// <summary>
    /// get data of all accepted participants, to sync with other tools (eg. seminar registration tool)
    /// </summary>
    [WebMethod(EnableSession = true)]
    public string GetAllParticipants()
    {
        if (IsUserLoggedIn())
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MConference.Applications.TApplicationManagement),
                "GetAllParticipants",
                ";LONG;STRING;");

            DataTable result = TApplicationManagement.GetAllParticipants(TAppSettingsManager.GetInt64("ConferenceTool.EventPartnerKey"),
                TAppSettingsManager.GetValue("ConferenceTool.EventCode"));

            StringWriter sw = new StringWriter();
            result.WriteXml(sw);
            return sw.ToString();
        }

        return string.Empty;
    }
}
}