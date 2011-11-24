//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       thomass (on the basis of work done by timop)
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
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner;
using Ict.Petra.Server.MPartner.Partner.Data.Access;


namespace SampleDataConstructor
{
/// <summary>
/// This class bundles executive summary of the last action
/// (e.g. in human readable form).
/// </summary>
class ExecutionReport
{
    string lastActionHumanReadable;
    public ExecutionReport(string lastActionHumanReadable)
    {
        this.lastActionHumanReadable = lastActionHumanReadable;
    }

    public override string ToString()
    {
        return lastActionHumanReadable;
    }
}

/// <summary>
/// SampleDataConstructors main class: Creates a TDS from Raw Data
/// </summary>
/// <remarks>
/// TOCHECK: Unhappy that I duplicated all the code from ImportExportYml.cs.
///	What should one do instead?
/// Some sort of standard "make xml to OpenPetra Objects"? Should we make some?
/// </remarks>
class DataBuilder : RawData
{
    /// <summary>
    /// Used for the internal (non PetraServer) way of determining the next
    /// new partner key only.
    /// </summary>
    private static Int64 nextNewPartnerKey = 1;     // used with the internal method only

    /// <summary>
    /// The SiteKey used when creating or altering data
    /// TODO: use it! It is not used yet.
    /// </summary>
    public static Int32 SiteKey {
        get; set;
    }

    /// <summary>
    /// This gets a new Partner Key.
    /// </summary>
    /// <remarks>
    /// Done this way in ImportExportYml.cs
    /// </remarks>
    protected static Int64 getNewPartnerKey()
    {
        //
        // Determines if the partner keys are created by OpenPetra or internally
        //
        // Either we create a partner key internally "somehow" (e.g. by just counting up
        // from a certain number), or we ask the server to do so. Asking the server would
        // be the right way to do it. On the other hand, the server seems to demand from us
        // that we immediately use the partner keys by saving partners on the server.
        // Unfortunately this probibits us from "just building the data in memory on the client",
        // and require more server interaction. So for now, do without.
        // One could (a) ask the server (b) use that number as a starter - or somehow like that.
        //
        bool openPetraCreatesNewPartnerKey = false;
        Int64 newPartnerKey;

        if (openPetraCreatesNewPartnerKey)
        {
            newPartnerKey = Ict.Petra.Server.MPartner.Common.TNewPartnerKey.GetNewPartnerKey(-1);
            // TODO: keeping the sitekey would be helpful anyway
            // (somewhere at the start of the program)
            // Using getNewPartner is probably simplest (says christiank)
            Int32 SiteKey = Int32.Parse(newPartnerKey.ToString().Substring(0, 4));
            Ict.Petra.Server.MPartner.Common.TNewPartnerKey.SubmitNewPartnerKey(
                SiteKey, newPartnerKey, ref newPartnerKey);
        }
        else
        {
            newPartnerKey = nextNewPartnerKey++;
        }

        return newPartnerKey;
    }

    private static Int32 newLocationKey = 1;
    /// <summary>
    /// This gets a new Location Key.
    /// </summary>
    /// <remarks>
    /// The location key is not managed by server, important :
    /// locationkey >0 (that's all!)
    /// </remarks>
    protected static Int32 getNewLocationKey()
    {
        Int32 locationKey = newLocationKey;

        newLocationKey++;
        return locationKey;
    }

    public static PPartnerRow createNewPartner(SampleDataConstructorTDS dataTDS)
    {
        PPartnerRow partner = dataTDS.PPartner.NewRowTyped();

        partner.PartnerKey = getNewPartnerKey();
        partner.StatusCode = "ACTIVE";
        return partner;
    }

    public static PFamilyRow createNewFamily(SampleDataConstructorTDS dataTDS, RPerson person)
    {
        PFamilyRow family = dataTDS.PFamily.NewRowTyped();

        family.FirstName = person.FirstName;
        family.FamilyName = person.FamilyName;
        family.Title = person.Title;
        family.CreatedBy = "DemoData";
        family.DateCreated = DateTime.Now;
        //// family.FieldKey
        //// family.MaritalStatus
        //// family.MaritalStatusComment
        //// family.MaritalStatusSince
        return family;
    }

    // TOCHECK: why sometimes organization (with a "z") and sometimes organisation (with "s")?
    public static POrganisationRow createNewOrganisation(SampleDataConstructorTDS dataTDS, ROrganization organization)
    {
        POrganisationRow organisationRow = dataTDS.POrganisation.NewRowTyped();

        organisationRow.OrganisationName = organization.Name;
        return organisationRow;
    }

