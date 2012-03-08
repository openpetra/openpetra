// auto generated with nant generateORM
// Do not modify this file manually!
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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

namespace Ict.Petra.Shared.MFinance
{
    /// <summary>
    /// Enums holding the possible cacheable tables for the Petra Finance Module, specifically Finance submodule
    /// </summary>
    public enum TCacheableFinanceTablesEnum
    {
        /// <summary>
        /// Contains types of analysis attributes
        /// </summary>
        AnalysisTypeList,

        /// <summary>
        /// Values for the analysis types
        /// </summary>
        FreeformAnalysisList,

        /// <summary>
        /// Relationsship between account and analysis type
        /// </summary>
        AnalysisAttributeList,

        /// <summary>
        /// Used for initial set up of budgets, for how to calculate amounts for each period.  Some possible types are adhoc,same,percentage of annual.
        /// </summary>
        BudgetTypeList,

        /// <summary>
        /// Stores standard and user-defined cost centre types.  For example: Foreign, Local.
        /// </summary>
        CostCentreTypesList,

        /// <summary>
        /// Where Petra supports it a cross reference between a file and destination can be established for automatic distribution.
        /// </summary>
        EmailDestinationList,

        /// <summary>
        /// Special payment programs the donor may give money through. (ie, Gift Aid in the UK)
        /// </summary>
        MethodOfGivingList,

        /// <summary>
        /// Media types of money received. Eg: Cash, Check Credit Card
        /// </summary>
        MethodOfPaymentList,

        /// <summary>
        /// This is used to track a partner's reason for contacting the organisation/sending money. Divided into Motivation Detail codes
        /// </summary>
        MotivationGroupList,

        /// <summary>
        /// list of motivations (for gifts)
        /// </summary>
        MotivationList,

        /// <summary>
        /// Fees owed to another ledger. (e.g. admin grant)
        /// </summary>
        FeesPayableList,

        /// <summary>
        /// Charges to collect from other ledgers. (e.g. office admin fee)
        /// </summary>
        FeesReceivableList,
        /// <summary>
        /// todoComment
        /// </summary>
        AccountingPeriodList,

        /// <summary>
        /// names of available ledgers
        /// </summary>
        LedgerNameList,

        /// <summary>
        /// ledger details (number of accounting periods, current period, etc)
        /// </summary>
        LedgerDetails,

        /// <summary>
        /// todoComment
        /// </summary>
        CostCentreList,

        /// <summary>
        /// todoComment
        /// </summary>
        AccountList,

        /// <summary>
        /// todoComment
        /// </summary>
        AccountHierarchyList
    };
}
