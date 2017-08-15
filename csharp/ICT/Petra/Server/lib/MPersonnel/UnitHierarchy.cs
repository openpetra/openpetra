//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2012-2017 by OM International
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
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPersonnel.Units.Data.Access;
using System.Collections;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared.MPartner.Partner.Data;
using System.Data;
using Ict.Petra.Shared.MPersonnel;

namespace Ict.Petra.Server.MPersonnel.WebConnectors
{
    /// <summary>
    /// Description of Personnel.
    /// </summary>
    public partial class TPersonnelWebConnector
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PERSONNEL")]
        public static ArrayList GetUnitHeirarchy()
        {
            const Int64 THE_ORGANISATION = 1000000;

            ArrayList Ret = new ArrayList();

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                delegate
                {
                    PPartnerTable PartnerTbl = PPartnerAccess.LoadViaPPartnerClasses("UNIT", Transaction);
                    PartnerTbl.DefaultView.RowFilter = "p_status_code_c <> 'MERGED'";
                    PartnerTbl.DefaultView.Sort = PPartnerTable.GetPartnerKeyDBName();

                    PUnitTable UnitTbl = PUnitAccess.LoadAll(Transaction);
                    UUnitTypeTable UnitTypeTbl = UUnitTypeAccess.LoadAll(Transaction);
                    UnitTypeTbl.DefaultView.Sort = UUnitTypeTable.GetUnitTypeCodeDBName();

                    UmUnitStructureTable HierarchyTbl = UmUnitStructureAccess.LoadAll(Transaction);
                    HierarchyTbl.DefaultView.Sort = UmUnitStructureTable.GetChildUnitKeyDBName();

                    UnitTbl.DefaultView.Sort = PUnitTable.GetPartnerKeyDBName();
                    UnitHierarchyNode RootNode = new UnitHierarchyNode();
                    UnitHierarchyNode UnassignedNode = new UnitHierarchyNode();

                    RootNode.MyUnitKey = THE_ORGANISATION;
                    RootNode.ParentUnitKey = THE_ORGANISATION;
                    RootNode.Description = "The Organisation";
                    RootNode.TypeCode = "Root";

                    Int32 RootUnitIdx = UnitTbl.DefaultView.Find(THE_ORGANISATION);

                    if (RootUnitIdx >= 0)
                    {
                        RootNode.Description = ((PUnitRow)UnitTbl.DefaultView[RootUnitIdx].Row).UnitName;
                        UnitTbl.DefaultView.Delete(RootUnitIdx);
                    }

                    Ret.Add(RootNode);

                    UnassignedNode.MyUnitKey = 0;
                    UnassignedNode.ParentUnitKey = 0;
                    UnassignedNode.Description = Catalog.GetString("Unassigned Units");
                    Ret.Add(UnassignedNode);

                    foreach (DataRowView rv in UnitTbl.DefaultView)
                    {
                        PUnitRow UnitRow = (PUnitRow)rv.Row;

                        if (PartnerTbl.DefaultView.Find(UnitRow.PartnerKey) < 0)
                        {
                            // skip all merged units
                            continue;
                        }

                        UnitHierarchyNode Node = new UnitHierarchyNode();
                        Node.Description = UnitRow.UnitName + " " + UnitRow.Description;

                        if (Node.Description == "")
                        {
                            Node.Description = "[" + UnitRow.PartnerKey.ToString("D10") + "]";
                        }

                        Node.MyUnitKey = UnitRow.PartnerKey;

                        //
                        // Retrieve parent..
                        Int32 HierarchyTblIdx = HierarchyTbl.DefaultView.Find(Node.MyUnitKey);

                        if (HierarchyTblIdx >= 0)
                        {
                            Node.ParentUnitKey = ((UmUnitStructureRow)HierarchyTbl.DefaultView[HierarchyTblIdx].Row).ParentUnitKey;
                        }
                        else
                        {
                            Node.ParentUnitKey = UnassignedNode.MyUnitKey;
                        }

                        //
                        // Retrieve TypeCode..
                        Int32 TypeTblIndex = UnitTypeTbl.DefaultView.Find(UnitRow.UnitTypeCode);

                        if (TypeTblIndex >= 0)
                        {
                            Node.TypeCode = ((UUnitTypeRow)UnitTypeTbl.DefaultView[TypeTblIndex].Row).UnitTypeName;
                        }
                        else
                        {
                            Node.TypeCode = "Type: " + UnitRow.UnitTypeCode;
                        }

                        Ret.Add(Node);
                    }
                });

            return Ret;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Nodes"></param>
        /// <returns></returns>
        [RequireModulePermission("PERSONNEL")]
        public static void SaveUnitHierarchy(ArrayList Nodes)
        {
            UmUnitStructureTable NewTable = new UmUnitStructureTable();

            foreach (UnitHierarchyNode Node in Nodes)
            {
                NewTable.Rows.Add(Node.ParentUnitKey, Node.MyUnitKey);
            }

            // This new table I've constructed COMPLETELY REPLACES
            // the existing UmUnitStructure table.
            // I'll delete the whole content before calling SubmitChanges with my new data.

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_um_unit_structure", Transaction);

                    NewTable.ThrowAwayAfterSubmitChanges = true;  // I'm not interested in this table after this Submit:
                    UmUnitStructureAccess.SubmitChanges(NewTable, Transaction);

                    SubmissionOK = true;
                });
        }
    }
}
