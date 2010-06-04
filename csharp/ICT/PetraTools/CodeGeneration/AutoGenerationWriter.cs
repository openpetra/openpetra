//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using Ict.Common;
using Ict.Common.IO;
using DDW;

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
        public static Int32 CODE_LENGTH_UNCRUSTIFY = 150;

        private Int16 indent = 0;
        private TextWriter tw;
        private StringCollection originalVersion;
        private StringCollection BufferTemp = null;
        private string LastWrittenLine = ""; /// needed for synchronizing
        private bool Synchronized = false;
        private bool UsingSync = false;
        private Int32 CurrentOriginalLine = -1;
        private string OutputFile;

        /// <summary>
        /// this switches the current output from writing to the file to writing to a buffer
        /// this helps with conditional writing (eg only write namespace, if there is a valid interface to write)
        /// sync will be disabled, and applied when the buffer is written for real
        /// </summary>
        public void StartWriteToBuffer()
        {
            BufferTemp = new StringCollection();
        }

        /// <summary>
        /// will clear the buffer
        /// </summary>
        /// <returns>the buffer that has been filled until this call</returns>
        public StringCollection FinishWriteToBuffer()
        {
            StringCollection ReturnValue = BufferTemp;

            BufferTemp = null;
            return ReturnValue;
        }

        public void WriteBuffer(StringCollection ABuffer)
        {
            foreach (string s in ABuffer)
            {
                WriteLine(s);
            }
        }

        private Int32 SkipEmptyLinesInOriginal(Int32 ACurrentOriginalLine)
        {
            while (originalVersion[ACurrentOriginalLine + 1].Trim().Length == 0
                   && ACurrentOriginalLine + 2 < originalVersion.Count)
            {
                // uncrustify sometimes adds empty lines before comments etc.
                // even before commented lines; therefore skip empty lines if necessary
                ACurrentOriginalLine++;
            }

            return ACurrentOriginalLine;
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

            if (UsingSync || (BufferTemp != null))
            {
                if (CurrentOriginalLine + 1 >= originalVersion.Count)
                {
                    Synchronized = false;
                }
                else if (Synchronized)
                {
                    CurrentOriginalLine = SkipEmptyLinesInOriginal(CurrentOriginalLine);

                    if (originalVersion[CurrentOriginalLine + 1].Trim().StartsWith("#region ManualCode"))
                    {
                        // copy the manual code over to the new document
                        while (CurrentOriginalLine + 1 < originalVersion.Count
                               && !originalVersion[CurrentOriginalLine + 1].Trim().StartsWith("#endregion"))
                        {
                            tw.WriteLine(originalVersion[CurrentOriginalLine + 1]);
                            CurrentOriginalLine++;
                        }

                        if (CurrentOriginalLine + 1 >= originalVersion.Count)
                        {
                            throw new Exception("region ManualCode was not closed");
                        }

                        tw.WriteLine(originalVersion[CurrentOriginalLine + 1]);
                        CurrentOriginalLine++;
                    }

                    CurrentOriginalLine = SkipEmptyLinesInOriginal(CurrentOriginalLine);

                    // if line has been commented, comment it again; e.g. for using clauses
                    if ((originalVersion[CurrentOriginalLine + 1].IndexOf(line.Trim()) != -1)
                        && originalVersion[CurrentOriginalLine + 1].Trim().StartsWith("//"))
                    {
                        line = originalVersion[CurrentOriginalLine + 1];
                    }

                    if (originalVersion[CurrentOriginalLine + 1].Trim() != line.Trim())
                    {
                        // try to find if it is just a longer namespaces; then use the generated version
                        // eg. "out System.Int64 AMergedIntoPartnerKey," vs "out Int64 AMergedIntoPartnerKey,"
                        try
                        {
                            StringCollection origWords = StringHelper.StrSplit(originalVersion[CurrentOriginalLine + 1].Trim(), " ");
                            StringCollection newWords = StringHelper.StrSplit(line.Trim(), " ");

                            if ((origWords.Count == newWords.Count)
                                && (origWords[origWords.Count - 1] == newWords[newWords.Count - 1]))
                            {
                                Console.WriteLine("assuming it is the same: ");
                                Console.WriteLine("   " + originalVersion[CurrentOriginalLine + 1].Trim());
                                Console.WriteLine("   " + line.Trim());
                                originalVersion[CurrentOriginalLine + 1] = line;
                            }
                        }
                        catch (Exception)
                        {
                            // good try; probably we failed synchronization
                        }
                    }

                    // check if we are still synchronized
                    if (originalVersion[CurrentOriginalLine + 1].Trim() == line.Trim())
                    {
                        CurrentOriginalLine++;
                    }
                    else
                    {
                        // 2 possibilities:
                        //   A there is new code that needs to be inserted
                        //   B or there is code that needs to be deleted from the original file
                        // B has now been implemented as well; it will pick a significant line (more than 5 characters length and try to find it in the original lines)
                        // A is implemented in the way, that only the first difference is logged.
                        //   as soon as there is same code again, the synchronization is picked up again.
                        //   if at the end of the file there is no synchronisation, then the writing fails with an exception

                        Console.WriteLine("not in sync anymore; line in Original file: " + CurrentOriginalLine.ToString());
                        Console.WriteLine("Original: " + originalVersion[CurrentOriginalLine + 1].Trim());
                        Console.WriteLine("New Line: " + line.Trim());
                        Synchronized = false;
                    }
                }
                else // not in sync at the moment
                {
                    // we are in sync mode,
                    // but at the moment we are not synchronized.
                    // if the new line matches the current line in the original file,
                    // then we are back in sync

                    // to skip lines, don't try doing it with a line that is too short (empty lines, brackets etc)

                    int skipLines = 0;

                    while (!Synchronized && (skipLines == 0 || line.Trim().Length > 5) && CurrentOriginalLine + 1 + skipLines < originalVersion.Count)
                    {
                        // if line has been commented, comment it again; e.g. for using clauses
                        if ((originalVersion[CurrentOriginalLine + 1 + skipLines].IndexOf(line.Trim()) != -1)
                            && originalVersion[CurrentOriginalLine + 1 + skipLines].Trim().StartsWith("//"))
                        {
                            line = originalVersion[CurrentOriginalLine + 1 + skipLines];
                        }

                        if (originalVersion[CurrentOriginalLine + 1 + skipLines].Trim() == line.Trim())
                        {
                            // situation A: there is new code added to the file
                            Console.WriteLine("back in sync: {0} {1}", CurrentOriginalLine + skipLines, line.Trim());
                            Synchronized = true;
                            CurrentOriginalLine += skipLines + 1;
                        }

                        skipLines++;
                    }
                }
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
                tw.WriteLine(formattedLine + line);
            }

            // to make synchronization possible
            LastWrittenLine = line;
        }

        public void WriteLine()
        {
            tw.WriteLine();

            if (Synchronized)
            {
                if ((CurrentOriginalLine + 1 < originalVersion.Count)
                    && (originalVersion[CurrentOriginalLine + 1].Trim() == ""))
                {
                    CurrentOriginalLine++;
                }
            }
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

        public void StartBlock(String ALine)
        {
            if (ALine.Length > 0)
            {
                WriteLine(ALine);
            }

            WriteLine("{");
            Indent();
        }

        public void EndBlock()
        {
            DeIndent();
            WriteLine("}");
            WriteLine();
        }

        public void Indent()
        {
            indent++;
        }

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
        /// <param name="AParamType"></param>
        public void AddParameter(ref string MethodDeclaration, ref bool firstParameter, int align,
            string AParamName, Modifier AParamModifier, IType AParamType)
        {
            if (!firstParameter)
            {
                MethodDeclaration += "," + Environment.NewLine;
                MethodDeclaration += new String(' ', align);
            }

            firstParameter = false;

            String parameterType = CSParser.GetName(AParamType);
            String StrParameter = "";

            if ((AParamModifier & Modifier.Ref) != 0)
            {
                StrParameter += "ref ";
            }
            else if ((AParamModifier & Modifier.Out) != 0)
            {
                StrParameter += "out ";
            }

            StrParameter += parameterType + (parameterType.EndsWith(">") ? "" : " ") + AParamName;

            MethodDeclaration += StrParameter;
        }

        public AutoGenerationWriter()
        {
        }

        public AutoGenerationWriter(string AOutputFile)
        {
            OpenFile(AOutputFile);
        }

        public bool OpenFile(string AOutputFile)
        {
            OutputFile = AOutputFile;
            originalVersion = new StringCollection();
            UsingSync = false;
            Synchronized = false;
            CurrentOriginalLine = -1;
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
            tw = new StreamWriter(OutputFile + ".new");
            return tw != null;
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

        /** finds the last written line in the original file
         *  this line should be unique
         */
        public void SynchronizeLines()
        {
            // only search after the previous CurrentOriginalLine
            Int32 OrigCurrentOriginalLine = CurrentOriginalLine;

            CurrentOriginalLine = FirstContains(originalVersion, CurrentOriginalLine - 1, LastWrittenLine);
            Synchronized = (CurrentOriginalLine != -1);

            if ((Synchronized == false) && (originalVersion.Count > 0))
            {
                throw new Exception(
                    "cannot synchronize lines because we cannot find line '" + LastWrittenLine + "' in file '" + OutputFile + ":" +
                    OrigCurrentOriginalLine.ToString() + "'");
            }

            UsingSync = true;
        }

        public void Close()
        {
            Close(true);
        }

        public void Close(bool DoWrite)
        {
            tw.Close();

            if (UsingSync && !Synchronized && (originalVersion.Count > 0))
            {
                throw new AutoGenerationSyncBrokeException("sync broke; please check the file " + OutputFile + ".new " +
                    "(easiest solution: merge changes from " +
                    Path.GetFileName(OutputFile) + ".new into " +
                    Path.GetFileName(OutputFile) + ")");
            }

            if (DoWrite)
            {
                // only do a backup if the file is not the same
                if (TTextFile.UpdateFile(OutputFile))
                {
                    //Console.WriteLine("Wrote file " + OutputFile);
                }
            }
        }
    }

    public class AutoGenerationSyncBrokeException : Exception
    {
        public AutoGenerationSyncBrokeException(String msg) : base(msg)
        {
        }
    }
}