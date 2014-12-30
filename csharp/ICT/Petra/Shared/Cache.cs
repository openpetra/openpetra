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
using Ict.Petra.Shared.MPartner.Partner.Data;

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
            /// Delegate for retrieving all p_partner_attribute_type records that are of a certain kind.
            /// </summary>
            public delegate string TGetPartnerCalculationsCertainPartnerAttributeKinds(PPartnerAttributeTypeTable APPartnerAttributeTypeDT = null,
                TGetCacheableDataTableFromCache ACacheRetriever = null);

            /// <summary>Used for the caching of a calculated result whose calculation depends on a Cacheable DataTable.</summary>
            private static string FSystemCategorySettingsConcatStr = null;

            /// <summary>Used for the caching of a calculated result whose calculation depends on a Cacheable DataTable.</summary>
            private static string FPartnerContactDetailAttributeTypesConcatStr = null;

            /// <summary>Used for the caching of a calculated result whose calculation depends on a Cacheable DataTable.</summary>
            private static string FEmailPartnerAttributesConcatStr = null;

            /// <summary>Used for the caching of a calculated result whose calculation depends on a Cacheable DataTable.</summary>
            private static string FPhonePartnerAttributesConcatStr = null;


            /// <summary>
            /// This property is used to provide a function which retrieves data of a cacheable mailing table
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetCacheableMailingTable GetCacheableMailingTableDelegate {
                get;
                set;
            }

            /// <summary>
            /// This property is used to provide a function which retrieves data of a cacheable partner table
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetCacheablePartnerTable GetCacheablePartnerTableDelegate {
                get;
                set;
            }

            /// <summary>
            /// This property is used to provide a function which retrieves data of a cacheable Subscriptions table
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetCacheableSubscriptionsTable GetCacheableSubscriptionsTableDelegate {
                get;
                set;
            }

            /// <summary>
            /// This property is used to provide a function which all p_partner_attribute records whose p_attribute_type
            /// points to any p_partner_attribute_type record that is part of a System Category.
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetPartnerCalculationsCertainPartnerAttributeKinds GetPartnerCalculationsSystemCategoryAttributeTypesDelegate {
                get;
                set;
            }

            /// <summary>
            /// This property is used to provide a function which determines all p_partner_attribute_type records that
            /// which constitute Partner Contact Details and returns the result.
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetPartnerCalculationsCertainPartnerAttributeKinds GetPartnerCalculationsPartnerContactDetailAttributeTypesDelegate {
                get;
                set;
            }

            /// <summary>
            /// This property is used to provide a function which determines all p_partner_attribute_type records that
            /// are of p_attribute_type_value_kind_c 'CONTACTDETAIL_EMAILADDRESS' and returns the result.
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetPartnerCalculationsCertainPartnerAttributeKinds GetPartnerCalculationsEmailPartnerAttributeTypesDelegate {
                get;
                set;
            }

            /// <summary>
            /// This property is used to provide a function which determines all p_partner_attribute_type records that
            /// that have p_category_code_c 'Phone'.
            /// </summary>
            /// <description>The Delegate is set up at the start of the application.</description>
            public static TGetPartnerCalculationsCertainPartnerAttributeKinds GetPartnerCalculationsPhonePartnerAttributeTypesDelegate {
                get;
                set;
            }


            /// <summary>
            /// retrieve cacheable mailing table
            /// </summary>
            /// <param name="ACacheableTable">enum that defines mailing table to be returned</param>
            /// <returns></returns>
            public static DataTable GetCacheableMailingTable(TCacheableMailingTablesEnum ACacheableTable)
            {
                if (GetCacheableMailingTableDelegate != null)
                {
                    return GetCacheableMailingTableDelegate(ACacheableTable);
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
                if (GetCacheablePartnerTableDelegate != null)
                {
                    return GetCacheablePartnerTableDelegate(ACacheableTable);
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
                if (GetCacheableSubscriptionsTableDelegate != null)
                {
                    return GetCacheableSubscriptionsTableDelegate(ACacheableTable);
                }
                else
                {
                    throw new InvalidOperationException("Delegate 'TGetCacheableSubscriptionsTable' must be initialised before calling this Method");
                }
            }

            /// <summary>
            /// Determines all p_partner_attribute_type records which constitute Partner Contact Details and returns the result.
            /// </summary>
            /// <returns>String that contains all p_partner_attribute_type records which constitute Partner Contact Details.
            /// </returns>
            public static string GetSystemCategorySettingsConcatStr()
            {
                // TODO Make this Method multi-threading safe (as it gets used server-side, too)! See implementations in Methods 'AddCachedTableInternal' and 'GetContentsEntry' of TCacheableTablesManager...

                if (FSystemCategorySettingsConcatStr == null)
                {
                    PPartnerAttributeTypeTable PPartnerAttributeTypeDT = null;

                    if (GetCacheablePartnerTableDelegate != null)
                    {
                        PPartnerAttributeTypeDT = (PPartnerAttributeTypeTable)GetCacheablePartnerTableDelegate(
                            TCacheablePartnerTablesEnum.PartnerAttributeSystemCategoryTypeList);
                    }
                    else
                    {
                        throw new InvalidOperationException("Delegate 'TGetCacheablePartnerTable' must be initialised before calling this Method");
                    }

                    if (GetPartnerCalculationsSystemCategoryAttributeTypesDelegate != null)
                    {
                        FSystemCategorySettingsConcatStr = GetPartnerCalculationsSystemCategoryAttributeTypesDelegate(PPartnerAttributeTypeDT, null);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "Delegate 'TGetPartnerCalculationsCertainPartnerAttributeKinds' must be initialised before calling this Method");
                    }
                }

                return FSystemCategorySettingsConcatStr;
            }

            /// <summary>
            /// Determines all p_partner_attribute_type records which constitute Partner Contact Details and returns the result.
            /// </summary>
            /// <returns>String that contains all p_partner_attribute_type records which constitute Partner Contact Details.
            /// </returns>
            public static string GetPartnerContactDetailAttributeTypesConcatStr()
            {
                // TODO Make this Method multi-threading safe (as it gets used server-side, too)! See implementations in Methods 'AddCachedTableInternal' and 'GetContentsEntry' of TCacheableTablesManager...

                if (FPartnerContactDetailAttributeTypesConcatStr == null)
                {
                    PPartnerAttributeTypeTable PPartnerAttributeTypeDT = null;

                    if (GetCacheablePartnerTableDelegate != null)
                    {
                        PPartnerAttributeTypeDT = (PPartnerAttributeTypeTable)GetCacheablePartnerTableDelegate(
                            TCacheablePartnerTablesEnum.ContactTypeList);
                    }
                    else
                    {
                        throw new InvalidOperationException("Delegate 'TGetCacheablePartnerTable' must be initialised before calling this Method");
                    }

                    if (GetPartnerCalculationsPartnerContactDetailAttributeTypesDelegate != null)
                    {
                        FPartnerContactDetailAttributeTypesConcatStr = GetPartnerCalculationsPartnerContactDetailAttributeTypesDelegate(
                            PPartnerAttributeTypeDT,
                            null);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "Delegate 'TGetPartnerCalculationsCertainPartnerAttributeKinds' must be initialised before calling this Method");
                    }
                }

                return FPartnerContactDetailAttributeTypesConcatStr;
            }

            /// <summary>
            /// Determines all p_partner_attribute_type records that are of p_attribute_type_value_kind_c
            /// 'CONTACTDETAIL_EMAILADDRESS' and returns the result.
            /// </summary>
            /// <returns>String that contains all p_partner_attribute_type records are of p_attribute_type_value_kind_c
            /// 'CONTACTDETAIL_EMAILADDRESS'.</returns>
            public static string GetEmailPartnerAttributesConcatStr()
            {
                // TODO Make this Method multi-threading safe (as it gets used server-side, too)! See implementations in Methods 'AddCachedTableInternal' and 'GetContentsEntry' of TCacheableTablesManager...

                if (FEmailPartnerAttributesConcatStr == null)
                {
                    PPartnerAttributeTypeTable PPartnerAttributeTypeDT = null;

                    if (GetCacheablePartnerTableDelegate != null)
                    {
                        PPartnerAttributeTypeDT = (PPartnerAttributeTypeTable)GetCacheablePartnerTableDelegate(
                            TCacheablePartnerTablesEnum.ContactTypeList);
                    }
                    else
                    {
                        throw new InvalidOperationException("Delegate 'TGetCacheablePartnerTable' must be initialised before calling this Method");
                    }

                    if (GetPartnerCalculationsEmailPartnerAttributeTypesDelegate != null)
                    {
                        FEmailPartnerAttributesConcatStr = GetPartnerCalculationsEmailPartnerAttributeTypesDelegate(PPartnerAttributeTypeDT, null);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "Delegate 'TGetPartnerCalculationsCertainPartnerAttributeKinds' must be initialised before calling this Method");
                    }
                }

                return FEmailPartnerAttributesConcatStr;
            }

            /// <summary>
            /// Determines all p_partner_attribute_type records that have p_category_code_c 'Phone'.
            /// </summary>
            /// <returns>String that contains all p_partner_attribute_type records that have p_category_code_c 'Phone'.</returns>
            public static string GetPhonePartnerAttributesConcatStr()
            {
                // TODO Make this Method multi-threading safe (as it gets used server-side, too)! See implementations in Methods 'AddCachedTableInternal' and 'GetContentsEntry' of TCacheableTablesManager...

                if (FPhonePartnerAttributesConcatStr == null)
                {
                    PPartnerAttributeTypeTable PPartnerAttributeTypeDT = null;

                    if (GetCacheablePartnerTableDelegate != null)
                    {
                        PPartnerAttributeTypeDT = (PPartnerAttributeTypeTable)GetCacheablePartnerTableDelegate(
                            TCacheablePartnerTablesEnum.ContactTypeList);
                    }
                    else
                    {
                        throw new InvalidOperationException("Delegate 'TGetCacheablePartnerTable' must be initialised before calling this Method");
                    }

                    if (GetPartnerCalculationsPhonePartnerAttributeTypesDelegate != null)
                    {
                        FPhonePartnerAttributesConcatStr = GetPartnerCalculationsPhonePartnerAttributeTypesDelegate(PPartnerAttributeTypeDT, null);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "Delegate 'TGetPartnerCalculationsCertainPartnerAttributeKinds' must be initialised before calling this Method");
                    }
                }

                return FPhonePartnerAttributesConcatStr;
            }

            /// <summary>
            /// Causes the internal cached data that the Method
            /// <see cref="GetSystemCategorySettingsConcatStr"/> returns to be refreshed the next time it
            /// gets called.
            /// </summary>
            /// <remarks>Needs to be called once the data in the underlying Cacheable DataTable changes!</remarks>
            public static void MarkSystemCategorySettingsConcatStrNeedsRefreshing()
            {
                // TODO Make this Method multi-threading safe (as it gets used server-side, too)! See implementations in Method 'AddCachedTableInternal' of TCacheableTablesManager...

                FSystemCategorySettingsConcatStr = null;
            }

            /// <summary>
            /// Causes the internal cached data that the Method
            /// <see cref="GetPartnerContactDetailAttributeTypesConcatStr"/> returns to be refreshed the next time it
            /// gets called.
            /// </summary>
            /// <remarks>Needs to be called once the data in the underlying Cacheable DataTable changes!</remarks>
            public static void MarkPartnerContactDetailAttributeTypesConcatStrNeedsRefreshing()
            {
                // TODO Make this Method multi-threading safe (as it gets used server-side, too)! See implementations in Method 'AddCachedTableInternal' of TCacheableTablesManager...

                FPartnerContactDetailAttributeTypesConcatStr = null;
            }

            /// <summary>
            /// Causes the internal cached data that the Method
            /// <see cref="GetPhonePartnerAttributesConcatStr"/> returns to be refreshed the next time it
            /// gets called.
            /// </summary>
            /// <remarks>Needs to be called once the data in the underlying Cacheable DataTable changes!</remarks>
            public static void MarkPhonePartnerAttributesConcatStrNeedsRefreshing()
            {
                // TODO Make this Method multi-threading safe (as it gets used server-side, too)! See implementations in Method 'AddCachedTableInternal' of TCacheableTablesManager...

                FPhonePartnerAttributesConcatStr = null;
            }

            /// <summary>
            /// Causes the internal cached data that the Method
            /// <see cref="GetEmailPartnerAttributesConcatStr"/> returns to be refreshed the next time it
            /// gets called.
            /// </summary>
            /// <remarks>Needs to be called once the data in the underlying Cacheable DataTable changes!</remarks>
            public static void MarkEmailPartnerAttributesConcatStrNeedsRefreshing()
            {
                // TODO Make this Method multi-threading safe (as it gets used server-side, too)! See implementations in Method 'AddCachedTableInternal' of TCacheableTablesManager...

                FEmailPartnerAttributesConcatStr = null;
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