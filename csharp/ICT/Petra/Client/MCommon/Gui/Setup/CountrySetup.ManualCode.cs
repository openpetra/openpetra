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
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonDialogs;

namespace Ict.Petra.Client.MCommon.Gui.Setup
{
    public partial class TFrmCountrySetup
    {
        private void InitializeManualCode()
        {
            FPetraUtilsObject.DataSaved += HandleDataSaved;
        }

        private void NewRowManual(ref PCountryRow ARow)
        {
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.PCountry.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PCountry.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.CountryCode = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPCountry();
        }

        private void UpdateCountryNameLocal(Object sender, EventArgs e)
        {
            if (txtDetailCountryNameLocal.Text == String.Empty)
            {
                txtDetailCountryNameLocal.Text = txtDetailCountryName.Text;
            }
        }

        private void UpdateTimeZoneMaximum(Object sender, EventArgs e)
        {
            if (txtDetailTimeZoneMaximum.NumberValueDouble == 0.0)
            {
                txtDetailTimeZoneMaximum.NumberValueDouble = txtDetailTimeZoneMinimum.NumberValueDouble;
            }
        }

        private void ValidateDataDetailsManual(PCountryRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedValidation_CacheableDataTables.ValidateCountrySetupManual(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        void HandleDataSaved(object Sender, TDataSavedEventArgs e)
        {
            if (e.Success)
            {
                TDataCache.TMCommon.RefreshCacheableCommonTable(TCacheableCommonTablesEnum.CountryList);
            }
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TFrmSelectPrintFields.SelectAndPrintGridFields(this, APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[]
                {
                    PCountryTable.ColumnCountryCodeId,
                    PCountryTable.ColumnCountryNameId,
                    PCountryTable.ColumnTimeZoneMinimumId,
                    PCountryTable.ColumnTimeZoneMaximumId,
                    PCountryTable.ColumnUndercoverId,
                    PCountryTable.ColumnDeletableId
                });
        }
    }
}