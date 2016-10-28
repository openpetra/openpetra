//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Net;
using System.Net.Mail;
using System.Net.Security;

namespace Ict.Testing.Common.SmtpEmail
{
	public class Test
	{
        	public static void Main() {
                	try
	                {
        	                SmtpClient s = new SmtpClient();
                	        s.Host = "tim00.hostsharing.net";
                        	s.Port = 587;
	                        s.EnableSsl = true;
        	                s.Credentials = new NetworkCredential("tim00-meintest", "secret");

                	        MailMessage m = new MailMessage();
                        	m.Sender = new MailAddress("test@solidcharity.com", "Max Mustermann");
	                        m.From = m.Sender;
        	                m.To.Add("test2@solidcharity.com");
                	        m.Subject = "test";
	                        m.Body = "testbody";

				// SSL authentication error: RemoteCertificateNotAvailable
				// see http://mono.1490590.n4.nabble.com/SSL-authentication-error-RemoteCertificateNotAvailable-RemoteCertificateChainErrors-td1755733.html
				// and http://www.mono-project.com/FAQ:_Security#Does_SSL_works_for_SMTP.2C_like_GMail_.3F
				// on Mono command prompt:
				//    mozroots --import --ask-remove --machine
				//    mozroots --import --ask-remove
				//    certmgr -ssl smtps://tim00.hostsharing.net:443

        	                bool DontValidateCertificate = false;
                	        if (DontValidateCertificate)
                        	{
        	                        ServicePointManager.ServerCertificateValidationCallback =
	                                        new RemoteCertificateValidationCallback(
                	                                delegate { return true; });
                        	}

	                        s.Send(m);
        	        }
                	catch (Exception e)
	                {
        	                Console.WriteLine("Error! " + e.ToString());
                	}
	        }
	}
}

