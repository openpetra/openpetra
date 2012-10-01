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
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using Ict.Common;
using Ict.Common.IO;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;

namespace Ict.Tools.CodeGeneration
{
    /// some useful functions for auto generating code
    public class AutoGenerationTools
    {
        /// <summary>
        /// prepare the method declaration for another parameter;
        /// this is useful for writing method declarations
        /// </summary>
        /// <param name="MethodDeclaration"></param>
        /// <param name="firstParameter"></param>
        /// <param name="align"></param>
        /// <param name="AParamName"></param>
        /// <param name="AParamModifier"></param>
        /// <param name="AParamTypeName"></param>
        public static void AddParameter(ref string MethodDeclaration, ref bool firstParameter, int align,
            string AParamName, ParameterModifiers AParamModifier, string AParamTypeName)
        {
            if (!firstParameter)
            {
                MethodDeclaration += "," + Environment.NewLine;
                MethodDeclaration += new String(' ', align);
            }

            firstParameter = false;

            String parameterType = AParamTypeName;
            String StrParameter = "";

            if ((ParameterModifiers.Ref & AParamModifier) > 0)
            {
                StrParameter += "ref ";
            }
            else if ((ParameterModifiers.Out & AParamModifier) > 0)
            {
                StrParameter += "out ";
            }

            StrParameter += parameterType + " " + AParamName;

            MethodDeclaration += StrParameter;
        }

        /// <summary>
        /// format the actual parameters, and the parameter definition of a method
        /// </summary>
        public static void FormatParameters(List <ParameterDeclarationExpression>AMethodParameters,
            out string AActualParameters, out string AParameterDefinition)
        {
            int MethodDeclarationLength = 10;
            bool firstParameter = true;

            AActualParameters = string.Empty;
            AParameterDefinition = string.Empty;

            foreach (ParameterDeclarationExpression p in AMethodParameters)
            {
                AddParameter(ref AParameterDefinition,
                    ref firstParameter,
                    MethodDeclarationLength,
                    p.ParameterName,
                    p.ParamModifier,
                    p.TypeReference.ToString());

                if (AActualParameters.Length > 0)
                {
                    AActualParameters += ", ";
                }

                if ((ParameterModifiers.Ref & p.ParamModifier) > 0)
                {
                    AActualParameters += "ref ";
                }
                else if ((ParameterModifiers.Out & p.ParamModifier) > 0)
                {
                    AActualParameters += "out ";
                }

                AActualParameters += p.ParameterName;
            }
        }

        /// <summary>
        /// format a type name to a string.
        /// reduce the length of the type name if the namespace is unique.
        /// </summary>
        /// <param name="ATypeRef"></param>
        /// <param name="ANamespace"></param>
        /// <returns></returns>
        public static string TypeToString(TypeReference ATypeRef, string ANamespace)
        {
            string TypeAsString = ATypeRef.Type;

            if (ATypeRef.GenericTypes.Count > 0)
            {
                TypeAsString += "<";

                foreach (TypeReference tr in ATypeRef.GenericTypes)
                {
                    if (!TypeAsString.EndsWith("<"))
                    {
                        TypeAsString += ", ";
                    }

                    TypeAsString += tr.Type;
                }

                TypeAsString += ">";
            }

            if (ATypeRef.IsArrayType)
            {
                TypeAsString += "[]";
            }

            if (TypeAsString == "System.Void")
            {
                TypeAsString = "void";
            }

            // ReturnType sometimes has a very long name;
            // shorten it for readability
            // test whether the namespace is contained in the current namespace
            if (TypeAsString.IndexOf(".") != -1)
            {
                String returnTypeNS = TypeAsString.Substring(0, TypeAsString.LastIndexOf("."));

                if (ANamespace.IndexOf(returnTypeNS) == 0)
                {
                    TypeAsString = TypeAsString.Substring(TypeAsString.LastIndexOf(".") + 1);
                }
            }

            return TypeAsString;
        }
    }
}