//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections.Specialized;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// manual methods for the generated window
    public partial class TFrmMaintainUsers
    {
        private void InitializeManualCode()
        {
            bool CanCreateUser;
            bool CanChangePassword;
            bool CanChangePermissions;

            TRemote.MSysMan.Maintenance.WebConnectors.GetAuthenticationFunctionality(out CanCreateUser,
                out CanChangePassword,
                out CanChangePermissions);

            if (!CanCreateUser)
            {
                this.FPetraUtilsObject.EnableAction("actNewUser", false);
            }

            if (!CanChangePassword)
            {
                this.FPetraUtilsObject.EnableAction("actSetPassword", false);
            }

            if (!CanChangePermissions)
            {
                // TODO: clbUserGroup should depend on cndChangePermissions, and disabled if the condition is false
                this.FPetraUtilsObject.EnableAction("cndChangePermissions", false);
            }

            LoadUsers();

            // SModuleTable is loaded with the users, therefore we can only now fill the checked list box.
            // TODO: should use cached table instead?
            LoadAvailableModulesIntoCheckedListBox();
        }

        private void LoadAvailableModulesIntoCheckedListBox()
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = SModuleTable.GetModuleNameDBName();
            string ValueMember = SModuleTable.GetModuleIdDBName();

            DataTable NewTable = FMainDS.SModule.DefaultView.ToTable(true, new string[] { ValueMember, DisplayMember });

            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbUserGroup.Columns.Clear();
            clbUserGroup.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            clbUserGroup.AddTextColumn(Catalog.GetString("Module"), NewTable.Columns[ValueMember], 100);
            clbUserGroup.AddTextColumn(Catalog.GetString("Description"), NewTable.Columns[DisplayMember], 220);
            clbUserGroup.DataBindGrid(NewTable, ValueMember, CheckedMember, ValueMember, false, true, false);
        }

        private void LoadUsers()
        {
            FMainDS = TRemote.MSysMan.Maintenance.WebConnectors.LoadUsersAndModulePermissions();

            if (FMainDS != null)
            {
                FMainDS.SUser.DefaultView.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.SUser.DefaultView);
            }

            SelectRowInGrid(1);
        }

        private TSubmitChangesResult StoreManualCode(ref MaintainUsersTDS ASubmitDS, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            return TRemote.MSysMan.Maintenance.WebConnectors.SaveSUser(ref ASubmitDS);
        }

        private void ShowDetailsManual(SUserRow ARow)
        {
            string currentPermissions = String.Empty;

            if (ARow != null)
            {
                FMainDS.SUserModuleAccessPermission.DefaultView.RowFilter =
                    String.Format("{0}='{1}'",
                        SUserModuleAccessPermissionTable.GetUserIdDBName(),
                        ARow.UserId);

                foreach (DataRowView rv in FMainDS.SUserModuleAccessPermission.DefaultView)
                {
                    SUserModuleAccessPermissionRow permission = (SUserModuleAccessPermissionRow)rv.Row;

                    if (permission.CanAccess)
                    {
                        currentPermissions = StringHelper.AddCSV(currentPermissions, permission.ModuleId);
                    }
                }
            }

            clbUserGroup.SetCheckedStringList(currentPermissions);
        }

        private void GetDetailDataFromControlsManual(SUserRow ARow)
        {
            ARow.UserId = ARow.UserId.ToUpperInvariant();
            FMainDS.SUserModuleAccessPermission.DefaultView.RowFilter =
                String.Format("{0}='{1}'",
                    SUserModuleAccessPermissionTable.GetUserIdDBName(),
                    ARow.UserId);
            string currentPermissions = clbUserGroup.GetCheckedStringList();
            StringCollection CSVValues = StringHelper.StrSplit(currentPermissions, ",");

            foreach (DataRowView rv in FMainDS.SUserModuleAccessPermission.DefaultView)
            {
                SUserModuleAccessPermissionRow permission = (SUserModuleAccessPermissionRow)rv.Row;

                if (permission.CanAccess)
                {
                    if (!CSVValues.Contains(permission.ModuleId))
                    {
                        permission.CanAccess = false;
                    }
                    else
                    {
                        CSVValues.Remove(permission.ModuleId);
                    }
                }
                else if (!permission.CanAccess && CSVValues.Contains(permission.ModuleId))
                {
                    permission.CanAccess = true;
                    CSVValues.Remove(permission.ModuleId);
                }
            }

            // add new permissions
            foreach (string module in CSVValues)
            {
                SUserModuleAccessPermissionRow newRow = FMainDS.SUserModuleAccessPermission.NewRowTyped();
                newRow.UserId = ARow.UserId;
                newRow.ModuleId = module;
                newRow.CanAccess = true;
                FMainDS.SUserModuleAccessPermission.Rows.Add(newRow);
            }
        }

        private void NewUser(Object Sender, EventArgs e)
        {
            CreateNewSUser();
        }

        private void NewRowManual(ref SUserRow ARow)
        {
            string newName = Catalog.GetString("NEWUSER");
            Int32 countNewDetail = 0;

            if (FMainDS.SUser.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.SUser.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.UserId = newName;
        }

        private void RetireUser(Object Sender, EventArgs e)
        {
            GetSelectedDetailRow().Retired = !GetSelectedDetailRow().Retired;
            FPetraUtilsObject.SetChangedFlag();
        }

        private void SetPassword(Object Sender, EventArgs e)
        {
            if (this.FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Catalog.GetString("Please save the current changes first before changing the password."));
                return;
            }

            string username = GetSelectedDetailRow().UserId;

            // only request the password once, since this is the sysadmin changing it.
            // see http://bazaar.launchpad.net/~openpetracore/openpetraorg/trunkhosted/view/head:/csharp/ICT/Petra/Client/MSysMan/Gui/SysManMain.cs
            // for the change password dialog for the normal user
            PetraInputBox input = new PetraInputBox(
                Catalog.GetString("Change the password"),
                String.Format(Catalog.GetString("Please enter the new password for user {0}:"), username),
                "", true);

            if (input.ShowDialog() == DialogResult.OK)
            {
                string password = input.GetAnswer();

                if (TRemote.MSysMan.Maintenance.WebConnectors.SetUserPassword(username, password, true))
                {
                    LoadUsers();
                    MessageBox.Show(String.Format(Catalog.GetString("Password was successfully set for user {0}"), username));
                }
                else
                {
                    MessageBox.Show(String.Format(Catalog.GetString("There was a problem setting the password for user {0}"), username));
                }
            }
        }
    }
}