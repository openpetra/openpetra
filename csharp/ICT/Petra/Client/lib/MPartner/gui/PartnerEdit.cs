// auto generated with nant generateWinforms from PartnerEdit.yaml
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
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{

  /// auto generated: Partner Edit
  public partial class TPartnerEditDSWinForm: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    /// constructor
    public TPartnerEditDSWinForm(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.tbbSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.tbbSave.Text = Catalog.GetString("&Save");
      this.tbbNew.Text = Catalog.GetString("New Partner");
      this.tbbTogglePartner.Text = Catalog.GetString("Partner Data");
      this.tbbTogglePersonnel.Text = Catalog.GetString("Personnel Data");
      this.tbbToggleFinance.Text = Catalog.GetString("Finance Data");
      this.mniFileSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.mniFileSave.Text = Catalog.GetString("&Save");
      this.mniFilePrint.Text = Catalog.GetString("&Print...");
      this.mniFileNew.Text = Catalog.GetString("&New Partner...");
      this.mniFileDeactivatePartner.Text = Catalog.GetString("Deacti&vate Partner...");
      this.mniFileDeletePartner.Text = Catalog.GetString("&Delete THIS Partner...");
      this.mniFileCopyPartnerKey.Text = Catalog.GetString("Copy Partner's Partner &Key");
      this.mniFileCopyAddress.Text = Catalog.GetString("Copy Partner's &Address...");
      this.mniFileSendEmail.Text = Catalog.GetString("&Send Email");
      this.mniFilePrintSection.Text = Catalog.GetString("P&rint Section...");
      this.mniFileExportPartner.Text = Catalog.GetString("&Export Partner");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniEditUndoCurrentField.Text = Catalog.GetString("Undo &Current Field");
      this.mniEditUndoScreen.Text = Catalog.GetString("&Undo Screen");
      this.mniEditFind.Text = Catalog.GetString("&Find...");
      this.mniEditFindNewAddress.Text = Catalog.GetString("Find New &Address...");
      this.mniEdit.Text = Catalog.GetString("&Edit");
      this.mniViewPartnerData.Text = Catalog.GetString("&Partner Data");
      this.mniViewPersonnelData.Text = Catalog.GetString("P&ersonnel Data");
      this.mniViewFinanceData.Text = Catalog.GetString("&Finance Data");
      this.mniViewUpperPartExpanded.Text = Catalog.GetString("&Expanded");
      this.mniViewUpperPartCollapsed.Text = Catalog.GetString("&Collapsed");
      this.mniViewUpperScreenPart.Text = Catalog.GetString("&Upper Screen Part");
      this.mniView.Text = Catalog.GetString("View");
      this.mniMaintainAddresses.Text = Catalog.GetString("&Addresses");
      this.mniMaintainPartnerDetails.Text = Catalog.GetString("Partner &Details");
      this.mniMaintainFoundationDetails.Text = Catalog.GetString("Foundation Details");
      this.mniMaintainSubscriptions.Text = Catalog.GetString("&Subscriptions");
      this.mniMaintainSpecialTypes.Text = Catalog.GetString("Special &Types");
      this.mniMaintainContacts.Text = Catalog.GetString("&Contacts...");
      this.mniMaintainFamilyMembers.Text = Catalog.GetString("Family &Members...");
      this.mniMaintainRelationships.Text = Catalog.GetString("&Relationships...");
      this.mniMaintainInterests.Text = Catalog.GetString("&Interests...");
      this.mniMaintainReminders.Text = Catalog.GetString("R&eminders...");
      this.mniMaintainNotes.Text = Catalog.GetString("&Notes");
      this.mniMaintainOfficeSpecific.Text = Catalog.GetString("&Local Partner Data");
      this.mniMaintainWorkerField.Text = Catalog.GetString("&Worker Field...");
      this.mniMaintainPersonnelIndividualData.Text = Catalog.GetString("&Personnel/Individual Data");
      this.mniMaintainDonorHistory.Text = Catalog.GetString("Donor &History...");
      this.mniMaintainRecipientHistory.Text = Catalog.GetString("Recipient Histor&y...");
      this.mniMaintainFinanceReports.Text = Catalog.GetString("Finance Reports");
      this.mniMaintainBankAccounts.Text = Catalog.GetString("Ban&k Accounts");
      this.mniMaintainGiftReceipting.Text = Catalog.GetString("&Gift Receipting");
      this.mniMaintainFinanceDetails.Text = Catalog.GetString("&Finance Details...");
      this.mniMaintain.Text = Catalog.GetString("Maintain");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Partner Edit");
      #endregion

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this, stbMain);
      ucoUpperPart.PetraUtilsObject = FPetraUtilsObject;
      ucoUpperPart.InitUserControl();
      ucoPartnerTabSet.PetraUtilsObject = FPetraUtilsObject;
      ucoPartnerTabSet.InitUserControl();
      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();

    }

    private void TFrmPetra_Activated(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Activated(sender, e);
    }

    private void Form_KeyDown(object sender, KeyEventArgs e)
    {
        FPetraUtilsObject.Form_KeyDown(sender, e);
    }

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position
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
        mniFileNew.Enabled = false;
        mniFileDeactivatePartner.Enabled = false;
        mniFileDeletePartner.Enabled = false;
        mniFileCopyPartnerKey.Enabled = false;
        mniFileCopyAddress.Enabled = false;
        mniFileSendEmail.Enabled = false;
        mniFilePrintSection.Enabled = false;
        mniFileExportPartner.Enabled = false;
        mniEditUndoCurrentField.Enabled = false;
        mniEditUndoScreen.Enabled = false;
        mniEditFind.Enabled = false;
        mniEditFindNewAddress.Enabled = false;
        mniViewPartnerData.Enabled = false;
        mniViewPersonnelData.Enabled = false;
        mniViewFinanceData.Enabled = false;
        mniViewUpperPartExpanded.Enabled = false;
        mniViewUpperPartCollapsed.Enabled = false;
        mniMaintainAddresses.Enabled = false;
        mniMaintainPartnerDetails.Enabled = false;
        mniMaintainFoundationDetails.Enabled = false;
        mniMaintainSubscriptions.Enabled = false;
        mniMaintainSpecialTypes.Enabled = false;
        mniMaintainContacts.Enabled = false;
        mniMaintainFamilyMembers.Enabled = false;
        mniMaintainRelationships.Enabled = false;
        mniMaintainInterests.Enabled = false;
        mniMaintainReminders.Enabled = false;
        mniMaintainNotes.Enabled = false;
        mniMaintainOfficeSpecific.Enabled = false;
        mniMaintainWorkerField.Enabled = false;
        mniMaintainPersonnelIndividualData.Enabled = false;
        mniMaintainDonorHistory.Enabled = false;
        mniMaintainRecipientHistory.Enabled = false;
        mniMaintainFinanceReports.Enabled = false;
        mniMaintainBankAccounts.Enabled = false;
        mniMaintainGiftReceipting.Enabled = false;
        mniMaintainFinanceDetails.Enabled = false;
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
