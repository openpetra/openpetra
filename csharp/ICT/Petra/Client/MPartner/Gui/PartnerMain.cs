// auto generated with nant generateWinforms from PartnerMain.yaml
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
using Ict.Petra.Client.MReporting.Gui; // Implicit reference
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{

  /// auto generated: Partner Module OpenPetra.org
  public partial class TFrmPartnerMain: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraModuleUtils FPetraUtilsObject;

    /// constructor
    public TFrmPartnerMain(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.mniImport.Text = Catalog.GetString("&Import");
      this.mniExport.Text = Catalog.GetString("&Export");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniFindMaintain.Text = Catalog.GetString("&Find && Maintain...");
      this.mniLastPartner.Text = Catalog.GetString("Work with &Last Partner...");
      this.mniLastPartners.Text = Catalog.GetString("&Recent Partners...");
      this.mniExtracts.Text = Catalog.GetString("&Extracts...");
      this.mniReportPartnerByCity.Text = Catalog.GetString("&Partner By City Report (experiment)");
      this.mniReportPartnerAddress.Text = Catalog.GetString("Brief &Address Report");
      this.mniReportBriefFoundation.Text = Catalog.GetString("Brief &Foundation Report");
      this.mniReportSupportingChurches.Text = Catalog.GetString("Supporting &Churches Report");
      this.mniReportValidBankAccount.Text = Catalog.GetString("&Valid Bank Account Report");
      this.mniReportPublicationStatisticalReport.Text = Catalog.GetString("Publication &Statistical Report");
      this.mniReportBulkAddressReport.Text = Catalog.GetString("&Bulk Address Report");
      this.mniReportPartnerContactReport.Text = Catalog.GetString("Partner C&ontact Report");
      this.mniReportSubscriptionReport.Text = Catalog.GetString("Subscri&ption Report");
      this.mniReportLocalPartnerDataReport.Text = Catalog.GetString("&Local Partner Data");
      this.mniReports.Text = Catalog.GetString("&Reports...");
      this.mniNewPartner.Text = Catalog.GetString("New &Partner...");
      this.mniNewPerson.Text = Catalog.GetString("Add &Person");
      this.mniNewFamily.Text = Catalog.GetString("Add &Family");
      this.mniNewChurch.Text = Catalog.GetString("Add &Church");
      this.mniNewPartnerAssistant.Text = Catalog.GetString("&New Partner (assistant)");
      this.mniMergePartners.Text = Catalog.GetString("&Merge Partners");
      this.mniDeletePartner.Text = Catalog.GetString("D&elete Partner");
      this.mniCheckDuplicateAddresses.Text = Catalog.GetString("&Duplicate Address Check");
      this.mniMergeAddresses.Text = Catalog.GetString("Mer&ge Addresses");
      this.mniViewPartnersAtLocation.Text = Catalog.GetString("&View Partners at Location");
      this.mniPartner.Text = Catalog.GetString("P&artner");
      this.mniLabelPrint.Text = Catalog.GetString("&Label Print");
      this.mniMailsortLabelPrint.Text = Catalog.GetString("Mails&ort Label Print");
      this.mniSubscriptionExpiryNotices.Text = Catalog.GetString("Subscription Expiry &Notices");
      this.mniSubscriptionCancellation.Text = Catalog.GetString("Subscription &Cancellation");
      this.mniFormLetterPrint.Text = Catalog.GetString("&Form Letter Print");
      this.mniExtractMailMerge.Text = Catalog.GetString("Extrac&t Mail Merge");
      this.mniMailing.Text = Catalog.GetString("Mailin&g");
      this.mniTodo.Text = Catalog.GetString("Todo");
      this.mniMaintainTables.Text = Catalog.GetString("Maintain &Tables");
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
      this.Text = Catalog.GetString("Partner Module OpenPetra.org");
      #endregion

      FPetraUtilsObject = new TFrmPetraModuleUtils(AParentFormHandle, this, stbMain);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();

    }

    private void FindPartner(object sender, EventArgs e)
    {
        FindPartner(this.Handle);
    }

    private void NewPartner(object sender, EventArgs e)
    {
        NewPartner(this.Handle);
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
        if (e.ActionName == "actFindPartner")
        {
            mniFindMaintain.Enabled = e.Enabled;
        }
        if (e.ActionName == "actNewPartner")
        {
            mniNewPartner.Enabled = e.Enabled;
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
        mniImport.Enabled = false;
        mniExport.Enabled = false;
        mniLastPartner.Enabled = false;
        mniLastPartners.Enabled = false;
        mniExtracts.Enabled = false;
        mniNewPerson.Enabled = false;
        mniNewFamily.Enabled = false;
        mniNewChurch.Enabled = false;
        mniMergePartners.Enabled = false;
        mniDeletePartner.Enabled = false;
        mniCheckDuplicateAddresses.Enabled = false;
        mniMergeAddresses.Enabled = false;
        mniViewPartnersAtLocation.Enabled = false;
        mniLabelPrint.Enabled = false;
        mniMailsortLabelPrint.Enabled = false;
        mniSubscriptionExpiryNotices.Enabled = false;
        mniSubscriptionCancellation.Enabled = false;
        mniFormLetterPrint.Enabled = false;
        mniExtractMailMerge.Enabled = false;
        mniTodo.Enabled = false;
        mniHelpPetraHelp.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
    }

    /// auto generated
    protected void OpenScreenReportPartnerByCity(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPartner.TFrmPartnerByCity frm = new Ict.Petra.Client.MReporting.Gui.MPartner.TFrmPartnerByCity(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenReportPartnerAddress(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPartner.TFrmBriefAddressReport frm = new Ict.Petra.Client.MReporting.Gui.MPartner.TFrmBriefAddressReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenReportBriefFoundation(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPartner.TFrmBriefFoundationReport frm = new Ict.Petra.Client.MReporting.Gui.MPartner.TFrmBriefFoundationReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenReportSupportingChurches(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPartner.TFrmSupportingChurchesReport frm = new Ict.Petra.Client.MReporting.Gui.MPartner.TFrmSupportingChurchesReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenReportValidBankAccount(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPartner.TFrmValidBankAccountReport frm = new Ict.Petra.Client.MReporting.Gui.MPartner.TFrmValidBankAccountReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenReportPublicationStatisticalReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPartner.TFrmPublicationStatisticalReport frm = new Ict.Petra.Client.MReporting.Gui.MPartner.TFrmPublicationStatisticalReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenReportBulkAddressReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPartner.TFrmBulkAddressReport frm = new Ict.Petra.Client.MReporting.Gui.MPartner.TFrmBulkAddressReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenReportPartnerContactReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPartner.TFrmPartnerContactReport frm = new Ict.Petra.Client.MReporting.Gui.MPartner.TFrmPartnerContactReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenReportSubscriptionReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPartner.TFrmSubscriptionReport frm = new Ict.Petra.Client.MReporting.Gui.MPartner.TFrmSubscriptionReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenReportLocalPartnerDataReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPartner.TFrmLocalPartnerDataReport frm = new Ict.Petra.Client.MReporting.Gui.MPartner.TFrmLocalPartnerDataReport(this.Handle);
        frm.Show();
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
