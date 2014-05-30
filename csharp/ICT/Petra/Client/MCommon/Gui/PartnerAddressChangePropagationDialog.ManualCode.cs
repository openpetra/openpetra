//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2014 by OM International
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
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MCommon.Gui
{
    /// <summary>
    /// manual code for TFrmPartnerAddressChangePropagationDialog class
    /// </summary>
    public partial class TFrmPartnerAddressChangePropagationDialog
    {
        private String FUserAnswer;

        // DataView that contains records for clbAddress to be displayed
        private System.Data.DataView FPartnerSharingLocationDV;

        /// <summary>
        /// The screen has been shown
        /// </summary>
        private void RunOnceOnActivationManual()
        {
            // enable autofind in list for first character (so the user can press character to find list entry)
            this.clbAddress.AutoFindColumn = ((Int16)(1));
            this.clbAddress.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;

            clbAddress.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));


            string CheckedColumn = "CHECKED";
            string ShortNameColumn = PPartnerTable.GetPartnerShortNameDBName();
            string PartnerKeyColumn = PPartnerLocationTable.GetPartnerKeyDBName();
            string PartnerClassColumn = PPartnerTable.GetPartnerClassDBName();
            string TelephoneColumn = PPartnerLocationTable.GetTelephoneNumberDBName();
            string LocationTypeColumn = PPartnerLocationTable.GetLocationTypeDBName();

            DataTable NewTable = FPartnerSharingLocationDV.ToTable(true, new string[] { ShortNameColumn, PartnerKeyColumn, PartnerClassColumn, TelephoneColumn, LocationTypeColumn });
            NewTable.Columns.Add(new DataColumn(CheckedColumn, typeof(bool)));

            clbAddress.Columns.Clear();
            clbAddress.AddCheckBoxColumn("", NewTable.Columns[CheckedColumn], 17, false);
            clbAddress.AddTextColumn(Catalog.GetString("Name"), NewTable.Columns[ShortNameColumn], 240);
            clbAddress.AddPartnerKeyColumn(Catalog.GetString("Partner Key"), NewTable.Columns[PartnerKeyColumn], 90);
            clbAddress.AddTextColumn(Catalog.GetString("Partner Class"), NewTable.Columns[PartnerClassColumn], 90);
            clbAddress.AddTextColumn(Catalog.GetString("Telephone"), NewTable.Columns[TelephoneColumn], 130);
            clbAddress.AddTextColumn(Catalog.GetString("Location Type"), NewTable.Columns[LocationTypeColumn], 90);

            clbAddress.DataBindGrid(NewTable, ShortNameColumn, CheckedColumn, PartnerKeyColumn, false, true, false);

            // initialize list of checked items
            clbAddress.SetCheckedStringList("");

        }

        private void ApplyText(String AOtherFormTitle, String AOtherExplanation, PLocationRow ALocationRow)
        {
            PLocationTable LocationDT;

            if (AOtherExplanation != "")
            {
                lblExplainPartnerList1.Text = AOtherExplanation;
            }
            else
            {
                lblExplainPartnerList1.Text = Catalog.GetString("The following Partners also use this Partner's address.");
            }

            if (AOtherFormTitle != "")
            {
                this.Text = AOtherFormTitle;
            }
            else
            {
                this.Text = Catalog.GetString("Change Location for selected Partners");
            }

            /* Set up address lines display */
            LocationDT = (PLocationTable)ALocationRow.Table;
            txtChangedAddress.Text =
                TSaveConvert.StringColumnToString(LocationDT.ColumnLocality, ALocationRow) + "\r\n" + TSaveConvert.StringColumnToString(
                    LocationDT.ColumnStreetName,
                    ALocationRow) + "\r\n" +
                TSaveConvert.StringColumnToString(LocationDT.ColumnAddress3, ALocationRow) + "\r\n" + TSaveConvert.StringColumnToString(
                    LocationDT.ColumnCity,
                    ALocationRow) + ' ' +
                TSaveConvert.StringColumnToString(LocationDT.ColumnPostalCode, ALocationRow) + "\r\n" + TSaveConvert.StringColumnToString(
                    LocationDT.ColumnCounty,
                    ALocationRow) + ' ' + TSaveConvert.StringColumnToString(LocationDT.ColumnCountryCode, ALocationRow);
        }

        private void SelectAllRecords(System.Object sender, EventArgs e)
        {
            string SelectString = "";

            foreach (DataRowView rowView in FPartnerSharingLocationDV)
            {
                DataRow row = rowView.Row;
                if (SelectString.Length > 0)
                {
                    SelectString = SelectString + ",";
                }
                SelectString = SelectString + row[PPartnerLocationTable.GetPartnerKeyDBName()].ToString();
            }

            clbAddress.SetCheckedStringList(SelectString);
        }

        private void DeselectAllRecords(System.Object sender, EventArgs e)
        {
            clbAddress.SetCheckedStringList("");
        }
        
        private void BtnOK_Click(System.Object sender, EventArgs e)
        {
            if (clbAddress.CheckedItemsCount == 0)
            {
                FUserAnswer = "CHANGE-NONE";
            }
            else if (clbAddress.CheckedItemsCount == FPartnerSharingLocationDV.Count)
            {
                FUserAnswer = "CHANGE-ALL";
            }
            else
            {
                FUserAnswer = "CHANGE-SOME" + ':';

                FUserAnswer = FUserAnswer + clbAddress.GetCheckedStringList();

                // return list separated by ';'
                FUserAnswer.Replace(',', ';');
            }

            //MessageBox.Show("FUserAnswer: " + FUserAnswer);

            if ((FUserAnswer == "CHANGE-NONE") || (FUserAnswer.StartsWith("CHANGE-SOME")))
            {
                /* Check whether user has CREATE right on p_location table */
                if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PLocationTable.GetTableDBName()))
                {
                    TMessages.MsgSecurityException(new ESecurityDBTableAccessDeniedException("", "create",
                            PLocationTable.GetTableDBName()), this.GetType());
                    MessageBox.Show(Catalog.GetString("Due to the selection that you have made" + " a new Address would need" + "\r\n" +
                                                      "to be created. However, you do not have permission to do this." + "\r\n" + "\r\n" +
                                                      "Either select 'Change all' to change all addresses (if this is appropriate)," +
                                                      "\r\n" + "or choose 'Cancel' to abort the Save operation."),
                        Catalog.GetString("Security Violation - Explanation"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }

            Close();
        }

        /// <summary>
        /// set parameters before screen is opened
        /// </summary>
        /// <param name="AAddressAddedOrChangedPromotionDR"></param>
        /// <param name="APartnerSharingLocationDV"></param>
        /// <param name="ALocationRow">Location Row that has been modified</param>
        /// <param name="AOtherFormTitle">Fill this if form should not use standard title</param>
        /// <param name="AOtherExplanation">Fill this if explanation should not be standard one</param>
        /// <returns></returns>
        public void SetParameters(PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow AAddressAddedOrChangedPromotionDR,
            DataView APartnerSharingLocationDV,
            PLocationRow ALocationRow,
            String AOtherFormTitle,
            String AOtherExplanation)
        {
            //TODOWB FAddressAddedOrChangedPromotionDR = AAddressAddedOrChangedPromotionDR;
            FPartnerSharingLocationDV = APartnerSharingLocationDV;

            /* MessageBox.Show('FPartnerSharingLocationDV.Count: ' + FPartnerSharingLocationDV.Count.ToString); */
            //TODOWB FLocationRow = ALocationRow;
            ApplyText(AOtherFormTitle, AOtherExplanation, ALocationRow);
        }

        /// <summary>
        /// return parameters after screen is closed
        /// </summary>
        /// <param name="AUserAnswer">User Answer</param>
        /// <returns>Boolean</returns>
        public Boolean GetReturnedParameters(out String AUserAnswer)
        {
            Boolean ReturnValue = true;

            AUserAnswer = FUserAnswer;

            return ReturnValue;
        }

        /// <summary>
        /// save the changes on the screen
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            //TODOWB
            return true;
        }
    }
}