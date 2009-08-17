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
using Mono.Unix;
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLBatches
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;

            // TODO: more criteria: state of batch, period, etc
            FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(ALedgerNumber));

            ShowData();
        }

        private void ShowDetailsManual(Int32 ACurrentDetailIndex)
        {
            ((TFrmGLBatch)ParentForm).LoadJournals(
                FMainDS.ABatch[ACurrentDetailIndex].LedgerNumber,
                FMainDS.ABatch[ACurrentDetailIndex].BatchNumber);
        }

        private void ShowJournalTab(Object sender, EventArgs e)
        {
            ((TFrmGLBatch)ParentForm).SelectTab(TFrmGLBatch.eGLTabs.Journals);
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewABatch();

            // TODO: this.dtpDateCantBeBeyond.Value = AAccountingPeriod[ALedger.CurrentPeriod + ALedger.ForwardingPostingPeriods].EndOfPeriod

            // TODO: on change of FMainDS.ABatch[GetSelectedDetailDataTableIndex()].DateEffective
            // also change FMainDS.ABatch[GetSelectedDetailDataTableIndex()].BatchPeriod
            // and control this.dtpDateCantBeBeyond
        }

        /// <summary>
        /// cancel a batch (there is no deletion of batches)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CancelRow(System.Object sender, EventArgs e)
        {
            // TODO
        }

        /// <summary>
        /// don't allow changing the batch if there are changes
        /// also don't allow reloading the current batch because the journals might be overwritten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeavingRow(System.Object sender, SourceGrid.RowCancelEventArgs e)
        {
            if (FPetraUtilsObject.HasChanges)
            {
                if (e.Row != FPreviouslySelectedDetailRow)
                {
//                    System.Windows.Forms.MessageBox.Show(Catalog.GetString("Please first save the current batch, before you can work on other batches!"));
                }

                // TODO: alternatively: open another window with the new batch? or don't reload from db when going back to a batch that has already been opened?
//                e.Cancel = true;
            }
        }
    }
}