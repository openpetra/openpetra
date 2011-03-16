//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash,timop
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
using System.Collections;
using System.IO;
using System.Windows.Forms;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// Manual code for the Gift Batch export
    /// </summary>
    public partial class TFrmGiftBatchExport
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

        private Ict.Petra.Shared.MFinance.Gift.Data.GiftBatchTDS FMainDS;
        private Int32 FLedgerNumber;

        /// dataset for the whole screen
        public Ict.Petra.Shared.MFinance.Gift.Data.GiftBatchTDS MainDS
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

            cmbDateFormat.SelectedItem = TUserDefaults.GetStringDefault("Imp Date", "MDY");
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
            TUserDefaults.SetDefault("Imp Date", (String)cmbDateFormat.SelectedItem);
            TUserDefaults.SaveChangedUserDefaults();
        }

        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void ExportBatches(object sender, EventArgs e)
        {
            StreamWriter sw1 = null;

            try
            {
                String fileName = txtFilename.Text;
                String dateFormatString = cmbDateFormat.SelectedItem.ToString();

                // might be called from the main navigation window (FMainDS is null), or from the GL Batch screen (reusing MainDS)
                if (FMainDS == null)
                {
                    FMainDS = new Ict.Petra.Shared.MFinance.Gift.Data.GiftBatchTDS();
                    FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadAGiftBatch(FLedgerNumber));
                }

                Hashtable requestParams = new Hashtable();

                Int32 ALedgerNumber = 0;

                ArrayList batches = new ArrayList();

                foreach (AGiftBatchRow batch  in FMainDS.AGiftBatch.Rows)
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
                        exportThisBatch &= (batch.GlEffectiveDate >= dtpDateFrom.Date);
                        exportThisBatch &= (batch.GlEffectiveDate <= dtpDateTo.Date);
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

                requestParams.Add("ALedgerNumber", ALedgerNumber);
                requestParams.Add("Delimiter", ConvertDelimiter(cmbDelimiter.GetSelectedString(), false));
                requestParams.Add("DateFormatString", dateFormatString);
                requestParams.Add("Summary", rbtSummary.Checked);
                requestParams.Add("bUseBaseCurrency", rbtBaseCurrency.Checked);
                requestParams.Add("BaseCurrency", FMainDS.ALedger[0].BaseCurrency);
                requestParams.Add("TransactionsOnly", chkTransactionsOnly.Checked);
                requestParams.Add("RecipientNumber", Convert.ToInt64(txtDetailRecipientKey.Text));
                requestParams.Add("FieldNumber", Convert.ToInt64(txtDetailFieldKey.Text));
                requestParams.Add("DateForSummary", dtpDateSummary.Date);
                requestParams.Add("NumberFormat", ConvertNumberFormat(cmbNumberFormat));
                requestParams.Add("ExtraColumns", chkExtraColumns.Checked);

                String exportString;
                TVerificationResultCollection AMessages;

                bool completed = false;
                sw1 = new StreamWriter(fileName);
                string ErrorMessages = String.Empty;

                do
                {
                    completed = TRemote.MFinance.Gift.WebConnectors.ExportAllGiftBatchData(ref batches,
                        requestParams,
                        out exportString,
                        out AMessages);
                    sw1.Write(exportString);

                    if (AMessages.Count > 0)
                    {
                        foreach (TVerificationResult message in AMessages)
                        {
                            ErrorMessages += "[" + message.ResultContext + "] " +
                                             message.ResultTextCaption + ": " +
                                             message.ResultText + Environment.NewLine;
                        }
                    }
                }   while (!completed);

                if (ErrorMessages.Length > 0)
                {
                    System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Warning"),

                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                MessageBox.Show(Catalog.GetString("Your data was exported successfully!"),
                    Catalog.GetString("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                SaveUserDefaults();
            }
            finally
            {
                if (sw1 != null)
                {
                    sw1.Close();
                }
            }
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