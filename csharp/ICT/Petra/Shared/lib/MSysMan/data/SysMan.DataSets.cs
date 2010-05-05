/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
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

using Ict.Common;
using Ict.Common.Data;
using System;
using System.Data;
using System.Data.Odbc;
using Ict.Petra.Shared.MSysMan.Data;

namespace Ict.Petra.Shared.MSysMan.Data
{

     /// auto generated
    [Serializable()]
    public class MaintainUsersTDS : TTypedDataSet
    {

        private SUserTable TableSUser;
        private SModuleTable TableSModule;
        private SGroupTable TableSGroup;
        private SUserGroupTable TableSUserGroup;
        private SGroupModuleAccessPermissionTable TableSGroupModuleAccessPermission;
        private SGroupTableAccessPermissionTable TableSGroupTableAccessPermission;
        private SUserModuleAccessPermissionTable TableSUserModuleAccessPermission;
        private SUserTableAccessPermissionTable TableSUserTableAccessPermission;

        /// auto generated
        public MaintainUsersTDS() :
                base("MaintainUsersTDS")
        {
        }

        /// auto generated for serialization
        public MaintainUsersTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public MaintainUsersTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public SUserTable SUser
        {
            get
            {
                return this.TableSUser;
            }
        }

        /// auto generated
        public SModuleTable SModule
        {
            get
            {
                return this.TableSModule;
            }
        }

        /// auto generated
        public SGroupTable SGroup
        {
            get
            {
                return this.TableSGroup;
            }
        }

        /// auto generated
        public SUserGroupTable SUserGroup
        {
            get
            {
                return this.TableSUserGroup;
            }
        }

        /// auto generated
        public SGroupModuleAccessPermissionTable SGroupModuleAccessPermission
        {
            get
            {
                return this.TableSGroupModuleAccessPermission;
            }
        }

        /// auto generated
        public SGroupTableAccessPermissionTable SGroupTableAccessPermission
        {
            get
            {
                return this.TableSGroupTableAccessPermission;
            }
        }

        /// auto generated
        public SUserModuleAccessPermissionTable SUserModuleAccessPermission
        {
            get
            {
                return this.TableSUserModuleAccessPermission;
            }
        }

        /// auto generated
        public SUserTableAccessPermissionTable SUserTableAccessPermission
        {
            get
            {
                return this.TableSUserTableAccessPermission;
            }
        }

