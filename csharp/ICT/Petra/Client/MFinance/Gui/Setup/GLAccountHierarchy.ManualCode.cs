//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//      timop, wolfgangu
//      Tim Ingham
//
// Copyright 2004-2014 by OM International
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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.App.Core;
using System.Drawing;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    /// <summary>
    ///
    /// </summary>
    public class AccountNodeDetails
    {
        /// <summary>
        /// This will be true for Summary accounts, initially Unknown for existing posting accounts.
        /// On newly created accounts, this will be true.
        /// On a "need to know" basis, it will be set false for posting accounts that already have transactions posted to them.
        /// </summary>
        ///
        public Boolean? CanHaveChildren;

        /// <summary>
        /// This will be initially false for Summary accounts that have children, unknown for existing posting accounts.
        /// On newly created accounts, this will be true.
        /// On a "need to know" basis, it will be set false for posting accounts that already have transactions posted to them.
        /// </summary>
        public Boolean? CanDelete;

        /// <summary>If an account is new (created in this session) I can change it without consulting the server:</summary>
        public Boolean IsNew;

        /// <summary>A Message will be returned from the server if actions on this account are restricted.</summary>
        public String Msg;

        /// <summary>The actual data:</summary>
        public AAccountRow AccountRow;

        /// <summary>The actual data:</summary>
        public AAccountHierarchyDetailRow DetailRow;

        /// <summary>Reference to the tree node that also references this item</summary>
        public TreeNode linkedTreeNode;

        /// <summary>All the nodes share this Ledger Number</summary>
        public static Int32 FLedgerNumber;

        /// <summary>
        /// The information for this node is initially unknown (to save load-up time).
        /// This method fills in the details.
        /// </summary>
        public void GetAttrributes()
        {
            if (IsNew)
            {
                CanHaveChildren = true;
                CanDelete = (linkedTreeNode.Nodes.Count == 0);
                return;
            }

            if (!CanHaveChildren.HasValue || !CanDelete.HasValue)
            {
                bool RemoteCanBeParent = false;
                bool RemoteCanDelete = false;

                if (TRemote.MFinance.Setup.WebConnectors.GetAccountCodeAttributes(FLedgerNumber, DetailRow.ReportingAccountCode,
                        out RemoteCanBeParent, out RemoteCanDelete, out Msg))
                {
                    CanHaveChildren = RemoteCanBeParent;
                    CanDelete = RemoteCanDelete;
                }
            }
        }

        /// <summary>
        /// Create an AccountNodeDetails object for this account
        /// </summary>
        public static AccountNodeDetails AddNewAccount(TreeNode NewTreeNode, AAccountRow AccountRow, AAccountHierarchyDetailRow HierarchyDetailRow)
        {
            AccountNodeDetails NodeDetails = new AccountNodeDetails();

            NodeDetails.CanHaveChildren = true;

            if (AccountRow.PostingStatus) // A "Posting account" that's not been used may yet be promoted to a "Summary account".
            {
                NodeDetails.CanHaveChildren = null;
            }
            else      // A "Summary account" can have children.
            {
                NodeDetails.CanHaveChildren = true;
            }

            NodeDetails.IsNew = true;
            NodeDetails.DetailRow = HierarchyDetailRow;
            NodeDetails.AccountRow = AccountRow;
            NewTreeNode.Tag = NodeDetails;
            NodeDetails.linkedTreeNode = NewTreeNode;
            return NodeDetails;
        }
    };

    public partial class TFrmGLAccountHierarchy
    {
        private const string INTERNAL_UNASSIGNED_DETAIL_ACCOUNT_CODE = "#UNASSIGNEDDETAILACCOUNTCODE#";

        private String FStatus = "";

        private Int32 FLedgerNumber;
        private string FSelectedHierarchy = "STANDARD";

        /// <summary>This prevents the updates causing cascading functions</summary>
        public Int32 FIAmUpdating = 0;

        // The routine ChangeAccountCodeValue() needs the old value of
        // txtDetailAccountCode and the new actual value.
        // This string is used to store the old value.
        private string strOldDetailAccountCode = "";

        AccountNodeDetails FCurrentAccount = null;

        private string FRecentlyUpdatedDetailAccountCode = INTERNAL_UNASSIGNED_DETAIL_ACCOUNT_CODE;
        private string FNameForNewAccounts;

        /// <summary>
        /// Called from the user controls when the user selects a row,
        /// this common method keeps both user controls in sync.
        /// </summary>
        public void SetSelectedAccount(AccountNodeDetails AnewSelection)
        {
            if (FCurrentAccount != AnewSelection)
            {
                FCurrentAccount = AnewSelection;
                ucoAccountsList.SelectedAccount = AnewSelection;
                ucoAccountsTree.SelectedAccount = AnewSelection;

                pnlDetails.Enabled = (AnewSelection != null);

/*
 *              String Msg = "null";
 *              if (FCurrentAccount != null)
 *              {
 *                  Msg = FCurrentAccount.AccountRow.AccountCode;
 *              }
 *              ShowStatus("SetSelectedAccount: " + Msg);
 */
            }
        }

        /// <summary>
        /// The ListView only gives me the AccountCode
        /// That's OK - I can ask the TreeView to find the actual record.
        /// It calls back to SetSelectedAccount, above.
        /// </summary>
        public void SetSelectedAccountCode(String AnewAccountCode)
        {
            ucoAccountsTree.SelectNodeByName(AnewAccountCode);
        }

        /// <summary>Clear the Status Box</summary>
        public void ClearStatus()
        {
            FStatus = "";
            txtStatus.Text = FStatus;
            txtStatus.Refresh();
        }

        /// <summary>Add this in the Status Box</summary>
        /// <param name="NewStr"></param>
        public void ShowStatus(String NewStr)
        {
            FStatus = FStatus + "\r\n" + NewStr;
            txtStatus.Text = FStatus;
            txtStatus.Refresh();
        }

        /// <summary>
        /// Print out the Hierarchy using FastReports template.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilePrint(object sender, EventArgs e)
        {
            FastReportsWrapper ReportingEngine = new FastReportsWrapper("Account Hierarchy");

            if (!ReportingEngine.LoadedOK)
            {
                ReportingEngine.ShowErrorPopup();
                return;
            }

            if (!FMainDS.AAccount.Columns.Contains("AccountPath"))
            {
                FMainDS.AAccount.Columns.Add("AccountPath", typeof(String));
                FMainDS.AAccount.Columns.Add("AccountLevel", typeof(Int32));
            }

            DataView PathView = new DataView(FMainDS.AAccountHierarchyDetail);
            PathView.Sort = "a_reporting_account_code_c";

            DataView AccountView = new DataView(FMainDS.AAccount);
            AccountView.Sort = "a_account_code_c";

            // I need to make the "AccountPath" field that will be used to sort the table for printout:
            foreach (DataRowView rv in PathView)
            {
                DataRow Row = rv.Row;
                String AccountCode = Row["a_reporting_account_code_c"].ToString();
                String Path = Row["a_report_order_i"] + "-" + AccountCode + '~';
                Int32 Level = 0;
                String ReportsTo = Row["a_account_code_to_report_to_c"].ToString();

                while (ReportsTo != "")
                {
                    Int32 ParentIdx = PathView.Find(ReportsTo);

                    if (ParentIdx >= 0)
                    {
                        DataRow ParentRow = PathView[ParentIdx].Row;
                        ReportsTo = ParentRow["a_account_code_to_report_to_c"].ToString();
                        Path = ParentRow["a_report_order_i"] + "-" + ParentRow["a_reporting_account_code_c"].ToString() + "~" + Path;
                        Level++;
                    }
                    else
                    {
                        ReportsTo = "";
                    }
                }

                Int32 AccountIdx = AccountView.Find(AccountCode);
                DataRow AccountRow = AccountView[AccountIdx].Row;
                AccountRow["AccountPath"] = Path;
                AccountRow["AccountLevel"] = Level;
            }

            AccountView.Sort = "AccountPath";
            DataTable SortedByPath = AccountView.ToTable();

            ReportingEngine.RegisterData(SortedByPath, "AccountHierarchy");
            ReportingEngine.RegisterData(FMainDS.AAnalysisAttribute, "AnalysisAttribute");
            TRptCalculator Calc = new TRptCalculator();
            ALedgerRow LedgerRow = FMainDS.ALedger[0];
            Calc.AddParameter("param_ledger_nunmber", LedgerRow.LedgerNumber);
            Calc.AddStringParameter("param_ledger_name", LedgerRow.LedgerName);

            if (ModifierKeys.HasFlag(Keys.Control))
            {
                ReportingEngine.DesignReport(Calc);
            }
            else
            {
                ReportingEngine.GenerateReport(Calc);
            }
        }

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.UnhookControl(txtDetailAccountCode, false); // I don't want changes in this edit box to cause SetChangedFlag - I'll set it myself.
            FPetraUtilsObject.UnhookControl(txtStatus, false); // This control is not to be spied on!
            txtDetailAccountCode.TextChanged += new EventHandler(txtDetailAccountCode_TextChanged);
            txtDetailAccountCode.Validated -= ControlValidatedHandler; // Don't trigger validation on change - I need to do it manually

            chkDetailForeignCurrencyFlag.CheckedChanged += new EventHandler(chkDetailForeignCurrencyFlag_CheckedChanged);
            chkDetailIsSummary.CheckedChanged += chkDetailIsSummary_CheckedChanged;

            FPetraUtilsObject.ControlChanged += new TValueChangedHandler(FPetraUtilsObject_ControlChanged);
            txtDetailEngAccountCodeLongDesc.Leave += new EventHandler(AutoFillDescriptions);
            cmbDetailValidCcCombo.SelectedValueChanged += cmbDetailValidCcCombo_SelectedValueChanged;

            FPetraUtilsObject.DataSaved += OnHierarchySaved;
            FormClosing += TFrmGLAccountHierarchy_FormClosing;
            FIAmUpdating = 0;
            FNameForNewAccounts = Catalog.GetString("NEWACCOUNT");

            mniFilePrint.Click += FilePrint;
            mniFilePrint.Enabled = true;

            if (TAppSettingsManager.GetBoolean("OmBuild", false)) // In OM, no-one needs to see the import or export functions:
            {
                tbrMain.Items.Remove(tbbImportHierarchy);
                tbrMain.Items.Remove(tbbExportHierarchy);
                mnuMain.Items.Remove(mniImportHierarchy);
                mnuMain.Items.Remove(mniExportHierarchy);
            }
        }

        /// <summary>If the user sets this strangely, I'll just warn her...</summary>
        ///
        void cmbDetailValidCcCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((FCurrentAccount == null) || (FIAmUpdating > 0)) // Only look into this if the user has changed it...
            {
                return;
            }

            String AccountType = cmbDetailAccountType.Text;
            String ValidCCType = cmbDetailValidCcCombo.Text;
            String RequiredValue = "";

            if ((AccountType == "Asset") || (AccountType == "Liability"))
            {
                RequiredValue = "Local";
            }

            if (AccountType == "Equity")
            {
                RequiredValue = "All";
            }

            if ((RequiredValue != "") && (ValidCCType != RequiredValue))
            {
                if (MessageBox.Show(String.Format(Catalog.GetString(
                                "{0} Accounts should accept CostCentres of type {1}.\n" +
                                "Are you sure you want to use {2}?"),
                            AccountType,
                            RequiredValue,
                            ValidCCType), Catalog.GetString("Valid Cost Centre Type"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                    != System.Windows.Forms.DialogResult.Yes)
                {
                    FIAmUpdating++;
                    cmbDetailValidCcCombo.Text = RequiredValue;
                    FIAmUpdating--;
                }
            }
        }

        void chkDetailIsSummary_CheckedChanged(object sender, EventArgs e)
        {
            if ((FCurrentAccount != null) && (FIAmUpdating == 0)) // Only look into this is the user has changed it...
            {
                FCurrentAccount.GetAttrributes();

                if (chkDetailIsSummary.Checked) // I can't allow this to be made a summary if it has transactions posted:
                {
                    if (!FCurrentAccount.CanHaveChildren.Value)
                    {
                        MessageBox.Show(String.Format("Account {0} cannot be made summary because it has tranactions posted to it.",
                                FCurrentAccount.AccountRow.AccountCode), "Summary Account");
                        chkDetailIsSummary.Checked = false;
                    }
                }
                else // I can't allow this account to be a posting account if it has children:
                {
                    if (FCurrentAccount.linkedTreeNode.Nodes.Count > 0)
                    {
                        MessageBox.Show(String.Format("Account {0} cannot be made postable while it has children.",
                                FCurrentAccount.AccountRow.AccountCode), "Summary Account");
                        chkDetailIsSummary.Checked = true;
                    }
                }
            }
        }

        void FPetraUtilsObject_ControlChanged(Control Sender)
        {
            ucoAccountsTree.SetNodeLabel(txtDetailAccountCode.Text, txtDetailAccountCodeShortDesc.Text);
        }

        void chkDetailForeignCurrencyFlag_CheckedChanged(object sender, EventArgs e)
        {
            cmbDetailForeignCurrencyCode.Enabled = chkDetailForeignCurrencyFlag.Checked;
            String CurrencyLabel = (cmbDetailForeignCurrencyCode.Enabled ? GetSelectedDetailRowManual().ForeignCurrencyCode : "");
            cmbDetailForeignCurrencyCode.SetSelectedString(CurrencyLabel, -1);
        }

        private void AutoFillDescriptions(object sender, EventArgs e)
        {
            String NewText = txtDetailEngAccountCodeLongDesc.Text;

            if (txtDetailEngAccountCodeShortDesc.Text == "")
            {
                txtDetailEngAccountCodeShortDesc.Text = NewText;
            }

            if (txtDetailAccountCodeLongDesc.Text == "")
            {
                txtDetailAccountCodeLongDesc.Text = NewText;
            }

            if (txtDetailAccountCodeShortDesc.Text == "")
            {
                txtDetailAccountCodeShortDesc.Text = NewText;
            }

            FPetraUtilsObject_ControlChanged(txtDetailEngAccountCodeLongDesc);
        }

        /// <summary>
        /// Setup the account hierarchy of this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                AccountNodeDetails.FLedgerNumber = FLedgerNumber;
                ucoAccountAnalysisAttributes.LedgerNumber = FLedgerNumber;
                ucoAccountAnalysisAttributes.ShowStatus = ShowStatus;
                FMainDS.Clear();
                FMainDS.Merge(TRemote.MFinance.Setup.WebConnectors.LoadAccountHierarchies(FLedgerNumber));
                ucoAccountsTree.RunOnceOnActivationManual(this);
                ucoAccountsTree.PopulateTreeView(FMainDS, FLedgerNumber, FSelectedHierarchy);

                ucoAccountsList.RunOnceOnActivationManual(this);
                ucoAccountsList.PopulateListView(FMainDS, FLedgerNumber, FSelectedHierarchy);
            }
        }


        private void ValidateDataDetailsManual(GLSetupTDSAAccountRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_Setup.ValidateAccountDetailManual(
                this,
                ARow,
                ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        private void ShowDetailsManual(GLSetupTDSAAccountRow ARow)
        {
            if (ARow == null)
            {
                txtDetailAccountCode.Text = "";
                txtDetailAccountCodeLongDesc.Text = "";
                txtDetailAccountCodeShortDesc.Text = "";
                txtDetailEngAccountCodeLongDesc.Text = "";
                txtDetailEngAccountCodeShortDesc.Text = "";

                pnlDetails.Enabled = false;
            }
            else
            {
                pnlDetails.Enabled = true;

                strOldDetailAccountCode = txtDetailAccountCode.Text;

                ucoAccountAnalysisAttributes.Enabled = ARow.PostingStatus;
                // This call to an external user control resets change detection suppression so we need to re-enable it
                ucoAccountAnalysisAttributes.AccountCode = ARow.AccountCode;
                FPetraUtilsObject.DisableDataChangedEvent();

                chkDetailForeignCurrencyFlag.Enabled = (ARow.PostingStatus && !ARow.SystemAccountFlag);
                chkDetailBankAccountFlag.Enabled = !ARow.SystemAccountFlag;
                cmbDetailForeignCurrencyCode.Enabled = (ARow.PostingStatus && !ARow.SystemAccountFlag && ARow.ForeignCurrencyFlag);

                chkDetailIsSummary.Checked = !ARow.PostingStatus;
                chkDetailIsSummary.Enabled = !ARow.SystemAccountFlag;

                //
                // Reporting Order is in AAccountHierarchyDetail

                FMainDS.AAccountHierarchyDetail.DefaultView.RowFilter = String.Format("{0}='{1}'",
                    AAccountHierarchyDetailTable.GetReportingAccountCodeDBName(), ARow.AccountCode);
                String txtReportingOrder = "";

                if (FMainDS.AAccountHierarchyDetail.DefaultView.Count > 0)
                {
                    txtReportingOrder = ((AAccountHierarchyDetailRow)FMainDS.AAccountHierarchyDetail.DefaultView[0].Row).ReportOrder.ToString();
                }

                txtRptOrder.Text = txtReportingOrder;
                txtRptOrder.Enabled = !ARow.SystemAccountFlag;

                if (!ARow.ForeignCurrencyFlag)
                {
                    cmbDetailForeignCurrencyCode.SelectedIndex = -1;
                    ARow.ForeignCurrencyCode = "";
                }

                chkDetailAccountActiveFlag.Enabled = !ARow.SystemAccountFlag;

                // I allow the user to attempt to change the primary key,
                // but if the selected record is not new, AND they have made any other changes,
                // the txtDetailAccountCode _TextChanged method will disallow any change.
                SetPrimaryKeyReadOnly(false);
                btnRename.Visible = false;
            }
        }

        void txtDetailAccountCode_TextChanged(object sender, EventArgs e)
        {
            if (FIAmUpdating > 0)
            {
                return;
            }

            if (FCurrentAccount.AccountRow.SystemAccountFlag)
            {
                FIAmUpdating++;
                txtDetailAccountCode.Text = strOldDetailAccountCode;
                FIAmUpdating--;
                MessageBox.Show(Catalog.GetString("System Account Code cannot be changed."),
                    Catalog.GetString("Rename Account"),
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (strOldDetailAccountCode.IndexOf(FNameForNewAccounts) == 0)  // This is the first time the name is being set?
            {
                FPetraUtilsObject_ControlChanged(txtDetailAccountCode);
                return;
            }

            bool ICanEditAccountCode = (FCurrentAccount.IsNew || !FPetraUtilsObject.HasChanges);
            btnRename.Visible = (strOldDetailAccountCode != "") && (strOldDetailAccountCode != txtDetailAccountCode.Text) && ICanEditAccountCode;

            if (!FCurrentAccount.IsNew && FPetraUtilsObject.HasChanges) // The user wants to change an Account code, but I can't allow it.
            {
                FIAmUpdating++;
                txtDetailAccountCode.Text = strOldDetailAccountCode;
                FIAmUpdating--;
                MessageBox.Show(Catalog.GetString(
                        "Account Codes cannot be changed while there are other unsaved changes.\r\nSave first, then rename the Account."),
                    Catalog.GetString("Rename Account"),
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                FPetraUtilsObject_ControlChanged(txtDetailAccountCode);
            }
        }

        private void OnHierarchySaved(System.Object sender, TDataSavedEventArgs e)
        {
            SetPrimaryKeyReadOnly(false);
        }

        private void AddNewAccount(Object sender, EventArgs e)
        {
            if (FCurrentAccount == null)
            {
                MessageBox.Show(Catalog.GetString("You can only add a new account after selecting a parent account"));
                return;
            }

            ValidateAllData(true, false);
            string newName = FNameForNewAccounts;
            Int32 countNewAccount = 0;

            if (FMainDS.AAccount.Rows.Find(new object[] { FLedgerNumber, newName }) != null)
            {
                while (FMainDS.AAccount.Rows.Find(new object[] { FLedgerNumber, newName + countNewAccount.ToString() }) != null)
                {
                    countNewAccount++;
                }

                newName += countNewAccount.ToString();
            }

            // ChangeAccountCodeValue() needs this value!
            strOldDetailAccountCode = newName;

            AAccountRow parentAccount = FCurrentAccount.AccountRow;

            AAccountRow newAccountRow = FMainDS.AAccount.NewRowTyped();
            newAccountRow.AccountCode = newName;
            newAccountRow.LedgerNumber = FLedgerNumber;
            newAccountRow.AccountActiveFlag = true;
            newAccountRow.DebitCreditIndicator = parentAccount.DebitCreditIndicator;
            newAccountRow.AccountType = parentAccount.AccountType;
            newAccountRow.ValidCcCombo = parentAccount.ValidCcCombo;
            newAccountRow.PostingStatus = true;
            FMainDS.AAccount.Rows.Add(newAccountRow);

            AAccountHierarchyDetailRow hierarchyDetailRow = FMainDS.AAccountHierarchyDetail.NewRowTyped();
            hierarchyDetailRow.LedgerNumber = FLedgerNumber;
            hierarchyDetailRow.AccountHierarchyCode = FSelectedHierarchy;
            hierarchyDetailRow.AccountCodeToReportTo = parentAccount.AccountCode;
            hierarchyDetailRow.ReportingAccountCode = newName;

            // change posting/summary flag of parent account if it was previously a leaf
            parentAccount.PostingStatus = false; // The parent is now a summary account!

            hierarchyDetailRow.ReportOrder = ucoAccountsTree.GetLastChildReportingOrder() + 1;
            FMainDS.AAccountHierarchyDetail.Rows.Add(hierarchyDetailRow);

            ucoAccountsTree.AddNewAccount(newAccountRow, hierarchyDetailRow);

            txtDetailAccountCode.Focus();
            FPetraUtilsObject.SetChangedFlag();
        }

        private void ExportHierarchy(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(TRemote.MFinance.Setup.WebConnectors.ExportAccountHierarchy(FLedgerNumber, FSelectedHierarchy));

            TImportExportDialogs.ExportWithDialog(doc, Catalog.GetString("Save Account Hierarchy to file"));
        }

        private void ImportHierarchy(object sender, EventArgs e)
        {
            // TODO: open file; only will work if there are no GLM records and transactions yet
            XmlDocument doc;

            try
            {
                doc = TImportExportDialogs.ImportWithDialog(Catalog.GetString("Load Account Hierarchy from file"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Catalog.GetString("Load Account Hierarchy from file"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (doc == null)
            {
                // was cancelled
                return;
            }

            if (!TRemote.MFinance.Setup.WebConnectors.ImportAccountHierarchy(FLedgerNumber, FSelectedHierarchy, TXMLParser.XmlToString(doc)))
            {
                MessageBox.Show(Catalog.GetString(
                        "Import of new Account Hierarchy failed; perhaps there were already balances? Try with a new ledger!"),
                    Catalog.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // refresh the screen
                FMainDS.Clear();
                FMainDS.Merge(TRemote.MFinance.Setup.WebConnectors.LoadAccountHierarchies(FLedgerNumber));
                ucoAccountsTree.PopulateTreeView(FMainDS, FLedgerNumber, FSelectedHierarchy);

                MessageBox.Show("Import of new Account Hierarchy has been successful",
                    Catalog.GetString("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private TSubmitChangesResult StoreManualCode(ref GLSetupTDS ASubmitDS, out TVerificationResultCollection AVerificationResult)
        {
            //
            // I need to remove any AnalysisAttribute records that are still set to "Unassigned"
            //
            if ((ASubmitDS.AAnalysisAttribute != null) && (ASubmitDS.AAnalysisAttribute.Rows.Count > 0))
            {
                for (int Idx = ASubmitDS.AAnalysisAttribute.Rows.Count - 1; Idx >= 0; Idx--)
                {
                    AAnalysisAttributeRow Row = ASubmitDS.AAnalysisAttribute[Idx];

                    if ((Row.RowState != DataRowState.Deleted) && (Row.AnalysisTypeCode.IndexOf("Unassigned") == 0))
                    {
                        Row.Delete();
                    }
                }
            }

            //
            // I'll take this opportunity to remove any similar records in my own TDS
            //
            for (int Idx = FMainDS.AAnalysisAttribute.Rows.Count - 1; Idx >= 0; Idx--)
            {
                AAnalysisAttributeRow Row = FMainDS.AAnalysisAttribute[Idx];

                if ((Row.RowState != DataRowState.Deleted) && (Row.AnalysisTypeCode.IndexOf("Unassigned") == 0))
                {
                    Row.Delete();
                }
            }

            return TRemote.MFinance.Setup.WebConnectors.SaveGLSetupTDS(FLedgerNumber, ref ASubmitDS, out AVerificationResult);
        }

        private void DeleteAccount(Object sender, EventArgs ev)
        {
            string AccountCode = FCurrentAccount.DetailRow.ReportingAccountCode;

            if (!FCurrentAccount.CanDelete.HasValue)
            {
                MessageBox.Show(Catalog.GetString("Fault: CanDelete status is unknown."), Catalog.GetString(
                        "Delete Account"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!FCurrentAccount.CanDelete.Value)
            {
                MessageBox.Show(
                    String.Format(Catalog.GetString(
                            "Account {0} cannot be deleted. You can deactivate the account, but not delete it."),
                        AccountCode) +
                    "\r\n" + FCurrentAccount.Msg,
                    Catalog.GetString("Delete Account"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ucoAccountsTree.DeleteSelectedAccount(); // Calling this changes the current FCurrentAccount to the parent of the deleted account!

            //
            // If this account has analysis Attributes,
            // I need to remove them.

            if (FMainDS.AAnalysisAttribute != null)
            {
                DataView DeleteThese = new DataView(FMainDS.AAnalysisAttribute);
                DeleteThese.RowFilter = String.Format("a_ledger_number_i={0} AND a_account_code_c='{1}'",
                    FLedgerNumber, AccountCode);

                foreach (DataRowView rv in DeleteThese)
                {
                    DataRow TempRow = rv.Row;
                    TempRow.Delete();
                }
            }

            AAccountHierarchyDetailRow AccountHDetailToBeDeleted = (AAccountHierarchyDetailRow)FMainDS.AAccountHierarchyDetail.Rows.Find(
                new object[] { FLedgerNumber, FSelectedHierarchy, AccountCode });
            AccountHDetailToBeDeleted.Delete();

            //
            // I can delete this account if it no longer appears in any Hieararchy.

            DataView AHD_stillInUse = new DataView(FMainDS.AAccountHierarchyDetail);
            AHD_stillInUse.RowFilter = String.Format("a_ledger_number_i={0} AND a_reporting_account_code_c='{1}'",
                FLedgerNumber, AccountCode);

            if (AHD_stillInUse.Count == 0)
            {
                AAccountRow AccountToBeDeleted = (AAccountRow)FMainDS.AAccount.Rows.Find(
                    new object[] { FLedgerNumber, AccountCode });
                AccountToBeDeleted.Delete();
            }
            else
            {
                MessageBox.Show(String.Format(
                        Catalog.GetString(
                            "The account {0} is removed from the {1} hierarchy, but not deleted, since it remains part of another heirarchy."),
                        AccountCode,
                        FSelectedHierarchy),
                    Catalog.GetString("Delete Account"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // FCurrentAccount is now the parent of the account that was just deleted.
            // If the parent account now has no accounts reporting to it (in any hierarchies), mark it as posting account.
            FCurrentAccount.GetAttrributes();
            tbbDeleteAccount.Enabled = (FCurrentAccount.CanDelete.HasValue ? FCurrentAccount.CanDelete.Value : false);
            AAccountRow ParentAccountRow = FCurrentAccount.AccountRow;
            AHD_stillInUse.RowFilter = String.Format("a_ledger_number_i={0} AND a_account_code_to_report_to_c='{1}'",
                FLedgerNumber, ParentAccountRow.AccountCode);

            if (AHD_stillInUse.Count == 0)  // No-one now reports to this account, so I can mark it as "Posting"
            {                               // Note that since the "Posting" status is now editable, this is unneccessary and may be inappropriate.
                ParentAccountRow.PostingStatus = true;
                chkDetailForeignCurrencyFlag.Enabled = (!ParentAccountRow.SystemAccountFlag);
                cmbDetailForeignCurrencyCode.Enabled = (!ParentAccountRow.SystemAccountFlag && ParentAccountRow.ForeignCurrencyFlag);

                // It's possible this account could now be deleted, but the user would need to save and re-load first,
                // if the server still has it down as a summary account.
            }

            FPetraUtilsObject.SetChangedFlag();
        }

        /// <summary>
        /// Called from ValidateAllData
        /// </summary>
        private void GetDataFromControlsManual()
        {
            if (FCurrentAccount != null)
            {
                AutoFillDescriptions(null, null);
                GetDetailsFromControls(GetSelectedDetailRowManual());
                //
                // The auto-generated code doesn't get the details from the UC_AnalasisAttributes control,
                // so I need to do that here:
                ucoAccountAnalysisAttributes.GetDataFromControls();

                //
                // I need to ensure that the AccountHierarchyDetail row has the same AccountCode as the Account Row
                FCurrentAccount.DetailRow.ReportingAccountCode = FCurrentAccount.AccountRow.AccountCode;
                ucoAccountsTree.SetNodeLabel(GetSelectedDetailRowManual());

                FCurrentAccount.AccountRow.PostingStatus = !chkDetailIsSummary.Checked;
                Int32 ReportingOrder = 0;

                if (Int32.TryParse(txtRptOrder.Text, out ReportingOrder) && (FCurrentAccount.DetailRow.ReportOrder != ReportingOrder))
                {
                    FCurrentAccount.DetailRow.ReportOrder = ReportingOrder;
                }
            }
        }

        /// <summary>
        /// When the user selects a new account, unload the controls and check whether that's OK.
        /// </summary>
        /// <returns>false if the user must stay on the current row and fix the problem</returns>
        public Boolean CheckControlsValidateOk()
        {
            GetDetailsFromControls((GLSetupTDSAAccountRow)FCurrentAccount.AccountRow);
            return ValidateAllData(true, true);
        }

        /// <summary>
        /// Essentially a public wrapper for ShowDetails.
        /// </summary>
        public void PopulateControlsAfterRowSelection()
        {
            bool hasChanges = FPetraUtilsObject.HasChanges;

            FIAmUpdating++;
            FPetraUtilsObject.SuppressChangeDetection = true;

            if (FCurrentAccount == null)
            {
                ShowDetails(null);
            }
            else
            {
                ShowDetails((GLSetupTDSAAccountRow)FCurrentAccount.AccountRow);
            }

            FPetraUtilsObject.SuppressChangeDetection = false;
            FIAmUpdating--;

            tbbAddNewAccount.Enabled =
                ((FCurrentAccount != null) && (FCurrentAccount.CanHaveChildren.HasValue ? FCurrentAccount.CanHaveChildren.Value : false));
            tbbDeleteAccount.Enabled = ((FCurrentAccount != null) && (FCurrentAccount.CanDelete.HasValue ? FCurrentAccount.CanDelete.Value : false));

            FPetraUtilsObject.HasChanges = hasChanges;
        }

        /// <summary>
        /// The Tree control can't set the selectedNode unless it's in focus
        /// (I guess this is a bug)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTabChange(object sender, EventArgs e)
        {
            if (ucoAccountsTree.Visible)
            {
                ucoAccountsList.CollapseFilterFind();

                ucoAccountsTree.Focus();
                ucoAccountsTree.RefreshSelectedAccount();
            }
            else
            {
                ucoAccountsList.UpdateRecordNumberDisplay();
                ucoAccountsList.Focus();
            }
        }

        private GLSetupTDSAAccountRow GetSelectedDetailRowManual()
        {
            if (FCurrentAccount == null)
            {
                return null;
            }

            return (GLSetupTDSAAccountRow)FCurrentAccount.AccountRow;
        }

        /// <summary>
        /// ChangeAccountCodeValue is invoked when txtDetailAccountCode is left
        ///
        /// But if the user invokes an other event - i.e. FileSave the FileSave-Event runs first.
        /// </summary>

        public void ChangeAccountCodeValue(object sender, EventArgs e)
        {
            CheckAccountCodeValueChanged();
        }

        private Boolean CheckAccountCodeValueChanged()
        {
            String strNewDetailAccountCode = txtDetailAccountCode.Text;
            String strAccountShortDescr = txtDetailEngAccountCodeShortDesc.Text;

            bool changeAccepted = false;

            if (strNewDetailAccountCode != FRecentlyUpdatedDetailAccountCode)
            {
                if (strNewDetailAccountCode != strOldDetailAccountCode)
                {
                    if (strOldDetailAccountCode.IndexOf(FNameForNewAccounts) < 0) // If they're just changing this from the initial value, don't show warning.
                    {
                        if (MessageBox.Show(String.Format(Catalog.GetString(
                                        "You have changed the Account Code from '{0}' to '{1}'.\r\n\r\n" +
                                        "Please confirm that you want to rename this account by choosing 'OK'.\r\n\r\n" +
                                        "(Renaming will take a few moments, then the form will be re-loaded.)"),
                                    strOldDetailAccountCode,
                                    strNewDetailAccountCode),
                                Catalog.GetString("Rename Account: Confirmation"), MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) != DialogResult.OK)
                        {
                            txtDetailAccountCode.Text = strOldDetailAccountCode;
                            return false;
                        }
                    }

                    this.Cursor = Cursors.WaitCursor;
                    this.Refresh();  // Just to get the Wait Cursor to display...

                    FRecentlyUpdatedDetailAccountCode = strNewDetailAccountCode;

                    try
                    {
                        FCurrentAccount.AccountRow.BeginEdit();
                        FCurrentAccount.AccountRow.AccountCode = strNewDetailAccountCode;
                        FCurrentAccount.DetailRow.ReportingAccountCode = strNewDetailAccountCode;
                        FCurrentAccount.AccountRow.EndEdit();
                        ucoAccountsTree.SetNodeLabel(strNewDetailAccountCode, strAccountShortDescr);

                        changeAccepted = true;
                    }
                    catch (System.Data.ConstraintException)
                    {
                        txtDetailAccountCode.Text = strOldDetailAccountCode;
                        FCurrentAccount.AccountRow.CancelEdit();
                        FCurrentAccount.DetailRow.CancelEdit();

                        FRecentlyUpdatedDetailAccountCode = INTERNAL_UNASSIGNED_DETAIL_ACCOUNT_CODE;

                        ShowStatus(Catalog.GetString("Account Code change REJECTED!"));

                        MessageBox.Show(String.Format(
                                Catalog.GetString(
                                    "Renaming Account Code '{0}' to '{1}' is not possible because an Account Code by the name of '{2}' already exists.\r\n\r\n--> Account Code reverted to previous value!"),
                                strOldDetailAccountCode, strNewDetailAccountCode, strNewDetailAccountCode),
                            Catalog.GetString("Renaming Not Possible - Conflicts With Existing Account Code"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        txtDetailAccountCode.Focus();
                    }

                    if (changeAccepted)
                    {
                        if (FCurrentAccount.IsNew)
                        {
                            // This is the code for changes in "un-committed" nodes:
                            // there are no references to this new row yet, apart from children nodes, so I can just change them here and carry on!
                            ucoAccountsTree.FixupChildrenAfterAccountNameChange();

                            strOldDetailAccountCode = strNewDetailAccountCode;
                            FPetraUtilsObject.HasChanges = true;
                            ucoAccountAnalysisAttributes.AccountCode = strNewDetailAccountCode;
                        }
                        else
                        {
                            ShowStatus(Catalog.GetString("Updating Account Code change - please wait."));
                            TVerificationResultCollection VerificationResults;

                            // If this code was previously in the DB, I need to assume that there may be transactions posted to it.
                            // There's a server call I need to use, and after the call I need to re-load this page.
                            // (No other changes will be lost, because the txtDetailAccountCode_TextChanged would
                            // have disallowed the change if there were already changes.)
                            bool Success = TRemote.MFinance.Setup.WebConnectors.RenameAccountCode(strOldDetailAccountCode,
                                strNewDetailAccountCode,
                                FLedgerNumber,
                                out VerificationResults);                                                           // This call takes ages..

                            if (Success)
                            {
                                FIAmUpdating++;
                                FMainDS.Clear();
                                FMainDS.Merge(TRemote.MFinance.Setup.WebConnectors.LoadAccountHierarchies(FLedgerNumber));      // and this also takes a while!
                                strOldDetailAccountCode = "";
                                FPetraUtilsObject.SuppressChangeDetection = true;
                                ucoAccountsTree.PopulateTreeView(FMainDS, FLedgerNumber, FSelectedHierarchy);
                                ShowDetailsManual(null);
                                ClearStatus();
                                FIAmUpdating--;
                                FPetraUtilsObject.SuppressChangeDetection = false;
                                ucoAccountsTree.SelectNodeByName(FRecentlyUpdatedDetailAccountCode);

                                ShowStatus(String.Format(Catalog.GetString("Account Code changed to '{0}'."), strNewDetailAccountCode));
                            }
                            else
                            {
                                MessageBox.Show(VerificationResults.BuildVerificationResultString(), Catalog.GetString("Rename Account"));
                            }

                            changeAccepted = false; // Actually the change was accepted, but processed here, so there's nothing left to do.
                            FPetraUtilsObject.HasChanges = false;
                        }

                        btnRename.Visible = false;
                    } // if changeAccepted

                    this.Cursor = Cursors.Default;
                } // if changed

            } // if not handling the same change as before (prevents this method running several times for a single change!)

            return changeAccepted;
        }

        private void TFrmGLAccountHierarchy_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel |= CheckAccountCodeValueChanged();
        }
    } // TFrmGLAccountHierarchy
}