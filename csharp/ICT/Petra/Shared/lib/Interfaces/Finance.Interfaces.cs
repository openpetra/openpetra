/* Auto generated with nant generateGlue, based on csharp\ICT\Petra\Definitions\NamespaceHierarchy.xml
 * and the Server c# files (eg. UIConnector implementations)
 * Do not change this file manually.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared.Interfaces.MFinance.AccountsPayable;
using Ict.Petra.Shared.Interfaces.MFinance.Budget;
using Ict.Petra.Shared.Interfaces.MFinance.Cacheable;
using Ict.Petra.Shared.Interfaces.MFinance.Gift;
using Ict.Petra.Shared.Interfaces.MFinance.GL;
using Ict.Petra.Shared.Interfaces.MFinance.ICH;
using Ict.Petra.Shared.Interfaces.MFinance.PeriodEnd;
using Ict.Petra.Shared.Interfaces.MFinance.Reporting;
using Ict.Petra.Shared.Interfaces.MFinance.Setup;
using Ict.Petra.Shared.Interfaces.MFinance.AccountsPayable.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.AccountsPayable.WebConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Budget.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Gift.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Gift.WebConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.GL.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.ICH.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.PeriodEnd.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Reporting.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Setup.UIConnectors;
#region ManualCode
using System.Xml;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.Interfaces.AsynchronousExecution;
#endregion
namespace Ict.Petra.Shared.Interfaces.MFinance
{
    /// <summary>auto generated</summary>
    public interface IMFinanceNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IAccountsPayableNamespace AccountsPayable
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IBudgetNamespace Budget
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        ICacheableNamespace Cacheable
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IGiftNamespace Gift
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IGLNamespace GL
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IICHNamespace ICH
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IPeriodEndNamespace PeriodEnd
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IReportingNamespace Reporting
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        ISetupNamespace Setup
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.AccountsPayable
{
    /// <summary>auto generated</summary>
    public interface IAccountsPayableNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IAccountsPayableUIConnectorsNamespace UIConnectors
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IAccountsPayableWebConnectorsNamespace WebConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.AccountsPayable.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IAccountsPayableUIConnectorsNamespace : IInterface
    {
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TFindUIConnector)</summary>
        IAccountsPayableUIConnectorsFind Find();
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TSupplierEditUIConnector)</summary>
        IAccountsPayableUIConnectorsSupplierEdit SupplierEdit();
        /// <summary>auto generated from Connector constructor and GetData (Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TSupplierEditUIConnector)</summary>
        IAccountsPayableUIConnectorsSupplierEdit SupplierEdit(ref AccountsPayableTDS ADataSet,
                                                              Int64 APartnerKey);
    }

    /// <summary>auto generated</summary>
    public interface IAccountsPayableUIConnectorsFind : IInterface
    {
        /// <summary>auto generated from Connector property (Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TFindUIConnector)</summary>
        IAsynchronousExecutionProgress AsyncExecProgress
        {
            get;
        }

        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TFindUIConnector)</summary>
        void FindSupplier(DataTable ACriteriaData);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TFindUIConnector)</summary>
        void FindInvoices(DataTable ACriteriaData);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TFindUIConnector)</summary>
        void PerformSearch(DataTable ACriteriaData);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TFindUIConnector)</summary>
        void StopSearch(object ASender,
                        EventArgs AArgs);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TFindUIConnector)</summary>
        DataTable GetDataPagedResult(System.Int16 APage,
                                     System.Int16 APageSize,
                                     out System.Int32 ATotalRecords,
                                     out System.Int16 ATotalPages);
    }

    /// <summary>auto generated</summary>
    public interface IAccountsPayableUIConnectorsSupplierEdit : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TSupplierEditUIConnector)</summary>
        bool CanFindSupplier(Int64 APartnerKey);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TSupplierEditUIConnector)</summary>
        AccountsPayableTDS GetData(Int64 APartnerKey);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TSupplierEditUIConnector)</summary>
        TSubmitChangesResult SubmitChanges(ref AccountsPayableTDS AInspectDS,
                                           out TVerificationResultCollection AVerificationResult);
    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.AccountsPayable.WebConnectors
{
    /// <summary>auto generated</summary>
    public interface IAccountsPayableWebConnectorsNamespace : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors.TTransactionWebConnector)</summary>
        AccountsPayableTDS LoadAApDocument(Int32 ALedgerNumber,
                                           Int32 AAPNumber);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors.TTransactionWebConnector)</summary>
        AccountsPayableTDS CreateAApDocument(Int32 ALedgerNumber,
                                             Int64 APartnerKey,
                                             bool ACreditNoteOrInvoice);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors.TTransactionWebConnector)</summary>
        TSubmitChangesResult SaveAApDocument(ref AccountsPayableTDS AInspectDS,
                                             out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors.TTransactionWebConnector)</summary>
        AccountsPayableTDS CreateAApDocumentDetail(Int32 ALedgerNumber,
                                                   Int32 AApNumber,
                                                   string AApSupplier_DefaultExpAccount,
                                                   string AApSupplier_DefaultCostCentre,
                                                   double AAmount,
                                                   Int32 ALastDetailNumber);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors.TTransactionWebConnector)</summary>
        AccountsPayableTDS FindAApDocument(Int32 ALedgerNumber,
                                           Int64 ASupplierKey,
                                           string ADocumentStatus,
                                           bool IsCreditNoteNotInvoice,
                                           bool AHideAgedTransactions);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors.TTransactionWebConnector)</summary>
        bool PostAPDocuments(Int32 ALedgerNumber,
                             List <Int32> AAPDocumentNumbers,
                             DateTime APostingDate,
                             out TVerificationResultCollection AVerifications);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors.TTransactionWebConnector)</summary>
        bool PostAPPayments(AccountsPayableTDSAApPaymentTable APayments,
                            AccountsPayableTDSAApDocumentPaymentTable ADocumentPayments,
                            DateTime APostingDate,
                            out TVerificationResultCollection AVerifications);
    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.Budget
{
    /// <summary>auto generated</summary>
    public interface IBudgetNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IBudgetUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.Budget.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IBudgetUIConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.Cacheable
{
    /// <summary>auto generated</summary>
    public interface ICacheableNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.Cacheable.TCacheableNamespace)</summary>
        System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MFinance.TCacheableFinanceTablesEnum ACacheableTable,
                                                System.String AHashCode,
                                                out System.Type AType);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.Cacheable.TCacheableNamespace)</summary>
        System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MFinance.TCacheableFinanceTablesEnum ACacheableTable,
                                                System.String AHashCode,
                                                System.Int32 ALedgerNumber,
                                                out System.Type AType);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.Cacheable.TCacheableNamespace)</summary>
        void RefreshCacheableTable(Ict.Petra.Shared.MFinance.TCacheableFinanceTablesEnum ACacheableTable);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.Cacheable.TCacheableNamespace)</summary>
        void RefreshCacheableTable(Ict.Petra.Shared.MFinance.TCacheableFinanceTablesEnum ACacheableTable,
                                   out System.Data.DataTable ADataTable);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.Cacheable.TCacheableNamespace)</summary>
        void RefreshCacheableTable(Ict.Petra.Shared.MFinance.TCacheableFinanceTablesEnum ACacheableTable,
                                   System.Int32 ALedgerNumber);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.Cacheable.TCacheableNamespace)</summary>
        void RefreshCacheableTable(Ict.Petra.Shared.MFinance.TCacheableFinanceTablesEnum ACacheableTable,
                                   System.Int32 ALedgerNumber,
                                   out System.Data.DataTable ADataTable);
    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.Gift
{
    /// <summary>auto generated</summary>
    public interface IGiftNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IGiftUIConnectorsNamespace UIConnectors
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IGiftWebConnectorsNamespace WebConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.Gift.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IGiftUIConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.Gift.WebConnectors
{
    /// <summary>auto generated</summary>
    public interface IGiftWebConnectorsNamespace : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector)</summary>
        GiftBatchTDS CreateAGiftBatch(Int32 ALedgerNumber);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector)</summary>
        GiftBatchTDS LoadAGiftBatch(Int32 ALedgerNumber);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector)</summary>
        GiftBatchTDS LoadTransactions(Int32 ALedgerNumber,
                                      Int32 ABatchNumber);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector)</summary>
        TSubmitChangesResult SaveGiftBatchTDS(ref GiftBatchTDS AInspectDS,
                                              out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector)</summary>
        bool PostGiftBatch(Int32 ALedgerNumber,
                           Int32 ABatchNumber,
                           out TVerificationResultCollection AVerifications);
    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.GL
{
    /// <summary>auto generated</summary>
    public interface IGLNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IGLUIConnectorsNamespace UIConnectors
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IGLWebConnectorsNamespace WebConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.GL.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IGLUIConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.GL.WebConnectors
{
    /// <summary>auto generated</summary>
    public interface IGLWebConnectorsNamespace : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.GL.WebConnectors.TGLSetupWebConnector)</summary>
        GLSetupTDS LoadAccountHierarchies(Int32 ALedgerNumber);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.GL.WebConnectors.TGLSetupWebConnector)</summary>
        GLSetupTDS LoadCostCentreHierarchy(Int32 ALedgerNumber);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.GL.WebConnectors.TGLSetupWebConnector)</summary>
        TSubmitChangesResult SaveGLSetupTDS(ref GLSetupTDS AInspectDS,
                                            out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.GL.WebConnectors.TGLSetupWebConnector)</summary>
        string ExportAccountHierarchy(Int32 ALedgerNumber,
                                      string AAccountHierarchyName);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.GL.WebConnectors.TGLSetupWebConnector)</summary>
        string ExportCostCentreHierarchy(Int32 ALedgerNumber);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.GL.WebConnectors.TGLSetupWebConnector)</summary>
        bool ImportAccountHierarchy(Int32 ALedgerNumber,
                                    string AHierarchyName,
                                    string AXmlAccountHierarchy);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.GL.WebConnectors.TGLSetupWebConnector)</summary>
        bool ImportCostCentreHierarchy(Int32 ALedgerNumber,
                                       string AXmlHierarchy);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.GL.WebConnectors.TGLSetupWebConnector)</summary>
        bool ImportNewLedger(Int32 ALedgerNumber,
                             string AXmlAccountHierarchy,
                             string AXmlCostCentreHierarchy,
                             string AXmlInitialBalances);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.GL.WebConnectors.TGLSetupWebConnector)</summary>
        bool CanDeleteAccount(Int32 ALedgerNumber,
                              string AAccountCode);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.GL.WebConnectors.TGLWebConnectorsNamespace)</summary>
        bool GetCurrentPeriodDates(Int32 ALedgerNumber,
                                   out DateTime AStartDate,
                                   out DateTime AEndDate);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.GL.WebConnectors.TGLWebConnectorsNamespace)</summary>
        bool GetCurrentPostingRangeDates(Int32 ALedgerNumber,
                                         out DateTime AStartDateCurrentPeriod,
                                         out DateTime AEndDateLastForwardingPeriod);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.GL.WebConnectors.TGLWebConnectorsNamespace)</summary>
        GLBatchTDS CreateABatch(Int32 ALedgerNumber);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.GL.WebConnectors.TGLWebConnectorsNamespace)</summary>
        GLBatchTDS LoadABatch(Int32 ALedgerNumber);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.GL.WebConnectors.TGLWebConnectorsNamespace)</summary>
        GLBatchTDS LoadAJournal(Int32 ALedgerNumber,
                                Int32 ABatchNumber);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.GL.WebConnectors.TGLWebConnectorsNamespace)</summary>
        GLBatchTDS LoadATransaction(Int32 ALedgerNumber,
                                    Int32 ABatchNumber,
                                    Int32 AJournalNumber);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.GL.WebConnectors.TGLWebConnectorsNamespace)</summary>
        TSubmitChangesResult SaveGLBatchTDS(ref GLBatchTDS AInspectDS,
                                            out TVerificationResultCollection AVerificationResult);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.GL.WebConnectors.TGLWebConnectorsNamespace)</summary>
        bool PostGLBatch(Int32 ALedgerNumber,
                         Int32 ABatchNumber,
                         out TVerificationResultCollection AVerifications);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.GL.WebConnectors.TGLWebConnectorsNamespace)</summary>
        string GetStandardCostCentre(Int32 ALedgerNumber);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.GL.WebConnectors.TGLWebConnectorsNamespace)</summary>
        double GetDailyExchangeRate(string ACurrencyFrom,
                                    string ACurrencyTo,
                                    DateTime ADateEffective);
    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.ICH
{
    /// <summary>auto generated</summary>
    public interface IICHNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IICHUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.ICH.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IICHUIConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.PeriodEnd
{
    /// <summary>auto generated</summary>
    public interface IPeriodEndNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IPeriodEndUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.PeriodEnd.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IPeriodEndUIConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.Reporting
{
    /// <summary>auto generated</summary>
    public interface IReportingNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IReportingUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.Reporting.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IReportingUIConnectorsNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.Reporting.UIConnectors.TReportingUIConnectorsNamespace)</summary>
        void SelectLedger(System.Int32 ALedgerNr);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.Reporting.UIConnectors.TReportingUIConnectorsNamespace)</summary>
        void GetRealPeriod(System.Int32 ADiffPeriod,
                           System.Int32 AYear,
                           System.Int32 APeriod,
                           out System.Int32 ARealPeriod,
                           out System.Int32 ARealYear);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.Reporting.UIConnectors.TReportingUIConnectorsNamespace)</summary>
        void GetLedgerPeriodDetails(out System.Int32 ANumberAccountingPeriods,
                                    out System.Int32 ANumberForwardingPeriods,
                                    out System.Int32 ACurrentPeriod,
                                    out System.Int32 ACurrentYear);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.Reporting.UIConnectors.TReportingUIConnectorsNamespace)</summary>
        System.DateTime GetPeriodStartDate(System.Int32 AYear,
                                           System.Int32 ADiffPeriod,
                                           System.Int32 APeriod);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.Reporting.UIConnectors.TReportingUIConnectorsNamespace)</summary>
        System.DateTime GetPeriodEndDate(System.Int32 AYear,
                                         System.Int32 ADiffPeriod,
                                         System.Int32 APeriod);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MFinance.Instantiator.Reporting.UIConnectors.TReportingUIConnectorsNamespace)</summary>
        System.Data.DataTable GetAvailableFinancialYears(System.Int32 ADiffPeriod,
                                                         out System.String ADisplayMember,
                                                         out System.String AValueMember);
    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.Setup
{
    /// <summary>auto generated</summary>
    public interface ISetupNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        ISetupUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.Setup.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface ISetupUIConnectorsNamespace : IInterface
    {
    }

}

