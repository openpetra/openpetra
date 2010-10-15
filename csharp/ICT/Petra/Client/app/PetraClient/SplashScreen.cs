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
using GNU.Gettext;
using Ict.Common;

namespace SplashScreen
{
/// <summary>
/// Description of SplashScreen.
/// </summary>
public partial class frmSplashScreen : Form
{
    /// <summary>todoComment</summary>
    public delegate void TMyUpdateDelegate();

    private string FPetraVersion, FInstallationKind, FCustomText;
    private string FProgressText = "Loading...";
    private const int TIMER_INTERVAL = 50;

    #region MessageBox-related Variables
    private string FMessageBoxMessage, FMessageBoxTitle;
    private MessageBoxButtons FMessageBoxButtons;
    private MessageBoxIcon FMessageBoxIcon;
    private MessageBoxDefaultButton FMessageBoxDefaultButton;
    private DialogResult FMessageBoxDialogResult;
    #endregion

    /// <summary>
    /// constructor
    /// </summary>
    public frmSplashScreen()
    {
        //
        // The InitializeComponent() call is required for Windows Forms designer support.
        //
        InitializeComponent();
        #region CATALOGI18N

        // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
        this.lblProgressText.Text = Catalog.GetString("Loading...");
        this.lblCustomText.Text = Catalog.GetString("Custom Text can be displayed here...");
        #endregion

        ucoPetraLogoAndVersionInfo.PetraVersion = "";
        ucoPetraLogoAndVersionInfo.InstallationKind = "";
        lblCustomText.Text = "";

        // this timer is needed to check whether the dialog should be closed
        timerClose.Interval = TIMER_INTERVAL;
        timerClose.Start();
    }

    #region Properties

    /// <summary>
    /// petra version
    /// </summary>
    public string PetraVersion
    {
        get
        {
            return ucoPetraLogoAndVersionInfo.PetraVersion;
        }
    }

    /// <summary>
    /// type of installation
    /// </summary>
    public string InstallationKind
    {
        get
        {
            return ucoPetraLogoAndVersionInfo.InstallationKind;
        }
    }

    /// <summary>todoComment</summary>
    public string CustomText
    {
        get
        {
            return lblCustomText.Text;
        }
    }

    /// <summary>todoComment</summary>
    public string ProgressText
    {
        get
        {
            return lblProgressText.Text;
        }

        set
        {
            SetProgressText(value);
        }
    }

