//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//       ChristianK
//
// Copyright 2004-2021 by OM International
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
using System.Data;
using System.Xml;

using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.IO;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared;
using Ict.Petra.Server.MSysMan.Common.WebConnectors;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.Partner.WebConnectors;

namespace Ict.Petra.Server.MPartner.ImportExport
{
    /// <summary>
    /// This will create a new partner and new relationships, match addresses etc
    /// </summary>
    public class TPartnerImportCSV
    {
        private Int32 FLocationKey = -1;
        private TVerificationResultCollection ResultsCol;
        private Int32 FCurrentLine = -1;
        private String ResultsContext;
        private List<String> FUnusedColumns;

        private void AddVerificationResult(String AResultText, TResultSeverity ASeverity = TResultSeverity.Resv_Critical)
        {
            if (ASeverity != TResultSeverity.Resv_Status)
            {
                TLogging.Log(AResultText);
            }

            ResultsCol.Add(new TVerificationResult(ResultsContext, AResultText, ASeverity));
        }
        
        private bool HasColumnValue(DataRow ARow, string AAttrName)
        {
            if (!ARow.Table.Columns.Contains(AAttrName))
            {
                return false;
            }
            return (ARow[AAttrName] != System.DBNull.Value) && (ARow[AAttrName].ToString() != String.Empty);
        }

        private string GetColumnValue(DataRow ARow, string AAttrName)
        {
            if (!ARow.Table.Columns.Contains(AAttrName))
            {
                return String.Empty;
            }
            if (FUnusedColumns.Contains(AAttrName))
            {
                FUnusedColumns.Remove(AAttrName);
            }
            return ARow[AAttrName].ToString();
        }

        /// <summary>
        /// Import data from an imported file
        /// </summary>
        /// <param name="ATable"></param>
        /// <param name="ADateFormat">A date format string like MDY or DMY.  Only the first character is significant and must be M for month first.
        /// The date format string is only relevant to ambiguous dates which typically have a 1 or 2 digit month</param>
        /// <param name="AReferenceResults"></param>
        /// <returns></returns>
        public PartnerImportExportTDS ImportData(DataTable ATable, string ADateFormat, ref TVerificationResultCollection AReferenceResults)
        {
            PartnerImportExportTDS ResultDS = new PartnerImportExportTDS();

            FLocationKey = -1;
            int BankingDetailsKey = -1;
            ResultsCol = AReferenceResults;
            TDBTransaction Transaction = new TDBTransaction();
            bool SubmissionOK = true;
            FUnusedColumns = new List<string>();

            foreach (DataColumn c in ATable.Columns)
            {
                FUnusedColumns.Add(c.ColumnName);
            }

            // starting in line 2, because there is the line with the captions
            FCurrentLine = 2;

            DBAccess.WriteTransaction(ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    foreach (DataRow r in ATable.Rows)
                    {
                        ResultsContext = "CSV Import";
                        String PartnerClass = GetColumnValue(r, MPartnerConstants.PARTNERIMPORT_PARTNERCLASS).ToUpper();
                        Int64 PartnerKey = 0;
                        int LocationKey = 0;
                        PPartnerRow newPartner = null;

                        if (PartnerClass.Length == 0)
                        {
                            PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;

                            // if the import line contains an event then this will need to be for a person
                            if (HasColumnValue(r, MPartnerConstants.PARTNERIMPORT_EVENTKEY))
                            {
                                PartnerClass = MPartnerConstants.PARTNERCLASS_PERSON;
                            }

                            //TODOWB: if partner class is not set then check if for example a value is set for column "EventPartnerKey" in which case we can assume it is a Person that is imported
                            // there may be other fields that hint for using Person
                        }

                        if ((PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY) || (PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON))
                        {
                            Boolean FamilyForPerson = (PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON);
                            ResultsContext = "CSV Import Family";
                            newPartner = CreateNewFamily(r, FamilyForPerson, out LocationKey, ref ResultDS, Transaction);
                            PartnerKey = newPartner.PartnerKey;
                            CreateSpecialTypes(r, PartnerKey, "Category", ref ResultDS, Transaction);
                            string AccountName = (GetColumnValue(r, "FirstName") + " " +
                                GetColumnValue(r, "FamilyName")).Trim();
                            CreateBankAccounts(r, PartnerKey, AccountName, ref BankingDetailsKey, "IBAN",
                                ref ResultDS, Transaction, ref ResultsCol);
                            CreateOutputData(r, PartnerKey, MPartnerConstants.PARTNERCLASS_FAMILY,
                                (PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY), ref ResultDS);
                        }

                        if (PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                        {
                            ResultsContext = "CSV Import Person";
                            newPartner = CreateNewPerson(PartnerKey, LocationKey, r, ADateFormat, ref ResultDS, Transaction);
                            PartnerKey = newPartner.PartnerKey;
                            CreateShortTermApplication(r, PartnerKey, ADateFormat, ref ResultDS, Transaction);
                            CreateSubscriptions(r, PartnerKey, ref ResultDS, Transaction);
                            CreateContacts(r, PartnerKey, ADateFormat, ref ResultDS, "_1", Transaction);
                            CreateContacts(r, PartnerKey, ADateFormat, ref ResultDS, "_2", Transaction);
                            CreatePassport(r, PartnerKey, ADateFormat, ref ResultDS, Transaction);
                            CreateSpecialNeeds(r, PartnerKey, ref ResultDS, Transaction);
                            CreateOutputData(r, PartnerKey, PartnerClass, true, ref ResultDS);
                        }

                        FCurrentLine += 1;
                    }
                });

            if (FUnusedColumns.Count > 0)
            {
                AddVerificationResult("Unknown Column(s): " + String.Join(" ", FUnusedColumns.ToArray()));
                return new PartnerImportExportTDS();
            }

            return ResultDS;
        }

        /// <summary>
        /// Create new partner, family, location and PartnerLocation records in MainDS
        /// </summary>
        private PPartnerRow CreateNewFamily(DataRow ARow, Boolean AFamilyForPerson, out int ALocationKey, ref PartnerImportExportTDS AMainDS,
            TDBTransaction ATransaction)
        {
            Int64 FamilyKey = 0;
            Boolean IsNewRecord = true;

            PPartnerRow newPartner = AMainDS.PPartner.NewRowTyped();

            AMainDS.PPartner.Rows.Add(newPartner);

            // first check in FamilyPartnerKey for Person Key, otherwise try to find family for given PersonPartnerKey, or then check in field PartnerKey
            String strPartnerKey = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_FAMILYPARTNERKEY);
            newPartner.PartnerKey = 0;

