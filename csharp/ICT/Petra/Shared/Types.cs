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

namespace Ict.Petra.Shared
{
    /// <summary>
    /// partner classes
    /// </summary>
    public enum TPartnerClass
    {
        /// <summary>
        /// person (required for personnel, date of birth, etc
        /// </summary>
        PERSON,

        /// <summary>
        /// family is the default class for any supporter etc
        /// </summary>
        FAMILY,

        /// <summary>
        /// church group
        /// </summary>
        CHURCH,

        /// <summary>
        /// organisation, company
        /// </summary>
        ORGANISATION,

        /// <summary>
        /// bank
        /// </summary>
        BANK,

        /// <summary>
        /// unit, conference, project
        /// </summary>
        UNIT,

        /// <summary>
        /// venue for conference
        /// </summary>
        VENUE
    };

    /// <summary>
    /// type of addressee
    /// </summary>
    public enum TStdAddresseeTypeCode
    {
        /// <summary>
        /// female
        /// </summary>
        satcFEMALE,

        /// <summary>
        /// male
        /// </summary>
        satcMALE,

        /// <summary>
        /// church
        /// </summary>
        satcCHURCH,

        /// <summary>
        /// couple
        /// </summary>
        satcCOUPLE,

        /// <summary>
        /// default
        /// </summary>
        satcDEFAULT,

        /// <summary>
        /// family
        /// </summary>
        satcFAMILY,

        /// <summary>
        /// organisation, company
        /// </summary>
        satcORGANISA
    };

    /// <summary>
    /// partner status
    /// </summary>
    public enum TStdPartnerStatusCode
    {
        /// <summary>
        /// active
        /// </summary>
        spscACTIVE,

        /// <summary>
        /// inactive
        /// </summary>
        spscINACTIVE,

        /// <summary>
        /// partner has died
        /// </summary>
        spscDIED,

        /// <summary>
        /// partner has been merged into another partner
        /// </summary>
        spscMERGED,

        /// <summary>
        /// partner is private to only one user; deprecated
        /// </summary>
        spscPRIVATE
    };

    /// <summary>
    /// table access permissions
    /// </summary>
    public enum TTableAccessPermission
    {
        /// <summary>
        /// read access
        /// </summary>
        tapINQUIRE,

        /// <summary>
        /// write access
        /// </summary>
        tapMODIFY,

        /// <summary>
        /// can create new rows
        /// </summary>
        tapCREATE,

        /// <summary>
        /// can delete rows
        /// </summary>
        tapDELETE
    };

    /// <summary>
    /// where the partner was accessed the last time
    /// </summary>
    public enum TLastPartnerUse
    {
        /// <summary>
        /// mailroom department
        /// </summary>
        lpuMailroomPartner,

        /// <summary>
        /// personnel department
        /// </summary>
        lpuPersonnelPerson,

        /// <summary>
        /// as a unit
        /// </summary>
        lpuPersonnelUnit,

        /// <summary>
        /// for a conference
        /// </summary>
        lpuConferencePerson
    };

    /// <summary>
    /// enumeration of values for items that can be associated with modules
    /// </summary>
    public enum TModule
    {
        /// <summary>
        /// Partner module
        /// </summary>
        mPartner,

        /// <summary>
        /// Finance module
        /// </summary>
        mFinance,

        /// <summary>
        /// Personnel module
        /// </summary>
        mPersonnel,

        /// <summary>
        /// Financial Development module
        /// </summary>
        mFinDev,

        /// <summary>
        /// System Manager module
        /// </summary>
        mSysMan
    }

