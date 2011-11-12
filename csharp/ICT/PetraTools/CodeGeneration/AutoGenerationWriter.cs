//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
    /**
     * This class allows generation of code.
     * It manages indenting of lines.
     * It is also able to recognise manual code marked by a region, and reinserting that code.
     * It also recognizes if code has been manually commented with double forward slash.
     * Call SynchronizeLines after you have written a uniquely identifiable line;
     * if a file gets out of sync, it is not clear where the ManualCode regions fit anymore
     * the best then is to compare the current version with the .new version
     *
     * all code that is different outside the #region ManualCode, and is not just commented, will be removed
     * new code from the generator will always be added
     */
    public class AutoGenerationWriter
    {
        /// a line should have only maximum number of characters
        public static Int32 CODE_LENGTH_UNCRUSTIFY = 150;

        private Int16 indent = 0;
        private List <string>originalVersion;
        private List <string>newVersion;
        private List <string>BufferTemp = null;
        private string OutputFile;

        /// <summary>
        /// this switches the current output from writing to the file to writing to a buffer
        /// this helps with conditional writing (eg only write namespace, if there is a valid interface to write)
        /// sync will be disabled, and applied when the buffer is written for real
        /// </summary>
        public void StartWriteToBuffer()
        {
            BufferTemp = new List <string>();
        }

        /// <summary>
        /// write the string collections, line by line
        /// </summary>
        /// <param name="ABuffer"></param>
        public void WriteBuffer(StringCollection ABuffer)
        {
            foreach (string s in ABuffer)
            {
                WriteLine(s);
            }
        }

        /// <summary>
        /// return the current indentation, number of spaces
        /// </summary>
        public Int32 CurrentIndentCharacters
        {
            get
            {
                return indent * 4;
            }
        }

        /**
         * @return the line number in the original version that has been edited manually
         */
        public void WriteLine(String line)
        {
            if (line.Contains(Environment.NewLine))
            {
                string[] sArray = line.Replace("\r", "").Split('\n');

                foreach (string s in sArray)
                {
                    WriteLine(s);
                }

                return;
            }

            // special treatment of empty lines
            if (line.Trim().Length == 0)
            {
                WriteLine();
                return;
            }

            line = line.Replace("\t", "    ");
            String formattedLine = "";

            for (int c = 0; c < indent; c++)
            {
                formattedLine += "    ";
            }

            if (line.Trim().StartsWith("#"))
            {
                line = line.Trim();
                formattedLine = "";
            }

            if (line.Trim().StartsWith("//") && (indent != 0))
            {
                line = line.Trim();
            }

            if (BufferTemp != null)
            {
                BufferTemp.Add(line);
            }
            else
            {
                newVersion.Add(formattedLine + line);
            }
        }

        /// <summary>
        /// write new line
        /// </summary>
        public void WriteLine()
        {
            newVersion.Add(string.Empty);
        }

        /// <summary>
        /// this will write a method call according to our uncrustify settings.
        /// if the line is longer than the maximum characters per line, then the parameters will be in the new line;
        /// </summary>
        /// <param name="ALine"></param>
        /// <param name="AParameters"></param>
        public void WriteLineMethodCall(string ALine, StringCollection AParameters)
        {
            WriteLine(WriteLineMethodCallToString(ALine, AParameters));
        }

        /// <summary>
        /// this will write a method call according to our uncrustify settings.
        /// if the line is longer than the maximum characters per line, then the parameters will be in the new line;
        /// </summary>
        /// <param name="ALine"></param>
        /// <param name="AParameters"></param>
        public string WriteLineMethodCallToString(string ALine, StringCollection AParameters)
        {
            string Result = String.Empty;
            int countLength = CurrentIndentCharacters + ALine.Length;

            foreach (string param in AParameters)
            {
                countLength += param.Length;
            }

            // add commas
            countLength += 2 * AParameters.Count - 1;

            // closing bracket and semicolon
            countLength += 2;

            if (countLength > CODE_LENGTH_UNCRUSTIFY)
            {
                bool first = true;

                foreach (string param in AParameters)
                {
                    if (!first)
                    {
                        ALine += ", ";
                        Result += ALine + Environment.NewLine;
                        ALine = "   ";
                    }

                    ALine += param;
                    first = false;
                }

                ALine += ");";
                Result += ALine + Environment.NewLine;
            }
            else
            {
                bool first = true;

                foreach (string param in AParameters)
                {
                    if (!first)
                    {
                        ALine += ", ";
                    }

                    ALine += param;
                    first = false;
                }

                ALine += ");";
                Result += ALine + Environment.NewLine;
            }

            return Result;
        }

        /// <summary>
        /// write a comment
        /// </summary>
        /// <param name="AComment"></param>
        public void WriteComment(string AComment)
        {
            StringCollection Lines = StringHelper.StrSplit(AComment, Environment.NewLine);

            if (Lines.Count > 1)
            {
                int count = 0;

                foreach (String line in Lines)
                {
                    if (count == 0)
                    {
                        WriteLine("/* " + line);
                    }
                    else if (count == Lines.Count - 1)
                    {
                        WriteLine("   " + line + Environment.NewLine + "*/");
                    }
                    else
                    {
                        WriteLine("   " + line);
                    }

                    count++;
                }
            }
            else
            {
                WriteLine("// " + AComment.Trim());
            }
        }

        /// <summary>
        /// start a code block
        /// </summary>
        /// <param name="ALine"></param>
        public void StartBlock(String ALine)
        {
            if (ALine.Length > 0)
            {
                WriteLine(ALine);
            }

            WriteLine("{");
            Indent();
        }

        /// <summary>
        /// close a code block, closing bracket
        /// </summary>
        public void EndBlock()
        {
            DeIndent();
            WriteLine("}");
            WriteLine();
        }

        /// <summary>
        /// indent the code by one level
        /// </summary>
        public void Indent()
        {
            indent++;
        }

        /// <summary>
        /// the code moves by one level to the left again
        /// </summary>
        public void DeIndent()
        {
            indent--;
        }

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
        public void AddParameter(ref string MethodDeclaration, ref bool firstParameter, int align,
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

            StrParameter += parameterType + (parameterType.EndsWith(">") ? "" : " ") + AParamName;

            MethodDeclaration += StrParameter;
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public AutoGenerationWriter()
        {
        }

        /// <summary>
        /// constructor, opens the file for reading, the file that we want to update
        /// </summary>
        /// <param name="AOutputFile"></param>
        public AutoGenerationWriter(string AOutputFile)
        {
            OpenFile(AOutputFile);
        }

        /// <summary>
        /// get the original version of the file that should be rewritten
        /// </summary>
        /// <param name="AOutputFile"></param>
        /// <returns></returns>
        public bool OpenFile(string AOutputFile)
        {
            OutputFile = AOutputFile;
            originalVersion = new List <string>();
            indent = 0;

            if (File.Exists(OutputFile))
            {
                // read current version of file to preserve manual changes in the permitted sections
                StreamReader tr = new StreamReader(OutputFile);

                // cannot use StringHelper.strsplit because it trims the lines
                string[] sArray = tr.ReadToEnd().Replace("\r", "").Split('\n');

                foreach (string s in sArray)
                {
                    originalVersion.Add(s);
                }

                tr.Close();
            }

            // open file for writing
            newVersion = new List <string>();
            return true;
        }

        Int32 FirstContains(StringCollection c, Int32 startIndex, string s)
        {
            if (startIndex < 0)
            {
                startIndex = 0;
            }

            for (Int32 counter = startIndex; counter < c.Count; counter++)
            {
                if (c[counter].IndexOf(s) != -1)
                {
                    return counter;
                }
            }

            return -1;
        }

        /// <summary>
        /// merge the new content into the existing file
        /// </summary>
        public void Close()
        {
            Close(true);
        }

        /// <summary>
        /// merge the new content into the existing file, if DoWrite is true
        /// </summary>
        /// <param name="DoWrite"></param>
        public void Close(bool DoWrite)
        {
            if (DoWrite)
            {
                // Merge the new file with the original file
                TFileDiffMerge.Merge2Files(OutputFile, newVersion.ToArray());
            }
        }
    }
}