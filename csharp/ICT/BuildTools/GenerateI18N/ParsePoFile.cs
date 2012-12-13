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
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using Ict.Common;
using Ict.Common.IO;

namespace GenerateI18N
{
/// <summary>
/// parse a gettext po file
/// </summary>
public class TPoFileParser
{
    /// <summary>
    /// a line in a po translation file starts with either msgid or msgstr, and can cover several lines.
    /// the text is in quotes.
    /// </summary>
    public static string ParsePoLine(StreamReader sr, ref string ALine, out StringCollection AOriginalLines)
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
    /// add new translations to the po file
    /// </summary>
    public static void WriteUpdatedPoFile(string APoFilePath, SortedList <string, string>ANewTranslations)
    {
        List <string>pofile = new List <string>();

        if (ANewTranslations.Keys.Count > 0)
        {
            TLogging.Log("updating " + APoFilePath);

            // parse the whole po file
            StreamReader sr = new StreamReader(APoFilePath);
            Encoding enc = new UTF8Encoding(false);
            StreamWriter sw = new StreamWriter(APoFilePath + ".new", false, enc);

            string line = sr.ReadLine();

            while (line != null)
            {
                if (line.StartsWith("msgid \""))
                {
                    StringCollection OriginalLines;
                    string messageId = TPoFileParser.ParsePoLine(sr, ref line, out OriginalLines);

                    if (pofile.Contains(messageId))
                    {
                        // ignore this instance
                        TPoFileParser.ParsePoLine(sr, ref line, out OriginalLines);

                        TLogging.Log("duplicate messageid: " + messageId);
                    }
                    else
                    {
                        pofile.Add(messageId);
                    }

                    foreach (string s in OriginalLines)
                    {
                        sw.WriteLine(s);
                    }

                    if (ANewTranslations.ContainsKey(messageId))
                    {
                        // skip msgstr line
                        TPoFileParser.ParsePoLine(sr, ref line, out OriginalLines);

                        sw.WriteLine(String.Format("msgstr \"{0}\"", ANewTranslations[messageId]));

                        ANewTranslations.Remove(messageId);
                    }
                }
                else
                {
                    sw.WriteLine(line);
                    line = sr.ReadLine();
                }
            }

            sr.Close();
            sw.Close();

            TTextFile.UpdateFile(APoFilePath);

            foreach (string key in ANewTranslations.Keys)
            {
                TLogging.Log("Warning: cannot find in po file: " + key);
            }
        }
    }
}
}