//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2012 by OM International
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


namespace Ict.Petra.Client.MCommon
{
    /// <summary>
    /// Contains resourcetexts that are used across several Petra Modules.
    /// </summary>
    public class MCommonResourcestrings
    {
        #region Internal strings used as flags

        /// <summary>todoComment</summary>
        public const String StrCtrlSuppressChangeDetection = "SuppressChangeDetection";

        #endregion

        #region Miscellaneous Generic Strings

        /// <summary>todoComment</summary>
        public static readonly string StrGenericInfo = Catalog.GetString("Information");

        /// <summary>todoComment</summary>
        public static readonly string StrGenericWarning = Catalog.GetString("Warning");

        /// <summary>todoComment</summary>
        public static readonly string StrGenericError = Catalog.GetString("Error");

        /// <summary>todoComment</summary>
        public static readonly string StrGenericReady = Catalog.GetString("Ready");

        /// <summary>todoComment</summary>
        public static readonly string StrDetails = Catalog.GetString("Details: ");

        /// <summary>todoComment</summary>
        public static readonly string StrGenericInactiveCode = Catalog.GetString(" (inactive)");

        /// <summary>todoComment</summary>
        public static readonly string StrGenericFunctionalityNotAvailable = Catalog.GetString("Functionality not available");

        /// <summary>todoComment</summary>
        public static readonly string StrPetraServerTooBusy = Catalog.GetString("The OpenPetra Server is currently too busy to {0}.\r\n\r\n" +
            "Please wait a few seconds and press 'Retry' then to retry, or 'Cancel' to abort.");

        /// <summary>todoComment</summary>
        public static readonly string StrPetraServerTooBusyTitle = Catalog.GetString("OpenPetra Server Too Busy");

        /// <summary>todoComment</summary>
        public static readonly string StrOpeningCancelledByUser = Catalog.GetString("Opening of {0} screen got cancelled by user.");

        /// <summary>todoComment</summary>
        public static readonly string StrOpeningCancelledByUserTitle = Catalog.GetString("Screen opening cancelled");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorNoInstalledSites = Catalog.GetString("No Installed Sites!");

        /// <summary>todoComment</summary>
        public static readonly string StrBtnTextNew = Catalog.GetString("&New");

        /// <summary>todoComment</summary>
        public static readonly string StrBtnTextEdit = Catalog.GetString("Edi&t");

        /// <summary>todoComment</summary>
        public static readonly string StrBtnTextDelete = Catalog.GetString("&Delete");

        /// <summary>todoComment</summary>
        public static readonly string StrBtnTextDone = Catalog.GetString("D&one");

        /// <summary>todoComment</summary>
        public static readonly string StrBtnTextFilter = Catalog.GetString("Filte&r");

        /// <summary>
        /// This string is for the Find Next button on the Filter/Find panel in Ict.Common
        /// So the definition is not accessible from there.  See TucoFilterAndFind
        /// </summary>
        public static readonly string StrBtnTextFindNext = Catalog.GetString("Find Ne&xt");

        /// <summary>todoComment</summary>
        public static readonly string StrValueUnassignable = Catalog.GetString("Unassignable Value");

        #endregion

        #region Strings associated with saving data

        /// <summary>todoComment</summary>
        public static readonly string StrFormHasUnsavedChanges = Catalog.GetString("This window has changes that have not been saved.");

        /// <summary>todoComment</summary>
        public static readonly string StrFormHasUnsavedChangesQuestion = Catalog.GetString("Save changes before closing?");

        /// <summary>Shown while data is being saved.</summary>
        public static readonly string StrSavingDataInProgress = Catalog.GetString("Saving data...");

        /// <summary>Shown when data was saved successfully.</summary>
        public static readonly string StrSavingDataSuccessful = Catalog.GetString("Data successfully saved.");

        /// <summary>Shown when saving of data failed.</summary>
        public static readonly string StrSavingDataException = Catalog.GetString("Data could not be saved because an unexpected error occured!");

        /// <summary>todoComment</summary>
        public static readonly string StrSavingDataErrorOccured = Catalog.GetString("Data could not be saved because an error occured!");

        /// <summary>todoComment</summary>
        public static readonly string StrSavingDataCancelled = Catalog.GetString("Saving of data cancelled by user!");

        /// <summary>Shown when no data needs saving.</summary>
        public static readonly string StrSavingDataNothingToSave = Catalog.GetString("There was nothing to be saved.");

        #endregion

        #region Strings associated with deleting data

        /// <summary>todoComment</summary>
        public static readonly string StrCountTerminatedEarly1 = Catalog.GetString(
            "{0}{0}The reference count was terminated after {1} records had been found.  ");

