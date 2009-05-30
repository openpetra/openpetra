/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop, christiank
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
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// This UserControl is the Base UserControl for all Petra UserControls.
    ///
    /// It contains a StatusBarTextProvider and some default variables and properties.
    ///
    /// @Comment All UserControls that are used in Petra should inherit from this
    ///   UserControl!
    /// </summary>
    public class TPetraUserControl : System.Windows.Forms.UserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;

        // TODO statusbar text
        private System.Windows.Forms.ToolTip tipUC;

        /// <summary>Holds the DataSet that contains most data that is used in the UserControl</summary>
        protected DataSet FMainDS;

        /// <summary>Used for keeping track of data verification errors</summary>
        protected TVerificationResultCollection FVerificationResultCollection;

        /// <summary>Used for storing the current DataMode (Browse, Edit, Add, ...)</summary>
        protected TDataModeEnum FDataMode;

        /// <summary>todoComment</summary>
        public DataSet MainDS
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
        public TVerificationResultCollection VerificationResultCollection
        {
            get
            {
                return FVerificationResultCollection;
            }

            set
            {
                FVerificationResultCollection = value;
            }
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tipUC = new System.Windows.Forms.ToolTip(this.components);

            //
            // TPetraUserControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);  // IMPORTANT ASSIGNMENT FOR ALL DESCENDANT USERCONTROLS THAT HAVE A DIFFERENT FONT/FONTSIZE THAN THE STANDARD ('VERDANA, 8.25F'): THIS ALLOWS CORRECT SCALING ON NON-96DPI
                                                                           // (E.G. 'LARGE FONTS (120DPI)') DISPLAY SETTINGS!
            this.Name = "TPetraUserControl";
        }

        #endregion

        /// <summary>
        /// Special property to determine whether our code is running in the WinForms Designer.
        /// The result of this property is correct even if InitializeComponent() wasn't run yet
        /// (.NET's DesignMode property returns false in that case)!
        /// </summary>
        private bool InDesignMode
        {
            get
            {
                return (this.GetService(typeof(IDesignerHost)) != null)
                       || (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime);
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TPetraUserControl() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            if (UserInfo.GUserInfo != null)
            {
                /*
                 * Initialise the RestoreState component. It manages the Control
                 * Locations/Sizes (for detemining Splitters' Locations).
                 * We do this only if the UserInfo.GUserInfo System.Object exists - this prevents running the
                 * following code by the Designer and consequently breaking the Designer
                 * because the UserInfo.GUserInfo System.Object won't exist at design time...
                 */

                // this.rpsUserControl.RegistryPath := PETRA_REGISTRY_MAIN_KEY + '\' +
                // PETRA_REGISTRY_USERS_KEY + '\' + UserInfo.GUserInfo.UserID + '\' + PETRA_REGISTRY_POSITIONS_KEY +
                // '\' + this.GetType().Name;
                // this.rpsUserControl.SetRestoreLocation(this, true);
            }
        }

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
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

        #region Helper functions

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitialiseUserControl()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void DataSavingStartedEventFired()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASuccess"></param>
        public void DataSavedEventFired(Boolean ASuccess)
        {
        }

        #endregion

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="AHelp"></param>
        public void SetStatusBarText(Control AControl, string AHelp)
        {
            // TODO SetStatusBarText
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public interface IPetraUserControl
    {
        /// <summary>
        /// todoComment
        /// </summary>
        TFrmPetraUtils PetraUtilsObject
        {
            get;
            set;
        }
    }

    /// <summary>
    /// this interface is needed to let the user control know about the utils object
    /// the user control can subscribe to action enabling events etc
    /// </summary>
    public interface IPetraEditUserControl
    {
        /// <summary>
        /// todoComment
        /// </summary>
        TFrmPetraEditUtils PetraUtilsObject
        {
            get;
            set;
        }
    }
}