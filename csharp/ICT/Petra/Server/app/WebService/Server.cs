//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Web.Services;
using System.Data;
using Ict.Common;
using Ict.Petra.Server.App.Main;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors;
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
    static TAppSettingsManager opts = new TAppSettingsManager("web.config");

    /// <summary>Initialise the server; this can only be called once, after that it will have no effect;
    /// it will be called automatically by Login</summary>
    [WebMethod]
    public bool InitServer()
    {
        if (TheServerManager == null)
        {
            TheServerManager = new TServerManager();
            try
            {
                TheServerManager.EstablishDBConnection();
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                throw;
            }
        }

        return true;
    }

    /// <summary>Login a user</summary>
    [WebMethod(EnableSession = true)]
    public bool Login(string username, string password)
    {
        Int32 ProcessID;
        bool ASystemEnabled;

        try
        {
            InitServer();

            // TODO? store user principal in http cache? HttpRuntime.Cache
            TPetraPrincipal userData = TClientManager.PerformLoginChecks(username, password, "WEB", "127.0.0.1", out ProcessID, out ASystemEnabled);
            Session["LoggedIn"] = true;
            return true;
        }
        catch (Exception e)
        {
            TLogging.Log(e.Message);
            Session["LoggedIn"] = false;
            return false;
        }
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
}
}