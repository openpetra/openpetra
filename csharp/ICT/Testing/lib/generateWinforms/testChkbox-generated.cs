// auto generated with nant generateWinforms from testChkbox.yaml and template window
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2013 by OM International
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
using System.Reflection;
using System.Data;
using SourceGrid;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;

namespace Ict.Testing
{

  /// auto generated: Test screen
  public partial class TFrmtestChkbox: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;

    /// constructor
    public TFrmtestChkbox(Form AParentForm) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblSonst.Text = Catalog.GetString("Sonst") + ":";
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&OpenPetra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniAbout.Text = Catalog.GetString("&About OpenPetra");
      this.mniDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Test screen");
      #endregion

      FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentForm, this, stbMain);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
      chkParentCheckedChanged(null, null);
      chkSonstCheckedChanged(null, null);
      chkChildCheckedChanged(null, null);

    }

    #region Show Method overrides

    /// <summary>
    /// Override of Form.Show(IWin32Window owner) Method. Caters for singleton Forms.
    /// </summary>
    /// <param name="owner">Any object that implements <see cref="IWin32Window" /> and represents the top-level window that will own this Form. </param>
    public new void Show(IWin32Window owner)
    {
        Form OpenScreen = TFormsList.GFormsList[this.GetType().FullName];
        bool OpenSelf = true;

        if ((OpenScreen != null)
            && (OpenScreen.Modal != true))
        {
            if (TFormsList.GSingletonForms.Contains(this.GetType().Name))
            {
//                MessageBox.Show("Activating singleton screen of Type '" + this.GetType().FullName + "'.");

                OpenSelf = false;
                this.Visible = false;   // needed as this.Close() would otherwise bring this Form to the foreground and OpenScreen.BringToFront() would not help...
                this.Close();

                OpenScreen.BringToFront();
            }
        }

        if (OpenSelf)
        {
            if (owner != null)
            {
                base.Show(owner);
            }
            else
            {
                base.Show();
            }
        }
    }

    /// <summary>
    /// Override of Form.Show() Method. Caters for singleton Forms.
    /// </summary>
    public new void Show()
    {
        this.Show(null);
    }

    #endregion

    void chkParentCheckedChanged(object sender, System.EventArgs e)
    {
      chkChild.Enabled = chkParent.Checked;
    }

    void chkSonstCheckedChanged(object sender, System.EventArgs e)
    {
    }

    void chkChildCheckedChanged(object sender, System.EventArgs e)
    {
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

    private void GetDataFromControls()
    {
    }

    /// <summary>
    /// Performs data validation.
    /// </summary>
    /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing
    /// to another record, otherwise set it to false.</param>
    /// <param name="AValidateSpecificControl">Pass in a Control to restrict Data Validation error checking to a
    /// specific Control for which Data Validation errors might have been recorded. (Default=this.ActiveControl).
    /// <para>
    /// This is useful for restricting Data Validation error checking to the current TabPage of a TabControl in order
    /// to only display Data Validation errors that pertain to the current TabPage. To do this, pass in a TabControl in
    /// this Argument.
    /// </para>
    /// </param>
    /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
    private bool ValidateAllData(bool ARecordChangeVerification, Control AValidateSpecificControl = null)
    {
        bool ReturnValue = false;
        Control ControlToValidate = null;

        // Record a new Data Validation Run. (All TVerificationResults/TScreenVerificationResults that are created during this 'run' are associated with this 'run' through that.)
        FPetraUtilsObject.VerificationResultCollection.RecordNewDataValidationRun();

        if (AValidateSpecificControl != null)
        {
            ControlToValidate = AValidateSpecificControl;
        }
        else
        {
            ControlToValidate = this.ActiveControl;
        }

        // Only process the Data Validations here if ControlToValidate is not null.
        // It can be null if this.ActiveControl yields null - this would happen if no Control
        // on this Form has got the Focus.
        if (ControlToValidate != null)
        {
            if(ControlToValidate.FindUserControlOrForm(true) == this)
            {
                ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(false, FPetraUtilsObject.VerificationResultCollection,
                    this.GetType(), ControlToValidate.FindUserControlOrForm(true).GetType());
            }
            else
            {
                ReturnValue = true;
            }
        }

        if(ReturnValue)
        {
            // Remove a possibly shown Validation ToolTip as the data validation succeeded
            FPetraUtilsObject.ValidationToolTip.RemoveAll();
        }

        return ReturnValue;
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
        if (e.ActionName == "actAbout")
        {
            mniAbout.Enabled = e.Enabled;
        }
        mniHelpPetraHelp.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniDevelopmentTeam.Enabled = false;
    }

    /// auto generated
    protected void actClose(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eClose);
    }

    /// auto generated
    protected void actAbout(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eHelpAbout);
    }

#endregion

#region Data Validation

    private void ControlValidatedHandler(object sender, EventArgs e)
    {
        TScreenVerificationResult SingleVerificationResult;

        ValidateAllData(true, (Control)sender);

        FPetraUtilsObject.ValidationToolTip.RemoveAll();

        if (FPetraUtilsObject.VerificationResultCollection.Count > 0)
        {
            for (int Counter = 0; Counter < FPetraUtilsObject.VerificationResultCollection.Count; Counter++)
            {
                SingleVerificationResult = (TScreenVerificationResult)FPetraUtilsObject.VerificationResultCollection[Counter];

                if (SingleVerificationResult.ResultControl == sender)
                {
                    if (FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested)
                    {
                        SingleVerificationResult.ResultControl.Focus();
                        FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = false;
                    }

                    if (!SingleVerificationResult.SuppressValidationToolTip)
                    {
                        FPetraUtilsObject.ValidationToolTipSeverity = SingleVerificationResult.ResultSeverity;

                        if (SingleVerificationResult.ResultTextCaption != String.Empty)
                        {
                            FPetraUtilsObject.ValidationToolTip.ToolTipTitle += ":  " + SingleVerificationResult.ResultTextCaption;
                        }

                        FPetraUtilsObject.ValidationToolTip.Show(SingleVerificationResult.ResultText, (Control)sender,
                            ((Control)sender).Width / 2, ((Control)sender).Height);
                    }
                }
            }
        }
    }

#endregion

  }
}