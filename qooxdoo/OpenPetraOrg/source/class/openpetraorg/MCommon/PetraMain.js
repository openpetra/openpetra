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
qx.Class.define("openpetraorg.MCommon.PetraMain", { extend : qx.ui.window.Window,
    construct : function() {
        this.base(arguments, "OpenPetra.org Main Screen");
        this.setWidth(600);
        this.setHeight(200);
        this.setShowMinimize(false);

        this.setContentPadding(0);
        this.add(this.getMenuBar(), {left: 0, top: 0});

        this.setLayout(new qx.ui.layout.VBox(10));
        
        var panel = new qx.ui.container.Composite();
        this.add(panel);
        panel.setLayout(new qx.ui.layout.HBox(10));
        var btnPartnerModule = new qx.ui.form.Button("Partner");
        btnPartnerModule.addListener("execute", this.openPartnerMain, this);        
        panel.add(btnPartnerModule);
        panel.add(new qx.ui.form.Button("Finance"));
        panel.add(new qx.ui.form.Button("Personnel"));
        panel.add(new qx.ui.form.Button("Financial Development"));
        panel.add(new qx.ui.form.Button("Conference"));
    },
  members :
  {
    getMenuBar : function()
    {
      var frame = new qx.ui.container.Composite(new qx.ui.layout.Grow);

      var menubar = new qx.ui.menubar.MenuBar;
      menubar.setWidth(600);
      frame.add(menubar);

      var fileMenu = new qx.ui.menubar.Button("File", null, this.getFileMenu());
      var editMenu = new qx.ui.menubar.Button("Edit", null, null);
      var helpMenu = new qx.ui.menubar.Button("Help", null, null);

      menubar.add(fileMenu);
      menubar.add(editMenu);
      menubar.add(helpMenu);

      return frame;
    },

    getFileMenu : function()
    {
      var menu = new qx.ui.menu.Menu;

      var newButton = new qx.ui.menu.Button("New", "icon/16/actions/document-new.png");
      var exitButton = new qx.ui.menu.Button("Exit", "icon/16/actions/application-exit.png");

      menu.add(newButton);
      menu.add(exitButton);

      exitButton.addListener("execute", this.exitApplication, this);

      return menu;
    },
    
    exitApplication: function() {
        // TODO reset cookies, close session?
        // show login screen
        var loginScreen = new openpetraorg.MCommon.LoginDialog();
        openpetraorg.Application.DESKTOP.add(loginScreen, {left: "30%", top: "30%"});
        loginScreen.open();
        this.close();
    },
    
    openPartnerMain: function() {
        // open partner module screen
        var PartnerModuleScreen = new openpetraorg.MPartner.PartnerMain(openpetraorg.Application.DESKTOP);
        openpetraorg.Application.DESKTOP.add(PartnerModuleScreen, {left: 30, top: 30});
        PartnerModuleScreen.open();       
    }
  }
});    
