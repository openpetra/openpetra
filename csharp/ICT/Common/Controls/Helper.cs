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
}