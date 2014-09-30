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
using System.Collections;
using System.Reflection;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;

using System.Data;

namespace Ict.Petra.Server.App.Core.Security
{
    /// <summary>
    /// The TModuleAccessManager class provides provides functions to work with the
    /// Module Access Permissions of a Petra DB.
    /// </summary>
    public class TModuleAccessManager
    {
        private const string LEDGER_MODULESTRING = "LEDGER";

        /// <summary>
        /// load the modules available to the given user
        /// </summary>
        /// <param name="AUserID"></param>
        /// <returns></returns>
        public static string[] LoadUserModules(String AUserID)
        {
            string[] ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            SUserModuleAccessPermissionTable UserModuleAccessPermissionsDT;
            Int32 CounterOverall;
            Int32 CounterAdded;
            ArrayList UserModuleAccessPermissions;

            // ModulesList: string;
            // Counter: Int32;
            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

                if (SUserModuleAccessPermissionAccess.CountViaSUser(AUserID, ReadTransaction) > 0)
                {
                    UserModuleAccessPermissionsDT = SUserModuleAccessPermissionAccess.LoadViaSUser(AUserID, ReadTransaction);

//TLogging.Log("UserModuleAccessPermissionsDT.Rows.Count - 1: " + (UserModuleAccessPermissionsDT.Rows.Count - 1).ToString());

                    // Dimension the ArrayList with the maximum number of ModuleAccessPermissions first
                    UserModuleAccessPermissions = new ArrayList(UserModuleAccessPermissionsDT.Rows.Count - 1);

                    CounterAdded = 0;

                    for (CounterOverall = 0; CounterOverall <= UserModuleAccessPermissionsDT.Rows.Count - 1; CounterOverall += 1)
                    {
                        if (UserModuleAccessPermissionsDT[CounterOverall].CanAccess)
                        {
//TLogging.Log("UserModuleAccessPermissionsDT[" + CounterOverall.ToString() + "].ModuleId: " + UserModuleAccessPermissionsDT[CounterOverall].ModuleId + ": CounterAdded: " + CounterAdded.ToString());

                            UserModuleAccessPermissions.Add(UserModuleAccessPermissionsDT[CounterOverall].ModuleId);
                            CounterAdded = CounterAdded + 1;
                        }
                    }

                    if (CounterAdded != 0)
                    {
                        // Copy contents of the ArrayList into the ReturnValue
                        ReturnValue = new string[CounterAdded];
                        Array.Copy(UserModuleAccessPermissions.ToArray(), ReturnValue, CounterAdded);

                        // ModulesList := '';
                        //
                        // for Counter := 0 to CounterAdded  1 do
                        // begin
                        // Console.WriteLine('ModulesList: working on Counter ' + Counter.ToString());
                        // ModulesList := ModulesList + UserModuleAccessPermissionsArray[Counter].ToString() + #10#13;
                        // end;
                        // Console.WriteLine(ModulesList);
                    }
                    else
                    {
                        ReturnValue = new string[0];
                    }
                }
                else
                {
                    ReturnValue = new string[0];
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(8, "TModuleAccessManager.LoadUserModules: committed own transaction.");
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// recursively check the expression
        /// </summary>
        /// <param name="AModuleExpression">is in uppercase</param>
        /// <returns>true if the user has access permissions</returns>
        static public bool CheckUserModulePermissions(string AModuleExpression)
        {
            if (AModuleExpression.StartsWith("OR(") || AModuleExpression.StartsWith("AND("))
            {
                // find the closing bracket
                int openingBracketPos = AModuleExpression.IndexOf('(');
                int closingBracketPos = AModuleExpression.IndexOf(')');

                if (closingBracketPos < 0)
                {
                    throw new ArgumentException("missing closing bracket");
                }

                string modulesList = AModuleExpression.Substring(openingBracketPos + 1, closingBracketPos - openingBracketPos - 1);
                string[] modules = modulesList.Split(new char[] { ',' });
                bool oneTrue = false;

                foreach (string module in modules)
                {
                    if (!UserInfo.GUserInfo.IsInModule(module))
                    {
                        if (AModuleExpression.StartsWith("AND("))
                        {
                            throw new ESecurityModuleAccessDeniedException(String.Format(
                                    Catalog.GetString("No access for user {0} to module {1}."),
                                    UserInfo.GUserInfo.UserID, module),
                                UserInfo.GUserInfo.UserID, module);
                        }
                    }
                    else
                    {
                        oneTrue = true;
                    }
                }

                if (AModuleExpression.StartsWith("OR(") && !oneTrue)
                {
                    throw new ESecurityModuleAccessDeniedException(String.Format(
                            Catalog.GetString("No access for user {0} to either of the modules {1}."),
                            UserInfo.GUserInfo.UserID, modulesList),
                        UserInfo.GUserInfo.UserID, modulesList);
                }

                return true;
            }
            else
            {
                // just a single module name
                string ModuleName = AModuleExpression;

                // find the closing bracket if there is one
                int closingBracketPos = AModuleExpression.IndexOf(')');

                if (closingBracketPos > -1)
                {
                    ModuleName = AModuleExpression.Substring(0, closingBracketPos);
                }

                if (ModuleName.Contains(","))
                {
                    throw new ArgumentException("There is a Comma in the wrong place");
                }

                if (!UserInfo.GUserInfo.IsInModule(ModuleName))
                {
                    throw new ESecurityModuleAccessDeniedException(String.Format(
                            Catalog.GetString("No access for user {0} to {1}."),
                            UserInfo.GUserInfo.UserID, GetModuleOrLedger(ModuleName)),
                        UserInfo.GUserInfo.UserID, ModuleName);
                }

                return true;
            }
        }

        static private string GetModuleOrLedger(string AModuleName)
        {
            if (AModuleName.StartsWith(LEDGER_MODULESTRING))
            {
                // Get pure ledger number without the LEDGER_MODULESTRING prefix, e.g. '0043' instead of 'LEDGER0043'
                string ALedgerNumberWithoutLeadingZeroes = AModuleName.Substring(LEDGER_MODULESTRING.Length);

                // Determine ledger number without leading zeroes
                for (int Counter = 0; Counter < ALedgerNumberWithoutLeadingZeroes.Length; Counter++)
                {
                    if (ALedgerNumberWithoutLeadingZeroes[Counter] != '0')
                    {
                        ALedgerNumberWithoutLeadingZeroes = ALedgerNumberWithoutLeadingZeroes.Substring(Counter);
                    }
                }

                return Catalog.GetString("Ledger") + " " + ALedgerNumberWithoutLeadingZeroes;
            }
            else
            {
                return Catalog.GetString("Module") + " " + AModuleName;
            }
        }

        /// throws an exception if the current user does not have enough permission to access the method;
        /// this uses a custom attribute associated with the method of the connector
        static public bool CheckUserPermissionsForMethod(System.Type AConnectorType, string AMethodName, string AParameterTypes)
        {
            return CheckUserPermissionsForMethod(AConnectorType, AMethodName, AParameterTypes, -1);
        }

        /// throws an exception if the current user does not have enough permission to access the method;
        /// this uses a custom attribute associated with the method of the connector
        static public bool CheckUserPermissionsForMethod(System.Type AConnectorType, string AMethodName, string AParameterTypes, Int32 ALedgerNumber)
        {
            MethodInfo[] methods = AConnectorType.GetMethods();

            MethodInfo MethodToTest = null;
            AParameterTypes = AParameterTypes.Replace("[]", ".ARRAY");

            foreach (MethodInfo method in methods)
            {
                if (method.Name == AMethodName)
                {
                    string ParameterTypes = ";";
                    ParameterInfo[] Parameters = method.GetParameters();

                    foreach (ParameterInfo Parameter in Parameters)
                    {
                        string ParameterName = Parameter.ParameterType.ToString().Replace("&", "");

                        ParameterName = ParameterName.Replace("System.Collections.Generic.Dictionary`2", "DICTIONARY");
                        ParameterName = ParameterName.Replace("Ict.Common.", string.Empty);
                        ParameterName = ParameterName.Replace("System.", string.Empty);

                        if (ParameterName.Contains("."))
                        {
                            ParameterName = ParameterName.Substring(ParameterName.LastIndexOf(".") + 1);
                        }

                        ParameterName = ParameterName.Replace("`1", string.Empty);
                        ParameterName = ParameterName.Replace("`2", string.Empty);
                        ParameterName = ParameterName.Replace("Boolean", "bool");
                        ParameterName = ParameterName.Replace("Int32", "int");
                        ParameterName = ParameterName.Replace("Int64", "long");
                        ParameterName = ParameterName.Replace("[]", ".Array");

                        ParameterTypes += ParameterName + ";";
                    }

                    ParameterTypes = ParameterTypes.ToUpper();

                    if (ParameterTypes == AParameterTypes)
                    {
                        MethodToTest = method;
                        break;
                    }
                }
            }

            if (MethodToTest != null)
            {
                System.Object[] attributes = MethodToTest.GetCustomAttributes(typeof(RequireModulePermissionAttribute), false);

                if ((attributes != null) && (attributes.Length > 0))
                {
                    RequireModulePermissionAttribute requiredModules = (RequireModulePermissionAttribute)attributes[0];

                    string moduleExpression = requiredModules.RequiredModulesExpression.ToUpper();

                    if (moduleExpression == "NONE")
                    {
                        return true;
                    }

                    try
                    {
                        CheckUserModulePermissions(moduleExpression);

                        if (ALedgerNumber != -1)
                        {
                            CheckUserModulePermissions(LEDGER_MODULESTRING + ALedgerNumber.ToString("0000"));
                        }
                    }
                    catch (ESecurityModuleAccessDeniedException Exc)
                    {
                        string msg =
                            String.Format(Catalog.GetString(
                                    "Module access permission was violated for method '{0}' in Connector class '{1}':  Required Module access permission: {2}, UserName: {3}"),
                                AMethodName, AConnectorType, Exc.Module, Exc.UserName);
                        TLogging.Log(msg);

                        Exc.Context = AMethodName + " [raised by ModuleAccessManager]";

                        throw;
                    }
                    catch (ArgumentException argException)
                    {
                        throw new EOPAppException("Problem with ModulePermissions, " +
                            argException.Message + ": '" +
                            moduleExpression + "' for " +
                            AConnectorType.ToString() + "." +
                            AMethodName + "()", argException);
                    }

                    return true;
                }
            }

            // TODO: resolve module permission from namespace?

            throw new EOPAppException(
                "Missing definition of access permission for method " + AMethodName + " in Connector class " + AConnectorType);
        }
    }

    /// <summary>
    /// attribute for annotation of server functions. Instantiator will check access permissions
    /// </summary>
    public class RequireModulePermissionAttribute : System.Attribute
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ARequiredModulesExpression">this is a logical expression, eg. OR(MSYSMAN,AND(PTNRADMIN,DEVADMIN))</param>
        public RequireModulePermissionAttribute(string ARequiredModulesExpression)
        {
            RequiredModulesExpression = ARequiredModulesExpression;
        }

        /// <summary>
        /// list of modules for which the user needs to have access permissions.
        /// this is a logical expression, eg. OR(MSYSMAN,AND(PTNRADMIN,DEVADMIN))
        /// </summary>
        public readonly string RequiredModulesExpression;
    }
}