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
using System.Windows.Forms;
using System.Data;
using Ict.Common;
using System.Resources;
using System.Globalization;
using Ict.Petra.Client.CommonForms;
using GNU.Gettext;

namespace Ict.Petra.Client.MCommon.Gui
{
    /// <summary>
    /// todoComment
    /// </summary>
    public partial class TUC_EditButtonPane : TPetraUserControl
    {
        #region Button Texts

        /// <summary>todoComment</summary>
        public const String resNewText = "       &New";

        /// <summary>todoComment</summary>
        public const String resEditText = "      Edi&t";

        /// <summary>todoComment</summary>
        public const String resDeleteText = "     &Delete";

        /// <summary>todoComment</summary>
        public const String resDoneText = "      D&one";

        /// <summary>todoComment</summary>
        public const String resCancelText = "     &Cancel";

        /// <summary>todoComment</summary>
        public const String resResizeText = "";
        #endregion

        #region ButtonTags

        /// <summary>todoComment</summary>
        public const String resNewTag = "New";

        /// <summary>todoComment</summary>
        public const String resEditTag = "Edit";

        /// <summary>todoComment</summary>
        public const String resDeleteTag = "Delete";

        /// <summary>todoComment</summary>
        public const String resDoneTag = "Done";

        /// <summary>todoComment</summary>
        public const String resCancelTag = "Cancel";

        /// <summary>todoComment</summary>
        public const String resResizeTag = "Resize";
        #endregion

        /// <summary>todoComment</summary>
        protected const Int32 constMinButtonWidth = 76;

        /// <summary>todoComment</summary>
        protected const Int32 constMinButtonHeight = 23;

        /// <summary>todoComment</summary>
        protected const Int32 constControlWidth = 80;

        /// <summary>todoComment</summary>
        protected const Int32 constControlHeight = 95;

        #region Variables

        /// <summary>todoComment</summary>
        protected TDataModeEnum FEditStatus;

        /// <summary>todoComment</summary>
        protected bool FNewButtonDisabled;

        /// <summary>todoComment</summary>
        protected bool FEditButtonDisabled;

        /// <summary>todoComment</summary>
        protected bool FDeleteButtonDisabled;

        /// <summary>todoComment</summary>
        protected bool FDoneButtonDisabled;

        /// <summary>todoComment</summary>
        protected bool FCancelButtonDisabled;

        /// <summary>todoComment</summary>
        protected bool FShowResizeButton;
        #endregion

        #region ButtonMargins

        /// <summary>todoComment</summary>
        protected int FMarginTop;

        /// <summary>todoComment</summary>
        protected int FMargin;

        /// <summary>todoComment</summary>
        protected int FMarginBottom;

        /// <summary>todoComment</summary>
        protected int FMarginLeft;

        /// <summary>todoComment</summary>
        protected int FMarginRight;

        /// <summary>todoComment</summary>
        protected int FMinButtonWidth;

        /// <summary>todoComment</summary>
        protected int FMinButtonHeight;

        /// <summary>todoComment</summary>
        protected int FButtonWidth;

        /// <summary>todoComment</summary>
        protected int FButtonHeight;

        /// <summary>todoComment</summary>
        protected int FMinWidth;

        /// <summary>todoComment</summary>
        protected int FMinHeight;
        #endregion

        #region statusbar

        /// <summary>todoComment</summary>
        protected string FStatusBarTextButtonNew;

        /// <summary>todoComment</summary>
        protected string FStatusBarTextButtonEdit;

        /// <summary>todoComment</summary>
        protected string FStatusBarTextButtonDelete;

        /// <summary>todoComment</summary>
        protected string FStatusBarTextButtonDone;

        /// <summary>todoComment</summary>
        protected string FStatusBarTextButtonCancel;

        /// <summary>todoComment</summary>
        protected string FStatusBarTextButtonResize;

        /// <summary>todoComment</summary>
        protected System.Windows.Forms.StatusBar FStatusbar;
        #endregion

