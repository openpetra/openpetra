/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using Ict.Common;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using System.Globalization;
using System.Threading;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Formatting;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// UserControl for editing Partner Notes.
    /// </summary>
    public class TUC_PartnerNotes : System.Windows.Forms.UserControl, IPetraEditUserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlPartnerDetailsPerson;
        private System.Windows.Forms.GroupBox grpNames;
        private TbtnCreated btnCreatedPartner;
        private System.Windows.Forms.Panel pnlNotes;
        private System.Windows.Forms.TextBox txtPartnerComment;
        private TexpTextBoxStringLengthCheck expStringLengthCheckNotes;
        private System.Windows.Forms.Panel pnlBalloonTipAnchor;

        /// <summary>todoComment</summary>
        protected PartnerEditTDS FMainDS;

        /// <summary>todoComment</summary>
        public PartnerEditTDS MainDS
        {
            get
            {
                return FMainDS;
            }

            set
            {
                FMainDS = value;
            }
        }

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #region Windows Form Designer generated code

        /// <summary>
        /// txtOccupationCode: TtxtAutoPopulatedButtonLabel; <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the
        /// contents of this method with the code editor. </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerNotes));
            this.components = new System.ComponentModel.Container();
            this.pnlPartnerDetailsPerson = new System.Windows.Forms.Panel();
            this.btnCreatedPartner = new Ict.Common.Controls.TbtnCreated();
            this.grpNames = new System.Windows.Forms.GroupBox();
            this.pnlNotes = new System.Windows.Forms.Panel();
            this.txtPartnerComment = new System.Windows.Forms.TextBox();
            this.expStringLengthCheckNotes = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlBalloonTipAnchor = new System.Windows.Forms.Panel();
            this.pnlPartnerDetailsPerson.SuspendLayout();
            this.grpNames.SuspendLayout();
            this.pnlNotes.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlPartnerDetailsPerson
            //
            this.pnlPartnerDetailsPerson.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPartnerDetailsPerson.AutoScroll = true;
            this.pnlPartnerDetailsPerson.AutoScrollMinSize = new System.Drawing.Size(550, 330);
            this.pnlPartnerDetailsPerson.Controls.Add(this.pnlBalloonTipAnchor);
            this.pnlPartnerDetailsPerson.Controls.Add(this.btnCreatedPartner);
            this.pnlPartnerDetailsPerson.Controls.Add(this.grpNames);
            this.pnlPartnerDetailsPerson.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerDetailsPerson.Name = "pnlPartnerDetailsPerson";
            this.pnlPartnerDetailsPerson.Size = new System.Drawing.Size(634, 530);
            this.pnlPartnerDetailsPerson.TabIndex = 0;

            //
            // btnCreatedPartner
            //
            this.btnCreatedPartner.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedPartner.CreatedBy = null;
            this.btnCreatedPartner.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedPartner.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedPartner.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedPartner.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnCreatedPartner.Image")));
            this.btnCreatedPartner.Location = new System.Drawing.Point(616, 2);
            this.btnCreatedPartner.ModifiedBy = null;
            this.btnCreatedPartner.Name = "btnCreatedPartner";
            this.btnCreatedPartner.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedPartner.TabIndex = 25;
            this.btnCreatedPartner.Tag = "dontdisable";

            //
            // grpNames
            //
            this.grpNames.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpNames.Controls.Add(this.pnlNotes);
            this.grpNames.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpNames.Location = new System.Drawing.Point(4, 0);
            this.grpNames.Name = "grpNames";
            this.grpNames.Size = new System.Drawing.Size(610, 176);
            this.grpNames.TabIndex = 0;
            this.grpNames.TabStop = false;
            this.grpNames.Text = "Notes";

            //
            // pnlNotes
            //
            this.pnlNotes.Controls.Add(this.txtPartnerComment);
            this.pnlNotes.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNotes.Location = new System.Drawing.Point(3, 17);
            this.pnlNotes.Name = "pnlNotes";
            this.pnlNotes.Size = new System.Drawing.Size(604, 151);
            this.pnlNotes.TabIndex = 6;

            //
            // txtPartnerComment
            //
            this.txtPartnerComment.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPartnerComment.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtPartnerComment.Location = new System.Drawing.Point(6, 0);
            this.txtPartnerComment.Multiline = true;
            this.txtPartnerComment.Name = "txtPartnerComment";
            this.txtPartnerComment.Size = new System.Drawing.Size(592, 146);
            this.txtPartnerComment.TabIndex = 22;
            this.txtPartnerComment.Text = "";

            //
            // pnlBalloonTipAnchor
            //
            this.pnlBalloonTipAnchor.Location = new System.Drawing.Point(60, 0);
            this.pnlBalloonTipAnchor.Name = "pnlBalloonTipAnchor";
            this.pnlBalloonTipAnchor.Size = new System.Drawing.Size(1, 1);
            this.pnlBalloonTipAnchor.TabIndex = 10;

            //
            // TUC_PartnerNotes
            //
            this.Controls.Add(this.pnlPartnerDetailsPerson);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_PartnerNotes";
            this.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsPerson.ResumeLayout(false);
            this.grpNames.ResumeLayout(false);
            this.pnlNotes.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_PartnerNotes() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        private TFrmPetraEditUtils FPetraUtilsObject;

        /// <summary>
        /// this provides general functionality for edit screens
        /// </summary>
        public TFrmPetraEditUtils PetraUtilsObject
        {
            get
            {
                return FPetraUtilsObject;
            }
            set
            {
                FPetraUtilsObject = value;
            }
        }

        private void TxtPartnerComment_Validated(System.Object sender, System.EventArgs e)
        {
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="Disposing"></param>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        #region Public methods

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitialiseUserControl()
        {
            // Special information
            btnCreatedPartner.UpdateFields(FMainDS.PPartner);

            // Notes GroupBox
            txtPartnerComment.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetCommentDBName());

            // Set StatusBar Texts
            FPetraUtilsObject.SetStatusBarText(txtPartnerComment, PPartnerTable.GetCommentHelp());
            ApplySecurity();

            // Extender Provider
            this.expStringLengthCheckNotes.RetrieveTextboxes(this);
            this.txtPartnerComment.Validated += new EventHandler(this.TxtPartnerComment_Validated);
        }

        /// <summary>
        /// Checks whether there any Tips to show to the User; if there are, they will be
        /// shown.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CheckForUserTips()
        {
            Rectangle SelectedTabHeaderRectangle;

            // Calculate where the middle of the TabHeader of the Tab lies at this moment
            SelectedTabHeaderRectangle = ((TTabVersatile) this.Parent.Parent.Parent).SelectedTabHeaderRectangle;
            pnlBalloonTipAnchor.Left = SelectedTabHeaderRectangle.X + Convert.ToInt16(SelectedTabHeaderRectangle.Width / 2.0);

            // pnlBalloonTipAnchor.Left := 60;
            // pnlBalloonTipAnchor.Top := 10;
            // Check for BalloonTips to display, and show them
// TODO            System.Threading.ThreadPool.QueueUserWorkItem(@TThreadedNotes.ThreadedCheckForUserTips, pnlBalloonTipAnchor);
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        #endregion

        #region Helper functions
        private void ApplySecurity()
        {
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_partner
                // MessageBox.Show('Disabling p_partner fields...');
                CustomEnablingDisabling.DisableControl(pnlNotes, txtPartnerComment);
            }
        }

        #endregion
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TThreadedNotes : System.Object
    {
// TODO balloon
#if TODO
        private static TBalloonTip UBalloonTip;

        public static void PrepareBalloonTip()
        {
            // Ensure that we have an instance of TBalloonTip
            if (UBalloonTip == null)
            {
                UBalloonTip = new TBalloonTip();
            }
        }

        public static void ThreadedCheckForUserTips(System.Object AControl)
        {
            /*
             * Check for specific TipStatus: '!' means 'show the Tip, but then set it'
             *
             * This is done to pick up Tips only for the Users that they were specifically
             * set for, and not for users where the TipStatus would be un-set ('-')!
             * This is helfpful eg. for Patch Notices - they should be shown to all current
             * users (the patch program would set the TipStatus to '!' for all current users),
             * but not for users that get created after the Patch was applied (they don't
             * need to know what was new/has changed in a certain Patch that was applied in
             * the past!)
             */
            if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersNotes) == '!')
            {
                // Set Tip Status so it doesn't get picked up again!
                TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatNewTabCountersNotes);

                // MessageBox.Show('TUC_PartnerNotes.CheckForUserTips: showing BalloonTip...');
                PrepareBalloonTip();
                UBalloonTip.ShowBalloonTipNewFunction(
                    "Petra Version 2.2.7 Change: Indicator in Notes Tab Header",
                    "An indicator has been added to the tab header that shows whether Notes are entered for the Partner," + "\r\n" +
                    "or not: (0) means that no Notes are entered, (" + (char)8730 + ") means that Notes are entered." + "\r\n" +
                    "The Addresses tab and Subscriptions tab also have changes to the counters in the tab headers.",
                    (Control)AControl);

                // 8730 = 'square root symbol' in Verdana
                UBalloonTip = null;
            }
        }
#endif

    }
}