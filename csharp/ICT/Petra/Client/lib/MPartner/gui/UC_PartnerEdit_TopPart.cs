// auto generated with nant generateWinforms from UC_PartnerEdit_TopPart.yaml and template usercontrol
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2011 by OM International
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
      this.lblPartnerClass.Text = Catalog.GetString("Class:");
      this.lblFamilyTitle.Text = Catalog.GetString("Title/Na&me:");
      this.lblFamilyAddresseeTypeCode.Text = Catalog.GetString("&Addressee Type:");
      this.chkFamilyNoSolicitations.Text = Catalog.GetString("No Solicitations");
      this.lblPersonTitle.Text = Catalog.GetString("Title/Na&me:");
      this.lblPersonGender.Text = Catalog.GetString("&Gender:");
      this.lblPersonAddresseeTypeCode.Text = Catalog.GetString("&Addressee Type:");
      this.chkPersonNoSolicitations.Text = Catalog.GetString("No Solicitations");
      this.lblChurchName.Text = Catalog.GetString("Na&me:");
      this.lblOrganisationName.Text = Catalog.GetString("Na&me:");
      this.lblUnitName.Text = Catalog.GetString("Na&me:");
      this.lblBranchName.Text = Catalog.GetString("Na&me:");
      this.lblVenueName.Text = Catalog.GetString("Na&me:");
      this.lblOtherAddresseeTypeCode.Text = Catalog.GetString("&Addressee Type:");
      this.chkOtherNoSolicitations.Text = Catalog.GetString("No Solicitations");
      this.lblLastGiftDetailsDate.Text = Catalog.GetString("Last Gift:");
      this.btnWorkerField.Text = Catalog.GetString("&Worker Field...");
      this.lblPartnerStatus.Text = Catalog.GetString("Partner &Status:");
      this.lblStatusUpdated.Text = Catalog.GetString("Status Updated:");
      this.lblLastContact.Text = Catalog.GetString("Last Contact:");
      this.grpCollapsible.Text = Catalog.GetString("Key Partner Data");
      #endregion

      this.txtPartnerClass.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtFamilyTitle.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtFamilyFirstName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtFamilyFamilyName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtPersonTitle.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtPersonFirstName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtPersonMiddleName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtPersonFamilyName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtChurchName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtOrganisationName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtUnitName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtBranchName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtVenueName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtLastGiftDetailsDate.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtLastGiftDetails.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtWorkerField.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtStatusUpdated.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtLastContact.Font = TAppSettingsManager.GetDefaultBoldFont();
    }

    /// helper object for the whole screen
    public TFrmPetraEditUtils PetraUtilsObject
    {
        get
        {
            return FPetraUtilsObject;
        }

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
        FPetraUtilsObject.SetStatusBarText(txtFamilyFirstName, Catalog.GetString("Enter the person's full first name"));
        FPetraUtilsObject.SetStatusBarText(txtFamilyFamilyName, Catalog.GetString("Enter a Last Name/Surname/Family Name"));
        FPetraUtilsObject.SetStatusBarText(cmbFamilyAddresseeTypeCode, Catalog.GetString("Enter an addressee type code"));
        cmbFamilyAddresseeTypeCode.InitialiseUserControl();
        FPetraUtilsObject.SetStatusBarText(chkFamilyNoSolicitations, Catalog.GetString("Set this if the partner does not want extra mailings"));
        FPetraUtilsObject.SetStatusBarText(txtPersonTitle, Catalog.GetString("e.g. Herr, Mr(s)., Frau, Miss., Senor(ita), M., Mme., Dr., et"));
        FPetraUtilsObject.SetStatusBarText(txtPersonFirstName, Catalog.GetString("Enter the person's full first name"));
        FPetraUtilsObject.SetStatusBarText(txtPersonMiddleName, Catalog.GetString("Enter the person's full middle name or initial"));
        FPetraUtilsObject.SetStatusBarText(txtPersonFamilyName, Catalog.GetString("Enter a Last Name/Surname/Family Name"));
        FPetraUtilsObject.SetStatusBarText(cmbPersonGender, Catalog.GetString("Select the gender of the Person"));
        cmbPersonGender.InitialiseUserControl();
        FPetraUtilsObject.SetStatusBarText(cmbPersonAddresseeTypeCode, Catalog.GetString("Enter an addressee type code"));
        cmbPersonAddresseeTypeCode.InitialiseUserControl();
        FPetraUtilsObject.SetStatusBarText(chkPersonNoSolicitations, Catalog.GetString("Set this if the partner does not want extra mailings"));
        FPetraUtilsObject.SetStatusBarText(txtChurchName, Catalog.GetString("Enter the name of the church"));
        FPetraUtilsObject.SetStatusBarText(txtOrganisationName, Catalog.GetString("Enter the name of the organisation"));
        FPetraUtilsObject.SetStatusBarText(txtUnitName, Catalog.GetString("Enter the name of the unit"));
        FPetraUtilsObject.SetStatusBarText(txtBranchName, Catalog.GetString("Enter the name of the bank"));
        FPetraUtilsObject.SetStatusBarText(txtVenueName, Catalog.GetString("Enter the name of the venue"));
        FPetraUtilsObject.SetStatusBarText(cmbOtherAddresseeTypeCode, Catalog.GetString("Enter an addressee type code"));
        cmbOtherAddresseeTypeCode.InitialiseUserControl();
        FPetraUtilsObject.SetStatusBarText(chkOtherNoSolicitations, Catalog.GetString("Set this if the partner does not want extra mailings"));
        FPetraUtilsObject.SetStatusBarText(cmbPartnerStatus, Catalog.GetString("Select a partner status"));
        cmbPartnerStatus.InitialiseUserControl();
        FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

        if(FMainDS != null)
        {
            ShowData(FMainDS.PPartner[0]);
        }
    }

    /// make sure that the primary key cannot be edited anymore
    public void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
        txtPartnerKey.ReadOnly = AReadOnly;
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
        if (FMainDS.PFamily == null || ((FMainDS.PFamily.Rows.Count > 0) && (FMainDS.PFamily[0].IsTitleNull())))
        {
            txtFamilyTitle.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PFamily.Rows.Count > 0)
            {
                txtFamilyTitle.Text = FMainDS.PFamily[0].Title;
            }
        }
        if (FMainDS.PFamily == null || ((FMainDS.PFamily.Rows.Count > 0) && (FMainDS.PFamily[0].IsFirstNameNull())))
        {
            txtFamilyFirstName.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PFamily.Rows.Count > 0)
            {
                txtFamilyFirstName.Text = FMainDS.PFamily[0].FirstName;
            }
        }
        if (FMainDS.PFamily == null || ((FMainDS.PFamily.Rows.Count > 0) && (FMainDS.PFamily[0].IsFamilyNameNull())))
        {
            txtFamilyFamilyName.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PFamily.Rows.Count > 0)
            {
                txtFamilyFamilyName.Text = FMainDS.PFamily[0].FamilyName;
            }
        }
        if (ARow.IsAddresseeTypeCodeNull())
        {
            cmbFamilyAddresseeTypeCode.SelectedIndex = -1;
        }
        else
        {
            cmbFamilyAddresseeTypeCode.SetSelectedString(ARow.AddresseeTypeCode);
        }
        if (ARow.IsNoSolicitationsNull())
        {
            chkFamilyNoSolicitations.Checked = false;
        }
        else
        {
            chkFamilyNoSolicitations.Checked = ARow.NoSolicitations;
        }
        if (FMainDS.PPerson == null || ((FMainDS.PPerson.Rows.Count > 0) && (FMainDS.PPerson[0].IsTitleNull())))
        {
            txtPersonTitle.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PPerson.Rows.Count > 0)
            {
                txtPersonTitle.Text = FMainDS.PPerson[0].Title;
            }
        }
        if (FMainDS.PPerson == null || ((FMainDS.PPerson.Rows.Count > 0) && (FMainDS.PPerson[0].IsFirstNameNull())))
        {
            txtPersonFirstName.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PPerson.Rows.Count > 0)
            {
                txtPersonFirstName.Text = FMainDS.PPerson[0].FirstName;
            }
        }
        if (FMainDS.PPerson == null || ((FMainDS.PPerson.Rows.Count > 0) && (FMainDS.PPerson[0].IsMiddleName1Null())))
        {
            txtPersonMiddleName.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PPerson.Rows.Count > 0)
            {
                txtPersonMiddleName.Text = FMainDS.PPerson[0].MiddleName1;
            }
        }
        if (FMainDS.PPerson == null || ((FMainDS.PPerson.Rows.Count > 0) && (FMainDS.PPerson[0].IsFamilyNameNull())))
        {
            txtPersonFamilyName.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PPerson.Rows.Count > 0)
            {
                txtPersonFamilyName.Text = FMainDS.PPerson[0].FamilyName;
            }
        }
        if (FMainDS.PPerson == null || ((FMainDS.PPerson.Rows.Count > 0) && (FMainDS.PPerson[0].IsGenderNull())))
        {
            cmbPersonGender.SelectedIndex = -1;
        }
        else
        {
            if (FMainDS.PPerson.Rows.Count > 0)
            {
                cmbPersonGender.SetSelectedString(FMainDS.PPerson[0].Gender);
            }
        }
        if (ARow.IsAddresseeTypeCodeNull())
        {
            cmbPersonAddresseeTypeCode.SelectedIndex = -1;
        }
        else
        {
            cmbPersonAddresseeTypeCode.SetSelectedString(ARow.AddresseeTypeCode);
        }
        if (ARow.IsNoSolicitationsNull())
        {
            chkPersonNoSolicitations.Checked = false;
        }
        else
        {
            chkPersonNoSolicitations.Checked = ARow.NoSolicitations;
        }
        if (FMainDS.PChurch == null || ((FMainDS.PChurch.Rows.Count > 0) && (FMainDS.PChurch[0].IsChurchNameNull())))
        {
            txtChurchName.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PChurch.Rows.Count > 0)
            {
                txtChurchName.Text = FMainDS.PChurch[0].ChurchName;
            }
        }
        if (FMainDS.POrganisation == null || ((FMainDS.POrganisation.Rows.Count > 0) && (FMainDS.POrganisation[0].IsOrganisationNameNull())))
        {
            txtOrganisationName.Text = String.Empty;
        }
        else
        {
            if (FMainDS.POrganisation.Rows.Count > 0)
            {
                txtOrganisationName.Text = FMainDS.POrganisation[0].OrganisationName;
            }
        }
        if (FMainDS.PUnit == null || ((FMainDS.PUnit.Rows.Count > 0) && (FMainDS.PUnit[0].IsUnitNameNull())))
        {
            txtUnitName.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PUnit.Rows.Count > 0)
            {
                txtUnitName.Text = FMainDS.PUnit[0].UnitName;
            }
        }
        if (FMainDS.PBank == null || ((FMainDS.PBank.Rows.Count > 0) && (FMainDS.PBank[0].IsBranchNameNull())))
        {
            txtBranchName.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PBank.Rows.Count > 0)
            {
                txtBranchName.Text = FMainDS.PBank[0].BranchName;
            }
        }
        if (FMainDS.PVenue == null || ((FMainDS.PVenue.Rows.Count > 0) && (FMainDS.PVenue[0].IsVenueNameNull())))
        {
            txtVenueName.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PVenue.Rows.Count > 0)
            {
                txtVenueName.Text = FMainDS.PVenue[0].VenueName;
            }
        }
        if (ARow.IsAddresseeTypeCodeNull())
        {
            cmbOtherAddresseeTypeCode.SelectedIndex = -1;
        }
        else
        {
            cmbOtherAddresseeTypeCode.SetSelectedString(ARow.AddresseeTypeCode);
        }
        if (ARow.IsNoSolicitationsNull())
        {
            chkOtherNoSolicitations.Checked = false;
        }
        else
        {
            chkOtherNoSolicitations.Checked = ARow.NoSolicitations;
        }
        if (FMainDS.MiscellaneousData == null || ((FMainDS.MiscellaneousData.Rows.Count > 0) && (FMainDS.MiscellaneousData[0].IsLastGiftDateNull())))
        {
            txtLastGiftDetailsDate.Text = String.Empty;
        }
        else
        {
            if (FMainDS.MiscellaneousData.Rows.Count > 0)
            {
                txtLastGiftDetailsDate.Text = FMainDS.MiscellaneousData[0].LastGiftDate.ToString();
            }
        }
        if (FMainDS.MiscellaneousData == null || ((FMainDS.MiscellaneousData.Rows.Count > 0) && (FMainDS.MiscellaneousData[0].IsLastGiftInfoNull())))
        {
            txtLastGiftDetails.Text = String.Empty;
        }
        else
        {
            if (FMainDS.MiscellaneousData.Rows.Count > 0)
            {
                txtLastGiftDetails.Text = FMainDS.MiscellaneousData[0].LastGiftInfo;
            }
        }
        if (ARow.IsStatusCodeNull())
        {
            cmbPartnerStatus.SelectedIndex = -1;
        }
        else
        {
            cmbPartnerStatus.SetSelectedString(ARow.StatusCode);
        }
        if (FMainDS.MiscellaneousData == null || ((FMainDS.MiscellaneousData.Rows.Count > 0) && (FMainDS.MiscellaneousData[0].IsLastContactDateNull())))
        {
            txtLastContact.Text = String.Empty;
        }
        else
        {
            if (FMainDS.MiscellaneousData.Rows.Count > 0)
            {
                txtLastContact.Text = FMainDS.MiscellaneousData[0].LastContactDate.ToString();
            }
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private void GetDataFromControls(PPartnerRow ARow)
    {
        if ((FMainDS.PFamily != null) && (FMainDS.PFamily.Rows.Count > 0))
        {
            if (txtFamilyTitle.Text.Length == 0)
            {
                FMainDS.PFamily[0].SetTitleNull();
            }
            else
            {
                FMainDS.PFamily[0].Title = txtFamilyTitle.Text;
            }
        }
        if ((FMainDS.PFamily != null) && (FMainDS.PFamily.Rows.Count > 0))
        {
            if (txtFamilyFirstName.Text.Length == 0)
            {
                FMainDS.PFamily[0].SetFirstNameNull();
            }
            else
            {
                FMainDS.PFamily[0].FirstName = txtFamilyFirstName.Text;
            }
        }
        if ((FMainDS.PFamily != null) && (FMainDS.PFamily.Rows.Count > 0))
        {
            if (txtFamilyFamilyName.Text.Length == 0)
            {
                FMainDS.PFamily[0].SetFamilyNameNull();
            }
            else
            {
                FMainDS.PFamily[0].FamilyName = txtFamilyFamilyName.Text;
            }
        }
        if (cmbFamilyAddresseeTypeCode.SelectedIndex == -1)
        {
            ARow.SetAddresseeTypeCodeNull();
        }
        else
        {
            ARow.AddresseeTypeCode = cmbFamilyAddresseeTypeCode.GetSelectedString();
        }
        ARow.NoSolicitations = chkFamilyNoSolicitations.Checked;
        if ((FMainDS.PPerson != null) && (FMainDS.PPerson.Rows.Count > 0))
        {
            if (txtPersonTitle.Text.Length == 0)
            {
                FMainDS.PPerson[0].SetTitleNull();
            }
            else
            {
                FMainDS.PPerson[0].Title = txtPersonTitle.Text;
            }
        }
        if ((FMainDS.PPerson != null) && (FMainDS.PPerson.Rows.Count > 0))
        {
            if (txtPersonFirstName.Text.Length == 0)
            {
                FMainDS.PPerson[0].SetFirstNameNull();
            }
            else
            {
                FMainDS.PPerson[0].FirstName = txtPersonFirstName.Text;
            }
        }
        if ((FMainDS.PPerson != null) && (FMainDS.PPerson.Rows.Count > 0))
        {
            if (txtPersonMiddleName.Text.Length == 0)
            {
                FMainDS.PPerson[0].SetMiddleName1Null();
            }
            else
            {
                FMainDS.PPerson[0].MiddleName1 = txtPersonMiddleName.Text;
            }
        }
        if ((FMainDS.PPerson != null) && (FMainDS.PPerson.Rows.Count > 0))
        {
            if (txtPersonFamilyName.Text.Length == 0)
            {
                FMainDS.PPerson[0].SetFamilyNameNull();
            }
            else
            {
                FMainDS.PPerson[0].FamilyName = txtPersonFamilyName.Text;
            }
        }
        if ((FMainDS.PPerson != null) && (FMainDS.PPerson.Rows.Count > 0))
        {
            if (cmbPersonGender.SelectedIndex == -1)
            {
                FMainDS.PPerson[0].SetGenderNull();
            }
            else
            {
                FMainDS.PPerson[0].Gender = cmbPersonGender.GetSelectedString();
            }
        }
        if (cmbPersonAddresseeTypeCode.SelectedIndex == -1)
        {
            ARow.SetAddresseeTypeCodeNull();
        }
        else
        {
            ARow.AddresseeTypeCode = cmbPersonAddresseeTypeCode.GetSelectedString();
        }
        ARow.NoSolicitations = chkPersonNoSolicitations.Checked;
        if ((FMainDS.PChurch != null) && (FMainDS.PChurch.Rows.Count > 0))
        {
            if (txtChurchName.Text.Length == 0)
            {
                FMainDS.PChurch[0].SetChurchNameNull();
            }
            else
            {
                FMainDS.PChurch[0].ChurchName = txtChurchName.Text;
            }
        }
        if ((FMainDS.POrganisation != null) && (FMainDS.POrganisation.Rows.Count > 0))
        {
            if (txtOrganisationName.Text.Length == 0)
            {
                FMainDS.POrganisation[0].SetOrganisationNameNull();
            }
            else
            {
                FMainDS.POrganisation[0].OrganisationName = txtOrganisationName.Text;
            }
        }
        if ((FMainDS.PUnit != null) && (FMainDS.PUnit.Rows.Count > 0))
        {
            if (txtUnitName.Text.Length == 0)
            {
                FMainDS.PUnit[0].SetUnitNameNull();
            }
            else
            {
                FMainDS.PUnit[0].UnitName = txtUnitName.Text;
            }
        }
        if ((FMainDS.PBank != null) && (FMainDS.PBank.Rows.Count > 0))
        {
            if (txtBranchName.Text.Length == 0)
            {
                FMainDS.PBank[0].SetBranchNameNull();
            }
            else
            {
                FMainDS.PBank[0].BranchName = txtBranchName.Text;
            }
        }
        if ((FMainDS.PVenue != null) && (FMainDS.PVenue.Rows.Count > 0))
        {
            if (txtVenueName.Text.Length == 0)
            {
                FMainDS.PVenue[0].SetVenueNameNull();
            }
            else
            {
                FMainDS.PVenue[0].VenueName = txtVenueName.Text;
            }
        }
        if (cmbOtherAddresseeTypeCode.SelectedIndex == -1)
        {
            ARow.SetAddresseeTypeCodeNull();
        }
        else
        {
            ARow.AddresseeTypeCode = cmbOtherAddresseeTypeCode.GetSelectedString();
        }
        ARow.NoSolicitations = chkOtherNoSolicitations.Checked;
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
