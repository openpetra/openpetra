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
                APartnerKeyTextBox.BackColor = AOriginalPartnerClassColour ?? System.Drawing.SystemColors.Control;
            }
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