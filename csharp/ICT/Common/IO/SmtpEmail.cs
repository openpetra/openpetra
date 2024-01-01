//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2024 by OM International
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
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Security;
using System.Security;
using System.Runtime;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

using MailKit.Net.Smtp;
using MailKit;
using MailKit.Security;
using MimeKit;

using Ict.Common;
using Ict.Common.Exceptions;

#region changelog

/* Unifying and standardising system settings, especially for SMTP and email - Moray
 *
 *   Enable the parameterless constructor to be called from both client and server. The callers shouldn't need to know how to get
 *   SMTP configurations from the server; that's our responsibility. Remove redundant parameterised constructor.
 *
 *   Remove OutputEMLToDirectory after discussion with Timo (http://irclogs.openpetra.org/logfile_2016-09-02.html)
 */
#endregion

namespace Ict.Common.IO
{
    /// <summary>
    /// Delegate to fetch the SMTP server configuration.
    /// </summary>
    /// <remarks>Ultimately these settings come from Server.config. But as <see cref="TSmtpSender"/> can be
    /// instantiated from either Client or Server, we need different routes to get to them.</remarks>
    /// <returns><see cref="TSmtpServerSettings"/> struct containing the settings</returns>
    public delegate TSmtpServerSettings TGetSmtpSettings();

    /// <summary>
    /// Returns the settings to initialize SmtpSender, either to Client or Server
    /// </summary>
    [Serializable()]
    public struct TSmtpServerSettings
    {
        /// <summary>
        /// Constructor method.
        /// </summary>
        /// <param name="ASmtpHost"></param>
        /// <param name="ASmtpPort"></param>
        /// <param name="ASmtpEnableSsl"></param>
        /// <param name="ASmtpUsername"></param>
        /// <param name="ASmtpPassword"></param>
        /// <param name="ASmtpIgnoreServerCertificateValidation"></param>
#if USE_SECURESTRING
        public TSmtpServerSettings(string ASmtpHost,
            int ASmtpPort,
            bool ASmtpEnableSsl,
            string ASmtpUsername,
            System.Security.SecureString ASmtpPassword,
            bool ASmtpIgnoreServerCertificateValidation)
#else
        public TSmtpServerSettings(string ASmtpHost,
            int ASmtpPort,
            bool ASmtpEnableSsl,
            string ASmtpUsername,
            string ASmtpPassword,
            bool ASmtpIgnoreServerCertificateValidation)
#endif
        {
            FSmtpHost = ASmtpHost;
            FSmtpPort = ASmtpPort;
            FSmtpEnableSsl = ASmtpEnableSsl;
            FSmtpUsername = ASmtpUsername;
            FSmtpPassword = ASmtpPassword;
            FSmtpIgnoreServerCertificateValidation = ASmtpIgnoreServerCertificateValidation;
        }

        /// <summary/>
        public string SmtpHost {
            get
            {
                return FSmtpHost;
            } private set
            {
                FSmtpHost = value;
            }
        }
        /// <summary/>
        public int SmtpPort {
            get
            {
                return FSmtpPort;
            } private set
            {
                FSmtpPort = value;
            }
        }
        /// <summary/>
        public bool SmtpEnableSsl {
            get
            {
                return FSmtpEnableSsl;
            } private set
            {
                FSmtpEnableSsl = value;
            }
        }
        /// <summary/>
        public string SmtpUsername {
            get
            {
                return FSmtpUsername;
            } private set
            {
                FSmtpUsername = value;
            }
        }
        /// <summary/>
#if USE_SECURESTRING
        public System.Security.SecureString SmtpPassword {
            get
            {
                return FSmtpPassword;
            } private set
            {
                FSmtpPassword = value;
            }
        }
#else
        public string SmtpPassword {
            get
            {
                return FSmtpPassword;
            } private set
            {
                FSmtpPassword = value;
            }
        }
#endif
        /// <summary/>
        public bool SmtpIgnoreServerCertificateValidation {
            get
            {
                return FSmtpIgnoreServerCertificateValidation;
            } private set
            {
                FSmtpIgnoreServerCertificateValidation = value;
            }
        }

