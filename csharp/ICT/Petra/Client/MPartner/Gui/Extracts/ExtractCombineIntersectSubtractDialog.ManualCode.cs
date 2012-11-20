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
using System.Collections.Generic;
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
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    /// manual methods for the generated window
    public partial class TFrmExtractCombineIntersectSubtractDialog : System.Windows.Forms.Form
    {
        /// <summary>Mode to be used for this dialog</summary>
        public enum TMode
        {
            /// <summary>show dialog for combining extract</summary>
            ecisCombineMode,

            /// <summary>show dialog for intersecting extract</summary>
            ecisIntersectMode,

            /// <summary>show dialog for subtracting extract</summary>
            ecisSubtractMode
        }

        /// <summary>
        /// enum to remember which mode to be used for dialog
        /// </summary>
        private TMode FMode;

        /// <summary>
        /// return true if dialog action (combine or intersect) was successful
        /// </summary>
        private MExtractMasterTable FExtractMasterTable;

        /// <summary>
        /// set dialog mode (combine, intersect or subtract)
        /// </summary>
        /// <param name="AMode"></param>
        public void SetMode(TFrmExtractCombineIntersectSubtractDialog.TMode AMode)
        {
            FMode = AMode;

            // hide field for base extract if not in subtraction mode
            if (FMode != TMode.ecisSubtractMode)
            {
                int ReducedHeight = txtBaseExtract.Height;
                txtBaseExtract.Height = 0;
                lblBaseExtract.Height = 0;
                txtBaseExtract.Hide();
                lblBaseExtract.Hide();

                pnlTop.Height = pnlTop.Height - ReducedHeight;
                lblExplanation.Location = new System.Drawing.Point(lblExplanation.Location.X, lblExplanation.Location.Y - ReducedHeight);
            }

            switch (FMode)
            {
                case TMode.ecisCombineMode:
                    lblExplanation.Text = Catalog.GetString("Please add extracts to the list that you want to combine and then click OK:");
                    FindForm().Text = "Combine Extracts";
                    break;

                case TMode.ecisIntersectMode:
                    lblExplanation.Text = Catalog.GetString("Please add extracts to the list that you want to intersect and then click OK:");
                    FindForm().Text = "Intersect Extracts";
                    break;

                case TMode.ecisSubtractMode:
                    lblExplanation.Text = Catalog.GetString("Please add extracts to the list to be subtracted from the one above:");
                    FindForm().Text = "Subtract Extracts";
                    break;
            }
        }

        private void InitializeManualCode()
        {
            // show this dialog in center of screen
            this.StartPosition = FormStartPosition.CenterScreen;

            // enable grid to react to insert and delete keyboard keys
            grdExtracts.InsertKeyPressed += new TKeyPressedEventHandler(grdExtracts_InsertKeyPressed);
            grdExtracts.DeleteKeyPressed += new TKeyPressedEventHandler(grdExtracts_DeleteKeyPressed);

            FExtractMasterTable = new MExtractMasterTable();

            grdExtracts.Columns.Clear();

            grdExtracts.AddTextColumn("Extract Name", FExtractMasterTable.ColumnExtractName);
            grdExtracts.AddTextColumn("Created By", FExtractMasterTable.ColumnCreatedBy);
            grdExtracts.AddTextColumn("Key Count", FExtractMasterTable.ColumnKeyCount);
            grdExtracts.AddTextColumn("Description", FExtractMasterTable.ColumnExtractDesc);
            grdExtracts.AddDateColumn("Created Date", FExtractMasterTable.ColumnDateCreated);

            DataView myDataView = FExtractMasterTable.DefaultView;
            myDataView.AllowNew = false;
            grdExtracts.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            btnRemove.Enabled = false;
        }

        /// <summary>
        /// Open Dialog to find extracts to be added list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddExtractToList(System.Object sender, EventArgs e)
        {
            TFrmExtractFind ExtractFindDialog = new TFrmExtractFind(this.ParentForm);
            MExtractMasterTable ExtractMasterTable = new MExtractMasterTable();
            MExtractMasterRow NewRow;

            // let the user select base extract
            ExtractFindDialog.AllowMultipleSelect = true;
            ExtractFindDialog.ShowDialog(true);

            // get data for selected base extract
            ExtractFindDialog.GetResult(ref ExtractMasterTable);
            ExtractFindDialog.Dispose();

            // only continue if an extract was selected
            foreach (MExtractMasterRow Row in ExtractMasterTable.Rows)
            {
                if (!FExtractMasterTable.Rows.Contains(new object[] { Row.ExtractId }))
                {
                    NewRow = FExtractMasterTable.NewRowTyped();
                    NewRow.ExtractId = Row.ExtractId;
                    NewRow.ExtractName = Row.ExtractName;
                    NewRow.ExtractDesc = Row.ExtractDesc;
                    NewRow.KeyCount = Row.KeyCount;
                    NewRow.CreatedBy = Row.CreatedBy;
                    NewRow.DateCreated = Row.DateCreated;

                    FExtractMasterTable.Rows.Add(NewRow);

                    btnRemove.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Remove selected extract from list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveExtractFromList(System.Object sender, EventArgs e)
        {
            MExtractMasterRow RowToDelete;

            if (grdExtracts.SelectedRowIndex() < 0)
            {
                MessageBox.Show(Catalog.GetString("You need to select an Extract to be removed from the list"),
                    Catalog.GetString("Remove from List"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if ((MessageBox.Show(Catalog.GetString("Do you want to remove the selected Extract from this list?"),
                     Catalog.GetString("Confirm Remove"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
            {
                int nSelectedRow = grdExtracts.SelectedRowIndex();
                RowToDelete = (MExtractMasterRow)((DataRowView)grdExtracts.SelectedDataRows[0]).Row;
                RowToDelete.Delete();
                grdExtracts.SelectRowInGrid(nSelectedRow, true);

                // enable/disable "remove button" depending on number of rows in list
                if (FExtractMasterTable.Rows.Count == 0)
                {
                    btnRemove.Enabled = false;
                }
                else
                {
                    btnRemove.Enabled = true;
                }
            }
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
        /// <param name="AExtractIdList"></param>
        /// <returns>Boolean</returns>
        public Boolean GetReturnedParameters(out List <Int32>AExtractIdList)
        {
            Boolean ReturnValue = true;

            AExtractIdList = new List <Int32>();

            foreach (DataRow ExtractMasterRow in FExtractMasterTable.Rows)
            {
                AExtractIdList.Add(((MExtractMasterRow)ExtractMasterRow).ExtractId);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the values of Fields
        /// on the screen.
        /// </summary>
        /// <param name="ABaseExtractName"></param>
        /// <param name="AExtractIdList"></param>
        /// <returns>Boolean</returns>
        public Boolean GetReturnedParameters(out String ABaseExtractName, out List <Int32>AExtractIdList)
        {
            Boolean ReturnValue = true;

            GetReturnedParameters(out AExtractIdList);

            ABaseExtractName = txtBaseExtract.Text;

            return ReturnValue;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            String MessageText = "";
            String TitleText = "";

            switch (FMode)
            {
                case TMode.ecisCombineMode:
                    TitleText = Catalog.GetString("Combine Extracts");
                    break;

                case TMode.ecisIntersectMode:
                    TitleText = Catalog.GetString("Intersect Extracts");
                    break;

                case TMode.ecisSubtractMode:
                    TitleText = Catalog.GetString("Subtract Extracts");
                    break;
            }

            // check if user tries to subtract extract from itself
            if (FMode == TMode.ecisSubtractMode)
            {
                foreach (DataRow ExtractRow in FExtractMasterTable.Rows)
                {
                    if (((MExtractMasterRow)ExtractRow).ExtractName == txtBaseExtract.Text)
                    {
                        MessageBox.Show(Catalog.GetString("You cannot subtract an extract from itself"),
                            TitleText,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            if (FExtractMasterTable.Rows.Count > 0)
            {
                switch (FMode)
                {
                    case TMode.ecisCombineMode:
                        MessageText = Catalog.GetString("Are you sure that you want to combine the extracts in the list?");
                        break;

                    case TMode.ecisIntersectMode:
                        MessageText = Catalog.GetString("Are you sure that you want to intersect the extracts in the list?");
                        break;

                    case TMode.ecisSubtractMode:
                        MessageText = Catalog.GetString("Are you sure that you want to subtract the extracts in the list from the one at the top?");
                        break;
                }

                if (MessageBox.Show(MessageText,
                        TitleText,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                // list is empty: nothing to be done
                MessageText = Catalog.GetString("You have not added any extracts to the list!");

                MessageBox.Show(MessageText,
                    TitleText,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdExtracts_InsertKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            this.AddExtractToList(Sender, null);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdExtracts_DeleteKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            this.RemoveExtractFromList(Sender, null);
        }
    }
}