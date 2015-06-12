//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
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
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using System.Drawing;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    /// <summary>
    /// These objects are linked to nodes in the tree, and hold the actual data.
    /// </summary>
    public class CostCentreNodeDetails
    {
        /// <summary>
        /// This will be true for Summary cost codes, initially Unknown for "leaves".
        /// On newly created cost codes, this will be true.
        /// On a "need to know" basis, it will be set false for cost codes that already have transactions posted to them.
        /// </summary>
        ///
        public Boolean? CanHaveChildren;

        /// <summary>
        /// This will be initially false for Summary cost codes that have children, unknown for "leaves".
        /// On newly created cost codes, this will be true.
        /// On a "need to know" basis, it will be set false for cost codes that already have transactions posted to them.
        /// </summary>
        public Boolean? CanDelete;

        /// <summary>If the Node is new, I can delete it without worrying</summary>
        public Boolean IsNew;

        /// <summary>If actions on the Node are restricted, this message is returned from the server.</summary>
        public String Msg;

        /// <summary>..and here's the actual data! </summary>
        public ACostCentreRow CostCentreRow;

        /// <summary>Reference to the tree node that also references this item</summary>
        public TreeNode linkedTreeNode;

        /// <summary>All the nodes share this Ledger Number</summary>
        public static Int32 FLedgerNumber;

        /// <summary>If lock is applied, some details of child Cost Centres cannot be changed.</summary>
        public Boolean IsLocked;

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

                if (TRemote.MFinance.Setup.WebConnectors.GetCostCentreAttributes(FLedgerNumber, CostCentreRow.CostCentreCode,
                        out RemoteCanBeParent, out RemoteCanDelete, out Msg))
                {
                    CanHaveChildren = RemoteCanBeParent;
                    CanDelete = RemoteCanDelete;
                }
            }
        }

        /// <summary>
        /// Create an CostCentreNodeDetails object for this CostCentre
        /// </summary>
        public static CostCentreNodeDetails AddNewCostCentre(TreeNode NewTreeNode, ACostCentreRow CostCentreRow)
        {
            CostCentreNodeDetails NodeDetails = new CostCentreNodeDetails();

            NodeDetails.CanHaveChildren = true;

            if (CostCentreRow.PostingCostCentreFlag) // A "Posting CostCentre" that's not been used may yet be promoted to a "Summary CostCentre".
            {
                NodeDetails.CanHaveChildren = null;
            }
            else      // A "Summary CostCentre" can have children.
            {
                NodeDetails.CanHaveChildren = true;
            }

            NodeDetails.IsNew = true;
            NodeDetails.CostCentreRow = CostCentreRow;
            NewTreeNode.Tag = NodeDetails;
            NodeDetails.linkedTreeNode = NewTreeNode;
            return NodeDetails;
        }
    };

    public partial class TFrmGLCostCentreHierarchy
    {
        private const string INTERNAL_UNASSIGNED_DETAIL_COSTCENTRE_CODE = "#UNASSIGNEDDETAILCOSTCENTRECODE#";

        private String FStatus = "";

        private Int32 FLedgerNumber;

        /// <summary>This is set to prevent infinite cascades</summary>
        public Int32 FIAmUpdating = 0;

        private String strOldDetailCostCentreCode; // this string is used to detect that the user has renamed an existing Cost Centre.

        private string FRecentlyUpdatedDetailCostCentreCode = INTERNAL_UNASSIGNED_DETAIL_COSTCENTRE_CODE;
        string FnameForNewCostCentre;
        private CostCentreNodeDetails FCurrentCostCentre;
        private Boolean FInitialised = false;

        /// <summary>
        /// Setup the CostCentre hierarchy of this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                CostCentreNodeDetails.FLedgerNumber = FLedgerNumber;
                FMainDS = TRemote.MFinance.Setup.WebConnectors.LoadCostCentreHierarchy(FLedgerNumber);
                ucoCostCentreTree.RunOnceOnActivationManual(this);
                ucoCostCentreTree.PopulateTreeView(FMainDS);

                ucoCostCentreList.RunOnceOnActivationManual(this);
                ucoCostCentreList.PopulateListView(FMainDS, FLedgerNumber);
            }
        }

        /// <summary>
        /// Print out the Hierarchy using FastReports template.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilePrint(object sender, EventArgs e)
        {
            TLogging.Log("CostCentreHierarchy.File Print..");
            FastReportsWrapper ReportingEngine = new FastReportsWrapper("Cost Centre Hierarchy");

            if (!ReportingEngine.LoadedOK)
            {
                ReportingEngine.ShowErrorPopup();
                return;
            }

            if (!FMainDS.ACostCentre.Columns.Contains("CostCentrePath"))
            {
                FMainDS.ACostCentre.Columns.Add("CostCentrePath", typeof(String));
                FMainDS.ACostCentre.Columns.Add("CostCentreLevel", typeof(Int32));
            }

            DataView PathView = new DataView(FMainDS.ACostCentre);
            PathView.Sort = "a_cost_centre_code_c";
            TLogging.Log("CostCentreHierarchy.File Print calculating paths..");

            // I need to make the "CostCentrePath" field that will be used to sort the table for printout:
            foreach (DataRowView rv in PathView)
            {
                DataRow Row = rv.Row;
                String Path = Row["a_cost_centre_code_c"].ToString() + '~';
                Int32 Level = 0;
                String ReportsTo = Row["a_cost_centre_to_report_to_c"].ToString();

                while (ReportsTo != "")
                {
                    Int32 ParentIdx = PathView.Find(ReportsTo);

                    if (ParentIdx >= 0)
                    {
                        DataRow ParentRow = PathView[ParentIdx].Row;
                        ReportsTo = ParentRow["a_cost_centre_to_report_to_c"].ToString();
                        Path = ParentRow["a_cost_centre_code_c"].ToString() + "~" + Path;
                        Level++;

                        if (Level > 100) // Surely this is a fault. If I just break here,
                        {
                            break;  // the report will print and I should be able to see what the fault is.
                        }
                    }
                    else
                    {
                        ReportsTo = "";
                    }
                }

                Row["CostCentrePath"] = Path;
                Row["CostCentreLevel"] = Level;
            }

            PathView.Sort = "CostCentrePath";
            DataTable SortedByPath = PathView.ToTable();

            TLogging.Log("CostCentreHierarchy.File Print paths all done.");

            ReportingEngine.RegisterData(SortedByPath, "CostCentreHierarchy");
            TRptCalculator Calc = new TRptCalculator();
            ALedgerRow LedgerRow = FMainDS.ALedger[0];
            Calc.AddParameter("param_ledger_nunmber", LedgerRow.LedgerNumber);
            Calc.AddStringParameter("param_ledger_name", LedgerRow.LedgerName);

            TLogging.Log("CostCentreHierarchy.File Print calling FastReport...");

            if (ModifierKeys.HasFlag(Keys.Control))
            {
                ReportingEngine.DesignReport(Calc);
            }
            else
            {
                ReportingEngine.GenerateReport(Calc);
            }
        }

        private void ShowEquityOrClearingControls(CostCentreNodeDetails AnewSelection)
        {
            FPetraUtilsObject.SuppressChangeDetection = true;
            grpClearing.Visible = (AnewSelection.CostCentreRow.CostCentreType == "Foreign");
            grpEquitySettings.Visible =
                ((!grpClearing.Visible) && (AnewSelection.CostCentreRow.CostCentreCode != MFinanceConstants.INTER_LEDGER_HEADING));
            grpProjectStatusBox.Visible = grpEquitySettings.Visible && AnewSelection.CostCentreRow.PostingCostCentreFlag;
            grpEquitySettings.Height = grpProjectStatusBox.Visible ? 70 : 170;
            FPetraUtilsObject.SuppressChangeDetection = false;
        }

        /// <summary>
        /// Called from the user controls when the user selects a row,
        /// this common method keeps both user controls in sync.
        /// </summary>
        public void SetSelectedCostCentre(CostCentreNodeDetails AnewSelection)
        {
            FCurrentCostCentre = AnewSelection;

            ucoCostCentreList.SelectedCostCentre = AnewSelection;
            ucoCostCentreTree.SelectedCostCentre = AnewSelection;

            pnlDetails.Enabled = (AnewSelection != null);
            ShowEquityOrClearingControls(FCurrentCostCentre);

            if (pnlDetails.Enabled)
            {
                strOldDetailCostCentreCode = FCurrentCostCentre.CostCentreRow.CostCentreCode;
                Console.WriteLine("Current account code is {0}", FCurrentCostCentre.CostCentreRow.CostCentreCode);
            }
        }

        /// <summary>
        /// The ListView only gives me the CostCentreCode
        /// That's OK - I can ask the TreeView to find the actual record.
        /// It calls back to SetSelectedCostCentre, above.
        /// </summary>
        public void SetSelectedCostCentreCode(String AnewCostCentreCode)
        {
            ucoCostCentreTree.SelectNodeByName(AnewCostCentreCode);
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

        private void RunOnceOnActivationManual()
        {
            if (FInitialised) // By the time this is called by infrastructure, I've already called it!
            {
                return;
            }

            FIAmUpdating++;
            FPetraUtilsObject.UnhookControl(pnlDetails, true); // I don't want changes in these values to cause SetChangedFlag - I'll set it myself.
            FPetraUtilsObject.UnhookControl(txtStatus, false); // This control is not to be spied on!

            txtDetailCostCentreName.TextChanged += new EventHandler(UpdateOnControlChanged);
            chkDetailCostCentreActiveFlag.CheckedChanged += new EventHandler(UpdateOnControlChanged);
            cmbDetailCostCentreType.SelectedIndexChanged += new EventHandler(UpdateOnControlChanged);
            txtDetailCostCentreCode.Validated -= ControlValidatedHandler; // Don't trigger validation on change - I need to do it manually

            FPetraUtilsObject.ControlChanged += new TValueChangedHandler(FPetraUtilsObject_ControlChanged);
            FnameForNewCostCentre = Catalog.GetString("NEWCOSTCENTRE");

            txtDetailCostCentreCode.TextChanged += new EventHandler(txtDetailCostCentreCode_TextChanged);
            txtDetailCostCentreCode.Leave += txtDetailCostCentreCode_Leave;
            FPetraUtilsObject.DataSaved += OnHierarchySaved;

            mniFilePrint.Click += FilePrint;
            mniFilePrint.Enabled = true;

            chkDetailSummaryFlag.CheckedChanged += chkDetailSummaryFlag_CheckedChanged;
            chkDetailProjectStatus.CheckedChanged += chkDetailProjectStatus_CheckedChanged;

            if (TAppSettingsManager.GetBoolean("OmBuild", false)) // In OM, no-one needs to see the import or export functions:
            {
                tbrMain.Items.Remove(tbbImportHierarchy);
                tbrMain.Items.Remove(tbbExportHierarchy);

                /* For some reason, this screen never had these menu options!
                 * mnuMain.Items.Remove(mniImportHierarchy);
                 * mnuMain.Items.Remove(mniExportHierarchy);
                 */
            }

            //
            // I have two group boxes, for "Clearing" "Equity Settings" controls.
            // Since any given Cost Centre will have either one or the other, the two groups of controls share the same space,
            // and either one group or the other is shown when a particular Cost Centre is selected.

            grpEquitySettings.Location = grpClearing.Location;

            //
            // I also have the "Project Status" box, which I only show on local, posting Cost Centres.
            // When it is visible, it displays over the bottom half of the Equity Settings group.

            grpProjectStatusBox.Location = grpClearing.Location;
            grpProjectStatusBox.Location = new Point(grpProjectStatusBox.Location.X, grpProjectStatusBox.Location.Y + 75);

            // cmbDetailClearingAccount is a choice of accounts that are descendants of 8500X:
            TFinanceControls.InitialiseClearingAccountList(ref cmbDetailClearingAccount, FLedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD);

            // cmbDetailRetEarningsAccountCode is a choice of Equity accounts:
            TFinanceControls.InitialiseRetEarningsAccountAccountList(ref cmbDetailRetEarningsAccountCode, FLedgerNumber);

            String[] RollupOptions = Enum.GetNames(typeof(CCRollupStyleEnum));
            cmbRollupStyleManual.Items.AddRange(RollupOptions);

            cmbRollupStyleManual.SelectedIndexChanged += cmbRollupStyleManual_SelectedIndexChanged;
            cmbDetailRetEarningsAccountCode.SelectedValueChanged += cmbDetailRetEarningsAccountCode_SelectedValueChanged;
            cmbDetailClearingAccount.SelectedValueChanged += cmbDetailClearingAccount_SelectedValueChanged;
            chkChildrenLocked.CheckedChanged += chkChildrenLocked_CheckedChanged;

            btnRename.Left = 290;

            lblChildrenLocked.Height += 17; // This control wants two lines of text, which YAML doesn't give me.
            lblLockedMessage.Top += 20;
            lblLockedMessage.Height += 17;

            FPetraUtilsObject.SetStatusBarText(cmbRollupStyleManual,
                Catalog.GetString("At year end, how will this Cost Centre be rolled up to the Standard Cost Centre?"));
            FPetraUtilsObject.SetStatusBarText(chkChildrenLocked, Catalog.GetString("All children also have these Equity / Roll-up settings"));
            FIAmUpdating = 0;
            FInitialised = true;
        }

        void chkDetailProjectStatus_CheckedChanged(object sender, EventArgs e)
        {
            dtpDetailProjectConstraintDate.Enabled = chkDetailProjectStatus.Checked;
            txtDetailProjectConstraintAmount.Enabled = chkDetailProjectStatus.Checked;
        }

        void cmbRollupStyleManual_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FIAmUpdating != 0)
            {
                return;
            }

            CCRollupStyleEnum RollupStyle;
            Enum.TryParse <CCRollupStyleEnum>(((String)cmbRollupStyleManual.SelectedItem), out RollupStyle);

            String RollupStyleMsg = "FAULT: Bad Rollup Style.";

            if (chkDetailSummaryFlag.Checked)
            {
                switch (RollupStyle)
                {
                    case CCRollupStyleEnum.Never:
                        RollupStyleMsg =
                            String.Format(Catalog.GetString("At Year End, any balance in children of {0} will remain in the same Cost Centre."),
                                FCurrentCostCentre.CostCentreRow.CostCentreCode);
                        break;

                    case CCRollupStyleEnum.Always:
                        RollupStyleMsg = String.Format(Catalog.GetString("At Year End, any balance in children of {0} will be rolled up to {1}00"),
                        FCurrentCostCentre.CostCentreRow.CostCentreCode, FLedgerNumber);
                        break;

                    case CCRollupStyleEnum.Surplus:
                        RollupStyleMsg =
                            String.Format(Catalog.GetString(
                                    "At Year End, any surplus in children of {0} will be rolled up to {1}00, but any deficit will remain in the same Cost Centre."),
                                FCurrentCostCentre.CostCentreRow.CostCentreCode, FLedgerNumber);
                        break;

                    case CCRollupStyleEnum.Deficit:
                        RollupStyleMsg =
                            String.Format(Catalog.GetString(
                                    "At Year End, any deficit in children of {0} will be rolled up to {1}00, but any surplus will remain in the same Cost Centre."),
                                FCurrentCostCentre.CostCentreRow.CostCentreCode, FLedgerNumber);
                        break;
                }
            }
            else
            {
                switch (RollupStyle)
                {
                    case CCRollupStyleEnum.Never:
                        RollupStyleMsg = String.Format(Catalog.GetString("At Year End, any balance in {0} will remain in {0}"),
                        FCurrentCostCentre.CostCentreRow.CostCentreCode);
                        break;

                    case CCRollupStyleEnum.Always:
                        RollupStyleMsg = String.Format(Catalog.GetString("At Year End, any balance in {0} will be rolled up to {1}00"),
                        FCurrentCostCentre.CostCentreRow.CostCentreCode, FLedgerNumber);
                        break;

                    case CCRollupStyleEnum.Surplus:
                        RollupStyleMsg =
                            String.Format(Catalog.GetString(
                                    "At Year End, any surplus in {0} will be rolled up to {1}00, but any deficit will remain in {0}."),
                                FCurrentCostCentre.CostCentreRow.CostCentreCode, FLedgerNumber);
                        break;

                    case CCRollupStyleEnum.Deficit:
                        RollupStyleMsg =
                            String.Format(Catalog.GetString(
                                    "At Year End, any deficit in {0} will be rolled up to {1}00, but any surplus will remain in {0}."),
                                FCurrentCostCentre.CostCentreRow.CostCentreCode, FLedgerNumber);
                        break;
                }
            }

            MessageBox.Show(RollupStyleMsg, "Roll-up style", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (FCurrentCostCentre.CostCentreRow.RollupStyle.IndexOf("LOCK_") == 0)
            {
                GetDataFromControlsManual();
                MakeMyChildrenLikeMe(FCurrentCostCentre.linkedTreeNode);
            }
        }

        void cmbDetailRetEarningsAccountCode_SelectedValueChanged(object sender, EventArgs e)
        {
            if (FIAmUpdating != 0)
            {
                return;
            }

            if (FCurrentCostCentre.CostCentreRow.RollupStyle.IndexOf("LOCK_") == 0)
            {
                GetDataFromControlsManual();
                MakeMyChildrenLikeMe(FCurrentCostCentre.linkedTreeNode);
            }
        }

        void cmbDetailClearingAccount_SelectedValueChanged(object sender, EventArgs e)
        {
            if (FIAmUpdating != 0)
            {
                return;
            }

            if (cmbDetailClearingAccount.GetSelectedString() != MFinanceConstants.ICH_ACCT_ICH)
            {
                MessageBox.Show(String.Format("NOTE that changing from {0} means that this Cost Centre is excluded from Stewardship processing.",
                        MFinanceConstants.ICH_ACCT_ICH),
                    "Clearing Account", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>The Equity / roll-up settings for this Summary node will be copied to all its children.
        ///          METHOD IS RECURSIVE
        /// </summary>
        /// <param name="Parent"></param>
        void MakeMyChildrenLikeMe(TreeNode Parent)
        {
            ACostCentreRow MyRow = ((CostCentreNodeDetails)Parent.Tag).CostCentreRow;
            String RollupStyle = MyRow.RollupStyle;

            if (RollupStyle.IndexOf("LOCK_") == 0) // Child nodes will not be locked (apart from by me)
            {
                RollupStyle = RollupStyle.Substring(5);
            }

            foreach (TreeNode ChildNode in Parent.Nodes)
            {
                ACostCentreRow ChildRow = ((CostCentreNodeDetails)ChildNode.Tag).CostCentreRow;
                ChildRow.RetEarningsAccountCode = MyRow.RetEarningsAccountCode;
                ChildRow.RollupStyle = RollupStyle;
                MakeMyChildrenLikeMe(ChildNode);
            }
        }

        void chkChildrenLocked_CheckedChanged(object sender, EventArgs e)
        {
            if (FIAmUpdating != 0)
            {
                return;
            }

            FIAmUpdating++;

            if (chkChildrenLocked.Checked)
            {
                if (MessageBox.Show("Settings in Child Cost Centres will be overwritten with these settings.", "Lock Children",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
                    == System.Windows.Forms.DialogResult.OK)
                {
                    GetDataFromControlsManual();
                    MakeMyChildrenLikeMe(FCurrentCostCentre.linkedTreeNode);
                }
                else
                {
                    chkChildrenLocked.Checked = false;
                }
            }
            else
            {
                MessageBox.Show("Child Cost Centres can be set individually.", "Unlock Children");
            }

            FIAmUpdating--;
            GetDataFromControls();
            ShowDetails(FCurrentCostCentre.CostCentreRow);
        }

        void txtDetailCostCentreCode_Leave(object sender, EventArgs e)
        {
            CheckCostCentreValueChanged();
        }

        private void OnHierarchySaved(System.Object sender, TDataSavedEventArgs e)
        {
            SetPrimaryKeyReadOnly(false);
        }

        private void txtDetailCostCentreCode_TextChanged(object sender, EventArgs e)
        {
            if (FIAmUpdating > 0)
            {
                return;
            }

            if (FCurrentCostCentre.CostCentreRow.SystemCostCentreFlag)
            {
                FIAmUpdating++;
                txtDetailCostCentreCode.Text = strOldDetailCostCentreCode;
                FIAmUpdating--;
                MessageBox.Show(Catalog.GetString("System Cost Centre Code cannot be changed."),
                    Catalog.GetString("Rename Cost Centre"),
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (strOldDetailCostCentreCode.IndexOf(FnameForNewCostCentre) == 0)  // This is the first time the name is being set?
            {
                FPetraUtilsObject_ControlChanged(txtDetailCostCentreCode);
                return;
            }

            bool ICanEditCostCentreCode = (FCurrentCostCentre.IsNew || !FPetraUtilsObject.HasChanges);

            btnRename.Visible = (strOldDetailCostCentreCode != txtDetailCostCentreCode.Text) && ICanEditCostCentreCode;

            if (!FCurrentCostCentre.IsNew && FPetraUtilsObject.HasChanges) // The user wants to change a cost centre code, but I can't allow it.
            {
                FIAmUpdating++;
                txtDetailCostCentreCode.Text = strOldDetailCostCentreCode;
                FIAmUpdating--;
                MessageBox.Show(Catalog.GetString(
                        "Cost Centre Codes cannot be changed while there are other unsaved changes.\r\nSave first, then rename the Cost Centre."),
                    Catalog.GetString("Rename Cost Centre"),
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        void FPetraUtilsObject_ControlChanged(Control Sender)
        {
            ucoCostCentreTree.SetNodeLabel(txtDetailCostCentreCode.Text, txtDetailCostCentreName.Text);
        }

        private String IsLockedByParent(CostCentreNodeDetails ThisNode)
        {
            TreeNode Parent = ThisNode.linkedTreeNode.Parent;

            if (Parent == null)
            {
                return "";
            }

            CostCentreNodeDetails ParentNode = (CostCentreNodeDetails)Parent.Tag;

            if (ParentNode.CostCentreRow.RollupStyle.IndexOf("LOCK_") == 0)
            {
                return ParentNode.CostCentreRow.CostCentreCode;
            }
            else
            {
                return IsLockedByParent(ParentNode);
            }
        }

        private void ShowDetailsManual(ACostCentreRow ARow)
        {
            if (ARow != null)
            {
                // I allow the user to attempt to change the primary key,
                // but if the selected record is not new, AND they have made any other changes,
                // the txtDetailCostCentreCode_TextChanged method will disallow any change.
                SetPrimaryKeyReadOnly(false);
                btnRename.Visible = false;
                chkDetailSummaryFlag.Checked = !ARow.PostingCostCentreFlag;
                chkDetailSummaryFlag.Enabled = !ARow.SystemCostCentreFlag;

                String ParentLock = IsLockedByParent(FCurrentCostCentre);

                if (ParentLock != "") // Equity / Roll-up Controls cannot be changed:
                {
                    grpEquitySettings.Enabled = false;
                    lblLockedMessage.Text = "These settings are derived from " + ParentLock;
                }
                else
                {
                    grpEquitySettings.Enabled = true;
                    lblLockedMessage.Text = "";
                }

                //
                // The value of ARow.RollupStyle may contain "LOCK_" in addition to the enum value.
                // This means that all children are constrained to have this value.
                String RollupStyle = ARow.RollupStyle;

                if (RollupStyle.IndexOf("LOCK_") == 0)
                {
                    RollupStyle = RollupStyle.Substring(5);
                    FCurrentCostCentre.IsLocked = true;

                    if (ParentLock == "")
                    {
                        lblLockedMessage.Text = "These settings apply to child Cost Centres.";
                    }
                }
                else
                {
                    FCurrentCostCentre.IsLocked = false;

                    if ((ParentLock == "") && (!ARow.PostingCostCentreFlag))
                    {
                        lblLockedMessage.Text = "THESE SETTINGS ARE NOT USED:\n  child Cost Centres have individual settings.";
                    }
                }

                chkChildrenLocked.Visible = (!ARow.PostingCostCentreFlag);
                lblChildrenLocked.Visible = (!ARow.PostingCostCentreFlag);

                chkChildrenLocked.Checked = FCurrentCostCentre.IsLocked;
                CCRollupStyleEnum cmbVal;

                if (!Enum.TryParse <CCRollupStyleEnum>(RollupStyle, out cmbVal))
                {
                    RollupStyle = Enum.GetName(typeof(CCRollupStyleEnum), CCRollupStyleEnum.Always);
                }

                cmbRollupStyleManual.SetSelectedString(RollupStyle);
                chkDetailProjectStatus_CheckedChanged(null, null);
            }
        }

        private void AddNewCostCentre(Object sender, EventArgs e)
        {
            if (FCurrentCostCentre == null)
            {
                MessageBox.Show(Catalog.GetString("You can only add a new cost centre after selecting a parent cost centre"));
                return;
            }

            if (ValidateAllData(true, true))
            {
                FCurrentCostCentre.GetAttrributes();
                ACostCentreRow ParentRow = FCurrentCostCentre.CostCentreRow;

                if (!FCurrentCostCentre.CanHaveChildren.Value)
                {
                    MessageBox.Show(
                        String.Format(Catalog.GetString("Cost Centre {0} is in use and cannot become a summary Cost Centre."),
                            ParentRow.CostCentreCode), Catalog.GetString("NewCostCentre"));
                    return;
                }

                Int32 countNewCostCentre = 0;
                string newCostCentreName = FnameForNewCostCentre;

                if (FMainDS.ACostCentre.Rows.Find(new object[] { FLedgerNumber, newCostCentreName }) != null)
                {
                    while (FMainDS.ACostCentre.Rows.Find(new object[] { FLedgerNumber, newCostCentreName + countNewCostCentre.ToString() }) != null)
                    {
                        countNewCostCentre++;
                    }

                    newCostCentreName += countNewCostCentre.ToString();
                }

                ParentRow.PostingCostCentreFlag = false;
                FCurrentCostCentre.CanDelete = false;

                ACostCentreRow newCostCentreRow = FMainDS.ACostCentre.NewRowTyped();
                newCostCentreRow.CostCentreCode = newCostCentreName;
                newCostCentreRow.LedgerNumber = FLedgerNumber;
                newCostCentreRow.CostCentreActiveFlag = true;

                newCostCentreRow.ClearingAccount = ParentRow.ClearingAccount;
                newCostCentreRow.RollupStyle = ParentRow.RollupStyle;
                newCostCentreRow.RetEarningsAccountCode = ParentRow.RetEarningsAccountCode;

                //
                // OM - specific code ahead!
                if (ParentRow.CostCentreCode == "ILT")
                {
                    newCostCentreRow.CostCentreType = "Foreign";
                }
                else
                {
                    newCostCentreRow.CostCentreType = ParentRow.CostCentreType;
                }

                newCostCentreRow.PostingCostCentreFlag = true;
                newCostCentreRow.CostCentreToReportTo = ParentRow.CostCentreCode;
                FMainDS.ACostCentre.Rows.Add(newCostCentreRow);

                FRecentlyUpdatedDetailCostCentreCode = INTERNAL_UNASSIGNED_DETAIL_COSTCENTRE_CODE;

                FIAmUpdating++;
                ShowDetails(newCostCentreRow);
                FIAmUpdating--;

                ucoCostCentreTree.AddNewCostCentre(newCostCentreRow);
                txtDetailCostCentreCode.Focus();
                txtDetailCostCentreCode.SelectAll();
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void ExportHierarchy(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Catalog.GetString("Save changes before exporting."), Catalog.GetString(
                        "Export Hierarchy"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(TRemote.MFinance.Setup.WebConnectors.ExportCostCentreHierarchy(FLedgerNumber));

            TImportExportDialogs.ExportWithDialog(doc, Catalog.GetString("Save Cost Centre Hierarchy to file"));
        }

        private void ImportHierarchy(object sender, EventArgs e)
        {
            // TODO: open file; only will work if there are no GLM records and transactions yet
            XmlDocument doc = TImportExportDialogs.ImportWithDialog(Catalog.GetString("Load Cost Centre Hierarchy from file"));

            if (doc == null)
            {
                // import was cancelled
                return;
            }

            TVerificationResultCollection VerificationResultCol;

            if (!TRemote.MFinance.Setup.WebConnectors.ImportCostCentreHierarchy(FLedgerNumber, TXMLParser.XmlToString(doc), out VerificationResultCol))
            {
                MessageBox.Show(VerificationResultCol.BuildVerificationResultString(),
                    Catalog.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // refresh the screen
                FMainDS = TRemote.MFinance.Setup.WebConnectors.LoadCostCentreHierarchy(FLedgerNumber);
                ucoCostCentreTree.PopulateTreeView(FMainDS);
                MessageBox.Show("Import of new Cost Centre Hierarchy has been successful",
                    Catalog.GetString("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CheckForInvalidCostCentre()
        {
            foreach (ACostCentreRow CheckRow in FMainDS.ACostCentre.Rows)
            {
                if (CheckRow.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                if (CheckRow.CostCentreCode == "")
                {
                    MessageBox.Show(
                        Catalog.GetString(
                            "Cost centre code is empty.\r\nSupply a valid cost centre code."),
                        Catalog.GetString("GL Cost Centre Hierarchy"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    ucoCostCentreTree.SelectNodeByName(CheckRow.CostCentreCode);
                    return true;
                }

                if (CheckRow.CostCentreCode.IndexOf(FnameForNewCostCentre) == 0)
                {
                    MessageBox.Show(
                        String.Format(Catalog.GetString("{0} is not a valid Cost Centre code."),
                            CheckRow.CostCentreCode),
                        Catalog.GetString("GL Cost Centre Hierarchy"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    ucoCostCentreTree.SelectNodeByName(CheckRow.CostCentreCode);
                    return true;
                }
            }

            return false;
        }

        private TSubmitChangesResult StoreManualCode(ref GLSetupTDS ASubmitDS, out TVerificationResultCollection AVerificationResult)
        {
            //
            // The Controls might not have changed, but if they have, this will make the tree look right:
            FPetraUtilsObject_ControlChanged(null);

            //
            // I'll look through and check whether any of the cost centres still have "NEWCOSTCENTRE"..
            //
            if (CheckForInvalidCostCentre())
            {
                AVerificationResult = null;
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataErrorOccured);
                return TSubmitChangesResult.scrInfoNeeded;
            }

            ucoCostCentreTree.MarkAllNodesCommitted();
            TSubmitChangesResult ServerResult =
                TRemote.MFinance.Setup.WebConnectors.SaveGLSetupTDS(FLedgerNumber, ref ASubmitDS, out AVerificationResult);
            TDataCache.TMFinance.RefreshCacheableFinanceTable(Shared.TCacheableFinanceTablesEnum.CostCentreList, FLedgerNumber);
            return ServerResult;
        }

        /// <summary>
        /// Delete the row in the editor
        /// NOTE: A cost centre with children cannot be deleted.
        /// </summary>
        private void DeleteCostCentre(Object sender, EventArgs e)
        {
            if (FCurrentCostCentre == null)
            {
                return;
            }

            FCurrentCostCentre.GetAttrributes();

            if (FCurrentCostCentre.CanDelete.Value)
            {
                ACostCentreRow SelectedRow = FCurrentCostCentre.CostCentreRow;
                TreeNode DeletedNode = FCurrentCostCentre.linkedTreeNode;
                TreeNode ParentNode = DeletedNode.Parent;
                SelectedRow.Delete();
                ucoCostCentreTree.DeleteSelectedCostCentre();

                // FCurrentCostCentre is now the parent of the CostCentre that was just deleted.
                // If just I added a sub-tree and I decide I don't want it, I might be about to remove the parent too.
                if (FCurrentCostCentre != null)
                {
                    FCurrentCostCentre.GetAttrributes();
                }

                FPetraUtilsObject.SetChangedFlag();
            }
            else
            {
                MessageBox.Show(
                    Catalog.GetString("This Cost Centre Code is in use and cannot be deleted.") + "\n" + FCurrentCostCentre.Msg,
                    Catalog.GetString("Delete Cost Centre"));
            }
        }

        /// <summary>
        /// The fact that the CostCentreCode is the database primary key causes SO MUCH GRIEF all over the system!
        /// </summary>
        /// <param name="ACostCentreNode"></param>
        /// <returns></returns>
        private Boolean ProtectedChangeOfPrimaryKey(CostCentreNodeDetails ACostCentreNode)
        {
            String NewValue = txtDetailCostCentreCode.Text;

            try
            {
                ACostCentreNode.CostCentreRow.CostCentreCode = NewValue;
                return true;
            }
            catch (System.Data.ConstraintException)
            {
                txtDetailCostCentreCode.Text = strOldDetailCostCentreCode;

                FRecentlyUpdatedDetailCostCentreCode = INTERNAL_UNASSIGNED_DETAIL_COSTCENTRE_CODE;

                ShowStatus(Catalog.GetString("Account Code change REJECTED!"));

                MessageBox.Show(String.Format(
                        Catalog.GetString(
                            "Renaming Cost Centre Code '{0}' to '{1}' is not possible because a Cost Centre Code by the name of '{1}' already exists."
                            +
                            "\r\n\r\n--> Cost Centre Code reverted to previous value."),
                        strOldDetailCostCentreCode, NewValue),
                    Catalog.GetString("Renaming Not Possible - Conflicts With Existing Cost Centre Code"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtDetailCostCentreCode.Focus();
            }
            return false;
        }

        /// <summary>
        /// Change CostCentre Value
        ///
        /// The Cost Centre code is a foreign key in loads of tables,
        /// so renaming a Cost Centre code is a major work on the server.
        /// From the client's perspective it's easy - we just need to ask the server to do it!
        ///
        /// </summary>
        private bool CheckCostCentreValueChanged()
        {
            if ((FIAmUpdating > 0) || (strOldDetailCostCentreCode == null))
            {
                return false;
            }

            String strNewDetailCostCentreCode = txtDetailCostCentreCode.Text;
            bool changeAccepted = false;

            if (strNewDetailCostCentreCode != FRecentlyUpdatedDetailCostCentreCode)
            {
                if (strNewDetailCostCentreCode != strOldDetailCostCentreCode)
                {
                    if (strOldDetailCostCentreCode.IndexOf(FnameForNewCostCentre) < 0) // If they're just changing this from the initial value, don't show warning.
                    {
                        if (MessageBox.Show(String.Format(Catalog.GetString(
                                        "You have changed the Cost Centre Code from '{0}' to '{1}'.\r\n\r\n" +
                                        "Please confirm that you want to rename this Cost Centre Code by choosing 'OK'.\r\n\r\n" +
                                        "(Renaming will take a few moments, then the form will be re-loaded.)"),
                                    strOldDetailCostCentreCode,
                                    strNewDetailCostCentreCode), Catalog.GetString("Rename Cost Centre Code: Confirmation"),
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                        {
                            txtDetailCostCentreCode.Text = strOldDetailCostCentreCode;
                            btnRename.Visible = false;
                            return false;
                        }
                    }

                    this.UseWaitCursor = true;
                    this.Refresh();

                    FRecentlyUpdatedDetailCostCentreCode = strNewDetailCostCentreCode;
                    changeAccepted = ProtectedChangeOfPrimaryKey(FCurrentCostCentre);

                    if (changeAccepted)
                    {
                        ucoCostCentreTree.SetNodeLabel(FCurrentCostCentre.CostCentreRow);

                        if (FCurrentCostCentre.IsNew)
                        {
                            // This is the code for changes in "un-committed" nodes:
                            // there are no references to this new row yet, apart from children nodes, so I can just change them here and carry on!

                            // fixup children nodes
                            ucoCostCentreTree.FixupChildrenAfterCostCentreNameChange();
                            strOldDetailCostCentreCode = strNewDetailCostCentreCode;
                            FPetraUtilsObject.HasChanges = true;
                        }
                        else
                        {
                            ShowStatus(Catalog.GetString("Updating Cost Centre Code change - please wait.\r\n"));
                            TVerificationResultCollection VerificationResults;

                            // If this code was previously in the DB, I need to assume that there may be transactions posted against it.
                            // There's a server call I need to use, and after the call I need to re-load this page.
                            // (No other changes will be lost, because the change would not have been allowed if there were already changes.)
                            this.Cursor = Cursors.WaitCursor;
                            bool Success = TRemote.MFinance.Setup.WebConnectors.RenameCostCentreCode(strOldDetailCostCentreCode,
                                strNewDetailCostCentreCode,
                                FLedgerNumber,
                                out VerificationResults);
                            this.Cursor = Cursors.Default;

                            if (Success)
                            {
                                TDataCache.TMFinance.RefreshCacheableFinanceTable(Shared.TCacheableFinanceTablesEnum.CostCentreList, FLedgerNumber);
                                FMainDS = TRemote.MFinance.Setup.WebConnectors.LoadCostCentreHierarchy(FLedgerNumber);
                                strOldDetailCostCentreCode = "";
                                FIAmUpdating++;
                                FPetraUtilsObject.SuppressChangeDetection = false;
                                txtDetailCostCentreCode.Text = "";
                                FPetraUtilsObject.SuppressChangeDetection = false;
                                FCurrentCostCentre = null;
                                ucoCostCentreTree.PopulateTreeView(FMainDS);
                                ucoCostCentreList.PopulateListView(FMainDS, FLedgerNumber);
                                FIAmUpdating--;
                                ucoCostCentreTree.SelectNodeByName(FRecentlyUpdatedDetailCostCentreCode);
                                ClearStatus();
                                changeAccepted = true;
                                ShowStatus(String.Format("Cost Centre Code changed to '{0}'.\r\n", strNewDetailCostCentreCode));
                            }
                            else
                            {
                                MessageBox.Show(VerificationResults.BuildVerificationResultString(), Catalog.GetString("Rename Cost Centre Code"));
                            }
                        }
                    } // if changeAccepted

                    this.UseWaitCursor = false;
                } // if changed
            }

            return changeAccepted;
        }

        /// <summary>
        /// I need to find out whether the specified AccountCode can be allowed.
        /// </summary>
        private void GetDetailsFromControlsManual(ACostCentreRow ARow)
        {
            //
            // If changing the PrimaryKey to that specified causes a contraints error,
            // I'll catch it here, issue a warning, and return the control to the "safe" value.
            ProtectedChangeOfPrimaryKey(FCurrentCostCentre);

            //
            // The value of ARow.RollupStyle may contain "LOCK_" in addition to the enum value.
            // This means that all children are constrained to have this value.
            String RollupStyle = cmbRollupStyleManual.GetSelectedString();

            if (chkChildrenLocked.Checked)
            {
                RollupStyle = "LOCK_" + RollupStyle;
            }

            ARow.RollupStyle = RollupStyle;
        }

        private void GetDataFromControlsManual()
        {
            if (FCurrentCostCentre != null)
            {
                ACostCentreRow SelectedRow = GetSelectedDetailRowManual();
                GetDetailsFromControls(SelectedRow);
                FCurrentCostCentre.CostCentreRow.PostingCostCentreFlag = !chkDetailSummaryFlag.Checked;
            }
        }

        void chkDetailSummaryFlag_CheckedChanged(object sender, EventArgs e)
        {
            if ((FCurrentCostCentre != null) && (FIAmUpdating == 0)) // Only look into this if the user has changed it...
            {
                FCurrentCostCentre.GetAttrributes();

                if (chkDetailSummaryFlag.Checked) // I can't allow this to be made a summary if it has transactions posted:
                {
                    if (!FCurrentCostCentre.CanHaveChildren.Value)
                    {
                        MessageBox.Show(String.Format("Cost Centre {0} cannot be made summary because it has tranactions posted to it.",
                                FCurrentCostCentre.CostCentreRow.CostCentreCode), "Summary Cost Centre");
                        chkDetailSummaryFlag.Checked = false;
                    }
                }
                else // I can't allow this Cost Centre to be a posting Cost Centre if it has children:
                {
                    if (FCurrentCostCentre.linkedTreeNode.Nodes.Count > 0)
                    {
                        MessageBox.Show(String.Format("Cost Centre {0} cannot be made postable while it has children.",
                                FCurrentCostCentre.CostCentreRow.CostCentreCode), "Summary Cost Centre");
                        chkDetailSummaryFlag.Checked = true;
                    }
                }
            }
        }

        /// <summary>
        /// When the user selects a new CostCentre, unload the controls and check whether that's OK.
        /// </summary>
        /// <returns>false if the user must stay on the current row and fix the problem</returns>
        public Boolean CheckControlsValidateOk()
        {
            GetDetailsFromControls(FCurrentCostCentre.CostCentreRow);
            return ValidateAllData(true, true);
        }

        /// <summary>
        /// Essentially a public wrapper for ShowDetails.
        /// </summary>
        public void PopulateControlsAfterRowSelection()
        {
            if (!FInitialised)
            {
                RunOnceOnActivationManual();
            }

            bool hasChanges = FPetraUtilsObject.HasChanges;

            FIAmUpdating++;
            FPetraUtilsObject.SuppressChangeDetection = true;

            if (FCurrentCostCentre == null)
            {
                ShowDetails(null);
            }
            else
            {
                ShowDetails(FCurrentCostCentre.CostCentreRow);
                FCurrentCostCentre.GetAttrributes();
            }

            FPetraUtilsObject.SuppressChangeDetection = false;
            FIAmUpdating--;

            if ((FCurrentCostCentre != null) && (FCurrentCostCentre.CanHaveChildren.HasValue))
            {
                tbbAddNewCostCentre.Enabled = FCurrentCostCentre.CanHaveChildren.Value;
                tbbAddNewCostCentre.ToolTipText = (tbbAddNewCostCentre.Enabled) ? "New Cost Centre" : FCurrentCostCentre.Msg;
            }
            else
            {
                tbbAddNewCostCentre.Enabled = false;
                tbbAddNewCostCentre.ToolTipText = "";
            }

            if ((FCurrentCostCentre != null) && (FCurrentCostCentre.CanDelete.HasValue))
            {
                tbbDeleteCostCentre.Enabled = FCurrentCostCentre.CanDelete.Value;
                tbbDeleteCostCentre.ToolTipText = (tbbDeleteCostCentre.Enabled) ? "Delete Cost Centre" : FCurrentCostCentre.Msg;
            }
            else
            {
                tbbDeleteCostCentre.Enabled = false;
                tbbDeleteCostCentre.ToolTipText = "";
            }

            FPetraUtilsObject.HasChanges = hasChanges;
        }

        private ACostCentreRow GetSelectedDetailRowManual()
        {
            if (FCurrentCostCentre != null)
            {
                return FCurrentCostCentre.CostCentreRow;
            }
            else
            {
                return null;
            }
        }

        private void UpdateOnControlChanged(Object sender, EventArgs e)
        {
            if (FIAmUpdating == 0)
            {
                ACostCentreRow Row = GetSelectedDetailRowManual();

                if ((Row != null)
                    && (cmbDetailCostCentreType.GetSelectedString() != Row.CostCentreType))
                {
                    if (Row.SystemCostCentreFlag)
                    {
                        MessageBox.Show(
                            Catalog.GetString(
                                "This is a System Cost Centre and cannot be changed."),
                            Catalog.GetString("Cost Centre Type"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        FIAmUpdating++;
                        cmbDetailCostCentreType.SetSelectedString(Row.CostCentreType);
                        FIAmUpdating--;
                    }
                    else // It's not a system Cost Centre, but probably I still shouldn't be changing it...
                    {
                        if (MessageBox.Show(
                                Catalog.GetString(
                                    "Are you sure you want to change this?\n" +
                                    "Changing from Local to foreign, or vice versa,\n" +
                                    "will affect the reporting of foreign ledgers.\n" +
                                    "In OM, Foreign Cost Centres are children of ILT.\n" +
                                    "All other Cost Centres are Local."),
                                Catalog.GetString("Cost Centre Type"),
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Stop) == System.Windows.Forms.DialogResult.No)
                        {
                            FIAmUpdating++;
                            cmbDetailCostCentreType.SetSelectedString(Row.CostCentreType);
                            FIAmUpdating--;
                        }
                    }
                }

                if (CheckCostCentreValueChanged())
                {
                    return;
                }  // If not changed, or the rename didn't happen, I can carry on...

                if (
                    (Row.CostCentreActiveFlag != chkDetailCostCentreActiveFlag.Checked)
                    || (Row.CostCentreType != cmbDetailCostCentreType.GetSelectedString())
                    || (Row.CostCentreCode != txtDetailCostCentreCode.Text)
                    || (Row.CostCentreName != txtDetailCostCentreName.Text)
                    )
                {
                    FPetraUtilsObject.SetChangedFlag();
                }

                GetDataFromControlsManual();
                ucoCostCentreTree.SetNodeLabel(Row);
            }
        } // UpdateOnControlChanged

        private void LinkPartnerCostCentre(object sender, EventArgs e)
        {
            TFrmLinkPartnerCostCentreDialog PartnerLinkScreen = new TFrmLinkPartnerCostCentreDialog(this);

            PartnerLinkScreen.LedgerNumber = FLedgerNumber;
            PartnerLinkScreen.Show();
        }  // LinkPartnerCostCentre

        /// <summary>
        /// The Tree control can't set the selectedNode unless it's in focus
        /// (I guess this is a bug)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTabChange(object sender, EventArgs e)
        {
            if (ucoCostCentreTree.Visible)
            {
                ucoCostCentreList.CollapseFilterFind();

                ucoCostCentreTree.Focus();
                ucoCostCentreTree.RefreshSelectedCostCentre();
            }
            else
            {
                ucoCostCentreList.UpdateRecordNumberDisplay();
                ucoCostCentreList.Focus();
            }
        }

        private bool CanCloseManual()
        {
            return FPetraUtilsObject.ChangesWereAbandonded || !CheckCostCentreValueChanged();
        }
    } // TFrmGLCostCentreHierarchy
} // namespace