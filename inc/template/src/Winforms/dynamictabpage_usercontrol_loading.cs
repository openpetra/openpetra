{##USERCONTROLLOADING}
case TDynamicLoadableUserControls.dluc{#CONTROLNAMEWITHOUTPREFIX}:
    // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
    Panel pnlHostForUC{#CONTROLNAMEWITHOUTPREFIX} = new Panel();
    pnlHostForUC{#CONTROLNAMEWITHOUTPREFIX}.AutoSize = true;
    pnlHostForUC{#CONTROLNAMEWITHOUTPREFIX}.Dock = System.Windows.Forms.DockStyle.Fill;
    pnlHostForUC{#CONTROLNAMEWITHOUTPREFIX}.Location = new System.Drawing.Point(0, 0);
    pnlHostForUC{#CONTROLNAMEWITHOUTPREFIX}.Padding = new System.Windows.Forms.Padding(2);
    {#CONTROLNAME}.Controls.Add(pnlHostForUC{#CONTROLNAMEWITHOUTPREFIX});

    // Create the UserControl
    {#DYNAMICCONTROLTYPE} uco{#CONTROLNAMEWITHOUTPREFIX} = new {#DYNAMICCONTROLTYPE}();
    FTabSetup.Add(TDynamicLoadableUserControls.dluc{#CONTROLNAMEWITHOUTPREFIX}, uco{#CONTROLNAMEWITHOUTPREFIX});
    uco{#CONTROLNAMEWITHOUTPREFIX}.Location = new Point(0, 2);
    uco{#CONTROLNAMEWITHOUTPREFIX}.Dock = DockStyle.Fill;
    pnlHostForUC{#CONTROLNAMEWITHOUTPREFIX}.Controls.Add(uco{#CONTROLNAMEWITHOUTPREFIX});

    /*
     * The following four commands seem strange and unnecessary; however, they are necessary
     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
     */
    if (TClientSettings.GUIRunningOnNonStandardDPI)
    {
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        pnlHostForUC{#CONTROLNAMEWITHOUTPREFIX}.Dock = System.Windows.Forms.DockStyle.None;
        pnlHostForUC{#CONTROLNAMEWITHOUTPREFIX}.Dock = System.Windows.Forms.DockStyle.Fill;
    }

    ReturnValue = uco{#CONTROLNAMEWITHOUTPREFIX};
    break;