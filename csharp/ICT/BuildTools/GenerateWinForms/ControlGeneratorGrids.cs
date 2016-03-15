//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.Windows.Forms;
using System.Xml;
using Ict.Tools.CodeGeneration;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.Winforms
{
    /// generator for the SourceGrid data grid
    public class SourceGridGenerator : TControlGenerator
    {
        //Int16 FDecimalPrecision = 2;

        private Int16 FColumnIndex = -1;
        private string FPrevControlName = String.Empty;
        private const string TYPE_DATA_GRID_PAGED = "Ict.Common.Controls.TSgrdDataGridPaged";
        private const string TYPE_DATA_GRID_NON_PAGED = "Ict.Common.Controls.TSgrdDataGrid";

        /// <summary>constructor</summary>
        public SourceGridGenerator()
            : base("grd", TYPE_DATA_GRID_PAGED)
        {
            FGenerateLabel = false;
            FDefaultHeight = 100;
            FDefaultWidth = 200;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                string typeAttribute = TYml2Xml.GetAttribute(curNode, "Type");

                if (typeAttribute.ToLower() == "winforms")
                {
                    return false;
                }

                if (typeAttribute.Length == 0)
                {
                    this.FControlType = TYPE_DATA_GRID_NON_PAGED;
                }
                else
                {
                    this.FControlType = typeAttribute;
                }

                return true;
            }

            return false;
        }

        private void AddColumnToGrid(TFormWriter writer, string AGridControlName, string AColumnType, string ALabel,
            string AHeaderTooltip, string ATableName, string AColumnName)
        {
            string ColumnType = "Text";
            string PotentialDecimalPrecision;
            string TrueString = string.Empty;
            string FalseString = string.Empty;
            string HeaderTooltip = (AHeaderTooltip == string.Empty) ? ALabel : AHeaderTooltip;
            Int16 DecimalPrecision = 2;

            // Valid types are: Text, DateTime, Currency, Decimal, Boolean, PartnerKey and Short/LongTime
            if (AColumnType.Contains("DateTime"))
            {
                ColumnType = "Date";
            }
            else if (AColumnType.Contains("Currency"))
            {
                ColumnType = "Currency";

                if (AColumnType.Contains("Currency(") && AColumnType.Contains(")"))
                {
                    PotentialDecimalPrecision = AColumnType.Substring(AColumnType.IndexOf('(') + 1,
                        AColumnType.IndexOf(')') - AColumnType.IndexOf('(') - 1);

                    if (PotentialDecimalPrecision != String.Empty)
                    {
                        try
                        {
                            DecimalPrecision = Convert.ToInt16(PotentialDecimalPrecision);
                        }
                        catch (System.FormatException)
                        {
                            throw new ApplicationException(
                                "Grid Column with currency formatting: The specifier for the currency precision '" + PotentialDecimalPrecision +
                                "' is not a number!");
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
            else if (AColumnType.Contains("Decimal"))
            {
                ColumnType = "Decimal";

                if (AColumnType.Contains("Decimal(") && AColumnType.Contains(")"))
                {
                    PotentialDecimalPrecision = AColumnType.Substring(AColumnType.IndexOf('(') + 1,
                        AColumnType.IndexOf(')') - AColumnType.IndexOf('(') - 1);

                    if (PotentialDecimalPrecision != String.Empty)
                    {
                        try
                        {
                            DecimalPrecision = Convert.ToInt16(PotentialDecimalPrecision);
                        }
                        catch (System.FormatException)
                        {
                            throw new ApplicationException(
                                "Grid Column with decimal formatting: The specifier for the decimal precision '" + PotentialDecimalPrecision +
                                "' is not a number!");
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
            else if (AColumnType.Contains("Boolean"))
            {
                if (AColumnType.Contains("("))
                {
                    string BooleanNames = AColumnType.Substring(AColumnType.IndexOf('(') + 1,
                        AColumnType.IndexOf(')') - AColumnType.IndexOf('(') - 1);
                    TrueString = BooleanNames.Split(',')[0];
                    FalseString = BooleanNames.Split(',')[1];
                }

                if ((TrueString.Length > 0) || (FalseString.Length > 0))
                {
                    ColumnType = "Boolean";
                }
                else
                {
                    ColumnType = "CheckBox";
                }
            }
            else if (AColumnType.Contains("PartnerKey"))
            {
                ColumnType = "PartnerKey";
            }
            else if (AColumnType.Contains("Time"))
            {
                ColumnType = AColumnType;
            }

            if (ColumnType == "Boolean")
            {
                writer.Template.AddToCodelet("INITMANUALCODE",
                    AGridControlName + ".Add" + ColumnType + "Column(Catalog.GetString(\"" + ALabel + "\"), " +
                    "FMainDS." +
                    ATableName + ".Column" +
                    AColumnName + ", Catalog.GetString(\"" + TrueString + "\"), Catalog.GetString(\"" + FalseString + "\"));" + Environment.NewLine);
            }
            else if (((ColumnType == "Currency") || (ColumnType == "Decimal")) && (DecimalPrecision != 2))
            {
                // Currency or decimal with non-standard decimal digits
                writer.Template.AddToCodelet("INITMANUALCODE",
                    AGridControlName + ".Add" + ColumnType + "Column(Catalog.GetString(\"" + ALabel + "\"), " +
                    "FMainDS." +
                    ATableName + ".Column" +
                    AColumnName + ", " + DecimalPrecision.ToString() + ");" + Environment.NewLine);
            }
            else
            {
                // General case for most types
                writer.Template.AddToCodelet("INITMANUALCODE",
                    AGridControlName + ".Add" + ColumnType + "Column(Catalog.GetString(\"" + ALabel + "\"), " +
                    "FMainDS." +
                    ATableName + ".Column" +
                    AColumnName + ");" + Environment.NewLine);
            }

            // Are we still working with the same grid?  If not reset our tracking variables back to the first column
            if (AGridControlName != FPrevControlName)
            {
                FColumnIndex = 0;
                FPrevControlName = AGridControlName;
            }

            writer.Template.AddToCodelet("GRIDHEADERTOOLTIP",
                AGridControlName + ".SetHeaderTooltip(" + FColumnIndex.ToString() + ", Catalog.GetString(\"" + HeaderTooltip + "\"));" +
                Environment.NewLine);
            FColumnIndex++;
        }

        /// <summary>
        /// Checks whether a specified column in a table does contain the word detail
        /// </summary>
        /// <param name="ATableName"></param>
        /// <param name="AFieldName"></param>
        /// <returns></returns>
        private bool IsLegitimateDetailFieldName(string ATableName, string AFieldName)
        {
            List <string>TableFields = new List <string>();

            //A list of table columns that should contain the word Detail (separated by a |)
            //  Just add accordingly
            TableFields.Add("AGiftDetail|DetailNumber");
            TableFields.Add("ARecurringGiftDetail|DetailNumber");

            return TableFields.Contains(ATableName + "|" + AFieldName);
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            if (!ctrl.HasAttribute("Width"))
            {
                ctrl.SetAttribute("Width", FDefaultWidth.ToString());
            }

            base.SetControlProperties(writer, ctrl);

            writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".CancelEditingWithEscapeKey = false;" + Environment.NewLine);

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "SelectedRowActivates"))
            {
                // TODO: this function needs to be called by the manual code at the moment when eg a search finishes
                // TODO: call "Activate" + TYml2Xml.GetAttribute(ctrl.xmlNode, "SelectedRowActivates")
            }

            StringCollection Columns = TYml2Xml.GetElements(ctrl.xmlNode, "Columns");

            if (Columns.Count > 0)
            {
                writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".Columns.Clear();" + Environment.NewLine);

                //This needs to come immediately after the Columns.Clear() and before the creation of the columns
                if (ctrl.HasAttribute("SortableHeaders"))
                {
                    string trueOrFalse = ctrl.GetAttribute("SortableHeaders");
                    writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".SortableHeaders = " + trueOrFalse + ";" + Environment.NewLine);
                }

                bool isFirstColumnVarchar = false;
                bool doneFirstColumn = false;

                foreach (string ColumnFieldName in Columns)
                {
                    bool IsDetailNotMaster;
                    TTableField field = null;
                    string TableFieldTable;
                    string ColumnFieldNameResolved;

                    // customfield, eg. UC_GLTransactions, ATransaction.DateEntered and ATransaction.AnalysisAttributes
                    // there needs to be a list of CustomColumns
                    XmlNode CustomColumnsNode = TYml2Xml.GetChild(ctrl.xmlNode, "CustomColumns");
                    XmlNode CustomColumnNode = null;

                    if (CustomColumnsNode != null)
                    {
                        CustomColumnNode = TYml2Xml.GetChild(CustomColumnsNode, ColumnFieldName);
                    }

                    if ((ctrl.controlName == "grdDetails") && FCodeStorage.HasAttribute("DetailTable"))
                    {
                        TableFieldTable = FCodeStorage.GetAttribute("DetailTable");

                        if (ColumnFieldName.StartsWith("Detail") && !IsLegitimateDetailFieldName(TableFieldTable, ColumnFieldName))
                        {
                            ColumnFieldNameResolved = ColumnFieldName.Substring(6);     // Drop 'Details' out of 'Details...'
                        }
                        else
                        {
                            ColumnFieldNameResolved = ColumnFieldName;
                        }
                    }
                    else
                    {
                        TableFieldTable = ctrl.GetAttribute("TableName");
                        ColumnFieldNameResolved = ColumnFieldName;
                    }

                    if (CustomColumnNode != null)
                    {
                        TTableField tf = null;

                        // if grd has no TableName property
                        if ((TableFieldTable == "") && ColumnFieldNameResolved.Contains("."))
                        {
                            int Period = ColumnFieldNameResolved.IndexOf(".");
                            string TableName = ColumnFieldNameResolved.Remove(Period);
                            string ColumnName = ColumnFieldNameResolved.Remove(0, TableName.Length + 1);

                            AddColumnToGrid(writer, ctrl.controlName,
                                TYml2Xml.GetAttribute(CustomColumnNode, "Type"),
                                TYml2Xml.GetAttribute(CustomColumnNode, "Label"),
                                TYml2Xml.GetAttribute(CustomColumnNode, "Tooltip"),
                                TableName,
                                ColumnName);
                            tf = TDataBinding.GetTableField(null, TableName + "." + ColumnName, out IsDetailNotMaster, true);
                        }
                        else
                        {
                            AddColumnToGrid(writer, ctrl.controlName,
                                TYml2Xml.GetAttribute(CustomColumnNode, "Type"),
                                TYml2Xml.GetAttribute(CustomColumnNode, "Label"),
                                TYml2Xml.GetAttribute(CustomColumnNode, "Tooltip"),
                                TableFieldTable,
                                ColumnFieldNameResolved);
                            tf = TDataBinding.GetTableField(null, TableFieldTable + "." + ColumnFieldNameResolved, out IsDetailNotMaster, true);
                        }

                        if (!doneFirstColumn)
                        {
                            isFirstColumnVarchar = tf.strName.EndsWith("_c");
                            doneFirstColumn = true;
                        }
                    }
                    else if (ctrl.HasAttribute("TableName"))
                    {
                        field = TDataBinding.GetTableField(null, ctrl.GetAttribute("TableName") + "." + ColumnFieldName,
                            out IsDetailNotMaster, true);
                    }
                    else
                    {
                        field = TDataBinding.GetTableField(null, ColumnFieldName, out IsDetailNotMaster, true);
                    }

                    if (field != null)
                    {
                        AddColumnToGrid(writer, ctrl.controlName,
                            field.iDecimals == 10 && field.iLength == 24 ? "Decimal" : field.GetDotNetType(),
                            field.strLabel.Length > 0 ? field.strLabel : field.strName,
                            String.Empty,
                            TTable.NiceTableName(field.strTableName),
                            TTable.NiceFieldName(field.strName));

                        if (!doneFirstColumn)
                        {
                            isFirstColumnVarchar = field.strName.EndsWith("_c");
                            doneFirstColumn = true;
                        }
                    }
                }

                if (FControlType == TYPE_DATA_GRID_NON_PAGED)
                {
                    // Grid AutoFind definition (not allowed in paged grids)
                    string autoFindStr = ctrl.controlName + ".AutoFindMode = TAutoFindModeEnum.";
                    string mode = "NoAutoFind";

                    if (ctrl.HasAttribute("AutoFindMode"))
                    {
                        // Use the specified value in YAML
                        mode = ctrl.GetAttribute("AutoFindMode");
                        TLogging.Log("Info: AutoFindMode (with columns) was set to " + mode + " from explicit YAML attribute: " + ctrl.controlName);
                    }
                    else if (isFirstColumnVarchar)
                    {
                        // We can use auto-find because we have a first column based on a varchar
                        mode = "FirstCharacter";
                        TLogging.Log("Info: AutoFindMode (with columns) was set implicitly for: " + ctrl.controlName);
                    }
                    else
                    {
                        TLogging.Log("Info: AutoFindMode (with columns) was set to NoAutoFind for: " + ctrl.controlName);
                    }

                    writer.Template.AddToCodelet("INITMANUALCODE", autoFindStr + mode + ";" + Environment.NewLine);

                    if (ctrl.HasAttribute("AutoFindColumn"))
                    {
                        string colNum = ctrl.GetAttribute("AutoFindColumn");
                        writer.Template.AddToCodelet("INITMANUALCODE",
                            ctrl.controlName + ".AutoFindColumn = " + colNum + ";" + Environment.NewLine);
                        TLogging.Log("Info: AutoFindColumn was set to " + colNum + " for: " + ctrl.controlName);
                    }

                    if ((mode == "FirstCharacter") && !ctrl.HasAttribute("SortOrder"))
                    {
                        TLogging.Log("Info: AutoFind has been turned on for a grid with no YAML-defined sort order: (" + ctrl.controlName +
                            "). You can remove this message by explicitly setting a SortOrder in the YAML file.");
                    }
                }
            }
            else
            {
                //If no columns, but the user is able to add columns dynamically during the running of the form, then need this here.
                if (ctrl.HasAttribute("SortableHeaders"))
                {
                    string trueOrFalse = ctrl.GetAttribute("SortableHeaders");
                    writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".SortableHeaders = " + trueOrFalse + ";" + Environment.NewLine);
                }

                if (FControlType == TYPE_DATA_GRID_NON_PAGED)
                {
                    // Grid AutoFind definition (not allowed in paged grids)
                    string autoFindStr = ctrl.controlName + ".AutoFindMode = TAutoFindModeEnum.";
                    string mode = "FirstCharacter";

                    if (ctrl.HasAttribute("AutoFindMode"))
                    {
                        // Use the specified value in YAML
                        mode = ctrl.GetAttribute("AutoFindMode");
                        TLogging.Log("Info: AutoFindMode (without columns) was set to " + mode + " from explicit YAML attribute: " + ctrl.controlName);
                    }
                    else if (writer.FCodeStorage.ManualFileExistsAndContains(ctrl.controlName + ".AddTextColumn("))
                    {
                        // We presume can use auto-find because we have a column (maybe the first) based on a varchar
                        TLogging.Log("Info: AutoFindMode (without columns) was set implicitly for: " + ctrl.controlName);
                    }
                    else
                    {
                        mode = "NoAutoFind";
                        TLogging.Log("Info: AutoFindMode (without columns) was set to NoAutoFind for: " + ctrl.controlName);
                    }

                    writer.Template.AddToCodelet("INITMANUALCODE", autoFindStr + mode + ";" + Environment.NewLine);

                    if (ctrl.HasAttribute("AutoFindColumn"))
                    {
                        string colNum = ctrl.GetAttribute("AutoFindColumn");
                        writer.Template.AddToCodelet("INITMANUALCODE",
                            ctrl.controlName + ".AutoFindColumn = " + colNum + ";" + Environment.NewLine);
                        TLogging.Log("Info: AutoFindColumn was set to " + colNum + " for: " + ctrl.controlName);
                    }
                }
            }

            if (ctrl.controlName != "grdDetails")
            {
                if (ctrl.HasAttribute("ActionLeavingRow"))
                {
                    AssignEventHandlerToControl(writer, ctrl, "Selection.FocusRowLeaving", "SourceGrid.RowCancelEventHandler",
                        ctrl.GetAttribute("ActionLeavingRow"));
                }

                if (ctrl.HasAttribute("ActionFocusRow"))
                {
                    AssignEventHandlerToControl(writer, ctrl, "Selection.FocusRowEntered", "SourceGrid.RowEventHandler",
                        ctrl.GetAttribute("ActionFocusRow"));
                }
            }

            if (ctrl.HasAttribute("ActionEnterKeyPressed"))
            {
                AssignEventHandlerToControl(writer, ctrl, "EnterKeyPressed", "TKeyPressedEventHandler",
                    ctrl.GetAttribute("ActionEnterKeyPressed"));
            }

            if ((ctrl.controlName == "grdDetails") && FCodeStorage.HasAttribute("DetailTable"))
            {
                writer.Template.AddToCodelet("SHOWDATA", "");

                if (ctrl.HasAttribute("SortOrder"))
                {
                    // SortOrder is comma separated and has DESC or ASC after the column name
                    string SortOrder = ctrl.GetAttribute("SortOrder");

                    foreach (string SortOrderPart in SortOrder.Split(','))
                    {
                        bool temp;
                        TTableField field = null;
                        string columnNamePart = SortOrderPart.Split(' ')[0];

                        if ((columnNamePart.IndexOf(".") == -1) && ctrl.HasAttribute("TableName"))
                        {
                            field = TDataBinding.GetTableField(null, ctrl.GetAttribute("TableName") + "." + columnNamePart, out temp, true);
                        }
                        else
                        {
                            field = TDataBinding.GetTableField(null, columnNamePart, out temp, true);
                        }

                        if (field != null)
                        {
                            SortOrder = SortOrder.Replace(columnNamePart, field.strName);
                        }
                    }

                    writer.Template.AddToCodelet("GRIDSORT", SortOrder);
                }

                if (ctrl.HasAttribute("RowFilter"))
                {
                    // this references a field in the table, and assumes there exists a local variable with the same name
                    // eg. FBatchNumber in GL Journals
                    string RowFilter = ctrl.GetAttribute("RowFilter");

                    String FilterString = "\"";

                    foreach (string RowFilterPart in RowFilter.Split(','))
                    {
                        bool temp;
                        string columnName =
                            TDataBinding.GetTableField(
                                null,
                                RowFilterPart,
                                out temp, true).strName;

                        if (FilterString.Length > 1)
                        {
                            FilterString += " + \" and ";
                        }

                        FilterString += columnName + " = \" + F" + TTable.NiceFieldName(columnName) + ".ToString()";
                    }

                    writer.Template.AddToCodelet("GRIDFILTER", FilterString);
                }
            }

            if (ctrl.controlName == "grdDetails")
            {
                if (ctrl.HasAttribute("EnableMultiSelection"))
                {
                    writer.Template.SetCodelet("GRIDMULTISELECTION",
                        String.Format("grdDetails.Selection.EnableMultiSelection = {0};{1}", ctrl.GetAttribute("EnableMultiSelection"),
                            Environment.NewLine));
                }
                else if (FCodeStorage.FControlList.ContainsKey("btnDelete"))
                {
                    writer.Template.SetCodelet("GRIDMULTISELECTION",
                        "grdDetails.Selection.EnableMultiSelection = true;" + Environment.NewLine);
                }
            }

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for the winforms data grid
    /// </summary>
    public class WinformsGridGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public WinformsGridGenerator()
            : base("grd", typeof(System.Windows.Forms.DataGridView))
        {
            FGenerateLabel = false;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if (TYml2Xml.GetAttribute(curNode, "Type").ToLower() == "winforms")
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "SelectedRowActivates"))
            {
                // TODO: this function needs to be called by the manual code at the moment when eg a search finishes
                // TODO: call "Activate" + TYml2Xml.GetAttribute(ctrl.xmlNode, "SelectedRowActivates")
            }

            StringCollection Columns = TYml2Xml.GetElements(ctrl.xmlNode, "Columns");

            if (Columns.Count > 0)
            {
                writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".Columns.Clear();" + Environment.NewLine);

                foreach (string ColumnFieldName in Columns)
                {
                    bool IsDetailNotMaster;
                    TTableField field = null;

                    // customfield, eg. UC_GLTransactions, ATransaction.DateEntered and ATransaction.AnalysisAttributes
                    // there needs to be a list of CustomColumns
                    XmlNode CustomColumnsNode = TYml2Xml.GetChild(ctrl.xmlNode, "CustomColumns");
                    XmlNode CustomColumnNode = null;

                    if (CustomColumnsNode != null)
                    {
                        CustomColumnNode = TYml2Xml.GetChild(CustomColumnsNode, ColumnFieldName);
                    }

                    if (CustomColumnNode != null)
                    {
                        //string ColumnType = "System.String";

                        /* TODO DateTime (tracker: #58)
                         * if (TYml2Xml.GetAttribute(CustomColumnNode, "Type") == "System.DateTime")
                         * {
                         *  ColumnType = "DateTime";
                         * }
                         */

                        // TODO: different behaviour for double???
                        if (TYml2Xml.GetAttribute(CustomColumnNode, "Type") == "Boolean")
                        {
                            //ColumnType = "CheckBox";
                        }

                        writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".Columns.Add(" +
                            "FMainDS." + ctrl.GetAttribute("TableName") + ".Get" + ColumnFieldName + "DBName(), \"" +
                            TYml2Xml.GetAttribute(CustomColumnNode, "Label") + "\");" + Environment.NewLine);
                    }
                    else if (ctrl.HasAttribute("TableName"))
                    {
                        field =
                            TDataBinding.GetTableField(null, ctrl.GetAttribute("TableName") + "." + ColumnFieldName,
                                out IsDetailNotMaster,
                                true);
                    }
                    else
                    {
                        field = TDataBinding.GetTableField(null, ColumnFieldName, out IsDetailNotMaster, true);
                    }

                    if (field != null)
                    {
                        //string ColumnType = "System.String";

                        /* TODO DateTime (tracker: #58)
                         * if (field.GetDotNetType() == "System.DateTime")
                         * {
                         *  ColumnType = "DateTime";
                         * }
                         */

                        // TODO: different behaviour for double???
                        if (field.GetDotNetType() == "Boolean")
                        {
                            //ColumnType = "CheckBox";
                        }

                        writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".Columns.Add(" +
                            TTable.NiceTableName(field.strTableName) + "Table.Get" +
                            TTable.NiceFieldName(field.strName) + "DBName(), \"" +
                            field.strLabel + "\");" + Environment.NewLine);
                    }
                }
            }

            if (ctrl.HasAttribute("ActionLeavingRow"))
            {
                AssignEventHandlerToControl(writer, ctrl, "Selection.FocusRowLeaving", "SourceGrid.RowCancelEventHandler",
                    ctrl.GetAttribute("ActionLeavingRow"));
            }

            if (ctrl.HasAttribute("ActionFocusRow"))
            {
// TODO                AssignEventHandlerToControl(writer, ctrl, "Selection.FocusRowEntered", "SourceGrid.RowEventHandler",
//                    ctrl.GetAttribute("ActionFocusRow"));
            }

            if ((ctrl.controlName == "grdDetails") && FCodeStorage.HasAttribute("DetailTable"))
            {
                writer.Template.AddToCodelet("SHOWDATA", "");

                if (ctrl.HasAttribute("SortOrder"))
                {
                    // SortOrder is comma separated and has DESC or ASC after the column name
                    string SortOrder = ctrl.GetAttribute("SortOrder");

                    foreach (string SortOrderPart in SortOrder.Split(','))
                    {
                        bool temp;
                        TTableField field = null;

                        if ((SortOrderPart.Split(' ')[0].IndexOf(".") == -1) && ctrl.HasAttribute("TableName"))
                        {
                            field =
                                TDataBinding.GetTableField(null, ctrl.GetAttribute("TableName") + "." + SortOrderPart.Split(
                                        ' ')[0], out temp, true);
                        }
                        else
                        {
                            field =
                                TDataBinding.GetTableField(
                                    null,
                                    SortOrderPart.Split(' ')[0],
                                    out temp, true);
                        }

                        if (field != null)
                        {
                            SortOrder = SortOrder.Replace(SortOrderPart.Split(' ')[0], field.strName);
                        }
                    }

                    writer.Template.AddToCodelet("GRIDSORT", SortOrder);
                }

                if (ctrl.HasAttribute("RowFilter"))
                {
                    // this references a field in the table, and assumes there exists a local variable with the same name
                    // eg. FBatchNumber in GL Journals
                    string RowFilter = ctrl.GetAttribute("RowFilter");

                    String FilterString = "\"";

                    foreach (string RowFilterPart in RowFilter.Split(','))
                    {
                        bool temp;
                        string columnName =
                            TDataBinding.GetTableField(
                                null,
                                RowFilterPart,
                                out temp, true).strName;

                        if (FilterString.Length > 1)
                        {
                            FilterString += " + \" and ";
                        }

                        FilterString += columnName + " = \" + F" + TTable.NiceFieldName(columnName) + ".ToString()";
                    }

                    writer.Template.AddToCodelet("GRIDFILTER", FilterString);
                }
            }

            return writer.FTemplate;
        }
    }
}