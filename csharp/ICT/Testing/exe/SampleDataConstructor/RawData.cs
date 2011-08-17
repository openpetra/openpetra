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

namespace SampleDataConstructor
{
class RawData
{
    public enum filetypes
    {
        people = 1,
        organizations = 2,
        addresses = 3,
    }

    public Dictionary <filetypes, int>cntProcessedLines;

    public List <RPerson>People;
    public List <ROrganization>Organizations;
    public List <RLocation>Locations;
    public List <RMobilePhone>Mobilephones;
    public Dictionary <string, RCountry>Countries;

    public RawData()
    {
        People = new List <RPerson>();
        Organizations = new List <ROrganization>();
        Locations = new List <RLocation>();
        Mobilephones = new List <RMobilePhone>();
        Countries = new Dictionary <string, RCountry>();

        cntProcessedLines = new Dictionary <filetypes, int>();
        cntProcessedLines.Add(filetypes.people, 0);
        cntProcessedLines.Add(filetypes.organizations, 0);
        cntProcessedLines.Add(filetypes.addresses, 0);
    }

    //public void readRawDataFromFile(string filename);
    public void readRawDataFromFile(string filename, filetypes filetype, string directory)
    {
        Console.Write("Processing File: {0,20}", filename);
        filename = directory + filename;
        DataLine dL;
        QnDCSVRead csv = new QnDCSVRead(filename);
        int cntBegin = cntProcessedLines[filetype];

        while ((dL = csv.readNextLine()) != null)
        {
            switch (filetype)
            {
                case filetypes.people:
                    People.Add(new RPerson(dL));
                    break;

                case filetypes.organizations:
                    Organizations.Add(new ROrganization(dL));
                    break;

                case filetypes.addresses:
                    // the relevant data for each class is extracted by the classes constructor
                    Locations.Add(new RLocation(dL));
                    Mobilephones.Add(new RMobilePhone(dL));
                    RCountry.AddIfNew(dL, Countries);
                    break;

                default:
                    throw new Exception(
                    "I don't know how to process the filetype of this file: " + filename
                    );
            }

            cntProcessedLines[filetype]++;
        }

        int cntEnd = cntProcessedLines[filetype];
        Console.WriteLine(" Lines of this filetype (before) " + cntBegin + " (after) " + cntEnd);
    }
}
}