        /// <summary>
        /// This property sets up the availability and the behavior of the different
        /// buttons.
        ///
        /// </summary>
        public TDataModeEnum EditStatus
        {
            get
            {
                return this.FEditStatus;
            }

            set
            {
                TDataModeEnum mOldvalue;
                TEditStatusChangedEventArgs mEventArgs;
                mOldvalue = this.FEditStatus;
                mEventArgs = new  TEditStatusChangedEventArgs();
                mEventArgs.OldEditStatus = mOldvalue;
                mEventArgs.NewEditStatus = value;

                if (mOldvalue != value)
                {
                    // Change the Edit Status
                    this.ChangeEditStatus(value);
                    this.FEditStatus = value;

                    // Fire EditStatusChanged event
                    // this.OnEditStatusChanged(this, mEventArgs);
                }
            }
        }

        /// <summary>
        /// This gets or sets the statusbar text for the 'New Button'.
        ///
        /// </summary>
        public string StatusBarTextButtonNew
        {
            get
            {
                return this.FStatusBarTextButtonNew;
            }

            set
            {
                this.FStatusBarTextButtonNew = value;
            }
        }

        /// <summary>
        /// This gets or sets the statusbar text for the 'New Button'.
        ///
        /// </summary>
        public string StatusBarTextButtonEdit
        {
            get
            {
                return this.FStatusBarTextButtonEdit;
            }
            set
            {
                this.FStatusBarTextButtonEdit = value;
            }
        }

        /// <summary>
        /// This gets or sets the statusbar text for the 'New Button'.
        ///
        /// </summary>
        public string StatusBarTextButtonDelete
        {
            get
            {
                return this.FStatusBarTextButtonDelete;
            }
            set
            {
                this.FStatusBarTextButtonDelete = value;
            }
        }

        /// <summary>
        /// This gets or sets the statusbar text for the 'New Button'.
        ///
        /// </summary>
        public string StatusBarTextButtonDone
        {
            get
            {
                return this.FStatusBarTextButtonDone;
            }
            set
            {
                this.FStatusBarTextButtonDone = value;
            }
        }

        /// <summary>
        /// This gets or sets the statusbar text for the 'New Button'.
        ///
        /// </summary>
        public string StatusBarTextButtonCancel
        {
            get
            {
                return this.FStatusBarTextButtonCancel;
            }
            set
            {
                this.FStatusBarTextButtonCancel = value;
            }
        }

        /// <summary>
        /// This gets or sets the statusbar text for the 'New Button'.
        ///
        /// </summary>
        public string StatusBarTextButtonResize
        {
            get
            {
                return this.FStatusBarTextButtonResize;
            }
            set
            {
                this.FStatusBarTextButtonResize = value;
            }
        }

        /// <summary>
        /// This disables (true) or enables (false) the Cancel Button in every Edit Status.
        ///
        /// </summary>
        public event TEventHandlerEditStatusChanged EditStatusChanged;

        /// <summary>
        /// This event is fired, when the 'New Button' has been clicked.
        ///
        /// </summary>
        public event TEventHandlerButtonNewClick ButtonNewClick;

        /// <summary>
        /// This event is fired, when the 'Edit Button' has been clicked.
        ///
        /// </summary>
        public event TEventHandlerButtonEditClick ButtonEditClick;

        /// <summary>
        /// This event is fired, when the 'Delete Button' has been clicked.
        ///
        /// </summary>
        public event TEventHandlerButtonDeleteClick ButtonDeleteClick;

        /// <summary>
        /// This event is fired, when the 'Done Button' has been clicked.
        ///
        /// </summary>
        public event TEventHandlerButtonDoneClick ButtonDoneClick;

        /// <summary>
        /// This event is fired, when the 'Cancel Button' has been clicked.
        ///
        /// </summary>
        public event TEventHandlerButtonCancelClick ButtonCancelClick;

        /// <summary>
        /// This event is fired, when the 'Resize Button' has been clicked.
        ///
        /// </summary>
        public event TEventHandlerButtonResizeClick ButtonResizeClick;

