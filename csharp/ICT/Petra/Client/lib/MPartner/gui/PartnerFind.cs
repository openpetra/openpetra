/* auto generated with nant generateWinforms from PartnerFind.yaml and template windowFind
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
      this.mniFileSearch.Text = Catalog.GetString("&Search");
      this.mniFileWorkWithLastPartner.Text = Catalog.GetString("&Work with Last Partner...");
      this.mniFileRecentPartners.Text = Catalog.GetString("&Recent Partners");
      this.mniFileNewPartner.Text = Catalog.GetString("&New Partner...");
      this.mniFileViewPartner.Text = Catalog.GetString("&View Partner");
      this.mniFileEditPartner.Text = Catalog.GetString("&Edit Partner");
      this.mniFileMergePartners.Text = Catalog.GetString("Mer&ge Partners...");
      this.mniFileDeletePartner.Text = Catalog.GetString("&Delete Partner");
      this.mniFileCopyAddress.Text = Catalog.GetString("Copy Partner's &Address...");
      this.mniFileCopyPartnerKey.Text = Catalog.GetString("Copy Partner's Partner &Key");
      this.mniFileSendEmail.Text = Catalog.GetString("Send E&mail to Partner");
      this.mniFilePrintPartner.Text = Catalog.GetString("&Print Partner");
      this.mniFileExportPartner.Text = Catalog.GetString("E&xport Partner");
      this.mniFileImportPartner.Text = Catalog.GetString("&Import Partner");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniMaintainAddresses.Text = Catalog.GetString("&Addresses...");
      this.mniMaintainPartnerDetails.Text = Catalog.GetString("Partner &Details...");
      this.mniMaintainFoundationDetails.Text = Catalog.GetString("Foundation Details...");
      this.mniMaintainSubscriptions.Text = Catalog.GetString("&Subscriptions...");
      this.mniMaintainSpecialTypes.Text = Catalog.GetString("Special &Types...");
      this.mniMaintainContacts.Text = Catalog.GetString("&Contacts...");
      this.mniMaintainFamilyMembers.Text = Catalog.GetString("Family &Members...");
      this.mniMaintainRelationships.Text = Catalog.GetString("&Relationships...");
      this.mniMaintainInterests.Text = Catalog.GetString("&Interests...");
      this.mniMaintainReminders.Text = Catalog.GetString("R&eminders...");
      this.mniMaintainNotes.Text = Catalog.GetString("&Notes...");
      this.mniMaintainOfficeSpecific.Text = Catalog.GetString("&Local Partner Data...");
      this.mniMaintainOMerField.Text = Catalog.GetString("&Worker Field...");
      this.mniMaintainPersonnelIndividualData.Text = Catalog.GetString("&Personnel/Individual Data...");
      this.mniMaintainDonorHistory.Text = Catalog.GetString("Donor  &History...");
      this.mniMaintainRecipientHistory.Text = Catalog.GetString("Recipient  Histor&y...");
      this.mniMaintainFinanceDetails.Text = Catalog.GetString("&Finance Details...");
      this.mniMaintain.Text = Catalog.GetString("Maintain");
      this.mniMailingGenerateExtract.Text = Catalog.GetString("&Generate Extract From Found Partners...");
      this.mniMailingExtracts.Text = Catalog.GetString("&Extracts...");
      this.mniMailingDuplicateAddressCheck.Text = Catalog.GetString("&Duplicate  Address Check...");
      this.mniMailingMergeAddresses.Text = Catalog.GetString("&Merge  Addresses...");
      this.mniMailingPartnersAtLocation.Text = Catalog.GetString("Find Partners at &Location...");
      this.mniMailingSubscriptionExpNotice.Text = Catalog.GetString("Subscription Expiry &Notices...");
      this.mniMailingSubscriptionCancellation.Text = Catalog.GetString("Subscription  &Cancellation...");
      this.mniMailing.Text = Catalog.GetString("&Mailing");
      this.mniTools.Text = Catalog.GetString("&Tools");
      this.mniPetraMainMenu.Text = Catalog.GetString("Petra &Main Menu");
      this.mniPetraPartnerModule.Text = Catalog.GetString("Pa&rtner");
      this.mniPetraFinanceModule.Text = Catalog.GetString("&Finance");
      this.mniPetraPersonnelModule.Text = Catalog.GetString("P&ersonnel");
      this.mniPetraConferenceModule.Text = Catalog.GetString("C&onference");
      this.mniPetraFinDevModule.Text = Catalog.GetString("Financial &Development");
      this.mniPetraSysManModule.Text = Catalog.GetString("&System Manager");
      this.mniPetraModules.Text = Catalog.GetString("&Petra");
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
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
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
        mniFileSearch.Enabled = false;
        mniFileWorkWithLastPartner.Enabled = false;
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
        mniFileNewPartner.Enabled = false;
        mniFileViewPartner.Enabled = false;
        mniFileEditPartner.Enabled = false;
        mniFileMergePartners.Enabled = false;
        mniFileDeletePartner.Enabled = false;
        mniFileCopyAddress.Enabled = false;
        mniFileCopyPartnerKey.Enabled = false;
        mniFileSendEmail.Enabled = false;
        mniFilePrintPartner.Enabled = false;
        mniFileExportPartner.Enabled = false;
        mniFileImportPartner.Enabled = false;
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
        mniMaintainOMerField.Enabled = false;
        mniMaintainPersonnelIndividualData.Enabled = false;
        mniMaintainDonorHistory.Enabled = false;
        mniMaintainRecipientHistory.Enabled = false;
        mniMaintainFinanceDetails.Enabled = false;
        mniMailingGenerateExtract.Enabled = false;
        mniMailingExtracts.Enabled = false;
        mniMailingDuplicateAddressCheck.Enabled = false;
        mniMailingMergeAddresses.Enabled = false;
        mniMailingPartnersAtLocation.Enabled = false;
        mniMailingSubscriptionExpNotice.Enabled = false;
        mniMailingSubscriptionCancellation.Enabled = false;
        mniTools.Enabled = false;
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
