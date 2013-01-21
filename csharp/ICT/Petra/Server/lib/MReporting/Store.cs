//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using Ict.Common.IO;
using System.Collections;
using System.Collections.Generic;
using Ict.Common;

namespace Ict.Petra.Server.MReporting
{
    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptCalculation
    {
        /// <summary>todoComment</summary>
        public List <TRptValue>rptGrpCaption;

        /// <summary>todoComment</summary>
        public List <TRptValue>rptGrpShortCaption;

        /// <summary>todoComment</summary>
        public List <TRptQuery>rptGrpTemplate;

        /// <summary>todoComment</summary>
        public List <TRptQuery>rptGrpQuery;

        /// <summary>todoComment</summary>
        public string strReturns;

        /// <summary>todoComment</summary>
        public string strAlign;

        /// <summary>todoComment</summary>
        public string strReturnsFormat;

        /// <summary>todoComment</summary>
        public string strId;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptCalculation(int order)
        {
            rptGrpCaption = null;
            rptGrpShortCaption = null;
            rptGrpTemplate = null;
            rptGrpQuery = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptCase
    {
        /// <summary>todoComment</summary>
        public List <TRptLowerLevel>rptGrpLowerLevel;

        /// <summary>todoComment</summary>
        public List <TRptField>rptGrpField;

        /// <summary>todoComment</summary>
        public List <TRptValue>rptGrpValue;

        /// <summary>todoComment</summary>
        public TRptSwitch rptSwitch;

        /// <summary>todoComment</summary>
        public string strCondition;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptCase(int order)
        {
            rptGrpLowerLevel = null;
            rptGrpField = null;
            rptGrpValue = null;
            rptSwitch = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptDetail
    {
        /// <summary>todoComment</summary>
        public TRptSwitch rptSwitch;

        /// <summary>todoComment</summary>
        public List <TRptLowerLevel>rptGrpLowerLevel;

        /// <summary>todoComment</summary>
        public List <TRptField>rptGrpField;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptDetail(int order)
        {
            rptSwitch = null;
            rptGrpLowerLevel = null;
            rptGrpField = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptField
    {
        /// <summary>todoComment</summary>
        public List <TRptValue>rptGrpValue;

        /// <summary>todoComment</summary>
        public List <TRptFieldDetail>rptGrpFieldDetail;

        /// <summary>todoComment</summary>
        public string strWhichfield;

        /// <summary>todoComment</summary>
        public string strCalculation;

        /// <summary>todoComment</summary>
        public string strPos;

        /// <summary>todoComment</summary>
        public string strWidth;

        /// <summary>todoComment</summary>
        public string strFormat;

        /// <summary>todoComment</summary>
        public string strAlign;

        /// <summary>todoComment</summary>
        public string strLine;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptField(int order)
        {
            rptGrpValue = null;
            rptGrpFieldDetail = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptFieldDetail
    {
        /// <summary>todoComment</summary>
        public TRptSwitch rptSwitch;

        /// <summary>todoComment</summary>
        public TRptSwitch rptIf;

        /// <summary>todoComment</summary>
        public List <TRptValue>rptGrpValue;

        /// <summary>todoComment</summary>
        public string strCondition;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptFieldDetail(int order)
        {
            rptSwitch = null;
            rptIf = null;
            rptGrpValue = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptLevel
    {
        /// <summary>todoComment</summary>
        public List <TRptField>rptGrpHeaderField;

        /// <summary>todoComment</summary>
        public List <TRptSwitch>rptGrpHeaderSwitch;

        /// <summary>todoComment</summary>
        public List <TRptField>rptGrpFooterField;

        /// <summary>todoComment</summary>
        public List <TRptSwitch>rptGrpFooterSwitch;

        /// <summary>todoComment</summary>
        public TRptDetail rptDetail;

        /// <summary>todoComment</summary>
        public string strName;

        /// <summary>todoComment</summary>
        public string strIdentification;

        /// <summary>todoComment</summary>
        public string strCondition;

        /// <summary>todoComment</summary>
        public string strFooterLine;

        /// <summary>todoComment</summary>
        public string strFooterSpace;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptLevel(int order)
        {
            rptGrpHeaderField = null;
            rptGrpHeaderSwitch = null;
            rptGrpFooterField = null;
            rptGrpFooterSwitch = null;
            rptDetail = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptLowerLevel
    {
        /// <summary>todoComment</summary>
        public List <TRptParameter>rptGrpParameter;

        /// <summary>todoComment</summary>
        public string strLevel;

        /// <summary>todoComment</summary>
        public string strCalculation;

        /// <summary>todoComment</summary>
        public string strCondition;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptLowerLevel(int order)
        {
            rptGrpParameter = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptParameter
    {
        /// <summary>todoComment</summary>
        public List <TRptValue>rptGrpValue;

        /// <summary>todoComment</summary>
        public string strName;

        /// <summary>todoComment</summary>
        public string strValue;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptParameter(int order)
        {
            rptGrpValue = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptQuery
    {
        /// <summary>todoComment</summary>
        public List <TRptValue>rptGrpValue;

        /// <summary>todoComment</summary>
        public List <TRptParameter>rptGrpParameter;

        /// <summary>todoComment</summary>
        public List <TRptSwitch>rptGrpSwitch;

        /// <summary>todoComment</summary>
        public string strCondition;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptQuery(int order)
        {
            rptGrpValue = null;
            rptGrpParameter = null;
            rptGrpSwitch = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptSwitch
    {
        /// <summary>todoComment</summary>
        public List <TRptCase>rptGrpCases;

        /// <summary>todoComment</summary>
        public TRptCase rptDefault;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptSwitch(int order)
        {
            rptGrpCases = null;
            rptDefault = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptValue
    {
        /// <summary>todoComment</summary>
        public string strCondition;

        /// <summary>todoComment</summary>
        public string strFormat;

        /// <summary>todoComment</summary>
        public string strFunction;

        /// <summary>todoComment</summary>
        public string strVariable;

        /// <summary>todoComment</summary>
        public string strCalculation;

        /// <summary>todoComment</summary>
        public string strText;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptValue(int order)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptDetailReport
    {
        /// <summary>todoComment</summary>
        public string strId;

        /// <summary>todoComment</summary>
        public string strAction;

        /// <summary>todoComment</summary>
        public string strQuery;

        /// <summary>todoComment</summary>
        public List <TRptParameter>rptGrpParameter;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptDetailReport(string AID)
        {
            strId = AID;
            rptGrpParameter = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptReport
    {
        /// <summary>todoComment</summary>
        public string name;

        /// <summary>todoComment</summary>
        public List <TRptField>reportfield;

        /// <summary>todoComment</summary>
        public List <TRptField>pagefield;

        /// <summary>todoComment</summary>
        public List <TRptSwitch>reportswitch;

        /// <summary>todoComment</summary>
        public List <TRptSwitch>pageswitch;

        /// <summary>todoComment</summary>
        public List <TRptDetailReport>rptGrpDetailReport;

        /// <summary>todoComment</summary>
        public List <TRptCalculation>rptGrpCalculation;

        /// <summary>todoComment</summary>
        public List <TRptLevel>rptGrpLevel;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptReport(string name)
        {
            reportfield = null;
            reportswitch = null;
            pagefield = null;
            pageswitch = null;
            this.name = name;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TRptLevel GetLevel(string name)
        {
            TRptLevel element;
            int counter;

            counter = 0;

            if (rptGrpLevel == null)
            {
                return null;
            }

            while (counter < rptGrpLevel.Count)
            {
                element = (TRptLevel)rptGrpLevel[counter];

                if (element.strName == name)
                {
                    return element;
                }

                counter++;
            }

            TLogging.Log("could not find level \"" + name + "\"");
            throw new Exception("could not find level \"" + name + "\"");
        }

        /// <summary>
        /// get the calcuation with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TRptCalculation GetCalculation(string name)
        {
            TRptCalculation ReturnValue;
            TRptCalculation element;
            int counter;

            counter = 0;
            ReturnValue = null;

            if (rptGrpCalculation == null)
            {
                return null;
            }

            while (counter < rptGrpCalculation.Count)
            {
                element = (TRptCalculation)rptGrpCalculation[counter];

                if (element.strId.ToLower() == name.ToLower())
                {
                    return element;
                }

                counter++;
            }

            return ReturnValue;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TReportDefinition
    {
        /// <summary>todoComment</summary>
        public string id;

        /// <summary>todoComment</summary>
        public TRptReport rptReport;

        /// <summary>todoComment</summary>
        public int linesId;

        /// <summary>todoComment</summary>
        public int fieldsId;

        /// <summary>todoComment</summary>
        public int valuesId;

        /// <summary>todoComment</summary>
        public int reportParametersId;

        /// <summary>todoComment</summary>
        public int levelsId;

        /// <summary>todoComment</summary>
        public int calculationsId;

        /// <summary>todoComment</summary>
        public int detailsId;

        /// <summary>todoComment</summary>
        public int lowerReportsId;

        /// <summary>todoComment</summary>
        public int switchesId;

        /// <summary>todoComment</summary>
        public int casesId;

        /// <summary>todoComment</summary>
        public int parametersId;

        /// <summary>todoComment</summary>
        public int queriesId;

        /// <summary>todoComment</summary>
        public int fieldDetailsId;

        /// <summary>todoComment</summary>
        public int detailReportsId;

        /// <summary>
        /// constructor
        /// </summary>
        public TReportDefinition(string reportId) : base()
        {
            id = reportId;
            rptReport = new TRptReport(reportId);
        }

        /// <summary>
        /// clear the local variables
        /// </summary>
        public void Clear()
        {
            linesId = 0;
            fieldsId = 0;
            valuesId = 0;
            reportParametersId = 0;
            levelsId = 0;
            calculationsId = 0;
            detailReportsId = 0;
            detailsId = 0;
            lowerReportsId = 0;
            switchesId = 0;
            casesId = 0;
            parametersId = 0;
            queriesId = 0;
            fieldDetailsId = 0;
        }

        /// <summary>
        /// get the current report
        /// </summary>
        /// <returns></returns>
        public TRptReport GetReport()
        {
            return rptReport;
        }
    }

    /// <summary>
    /// the main class for the report definition;
    /// stores the parsed XML file in memory, provides easy object orientated access
    /// </summary>
    public class TReportStore
    {
        private ArrayList reports;

        /// <summary>
        /// constructor
        /// </summary>
        public TReportStore() : base()
        {
            reports = new ArrayList();
        }

        /// <summary>
        /// clear reports
        /// </summary>
        public void Clear()
        {
            int counter;

            counter = 0;

            while (counter < reports.Count)
            {
                reports.RemoveAt(counter);
                counter = 0;
            }
        }

        /// <summary>
        /// clear one specific report
        /// </summary>
        /// <param name="reportId"></param>
        public void Clear(string reportId)
        {
            TReportDefinition element;
            int counter;

            counter = 0;

            while (counter < reports.Count)
            {
                element = (TReportDefinition)reports[counter];

                if (element.id == reportId)
                {
                    reports.RemoveAt(counter);
                    return;
                }

                counter++;
            }
        }

        /// <summary>
        /// add a report
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public TReportDefinition Add(string reportId)
        {
            TReportDefinition element;

            Clear(reportId);
            element = new TReportDefinition(reportId);
            reports.Add(element);
            return element;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public TRptReport Get(string reportId)
        {
            TRptReport ReturnValue;
            TReportDefinition element;
            int counter;

            counter = 0;
            ReturnValue = null;

            while (counter < reports.Count)
            {
                element = (TReportDefinition)reports[counter];

                if (element.id == reportId)
                {
                    return element.GetReport();
                }

                counter++;
            }

            return ReturnValue;
        }

        // if report has no calculation with this name,
        // a calculation from any report is selected, and the calculation is returned

        /// <summary>
        /// if report has no calculation with this name,
        /// a calculation from any report is selected, and the calculation is returned
        /// </summary>
        /// <returns>void</returns>
        public TRptCalculation GetCalculation(TRptReport report, string name)
        {
            TRptCalculation ReturnValue;
            TReportDefinition element;
            int counter;

            ReturnValue = null;

            if (report != null)
            {
                ReturnValue = report.GetCalculation(name);
            }

            if (ReturnValue == null)
            {
                counter = 0;

                while (counter < reports.Count)
                {
                    element = (TReportDefinition)reports[counter];
                    ReturnValue = element.rptReport.GetCalculation(name);

                    if (ReturnValue != null)
                    {
                        return ReturnValue;
                    }

                    counter++;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// this function is used for calculations that are based on a function, e.g. variance
        /// it returns true, if the calcution returns "functionresult"
        /// </summary>
        /// <returns>void</returns>
        public Boolean IsFunctionCalculation(TRptReport report, string name)
        {
            Boolean ReturnValue;
            TRptCalculation rptCalculation;

            ReturnValue = true;

            if (name.Length == 0)
            {
                return false;
            }

            rptCalculation = GetCalculation(report, name);

            if (rptCalculation != null)
            {
                ReturnValue = (rptCalculation.strReturns == "functionresult");
            }

            return ReturnValue;
        }
    }
}