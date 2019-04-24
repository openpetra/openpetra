//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2019 by OM International
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
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MReporting;
using System.Data.Odbc;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO; // Implicit reference
using Ict.Common.Remoting.Server;
using System.IO;
using OfficeOpenXml;
using HtmlAgilityPack;

namespace Ict.Petra.Server.MReporting.Calculator
{
    /// <summary>
    /// calculate a report
    /// </summary>
    public class TRptDataCalculator
    {
        /// <summary>where to search for the standard reports (xml files)</summary>
        protected String FPathStandardReports;

        /// <summary>where to search for the custom reports (xml files)</summary>
        protected String FPathCustomReports;

        /// the HTML template
        protected string FHTMLTemplate;

        /// the HTML Output
        protected HtmlDocument FHtmlDocument;

        /// the Parameters
        protected TParameterList FParameters;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptDataCalculator(String APathStandardReports, String APathCustomReports)
        {
            FPathStandardReports = APathStandardReports;
            FPathCustomReports = APathCustomReports;
        }

        /// <summary>
        /// this is where all the calculations take place
        /// </summary>
        /// <returns>true if the report was successfully generated
        /// </returns>
        public Boolean GenerateResult(ref TParameterList parameterlist,
            ref string strHTMLOutput,
            out HtmlDocument HTMLDocument,
            ref String AErrorMessage,
            ref Exception AException,
            TDBTransaction ATransaction)
        {
            Boolean ReturnValue = false;
            HTMLDocument = null;

            if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_REPORTING)
            {
                // for timing of reports
                TLogging.Log("start calculating", TLoggingType.ToLogfile);
            }

            AErrorMessage = "";
            AException = null;

            try
            {
                this.FParameters = parameterlist;

                this.FHTMLTemplate = LoadReportDefinitionFile(FParameters.Get("htmlfile").ToString());

                if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_REPORTING)
                {
                    FParameters.Save(Path.GetDirectoryName(TSrvSetting.ServerLogFile) + Path.DirectorySeparatorChar + "LogParamAfterPreproc.json");
                }

                // to avoid still having in the status line: loading common.xml, although he is already working on the report
                TLogging.Log("Preparing data for the report... ", TLoggingType.ToStatusBar);

                if (CalculateFromClass(FParameters.Get("calculateFromClass").ToString(), ATransaction))
                {
                    if (FParameters.Get("CancelReportCalculation").ToBool() == true)
                    {
                        AErrorMessage = "Report calculation was cancelled";
                        return false;
                    }

                    HTMLDocument = this.FHtmlDocument;
                    strHTMLOutput = this.FHtmlDocument.DocumentNode.WriteTo();

                    if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_REPORTING)
                    {
                        string FilePath = Path.GetDirectoryName(TSrvSetting.ServerLogFile) + Path.DirectorySeparatorChar;
                        using (StreamWriter sw = new StreamWriter(FilePath + "LogReport.html"))
                        {
                            sw.Write(strHTMLOutput);
                            sw.Close();
                        }
                    }

                    ReturnValue = true;
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log(Exc.ToString());
                TLogging.Log(Exc.StackTrace);

                if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_REPORTING)
                {
                    FParameters.Save(Path.GetDirectoryName(TSrvSetting.ServerLogFile) + Path.DirectorySeparatorChar + "LogAfterException.json");
                }

                Console.WriteLine(Exc.StackTrace);

                AErrorMessage = Exc.Message;
                AException = Exc;
            }

            if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_REPORTING)
            {
                // for timing of reports
                TLogging.Log("finished calculating", TLoggingType.ToLogfile);
            }

            return ReturnValue;
        }

        private SortedList <string, Assembly>FReportAssemblies = new SortedList <string, Assembly>();

        /// <summary>
        /// call a class that calculates the result for a report or extract
        /// </summary>
        protected Boolean CalculateFromClass(string ANamespaceClass, TDBTransaction ATransaction)
        {
            string className = ANamespaceClass.Substring(ANamespaceClass.LastIndexOf(".") + 1);
            string namespaceName = ANamespaceClass.Substring(0, ANamespaceClass.LastIndexOf("."));

            if (!FReportAssemblies.Keys.Contains(namespaceName))
            {
                // work around dlls containing several namespaces, eg Ict.Petra.Client.MFinance.Gui contains AR as well
                string DllName = (TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + namespaceName).ToString().
                                 Replace("Ict.Petra.Server.", "Ict.Petra.Server.lib.");

                if (!System.IO.File.Exists(DllName + ".dll"))
                {
                    DllName = DllName.Substring(0, DllName.LastIndexOf("."));
                }

                try
                {
                    // TLogging.Log("loading dll " + DllName + ".dll");
                    FReportAssemblies.Add(namespaceName, Assembly.LoadFrom(DllName + ".dll"));
                }
                catch (Exception exp)
                {
                    throw new Exception("error loading assembly " + namespaceName + ".dll: " + exp.Message);
                }
            }

            Assembly asm = FReportAssemblies[namespaceName];

            System.Type classType = asm.GetType(namespaceName + "." + className);

            if (classType == null)
            {
                throw new Exception("cannot find class " + namespaceName + "." + className);
            }

            string methodName = "Calculate";
            MethodInfo method = classType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);

            if (method != null)
            {
                object[] mparameters = new object[] { this.FHTMLTemplate, this.FParameters, ATransaction };
                this.FHtmlDocument = (HtmlDocument)method.Invoke(null, mparameters);
                this.FParameters = (TParameterList)mparameters[1];
                return true;
            }
            else
            {
                throw new Exception("cannot find method " + className + "." + methodName);
            }
        }

        /// <summary>
        /// load the HTML file that defines the report
        /// </summary>
        /// <param name="htmlfile">a file name
        /// </param>
        protected string LoadReportDefinitionFile(String htmlfile)
        {
            if (htmlfile.Length == 0)
            {
                throw new Exception("No htmlfile defined to be loaded");
            }

            TLogging.Log("Loading " + htmlfile, TLoggingType.ToStatusBar);

            if (!System.IO.File.Exists(htmlfile) && System.IO.File.Exists(FPathCustomReports + '/' + htmlfile))
            {
                htmlfile = FPathCustomReports + '/' + htmlfile;
            }

            if (!System.IO.File.Exists(htmlfile) && System.IO.File.Exists(FPathStandardReports + '/' + htmlfile))
            {
                htmlfile = FPathStandardReports + '/' + htmlfile;
            }

            if (!System.IO.File.Exists(htmlfile))
            {
                throw new Exception("Error: Cannot find the html file " + htmlfile + " in " + FPathStandardReports + " or " + FPathCustomReports);
            }

            return System.IO.File.ReadAllText(htmlfile);
        }
    }
}
