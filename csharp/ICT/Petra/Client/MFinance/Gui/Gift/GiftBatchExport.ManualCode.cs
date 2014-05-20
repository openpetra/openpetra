//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash,timop,dougm
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
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonDialogs;

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
            CultureInfo myCulture = Thread.CurrentThread.CurrentCulture;
            String regionalDateString = myCulture.DateTimeFormat.ShortDatePattern;

            if (!cmbDateFormat.Items.Contains(regionalDateString))
            {
                cmbDateFormat.Items.Insert(0, regionalDateString);
            }

            txtDetailFieldKey.PartnerClass = "UNIT";
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

        /// set the batch number range
        public Int32 FirstBatchNumber
        {
            set
            {
                rbtBatchNumberSelection.Checked = true;
                txtBatchNumberStart.NumberValueInt = value;
            }
        }

        /// set the batch number range
        public Int32 LastBatchNumber
        {
            set
            {
                rbtBatchNumberSelection.Checked = true;
                txtBatchNumberEnd.NumberValueInt = value;
            }
        }

        /// set whether unposted batches should be exported as well
        public bool IncludeUnpostedBatches
        {
            set
            {
                chkIncludeUnposted.Checked = value;
            }
        }

        /// set whether only transactions should be exported, without the batch information
        public bool TransactionsOnly
        {
            set
            {
                chkTransactionsOnly.Checked = value;
            }
        }

        /// set whether extra columns should be exported. there are two formats
        public bool ExtraColumns
        {
            set
            {
                chkExtraColumns.Checked = value;
            }
        }

        /// set the output filename
        public string OutputFilename
        {
            set
            {
                txtFilename.Text = value;
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

            CultureInfo myCulture = Thread.CurrentThread.CurrentCulture;

            string defaultImpOptions = myCulture.TextInfo.ListSeparator + "European";

            if (myCulture.EnglishName.EndsWith("-US"))
            {
                defaultImpOptions = myCulture.TextInfo.ListSeparator + "American";
            }

            String impOptions = TUserDefaults.GetStringDefault("Imp Options", defaultImpOptions);

            if (impOptions.Length > 0)
            {
                cmbDelimiter.SetSelectedString(ConvertDelimiter(impOptions.Substring(0, 1), true));
            }

            if (impOptions.Length > 1)
            {
                cmbNumberFormat.SelectedIndex = impOptions.Substring(1) == "American" ? 0 : 1;
            }

            cmbDateFormat.SetSelectedString(TUserDefaults.GetStringDefault("Imp Date",
                    myCulture.EnglishName.EndsWith("-US") ? "MDY" : "DMY"));
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
        public void ExportBatches(object sender, EventArgs e)
        {
            if (File.Exists(txtFilename.Text))
            {
                if (MessageBox.Show(Catalog.GetString("The file already exists. Is it OK to overwrite it?"),
                        Catalog.GetString("Export Gifts"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            StreamWriter sw1 = null;

            try
            {
                sw1 = new StreamWriter(txtFilename.Text,
                    false,
                    Encoding.GetEncoding(TAppSettingsManager.GetInt32("ExportGiftBatchEncoding", 1252)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    Catalog.GetString("Failed to open file"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            try
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
                    if ((dtpDateFrom.Text == "") || (dtpDateTo.Text == ""))
                    {
                        MessageBox.Show(Catalog.GetString("Start and end dates must be provided."),
                            Catalog.GetString("Error"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    if ((!dtpDateFrom.ValidDate()) || (!dtpDateTo.ValidDate()))  // If ValidDate fails, it displays a helpful message.
                    {
                        return;
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
                    return;
                }

                Hashtable requestParams = new Hashtable();
                requestParams.Add("ALedgerNumber", FLedgerNumber);
                requestParams.Add("Delimiter", ConvertDelimiter(cmbDelimiter.GetSelectedString(), false));
                requestParams.Add("DateFormatString", cmbDateFormat.GetSelectedString());
                requestParams.Add("Summary", rbtSummary.Checked);
                requestParams.Add("IncludeUnposted", chkIncludeUnposted.Checked);
                requestParams.Add("bUseBaseCurrency", rbtBaseCurrency.Checked);
                requestParams.Add("TransactionsOnly", chkTransactionsOnly.Checked);
                requestParams.Add("RecipientNumber", Convert.ToInt64(txtDetailRecipientKey.Text));
                requestParams.Add("FieldNumber", Convert.ToInt64(txtDetailFieldKey.Text));
                requestParams.Add("DateForSummary", dtpDateSummary.Date);
                requestParams.Add("NumberFormat", ConvertNumberFormat(cmbNumberFormat));
                requestParams.Add("ExtraColumns", chkExtraColumns.Checked);

                if (rbtBatchNumberSelection.Checked)
                {
                    requestParams.Add("BatchNumberStart", txtBatchNumberStart.NumberValueInt);
                    requestParams.Add("BatchNumberEnd", txtBatchNumberEnd.NumberValueInt);
                }
                else
                {
                    requestParams.Add("BatchDateFrom", dtpDateFrom.Date);
                    requestParams.Add("BatchDateTo", dtpDateTo.Date);
                }

                TVerificationResultCollection AMessages = new TVerificationResultCollection();
                String exportString = null;
                Int32 BatchCount = 0;

                Thread ExportThread = new Thread(() => ExportAllGiftBatchData(
                        requestParams,
                        out exportString,
                        out AMessages,
                        out BatchCount));

                using (TProgressDialog ExportDialog = new TProgressDialog(ExportThread))
                {
                    ExportDialog.ShowDialog();
                }

                if ((AMessages != null)
                    && (AMessages.Count > 0))
                {
                    if (!TVerificationHelper.IsNullOrOnlyNonCritical(AMessages))
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
                    MessageBox.Show(Catalog.GetString("There are no batches matching your criteria"),
                        Catalog.GetString("Error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                sw1.Write(exportString);
                sw1.Close();

                SaveUserDefaults();
                MessageBox.Show(Catalog.GetString("Gift Batches Exported successfully."),
                    Catalog.GetString("Gift Batch Export"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                sw1.Close();
            }
        }

        /// <summary>
        /// Wrapper method to handle returned BatchCount value from remoting call to ExportAllGiftBatchData
        /// </summary>
        /// <param name="ARequestParams"></param>
        /// <param name="exportString"></param>
        /// <param name="AMessages"></param>
        /// <param name="BatchCount"></param>
        private void ExportAllGiftBatchData(
            Hashtable ARequestParams,
            out string exportString,
            out TVerificationResultCollection AMessages,
            out Int32 BatchCount)
        {
            TVerificationResultCollection AResultMessages;
            string AExportString;
            Int32 ABatchCount;

            ABatchCount = TRemote.MFinance.Gift.WebConnectors.ExportAllGiftBatchData(
                ARequestParams,
                out AExportString,
                out AResultMessages);

            AMessages = AResultMessages;
            BatchCount = ABatchCount;
            exportString = AExportString;
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