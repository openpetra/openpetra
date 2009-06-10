/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       petrih, christiank
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
using System.Resources;
using Ict.Petra.Shared.MPartner.Partner.Data;
using SourceGrid;
using Ict.Common.Controls;
using Ict.Petra.Client.MCommon.Applink;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner
{
    /// Form to show all Partners of an Extract that already are subscribed to the
    /// Publication.
    public partial class TExtractPartnersSubscribedWinForm : TFrmPetraEdit
    {
        public void ShowPartners(DataTable Table, String PubCode, String PubName)
        {
            DataTable tmpTable;
            DataColumn column1;
            DataRow Row1;
            DataView tmpDV;

            tmpTable = new DataTable("Partners");
            column1 = new DataColumn();
            column1.DataType = System.Type.GetType("System.String");
            column1.ColumnName = "PartnerKey";
            column1.Caption = "Partner Key";
            column1.ReadOnly = false;
            column1.Unique = true;
            tmpTable.Columns.Add(column1);
            column1 = new DataColumn();
            column1.DataType = System.Type.GetType("System.String");
            column1.ColumnName = "PartnerName";
            column1.Caption = "Partner Name";
            column1.ReadOnly = false;
            column1.Unique = false;
            tmpTable.Columns.Add(column1);

            foreach (DataRow Row2 in Table.Rows)
            {
                Row1 = tmpTable.NewRow();
                Row1["PartnerKey"] = ((PPartnerRow)Row2).PartnerKey.ToString();
                Row1["PartnerName"] = ((PPartnerRow)Row2).PartnerShortName.ToString();
                tmpTable.Rows.Add(Row1);
            }

            tmpDV = tmpTable.DefaultView;
            tmpDV.AllowNew = false;
            tmpDV.AllowEdit = false;
            tmpDV.AllowDelete = false;

            // DataBind the DataGrid
            grdPartners.DataSource = new DevAge.ComponentModel.BoundDataView(tmpDV);
            lblMiddle.Text = "(" + PubCode + ") " + PubName;
        }

        private void TExtractPartnersSubscribedWinForm_Closing(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Need to call the following method in the Base Form to remove this Form from the Open Forms List
            TFrmPetra_Closing(this, null);
        }

        private void BtnHelp_Click(System.Object sender, System.EventArgs e)
        {
            TCmdMCommon.OpenScreenAboutPetra(this);
            TCmdMCommon.OpenHelp(this);
        }

        /// <summary>
        /// Private Declarations
        /// </summary>
        /// <returns>void</returns>
        public TExtractPartnersSubscribedWinForm() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

// this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnOK.Text = Catalog.GetString("&OK");
            this.lbltop.Text = Catalog.GetString("These partners already have a Subscription for");
            this.lblBottom.Text = Catalog.GetString("The Subscription was not added to the following Partners:");
            this.btnHelp.Text = Catalog.GetString("&Help");
            this.Text = Catalog.GetString("Partners Who Were Already Subscribed");
            #endregion

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}