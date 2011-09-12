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

namespace Ict.Petra.Client.MCommon
{
    /// <summary>
    /// Contains logic for the Partner Address Change Propagation Selection Dialog.
    /// </summary>
    public class TPartnerAddressChangePropagationSelectionLogic : System.Object
    {
        private System.Data.DataView FPartnerSharingLocationDV;
        private TSgrdDataGrid FDataGrid;

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
        public TSgrdDataGrid DataGrid
        {
            get
            {
                return this.FDataGrid;
            }

            set
            {
                this.FDataGrid = value;
            }
        }


        /// <summary>
        /// This procedure creates the colums of the DataGrid displayed
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CreateColumns(TSgrdDataGrid AGrid, System.Data.DataTable ASourceTable)
        {
            this.DataGrid = AGrid;
            this.FDataGrid.AddTextColumn("Short Name",
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetPartnerShortNameDBName()], 240);
            this.FDataGrid.AddTextColumn("Partner Key",
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetPartnerKeyDBName()], 77);
            this.FDataGrid.AddTextColumn("Partner Class",
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetPartnerClassDBName()], 83);
            this.FDataGrid.AddTextColumn("Telephone",
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetTelephoneNumberDBName()], 110);
            this.FDataGrid.AddTextColumn("Location Type",
                ASourceTable.Columns[PartnerAddressAggregateTDSChangePromotionParametersTable.GetLocationTypeDBName()], 88);
        }

        /// <summary>
        /// This procedure initializes this object
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InitialisePartnerTypeFamilyMembers(PartnerAddressAggregateTDSChangePromotionParametersTable APartnerSharingLocationDT)
        {
            // seems that variable is never actually used
            // was: private PartnerAddressAggregateTDSChangePromotionParametersTable FPartnerSharingLocationDT;
            // this.FPartnerSharingLocationDT = APartnerSharingLocationDT;
        }
    }
}