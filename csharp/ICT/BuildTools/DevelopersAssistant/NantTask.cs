//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2011 by OM International
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

namespace Ict.Tools.DevelopersAssistant
{
    /**************************************************************************************************************************************************
     *
     * This class exists solely to manage the strings associated with each Nant task.
     * The programmer instantiates this class, either using the task enumeration, or by passing a string that describes what the task does.
     * The other strings associated with the task are then available as public properties.
     * So, for example, the stringto put in the output log, or the string to pass to the Nant target name.
     *
     * The enumeration should, where possible, be the Nant target name.  This is not possible with quickCompile where there are multiple options
     *    for the same target name
     *
     * ***********************************************************************************************************************************************/

    /// <summary>
    /// A class that manages the strings associated with each Nant task
    /// </summary>
    public class NantTask
    {
        /// <summary>
        /// One item for each Nant target that the Assistant supports.
        ///   Where possible the enumeration should be the parameter string for Nant
        /// </summary>
        public enum TaskItem
        {
            /// <summary>
            /// Dummy placeholder for position 0
            /// </summary>
            None,
            // Basic items ---------------------------------
            /// <summary>
            /// Start the server
            /// </summary>
            startPetraServer,
            /// <summary>
            /// Stop the server
            /// </summary>
            stopPetraServer,
            /// <summary>
            /// Start the client
            /// </summary>
            startPetraClient,
            /// <summary>
            /// Generate a Windows Form
            /// </summary>
            generateWinform,
            /// <summary>
            /// Preview a previously generated Windows Form
            /// </summary>
            previewWinform,
            // Code generation ---------------------------------------
            /// <summary>
            /// Generate the solution without compilation
            /// </summary>
            generateSolutionNoCompile = 11,
            /// <summary>
            /// Generate the solution with only a minimal compile of critical components
            /// </summary>
            minimalGenerateSolution,
            /// <summary>
            /// Generate he solution with full compile
            /// </summary>
            generateSolution,
            /// <summary>
            /// Generate object relational mapper code
            /// </summary>
            generateORM,
            /// <summary>
            /// Generate the cahed tables code
            /// </summary>
            generateORMCachedTables,
            /// <summary>
            /// Generate ORM Data
            /// </summary>
            generateORMData,
            /// <summary>
            /// Generate ORM Access
            /// </summary>
            generateORMAccess,
            /// <summary>
            /// Generate glue
            /// </summary>
            generateGlue,
            /// <summary>
            /// Generate all WinForms
            /// </summary>
            generateWinforms,
            // Compilation ----------------------------------------------------
            /// <summary>
            /// Clean the folders prior to new build
            /// </summary>
            clean = 21,
            /// <summary>
            /// Perform a 'quick' clean
            /// </summary>
            quickClean,
            /// <summary>
            /// Comile the complete solution
            /// </summary>
            compile,
            /// <summary>
            /// Quick compile the complete solution
            /// </summary>
            quickCompile,
            /// <summary>
            /// Quick compile the server solution only
            /// </summary>
            quickCompileServer,
            /// <summary>
            /// Quick compile the client solution only
            /// </summary>
            quickCompileClient,
            /// <summary>
            /// Quick compile the testing solution only
            /// </summary>
            quickCompileTesting,
            /// <summary>
            /// Quick compile the tools solution only
            /// </summary>
            quickCompileTools,
            // Miscellaneous -----------------------------------------------------
            /// <summary>
            /// Initialise the configuration files
            /// </summary>
            initConfigFiles = 31,
            /// <summary>
            /// Delete all backup files
            /// </summary>
            deleteBakFiles,
            /// <summary>
            /// Uncrustify the source files
            /// </summary>
            uncrustify,
            // Database ---------------------------------------------------------
            /// <summary>
            /// Recreate the database
            /// </summary>
            recreateDatabase = 41,
            /// <summary>
            /// Reset the database content
            /// </summary>
            resetDatabase,
            /// <summary>
            /// Patch the current database
            /// </summary>
            patchDatabase
        }
        private TaskItem _taskItem = TaskItem.None;

