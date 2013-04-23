//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using Ict.Common;

namespace Ict.Common.IO
{
    /// <summary>
    /// this is a small wrapper around the .net SMTP Email services
    /// </summary>
    public class TSmtpSender
    {
        private SmtpClient FSmtpClient;

        /// <summary>
        /// setup the smtp client
        /// </summary>
        public TSmtpSender(string ASMTPHost, int ASMTPPort, bool AEnableSsl, string AUsername, string APassword, string AOutputEMLToDirectory)
        {
            //Set up SMTP client
            FSmtpClient = new SmtpClient();

            if (AOutputEMLToDirectory.Length > 0)
            {
                FSmtpClient.PickupDirectoryLocation = AOutputEMLToDirectory;
                FSmtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            }
            else
            {
                FSmtpClient.Host = ASMTPHost;
                FSmtpClient.Port = ASMTPPort;
                FSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                FSmtpClient.UseDefaultCredentials = false;
                FSmtpClient.Credentials = new NetworkCredential(AUsername, APassword);
                FSmtpClient.EnableSsl = AEnableSsl;
            }
        }

        /// <summary>
        /// setup the smtp client from the config file or command line parameters
        /// </summary>
        public TSmtpSender()
        {
            //Set up SMTP client
            FSmtpClient = new SmtpClient();

            if (TAppSettingsManager.HasValue("OutputEMLToDirectory"))
            {
                FSmtpClient.PickupDirectoryLocation = TAppSettingsManager.GetValue("OutputEMLToDirectory");
                FSmtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            }
            else
            {
                FSmtpClient.Host = TAppSettingsManager.GetValue("SmtpHost");
                FSmtpClient.Port = TAppSettingsManager.GetInt16("SmtpPort", 25);
                FSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                FSmtpClient.Credentials = new NetworkCredential(
                    TAppSettingsManager.GetValue("SmtpUser", ""),
                    TAppSettingsManager.GetValue("SmtpPassword", ""));
                FSmtpClient.EnableSsl = TAppSettingsManager.GetBoolean("SmtpEnableSsl", false);
            }
        }

        /// <summary>
        /// create a mail message and send it
        /// </summary>
        /// <returns></returns>
        public bool SendEmail(string fromemail, string fromDisplayName, string receipients, string subject, string body, string attachfile)
        {
            try
            {
                using (MailMessage email = new MailMessage())
                {
                    //From and To
                    email.Sender = new MailAddress(fromemail, fromDisplayName);
                    email.From = new MailAddress(fromemail, fromDisplayName);
                    email.To.Add(receipients);

                    //Subject and Body
                    email.Subject = subject;
                    email.Body = body;
                    email.IsBodyHtml = false;

                    Attachment data = null;

                    //Attachement if any:
                    if ((attachfile != null) && (attachfile.Length > 0))
                    {
                        if (System.IO.File.Exists(attachfile) == true)
                        {
                            data = new Attachment(attachfile, System.Net.Mime.MediaTypeNames.Application.Octet);
                            email.Attachments.Add(data);
                        }
                        else
                        {
                            TLogging.Log("Could not send email");
                            TLogging.Log("File to attach '" + attachfile + "' does not exist!");
                            return false;
                        }
                    }

                    bool Result = SendMessage(email);

                    if (data != null)
                    {
                        // make sure that the file is not locked any longer
                        data.Dispose();
                    }

                    return Result;
                }
            }
            catch (Exception ex)
            {
                TLogging.Log("Could not send email");
                TLogging.Log(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Send an email message
        /// </summary>
        /// <param name="AEmail">on successful sending, the header is modified with the sent date</param>
        /// <returns>true if email was sent successfully</returns>
        public bool SendMessage(MailMessage AEmail)
        {
            if (AEmail.Headers.Get("Date-Sent") != null)
            {
                // don't send emails several times
                return false;
            }

            if (FSmtpClient.Host.EndsWith("example.org"))
            {
                TLogging.Log("Not sending the email, since the configuration is just with an example server: " + FSmtpClient.Host);
                TLogging.Log("You can configure the mail settings in the config file.");
                return false;
            }

            //Attempt to send the email
            try
            {
                AEmail.IsBodyHtml = AEmail.Body.ToLower().Contains("<html>");

                FSmtpClient.Send(AEmail);

                AEmail.Headers.Add("Date-Sent", DateTime.Now.ToString());
                return true;
            }
            catch (Exception ex)
            {
                // SSL authentication error: RemoteCertificateNotAvailable
                // see http://mono.1490590.n4.nabble.com/SSL-authentication-error-RemoteCertificateNotAvailable-RemoteCertificateChainErrors-td1755733.html
                // and http://www.mono-project.com/FAQ:_Security#Does_SSL_works_for_SMTP.2C_like_GMail_.3F
                // on Mono command prompt:
                //    mozroots --import --ask-remove --machine
                //    mozroots --import --ask-remove
                //    certmgr -ssl smtps://tim00.hostsharing.net:443

                TLogging.Log("There has been a problem sending the email");
                TLogging.Log("server: " + FSmtpClient.Host + ":" + FSmtpClient.Port.ToString());
                TLogging.Log(ex.ToString() + " " + ex.Message);
                TLogging.Log(ex.StackTrace);

                throw;
            }
        }
    }
}