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
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// This class works alongside TFrmPetraEdit.  It contains the code that handles the saving and loading of window size, position and window state
    /// as well as handling the splitter bar distances.
    /// 
    /// It also contains a method for determining if a control on a screen needs to handle the ESCAPE keypress
    /// 
    /// The class was created so as to keep the TFrmPetraUtils class file smaller and so as to encapsulate this functionality in one file
    /// </summary>
    public class TFrmPetraWindowExtensions
    {
        /// private static variables that manage the storing of window size and position etc
        private static SortedList<string, string> FWindowPositions = new SortedList<string, string>();
        private static bool FWindowPositionsLoaded = false;

        /// private class variable that stores the splitter positions that have already been displayed
        private List<string> FSplittersDisplayed = new List<string>();

        private Form FWinForm;
        private Form FCallerForm;

        /// <summary>
        /// The main constructor for this helper class.  It gets instantiated by TFrmPetraUtils
        /// </summary>
        /// <param name="AWinForm">The variables from TFrmPetraUtils</param>
        /// <param name="ACallerForm">The variables from TFrmPetraUtils</param>
        public TFrmPetraWindowExtensions(Form AWinForm, Form ACallerForm)
        {
            FWinForm = AWinForm;
            FCallerForm = ACallerForm;
        }

        /// <summary>
        /// Called by TFrmPetraUtils during the TFrmPetra_Load event
        /// </summary>
        public void TForm_Load()
        {
            // Are we saving/restoring the window position?  This option is stored in User Defaults.
            if (TUserDefaults.GetBooleanDefault(TUserDefaults.NamedDefaults.USERDEFAULT_SAVE_WINDOW_POS_AND_SIZE, true))
            {
                // Restore the window positions if we know them
                if ((FWinForm.Name == "TFrmMainWindowNew") || (FCallerForm.Name == "TFrmMainWindowNew"))
                {
                    // Either we are loading the main window or we have been opened by the main window
                    if (!FWindowPositionsLoaded)
                    {
                        LoadWindowPositionsFromFile();
                    }

                    RestoreBasicWindowPositionProperties();     // We restore additional properties 'on activation' above
                }
            }
        }

        /// <summary>
        /// Called by TFrmPetraUtils during the TFrmPetra_Closing event
        /// </summary>
        public void TForm_Closing()
        {
            // Deal with the window positions which are stored on the local file system
            string commonSettingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                CommonFormsResourcestrings.StrFolderOrganisationName,
                System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName);
            string settingsFileName = String.Format(CommonFormsResourcestrings.StrScreenPositionsFileName, UserInfo.GUserInfo.UserID);
            string settingsPath = Path.Combine(commonSettingsPath, settingsFileName);

            if (TUserDefaults.GetBooleanDefault(TUserDefaults.NamedDefaults.USERDEFAULT_SAVE_WINDOW_POS_AND_SIZE, true))
            {
                // This is where we 'remember' the window position information
                if (FWinForm.Name == "TFrmMainWindowNew")
                {
                    // we are closing the main window
                    // Get our own window position properties
                    GetWindowPositionProperties();

                    // Now save all the properties for all windows we know about
                    try
                    {
                        if (!Directory.Exists(commonSettingsPath))
                        {
                            Directory.CreateDirectory(commonSettingsPath);
                        }

                        using (StreamWriter sw = new StreamWriter(settingsPath))
                        {
                            foreach (string key in FWindowPositions.Keys)
                            {
                                sw.WriteLine("{0}={1}", key, FWindowPositions[key]);
                            }

                            sw.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        TLogging.Log(String.Format("Exception occurred while saving the window position file '{0}': {1}", settingsPath,
                                ex.Message), TLoggingType.ToLogfile);
                    }
                }
                else if ((FCallerForm.Name == "TFrmMainWindowNew") && !FWinForm.Modal)
                {
                    // we were opened by the main window
                    GetWindowPositionProperties();
                }
            }
            else
            {
                // We are closing a screen and not saving window positions (or no longer saving them)
                FWindowPositions.Clear();
                FWindowPositionsLoaded = false;
            }
        }

        /// <summary>
        /// Helper function to discover if any control in a specified container wants to handle the Escape key.
        /// For example, a combo box that is dropped down would want to handle escape so as to close the list.
        /// </summary>
        /// <param name="AContainerControl">The container control to recursively search</param>
        /// <returns>True if any control in the container wants to handle escape</returns>
        public bool AContainerControlWantsEscape(object AContainerControl)
        {
            // Assume that no controls want to handle ESCape
            bool WantsEscape = false;

            foreach (Control control in ((Control)AContainerControl).Controls)
            {
                // The various ComboBox derivatives want ESCape if the list is Dropped Down
                if (control is ComboBox)
                {
                    WantsEscape = ((ComboBox)control).DroppedDown;
                }
                else if (control is TUC_CountryComboBox)
                {
                    WantsEscape = ((TUC_CountryComboBox)control).DroppedDown;
                }
                else if (control is TUC_CountryLabelledComboBox)
                {
                    WantsEscape = ((TUC_CountryLabelledComboBox)control).DroppedDown;
                }
                else if (control is TCmbAutoPopulated)
                {
                    WantsEscape = ((TCmbAutoPopulated)control).cmbCombobox.DroppedDown;
                }
                else if (control is TSgrdDataGrid)
                {
                    // The grid wants ESCape if a cell is being edited
                    TSgrdDataGrid grid = (TSgrdDataGrid)control;
                    SourceGrid.CellContext focusCellContext = new SourceGrid.CellContext(grid, grid.Selection.ActivePosition);
                    SourceGrid.Cells.ICellVirtual contextCell = focusCellContext.Cell;
                    WantsEscape = ((contextCell != null) && (contextCell.Editor != null) && (contextCell.Editor.IsEditing));
                }
                else if (control.Controls.Count > 0)
                {
                    // Recursive call to ourself for the controls in this container
                    WantsEscape = AContainerControlWantsEscape(control);
                }

                if (WantsEscape)
                {
                    break;
                }
            }

            return WantsEscape;
        }

        /// <summary>
        /// Loads our window positions file for the current user
        /// </summary>
        public void LoadWindowPositionsFromFile()
        {
            FWindowPositions.Clear();

            string commonSettingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                CommonFormsResourcestrings.StrFolderOrganisationName,
                System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName);
            string settingsFileName = String.Format(CommonFormsResourcestrings.StrScreenPositionsFileName, UserInfo.GUserInfo.UserID);
            string settingsPath = Path.Combine(commonSettingsPath, settingsFileName);

            try
            {
                if (File.Exists(settingsPath))
                {
                    using (StreamReader sr = new StreamReader(settingsPath))
                    {
                        while (!sr.EndOfStream)
                        {
                            string oneLine = sr.ReadLine();

                            if (oneLine.Contains("="))
                            {
                                string[] items = oneLine.Split('=');
                                FWindowPositions.Add(items[0], items[1]);
                            }
                        }

                        sr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Exception occurred while loading the window position file '{0}': {1}",
                        settingsPath,
                        ex.Message), TLoggingType.ToLogfile);
            }

            FWindowPositionsLoaded = true;
        }

        /// <summary>
        /// Restores the window size, position and windowstate for the current screen
        /// </summary>
        public void RestoreBasicWindowPositionProperties()
        {
            if (FWindowPositions.ContainsKey(FWinForm.Name))
            {
                // we can set the positions for this window
                string[] items = FWindowPositions[FWinForm.Name].Split(';');

                if (items.Length >= 5)
                {
                    // parse the left, top, width and height
                    int l = int.Parse(items[0]);
                    int t = int.Parse(items[1]);
                    int w = int.Parse(items[2]);
                    int h = int.Parse(items[3]);

                    // parse the sindow state
                    string windowState = items[4];

                    // specify these as Points and Size
                    Point location = new Point(l, t);
                    Size size = new Size(w, h);
                    Point locationBottomRight = new Point(l + w, t + h);

                    // Check the location - just in case it is on a screen that is no longer attached
                    bool bFound = false;

                    foreach (Screen screen in Screen.AllScreens)
                    {
                        // If the whole form is visible on one or more of the available screens we can display it.
                        // Otherwise we let the OS show the form
                        if (screen.Bounds.Contains(locationBottomRight))
                        {
                            FWinForm.Location = location;
                            FWinForm.Size = size;
                            bFound = true;
                            break;
                        }
                    }

                    if (!bFound)
                    {
                        // We did not find the previous position, but may be able to display it at its current size on the base screen
                        if (Screen.AllScreens[0].Bounds.Contains(w - 5, h - 5))
                        {
                            FWinForm.Location = new Point(2, 2);
                            FWinForm.Size = size;
                        }
                    }

                    if (windowState == "Maximized")
                    {
                        FWinForm.WindowState = FormWindowState.Maximized;
                    }
                }
            }
        }

        /// <summary>
        /// Restores the additional position properties (such as split container distances) for the current screen
        /// </summary>
        public void RestoreAdditionalWindowPositionProperties()
        {
            if (FWindowPositions.ContainsKey(FWinForm.Name))
            {
                // we can set the positions for this window
                string[] items = FWindowPositions[FWinForm.Name].Split(';');

                for (int i = 5; i < items.Length; i++)
                {
                    // we must have additional information
                    string[] extraItems = items[i].Split(':');

                    if (extraItems[1] == "SplitterDistance")
                    {
                        // Have we displayed this splitter already?
                        if (!FSplittersDisplayed.Contains(extraItems[0]))
                        {
                            Control splitterControl = FindControlByName(extraItems[0], FWinForm);

                            // Setting the splitter distance only works if the control is displayed
                            // If it is not displayed yet we will have another go later
                            if ((splitterControl != null) && splitterControl.CanFocus)
                            {
                                ((SplitContainer)splitterControl).SplitterDistance = int.Parse(extraItems[2]);
                                FSplittersDisplayed.Add(extraItems[0]);
                            }
                        }
                    }
                }
            }
        }

        #region Helper methods

        /// <summary>
        /// Recursive method to find one or more splitter bars inside a container control and return the control name(s) and distance properties as a string
        /// that can be saved in the screen positions file.  Returns an empty string if there are no splitters.
        /// </summary>
        /// <param name="AContainerControl">The container control - typically a Form</param>
        /// <param name="AResultString">The result string</param>
        /// <param name="AParentFormOrControlName">This is used in the recursive call.  At the topmost level you should pass String.Empty</param>
        private void GetSplitterPropertiesAsString(Control AContainerControl, ref string AResultString, string AParentFormOrControlName)
        {
            if ((AContainerControl is Form) || (AContainerControl is UserControl))
            {
                // When the control is a form or user control we update the name that will be the parent part of the string
                AParentFormOrControlName = AContainerControl.Name;
            }

            foreach (Control control in AContainerControl.Controls)
            {
                if (control is SplitContainer)
                {
                    SplitContainer splitter = (SplitContainer)control;

                    if (AResultString.Contains(String.Format("{0}.{1}", AParentFormOrControlName, splitter.Name)))
                    {
                        // the control is used more than once!!  (This does happen on PartnerFind)
                        // so we need to determine a new suffix that we will store as, e.g., ucoControl(1).
                        int suffix = 1;

                        while (AResultString.Contains(String.Format("{0}({1}).{2}", AParentFormOrControlName, suffix, splitter.Name)))
                        {
                            suffix++;
                        }

                        AResultString += String.Format(";{0}({3}).{1}:SplitterDistance:{2}",
                            AParentFormOrControlName,
                            splitter.Name,
                            splitter.SplitterDistance,
                            suffix);
                    }
                    else
                    {
                        // We have not saved this form.control before
                        AResultString += String.Format(";{0}.{1}:SplitterDistance:{2}",
                            AParentFormOrControlName,
                            splitter.Name,
                            splitter.SplitterDistance);
                    }

                    // Now get the splitter properties inside the two panels of the split container
                    GetSplitterPropertiesAsString(control, ref AResultString, AParentFormOrControlName);
                }
                else if (control.Controls.Count > 0)
                {
                    // Get the splitter properties of this container control
                    GetSplitterPropertiesAsString(control, ref AResultString, AParentFormOrControlName);
                }
            }
        }

        /// <summary>
        /// Top-level method to find a control in a container based on a string representation such as TFrmBlah.sptBlah or ucoBlah.sptBlah
        /// </summary>
        /// <param name="AControlNameToFind">A dot notation string such as ucoBlah.sptBlah</param>
        /// <param name="AContainerControl">The container control - typically a Form</param>
        /// <returns>The found control or null if no control was found</returns>
        private Control FindControlByName(string AControlNameToFind, Control AContainerControl)
        {
            int ControlsSkipped = 0;

            return FindControlByName(AControlNameToFind, AContainerControl, String.Empty, ref ControlsSkipped);
        }

        /// <summary>
        /// This is the recursive method to find a control in a container based on a string representation such as TFrmBlah.sptBlah or ucoBlah.sptBlah.
        /// Do not call this method at the top level.  This method is used internally to recurse through contained controls
        /// </summary>
        /// <param name="AControlNameToFind">A dot notation string such as ucoBlah.sptBlah</param>
        /// <param name="AContainerControl">The container control - typically a Form</param>
        /// <param name="AParentFormOrControlName">This is used in the recursive call.  At the topmost level you should pass String.Empty</param>
        /// <param name="AControlsSkippedCount">This is used in the recursive call.  It keeps track of the controls skipped when more than one control has the same name.</param>
        /// <returns>The found control or null if no control was found</returns>
        private Control FindControlByName(string AControlNameToFind,
            Control AContainerControl,
            string AParentFormOrControlName,
            ref int AControlsSkippedCount)
        {
            if ((AContainerControl is Form) || (AContainerControl is UserControl))
            {
                AParentFormOrControlName = AContainerControl.Name;
            }

            // Parse the control name to find - bearing in mind that worst case is parentControl(1).controlName
            Control foundControl = null;
            string parentFormOrControlName = String.Empty;
            string controlName = AControlNameToFind;
            int controlsToSkipWithSameName = 0;

            if (AControlNameToFind.Contains("."))
            {
                string[] items = AControlNameToFind.Split('.');
                parentFormOrControlName = items[0];
                controlName = items[1];

                if (parentFormOrControlName.EndsWith(")"))
                {
                    // the parent control is a duplicate
                    int pos = parentFormOrControlName.LastIndexOf('(');
                    controlsToSkipWithSameName = Convert.ToInt32(parentFormOrControlName.Substring(pos + 1, parentFormOrControlName.Length - pos - 2));
                    parentFormOrControlName = parentFormOrControlName.Substring(0, pos);
                }
            }

            // Now we know what to look for
            foreach (Control control in AContainerControl.Controls)
            {
                if ((control.Name == controlName) && (AParentFormOrControlName == parentFormOrControlName))
                {
                    if (controlsToSkipWithSameName == AControlsSkippedCount)
                    {
                        foundControl = control;
                    }
                    else
                    {
                        // carry on a find the next occurrence
                        AControlsSkippedCount++;
                    }
                }
                else if (control.Controls.Count > 0)
                {
                    // Recurse into this control's controls
                    foundControl = FindControlByName(AControlNameToFind, control, AParentFormOrControlName, ref AControlsSkippedCount);
                }

                if (foundControl != null)
                {
                    break;
                }
            }

            return foundControl;
        }

        /// <summary>
        /// Gets the window size and position (and splitter positions) for the current screen in our static sorted list variable
        /// </summary>
        private void GetWindowPositionProperties()
        {
            // First we remember what the splitter string was before, because we may need parts of it again
            string prevWinPosString = String.Empty;

            if (FWindowPositions.ContainsKey(FWinForm.Name))
            {
                prevWinPosString = FWindowPositions[FWinForm.Name];
            }

            // Now get the current splitter properties for all splitters on this form
            string splitterProperties = String.Empty;
            GetSplitterPropertiesAsString(FWinForm, ref splitterProperties, String.Empty);

            // Now we check to see if we are saving any positions on controls that were not actually displayed this time
            if (prevWinPosString != String.Empty)
            {
                string[] prevItems = prevWinPosString.Split(';');

                for (int i = 5; i < prevItems.Length; i++)
                {
                    string[] prevExtraItems = prevItems[i].Split(':');

                    if (!FSplittersDisplayed.Contains(prevExtraItems[0]))
                    {
                        // This is a splitter we know about already and it has not been displayed while the screen was open this time
                        // So we need to substitute the value we know for the one in splitterProperties, which will be some kind of default YAML value
                        string[] newItems = splitterProperties.Split(';');

                        for (int k = 0; k < newItems.Length; k++)
                        {
                            string[] newExtraItems = newItems[k].Split(':');

                            if (newExtraItems[0] == prevExtraItems[0])
                            {
                                splitterProperties = splitterProperties.Replace(newItems[k], prevItems[i]);
                                break;
                            }
                        }
                    }
                }
            }

            // Now start on the window position and size
            string windowProperties;

            if (FWinForm.WindowState == FormWindowState.Normal)
            {
                windowProperties = String.Format("{0};{1};{2};{3};{4}{5}",
                    FWinForm.Left,
                    FWinForm.Top,
                    FWinForm.Width,
                    FWinForm.Height,
                    "Normal",
                    splitterProperties);
            }
            else
            {
                windowProperties = String.Format("{0};{1};{2};{3};{4}{5}",
                    FWinForm.RestoreBounds.Left,
                    FWinForm.RestoreBounds.Top,
                    FWinForm.RestoreBounds.Width,
                    FWinForm.RestoreBounds.Height,
                    FWinForm.WindowState.ToString(),
                    splitterProperties);
            }

            if (FWindowPositions.ContainsKey(FWinForm.Name))
            {
                FWindowPositions[FWinForm.Name] = windowProperties;
            }
            else
            {
                FWindowPositions.Add(FWinForm.Name, windowProperties);
            }
        }

        #endregion
    }
}
