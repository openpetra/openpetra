//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 alanP
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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Text.RegularExpressions;
using Ict.Common;
using Ict.Common.Controls;


namespace Ict.Petra.Client.CommonControls
{

    #region Helper classes used by Filter/Find panels on individual screens (see ucoFilterAndFind.cs)

    #region TIndividualFilterFindPanel

    /// <summary>
    /// A simple class that maintains the information about the controls on a panel and the database column/name to which it refers
    /// </summary>
    public class TIndividualFilterFindPanel
    {
        private Label FPanelLabel;
        private Control FPanelControl;
        private string FDBColumnName;
        private string FDBColumnDataType;
        private string FFilterComparison;
        private bool FHasClearButton;

        private string FInstanceName;

        /// <summary>
        /// Returns a reference to the label on the filter/find panel
        /// </summary>
        public Label PanelLabel
        {
            get
            {
                return FPanelLabel;
            }
        }

        /// <summary>
        /// Returns a reference to the user input control on the filter/find panel
        /// </summary>
        public Control PanelControl
        {
            get
            {
                return FPanelControl;
            }
        }

        /// <summary>
        /// Returns the database column name associated with the user input control on the filter/find panel
        /// </summary>
        public string DBColumnName
        {
            get
            {
                return FDBColumnName;
            }
        }

        /// <summary>
        /// Returns the database column type associated with the user input control on the filter/find panel
        /// </summary>
        public string DBColumnDataType
        {
            get
            {
                return FDBColumnDataType;
            }
        }

        /// <summary>
        /// Returns the filter comparison to be used for the filter as a format string
        /// </summary>
        public string FilterComparison
        {
            get
            {
                return FFilterComparison;
            }
        }

        /// <summary>
        /// Returns whether or not the filter/find Panel has a Clear Button
        /// </summary>
        public bool HasClearButton
        {
            get
            {
                return FHasClearButton;
            }
        }

        /// <summary>
        /// Main constructor for an individual Filter/Find panel in which the label and control are passed in as parameters
        /// </summary>
        /// <param name="APanelLabel">The Label control</param>
        /// <param name="APanelControl">The user input control</param>
        /// <param name="ADBColumnName">The database column name.  If this parameter is null the GetFilterComparison method will return null.
        /// You will need a manual method to handle filtering response.</param>
        /// <param name="ADBColumnDataType">The database column type: e.g numeric, varchar, integer, boolean etc.</param>
        /// <param name="ATag">Sets additional properties such as whether the Panel control has a clear button (True by default)</param>
        public TIndividualFilterFindPanel(Label APanelLabel,
            Control APanelControl,
            string ADBColumnName,
            string ADBColumnDataType,
            string ATag)
        {
            FPanelLabel = APanelLabel;
            FPanelControl = APanelControl;
            FDBColumnName = ADBColumnName;
            FDBColumnDataType = ADBColumnDataType;
            FHasClearButton = true;             // By default filter controls have a clear button

            // Set the additional Tag properties on the panel control
            SetPanelControlTag(ATag);
            // Set the instance name (if any)
            SetInstanceFromTag(ATag);
            // Set the filter comparison from the column name and data type
            FFilterComparison = GetFilterComparison(ADBColumnName, ADBColumnDataType, ATag);

            if (FPanelControl is CheckBox)
            {
                ((CheckBox)FPanelControl).CheckState = CheckState.Indeterminate;
                ((CheckBox)FPanelControl).ThreeState = true;

                if ((FPanelLabel != null) && (FPanelLabel.Text != String.Empty) && (FPanelControl.Text == String.Empty))
                {
                    FPanelControl.Text = FPanelLabel.Text;
                    FPanelLabel = null;
                }
            }

            if (FPanelControl is TCmbAutoComplete && FHasClearButton)
            {
                ((TCmbAutoComplete)FPanelControl).IgnoreNewValues = true;
            }
        }

