//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
//
// Copyright 2004-2012 by OM International
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
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance;
//using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Client.MFinance.Gui.Budget
{
    public partial class TFrmExportBudget
    {
        /// <summary>
        /// Initialize values
        /// </summary>
        public void InitializeManualCode()
        {
            System.Globalization.CultureInfo myCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            String regionalDateString = myCulture.DateTimeFormat.ShortDatePattern;

            if (!cmbDateFormat.Items.Contains(regionalDateString))
            {
                cmbDateFormat.Items.Insert(0, regionalDateString);
            }

            LoadUserDefaults();
        }

        private Int32 FLedgerNumber;

        /// the ledger that the user is currently working with
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                //TFinanceControls.InitialiseAccountList(ref cmbDontSummarizeAccount, FLedgerNumber, true, false, false, false);
            }
        }

        const String sSpace = "[SPACE]";
        private String ConvertDelimiter(String Delimiter, bool displayform)
        {
            if (Delimiter.Equals(sSpace) || Delimiter.Equals(" "))
            {
                Delimiter = displayform ? sSpace : " ";
            }

            return Delimiter;
        }

        private String ConvertNumberFormat(ComboBox ACmb)
        {
            return ACmb.SelectedIndex == 0 ? "American" : "European";
        }

        private void LoadUserDefaults()
        {
            // This is for compatibility with old Petra
            txtFilename.Text = TUserDefaults.GetStringDefault("Imp Filename",
                TClientSettings.GetExportPath() + Path.DirectorySeparatorChar + "export.csv");

            //TODO: unrem when adding export behaviour
            //String expOptions = TUserDefaults.GetStringDefault("Exp Options", "DTrans");

            String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";American");

            if (impOptions.Length > 0)
            {
                cmbDelimiter.SetSelectedString(ConvertDelimiter(impOptions.Substring(0, 1), true));
            }

            if (impOptions.Length > 1)
            {
                cmbNumberFormat.SelectedIndex = impOptions.Substring(1) == "American" ? 0 : 1;
            }

            cmbDateFormat.SetSelectedString(TUserDefaults.GetStringDefault("Imp Date", "MDY"));
        }

        private void SaveUserDefaults()
        {
            TUserDefaults.SetDefault("Imp Filename", txtFilename.Text);

            String expOptions = "";
            TUserDefaults.SetDefault("Exp Options", expOptions);
            String impOptions = ConvertDelimiter((String)cmbDelimiter.SelectedItem, false);
            impOptions += ConvertNumberFormat(cmbNumberFormat);
            TUserDefaults.SetDefault("Imp Options", impOptions);
            TUserDefaults.SetDefault("Imp Date", (String)cmbDateFormat.SelectedItem);
            TUserDefaults.SaveChangedUserDefaults();
        }

        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void ExportBudget(object sender, EventArgs e)
        {
            String fileName = txtFilename.Text;

            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                MessageBox.Show(Catalog.GetString("Please select an existing directory for this file!"),
                    Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Hashtable requestParams = new Hashtable();

            requestParams.Add("ALedgerNumber", FLedgerNumber);
            requestParams.Add("Delimiter", ConvertDelimiter(cmbDelimiter.GetSelectedString(), false));
            requestParams.Add("DateFormatString", cmbDateFormat.GetSelectedString());
            requestParams.Add("NumberFormat", ConvertNumberFormat(cmbNumberFormat));

            String exportString;
            TVerificationResultCollection AMessages;


            Int32 BatchCount = TRemote.MFinance.Gift.WebConnectors.ExportAllGiftBatchData(
                requestParams,
                out exportString,
                out AMessages);

            if (AMessages.Count > 0)
            {
                if (AMessages.HasCriticalErrors)
                {
                    MessageBox.Show(AMessages.BuildVerificationResultString(), Catalog.GetString("Error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
                else
                {
                    MessageBox.Show(AMessages.BuildVerificationResultString(), Catalog.GetString("Warnings"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }

            if (BatchCount == 0)
            {
                MessageBox.Show(Catalog.GetString("There are no Budgets matching your criteria"),
                    Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            StreamWriter sw1 = null;

            try
            {
                sw1 = new StreamWriter(fileName);
                sw1.Write(exportString);
            }
            finally
            {
                if (sw1 != null)
                {
                    sw1.Close();
                }
            }

            MessageBox.Show(Catalog.GetString("Data exported successfully!"),
                Catalog.GetString("Success"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            SaveUserDefaults();
        }

        void BtnBrowseClick(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFilename.Text = saveFileDialog1.FileName;
            }
        }

        void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        void BtnHelpClick(object sender, EventArgs e)
        {
            // TODO
        }

        void BtnRecipientClick(object sender, EventArgs e)
        {
            // TODO
        }

        void BtnFieldClick(object sender, EventArgs e)
        {
            // TODO
        }
    }
}