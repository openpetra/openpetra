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

            clbDocuments_InitialiseData();
        }

        private void clbDocuments_InitialiseData()
        {
            string CheckedMember = "CHECKED";

            // Get list of documents
            FDocumentTypeTable = (PmDocumentTypeTable)TDataCache.TMPersonnel.GetCacheablePersonnelTable(
                TCacheablePersonTablesEnum.DocumentTypeList);

            DataColumn FirstColumn = new DataColumn(CheckedMember, typeof(bool));
            FirstColumn.DefaultValue = false;
            FDocumentTypeTable.Columns.Add(FirstColumn);

            clbDocuments.Columns.Clear();
            clbDocuments.AddCheckBoxColumn("", FDocumentTypeTable.Columns[CheckedMember], 17, false);
            clbDocuments.AddTextColumn("Document Type", FDocumentTypeTable.Columns[PmDocumentTypeTable.GetDocCodeDBName()]);
            clbDocuments.AddTextColumn("Description", FDocumentTypeTable.Columns[PmDocumentTypeTable.GetDescriptionDBName()]);

            clbDocuments.DataBindGrid(FDocumentTypeTable, PmDocumentTypeTable.GetDocCodeDBName(), CheckedMember,
                PmDocumentTypeTable.GetDocCodeDBName(), false, true, false);
            clbDocuments.AutoResizeGrid();

            UseDatesChanged(null, null);
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            String paramDocuments = clbDocuments.GetCheckedStringList(true);

            if (AReportAction == TReportActionEnum.raGenerate)
            {
                if (paramDocuments.Length == 0)
                {
                    TVerificationResult VerificationMessage = new TVerificationResult(
                        Catalog.GetString("Please select at least one document type."),
                        Catalog.GetString("No document types selected!"), TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationMessage);
                }
            }

            paramDocuments = paramDocuments.Replace("\"", "");
            ACalc.AddParameter("param_doctype", paramDocuments);
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            clbDocuments.SetCheckedStringList(AParameters.Get("param_doctype").ToString());
        }

        private void UseDatesChanged(System.Object sender, EventArgs e)
        {
            dtpFromDate.Enabled = chkUseDates.Checked;
            dtpToDate.Enabled = chkUseDates.Checked;
        }

        private void SelectAllDocuments(object sender, EventArgs e)
        {
            for (int Counter = 0; Counter < FDocumentTypeTable.Rows.Count; ++Counter)
            {
                FDocumentTypeTable.Rows[Counter]["CHECKED"] = true;
            }
        }

        private void UnselectAllDocuments(object sender, EventArgs e)
        {
            clbDocuments.ClearSelected();
        }
    }
}