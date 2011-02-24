// auto generated with nant generateWinforms from UC_PartnerDetails_Unit.yaml and template usercontrol
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
  public partial class TUC_PartnerDetails_Unit: System.Windows.Forms.UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS FMainDS;

    /// constructor
    public TUC_PartnerDetails_Unit() : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblPreviousName.Text = Catalog.GetString("Previous Name:");
      this.lblLocalName.Text = Catalog.GetString("Local Name:");
      this.grpNames.Text = Catalog.GetString("Names");
      this.lblCountryCode.Text = Catalog.GetString("Country Code:");
      this.lblUnitTypeCode.Text = Catalog.GetString("Unit Type Code:");
      this.lblLanguageCode.Text = Catalog.GetString("Language Code:");
      this.lblAcquisitionCode.Text = Catalog.GetString("Acquisition Code:");
      this.grpMisc.Text = Catalog.GetString("Miscellaneous");
      this.lblXyzTbdCode.Text = Catalog.GetString("Xyz Tbd Code:");
      this.lblXyzTbdCost.Text = Catalog.GetString("Xyz Tbd Cost:");
      this.grpCampaignInfo.Text = Catalog.GetString("Campaign Information");
      #endregion

      this.txtPreviousName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtLocalName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtXyzTbdCode.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtXyzTbdCost.Font = TAppSettingsManager.GetDefaultBoldFont();
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
        FPetraUtilsObject.SetStatusBarText(txtPreviousName, Catalog.GetString("Enter the previously used Surname (eg before marriage)"));
        FPetraUtilsObject.SetStatusBarText(txtLocalName, Catalog.GetString("Enter a short name for this partner in your local language"));
        FPetraUtilsObject.SetStatusBarText(cmbCountryCode, Catalog.GetString("Country in which this Unit is found"));
        cmbCountryCode.InitialiseUserControl();
        FPetraUtilsObject.SetStatusBarText(cmbUnitTypeCode, Catalog.GetString("Enter the unit type"));
        cmbUnitTypeCode.InitialiseUserControl();
        FPetraUtilsObject.SetStatusBarText(cmbLanguageCode, Catalog.GetString("Select the partner's preferred language"));
        cmbLanguageCode.InitialiseUserControl();
        FPetraUtilsObject.SetStatusBarText(cmbAcquisitionCode, Catalog.GetString("Select a method-of-acquisition code"));
        cmbAcquisitionCode.InitialiseUserControl();
        FPetraUtilsObject.SetStatusBarText(txtXyzTbdCode, Catalog.GetString("todo"));
        FPetraUtilsObject.SetStatusBarText(txtXyzTbdCost, Catalog.GetString("Enter the cost of this xyz_tbd"));
        FPetraUtilsObject.SetStatusBarText(cmbXyzTbdCostCurrencyCode, Catalog.GetString("Currency used for the xyz_tbd cost"));
        cmbXyzTbdCostCurrencyCode.InitialiseUserControl();

        if(FMainDS != null)
        {
            ShowData(FMainDS.PUnit[0]);
        }
    }

    /// make sure that the primary key cannot be edited anymore
    public void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
    }

    private void ShowData(PUnitRow ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        if (FMainDS.PPartner == null || ((FMainDS.PPartner.Rows.Count > 0) && (FMainDS.PPartner[0].IsPreviousNameNull())))
        {
            txtPreviousName.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PPartner.Rows.Count > 0)
            {
                txtPreviousName.Text = FMainDS.PPartner[0].PreviousName;
            }
        }
        if (FMainDS.PPartner == null || ((FMainDS.PPartner.Rows.Count > 0) && (FMainDS.PPartner[0].IsPartnerShortNameLocNull())))
        {
            txtLocalName.Text = String.Empty;
        }
        else
        {
            if (FMainDS.PPartner.Rows.Count > 0)
            {
                txtLocalName.Text = FMainDS.PPartner[0].PartnerShortNameLoc;
            }
        }
        if (ARow.IsCountryCodeNull())
        {
            cmbCountryCode.SelectedIndex = -1;
        }
        else
        {
            cmbCountryCode.SetSelectedString(ARow.CountryCode);
        }
        if (ARow.IsUnitTypeCodeNull())
        {
            cmbUnitTypeCode.SelectedIndex = -1;
        }
        else
        {
            cmbUnitTypeCode.SetSelectedString(ARow.UnitTypeCode);
        }
        if (FMainDS.PPartner == null || ((FMainDS.PPartner.Rows.Count > 0) && (FMainDS.PPartner[0].IsLanguageCodeNull())))
        {
            cmbLanguageCode.SelectedIndex = -1;
        }
        else
        {
            if (FMainDS.PPartner.Rows.Count > 0)
            {
                cmbLanguageCode.SetSelectedString(FMainDS.PPartner[0].LanguageCode);
            }
        }
        if (FMainDS.PPartner == null || ((FMainDS.PPartner.Rows.Count > 0) && (FMainDS.PPartner[0].IsAcquisitionCodeNull())))
        {
            cmbAcquisitionCode.SelectedIndex = -1;
        }
        else
        {
            if (FMainDS.PPartner.Rows.Count > 0)
            {
                cmbAcquisitionCode.SetSelectedString(FMainDS.PPartner[0].AcquisitionCode);
            }
        }
        if (ARow.IsXyzTbdCodeNull())
        {
            txtXyzTbdCode.Text = String.Empty;
        }
        else
        {
            txtXyzTbdCode.Text = ARow.XyzTbdCode;
        }
        txtXyzTbdCost.Text = ARow.XyzTbdCost.ToString();
        if (ARow.IsXyzTbdCostCurrencyCodeNull())
        {
            cmbXyzTbdCostCurrencyCode.SelectedIndex = -1;
        }
        else
        {
            cmbXyzTbdCostCurrencyCode.SetSelectedString(ARow.XyzTbdCostCurrencyCode);
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private void GetDataFromControls(PUnitRow ARow)
    {
        if ((FMainDS.PPartner != null) && (FMainDS.PPartner.Rows.Count > 0))
        {
            if (txtPreviousName.Text.Length == 0)
            {
                FMainDS.PPartner[0].SetPreviousNameNull();
            }
            else
            {
                FMainDS.PPartner[0].PreviousName = txtPreviousName.Text;
            }
        }
        if ((FMainDS.PPartner != null) && (FMainDS.PPartner.Rows.Count > 0))
        {
            if (txtLocalName.Text.Length == 0)
            {
                FMainDS.PPartner[0].SetPartnerShortNameLocNull();
            }
            else
            {
                FMainDS.PPartner[0].PartnerShortNameLoc = txtLocalName.Text;
            }
        }
        if (cmbCountryCode.SelectedIndex == -1)
        {
            ARow.SetCountryCodeNull();
        }
        else
        {
            ARow.CountryCode = cmbCountryCode.GetSelectedString();
        }
        if (cmbUnitTypeCode.SelectedIndex == -1)
        {
            ARow.SetUnitTypeCodeNull();
        }
        else
        {
            ARow.UnitTypeCode = cmbUnitTypeCode.GetSelectedString();
        }
        if ((FMainDS.PPartner != null) && (FMainDS.PPartner.Rows.Count > 0))
        {
            if (cmbLanguageCode.SelectedIndex == -1)
            {
                FMainDS.PPartner[0].SetLanguageCodeNull();
            }
            else
            {
                FMainDS.PPartner[0].LanguageCode = cmbLanguageCode.GetSelectedString();
            }
        }
        if ((FMainDS.PPartner != null) && (FMainDS.PPartner.Rows.Count > 0))
        {
            if (cmbAcquisitionCode.SelectedIndex == -1)
            {
                FMainDS.PPartner[0].SetAcquisitionCodeNull();
            }
            else
            {
                FMainDS.PPartner[0].AcquisitionCode = cmbAcquisitionCode.GetSelectedString();
            }
        }
        if (txtXyzTbdCode.Text.Length == 0)
        {
            ARow.SetXyzTbdCodeNull();
        }
        else
        {
            ARow.XyzTbdCode = txtXyzTbdCode.Text;
        }
        ARow.XyzTbdCost = Convert.ToDecimal(txtXyzTbdCost.Text);
        if (cmbXyzTbdCostCurrencyCode.SelectedIndex == -1)
        {
            ARow.SetXyzTbdCostCurrencyCodeNull();
        }
        else
        {
            ARow.XyzTbdCostCurrencyCode = cmbXyzTbdCostCurrencyCode.GetSelectedString();
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
  }
}
