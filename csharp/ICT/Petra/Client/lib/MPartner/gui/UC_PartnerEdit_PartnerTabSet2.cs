/* auto generated with nant generateWinforms from UC_PartnerEdit_PartnerTabSet2.yaml and template usercontrolUnbound
 *
 * DO NOT edit manually, DO NOT edit with the designer
 *
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{

  /// auto generated user control
  public partial class TUC_PartnerEdit_PartnerTabSet2: System.Windows.Forms.UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS FMainDS;

    private SortedList<TDynamicLoadableUserControls, UserControl> FTabSetup;
    private event TTabPageEventHandler FTabPageEvent;

    /// constructor
    public TUC_PartnerEdit_PartnerTabSet2() : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.tpgAddresses.Text = Catalog.GetString("Addresses ({0})");
      this.tpgPartnerDetails.Text = Catalog.GetString("Partner Details");
      this.tpgFoundationDetails.Text = Catalog.GetString("Foundation Details");
      this.tpgSubscriptions.Text = Catalog.GetString("Subscriptions ({0})");
      this.tpgPartnerTypes.Text = Catalog.GetString("Special Types ({0})");
      this.tpgFamilyMembers.Text = Catalog.GetString("Family Members");
      this.tpgNotes.Text = Catalog.GetString("Notes ({0})");
      this.tpgOfficeSpecific.Text = Catalog.GetString("Local Data");
      #endregion

    }

    /// helper object for the whole screen
    public TFrmPetraEditUtils PetraUtilsObject
    {
        set
        {
            FPetraUtilsObject = value;
        }
    }

    /// dataset for the whole screen
    public Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS MainDS
    {
        set
        {
            FMainDS = value;
        }
    }

    /// <summary>todoComment</summary>
    public event System.EventHandler DataLoadingStarted;

    /// <summary>todoComment</summary>
    public event System.EventHandler DataLoadingFinished;

    /// <summary>
    /// Enumeration of dynamic loadable UserControls which are used
    /// on the Tabs of a TabControl. AUTO-GENERATED, don't modify by hand!
    /// </summary>
    public enum TDynamicLoadableUserControls
    {
        ///<summary>Denotes dynamic loadable UserControl tpgAddresses</summary>
        dlucAddresses,
        ///<summary>Denotes dynamic loadable UserControl tpgPartnerDetails</summary>
        dlucPartnerDetails,
        ///<summary>Denotes dynamic loadable UserControl tpgPartnerTypes</summary>
        dlucPartnerTypes,
        ///<summary>Denotes dynamic loadable UserControl tpgNotes</summary>
        dlucNotes,
    }

    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
        InitializeManualCode();
        tabPartners.SelectedIndex = 0;
        TabSelectionChanged(null, null);
    }

#region Implement interface functions
    /// auto generated
    public void RunOnceOnActivation()
    {
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    public void HookupAllControls()
    {
    }

    /// auto generated
    public void HookupAllInContainer(Control container)
    {
        FPetraUtilsObject.HookupAllInContainer(container);
    }

    /// auto generated
    public bool CanClose()
    {
        return FPetraUtilsObject.CanClose();
    }

    /// auto generated
    public TFrmPetraUtils GetPetraUtilsObject()
    {
        return (TFrmPetraUtils)FPetraUtilsObject;
    }
#endregion

    private void OnDataLoadingFinished()
    {
        if (DataLoadingFinished != null)
        {
            DataLoadingFinished(this, new EventArgs());
        }
    }

    private void OnDataLoadingStarted()
    {
        if (DataLoadingStarted != null)
        {
            DataLoadingStarted(this, new EventArgs());
        }
    }

    private void OnTabPageEvent(TTabPageEventArgs e)
    {
        if (FTabPageEvent != null)
        {
            FTabPageEvent(this, e);
        }
    }

    /// <summary>
    /// Dynamically loads UserControls that are associated with the Tabs. AUTO-GENERATED, don't modify by hand!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TabSelectionChanged(System.Object sender, EventArgs e)
    {
        //MessageBox.Show("TabSelectionChanged. Current Tab: " + tabPartners.SelectedTab.ToString());

        if (FTabSetup == null)
	    {
		    FTabSetup = new SortedList<TDynamicLoadableUserControls, UserControl>();

            // The first time we run this Method we exit straight away; this is when the Form gets initialised
            return;
	    }

        /*
         * Raise the following Event to inform the base Form that we might be loading some fresh data.
         * We need to bypass the ChangeDetection routine while this happens.
         */
        OnDataLoadingStarted();

        if (tabPartners.SelectedTab == tpgAddresses)
        {
            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucAddresses))
            {
                Ict.Petra.Client.MCommon.TUCPartnerAddresses UCAddresses;

                if (TClientSettings.DelayedDataLoading)
                {
                    // Signalise the user that data is beeing loaded
                    this.Cursor = Cursors.AppStarting;
                }

                UCAddresses = (Ict.Petra.Client.MCommon.TUCPartnerAddresses)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);
                UCAddresses.MainDS = FMainDS;
                UCAddresses.PetraUtilsObject = FPetraUtilsObject;
                UCAddresses.InitUserControl();
                ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(UCAddresses);

                OnTabPageEvent(new TTabPageEventArgs(tpgAddresses, UCAddresses, "FurtherInit"));

                this.Cursor = Cursors.Default;
            }
        }

        if (tabPartners.SelectedTab == tpgPartnerDetails)
        {
            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetails))
            {
                Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Family2 UCPartnerDetails;

                if (TClientSettings.DelayedDataLoading)
                {
                    // Signalise the user that data is beeing loaded
                    this.Cursor = Cursors.AppStarting;
                }

                UCPartnerDetails = (Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Family2)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerDetails);
                UCPartnerDetails.MainDS = FMainDS;
                UCPartnerDetails.PetraUtilsObject = FPetraUtilsObject;
                UCPartnerDetails.InitUserControl();
                ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(UCPartnerDetails);

                OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, UCPartnerDetails, "FurtherInit"));

                this.Cursor = Cursors.Default;
            }
        }

        if (tabPartners.SelectedTab == tpgPartnerTypes)
        {
            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerTypes))
            {
                Ict.Petra.Client.MPartner.Gui.TUCPartnerTypes UCPartnerTypes;

                if (TClientSettings.DelayedDataLoading)
                {
                    // Signalise the user that data is beeing loaded
                    this.Cursor = Cursors.AppStarting;
                }

                UCPartnerTypes = (Ict.Petra.Client.MPartner.Gui.TUCPartnerTypes)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerTypes);
                UCPartnerTypes.MainDS = FMainDS;
                UCPartnerTypes.PetraUtilsObject = FPetraUtilsObject;
                UCPartnerTypes.InitUserControl();
                ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(UCPartnerTypes);

                OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, UCPartnerTypes, "FurtherInit"));

                this.Cursor = Cursors.Default;
            }
        }

        if (tabPartners.SelectedTab == tpgNotes)
        {
            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucNotes))
            {
                Ict.Petra.Client.MPartner.Gui.TUC_PartnerNotes UCNotes;

                if (TClientSettings.DelayedDataLoading)
                {
                    // Signalise the user that data is beeing loaded
                    this.Cursor = Cursors.AppStarting;
                }

                UCNotes = (Ict.Petra.Client.MPartner.Gui.TUC_PartnerNotes)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucNotes);
                UCNotes.MainDS = FMainDS;
                UCNotes.PetraUtilsObject = FPetraUtilsObject;
                UCNotes.InitUserControl();
                ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(UCNotes);

                OnTabPageEvent(new TTabPageEventArgs(tpgNotes, UCNotes, "FurtherInit"));

                this.Cursor = Cursors.Default;
            }
        }

        /*
         * Raise the following Event to inform the base Form that we have finished loading fresh data.
         * We need to turn the ChangeDetection routine back on.
         */
        OnDataLoadingFinished();
    }

    /// <summary>
    /// Creates UserControls on request. AUTO-GENERATED, don't modify by hand!
    /// </summary>
    /// <param name="AUserControl">UserControl to load.</param>
    private UserControl DynamicLoadUserControl(TDynamicLoadableUserControls AUserControl)
    {
        UserControl ReturnValue = null;

        switch (AUserControl)
        {
            case TDynamicLoadableUserControls.dlucAddresses:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCAddresses = new Panel();
                pnlHostForUCAddresses.AutoSize = true;
                pnlHostForUCAddresses.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCAddresses.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCAddresses.Padding = new System.Windows.Forms.Padding(2);
                tpgAddresses.Controls.Add(pnlHostForUCAddresses);

                // Create the UserControl
                Ict.Petra.Client.MCommon.TUCPartnerAddresses ucoAddresses = new Ict.Petra.Client.MCommon.TUCPartnerAddresses();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucAddresses, ucoAddresses);
                ucoAddresses.Location = new Point(0, 2);
                ucoAddresses.Dock = DockStyle.Fill;
                pnlHostForUCAddresses.Controls.Add(ucoAddresses);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCAddresses.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCAddresses.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoAddresses;
                break;

            case TDynamicLoadableUserControls.dlucPartnerDetails:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCPartnerDetails = new Panel();
                pnlHostForUCPartnerDetails.AutoSize = true;
                pnlHostForUCPartnerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCPartnerDetails.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCPartnerDetails.Padding = new System.Windows.Forms.Padding(2);
                tpgPartnerDetails.Controls.Add(pnlHostForUCPartnerDetails);

                // Create the UserControl
                Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Family2 ucoPartnerDetails = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Family2();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucPartnerDetails, ucoPartnerDetails);
                ucoPartnerDetails.Location = new Point(0, 2);
                ucoPartnerDetails.Dock = DockStyle.Fill;
                pnlHostForUCPartnerDetails.Controls.Add(ucoPartnerDetails);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCPartnerDetails.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCPartnerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoPartnerDetails;
                break;

            case TDynamicLoadableUserControls.dlucPartnerTypes:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCPartnerTypes = new Panel();
                pnlHostForUCPartnerTypes.AutoSize = true;
                pnlHostForUCPartnerTypes.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCPartnerTypes.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCPartnerTypes.Padding = new System.Windows.Forms.Padding(2);
                tpgPartnerTypes.Controls.Add(pnlHostForUCPartnerTypes);

                // Create the UserControl
                Ict.Petra.Client.MPartner.Gui.TUCPartnerTypes ucoPartnerTypes = new Ict.Petra.Client.MPartner.Gui.TUCPartnerTypes();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucPartnerTypes, ucoPartnerTypes);
                ucoPartnerTypes.Location = new Point(0, 2);
                ucoPartnerTypes.Dock = DockStyle.Fill;
                pnlHostForUCPartnerTypes.Controls.Add(ucoPartnerTypes);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCPartnerTypes.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCPartnerTypes.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoPartnerTypes;
                break;

            case TDynamicLoadableUserControls.dlucNotes:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCNotes = new Panel();
                pnlHostForUCNotes.AutoSize = true;
                pnlHostForUCNotes.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCNotes.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCNotes.Padding = new System.Windows.Forms.Padding(2);
                tpgNotes.Controls.Add(pnlHostForUCNotes);

                // Create the UserControl
                Ict.Petra.Client.MPartner.Gui.TUC_PartnerNotes ucoNotes = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerNotes();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucNotes, ucoNotes);
                ucoNotes.Location = new Point(0, 2);
                ucoNotes.Dock = DockStyle.Fill;
                pnlHostForUCNotes.Controls.Add(ucoNotes);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCNotes.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCNotes.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoNotes;
                break;
        }

        return ReturnValue;
    }

  }
}
