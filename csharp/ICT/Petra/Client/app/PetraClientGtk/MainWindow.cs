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
using System;
using System.Collections;
using Mono.Unix;
using Ict.Common;
using Ict.Common.GTK;
using Gtk;

namespace PetraClientMain
{
/// <summary>
/// the main window of Petra
/// it is only displayed after successful login
/// it gives the user the way to access all available modules that he has access permissions for
/// </summary>
public class TFrmMain : Gtk.Window
{
    /// <summary>
    /// constructor for the main window
    /// </summary>
    /// <param name="APetraVersion">the Petra Client version, taken from PetraClient.exe</param>
    /// <param name="AProcessID">process ID of Appdomain on the server</param>
    /// <param name="AWelcomeMessage">welcome message from the server</param>
    /// <param name="ASystemEnabled">is the Petra system enabled at all</param>
    public TFrmMain(System.Version APetraVersion, Int32 AProcessID, String AWelcomeMessage, Boolean ASystemEnabled) : base ("")
    {
        // leave out 'Revision' and 'Build'
        this.Title = String.Format(Catalog.GetString("OpenPetra.org {0} Main Menu"), APetraVersion.ToString(2));
        this.SetSizeRequest(700, 500);
        this.SetIconFromFile("img/petraico-small.ico");
        this.DeleteEvent += CloseApplication;

        Gtk.VBox MainVBox = new VBox();
        Gtk.Toolbar tbrMainToolbar = new Toolbar();
        Gtk.ToolButton btnToolbarClose = new ToolButton(null, Catalog.GetString("Close"));
        tbrMainToolbar.Insert(btnToolbarClose, -1);
        MainVBox.Add(tbrMainToolbar);

        Gtk.Image img = new Image("img/petrarocks.gif");
        MainVBox.Add(img);

        // todo: message window overlaying the picture?

        // TODO: modules
        Gtk.Statusbar stbMain = new Statusbar();
        MainVBox.Add(stbMain);


        Gtk.Button btnTest = new Button();
        btnTest.Label = Catalog.GetString("Test button");
        btnTest.Clicked += new EventHandler(TestClick);

        Gtk.VBox MainArea = new VBox();
        MainArea.PackStart(btnTest, false, false, 5);

        this.Add(MainArea);
    }

    private void TestClick(object obj, EventArgs args)
    {
        Ict.Petra.Client.MSysMan.TFrmMaintainUsers test = new Ict.Petra.Client.MSysMan.TFrmMaintainUsers();
    }

    private void CloseApplication(object obj, DeleteEventArgs args)
    {
        // close the Petra client cleanly
        PetraClientShutdown.Shutdown.StopPetraClient();
    }
}
}