        /// <summary>
        /// Returns a filter comparison format string by testing the last characters of the database column name.
        /// If the column name was set to null in the class contructor this method will return null.
        /// </summary>
        /// <param name="ADBColumnName">The database column name</param>
        /// <param name="ADBColumnDataType">The database column type e.g varchar, integer, numeric, boolean</param>
        /// <param name="ATag">A tag string that may contain a comparison operator, e.g. gte, lt, eq.</param>
        /// <returns>A format string such as LIKE '%{0}%'</returns>
        private static string GetFilterComparison(string ADBColumnName, string ADBColumnDataType, string ATag)
        {
            if ((ADBColumnName == null) || (ADBColumnDataType == null))
            {
                return null;
            }

            string op = String.Empty;

            if (ATag.Contains(CommonTagString.COMPARISON_EQUALS))
            {
                int posStart = ATag.IndexOf(CommonTagString.COMPARISON_EQUALS) + CommonTagString.COMPARISON_EQUALS.Length;
                int posEnd = ATag.IndexOf(';', posStart);
                if (posEnd > 0)
                {
                    op = ATag.Substring(posStart, posEnd - posStart);
                    
                    switch (op)
                    {
                        case "gt":
                            op = ">";
                            break;
                        case "gte":
                            op = ">=";
                            break;
                        case "lt":
                            op = "<";
                            break;
                        case "lte":
                            op = "<=";
                            break;
                        case "eq":
                            op = "=";
                            break;
                        default:
                            break;
                    }
                }
            }

            if (ADBColumnDataType == "bit")
            {
                // Boolean
                return "={0}";
            }
            else if ((ADBColumnDataType == "integer") || (ADBColumnDataType == "number"))
            {
                // numbers
                if (op == String.Empty)
                {
                    return "_text LIKE '%{0}%'";
                }
                else
                {
                    return op + "{0}";
                }
            }
            else if (ADBColumnDataType == "date")
            {
                // date
                if (op == String.Empty)
                {
                    return "=#{0}#";
                }
                else
                {
                    return op + "#{0}#";
                }
            }
            else if ((ADBColumnDataType == "varchar") || (ADBColumnDataType == "String"))
            {
                // text
                return " LIKE '%{0}%'";
            }
            else
            {
                throw new Exception("Unsupported column type: " + ADBColumnDataType);
            }
        }

