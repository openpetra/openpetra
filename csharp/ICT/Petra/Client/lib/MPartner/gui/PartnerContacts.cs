// auto generated with nant generateWinforms from PartnerContacts.yaml and template windowFind
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
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Gui
{

  /// auto generated: Contacts with Partners
  public partial class TFrmPartnerContacts: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;
    private Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS FMainDS;

    /// constructor
    public TFrmPartnerContacts(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblContactDate.Text = Catalog.GetString("Contact Date:");
      this.lblContactor.Text = Catalog.GetString("Contactor:");
      this.lblCommentContains.Text = Catalog.GetString("Comment Contains:");
      this.lblModule.Text = Catalog.GetString("Module:");
      this.lblMethodOfContact.Text = Catalog.GetString("Method Of Contact:");
      this.lblMailingCode.Text = Catalog.GetString("Mailing Code:");
      this.tbbSearch.Text = Catalog.GetString("Search");
      this.tbbDeleteContacts.Text = Catalog.GetString("Delete Contacts");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Contacts with Partners");
      #endregion

      this.txtContactor.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtCommentContains.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtModule.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtMethodOfContact.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtMailingCode.Font = TAppSettingsManager.GetDefaultBoldFont();

      FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentFormHandle, this, stbMain);
      FMainDS = new Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS();
      FPetraUtilsObject.SetStatusBarText(dtpContactDate, Catalog.GetString("Enter the date the contact was made"));
      FPetraUtilsObject.SetStatusBarText(txtContactor, Catalog.GetString("Enter the User ID"));
      FPetraUtilsObject.SetStatusBarText(txtMailingCode, Catalog.GetString("Enter the mailing code associated with the contact"));
      InitializeManualCode();
      grdDetails.Columns.Clear();
      grdDetails.AddTextColumn("Partner Key", FMainDS.PPartnerContact.ColumnPartnerKey);
      grdDetails.AddDateColumn("Contact Date", FMainDS.PPartnerContact.ColumnContactDate);
      grdDetails.AddTextColumn("Contact Code", FMainDS.PPartnerContact.ColumnContactCode);
      grdDetails.AddTextColumn("User ID", FMainDS.PPartnerContact.ColumnContactor);
      grdDetails.AddTextColumn("Description", FMainDS.PPartnerContact.ColumnContactComment);
      grdDetails.AddTextColumn("Module ID", FMainDS.PPartnerContact.ColumnModuleId);
      grdDetails.AddTextColumn("Mailing Code", FMainDS.PPartnerContact.ColumnMailingCode);
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

    private void ShowData(PPartnerContactRow ARow)
    {
        dtpContactDate.Date = ARow.ContactDate;
        if (ARow.IsContactorNull())
        {
            txtContactor.Text = String.Empty;
        }
        else
        {
            txtContactor.Text = ARow.Contactor;
        }
        if (ARow.IsMailingCodeNull())
        {
            txtMailingCode.Text = String.Empty;
        }
        else
        {
            txtMailingCode.Text = ARow.MailingCode;
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
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actSearch")
        {
            tbbSearch.Enabled = e.Enabled;
        }
        if (e.ActionName == "actDeleteContacts")
        {
            tbbDeleteContacts.Enabled = e.Enabled;
        }
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
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
