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

                // Add relation table to data set
                if (FMainDS.PRelation == null)
                {
                    FMainDS.Tables.Add(new PRelationTable());
                }

                relationTable = (PRelationTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.RelationList);
                // rename data table as otherwise the merge with the data set won't work; tables need to have same name
                relationTable.TableName = "PRelation";
                FMainDS.Merge(relationTable);

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

                // add column for relation description
                ForeignTableColumn = new DataColumn();
                ForeignTableColumn.DataType = System.Type.GetType("System.String");
                ForeignTableColumn.ColumnName = "Parent_" + PRelationTable.GetRelationDescriptionDBName();
                ForeignTableColumn.Expression = "Parent." + PRelationTable.GetRelationDescriptionDBName();
                FMainDS.PPartnerRelationship.Columns.Add(ForeignTableColumn);

                // add column for reciprocal description
                ForeignTableColumn = new DataColumn();
                ForeignTableColumn.DataType = System.Type.GetType("System.String");
                ForeignTableColumn.ColumnName = "Parent_" + PRelationTable.GetReciprocalDescriptionDBName();
                ForeignTableColumn.Expression = "Parent." + PRelationTable.GetReciprocalDescriptionDBName();
                FMainDS.PPartnerRelationship.Columns.Add(ForeignTableColumn);

                // depending on relation use correct description (normal or reciprocal)
                ForeignTableColumn = new DataColumn();
                ForeignTableColumn.DataType = System.Type.GetType("System.String");
                ForeignTableColumn.ColumnName = "RelationDescription";
                ForeignTableColumn.Expression = "IIF(" + PPartnerRelationshipTable.GetPartnerKeyDBName() + "=" +
                                                ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey.ToString() + ",Parent_" +
                                                PRelationTable.GetRelationDescriptionDBName() +
                                                ",Parent_" + PRelationTable.GetReciprocalDescriptionDBName() + ")";
                FMainDS.PPartnerRelationship.Columns.Add(ForeignTableColumn);

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
    }
}