//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2016 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Common.IO
{
    /// <summary>
    /// TSimpleYmlParser is mainly used for restoring a database saved as yml.gz;
    /// avoids loading a huge XmlDocument with all tables at once.
    /// for smaller files, the other implementation, TYml2Xml, is much easier to step through, since you get an XmlDocument
    /// </summary>
    public class TSimpleYmlParser : TYml2Xml
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AFilename"></param>
        public TSimpleYmlParser(string AFilename) : base(AFilename)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TSimpleYmlParser(string[] ALines) : base(ALines)
        {
        }

        /// <summary>
        /// node name with the start and end line number of the children
        /// </summary>
        private SortedList <string, int[]>Captions = null;

        private void ParseCaptionsInternal()
        {
            string line = GetNextLine();

            if (line != null)
            {
                string nodeName, nodeContent;

                if (SplitNode(line, out nodeName, out nodeContent))
                {
                    if ((nodeContent.Length == 0) && (GetIndentationNext(currentLine) > 0))
                    {
                        int startLine = currentLine + 1;

                        // There are children
                        Int32 childrenAbsoluteIndentation = GetAbsoluteIndentationNext(currentLine);

                        do
                        {
                            ParseCaptionsInternal();
                        } while (GetAbsoluteIndentationNext(currentLine) == childrenAbsoluteIndentation);

                        int endLine = currentLine;

                        Captions.Add(nodeName, new int[] { startLine, endLine });
                    }
                }
            }
        }

        /// <summary>
        /// parse all captions, ie nodes that do not have attributes, but children
        /// </summary>
        public void ParseCaptions()
        {
            Captions = new SortedList <string, int[]>();

            currentLine = 0;

            while (currentLine < lines.Length)
            {
                ParseCaptionsInternal();
            }
        }

        int StartLine = -1;
        int LastLine = -1;

        /// <summary>
        /// parse a list of simple rows, with their attributes
        /// </summary>
        /// <param name="ACaption"></param>
        /// <returns></returns>
        public bool StartParseList(string ACaption)
        {
            if (!Captions.ContainsKey(ACaption))
            {
                return false;
            }

            StartLine = Captions[ACaption][0] - 1;
            LastLine = Captions[ACaption][1];

            currentLine = StartLine;

            return true;
        }

        /// <summary>
        /// get the attributes of the next row
        /// </summary>
        /// <returns></returns>
        public SortedList <string, string>GetNextLineAttributes()
        {
            string line = GetNextLine();

            if ((line != null) && (currentLine <= LastLine))
            {
                string nodeName, nodeContent;

                if (SplitNode(line, out nodeName, out nodeContent))
                {
                    if (nodeContent.StartsWith("{"))
                    {
                        SortedList <string, string>Result = new SortedList <string, string>();
                        Result.Add("RowName", nodeName);

                        // first get the list without brackets
                        string list = nodeContent.Substring(1, nodeContent.Length - 2).Trim();

                        // now use getNextCSV which is able to deal with quoted strings
                        while (list.Length > 0)
                        {
                            string mapping = StringHelper.GetNextCSV(ref list, ",");
                            string mappingName = StringHelper.GetNextCSV(ref mapping, new string[] { "=", ":" }).Trim();
                            string mappingValue = mapping.Trim();

                            if (mappingValue.Length > 1 && mappingValue[0] == '"')
                            {
                                mappingValue = StripQuotes(mappingValue);
                                mappingValue = mappingValue.Replace("\\\\", "\\");
                            }

                            mappingValue = mappingValue.Replace("\\n", Environment.NewLine);
                            Result.Add(mappingName, mappingValue);
                        }

                        return Result;
                    }
                }
            }

            return null;
        }
    }
}
