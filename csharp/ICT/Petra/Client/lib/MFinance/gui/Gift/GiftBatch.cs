// auto generated with nant generateWinforms from GiftBatch.yaml and template windowTDS
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{

  /// auto generated: Gift Batches
  public partial class TFrmGiftBatch: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;
    private Ict.Petra.Shared.MFinance.Gift.Data.GiftBatchTDS FMainDS;

    private SortedList<TDynamicLoadableUserControls, UserControl> FTabSetup;
    private event TTabPageEventHandler FTabPageEvent;
    private Ict.Petra.Client.MFinance.Gui.Gift.TUC_GiftBatches FUcoBatches;
    private Ict.Petra.Client.MFinance.Gui.Gift.TUC_GiftTransactions FUcoTransactions;

    /// <summary>
    /// Enumeration of dynamic loadable UserControls which are used
    /// on the Tabs of a TabControl. AUTO-GENERATED, don't modify by hand!
    /// </summary>
    public enum TDynamicLoadableUserControls
    {
        ///<summary>Denotes dynamic loadable UserControl FUcoBatches</summary>
        dlucBatches,
        ///<summary>Denotes dynamic loadable UserControl FUcoTransactions</summary>
        dlucTransactions,
    }

    /// constructor
    public TFrmGiftBatch(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.tpgBatches.Text = Catalog.GetString("Batches");
      this.tpgTransactions.Text = Catalog.GetString("Transactions");
      this.tbbSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.tbbSave.Text = Catalog.GetString("&Save");
      this.mniFileSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.mniFileSave.Text = Catalog.GetString("&Save");
      this.mniFilePrint.Text = Catalog.GetString("&Print...");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniEditUndoCurrentField.Text = Catalog.GetString("Undo &Current Field");
      this.mniEditUndoScreen.Text = Catalog.GetString("&Undo Screen");
      this.mniEditFind.Text = Catalog.GetString("&Find...");
      this.mniEdit.Text = Catalog.GetString("&Edit");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Gift Batches");
      #endregion

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this, stbMain);
      FMainDS = new Ict.Petra.Shared.MFinance.Gift.Data.GiftBatchTDS();
      InitializeManualCode();
      tabGiftBatch.SelectedIndex = 0;
      TabSelectionChanged(null, null);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();

    }

    private void TFrmPetra_Activated(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Activated(sender, e);
    }

    private void TFrmPetra_Closing(object sender, CancelEventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Closing(sender, e);
    }

    private void Form_KeyDown(object sender, KeyEventArgs e)
    {
        FPetraUtilsObject.Form_KeyDown(sender, e);
    }

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position

    }

    private void GetDataFromControls()
    {
        if(FUcoBatches != null)
        {
            FUcoBatches.GetDataFromControls();
        }
        if(FUcoTransactions != null)
        {
            FUcoTransactions.GetDataFromControls();
        }
    }

