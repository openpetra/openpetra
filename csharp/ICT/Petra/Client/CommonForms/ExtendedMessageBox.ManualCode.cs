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
        private TDefaultButton FDefaultButton = TDefaultButton.embdDefButton1;

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

        /// <summary>Default button index</summary>
        public enum TDefaultButton
        {
            /// <summary>No default Button</summary>
            embdDefButtonNone = 0,

            /// <summary>First Button</summary>
            embdDefButton1,

            /// <summary>Second Button</summary>
            embdDefButton2,

            /// <summary>Third Button</summary>
            embdDefButton3,

            /// <summary>Fourth Button</summary>
            embdDefButton4,

            /// <summary>Fifth Button</summary>
            embdDefButton5
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
            return ShowDialog(AMessage, ACaption, AChkOptionText, AButtons, TDefaultButton.embdDefButton1, TIcon.embiNone, false);
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
            return ShowDialog(AMessage, ACaption, AChkOptionText, AButtons, TDefaultButton.embdDefButton1, AIcon, false);
        }

        /// <summary>
        /// show form as dialog with given parameters
        /// </summary>
        /// <param name="AMessage">Message to be displayed to the user</param>
        /// <param name="ACaption">Caption of the dialog window</param>
        /// <param name="AChkOptionText">Text to be shown with check box (check box hidden if text empty)</param>
        /// <param name="AButtons">Button set to be displayed</param>
        /// <param name="ADefaultButton">The button with a default action</param>
        /// <param name="AIcon">Icon to be displayed</param>
        /// <returns></returns>
        public TFrmExtendedMessageBox.TResult ShowDialog(string AMessage, string ACaption, string AChkOptionText,
            TFrmExtendedMessageBox.TButtons AButtons,
            TFrmExtendedMessageBox.TDefaultButton ADefaultButton,
            TFrmExtendedMessageBox.TIcon AIcon)
        {
            return ShowDialog(AMessage, ACaption, AChkOptionText, AButtons, ADefaultButton, AIcon, false);
        }

        /// <summary>
        /// show form as dialog with given parameters
        /// </summary>
        /// <param name="AMessage">Message to be displayed to the user</param>
        /// <param name="ACaption">Caption of the dialog window</param>
        /// <param name="AChkOptionText">Text to be shown with check box (check box hidden if text empty)</param>
        /// <param name="AButtons">Button set to be displayed</param>
        /// <param name="ADefaultButton">The button with a default action</param>
        /// <param name="AIcon">Icon to be displayed</param>
        /// <param name="AOptionSelected">initial value for option check box</param>
        /// <returns></returns>
        public TFrmExtendedMessageBox.TResult ShowDialog(string AMessage, string ACaption, string AChkOptionText,
            TFrmExtendedMessageBox.TButtons AButtons,
            TFrmExtendedMessageBox.TDefaultButton ADefaultButton,
            TFrmExtendedMessageBox.TIcon AIcon,
            bool AOptionSelected)
        {
            string ResourceDirectory;
            string IconFileName;

            // initialize return values
            FResult = TResult.embrUndefined;
            FOptionSelected = AOptionSelected;

            lblMessage.Text = AMessage;
            lblMessage.BorderStyle = BorderStyle.FixedSingle;
            lblMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            pnlLeftButtons.MinimumSize = new Size(btnHelp.Width + btnCopy.Width + 10, pnlLeftButtons.Height);

            this.Text = ACaption;
            chkOption.Text = AChkOptionText;

            if (AChkOptionText.Length == 0)
            {
                chkOption.Visible = false;
            }

            chkOption.Checked = AOptionSelected;

            this.MinimumSize = new System.Drawing.Size(pnlLeftButtons.Width + pnlRightButtons.Width, 250);

            btnYes.Visible = false;
            btnYesToAll.Visible = false;
            btnNo.Visible = false;
            btnNoToAll.Visible = false;
            btnOK.Visible = false;
            btnCancel.Visible = false;

            FDefaultButton = ADefaultButton;

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

        private void Option_CheckStateChanged(object sender, System.EventArgs e)
        {
            // set member which is needed to return result later
            FOptionSelected = chkOption.Checked;
        }

        private void InitializeManualCode()
        {
            chkOption.CheckStateChanged += new System.EventHandler(this.Option_CheckStateChanged);
            this.Shown += new EventHandler(TFrmExtendedMessageBox_Shown);
        }

        void TFrmExtendedMessageBox_Shown(object sender, EventArgs e)
        {
            // Set the button positions based on their visibilty
            // Note - we have to do this in _Shown because before then we cannot check for Visible
            // We treat OK Cancel as being in a separate group so they get a bit more space between them and the others.
            // We rely on the fact that not all the buttons will be visible - true because we do not offer OK at the same time as Yes(ToAll) No(ToAll)
            int distance = btnYesToAll.Left - btnYes.Left;
            bool bDoneSmallDistance = false;
            bool bHasOkOrCancel = (btnOK.Visible || btnCancel.Visible);

            // We ignore btnApply - it was set to invisible at design time so never was placed on the button panel
            Button[] buttons =
            {
                btnYes, btnYesToAll, btnNo, btnNoToAll, btnOK, btnCancel
            };

            // Go through the buttons starting on the right
            for (int btnID = 5; btnID >= 0; btnID--)
            {
                if (!buttons[btnID].Visible)
                {
                    if (bDoneSmallDistance || !bHasOkOrCancel)
                    {
                        // Move the button a standard distance to the right
                        for (int k = 0; k < btnID; k++)
                        {
                            Console.WriteLine("{0}:  moving {1} by {2}", btnID, k, distance);
                            buttons[k].Left += distance;
                        }
                    }
                    else
                    {
                        // We need to move the left group of buttons a smaller distance to the right because we want to get a gap between the two groups
                        for (int k = 0; (k < btnID) && (k < 4); k++)
                        {
                            Console.WriteLine("{0}:  moving {1} by {2}", btnID, k, distance - 20);
                            buttons[k].Left += (distance - 20);
                        }

                        for (int k = 4; k < btnID; k++)
                        {
                            Console.WriteLine("{0}:  moving {1} by {2}", btnID, k, distance);
                            buttons[k].Left += distance;
                        }

                        // set the flag because we only want to do this smaller distance once
                        bDoneSmallDistance = true;
                    }
                }
            }

            // Set up the default button if one was specified.
            if (FDefaultButton != TDefaultButton.embdDefButtonNone)
            {
                // This is a 'counter'
                TDefaultButton currentDefID = TDefaultButton.embdDefButtonNone;

                for (int btnID = 0; btnID < buttons.Length; btnID++)
                {
                    if (buttons[btnID].Visible)
                    {
                        // increment the counter and see if we have reached the one we are looking for
                        currentDefID++;

                        if (currentDefID == FDefaultButton)
                        {
                            // Set the accept button for the form and focus it.
                            this.FindForm().AcceptButton = buttons[btnID];
                            buttons[btnID].Focus();
                            break;
                        }
                    }
                }
            }

            // Calculate the size required for the message, assuming the width is as it is
            SizeF size = lblMessage.CreateGraphics().MeasureString(lblMessage.Text, lblMessage.Font, pnlMessage.Width - 20);

            // Now set the height of the form based on allowing enough height for the message
            this.FindForm().Size = new Size(this.FindForm().Width, Convert.ToInt32(size.Height) + pnlLeftButtons.Height + 150);

            // Now center the message in its panel
            lblMessage.Size = new Size(pnlMessage.Width - 20, pnlMessage.Height - 20);
            lblMessage.Location = new Point((pnlMessage.Width - lblMessage.Width) / 2, (pnlMessage.Height - lblMessage.Height) / 2);
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

        private void Message_CopyToClipboard(object sender, EventArgs e)
        {
            Clipboard.SetText(lblMessage.Text);

            MessageBox.Show("The message text has been copied to clipboard.", MCommonResourcestrings.StrRecordDeletionTitle);
        }
    }
}