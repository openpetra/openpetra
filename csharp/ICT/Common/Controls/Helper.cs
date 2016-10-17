//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 chadds
//		 ashleyc
//       sethb
//       christiank
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Text.RegularExpressions;
using Ict.Common;
using Ict.Common.IO;

using Ict.Common.Exceptions;

namespace Ict.Common.Controls
{
    #region partial class TCommonControlsHelper

    /// <summary>
    /// Helper Class.
    /// </summary>
    public partial class TCommonControlsHelper
    {
        /// <summary>
        /// Used for passing what certain Controls regard as the identifier string of inactive items (e.g. inactive Cost Centres).
        /// Set up once at the startup of the application!
        /// </summary>
        public static Func <string>SetInactiveIdentifier;

        /// <summary>
        /// The particulary 'yellow colour' that highlights Partners of Partner Class PERSON
        /// </summary>
        public static readonly Color PartnerClassPERSONColour = System.Drawing.Color.FromArgb(255, 255, 94);


        /// <summary>
        /// Used for determining whether the Control is instantiated in the WinForms Designer, or not.
        /// </summary>
        /// <returns>True if the Control is instantiated in the WinForms Designer, otherwise false.</returns>
        public static bool IsInDesignMode()
        {
            bool returnFlag = false;

#if DEBUG
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                returnFlag = true;
                //                MessageBox.Show("Design Mode #1");
            }
            else if (Application.ExecutablePath.IndexOf("devenv.exe", StringComparison.OrdinalIgnoreCase) > -1)
            {
                returnFlag = true;
                //                MessageBox.Show("Design Mode #2");
            }
            else
            {
                //                MessageBox.Show("Runtime Mode");
            }
#endif

            return returnFlag;
        }

        /// <summary>
        /// Sets a Partner Key TextBox's BackColour according to the <paramref name="APartnerClass" />.
        /// </summary>
        /// <param name="APartnerClass">Partner Class of the Partner.</param>
        /// <param name="APartnerKeyTextBox">TextBox that displays the PartnerKey.</param>
        /// <param name="AOriginalPartnerClassColour">State-holding Field of a Class.</param>
        public static void SetPartnerKeyBackColour(string APartnerClass, TextBox APartnerKeyTextBox, Color ? AOriginalPartnerClassColour)
        {
            if (APartnerClass == "PERSON")
            {
                if (APartnerKeyTextBox.BackColor != TCommonControlsHelper.PartnerClassPERSONColour)
                {
                    AOriginalPartnerClassColour = APartnerKeyTextBox.BackColor;
                    APartnerKeyTextBox.BackColor = TCommonControlsHelper.PartnerClassPERSONColour;
                }
            }
            else
            {
                APartnerKeyTextBox.BackColor = AOriginalPartnerClassColour ?? System.Drawing.Color.White;
            }
        }

