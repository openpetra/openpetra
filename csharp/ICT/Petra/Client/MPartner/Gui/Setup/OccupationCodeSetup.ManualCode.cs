//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Client.CommonDialogs;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmOccupationCodeSetup
    {
        private void NewRowManual(ref POccupationRow ARow)
        {
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.POccupation.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.POccupation.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.OccupationCode = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPOccupation();
        }

        private void BtnAccept_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// </summary>
        /// <returns>void</returns>
        public void SetParameters(String AOccupationCode)
        {
            // find the location of the currently selected OccupationCode (if one exists)
            if (!string.IsNullOrEmpty(AOccupationCode))
            {
                int RowPos = 1;

                foreach (DataRowView rowView in FMainDS.POccupation.DefaultView)
                {
                    POccupationRow Row = (POccupationRow)rowView.Row;

                    if (Row.OccupationCode == AOccupationCode)
                    {
                        break;
                    }

                    RowPos++;
                }

                // automatically select the current conference
                grdDetails.SelectRowInGrid(RowPos, true);
            }

            pnlAcceptCancelButtons.Visible = true;

            if (grdDetails.Rows.Count < 2)
            {
                btnAccept.Enabled = false;
            }

            // only one record can be selected
            grdDetails.Selection.EnableMultiSelection = false;
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TFrmSelectPrintFields.SelectAndPrintGridFields(this, APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[]
                {
                    POccupationTable.ColumnOccupationCodeId,
                    POccupationTable.ColumnOccupationDescriptionId,
                    POccupationTable.ColumnValidOccupationId,
                    POccupationTable.ColumnDeletableId
                });
        }
    }

    /// <summary>
    /// Manages the opening of a new/showing of an existing Instance of the Occupation Code Setup Screen.
    /// </summary>
    public static class TOccupationCodeSetupManager
    {
        /// <summary>
        /// Opens a Modal instance of the Occupation Code Setup Screen.
        /// </summary>
        /// <param name="AOccupationCode">Pass in the selected Occupation's code.</param>
        /// <param name="AParentForm"></param>
        /// <returns>True if an Occupation was found and accepted by the user, otherwise false.</returns>
        public static bool OpenModalForm(ref String AOccupationCode,
            Form AParentForm)
        {
            DialogResult dlgResult;

            TFrmOccupationCodeSetup SelectOccupation = new TFrmOccupationCodeSetup(AParentForm);

            SelectOccupation.SetParameters(AOccupationCode);

            dlgResult = SelectOccupation.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                POccupationRow SelectedRow = SelectOccupation.GetSelectedDetailRow();

                if (SelectedRow == null)
                {
                    MessageBox.Show(String.Format("No valid Occupation Code has been selected."), String.Format("Find Occupation Code"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    AOccupationCode = SelectOccupation.GetSelectedDetailRow().OccupationCode;
                }

                return true;
            }

            return false;
        }
    }
}