//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2013 by OM International
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
using System.Windows.Forms;

namespace Ict.Tools.DevelopersAssistant
{
    /**************************************************************************************************************************************************
     *
     * This class exists solely to manage the strings associated with each Bazaar task.
     * The programmer instantiates this class, either using the task enumeration, or by passing a string that describes what the task does.
     * The other strings associated with the task are then available as public properties.
     *
     * ***********************************************************************************************************************************************/

    /// <summary>
    /// A class that manages the strings associated with each Nant task
    /// </summary>
    public class BazaarTask
    {
        /// <summary>
        /// One item for each Bazaar action that the Assistant supports.
        /// </summary>
        public enum TaskItem
        {
            /// <summary>
            /// Dummy placeholder for position 0
            /// </summary>
            None,

            /// <summary>
            /// View the History Log for all files
            /// </summary>
            qlog,

            /// <summary>
            /// View the History Log for a specific file
            /// </summary>
            qlogFile,

            /// <summary>
            /// View all the current differences
            /// </summary>
            qdiff,

            /// <summary>
            /// View the current differences for a specific file
            /// </summary>
            qdiffFile,

            /// <summary>
            /// Commit outstanding changes in the current branch
            /// </summary>
            qcommit,

            /// <summary>
            /// Add unversioned files in the current branch
            /// </summary>
            qadd,

            /// <summary>
            /// Browse the file system for the current branch
            /// </summary>
            qbrowse,

            /// <summary>
            /// Shelve selected changes
            /// </summary>
            qshelve,

            /// <summary>
            /// Un-shelve previously shelved changes
            /// </summary>
            qunshelve,

            /// <summary>
            /// Pull the latest changes from trunk
            /// </summary>
            qmerge,

            /// <summary>
            /// Resolve outstanding conflicts
            /// </summary>
            qconflicts,

            /// <summary>
            /// Pull changes into a local copy of trunk
            /// </summary>
            qpull,

            /// <summary>
            /// Update a local copy of trunk
            /// </summary>
            qupdate,

            /// <summary>
            /// Create a new branch on Launchpad
            /// </summary>
            qbranch,

            /// <summary>
            /// View the Mantis case history for a specific bug
            /// </summary>
            bugUrl,

            /// <summary>
            /// Open Windows Explorer in the current branch location
            /// </summary>
            winexplore,

            /// <summary>
            /// Internal task associated with branch creation
            /// </summary>
            qbranch2
        }
        private TaskItem _taskItem = TaskItem.None;

        // Public properties

        /// <summary>
        /// Gets the TaskItem for this class instance
        /// </summary>
        public TaskItem Item
        {
            get
            {
                return _taskItem;
            }
        }

        /// <summary>
        /// Constructor that uses an item enumeration
        /// </summary>
        /// <param name="Item"></param>
        public BazaarTask(TaskItem Item)
        {
            // This constructor is used by tasks initiated from specific buttons
            _taskItem = Item;
        }

        /// <summary>
        /// Gets the first 'miscellaneous' item in the enumeration
        /// </summary>
        public static TaskItem FirstBazaarItem
        {
            get
            {
                return TaskItem.qlog;
            }
        }
        /// <summary>
        /// Gets the last 'miscellaneous' item in the enumeration
        /// </summary>
        public static TaskItem LastBazaarItem
        {
            get
            {
                return TaskItem.winexplore;
            }
        }


        /***************************************************************************************************************
         *
         * Methods that return a string relevant to the current instantiated task.
         *
         * ************************************************************************************************************/

