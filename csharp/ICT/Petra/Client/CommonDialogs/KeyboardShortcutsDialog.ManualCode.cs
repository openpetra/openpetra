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
using System.Collections;
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
            FilterFind,

            /// Dates table
            Dates,

            /// Main Menu screen table
            MainMenu,

            /// PartnerEditContactDetailsTab table
            PartnerEditContactDetailsTab
        };

        /// <summary>
        /// Determines the Tab that gets initially displayed. Must be set before the Form gets shown to have an effect!
        /// </summary>
        public string InitiallySelectedTab
        {
            get
            {
                return FInitiallySelectedTab;
            }

            set
            {
                FInitiallySelectedTab = value;
            }
        }

        /// <summary>
        /// Our main DataSet that will have one table per screen tab
        /// </summary>
        private DataSet FMainDS = new DataSet();
        private string[] FColumnNames = new string[2] {
            Catalog.GetString("Shortcut"), Catalog.GetString("Description")
        };
        private string FInitiallySelectedTab = String.Empty;

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
            AddShortcutInfoToTable(navigationTable, ApplWideResourcestrings.StrKeyShortcutCtrlE, ApplWideResourcestrings.StrKeyShortcutCtrlEHelp);

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

            string datesTable = KeyboardShortcutTableNames.Dates.ToString();
            AddTableToDataSet(datesTable);

            AddShortcutInfoToTable(datesTable, Catalog.GetString("= + today"), Catalog.GetString(
                    "Enters today's date into the text area when you leave the control"));
            AddShortcutInfoToTable(datesTable, Catalog.GetString("+NN"), Catalog.GetString(
                    "Enters the date NN days after today when you leave the control.  NN is an integer number."));
            AddShortcutInfoToTable(datesTable, Catalog.GetString("-NN"), Catalog.GetString(
                    "Enters the date NN days before today when you leave the control.  NN is an integer number."));
            AddShortcutInfoToTable(datesTable, Catalog.GetString("201115 or 20112015"), Catalog.GetString(
                    "Enters 20-NOV-2015 when you leave the control if the short date format in Control Panel is like Day-Month-Year"));
            AddShortcutInfoToTable(datesTable, Catalog.GetString("151120 or 20151120"), Catalog.GetString(
                    "Enters 20-NOV-2015 when you leave the control if the short date format in Control Panel is like Year-Month-Day"));
            AddShortcutInfoToTable(datesTable, Catalog.GetString("112015 or 11202015"), Catalog.GetString(
                    "Enters 20-NOV-2015 when you leave the control if the short date format in Control Panel is like Month-Day-Year"));

            string mainMenuTable = KeyboardShortcutTableNames.MainMenu.ToString();
            AddTableToDataSet(mainMenuTable);

            AddShortcutInfoToTable(mainMenuTable, Catalog.GetString("TAB / Shift+TAB"), Catalog.GetString(
                    "Move forwards/backwards through the Task Items on the right side of the screen and Sub-Modules on the upper left."));
            AddShortcutInfoToTable(mainMenuTable, Catalog.GetString("Home"), Catalog.GetString(
                    "Move to the first Task Item."));
            AddShortcutInfoToTable(mainMenuTable, Catalog.GetString("End"), Catalog.GetString(
                    "Move to the last Task Item."));
            AddShortcutInfoToTable(mainMenuTable, Catalog.GetString("+"), Catalog.GetString(
                    "Move to the first Task Item in the next Task Group."));
            AddShortcutInfoToTable(mainMenuTable, Catalog.GetString("-"), Catalog.GetString(
                    "Move to the first Task Item in the previous Task Group."));
            AddShortcutInfoToTable(mainMenuTable, Catalog.GetString("PgUp"), Catalog.GetString(
                    "Show the Task List for the previous Sub-Module listed at the upper left of the screen."));
            AddShortcutInfoToTable(mainMenuTable, Catalog.GetString("PgDn"), Catalog.GetString(
                    "Show the Task List for the next Sub-Module listed at the upper left of the screen."));
            AddShortcutInfoToTable(mainMenuTable, Catalog.GetString("Ctrl+PgUp"), Catalog.GetString(
                    "Select the previous Main Module listed at the lower left of the screen and show the most recent Sub-Module Task List."));
            AddShortcutInfoToTable(mainMenuTable, Catalog.GetString("Ctrl+PgDn"), Catalog.GetString(
                    "Select the next Main Module listed at the lower left of the screen and show the most recent Sub-Module Task List."));
            AddShortcutInfoToTable(mainMenuTable, Catalog.GetString("ENTER"), Catalog.GetString(
                    "Launch the screen for the focused Task Item or show the Task List for the focused Sub-Module."));

            string partnerEditContactDetailsTabTable = KeyboardShortcutTableNames.PartnerEditContactDetailsTab.ToString();
            AddTableToDataSet(partnerEditContactDetailsTabTable);

            AddShortcutInfoToTable(partnerEditContactDetailsTabTable, Catalog.GetString("F5"), Catalog.GetString(
                    "New Records: Select Contact Type 'Phone'."));
            AddShortcutInfoToTable(partnerEditContactDetailsTabTable, Catalog.GetString("Shift+F5"), Catalog.GetString(
                    "New Records: Select Contact Type 'Mobile Phone'."));
            AddShortcutInfoToTable(partnerEditContactDetailsTabTable, Catalog.GetString("F6"), Catalog.GetString(
                    "New Records: Select Contact Type 'E-Mail'."));
            AddShortcutInfoToTable(partnerEditContactDetailsTabTable, Catalog.GetString("Shift+F6"), Catalog.GetString(
                    "New Records: Select Contact Type 'Secure E-Mail'."));
            AddShortcutInfoToTable(partnerEditContactDetailsTabTable, Catalog.GetString("F7"), Catalog.GetString(
                    "New Records: Select Contact Type 'Web Site'."));
            AddShortcutInfoToTable(partnerEditContactDetailsTabTable, Catalog.GetString("Shift+F7"), Catalog.GetString(
                    "New Records: Select Contact Type 'Twitter'."));
            AddShortcutInfoToTable(partnerEditContactDetailsTabTable, Catalog.GetString("F8"), Catalog.GetString(
                    "New Records: Select Contact Type 'Skype'."));
            AddShortcutInfoToTable(partnerEditContactDetailsTabTable, Catalog.GetString("Shift+F8"), Catalog.GetString(
                    "New Records: Select Contact Type 'Skype for Business'."));
            AddShortcutInfoToTable(partnerEditContactDetailsTabTable, Catalog.GetString("F9"), Catalog.GetString(
                    "Send E-mail to 'Primary E-Mail' address'."));
            AddShortcutInfoToTable(partnerEditContactDetailsTabTable, Catalog.GetString("F10"), Catalog.GetString(
                    "Send E-mail to 'Office E-Mail' address (when Partner is a PERSON) or to 'Secondary E-mail' address (when Partner is a FAMILY)."));
            AddShortcutInfoToTable(partnerEditContactDetailsTabTable, Catalog.GetString("F11"), Catalog.GetString(
                    "Either send E-mail to E-mail address that is currently displayed in 'Value' or open Hyperlink that is currently displayed in 'Value'."));
            AddShortcutInfoToTable(partnerEditContactDetailsTabTable, Catalog.GetString("F12"), Catalog.GetString(
                    "New Records of E-Mail or Phone Contact Type: Make Value the 'Primary E-mail' or the 'Primary Phone'."));

            this.btnCancel.Text = "&Close";
        }

        private void RunOnceOnActivationManual()
        {
            ArrayList TabsToHide = new ArrayList();

            // Called when the screen has loaded
            tpgFilter.Text = Catalog.GetString("Filter and Find");
            tpgPartnerEditContactDetailsTab.Text = Catalog.GetString("Partner Edit / Contact Details Tab");

            //
            // For some Forms (and Tabs on these Forms) we show a specific Shortcut Tab
            //

            // Partner Edit Form: Contact Details Tab.
            if (FInitiallySelectedTab != String.Empty)
            {
                if (FInitiallySelectedTab == "PartnerEditContactDetailsTab")
                {
                    tabAllShortcuts.SelectedTab = tpgPartnerEditContactDetailsTab;
                }
                else if (FInitiallySelectedTab == "tpgMainMenu")
                {
                    tabAllShortcuts.SelectedTab = tpgMainMenu;
                    TabsToHide.Add("tpgPartnerEditContactDetailsTab");
                }
            }
            else
            {
                // Don't show specific Shortcut Tabs if they aren't requested!
                TabsToHide.Add("tpgPartnerEditContactDetailsTab");
            }

            // Hide specific Shortcut Tabs that aren't requested
            ControlsUtilities.HideTabs(tabAllShortcuts, TabsToHide);
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
            else if (tabAllShortcuts.SelectedTab == tpgDates)
            {
                InitialiseTab(KeyboardShortcutTableNames.Dates, ucoShortcutsDates.HelpGrid, ucoShortcutsDates.DescriptionLabel);
            }
            else if (tabAllShortcuts.SelectedTab == tpgMainMenu)
            {
                InitialiseTab(KeyboardShortcutTableNames.MainMenu, ucoShortcutsMainMenu.HelpGrid, ucoShortcutsMainMenu.DescriptionLabel);
            }
            else if (tabAllShortcuts.SelectedTab == tpgPartnerEditContactDetailsTab)
            {
                InitialiseTab(KeyboardShortcutTableNames.PartnerEditContactDetailsTab,
                    ucoShortcutsPartnerEditContactDetailsTab.HelpGrid,
                    ucoShortcutsPartnerEditContactDetailsTab.DescriptionLabel);
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

                case KeyboardShortcutTableNames.Dates:
                    ADescriptionLabel.Text = Catalog.GetString(
                    "These keyboard shortcuts apply anywhere where a date is required.  Dates can be entered as displayed " +
                    "or can be typed in one of the following ways, for example as digits only or as a number of days from today.");
                    break;

                case KeyboardShortcutTableNames.MainMenu:
                    ADescriptionLabel.Text = Catalog.GetString(
                    "The Main Menu screen lists Modules at the lower left, Sub-Modules at the upper left and " +
                    "Task Items divided into Task Groups on the right side of the screen.  The shortcuts listed here allow you to quickly change " +
                    "Module, Sub-Module or Task and to launch the selected Task.");
                    break;

                case KeyboardShortcutTableNames.PartnerEditContactDetailsTab:
                    ADescriptionLabel.Text = Catalog.GetString(
                    "These keyboard shortcuts are specific to the Partner Edit screens' Contact Details Tab. Use them to " +
                    "speed up entry of new Contact Detail records or to send E-mails conveniently.");
                    break;
            }

            DataTable table = FMainDS.Tables[ATableName.ToString()];

            AGrid.AddTextColumn(FColumnNames[0], table.Columns[0], 130);
            AGrid.AddTextColumn(FColumnNames[1], table.Columns[1], 250);
            AGrid.AutoStretchColumnsToFitWidth = true;
            AGrid.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.None;
            AGrid.Columns[1].AutoSizeMode = SourceGrid.AutoSizeMode.EnableStretch | SourceGrid.AutoSizeMode.EnableAutoSize;

            table.DefaultView.AllowNew = false;
            AGrid.DataSource = new DevAge.ComponentModel.BoundDataView(table.DefaultView);
            AGrid.AutoSizeCells(new SourceGrid.Range(1, 1, AGrid.Rows.Count - 1, 1));

            // We need this line, otherwise the Enter key locks up the screen.  For this grid, on a dialog, we have no special keys.
            AGrid.SpecialKeys = SourceGrid.GridSpecialKeys.None;
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