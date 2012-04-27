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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;

namespace Ict.Petra.Shared
{
    /// <summary>
    /// Contains functions for the shared access to cacheable data
    /// </summary>
    public class TSharedDataCache
    {
        #region TDataCache.TMCommon
        /// <summary>
        /// todoComment
        /// </summary>
        public class TMCommon
        {
            /// <summary>
            /// Delegate for retrieving data of a cacheable Common table
            /// </summary>
            public delegate DataTable TGetCacheableCommonTable(TCacheableCommonTablesEnum ACacheableTable);

            /// <summary>
            /// Reference to the Delegate for retrieving data of a cacheable Common table
            /// </summary>
            private static TGetCacheableCommonTable FDelegateGetCacheableCommonTable;

            /// <summary>
            /// This property is used to provide a function which retrieves data of a cacheable Common table
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetCacheableCommonTable GetCacheableCommonTableDelegate
            {
                get
                {
                    return FDelegateGetCacheableCommonTable;
                }

                set
                {
                    FDelegateGetCacheableCommonTable = value;
                }
            }

            /// <summary>
            /// retrieve cacheable Common table
            /// </summary>
            /// <param name="ACacheableTable">enum that defines Common table to be returned</param>
            /// <returns></returns>
            public static DataTable GetCacheableCommonTable(TCacheableCommonTablesEnum ACacheableTable)
            {
                if (FDelegateGetCacheableCommonTable != null)
                {
                    return FDelegateGetCacheableCommonTable(ACacheableTable);
                }
                else
                {
                    throw new InvalidOperationException("Delegate 'TGetCacheableCommonTable' must be initialised before calling this Method");
                }
            }
        }
        #endregion

        #region TDataCache.TMConference
        /// <summary>
        /// todoComment
        /// </summary>
        public class TMConference
        {
            /// <summary>
            /// Delegate for retrieving data of a cacheable Conference table
            /// </summary>
            public delegate DataTable TGetCacheableConferenceTable(TCacheableConferenceTablesEnum ACacheableTable);

            /// <summary>
            /// Reference to the Delegate for retrieving data of a cacheable Conference table
            /// </summary>
            private static TGetCacheableConferenceTable FDelegateGetCacheableConferenceTable;

            /// <summary>
            /// This property is used to provide a function which retrieves data of a cacheable Conference table
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetCacheableConferenceTable GetCacheableConferenceTableDelegate
            {
                get
                {
                    return FDelegateGetCacheableConferenceTable;
                }

                set
                {
                    FDelegateGetCacheableConferenceTable = value;
                }
            }

            /// <summary>
            /// retrieve cacheable Conference table
            /// </summary>
            /// <param name="ACacheableTable">enum that defines Conference table to be returned</param>
            /// <returns></returns>
            public static DataTable GetCacheableConferenceTable(TCacheableConferenceTablesEnum ACacheableTable)
            {
                if (FDelegateGetCacheableConferenceTable != null)
                {
                    return FDelegateGetCacheableConferenceTable(ACacheableTable);
                }
                else
                {
                    throw new InvalidOperationException("Delegate 'TGetCacheableConferenceTable' must be initialised before calling this Method");
                }
            }
        }
        #endregion

        #region TDataCache.TMFinance
        /// <summary>
        /// todoComment
        /// </summary>
        public class TMFinance
        {
            /// <summary>
            /// Delegate for retrieving data of a cacheable Finance table
            /// </summary>
            public delegate DataTable TGetCacheableFinanceTable(TCacheableFinanceTablesEnum ACacheableTable);

            /// <summary>
            /// Reference to the Delegate for retrieving data of a cacheable Finance table
            /// </summary>
            private static TGetCacheableFinanceTable FDelegateGetCacheableFinanceTable;

            /// <summary>
            /// This property is used to provide a function which retrieves data of a cacheable Finance table
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetCacheableFinanceTable GetCacheableFinanceTableDelegate
            {
                get
                {
                    return FDelegateGetCacheableFinanceTable;
                }

                set
                {
                    FDelegateGetCacheableFinanceTable = value;
                }
            }

            /// <summary>
            /// retrieve cacheable Finance table
            /// </summary>
            /// <param name="ACacheableTable">enum that defines Finance table to be returned</param>
            /// <returns></returns>
            public static DataTable GetCacheableFinanceTable(TCacheableFinanceTablesEnum ACacheableTable)
            {
                if (FDelegateGetCacheableFinanceTable != null)
                {
                    return FDelegateGetCacheableFinanceTable(ACacheableTable);
                }
                else
                {
                    throw new InvalidOperationException("Delegate 'TGetCacheableFinanceTable' must be initialised before calling this Method");
                }
            }
        }
        #endregion

