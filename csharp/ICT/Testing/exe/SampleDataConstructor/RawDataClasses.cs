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
using System.Collections.Generic;

namespace SampleDataConstructor
{
/*
 *  abstract class ReadableFromCSV
 *  {
 *              public ReadableFromCSV(DataLine dL)
 *              {
 *              }
 *  }
 */


/* corresponds loosely to table p_partner */
class RPartner
{
}

/* corresponds loosely to table p_family */
class RPerson : RPartner
{
    public string Title;
    public string NobilityTitle;     // e.g. "Duke"
    public string Gender;     // m/w    /// OUCH: char
    public string FirstName;
    public string MiddleName;
    public string FamilyName;
    public string DateOfBirth;     // birthdate
    public string Email;

    public RPerson(DataLine dL)
    {
        dL.mustSet(ref Title, "Title");
        dL.maySet(ref NobilityTitle, "NobilityTitle");
        dL.mustSet(ref Gender, "Gender");
        dL.mustSet(ref FirstName, "FirstName");
        dL.maySet(ref MiddleName, "MiddleName");
        dL.mustSet(ref FamilyName, "FamilyName");
        dL.mustSet(ref DateOfBirth, "DateOfBirth");
        dL.maySet(ref Email, "Email");
    }
}

/* will be represented by p_organisation */
class ROrganization : RPartner
{
    public string Name;

    public ROrganization(DataLine dL)
    {
        dL.mustSet(ref Name, "Name");
    }
}


/// <summary>
/// Contains Contact Information
/// </summary>
/// <remarks>
/// In OpenPetra, points to location:
/// <code>
/// Partner &lt;-- PartnerLocation --&gt; Location
/// </code>
///
/// We will not do that here. We will do, for the start:
///
/// <code>
/// Partner &lt;= Location (Address,Landline) -- things attached to physical location
/// Partner &lt;= MobilePhone -- not attached to location
/// Partner &lt;= Email
/// </code>
///
/// Obviously,
/// - Location and Mobilephone always are linked to Country
/// - Email _can_ be linked to Country
///
/// </remarks>

/* corresponds to tables p_location and p_partner_location */
class RLocation
{
    // these values will stay in Location (p_location)
    public string Addr2;
    public string PostCode;
    public string City;
    public string Province;
    public string CountryCode;

    // later in OpenPetra: these will be in PartnerLocation (p_partner_location)
    public string Phone;
    public string Alternate;
    public string FaxExtension;

    public RLocation(DataLine dL)
    {
        dL.mustSet(ref Addr2, "Addr2");
        dL.mustSet(ref PostCode, "PostCode");
        dL.mustSet(ref City, "City");
        dL.mustSet(ref CountryCode, "CountryCode");
        dL.maySet(ref Province, "Province");
        dL.maySet(ref Phone, "Phone");
        dL.maySet(ref Alternate, "Alternate");
        dL.maySet(ref FaxExtension, "FaxExtension");
    }
}

/* contained in p_partner_location */
class RMobilePhone
{
    public string Mobile;

    public RMobilePhone(DataLine dL)
    {
        dL.mustSet(ref Mobile, "Mobile");
    }
}

/* countries should already be completely present in p_country */
class RCountry
{
    public string CountryCode;
    public string CountryName;
    public string InternationalDialingCode;

    public RCountry(DataLine dL)
    {
        dL.mustSet(ref CountryCode, "CountryCode");
        dL.mustSet(ref CountryName, "CountryName");
        dL.maySet(ref InternationalDialingCode, "InternationalDialingCode");
    }

    public static void AddIfNew(DataLine dL, Dictionary <string, RCountry>Countries)
    {
        string CountryCode = dL.getField("CountryCode");

        if (!Countries.ContainsKey(CountryCode))
        {
            Countries.Add(CountryCode, new RCountry(dL));
        }
    }
}
}