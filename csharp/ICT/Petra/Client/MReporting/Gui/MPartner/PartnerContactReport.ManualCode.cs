//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using GNU.Gettext;
using SourceGrid;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using System.Resources;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Logic;


using Ict.Petra.Shared.MCommon.Data;


namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    /// <summary>
    /// manual code for TFrmBriefFoundationReport class
    /// </summary>
    public partial class TFrmPartnerContactReport
    {
        private PContactAttributeDetailTable FContactAttributesTable;
        private PContactAttributeDetailTable FSelectionTable;

        private void grdAttribute_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            // Load Contact Attributes
            PContactAttributeTable ContactAttributeTable = (PContactAttributeTable)TDataCache.TMPartner.GetCacheableMailingTable(
                TCacheableMailingTablesEnum.ContactAttributeList);

            grdAttribute.Columns.Clear();
            grdAttribute.AddTextColumn("Attribute", ContactAttributeTable.ColumnContactAttributeCode);

            DataView myDataView = ContactAttributeTable.DefaultView;
            myDataView.AllowNew = false;
            grdAttribute.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            grdAttribute.AutoSizeCells();

            // Do some other initialisation
            InitializeOtherControls();
        }

        private void grdDetail_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FContactAttributesTable = (PContactAttributeDetailTable)
                                      TDataCache.TMPartner.GetCacheableMailingTable(TCacheableMailingTablesEnum.ContactAttributeDetailList);

            grdDetail.Columns.Clear();
            grdDetail.AddTextColumn("Attribute", FContactAttributesTable.ColumnContactAttributeCode);
            grdDetail.AddTextColumn("Detail", FContactAttributesTable.ColumnContactAttrDetailCode);
            grdDetail.AddTextColumn("Description", FContactAttributesTable.ColumnContactAttrDetailDescr);
            grdDetail.Columns[0].Visible = false;
            grdDetail.AutoSizeCells();

            grdDetail.Selection.EnableMultiSelection = true;
        }

        private void grdSelection_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FSelectionTable = new PContactAttributeDetailTable();
            FSelectionTable.DefaultView.AllowNew = false;
            FSelectionTable.DefaultView.AllowDelete = true;
            FSelectionTable.DefaultView.AllowEdit = false;

            grdSelection.Columns.Clear();
            grdSelection.AddTextColumn("Attribute", FSelectionTable.ColumnContactAttributeCode);
            grdSelection.AddTextColumn("Detail", FSelectionTable.ColumnContactAttrDetailCode);
            grdSelection.AddTextColumn("Description", FSelectionTable.ColumnContactAttrDetailDescr);

            grdSelection.DataSource = new DevAge.ComponentModel.BoundDataView(FSelectionTable.DefaultView);
            grdSelection.AutoSizeCells();
            grdSelection.Selection.EnableMultiSelection = true;
        }

        private void InitializeOtherControls()
        {
            // Load MethodOfContact List
            PMethodOfContactTable MethodOfContactTable = (PMethodOfContactTable)TDataCache.TMPartner.GetCacheableMailingTable(
                TCacheableMailingTablesEnum.MethodOfContactList);

            cmbContact.Items.Add("*");

            foreach (PMethodOfContactRow Row in MethodOfContactTable.Rows)
            {
                cmbContact.Items.Add(Row.MethodOfContactCode);
            }

            cmbContact.SelectedIndex = 0;

            // Load User List
            SUserTable UserTable = (SUserTable)TDataCache.TMSysMan.GetCacheableSysManTable(TCacheableSysManTablesEnum.UserList);
            DataView dvActive = new DataView(UserTable);
            dvActive.RowFilter = string.Format("{0}=0 AND {1}=0", SUserTable.GetRetiredDBName(), SUserTable.GetAccountLockedDBName());
            dvActive.Sort = string.Format("{0}", SUserTable.GetUserIdDBName());

            cmbContactor.Items.Add("*");

            foreach (DataRowView drv in dvActive)
            {
                SUserRow row = (SUserRow)drv.Row;
                cmbContactor.Items.Add(row.UserId);
            }

            cmbContactor.SelectedIndex = 0;
        }

        private void grdAttribute_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
        }

        private void grdDetail_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
        }

        private void grdSelection_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (FSelectionTable.Rows.Count > 0)
            {
                // Add the contact attributes as sql command to param_contact_attributes
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append(" AND(");
                Boolean FirstAttribute = true;

                foreach (PContactAttributeDetailRow Row in FSelectionTable.Rows)
                {
                    if (FirstAttribute)
                    {
                        FirstAttribute = false;
                    }
                    else
                    {
                        sb.Append(" OR ");
                    }

                    String Attribute = Row.ContactAttributeCode;
                    String Detail = Row.ContactAttrDetailCode;

                    sb.Append(" (PUB_p_partner_contact_attribute.p_contact_attribute_code_c = ");
                    sb.Append('\'');
                    sb.Append(Attribute);
                    sb.Append('\'');
                    sb.Append(" AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = ");
                    sb.Append('\'');
                    sb.Append(Detail);
                    sb.Append('\'');
                    sb.Append(')');
                }

                sb.Append(")");

                ACalc.AddParameter("param_contact_attributes", sb.ToString());
                //MessageBox.Show(sb.ToString());
            }
            else
            {
                ACalc.AddParameter("param_contact_attributes", " ");
            }

            ACalc.AddParameter("NumberOfAddresses", 0);
            ACalc.AddParameter("ControlSource", "", ReportingConsts.HEADERCOLUMN);

            // TODO: If you want to export only the lines with relevant data and not the higher level lines
            // in csv export then enable this line
            // ACalc.AddParameter("csv_export_only_lowest_level", true);

            if (AReportAction == TReportActionEnum.raSave)
            {
                int Counter = 0;

                foreach (PContactAttributeDetailRow Row in FSelectionTable.Rows)
                {
                    ACalc.AddParameter("param_contact_attribute_attribute_" + Counter.ToString(), Row.ContactAttributeCode);
                    ACalc.AddParameter("param_contact_attribute_detail_" + Counter.ToString(), Row.ContactAttrDetailCode);
                    ACalc.AddParameter("param_contact_attribute_description_" + Counter.ToString(), Row.ContactAttrDetailDescr);
                    Counter++;
                }

                ACalc.AddParameter("param_number_of_contact_attributes", Counter);
            }
        }

        private void grdAttribute_SetControls(TParameterList AParameters)
        {
        }

        private void grdDetail_SetControls(TParameterList AParameters)
        {
        }

        private void grdSelection_SetControls(TParameterList AParameters)
        {
            FSelectionTable.Rows.Clear();
            int NumAttributes = AParameters.Get("param_number_of_contact_attributes").ToInt32();

            for (int Counter = 0; Counter < NumAttributes; ++Counter)
            {
                PContactAttributeDetailRow Row = FSelectionTable.NewRowTyped(true);

                Row.ContactAttributeCode = AParameters.Get("param_contact_attribute_attribute_" + Counter.ToString()).ToString();
                Row.ContactAttrDetailCode = AParameters.Get("param_contact_attribute_detail_" + Counter.ToString()).ToString();
                Row.ContactAttrDetailDescr = AParameters.Get("param_contact_attribute_description_" + Counter.ToString()).ToString();

                FSelectionTable.Rows.Add(Row);
            }
        }

        private void AttributeFocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            String AttributeCode = "";

            if (grdAttribute.SelectedDataRows.Length > 0)
            {
                AttributeCode = (String)((DataRowView)grdAttribute.SelectedDataRows[0]).Row[0];
            }

            PContactAttributeDetailTable tmpTable = (PContactAttributeDetailTable)FContactAttributesTable.Copy();

            for (int Counter = tmpTable.Rows.Count - 1; Counter >= 0; --Counter)
            {
                if ((String)tmpTable.Rows[Counter][PContactAttributeDetailTable.ColumnContactAttributeCodeId] != AttributeCode)
                {
                    tmpTable.Rows.RemoveAt(Counter);
                }
            }

            TDataCache.TMPartner.GetCacheableMailingTable(TCacheableMailingTablesEnum.ContactAttributeDetailList);

            tmpTable.DefaultView.AllowNew = false;
            grdDetail.DataSource = new DevAge.ComponentModel.BoundDataView(tmpTable.DefaultView);
            grdDetail.AutoSizeCells();

            grdDetail.Selection.ResetSelection(true);
        }

        private void grdDetailDoubleClick(System.Object sender, EventArgs e)
        {
            AddDetail(sender, e);
        }

        private void grdSelectionDoubleClick(System.Object sender, EventArgs e)
        {
            RemoveDetail(sender, e);
        }

        private void AddDetail(System.Object sender, EventArgs e)
        {
            String Attribute = "";
            String Detail = "";
            String Description = "";

            for (int Counter = 0; Counter < grdDetail.SelectedDataRows.Length; ++Counter)
            {
                Attribute = (String)((DataRowView)grdDetail.SelectedDataRows[Counter]).Row[0];
                Detail = (String)((DataRowView)grdDetail.SelectedDataRows[Counter]).Row[1];
                Description = (String)((DataRowView)grdDetail.SelectedDataRows[Counter]).Row[2];

                PContactAttributeDetailRow newRow = (PContactAttributeDetailRow)FSelectionTable.NewRow();
                newRow.Active = true;

                newRow.ContactAttributeCode = Attribute;
                newRow.ContactAttrDetailCode = Detail;
                newRow.ContactAttrDetailDescr = Description;

                if (FSelectionTable.Rows.Find(new String[] { Attribute, Detail }) == null)
                {
                    FSelectionTable.Rows.Add(newRow);
                }
            }
        }

        private void RemoveDetail(System.Object sender, EventArgs e)
        {
            for (int Counter = grdSelection.SelectedDataRows.Length - 1; Counter >= 0; --Counter)
            {
                String Attribute = (String)((DataRowView)grdSelection.SelectedDataRows[Counter]).Row[0];
                String Detail = (String)((DataRowView)grdSelection.SelectedDataRows[Counter]).Row[1];

                for (int Counter2 = FSelectionTable.Rows.Count - 1; Counter2 >= 0; --Counter2)
                {
                    PContactAttributeDetailRow currentRow = (PContactAttributeDetailRow)FSelectionTable.Rows[Counter2];

                    if ((currentRow.ContactAttributeCode == Attribute)
                        && (currentRow.ContactAttrDetailCode == Detail))
                    {
                        FSelectionTable.Rows.RemoveAt(Counter);
                        break;
                    }
                }
            }

            grdSelection.Selection.ResetSelection(true);
        }
    }
}