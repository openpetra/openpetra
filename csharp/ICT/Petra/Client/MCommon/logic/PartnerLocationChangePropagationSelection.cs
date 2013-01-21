//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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
using Ict.Common.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using DevAge.Drawing;
using DevAge.ComponentModel;
using DevAge.ComponentModel.Converter;
using DevAge.ComponentModel.Validator;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Views;
using Ict.Common.Controls;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MCommon
{
    /// <summary>
    /// Contains logic for the Partner Location Change Propagation Selection Dialog.
    /// </summary>
    public class TPartnerLocationChangePropagationSelectionLogic : System.Object
    {
        private System.Data.DataView FPartnerSharingLocationDV;
        private TSgrdDataGrid FDataGridPersonsLocations;
        private TSgrdDataGrid FDataGridChangedDetails;

        /// <summary>
        /// This property handles the datasource of this dialogue
        ///
        /// </summary>
        public System.Data.DataView PartnerSharingLocationDV
        {
            get
            {
                return FPartnerSharingLocationDV;
            }

            set
            {
                FPartnerSharingLocationDV = value;
            }
        }

        /// <summary>
        /// This property handles the TypeCode property
        ///
        /// </summary>
        public TSgrdDataGrid DataGridPersonsLocations
        {
            get
            {
                return this.FDataGridPersonsLocations;
            }

            set
            {
                this.FDataGridPersonsLocations = value;
            }
        }


        /// <summary>
        /// This procedure creates the colums of the Persons' Locations DataGrid
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CreateColumnsPersonsLocations(TSgrdDataGrid AGrid, System.Data.DataTable ASourceTable)
        {
            SourceGrid.Cells.Editors.TextBoxUITypeEditor DateEditor;
            Ict.Common.TypeConverter.TDateConverter DateTypeConverter;
            this.DataGridPersonsLocations = AGrid;
            DateEditor = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(DateTime));
            DateEditor.EditableMode = EditableMode.None;
            DateTypeConverter = new Ict.Common.TypeConverter.TDateConverter();
            DateEditor.TypeConverter = DateTypeConverter;
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerTable.TableId, PPartnerTable.ColumnPartnerShortNameId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetPartnerShortNameDBName()]);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnPartnerKeyId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetPartnerKeyDBName()], 77);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnTelephoneNumberId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetTelephoneNumberDBName()]);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnExtensionId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetExtensionDBName()]);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnFaxNumberId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetFaxNumberDBName()]);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnFaxExtensionId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetFaxExtensionDBName()]);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnMobileNumberId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetMobileNumberDBName()]);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnAlternateTelephoneId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetAlternateTelephoneDBName()]);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnEmailAddressId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetEmailAddressDBName()]);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnUrlId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetUrlDBName()]);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnLocationTypeId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetLocationTypeDBName()], 88);
            this.FDataGridPersonsLocations.AddCheckBoxColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnSendMailId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetSendMailDBName()]);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnDateEffectiveId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetDateEffectiveDBName()], -1, null, DateEditor, null,
                null);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnDateGoodUntilId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetDateGoodUntilDBName()], -1, null, DateEditor, null,
                null);

            // this.FDataGridPersonsLocations.AddTextColumn(PartnerAddressAggregateTDSChangePromotionParametersTable.GetSiteKeyLabel(),
            // ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetSiteKeyDBName()], 77);
            this.FDataGridPersonsLocations.AddTextColumn(
                TTypedDataTable.GetLabel(PPartnerLocationTable.TableId, PPartnerLocationTable.ColumnLocationKeyId),
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetLocationKeyDBName()]);

            // Following Columns are for debugging only!
            // this.FDataGridPersonsLocations.AddTextColumn('Site Key Of Edited Record', ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetSiteKeyOfEditedRecordDBName()], 77);
            // this.FDataGridPersonsLocations.AddTextColumn('Location Key Of Edited Record', ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetLocationKeyOfEditedRecordDBName()]);
        }

        #region TPartnerLocationChangePropagationSelectionLogic

        /// <summary>
        /// This procedure creates the colums of the Changed Details DataGrid
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CreateColumnsChangedDetails(TSgrdDataGrid AGrid, System.Data.DataTable ASourceTable)
        {
            FDataGridChangedDetails = AGrid;

            // this.FDataGridChangedDetails.AddTextColumn('Field DB Name', ASourceTable.Columns['DBName']);
            this.FDataGridChangedDetails.AddTextColumn("Detail", ASourceTable.Columns["DBLabel"]);
            this.FDataGridChangedDetails.AddTextColumn("Old Value", ASourceTable.Columns["OriginalValue"]);
            this.FDataGridChangedDetails.AddTextColumn("New Value", ASourceTable.Columns["CurrentValue"]);
        }

        /// <summary>
        /// This procedure initializes this System.Object.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InitialisePartnerTypeFamilyMembers(PartnerAddressAggregateTDSChangePromotionParametersTable APartnerSharingLocationDT)
        {
            // seems this variable is never actually being used
            // was: private PartnerAddressAggregateTDSChangePromotionParametersTable FPartnerSharingLocationDT;
            // this.FPartnerSharingLocationDT = APartnerSharingLocationDT;
        }

        #endregion
    }
}