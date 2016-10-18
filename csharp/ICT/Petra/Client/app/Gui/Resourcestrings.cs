//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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

using Ict.Common;


namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// Contains resourcetexts that are used application-wide.
    /// </summary>
    public class ApplWideResourcestrings
    {
        #region General
        /// <summary>todoComment</summary>
        public static readonly string StrUndefined = Catalog.GetString("Undefined");

        /// <summary>todoComment</summary>
        public static readonly string StrBtnCreatedUpdatedStatusBarText =
            Catalog.GetString("Click this button to show information about who created / modified this record.");

        #endregion

        #region Partner
        /// <summary>todoComment</summary>
        public static readonly string StrPartner = Catalog.GetString("Partner");

        /// <summary>todoComment</summary>
        public static readonly string StrPartnerClass = Catalog.GetString("Partner Class");

        /// <summary>todoComment</summary>
        public static readonly string StrPartnerKey = Catalog.GetString("PartnerKey");

        #endregion

        #region Keyboard Shortcuts Help

        #region Category Descriptions
        /// <summary>todoComment</summary>
        public static readonly string StrKeysHelpCategoryGeneral = Catalog.GetString(
            "These familiar keyboard shortcuts are used throughout many Windows applications.  OpenPetra uses them to perform the functionality that you would typically expect.  In addition to the shortcuts listed here, many screens have ALT+key combinations associated with buttons.  Often, for example, ALT+N will create a new record.  Simply press the ALT key to see the available options for the current screen.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeysHelpCategoryList = Catalog.GetString(
            "These keyboard shortcuts only apply when keyboard input is currently directed to a List of records.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeysHelpCategoryNavigation = Catalog.GetString(
            "These keyboard shortcuts apply to screens that have a List of records. They can be used wherever keyboard input is currently directed.  They direct the next keyboard input to the first data entry field below the List, or the List itself if there is no data entry field.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeysHelpCategoryFilterFind = Catalog.GetString(
            "These keyboard shortcuts apply to screens that have a List of records and a Filter/Find panel.  Like the Navigation shortcuts, these apply wherever keyboard input is currently directed.  Depending on the context you may be able to follow F3 or SHIFT+F3 by pressing ENTER to open a new screen with more details.");

        #endregion

        #region General

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlC = Catalog.GetString("Ctrl+C");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlCHelp = Catalog.GetString(
            "Copies the highlighted text in a text entry field to the clipboard.  Cannot be used to copy whole records.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlX = Catalog.GetString("Ctrl+X");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlXHelp = Catalog.GetString(
            "Deletes the highlighted text in a text entry field and places it on the clipboard so that it can be pasted elsewhere.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlV = Catalog.GetString("Ctrl+V");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlVHelp = Catalog.GetString(
            "Pastes the clipboard text into the current text entry field at the cursor position.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlTab = Catalog.GetString("Ctrl+Tab");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlTabHelp = Catalog.GetString(
            "On screens that have multiple tabbed 'pages' this key combination switches to the next tab.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutShiftCtrlTab = Catalog.GetString("Shift+Ctrl+Tab");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutShiftCtrlTabHelp = Catalog.GetString(
            "On screens that have multiple tabbed 'pages' this key combination switches to the previous tab.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlS = Catalog.GetString("Ctrl+S");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlSHelp = Catalog.GetString("Saves the pending screen changes.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlP = Catalog.GetString("Ctrl+P");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlPHelp = Catalog.GetString("Prints the data displayed in the List (where available)");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutEscape = Catalog.GetString("Escape");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutEscapeHelp = Catalog.GetString(
            "The Esc key closes a screen, if this behaviour has been set in 'User Preferences'.");

        #endregion

        #region List
        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutHome = Catalog.GetString("Home");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutHomeHelp = Catalog.GetString("Selects the first row in the List.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutPgUp = Catalog.GetString("PgUp");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutPgUpHelp = Catalog.GetString("Selects the row that is one 'page' up in the List.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutUp = Catalog.GetString("Up");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutUpHelp = Catalog.GetString("Selects the previous row in the List.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutDown = Catalog.GetString("Down");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutDownHelp = Catalog.GetString("Selects the next row in the List.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutPgDn = Catalog.GetString("PgDn");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutPgDnHelp = Catalog.GetString("Selects the row that is one 'page' down in the List.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutEnd = Catalog.GetString("End");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutEndHelp = Catalog.GetString("Selects the last row in the List.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutIns = Catalog.GetString("Ins");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutInsHelp = Catalog.GetString("Inserts a new row into the List.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutDel = Catalog.GetString("Del");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutDelHelp = Catalog.GetString("Deletes the current row from the List.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutEnter = Catalog.GetString("Enter");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutEnterHelp = Catalog.GetString(
            "This may act like a mouse double-click or may start editing an entry on the highlighted row - depending on context.");

        #endregion

        #region Navigation
        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlHome = Catalog.GetString("Ctrl+Home");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlHomeHelp = Catalog.GetString(
            "Selects the first row in the List of records and directs keyboard input to the first data entry field.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlUp = Catalog.GetString("Ctrl+Up");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlUpHelp = Catalog.GetString(
            "Selects the previous row in the List of records and directs keyboard input to the first data entry field.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlDown = Catalog.GetString("Ctrl+Down");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlDownHelp = Catalog.GetString(
            "Selects the next row in the List of records and directs keyboard input to the first data entry field.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlEnd = Catalog.GetString("Ctrl+End");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlEndHelp = Catalog.GetString(
            "Selects the last row in the List of records and directs keyboard input to the first data entry field.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlL = Catalog.GetString("Ctrl+L");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlLHelp = Catalog.GetString("Directs keyboard input to the List of records at the current row.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlE = Catalog.GetString("Ctrl+E");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlEHelp = Catalog.GetString(
            "Directs keyboard input to the first data entry field of the current record.");

        #endregion

        #region Filter/Find
        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlR = Catalog.GetString("Ctrl+R");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlRHelp = Catalog.GetString(
            "Opens the Filter Panel and directs the keyboard input to the first criteria field.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlF = Catalog.GetString("Ctrl+F");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutCtrlFHelp = Catalog.GetString(
            "Opens the Find Panel and directs the keyboard input to the first criteria field.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutF3 = Catalog.GetString("F3");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutF3Help = Catalog.GetString(
            "Opens the Find Panel and finds the next matching item in the List.  Keyboard entry is directed to the List.");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutShiftF3 = Catalog.GetString("Shift+F3");

        /// <summary>todoComment</summary>
        public static readonly string StrKeyShortcutShiftF3Help = Catalog.GetString(
            "Opens the Find Panel and finds the previous matching item in the List.  Keyboard entry is directed to the List.");

        #endregion

        #endregion
    }
}