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
using System;
using System.Data;
using System.Collections;

namespace Ict.Common.GTK
{
    /// <summary>
    /// an easy to use DataGrid, using Gtk.TreeView
    /// </summary>
    public class TDataGrid : Gtk.TreeView
    {
        /// <summary>
        /// this will generate a data grid
        /// and fill it with the columns and content from the datatable
        /// </summary>
        /// <param name="ATable">the columns and the values</param>
        public void Init(DataTable ATable)
        {
            ArrayList types = new ArrayList();

            foreach (DataColumn col in ATable.Columns)
            {
                if (col.DataType == typeof(System.IO.Path))
                {
                    // could also use artistColumn.SetCellDataFunc (artistNameCell, new Gtk.TreeCellDataFunc (RenderArtistName));
                    // that would be more similar to Client/lib/MCommon/logic/UC_PartnerAddresses.cs GetMailingAddressIconForGridRow
                    types.Add(typeof(Gdk.Pixbuf));
                    this.AppendColumn(col.Caption, new Gtk.CellRendererPixbuf(), "pixbuf", col.Ordinal);
                }
                else if (col.DataType == typeof(bool))
                {
                    types.Add(col.DataType);
                    this.AppendColumn(col.Caption, new Gtk.CellRendererToggle(), "active", col.Ordinal);
                }
                else
                {
                    types.Add(col.DataType);
                    this.AppendColumn(col.Caption, new Gtk.CellRendererText(), "text", col.Ordinal);
                }
            }

            Gtk.ListStore store = new Gtk.ListStore(
                (System.Type[])types.ToArray(typeof(System.Type)));

            foreach (DataRow row in ATable.Rows)
            {
                ArrayList values = new ArrayList();

                foreach (DataColumn col in ATable.Columns)
                {
                    if (col.DataType == typeof(System.IO.Path))
                    {
                        values.Add(new Gdk.Pixbuf(row[col.Ordinal].ToString()));
                    }
                    else if (col.DataType == typeof(System.Boolean))
                    {
                        values.Add(Convert.ToBoolean(row[col.Ordinal]));
                    }
                    else
                    {
                        values.Add(Convert.ToString(row[col.Ordinal]));
                    }
                }

                store.AppendValues(values.ToArray());
            }

            this.Model = store;

            foreach (DataColumn col in ATable.Columns)
            {
                this.Columns[col.Ordinal].SortColumnId = col.Ordinal;
            }
        }
    }
}