﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2024 by OM International
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
using System.IO;
using System.Text;
using System.Threading;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Web;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Newtonsoft.Json;

namespace Ict.Common.Remoting.Shared
{
    /// serialize and deserialize complex types using JSON
    /// TODO: rename the class
    public class THttpBinarySerializer
    {
        private const int MAXBASE64PREFIX = 80;

        static private string DataSetToJson(DataSet ADataset)
        {
            Dictionary <string, List <Dictionary <string, object>>>dataset =
                new Dictionary <string, List <Dictionary <string, object>>>();
            List <Dictionary <string, object>>table = null;
            Dictionary <string, object>row = null;

            foreach (DataTable dt in ADataset.Tables)
            {
                table = new List <Dictionary <string, object>>();

                foreach (DataRowView vr in dt.DefaultView)
                {
                    DataRow dr = vr.Row;

                    row = new Dictionary <string, object>();

                    foreach (DataColumn col in dt.Columns)
                    {
                        // we want DateTime in ISO format
                        if (dr[col] is DateTime || dr[col] is DateTime?)
                        {
                            row.Add(col.ColumnName.Trim(), SerializeObject(dr[col]).Trim('"'));
                        }
                        else
                        {
                            row.Add(col.ColumnName.Trim(), dr[col]);
                        }
                    }

                    table.Add(row);
                }

                dataset.Add(dt.TableName, table);
            }

	    return JsonConvert.SerializeObject(dataset);
        }

        static private string DataTableToJson(DataTable ATable)
        {
            List <Dictionary <string, object>>table = null;
            Dictionary <string, object>row = null;

            table = new List <Dictionary <string, object>>();

            foreach (DataRowView vr in ATable.DefaultView)
            {
                DataRow dr = vr.Row;

                row = new Dictionary <string, object>();

                foreach (DataColumn col in ATable.Columns)
                {
                    // we want DateTime in ISO format
                    if (dr[col] is DateTime || dr[col] is DateTime?)
                    {
                        row.Add(col.ColumnName.Trim(), SerializeObject(dr[col]).Trim('"'));
                    }
                    else
                    {
                        row.Add(col.ColumnName.Trim(), dr[col]);
                    }
                }

                table.Add(row);
            }

	    return JsonConvert.SerializeObject(table);
        }

        static private string VerificationResultCollectionToJson(TVerificationResultCollection ACollection)
        {
            // only return the error codes
            return ACollection.GetErrorCodes();
        }

