//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       markusm
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
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Common;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Controllers;
using Ict.Common.Controls;
using System.Collections;
using DevAge;
using DevAge.ComponentModel.Validator;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using System.Globalization;
using DevAge.ComponentModel.Converter;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// UserControl for editing Partner Details for a Partner of Partner Class  BANK.
    ///
    /// When dealing with this grid one has to consider that the Netherland do not     know bank branches and that Credit Cards do not have branches as well.     Last time I checked how many accounts one bank in the Netherlands had     I ended up
    /// with some 9756. I was also puzzled what use this grid may have.     It is very tedious to find the right bank account from some 9000 bank     accounts. Therefore the paged grid would be needed. But Rob decided that     we currently do not
    /// build this grid into this screen, since this sreen     was hardly used anyway. We are happy to wait for the first complaints.
    /// </summary>
    public class TUC_PartnerDetailsBankLogic
    {
        private PartnerEditTDS FMainDS;

        /// <summary>
        /// FBankingDetailsKey:        System.Int32;    FDataGrid:                 TSgrdDataGrid;    FPartnerEditUIConnector:   IPartnerUIConnectorsPartnerEdit;    FPBankingDetailsDT:        PBankingDetailsTable;    FPBankingDetailsDV:
        /// System.Data.DataView;    FPPartnerBankingDetailsDT: PPartnerBankingDetailsTable;    FPPartnerBankingDetailsDV: System.Data.DataView;    FRecalculateScreenParts:   TRecalculateScreenPartsEventHandler; Custom Events    procedure
        /// OnRecalculateScreenParts(e: TRecalculateScreenPartsEventArgs);
        /// This property is used to hold the main DataSet used in this screen
        ///
        /// </summary>
        public PartnerEditTDS MainDS
        {
            get
            {
                return FMainDS;
            }

            set
            {
                FMainDS = value;
            }
        }
    }
}