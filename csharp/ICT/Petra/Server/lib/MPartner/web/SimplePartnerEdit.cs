//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// functions for creating new partners and to edit partners
    /// </summary>
    public class TSimplePartnerEditWebConnector
    {
        /// <summary>
        /// get a partner key for a new partner
        /// </summary>
        /// <param name="AFieldPartnerKey">can be -1, then the default site key is used</param>
        [RequireModulePermission("PTNRUSER")]
        public static Int64 NewPartnerKey(Int64 AFieldPartnerKey)
        {
            Int64 NewPartnerKey = TNewPartnerKey.GetNewPartnerKey(AFieldPartnerKey);

            TNewPartnerKey.SubmitNewPartnerKey(NewPartnerKey - NewPartnerKey % 1000000, NewPartnerKey, ref NewPartnerKey);
            return NewPartnerKey;
        }

        /// <summary>
        /// return the existing data of a partner
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerEditTDS GetPartnerDetails(Int64 APartnerKey, bool AWithAddressDetails, bool AWithSubscriptions, bool AWithRelationships)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                delegate
                {
                    PPartnerAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);

                    if (MainDS.PPartner.Rows.Count > 0)
                    {
                        switch (MainDS.PPartner[0].PartnerClass)
                        {
                            case MPartnerConstants.PARTNERCLASS_FAMILY:
                                PFamilyAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                                break;

                            case MPartnerConstants.PARTNERCLASS_PERSON:
                                PPersonAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                                break;

                            case MPartnerConstants.PARTNERCLASS_CHURCH:
                                PChurchAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                                break;

                            case MPartnerConstants.PARTNERCLASS_ORGANISATION:
                                POrganisationAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                                break;
                        }

                        if (AWithAddressDetails)
                        {
                            PPartnerLocationAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                            PLocationAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                        }

                        if (AWithRelationships)
                        {
                            PPartnerRelationshipAccess.LoadViaPPartnerPartnerKey(MainDS, APartnerKey, Transaction);
                        }

                        if (AWithSubscriptions)
                        {
                            PSubscriptionAccess.LoadViaPPartnerPartnerKey(MainDS, APartnerKey, Transaction);
                        }
                    }
                });

            return MainDS;
        }

        /// <summary>
        /// Return the existing data of a partner, including address information and primary phone and email
        /// </summary>
        /// <param name="APartnerKey">The partner</param>
        /// <param name="AWithSubscriptions">Option to return subscriptions information</param>
        /// <param name="AWithRelationships">Option to return relationships information</param>
        /// <param name="APrimaryPhoneNumber">Returns the primary phone</param>
        /// <param name="APrimaryEmailAddress">Returns the primary email</param>
        /// <returns>A PartnerEdit data set</returns>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerEditTDS GetPartnerDetails(Int64 APartnerKey, bool AWithSubscriptions, bool AWithRelationships,
            out string APrimaryPhoneNumber, out string APrimaryEmailAddress)
        {
            // Call the standard method including address details
            PartnerEditTDS MainDS = GetPartnerDetails(APartnerKey, true, AWithSubscriptions, AWithRelationships);

            // Now get the primary email and phone
            PPartnerAttributeTable attributeTable = TContactDetailsAggregate.GetPartnersContactDetailAttributes(APartnerKey);

            Calculations.GetPrimaryEmailAndPrimaryPhone(attributeTable, out APrimaryPhoneNumber, out APrimaryEmailAddress);

            return MainDS;
        }

        /// <summary>
        /// store the currently edited partner
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static void SavePartner(PartnerEditTDS AMainDS)
        {
            TDBTransaction ReadTransaction = null;
            TDBTransaction SubmitChangesTransaction = null;
            bool SubmissionOK = false;
            bool ImportDefaultAcquCodeExists = false;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
                delegate
                {
                    ImportDefaultAcquCodeExists = PAcquisitionAccess.Exists(MPartnerConstants.PARTNERIMPORT_AQUISITION_DEFAULT,
                        ReadTransaction);
                });

            if (!ImportDefaultAcquCodeExists)
            {
                PAcquisitionTable AcqTable = new PAcquisitionTable();
                PAcquisitionRow row = AcqTable.NewRowTyped();
                row.AcquisitionCode = MPartnerConstants.PARTNERIMPORT_AQUISITION_DEFAULT;
                row.AcquisitionDescription = Catalog.GetString("Imported Data");
                AcqTable.Rows.Add(row);

                DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref SubmitChangesTransaction, ref SubmissionOK,
                    delegate
                    {
                        PAcquisitionAccess.SubmitChanges(AcqTable, SubmitChangesTransaction);

                        SubmissionOK = true;
                    });
            }

            PartnerEditTDSAccess.SubmitChanges(AMainDS);
        }
    }
}