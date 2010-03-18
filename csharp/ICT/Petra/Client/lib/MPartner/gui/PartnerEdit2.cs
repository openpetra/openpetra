/* auto generated with nant generateWinforms from PartnerEdit2.yaml and template windowTDS
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
 *
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui
{

  /// auto generated: Partner Edit
  public partial class TFrmPartnerEdit2: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;
    private Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS FMainDS;

    /// constructor
    public TFrmPartnerEdit2(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblPartnerKey.Text = Catalog.GetString("Key:");
      this.lblEmpty2.Text = Catalog.GetString("Empty2:");
      this.lblPartnerClass.Text = Catalog.GetString("Class:");
      this.lblTitle.Text = Catalog.GetString("Title/Na&me:");
      this.lblEmpty.Text = Catalog.GetString("Empty:");
      this.lblAddresseeTypeCode.Text = Catalog.GetString("&Addressee Type:");
      this.chkNoSolicitations.Text = Catalog.GetString("No Solicitations");
      this.lblLastGift.Text = Catalog.GetString("Last Gift:");
      this.btnWorkerField.Text = Catalog.GetString("&OMer Field...");
      this.lblPartnerStatus.Text = Catalog.GetString("Partner &Status:");
      this.lblStatusUpdated.Text = Catalog.GetString("Status Updated:");
      this.lblLastContact.Text = Catalog.GetString("Last Contact:");
      this.grpCollapsible.Text = Catalog.GetString("Partner");
      this.lblTest.Text = Catalog.GetString("Test only:");
      this.tpgAddresses.Text = Catalog.GetString("Addresses ({0})");
      this.tpgDetails.Text = Catalog.GetString("Partner Details");
      this.tpgSubscriptions.Text = Catalog.GetString("Subscriptions ({0})");
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
      this.Text = Catalog.GetString("Partner Edit");
      #endregion

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this, stbMain);
      FMainDS = new Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS();
      FPetraUtilsObject.SetStatusBarText(txtPartnerKey, Catalog.GetString("Enter the partner key (SiteID + Number)"));
      FPetraUtilsObject.SetStatusBarText(txtPartnerClass, Catalog.GetString("Select a partner class"));
      FPetraUtilsObject.SetStatusBarText(txtTitle, Catalog.GetString("e.g. Family, Mr & Mrs, Herr und Frau"));
      FPetraUtilsObject.SetStatusBarText(txtFirstName, Catalog.GetString("Enter the person's full first name"));
      FPetraUtilsObject.SetStatusBarText(txtFamilyName, Catalog.GetString("Enter a Last Name/Surname/Family Name"));
      FPetraUtilsObject.SetStatusBarText(cmbAddresseeTypeCode, Catalog.GetString("Enter an addressee type code"));
      cmbAddresseeTypeCode.InitialiseUserControl();
      FPetraUtilsObject.SetStatusBarText(chkNoSolicitations, Catalog.GetString("Set this if the partner does not want extra mailings"));
      FPetraUtilsObject.SetStatusBarText(cmbPartnerStatus, Catalog.GetString("Select a partner status"));
      cmbPartnerStatus.InitialiseUserControl();
      ucoPartnerDetails.PetraUtilsObject = FPetraUtilsObject;
      ucoPartnerDetails.MainDS = FMainDS;
      ucoPartnerDetails.InitUserControl();
      tabPartners.SelectedIndex = 0;
      TabSelectionChanged(null, null);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();

    }

    private void TFrmPetra_Activated(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Activated(sender, e);
    }

    private void TFrmPetra_Load(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Load(sender, e);
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

    private void ShowData(PPartnerRow ARow)
    {
        txtPartnerKey.Text = String.Format("{0:0000000000}", ARow.PartnerKey);
        txtPartnerKey.ReadOnly = (ARow.RowState != DataRowState.Added);
        if (ARow.IsPartnerClassNull())
        {
            txtPartnerClass.Text = String.Empty;
        }
        else
        {
            txtPartnerClass.Text = ARow.PartnerClass;
        }
        if (FMainDS.PFamily[0].IsTitleNull())
        {
            txtTitle.Text = String.Empty;
        }
        else
        {
            txtTitle.Text = FMainDS.PFamily[0].Title;
        }
        if (FMainDS.PFamily[0].IsFirstNameNull())
        {
            txtFirstName.Text = String.Empty;
        }
        else
        {
            txtFirstName.Text = FMainDS.PFamily[0].FirstName;
        }
        if (FMainDS.PFamily[0].IsFamilyNameNull())
        {
            txtFamilyName.Text = String.Empty;
        }
        else
        {
            txtFamilyName.Text = FMainDS.PFamily[0].FamilyName;
        }
        if (ARow.IsAddresseeTypeCodeNull())
        {
            cmbAddresseeTypeCode.SelectedIndex = -1;
        }
        else
        {
            cmbAddresseeTypeCode.SetSelectedString(ARow.AddresseeTypeCode);
        }
        if (ARow.IsNoSolicitationsNull())
        {
            chkNoSolicitations.Checked = false;
        }
        else
        {
            chkNoSolicitations.Checked = ARow.NoSolicitations;
        }
        if (ARow.IsStatusCodeNull())
        {
            cmbPartnerStatus.SelectedIndex = -1;
        }
        else
        {
            cmbPartnerStatus.SetSelectedString(ARow.StatusCode);
        }
    }

    private void GetDataFromControls(PPartnerRow ARow)
    {
        if (txtTitle.Text.Length == 0)
        {
            FMainDS.PFamily[0].SetTitleNull();
        }
        else
        {
            FMainDS.PFamily[0].Title = txtTitle.Text;
        }
        if (txtFirstName.Text.Length == 0)
        {
            FMainDS.PFamily[0].SetFirstNameNull();
        }
        else
        {
            FMainDS.PFamily[0].FirstName = txtFirstName.Text;
        }
        if (txtFamilyName.Text.Length == 0)
        {
            FMainDS.PFamily[0].SetFamilyNameNull();
        }
        else
        {
            FMainDS.PFamily[0].FamilyName = txtFamilyName.Text;
        }
        if (cmbAddresseeTypeCode.SelectedIndex == -1)
        {
            ARow.SetAddresseeTypeCodeNull();
        }
        else
        {
            ARow.AddresseeTypeCode = cmbAddresseeTypeCode.GetSelectedString();
        }
        ARow.NoSolicitations = chkNoSolicitations.Checked;
        if (cmbPartnerStatus.SelectedIndex == -1)
        {
            ARow.SetStatusCodeNull();
        }
        else
        {
            ARow.StatusCode = cmbPartnerStatus.GetSelectedString();
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
        GetDataFromControls(FMainDS.PPartner[0]);

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

            if (FPetraUtilsObject.HasChanges)
            {
                FPetraUtilsObject.WriteToStatusBar("Saving data...");
                this.Cursor = Cursors.WaitCursor;

                TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrOK;
                TVerificationResultCollection VerificationResult;

                Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS SubmitDS = FMainDS.GetChangesTyped(true);

                // Submit changes to the PETRAServer
                try
                {
//                    SubmissionResult = TRemote.MPartner.Gui.WebConnectors.SavePartnerEditTDS(ref SubmitDS, out VerificationResult);
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
                    bool ReturnValue = false;

                    // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
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
                        break;

                    case TSubmitChangesResult.scrNothingToBeSaved:

                        // TODO scrNothingToBeSaved
                        break;

                    case TSubmitChangesResult.scrInfoNeeded:

                        // TODO scrInfoNeeded
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
        /// change the toolbars that are associated with the tabs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabSelectionChanged(System.Object sender, EventArgs e)
        {
            TabPage currentTab = tabPartners.TabPages[tabPartners.SelectedIndex];

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
  }
}
