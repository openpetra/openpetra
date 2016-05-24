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
using System.Diagnostics;
using System.Globalization;

using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.CommonDialogs;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Manual code for the GL Batch export
    /// </summary>
    public partial class TFrmGLBatchExport
    {
        // This variable holds the user setting for 'Extra Columns' used by Gift Export but not by GL.
        // When we save defaults we write the value back.
        private char FGiftExtraColumnsUserDefault = '-';

        /// <summary>
        /// Initialize values
        /// </summary>
        public void InitializeManualCode()
        {
            // Let the user change his/her mind and clear the box
            cmbDontSummarizeAccount.cmbCombobox.AllowBlankValue = true;

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
                this.Text += " - " + TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
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
            return ACmb.SelectedIndex == 0 ? TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN : TDlgSelectCSVSeparator.NUMBERFORMAT_EUROPEAN;
        }

        private void LoadUserDefaults()
        {
            // This is for compatibility with old Petra
            txtFilename.Text = TUserDefaults.GetStringDefault("Imp Filename",
                TClientSettings.GetExportPath() + Path.DirectorySeparatorChar + "export.csv");
            String expOptions = TUserDefaults.GetStringDefault("Exp Options", "DU-T-X-DTrans");

            // This is for compatibility with old Petra
            if (expOptions.StartsWith("D"))
            {
                rbtDetail.Checked = true;
            }
            else
            {
                rbtSummary.Checked = true;
            }

            if (expOptions.EndsWith("Trans"))
            {
                rbtOriginalTransactionCurrency.Checked = true;
            }
            else
            {
                rbtBaseCurrency.Checked = true;
            }

            if (expOptions.Length > 11)
            {
                // Extended options
                if (expOptions[1] == 'U')
                {
                    chkIncludeUnposted.Checked = (expOptions[2] == '+');
                }

                if (expOptions[3] == 'T')
                {
                    chkTransactionsOnly.Checked = (expOptions[4] == '+');
                }

                if (expOptions[5] == 'X')
                {
                    FGiftExtraColumnsUserDefault = expOptions[6];
                }

                if (expOptions[7] == 'N')
                {
                    rbtBatchNumberSelection.Checked = true;
                }
                else
                {
                    rbtDateRange.Checked = true;
                }
            }

            CultureInfo myCulture = Thread.CurrentThread.CurrentCulture;
            string defaultImpOptions = myCulture.TextInfo.ListSeparator + TDlgSelectCSVSeparator.NUMBERFORMAT_EUROPEAN;

            if (myCulture.EnglishName.EndsWith("-US"))
            {
                defaultImpOptions = myCulture.TextInfo.ListSeparator + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN;
            }

            String impOptions = TUserDefaults.GetStringDefault("Imp Options", defaultImpOptions);

            if (impOptions.Length > 0)
            {
                cmbDelimiter.SelectedItem = ConvertDelimiter(impOptions.Substring(0, 1), true);
            }

            if (impOptions.Length > 1)
            {
                cmbNumberFormat.SelectedIndex = impOptions.Substring(1) == TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN ? 0 : 1;
            }

            string DateFormat = TUserDefaults.GetStringDefault("Imp Date", "yyyy-MM-dd");

            // mdy and dmy have been the old default settings in Petra 2.x
            if (DateFormat.ToLower() == "mdy")
            {
                DateFormat = "MM/dd/yyyy";
            }

            if (DateFormat.ToLower() == "dmy")
            {
                DateFormat = "dd/MM/yyyy";
            }

            cmbDateFormat.SetSelectedString(DateFormat);
        }

        private void SaveUserDefaults()
        {
            TUserDefaults.SetDefault("Imp Filename", txtFilename.Text);

            String expOptions = (rbtDetail.Checked) ? "D" : "S";
            expOptions += (chkIncludeUnposted.Checked) ? "U+" : "U-";
            expOptions += (chkTransactionsOnly.Checked) ? "T+" : "T-";
            expOptions += (FGiftExtraColumnsUserDefault == '+') ? "X+" : "X-";
            expOptions += (rbtBatchNumberSelection.Checked) ? "N" : "D";
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
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (ExportBatches())
            {
                // We are done so we quit
                Close();
            }
        }

        /// <summary>
        /// Public method to export GL batches
        /// </summary>
        /// <returns>True if the Export succeeded and a file was created, false otherwise</returns>
        public bool ExportBatches(bool AWithInteractionOnSuccess = true)
        {
            string ExportFileName = txtFilename.Text;

            if (ExportFileName == String.Empty)
            {
                MessageBox.Show(Catalog.GetString("Please choose a location for the Export File."),
                    Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            else if (!ExportFileName.EndsWith(".csv",
                         StringComparison.CurrentCultureIgnoreCase) && !ExportFileName.EndsWith(".txt", StringComparison.CurrentCultureIgnoreCase))
            {
                ExportFileName += ".csv";
                txtFilename.Text = ExportFileName;
            }

            if (!Directory.Exists(Path.GetDirectoryName(ExportFileName)))
            {
                MessageBox.Show(Catalog.GetString("Please select an existing directory for this file!"),
                    Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                txtFilename.Text = string.Empty;
                return false;
            }

            if (File.Exists(ExportFileName))
            {
                if (MessageBox.Show(Catalog.GetString("The file already exists. Is it OK to overwrite it?"),
                        Catalog.GetString("Export Batches"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    return false;
                }

                try
                {
                    File.Delete(ExportFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(
                            Catalog.GetString(
                                "Failed to delete the file. Maybe it is already open in another application?  The system message was:{0}{1}"),
                            Environment.NewLine, ex.Message),
                        Catalog.GetString("Export GL Batches"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }

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
                    return false;
                }
            }

            String numberFormat = ConvertNumberFormat(cmbNumberFormat);
            String delimiter = ConvertDelimiter(cmbDelimiter.GetSelectedString(), false);

            if (((numberFormat == "European") && (delimiter == ",")) || ((numberFormat == "American") && (delimiter == ".")))
            {
                MessageBox.Show(Catalog.GetString("Numeric Decimal cannot be the same as the delimiter."),
                    Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            // Save the defaults
            SaveUserDefaults();

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
                return false;
            }

            // Do the actual export work
            try
            {
                Hashtable requestParams = new Hashtable();
                requestParams.Add("ALedgerNumber", ALedgerNumber);
                requestParams.Add("Delimiter", delimiter);
                requestParams.Add("DateFormatString", dateFormatString);
                requestParams.Add("Summary", rbtSummary.Checked);
                requestParams.Add("bUseBaseCurrency", rbtBaseCurrency.Checked);
                requestParams.Add("BaseCurrency", FMainDS.ALedger[0].BaseCurrency);
                requestParams.Add("TransactionsOnly", chkTransactionsOnly.Checked);
                requestParams.Add("bDontSummarize", chkDontSummarize.Checked);
                requestParams.Add("DontSummarizeAccount", cmbDontSummarizeAccount.GetSelectedString());
                requestParams.Add("DateForSummary", dtpDateSummary.Date);
                requestParams.Add("NumberFormat", numberFormat);

                String exportString = null;
                Thread ExportThread = new Thread(() => ExportAllGLBatchData(batches, requestParams, out exportString));
                using (TProgressDialog ExportDialog = new TProgressDialog(ExportThread))
                {
                    ExportDialog.ShowDialog();
                }

                // Now we have the string we can write it to the file
                StreamWriter sw1 = new StreamWriter(ExportFileName);
                sw1.Write(exportString);
                sw1.Close();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            bool ShowExportedFileInExplorer = false;

            if (AWithInteractionOnSuccess)
            {
                if (MessageBox.Show(Catalog.GetString(
                            "GL Batches Exported successfully. Would you like to open the file in your default application?"),
                        Catalog.GetString("GL Batch Export"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        ProcessStartInfo si = new ProcessStartInfo(ExportFileName);
                        si.UseShellExecute = true;
                        si.Verb = "open";

                        Process p = new Process();
                        p.StartInfo = si;
                        p.Start();
                    }
                    catch
                    {
                        MessageBox.Show(Catalog.GetString(
                                "Unable to launch the default application to open: '") + ExportFileName + "'!", Catalog.GetString(
                                "GL Export"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        ShowExportedFileInExplorer = true;
                    }
                }
            }
            else
            {
                ShowExportedFileInExplorer = true;
            }

            if (ShowExportedFileInExplorer)
            {
                //If windows start Windows File Explorer
                TExecutingOSEnum osVersion = Utilities.DetermineExecutingOS();

                if ((osVersion >= TExecutingOSEnum.eosWinXP)
                    && (osVersion < TExecutingOSEnum.oesUnsupportedPlatform))
                {
                    try
                    {
                        Process.Start("explorer.exe", string.Format("/select,\"{0}\"", ExportFileName));
                    }
                    catch
                    {
                        MessageBox.Show(Catalog.GetString(
                                "Unable to launch Windows File Explorer to open: '") + ExportFileName + "'!", Catalog.GetString(
                                "GL Export"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Wrapper method to handle returned bool value from remoting call to ExportAllGLBatchData
        /// </summary>
        /// <param name="Abatches"></param>
        /// <param name="ArequestParams"></param>
        /// <param name="exportString"></param>
        private void ExportAllGLBatchData(
            ArrayList Abatches,
            Hashtable ArequestParams,
            out string exportString)
        {
            string AexportString;
            bool Acompleted = false;

            do
            {
                Acompleted = TRemote.MFinance.GL.WebConnectors.ExportAllGLBatchData(
                    Abatches,
                    ArequestParams,
                    out AexportString);
            } while (!Acompleted);

            exportString = AexportString;
        }

        void BtnBrowseClick(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Text Files (*.txt)|*.txt|Delimited Files (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 3;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFilename.Text = saveFileDialog1.FileName;
            }
        }
    }
}