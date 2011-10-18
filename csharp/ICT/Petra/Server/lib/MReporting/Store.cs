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
using Ict.Common.IO;
using System.Collections;
using Ict.Common;

namespace Ict.Petra.Server.MReporting
{
    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptGrpCalculation : TXMLGroup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptGrpCalculation(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptGrpCase : TXMLGroup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptGrpCase(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptGrpDetail : TXMLGroup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptGrpDetail(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptGrpField : TXMLGroup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptGrpField(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptGrpFieldDetail : TXMLGroup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptGrpFieldDetail(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptGrpLevel : TXMLGroup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptGrpLevel(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptGrpLowerLevel : TXMLGroup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptGrpLowerLevel(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptGrpParameter : TXMLGroup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptGrpParameter(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptGrpQuery : TXMLGroup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptGrpQuery(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptGrpSwitch : TXMLGroup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptGrpSwitch(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptGrpValue : TXMLGroup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptGrpValue(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptGrpDetailReport : TXMLGroup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptGrpDetailReport(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptCalculation : TXMLElement
    {
        /// <summary>todoComment</summary>
        public TRptGrpValue rptGrpCaption;

        /// <summary>todoComment</summary>
        public TRptGrpValue rptGrpShortCaption;

        /// <summary>todoComment</summary>
        public TRptGrpQuery rptGrpTemplate;

        /// <summary>todoComment</summary>
        public TRptGrpQuery rptGrpQuery;

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
        public TRptCalculation(int order) : base(order)
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
    public class TRptCase : TXMLElement
    {
        /// <summary>todoComment</summary>
        public TRptGrpLowerLevel rptGrpLowerLevel;

        /// <summary>todoComment</summary>
        public TRptGrpField rptGrpField;

        /// <summary>todoComment</summary>
        public TRptGrpValue rptGrpValue;

        /// <summary>todoComment</summary>
        public TRptSwitch rptSwitch;

        /// <summary>todoComment</summary>
        public string strCondition;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptCase(int order) : base(order)
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
    public class TRptDetail : TXMLElement
    {
        /// <summary>todoComment</summary>
        public TRptSwitch rptSwitch;

        /// <summary>todoComment</summary>
        public TRptGrpLowerLevel rptGrpLowerLevel;

        /// <summary>todoComment</summary>
        public TRptGrpField rptGrpField;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptDetail(int order) : base(order)
        {
            rptSwitch = null;
            rptGrpLowerLevel = null;
            rptGrpField = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptField : TXMLElement
    {
        /// <summary>todoComment</summary>
        public TRptGrpValue rptGrpValue;

        /// <summary>todoComment</summary>
        public TRptGrpFieldDetail rptGrpFieldDetail;

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
        public TRptField(int order) : base(order)
        {
            rptGrpValue = null;
            rptGrpFieldDetail = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptFieldDetail : TXMLElement
    {
        /// <summary>todoComment</summary>
        public TRptSwitch rptSwitch;

        /// <summary>todoComment</summary>
        public TRptSwitch rptIf;

        /// <summary>todoComment</summary>
        public TRptGrpValue rptGrpValue;

        /// <summary>todoComment</summary>
        public string strCondition;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptFieldDetail(int order) : base(order)
        {
            rptSwitch = null;
            rptIf = null;
            rptGrpValue = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptLevel : TXMLElement
    {
        /// <summary>todoComment</summary>
        public TRptGrpField rptGrpHeaderField;

        /// <summary>todoComment</summary>
        public TRptGrpSwitch rptGrpHeaderSwitch;

        /// <summary>todoComment</summary>
        public TRptGrpField rptGrpFooterField;

        /// <summary>todoComment</summary>
        public TRptGrpSwitch rptGrpFooterSwitch;

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
        public TRptLevel(int order) : base(order)
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
    public class TRptLowerLevel : TXMLElement
    {
        /// <summary>todoComment</summary>
        public TRptGrpParameter rptGrpParameter;

        /// <summary>todoComment</summary>
        public string strLevel;

        /// <summary>todoComment</summary>
        public string strCalculation;

        /// <summary>todoComment</summary>
        public string strCondition;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptLowerLevel(int order) : base(order)
        {
            rptGrpParameter = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptParameter : TXMLElement
    {
        /// <summary>todoComment</summary>
        public TRptGrpValue rptGrpValue;

        /// <summary>todoComment</summary>
        public string strName;

        /// <summary>todoComment</summary>
        public string strValue;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptParameter(int order) : base(order)
        {
            rptGrpValue = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptQuery : TXMLElement
    {
        /// <summary>todoComment</summary>
        public TRptGrpValue rptGrpValue;

        /// <summary>todoComment</summary>
        public TRptGrpParameter rptGrpParameter;

        /// <summary>todoComment</summary>
        public TRptGrpSwitch rptGrpSwitch;

        /// <summary>todoComment</summary>
        public string strCondition;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptQuery(int order) : base(order)
        {
            rptGrpValue = null;
            rptGrpParameter = null;
            rptGrpSwitch = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptSwitch : TXMLElement
    {
        /// <summary>todoComment</summary>
        public TRptGrpCase rptGrpCases;

        /// <summary>todoComment</summary>
        public TRptCase rptDefault;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptSwitch(int order) : base(order)
        {
            rptGrpCases = null;
            rptDefault = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptValue : TXMLElement
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
        public TRptValue(int order) : base(order)
        {
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptDetailReport : TXMLElement
    {
        /// <summary>todoComment</summary>
        public string strId;

        /// <summary>todoComment</summary>
        public string strAction;

        /// <summary>todoComment</summary>
        public string strQuery;

        /// <summary>todoComment</summary>
        public TRptGrpParameter rptGrpParameter;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptDetailReport(string AID) : base(-1)
        {
            strId = AID;
            rptGrpParameter = null;
        }
    }

    /// <summary>
    /// todoCommment
    /// </summary>
    public class TRptReport : TXMLElement
    {
        /// <summary>todoComment</summary>
        public string name;

        /// <summary>todoComment</summary>
        public TRptGrpField reportfield;

        /// <summary>todoComment</summary>
        public TRptGrpField pagefield;

        /// <summary>todoComment</summary>
        public TRptGrpSwitch reportswitch;

        /// <summary>todoComment</summary>
        public TRptGrpSwitch pageswitch;

        /// <summary>todoComment</summary>
        public TRptGrpDetailReport rptGrpDetailReport;

        /// <summary>todoComment</summary>
        public TRptGrpCalculation rptGrpCalculation;

        /// <summary>todoComment</summary>
        public TRptGrpLevel rptGrpLevel;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptReport(string name) : base(-1)
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

            while (counter < rptGrpLevel.List.Count)
            {
                element = (TRptLevel)rptGrpLevel.List[counter];

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

            while (counter < rptGrpCalculation.List.Count)
            {
                element = (TRptCalculation)rptGrpCalculation.List[counter];

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