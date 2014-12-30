//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Threading;
using System.Net.Security;
using Ict.Common;

namespace Ict.Common.IO
{
    /// <summary>
    /// If TSmtpSender.SendMessage can detect that an email didn't reach its recipient
    /// (which is not always possible)
    /// It will report an array of these as FailedRecipients.
    /// </summary>
    public class TsmtpFailedRecipient
    {
        /// <summary></summary>
        public String FailedAddress;
        /// <summary></summary>
        public String FailedMessage;
    }

    /// <summary>
    /// this is a small wrapper around the .net SMTP Email services
    /// </summary>
    public class TSmtpSender
    {
        private SmtpClient FSmtpClient;

        /// <summary>
        /// After SendMessage, this array should be empty.
        /// If it is not empty, the contents must be shown to the user.
        /// </summary>
        public List <TsmtpFailedRecipient>FailedRecipients;

        /// <summary>Caller should check this after initialisation</summary>
        public Boolean FInitOk;

        /// <summary>If the Sender doesn't initialise, or mail doesn't send, this status string may give a clue why?</summary>
        public String FErrorStatus;


        private Boolean Initialise(string ASMTPHost, int ASMTPPort, bool AEnableSsl, string AUsername, string APassword, string AOutputEMLToDirectory)
        {
            try
            {
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

                    if (TAppSettingsManager.GetValue("IgnoreServerCertificateValidation", "false", false) == "true")
                    {
                        // when checking the validity of a SSL certificate, always pass
                        // this is needed for smtp.outlook365.com, since I cannot find a place to get the public key for the ssl certificate
                        ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(
                                delegate
                                { return true; }
                                );
                    }
                }

                FailedRecipients = new List <TsmtpFailedRecipient>();
                FInitOk = true;
            } // try
            catch (Exception e)
            {
                FErrorStatus = e.Message;
                FInitOk = false;
            }
            return FInitOk;
        }

        /// <summary>
        /// setup the smtp client
        /// </summary>
        public TSmtpSender(string ASMTPHost, int ASMTPPort, bool AEnableSsl, string AUsername, string APassword, string AOutputEMLToDirectory)
        {
            FErrorStatus = "";
            //Set up SMTP client
            Initialise(ASMTPHost, ASMTPPort, AEnableSsl, AUsername, APassword, AOutputEMLToDirectory);
        }

        /// <summary>
        /// setup the smtp client from the config file or command line parameters
        /// </summary>
        public TSmtpSender()
        {
            //Set up SMTP client
            String EmailDirectory = "";

            FErrorStatus = "";

            if (TAppSettingsManager.HasValue("OutputEMLToDirectory"))
            {
                EmailDirectory = TAppSettingsManager.GetValue("OutputEMLToDirectory");
            }

            Initialise(
                TAppSettingsManager.GetValue("SmtpHost"),
                TAppSettingsManager.GetInt16("SmtpPort", 25),
                TAppSettingsManager.GetBoolean("SmtpEnableSsl", false),
                TAppSettingsManager.GetValue("SmtpUser", ""),
                TAppSettingsManager.GetValue("SmtpPassword", ""),
                EmailDirectory);
        }