        /// <summary>
        /// Sets the additional Tag properties of the panel control as set by the constructor
        /// </summary>
        /// <param name="ATag">The Tag value passed into the contructor</param>
        private void SetPanelControlTag(string ATag)
        {
            // Clear Button??
            if (ATag.Contains(CommonTagString.ARGUMENTPANELTAG_NO_AUTOM_ARGUMENTCLEARBUTTON))
            {
                FHasClearButton = false;
                FPanelControl.Tag += ";" + CommonTagString.ARGUMENTPANELTAG_NO_AUTOM_ARGUMENTCLEARBUTTON;
            }

            if (FHasClearButton)
            {
                // Clear Value??
                string strClearValue = CommonTagString.ARGUMENTCONTROLTAG_CLEARVALUE;

                if (ATag.Contains(strClearValue))
                {
                    // A Clear Value has been specified
                    int p = ATag.IndexOf(strClearValue);
                    p = ATag.IndexOf("=", p);
                    int p2 = ATag.IndexOf(';', p);

                    string value = ATag.Substring(p, p2 - p);
                    FPanelControl.Tag += String.Format(";{0}{1}", CommonTagString.ARGUMENTCONTROLTAG_CLEARVALUE, value);
                }
                else
                {
                    if (FPanelControl is TCmbAutoComplete)
                    {
                        // No clear value specified, but if we are using a TCmbAutoComplete with a clear button the value will be -1 by default
                        FPanelControl.Tag += String.Format(";{0}=-1", CommonTagString.ARGUMENTCONTROLTAG_CLEARVALUE);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the internal instance name variable if it is part of the Tag
        /// </summary>
        /// <param name="ATag">The tag string</param>
        private void SetInstanceFromTag(string ATag)
        {
            FInstanceName = String.Empty;

            if (ATag.Contains(CommonTagString.INSTANCE_EQUALS))
            {
                int posStart = ATag.IndexOf(CommonTagString.INSTANCE_EQUALS) + CommonTagString.INSTANCE_EQUALS.Length;
                int posEnd = ATag.IndexOf(';', posStart);

                if (posEnd > 0)
                {
                    FInstanceName = ATag.Substring(posStart, posEnd - posStart);

                    // Update the label and control names accordingly
                    if (FPanelLabel != null)
                    {
                        if (FPanelLabel.Name.EndsWith(TFilterPanelControls.FILTER_NAME_SUFFIX))
                        {
                            FPanelLabel.Name = FPanelLabel.Name.Insert(FPanelLabel.Name.LastIndexOf(TFilterPanelControls.FILTER_NAME_SUFFIX), FInstanceName);
                        }
                        else if (FPanelLabel.Name.EndsWith(TFindPanelControls.FIND_NAME_SUFFIX))
                        {
                            FPanelLabel.Name = FPanelLabel.Name.Insert(FPanelLabel.Name.LastIndexOf(TFindPanelControls.FIND_NAME_SUFFIX), FInstanceName);
                        }
                    }

                    if (FPanelControl != null)
                    {
                        if (FPanelControl.Name.EndsWith(TFilterPanelControls.FILTER_NAME_SUFFIX))
                        {
                            FPanelControl.Name = FPanelControl.Name.Insert(FPanelControl.Name.LastIndexOf(TFilterPanelControls.FILTER_NAME_SUFFIX), FInstanceName);
                        }
                        else if (FPanelControl.Name.EndsWith(TFindPanelControls.FIND_NAME_SUFFIX))
                        {
                            FPanelControl.Name = FPanelControl.Name.Insert(FPanelControl.Name.LastIndexOf(TFindPanelControls.FIND_NAME_SUFFIX), FInstanceName);
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region TFilterPanelControls

    /// <summary>
    /// A class that contains information about all the controls on the filter panel
    /// </summary>
    public class TFilterPanelControls
    {
        /// <summary>
        /// A base filter string that is set externally by the host screen to be applied in addition to the filter specified through the Filter/Find GUI
        /// </summary>
        private string FBaseFilter = String.Empty;

        /// <summary>
        /// True if the base filter shows all records accessible from the GUI.  False if some records are potentially hidden.
        /// </summary>
        private bool FBaseFilterShowsAllRecords = true;

        /// <summary>
        /// A list of all the individual standard filter panels
        /// </summary>
        public List<TIndividualFilterFindPanel> FStandardFilterPanels = new List<TIndividualFilterFindPanel>();

        /// <summary>
        /// A list of all the individual Extra filter panels
        /// </summary>
        public List<TIndividualFilterFindPanel> FExtraFilterPanels = new List<TIndividualFilterFindPanel>();

        /// <summary>
        /// The suffix to be appended to the control name when it is on the filter panel
        /// </summary>
        public const string FILTER_NAME_SUFFIX = "_filter";

        /// <summary>
        /// Set this value to a filter string that you want to include as part of the overall filter string generated by the users selection of filter controls
        /// while the Filter panel is shown
        /// </summary>
        public string BaseFilter
        {
            get
            {
                return FBaseFilter;
            }
        }

        /// <summary>
        /// Returns true if the base filter returns all the available records for the current screen.  Returns false if some records are potentially hidden.
        /// </summary>
        public bool BaseFilterShowsAllRecords
        {
            get
            {
                return FBaseFilterShowsAllRecords;
            }
        }

        /// <summary>
        /// Returns a list of panels and their controls on the standard filter panel ready to be passed to the Filter/Find user control
        /// </summary>
        /// <returns>A list of panels and their controls ready to be passed to the Filter/Find user control</returns>
        public List<Panel> GetFilterPanels()
        {
            List<Panel> standardPanels = null;

            if (FStandardFilterPanels.Count > 0)
            {
                standardPanels = new List<Panel>();

                for (int i = 0; i < FStandardFilterPanels.Count; i++)
                {
                    standardPanels.Add(TUcoFilterAndFind.ArgumentPanelHelper.CreateArgumentPanel(
                            FStandardFilterPanels[i].PanelLabel, FStandardFilterPanels[i].PanelControl, FStandardFilterPanels[i].HasClearButton));
                }
            }

            return standardPanels;
        }

        /// <summary>
        /// Returns a list of panels and their controls on the Extra filter panel ready to be passed to the Filter/Find user control
        /// </summary>
        /// <returns>A list of panels and their controls ready to be passed to the Filter/Find user control</returns>
        public List<Panel> GetExtraFilterPanels()
        {
            List<Panel> extraPanels = null;

            if (FExtraFilterPanels.Count > 0)
            {
                extraPanels = new List<Panel>();

                for (int i = 0; i < FExtraFilterPanels.Count; i++)
                {
                    extraPanels.Add(TUcoFilterAndFind.ArgumentPanelHelper.CreateArgumentPanel(
                            FExtraFilterPanels[i].PanelLabel, FExtraFilterPanels[i].PanelControl));
                }
            }

            return extraPanels;
        }

        /// <summary>
        /// Gets the filter string from the specified set of panels
        /// </summary>
        /// <param name="AFilterPanelControls">The panel set (standard or extra)</param>
        /// <returns></returns>
        private string GetCurrentFilter(List<TIndividualFilterFindPanel> AFilterPanelControls)
        {
            string filter = String.Empty;

            for (int i = 0; i < AFilterPanelControls.Count; i++)
            {
                if (AFilterPanelControls[i].FilterComparison == null)
                {
                    continue;
                }

                string value;

                if (AFilterPanelControls[i].PanelControl is CheckBox)
                {
                    CheckBox ctrlAsCheckBox = (CheckBox)AFilterPanelControls[i].PanelControl;

                    switch (ctrlAsCheckBox.CheckState)
                    {
                        case CheckState.Checked:
                            value = "1";
                            break;

                        case CheckState.Unchecked:
                            value = "0";
                            break;

                        default:
                            value = String.Empty;
                            break;
                    }
                }
                else if (AFilterPanelControls[i].PanelControl is TtxtPetraDate)
                {
                    TtxtPetraDate ctrlAsDtp = (TtxtPetraDate)AFilterPanelControls[i].PanelControl;
                    if (ctrlAsDtp.Date.HasValue)
                    {
                        value = ctrlAsDtp.Date.Value.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        value = String.Empty;
                    }
                }
                else
                {
                    value = AFilterPanelControls[i].PanelControl.Text;
                }

                if (value != String.Empty)
                {
                    if (filter.Length > 0)
                    {
                        filter += " AND ";
                    }

                    filter += AFilterPanelControls[i].DBColumnName;
                    filter += String.Format(AFilterPanelControls[i].FilterComparison, value);
                }
            }

            return filter;
        }

        /// <summary>
        /// Gets the current filter from all the inputs on the filter panel
        /// </summary>
        /// <returns>The current filter from all the inputs on the filter panel</returns>
        public string GetCurrentFilter(bool AIsCollapsed,
            TUcoFilterAndFind.FilterContext AKeepFilterOnButtonDepressedContext,
            TUcoFilterAndFind.FilterContext AFilterAlwaysOnLabelContext)
        {
            // Start with the base filter
            string filter = FBaseFilter;

            bool bIgnoreStandardFilter = (AIsCollapsed
                                          && (AKeepFilterOnButtonDepressedContext == TUcoFilterAndFind.FilterContext.None
                                              || AKeepFilterOnButtonDepressedContext == TUcoFilterAndFind.FilterContext.ExtraFilterOnly)
                                          && (AFilterAlwaysOnLabelContext == TUcoFilterAndFind.FilterContext.None
                                              || AFilterAlwaysOnLabelContext == TUcoFilterAndFind.FilterContext.ExtraFilterOnly));
            string stdFilter = (bIgnoreStandardFilter) ? String.Empty : GetCurrentFilter(FStandardFilterPanels);

            bool bIgnoreExtraFilter = (AIsCollapsed
                                       && (AKeepFilterOnButtonDepressedContext == TUcoFilterAndFind.FilterContext.None
                                           || AKeepFilterOnButtonDepressedContext == TUcoFilterAndFind.FilterContext.StandardFilterOnly)
                                       && (AFilterAlwaysOnLabelContext == TUcoFilterAndFind.FilterContext.None
                                           || AFilterAlwaysOnLabelContext == TUcoFilterAndFind.FilterContext.StandardFilterOnly));
            string extFilter = (bIgnoreExtraFilter) ? String.Empty : GetCurrentFilter(FExtraFilterPanels);

            if ((filter.Length > 0) && (stdFilter.Length > 0))
            {
                filter += " AND ";
            }

            filter += stdFilter;

            if ((filter.Length > 0) && (extFilter.Length > 0))
            {
                filter += " AND ";
            }

            filter += extFilter;
            return filter;
        }

        /// <summary>
        /// Sets a base filter that is applied in addition to the filter specified by the user through the GUI.
        /// </summary>
        /// <param name="ABaseFilter">The filter string</param>
        /// <param name="ABaseFilterShowsAllRecords">Set tis to True if the filter shows all the records accessible from this screen.  Set to False if some records are potentially hidden.</param>
        public void SetBaseFilter(string ABaseFilter, bool ABaseFilterShowsAllRecords)
        {
            FBaseFilter = ABaseFilter;
            FBaseFilterShowsAllRecords = ABaseFilterShowsAllRecords;
        }

        /// <summary>
        /// This method clears the text (or sets checkbox state to indeterminate) on all controls that have a clear button and a filter comparison.
        /// As the items are cleared an argumentPanelChange event will be fired.
        /// </summary>
        public void ClearAllDiscretionaryFilters()
        {
            List<TIndividualFilterFindPanel> SearchList;

            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    SearchList = FStandardFilterPanels;
                }
                else
                {
                    SearchList = FExtraFilterPanels;
                }

                for (int k = 0; k < SearchList.Count; k++)
                {
                    TIndividualFilterFindPanel iffp = SearchList[k];

                    if (iffp.HasClearButton)
                    {
                        if (iffp.FilterComparison == null)
                        {
                            continue;
                        }

                        if (iffp.PanelControl is CheckBox)
                        {
                            ((CheckBox)iffp.PanelControl).CheckState = CheckState.Indeterminate;
                        }
                        else
                        {
                            iffp.PanelControl.Text = String.Empty;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds a panel containing the clone of a control (label or active control)
        /// </summary>
        /// <param name="AFromControl">The control that has been cloned</param>
        /// <returns>The panel containing the cloned control</returns>
        public TIndividualFilterFindPanel FindPanelByClonedFrom(Control AFromControl)
        {
            string LookFor = AFromControl.Name + TFilterPanelControls.FILTER_NAME_SUFFIX;

            List<TIndividualFilterFindPanel> SearchList;

            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    SearchList = FStandardFilterPanels;
                }
                else
                {
                    SearchList = this.FExtraFilterPanels;
                }

                foreach (TIndividualFilterFindPanel iffp in SearchList)
                {
                    if ((iffp.PanelControl.Name == LookFor) || ((iffp.PanelLabel != null) && (iffp.PanelLabel.Name == LookFor)))
                    {
                        return iffp;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the panel that refers to a database column name
        /// </summary>
        /// <param name="AColumnName">The column name</param>
        /// <returns>The panel</returns>
        public TIndividualFilterFindPanel FindPanelByColumnName(string AColumnName)
        {
            List<TIndividualFilterFindPanel> SearchList;

            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    SearchList = FStandardFilterPanels;
                }
                else
                {
                    SearchList = this.FExtraFilterPanels;
                }

                foreach (TIndividualFilterFindPanel iffp in SearchList)
                {
                    if (iffp.DBColumnName == AColumnName)
                    {
                        return iffp;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a control from the filter panel based on the control it was cloned from in the details panel
        /// </summary>
        /// <param name="AFromControl"></param>
        /// <returns></returns>
        public Control FindControlByClonedFrom(Control AFromControl)
        {
            return FindControlByName(AFromControl.Name);
        }

        /// <summary>
        /// Finds a particular control by its name
        /// </summary>
        /// <param name="AControlName">The name of the control to serach for.  Do not include the suffix used internally by this class</param>
        /// <returns>The control</returns>
        public Control FindControlByName(string AControlName)
        {
            List<TIndividualFilterFindPanel> SearchList;
            string SearchName = AControlName + TFilterPanelControls.FILTER_NAME_SUFFIX;

            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    SearchList = FStandardFilterPanels;
                }
                else
                {
                    SearchList = FExtraFilterPanels;
                }

                foreach (TIndividualFilterFindPanel iffp in SearchList)
                {
                    if (iffp.PanelControl is Panel || iffp.PanelControl is GroupBox)
                    {
                        foreach (Control ctrl in iffp.PanelControl.Controls)
                        {
                            if (ctrl.Name == SearchName)
                            {
                                return ctrl;
                            }
                        }
                    }
                    else
                    {
                        if (iffp.PanelControl.Name == SearchName)
                        {
                            return iffp.PanelControl;
                        }
                        else if ((iffp.PanelLabel != null) && (iffp.PanelLabel.Name == SearchName))
                        {
                            return iffp.PanelLabel;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Initialises all ComboBoxes to empty text.  Call this after the filter/find controls have been populated with data
        /// </summary>
        public void InitialiseComboBoxes()
        {
            foreach (TIndividualFilterFindPanel iffp in FStandardFilterPanels)
            {
                if (iffp.PanelControl is TCmbAutoComplete)
                {
                    if (iffp.HasClearButton)
                    {
                        ((TCmbAutoComplete)iffp.PanelControl).SelectedIndex = -1;
                        ((TCmbAutoComplete)iffp.PanelControl).SelectedIndex = -1;
                        ((TCmbAutoComplete)iffp.PanelControl).Text = String.Empty;
                    }
                }
            }

            foreach (TIndividualFilterFindPanel iffp in FExtraFilterPanels)
            {
                if (iffp.PanelControl is TCmbAutoComplete)
                {
                    if (iffp.HasClearButton)
                    {
                        ((TCmbAutoComplete)iffp.PanelControl).SelectedIndex = -1;
                        ((TCmbAutoComplete)iffp.PanelControl).SelectedIndex = -1;
                        ((TCmbAutoComplete)iffp.PanelControl).Text = String.Empty;
                    }
                }
            }
        }
    }

    #endregion

    #region TFindPanelControls

    /// <summary>
    /// A class that contains information about all the controls on the find panel
    /// </summary>
    public class TFindPanelControls
    {
        /// <summary>
        /// A list of all the individual find panels
        /// </summary>
        public List<TIndividualFilterFindPanel> FFindPanels = new List<TIndividualFilterFindPanel>();

        /// <summary>
        /// The suffix to be appended to the control name when it is on the find panel
        /// </summary>
        public const string FIND_NAME_SUFFIX = "_find";

        /// <summary>
        /// Returns a list of panels and their controls on the find panel ready to be passed to the Filter/Find user control
        /// </summary>
        /// <returns>A list of panels and their controls ready to be passed to the Filter/Find user control</returns>
        public List<Panel> GetFindPanels()
        {
            List<Panel> findPanels = null;

            if (FFindPanels.Count > 0)
            {
                findPanels = new List<Panel>();

                for (int i = 0; i < FFindPanels.Count; i++)
                {
                    findPanels.Add(TUcoFilterAndFind.ArgumentPanelHelper.CreateArgumentPanel(
                            FFindPanels[i].PanelLabel, FFindPanels[i].PanelControl));
                }
            }

            return findPanels;
        }

        /// <summary>
        /// Gets a control from the find panel based on the control it was cloned from in the details panel
        /// </summary>
        /// <param name="AFromControl"></param>
        /// <returns></returns>
        public Control FindControlByClonedFrom(Control AFromControl)
        {
            string LookFor = AFromControl.Name + TFindPanelControls.FIND_NAME_SUFFIX;

            foreach (TIndividualFilterFindPanel iffp in FFindPanels)
            {
                if (iffp.PanelControl.Name == LookFor)
                {
                    return iffp.PanelControl;
                }
                else if ((iffp.PanelLabel != null) && (iffp.PanelLabel.Name == LookFor))
                {
                    return iffp.PanelLabel;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a particular control by its name
        /// </summary>
        /// <param name="AControlName">The name of the control to serach for.  Do not include the suffix used internally by this class</param>
        /// <returns>The control</returns>
        public Control FindControlByName(string AControlName)
        {
            string SearchName = AControlName + TFindPanelControls.FIND_NAME_SUFFIX;

            foreach (TIndividualFilterFindPanel iffp in FFindPanels)
            {
                if (iffp.PanelControl is Panel || iffp.PanelControl is GroupBox)
                {
                    foreach (Control ctrl in iffp.PanelControl.Controls)
                    {
                        if (ctrl.Name == SearchName)
                        {
                            return ctrl;
                        }
                    }
                }
                else
                {
                    if (iffp.PanelControl.Name == SearchName)
                    {
                        return iffp.PanelControl;
                    }
                    else if ((iffp.PanelLabel != null) && (iffp.PanelLabel.Name == SearchName))
                    {
                        return iffp.PanelLabel;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a panel containing the clone of a control (label or active control)
        /// </summary>
        /// <param name="AFromControl">The control that has been cloned</param>
        /// <returns>The panel containing the cloned control</returns>
        public TIndividualFilterFindPanel FindPanelByClonedFrom(Control AFromControl)
        {
            string LookFor = AFromControl.Name + TFindPanelControls.FIND_NAME_SUFFIX;

            foreach (TIndividualFilterFindPanel iffp in this.FFindPanels)
            {
                if ((iffp.PanelControl.Name == LookFor) || ((iffp.PanelLabel != null) && (iffp.PanelLabel.Name == LookFor)))
                {
                    return iffp;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the panel that refers to a database column name
        /// </summary>
        /// <param name="AColumnName">The column name</param>
        /// <returns>The panel</returns>
        public TIndividualFilterFindPanel FindPanelByColumnName(string AColumnName)
        {
            foreach (TIndividualFilterFindPanel iffp in this.FFindPanels)
            {
                if (iffp.DBColumnName == AColumnName)
                {
                    return iffp;
                }
            }

            return null;
        }

        /// <summary>
        /// Tests a DataRow to see if it matches the current information in the find panel
        /// </summary>
        /// <param name="ARow">The data row to check for a match</param>
        /// <returns>True if all the user inputs match the specified row</returns>
        public bool IsMatchingRow(DataRow ARow)
        {
            foreach (TIndividualFilterFindPanel iffp in FFindPanels)
            {
                Control ctrl = iffp.PanelControl;

                if (ctrl is CheckBox)
                {
                    CheckBox ctrlAsCheckBox = (CheckBox)ctrl;

                    if (ctrlAsCheckBox.CheckState == CheckState.Indeterminate)
                    {
                        continue;
                    }

                    if (Convert.ToBoolean(ARow[iffp.DBColumnName]) != ctrlAsCheckBox.Checked)
                    {
                        return false;
                    }
                }
                else
                {
                    // We need to think about numeric and text comparisons
                    string controlText = iffp.PanelControl.Text;

                    if (controlText == String.Empty)
                    {
                        continue;
                    }

                    // Just do a case-insensitive regular expression comparison of the text
                    if (!System.Text.RegularExpressions.Regex.IsMatch(ARow[iffp.DBColumnName].ToString(), "(?i)" + controlText))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Initialises all ComboBoxes to empty text.  Call this after the filter/find controls have been populated with data
        /// </summary>
        public void InitialiseComboBoxes()
        {
            foreach (TIndividualFilterFindPanel iffp in FFindPanels)
            {
                if (iffp.PanelControl is TCmbAutoComplete)
                {
                    if (iffp.HasClearButton)
                    {
                        ((TCmbAutoComplete)iffp.PanelControl).SelectedIndex = -1;
                        ((TCmbAutoComplete)iffp.PanelControl).SelectedIndex = -1;
                        ((TCmbAutoComplete)iffp.PanelControl).Text = String.Empty;
                    }
                }
            }
        }
    }

    #endregion

    #endregion

}