    public static PLocationRow createNewLocation(
        SampleDataConstructorTDS dataTDS, RLocation location)
    {
        PLocationRow locationRow = dataTDS.PLocation.NewRowTyped(true);

        locationRow.LocationKey = getNewLocationKey();
        locationRow.SiteKey = 0;

        locationRow.CountryCode = location.CountryCode;
        locationRow.StreetName = location.Addr2;
        locationRow.City = location.City;
        locationRow.PostalCode = location.PostCode;
        return locationRow;
    }

    public static void couple(PPartnerRow partner, PFamilyRow family)
    {
        // partner: data associated with family
        partner.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
        partner.AddresseeTypeCode = MPartnerConstants.PARTNERCLASS_FAMILY;

        partner.PartnerShortName =
            Calculations.DeterminePartnerShortName(family.FamilyName, family.Title, family.FirstName);
        family.PartnerKey = partner.PartnerKey;
    }

    public static void couple(PPartnerRow partnerRow, POrganisationRow organisationRow)
    {
        organisationRow.PartnerKey = partnerRow.PartnerKey;
        partnerRow.PartnerShortName = organisationRow.OrganisationName;
        partnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_ORGANISATION;
    }

    /// <summary>
    /// Just creates a new PartnerLocation row without initializing data
    /// </summary>
    /// <returns>newly created partnerLocationRow</returns>
    public static PPartnerLocationRow createNewPartnerLocation(
        SampleDataConstructorTDS dataTDS
        )
    {
        PPartnerLocationRow partnerLocationRow = dataTDS.PPartnerLocation.NewRowTyped(true);

        return partnerLocationRow;
    }

    /// <summary>
    /// Internal: supposed to be used from coupling partner + location
    /// </summary>
    public static void fillInitialData(
        PPartnerLocationRow partnerLocationRow
        )
    {
        partnerLocationRow.SiteKey = 0;
        partnerLocationRow.SendMail = true;
        partnerLocationRow.DateEffective = DateTime.Now;
        partnerLocationRow.LocationType = "HOME";

        // TODO: EmailAddress - fill in from RawData Person
        // Chat with christiank: was suggested that both email + mobiltelefon
        // should be place in the PartnerLocation, even though this will change
        // in future. Just to have good sample data for now.
        // partnerlocationRow.EmailAddress =

        // partnerLocationRow.TelephoneNumber = location.Phone;

        // TODO: MobileNumber - fill in from RawData mobile phone
        // (see above)
        // partnerlocationRow.MobileNumber =
    }

    /// <summary>
    /// Internal: just supposed to be called by couple(Partner,PartnerLocation)
    /// </summary>
    private static void couple(
        PPartnerRow partnerRow,
        PPartnerLocationRow partnerLocationRow
        )
    {
        partnerLocationRow.PartnerKey = partnerRow.PartnerKey;
    }

    /// <summary>
    /// Internal: just supposed to be called by couple(Partner,PartnerLocation)
    /// </summary>
    /// <param name="locationRow"></param>
    /// <param name="partnerLocationRow"></param>
    private static void couple(
        PLocationRow locationRow,
        PPartnerLocationRow partnerLocationRow
        )
    {
        partnerLocationRow.LocationKey = locationRow.LocationKey;
    }

    /// <summary>
    /// Couples the given partnerRow and locationRow, by creating a partnerLocationRow
    /// and connecting these by filling them with theit corresponding keys and default data.
    /// The partnerLocation is also inserted into the dataTDS.
    ///
    /// </summary>
    /// <param name="dataTDS"></param>
    /// <param name="partnerRow"></param>
    /// <param name="locationRow"></param>
    public static void coupleAndInsertConnectorIntoTDS(
        SampleDataConstructorTDS dataTDS,
        PPartnerRow partnerRow,
        PLocationRow locationRow
        )
    {
        PPartnerLocationRow partnerLocationRow = createNewPartnerLocation(dataTDS);

        dataTDS.PPartnerLocation.Rows.Add(partnerLocationRow);
        fillInitialData(partnerLocationRow);
        couple(partnerRow, partnerLocationRow);
        couple(locationRow, partnerLocationRow);
    }

    public static void InsertSpecialTypes(SampleDataConstructorTDS dataTDS)
    {
        StringCollection specialTypes = new StringCollection();

        specialTypes.Add("VOLUNTEER");
        specialTypes.Add("SUPPORTER");
        specialTypes.Add("GENERATED_SAMPLE_DATA");

        // no need for a temporary list...
        // List <PPartnerTypeRow>partnerTypes = new List <PPartnerTypeRow>();

        foreach (string specialType in specialTypes)
        {
            PPartnerTypeRow partnerType = dataTDS.PPartnerType.NewRowTyped();
            partnerType.TypeCode = specialType.Trim();
            dataTDS.PPartnerType.Rows.Add(partnerType);
        }
    }

    public static void couple(PPartnerRow partner, List <PPartnerTypeRow>specialTypes)
    {
        foreach (PPartnerTypeRow partnerType in specialTypes)
        {
            partnerType.PartnerKey = partner.PartnerKey;
            // TODO: check if special type does not exist yet, and create it
        }
    }