        #region TDataCache.TMPartner
        /// <summary>
        /// todoComment
        /// </summary>
        public class TMPartner
        {
            /// <summary>
            /// Delegate for retrieving data of a cacheable mailing table
            /// </summary>
            public delegate DataTable TGetCacheableMailingTable(TCacheableMailingTablesEnum ACacheableTable);

            /// <summary>
            /// Delegate for retrieving data of a cacheable partner table
            /// </summary>
            public delegate DataTable TGetCacheablePartnerTable(TCacheablePartnerTablesEnum ACacheableTable);

            /// <summary>
            /// Delegate for retrieving data of a cacheable subscriptions table
            /// </summary>
            public delegate DataTable TGetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum ACacheableTable);

            /// <summary>
            /// Reference to the Delegate for retrieving data of a cacheable mailing table
            /// </summary>
            private static TGetCacheableMailingTable FDelegateGetCacheableMailingTable;

            /// <summary>
            /// Reference to the Delegate for retrieving data of a cacheable partner table
            /// </summary>
            private static TGetCacheablePartnerTable FDelegateGetCacheablePartnerTable;

            /// <summary>
            /// Reference to the Delegate for retrieving data of a cacheable subscriptions table
            /// </summary>
            private static TGetCacheableSubscriptionsTable FDelegateGetCacheableSubscriptionsTable;

            /// <summary>
            /// This property is used to provide a function which retrieves data of a cacheable mailing table
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetCacheableMailingTable GetCacheableMailingTableDelegate
            {
                get
                {
                    return FDelegateGetCacheableMailingTable;
                }

                set
                {
                    FDelegateGetCacheableMailingTable = value;
                }
            }

            /// <summary>
            /// This property is used to provide a function which retrieves data of a cacheable partner table
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetCacheablePartnerTable GetCacheablePartnerTableDelegate
            {
                get
                {
                    return FDelegateGetCacheablePartnerTable;
                }

                set
                {
                    FDelegateGetCacheablePartnerTable = value;
                }
            }

            /// <summary>
            /// This property is used to provide a function which retrieves data of a cacheable Subscriptions table
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetCacheableSubscriptionsTable GetCacheableSubscriptionsTableDelegate
            {
                get
                {
                    return FDelegateGetCacheableSubscriptionsTable;
                }

                set
                {
                    FDelegateGetCacheableSubscriptionsTable = value;
                }
            }

            /// <summary>
            /// retrieve cacheable mailing table
            /// </summary>
            /// <param name="ACacheableTable">enum that defines mailing table to be returned</param>
            /// <returns></returns>
            public static DataTable GetCacheableMailingTable(TCacheableMailingTablesEnum ACacheableTable)
            {
                if (FDelegateGetCacheableMailingTable != null)
                {
                    return FDelegateGetCacheableMailingTable(ACacheableTable);
                }
                else
                {
                    throw new InvalidOperationException("Delegate 'TGetCacheableMailingTable' must be initialised before calling this Method");
                }
            }

            /// <summary>
            /// retrieve cacheable partner table
            /// </summary>
            /// <param name="ACacheableTable">enum that defines partner table to be returned</param>
            /// <returns></returns>
            public static DataTable GetCacheablePartnerTable(TCacheablePartnerTablesEnum ACacheableTable)
            {
                if (FDelegateGetCacheablePartnerTable != null)
                {
                    return FDelegateGetCacheablePartnerTable(ACacheableTable);
                }
                else
                {
                    throw new InvalidOperationException("Delegate 'TGetCacheablePartnerTable' must be initialised before calling this Method");
                }
            }

