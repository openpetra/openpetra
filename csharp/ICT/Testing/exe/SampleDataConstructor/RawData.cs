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

        public Dictionary<filetypes, int> cntProcessedLines;

        public List<RPerson> People;
        public List<ROrganization> Organizations;
        public List<RLocation> Locations;
        public List<RMobilePhone> Mobilephones;
        public Dictionary<string, RCountry> Countries;

        public RawData()
        {
            People = new List<RPerson>();
            Organizations = new List<ROrganization>();
            Locations = new List<RLocation>();
            Mobilephones = new List<RMobilePhone>();
            Countries = new Dictionary<string, RCountry>();

            cntProcessedLines = new Dictionary<filetypes, int>();
            cntProcessedLines.Add(filetypes.people, 0);
            cntProcessedLines.Add(filetypes.organizations, 0);
            cntProcessedLines.Add(filetypes.addresses, 0);
        }

        //public void readRawDataFromFile(string filename);
        public void readRawDataFromFile(string filename, filetypes filetype, string directory)
        {
            Console.Write("Processing File: {0,20}",filename);
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
