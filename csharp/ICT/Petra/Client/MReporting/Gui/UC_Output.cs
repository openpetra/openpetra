//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// This helps to define where the output of the report should go (printing, email, CSV/File export)
    /// </summary>
    public partial class UC_Output : UserControl
    {
        /// <summary>
        /// constructor
        /// </summary>
        public UC_Output()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            this.grpCSVOutput.Text = Catalog.GetString("Export to CSV");
            this.Label2.Text = Catalog.GetString("Hint: If you don\'t want quotes around your values, please choose a delimiter that");
            this.Label1.Text = Catalog.GetString("(e.g. , or ; or : or Space or Tab)");
            this.lblCSVSeparator.Text = Catalog.GetString("Delimiter") + ":";
            this.chbExportToCSVOnly.Text = Catalog.GetString("Only save as CSV, don\'t print Report");
            this.BtnCSVDestination.Text = Catalog.GetString("...");
            this.lblCSVDestination.Text = Catalog.GetString("Destination file");
            #endregion

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        private TFrmPetraUtils FPetraUtilsObject;

        /// <summary>
        /// utilities for Petra forms
        /// </summary>
        public TFrmPetraUtils PetraUtilsObject
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

        private void BtnCSVDestination_Click(System.Object sender, System.EventArgs e)
        {
            if (SaveFileDialogCSV.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtCSVDestination.Text = SaveFileDialogCSV.FileName;
            }
        }

        /// <summary>
        /// read the values from the controls and give them to the calculator
        /// </summary>
        /// <param name="ACalculator"></param>
        public void ReadControls(TRptCalculator ACalculator)
        {
            ACalculator.AddParameter("SaveCSVFilename", txtCSVDestination.Text);

            if (txtCSVSeparator.Text.Length > 0)
            {
                ACalculator.AddParameter("CSV_separator", txtCSVSeparator.Text);
            }

            ACalculator.AddParameter("OnlySaveCSV", chbExportToCSVOnly.Checked);
        }

        /// <summary>
        /// initialise the controls using the parameters
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControls(TParameterList AParameters)
        {
            txtCSVDestination.Text = AParameters.Get("SaveCSVFilename").ToString();
            chbExportToCSVOnly.Checked = AParameters.Get("OnlySaveCSV").ToBool();
            txtCSVSeparator.Text = AParameters.Get("CSV_separator").ToString();
        }
    }
}