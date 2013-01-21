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
using System.Xml;
using System.Collections.Generic;
using Ict.Common;
using System.Globalization;

namespace Ict.Petra.Server.MReporting
{
    /// <summary>
    /// a parser for the XML file that defines a report
    /// </summary>
    public class TReportParser : TXMLParser
    {
        private TReportStore myStore;

        /// <summary>the currently processed reportId</summary>
        private string reportId;

        /// <summary>the currently processed report</summary>
        private TReportDefinition report;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="withValidation"></param>
        public TReportParser(string filename, Boolean withValidation) :
            base(filename, withValidation)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="filename"></param>
        public TReportParser(string filename) : base(filename)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AReuseParser"></param>
        public TReportParser(TXMLParser AReuseParser) : base(AReuseParser)
        {
        }

        /// <summary>
        /// Knows how to parse all the entities in the document.
        /// It reads a group of entities.
        /// </summary>
        /// <param name="cur2">The current node in the document that should be parsed</param>
        /// <param name="groupId">The id of the previous group System.Object of this type</param>
        /// <param name="entity">The name of the elements that make up the group</param>
        /// <param name="newcur">After parsing, the then current node is returned in this parameter</param>
        /// <returns>The group of elements, or nil</returns>
        protected Object ParseGroup(XmlNode cur2, ref int groupId, string entity, out XmlNode newcur)
        {
            Object ReturnValue;
            int order;
            int currentGroup;
            XmlNode cur;

            newcur = cur2;
            cur = NextNotBlank(cur2);
            ReturnValue = null;

            if ((cur == null) || (cur.Name != entity))
            {
                return null;
            }

            groupId++;
            currentGroup = groupId;
            order = 0;

            while ((cur != null) && (cur.Name == entity))
            {
                if (entity == "value")
                {
                    ReturnValue = ParseValueGroup(cur, (List <TRptValue> )ReturnValue, currentGroup, order);
                }
                else if (entity == "calculation")
                {
                    ReturnValue = ParseCalculationGroup(cur, (List <TRptCalculation> )ReturnValue, currentGroup, order);
                }
                else if (entity == "field")
                {
                    ReturnValue = ParseFieldGroup(cur, (List <TRptField> )ReturnValue, currentGroup, order);
                }
                else if (entity == "fielddetail")
                {
                    ReturnValue = ParseFieldDetailGroup(cur, (List <TRptFieldDetail> )ReturnValue, currentGroup, order);
                }
                else if (entity == "level")
                {
                    ReturnValue = ParseLevelGroup(cur, (List <TRptLevel> )ReturnValue, currentGroup, order);
                }
                else if (entity == "lowerLevelReport")
                {
                    ReturnValue = ParseLowerLevelGroup(cur, (List <TRptLowerLevel> )ReturnValue, currentGroup, order);
                }
                else if (entity == "switch")
                {
                    ReturnValue = ParseSwitchGroup(cur, (List <TRptSwitch> )ReturnValue, currentGroup, order);
                }
                else if (entity == "if")
                {
                    ReturnValue = ParseIfGroup(cur, (List <TRptSwitch> )ReturnValue, currentGroup, order);
                }
                else if (entity == "case")
                {
                    ReturnValue = ParseCaseGroup(cur, (List <TRptCase> )ReturnValue, currentGroup, order);
                }
                else if (entity == "queryDetail")
                {
                    ReturnValue = ParseQueryGroup(cur, (List <TRptQuery> )ReturnValue, currentGroup, order);
                }
                else if (entity == "parameter")
                {
                    ReturnValue = ParseParameterGroup(cur, (List <TRptParameter> )ReturnValue, currentGroup, order);
                }
                else if (entity == "detailreport")
                {
                    ReturnValue = ParseDetailReportGroup(cur, (List <TRptDetailReport> )ReturnValue, currentGroup, order);
                }
                else if (entity == "report")
                {
                    ParseReport(cur);
                }
                else if (entity == "reportparameter")
                {
                }
                // do nothing
                else
                {
                    TLogging.Log("missing something? group " + entity);
                }

                order++;
                cur = NextNotBlank(cur.NextSibling);
            }

            newcur = cur;
            return ReturnValue;
        }

