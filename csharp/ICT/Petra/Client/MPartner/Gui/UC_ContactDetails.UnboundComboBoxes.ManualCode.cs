//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2015 by OM International
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
using System.Data;
using System.IO;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_ContactDetails
    {
        private enum TOverallContactComboType
        {
            /// <summary>Primary Phone</summary>
            occtPrimaryPhone,

            /// <summary>Phone Within Organisation (for PERSONs only!)</summary>
            occtPhoneWithinOrganisation,

            /// <summary>Primary Email</summary>
            occtPrimaryEmail,

            /// <summary>Secondary Email (for FAMILYs only!)</summary>
            occtSecondaryEmail,

            /// <summary>Email Within Organisation (for PERSONs only!)</summary>
            occtEmailWithinOrganisation
        }

        /// <summary>
        /// Manual way for getting the data that is contained in the 'Overall Contact Settings' GroupBox' Control.
        /// </summary>
        /// <returns>True if successful.</returns>
        public bool GetOverallContactSettingsDataFromControls()
        {
            bool ReturnValue;

            ReturnValue = UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType.occtPrimaryEmail, true);

            if (ReturnValue)
            {
                ReturnValue = UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType.occtPrimaryPhone, true);
            }

            if (ReturnValue)
            {
                ReturnValue = UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType.occtPhoneWithinOrganisation, true);
            }

            if (ReturnValue)
            {
                ReturnValue = UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType.occtEmailWithinOrganisation, true);
            }

            if (ReturnValue)
            {
                ReturnValue = ValidateOvrlContSettgsCombos(TOverallContactComboType.occtSecondaryEmail, cmbSecondaryEMail,
                    cmbSecondaryEMail.GetSelectedString(), GetDataViewForContactCombo(TOverallContactComboType.occtSecondaryEmail, false));
            }

            return ReturnValue;
        }

        private void UpdatePhoneComboItemsOfAllPhoneCombos(string ANewPhoneNumberValue = null)
        {
            UpdatePhoneComboItems(cmbPrimaryPhoneForContacting, ANewPhoneNumberValue);
            UpdatePhoneComboItems(cmbPhoneWithinTheOrganisation, ANewPhoneNumberValue);
        }

        /// <summary>
        /// Adds available Phone Numbers to a ComboBox. If <paramref name="ANewPhoneNumberValue"/> is specified
        /// then this value gets selected in the ComboBox.
        /// </summary>
        /// <remarks>Method is similar to Method <see cref="UpdateEmailComboItems"/>.</remarks>
        /// <param name="AComboBox">The ComboBox Control whose Items should get updated.</param>
        /// <param name="ANewPhoneNumberValue">Pass a new Phone Number to get it selected in the ComboBox. (Default=null)</param>
        private void UpdatePhoneComboItems(TCmbAutoComplete AComboBox, string ANewPhoneNumberValue = null)
        {
            object[] EligiblePhoneNumbers;
            string ThePrimaryPhoneNumber = String.Empty;
            string TheWithinOrganisationPhoneNumber = String.Empty;
            DataView EligiblePhoneNrsDV;
            string CurrentlySelectedPhoneNr = AComboBox.GetSelectedString(-1);

            // Determine all Partner Attributes that have a Partner Attribute Type that constitutes a Phone Number
            // and that are Current (Fax Numbers are excluded from this!).
            EligiblePhoneNrsDV = Calculations.DeterminePartnerPhoneNumbers(FMainDS.PPartnerAttribute, true, false);

            EligiblePhoneNumbers = new object[EligiblePhoneNrsDV.Count + 1];
            EligiblePhoneNumbers[0] = String.Empty;

            for (int Counter = 0; Counter < EligiblePhoneNrsDV.Count; Counter++)
            {
                var ThePhoneRow = ((PPartnerAttributeRow)EligiblePhoneNrsDV[Counter].Row);

                EligiblePhoneNumbers[Counter + 1] = ThePhoneRow.Value;

                if (ThePhoneRow.Primary)
                {
                    ThePrimaryPhoneNumber = ThePhoneRow.Value;
                }

                if (ThePhoneRow.WithinOrganisation)
                {
                    TheWithinOrganisationPhoneNumber = ThePhoneRow.Value;
                }
            }

            if (!AreComboBoxItemsIdenticalToArgument(AComboBox, EligiblePhoneNumbers))
            {
                // Add the available Phone Numbers to the ComboBox
                AComboBox.Items.Clear();
                AComboBox.Items.AddRange(EligiblePhoneNumbers);
            }

            if ((AComboBox == cmbPrimaryPhoneForContacting)
                && (ThePrimaryPhoneNumber != String.Empty))
            {
                if (AComboBox.GetSelectedString() != ThePrimaryPhoneNumber)
                {
                    // Select the Primary Phone Number in the ComboBox
                    FPhoneSelectedValueChangedEvent = true;

                    AComboBox.SetSelectedString(ThePrimaryPhoneNumber);

                    FPhoneSelectedValueChangedEvent = false;
                }
            }
            else if ((AComboBox == cmbPhoneWithinTheOrganisation)
                     && (TheWithinOrganisationPhoneNumber != String.Empty))
            {
                if (AComboBox.GetSelectedString() != TheWithinOrganisationPhoneNumber)
                {
                    // Select the Within Organisation Phone Number in the ComboBox
                    FPhoneSelectedValueChangedEvent = true;

                    AComboBox.SetSelectedString(TheWithinOrganisationPhoneNumber);

                    FPhoneSelectedValueChangedEvent = false;
                }
            }
            else
            {
                CurrentlySelectedPhoneNr = ANewPhoneNumberValue ?? CurrentlySelectedPhoneNr;

                AComboBox.SetSelectedString(CurrentlySelectedPhoneNr);
            }
        }

        /// <summary>
        /// Updates the Items of a ComboBox from the 'Overall Contact Settings Group'.
        /// </summary>
        /// <param name="AComboBox">The ComboBox from the 'Overall Contact Settings Group' whose Items should be updated.</param>
        /// <param name="ASuppressMessages">If set to false: Messages will be shown to the user in certain circumstances.
        /// However, this will only be done in case <paramref name="AComboBox"/> is cmbPrimaryEMail, in all other cases
        /// this Argument gets ignored!</param>
        private void UpdateItemsOfOvrlContSettgsCombo(TCmbAutoComplete AComboBox, bool ASuppressMessages = true)
        {
            if ((AComboBox == cmbPrimaryEMail)
                || (AComboBox == cmbEMailWithinTheOrganisation))
            {
                UpdateEmailComboItems(AComboBox, ASuppressMessages);
            }
            else if ((AComboBox == cmbPrimaryPhoneForContacting)
                     || (AComboBox == cmbPhoneWithinTheOrganisation))
            {
                UpdatePhoneComboItems(AComboBox);
            }
        }

        private DataView GetDataViewForContactCombo(TOverallContactComboType AContactComboType,
            bool AOnlyCurrentValues)
        {
            DataView ReturnValue;

            if ((AContactComboType == TOverallContactComboType.occtPrimaryPhone)
                || (AContactComboType == TOverallContactComboType.occtPhoneWithinOrganisation))
            {
                ReturnValue = Calculations.DeterminePartnerPhoneNumbers(FMainDS.PPartnerAttribute, AOnlyCurrentValues, false);
            }
            else
            {
                ReturnValue = Calculations.DeterminePartnerEmailAddresses(FMainDS.PPartnerAttribute, AOnlyCurrentValues);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Updates the DataRows that pertain to a ComboBox from the 'Overall Contact Settings Group' to reflect the
        /// current ComboBox setting.
        /// </summary>
        /// <param name="AContactComboType">The Contact ComboBox Type that designates which Records' data should get
        /// updated.</param>
        /// <param name="ARunValidation">If set to true, various validations are run against the ComboBox setting.</param>
        /// <returns>True if <paramref name="ARunValidation"/> is false, or when <paramref name="ARunValidation"/>
        /// is true and validation succeeds, otherwise false.</returns>
        private bool UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType AContactComboType, bool ARunValidation)
        {
            bool ReturnValue = true;
            TCmbAutoComplete ComboBoxForContactComboType;
            DataColumn UpdateDataColumn;
            PPartnerAttributeRow ThePartnerAttributeRow;
            string ComboBoxSelectedString;
            DataView EligibleValuesDV = GetDataViewForContactCombo(AContactComboType, false);

            GetDataAccordingToContactComboType(AContactComboType, out ComboBoxForContactComboType, out UpdateDataColumn);

            ComboBoxSelectedString = ComboBoxForContactComboType.GetSelectedString();

            if (ComboBoxSelectedString != String.Empty)
            {
                for (int Counter = 0; Counter < EligibleValuesDV.Count; Counter++)
                {
                    ThePartnerAttributeRow = ((PPartnerAttributeRow)EligibleValuesDV[Counter].Row);

                    // Modify Rows only as necessary
                    if ((bool)ThePartnerAttributeRow[UpdateDataColumn.Ordinal])
                    {
                        if (ThePartnerAttributeRow.Value != ComboBoxSelectedString)
                        {
                            ThePartnerAttributeRow[UpdateDataColumn.Ordinal] = (object)false;
                        }
                    }
                    else
                    {
                        if (ThePartnerAttributeRow.Value == ComboBoxSelectedString)
                        {
                            ThePartnerAttributeRow[UpdateDataColumn.Ordinal] = (object)true;
                        }
                    }
                }
            }
            else
            {
                for (int Counter2 = 0; Counter2 < EligibleValuesDV.Count; Counter2++)
                {
                    ThePartnerAttributeRow = ((PPartnerAttributeRow)EligibleValuesDV[Counter2].Row);

                    // Modify Rows only as necessary
                    if ((bool)ThePartnerAttributeRow[UpdateDataColumn.Ordinal])
                    {
                        ThePartnerAttributeRow[UpdateDataColumn.Ordinal] = (object)false;
                    }
                }
            }

            if (ARunValidation)
            {
                ReturnValue = ValidateOvrlContSettgsCombos(AContactComboType, ComboBoxForContactComboType, ComboBoxSelectedString, EligibleValuesDV);
            }

            return ReturnValue;
        }

        private bool ValidateOvrlContSettgsCombos(TOverallContactComboType AContactComboType, TCmbAutoComplete AComboBoxForContactComboType,
            string AComboBoxSelectedString, DataView AEligibleValuesDV)
        {
            bool ReturnValue = true;
            DataView CurrentValuesDV;
            PPartnerAttributeRow ThePartnerAttributeRow;
            bool ChoiceFoundAmongEligibleValues = false;

            CurrentValuesDV = GetDataViewForContactCombo(AContactComboType, true);

            if (CurrentValuesDV.Count != 0)
            {
                if ((AContactComboType == TOverallContactComboType.occtPrimaryEmail)
                    && (AComboBoxSelectedString == String.Empty))
                {
                    // Generate a Validation *Warning*, not an error. The user can ignore this if (s)he chooses to do so!
                    ValidationPrimaryEmailAddrNotSet();
                }
                else if ((AContactComboType == TOverallContactComboType.occtSecondaryEmail)
                         && (AComboBoxSelectedString != String.Empty)
                         && (AComboBoxSelectedString == cmbPrimaryEMail.GetSelectedString()))
                {
                    // Generate a Validation *Warning*, not an error. The user can ignore this if (s)he chooses to do so!
                    ValidationSecondaryEmailAddrEqualsPrimaryEmailAddr();
                }
                else
                {
                    if (AComboBoxSelectedString != String.Empty)
                    {
                        for (int Counter3 = 0; Counter3 < AEligibleValuesDV.Count; Counter3++)
                        {
                            ThePartnerAttributeRow = ((PPartnerAttributeRow)AEligibleValuesDV[Counter3].Row);

                            if (ThePartnerAttributeRow.Value == AComboBoxSelectedString)
                            {
                                ChoiceFoundAmongEligibleValues = true;

                                if (!ThePartnerAttributeRow.Current)
                                {
                                    // This condition should not occur, unless the program code that runs when the 'Valid'
                                    // CheckBox is disabled and which should clear all the Items from the ComboBox is somehow
                                    // not working correctly, or not run at all. This condition is therefore a 'back-stop'
                                    // that will prevent invalid data going to the DB!

                                    // Generate a Validation *Error*. The user cannot ignore this.
                                    if ((AContactComboType == TOverallContactComboType.occtPrimaryPhone)
                                        || (AContactComboType == TOverallContactComboType.occtPhoneWithinOrganisation))
                                    {
                                        ValidationPhoneNrSetButItIsntCurrent(AContactComboType);
                                    }
                                    else
                                    {
                                        ValidationEmailAddrSetButItIsntCurrent(AContactComboType);
                                    }

                                    UpdateItemsOfOvrlContSettgsCombo(AComboBoxForContactComboType, false);

                                    ReturnValue = false;
                                }
                            }
                        }

                        if (!ChoiceFoundAmongEligibleValues)
                        {
                            // This condition should not occur, unless various bits of program code are somehow
                            // not working correctly, or not run at all. This condition is therefore a 'back-stop'
                            // that will prevent invalid data going to the DB!

                            // Generate a Validation *Error*. The user cannot ignore this.
                            if ((AContactComboType == TOverallContactComboType.occtPrimaryPhone)
                                || (AContactComboType == TOverallContactComboType.occtPhoneWithinOrganisation))
                            {
                                ValidationPhoneNrSetButNotAmongPhoneNrs(AContactComboType);
                            }
                            else
                            {
                                ValidationEmailAddrSetButNotAmongEmailAddrs(AContactComboType);
                            }

                            UpdateItemsOfOvrlContSettgsCombo(AComboBoxForContactComboType, true);

                            ReturnValue = false;
                        }
                    }
                }
            }
            else
            {
                if (AComboBoxSelectedString != String.Empty)
                {
                    // This condition should not occur, unless various bits of program code are somehow
                    // not working correctly, or not run at all. This condition is therefore a 'back-stop'
                    // that will prevent invalid data going to the DB!

                    // Generate a Validation *Error*. The user cannot ignore this.
                    if ((AContactComboType == TOverallContactComboType.occtPrimaryPhone)
                        || (AContactComboType == TOverallContactComboType.occtPhoneWithinOrganisation))
                    {
                        ValidationPhoneNrSetButNoPhoneNrAvailable(AContactComboType);
                    }
                    else
                    {
                        ValidationEmailAddrSetButNoEmailAddrAvailable(AContactComboType);
                    }

                    UpdateItemsOfOvrlContSettgsCombo(AComboBoxForContactComboType, true);

                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }

        private void UpdateEmailComboItemsOfAllEmailCombos(bool ASuppressMessages = false, string ANewEmailAddressValue = null)
        {
            UpdateEmailComboItems(cmbPrimaryEMail, ASuppressMessages, ANewEmailAddressValue);

            UpdateEmailComboItems(cmbEMailWithinTheOrganisation, false, ANewEmailAddressValue);  // ASuppressMessages has no meaning for this Combo

            UpdateEmailComboItems(cmbSecondaryEMail, false, ANewEmailAddressValue);  // ASuppressMessages has no meaning for this Combo
        }

        /// <summary>
        /// Adds available E-Mail Addresses to a ComboBox. If <paramref name="ANewEmailAddressValue"/> is specified
        /// then this value gets selected in the ComboBox.
        /// </summary>
        /// <remarks>Method is similar to Method <see cref="UpdatePhoneComboItems"/>.</remarks>
        /// <param name="AComboBox">The ComboBox Control whose Items should get updated.</param>
        /// <param name="ASuppressMessages">If set to false: Messages will be shown to the user in certain circumstances.
        /// However, this will only be done in case <paramref name="AComboBox"/> is cmbPrimaryEMail!</param>
        /// <param name="ANewEmailAddressValue">Pass a new E-Mail Address to get it selected in the ComboBox. (Default=null)</param>
        /// <param name="APreviousValue">The Previous Value. Only needed for cmbSecondaryEMail!</param>
        private void UpdateEmailComboItems(TCmbAutoComplete AComboBox, bool ASuppressMessages,
            string ANewEmailAddressValue = null, string APreviousValue = null)
        {
            object[] EligibleEmailAddresses;
            string ThePrimaryEmailAddress = String.Empty;
            string TheSecondaryEmailAddress = String.Empty;
            string TheWithinOrganisationEmailAddress = String.Empty;
            string PrimaryContactMethod;  // Ignored, but needed as this is an out Argument of GetSystemCategoryOvrlContSettgsValues...
            DataView EligibleEmailAddrsDV;
            DataView AllEmailAddrsDV;
            string CurrentlySelectedEmailAddr = AComboBox.GetSelectedString(-1);

            // Determine all Partner Attributes that have a Partner Attribute Type that constitutes an E-Mail
            // and that are Current.
            EligibleEmailAddrsDV = GetDataViewForContactCombo(TOverallContactComboType.occtPrimaryEmail, true);

            EligibleEmailAddresses = new object[EligibleEmailAddrsDV.Count + 1];
            EligibleEmailAddresses[0] = String.Empty;

            if (AComboBox == cmbSecondaryEMail)
            {
                GetSystemCategoryOvrlContSettgsValues(out PrimaryContactMethod, out TheSecondaryEmailAddress);
            }

            for (int Counter = 0; Counter < EligibleEmailAddrsDV.Count; Counter++)
            {
                var TheEmailRow = ((PPartnerAttributeRow)EligibleEmailAddrsDV[Counter].Row);

                EligibleEmailAddresses[Counter + 1] = TheEmailRow.Value;

                if (AComboBox != cmbSecondaryEMail)
                {
                    if (TheEmailRow.Primary)
                    {
                        ThePrimaryEmailAddress = TheEmailRow.Value;
                    }

                    if (TheEmailRow.WithinOrganisation)
                    {
                        TheWithinOrganisationEmailAddress = TheEmailRow.Value;
                    }
                }
            }

            if (!AreComboBoxItemsIdenticalToArgument(AComboBox, EligibleEmailAddresses))
            {
                // Add the available E-Mail Addresses to the ComboBox
                AComboBox.Items.Clear();
                AComboBox.Items.AddRange(EligibleEmailAddresses);
            }

            if ((AComboBox == cmbPrimaryEMail)
                && (ThePrimaryEmailAddress != String.Empty))
            {
                if (AComboBox.GetSelectedString() != ThePrimaryEmailAddress)
                {
                    // Select the Primary E-mail Address in the ComboBox
                    FEmailSelectedValueChangedEvent = true;

                    AComboBox.SetSelectedString(ThePrimaryEmailAddress);

                    FEmailSelectedValueChangedEvent = false;
                }
            }
            else if ((AComboBox == cmbEMailWithinTheOrganisation)
                     && (TheWithinOrganisationEmailAddress != String.Empty))
            {
                if (AComboBox.GetSelectedString() != TheWithinOrganisationEmailAddress)
                {
                    // Select the Within Organisation E-mail Address in the ComboBox
                    FEmailSelectedValueChangedEvent = true;

                    AComboBox.SetSelectedString(TheWithinOrganisationEmailAddress);

                    FEmailSelectedValueChangedEvent = false;
                }
            }
            else if ((AComboBox == cmbSecondaryEMail)
                     && (ANewEmailAddressValue != null)
                     && (ANewEmailAddressValue != String.Empty))
            {
                if (APreviousValue == TheSecondaryEmailAddress)
                {
                    if ((APreviousValue != ANewEmailAddressValue)
                        && (CurrentlySelectedEmailAddr == APreviousValue)
                        && (AComboBox.GetSelectedString() != ANewEmailAddressValue))
                    {
                        // Select the Secondary E-mail Address in the ComboBox
                        FEmailSelectedValueChangedEvent = true;

                        AComboBox.SetSelectedString(ANewEmailAddressValue);

                        FEmailSelectedValueChangedEvent = false;
                    }
                }
                else
                {
                    // Select the Secondary E-mail Address in the ComboBox
                    FEmailSelectedValueChangedEvent = true;

                    AComboBox.SetSelectedString(TheSecondaryEmailAddress);

                    FEmailSelectedValueChangedEvent = false;
                }
            }
            else
            {
                CurrentlySelectedEmailAddr = ANewEmailAddressValue ?? CurrentlySelectedEmailAddr;

                AComboBox.SetSelectedString(CurrentlySelectedEmailAddr);

                if ((AComboBox == cmbPrimaryEMail)
                    && (!ASuppressMessages))
                {
                    if (EligibleEmailAddrsDV.Count > 0)
                    {
                        FTimerDrivenMessageBoxKind = TTimerDrivenMessageBoxKind.tdmbkNoPrimaryEmailAsNoCurrentAvailable;
                        ShowMessageBoxTimer.Start();
                    }
                    else
                    {
                        AllEmailAddrsDV = GetDataViewForContactCombo(TOverallContactComboType.occtPrimaryEmail, false);

                        if (AllEmailAddrsDV.Count > 0)
                        {
                            FTimerDrivenMessageBoxKind = TTimerDrivenMessageBoxKind.tdmbkNoPrimaryEmailButNonCurrentAvailable;
                            ShowMessageBoxTimer.Start();
                        }
                    }
                }
            }
        }

        private bool AreComboBoxItemsIdenticalToArgument(ComboBox AComboBox, object[] ASoughtForComboBoxItems)
        {
            bool ReturnValue = true;

            if (AComboBox.Items.Count == ASoughtForComboBoxItems.Length)
            {
                for (int Counter = 0; Counter < AComboBox.Items.Count; Counter++)
                {
                    if (AComboBox.Items[Counter].ToString() != ASoughtForComboBoxItems[Counter].ToString())
                    {
                        ReturnValue = false;

                        break;
                    }
                }
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        private void SelectSystemCategoryOvrlContSettgsCombosItems()
        {
            string PrimaryContactMethod;
            string SecondaryEmailAddress;
            int ComboBoxItemThatMatches;

            GetSystemCategoryOvrlContSettgsValues(out PrimaryContactMethod, out SecondaryEmailAddress);

            if (PrimaryContactMethod != String.Empty)
            {
                ComboBoxItemThatMatches = cmbPrimaryWayOfContacting.Items.IndexOf(PrimaryContactMethod);

                if (ComboBoxItemThatMatches != -1)
                {
                    cmbPrimaryWayOfContacting.SelectedIndex = ComboBoxItemThatMatches;
                }
            }
            else
            {
                // Select 'empty' Item - this is needed as otherwise SelectedIndex is -1 (set up like this through
                // TFrmPetraEditUtils.ClearControl) and when the user would 'tab' over this Control then the
                // 'Save' Button would get enabled...)
                cmbPrimaryWayOfContacting.SelectedIndex = 0;
            }

            if (SecondaryEmailAddress != String.Empty)
            {
                ComboBoxItemThatMatches = cmbSecondaryEMail.Items.IndexOf(SecondaryEmailAddress);

                if (ComboBoxItemThatMatches != -1)
                {
                    cmbSecondaryEMail.SelectedIndex = ComboBoxItemThatMatches;
                }
            }
        }

        private void GetSystemCategoryOvrlContSettgsValues(out string APrimaryContactMethod, out string ASecondaryEmailAddress)
        {
            DataView EligibleSystemCategoryAttributesDV = Calculations.DeterminePartnerSystemCategoryAttributes(
                FMainDS.PPartnerAttribute, TSharedDataCache.TMPartner.GetSystemCategorySettingsConcatStr());
            PPartnerAttributeRow PartnerAttributeDR;

            APrimaryContactMethod = String.Empty;
            ASecondaryEmailAddress = String.Empty;

            for (int Counter = 0; Counter < EligibleSystemCategoryAttributesDV.Count; Counter++)
            {
                PartnerAttributeDR = (PPartnerAttributeRow)EligibleSystemCategoryAttributesDV[Counter].Row;

                if (PartnerAttributeDR.AttributeType == Calculations.ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD)
                {
                    APrimaryContactMethod = PartnerAttributeDR.Value;
                }

                if (PartnerAttributeDR.AttributeType == Calculations.ATTR_TYPE_PARTNERS_SECONDARY_EMAIL_ADDRESS)
                {
                    ASecondaryEmailAddress = PartnerAttributeDR.Value;
                }
            }
        }

        private void UpdateSystemCategoryOvrlContSettgsCombosRecords()
        {
            string CurrPrimaryWayOfContactingStr = cmbPrimaryWayOfContacting.GetSelectedString();
            string CurrSecondaryEmailAddressStr = cmbSecondaryEMail.GetSelectedString();
            string ExistingPrimContactMethStr = String.Empty;
            string ExistingSecondaryEmailAddressStr = String.Empty;
            PPartnerAttributeRow NewPrimaryWayOfContactingAttributeRow;
            PPartnerAttributeRow NewSecondaryEmailAddressRow;

            DataRow[] ExistingPrimContactMethArr = FMainDS.PPartnerAttribute.Select(
                PPartnerAttributeTable.GetAttributeTypeDBName() + " = '" +
                Calculations.ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD + "'");
            DataRow[] ExistingSecondaryEmailAddressArr = FMainDS.PPartnerAttribute.Select(
                PPartnerAttributeTable.GetAttributeTypeDBName() + " = '" +
                Calculations.ATTR_TYPE_PARTNERS_SECONDARY_EMAIL_ADDRESS + "'");

            // Check if a p_partner_attribute record exists which holds the information about this Partners' 'Primary Way of Contacting'
            if (ExistingPrimContactMethArr.Length != 0)
            {
                // There must always be only one such record, so we can simply pick the first record in the Array
                ExistingPrimContactMethStr = ((PPartnerAttributeRow)ExistingPrimContactMethArr[0]).Value;
            }

            if (ExistingPrimContactMethStr != String.Empty)
            {
                if (ExistingPrimContactMethStr != CurrPrimaryWayOfContactingStr)
                {
                    if (CurrPrimaryWayOfContactingStr != String.Empty)
                    {
                        // Update the existing record with the new 'Primary Way of Contacting' selection
                        ((PPartnerAttributeRow)ExistingPrimContactMethArr[0]).Value = CurrPrimaryWayOfContactingStr;
                    }
                    else
                    {
                        // If the user chose to clear the 'Primary Way of Contacting': delete the record
                        ExistingPrimContactMethArr[0].Delete();
                    }
                }
            }
            else
            {
                if (CurrPrimaryWayOfContactingStr != String.Empty)
                {
                    // We need to add a record that holds the 'Primary Way of Contacting' selection as there isn't one yet for this Partner
                    NewPrimaryWayOfContactingAttributeRow = FMainDS.PPartnerAttribute.NewRowTyped(true);
                    NewPrimaryWayOfContactingAttributeRow.PartnerKey = FMainDS.PPartner[0].PartnerKey;
                    NewPrimaryWayOfContactingAttributeRow.AttributeType = Calculations.ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD;
                    NewPrimaryWayOfContactingAttributeRow.Sequence = -1;
                    NewPrimaryWayOfContactingAttributeRow.Index = 9999;
                    NewPrimaryWayOfContactingAttributeRow.Value = CurrPrimaryWayOfContactingStr;
                    NewPrimaryWayOfContactingAttributeRow.Current = true;

                    FMainDS.PPartnerAttribute.Rows.Add(NewPrimaryWayOfContactingAttributeRow);
                }
            }

            // Check if a p_partner_attribute record exists which holds the information about this Partners' 'Secondary E-mail Address'
            if (ExistingSecondaryEmailAddressArr.Length != 0)
            {
                // There must always be only one such record, so we can simply pick the first record in the Array
                ExistingSecondaryEmailAddressStr = ((PPartnerAttributeRow)ExistingSecondaryEmailAddressArr[0]).Value;
            }

            if (ExistingSecondaryEmailAddressStr != String.Empty)
            {
                if (ExistingSecondaryEmailAddressStr != CurrSecondaryEmailAddressStr)
                {
                    if (CurrSecondaryEmailAddressStr != String.Empty)
                    {
                        // Update the existing record with the new 'Secondary E-mail Address' selection
                        ((PPartnerAttributeRow)ExistingSecondaryEmailAddressArr[0]).Value = CurrSecondaryEmailAddressStr;
                    }
                    else
                    {
                        // If the user chose to clear the 'Secondary E-mail Address': delete the record
                        ExistingSecondaryEmailAddressArr[0].Delete();
                    }
                }
            }
            else
            {
                if (CurrSecondaryEmailAddressStr != String.Empty)
                {
                    // We need to add a record that holds the 'Secondary E-mail Address' selection as there isn't one yet for this Partner
                    NewSecondaryEmailAddressRow = FMainDS.PPartnerAttribute.NewRowTyped(true);
                    NewSecondaryEmailAddressRow.PartnerKey = FMainDS.PPartner[0].PartnerKey;
                    NewSecondaryEmailAddressRow.AttributeType = Calculations.ATTR_TYPE_PARTNERS_SECONDARY_EMAIL_ADDRESS;
                    NewSecondaryEmailAddressRow.Sequence = -1;
                    NewSecondaryEmailAddressRow.Index = 9999;
                    NewSecondaryEmailAddressRow.Value = CurrSecondaryEmailAddressStr;
                    NewSecondaryEmailAddressRow.Current = true;

                    FMainDS.PPartnerAttribute.Rows.Add(NewSecondaryEmailAddressRow);
                }
            }
        }

        /// <summary>
        /// Sets a Record as being Primary (only works for E-Mail and Phone Contact Types).
        /// </summary>
        private void SetRecordSpecialFlag(bool ASetAsPrimary)
        {
            var SelectedDetailDR = GetSelectedDetailRow();

            if ((txtValue.Text != String.Empty)
                && (!(ASetAsPrimary ? SelectedDetailDR.Primary : SelectedDetailDR.WithinOrganisation)))
            {
                // Ensure that the current Value is available in the respective ComboBox
                HandleValueLeave(null, null);

                if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS)
                {
                    if (TStringChecks.ValidateEmail(txtValue.Text) == null)
                    {
                        if (ASetAsPrimary)
                        {
                            // Select current Value in the ComboBox
                            cmbPrimaryEMail.SetSelectedString(txtValue.Text);

                            // Reflect the new Primary record in the underlying data
                            UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType.occtPrimaryEmail, false);
                        }
                        else
                        {
                            // Select current Value in the ComboBox
                            cmbEMailWithinTheOrganisation.SetSelectedString(txtValue.Text);

                            // Reflect the new Primary record in the underlying data
                            UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType.occtEmailWithinOrganisation, false);
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            Catalog.GetString("Can't make this records' E-Mail Address the Primary E-mail as it isn't " +
                                "a valid E-mail Address!"),
                            Catalog.GetString("Invalid E-Mail Address can't be made Primary E-mail!"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL)
                {
                    if (ASetAsPrimary)
                    {
                        if (RowHasPhoneAttributeType(SelectedDetailDR))
                        {
                            // Select current Value in the ComboBox
                            cmbPrimaryPhoneForContacting.SetSelectedString(txtValue.Text);

                            // Reflect the new Primary record in the underlying data
                            UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType.occtPrimaryPhone, false);
                        }
                    }
                    else
                    {
                        // Select current Value in the ComboBox
                        cmbPhoneWithinTheOrganisation.SetSelectedString(txtValue.Text);

                        // Reflect the new Primary record in the underlying data
                        UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType.occtPhoneWithinOrganisation, false);
                    }
                }
                else
                {
                    throw new EOPAppException(
                        "Method 'SetRecordAsPrimary' must only be called for records that have E-Mail or Phone Contact Types");
                }

                // Remove the <F12>/<SHIFT>+<F12> function key hint from the StatusBar Text
                UpdateValueManual();
            }
        }

        #region Checks

        private void CheckThatNonCurrentPhoneNrIsntSpecificPhoneNr(TOverallContactComboType AContactComboType,
            bool APhoneNumberInQuestionIsThisRecord)
        {
            TCmbAutoComplete ComboBoxForContactComboType;
            string PhoneComboTypeStr;
            bool NoPhoneNumbersAvailableAnymore = false;
            string PhoneConsequenceStr = StrPhoneUnavailableConsequenceSelectNewOne;
            string PhoneMessageBoxTitleStr;


            GetDataAccordingToContactComboType(AContactComboType, out ComboBoxForContactComboType, out PhoneComboTypeStr);

            PhoneMessageBoxTitleStr = String.Format(StrPhoneUnavailableConsequenceSelectNewOneTitle, PhoneComboTypeStr);

            if (APhoneNumberInQuestionIsThisRecord)
            {
                DataView EligiblePhoneNrsDV = GetDataViewForContactCombo(TOverallContactComboType.occtPrimaryPhone, true);

                if (EligiblePhoneNrsDV.Count == 0)
                {
                    NoPhoneNumbersAvailableAnymore = true;

                    PhoneConsequenceStr = String.Format(StrPhoneUnavailableConsequenceCleared, PhoneComboTypeStr);
                    PhoneMessageBoxTitleStr = String.Format(StrPhoneUnavailableConsequenceClearedTitle, PhoneComboTypeStr);
                }

                // Select the 'empty' ComboBox Item
                ComboBoxForContactComboType.SelectedIndex = 0;

                MessageBox.Show(
                    String.Format(
                        Catalog.GetString(
                            "You have made the Phone Number no longer current, but up till now it was set to be the {0}.\r\n\r\n{1}"),
                        PhoneComboTypeStr,
                        PhoneConsequenceStr),
                    PhoneMessageBoxTitleStr,
                    MessageBoxButtons.OK, NoPhoneNumbersAvailableAnymore ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                if (!NoPhoneNumbersAvailableAnymore)
                {
                    // Show the other current Phone Number(s) to the user
                    ComboBoxForContactComboType.DroppedDown = true;
                }
            }
        }

        private void CheckThatNonCurrentEmailAddressIsntSpecificEmailAddr(TOverallContactComboType AContactComboType,
            bool AEmailAddressInQuestionIsThisRecord)
        {
            TCmbAutoComplete ComboBoxForContactComboType;
            string EmailComboTypeStr;
            bool NoEmailAddressesAvailableAnymore = false;
            string EmailConsequenceStr = StrEmailUnavailableConsequenceSelectNewOne;
            string EmailMessageBoxTitleStr;


            GetDataAccordingToContactComboType(AContactComboType, out ComboBoxForContactComboType, out EmailComboTypeStr);

            EmailMessageBoxTitleStr = String.Format(StrEmailUnavailableConsequenceSelectNewOneTitle, EmailComboTypeStr);

            if (AEmailAddressInQuestionIsThisRecord)
            {
                DataView EligibleEmailAddrsDV = GetDataViewForContactCombo(TOverallContactComboType.occtPrimaryEmail, true);

                if (EligibleEmailAddrsDV.Count == 0)
                {
                    NoEmailAddressesAvailableAnymore = true;

                    EmailConsequenceStr = String.Format(StrEmailUnavailableConsequenceCleared, EmailComboTypeStr);
                    EmailMessageBoxTitleStr = String.Format(StrEmailUnavailableConsequenceClearedTitle, EmailComboTypeStr);
                }

                // Select the 'empty' ComboBox Item
                ComboBoxForContactComboType.SelectedIndex = 0;

                MessageBox.Show(
                    String.Format(
                        Catalog.GetString(
                            "You have made the E-mail Address no longer current, but up till now it was set to be the {0}.\r\n\r\n{1}"),
                        EmailComboTypeStr,
                        EmailConsequenceStr),
                    EmailMessageBoxTitleStr,
                    MessageBoxButtons.OK, NoEmailAddressesAvailableAnymore ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                if (!NoEmailAddressesAvailableAnymore)
                {
                    // Show the other current e-mail-Address(es) to the user
                    ComboBoxForContactComboType.DroppedDown = true;
                }
            }
        }

        private void GetDataAccordingToContactComboType(TOverallContactComboType AContactComboType,
            out TCmbAutoComplete AComboBoxForContactComboType, out string APhoneComboTypeStr)
        {
            switch (AContactComboType)
            {
                case TOverallContactComboType.occtPrimaryPhone:
                    AComboBoxForContactComboType = cmbPrimaryPhoneForContacting;
                    APhoneComboTypeStr = StrPrimaryPhone;

                    break;

                case TOverallContactComboType.occtPhoneWithinOrganisation:
                    AComboBoxForContactComboType = cmbPhoneWithinTheOrganisation;
                    APhoneComboTypeStr = StrPhoneWithinOrgansiation;

                    break;

                case TOverallContactComboType.occtPrimaryEmail:
                    AComboBoxForContactComboType = cmbPrimaryEMail;
                    APhoneComboTypeStr = StrPrimaryEmail;

                    break;

                case TOverallContactComboType.occtSecondaryEmail:
                    AComboBoxForContactComboType = cmbSecondaryEMail;
                    APhoneComboTypeStr = StrSecondaryEmail;

                    break;

                case TOverallContactComboType.occtEmailWithinOrganisation:
                    AComboBoxForContactComboType = cmbEMailWithinTheOrganisation;
                    APhoneComboTypeStr = StrEmailWithinOrgansiation;

                    break;

                default:
                    AComboBoxForContactComboType = null;
                    APhoneComboTypeStr = String.Empty;

                    break;
            }
        }

        private void GetDataAccordingToContactComboType(TOverallContactComboType AContactComboType,
            out TCmbAutoComplete AComboBoxForContactComboType, out DataColumn ADataColumn)
        {
            string PhoneComboTypeStr;  // Ignored, but needed as this is an out Argument of GetDataAccordingToContactComboType...

            GetDataAccordingToContactComboType(AContactComboType, out AComboBoxForContactComboType, out PhoneComboTypeStr);

            switch (AContactComboType)
            {
                case TOverallContactComboType.occtPrimaryPhone:
                    ADataColumn = new PPartnerAttributeTable().Columns[PPartnerAttributeTable.ColumnPrimaryId];

                    break;

                case TOverallContactComboType.occtPhoneWithinOrganisation:
                    ADataColumn = new PPartnerAttributeTable().Columns[PPartnerAttributeTable.ColumnWithinOrganisationId];

                    break;

                case TOverallContactComboType.occtPrimaryEmail:
                    ADataColumn = new PPartnerAttributeTable().Columns[PPartnerAttributeTable.ColumnPrimaryId];

                    break;

                case TOverallContactComboType.occtEmailWithinOrganisation:
                    ADataColumn = new PPartnerAttributeTable().Columns[PPartnerAttributeTable.ColumnWithinOrganisationId];

                    break;

                default:
                    ADataColumn = null;

                    break;
            }
        }

        #endregion

        #region Data Validation

        /// <summary>
        /// Creates a Data Validation *Error* for Phone ComboBoxes.
        /// </summary>
        private void ValidationPhoneNrSetButNoPhoneNrAvailable(TOverallContactComboType AContactComboType)
        {
            const string ResCont = "ContactDetails_Phone_Set_But_No_Phone_Nr_Available";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            string ErrorCode;
            TCmbAutoComplete ComboBoxForContactComboType;
            string ComboTypeStr;   // Ignored, but needed as this is an out Argument of GetDataAccordingToContactComboType...

            if (AContactComboType == TOverallContactComboType.occtPrimaryPhone)
            {
                ErrorCode = PetraErrorCodes.ERR_PRIMARY_PHONE_NR_SET_DESIPITE_NO_PHONE_NR_AVAIL;
            }
            else if (AContactComboType == TOverallContactComboType.occtPhoneWithinOrganisation)
            {
                ErrorCode = PetraErrorCodes.ERR_OFFICE_PHONE_NR_SET_DESIPITE_NO_PHONE_NR_AVAIL;
            }
            else
            {
                throw new EOPAppException("AContactComboType Argument must designate a Phone ComboBox");
            }

            GetDataAccordingToContactComboType(AContactComboType,
                out ComboBoxForContactComboType, out ComboTypeStr);

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(ErrorCode),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, ComboBoxForContactComboType, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);

            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for Phone ComboBoxes.
        /// </summary>
        private void ValidationPhoneNrSetButItIsntCurrent(TOverallContactComboType AContactComboType)
        {
            const string ResCont = "ContactDetails_Phone_Set_But_It_Isnt_Current";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            string ErrorCode;
            TCmbAutoComplete ComboBoxForContactComboType;
            string ComboTypeStr;   // Ignored, but needed as this is an out Argument of GetDataAccordingToContactComboType...

            if (AContactComboType == TOverallContactComboType.occtPrimaryPhone)
            {
                ErrorCode = PetraErrorCodes.ERR_PRIMARY_PHONE_NR_SET_BUT_IT_ISNT_CURRENT;
            }
            else if (AContactComboType == TOverallContactComboType.occtPhoneWithinOrganisation)
            {
                ErrorCode = PetraErrorCodes.ERR_OFFICE_PHONE_NR_SET_BUT_IT_ISNT_CURRENT;
            }
            else
            {
                throw new EOPAppException("AContactComboType Argument must designate a Phone ComboBox");
            }

            GetDataAccordingToContactComboType(AContactComboType,
                out ComboBoxForContactComboType, out ComboTypeStr);

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(ErrorCode),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, ComboBoxForContactComboType, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);

            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for Phone ComboBoxes.
        /// </summary>
        private void ValidationPhoneNrSetButNotAmongPhoneNrs(TOverallContactComboType AContactComboType)
        {
            const string ResCont = "ContactDetails_Phone_Set_But_Not_Among_Phone_Nrs";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            string ErrorCode;
            TCmbAutoComplete ComboBoxForContactComboType;
            string ComboTypeStr;   // Ignored, but needed as this is an out Argument of GetDataAccordingToContactComboType...

            if (AContactComboType == TOverallContactComboType.occtPrimaryPhone)
            {
                ErrorCode = PetraErrorCodes.ERR_PRIMARY_PHONE_NR_SET_BUT_NOT_AMONG_PHONE_NRS;
            }
            else if (AContactComboType == TOverallContactComboType.occtPhoneWithinOrganisation)
            {
                ErrorCode = PetraErrorCodes.ERR_OFFICE_PHONE_NR_SET_BUT_NOT_AMONG_PHONE_NRS;
            }
            else
            {
                throw new EOPAppException("AContactComboType Argument must designate a Phone ComboBox");
            }

            GetDataAccordingToContactComboType(AContactComboType,
                out ComboBoxForContactComboType, out ComboTypeStr);

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(ErrorCode),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, ComboBoxForContactComboType, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);

            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Warning* for specifically for cmbPrimaryEmail.
        /// </summary>
        private void ValidationPrimaryEmailAddrNotSet()
        {
            const string ResCont = "ContactDetails_PrimaryEmailAddress_Not_Set";
            TScreenVerificationResult VerificationResult;

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_NOT_SET_DESIPITE_EMAIL_ADDR_AVAIL),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbPrimaryEMail, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Warning* for specifically for cmbSecondaryEmail.
        /// </summary>
        private void ValidationSecondaryEmailAddrEqualsPrimaryEmailAddr()
        {
            const string ResCont = "ContactDetails_SecondaryEmailAddr_Equals_PrimaryEmailAddr";
            TScreenVerificationResult VerificationResult;

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_SECONDARY_EMAIL_ADDR_EQUALS_PRIMARY_EMAIL_ADDR,
                        new string[] { cmbSecondaryEMail.GetSelectedString() }),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbSecondaryEMail, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for E-Mail ComboBoxes.
        /// </summary>
        private void ValidationEmailAddrSetButNoEmailAddrAvailable(TOverallContactComboType AContactComboType)
        {
            const string ResCont = "ContactDetails_EmailAddress_Set_But_No_Email_Addr_Available";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            string ErrorCode;
            TCmbAutoComplete ComboBoxForContactComboType;
            string ComboTypeStr;   // Ignored, but needed as this is an out Argument of GetDataAccordingToContactComboType...

            if (AContactComboType == TOverallContactComboType.occtPrimaryEmail)
            {
                ErrorCode = PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_SET_DESIPITE_NO_EMAIL_ADDR_AVAIL;
            }
            else if (AContactComboType == TOverallContactComboType.occtSecondaryEmail)
            {
                ErrorCode = PetraErrorCodes.ERR_SECONDARY_EMAIL_ADDR_SET_DESIPITE_NO_EMAIL_ADDR_AVAIL;
            }
            else if (AContactComboType == TOverallContactComboType.occtEmailWithinOrganisation)
            {
                ErrorCode = PetraErrorCodes.ERR_OFFICE_EMAIL_ADDR_SET_DESIPITE_NO_EMAIL_ADDR_AVAIL;
            }
            else
            {
                throw new EOPAppException("AContactComboType Argument must designate an E-Mail ComboBox");
            }

            GetDataAccordingToContactComboType(AContactComboType,
                out ComboBoxForContactComboType, out ComboTypeStr);

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(ErrorCode),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, ComboBoxForContactComboType, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);

            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for E-Mail ComboBoxes.
        /// </summary>
        private void ValidationEmailAddrSetButItIsntCurrent(TOverallContactComboType AContactComboType)
        {
            const string ResCont = "ContactDetails_EmailAddress_Set_But_It_Isnt_Current";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            string ErrorCode;
            TCmbAutoComplete ComboBoxForContactComboType;
            string ComboTypeStr;   // Ignored, but needed as this is an out Argument of GetDataAccordingToContactComboType...

            if (AContactComboType == TOverallContactComboType.occtPrimaryEmail)
            {
                ErrorCode = PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_SET_BUT_IT_ISNT_CURRENT;
            }
            else if (AContactComboType == TOverallContactComboType.occtSecondaryEmail)
            {
                ErrorCode = PetraErrorCodes.ERR_SECONDARY_EMAIL_ADDR_SET_BUT_IT_ISNT_CURRENT;
            }
            else if (AContactComboType == TOverallContactComboType.occtEmailWithinOrganisation)
            {
                ErrorCode = PetraErrorCodes.ERR_OFFICE_EMAIL_ADDR_SET_BUT_IT_ISNT_CURRENT;
            }
            else
            {
                throw new EOPAppException("AContactComboType Argument must designate an E-Mail ComboBox");
            }

            GetDataAccordingToContactComboType(AContactComboType,
                out ComboBoxForContactComboType, out ComboTypeStr);

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(ErrorCode),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, ComboBoxForContactComboType, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);

            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for E-Mail ComboBoxes.
        /// </summary>
        private void ValidationEmailAddrSetButNotAmongEmailAddrs(TOverallContactComboType AContactComboType)
        {
            const string ResCont = "ContactDetails_EmailAddress_Set_But_Not_Among_Email_Addrs";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            string ErrorCode;
            TCmbAutoComplete ComboBoxForContactComboType;
            string ComboTypeStr;   // Ignored, but needed as this is an out Argument of GetDataAccordingToContactComboType...

            if (AContactComboType == TOverallContactComboType.occtPrimaryEmail)
            {
                ErrorCode = PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_SET_BUT_NOT_AMONG_EMAIL_ADDR;
            }
            else if (AContactComboType == TOverallContactComboType.occtSecondaryEmail)
            {
                ErrorCode = PetraErrorCodes.ERR_SECONDARY_EMAIL_ADDR_SET_BUT_NOT_AMONG_EMAIL_ADDR;
            }
            else if (AContactComboType == TOverallContactComboType.occtEmailWithinOrganisation)
            {
                ErrorCode = PetraErrorCodes.ERR_OFFICE_EMAIL_ADDR_SET_BUT_NOT_AMONG_EMAIL_ADDR;
            }
            else
            {
                throw new EOPAppException("AContactComboType Argument must designate an E-Mail ComboBox");
            }

            GetDataAccordingToContactComboType(AContactComboType,
                out ComboBoxForContactComboType, out ComboTypeStr);

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(ErrorCode),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, ComboBoxForContactComboType, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);

            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        #endregion

        #region Events

        private void HandlePrimaryPhoneSelectedValueChanged(object sender, EventArgs e)
        {
            if (!FPhoneSelectedValueChangedEvent)
            {
                UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType.occtPrimaryPhone, false);

                // Update the <F12>/<SHIFT>+<F12> function key hint in the StatusBar Text
                UpdateValueManual();
            }
        }

        private void HandlePrimaryEmailSelectedValueChanged(object sender, EventArgs e)
        {
            if (!FEmailSelectedValueChangedEvent)
            {
                UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType.occtPrimaryEmail, false);

                // Update the <F12>/<SHIFT>+<F12> function key hint in the StatusBar Text
                UpdateValueManual();
            }

            btnLaunchHyperlinkPrefEMail.Enabled = (cmbPrimaryEMail.Text != String.Empty);
        }

        private void HandlePhoneWithinTheOrganisationSelectedValueChanged(object sender, EventArgs e)
        {
            if (!FPhoneSelectedValueChangedEvent)
            {
                UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType.occtPhoneWithinOrganisation, false);

                // Update the <F12>/<SHIFT>+<F12> function key hint in the StatusBar Text
                UpdateValueManual();
            }
        }

        private void HandleEMailWithinTheOrganisationSelectedValueChanged(object sender, EventArgs e)
        {
            if (!FEmailSelectedValueChangedEvent)
            {
                UpdateDataRowsPertainingToOvrlContSettgsCombo(TOverallContactComboType.occtEmailWithinOrganisation, false);

                // Update the <F12>/<SHIFT>+<F12> function key hint in the StatusBar Text
                UpdateValueManual();
            }

            btnLaunchHyperlinkEMailWithinOrg.Enabled = (cmbEMailWithinTheOrganisation.Text != String.Empty);
        }

        private void HandleSecondaryEmailSelectedValueChanged(object sender, EventArgs e)
        {
            if (!FEmailSelectedValueChangedEvent)
            {
                // Update the <F12>/<SHIFT>+<F12> function key hint in the StatusBar Text
                UpdateValueManual();
            }

            btnLaunchHyperlinkSecondaryEMail.Enabled = (cmbSecondaryEMail.Text != String.Empty);
        }

        #endregion
    }
}