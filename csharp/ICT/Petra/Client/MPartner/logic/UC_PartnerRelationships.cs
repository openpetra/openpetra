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
using System.Drawing;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Controls;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using SourceGrid;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// Contains logic for the UC_PartnerRelationships UserControl.
    /// </summary>
    public class TUCPartnerRelationshipsLogic
    {
        private PartnerEditTDS FMainDS;
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        #region Properties

        /// <summary>todoComment</summary>
        public PartnerEditTDS MultiTableDS
        {
            get
            {
                return FMainDS;
            }

            set
            {
                FMainDS = value;
            }
        }


        /// <summary>used for passing through the Clientside Proxy for the UIConnector</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }
        #endregion

        /// <summary>
        /// Loads Partner Types Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        public Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;
            DataColumn ForeignTableColumn;
            PRelationTable relationTable;
            Boolean NoAcceptChangesNeeded = false;

            // Load Partner Types, if not already loaded
            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PPartnerRelationship == null)
                {
                    FMainDS.Tables.Add(new PartnerEditTDSPPartnerRelationshipTable());
                    FMainDS.InitVars();
                }
                else
                {
                    // special case: if there are already relations on the client before server data is
                    // loaded then don't call AcceptChanges later. This is the case for a newly created
                    // Person record when a relation to the Family record is automatically created but
                    // not yet saved to the database (therefore it appears in the local data sets but not
                    // the one's retrieved from the database)
                    if (FMainDS.PPartnerRelationship.Rows.Count > 0)
                    {
                        NoAcceptChangesNeeded = true;
                    }
                }

                if (TClientSettings.DelayedDataLoading)
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataPartnerRelationships());

                    // Make DataRows unchanged
                    if ((FMainDS.PPartnerRelationship.Rows.Count > 0)
                        && !NoAcceptChangesNeeded)
                    {
                        FMainDS.PPartnerRelationship.AcceptChanges();
                    }
                }

                // fill extra fields in PartnerRelationship table for description and reciprocal description
                // so the expression can later on decide which value to use in each case
                relationTable = (PRelationTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.RelationList);
                PRelationRow relationRow;
                PartnerEditTDSPPartnerRelationshipRow relationshipRow;

                foreach (DataRow row in FMainDS.PPartnerRelationship.Rows)
                {
                    relationshipRow = (PartnerEditTDSPPartnerRelationshipRow)row;
                    relationRow = (PRelationRow)relationTable.Rows.Find(relationshipRow.RelationName);

                    if (relationRow != null)
                    {
                        relationshipRow.RelationDescription = relationRow.RelationDescription;
                        relationshipRow.ReciprocalRelationDescription = relationRow.ReciprocalDescription;
                    }
                }

                // Relations are not automatically enabled. Need to enable them here in order to use for columns.
                FMainDS.EnableRelations();

                // add column to display the other partner key depending on direction of relationship
                ForeignTableColumn = new DataColumn();
                ForeignTableColumn.DataType = System.Type.GetType("System.Int64");
                ForeignTableColumn.ColumnName = "OtherPartnerKey";
                ForeignTableColumn.Expression = "IIF(" + PPartnerRelationshipTable.GetPartnerKeyDBName() + "=" +
                                                ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey.ToString() + "," +
                                                PPartnerRelationshipTable.GetRelationKeyDBName() + "," +
                                                PPartnerRelationshipTable.GetPartnerKeyDBName() + ")";
                FMainDS.PPartnerRelationship.Columns.Add(ForeignTableColumn);

                // make sure that description column takes the correct description (normal or reciprocal)
                FMainDS.PPartnerRelationship.ColumnDisplayRelationDescription.Expression =
                    "IIF(" + PPartnerRelationshipTable.GetPartnerKeyDBName() + "=" +
                    ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey.ToString() +
                    "," + FMainDS.PPartnerRelationship.ColumnRelationDescription +
                    "," + FMainDS.PPartnerRelationship.ColumnReciprocalRelationDescription + ")";

                if (FMainDS.PPartnerRelationship.Rows.Count != 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            return ReturnValue;
        }

        /// <summary>
        /// Updates Description (normal and reciprocal) for given relation name in row
        /// </summary>
        public void UpdateRelationDescription(PartnerEditTDSPPartnerRelationshipRow ARow, String ANewRelationName)
        {
            PRelationTable relationTable = (PRelationTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.RelationList);
            PRelationRow relationRow;

            if (ARow != null)
            {
                relationRow = (PRelationRow)relationTable.Rows.Find(ANewRelationName);

                if (relationRow != null)
                {
                    ARow.RelationDescription = relationRow.RelationDescription;
                    ARow.ReciprocalRelationDescription = relationRow.ReciprocalDescription;
                }
                else
                {
                    ARow.RelationDescription = "";
                    ARow.ReciprocalRelationDescription = "";
                }
            }
        }
    }
}