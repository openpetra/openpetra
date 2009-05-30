/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
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
using Gtk;
using GLib;
using System;
using System.Data;
using System.Collections;
using Mono.Unix;

namespace Ict.Common.GTK
{
    /// <summary>
    /// an easy to use Window that contains
    /// a grid and an area for editing the details
    /// </summary>
    public class TFrmBrowseEdit : Gtk.Window
    {
        private Gtk.Toolbar FToolbar;
        private TDataGrid FDataGrid;
        private Gtk.HBox FFilterBox;
        private Gtk.VBox FDetailBox;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ATitle"></param>
        /// <param name="AWidth"></param>
        /// <param name="AHeight"></param>
        public TFrmBrowseEdit(String ATitle, int AWidth, int AHeight) :
            base(ATitle)
        {
            SetSizeRequest(AWidth, AHeight);

            FToolbar = new Gtk.Toolbar();
            FDataGrid = new TDataGrid();
            FFilterBox = new Gtk.HBox();
            FDetailBox = new Gtk.VBox();
        }

        /// <summary>
        /// assemble the window from the different parts and show it
        /// </summary>
        public void AssembleAndShow()
        {
            // not implemented: could do detachable toolbar by putting it into a HandleBox

            // Create a box to hold the toolbar, find section, grid, and detail section
            Gtk.VBox box = new Gtk.VBox();

            // Add the widgets to the box
            box.PackStart(FToolbar, false, false, 5);
            box.PackStart(FFilterBox, false, false, 5);
            box.PackStart(FDataGrid, true, true, 5);
            box.PackStart(FDetailBox, false, false, 5);

            this.Add(box);

            this.ShowAll();
        }

        /// <summary>
        /// set the columns and load the data from the table
        /// </summary>
        /// <param name="ATable"></param>
        public void InitGrid(DataTable ATable)
        {
            FDataGrid.Init(ATable);
        }

        /// <summary>
        /// populate the toolbar
        /// </summary>
        /// <param name="AEdit"></param>
        /// <param name="AAdd"></param>
        /// <param name="ADelete"></param>
        public void InitToolbar(bool AEdit, bool AAdd, bool ADelete)
        {
            // close button
            FToolbar.Insert(new Gtk.ToolButton(null, Catalog.GetString("Close")), -1);

            // save button
            if (AEdit || AAdd || ADelete)
            {
                FToolbar.Insert(new Gtk.ToolButton(null, Catalog.GetString("Save")), -1);
            }

            if (AEdit)
            {
                FToolbar.Insert(new Gtk.ToolButton(null, Catalog.GetString("Edit")), -1);
            }

            if (AAdd)
            {
                FToolbar.Insert(new Gtk.ToolButton(null, Catalog.GetString("Add")), -1);
            }

            if (ADelete)
            {
                FToolbar.Insert(new Gtk.ToolButton(null, Catalog.GetString("Delete")), -1);
            }
        }
    }
}