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
    /// <summary>
    /// Helper Class.
    /// </summary>
    public partial class TCommonControlsHelper
    {
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
    }
}