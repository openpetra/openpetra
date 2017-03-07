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
using System.Collections.Generic;
using Ict.Common.DB;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;

namespace Ict.Petra.Server.MPartner.Common
{
    ///<summary>
    ///
    ///</summary>
    public class TFormLetterTools
    {
        /// <summary>
        /// build and return the address according to country and address layout code
        /// </summary>
        /// <param name="AFormData"></param>
        /// <param name="AAddressLayoutCode"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static String BuildAddressBlock(TFormDataPartner AFormData,
            String AAddressLayoutCode,
            TPartnerClass APartnerClass,
            TDBTransaction ATransaction)
        {
            PAddressBlockTable AddressBlockTable;
            String AddressLayoutBlock = "";

            if ((AAddressLayoutCode == null)
                || (AAddressLayoutCode == ""))
            {
                // this should not happen but just in case we use SMLLABEL as default layout code
                AddressBlockTable = PAddressBlockAccess.LoadByPrimaryKey(AFormData.CountryCode, "SMLLABEL", ATransaction);

                if (AddressBlockTable.Count == 0)
                {
                    // if no address block layout could be found for given country then try to retrieve for default "99"
                    AddressBlockTable = PAddressBlockAccess.LoadByPrimaryKey("99", "SMLLABEL", ATransaction);
                }
            }
            else
            {
                AddressBlockTable = PAddressBlockAccess.LoadByPrimaryKey(AFormData.CountryCode, AAddressLayoutCode, ATransaction);

                if (AddressBlockTable.Count == 0)
                {
                    // if no address block layout could be found for given country then try to retrieve for default "99"
                    AddressBlockTable = PAddressBlockAccess.LoadByPrimaryKey("99", AAddressLayoutCode, ATransaction);
                }
            }

            if (AddressBlockTable.Count == 0)
            {
                return "";
            }
            else
            {
                PAddressBlockRow AddressBlockRow = (PAddressBlockRow)AddressBlockTable.Rows[0];
                AddressLayoutBlock = AddressBlockRow.AddressBlockText;
            }

            return BuildAddressBlock(AddressLayoutBlock, AFormData, APartnerClass, ATransaction);
        }

