//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2014 by OM International
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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Ict.Tools.DevelopersAssistant
{
    class CheckForUpdates
    {
        /// <summary>
        /// A static method to check if the branch location contains a newer version of OPDA than the one we are running.
        /// The file must exist (ie must have been compiled).  The user is asked whether to upgrade or not.
        /// </summary>
        /// <param name="InBranchLocation">The root path for the branch location</param>
        /// <param name="IsManualCheck">Set to true if the check was initiated manually, or false if it was initiated automatically.
        /// The difference is that an automatic check does not display messages if there is no update available.</param>
        /// <returns></returns>
        public static bool DoCheck(string InBranchLocation, bool IsManualCheck)
        {
            string exePathInBranch = Path.Combine(InBranchLocation, "delivery/bin/Ict.Tools.DevelopersAssistant.exe");

            if (!File.Exists(exePathInBranch))
            {
                return false;
            }

            FileVersionInfo thisInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            FileVersionInfo branchInfo = FileVersionInfo.GetVersionInfo(exePathInBranch);

            bool isUpdate = false;

            if (branchInfo.FileMajorPart > thisInfo.FileMajorPart)
            {
                isUpdate = true;
            }
            else if (branchInfo.FileMajorPart == thisInfo.FileMajorPart)
            {
                if (branchInfo.FileMinorPart > thisInfo.FileMinorPart)
                {
                    isUpdate = true;
                }
                else if (branchInfo.FileMinorPart == thisInfo.FileMinorPart)
                {
                    if (branchInfo.FileBuildPart > thisInfo.FileBuildPart)
                    {
                        isUpdate = true;
                    }
                    else if (branchInfo.FileBuildPart == thisInfo.FileBuildPart)
                    {
                        if (branchInfo.FilePrivatePart > thisInfo.FilePrivatePart)
                        {
                            isUpdate = true;
                        }
                    }
                }
            }

            if (isUpdate)
            {
                string msg = "A newer version of the Developer's Assistant exists in the branch location. ";
                msg += String.Format("Would you like to update your working version from {0} to {1} now?",
                    thisInfo.FileVersion, branchInfo.FileVersion);

                if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    return false;
                }

                msg =
                    "The Assistant will save your settings and then shut down.  When the new version has been installed it will restart automatically.";

                if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return false;
                }

                // Update the Assistant to the newer version
                return true;
            }
            else if (IsManualCheck)
            {
                if ((branchInfo.FileMajorPart == thisInfo.FileMajorPart)
                    && (branchInfo.FileMinorPart == thisInfo.FileMinorPart)
                    && (branchInfo.FileBuildPart == thisInfo.FileBuildPart)
                    && (branchInfo.FilePrivatePart == thisInfo.FilePrivatePart))
                {
                    MessageBox.Show("The branch version is the same as the current version.",
                        Program.APP_TITLE,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("The branch version is older than the current version.",
                        Program.APP_TITLE,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }

            return false;
        }
    }
}