        #region Creation and Disposal

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_EditButtonPane() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnResizeButton.Text = Catalog.GetString("Resize Button");
            this.btnLowerButton.Text = Catalog.GetString("    LowerButton");
            this.btnMiddleButton.Text = Catalog.GetString("       MiddleButton");
            this.btnUpperButton.Text = Catalog.GetString("        UpperButton");
            #endregion

            this.InitializeUserControl();
        }

        /// <summary>
        /// Private Declarations
        /// </summary>
        /// <returns>void</returns>
        private void InitializeUserControl()
        {
            this.SetConstants();
            this.Set_EditStatus_Browse();

            // this.FStatusbar := new  System.Windows.Forms.StatusBar();
        }

        #endregion

        #region Button SetUp
        private void Set_EditStatus_Add()
        {
            this.SetUpNewButton();
            this.SetUpEditButton();
            this.btnMiddleButton.Enabled = false;
            this.SetUpDeleteButton();
            this.btnLowerButton.Enabled = false;
            this.SetUpResizeButton();
        }

        private void Set_EditStatus_Browse()
        {
            this.SetUpNewButton();
            this.SetUpEditButton();
            this.SetUpDeleteButton();
            this.SetUpResizeButton();
        }

        private void Set_EditStatus_Edit()
        {
            this.SetUpNewButton();
            this.btnUpperButton.Enabled = false;
            this.SetUpDoneButton();
            this.SetUpCancelButton();
            this.SetUpResizeButton();
        }

        private void ChangeEditStatus(TDataModeEnum Value)
        {
            switch (Value)
            {
                case TDataModeEnum.dmBrowse:
                    this.Set_EditStatus_Browse();
                    break;

                case TDataModeEnum.dmEdit:
                    this.Set_EditStatus_Edit();
                    break;

                case TDataModeEnum.dmAdd:
                    this.Set_EditStatus_Add();
                    break;

                default:
                    this.Set_EditStatus_Add();
                    break;
            }
        }

        private void SetConstants()
        {
            this.FMinButtonHeight = constMinButtonHeight;
            this.FMinButtonWidth = constMinButtonWidth;
        }

        private void SetUpCancelButton()
        {
            this.btnLowerButton.Tag = resCancelTag;
            this.btnLowerButton.Text = resCancelText;
            this.btnLowerButton.ImageIndex = 2;

            if (this.FNewButtonDisabled == false)
            {
                this.btnLowerButton.Enabled = true;
            }
            else
            {
                this.btnLowerButton.Enabled = false;
            }
        }

        private void SetUpDeleteButton()
        {
            this.btnLowerButton.Tag = resDeleteTag;
            this.btnLowerButton.Text = resDeleteText;
            this.btnLowerButton.ImageIndex = 4;

            if (this.FDeleteButtonDisabled == false)
            {
                this.btnLowerButton.Enabled = true;
            }
            else
            {
                this.btnLowerButton.Enabled = false;
            }
        }

        private void SetUpDoneButton()
        {
            this.btnMiddleButton.Tag = resDoneTag;
            this.btnMiddleButton.Text = resDoneText;
            this.btnMiddleButton.ImageIndex = 3;

            if (this.FDoneButtonDisabled == false)
            {
                this.btnMiddleButton.Enabled = true;
            }
            else
            {
                this.btnMiddleButton.Enabled = false;
            }
        }

        private void SetUpEditButton()
        {
            this.btnMiddleButton.Tag = resEditTag;
            this.btnMiddleButton.Text = resEditText;
            this.btnMiddleButton.ImageIndex = 1;

            if (this.FEditButtonDisabled == false)
            {
                this.btnMiddleButton.Enabled = true;
            }
            else
            {
                this.btnMiddleButton.Enabled = false;
            }
        }

        private void SetUpNewButton()
        {
            this.btnUpperButton.Tag = resNewTag;
            this.btnUpperButton.Text = resNewText;
            this.btnUpperButton.ImageIndex = 0;

            if (this.FNewButtonDisabled == false)
            {
                this.btnUpperButton.Enabled = true;
            }
            else
            {
                this.btnUpperButton.Enabled = false;
            }
        }

