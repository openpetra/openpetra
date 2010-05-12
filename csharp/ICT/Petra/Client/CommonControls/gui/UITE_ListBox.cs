//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//        orayh
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// Allows runtime creation of custom list boxes in a PropertyGrid control.
    ///
    /// @Comment First used in the (WinForms) Finance Hierarchy Demo screen.
    /// </summary>
    public class TUITE_ListBox : UITypeEditor
    {
        private ListBox TheListBox;
        private IWindowsFormsEditorService service;

        /// <summary>todoComment</summary>
        public ArrayList ListItemsArray;

        #region TUITE_ListBox

        /// <summary>
        /// constructor
        /// </summary>
        public TUITE_ListBox()
            : base()
        {
            ArrayList ListItemsArray;

            ListItemsArray = new ArrayList();
            ListItemsArray.Add("item 1");
            ListItemsArray.Add("item 2");
            ListItemsArray.Add("item 3");
            DrawDropDown(ListItemsArray);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="al"></param>
        public void DrawDropDown(ArrayList al)
        {
            int numVisibleItems;
            int counter;

            numVisibleItems = al.Count;
            TheListBox = new ListBox();
            TheListBox.Size = new Size(0, (this.TheListBox.Font.Height * numVisibleItems));
            TheListBox.BorderStyle = BorderStyle.None;

            for (counter = 0; counter <= al.Count - 1; counter += 1)
            {
                TheListBox.Items.Add(al[counter]);
            }

            TheListBox.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override System.Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, System.Object value)
        {
            System.Object ReturnValue = false;
            service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (service != null)
            {
                service.DropDownControl(TheListBox);

                if (TheListBox.SelectedItem != null)
                {
                    ReturnValue = TheListBox.SelectedItem;
                }
            }
            else
            {
                ReturnValue = value;
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            UITypeEditorEditStyle ReturnValue;

            if ((context != null) && (context.Instance != null))
            {
                if (context.PropertyDescriptor.IsReadOnly)
                {
                    ReturnValue = UITypeEditorEditStyle.None;
                }
                else
                {
                    ReturnValue = UITypeEditorEditStyle.DropDown;
                }
            }
            else
            {
                ReturnValue = base.GetEditStyle(context);
            }

            return ReturnValue;
        }

        private void OnSelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if (service != null)
            {
                service.CloseDropDown();
                service = null;
            }
        }

        #endregion
    }
}