// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
//
// Copyright 2004-2016 by OM International
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
using Ict.Petra.Shared.MSysMan.Validation;
using Ict.Common.Remoting.Client;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// manual methods for the generated window
    public partial class TFrmMaintainUsers
    {
        #region Resourcetexts

        private readonly string StrAccountStateChangeQuestion = Catalog.GetString(
            "Are you sure you want to {0} the user account of user {1} (currently it is {2})?");
        private readonly string StrAccountStateChangeConfirmation = Catalog.GetString(
            "User Account got {0} - user {1} {2}!\r\n(Press 'Save' to apply now.)");
        private readonly string StrAccountStateChangeConfirmationTitle = Catalog.GetString("User Account: {0} Changed");
        private readonly string StrWillBeAbleToLogin = Catalog.GetString("will be able to log in again");
        private readonly string StrWillNotBeAbleToLogin = Catalog.GetString("will not be able to log in anymore");
        private readonly string StrWillBeStillNotBeAbleToLogin = Catalog.GetString("will still not be able to log in because {0}");

        #endregion

        bool FNewRecordBeingAdded = false;
        ToolTip FTipPwd = new ToolTip();
        bool FChangesForCurrentUser = false;

        private void InitializeManualCode()
        {
            bool CanCreateUser;
            bool CanChangePassword;
            bool CanChangePermissions;

            sptContent.SplitterDistance = 260;

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

            txtDetailPasswordHash.Enter += TxtDetailPasswordHash_Enter;
            txtDetailPasswordHash.Click += TxtDetailPasswordHash_Click;
            txtDetailPasswordHash.KeyUp += TxtDetailPasswordHash_KeyUp;

            // SModuleTable is loaded with the users, therefore we can only now fill the checked list box.
            // TODO: should use cached table instead?
            LoadAvailableModulesIntoCheckedListBox();
        }

        private void TxtDetailPasswordHash_Enter(object sender, EventArgs e)
        {
            txtDetailPasswordHash.SelectAll();
        }

        private void TxtDetailPasswordHash_Click(object sender, EventArgs e)
        {
            txtDetailPasswordHash.SelectAll();
        }

        private void TxtDetailPasswordHash_KeyUp(object sender, KeyEventArgs e)
        {
            FTipPwd.Hide(txtDetailPasswordHash);
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
            clbUserGroup.AddTextColumn(Catalog.GetString("Module"), NewTable.Columns[ValueMember], 120);
            clbUserGroup.AddTextColumn(Catalog.GetString("Description"), NewTable.Columns[DisplayMember], 342);
            clbUserGroup.DataBindGrid(NewTable, ValueMember, CheckedMember, ValueMember, false, true, false);
        }

        private void LoadUsers()
        {
            FMainDS = TRemote.MSysMan.Maintenance.WebConnectors.LoadUsersAndModulePermissions();

            if (FMainDS != null)
            {
                DataView myDataView = FMainDS.SUser.DefaultView;
                myDataView.AllowNew = false;
                myDataView.Sort = "s_user_id_c ASC";
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            }
        }

        private TSubmitChangesResult StoreManualCode(ref MaintainUsersTDS ASubmitDS, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();

            TSubmitChangesResult Result = TRemote.MSysMan.Maintenance.WebConnectors.SaveSUser(ref ASubmitDS,
                TClientInfo.ClientComputerName, TClientInfo.ClientIPAddress);

            if (Result == TSubmitChangesResult.scrOK)
            {
                if (FChangesForCurrentUser)
                {
                    MessageBox.Show(Catalog.GetString("If you made any changes to your user they will only take effect at the next login!"),
                        Catalog.GetString("Maintain Users: Saving of Data Successful"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Reload the grid after every successful save. (This will add new password's hash and salt to the table.)
                Int32 rowIdx = GetSelectedRowIndex();
                FPreviouslySelectedDetailRow = null;
                LoadUsers();
                grdDetails.SelectRowWithoutFocus(rowIdx);

                ASubmitDS = FMainDS;

                btnResetPassword.Enabled = true;
                txtDetailPasswordHash.Enabled = false;
            }

            return Result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        /// <param name="ARow"></param>
        private void ShowDetailsManual(SUserRow ARow)
        {
            CheckForCurrentUser(ARow);
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

                // If a password has been saved for a user it can be changed using btnResetPassword.
                // If a password has not been saved then it can be added using txtDetailPasswordHash.
                if (string.IsNullOrEmpty(ARow.PasswordHash) || (string.IsNullOrEmpty(ARow.PasswordSalt) && (ARow.RowState != DataRowState.Unchanged)))
                {
                    btnResetPassword.Enabled = false;
                    txtDetailPasswordHash.Enabled = true;

                    if (FNewRecordBeingAdded)
                    {
                        FNewRecordBeingAdded = false;
                        FTipPwd.Show(Catalog.GetString(
                                "A 'random, secure' password got assigned.\r\nCopy it with the provided hyperlink!"),
                            txtDetailPasswordHash, 0, 25, 5000);
                    }
                    else
                    {
                        FTipPwd.Hide(txtDetailPasswordHash);
                    }

                    llbCopyPassword.Visible = true;
                }
                else
                {
                    btnResetPassword.Enabled = true;
                    txtDetailPasswordHash.Enabled = false;

                    llbCopyPassword.Visible = false;
                    FTipPwd.Hide(txtDetailPasswordHash);
                }
            }

            clbUserGroup.SetCheckedStringList(currentPermissions);
        }

        private void CheckForCurrentUser(SUserRow ARow)
        {
            if (ARow.UserId == UserInfo.GUserInfo.UserID)
            {
                FChangesForCurrentUser = true;
            }
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
            ARow.PasswordHash = TPasswordHelper.GetRandomSecurePassword();

            FNewRecordBeingAdded = true;
        }

        private void LockUnlockUser(Object Sender, EventArgs e)
        {
            DialogResult UserAnswer = MessageBox.Show(String.Format(StrAccountStateChangeQuestion,
                    GetSelectedDetailRow().AccountLocked ? "unlock" : "lock", txtDetailUserId.Text,
                    GetSelectedDetailRow().AccountLocked ? "locked" : "not locked"),
                Catalog.GetString("Lock/Unlock User Account?"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (UserAnswer == DialogResult.Yes)
            {
                chkDetailAccountLocked.Checked = !chkDetailAccountLocked.Checked;
                GetSelectedDetailRow().AccountLocked = !GetSelectedDetailRow().AccountLocked;
                GetSelectedDetailRow().FailedLogins = 0;
                FPetraUtilsObject.SetChangedFlag();

                MessageBox.Show(String.Format(StrAccountStateChangeConfirmation,
                        chkDetailAccountLocked.Checked ? Catalog.GetString("locked") : Catalog.GetString(
                            "unlocked and the user's Failed Login count was reset to 0"),
                        txtDetailUserId.Text, chkDetailAccountLocked.Checked ? StrWillNotBeAbleToLogin :
                        (!chkDetailRetired.Checked ? StrWillBeAbleToLogin : String.Format(StrWillBeStillNotBeAbleToLogin,
                             Catalog.GetString("the user is retired"))
                        )),
                    String.Format(StrAccountStateChangeConfirmationTitle, Catalog.GetString("Locked State")),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(Catalog.GetString("Lock state of User Account did not get changed."),
                    Catalog.GetString("User Account: Lock State Not Changed"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RetireUnretireUser(Object Sender, EventArgs e)
        {
            DialogResult UserAnswer = MessageBox.Show(String.Format(StrAccountStateChangeQuestion,
                    GetSelectedDetailRow().Retired ? "unretire" : "retire", txtDetailUserId.Text,
                    GetSelectedDetailRow().Retired ? "retired" : "not retired"),
                Catalog.GetString("Retire/Unretire User Account?"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (UserAnswer == DialogResult.Yes)
            {
                chkDetailRetired.Checked = !chkDetailRetired.Checked;
                GetSelectedDetailRow().Retired = !GetSelectedDetailRow().Retired;
                FPetraUtilsObject.SetChangedFlag();

                MessageBox.Show(String.Format(StrAccountStateChangeConfirmation,
                        chkDetailRetired.Checked ? Catalog.GetString("retired") : Catalog.GetString("unretired"),
                        txtDetailUserId.Text, chkDetailRetired.Checked ? StrWillNotBeAbleToLogin :
                        (!chkDetailAccountLocked.Checked ? StrWillBeAbleToLogin : String.Format(StrWillBeStillNotBeAbleToLogin,
                             Catalog.GetString("the User Account is Locked"))
                        )),
                    String.Format(StrAccountStateChangeConfirmationTitle, Catalog.GetString("Retired State")),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(Catalog.GetString("Retired state of User Account did not get changed."),
                    Catalog.GetString("User Account: Retired State Not Changed"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CopyPassword(Object Sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
            {
                CopyPasswordIntoClipboard(txtDetailPasswordHash.Text);
            }
        }

        private void CopyPasswordIntoClipboard(string APassword)
        {
            Clipboard.SetDataObject(APassword);

            MessageBox.Show(Catalog.GetString(
                    "The Password of this user record got copied to the clipboard.\r\n\r\nImmediately...\r\n  * Paste the password elsewhere; " +
                    "\r\n  * After you have done this click 'OK' to overwrite the password in the clipboard!!!"),
                Catalog.GetString("User Password Copied to Clipboard!"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            Clipboard.SetDataObject(String.Empty);
        }

        private void ShowResettingOfUserPwdCancelledMessage()
        {
            MessageBox.Show(Catalog.GetString("Resetting of User Password cancelled."),
                Catalog.GetString("User Password Reset Cancelled"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ResetPassword(Object Sender, EventArgs e)
        {
            TVerificationResultCollection VerificationResultCollection = null;
            string OneTimePassword = String.Empty;
            bool RandomSecurePasswordUtilised = false;

            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(
                    Catalog.GetString("It is necessary to save any changes before a user's password can be changed." +
                        Environment.NewLine + "Please save changes now and then repeat the operation."),
                    CommonDialogsResourcestrings.StrResetUserPasswordTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }

            string username = GetSelectedDetailRow().UserId;

            var UserChoice = MessageBox.Show(Catalog.GetString(
                    "The resetting of a User Password requires the creation of a one-time password that the user will need to enter.\r\n" +
                    "OpenPetra can generate a 'random secure' password for this purpose (recommended!). Alternatively, you can come up with such a password manually.\r\n\r\nCreate 'random secure' User Password?"),
                Catalog.GetString("User Password Reset"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            switch (UserChoice)
            {
                case DialogResult.Yes:
                    OneTimePassword = TPasswordHelper.GetRandomSecurePassword();
                    RandomSecurePasswordUtilised = true;

                    break;

                case DialogResult.No:
                    // only request the password once, since this is the sysadmin changing it.
                    // see http://bazaar.launchpad.net/~openpetracore/openpetraorg/trunkhosted/view/head:/csharp/ICT/Petra/Client/MSysMan/Gui/SysManMain.cs
                    // for the change password dialog for the normal user
                    PetraInputBox input = new PetraInputBox(
                    CommonDialogsResourcestrings.StrResetUserPasswordTitle,
                    String.Format(Catalog.GetString("Please enter a one-time password for user {0}:"), username),
                    "", true);

                    if (input.ShowDialog() == DialogResult.OK)
                    {
                        OneTimePassword = input.GetAnswer();

                        break;
                    }
                    else
                    {
                        ShowResettingOfUserPwdCancelledMessage();

                        return;
                    }

                case DialogResult.Cancel:
                    ShowResettingOfUserPwdCancelledMessage();

                    return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();  // give Windows a chance to update the Cursor

                // Save the new password (server-side checks get performed)
                if (TRemote.MSysMan.Maintenance.WebConnectors.SetUserPassword(username, OneTimePassword, true, true,
                        TClientInfo.ClientComputerName, TClientInfo.ClientIPAddress,
                        out VerificationResultCollection))
                {
                    MessageBox.Show(String.Format(Catalog.GetString(CommonDialogsResourcestrings.StrChangePasswordSuccess +
                                Environment.NewLine +
                                (RandomSecurePasswordUtilised ? Catalog.GetString(
                                     "The 'random secure' password will get copied to the clipboard after you have closed this message. Follow the steps lined out in the next message!")
                                 +
                                 Environment.NewLine : String.Empty) +
                                Environment.NewLine +
                                "(The user must change the new password for a password of his/her choice the next time (s)he logs on.)"),
                            username), CommonDialogsResourcestrings.StrResetUserPasswordTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (RandomSecurePasswordUtilised)
                    {
                        CopyPasswordIntoClipboard(OneTimePassword);
                    }

                    // This has been saved on the server so my data is out-of-date - re-loading needed to get new
                    // ModificationId etc:
                    FPreviouslySelectedDetailRow = null;
                    Int32 rowIdx = GetSelectedRowIndex();

                    LoadUsers();

                    grdDetails.SelectRowInGrid(rowIdx);
                }
                else
                {
                    MessageBox.Show(String.Format(CommonDialogsResourcestrings.StrChangePasswordError, username) +
                        Environment.NewLine + Environment.NewLine +
                        VerificationResultCollection.BuildVerificationResultString(),
                        CommonDialogsResourcestrings.StrResetUserPasswordTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ValidateDataDetailsManual(SUserRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            // validate bank account details
            TSharedSysManValidation.ValidateSUserDetails(this,
                ARow,
                ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }
    }
}