    /// <summary>
    /// sequences that can be used with the MCommon WebConnector TSequenceWebConnector
    /// </summary>
    public enum TSequenceNames
    {
        /// <summary>see petra.xml</summary>
        seq_application,
        /// <summary>see petra.xml</summary>
            seq_contact,
        /// <summary>see petra.xml</summary>
            seq_extract_number,
        /// <summary>see petra.xml</summary>
            seq_location_number,
        /// <summary>see petra.xml</summary>
            seq_pe_evaluation_number,
        /// <summary>see petra.xml</summary>
            seq_report_number,
        /// <summary>see petra.xml</summary>
            seq_general_ledger_master,
        /// <summary>see petra.xml</summary>
            seq_budget,
        /// <summary>see petra.xml</summary>
            seq_bank_details,
        /// <summary>see petra.xml</summary>
            seq_document,
        /// <summary>see petra.xml</summary>
            seq_past_experience,
        /// <summary>see petra.xml</summary>
            seq_staff_data,
        /// <summary>see petra.xml</summary>
            seq_job,
        /// <summary>see petra.xml</summary>
            seq_job_assignment,
        /// <summary>see petra.xml</summary>
            seq_data_label,
        /// <summary>see petra.xml</summary>
            seq_foundation_proposal,
        /// <summary>see petra.xml</summary>
            seq_proposal_detail,
        /// <summary>see petra.xml</summary>
            seq_form_letter_insert,
        /// <summary>see petra.xml</summary>
            seq_workflow,
        /// <summary>see petra.xml</summary>
            seq_file_info,
        /// <summary>see petra.xml</summary>
            seq_person_skill,
        /// <summary>see petra.xml</summary>
            seq_booking,
        /// <summary>see petra.xml</summary>
            seq_room_alloc,
        /// <summary>see petra.xml</summary>
            seq_ar_invoice,
        /// <summary>see petra.xml</summary>
            seq_match_number,
        /// <summary>see petra.xml</summary>
            seq_statement_number,
        /// <summary>AP Document Reference</summary>
            seq_ap_document,
        /// <summary>see petra.xml</summary>
            seq_partner_attribute_index
    }

    /// <summary>
    /// provides useful functions for shared types
    /// </summary>
    public class SharedTypes
    {
        /// <summary>
        /// convert partner class from string to enum
        /// </summary>
        /// <param name="APartnerClass"></param>
        /// <returns></returns>
        public static TPartnerClass PartnerClassStringToEnum(String APartnerClass)
        {
            TPartnerClass ReturnValue;

            ReturnValue = TPartnerClass.PERSON;

            if (APartnerClass == TPartnerClass.PERSON.ToString())
            {
                ReturnValue = TPartnerClass.PERSON;
            }
            else if (APartnerClass == TPartnerClass.FAMILY.ToString())
            {
                ReturnValue = TPartnerClass.FAMILY;
            }
            else if (APartnerClass == TPartnerClass.CHURCH.ToString())
            {
                ReturnValue = TPartnerClass.CHURCH;
            }
            else if (APartnerClass == TPartnerClass.ORGANISATION.ToString())
            {
                ReturnValue = TPartnerClass.ORGANISATION;
            }
            else if (APartnerClass == TPartnerClass.BANK.ToString())
            {
                ReturnValue = TPartnerClass.BANK;
            }
            else if (APartnerClass == TPartnerClass.UNIT.ToString())
            {
                ReturnValue = TPartnerClass.UNIT;
            }
            else if (APartnerClass == TPartnerClass.VENUE.ToString())
            {
                ReturnValue = TPartnerClass.VENUE;
            }

            return ReturnValue;
        }

        /// <summary>
        /// enumeration of values for items that can be associated with Office specific data labels
        /// </summary>
        public enum TModuleEnum
        {
            /// <summary>
            /// Partner module
            /// </summary>
            Partner,

            /// <summary>
            /// Finance module
            /// </summary>
            Finance,

            /// <summary>
            /// Personnel module
            /// </summary>
            Personnel,

            /// <summary>
            /// Financial Development module
            /// </summary>
            FinDev,

            /// <summary>
            /// System Manager module
            /// </summary>
            SysMan
        }


        /// <summary>
        /// convert partner class from enum to string
        /// </summary>
        /// <param name="APartnerClass"></param>
        /// <returns></returns>
        public static String PartnerClassEnumToString(TPartnerClass APartnerClass)
        {
            switch (APartnerClass)
            {
                case TPartnerClass.PERSON:
                case TPartnerClass.FAMILY:
                case TPartnerClass.CHURCH:
                case TPartnerClass.ORGANISATION:
                case TPartnerClass.BANK:
                case TPartnerClass.UNIT:
                case TPartnerClass.VENUE:
                    return APartnerClass.ToString();
            }

            return "##UNCONVERTABLE##";
        }

        /*
         * It turns out that there's no demand for this method as yet.
         * A single letter is used in the Gift Batch Detail tab,
         * but in that case the SQL SUBSTRING function is used.
         *
         * /// <summary>
         * /// Get a single-letter representation of a Partner Class
         * /// </summary>
         * /// <remarks>In this version, just return the first letter</remarks>
         * /// <param name="APartnerClass"></param>
         * /// <returns></returns>
         * public static String PartnerClassAbrev(String APartnerClass)
         * {
         *  return APartnerClass.Substring(0, 1);
         * }
         */

