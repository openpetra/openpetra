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
using System.Collections;
using System.Collections.Specialized;
using System.CodeDom;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration
{
    public class codeGenerationPetra
    {
        /** convert an sql type from our xml file into a delphi type
         * should also work for Convert.To<DelphiType>
         * for C#, we can only cast primitive types;
         * to specify a type that needs to be casted via CodeObjectCreateExpression,
         * add System. in front (e.g DateTime)
         *
         */
        public static string ToDelphiType(TTableField tableField)
        {
            return tableField.GetDotNetType();
        }

        public static CodeExpression ToPetraCast(TTableField tableField, CodeExpression variable)
        {
            CodeExpression ReturnValue;
            string delphiType;

            delphiType = ToDelphiType(tableField);

            if ((tableField.strTypeDotNet != null) && (tableField.strTypeDotNet.Length > 0)
                && (delphiType != tableField.strTypeDotNet) && (delphiType != "System." + tableField.strTypeDotNet))
            {
                // e.g.    (decimal.ToInt64(decimal(ret)));
                ReturnValue = CodeDom.MethodInvoke(CodeDom.TypeRefExp(delphiType), "To" + tableField.strTypeDotNet, new CodeExpression[]
                    { CodeDom.Cast(delphiType, variable) });
            }
            else
            {
                // e.g.  (string(ret));
                ReturnValue = CodeDom.Cast(delphiType, variable);
            }

            return ReturnValue;
        }

        // convert the type from the xml file to an ODBC type

        public static CodeExpression ToOdbcType(TTableField tableField)
        {
            CodeExpression ReturnValue;

            if (tableField.strType == "number")
            {
                ReturnValue = CodeDom.PropRef(CodeDom.TypeRefExp("OdbcType"), "Decimal");
            }
            else if (tableField.strType == "varchar")
            {
                ReturnValue = CodeDom.PropRef(CodeDom.TypeRefExp("OdbcType"), "VarChar");
            }
            else if (tableField.strType == "bit")
            {
                ReturnValue = CodeDom.PropRef(CodeDom.TypeRefExp("OdbcType"), "Bit");
            }
            else if (tableField.strType == "date")
            {
                ReturnValue = CodeDom.PropRef(CodeDom.TypeRefExp("OdbcType"), "Date");
            }
            else if (tableField.strType == "integer")
            {
                ReturnValue = CodeDom.PropRef(CodeDom.TypeRefExp("OdbcType"), "Int");
            }
            else
            {
                ReturnValue = CodeDom.PropRef(CodeDom.TypeRefExp("OdbcType"), "Int");
            }

            return ReturnValue;
        }

        // convert a list of fields to an array of expressions, StringCollection

        public static CodeExpression[] FieldListToStringExpressionArray(TDataSetTable table, StringCollection fields)
        {
            ArrayList fieldNames;
            TTableField f;

            fieldNames = new ArrayList();

            if ((table.Fields.Count == 0) && (table.OrigTable == null))
            {
                Console.WriteLine("Table " + table.tableorig + " does not exist. Please check your relations, constraints etc.");
                return (CodeExpression[])fieldNames.ToArray(typeof(CodePrimitiveExpression));
            }

            foreach (string s in fields)
            {
                if (table != null)
                {
                    f = table.GetField(s);

                    if (f != null)
                    {
                        fieldNames.Add(CodeDom._Const(f.strName));
                    }
                    else
                    {
                        Console.WriteLine(
                            "Problem in FieldListToStringExpressionArray; cannot find field " + s + " in table " +
                            table.tablename);
                        Environment.Exit(1);
                    }
                }
                else
                {
                    fieldNames.Add(CodeDom._Const(s));
                }
            }

            return (CodeExpression[])fieldNames.ToArray(typeof(CodePrimitiveExpression));
        }

        // convert a list of fields to an array of expressions

        public static CodeExpression[] FieldListToExpressionArray(TDataSetTable table, StringCollection fields)
        {
            ArrayList fieldNames;

            fieldNames = new ArrayList();

            foreach (string s in fields)
            {
                fieldNames.Add(CodeDom.PropRef(CodeDom.PropRef(table.tablealias), "Column" + TTable.NiceFieldName(s)));
            }

            return (CodeExpression[])fieldNames.ToArray(typeof(CodePropertyReferenceExpression));
        }

        // convert a list of fields to an array of expressions

        public static CodeExpression[] FieldListToExpressionArray(TTable table, StringCollection fields)
        {
            ArrayList fieldNames;

            fieldNames = new ArrayList();

            foreach (string s in fields)
            {
                fieldNames.Add(CodeDom.FieldRef("Column" + TTable.NiceFieldName(table.GetField(s))));
            }

            return (CodeExpression[])fieldNames.ToArray(typeof(CodeFieldReferenceExpression));
        }

        // convert a list of custom fields to an array of expressions; not using table nice names

        public static CodeExpression[] FieldListToExpressionArray(StringCollection fields)
        {
            ArrayList fieldNames;

            fieldNames = new ArrayList();

            foreach (string s in fields)
            {
                fieldNames.Add(CodeDom.FieldRef("Column" + s));
            }

            return (CodeExpression[])fieldNames.ToArray(typeof(CodeFieldReferenceExpression));
        }

        // using declarations

        public static void AddImports(CodeNamespace cns)
        {
            cns.Imports.Add(new CodeNamespaceImport("System"));
            cns.Imports.Add(new CodeNamespaceImport("System.Collections"));
            cns.Imports.Add(new CodeNamespaceImport("System.ComponentModel"));
            cns.Imports.Add(new CodeNamespaceImport("System.Data"));
            cns.Imports.Add(new CodeNamespaceImport("System.Data.Odbc"));
            cns.Imports.Add(new CodeNamespaceImport("System.Runtime.Serialization"));
            cns.Imports.Add(new CodeNamespaceImport("System.Xml"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Common"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Common.Data"));
        }
    }
}