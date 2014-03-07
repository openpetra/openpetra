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
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Client.MPersonnel.Gui
{
    public partial class TFrmApplicationStatusDialog
    {
        private static PtApplicantStatusTable FApplicantStatusTable;
        private delegate void CheckChangedArgs (int ChangedRow);
        private event CheckChangedArgs ChangedRowEvent;
        private CustomValueChangedEvent FGridValueChangedEvent;

        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitUserControlsManually()
        {
            // retrieve data from cache
            FApplicantStatusTable = (PtApplicantStatusTable)TDataCache.TMPersonnel.GetCacheablePersonnelTable(
                TCacheablePersonTablesEnum.ApplicantStatusList);
            FApplicantStatusTable.Columns.Add("Is Selected", Type.GetType("System.Boolean"));

            for (int Counter = 0; Counter < FApplicantStatusTable.Rows.Count; ++Counter)
            {
                FApplicantStatusTable.Rows[Counter]["Is Selected"] = false;
            }

            // use data to set up the grid
            SetUpGrid();
        }

        private void SetUpGrid()
        {
            grdApplicationStatuses.AddCheckBoxColumn("", FApplicantStatusTable.Columns["Is Selected"], 17, false);
            grdApplicationStatuses.AddTextColumn(Catalog.GetString("Applicant Status Code"), FApplicantStatusTable.ColumnCode);
            grdApplicationStatuses.AddTextColumn(Catalog.GetString("Description"), FApplicantStatusTable.ColumnDescription);

            FApplicantStatusTable.DefaultView.AllowNew = false;
            FApplicantStatusTable.DefaultView.AllowEdit = true;
            FApplicantStatusTable.DefaultView.AllowDelete = false;

            grdApplicationStatuses.DataSource = new DevAge.ComponentModel.BoundDataView(FApplicantStatusTable.DefaultView);

            // check boxes by mouse click anywhere on a row, spacebar or enter key
            grdApplicationStatuses.MouseClick += new MouseEventHandler(this.GrdApplicationStatuses_Click);
            grdApplicationStatuses.SpaceKeyPressed += new TKeyPressedEventHandler(this.GrdApplicationStatuses_SpaceKeyPressed);
            grdApplicationStatuses.EnterKeyPressed += new TKeyPressedEventHandler(this.GrdApplicationStatuses_EnterKeyPressed);

            // This is neccessary to counteract the grid automatically checking a box when the user clicks on the checkbox column.
            // We do not want this to happen as the box is also checked by the MouseEventHandler.
            FGridValueChangedEvent = new CustomValueChangedEvent(this);
            grdApplicationStatuses.Controller.AddController(FGridValueChangedEvent);
            ChangedRowEvent += new CheckChangedArgs(ChangeCheckedStateForRow);

            grdApplicationStatuses.AutoSizeCells();
        }

        private void AcceptSelection(System.Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelClick(System.Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ChangeCheckedStateForRow(Int32 ARow)
        {
            if (ARow >= 0)
            {
                FApplicantStatusTable[ARow]["Is Selected"] = (System.Object)((!(Boolean)(FApplicantStatusTable[ARow]["Is Selected"])));
            }
        }

        // checks the statuses that have previously been selected
        private void SetCurrentlySelectedStatuses(string ACurrentApplicationStatusList)
        {
            string[] Statuses = ACurrentApplicationStatusList.Split(',');

            foreach (string Status in Statuses)
            {
                foreach (DataRow Row in FApplicantStatusTable.Rows)
                {
                    if ((string)Row[FApplicantStatusTable.ColumnCode] == Status)
                    {
                        Row["Is Selected"] = true;
                    }
                }
            }
        }

        private void SelectStatus(Boolean AValue)
        {
            foreach (DataRow Row in FApplicantStatusTable.Rows)
            {
                Row["Is Selected"] = AValue;
            }
        }

        private void DeselectAll(System.Object sender, EventArgs e)
        {
            SelectStatus(false);
        }

        private void GrdApplicationStatuses_Click(System.Object Sender, System.EventArgs e)
        {
            ChangeCheckedStateForRow(grdApplicationStatuses.MouseCellPosition.Row - 1);
        }

        private void GrdApplicationStatuses_SpaceKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            ChangeCheckedStateForRow(e.Row);
        }

        private void GrdApplicationStatuses_EnterKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            ChangeCheckedStateForRow(e.Row);
        }

        private class CustomValueChangedEvent : SourceGrid.Cells.Controllers.ControllerBase
        {
            TFrmApplicationStatusDialog FParentClass;

            public CustomValueChangedEvent(TFrmApplicationStatusDialog AParentClass)
            {
                FParentClass = AParentClass;
            }

            public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
            {
                base.OnValueChanged(sender, e);

                FParentClass.ChangedRowEvent(sender.Position.Row - 1);
            }
        }

        /// <summary>
        /// Returns the selected application statuses
        /// </summary>
        /// <param name="AApplicationStatusList">List with all selected application statuses seperated by commas</param>
        public static void GetSelectedApplicationStatuses(ref string AApplicationStatusList)
        {
            foreach (DataRow Row in FApplicantStatusTable.Rows)
            {
                if ((bool)Row["Is Selected"])
                {
                    String Status = (String)Row[PtApplicantStatusTable.GetCodeDBName()];

                    if (AApplicationStatusList.Length > 0)
                    {
                        AApplicationStatusList += ",";
                    }

                    AApplicationStatusList += Status;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ACurrentApplicationStatusList">List with all currently selected application statuses seperated by commas</param>
        /// <param name="AOwner">The parent form</param>
        /// <param name="AApplicationStatusList">List with all selected application statuses seperated by commas</param>
        /// <returns></returns>
        public static DialogResult OpenApplicationStatusDialog(string ACurrentApplicationStatusList, Form AOwner, out string AApplicationStatusList)
        {
            TFrmApplicationStatusDialog ApplicationStatusDialog;
            DialogResult DlgResult = DialogResult.Cancel;

            AApplicationStatusList = "";

            ApplicationStatusDialog = new TFrmApplicationStatusDialog(AOwner);
            ApplicationStatusDialog.SetCurrentlySelectedStatuses(ACurrentApplicationStatusList);
            DlgResult = ApplicationStatusDialog.ShowDialog(AOwner);

            if (DlgResult == DialogResult.OK)
            {
                GetSelectedApplicationStatuses(ref AApplicationStatusList);
            }

            return DlgResult;
        }
    }
}