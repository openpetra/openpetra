//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using GNU.Gettext;

using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// UserControl for displaying Partner Info data in the Partner
    /// Find screen.
    /// </summary>
    public partial class TUC_PartnerFind_PartnerInfo : TPetraUserControl, IPartnerInfoMethods
    {
        /// <summary>todoComment</summary>
        private const Int16 COLLAPSEDHEIGHT = 29;

        /// <summary>todoComment</summary>
        private const Int16 EXPANDEDHEIGHT = 153;

        private bool FIsCollapsed = false;

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_PartnerFind_PartnerInfo()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblDetailHeading.Text = Catalog.GetString("Partner Info");
            #endregion

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        /// <summary>
        /// Stops the Timer for the fetching of data. This is needed to allow a garbage-collection of the UC_PartnerInfo UserControl
        /// (a System.Threading.Timer is an unmanaged resource and needs manual Disposal, plus it holds a reference to the Class that instantiated it!)
        /// </summary>
        public void StopTimer()
        {
            ucoPartnerInfo.StopTimer();
        }

        /// <summary>todoComment</summary>
        public bool IsCollapsed
        {
            get
            {
                return FIsCollapsed;
            }

            set
            {
                FIsCollapsed = value;
            }
        }

        /// <summary>todoComment</summary>
        public int CollapsedHeight
        {
            get
            {
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    return (int)(COLLAPSEDHEIGHT * ((double)this.ParentForm.AutoScaleBaseSize.Width /
                                                    (double)PetraForm.AUTOSCALEBASESIZEWIDTHFOR96DPI));
                }
                else
                {
                    return COLLAPSEDHEIGHT;
                }
            }
        }

        /// <summary>todoComment</summary>
        public int ExpandedHeight
        {
            get
            {
                int HeightOnNonStandardDPI;

                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    HeightOnNonStandardDPI = (int)(EXPANDEDHEIGHT * ((double)this.ParentForm.AutoScaleBaseSize.Width /
                                                                     (double)PetraForm.AUTOSCALEBASESIZEWIDTHFOR96DPI));

                    // We only need 96% of that calculated value - otherwise it's to high (at least at 120DPI)...
                    return (HeightOnNonStandardDPI / 100) * 96;
                }
                else
                {
                    return EXPANDEDHEIGHT;
                }
            }
        }

        /// <summary>todoComment</summary>
        public event System.EventHandler Collapsed;

        /// <summary>todoComment</summary>
        public event System.EventHandler Expanded;

        private void OnCollapsed()
        {
            if (Collapsed != null)
            {
                Collapsed(this, new EventArgs());
            }
        }

        private void OnExpanded()
        {
            if (Expanded != null)
            {
                Expanded(this, new EventArgs());
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void Collapse()
        {
            FIsCollapsed = true;

            btnTogglePartnerDetails.ImageIndex = 1;
            this.Height = this.CollapsedHeight;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void Expand()
        {
            FIsCollapsed = false;

            btnTogglePartnerDetails.ImageIndex = 0;
            this.Height = this.ExpandedHeight;
        }

        #region Event Handlers

        private void BtnTogglePartnerDetailsClick(object sender, EventArgs e)
        {
            if (FIsCollapsed)
            {
                Expand();
                OnExpanded();
            }
            else
            {
                Collapse();
                OnCollapsed();
            }
        }

        void BtnTogglePartnerDetailsMouseEnter(object sender, EventArgs e)
        {
//            TLogging.Log("BtnTogglePartnerDetailsMouseEnter: btnTogglePartnerDetails.ImageIndex: " + btnTogglePartnerDetails.ImageIndex.ToString());
            if (btnTogglePartnerDetails.ImageIndex == 0)
            {
                btnTogglePartnerDetails.ImageIndex = 3;
            }
            else
            {
                btnTogglePartnerDetails.ImageIndex = 2;
            }

            lblDetailHeading.ForeColor = System.Drawing.Color.RoyalBlue;      // RoyalBlue;  SteelBlue     // Blue
//            TLogging.Log("BtnTogglePartnerDetailsMouseEnter END: btnTogglePartnerDetails.ImageIndex: " + btnTogglePartnerDetails.ImageIndex.ToString());
        }

        void BtnTogglePartnerDetailsMouseLeave(object sender, EventArgs e)
        {
//            TLogging.Log("BtnTogglePartnerDetailsMouseLeave: btnTogglePartnerDetails.ImageIndex: " + btnTogglePartnerDetails.ImageIndex.ToString());
            if (btnTogglePartnerDetails.ImageIndex == 3)
            {
                btnTogglePartnerDetails.ImageIndex = 0;
            }
            else if (btnTogglePartnerDetails.ImageIndex != 0)
            {
                btnTogglePartnerDetails.ImageIndex = 1;
            }

            lblDetailHeading.ForeColor = System.Drawing.Color.MediumBlue;  // Blue     // DarkBlue
//            TLogging.Log("BtnTogglePartnerDetailsMouseLeave END: btnTogglePartnerDetails.ImageIndex: " + btnTogglePartnerDetails.ImageIndex.ToString());
        }

        #endregion

        #region Method calls that get passed through to ucoPartnerInfo

        /// <summary>
        /// todoComment
        /// </summary>
        public void ClearControls()
        {
            ucoPartnerInfo.ClearControls();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerDR"></param>
        public void PassPartnerDataPartialWithLocation(Int64 APartnerKey, DataRow APartnerDR)
        {
            ucoPartnerInfo.PassPartnerDataPartialWithLocation(APartnerKey, APartnerDR);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerDR"></param>
        public void PassPartnerDataPartialWithoutLocation(Int64 APartnerKey, DataRow APartnerDR)
        {
            ucoPartnerInfo.PassPartnerDataPartialWithoutLocation(APartnerKey, APartnerDR);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerDR"></param>
        public void PassPartnerDataFull(Int64 APartnerKey, DataRow APartnerDR)
        {
            ucoPartnerInfo.PassPartnerDataFull(APartnerKey, APartnerDR);
        }

        #endregion
    }
}