        /// <summary>
        /// convert addressee type from string to enum
        /// </summary>
        /// <param name="AAddresseeTypeCode"></param>
        /// <returns></returns>
        public static TStdAddresseeTypeCode StdAddresseeTypeCodeStringToEnum(String AAddresseeTypeCode)
        {
            // just to have a default
            TStdAddresseeTypeCode ReturnValue = TStdAddresseeTypeCode.satcDEFAULT;

            if (AAddresseeTypeCode == "1-FEMALE")
            {
                ReturnValue = TStdAddresseeTypeCode.satcFEMALE;
            }
            else if (AAddresseeTypeCode == "1-MALE")
            {
                ReturnValue = TStdAddresseeTypeCode.satcMALE;
            }
            else if (AAddresseeTypeCode == "CHURCH")
            {
                ReturnValue = TStdAddresseeTypeCode.satcCHURCH;
            }
            else if (AAddresseeTypeCode == "COUPLE")
            {
                ReturnValue = TStdAddresseeTypeCode.satcCOUPLE;
            }
            else if (AAddresseeTypeCode == "DEFAULT")
            {
                ReturnValue = TStdAddresseeTypeCode.satcDEFAULT;
            }
            else if (AAddresseeTypeCode == "FAMILY")
            {
                ReturnValue = TStdAddresseeTypeCode.satcFAMILY;
            }
            else if (AAddresseeTypeCode == "ORGANISA")
            {
                ReturnValue = TStdAddresseeTypeCode.satcORGANISA;
            }

            return ReturnValue;
        }

        /// <summary>
        /// convert addressee type from enum to string
        /// </summary>
        /// <param name="AAddresseeTypeCode"></param>
        /// <returns></returns>
        public static String StdAddresseeTypeCodeEnumToString(TStdAddresseeTypeCode AAddresseeTypeCode)
        {
            String ReturnValue;

            ReturnValue = "##UNCONVERTABLE##";

            switch (AAddresseeTypeCode)
            {
                case TStdAddresseeTypeCode.satcFEMALE:
                    ReturnValue = "1-FEMALE";
                    break;

                case TStdAddresseeTypeCode.satcMALE:
                    ReturnValue = "1-MALE";
                    break;

                case TStdAddresseeTypeCode.satcCHURCH:
                    ReturnValue = "CHURCH";
                    break;

                case TStdAddresseeTypeCode.satcCOUPLE:
                    ReturnValue = "COUPLE";
                    break;

                case TStdAddresseeTypeCode.satcDEFAULT:
                    ReturnValue = "DEFAULT";
                    break;

                case TStdAddresseeTypeCode.satcFAMILY:
                    ReturnValue = "FAMILY";
                    break;

                case TStdAddresseeTypeCode.satcORGANISA:
                    ReturnValue = "ORGANISA";
                    break;
            }

            return ReturnValue;
        }

        /// <summary>
        /// convert partner status from string to enum
        /// </summary>
        /// <param name="APartnerStatusCode"></param>
        /// <returns></returns>
        public static TStdPartnerStatusCode StdPartnerStatusCodeStringToEnum(String APartnerStatusCode)
        {
            // just to have a default
            TStdPartnerStatusCode ReturnValue = TStdPartnerStatusCode.spscACTIVE;

            if (APartnerStatusCode == "ACTIVE")
            {
                ReturnValue = TStdPartnerStatusCode.spscACTIVE;
            }
            else if (APartnerStatusCode == "INACTIVE")
            {
                ReturnValue = TStdPartnerStatusCode.spscINACTIVE;
            }
            else if (APartnerStatusCode == "DIED")
            {
                ReturnValue = TStdPartnerStatusCode.spscDIED;
            }
            else if (APartnerStatusCode == "MERGED")
            {
                ReturnValue = TStdPartnerStatusCode.spscMERGED;
            }
            else if (APartnerStatusCode == "PRIVATE")
            {
                ReturnValue = TStdPartnerStatusCode.spscACTIVE;
            }

            return ReturnValue;
        }

        /// <summary>
        /// convert partner status from enum to string
        /// </summary>
        /// <param name="APartnerStatusCode"></param>
        /// <returns></returns>
        public static String StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode APartnerStatusCode)
        {
            // Return the Enum value from the 4th character onwards (strip out prefix)
            return APartnerStatusCode.ToString("G").Substring(4);
        }
    }
}