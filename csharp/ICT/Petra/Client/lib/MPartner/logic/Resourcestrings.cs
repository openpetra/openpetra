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

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// Contains resourcetexts that are used in the Petra Partner Module.
    /// </summary>
    public class Resourcestrings
    {
        /// <summary>todoComment</summary>
        public const String StrErrorNeedToSavePartner1 = "You have to save the Partner first, before ";

        /// <summary>todoComment</summary>
        public const String StrErrorNeedToSavePartnerTitle = "First Save the Partner";

        /// <summary>todoComment</summary>
        public const String StrErrorFindNewAddress2 = "finding a new Address";

        /// <summary>todoComment</summary>
        public const String StrErrorExportPartner2 = "exporting the Partner";

        /// <summary>todoComment</summary>
        public const String StrErrorPrintPartner2 = "printing the Partner";

        /// <summary>todoComment</summary>
        /// <summary>todoComment</summary>
        public const String StrErrorChangeFamily2 = "changing the Family";

        /// <summary>todoComment</summary>
        public const String StrErrorMaintainContacts2 = "maintaining Contacts";

        /// <summary>todoComment</summary>
        public const String StrErrorDeletePartner2 = "deleting the Partner";

        /// <summary>todoComment</summary>
        public const String StrErrorMaintainField2 = "changing the Field";

        /// <summary>todoComment</summary>
        public const String StrErrorMaintainFamilyMembers2 = "maintaining Family Members";

        /// <summary>todoComment</summary>
        public const String StrErrorMaintainFinanceDetails2 = "maintaining Finance Details";

        /// <summary>todoComment</summary>
        public const String StrErrorMaintainDonorGiftHistory2 = "showing the Donor Gift History";

        /// <summary>todoComment</summary>
        public const String StrErrorMaintainRecipientGiftHistory2 = "showing the Recipient Gift History";

        /// <summary>todoComment</summary>
        public const String StrErrorMaintainInterests2 = "maintaining Interests";

        /// <summary>todoComment</summary>
        public const String StrErrorMaintainIndividualData2 = "maintaining Individual Data";

        /// <summary>todoComment</summary>
        public const String StrErrorMaintainRelationships2 = "maintaining Relationships";

        /// <summary>todoComment</summary>
        public const String StrErrorMaintainReminders2 = "maintaining Reminders";

        /// <summary>todoComment</summary>
        public const String StrErrorCopyAddress2 = "copying an Address";

        /// <summary>todoComment</summary>
        public const String StrErrorNoEmailAddressForThisPartner = "No e-mail address for this Partner" + " in the selected address record.";

        /// <summary>todoComment</summary>
        public const String StrErrorNoValidEmailAddressForThisPartner = "No valid e-mail address for this" +
                                                                        " Partner in the selected address record.";

        /// <summary>todoComment</summary>
        public const String StrErrorNoEmailAddressForThisPartnerTitle = "Cannot Send E-mail To Partner";

        /// <summary>todoComment</summary>
        public const String StrDetails = "Details: ";

        /// <summary>todoComment</summary>
        public const String StrMergedPartnerNotPossible =
            "For this Merged Partner the Merged-Into Partner is not known. It is generally not possible to work with a Partner that was Merged.\r\n"
            +
            "Choose 'Cancel' to cancel the operation that you were in.";

        // Address related resourcestrings
        /// <summary>todoComment</summary>
        public const String StrEmailAddressHelpText = "Enter an email address";

        /// <summary>todoComment</summary>
        public const String StrAddress1Helptext =
            "Enter a first line of an address (this does NOT contain the street, but usually contains a business' name)";

        /// <summary>todoComment</summary>
        public const String StrAddress2Helptext = "Enter a second line of an address (this usually contains the street)";

        /// <summary>todoComment</summary>
        public const String StrAddress3Helptext = "Enter a third line of an address (eg. district of the city)";

        /// <summary>todoComment</summary>
        public const String StrCityHelptext = "Enter a city or town";

        /// <summary>todoComment</summary>
        public const String StrPostCodeHelpText = "Enter an international postal/zip code";

        /// <summary>todoComment</summary>
        public const String StrCountyHelpText = "Enter a county/province/state/district/canton";

        /// <summary>todoComment</summary>
        public const String StrCountryHelpText = "Select a country from the list or type the 2 digit code";

        /// <summary>todoComment</summary>
        public const String StrLocationKeyHelpText = "Enter a Location Key";

        /// <summary>todoComment</summary>
        public const String StrLocationKeyExtraHelpText = " (use button to the left to find a Location Key)";

        /// <summary>todoComment</summary>
        public const String StrLocationKeyButtonFindHelpText = "Finds a Location Key";

        /// <summary>todoComment</summary>
        public const String StrMailingOnlyFindHelpText = "Restricts returned Addresses to Mailing Addresses if ticked";

        /// <summary>todoComment</summary>
        public const String StrPhoneNumberFindHelpText = "Searches for a phone number or an alternate phone number";

        // Partner Find related resourcestrings
        /// <summary>todoComment</summary>
        public const String StrPartnerNameFindHelptext = "Enter [last name, first name, title] or church / organi" +
                                                         "sation / bank / unit / venue name.";

        /// <summary>todoComment</summary>
        public const String StrPersonalNameFindHelpText = "Enter a Personal name when searching for specif" + "ic people";

        /// <summary>todoComment</summary>
        public const String StrPartnerKeyFindHelpText = "Enter a Partner Key as 4+6=10 digit Unit/Partner number";

        /// <summary>todoComment</summary>
        public const String StrOMSSKeyFindHelpText = "Enter an OMSS Key";

        /// <summary>todoComment</summary>
        public const String StrActivePartnersFindHelpText = "Restricts search to Partners with Status ACTIVE";

        /// <summary>todoComment</summary>
        public const String StrAllPartnersFindHelpText = "Search for Partners with any Status";

        /// <summary>todoComment</summary>
        public const String StrPartnersAddedToExtractText = "{0} Partner was added to the new Extract.";

        /// <summary>todoComment</summary>
        public const String StrPartnersAddedToExtractPluralText = "{0} Partners were added to the new Extract.";

        /// <summary>todoComment</summary>
        public const String StrPartnersAddedToExtractTitle = "Generate Extract From Found Partners";

        // Find screens related resourcestrings
        /// <summary>todoComment</summary>
        public const String StrSearching = "Searching...";

        /// <summary>todoComment</summary>
        public const String StrStoppingSearch = "Stopping search...";

        /// <summary>todoComment</summary>
        public const String StrSearchStopped = "Search stopped!";

        /// <summary>todoComment</summary>
        public const String StrSearchResult = "Fin&d Result";

        /// <summary>todoComment</summary>
        public const String StrNoCriteriaSpecified = "No Search Criteria Specified";

        /// <summary>todoComment</summary>
        public const String StrSearchButtonHelpText = "Searches the Petra database with above criteria";

        /// <summary>todoComment</summary>
        public const String StrClearCriteriaButtonHelpText = "Clears the search criteria fields and the search result";

        /// <summary>todoComment</summary>
        public const String StrNewButtonHelpText = "Creates a new Partner (independent of found Partners)";

        /// <summary>todoComment</summary>
        public const String StrEditButtonHelpText = "Opens a Partner Edit screen for the selected Partner";

        /// <summary>todoComment</summary>
        public const String StrViewButtonHelpText = "Gives a short overview of the main information for the selected Partner";

        /// <summary>todoComment</summary>
        public const String StrAcceptButtonHelpText = "Accepts the selected ";

        /// <summary>todoComment</summary>
        public const String StrCancelButtonHelpText = "Closes the window without selecting a ";

        /// <summary>todoComment</summary>
        public const String StrResultGridHelpText = "These are the results of your search. Highlight (or right-click) a line to work with the ";

        /// <summary>todoComment</summary>
        public const String StrDetailedResultsHelpText =
            "If this is ticked, all data is shown; untick to speed up searches (esp. on remote connections).";

        /// <summary>todoComment</summary>
        public const String StrFamilyMembersMenuItemText = "Family &Members...";

        /// <summary>todoComment</summary>
        public const String StrFamilyMenuItemText = "Fa&mily...";

        /// <summary>todoComment</summary>
        public const String StrPersonnelPersonMenuItemText = "&Personnel/Individual Data...";

        /// <summary>todoComment</summary>
        public const String StrPersonnelUnitMenuItemText = "&Personnel/Unit Maintenance...";

        /// <summary>todoComment</summary>
        public const String StrTransferringDataForPageText = "Transferring data...    (for Page ";

        /// <summary>todoComment</summary>
        public const String StrNoPartnerSelectedText = "No Partner selected!";

        /// <summary>todoComment</summary>
        public const String StrFoundText = "found";

        /// <summary>todoComment</summary>
        public const String StrNoRecordsFound1Text = "No";

        /// <summary>todoComment</summary>
        public const String StrNoRecordsFound2Text = " records found." + "\r\n" + "\r\n" +
                                                     "Please check your search criteria or broaden your conditions!";

        /// <summary>todoComment</summary>
        public const String StrSearchButtonText = "&Search";

        /// <summary>todoComment</summary>
        public const String StrSearchButtonStopText = "&Stop";

        /// <summary>todoComment</summary>
        public const String StrSearchMenuItemStopText = "&Stop Search";

        // Partner Info related resourcestrings
        /// <summary>todoComment</summary>
        public const String StrPartnerOrLocationNotExistantText =
            "This Partner (or this Partner+Location combination) doesn't exist anymore in Petra.";

        /// <summary>todoComment</summary>
        public const String StrPartnerOrLocationNotExistantTextReRunSearchText =
            "Please re-run the Partner Find query by pressing 'Search' to get up-to-date Find Results.";

        /// <summary>todoComment</summary>
        public const String StrPartnerOrLocationNotExistantTitle = "Partner Information Cannot be Shown for Partner {0}";

        /// <summary>todoComment</summary>
        public const String StrPartnerFindSearchTargetText = "Partner+Location combination";

        /// <summary>todoComment</summary>
        public const String StrPartnerFindSearchTarget2Text = "Partner";

        /// <summary>todoComment</summary>
        public const String StrPartnerFindSearchTargetPluralText = "Partner+Location combinations";
    }
}