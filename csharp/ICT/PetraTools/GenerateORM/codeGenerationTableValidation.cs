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
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.DataStore
{
    /// <summary>
    /// the code generator for validation for typed tables
    /// </summary>
    public class CodeGenerationTableValidation
    {
        /// <summary>
        /// write the definition for the code of validation of a typed table
        /// </summary>
        /// <param name="Template"></param>
        /// <param name="currentTable"></param>
        /// <param name="origTable"></param>
        /// <param name="WhereToInsert"></param>
        public static void InsertTableValidation(ProcessTemplate Template, TTable currentTable, TTable origTable, string WhereToInsert)
        {
            ProcessTemplate snippet = Template.GetSnippet("TABLEVALIDATION");
            string ReasonForAutomValidation;

            snippet.SetCodeletComment("TABLE_DESCRIPTION", currentTable.strDescription);
            snippet.SetCodelet("TABLENAME", currentTable.strDotNetName);

            foreach (TTableField col in currentTable.grpTableField)
            {
                ProcessTemplate columnTemplate;

                // NOT NULL checks
                if (TDataValidation.GenerateAutoValidationCodeForDBTableField(col, TDataValidation.TAutomDataValidationScope.advsNotNullChecks,
                        out ReasonForAutomValidation))
                {
                    columnTemplate = Template.GetSnippet("VALIDATECOLUMN");
                    columnTemplate.SetCodelet("COLUMNNAME", col.strNameDotNet);

                    if (col.GetDotNetType().Contains("String"))
                    {
                        ProcessTemplate validateColumnTemplate = Template.GetSnippet("CHECKEMPTYSTRING");
                        validateColumnTemplate.SetCodelet("COLUMNNAME", col.strNameDotNet);

                        columnTemplate.InsertSnippet("COLUMNSPECIFICCHECK", validateColumnTemplate);
                    }
                    else if (col.GetDotNetType().Contains("DateTime"))
                    {
                        ProcessTemplate validateColumnTemplate = Template.GetSnippet("CHECKEMPTYDATE");
                        validateColumnTemplate.SetCodelet("COLUMNNAME", col.strNameDotNet);

                        columnTemplate.InsertSnippet("COLUMNSPECIFICCHECK", validateColumnTemplate);
                    }
                    else
                    {
                        ProcessTemplate validateColumnTemplate = Template.GetSnippet("CHECKGENERALNOTNULL");
                        validateColumnTemplate.SetCodelet("COLUMNNAME", col.strNameDotNet);

                        columnTemplate.InsertSnippet("COLUMNSPECIFICCHECK", validateColumnTemplate);
                    }

                    columnTemplate.SetCodelet("COLUMNSPECIFICCOMMENT", "'" + col.strNameDotNet + "' " + ReasonForAutomValidation);

                    snippet.InsertSnippet("VALIDATECOLUMNS", columnTemplate);
                }

                // String Length checks
                if (TDataValidation.GenerateAutoValidationCodeForDBTableField(col, TDataValidation.TAutomDataValidationScope.advsStringLengthChecks,
                        out ReasonForAutomValidation))
                {
                    columnTemplate = Template.GetSnippet("VALIDATECOLUMN");
                    columnTemplate.SetCodelet("COLUMNNAME", col.strNameDotNet);

                    ProcessTemplate validateColumnTemplate = Template.GetSnippet("CHECKSTRINGLENGTH");
                    validateColumnTemplate.SetCodelet("COLUMNNAME", col.strNameDotNet);
                    validateColumnTemplate.SetCodelet("COLUMNLENGTH", (col.iCharLength * 2).ToString());

                    columnTemplate.InsertSnippet("COLUMNSPECIFICCHECK", validateColumnTemplate);
                    columnTemplate.SetCodelet("COLUMNSPECIFICCOMMENT", "'" + col.strNameDotNet + "' " + ReasonForAutomValidation);

                    snippet.InsertSnippet("VALIDATECOLUMNS", columnTemplate);
                }

                // Number Range checks
                if (TDataValidation.GenerateAutoValidationCodeForDBTableField(col, TDataValidation.TAutomDataValidationScope.advsNumberRangeChecks,
                        out ReasonForAutomValidation))
                {
                    columnTemplate = Template.GetSnippet("VALIDATECOLUMN");
                    columnTemplate.SetCodelet("COLUMNNAME", col.strNameDotNet);

                    ProcessTemplate validateColumnTemplate = Template.GetSnippet("CHECKNUMBERRANGE");
                    validateColumnTemplate.SetCodelet("COLUMNNAME", col.strNameDotNet);
                    validateColumnTemplate.SetCodelet("NUMBEROFDECIMALDIGITS", col.iLength.ToString());
                    validateColumnTemplate.SetCodelet("NUMBEROFFRACTIONALDIGITS", col.iDecimals > 0 ? col.iDecimals.ToString() : "0");

                    columnTemplate.InsertSnippet("COLUMNSPECIFICCHECK", validateColumnTemplate);
                    columnTemplate.SetCodelet("COLUMNSPECIFICCOMMENT", "'" + col.strNameDotNet + "' " + ReasonForAutomValidation);

                    snippet.InsertSnippet("VALIDATECOLUMNS", columnTemplate);
                }
            }

            Template.InsertSnippet(WhereToInsert, snippet);
        }

        /// <summary>
        /// create the code for validation of a typed table
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="strGroup"></param>
        /// <param name="AFilePath"></param>
        /// <param name="ANamespaceName"></param>
        /// <param name="AFileName"></param>
        /// <returns></returns>
        public static Boolean WriteValidation(TDataDefinitionStore AStore, string strGroup, string AFilePath, string ANamespaceName, string AFileName)
        {
            Console.WriteLine("processing validation of Typed Tables " + strGroup.Substring(0, 1).ToUpper() + strGroup.Substring(1));

            string templateDir = TAppSettingsManager.GetValue("TemplateDir", true);
            ProcessTemplate Template = new ProcessTemplate(templateDir + Path.DirectorySeparatorChar +
                "ORM" + Path.DirectorySeparatorChar +
                "DataTableValidation.cs");

            Template.AddToCodelet("NAMESPACE", ANamespaceName);
            Template.AddToCodelet("DATATABLENAMESPACE", ANamespaceName.Replace("Validation", "Data"));

            // load default header with license and copyright
            Template.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(templateDir));

            foreach (TTable currentTable in AStore.GetTables())
            {
                if (currentTable.strGroup == strGroup)
                {
                    InsertTableValidation(Template, currentTable, null, "TABLELOOP");
                }
            }

            if (!Directory.Exists(AFilePath))
            {
                Directory.CreateDirectory(AFilePath);
            }

            Template.FinishWriting(AFilePath + AFileName + "-generated.cs", ".cs", true);

            return true;
        }
    }
}