        /// <summary>todoComment</summary>
        public static readonly string StrCountTerminatedEarly2 = Catalog.GetString(
            "There may be other tables that reference the highlighted record that were not scanned.  Choose 'Yes' to re-run the count to completion.  ");

        /// <summary>todoComment</summary>
        public static readonly string StrCountTerminatedEarly3 = Catalog.GetString(
            "In some cases this may take several minutes.  While the count is taking place the server will be fully loaded.");

        /// <summary>todoComment</summary>
        public static readonly string StrCountTerminatedEarly4 = Catalog.GetString(
            "{0}{0}Choose 'No' to close this message and return directly to the '{1}' screen.");

        /// <summary>todoComment</summary>
        public static readonly string StrCountTerminatedEarlyOK = Catalog.GetString("{0}{0}Click 'OK' to close this message.");

        #endregion

        #region Partner Strings
        /// <summary>todoComment</summary>
        public static readonly string StrErrorOnlyForPerson = Catalog.GetString("This only works for Partners of Partner Class PERSON");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorOnlyForFamilyOrPerson = Catalog.GetString(
            "This only works for Partners of Partner Classes FAMILY or PERSON");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorOnlyForPersonOrUnit = Catalog.GetString(
            "This only works for Partners of Partner Classes PERSON or UNIT");

        /// <summary>todoComment</summary>
        public static readonly string StrPartnerStatusChange = Catalog.GetString("The Partner's Status is currently" + " '{0}'.\r\n" +
            "OpenPetra will change it automatically to '{1}'");

        /// <summary>todoComment</summary>
        public static readonly string StrPartnerReActivationTitle = Catalog.GetString("Partner Gets Re-activated!");

        #endregion

        #region Singular/Plural string pairs (no Catalog.GetString())

        /// <summary>todoComment</summary>
        public static readonly string StrSingularRecordCount = "{0} record";

        /// <summary>todoComment</summary>
        public static readonly string StrPluralRecordCount = "{0} records";

        #endregion

        #region Auto-generated Record Creation

        /// <summary>todoComment</summary>
        public static readonly string StrNewRecordIsFiltered = Catalog.GetString(
            "A new record has been added but the current Filter is preventing it from being displayed.  The Filter will be reset so that you can continue to edit the new record.");

        /// <summary>todoComment</summary>
        public static readonly string StrAddNewRecordTitle = Catalog.GetString("Add New Record");

        /// <summary>todoComment</summary>
        public static readonly string StrPleaseEnterDescription = Catalog.GetString("PLEASE ENTER DESCRIPTION");

        #endregion

        #region Auto-generated Record Deletion

        #region Simple Strings
        /// <summary>todoComment</summary>
        public static readonly String StrRecordCannotBeDeleted = Catalog.GetString("Record cannot be deleted!");

        /// <summary>todoComment</summary>
        public static readonly String StrRecordDeletionTitle = Catalog.GetString("Record Deletion");

        /// <summary>todoComment</summary>
        public static readonly String StrConfirmDeleteTitle = Catalog.GetString("Confirm Delete");

        /// <summary>todoComment</summary>
        public static readonly String StrDefaultDeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");

        /// <summary>todoComment</summary>
        public static readonly String StrErrorWhileDeleting = Catalog.GetString("An error occurred while deleting this record.{0}{0}{1}");

        /// <summary>todoComment</summary>
        public static readonly String StrDeletionCompletedTitle = Catalog.GetString("Deletion Completed");

        /// <summary>todoComment</summary>
        public static readonly String StrMultiRowDeletionQuestion = Catalog.GetString("Would you like to delete the {0} highlighted rows?{1}{1}");

        /// <summary>todoComment</summary>
        public static readonly String StrMultiRowDeletionCheck = Catalog.GetString("Each record will be checked to confirm that it can be deleted.");

        /// <summary>todoComment</summary>
        public static readonly String StrNoRecordsWereDeleted = Catalog.GetString("No records were deleted.");

        /// <summary>todoComment</summary>
        public static readonly String StrClickToReviewDeletionOrCancel = Catalog.GetString(
            "{0}{0}Click OK to review the details, or Cancel to return direct to the data screen");

        /// <summary>todoComment</summary>
        public static readonly String StrDeleteActionSummaryTitle = Catalog.GetString("Delete Action Summary");

        /// <summary>todoComment</summary>
        public static readonly String StrRowsReferencedByOtherTables = Catalog.GetString("Rows in this table that are referenced by other tables");

        /// <summary>todoComment</summary>
        public static readonly String StrExceptions = Catalog.GetString("Exceptions");

