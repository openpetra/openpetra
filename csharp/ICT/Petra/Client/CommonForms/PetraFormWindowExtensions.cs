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
        private static SortedList <string, string>FWindowPositions = new SortedList <string, string>();
        private static bool FWindowPositionsLoaded = false;

        /// These forms will always be saved even though they are not launched by the main screen
        private static List <string>FAlwaysSaveFormsList = null;

        /// dictionary to hold the most recent window position for floating (multi-use) windows
        private static Dictionary <string, Point>FDicFloatingWindowLocations = new Dictionary <string, Point>();

        /// private class variable that stores the splitter positions that have already been displayed
        private List <string>FSplittersDisplayed = new List <string>();

        private Form FWinForm;
        private Form FCallerForm;
        private bool FIsWindowInitiallyMaximised = false;
        private bool FIsShownEventDefined = false;

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
                if (FAlwaysSaveFormsList == null)
                {
                    FAlwaysSaveFormsList = new List <string>();
                    FAlwaysSaveFormsList.AddRange(new string[]
                        {
                            "TFrmMainWindowNew",
                            "TFrmPartnerEdit",
                            "TFrmGiftBatch",
                            "TFrmGLBatch",
                            "TPartnerFindScreen",
                            "TFrmExtFilePreviewDialog"
                        }
                        );
                }

                // Restore the window positions if we know them
                // (Note: Nant tests do not have a caller so we need to allow for this possibility)
                if (FAlwaysSaveFormsList.Contains(FWinForm.Name)
                    || ((FCallerForm != null) && (FCallerForm.Name == "TFrmMainWindowNew")))
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
            string localAppDataPath = Path.Combine(
                TAppSettingsManager.GetLocalAppDataPath(),
                CommonFormsResourcestrings.StrFolderOrganisationName,
                System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName);
            string settingsFileName = String.Format(CommonFormsResourcestrings.StrScreenPositionsFileName, UserInfo.GUserInfo.UserID);
            string settingsPath = Path.Combine(localAppDataPath, settingsFileName);

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
                        if (!Directory.Exists(localAppDataPath))
                        {
                            Directory.CreateDirectory(localAppDataPath);
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
                else if (FAlwaysSaveFormsList.Contains(FWinForm.Name))
                {
                    // We always save the settings for these forms - they can be launched from the main window or from Partner/Find or several other ways
                    GetWindowPositionProperties();
                }
                else if ((FCallerForm != null) && (FCallerForm.Name == "TFrmMainWindowNew") && !FWinForm.Modal)
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

            string localAppDataPath = Path.Combine(
                TAppSettingsManager.GetLocalAppDataPath(),
                CommonFormsResourcestrings.StrFolderOrganisationName,
                System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName);
            string settingsFileName = String.Format(CommonFormsResourcestrings.StrScreenPositionsFileName, UserInfo.GUserInfo.UserID);
            string settingsPath = Path.Combine(localAppDataPath, settingsFileName);

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

                    // parse the window state
                    string windowState = items[4];

                    // Screens that are multi-use need to be left in the position where Windows has defaulted them (if possible)
                    // This prevents us from always positioning a new screen on top of a previous one
                    bool ignoreLocation = (FWinForm.Name == "TFrmPartnerEdit");

                    // specify these as Points and Size
                    if (ignoreLocation)
                    {
                        l = FWinForm.Location.X;
                        t = FWinForm.Location.Y;
                    }

                    Size size = new Size(w, h);
                    Point location = new Point(l, t);
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
                        if (ignoreLocation)
                        {
                            // We could not fit a multi-use screen on the display where the OS has located the top left,
                            //   but we can probably move it.  Try not to overlay any previous screen
                            foreach (Screen screen in Screen.AllScreens)
                            {
                                if (screen.Bounds.Contains(location))
                                {
                                    // This is the screen that the OS is using for the display
                                    if (FDicFloatingWindowLocations.ContainsKey(FWinForm.Name))
                                    {
                                        Point tryLocation = FDicFloatingWindowLocations[FWinForm.Name];
                                        tryLocation.Offset(screen.Bounds.Width / 50, screen.Bounds.Height / 50);
                                        locationBottomRight = new Point(tryLocation.X + w, tryLocation.Y + h);

                                        if (screen.Bounds.Contains(locationBottomRight))
                                        {
                                            FWinForm.Location = tryLocation;
                                            FWinForm.Size = size;
                                            bFound = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (!bFound)
                        {
                            // We did not find the previous position of a single-use screen,
                            // but we may be able to display it at its current size on the base screen
                            if (Screen.AllScreens[0].Bounds.Contains(w - 5, h - 5))
                            {
                                FWinForm.Location = new Point(2, 2);
                                FWinForm.Size = size;
                            }
                        }

                        if (ignoreLocation)
                        {
                            // Finally, for floating windows we save the location that we use
                            if (FDicFloatingWindowLocations.ContainsKey(FWinForm.Name))
                            {
                                FDicFloatingWindowLocations[FWinForm.Name] = FWinForm.Location;
                            }
                            else
                            {
                                FDicFloatingWindowLocations.Add(FWinForm.Name, FWinForm.Location);
                            }
                        }
                    }

                    if ((windowState == "Maximized") && !ignoreLocation)
                    {
                        FIsWindowInitiallyMaximised = true;
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

                if ((items.Length > 5) && FIsWindowInitiallyMaximised && !FIsShownEventDefined)
                {
                    // The screen does have splitters and is going to be initially maximised (but is not visible yet).
                    // We have to come back here and run this method again to do the splitters in the Shown event
                    FIsShownEventDefined = true;
                    FWinForm.Shown += FWinForm_Shown;
                    return;
                }

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
                                SplitContainer splitter = (SplitContainer)splitterControl;
                                int distance = int.Parse(extraItems[2]);

                                // Just in case the numbers are screwed up we enforce the max/min splitter position
                                if (splitter.Orientation == Orientation.Horizontal)
                                {
                                    distance = Math.Max(distance, splitter.Panel1MinSize);
                                    distance = Math.Min(distance, splitter.Height - splitter.Panel2MinSize);
                                }
                                else
                                {
                                    distance = Math.Max(distance, splitter.Panel1MinSize);
                                    distance = Math.Min(distance, splitter.Width - splitter.Panel2MinSize);
                                }

                                // the value of distance can end up as 0 if there are two splitters and one is 'underneath' the other
                                // this happens on Motivation Details setup screen for example.  The second splitter is effectively hidden by the first.
                                if (distance > 0)
                                {
                                    splitter.SplitterDistance = distance;
                                }

                                FSplittersDisplayed.Add(extraItems[0]);
                            }
                        }
                    }
                }
            }
        }

        private void FWinForm_Shown(object sender, EventArgs e)
        {
            RestoreAdditionalWindowPositionProperties();
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

            bool treatAsMaximized = false;

            if (FWinForm.WindowState == FormWindowState.Minimized)
            {
                // If the window has splitters and is minimized we may have a problem if it was minimized from maximized
                treatAsMaximized = IsWindowMinimizedFromMaximized(splitterProperties);
            }

            // We handle the splitter properties one of two ways...
            // depending on whether the form uses dynamically loaded controls or not
            if (FWinForm.Name == "TFrmPartnerEdit")
            {
                // Dynamically loaded controls...
                // This means that all the splitter distances will be correct but we will only have the splitters that have been opened
                if (prevWinPosString != String.Empty)
                {
                    string[] prevItems = prevWinPosString.Split(';');

                    for (int i = 5; i < prevItems.Length; i++)
                    {
                        string[] prevExtraItems = prevItems[i].Split(':');

                        if (!splitterProperties.Contains(";" + prevExtraItems[0]))
                        {
                            splitterProperties += (";" + prevItems[i]);
                        }
                    }
                }
            }
            else
            {
                // Non-dynamically loaded controls...
                // This means we will have all the splitters - but the distances on those we did not actually load will be incorrect.
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
            else if (treatAsMaximized)
            {
                windowProperties = String.Format("{0};{1};{2};{3};{4}{5}",
                    FWinForm.RestoreBounds.Left,
                    FWinForm.RestoreBounds.Top,
                    FWinForm.RestoreBounds.Width,
                    FWinForm.RestoreBounds.Height,
                    "Maximized",
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

        /// <summary>
        /// If a window is minimized we need to know if it was minimized from maximized or minimized from Normal
        /// </summary>
        /// <param name="ASplitterProperties">The current splitter properties string</param>
        /// <returns>True if it looks  like the window was minimized from maximized.</returns>
        private bool IsWindowMinimizedFromMaximized(string ASplitterProperties)
        {
            if (ASplitterProperties.Length > 0)
            {
                string[] items = ASplitterProperties.Split(';');

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].Length == 0)
                    {
                        continue;
                    }

                    string[] extraItems = items[i].Split(':');
                    Control splitterControl = FindControlByName(extraItems[0], FWinForm);

                    if (splitterControl != null)
                    {
                        SplitContainer splitter = (SplitContainer)splitterControl;
                        int distance = int.Parse(extraItems[2]);

                        // Our best guess is that if the splitter position is significantly more than the normal window position
                        //  then we say it must be minimized from maximized.  But this is open to debate!
                        if (splitter.Orientation == Orientation.Horizontal)
                        {
                            if (distance > FWinForm.RestoreBounds.Height - splitter.Panel2MinSize - 60)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (distance > FWinForm.RestoreBounds.Width - splitter.Panel2MinSize - 60)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        #endregion
    }
}