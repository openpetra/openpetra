/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;

namespace Ict.Petra.Shared.MFinance
{
    /// <summary>
    /// Enums holding the possible cacheable tables for the Petra Finance Module.
    /// </summary>
    public enum TCacheableFinanceTablesEnum
    {
        /// <summary>
        /// list of budget types
        /// </summary>
        BudgetTypeList,

        /// <summary>
        /// list of cost centre types
        /// </summary>
        CostCentreTypeList,

        /// <summary>
        /// names of available ledgers
        /// </summary>
        LedgerNameList,

        /// <summary>
        /// ledger details (number of accounting periods, current period, etc)
        /// </summary>
        LedgerDetails,

        /// <summary>
        /// list of accounting periods
        /// </summary>
        AccountingPeriodList,

        /// <summary>
        /// list of cost centres
        /// </summary>
        CostCentreList,

        /// <summary>
        /// list of accounts
        /// </summary>
        AccountList,

        /// <summary>
        /// list of account hierarchies
        /// </summary>
        AccountHierarchyList,

        /// <summary>
        /// list of motivations (for gifts)
        /// </summary>
        MotivationList
    };
}