        /// <summary>
        /// serialize string to Base64
        /// </summary>
        static public string SerializeToBase64(string s)
        {
            if (s == null)
            {
                return "null";
            }

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(s);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// deserialize string from Base64
        /// </summary>
        static public string DeserializeFromBase64(string s)
        {
            byte[] data = Convert.FromBase64String(s);
            return(Encoding.UTF8.GetString(data));
        }

        /// <summary>
        /// serialize any object. if it is a complex type, use JSON
        /// </summary>
        static public string SerializeObject(object o)
        {
            if (o == null)
            {
                return "null";
            }

            if (o.GetType() == typeof(bool))
            {
                return o.ToString().ToLower();
            }

            if (o.GetType() == typeof(string))
            {
                return '"' + o.ToString().Replace('"', '\'') + '"';
            }

            if (o.GetType().ToString().StartsWith("System.Int") || o.GetType().ToString().StartsWith("System.Decimal"))
            {
                return o.ToString();
            }

            if (o.GetType() == typeof(DateTime))
            {
                return '"' + ((DateTime)o).ToString("s") + '"';
            }

            if (o.GetType() == typeof(DateTime?))
            {
                DateTime? dt = (DateTime?)o;

                if (dt.HasValue)
                {
                    return '"' + dt.Value.ToString("s") + '"';
                }

                return String.Empty;
            }

            if (o is Type)
            {
                return o.ToString();
            }

            if (o is IList && o.GetType().IsGenericType)
            {
		return JsonConvert.SerializeObject(o);
            }

            if (o is DataSet)
            {
                return DataSetToJson((DataSet)o);
            }

            if (o is DataTable)
            {
                return DataTableToJson((DataTable)o);
            }

            if (o is TVerificationResultCollection)
            {
                return VerificationResultCollectionToJson((TVerificationResultCollection)o);
            }

            if (o is TProgressState)
            {
                return JsonConvert.SerializeObject(o);
            }

            throw new Exception("cannot deserialize object to JSON of Type " + o.GetType().ToString());
        }

        /// <summary>
        /// reverse of SerializeObject
        /// </summary>
        static public object DeserializeObject(string s, string type)
        {
            if (s == "null")
            {
                return null;
            }

            if (s == null)
            {
                return null;
            }

            if (s == "" && type == "binary")
            {
                return new byte[0];
            }

            if (s.EndsWith(":System.String"))
            {
                s = s.Substring(0, s.Length - ":System.String".Length);
                ValidateStringForInjectedHTML(s);
            }

            if (type == "System.Int64")
            {
                return Convert.ToInt64(s);
            }
            else if (type == "System.Int32")
            {
                return Convert.ToInt32(s);
            }
            else if (type == "System.Int16")
            {
                return Convert.ToInt16(s);
            }
            else if (type == "System.UInt64")
            {
                return Convert.ToUInt64(s);
            }
            else if (type == "System.UInt32")
            {
                return Convert.ToUInt32(s);
            }
            else if (type == "System.UInt16")
            {
                return Convert.ToUInt16(s);
            }
            else if (type == "System.Boolean")
            {
                return Convert.ToBoolean(s);
            }
            else if (type == "System.Decimal")
            {
                return Convert.ToDecimal(s);
            }
            else if (type == "System.String")
            {
                if (s.Length > MAXBASE64PREFIX && s.StartsWith("data:") && s.Substring(0, MAXBASE64PREFIX).Contains(";base64,"))
                {
                    return DeserializeFromBase64(s.Substring(s.IndexOf(";base64,") + ";base64,".Length));
                }
                ValidateStringForInjectedHTML(s);
                return s;
            }
            else if (type == "System.Data.DataTable")
            {
                return DeserializeDataTable(s);
            }
            else if (type.EndsWith("Enum"))
            {
                Type t = Type.GetType(type);

                if (t == null)
                {
                    foreach (Assembly a in System.AppDomain.CurrentDomain.GetAssemblies())
                    {
                        t = a.GetType(type);

                        if (t != null)
                        {
                            break;
                        }
                    }
                }

                if (t != null)
                {
                    return Enum.Parse(t, s);
                }

                throw new Exception("THttpBinarySerializer.DeserializeObject: unknown enum " + type);
            }
            else if (s.Length > MAXBASE64PREFIX && s.StartsWith("data:") && s.Substring(0, MAXBASE64PREFIX).Contains(";base64,"))
            {
                return Convert.FromBase64String(s.Substring(s.IndexOf(";base64,") + ";base64,".Length));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Deserialize List of String
        /// </summary>
        static public List<string> DeserializeObject(List<string> l)
        {
            if (l == null)
            {
                return null;
            }

            List<string> result = new List<string>();

            foreach (string s in l) {
                result.Add(s);
            }

            return result;
        }

        /// <summary>
        /// Deserialize Dictionary of String
        /// </summary>
        static public Dictionary<string,string> DeserializeObject(Dictionary<string,string> d)
        {
            if (d == null)
            {
                return null;
            }

            Dictionary<string,string> result = new Dictionary<string,string>();

            foreach (KeyValuePair<string, string> entry in d) {
                string key = entry.Key;
                string value = entry.Value;

                ValidateStringForInjectedHTML(value);
                result.Add(key, value);
            }

            return result;
        }

        /// Deserialize a DataTable
        static public DataTable DeserializeDataTable(string s)
        {
            List<object> list = JsonConvert.DeserializeObject<List<object>>(s);
            DataTable result = new DataTable();

            foreach (Dictionary<string,object> obj in list)
            {
                foreach (KeyValuePair<string, object> entry in obj)
                {
                    if (!result.Columns.Contains(entry.Key))
                    {
                        result.Columns.Add(entry.Key);
                    }
                }

                DataRow row = result.NewRow();

                foreach (KeyValuePair<string, object> entry in obj)
                {
                    if (entry.Value is String)
                    {
                        ValidateStringForInjectedHTML(entry.Value.ToString());
                    }
                    row[entry.Key] = entry.Value;
                }

                result.Rows.Add(row);
            }

            return result;
        }

        /// Deserialize a DataSet
        static public DataSet DeserializeDataSet(string s, DataSet dataset)
        {
            Dictionary<string,object> tables = JsonConvert.DeserializeObject<Dictionary<string,object>>(s);
            foreach (KeyValuePair<string, object> entry in tables)
            {
                object[] list2 = (object[]) entry.Value;
                foreach (Dictionary<string,object> obj in list2)
                {
                    DataRow row = dataset.Tables[entry.Key].NewRow();
                    foreach (KeyValuePair<string, object> cell in obj)
                    {
                        if (cell.Value == null)
                        {
                            row[cell.Key] = DBNull.Value;
                        }
                        else
                        {
                            if (cell.Value is String)
                            {
                                ValidateStringForInjectedHTML(cell.Value.ToString());
                            }
                            row[cell.Key] = cell.Value;
                        }
                    }

                    dataset.Tables[entry.Key].Rows.Add(row);
                }
            }

            return dataset;
        }

        static private void ValidateStringForInjectedHTML(string s)
        {
            if (s.Contains('<') && s.Contains('>') && s.Contains('='))
            {
                throw new Exception("Invalid characters <, > and = in parameter: " + s);
            }
        }
    }
}
