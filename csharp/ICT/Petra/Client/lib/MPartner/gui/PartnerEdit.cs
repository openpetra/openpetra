/* auto generated with nant generateWinforms from PartnerEdit.yaml
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

namespace Ict.Petra.Client.MPartner
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

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this);
      ucoUpperPart.PetraUtilsObject = FPetraUtilsObject;
      ucoPartnerTabSet.PetraUtilsObject = FPetraUtilsObject;
      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
    }

    private void tbbSaveClick(object sender, EventArgs e)
    {
        actSave(sender, e);
    }

    private void mniFileSaveClick(object sender, EventArgs e)
    {
        actSave(sender, e);
    }

    private void mniCloseClick(object sender, EventArgs e)
    {
        actClose(sender, e);
    }

    private void TFrmPetra_Activated(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Activated(sender, e);
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
    public TFrmPetraEditUtils GetUtilObject()
    {
        return (TFrmPetraEditUtils)FPetraUtilsObject;
    }
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actSave")
        {
            tbbSave.Enabled = e.Enabled;
        }
        mniFileNew.Enabled = false;
        mniFileDeactivatePartner.Enabled = false;
        mniFileDeletePartner.Enabled = false;
        mniFileCopyPartnerKey.Enabled = false;
        mniFileCopyAddress.Enabled = false;
        mniFileSendEmail.Enabled = false;
        mniSeparator0.Enabled = false;
        mniFilePrintSection.Enabled = false;
        mniFileExportPartner.Enabled = false;
        if (e.ActionName == "actSave")
        {
            mniFileSave.Enabled = e.Enabled;
        }
        mniSeparator1.Enabled = false;
        mniFilePrint.Enabled = false;
        mniSeparator2.Enabled = false;
        mniSeparator3.Enabled = false;
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        mniEditFindNewAddress.Enabled = false;
        mniEditUndoCurrentField.Enabled = false;
        mniEditUndoScreen.Enabled = false;
        mniSeparator7.Enabled = false;
        mniEditFind.Enabled = false;
        mniViewPartnerData.Enabled = false;
        mniViewPersonnelData.Enabled = false;
        mniViewFinanceData.Enabled = false;
        mniSeparator4.Enabled = false;
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
        mniMaintainOMerField.Enabled = false;
        mniSeparator5.Enabled = false;
        mniMaintainPersonnelIndividualData.Enabled = false;
        mniSeparator6.Enabled = false;
        mniMaintainDonorHistory.Enabled = false;
        mniMaintainRecipientHistory.Enabled = false;
        mniMaintainFinanceReports.Enabled = false;
        mniMaintainBankAccounts.Enabled = false;
        mniMaintainGiftReceipting.Enabled = false;
        mniMaintainFinanceDetails.Enabled = false;
        mniHelpPetraHelp.Enabled = false;
        mniSeparator8.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniSeparator9.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
    }

    /// auto generated
    protected void actSave(object sender, EventArgs e)
    {
        FileSave(sender, e);
    }

    /// auto generated
    protected void actClose(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eClose);
    }

#endregion
  }
}