        /// auto generated
        public new virtual MaintainUsersTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((MaintainUsersTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new SUserTable("SUser"));
            this.Tables.Add(new SModuleTable("SModule"));
            this.Tables.Add(new SGroupTable("SGroup"));
            this.Tables.Add(new SUserGroupTable("SUserGroup"));
            this.Tables.Add(new SGroupModuleAccessPermissionTable("SGroupModuleAccessPermission"));
            this.Tables.Add(new SGroupTableAccessPermissionTable("SGroupTableAccessPermission"));
            this.Tables.Add(new SUserModuleAccessPermissionTable("SUserModuleAccessPermission"));
            this.Tables.Add(new SUserTableAccessPermissionTable("SUserTableAccessPermission"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("SUser") != -1))
            {
                this.Tables.Add(new SUserTable("SUser"));
            }
            if ((ds.Tables.IndexOf("SModule") != -1))
            {
                this.Tables.Add(new SModuleTable("SModule"));
            }
            if ((ds.Tables.IndexOf("SGroup") != -1))
            {
                this.Tables.Add(new SGroupTable("SGroup"));
            }
            if ((ds.Tables.IndexOf("SUserGroup") != -1))
            {
                this.Tables.Add(new SUserGroupTable("SUserGroup"));
            }
            if ((ds.Tables.IndexOf("SGroupModuleAccessPermission") != -1))
            {
                this.Tables.Add(new SGroupModuleAccessPermissionTable("SGroupModuleAccessPermission"));
            }
            if ((ds.Tables.IndexOf("SGroupTableAccessPermission") != -1))
            {
                this.Tables.Add(new SGroupTableAccessPermissionTable("SGroupTableAccessPermission"));
            }
            if ((ds.Tables.IndexOf("SUserModuleAccessPermission") != -1))
            {
                this.Tables.Add(new SUserModuleAccessPermissionTable("SUserModuleAccessPermission"));
            }
            if ((ds.Tables.IndexOf("SUserTableAccessPermission") != -1))
            {
                this.Tables.Add(new SUserTableAccessPermissionTable("SUserTableAccessPermission"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableSUser != null))
            {
                this.TableSUser.InitVars();
            }
            if ((this.TableSModule != null))
            {
                this.TableSModule.InitVars();
            }
            if ((this.TableSGroup != null))
            {
                this.TableSGroup.InitVars();
            }
            if ((this.TableSUserGroup != null))
            {
                this.TableSUserGroup.InitVars();
            }
            if ((this.TableSGroupModuleAccessPermission != null))
            {
                this.TableSGroupModuleAccessPermission.InitVars();
            }
            if ((this.TableSGroupTableAccessPermission != null))
            {
                this.TableSGroupTableAccessPermission.InitVars();
            }
            if ((this.TableSUserModuleAccessPermission != null))
            {
                this.TableSUserModuleAccessPermission.InitVars();
            }
            if ((this.TableSUserTableAccessPermission != null))
            {
                this.TableSUserTableAccessPermission.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "MaintainUsersTDS";
            this.TableSUser = ((SUserTable)(this.Tables["SUser"]));
            this.TableSModule = ((SModuleTable)(this.Tables["SModule"]));
            this.TableSGroup = ((SGroupTable)(this.Tables["SGroup"]));
            this.TableSUserGroup = ((SUserGroupTable)(this.Tables["SUserGroup"]));
            this.TableSGroupModuleAccessPermission = ((SGroupModuleAccessPermissionTable)(this.Tables["SGroupModuleAccessPermission"]));
            this.TableSGroupTableAccessPermission = ((SGroupTableAccessPermissionTable)(this.Tables["SGroupTableAccessPermission"]));
            this.TableSUserModuleAccessPermission = ((SUserModuleAccessPermissionTable)(this.Tables["SUserModuleAccessPermission"]));
            this.TableSUserTableAccessPermission = ((SUserTableAccessPermissionTable)(this.Tables["SUserTableAccessPermission"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {

            if (((this.TableSUser != null)
                        && (this.TableSGroup != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGroupcr", "SUser", new string[] {
                                "s_user_id_c"}, "SGroup", new string[] {
                                "s_created_by_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSGroup != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGroupmd", "SUser", new string[] {
                                "s_user_id_c"}, "SGroup", new string[] {
                                "s_modified_by_c"}));
            }
            if (((this.TableSGroup != null)
                        && (this.TableSGroupModuleAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGroupModuleAccPerm1", "SGroup", new string[] {
                                "s_group_id_c", "s_unit_key_n"}, "SGroupModuleAccessPermission", new string[] {
                                "s_group_id_c", "s_group_unit_key_n"}));
            }
            if (((this.TableSModule != null)
                        && (this.TableSGroupModuleAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGroupModuleAccPerm2", "SModule", new string[] {
                                "s_module_id_c"}, "SGroupModuleAccessPermission", new string[] {
                                "s_module_id_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSGroupModuleAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGroupModuleAccessPermissioncr", "SUser", new string[] {
                                "s_user_id_c"}, "SGroupModuleAccessPermission", new string[] {
                                "s_created_by_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSGroupModuleAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGroupModuleAccessPermissionmd", "SUser", new string[] {
                                "s_user_id_c"}, "SGroupModuleAccessPermission", new string[] {
                                "s_modified_by_c"}));
            }
            if (((this.TableSGroup != null)
                        && (this.TableSGroupTableAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGroupTableAccPerm1", "SGroup", new string[] {
                                "s_group_id_c", "s_unit_key_n"}, "SGroupTableAccessPermission", new string[] {
                                "s_group_id_c", "s_group_unit_key_n"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSGroupTableAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGroupTableAccessPermissioncr", "SUser", new string[] {
                                "s_user_id_c"}, "SGroupTableAccessPermission", new string[] {
                                "s_created_by_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSGroupTableAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGroupTableAccessPermissionmd", "SUser", new string[] {
                                "s_user_id_c"}, "SGroupTableAccessPermission", new string[] {
                                "s_modified_by_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSModule != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKModulecr", "SUser", new string[] {
                                "s_user_id_c"}, "SModule", new string[] {
                                "s_created_by_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSModule != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKModulemd", "SUser", new string[] {
                                "s_user_id_c"}, "SModule", new string[] {
                                "s_modified_by_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSUserGroup != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUserGroup1", "SUser", new string[] {
                                "s_user_id_c"}, "SUserGroup", new string[] {
                                "s_user_id_c"}));
            }
            if (((this.TableSGroup != null)
                        && (this.TableSUserGroup != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUserGroup2", "SGroup", new string[] {
                                "s_group_id_c", "s_unit_key_n"}, "SUserGroup", new string[] {
                                "s_group_id_c", "s_unit_key_n"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSUserGroup != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUserGroupcr", "SUser", new string[] {
                                "s_user_id_c"}, "SUserGroup", new string[] {
                                "s_created_by_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSUserGroup != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUserGroupmd", "SUser", new string[] {
                                "s_user_id_c"}, "SUserGroup", new string[] {
                                "s_modified_by_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSUserModuleAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUserModuleAccPerm1", "SUser", new string[] {
                                "s_user_id_c"}, "SUserModuleAccessPermission", new string[] {
                                "s_user_id_c"}));
            }
            if (((this.TableSModule != null)
                        && (this.TableSUserModuleAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUserModuleAccPerm2", "SModule", new string[] {
                                "s_module_id_c"}, "SUserModuleAccessPermission", new string[] {
                                "s_module_id_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSUserModuleAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUserModuleAccessPermissioncr", "SUser", new string[] {
                                "s_user_id_c"}, "SUserModuleAccessPermission", new string[] {
                                "s_created_by_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSUserModuleAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUserModuleAccessPermissionmd", "SUser", new string[] {
                                "s_user_id_c"}, "SUserModuleAccessPermission", new string[] {
                                "s_modified_by_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSUserTableAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUserTableAccPerm1", "SUser", new string[] {
                                "s_user_id_c"}, "SUserTableAccessPermission", new string[] {
                                "s_user_id_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSUserTableAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUserTableAccessPermissioncr", "SUser", new string[] {
                                "s_user_id_c"}, "SUserTableAccessPermission", new string[] {
                                "s_created_by_c"}));
            }
            if (((this.TableSUser != null)
                        && (this.TableSUserTableAccessPermission != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUserTableAccessPermissionmd", "SUser", new string[] {
                                "s_user_id_c"}, "SUserTableAccessPermission", new string[] {
                                "s_modified_by_c"}));
            }
        }
    }
}