        // Public properties

        /// <summary>
        /// Gets the TaskItem for this class instance
        /// </summary>
        public TaskItem Item {
            get
            {
                return _taskItem;
            }
        }
        /// <summary>
        /// Gets the first 'basic' item in the enumeration
        /// </summary>
        public static TaskItem FirstBasicItem {
            get
            {
                return TaskItem.startPetraServer;
            }
        }
        /// <summary>
        /// Gets the last 'basic' item in the enumeration
        /// </summary>
        public static TaskItem LastBasicItem {
            get
            {
                return TaskItem.generateWinform;
            }
        }
        /// <summary>
        /// Gets the first 'code generation' item in the enumeration
        /// </summary>
        public static TaskItem FirstCodeGenItem {
            get
            {
                return TaskItem.generateSolutionNoCompile;
            }
        }
        /// <summary>
        /// Gets the last 'code generation' item in the enumeration
        /// </summary>
        public static TaskItem LastCodeGenItem {
            get
            {
                return TaskItem.generateWinforms;
            }
        }
        /// <summary>
        /// Gets the first 'compilation' item in the enumeration
        /// </summary>
        public static TaskItem FirstCompileItem {
            get
            {
                return TaskItem.clean;
            }
        }
        /// <summary>
        /// Gets the last 'compilation' item in the enumeration
        /// </summary>
        public static TaskItem LastCompileItem {
            get
            {
                return TaskItem.quickCompileTools;
            }
        }
        /// <summary>
        /// Gets the first 'miscellaneous' item in the enumeration
        /// </summary>
        public static TaskItem FirstMiscItem {
            get
            {
                return TaskItem.initConfigFiles;
            }
        }
        /// <summary>
        /// Gets the last 'miscellaneous' item in the enumeration
        /// </summary>
        public static TaskItem LastMiscItem {
            get
            {
                return TaskItem.uncrustify;
            }
        }
        /// <summary>
        /// Gets the first 'database' item in the enumeration
        /// </summary>
        public static TaskItem FirstDatabaseItem {
            get
            {
                return TaskItem.recreateDatabase;
            }
        }
        /// <summary>
        /// Gets the last 'database' item in the enumeration
        /// </summary>
        public static TaskItem LastDatabaseItem {
            get
            {
                return TaskItem.patchDatabase;
            }
        }

