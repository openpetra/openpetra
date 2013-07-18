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
        	FBudgetDS = TRemote.MFinance.Budget.WebConnectors.LoadBudget(FLedgerNumber);
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
            txtFilename.Text = TUserDefaults.GetStringDefault("Exp Filename",
                TClientSettings.GetExportPath() + Path.DirectorySeparatorChar + "BudgetExport.csv");

            //String expOptions = TUserDefaults.GetStringDefault("Exp Options", "DTrans");
            String expOptions = TUserDefaults.GetStringDefault("Exp Options", ";American");

            if (expOptions.Length > 0)
            {
                cmbDelimiter.SetSelectedString(ConvertDelimiter(expOptions.Substring(0, 1), true));
            }

            if (expOptions.Length > 1)
            {
                cmbNumberFormat.SelectedIndex = expOptions.Substring(1) == "American" ? 0 : 1;
            }

            cmbDateFormat.SetSelectedString(TUserDefaults.GetStringDefault("Exp Date", "DMY"));
        }

        private void SaveUserDefaults()
        {
            String expOptions = ConvertDelimiter((String)cmbDelimiter.SelectedItem, false);
            expOptions += ConvertNumberFormat(cmbNumberFormat);
            TUserDefaults.SetDefault("Exp Options", expOptions);
            TUserDefaults.SetDefault("Exp Filename", txtFilename.Text);
            TUserDefaults.SetDefault("Exp Date", (String)cmbDateFormat.SelectedItem);
            TUserDefaults.SaveChangedUserDefaults();
        }

		private void ExportBudgetSelect(object sender, EventArgs e)
		{
			ExportBudget(null, null);

			//Open file in explorer
			if (FExportFileName.Length > 0 && Utilities.DetermineExecutingOS() != TExecutingOSEnum.oesUnsupportedPlatform)
			{
				if (Utilities.DetermineExecutingOS() == TExecutingOSEnum.eosLinux)
				{
					//TODO: add code to select a file in Linux					
				}
				else
				{
					Process.Start("explorer", "/e,/select," + FExportFileName);
				}
			}
		}

        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void ExportBudget(object sender, EventArgs e)
        {
        	FExportFileName = txtFilename.Text;
            String fileContents = string.Empty;

            if (!Directory.Exists(Path.GetDirectoryName(FExportFileName)))
            {
                MessageBox.Show(Catalog.GetString("Please select an existing directory for this file!"),
                    Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                
            	FExportFileName = string.Empty;
            	return;
            }

            Hashtable requestParams = new Hashtable();

            requestParams.Add("ALedgerNumber", FLedgerNumber);
            requestParams.Add("Delimiter", ConvertDelimiter(cmbDelimiter.GetSelectedString(), false));
            requestParams.Add("DateFormatString", cmbDateFormat.GetSelectedString());
            requestParams.Add("NumberFormat", ConvertNumberFormat(cmbNumberFormat));

            TVerificationResultCollection AMessages;

			string[] delims = new string[1];
			delims[0] = ConvertDelimiter(cmbDelimiter.GetSelectedString(), false);

            Int32 budgetCount = TRemote.MFinance.Budget.WebConnectors.ExportBudgets(FLedgerNumber, FExportFileName, delims, ref fileContents, ref FBudgetDS, out AMessages);

            if (AMessages != null && AMessages.Count > 0)
            {
                if (AMessages.HasCriticalErrors)
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

            if (budgetCount == 0)
            {
                MessageBox.Show(Catalog.GetString("There are no Budgets matching your criteria"),
                    Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            	FExportFileName = string.Empty;
                return;
            }

            StreamWriter sw1 = null;

            try
            {
                sw1 = new StreamWriter(FExportFileName);
                sw1.Write(fileContents);
            }
            finally
            {
                if (sw1 != null)
                {
                    sw1.Close();
                }
            }

            MessageBox.Show(Catalog.GetString(String.Format("Exported successfully! {0} Budget rows exported as file:{1}{1}{2}",
                                                            budgetCount.ToString(),
                                                            Environment.NewLine,
                                                            FExportFileName.ToUpper())),
                Catalog.GetString("Budget Export"),
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
    }
}