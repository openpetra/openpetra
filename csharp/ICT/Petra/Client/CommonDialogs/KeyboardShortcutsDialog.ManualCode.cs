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
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Gui;

namespace Ict.Petra.Client.CommonDialogs
{
    /// manual methods for the generated window
    public partial class TFrmKeyboardShortcutsDialog
    {
        /// <summary>
        /// Internal Table names used by the class
        /// </summary>
        public enum KeyboardShortcutTableNames
        {
            /// General table
            General,

            /// Navigation table
            Navigation,

            /// List table
            List,

            /// FilterFind table
            FilterFind
        };

        /// <summary>
        /// Our main DataSet that will have one table per screen tab
        /// </summary>
        private DataSet FMainDS = new DataSet();
        private string[] FColumnNames = new string[2] {
            Catalog.GetString("Shortcut"), Catalog.GetString("Description")
        };

        private void InitializeManualCode()
        {
            // Called from the constructor.  We initialise our data set
            string generalTable = KeyboardShortcutTableNames.General.ToString();

            AddTableToDataSet(generalTable);

            AddShortcutInfoToTable(generalTable, ApplWideResourcestrings.StrKeyShortcutCtrlC, ApplWideResourcestrings.StrKeyShortcutCtrlCHelp);
            AddShortcutInfoToTable(generalTable, ApplWideResourcestrings.StrKeyShortcutCtrlX, ApplWideResourcestrings.StrKeyShortcutCtrlXHelp);
            AddShortcutInfoToTable(generalTable, ApplWideResourcestrings.StrKeyShortcutCtrlV, ApplWideResourcestrings.StrKeyShortcutCtrlVHelp);
            AddShortcutInfoToTable(generalTable, ApplWideResourcestrings.StrKeyShortcutCtrlTab, ApplWideResourcestrings.StrKeyShortcutCtrlTabHelp);
            AddShortcutInfoToTable(generalTable,
                ApplWideResourcestrings.StrKeyShortcutShiftCtrlTab,
                ApplWideResourcestrings.StrKeyShortcutShiftCtrlTabHelp);
            AddShortcutInfoToTable(generalTable, ApplWideResourcestrings.StrKeyShortcutCtrlS, ApplWideResourcestrings.StrKeyShortcutCtrlSHelp);
            AddShortcutInfoToTable(generalTable, ApplWideResourcestrings.StrKeyShortcutCtrlP, ApplWideResourcestrings.StrKeyShortcutCtrlPHelp);
            AddShortcutInfoToTable(generalTable, ApplWideResourcestrings.StrKeyShortcutEscape, ApplWideResourcestrings.StrKeyShortcutEscapeHelp);

            string navigationTable = KeyboardShortcutTableNames.Navigation.ToString();
            AddTableToDataSet(navigationTable);

            AddShortcutInfoToTable(navigationTable,
                ApplWideResourcestrings.StrKeyShortcutCtrlHome,
                ApplWideResourcestrings.StrKeyShortcutCtrlHomeHelp);
            AddShortcutInfoToTable(navigationTable, ApplWideResourcestrings.StrKeyShortcutCtrlUp, ApplWideResourcestrings.StrKeyShortcutCtrlUpHelp);
            AddShortcutInfoToTable(navigationTable,
                ApplWideResourcestrings.StrKeyShortcutCtrlDown,
                ApplWideResourcestrings.StrKeyShortcutCtrlDownHelp);
            AddShortcutInfoToTable(navigationTable, ApplWideResourcestrings.StrKeyShortcutCtrlEnd, ApplWideResourcestrings.StrKeyShortcutCtrlEndHelp);
            AddShortcutInfoToTable(navigationTable, ApplWideResourcestrings.StrKeyShortcutCtrlL, ApplWideResourcestrings.StrKeyShortcutCtrlLHelp);

            string listTable = KeyboardShortcutTableNames.List.ToString();
            AddTableToDataSet(listTable);

            AddShortcutInfoToTable(listTable, ApplWideResourcestrings.StrKeyShortcutHome, ApplWideResourcestrings.StrKeyShortcutHomeHelp);
            AddShortcutInfoToTable(listTable, ApplWideResourcestrings.StrKeyShortcutPgUp, ApplWideResourcestrings.StrKeyShortcutPgUpHelp);
            AddShortcutInfoToTable(listTable, ApplWideResourcestrings.StrKeyShortcutUp, ApplWideResourcestrings.StrKeyShortcutUpHelp);
            AddShortcutInfoToTable(listTable, ApplWideResourcestrings.StrKeyShortcutDown, ApplWideResourcestrings.StrKeyShortcutDownHelp);
            AddShortcutInfoToTable(listTable, ApplWideResourcestrings.StrKeyShortcutPgDn, ApplWideResourcestrings.StrKeyShortcutPgDnHelp);
            AddShortcutInfoToTable(listTable, ApplWideResourcestrings.StrKeyShortcutEnd, ApplWideResourcestrings.StrKeyShortcutEndHelp);
            AddShortcutInfoToTable(listTable, ApplWideResourcestrings.StrKeyShortcutIns, ApplWideResourcestrings.StrKeyShortcutInsHelp);
            AddShortcutInfoToTable(listTable, ApplWideResourcestrings.StrKeyShortcutDel, ApplWideResourcestrings.StrKeyShortcutDelHelp);
            AddShortcutInfoToTable(listTable, ApplWideResourcestrings.StrKeyShortcutEnter, ApplWideResourcestrings.StrKeyShortcutEnterHelp);

            string filterFindTable = KeyboardShortcutTableNames.FilterFind.ToString();
            AddTableToDataSet(filterFindTable);

            AddShortcutInfoToTable(filterFindTable, ApplWideResourcestrings.StrKeyShortcutCtrlR, ApplWideResourcestrings.StrKeyShortcutCtrlRHelp);
            AddShortcutInfoToTable(filterFindTable, ApplWideResourcestrings.StrKeyShortcutCtrlF, ApplWideResourcestrings.StrKeyShortcutCtrlFHelp);
            AddShortcutInfoToTable(filterFindTable, ApplWideResourcestrings.StrKeyShortcutF3, ApplWideResourcestrings.StrKeyShortcutF3Help);
            AddShortcutInfoToTable(filterFindTable, ApplWideResourcestrings.StrKeyShortcutShiftF3, ApplWideResourcestrings.StrKeyShortcutShiftF3Help);

            this.btnCancel.Text = "&Close";
        }

