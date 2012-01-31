//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MPartner.Extracts.WebConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Gui;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    public partial class TUC_ExtractMasterList
    {
        #region Public Methods

        /// <summary>
        /// needed for the interface
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            bool ReturnValue = true;

            return ReturnValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteRow(System.Object sender, EventArgs e)
        {
        	
            if (MessageBox.Show(String.Format(Catalog.GetString(
                            "You have choosen to delete this value ({0}).\n\nDo you really want to delete it?"),
                        FPreviouslySelectedDetailRow.ExtractName), Catalog.GetString("Confirm Delete"),
                    MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int rowIndex = CurrentRowIndex();

                // delete extract on the server (needs cascading delete)
                TRemote.MPartner.Partner.WebConnectors.DeleteExtract(FPreviouslySelectedDetailRow.ExtractId);
                
                // now delete it in the grid
                FPreviouslySelectedDetailRow.Delete();

                
                SelectByIndex(rowIndex);

                if (grdDetails.Rows.Count <= 1)
                {
                    // disable buttons if no record in grid (first row for headings)
                    btnDelete.Enabled = false;
                }
            }
        }
        
        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
            FMainDS = new ExtractTDS();
            LoadData();
        }

        /// <summary>
        /// Loads Partner Types Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadData()
        {
            Boolean ReturnValue;

            // Load Extract Headers, if not already loaded
            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.MExtractMaster == null)
                {
                    FMainDS.Tables.Add(new MExtractMasterTable());
                    FMainDS.InitVars();
                }

                FMainDS.Merge(TRemote.MPartner.Partner.WebConnectors.GetAllExtractHeaders());

                // Make DataRows unchanged
                if (FMainDS.MExtractMaster.Rows.Count > 0)
                {
                    FMainDS.MExtractMaster.AcceptChanges();
                }

                if (FMainDS.MExtractMaster.Rows.Count != 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            return ReturnValue;
        }

        /// <summary>
        ///
        /// </summary>
        private void ShowDataManual()
        {
        }

        private int CurrentRowIndex()
        {
            int rowIndex = -1;

            SourceGrid.RangeRegion selectedRegion = grdDetails.Selection.GetSelectionRegion();

            if ((selectedRegion != null) && (selectedRegion.GetRowsIndex().Length > 0))
            {
                rowIndex = selectedRegion.GetRowsIndex()[0];
            }

            return rowIndex;
        }

        private void SelectByIndex(int rowIndex)
        {
            if (rowIndex >= grdDetails.Rows.Count)
            {
                rowIndex = grdDetails.Rows.Count - 1;
            }

            if ((rowIndex < 1) && (grdDetails.Rows.Count > 1))
            {
                rowIndex = 1;
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);
            }
            else
            {
                FPreviouslySelectedDetailRow = null;
            }
        }
        
        #endregion
    }
}