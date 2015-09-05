//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr, peters
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using GNU.Gettext;
using SourceGrid;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    /// <summary>
    /// manual code for TFrmBriefFoundationReport class
    /// </summary>
    public partial class TFrmPublicationStatisticalReport
    {
        private void InitializeManualCode()
        {
            FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("County", 3.0));
            FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Donors", 2.5));
            FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("ExParticipants", 2.5));
            FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Churches", 2.5));
            FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Applicants", 2.5));

            ucoReportColumns.SetAvailableFunctions(FPetraUtilsObject.GetAvailableFunctions());
            ucoReportColumns.FillColumnGrid();

            InitializePublicationCodeList();
        }

        private void InitializePublicationCodeList()
        {
            string CheckedMember = "CHECKED";
            string ValueMember = PPublicationTable.GetPublicationCodeDBName();
            string DisplayMember = PPublicationTable.GetPublicationDescriptionDBName();

            DataTable Publications = TDataCache.TMPartner.GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum.PublicationList);
            DataView PublicationsView = new DataView(Publications);

            PublicationsView.Sort = ValueMember;

            DataTable PublicationsTable = PublicationsView.ToTable(true, new string[] { ValueMember, DisplayMember });
            PublicationsTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbPublicationCode.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            clbPublicationCode.Columns.Clear();
            clbPublicationCode.AddCheckBoxColumn("", PublicationsTable.Columns[CheckedMember], 17, false);
            clbPublicationCode.AddTextColumn(Catalog.GetString("Group Code"), PublicationsTable.Columns[ValueMember]);
            clbPublicationCode.AddTextColumn(Catalog.GetString("Description"), PublicationsTable.Columns[DisplayMember]);
            clbPublicationCode.DataBindGrid(PublicationsTable,
                ValueMember,
                CheckedMember,
                ValueMember,
                false,
                true,
                false);

            clbPublicationCode.AutoResizeGrid();
            clbPublicationCode.AutoStretchColumnsToFitWidth = true;
            clbPublicationCode.SetCheckedStringList("");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ACalculator"></param>
        /// <param name="AReportAction"></param>
        private void ReadControlsManual(TRptCalculator ACalculator, TReportActionEnum AReportAction)
        {
            TVerificationResult VerificationResult;

            string PublicationCodesToAdd = string.Empty;

            // Disabled selecting all publications for now as report currently cuts them off.
            // Can show a maximum of 8.

            /*if (rbtFromList.Checked)
             * {*/
            string SelectedPublicationCodes = clbPublicationCode.GetCheckedStringList(true);

            PublicationCodesToAdd = clbPublicationCode.GetCheckedStringList(false);

            if (clbPublicationCode.CheckedItemsCount == 0)
            {
                VerificationResult = new TVerificationResult(Catalog.GetString("Select Publication Codes"),
                    Catalog.GetString("No Publication Code was selected!"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
            else if (clbPublicationCode.CheckedItemsCount > 8)
            {
                VerificationResult = new TVerificationResult(Catalog.GetString("Select Publication Codes"),
                    string.Format(Catalog.GetString("You have selected {0} publications. Please reduce this number to 8 or fewer."),
                        clbPublicationCode.CheckedItemsCount),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            ACalculator.AddStringParameter("param_clbPublicationCode", SelectedPublicationCodes);

            if (SelectedPublicationCodes.Length > 25)
            {
                SelectedPublicationCodes = "Selected Accounts";
            }

            ACalculator.AddParameter("param_publication_list_title", SelectedPublicationCodes);

            /*}
             * else
             * {
             *  PublicationCodesToAdd = clbPublicationCode.GetAllStringList();
             * }*/

            if (AReportAction == TReportActionEnum.raGenerate)
            {
                StringCollection PublicationCodesToAddSC = StringHelper.GetCSVList(PublicationCodesToAdd, ",");
                int ColumnCounter = 0;
                double Position = 0;

                // add extra parameters for user defined columns
                while (ACalculator.GetParameters().GetParameter("param_calculation", ColumnCounter, -1, eParameterFit.eAllColumnFit) != null)
                {
                    string Description =
                        ACalculator.GetParameters().GetParameter("param_calculation", ColumnCounter, -1, eParameterFit.eAllColumnFit).value.ToString();
                    double Width =
                        ACalculator.GetParameters().GetParameter("ColumnWidth", ColumnCounter, -1, eParameterFit.eAllColumnFit).value.ToDouble();

                    ACalculator.AddParameter("param_label", Description, ColumnCounter);
                    ACalculator.AddParameter("ColumnAlign", "left", ColumnCounter);
                    ACalculator.AddParameter("ColumnCaption", Description, ColumnCounter);
                    ACalculator.AddParameter("ColumnPosition", Position, ColumnCounter);

                    Position += Width;

                    ++ColumnCounter;
                }

                double ColumnWidth = 2.3;

                // add a new column for each publication
                foreach (string Code in PublicationCodesToAddSC)
                {
                    ACalculator.AddParameter("param_calculation", "Publication", ColumnCounter);
                    ACalculator.AddParameter("param_label", "Publication", ColumnCounter);
                    ACalculator.AddParameter("ColumnAlign", "left", ColumnCounter);
                    ACalculator.AddParameter("ColumnCaption", Code, ColumnCounter);
                    ACalculator.AddParameter("ColumnPosition", Position, ColumnCounter);
                    ACalculator.AddParameter("ColumnWidth", ColumnWidth, ColumnCounter);

                    Position += ColumnWidth;

                    ++ColumnCounter;
                }

                ACalculator.SetMaxDisplayColumns(ColumnCounter);

                // get the country code for this openpetra's site ledger
                string CountryCode = TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetCountryCodeFromSiteLedger();
                ACalculator.AddParameter("param_cmbCountryCode", CountryCode);
            }
        }
    }
}