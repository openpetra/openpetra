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
using System.Drawing;
using System.Windows.Forms;
using Ict.Common;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// Provides procedures that allow a custom 'disabled' (non-editable) look for
    /// TextBoxes, ComboBoxes and DateTimePickers.
    ///
    /// How it works:
    /// The controls are hidden and labels with a certain look are placed where the
    /// controls lie. When the controls should be 'enabled' again, these labels are
    /// hidden and the controls are shown again.
    ///
    /// Note: Controls other than TextBoxes, ComboBoxes and DateTimePickers are
    /// disabled/enabled in the usual way.
    /// </summary>
    public class CustomEnablingDisabling
    {
        /// <summary>todoComment</summary>
        public const String NO_DISABLING_OF_CONTROL = "dontdisable";

        /// <summary>todoComment</summary>
        public const String CUSTOMDISABLING_ALTHOUGH_INVISIBLE = "CustomDisableAlthoughInvisible";

        /// <summary>for 'LabelInsteadOfControl'</summary>
        public const String LABELINSTEADOFCONTROL_NAMEPREFIX = "lioc";

        /// <summary>todoComment</summary>
        public const Int32 ANYCONTROL_HSHIFT = 1;

        /// <summary>todoComment</summary>
        public const Int32 ANYCONTROL_VSHIFT = 3;

        /// <summary>todoComment</summary>
        public const Int32 COMBOBOX_HSHIFT = 2;

        /// <summary>todoComment</summary>
        public const Int32 DATETIMEPICKER_VSHIFT = 4;

        /// <summary>todoComment</summary>
        public delegate void TDelegateDisabledControlClick(System.Object sender, System.EventArgs e);

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControlContainer"></param>
        /// <param name="AControlToDisable"></param>
        public static void DisableControl(Control AControlContainer, Control AControlToDisable)
        {
            DisableControl(AControlContainer, AControlContainer, null);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControlContainer"></param>
        public static void AdjustLabelControlsAfterResizingGroup(Control AControlContainer)
        {
            Int16 Counter;
            Control InspectControl;

            if (!((AControlContainer is System.Windows.Forms.GroupBox) || (AControlContainer is System.Windows.Forms.Panel)
                  || (AControlContainer is System.Windows.Forms.UserControl) || (AControlContainer is System.Windows.Forms.TabControl)
                  || (AControlContainer is System.Windows.Forms.Form)))
            {
                throw new ArgumentException("AControlContainer",
                    "AControlContainer must be either a GroupBox, Panel, TabControl, UserControl, or a Form");
            }

            // Loop over all Controls in a control container
            for (Counter = 0; Counter <= AControlContainer.Controls.Count - 1; Counter += 1)
            {
                InspectControl = AControlContainer.Controls[Counter];

                if ((InspectControl is System.Windows.Forms.GroupBox) || (InspectControl is System.Windows.Forms.Panel)
                    || (InspectControl is System.Windows.Forms.UserControl) || (InspectControl is System.Windows.Forms.TabControl)
                    || (InspectControl is System.Windows.Forms.Form))
                {
                    // MessageBox.Show('DisableControlGroup for AControlContainer ''' + AControlContainer.Name + ''': calling DisableControlGroup for Control ' + InspectControl.Name);
                    AdjustLabelControlsAfterResizingGroup(InspectControl);
                }
                else
                {
                    // MessageBox.Show('DisableControlGroup for AControlContainer ''' + AControlContainer.Name + ''': calling DisableControl for Control ' + InspectControl.Name);
                    AdjustLabelControlsAfterResizing(AControlContainer, InspectControl);
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControlContainer"></param>
        /// <param name="AControlToAdjust"></param>
        public static void AdjustLabelControlsAfterResizing(Control AControlContainer, Control AControlToAdjust)
        {
            Int16 Counter;

            if (!((AControlContainer is System.Windows.Forms.GroupBox) || (AControlContainer is System.Windows.Forms.Panel)
                  || (AControlContainer is System.Windows.Forms.UserControl) || (AControlContainer is System.Windows.Forms.TabControl)
                  || (AControlContainer is System.Windows.Forms.Form)))
            {
                throw new ArgumentException("AControlContainer",
                    "AControlContainer must be either a GroupBox, Panel, TabControl, UserControl, or a Form");
            }

            try
            {
                if ((AControlToAdjust.Tag == null) || (AControlToAdjust.Tag.ToString() != NO_DISABLING_OF_CONTROL))
                {
                    if (AControlToAdjust is System.Windows.Forms.UserControl)
                    {
                        // Adjust Label Controls After Resizing for all controls in the UserControl
                        // MessageBox.Show('' + AControlToAdjust.Name + ''' is a UserControl > AdjustLabelControlsAfterResizing all controls in the UserControl!');
                        AdjustLabelControlsAfterResizingGroup(AControlToAdjust);

                        // MessageBox.Show('Finished Adjust Label Controls After Resizing for all controls in UserControl ''' + AControlToAdjust.Name + '''!');
                    }
                    else        // AControlToAdjust isn't a UserControl
                    {
                        for (Counter = 0; Counter <= AControlContainer.Controls.Count - 1; Counter += 1)
                        {
                            if (AControlContainer.Controls[Counter].Name == LABELINSTEADOFCONTROL_NAMEPREFIX + AControlToAdjust.Name)
                            {
                                if (AControlContainer.Controls[Counter].Size.Width != AControlToAdjust.Size.Width)
                                {
//                                  MessageBox.Show("AControlToDisable.Size.Width: " + AControlToAdjust.Size.Width.ToString() + "\r\n" +
//                                                  "AControlContainer.Controls[Counter].Size.Width: " + AControlContainer.Controls[Counter].Size.Width.ToString());
                                    AControlContainer.Controls[Counter].Size = AControlToAdjust.Size;
                                    AControlContainer.Controls[Counter].Location = AControlToAdjust.Location;

//                                    MessageBox.Show("AControlContainer.Controls[Counter].Size.Width: " + AControlContainer.Controls[Counter].Size.Width.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception occurred in DisableControl: " + exp.ToString());
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControlContainer"></param>
        /// <param name="AControlToDisable"></param>
        /// <param name="AClickDelegate"></param>
        public static void DisableControl(Control AControlContainer, Control AControlToDisable, TDelegateDisabledControlClick AClickDelegate)
        {
            Int16 Counter;

            System.Windows.Forms.Label LabelInsteadOfControl;

            if (!((AControlContainer is System.Windows.Forms.GroupBox) || (AControlContainer is System.Windows.Forms.Panel)
                  || (AControlContainer is System.Windows.Forms.UserControl) || (AControlContainer is System.Windows.Forms.TabControl)
                  || (AControlContainer is System.Windows.Forms.Form)))
            {
                throw new ArgumentException("AControlContainer",
                    "AControlContainer must be either a GroupBox, Panel, TabControl, UserControl, or a Form");
            }

            try
            {
                if ((AControlToDisable.Visible)
                    || ((!AControlToDisable.Visible) && (AControlToDisable.Tag != null)
                        && (AControlToDisable.Tag.ToString() == CUSTOMDISABLING_ALTHOUGH_INVISIBLE)))
                {
                    if ((AControlToDisable.Tag == null) || (AControlToDisable.Tag.ToString() != NO_DISABLING_OF_CONTROL))
                    {
                        // MessageBox.Show('Call to DisableControl for disabling of Control ''' + AControlToDisable.Name + '''...');
                        // TLogging.Log('Call to DisableControl for disabling of Control ''' + AControlToDisable.Name + '''...', [TLoggingType.ToLogfile]);
                        // TLogging.LogStackTrace([TLoggingType.ToLogfile]);
                        if (AControlToDisable is System.Windows.Forms.UserControl)
                        {
                            // CustomDisable all controls in the UserControl
                            // MessageBox.Show('' + AControlToDisable.Name + ''' is a UserControl > CustomDisable all controls in the UserControl!');
                            DisableControlGroup(AControlToDisable);

                            // MessageBox.Show('Finished disabling all controls in UserControl ''' + AControlToDisable.Name + '''!');
                        }
                        else        // AControlToDisable isn't a UserControl
                        {
                            for (Counter = 0; Counter <= AControlContainer.Controls.Count - 1; Counter += 1)
                            {
                                if (AControlContainer.Controls[Counter].Name == LABELINSTEADOFCONTROL_NAMEPREFIX + AControlToDisable.Name)
                                {
                                    // LabelInsteadOfControl already exists
                                    // MessageBox.Show('1) setting text for control ' + AControlContainer.Controls[Counter].Name + ' (corresponds to Control ' + AControlToDisable.Name + '): ' + AControlToDisable.Text);
                                    // Take over current Text from the original Control
                                    AControlContainer.Controls[Counter].Text = AControlToDisable.Text;

                                    // Hide the original control
                                    AControlToDisable.Visible = false;

                                    // Show the Label instead
                                    AControlContainer.Controls[Counter].Visible = true;
                                    return;
                                }
                                else if (Counter == AControlContainer.Controls.Count - 1)
                                {
                                    // LabelInsteadOfControl doesn't exist yet
                                    // MessageBox.Show('Would add Label for Control ' + AControlToDisable.Name);
                                    if ((AControlToDisable is System.Windows.Forms.TextBox)
                                        || (AControlToDisable is System.Windows.Forms.ComboBox)
                                        || (AControlToDisable is System.Windows.Forms.DateTimePicker))
                                    {
                                        // MessageBox.Show('Adding Label for Control ' + AControlToDisable.Name + '; Type: ' + AControlToDisable.GetType().FullName);
                                        // Create LabelInsteadOfControl
                                        LabelInsteadOfControl = new System.Windows.Forms.Label();
                                        LabelInsteadOfControl.UseMnemonic = false;

                                        if ((AControlToDisable is System.Windows.Forms.ComboBox)
                                            || (AControlToDisable is System.Windows.Forms.DateTimePicker))
                                        {
                                            // Size adjustments
                                            LabelInsteadOfControl.Width = AControlToDisable.Width - COMBOBOX_HSHIFT;
                                            LabelInsteadOfControl.Height = AControlToDisable.Height - (ANYCONTROL_VSHIFT - 1);
                                            LabelInsteadOfControl.Left = AControlToDisable.Left + COMBOBOX_HSHIFT;

                                            // Vertical position adjustments
                                            if (AControlToDisable is System.Windows.Forms.DateTimePicker)
                                            {
                                                LabelInsteadOfControl.Top = AControlToDisable.Top + DATETIMEPICKER_VSHIFT;
                                            }
                                            else
                                            {
                                                LabelInsteadOfControl.Top = AControlToDisable.Top + ANYCONTROL_VSHIFT;
                                            }
                                        }
                                        else
                                        {
                                            LabelInsteadOfControl.Width = AControlToDisable.Width - ANYCONTROL_HSHIFT;
                                            LabelInsteadOfControl.Top = AControlToDisable.Top + ANYCONTROL_VSHIFT;
                                            LabelInsteadOfControl.Left = AControlToDisable.Left + ANYCONTROL_HSHIFT;

                                            // Height adjustments
                                            if (AControlToDisable is System.Windows.Forms.TextBox)
                                            {
                                                if (((System.Windows.Forms.TextBox)AControlToDisable).BorderStyle != BorderStyle.None)
                                                {
                                                    LabelInsteadOfControl.Height = AControlToDisable.Height - (ANYCONTROL_VSHIFT - 1);
                                                }
                                                else
                                                {
                                                    LabelInsteadOfControl.Height = AControlToDisable.Height;
                                                }
                                            }
                                            else
                                            {
                                                LabelInsteadOfControl.Height = AControlToDisable.Height - (ANYCONTROL_VSHIFT - 1);
                                            }

                                            // ControlObject is System.Windows.Forms.TextBox
                                        }

                                        // (ControlObject is System.Windows.Forms.ComboBox) or (ControlObject is System.Windows.Forms.DateTimePicker)
                                        LabelInsteadOfControl.Anchor = AControlToDisable.Anchor;
                                        LabelInsteadOfControl.Name = LABELINSTEADOFCONTROL_NAMEPREFIX + AControlToDisable.Name;
                                        LabelInsteadOfControl.Text = AControlToDisable.Text;
                                        LabelInsteadOfControl.BackColor = System.Drawing.SystemColors.ControlLightLight;
                                        LabelInsteadOfControl.Font = AControlToDisable.Font;

                                        // Assign Click Handler, if a Delegate was specified for it
                                        if (AClickDelegate != null)
                                        {
                                            LabelInsteadOfControl.Click += new System.EventHandler(AClickDelegate);
                                        }

                                        // Hide the original control
                                        AControlToDisable.Visible = false;

                                        // Remember original control in Tag property of the Label (used for setting focus to the Control that lies under the Clicked Label)
                                        LabelInsteadOfControl.Tag = AControlToDisable;

                                        // Add (and therefore show) the Label instead
                                        AControlContainer.Controls.Add(LabelInsteadOfControl);
                                    }
                                    else
                                    {
                                        if ((!(AControlToDisable is System.Windows.Forms.Label))
                                            || (AControlToDisable is System.Windows.Forms.LinkLabel))
                                        {
                                            // just disable any other control
                                            AControlToDisable.Enabled = false;

                                            // MessageBox.Show('Disable any other control ' + AControlToDisable.Name + ' (during Label creation)');
                                        }

                                        // (not (ControlObject is System.Windows.Forms.&Label)) or (ControlObject is System.Windows.Forms.LinkLabel)
                                    }

                                    // ControlObject is ...
                                }

                                // ContainerObject.Controls[Counter].Name = LABELINSTEADOFCONTROL_NAMEPREFIX + ControlObject.Name
                            }

                            // for loop
                        }

                        // ControlObject is System.Windows.Forms.UserControl
                    }

                    // (AControlToDisable.Tag = nil) or (AControlToDisable.Tag.ToString <> NO_DISABLING_OF_CONTROL)
                }

                // AControlToDisable.Visible
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception occurred in DisableControl: " + exp.ToString());
            }

            // try
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControlContainer"></param>
        /// <param name="AControlToEnable"></param>
        public static void EnableControl(Control AControlContainer, Control AControlToEnable)
        {
            Int16 Counter;

            if (!((AControlContainer is System.Windows.Forms.GroupBox)
                  || (AControlContainer is System.Windows.Forms.Panel)
                  || (AControlContainer is System.Windows.Forms.UserControl)
                  || (AControlContainer is System.Windows.Forms.TabControl)
                  || (AControlContainer is System.Windows.Forms.Form)))
            {
                throw new ArgumentException("AControlContainer",
                    "AControlContainer must be either a GroupBox, Panel, TabControl, UserControl, or a Form");
            }

            try
            {
                if (AControlToEnable is System.Windows.Forms.UserControl)
                {
                    // CustomEnable all controls in the UserControl
                    EnableControlGroup(AControlToEnable);
                }
                else
                {
                    for (Counter = 0; Counter <= AControlContainer.Controls.Count - 1; Counter += 1)
                    {
                        if ((AControlContainer.Controls[Counter].Name == LABELINSTEADOFCONTROL_NAMEPREFIX + AControlToEnable.Name)
                            && (AControlContainer.Controls[Counter] is System.Windows.Forms.Label))
                        {
                            // found representing LabelInsteadOfControl
                            AControlToEnable.Visible = true;
                            AControlContainer.Controls[Counter].Visible = false;
                            return;
                        }
                        else
                        {
                            // just enable any other control
                            AControlContainer.Controls[Counter].Enabled = true;
                        }
                    }

                    // for loop
                }

                // ControlObject is System.Windows.Forms.UserControl
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception occurred in EnableControl: " + exp.ToString());
            }

            // try
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControlContainer"></param>
        public static void DisableControlGroup(Control AControlContainer)
        {
            DisableControlGroup(AControlContainer, null);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControlContainer"></param>
        /// <param name="AClickDelegate"></param>
        public static void DisableControlGroup(Control AControlContainer, TDelegateDisabledControlClick AClickDelegate)
        {
            Int16 Counter;
            Control InspectControl;

            if (!((AControlContainer is System.Windows.Forms.GroupBox) || (AControlContainer is System.Windows.Forms.Panel)
                  || (AControlContainer is System.Windows.Forms.UserControl) || (AControlContainer is System.Windows.Forms.TabControl)
                  || (AControlContainer is System.Windows.Forms.Form)))
            {
                throw new ArgumentException("AControlContainer",
                    "AControlContainer must be either a GroupBox, Panel, TabControl, UserControl, or a Form");
            }

            // Loop over all Controls in a control container
            for (Counter = 0; Counter <= AControlContainer.Controls.Count - 1; Counter += 1)
            {
                InspectControl = AControlContainer.Controls[Counter];

                if ((InspectControl is System.Windows.Forms.GroupBox) || (InspectControl is System.Windows.Forms.Panel)
                    || (InspectControl is System.Windows.Forms.UserControl) || (InspectControl is System.Windows.Forms.TabControl)
                    || (InspectControl is System.Windows.Forms.Form))
                {
                    // MessageBox.Show('DisableControlGroup for AControlContainer ''' + AControlContainer.Name + ''': calling DisableControlGroup for Control ' + InspectControl.Name);
                    DisableControlGroup(InspectControl, AClickDelegate);
                }
                else
                {
                    // MessageBox.Show('DisableControlGroup for AControlContainer ''' + AControlContainer.Name + ''': calling DisableControl for Control ' + InspectControl.Name);
                    DisableControl(AControlContainer, InspectControl, AClickDelegate);
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControlContainer"></param>
        public static void EnableControlGroup(Control AControlContainer)
        {
            Int16 Counter;
            Control InspectControl;

            if (!((AControlContainer is System.Windows.Forms.GroupBox) || (AControlContainer is System.Windows.Forms.Panel)
                  || (AControlContainer is System.Windows.Forms.UserControl) || (AControlContainer is System.Windows.Forms.TabControl)
                  || (AControlContainer is System.Windows.Forms.Form)))
            {
                throw new ArgumentException("AControlContainer",
                    "AControlContainer must be either a GroupBox, Panel, TabControl, UserControl, or a Form");
            }

            // Loop over all Controls in a control container
            for (Counter = 0; Counter <= AControlContainer.Controls.Count - 1; Counter += 1)
            {
                InspectControl = AControlContainer.Controls[Counter];

                if ((InspectControl is System.Windows.Forms.GroupBox) || (InspectControl is System.Windows.Forms.Panel)
                    || (InspectControl is System.Windows.Forms.UserControl) || (InspectControl is System.Windows.Forms.TabControl)
                    || (InspectControl is System.Windows.Forms.Form))
                {
                    EnableControlGroup(InspectControl);
                }
                else
                {
                    EnableControl(AControlContainer, InspectControl);
                }
            }
        }
    }
}