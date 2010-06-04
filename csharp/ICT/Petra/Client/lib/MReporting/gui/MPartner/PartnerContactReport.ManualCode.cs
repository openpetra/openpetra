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
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Mono.Unix;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using System.Resources;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Logic;



using Ict.Petra.Shared.MCommon.Data;


namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    /// <summary>
    /// manual code for TFrmBriefFoundationReport class
    /// </summary>
    public partial class TFrmPartnerContactReport
    {
		protected void grdAttribute_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
		{
			
			Ict.Common.Data.TTypedDataTable TypedTable;
			TRemote.MCommon.DataReader.GetData(PContactAttributeTable.GetTableDBName(), null, out TypedTable);

			PContactAttributeTable ContactAttributeTable = new PContactAttributeTable();
			ContactAttributeTable.Merge(TypedTable);
			grdAttribute.Columns.Clear();
			grdAttribute.AddTextColumn("Attribute", ContactAttributeTable.ColumnContactAttributeCode);

//			FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
			
			DataView myDataView = TypedTable.DefaultView;
			myDataView.AllowNew = false;
			grdAttribute.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
			grdAttribute.AutoSizeCells();
		}
		protected void grdDetail_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
		{
			PContactAttributeDetailTable DetailTable = new PContactAttributeDetailTable();
			
			grdDetail.Columns.Clear();
			grdDetail.AddTextColumn("Detail", DetailTable.ColumnContactAttrDetailCode);
			grdDetail.AddTextColumn("Description", DetailTable.ColumnContactAttrDetailDescr);
			grdDetail.AutoSizeCells();
		}
		protected void grdSelection_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
		{
//			grdSelection.AddTextColumn("Attribute", );
//			grdSelection.AddTextColumn("Detail", );
//			grdSelection.AddTextColumn("Description", );
		}
		protected void grdAttribute_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
		{
			
		}
		protected void grdDetail_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
		{
			
		}
		protected void grdSelection_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
		{
			
		}
		protected void grdAttribute_SetControls(TParameterList AParameters)
		{
			
		}
		protected void grdDetail_SetControls(TParameterList AParameters)
		{
			
		}
		protected void grdSelection_SetControls(TParameterList AParameters)
		{
			
		}
		protected void AttributeFocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
		{
			
		}
	}
}
