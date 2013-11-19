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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    /// <summary>
    /// manual code for TFrmPersonalDocumentExpiryReport class
    /// </summary>
    public partial class TFrmPersonalDocumentExpiryReport
    {
        private PmDocumentTypeTable FDocumentTypeTable;

        private void InitializeManualCode()
        {
            ucoPartnerSelection.SetRestrictedPartnerClasses("PERSON");
        }

        private void grdDocuments_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            // Get list of documents
            FDocumentTypeTable = (PmDocumentTypeTable)TDataCache.TMPersonnel.GetCacheablePersonnelTable(
                TCacheablePersonTablesEnum.DocumentTypeList);

            grdDocuments.Columns.Clear();

            grdDocuments.AddTextColumn("Document Type", FDocumentTypeTable.Columns[PmDocumentTypeTable.GetDocCodeDBName()]);
            grdDocuments.AddTextColumn("Description", FDocumentTypeTable.Columns[PmDocumentTypeTable.GetDescriptionDBName()]);

            FDocumentTypeTable.DefaultView.AllowNew = false;
            FDocumentTypeTable.DefaultView.AllowDelete = false;

            grdDocuments.DataSource = new DevAge.ComponentModel.BoundDataView(FDocumentTypeTable.DefaultView);
            grdDocuments.AutoSizeCells();
            grdDocuments.Selection.EnableMultiSelection = true;

            UseDatesChanged(null, null);
        }

        private void grdDocuments_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            String DocumentTypeList = "";

            for (int Counter = 0; Counter < grdDocuments.SelectedDataRows.Length; ++Counter)
            {
                DocumentTypeList = DocumentTypeList +
                                   (String)((DataRowView)grdDocuments.SelectedDataRows[Counter]).Row[FDocumentTypeTable.Columns[PmDocumentTypeTable.
                                                                                                                                GetDocCodeDBName()]]
                                   +
                                   ",";
            }

            // remove the last ,
            if (DocumentTypeList.Length > 0)
            {
                DocumentTypeList = DocumentTypeList.Substring(0, DocumentTypeList.Length - 1);
            }
            else if (AReportAction == TReportActionEnum.raGenerate)
            {
                // at least one document type must be checked
                TVerificationResult VerificationResult = new TVerificationResult("Select at least one document type.",
                    "No document type selected!",
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            ACalc.AddParameter("param_doctype", DocumentTypeList);
        }

        private void grdDocuments_SetControls(TParameterList AParameters)
        {
            String SelectedTypes = AParameters.Get("param_doctype").ToString() + ",";

            grdDocuments.Selection.ResetSelection(false);

            for (int Counter = 0; Counter <= FDocumentTypeTable.Rows.Count; ++Counter)
            {
                DataRowView Row = (DataRowView)grdDocuments.Rows.IndexToDataSourceRow(Counter);

                if (Row != null)
                {
                    String CurrentType = (String)Row[0];

                    grdDocuments.Selection.SelectRow(Counter, SelectedTypes.Contains((CurrentType + ",")));
                }
            }
        }

        private void UseDatesChanged(System.Object sender, EventArgs e)
        {
            dtpFromDate.Enabled = chkUseDates.Checked;
            dtpToDate.Enabled = chkUseDates.Checked;
        }
    }
}