    /// <summary>
    /// Add People from raw data to given TDS (creates Partner and Family Record)
    /// </summary>
    /// <remarks>
    /// Based on ImportExportYml.cs
    /// </remarks>
    public static void insertPeople(
        SampleDataConstructorTDS rawDataTDS,
        RawData rawData,
        out ExecutionReport report
        )
    {
        int numEntries = rawData.People.Count;

        foreach (RPerson rPerson in rawData.People)
        {
            PPartnerRow partner = createNewPartner(rawDataTDS);
            PFamilyRow family = createNewFamily(rawDataTDS, rPerson);
            couple(partner, family);

            rawDataTDS.PPartner.Rows.Add(partner);
            rawDataTDS.PFamily.Rows.Add(family);

            // TODO: add at least special type "GENERATED_SAMPLE_DATA"
            // This would allow identification of sample data that was accidentily
            // mixed with real data (which should never happen).
            // List <PPartnerTypeRow>specialTypes = createSpecialTypes(rawDataTDS, rPerson);
            // couple(partner, specialTypes);


            /*
             * foreach (PPartnerTypeRow specialType in specialTypes) {
             *      rawDataTDS.PPartnerType.Rows.Add(specialType);
             * }
             */

            //// unused:
            // person.DateOfBirth
            // person.Email
        }

        report = new ExecutionReport(
            "Added " + numEntries + " people (partner+family) from raw data"
            );
    }

    /// <summary>
    /// Add Organisations from raw data to given TDS (Adds Partner and Organisation Record)
    /// </summary>
    public static void insertOrganisations(
        SampleDataConstructorTDS rawDataTDS,
        RawData rawData,
        out ExecutionReport report
        )
    {
        int numEntries = rawData.Organizations.Count;

        foreach (ROrganization rOrganisation in rawData.Organizations)
        {
            PPartnerRow partner = createNewPartner(rawDataTDS);
            POrganisationRow organisationRow = createNewOrganisation(rawDataTDS, rOrganisation);
            couple(partner, organisationRow);

            rawDataTDS.PPartner.Rows.Add(partner);
            rawDataTDS.POrganisation.Rows.Add(organisationRow);
        }

        report = new ExecutionReport(
            "Added " + numEntries + " organisation (partner+organisation) from raw data"
            );
    }

    /// <summary>
    /// Add Locations from raw data to given Stack (creates Locations)
    /// </summary>
    /// <remarks>
    /// Based on ImportExportYml.cs
    /// </remarks>
    /// <param name="dataTDS">Used only in order to access some of it's functions (readonly)</param>
    /// <param name="unusedLocations">The created LocationRows are put onto this stack </param>
    /// <param name="rawData">The raw Data from which the locations are extracted</param>
    /// <param name="report">Contains a human readable output of what this function did</param>
    public static void insertLocations(
        SampleDataConstructorTDS dataTDS,
        Stack <PLocationRow>unusedLocations,
        RawData rawData,
        out ExecutionReport report
        )
    {
        int numEntries = rawData.Locations.Count;

        foreach (RLocation location in rawData.Locations)
        {
            unusedLocations.Push(createNewLocation(dataTDS, location));
        }

        report = new ExecutionReport(
            "Created " + numEntries + " Locations (locations) from raw data"
            );
    }

    public static void insertSpecialTypes(SampleDataConstructorTDS dataTDS)
    {
    }

    /// <summary>
    /// Assign Locations to Partners (people and organisations).
    /// This also creates PartnerLocations.
    /// </summary>
    /// <param name="MainTDS"></param>
    /// <param name="unusedLocations"></param>
    /// <param name="fractionOfPeopleToBeGivenLocations"></param>
    /// <param name="report"></param>
    public static void AssignHomesToPartners(
        SampleDataConstructorTDS MainTDS,
        Stack <PLocationRow>unusedLocations,
        double fractionOfPeopleToBeGivenLocations,
        out ExecutionReport report
        )
    {
        // Attach people to homes
        long cntAll = 0;
        long cntAssigned = 0;
        Random rand = new Random(1);                 // Well-Known-Seed: Reproducible results

        foreach (PPartnerRow partnerRow in MainTDS.PPartner.Rows)
        {
            if (rand.NextDouble() < fractionOfPeopleToBeGivenLocations)
            {
                PLocationRow unusedLocationRow = unusedLocations.Pop();
                MainTDS.PLocation.Rows.Add(unusedLocationRow);
                coupleAndInsertConnectorIntoTDS(MainTDS, partnerRow, unusedLocationRow);
                cntAssigned++;
            }

            cntAll++;
        }

        report = new ExecutionReport("Assigned locations to " + cntAssigned + " of all " + cntAll + " Partners");
    }
}
}