        /// <summary>todoComment</summary>
        public static readonly String StrItemXofYRecordColon = Catalog.GetString("{0}: {1} of {2}{3}Record: {4}{3}{5}");

        /// <summary>todoComment</summary>
        public static readonly String StrViewNextDetailOrCancel = Catalog.GetString("{0}{0}Click OK to review the next detail or Cancel to finish.");

        /// <summary>todoComment</summary>
        public static readonly String StrMoreDetailsAboutRowsNotDeleted = Catalog.GetString("More Details About Rows Not Deleted");

        #endregion

        #region GetPlural strings (without Catalog.GetString())

        /// <summary>todoComment</summary>
        public static readonly String StrReasonColon = "Reason:";

        /// <summary>todoComment</summary>
        public static readonly String StrReasonsColon = "Reasons:";

        /// <summary>todoComment</summary>
        public static readonly String StrRecordSuccessfullyDeleted = "{0} record was successfully deleted.";

        /// <summary>todoComment</summary>
        public static readonly String StrRecordsSuccessfullyDeleted = "{0} records were successfully deleted.";

        /// <summary>todoComment</summary>
        public static readonly String StrRowNotDeletedBecauseNonDeletable =
            "{0}{1} record could not be deleted because it is marked as non-deletable.";

        /// <summary>todoComment</summary>
        public static readonly String StrRowsNotDeletedBecauseNonDeletable =
            "{0}{1} records could not be deleted because they are marked as non-deletable.";

        /// <summary>todoComment</summary>
        public static readonly String StrRowNotDeletedBecauseDeleteNotAllowed = "{0}{1} record was not be deleted because deletion was not allowed.";

        /// <summary>todoComment</summary>
        public static readonly String StrRowsNotDeletedBecauseDeleteNotAllowed =
            "{0}{1} records were not be deleted because deletion was not allowed.";

        /// <summary>todoComment</summary>
        public static readonly String StrRowNotDeletedBecauseReferencedElsewhere =
            "{0}{1} record could not be deleted because it is referenced by at least one other table.";

        /// <summary>todoComment</summary>
        public static readonly String StrRowsNotDeletedBecauseReferencedElsewhere =
            "{0}{1} records could not be deleted because they are referenced by at least one other table.";

        /// <summary>todoComment</summary>
        public static readonly String StrRowNotDeletedDueToUnexpectedException =
            "{0}{1} record could not be deleted because the delete action failed unexpectedly.";

        /// <summary>todoComment</summary>
        public static readonly String StrRowsNotDeletedDueToUnexpectedException =
            "{0}{1} records could not be deleted because the delete action failed unexpectedly.";

        #endregion

        #endregion

        #region Auto-generated Validation

        /// <summary>todoComment</summary>
        public static readonly string StrDuplicateRecordNotAllowed = Catalog.GetString(
            "You have attempted to create a duplicate record.  Please ensure that you have unique input data for the field(s) {0}.");

        /// <summary>todoComment</summary>
        public static readonly string StrEditedRecordIsFiltered = Catalog.GetString(
            "The record has been edited but the current Filter is preventing it from being displayed.  The Filter will be reset so that you can continue to edit the record.");

        /// <summary>todoComment</summary>
        public static readonly string StrEditRecordTitle = Catalog.GetString("Edit Record");

        #endregion

        #region Auto-generated Filter/Find

        /// <summary>todoComment</summary>
        public static readonly string StrClickToShowHideFilterPanel = Catalog.GetString(
            "Click to show or hide the filter panel");

        /// <summary>todoComment</summary>
        public static readonly string StrClickToHideFilterPanel = Catalog.GetString(
            "Click to hide the filter panel");

        /// <summary>todoComment</summary>
        public static readonly string StrClickToFindNextRecord = Catalog.GetString(
            "Click to find the next record that matches the search criteria");

        /// <summary>todoComment</summary>
        public static readonly string StrClickToClearFilterAttribute = Catalog.GetString(
            "Click to clear this filter attribute");

        /// <summary>todoComment</summary>
        public static readonly string StrClickToClearFindAttribute = Catalog.GetString(
            "Click to clear this find attribute");

        /// <summary>todoComment</summary>
        public static readonly string StrUpDownFindDirection = Catalog.GetString(
            "Choose up or down to set the search direction");

        /// <summary>todoComment</summary>
        public static readonly string StrErrorInFilterCriterion = Catalog.GetString(
            "There is an error in the input text of one of the filter panel controls");

        /// <summary>todoComment</summary>
        public static readonly string StrFilterTitle = Catalog.GetString(
            "Filtering Records");

        #endregion
    }
}