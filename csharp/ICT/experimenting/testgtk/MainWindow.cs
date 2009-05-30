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
using System.Collections;
using System.Data;

namespace testgtk
{
/// this shows some things how to use a grid with GTK#
/// see also http://mono-project.com/GtkSharp_TreeView_Tutorial
public class MessageBox
{
    public static void Show(String msg, String title)
    {
        MessageDialog md = new MessageDialog(null,
            DialogFlags.DestroyWithParent,
            MessageType.Info,
            ButtonsType.Close, msg);

        md.Title = title;
        md.Run();
        md.Destroy();
    }
}

public class GtkHelpers
{
    public static Gtk.TreeView CreateTreeView(DataTable ATable)
    {
        Gtk.TreeView tree = new Gtk.TreeView();

        ArrayList types = new ArrayList();

        foreach (DataColumn col in ATable.Columns)
        {
            if (col.DataType == typeof(System.IO.Path))
            {
                // could also use artistColumn.SetCellDataFunc (artistNameCell, new Gtk.TreeCellDataFunc (RenderArtistName));
                // that would be more similar to Client/lib/MCommon/logic/UC_PartnerAddresses.cs GetMailingAddressIconForGridRow
                types.Add(typeof(Gdk.Pixbuf));
                tree.AppendColumn(col.Caption, new Gtk.CellRendererPixbuf(), "pixbuf", col.Ordinal);
            }
            else if (col.DataType == typeof(bool))
            {
                types.Add(col.DataType);
                tree.AppendColumn(col.Caption, new Gtk.CellRendererToggle(), "active", col.Ordinal);
            }
            else
            {
                types.Add(col.DataType);
                tree.AppendColumn(col.Caption, new Gtk.CellRendererText(), "text", col.Ordinal);
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

        tree.Model = store;

        foreach (DataColumn col in ATable.Columns)
        {
            tree.Columns[col.Ordinal].SortColumnId = col.Ordinal;
        }

        return tree;
    }
}

public class TreeViewExample
{
    public static void Main()
    {
        try
        {
            Gtk.Application.Init();
            new TreeViewExample();
            Gtk.Application.Run();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.GetType().ToString() + ": " + e.Message + Environment.NewLine +
                e.StackTrace, "error");
        }
    }

    private Gtk.TreeView CreateDemoTreeView()
    {
        Gtk.TreeView tree = new Gtk.TreeView();
        Gtk.ListStore musicListStore = new Gtk.ListStore(typeof(Gdk.Pixbuf),
            typeof(bool),
            typeof(string), typeof(string));

        tree.AppendColumn("Icon", new Gtk.CellRendererPixbuf(), "pixbuf", 0);
        tree.AppendColumn("Ticked", new Gtk.CellRendererToggle(), "active", 1);
        tree.AppendColumn("Artist", new Gtk.CellRendererText(), "text", 2);
        tree.AppendColumn("Title", new Gtk.CellRendererText(), "text", 3);

        musicListStore.AppendValues(new Gdk.Pixbuf("Address_Best.png"), true,
            "Rupert1", "Yellow bananas");
        musicListStore.AppendValues(new Gdk.Pixbuf("Address_Best.png"), false,
            "Rupert3", "Green bananas");
        musicListStore.AppendValues(new Gdk.Pixbuf("Address_Best.png"), true,
            "Rupert2", "Red apples");

        tree.Model = musicListStore;

        tree.Columns[0].SortColumnId = 0;
        tree.Columns[1].SortColumnId = 1;
        tree.Columns[2].SortColumnId = 2;

        return tree;
    }

    private DataTable GenerateDemoDataTable()
    {
        DataTable demo = new DataTable();

        demo.Columns.Add("Icon", typeof(System.IO.Path));
        demo.Columns.Add("Ticked", typeof(System.Boolean));
        demo.Columns.Add("Artist", typeof(System.String));

        demo.Rows.Add(new object[] { "test1.png", false, "Rupert1" });
        demo.Rows.Add(new object[] { "test2.jpg", true, "Rupert2" });

        return demo;
    }

    public TreeViewExample()
    {
        Gtk.Window window = new Gtk.Window("TreeView Example");
        window.SetSizeRequest(500, 200);

        // when this window is deleted, it'll run delete_event()
        window.DeleteEvent += delete_event;

        //    Gtk.TreeView tree = CreateDemoTreeView();
        DataTable table = GenerateDemoDataTable();
        Gtk.TreeView tree = GtkHelpers.CreateTreeView(table);
        window.Add(tree);
        window.ShowAll();
    }

    // runs when the user deletes the window using the "close
    // window" widget in the window frame.
    static void delete_event(object obj, DeleteEventArgs args)
    {
        Application.Quit();
    }
}
}