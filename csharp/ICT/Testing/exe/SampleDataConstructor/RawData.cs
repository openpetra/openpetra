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
using System.IO;
using System.Collections.Generic;
using Ict.Common;

namespace SampleDataConstructor
{
/// <summary>
/// Contains information about a csv file and it's type.
/// Is used for later reading that file while knowing what sort of data the file contains.
/// </summary>
class RawDataSourceCSVFile
{
    public string FilePath;
    public RawData.FileTypes FileType;

    public RawDataSourceCSVFile(string Filename, RawData.FileTypes FileType, string Directory)
    {
        string FilePath = Path.Combine(Directory, Filename);

        if (!File.Exists(FilePath))
        {
            throw new Exception("Given File \"" + FilePath + "\" does not exist");
        }

        // We purposely leave away other checks, to keep things simple (developers only)
        this.FilePath = FilePath;
        this.FileType = FileType;
    }
}

class RawData
{
    public enum FileTypes
    {
        people = 1,
        organizations = 2,
        addresses = 3,
    }

    public Dictionary <FileTypes, int>cntProcessedLines;

    // Data source information (for now, the csv files), are stored in these variables:
    private List <RawDataSourceCSVFile>CSVDataSources;

    // Rawdata is saved in these variables:
    public List <RPerson>People;
    public List <ROrganization>Organizations;
    public List <RLocation>Locations;
    public List <RMobilePhone>Mobilephones;
    public Dictionary <string, RCountry>Countries;

    public RawData()
    {
        CSVDataSources = new List <RawDataSourceCSVFile>();

        People = new List <RPerson>();
        Organizations = new List <ROrganization>();
        Locations = new List <RLocation>();
        Mobilephones = new List <RMobilePhone>();
        Countries = new Dictionary <string, RCountry>();

        cntProcessedLines = new Dictionary <FileTypes, int>();
        cntProcessedLines.Add(FileTypes.people, 0);
        cntProcessedLines.Add(FileTypes.organizations, 0);
        cntProcessedLines.Add(FileTypes.addresses, 0);
    }

    /// <summary>
    /// Add Data Sources (in this case, csv-files) from which data will later be read
    /// </summary>
    /// <example><code>
    /// AddDataSourcesCSV("people.csv",RawData.FileTypes.people,"/mydata/people");
    /// </code></example>
    /// <param name="FileName"></param>
    /// <param name="FileType"></param>
    /// <param name="Directory"></param>
    public void AddDataSourceCSV(string FileName, FileTypes FileType, string Directory)
    {
        CSVDataSources.Add(new RawDataSourceCSVFile(FileName, FileType, Directory));
    }

    /// <summary>
    /// Load All Data from the previously defined DataSources
    /// </summary>
    public void LoadAllData()
    {
        foreach (var CSVDataSource in CSVDataSources)
        {
            // btw: Logging is of type "Info" (sorry, we don't seem to use that)
            TLogging.Log("\treadRawDataFromFile: processing File " + CSVDataSource.FilePath);
            readRawDataFromFile(CSVDataSource.FilePath, CSVDataSource.FileType);
        }
    }

    //public void readRawDataFromFile(string filename);
    public ExecutionReport readRawDataFromFile(string FilePath, FileTypes FileType)
    {
        DataLine dL;
        QnDCSVRead csv = new QnDCSVRead(FilePath);
        int cntBegin = cntProcessedLines[FileType];

        while ((dL = csv.readNextLine()) != null)
        {
            switch (FileType)
            {
                case FileTypes.people:
                    People.Add(new RPerson(dL));
                    break;

                case FileTypes.organizations:
                    Organizations.Add(new ROrganization(dL));
                    break;

                case FileTypes.addresses:
                    // the relevant data for each class is extracted by the classes constructor
                    Locations.Add(new RLocation(dL));
                    Mobilephones.Add(new RMobilePhone(dL));
                    RCountry.AddIfNew(dL, Countries);
                    break;

                default:
                    throw new Exception(
                    "I don't know how to process the filetype of this file: " + FilePath
                    );
            }

            cntProcessedLines[FileType]++;
        }

        int cntEnd = cntProcessedLines[FileType];
        return new ExecutionReport(" Lines of this filetype (before) " + cntBegin + " (after) " + cntEnd);
    }
}
}