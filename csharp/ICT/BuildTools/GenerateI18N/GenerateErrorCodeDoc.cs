//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
//
// Copyright 2004-2013 by OM International
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
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;

namespace GenerateI18N
{
/// <summary>
/// Parse the code for Error Codes and store to HTML file
/// </summary>
public class TGenerateErrorCodeDoc
{
    /// <summary>
    /// Creates a HTML file that contains central documentation of the Error Codes that are used throughout OpenPetra code
    /// </summary>
    public static bool Execute(string ACSharpPath, string ATemplateFilePath, string AOutFilePath)
    {
        Dictionary <string, ErrCodeInfo>ErrorCodes = new Dictionary <string, ErrCodeInfo>();
        CSParser parsedFile = new CSParser(ACSharpPath + "/ICT/Common/ErrorCodes.cs");
        string ErrCodeCategoryNice = String.Empty;

        TLogging.Log("Creating HTML documentation of OpenPetra Error Codes...");

        ProcessFile(parsedFile, ref ErrorCodes);
        parsedFile = new CSParser(ACSharpPath + "/ICT/Petra/Shared/ErrorCodes.cs");
        ProcessFile(parsedFile, ref ErrorCodes);
        parsedFile = new CSParser(ACSharpPath + "/ICT/Common/Verification/StringChecks.cs");
        ProcessFile(parsedFile, ref ErrorCodes);

        ProcessTemplate t = new ProcessTemplate(ATemplateFilePath);

        Dictionary <string, ProcessTemplate>snippets = new Dictionary <string, ProcessTemplate>();

        snippets.Add("GENC", t.GetSnippet("TABLE"));
        snippets["GENC"].SetCodelet("TABLEDESCRIPTION", "GENERAL (<i>Ict.Common* Libraries only</i>)");
        snippets.Add("GEN", t.GetSnippet("TABLE"));
        snippets["GEN"].SetCodelet("TABLEDESCRIPTION", "GENERAL (across the OpenPetra application)");
        snippets.Add("PARTN", t.GetSnippet("TABLE"));
        snippets["PARTN"].SetCodelet("TABLEDESCRIPTION", "PARTNER Module");
        snippets.Add("PERS", t.GetSnippet("TABLE"));
        snippets["PERS"].SetCodelet("TABLEDESCRIPTION", "PERSONNEL Module");
        snippets.Add("FIN", t.GetSnippet("TABLE"));
        snippets["FIN"].SetCodelet("TABLEDESCRIPTION", "FINANCE Module");
        snippets.Add("CONF", t.GetSnippet("TABLE"));
        snippets["CONF"].SetCodelet("TABLEDESCRIPTION", "CONFERENCE Module");
        snippets.Add("FINDEV", t.GetSnippet("TABLE"));
        snippets["FINDEV"].SetCodelet("TABLEDESCRIPTION", "FINANCIAL DEVELOPMENT Module");
        snippets.Add("SYSMAN", t.GetSnippet("TABLE"));
        snippets["SYSMAN"].SetCodelet("TABLEDESCRIPTION", "SYSTEM MANAGER Module");

        foreach (string snippetkey in snippets.Keys)
        {
            snippets[snippetkey].SetCodelet("ABBREVIATION", snippetkey);
            snippets[snippetkey].SetCodelet("ROWS", string.Empty);
        }

        foreach (string code in ErrorCodes.Keys)
        {
            foreach (string snippetkey in snippets.Keys)
            {
                if (code.StartsWith(snippetkey + "."))
                {
                    ProcessTemplate row = t.GetSnippet("ROW");
                    row.SetCodelet("CODE", code);

                    ErrCodeInfo ErrCode = ErrorCodes[code];

                    switch (ErrCode.Category)
                    {
                        case ErrCodeCategory.NonCriticalError:
                            ErrCodeCategoryNice = "Non-critical Error";
                            break;

                        default:
                            ErrCodeCategoryNice = ErrCode.Category.ToString("G");
                            break;
                    }

                    row.AddToCodelet("SHORTDESCRIPTION", (ErrCode.ShortDescription));
                    row.AddToCodelet("FULLDESCRIPTION", (ErrCode.FullDescription));
                    row.AddToCodelet("ERRORCODECATEGORY", (ErrCodeCategoryNice));
                    row.AddToCodelet("DECLARINGCLASS", (ErrCode.ErrorCodeConstantClass));

                    snippets[snippetkey].InsertSnippet("ROWS", row);
                }
            }
        }

        foreach (string snippetkey in snippets.Keys)
        {
            t.InsertSnippet("TABLES", snippets[snippetkey]);
        }

        return t.FinishWriting(AOutFilePath, ".html", true);
    }