        private string FSmtpHost;
        private int FSmtpPort;
        private bool FSmtpEnableSsl;
        private string FSmtpUsername;
#if USE_SECURESTRING
        private System.Security.SecureString FSmtpPassword;
#else
        private string FSmtpPassword;
#endif
        private bool FSmtpIgnoreServerCertificateValidation;
    }

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
    /// This is a small wrapper around the .net SMTP Email services
    /// </summary>
    public class TSmtpSender : IDisposable
    {
        /// <summary>todoComment</summary>
        public static readonly String EMAIL_USER_LOGIN_NAME = "IUSROPEMAIL";
        /// <summary>The domain of the default dummy SmtpHost written to our Server.config. Used here to check whether SMTP has been configured.</summary>
        public static readonly String SMTP_HOST_DEFAULT = ".example.org";

        /// <summary>User Default code for the list of most recently used email addresses.</summary>
        public static readonly string RECENTADDRS = "EmailRecentAddresses";


        [ThreadStatic]
        static TGetSmtpSettings FGetSmtpSettings;

        private SmtpClient FSmtpClient;
        private MailboxAddress FSender;
        private InternetAddressList FReplyTo;
        private InternetAddressList FCcEverythingTo;
        private InternetAddressList FBccEverythingTo;
        private Dictionary<string, byte[]> FAttachments = new Dictionary<string, byte[]>();

        /// <summary>
        /// After SendMessage, this list should be empty.
        /// If it is not empty, the contents must be shown to the user.
        /// </summary>
        private List <TsmtpFailedRecipient>FFailedRecipients;

        /// <summary>If the mail doesn't send, this status string may give a clue why?</summary>
        private String FErrorStatus;

        /// <summary>
        /// Client or Server Delegate used to get the SMTP server settings.
        /// </summary>
        public static TGetSmtpSettings GetSmtpSettings
        {
            get
            {
                return FGetSmtpSettings;
            }
            set
            {
                FGetSmtpSettings = value;
            }
        }

        /// Method to obtain the SMTP email server configuration settings from the configuration file
        public static TSmtpServerSettings GetSmtpSettingsFromAppSettings()
        {
            return new TSmtpServerSettings(
                TAppSettingsManager.GetValue("SmtpHost"),
                TAppSettingsManager.GetInt16("SmtpPort"),
                TAppSettingsManager.GetBoolean("SmtpEnableSsl", true),
                TAppSettingsManager.GetValue("SmtpUser"),
                TAppSettingsManager.GetValue("SmtpPassword"),
                TAppSettingsManager.GetBoolean("IgnoreServerCertificateValidation", false));
        }

        /// <summary>
        /// Returns the sender address for use if constructing a MimeMessage outside this class.
        /// </summary>
        public MailboxAddress Sender
        {
            get
            {
                return FSender;
            }
        }

        /// <summary>
        /// Static method that can be called from anywhere to validate an email address.
        /// </summary>
        /// <param name="field"></param>
        /// <returns>True if the string forms a valid email address; false otherwise.</returns>
        public static bool ValidateEmailAddress(string field)
        {
            return new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(field);
        }

        /// seems that MimeKit MailboxAddress is not able to parse the name and the address
        private static MailboxAddress StringToMailboxAddress(string AAddressWithName)
        {
            if (AAddressWithName.IndexOf('<') != -1)
            {
                string address = AAddressWithName.Substring(
                    AAddressWithName.IndexOf('<') + 1,
                    AAddressWithName.IndexOf('>') - AAddressWithName.IndexOf('<') - 1).Trim();
                string name = AAddressWithName.Substring(0, AAddressWithName.IndexOf('<')).Trim().Trim(new char[]{'"'});

                //TLogging.Log(name + "     " + address);

                return new MailboxAddress(name, address);
            }
            else
            {
                return MailboxAddress.Parse(AAddressWithName.Trim());
            }
        }