        private void SetUpResizeButton()
        {
            this.btnResizeButton.Tag = resResizeTag;
            this.btnResizeButton.Text = resResizeText;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void SetUpStatusBar()
        {
            // TODO: SetUpStatusBar
#if statusbar
            System.Windows.Forms.StatusBar mStatusBar;
            mStatusBar = (System.Windows.Forms.StatusBar)(this.sbtUserControl.InstanceStatusBar);

            if (this.FStatusbar != mStatusBar)
            {
                this.FStatusbar = mStatusBar;
            }
#endif
        }

        #endregion

        #region Events

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnEditStatusChanged(System.Object sender, TEditStatusChangedEventArgs e)
        {
            if (EditStatusChanged != null)
            {
                EditStatusChanged(this, e);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(System.EventArgs e)
        {
            System.Drawing.Size mSize;
            int mControlHeight;
            int mControlWidth;
            int mWidth;
            int mHeight;

            // Initialization
            mHeight = this.Height;
            mWidth = this.Width;

            // Set minimum height
            mControlHeight = TUC_EditButtonPane.constControlHeight;

            if (mHeight != mControlHeight)
            {
                mHeight = mControlHeight;
            }

            // Set minimum width
            mControlWidth = TUC_EditButtonPane.constControlWidth;

            if (mWidth != mControlWidth)
            {
                mWidth = mControlWidth;
            }

            // Set Size
            mSize = new System.Drawing.Size(mWidth, mHeight);
            this.Size = mSize;
        }

        private void TriggerButtonEvent(System.Object sender, System.EventArgs e)
        {
            System.Windows.Forms.Button mButton;
            String mTag;
            mButton = (System.Windows.Forms.Button)sender;
            mTag = mButton.Tag.ToString();

            if (mTag.Equals(resNewTag) == true)
            {
                if (ButtonNewClick != null)
                {
                    ButtonNewClick(sender, e);
                }
            }
            else if (mTag.Equals(resEditTag) == true)
            {
                if (ButtonEditClick != null)
                {
                    ButtonEditClick(sender, e);
                }
            }
            else if (mTag.Equals(resDeleteTag) == true)
            {
                if (ButtonDeleteClick != null)
                {
                    ButtonDeleteClick(sender, e);
                }
            }
            else if (mTag.Equals(resDoneTag) == true)
            {
                if (ButtonDoneClick != null)
                {
                    ButtonDoneClick(sender, e);
                }
            }
            else if (mTag.Equals(resCancelTag) == true)
            {
                if (ButtonCancelClick != null)
                {
                    ButtonCancelClick(sender, e);
                }
            }
            else if (mTag.Equals(resResizeTag) == true)
            {
                if (ButtonResizeClick != null)
                {
                    ButtonResizeClick(sender, e);
                }
            }
        }

        private void BtnUpperButton_Click(System.Object sender, System.EventArgs e)
        {
            this.TriggerButtonEvent(sender, e);
        }

        private void BtnMiddleButton_Click(System.Object sender, System.EventArgs e)
        {
            this.TriggerButtonEvent(sender, e);
        }

        private void BtnLowerButton_Click(System.Object sender, System.EventArgs e)
        {
            this.TriggerButtonEvent(sender, e);
        }

        private void BtnResizeButton_Click(System.Object sender, System.EventArgs e)
        {
            this.TriggerButtonEvent(sender, e);
        }

        private void TUC_EditButtonPane_Enter(System.Object sender, System.EventArgs e)
        {
            this.SetUpStatusBar();
        }

        #region Focus enters a button on the pane
        private void BtnUpperButton_Enter(System.Object sender, System.EventArgs e)
        {
            this.UpdateStatusBarTextButton(sender);
        }

        private void BtnResizeButton_Enter(System.Object sender, System.EventArgs e)
        {
            this.UpdateStatusBarTextButton(sender);
        }

        private void BtnLowerButton_Enter(System.Object sender, System.EventArgs e)
        {
            this.UpdateStatusBarTextButton(sender);
        }

        private void BtnMiddleButton_Enter(System.Object sender, System.EventArgs e)
        {
            this.UpdateStatusBarTextButton(sender);
        }

        #endregion

        #region Focus leave a button on the pane
        private void BtnUpperButton_Leave(System.Object sender, System.EventArgs e)
        {
            this.UpdateStatusBarTextButton(sender, "");
        }

        private void BtnMiddleButton_Leave(System.Object sender, System.EventArgs e)
        {
            this.UpdateStatusBarTextButton(sender, "");
        }

        private void BtnResizeButton_Leave(System.Object sender, System.EventArgs e)
        {
            this.UpdateStatusBarTextButton(sender, "");
        }

        private void BtnLowerButton_Leave(System.Object sender, System.EventArgs e)
        {
            this.UpdateStatusBarTextButton(sender, "");
        }

        #endregion

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AButton"></param>
        protected void UpdateStatusBarTextButton(System.Object AButton)
        {
            System.Windows.Forms.Button mButton;
            String mTag;

            if (this.FStatusbar == null)
            {
                return;
            }

            mButton = (System.Windows.Forms.Button)AButton;
            mTag = mButton.Tag.ToString();

            if (mTag.Equals(resNewTag) == true)
            {
                this.FStatusbar.Text = this.FStatusBarTextButtonNew;
            }
            else if (mTag.Equals(resEditTag) == true)
            {
                this.FStatusbar.Text = this.FStatusBarTextButtonEdit;
            }
            else if (mTag.Equals(resDeleteTag) == true)
            {
                this.FStatusbar.Text = this.FStatusBarTextButtonDelete;
            }
            else if (mTag.Equals(resDoneTag) == true)
            {
                this.FStatusbar.Text = this.FStatusBarTextButtonDone;
            }
            else if (mTag.Equals(resCancelTag) == true)
            {
                this.FStatusbar.Text = this.FStatusBarTextButtonCancel;
            }
            else if (mTag.Equals(resResizeTag) == true)
            {
                this.FStatusbar.Text = this.FStatusBarTextButtonResize;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AButton"></param>
        /// <param name="AStatusBarText"></param>
        protected void UpdateStatusBarTextButton(System.Object AButton, String AStatusBarText)
        {
            // System.Windows.Forms.Button mButton = (System.Windows.Forms.Button)AButton;

            if (this.FStatusbar == null)
            {
                return;
            }

            this.FStatusbar.Text = AStatusBarText;
        }

        #endregion
    }


    /// <summary>todoComment</summary>
    public delegate void TEventHandlerEditStatusChanged(System.Object sender, TEditStatusChangedEventArgs e);

    /// <summary>todoComment</summary>
    public delegate void TEventHandlerButtonNewClick(System.Object sender, System.EventArgs e);

    /// <summary>todoComment</summary>
    public delegate void TEventHandlerButtonEditClick(System.Object sender, System.EventArgs e);

    /// <summary>todoComment</summary>
    public delegate void TEventHandlerButtonDeleteClick(System.Object sender, System.EventArgs e);

    /// <summary>todoComment</summary>
    public delegate void TEventHandlerButtonDoneClick(System.Object sender, System.EventArgs e);

    /// <summary>todoComment</summary>
    public delegate void TEventHandlerButtonCancelClick(System.Object sender, System.EventArgs e);

    /// <summary>todoComment</summary>
    public delegate void TEventHandlerButtonResizeClick(System.Object sender, System.EventArgs e);

    /// <summary>
    /// Events
    /// </summary>
    public class TEditStatusChangedEventArgs : System.EventArgs
    {
        private TDataModeEnum FOldEditStatus;
        private TDataModeEnum FNewEditStatus;

        /// <summary>todoComment</summary>
        public TDataModeEnum OldEditStatus
        {
            get
            {
                return FOldEditStatus;
            }
            set
            {
                FOldEditStatus = value;
            }
        }

        /// <summary>todoComment</summary>
        public TDataModeEnum NewEditStatus
        {
            get
            {
                return FNewEditStatus;
            }
            set
            {
                FNewEditStatus = value;
            }
        }
    }
}