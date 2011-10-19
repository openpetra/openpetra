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
    /// a line in a po translation file starts with either msgid or msgstr, and can cover several lines.
    /// the text is in quotes.
    /// </summary>
    private static string ParsePoLine(StreamReader sr, ref string ALine, out StringCollection AOriginalLines)
    {
        AOriginalLines = new StringCollection();
        AOriginalLines.Add(ALine);

        string messageId = String.Empty;
        StringHelper.GetNextCSV(ref ALine, " ");
        string quotedMessage = StringHelper.GetNextCSV(ref ALine, " ");

        if (quotedMessage.StartsWith("\""))
        {
            quotedMessage = quotedMessage.Substring(1, quotedMessage.Length - 2);
        }

        messageId += quotedMessage;

        ALine = sr.ReadLine();

        while (ALine.StartsWith("\""))
        {
            AOriginalLines.Add(ALine);
            messageId += ALine.Substring(1, ALine.Length - 2);
            ALine = sr.ReadLine();
        }

        return messageId;
    }

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
                string messageId = ParsePoLine(sr, ref line, out OriginalLines);

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

        string line = sr.ReadLine();

        while (line != null)
        {
            if (!line.StartsWith("msgid"))
            {
                sw.WriteLine(line);
                line = sr.ReadLine();
            }
            else
            {
                StringCollection OriginalLines;
                string messageId = ParsePoLine(sr, ref line, out OriginalLines);

                if (DoNotTranslate.Contains(messageId))
                {
                    if (!line.StartsWith("msgstr"))
                    {
                        throw new Exception("did expect msgstr in the line");
                    }

                    ParsePoLine(sr, ref line, out OriginalLines);
                }
                else
                {
                    foreach (string s in OriginalLines)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
        }

        sr.Close();
        sw.Close();
        TTextFile.UpdateFile(ATranslationFile);
    }
}
}