//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//     Tim Ingham
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
using System.Data;
using System.Windows.Forms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;
using System.Collections;
using Ict.Common;
using System.Reflection;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Data;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// If the FastReports DLL can be loaded, this object insinuates FastReports into the GUI via the PetraUtilsObject,
    /// otherwise it does nothing.
    /// </summary>
    public class FastReportsWrapper
    {
        TFrmPetraReportingUtils FPetraUtilsObject;
        /// <summary>
        /// Delegate for getting data from the server and into the report
        /// </summary>
        /// <param name="ACalc"></param>
        /// <returns>true if the data is OK. (If it's not OK, the method should have told the user why not!)</returns>
        public delegate bool TDataGetter (TRptCalculator ACalc);
        private TDataGetter FDataGetter;
        private Assembly FastReportsDll;
        private object FfastReportInstance;

        Type FFastReportType;
        /// <summary>
        /// Use this to check whether loading the FastReports DLL worked.
        /// </summary>
        public Boolean LoadedOK;

        private SReportTemplateRow FSelectedTemplate = null;

        private enum TInitState
        {
            Unknown, LoadDll, LoadTemplate, InitSystem, LoadedOK
        };
        private TInitState FInitState;

        /// <summary>
        /// Use This template for the report
        /// </summary>
        /// <param name="ATemplate"></param>
        public void SetTemplate(SReportTemplateRow ATemplate)
        {
            FSelectedTemplate = ATemplate;
            FPetraUtilsObject.SetWindowTitle();
        }

        /// <summary>The Template Name will be written to the UI title bar</summary>
        public string SelectedTemplateName
        {
            get
            {
                if (!LoadedOK || (FSelectedTemplate == null))
                {
                    return "";
                }

                return String.Format(" [{0}]", FSelectedTemplate.ReportVariant);
            }
        }

        /// <summary>
        /// FastReports uses this function to prepare the Dataset.
        /// The DataGetter function will be called for "GenerateReport" or "DesignReport".
        /// It should make calls back to RegisterData, below.
        /// </summary>
        /// <param name="DataGetter"></param>
        public void SetDataGetter(TDataGetter DataGetter)
        {
            if (LoadedOK)
            {
                FDataGetter = DataGetter;
            }
            else
            {
                ShowErrorPopup();
            }
        }

        /// <summary>
        /// Instance this object and it changes the behaviour to use FastReports if the DLL is installed.
        /// </summary>
        /// <param name="PetraUtilsObject"></param>
        public FastReportsWrapper(TFrmPetraReportingUtils PetraUtilsObject)
        {
            try
            {
                LoadedOK = false;
                FInitState = TInitState.LoadDll;
                FDataGetter = null;
                FPetraUtilsObject = PetraUtilsObject;
                FastReportsDll = Assembly.LoadFrom("FastReport.DLL"); // If there's no FastReports DLL, this will "fall at the first hurdle"!
                SReportTemplateTable TemplateTable = TRemote.MReporting.WebConnectors.GetTemplateVariants(FPetraUtilsObject.FReportName,
                    UserInfo.GUserInfo.UserID,
                    true);

                if (TemplateTable.Rows.Count == 0)
                {
                    FInitState = TInitState.LoadTemplate;
                    TLogging.Log("No FastReports template for " + FPetraUtilsObject.FReportName);
                    return;
                }

                FInitState = TInitState.InitSystem;
                FfastReportInstance = FastReportsDll.CreateInstance("FastReport.Report");
                FFastReportType = FfastReportInstance.GetType();
                FFastReportType.GetProperty("StoreInResources").SetValue(FfastReportInstance, false, null);
                SetTemplate(TemplateTable[0]);

                FPetraUtilsObject.DelegateGenerateReportOverride = GenerateReport;
                FPetraUtilsObject.DelegateViewReportOverride = DesignReport;
                FPetraUtilsObject.DelegateCancelReportOverride = CancelReportGeneration;

                FPetraUtilsObject.EnableDisableSettings(false);
                FInitState = TInitState.LoadedOK;
                LoadedOK = true;
            }
            catch (Exception e) // If there's no FastReports DLL, this object will do nothing.
            {
                TLogging.Log("FastReports Wrapper Not loaded: " + e.Message);
            }
        }

        /// <summary>
        /// If the wrapper didn't initialise, the caller can call this.
        /// </summary>
        public void ShowErrorPopup()
        {
            if (FPetraUtilsObject.GetCallerForm() != null)
            {
                String Msg = "";

                switch (FInitState)
                {
                    case TInitState.LoadDll:
                    {
                        Msg = "Failed to load FastReport Dll.";
                        break;
                    }

                    case TInitState.LoadTemplate:
                    {
                        Msg = String.Format("No reporting template found for {0}.", FPetraUtilsObject.FReportName);
                        break;
                    }

                    case TInitState.InitSystem:
                    {
                        Msg = "The DLL failed to initialise.";
                        break;
                    }

                    default:
                    {
                        return;     // Anything else is not an error...
                    }
                }

                MessageBox.Show("The FastReports plugin did not initialise\r\n" + Msg, "Reporting engine");
            }
        }

        //
        // Called on Cancel button:
        private void CancelReportGeneration(TRptCalculator ACalc)
        {
            TRemote.MReporting.WebConnectors.CancelDataTableGeneration();
        }

        /// <summary>
        /// Call the FastReport method of the same name
        /// This should only be called from the "DataGetter" method. It can only succeed if FastReports initialised correctly.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        public void RegisterData(DataTable data, string name)
        {
            FFastReportType.GetMethod("RegisterData", new Type[] { data.GetType(), name.GetType() }).Invoke(FfastReportInstance,
                new object[] { data, name });
        }

        private void LoadReportParams(TRptCalculator ACalc)
        {
            ArrayList reportParam = ACalc.GetParameters().Elems;
            MethodInfo FastReport_SetParameterValue = FFastReportType.GetMethod("SetParameterValue");

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation"))
                {
                    FastReport_SetParameterValue.Invoke(FfastReportInstance, new object[] { p.name, p.value.ToObject() });
                }
            }
        }

        private void DesignReport(TRptCalculator ACalc)
        {
            if ((FSelectedTemplate != null) && (FDataGetter != null) && FDataGetter(ACalc))
            {
                FFastReportType.GetMethod("LoadFromString", new Type[] { FSelectedTemplate.XmlText.GetType() }).Invoke(FfastReportInstance,
                    new object[] { FSelectedTemplate.XmlText });

                LoadReportParams(ACalc);
                FFastReportType.GetMethod("Design", new Type[0]).Invoke(FfastReportInstance, null);

                //
                // The user can change the report template - if it's changed I'll update the server
                // (unless the template is read-only, in which case I'll need to make a copy.)
                object ret = FFastReportType.GetMethod("SaveToString", new Type[0]).Invoke(FfastReportInstance, null);
                String XmlString = (String)ret;
                //
                // I only want to check part of the report to assess whether it's changed, otherwise it always detects a change
                // (the modified date is changed, and the parameters may also be different.)

                Boolean TemplateChanged = false;
                Int32 Page1Pos = XmlString.IndexOf("<ReportPage");
                Int32 PrevPage1Pos = FSelectedTemplate.XmlText.IndexOf("<ReportPage");

                if ((Page1Pos < 1) || (PrevPage1Pos < 1))
                {
                    TemplateChanged = true;
                }
                else
                {
                    if (XmlString.Substring(Page1Pos) != FSelectedTemplate.XmlText.Substring(PrevPage1Pos))
                    {
                        TemplateChanged = true;
                    }
                }

                if (TemplateChanged)
                {
                    Boolean MakeACopy = false;

                    if (FSelectedTemplate.Readonly)
                    {
                        if (MessageBox.Show(
                                String.Format(Catalog.GetString("{0} cannot be ovewritten.\r\nMake a copy instead?"), FSelectedTemplate.ReportVariant),
                                Catalog.GetString("Design Template"),
                                MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }

                        MakeACopy = true;
                    }
                    else
                    {
                        if (MessageBox.Show(
                                String.Format(Catalog.GetString("Save changes to {0}?"), FSelectedTemplate.ReportVariant),
                                Catalog.GetString("Design Template"),
                                MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    SReportTemplateTable TemplateTable = new SReportTemplateTable();
                    SReportTemplateRow NewRow = TemplateTable.NewRowTyped();
                    DataUtilities.CopyAllColumnValues(FSelectedTemplate, NewRow);
                    TemplateTable.Rows.Add(NewRow);

                    if (MakeACopy)
                    {
                        NewRow.TemplateId = -1; // The value will come from the sequence
                        NewRow.ReportVariant = "Copy of " + TemplateTable[0].ReportVariant;
                        NewRow.Readonly = false;
                        NewRow.Default = false;
                        NewRow.PrivateDefault = false;
                    }
                    else
                    {
                        TemplateTable.AcceptChanges(); // Don't allow this one-row table to be seen as "new"
                    }

                    NewRow.XmlText = XmlString;
                    SReportTemplateTable Tbl = TRemote.MReporting.WebConnectors.SaveTemplates(TemplateTable);
                    Tbl.AcceptChanges();
                    SetTemplate(Tbl[0]);
                }
            }

            FPetraUtilsObject.UpdateParentFormEndOfReport();
        }

        private void GenerateReport(TRptCalculator ACalc)
        {
            if ((FSelectedTemplate != null) && (FDataGetter != null) && FDataGetter(ACalc))
            {
                FFastReportType.GetMethod("LoadFromString", new Type[] { FSelectedTemplate.XmlText.GetType() }).Invoke(FfastReportInstance,
                    new object[] { FSelectedTemplate.XmlText });
                LoadReportParams(ACalc);
                FFastReportType.GetMethod("Show", new Type[0]).Invoke(FfastReportInstance, null);
            }

            FPetraUtilsObject.UpdateParentFormEndOfReport();
        }
    }
}