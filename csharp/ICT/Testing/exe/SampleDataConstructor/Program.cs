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
using System.Collections.Specialized;

using Ict.Testing.NUnitPetraServer;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Interfaces.MSysMan.ImportExport.WebConnectors;
using Ict.Petra.Server.MSysMan.ImportExport.WebConnectors;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Server.App.Core;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Common;
using SampleDataConstructor;

namespace Ict.Testing.SampleDataConstructor
{
    /// <summary>
    /// This class creates sample data (partners, organisations, gifts) and imports them into OpenPetra.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The class requires raw data to have been created already by benerator, reads this data, enhances
    /// and compiles it (using the literal meaning of "compile", i.e. putting together People, Addresses,
    /// Phonenumbers to create partners), and them imports this data to the OpenPetra Server.
    /// </para>
    /// <para>
    /// Generally, the Sample Data creator DOES NOT use the Petra Model internally,
    /// although it tries to stay close to it ( e.g. Naming Convention).
    /// This is so it can run a simple simulation for creating events (marriages resulting in same location, children, gift entries).
    /// These can then be saved in Petra.
    /// </para>
    /// <para>
    /// Classes starting in "R" (RPartner) are raw data for later use.
    /// The actual Petra data is stored in a SampleDataConstructorTDS.
    /// </para>
    ///
    /// TODO: Check comment from Timo: This is actually rather a tool than a test
    /// - so one could change it's location.
    /// TODO: Call (1) Datagenerator (2) SampleDataConstructur - from nant (timo)
    /// </remarks>
    class TSampleDataConstructor
    {
        public static void doReport(ExecutionReport report)
        {
            TLogging.Log("\t" + report.ToString());
        }

        /// <summary>
        /// Creates Sample Data using the raw data provided and exports this to the Petra Server
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            TLogging.Log("Running Sample Data Constructor");

            // Execution is split into several steps:
            // 0) Initialize, check availability of resources, connect to server
            // 1) Read raw data files into memory
            // 2) Create OpenPetra Sample data from raw data (in a local TDS)
            // 3) [Create Simulated financial data - not for now]
            // 4) Save to Server


            ExecutionReport report;
            try
            {
                TLogging.Log("(0) Initialize (check availability of resources, start the server)");

                TLogging.Log("\tStarting the server...");

                // use the config file defined on the command line with -C:
                TPetraServerConnector.Connect(string.Empty);

                //// WISHLIST: check that database is empty before attempting to fill
                // checkDatabaseEmpty();

                // data directory containing the raw data files created by benerator
                // Please forgive me: dd = dataDirectory</remarks>
                string dd = TAppSettingsManager.GetValue("dir.data.generated");


                TLogging.Log("(1) Read raw data files into memory");

                RawData rawDataDe = new RawData(); // Raw Data Germany
                rawDataDe.AddDataSourceCSV("People.csv", RawData.FileTypes.people, dd);
                rawDataDe.AddDataSourceCSV("Organisations.csv", RawData.FileTypes.organizations, dd);
                rawDataDe.AddDataSourceCSV("Addresses.csv", RawData.FileTypes.addresses, dd);
                rawDataDe.LoadAllData();
                // Print some stats
                TLogging.Log("\tPeople:        " + rawDataDe.People.Count);
                TLogging.Log("\tOrganizations: " + rawDataDe.Organizations.Count);
                TLogging.Log("\tLocations:     " + rawDataDe.Locations.Count);
                TLogging.Log("\tMobile Phones: " + rawDataDe.Mobilephones.Count);
                TLogging.Log("\tCountries:     " + rawDataDe.Countries.Count);

                TLogging.Log("(2) Creating Sample Data from raw data (in local TDS)");

                SampleDataConstructorTDS dataTDS = new SampleDataConstructorTDS();
                ConstructionStats constructionStats = new ConstructionStats();
                Stack <PLocationRow>unusedLocations = new Stack <PLocationRow>();

                DataBuilder.insertPeople(dataTDS, rawDataDe, out report); doReport(report);
                DataBuilder.insertOrganisations(dataTDS, rawDataDe, out report); doReport(report);
                // Units? No units for now (could be copied from ImportExportYML)
                // Subscriptions? No Subscriptions for now
                DataBuilder.insertLocations(dataTDS, unusedLocations, rawDataDe, out report); doReport(report);
                // DataBuilder.insertSpecialTypes(dataTDS);

                // DataBuilder.initMobilePhones(dataTDS,rawData, out report);
                // DataBuilder.initCountries(dataTDS,rawData, out report);

                DataBuilder.AssignHomesToPartners(
                    dataTDS, unusedLocations,
                    constructionStats.PeopleWithHomeKnown,
                    out report); doReport(report);

                // DataBuilder.assignSpecialTypesToPartners(dataTDS,rawData,supporterStats,out report); doReport(report);


                // WISHLIST: Questions regarding how what financial data to be created is "helpful"
                // should ask : Rob or Timo

                TLogging.Log("(3) Save to Server");

                TVerificationResultCollection VerificationResult;

                if (!TImportExportWebConnector.SaveTDS(dataTDS, out VerificationResult))
                {
                    TLogging.Log(VerificationResult.BuildVerificationResultString());
                    throw new Exception("Error saving to database: " + VerificationResult.BuildVerificationResultString());
                }

                TLogging.Log("Completed.");
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
            }
        }
    }
}