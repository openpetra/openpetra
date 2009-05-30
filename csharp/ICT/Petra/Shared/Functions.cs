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
using Ict.Petra.Shared.DataStore.TableList;
using System.Collections;
using System.Reflection;

namespace Ict.Petra.Shared.DataStore
{
    /// <summary>
    /// some useful functions to operate on the typed tables of Petra
    /// </summary>
    public class TPetraDataStore : TTableList
    {
        /// <summary>
        /// load typed table by name from the dll, using reflection
        /// </summary>
        /// <param name="ATableName"></param>
        /// <returns></returns>
        public static System.Type GetTypeDataTable(String ATableName)
        {
            String typename;
            String snamespace;

            if (TTableList.FTableList == null)
            {
                throw new Exception("Problem in TPetraDataStore.GetTypeDataTable: TableList has not been initialised.");
            }

            if (TTableList.FTableList.IndexOfKey(ATableName) == -1)
            {
                throw new Exception("Problem in TPetraDataStore.GetTypeDataTable: cannot find table " + ATableName);
            }

            snamespace = TTableList.FTableList.GetByIndex(TTableList.FTableList.IndexOfKey(ATableName)).ToString();
            typename = "Ict.Petra.Shared." + snamespace + ".Data." + ATableName + "Table";

            // we have put several namespaces together in one dll (eg. MPartner.Partner and MPartner.Mailroom)
            if (snamespace.Contains("."))
            {
                snamespace = snamespace.Substring(0, snamespace.IndexOf("."));
            }

            string dllName = "Ict.Petra.Shared." + snamespace + ".DataTables";
            return System.Type.GetType(
                typename + ',' + dllName + ",Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
        }

        /// <summary>
        /// get string length for field value
        /// </summary>
        /// <param name="ATableName">in CamelCase</param>
        /// <param name="AFieldName">in CamelCase</param>
        /// <returns></returns>
        public static System.Int32 GetLengthOfTableField(String ATableName, String AFieldName)
        {
            System.Int32 ReturnValue;
            MethodInfo m;
            System.Type t;
            ReturnValue = -1;
            t = GetTypeDataTable(ATableName);

            if (t != null)
            {
                m = t.GetMethod("Get" + AFieldName + "Length");

                if (m != null)
                {
                    ReturnValue = (Int16)m.Invoke(null, null);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// ATableName in CamelCase
        /// </summary>
        /// <returns>void</returns>
        public static String GetTableLabel(String ATableName)
        {
            String ReturnValue;
            MethodInfo m;

            System.Type t;
            ReturnValue = "### Unknown DB Table ###";
            t = GetTypeDataTable(ATableName);

            if (t != null)
            {
                m = t.GetMethod("GetTableLabel");

                if (m != null)
                {
                    ReturnValue = (String)m.Invoke(null, null);
                }
            }

            return ReturnValue;
        }
    }
}