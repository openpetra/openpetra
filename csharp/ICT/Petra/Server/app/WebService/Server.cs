//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.IO;
using System.Web.Services;
using System.Data;
using System.Collections;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.DB;
using Ict.Petra.Shared.Interfaces; // Implicit reference
using Ict.Petra.Server.App.Main;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.ClientDomain;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.MFinance.AP.UIConnectors;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.AP.Data;
using Jayrock.Json;

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
[WebService]
public class TOpenPetraOrg : WebService
{
    /// <summary>
    /// static: only initialised once for the whole server
    /// </summary>
    static TServerManager TheServerManager = null;

    // make sure the correct config file is used
    static TAppSettingsManager opts = new TAppSettingsManager(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "web.config");

    /// <summary>Initialise the server; this can only be called once, after that it will have no effect;
    /// it will be called automatically by Login</summary>
    [WebMethod]
    public bool InitServer()
    {
        if (TheServerManager == null)
        {
            Catalog.Init();

            TheServerManager = new TServerManager();
            try
            {
                TheServerManager.EstablishDBConnection();

                DomainManager.GSystemDefaultsCache = new TSystemDefaultsCache();
                DomainManager.GSiteKey = DomainManager.GSystemDefaultsCache.GetInt64Default(Ict.Petra.Shared.SharedConstants.SYSDEFAULT_SITEKEY);
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                throw;
            }
        }

        // create a database connection for each user
        if (Ict.Common.DB.DBAccess.GDBAccessObj == null)
        {
            TheServerManager.EstablishDBConnection();
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

            // TODO? store user principal in http cache? HttpRuntime.Cache
            TPetraPrincipal userData = TClientManager.PerformLoginChecks(
                username.ToUpper(), password, "WEB", "127.0.0.1", out ProcessID, out ASystemEnabled);
            Session["LoggedIn"] = true;
            return true;
        }
        catch (Exception e)
        {
            TLogging.Log(e.Message);
            TLogging.Log(e.StackTrace);
            Session["LoggedIn"] = false;
            Ict.Common.DB.DBAccess.GDBAccessObj.RollbackTransaction();
            return false;
        }
    }

    /// <summary>Login a user</summary>
    [WebMethod(EnableSession = true)]
    public string Login(string username, string password)
    {
        bool loggedIn = LoginInternal(username, password);

        return Jayrock.Json.Conversion.JsonConvert.ExportToString(loggedIn);
    }

    /// <summary>check if the user has logged in successfully</summary>
    [WebMethod(EnableSession = true)]
    public bool IsUserLoggedIn()
    {
        object loggedIn = Session["LoggedIn"];

        if (null != loggedIn)
        {
            return (bool)loggedIn;
        }

        return false;
    }

    /// <summary>log the user out</summary>
    [WebMethod(EnableSession = true)]
    public void Logout()
    {
        TLogging.Log("Logout from a session", TLoggingType.ToLogfile | TLoggingType.ToConsole);
        DBAccess.GDBAccessObj.CloseDBConnection();
        Session.Abandon();
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
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult changesResult = uiconnector.SubmitChanges(ref AInspectDS, out VerificationResult);
            return Jayrock.Json.Conversion.JsonConvert.ExportToString(new TCombinedSubmitChangesResult(changesResult, AInspectDS, VerificationResult));
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
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult changesResult = uiconnector.SubmitChanges(ref AInspectDS, out VerificationResult);
            return new TCombinedSubmitChangesResult(changesResult, AInspectDS, VerificationResult);
        }

        return new TCombinedSubmitChangesResult(TSubmitChangesResult.scrError, new DataSet(), new TVerificationResultCollection());
    }

    private string parseJSonValues(JsonObject ARoot)
    {
        string result = "";

        foreach (string key in ARoot.Names)
        {
            if (key.ToString().StartsWith("ext-comp"))
            {
                string content = parseJSonValues((JsonObject)ARoot[key]);

                if (content.Length > 0)
                {
                    if (result.Length > 0)
                    {
                        result += ",";
                    }

                    result += content;
                }
            }
            else
            {
                if (result.Length > 0)
                {
                    result += ",";
                }

                if (key.EndsWith("CountryCode"))
                {
                    // we need this so that we can parse the dates correctly from json
                    Ict.Common.Catalog.Init(ARoot[key].ToString(), ARoot[key].ToString());
                }

                result += "\"" + key + "\":\"" + ARoot[key].ToString().Replace("\n", "<br/>").Replace("\"", "&quot;") + "\"";
            }
        }

        return result;
    }

    /// remove ext-comp controls, for multi-page forms
    private string RemoveContainerControls(string AJSONFormData)
    {
        JsonObject root = (JsonObject)Jayrock.Json.Conversion.JsonConvert.Import(AJSONFormData);

        string result = "{" + parseJSonValues(root) + "}";

        return result;
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
        // user ANONYMOUS, can only write, not read
        if (!IsUserLoggedIn())
        {
            if (!LoginInternal("ANONYMOUS", TAppSettingsManager.GetValueStatic("AnonymousUserPasswd")))
            {
                string message =
                    "In order to process anonymous submission of data from the web, we need to have a user ANONYMOUS which does not have any read permissions";
                TLogging.Log(message);

#if DEBUGMODE
#else
                // do not disclose errors on production version
                message = "";
#endif

                return "{\"failure\":true, \"data\":{\"result\":\"" + message + "\"}}";
            }
        }

        // remove ext-comp controls, for multi-page forms
        TLogging.Log(AJSONFormData);

        try
        {
            AJSONFormData = RemoveContainerControls(AJSONFormData);

            AJSONFormData = AJSONFormData.Replace("\"txt", "\"").
                            Replace("\"chk", "\"").
                            Replace("\"rbt", "\"").
                            Replace("\"cmb", "\"").
                            Replace("\"hid", "\"").
                            Replace("\"dtp", "\"").
                            Replace("\n", " ").Replace("\r", "");

            TLogging.Log(AJSONFormData);
            return Ict.Petra.Server.MPartner.Import.TImportPartnerForm.DataImportFromForm(AFormID, AJSONFormData);
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
}
}