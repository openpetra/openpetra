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
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// UserControl for displaying Partner Info data in the Partner
    /// Find screen.
    /// </summary>
    public partial class TUC_PartnerFind_PartnerInfo : TPnlCollapsible, IPartnerInfoMethods
    {
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