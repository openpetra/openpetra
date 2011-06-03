//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timh
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
//
using System;
using Ict.Common;
using Ict.Common.Remoting.Server;
using System.Net.Mail;

namespace Ict.Common.EMailing
{
    /// <summary>
    /// Enables the server to send email
    /// </summary>
    ///
    public class SMTPEmail
    {
        /// <summary>
        /// Send an email to one or many recipients with a file attachment
        /// </summary>
        /// <param name="fromemail">The email address of the sender</param>
        /// <param name="fromDisplayName">The Displayed text of the senders name</param>
        /// <param name="receipients">Comman seperated list of email recipients</param>
        /// <param name="subject">The Subject of the email</param>
        /// <param name="body">The plain text body of the email</param>
        /// <param name="attachfile">A filename to attach or null</param>
        /// <returns>True if no errors occured delivering email to smtp server</returns>
        public static bool SendEmail(string fromemail, string fromDisplayName, string receipients, string subject, string body, string attachfile)
        {
            bool result = false;

            try
            {
                MailMessage email = new MailMessage();

                //From and To
                email.Sender = new MailAddress(fromemail, fromDisplayName);
                email.From = new MailAddress(fromemail, fromDisplayName);
                email.To.Add(receipients);

                //Subject and Body
                email.Subject = subject;
                email.Body = body;
                email.IsBodyHtml = false;

                //Attachement if any:

                if (attachfile != null)
                {
                    if (attachfile.Length > 0)
                    {
                        if (System.IO.File.Exists(attachfile) == true)
                        {
                            Attachment data = new Attachment(attachfile, System.Net.Mime.MediaTypeNames.Application.Octet);

                            email.Attachments.Add(data);
                        }
                        else
                        {
                            TLogging.Log("Could not send email");
                            TLogging.Log("File to attach '" + attachfile + "' does not exist!");
                            return false;
                        }
                    }
                }

                SmtpClient smtpclient = new SmtpClient();
                smtpclient.Host = TSrvSetting.SMTPServer;
                smtpclient.Send(email);

                result = true;
            }
            catch (Exception ex)
            {
                TLogging.Log("Could not send email");
                TLogging.Log(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Send an email to one or many recipients with a file attachment
        /// </summary>
        /// <param name="fromemail">The email address of the sender</param>
        /// <param name="fromDisplayName">The Displayed text of the senders name</param>
        /// <param name="receipients">Comman seperated list of email recipients</param>
        /// <param name="subject">The Subject of the email</param>
        /// <param name="body">The plain text body of the email</param>
        /// <returns>True if no errors occured deliveruing email to smtp server</returns>

        public static bool SendEmail(string fromemail, string fromDisplayName, string receipients, string subject, string body)
        {
            return SendEmail(fromemail, fromDisplayName, receipients, subject, body, null);
        }
    }
}