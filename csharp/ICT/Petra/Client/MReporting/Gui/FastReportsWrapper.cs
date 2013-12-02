using System;
using System.Data;
using System.Windows.Forms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;
using System.Collections;
using Ict.Common;
using System.Reflection;

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
        /// Instance this object and it changes the behaviour to use FastReports if the DLL is installed.
        /// </summary>
        /// <param name="PetraUtilsObject"></param>
        /// <param name="DataGetter"></param>
        public FastReportsWrapper(TFrmPetraReportingUtils PetraUtilsObject, TDataGetter DataGetter)
        {
            try
            {
                FPetraUtilsObject = PetraUtilsObject;
                FastReportsDll = Assembly.LoadFrom("FastReport.DLL"); // If there's no FastReports DLL, this will "fall at the first hurdle"!
                String reportPath = TAppSettingsManager.GetValue("Reporting.PathStandardReports") + "/Finance/" + FPetraUtilsObject.FReportName + ".frx";

                FDataGetter = DataGetter;
                FfastReportInstance = FastReportsDll.CreateInstance("FastReport.Report");
                FFastReportType = FfastReportInstance.GetType();
                FFastReportType.GetProperty("StoreInResources").SetValue(FfastReportInstance, false, null);
                FFastReportType.GetMethod("Load", new Type[] { reportPath.GetType() }).Invoke(FfastReportInstance, new object[] { reportPath });

                FPetraUtilsObject.DelegateGenerateReportOverride = GenerateReport;
                FPetraUtilsObject.DelegateViewReportOverride = DesignReport;
            }
            catch (Exception) // If there's no FastReports DLL, this object will do nothing.
            {
            }
        }

        /// <summary>
        /// Call the FastReport method of the same name
        /// This should only be called from the "DataGetter" method. It can only succeed if FastReports initialised correctly.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        public void RegisterData(DataTable data, string name)
        {
            FFastReportType.GetMethod("RegisterData", new Type[] { data.GetType(), name.GetType() }).Invoke(FfastReportInstance, new object[] { data, name });
        }

        private void LoadReportParams(TRptCalculator ACalc)
        {
            ArrayList reportParam = ACalc.GetParameters().Elems;
            MethodInfo FastReport_SetParameterValue = FFastReportType.GetMethod("SetParameterValue");
            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && p.name != "param_calculation")
                {
                    FastReport_SetParameterValue.Invoke(FfastReportInstance, new object[]{ p.name, p.value.ToObject()});
                }
            }
        }

        private void DesignReport(TRptCalculator ACalc)
        {
            FFastReportType.GetMethod("Design", new Type[0]).Invoke(FfastReportInstance, null);
        }

        private void GenerateReport(TRptCalculator ACalc)
        {
            if (FDataGetter(ACalc))
            {
                LoadReportParams(ACalc);
                //DesignReport(null, null);
                FFastReportType.GetMethod("Show", new Type[0]).Invoke(FfastReportInstance, null);
                FPetraUtilsObject.UpdateParentFormEndOfReport();
            }
        }
    }
}
