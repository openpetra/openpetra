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
using System;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Testing.NUnitForms
{
    /// <summary>
    /// test the dialog for selecting CSV separators
    /// </summary>
    public class TDlgSelectCSVSeparatorTester
    {
        private TDlgSelectCSVSeparator TheObject;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="hWnd"></param>
        public TDlgSelectCSVSeparatorTester(IntPtr hWnd)
        {
            TheObject = (TDlgSelectCSVSeparator)Form.FromHandle(hWnd);

            if (TheObject == null)
            {
                throw new Exception("cannot find form for TDlgSelectCSVSeparatorTester");
            }
        }

        /// <summary>
        /// access to the properties
        /// </summary>
        public TDlgSelectCSVSeparator Properties
        {
            get
            {
                return TheObject;
            }
        }
    }
}