        private void RunOnceOnActivationManual()
        {
            // Called when the screen has loaded
            tpgFilter.Text = Catalog.GetString("Filter and Find");
        }

        private void TabSelectionChanged(object sender, EventArgs e)
        {
            // Called when a tab is changed
            if (tabAllShortcuts.SelectedTab == tpgGeneral)
            {
                InitialiseTab(KeyboardShortcutTableNames.General, ucoShortcutsGeneral.HelpGrid, ucoShortcutsGeneral.DescriptionLabel);
            }
            else if (tabAllShortcuts.SelectedTab == tpgList)
            {
                InitialiseTab(KeyboardShortcutTableNames.List, ucoShortcutsList.HelpGrid, ucoShortcutsList.DescriptionLabel);
            }
            else if (tabAllShortcuts.SelectedTab == tpgNavigation)
            {
                InitialiseTab(KeyboardShortcutTableNames.Navigation, ucoShortcutsNavigation.HelpGrid, ucoShortcutsNavigation.DescriptionLabel);
            }
            else if (tabAllShortcuts.SelectedTab == tpgFilter)
            {
                InitialiseTab(KeyboardShortcutTableNames.FilterFind, ucoShortcutsFilterFind.HelpGrid, ucoShortcutsFilterFind.DescriptionLabel);
            }
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            // We do not show this button because the grid interferes with use of the ENTER key
        }

        /// <summary>
        /// Main method to initialise the grid and label on a specified tab
        /// </summary>
        /// <param name="ATableName"></param>
        /// <param name="AGrid"></param>
        /// <param name="ADescriptionLabel"></param>
        private void InitialiseTab(KeyboardShortcutTableNames ATableName, TSgrdDataGrid AGrid, Label ADescriptionLabel)
        {
            if (AGrid == null)
            {
                // The grid has already been initialised so we have nothing to do
                return;
            }

            ADescriptionLabel.AutoSize = false;
            ADescriptionLabel.Width = this.Width - 40;
            ADescriptionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            switch (ATableName)
            {
                case KeyboardShortcutTableNames.General:
                    ADescriptionLabel.Text = ApplWideResourcestrings.StrKeysHelpCategoryGeneral;
                    break;

                case KeyboardShortcutTableNames.List:
                    ADescriptionLabel.Text = ApplWideResourcestrings.StrKeysHelpCategoryList;
                    break;

                case KeyboardShortcutTableNames.Navigation:
                    ADescriptionLabel.Text = ApplWideResourcestrings.StrKeysHelpCategoryNavigation;
                    break;

                case KeyboardShortcutTableNames.FilterFind:
                    ADescriptionLabel.Text = ApplWideResourcestrings.StrKeysHelpCategoryFilterFind;
                    break;
            }

            DataTable table = FMainDS.Tables[ATableName.ToString()];

            AGrid.AddTextColumn(FColumnNames[0], table.Columns[0], 100);
            AGrid.AddTextColumn(FColumnNames[1], table.Columns[1], 250);
            AGrid.AutoStretchColumnsToFitWidth = true;
            AGrid.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.None;
            AGrid.Columns[1].AutoSizeMode = SourceGrid.AutoSizeMode.EnableStretch | SourceGrid.AutoSizeMode.EnableAutoSize;

            table.DefaultView.AllowNew = false;
            AGrid.DataSource = new DevAge.ComponentModel.BoundDataView(table.DefaultView);
            AGrid.AutoSizeCells(new SourceGrid.Range(1, 1, AGrid.Rows.Count - 1, 1));
        }

        /// <summary>
        /// Helper method to add a table to the DataSet
        /// </summary>
        /// <param name="ATableName"></param>
        private void AddTableToDataSet(string ATableName)
        {
            FMainDS.Tables.Add(ATableName);
            FMainDS.Tables[ATableName].Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn(FColumnNames[0], typeof(System.String)),
                    new DataColumn(FColumnNames[1], typeof(System.String))
                });
        }

        /// <summary>
        /// Helper method to add one item of data to a specified table
        /// </summary>
        /// <param name="ATableName"></param>
        /// <param name="AShortcut"></param>
        /// <param name="ADescription"></param>
        private void AddShortcutInfoToTable(string ATableName, string AShortcut, string ADescription)
        {
            DataTable table = FMainDS.Tables[ATableName];
            DataRow row = table.NewRow();

            row[0] = AShortcut;
            row[1] = ADescription;
            table.Rows.Add(row);
        }
    }
}