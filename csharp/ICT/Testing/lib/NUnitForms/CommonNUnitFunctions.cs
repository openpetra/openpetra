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
using System;
using System.IO;
using System.Windows.Forms;
using NUnit.Extensions.Forms;
using NUnit.Framework;
using NUnit.Framework.Constraints;


using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.MFinance.Gui;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;

namespace Ict.Testing.NUnitForms
{
    /// <summary>
    /// Description of Class1.
    /// </summary>
    public class CommonNUnitFunctions : NUnitFormTest
    {
        public CommonNUnitFunctions()
        {
        }

        public String lastMessageTitle;
        public String lastMessageText;

        public void WaitForMessageBox(NUnit.Extensions.Forms.MessageBoxTester.Command cmd)
        {
            lastMessageTitle = "";
            lastMessageText = "";

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);

                System.Console.WriteLine("Title: " + tester.Title);
                System.Console.WriteLine("Message: " + tester.Text);

                lastMessageTitle = tester.Title;
                lastMessageText = tester.Text;

                tester.SendCommand(cmd);
            };
        }
    }
}