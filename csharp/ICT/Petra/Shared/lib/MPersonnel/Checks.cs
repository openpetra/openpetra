//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;


namespace Ict.Petra.Shared.MPersonnel
{
    /// <summary>
    /// todoComment
    /// </summary>
    public delegate bool TDelegateShowFamilyChangeWarning(String AWarningMessage);

    /// <summary>
    /// Contains functions to be used by the Server and the Client that perform
    /// certain checks - specific for the Personnel Module.
    /// </summary>
    public class PersonnelChecks
    {
        /// <summary>
        /// Shows warning message if the change of a family affects user access to Caleb, i.e. at the
        /// moment a message is displayed if the person has a current commitment record and his/her
        /// family is about to be changed.
        /// Method returns TRUE if the user wants to continue.
        /// </summary>
        /// <param name="APersonKey"></param>Partner Key for person
        /// <param name="APersonShortName"></param>short name for person
        /// <param name="AOldFamilyKey"></param>Partner key for current family
        /// <param name="AOldFamilyShortName"></param>short name for current family
        /// <param name="ANewFamilyKey"></param>Partner key for new family
        /// <param name="ANewFamilyShortName"></param>short name for new family
        /// <param name="AHasCurrentCommitment"></param>Person has current commitment (staff data) record
        /// <param name="ADelegateShowWarning"></param>Delegate Method so message box can be shown on client or appropriate action taken on server
        /// <returns>true if family key change should continue, otherwise false</returns>
        public static bool WarnAboutFamilyChange(Int64 APersonKey, String APersonShortName,
            Int64 AOldFamilyKey, String AOldFamilyShortName, Int64 ANewFamilyKey, String ANewFamilyShortName,
            bool AHasCurrentCommitment, TDelegateShowFamilyChangeWarning ADelegateShowWarning)
        {
            String WarningMessage = "";
            String PersonKey;
            String OldFamilyKey;

            // Return immediately if there was no family key set yet
            if (AOldFamilyKey == 0)
            {
                return true;
            }

            // Return immediately if the family key will not be changed
            if (ANewFamilyKey == AOldFamilyKey)
            {
                return true;
            }

            if (AHasCurrentCommitment)
            {
                return true;
            }

            if ((APersonKey != 0)
                && (AOldFamilyKey != 0))
            {
                PersonKey = String.Format("{0:0000000000}", APersonKey);
                OldFamilyKey = String.Format("{0:0000000000}", AOldFamilyKey);

                WarningMessage = String.Format(Catalog.GetString
                        ("WARNING: This change will affect the information visible to person \"" +
                        "{0}" +
                        "\" (" +
                        "{1}" + ") in The Intranet." + "\r\n" + "\r\n" +
                        "\"" +
                        "{2}" +
                        "\" will no longer be able to see any personal support that previously came" +
                        " in for family \"" +
                        "{3}" +
                        "\" (" + "{4}" +
                        ") but will only be able to see new support that comes in for his/her new family." +
                        "\r\n" + "\r\n" +
                        "Other persons of family \"" +
                        "{5}" +
                        "\" will continue to be able to see in The Intranet all previously received" +
                        " support and any future donations to their family." +
                        "\r\n" + "\r\n" +
                        "Do you want to continue?"),
                    APersonShortName,
                    PersonKey,
                    APersonShortName,
                    AOldFamilyShortName,
                    OldFamilyKey,
                    AOldFamilyShortName);

                if (ADelegateShowWarning == null)
                {
                    throw new ArgumentException("ADelegateShowWarning must not be null");
                }

                return ADelegateShowWarning(WarningMessage);
            }

            return true;
        }
    }
}