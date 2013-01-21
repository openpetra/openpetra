// auto generated with nant generateORM
// Do not modify this file manually!
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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

namespace Ict.Petra.Shared.MCommon
{
    /// <summary>
    /// Contains functions for handling of Addresses.
    /// </summary>
    public class TSharedAddressHandling
    {
        /// <summary>
        /// Returns the default Location Type for the given PartnerClass
        /// </summary>
        /// <param name="APartnerClass">PartnerClass.</param>
        /// <returns>Default Location Type for the PartnerClass specified with <paramref name="APartnerClass" />.</returns>
        public static String GetDefaultLocationType(TPartnerClass APartnerClass)
        {
            String ReturnValue = "";

            // No copying > assign values of columns manually
            switch (APartnerClass)
            {
                case TPartnerClass.PERSON:
                case TPartnerClass.FAMILY:
                    ReturnValue = "HOME";
                    break;

                case TPartnerClass.CHURCH:
                    ReturnValue = "CHURCH";
                    break;

                case TPartnerClass.ORGANISATION:
                case TPartnerClass.BANK:
                case TPartnerClass.VENUE:
                    ReturnValue = "BUSINESS";
                    break;

                case TPartnerClass.UNIT:
                    ReturnValue = "FIELD";
                    break;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the default Addressee Type for the given PartnerClass.
        /// </summary>
        /// <param name="APartnerClass">PartnerClass.</param>
        /// <returns>Default Addressee Type for the PartnerClass specified with <paramref name="APartnerClass" />.</returns>
        public static String GetDefaultAddresseeType(TPartnerClass APartnerClass)
        {
            switch (APartnerClass)
            {
                case TPartnerClass.PERSON:
                case TPartnerClass.FAMILY:
                    return "FAMILY";

                case TPartnerClass.CHURCH:
                    return "CHURCH";

                case TPartnerClass.ORGANISATION:
                case TPartnerClass.BANK:
                case TPartnerClass.UNIT:
                    return "ORGANISA";

                case TPartnerClass.VENUE:
                    return "VENUE";
            }

            return "";
        }    	
    }
}
