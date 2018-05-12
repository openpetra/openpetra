//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2018 by OM International
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
using System.Runtime.InteropServices;
using NUnit.Framework;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.Xml;
using System.Text;
using System.IO;
using System.Data;

using MimeKit;

using Ict.Common;
using Ict.Common.IO;

namespace Ict.Common.IO.Testing
{
    ///  This is a test for sending e-mails
    [TestFixture]
    public class TTestSmtpSender
    {
        /// init
        [SetUp]
        public void Init()
        {
            new TLogging("../../log/test.log");
            new TAppSettingsManager("../../etc/TestServer.config");
            TLogging.DebugLevel = TAppSettingsManager.GetInt16("Client.DebugLevel", 0);
        }

        /// test the TSmtpSender
        [Test]
        public void TestSendMail()
        {
            StringAssert.DoesNotEndWith(TSmtpSender.SMTP_HOST_DEFAULT, TAppSettingsManager.GetValue("SmtpHost"),
                "need to configure SmptHost in the config file");

            TSmtpSender.GetSmtpSettings = @TSmtpSender.GetSmtpSettingsFromAppSettings;

            TSmtpSender sender = new TSmtpSender();
            Assert.AreEqual(true, sender.SendEmail(
                "<no-reply@test.openpetra.org>",
                "OpenPetra Test",
                "test.dummy@openpetra.org",
                "This is a test subject",
                "Please ignore this e-mail!"),
                "SendEmail should return true");
        }
    }
}
