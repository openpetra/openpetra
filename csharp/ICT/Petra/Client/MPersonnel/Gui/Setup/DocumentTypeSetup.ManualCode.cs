//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MCommon.Validation;

namespace Ict.Petra.Client.MPersonnel.Gui.Setup
{
    public partial class TFrmDocumentTypeSetup
    {
        private void NewRowManual(ref PmDocumentTypeRow ARow)
        {
            // Deal with primary key.  DocCode is a unique string
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.PmDocumentType.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PmDocumentType.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.DocCode = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            // Deal with the possibility that we have no categories set up for the comboBox
            Type DataTableType;

            // Load Data
            PmDocumentCategoryTable allCategories = new PmDocumentCategoryTable();
            DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("DocumentTypeCategoryList", String.Empty, null, out DataTableType);

            allCategories.Merge(CacheDT);

            if (allCategories.Rows.Count == 0)
            {
                string Msg =
                    "Before you attempt to create a New Document Type you should return to the Personnel Setup screen and create a new 'Document Type Category'.";
                MessageBox.Show(Msg, "Open Petra Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CreateNewPmDocumentType();
        }

        private void EnableDisableUnassignableDate(Object sender, EventArgs e)
        {
            dtpDetailUnassignableDate.Enabled = chkDetailUnassignableFlag.Checked;

            if (!chkDetailUnassignableFlag.Checked)
            {
                dtpDetailUnassignableDate.Date = null;
            }
            else
            {
                dtpDetailUnassignableDate.Date = DateTime.Now.Date;
            }
        }

        private void ValidateDataDetailsManual(PmDocumentTypeRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedValidation_CacheableDataTables.ValidateDocumentType(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }
    }
}