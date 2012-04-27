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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using GNU.Gettext;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using System.Resources;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    /// <summary>
    /// manual code for TFrmBriefFoundationReport class
    /// </summary>
    public partial class TFrmPublicationStatisticalReport
    {
        /// <summary>
        /// Called during loading of the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TFrmPublicationStatisticalReport_Load(object sender, EventArgs e)
        {
            FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("County", 3.0));
            FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Donors", 2.5));
            FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("ExParticipants", 2.5));
            FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Churches", 2.5));
            FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Applicants", 2.5));

            String PublicationCodeDBName = Ict.Petra.Shared.MPartner.Mailroom.Data.PPublicationTable.GetPublicationCodeDBName();
            DataTable SubscriptionTable = TDataCache.TMPartner.GetCacheableSubscriptionsTable(
                Ict.Petra.Shared.TCacheableSubscriptionsTablesEnum.PublicationList);

            foreach (DataRow row in SubscriptionTable.Rows)
            {
                String CurrentPublication = (String)row[PublicationCodeDBName];
                FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction(CurrentPublication, 2.8));
            }

            ucoReportColumns.SetAvailableFunctions(FPetraUtilsObject.GetAvailableFunctions());
            ucoReportColumns.FillColumnGrid();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ACalculator"></param>
        /// <param name="AReportAction"></param>
        private void ReadControlsManual(TRptCalculator ACalculator, TReportActionEnum AReportAction)
        {
            int ColumnCounter = 0;
            double ColumnPosition = 0;
            String SpecialValues = "County,Donors,ExParticipants,Churches,Applicants";

            // Get the columns
            List <KeyValuePair <String, Double>>ColumnDefinitions = ucoReportColumns.GetColumnHeadings();

            // store the column values in ACalculator
            foreach (KeyValuePair <String, Double>ColumnDefinition in ColumnDefinitions)
            {
                String ColumnType = "Publication";

                if (SpecialValues.Contains(ColumnDefinition.Key))
                {
                    // the column is one of the special columns County, Donors, ExParticipants, Churches or Applicants
                    ColumnType = ColumnDefinition.Key;
                }

                if (AReportAction == TReportActionEnum.raGenerate)
                {
                    // Add these only to the calculator if we generate the report. Not used
                    // when the report is saved.
                    ACalculator.AddParameter("param_calculation", ColumnType, ColumnCounter);
                    ACalculator.AddParameter("param_label", ColumnType, ColumnCounter);
                    ACalculator.AddParameter("ColumnAlign", "left", ColumnCounter);
                    ACalculator.AddParameter("ColumnCaption", ColumnDefinition.Key, ColumnCounter);
                    ACalculator.AddParameter("ColumnPosition", ColumnPosition.ToString(), ColumnCounter);
                }

                ++ColumnCounter;
                ColumnPosition += ColumnDefinition.Value;
            }
        }
    }
}