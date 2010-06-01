// auto generated with nant generateWinforms from PartnerImport.yaml
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

  /// auto generated: Import Partners
  public partial class TFrmPartnerImport: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;

    /// constructor
    public TFrmPartnerImport(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblFilename.Text = Catalog.GetString("Filename:");
      this.btnSelectFile.Text = Catalog.GetString("Select File");
      this.lblCurrentRecordStatus.Text = Catalog.GetString("Current Import File Record Status:");
      this.lblExplanation.Text = Catalog.GetString("Explanation:");
      this.lblTakeAction.Text = Catalog.GetString("Take Action:");
      this.btnSkip.Text = Catalog.GetString("Skip Record");
      this.btnCreateNewFamilyAndPerson.Text = Catalog.GetString("Create new Family and Person");
      this.btnUseSelectedPerson.Text = Catalog.GetString("Use selected Person in list below");
      this.btnCreateNewPersonForSelectedFamily.Text = Catalog.GetString("Add as new Person to selected Family in list below");
      this.btnCreateNewFamily.Text = Catalog.GetString("Create new Family Record only");
      this.btnUseSelectedFamily.Text = Catalog.GetString("Use selected Family in list below");
      this.btnFindOtherPerson.Text = Catalog.GetString("Find other Person to use for this Record...");
      this.btnFindOtherFamily.Text = Catalog.GetString("Find other Family to add this Person to...");
      this.chkReplaceAddress.Text = Catalog.GetString("Replace current address in list below with imported one");
      this.tbbStartImport.Text = Catalog.GetString("Start Import");
      this.tbbCancelImport.Text = Catalog.GetString("Cancel Import");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Import Partners");
      #endregion

      this.txtFilename.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtCurrentRecordStatus.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtExplanation.Font = TAppSettingsManager.GetDefaultBoldFont();

      FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentFormHandle, this, stbMain);
      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
      tbbCancelImport.Enabled = false;

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
        if (e.ActionName == "actSkip")
        {
            btnSkip.Enabled = e.Enabled;
        }
        if (e.ActionName == "actCreateNewFamily")
        {
            btnCreateNewFamily.Enabled = e.Enabled;
        }
        if (e.ActionName == "actStartImport")
        {
            tbbStartImport.Enabled = e.Enabled;
        }
        if (e.ActionName == "actCancelImport")
        {
            tbbCancelImport.Enabled = e.Enabled;
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