            if (strPartnerKey.Length > 0)
            {
                try
                {
                    FamilyKey = long.Parse(strPartnerKey);
                }
                catch (System.FormatException)
                {
                    AddVerificationResult("Bad number format in FamilyPartnerKey: " + strPartnerKey);
                }
            }
            else
            {
                if (AFamilyForPerson)
                {
                    // if we don't have the family partner key then we can check if we can find the family for a person partner key
                    String strPersonKey = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PERSONPARTNERKEY);

                    if (strPersonKey.Length > 0)
                    {
                        try
                        {
                            FamilyKey = TPartnerServerLookups.GetFamilyKeyForPerson(long.Parse(strPersonKey));
                        }
                        catch (System.FormatException)
                        {
                            AddVerificationResult("Bad number format in PersonPartnerKey: " + strPartnerKey);
                        }
                        catch (EOPAppException)
                        {
                            AddVerificationResult(string.Format("There is no matching Family key for the specified Person Partner key {0}",
                                    strPersonKey) +
                                "  Did you enter the key correctly?");
                        }
                    }
                }

                if ((FamilyKey == 0) && !AFamilyForPerson)
                {
                    strPartnerKey = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PARTNERKEY);

                    if (strPartnerKey.Length > 0)
                    {
                        try
                        {
                            FamilyKey = long.Parse(strPartnerKey);
                        }
                        catch (System.FormatException)
                        {
                            AddVerificationResult("Bad number format in PartnerKey: " + strPartnerKey);
                        }
                    }
                }
            }

            if (FamilyKey > 0)
            {
                //TODOWBxxx
                // now we know the family record that we need to look for in db
                PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(FamilyKey, ATransaction);

                if ((PartnerTable.Count > 0)
                    && (((PPartnerRow)PartnerTable.Rows[0]).PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY))
                {
                    // we have an existing family partner record
                    IsNewRecord = false;
                    newPartner.ItemArray = (object[])PartnerTable.Rows[0].ItemArray.Clone();
                    newPartner.AcceptChanges();
                }
                else
                {
                    // no partner found with this family key in this db --> need to create new family
                    IsNewRecord = true;
                    FamilyKey = 0;
                }
            }

            // now preset the partner key if it is not contained in the csv file or if given key cannot be found in db
            newPartner.PartnerKey = FamilyKey;

            if (newPartner.PartnerKey == 0)
            {
                newPartner.PartnerKey = (AMainDS.PPartner.Rows.Count + 1) * -1;
            }

            newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
            newPartner.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;

            if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_NOTESFAMILY))
            {
                newPartner.Comment = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_NOTESFAMILY);
            }

            if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_AQUISITION))
            {
                String AcquisitionCode = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_AQUISITION);

                newPartner.AcquisitionCode = (AcquisitionCode.Length > 0) ? AcquisitionCode : MPartnerConstants.PARTNERIMPORT_AQUISITION_DEFAULT;
            }
            else if (IsNewRecord)
            {
                newPartner.AcquisitionCode = MPartnerConstants.PARTNERIMPORT_AQUISITION_DEFAULT;
            }

            if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_ADDRESSEE_TYPE))
            {
                newPartner.AddresseeTypeCode = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_ADDRESSEE_TYPE);
            }
            else if (IsNewRecord)
            {
                newPartner.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_DEFAULT;

                string gender = GetGenderCode(ARow);

                if (gender == MPartnerConstants.GENDER_MALE)
                {
                    newPartner.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_MALE;
                }
                else if (gender == MPartnerConstants.GENDER_FEMALE)
                {
                    newPartner.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_FEMALE;
                }
            }

            if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_LANGUAGE))
            {
                newPartner.LanguageCode = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_LANGUAGE);
            }
            else if (IsNewRecord && TUserDefaults.HasDefault(MSysManConstants.PARTNER_LANGUAGECODE))
            {
                newPartner.LanguageCode = TUserDefaults.GetStringDefault(MSysManConstants.PARTNER_LANGUAGECODE);
            }

            if (IsNewRecord)
            {
                string[] giftReceiptingDefaults = new TSystemDefaults(ATransaction.DataBaseObj).GetStringDefault("GiftReceiptingDefaults", ",no").Split(new char[] { ',' });
                newPartner.ReceiptLetterFrequency = giftReceiptingDefaults[0];
                newPartner.ReceiptEachGift = giftReceiptingDefaults[1].ToUpper() == "YES" || giftReceiptingDefaults[1].ToUpper() == "TRUE";
            }

            PFamilyRow newFamily = AMainDS.PFamily.NewRowTyped();
            AMainDS.PFamily.Rows.Add(newFamily);

            if (!IsNewRecord)
            {
                // now we know the family record that we need to look for in db
                PFamilyTable FamilyTable = PFamilyAccess.LoadByPrimaryKey(FamilyKey, ATransaction);

                if (FamilyTable.Count > 0)
                {
                    // we have an existing family partner record
                    newFamily.ItemArray = (object[])FamilyTable.Rows[0].ItemArray.Clone();
                    newFamily.AcceptChanges();
                }
            }

            newFamily.PartnerKey = newPartner.PartnerKey;

            if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_FIRSTNAME))
            {
                newFamily.FirstName = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_FIRSTNAME);
            }

            if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_FAMILYNAME))
            {
                newFamily.FamilyName = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_FAMILYNAME);
            }

            if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_MARITALSTATUS))
            {
                newFamily.MaritalStatus = GetMaritalStatusCode(ARow, ATransaction);
            }

            if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_TITLE))
            {
                newFamily.Title = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_TITLE);
            }

            if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_LANGUAGE))
            {
                newPartner.LanguageCode = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_LANGUAGE);
            }

            if ((newFamily.FirstName == String.Empty) && (newFamily.FamilyName == String.Empty))
            {
                AddVerificationResult("Missing Firstname or family name in line " + FCurrentLine.ToString());
            }

            newPartner.PartnerShortName = Calculations.DeterminePartnerShortName(newFamily.FamilyName, newFamily.Title, newFamily.FirstName);

            ALocationKey = CreateNewLocation(ref AMainDS, newPartner, 0, ARow);
            FLocationKey -= 1;

            return newPartner;
        }

        /// <summary>
        /// Create a Partner and a Person having this FamilyKey, living at this address.
        /// </summary>
        /// <param name="AFamilyKey"></param>
        /// <param name="ALocationKey"></param>
        /// <param name="ARow"></param>
        /// <param name="ADateFormat">A date format string like MDY or DMY.  Only the first character is significant and must be M for month first.
        /// The date format string is only relevant to ambiguous dates which typically have a 1 or 2 digit month</param>
        /// <param name="AMainDS"></param>
        /// <param name="ATransaction"></param>
        private PPartnerRow CreateNewPerson(Int64 AFamilyKey, int ALocationKey, DataRow ARow, string ADateFormat,
            ref PartnerImportExportTDS AMainDS, TDBTransaction ATransaction)
        {
            Int64 PersonKey = 0;
            Boolean IsNewRecord = true;

            AMainDS.PFamily.DefaultView.RowFilter = String.Format("{0}={1}", PFamilyTable.GetPartnerKeyDBName(), AFamilyKey);
            PFamilyRow FamilyRow = (PFamilyRow)AMainDS.PFamily.DefaultView[0].Row;

            AMainDS.PPartner.DefaultView.RowFilter = String.Format("{0}={1}", PPartnerTable.GetPartnerKeyDBName(), AFamilyKey);
            PPartnerRow PartnerRow = (PPartnerRow)AMainDS.PPartner.DefaultView[0].Row;

            PPartnerRow newPartner = AMainDS.PPartner.NewRowTyped();
            AMainDS.PPartner.Rows.Add(newPartner);

            // check in PersonPartnerKey for Person Key, otherwise check in field PartnerKey
            String strPartnerKey = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PERSONPARTNERKEY);
            newPartner.PartnerKey = 0;

            if (strPartnerKey.Length > 0)
            {
                try
                {
                    PersonKey = long.Parse(strPartnerKey);
                }
                catch (System.FormatException)
                {
                    AddVerificationResult("Bad number format in PersonPartnerKey: " + strPartnerKey);
                }
            }
            else
            {
                strPartnerKey = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PARTNERKEY);

                if (strPartnerKey.Length > 0)
                {
                    try
                    {
                        PersonKey = long.Parse(strPartnerKey);
                    }
                    catch (System.FormatException)
                    {
                        AddVerificationResult("Bad number format in PartnerKey: " + strPartnerKey);
                    }
                }
            }

            if (PersonKey > 0)
            {
                //TODOWBxxx
                // now we know the family record that we need to look for in db
                PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(PersonKey, ATransaction);

                if ((PartnerTable.Count > 0)
                    && (((PPartnerRow)PartnerTable.Rows[0]).PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON))
                {
                    // we have an existing person partner record
                    IsNewRecord = false;
                    newPartner.ItemArray = (object[])PartnerTable.Rows[0].ItemArray.Clone();
                    newPartner.AcceptChanges();
                }
                else
                {
                    // no partner found with this person key in this db --> need to create new person
                    IsNewRecord = true;
                    PersonKey = 0;
                }
            }

            // now preset the partner key if it is not contained in the csv file or if given key cannot be found in db
            newPartner.PartnerKey = PersonKey;

            if (newPartner.PartnerKey == 0)
            {
                newPartner.PartnerKey = (AMainDS.PPartner.Rows.Count + 1) * -1;
            }

            newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_PERSON;
            newPartner.AddresseeTypeCode = PartnerRow.AddresseeTypeCode;
            newPartner.PartnerShortName = PartnerRow.PartnerShortName;
            newPartner.LanguageCode = PartnerRow.LanguageCode;
            newPartner.Comment = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_NOTES);
            newPartner.AcquisitionCode = PartnerRow.AcquisitionCode;
            newPartner.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;

            PPersonRow newPerson = AMainDS.PPerson.NewRowTyped();
            AMainDS.PPerson.Rows.Add(newPerson);

            if (!IsNewRecord)
            {
                // now we know the family record that we need to look for in db
                PPersonTable PersonTable = PPersonAccess.LoadByPrimaryKey(PersonKey, ATransaction);

                if (PersonTable.Count > 0)
                {
                    // we have an existing person partner record
                    newPerson.ItemArray = (object[])PersonTable.Rows[0].ItemArray.Clone();
                    newPerson.AcceptChanges();
                }
            }

            newPerson.PartnerKey = newPartner.PartnerKey;
            newPerson.FamilyKey = AFamilyKey;
            // When this record is imported, newPerson.FamilyId must be unique for this family!
            newPerson.FirstName = FamilyRow.FirstName;
            newPerson.FamilyName = FamilyRow.FamilyName;
            newPerson.Title = FamilyRow.Title;
            newPerson.Gender = GetGenderCode(ARow);
            newPerson.MaritalStatus = FamilyRow.MaritalStatus;

            String TimeString = "";
            try
            {
                TimeString = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_DATEOFBIRTH);

                if (TimeString.Length > 0)
                {
                    if (TimeString.StartsWith("eDateTime:"))
                    {
                        newPerson.DateOfBirth = TVariant.DecodeFromString(TimeString).ToDate();
                    }
                    else
                    {
                        newPerson.DateOfBirth = DateTime.Parse(TimeString, StringHelper.GetCultureInfoForDateFormat(ADateFormat));
                    }
                }
            }
            catch (System.FormatException)
            {
                string fmt = ADateFormat.StartsWith("M", StringComparison.OrdinalIgnoreCase) ? "month-day-year" : "day-month-year";
                AddVerificationResult(string.Format("Bad date of birth: {0} (Expected format: {1})", TimeString, fmt), TResultSeverity.Resv_Critical);
            }

            CreateNewLocation(ref AMainDS, newPartner, ALocationKey, ARow);

            AddVerificationResult("Person Record Created.", TResultSeverity.Resv_Status);

            return newPartner;
        }

        private int CreateNewLocation(ref PartnerImportExportTDS AMainDS, PPartnerRow ANewPartner, int ALocationKey, DataRow ARow)
        {
            PLocationRow newLocation = AMainDS.PLocation.NewRowTyped(true);

            // is this a PERSON and we use the same location as for the family? then ALocationKey will already be set
            if (ALocationKey == 0)
            {
                AMainDS.PLocation.Rows.Add(newLocation);
                newLocation.LocationKey = FLocationKey;
                newLocation.Locality = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_ADDRESS1);
                newLocation.StreetName = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_STREET);
                newLocation.Address3 = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_ADDRESS3);
                newLocation.PostalCode = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_POSTCODE);
                newLocation.City = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_CITY);
                newLocation.County = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_COUNTY);
                newLocation.CountryCode = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_COUNTRYCODE);

                ALocationKey = newLocation.LocationKey;
            }

            TPartnerContactDetails_LocationConversionHelper myHelper =
                new TPartnerContactDetails_LocationConversionHelper();

            PPartnerLocationRow partnerlocation = AMainDS.PPartnerLocation.NewRowTyped(true);
            myHelper.AddOldDBTableColumnsToPartnerLocation(AMainDS.PPartnerLocation);

            partnerlocation.LocationKey = ALocationKey;
            partnerlocation.SiteKey = 0;
            partnerlocation.PartnerKey = ANewPartner.PartnerKey;
            partnerlocation.DateEffective = DateTime.Now.Date;
            partnerlocation.LocationType = MPartnerConstants.LOCATIONTYPE_HOME;
            partnerlocation.SendMail = true;

            int SequenceCounter = 0;
            string email = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_EMAIL);
            string phone = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PHONE);
            string mobile = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_MOBILEPHONE);

            if (email.Length > 0)
            {
                PPartnerAttributeRow partnerAttributeRow = AMainDS.PPartnerAttribute.NewRowTyped();
                partnerAttributeRow.PartnerKey = ANewPartner.PartnerKey;
                partnerAttributeRow.AttributeType = MPartnerConstants.ATTR_TYPE_EMAIL;
                partnerAttributeRow.Index = 0;
                partnerAttributeRow.Primary = true;
                partnerAttributeRow.Value = email;
                AMainDS.PPartnerAttribute.Rows.Add(partnerAttributeRow);

                partnerAttributeRow = AMainDS.PPartnerAttribute.NewRowTyped();
                partnerAttributeRow.PartnerKey = ANewPartner.PartnerKey;
                partnerAttributeRow.AttributeType = MPartnerConstants.ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD;
                partnerAttributeRow.Index = 9999;
                partnerAttributeRow.Primary = false;
                partnerAttributeRow.Value = MPartnerConstants.ATTR_TYPE_EMAIL;
                partnerAttributeRow.Sequence = SequenceCounter++;
                AMainDS.PPartnerAttribute.Rows.Add(partnerAttributeRow);
            }

            if (phone.Length > 0)
            {
                PPartnerAttributeRow partnerAttributeRow = AMainDS.PPartnerAttribute.NewRowTyped();
                partnerAttributeRow.PartnerKey = ANewPartner.PartnerKey;
                partnerAttributeRow.AttributeType = MPartnerConstants.ATTR_TYPE_PHONE;
                partnerAttributeRow.Index = 0;
                partnerAttributeRow.Primary = true;
                partnerAttributeRow.Value = phone;
                AMainDS.PPartnerAttribute.Rows.Add(partnerAttributeRow);

                partnerAttributeRow = AMainDS.PPartnerAttribute.NewRowTyped();
                partnerAttributeRow.PartnerKey = ANewPartner.PartnerKey;
                partnerAttributeRow.AttributeType = MPartnerConstants.ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD;
                partnerAttributeRow.Index = 9999;
                partnerAttributeRow.Primary = false;
                partnerAttributeRow.Value = MPartnerConstants.ATTR_TYPE_PHONE;
                partnerAttributeRow.Sequence = SequenceCounter++;
                AMainDS.PPartnerAttribute.Rows.Add(partnerAttributeRow);
            }

            if (mobile.Length > 0)
            {
                PPartnerAttributeRow partnerAttributeRow = AMainDS.PPartnerAttribute.NewRowTyped();
                partnerAttributeRow.PartnerKey = ANewPartner.PartnerKey;
                partnerAttributeRow.AttributeType = MPartnerConstants.ATTR_TYPE_MOBILE_PHONE;
                partnerAttributeRow.Index = 0;
                partnerAttributeRow.Primary = true;
                partnerAttributeRow.Value = mobile;
                AMainDS.PPartnerAttribute.Rows.Add(partnerAttributeRow);

                partnerAttributeRow = AMainDS.PPartnerAttribute.NewRowTyped();
                partnerAttributeRow.PartnerKey = ANewPartner.PartnerKey;
                partnerAttributeRow.AttributeType = MPartnerConstants.ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD;
                partnerAttributeRow.Index = 9999;
                partnerAttributeRow.Primary = false;
                partnerAttributeRow.Value = MPartnerConstants.ATTR_TYPE_MOBILE_PHONE;
                partnerAttributeRow.Sequence = SequenceCounter++;
                AMainDS.PPartnerAttribute.Rows.Add(partnerAttributeRow);
            }

            bool HasAddress = (newLocation.StreetName != String.Empty) || (newLocation.City != String.Empty) || (newLocation.CountryCode != String.Empty);
            if (HasAddress)
            {
                if ((newLocation.StreetName == String.Empty) || (newLocation.City == String.Empty) || (newLocation.CountryCode == String.Empty))
                {
                    AddVerificationResult("Address is incomplete, we need streetname, city and country in line " + FCurrentLine.ToString());
                    HasAddress = false;
                }
            }
            bool HasContactDetail = (email != String.Empty) || 
                (phone != String.Empty) ||
                (mobile != String.Empty);
            if (!HasAddress && !HasContactDetail)
            {
                AddVerificationResult("Missing an address (streetname, city, country code) or phone number or email address in line " + FCurrentLine.ToString());
            }

            AMainDS.PPartnerLocation.Rows.Add(partnerlocation);

            TVerificationResultCollection FindVerification;
            // will a new partner be created?
            if (ANewPartner.PartnerKey < 0)
            {
                // do we have a partner with this name and location already?
                Int32 TotalRecords;
                PartnerFindTDSSearchResultTable findPartner = TSimplePartnerFindWebConnector.FindPartners(
                    String.Empty, // PartnerKey
                    String.Empty, // FirstName
                    ANewPartner.PartnerShortName,
                    newLocation.StreetName,
                    newLocation.City,
                    newLocation.PostalCode,
                    String.Empty, // E-Mail Address
                    ANewPartner.PartnerClass,
                    true, // ActiveOnly
                    String.Empty, // SortBy
                    10, // MaxRecords
                    out TotalRecords,
                    out FindVerification);

                if ((TotalRecords == 0) && (email != String.Empty))
                {
                    // what about name and email?
                    findPartner = TSimplePartnerFindWebConnector.FindPartners(
                        String.Empty, // PartnerKey
                        String.Empty, // FirstName
                        ANewPartner.PartnerShortName,
                        String.Empty, // StreetName
                        String.Empty, // City
                        String.Empty, // PostCode
                        email,
                        ANewPartner.PartnerClass,
                        true, // ActiveOnly
                        String.Empty, // SortBy
                        10, // MaxRecords
                        out TotalRecords,
                        out FindVerification);
                }

                if (TotalRecords > 0)
                {
                    AddVerificationResult("Partner " + ANewPartner.PartnerShortName + " in line " + FCurrentLine.ToString() +
                        " already exists with key " + findPartner[0]["p_partner_key_n"].ToString());
                }
            }

            return FLocationKey;
        }

        private bool CreateBankAccounts(DataRow ARow,
            Int64 APartnerKey,
            string AAccountName,
            ref int ABankingDetailsKey,
            String ACSVKey,
            ref PartnerImportExportTDS AMainDS,
            TDBTransaction ATransaction,
            ref TVerificationResultCollection AVerificationResult)
        {
            TVerificationResultCollection VerificationResult = new TVerificationResultCollection();
            bool MainAccount = true;

            for (int Idx = -1; Idx < 6; Idx++)
            {
                String IBAN = GetColumnValue(ARow, ACSVKey + (Idx>=0?Idx.ToString():String.Empty)).Replace(" ", "");

                if (IBAN.Length > 0)
                {
                    // Validate IBAN, and calculate the BIC
                    string BIC;
                    string BankName;
                    if (!TSimplePartnerEditWebConnector.ValidateIBAN(IBAN, out BIC, out BankName, out VerificationResult))
                    {
                        AVerificationResult.Add(VerificationResult[0]);
                        return false;
                    }

                    // TODO: check for existing bank accounts, if this partner already exists in the database
                    // TODO: also check for main account
                    PBankingDetailsRow row = AMainDS.PBankingDetails.NewRowTyped();
                    row.BankingDetailsKey = ABankingDetailsKey;
                    row.BankingType = 0; // BANK ACCOUNT
                    row.AccountName = AAccountName;
                    row.Iban = IBAN;
                    row.BankKey = TSimplePartnerEditWebConnector.FindOrCreateBank(BIC, BankName);
                    AMainDS.PBankingDetails.Rows.Add(row);

                    PPartnerBankingDetailsRow pdrow = AMainDS.PPartnerBankingDetails.NewRowTyped();
                    pdrow.PartnerKey = APartnerKey;
                    pdrow.BankingDetailsKey = ABankingDetailsKey;
                    AMainDS.PPartnerBankingDetails.Rows.Add(pdrow);

                    if (MainAccount)
                    {
                        PBankingDetailsUsageRow newUsageRow = AMainDS.PBankingDetailsUsage.NewRowTyped(true);
                        newUsageRow.PartnerKey = APartnerKey;
                        newUsageRow.BankingDetailsKey = row.BankingDetailsKey;
                        newUsageRow.Type = MPartnerConstants.BANKINGUSAGETYPE_MAIN;
                        AMainDS.PBankingDetailsUsage.Rows.Add(newUsageRow);                        
                    }

                    MainAccount = false;
                    ABankingDetailsKey -= 1;
                }
            }

            return true;
        }

        private void CreateSpecialTypes(DataRow ARow,
            Int64 APartnerKey,
            String ACSVKey,
            ref PartnerImportExportTDS AMainDS,
            TDBTransaction ATransaction)
        {
            for (int Idx = -1; Idx < 6; Idx++)
            {
                String SpecialType = GetColumnValue(ARow, ACSVKey + (Idx>=0?Idx.ToString():String.Empty));

                if (SpecialType.Length > 0)
                {
                    // if partner type already exists for this partner then we don't need to import it again
                    if (!PPartnerTypeAccess.Exists(APartnerKey, SpecialType, ATransaction))
                    {
                        PPartnerTypeRow partnerType = AMainDS.PPartnerType.NewRowTyped(true);
                        partnerType.PartnerKey = APartnerKey;
                        partnerType.TypeCode = SpecialType;
                        AMainDS.PPartnerType.Rows.Add(partnerType);
                    }

                    // validate that this special type exists
                    if (!PTypeAccess.Exists(SpecialType, ATransaction))
                    {
                        AddVerificationResult("invalid category " + SpecialType);
                    }
                }
            }
        }

        private void CreateShortTermApplication(DataRow ARow,
            Int64 APartnerKey,
            string ADateFormat,
            ref PartnerImportExportTDS AMainDS,
            TDBTransaction ATransaction)
        {
            String strEventKey = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_EVENTKEY);
            long EventKey = -1;
            Boolean IsNewApplication = true;

            if (strEventKey.Length > 0)
            {
                try
                {
                    EventKey = long.Parse(strEventKey);
                }
                catch (System.FormatException)
                {
                    AddVerificationResult("Bad number format in EventKey: " + strEventKey);
                }

                if (!PUnitAccess.Exists(EventKey, ATransaction))
                {
                    AddVerificationResult("EventKey not known - application cannot be imported: " + EventKey);
                    return;
                }

                PmGeneralApplicationRow GenAppRow = AMainDS.PmGeneralApplication.NewRowTyped();
                PmShortTermApplicationRow ShortTermRow = AMainDS.PmShortTermApplication.NewRowTyped();


                //TODO: if an application of this person for this event already exists then load that data
                PmShortTermApplicationTable ShortTermTable = null;
                PmGeneralApplicationTable GenAppTable = null;
                PmShortTermApplicationRow TemplateShortTermRow = new PmShortTermApplicationTable().NewRowTyped(false);
                TemplateShortTermRow.PartnerKey = APartnerKey;
                TemplateShortTermRow.StConfirmedOption = EventKey;

                ShortTermTable = PmShortTermApplicationAccess.LoadUsingTemplate(TemplateShortTermRow, ATransaction);

                if (ShortTermTable.Count > 0)
                {
                    IsNewApplication = false;

                    // we have an existing application for this event, now find the general application record for it
                    ShortTermRow.ItemArray = (object[])ShortTermTable.Rows[0].ItemArray.Clone();

                    GenAppTable = PmGeneralApplicationAccess.LoadByPrimaryKey(APartnerKey,
                        ShortTermRow.ApplicationKey,
                        ShortTermRow.RegistrationOffice,
                        ATransaction);

                    if (GenAppTable.Count > 0)
                    {
                        GenAppRow.ItemArray = (object[])GenAppTable.Rows[0].ItemArray.Clone();
                    }
                    else
                    {
                        // this should not happen as there should always be a GenApp row for a ShortTermApp row
                        return;
                    }
                }
                else
                {
                    IsNewApplication = true;

                    // we need to create a new application
                    // initialize GenApp record
                    GenAppRow.PartnerKey = APartnerKey;
                    GenAppRow.ApplicationKey = (int)ATransaction.DataBaseObj.GetNextSequenceValue("seq_application", ATransaction);
                    GenAppRow.OldLink =
                        new TSystemDefaults(ATransaction.DataBaseObj).GetSiteKeyDefault() + ";" + GenAppRow.ApplicationKey.ToString();
                    GenAppRow.RegistrationOffice = DomainManager.GSiteKey; // When this is imported, RegistrationOffice can't be null.

                    // and initialize record for ShortTermApp
                    ShortTermRow.PartnerKey = APartnerKey;
                    ShortTermRow.ApplicationKey = GenAppRow.ApplicationKey;
                    ShortTermRow.RegistrationOffice = GenAppRow.RegistrationOffice; // When this is imported, RegistrationOffice can't be null.
                    ShortTermRow.StBasicOutreachId = GenAppRow.OldLink; //"Unused field"; // This field is scheduled for deletion, but NOT NULL now.
                    ShortTermRow.StAppDate = GenAppRow.GenAppDate;
                    ShortTermRow.StApplicationType = GenAppRow.AppTypeName;
                    ShortTermRow.StConfirmedOption = EventKey;
                }

                AMainDS.PmGeneralApplication.Rows.Add(GenAppRow);
                AMainDS.PmShortTermApplication.Rows.Add(ShortTermRow);
                AddVerificationResult("Application Record Created.", TResultSeverity.Resv_Status);

                if (!IsNewApplication)
                {
                    GenAppRow.AcceptChanges();
                    ShortTermRow.AcceptChanges();
                }

                // Make sure that application date is definitely set. If not in import file then use today's date.
                if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_APPDATE))
                {
                    string appDate = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_APPDATE);

                    if (appDate.Length > 0)
                    {
                        try
                        {
                            GenAppRow.GenAppDate = DateTime.Parse(appDate, StringHelper.GetCultureInfoForDateFormat(ADateFormat));
                        }
                        catch (FormatException)
                        {
                            string fmt = ADateFormat.StartsWith("M", StringComparison.OrdinalIgnoreCase) ? "month-day-year" : "day-month-year";
                            AddVerificationResult(string.Format("Bad general application date: {0} (Expected format: {1})",
                                    appDate,
                                    fmt), TResultSeverity.Resv_Critical);
                        }
                    }
                }

                if (IsNewApplication
                    && GenAppRow.IsGenAppDateNull())
                {
                    GenAppRow.GenAppDate = DateTime.Now;
                }

                ShortTermRow.StAppDate = GenAppRow.GenAppDate;

                if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_APPTYPE))
                {
                    GenAppRow.AppTypeName = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_APPTYPE);
                    ShortTermRow.StApplicationType = GenAppRow.AppTypeName;
                }

                if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_APPSTATUS))
                {
                    GenAppRow.GenApplicationStatus = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_APPSTATUS);
                }

                if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_APPCOMMENTS))
                {
                    GenAppRow.Comment = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_APPCOMMENTS);
                }

                String TimeString = "";

                try
                {
                    TimeString = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_ARRIVALDATE);

                    if (TimeString.Length > 0)
                    {
                        ShortTermRow.Arrival = DateTime.Parse(TimeString, StringHelper.GetCultureInfoForDateFormat(ADateFormat));
                    }

                    TimeString = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_DEPARTUREDATE);

                    if (TimeString.Length > 0)
                    {
                        ShortTermRow.Departure = DateTime.Parse(TimeString, StringHelper.GetCultureInfoForDateFormat(ADateFormat));
                    }
                }
                catch (System.FormatException)
                {
                    string fmt = ADateFormat.StartsWith("M", StringComparison.OrdinalIgnoreCase) ? "month-day-year" : "day-month-year";
                    AddVerificationResult(string.Format("Bad arrival/departure date format in Application: {0} (Expected format: {1})", TimeString,
                            fmt), TResultSeverity.Resv_Critical);
                }

                DateTime TempTime;

                TimeString = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_ARRIVALTIME);

                if (TimeString.Length > 0)
                {
                    try
                    {
                        TempTime = DateTime.Parse(TimeString, StringHelper.GetCultureInfoForDateFormat(ADateFormat));
                        ShortTermRow.ArrivalHour = TempTime.Hour;
                        ShortTermRow.ArrivalMinute = TempTime.Minute;
                    }
                    catch (System.FormatException)
                    {
                        string fmt = ADateFormat.StartsWith("M", StringComparison.OrdinalIgnoreCase) ? "month-day-year" : "day-month-year";
                        AddVerificationResult(string.Format("Bad arrival time format in Application: {0} (Expected format: {1})", TimeString,
                                fmt), TResultSeverity.Resv_Critical);
                    }
                }

                TimeString = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_DEPARTURETIME);

                if (TimeString.Length > 0)
                {
                    try
                    {
                        TempTime = DateTime.Parse(TimeString, StringHelper.GetCultureInfoForDateFormat(ADateFormat));
                        ShortTermRow.DepartureHour = TempTime.Hour;
                        ShortTermRow.DepartureMinute = TempTime.Minute;
                    }
                    catch (System.FormatException)
                    {
                        string fmt = ADateFormat.StartsWith("M", StringComparison.OrdinalIgnoreCase) ? "month-day-year" : "day-month-year";
                        AddVerificationResult(string.Format("Bad departure time format in Application: {0} (Expected format: {1})", TimeString,
                                fmt), TResultSeverity.Resv_Critical);
                    }
                }

                if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_EVENTROLE))
                {
                    ShortTermRow.StCongressCode = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_EVENTROLE);
                }

                String ChargedField = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_CHARGEDFIELD);

                if (ChargedField.Length > 0)
                {
                    try
                    {
                        ShortTermRow.StFieldCharged = long.Parse(ChargedField);
                    }
                    catch
                    {
                        AddVerificationResult("Bad number format in ChargedField: " + ChargedField);
                    }
                }
            }
        }

        private void CreatePassport(DataRow ARow, Int64 APartnerKey, string ADateFormat, ref PartnerImportExportTDS AMainDS,
            TDBTransaction ATransaction)
        {
            string PassportNum = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PASSPORTNUMBER);

            if (PassportNum.Length > 0)
            {
                String DateString = "";
                Boolean IsNewRecord = true;

                PmPassportDetailsTable PassportTable = PmPassportDetailsAccess.LoadByPrimaryKey(APartnerKey, PassportNum, ATransaction);

                PmPassportDetailsRow NewRow = AMainDS.PmPassportDetails.NewRowTyped();

                if (PassportTable.Count > 0)
                {
                    IsNewRecord = false;
                    // we have an existing passport record
                    NewRow.ItemArray = (object[])PassportTable.Rows[0].ItemArray.Clone();
                }
                else
                {
                    IsNewRecord = true;
                    NewRow.PartnerKey = APartnerKey;
                    NewRow.PassportNumber = PassportNum;
                }

                AMainDS.PmPassportDetails.Rows.Add(NewRow);
                AddVerificationResult("Passport Record Created.", TResultSeverity.Resv_Status);

                if (!IsNewRecord)
                {
                    NewRow.AcceptChanges();
                }

                NewRow.FullPassportName = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PASSPORTNAME);
                NewRow.PassportDetailsType = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PASSPORTTYPE);
                NewRow.PlaceOfBirth = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PASSPORTPLACEOFBIRTH);
                NewRow.PassportNationalityCode = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PASSPORTNATIONALITY);
                NewRow.PlaceOfIssue = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PASSPORTPLACEOFISSUE);
                NewRow.CountryOfIssue = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PASSPORTCOUNTRYOFISSUE);

                DateString = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PASSPORTDATEOFISSUE);

                if (DateString.Length > 0)
                {
                    try
                    {
                        NewRow.DateOfIssue = DateTime.Parse(DateString, StringHelper.GetCultureInfoForDateFormat(ADateFormat));
                    }
                    catch (FormatException)
                    {
                        string fmt = ADateFormat.StartsWith("M", StringComparison.OrdinalIgnoreCase) ? "month-day-year" : "day-month-year";
                        AddVerificationResult(string.Format("Bad passport date of issue: {0} (Expected format: {1})", DateString, fmt),
                            TResultSeverity.Resv_Critical);
                    }
                }

                DateString = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_PASSPORTDATEOFEXPIRATION);

                if (DateString.Length > 0)
                {
                    try
                    {
                        NewRow.DateOfExpiration = DateTime.Parse(DateString, StringHelper.GetCultureInfoForDateFormat(ADateFormat));
                    }
                    catch (FormatException)
                    {
                        string fmt = ADateFormat.StartsWith("M", StringComparison.OrdinalIgnoreCase) ? "month-day-year" : "day-month-year";
                        AddVerificationResult(string.Format("Bad passport date of expiry: {0} (Expected format: {1})", DateString, fmt),
                            TResultSeverity.Resv_Critical);
                    }
                }
            }
        }

        private void CreateSpecialNeeds(DataRow ARow, Int64 APartnerKey, ref PartnerImportExportTDS AMainDS,
            TDBTransaction ATransaction)
        {
            // only create special need record if data exists in import file
            if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_VEGETARIAN)
                || HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_MEDICALNEEDS)
                || HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_DIETARYNEEDS)
                || HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_OTHERNEEDS))
            {
                PmSpecialNeedRow NewRow = AMainDS.PmSpecialNeed.NewRowTyped();
                Boolean IsNewRecord = true;

                PmSpecialNeedTable SpecialNeedTable = PmSpecialNeedAccess.LoadByPrimaryKey(APartnerKey, ATransaction);

                if (SpecialNeedTable.Count > 0)
                {
                    IsNewRecord = false;
                    // we have an existing special needs record
                    NewRow.ItemArray = (object[])SpecialNeedTable.Rows[0].ItemArray.Clone();
                }
                else
                {
                    IsNewRecord = true;
                    NewRow.PartnerKey = APartnerKey;
                }

                AMainDS.PmSpecialNeed.Rows.Add(NewRow);
                AddVerificationResult("Special Need Record Created.", TResultSeverity.Resv_Status);

                if (!IsNewRecord)
                {
                    NewRow.AcceptChanges();
                }

                NewRow.MedicalComment = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_MEDICALNEEDS);
                NewRow.DietaryComment = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_DIETARYNEEDS);
                NewRow.OtherSpecialNeed = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_OTHERNEEDS);

                if (GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_VEGETARIAN).ToLower() == "yes")
                {
                    NewRow.VegetarianFlag = true;
                }
                else
                {
                    NewRow.VegetarianFlag = false;
                }
            }
        }

        private void CreateSubscriptions(DataRow ARow, Int64 APartnerKey, ref PartnerImportExportTDS AMainDS,
            TDBTransaction ATransaction)
        {
            int SubsCount = 0;
            String PublicationCode = "";

            foreach (DataColumn c in ARow.Table.Columns)
            {
                if ((c.ColumnName.ToLower().IndexOf("subscribe_") == 0) && (ARow[c.ColumnName].ToString().ToLower() == "yes"))
                {
                    FUnusedColumns.Remove(c.ColumnName);
                    PublicationCode = c.ColumnName.Substring(10).ToUpper();

                    // only add subscription if it does not exist yet
                    //TODOWB: what about existing subscriptions that are cancelled? They would not get updated here.
                    if (!PSubscriptionAccess.Exists(PublicationCode, APartnerKey, ATransaction))
                    {
                        PSubscriptionRow NewRow = AMainDS.PSubscription.NewRowTyped();
                        NewRow.PartnerKey = APartnerKey;
                        NewRow.PublicationCode = PublicationCode;
                        AMainDS.PSubscription.Rows.Add(NewRow);
                        SubsCount++;
                    }
                }
            }

            if (SubsCount > 0)
            {
                AddVerificationResult("Subscriptions Created.", TResultSeverity.Resv_Status);
            }
        }

        private void CreateContacts(DataRow ARow,
            Int64 APartnerKey,
            string ADateFormat,
            ref PartnerImportExportTDS AMainDS,
            string Suffix,
            TDBTransaction ATransaction)
        {
            //TODOWBxxx check for update/create

            string ContactCode = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_CONTACTCODE + Suffix);

            if (ContactCode.Length > 0)
            {
                String DateString = "";

                PartnerImportExportTDSPContactLogRow ContactLogRow = AMainDS.PContactLog.NewRowTyped();
                ContactLogRow.ContactCode = ContactCode;

                DateString = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_CONTACTDATE + Suffix);

                if (DateString.Length > 0)
                {
                    try
                    {
                        ContactLogRow.ContactDate = DateTime.Parse(DateString, StringHelper.GetCultureInfoForDateFormat(ADateFormat));
                    }
                    catch (FormatException)
                    {
                        string fmt = ADateFormat.StartsWith("M", StringComparison.OrdinalIgnoreCase) ? "month-day-year" : "day-month-year";
                        AddVerificationResult(string.Format("Bad contact date: {0} (Expected format: {1})", DateString, fmt),
                            TResultSeverity.Resv_Critical);
                    }
                }

                //DateTime ContactTime = DateTime.Parse(GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_CONTACTTIME + Suffix));
                //ContactLogRow.ContactTime = ((ContactTime.Hour * 60) + ContactTime.Minute * 60) + ContactTime.Second;
                ContactLogRow.Contactor = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_CONTACTOR + Suffix);
                ContactLogRow.ContactComment = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_CONTACTNOTES + Suffix);
                ContactLogRow.ContactAttr = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_CONTACTATTR + Suffix);
                ContactLogRow.ContactDetail = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_CONTACTDETAIL + Suffix);
                AMainDS.PContactLog.Rows.Add(ContactLogRow);
                AddVerificationResult("Contact Log Record Created.", TResultSeverity.Resv_Status);

                var PartnerContactRow = AMainDS.PPartnerContact.NewRowTyped();
                PartnerContactRow.PartnerKey = APartnerKey;
                PartnerContactRow.ContactLogId = ContactLogRow.ContactLogId;
                AMainDS.PPartnerContact.Rows.Add(PartnerContactRow);
                AddVerificationResult("Contact Associated with Partner", TResultSeverity.Resv_Status);
            }
        }

        /// <summary>
        /// Returns the gender of the currently selected partner
        /// </summary>
        /// <returns></returns>
        private string GetGenderCode(DataRow ARow)
        {
            string gender = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_GENDER);

            if ((gender.ToLower() == Catalog.GetString("Female").ToLower()) || (gender.ToLower() == "female"))
            {
                return MPartnerConstants.GENDER_FEMALE;
            }
            else if ((gender.ToLower() == Catalog.GetString("Male").ToLower()) || (gender.ToLower() == "male"))
            {
                return MPartnerConstants.GENDER_MALE;
            }

            return MPartnerConstants.GENDER_UNKNOWN;
        }

        private string GetTitle(DataRow ARow)
        {
            if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_TITLE))
            {
                return GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_TITLE);
            }

            string genderCode = GetGenderCode(ARow);

            if (genderCode == MPartnerConstants.GENDER_MALE)
            {
                return Catalog.GetString("Mr");
            }
            else if (genderCode == MPartnerConstants.GENDER_FEMALE)
            {
                return Catalog.GetString("Ms");
            }

            return "";
        }

        private string GetMaritalStatusCode(DataRow ARow, TDBTransaction ATransaction)
        {
            if (HasColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_MARITALSTATUS))
            {
                string maritalStatus = GetColumnValue(ARow, MPartnerConstants.PARTNERIMPORT_MARITALSTATUS);

                // first look for special cases
                if (maritalStatus.ToLower() == Catalog.GetString("married").ToLower())
                {
                    return MPartnerConstants.MARITALSTATUS_MARRIED;
                }
                else if (maritalStatus.ToLower() == Catalog.GetString("engaged").ToLower())
                {
                    return MPartnerConstants.MARITALSTATUS_ENGAGED;
                }
                else if (maritalStatus.ToLower() == Catalog.GetString("single").ToLower())
                {
                    return MPartnerConstants.MARITALSTATUS_SINGLE;
                }
                else if (maritalStatus.ToLower() == Catalog.GetString("divorced").ToLower())
                {
                    return MPartnerConstants.MARITALSTATUS_DIVORCED;
                }

                // now check for value of marital status in setup table
                if (PtMaritalStatusAccess.Exists(maritalStatus, ATransaction))
                {
                    return maritalStatus;
                }
            }

            return "";
        }

        #region Output CSV Data

        private void CreateOutputData(DataRow ARow,
            Int64 APartnerKey,
            string APartnerClass,
            bool AIsFromFile,
            ref PartnerImportExportTDS AMainDS)
        {
            PartnerImportExportTDSOutputDataRow newRow = AMainDS.OutputData.NewRowTyped();

            newRow.IsFromFile = AIsFromFile;
            newRow.PartnerClass = APartnerClass;
            newRow.ImportStatus = "N";
            newRow.PartnerShortName = string.Empty;
            newRow.ImportID = string.Empty;

            if (APartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
            {
                newRow.OutputFamilyPartnerKey = APartnerKey;
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                newRow.OutputPersonPartnerKey = APartnerKey;
            }

            if (AIsFromFile)
            {
                if (HasColumnValue(ARow, "ImportID"))
                {
                    newRow.ImportID = ARow["ImportID"].ToString();
                }
                else if (HasColumnValue(ARow, "EnrolmentID"))
                {
                    // British spelling
                    newRow.ImportID = ARow["EnrolmentID"].ToString();
                }
                else if (HasColumnValue(ARow, "EnrollmentID"))
                {
                    // American spelling
                    newRow.ImportID = ARow["EnrollmentID"].ToString();
                }

                if (HasColumnValue(ARow, "FirstName") && HasColumnValue(ARow, "FamilyName"))
                {
                    string title = string.Empty;

                    if (HasColumnValue(ARow, "Title"))
                    {
                        title = ARow["Title"].ToString();
                    }

                    newRow.PartnerShortName =
                        Calculations.DeterminePartnerShortName(ARow["FamilyName"].ToString(), title, ARow["FirstName"].ToString());
                }

                long key;

                if (APartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
                {
                    if (HasColumnValue(ARow, "FamilyPartnerKey") && (ARow["FamilyPartnerKey"].ToString().Length > 0))
                    {
                        if (Int64.TryParse(ARow["FamilyPartnerKey"].ToString(), out key))
                        {
                            newRow.InputPartnerKey = key;
                        }
                    }
                }
                else if (APartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                {
                    if (HasColumnValue(ARow, "PersonPartnerKey") && (ARow["PersonPartnerKey"].ToString().Length > 0))
                    {
                        if (Int64.TryParse(ARow["PersonPartnerKey"].ToString(), out key))
                        {
                            newRow.InputPartnerKey = key;
                        }
                    }
                }
            }

            AMainDS.OutputData.Rows.Add(newRow);
        }

        #endregion
    }
}