    private static string ExpressionToString(Expression e)
    {
        if (e is BinaryOperatorExpression)
        {
            return ExpressionToString(((BinaryOperatorExpression)e).Left) + " " +
                   ExpressionToString(((BinaryOperatorExpression)e).Right);
        }
        else
        {
            return ((PrimitiveExpression)e).Value.ToString();
        }
    }

    private static void ProcessFile(CSParser AParsedFile, ref Dictionary <string, ErrCodeInfo>AErrorCodes)
    {
        ErrCodeInfo ErrCodeDetails = null;
        string ErrCodeValue;
        string ShortDescription = String.Empty;
        string LongDescription = String.Empty;
        ErrCodeCategory ErrCodeCat;

        foreach (TypeDeclaration t in AParsedFile.GetClasses())
        {
            foreach (object child in t.Children)
            {
                if (child is INode)
                {
                    INode node = (INode)child;
                    Type type = node.GetType();

                    if (type.Name == "FieldDeclaration")
                    {
                        FieldDeclaration fd = (FieldDeclaration)node;

                        foreach (VariableDeclaration vd in fd.Fields)
                        {
                            foreach (AttributeSection attrSection in fd.Attributes)
                            {
                                foreach (ICSharpCode.NRefactory.Ast.Attribute attr in attrSection.Attributes)
                                {
                                    LongDescription = String.Empty;

                                    if (attr.Name == "ErrCodeAttribute")
                                    {
                                        ErrCodeValue = ((PrimitiveExpression)vd.Initializer).Value.ToString();

                                        if (ErrCodeValue.EndsWith("V"))
                                        {
                                            ErrCodeCat = ErrCodeCategory.Validation;
                                        }
                                        else if (ErrCodeValue.EndsWith("N"))
                                        {
                                            ErrCodeCat = ErrCodeCategory.NonCriticalError;
                                        }
                                        else
                                        {
                                            ErrCodeCat = ErrCodeCategory.Error;
                                        }

//                                        TLogging.Log("");
//                                        TLogging.Log("");
//                                        TLogging.Log("");
//                                        TLogging.Log(vd.Name + " = " + ((PrimitiveExpression)vd.Initializer).Value.ToString());

                                        foreach (Expression e in attr.PositionalArguments)
                                        {
//                                            TLogging.Log("ShortDescription: " + ShortDescription);
                                            ShortDescription = ((PrimitiveExpression)e).Value.ToString();
                                        }

                                        foreach (Expression e in attr.NamedArguments)
                                        {
                                            if (((NamedArgumentExpression)e).Name == "FullDescription")
                                            {
                                                LongDescription = ExpressionToString(((NamedArgumentExpression)e).Expression);
                                            }

//                                            TLogging.Log("NamedArgumentExpression Name: " + LongDescription);
                                        }

                                        ErrCodeDetails = new ErrCodeInfo(ErrCodeValue, t.Name, vd.Name,
                                            ShortDescription, LongDescription,
                                            String.Empty, String.Empty, ErrCodeCat, String.Empty, false);

                                        AErrorCodes.Add(ErrCodeValue, ErrCodeDetails);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
}