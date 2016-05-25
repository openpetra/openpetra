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
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Globalization;

using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
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

            btnBrowseFilename.Top = txtFilename.Top;
            btnBrowseFilename.Height = txtFilename.Height;
            btnBrowseFilename.Width -= 7;

            LoadUserDefaults();
        }

        private Int32 FLedgerNumber;
        private BudgetTDS FBudgetDS = new BudgetTDS();
        private String FExportFileName = string.Empty;

        /// the ledger that the user is currently working with
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                LoadBudgets();

                //TFinanceControls.InitialiseAvailableFinancialYearsList(ref cmbDetailYear, FLedgerNumber, true);

                //TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, false, false);

                // Do not include summary cost centres: we want to use one cost centre for each Motivation Details
                //TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, false, true);
            }
        }

        private void LoadBudgets()
        {
            FBudgetDS = TRemote.MFinance.Budget.WebConnectors.LoadAllBudgets(FLedgerNumber);
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
            txtFilename.Text = TUserDefaults.GetStringDefault("Exp Filename",
                TClientSettings.GetExportPath() + Path.DirectorySeparatorChar + "BudgetExport.csv");

            CultureInfo myCulture = Thread.CurrentThread.CurrentCulture;
            string defaultImpOptions = myCulture.TextInfo.ListSeparator + TDlgSelectCSVSeparator.NUMBERFORMAT_EUROPEAN;

            if (myCulture.EnglishName.EndsWith("-US"))
            {
                defaultImpOptions = myCulture.TextInfo.ListSeparator + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN;
            }

            String impOptions = TUserDefaults.GetStringDefault("Imp Options", defaultImpOptions);

            if (impOptions.Length > 0)
            {
                cmbDelimiter.SetSelectedString(ConvertDelimiter(impOptions.Substring(0, 1), true));
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
            TUserDefaults.SetDefault("Exp Filename", txtFilename.Text);

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
        private void BtnOK_Click(object sender, EventArgs e)
        {
            FExportFileName = txtFilename.Text;
            String fileContents = string.Empty;
            Int32 budgetCount = 0;

            if (FExportFileName == String.Empty)
            {
                MessageBox.Show(Catalog.GetString("Please choose a location for the Export File."),
                    Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            else if (!FExportFileName.EndsWith(".csv",
                         StringComparison.CurrentCultureIgnoreCase) && !FExportFileName.EndsWith(".txt", StringComparison.CurrentCultureIgnoreCase))
            {
                FExportFileName += ".csv";
                txtFilename.Text = FExportFileName;
            }

            if (!Directory.Exists(Path.GetDirectoryName(FExportFileName)))
            {
                MessageBox.Show(Catalog.GetString("Please select an existing directory for this file!"),
                    Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                FExportFileName = string.Empty;
                return;
            }

            if (File.Exists(FExportFileName))
            {
                if (MessageBox.Show(Catalog.GetString("The file already exists. Is it OK to overwrite it?"),
                        Catalog.GetString("Export Budget"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                try
                {
                    File.Delete(FExportFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(
                            Catalog.GetString(
                                "Failed to delete the file. Maybe it is already open in another application?  The system message was:{0}{1}"),
                            Environment.NewLine, ex.Message),
                        Catalog.GetString("Export Budget"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }

            //Hashtable requestParams = new Hashtable();

            //requestParams.Add("ALedgerNumber", FLedgerNumber);
            //requestParams.Add("Delimiter", ConvertDelimiter(cmbDelimiter.GetSelectedString(), false));
            //requestParams.Add("DateFormatString", cmbDateFormat.GetSelectedString());
            //requestParams.Add("NumberFormat", ConvertNumberFormat(cmbNumberFormat));

            TVerificationResultCollection AMessages;

            string[] delims = new string[1];
            delims[0] = ConvertDelimiter(cmbDelimiter.GetSelectedString(), false);

            try
            {
                this.Cursor = Cursors.WaitCursor;

                budgetCount = TRemote.MFinance.Budget.WebConnectors.ExportBudgets(FLedgerNumber,
                    FExportFileName,
                    delims,
                    ref fileContents,
                    ref FBudgetDS,
                    out AMessages);

                this.Cursor = Cursors.Default;

                if ((AMessages != null) && (AMessages.Count > 0))
                {
                    if (!TVerificationHelper.IsNullOrOnlyNonCritical(AMessages))
                    {
                        MessageBox.Show(AMessages.BuildVerificationResultString(), Catalog.GetString("Error"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        FExportFileName = string.Empty;
                        return;
                    }
                    else
                    {
                        MessageBox.Show(AMessages.BuildVerificationResultString(), Catalog.GetString("Warnings"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }

                SaveUserDefaults();

                if (budgetCount == 0)
                {
                    MessageBox.Show(Catalog.GetString("There are no Budgets matching your criteria"),
                        Catalog.GetString("Error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    FExportFileName = string.Empty;
                    return;
                }

                StreamWriter sw1 = new StreamWriter(FExportFileName);
                sw1.Write(fileContents);
                sw1.Close();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            bool ShowExportedFileInExplorer = false;

            // Offer the client the chance to open the file in Excel or whatever
            if (MessageBox.Show(String.Format(Catalog.GetString(
                            "{0} Budget rows were exported successfully! Would you like to open the file in your default application?"),
                        budgetCount.ToString()),
                    Catalog.GetString("Budget Export"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    ProcessStartInfo si = new ProcessStartInfo(FExportFileName);
                    si.UseShellExecute = true;
                    si.Verb = "open";

                    Process p = new Process();
                    p.StartInfo = si;
                    p.Start();
                }
                catch
                {
                    MessageBox.Show(Catalog.GetString(
                            "Unable to launch the default application to open: '") + FExportFileName + "'!", Catalog.GetString(
                            "Budget Export"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    ShowExportedFileInExplorer = true;
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
                        Process.Start("explorer.exe", string.Format("/select,\"{0}\"", FExportFileName));
                    }
                    catch
                    {
                        MessageBox.Show(Catalog.GetString(
                                "Unable to launch Windows File Explorer to open: '") + FExportFileName + "'!", Catalog.GetString(
                                "Budget Export"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            Close();
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