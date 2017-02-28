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
using System.Data;
using GNU.Gettext;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using System.Resources;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    /// <summary>
    /// manual code for TFrmLocalPartnerDataReport class
    /// </summary>
    public partial class TFrmLocalPartnerDataReport
    {
        /// <summary>
        /// Called during loading of the form
        /// Adds the available labels to the Selection box.
        /// </summary>
        private void SetLocalDataLabels()
        {
            double widthInCm;
            PDataLabelTable DataLabels;
            PDataLabelUseTable DataLabelUse;

            DataRow[] filteredRows;

            DataLabels = (PDataLabelTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.DataLabelList);
            DataLabelUse = (PDataLabelUseTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.DataLabelUseList);

            foreach (PDataLabelUseRow UseRow in DataLabelUse.Rows)
            {
                if ((UseRow.Use == "Bank")
                    || (UseRow.Use == "Church")
                    || (UseRow.Use == "Family")
                    || (UseRow.Use == "Organisation")
                    || (UseRow.Use == "Person")
                    || (UseRow.Use == "Unit")
                    || (UseRow.Use == "Venue"))
                {
                    filteredRows = DataLabels.Select(PDataLabelTable.GetKeyDBName() + " = " + UseRow.DataLabelKey.ToString());

                    if (filteredRows.Length > 0)
                    {
                        PDataLabelRow row = (PDataLabelRow)filteredRows[0];

                        if (row.DataType == "char")
                        {
                            widthInCm = TPartnerColumnFunction.CharLengthToCM(row.CharLength);
                        }
                        else if (row.DataType == "partnerkey")
                        {
                            widthInCm = TPartnerColumnFunction.CharLengthToCM(10);
                        }
                        else if (row.DataType == "lookup")
                        {
                            widthInCm = TPartnerColumnFunction.CharLengthToCM(14);
                        }
                        else if (row.DataType == "boolean")
                        {
                            widthInCm = TPartnerColumnFunction.CharLengthToCM(5);
                        }
                        else
                        {
                            widthInCm = TPartnerColumnFunction.CharLengthToCM(10);
                        }

                        /* minimum width of column, so that the caption can be displayed (with a footnote number if necessary) */
                        if (widthInCm < 1.5)
                        {
                            widthInCm = 1.5;
                        }

                        FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("DataLabelColumn", "param_label", row.Text, widthInCm)); //FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction(row.Text, "param_label",
                                                                                                                                                   // "DataLabelColumn", widthInCm));
                    }
                }
            }
        }

        /// <summary>
        /// Adds the selected columns to the calculation.
        /// </summary>
        /// <param name="ACalc"></param>
        /// <param name="AReportAction"></param>
        private void ReadLocalDataLabel(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_label_type", "partner");
            ACalc.AddParameter("param_labeluse", "Person,Family,Church,Organisation,Bank,Unit,Venue");

            String SelectedColumns = "";

            DataTable ColumnTable = ACalc.GetParameters().ToDataTable();

            foreach (DataRow Row in ColumnTable.Rows)
            {
                if ((Row["name"].ToString() == "param_calculation")
                    && (Row["level"].ToString() == "-1")
                    && (Row["subreport"].ToString() == "-1")
                    && (Row["value"].ToString().Contains("eString:")))
                {
                    int ColumnIndex = Convert.ToInt32(Row["column"].ToString());

                    int Index = Row["value"].ToString().IndexOf("eString:");

                    String ColumnName = Row["value"].ToString().Substring(Index + 8);

                    if ((ColumnName == "Partner Key")
                        || (ColumnName == "Partner Name"))
                    {
                        // don't add partner key and partner name because these are not data labels
                        continue;
                    }

                    if (ColumnName == "DataLabelColumn")
                    {
                        ColumnName = ACalc.GetParameters().Get("param_label", ColumnIndex, -1, eParameterFit.eExact).ToString();
                    }

                    SelectedColumns = SelectedColumns + ColumnName + ",";

                    double ColumnWidth = ACalc.GetParameters().Get("ColumnWidth", ColumnIndex, -1, eParameterFit.eExact).ToDouble();

                    ACalc.AddParameter("param_calculation", "DataLabelColumn", ColumnIndex);
                    ACalc.AddParameter("ColumnWidth", ColumnWidth, ColumnIndex);
                    ACalc.AddParameter("param_label", ColumnName, ColumnIndex);
                }
            }

            if (SelectedColumns.Length > 0)
            {
                SelectedColumns.Substring(0, SelectedColumns.Length - 1);
            }

            ACalc.AddParameter("param_labels", SelectedColumns);
        }
    }
}