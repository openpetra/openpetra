//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
//
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Resources;
using System.Net.Sockets;
using System.Runtime.Remoting;

using Ict.Common;
using Ict.Common.Remoting.Client;

using Npgsql;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Dialog for displaying information to the user that a severe error occured in
    /// the PetraClient application.
    /// </summary>
    /// <description>
    /// Mimics Windows' own 'application crash window', but allows
    ///   (1) different texts to be displayed (eg. depending on the situation, the
    ///       error, etc.)
    ///   (2) different 'OK' button text (if it is a fatal error, the Text is changed
    ///       to 'Close Petra')
    ///   (3) different Icons to be displayed (eg. depending on the situation, the
    ///       error, etc.)
    ///   (4) opening a Error Detail screen that shows the Exception text and allows
    ///       copying of that to the Clipboard
    ///   (5) e-mailing the Exception text to the Petra Team using the 'Bugreport'
    ///       feature (only enabled if Progress 4GL didn't crash)
    ///
    /// This Form is intended to be called from Ict.Petra.Client.App.Core.ExceptionHandling.
    /// </description>
    public partial class TUnhandledExceptionForm : System.Windows.Forms.Form
    {
        readonly String StrNonRecoverableHeading = Catalog.GetString("OpenPetra Client Has Encountered An Error And Needs To Close");
        readonly String StrRecoverableHeading = Catalog.GetString("OpenPetra Client Has Encountered An Error");
        readonly String StrNonRecoverableInfo1 = Catalog.GetString(
            "The OpenPetra Client application has encountered an internal error from which it cannot recover.");
        readonly String StrRecoverableInfo1 = Catalog.GetString(
            "The OpenPetra Client application has encountered an internal error which it cannot handle properly.");
        readonly String StrNonRecoverableInfo2 = Catalog.GetString(
            "Any data that has not been saved will be lost. We are sorry for any inconvenience caused.");
        readonly String StrRecoverableInfo2 = Catalog.GetString(
            "You may be able to continue working with OpenPetra, but we recommend closing OpenPetra and starting over again. We are sorry for any inconvenience caused.");
        readonly String StrRecoverableInfo3 = Catalog.GetString("Choose 'OK' to close this error information. ");
        readonly String StrNonRecoverableInfo3 = Catalog.GetString("Choose 'Close OpenPetra' to close the OpenPetra application. ");
        readonly String StrErrorDetailsInfo = Catalog.GetString("Choose 'Error Details...' to see detailed information. ");
        readonly String StrRecoverableInfo3NoEmail = Catalog.GetString(
            "If you want to send this information to your OpenPetra Support Team, copy the information to the Clipboard and paste it into an e-mail. The e-mail address is: {0}.");
        readonly String StrConnectionBrokenInfo1 = Catalog.GetString(
            "The OpenPetra Client application has lost its connection to the OpenPetra Server. The broken connection cannot be recovered.");
        readonly String StrConnectionBrokenInfo2 = Catalog.GetString(
            "Try to save any data that has not been saved, then close OpenPetra and start over again. We are sorry for any inconvenience caused.");
        readonly String StrConnectionBrokenInfo3 = "";
        readonly String StrConnectionBrokenHeading = Catalog.GetString("OpenPetra Client Has Lost Its Connection To The OpenPetra Server");

        private Exception FException;
        private String FErrorDetails;
        private Boolean FNonRecoverable;
        private String FFormTitle = "",
                       FInfo2Text = "",
                       FInfo3Text = "",
                       FInfo1Text = "",
                       FHeadingText = "";

        /// <summary>The Exception that is handled by this screen.</summary>
        public Exception TheException
        {
            get
            {
                return FException;
            }

            set
            {
                string ApplicationVersion = String.Empty;
                Npgsql.NpgsqlException NpgEx;

                FException = value;

                if (TClientInfo.ClientAssemblyVersion != null)
                {
                    ApplicationVersion =
                        (new Version(TClientInfo.ClientAssemblyVersion)).ToString();
                }

                /* Build error details String */
                FErrorDetails = FException.ToString();

                // To get full details incl. Severity, Error, Position in SQL, etc. of a PostgreSQL Exception we need to
                // call .ToString() on an Exception of Type Npgsql.NpgsqlException
                NpgEx = FException as Npgsql.NpgsqlException;

                if (NpgEx == null)
                {
                    NpgEx = FException.InnerException as Npgsql.NpgsqlException;
                }

                if (NpgEx != null)
                {
                    FErrorDetails += Environment.NewLine + "----------------" + Environment.NewLine +
                                     "Npgsql.NpgsqlException Detail Information:" + Environment.NewLine;

                    FErrorDetails += NpgEx.ToString();
                }

                FErrorDetails += Environment.NewLine + "--------------------------------------" +
                                 Environment.NewLine;

                if (ApplicationVersion != String.Empty)
                {
                    FErrorDetails = FErrorDetails + "OpenPetra Version " + ApplicationVersion + Environment.NewLine;
                }

                FErrorDetails = FErrorDetails + "Date/Time: " + DateTime.Now.ToString() + " (UTC: " + DateTime.UtcNow.ToString("r") + ")";
            }
        }

        /// <summary>True if the Exception handled by this screen is non-recoverable, false if it is recoverable.</summary>
        public Boolean NonRecoverable
        {
            get
            {
                return FNonRecoverable;
            }

            set
            {
                FNonRecoverable = value;
            }
        }

        /// <summary>Set this to set a non-default Title for this Form.</summary>
        public String FormTitle
        {
            get
            {
                return FFormTitle;
            }

            set
            {
                FFormTitle = value;
            }
        }

        /// <summary>Set this to set a non-default heading.</summary>
        public String HeadingText
        {
            get
            {
                return FHeadingText;
            }

            set
            {
                FHeadingText = value;
            }
        }

        /// <summary>Set this to set a non-default Info 1 text (shown in the header part of the Form).</summary>
        public String Info1Text
        {
            get
            {
                return FInfo1Text;
            }

            set
            {
                FInfo1Text = value;
            }
        }

        /// <summary>Set this to set a non-default Info 2 text (shown in the lower part of the Form).</summary>
        public String Info2Text
        {
            get
            {
                return FInfo2Text;
            }

            set
            {
                FInfo2Text = value;
            }
        }

        /// <summary>Set this to set a non-default Info 3 text (shown in the lower part of the Form).</summary>
        public String Info3Text
        {
            get
            {
                return FInfo3Text;
            }

            set
            {
                FInfo3Text = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TUnhandledExceptionForm() : base()
        {
            /*  */
            /* Required for Windows Form Designer support */
            /*  */
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblInfo1.Text = Catalog.GetString("Info 1 Text");
            this.lblHeading.Text = Catalog.GetString("Title Text");
            this.lblInfo3.Text = Catalog.GetString("Info 3 Text");
            this.lblInfo2.Text = Catalog.GetString("Info 2 Text");
            this.btnErrorDetails.Text = Catalog.GetString("Error &Details...");
            this.btnClose.Text = Catalog.GetString("&Close OpenPetra");
            this.btnSend.Text = Catalog.GetString("&Report Error");
            this.Text = Catalog.GetString("OpenPetra x.x.x.x Application Error");
            #endregion

            /*  */
            /* TODO: Add any constructor code after InitializeComponent call */
            /*  */
        }

        private void BtnSend_Click(System.Object sender, System.EventArgs e)
        {
            // TODO Send content of FErrorDetails via email!
        }

        private void BtnErrorDetails_Click(System.Object sender, System.EventArgs e)
        {
            TFrmUnhandledExceptionDetailsDialog UHEDDialogue;

            UHEDDialogue = new TFrmUnhandledExceptionDetailsDialog(this);
            UHEDDialogue.ErrorDetails = FErrorDetails;
            UHEDDialogue.ShowDialog();

            /* get UnhandledExceptionDetails Dialogue out of memory */
            UHEDDialogue.Dispose();
        }

        private void TUnhandledExceptionForm_Load(System.Object sender, System.EventArgs e)
        {
            if ((FException is System.Net.Sockets.SocketException
                 || FException is System.Runtime.Remoting.RemotingException)
                || FException.InnerException is System.Net.Sockets.SocketException
                || FException.InnerException is System.Runtime.Remoting.RemotingException)
            {
                FNonRecoverable = false;
                FHeadingText = StrConnectionBrokenHeading;
                FInfo1Text = StrConnectionBrokenInfo1;
                FInfo2Text = StrConnectionBrokenInfo2;
                FInfo3Text = StrConnectionBrokenInfo3 + StrRecoverableInfo3;

                /* Adjust Icon */
                picIcon.Image = imlIcons.Images[2];
            }
            else
            {
                if (!FNonRecoverable)
                {
                    if (FHeadingText == "")
                    {
                        lblHeading.Text = StrRecoverableHeading;
                    }

                    if (FInfo1Text == "")
                    {
                        lblInfo1.Text = StrRecoverableInfo1;
                    }

                    if (FInfo2Text == "")
                    {
                        lblInfo2.Text = StrRecoverableInfo2;
                    }

                    if (FInfo3Text == "")
                    {
                        lblInfo3.Text = StrRecoverableInfo3;
                    }

                    /* Adjust Icon */
                    picIcon.Image = imlIcons.Images[1];
                }
                else
                {
                    if (FHeadingText == "")
                    {
                        lblHeading.Text = StrNonRecoverableHeading;
                    }

                    if (FInfo1Text == "")
                    {
                        lblInfo1.Text = StrNonRecoverableInfo1;
                    }

                    if (FInfo2Text == "")
                    {
                        lblInfo2.Text = StrNonRecoverableInfo2;
                    }
                }
            }

            if (!FNonRecoverable)
            {
                btnClose.Text = "&OK";
            }
            else
            {
                FInfo3Text = StrNonRecoverableInfo3;
            }

            FInfo3Text = FInfo3Text + StrErrorDetailsInfo;

            if (TClientSettings.PetraSupportTeamEmail != String.Empty)
            {
                // TODO Replace TClientSettings.PetraSupportTeamEmail with StrRecoverableInfo3Email once emailing of exception details works!
                // readonly String StrRecoverableInfo3Email = Catalog.GetString("Choose 'Report Error' to send an error report in an e-mail to your OpenPetra Support Team.");
                FInfo3Text = FInfo3Text + String.Format(StrRecoverableInfo3NoEmail, TClientSettings.PetraSupportTeamEmail);
            }

            if (FFormTitle != "")
            {
                this.Text = FFormTitle;
            }
            else
            {
                if (TClientInfo.ClientAssemblyVersion != null)
                {
                    this.Text = "OpenPetra.org " +
                                (new Version(TClientInfo.ClientAssemblyVersion)).ToString() + ' ' + Catalog.GetString("Application Error");
                }
                else
                {
                    this.Text = "OpenPetra.org " + Catalog.GetString("Application Error");
                }
            }

            if (FHeadingText != "")
            {
                lblHeading.Text = FHeadingText;
            }

            if (FInfo1Text != "")
            {
                lblInfo1.Text = FInfo1Text;
            }

            if (FInfo2Text != "")
            {
                lblInfo2.Text = FInfo2Text;
            }

            if (FInfo3Text != "")
            {
                lblInfo3.Text = FInfo3Text;
            }

            /* btnSend_Click(Self, nil); */
        }

        private void BtnClose_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();

            if (FNonRecoverable)
            {
                // APPLICATION STOPS HERE !!!
                if (ExceptionHandling.GApplicationShutdownCallback != null)
                {
                    ExceptionHandling.GApplicationShutdownCallback();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}