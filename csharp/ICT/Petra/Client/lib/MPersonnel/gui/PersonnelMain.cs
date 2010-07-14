// auto generated with nant generateWinforms from PersonnelMain.yaml
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

namespace Ict.Petra.Client.MPersonnel.Gui
{

  /// auto generated: Personnel Module OpenPetra.org
  public partial class TFrmPersonnelMain: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraModuleUtils FPetraUtilsObject;

    /// constructor
    public TFrmPersonnelMain(IntPtr AParentFormHandle) : base()
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
      this.mniExtracts.Text = Catalog.GetString("&Extracts...");
      this.mniReportPartnerByCity.Text = Catalog.GetString("Progress Report");
      this.mniGeneralShortTermerReport.Text = Catalog.GetString("General Short Termer Report");
      this.mniAbilitiesReport.Text = Catalog.GetString("Abilities Report");
      this.mniPersonnelEmergencyContactReport.Text = Catalog.GetString("Emergency Contact Report");
      this.mniLanguagesReport.Text = Catalog.GetString("Languages Report");
      this.mniShortTermerReports.Text = Catalog.GetString("Short Termer Reports");
      this.mniIndividualDataReport.Text = Catalog.GetString("Individual Data Report");
      this.mniEmergencyDataReport.Text = Catalog.GetString("Emergency Data Report");
      this.mniLocalPersonnelDataReport.Text = Catalog.GetString("Local Personnel Data Report");
      this.mniBirthdayList.Text = Catalog.GetString("Birthday List");
      this.mniEmergencyContactReport.Text = Catalog.GetString("Emergency Contact Report");
      this.mniPassportExpiryReport.Text = Catalog.GetString("Passport Expiry Report");
      this.mniPersonalDocumentExpiryReport.Text = Catalog.GetString("Personal Document Expiry Report");
      this.mniStartOfCommitmentReport.Text = Catalog.GetString("Start Of Commitment Report");
      this.mniEndOfCommitmentReport.Text = Catalog.GetString("End Of Commitment Report");
      this.mniPreviousExperienceReport.Text = Catalog.GetString("Previous Experience Report");
      this.mniProgressReport.Text = Catalog.GetString("Progress Report");
      this.mniCampaignOptions.Text = Catalog.GetString("Campaign Options");
      this.mniUnitHierarchy.Text = Catalog.GetString("Unit Hierarchy");
      this.mniReports.Text = Catalog.GetString("&Reports...");
      this.mniPartner.Text = Catalog.GetString("Partner");
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
      this.Text = Catalog.GetString("Personnel Module OpenPetra.org");
      #endregion

      FPetraUtilsObject = new TFrmPetraModuleUtils(AParentFormHandle, this, stbMain);
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
        mniImport.Enabled = false;
        mniExport.Enabled = false;
        mniExtracts.Enabled = false;
        mniTodo.Enabled = false;
        mniHelpPetraHelp.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
    }

    /// auto generated
    protected void OpenScreenReportPartnerByCity(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmProgressReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmProgressReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenGeneralShortTermerReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmShortTermerReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmShortTermerReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenAbilitiesReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmAbilitiesReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmAbilitiesReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenPersonnelEmergencyContactReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmEmergencyContactInformationReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmEmergencyContactInformationReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenLanguagesReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmLanguagesReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmLanguagesReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenIndividualDataReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmPersonalDataReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmPersonalDataReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenEmergencyDataReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmEmergencyDataReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmEmergencyDataReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenLocalPersonnelDataReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmLocalPersonnelDataReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmLocalPersonnelDataReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenBirthdayList(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmBirthdayListReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmBirthdayListReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenEmergencyContactReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmEmergencyContactReportStaff frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmEmergencyContactReportStaff(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenPassportExpiryReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmPassportExpiryReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmPassportExpiryReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenPersonalDocumentExpiryReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmPersonalDocumentExpiryReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmPersonalDocumentExpiryReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenStartOfCommitmentReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmStartOfCommitmentReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmStartOfCommitmentReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenEndOfCommitmentReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmEndOfCommitmentReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmEndOfCommitmentReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenPreviousExperienceReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmPreviousExperienceReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmPreviousExperienceReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenProgressReport(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmProgressReport frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmProgressReport(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenCampaignOptions(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmCampaignOptions frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmCampaignOptions(this.Handle);
        frm.Show();
    }

    /// auto generated
    protected void OpenScreenUnitHierarchy(object sender, EventArgs e)
    {
        Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmUnitHierarchy frm = new Ict.Petra.Client.MReporting.Gui.MPersonnel.TFrmUnitHierarchy(this.Handle);
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
