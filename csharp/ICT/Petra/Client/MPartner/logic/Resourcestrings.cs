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
using Ict.Common;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// Contains resourcetexts that are used in the Petra Partner Module.
    /// </summary>
    public class MPartnerResourcestrings
    {
        #region General resourcestrings

        /// <summary>todoComment</summary>
        public static readonly string StrErrorNeedToSavePartner1 = Catalog.GetString("You have to save the Partner first, before ");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorNeedToSavePartnerTitle = Catalog.GetString("First Save the Partner");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorFindNewAddress2 = Catalog.GetString("finding a new Address");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorExportPartner2 = Catalog.GetString("exporting the Partner");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorPrintPartner2 = Catalog.GetString("printing the Partner");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorChangeFamily2 = Catalog.GetString("changing the Family");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorMaintainContacts2 = Catalog.GetString("maintaining Contacts");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorDeletePartner2 = Catalog.GetString("deleting the Partner");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorMaintainField2 = Catalog.GetString("changing the Field");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorMaintainFamilyMembers2 = Catalog.GetString("maintaining Family Members");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorMaintainFinanceDetails2 = Catalog.GetString("maintaining Finance Details");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorMaintainDonorGiftHistory2 = Catalog.GetString("showing the Donor Gift History");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorMaintainRecipientGiftHistory2 = Catalog.GetString("showing the Recipient Gift History");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorMaintainInterests2 = Catalog.GetString("maintaining Interests");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorMaintainIndividualData2 = Catalog.GetString("maintaining Individual Data");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorMaintainRelationships2 = Catalog.GetString("maintaining Relationships");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorMaintainReminders2 = Catalog.GetString("maintaining Reminders");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorCopyAddress2 = Catalog.GetString("copying an Address");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorNoEmailAddressForThisPartner = Catalog.GetString(
            "No e-mail address for this Partner in the selected address record.");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorNoValidEmailAddressForThisPartner = Catalog.GetString(
            "No valid e-mail address for this Partner in the selected address record.");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorNoEmailAddressForThisPartnerTitle = Catalog.GetString("Cannot Send E-mail To Partner");

        /// <summary>todoComment</summary>
        public static readonly string StrMergedPartnerNotPossible =
            Catalog.GetString(
                "For this Merged Partner the Merged-Into Partner is not known. It is generally not possible to work with a Partner that was Merged.\r\n"
                +
                "Choose 'Cancel' to cancel the operation that you were in.");

        #endregion

        #region Address related resourcestrings

        /// <summary>todoComment</summary>
        public static readonly string StrEmailAddressHelpText = Catalog.GetString("Enter an email address");

        /// <summary>todoComment</summary>
        public static readonly string StrAddress1Helptext =
            Catalog.GetString("Enter a first line of an address (this does NOT contain the street, but usually contains a business' name)");

        /// <summary>todoComment</summary>
        public static readonly string StrAddress2Helptext = Catalog.GetString("Enter a second line of an address (this usually contains the street)");

        /// <summary>todoComment</summary>
        public static readonly string StrAddress3Helptext = Catalog.GetString("Enter a third line of an address (eg. district of the city)");

        /// <summary>todoComment</summary>
        public static readonly string StrCityHelptext = Catalog.GetString("Enter a city or town");

        /// <summary>todoComment</summary>
        public static readonly string StrPostCodeHelpText = Catalog.GetString("Enter an international postal/zip code");

        /// <summary>todoComment</summary>
        public static readonly string StrCountyHelpText = Catalog.GetString("Enter a county/province/state/district/canton");

        /// <summary>todoComment</summary>
        public static readonly string StrCountryHelpText = Catalog.GetString("Select a country from the list or type the 2 digit code");

        /// <summary>todoComment</summary>
        public static readonly string StrLocationKeyHelpText = Catalog.GetString("Enter a Location Key");

        /// <summary>todoComment</summary>
        public static readonly string StrLocationKeyExtraHelpText = Catalog.GetString(" (use button to the left to find a Location Key)");

        /// <summary>todoComment</summary>
        public static readonly string StrLocationKeyButtonFindHelpText = Catalog.GetString("Finds a Location Key");

        /// <summary>todoComment</summary>
        public static readonly string StrMailingOnlyFindHelpText = Catalog.GetString("Restricts returned Addresses to Mailing Addresses if ticked");

        /// <summary>todoComment</summary>
        public static readonly string StrPhoneNumberFindHelpText = Catalog.GetString("Searches for a phone number or an alternate phone number");

        #endregion

        #region Find screens related resourcestrings

        /// <summary>todoComment</summary>
        public static readonly string StrSearching = Catalog.GetString("Searching...");

        /// <summary>todoComment</summary>
        public static readonly string StrStoppingSearch = Catalog.GetString("Stopping search...");

        /// <summary>todoComment</summary>
        public static readonly string StrSearchStopped = Catalog.GetString("Search stopped!");

        /// <summary>todoComment</summary>
        public static readonly string StrSearchResult = Catalog.GetString("Fin&d Result");

        /// <summary>todoComment</summary>
        public static readonly string StrNoCriteriaSpecified = Catalog.GetString("No Search Criteria Specified");

        /// <summary>todoComment</summary>
        public static readonly string StrSearchButtonHelpText = Catalog.GetString("Searches the OpenPetra database with above criteria");

        /// <summary>todoComment</summary>
        public static readonly string StrClearCriteriaButtonHelpText = Catalog.GetString("Clears the search criteria fields and the search result");

        /// <summary>todoComment</summary>
        public static readonly string StrAcceptButtonHelpText = Catalog.GetString("Accepts the selected ");

        /// <summary>todoComment</summary>
        public static readonly string StrCancelButtonHelpText = Catalog.GetString("Closes the window without selecting a ");

        /// <summary>todoComment</summary>
        public static readonly string StrResultGridHelpText = Catalog.GetString(
            "These are the results of your search. Highlight (or right-click) a line to work with the ");

        /// <summary>todoComment</summary>
        public static readonly string StrDetailedResultsHelpText =
            Catalog.GetString("If this is ticked, all data is shown; untick to speed up searches (esp. on remote connections).");

        /// <summary>todoComment</summary>
        public static readonly string StrTransferringDataForPageText = Catalog.GetString("Transferring data...    (for Page ");

        /// <summary>todoComment</summary>
        public static readonly string StrFoundText = Catalog.GetString("found");

        /// <summary>todoComment</summary>
        public static readonly string StrNoRecordsFound1Text = Catalog.GetString("No");

        /// <summary>todoComment</summary>
        public static readonly string StrNoRecordsFound2Text = Catalog.GetString(
            " records found.\r\n\r\nPlease check your search criteria or broaden your conditions!");

        /// <summary>todoComment</summary>
        public static readonly string StrSearchButtonText = Catalog.GetString("&Search");

        /// <summary>todoComment</summary>
        public static readonly string StrSearchButtonStopText = Catalog.GetString("&Stop");

        /// <summary>todoComment</summary>
        public static readonly string StrSearchMenuItemStopText = Catalog.GetString("&Stop Search");

        #endregion

        #region Partner Find resourcestrings (shared across several Classes)

        /// <summary>todoComment</summary>
        public static readonly string StrPartnerFindSearchTargetText = Catalog.GetString("Partner+Location combination");

        /// <summary>todoComment</summary>
        public static readonly string StrPartnerFindByBankDetailsSearchTargetText = Catalog.GetString("Partner+Bank Account combination");

        /// <summary>todoComment</summary>
        public static readonly string StrPartnerFindSearchTarget2Text = Catalog.GetString("Partner");

        /// <summary>todoComment</summary>
        public static readonly string StrPartnerFindSearchTargetPluralText = Catalog.GetString("Partner+Location combinations");

        /// <summary>todoComment</summary>
        public static readonly string StrPartnerFindByBankDetailsSearchTargetPluralText = Catalog.GetString("Partner+Bank Account combinations");

        #endregion

        #region Resourcestrings which are shared between Partner Edit screen and Partner Find screen

        /// <summary>todoComment</summary>
        public static readonly string StrFamilyMembersMenuItemText = Catalog.GetString("Family &Members...");

        /// <summary>todoComment</summary>
        public static readonly string StrFamilyMenuItemText = Catalog.GetString("Fa&mily...");

        /// <summary>todoComment</summary>
        public static readonly string StrPersonnelPersonMenuItemText = Catalog.GetString("&Personnel/Individual Data...");

        /// <summary>todoComment</summary>
        public static readonly string StrPersonnelUnitMenuItemText = Catalog.GetString("&Personnel/Unit Maintenance...");

        #endregion
    }
}