#region Implement interface functions

    /// auto generated
    public void RunOnceOnActivation()
    {
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    public void HookupAllControls()
    {
    }

    /// auto generated
    public void HookupAllInContainer(Control container)
    {
        FPetraUtilsObject.HookupAllInContainer(container);
    }

    /// auto generated
    public bool CanClose()
    {
        return FPetraUtilsObject.CanClose();
    }

    /// auto generated
    public TFrmPetraUtils GetPetraUtilsObject()
    {
        return (TFrmPetraUtils)FPetraUtilsObject;
    }

    /// auto generated
    public void FileSave(object sender, EventArgs e)
    {
        SaveChanges();
    }

    /// <summary>
    /// save the changes on the screen
    /// </summary>
    /// <returns></returns>
    public bool SaveChanges()
    {
        FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

//TODO?  still needed?      FMainDS.AApDocument.Rows[0].BeginEdit();
        GetDataFromControls();

        // TODO: verification

        if (FPetraUtilsObject.VerificationResultCollection.Count == 0)
        {
            foreach (DataTable InspectDT in FMainDS.Tables)
            {
                foreach (DataRow InspectDR in InspectDT.Rows)
                {
                    InspectDR.EndEdit();
                }
            }

            if (!FPetraUtilsObject.HasChanges)
            {
                return true;
            }
            else
            {
                FPetraUtilsObject.WriteToStatusBar("Saving data...");
                this.Cursor = Cursors.WaitCursor;

                TSubmitChangesResult SubmissionResult;
                TVerificationResultCollection VerificationResult;

                Ict.Petra.Shared.MFinance.Gift.Data.GiftBatchTDS SubmitDS = FMainDS.GetChangesTyped(true);

                if (SubmitDS == null)
                {
                    // There is nothing to be saved.
                    // Update UI
                    FPetraUtilsObject.WriteToStatusBar(Catalog.GetString("There is nothing to be saved."));
                    this.Cursor = Cursors.Default;

                    // We don't have unsaved changes anymore
                    FPetraUtilsObject.DisableSaveButton();

                    return true;
                }

                // Submit changes to the PETRAServer
                try
                {
                    SubmissionResult = TRemote.MFinance.Gift.WebConnectors.SaveGiftBatchTDS(ref SubmitDS, out VerificationResult);
                }
                catch (System.Net.Sockets.SocketException)
                {
                    FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("The PETRA Server cannot be reached! Data cannot be saved!",
                        "No Server response",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    bool ReturnValue = false;

                    // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
/* TODO ESecurityDBTableAccessDeniedException
*                  catch (ESecurityDBTableAccessDeniedException Exp)
*                  {
*                      FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
*                      this.Cursor = Cursors.Default;
*                      // TODO TMessages.MsgSecurityException(Exp, this.GetType());
*                      bool ReturnValue = false;
*                      // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
*                      return ReturnValue;
*                  }
*/
                catch (EDBConcurrencyException)
                {
                    FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                    this.Cursor = Cursors.Default;

                    // TODO TMessages.MsgDBConcurrencyException(Exp, this.GetType());
                    bool ReturnValue = false;

                    // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
                catch (Exception exp)
                {
                    FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                    this.Cursor = Cursors.Default;
                    TLogging.Log(
                        "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + exp.ToString(),
                        TLoggingType.ToLogfile);
                    MessageBox.Show(
                        "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine +
                        "For details see the log file: " + TLogging.GetLogFileName(),
                        "Server connection error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);

                    // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return false;
                }

                switch (SubmissionResult)
                {
                    case TSubmitChangesResult.scrOK:

                        // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                        FMainDS.AcceptChanges();

                        // Merge back with data from the Server (eg. for getting Sequence values)
                        FMainDS.Merge(SubmitDS, false);

                        // need to accept the new modification ID
                        FMainDS.AcceptChanges();

                        // Update UI
                        FPetraUtilsObject.WriteToStatusBar("Data successfully saved.");
                        this.Cursor = Cursors.Default;

                        // TODO EnableSave(false);

                        // We don't have unsaved changes anymore
                        FPetraUtilsObject.DisableSaveButton();

                        // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return true;

                    case TSubmitChangesResult.scrError:

                        // TODO scrError
                        this.Cursor = Cursors.Default;
                        break;

                    case TSubmitChangesResult.scrNothingToBeSaved:

                        // TODO scrNothingToBeSaved
                        this.Cursor = Cursors.Default;
                        return true;

                    case TSubmitChangesResult.scrInfoNeeded:

                        // TODO scrInfoNeeded
                        this.Cursor = Cursors.Default;
                        break;
                }
            }
        }

        return false;
    }
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actSave")
        {
            tbbSave.Enabled = e.Enabled;
            mniFileSave.Enabled = e.Enabled;
        }
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        mniFilePrint.Enabled = false;
        mniEditUndoCurrentField.Enabled = false;
        mniEditUndoScreen.Enabled = false;
        mniEditFind.Enabled = false;
        mniHelpPetraHelp.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
    }

    /// auto generated
    protected void actClose(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eClose);
    }

#endregion

        private ToolStrip PreviouslyMergedToolbarItems = null;
        private ToolStrip PreviouslyMergedMenuItems = null;

        /// <summary>
        /// Changes the toolbars that are associated with the Tabs.
        /// Optionally dynamically loads UserControls that are associated with the Tabs.
        /// AUTO-GENERATED, don't modify by hand!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabSelectionChanged(System.Object sender, EventArgs e)
        {
            bool FirstTabPageSelectionChanged = false;
            TabPage currentTab = tabGiftBatch.TabPages[tabGiftBatch.SelectedIndex];

            if (FTabSetup == null)
            {
                FTabSetup = new SortedList<TDynamicLoadableUserControls, UserControl>();
                FirstTabPageSelectionChanged = true;
            }

            if (FirstTabPageSelectionChanged)
            {
                // The first time we run this Method we exit straight away!
                return;
            }

            if (tabGiftBatch.SelectedTab == tpgBatches)
            {
                if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucBatches))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoBatches = (Ict.Petra.Client.MFinance.Gui.Gift.TUC_GiftBatches)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucBatches);
                    FUcoBatches.MainDS = FMainDS;
                    FUcoBatches.PetraUtilsObject = FPetraUtilsObject;
                    FUcoBatches.InitUserControl();

                    OnTabPageEvent(new TTabPageEventArgs(tpgBatches, FUcoBatches, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    OnTabPageEvent(new TTabPageEventArgs(tpgBatches, FUcoBatches, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoBatches.AdjustAfterResizing();
                    }
                }
            }
            if (tabGiftBatch.SelectedTab == tpgTransactions)
            {
                if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucTransactions))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoTransactions = (Ict.Petra.Client.MFinance.Gui.Gift.TUC_GiftTransactions)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucTransactions);
                    FUcoTransactions.MainDS = FMainDS;
                    FUcoTransactions.PetraUtilsObject = FPetraUtilsObject;
                    FUcoTransactions.InitUserControl();

                    OnTabPageEvent(new TTabPageEventArgs(tpgTransactions, FUcoTransactions, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    OnTabPageEvent(new TTabPageEventArgs(tpgTransactions, FUcoTransactions, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoTransactions.AdjustAfterResizing();
                    }
                }
            }

            if (PreviouslyMergedToolbarItems != null)
            {
                ToolStripManager.RevertMerge(tbrMain, PreviouslyMergedToolbarItems);
                PreviouslyMergedToolbarItems = null;
            }

            if (PreviouslyMergedMenuItems != null)
            {
                ToolStripManager.RevertMerge(mnuMain, PreviouslyMergedMenuItems);
                PreviouslyMergedMenuItems = null;
            }

            Control[] tabToolbar = currentTab.Controls.Find("tbrTabPage", true);
            if (tabToolbar.Length == 1)
            {
                ToolStrip ItemsToMerge = (ToolStrip) tabToolbar[0];
                ItemsToMerge.Visible = false;
                foreach (ToolStripItem item in ItemsToMerge.Items)
                {
                    item.MergeAction = MergeAction.Append;
                }
                ToolStripManager.Merge(ItemsToMerge, tbrMain);

                PreviouslyMergedToolbarItems = ItemsToMerge;
            }

            Control[] tabMenu = currentTab.Controls.Find("mnuTabPage", true);
            if (tabMenu.Length == 1)
            {
                ToolStrip ItemsToMerge = (ToolStrip) tabMenu[0];
                ItemsToMerge.Visible = false;
                Int32 NewPosition = mnuMain.Items.IndexOf(mniHelp);
                foreach (ToolStripItem item in ItemsToMerge.Items)
                {
                    item.MergeAction = MergeAction.Insert;
                    item.MergeIndex = NewPosition++;
                }
                ToolStripManager.Merge(ItemsToMerge, mnuMain);

                PreviouslyMergedMenuItems = ItemsToMerge;
            }
        }

    private void OnTabPageEvent(TTabPageEventArgs e)
    {
        if (FTabPageEvent != null)
        {
            FTabPageEvent(this, e);
        }
    }

    /// <summary>
    /// Creates UserControls on request. AUTO-GENERATED, don't modify by hand!
    /// </summary>
    /// <param name="AUserControl">UserControl to load.</param>
    private UserControl DynamicLoadUserControl(TDynamicLoadableUserControls AUserControl)
    {
        UserControl ReturnValue = null;

        switch (AUserControl)
        {
            case TDynamicLoadableUserControls.dlucBatches:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCBatches = new Panel();
                pnlHostForUCBatches.AutoSize = true;
                pnlHostForUCBatches.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCBatches.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCBatches.Padding = new System.Windows.Forms.Padding(2);
                tpgBatches.Controls.Add(pnlHostForUCBatches);

                // Create the UserControl
                Ict.Petra.Client.MFinance.Gui.Gift.TUC_GiftBatches ucoBatches = new Ict.Petra.Client.MFinance.Gui.Gift.TUC_GiftBatches();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucBatches, ucoBatches);
                ucoBatches.Location = new Point(0, 2);
                ucoBatches.Dock = DockStyle.Fill;
                pnlHostForUCBatches.Controls.Add(ucoBatches);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                 */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCBatches.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCBatches.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoBatches;
                break;
            case TDynamicLoadableUserControls.dlucTransactions:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCTransactions = new Panel();
                pnlHostForUCTransactions.AutoSize = true;
                pnlHostForUCTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCTransactions.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCTransactions.Padding = new System.Windows.Forms.Padding(2);
                tpgTransactions.Controls.Add(pnlHostForUCTransactions);

                // Create the UserControl
                Ict.Petra.Client.MFinance.Gui.Gift.TUC_GiftTransactions ucoTransactions = new Ict.Petra.Client.MFinance.Gui.Gift.TUC_GiftTransactions();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucTransactions, ucoTransactions);
                ucoTransactions.Location = new Point(0, 2);
                ucoTransactions.Dock = DockStyle.Fill;
                pnlHostForUCTransactions.Controls.Add(ucoTransactions);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                 */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCTransactions.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoTransactions;
                break;
        }

        return ReturnValue;
    }
  }
}