        /// <summary>
        /// Constructor that uses the string passed from a comboBox or task sequence list
        /// </summary>
        /// <param name="TaskName">A string representation of the task that is parsed to determine the specific task</param>
        public NantTask(string TaskName)
        {
            // This constructor is used by tasks initiated from a comboBox list
            if (TaskName.IndexOf("quick", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                if (TaskName.IndexOf("clean", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.quickClean;
                }
                else if (TaskName.IndexOf("complete", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.quickCompile;
                }
                else if (TaskName.IndexOf("server", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.quickCompileServer;
                }
                else if (TaskName.IndexOf("client", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.quickCompileClient;
                }
                else if (TaskName.IndexOf("testing", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.quickCompileTesting;
                }
                else if (TaskName.IndexOf("tools", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.quickCompileTools;
                }
            }
            else
            {
                if (TaskName.IndexOf("clean", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.clean;
                }
                else if (TaskName.IndexOf("uncrustify", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.uncrustify;
                }
                else if (TaskName.IndexOf("preview", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.previewWinform;
                }
                else if (TaskName.IndexOf("Mapper", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.generateORM;
                }
                else if (TaskName.IndexOf(" glue", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.generateGlue;
                }
                else if (TaskName.IndexOf(" backup ", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.deleteBakFiles;
                }
                else if (TaskName.IndexOf(" ORM Access ", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.generateORMAccess;
                }
                else if (TaskName.IndexOf(" ORM Cached ", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.generateORMCachedTables;
                }
                else if (TaskName.IndexOf(" ORM Data", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.generateORMData;
                }
                else if (TaskName.IndexOf("with no ", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.generateSolutionNoCompile;
                }
                else if (TaskName.IndexOf("with mini", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.minimalGenerateSolution;
                }
                else if (TaskName.IndexOf("with full", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.generateSolution;
                }
                else if (TaskName.IndexOf("all the windows", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.generateWinforms;
                }
                else if (TaskName.IndexOf("dows form", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.generateWinform;
                }
                else if (TaskName.IndexOf("art serv", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.startPetraServer;
                }
                else if (TaskName.IndexOf("top serv", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.stopPetraServer;
                }
                else if (TaskName.IndexOf("art cli", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.startPetraClient;
                }
                else if (TaskName.IndexOf("full com", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.compile;
                }
                else if (TaskName.IndexOf("config", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.initConfigFiles;
                }
                else if (TaskName.IndexOf("create", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.recreateDatabase;
                }
                else if (TaskName.IndexOf("reset", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.resetDatabase;
                }
                else if (TaskName.IndexOf("patch", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _taskItem = TaskItem.patchDatabase;
                }
            }
        }

        /// <summary>
        /// Constructor that uses an item enumeration
        /// </summary>
        /// <param name="Item"></param>
        public NantTask(TaskItem Item)
        {
            // This constructor is used by tasks initiated from specific buttons
            _taskItem = Item;
        }

        /***************************************************************************************************************
         *
         * Methods that return a string relevant to the current instantiated task.
         *
         * ************************************************************************************************************/

        /// <summary>
        /// The test that is displayed on the splash screen while a task runs
        /// </summary>
        public string StatusText
        {
            get
            {
                switch (_taskItem)
                {
                    case TaskItem.clean: return "Performing full clean.  Please be patient ...";

                    case TaskItem.compile: return "Performing full compile.  This could take several minutes ...";

                    case TaskItem.deleteBakFiles: return "Deleting backup files.  Please wait ...";

                    case TaskItem.generateORM: return "Performing task to generate Object-Relational mapper code.  Please be patient ...";

                    case TaskItem.generateGlue: return "Generating glue.  Please wait ...";

                    case TaskItem.generateORMAccess: return "Generating ORM access.  Please wait ...";

                    case TaskItem.generateORMCachedTables: return "Generating ORM cached tables.  Please wait ...";

                    case TaskItem.generateORMData: return "Generating ORM data.  Please wait ...";

                    case TaskItem.generateSolution: return "Generating solution with full compile.  This could take several minutes ...";

                    case TaskItem.generateSolutionNoCompile: return "Generating solution without compilation.  This could take a minute or two ...";

                    case TaskItem.minimalGenerateSolution: return
                            "Generating solution with a quick (minimal) compile.  This could take a minute or two ...";

                    case TaskItem.generateWinform: return "Generating Windows form from YAML ...";

                    case TaskItem.generateWinforms: return "Generating all the Windows forms ...";

                    case TaskItem.previewWinform: return "Creating a preview of a Windows form ...";

                    case TaskItem.initConfigFiles: return "Initialising configuration files.  Please be patient ...";

                    case TaskItem.patchDatabase: return "Patching (updating) your existing database.  Please be patient ...";

                    case TaskItem.quickClean: return "Performing Quick clean ...";

                    case TaskItem.quickCompile: return "Performing full compile of all projects.  This could take several minutes ...";

                    case TaskItem.quickCompileClient: return "Performing compile of client solution.  This could take a minute or two ...";

                    case TaskItem.quickCompileServer: return "Performing compile of server solution.  This could take a minute or two ...";

                    case TaskItem.quickCompileTesting: return "Performing compile of testing solution.  This could take a minute or two ...";

                    case TaskItem.quickCompileTools: return "Performing compile of tools solution.  This could take a minute or two ...";

                    case TaskItem.recreateDatabase: return "Recreating database ... Please wait ...";

                    case TaskItem.resetDatabase: return "Resetting database ... Please wait ...";

                    case TaskItem.startPetraClient: return "Starting OpenPetra Client ...";

                    case TaskItem.startPetraServer: return "Starting OpenPetra Server ... Please wait ...";

                    case TaskItem.stopPetraServer: return "Stopping OpenPetra Server ... Please wait ...";

                    case TaskItem.uncrustify: return "Uncrustifying the source code ... Please wait ...";

                    default: return "Unknown task title";
                }
            }
        }

        /// <summary>
        /// The target name that is the parameter for nant.bat
        /// </summary>
        public string TargetName
        {
            get
            {
                switch (_taskItem)
                {
                    case TaskItem.quickCompileClient: return "quickCompile -D:solution=Client";

                    case TaskItem.quickCompileServer: return "quickCompile -D:solution=Server";

                    case TaskItem.quickCompileTesting: return "quickCompile -D:solution=Testing";

                    case TaskItem.quickCompileTools: return "quickCompile -D:solution=Tools";

                    default: return _taskItem.ToString();
                }
            }
        }

        /// <summary>
        /// The text that is inserted in the output log
        /// </summary>
        public string LogText
        {
            get
            {
                switch (_taskItem)
                {
                    case TaskItem.clean: return "Starting full clean";

                    case TaskItem.compile: return "Starting full compile";

                    case TaskItem.deleteBakFiles: return "Starting deleteBakFiles";

                    case TaskItem.generateORM: return "Starting generate ORM";

                    case TaskItem.generateGlue: return "Starting generate glue";

                    case TaskItem.generateORMAccess: return "Starting generate ORM access";

                    case TaskItem.generateORMCachedTables: return "Starting generate ORM cached tables";

                    case TaskItem.generateORMData: return "Starting generate ORM data";

                    case TaskItem.generateSolution: return "Starting generateSolution with full compile";

                    case TaskItem.generateSolutionNoCompile: return "Starting generateSolution with NoCompile";

                    case TaskItem.minimalGenerateSolution: return "Starting minimalGenerateSolution";

                    case TaskItem.generateWinform: return "Starting generation of Windows form from YAML";

                    case TaskItem.generateWinforms: return "Starting generation of all the Windows forms";

                    case TaskItem.previewWinform: return "Starting generation of Windows form preview";

                    case TaskItem.initConfigFiles: return "Starting initConfigFiles";

                    case TaskItem.patchDatabase: return "Starting patchDatabase";

                    case TaskItem.quickClean: return "Starting quickClean";

                    case TaskItem.quickCompile: return "Starting quickCompile of OpenPetra.sln";

                    case TaskItem.quickCompileClient: return "Starting quickCompile of Client solution";

                    case TaskItem.quickCompileServer: return "Starting quickCompile of Server solution";

                    case TaskItem.quickCompileTesting: return "Starting quickCompile of Testing solution";

                    case TaskItem.quickCompileTools: return "Starting quickCompile of Tools solution";

                    case TaskItem.recreateDatabase: return "Starting Re-create database";

                    case TaskItem.resetDatabase: return "Starting Re-set database";

                    case TaskItem.startPetraClient: return "Starting OpenPetra client";

                    case TaskItem.startPetraServer: return "Starting OpenPetra server";

                    case TaskItem.stopPetraServer: return "Stopping OpenPetra server";

                    case TaskItem.uncrustify: return "Uncrustifying source code";

                    default: return "";
                }
            }
        }

        /// <summary>
        /// The task description used, for example, when selecting a task in a sequence
        /// </summary>
        public string Description
        {
            get
            {
                switch (_taskItem)
                {
                    case TaskItem.clean: return "Perform a full clean";

                    case TaskItem.compile: return "Perform a full compile";

                    case TaskItem.deleteBakFiles: return "Delete all backup files";

                    case TaskItem.generateORM: return "Generate the Object-Relational Mapper (ORM)";

                    case TaskItem.generateGlue: return "Generate the glue";

                    case TaskItem.generateORMAccess: return "Generate the ORM access files";

                    case TaskItem.generateORMCachedTables: return "Generate the ORM cached tables";

                    case TaskItem.generateORMData: return "Generate the ORM data";

                    case TaskItem.generateSolution: return "Generate the solution with full compile";

                    case TaskItem.generateSolutionNoCompile: return "Generate the solution with no compile";

                    case TaskItem.generateWinform: return "Generate a Windows form";

                    case TaskItem.generateWinforms: return "Generate all the Windows forms";

                    case TaskItem.previewWinform: return "Preview a Windows form";

                    case TaskItem.initConfigFiles: return "Initialise the configuration files";

                    case TaskItem.patchDatabase: return "Patch (update) the existing database";

                    case TaskItem.minimalGenerateSolution: return "Generate the solution with minimal compile";

                    case TaskItem.quickClean: return "Perform a quick clean";

                    case TaskItem.quickCompile: return "Perform a quick compile of the complete solution";

                    case TaskItem.quickCompileClient: return "Perform a quick compile of the client solution";

                    case TaskItem.quickCompileServer: return "Perform a quick compile of the server solution";

                    case TaskItem.quickCompileTesting: return "Perform a quick compile of the testing solution";

                    case TaskItem.quickCompileTools: return "Perform a quick compile of the tools solution";

                    case TaskItem.recreateDatabase: return "Re-create the complete database";

                    case TaskItem.resetDatabase: return "Reset the database content";

                    case TaskItem.startPetraClient: return "Start client";

                    case TaskItem.startPetraServer: return "Start server (if it is not running)";

                    case TaskItem.stopPetraServer: return "Stop server (if it is running)";

                    case TaskItem.uncrustify: return "Uncrustify the source code";

                    default: return "Not known";
                }
            }
        }

        /// <summary>
        /// A shorter description used, for example, in the drop-down lists of task options
        /// </summary>
        public string ShortDescription
        {
            get
            {
                switch (_taskItem)
                {
                    case TaskItem.clean: return "Full clean";

                    case TaskItem.compile: return "Full compile";

                    case TaskItem.deleteBakFiles: return "Delete backup files";

                    case TaskItem.generateORM: return "Generate the Object-Relational Mapper (ORM)";

                    case TaskItem.generateGlue: return "Generate the glue";

                    case TaskItem.generateORMAccess: return "Generate the ORM access files";

                    case TaskItem.generateORMCachedTables: return "Generate the ORM cached tables";

                    case TaskItem.generateORMData: return "Generate the ORM data";

                    case TaskItem.generateSolution: return "Generate the solution with full compile";

                    case TaskItem.generateSolutionNoCompile: return "Generate the solution with no compile";

                    case TaskItem.generateWinform: return "Generate a Windows form";

                    case TaskItem.generateWinforms: return "Generate all the Windows forms";

                    case TaskItem.previewWinform: return "Preview a Windows form";

                    case TaskItem.initConfigFiles: return "Initialise the configuration files";

                    case TaskItem.patchDatabase: return "Patch the existing database";

                    case TaskItem.minimalGenerateSolution: return "Generate the solution with minimal compile";

                    case TaskItem.quickClean: return "Quick clean";

                    case TaskItem.quickCompile: return "Quick compile of complete solution";

                    case TaskItem.quickCompileClient: return "Quick compile of client solution";

                    case TaskItem.quickCompileServer: return "Quick compile of server solution";

                    case TaskItem.quickCompileTesting: return "Quick compile of testing solution";

                    case TaskItem.quickCompileTools: return "Quick compile of tools solution";

                    case TaskItem.recreateDatabase: return "Re-create the complete database";

                    case TaskItem.resetDatabase: return "Reset the database content";

                    case TaskItem.startPetraClient: return "Start client";

                    case TaskItem.startPetraServer: return "Start server";

                    case TaskItem.stopPetraServer: return "Stop server";

                    case TaskItem.uncrustify: return "Uncrustify the source code";

                    default: return "Not known";
                }
            }
        }
    }
}