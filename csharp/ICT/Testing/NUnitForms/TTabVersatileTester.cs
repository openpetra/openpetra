//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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

using Ict.Petra.Client.CommonControls;
using Ict.Common.Controls;
using System.Windows.Forms;
using NUnit.Extensions.Forms;

namespace Ict.Testing.NUnitForms
{
    public class TTabVersatileTester : TabControlTester
    {
        /// constructor
        public TTabVersatileTester()
        {
        }

        /// constructor
        public TTabVersatileTester(string name, Form form) : base(name, form)
        {
        }

        /// constructor
        public TTabVersatileTester(string name, string formName) : base(name, formName)
        {
        }

        /// constructor
        public TTabVersatileTester(string name) : base(name)
        {
        }

        /// constructor
        public TTabVersatileTester(TabControlTester tester, int index) : base(tester, index)
        {
        }

        /// <summary>
        /// access the properties of the auto populated combobox
        /// </summary>
        public new TTabVersatile Properties
        {
            get
            {
                return (TTabVersatile) base.TheObject;
            }
        }
    }
}