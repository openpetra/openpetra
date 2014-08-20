//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash,timop
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
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Threading;

using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Client.CommonDialogs;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Manual code for the GL Batch export
    /// </summary>
    public partial class TFrmGLBatchExport
    {
        /// <summary>
        /// Initialize values
        /// </summary>
        public void InitializeManualCode()
        {
            txtBatchNumberStart.NumberValueInt = 0;
            txtBatchNumberEnd.NumberValueInt = 999999;
            DateTime now = DateTime.Now;

            DateTime firstDayMonth = new DateTime(now.Year, now.Month, 1);
            dtpDateFrom.Date = firstDayMonth.AddMonths(-1);
            dtpDateTo.Date = firstDayMonth.AddDays(-1);
            dtpDateSummary.Date = dtpDateTo.Date;
            System.Globalization.CultureInfo myCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            String regionalDateString = myCulture.DateTimeFormat.ShortDatePattern;

            if (!cmbDateFormat.Items.Contains(regionalDateString))
            {
                cmbDateFormat.Items.Insert(0, regionalDateString);
            }

            LoadUserDefaults();
        }

        private Ict.Petra.Shared.MFinance.GL.Data.GLBatchTDS FMainDS;
        private Int32 FLedgerNumber;

        /// dataset for the whole screen
        public Ict.Petra.Shared.MFinance.GL.Data.GLBatchTDS MainDS
        {
            set
            {
                FMainDS = value;
            }
        }

        /// the ledger that the user is currently working with
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                TFinanceControls.InitialiseAccountList(ref cmbDontSummarizeAccount, FLedgerNumber, true, false, false, false);
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
            String expOptions = TUserDefaults.GetStringDefault("Exp Options", "DTrans");

            // This is for compatibility with old Petra
            if (expOptions.StartsWith("D"))
            {
                rbtDetail.Select();
            }
            else
            {
                rbtSummary.Select();
            }

            if (expOptions.EndsWith("Trans"))
            {
                rbtOriginalTransactionCurrency.Select();
            }
            else
            {
                rbtBaseCurrency.Select();
            }

            String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";American");

            if (impOptions.Length > 0)
            {
                cmbDelimiter.SelectedItem = ConvertDelimiter(impOptions.Substring(0, 1), true);
            }

            if (impOptions.Length > 1)
            {
                cmbNumberFormat.SelectedIndex = impOptions.Substring(1) == "American" ? 0 : 1;
            }

            string DateFormatDefault = TUserDefaults.GetStringDefault("Imp Date", "yyyy-MM-dd");

            // mdy and dmy have been the old default settings in Petra 2.x
            if (DateFormatDefault.ToLower() == "mdy")
            {
                DateFormatDefault = "MM/dd/yyyy";
            }

            if (DateFormatDefault.ToLower() == "dmy")
            {
                DateFormatDefault = "dd/MM/yyyy";
            }

            cmbDateFormat.SetSelectedString(DateFormatDefault);
        }

        private void SaveUserDefaults()
        {
            TUserDefaults.SetDefault("Imp Filename", txtFilename.Text);

            String expOptions = (rbtDetail.Checked) ? "D" : "S";
            expOptions += (rbtOriginalTransactionCurrency.Checked) ? "Trans" : "Base";
            TUserDefaults.SetDefault("Exp Options", expOptions);
            String impOptions = ConvertDelimiter((String)cmbDelimiter.SelectedItem, false);
            impOptions += ConvertNumberFormat(cmbNumberFormat);
            TUserDefaults.SetDefault("Imp Options", impOptions);
            TUserDefaults.SetDefault("Imp Date", cmbDateFormat.GetSelectedString());
            TUserDefaults.SaveChangedUserDefaults();
        }

        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void ExportBatches(object sender, EventArgs e)
        {
            if (rbtBatchNumberSelection.Checked)
            {
                if (!txtBatchNumberStart.NumberValueInt.HasValue)
                {
                    txtBatchNumberStart.NumberValueInt = 0;
                }

                if (!txtBatchNumberEnd.NumberValueInt.HasValue)
                {
                    txtBatchNumberEnd.NumberValueInt = 999999;
                }
            }
            else
            {
                if ((!dtpDateFrom.ValidDate()) || (!dtpDateTo.ValidDate()))
                {
                    return;
                }
            }

            if (File.Exists(txtFilename.Text))
            {
                if (MessageBox.Show(Catalog.GetString("The file already exists. Is it OK to overwrite it?"),
                        Catalog.GetString("Export Batches"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            StreamWriter sw1 = null;
            try
            {
                sw1 = new StreamWriter(txtFilename.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    Catalog.GetString("Failed to open file"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            String dateFormatString = cmbDateFormat.GetSelectedString();

            // might be called from the main navigation window (FMainDS is null), or from the GL Batch screen (reusing MainDS)
            if (FMainDS == null)
            {
                FMainDS = new Ict.Petra.Shared.MFinance.GL.Data.GLBatchTDS();
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, -1, -1));
            }

            Int32 ALedgerNumber = 0;

            ArrayList batches = new ArrayList();

            foreach (ABatchRow batch in FMainDS.ABatch.Rows)
            {
                // check conditions for exporting this batch
                // Batch Status
                bool exportThisBatch = batch.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED)
                                       || (chkIncludeUnposted.Checked && batch.BatchStatus.Equals(MFinanceConstants.BATCH_UNPOSTED));

                if (rbtBatchNumberSelection.Checked)
                {
                    exportThisBatch &= (batch.BatchNumber >= txtBatchNumberStart.NumberValueInt);
                    exportThisBatch &= (batch.BatchNumber <= txtBatchNumberEnd.NumberValueInt);
                }
                else
                {
                    exportThisBatch &= (batch.DateEffective >= dtpDateFrom.Date);
                    exportThisBatch &= (batch.DateEffective <= dtpDateTo.Date);
                }

                if (exportThisBatch)
                {
                    batches.Add(batch.BatchNumber);
                }

                ALedgerNumber = batch.LedgerNumber;
            }

            if (batches.Count == 0)
            {
                MessageBox.Show(Catalog.GetString("There are no batches matching your criteria"),
                    Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Hashtable requestParams = new Hashtable();
            requestParams.Add("ALedgerNumber", ALedgerNumber);
            requestParams.Add("Delimiter", ConvertDelimiter(cmbDelimiter.GetSelectedString(), false));
            requestParams.Add("DateFormatString", dateFormatString);
            requestParams.Add("Summary", rbtSummary.Checked);
            requestParams.Add("bUseBaseCurrency", rbtBaseCurrency.Checked);
            requestParams.Add("BaseCurrency", FMainDS.ALedger[0].BaseCurrency);
            requestParams.Add("TransactionsOnly", chkTransactionsOnly.Checked);
            requestParams.Add("bDontSummarize", chkDontSummarize.Checked);
            requestParams.Add("DontSummarizeAccount", cmbDontSummarizeAccount.GetSelectedString());
            requestParams.Add("DateForSummary", dtpDateSummary.Date);
            requestParams.Add("NumberFormat", ConvertNumberFormat(cmbNumberFormat));

            String exportString = null;

            Thread ExportThread = new Thread(() => ExportAllGLBatchData(ref batches, requestParams, out exportString));
            using (TProgressDialog ExportDialog = new TProgressDialog(ExportThread))
            {
                ExportDialog.ShowDialog();
            }

            sw1.Write(exportString);
            sw1.Close();

            MessageBox.Show(Catalog.GetString("Your data was exported successfully!"),
                Catalog.GetString("Success"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            SaveUserDefaults();
        }

        /// <summary>
        /// Wrapper method to handle returned bool value from remoting call to ExportAllGLBatchData
        /// </summary>
        /// <param name="Abatches"></param>
        /// <param name="ArequestParams"></param>
        /// <param name="exportString"></param>
        private void ExportAllGLBatchData(
            ref ArrayList Abatches,
            Hashtable ArequestParams,
            out string exportString)
        {
            string AexportString;
            bool Acompleted = false;

            do
            {
                Acompleted = TRemote.MFinance.GL.WebConnectors.ExportAllGLBatchData(
                    ref Abatches,
                    ArequestParams,
                    out AexportString);
            } while (!Acompleted);

            exportString = AexportString;
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
    }
}