        /// <summary>
        /// Convert a semicolon-separated list of email addresses to a list of email address objects.
        /// </summary>
        /// <remarks>
        /// Where OpenPetra has multiple addresses stored in one database field, most are separated by semicolons because by
        /// default Microsoft Outlook requires semicolons to separate addresses (https://blogs.msdn.microsoft.com/oldnewthing/20150119-00/?p=44883).
        /// But the Internet, including .NET's MailMessage.To.Add(string) method, requires commas (https://tools.ietf.org/html/rfc5322#section-3.4).
        /// So when pulling "To" addresses from the database we must parse them for unquoted semicolons and convert them to commas.
        /// </remarks>
        /// <param name="AList"></param>
        /// <returns></returns>
        public static List<MailboxAddress> ConvertAddressList(string AList)
        {
            var InQuote = false;
            var chars = AList.ToCharArray();

            List<MailboxAddress> result = new List<MailboxAddress>();

            string nextAddress = String.Empty;

            for (int i = 0; i < chars.Length; i++)
            {
                var ch = chars[i];

                switch (ch)
                {
                    case '\\':
                        // The next character is escaped, so don't treat it as a control character.
                        i++;
                        nextAddress += chars[i];
                        break;

                    case '"':
                        // A quoted-string is delimited by DQUOTE, ASCII 34, only.
                        InQuote = !InQuote;
                        nextAddress += ch;
                        break;

                    case ';':
                    case ',':

                        // An unquoted semicolon must be a mailbox-list separator; .NET does not support the
                        // group syntax of https://tools.ietf.org/html/rfc5322#section-3.2.4 which is the
                        // only valid use of an unquoted semicolon in an address specification.
                        if (!InQuote)
                        {
                            result.Add(StringToMailboxAddress(nextAddress));
                            nextAddress = String.Empty;
                        }
                        else
                        {
                            nextAddress += ch;
                        }

                        break;

                    default:
                        nextAddress += ch;
                        break;
                }
            }

            result.Add(StringToMailboxAddress(nextAddress));

            return result;
        }

        /// <summary>
        /// After SendMessage, this contains a list of any recipients we failed to send to.
        /// </summary>
        public List <TsmtpFailedRecipient>FailedRecipients
        {
            get
            {
                return FFailedRecipients;
            }
        }

        /// <summary>
        /// An indication of any errors that occurred during SendMessage.
        /// </summary>
        public string ErrorStatus
        {
            get
            {
                return FErrorStatus;
            }
        }

        /// <summary>
        /// Set up the SMTP client
        /// </summary>
        public TSmtpSender()
        {
            FErrorStatus = "";

            if (GetSmtpSettings == null)
            {
                throw new ESmtpSenderInitializeException("Delegate GetSmtpSettings not assigned.");
            }

            try
            {
                /* SmtpClient can throw:
                 *   ArgumentNullException          - if Host is set to null
                 *   ArgumentException              - if Host is set to an empty string
                 *   ArgumentOutOfRangeException    - if Port is out of range
                 */
                var SmtpSettings = GetSmtpSettings();

                FSmtpClient = new SmtpClient();

                if (SmtpSettings.SmtpIgnoreServerCertificateValidation)
                {
                    // When checking the validity of a SSL certificate, always pass.
                    // This is needed for smtp.outlook365.com, since I cannot find a place to get the public key for the ssl certificate.
                    FSmtpClient.ServerCertificateValidationCallback = (s,c,h,e) => true;
                }

                FSmtpClient.Connect(SmtpSettings.SmtpHost, SmtpSettings.SmtpPort, SecureSocketOptions.Auto);
                FSmtpClient.Authenticate(SmtpSettings.SmtpUsername, SmtpSettings.SmtpPassword);

                FFailedRecipients = new List <TsmtpFailedRecipient>();
            } // try
            catch (Exception e)
            {
                if (FSmtpClient != null)
                {
                    FSmtpClient.Dispose();
                }

                throw new ESmtpSenderInitializeException(Catalog.GetString(
                        "SMTP Sender failed to initialize. Ask your System Administrator to check the settings in the OpenPetra Server.config file."),
                    e,
                    TSmtpErrorClassEnum.secServer);
            }
        }

