//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, jomammele
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
using System.IO;
using System.Collections.Specialized;
using System.Text;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;

namespace GenerateI18N
{
/// <summary>
/// drop unwanted strings from the po file
/// </summary>
public class TDropUnwantedStrings
{
    /// <summary>
    /// collect all message ids of strings that should not be translated
    /// </summary>
    /// <param name="ADoNotTranslatePath"></param>
    /// <returns></returns>
    private static StringCollection GetDoNotTranslateStrings(string ADoNotTranslatePath)
    {
        StringCollection result = new StringCollection();
        StringCollection OriginalLines = new StringCollection();

        StreamReader sr = new StreamReader(ADoNotTranslatePath);
        string line = sr.ReadLine();

        while (line != null)
        {
            if (line.StartsWith("msgid"))
            {
                string messageId = TPoFileParser.ParsePoLine(sr, ref line, out OriginalLines);

                result.Add(messageId);
            }
            else
            {
                line = sr.ReadLine();
            }
        }

        sr.Close();

        return result;
    }

    /// <summary>
    /// remove all strings from po file that are listed in the "Do Not Translate" file
    /// </summary>
    /// <param name="ADoNotTranslatePath"></param>
    /// <param name="ATranslationFile"></param>
    public static void RemoveUnwantedStringsFromTranslation(string ADoNotTranslatePath, string ATranslationFile)
    {
        StringCollection DoNotTranslate = GetDoNotTranslateStrings(ADoNotTranslatePath);
        StreamReader sr = new StreamReader(ATranslationFile);
        Encoding enc = new UTF8Encoding(false);
        StreamWriter sw = new StreamWriter(ATranslationFile + ".new", false, enc);
        //create a template in which all the source links are contained
        StreamWriter sw_all = new StreamWriter(ATranslationFile + ".withallsources", false, enc);

        string line = sr.ReadLine();
        int counter = 0;

        while (line != null)
        {
            counter++;

            if (!line.StartsWith("msgid"))
            {
                sw_all.WriteLine(line);
                sw.WriteLine(line);
                line = sr.ReadLine();   //get the empty line

                if (line != null)
                {
                    if (line.Contains("todoComment"))
                    {
                        //Console.WriteLine("here");
                    }

                    while (line.StartsWith("#.") && !line.Contains("todoComment"))   //take over the comments(if they exist)
                    {
                        string line_part1 = AdaptString(line, "_ ");
                        string line_part2 = AdaptString(line_part1, "<summary>");
                        string line_part3 = AdaptString(line_part2, "</summary>");

                        sw_all.WriteLine(line_part3);
                        sw.WriteLine(line_part3);
                        line = sr.ReadLine();
                    }

                    if (line.StartsWith("#:"))   //take over the first source code line (if it exists)
                    {
                        sw_all.WriteLine(line);

                        if (line.Contains("GenerateI18N.CollectedGettext.cs"))
                        {
                            sw.WriteLine("#: - This item was created automatically from a designer file");
                            line = sr.ReadLine();
                        }
                        else
                        {
                            string currentLine = line;
                            line = sr.ReadLine();

                            if (line.StartsWith("#:"))
                            {
                                sw.WriteLine(
                                    currentLine + " (first of several occurrences - the whole list can be found in i8n/template.pot.withallsources)");
                            }
                            else
                            {
                                sw.WriteLine(currentLine);
                            }
                        }
                    }

                    while (line.StartsWith("#:") || line.StartsWith("#,"))  //ignore all other source code lines
                    {
                        sw_all.WriteLine(line);
                        line = sr.ReadLine();
                    }

                    if (line.Contains("todoComment"))
                    {
                        line = sr.ReadLine();   //ignore todoComment
                    }
                }
            }
            else if ((line != null) && line.StartsWith("msgid"))
            {
                if (line.Contains("Maintain Month Names for Different Languages"))
                {
                    // Console.WriteLine("here");
                }

                StringCollection OriginalLines;
                string messageId = TPoFileParser.ParsePoLine(sr, ref line, out OriginalLines);

                if (DoNotTranslate.Contains(messageId))
                {
                    if (!line.StartsWith("msgstr"))
                    {
                        throw new Exception("did expect msgstr in the line");
                    }

                    TPoFileParser.ParsePoLine(sr, ref line, out OriginalLines);
                }
                else
                {
                    foreach (string s in OriginalLines)
                    {
                        sw_all.WriteLine(s);
                        sw.WriteLine(s);
                    }
                }
            }
        }

        sr.Close();
        sw_all.Close();
        sw.Close();

        TTextFile.UpdateFile(ATranslationFile);

        ReviewTemplateFile(ATranslationFile);
    }

    /// <summary>
    /// open template.pot again and remove source code lines of not translated msgids
    /// </summary>
    private static void ReviewTemplateFile(string ATranslationFile)
    {
        StreamReader sr = new StreamReader(ATranslationFile);
        Encoding enc = new UTF8Encoding(false);
        StreamWriter sw = new StreamWriter(ATranslationFile + ".new", false, enc);

        string line = sr.ReadLine();

        while (line != null)
        {
            if (line.StartsWith("#:"))
            {
                string currentLine;

                if (line.Contains("csharp"))
                {
                    //cut away first part of source location
                    currentLine = line.Remove(3, line.IndexOf("csharp") - 3);
                }
                else
                {
                    currentLine = line;
                }

                //write line only to new template file if the following line is not empty
                line = sr.ReadLine();

                if (line != "")
                {
                    sw.WriteLine(currentLine);
                }
            }
            else if (line == "")
            {
                //add empty line only if the next line is not aswell an empty line
                string currentLine = line;
                line = sr.ReadLine();

                if (line != "")
                {
                    sw.WriteLine(currentLine);
                }
            }
            else
            {
                sw.WriteLine(line);
                line = sr.ReadLine();
            }

            /*if(line.Contains("txtAutoPopulatedButtonLabel"))
             * {
             *  Console.WriteLine("here");
             * }*/
        }

        sr.Close();
        sw.Close();

        TTextFile.UpdateFile(ATranslationFile);
    }

    /// <summary>
    /// remove a substring from a String
    /// </summary>
    /// <param name="ALine">String from which the substring should be removed</param>
    /// <param name="ARemoveString">substring to remove</param>
    private static string AdaptString(string ALine, string ARemoveString)
    {
        string ALine_part;

        if (ALine.Contains(ARemoveString))
        {
            ALine_part = ALine.Remove(ALine.IndexOf(ARemoveString), ARemoveString.Length);
        }
        else
        {
            ALine_part = ALine;
        }

        return ALine_part;
    }
}
}