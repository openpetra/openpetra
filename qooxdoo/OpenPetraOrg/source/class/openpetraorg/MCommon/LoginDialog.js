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
qx.Class.define("openpetraorg.MCommon.LoginDialog", { extend : qx.ui.window.Window,
    construct : function() {
      this.base(arguments, "OpenPetra.org Login");

      this.setModal(true);
      this.setShowClose(false);
      this.setShowMaximize(false);
      this.setShowMinimize(false);
      this.setWidth(190);
      this.setHeight(150);

      /* Container layout */
      var layout = new qx.ui.layout.Grid(9, 5);
      layout.setColumnAlign(0, "right", "top");
      layout.setColumnAlign(2, "right", "top");

      /* Container widget */
      this.__container = new qx.ui.groupbox.GroupBox().set({
        contentPadding: [16, 16, 16, 16]
      });
      this.__container.setLayout(layout);

      this.setLayout(new qx.ui.layout.HBox(10));
      this.add(this.__container);

      /* Labels */
      var labels = ["Name", "Password"];
      for (var i=0; i<labels.length; i++) {
        this.__container.add(new qx.ui.basic.Label(labels[i]).set({
          allowShrinkX: false,
          paddingTop: 3
        }), {row: i, column : 0});
      }

      /* Text fields */
      this.txtUsername = new qx.ui.form.TextField();
      this.txtPassword = new qx.ui.form.PasswordField();

      this.__container.add(this.txtUsername.set({
        allowShrinkX: false,
        paddingTop: 3
      }), {row: 0, column : 1});

      this.__container.add(this.txtPassword.set({
        allowShrinkX: false,
        paddingTop: 3
      }), {row: 1, column : 1});

      /* Button */
      var button1 = this.__okButton =  new qx.ui.form.Button("Login");
      button1.setAllowStretchX(false);

      this.__container.add(
        button1,
        {
          row : 3,
          column : 1
        }
      );

      /* Check input on click */
      button1.addListener("execute", this.checkCredentials, this);

      /* Prepare effect as soon as the container is ready */
      this.__container.addListener("appear", this.__prepareEffect, this);
      
      openpetraorg.MCommon.LoginDialog.MYSELF = this;
    },
    statics :
    {
        __effect: null,
        MYSELF: null
    },
    members :
    {
        checkCredentials : function()
        {
            var params = new openpetraorg.soap.Parameters();
            params.add("username", this.txtUsername.getValue().toUpperCase());
            params.add("password", this.txtPassword.getValue());
    
            this.SoapRunning = openpetraorg.Application.WEBSERVICE.callAsync( "Login", params, function(r, xmlDoc) {
                this.SoapRunning = null;
                if (r instanceof Error) {
                    alert("An error has occured!\r\n\r\n" + r.fileName + " line " + r.lineNumber);
                }
                else {
                      // alert(qx.xml.Element.serialize(xmlDoc));
                      // alert(getSingleNodeText(xmlDoc, "//soap:Envelope/soap:Body/LoginResponse/LoginResult"));
                      if (qx.xml.Element.serialize(xmlDoc).indexOf("<LoginResult>true</LoginResult>") > 0)
                      {
                        // open main screen
                        var MainScreen = new openpetraorg.MCommon.PetraMain();
                        openpetraorg.Application.DESKTOP.add(MainScreen, {left: "30%", top: "30%"});
                        MainScreen.open();       

                        openpetraorg.MCommon.LoginDialog.MYSELF.close();
                      }
                      else
                      {
                        openpetraorg.MCommon.LoginDialog.__effect.start();
                      }

                }
            });
/*

        if (this.txtUsername.getValue() == "demo" && this.txtPassword.getValue() == "demo")
          {
            // open main screen
            var MainScreen = new openpetraorg.MCommon.PetraMain();
            openpetraorg.Application.DESKTOP.add(MainScreen, {left: "30%", top: "30%"});
            MainScreen.open();       

            this.close();
          }
          else
          {
            this.__effect.start();
          }
*/          
        },

        __prepareEffect : function()
        {
          openpetraorg.MCommon.LoginDialog.__effect = new qx.fx.effect.combination.Shake(this.getContainerElement().getDomElement());
        }
    }
 });