    /// <summary>todoComment</summary>
    public DialogResult MessageBoxDialogResult
    {
        get
        {
            return FMessageBoxDialogResult;
        }
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Updates Texts on the Form
    /// Call only with .Invoke to make it actually happen!
    /// </summary>
    /// <returns>void</returns>
    private void SetTexts()
    {
        ucoPetraLogoAndVersionInfo.PetraVersion = FPetraVersion;
        ucoPetraLogoAndVersionInfo.InstallationKind = FInstallationKind;
        lblCustomText.Text = FCustomText;
        lblProgressText.Text = FProgressText;
    }

    /// <summary>
    /// Updates Progress Text on the Form
    /// Call only with .Invoke to make it actually happen!
    /// </summary>
    /// <returns>void</returns>
    private void SetProgressText()
    {
        lblProgressText.Text = FProgressText;
    }

    /// <summary>
    /// Show MessageBox which is in front of the Splash Screen
    /// Call only with .Invoke to make it happen!
    /// </summary>
    /// <returns>void</returns>
    private void ShowMessageBox()
    {
        if (FMessageBoxButtons == MessageBoxButtons.OK)
        {
            /*
             * By passing in 'this' as the first Argument, the MessageBox is displayed
             * in front of the Splash Screen, which is the Top Window!
             */
            MessageBox.Show(this, FMessageBoxMessage, FMessageBoxTitle, MessageBoxButtons.OK, FMessageBoxIcon);
        }
        else
        {
            /*
             * By passing in 'this' as the first Argument, the MessageBox is displayed
             * in front of the Splash Screen, which is the Top Window!
             */
            FMessageBoxDialogResult = MessageBox.Show(this,
                FMessageBoxMessage,
                FMessageBoxTitle,
                FMessageBoxButtons,
                FMessageBoxIcon,
                FMessageBoxDefaultButton);
        }
    }

    /// <summary>
    /// Updates Progress Text on the Form
    /// </summary>
    /// <returns>void</returns>
    private void SetProgressText(string AProgressText)
    {
        FProgressText = AProgressText;

        if (lblCustomText.InvokeRequired)
        {
            lblCustomText.Invoke(new TMyUpdateDelegate(SetProgressText));
        }
        else
        {
            SetProgressText();
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Updates Texts on the Form
    /// </summary>
    /// <returns>void</returns>
    public void SetTexts(string APetraVersion, string AInstallationKind, string ACustomText)
    {
        FPetraVersion = APetraVersion;
        FInstallationKind = AInstallationKind;
        FCustomText = ACustomText;

        if (lblCustomText.InvokeRequired)
        {
            lblCustomText.Invoke(new TMyUpdateDelegate(SetTexts));
        }
        else
        {
            SetTexts();
        }
    }

    #region MessageBox Helper

    /// <summary>
    /// Show MessageBox which is in front of the Splash Screen
    /// </summary>
    /// <returns>void</returns>
    public void ShowMessageBox(string AMessage)
    {
        ShowMessageBox(AMessage, "");
    }

    /// <summary>
    /// Show MessageBox which is in front of the Splash Screen
    /// </summary>
    /// <returns>void</returns>
    public void ShowMessageBox(string AMessage, string ATitle)
    {
        ShowMessageBox(AMessage, ATitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Show MessageBox which is in front of the Splash Screen
    /// </summary>
    /// <returns>void</returns>
    public void ShowMessageBox(string AMessage, string ATitle, MessageBoxIcon AMessageBoxIcon)
    {
        ShowMessageBox(AMessage, ATitle, MessageBoxButtons.OK, AMessageBoxIcon);
    }

    /// <summary>
    /// Show MessageBox which is in front of the Splash Screen
    /// </summary>
    /// <returns>void</returns>
    public void ShowMessageBox(string AMessage, string ATitle, MessageBoxButtons AMessageBoxButtons)
    {
        ShowMessageBox(AMessage, ATitle, AMessageBoxButtons, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Show MessageBox which is in front of the Splash Screen
    /// </summary>
    /// <returns>void</returns>
    public void ShowMessageBox(string AMessage, string ATitle,
        MessageBoxButtons AMessageBoxButtons, MessageBoxIcon AMessageBoxIcon)
    {
        ShowMessageBox(AMessage, ATitle, AMessageBoxButtons, AMessageBoxIcon, MessageBoxDefaultButton.Button1);
    }

    /// <summary>
    /// todoComment
    /// </summary>
    /// <param name="AMessage"></param>
    /// <param name="ATitle"></param>
    /// <param name="AMessageBoxButtons"></param>
    /// <param name="AMessageBoxIcon"></param>
    /// <param name="AMessageBoxDefaultButton"></param>
    public void ShowMessageBox(string AMessage, string ATitle,
        MessageBoxButtons AMessageBoxButtons, MessageBoxIcon AMessageBoxIcon,
        MessageBoxDefaultButton AMessageBoxDefaultButton)
    {
        FMessageBoxMessage = AMessage;
        FMessageBoxTitle = ATitle;
        FMessageBoxButtons = AMessageBoxButtons;
        FMessageBoxIcon = AMessageBoxIcon;
        FMessageBoxDefaultButton = AMessageBoxDefaultButton;

        if (lblCustomText.InvokeRequired)
        {
            lblCustomText.Invoke(new TMyUpdateDelegate(ShowMessageBox));
        }
        else
        {
            ShowMessageBox();
        }
    }

    #endregion

    #endregion

    private void FrmSplashScreenLoad(object sender, EventArgs e)
    {
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    }

    private void FrmSplashScreenClick(object sender, EventArgs e)
    {
        // this should be helpful in case the client startup stops for any reason,
        // or a message box is displayed in the wrong place during startup
        CloseRequested = true;
    }

    private bool CloseRequested = false;

    /// <summary>
    /// we cannot call .Close() from another thread
    /// otherwise System.InvalidOperationException;
    /// use a variable and a timer instead
    /// </summary>
    public void PleaseClose()
    {
        CloseRequested = true;
    }

    private void timerCloseTick(object sender, EventArgs e)
    {
        if (CloseRequested)
        {
            timerClose.Stop();
            Close();
        }
    }
}
}