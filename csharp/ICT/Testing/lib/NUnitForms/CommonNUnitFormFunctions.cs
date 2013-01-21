//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
//
// Copyright 2004-2011 by OM International
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
using System.Text.RegularExpressions;
using System.Diagnostics;

using NUnit.Extensions.Forms;
using NUnit.Framework;
using NUnit.Framework.Constraints;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.MFinance.Gui;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Server.App.Core;
using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;

using Ict.Testing.NUnitPetraServer;

namespace Ict.Testing.NUnitForms
{
    /// <summary>
    /// The idea of CommonNUnitFunctions is that you can replace the test inheritace of
    /// NUnitFormTest by NUnitFormTest. So you will get a set of small helpfull routines
    /// to make testing something easier.
    /// </summary>
    public class CommonNUnitFormFunctions : NUnitFormTest
    {
        /// <summary>
        /// This "string" can be used as an public property to read in the
        /// Title of the last Message box.
        /// </summary>
        public String lastMessageTitle;

        /// <summary>
        /// This "string" can be used as an public property to read in the
        /// Message of the last Message box.
        /// </summary>
        public String lastMessageText;

        /// <summary>
        /// The delegate to handle the message box is installed.
        /// </summary>
        /// <param name="cmd">Contains a NUnit.Extensions.Forms.MessageBoxTester.Command to
        /// insert the desired reaction.</param>
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