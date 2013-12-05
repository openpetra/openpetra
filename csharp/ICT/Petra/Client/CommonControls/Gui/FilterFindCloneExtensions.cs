//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;


namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// An extension class for cloning controls.  Written especially for the FilterFind panels so is not completely generic.
    /// A few properties are modified (such as transparency and text box height) in order to work right on Filter/Find panels.
    /// </summary>
    public static class TCloneFilterFindControl
    {
        /// <summary>
        /// This performs a 'shallow' clone returning a control whose basic properties are the same as the original.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="AControlToClone">The control to be cloned.  Only labels, checkboxes and text boxes can be shallow cloned.</param>
        /// <param name="ANameSuffix">A suffix to add to the name to make the new clone name different from the original control</param>
        /// <param name="AParentControl">If AControlToClone is on a Panel or in a GroupBox, set AParentControl to a reference to this control</param>
        /// <returns>The clone</returns>
        public static T ShallowClone<T>(this T AControlToClone, string ANameSuffix, Control AParentControl = null)
        where T : Control
        {
            T instance = null;

            if (AControlToClone is Label || AControlToClone is CheckBox || AControlToClone is RadioButton)
            {
                instance = Activator.CreateInstance<T>();
                instance.Text = AControlToClone.Text;
                instance.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AControlToClone is TextBox || AControlToClone is TtxtPetraDate)
            {
                instance = Activator.CreateInstance<T>();
            }
            else if (AControlToClone is Panel || AControlToClone is GroupBox)
            {
                instance = Activator.CreateInstance<T>();

                if (AControlToClone is GroupBox)
                {
                    instance.Text = AControlToClone.Text;
                    instance.Height = 10;
                }

                for (int i = 0; i < AControlToClone.Controls.Count; i++)
                {
                    Control ctrl = AControlToClone.Controls[i];

                    if (ctrl is RadioButton)
                    {
                        instance.Controls.Add(TCloneFilterFindControl.ShallowClone<RadioButton>((RadioButton)ctrl,
                                TFilterPanelControls.FILTER_NAME_SUFFIX,
                                instance));
                    }
                    else if (ctrl is CheckBox)
                    {
                        instance.Controls.Add(TCloneFilterFindControl.ShallowClone<CheckBox>((CheckBox)ctrl, TFilterPanelControls.FILTER_NAME_SUFFIX,
                                instance));
                    }
                    else if (ctrl is TextBox)
                    {
                        instance.Controls.Add(TCloneFilterFindControl.ShallowClone<TextBox>((TextBox)ctrl, TFilterPanelControls.FILTER_NAME_SUFFIX,
                                instance));
                    }
                    else
                    {
                        throw new Exception("Only radio buttons, check boxes and text boxes can be cloned inside a panel or group box");
                    }
                }
            }
            else
            {
                throw new NotSupportedException("Only labels, checkboxes, radio buttons, panels, group boxes and text boxes can be shallow cloned");
            }

            // add our name suffix
            instance.Name = AControlToClone.Name + ANameSuffix;
            instance.Tag = CommonTagString.SUPPRESS_CHANGE_DETECTION;

            // our controls are always enabled and have a standard width (which usually gets reduced to fit)
            instance.Width = 280;
            instance.Enabled = true;

            if (AParentControl != null)
            {
                // we need to position this control on the parent
                bool bIsGroupBox = AParentControl is GroupBox;

                if (bIsGroupBox)
                {
                    instance.Left = 15;
                }

                bool bIsFirstControl = AParentControl.Controls.Count == 0;

                if (bIsFirstControl)
                {
                    instance.Top = (bIsGroupBox) ? 10 : 0;
                }
                else
                {
                    Control prevControl = AParentControl.Controls[AParentControl.Controls.Count - 1];
                    instance.Top = prevControl.Top + prevControl.Height;
                }

                AParentControl.Height += instance.Height;
            }

            return instance;
        }

        /// <summary>
        /// Shallow clones any of our various styles of ComboBox and creates a TCmbAutoComplete one that has the same items.
        /// The new control has AcceptNewValues=true so that the display text can be cleared
        /// </summary>
        /// <typeparam name="T">The type of control to clone</typeparam>
        /// <param name="AControlToClone">The control to clone</param>
        /// <param name="ANameSuffix">The suffix to add to the name to distinguish it from the source control</param>
        /// <returns></returns>
        public static TCmbAutoComplete ShallowCloneToComboBox<T>(this T AControlToClone, string ANameSuffix)
        where T : Control
        {
            // We always clone to a TCmbAutoComplete
            TCmbAutoComplete instance = new TCmbAutoComplete();

            ComboBox clonedFrom = null;

            if (AControlToClone is TCmbLabelled)
            {
                clonedFrom = ((TCmbLabelled)(object)AControlToClone).cmbCombobox;
            }
            else if (AControlToClone is ComboBox)
            {
                clonedFrom = (ComboBox)(object)AControlToClone;
            }
            else if (AControlToClone is GroupBox)
            {
                clonedFrom = new TCmbAutoComplete();
                clonedFrom.DataSource = null;

                for (int i = 0; i < AControlToClone.Controls.Count; i++)
                {
                    if (AControlToClone.Controls[i] is RadioButton)
                    {
                        clonedFrom.Items.Add(AControlToClone.Controls[i].Text);
                    }
                }
            }
            else
            {
                throw new Exception("Cannot copy the data from " + AControlToClone.Name);
            }

            // Clone the data
            if (clonedFrom.DataSource == null)
            {
                // No fancy data source - just copy the items as strings
                foreach (string item in clonedFrom.Items)
                {
                    instance.AddStringItem(item);
                }
            }
            else
            {
                // Clone the DataSource from the clonedFrom to the new instance
                instance.DisplayMember = clonedFrom.DisplayMember;
                instance.ValueMember = clonedFrom.ValueMember;
                instance.DataSource = ((DataView)clonedFrom.DataSource).ToTable().DefaultView;
            }

            // add our name suffix
            instance.Name = AControlToClone.Name + ANameSuffix;
            instance.Tag = CommonTagString.SUPPRESS_CHANGE_DETECTION;

            // our controls are always enabled and have a standard width (which usually gets reduced to fit)
            instance.Width = 280;
            instance.Enabled = true;

            return instance;
        }
    }
}