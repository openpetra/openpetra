//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.CommonForms
{
    /// manual methods for the generated window
    public partial class TFrmExtendedMessageBox
    {
        // result of dialog frame that is kept to be returned with GetResult
        private TResult FResult = TResult.embrUndefined;

        // indicates if option check box is ticked or not
        private bool FOptionSelected;

        private PictureBox FIconControl;
        private Bitmap FBitmap;

        /// <summary>Scope of data that is already available client-side.</summary>
        public enum TButtons
        {
            /// <summary>Show buttons for Yes, Yes to all, No and Cancel</summary>
            embbYesYesToAllNoCancel,

            /// <summary>Show buttons for Yes, Yes to all, No, No to all and Cancel</summary>
            embbYesYesToAllNoNoToAllCancel,

            /// <summary>Show buttons for Yes, Yes to all, No and No to all</summary>
            embbYesYesToAllNoNoToAll,

            /// <summary>Show buttons for Yes and No</summary>
            embbYesNo,

            /// <summary>Show buttons for Yes, No and Cancel</summary>
            embbYesNoCancel,

            /// <summary>Show button for OK</summary>
            embbOK,

            /// <summary>Show buttons for OK and Cancel</summary>
            embbOKCancel
        }

        /// <summary>Result returned from message box in GetResult</summary>
        public enum TResult
        {
            /// <summary>Button 'Yes' was pressed</summary>
            embrYes,

            /// <summary>Button 'Yes To All' was pressed</summary>
                embrYesToAll,

            /// <summary>Button 'No' was pressed</summary>
                embrNo,

            /// <summary>Button 'No To All' was pressed</summary>
                embrNoToAll,

            /// <summary>Button 'OK' was pressed</summary>
                embrOK,

            /// <summary>Button 'Cancel' was pressed</summary>
                embrCancel,

            /// <summary>It is undefined which button was pressed</summary>
                embrUndefined
        }

        /// <summary>Result returned from message box in GetResult</summary>
        public enum TIcon
        {
            /// <summary>No icon to be displayed</summary>
            embiNone,

            /// <summary>display icon for question</summary>
                embiQuestion,

            /// <summary>display icon for information</summary>
                embiInformation,

            /// <summary>display warning icon</summary>
                embiWarning,

            /// <summary>display error icon</summary>
                embiError
        }

        /// <summary>
        /// show form as dialog with given parameters
        /// </summary>
        /// <param name="AMessage">Message to be displayed to the user</param>
        /// <param name="ACaption">Caption of the dialog window</param>
        /// <param name="AChkOptionText">Text to be shown with check box (check box hidden if text empty)</param>
        /// <param name="AButtons">Button set to be displayed</param>
        /// <returns></returns>
        public TFrmExtendedMessageBox.TResult ShowDialog(string AMessage, string ACaption, string AChkOptionText,
            TFrmExtendedMessageBox.TButtons AButtons)
        {
            return ShowDialog(AMessage, ACaption, AChkOptionText, AButtons, TIcon.embiNone, false);
        }

        /// <summary>
        /// show form as dialog with given parameters
        /// </summary>
        /// <param name="AMessage">Message to be displayed to the user</param>
        /// <param name="ACaption">Caption of the dialog window</param>
        /// <param name="AChkOptionText">Text to be shown with check box (check box hidden if text empty)</param>
        /// <param name="AButtons">Button set to be displayed</param>
        /// <param name="AIcon">Icon to be displayed</param>
        /// <returns></returns>
        public TFrmExtendedMessageBox.TResult ShowDialog(string AMessage, string ACaption, string AChkOptionText,
            TFrmExtendedMessageBox.TButtons AButtons,
            TFrmExtendedMessageBox.TIcon AIcon)
        {
            return ShowDialog(AMessage, ACaption, AChkOptionText, AButtons, AIcon, false);
        }

        /// <summary>
        /// show form as dialog with given parameters
        /// </summary>
        /// <param name="AMessage">Message to be displayed to the user</param>
        /// <param name="ACaption">Caption of the dialog window</param>
        /// <param name="AChkOptionText">Text to be shown with check box (check box hidden if text empty)</param>
        /// <param name="AButtons">Button set to be displayed</param>
        /// <param name="AIcon">Icon to be displayed</param>
        /// <param name="AOptionSelected">initial value for option check box</param>
        /// <returns></returns>
        public TFrmExtendedMessageBox.TResult ShowDialog(string AMessage, string ACaption, string AChkOptionText,
            TFrmExtendedMessageBox.TButtons AButtons,
            TFrmExtendedMessageBox.TIcon AIcon,
            bool AOptionSelected)
        {
            string ResourceDirectory;
            string IconFileName;

            // initialize return values
            FResult = TResult.embrUndefined;
            FOptionSelected = AOptionSelected;

            lblMessage.Text = AMessage;
            this.FindForm().Text = ACaption;
            chkOption.Text = AChkOptionText;

            if (AChkOptionText.Length == 0)
            {
                chkOption.Visible = false;
            }

            chkOption.Checked = AOptionSelected;

            btnYes.Visible = false;
            btnYesToAll.Visible = false;
            btnNo.Visible = false;
            btnNoToAll.Visible = false;
            btnOK.Visible = false;
            btnCancel.Visible = false;

            switch (AButtons)
            {
                case TButtons.embbYesYesToAllNoCancel:
                    btnYes.Visible = true;
                    btnYesToAll.Visible = true;
                    btnNo.Visible = true;
                    btnCancel.Visible = true;
                    break;

                case TButtons.embbYesYesToAllNoNoToAllCancel:
                    btnYes.Visible = true;
                    btnYesToAll.Visible = true;
                    btnNo.Visible = true;
                    btnNoToAll.Visible = true;
                    btnCancel.Visible = true;
                    break;

                case TButtons.embbYesYesToAllNoNoToAll:
                    btnYes.Visible = true;
                    btnYesToAll.Visible = true;
                    btnNo.Visible = true;
                    btnNoToAll.Visible = true;
                    break;

                case TButtons.embbYesNo:
                    btnYes.Visible = true;
                    btnNo.Visible = true;
                    break;

                case TButtons.embbYesNoCancel:
                    btnYes.Visible = true;
                    btnNo.Visible = true;
                    btnCancel.Visible = true;
                    break;

                case TButtons.embbOK:
                    btnOK.Visible = true;
                    break;

                case TButtons.embbOKCancel:
                    btnOK.Visible = true;
                    btnCancel.Visible = true;
                    break;

                default:
                    break;
            }

            // dispose of items in case they were used already earlier
            if (FBitmap != null)
            {
                FBitmap.Dispose();
            }

            // find the right icon name
            switch (AIcon)
            {
                case TIcon.embiQuestion:
                    IconFileName = "Help.ico";
                    break;

                case TIcon.embiInformation:
                    IconFileName = "PetraInformation.ico";
                    break;

                case TIcon.embiWarning:
                    IconFileName = "Warning.ico";
                    break;

                case TIcon.embiError:
                    IconFileName = "Error.ico";
                    break;

                default:
                    IconFileName = "";
                    break;
            }

            if (FIconControl == null)
            {
                FIconControl = new PictureBox();

                // Stretches the image to fit the pictureBox.
                FIconControl.SizeMode = PictureBoxSizeMode.StretchImage;
                FIconControl.ClientSize = new Size(30, 30);
                pnlIcon.Padding = new Padding(3, 3, 3, 3);
            }

            // load and set the image
            ResourceDirectory = TAppSettingsManager.GetValue("Resource.Dir");

            if ((AIcon != TIcon.embiNone)
                && System.IO.File.Exists(ResourceDirectory + System.IO.Path.DirectorySeparatorChar + IconFileName))
            {
                pnlIcon.Visible = true;
                FBitmap = new System.Drawing.Bitmap(ResourceDirectory + System.IO.Path.DirectorySeparatorChar + IconFileName);
                FIconControl.Image = (Image)FBitmap;

                if (!pnlIcon.Controls.Contains(FIconControl))
                {
                    pnlIcon.Controls.Add(FIconControl);
                }
            }
            else
            {
                // remove icon panel if it already exists
                if (pnlIcon.Controls.Contains(FIconControl))
                {
                    pnlIcon.Controls.Remove(FIconControl);
                }

                pnlIcon.Visible = false;
            }

            // now show the actual dialog
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowDialog();

            // FResult is initialized when buttons are pressed
            return FResult;
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the result of the dialog
        ///
        /// </summary>
        /// <returns>information which button was pressed
        /// </returns>
        public TFrmExtendedMessageBox.TResult GetResult(out bool AOptionSelected)
        {
            AOptionSelected = FOptionSelected;
            return FResult;
        }

        private void Message_Resize(object sender, System.EventArgs e)
        {
            // adapt size of form to size of message displayed
            Control control = (Control)sender;

            if (control == lblMessage)
            {
                control.FindForm().Size = new Size(FindForm().Size.Width, control.Size.Height + 180);
            }
        }

        private void Option_CheckStateChanged(object sender, System.EventArgs e)
        {
            // set member which is needed to return result later
            FOptionSelected = chkOption.Checked;
        }

        private void InitializeManualCode()
        {
            // set the maximum width (height still flexible)
            lblMessage.MaximumSize = new Size(500, 0);

            // make the form resize when the message field resizes
            lblMessage.Resize += new System.EventHandler(this.Message_Resize);

            chkOption.CheckStateChanged += new System.EventHandler(this.Option_CheckStateChanged);
        }

        private void BtnYes_Click(Object Sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            FResult = TResult.embrYes;
            this.Close();
        }

        private void BtnYesToAll_Click(Object Sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            FResult = TResult.embrYesToAll;
            this.Close();
        }

        private void BtnNo_Click(Object Sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            FResult = TResult.embrNo;
            this.Close();
        }

        private void BtnNoToAll_Click(Object Sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            FResult = TResult.embrNoToAll;
            this.Close();
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            FResult = TResult.embrOK;
            this.Close();
        }

        private void BtnCancel_Click(Object Sender, EventArgs e)
        {
            //this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            FResult = TResult.embrCancel;
            this.Close();
        }
    }
}