        /// <summary>
        /// check if smtp host has been configured
        /// </summary>
        /// <returns></returns>
        public bool ValidateEmailConfiguration()
        {
            if (FSmtpClient.DeliveryMethod == SmtpDeliveryMethod.Network)
            {
                return FSmtpClient.Host.Length > 0 && !FSmtpClient.Host.Contains("example.org");
            }
            else if (FSmtpClient.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
            {
                return Directory.Exists(FSmtpClient.PickupDirectoryLocation);
            }

            return false;
        }

        /// <summary>
        /// Use this to get all the emails copied to an address
        /// </summary>
        public String CcEverythingTo = "";

        /// <summary>
        /// If the ReplyTo address should be different, use this.
        /// </summary>
        public String ReplyTo = "";

        private Attachment FAttachedObject = null;

        /// <summary>
        /// If the attachment is not in a file, don't save it into one, use this instead:
        /// </summary>
        /// <param name="AReportText"></param>
        /// <param name="AReportName"></param>
        public void AttachFromStream(Stream AReportText, String AReportName)
        {
            FAttachedObject = new Attachment(AReportText, AReportName);
        }

        /// <summary>
        /// Create a mail message and send it
        /// </summary>
        /// <returns></returns>
        public bool SendEmail(string fromemail, string fromDisplayName, string receipients, string subject, string body,
            string[] attachfiles = null)
        {
            try
            {
                using (MailMessage email = new MailMessage())
                {
                    //From and To
                    email.Sender = new MailAddress(fromemail, fromDisplayName);
                    email.From = new MailAddress(fromemail, fromDisplayName);
                    email.To.Add(receipients);

                    if (CcEverythingTo != "")
                    {
                        email.CC.Add(new MailAddress(CcEverythingTo));
                    }

                    if (ReplyTo != "")
                    {
                        email.ReplyToList.Add(new MailAddress(ReplyTo));
                    }

                    //Subject and Body
                    email.Subject = subject;
                    email.Body = body;
                    email.IsBodyHtml = false;

                    List <Attachment>attachments = new List <Attachment>();

                    // A single attachment may have been specified using the AttachFromStream method, above.
                    if (FAttachedObject != null)
                    {
                        email.Attachments.Add(FAttachedObject);
                        attachments.Add(FAttachedObject);
                    }

                    //Attachment files if any:
                    if (attachfiles != null)
                    {
                        foreach (string attachfile in attachfiles)
                        {
                            if (System.IO.File.Exists(attachfile) == true)
                            {
                                Attachment data = new Attachment(attachfile, System.Net.Mime.MediaTypeNames.Application.Octet);
                                email.Attachments.Add(data);
                                attachments.Add(data);
                            }
                            else
                            {
                                FErrorStatus = "File to attach '" + attachfile + "' does not exist!";
                                TLogging.Log("Could not send email");
                                TLogging.Log(FErrorStatus);
                                return false;
                            }
                        }
                    }

                    bool Result = SendMessage(email);

                    foreach (Attachment data in attachments)
                    {
                        // make sure that the file is not locked any longer
                        data.Dispose();
                    }

                    FAttachedObject = null;
                    return Result;
                }
            }
            catch (Exception ex)
            {
                FErrorStatus = ex.Message;
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

            FailedRecipients.Clear();

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

                int AttemptCount = 3;

                while (AttemptCount > 0)
                {
                    AttemptCount--;
                    try
                    {
                        // for office365, this takes about 15 seconds
                        FSmtpClient.Send(AEmail);

                        AEmail.Headers.Add("Date-Sent", DateTime.Now.ToString());

                        return true;
                    }
                    catch (Exception e)
                    {
                        if (AttemptCount > 0)
                        {
                            Thread.Sleep(TimeSpan.FromMinutes(1));
                        }
                        else
                        {
                            throw e;
                        }
                    }
                }
            }
            catch (SmtpFailedRecipientsException frEx)  // If the SMTP server knows that the send failed because of failed recipients,
            {                                           // I can produce a list of failed recipient addresses, and return false.
                                                        // The caller can then retrieve the list and inform the user.
                TLogging.Log("SmtpEmail: Email to the following addresses did not succeed:");
                SmtpFailedRecipientException[] failureList = frEx.InnerExceptions;

                foreach (SmtpFailedRecipientException problem in failureList)
                {
                    TsmtpFailedRecipient FailureDetails = new TsmtpFailedRecipient();
                    FailureDetails.FailedAddress = problem.FailedRecipient;
                    FailureDetails.FailedMessage = problem.Message;
                    FailedRecipients.Add(FailureDetails);
                    TLogging.Log(problem.FailedRecipient + " : " + problem.Message);
                }

                return false;
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

            return false;
        }
    }
}