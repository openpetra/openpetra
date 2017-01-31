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
using System.Text;
using System.Threading;
using System.Net.Security;
using Ict.Common;
using System.Security;
using Ict.Common.Exceptions;
using System.Runtime.Serialization;

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


        static TGetSmtpSettings FGetSmtpSettings;

        private SmtpClient FSmtpClient;
        private MailAddress FSender;
        private MailAddressCollection FReplyTo;
        private MailAddressCollection FCcEverythingTo;

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

        /// <summary>
        /// Returns the sender address for use if constructing a MailMessage outside this class.
        /// </summary>
        public MailAddress Sender
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
            // In .NET 4.5: return new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(field.Text);
            try
            {
                var TestAddress = new System.Net.Mail.MailAddress(field);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Convert a semicolon-separated list of email addresses to a comma-separated list of email addresses.
        /// </summary>
        /// <remarks>
        /// Where OpenPetra has multiple addresses stored in one database field, most are separated by semicolons because by
        /// default Microsoft Outlook requiers semicolons to separate addresses (https://blogs.msdn.microsoft.com/oldnewthing/20150119-00/?p=44883).
        /// But the Internet, including .NET's <see cref="MailMessage.To"/>.Add(string) method, requires commas (https://tools.ietf.org/html/rfc5322#section-3.4).
        /// So when pulling "To" addresses from the database we must parse them for unquoted semicolons and convert them to commas.
        /// </remarks>
        /// <param name="AList"></param>
        /// <returns></returns>
        public static string ConvertAddressList(string AList)
        {
            var InQuote = false;
            var chars = AList.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                var ch = chars[i];

                switch (ch)
                {
                    case '\\':
                        // The next character is escaped, so skip it.
                        i++;
                        break;

                    case '"':
                        // A quoted-string is delimited by DQUOTE, ASCII 34, only.
                        InQuote = !InQuote;
                        break;

                    case ';':

                        // An unquoted semicolon must be a mailbox-list separator; .NET does not support the
                        // group syntax of https://tools.ietf.org/html/rfc5322#section-3.2.4 which is the
                        // only valid use of an unquoted semicolon in an address specification.
                        if (!InQuote)
                        {
                            chars[i] = ',';
                        }

                        break;

                    default:
                        break;
                }
            }

            return new string(chars);
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

                FSmtpClient.Host = SmtpSettings.SmtpHost;
                FSmtpClient.Port = SmtpSettings.SmtpPort;
                FSmtpClient.EnableSsl = SmtpSettings.SmtpEnableSsl;
                FSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                FSmtpClient.Credentials = new NetworkCredential(SmtpSettings.SmtpUsername, SmtpSettings.SmtpPassword);

                if (SmtpSettings.SmtpIgnoreServerCertificateValidation)
                {
                    // When checking the validity of a SSL certificate, always pass.
                    // This is needed for smtp.outlook365.com, since I cannot find a place to get the public key for the ssl certificate.
                    ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(
                            delegate
                            { return true; }
                            );
                }

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
                FSender = new MailAddress(AAddress, ADisplayName);
            }
            catch (Exception e)
            {
                throw new ESmtpSenderInitializeException(String.Format("Invalid sender address '{0} <{1}>'.",
                        ADisplayName,
                        AAddress), e, TSmtpErrorClassEnum.secClient);
            }
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
                        FCcEverythingTo = new MailAddressCollection();
                        FCcEverythingTo.Add(ConvertAddressList(value));
                    }
                    catch (Exception e)
                    {
                        throw new ESmtpSenderInitializeException(String.Format("Invalid CC address '{0}'.", value), e, TSmtpErrorClassEnum.secClient);
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
                        FReplyTo = new MailAddressCollection();
                        FReplyTo.Add(ConvertAddressList(value));
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
        /// Returns a new MailMessage with From, Reply-To and CC addresses set from the values in User Defaults.
        /// </summary>
        /// <returns>A MailMessage</returns>
        /// <exception cref="ESmtpSenderInitializeException">Thrown if the sender address has not been set.</exception>
        public MailMessage GetNewMailMessage()
        {
            if (FSender == null)
            {
                throw new ESmtpSenderInitializeException("Sender address has not been set.", TSmtpErrorClassEnum.secClient);
            }

            var NewMessage = new MailMessage();

            //Settings from User Defaults: From, Copy and Reply
            NewMessage.Sender = FSender;
            NewMessage.From = FSender;

            if (FCcEverythingTo != null)
            {
                foreach (var addr in FCcEverythingTo)
                {
                    NewMessage.CC.Add(addr);
                }
            }

            if (FReplyTo != null)
            {
                foreach (var addr in FReplyTo)
                {
                    NewMessage.ReplyToList.Add(addr);
                }
            }

            return NewMessage;
        }

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
                // Initialize a new MailMessage with settings from User Defaults
                using (MailMessage email = GetNewMailMessage())
                {
                    //To
                    email.To.Add(ConvertAddressList(recipients));

                    //Subject and Body
                    email.Subject = subject;
                    email.Body = body;
                    email.IsBodyHtml = false;

                    // at least for Mono, we need to keep the attachments separately in memory
                    // to avoid Exception: Object reference not set to an instance of an object
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
                } // End of using block. This will Dispose email and clean up any attachments.
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
        public bool SendMessage(MailMessage AEmail)
        {
            if (AEmail.Headers.Get("Date-Sent") != null)
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
            //    i) If a MailMessage has one To: address, failure is returned in a SmtpFailedRecipientException.
            //       If a MailMessage has one To: address and one CC: address, failure is returned in a SmtpFailedRecipientsException (note the plural).
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
                    FFailedRecipients.Add(FailureDetails);
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

        /// <summary>
        /// Dispose of the IDisposable objects used by this object.
        /// </summary>
        public void Dispose()
        {
            if (FSmtpClient != null)
            {
                FSmtpClient.Dispose();
            }

            if (FAttachedObject != null)
            {
                FAttachedObject.Dispose();
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

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public ESmtpSenderException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
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

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public ESmtpSenderInitializeException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
            //FErrorClass = (TSmtpErrorClassEnum)AInfo.GetValue("ErrorClass", typeof(TSmtpErrorClassEnum));
            ErrorClass = (TSmtpErrorClassEnum)AInfo.GetSByte("ErrorClass");
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            //AInfo.AddValue("ErrorClass", ErrorClass, typeof(TSmtpErrorClassEnum));
            AInfo.AddValue("ErrorClass", (sbyte)ErrorClass);

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
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

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public ESmtpSenderSendException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }
}