        /// <summary>
        /// build and return the address according to the template address layout block
        /// </summary>
        /// <param name="AAddressLayoutBlock"></param>
        /// <param name="AFormData"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static String BuildAddressBlock(String AAddressLayoutBlock,
            TFormDataPartner AFormData,
            TPartnerClass APartnerClass,
            TDBTransaction ATransaction)
        {
            String AddressBlock = "";

            List <String>AddressTokenList = new List <String>();
            String AddressLineText = "";
            String AddressLineTokenText = "";
            Boolean PrintAnyway = false;
            Boolean CapsOn = false;
            Boolean UseContact = false;
            String SpacePlaceholder = "";

            PPersonTable PersonTable;
            PPersonRow PersonRow = null;
            PFamilyTable FamilyTable;
            PFamilyRow FamilyRow = null;
            Int64 ContactPartnerKey = 0;
            string workingText = string.Empty;


            AddressTokenList = BuildTokenListFromAddressLayoutBlock(AAddressLayoutBlock);

            // initialize values
            AddressLineText = "";
            PrintAnyway = false;

            foreach (String AddressLineToken in AddressTokenList)
            {
                switch (AddressLineToken)
                {
                    case "[[TitleAndSpace]]":
                    case "[[FirstNameAndSpace]]":
                    case "[[FirstInitialAndSpace]]":
                    case "[[LastNameAndSpace]]":

                        SpacePlaceholder = " ";
                        break;

                    default:

                        SpacePlaceholder = "";
                        break;
                }

                switch (AddressLineToken)
                {
                    case "[[AcademicTitle]]":

                        if (UseContact)
                        {
                            if (PersonRow != null)
                            {
                                AddressLineText += ConvertIfUpperCase(PersonRow.AcademicTitle, CapsOn);
                            }
                        }
                        else
                        {
                            if (AFormData.GetType() == typeof(TFormDataPerson))
                            {
                                AddressLineText += ConvertIfUpperCase(((TFormDataPerson)AFormData).AcademicTitle, CapsOn);
                            }
                        }

                        break;

                    case "[[AddresseeType]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.AddresseeType, CapsOn);
                        break;

                    case "[[Address3]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.Address3, CapsOn);
                        break;

                    case "[[CapsOn]]":
                        CapsOn = true;
                        break;

                    case "[[CapsOff]]":
                        CapsOn = false;
                        break;

                    case "[[City]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.City, CapsOn);
                        break;

                    case "[[CountryName]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.CountryName, CapsOn);
                        break;

                    case "[[CountryInLocalLanguage]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.CountryInLocalLanguage, CapsOn);
                        break;

                    case "[[County]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.County, CapsOn);
                        break;

                    case "[[UseContact]]":

                        /* Get the person or family record that is acting as the contact
                         *  only applicable to churches and organisations. */
                        switch (APartnerClass)
                        {
                            case TPartnerClass.CHURCH:
                                PChurchTable ChurchTable;
                                PChurchRow ChurchRow;
                                ChurchTable = PChurchAccess.LoadByPrimaryKey(Convert.ToInt64(AFormData.PartnerKey), ATransaction);

                                if (ChurchTable.Count > 0)
                                {
                                    ChurchRow = (PChurchRow)ChurchTable.Rows[0];

                                    if (!ChurchRow.IsContactPartnerKeyNull())
                                    {
                                        ContactPartnerKey = ChurchRow.ContactPartnerKey;
                                    }
                                }

                                break;

                            case TPartnerClass.ORGANISATION:
                                POrganisationTable OrganisationTable;
                                POrganisationRow OrganisationRow;
                                OrganisationTable = POrganisationAccess.LoadByPrimaryKey(Convert.ToInt64(AFormData.PartnerKey), ATransaction);

                                if (OrganisationTable.Count > 0)
                                {
                                    OrganisationRow = (POrganisationRow)OrganisationTable.Rows[0];

                                    if (!OrganisationRow.IsContactPartnerKeyNull())
                                    {
                                        ContactPartnerKey = OrganisationRow.ContactPartnerKey;
                                    }
                                }

                                break;

                            default:
                                ContactPartnerKey = 0;
                                break;
                        }

                        if (ContactPartnerKey > 0)
                        {
                            PersonTable = PPersonAccess.LoadByPrimaryKey(ContactPartnerKey, ATransaction);

                            if (PersonTable.Count > 0)
                            {
                                PersonRow = (PPersonRow)PersonTable.Rows[0];
                            }
                            else
                            {
                                FamilyTable = PFamilyAccess.LoadByPrimaryKey(ContactPartnerKey, ATransaction);

                                if (FamilyTable.Count > 0)
                                {
                                    FamilyRow = (PFamilyRow)FamilyTable.Rows[0];
                                }
                            }
                        }

                        UseContact = true;
                        break;

                    case "[[CountryCode]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.CountryCode, CapsOn);
                        break;

                    case "[[Decorations]]":

                        if (UseContact)
                        {
                            if (PersonRow != null)
                            {
                                AddressLineText += ConvertIfUpperCase(PersonRow.Decorations, CapsOn);
                            }
                        }
                        else
                        {
                            if (AFormData.GetType() == typeof(TFormDataPerson))
                            {
                                AddressLineText += ConvertIfUpperCase(((TFormDataPerson)AFormData).Decorations, CapsOn);
                            }
                        }

                        break;

                    case "[[FirstName]]":
                    case "[[FirstNameAndSpace]]":

                        AddressLineTokenText = "";

                        if (UseContact)
                        {
                            if (PersonRow != null)
                            {
                                AddressLineTokenText = ConvertIfUpperCase(PersonRow.FirstName, CapsOn);
                            }
                            else if (FamilyRow != null)
                            {
                                AddressLineTokenText = ConvertIfUpperCase(FamilyRow.FirstName, CapsOn);
                            }
                        }
                        else
                        {
                            AddressLineTokenText = ConvertIfUpperCase(AFormData.FirstName, CapsOn);
                        }

                        if ((AddressLineTokenText != null)
                            && (AddressLineTokenText.Length > 0))
                        {
                            AddressLineText += AddressLineTokenText + SpacePlaceholder;
                        }

                        break;

                    case "[[FirstInitial]]":
                    case "[[FirstInitialAndSpace]]":

                        AddressLineTokenText = "";

                        if (UseContact)
                        {
                            if (PersonRow != null)
                            {
                                if (PersonRow.FirstName.Length > 0)
                                {
                                    AddressLineTokenText = ConvertIfUpperCase(PersonRow.FirstName.Substring(0, 1), CapsOn);
                                }
                            }
                            else if (FamilyRow != null)
                            {
                                if (PersonRow.FirstName.Length > 0)
                                {
                                    AddressLineTokenText = ConvertIfUpperCase(FamilyRow.FirstName.Substring(0, 1), CapsOn);
                                }
                            }
                        }
                        else
                        {
                            if (AFormData.FirstName.Length > 0)
                            {
                                AddressLineTokenText = ConvertIfUpperCase(AFormData.FirstName.Substring(0, 1), CapsOn);
                            }
                        }

                        if ((AddressLineTokenText != null)
                            && (AddressLineTokenText.Length > 0))
                        {
                            AddressLineText += AddressLineTokenText + SpacePlaceholder;
                        }

                        break;

                    case "[[LastName]]":
                    case "[[LastNameAndSpace]]":

                        AddressLineTokenText = "";

                        if (UseContact)
                        {
                            if (PersonRow != null)
                            {
                                AddressLineTokenText = ConvertIfUpperCase(PersonRow.FamilyName, CapsOn);
                            }
                            else if (FamilyRow != null)
                            {
                                AddressLineTokenText = ConvertIfUpperCase(FamilyRow.FamilyName, CapsOn);
                            }
                        }
                        else
                        {
                            AddressLineTokenText = ConvertIfUpperCase(AFormData.LastName, CapsOn);
                        }

                        if ((AddressLineTokenText != null)
                            && (AddressLineTokenText.Length > 0))
                        {
                            AddressLineText += AddressLineTokenText + SpacePlaceholder;
                        }

                        break;

                    case "[[Address1]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.Address1, CapsOn);
                        break;

                    case "[[MiddleName]]":

                        if (UseContact)
                        {
                            if (PersonRow != null)
                            {
                                AddressLineText += ConvertIfUpperCase(PersonRow.MiddleName1, CapsOn);
                            }
                        }
                        else
                        {
                            if (AFormData.GetType() == typeof(TFormDataPerson))
                            {
                                AddressLineText += ConvertIfUpperCase(((TFormDataPerson)AFormData).MiddleName, CapsOn);
                            }
                        }

                        break;

                    case "[[Org/Church]]":

                        /* if the contact person is being printed then might still want the
                         *  Organisation or Church name printed.  This does it but only if there
                         *  is a valid contact. */
                        if (UseContact)
                        {
                            AddressLineText += ConvertIfUpperCase(AFormData.ShortName, CapsOn);
                        }

                        break;

                    case "[[PartnerKey]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.PartnerKey, CapsOn);
                        break;

                    case "[[ShortName]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.ShortName, CapsOn);
                        break;

                    case "[[LocalName]]":

                        if (AFormData.LocalName != "")
                        {
                            AddressLineText += ConvertIfUpperCase(AFormData.LocalName, CapsOn);
                        }
                        else
                        {
                            AddressLineText += ConvertIfUpperCase(AFormData.ShortName, CapsOn);
                        }

                        break;

                    case "[[PostalCode]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.PostalCode, CapsOn);
                        break;

                    case "[[Enclosures]]":
                        AddressLineText += AFormData.Enclosures;
                        break;

                    case "[[MailingCode]]":
                        AddressLineText += AFormData.MailingCode;
                        break;

                    case "[[Tab]]":
                        AddressLineText += "\t";
                        break;

                    case "[[Space]]":
                        AddressLineText += " ";
                        break;

                    case "[[AddressStreet2]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.AddressStreet2, CapsOn);
                        break;

                    case "[[Title]]":
                    case "[[TitleAndSpace]]":
                        AddressLineTokenText = ConvertIfUpperCase(AFormData.Title, CapsOn);

                        if ((AddressLineTokenText != null)
                            && (AddressLineTokenText.Length > 0))
                        {
                            AddressLineText += AddressLineTokenText + SpacePlaceholder;
                        }

                        break;

                    case "[[NoSuppress]]":
                        PrintAnyway = true;
                        break;

                    case "[[LocationKey]]":
                        AddressLineText += AFormData.LocationKey;
                        break;

                    case "[[LineFeed]]":

                        // only add line if not empty and not suppressed
                        if (PrintAnyway
                            || (!PrintAnyway
                                && (AddressLineText.Trim() != "")))
                        {
                            AddressBlock += AddressLineText + "\r\n";
                        }

                        // reset values
                        AddressLineText = "";
                        PrintAnyway = false;
                        break;

                    default:
                        AddressLineText += ConvertIfUpperCase(AddressLineToken, CapsOn);
                        break;
                }
            }

            // this is only for last line if there was no line feed:
            // only add line if not empty and not suppressed
            if (PrintAnyway
                || (!PrintAnyway
                    && (AddressLineText.Trim() != "")))
            {
                AddressBlock += AddressLineText + "\r\n";
            }

            // or just get the element list from cached table (since we need to get different ones depending on country)

            return AddressBlock;
        }

        /// <summary>
        /// build and return the address according to country and address layout code
        /// </summary>
        /// <param name="AAddressLayoutBlock"></param>
        /// <returns>list of token built from address layout string</returns>
        [RequireModulePermission("PTNRUSER")]
        private static List <String>BuildTokenListFromAddressLayoutBlock(String AAddressLayoutBlock)
        {
            List <String>TokenList = new List <String>();
            String AddressBlock = AAddressLayoutBlock;
            Int32 IndexStartToken;
            Int32 IndexEndToken;

            AddressBlock = AddressBlock.Replace("\r\n", "[[LineFeed]]");

            do
            {
                IndexStartToken = AddressBlock.IndexOf("[[");

                if (IndexStartToken == 0)
                {
                    // we have reached a real token --> find index of end of token
                    IndexEndToken = AddressBlock.IndexOf("]]");
                    TokenList.Add(AddressBlock.Substring(0, IndexEndToken + 2));
                    AddressBlock = AddressBlock.Substring(IndexEndToken + 2);
                }
                else if (IndexStartToken > 0)
                {
                    // this is normal text before the next token --> just add this whole text as one "token"
                    TokenList.Add(AddressBlock.Substring(0, IndexStartToken));
                    AddressBlock = AddressBlock.Substring(IndexStartToken);
                }
                else if (IndexStartToken < 0)
                {
                    // no more token to be found --> just append rest of string
                    TokenList.Add(AddressBlock);
                    AddressBlock = "";
                }
            } while (AddressBlock.Length > 0);

            return TokenList;
        }

        /// <summary>
        /// convert a string to uppercase if needed (or otherwise return as is)
        /// </summary>
        /// <param name="AString"></param>
        /// <param name="AConvertToUpperCase"></param>
        /// <returns></returns>
        public static String ConvertIfUpperCase(String AString, Boolean AConvertToUpperCase)
        {
            if (AConvertToUpperCase)
            {
                return AString.ToUpper();
            }

            return AString;
        }
    }
}