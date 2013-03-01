//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;

namespace GenerateI18N
{
/// <summary>
/// parse the code for error codes and store to HTML file
/// </summary>
public class TGenerateErrorCodeDoc
{
    /// <summary>
    /// create HTML file with central documentation of error codes used throughout OpenPetra code
    /// </summary>
    public static bool Execute(string ACSharpPath)
    {
        CSParser parsedFile = new CSParser(ACSharpPath + "/ICT/Common/ErrorCodes.cs");

        ProcessFile(parsedFile);
        parsedFile = new CSParser(ACSharpPath + "/ICT/Petra/Shared/ErrorCodes.cs");
        ProcessFile(parsedFile);
        parsedFile = new CSParser(ACSharpPath + "/ICT/Common/Verification/StringChecks.cs");
        ProcessFile(parsedFile);

        return true;
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

    private static void ProcessFile(CSParser AParsedFile)
    {
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
                                    if (attr.Name == "ErrCodeAttribute")
                                    {
                                        TLogging.Log("");
                                        TLogging.Log("");
                                        TLogging.Log("");
                                        TLogging.Log(vd.Name + " = " + ((PrimitiveExpression)vd.Initializer).Value.ToString());

                                        foreach (Expression e in attr.PositionalArguments)
                                        {
                                            TLogging.Log(((PrimitiveExpression)e).Value.ToString());
                                        }

                                        foreach (Expression e in attr.NamedArguments)
                                        {
                                            TLogging.Log(((NamedArgumentExpression)e).Name + " = " +
                                                ExpressionToString(((NamedArgumentExpression)e).Expression));
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
}