//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2016 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.DB.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.Extracts;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner.Partner.WebConnectors;

namespace Ict.Petra.Server.MPartner.ImportExport.WebConnectors
{
    /// <summary>
    /// Import and export partner tax authority data
    /// </summary>
    public class TImportExportTaxWebConnector
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool ImportPartnerTaxCodes(
            Hashtable AParameters,
            String AImportString,
            out TVerificationResultCollection AErrorMessages,
            out List <string>AOutputLines,
            out bool AExtractCreated,
            out int AExtractId,
            out int AExtractKeyCount,
            out int AImportedCodeCount,
            out int ADeletedCodeCount,
            out int ATaxCodeMismatchCount)
        {
            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Importing Partner Tax Codes"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Initialising"),
                5);

            TVerificationResultCollection errorMessages = new TVerificationResultCollection();
            List <string>outputLines = new List <string>();
            List <long>partnerTaxCodesImported = new List <long>();
            List <long>partnerTaxCodesDeleted = new List <long>();
            int taxCodeMismatchCount = 0;
            bool extractCreated = false;
            int extractId = -1;
            int extractKeyCount = 0;

            string delimiter = Convert.ToString(AParameters["Delimiter"]);
            bool firstRowIsHeader = Convert.ToBoolean(AParameters["FirstRowIsHeader"]);
            bool failIfNotPerson = Convert.ToBoolean(AParameters["FailIfNotPerson"]);
            bool failIfInvalidPartner = Convert.ToBoolean(AParameters["FailIfInvalidPartner"]);
            bool overwriteExistingTaxCode = Convert.ToBoolean(AParameters["OverwriteExistingTaxCode"]);
            bool createExtract = Convert.ToBoolean(AParameters["CreateExtract"]);
            bool createOutputFile = Convert.ToBoolean(AParameters["CreateOutFile"]);
            bool includePartnerDetails = Convert.ToBoolean(AParameters["IncludePartnerDetails"]);

            int emptyTaxCodeAction = Convert.ToInt32(AParameters["EmptyTaxCode"]);

            string taxCodeType = Convert.ToString(AParameters["TaxCodeType"]);
            string extractName = Convert.ToString(AParameters["ExtractName"]);
            string extractDescription = Convert.ToString(AParameters["ExtractDescription"]);

            StringReader sr = new StringReader(AImportString);
            int rowNumber = 0;
            bool cancelledByUser = false;
            bool doneHeaderRow = (firstRowIsHeader == false);

            TDBTransaction transaction = null;
            bool submissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, 3, ref transaction, ref submissionOK,
                delegate
                {
                    try
                    {
                        PTaxTable taxTable = PTaxAccess.LoadAll(transaction);

                        string importLine = sr.ReadLine();
                        int percentDone = 10;
                        int previousPercentDone = 0;

                        int initialTextLength = AImportString.Length;
                        int totalRows = AImportString.Split('\n').Length;

                        if (delimiter.Length < 1)
                        {
                            // should not happen
                            delimiter = ",";
                        }

                        while (importLine != null)
                        {
                            rowNumber++;
                            percentDone = 10 + ((rowNumber * 80) / totalRows);

                            // skip empty lines and commented lines
                            if ((importLine.Trim().Length == 0) || importLine.StartsWith("/*") || importLine.StartsWith("#"))
                            {
                                importLine = sr.ReadLine();
                                continue;
                            }

                            int numberOfElements = StringHelper.GetCSVList(importLine, delimiter).Count;

                            if (numberOfElements < 2)
                            {
                                // That is a critical error
                                TVerificationResult errorMsg =
                                    new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, rowNumber),
                                        Catalog.GetString("Wrong number of columns.  There must be at least 2"), TResultSeverity.Resv_Critical);
                                errorMessages.Add(errorMsg);
                                outputLines.Add(string.Format("\"Wrong number of columns in Line {0}\"", rowNumber));
                                importLine = sr.ReadLine();
                                continue;
                            }

                            if (!doneHeaderRow)
                            {
                                // Skip a header row
                                doneHeaderRow = true;
                                importLine = sr.ReadLine();
                                continue;
                            }

                            int preParseMessageCount = errorMessages.Count;
                            importLine = importLine.TrimStart(' ');     // TCommonImport.ImportInt64 does not remove leading spaces before optional quotes
                            Int64 partnerKey = TCommonImport.ImportInt64(ref importLine, delimiter, null, null, rowNumber, errorMessages, null);
                            importLine = importLine.TrimStart(' ');     // TCommonImport.ImportString does not remove leading spaces before optional quotes
                            string taxCode = TCommonImport.ImportString(ref importLine, delimiter, null, null, rowNumber, errorMessages, null, false);

                            if (errorMessages.Count > preParseMessageCount)
                            {
                                outputLines.Add(string.Format("\"Error reading Line {0}\"", rowNumber));
                                importLine = sr.ReadLine();
                                continue;
                            }

                            // Ok so we have a good key and a code, which might be null
                            // See if the partner exists
                            PartnerFindTDS partner = TSimplePartnerFindWebConnector.FindPartners(partnerKey, true);
                            PartnerFindTDSSearchResultTable result = partner.SearchResult;

                            if (result.DefaultView.Count == 0)
                            {
                                // We don't have the partner
                                if (failIfInvalidPartner)
                                {
                                    TVerificationResult errorMsg =
                                        new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, rowNumber),
                                            Catalog.GetString("Could not find the specified partner"), TResultSeverity.Resv_Critical);
                                    errorMessages.Add(errorMsg);
                                    outputLines.Add(string.Format("\"Error\"{0}{1}{0}{0}{0}{0}\"Partner not found\"", delimiter, partnerKey));
                                }
                                else
                                {
                                    outputLines.Add(string.Format("\"Skipped\"{0}{1}{0}{0}{0}{0}\"Partner not found\"", delimiter, partnerKey));
                                }

                                importLine = sr.ReadLine();
                                continue;
                            }

                            // We already have this partner
                            PartnerFindTDSSearchResultRow row = (PartnerFindTDSSearchResultRow)result.DefaultView[0].Row;

                            string partnerClass = row.PartnerClass;
                            string partnerDetails = string.Empty;

                            if (includePartnerDetails)
                            {
                                string shortName = row.PartnerShortName;
                                string bestAddress = string.Empty;

                                PLocationTable locationTable;
                                string country;

                                if (TAddressTools.GetBestAddress(partnerKey, out locationTable, out country, transaction))
                                {
                                    if ((locationTable != null) && (locationTable.Rows.Count > 0))
                                    {
                                        bestAddress =
                                            Calculations.DetermineLocationString((PLocationRow)locationTable.Rows[0],
                                                Calculations.TPartnerLocationFormatEnum.plfCommaSeparated);
                                    }
                                }

                                partnerDetails = string.Format("{0}\"{1}\"{0}\"{2}\"", delimiter, shortName, bestAddress);
                            }

                            if (partnerClass != "PERSON")
                            {
                                if (failIfNotPerson)
                                {
                                    TVerificationResult errorMsg =
                                        new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, rowNumber),
                                            Catalog.GetString("Partner is not a PERSON"), TResultSeverity.Resv_Critical);
                                    errorMessages.Add(errorMsg);
                                    outputLines.Add(string.Format("\"Error\"{0}{1}{0}\"{2}\"{3}{0}\"{4}\"", delimiter,
                                            partnerKey, taxCode, partnerDetails, "Partner is not a PERSON"));
                                }
                                else
                                {
                                    outputLines.Add(string.Format("\"Skipped\"{0}{1}{0}\"{2}\"{3}{0}\"{4}\"", delimiter,
                                            partnerKey, taxCode, partnerDetails, "Partner is not a PERSON"));
                                }

                                importLine = sr.ReadLine();
                                continue;
                            }

                            bool deleteExistingRow = false;

                            if (taxCode == null)
                            {
                                // what to do if the code is not supplied?
                                if ((emptyTaxCodeAction == 0) || (emptyTaxCodeAction == 1))
                                {
                                    if (emptyTaxCodeAction == 0)
                                    {
                                        // Fail
                                        TVerificationResult errorMsg =
                                            new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, rowNumber),
                                                Catalog.GetString("No tax code specified"), TResultSeverity.Resv_Critical);
                                        errorMessages.Add(errorMsg);
                                        outputLines.Add(string.Format("\"Error\"{0}{1}{0}{2}{0}\"{3}\"", delimiter,
                                                partnerKey, partnerDetails, "No tax code specified"));
                                    }
                                    else
                                    {
                                        outputLines.Add(string.Format("\"Skipped\"{0}{1}{0}{2}{0}\"{3}\"", delimiter,
                                                partnerKey, partnerDetails, "No tax code specified"));
                                    }

                                    importLine = sr.ReadLine();
                                    continue;
                                }
                                else if (emptyTaxCodeAction == 2)
                                {
                                    deleteExistingRow = true;
                                }
                                else
                                {
                                    throw new Exception("Developer error! Unknown action for an empty tax code...");
                                }
                            }

                            // See if there is a tax code already for this partner
                            PTaxRow taxRow = null;
                            taxTable.DefaultView.RowFilter = string.Format("{0}={1} AND {2}='{3}'",
                                PTaxTable.GetPartnerKeyDBName(), partnerKey, PTaxTable.GetTaxTypeDBName(), taxCodeType);

                            if (taxTable.DefaultView.Count == 0)
                            {
                                if (deleteExistingRow)
                                {
                                    // The tax code is null and we can only delete if the row already exists in the tax table
                                    outputLines.Add(string.Format("\"Skipped\"{0}{1}{0}\"{2}\"{3}{0}\"{4}\"",
                                            delimiter, partnerKey, taxCode, partnerDetails, "Cannot delete. The partner is not in the tax table."));
                                }
                                else
                                {
                                    // Add a new row
                                    taxRow = taxTable.NewRowTyped(true);
                                    taxRow.PartnerKey = partnerKey;
                                    taxRow.TaxType = taxCodeType;
                                    taxRow.TaxRef = taxCode;
                                    taxTable.Rows.Add(taxRow);
                                    outputLines.Add(string.Format("\"Added\"{0}{1}{0}\"{2}\"{3}",
                                            delimiter, partnerKey, taxCode, partnerDetails));
                                    partnerTaxCodesImported.Add(partnerKey);
                                }
                            }
                            else
                            {
                                // the row exists already.  We expect there to only be one row per tax code type for a given partner
                                // If partners can have multiple codes of a specified type the code below will need to change!
                                taxRow = (PTaxRow)taxTable.DefaultView[0].Row;
                                string currentCode = taxRow.TaxRef;

                                if (deleteExistingRow)
                                {
                                    if ((taxRow.RowState != DataRowState.Detached) && (taxRow.RowState != DataRowState.Deleted))
                                    {
                                        taxRow.Delete();
                                    }

                                    outputLines.Add(string.Format("\"Deleted\"{0}{1}{0}\"{2}\"{3}",
                                            delimiter, partnerKey, currentCode, partnerDetails));
                                    partnerTaxCodesDeleted.Add(partnerKey);
                                }
                                else
                                {
                                    // Check if it is the same
                                    if (string.Compare(currentCode, taxCode, false) == 0)
                                    {
                                        // they are the same
                                        outputLines.Add(string.Format("\"Unchanged\"{0}{1}{0}\"{2}\"{3}",
                                                delimiter, partnerKey, taxCode, partnerDetails));
                                        partnerTaxCodesImported.Add(partnerKey);        // this counts as an import
                                    }
                                    else
                                    {
                                        // They are different
                                        if (overwriteExistingTaxCode)
                                        {
                                            // Overwrite the old value
                                            taxRow.TaxRef = taxCode;
                                            outputLines.Add(string.Format("\"Modified\"{0}{1}{0}\"{2}\"{3}{0}\"{4}\"",
                                                    delimiter, partnerKey, taxCode, partnerDetails,
                                                    "Previous code was: " + currentCode));
                                            partnerTaxCodesImported.Add(partnerKey);
                                        }
                                        else
                                        {
                                            // Do nothing on this row
                                            outputLines.Add(string.Format("\"Skipped\"{0}{1}{0}\"{2}\"{3}{0}\"{4}\"",
                                                    delimiter, partnerKey, currentCode, partnerDetails,
                                                    "WARNING!! File and Database codes differ. New code is: " + taxCode));
                                            taxCodeMismatchCount++;
                                        }
                                    }
                                }

                                if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                                {
                                    cancelledByUser = true;
                                    break;
                                }

                                if (errorMessages.HasCriticalErrors && (errorMessages.Count > 100))
                                {
                                    // This probably means that it is a big file and the user has made the same mistake many times over
                                    break;
                                }

                                // Update progress tracker every few percent
                                if ((percentDone - previousPercentDone) > 3)
                                {
                                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                        String.Format(Catalog.GetString("Importing row {0}"), rowNumber),
                                        (percentDone > 90) ? 90 : percentDone);
                                    previousPercentDone = percentDone;
                                }
                            }

                            importLine = sr.ReadLine();
                        }

                        // No more lines in the file - or cancelled by the user
                        if (cancelledByUser)
                        {
                            errorMessages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    "The import was cancelled by the user.", TResultSeverity.Resv_Critical));
                            outputLines.Add("\"Import cancelled by user\"");
                        }

                        if (errorMessages.HasCriticalErrors)
                        {
                            submissionOK = false;
                            partnerTaxCodesImported.Clear();
                        }
                        else
                        {
                            PTaxAccess.SubmitChanges(taxTable, transaction);
                            submissionOK = true;
                        }

                        // Now create the optional extract
                        if (submissionOK && (partnerTaxCodesImported.Count > 0) && createExtract)
                        {
                            // we have more server work to do so we reset submission OK to false
                            submissionOK = false;

                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Creating new Extract ..."),
                                97);

                            DataTable table = new DataTable();
                            table.Columns.Add(new DataColumn());

                            for (int i = 0; i < partnerTaxCodesImported.Count; i++)
                            {
                                DataRow row = table.NewRow();
                                row[0] = partnerTaxCodesImported[i];
                                table.Rows.Add(row);
                            }

                            List <long>ignoredKeys;

                            extractCreated = TExtractsHandling.CreateExtractFromListOfPartnerKeys(extractName,
                                extractDescription,
                                out extractId,
                                table,
                                0,
                                false,
                                out extractKeyCount,
                                out ignoredKeys);

                            submissionOK = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                            Catalog.GetString("Exception Occurred"),
                            0);

                        if (TDBExceptionHelper.IsTransactionSerialisationException(ex))
                        {
                            errorMessages.Add(new TVerificationResult("ImportPartnerTaxCodes",
                                    ErrorCodeInventory.RetrieveErrCodeInfo(PetraErrorCodes.ERR_DB_SERIALIZATION_EXCEPTION)));
                        }
                        else
                        {
                            errorMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine, rowNumber),
                                    ex.Message, TResultSeverity.Resv_Critical));
                        }

                        partnerTaxCodesImported.Clear();
                    }
                    finally
                    {
                        TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                    }
                });

            AErrorMessages = errorMessages;
            AOutputLines = outputLines;
            AExtractCreated = extractCreated;
            AExtractId = extractId;
            AExtractKeyCount = extractKeyCount;
            AImportedCodeCount = partnerTaxCodesImported.Count;
            ADeletedCodeCount = partnerTaxCodesDeleted.Count;
            ATaxCodeMismatchCount = taxCodeMismatchCount;

            return submissionOK;
        }
    }
}
