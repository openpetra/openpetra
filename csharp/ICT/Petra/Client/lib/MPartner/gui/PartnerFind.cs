/* auto generated with nant generateWinforms from PartnerFind.yaml
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

      FPetraUtilsObject = new TFrmPetraModuleUtils(AParentFormHandle, this);
      ucoFindByPartnerDetails.PetraUtilsObject = FPetraUtilsObject;
      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
    }

    private void mniCloseClick(object sender, EventArgs e)
    {
        actClose(sender, e);
    }

    private void mniPetraMainMenuClick(object sender, EventArgs e)
    {
        actMainMenu(sender, e);
    }

    private void mniPetraPartnerModuleClick(object sender, EventArgs e)
    {
        actPartnerModule(sender, e);
    }

    private void mniPetraFinanceModuleClick(object sender, EventArgs e)
    {
        actFinanceModule(sender, e);
    }

    private void mniPetraPersonnelModuleClick(object sender, EventArgs e)
    {
        actPersonnelModule(sender, e);
    }

    private void mniPetraConferenceModuleClick(object sender, EventArgs e)
    {
        actConferenceModule(sender, e);
    }

    private void mniPetraFinDevModuleClick(object sender, EventArgs e)
    {
        actFinDevModule(sender, e);
    }

    private void mniPetraSysManModuleClick(object sender, EventArgs e)
    {
        actSysManModule(sender, e);
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
    public TFrmPetraModuleUtils GetUtilObject()
    {
        return (TFrmPetraModuleUtils)FPetraUtilsObject;
    }
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        mniFileSearch.Enabled = false;
        mniFileSeparator1.Enabled = false;
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
        mniFileSeparator2.Enabled = false;
        mniFileNewPartner.Enabled = false;
        mniFileViewPartner.Enabled = false;
        mniFileEditPartner.Enabled = false;
        mniFileMergePartners.Enabled = false;
        mniFileDeletePartner.Enabled = false;
        mniFileSeparator3.Enabled = false;
        mniFileCopyAddress.Enabled = false;
        mniFileCopyPartnerKey.Enabled = false;
        mniFileSendEmail.Enabled = false;
        mniFileSeparator4.Enabled = false;
        mniFilePrintPartner.Enabled = false;
        mniFileSeparator5.Enabled = false;
        mniFileExportPartner.Enabled = false;
        mniFileImportPartner.Enabled = false;
        mniFileSeparator6.Enabled = false;
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
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
        mniMaintainSeparator1.Enabled = false;
        mniMaintainPersonnelIndividualData.Enabled = false;
        mniMaintainSeparator2.Enabled = false;
        mniMaintainDonorHistory.Enabled = false;
        mniMaintainRecipientHistory.Enabled = false;
        mniMaintainFinanceDetails.Enabled = false;
        mniMailingGenerateExtract.Enabled = false;
        mniMailingExtracts.Enabled = false;
        mniMailingSeparator1.Enabled = false;
        mniMailingDuplicateAddressCheck.Enabled = false;
        mniMailingMergeAddresses.Enabled = false;
        mniMailingPartnersAtLocation.Enabled = false;
        mniMailingSeparator2.Enabled = false;
        mniMailingSubscriptionExpNotice.Enabled = false;
        mniMailingSubscriptionCancellation.Enabled = false;
        mniTools.Enabled = false;
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
        mniHelpPetraHelp.Enabled = false;
        mniSeparator0.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniSeparator1.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
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

    /// auto generated
    protected void actClose(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eClose);
    }

#endregion
  }
}
