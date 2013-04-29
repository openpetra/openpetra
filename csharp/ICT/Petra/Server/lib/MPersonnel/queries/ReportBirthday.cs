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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Xml.XPath;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Server.MPersonnel.queries
{
    /// <summary>
    /// some useful methods for birthday reports
    /// </summary>
    public class QueryBirthdayReport
    {
        /// <summary>
        /// get the family keys of the specified persons
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        public static DataTable GetFamilyKeys(TParameterList AParameters, TResultList AResults)
        {
            SortedList <string, string>Defines = new SortedList <string, string>();
            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();

            try
            {
                // prepare the sql statement parameters
                AddPartnerSelectionParametersToSqlQuery(AParameters, Defines, SqlParameterList);
            }
            catch (Exception e)
            {
                TLogging.Log("problem while preparing sql statement for birthday report: " + e.ToString());
                return null;
            }

            string SqlStmt = TDataBase.ReadSqlFile("Personnel.Reports.GetFamilyKeyOfPerson.sql", Defines);
            Boolean NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            try
            {
                // now run the database query
                DataTable resultTable = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "result", Transaction,
                    SqlParameterList.ToArray());

                // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
                if (AParameters.Get("CancelReportCalculation").ToBool() == true)
                {
                    return null;
                }

                return resultTable;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                return null;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }

        private static void AddPartnerSelectionParametersToSqlQuery(TParameterList AParameters,
            SortedList <string, string>ADefines,
            List <OdbcParameter>ASqlParameterList)
        {
            string paramSelection = AParameters.Get("param_selection").ToString();

            if (paramSelection == "one partner")
            {
                ADefines.Add("ONEPARTNER", string.Empty);
                ASqlParameterList.Add(new OdbcParameter("partnerkey", OdbcType.Decimal)
                    {
                        Value = AParameters.Get("param_partnerkey").ToDecimal()
                    });
            }
            else if (paramSelection == "an extract")
            {
                ADefines.Add("BYEXTRACT", string.Empty);

                ASqlParameterList.Add(new OdbcParameter("extractname", OdbcType.VarChar)
                    {
                        Value = AParameters.Get("param_extract").ToString()
                    });
            }
            else if (paramSelection == "all current staff")
            {
                ADefines.Add("BYSTAFF", string.Empty);
                ASqlParameterList.Add(new OdbcParameter("staffdate", OdbcType.Date)
                    {
                        Value = AParameters.Get("param_currentstaffdate").ToDate()
                    });
                ASqlParameterList.Add(new OdbcParameter("staffdate2", OdbcType.Date)
                    {
                        Value = AParameters.Get("param_currentstaffdate").ToDate()
                    });
            }
        }

        /// <summary>
        /// get all partners that we want to display on the current birthday report
        /// </summary>
        public static DataTable CalculateBirthdays(TParameterList AParameters, TResultList AResults)
        {
            SortedList <string, string>Defines = new SortedList <string, string>();
            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();

            try
            {
                // prepare the sql statement parameters
                if (AParameters.Exists("FamilyKey"))
                {
                    SqlParameterList.Add(new OdbcParameter("FamilyKey", OdbcType.Decimal)
                        {
                            Value = AParameters.Get("FamilyKey").ToDecimal()
                        });
                    Defines.Add("BYFAMILYKEY", string.Empty);
                }
                else
                {
                    AddPartnerSelectionParametersToSqlQuery(AParameters, Defines, SqlParameterList);
                }

                if (AParameters.Get("param_chkSelectTypes").ToBool() == true)
                {
                    string[] types = AParameters.Get("param_typecode").ToString().Split(new char[] { ',' });
                    string FilterForTypes = string.Empty;

                    foreach (string type in types)
                    {
                        if (FilterForTypes.Length > 0)
                        {
                            FilterForTypes += " OR ";
                        }

                        FilterForTypes += "pptype.p_type_code_c = ?";

                        SqlParameterList.Add(new OdbcParameter("typecode" + FilterForTypes.Length, OdbcType.VarChar)
                            {
                                Value = type
                            });
                    }

                    Defines.Add("SELECTTYPES", "(" + FilterForTypes + ")");
                }

                if (AParameters.Get("param_chkUseDate").ToBool() == true)
                {
                    DateTime FromDate = AParameters.Get("param_dtpFromDate").ToDate();
                    DateTime ToDate = AParameters.Get("param_dtpToDate").ToDate();

                    if (FromDate.DayOfYear < ToDate.DayOfYear)
                    {
                        Defines.Add("WITHDATERANGE", string.Empty);
                        SqlParameterList.Add(new OdbcParameter("startdate", OdbcType.Date)
                            {
                                Value = FromDate
                            });
                        SqlParameterList.Add(new OdbcParameter("enddate", OdbcType.Date)
                            {
                                Value = ToDate
                            });
                    }
                    else
                    {
                        Defines.Add("WITHOUTDATERANGE", string.Empty);
                        SqlParameterList.Add(new OdbcParameter("startdate", OdbcType.Date)
                            {
                                Value = FromDate
                            });
                        SqlParameterList.Add(new OdbcParameter("enddate", OdbcType.Date)
                            {
                                Value = ToDate
                            });
                    }
                }
            }
            catch (Exception e)
            {
                TLogging.Log("problem while preparing sql statement for birthday report: " + e.ToString());
                return null;
            }

            string SqlStmt = TDataBase.ReadSqlFile("Personnel.Reports.Birthday.sql", Defines);
            Boolean NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            try
            {
                // now run the database query
                TLogging.Log("getting the data from the database", TLoggingType.ToStatusBar);
                DataTable resultTable = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "result", Transaction,
                    SqlParameterList.ToArray());

                // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
                if (AParameters.Get("CancelReportCalculation").ToBool() == true)
                {
                    return null;
                }

                // if end date is not set, use today
                if (AParameters.Get("param_chkUseDate").ToBool() != true)
                {
                    AParameters.Add("param_dtpToDate", DateTime.Now);
                }

                // Calculate the age, in new column
                resultTable.Columns.Add(new DataColumn("age", typeof(Int32)));

                foreach (DataRow r in resultTable.Rows)
                {
                    int age = 0;
                    DateTime BDay = Convert.ToDateTime(r["DOB"]);

                    while (BDay.AddYears(age) <= AParameters.Get("param_dtpToDate").ToDate())
                    {
                        age++;
                    }

                    r["Age"] = age;
                }

                // filter by anniversaries?
                if ((AParameters.Get("param_chkAnniversaries").ToBool() == true)
                    && !AParameters.Get("param_txtAnniversaries").IsZeroOrNull())
                {
                    List <string>anniversaries = new List <string>(AParameters.Get("param_txtAnniversaries").ToString().Split(new char[] { ',' }));

                    List <DataRow>RowsToDelete = new List <DataRow>();

                    foreach (DataRow r in resultTable.Rows)
                    {
                        if (!anniversaries.Contains(r["Age"].ToString()))
                        {
                            RowsToDelete.Add(r);
                        }
                    }

                    foreach (DataRow r in RowsToDelete)
                    {
                        resultTable.Rows.Remove(r);
                    }
                }

                return resultTable;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                return null;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }
    }
}