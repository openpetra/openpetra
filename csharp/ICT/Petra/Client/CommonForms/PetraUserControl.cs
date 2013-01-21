//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
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
    public partial class TPetraUserControl : System.Windows.Forms.UserControl
    {
        private System.Windows.Forms.ToolTip tipUC;

        /// <summary>
        /// property for setting the tooltip.
        /// needed to avoid warning about unused variable tipUC
        /// </summary>
        public string ToolTip
        {
            set
            {
                tipUC.SetToolTip(this, value);
            }
        }

        /// <summary>Holds the DataSet that contains most data that is used in the UserControl</summary>
        protected DataSet FMainDS;

        /// <summary>use this property to exclude controls from being hooked up to the automatic value changed event</summary>
        protected bool FCanBeHookedUpForValueChangedEvent = true;

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

        /// <summary>use this property to exclude controls from being hooked up to the automatic value changed event</summary>
        public bool CanBeHookedUpForValueChangedEvent
        {
            get
            {
                return FCanBeHookedUpForValueChangedEvent;
            }

            set
            {
                FCanBeHookedUpForValueChangedEvent = value;
            }
        }


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
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion

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

        #region Helper functions

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

        /// <summary>
        /// used to initialize the user control
        /// </summary>
        void InitUserControl();
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

        /// <summary>
        /// used to initialize the user control
        /// </summary>
        void InitUserControl();
    }
}