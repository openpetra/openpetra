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
using System.Windows.Forms;
using Mono.Unix;
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
        public const Int16 COLLAPSEDHEIGHT = 29;

        /// <summary>todoComment</summary>
        public const Int16 EXPANDEDHEIGHT = 153;

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
            this.btnTogglePartnerDetails.Text = Catalog.GetString("v");
            #endregion

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
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

            this.btnTogglePartnerDetails.Text = "^";
            this.Height = COLLAPSEDHEIGHT;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void Expand()
        {
            FIsCollapsed = false;

            this.btnTogglePartnerDetails.Text = "v";
            this.Height = EXPANDEDHEIGHT;
        }

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