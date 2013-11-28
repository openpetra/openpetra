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
using Ict.Petra.Shared.MPartner;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MPartner;

namespace Ict.Petra.Client.MPartner
{
    #region TPartnerFindScreenLogic

    /// <summary>
    /// todoComment
    /// </summary>
    public class TPartnerFindScreenLogic : System.Object
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public static TPartnerFindScreenLogic ULogic;

        /// <summary>Holds a reference to the Form that uses this Class.</summary>
        private IWin32Window FParentForm;

        /// <summary>Holds a reference to the DataGrid that is used to display the records</summary>
        private TSgrdDataGrid FDataGrid;

        /// <summary>The PartnerKey of the currently selected Row in the DataGrid</summary>
        private Int64 FPartnerKey = -1;

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
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TPartnerFindScreenLogic() : base()
        {
            ULogic = this;
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


            // First get rid of columns of previous searches...
            FDataGrid.Columns.Clear();
            LocalisedStrings.GetLocStrCounty(out LocalisedCountyLabel, out dummy);

            // done this way in case it changes
            LocalisedCountyLabel = LocalisedCountyLabel.Replace(":", "").Replace("&", "");

            if (ADetailedResults)
            {
                FDataGrid.AddTextColumn("Class", ASourceTable.Columns["p_partner_class_c"], PARTNERCLASS_COLUMNWIDTH);
                FDataGrid.AddTextColumn("Partner Key", ASourceTable.Columns["p_partner_key_n"]);
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
                    FDataGrid.AddTextColumn("Family Key", ASourceTable.Columns["p_family_key_n"]);
                }

                if (AVisibleFields.Contains("PhoneNumber"))
                {
                    FDataGrid.AddTextColumn("Telephone", ASourceTable.Columns["p_telephone_number_c"]);
                }

                if (AVisibleFields.Contains("Email"))
                {
                    FDataGrid.AddTextColumn("Email", ASourceTable.Columns["p_email_address_c"]);
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
                FDataGrid.AddTextColumn("Partner Name", ASourceTable.Columns["p_partner_short_name_c"]);

                if (ASearchForActivePartners)
                {
                    FDataGrid.AddTextColumn("Partner Status", ASourceTable.Columns["p_status_code_c"]);
                }

                FDataGrid.AddTextColumn("City", ASourceTable.Columns["p_city_c"]);
                FDataGrid.AddTextColumn("Addr2", ASourceTable.Columns["p_street_name_c"]);
                FDataGrid.AddTextColumn("Partner Key", ASourceTable.Columns["p_partner_key_n"]);

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

        #region Helper Functions

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public String DetermineCurrentEmailAddress()
        {
            DataRow CurrentDR = this.CurrentDataRow;
            String EmailAddress;

            if (CurrentDR != null)
            {
                // get Email Address of current DataRow
                EmailAddress = Convert.ToString(CurrentDR[PPartnerLocationTable.GetEmailAddressDBName()]);
            }
            else
            {
                EmailAddress = "";
            }

            // MessageBox.Show(EmailAddress.ToString);
            return EmailAddress;
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
    }
    #endregion

    #region TMenuFunctions

    /// <summary>
    /// todoComment
    /// </summary>
    public class TMenuFunctions
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public static void CopyPartnerKeyToClipboard()
        {
            Clipboard.SetDataObject(TPartnerFindScreenLogic.ULogic.PartnerKey.ToString());
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void DeletePartner()
        {
// TODO            Logic.UCmdMPartner.RunDeletePartner(Logic.ULogic.ParentForm, Logic.ULogic.PartnerKey);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void DuplicateAddressCheck()
        {
// TODO            Logic.UCmdMPartner.RunDuplicateAddressCheck(Logic.ULogic.ParentForm);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void ExportPartner()
        {
// TODO ExportPartner
#if TODO
            TLocationPK LocationPK;

            LocationPK = Logic.ULogic.DetermineCurrentLocationPK();
            Logic.UCmdMPartner.RunExportPartner(Logic.ULogic.ParentForm, Logic.ULogic.PartnerKey,
                LocationPK.SiteKey, LocationPK.LocationKey);
#endif
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void ImportPartner()
        {
// TODO            Logic.UCmdMPartner.RunImportPartner(Logic.ULogic.ParentForm, "");
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void MergeAddresses()
        {
// TODO            Logic.UCmdMPartner.RunMergeAddresses(Logic.ULogic.ParentForm);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void MergePartners()
        {
// TODO            Logic.UCmdMPartner.RunPartnerMerge(Logic.ULogic.ParentForm);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void OpenExtracts()
        {
// TODO            Logic.UCmdMPartner.OpenExtractsMainScreen(Logic.ULogic.ParentForm);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void PrintPartner()
        {
// TODO           Logic.UCmdMPartner.RunPrintPartner(Logic.ULogic.ParentForm, Logic.ULogic.PartnerKey);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void SendEmailToPartner()
        {
            String EmailAddress;
            TVerificationResult VerificationResult;
            Boolean EmailAddressValid;

            EmailAddress = TPartnerFindScreenLogic.ULogic.DetermineCurrentEmailAddress();
            VerificationResult = (TVerificationResult)TStringChecks.ValidateEmail(EmailAddress, true);

            if (VerificationResult == null)
            {
                EmailAddressValid = true;
            }
            else
            {
                EmailAddressValid = false;
            }

            if (EmailAddress == "")
            {
                MessageBox.Show(Catalog.GetString("No e-mail address for this Partner in the selected address record."),
                    Catalog.GetString("Cannot Send E-mail To Partner"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else if (!EmailAddressValid)
            {
                MessageBox.Show(
                    Catalog.GetString("No valid e-mail address for this Partner in the selected address record.") +
                    Environment.NewLine + Environment.NewLine +
                    Catalog.GetString("Details: ") +
                    Environment.NewLine +
                    VerificationResult.ResultText,
                    Catalog.GetString("Cannot Send E-mail To Partner"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
// TODO                Logic.UCmdMSysMan.SendEmail(EmailAddress);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void SubscriptionCancellation()
        {
// TODO            Logic.UCmdMPartner.RunSubscriptionCancellation(Logic.ULogic.ParentForm);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void SubscriptionExpiryNotices()
        {
// TODO            Logic.UCmdMPartner.RunSubscriptionExpiryNotices(Logic.ULogic.ParentForm);
        }
    }
    #endregion
}