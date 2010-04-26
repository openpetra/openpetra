/* auto generated with nant generateWinforms from PartnerEdit2.yaml and template windowEditUIConnector
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

    /// <summary>holds a reference to the Proxy object of the Serverside UIConnector</summary>
    private Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors.IPartnerUIConnectorsPartnerEdit FUIConnector = null;

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
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();

      FUIConnector = TRemote.MPartner.Partner.UIConnectors.PartnerEdit2();
      // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
      TEnsureKeepAlive.Register(FUIConnector);
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

        if (FUIConnector != null)
        {
            // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
            TEnsureKeepAlive.UnRegister(FUIConnector);
            FUIConnector = null;
        }
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
  }
}