        /// <summary>
        /// The test that is displayed on the splash screen while a task runs
        /// </summary>
        public string Description
        {
            get
            {
                switch (_taskItem)
                {
                    case TaskItem.qlog:
                        return "Show history log";

                    case TaskItem.qlogFile:
                        return "   Show history log for a file";

                    case TaskItem.qdiff:
                        return "Show current differences";

                    case TaskItem.qdiffFile:
                        return "   Show differences for a file";

                    case TaskItem.qcommit:
                        return "Commit some or all current changes";

                    case TaskItem.qadd:
                        return "   Add unversioned files";

                    case TaskItem.qbrowse:
                        return "   Browse folders for this branch";

                    case TaskItem.qshelve:
                        return "   Shelve selected changes";

                    case TaskItem.qunshelve:
                        return "   Unshelve saved changes";

                    case TaskItem.qmerge:
                        return "Merge revisions from trunk";

                    case TaskItem.qconflicts:
                        return "   Check and resolve conflicts";

                    case TaskItem.qpull:
                        return "   Pull (mirror) changes into a local copy of trunk";

                    case TaskItem.qupdate:
                        return "   Update a local copy of trunk from trunk";

                    case TaskItem.qbranch:
                        return "Create a new branch";

                    case TaskItem.bugUrl:
                        return "   Review a specific Bug ID";

                    case TaskItem.winexplore:
                        return "Open Windows Explorer ( for Tortoise Bazaar )";

                    default:
                        return "Unknown task";
                }
            }
        }

        /// <summary>
        /// Gets the command line arguments for the current task
        /// </summary>
        /// <param name="BranchLocation">Full path to the current branch location (or the new branch location for qbranch)</param>
        /// <param name="LaunchpadUrl">Full URL to Launchpad for the current user and current branch (or the new branch for qbranch)</param>
        /// <returns>String containg the command line args</returns>
        public string GetBazaarArgs(string BranchLocation, string LaunchpadUrl)
        {
            switch (_taskItem)
            {
                case TaskItem.qlog:
                    return "qlog";

                case TaskItem.qlogFile:
                    OpenFileDialog dlgLog = new OpenFileDialog();
                    dlgLog.InitialDirectory = BranchLocation;
                    dlgLog.Multiselect = false;

                    if (dlgLog.ShowDialog() == DialogResult.Cancel)
                    {
                        return String.Empty;
                    }

                    return String.Format("qlog \"{0}\"", dlgLog.FileName);

                case TaskItem.qdiff:
                    return "qdiff";

                case TaskItem.qdiffFile:
                    OpenFileDialog dlgDiff = new OpenFileDialog();
                    dlgDiff.InitialDirectory = BranchLocation;
                    dlgDiff.Multiselect = false;

                    if (dlgDiff.ShowDialog() == DialogResult.Cancel)
                    {
                        return String.Empty;
                    }

                    return String.Format("qdiff \"{0}\"", dlgDiff.FileName);

                case TaskItem.qbrowse:
                    return "qbrowse";

                case TaskItem.qadd:
                    return "qadd --ui-mode";

                case TaskItem.qcommit:
                    return "qcommit --ui-mode";

                case TaskItem.qshelve:
                    return "qshelve --ui-mode";

                case TaskItem.qunshelve:
                    return "qunshelve --ui-mode";

                case TaskItem.qmerge:
                    return "qmerge lp:/openpetraorg --ui-mode";

                case TaskItem.qconflicts:
                    return "qconflicts";

                case TaskItem.qpull:
                    return "qpull lp:/openpetraorg --ui-mode";

                case TaskItem.qupdate:
                    return "qupdate --ui-mode";

                case TaskItem.bugUrl:
                    DlgBugNumber dlgBug = new DlgBugNumber();

                    if (dlgBug.ShowDialog() != DialogResult.OK)
                    {
                        return String.Empty;
                    }

                    return String.Format("bug-url op:{0} --open", dlgBug.txtBugNumber.Text);

                case TaskItem.qbranch:
                    return String.Format("qbranch lp:/openpetraorg {0} --ui-mode", LaunchpadUrl);

                case TaskItem.qbranch2:
                    return String.Format("qbranch {0} \"{1}\" --bind --ui-mode", LaunchpadUrl, BranchLocation);

                case TaskItem.winexplore:
                    return String.Format("/C explorer.exe \"{0}\"", BranchLocation);

                default:
                    return String.Empty;
            }
        }

        /// <summary>
        /// Gets the executable command for the current task
        /// </summary>
        /// <returns>A string value to pass to 'StartProcess'</returns>
        public string GetBazaarCommand()
        {
            switch (_taskItem)
            {
                case TaskItem.winexplore:
                    return "cmd.exe";

                default:
                    return "bzrw.exe";
            }
        }
    }
}