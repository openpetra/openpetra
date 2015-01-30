//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Data;
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Server.MCommon.UIConnectors
{
    /// <summary>
    /// Office Specific Data Labels Screen UIConnector
    ///
    /// UIConnector Objects are instantiated by the Client's User Interface via the
    /// Instantiator classes.
    /// They handle requests for data retrieval and saving of data (including data
    /// verification).
    ///
    /// Their role is to
    ///   - retrieve (and possibly aggregate) data using Business Objects,
    ///   - put this data into///one* DataSet that is passed to the Client and make
    ///     sure that no unnessary data is transferred to the Client,
    ///   - optionally provide functionality to retrieve additional, different data
    ///     if requested by the Client (for Client screens that load data initially
    ///     as well as later, eg. when a certain tab on the screen is clicked),
    ///   - save data using Business Objects.
    ///
    /// @Comment These Objects would usually not be instantiated by other Server
    ///          Objects, but only by the Client UI via the Instantiator classes.
    ///          However, Server Objects that derive from these objects and that
    ///          are also UIConnectors are feasible.
    ///
    /// </summary>
    public class TOfficeSpecificDataLabelsUIConnector : IDataElementsUIConnectorsOfficeSpecificDataLabels
    {
        private OfficeSpecificDataLabelsTDS FOfficeSpecificDataLabelsTDS;
        private System.Int64 FPartnerKey;
        private System.Int64 FRegistrationOffice;
        private Int32 FApplicationKey;
        private TOfficeSpecificDataLabelUseEnum FOfficeSpecificDataLabelUse;

        #region TOfficeSpecificDataLabelsUIConnector

        /// <summary>
        /// Constructor.
        ///
        /// </summary>
        /// <param name="APartnerKey">PartnerKey for the Partner to instantiate this object with</param>
        /// <param name="AOfficeSpecificDataLabelUse">Office Specific Data Label Use
        /// </param>
        /// <returns>void</returns>
        public TOfficeSpecificDataLabelsUIConnector(Int64 APartnerKey, TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse)
        {
            TLogging.LogAtLevel(9, "TOfficeSpecificDataLabelsUIConnector created: Instance hash is " + this.GetHashCode().ToString());
            FPartnerKey = APartnerKey;
            FOfficeSpecificDataLabelUse = AOfficeSpecificDataLabelUse;
        }

        /// <summary>
        /// Returns the number of Labels available for a certain LabelUse.
        ///
        /// </summary>
        /// <param name="ALabelUse">LabelUse as stored in p_data_label_use</param>
        /// <param name="AReadTransaction">Transaction for the SELECT COUNT statement</param>
        /// <returns>Number of Labels available for a certain LabelUse.
        /// </returns>
        [NoRemoting]
        public Int32 CountLabelUse(String ALabelUse, TDBTransaction AReadTransaction)
        {
            Int32 ReturnValue;
            PDataLabelUseTable TmpDT;
            PDataLabelUseRow TemplateRow;

            TmpDT = new PDataLabelUseTable();
            TemplateRow = TmpDT.NewRowTyped(false);
            TemplateRow.Use = ALabelUse;
            ReturnValue = PDataLabelUseAccess.CountUsingTemplate(TemplateRow, null, AReadTransaction);
            TLogging.LogAtLevel(10, "TOfficeSpecificDataLabelsUIConnector.CountLabelUse Result: " + ReturnValue.ToString());
            return ReturnValue;
        }

        /// <summary>
        /// Constructor.
        ///
        /// </summary>
        /// <param name="APartnerKey">PartnerKey for the Partner to instantiate this object with</param>
        /// <param name="AApplicationKey">ApplicationKey for the Application to instantiate this
        /// object with</param>
        /// <param name="ARegistrationOffice">RegistrationOffice PartnerKey for the Application
        /// to instantiate this object with</param>
        /// <param name="AOfficeSpecificDataLabelUse">Office Specific Data Label Use
        /// </param>
        /// <returns>void</returns>
        public TOfficeSpecificDataLabelsUIConnector(Int64 APartnerKey,
            Int32 AApplicationKey,
            Int64 ARegistrationOffice,
            TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse)
        {
            TLogging.LogAtLevel(9, "TOfficeSpecificDataLabelsUIConnector created: Instance hash is " + this.GetHashCode().ToString());
            FPartnerKey = APartnerKey;
            FOfficeSpecificDataLabelUse = AOfficeSpecificDataLabelUse;
            FApplicationKey = AApplicationKey;
            FRegistrationOffice = ARegistrationOffice;
        }

        /// <summary>
        /// Check if a data row is obsolete. It does not need to be saved if there is no
        /// value in it.
        ///
        /// </summary>
        /// <returns>void</returns>
        private Boolean IsRowObsolete(PDataLabelTable ADataLabelTable, DataRow ADataRow, Boolean AIsPartnerTable)
        {
            Boolean ReturnValue;
            PDataLabelValuePartnerRow DataLabelValuePartnerRow;
            PDataLabelValueApplicationRow DataLabelValueApplicationRow;
            PDataLabelRow DataLabelRow;

            // initialize Result and variables
            ReturnValue = false;
            DataLabelValuePartnerRow = null;
            DataLabelValueApplicationRow = null;

            if (AIsPartnerTable)
            {
                DataLabelValuePartnerRow = (PDataLabelValuePartnerRow)ADataRow;
                DataLabelRow = (PDataLabelRow)ADataLabelTable.Rows.Find(DataLabelValuePartnerRow.DataLabelKey);
            }
            else
            {
                DataLabelValueApplicationRow = (PDataLabelValueApplicationRow)ADataRow;
                DataLabelRow = (PDataLabelRow)ADataLabelTable.Rows.Find(DataLabelValueApplicationRow.DataLabelKey);
            }

            if (DataLabelRow.DataType == Ict.Petra.Shared.MCommon.MCommonConstants.OFFICESPECIFIC_DATATYPE_CHAR)
            {
                if (DataLabelValuePartnerRow != null)
                {
                    if (DataLabelValuePartnerRow.IsValueCharNull())
                    {
                        ReturnValue = true;
                    }
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    if (DataLabelValueApplicationRow.IsValueCharNull())
                    {
                        ReturnValue = true;
                    }
                }
            }
            else if (DataLabelRow.DataType == Ict.Petra.Shared.MCommon.MCommonConstants.OFFICESPECIFIC_DATATYPE_FLOAT)
            {
                if (DataLabelValuePartnerRow != null)
                {
                    if (DataLabelValuePartnerRow.IsValueNumNull())
                    {
                        ReturnValue = true;
                    }
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    if (DataLabelValueApplicationRow.IsValueNumNull())
                    {
                        ReturnValue = true;
                    }
                }
            }
            else if (DataLabelRow.DataType == Ict.Petra.Shared.MCommon.MCommonConstants.OFFICESPECIFIC_DATATYPE_DATE)
            {
                if (DataLabelValuePartnerRow != null)
                {
                    if (DataLabelValuePartnerRow.IsValueDateNull())
                    {
                        ReturnValue = true;
                    }
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    if (DataLabelValueApplicationRow.IsValueDateNull())
                    {
                        ReturnValue = true;
                    }
                }
            }
            else if (DataLabelRow.DataType == Ict.Petra.Shared.MCommon.MCommonConstants.OFFICESPECIFIC_DATATYPE_INTEGER)
            {
                if (DataLabelValuePartnerRow != null)
                {
                    if (DataLabelValuePartnerRow.IsValueIntNull())
                    {
                        ReturnValue = true;
                    }
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    if (DataLabelValueApplicationRow.IsValueIntNull())
                    {
                        ReturnValue = true;
                    }
                }
            }
            else if (DataLabelRow.DataType == Ict.Petra.Shared.MCommon.MCommonConstants.OFFICESPECIFIC_DATATYPE_CURRENCY)
            {
                if (DataLabelValuePartnerRow != null)
                {
                    if (DataLabelValuePartnerRow.IsValueCurrencyNull())
                    {
                        ReturnValue = true;
                    }
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    if (DataLabelValueApplicationRow.IsValueCurrencyNull())
                    {
                        ReturnValue = true;
                    }
                }
            }
            else if (DataLabelRow.DataType == Ict.Petra.Shared.MCommon.MCommonConstants.OFFICESPECIFIC_DATATYPE_BOOLEAN)
            {
                if (DataLabelValuePartnerRow != null)
                {
                    if (DataLabelValuePartnerRow.IsValueBoolNull())
                    {
                        ReturnValue = true;
                    }
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    if (DataLabelValueApplicationRow.IsValueBoolNull())
                    {
                        ReturnValue = true;
                    }
                }
            }
            else if (DataLabelRow.DataType == Ict.Petra.Shared.MCommon.MCommonConstants.OFFICESPECIFIC_DATATYPE_PARTNERKEY)
            {
                if (DataLabelValuePartnerRow != null)
                {
                    // do not only check for nil but also check if the partner key is 0
                    // in which case the row is obsolete as well
                    if ((DataLabelValuePartnerRow.IsValuePartnerKeyNull()) || (DataLabelValuePartnerRow.ValuePartnerKey == 0))
                    {
                        ReturnValue = true;
                    }
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    // do not only check for nil but also check if the partner key is 0
                    // in which case the row is obsolete as well
                    if ((DataLabelValueApplicationRow.IsValuePartnerKeyNull()) || (DataLabelValueApplicationRow.ValuePartnerKey == 0))
                    {
                        ReturnValue = true;
                    }
                }
            }
            else if (DataLabelRow.DataType == Ict.Petra.Shared.MCommon.MCommonConstants.OFFICESPECIFIC_DATATYPE_LOOKUP)
            {
                if (DataLabelValuePartnerRow != null)
                {
                    if (DataLabelValuePartnerRow.IsValueLookupNull())
                    {
                        ReturnValue = true;
                    }
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    if (DataLabelValueApplicationRow.IsValueLookupNull())
                    {
                        ReturnValue = true;
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Passes data as a Typed DataSet to the Screen, containing multiple DataTables.
        /// </summary>
        /// <returns></returns>
        [NoRemoting]
        public OfficeSpecificDataLabelsTDS GetData()
        {
            PDataLabelUseTable DataLabelUseDT;
            String LabelUse;
            Int32 Counter;
            PDataLabelValuePartnerRow DataLabelValueRow;
            PDataLabelUseRow DataLabelUseRow;

            // create the FOfficeSpecificDataLabelsTDS DataSet that will later be passed to the Client
            FOfficeSpecificDataLabelsTDS = new OfficeSpecificDataLabelsTDS("OfficeSpecificDataLabels");

            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    switch (FOfficeSpecificDataLabelUse)
                    {
                        case TOfficeSpecificDataLabelUseEnum.Family:
                        case TOfficeSpecificDataLabelUseEnum.Church:
                        case TOfficeSpecificDataLabelUseEnum.Organisation:
                        case TOfficeSpecificDataLabelUseEnum.Unit:
                        case TOfficeSpecificDataLabelUseEnum.Bank:
                        case TOfficeSpecificDataLabelUseEnum.Venue:
                            PDataLabelValuePartnerAccess.LoadViaPPartnerPartnerKey(FOfficeSpecificDataLabelsTDS, FPartnerKey, ReadTransaction);
                            break;

                        case TOfficeSpecificDataLabelUseEnum.Person:
                        case TOfficeSpecificDataLabelUseEnum.Personnel:
                            PDataLabelValuePartnerAccess.LoadViaPPartnerPartnerKey(FOfficeSpecificDataLabelsTDS, FPartnerKey, ReadTransaction);

                            if (FOfficeSpecificDataLabelsTDS.PDataLabelValuePartner.Rows.Count > 0)
                            {
                                DataLabelUseDT = PDataLabelUseAccess.LoadAll(ReadTransaction);

                                // Initialize 'use' criterium according to the enum
                                LabelUse = Enum.GetName(typeof(TOfficeSpecificDataLabelUseEnum), FOfficeSpecificDataLabelUse);

                                // for every value row: check if the label exists in the requested 'use' section
                                // loop counts down, otherwise access violations when elements are deleted
                                for (Counter = FOfficeSpecificDataLabelsTDS.PDataLabelValuePartner.Rows.Count - 1; Counter >= 0; Counter--)
                                {
                                    DataLabelValueRow = FOfficeSpecificDataLabelsTDS.PDataLabelValuePartner[Counter];
                                    DataLabelUseRow =
                                        (PDataLabelUseRow)DataLabelUseDT.Rows.Find(new Object[] { DataLabelValueRow.DataLabelKey, LabelUse });

                                    if (DataLabelUseRow == null)
                                    {
                                        DataLabelValueRow.Delete();
                                        DataLabelValueRow.AcceptChanges();
                                    }
                                }
                            }

                            break;

                        case TOfficeSpecificDataLabelUseEnum.LongTermApp:
                        case TOfficeSpecificDataLabelUseEnum.ShortTermApp:
                            PDataLabelValueApplicationAccess.LoadViaPmGeneralApplication(FOfficeSpecificDataLabelsTDS,
                            FPartnerKey,
                            FApplicationKey,
                            FRegistrationOffice,
                            ReadTransaction);
                            break;
                    }
                });

            return FOfficeSpecificDataLabelsTDS;
        }

        /// <summary>
        /// Saves data from the Office Specific Data Label Edit Screen (contained in a DataTable).
        ///
        /// The DataTable is inspected for added, changed or
        /// deleted rows by submitting them to the Business Objects that relate to them.
        /// The Business Objects check the DataRow(s) that belong to them for validity
        /// before saving the data by calling Stored Procedures for inserting, updating
        /// or deleting data are called.
        ///
        /// </summary>
        /// <param name="AInspectDT">DataTable that needs to be submitted</param>
        /// <param name="AReadTransaction">Current Transaction</param>
        /// <returns>true if all verifications are OK and all DB calls succeeded, false if
        /// any verification or DB call failed
        /// </returns>
        [NoRemoting]
        public TSubmitChangesResult PrepareChangesServerSide(
            DataTable AInspectDT,
            TDBTransaction AReadTransaction)
        {
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrOK;

            // TODO: once we have centrally cached data tables on the server then get the data
            // from there. Until then just load it on the spot here!
            PDataLabelTable DataLabelDT = PDataLabelAccess.LoadAll(AReadTransaction);

            if (AInspectDT != null)
            {
                // Run through all rows of the value table and see if the significant column is empty/null. If so
                // then delete the row from the table (these rows are not needed any longer in order to save space
                // in the database)
                int NumRows = AInspectDT.Rows.Count;

                for (int RowIndex = NumRows - 1; RowIndex >= 0; RowIndex -= 1)
                {
                    DataRow InspectedDataRow = AInspectDT.Rows[RowIndex];

                    // only check modified or added rows because the deleted ones are deleted anyway
                    if ((InspectedDataRow.RowState == DataRowState.Modified) || (InspectedDataRow.RowState == DataRowState.Added))
                    {
                        if (IsRowObsolete(DataLabelDT, InspectedDataRow, (AInspectDT.TableName == PDataLabelValuePartnerTable.GetTableName())))
                        {
                            InspectedDataRow.Delete();
                        }
                    }
                }
            }
            else
            {
                TLogging.LogAtLevel(8, "AInspectDS = null!");
                SubmissionResult = TSubmitChangesResult.scrNothingToBeSaved;
            }

            return SubmissionResult;
        }

        /// <summary>
        /// Get short name for a partner. Boolean returns result if partner was found.
        ///
        /// @Comment Calling this function should soon be no longer necessary since it
        /// can be replaced with a call to Ict.Petra.Client.App.Core.ServerLookups!
        ///
        /// </summary>
        /// <param name="APartnerKey">PartnerKey to find the short name for</param>
        /// <param name="APartnerShortName">short name found for partner key</param>
        /// <param name="APartnerClass">Partner Class for partner that was found</param>
        /// <returns>true if Partner was found in DB, otherwise false
        /// </returns>
        public Boolean GetPartnerShortName(Int64 APartnerKey, out String APartnerShortName, out TPartnerClass APartnerClass)
        {
            TStdPartnerStatusCode PartnerStatus;

            return MCommonMain.RetrievePartnerShortName(APartnerKey, out APartnerShortName, out APartnerClass, out PartnerStatus);
        }

        /// <summary>
        /// get the local partner data
        /// </summary>
        [NoRemoting]
        public PDataLabelValuePartnerTable GetDataLocalPartnerDataValues(Int64 APartnerKey, out Boolean ALabelsAvailable,
            Boolean ACountOnly, TDBTransaction AReadTransaction)
        {
            PDataLabelValuePartnerTable DataLabelValuePartnerDT;
            OfficeSpecificDataLabelsTDS OfficeSpecificDataLabels;

            ALabelsAvailable = false;

            DataLabelValuePartnerDT = new PDataLabelValuePartnerTable();

            try
            {
                OfficeSpecificDataLabels = GetData();
                //ALabelsAvailable =
                //  (CountLabelUse(SharedTypes.PartnerClassEnumToString(FPartnerClass), AReadTransaction) != 0);

                if (!ACountOnly)
                {
                    DataLabelValuePartnerDT.Merge(OfficeSpecificDataLabels.PDataLabelValuePartner);
                }
            }
            catch (Exception)
            {
                // TODO: Exception processing
            }

            return DataLabelValuePartnerDT;
        }

        #endregion
    }
}