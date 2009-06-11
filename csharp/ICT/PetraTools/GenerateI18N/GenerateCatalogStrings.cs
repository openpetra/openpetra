/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.IO;
using Ict.Common;
using Ict.Tools.CodeGeneration;

namespace GenerateI18N
{
/// <summary>
/// Use all Text properties from the designer file and add Catalog.SetString in the constructor
/// </summary>
public class TGenerateCatalogStrings
{
    /// <summary>
    /// read the designer file and add the strings to the main file
    /// </summary>
    /// <param name="AMainFilename"></param>
    public static void Execute(string AMainFilename)
    {
        string DesignerFileName = Path.GetDirectoryName(AMainFilename) +
                                  Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(AMainFilename) + ".Designer.cs";
        StreamReader readerDesignerFile = new StreamReader(DesignerFileName);
        StreamReader readerMainFile = new StreamReader(AMainFilename);
        StreamWriter writer = new StreamWriter(AMainFilename + ".new");

        // find the call to InitializeComponent
        string line = "";

        while (!readerMainFile.EndOfStream && !line.Contains("InitializeComponent();"))
        {
            line = readerMainFile.ReadLine();
            writer.WriteLine(line);
        }

        if (readerMainFile.EndOfStream)
        {
            readerMainFile.Close();
            readerDesignerFile.Close();
            writer.Close();
            throw new Exception("Problem: cannot find InitializeComponent in " + AMainFilename);
        }

        string identation = "".PadLeft(line.IndexOf("InitializeComponent()"));

        writer.WriteLine(identation + "#region CATALOGI18N");

        // empty line for uncrustify
        writer.WriteLine();
        writer.WriteLine(
            identation + "// this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N");

        // parse the designer files and insert all labels etc into the main file
        while (!readerDesignerFile.EndOfStream)
        {
            string designerLine = readerDesignerFile.ReadLine();

            // catch all .Text = , but also TooltipsText = , but ignore lblSomethingText = new ...
            if (designerLine.Contains("Text = \""))
            {
                string content = designerLine.Substring(
                    designerLine.IndexOf("\"") + 1, designerLine.LastIndexOf("\"") - designerLine.IndexOf("\"") - 1);

                bool skipThisString = false;

                // if there is MANUALTRANSLATION then don't translate; that is a workaround for \r\n in labels;
                // see eg. Client\lib\MPartner\gui\UC_PartnerInfo.Designer.cs, lblLoadingPartnerLocation.Text
                if (content.Contains("MANUALTRANSLATION"))
                {
                    skipThisString = true;
                }

                if (content.Trim().Length == 0)
                {
                    skipThisString = true;
                }

                if (content.Trim() == "-")
                {
                    // menu separators etc
                    skipThisString = true;
                }

                if (!skipThisString)
                {
                    // careful with \n and \r in the string; that is not allowed by gettext
                    if (content.Contains("\\r") || content.Contains("\\n"))
                    {
                        throw new Exception("Problem with \\r or \\n in file " + DesignerFileName + ": " + designerLine);
                    }

                    writer.WriteLine(identation +
                        designerLine.Substring(0, designerLine.IndexOf(" = ")).Trim() +
                        " = Catalog.GetString(\"" + content + "\");");
                }
            }
        }

        writer.WriteLine(identation + "#endregion");

        readerDesignerFile.Close();

        bool skip = false;

        while (!readerMainFile.EndOfStream)
        {
            line = readerMainFile.ReadLine();

            if (line.Trim().StartsWith("#region CATALOGI18N"))
            {
                skip = true;
            }

            if (!skip)
            {
                writer.WriteLine(line);
            }

            if (skip && line.Trim().StartsWith("#endregion"))
            {
                skip = false;
            }
        }

        writer.Close();
        readerMainFile.Close();

        TTextFile.UpdateFile(AMainFilename);
    }
}
}