/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Windows.Forms;

namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// Contains routines that help with Focus setting.
    ///
    /// </summary>
    public class TFocusing : System.Object
    {
        #region TFocusing

        /// <summary>
        /// Selects a TabPage by name on a specified TabControl control.
        ///
        /// </summary>
        /// <param name="ATabControl">TabControl control to which the TabPage belongs.</param>
        /// <param name="ATabToSelect">Name of the TabPage that should be selected.
        /// </param>
        /// <returns>void</returns>
        public static void SelectTab(TabControl ATabControl, String ATabToSelect)
        {
            Int16 Counter;

            for (Counter = 0; Counter <= ATabControl.TabPages.Count - 1; Counter += 1)
            {
                if (ATabControl.TabPages[Counter].Name == ATabToSelect)
                {
                    ATabControl.SelectedIndex = Counter;

                    // MessageBox.Show('Selected TabPage ' + ATabToSelect + ' in TabControl ' + ATabControl.Name);
                }
            }
        }

        /// <summary>
        /// Sets the Focus to a control within a given Form or a UserControl.
        ///
        /// </summary>
        /// <param name="AContainer">Either a Form or a UserControl.</param>
        /// <param name="AControlName">Name of the control to set the focus to</param>
        /// <returns>The name of the control if the control is found (used in
        /// SetFocusOnDataBoundControlInternal), otherwise ''.
        /// </returns>
        public static String SetFocusOnControlInFormOrUserControl(ContainerControl AContainer, String AControlName)
        {
            return SetFocusOnControlInFormOrUserControl(AContainer, AControlName, null, "");
        }

        /// <summary>
        /// Sets the Focus to a control within a given Form or a UserControl.
        ///
        /// </summary>
        /// <param name="AContainer">Either a Form or a UserControl.</param>
        /// <param name="AControlName">Name of the control to set the focus to</param>
        /// <param name="ATabControl">A TabControl control whose TabPage should be selected to
        /// show the focused control.</param>
        /// <param name="ATabPageToFocus">The name of the TabPage that should be selected to
        /// show the focused control.</param>
        /// <returns>The name of the control if the control is found (used in
        /// SetFocusOnDataBoundControlInternal), otherwise ''.
        /// </returns>
        public static String SetFocusOnControlInFormOrUserControl(ContainerControl AContainer,
            String AControlName,
            TabControl ATabControl,
            String ATabPageToFocus)
        {
            String ReturnValue = "";
            Int16 Counter;

            for (Counter = 0; Counter <= AContainer.Controls.Count - 1; Counter += 1)
            {
                if ((AContainer.Controls[Counter] is System.Windows.Forms.Panel)
                    || (AContainer.Controls[Counter] is System.Windows.Forms.GroupBox)
                    || (AContainer.Controls[Counter] is System.Windows.Forms.UserControl)
                    || (AContainer.Controls[Counter] is System.Windows.Forms.TabControl))
                {
                    // MessageBox.Show('SetFocusOnControlInFormOrUserControl: Checking control in Panel/GrouBox/UserControl/TabControl ' + AContainer.Controls[Counter].Name);
                    // Control is a Panel or GroupBox  or a UserControl or a TabControl
                    AControlName = SetFocusOnControlInControlContainer(AContainer.Controls[Counter], AControlName, ATabControl, ATabPageToFocus);

                    if (AControlName != "")
                    {
                        ReturnValue = AControlName;
                        break;
                    }
                }
                else
                {
                    // Control is a normal control

                    // MessageBox.Show('SetFocusOnControlInFormOrUserControl: Checking control ' + AContainer.Controls[Counter].Name);
                    if (AContainer.Controls[Counter].Name == AControlName)
                    {
                        // MessageBox.Show('SetFocusOnControlInFormOrUserControl: FOUND IT!');
                        AContainer.Controls[Counter].Focus();
                        ReturnValue = AContainer.Controls[Counter].Name;
                        break;
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Sets the Focus to a control within a GroupBox, Panel, TabControl or
        /// UserControl.
        ///
        /// </summary>
        /// <param name="AControlContainer">Either a GroupBox or Panel.</param>
        /// <param name="AControlName">Name of the control to set the focus to</param>
        /// <returns>The name of the control if the control is found (used in
        /// SetFocusOnControlInFormOrUserControl), otherwise ''.
        /// </returns>
        public static String SetFocusOnControlInControlContainer(System.Object AControlContainer, String AControlName)
        {
            return SetFocusOnControlInControlContainer(AControlContainer, AControlName, null, "");
        }

        /// <summary>
        /// Sets the Focus to a control within a GroupBox, Panel, TabControl or
        /// UserControl.
        ///
        /// </summary>
        /// <param name="AControlContainer">Either a GroupBox or Panel.</param>
        /// <param name="AControlName">Name of the control to set the focus to</param>
        /// <param name="ATabControl">A TabControl control whose TabPage should be selected to
        /// show the focused control.</param>
        /// <param name="ATabPageToFocus">The name of the TabPage that should be selected to
        /// show the focused control.</param>
        /// <returns>The name of the control if the control is found (used in
        /// SetFocusOnControlInFormOrUserControl), otherwise ''.
        /// </returns>
        public static String SetFocusOnControlInControlContainer(System.Object AControlContainer,
            String AControlName,
            TabControl ATabControl,
            String ATabPageToFocus)
        {
            String ReturnValue = "";
            Int16 Counter;
            String ControlName;

            // MessageBox.Show('SetFocusOnControlInGroupBoxOrPanel: TabPage: ' + ATabPageToFocus);
            ControlName = "";

            if ((AControlContainer is System.Windows.Forms.GroupBox) || (AControlContainer is System.Windows.Forms.Panel)
                || (AControlContainer is System.Windows.Forms.UserControl) || (AControlContainer is System.Windows.Forms.TabControl))
            {
                if (AControlContainer is System.Windows.Forms.GroupBox)
                {
                    for (Counter = 0; Counter <= ((GroupBox)AControlContainer).Controls.Count - 1; Counter += 1)
                    {
                        if ((((GroupBox)AControlContainer).Controls[Counter] is System.Windows.Forms.Panel)
                            || (((GroupBox)AControlContainer).Controls[Counter] is System.Windows.Forms.GroupBox)
                            || (((GroupBox)AControlContainer).Controls[Counter] is System.Windows.Forms.UserControl)
                            || (((GroupBox)AControlContainer).Controls[Counter] is System.Windows.Forms.TabControl))
                        {
                            // Control in GroupBox is a Panel or GroupBox or a UserControl or a TabControl
                            // MessageBox.Show('SetFocusOnControlInGroupBoxOrPanel: Looking in GroupBox ' + (AControlContainer as GroupBox).Name + '...');
                            ControlName = SetFocusOnControlInControlContainer(((GroupBox)AControlContainer).Controls[Counter],
                                AControlName,
                                ATabControl,
                                ATabPageToFocus);

                            if (ControlName != "")
                            {
                                ReturnValue = ControlName;
                                break;
                            }
                        }
                        else
                        {
                            // Control in GroupBox is a normal control

                            // MessageBox.Show('SetFocusOnControlInGroupBoxOrPanel: Checking control ' + (AControlContainer as GroupBox).Controls[Counter].Name);
                            if (((GroupBox)AControlContainer).Controls[Counter].Name == AControlName)
                            {
                                // MessageBox.Show('SetFocusOnControlInGroupBoxOrPanel: FOUND IT!');
                                if (ATabControl != null)
                                {
                                    SelectTab(ATabControl, ATabPageToFocus);
                                }

                                ((GroupBox)AControlContainer).Controls[Counter].Focus();
                                ReturnValue = ((GroupBox)AControlContainer).Controls[Counter].Name;
                                break;
                            }
                        }
                    }
                }

                if (AControlContainer is System.Windows.Forms.Panel)
                {
                    for (Counter = 0; Counter <= ((Panel)AControlContainer).Controls.Count - 1; Counter += 1)
                    {
                        if ((((Panel)AControlContainer).Controls[Counter] is System.Windows.Forms.Panel)
                            || (((Panel)AControlContainer).Controls[Counter] is System.Windows.Forms.GroupBox)
                            || (((Panel)AControlContainer).Controls[Counter] is System.Windows.Forms.UserControl)
                            || (((Panel)AControlContainer).Controls[Counter] is System.Windows.Forms.TabControl))
                        {
                            // Control in Panel is a Panel or GroupBox or a UserControl
                            // MessageBox.Show('SetFocusOnControlInGroupBoxOrPanel: Looking in Panel ' + (AControlContainer as Panel).Name + '...');
                            ControlName = SetFocusOnControlInControlContainer(((Panel)AControlContainer).Controls[Counter],
                                AControlName,
                                ATabControl,
                                ATabPageToFocus);

                            if (ControlName != "")
                            {
                                ReturnValue = ControlName;
                                break;
                            }
                        }
                        else
                        {
                            // Control in Panel is a normal control

                            // MessageBox.Show('SetFocusOnControlInGroupBoxOrPanel: Checking control ' + (AControlContainer as Panel).Controls[Counter].Name);
                            if (((Panel)AControlContainer).Controls[Counter].Name == AControlName)
                            {
                                // MessageBox.Show('SetFocusOnControlInGroupBoxOrPanel: FOUND IT!');
                                if (ATabControl != null)
                                {
                                    SelectTab(ATabControl, ATabPageToFocus);
                                }

                                ((Panel)AControlContainer).Controls[Counter].Focus();
                                ReturnValue = ((Panel)AControlContainer).Controls[Counter].Name;
                                break;
                            }
                        }
                    }
                }

                if (AControlContainer is System.Windows.Forms.TabControl)
                {
                    for (Counter = 0; Counter <= ((TabControl)AControlContainer).TabPages.Count - 1; Counter += 1)
                    {
                        // MessageBox.Show('SetFocusOnControlInGroupBoxOrPanel: Looking in TabControl ' + (AControlContainer as TabControl).Name + '...');
                        ControlName =
                            SetFocusOnControlInControlContainer(((TabControl)AControlContainer).TabPages[Counter], AControlName,
                                ((TabControl)AControlContainer), ((TabControl)AControlContainer).TabPages[Counter].Name);

                        if (ControlName != "")
                        {
                            // SelectTab((AControlContainer as TabControl), (AControlContainer as TabControl).TabPages[Counter].Name);
                            // ControlName := SetFocusOnControlInGroupBoxOrPanel((AControlContainer as TabControl).TabPages[Counter], AControlName);
                            ReturnValue = ControlName;
                            break;
                        }
                    }
                }

                if (AControlContainer is System.Windows.Forms.UserControl)
                {
                    // MessageBox.Show('SetFocusOnControlInGroupBoxOrPanel: Looking in UserControl ' + (AControlContainer as UserControl).Name + '...');
                    ControlName = SetFocusOnControlInFormOrUserControl(((UserControl)AControlContainer), AControlName, ATabControl, ATabPageToFocus);

                    if (ControlName != "")
                    {
                        return ControlName;
                    }
                }
            }
            else
            {
            }

            // raise exception
            return ReturnValue;
        }

        #endregion
    }
}