        /// <summary>
        /// Sets the sender address for the email - or set of emails.
        /// </summary>
        /// <param name="AAddress"></param>
        /// <param name="ADisplayName"></param>
        public void SetSender(string AAddress, string ADisplayName)
        {
            try
            {
                // there seems to be an issue with older Mono (tested with 4.6).
                // MimeKit.Utils.CharsetUtils.AddAliases crashes.
                // See: https://github.com/jstedfast/MailKit/issues/617
                // and https://github.com/jstedfast/MimeKit/issues/388
                // Upgrading to Mono >= 5.10 solves the issue
                FSender = new MailboxAddress(ADisplayName, AAddress);
            }
            catch (Exception e)
            {
                throw new ESmtpSenderInitializeException(String.Format("Invalid sender address '{0} <{1}>'.",
                        ADisplayName,
                        AAddress), e, TSmtpErrorClassEnum.secClient);
            }
        }

        /// add an attachment with a different name to avoid temp names in the email
        public void AddAttachment(string ADisplayFileName, string APhysicalFileName)
        {
            FAttachments.Add(ADisplayFileName, File.ReadAllBytes(APhysicalFileName));
        }

        /// <summary>
        /// Use this to get all the emails copied to an address
        /// </summary>
        public String CcEverythingTo
        {
            set
            {
                if (value == "")
                {
                    FCcEverythingTo = null;
                }
                else
                {
                    try
                    {
                        FCcEverythingTo = new InternetAddressList();
                        List<MailboxAddress> list = ConvertAddressList(value);
                        foreach (MailboxAddress addr in list)
                        {
                            FCcEverythingTo.Add(addr);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ESmtpSenderInitializeException(String.Format("Invalid CC address '{0}'.", value), e, TSmtpErrorClassEnum.secClient);
                    }
                }
            }
        }

        /// <summary>
        /// Use this to get all the emails blind copied to an address
        /// </summary>
        public String BccEverythingTo
        {
            set
            {
                if (value == "")
                {
                    FBccEverythingTo = null;
                }
                else
                {
                    try
                    {
                        FBccEverythingTo = new InternetAddressList();
                        List<MailboxAddress> list = ConvertAddressList(value);
                        foreach (MailboxAddress addr in list)
                        {
                            FBccEverythingTo.Add(addr);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ESmtpSenderInitializeException(String.Format("Invalid BCC address '{0}'.", value), e, TSmtpErrorClassEnum.secClient);
                    }
                }
            }
        }

        /// <summary>
        /// If the ReplyTo address should be different, use this.
        /// </summary>
        public String ReplyTo
        {
            set
            {
                if (value == "")
                {
                    FReplyTo = null;
                }
                else
                {
                    try
                    {
                        FReplyTo = new InternetAddressList();
                        List<MailboxAddress> list = ConvertAddressList(value);
                        foreach (MailboxAddress addr in list)
                        {
                            FReplyTo.Add(addr);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ESmtpSenderInitializeException(String.Format("Invalid Reply-To address '{0}'.",
                                value), e, TSmtpErrorClassEnum.secClient);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a new MimeMessage with From, Reply-To and CC addresses set from the values in User Defaults.
        /// </summary>
        /// <returns>A MimeMessage</returns>
        /// <exception cref="ESmtpSenderInitializeException">Thrown if the sender address has not been set.</exception>
        public MimeMessage GetNewMimeMessage()
        {
            if (FSender == null)
            {
                throw new ESmtpSenderInitializeException("Sender address has not been set.", TSmtpErrorClassEnum.secClient);
            }

            var NewMessage = new MimeMessage();

            //Settings from User Defaults: From, Copy and Reply
            NewMessage.From.Add(FSender);

            if (FCcEverythingTo != null)
            {
                foreach (var addr in FCcEverythingTo)
                {
                    NewMessage.Cc.Add(addr);
                }
            }

            if (FBccEverythingTo != null)
            {
                foreach (var addr in FBccEverythingTo)
                {
                    NewMessage.Bcc.Add(addr);
                }
            }

            if (FReplyTo != null)
            {
                foreach (var addr in FReplyTo)
                {
                    NewMessage.ReplyTo.Add(addr);
                }
            }

            return NewMessage;
        }

        /// <summary>
        /// Create a mail message and send it as from the specified sender address.
        /// </summary>
        /// <returns>Flag indicating whether email was sent.</returns>
        /// <exception cref="ESmtpSenderInitializeException">Thrown when the sender address is invalid. An inner exception may contain more detail.</exception>
        public bool SendEmail(string fromemail, string fromDisplayName, string recipients, string subject, string body,
            string[] attachfiles = null)
        {
            SetSender(fromemail, fromDisplayName);
            return SendEmail(recipients, subject, body, attachfiles);
        }

        /// <summary>
        /// Create a mail message from a language specific template and send it as from the specified sender address.
        /// The first line in the template is the subject, all following lines are the body.
        /// </summary>
        /// <returns>Flag indicating whether email was sent.</returns>
        /// <exception cref="ESmtpSenderInitializeException">Thrown when the sender address is invalid. An inner exception may contain more detail.</exception>
        public bool SendEmailFromTemplate(string fromemail, string fromDisplayName, string recipients,
            string template, string language, Dictionary<string, string> parameters,
            string[] attachfiles = null)
        {
            SetSender(fromemail, fromDisplayName);

            string TemplateFilename = TAppSettingsManager.GetValue("EMailTemplates.Path", ".") +
                           Path.DirectorySeparatorChar +
                           template + "_" + language.ToLower() + ".txt";

            if (!File.Exists(TemplateFilename) && language.Contains("-"))
            {
                TemplateFilename = TAppSettingsManager.GetValue("EMailTemplates.Path", ".") +
                           Path.DirectorySeparatorChar +
                           template + "_" + language.ToLower().Substring(0,language.IndexOf('-')) + ".txt";
            }

            if (!File.Exists(TemplateFilename))
            {
                TemplateFilename = TAppSettingsManager.GetValue("EMailTemplates.Path", ".") +
                           Path.DirectorySeparatorChar +
                           template + "_en.txt";
            }

            string subject = String.Empty;
            string body = String.Empty;

            using (StreamReader reader = new StreamReader(TemplateFilename))
            {
                if (reader == null)
                {
                    throw new Exception("cannot open file " + TemplateFilename);
                }

                subject = reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    body += line + Environment.NewLine;
                }
            }

            foreach (var pair in parameters)
            {
                subject = subject.Replace("{" + pair.Key + "}", pair.Value);
                body = body.Replace("{" + pair.Key + "}", pair.Value);
                body = body.Replace("{ifdef " + pair.Key + "}" + Environment.NewLine, "");
                body = body.Replace("{endif " + pair.Key + "}" + Environment.NewLine, "");
            }

            // drop all {ifdef var}...{endif var} if var does not exist in the parameters list
            int indexIfDef = -1;
            while ((indexIfDef = body.IndexOf("{ifdef ")) >= 0)
            {
                string tmp = body.Substring(indexIfDef + "{ifdef ".Length);
                int indexIfDefClosingBracket = tmp.IndexOf("}");
                if (indexIfDefClosingBracket < 0)
                {
                    throw new Exception("SendEmailFromTemplate: " + template + ": cannot determine key from ifdef");
                }
                string key = tmp.Substring(0, indexIfDefClosingBracket);
                int indexEnd = body.IndexOf("{endif " + key + "}" + Environment.NewLine);
                if (indexEnd < 0)
                {
                    throw new Exception("SendEmailFromTemplate: " + template + ": missing endif for key " + key);
                }
                body = body.Substring(0, indexIfDef) + body.Substring(indexEnd + ("{endif " + key + "}" + Environment.NewLine).Length);
            }

            if ((subject.Length == 0) || (body.Length == 0))
            {
                return false;
            }

            return SendEmail(recipients, subject, body, attachfiles);
        }

        /// <summary>
        /// Create a mail message and send it as from the address that has already been set.
        /// </summary>
        /// <remarks>The sender address must be set using <see cref="SetSender(string, string)"/></remarks>
        /// <returns>Flag indicating whether email was sent. Check the <see cref="ErrorStatus"/> property for further details.</returns>
        /// <exception cref="ESmtpSenderInitializeException">Thrown when the sender address is invalid. An inner exception may contain detail.</exception>
        public bool SendEmail(string recipients, string subject, string body,
            string[] attachfiles = null)
        {
            try
            {
                // Initialize a new MimeMessage with settings from User Defaults
                MimeMessage email = GetNewMimeMessage();

                //To
                List<MailboxAddress> list = ConvertAddressList(recipients);
                foreach (MailboxAddress addr in list)
                {
                    email.To.Add(addr);
                }

                //Subject and Body
                email.Subject = subject;
                
                var builder = new BodyBuilder ();
                builder.TextBody = body;

                //Attachment files if any:
                if (attachfiles != null)
                {
                    foreach (string attachfile in attachfiles)
                    {
                        if (System.IO.File.Exists(attachfile) == true)
                        {
                            builder.Attachments.Add(attachfile);
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
                else
                {
                    foreach (string filename in FAttachments.Keys)
                    {
                        builder.Attachments.Add(filename, FAttachments[filename]);
                    }
                }

                email.Body = builder.ToMessageBody ();

                return SendMessage(email);
            }
            catch (Exception ex)
            {
                FErrorStatus = ex.Message;
                TLogging.Log("Could not send email");
                TLogging.Log(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Send an email message
        /// </summary>
        /// <param name="AEmail">On successful sending, the header is modified with the sent date.</param>
        /// <returns>True if email was sent successfully.</returns>
        public bool SendMessage(MimeMessage AEmail)
        {
            if (AEmail.Headers["Date-Sent"] != null)
            {
                // don't send emails several times
                return false;
            }

            FFailedRecipients.Clear();

            //Attempt to send the email
            // FIXME!! Some SMTP servers have a message rate limit, so if a message is rejected because we have exceeded the maximum number
            // of messages per minute, we need to wait and retry (https://tracker.openpetra.org/view.php?id=3179). Unfortunately the solution
            // here doesn't check what _kind_ of error occurred. So for a permanent error like incorrect credentials... for 100 HOSAs... it
            // will happily retry one after another for 5 hours before telling the user they all failed.
            //
            // You can catch the SmtpClient exceptions and check the StatusCode enum to get a better idea of the problem. The numeric values of
            // the enum match SMTP error codes as shown below. Codes in the 400 range are generally 'temporary' failures while codes in the 500
            // range are usually'permanent'. It's more complicated than it should be because:
            //    i) If a MimeMessage has one To: address, failure is returned in a SmtpFailedRecipientException.
            //       If a MimeMessage has one To: address and one CC: address, failure is returned in a SmtpFailedRecipientsException (note the plural).
            //       This contains an InnerExceptions property containing the individual SmtpFailedRecipientExceptions which Microsoft say is "not
            //       intended to be used directly from your code".
            //   ii) There are different kinds of "permanent" errors and SMTP servers aren't consistent about how they report things. For example,
            //       for an authentication error, mail.smtp2go.com returns "Mailbox Unavailable. The server response was: Relay denied for unauthenticated sender".
            //       You need to check the exception Message text to see whether the error is to do with the recipient mailbox (so mail to a
            //       different recipient may work) or the sender account (so nothing will work and you may as well abort the whole process now).
            //
            // SmtpStatusCode Enumeration with numeric values:
            //    -1      GeneralFailure
            //   211     SystemStatus
            //   214     HelpMessage
            //   220     ServiceReady
            //   221     ServiceClosingTransmissionChannel
            //   250     Ok
            //   251     UserNotLocalWillForward
            //   252     CannotVerifyUserWillAttemptDelivery
            //   354     StartMailInput
            //   421     ServiceNotAvailable
            //   450     MailboxBusy
            //   451     LocalErrorInProcessing
            //   452     InsufficientStorage
            //   454     ClientNotPermitted
            //   500     CommandUnrecognized
            //   501     SyntaxError
            //   502     CommandNotImplemented
            //   503     BadCommandSequence
            //   504     CommandParameterNotImplemented
            //   530     MustIssueStartTlsFirst
            //   550     MailboxUnavailable
            //   551     UserNotLocalTryAlternatePath
            //   552     ExceededStorageAllocation
            //   553     MailboxNameNotAllowed
            //   554     TransactionFailed

            try
            {
                int AttemptCount = 3;

                while (AttemptCount > 0)
                {
                    AttemptCount--;
                    try
                    {
                        TLogging.LogAtLevel(1, "Trying to send E-Mail to " +
                                            AEmail.To.ToString() + " from " + AEmail.From.ToString());

                        if (!AEmail.From.ToString().Substring(AEmail.From.ToString().IndexOf("@")).Contains("."))
                        {
                            // invalid Email domain, eg. local
                            return false;
                        }

                        // for office365, this takes about 15 seconds
                        FSmtpClient.Send(AEmail);

                        AEmail.Headers.Add("Date-Sent", DateTime.Now.ToString());

                        TLogging.LogAtLevel(1, "E-Mail was sent successfully");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (AttemptCount > 0)
                        {
                            Thread.Sleep(TimeSpan.FromMinutes(1));
                        }
                        else
                        {
                            TLogging.LogException(ex, Utilities.GetMethodSignature());
                            throw;
                        }
                    }
                }
            }
            catch (SmtpCommandException frEx)  // If the SMTP server knows that the send failed because of failed recipients,
            {                                           // I can produce a list of failed recipient addresses, and return false.
                                                        // The caller can then retrieve the list and inform the user.
                TLogging.Log("SmtpEmail: Email to the following addresses did not succeed:");

                if (frEx.ErrorCode == SmtpErrorCode.RecipientNotAccepted)
                {
                    TsmtpFailedRecipient FailureDetails = new TsmtpFailedRecipient();
                    FailureDetails.FailedAddress = frEx.Mailbox.ToString();
                    FailureDetails.FailedMessage = "Recipient not accepted";
                    FFailedRecipients.Add(FailureDetails);
                    TLogging.Log(FailureDetails.FailedAddress + " : " + FailureDetails.FailedMessage);
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
                TLogging.Log(ex.ToString() + " " + ex.Message);
                TLogging.Log(ex.StackTrace);

                throw;
            }

            return false;
        }

        /// <summary>
        /// Dispose of the IDisposable objects used by this object.
        /// </summary>
        public void Dispose()
        {
            if (FSmtpClient != null)
            {
                FSmtpClient.Dispose();
            }
        }
    }

    /// <summary>
    /// Used to distinguish server-config exceptions from client-specified exceptions.
    /// </summary>
    public enum TSmtpErrorClassEnum
    {
        /// <summary>
        /// Default value
        /// </summary>
        secUnspecified,

        /// <summary>
        /// Error came from server configuration. Exception message should already include "Ask your System Administrator to check the settings in the OpenPetra Server.config file."
        /// </summary>
        secServer,

        /// <summary>
        /// Error cause by client-provided data. Use this value to add a helpful hint to the message appropriate to the client settings. Something like "Check the Email tab in User Settings >> Preferences."
        /// </summary>
        secClient
    }

    /// <summary>
    /// Generic emailer exception
    /// </summary>
    [Serializable()]
    public class ESmtpSenderException : EOPAppException
    {
        // Copy serialization constructors from C:\OpenPetra\dev_20160805\csharp\ICT\Common\Exceptions.Remoted.cs ?

        /// <summary>
        /// Creates a new instance of this exception.
        /// </summary>
        public ESmtpSenderException() : base()
        {
        }

        /// <summary>
        /// Creates a new instance of this exception with the specified message.
        /// </summary>
        /// <param name="AMessage">The message that describes the error.</param>
        public ESmtpSenderException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Creates a new instance of this exception with the specified message and reference to the inner exception.
        /// </summary>
        /// <param name="AMessage">The message that describes the error.</param>
        /// <param name="AInnerException">The exception that caused this exception.</param>
        public ESmtpSenderException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    /// <summary>
    /// Exception for errors that occur during initialization
    /// </summary>
    [Serializable()]
    public class ESmtpSenderInitializeException : ESmtpSenderException
    {
        /// <summary>
        /// Indicates whether the exception was caused by server configuration data, or client-supplied data.
        /// </summary>
        public TSmtpErrorClassEnum ErrorClass;

        /// <summary>
        /// Creates a new instance of this exception.
        /// </summary>
        public ESmtpSenderInitializeException() : base()
        {
            this.ErrorClass = TSmtpErrorClassEnum.secUnspecified;
        }

        /// <summary>
        /// Creates a new instance of this exception with the specified message.
        /// </summary>
        /// <param name="AMessage">The message that describes the error.</param>
        /// <param name="ErrorClass">Field specifying whether the exception was caused by server configuration data, or client-supplied data. Default TSmtpErrorClassEnum.secUnspecified</param>
        public ESmtpSenderInitializeException(String AMessage, TSmtpErrorClassEnum ErrorClass = TSmtpErrorClassEnum.secUnspecified) : base(AMessage)
        {
            this.ErrorClass = ErrorClass;
        }

        /// <summary>
        /// Creates a new instance of this exception with the specified message and reference to the inner exception.
        /// </summary>
        /// <param name="AMessage">The message that describes the error.</param>
        /// <param name="AInnerException">The exception that caused this exception.</param>
        /// <param name="ErrorClass">Field specifying whether the exception was caused by server configuration data, or client-supplied data. Default TSmtpErrorClassEnum.secUnspecified</param>
        public ESmtpSenderInitializeException(string AMessage,
            Exception AInnerException,
            TSmtpErrorClassEnum ErrorClass = TSmtpErrorClassEnum.secUnspecified) : base(AMessage, AInnerException)
        {
            this.ErrorClass = ErrorClass;
        }
    }

    /// <summary>
    /// Exception for errors that occur during sending
    /// </summary>
    [Serializable()]
    public class ESmtpSenderSendException : ESmtpSenderException
    {
        /// <summary>
        /// Creates a new instance of this exception.
        /// </summary>
        public ESmtpSenderSendException() : base()
        {
        }

        /// <summary>
        /// Creates a new instance of this exception with the specified message.
        /// </summary>
        /// <param name="AMessage">The message that describes the error.</param>
        public ESmtpSenderSendException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Creates a new instance of this exception with the specified message and reference to the inner exception.
        /// </summary>
        /// <param name="AMessage">The message that describes the error.</param>
        /// <param name="AInnerException">The exception that caused this exception.</param>
        public ESmtpSenderSendException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }
}
