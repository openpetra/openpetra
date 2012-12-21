//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.AR.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.GL.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MPartner.Partner.Data.Access;

namespace Ict.Petra.Server.MFinance.Setup.WebConnectors
{
    /// <summary>
    /// setup the account hierarchy, cost centre hierarchy, and other data relevant for a General Ledger
    /// </summary>
    public partial class TGLSetupWebConnector
    {
        /// <summary>
        /// returns all account hierarchies available for this ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLSetupTDS LoadAccountHierarchies(Int32 ALedgerNumber)
        {
            GLSetupTDS MainDS = new GLSetupTDS();

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, null);
            AAccountHierarchyAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAccountHierarchyDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAccountAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAccountPropertyAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAnalysisTypeAccess.LoadAll(MainDS, null);
            AAnalysisAttributeAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AFreeformAnalysisAccess.LoadViaALedger(MainDS, ALedgerNumber, null);

            // set Account BankAccountFlag if there exists a property
            foreach (AAccountPropertyRow accProp in MainDS.AAccountProperty.Rows)
            {
                if ((accProp.PropertyCode == MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT) && (accProp.PropertyValue == "true"))
                {
                    MainDS.AAccount.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        AAccountTable.GetAccountCodeDBName(),
                        accProp.AccountCode);
                    GLSetupTDSAAccountRow acc = (GLSetupTDSAAccountRow)MainDS.AAccount.DefaultView[0].Row;
                    acc.BankAccountFlag = true;
                    MainDS.AAccount.DefaultView.RowFilter = "";
                }
            }

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// returns cost centre hierarchy and cost centre details for this ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLSetupTDS LoadCostCentreHierarchy(Int32 ALedgerNumber)
        {
            GLSetupTDS MainDS = new GLSetupTDS();

            ACostCentreAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AValidLedgerNumberAccess.LoadViaALedger(MainDS, ALedgerNumber, null);

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        private static void DropAccountProperties(
            ref GLSetupTDS AInspectDS,
            Int32 ALedgerNumber,
            String AAccountCode)
        {
            AInspectDS.AAccountProperty.DefaultView.RowFilter = String.Format("{0}={1} and {2}='{3}'",
                AAccountPropertyTable.GetLedgerNumberDBName(),
                ALedgerNumber,
                AAccountPropertyTable.GetAccountCodeDBName(),
                AAccountCode);

            foreach (DataRowView rv in AInspectDS.AAccountProperty.DefaultView)
            {
                AAccountPropertyRow accountPropertyRow = (AAccountPropertyRow)rv.Row;
                accountPropertyRow.Delete();
            }

            AInspectDS.AAccountProperty.DefaultView.RowFilter = "";
        }

        /// <summary>
        /// save modified account hierarchy etc; does not support moving accounts;
        /// also used for saving cost centre hierarchy and cost centre details
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static TSubmitChangesResult SaveGLSetupTDS(
            Int32 ALedgerNumber,
            ref GLSetupTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult ReturnValue = TSubmitChangesResult.scrOK;
            TValidationControlsDict ValidationControlsDict = new TValidationControlsDict();

            AVerificationResult = new TVerificationResultCollection();

            if (AInspectDS == null)
            {
                return TSubmitChangesResult.scrNothingToBeSaved;
            }

            if ((AInspectDS.ACostCentre != null) && (AInspectDS.AValidLedgerNumber != null))
            {
                // check for removed cost centres, and also delete the AValidLedgerNumber row if there is one for the removed cost centre
                foreach (ACostCentreRow cc in AInspectDS.ACostCentre.Rows)
                {
                    if (cc.RowState == DataRowState.Deleted)
                    {
                        string CostCentreCodeToDelete = cc[ACostCentreTable.ColumnCostCentreCodeId, DataRowVersion.Original].ToString();
                        AInspectDS.AValidLedgerNumber.DefaultView.RowFilter =
                            String.Format("{0}='{1}'",
                                AValidLedgerNumberTable.GetCostCentreCodeDBName(),
                                CostCentreCodeToDelete);

                        foreach (DataRowView rv in AInspectDS.AValidLedgerNumber.DefaultView)
                        {
                            AValidLedgerNumberRow ValidLedgerNumberRow = (AValidLedgerNumberRow)rv.Row;

                            ValidLedgerNumberRow.Delete();
                        }
                    }
                }

                AInspectDS.AValidLedgerNumber.DefaultView.RowFilter = "";
            }

            if (AInspectDS.AAccount != null)
            {
                // check AAccount, if BankAccountFlag is not null, then create AAccountProperty or delete it
                foreach (GLSetupTDSAAccountRow acc in AInspectDS.AAccount.Rows)
                {
                    // special treatment of deleted accounts
                    if (acc.RowState == DataRowState.Deleted)
                    {
                        // delete all account properties as well
                        string AccountCodeToDelete = acc[GLSetupTDSAAccountTable.ColumnAccountCodeId, DataRowVersion.Original].ToString();

                        DropAccountProperties(ref AInspectDS, ALedgerNumber, AccountCodeToDelete);

                        continue;
                    }

                    // if the flag has been changed by the client, it will not be null
                    if (!acc.IsBankAccountFlagNull())
                    {
                        if (AInspectDS.AAccountProperty == null)
                        {
                            // because AccountProperty has not been changed on the client, GetChangesTyped will have removed the table
                            // so we need to reload the table from the database
                            AInspectDS.Merge(new AAccountPropertyTable());
                            AAccountPropertyAccess.LoadViaALedger(AInspectDS, ALedgerNumber, null);
                        }

                        AInspectDS.AAccountProperty.DefaultView.RowFilter =
                            String.Format("{0}='{1}' and {2}='{3}'",
                                AAccountPropertyTable.GetAccountCodeDBName(),
                                acc.AccountCode,
                                AAccountPropertyTable.GetPropertyCodeDBName(),
                                MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT);

                        if ((AInspectDS.AAccountProperty.DefaultView.Count == 0) && acc.BankAccountFlag)
                        {
                            AAccountPropertyRow accProp = AInspectDS.AAccountProperty.NewRowTyped(true);
                            accProp.LedgerNumber = acc.LedgerNumber;
                            accProp.AccountCode = acc.AccountCode;
                            accProp.PropertyCode = MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT;
                            accProp.PropertyValue = "true";
                            AInspectDS.AAccountProperty.Rows.Add(accProp);
                        }
                        else if (AInspectDS.AAccountProperty.DefaultView.Count == 1)
                        {
                            AAccountPropertyRow accProp = (AAccountPropertyRow)AInspectDS.AAccountProperty.DefaultView[0].Row;

                            if (!acc.BankAccountFlag)
                            {
                                accProp.Delete();
                            }
                            else
                            {
                                accProp.PropertyValue = "true";
                            }
                        }

                        AInspectDS.AAccountProperty.DefaultView.RowFilter = "";
                    }
                }
            }

            if (AInspectDS.AAnalysisType != null)
            {
                if (AInspectDS.AAnalysisType.Rows.Count > 0)
                {
                    ValidateAAnalysisType(ValidationControlsDict, ref AVerificationResult, AInspectDS.AAnalysisType);
                    ValidateAAnalysisTypeManual(ValidationControlsDict, ref AVerificationResult, AInspectDS.AAnalysisType);

                    if (AVerificationResult.HasCriticalErrors)
                    {
                        ReturnValue = TSubmitChangesResult.scrError;
                    }
                }
            }

            if (ReturnValue != TSubmitChangesResult.scrError)
            {
                ReturnValue = GLSetupTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);
            }

            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                TCacheableFinanceTablesEnum.AccountList.ToString());
            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                TCacheableFinanceTablesEnum.AnalysisTypeList.ToString());
            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                TCacheableFinanceTablesEnum.CostCentreList.ToString());

            if (AVerificationResult.Count > 0)
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
            }

            return ReturnValue;
        }

        private static bool AccountCodeHasBeenUsed(Int32 ALedgerNumber, string AAccountCode, TDBTransaction Transaction)
        {
            // TODO: enhance sql statement to check for more references to a_account

            String QuerySql =
                "SELECT COUNT (*) FROM PUB_a_transaction WHERE " +
                "a_ledger_number_i=" + ALedgerNumber + " AND " +
                "a_account_code_c = '" + AAccountCode + "';";
            object SqlResult = DBAccess.GDBAccessObj.ExecuteScalar(QuerySql, Transaction);
            bool IsInUse = (Convert.ToInt32(SqlResult) > 0);

            if (!IsInUse)
            {
                QuerySql =
                    "SELECT COUNT (*) FROM PUB_a_ap_document_detail WHERE " +
                    "a_ledger_number_i=" + ALedgerNumber + " AND " +
                    "a_account_code_c = '" + AAccountCode + "';";
                SqlResult = DBAccess.GDBAccessObj.ExecuteScalar(QuerySql, Transaction);
                IsInUse = (Convert.ToInt32(SqlResult) > 0);
            }

            return IsInUse;
        }

        private static bool AccountHasChildren(Int32 ALedgerNumber, string AAccountCode, TDBTransaction Transaction)
        {
            String QuerySql =
                "SELECT COUNT (*) FROM PUB_a_account_hierarchy_detail WHERE " +
                "a_ledger_number_i=" + ALedgerNumber + " AND " +
                "a_account_code_to_report_to_c = '" + AAccountCode + "';";
            object SqlResult = DBAccess.GDBAccessObj.ExecuteScalar(QuerySql, Transaction);

            return Convert.ToInt32(SqlResult) > 0;
        }

        /// <summary>I can add child accounts to this account if it's a summary account,
        ///          or if there have never been transactions posted to it.
        ///
        ///          (If children are added to this account, it will be promoted to a summary account.)
        ///
        ///          I can delete this account if it has no transactions posted as above,
        ///          AND it has no children.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACanBeParent"></param>
        /// <param name="ACanDelete"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean GetAccountCodeAttributes(Int32 ALedgerNumber, String AAccountCode, out bool ACanBeParent, out bool ACanDelete)
        {
//        public static Boolean AccountCodeCanHaveChildren(Int32 ALedgerNumber, String AAccountCode)
            ACanBeParent = true;
            ACanDelete = true;
            bool DbSuccess = true;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            AAccountTable AccountTbl = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, AAccountCode, Transaction);

            if (AccountTbl.Rows.Count < 1)  // This shouldn't happen..
            {
                DbSuccess = false;
            }
            else
            {
                bool IsParent = AccountHasChildren(ALedgerNumber, AAccountCode, Transaction);
                AAccountRow AccountRow = AccountTbl[0];
                ACanBeParent = IsParent; // If it's a summary account, it's OK (This shouldn't happen either, because the client shouldn't ask me!)
                ACanDelete = !IsParent;

                if (!ACanBeParent || ACanDelete)
                {
                    bool IsInUse = AccountCodeHasBeenUsed(ALedgerNumber, AAccountCode, Transaction);
                    ACanBeParent = !IsInUse;    // For posting accounts, I can still add children (and upgrade the account) if there's nothing posted to it yet.
                    ACanDelete = !IsInUse;      // Once it has transactions posted, I can't delete it, ever.
                }
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
            return DbSuccess;
        }

        #region Data Validation

        static partial void ValidateAAnalysisType(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidateAAnalysisTypeManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        #endregion Data Validation

        /// <summary>
        /// helper function for ExportAccountHierarchy
        /// </summary>
        private static void InsertNodeIntoXmlDocument(GLSetupTDS AMainDS,
            XmlDocument ADoc,
            XmlNode AParentNode,
            AAccountHierarchyDetailRow ADetailRow)
        {
            AAccountRow account = (AAccountRow)AMainDS.AAccount.Rows.Find(new object[] { ADetailRow.LedgerNumber, ADetailRow.ReportingAccountCode });
            XmlElement accountNode = ADoc.CreateElement(TYml2Xml.XMLELEMENT);

            // AccountCodeToReportTo and ReportOrder are encoded implicitly
            accountNode.SetAttribute("name", ADetailRow.ReportingAccountCode);
            accountNode.SetAttribute("active", account.AccountActiveFlag ? "True" : "False");
            accountNode.SetAttribute("type", account.AccountType.ToString());
            accountNode.SetAttribute("debitcredit", account.DebitCreditIndicator ? "debit" : "credit");
            accountNode.SetAttribute("validcc", account.ValidCcCombo);
            accountNode.SetAttribute("shortdesc", account.EngAccountCodeShortDesc);

            if (account.EngAccountCodeLongDesc != account.EngAccountCodeShortDesc)
            {
                accountNode.SetAttribute("longdesc", account.EngAccountCodeLongDesc);
            }

            if (account.EngAccountCodeShortDesc != account.AccountCodeShortDesc)
            {
                accountNode.SetAttribute("localdesc", account.AccountCodeShortDesc);
            }

            if (account.EngAccountCodeLongDesc != account.AccountCodeLongDesc)
            {
                accountNode.SetAttribute("locallongdesc", account.AccountCodeLongDesc);
            }

            if (AMainDS.AAccountProperty.Rows.Find(new object[] { account.LedgerNumber, account.AccountCode,
                                                                  MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT, "true" }) != null)
            {
                accountNode.SetAttribute("bankaccount", "true");
            }

            if (account.ForeignCurrencyFlag)
            {
                accountNode.SetAttribute("currency", account.ForeignCurrencyCode);
            }

            AParentNode.AppendChild(accountNode);

            AMainDS.AAccountHierarchyDetail.DefaultView.Sort = AAccountHierarchyDetailTable.GetReportOrderDBName();
            AMainDS.AAccountHierarchyDetail.DefaultView.RowFilter =
                AAccountHierarchyDetailTable.GetAccountHierarchyCodeDBName() + " = '" + ADetailRow.AccountHierarchyCode + "' AND " +
                AAccountHierarchyDetailTable.GetAccountCodeToReportToDBName() + " = '" + ADetailRow.ReportingAccountCode + "'";

            foreach (DataRowView rowView in AMainDS.AAccountHierarchyDetail.DefaultView)
            {
                AAccountHierarchyDetailRow accountDetail = (AAccountHierarchyDetailRow)rowView.Row;
                InsertNodeIntoXmlDocument(AMainDS, ADoc, accountNode, accountDetail);
            }
        }

        /// <summary>
        /// return a simple XMLDocument (encoded into a string) with the account hierarchy and account details;
        /// root account can be calculated (find which account is reporting nowhere)
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountHierarchyName"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static string ExportAccountHierarchy(Int32 ALedgerNumber, string AAccountHierarchyName)
        {
            GLSetupTDS MainDS = new GLSetupTDS();

            AAccountHierarchyAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAccountHierarchyDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAccountAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAccountPropertyAccess.LoadViaALedger(MainDS, ALedgerNumber, null);

            XmlDocument doc = TYml2Xml.CreateXmlDocument();

            AAccountHierarchyRow accountHierarchy = (AAccountHierarchyRow)MainDS.AAccountHierarchy.Rows.Find(new object[] { ALedgerNumber,
                                                                                                                            AAccountHierarchyName });

            if (accountHierarchy != null)
            {
                // find the BALSHT account that is reporting to the root account
                MainDS.AAccountHierarchyDetail.DefaultView.RowFilter =
                    AAccountHierarchyDetailTable.GetAccountHierarchyCodeDBName() + " = '" + AAccountHierarchyName + "' AND " +
                    AAccountHierarchyDetailTable.GetAccountCodeToReportToDBName() + " = '" + accountHierarchy.RootAccountCode + "'";

                InsertNodeIntoXmlDocument(MainDS, doc, doc.DocumentElement,
                    (AAccountHierarchyDetailRow)MainDS.AAccountHierarchyDetail.DefaultView[0].Row);
            }

            // XmlDocument is not serializable, therefore print it to string and return the string
            return TXMLParser.XmlToString(doc);
        }

        /// <summary>
        /// helper function for ExportCostCentreHierarchy
        /// </summary>
        private static void InsertNodeIntoXmlDocument(GLSetupTDS AMainDS,
            XmlDocument ADoc,
            XmlNode AParentNode,
            ACostCentreRow ADetailRow)
        {
            XmlElement costCentreNode = ADoc.CreateElement(TYml2Xml.XMLELEMENT);

            // CostCentreToReportTo is encoded implicitly
            costCentreNode.SetAttribute("name", ADetailRow.CostCentreCode);
            costCentreNode.SetAttribute("descr", ADetailRow.CostCentreName);
            costCentreNode.SetAttribute("active", ADetailRow.CostCentreActiveFlag ? "True" : "False");
            costCentreNode.SetAttribute("type", ADetailRow.CostCentreType.ToString());
            AParentNode.AppendChild(costCentreNode);

            AMainDS.ACostCentre.DefaultView.Sort = ACostCentreTable.GetCostCentreCodeDBName();
            AMainDS.ACostCentre.DefaultView.RowFilter =
                ACostCentreTable.GetCostCentreToReportToDBName() + " = '" + ADetailRow.CostCentreCode + "'";

            foreach (DataRowView rowView in AMainDS.ACostCentre.DefaultView)
            {
                InsertNodeIntoXmlDocument(AMainDS, ADoc, costCentreNode, (ACostCentreRow)rowView.Row);
            }
        }

        /// <summary>
        /// return a simple XMLDocument (encoded into a string) with the cost centre hierarchy and cost centre details;
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static string ExportCostCentreHierarchy(Int32 ALedgerNumber)
        {
            GLSetupTDS MainDS = new GLSetupTDS();

            ACostCentreAccess.LoadViaALedger(MainDS, ALedgerNumber, null);

            XmlDocument doc = TYml2Xml.CreateXmlDocument();

            MainDS.ACostCentre.DefaultView.RowFilter =
                ACostCentreTable.GetCostCentreToReportToDBName() + " IS NULL";

            InsertNodeIntoXmlDocument(MainDS, doc, doc.DocumentElement,
                (ACostCentreRow)MainDS.ACostCentre.DefaultView[0].Row);

            // XmlDocument is not serializable, therefore print it to string and return the string
            return TXMLParser.XmlToString(doc);
        }

        private static void CreateAccountHierarchyRecursively(ref GLSetupTDS AMainDS,
            Int32 ALedgerNumber,
            ref StringCollection AImportedAccountNames,
            XmlNode ACurrentNode,
            string AParentAccountCode)
        {
            AAccountRow newAccount = null;

            string AccountCode = TYml2Xml.GetElementName(ACurrentNode);

            AImportedAccountNames.Add(AccountCode);

            // does this account already exist?
            bool newRow = false;
            DataRow existingAccount = AMainDS.AAccount.Rows.Find(new object[] { ALedgerNumber, AccountCode });

            if (existingAccount != null)
            {
                newAccount = (AAccountRow)existingAccount;
                DropAccountProperties(ref AMainDS, ALedgerNumber, AccountCode);
                ((GLSetupTDSAAccountRow)newAccount).BankAccountFlag = false;
            }
            else
            {
                newRow = true;
                newAccount = AMainDS.AAccount.NewRowTyped();
            }

            newAccount.LedgerNumber = AMainDS.AAccountHierarchy[0].LedgerNumber;
            newAccount.AccountCode = AccountCode;
            newAccount.AccountActiveFlag = TYml2Xml.GetAttributeRecursive(ACurrentNode, "active").ToLower() == "true";
            newAccount.AccountType = TYml2Xml.GetAttributeRecursive(ACurrentNode, "type");
            newAccount.DebitCreditIndicator = TYml2Xml.GetAttributeRecursive(ACurrentNode, "debitcredit") == "debit";
            newAccount.ValidCcCombo = TYml2Xml.GetAttributeRecursive(ACurrentNode, "validcc");
            newAccount.EngAccountCodeShortDesc = TYml2Xml.GetAttributeRecursive(ACurrentNode, "shortdesc");

            if (TXMLParser.HasAttribute(ACurrentNode, "shortdesc"))
            {
                newAccount.EngAccountCodeLongDesc = TYml2Xml.GetAttribute(ACurrentNode, "longdesc");
                newAccount.AccountCodeShortDesc = TYml2Xml.GetAttribute(ACurrentNode, "localdesc");
                newAccount.AccountCodeLongDesc = TYml2Xml.GetAttribute(ACurrentNode, "locallongdesc");
            }
            else
            {
                newAccount.EngAccountCodeLongDesc = TYml2Xml.GetAttributeRecursive(ACurrentNode, "longdesc");
                newAccount.AccountCodeShortDesc = TYml2Xml.GetAttributeRecursive(ACurrentNode, "localdesc");
                newAccount.AccountCodeLongDesc = TYml2Xml.GetAttributeRecursive(ACurrentNode, "locallongdesc");
            }

            if (newAccount.EngAccountCodeLongDesc.Length == 0)
            {
                newAccount.EngAccountCodeLongDesc = newAccount.EngAccountCodeShortDesc;
            }

            if (newAccount.AccountCodeShortDesc.Length == 0)
            {
                newAccount.AccountCodeShortDesc = newAccount.EngAccountCodeShortDesc;
            }

            if (newAccount.AccountCodeLongDesc.Length == 0)
            {
                newAccount.AccountCodeLongDesc = newAccount.AccountCodeShortDesc;
            }

            if (newRow)
            {
                AMainDS.AAccount.Rows.Add(newAccount);
            }

            if (TYml2Xml.GetAttributeRecursive(ACurrentNode, "bankaccount") == "true")
            {
                AAccountPropertyRow accProp = AMainDS.AAccountProperty.NewRowTyped(true);
                accProp.LedgerNumber = newAccount.LedgerNumber;
                accProp.AccountCode = newAccount.AccountCode;
                accProp.PropertyCode = MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT;
                accProp.PropertyValue = "true";
                AMainDS.AAccountProperty.Rows.Add(accProp);
                ((GLSetupTDSAAccountRow)newAccount).BankAccountFlag = true;
            }

            if (TYml2Xml.HasAttributeRecursive(ACurrentNode, "currency"))
            {
                string currency = TYml2Xml.GetAttributeRecursive(ACurrentNode, "currency");

                if (currency != AMainDS.ALedger[0].BaseCurrency)
                {
                    newAccount.ForeignCurrencyCode = currency;
                    newAccount.ForeignCurrencyFlag = true;
                }
            }

            // account hierarchy has been deleted, so always add
            AAccountHierarchyDetailRow newAccountHDetail = AMainDS.AAccountHierarchyDetail.NewRowTyped();
            newAccountHDetail.LedgerNumber = AMainDS.AAccountHierarchy[0].LedgerNumber;
            newAccountHDetail.AccountHierarchyCode = AMainDS.AAccountHierarchy[0].AccountHierarchyCode;
            newAccountHDetail.AccountCodeToReportTo = AParentAccountCode;
            newAccountHDetail.ReportingAccountCode = AccountCode;

            AMainDS.AAccountHierarchyDetail.Rows.Add(newAccountHDetail);

            newAccount.PostingStatus = !ACurrentNode.HasChildNodes;

            foreach (XmlNode child in ACurrentNode.ChildNodes)
            {
                CreateAccountHierarchyRecursively(ref AMainDS, ALedgerNumber, ref AImportedAccountNames, child, newAccount.AccountCode);
            }
        }

        /// <summary>
        /// only works if there are no balances/transactions yet for the accounts that are deleted
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AHierarchyName"></param>
        /// <param name="AXmlAccountHierarchy"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool ImportAccountHierarchy(Int32 ALedgerNumber, string AHierarchyName, string AXmlAccountHierarchy)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(AXmlAccountHierarchy);
            }
            catch (XmlException exp)
            {
                throw new Exception(
                    Catalog.GetString("There was a problem with the syntax of the file.") +
                    Environment.NewLine +
                    exp.Message +
                    Environment.NewLine +
                    AXmlAccountHierarchy);
            }

            GLSetupTDS MainDS = LoadAccountHierarchies(ALedgerNumber);
            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            StringCollection ImportedAccountNames = new StringCollection();

            ImportedAccountNames.Add(ALedgerNumber.ToString());

            // delete all account hierarchy details of this hierarchy
            foreach (AAccountHierarchyDetailRow accounthdetail in MainDS.AAccountHierarchyDetail.Rows)
            {
                if (accounthdetail.AccountHierarchyCode == AHierarchyName)
                {
                    accounthdetail.Delete();
                }
            }

            CreateAccountHierarchyRecursively(ref MainDS, ALedgerNumber, ref ImportedAccountNames, root, ALedgerNumber.ToString());

            foreach (AAccountRow accountRow in MainDS.AAccount.Rows)
            {
                if ((accountRow.RowState != DataRowState.Deleted) && !ImportedAccountNames.Contains(accountRow.AccountCode))
                {
                    // TODO: delete accounts that don't exist anymore in the new hierarchy, or deactivate them?
                    //       but need to raise a failure because they are missing in the account hierarchy
                    // (check if their balance is empty and no transactions exist, or catch database constraint violation)
                    // TODO: what about system accounts? probably alright to ignore here

                    accountRow.Delete();
                }
            }

            TVerificationResultCollection VerificationResult;
            return SaveGLSetupTDS(ALedgerNumber, ref MainDS, out VerificationResult) == TSubmitChangesResult.scrOK;
        }

        private static void CreateCostCentresRecursively(ref GLSetupTDS AMainDS,
            Int32 ALedgerNumber,
            ref StringCollection AImportedCostCentreCodes,
            XmlNode ACurrentNode,
            string AParentCostCentreCode)
        {
            ACostCentreRow newCostCentre = null;

            string CostCentreCode = TYml2Xml.GetElementName(ACurrentNode);

            AImportedCostCentreCodes.Add(CostCentreCode);

            // does this costcentre already exist?
            bool newRow = false;
            DataRow existingCostCentre = AMainDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, CostCentreCode });

            if (existingCostCentre != null)
            {
                newCostCentre = (ACostCentreRow)existingCostCentre;
            }
            else
            {
                newRow = true;
                newCostCentre = AMainDS.ACostCentre.NewRowTyped();
            }

            newCostCentre.LedgerNumber = ALedgerNumber;
            newCostCentre.CostCentreCode = CostCentreCode;
            newCostCentre.CostCentreName = TYml2Xml.GetAttribute(ACurrentNode, "descr");
            newCostCentre.CostCentreActiveFlag = TYml2Xml.GetAttributeRecursive(ACurrentNode, "active").ToLower() == "true";
            newCostCentre.CostCentreType = TYml2Xml.GetAttributeRecursive(ACurrentNode, "type");
            newCostCentre.PostingCostCentreFlag = (ACurrentNode.ChildNodes.Count == 0);

            if ((AParentCostCentreCode != null) && (AParentCostCentreCode.Length != 0))
            {
                newCostCentre.CostCentreToReportTo = AParentCostCentreCode;
            }

            if (newRow)
            {
                AMainDS.ACostCentre.Rows.Add(newCostCentre);
            }

            foreach (XmlNode child in ACurrentNode.ChildNodes)
            {
                CreateCostCentresRecursively(ref AMainDS, ALedgerNumber, ref AImportedCostCentreCodes, child, newCostCentre.CostCentreCode);
            }
        }

        /// <summary>
        /// only works if there are no balances/transactions yet for the cost centres that are deleted
        /// </summary>
        [RequireModulePermission("FINANCE-3")]
        public static bool ImportCostCentreHierarchy(Int32 ALedgerNumber, string AXmlHierarchy)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(AXmlHierarchy);

            GLSetupTDS MainDS = LoadCostCentreHierarchy(ALedgerNumber);
            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            StringCollection ImportedCostCentreNames = new StringCollection();

            CreateCostCentresRecursively(ref MainDS, ALedgerNumber, ref ImportedCostCentreNames, root, null);

            foreach (ACostCentreRow costCentreRow in MainDS.ACostCentre.Rows)
            {
                if ((costCentreRow.RowState != DataRowState.Deleted) && !ImportedCostCentreNames.Contains(costCentreRow.CostCentreCode))
                {
                    // TODO: delete costcentres that don't exist anymore in the new hierarchy, or deactivate them?
                    // (check if their balance is empty and no transactions exist, or catch database constraint violation)
                    // TODO: what about system cost centres? probably alright to ignore here

                    costCentreRow.Delete();
                }
            }

            TVerificationResultCollection VerificationResult;
            return SaveGLSetupTDS(ALedgerNumber, ref MainDS, out VerificationResult) == TSubmitChangesResult.scrOK;
        }

        /// <summary>
        /// import basic data for new ledger
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool ImportNewLedger(Int32 ALedgerNumber,
            string AXmlLedgerDetails,
            string AXmlAccountHierarchy,
            string AXmlCostCentreHierarchy,
            string AXmlMotivationDetails
            )
        {
            // TODO ImportNewLedger

            // if this ledger already exists, delete all tables first?
            // Or try to reuse existing balances etc?

            // first create/modify ledger
            // set ForexGainsLossesAccount; there is no foreign key, so no problem

            // create the calendar for the ledger, automatically calculating the dates of the forwarding periods

            // create the partner with special type LEDGER from the ledger number, with 6 trailing zeros

            // create/modify accounts (might need to drop motivation details)

            // create/modify costcentres (might need to drop motivation details)

            // create/modify motivation details

            return false;
        }

        /// import a new Account hierarchy into an empty new ledger
        private static void ImportDefaultAccountHierarchy(ref GLSetupTDS AMainDS, Int32 ALedgerNumber)
        {
            XmlDocument doc;
            TYml2Xml ymlFile;
            string Filename = TAppSettingsManager.GetValue("SqlFiles.Path", ".") +
                              Path.DirectorySeparatorChar +
                              "DefaultAccountHierarchy.yml";

            try
            {
                ymlFile = new TYml2Xml(Filename);
                doc = ymlFile.ParseYML2XML();
            }
            catch (XmlException exp)
            {
                throw new Exception(
                    Catalog.GetString("There was a problem with the syntax of the file.") +
                    Environment.NewLine +
                    exp.Message +
                    Environment.NewLine +
                    Filename);
            }

            // create the root account
            AAccountHierarchyRow accountHierarchyRow = AMainDS.AAccountHierarchy.NewRowTyped();
            accountHierarchyRow.LedgerNumber = ALedgerNumber;
            accountHierarchyRow.AccountHierarchyCode = "STANDARD";
            accountHierarchyRow.RootAccountCode = ALedgerNumber.ToString();
            AMainDS.AAccountHierarchy.Rows.Add(accountHierarchyRow);

            AAccountRow accountRow = AMainDS.AAccount.NewRowTyped();
            accountRow.LedgerNumber = ALedgerNumber;
            accountRow.AccountCode = ALedgerNumber.ToString();
            accountRow.PostingStatus = false;
            AMainDS.AAccount.Rows.Add(accountRow);

            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            StringCollection ImportedAccountNames = new StringCollection();

            CreateAccountHierarchyRecursively(ref AMainDS, ALedgerNumber, ref ImportedAccountNames, root, ALedgerNumber.ToString());
        }

        private static void ImportDefaultCostCentreHierarchy(ref GLSetupTDS AMainDS, Int32 ALedgerNumber, string ALedgerName)
        {
            // load XmlCostCentreHierarchy from a default file

            string Filename = TAppSettingsManager.GetValue("SqlFiles.Path", ".") +
                              Path.DirectorySeparatorChar +
                              "DefaultCostCentreHierarchy.yml";
            TextReader reader = new StreamReader(Filename, TTextFile.GetFileEncoding(Filename), false);
            string XmlCostCentreHierarchy = reader.ReadToEnd();

            reader.Close();

            XmlCostCentreHierarchy = XmlCostCentreHierarchy.Replace("{#LEDGERNUMBER}", ALedgerNumber.ToString());
            XmlCostCentreHierarchy = XmlCostCentreHierarchy.Replace("{#LEDGERNUMBERWITHLEADINGZEROS}", ALedgerNumber.ToString("00"));

            if (ALedgerName.Length == 0)
            {
                throw new Exception("We need a name for the ledger, otherwise the yml will be invalid");
            }

            XmlCostCentreHierarchy = XmlCostCentreHierarchy.Replace("{#LEDGERNAME}", ALedgerName);

            string[] lines = XmlCostCentreHierarchy.Replace("\r", "").Split(new char[] { '\n' });
            TYml2Xml ymlFile = new TYml2Xml(lines);
            XmlDocument doc = ymlFile.ParseYML2XML();

            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            StringCollection ImportedCostCentreNames = new StringCollection();

            CreateCostCentresRecursively(ref AMainDS, ALedgerNumber, ref ImportedCostCentreNames, root, null);
        }

        /// <summary>
        /// create a new ledger and do the initial setup
        /// </summary>
        [RequireModulePermission("FINANCE-3")]
        public static bool CreateNewLedger(Int32 ANewLedgerNumber,
            String ALedgerName,
            String ACountryCode,
            String ABaseCurrency,
            String AIntlCurrency,
            DateTime ACalendarStartDate,
            Int32 ANumberOfPeriods,
            Int32 ACurrentPeriod,
            Int32 ANumberOfFwdPostingPeriods,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            // check if such a ledger already exists
            ALedgerTable tempLedger = ALedgerAccess.LoadByPrimaryKey(ANewLedgerNumber, null);

            if (tempLedger.Count > 0)
            {
                AVerificationResult = new TVerificationResultCollection();
                string msg = String.Format(Catalog.GetString(
                        "There is already a ledger with number {0}. Please choose another number."), ANewLedgerNumber);
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Creating Ledger"), msg, TResultSeverity.Resv_Critical));
                return false;
            }

            if ((ANewLedgerNumber <= 1) || (ANewLedgerNumber > 9999))
            {
                // ledger number 1 does not work, because the root unit has partner key 1000000.
                AVerificationResult = new TVerificationResultCollection();
                string msg = String.Format(Catalog.GetString(
                        "Invalid number {0} for a ledger. Please choose a number between 2 and 9999."), ANewLedgerNumber);
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Creating Ledger"), msg, TResultSeverity.Resv_Critical));
                return false;
            }

            Int64 PartnerKey = Convert.ToInt64(ANewLedgerNumber) * 1000000L;
            GLSetupTDS MainDS = new GLSetupTDS();

            ALedgerRow ledgerRow = MainDS.ALedger.NewRowTyped();
            ledgerRow.LedgerNumber = ANewLedgerNumber;
            ledgerRow.LedgerName = ALedgerName;
            ledgerRow.CurrentPeriod = ACurrentPeriod;
            ledgerRow.NumberOfAccountingPeriods = ANumberOfPeriods;
            ledgerRow.NumberFwdPostingPeriods = ANumberOfFwdPostingPeriods;
            ledgerRow.BaseCurrency = ABaseCurrency;
            ledgerRow.IntlCurrency = AIntlCurrency;
            ledgerRow.ActualsDataRetention = 11;
            ledgerRow.GiftDataRetention = 5;
            ledgerRow.CountryCode = ACountryCode;
            ledgerRow.ForexGainsLossesAccount = "5003";
            ledgerRow.PartnerKey = PartnerKey;
            MainDS.ALedger.Rows.Add(ledgerRow);

            if (!PPartnerAccess.Exists(PartnerKey, null))
            {
                PPartnerRow partnerRow = MainDS.PPartner.NewRowTyped();
                ledgerRow.PartnerKey = PartnerKey;
                partnerRow.PartnerKey = PartnerKey;
                partnerRow.PartnerShortName = ALedgerName;
                partnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;
                partnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_UNIT;
                MainDS.PPartner.Rows.Add(partnerRow);
            }

            PPartnerTypeAccess.LoadViaPPartner(MainDS, PartnerKey, null);
            PPartnerTypeRow partnerTypeRow;

            if (MainDS.PPartnerType.Rows.Count > 0)
            {
                partnerTypeRow = MainDS.PPartnerType[0];
                partnerTypeRow.TypeCode = MPartnerConstants.PARTNERTYPE_LEDGER;
            }
            else
            {
                partnerTypeRow = MainDS.PPartnerType.NewRowTyped();
                partnerTypeRow.PartnerKey = PartnerKey;
                partnerTypeRow.TypeCode = MPartnerConstants.PARTNERTYPE_LEDGER;
                MainDS.PPartnerType.Rows.Add(partnerTypeRow);
            }

            if (!PUnitAccess.Exists(PartnerKey, null))
            {
                PUnitRow unitRow = MainDS.PUnit.NewRowTyped();
                unitRow.PartnerKey = PartnerKey;
                MainDS.PUnit.Rows.Add(unitRow);
            }

            if (!PLocationAccess.Exists(PartnerKey, 0, null))
            {
                PLocationRow locationRow = MainDS.PLocation.NewRowTyped();
                locationRow.SiteKey = PartnerKey;
                locationRow.LocationKey = 0;
                locationRow.StreetName = Catalog.GetString("No valid address on file");
                locationRow.CountryCode = ACountryCode;
                MainDS.PLocation.Rows.Add(locationRow);

                PPartnerLocationRow partnerLocationRow = MainDS.PPartnerLocation.NewRowTyped();
                partnerLocationRow.SiteKey = PartnerKey;
                partnerLocationRow.PartnerKey = PartnerKey;
                partnerLocationRow.LocationKey = 0;
                MainDS.PPartnerLocation.Rows.Add(partnerLocationRow);
            }

            if (!PPartnerLedgerAccess.Exists(PartnerKey, null))
            {
                PPartnerLedgerRow partnerLedgerRow = MainDS.PPartnerLedger.NewRowTyped();
                partnerLedgerRow.PartnerKey = PartnerKey;

                // calculate last partner id, from older uses of this ledger number
                object MaxExistingPartnerKeyObj = DBAccess.GDBAccessObj.ExecuteScalar(
                    String.Format("SELECT MAX(p_partner_key_n) FROM PUB_p_partner " +
                        "WHERE p_partner_key_n > {0} AND p_partner_key_n < {1}",
                        PartnerKey,
                        PartnerKey + 500000), IsolationLevel.ReadCommitted);

                if (MaxExistingPartnerKeyObj.GetType() != typeof(DBNull))
                {
                    partnerLedgerRow.LastPartnerId = Convert.ToInt32(Convert.ToInt64(MaxExistingPartnerKeyObj) - PartnerKey);
                }
                else
                {
                    partnerLedgerRow.LastPartnerId = 5000;
                }

                MainDS.PPartnerLedger.Rows.Add(partnerLedgerRow);
            }

            String ModuleId = "LEDGER" + ANewLedgerNumber.ToString("0000");

            if (!SModuleAccess.Exists(ModuleId, null))
            {
                SModuleRow moduleRow = MainDS.SModule.NewRowTyped();
                moduleRow.ModuleId = ModuleId;
                moduleRow.ModuleName = moduleRow.ModuleId;
                MainDS.SModule.Rows.Add(moduleRow);
            }

            // if this is the first ledger, make it the default site
            SSystemDefaultsTable systemDefaults = SSystemDefaultsAccess.LoadByPrimaryKey("SiteKey", null);

            if (systemDefaults.Rows.Count == 0)
            {
                SSystemDefaultsRow systemDefaultsRow = MainDS.SSystemDefaults.NewRowTyped();
                systemDefaultsRow.DefaultCode = SharedConstants.SYSDEFAULT_SITEKEY;
                systemDefaultsRow.DefaultDescription = "there has to be one site key for the database";
                systemDefaultsRow.DefaultValue = PartnerKey.ToString("0000000000");
                MainDS.SSystemDefaults.Rows.Add(systemDefaultsRow);
            }

            // create calendar
            // at the moment we only support financial years that start on the first day of a month
            DateTime periodStartDate = ACalendarStartDate;

            for (Int32 periodNumber = 1; periodNumber <= ANumberOfPeriods + ANumberOfFwdPostingPeriods; periodNumber++)
            {
                AAccountingPeriodRow accountingPeriodRow = MainDS.AAccountingPeriod.NewRowTyped();
                accountingPeriodRow.LedgerNumber = ANewLedgerNumber;
                accountingPeriodRow.AccountingPeriodNumber = periodNumber;
                accountingPeriodRow.PeriodStartDate = periodStartDate;
                accountingPeriodRow.PeriodEndDate = periodStartDate.AddMonths(1).AddDays(-1);
                accountingPeriodRow.AccountingPeriodDesc = periodStartDate.ToString("MMMM");
                MainDS.AAccountingPeriod.Rows.Add(accountingPeriodRow);
                periodStartDate = periodStartDate.AddMonths(1);
            }

            AAccountingSystemParameterRow accountingSystemParameterRow = MainDS.AAccountingSystemParameter.NewRowTyped();
            accountingSystemParameterRow.LedgerNumber = ANewLedgerNumber;
            accountingSystemParameterRow.ActualsDataRetention = ledgerRow.ActualsDataRetention;
            accountingSystemParameterRow.GiftDataRetention = ledgerRow.GiftDataRetention;
            accountingSystemParameterRow.NumberFwdPostingPeriods = ledgerRow.NumberFwdPostingPeriods;
            accountingSystemParameterRow.NumberOfAccountingPeriods = ledgerRow.NumberOfAccountingPeriods;
            accountingSystemParameterRow.BudgetDataRetention = ledgerRow.BudgetDataRetention;
            MainDS.AAccountingSystemParameter.Rows.Add(accountingSystemParameterRow);

            ASystemInterfaceRow systemInterfaceRow = MainDS.ASystemInterface.NewRowTyped();
            systemInterfaceRow.LedgerNumber = ANewLedgerNumber;

            systemInterfaceRow.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            systemInterfaceRow.SetUpComplete = true;
            MainDS.ASystemInterface.Rows.Add(systemInterfaceRow);
            systemInterfaceRow = MainDS.ASystemInterface.NewRowTyped();
            systemInterfaceRow.LedgerNumber = ANewLedgerNumber;
            systemInterfaceRow.SubSystemCode = CommonAccountingSubSystemsEnum.GR.ToString();
            systemInterfaceRow.SetUpComplete = true;
            MainDS.ASystemInterface.Rows.Add(systemInterfaceRow);
            systemInterfaceRow = MainDS.ASystemInterface.NewRowTyped();
            systemInterfaceRow.LedgerNumber = ANewLedgerNumber;
            systemInterfaceRow.SubSystemCode = CommonAccountingSubSystemsEnum.AP.ToString();
            systemInterfaceRow.SetUpComplete = true;
            MainDS.ASystemInterface.Rows.Add(systemInterfaceRow);

            ATransactionTypeRow transactionTypeRow;

            // TODO: this might be different for other account or costcentre names
            transactionTypeRow = MainDS.ATransactionType.NewRowTyped();
            transactionTypeRow.LedgerNumber = ANewLedgerNumber;
            transactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            transactionTypeRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.ALLOC.ToString();
            transactionTypeRow.DebitAccountCode = "BAL SHT";
            transactionTypeRow.CreditAccountCode = "BAL SHT";
            transactionTypeRow.TransactionTypeDescription = "Allocation Journal";
            transactionTypeRow.SpecialTransactionType = true;
            MainDS.ATransactionType.Rows.Add(transactionTypeRow);
            transactionTypeRow = MainDS.ATransactionType.NewRowTyped();
            transactionTypeRow.LedgerNumber = ANewLedgerNumber;
            transactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            transactionTypeRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.REALLOC.ToString();
            transactionTypeRow.DebitAccountCode = "BAL SHT";
            transactionTypeRow.CreditAccountCode = "BAL SHT";
            transactionTypeRow.TransactionTypeDescription = "Reallocation Journal";
            transactionTypeRow.SpecialTransactionType = true;
            MainDS.ATransactionType.Rows.Add(transactionTypeRow);
            transactionTypeRow = MainDS.ATransactionType.NewRowTyped();
            transactionTypeRow.LedgerNumber = ANewLedgerNumber;
            transactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            transactionTypeRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.REVAL.ToString();
            transactionTypeRow.DebitAccountCode = "5003";
            transactionTypeRow.CreditAccountCode = "5003";
            transactionTypeRow.TransactionTypeDescription = "Foreign Exchange Revaluation";
            transactionTypeRow.SpecialTransactionType = true;
            MainDS.ATransactionType.Rows.Add(transactionTypeRow);
            transactionTypeRow = MainDS.ATransactionType.NewRowTyped();
            transactionTypeRow.LedgerNumber = ANewLedgerNumber;
            transactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            transactionTypeRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
            transactionTypeRow.DebitAccountCode = MFinanceConstants.ACCOUNT_BAL_SHT;
            transactionTypeRow.CreditAccountCode = MFinanceConstants.ACCOUNT_BAL_SHT;
            transactionTypeRow.TransactionTypeDescription = "Standard Journal";
            transactionTypeRow.SpecialTransactionType = false;
            MainDS.ATransactionType.Rows.Add(transactionTypeRow);
            transactionTypeRow = MainDS.ATransactionType.NewRowTyped();
            transactionTypeRow.LedgerNumber = ANewLedgerNumber;
            transactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.GR.ToString();
            transactionTypeRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.GR.ToString();
            transactionTypeRow.DebitAccountCode = "CASH";
            transactionTypeRow.CreditAccountCode = "GIFT";
            transactionTypeRow.TransactionTypeDescription = "Gift Receipting";
            transactionTypeRow.SpecialTransactionType = true;
            MainDS.ATransactionType.Rows.Add(transactionTypeRow);
            transactionTypeRow = MainDS.ATransactionType.NewRowTyped();
            transactionTypeRow.LedgerNumber = ANewLedgerNumber;
            transactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.AP.ToString();
            transactionTypeRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.INV.ToString();
            transactionTypeRow.DebitAccountCode = MFinanceConstants.ACCOUNT_BAL_SHT;
            transactionTypeRow.CreditAccountCode = MFinanceConstants.ACCOUNT_CREDITORS;
            transactionTypeRow.TransactionTypeDescription = "Input Creditor's Invoice";
            transactionTypeRow.SpecialTransactionType = true;
            MainDS.ATransactionType.Rows.Add(transactionTypeRow);

            AValidLedgerNumberTable validLedgerNumberTable = AValidLedgerNumberAccess.LoadByPrimaryKey(ANewLedgerNumber, PartnerKey, null);

            if (validLedgerNumberTable.Rows.Count == 0)
            {
                AValidLedgerNumberRow validLedgerNumberRow = MainDS.AValidLedgerNumber.NewRowTyped();
                validLedgerNumberRow.PartnerKey = PartnerKey;
                validLedgerNumberRow.LedgerNumber = ANewLedgerNumber;

                // TODO can we assume that ledger 4 is used for international clearing?
                // but in the empty database, that ledger and therefore p_partner with key 4000000 does not exist
                // validLedgerNumberRow.IltProcessingCentre = 4000000;

                validLedgerNumberRow.CostCentreCode = (ANewLedgerNumber * 100).ToString("0000");
                MainDS.AValidLedgerNumber.Rows.Add(validLedgerNumberRow);
            }

            ACostCentreTypesRow costCentreTypesRow = MainDS.ACostCentreTypes.NewRowTyped();
            costCentreTypesRow.LedgerNumber = ANewLedgerNumber;
            costCentreTypesRow.CostCentreType = "Local";
            costCentreTypesRow.Deletable = false;
            MainDS.ACostCentreTypes.Rows.Add(costCentreTypesRow);
            costCentreTypesRow = MainDS.ACostCentreTypes.NewRowTyped();
            costCentreTypesRow.LedgerNumber = ANewLedgerNumber;
            costCentreTypesRow.CostCentreType = "Foreign";
            costCentreTypesRow.Deletable = false;
            MainDS.ACostCentreTypes.Rows.Add(costCentreTypesRow);

            AMotivationGroupRow motivationGroupRow = MainDS.AMotivationGroup.NewRowTyped();
            motivationGroupRow.LedgerNumber = ANewLedgerNumber;
            motivationGroupRow.MotivationGroupCode = "GIFT";
            motivationGroupRow.MotivationGroupDescLocal = Catalog.GetString("Gifts");
            motivationGroupRow.MotivationGroupDescription = motivationGroupRow.MotivationGroupDescLocal;
            MainDS.AMotivationGroup.Rows.Add(motivationGroupRow);

            AMotivationDetailRow motivationDetailRow = MainDS.AMotivationDetail.NewRowTyped();
            motivationDetailRow.LedgerNumber = ANewLedgerNumber;
            motivationDetailRow.MotivationGroupCode = "GIFT";
            motivationDetailRow.MotivationDetailCode = "SUPPORT";
            motivationDetailRow.MotivationDetailDesc = Catalog.GetString("Personal Support");
            motivationDetailRow.MotivationDetailDescLocal = motivationDetailRow.MotivationDetailDesc;
            motivationDetailRow.AccountCode = "0100";
            motivationDetailRow.CostCentreCode = (ANewLedgerNumber * 100).ToString("0000");
            MainDS.AMotivationDetail.Rows.Add(motivationDetailRow);

            motivationDetailRow = MainDS.AMotivationDetail.NewRowTyped();
            motivationDetailRow.LedgerNumber = ANewLedgerNumber;
            motivationDetailRow.MotivationGroupCode = "GIFT";
            motivationDetailRow.MotivationDetailCode = "FIELD";
            motivationDetailRow.MotivationDetailDesc = Catalog.GetString("Gifts for Field");
            motivationDetailRow.MotivationDetailDescLocal = motivationDetailRow.MotivationDetailDesc;
            motivationDetailRow.AccountCode = "0200";
            motivationDetailRow.CostCentreCode = (ANewLedgerNumber * 100).ToString("0000");
            MainDS.AMotivationDetail.Rows.Add(motivationDetailRow);

            motivationDetailRow = MainDS.AMotivationDetail.NewRowTyped();
            motivationDetailRow.LedgerNumber = ANewLedgerNumber;
            motivationDetailRow.MotivationGroupCode = "GIFT";
            motivationDetailRow.MotivationDetailCode = "KEYMIN";
            motivationDetailRow.MotivationDetailDesc = Catalog.GetString("Key Ministry Gift");
            motivationDetailRow.MotivationDetailDescLocal = motivationDetailRow.MotivationDetailDesc;
            motivationDetailRow.AccountCode = "0400";
            motivationDetailRow.CostCentreCode = (ANewLedgerNumber * 100).ToString("0000");
            MainDS.AMotivationDetail.Rows.Add(motivationDetailRow);

            ImportDefaultAccountHierarchy(ref MainDS, ANewLedgerNumber);
            ImportDefaultCostCentreHierarchy(ref MainDS, ANewLedgerNumber, ALedgerName);

            // TODO: modify UI navigation yml file etc?
            // TODO: permissions for which users?

            TSubmitChangesResult result = GLSetupTDSAccess.SubmitChanges(MainDS, out AVerificationResult);

            if (result == TSubmitChangesResult.scrOK)
            {
                // give the current user access permissions to this new ledger
                SUserModuleAccessPermissionTable moduleAccessPermissionTable = new SUserModuleAccessPermissionTable();

                SUserModuleAccessPermissionRow moduleAccessPermissionRow = moduleAccessPermissionTable.NewRowTyped();
                moduleAccessPermissionRow.UserId = UserInfo.GUserInfo.UserID;
                moduleAccessPermissionRow.ModuleId = "LEDGER" + ANewLedgerNumber.ToString("0000");
                moduleAccessPermissionRow.CanAccess = true;
                moduleAccessPermissionTable.Rows.Add(moduleAccessPermissionRow);

                TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

                try
                {
                    if (!SUserModuleAccessPermissionAccess.SubmitChanges(moduleAccessPermissionTable, Transaction, out AVerificationResult))
                    {
                        return false;
                    }

                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                finally
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return result == TSubmitChangesResult.scrOK;
        }

        /// <summary>
        /// deletes the complete ledger, with all finance data. useful for testing purposes
        /// </summary>
        [RequireModulePermission("FINANCE-3")]
        public static bool DeleteLedger(Int32 ALedgerNumber, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Deleting ledger"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Deleting ledger"),
                20);

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                OdbcParameter[] ledgerparameter = new OdbcParameter[] {
                    new OdbcParameter("ledgernumber", OdbcType.Int)
                };
                ledgerparameter[0].Value = ALedgerNumber;

                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    String.Format("DELETE FROM PUB_{0} WHERE {1} = 'LEDGER{2:0000}'",
                        SUserModuleAccessPermissionTable.GetTableDBName(),
                        SUserModuleAccessPermissionTable.GetModuleIdDBName(),
                        ALedgerNumber),
                    Transaction);

                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    String.Format(
                        "DELETE FROM PUB_{0} AS GLMP WHERE EXISTS (SELECT * FROM PUB_{1} AS GLM WHERE GLM.a_glm_sequence_i = GLMP.a_glm_sequence_i AND GLM.a_ledger_number_i = ?)",
                        AGeneralLedgerMasterPeriodTable.GetTableDBName(),
                        AGeneralLedgerMasterTable.GetTableDBName()),
                    Transaction, ledgerparameter);

                string[] tablenames = new string[] {
                    AValidLedgerNumberTable.GetTableDBName(),
                         AProcessedFeeTable.GetTableDBName(),
                         AGeneralLedgerMasterTable.GetTableDBName(),
                         AMotivationDetailFeeTable.GetTableDBName(),

                         ARecurringGiftDetailTable.GetTableDBName(),
                         ARecurringGiftTable.GetTableDBName(),
                         ARecurringGiftBatchTable.GetTableDBName(),

                         AGiftDetailTable.GetTableDBName(),
                         AGiftTable.GetTableDBName(),
                         AGiftBatchTable.GetTableDBName(),

                         ATransAnalAttribTable.GetTableDBName(),
                         ATransactionTable.GetTableDBName(),
                         AJournalTable.GetTableDBName(),
                         ABatchTable.GetTableDBName(),

                         ARecurringTransAnalAttribTable.GetTableDBName(),
                         ARecurringTransactionTable.GetTableDBName(),
                         ARecurringJournalTable.GetTableDBName(),
                         ARecurringBatchTable.GetTableDBName(),

                         AFreeformAnalysisTable.GetTableDBName(),

                         AEpDocumentPaymentTable.GetTableDBName(),
                         AEpPaymentTable.GetTableDBName(),

                         AApAnalAttribTable.GetTableDBName(),
                         AApDocumentPaymentTable.GetTableDBName(),
                         AApPaymentTable.GetTableDBName(),
                         AApDocumentDetailTable.GetTableDBName(),
                         AApDocumentTable.GetTableDBName(),

                         AMotivationDetailTable.GetTableDBName(),
                         AMotivationGroupTable.GetTableDBName(),
                         AFeesReceivableTable.GetTableDBName(),
                         AFeesPayableTable.GetTableDBName(),
                         ACostCentreTable.GetTableDBName(),
                         ATransactionTypeTable.GetTableDBName(),
                         AAccountPropertyTable.GetTableDBName(),
                         AAccountHierarchyDetailTable.GetTableDBName(),
                         AAccountHierarchyTable.GetTableDBName(),
                         AAccountTable.GetTableDBName(),
                         ASystemInterfaceTable.GetTableDBName(),
                         AAccountingSystemParameterTable.GetTableDBName(),
                         ACostCentreTypesTable.GetTableDBName(),

                         ALedgerInitFlagTable.GetTableDBName(),
                         ATaxTableTable.GetTableDBName(),
                         AEpAccountTable.GetTableDBName(),

                         AAccountingPeriodTable.GetTableDBName(),

                         SGroupLedgerTable.GetTableDBName()
                };

                foreach (string table in tablenames)
                {
                    DBAccess.GDBAccessObj.ExecuteNonQuery(
                        String.Format("DELETE FROM PUB_{0} WHERE a_ledger_number_i = ?", table),
                        Transaction, ledgerparameter);
                }

                ALedgerAccess.DeleteByPrimaryKey(ALedgerNumber, Transaction);

                // remove from the list of sites when creating new partners
                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    String.Format("DELETE FROM PUB_{0} WHERE p_partner_key_n = {1}",
                        PPartnerLedgerTable.GetTableDBName(),
                        Convert.ToInt64(ALedgerNumber) * 1000000),
                    Transaction);

                if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                {
                    throw new Exception("Deletion of Ledger was cancelled by the user");
                }

                DBAccess.GDBAccessObj.CommitTransaction();

                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());

                AVerificationResult = new TVerificationResultCollection();
                AVerificationResult.Add(new TVerificationResult(
                        "Problems deleting ledger " + ALedgerNumber.ToString(),
                        e.Message,
                        "Cannot delete ledger",
                        string.Empty,
                        TResultSeverity.Resv_Critical,
                        Guid.Empty));
                DBAccess.GDBAccessObj.RollbackTransaction();
                TProgressTracker.CancelJob(DomainManager.GClientID.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// get the ledger numbers that are available for the current user
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static ALedgerTable GetAvailableLedgers()
        {
            // TODO check for permissions of the current user
            StringCollection Fields = new StringCollection();

            Fields.Add(ALedgerTable.GetLedgerNameDBName());
            Fields.Add(ALedgerTable.GetLedgerNumberDBName());
            return ALedgerAccess.LoadAll(Fields, null, null, 0, 0);
        }

        /// <summary>
        /// Load  the table AFREEFORMANALSYSIS
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static AFreeformAnalysisTable LoadAFreeformAnalysis(Int32 ALedgerNumber)
        {
            GLSetupTDS MainDS = new GLSetupTDS();

            AFreeformAnalysisAccess.LoadViaALedger(MainDS, ALedgerNumber, null);

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();
            AFreeformAnalysisTable myAT = MainDS.AFreeformAnalysis;
            return myAT;
        }

        /// <summary>
        /// Check if a value in  AFREEFORMANALSYSIS cand be deleted (count the references in ATRansANALATTRIB)
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static int CheckDeleteAFreeformAnalysis(Int32 ALedgerNumber, String ATypeCode, String AAnalysisValue)
        {
            return ATransAnalAttribAccess.CountViaAFreeformAnalysis(ALedgerNumber, ATypeCode, AAnalysisValue, null);
        }

        /// <summary>
        /// Check if a TypeCode in  AnalysisType can be deleted (count the references in ATRansAnalysisAtrributes)
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static int CheckDeleteAAnalysisType(String ATypeCode)
        {
            return AAnalysisAttributeAccess.CountViaAAnalysisType(ATypeCode, null);
        }

        /// <summary>
        /// Check if a account code for Ledger ALedgerNumber has analysis attributes set up
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool HasAccountSetupAnalysisAttributes(Int32 ALedgerNumber, String AAccountCode)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            bool HasAccountAnalysisAttributes = false;
            
            HasAccountAnalysisAttributes = (AAnalysisAttributeAccess.CountViaAAccount(ALedgerNumber, AAccountCode, Transaction) > 0);

            DBAccess.GDBAccessObj.RollbackTransaction();
            
            return HasAccountAnalysisAttributes;
        }
        
        //
        //    Rename Account: to rename an AccountCode, we need to update lots of values all over the database:

        private static void UpdateAccountField(String ATblName,
            String AFldName,
            String AOldName,
            String ANewName,
            Int32 ALedgerNumber,
            TDBTransaction ATransaction,
            ref String AttemptedOperation)
        {
            AttemptedOperation = "Rename " + AFldName + " in " + ATblName;
            String QuerySql =
                "UPDATE PUB_" + ATblName +
                " SET " + AFldName + "='" + ANewName +
                "' WHERE " + AFldName + "='" + AOldName + "'";

            if (ALedgerNumber >= 0)
            {
                QuerySql += (" AND a_ledger_number_i=" + ALedgerNumber);
            }

            DBAccess.GDBAccessObj.ExecuteNonQuery(QuerySql, ATransaction);
        }

        /// <summary>
        /// Use this new account code instead of that old one.
        /// THIS RENAMES THE FIELD IN LOTS OF PLACES!
        /// </summary>
        /// <param name="AOldCode"></param>
        /// <param name="ANewCode"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="VerificationResults"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool RenameAccountCode(String AOldCode,
            String ANewCode,
            Int32 ALedgerNumber,
            out TVerificationResultCollection VerificationResults)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            Boolean RenameComplete = false;
            String VerificationContext = "Rename Account Code";
            String AttemptedOperation = "";

            VerificationResults = new TVerificationResultCollection();
            try
            {
                //
                // First check whether this new code is available for use!
                //
                AAccountTable TempAccountTbl = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, ANewCode, Transaction);

                if (TempAccountTbl.Rows.Count > 0)
                {
                    VerificationResults.Add(new TVerificationResult(VerificationContext, "Target name is already present",
                            TResultSeverity.Resv_Critical));
                    return false;
                }

                TempAccountTbl = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, AOldCode, Transaction);

                if (TempAccountTbl.Rows.Count != 1)
                {
                    VerificationResults.Add(new TVerificationResult(VerificationContext, "Existing name not accessible",
                            TResultSeverity.Resv_Critical));
                    return false;
                }

                AAccountRow PrevAccountRow = TempAccountTbl[0];
                AAccountRow NewAccountRow = TempAccountTbl.NewRowTyped();
                DataUtilities.CopyAllColumnValues(PrevAccountRow, NewAccountRow);
                NewAccountRow.AccountCode = ANewCode;
                TempAccountTbl.Rows.Add(NewAccountRow);

                if (!AAccountAccess.SubmitChanges(TempAccountTbl, Transaction, out VerificationResults))
                {
                    return false;
                }

                TempAccountTbl.AcceptChanges();

                UpdateAccountField("a_ledger", "a_creditor_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_debtor_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_fa_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_ilt_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_po_accrual_gl_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_profit_loss_gl_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_purchase_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_sales_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_so_accrual_gl_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_stock_adj_gl_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_stock_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_tax_input_gl_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_tax_output_gl_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_cost_of_sales_gl_account_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_forex_gains_losses_account_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_ret_earnings_gl_account_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_stock_accrual_gl_account_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_transaction", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_transaction",
                    "a_primary_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);

/*
 *              UpdateAccountField ("a_this_year_old_transaction","a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
 *              UpdateAccountField ("a_this_year_old_transaction","a_primary_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
 *              UpdateAccountField ("a_previous_year_transaction","a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
 *              UpdateAccountField ("a_previous_year_transaction","a_primary_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
 */
                UpdateAccountField("a_fees_receivable", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_fees_receivable", "a_dr_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_fees_payable", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_fees_payable", "a_dr_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_transaction_type",
                    "a_balancing_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_transaction_type",
                    "a_credit_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_transaction_type",
                    "a_debit_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);

                AAnalysisAttributeTable TempAnalAttrTbl = AAnalysisAttributeAccess.LoadViaAAccount(ALedgerNumber, AOldCode, Transaction);

                foreach (AAnalysisAttributeRow OldAnalAttribRow in TempAnalAttrTbl.Rows)
                {
                    // "a_analysis_attribute"  is the referrent in foreign keys, so I can't just go changing it - I need to make a copy?
                    AAnalysisAttributeRow NewAnalAttribRow = TempAnalAttrTbl.NewRowTyped();
                    DataUtilities.CopyAllColumnValues(OldAnalAttribRow, NewAnalAttribRow);
                    NewAnalAttribRow.AccountCode = ANewCode;
                    TempAnalAttrTbl.Rows.Add(NewAnalAttribRow);

                    if (!AAnalysisAttributeAccess.SubmitChanges(TempAnalAttrTbl, Transaction, out VerificationResults))
                    {
                        return false;
                    }

                    TempAnalAttrTbl.AcceptChanges();

                    UpdateAccountField("a_trans_anal_attrib",
                        "a_account_code_c",
                        AOldCode,
                        ANewCode,
                        ALedgerNumber,
                        Transaction,
                        ref AttemptedOperation);
                    UpdateAccountField("a_recurring_trans_anal_attrib",
                        "a_account_code_c",
                        AOldCode,
                        ANewCode,
                        ALedgerNumber,
                        Transaction,
                        ref AttemptedOperation);
                    UpdateAccountField("a_thisyearold_trans_anal_attrib",
                        "a_account_code_c",
                        AOldCode,
                        ANewCode,
                        ALedgerNumber,
                        Transaction,
                        ref AttemptedOperation);
                    UpdateAccountField("a_prev_year_trans_anal_attrib",
                        "a_account_code_c",
                        AOldCode,
                        ANewCode,
                        ALedgerNumber,
                        Transaction,
                        ref AttemptedOperation);
                    UpdateAccountField("a_ap_anal_attrib", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);

                    OldAnalAttribRow.Delete();

                    if (!AAnalysisAttributeAccess.SubmitChanges(TempAnalAttrTbl, Transaction, out VerificationResults))
                    {
                        return false;
                    }
                }

                UpdateAccountField("a_suspense_account",
                    "a_suspense_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_motivation_detail", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_recurring_transaction",
                    "a_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_gift_batch", "a_bank_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_recurring_gift_batch",
                    "a_bank_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_ap_document_detail", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ap_document", "a_ap_account_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ap_payment", "a_bank_account_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ep_payment", "a_bank_account_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);

                UpdateAccountField("a_ap_supplier", "a_default_ap_account_c", AOldCode, ANewCode, -1, Transaction, ref AttemptedOperation); // There's no Ledger associated with this field.

                UpdateAccountField("a_budget", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_general_ledger_master",
                    "a_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_account_hierarchy_detail",
                    "a_reporting_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_account_hierarchy_detail",
                    "a_account_code_to_report_to_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_account_hierarchy",
                    "a_root_account_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_account_property", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                //              UpdateAccountField("a_fin_statement_group","a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);

                PrevAccountRow.Delete();

                if (!AAccountAccess.SubmitChanges(TempAccountTbl, Transaction, out VerificationResults))
                {
                    return false;
                }

                RenameComplete = true;
            }
            //
            // There's no "catch" - if any of the calls above fails (with an SQL problem),
            // the server task will fail, and cause a descriptive exception on the client.
            // (And the VerificationResults might also contain a useful string because of "finally" below.)
            //
            finally
            {
                if (RenameComplete)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    if (AttemptedOperation != "")
                    {
                        VerificationResults.Add(new TVerificationResult(VerificationContext, "Problem " + AttemptedOperation,
                                TResultSeverity.Resv_Critical));
                    }

                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return RenameComplete;
        }
    }
}