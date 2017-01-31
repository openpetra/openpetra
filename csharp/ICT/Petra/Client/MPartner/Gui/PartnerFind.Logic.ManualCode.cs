//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common.Controls;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MPartner;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MPartner;

namespace Ict.Petra.Client.MPartner.Gui
{
    #region TPartnerFindScreen_Logic

    /// <summary>
    /// todoComment
    /// </summary>
    public class TPartnerFindScreen_Logic
    {
        /// <summary>Holds a reference to the Form that uses this Class.</summary>
        private IWin32Window FParentForm;

        /// <summary>Holds a reference to the DataGrid that is used to display the records</summary>
        private TSgrdDataGrid FDataGrid;

        /// <summary>Holds a reference to the 'Partner Info' Collapsible Panel.</summary>
        private TPnlCollapsible FPartnerInfoCollPanel;

        /// <summary>The PartnerKey of the currently selected Row in the DataGrid</summary>
        private Int64 FPartnerKey = -1;

        /// <summary>Last PartnerKey for which the Partner Info Panel was opened.</summary>
        private Int64 FLastPartnerKeyInfoPanelOpened = -1;

        /// <summary>Last LocationKey for which the Partner Info Panel was opened.</summary>
        private TLocationPK FLastLocationPKInfoPanelOpened = new TLocationPK();


        /// <summary>DataGrid that is used to display the records</summary>
        public TSgrdDataGrid DataGrid
        {
            get
            {
                return FDataGrid;
            }

            set
            {
                FDataGrid = value;
            }
        }

        /// <summary>todoComment</summary>
        public IWin32Window ParentForm
        {
            get
            {
                return FParentForm;
            }

            set
            {
                FParentForm = value;
            }
        }

        /// <summary>todoComment</summary>
        public Int64 PartnerKey
        {
            get
            {
                return FPartnerKey;
            }

            set
            {
                FPartnerKey = value;
            }
        }

