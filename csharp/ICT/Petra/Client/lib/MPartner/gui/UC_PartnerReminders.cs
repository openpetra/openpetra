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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.DataGrid;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonForms;
using System.Threading;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// UserControl for editing Partner Contacts (List/Detail view).
    /// @Comment Currently only a template that doesn't do much. Needs to be extended
    ///      to do anything useful (see Addresses Tab or Subscriptions Tab on how to do that)!
    /// </summary>
    public class TUCPartnerReminders : TPetraUserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Label label1;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        protected IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// Object that holds the logic for this screen
//		protected TUCPartnerRemindersLogic FLogic;
        /// <summary>holds the DataSet that contains most data that is used on the screen</summary>
        protected new PartnerEditTDS FMainDS;

        /// <summary>DataView that the SourceDataGrid is bound to</summary>
        protected DataView FDataGridDV;

        /// <summary>DataSet that contains most data that is used on the screen</summary>
        public new PartnerEditTDS MainDS
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

        /// <summary>Used for passing through the Clientside Proxy for the UIConnector</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }

        /// <summary>Custom Event for enabling/disabling of other parts of the screen</summary>
        public event TEnableDisableScreenPartsEventHandler EnableDisableOtherScreenParts;

        /// <summary>Custom Event for recalculation of the Tab Header</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>Custom Event for hooking up data change events</summary>
        public event THookupDataChangeEventHandler HookupDataChange;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify the contents of this
        /// method with the code editor.
        /// /// <summary>/// Required method for Designer support  do not modify/// the contents of this method with the code editor./// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();

            //
            // label1
            //
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(677, 32);
            this.label1.TabIndex = 0;
            this.label1.Text =
                "THIS IS CURRENTLY A NON-FUNCTIONAL TEMPLATE FOR A USERCONTROL FOR PARTNER REMINDERS. EXTEND IT TO MAKE IT FUNCTIONAL!";

            //
            // TUCPartnerReminders
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TUCPartnerReminders";
            this.Size = new System.Drawing.Size(740, 440);
            this.ResumeLayout(false);
        }

        #endregion

        #region TUCPartnerReminders

        /// <summary>
        /// Default Constructor.
        ///
        /// Define the screen's logic.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TUCPartnerReminders() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // define the screen's logic

//			FLogic = new TUCPartnerRemindersLogic();
        }

        /// <summary>
        /// Default WinForms function, created by the Designer
        ///
        /// </summary>
        /// <returns>void</returns>
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

        /// <summary>
        /// Raises Event EnableDisableOtherScreenParts.
        ///
        /// </summary>
        /// <param name="e">Event parameters
        /// </param>
        /// <returns>void</returns>
        protected void OnEnableDisableOtherScreenParts(TEnableDisableEventArgs e)
        {
            if (EnableDisableOtherScreenParts != null)
            {
                EnableDisableOtherScreenParts(this, e);
            }
        }

        /// <summary>
        /// Raises Event HookupDataChange.
        ///
        /// </summary>
        /// <param name="e">Event parameters (nothing in particluar for this Event)
        /// </param>
        /// <returns>void</returns>
        protected void OnHookupDataChange(System.EventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        /// <summary>
        /// Raises Event RecalculateScreenParts.
        ///
        /// </summary>
        /// <param name="e">Event parameters
        /// </param>
        /// <returns>void</returns>
        protected void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        #region Public Methods

        /// <summary>
        /// Sets up the screen logic, retrieves data, databinds the Grid and the Detail
        /// UserControl.
        ///
        /// </summary>
        /// <returns>void</returns>
        public new void InitialiseUserControl()
        {
            OnHookupDataChange(new System.EventArgs());
        }

        /// <summary>
        /// Checks whether there any Tips to show to the User; if there are, they will be
        /// shown.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CheckForUserTips()
        {
        }

        /// <summary>
        /// Applies Petra Security to restrict functionality, if needed.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void ApplySecurity()
        {
        }

        #endregion

        #region Helper functions

        /// <summary>
        /// Switches between 'Read Only Mode' and 'Edit Mode' of the Detail UserControl.
        ///
        /// In 'Read Only Mode' all controls in the Detail UserControl are disabled and
        /// the three Buttons next to the Grid are set up to perform Add, Edit and Delete
        /// operations. In 'Edit Mode' the controls in the Detail UserControl are enabled
        /// and the three Buttons next to the Grid are set up to perform Done and Cancel
        /// operations.
        ///
        /// </summary>
        /// <param name="ASelectCurrentRow">Set this to true to select the DataRow that is deemed
        /// to be the 'current' one in FLogic in the Grid and to DataBind the Details
        /// UserControl to this row, otherwise set to false.
        /// </param>
        /// <returns>void</returns>
        protected void SwitchDetailReadOnlyModeOrEditMode(Boolean ASelectCurrentRow)
        {
            ApplySecurity();
        }

        public void AdjustLabelControlsAfterResizing()
        {
        }

        public void HandleDisabledControlClick(System.Object sender, System.EventArgs e)
        {
        }

        #endregion

        #region Event Handlers
        #endregion

        #region SourceGrid Event Handlers
        #endregion

        #region Setup SourceDataGrid
        #endregion
        #endregion
    }

    #region TThreadedPartnerReminders
    public class TThreadedPartnerReminders : System.Object
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
#endif



        public static void ThreadedCheckForUserTips(System.Object AControl)
        {
        }
    }
    #endregion
}