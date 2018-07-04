//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2018 by OM International
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
using GNU.Gettext;
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
            TDBTransaction ReadTransaction = null;
            PartnerSetupTDS MainDS = new PartnerSetupTDS();

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
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
        /// save modified partner tables
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
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
        [RequireModulePermission("PTNRUSER")]
        public static bool MaintainTypes(string action, Dictionary<string, string> data)
        {
            PartnerSetupTDS MainDS = new PartnerSetupTDS();

            if (action == "create")
            {
                PTypeRow row = MainDS.PType.NewRowTyped();
                row.TypeCode = data["p_type_code_c"].ToUpper();
                row.TypeDescription = data["p_type_description_c"];
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
                    if (row.TypeCode == data["p_type_code_c"])
                    {
                        row.TypeDescription = data["p_type_description_c"];
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
                    if (row.TypeCode == data["p_type_code_c"])
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
            TDBTransaction ReadTransaction = null;
            PartnerSetupTDS MainDS = new PartnerSetupTDS();

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
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
        [RequireModulePermission("PTNRUSER")]
        public static bool MaintainPublications(string action, Dictionary<string, string> data)
        {
            PartnerSetupTDS MainDS = new PartnerSetupTDS();

            if (action == "create")
            {
                PPublicationRow row = MainDS.PPublication.NewRowTyped();
                row.PublicationCode = data["p_publication_code_c"].ToUpper();
                row.PublicationDescription = data["p_publication_description_c"];
                row.FrequencyCode = "Annual";
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
                    if (row.PublicationCode == data["p_publication_code_c"])
                    {
                        row.PublicationDescription = data["p_publication_description_c"];
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
                    if (row.PublicationCode == data["p_publication_code_c"])
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
