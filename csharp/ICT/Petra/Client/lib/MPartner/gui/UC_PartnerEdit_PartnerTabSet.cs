// auto generated with nant generateWinforms from UC_PartnerEdit_PartnerTabSet.yaml and template usercontrolUnbound
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2011 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{

  /// auto generated user control
  public partial class TUC_PartnerEdit_PartnerTabSet: System.Windows.Forms.UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS FMainDS;

    private SortedList<TDynamicLoadableUserControls, UserControl> FTabSetup;
    private event TTabPageEventHandler FTabPageEvent;
    private Ict.Petra.Client.MCommon.TUCPartnerAddresses FUcoAddresses;
    private Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Family FUcoPartnerDetailsFamily;
    private Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Person FUcoPartnerDetailsPerson;
    private Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Bank FUcoPartnerDetailsBank;
    private Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Church FUcoPartnerDetailsChurch;
    private Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Unit FUcoPartnerDetailsUnit;
    private Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Organisation FUcoPartnerDetailsOrganisation;
    private Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Venue FUcoPartnerDetailsVenue;
    private Ict.Petra.Client.MPartner.Gui.TUCPartnerTypes FUcoPartnerTypes;
    private Ict.Petra.Client.MPartner.Gui.TUC_PartnerNotes FUcoNotes;

    /// constructor
    public TUC_PartnerEdit_PartnerTabSet() : base()
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
        get
        {
            return FPetraUtilsObject;
        }

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

    /// to avoid warning CS0067: unused event
    private void OnDataLoadingStarted(object sender, EventArgs e)
    {
        if (DataLoadingStarted != null)
        {
            DataLoadingStarted(sender, e);
        }
    }

    /// to avoid warning CS0067: unused event
    private void OnDataLoadingFinished(object sender, EventArgs e)
    {
        if (DataLoadingFinished != null)
        {
            DataLoadingFinished(sender, e);
        }
    }

    /// <summary>
    /// Enumeration of dynamic loadable UserControls which are used
    /// on the Tabs of a TabControl. AUTO-GENERATED, don't modify by hand!
    /// </summary>
    public enum TDynamicLoadableUserControls
    {
        ///<summary>Denotes dynamic loadable UserControl FUcoAddresses</summary>
        dlucAddresses,
        ///<summary>Denotes dynamic loadable UserControl FUcoPartnerDetailsFamily</summary>
        dlucPartnerDetailsFamily,
        ///<summary>Denotes dynamic loadable UserControl FUcoPartnerDetailsPerson</summary>
        dlucPartnerDetailsPerson,
        ///<summary>Denotes dynamic loadable UserControl FUcoPartnerDetailsBank</summary>
        dlucPartnerDetailsBank,
        ///<summary>Denotes dynamic loadable UserControl FUcoPartnerDetailsChurch</summary>
        dlucPartnerDetailsChurch,
        ///<summary>Denotes dynamic loadable UserControl FUcoPartnerDetailsUnit</summary>
        dlucPartnerDetailsUnit,
        ///<summary>Denotes dynamic loadable UserControl FUcoPartnerDetailsOrganisation</summary>
        dlucPartnerDetailsOrganisation,
        ///<summary>Denotes dynamic loadable UserControl FUcoPartnerDetailsVenue</summary>
        dlucPartnerDetailsVenue,
        ///<summary>Denotes dynamic loadable UserControl FUcoPartnerTypes</summary>
        dlucPartnerTypes,
        ///<summary>Denotes dynamic loadable UserControl FUcoNotes</summary>
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
    private void OnTabPageEvent(TTabPageEventArgs e)
    {
        if (FTabPageEvent != null)
        {
            FTabPageEvent(this, e);
        }
    }

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

    /// <summary>
    /// Dynamically loads UserControls that are associated with the Tabs. AUTO-GENERATED, don't modify by hand!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TabSelectionChanged(System.Object sender, EventArgs e)
    {
        bool FirstTabPageSelectionChanged = false;
        //MessageBox.Show("TabSelectionChanged. Current Tab: " + tabPartners.SelectedTab.ToString());

        if (FTabSetup == null)
        {
            FTabSetup = new SortedList<TDynamicLoadableUserControls, UserControl>();
            FirstTabPageSelectionChanged = true;
        }

        if (FirstTabPageSelectionChanged)
        {
            // The first time we run this Method we exit straight away!
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
                SetupUserControlAddresses();
            }
            else
            {
                OnTabPageEvent(new TTabPageEventArgs(tpgAddresses, FUcoAddresses, "SubsequentActivation"));

                /*
                 * The following command seems strange and unnecessary; however, it is necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                 */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    FUcoAddresses.AdjustAfterResizing();
                }
            }
        }
        if (tabPartners.SelectedTab == tpgPartnerDetails)
        {
            if (!FTabSetup.ContainsKey(GetPartnerDetailsVariableUC()))
            {
                SetupVariableUserControlForTabPagePartnerDetails();
            }
            else
            {
                if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsFamily)
                {
                    OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsFamily, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPartnerDetailsFamily.AdjustAfterResizing();
                    }
                }
                else
                if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsPerson)
                {
                    OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsPerson, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPartnerDetailsPerson.AdjustAfterResizing();
                    }
                }
                else
                if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsBank)
                {
                    OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsBank, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPartnerDetailsBank.AdjustAfterResizing();
                    }
                }
                else
                if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsChurch)
                {
                    OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsChurch, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPartnerDetailsChurch.AdjustAfterResizing();
                    }
                }
                else
                if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsUnit)
                {
                    OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsUnit, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPartnerDetailsUnit.AdjustAfterResizing();
                    }
                }
                else
                if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation)
                {
                    OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsOrganisation, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPartnerDetailsOrganisation.AdjustAfterResizing();
                    }
                }
                else
                if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsVenue)
                {
                    OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsVenue, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPartnerDetailsVenue.AdjustAfterResizing();
                    }
                }
            }
        }
        if (tabPartners.SelectedTab == tpgPartnerTypes)
        {
            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerTypes))
            {
                if (TClientSettings.DelayedDataLoading)
                {
                    // Signalise the user that data is beeing loaded
                    this.Cursor = Cursors.AppStarting;
                }

                FUcoPartnerTypes = (Ict.Petra.Client.MPartner.Gui.TUCPartnerTypes)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerTypes);
                FUcoPartnerTypes.MainDS = FMainDS;
                FUcoPartnerTypes.PetraUtilsObject = FPetraUtilsObject;
                FUcoPartnerTypes.InitUserControl();
                ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerTypes);

                OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                this.Cursor = Cursors.Default;
            }
            else
            {
                OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                /*
                 * The following command seems strange and unnecessary; however, it is necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                 */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    FUcoPartnerTypes.AdjustAfterResizing();
                }
            }
        }
        if (tabPartners.SelectedTab == tpgNotes)
        {
            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucNotes))
            {
                if (TClientSettings.DelayedDataLoading)
                {
                    // Signalise the user that data is beeing loaded
                    this.Cursor = Cursors.AppStarting;
                }

                FUcoNotes = (Ict.Petra.Client.MPartner.Gui.TUC_PartnerNotes)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucNotes);
                FUcoNotes.MainDS = FMainDS;
                FUcoNotes.PetraUtilsObject = FPetraUtilsObject;
                FUcoNotes.InitUserControl();
                ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoNotes);

                OnTabPageEvent(new TTabPageEventArgs(tpgNotes, FUcoNotes, "InitialActivation"));

                this.Cursor = Cursors.Default;
            }
            else
            {
                OnTabPageEvent(new TTabPageEventArgs(tpgNotes, FUcoNotes, "SubsequentActivation"));

                /*
                 * The following command seems strange and unnecessary; however, it is necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                 */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    FUcoNotes.AdjustAfterResizing();
                }
            }
        }

        /*
         * Raise the following Event to inform the base Form that we have finished loading fresh data.
         * We need to turn the ChangeDetection routine back on.
         */
        OnDataLoadingFinished();
    }
    /// <summary>
    /// Sets up dynamically loaded UserControl 'FUcoAddresses'.
    /// AUTO-GENERATED, don't modify by hand!
    /// </summary>
    private void SetupUserControlAddresses()
    {
        if (TClientSettings.DelayedDataLoading)
        {
            // Signalise the user that data is beeing loaded
            this.Cursor = Cursors.AppStarting;
        }

        FUcoAddresses = (Ict.Petra.Client.MCommon.TUCPartnerAddresses)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);
        FUcoAddresses.MainDS = FMainDS;
        FUcoAddresses.PetraUtilsObject = FPetraUtilsObject;
        FUcoAddresses.InitUserControl();
        ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoAddresses);

        OnTabPageEvent(new TTabPageEventArgs(tpgAddresses, FUcoAddresses, "InitialActivation"));

        this.Cursor = Cursors.Default;
    }
    /// <summary>
    /// Sets up dynamically loaded TabPage 'tpgPartnerDetails' with varying UserControls.
    /// AUTO-GENERATED, don't modify by hand!
    /// </summary>
    private void SetupVariableUserControlForTabPagePartnerDetails()
    {
        if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsFamily)
        {
            if (TClientSettings.DelayedDataLoading)
            {
                // Signalise the user that data is beeing loaded
                this.Cursor = Cursors.AppStarting;
            }

            FUcoPartnerDetailsFamily = (Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Family)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerDetailsFamily);
            FUcoPartnerDetailsFamily.MainDS = FMainDS;
            FUcoPartnerDetailsFamily.PetraUtilsObject = FPetraUtilsObject;
            FUcoPartnerDetailsFamily.InitUserControl();
            ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerDetailsFamily);

            OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsFamily, "InitialActivation"));

            this.Cursor = Cursors.Default;
        }
        else
        if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsPerson)
        {
            if (TClientSettings.DelayedDataLoading)
            {
                // Signalise the user that data is beeing loaded
                this.Cursor = Cursors.AppStarting;
            }

            FUcoPartnerDetailsPerson = (Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Person)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerDetailsPerson);
            FUcoPartnerDetailsPerson.MainDS = FMainDS;
            FUcoPartnerDetailsPerson.PetraUtilsObject = FPetraUtilsObject;
            FUcoPartnerDetailsPerson.InitUserControl();
            ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerDetailsPerson);

            OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsPerson, "InitialActivation"));

            this.Cursor = Cursors.Default;
        }
        else
        if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsBank)
        {
            if (TClientSettings.DelayedDataLoading)
            {
                // Signalise the user that data is beeing loaded
                this.Cursor = Cursors.AppStarting;
            }

            FUcoPartnerDetailsBank = (Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Bank)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerDetailsBank);
            FUcoPartnerDetailsBank.MainDS = FMainDS;
            FUcoPartnerDetailsBank.PetraUtilsObject = FPetraUtilsObject;
            FUcoPartnerDetailsBank.InitUserControl();
            ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerDetailsBank);

            OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsBank, "InitialActivation"));

            this.Cursor = Cursors.Default;
        }
        else
        if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsChurch)
        {
            if (TClientSettings.DelayedDataLoading)
            {
                // Signalise the user that data is beeing loaded
                this.Cursor = Cursors.AppStarting;
            }

            FUcoPartnerDetailsChurch = (Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Church)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerDetailsChurch);
            FUcoPartnerDetailsChurch.MainDS = FMainDS;
            FUcoPartnerDetailsChurch.PetraUtilsObject = FPetraUtilsObject;
            FUcoPartnerDetailsChurch.InitUserControl();
            ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerDetailsChurch);

            OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsChurch, "InitialActivation"));

            this.Cursor = Cursors.Default;
        }
        else
        if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsUnit)
        {
            if (TClientSettings.DelayedDataLoading)
            {
                // Signalise the user that data is beeing loaded
                this.Cursor = Cursors.AppStarting;
            }

            FUcoPartnerDetailsUnit = (Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Unit)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerDetailsUnit);
            FUcoPartnerDetailsUnit.MainDS = FMainDS;
            FUcoPartnerDetailsUnit.PetraUtilsObject = FPetraUtilsObject;
            FUcoPartnerDetailsUnit.InitUserControl();
            ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerDetailsUnit);

            OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsUnit, "InitialActivation"));

            this.Cursor = Cursors.Default;
        }
        else
        if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation)
        {
            if (TClientSettings.DelayedDataLoading)
            {
                // Signalise the user that data is beeing loaded
                this.Cursor = Cursors.AppStarting;
            }

            FUcoPartnerDetailsOrganisation = (Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Organisation)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation);
            FUcoPartnerDetailsOrganisation.MainDS = FMainDS;
            FUcoPartnerDetailsOrganisation.PetraUtilsObject = FPetraUtilsObject;
            FUcoPartnerDetailsOrganisation.InitUserControl();
            ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerDetailsOrganisation);

            OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsOrganisation, "InitialActivation"));

            this.Cursor = Cursors.Default;
        }
        else
        if(GetPartnerDetailsVariableUC() == TDynamicLoadableUserControls.dlucPartnerDetailsVenue)
        {
            if (TClientSettings.DelayedDataLoading)
            {
                // Signalise the user that data is beeing loaded
                this.Cursor = Cursors.AppStarting;
            }

            FUcoPartnerDetailsVenue = (Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Venue)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerDetailsVenue);
            FUcoPartnerDetailsVenue.MainDS = FMainDS;
            FUcoPartnerDetailsVenue.PetraUtilsObject = FPetraUtilsObject;
            FUcoPartnerDetailsVenue.InitUserControl();
            ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerDetailsVenue);

            OnTabPageEvent(new TTabPageEventArgs(tpgPartnerDetails, FUcoPartnerDetailsVenue, "InitialActivation"));

            this.Cursor = Cursors.Default;
        }
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
            case TDynamicLoadableUserControls.dlucPartnerDetailsFamily:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCPartnerDetailsFamily = new Panel();
                pnlHostForUCPartnerDetailsFamily.AutoSize = true;
                pnlHostForUCPartnerDetailsFamily.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCPartnerDetailsFamily.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCPartnerDetailsFamily.Padding = new System.Windows.Forms.Padding(2);
                tpgPartnerDetails.Controls.Add(pnlHostForUCPartnerDetailsFamily);

                // Create the UserControl
                Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Family ucoPartnerDetailsFamily = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Family();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucPartnerDetailsFamily, ucoPartnerDetailsFamily);
                ucoPartnerDetailsFamily.Location = new Point(0, 2);
                ucoPartnerDetailsFamily.Dock = DockStyle.Fill;
                pnlHostForUCPartnerDetailsFamily.Controls.Add(ucoPartnerDetailsFamily);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                 */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCPartnerDetailsFamily.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCPartnerDetailsFamily.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoPartnerDetailsFamily;
                break;
            case TDynamicLoadableUserControls.dlucPartnerDetailsPerson:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCPartnerDetailsPerson = new Panel();
                pnlHostForUCPartnerDetailsPerson.AutoSize = true;
                pnlHostForUCPartnerDetailsPerson.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCPartnerDetailsPerson.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCPartnerDetailsPerson.Padding = new System.Windows.Forms.Padding(2);
                tpgPartnerDetails.Controls.Add(pnlHostForUCPartnerDetailsPerson);

                // Create the UserControl
                Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Person ucoPartnerDetailsPerson = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Person();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucPartnerDetailsPerson, ucoPartnerDetailsPerson);
                ucoPartnerDetailsPerson.Location = new Point(0, 2);
                ucoPartnerDetailsPerson.Dock = DockStyle.Fill;
                pnlHostForUCPartnerDetailsPerson.Controls.Add(ucoPartnerDetailsPerson);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                 */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCPartnerDetailsPerson.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCPartnerDetailsPerson.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoPartnerDetailsPerson;
                break;
            case TDynamicLoadableUserControls.dlucPartnerDetailsBank:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCPartnerDetailsBank = new Panel();
                pnlHostForUCPartnerDetailsBank.AutoSize = true;
                pnlHostForUCPartnerDetailsBank.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCPartnerDetailsBank.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCPartnerDetailsBank.Padding = new System.Windows.Forms.Padding(2);
                tpgPartnerDetails.Controls.Add(pnlHostForUCPartnerDetailsBank);

                // Create the UserControl
                Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Bank ucoPartnerDetailsBank = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Bank();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucPartnerDetailsBank, ucoPartnerDetailsBank);
                ucoPartnerDetailsBank.Location = new Point(0, 2);
                ucoPartnerDetailsBank.Dock = DockStyle.Fill;
                pnlHostForUCPartnerDetailsBank.Controls.Add(ucoPartnerDetailsBank);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                 */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCPartnerDetailsBank.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCPartnerDetailsBank.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoPartnerDetailsBank;
                break;
            case TDynamicLoadableUserControls.dlucPartnerDetailsChurch:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCPartnerDetailsChurch = new Panel();
                pnlHostForUCPartnerDetailsChurch.AutoSize = true;
                pnlHostForUCPartnerDetailsChurch.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCPartnerDetailsChurch.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCPartnerDetailsChurch.Padding = new System.Windows.Forms.Padding(2);
                tpgPartnerDetails.Controls.Add(pnlHostForUCPartnerDetailsChurch);

                // Create the UserControl
                Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Church ucoPartnerDetailsChurch = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Church();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucPartnerDetailsChurch, ucoPartnerDetailsChurch);
                ucoPartnerDetailsChurch.Location = new Point(0, 2);
                ucoPartnerDetailsChurch.Dock = DockStyle.Fill;
                pnlHostForUCPartnerDetailsChurch.Controls.Add(ucoPartnerDetailsChurch);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                 */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCPartnerDetailsChurch.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCPartnerDetailsChurch.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoPartnerDetailsChurch;
                break;
            case TDynamicLoadableUserControls.dlucPartnerDetailsUnit:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCPartnerDetailsUnit = new Panel();
                pnlHostForUCPartnerDetailsUnit.AutoSize = true;
                pnlHostForUCPartnerDetailsUnit.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCPartnerDetailsUnit.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCPartnerDetailsUnit.Padding = new System.Windows.Forms.Padding(2);
                tpgPartnerDetails.Controls.Add(pnlHostForUCPartnerDetailsUnit);

                // Create the UserControl
                Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Unit ucoPartnerDetailsUnit = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Unit();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucPartnerDetailsUnit, ucoPartnerDetailsUnit);
                ucoPartnerDetailsUnit.Location = new Point(0, 2);
                ucoPartnerDetailsUnit.Dock = DockStyle.Fill;
                pnlHostForUCPartnerDetailsUnit.Controls.Add(ucoPartnerDetailsUnit);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                 */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCPartnerDetailsUnit.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCPartnerDetailsUnit.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoPartnerDetailsUnit;
                break;
            case TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCPartnerDetailsOrganisation = new Panel();
                pnlHostForUCPartnerDetailsOrganisation.AutoSize = true;
                pnlHostForUCPartnerDetailsOrganisation.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCPartnerDetailsOrganisation.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCPartnerDetailsOrganisation.Padding = new System.Windows.Forms.Padding(2);
                tpgPartnerDetails.Controls.Add(pnlHostForUCPartnerDetailsOrganisation);

                // Create the UserControl
                Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Organisation ucoPartnerDetailsOrganisation = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Organisation();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation, ucoPartnerDetailsOrganisation);
                ucoPartnerDetailsOrganisation.Location = new Point(0, 2);
                ucoPartnerDetailsOrganisation.Dock = DockStyle.Fill;
                pnlHostForUCPartnerDetailsOrganisation.Controls.Add(ucoPartnerDetailsOrganisation);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                 */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCPartnerDetailsOrganisation.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCPartnerDetailsOrganisation.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoPartnerDetailsOrganisation;
                break;
            case TDynamicLoadableUserControls.dlucPartnerDetailsVenue:
                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                Panel pnlHostForUCPartnerDetailsVenue = new Panel();
                pnlHostForUCPartnerDetailsVenue.AutoSize = true;
                pnlHostForUCPartnerDetailsVenue.Dock = System.Windows.Forms.DockStyle.Fill;
                pnlHostForUCPartnerDetailsVenue.Location = new System.Drawing.Point(0, 0);
                pnlHostForUCPartnerDetailsVenue.Padding = new System.Windows.Forms.Padding(2);
                tpgPartnerDetails.Controls.Add(pnlHostForUCPartnerDetailsVenue);

                // Create the UserControl
                Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Venue ucoPartnerDetailsVenue = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Venue();
                FTabSetup.Add(TDynamicLoadableUserControls.dlucPartnerDetailsVenue, ucoPartnerDetailsVenue);
                ucoPartnerDetailsVenue.Location = new Point(0, 2);
                ucoPartnerDetailsVenue.Dock = DockStyle.Fill;
                pnlHostForUCPartnerDetailsVenue.Controls.Add(ucoPartnerDetailsVenue);

                /*
                 * The following four commands seem strange and unnecessary; however, they are necessary
                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                 */
                if (TClientSettings.GUIRunningOnNonStandardDPI)
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    pnlHostForUCPartnerDetailsVenue.Dock = System.Windows.Forms.DockStyle.None;
                    pnlHostForUCPartnerDetailsVenue.Dock = System.Windows.Forms.DockStyle.Fill;
                }

                ReturnValue = ucoPartnerDetailsVenue;
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
