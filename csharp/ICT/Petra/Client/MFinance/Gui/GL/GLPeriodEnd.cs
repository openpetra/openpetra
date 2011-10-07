//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Specification of TPeriodEnd to handle the month-to-month switch.
    /// </summary>
    public class TPeriodEndMonth : TPeriodEnd
    {
        /// <summary>
        /// The constructor is the typical constructor which shall be supported by
        /// refering this object in UINavigation.yml and AParentFormHandle
        /// is used to put into TPeriodEnd, adding a boolean constant to
        /// switch to the EndMonthMode
        /// </summary>
        /// <param name="AParentForm"></param>
        public TPeriodEndMonth(Form AParentForm) : base(AParentForm, true)
        {
        }
    }

    /// <summary>
    /// Specification of TPeriodEnd to handle the year-to-first-month switch.
    /// </summary>
    public class TPeriodEndYear : TPeriodEnd
    {
        /// <summary>
        /// The constructor is the typical constructor which shall be supported by
        /// refering this object in UINavigation.yml and AParentFormHandle
        /// is used to put into TPeriodEnd, adding a boolean constant to
        /// switch to the EndYearMode
        /// </summary>
        /// <param name="AParentForm"></param>
        public TPeriodEndYear(Form AParentForm) : base(AParentForm, false)
        {
        }
    }


    /// <summary>
    /// Main form to handle the period ends like Month and Year ...
    /// </summary>
    public partial class TPeriodEnd : Form
    {
        bool blnIsInMonthMode;


        /// <summary>
        /// Non UINavigation.yml standard constructor
        /// </summary>
        /// <param name="AParentForm">Standard parameter</param>
        /// <param name="AIsMonthMode">true = month-to-month mode,
        /// false = year-to-month mode</param>
        public TPeriodEnd(Form AParentForm, bool AIsMonthMode) : base()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //

            blnIsInMonthMode = AIsMonthMode;

            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion


            this.btnCancel.Click += new EventHandler(CancelButtonClick);
            this.btnPeriodEnd.Click += new EventHandler(PeriodEndButtonClick);

            this.ResizeEnd += new EventHandler(ResizeForm);
        }
    }
}