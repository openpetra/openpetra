//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, alanp
//
// Copyright 2004-2016 by OM International
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

using Ict.Common;
using Ict.Common.Exceptions;

namespace Ict.Petra.Shared.Security
{
    /// <summary>Implements security checks.</summary>
    public static class TSecurityChecks
    {
        #region Shared code for security of setup screens

        private static List <string>FSecurityPermissionsSetupScreensEditingAndSaving = null;

        /// <summary>
        /// Security Permission Check: Editing and Saving of data in 'Setup-screens'
        /// </summary>
        public const string SECURITYPERMISSION_EDITING_AND_SAVING_OF_SETUP_DATA = "EditingAndSavingOfSetupData";

        /// <summary>
        /// Security Permission Check: 'Finance Reporting'
        /// </summary>
        public const string SECURITYPERMISSION_FINANCEREPORTING = "FinanceReporting";

        /// <summary>
        /// Security Restriction: Editing and Saving of data disabled.
        /// </summary>
        public const string SECURITYRESTRICTION_READONLY = "READ-ONLY";

        /// <summary>
        /// Security Restriction: Execution (generating) of a 'Finance Report' (or something opening something like it, e.g.
        /// Donor/Recipient History) denied.
        /// </summary>
        public const string SECURITYRESTRICTION_FINANCEREPORTINGDENIED = "FINANCE_REPORTING_DENIED";

        /// <summary>
        /// Standard security permissions: Editing and saving of data in 'Setup' screens.
        /// </summary>
        public static List <string>SecurityPermissionsSetupScreensEditingAndSaving
        {
            get
            {
                if (FSecurityPermissionsSetupScreensEditingAndSaving == null)
                {
                    var SecurityPermissionRequired = new List <string>();
                    SecurityPermissionRequired.Add(TSecurityChecks.SECURITYPERMISSION_EDITING_AND_SAVING_OF_SETUP_DATA);

                    FSecurityPermissionsSetupScreensEditingAndSaving = SecurityPermissionRequired;
                }

                return FSecurityPermissionsSetupScreensEditingAndSaving;
            }
        }

        /// <summary>
        /// Returns the necessary Module Permission for the saving of data in Setup screens.
        /// </summary>
        /// <param name="AModule">Module that the Setup screen is in.</param>
        /// <returns>Necessary Module Permission for the saving of data in a Setup screens that
        /// is in Module <paramref name="AModule"/>.</returns>
        public static string GetModulePermissionForSavingOfSetupScreenData(string AModule)
        {
            switch (AModule)
            {
                case "MPartner":
                    return SharedConstants.PETRAMODULE_PTNRADMIN;

                case "MPersonnel":
                    return SharedConstants.PETRAMODULE_PERSADMIN;

                case "MFinance":
                    return SharedConstants.PETRAMODULE_FINANCE3;

                case "MFinDev":
                    return SharedConstants.PETRAMODULE_DEVADMIN;

                case "MConference":
                    return SharedConstants.PETRAMODULE_CONFADMIN;

                case "MSysMan":
                    return SharedConstants.PETRAMODULE_SYSADMIN;

                case "MCommon":
                    return "### ERROR ### - MCommon is not an appropriate Module for security checks on the saving of Setup data!";

                case "FIN-EX-RATE":
                    return SharedConstants.PETRAMODULE_FINEXRATE;

                default:
                    throw new EOPAppException(
                    String.Format("Unknown Module {0} passed to Method 'GetModulePermissionForSavingOfSetupScreenData'",
                        AModule));
            }
        }

        #endregion

        #region Shared code for checking the user has the required module permissions

        /// <summary>
        /// Recursively check the module expression and confirm that the user has the required permissions.
        /// This code is used by the server when checking permissions for server methods having the RequiredPermissions attribute,
        /// but also by the client if it needs to check permissions on the client-side and display a consistent message box thoughout the application.
        /// Note that checking on the client side does not negate any check that the server side makes.
        /// Sometimes it is easier to pre-check on the client before starting a new thread and Progress Dialog to make the actual server call.
        /// </summary>
        /// <param name="AModuleExpression">This is in uppercase and is for example FINANCE-1 or even OR(FINANCE-3,PTNRADMIN).  You can use OR or AND
        /// to combine permissions in a comma separated list</param>
        /// <param name="AModuleAccessExceptionContext">An optional string that will be displayed in the client exception dialog if an exception is thrown</param>
        /// <returns>True if the user has access permissions.  Otherwise the method throws a ESecurityModuleAccessDeniedException exception.</returns>
        public static bool CheckUserModulePermissions(string AModuleExpression, string AModuleAccessExceptionContext = "")
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
                                UserInfo.GUserInfo.UserID, module, AModuleAccessExceptionContext);
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
                        UserInfo.GUserInfo.UserID, modulesList, AModuleAccessExceptionContext);
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
                        UserInfo.GUserInfo.UserID, ModuleName, AModuleAccessExceptionContext);
                }

                return true;
            }
        }

        static private string GetModuleOrLedger(string AModuleName)
        {
            if (AModuleName.StartsWith(SharedConstants.LEDGER_MODULESTRING))
            {
                // Get pure ledger number without the LEDGER_MODULESTRING prefix, e.g. '0043' instead of 'LEDGER0043'
                string ALedgerNumberWithoutLeadingZeroes = AModuleName.Substring(SharedConstants.LEDGER_MODULESTRING.Length);

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

        #endregion
    }
}