        /// <summary>
        /// Used for determining whether any of the radio buttons inside a group box is selected
        /// </summary>
        /// <param name="ARadioGroupBox">Parent Group box that contains radio buttons.</param>
        /// <returns>True if any of the radio buttons inside the group box is selected, otherwise false.</returns>
        public static Boolean IsAnyRadioButtonSelected(GroupBox ARadioGroupBox)
        {
            foreach (var control1 in ARadioGroupBox.Controls)
            {
                if (control1 is RadioButton)
                {
                    if ((control1 as RadioButton).Checked)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    #endregion

    #region TWireUpSelectAllTextOnFocus

    /// <summary>
    /// Create an instance of this Class to make a TextBox automatically select all its text when
    /// the user enters the TextBox - no matter how the user enters the TextBox (by mouse click,
    /// by keyboard [TAB, SHIFT-TAB, keyboard shortcut]) - sounds trivial but isn't!
    /// </summary>
    public class TWireUpSelectAllTextOnFocus
    {
        static bool FSelectedState = false;
        static Func <bool>FEvaluationAction;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ATextBox">TextBox to which the bevhaviour that this Class adds should be added.</param>
        /// <param name="AEvaluationFunction">Optional Delegate. If specified it must return a bool value that
        /// instructs this Class to perform the 'Select All' action that this Class adds to a TextBox
        /// <em>only if true is returned by the Delegate</em>. (Default=null).
        /// </param>
        public TWireUpSelectAllTextOnFocus(TextBox ATextBox, Func <bool>AEvaluationFunction = null)
        {
            FEvaluationAction = AEvaluationFunction;

            ATextBox.GotFocus += new EventHandler((sender, e) =>
                {
                    if (Control.MouseButtons == MouseButtons.None)
                    {
                        if (ShouldSelectAll(AEvaluationFunction))
                        {
                            ATextBox.SelectAll();

                            FSelectedState = true;
                        }
                    }
                });

            ATextBox.Leave += new EventHandler((sender, e) => {
                    FSelectedState = false;
                });

            ATextBox.MouseUp += new MouseEventHandler((sender, e) => {
                    if (!FSelectedState)
                    {
                        if (ShouldSelectAll(AEvaluationFunction))
                        {
                            FSelectedState = true;

                            // Only select all TextBox text if the user didn't click-and-drag into the TextBox
                            // to select a specific text portion!
                            if (ATextBox.SelectionLength == 0)
                            {
                                ATextBox.SelectAll();
                            }
                        }
                    }
                });
        }

        private bool ShouldSelectAll(Func <bool>AEvaluationAction)
        {
            bool ReturnValue = true;

            if (FEvaluationAction != null)
            {
                ReturnValue = FEvaluationAction();
            }

            return ReturnValue;
        }
    }

    #endregion

    #region static class TCommonControlsExtensions

    /// <summary>
    /// A class for extensions to common controls
    /// </summary>
    public static class TCommonControlsExtensions
    {
        /// <summary>
        /// Control extension method to set the 'DoubleBuffered' property for various types of control.
        /// Used in particular for Filter/Find panels
        /// </summary>
        /// <param name="AControl">The control whose property is to be set</param>
        /// <param name="AValue">Set to True to set double buffered painting of the control</param>
        public static void SetDoubleBuffered(this Control AControl, bool AValue)
        {
            Type ControlType = AControl.GetType();
            PropertyInfo Info = ControlType.GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);

            Info.SetValue(AControl, AValue, null);
        }
    }

    #endregion

    #region static class TCommonControlsSecurity

    /// <summary>
    /// A static class that can handle client side security checks above and beyond simple YAML checks
    /// </summary>
    public static class TCommonControlsSecurity
    {
        /// <summary>
        /// A delegate that is required in order to access the module permissions features in Petra.Shared
        /// </summary>
        /// <param name="AModuleExpression">A module access permission such as PTNRUSER or even OR(PTNRUSER,FINANCE-1)</param>
        /// <param name="AModuleAccessExceptionContext">Optional context</param>
        /// <returns></returns>
        public delegate bool CheckUserModulePermissionsDelegate(string AModuleExpression, string AModuleAccessExceptionContext = "");

        /// <summary>
        /// A delegate that is required in order to show the 'standard' module access denied message
        /// </summary>
        /// <param name="AException">The exception</param>
        /// <param name="ATypeWhichRaisesError">Can be null if the exception is supplied with a Context</param>
        public delegate void SecurityAccessDeniedMessageDelegate(ESecurityAccessDeniedException AException, System.Type ATypeWhichRaisesError);

        /// <summary>
        /// Local copy of the delegate handler address
        /// </summary>
        public static CheckUserModulePermissionsDelegate FCheckUserModulePermissions = null;

        /// <summary>
        /// Local copy of the delegate handler address
        /// </summary>
        public static SecurityAccessDeniedMessageDelegate FSecurityAccessDeniedMessage = null;

        /// <summary>
        /// Public setter for the local copy of the address for the delegate
        /// </summary>
        public static CheckUserModulePermissionsDelegate CheckUserModulePermissions
        {
            set
            {
                FCheckUserModulePermissions = value;
            }
        }

        /// <summary>
        /// Public setter for the local copy of the address for the delegate
        /// </summary>
        public static SecurityAccessDeniedMessageDelegate SecurityAccessDeniedMessage
        {
            set
            {
                FSecurityAccessDeniedMessage = value;
            }
        }

        // It would have been nice to use the equivalents in Petra.Shared
        private static readonly string PETRAMODULE_PTNRUSER = "PTNRUSER";
        private static readonly string PETRAMODULE_PERSONNEL = "PERSONNEL";
        private static readonly string PETRAMODULE_CONFERENCE = "CONFERENCE";
        private static readonly string PETRAMODULE_FINANCE1 = "FINANCE-1";
        private static readonly string PETRAMODULE_FINANCE2 = "FINANCE-2";
        private static readonly string PETRAMODULE_FINANCE3 = "FINANCE-3";
        private static readonly string PETRAMODULE_FINANCERPT = "FINANCE-RPT";
        private static readonly string PETRAMODULE_DEVUSER = "DEVUSER";
        private static readonly string PETRAMODULE_SYSADMIN = "SYSMAN";

        private static readonly string MODULE_ACCESS_MANAGER_CLIENT_PROXY = "[raised by Client Proxy for Module Access Manager]";

        /// <summary>
        /// This is client-side code to check user permissions for a module.  It does NOT use the PermissionsRequired attribute of the UINavigation file.
        /// It does use the element name for the module and a switch statement with hard-coded permissions.
        /// It is not a substitute for server side permissions-checking but it DOES provide a first-pass check on the client side that the user has not
        /// tampered with the UINavigation file itself.
        /// </summary>
        /// <param name="AElementName">The XML element name of the module</param>
        /// <returns></returns>
        public static bool CheckUserAccessToModuleUsingModuleElementName(string AElementName)
        {
            bool ret = true;
            string modulePermissionRequired = string.Empty;

            switch (AElementName)
            {
                case "Partner" :
                    modulePermissionRequired = PETRAMODULE_PTNRUSER;
                    break;

                case "Finance":
                case "FinanceSystem":
                    modulePermissionRequired = PETRAMODULE_FINANCE1;
                    break;

                case "Personnel":
                    modulePermissionRequired = PETRAMODULE_PERSONNEL;
                    break;

                case "ConferenceManagement":
                    modulePermissionRequired = PETRAMODULE_CONFERENCE;
                    break;

                case "FinancialDevelopment":
                    modulePermissionRequired = PETRAMODULE_DEVUSER;
                    break;

                case "UserSettings":
                    break;

                case "SystemManager":
                    modulePermissionRequired = PETRAMODULE_SYSADMIN;
                    break;

                default:
                    modulePermissionRequired = "UNKNOWN_MODULE";
                    break;
            }

            if ((modulePermissionRequired != string.Empty) && (FCheckUserModulePermissions != null))
            {
                try
                {
                    FCheckUserModulePermissions(modulePermissionRequired, "Folder Navigation " + MODULE_ACCESS_MANAGER_CLIENT_PROXY);
                }
                catch (ESecurityModuleAccessDeniedException ex)
                {
                    ret = false;

                    if (FSecurityAccessDeniedMessage != null)
                    {
                        FSecurityAccessDeniedMessage(ex, null);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// This is client-side code to check user permissions for a module.  It does NOT use the PermissionsRequired attribute of the UINavigation file.
        /// It does use the Namespace name for the action that has been clicked and a switch statement with hard-coded permissions.
        /// It is not a substitute for server side permissions-checking but it DOES provide a **STRONG** check on the client side that the user has not
        /// tampered with the UINavigation file itself.
        /// With this check the user can neither modify the PermissionsRequired attribute nor move the task to a different place in the file!
        /// </summary>
        /// <param name="ANode">The node that contains the namespace and the ActionOpenScreen or ActionClick</param>
        /// <returns></returns>
        public static bool CheckUserAccessToModuleUsingModuleNamespaceName(XmlNode ANode)
        {
            bool ret = true;
            string modulePermissionRequired = string.Empty;
            string namespaceName = TYml2Xml.GetAttributeRecursive(ANode, "Namespace");
            string actionScreenClass = TYml2Xml.GetAttributeRecursive(ANode, "ActionOpenScreen");
            string actionClick = TYml2Xml.GetAttributeRecursive(ANode, "ActionClick");
            string actionContext = TYml2Xml.GetAttributeRecursive(ANode, "Context");

            if (namespaceName.Contains("Client.MPartner.Gui") || namespaceName.EndsWith("Client.MReporting.Gui.MPartner"))
            {
                // Although this looks like Partner there are some subtle variations, which we test below
                modulePermissionRequired = PETRAMODULE_PTNRUSER;
            }

            // The order of these if/else clauses is important!!
            if (namespaceName.Contains("Client.MPersonnel.Gui") || namespaceName.Contains("Client.MReporting.Gui.MPersonnel")
                || (actionContext == "Personnel") || (actionContext == "LongTermApp") || (actionContext == "ShortTermApp")
                || actionClick.StartsWith("TPersonnelMain."))
            {
                // Note that this includes Personnel Extracts and MCommon items
                modulePermissionRequired = PETRAMODULE_PERSONNEL;
            }
            else if (namespaceName.Contains("Client.MFinance.Gui.ICH"))
            {
                // ICH is associated with MonthEnd, so needs Finance-2
                modulePermissionRequired = string.Format("AND({0},{1})", PETRAMODULE_FINANCE1, PETRAMODULE_FINANCE2);
            }
            else if (namespaceName.Contains("Client.MFinance.Gui")
                     || (actionContext == "Finance") || actionClick.StartsWith("TFinanceMain."))
            {
                // This includes Finance extracts and MCommon items
                if (actionScreenClass == "TPeriodEndMonth")
                {
                    // MonthEnd needs Finance-2
                    modulePermissionRequired = string.Format("AND({0},{1})", PETRAMODULE_FINANCE1, PETRAMODULE_FINANCE2);
                }
                else if ((actionScreenClass == "TPeriodEndYear") || (actionScreenClass == "TFrmGLCreateLedgerDialog")
                         || actionClick.EndsWith("DeleteLedger"))
                {
                    // These need Finance-3
                    modulePermissionRequired = string.Format("AND({0},{1})", PETRAMODULE_FINANCE1, PETRAMODULE_FINANCE3);
                }
                else
                {
                    modulePermissionRequired = PETRAMODULE_FINANCE1;
                }
            }
            else if (namespaceName.EndsWith("Client.MReporting.Gui.MFinance"))
            {
                if ((actionScreenClass == "TFrmStewardshipForPeriod") || (actionScreenClass == "TFrmGiftBatchDetail")
                    || (actionScreenClass == "TFrmAP_RemittanceAdvice"))
                {
                    // These screens are reports but are actually screens to re-print something
                    modulePermissionRequired = PETRAMODULE_FINANCE1;
                }
                else
                {
                    modulePermissionRequired = string.Format("AND({0},{1})", PETRAMODULE_FINANCE1, PETRAMODULE_FINANCERPT);
                }
            }
            else if (namespaceName.Contains("Client.MCommon.Gui"))
            {
                // Now we have to handle MCommon - mostly these are Partner screens
                if (actionScreenClass == "TFrmCurrencySetup")
                {
                    modulePermissionRequired = PETRAMODULE_FINANCE1;
                }
                else if (actionScreenClass.Contains("Personnel"))
                {
                    modulePermissionRequired = PETRAMODULE_PERSONNEL;
                }
                else
                {
                    modulePermissionRequired = PETRAMODULE_PTNRUSER;
                }
            }
            else if (namespaceName.Contains("Client.MFinDev.Gui") || namespaceName.EndsWith("Client.MReporting.Gui.MFinDev"))
            {
                modulePermissionRequired = PETRAMODULE_DEVUSER;
            }
            else if (namespaceName.Contains("Client.MConference.Gui") || namespaceName.EndsWith("Client.MReporting.Gui.MConference"))
            {
                modulePermissionRequired = PETRAMODULE_CONFERENCE;
            }
            else if (namespaceName.Contains("Client.MSysMan.Gui"))
            {
                if ((actionScreenClass == "TFrmUserPreferencesDialog") || actionClick.EndsWith(".SetUserPassword")
                    || actionClick.EndsWith(".RunGlobalDataUpdater"))
                {
                    // No special permissions required
                }
                else if (actionScreenClass == "TFrmIntranetExportDialog")
                {
                    modulePermissionRequired = string.Format("OR({0},{1},{2})", PETRAMODULE_PTNRUSER, PETRAMODULE_FINANCE1, PETRAMODULE_PERSONNEL);
                }
                else
                {
                    modulePermissionRequired = PETRAMODULE_SYSADMIN;
                }
            }

            // Now run the permissions check
            if ((modulePermissionRequired != string.Empty) && (FCheckUserModulePermissions != null))
            {
                Console.WriteLine("Launching {0} -- Checking for permission: {1}", ANode.Name, modulePermissionRequired);

                try
                {
                    FCheckUserModulePermissions(modulePermissionRequired, "Launch Task " + MODULE_ACCESS_MANAGER_CLIENT_PROXY);
                }
                catch (ESecurityModuleAccessDeniedException ex)
                {
                    ret = false;

                    if (FSecurityAccessDeniedMessage != null)
                    {
                        FSecurityAccessDeniedMessage(ex, null);
                    }
                }
            }

            return ret;
        }
    }

    #endregion
}