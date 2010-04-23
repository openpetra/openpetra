/* auto generated with nant generateWinforms from UC_PartnerEdit_TopPart.yaml and template controlMaintainTable
 *
 * DO NOT edit manually, DO NOT edit with the designer
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
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui
{

  /// auto generated user control
  public partial class TUC_PartnerEdit_TopPart: TGrpCollapsible, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS FMainDS;

    /// constructor
    public TUC_PartnerEdit_TopPart() : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblPartnerKey.Text = Catalog.GetString("Key:");
      this.lblEmpty2.Text = Catalog.GetString("Empty2:");
      this.lblPartnerClass.Text = Catalog.GetString("Class:");
      this.lblFamilyTitle.Text = Catalog.GetString("Title/Na&me:");
      this.lblEmpty.Text = Catalog.GetString("Empty:");
      this.lblAddresseeTypeCode.Text = Catalog.GetString("&Addressee Type:");
      this.chkNoSolicitations.Text = Catalog.GetString("No Solicitations");
      this.lblLastGift.Text = Catalog.GetString("Last Gift:");
      this.btnWorkerField.Text = Catalog.GetString("&Worker Field...");
      this.lblPartnerStatus.Text = Catalog.GetString("Partner &Status:");
      this.lblStatusUpdated.Text = Catalog.GetString("Status Updated:");
      this.lblLastContact.Text = Catalog.GetString("Last Contact:");
      this.grpCollapsible.Text = Catalog.GetString("Key Partner Data");
      #endregion

    }

    /// helper object for the whole screen
    public TFrmPetraEditUtils PetraUtilsObject
    {
        set
        {
            FPetraUtilsObject = value;
        }
    }

    /// dataset for the whole screen
    public Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS MainDS
    {
        set
        {
            FMainDS = value;
        }
    }

    /// <summary>todoComment</summary>
    public event System.EventHandler DataLoadingStarted;

    /// <summary>todoComment</summary>
    public event System.EventHandler DataLoadingFinished;

    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
        FPetraUtilsObject.SetStatusBarText(txtPartnerKey, Catalog.GetString("Enter the partner key (SiteID + Number)"));
        FPetraUtilsObject.SetStatusBarText(txtPartnerClass, Catalog.GetString("Select a partner class"));
        FPetraUtilsObject.SetStatusBarText(txtFamilyTitle, Catalog.GetString("e.g. Family, Mr & Mrs, Herr und Frau"));
        FPetraUtilsObject.SetStatusBarText(txtFirstName, Catalog.GetString("Enter the person's full first name"));
        FPetraUtilsObject.SetStatusBarText(txtFamilyName, Catalog.GetString("Enter a Last Name/Surname/Family Name"));
        FPetraUtilsObject.SetStatusBarText(cmbAddresseeTypeCode, Catalog.GetString("Enter an addressee type code"));
        cmbAddresseeTypeCode.InitialiseUserControl();
        FPetraUtilsObject.SetStatusBarText(chkNoSolicitations, Catalog.GetString("Set this if the partner does not want extra mailings"));
        FPetraUtilsObject.SetStatusBarText(cmbPartnerStatus, Catalog.GetString("Select a partner status"));
        cmbPartnerStatus.InitialiseUserControl();
        FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

        if(FMainDS != null)
        {
            ShowData(FMainDS.PPartner[0]);
        }
    }

    private void ShowData(PPartnerRow ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        txtPartnerKey.Text = String.Format("{0:0000000000}", ARow.PartnerKey);
        txtPartnerKey.ReadOnly = (ARow.RowState != DataRowState.Added);
        if (ARow.IsPartnerClassNull())
        {
            txtPartnerClass.Text = String.Empty;
        }
        else
        {
            txtPartnerClass.Text = ARow.PartnerClass;
        }
        if (FMainDS.PFamily[0].IsTitleNull())
        {
            txtFamilyTitle.Text = String.Empty;
        }
        else
        {
            txtFamilyTitle.Text = FMainDS.PFamily[0].Title;
        }
        if (FMainDS.PFamily[0].IsFirstNameNull())
        {
            txtFirstName.Text = String.Empty;
        }
        else
        {
            txtFirstName.Text = FMainDS.PFamily[0].FirstName;
        }
        if (FMainDS.PFamily[0].IsFamilyNameNull())
        {
            txtFamilyName.Text = String.Empty;
        }
        else
        {
            txtFamilyName.Text = FMainDS.PFamily[0].FamilyName;
        }
        if (ARow.IsAddresseeTypeCodeNull())
        {
            cmbAddresseeTypeCode.SelectedIndex = -1;
        }
        else
        {
            cmbAddresseeTypeCode.SetSelectedString(ARow.AddresseeTypeCode);
        }
        if (ARow.IsNoSolicitationsNull())
        {
            chkNoSolicitations.Checked = false;
        }
        else
        {
            chkNoSolicitations.Checked = ARow.NoSolicitations;
        }
        if (FMainDS.MiscellaneousData[0].IsLastGiftInfoNull())
        {
            txtLastGift.Text = String.Empty;
        }
        else
        {
            txtLastGift.Text = FMainDS.MiscellaneousData[0].LastGiftInfo;
        }
        if (ARow.IsStatusCodeNull())
        {
            cmbPartnerStatus.SelectedIndex = -1;
        }
        else
        {
            cmbPartnerStatus.SetSelectedString(ARow.StatusCode);
        }
        if (FMainDS.MiscellaneousData[0].IsLastContactDateNull())
        {
            txtLastContact.Text = String.Empty;
        }
        else
        {
            txtLastContact.Text = FMainDS.MiscellaneousData[0].LastContactDate.ToString();
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private void GetDataFromControls(PPartnerRow ARow)
    {
        if (txtFamilyTitle.Text.Length == 0)
        {
            FMainDS.PFamily[0].SetTitleNull();
        }
        else
        {
            FMainDS.PFamily[0].Title = txtFamilyTitle.Text;
        }
        if (txtFirstName.Text.Length == 0)
        {
            FMainDS.PFamily[0].SetFirstNameNull();
        }
        else
        {
            FMainDS.PFamily[0].FirstName = txtFirstName.Text;
        }
        if (txtFamilyName.Text.Length == 0)
        {
            FMainDS.PFamily[0].SetFamilyNameNull();
        }
        else
        {
            FMainDS.PFamily[0].FamilyName = txtFamilyName.Text;
        }
        if (cmbAddresseeTypeCode.SelectedIndex == -1)
        {
            ARow.SetAddresseeTypeCodeNull();
        }
        else
        {
            ARow.AddresseeTypeCode = cmbAddresseeTypeCode.GetSelectedString();
        }
        ARow.NoSolicitations = chkNoSolicitations.Checked;
        if (cmbPartnerStatus.SelectedIndex == -1)
        {
            ARow.SetStatusCodeNull();
        }
        else
        {
            ARow.StatusCode = cmbPartnerStatus.GetSelectedString();
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
        if (e.ActionName == "actMaintainWorkerField")
        {
            btnWorkerField.Enabled = e.Enabled;
        }
    }

#endregion
  }
}