        /// <summary>todoComment</summary>
        public DataRow CurrentDataRow
        {
            get
            {
                if (FDataGrid != null)
                {
                    DataRowView[] TheDataRowViewArray = FDataGrid.SelectedDataRowsAsDataRowView;

                    if (TheDataRowViewArray.Length > 0)
                    {
                        return TheDataRowViewArray[0].Row;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>Reference to the 'Partner Info' Collapsible Panel.</summary>
        public TPnlCollapsible PartnerInfoCollPanel
        {
            get
            {
                return FPartnerInfoCollPanel;
            }

            set
            {
                FPartnerInfoCollPanel = value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ASourceTable"></param>
        /// <param name="ADetailedResults"></param>
        /// <param name="ASearchForActivePartners"></param>
        /// <param name="AVisibleFields"></param>
        public void CreateColumns(DataTable ASourceTable, Boolean ADetailedResults, Boolean ASearchForActivePartners, ArrayList AVisibleFields)
        {
            const int PARTNERCLASS_COLUMNWIDTH = 65;

            String dummy;
            String LocalisedCountyLabel;

/* The following code should work, but for some reason nothing happens... (not critical, but would be nice)
 *          //Create a custom View class and a Condition that uses it
 *          SourceGrid.Cells.Views.Cell NonActivePartnerView = new SourceGrid.Cells.Views.Cell();
 *          NonActivePartnerView.ForeColor = Color.DarkGray;
 *          NonActivePartnerView.BackColor = Color.Green;
 *
 *          SourceGrid.Conditions.ConditionView ConditionNonActive =
 *              new SourceGrid.Conditions.ConditionView(NonActivePartnerView);
 *
 *          ConditionNonActive.EvaluateFunction = delegate(SourceGrid.DataGridColumn column,
 *                                                            int gridRow, object itemRow)
 *          {
 *              DataRowView row = (DataRowView)itemRow;
 *
 *              MessageBox.Show("Partner Status: " + row[1]);
 *
 *              if (row["p_status_code_c"] != "ACTIVE")
 *              {
 *                  MessageBox.Show("Partner not Active: " + row[1]);
 *                  return true;
 *              }
 *              else
 *              {
 *                  return false;
 *              }
 *          };
 */
            LocalisedStrings.GetLocStrCounty(out LocalisedCountyLabel, out dummy);

            // done this way in case it changes
            LocalisedCountyLabel = LocalisedCountyLabel.Replace(":", "").Replace("&", "");

            if (ADetailedResults)
            {
                FDataGrid.AddTextColumn("Class", ASourceTable.Columns["p_partner_class_c"], PARTNERCLASS_COLUMNWIDTH);
                FDataGrid.AddPartnerKeyColumn("Partner Key", ASourceTable.Columns["p_partner_key_n"]);
                FDataGrid.AddTextColumn("Partner Name", ASourceTable.Columns["p_partner_short_name_c"]);

                if (AVisibleFields.Contains("PreviousName"))
                {
                    FDataGrid.AddTextColumn("Previous Name", ASourceTable.Columns["p_previous_name_c"]);
                }

                if (ASearchForActivePartners)
                {
                    FDataGrid.AddTextColumn("Partner Status", ASourceTable.Columns["p_status_code_c"]);
                }

                FDataGrid.AddTextColumn("City", ASourceTable.Columns["p_city_c"]);
                FDataGrid.AddTextColumn("Post Code", ASourceTable.Columns["p_postal_code_c"]);
                FDataGrid.AddTextColumn("Addr1", ASourceTable.Columns["p_locality_c"]);
                FDataGrid.AddTextColumn("Addr2", ASourceTable.Columns["p_street_name_c"]);

                if (AVisibleFields.Contains("Address3"))
                {
                    FDataGrid.AddTextColumn("Addr3", ASourceTable.Columns["p_address_3_c"]);
                }

                if (AVisibleFields.Contains("County"))
                {
                    FDataGrid.AddTextColumn(LocalisedCountyLabel, ASourceTable.Columns["p_county_c"]);
                }

                FDataGrid.AddTextColumn("Country", ASourceTable.Columns["p_country_code_c"]);

                if (ASourceTable.Columns.Contains("p_family_key_n"))
                {
                    FDataGrid.AddPartnerKeyColumn("Family Key", ASourceTable.Columns["p_family_key_n"]);
                }

                if (!ASearchForActivePartners)
                {
                    FDataGrid.AddTextColumn("Partner Status", ASourceTable.Columns["p_status_code_c"]);
                }

                FDataGrid.AddTextColumn("Location Type", ASourceTable.Columns["p_location_type_c"]);
                FDataGrid.AddTextColumn("Location Key", ASourceTable.Columns["p_location_key_i"]);
            }
            else
            {
                FDataGrid.AddTextColumn("Class", ASourceTable.Columns["p_partner_class_c"], PARTNERCLASS_COLUMNWIDTH);
                FDataGrid.AddPartnerKeyColumn("Partner Key", ASourceTable.Columns["p_partner_key_n"]);
                FDataGrid.AddTextColumn("Partner Name", ASourceTable.Columns["p_partner_short_name_c"]);

                if (ASearchForActivePartners)
                {
                    FDataGrid.AddTextColumn("Partner Status", ASourceTable.Columns["p_status_code_c"]);
                }

                FDataGrid.AddTextColumn("City", ASourceTable.Columns["p_city_c"]);
                FDataGrid.AddTextColumn("Addr2", ASourceTable.Columns["p_street_name_c"]);

                if (!ASearchForActivePartners)
                {
                    FDataGrid.AddTextColumn("Partner Status", ASourceTable.Columns["p_status_code_c"]);
                }

                FDataGrid.AddTextColumn("Location Key", ASourceTable.Columns["p_location_key_i"]);
            }

/*
 *          // Add Condition to all Columns of the Grid
 *                      foreach (SourceGrid.DataGridColumn col in FDataGrid.Columns)
 *                      {
 *                              col.Conditions.Add(ConditionNonActive);
 *                      }
 */
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ASourceTable"></param>
        /// <param name="ASearchForActivePartners"></param>
        /// <param name="AVisibleFields"></param>
        public void CreateBankDetailsColumns(DataTable ASourceTable, Boolean ASearchForActivePartners, ArrayList AVisibleFields)
        {
            const int PARTNERCLASS_COLUMNWIDTH = 65;

            String dummy;
            String LocalisedCountyLabel;

            LocalisedStrings.GetLocStrCounty(out LocalisedCountyLabel, out dummy);

            // done this way in case it changes
            LocalisedCountyLabel = LocalisedCountyLabel.Replace(":", "").Replace("&", "");

            FDataGrid.AddTextColumn("Class", ASourceTable.Columns["p_partner_class_c"], PARTNERCLASS_COLUMNWIDTH);
            FDataGrid.AddTextColumn("Partner Key", ASourceTable.Columns["p_partner_key_n"]);
            FDataGrid.AddTextColumn("Partner Name", ASourceTable.Columns["p_partner_short_name_c"]);
            FDataGrid.AddTextColumn("Account Number", ASourceTable.Columns["p_bank_account_number_c"]);
            FDataGrid.AddTextColumn("Account Name", ASourceTable.Columns["p_account_name_c"]);
            FDataGrid.AddTextColumn("Bank Name", ASourceTable.Columns["p_branch_name_c"]);
            FDataGrid.AddTextColumn("Bank/Branch Code", ASourceTable.Columns["p_branch_code_c"]);
            FDataGrid.AddTextColumn("BIC/SWIFT Code", ASourceTable.Columns["p_bic_c"]);
            FDataGrid.AddTextColumn("IBAN", ASourceTable.Columns["p_iban_c"]);
            FDataGrid.AddTextColumn("Expiry Date", ASourceTable.Columns["p_expiry_date_d"]);
            FDataGrid.AddTextColumn("Comment", ASourceTable.Columns["p_comment_c"]);
        }

        #region Helper Functions

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public void SendEmailToPartner(TFrmPetraUtils APetraUtilsObject, bool AEmailToClipboard)
        {
            if (FPartnerInfoCollPanel.UserControlInstance == null)
            {
                FPartnerInfoCollPanel.RealiseUserControlNow();
            }

            var PartnerInfoUC = ((TUC_PartnerInfo)(FPartnerInfoCollPanel.UserControlInstance));
            PartnerInfoUC.PetraUtilsObject = APetraUtilsObject;
            PartnerInfoUC.InitUserControl();

            if (!AEmailToClipboard)
            {
                PartnerInfoUC.DataLoaded += PartnerInfoUC_DataLoaded;
            }

            // Ask the UserControl to load the data for the Partner; once it is finished
            // the 'DataLoaded' Event will fire and the Email can get sent - if the Partner
            // has indeed got a 'Primary E-Mail Address'
            if (!UpdatePartnerInfoPanel(false, PartnerInfoUC))
            {
                if (AEmailToClipboard)
                {
                    String PrimaryEmailAddress;

                    if (Calculations.GetPrimaryEmailAddress((PartnerInfoUC).GetPartnerAttributeData(), out PrimaryEmailAddress))
                    {
                        Clipboard.SetDataObject(PrimaryEmailAddress);
                    }
                    else
                    {
                        Clipboard.SetDataObject(String.Empty);
                    }
                }
                else
                {
                    PartnerInfoUC_DataLoaded(PartnerInfoUC, null);
                }
            }
        }

        /// <summary>
        /// Data for the Partner got loaded and we can determine the Partners' 'Primary E-Mail Address'.
        /// If the Partner has got one we will ask the the E-Mail program to open a new E-Mail with
        /// the Partner's E-Mail address as the recipient of the E-Mail.
        /// </summary>
        /// <param name="Sender">Our instance of <see cref="TUC_PartnerInfo"/>.</param>
        /// <param name="e">Ignored.</param>
        private void PartnerInfoUC_DataLoaded(object Sender, Types.TPartnerKeyData e)
        {
            String PrimaryEmailAddress;

            if (Calculations.GetPrimaryEmailAddress(
                    ((TUC_PartnerInfo)Sender).GetPartnerAttributeData(), out PrimaryEmailAddress))
            {
//            MessageBox.Show(PrimaryEmailAddress.ToString());

                TRtbHyperlinks.DisplayHelper Launcher = new TRtbHyperlinks.DisplayHelper(new TRtbHyperlinks());

                Launcher.LaunchHyperLink(PrimaryEmailAddress, THyperLinkHandling.HYPERLINK_PREFIX_EMAILLINK);
            }
            else
            {
                MessageBox.Show(MPartnerResourcestrings.StrNoPrimaryEmailAvailableToSendEmailTo,
                    MPartnerResourcestrings.StrNoPrimaryEmailAvailableToSendEmailToTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // We don't want to get any further notifications (until the 'SendEmailToPartner' Method hooks myself up again!)
            ((TUC_PartnerInfo)Sender).DataLoaded -= PartnerInfoUC_DataLoaded;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public TLocationPK DetermineCurrentLocationPK()
        {
            DataRow CurrentDR = this.CurrentDataRow;
            TLocationPK LocationPK;

            if (CurrentDR != null)
            {
                // get LocationKey of current DataRow
                LocationPK =
                    new TLocationPK(Convert.ToInt64(CurrentDR[PPartnerLocationTable.GetSiteKeyDBName()]),
                        Convert.ToInt32(CurrentDR[PPartnerLocationTable.GetLocationKeyDBName()]));
            }
            else
            {
                LocationPK = new TLocationPK(-1, -1);
            }

            // MessageBox.Show('SiteKey: ' + LocationPK.SiteKey.ToString + "\r\n" +
            // 'LocationKey: ' + LocationPK.LocationKey.ToString);
            return LocationPK;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public String DetermineCurrentPartnerClass()
        {
            DataRow CurrentDR = this.CurrentDataRow;
            String PartnerClass;

            if (CurrentDR != null)
            {
                // get PartnerClass of current DataRow
                PartnerClass = Convert.ToString(CurrentDR[PPartnerTable.GetPartnerClassDBName()]);
            }
            else
            {
                PartnerClass = "";
            }

            // MessageBox.Show(PartnerClass);
            return PartnerClass;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public String DetermineCurrentPartnerStatus()
        {
            DataRow CurrentDR = this.CurrentDataRow;
            String PartnerStatus;

            if (CurrentDR != null)
            {
                // get PartnerStatus of current DataRow
                PartnerStatus = Convert.ToString(CurrentDR[PPartnerTable.GetStatusCodeDBName()]);
            }
            else
            {
                PartnerStatus = "";
            }

            // MessageBox.Show(PartnerStatus);
            return PartnerStatus;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public Int64 DetermineCurrentPartnerKey()
        {
            DataRow CurrentDR = this.CurrentDataRow;

            if (CurrentDR != null)
            {
                // get PartnerKey of current DataRow
                FPartnerKey = Convert.ToInt64(CurrentDR[PPartnerTable.GetPartnerKeyDBName()]);
            }
            else
            {
                FPartnerKey = -1;
            }

            // MessageBox.Show(FPartnerKey.ToString);
            return FPartnerKey;
        }

        /// <summary>
        /// Retrieves from the server the value if the selected partner is a foundation.
        /// </summary>
        /// <param name="AIsFoundation">True if the partner (organisation) is a foundation. Otherwise false.</param>
        public void DetermineCurrentFoundationStatus(out Boolean AIsFoundation)
        {
            AIsFoundation = false;

            if (FPartnerKey != -1)
            {
                Ict.Petra.Client.App.Core.TServerLookup.TMPartner.GetPartnerFoundationStatus(PartnerKey, out AIsFoundation);
            }
        }

        /// <summary>
        /// Checks if the current user can access this Partner.
        /// </summary>
        /// <param name="APartnerKey">The PartnerKey to check. Pass in -1 to use the
        /// PartnerKey of the currently selected Partner in the Search Result Grid.</param>
        /// <returns>True if the Partner can be accessed, otherwise false.</returns>
        public bool CanAccessPartner(Int64 APartnerKey)
        {
            TPartnerClass PartnerClass;
            String PartnerShortName;
            Boolean ValidPartner;
            Boolean PartnerIsMerged;
            Boolean UserCanAccessPartner;

            if (APartnerKey > 0)
            {
                ValidPartner = TServerLookup.TMPartner.VerifyPartner(APartnerKey,
                    out PartnerShortName, out PartnerClass, out PartnerIsMerged,
                    out UserCanAccessPartner);

                if (ValidPartner
                    && UserCanAccessPartner)
                {
                    return true;
                }
            }
            else
            {
                if ((APartnerKey == -1)
                    && (FPartnerKey != -1))
                {
                    return CanAccessPartner(FPartnerKey);
                }
            }

            return false;
        }

        #endregion


        /// <summary>
        /// Causes the 'Partner Info' UserControl to update itself.
        /// </summary>
        /// <param name="ALocationDataAvailable">Set to true if Location data is available.</param>
        /// <param name="APartnerInfoUC">Instance of the PartnerInfo UserControl</param>
        /// <returns>True if an 'update' was done, false if the Partner Info Control already had current data and hence no
        /// update was done.</returns>
        public bool UpdatePartnerInfoPanel(bool ALocationDataAvailable, TUC_PartnerInfo APartnerInfoUC)
        {
            bool ReturnValue = false;
            TLocationPK CurrentLocationPK;

            CurrentLocationPK = DetermineCurrentLocationPK();

            //                MessageBox.Show("Current PartnerKey: " + PartnerKey.ToString() + Environment.NewLine +
            //                                "FLastPartnerKeyInfoPanelOpened: " + FLastPartnerKeyInfoPanelOpened.ToString() + Environment.NewLine +
            //                                "CurrentLocationPK: " + CurrentLocationPK.SiteKey.ToString() + ", " + CurrentLocationPK.LocationKey.ToString() + Environment.NewLine +
            //                                "FLastLocationPKInfoPanelOpened: " + FLastLocationPKInfoPanelOpened.SiteKey.ToString() + ", " + FLastLocationPKInfoPanelOpened.LocationKey.ToString());
            if ((CurrentDataRow != null)
                && (((FLastPartnerKeyInfoPanelOpened == PartnerKey)
                     && ((FLastLocationPKInfoPanelOpened.SiteKey != CurrentLocationPK.SiteKey)
                         || (FLastLocationPKInfoPanelOpened.LocationKey != CurrentLocationPK.LocationKey)))
                    || (FLastPartnerKeyInfoPanelOpened != PartnerKey)))
            {
                FLastPartnerKeyInfoPanelOpened = PartnerKey;
                FLastLocationPKInfoPanelOpened = CurrentLocationPK;

                if (ALocationDataAvailable)
                {
                    // We have Location data available
                    APartnerInfoUC.PassPartnerDataPartialWithLocation(PartnerKey, CurrentDataRow);
                }
                else
                {
                    // We don't have Location data available
                    APartnerInfoUC.PassPartnerDataPartialWithoutLocation(PartnerKey, CurrentDataRow);
                }

                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Resets the 'last partner' data that is held in the 'Partner Info' UserControl.
        /// </summary>
        public void ResetLastPartnerDataInfoPanel()
        {
            FLastPartnerKeyInfoPanelOpened = -1;
            FLastLocationPKInfoPanelOpened = new TLocationPK(-1, -1);
        }

        /// <summary>
        /// Deletes the currently selected Partner.
        /// </summary>
        public bool DeletePartner()
        {
            TFormsMessage BroadcastMessage;

            if (TPartnerMain.DeletePartner(FPartnerKey, ((UserControl) this.ParentForm).ParentForm))
            {
                BroadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcPartnerDeleted,
                    null);

                BroadcastMessage.SetMessageDataPartner(
                    FPartnerKey,
                    SharedTypes.PartnerClassStringToEnum(DetermineCurrentPartnerClass()),
                    "",
                    DetermineCurrentPartnerStatus());

                TFormsList.GFormsList.BroadcastFormMessage(BroadcastMessage);

                return true;
            }

            return false;
        }
    }

    #endregion

    #region TMenuFunctions

    /// <summary>
    /// todoComment
    /// </summary>
    public class TMenuFunctions
    {
        readonly TPartnerFindScreen_Logic FLogic = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ALogic">Instance of TPartnerFindScreen_Logic.</param>
        public TMenuFunctions(TPartnerFindScreen_Logic ALogic)
        {
            FLogic = ALogic;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void CopyPartnerKeyToClipboard()
        {
            Clipboard.SetDataObject(StringHelper.PartnerKeyToStr(FLogic.PartnerKey));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void DeletePartner()
        {
            FLogic.DeletePartner();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void SendEmailToPartner(TFrmPetraUtils APetraUtilsObject)
        {
            FLogic.SendEmailToPartner(APetraUtilsObject, false);
        }
    }

    #endregion
}