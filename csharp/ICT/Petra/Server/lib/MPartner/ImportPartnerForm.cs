//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon.Data.Cascading;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Jayrock.Json;

namespace Ict.Petra.Server.MPartner.Import
{
    public class TApplicationFormData
    {
        public string firstname;
        public string lastname;
        public string street;
        public string postcode;
        public string city;
    }

    /// <summary>
    /// this class can be used for partners that want to insert or update their own data.
    /// this is time effective and helps the staff in the office.
    /// </summary>
    public class TImportPartnerForm
    {
        public static string DataImportFromForm(string AFormID, string AJSONFormData)
        {
            if (AFormID == "Personnel")
            {
                try
                {
                    TApplicationFormData data = (TApplicationFormData)Jayrock.Json.Conversion.JsonConvert.Import(typeof(TApplicationFormData),
                        AJSONFormData);

                    TLogging.Log(data.firstname);
                    TLogging.Log(data.street);

                    // TODO: check that email is unique. do not allow email to be associated with 2 records. this would cause trouble with authentication

                    // TODO send email, create PDF
                }
                catch (Exception e)
                {
                    TLogging.Log(e.Message);
                }

                return "success";
            }

            return "error";
        }
    }
}