/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
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
using System.Data;
using System.Windows.Forms;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MFinance.Gui
{
    /// <summary>
    /// Import bank statements and put them in the right sub system (gift, ap, gl, etc).
    /// At the moment, this is just a non functional prototype 
    /// that shows how the transactions on a bank statement can 
    /// be categorized and matched automatically to partners (eg. donors and recipients, suppliers, etc)
    /// </summary>
    public partial class TFrmBankStatementImport : TFrmPetra
    {

        public TFrmBankStatementImport()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.xpToolBarButton1.Text = Catalog.GetString("Print");
            this.xpToolBarButton2.Text = Catalog.GetString("Import New Bank statements");
            this.tabPage1.Text = Catalog.GetString("Unmatched");
            this.tabPage2.Text = Catalog.GetString("Recurring Gifts");
            this.tabPage4.Text = Catalog.GetString("One Time Gifts");
            this.tabPage3.Text = Catalog.GetString("Other (GL, AP, etc)");
            this.radioButton1.Text = Catalog.GetString("Recurring Gift");
            this.radioButton2.Text = Catalog.GetString("One Time Gift");
            this.radioButton3.Text = Catalog.GetString("Other (GL, AP, etc)");
            this.radioButton4.Text = Catalog.GetString("Unmatched");
            this.groupBox1.Text = Catalog.GetString("Gift");
            this.tabPage5.Text = Catalog.GetString("Donor Details");
            this.button1.Text = Catalog.GetString("Create New Partner");
            this.label21.Text = Catalog.GetString("Donor:");
            this.button8.Text = Catalog.GetString("Find Donor by Name");
            this.button7.Text = Catalog.GetString("Find Donor by Bank details");
            this.button6.Text = Catalog.GetString("Edit Donor");
            this.label5.Text = Catalog.GetString("Letter Code:");
            this.label4.Text = Catalog.GetString("Reference:");
            this.label3.Text = Catalog.GetString("Annual Receipt");
            this.label2.Text = Catalog.GetString("Method of Payment:");
            this.label1.Text = Catalog.GetString("Method of Giving:");
            this.ttxtPartnerKeyTextBox1.LabelText = Catalog.GetString("Name, Address,  Partner Class");
            this.tabPage6.Text = Catalog.GetString("Gift Details");
            this.label20.Text = Catalog.GetString("Mailing:");
            this.checkBox3.Text = Catalog.GetString("Confidential");
            this.checkBox2.Text = Catalog.GetString("Admin Grants");
            this.checkBox1.Text = Catalog.GetString("Tax deductable");
            this.label18.Text = Catalog.GetString("for:");
            this.label19.Text = Catalog.GetString("Comment 3:");
            this.label16.Text = Catalog.GetString("for:");
            this.label17.Text = Catalog.GetString("Comment 2:");
            this.label15.Text = Catalog.GetString("for:");
            this.label14.Text = Catalog.GetString("Comment 1:");
            this.label13.Text = Catalog.GetString("Account:");
            this.label12.Text = Catalog.GetString("Cost Centre:");
            this.label11.Text = Catalog.GetString("Motivation Detail:");
            this.label10.Text = Catalog.GetString("Motivation Group:");
            this.textBox3.Text = Catalog.GetString("EUR");
            this.label9.Text = Catalog.GetString("Amount:");
            this.label8.Text = Catalog.GetString("Key Ministry:");
            this.button5.Text = Catalog.GetString("Field");
            this.ttxtPartnerKeyTextBox3.LabelText = Catalog.GetString("Name of Field");
            this.button4.Text = Catalog.GetString("Remove Detail");
            this.button3.Text = Catalog.GetString("Add Gift Detail");
            this.label7.Text = Catalog.GetString("Total:");
            this.label6.Text = Catalog.GetString("Gift Date:");
            this.button2.Text = Catalog.GetString("Recipient");
            this.ttxtPartnerKeyTextBox2.LabelText = Catalog.GetString("Name of Recipient");
            this.xpToolBarComboBox1.ComboText = Catalog.GetString("Select Statement");
            this.xpToolBarComboBox1.Text = Catalog.GetString("13-June-2008");
            this.button9.Text = Catalog.GetString("Save");
            this.button10.Text = Catalog.GetString("Cancel");
            this.button11.Text = Catalog.GetString("Cancel");
            this.button12.Text = Catalog.GetString("Save");
            this.button13.Text = Catalog.GetString("Cancel");
            this.button14.Text = Catalog.GetString("Save");
            this.button15.Text = Catalog.GetString("Cancel");
            this.button16.Text = Catalog.GetString("Save");
            this.xpToolBarButton3.Text = Catalog.GetString("Add new Bank Account");
            this.xpToolBarComboBox2.ComboText = Catalog.GetString("Select Bank Account");
            this.Text = Catalog.GetString("Match recurring gifts");
            #endregion

            DataTable statementTable = new DataTable();
            statementTable.Columns.Add(new DataColumn("Amount", typeof(double)));
            statementTable.Columns.Add(new DataColumn("TransactionTypeCode", typeof(String)));
            statementTable.Columns.Add(new DataColumn("TransactionTypeDescr", typeof(String)));
            statementTable.Columns.Add(new DataColumn("Name", typeof(String)));
            statementTable.Columns.Add(new DataColumn("Purpose", typeof(String)));

            DataRow row = statementTable.NewRow();
            row["Amount"] = 50.0f;
            row["TransactionTypeCode"] = "051";
            row["TransactionTypeDescr"] = "Gutschrift, �berweisung";
            row["Name"] = "Donald Mustermann";
            row["Purpose"] = "Support some project";
            statementTable.Rows.Add(row);
            row = statementTable.NewRow();
            row["Amount"] = 30.0f;
            row["TransactionTypeCode"] = "051";
            row["TransactionTypeDescr"] = "Gutschrift, �berweisung";
            row["Name"] = "SomeOne Else";
            row["Purpose"] = "Another project";
            statementTable.Rows.Add(row);

            tsgrdDataGrid1.Columns.Clear();
            tsgrdDataGrid1.AddTextColumn("Amount", statementTable.Columns["Amount"]);
            tsgrdDataGrid1.AddTextColumn("Type", statementTable.Columns["TransactionTypeCode"]);
            tsgrdDataGrid1.AddTextColumn("Type", statementTable.Columns["TransactionTypeDescr"]);
            tsgrdDataGrid1.AddTextColumn("Name", statementTable.Columns["Name"]);
            tsgrdDataGrid1.AddTextColumn("Purpose", statementTable.Columns["Purpose"]);
            tsgrdDataGrid1.DataSource = new DevAge.ComponentModel.BoundDataView(new DataView(statementTable));
            ((DataView)tsgrdDataGrid1.DataSource).AllowEdit = false;
            ((DataView)tsgrdDataGrid1.DataSource).AllowNew = false;
            ((DataView)tsgrdDataGrid1.DataSource).AllowDelete = false;
        }
    }
}
