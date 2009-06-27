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
using Ict.Petra.Shared.Interfaces.MFinance.Budget.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Gift.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.GL.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.ICH.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.PeriodEnd.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Reporting.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Setup.UIConnectors;
#region ManualCode
using Ict.Petra.Shared.MFinance.AP.Data;
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

    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.AccountsPayable.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IAccountsPayableUIConnectorsNamespace : IInterface
    {
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TSupplierEditUIConnector)</summary>
        IAccountsPayableUIConnectorsSupplierEdit SupplierEdit(System.Int64 APartnerKey);
        /// <summary>auto generated from Connector constructor and GetData (Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TSupplierEditUIConnector)</summary>
        IAccountsPayableUIConnectorsSupplierEdit SupplierEdit(System.Int64 APartnerKey,
                                                              ref AccountsPayableTDS ADataSet);
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TSupplierEditUIConnector)</summary>
        IAccountsPayableUIConnectorsSupplierEdit SupplierEdit();
        /// <summary>auto generated from Connector constructor and GetData (Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TSupplierEditUIConnector)</summary>
        IAccountsPayableUIConnectorsSupplierEdit SupplierEdit(ref AccountsPayableTDS ADataSet);
    }

    /// <summary>auto generated</summary>
    public interface IAccountsPayableUIConnectorsSupplierEdit : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TSupplierEditUIConnector)</summary>
        AccountsPayableTDS GetData();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors.TSupplierEditUIConnector)</summary>
        TSubmitChangesResult SubmitChanges(ref AccountsPayableTDS AInspectDS,
                                           out TVerificationResultCollection AVerificationResult);
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

    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.Gift.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IGiftUIConnectorsNamespace : IInterface
    {
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

    }

}


namespace Ict.Petra.Shared.Interfaces.MFinance.GL.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IGLUIConnectorsNamespace : IInterface
    {
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

