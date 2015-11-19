//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2015 by OM International
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
using Ict.Common;
using Microsoft.CSharp;
using System.Windows.Forms;

namespace Ict.Common.IO
{
    /// <summary>
    /// this is a helper for sending emails via a local Outlook client
    /// </summary>
    public class TOutlookSender
    {
        bool FConnectionEstablished = false;
        dynamic FOutlookApp = null;
        dynamic FCurrentUser = null;

        /// <summary>
        /// can we communicate with the Outlook client
        /// </summary>
        public bool ConnectionEstablished {
            get
            {
                return FConnectionEstablished;
            }
        }

        /// <summary>
        /// setup the connection to Outlook
        /// </summary>
        public TOutlookSender()
        {
            FConnectionEstablished = false;
            try
            {
                System.Type t = System.Type.GetTypeFromProgID("Outlook.Application");
                FOutlookApp = System.Activator.CreateInstance(t);
                FCurrentUser = FOutlookApp.Session.CurrentUser.AddressEntry;
                FConnectionEstablished = true;
            }
            catch (System.Exception)
            {
                MessageBox.Show(Catalog.GetString("Please start Outlook and log in into your email account"), Catalog.GetString("Error"));
            }
        }

        /// <summary>
        /// Send an email message via Outlook.
        /// this makes use of Microsoft.Office.Interop.Outlook, but uses late binding so the dll is not required during building
        /// </summary>
        /// <param name="AEmail">on successful sending, the header is modified with the sent date</param>
        /// <returns>true if email was sent successfully</returns>
        public bool SendMessage(MailMessage AEmail)
        {
            if (!FConnectionEstablished)
            {
                return false;
            }

            if (AEmail.Headers.Get("Date-Sent") != null)
            {
                // don't send emails several times
                return false;
            }

            //Attempt to send the email
            try
            {
                AEmail.IsBodyHtml = AEmail.Body.ToLower().Contains("<html>");

                // Outlook.OlItemType.olMailItem is 0
                dynamic mail = FOutlookApp.CreateItem(0);
                mail.Subject = AEmail.Subject;

                if (AEmail.IsBodyHtml)
                {
                    mail.HTMLBody = AEmail.Body;
                }
                else
                {
                    mail.Body = AEmail.Body;
                }

                foreach (MailAddress email in AEmail.CC)
                {
                    mail.CC += email.ToString() + ";";
                }

                foreach (MailAddress email in AEmail.Bcc)
                {
                    mail.BCC += email.ToString() + ";";
                }

                foreach (MailAddress email in AEmail.To)
                {
                    mail.Recipients.Add(email.ToString());
                }

                mail.Recipients.ResolveAll();
                mail.Send();

                AEmail.Headers.Add("Date-Sent", DateTime.Now.ToString());

                return true;
            }
            catch (Exception ex)
            {
                TLogging.Log("There has been a problem sending the email");
                TLogging.Log(ex.ToString());
                MessageBox.Show(Catalog.GetString("There has been a problem sending the email.") +
                    Environment.NewLine +
                    ex.Message,
                    Catalog.GetString("Error"));
                return false;
            }
        }
    }
}
