/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using Ict.Common.IO;
using System.Xml;
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
        protected TXMLGroup ParseGroup(XmlNode cur2, ref int groupId, string entity, out XmlNode newcur)
        {
            TXMLGroup ReturnValue;
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
                    ReturnValue = ParseValueGroup(cur, (TRptGrpValue)ReturnValue, currentGroup, order);
                }
                else if (entity == "calculation")
                {
                    ReturnValue = ParseCalculationGroup(cur, (TRptGrpCalculation)ReturnValue, currentGroup, order);
                }
                else if (entity == "field")
                {
                    ReturnValue = ParseFieldGroup(cur, (TRptGrpField)ReturnValue, currentGroup, order);
                }
                else if (entity == "fielddetail")
                {
                    ReturnValue = ParseFieldDetailGroup(cur, (TRptGrpFieldDetail)ReturnValue, currentGroup, order);
                }
                else if (entity == "level")
                {
                    ReturnValue = ParseLevelGroup(cur, (TRptGrpLevel)ReturnValue, currentGroup, order);
                }
                else if (entity == "lowerLevelReport")
                {
                    ReturnValue = ParseLowerLevelGroup(cur, (TRptGrpLowerLevel)ReturnValue, currentGroup, order);
                }
                else if (entity == "switch")
                {
                    ReturnValue = ParseSwitchGroup(cur, (TRptGrpSwitch)ReturnValue, currentGroup, order);
                }
                else if (entity == "if")
                {
                    ReturnValue = ParseIfGroup(cur, (TRptGrpSwitch)ReturnValue, currentGroup, order);
                }
                else if (entity == "case")
                {
                    ReturnValue = ParseCaseGroup(cur, (TRptGrpCase)ReturnValue, currentGroup, order);
                }
                else if (entity == "queryDetail")
                {
                    ReturnValue = ParseQueryGroup(cur, (TRptGrpQuery)ReturnValue, currentGroup, order);
                }
                else if (entity == "parameter")
                {
                    ReturnValue = ParseParameterGroup(cur, (TRptGrpParameter)ReturnValue, currentGroup, order);
                }
                else if (entity == "detailreport")
                {
                    ReturnValue = ParseDetailReportGroup(cur, (TRptGrpDetailReport)ReturnValue, currentGroup, order);
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
        protected TXMLGroup ParseGroup(XmlNode cur2, ref int groupId, string entity)
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
        protected TXMLGroup Parse(string groupentity, ref XmlNode cur, ref int groupId, string entity)
        {
            TXMLGroup ReturnValue;

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
        protected TXMLElement Parse(XmlNode cur, ref int groupId, string entity)
        {
            TXMLElement ReturnValue;
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
        protected TXMLElement ParseReport(XmlNode cur2)
        {
            XmlNode cur;
            int dummy = -1;
            TXMLGroup rg;

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
                    report.rptReport.rptGrpDetailReport = (TRptGrpDetailReport)rg;
                }

                cur = GetNextEntity(cur);
            }

            rg = ParseGroup(cur.FirstChild, ref report.calculationsId, "calculation");

            if (rg != null)
            {
                report.rptReport.rptGrpCalculation = (TRptGrpCalculation)rg;
            }

            cur = GetNextEntity(cur);
            rg = ParseGroup(cur.FirstChild, ref report.levelsId, "level");

            if (rg != null)
            {
                report.rptReport.rptGrpLevel = (TRptGrpLevel)rg;
            }

            return report.rptReport;
        }

        /// <summary>
        /// parse the header and footer of the report
        /// </summary>
        /// <param name="cur2"></param>
        /// <param name="field"></param>
        /// <param name="ASwitch"></param>
        protected void ParseFooterHeader(XmlNode cur2, out TRptGrpField field, out TRptGrpSwitch ASwitch)
        {
            XmlNode cur;
            TXMLGroup rg;

            cur = cur2;
            field = null;
            ASwitch = null;
            rg = ParseGroup(cur, ref report.fieldsId, "field", out cur);

            if (rg != null)
            {
                field = (TRptGrpField)rg;
            }

            rg = ParseGroup(cur, ref report.switchesId, "switch", out cur);

            if (rg != null)
            {
                ASwitch = (TRptGrpSwitch)rg;
            }
            else
            {
                rg = ParseGroup(cur, ref report.switchesId, "if", out cur);

                if (rg != null)
                {
                    ASwitch = (TRptGrpSwitch)rg;
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
        protected TRptGrpField ParseFieldGroup(XmlNode cur, TRptGrpField group, int groupId, int order)
        {
            TRptField element;

            if (group == null)
            {
                group = new TRptGrpField(groupId);
            }

            element = ParseField(cur, order);
            group.List.Add(element);
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
        protected TRptGrpFieldDetail ParseFieldDetailGroup(XmlNode cur, TRptGrpFieldDetail group, int groupId, int order)
        {
            TRptFieldDetail element;

            if (group == null)
            {
                group = new TRptGrpFieldDetail(groupId);
            }

            element = ParseFieldDetail(cur, order);
            group.List.Add(element);
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
        protected TRptGrpSwitch ParseIfGroup(XmlNode cur, TRptGrpSwitch group, int groupId, int order)
        {
            TRptSwitch element;
            TRptCase ifcase;

            if (group == null)
            {
                group = new TRptGrpSwitch(groupId);
            }

            element = new TRptSwitch(order);
            element.rptGrpCases = new TRptGrpCase(0);
            ifcase = ParseCase(cur, 0);
            element.rptGrpCases.List.Add(ifcase);
            group.List.Add(element);
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
        protected TRptGrpLevel ParseLevelGroup(XmlNode cur, TRptGrpLevel group, int groupId, int order)
        {
            TRptLevel element;

            if (group == null)
            {
                group = new TRptGrpLevel(groupId);
            }

            element = ParseLevel(cur, order);
            group.List.Add(element);
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
        protected TRptGrpLowerLevel ParseLowerLevelGroup(XmlNode cur, TRptGrpLowerLevel group, int groupId, int order)
        {
            TRptLowerLevel element;

            if (group == null)
            {
                group = new TRptGrpLowerLevel(groupId);
            }

            element = ParseLowerLevel(cur, order);
            group.List.Add(element);
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
        protected TRptGrpSwitch ParseSwitchGroup(XmlNode cur, TRptGrpSwitch group, int groupId, int order)
        {
            TRptSwitch element;

            if (group == null)
            {
                group = new TRptGrpSwitch(groupId);
            }

            element = ParseSwitch(cur, order);
            group.List.Add(element);
            return group;
        }

        /// <summary>
        /// groups
        /// </summary>
        /// <returns>void</returns>
        protected TRptGrpCalculation ParseCalculationGroup(XmlNode cur, TRptGrpCalculation group, int groupId, int order)
        {
            TRptCalculation element;

            if (group == null)
            {
                group = new TRptGrpCalculation(groupId);
            }

            element = ParseCalculation(cur, order);
            group.List.Add(element);
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
        protected TRptGrpCase ParseCaseGroup(XmlNode cur, TRptGrpCase group, int groupId, int order)
        {
            TRptCase element;

            if (group == null)
            {
                group = new TRptGrpCase(groupId);
            }

            element = ParseCase(cur, order);
            group.List.Add(element);
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
        protected TRptGrpParameter ParseParameterGroup(XmlNode cur, TRptGrpParameter group, int groupId, int order)
        {
            TRptParameter element;

            if (group == null)
            {
                group = new TRptGrpParameter(groupId);
            }

            element = ParseParameter(cur, order);
            group.List.Add(element);
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
        protected TRptGrpDetailReport ParseDetailReportGroup(XmlNode cur, TRptGrpDetailReport group, int groupId, int order)
        {
            TRptDetailReport element;

            if (group == null)
            {
                group = new TRptGrpDetailReport(groupId);
            }

            element = ParseDetailReport(cur, order);
            group.List.Add(element);
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
        protected TRptGrpQuery ParseQueryGroup(XmlNode cur, TRptGrpQuery group, int groupId, int order)
        {
            TRptQuery element;

            if (group == null)
            {
                group = new TRptGrpQuery(groupId);
            }

            element = ParseQuery(cur, order);
            group.List.Add(element);
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
        protected TRptGrpValue ParseValueGroup(XmlNode cur, TRptGrpValue group, int groupId, int order)
        {
            TRptValue element;

            if (group == null)
            {
                group = new TRptGrpValue(groupId);
            }

            element = ParseValue(cur, order);
            group.List.Add(element);
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
            TXMLGroup rg;
            TXMLElement r;
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
                    element.rptGrpCases = ((TRptGrpCase)rg);
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
            TXMLGroup rg;
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
                    element.rptGrpCaption = ((TRptGrpValue)rg);
                }

                cur = GetNextEntity(cur);
            }

            if (cur.Name == "shortcaption")
            {
                rg = ParseGroup(cur.FirstChild, ref report.valuesId, "value");

                if (rg != null)
                {
                    element.rptGrpShortCaption = ((TRptGrpValue)rg);
                }

                cur = GetNextEntity(cur);
            }

            if (cur.Name == "template")
            {
                rg = ParseGroup(cur.FirstChild, ref report.queriesId, "queryDetail");

                if (rg != null)
                {
                    element.rptGrpTemplate = ((TRptGrpQuery)rg);
                }

                cur = GetNextEntity(cur);
            }

            if (cur.Name == "query")
            {
                rg = ParseGroup(cur.FirstChild, ref report.queriesId, "queryDetail");

                if (rg != null)
                {
                    element.rptGrpQuery = ((TRptGrpQuery)rg);
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
            TXMLGroup rg;
            TXMLElement r;
            TRptCase element;

            cur = cur2;
            element = new TRptCase(order);
            element.strCondition = StringHelper.CleanString(GetAttribute(cur, "condition"));
            cur = NextNotBlank(cur.FirstChild);
            rg = ParseGroup(cur, ref report.lowerReportsId, "lowerLevelReport");

            if (rg != null)
            {
                element.rptGrpLowerLevel = ((TRptGrpLowerLevel)rg);
            }

            rg = ParseGroup(cur, ref report.fieldsId, "field", out cur);

            if (rg != null)
            {
                element.rptGrpField = ((TRptGrpField)rg);
            }

            rg = ParseGroup(cur, ref report.valuesId, "value", out cur);

            if (rg != null)
            {
                element.rptGrpValue = ((TRptGrpValue)rg);
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
            TXMLGroup rg;
            TXMLElement r;
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
                element.rptGrpLowerLevel = ((TRptGrpLowerLevel)rg);
            }

            rg = ParseGroup(cur, ref report.fieldsId, "field", out cur);

            if (rg != null)
            {
                element.rptGrpField = ((TRptGrpField)rg);
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
            TXMLGroup rg;
            TRptDetailReport element;

            cur = cur2;
            element = new TRptDetailReport(GetAttribute(cur, "id"));
            element.strAction = GetAttribute(cur, "action");
            cur = NextNotBlank(cur.FirstChild);
            rg = ParseGroup(cur, ref report.parametersId, "parameter", out cur);

            if (rg != null)
            {
                element.rptGrpParameter = ((TRptGrpParameter)rg);
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
            TXMLGroup rg;
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
                element.rptGrpValue = ((TRptGrpValue)rg);
            }

            rg = ParseGroup(cur, ref report.fieldDetailsId, "fielddetail", out cur);

            if (rg != null)
            {
                element.rptGrpFieldDetail = ((TRptGrpFieldDetail)rg);
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
            TXMLElement r;
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
            TXMLGroup rg;
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
                element.rptGrpParameter = ((TRptGrpParameter)rg);
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
            TXMLGroup rg;
            TXMLElement r;
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
                element.rptGrpValue = ((TRptGrpValue)rg);
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
            TXMLGroup rg;
            TRptParameter element;

            cur = cur2;
            element = new TRptParameter(order);
            element.strName = GetAttribute(cur, "name");
            element.strValue = GetAttribute(cur, "value");
            cur = NextNotBlank(cur.FirstChild);
            rg = ParseGroup(cur, ref report.valuesId, "value", out cur);

            if (rg != null)
            {
                element.rptGrpValue = ((TRptGrpValue)rg);
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
            TXMLGroup rg;
            TRptQuery element;

            cur = cur2;
            element = new TRptQuery(order);
            element.strCondition = StringHelper.CleanString(GetAttribute(cur, "condition"));
            cur = NextNotBlank(cur.FirstChild);
            rg = ParseGroup(cur, ref report.valuesId, "value", out cur);

            if (rg != null)
            {
                element.rptGrpValue = ((TRptGrpValue)rg);
            }

            rg = ParseGroup(cur, ref report.parametersId, "parameter", out cur);

            if (rg != null)
            {
                element.rptGrpParameter = ((TRptGrpParameter)rg);
            }

            rg = ParseGroup(cur, ref report.switchesId, "switch", out cur);

            if (rg != null)
            {
                element.rptGrpSwitch = ((TRptGrpSwitch)rg);
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