/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Web.Services;
using Ict.Common;
using Ict.Petra.Server.App.Main;
using Ict.Petra.Shared.Security;

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
}
}