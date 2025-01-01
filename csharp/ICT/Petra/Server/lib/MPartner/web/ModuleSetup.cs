//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2025 by OM International
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
using System.Xml;
using System.IO;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MPartner.TableMaintenance.WebConnectors
{
    /// <summary>
    /// setup the partner tables
    /// </summary>
    public class TPartnerSetupWebConnector
    {
        /// <summary>
        /// Loads all available Partner Types.
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerSetupTDS LoadPartnerTypes()
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            PartnerSetupTDS MainDS = new PartnerSetupTDS();

            DBAccess.ReadTransaction(ref ReadTransaction,
                delegate
                {
                    PTypeAccess.LoadAll(MainDS, ReadTransaction);
                });

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// Loads all available Membership Types.
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerSetupTDS LoadMemberships()
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            PartnerSetupTDS MainDS = new PartnerSetupTDS();

            DBAccess.ReadTransaction(ref ReadTransaction,
                delegate
                {
                    PMembershipAccess.LoadAll(MainDS, ReadTransaction);
                });

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// save modified partner tables
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRADMIN")]
        public static TSubmitChangesResult SavePartnerMaintenanceTables(ref PartnerSetupTDS AInspectDS)
        {
            if (AInspectDS != null)
            {
                PartnerSetupTDSAccess.SubmitChanges(AInspectDS);

                return TSubmitChangesResult.scrOK;
            }

            return TSubmitChangesResult.scrError;
        }

        /// <summary>
        /// save partner types
        /// </summary>
        [RequireModulePermission("PTNRADMIN")]
        public static bool MaintainTypes(string action, string ATypeCode, string ATypeDescription, string ACategoryCode, bool AValidType, out TVerificationResultCollection AVerificationResult)
        {
            PartnerSetupTDS MainDS = new PartnerSetupTDS();
            AVerificationResult = new TVerificationResultCollection();

            if (action == "create")
            {
                PTypeRow row = MainDS.PType.NewRowTyped();
                row.TypeCode = ATypeCode.ToUpper();
                row.TypeDescription = ATypeDescription;
                row.CategoryCode = ACategoryCode;
                row.ValidType = AValidType;
                MainDS.PType.Rows.Add(row);
                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (action == "update")
            {
                MainDS = LoadPartnerTypes();

                foreach (PTypeRow row in MainDS.PType.Rows)
                {
                    if (row.TypeCode == ATypeCode)
                    {
                        row.TypeDescription = ATypeDescription;
                        row.CategoryCode = ACategoryCode;
                        row.ValidType = AValidType;
                    }
                }

                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (action == "delete")
            {
                MainDS = LoadPartnerTypes();

                foreach (PTypeRow row in MainDS.PType.Rows)
                {
                    if (row.TypeCode == ATypeCode)
                    {
                        if (!row.TypeDeletable)
                        {
                            AVerificationResult.Add(new TVerificationResult("error", "not_deletable", TResultSeverity.Resv_Critical));
                            return false;
                        }
                        
                        row.Delete();
                    }
                }

                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// save memberships
        /// </summary>
        [RequireModulePermission("PTNRADMIN")]
        public static bool MaintainMemberships(string action, string AMembershipCode, string AMembershipDescription, string AFrequencyCode, Decimal AMembershipFee, Decimal AMembershipHoursService, out TVerificationResultCollection AVerificationResult)
        {
            PartnerSetupTDS MainDS = new PartnerSetupTDS();
            AVerificationResult = new TVerificationResultCollection();

            if (action == "create")
            {
                PMembershipRow row = MainDS.PMembership.NewRowTyped();
                row.MembershipCode = AMembershipCode.ToUpper();
                row.MembershipDescription = AMembershipDescription;
                row.MembershipHoursService = AMembershipHoursService;
                row.MembershipFee = AMembershipFee;
                row.FrequencyCode = AFrequencyCode;
                MainDS.PMembership.Rows.Add(row);
                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (action == "update")
            {
                MainDS = LoadMemberships();

                foreach (PMembershipRow row in MainDS.PMembership.Rows)
                {
                    if (row.MembershipCode == AMembershipCode)
                    {
                        row.MembershipDescription = AMembershipDescription;
                        row.MembershipHoursService = AMembershipHoursService;
                        row.MembershipFee = AMembershipFee;
                        row.FrequencyCode = AFrequencyCode;
                    }
                }

                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (action == "delete")
            {
                MainDS = LoadMemberships();

                foreach (PMembershipRow row in MainDS.PMembership.Rows)
                {
                    if (row.MembershipCode == AMembershipCode)
                    {
                        row.Delete();
                    }
                }

                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads all available Consent Channels.
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerSetupTDS LoadConsentChannels()
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            PartnerSetupTDS MainDS = new PartnerSetupTDS();

            DBAccess.ReadTransaction(ref ReadTransaction,
                delegate
                {
                    PConsentChannelAccess.LoadAll(MainDS, ReadTransaction);
                });

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// save consent channels
        /// </summary>
        [RequireModulePermission("PTNRADMIN")]
        public static bool MaintainConsentChannels(string action, string AChannelCode, string AName, string AComment, out TVerificationResultCollection AVerificationResult)
        {
            PartnerSetupTDS MainDS = new PartnerSetupTDS();
            AVerificationResult = new TVerificationResultCollection();

            if (action == "create")
            {
                PConsentChannelRow row = MainDS.PConsentChannel.NewRowTyped();
                row.ChannelCode = AChannelCode.ToUpper();
                row.Name = AName;
                row.Comment = AComment;
                MainDS.PConsentChannel.Rows.Add(row);
                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (action == "update")
            {
                MainDS = LoadConsentChannels();

                foreach (PConsentChannelRow row in MainDS.PConsentChannel.Rows)
                {
                    if (row.ChannelCode == AChannelCode)
                    {
                        row.Name = AName;
                        row.Comment = AComment;
                    }
                }

                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (action == "delete")
            {
                MainDS = LoadConsentChannels();

                foreach (PConsentChannelRow row in MainDS.PConsentChannel.Rows)
                {
                    if (row.ChannelCode == AChannelCode)
                    {
                        row.Delete();
                    }
                }

                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads all available Consent Purposes.
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerSetupTDS LoadConsentPurposes()
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            PartnerSetupTDS MainDS = new PartnerSetupTDS();

            DBAccess.ReadTransaction(ref ReadTransaction,
                delegate
                {
                    PConsentPurposeAccess.LoadAll(MainDS, ReadTransaction);
                });

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// save consent purposes
        /// </summary>
        [RequireModulePermission("PTNRADMIN")]
        public static bool MaintainConsentPurposes(string action, string APurposeCode, string AName, string AComment, out TVerificationResultCollection AVerificationResult)
        {
            PartnerSetupTDS MainDS = new PartnerSetupTDS();
            AVerificationResult = new TVerificationResultCollection();

            if (action == "create")
            {
                PConsentPurposeRow row = MainDS.PConsentPurpose.NewRowTyped();
                row.PurposeCode = APurposeCode.ToUpper();
                row.Name = AName;
                row.Comment = AComment;
                MainDS.PConsentPurpose.Rows.Add(row);
                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (action == "update")
            {
                MainDS = LoadConsentPurposes();

                foreach (PConsentPurposeRow row in MainDS.PConsentPurpose.Rows)
                {
                    if (row.PurposeCode == APurposeCode)
                    {
                        row.Name = AName;
                        row.Comment = AComment;
                    }
                }

                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (action == "delete")
            {
                MainDS = LoadConsentPurposes();

                foreach (PConsentPurposeRow row in MainDS.PConsentPurpose.Rows)
                {
                    if (row.PurposeCode == APurposeCode)
                    {
                        row.Delete();
                    }
                }

                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads all available Publications.
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerSetupTDS LoadPublications()
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            PartnerSetupTDS MainDS = new PartnerSetupTDS();

            DBAccess.ReadTransaction(ref ReadTransaction,
                delegate
                {
                    PPublicationAccess.LoadAll(MainDS, ReadTransaction);
                });

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// maintain publications
        /// </summary>
        [RequireModulePermission("PTNRADMIN")]
        public static bool MaintainPublications(string action, string APublicationCode, string APublicationDescription, bool AValidPublication, out TVerificationResultCollection AVerificationResult)
        {
            PartnerSetupTDS MainDS = new PartnerSetupTDS();
            AVerificationResult = new TVerificationResultCollection();

            if (action == "create")
            {
                PPublicationRow row = MainDS.PPublication.NewRowTyped();
                row.PublicationCode = APublicationCode.ToUpper();
                row.PublicationDescription = APublicationDescription;
                row.FrequencyCode = "Annual";
                row.ValidPublication = true;
                MainDS.PPublication.Rows.Add(row);
                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (action == "update")
            {
                MainDS = LoadPublications();

                foreach (PPublicationRow row in MainDS.PPublication.Rows)
                {
                    if (row.PublicationCode == APublicationCode)
                    {
                        row.PublicationDescription = APublicationDescription;
                        row.ValidPublication = AValidPublication;
                    }
                }

                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (action == "delete")
            {
                MainDS = LoadPublications();

                foreach (PPublicationRow row in MainDS.PPublication.Rows)
                {
                    if (row.PublicationCode == APublicationCode)
                    {
                        row.Delete();
                    }
                }

                try
                {
                    PartnerSetupTDSAccess.SubmitChanges(MainDS);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
