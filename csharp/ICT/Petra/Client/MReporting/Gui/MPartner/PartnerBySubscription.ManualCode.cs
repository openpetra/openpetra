//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    public partial class TFrmPartnerBySubscription
    {
        private void InitializeManualCode()
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = PPublicationTable.GetPublicationDescriptionDBName();
            string ValueMember = PPublicationTable.GetPublicationCodeDBName();

            DataTable Table = TDataCache.TMPartner.GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum.PublicationList);
            DataView view = new DataView(Table);

            // TODO view.RowFilter = only active publications?
            view.Sort = ValueMember;

            DataTable NewTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbIncludePublication.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            //clbIncludePublication.SelectionMode = SourceGrid.GridSelectionMode.Row;
            clbIncludePublication.Columns.Clear();
            clbIncludePublication.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            clbIncludePublication.AddTextColumn(Catalog.GetString("Publication Code"), NewTable.Columns[ValueMember]);
            clbIncludePublication.AddTextColumn(Catalog.GetString("Publication Description"), NewTable.Columns[DisplayMember]);
            clbIncludePublication.DataBindGrid(NewTable, ValueMember, CheckedMember, ValueMember, false, true, false);

            dtpDateOfSendingCopy.Date = DateTime.Now;

            int offset = chkIncludeActiveSubscriptionsOnly.Top - (pnlDetails.Height - chkIncludeActiveSubscriptionsOnly.Height - 6);
            chkIncludeActiveSubscriptionsOnly.Top -= offset;
            lblDateOfSendingCopy.Top -= offset;
            dtpDateOfSendingCopy.Top -= offset;

            ActiveSubscriptionsBoxChanged(null, null);
        }

        /// <summary>
        /// only run this code once during activation
        /// </summary>
        private void RunOnceOnActivationManual()
        {
            // no columns tab needed if called from extracts
            if (CalledFromExtracts)
            {
                tabReportSettings.Controls.Remove(tpgColumns);
                tabReportSettings.Controls.Remove(tpgReportSorting);
                lblDateOfSendingCopy.Hide();
                dtpDateOfSendingCopy.Hide();
            }
            else
            {
                chkIncludeActiveSubscriptionsOnly.Hide();
                lblSubscriptionStatus.Hide();
                cmbSubscriptionStatus.Hide();
                chkFreeSubscriptionsOnly.Hide();
                ucoChkFilter.Hide();
            }

            rbtSingle.Checked = true;
            chkIncludeActiveSubscriptionsOnly.Checked = true;

            // enable autofind in list for first character (so the user can press character to find list entry)
            // from Sep 2015 this is handled automatically by the code generator
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if ((clbIncludePublication.GetCheckedStringList().Length == 0) && (AReportAction != TReportActionEnum.raSave))
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Select at least one subscription"),
                    Catalog.GetString("Please select at least one subscription, to avoid listing the whole database!"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }

        private void ActiveSubscriptionsBoxChanged(object sender, EventArgs e)
        {
            if (chkIncludeActiveSubscriptionsOnly.Checked)
            {
                lblSubscriptionStatus.Visible = false;
                cmbSubscriptionStatus.Visible = false;
            }
            else
            {
                lblSubscriptionStatus.Visible = true;
                cmbSubscriptionStatus.Visible = true;
            }
        }
    }
}