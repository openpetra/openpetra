//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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

using Ict.Common.Exceptions;

namespace Ict.Petra.Shared
{
    /// <summary>
    /// enum that identifies static partner tables
    /// </summary>
    public enum TStaticPartnerTablesEnum
    {
        /// <summary>
        /// codes of accomodation
        /// </summary>
        AccommodationCodeList,

        /// <summary>
        /// address display order
        /// </summary>
        AddressDisplayOrderList,

        /// <summary>
        /// address layouts
        /// </summary>
        AddressLayoutList,

        /// <summary>
        /// list of genders
        /// </summary>
        GenderList,

        /// <summary>
        /// list of partner classes
        /// </summary>
        PartnerClassList,

        /// <summary>
        /// list of frequencies to review a proposal
        /// </summary>
        ProposalReviewFrequency,

        /// <summary>
        /// list of frequencies to submit a proposal
        /// </summary>
        ProposalSubmitFrequency,

        /// <summary>
        /// different status for subscriptions
        /// </summary>
        SubscriptionStatus
    };

    /// <summary>
    /// Provides a central place for static DataTables.
    ///
    /// These are DataTables that hold information that is used in many parts of
    /// Petra and whose data is *not* stored anywhere in the Petra DB, but hardcoded
    /// by programmers.
    ///
    /// </summary>
    public class TStaticDataTables : object
    {
        /// <summary>used internally to hold all static tables</summary>
        public static DataSet UStaticDataTablesCacheDS = new DataSet("StaticDataTablesCache");

        #region TStaticDataTables

        /// <summary>
        /// static tables in the partner module
        /// </summary>
        public class TMPartner : object
        {
            #region TStaticDataTables.TMPartner

            /**
             * Returns a reference to the Cache DataSet that holds the chosen DataTable
             * for the Petra Partner Module, Partner Sub-Module.
             * If the DataTable is not available on the Client side, it is automatically
             * retrieved from the Petra Server.
             *
             * @param ADataTable The static DataTable that should be returned
             * @return DataSet The desired DataTable
             *
             */
            public static DataTable GetStaticTable(TStaticPartnerTablesEnum AStaticDataTable)
            {
                DataTable ReturnValue;

                if (UStaticDataTablesCacheDS.Tables.Contains(AStaticDataTable.ToString("G")))
                {
                    ReturnValue = UStaticDataTablesCacheDS.Tables[AStaticDataTable.ToString("G")];
                }
                else
                {
                    switch (AStaticDataTable)
                    {
                        case TStaticPartnerTablesEnum.AccommodationCodeList:
                            ReturnValue = BuildAccommodationCodeListTable();
                            break;

                        case TStaticPartnerTablesEnum.AddressDisplayOrderList:
                            ReturnValue = BuildAddressDisplayOrderListTable();
                            break;

                        case TStaticPartnerTablesEnum.AddressLayoutList:
                            ReturnValue = BuildAddressLayoutListTable();
                            break;

                        case TStaticPartnerTablesEnum.GenderList:
                            ReturnValue = BuildGenderListTable();
                            break;

                        case TStaticPartnerTablesEnum.PartnerClassList:
                            ReturnValue = BuildPartnerClassListTable();
                            break;

                        case TStaticPartnerTablesEnum.ProposalReviewFrequency:
                            ReturnValue = BuildProposalReviewFrequencyTable();
                            break;

                        case TStaticPartnerTablesEnum.ProposalSubmitFrequency:
                            ReturnValue = BuildProposalSubmitFrequencyTable();
                            break;

                        case TStaticPartnerTablesEnum.SubscriptionStatus:
                            ReturnValue = BuildSubscriptionStatusTable();
                            break;

                        default:
                            throw new EStaticDataTableNotImplementedException("Requested Static DataTable '" + AStaticDataTable.ToString(
                                "G") + "' is not (yet) implemented");

                            //break;
                    }
                }

                return ReturnValue;
            }

            /// <summary>
            /// build the list of accomodation codes
            /// </summary>
            /// <returns></returns>
            public static DataTable BuildAccommodationCodeListTable()
            {
                const String PRIMARYKEYROWNAME = "AccommodationCode";

                DataTable StaticDT;
                DataRow ARow;

                StaticDT = new DataTable(TStaticPartnerTablesEnum.AccommodationCodeList.ToString("G"));
                StaticDT.Columns.Add(PRIMARYKEYROWNAME, System.Type.GetType("System.String"));
                ARow = StaticDT.NewRow();
                ARow[0] = "ROOM";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "HOME";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "DORM";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "OTHER";
                StaticDT.Rows.Add(ARow);
                StaticDT.PrimaryKey = new DataColumn[] {
                    StaticDT.Columns[PRIMARYKEYROWNAME]
                };
                UStaticDataTablesCacheDS.Tables.Add(StaticDT);
                return StaticDT;
            }

            /// <summary>
            /// build list of address display orders
            /// </summary>
            /// <returns></returns>
            public static DataTable BuildAddressDisplayOrderListTable()
            {
                const String PRIMARYKEYROWNAME = "AddressDisplayOrder";

                DataTable StaticDT;
                DataRow ARow;

                StaticDT = new DataTable(TStaticPartnerTablesEnum.AddressDisplayOrderList.ToString("G"));
                StaticDT.Columns.Add(PRIMARYKEYROWNAME, System.Type.GetType("System.Int32"));
                StaticDT.Columns.Add("Description", System.Type.GetType("System.String"));
                ARow = StaticDT.NewRow();
                ARow[0] = "0";
                ARow[1] = "International";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "1";
                ARow[1] = "European";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "2";
                ARow[1] = "American";
                StaticDT.Rows.Add(ARow);

                StaticDT.PrimaryKey = new DataColumn[] {
                    StaticDT.Columns[PRIMARYKEYROWNAME]
                };
                UStaticDataTablesCacheDS.Tables.Add(StaticDT);
                return StaticDT;
            }

