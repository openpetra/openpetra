//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       thomass
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
//// using System.Linq;
using System.Text;

namespace SampleDataConstructor
{
class DataLine
{
    private Dictionary <string, int>existingHeaders;
    private string[] dataLine;

    public void mustSet(ref string s, string fieldName)
    {
        s = getField(fieldName);
    }

    public void mustSet(ref char c, string fieldName)
    {
        string s = getField(fieldName).Trim();

        if (s.Length != 1)
        {
            throw new Exception(
                "Illegal Data: " + s + " seems to contain several characters");
        }

        c = s[0];
    }

    public void mustSet(ref int i, string fieldName)
    {
        string s = getField(fieldName);

        try
        {
            i = int.Parse(s);
        }
        catch (Exception e)
        {
            throw new Exception("Illegal data: '" + s + "' could not be transformed into an integer ", e);
        }
    }

    public void maySet(ref string s, string fieldName)
    {
        if (headerExists(fieldName))
        {
            mustSet(ref s, fieldName);
        }
    }

    public void tryToSet(ref int i, string fieldName)
    {
        if (headerExists(fieldName))
        {
            mustSet(ref i, fieldName);
        }
    }

    public void tryToSet(ref char c, string fieldName)
    {
        if (headerExists(fieldName))
        {
            mustSet(ref c, fieldName);
        }
    }

    public bool headerExists(string header)
    {
        return existingHeaders.ContainsKey(header);
    }

    public string getField(string fieldName)
    {
        return dataLine[existingHeaders[fieldName]];
    }

    public DataLine(Dictionary <string, int>existingHeaders, string[] dataLine)
    {
        this.existingHeaders = existingHeaders;
        this.dataLine = dataLine;
    }
}
}