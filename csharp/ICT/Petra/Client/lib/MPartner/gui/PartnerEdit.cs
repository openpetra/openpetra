// auto generated with nant generateWinforms from PartnerEdit.yaml and template windowEditUIConnector
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
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui
{

  /// auto generated: Partner Edit
  public partial class TFrmPartnerEdit: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    /// <summary>holds a reference to the Proxy object of the Serverside UIConnector</summary>
    private Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors.IPartnerUIConnectorsPartnerEdit FUIConnector = null;

    /// constructor
    public TFrmPartnerEdit(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.tbbSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.tbbSave.Text = Catalog.GetString("&Save");
      this.tbbNewPartner.Text = Catalog.GetString("&New Partner");
      this.tbbSeparator0.Text = Catalog.GetString("Separator");
      this.tbbViewPartnerData.Text = Catalog.GetString("&Partner Data");
      this.tbbViewPersonnelData.Text = Catalog.GetString("P&ersonnel Data");
      this.tbbViewFinanceData.Text = Catalog.GetString("&Finance Data");
      this.mniFileNewPartner.Text = Catalog.GetString("&New Partner...");
      this.mniFileNewPartnerWithShepherdPerson.Text = Catalog.GetString("Add &Person with Shepherd...");
      this.mniFileNewPartnerWithShepherdFamily.Text = Catalog.GetString("Add &Family with Shepherd...");
      this.mniFileNewPartnerWithShepherdChurch.Text = Catalog.GetString("Add &Church with Shepherd...");
      this.mniFileNewPartnerWithShepherdOrganisation.Text = Catalog.GetString("Add &Organisation with Shepherd...");
      this.mniFileNewPartnerWithShepherdUnit.Text = Catalog.GetString("Add &Unit with Shepherd...");
      this.mniFileNewPartnerWithShepherd.Text = Catalog.GetString("N&ew Partner (Shepherd)");
      this.mniFileSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.mniFileSave.Text = Catalog.GetString("&Save");
      this.mniFilePrint.Text = Catalog.GetString("&Print...");
      this.mniDeactivatePartner.Text = Catalog.GetString("Deactivate Partner");
      this.mniFileDeletePartner.Text = Catalog.GetString("&Delete THIS Partner...");
      this.mniSeparator2.Text = Catalog.GetString("Separator");
      this.mniFileSendEmail.Text = Catalog.GetString("Send E&mail to Partner");
      this.mniSeparator3.Text = Catalog.GetString("Separator");
      this.mniFilePrintSection.Text = Catalog.GetString("P&rint Section...");
      this.mniSeparator4.Text = Catalog.GetString("Separator");
      this.mniFileExportPartner.Text = Catalog.GetString("E&xport Partner");
      this.mniSeparator5.Text = Catalog.GetString("Separator");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniEditUndoCurrentField.Text = Catalog.GetString("Undo &Current Field");
      this.mniEditUndoScreen.Text = Catalog.GetString("&Undo Screen");
      this.mniEditFind.Text = Catalog.GetString("&Find...");
      this.mniEdit.Text = Catalog.GetString("&Edit");
      this.mniMaintainAddresses.Text = Catalog.GetString("&Addresses");
      this.mniMaintainPartnerDetails.Text = Catalog.GetString("Partner &Details");
      this.mniMaintainFoundationDetails.Text = Catalog.GetString("Foundation Details");
      this.mniMaintainSubscriptions.Text = Catalog.GetString("&Subscriptions");
      this.mniMaintainSpecialTypes.Text = Catalog.GetString("Special &Types");
      this.mniMaintainContacts.Text = Catalog.GetString("&Contacts");
      this.mniMaintainFamilyMembers.Text = Catalog.GetString("Fa&mily Members");
      this.mniMaintainRelationships.Text = Catalog.GetString("&Relationships");
      this.mniMaintainInterests.Text = Catalog.GetString("&Interests");
      this.mniMaintainReminders.Text = Catalog.GetString("&Reminders");
      this.mniMaintainNotes.Text = Catalog.GetString("&Notes");
      this.mniMaintainLocalPartnerData.Text = Catalog.GetString("&Local Partner Data");
      this.mniMaintainWorkerField.Text = Catalog.GetString("&Worker Field");
      this.mniSeparator7.Text = Catalog.GetString("Separator");
      this.mniMaintainPersonnelData.Text = Catalog.GetString("&Personnel/Individual Data");
      this.mniSeparator8.Text = Catalog.GetString("Separator");
      this.mniMaintainDonorHistory.Text = Catalog.GetString("Donor &History");
      this.mniMaintainRecipientHistory.Text = Catalog.GetString("Recipient Histor&y");
      this.mniMaintainFinanceReports.Text = Catalog.GetString("Finance Report&s");
      this.mniMaintainBankAccounts.Text = Catalog.GetString("Ban&k Accounts");
      this.mniMaintainGiftReceipting.Text = Catalog.GetString("&Gift Receipting");
      this.mniMaintainFinanceDetails.Text = Catalog.GetString("&Finance Details");
      this.mniMaintain.Text = Catalog.GetString("Ma&intain");
      this.mniViewUpperScreenPartExpanded.Text = Catalog.GetString("&Expanded");
      this.mniViewUpperScreenPartCollapsed.Text = Catalog.GetString("&Collapsed");
      this.mniViewUpperScreenPart.Text = Catalog.GetString("&Upper Screen Part");
      this.mniViewPartnerData.Text = Catalog.GetString("&Partner Data");
      this.mniViewPersonnelData.Text = Catalog.GetString("P&ersonnel Data");
      this.mniViewFinanceData.Text = Catalog.GetString("&Finance Data");
      this.mniView.Text = Catalog.GetString("Vie&w");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniSeparator11.Text = Catalog.GetString("Separator");
      this.mniHelpVideoTutorial.Text = Catalog.GetString("&Video Tutorial for Partner Edit Screen...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Partner Edit");
      #endregion

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this, stbMain);
      ucoUpperPart.PetraUtilsObject = FPetraUtilsObject;
      ucoUpperPart.MainDS = FMainDS;
      ucoUpperPart.InitUserControl();
      ucoLowerPart.PetraUtilsObject = FPetraUtilsObject;
      ucoLowerPart.MainDS = FMainDS;
      ucoLowerPart.InitUserControl();
      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();

      FUIConnector = TRemote.MPartner.Partner.UIConnectors.PartnerEdit();
      // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
      TEnsureKeepAlive.Register(FUIConnector);
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

        if (FUIConnector != null)
        {
            // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
            TEnsureKeepAlive.UnRegister(FUIConnector);
            FUIConnector = null;
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
        if (e.ActionName == "actViewPartnerData")
        {
            tbbViewPartnerData.Enabled = e.Enabled;
            mniViewPartnerData.Enabled = e.Enabled;
        }
        if (e.ActionName == "actViewPersonnelData")
        {
            tbbViewPersonnelData.Enabled = e.Enabled;
            mniViewPersonnelData.Enabled = e.Enabled;
        }
        if (e.ActionName == "actViewFinanceData")
        {
            tbbViewFinanceData.Enabled = e.Enabled;
            mniViewFinanceData.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileNewPartner")
        {
            mniFileNewPartner.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileNewPartnerWithShepherdPerson")
        {
            mniFileNewPartnerWithShepherdPerson.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileNewPartnerWithShepherdFamily")
        {
            mniFileNewPartnerWithShepherdFamily.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileNewPartnerWithShepherdChurch")
        {
            mniFileNewPartnerWithShepherdChurch.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileNewPartnerWithShepherdOrganisation")
        {
            mniFileNewPartnerWithShepherdOrganisation.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileNewPartnerWithShepherdUnit")
        {
            mniFileNewPartnerWithShepherdUnit.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFilePrint")
        {
            mniFilePrint.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileDeletePartner")
        {
            mniFileDeletePartner.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileSendEmail")
        {
            mniFileSendEmail.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFilePrintSection")
        {
            mniFilePrintSection.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileExportPartner")
        {
            mniFileExportPartner.Enabled = e.Enabled;
        }
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainAddresses")
        {
            mniMaintainAddresses.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainPartnerDetails")
        {
            mniMaintainPartnerDetails.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainFoundationDetails")
        {
            mniMaintainFoundationDetails.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainSubscriptions")
        {
            mniMaintainSubscriptions.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainSpecialTypes")
        {
            mniMaintainSpecialTypes.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainContacts")
        {
            mniMaintainContacts.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainFamilyMembers")
        {
            mniMaintainFamilyMembers.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainRelationships")
        {
            mniMaintainRelationships.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainInterests")
        {
            mniMaintainInterests.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainReminders")
        {
            mniMaintainReminders.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainNotes")
        {
            mniMaintainNotes.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainLocalPartnerData")
        {
            mniMaintainLocalPartnerData.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainWorkerField")
        {
            mniMaintainWorkerField.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainPersonnelData")
        {
            mniMaintainPersonnelData.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainDonorHistory")
        {
            mniMaintainDonorHistory.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainRecipientHistory")
        {
            mniMaintainRecipientHistory.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainFinanceReports")
        {
            mniMaintainFinanceReports.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainBankAccounts")
        {
            mniMaintainBankAccounts.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainGiftReceipting")
        {
            mniMaintainGiftReceipting.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMaintainFinanceDetails")
        {
            mniMaintainFinanceDetails.Enabled = e.Enabled;
        }
        if (e.ActionName == "actViewUpperScreenPartExpanded")
        {
            mniViewUpperScreenPartExpanded.Enabled = e.Enabled;
        }
        if (e.ActionName == "actViewUpperScreenPartCollapsed")
        {
            mniViewUpperScreenPartCollapsed.Enabled = e.Enabled;
        }
        if (e.ActionName == "actHelpVideoTutorial")
        {
            mniHelpVideoTutorial.Enabled = e.Enabled;
        }
        mniDeactivatePartner.Enabled = false;
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
