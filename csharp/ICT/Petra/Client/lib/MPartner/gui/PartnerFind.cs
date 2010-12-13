// auto generated with nant generateWinforms from PartnerFind.yaml and template windowFind
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
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{

  /// auto generated: Partner Find OpenPetra.org
  public partial class TPartnerFindScreen: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraModuleUtils FPetraUtilsObject;

    /// constructor
    public TPartnerFindScreen(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.tpgFindPartner.Text = Catalog.GetString("Find by Partner Details");
      this.tpgFindBankDetails.Text = Catalog.GetString("Find by Bank Details");
      this.btnHelp.Text = Catalog.GetString("&Help");
      this.btnCancel.Text = Catalog.GetString("&Cancel");
      this.btnFullyLoadData.Text = Catalog.GetString("Fully load Data");
      this.btnAccept.Text = Catalog.GetString("&Accept");
      this.tbbEditPartner.Text = Catalog.GetString("Edit Partner");
      this.tbbNewPartner.Text = Catalog.GetString("New Partner");
      this.tbbSeparator0.Text = Catalog.GetString("Separator");
      this.tbbPartnerInfo.Text = Catalog.GetString("Partner Info");
      this.mniFileWorkWithLastPartner.Text = Catalog.GetString("&Work with Last Partner...");
      this.mniFileRecentPartners.Text = Catalog.GetString("&Recent Partners");
      this.mniFileNewPartner.Text = Catalog.GetString("&New Partner...");
      this.mniFileNewPartnerWithShepherdFamily.Text = Catalog.GetString("Add &Family with Shepherd...");
      this.mniFileNewPartnerWithShepherdChurch.Text = Catalog.GetString("Add &Church with Shepherd...");
      this.mniFileNewPartnerWithShepherdOrganisation.Text = Catalog.GetString("Add &Organisation with Shepherd...");
      this.mniFileNewPartnerWithShepherdUnit.Text = Catalog.GetString("Add &Unit with Shepherd...");
      this.mniFileNewPartnerWithShepherd.Text = Catalog.GetString("N&ew Partner (Shepherd)");
      this.mniFileEditPartner.Text = Catalog.GetString("&Edit Partner");
      this.mniFileMergePartners.Text = Catalog.GetString("Mer&ge Partners...");
      this.mniFileDeletePartner.Text = Catalog.GetString("&Delete Partner");
      this.mniFileSendEmail.Text = Catalog.GetString("Send E&mail to Partner");
      this.mniFilePrintPartner.Text = Catalog.GetString("&Print Partner");
      this.mniFileExportPartner.Text = Catalog.GetString("E&xport Partner");
      this.mniFileImportPartner.Text = Catalog.GetString("&Import Partner");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniEditSearch.Text = Catalog.GetString("&Search");
      this.mniEditCopyPartnerKey.Text = Catalog.GetString("Copy Partner's Partner &Key");
      this.mniEditCopyAddress.Text = Catalog.GetString("Copy Partner's &Address...");
      this.mniEditCopyEmailAddress.Text = Catalog.GetString("Copy Partner's E&mail Address");
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
      this.mniSeparator0.Text = Catalog.GetString("Separator");
      this.mniMaintainPersonnelData.Text = Catalog.GetString("&Personnel/Individual Data");
      this.mniSeparator1.Text = Catalog.GetString("Separator");
      this.mniMaintainDonorHistory.Text = Catalog.GetString("Donor &History");
      this.mniMaintainRecipientHistory.Text = Catalog.GetString("Recipient Histor&y");
      this.mniMaintainFinanceReports.Text = Catalog.GetString("Finance Report&s");
      this.mniMaintainBankAccounts.Text = Catalog.GetString("Ban&k Accounts");
      this.mniMaintainGiftReceipting.Text = Catalog.GetString("&Gift Receipting");
      this.mniMaintainFinanceDetails.Text = Catalog.GetString("&Finance Details");
      this.mniMaintain.Text = Catalog.GetString("Maintain");
      this.mniMailingGenerateExtract.Text = Catalog.GetString("&Generate Extract From Found Partners...");
      this.mniMailingExtracts.Text = Catalog.GetString("&Extracts...");
      this.mniMailingDuplicateAddressCheck.Text = Catalog.GetString("&Duplicate  Address Check...");
      this.mniMailingMergeAddresses.Text = Catalog.GetString("&Merge  Addresses...");
      this.mniMailingPartnersAtLocation.Text = Catalog.GetString("Find Partners at &Location...");
      this.mniMailingSubscriptionExpNotice.Text = Catalog.GetString("Subscription Expiry &Notices...");
      this.mniMailingSubscriptionCancellation.Text = Catalog.GetString("Subscription  &Cancellation...");
      this.mniMailing.Text = Catalog.GetString("&Mailing");
      this.mniToolsOptions.Text = Catalog.GetString("&Options...");
      this.mniTools.Text = Catalog.GetString("&Tools");
      this.mniViewPartnerInfo.Text = Catalog.GetString("Partner &Info Panel");
      this.mniView.Text = Catalog.GetString("Vie&w");
      this.mniPetraMainMenu.Text = Catalog.GetString("Petra &Main Menu");
      this.mniPetraPartnerModule.Text = Catalog.GetString("Pa&rtner");
      this.mniPetraFinanceModule.Text = Catalog.GetString("&Finance");
      this.mniPetraPersonnelModule.Text = Catalog.GetString("P&ersonnel");
      this.mniPetraConferenceModule.Text = Catalog.GetString("C&onference");
      this.mniPetraFinDevModule.Text = Catalog.GetString("Financial &Development");
      this.mniPetraSysManModule.Text = Catalog.GetString("&System Manager");
      this.mniPetraModules.Text = Catalog.GetString("&OpenPetra");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Partner Find OpenPetra.org");
      #endregion

      FPetraUtilsObject = new TFrmPetraModuleUtils(AParentFormHandle, this, stbMain);
      ucoFindByPartnerDetails.PetraUtilsObject = FPetraUtilsObject;
      ucoFindByPartnerDetails.InitUserControl();
      InitializeManualCode();
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
        if (e.ActionName == "actFileEditPartner")
        {
            tbbEditPartner.Enabled = e.Enabled;
            mniFileEditPartner.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileNewPartner")
        {
            tbbNewPartner.Enabled = e.Enabled;
            mniFileNewPartner.Enabled = e.Enabled;
        }
        if (e.ActionName == "actViewPartnerInfo")
        {
            tbbPartnerInfo.Enabled = e.Enabled;
            mniViewPartnerInfo.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileWorkWithLastPartner")
        {
            mniFileWorkWithLastPartner.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileMergePartners")
        {
            mniFileMergePartners.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileDeletePartner")
        {
            mniFileDeletePartner.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileSendEmail")
        {
            mniFileSendEmail.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFilePrintPartner")
        {
            mniFilePrintPartner.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileExportPartner")
        {
            mniFileExportPartner.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFileImportPartner")
        {
            mniFileImportPartner.Enabled = e.Enabled;
        }
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        if (e.ActionName == "actEditSearch")
        {
            mniEditSearch.Enabled = e.Enabled;
        }
        if (e.ActionName == "actEditCopyPartnerKey")
        {
            mniEditCopyPartnerKey.Enabled = e.Enabled;
        }
        if (e.ActionName == "actEditCopyAddress")
        {
            mniEditCopyAddress.Enabled = e.Enabled;
        }
        if (e.ActionName == "actEditCopyEmailAddress")
        {
            mniEditCopyEmailAddress.Enabled = e.Enabled;
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
        if (e.ActionName == "actMailingGenerateExtract")
        {
            mniMailingGenerateExtract.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMailingExtracts")
        {
            mniMailingExtracts.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMailingDuplicateAddressCheck")
        {
            mniMailingDuplicateAddressCheck.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMailingMergeAddresses")
        {
            mniMailingMergeAddresses.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMailingPartnersAtLocation")
        {
            mniMailingPartnersAtLocation.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMailingSubscriptionExpNotice")
        {
            mniMailingSubscriptionExpNotice.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMailingSubscriptionCancellation")
        {
            mniMailingSubscriptionCancellation.Enabled = e.Enabled;
        }
        if (e.ActionName == "actToolsOptions")
        {
            mniToolsOptions.Enabled = e.Enabled;
        }
        if (e.ActionName == "actMainMenu")
        {
            mniPetraMainMenu.Enabled = e.Enabled;
        }
        if (e.ActionName == "actPartnerModule")
        {
            mniPetraPartnerModule.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFinanceModule")
        {
            mniPetraFinanceModule.Enabled = e.Enabled;
        }
        if (e.ActionName == "actPersonnelModule")
        {
            mniPetraPersonnelModule.Enabled = e.Enabled;
        }
        if (e.ActionName == "actConferenceModule")
        {
            mniPetraConferenceModule.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFinDevModule")
        {
            mniPetraFinDevModule.Enabled = e.Enabled;
        }
        if (e.ActionName == "actSysManModule")
        {
            mniPetraSysManModule.Enabled = e.Enabled;
        }
        mniFileRecentPartner1.Enabled = false;
        mniFileRecentPartner2.Enabled = false;
        mniFileRecentPartner3.Enabled = false;
        mniFileRecentPartner4.Enabled = false;
        mniFileRecentPartner5.Enabled = false;
        mniFileRecentPartner6.Enabled = false;
        mniFileRecentPartner7.Enabled = false;
        mniFileRecentPartner8.Enabled = false;
        mniFileRecentPartner9.Enabled = false;
        mniFileRecentPartner10.Enabled = false;
        mniFileNewPartnerWithShepherdFamily.Enabled = false;
        mniFileNewPartnerWithShepherdChurch.Enabled = false;
        mniFileNewPartnerWithShepherdOrganisation.Enabled = false;
        mniFileNewPartnerWithShepherdUnit.Enabled = false;
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

    /// auto generated
    protected void actMainMenu(object sender, EventArgs e)
    {
        FPetraUtilsObject.OpenMainScreen(sender, e);
    }

    /// auto generated
    protected void actPartnerModule(object sender, EventArgs e)
    {
        FPetraUtilsObject.OpenPartnerModule(sender, e);
    }

    /// auto generated
    protected void actFinanceModule(object sender, EventArgs e)
    {
        FPetraUtilsObject.OpenFinanceModule(sender, e);
    }

    /// auto generated
    protected void actPersonnelModule(object sender, EventArgs e)
    {
        FPetraUtilsObject.OpenPersonnelModule(sender, e);
    }

    /// auto generated
    protected void actConferenceModule(object sender, EventArgs e)
    {
        FPetraUtilsObject.OpenConferenceModule(sender, e);
    }

    /// auto generated
    protected void actFinDevModule(object sender, EventArgs e)
    {
        FPetraUtilsObject.OpenFinDevModule(sender, e);
    }

    /// auto generated
    protected void actSysManModule(object sender, EventArgs e)
    {
        FPetraUtilsObject.OpenSysManModule(sender, e);
    }

#endregion
  }
}