            /// <summary>
            /// retrieve cacheable Subscriptions table
            /// </summary>
            /// <param name="ACacheableTable">enum that defines Subscriptions table to be returned</param>
            /// <returns></returns>
            public static DataTable GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum ACacheableTable)
            {
                if (FDelegateGetCacheableSubscriptionsTable != null)
                {
                    return FDelegateGetCacheableSubscriptionsTable(ACacheableTable);
                }
                else
                {
                    throw new InvalidOperationException("Delegate 'TGetCacheableSubscriptionsTable' must be initialised before calling this Method");
                }
            }
        }
        #endregion

        #region TDataCache.TMPersonnel
        /// <summary>
        /// todoComment
        /// </summary>
        public class TMPersonnel
        {
            /// <summary>
            /// Delegate for retrieving data of a cacheable personnel table
            /// </summary>
            public delegate DataTable TGetCacheablePersonnelTable(TCacheablePersonTablesEnum ACacheableTable);

            /// <summary>
            /// Delegate for retrieving data of a cacheable Unit table
            /// </summary>
            public delegate DataTable TGetCacheableUnitsTable(TCacheableUnitTablesEnum ACacheableTable);

            /// <summary>
            /// Reference to the Delegate for retrieving data of a cacheable personnel table
            /// </summary>
            private static TGetCacheablePersonnelTable FDelegateGetCacheablePersonnelTable;

            /// <summary>
            /// Reference to the Delegate for retrieving data of a cacheable Units table
            /// </summary>
            private static TGetCacheableUnitsTable FDelegateGetCacheableUnitsTable;

            /// <summary>
            /// This property is used to provide a function which retrieves data of a cacheable personnel table
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetCacheablePersonnelTable GetCacheablePersonnelTableDelegate
            {
                get
                {
                    return FDelegateGetCacheablePersonnelTable;
                }

                set
                {
                    FDelegateGetCacheablePersonnelTable = value;
                }
            }

            /// <summary>
            /// This property is used to provide a function which retrieves data of a cacheable unit table
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetCacheableUnitsTable GetCacheableUnitsTableDelegate
            {
                get
                {
                    return FDelegateGetCacheableUnitsTable;
                }

                set
                {
                    FDelegateGetCacheableUnitsTable = value;
                }
            }

            /// <summary>
            /// retrieve cacheable personnel table
            /// </summary>
            /// <param name="ACacheableTable">enum that defines personnel table to be returned</param>
            /// <returns></returns>
            public static DataTable GetCacheablePersonnelTable(TCacheablePersonTablesEnum ACacheableTable)
            {
                if (FDelegateGetCacheablePersonnelTable != null)
                {
                    return FDelegateGetCacheablePersonnelTable(ACacheableTable);
                }
                else
                {
                    throw new InvalidOperationException("Delegate 'TGetCacheablePersonnelTable' must be initialised before calling this Method");
                }
            }

            /// <summary>
            /// retrieve cacheable unit table
            /// </summary>
            /// <param name="ACacheableTable">enum that defines unit table to be returned</param>
            /// <returns></returns>
            public static DataTable GetCacheableUnitsTable(TCacheableUnitTablesEnum ACacheableTable)
            {
                if (FDelegateGetCacheableUnitsTable != null)
                {
                    return FDelegateGetCacheableUnitsTable(ACacheableTable);
                }
                else
                {
                    throw new InvalidOperationException("Delegate 'TGetCacheableUnitsTable' must be initialised before calling this Method");
                }
            }
        }
        #endregion

        #region TDataCache.TMSysMan
        /// <summary>
        /// todoComment
        /// </summary>
        public class TMSysMan
        {
            /// <summary>
            /// Delegate for retrieving data of a cacheable SysMan table
            /// </summary>
            public delegate DataTable TGetCacheableSysManTable(TCacheableSysManTablesEnum ACacheableTable);

            /// <summary>
            /// Reference to the Delegate for retrieving data of a cacheable SysMan table
            /// </summary>
            private static TGetCacheableSysManTable FDelegateGetCacheableSysManTable;

            /// <summary>
            /// This property is used to provide a function which retrieves data of a cacheable SysMan table
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetCacheableSysManTable GetCacheableSysManTableDelegate
            {
                get
                {
                    return FDelegateGetCacheableSysManTable;
                }

                set
                {
                    FDelegateGetCacheableSysManTable = value;
                }
            }

            /// <summary>
            /// retrieve cacheable SysMan table
            /// </summary>
            /// <param name="ACacheableTable">enum that defines SysMan table to be returned</param>
            /// <returns></returns>
            public static DataTable GetCacheableSysManTable(TCacheableSysManTablesEnum ACacheableTable)
            {
                if (FDelegateGetCacheableSysManTable != null)
                {
                    return FDelegateGetCacheableSysManTable(ACacheableTable);
                }
                else
                {
                    throw new InvalidOperationException("Delegate 'TGetCacheableSysManTable' must be initialised before calling this Method");
                }
            }
        }
        #endregion
    }
}