        /// <summary>
        /// Knows how to parse all the entities in the document.
        /// It reads a group of entities.
        /// </summary>
        /// <param name="cur2">The current node in the document that should be parsed</param>
        /// <param name="groupId">The id of the previous group System.Object of this type</param>
        /// <param name="entity">The name of the elements that make up the group</param>
        /// <returns>The group of elements, or nil
        /// </returns>
        protected Object ParseGroup(XmlNode cur2, ref int groupId, string entity)
        {
            XmlNode newNode;

            return ParseGroup(cur2, ref groupId, entity, out newNode);
        }

        /// <summary>
        /// If the current node holds a entity of type groupentity,
        /// then the contained elements of type entity are parsed.
        /// </summary>
        /// <param name="groupentity">The name of the entity that holds the group of elements</param>
        /// <param name="cur">The current node</param>
        /// <param name="groupId">The current id for the group.
        /// This number will be increased and then used to create the new group.</param>
        /// <param name="entity">The name of the entitiy that represents the elements</param>
        /// <returns>The group of elements, or nil if node does not point to that group entity or the group entity is empty.
        /// </returns>
        protected Object Parse(string groupentity, ref XmlNode cur, ref int groupId, string entity)
        {
            Object ReturnValue;

            ReturnValue = null;

            if (cur.Name == groupentity)
            {
                cur = NextNotBlank(cur);
                ReturnValue = ParseGroup(cur.FirstChild, ref groupId, entity);
                cur = GetNextEntity(cur);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Knows how to parse all the entities in the document.
        /// Reads one or more entities of the same type.
        /// </summary>
        /// <param name="cur">The current node</param>
        /// <param name="groupId">The id of the current group that the elements will belong to</param>
        /// <param name="entity">The name of the entity that should be read</param>
        /// <returns>the last of the read entities
        /// </returns>
        protected Object Parse(XmlNode cur, ref int groupId, string entity)
        {
            Object ReturnValue;
            int order;

            cur = NextNotBlank(cur);
            ReturnValue = null;

            if ((cur == null) || (cur.Name.ToLower() != entity.ToLower()))
            {
                return null;
            }

            groupId++;
            order = 0;

            while ((cur != null) && (cur.Name.ToLower() == entity.ToLower()))
            {
                if (entity == "detail")
                {
                    ReturnValue = ParseDetail(cur, order);
                }
                else
                {
                    if (entity == "default")
                    {
                        ReturnValue = ParseCase(cur, order);
                    }
                    else
                    {
                        if (entity == "case")
                        {
                            ReturnValue = ParseCase(cur, order);
                        }
                        else
                        {
                            if (entity == "if")
                            {
                                ReturnValue = ParseCase(cur, order);
                            }
                            else
                            {
                                if (entity == "switch")
                                {
                                    ReturnValue = ParseSwitch(cur, order);
                                }
                                else
                                {
                                    if (entity == "lowerlevelreport")
                                    {
                                        ReturnValue = ParseLowerLevel(cur, order);
                                    }
                                    else
                                    {
                                        if (entity == "report")
                                        {
                                            ReturnValue = ParseReport(cur);
                                        }
                                        else
                                        {
                                            if (entity == "description")
                                            {
                                            }
                                            // do nothing
                                            else
                                            {
                                                TLogging.Log("missing something? single " + entity);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                order++;
                cur = NextNotBlank(cur.NextSibling);
            }

            return ReturnValue;
        }

        /// <summary>
        /// main function
        /// </summary>
        /// <param name="myStore"></param>
        /// <returns></returns>
        public Boolean ParseDocument(ref TReportStore myStore)
        {
            Boolean ReturnValue;
            int dummy;
            XmlNode startNode;

            this.myStore = myStore;
            dummy = 0;
            ReturnValue = false;
            startNode = myDoc.DocumentElement;

            if (startNode.Name.ToLower() == "reports")
            {
                ReturnValue = (ParseGroup(startNode.FirstChild, ref dummy, "report") != null);
            }

            return ReturnValue;
        }

        /// <summary>
        /// specific for reports
        /// </summary>
        /// <returns>void</returns>
        protected Object ParseReport(XmlNode cur2)
        {
            XmlNode cur;
            int dummy = -1;
            Object rg;

            cur = cur2;
            reportId = GetAttribute(cur, "id");
            report = myStore.Add(reportId);
            cur = NextNotBlank(cur.FirstChild);
            Parse(cur, ref dummy, "description");
            cur = GetNextEntity(cur);
            Parse("reportparameters", ref cur, ref report.reportParametersId, "reportparameter");

            if (cur.Name == "reportheader")
            {
                ParseFooterHeader(NextNotBlank(cur.FirstChild), out report.rptReport.reportfield, out report.rptReport.reportswitch);
                cur = GetNextEntity(cur);
            }

            if (cur.Name == "pageheader")
            {
                ParseFooterHeader(NextNotBlank(cur.FirstChild), out report.rptReport.pagefield, out report.rptReport.pageswitch);
                cur = GetNextEntity(cur);
            }

            if (cur.Name == "detailreports")
            {
                rg = ParseGroup(cur.FirstChild, ref report.detailReportsId, "detailreport");

                if (rg != null)
                {
                    report.rptReport.rptGrpDetailReport = (List <TRptDetailReport> )rg;
                }

                cur = GetNextEntity(cur);
            }

            rg = ParseGroup(cur.FirstChild, ref report.calculationsId, "calculation");

            if (rg != null)
            {
                report.rptReport.rptGrpCalculation = (List <TRptCalculation> )rg;
            }

            cur = GetNextEntity(cur);
            rg = ParseGroup(cur.FirstChild, ref report.levelsId, "level");

            if (rg != null)
            {
                report.rptReport.rptGrpLevel = (List <TRptLevel> )rg;
            }

            return report.rptReport;
        }

        /// <summary>
        /// parse the header and footer of the report
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="field"></param>
        /// <param name="ASwitch"></param>
        protected void ParseFooterHeader(XmlNode cur2, out List <TRptField>field, out List <TRptSwitch>ASwitch)
        {
            XmlNode cur;
            Object rg;

            cur = cur2;
            field = null;
            ASwitch = null;
            rg = ParseGroup(cur, ref report.fieldsId, "field", out cur);

            if (rg != null)
            {
                field = (List <TRptField> )rg;
            }

            rg = ParseGroup(cur, ref report.switchesId, "switch", out cur);

            if (rg != null)
            {
                ASwitch = (List <TRptSwitch> )rg;
            }
            else
            {
                rg = ParseGroup(cur, ref report.switchesId, "if", out cur);

                if (rg != null)
                {
                    ASwitch = (List <TRptSwitch> )rg;
                }
            }
        }

        /// <summary>
        /// parse a group of fields
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="group"></param>
        /// <param name="groupId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected List <TRptField>ParseFieldGroup(XmlNode cur, List <TRptField>group, int groupId, int order)
        {
            TRptField element;

            if (group == null)
            {
                group = new List <TRptField>(groupId);
            }

            element = ParseField(cur, order);
            group.Add(element);
            return group;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="group"></param>
        /// <param name="groupId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected List <TRptFieldDetail>ParseFieldDetailGroup(XmlNode cur, List <TRptFieldDetail>group, int groupId, int order)
        {
            TRptFieldDetail element;

            if (group == null)
            {
                group = new List <TRptFieldDetail>();
            }

            element = ParseFieldDetail(cur, order);
            group.Add(element);
            return group;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="group"></param>
        /// <param name="groupId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected List <TRptSwitch>ParseIfGroup(XmlNode cur, List <TRptSwitch>group, int groupId, int order)
        {
            TRptSwitch element;
            TRptCase ifcase;

            if (group == null)
            {
                group = new List <TRptSwitch>();
            }

            element = new TRptSwitch(order);
            element.rptGrpCases = new List <TRptCase>();
            ifcase = ParseCase(cur, 0);
            element.rptGrpCases.Add(ifcase);
            group.Add(element);
            return group;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="group"></param>
        /// <param name="groupId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected List <TRptLevel>ParseLevelGroup(XmlNode cur, List <TRptLevel>group, int groupId, int order)
        {
            TRptLevel element;

            if (group == null)
            {
                group = new List <TRptLevel>();
            }

            element = ParseLevel(cur, order);
            group.Add(element);
            return group;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="group"></param>
        /// <param name="groupId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected List <TRptLowerLevel>ParseLowerLevelGroup(XmlNode cur, List <TRptLowerLevel>group, int groupId, int order)
        {
            TRptLowerLevel element;

            if (group == null)
            {
                group = new List <TRptLowerLevel>();
            }

            element = ParseLowerLevel(cur, order);
            group.Add(element);
            return group;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="group"></param>
        /// <param name="groupId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected List <TRptSwitch>ParseSwitchGroup(XmlNode cur, List <TRptSwitch>group, int groupId, int order)
        {
            TRptSwitch element;

            if (group == null)
            {
                group = new List <TRptSwitch>();
            }

            element = ParseSwitch(cur, order);
            group.Add(element);
            return group;
        }

        /// <summary>
        /// groups
        /// </summary>
        /// <returns>void</returns>
        protected List <TRptCalculation>ParseCalculationGroup(XmlNode cur, List <TRptCalculation>group, int groupId, int order)
        {
            TRptCalculation element;

            if (group == null)
            {
                group = new List <TRptCalculation>(groupId);
            }

            element = ParseCalculation(cur, order);
            group.Add(element);
            return group;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="group"></param>
        /// <param name="groupId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected List <TRptCase>ParseCaseGroup(XmlNode cur, List <TRptCase>group, int groupId, int order)
        {
            TRptCase element;

            if (group == null)
            {
                group = new List <TRptCase>();
            }

            element = ParseCase(cur, order);
            group.Add(element);
            return group;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="group"></param>
        /// <param name="groupId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected List <TRptParameter>ParseParameterGroup(XmlNode cur, List <TRptParameter>group, int groupId, int order)
        {
            TRptParameter element;

            if (group == null)
            {
                group = new List <TRptParameter>();
            }

            element = ParseParameter(cur, order);
            group.Add(element);
            return group;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="group"></param>
        /// <param name="groupId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected List <TRptDetailReport>ParseDetailReportGroup(XmlNode cur, List <TRptDetailReport>group, int groupId, int order)
        {
            TRptDetailReport element;

            if (group == null)
            {
                group = new List <TRptDetailReport>();
            }

            element = ParseDetailReport(cur, order);
            group.Add(element);
            return group;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="group"></param>
        /// <param name="groupId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected List <TRptQuery>ParseQueryGroup(XmlNode cur, List <TRptQuery>group, int groupId, int order)
        {
            TRptQuery element;

            if (group == null)
            {
                group = new List <TRptQuery>(groupId);
            }

            element = ParseQuery(cur, order);
            group.Add(element);
            return group;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="group"></param>
        /// <param name="groupId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected List <TRptValue>ParseValueGroup(XmlNode cur, List <TRptValue>group, int groupId, int order)
        {
            TRptValue element;

            if (group == null)
            {
                group = new List <TRptValue>(groupId);
            }

            element = ParseValue(cur, order);
            group.Add(element);
            return group;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected TRptSwitch ParseSwitch(XmlNode cur2, int order)
        {
            TRptSwitch ReturnValue;
            XmlNode cur;
            Object rg;
            Object r;
            TRptSwitch element;

            ReturnValue = null;
            cur = cur2;

            if (cur.Name == "switch")
            {
                element = new TRptSwitch(order);
                cur = NextNotBlank(cur.FirstChild);
                rg = ParseGroup(cur, ref report.casesId, "case", out cur);

                if (rg != null)
                {
                    element.rptGrpCases = ((List <TRptCase> )rg);
                }

                r = Parse(cur, ref report.casesId, "default");

                if (r != null)
                {
                    element.rptDefault = (TRptCase)r;
                }

                ReturnValue = element;
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected TRptCalculation ParseCalculation(XmlNode cur2, int order)
        {
            XmlNode cur;
            Object rg;
            TRptCalculation element;

            cur = cur2;
            element = new TRptCalculation(order);
            element.strId = GetAttribute(cur, "id");
            element.strReturnsFormat = GetAttribute(cur, "returnsFormat");
            element.strReturns = GetAttribute(cur, "returns");
            element.strAlign = GetAttribute(cur, "align");
            cur = NextNotBlank(cur.FirstChild);

            if (cur.Name == "caption")
            {
                rg = ParseGroup(cur.FirstChild, ref report.valuesId, "value");

                if (rg != null)
                {
                    element.rptGrpCaption = ((List <TRptValue> )rg);
                }

                cur = GetNextEntity(cur);
            }

            if (cur.Name == "shortcaption")
            {
                rg = ParseGroup(cur.FirstChild, ref report.valuesId, "value");

                if (rg != null)
                {
                    element.rptGrpShortCaption = ((List <TRptValue> )rg);
                }

                cur = GetNextEntity(cur);
            }

            if (cur.Name == "template")
            {
                rg = ParseGroup(cur.FirstChild, ref report.queriesId, "queryDetail");

                if (rg != null)
                {
                    element.rptGrpTemplate = ((List <TRptQuery> )rg);
                }

                cur = GetNextEntity(cur);
            }

            if (cur.Name == "query")
            {
                rg = ParseGroup(cur.FirstChild, ref report.queriesId, "queryDetail");

                if (rg != null)
                {
                    element.rptGrpQuery = ((List <TRptQuery> )rg);
                }

                cur = GetNextEntity(cur);
            }

            return element;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected TRptCase ParseCase(XmlNode cur2, int order)
        {
            XmlNode cur;
            Object rg;
            Object r;
            TRptCase element;

            cur = cur2;
            element = new TRptCase(order);
            element.strCondition = StringHelper.CleanString(GetAttribute(cur, "condition"));
            cur = NextNotBlank(cur.FirstChild);
            rg = ParseGroup(cur, ref report.lowerReportsId, "lowerLevelReport");

            if (rg != null)
            {
                element.rptGrpLowerLevel = ((List <TRptLowerLevel> )rg);
            }

            rg = ParseGroup(cur, ref report.fieldsId, "field", out cur);

            if (rg != null)
            {
                element.rptGrpField = ((List <TRptField> )rg);
            }

            rg = ParseGroup(cur, ref report.valuesId, "value", out cur);

            if (rg != null)
            {
                element.rptGrpValue = ((List <TRptValue> )rg);
            }

            r = Parse(cur, ref report.switchesId, "switch");

            if (r != null)
            {
                element.rptSwitch = ((TRptSwitch)r);
            }

            return element;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected TRptDetail ParseDetail(XmlNode cur2, int order)
        {
            XmlNode cur;
            Object rg;
            Object r;
            TRptDetail element;

            cur = cur2;
            element = new TRptDetail(order);
            cur = NextNotBlank(cur.FirstChild);
            r = Parse(cur, ref report.switchesId, "switch");

            if (r != null)
            {
                element.rptSwitch = ((TRptSwitch)r);
            }
            else
            {
                r = Parse(cur, ref report.switchesId, "if");

                if (r != null)
                {
                    element.rptSwitch = ((TRptSwitch)r);
                }
            }

            rg = ParseGroup(cur, ref report.lowerReportsId, "lowerLevelReport");

            if (rg != null)
            {
                element.rptGrpLowerLevel = ((List <TRptLowerLevel> )rg);
            }

            rg = ParseGroup(cur, ref report.fieldsId, "field", out cur);

            if (rg != null)
            {
                element.rptGrpField = ((List <TRptField> )rg);
            }

            return element;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected TRptDetailReport ParseDetailReport(XmlNode cur2, int order)
        {
            XmlNode cur;
            Object rg;
            TRptDetailReport element;

            cur = cur2;
            element = new TRptDetailReport(GetAttribute(cur, "id"));
            element.strAction = GetAttribute(cur, "action");
            cur = NextNotBlank(cur.FirstChild);
            rg = ParseGroup(cur, ref report.parametersId, "parameter", out cur);

            if (rg != null)
            {
                element.rptGrpParameter = ((List <TRptParameter> )rg);
            }

            // cur := nextNotBlank(cur.NextSibling);
            if ((cur != null) && (cur.Name == "detailreportquery") && (cur.InnerText != null))
            {
                element.strQuery = StringHelper.CleanString(cur.InnerText);
            }

            return element;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected TRptField ParseField(XmlNode cur2, int order)
        {
            XmlNode cur;
            Object rg;
            TRptField element;

            cur = cur2;
            element = new TRptField(order);
            element.strWhichfield = GetAttribute(cur, "whichfield").ToLower();
            element.strCalculation = GetAttribute(cur, "calculation");
            element.strPos = GetAttribute(cur, "pos").ToLower();
            element.strPos = element.strPos.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            element.strWidth = GetAttribute(cur, "width").ToLower();
            element.strWidth = element.strWidth.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            element.strFormat = GetAttribute(cur, "format");
            element.strAlign = GetAttribute(cur, "align").ToLower();
            element.strLine = GetAttribute(cur, "line").ToLower();
            cur = NextNotBlank(cur.FirstChild);
            rg = ParseGroup(cur, ref report.valuesId, "value", out cur);

            if (rg != null)
            {
                element.rptGrpValue = ((List <TRptValue> )rg);
            }

            rg = ParseGroup(cur, ref report.fieldDetailsId, "fielddetail", out cur);

            if (rg != null)
            {
                element.rptGrpFieldDetail = ((List <TRptFieldDetail> )rg);
            }

            return element;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected TRptLevel ParseLevel(XmlNode cur2, int order)
        {
            XmlNode cur;
            Object r;
            TRptLevel element;

            cur = cur2;
            element = new TRptLevel(order);
            element.strName = GetAttribute(cur, "name");
            element.strIdentification = GetAttribute(cur, "identification");
            element.strCondition = GetAttribute(cur, "condition");
            cur = NextNotBlank(cur.FirstChild);

            if ((cur != null) && (cur.Name == "header"))
            {
                ParseFooterHeader(NextNotBlank(cur.FirstChild), out element.rptGrpHeaderField, out element.rptGrpHeaderSwitch);
                cur = GetNextEntity(cur);
            }

            r = Parse(cur, ref report.detailsId, "detail");

            if (r != null)
            {
                element.rptDetail = ((TRptDetail)r);
                cur = GetNextEntity(cur);
            }

            if ((cur != null) && (cur.Name == "footer"))
            {
                element.strFooterLine = GetAttribute(cur, "line").ToLower();
                element.strFooterSpace = GetAttribute(cur, "space").ToLower();
                ParseFooterHeader(NextNotBlank(cur.FirstChild), out element.rptGrpFooterField, out element.rptGrpFooterSwitch);
            }

            return element;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected TRptLowerLevel ParseLowerLevel(XmlNode cur2, int order)
        {
            XmlNode cur;
            Object rg;
            TRptLowerLevel element;

            cur = cur2;
            element = new TRptLowerLevel(order);
            element.strLevel = GetAttribute(cur, "level");
            element.strCalculation = GetAttribute(cur, "calculation");
            element.strCondition = GetAttribute(cur, "condition");
            cur = NextNotBlank(cur.FirstChild);
            rg = ParseGroup(cur, ref report.parametersId, "parameter", out cur);

            if (rg != null)
            {
                element.rptGrpParameter = ((List <TRptParameter> )rg);
            }

            return element;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected TRptFieldDetail ParseFieldDetail(XmlNode cur2, int order)
        {
            XmlNode cur;
            Object rg;
            Object r;
            TRptFieldDetail element;

            cur = cur2;
            element = new TRptFieldDetail(order);
            element.strCondition = StringHelper.CleanString(GetAttribute(cur, "condition"));
            cur = NextNotBlank(cur.FirstChild);
            r = Parse(cur, ref report.switchesId, "switch");

            if (r != null)
            {
                element.rptSwitch = ((TRptSwitch)r);
            }
            else
            {
                r = Parse(cur, ref report.switchesId, "if");

                if (r != null)
                {
                    element.rptSwitch = ((TRptSwitch)r);
                }
            }

            rg = ParseGroup(cur, ref report.valuesId, "value", out cur);

            if (rg != null)
            {
                element.rptGrpValue = ((List <TRptValue> )rg);
            }

            return element;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected TRptParameter ParseParameter(XmlNode cur2, int order)
        {
            XmlNode cur;
            Object rg;
            TRptParameter element;

            cur = cur2;
            element = new TRptParameter(order);
            element.strName = GetAttribute(cur, "name");
            element.strValue = GetAttribute(cur, "value");
            cur = NextNotBlank(cur.FirstChild);
            rg = ParseGroup(cur, ref report.valuesId, "value", out cur);

            if (rg != null)
            {
                element.rptGrpValue = ((List <TRptValue> )rg);
            }

            return element;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected TRptQuery ParseQuery(XmlNode cur2, int order)
        {
            XmlNode cur;
            Object rg;
            TRptQuery element;

            cur = cur2;
            element = new TRptQuery(order);
            element.strCondition = StringHelper.CleanString(GetAttribute(cur, "condition"));
            cur = NextNotBlank(cur.FirstChild);
            rg = ParseGroup(cur, ref report.valuesId, "value", out cur);

            if (rg != null)
            {
                element.rptGrpValue = ((List <TRptValue> )rg);
            }

            rg = ParseGroup(cur, ref report.parametersId, "parameter", out cur);

            if (rg != null)
            {
                element.rptGrpParameter = ((List <TRptParameter> )rg);
            }

            rg = ParseGroup(cur, ref report.switchesId, "switch", out cur);

            if (rg != null)
            {
                element.rptGrpSwitch = ((List <TRptSwitch> )rg);
            }

            return element;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected TRptValue ParseValue(XmlNode cur2, int order)
        {
            XmlNode cur;
            TRptValue element;

            cur = cur2;
            element = new TRptValue(order);
            element.strCondition = StringHelper.CleanString(GetAttribute(cur, "condition"));
            element.strFormat = StringHelper.CleanString(GetAttribute(cur, "format"));
            element.strFunction = StringHelper.CleanString(GetAttribute(cur, "function"));
            element.strVariable = GetAttribute(cur, "variable");
            element.strCalculation = GetAttribute(cur, "calculation");
            element.strText = GetAttribute(cur, "text");

            if ((element.strText.Length == 0) && (cur.InnerText != null))
            {
                element.strText = StringHelper.CleanString(cur.InnerText);
            }

            return element;
        }
    }
}