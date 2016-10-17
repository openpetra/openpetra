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
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmFormalitySetup
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

        private void NewRowManual(ref PFormalityRow ARow)
        {
            ARow.CountryCode = "";
            ARow.LanguageCode = "";
            ARow.AddresseeTypeCode = "";
            ARow.FormalityLevel = 0;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPFormality();
        }

        private void ValidateDataDetailsManual(PFormalityRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidateFormalitySetup(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
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
                "FormalitySetup_R-O_");
        }

        #endregion

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TStandardFormPrint.PrintGrid(APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[] { 0, 1, 2, 3, 4, 5 },
                new int[]
                {
                    PFormalityTable.ColumnLanguageCodeId,
                    PFormalityTable.ColumnCountryCodeId,
                    PFormalityTable.ColumnAddresseeTypeCodeId,
                    PFormalityTable.ColumnFormalityLevelId,
                    PFormalityTable.ColumnSalutationTextId,
                    PFormalityTable.ColumnComplimentaryClosingTextId
                });
        }
    }
}