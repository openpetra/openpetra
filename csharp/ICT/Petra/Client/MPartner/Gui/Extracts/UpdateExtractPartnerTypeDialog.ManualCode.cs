//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2012 by OM International
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
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    /// manual methods for the generated window
    public partial class TFrmUpdateExtractPartnerTypeDialog : System.Windows.Forms.Form
    {
        /// <summary>
        /// if true then use dialog to add partner type, if false use it for delete
        /// </summary>
        Boolean FAdd;

        /// <summary>
        /// set extract name that action is taken for
        /// </summary>
        String FExtractName;

        /// <summary>
        /// type code selected before dialog was closed with OK button or double click
        /// </summary>
        String FTypeCode;

        PTypeTable FTypeTable;

        /// <summary>
        /// set the initial value for passport name in the dialog
        /// </summary>
        /// <param name="AExtractName"></param>
        public void SetExtractName(String AExtractName)
        {
            FExtractName = AExtractName;
            lblExtractNameAndCreator.Text = Catalog.GetString("Extract Name: ") + AExtractName;
            lblExtractNameAndCreator.Font = new System.Drawing.Font(lblExtractNameAndCreator.Font.FontFamily.Name, 10, System.Drawing.FontStyle.Bold);
        }

        /// <summary>
        /// sets the mode for dialog (add or delete)
        /// </summary>
        /// <param name="AAdd"></param>
        public void SetMode(Boolean AAdd)
        {
            FAdd = AAdd;

            if (FAdd)
            {
                lblExplanation.Text = Catalog.GetString("Please select Special Type you want to add to all Partners in this Extract:");
                FindForm().Text = Catalog.GetString("Add Partner Special Type");
            }
            else
            {
                lblExplanation.Text = Catalog.GetString("Please select Special Type you want to delete from all Partners in this Extract:");
                FindForm().Text = Catalog.GetString("Delete Partner Special Type");
            }
        }

        private void InitializeManualCode()
        {
            // show this dialog in center of screen
            this.StartPosition = FormStartPosition.CenterScreen;

            FTypeTable = (PTypeTable)TDataCache.TMPartner.GetCacheablePartnerTable(
                TCacheablePartnerTablesEnum.PartnerTypeList);

            for (int i = 0; i < FTypeTable.Rows.Count; i++)
            {
                PTypeRow row = (PTypeRow)FTypeTable.Rows[i];

                if (!row.ValidType)
                {
                    row.TypeDescription += MCommonResourcestrings.StrGenericInactiveCode;
                }
            }

            FTypeTable.AcceptChanges();

            grdTypes.AddTextColumn(Catalog.GetString("Type Code"), FTypeTable.Columns[PTypeTable.GetTypeCodeDBName()]);
            grdTypes.AddTextColumn(Catalog.GetString("Description"), FTypeTable.Columns[PTypeTable.GetTypeDescriptionDBName()]);

            DataView myDataView = FTypeTable.DefaultView;
            myDataView.AllowNew = false;
            grdTypes.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
        }

        private void CustomClosingHandler(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CanClose())
            {
                e.Cancel = true;
            }
            else
            {
                // Needs to be set to false because it got set to true in ancestor Form!
                e.Cancel = false;

                // Need to call the following method in the Base Form to remove this Form from the Open Forms List
                FPetraUtilsObject.TFrmPetra_Closing(this, null);
            }
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the values of Fields
        /// on the screen.
        /// </summary>
        /// <param name="ATypeCode"></param>
        /// <returns>Boolean</returns>
        public Boolean GetReturnedParameters(out String ATypeCode)
        {
            Boolean ReturnValue = true;

            ATypeCode = FTypeCode;

            return ReturnValue;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            String Message;
            String Title;

            // find selected type code
            DataRowView[] SelectedGridRow = grdTypes.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length < 1)
            {
                return;
            }

            PTypeRow row = (PTypeRow)SelectedGridRow[0].Row;
            FTypeCode = row.TypeCode;

            if (FAdd)
            {
                Title = Catalog.GetString("Add Partner Special Type");

                if (!row.ValidType)
                {
                    Message = string.Format(Catalog.GetString("The code '{0}' is no longer active.  Are you sure you want to use it?"), FTypeCode);

                    if (MessageBox.Show(Message, Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                }

                Message = String.Format(Catalog.GetString("Are you sure that you want to add special type '{0}' " +
                        "for all partners in extract '{1}'?"), FTypeCode, FExtractName);
            }
            else
            {
                Message = String.Format(Catalog.GetString("Are you sure that you want to delete special type '{0}' " +
                        "from all partners in extract '{1}'?"), FTypeCode, FExtractName);
                Title = Catalog.GetString("Delete Partner Special Type");
            }

            if (MessageBox.Show(Message,
                    Title,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void RunOnceOnActivationManual()
        {
            grdTypes.AutoResizeGrid();
        }
    }
}