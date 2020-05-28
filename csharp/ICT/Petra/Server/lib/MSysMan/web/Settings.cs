//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb, timop
//
// Copyright 2004-2020 by OM International
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MFinance.GL.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MSysMan.Validation;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MSysMan.Maintenance.WebConnectors;
using Ict.Petra.Server.MPartner.Partner.Cacheable.WebConnectors;
using Ict.Petra.Server.MFinance.Setup.WebConnectors;

namespace Ict.Petra.Server.MSysMan.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MCommon DataReader sub-namespace.
    ///
    /// </summary>
    public class TSettingsWebConnector
    {
        /// <summary>
        /// Return partner table which contains all available sites usable for this system.
        /// Records will first be sorted by info if they exist in p_partner_ledger and then by name.
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("SYSMAN")]
        public static DataTable GetAvailableSites()
        {
            TDBTransaction ReadTransaction = new TDBTransaction();

            DataTable SitesTable = new DataTable();
            DataTable UnusedSitesTable = new DataTable();
            DataTable UsedSitesTable = new DataTable();
            DataRow SitesRow;
            string IsPartnerLedger = SharedConstants.SYSMAN_AVAILABLE_SITES_COLUMN_IS_PARTNER_LEDGER;
            string SiteKey = PUnitTable.GetPartnerKeyDBName();
            string SiteShortName = PUnitTable.GetUnitNameDBName();
            Int64 PartnerKey;

            SitesTable.Columns.Add(new DataColumn(IsPartnerLedger, typeof(bool)));
            SitesTable.Columns.Add(new DataColumn(SiteKey, typeof(Int64)));
            SitesTable.Columns.Add(new DataColumn(SiteShortName, typeof(string)));

            UnusedSitesTable.Columns.Add(new DataColumn(IsPartnerLedger, typeof(bool)));
            UnusedSitesTable.Columns.Add(new DataColumn(SiteKey, typeof(Int64)));
            UnusedSitesTable.Columns.Add(new DataColumn(SiteShortName, typeof(string)));

            UsedSitesTable.Columns.Add(new DataColumn(IsPartnerLedger, typeof(bool)));
            UsedSitesTable.Columns.Add(new DataColumn(SiteKey, typeof(Int64)));
            UsedSitesTable.Columns.Add(new DataColumn(SiteShortName, typeof(string)));

            DBAccess.ReadTransaction(
                ref ReadTransaction,
                delegate
                {
                    try
                    {
                        // Load data
                        string SqlStmt = "SELECT pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetPartnerKeyDBName() +
                                         ", pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetUnitNameDBName() +
                                         " FROM " + PUnitTable.GetTableDBName() + ", " + PPartnerTable.GetTableDBName() +
                                         " WHERE ((" + PUnitTable.GetUnitTypeCodeDBName() + " = 'F')" +
                                         "        OR(" + PUnitTable.GetUnitTypeCodeDBName() + " = 'A'))" +
                                         " AND pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetPartnerKeyDBName() +
                                         " = pub_" + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName() +
                                         " AND " + PPartnerTable.GetStatusCodeDBName() + " = 'ACTIVE'";

                        // sort rows according to name
                        SqlStmt = SqlStmt + " ORDER BY " + PUnitTable.GetUnitNameDBName();

                        DataTable sites = ReadTransaction.DataBaseObj.SelectDT(SqlStmt, "fields", ReadTransaction);

                        foreach (DataRow tempSiteRow in sites.Rows)
                        {
                            PartnerKey = Convert.ToInt64(tempSiteRow[0]);

                            // check if a site is already used in table p_partner_ledger
                            if (PPartnerLedgerAccess.CountViaPUnit(PartnerKey, ReadTransaction) > 0)
                            {
                                SitesRow = UsedSitesTable.NewRow();
                                SitesRow[IsPartnerLedger] = true;
                                SitesRow[SiteKey] = PartnerKey;
                                SitesRow[SiteShortName] = Convert.ToString(tempSiteRow[1]);
                                UsedSitesTable.Rows.Add(SitesRow);
                            }
                            else
                            {
                                SitesRow = UnusedSitesTable.NewRow();
                                SitesRow[IsPartnerLedger] = false;
                                SitesRow[SiteKey] = PartnerKey;
                                SitesRow[SiteShortName] = Convert.ToString(tempSiteRow[1]);
                                UnusedSitesTable.Rows.Add(SitesRow);
                            }
                        }

                        // first add used sites to table
                        foreach (DataRow tempSiteRow in UsedSitesTable.Rows)
                        {
                            SitesRow = SitesTable.NewRow();
                            SitesRow[IsPartnerLedger] = tempSiteRow[IsPartnerLedger];
                            SitesRow[SiteKey] = tempSiteRow[SiteKey];
                            SitesRow[SiteShortName] = tempSiteRow[SiteShortName];
                            SitesTable.Rows.Add(SitesRow);
                        }

                        // and now add unused sites to table
                        foreach (DataRow tempSiteRow in UnusedSitesTable.Rows)
                        {
                            SitesRow = SitesTable.NewRow();
                            SitesRow[IsPartnerLedger] = tempSiteRow[IsPartnerLedger];
                            SitesRow[SiteKey] = tempSiteRow[SiteKey];
                            SitesRow[SiteShortName] = tempSiteRow[SiteShortName];
                            SitesTable.Rows.Add(SitesRow);
                        }
                    }
                    catch (Exception e)
                    {
                        TLogging.Log(e.ToString());
                    }
                });

            return SitesTable;
        }

        /// <summary>
        /// save site keys set up for use
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("SYSMAN")]
        public static bool SaveSiteKeys(List <Int64>ASiteKeysSetUpForUse, List <Int64>ASiteKeysToRemove)
        {
            TDBTransaction Transaction = new TDBTransaction();
            bool SubmissionOK = true;
            PPartnerLedgerTable PartnerLedgerTable = new PPartnerLedgerTable();
            PPartnerLedgerRow PartnerLedgerRow;

            // save site keys that can be used in p_partner_ledger

            DBAccess.WriteTransaction(
                ref Transaction, ref SubmissionOK,
                delegate
                {
                    // create new records in p_partner_ledger if not there yet
                    foreach (Int64 SiteKey in ASiteKeysSetUpForUse)
                    {
                        if (PPartnerLedgerAccess.CountViaPUnit(SiteKey, Transaction) == 0)
                        {
                            PartnerLedgerRow = PartnerLedgerTable.NewRowTyped();
                            PartnerLedgerRow.PartnerKey = SiteKey;

                            // calculate last partner id, from older uses of this ledger number
                            object MaxExistingPartnerKeyObj = Transaction.DataBaseObj.ExecuteScalar(
                                String.Format("SELECT MAX(" + PPartnerTable.GetPartnerKeyDBName() + ") FROM " + PPartnerTable.GetTableDBName() +
                                    " WHERE " + PPartnerTable.GetPartnerKeyDBName() + " > {0} AND " + PPartnerTable.GetPartnerKeyDBName() +
                                    " < {1}",
                                    SiteKey,
                                    SiteKey + 500000), Transaction);

                            if (MaxExistingPartnerKeyObj.GetType() != typeof(DBNull))
                            {
                                // found a partner key for this site already: set it to last used value
                                PartnerLedgerRow.LastPartnerId = Convert.ToInt32(Convert.ToInt64(MaxExistingPartnerKeyObj) - SiteKey);
                            }
                            else
                            {
                                // in this case there was no partner key for this site yet
                                PartnerLedgerRow.LastPartnerId = 0;
                            }

                            PartnerLedgerTable.Rows.Add(PartnerLedgerRow);
                        }
                    }

                    // delete records from p_partner_ledger that are no longer needed
                    foreach (Int64 SiteKey in ASiteKeysToRemove)
                    {
                        PPartnerLedgerAccess.DeleteByPrimaryKey(SiteKey, Transaction);
                    }

                    PPartnerLedgerAccess.SubmitChanges(PartnerLedgerTable, Transaction);
                });

            // make sure SitesList will be refreshed when called next time
            TPartnerCacheableWebConnector.RefreshCacheableTable(TCacheablePartnerTablesEnum.InstalledSitesList);

            return SubmissionOK;
        }

        /// if this system does not have a user that does have finance permissions,
        /// then redirect to the SysMan Setup Assistant
        [RequireModulePermission("SYSMAN")]
        public static string GetSetupAssistant()
        {
            TDBTransaction t = new TDBTransaction();
            TDataBase db = DBAccess.Connect("GetSetupAssistant");

            string result = String.Empty;
            string sql = "SELECT COUNT(*) FROM PUB_s_user_module_access_permission p1 " +
                "WHERE p1.s_module_id_c = 'FINANCE-3' AND p1.s_can_access_l = true";

            db.ReadTransaction(ref t,
                delegate
                {
                    if (Convert.ToInt32(db.ExecuteScalar(sql, t)) == 0)
                    {
                        result = "SystemManager/SysManAssistantInit";
                    }
                });

            db.CloseDBConnection();

            return result;
        }

        /// get suggested values for the first setup
        [RequireModulePermission("SYSMAN")]
        public static bool GetDefaultsForFirstSetup(
            string AClientLanguage,
            out string AUserID,
            out string AFirstName,
            out string ALastName,
            out string ALanguageCode,
            out string AEmailAddress,
            out string AInitialModulePermissions,
            out string AInitialPassword,
            out Int64 ASiteKey
            )
        {
            AUserID = AFirstName = ALastName = ALanguageCode = AEmailAddress = AInitialModulePermissions = AInitialPassword = String.Empty;
            ASiteKey = -1;

            TDBTransaction t = new TDBTransaction();
            TDataBase db = DBAccess.Connect("GetDefaultsForFirstSetup");

            string sql = "SELECT * FROM PUB_s_user " +
                "WHERE s_user_id_c = 'SYSADMIN'";

            SUserTable usertable = new SUserTable();

            db.ReadTransaction(ref t,
                delegate
                {
                    db.SelectDT(usertable, sql, t);

                });

            db.CloseDBConnection();

            if (usertable.Rows.Count == 1)
            {
                AUserID = usertable[0].FirstName.Replace(" ","").Replace("-","").ToUpper();
                AFirstName = usertable[0].FirstName;
                ALastName = usertable[0].LastName;
                ALanguageCode = usertable[0].LanguageCode;
                AEmailAddress = usertable[0].EmailAddress;
                if (AEmailAddress.Contains("+sysadmin@"))
                {
                    AEmailAddress = AEmailAddress.Replace("+sysadmin@", "@");
                }
                else
                {
                    AEmailAddress = AEmailAddress.Replace("@", "+openpetra@");
                }
                AInitialModulePermissions = "PTNRUSER,PTNRADMIN,CONFERENCE,DEVUSER,PERSONNEL,PERSADMIN,SPONSORADMIN,FINANCE-1,FINANCE-2,FINANCE-3,FINANCE-RPT,FIN-EX-RATE";
                AInitialPassword = TPasswordHelper.GetRandomSecurePassword();
                ASiteKey = 10 * 1000000;

                if (AEmailAddress == String.Empty)
                {
                    AEmailAddress = "user@example.org";
                }

                if (AFirstName == String.Empty)
                {
                    AFirstName = "Demo";
                }

                if (ALastName == String.Empty)
                {
                    ALastName = "User";
                }

                if (AUserID == String.Empty)
                {
                    AUserID = "DEMO";
                }

                if (ALanguageCode == "99")
                {
                    ALanguageCode = AClientLanguage.ToUpper();
                    if (ALanguageCode.Contains("-"))
                    {
                        ALanguageCode = ALanguageCode.Substring(ALanguageCode.IndexOf("-")+1);
                    }
                }

                return true;
            }

            return false;
        }

        /// run initial setup, creating unprivileged user and setting the site key
        [RequireModulePermission("SYSMAN")]
        public static bool RunFirstSetup(
            string AUserID,
            string AFirstName,
            string ALastName,
            string ALanguageCode,
            string AEmailAddress,
            List<string> AInitialModulePermissions,
            string AInitialPassword,
            Int64 ASiteKey,
            bool AEnableSelfSignup,
            out TVerificationResultCollection AVerificationResult)
        {
            bool result = true;
            AVerificationResult = new TVerificationResultCollection();
            TVerificationResult VerificationResult = null;
            TVerificationResultCollection VerificationResultCollection = new TVerificationResultCollection();

            if (AInitialPassword != String.Empty)
            {
                // check if password is valid, it meets the criteria
                if (!TSharedSysManValidation.CheckPasswordQuality(AInitialPassword, out VerificationResult))
                {
                    AVerificationResult.Add(VerificationResult);
                    return false;
                }
            }

            result = TMaintenanceWebConnector.SaveUserAndModulePermissions(
                AUserID, AFirstName, ALastName, AEmailAddress, ALanguageCode,
                false, false, false, AInitialModulePermissions, 0,
                out VerificationResultCollection);

            if (result != false)
            {
                TDBTransaction t = new TDBTransaction();
                TDataBase db = DBAccess.Connect("RunFirstSetup");
                bool SubmitOK = false;

                db.WriteTransaction(ref t,
                    ref SubmitOK,
                    delegate
                    {
                        if (AInitialPassword != String.Empty)
                        {
                            result = TMaintenanceWebConnector.SetUserPassword(AUserID, AInitialPassword, false, false,
                                String.Empty, String.Empty, out VerificationResultCollection);
                        }
                        else
                        {
                            // TODO send welcoming Email, with link for setting the password
                        }

                        if (result)
                        {
                            TSystemDefaults defaults = new TSystemDefaults(db);
                            defaults.SetSystemDefault(SharedConstants.SYSDEFAULT_SITEKEY, ASiteKey.ToString(), db);
                            defaults.SetSystemDefault(SharedConstants.SYSDEFAULT_SELFSIGNUPENABLED, AEnableSelfSignup.ToString(), db);

                            GLSetupTDS GLMainDS = new GLSetupTDS();
                            SubmitOK = TGLSetupWebConnector.CreateSite(ref GLMainDS, "Default Site", ASiteKey, t);

                            if (SubmitOK)
                            {
                                GLSetupTDSAccess.SubmitChanges(GLMainDS, db, t);
                            }
                        }
                    });

                db.CloseDBConnection();
            }

            if (!result)
            {
                if (VerificationResultCollection.HasCriticalErrors)
                {
                    AVerificationResult = VerificationResultCollection;
                }
                else
                {
                    AVerificationResult.Add(VerificationResult);
                }
            }

            return result;
        }

    }
}
