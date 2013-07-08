//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Client.MFinance.Logic;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Description of GLEnterDateEffective.
    /// </summary>
    public partial class TDlgGLEnterDateEffective : Form
    {
        private DateTime FStartDateCurrentPeriod;
        private DateTime FEndDateLastForwardingPeriod;

        /// <summary>
        /// constructor
        /// </summary>
        public TDlgGLEnterDateEffective()
        {
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblDateEffective.Text = Catalog.GetString("label1");
            this.lblValidDateRange.Text = Catalog.GetString("valid dates from {0} to {1}");
            this.btnCancel.Text = Catalog.GetString("Cancel");
            this.btnOK.Text = Catalog.GetString("OK");
            this.Text = Catalog.GetString("Select the posting date");
            #endregion
        }

        /// <summary>
        /// constructor with some parameters
        /// </summary>
        public TDlgGLEnterDateEffective(Int32 ALedgerNumber, string ACaption, string ALabel)
        {
            InitializeComponent();

            DateTime DefaultDate;

            TLedgerSelection.GetCurrentPostingRangeDates(ALedgerNumber,
                out FStartDateCurrentPeriod,
                out FEndDateLastForwardingPeriod,
                out DefaultDate);
            lblDateEffective.Text = ALabel;
            this.Text = ACaption;
            lblValidDateRange.Text = String.Format(Catalog.GetString(
                    "Valid dates from {0} to {1}"),
                StringHelper.DateToLocalizedString(FStartDateCurrentPeriod),
                StringHelper.DateToLocalizedString(FEndDateLastForwardingPeriod));

            dtpDateEffective.Date = DefaultDate;
        }

        private void BtnOKClick(object sender, EventArgs e)
        {
            if (!(dtpDateEffective.ValidDate()))
            {
                return;
/* // The method above has already displayed an error.
                MessageBox.Show(Catalog.GetString(
                        "Date format not recognised. Please use dd-mmm-yy."),
                    Catalog.GetString("Invalid date"), MessageBoxButtons.OK, MessageBoxIcon.Error);
 */
            }
            else
            if ((dtpDateEffective.Date < FStartDateCurrentPeriod)
                || (dtpDateEffective.Date > FEndDateLastForwardingPeriod))
            {
                MessageBox.Show(Catalog.GetString(
                        "Please select a date which is in the valid posting range of your ledger!"),
                    Catalog.GetString("Invalid date"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        /// access the selected date
        /// </summary>
        public DateTime SelectedDate
        {
            get
            {
                return dtpDateEffective.Date.Value;
            }
            set
            {
                dtpDateEffective.Date = value;
            }
        }
    }
}