//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmPostcodeRangeSetup
    {
        private void NewRowManual(ref PPostcodeRangeRow ARow)
        {
            string NewName = Catalog.GetString("NEWRANGE");
            int CountNewDetail = 0;

            if (FMainDS.PPostcodeRange.Rows.Find(new object[] { NewName }) != null)
            {
                while (FMainDS.PPostcodeRange.Rows.Find(new object[] { NewName + CountNewDetail.ToString() }) != null)
                {
                    CountNewDetail++;
                }

                NewName += CountNewDetail.ToString();
            }

            ARow.Range = NewName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPPostcodeRange();

            btnAccept.Enabled = true;
        }

        /// <summary>
        /// Returns all the rows selected in the grid.
        /// </summary>
        public DataRowView[] GetSelectedRows()
        {
            return grdDetails.SelectedDataRowsAsDataRowView;
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
        ///
        /// </summary>
        /// <param name="ARegionName">Pass in the selected Region's name.
        /// </param>
        /// <returns>void</returns>
        public void SetParameters(String ARegionName)
        {
            txtRegionName.Text = ARegionName;

            pnlRegionName.Visible = true;
            pnlAcceptCancelButtons.Visible = true;

            if (grdDetails.Rows.Count < 2)
            {
                btnAccept.Enabled = false;
            }
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TStandardFormPrint.PrintGrid(APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[] { 0, 1, 2 },
                new int[]
                {
                    PPostcodeRangeTable.ColumnRangeId,
                    PPostcodeRangeTable.ColumnFromId,
                    PPostcodeRangeTable.ColumnToId
                });
        }
    }

    /// <summary>
    /// Manages the opening of a new/showing of an existing Instance of the Postcode Ranges Find Screen.
    /// </summary>
    public static class TPostcodeRangeSetupManager
    {
        /// <summary>
        /// Opens a Modal instance of the Partner Find screen.
        /// </summary>
        /// <param name="ARegionName">Pass in the selected Region's name.</param>
        /// <param name="ARangeName">Name of the found Range.</param>
        /// <param name="AFrom">'From' field of the found Partner.</param>
        /// <param name="ATo">'To' field of the found Partner.</param>
        /// <param name="AParentForm"></param>
        /// <returns>True if a Partner was found and accepted by the user,
        /// otherwise false.</returns>
        public static bool OpenModalForm(String ARegionName,
            out String[] ARangeName,
            out String[] AFrom,
            out String[] ATo,
            Form AParentForm)
        {
            DialogResult dlgResult;

            ARangeName = null;
            AFrom = null;
            ATo = null;

            TFrmPostcodeRangeSetup SelectRange = new TFrmPostcodeRangeSetup(AParentForm);
            SelectRange.SetParameters(ARegionName);

            dlgResult = SelectRange.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                DataRowView[] HighlightedRows = SelectRange.GetSelectedRows();
                int NumberOfRows = HighlightedRows.Length;
                PPostcodeRangeTable CachedRangeTable = (PPostcodeRangeTable)TDataCache.GetCacheableDataTableFromCache("PostcodeRangeList");
                bool NoRangesSelected = true;

                ARangeName = new string[NumberOfRows];
                AFrom = new string[NumberOfRows];
                ATo = new string[NumberOfRows];

                for (int i = 0; i < NumberOfRows; i++)
                {
                    if (CachedRangeTable.Rows.Find(new object[] { ((PPostcodeRangeRow)HighlightedRows[i].Row).Range }) != null)
                    {
                        ARangeName[i] = ((PPostcodeRangeRow)HighlightedRows[i].Row).Range;
                        AFrom[i] = ((PPostcodeRangeRow)HighlightedRows[i].Row).From;
                        ATo[i] = ((PPostcodeRangeRow)HighlightedRows[i].Row).To;

                        NoRangesSelected = false;
                    }
                    else
                    {
                        ARangeName[i] = null;
                        AFrom[i] = null;
                        ATo[i] = null;
                    }
                }

                if (NoRangesSelected)
                {
                    MessageBox.Show(String.Format("No valid ranges have been selected."), String.Format("Add Range"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return true;
            }

            return false;
        }
    }
}