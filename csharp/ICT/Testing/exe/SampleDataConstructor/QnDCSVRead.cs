//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       >>>> Put your full name or just a shortname here <<<<
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
using System.Collections.Generic;
using System.IO;


namespace SampleDataConstructor
{
/**
 * <summary>
 * Quick and dirty CSV reader for sample import only.
 * </summary>
 *
 * For quick use and practicing purposes only.
 * Proper solution could be:
 * * csv to xml convert, and use that (as done somewhere else in OpenPetra)
 * * http://www.codeproject.com/KB/database/CsvReader.aspx (this one is _nice_!)
 *
 * This was _really_ meant as a minimal and practicing solution only.
 * Assumptions:
 * * no commata in fields (ie. not quoted) - but will throw an error if that is not true, ie too many fields
 * * first line contains field names
 *
 * Sample use:
 * <code>
 * QnDCSVRead csv = new QnDCSVRead("filename.csv");
 * DataLine dataLine;
 * while ((dataLine = csv.readNextLine()) != null) {
 *   Console.Writeln("firstname: " + dataLine.getField("firstname"));
 * }
 * </code>
 */
class QnDCSVRead
{
    private string _filename;
    private StreamReader _sr;
    private Dictionary <string, int>headers;    // DOXYGYNIZE!: headers["firstname"] == 0 , headers["familyname"] == 1 ...
    private int headerCount;
    private int lastAttemptedLine;
    private int lastSuccessfulLine;
    private string[] _curline = null;

    public QnDCSVRead(string Filename)
    {
        // code mainly copied from Microsofts .NET documentation:
        // http://msdn.microsoft.com/en-us/library/system.io.streamreader.aspx

        _filename = Filename;     // used for verbose error messages
        _sr = new StreamReader(Filename);

        // first line of the csv should be headers - get them
        headers = new Dictionary <string, int>();
        // MUST: either do proper split yourself, or use existing open source csv code
        // MAY: use CSV to XML instead
        string[] headers_tmp = _sr.ReadLine().Split(',');

        for (int cnt = 0; cnt < headers_tmp.GetLength(0); cnt++)
        {
            headers[headers_tmp[cnt]] = cnt;
        }

        headerCount = headers.Count;
        lastAttemptedLine = 1;
        lastSuccessfulLine = 1;
        // the next line to be read will be the data
    }

    public Dictionary <string, int>getHeaders()
    {
        return headers;
    }

    public int hpos(string key)
    {
        return (int)(headers[key]);
    }

    public string[] dc
    {
        get
        {
            return _curline;
        }
    }
    public int LinesRead
    {
        get
        {
            return lastAttemptedLine;
        }
    }
    public int DataLinesRead
    {
        get
        {
            if (lastSuccessfulLine <= 1)
            {
                return 0;
            }
            else
            {
                return lastSuccessfulLine;
            }
        }
    }

    public string[] readNextLineSA()
    {
        string line = _sr.ReadLine();

        if (line == null)
        {
            return null;
        }

        lastAttemptedLine++;
        string[] data = line.Split(',');

        if (data.GetLength(0) != headerCount)
        {
            throw new Exception(
                "Inappropriate amount of elements when reading line " + lastAttemptedLine +
                " in " + _filename
                );
        }

        lastSuccessfulLine = lastAttemptedLine;
        _curline = data;
        return _curline;
    }

    public DataLine readNextLine()
    {
        string[] data;

        if ((data = readNextLineSA()) != null)
        {
            return new DataLine(this.getHeaders(), data);
        }
        else
        {
            return null;
        }
    }
}
}