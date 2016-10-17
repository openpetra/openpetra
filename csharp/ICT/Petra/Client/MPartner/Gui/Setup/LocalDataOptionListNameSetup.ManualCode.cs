//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
//
// Copyright 2004-2016 by OM International
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
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Gui;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmLocalDataOptionListNameSetup
    {
        // Needed for Module-based Read-only security checks only.
        private string FContext;

        /// <summary>Needed for Module-based Read-only security checks only.</summary>
        public string Context
        {
            get
            {
                return FContext;
            }

            set
            {
                FContext = value;
            }
        }

        private void NewRowManual(ref PDataLabelLookupCategoryRow ARow)
        {
            // Deal with the primary key - we need a unique Category code
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.PDataLabelLookupCategory.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PDataLabelLookupCategory.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.CategoryCode = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPDataLabelLookupCategory();
        }

        #region Security

        private List <string>ApplySecurityManual()
        {
            // Whatever the Context is: the Module to check security for is *always* MPartner!
            FPetraUtilsObject.SecurityScreenContext = "MPartner";

            return new List <string>();
        }

        private void AfterRunOnceOnActivationManual()
        {
            TSetupScreensSecurityHelper.ShowMsgUserWillNeedToHaveDifferentAdminModulePermissionForEditing(
                this, FPetraUtilsObject.SecurityReadOnly, FContext, Catalog.GetString("Partner"), "PTNRADMIN",
                "LocalDataOptionListNameSetup_R-O_");
        }

        #endregion

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TStandardFormPrint.PrintGrid(APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[] { 0, 1 },
                new int[]
                {
                    PDataLabelLookupCategoryTable.ColumnCategoryCodeId,
                    PDataLabelLookupCategoryTable.ColumnCategoryDescId
                });
        }
    }
}