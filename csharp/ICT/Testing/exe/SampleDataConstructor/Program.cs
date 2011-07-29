//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       thomass
//
// Copyright 2004-2010 by OM International
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
using System.Collections.Specialized;

using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace SampleDataConstructor
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
/// Classes starting in "???" are the actual Petra TDS Records.
/// </para>
/// <para>
/// Goal for now:
/// just create many Partners with Addresses and then import.
/// This can perhaps be done with the original Petra Format.
/// 
/// For now, creation of 
/// - raw data
/// - then putting together Petra data (no simulation)
/// </para>
/// </remarks>
class TSampleDataConstructor
{       

	private static Int64 newPartnerKey = -1; 
	/// <summary>
	/// This gets a new Partner Key.
	/// </summary>
	/// <remarks>
	/// Done this way in ImportExportYml.cs
	/// TODO: where should the partnerKey _actually_ come from?
	/// </remarks>
	protected Int64 getNewPartnerKey()
	{
		Int64 partnerKey = newPartnerKey;
        newPartnerKey--;
        return partnerKey;
	}
	

	PPartnerRow createNewPartner(SampleDataConstructorTDS dataTDS)
	{
		PPartnerRow partner = dataTDS.PPartner.NewRowTyped();
		partner.PartnerKey = getNewPartnerKey();
		// partner.StatusCode
		return partner;
	}

	PFamilyRow createNewFamily(SampleDataConstructorTDS dataTDS, RPerson person)
	{
		PFamilyRow family = dataTDS.PFamily.NewRowTyped();
		family.FirstName  = person.FirstName;
		family.FamilyName = person.FamilyName;
		family.Title      = person.Title;
		family.CreatedBy  = "DemoData";
		family.DateCreated = DateTime.Now;
		//// family.FieldKey
		//// family.MaritalStatus
		//// family.MaritalStatusComment
		//// family.MaritalStatusSince
		return family;
	}
	
	void couple(PPartnerRow partner, PFamilyRow family)
	{
		// partner: data associated with family
        partner.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
        partner.AddresseeTypeCode = MPartnerConstants.PARTNERCLASS_FAMILY;

        partner.PartnerShortName =
        	Calculations.DeterminePartnerShortName(family.FamilyName,family.Title,family.FirstName);
 		family.PartnerKey = partner.PartnerKey;
	}
	
	List<PPartnerTypeRow> createSpecialTypes(SampleDataConstructorTDS dataTDS, RPerson person)
	{
		// new StringCollection("VOLUNTEER","SUPPORTER");
		// No Special Types are created for now.
		StringCollection specialTypes = new StringCollection();
		var partnerTypes = new List<PPartnerTypeRow>();
		foreach (string specialType in specialTypes) {
			PPartnerTypeRow partnerType = dataTDS.PPartnerType.NewRowTyped();
	        partnerType.TypeCode = specialType.Trim();
	        partnerTypes.Add(partnerType);
		}
		return partnerTypes;
	}

	void couple(PPartnerRow partner, List<PPartnerTypeRow> specialTypes)
	{
        foreach (PPartnerTypeRow partnerType in specialTypes)
        {
            partnerType.PartnerKey = partner.PartnerKey;                 
            // TODO: check if special type does not exist yet, and create it
        }				
	}

	/// <summary>
	/// Creates a TDS in memory from the given RawData and returns it.
	/// This is supposed to be used to save the created raw data in memory.
	/// </summary>
	/// <remarks>
	/// Based on ImportExportYml.cs 
	/// </remarks>
	public SampleDataConstructorTDS createTDSFromRawData(RawData rawData)
	{
		SampleDataConstructorTDS dataTDS = new SampleDataConstructorTDS();
		
		foreach (RPerson rPerson in rawData.People) {
			var partner = createNewPartner(dataTDS);
			var family = createNewFamily(dataTDS,rPerson);
			couple(partner,family);
		 	
			dataTDS.PPartner.Rows.Add(partner);
			dataTDS.PFamily.Rows.Add(family);

			var specialTypes = createSpecialTypes(dataTDS,rPerson);			
			couple(partner,specialTypes);
	

			dataTDS.PPartner.Rows.Add(partner);
			dataTDS.PFamily.Rows.Add(family);
			foreach (PPartnerTypeRow specialType in specialTypes) {
				dataTDS.PPartnerType.Rows.Add(specialType);
			}
				
				
			
			
			
			//// unused: 
			// person.DateOfBirth
			// person.Email
		}
		return dataTDS;
	}
	
	/// <summary>
	/// data directory containing the raw data files created by benerator
	/// </summary>
	/// <remarks>Please forgive me: dd = dataDirectory</remarks>
	/// TODO (before shipping this testing to trunk): get this path from nant (or sth.) and not set it statically
	const string dd = "../../tmp/Tests-exe.SampleDataConstructor/";

	const string filePeople = "people.csv";
	const string fileOrganisations = "organisations.csv";
	const string fileAddresses = "addresses.csv";


    public static void Main(string[] args)
    {
		// SampleDataConstructor
		Console.WriteLine("Reading Raw Data Files...");
		
		
		//// new TAppSettingsManager(false);
        //// string csvInputFileName = TAppSettingsManager.GetValue("file", true);

		
        try
        {
			RawData rawData = new RawData();
            rawData.readRawDataFromFile(filePeople, RawData.filetypes.people, dd);
            rawData.readRawDataFromFile(fileOrganisations, RawData.filetypes.organizations, dd);
            rawData.readRawDataFromFile(fileAddresses, RawData.filetypes.addresses, dd);
            Console.WriteLine("People:        " + rawData.People.Count);
            Console.WriteLine("Organizations: " + rawData.Organizations.Count);
            Console.WriteLine("Locations:     " + rawData.Locations.Count);
            Console.WriteLine("Mobile Phones: " + rawData.Mobilephones.Count);
            Console.WriteLine("Countries:     " + rawData.Countries.Count);
			
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
    }
}
}