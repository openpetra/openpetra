// auto generated with nant generateWinforms from UC_PartnerDetails_Church.yaml and template usercontrol
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
  public partial class TUC_PartnerDetails_Church: System.Windows.Forms.UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS FMainDS;

    /// constructor
    public TUC_PartnerDetails_Church() : base()
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
      this.lblDenominationCode.Text = Catalog.GetString("Denomination Code:");
      this.lblApproximateSize.Text = Catalog.GetString("Approximate Size:");
      this.chkMapOnFile.Text = Catalog.GetString("Map On File");
      this.chkPrayerGroup.Text = Catalog.GetString("Prayer Group");
      this.lblLanguageCode.Text = Catalog.GetString("Language Code:");
      this.lblAcquisitionCode.Text = Catalog.GetString("Acquisition Code:");
      this.txtContactPartnerKey.ButtonText = Catalog.GetString("Find");
      this.lblContactPartnerKey.Text = Catalog.GetString("Contact Partner:");
      this.lblAccomodation.Text = Catalog.GetString("Accomodation:");
      this.lblAccomodationSize.Text = Catalog.GetString("Accomodation Size:");
      this.lblAccomodationType.Text = Catalog.GetString("Accomodation Type:");
      this.grpMisc.Text = Catalog.GetString("Miscellaneous");
      #endregion

      this.txtPreviousName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtLocalName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtApproximateSize.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtAccomodationSize.Font = TAppSettingsManager.GetDefaultBoldFont();
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
        FPetraUtilsObject.SetStatusBarText(cmbDenominationCode, Catalog.GetString("Enter a denomination code"));
        cmbDenominationCode.InitialiseUserControl();
        FPetraUtilsObject.SetStatusBarText(txtApproximateSize, Catalog.GetString("Enter the number of people in regular attendence"));
        FPetraUtilsObject.SetStatusBarText(chkMapOnFile, Catalog.GetString("Mark if there is a map to the church on file"));
        FPetraUtilsObject.SetStatusBarText(chkPrayerGroup, Catalog.GetString("Mark if this church is a prayer group"));
        FPetraUtilsObject.SetStatusBarText(cmbLanguageCode, Catalog.GetString("Select the partner's preferred language"));
        cmbLanguageCode.InitialiseUserControl();
        FPetraUtilsObject.SetStatusBarText(cmbAcquisitionCode, Catalog.GetString("Select a method-of-acquisition code"));
        cmbAcquisitionCode.InitialiseUserControl();
        FPetraUtilsObject.SetStatusBarText(txtContactPartnerKey, Catalog.GetString("Enter the partner who is to be addressed in correspondence"));
        FPetraUtilsObject.SetStatusBarText(chkAccomodation, Catalog.GetString("This church has accommodation for visitors, travelors or teams"));
        FPetraUtilsObject.SetStatusBarText(txtAccomodationSize, Catalog.GetString("How many people can they accomodate"));
        FPetraUtilsObject.SetStatusBarText(cmbAccomodationType, Catalog.GetString("Select type of accomodation"));
        cmbAccomodationType.InitialiseUserControl();
        InitializeManualCode();

        if(FMainDS != null)
        {
            ShowData(FMainDS.PChurch[0]);
        }
    }

    /// make sure that the primary key cannot be edited anymore
    public void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
    }

    private void ShowData(PChurchRow ARow)
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
        if (ARow.IsDenominationCodeNull())
        {
            cmbDenominationCode.SelectedIndex = -1;
        }
        else
        {
            cmbDenominationCode.SetSelectedString(ARow.DenominationCode);
        }
        if (ARow.IsApproximateSizeNull())
        {
            txtApproximateSize.Text = String.Empty;
        }
        else
        {
            txtApproximateSize.Text = ARow.ApproximateSize.ToString();
        }
        if (ARow.IsMapOnFileNull())
        {
            chkMapOnFile.Checked = false;
        }
        else
        {
            chkMapOnFile.Checked = ARow.MapOnFile;
        }
        if (ARow.IsPrayerGroupNull())
        {
            chkPrayerGroup.Checked = false;
        }
        else
        {
            chkPrayerGroup.Checked = ARow.PrayerGroup;
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
        if (ARow.IsContactPartnerKeyNull())
        {
            txtContactPartnerKey.Text = String.Empty;
        }
        else
        {
            txtContactPartnerKey.Text = String.Format("{0:0000000000}", ARow.ContactPartnerKey);
        }
        if (ARow.IsAccomodationNull())
        {
            chkAccomodation.Checked = false;
        }
        else
        {
            chkAccomodation.Checked = ARow.Accomodation;
        }
        if (ARow.IsAccomodationSizeNull())
        {
            txtAccomodationSize.Text = String.Empty;
        }
        else
        {
            txtAccomodationSize.Text = ARow.AccomodationSize.ToString();
        }
        if (ARow.IsAccomodationTypeNull())
        {
            cmbAccomodationType.SelectedIndex = -1;
        }
        else
        {
            cmbAccomodationType.SetSelectedString(ARow.AccomodationType);
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private void GetDataFromControls(PChurchRow ARow)
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
        if (cmbDenominationCode.SelectedIndex == -1)
        {
            ARow.SetDenominationCodeNull();
        }
        else
        {
            ARow.DenominationCode = cmbDenominationCode.GetSelectedString();
        }
        if (txtApproximateSize.Text.Length == 0)
        {
            ARow.SetApproximateSizeNull();
        }
        else
        {
            ARow.ApproximateSize = Convert.ToInt32(txtApproximateSize.Text);
        }
        ARow.MapOnFile = chkMapOnFile.Checked;
        ARow.PrayerGroup = chkPrayerGroup.Checked;
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
        if (txtContactPartnerKey.Text.Length == 0)
        {
            ARow.SetContactPartnerKeyNull();
        }
        else
        {
            ARow.ContactPartnerKey = Convert.ToInt64(txtContactPartnerKey.Text);
        }
        ARow.Accomodation = chkAccomodation.Checked;
        if (txtAccomodationSize.Text.Length == 0)
        {
            ARow.SetAccomodationSizeNull();
        }
        else
        {
            ARow.AccomodationSize = Convert.ToInt32(txtAccomodationSize.Text);
        }
        if (cmbAccomodationType.SelectedIndex == -1)
        {
            ARow.SetAccomodationTypeNull();
        }
        else
        {
            ARow.AccomodationType = cmbAccomodationType.GetSelectedString();
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