            /// <summary>
            /// build list of address layouts
            /// </summary>
            /// <returns></returns>
            public static DataTable BuildAddressLayoutListTable()
            {
                const String PRIMARYKEYROWNAME = "AddressLayout";

                DataTable StaticDT;
                DataRow ARow;

                StaticDT = new DataTable(TStaticPartnerTablesEnum.AddressLayoutList.ToString("G"));
                StaticDT.Columns.Add(PRIMARYKEYROWNAME, System.Type.GetType("System.String"));
                ARow = StaticDT.NewRow();
                ARow[0] = "SmlLabel";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "One_Line";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "Rolodex";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "Envelope";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "Ltr_head";
                StaticDT.Rows.Add(ARow);

                StaticDT.PrimaryKey = new DataColumn[] {
                    StaticDT.Columns[PRIMARYKEYROWNAME]
                };
                UStaticDataTablesCacheDS.Tables.Add(StaticDT);
                return StaticDT;
            }

            /// <summary>
            /// build the list of genders
            /// </summary>
            /// <returns></returns>
            public static DataTable BuildGenderListTable()
            {
                const String PRIMARYKEYROWNAME = "Gender";
                DataTable StaticDT;
                DataRow ARow;

                StaticDT = new DataTable(TStaticPartnerTablesEnum.GenderList.ToString("G"));
                StaticDT.Columns.Add(PRIMARYKEYROWNAME, System.Type.GetType("System.String"));
                ARow = StaticDT.NewRow();
                ARow[0] = "Female";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "Male";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "Unknown";
                StaticDT.Rows.Add(ARow);
                StaticDT.PrimaryKey = new DataColumn[] {
                    StaticDT.Columns[PRIMARYKEYROWNAME]
                };
                UStaticDataTablesCacheDS.Tables.Add(StaticDT);
                return StaticDT;
            }

            /// <summary>
            /// create list of partner classes
            /// </summary>
            /// <returns></returns>
            public static DataTable BuildPartnerClassListTable()
            {
                const String PRIMARYKEYROWNAME = "PartnerClass";

                DataTable StaticDT;
                DataRow ARow;

                StaticDT = new DataTable(TStaticPartnerTablesEnum.PartnerClassList.ToString("G"));
                StaticDT.Columns.Add(PRIMARYKEYROWNAME, System.Type.GetType("System.String"));
                ARow = StaticDT.NewRow();
                ARow[0] = "PERSON";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "FAMILY";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "CHURCH";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "ORGANISATION";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "BANK";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "UNIT";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "VENUE";
                StaticDT.Rows.Add(ARow);
                StaticDT.PrimaryKey = new DataColumn[] {
                    StaticDT.Columns[PRIMARYKEYROWNAME]
                };
                UStaticDataTablesCacheDS.Tables.Add(StaticDT);
                return StaticDT;
            }

            /// <summary>
            /// build list of frequencies for proposal reviews
            /// </summary>
            /// <returns></returns>
            public static DataTable BuildProposalReviewFrequencyTable()
            {
                DataTable StaticDT;
                DataRow ARow;

                StaticDT = new DataTable(TStaticPartnerTablesEnum.ProposalReviewFrequency.ToString("G"));
                StaticDT.Columns.Add("ProposalReviewFrequency", System.Type.GetType("System.String"));
                ARow = StaticDT.NewRow();
                ARow[0] = "Annually";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "Quarterly";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "Monthly";
                StaticDT.Rows.Add(ARow);
                UStaticDataTablesCacheDS.Tables.Add(StaticDT);
                return StaticDT;
            }

            /// <summary>
            /// create a list of frequencies for proposal submission
            /// </summary>
            /// <returns></returns>
            public static DataTable BuildProposalSubmitFrequencyTable()
            {
                DataTable StaticDT;
                DataRow ARow;

                StaticDT = new DataTable(TStaticPartnerTablesEnum.ProposalSubmitFrequency.ToString("G"));
                StaticDT.Columns.Add("ProposalSubmitFrequency", System.Type.GetType("System.String"));
                ARow = StaticDT.NewRow();
                ARow[0] = "Bi-Annually";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "Annually";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "No Restrictions";
                StaticDT.Rows.Add(ARow);
                UStaticDataTablesCacheDS.Tables.Add(StaticDT);
                return StaticDT;
            }

            /// <summary>
            /// build table for status of subscription
            /// </summary>
            /// <returns></returns>
            public static DataTable BuildSubscriptionStatusTable()
            {
                DataTable StaticDT;
                DataRow ARow;

                StaticDT = new DataTable(TStaticPartnerTablesEnum.SubscriptionStatus.ToString("G"));
                StaticDT.Columns.Add("SubscriptionStatus", System.Type.GetType("System.String"));
                ARow = StaticDT.NewRow();
                ARow[0] = "PERMANENT";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "PROVISIONAL";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "GIFT";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "CANCELLED";
                StaticDT.Rows.Add(ARow);
                ARow = StaticDT.NewRow();
                ARow[0] = "EXPIRED";
                StaticDT.Rows.Add(ARow);
                UStaticDataTablesCacheDS.Tables.Add(StaticDT);
                return StaticDT;
            }

            #endregion
        }
        #endregion
    }

    /// <summary>
    /// datatable is not available
    /// </summary>
    public class EStaticDataTableNotImplementedException : EOPAppException
    {
        #region EStaticDataTableNotImplementedException

        /// <summary>
        /// constructor
        /// </summary>
        public EStaticDataTableNotImplementedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public EStaticDataTableNotImplementedException(String msg) : base(msg)
        {
        }

        #endregion
    }
}