/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using System.Net.Sockets;
using System.Resources;
using System.Runtime.Remoting.Lifetime;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client;
using System.IO;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using Ict.Common.Data;
using Ict.Petra.Shared.RemotedExceptions;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// Partner Edit screen.
    ///
    ///   Features:
    ///   - the top part of the screen is collapsible (useful with 800x600
    ///     resolution)
    ///   - the lower part uses large exchangable TabControls with many TabPages to
    ///     incorporate many Petra 4GL screens on different tabs
    ///   - several sections of the screen resize when the form is resized
    ///   - populating the screen with data is done with just one call to the
    ///     UIConnector and all data is returned as a result of this method
    ///     call - very low amount of data transfer is archieved through this!
    ///   - all data of all (populated) fields on the screen is held in only one
    ///     Typed DataSet, which holds several Typed DataTables. All (populated)
    ///     fields are DataBound to fields in DataTables in this DataSet
    ///   - saving data is also very easy: just submit the changed/new/deleted Rows
    ///     to the UIConnector class and see if errors are returned
    ///   - 1..n Addresses are displayed in a list control
    /// </summary>
    /// <include file='doc.extra/Ict.Petra.Client.MPartner.Gui.xml'
    /// path='doc/members/member[@name="T:Ict.Petra.Client.MPartner.TPartnerEditDSWinForm"]/*' />
    public partial class TPartnerEditDSWinForm
    {
        /// <summary>
        /// Used for enabling/disabling parts of the screen according to the
        /// Petra Module.
        /// </summary>
        private enum TModuleSwitchEnum
        {
            /// <summary>No module</summary>
            msNone,

            /// <summary>Partner Module</summary>
            msPartner,

            /// <summary>Personnel Module</summary>
            msPersonnel,

            /// <summary>Finance Module</summary>
            msFinance
        }

        /// <summary>
        /// Determines the type of call to the UIConnector of the screen.
        /// </summary>
        private enum TUIConnectorType
        {
            /// <summary>Call the UIConnector with a Partner Key Argument</summary>
            uictPartnerKey,

            /// <summary>Call the UIConnector with Partner Key, Location Key and Site Key Arguments</summary>
            uictLocationKey,

            /// <summary>Call the UIConnector without any Arguments, thus signalising that a new Partner should be created</summary>
            uictNewPartner
        }

        private const String StrScreenCaption = "Partner Edit";
        private const String StrQueryUnitParent = "All 'Units' MUST be assigned a 'Parent'." + "\r\n" + "Do you wish to assign one now?";
        private const String StrQueryUnitParentTitle = "Assign Parent in Unit Hierarchy?";
        private const String StrQueryOverwriteAddress = "Are you sure you want to replace the current " + "address with the address" + "\r\n" +
                                                        "that you are about to find?" + "\r\n" + "\r\n" + "WARNING:" + "\r\n" +
                                                        "If you choose 'Yes', history for the currently " +
                                                        "displayed address will be lost!" + "\r\n" + "If the displayed address " +
                                                        "was valid, you should instead add a new address" + "\r\n" +
                                                        "and then enter an end (to) date for the old address.";
        private const String StrQueryOverwriteAddressTitle = "Replace Current Address?";
        private const String StrCannotDeletePartner = "Cannot delete Partner that has unsaved changes." + "\r\n" + "\r\n" +
                                                      "Either save the changes that you have made, or close this Partner Edit sc" +
                                                      "reen without saving the data and then delete the Partner from the Partner" + " Module screen.";
        private const String StrCannotDeletePartnerTitle = "Cannot Delete Partner That Has Unsaved Chan" + "ges";
        private const String StrDownloadVideoTutorialTitle = "Download Video Tutorial";
        private const String StrDownloadVideoTutoriaManuallTitle = "Manual Download of Video Tutorial";
        private const String StrVideoTutorialTitle = "Video Tutorial for Partner Edit Screen";
        private const String StrVideoTutorialNotFound = "The Video Tutorial for Partner Edit Screen cannot be found on your system." + "\r\n" +
                                                        "(Petra is looking in '{0}' for a file named '{1}')." + "\r\n" + "\r\n" +
                                                        "The video can also be download from the Internet. Choose 'Yes' to download it and view it, "
                                                        +
                                                        "\r\n" +
                                                        "or choose 'No' to not download the video.";
        private const String StrVideoTutorialLaunchFailed =
            "There was a problem launching the Video Tutorial application. (Details can be found in the log file.)";
        private const String StrVideoTutorialWebBRowserLaunched = "The web bRowser should have been launched and offer the download of the file.";
        private const String StrVideoTutorialWebBRowserLaunchFailed =
            "There was a problem launching Internet Explorer. (Details can be found in the log file.)" + "\r\n" + "\r\n" +
            "To download the Video Tutorial, launch your web bRowser and enter the following address:" + "\r\n" + "   ";
        private const String StrVideoTutorialDownloadInstructions = "Please save the file in the following folder:" + "\r\n" + "   {0}" + "\r\n" +
                                                                    "\r\n" +
                                                                    "After the download is finished: choose 'Yes' to start the Video Tutorial from that folder,"
                                                                    +
                                                                    "\r\n" +
                                                                    "or choose 'No' to return to the Partner Edit screen.";
        private const String StrDeactivatePartnerTitle = "Deactivate Partner";
        private const String StrDeactivatePartnerActionCancelled = "Deactivate Partner procedure cancel" + "led - no data was changed.";
        private const String StrDeactivatePartnerSuccess = "Deactivate Partner procedure finished suces" + "sfully.";
        private const String StrDeactivatePartnerStatusNotChanged = "Partner Status wasn't changed - it " + "was already set to '{0}'.";

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary>holds the DataSet that contains most data that is used on the screen</summary>
        private PartnerEditTDS FMainDS;

        /// <summary>used for sending information back from the PetraServer while saving</summary>
        private DataSet FResponseDS;

        /// <summary>controls whether the SaveChanges function saves the changes or just submits parameters</summary>
        private Boolean FSubmitChangesContinue;

        /// <summary>holds the PartnerKey for which the screen is opened</summary>
        private System.Int64 FPartnerKey;

        /// <summary>SiteKey of a PartnerLocation record for which the screen is opened</summary>
        private Int64 FSiteKeyForSelectingPartnerLocation;

        /// <summary>LocationKey of a PartnerLocation record for which the screen is opened</summary>
        private Int32 FLocationKeyForSelectingPartnerLocation;

        /// <summary>The Class of the Partner that the screen is working with</summary>
        private string FPartnerClass;

        /// <summary>Tells whether the Partner that the screen is working with is a new Partner</summary>
        private Boolean FNewPartner;
        private Boolean FNewPartnerWithAutoCreatedAddress = false;
        private Int32 FNewPartnerFamilyLocationKey;
        private Int64 FNewPartnerSiteKey;
        private Int64 FNewPartnerPartnerKey;
        private Int64 FNewPartnerFamilyPartnerKey;
        private Int64 FNewPartnerFamilySiteKey;
        private String FNewPartnerPartnerClass;
        private String FNewPartnerCountryCode;
        private String FNewPartnerSiteCountryCode;
        private String FNewPartnerAcquisitionCode;
        private Boolean FNewPartnerPrivatePartner;
        private Boolean FNewPartnerShowNewPartnerDialog;
        private TPartnerEditTabPageEnum FShowTabPage;
        private TPartnerEditTabPageEnum FInitiallySelectedTabPage;
        private Boolean FUppperPartInitiallyCollapsed;
        private Boolean FFoundationDetailsEnabled;

//TODO        private Boolean FPersonnelTabSetInitialised;
//TODO        private Boolean FFinanceTabSetInitialised;
        private TModuleSwitchEnum FCurrentModuleTabGroup;

        /// <summary>Stores any Exception that occurs while the screen is loading</summary>
        public static Exception UExceptionAtLoad = null;

        /// <summary>
        /// Initializes a new instance of the TPartnerEditDSWinForm class (constructor)
        /// </summary>
        public void InitializeManualCode()
        {
            ucoPartnerTabSet.DataLoadingStarted += new System.EventHandler(this.DataLoadOperationStarting);
            ucoPartnerTabSet.DataLoadingFinished += new System.EventHandler(this.DataLoadOperationFinishing);

            FPetraUtilsObject.NoAutoHookupOfAllControls = true;

            // Initially, DON'T detect changes to the controls
            // and enable the save button. Wait until the data is loaded
            FPetraUtilsObject.SuppressChangeDetection = true;

            this.ucoUpperPart.PartnerClassMainDataChanged += new TPartnerClassMainDataChangedHandler(this.UcoUpperPart_PartnerClassMainDataChanged);
        }

        /// <summary>
        /// Set UserDefaults for LanguageCode and AcquisitionCode if those were changed
        /// or this is a new Partner record.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void MaintainUserDefaults()
        {
            if (!(FMainDS.PPartner.Rows[0].HasVersion(DataRowVersion.Original))
                || (FMainDS.PPartner.Rows[0][PPartnerTable.GetLanguageCodeDBName(), DataRowVersion.Current] !=
                    FMainDS.PPartner.Rows[0][PPartnerTable.GetLanguageCodeDBName(), DataRowVersion.Original]))
            {
                //              MessageBox.Show("Detected changed LanguageCode: " + FMainDS.PPartner[0].LanguageCode);
                TUserDefaults.SetDefault(TUserDefaults.PARTNER_LANGUAGECODE, FMainDS.PPartner[0].LanguageCode);

                // This needs to be saved instantaneously because the PetraServer will
                // read it when setting up data for a new Partner!
                TUserDefaults.SaveChangedUserDefault(TUserDefaults.PARTNER_LANGUAGECODE);
            }

            if (!(FMainDS.PPartner.Rows[0].HasVersion(DataRowVersion.Original))
                || (FMainDS.PPartner.Rows[0][PPartnerTable.GetAcquisitionCodeDBName(), DataRowVersion.Current] !=
                    FMainDS.PPartner.Rows[0][PPartnerTable.GetAcquisitionCodeDBName(), DataRowVersion.Original]))
            {
                //              MessageBox.Show("Detected changed AcquisitionCode: " + FMainDS.PPartner[0].AcquisitionCode);
                TUserDefaults.SetDefault(TUserDefaults.PARTNER_ACQUISITIONCODE, FMainDS.PPartner[0].AcquisitionCode);
            }
        }

        private void MniEditFind_Click(System.Object sender, System.EventArgs e)
        {
// TODO edit find click
#if TODO
            TCmdMPartner CmdMPartner;

            CmdMPartner = new TCmdMPartner();
            CmdMPartner.OpenScreenPartnerFind(this);
#endif
        }

        private void FileSave(System.Object sender, System.EventArgs e)
        {
            SaveChanges(ref FMainDS);
        }

        private void MniFileCopyPartnerKey_Click(System.Object sender, System.EventArgs e)
        {
            Clipboard.SetDataObject(FPartnerKey.ToString());
        }

        private void MniFileCopyAddress_Click(System.Object sender, System.EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorCopyAddress2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
                return;
            }

// TODO copy address
#if TODO
            TCopyPartnerAddressDialogWinForm cpad = new TCopyPartnerAddressDialogWinForm();

            PLocationRow LocationRow = ucoPartnerTabSet.Get_LocationRowOfCurrentlySelectedAddress();

            cpad.SetParameters(FPartnerKey, LocationRow.SiteKey, LocationRow.LocationKey);
            cpad.ShowDialog();
            cpad.Dispose();
#endif
        }

        private void MniFilePrint_Click(System.Object sender, System.EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorPrintPartner2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
                return;
            }

#if TODO
            cmd = new TCmdMPartner();
            cmd.RunPrintPartner(this, FPartnerKey);
#endif
        }

        private void MniFilePrintSection_Click(System.Object sender, System.EventArgs e)
        {
#if TODO
            TPartnerPrintSectionDialog APrintSectionDialog;
            Int32 SelectedTab;

            SelectedTab = -1;
            APrintSectionDialog = new TPartnerPrintSectionDialog();

            switch (this.ucoPartnerTabSet.CurrentlySelectedTabPage)
            {
                case TPartnerEditTabPageEnum.petpSubscriptions:
                    SelectedTab = 0;
                    break;

                case TPartnerEditTabPageEnum.petpInterests:
                    SelectedTab = 1;
                    break;

                case TPartnerEditTabPageEnum.petpContacts:
                    SelectedTab = 2;
                    break;

                case TPartnerEditTabPageEnum.petpReminders:
                    SelectedTab = 3;
                    break;
            }

            // TPartnerEditTabPageEnum.petpDocuments:     SelectedTab := 4;
            // TPartnerEditTabPageEnum.petpOfficeSpecific:     SelectedTab := 5;
            // TPartnerEditTabPageEnum.petpFoundationDetails:     SelectedTab := 6;
            APrintSectionDialog.SetParameters(SelectedTab, @GetDataRowOfCurrentlySelectedRecord, FPetraUtilsObject.HasChanges, FPartnerKey);
            APrintSectionDialog.ShowDialog();
#endif
        }

        #region Public Methods

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if an existing Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens)</param>
        /// <param name="APartnerKey">PartnerKey of the Partner for which the screen should be
        /// opened</param>
        /// <overloads>
        ///   <summary>This method has many overloads to suit the many ways in which the
        ///   Partner Edit Screen can be invoked.</summary>
        ///   <remarks>There are two different kinds of situation where SetParameters will be called:
        ///   for an existing Partner, or for a new Parameter. The description of each SetParameters
        ///   overload mentions in which situation it is to be used.</remarks>
        /// </overloads>
        public void SetParameters(TScreenMode AScreenMode, System.Int64 APartnerKey)
        {
            SetParameters(AScreenMode, APartnerKey, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if an existing Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens)</param>
        /// <param name="APartnerKey">PartnerKey of the Partner for which the screen should be
        /// opened</param>
        /// <param name="AShowTabPage">The tab page that should be initially shown</param>
        public void SetParameters(TScreenMode AScreenMode, System.Int64 APartnerKey, TPartnerEditTabPageEnum AShowTabPage)
        {
            if ((AScreenMode == TScreenMode.smNew) || (AScreenMode == TScreenMode.smNewInquireAll))
            {
                throw new ArgumentException(
                    "AScreenMode parameter must not be TScreenMode.smNew or TScreenMode.smNewInquireAll if this overload is used");
            }

            FPetraUtilsObject.ScreenMode = AScreenMode;
            FPartnerKey = APartnerKey;
            FShowTabPage = AShowTabPage;
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if an existing Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens)</param>
        /// <param name="APartnerKey">PartnerKey of the Partner for which the screen should be
        /// opened</param>
        /// <param name="ASiteKey">SiteKey of a PartnerLocation record for which the
        /// screen should be opened</param>
        /// <param name="ALocationKey">LocationKey of a PartnerLocation record for which the
        /// screen should be opened</param>
        public void SetParameters(TScreenMode AScreenMode, Int64 APartnerKey, Int64 ASiteKey, Int32 ALocationKey)
        {
            SetParameters(AScreenMode, APartnerKey, ASiteKey, ALocationKey, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if an existing Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens)</param>
        /// <param name="APartnerKey">PartnerKey of the Partner for which the screen should be
        /// opened</param>
        /// <param name="ASiteKey">SiteKey of a PartnerLocation record for which the
        /// screen should be opened</param>
        /// <param name="ALocationKey">LocationKey of a PartnerLocation record for which the
        /// screen should be opened</param>
        /// <param name="AShowTabPage">The tab page that should be initially shown</param>
        public void SetParameters(TScreenMode AScreenMode,
            Int64 APartnerKey,
            Int64 ASiteKey,
            Int32 ALocationKey,
            TPartnerEditTabPageEnum AShowTabPage)
        {
            if ((AScreenMode == TScreenMode.smNew) || (AScreenMode == TScreenMode.smNewInquireAll))
            {
                throw new ArgumentException(
                    "AScreenMode parameter must not be TScreenMode.smNew or TScreenMode.smNewInquireAll if this overload is used");
            }

            FPetraUtilsObject.ScreenMode = AScreenMode;
            FPartnerKey = APartnerKey;
            FSiteKeyForSelectingPartnerLocation = ASiteKey;
            FLocationKeyForSelectingPartnerLocation = ALocationKey;
            FShowTabPage = AShowTabPage;
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if a new Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens). Must be TScreenMode.smNew if this overload is
        /// used!</param>
        /// <param name="APartnerClass">PartnerClass that the Partner should have.
        /// Default: FAMILY</param>
        /// <param name="ASiteKey">SiteKey for which the Partner should be created.
        /// Pass in -1 to use the site Petra is installed for. Default: -1</param>
        /// <param name="APartnerKey">PartnerKey that the Partner should have.
        /// Pass in -1 to automatically determine a new PartnerKey (based on the
        /// SiteKey). Default: -1</param>
        /// <param name="ACountryCode">CountryCode that should be the default for new addresses
        /// (optional, default: ''). If '' is passed in, the CountryCode that is
        /// associated with the SiteKey will be used.</param>
        /// <param name="AAcquisitionCode">AcquisitionCode that the Partner should have (optional,
        /// default: ''). If '' is passed in, the User's UserDefault setting will be
        /// used.</param>
        /// <param name="APrivatePartner">If set to true, the new Partner will be a Private
        /// Partner for the current user.</param>
        /// <param name="ANewPartnerFamilyPartnerKey">PartnerKey of the Family (only needed if
        /// new Partner is of Partner Class PERSON). If -1 is passed in, the New Partner
        /// Dialog will inquire about the FAMILY, otherwise the new PERSON's Family will
        /// have this key. Default: -1</param>
        /// <param name="ANewPartnerFamilyLocationKey">LocationKey of the desired Location of
        /// the Family (only needed if new Partner is of Partner Class PERSON). If -1 is
        /// passed in, the New Partner Dialog will inquire about the FAMILY, otherwise
        /// the new PERSON's Family will have this Location Key. Default: -1</param>
        /// <param name="ANewPartnerFamilySiteKey">SiteKey of the location that is used as a
        /// source for the new location</param>
        /// <param name="AShowNewPartnerDialog">If set to true, the New Partner Dialog will be
        /// shown. If false, the dialog will not be shown and the Partner Edit screen
        /// will be automatically setup according to the parmeters passed in.
        /// Default: true</param>
        /// <param name="AShowTabPage">The tab page that should be initially shown</param>
        public void SetParameters(TScreenMode AScreenMode,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String ACountryCode,
            String AAcquisitionCode,
            Boolean APrivatePartner,
            Int64 ANewPartnerFamilyPartnerKey,
            Int32 ANewPartnerFamilyLocationKey,
            Int64 ANewPartnerFamilySiteKey,
            Boolean AShowNewPartnerDialog,
            TPartnerEditTabPageEnum AShowTabPage)
        {
            if (AScreenMode != TScreenMode.smNew)
            {
                throw new ArgumentException("AScreenMode parameter must be TScreenMode.smNew if this overload is used");
            }

            FPetraUtilsObject.ScreenMode = AScreenMode;
            FNewPartnerSiteKey = ASiteKey;
            FNewPartnerPartnerKey = APartnerKey;
            FNewPartnerPartnerClass = APartnerClass;
            FNewPartnerCountryCode = ACountryCode;
            FNewPartnerAcquisitionCode = AAcquisitionCode;
            FNewPartnerPrivatePartner = APrivatePartner;
            FNewPartnerFamilyPartnerKey = ANewPartnerFamilyPartnerKey;
            FNewPartnerFamilyLocationKey = ANewPartnerFamilyLocationKey;
            FNewPartnerFamilySiteKey = ANewPartnerFamilySiteKey;
            FNewPartnerShowNewPartnerDialog = AShowNewPartnerDialog;
            FShowTabPage = AShowTabPage;
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if a new Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens). Must be TScreenMode.smNew if this overload is
        /// used!</param>
        /// <param name="APartnerClass">PartnerClass that the Partner should have.
        /// Default: FAMILY</param>
        /// <param name="ASiteKey">SiteKey for which the Partner should be created.
        /// Pass in -1 to use the site Petra is installed for. Default: -1</param>
        /// <param name="APartnerKey">PartnerKey that the Partner should have.
        /// Pass in -1 to automatically determine a new PartnerKey (based on the
        /// SiteKey). Default: -1</param>
        /// <param name="ACountryCode">CountryCode that should be the default for new addresses
        /// (optional, default: ''). If '' is passed in, the CountryCode that is
        /// associated with the SiteKey will be used.</param>
        /// <param name="AAcquisitionCode">AcquisitionCode that the Partner should have (optional,
        /// default: ''). If '' is passed in, the User's UserDefault setting will be
        /// used.</param>
        /// <param name="APrivatePartner">If set to true, the new Partner will be a Private
        /// Partner for the current user.</param>
        /// <param name="ANewPartnerFamilyPartnerKey">PartnerKey of the Family (only needed if
        /// new Partner is of Partner Class PERSON). If -1 is passed in, the New Partner
        /// Dialog will inquire about the FAMILY, otherwise the new PERSON's Family will
        /// have this key. Default: -1</param>
        /// <param name="ANewPartnerFamilyLocationKey">LocationKey of the desired Location of
        /// the Family (only needed if new Partner is of Partner Class PERSON). If -1 is
        /// passed in, the New Partner Dialog will inquire about the FAMILY, otherwise
        /// the new PERSON's Family will have this Location Key. Default: -1</param>
        /// <param name="ANewPartnerFamilySiteKey">SiteKey of the location that is used as a
        /// source for the new location</param>
        /// <param name="AShowNewPartnerDialog">If set to true, the New Partner Dialog will be
        /// shown. If false, the dialog will not be shown and the Partner Edit screen
        /// will be automatically setup according to the parmeters passed in.
        /// Default: true</param>
        public void SetParameters(TScreenMode AScreenMode,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String ACountryCode,
            String AAcquisitionCode,
            Boolean APrivatePartner,
            Int64 ANewPartnerFamilyPartnerKey,
            Int32 ANewPartnerFamilyLocationKey,
            Int64 ANewPartnerFamilySiteKey,
            Boolean AShowNewPartnerDialog)
        {
            SetParameters(AScreenMode,
                APartnerClass,
                ASiteKey,
                APartnerKey,
                ACountryCode,
                AAcquisitionCode,
                APrivatePartner,
                ANewPartnerFamilyPartnerKey,
                ANewPartnerFamilyLocationKey,
                ANewPartnerFamilySiteKey,
                AShowNewPartnerDialog,
                TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if a new Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens). Must be TScreenMode.smNew if this overload is
        /// used!</param>
        /// <param name="APartnerClass">PartnerClass that the Partner should have.
        /// Default: FAMILY</param>
        /// <param name="ASiteKey">SiteKey for which the Partner should be created.
        /// Pass in -1 to use the site Petra is installed for. Default: -1</param>
        /// <param name="APartnerKey">PartnerKey that the Partner should have.
        /// Pass in -1 to automatically determine a new PartnerKey (based on the
        /// SiteKey). Default: -1</param>
        /// <param name="ACountryCode">CountryCode that should be the default for new addresses
        /// (optional, default: ''). If '' is passed in, the CountryCode that is
        /// associated with the SiteKey will be used.</param>
        /// <param name="AAcquisitionCode">AcquisitionCode that the Partner should have (optional,
        /// default: ''). If '' is passed in, the User's UserDefault setting will be
        /// used.</param>
        /// <param name="APrivatePartner">If set to true, the new Partner will be a Private
        /// Partner for the current user.</param>
        /// <param name="ANewPartnerFamilyPartnerKey">PartnerKey of the Family (only needed if
        /// new Partner is of Partner Class PERSON). If -1 is passed in, the New Partner
        /// Dialog will inquire about the FAMILY, otherwise the new PERSON's Family will
        /// have this key. Default: -1</param>
        /// <param name="ANewPartnerFamilyLocationKey">LocationKey of the desired Location of
        /// the Family (only needed if new Partner is of Partner Class PERSON). If -1 is
        /// passed in, the New Partner Dialog will inquire about the FAMILY, otherwise
        /// the new PERSON's Family will have this Location Key. Default: -1</param>
        /// <param name="ANewPartnerFamilySiteKey">SiteKey of the location that is used as a
        /// source for the new location</param>
        public void SetParameters(TScreenMode AScreenMode,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String ACountryCode,
            String AAcquisitionCode,
            Boolean APrivatePartner,
            Int64 ANewPartnerFamilyPartnerKey,
            Int32 ANewPartnerFamilyLocationKey,
            Int64 ANewPartnerFamilySiteKey)
        {
            SetParameters(AScreenMode,
                APartnerClass,
                ASiteKey,
                APartnerKey,
                ACountryCode,
                AAcquisitionCode,
                APrivatePartner,
                ANewPartnerFamilyPartnerKey,
                ANewPartnerFamilyLocationKey,
                ANewPartnerFamilySiteKey,
                true,
                TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if a new Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens). Must be TScreenMode.smNew if this overload is
        /// used!</param>
        /// <param name="APartnerClass">PartnerClass that the Partner should have.
        /// Default: FAMILY</param>
        /// <param name="ASiteKey">SiteKey for which the Partner should be created.
        /// Pass in -1 to use the site Petra is installed for. Default: -1</param>
        /// <param name="APartnerKey">PartnerKey that the Partner should have.
        /// Pass in -1 to automatically determine a new PartnerKey (based on the
        /// SiteKey). Default: -1</param>
        /// <param name="ACountryCode">CountryCode that should be the default for new addresses
        /// (optional, default: ''). If '' is passed in, the CountryCode that is
        /// associated with the SiteKey will be used.</param>
        /// <param name="AAcquisitionCode">AcquisitionCode that the Partner should have (optional,
        /// default: ''). If '' is passed in, the User's UserDefault setting will be
        /// used.</param>
        /// <param name="APrivatePartner">If set to true, the new Partner will be a Private
        /// Partner for the current user.</param>
        /// <param name="ANewPartnerFamilyPartnerKey">PartnerKey of the Family (only needed if
        /// new Partner is of Partner Class PERSON). If -1 is passed in, the New Partner
        /// Dialog will inquire about the FAMILY, otherwise the new PERSON's Family will
        /// have this key. Default: -1</param>
        /// <param name="ANewPartnerFamilyLocationKey">LocationKey of the desired Location of
        /// the Family (only needed if new Partner is of Partner Class PERSON). If -1 is
        /// passed in, the New Partner Dialog will inquire about the FAMILY, otherwise
        /// the new PERSON's Family will have this Location Key. Default: -1</param>
        public void SetParameters(TScreenMode AScreenMode,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String ACountryCode,
            String AAcquisitionCode,
            Boolean APrivatePartner,
            Int64 ANewPartnerFamilyPartnerKey,
            Int32 ANewPartnerFamilyLocationKey)
        {
            SetParameters(AScreenMode,
                APartnerClass,
                ASiteKey,
                APartnerKey,
                ACountryCode,
                AAcquisitionCode,
                APrivatePartner,
                ANewPartnerFamilyPartnerKey,
                ANewPartnerFamilyLocationKey,
                -1,
                true,
                TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if a new Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens). Must be TScreenMode.smNew if this overload is
        /// used!</param>
        /// <param name="APartnerClass">PartnerClass that the Partner should have.
        /// Default: FAMILY</param>
        /// <param name="ASiteKey">SiteKey for which the Partner should be created.
        /// Pass in -1 to use the site Petra is installed for. Default: -1</param>
        /// <param name="APartnerKey">PartnerKey that the Partner should have.
        /// Pass in -1 to automatically determine a new PartnerKey (based on the
        /// SiteKey). Default: -1</param>
        /// <param name="ACountryCode">CountryCode that should be the default for new addresses
        /// (optional, default: ''). If '' is passed in, the CountryCode that is
        /// associated with the SiteKey will be used.</param>
        /// <param name="AAcquisitionCode">AcquisitionCode that the Partner should have (optional,
        /// default: ''). If '' is passed in, the User's UserDefault setting will be
        /// used.</param>
        /// <param name="APrivatePartner">If set to true, the new Partner will be a Private
        /// Partner for the current user.</param>
        /// <param name="ANewPartnerFamilyPartnerKey">PartnerKey of the Family (only needed if
        /// new Partner is of Partner Class PERSON). If -1 is passed in, the New Partner
        /// Dialog will inquire about the FAMILY, otherwise the new PERSON's Family will
        /// have this key. Default: -1</param>
        public void SetParameters(TScreenMode AScreenMode,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String ACountryCode,
            String AAcquisitionCode,
            Boolean APrivatePartner,
            Int64 ANewPartnerFamilyPartnerKey)
        {
            SetParameters(AScreenMode,
                APartnerClass,
                ASiteKey,
                APartnerKey,
                ACountryCode,
                AAcquisitionCode,
                APrivatePartner,
                ANewPartnerFamilyPartnerKey,
                -1,
                -1,
                true,
                TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if a new Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens). Must be TScreenMode.smNew if this overload is
        /// used!</param>
        /// <param name="APartnerClass">PartnerClass that the Partner should have.
        /// Default: FAMILY</param>
        /// <param name="ASiteKey">SiteKey for which the Partner should be created.
        /// Pass in -1 to use the site Petra is installed for. Default: -1</param>
        /// <param name="APartnerKey">PartnerKey that the Partner should have.
        /// Pass in -1 to automatically determine a new PartnerKey (based on the
        /// SiteKey). Default: -1</param>
        /// <param name="ACountryCode">CountryCode that should be the default for new addresses
        /// (optional, default: ''). If '' is passed in, the CountryCode that is
        /// associated with the SiteKey will be used.</param>
        /// <param name="AAcquisitionCode">AcquisitionCode that the Partner should have (optional,
        /// default: ''). If '' is passed in, the User's UserDefault setting will be
        /// used.</param>
        /// <param name="APrivatePartner">If set to true, the new Partner will be a Private
        /// Partner for the current user.</param>
        public void SetParameters(TScreenMode AScreenMode,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String ACountryCode,
            String AAcquisitionCode,
            Boolean APrivatePartner)
        {
            SetParameters(AScreenMode,
                APartnerClass,
                ASiteKey,
                APartnerKey,
                ACountryCode,
                AAcquisitionCode,
                APrivatePartner,
                -1,
                -1,
                -1,
                true,
                TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if a new Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens). Must be TScreenMode.smNew if this overload is
        /// used!</param>
        /// <param name="APartnerClass">PartnerClass that the Partner should have.
        /// Default: FAMILY</param>
        /// <param name="ASiteKey">SiteKey for which the Partner should be created.
        /// Pass in -1 to use the site Petra is installed for. Default: -1</param>
        /// <param name="APartnerKey">PartnerKey that the Partner should have.
        /// Pass in -1 to automatically determine a new PartnerKey (based on the
        /// SiteKey). Default: -1</param>
        /// <param name="ACountryCode">CountryCode that should be the default for new addresses
        /// (optional, default: ''). If '' is passed in, the CountryCode that is
        /// associated with the SiteKey will be used.</param>
        /// <param name="AAcquisitionCode">AcquisitionCode that the Partner should have (optional,
        /// default: ''). If '' is passed in, the User's UserDefault setting will be
        /// used.</param>
        public void SetParameters(TScreenMode AScreenMode,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String ACountryCode,
            String AAcquisitionCode)
        {
            SetParameters(AScreenMode,
                APartnerClass,
                ASiteKey,
                APartnerKey,
                ACountryCode,
                AAcquisitionCode,
                false,
                -1,
                -1,
                -1,
                true,
                TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if a new Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens). Must be TScreenMode.smNew if this overload is
        /// used!</param>
        /// <param name="APartnerClass">PartnerClass that the Partner should have.
        /// Default: FAMILY</param>
        /// <param name="ASiteKey">SiteKey for which the Partner should be created.
        /// Pass in -1 to use the site Petra is installed for. Default: -1</param>
        /// <param name="APartnerKey">PartnerKey that the Partner should have.
        /// Pass in -1 to automatically determine a new PartnerKey (based on the
        /// SiteKey). Default: -1</param>
        /// <param name="ACountryCode">CountryCode that should be the default for new addresses
        /// (optional, default: ''). If '' is passed in, the CountryCode that is
        /// associated with the SiteKey will be used.</param>
        public void SetParameters(TScreenMode AScreenMode, String APartnerClass, System.Int64 ASiteKey, System.Int64 APartnerKey, String ACountryCode)
        {
            SetParameters(AScreenMode,
                APartnerClass,
                ASiteKey,
                APartnerKey,
                ACountryCode,
                "",
                false,
                -1,
                -1,
                -1,
                true,
                TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if a new Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens). Must be TScreenMode.smNew if this overload is
        /// used!</param>
        /// <param name="APartnerClass">PartnerClass that the Partner should have.
        /// Default: FAMILY</param>
        /// <param name="ASiteKey">SiteKey for which the Partner should be created.
        /// Pass in -1 to use the site Petra is installed for. Default: -1</param>
        /// <param name="APartnerKey">PartnerKey that the Partner should have.
        /// Pass in -1 to automatically determine a new PartnerKey (based on the
        /// SiteKey). Default: -1</param>
        public void SetParameters(TScreenMode AScreenMode, String APartnerClass, System.Int64 ASiteKey, System.Int64 APartnerKey)
        {
            SetParameters(AScreenMode, APartnerClass, ASiteKey, APartnerKey, "", "", false, -1, -1, -1, true, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if a new Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens). Must be TScreenMode.smNew if this overload is
        /// used!</param>
        /// <param name="APartnerClass">PartnerClass that the Partner should have.
        /// Default: FAMILY</param>
        /// <param name="ASiteKey">SiteKey for which the Partner should be created.
        /// Pass in -1 to use the site Petra is installed for. Default: -1</param>
        public void SetParameters(TScreenMode AScreenMode, String APartnerClass, System.Int64 ASiteKey)
        {
            SetParameters(AScreenMode, APartnerClass, ASiteKey, -1, "", "", false, -1, -1, -1, true, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if a new Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens). Must be TScreenMode.smNew if this overload is
        /// used!</param>
        /// <param name="APartnerClass">PartnerClass that the Partner should have.
        /// Default: FAMILY</param>
        public void SetParameters(TScreenMode AScreenMode, String APartnerClass)
        {
            SetParameters(AScreenMode, APartnerClass, -1, -1, "", "", false, -1, -1, -1, true, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// Overload to be used if a new Partner should be opened.
        /// </summary>
        /// <param name="AScreenMode">Tells which mode the screen should be opened in (has
        /// the same purpose than in 4GL screens). Must be TScreenMode.smNew if this overload is
        /// used!</param>
        public void SetParameters(TScreenMode AScreenMode)
        {
            SetParameters(AScreenMode, "FAMILY", -1, -1, "", "", false, -1, -1, -1, true, TPartnerEditTabPageEnum.petpDefault);
        }

        #endregion

        #region Private Methods
        private Boolean GetPartnerEditUIConnector(TUIConnectorType AUIConnectorType)
        {
            Boolean ServerCallSuccessful;

            System.Windows.Forms.DialogResult ServerBusyDialogResult;
            ServerCallSuccessful = false;

            do
            {
                try
                {
                    switch (AUIConnectorType)
                    {
                        case TUIConnectorType.uictPartnerKey:
                            FPartnerEditUIConnector = TRemote.MPartner.Partner.UIConnectors.PartnerEdit(FPartnerKey,
                            ref FMainDS,
                            TClientSettings.DelayedDataLoading,
                            FInitiallySelectedTabPage);
                            break;

                        case TUIConnectorType.uictLocationKey:

                            // MessageBox.Show('Passed in FLocationKeyForSelectingPartnerLocation: ' + FLocationKeyForSelectingPartnerLocation.toString);
                            FPartnerEditUIConnector = TRemote.MPartner.Partner.UIConnectors.PartnerEdit(FPartnerKey,
                            FSiteKeyForSelectingPartnerLocation,
                            FLocationKeyForSelectingPartnerLocation,
                            ref FMainDS,
                            TClientSettings.DelayedDataLoading,
                            FInitiallySelectedTabPage);
                            break;

                        case TUIConnectorType.uictNewPartner:
                            FPartnerEditUIConnector = TRemote.MPartner.Partner.UIConnectors.PartnerEdit();
                            break;
                    }

                    ServerCallSuccessful = true;
                }
                catch (EDBTransactionBusyException)
                {
                    ServerBusyDialogResult =
                        MessageBox.Show(String.Format(CommonResourcestrings.StrPetraServerTooBusy, "open the " + StrScreenCaption + " screen"),
                            CommonResourcestrings.StrPetraServerTooBusyTitle,
                            MessageBoxButtons.RetryCancel,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1);

                    if (ServerBusyDialogResult == System.Windows.Forms.DialogResult.Retry)
                    {
                        // retry will happen because of the repeat block
                    }
                    else
                    {
                        // break out of repeat block; this function will return false because of that.
                        break;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            } while (!(ServerCallSuccessful));

            if (ServerCallSuccessful)
            {
                // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
                TEnsureKeepAlive.Register(FPartnerEditUIConnector);
            }

            return ServerCallSuccessful;
        }

        private void SetScreenCaption()
        {
            String CaptionPrefix = "";

            if (FNewPartner)
            {
                CaptionPrefix = TFrmPetraEditUtils.StrFormCaptionPrefixNew;
            }

            this.Text = CaptionPrefix + StrScreenCaption + " - " + ucoUpperPart.PartnerQuickInfo(true);
        }

        /// <summary>
        /// Determines whether Partner is of PartnerClass ORGANISATION and whether it is
        /// a Foundation.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void DetermineOrganisationIsFoundation()
        {
            if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.ORGANISATION))
            {
                if (FMainDS.POrganisation[0].Foundation)
                {
                    FFoundationDetailsEnabled = true;
                }
                else
                {
                    FFoundationDetailsEnabled = false;
                }
            }
        }

        private void DataLoadOperationFinishing(System.Object sender, System.EventArgs e)
        {
            FPetraUtilsObject.SuppressChangeDetection = false;
        }

        private void DataLoadOperationStarting(System.Object sender, System.EventArgs e)
        {
            FPetraUtilsObject.SuppressChangeDetection = true;
        }

        /// <summary>
        /// Verify that tap pages are only opened for the partners which use them.
        /// </summary>
        private void DetermineInitiallySelectedTabPage()
        {
            switch (FShowTabPage)
            {
                case TPartnerEditTabPageEnum.petpDefault:

                    // TODO 2 oChristianK cPartner Edit / Tabs : Introduce a User Default that can specify which TabPage is the one the User wants to see as default.
                    FShowTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    FInitiallySelectedTabPage = FShowTabPage;
                    break;

                case TPartnerEditTabPageEnum.petpFoundationDetails:

                    if (!FFoundationDetailsEnabled)
                    {
                        FShowTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    }

                    FInitiallySelectedTabPage = FShowTabPage;
                    break;

                case TPartnerEditTabPageEnum.petpFamilyMembers:

                    if (!((FPartnerClass == "PERSON")
                          || (FPartnerClass == "FAMILY")))
                    {
                        FShowTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    }

                    FInitiallySelectedTabPage = FShowTabPage;
                    break;

                case TPartnerEditTabPageEnum.petpOfficeSpecific:

                    if (!FMainDS.MiscellaneousData[0].OfficeSpecificDataLabelsAvailable)
                    {
                        FShowTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    }

                    FInitiallySelectedTabPage = FShowTabPage;
                    break;

                case TPartnerEditTabPageEnum.petpAddresses:
                case TPartnerEditTabPageEnum.petpDetails:
                case TPartnerEditTabPageEnum.petpSubscriptions:
                case TPartnerEditTabPageEnum.petpPartnerTypes:
                case TPartnerEditTabPageEnum.petpNotes:
                    FInitiallySelectedTabPage = FShowTabPage;
                    break;

#if  SHOWUNFINISHEDTABS
                case TPartnerEditTabPageEnum.petpRelationships:
                case TPartnerEditTabPageEnum.petpContacts:
                case TPartnerEditTabPageEnum.petpReminders:
                case TPartnerEditTabPageEnum.petpInterests:
                    FInitiallySelectedTabPage = FShowTabPage;
                    break;

#else
                case TPartnerEditTabPageEnum.petpRelationships:
                case TPartnerEditTabPageEnum.petpContacts:
                case TPartnerEditTabPageEnum.petpReminders:
                case TPartnerEditTabPageEnum.petpInterests:
                    FShowTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    FInitiallySelectedTabPage = FShowTabPage;
                    break;
#endif
                default:

                    // Fallback
                    ucoPartnerTabSet.InitiallySelectedTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    break;
            }
        }

        private Boolean OpenNewPartnerDialog()
        {
            Boolean ReturnValue = false;

            TPartnerNewDialogWinForm NewPartnerDialog;

            NewPartnerDialog = new TPartnerNewDialogWinForm(this.Handle);
            NewPartnerDialog.SetParameters(FPartnerEditUIConnector,
                FNewPartnerPartnerClass,
                FNewPartnerSiteKey,
                FNewPartnerPartnerKey,
                FNewPartnerAcquisitionCode,
                FNewPartnerPrivatePartner,
                FNewPartnerFamilyPartnerKey,
                FNewPartnerFamilyLocationKey,
                FNewPartnerFamilySiteKey);

            if (NewPartnerDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                // get NewPartnerDialog out of memory
                NewPartnerDialog.Dispose();

                // MessageBox.Show('NewPartnerDialog: pressed Cancel, will exit again.');
                ReturnValue = false;
            }
            else
            {
                NewPartnerDialog.GetReturnedParameters(
                    out FNewPartnerPartnerClass,
                    out FNewPartnerSiteKey,
                    out FNewPartnerPartnerKey,
                    out FNewPartnerAcquisitionCode,
                    out FNewPartnerPrivatePartner,
                    out FNewPartnerFamilyPartnerKey,
                    out FNewPartnerFamilyLocationKey,
                    out FNewPartnerFamilySiteKey);

                // MessageBox.Show('FNewPartnerPartnerClass: ' + FNewPartnerPartnerClass + "\r\n" +
                // 'FNewPartnerSiteKey: ' + FNewPartnerSiteKey.ToString + "\r\n" +
                // 'FNewPartnerPartnerKey: ' + FNewPartnerPartnerKey.ToString + "\r\n" +
                // 'FNewPartnerAcquisitionCode: ' + FNewPartnerAcquisitionCode + "\r\n" +
                // 'FNewPartnerPrivatePartner: ' + FNewPartnerPrivatePartner.ToString + "\r\n" +
                // 'FNewPartnerFamilyPartnerKey: ' + FNewPartnerFamilyPartnerKey.ToString + "\r\n" +
                // 'FNewPartnerFamilyLocationKey: ' + FNewPartnerFamilyLocationKey.ToString);
                // get NewPartnerDialog out of memory
                NewPartnerDialog.Dispose();
                Application.DoEvents();

                // MessageBox.Show('NewPartnerDialog: pressed OK, will exit again.');
                FNewPartner = true;

                // SetScreenCaption;
                ReturnValue = true;
            }

            return ReturnValue;
        }

        private void HookupDataChangeEvents()
        {
            HookupPartnerEditDataChangeEvents(TPartnerEditTabPageEnum.petpAddresses);
        }

        /// <summary>
        /// Hook events that enable the 'Save' ToolBarButton and File/Save menu entry
        ///
        /// </summary>
        /// <returns>void</returns>
        private void HookupPartnerEditDataChangeEvents(TPartnerEditTabPageEnum ATabPage)
        {
            switch (ATabPage)
            {
                case TPartnerEditTabPageEnum.petpDefault:
                    FMainDS.PPartner.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);

                    if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                    {
                        FMainDS.PPerson.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    }
                    else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
                    {
                        FMainDS.PFamily.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    }
                    else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.CHURCH))
                    {
                        FMainDS.PChurch.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    }
                    else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.ORGANISATION))
                    {
                        FMainDS.POrganisation.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    }
                    else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.UNIT))
                    {
                        FMainDS.PUnit.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    }
                    else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.BANK))
                    {
                        FMainDS.PBank.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    }
                    else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.VENUE))
                    {
                        FMainDS.PVenue.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    }

                    break;

                case TPartnerEditTabPageEnum.petpAddresses:
                    FMainDS.PLocation.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    FMainDS.PPartnerLocation.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    break;

                case TPartnerEditTabPageEnum.petpDetails:
                    break;

                case TPartnerEditTabPageEnum.petpFoundationDetails:
                    FMainDS.PFoundation.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    FMainDS.PFoundationDeadline.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    FMainDS.PFoundationProposal.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    FMainDS.PFoundationProposal.RowDeleting += new DataRowChangeEventHandler(FPetraUtilsObject.OnAnyDataRowChanging);
                    FMainDS.PFoundationProposalDetail.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    FMainDS.PFoundationProposalDetail.RowDeleting += new DataRowChangeEventHandler(FPetraUtilsObject.OnAnyDataRowChanging);
                    break;

                case TPartnerEditTabPageEnum.petpSubscriptions:
                    FMainDS.PSubscription.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    FMainDS.PSubscription.RowDeleting += new DataRowChangeEventHandler(FPetraUtilsObject.OnAnyDataRowChanging);
                    break;

                case TPartnerEditTabPageEnum.petpPartnerTypes:
                    FMainDS.PPartnerType.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    FMainDS.PPartnerType.RowDeleting += new DataRowChangeEventHandler(FPetraUtilsObject.OnAnyDataRowChanging);
                    break;

                case TPartnerEditTabPageEnum.petpFamilyMembers:
                    FMainDS.FamilyMembers.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    FMainDS.FamilyMembers.RowDeleting += new DataRowChangeEventHandler(FPetraUtilsObject.OnAnyDataRowChanging);
                    break;

                case TPartnerEditTabPageEnum.petpInterests:
                    FMainDS.PPartnerInterest.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    break;

                case TPartnerEditTabPageEnum.petpOfficeSpecific:
                    FMainDS.PDataLabelValuePartner.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    break;

                case TPartnerEditTabPageEnum.petpRelationships:
                    break;

                case TPartnerEditTabPageEnum.petpContacts:
                    break;

                case TPartnerEditTabPageEnum.petpNotes:
                    break;
            }
        }

        /// <summary>
        /// Sets Module-related Toggle Buttons in ToolBar up
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupAvailableModuleDataItems(Boolean AEnable, TModuleSwitchEnum ALockOnModule)
        {
// TODO enable disable buttons
#if TODO
            Boolean IsEnabled = false;

            // TODO 2 oChristianK cSecurity : Take security settings into consideration.
            // Partner Module Data
            if ((ALockOnModule == TModuleSwitchEnum.msNone) || (ALockOnModule == TModuleSwitchEnum.msPartner))
            {
                if (ALockOnModule == TModuleSwitchEnum.msPartner)
                {
                    IsEnabled = false;
                }
                else
                {
                    IsEnabled = AEnable;
                }

                tbbTogglePartner.Enabled = IsEnabled;
                mniViewPartnerData.Enabled = IsEnabled;

                if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                {
                    mniMaintainFamilyMembers.Enabled = IsEnabled;
                    mniMaintainWorkerField.Enabled = IsEnabled;
                    mniMaintainFamilyMembers.Text = Resourcestrings.StrFamilyMenuItemText;

                    // Exchange the 'Family Members' icon with the 'Family' icon!
                    this.XPMenuItemExtender.SetMenuGlyph(this.mniMaintainFamilyMembers, imlMenuHelper.Images[0]);
                }
                else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
                {
                    mniMaintainFamilyMembers.Enabled = IsEnabled;
                    mniMaintainWorkerField.Enabled = IsEnabled;
                    mniMaintainFamilyMembers.Text = Resourcestrings.StrFamilyMembersMenuItemText;
                }
                else
                {
                    // Following functionality is available only for PERSON and FAMILY
                    mniMaintainFamilyMembers.Enabled = false;
                    mniMaintainWorkerField.Enabled = false;
                }

                mniMaintainAddresses.Enabled = IsEnabled;
                mniEditFindNewAddress.Enabled = IsEnabled;
                mniMaintainPartnerDetails.Enabled = IsEnabled;

                if (FFoundationDetailsEnabled)
                {
                    mniMaintainFoundationDetails.Enabled = IsEnabled;
                }

                mniMaintainSubscriptions.Enabled = IsEnabled;
                mniMaintainSpecialTypes.Enabled = IsEnabled;
                mniMaintainOfficeSpecific.Enabled = IsEnabled;
                mniMaintainInterests.Enabled = IsEnabled;
                mniMaintainReminders.Enabled = IsEnabled;
                mniMaintainRelationships.Enabled = IsEnabled;
                mniMaintainContacts.Enabled = IsEnabled;
                mniMaintainNotes.Enabled = IsEnabled;
                mniMaintainFinanceDetails.Enabled = IsEnabled;
            }
            else
            {
                tbbTogglePartner.Enabled = (!IsEnabled);
                mniViewFinanceData.Enabled = (!IsEnabled);
                mniMaintainAddresses.Enabled = (!IsEnabled);
                mniEditFindNewAddress.Enabled = (!IsEnabled);
                mniMaintainPartnerDetails.Enabled = (!IsEnabled);

                if (!FFoundationDetailsEnabled)
                {
                    mniMaintainFoundationDetails.Enabled = (!IsEnabled);
                }

                mniMaintainSubscriptions.Enabled = (!IsEnabled);
                mniMaintainSpecialTypes.Enabled = (!IsEnabled);
                mniMaintainOfficeSpecific.Enabled = (!IsEnabled);
                mniMaintainInterests.Enabled = (!IsEnabled);
                mniMaintainReminders.Enabled = (!IsEnabled);
                mniMaintainFamilyMembers.Enabled = (!IsEnabled);
                mniMaintainRelationships.Enabled = (!IsEnabled);
                mniMaintainContacts.Enabled = (!IsEnabled);
                mniMaintainNotes.Enabled = (!IsEnabled);
                mniMaintainWorkerField.Enabled = (!IsEnabled);
                mniMaintainFinanceDetails.Enabled = (!IsEnabled);
            }

            // Personnel Module Data
            if ((ALockOnModule == TModuleSwitchEnum.msNone) || (ALockOnModule == TModuleSwitchEnum.msPersonnel))
            {
                if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                {
                    tbbTogglePersonnel.Enabled = AEnable;
                    mniViewPersonnelData.Enabled = AEnable;
                    mniMaintainPersonnelIndividualData.Enabled = AEnable;
                    mniMaintainPersonnelIndividualData.Text = Resourcestrings.StrPersonnelPersonMenuItemText;
                }
                else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.UNIT))
                {
                    mniMaintainPersonnelIndividualData.Enabled = AEnable;
                    mniMaintainPersonnelIndividualData.Text = Resourcestrings.StrPersonnelUnitMenuItemText;
                }
                else
                {
                    tbbTogglePersonnel.Enabled = (!AEnable);
                    mniViewPersonnelData.Enabled = (!AEnable);
                    mniMaintainPersonnelIndividualData.Enabled = (!AEnable);
                }
            }
            else
            {
                tbbTogglePersonnel.Enabled = (!AEnable);
                mniViewPersonnelData.Enabled = (!AEnable);
                mniMaintainPersonnelIndividualData.Enabled = (!AEnable);
            }

#if  ENABLEMODULESWITCHBUTTONS
#else
            tbbTogglePersonnel.Enabled = false;
            mniViewPersonnelData.Enabled = false;
#endif

            // Finance Module Data
            if ((ALockOnModule == TModuleSwitchEnum.msNone) || (ALockOnModule == TModuleSwitchEnum.msFinance))
            {
                tbbToggleFinance.Enabled = AEnable;
                mniViewFinanceData.Enabled = AEnable;
                mniMaintainDonorHistory.Enabled = AEnable;
                mniMaintainRecipientHistory.Enabled = AEnable;

                // For the moment, we want all to stay disabled since they are not functional yet...
                // mniMaintainFinanceReports.Enabled := AEnable;
                // mniMaintainBankAccounts.Enabled := AEnable;
                // mniMaintainGiftReceipting.Enabled := AEnable;
            }
            else
            {
                tbbToggleFinance.Enabled = (!AEnable);
                mniViewFinanceData.Enabled = (!AEnable);
                mniMaintainDonorHistory.Enabled = (!AEnable);
                mniMaintainRecipientHistory.Enabled = (!AEnable);

                // For the moment, we want all to stay disabled since they are not functional yet...
                // mniMaintainFinanceReports.Enabled := not AEnable;
                // mniMaintainBankAccounts.Enabled := not AEnable;
                // mniMaintainGiftReceipting.Enabled := not AEnable;
            }

            // For the moment, we want all to stay invisible since they are not functional yet...
            mniMaintainFinanceReports.Visible = false;
            mniMaintainBankAccounts.Visible = false;
            mniMaintainGiftReceipting.Visible = false;
#if  ENABLEMODULESWITCHBUTTONS
#else
            tbbToggleFinance.Enabled = false;
            mniViewFinanceData.Enabled = false;
#endif
#if SHOWMODULESWITCHBUTTONS
            tbbSeparator2.Visible = true;
            mniViewSeparator1.Visible = true;
            tbbTogglePartner.Visible = true;
            mniViewPartnerData.Visible = true;
            tbbTogglePersonnel.Visible = true;
            mniViewPersonnelData.Visible = true;
            tbbToggleFinance.Visible = true;
            mniViewFinanceData.Visible = true;
#endif
#endif
        }

        /// <summary>
        /// Determines the changes in the screen's dataset and submits them to the
        /// Server.
        ///
        /// </summary>
        /// <param name="AInspectDS">The screen's dataset
        /// </param>
        /// <returns>void</returns>
        private Boolean SaveChanges(ref PartnerEditTDS AInspectDS)
        {
            Boolean ReturnValue;
            PartnerEditTDS SubmitDS;
            TSubmitChangesResult SubmissionResult;

            System.Int32 ChangedColumns;
            String ErrorMessages;
            TVerificationResultCollection VerificationResult;
            TVerificationResult VerificationResultItem;
            System.Windows.Forms.DialogResult UnitParentAssignment;
            int RowIndex;
            int NumRows;
            Control FirstErrorControl;
            System.Object FirstErrorContext;
            Int32 MaxColumn;
#if SHOWCHANGES
            System.Int32 Counter;
            String DebugMessage;
#endif


            // TmpRowCounter: Int16;
            // TmpDebugString: String;
            // Counter: Integer;
            FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

            // Don't allow saving if user is still editing a Detail of a List
            if (FPetraUtilsObject.InDetailEditMode())
            {
                ReturnValue = false;
                return ReturnValue;
            }

            // Make sure that DataBinding writes the value of the active Control to the underlying DataSource!
            TDataBinding.EnsureDataChangesAreStored(this);
            ReturnValue = false;

            if (FPetraUtilsObject.VerificationResultCollection.Count == 0)
            {
                foreach (DataTable InspectDT in AInspectDS.Tables)
                {
                    foreach (DataRow InspectDR in InspectDT.Rows)
                    {
                        InspectDR.EndEdit();
                    }
                }

                if (FPetraUtilsObject.HasChanges)
                {
                    FPetraUtilsObject.WriteToStatusBar("Saving data...");
                    this.Cursor = Cursors.WaitCursor;

                    /* $IFDEF SHOWCHANGES MessageBox.Show('SaveChanges: AInspectDS.PLocation.Rows[0].HasVersion(DataRowVersion.Original): ' + AInspectDS.PLocation.Rows[0].HasVersion(DataRowVersion.Original).ToString + '; LocationKey: ' +
                     *AInspectDS.PLocation.Row[0].LocationKey.ToString); $ENDIF */

                    /* $IFDEF SHOWCHANGES MessageBox.Show('SaveChanges: AInspectDS.PLocation.Rows[1].HasVersion(DataRowVersion.Original): ' + AInspectDS.PLocation.Rows[1].HasVersion(DataRowVersion.Original).ToString + '; LocationKey: ' +
                     *AInspectDS.PLocation.Row[1].LocationKey.ToString); $ENDIF */
                    if (!FSubmitChangesContinue)
                    {
                        foreach (DataTable InspectDT in AInspectDS.Tables)
                        {
                            // MessageBox.Show('inspectDataTable: ' + InspectDT.ToString);
                            if ((InspectDT.TableName != PLocationTable.GetTableName()) && (InspectDT.TableName != PPartnerLocationTable.GetTableName()))
                            {
                                MaxColumn = InspectDT.Columns.Count;
                                ChangedColumns = SharedDataUtilities.AcceptChangesForUnmodifiedRows(InspectDT, MaxColumn);
#if SHOWCHANGES
                                if (ChangedColumns != 0)
                                {
                                    MessageBox.Show(InspectDT.TableName + " - changed colums: " + ChangedColumns.ToString());
                                }
#endif
                            }
                            else if (InspectDT.TableName == PLocationTable.GetTableName())
                            {
                                MaxColumn = new PLocationTable().Columns.Count;

                                // MessageBox.Show('PLocation MaxColumn: ' + MaxColumn.ToString);
                                ChangedColumns = SharedDataUtilities.AcceptChangesForUnmodifiedRows(AInspectDS.PLocation, MaxColumn, true);
#if SHOWCHANGES
                                if (ChangedColumns != 0)
                                {
                                    MessageBox.Show(PLocationTable.GetTableName() + " - changed colums: " + ChangedColumns.ToString());
                                }
#endif
                            }
                            else
                            {
                                MaxColumn = new PPartnerLocationTable().Columns.Count;

                                // MessageBox.Show('PPartnerLocation MaxColumn: ' + MaxColumn.ToString);
                                ChangedColumns = SharedDataUtilities.AcceptChangesForUnmodifiedRows(AInspectDS.PPartnerLocation, MaxColumn, true);
#if SHOWCHANGES
                                if (ChangedColumns != 0)
                                {
                                    MessageBox.Show(PPartnerLocationTable.GetTableName() + " - changed colums: " + ChangedColumns.ToString());
                                }
#endif
                            }

#if SHOWCHANGES
                            foreach (DataRow InspectDR in InspectDT.Rows)
                            {
                                DebugMessage = InspectDT.ToString();

                                if ((InspectDR.RowState == DataRowState.Modified) || (InspectDR.RowState == DataRowState.Added))
                                {
                                    ChangedColumns = 0;

                                    if (InspectDR.RowState == DataRowState.Modified)
                                    {
                                        DebugMessage = DebugMessage + " --- changed columns:" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        DebugMessage = DebugMessage + " --- inserted Row. Column contents:" + Environment.NewLine;
                                    }

                                    for (Counter = 0; Counter <= MaxColumn - 1; Counter += 1)
                                    {
                                        if ((InspectDR.RowState == DataRowState.Added) || (InspectDR != InspectDR[Counter, DataRowVersion.Current]))
                                        {
                                            ChangedColumns = ChangedColumns + 1;
                                            DebugMessage = DebugMessage + "  " + (InspectDT.Columns[Counter].ColumnName).ToString() + ": " +
                                                           InspectDR[Counter, DataRowVersion.Current].ToString() + Environment.NewLine;
                                        }
                                    }

                                    if (ChangedColumns != 0)
                                    {
                                        MessageBox.Show(DebugMessage);
                                    }
                                    else
                                    {
                                        DebugMessage = DebugMessage + "  NO changed columns.";
                                        MessageBox.Show(DebugMessage);
                                    }
                                }
                                // (inspectDataRow.RowState = DataRowState.Modified) or (inspectDataRow.RowState = DataRowState.Added)
                                else
                                {
                                    // MessageBox.Show('inspectDataRow.RowState: ' + inspectDataRow.RowState.ToString("G"));
                                    if (InspectDR.RowState == DataRowState.Deleted)
                                    {
                                        DebugMessage = DebugMessage + " --- deleted Row. Original Column[0] contents: " +
                                                       InspectDR[0, DataRowVersion.Original].ToString();
                                        MessageBox.Show(DebugMessage);
                                    }
                                }
                            }
                            // for inspectDataRow in inspectDataTable.Rows do
#endif
                        }

                        // for inspectDataTable in inspectDataSet.Tables do
                    }

                    SubmitDS = AInspectDS.GetChangesTyped(true);

                    // $IFDEF DEBUGMODE if SubmitDS = nil then MessageBox.Show('SubmitDS = nil!'); $ENDIF
                    // TLogging.Log('Before submitting data to the Server  Client DataSet: ' + SubmitDS.GetXml);
                    // Submit changes to the PETRAServer
                    try
                    {
                        if (!FSubmitChangesContinue)
                        {
                            FResponseDS = null;
                            SubmissionResult = FPartnerEditUIConnector.SubmitChanges(ref SubmitDS, ref FResponseDS, out VerificationResult);
                        }
                        else
                        {
                            SubmissionResult = FPartnerEditUIConnector.SubmitChangesContinue(out SubmitDS, ref FResponseDS, out VerificationResult);
                        }
                    }
                    catch (System.Net.Sockets.SocketException)
                    {
                        FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("The PETRA Server cannot be reached! Data cannot be saved!",
                            "No Server response",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        ReturnValue = false;
                        OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }
                    catch (ESecurityDBTableAccessDeniedException Exp)
                    {
                        FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                        this.Cursor = Cursors.Default;
                        TMessages.MsgSecurityException(Exp, this.GetType());
                        ReturnValue = false;
                        OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }
                    catch (EDBConcurrencyException Exp)
                    {
                        FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                        this.Cursor = Cursors.Default;
                        TMessages.MsgDBConcurrencyException(Exp, this.GetType());
                        ReturnValue = false;
                        OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }
                    catch (Exception exp)
                    {
                        FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                        this.Cursor = Cursors.Default;
                        TLogging.Log(
                            "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + exp.ToString(),
                            TLoggingType.ToLogfile);
                        MessageBox.Show(
                            "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine +
                            "For details see the log file: " + TLogging.GetLogFileName(),
                            "Server connection error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        ReturnValue = false;
                        OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }

                    switch (SubmissionResult)
                    {
                        case TSubmitChangesResult.scrOK:

                            // MessageBox.Show('DUMMY: ' + (SubmitDS.Tables['Locations'].Rows[0]['DUMMY']).ToString() );
                            if ((SharedTypes.PartnerClassStringToEnum(AInspectDS.PPartner[0].PartnerClass) == TPartnerClass.UNIT)
                                && (IsNewPartner(AInspectDS)))
                            {
                                /*
                                 * A new Partner of PartnerClass UNIT has been created
                                 * -- give option to assign 'Parent' in Unit Hierarchy
                                 */
                                UnitParentAssignment = MessageBox.Show(StrQueryUnitParent,
                                    StrQueryUnitParentTitle,
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button1);

                                if (UnitParentAssignment == System.Windows.Forms.DialogResult.Yes)
                                {
// TODO Unit hierarchy
#if TODO
                                    cmdPartner = new TCmdMPartner();
                                    cmdPartner.RunUnitHierarchy(this, AInspectDS.PPartner[0].PartnerKey);
#endif
                                }
                            }

                            // TLogging.Log('After submtting  returned Server DataSet: ' + SubmitDS.GetXml);
                            // Get rid of any new Addresses; they are returned back with different LocationKeys (based on a Progress Sequence)
                            ucoPartnerTabSet.CleanupAddressesBeforeMerge();

                            // MessageBox.Show('Location[0] LocationKey: ' + SubmitDS.PLocation.Row[0].LocationKey.ToString + '; PartnerLocation[0] LocationKey: ' + SubmitDS.PPartnerLocation.Row[0].LocationKey.ToString);
                            if (AInspectDS.PDataLabelValuePartner != null)
                            {
                                // Delete all added Rows in the original dataset. They will automatically
                                // be put back in with the Merge. If added Rows are deleted they will
                                // be removed from the Row collection on the server. In that case the
                                // client will not be aware of that. For modified Rows: if they are
                                // deleted on the server they will not be taken out of the collection
                                // but come back to the client as being marked as 'Deleted'. Therefore
                                // with deleting the added Rows beforehand we are making sure that
                                // the dataset on the client is synchronized with the one on the server.
                                NumRows = AInspectDS.PDataLabelValuePartner.Rows.Count;

                                for (RowIndex = NumRows - 1; RowIndex >= 0; RowIndex -= 1)
                                {
                                    DataRow InspectDR = AInspectDS.PDataLabelValuePartner.Rows[RowIndex];

                                    // delete all added Rows.
                                    if (InspectDR.RowState == DataRowState.Added)
                                    {
                                        InspectDR.Delete();
                                    }
                                }
                            }

                            // Update UserDefaults, if necessary
                            MaintainUserDefaults();

                            // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                            AInspectDS.AcceptChanges();

                            // TLogging.Log('After getting rid now of any deleted columns   Client DataSet: ' + AInspectDS.GetXml);
                            // $IFDEF DEBUGMODE
                            // if AInspectDS.Tables.Contains(PLocationTable.GetTableName()) then
                            // begin
                            // for TmpRowCounter := 0 to AInspectDS.Tables[PLocationTable.GetTableName].Rows.Count  1 do
                            // begin

                            /* TmpDebugString := TmpDebugString + PLocationTable.GetTableName() + '.Row[' + TmpRowCounter.ToString + ']: PLocationKey: ' +
                            *AInspectDS.Tables[PLocationTable.GetTableName].Rows[TmpRowCounter][PLocationTable.GetLocationKeyDBName()].ToString + '(); PSiteKey: ' +
                            *AInspectDS.Tables[PLocationTable.GetTableName].Rows[TmpRowCounter][PLocationTable.GetSiteKeyDBName()].ToString + "\r\n"(); */

                            // end;
                            // end;
                            //
                            // if AInspectDS.Tables.Contains(PPartnerLocationTable.GetTableName()) then
                            // begin
                            // TmpDebugString := TmpDebugString + "\r\n";
                            // for TmpRowCounter := 0 to AInspectDS.Tables[PPartnerLocationTable.GetTableName].Rows.Count  1 do
                            // begin

                            /* TmpDebugString := TmpDebugString + PPartnerLocationTable.GetTableName() + '.Row[' + TmpRowCounter.ToString + ']: PLocationKey: ' +
                             *AInspectDS.Tables[PPartnerLocationTable.GetTableName].Rows[TmpRowCounter][PPartnerLocationTable.GetLocationKeyDBName()].ToString + '(); PSiteKey: ' +
                             *AInspectDS.Tables[PPartnerLocationTable.GetTableName].Rows[TmpRowCounter][PPartnerLocationTable.GetSiteKeyDBName()].ToString + '(); PPartnerKey: ' +
                             *AInspectDS.Tables[PPartnerLocationTable.GetTableName].Rows[TmpRowCounter][PPartnerLocationTable.GetPartnerKeyDBName()].ToString + "\r\n"(); */

                            // end;
                            // MessageBox.Show(TmpDebugString, 'DEBUG: PLocation / PPartnerLocation local contents');
                            // end;
                            // $ENDIF
                            // Merge back with data from the Server (eg. for getting Sequence values)
                            AInspectDS.Merge(SubmitDS, false);

                            // TLogging.Log('After Merge back with data from the Server  Server DataSet: ' + SubmitDS.GetXml);
                            // TLogging.Log('After Merge back with data from the Server  Client DataSet: ' + AInspectDS.GetXml);
                            // $IFDEF DEBUGMODE
                            // if AInspectDS.Tables.Contains(PLocationTable.GetTableName()) then
                            // begin
                            // TmpDebugString := '';
                            // for TmpRowCounter := 0 to AInspectDS.Tables[PLocationTable.GetTableName].Rows.Count  1 do
                            // begin

                            /* TmpDebugString := TmpDebugString + PLocationTable.GetTableName() + '.Row[' + TmpRowCounter.ToString + ']: PLocationKey: ' +
                            *AInspectDS.Tables[PLocationTable.GetTableName].Rows[TmpRowCounter][PLocationTable.GetLocationKeyDBName()].ToString + '(); PSiteKey: ' +
                            *AInspectDS.Tables[PLocationTable.GetTableName].Rows[TmpRowCounter][PLocationTable.GetSiteKeyDBName()].ToString + "\r\n"(); */

                            // end;
                            // end;
                            //
                            // if AInspectDS.Tables.Contains(PPartnerLocationTable.GetTableName()) then
                            // begin
                            // TmpDebugString := TmpDebugString + "\r\n";
                            // for TmpRowCounter := 0 to AInspectDS.Tables[PPartnerLocationTable.GetTableName].Rows.Count  1 do
                            // begin

                            /* TmpDebugString := TmpDebugString + PPartnerLocationTable.GetTableName() + '.Row[' + TmpRowCounter.ToString + ']: PLocationKey: ' +
                             *AInspectDS.Tables[PPartnerLocationTable.GetTableName].Rows[TmpRowCounter][PPartnerLocationTable.GetLocationKeyDBName()].ToString + '(); PSiteKey: ' +
                             *AInspectDS.Tables[PPartnerLocationTable.GetTableName].Rows[TmpRowCounter][PPartnerLocationTable.GetSiteKeyDBName()].ToString + '(); PPartnerKey: ' +
                             *AInspectDS.Tables[PPartnerLocationTable.GetTableName].Rows[TmpRowCounter][PPartnerLocationTable.GetPartnerKeyDBName()].ToString + '(); RowError: ' +
                             *AInspectDS.Tables[PPartnerLocationTable.GetTableName].Rows[TmpRowCounter].RowError.ToString + "\r\n"(); */

                            // end;
                            // MessageBox.Show(TmpDebugString, 'DEBUG: PLocation / PPartnerLocation local contents');
                            // end;
                            // $ENDIF
                            ucoPartnerTabSet.RefreshAddressesAfterMerge();

                            // Call AcceptChanges so that we don't have any changed data anymore!
                            AInspectDS.AcceptChanges();

                            // TLogging.Log('After calling AcceptChanges on the Client DataSet: ' + AInspectDS.GetXml);
                            // Update UI
                            FPetraUtilsObject.WriteToStatusBar("Data successfully saved.");
                            this.Cursor = Cursors.Default;
                            EnableSave(false);

                            // If Screen Title was for a NEW Partner, remove the 'NEW' indicator
                            if (this.Text.StartsWith(TFrmPetraEditUtils.StrFormCaptionPrefixNew))
                            {
                                this.Text = this.Text.Substring(TFrmPetraEditUtils.StrFormCaptionPrefixNew.Length);
                            }

                            // We don't have unsaved changes anymore
                            FPetraUtilsObject.DisableSaveButton();
                            FSubmitChangesContinue = false;

                            // Assign PartnerKey. This is needed in case this was a new Partner before saving!
                            FPartnerKey = AInspectDS.PPartner[0].PartnerKey;
                            ReturnValue = true;
                            OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                            break;

                        case TSubmitChangesResult.scrError:
                            this.Cursor = Cursors.Default;

                            if (!(VerificationResult.Contains((System.Object) "Location Change Promotion: Information")))
                            {
                                FPetraUtilsObject.WriteToStatusBar(CommonResourcestrings.StrSavingDataErrorOccured);

                                MessageBox.Show(
                                    Messages.BuildMessageFromVerificationResult(null, VerificationResult));
                            }
                            else
                            {
                                FPetraUtilsObject.WriteToStatusBar(CommonResourcestrings.StrSavingDataCancelled);

                                VerificationResultItem = (TVerificationResult)VerificationResult.FindBy(
                                    (object)"Location Change Promotion: Information");

                                MessageBox.Show(VerificationResultItem.FResultText,
                                    VerificationResultItem.FResultTextCaption, MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            }

                            FSubmitChangesContinue = false;
                            ReturnValue = false;
                            OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                            break;

                        case TSubmitChangesResult.scrNothingToBeSaved:

                            // Update UI
                            FPetraUtilsObject.WriteToStatusBar(CommonResourcestrings.StrSavingDataNothingToSave);
                            this.Cursor = Cursors.Default;
                            EnableSave(false);

                            // We don't have unsaved changes anymore
                            FPetraUtilsObject.HasChanges = false;
                            ReturnValue = true;
                            OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                            break;

                        case TSubmitChangesResult.scrInfoNeeded:
                            this.Cursor = Cursors.Default;

                            // for Counter := 0 to FResponseDS.Tables.Count  1 do
                            // begin

                            /* MessageBox.Show('Table #' + Counter.ToString + '''s name is ' + FResponseDS.Tables[Counter].TableName + '. It has ' + FResponseDS.Tables[Counter].Rows.Count.ToString + ' Rows. Type: ' +
                             *FResponseDS.Tables[Counter].GetType().ToString); */

                            // end;
                            // if FResponseDS.Tables.Contains(EXISTINGLOCATIONPARAMETERS_TABLENAME) then
                            // begin
                            // MessageBox.Show('FResponseDS Type: ' + FResponseDS.GetType().ToString);
                            // MessageBox.Show(EXISTINGLOCATIONPARAMETERS_TABLENAME + ' Type: ' + FResponseDS.Tables[EXISTINGLOCATIONPARAMETERS_TABLENAME].GetType().ToString);
                            // MessageBox.Show('FResponseDS.Tables[' + EXISTINGLOCATIONPARAMETERS_TABLENAME + '].Rows.Count: ' + FResponseDS.Tables[EXISTINGLOCATIONPARAMETERS_TABLENAME].Rows.Count.ToString);
                            // Check if there is a Parameter Row that is not yet processed
                            // ExistingLocationParametersDV := new DataView(
                            // FResponseDS.Tables[EXISTINGLOCATIONPARAMETERS_TABLENAME],
                            // PartnerAddressAggregateTDSSimilarLocationParametersTable.GetAnswerProcessedClientSideDBName() +
                            // ' = false', '', DataViewRowState.CurrentRows);
                            //
                            // if ExistingLocationParametersDV.Count > 0 then
                            // begin
                            // MessageBox.Show('ExistingLocationParametersDV.Count: ' + ExistingLocationParametersDV.Count.ToString);
                            // MessageBox.Show('Row[0].LocationKey: ' + (ExistingLocationParametersDV[0].Row as PartnerAddressAggregateTDSSimilarLocationParametersRow).LocationKey.ToString);
                            ucoPartnerTabSet.SimilarLocationsProcessing(
                            (PartnerAddressAggregateTDSSimilarLocationParametersTable)FResponseDS.Tables[MPartnerConstants.
                                                                                                         EXISTINGLOCATIONPARAMETERS_TABLENAME]);

                            // MessageBox.Show('Reuse?: ' + (FResponseDS.Tables[EXISTINGLOCATIONPARAMETERS_TABLENAME]
                            // as PartnerAddressAggregateTDSSimilarLocationParametersTable).Row[0].AnswerReuse.ToString);
                            // end;
                            // end;
                            // if FResponseDS.Tables.Contains(ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME) then
                            // begin
                            // MessageBox.Show(ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME + ' Type: ' + FResponseDS.Tables[ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME].GetType().ToString);
                            // MessageBox.Show('FResponseDS.Tables[' + ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME + '].Rows.Count: ' + FResponseDS.Tables[ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME].Rows.Count.ToString);
                            // Check if there is a Parameter Row that is not yet processed
                            // AddressAddedOrChangedParametersDV := new DataView(
                            // FResponseDS.Tables[ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME],
                            // PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetAnswerProcessedClientSideDBName() +
                            // ' = false', '', DataViewRowState.CurrentRows);
                            //
                            // if AddressAddedOrChangedParametersDV.Count > 0 then
                            // begin
                            // MessageBox.Show('AddressAddedOrChangedParametersDV.Count: ' + AddressAddedOrChangedParametersDV.Count.ToString);
                            // MessageBox.Show('Row[0].LocationKey: ' + (AddressAddedOrChangedParametersDV[0].Row as PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow).LocationKey.ToString);
                            ucoPartnerTabSet.AddressAddedOrChangedProcessing(
                            (PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable)
                            FResponseDS.Tables[MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME],
                            (PartnerAddressAggregateTDSChangePromotionParametersTable)
                            FResponseDS.Tables[MPartnerConstants.ADDRESSCHANGEPROMOTIONPARAMETERS_TABLENAME]);

                            // end;
                            // recursive call!
                            FSubmitChangesContinue = true;
                            ReturnValue = SaveChanges(ref AInspectDS);
                            return ReturnValue;
                    }
                }
                else
                {
                    // Update UI
                    FPetraUtilsObject.WriteToStatusBar(CommonResourcestrings.StrSavingDataNothingToSave);
                    this.Cursor = Cursors.Default;
                    EnableSave(false);

                    // We don't have unsaved changes anymore
                    FPetraUtilsObject.HasChanges = false;
                    ReturnValue = true;
                    OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                }
            }
            else
            {
                // User needs to fix data validation errors before he can save data
                // TDataBinding.IterateErrorsInData(AInspectDataSet, ErrorMessages, FirstErrorControlName);
                // MessageBox.Show('FPetraUtilsObject.VerificationResultCollection.Count: ' +
                // FPetraUtilsObject.VerificationResultCollection.Count.ToString);
                FPetraUtilsObject.VerificationResultCollection.BuildScreenVerificationResultList(out ErrorMessages,
                    out FirstErrorControl,
                    out FirstErrorContext);

                // TODO 1 ochristiank cUI : Make a message library and call a method there to show verification errors.
                MessageBox.Show(
                    "Cannot save data because invalid data has not been corrected!" + Environment.NewLine + Environment.NewLine + ErrorMessages,
                    "Form contains invalid data!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // TFocusing.SetFocusOnControlInFormOrUserControl(this, FirstErrorControl.Name);
                FirstErrorControl.Focus();
                ReturnValue = false;
                OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
            }

            return ReturnValue;
        }

        /// <summary>
        /// Determines whether the current Partner was just created and has not been
        /// saved yet.
        /// </summary>
        /// <param name="AInspectDataSet">DataSet in which the check should be performed on</param>
        /// <returns>true if the currently edit partner was just created, and has not
        /// been saved yet
        /// </returns>
        private bool IsNewPartner(PartnerEditTDS AInspectDataSet)
        {
            return !AInspectDataSet.PPartner[0].HasVersion(DataRowVersion.Original);
        }

        private void ApplySecurity()
        {
            if (!CheckSecurityOKToCreateNewPartner(false))
            {
// TODO                mniFileNew.Enabled = false;
// TODO                tbbNew.Enabled = false;
            }
        }

        private Boolean CheckSecurityOKToCreateNewPartner(Boolean AShowMessage)
        {
            Boolean ReturnValue;
            ESecurityDBTableAccessDeniedException SecurityException;

            ReturnValue = false;
            SecurityException = null;

            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PPartnerTable.GetTableDBName()))
            {
                SecurityException = new ESecurityDBTableAccessDeniedException("", "create", PPartnerTable.GetTableDBName());
            }
            else if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PLocationTable.GetTableDBName()))
            {
                SecurityException = new ESecurityDBTableAccessDeniedException("", "create", PLocationTable.GetTableDBName());
            }
            else if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PPartnerLocationTable.GetTableDBName()))
            {
                SecurityException = new ESecurityDBTableAccessDeniedException("", "create", PPartnerLocationTable.GetTableDBName());
            }
            else if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PChurchTable.GetTableDBName()))
            {
                SecurityException = new ESecurityDBTableAccessDeniedException("", "create", PChurchTable.GetTableDBName());
            }
            else if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, POrganisationTable.GetTableDBName()))
            {
                SecurityException = new ESecurityDBTableAccessDeniedException("", "create", POrganisationTable.GetTableDBName());
            }
            else if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PPersonTable.GetTableDBName()))
            {
                SecurityException = new ESecurityDBTableAccessDeniedException("", "create", PPersonTable.GetTableDBName());
            }
            else if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PUnitTable.GetTableDBName()))
            {
                SecurityException = new ESecurityDBTableAccessDeniedException("", "create", PUnitTable.GetTableDBName());
            }
            else if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PFamilyTable.GetTableDBName()))
            {
                SecurityException = new ESecurityDBTableAccessDeniedException("", "create", PFamilyTable.GetTableDBName());
            }
            else if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PBankTable.GetTableDBName()))
            {
                SecurityException = new ESecurityDBTableAccessDeniedException("", "create", PBankTable.GetTableDBName());
            }
            else if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PVenueTable.GetTableDBName()))
            {
                SecurityException = new ESecurityDBTableAccessDeniedException("", "create", PVenueTable.GetTableDBName());
            }
            else
            {
                // User has access to all checked tables
                ReturnValue = true;
            }

            if ((SecurityException != null) && (AShowMessage))
            {
                TMessages.MsgSecurityException(SecurityException, this.GetType());
            }

            return ReturnValue;
        }

        private Boolean CloseFormCheck()
        {
            Boolean ReturnValue;

            System.Windows.Forms.DialogResult SaveQuestionAnswer;
            FPetraUtilsObject.CloseFormCheckRun = true;
            ReturnValue = false;

            // MessageBox.Show('FPetraUtilsObject.HasChanges: ' + FPetraUtilsObject.HasChanges.ToString);
            if (FPetraUtilsObject.HasChanges)
            {
                if (FPetraUtilsObject.InDetailEditMode())
                {
                    FPetraUtilsObject.CloseFormCheckRun = false;
                    return false;
                }

                // still unsaved data in the DataSet
                SaveQuestionAnswer = MessageBox.Show(CommonResourcestrings.StrFormHasUnsavedChanges +
                    Environment.NewLine + Environment.NewLine +
                    CommonResourcestrings.StrFormHasUnsavedChangesQuestion,
                    CommonResourcestrings.StrGenericWarning,
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                if (SaveQuestionAnswer == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        if (SaveChanges(ref FMainDS) == false)
                        {
                            // Form contains invalid data that hasn't been corrected yet
                            FPetraUtilsObject.CloseFormCheckRun = false;
                            return false;
                        }
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show("Exception occured during saving of data: " + exp.ToString());
                        FPetraUtilsObject.CloseFormCheckRun = false;
                    }

                    ReturnValue = true;
                    this.Close();
                }
                else if (SaveQuestionAnswer == System.Windows.Forms.DialogResult.No)
                {
                    FPetraUtilsObject.HasChanges = false;
                    ReturnValue = true;
                }
                else if (SaveQuestionAnswer == System.Windows.Forms.DialogResult.Cancel)
                {
                    FPetraUtilsObject.CloseFormCheckRun = false;
                    ReturnValue = false;
                }
            }
            else
            {
                ReturnValue = true;
            }

            return ReturnValue;
        }

        private void EnableDisableUpperPart(bool AEnable)
        {
            ucoUpperPart.Enabled = AEnable;
        }

        private void EnableSave(bool Enable)
        {
// TODO enablesave
#if TODO
            if ((Enable) && (ucoUpperPart.Enabled))
            {
                mniFileSave.Enabled = true;
                tbbSave.Enabled = true;
            }
            else
            {
                mniFileSave.Enabled = false;
                tbbSave.Enabled = false;
            }
#endif
        }

        /// <summary>
        /// This Procedure will get called from the SaveChanges procedure whenever a
        /// Save operation is finished (successful or unsuccesful).
        ///
        /// </summary>
        /// <param name="sender">The Object that throws this Event</param>
        /// <param name="e">Event Arguments. Success property is true if saving was successful,
        /// otherwise false.
        /// </param>
        /// <returns>void</returns>
        private void FormDataSaved(System.Object sender, TDataSavedEventArgs e)
        {
            ucoPartnerTabSet.DataSavedEventFired(e.Success);

            if (e.Success)
            {
                // disable save button again because the fired event may trigger some initial
                // data changes (e.g. new dummy records in office specific data) which trigger
                // the enabling of the save button
                EnableSave(false);
                FPetraUtilsObject.HasChanges = false;
            }
        }

        /// <summary>
        /// This Procedure will get called from the SaveChanges procedure before it
        /// actually performs any saving operation.
        ///
        /// </summary>
        /// <param name="sender">The Object that throws this Event</param>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        private void FormDataSavingStarted(System.Object sender, System.EventArgs e)
        {
            ucoPartnerTabSet.DataSavingStartedEventFired();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// procedure tbrMain_ButtonClick(sender: System.Object; e: XPMenu.XPToolBarButtonClickEventArgs);
        /// </summary>
        /// <returns>void</returns>
        private void MniEdit_Click(System.Object sender, System.EventArgs e)
        {
// TODO edit
#if TODO
            TCmdMPartner CmdMPartner;

            System.Int64 SiteKey;
            System.Int32 LocationKey;
            bool IsNewLocation;
            PLocationTable LocationDT;
            System.Windows.Forms.DialogResult OverwriteWarningResult;
            TPartnerLocationFind frmPartnerLS;

            if (sender == mniEditFindNewAddress)
            {
                IsNewLocation = (ucoPartnerTabSet.LocationBeingAdded);

                if (FPetraUtilsObject.DetailEditMode)
                {
                    if (!IsNewLocation)
                    {
                        OverwriteWarningResult = MessageBox.Show(StrQueryOverwriteAddress,
                            StrQueryOverwriteAddressTitle,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2);

                        if (OverwriteWarningResult != System.Windows.Forms.DialogResult.Yes)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    // If user can't create Locations, don't let him find a new one
                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PLocationTable.GetTableDBName()))
                    {
                        TMessages.MsgSecurityException(new ESecurityDBTableAccessDeniedException("", "create",
                                PLocationTable.GetTableDBName()), this.GetType());
                        return;
                    }

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PPartnerLocationTable.GetTableDBName()))
                    {
                        TMessages.MsgSecurityException(new ESecurityDBTableAccessDeniedException("", "create",
                                PPartnerLocationTable.GetTableDBName()), this.GetType());
                        return;
                    }
                }

                // hourglass cursor
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                // give windows half a chance to show cursor
                Application.DoEvents();

                using (frmPartnerLS = new TPartnerLocationFind())
                      {
                          if (frmPartnerLS.ShowDialog() == DialogResult.OK)
                          {
                              LocationKey = frmPartnerLS.SelectedLocation.LocationKey;
                              SiteKey = frmPartnerLS.SelectedLocation.SiteKey;

                              // MessageBox.Show('selected location: ' + LocationKey.ToString());

                              // Need to switch to Address Tab to make sure it is initialised before
                              // executing methods on it...
                              ucoPartnerTabSet.SelectTabPage(TPartnerEditTabPageEnum.petpAddresses);

                              if (!ucoPartnerTabSet.IsAddressRowPresent(new TLocationPK(SiteKey, LocationKey)))
                              {
                                  // Retrieve data for the selected Location from the PetraServer
                                  LocationDT = FPartnerEditUIConnector.GetDataLocation(SiteKey, LocationKey);

                                  if (FPetraUtilsObject.DetailEditMode)
                                  {
                                      ucoPartnerTabSet.CopyFoundAddressData((PLocationRow)LocationDT.Rows[0]);
                                  }
                                  else
                                  {
                                      // Merge the data for the selected Location into the Location Table of the
                                      // Main DataSet.
                                      FMainDS.Merge(LocationDT);
                                      ucoPartnerTabSet.AddNewFoundAddress(SiteKey, LocationKey);
                                  }
                              }
                          }
                      }

                      // normal mouse cursor again
                      System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
#endif
        }

        private void MniView_Click(System.Object sender, System.EventArgs e)
        {
// TODO update menu enabled
#if TODO
            if (sender == mniViewPartnerData)
            {
                if (FCurrentModuleTabGroup != TModuleSwitchEnum.msPartner)
                {
                    tbbTogglePartner.Pushed = true;
                    tbbTogglePersonnel.Pushed = false;
                    tbbToggleFinance.Pushed = false;
                    ucoPartnerTabSet.Visible = true;
                    ucoPersonnelTabSet.Visible = false;
                    ucoFinanceTabSet.Visible = false;
                    FCurrentModuleTabGroup = TModuleSwitchEnum.msPartner;
                }
                else
                {
                    // don't allow ToolBarButton to be 'unpushed'
                    tbbTogglePartner.Pushed = true;
                }
            }
            else if (sender == mniViewPersonnelData)
            {
                if (FCurrentModuleTabGroup != TModuleSwitchEnum.msPersonnel)
                {
                    tbbTogglePersonnel.Pushed = true;
                    tbbTogglePartner.Pushed = false;
                    tbbToggleFinance.Pushed = false;

                    if (!FPersonnelTabSetInitialised)
                    {
                        ucoPersonnelTabSet.InitialiseUserControl();
                        FPersonnelTabSetInitialised = true;
                    }

                    ucoPersonnelTabSet.Visible = true;
                    ucoPartnerTabSet.Visible = false;
                    ucoFinanceTabSet.Visible = false;
                    FCurrentModuleTabGroup = TModuleSwitchEnum.msPersonnel;
                }
                else
                {
                    // don't allow ToolBarButton to be 'unpushed'
                    tbbTogglePersonnel.Pushed = true;
                }
            }
            else if (sender == mniViewFinanceData)
            {
                if (FCurrentModuleTabGroup != TModuleSwitchEnum.msFinance)
                {
                    tbbToggleFinance.Pushed = true;
                    tbbTogglePartner.Pushed = false;
                    tbbTogglePersonnel.Pushed = false;

                    if (!FFinanceTabSetInitialised)
                    {
                        ucoFinanceTabSet.InitialiseUserControl();
                        FFinanceTabSetInitialised = true;
                    }

                    ucoFinanceTabSet.Visible = true;
                    ucoPartnerTabSet.Visible = false;
                    ucoPersonnelTabSet.Visible = false;
                    FCurrentModuleTabGroup = TModuleSwitchEnum.msFinance;
                }
                else
                {
                    // don't allow ToolBarButton to be 'unpushed'
                    tbbToggleFinance.Pushed = true;
                }
            }
            else if (sender == mniViewUpperPartExpanded)
            {
                ucoUpperPart.Expand();
                mniViewUpperPartExpanded.Checked = true;
                mniViewUpperPartCollapsed.Checked = false;
            }
            else if (sender == mniViewUpperPartCollapsed)
            {
                ucoUpperPart.Collapse();
                mniViewUpperPartCollapsed.Checked = true;
                mniViewUpperPartExpanded.Checked = false;
            }
#endif
        }

        private void UcoPartnerTabSet_EnableDisableOtherScreenParts(System.Object sender, TEnableDisableEventArgs e)
        {
            // MessageBox.Show('TPartnerEditDSWinForm.ucoPartnerTabEnableDisableOtherScreenParts = ' + e.Enable.ToString + ')';
            EnableDisableUpperPart(e.Enable);

// TODO            tbrMain.Enabled = e.Enable;

            if (!e.Enable)
            {
                SetupAvailableModuleDataItems((!e.Enable), FCurrentModuleTabGroup);
                EnableSave(false);
                FPetraUtilsObject.DetailEditMode = true;
            }
            else
            {
                SetupAvailableModuleDataItems(e.Enable, TModuleSwitchEnum.msNone);
                EnableSave(FPetraUtilsObject.HasChanges);
                FPetraUtilsObject.DetailEditMode = false;
            }
        }

        private void UcoPartnerTabSet_HookupDataChange(System.Object sender, System.EventArgs e)
        {
            HookupDataChangeEvents();
        }

        private void UcoPartnerTabSet_HookupPartnerEditDataChange(System.Object sender, THookupPartnerEditDataChangeEventArgs e)
        {
            HookupPartnerEditDataChangeEvents(e.TabPage);
        }

        private void UcoPartnerTabSet_ShowTab(System.Object sender, TShowTabEventArgs e)
        {
// TODO foundation
#if TODO
            if (e.TabName == "tbpFoundationDetails")
            {
                if (e.Show)
                {
                    FFoundationDetailsEnabled = true;
                    mniMaintainFoundationDetails.Enabled = true;
                }
                else
                {
                    FFoundationDetailsEnabled = false;
                    mniMaintainFoundationDetails.Enabled = false;
                }
            }
#endif
        }

        private DataRow GetDataRowOfCurrentlySelectedRecord(Int32 SelectedTab)
        {
            return ucoPartnerTabSet.GetSelectedDataRow(SelectedTab);
        }

        /// <summary>
        /// Returns the new partner key that was created in this session of the Partner Edit screen.
        /// </summary>
        /// <returns>New partner key that was created in this session of the Partner Edit screen.
        /// If the screen was opened in Edit mode, then the return value is not valid.
        /// If the creation of a partner was cancelled, the return value is not valid.
        /// </returns>
        public System.Int64 GetNewPartnerKey()
        {
            return FNewPartnerPartnerKey;
        }

        private void TPartnerEditDSWinForm_Closing(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!FPetraUtilsObject.CloseFormCheckRun)
            {
                if (!CloseFormCheck())
                {
                    // MessageBox.Show('TPartnerEditDSWinForm.TPartnerEditDSWinForm_Closing: e.Cancel := true');
                    e.Cancel = true;
                }
                else
                {
                    if (FPartnerEditUIConnector != null)
                    {
                        // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
                        TEnsureKeepAlive.UnRegister(FPartnerEditUIConnector);
                    }

                    // Needs to be set to false because it got set to true in ancestor Form!
                    e.Cancel = false;

                    // Need to call the following method in the Base Form to remove this Form from the Open Forms List
                    FPetraUtilsObject.TFrmPetra_Closing(this, null);
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            else
            {
                if (FPartnerEditUIConnector != null)
                {
                    // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
                    TEnsureKeepAlive.UnRegister(FPartnerEditUIConnector);
                }

                // Needs to be set to false because it got set to true in ancestor Form!
                e.Cancel = false;

                // Need to call the following method in the Base Form to remove this Form from the Open Forms List
                FPetraUtilsObject.TFrmPetra_Closing(this, null);
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }

            TUserDefaults.SetDefault(TUserDefaults.PARTNER_EDIT_UPPERPARTCOLLAPSED, ucoUpperPart.IsCollapsed);
        }

        private void TPartnerEditDSWinForm_Load(System.Object sender, System.EventArgs e)
        {
            PartnerEditTDSPPartnerLocationRow NewPartnerLocationRow;

            // Reduce Form height to fit the PartnerEdit screen fully only on 800x600 resolution
            if (System.Windows.Forms.Screen.GetBounds(ucoUpperPart).Height == 600)
            {
                this.Height = 600;
            }

            /*
             * Load data for new Partner or existing Partner
             */
            if (FPetraUtilsObject.ScreenMode == TScreenMode.smNew)
            {
                try
                {
                    // Check security
                    if (!CheckSecurityOKToCreateNewPartner(true))
                    {
                        // User is not allowed to create new Partners!
                        // for the modal dialog (called from Progress)
                        DialogResult = System.Windows.Forms.DialogResult.Cancel;

                        // to prevent strange error message, that would stop the form from closing
                        FPetraUtilsObject.FormActivatedForFirstTime = false;
                        Close();
                        return;
                    }

                    // New Partner: retrieve default data for new Partner
                    FNewPartner = true;

                    if (!GetPartnerEditUIConnector(TUIConnectorType.uictNewPartner))
                    {
                        MessageBox.Show(
                            String.Format(CommonResourcestrings.StrOpeningCancelledByUser,
                                StrScreenCaption),
                            CommonResourcestrings.StrOpeningCancelledByUserTitle,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // to prevent strange error message, that would stop the form from closing
                        FPetraUtilsObject.FormActivatedForFirstTime = false;
                        Close();
                        return;
                    }

                    /*
                     * Show New Partner Dialog to get parameters for the new Partner,
                     * otherwise just use the parameters passed in SetParameters
                     */
                    if (FNewPartnerShowNewPartnerDialog)
                    {
                        if (!OpenNewPartnerDialog())
                        {
                            // for the modal dialog (called from Progress)
                            DialogResult = System.Windows.Forms.DialogResult.Cancel;

                            // to prevent strange error message, that would stop the form from closing
                            FPetraUtilsObject.FormActivatedForFirstTime = false;
                            Close();
                            return;
                        }
                    }

                    /*
                     * Obtain DataSet from Server, filled with default data according to parameters
                     */
                    this.Cursor = Cursors.WaitCursor;
                    FMainDS = FPartnerEditUIConnector.GetDataNewPartner(FNewPartnerSiteKey,
                        FNewPartnerPartnerKey,
                        SharedTypes.PartnerClassStringToEnum(FNewPartnerPartnerClass),
                        FNewPartnerCountryCode,
                        FNewPartnerAcquisitionCode,
                        FNewPartnerPrivatePartner,
                        FNewPartnerFamilyPartnerKey,
                        FNewPartnerFamilySiteKey,
                        FNewPartnerFamilyLocationKey,
                        out FNewPartnerSiteCountryCode);

                    if (FNewPartnerSiteCountryCode != "")
                    {
                        FNewPartnerCountryCode = FNewPartnerSiteCountryCode;
                    }

                    /*
                     * Create first Address for the new Partner
                     */
                    if (SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass) == TPartnerClass.PERSON)
                    {
                        // Create Address by copying over most of the data from the Family's Address
                        try
                        {
                            TAddressHandling.CreateNewAddress(FMainDS.PLocation,
                                FMainDS.PPartnerLocation,
                                FMainDS.PPartner[0].PartnerKey,
                                SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass),
                                "",
                                FNewPartnerFamilyLocationKey,
                                FNewPartnerFamilyPartnerKey,
                                FNewPartnerFamilyLocationKey,
                                FNewPartnerFamilySiteKey,
                                false,
                                true);
                        }
                        catch (ESecurityGroupAccessDeniedException Exp)
                        {
                            TMessages.MsgSecurityException(new ESecurityGroupAccessDeniedException(
                                    Exp.Message + "\r\n" + "\r\n" +
                                    "Cannot create new PERSON with the chosen Address of the FAMILY!" + "\r\n" +
                                    "You must choose a different Address of the FAMILY to be able to " + "create a PERSON for this FAMILY!"),
                                this.GetType());

                            // for the modal dialog (called from Progress)
                            DialogResult = System.Windows.Forms.DialogResult.Cancel;

                            // to prevent strange error message, that would stop the form from closing
                            FPetraUtilsObject.FormActivatedForFirstTime = false;
                            Close();
                            return;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    else
                    {
                        // Create Address with default values
                        TAddressHandling.CreateNewAddress(FMainDS.PLocation,
                            FMainDS.PPartnerLocation,
                            FMainDS.PPartner[0].PartnerKey,
                            SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass),
                            FNewPartnerCountryCode,
                            -1);

                        FNewPartnerWithAutoCreatedAddress = true;
                    }

                    // Make this address a Current Address and also the 'Best' Address
                    NewPartnerLocationRow = (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.Rows[0];
                    NewPartnerLocationRow.Icon = 1;
                    NewPartnerLocationRow.BestAddress = true;
                }
                catch (Exception Exp)
                {
                    this.Cursor = Cursors.Default;
                    TLogging.Log(
                        "An error occured while generating data for a new Partner!" + Environment.NewLine + Exp.ToString(), TLoggingType.ToLogfile);

                    // MessageBox.Show('An error occured while generating data for a new Partner!' + Environment.NewLine + 'For details see the log file.',
                    // 'Error in Partner Edit Screen', MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    // for the modal dialog (called from Progress)
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;

                    // to prevent strange error message, that would stop the form from closing
                    FPetraUtilsObject.FormActivatedForFirstTime = false;
                    this.Close();

                    throw;
                }

                /*
                 * From this point on, all data for the new Partner is in FMainDS!
                 */
            }
            else
            {
                // Existing Partner: retrieve screen data from the PetraServer
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (FLocationKeyForSelectingPartnerLocation == 0)
                    {
                        // Retrieve Partner Data using a specified Partner Key
                        // Obtain access to Server Object (DataSet is returned here as well)
                        if (!GetPartnerEditUIConnector(TUIConnectorType.uictPartnerKey))
                        {
                            MessageBox.Show(
                                String.Format(CommonResourcestrings.StrOpeningCancelledByUser,
                                    StrScreenCaption),
                                CommonResourcestrings.StrOpeningCancelledByUserTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // to prevent strange error message, that would stop the form from closing
                            FPetraUtilsObject.FormActivatedForFirstTime = false;
                            Close();
                            return;
                        }
                    }
                    else
                    {
                        // Retrieve Partner Data using a specified LocationRecID
                        // Obtain access to Server Object (DataSet is returned here as well)
                        if (!GetPartnerEditUIConnector(TUIConnectorType.uictLocationKey))
                        {
                            MessageBox.Show(
                                String.Format(CommonResourcestrings.StrOpeningCancelledByUser,
                                    StrScreenCaption),
                                CommonResourcestrings.StrOpeningCancelledByUserTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // to prevent strange error message, that would stop the form from closing
                            FPetraUtilsObject.FormActivatedForFirstTime = false;
                            Close();
                            return;
                        }

                        FPartnerKey = FMainDS.PPartner[0].PartnerKey;
                    }

#if TODO
                    // get Field of Partner (if PERSON or FAMILY)
                    cmdPartner = new TCmdMPartner();
                    System.Int64 FieldKey;
                    String FieldName;
                    bool HasCurrentCommitment;
                    cmdPartner.GetPartnerField(this, FPartnerKey, out FieldKey, out FieldName, out HasCurrentCommitment);

                    // MessageBox.Show('Partner ' + FPartnerKey.toString() + ' belongs to field ' + FieldName + ' (' + FieldKey.ToString() + ')');

                    // Check whether the Partner has "EX-WORKER*" Partner Type by looking this up in the MiscellaneusData Table
                    if ((FMainDS.MiscellaneousData[0].HasEXWORKERPartnerType == true)
                        && !HasCurrentCommitment)
                    {
                        ucoUpperPart.SetWorkerFieldText(MPartnerConstants.PARTNERTYPE_EX_WORKER);
                    }
                    else if (FieldName != "")
                    {
                        ucoUpperPart.SetWorkerFieldText(FieldName);
                    }
                    else
                    {
                        if (FieldKey > 0)
                        {
                            ucoUpperPart.SetWorkerFieldText(StringHelper.FormatStrToPartnerKeyString(FieldKey.ToString()));
                        }
                    }
#endif
                }
                catch (EPartnerNotExistantException)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(
                        "Partner with Partner Key " + FPartnerKey.ToString() + " does not exist.", "Nonexistant Partner!", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    // for the modal dialog (called from Progress)
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;

                    // to prevent strange error message, that would stop the form from closing
                    FPetraUtilsObject.FormActivatedForFirstTime = false;
                    this.Close();
                    return;
                }
                catch (EPartnerLocationNotExistantException Exp)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(
                        "Location with " + Exp.Message + " does not (or no longer) exist." + "\r\n" + "\r\n" +
                        "If you tried to open the Partner from a Partner Find screen you might need to perform" + "\r\n" +
                        "the Search operation again to get valid Location(s) for this Partner.",
                        "Nonexistant Location - Cannot Open Partner!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    // for the modal dialog (called from Progress)
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;

                    // to prevent strange error message, that would stop the form from closing
                    FPetraUtilsObject.FormActivatedForFirstTime = false;
                    this.Close();
                    return;
                }
                catch (ESecurityDBTableAccessDeniedException Exp)
                {
                    this.Cursor = Cursors.Default;
                    TMessages.MsgSecurityException(Exp, this.GetType());

                    // for the modal dialog (called from Progress)
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;

                    // to prevent strange error message, that would stop the form from closing
                    FPetraUtilsObject.FormActivatedForFirstTime = false;
                    this.Close();
                    return;
                }
                catch (ESecurityScreenAccessDeniedException Exp)
                {
                    this.Cursor = Cursors.Default;
                    TMessages.MsgSecurityException(Exp, this.GetType());

                    // for the modal dialog (called from Progress)
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;

                    // to prevent strange error message, that would stop the form from closing
                    FPetraUtilsObject.FormActivatedForFirstTime = false;
                    this.Close();
                    return;
                }
                catch (ESecurityPartnerAccessDeniedException Exp)
                {
                    this.Cursor = Cursors.Default;
                    TMessages.MsgSecurityException(Exp, this.GetType());

                    // for the modal dialog (called from Progress)
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;

                    // to prevent strange error message, that would stop the form from closing
                    FPetraUtilsObject.FormActivatedForFirstTime = false;
                    this.Close();
                    return;
                }
                catch (Exception Exp)
                {
                    this.Cursor = Cursors.Default;
                    TLogging.Log(
                        "An error occured while trying to retrieve data for the Partner Edit Screen!" + Environment.NewLine + Exp.ToString(),
                        TLoggingType.ToLogfile);

                    // MessageBox.Show('An error occured while trying to retrieve data for the Partner Edit Screen!' + Environment.NewLine + 'For details see the log file.',
                    // 'Error in Partner Edit Screen', MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    // for the modal dialog (called from Progress)
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;

                    // to prevent strange error message, that would stop the form from closing
                    FPetraUtilsObject.FormActivatedForFirstTime = false;
                    this.Close();

                    throw;
                }
            }

            /*
             * From here on we have access to the Server Object and the DataSet is filled
             * with data.
             */
            FPartnerClass = FMainDS.PPartner[0].PartnerClass;
            FCurrentModuleTabGroup = TModuleSwitchEnum.msPartner;

            // Determine whether Partner is of PartnerClass ORGANISATION and whether it is a Foundation
            DetermineOrganisationIsFoundation();

            // Determine which tab page will be shown
            DetermineInitiallySelectedTabPage();

            // Setup Modulerelated Toggle Buttons in ToolBar
            SetupAvailableModuleDataItems(true, TModuleSwitchEnum.msNone);


            //
            // Set up ucoPartnerTabSet
            //
            // this.ucoPartnerTabSet := new TUC_PartnerEdit_PartnerTabSet();
            // this.ucoPartnerTabSet.BackColor := System.Drawing.Color.LightSteelBlue;
            // this.ucoPartnerTabSet.Dock := System.Windows.Forms.DockStyle.Fill;
            // this.ucoPartnerTabSet.DockPadding.Bottom := 1;
            // this.ucoPartnerTabSet.DockPadding.Left := 2;
            // this.ucoPartnerTabSet.DockPadding.Right := 1;
            // this.ucoPartnerTabSet.DockPadding.Top := 2;
            // this.ucoPartnerTabSet.Font := new System.Drawing.Font('Verdana', 8.25f, System.Drawing.FontStyle.Regular,
            // System.Drawing.GraphicsUnit.Point, (byte)0);
            // this.ucoPartnerTabSet.InitiallySelectedTabPage := TPartnerEditTabPageEnum.petpDefault;
            // this.ucoPartnerTabSet.Location := new System.Drawing.Point(0, 1);
            // this.ucoPartnerTabSet.MainDS := nil;
            // this.ucoPartnerTabSet.Name := 'ucoPartnerTabSet';
            // this.ucoPartnerTabSet.Size := new System.Drawing.Size(754, 476);
            // this.ucoPartnerTabSet.TabIndex := 0;
            //
            // this.pnlLowerPart.Controls.Add(this.ucoPartnerTabSet);
            // this.ucoPartnerTabSet.BringToFront;
            ucoPartnerTabSet.MainDS = FMainDS;
            ucoPartnerTabSet.PetraUtilsObject = FPetraUtilsObject;
            ucoPartnerTabSet.PartnerEditUIConnector = FPartnerEditUIConnector;
            ucoPartnerTabSet.InitiallySelectedTabPage = FInitiallySelectedTabPage;
            ucoPartnerTabSet.InitialiseDelegateIsNewPartner(@IsNewPartner);

            // Hook up EnableDisableOtherScreenParts Event that is fired by ucoPartnerTabSet
            ucoPartnerTabSet.EnableDisableOtherScreenParts += new TEnableDisableScreenPartsEventHandler(
                this.UcoPartnerTabSet_EnableDisableOtherScreenParts);

            // Hook up ShowTab Event that is fired by FUcoPartnerDetailsOrganisation
            ucoPartnerTabSet.ShowTab += new TShowTabEventHandler(this.UcoPartnerTabSet_ShowTab);

            // Hook up HookUpDataChange Event that is fired by ucoPartnerTabSet
            ucoPartnerTabSet.HookupDataChange += new THookupDataChangeEventHandler(this.UcoPartnerTabSet_HookupDataChange);
            ucoPartnerTabSet.HookupPartnerEditDataChange += new THookupPartnerEditDataChangeEventHandler(
                this.UcoPartnerTabSet_HookupPartnerEditDataChange);
            ucoPartnerTabSet.InitialiseUserControl();

            if (FNewPartnerWithAutoCreatedAddress)
            {
                // hardcoded for the first Address of a new Partner
                ucoPartnerTabSet.DisableNewButtonOnAutoCreatedAddress();
            }

            HookupPartnerEditDataChangeEvents(TPartnerEditTabPageEnum.petpDefault);

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.FormDataSavingStarted);

            // Hook up DataSaved Event to be able to run code after SaveChanges was run
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(this.FormDataSaved);

// TODO            ucoUpperPart.InitialiseDelegateMaintainWorkerField(new TDelegateMaintainWorkerField(MaintainWorkerField));
            ucoUpperPart.VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            ucoUpperPart.PartnerEditUIConnector = FPartnerEditUIConnector;

            // DataBind ucoUpperPart
            FPetraUtilsObject.SuppressChangeDetection = true;
            ucoUpperPart.PerformDataBinding(FMainDS);
            FPetraUtilsObject.SuppressChangeDetection = false;

            // Set up screen caption
            SetScreenCaption();

            // Collapse upper Part, if user had it so last time
            FUppperPartInitiallyCollapsed = TUserDefaults.GetBooleanDefault(TUserDefaults.PARTNER_EDIT_UPPERPARTCOLLAPSED, false);

            if (FUppperPartInitiallyCollapsed)
            {
                MniViewUpperPartCollapsed_Click(this, null);
            }
            else
            {
                MniViewUpperPartExpanded_Click(this, null);
            }

            // Disable 'Local Partner Data' MenuItem if there are no Office Specific Data Labels available
            if (!FMainDS.MiscellaneousData[0].OfficeSpecificDataLabelsAvailable)
            {
// TODO                mniMaintainOfficeSpecific.Enabled = false;
            }

            FSubmitChangesContinue = false;
            ApplySecurity();

            // Need to do this manually  we disabled the automatic hookup in the Base Form because
            // we remove some TabPages, therefore the Controls on it, but the Events hooked
            // up to them would still be around and prevent a GC of the Form!
            FPetraUtilsObject.HookupAllControls();


            ucoUpperPart.Focus();
            this.Cursor = Cursors.Default;

            // Checks whether there any Tips to show to the User; if there are, they will be
            // shown.
            ucoUpperPart.CheckForUserTips();
        }

        private void UcoUpperPart_PartnerClassMainDataChanged(System.Object Sender, TPartnerClassMainDataChangedEventArgs e)
        {
            FPetraUtilsObject.HasChanges = true;
            SetScreenCaption();
        }

        private void UcoUpperPart_CollapsingEvent(System.Object sender, CollapsibleEventArgs args)
        {
            if (args.WillCollapse)
            {
                // is getting collapsed
                ucoUpperPart.Caption = "  " + ucoUpperPart.PartnerQuickInfo(false);
                ucoUpperPart.SubCaption = '[' + FPartnerClass + "] ";

                if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                {
                    ucoUpperPart.SubCaptionHighlighted = true;
                }
            }
            else
            {
                // is getting expanded
                ucoUpperPart.Caption = "";
                ucoUpperPart.SubCaption = "";
            }
        }

        private void TPartnerEditDSWinForm_KeyDown(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // evaluate CTRL keys
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.G:

                        if (ucoUpperPart.IsCollapsed)
                        {
                            MniViewUpperPartExpanded_Click(this, null);
                        }
                        else
                        {
                            MniViewUpperPartCollapsed_Click(this, null);
                        }

                        break;
                }
            }
            else
            {
                // evaluate all other keys
            }
        }

        private void MniViewUpperPartCollapsed_Click(System.Object sender, System.EventArgs e)
        {
            ucoUpperPart.Collapse();

// TODO            mniViewUpperPartCollapsed.Checked = true;
// TODO            mniViewUpperPartExpanded.Checked = false;
        }

        private void MniViewUpperPartExpanded_Click(System.Object sender, System.EventArgs e)
        {
            ucoUpperPart.Expand();

// TODO            mniViewUpperPartExpanded.Checked = true;
// TODO            mniViewUpperPartCollapsed.Checked = false;
        }

        private void MniFileDeactivatePartner_Click(System.Object sender, System.EventArgs e)
        {
// TODO partner deactivate
#if TODO
            TPartnerDeactivateDialogWinForm PartnerDeactivateDialog;
            String NewPartnerStatusCode;
            Boolean CancelDeactivationProcess;
            Boolean CancelAllSubscriptions;
            Boolean ExpireAllCurrentAddresses;
            DateTime ValidTo;
            Boolean StatusCodeChanged;

            CancelDeactivationProcess = false;
            StatusCodeChanged = false;

            // Show 'Deactivate Partner' Dialog
            PartnerDeactivateDialog = new TPartnerDeactivateDialogWinForm();

            if (PartnerDeactivateDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                PartnerDeactivateDialog.GetReturnedParameters(out NewPartnerStatusCode,
                    out CancelAllSubscriptions,
                    out ExpireAllCurrentAddresses,
                    out ValidTo);

                // MessageBox.Show('NewPartnerStatusCode: ' + NewPartnerStatusCode + "\r\n" +
                // 'CancelAllSubscriptions: ' + CancelAllSubscriptions.ToString + "\r\n" +
                // 'ExpireAllCurrentAddresses: ' + ExpireAllCurrentAddresses.ToString + "\r\n" +
                // 'ValidTo: ' + ValidTo.ToString);
                // get PartnerDeactivateDialog out of memory
                PartnerDeactivateDialog.Dispose();
                Application.DoEvents();

                // MessageBox.Show('PartnerDeactivateDialog: pressed OK.');

                /*
                 * Perform desired actions
                 */

                // Assign new Partner StatusCode?
                if (NewPartnerStatusCode != "")
                {
                    if (FMainDS.PPartner[0].StatusCode != NewPartnerStatusCode)
                    {
                        // Do a BeginEdit here so we can call CancelEdit later if needed
                        FMainDS.PPartner[0].BeginEdit();
                        StatusCodeChanged = true;

                        // If the current Partner is a FAMILY and there are Family Members:
                        // after the following assigment a MessageBox is shown that allows the
                        // user to choose whether to promote the StatusCode change to be
                        // promoted to all the Family Members!
                        FMainDS.PPartner[0].StatusCode = NewPartnerStatusCode;

                        // If the current Partner is a FAMILY and there are Family Members AND
                        // the user has chosen 'Cancel' in the MessageBox, the StatusCode change
                        // will be undone. Check for this here; if it was undone, we don't want
                        // to continue Deactivation of Partner.
                        if (FMainDS.PPartner[0].StatusCode != NewPartnerStatusCode)
                        {
                            CancelDeactivationProcess = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show(String.Format(StrDeactivatePartnerStatusNotChanged,
                                FMainDS.PPartner[0].StatusCode), StrDeactivatePartnerTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Cancel All Subscriptions?
                if ((CancelAllSubscriptions) && (!CancelDeactivationProcess))
                {
                    if (!ucoPartnerTabSet.PerformCancelAllSubscriptions(ValidTo))
                    {
                        CancelDeactivationProcess = true;
                    }
                }

                // Expire All Current Addresses?
                if ((ExpireAllCurrentAddresses) && (!CancelDeactivationProcess))
                {
                    ucoPartnerTabSet.ExpireAllCurrentAddresses(ValidTo);
                }

                // Check if user cancelled any of the actions
                if (CancelDeactivationProcess)
                {
                    if (StatusCodeChanged)
                    {
                        // Undo the change to the StatusCode that was done earlier on!
                        FMainDS.PPartner[0].CancelEdit();
                    }

                    MessageBox.Show(StrDeactivatePartnerActionCancelled, StrDeactivatePartnerTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (StatusCodeChanged)
                    {
                        // Commit the change to the StatusCode that was done earlier on!
                        FMainDS.PPartner.Rows[0].EndEdit();
                    }

                    MessageBox.Show(StrDeactivatePartnerSuccess, StrDeactivatePartnerTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // get PartnerDeactivateDialog out of memory
                PartnerDeactivateDialog.Dispose();

                // MessageBox.Show('PartnerDeactivateDialog: pressed Cancel.');
            }
#endif
        }

        private void MniFileDeletePartner_Click(System.Object sender, System.EventArgs e)
        {
// TODO delete partner
#if TODO
            TCmdMPartner cmd;

            cmd = new TCmdMPartner();

            // Check for new Partner that wasn't saved yet
            if (IsNewPartner(FMainDS))
            {
                // Tell user that he can't delete a new Partner that wasn't saved yet
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorDeletePartner2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
            }
            else
            {
                // Check for unsaved changes
                if (!FPetraUtilsObject.CloseFormCheckRun)
                {
                    if (!CloseFormCheck())
                    {
                        // Tell user that he can't delete a Partner that has changes that weren't saved yet
                        MessageBox.Show(StrCannotDeletePartner, StrCannotDeletePartnerTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // Delete Partner; if OK, close the screen
                        if (cmd.RunDeletePartner(this, FPartnerKey))
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    // Delete Partner; if OK, close the screen
                    if (cmd.RunDeletePartner(this, FPartnerKey))
                    {
                        this.Close();
                    }
                }
            }
#endif
        }

        private void MniFileExportPartner_Click(System.Object sender, System.EventArgs e)
        {
// TODO delete partner
#if TODO
            TCmdMPartner cmd;

            // MessageBox.Show('4GL call to pa12430.p will happen here');
            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorExportPartner2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
                return;
            }

            cmd = new TCmdMPartner();

            /*
             * Export the Partner.
             * Note: We pass in 0 for SiteKey and LocationKey parameters - this tells the Export routine to export
             * the 'Best Address' of the Partner. Wolfgang prefers that over passing in the SiteKey and LocationKey
             * of the currently selected Address in the Address Tab. The user might not be aware of what Location is
             * selected and could this way even export an Expired Address (if an Expired Address happens to be
             * selected) - without him/her realising that. This is a change to how it worked in Petra 2.1.
             */
            cmd.RunExportPartner(this, FPartnerKey, 0, 0);
        }

        private void MniFileNew_Click(System.Object sender, System.EventArgs e)
        {
            TApplinkOrNetAutoSelector.OpenNewPartnerDialog(this, FPartnerClass,
                ucoPartnerTabSet.LocationRowOfCurrentlySelectedAddress.CountryCode);
#endif
        }

        private void MniFileSendEmail_Click(System.Object sender, System.EventArgs e)
        {
// TODO delete partner
#if TODO
            String EmailAddress;
            TVerificationResult VerificationResult;
            Boolean EmailAddressValid;
            TCmdMSysMan cmd;

            EmailAddress = TSaveConvert.StringColumnToString(FMainDS.PPartnerLocation.ColumnEmailAddress,
                ucoPartnerTabSet.PartnerLocationRowOfCurrentlySelectedAddress);
            VerificationResult = (TVerificationResult)TStringChecks.ValidateEmail(EmailAddress, true);

            if (VerificationResult == null)
            {
                EmailAddressValid = true;
            }
            else
            {
                EmailAddressValid = false;
            }

            if (EmailAddress == "")
            {
                MessageBox.Show(Resourcestrings.StrErrorNoEmailAddressForThisPartner,
                    Resourcestrings.StrErrorNoEmailAddressForThisPartnerTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!EmailAddressValid)
            {
                MessageBox.Show(Resourcestrings.StrErrorNoValidEmailAddressForThisPartner + "\r\n" + "\r\n" +
                    Resourcestrings.StrDetails + "\r\n" +
                    VerificationResult.FResultText,
                    Resourcestrings.StrErrorNoEmailAddressForThisPartnerTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                cmd = new TCmdMSysMan();
                cmd.SendEmail(EmailAddress);
            }
#endif
        }

        private void MniHelpVideoTutorial_Click(System.Object sender, System.EventArgs e)
        {
// TODO no link to ict-software.org
            const String NETWORKINSTALL_TUTORIALPATH = "I:\\Petra\\";
            const String TUTORIAL_URL = "http://www.ict-software.org/petra/downloads/2.2/training/";
            const String TUTORIAL_NAME = "Petra-2.2PartnerEditDemo.exe";
            const String BRowSER_EXE = "IExplore.exe";
            String ExePath;
            String TutorialPath;

            System.Diagnostics.Process TutorialProcess;
            System.Diagnostics.Process BRowserProcess;
            Boolean TutorialLaunched;
            Boolean BRowserLaunched;
            System.Windows.Forms.DialogResult DialogAnswer;
            TutorialLaunched = false;
            BRowserLaunched = false;

            if ((TClientSettings.RunAsStandalone) || (TClientSettings.RunAsRemote))
            {
                // When we are running as Standalone or Remote installation,
                // we expect to find the Video on directory higher than the application
                // (normally this would be:  C:\Petra2 )
                ExePath = Application.ExecutablePath;

                // MessageBox.Show('ExePath: ' + ExePath + "\r\n" +
                // 'Path.DirectorySeparatorChar: ' + Path.DirectorySeparatorChar);
                TutorialPath = ExePath.Substring(0, ExePath.LastIndexOf(Path.DirectorySeparatorChar));

                // this gives the application directory
                TutorialPath = TutorialPath.Substring(0, TutorialPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                // this gives the next higher directory
            }
            else
            {
                // If we are running on a Network installation, just take the network path
                TutorialPath = NETWORKINSTALL_TUTORIALPATH;
            }

            // MessageBox.Show('TutorialPath: ' + TutorialPath);
            // Check if Video Tutorial program exists in the TutorialPath
            if (System.IO.File.Exists(TutorialPath + TUTORIAL_NAME))
            {
                // Try to start the Video Tutorial program
                try
                {
                    TutorialProcess = new System.Diagnostics.Process();
                    TutorialProcess.EnableRaisingEvents = false;
                    TutorialProcess.StartInfo.FileName = TutorialPath + TUTORIAL_NAME;
                    TutorialProcess.StartInfo.WorkingDirectory = TutorialPath;
                    TutorialProcess.StartInfo.Arguments = "";
                    TutorialProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

                    if (TutorialProcess.Start())
                    {
                        TutorialLaunched = true;
                    }
                    else
                    {
                        TutorialLaunched = false;
                    }
                }
                catch (Exception exp)
                {
                    TLogging.Log("An Exception occured while starting Video Tutorial program: " + exp.ToString());
                }

                if (!TutorialLaunched)
                {
                    MessageBox.Show(StrVideoTutorialLaunchFailed);
                }
            }
            else
            {
                // Video Tutorial program doesn't exist
                DialogAnswer = MessageBox.Show(String.Format(StrVideoTutorialNotFound,
                        TutorialPath,
                        TUTORIAL_NAME), StrVideoTutorialTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (DialogAnswer == System.Windows.Forms.DialogResult.Yes)
                {
                    // Try to start the Internet Explorer web bRowser and to point it to the download address!
                    try
                    {
                        BRowserProcess = new System.Diagnostics.Process();
                        BRowserProcess.EnableRaisingEvents = false;
                        BRowserProcess.StartInfo.FileName = BRowSER_EXE;
                        BRowserProcess.StartInfo.Arguments = TUTORIAL_URL + TUTORIAL_NAME;
                        BRowserProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

                        if (BRowserProcess.Start())
                        {
                            BRowserLaunched = true;
                        }
                        else
                        {
                            BRowserLaunched = false;
                        }
                    }
                    catch (Exception exp)
                    {
                        TLogging.Log("An Exception occured while starting Internet Explorer BRowser program: " + exp.ToString());
                    }

                    if (BRowserLaunched)
                    {
                        DialogAnswer =
                            MessageBox.Show(StrVideoTutorialWebBRowserLaunched + "\r\n" + String.Format(StrVideoTutorialDownloadInstructions,
                                    TutorialPath), StrDownloadVideoTutorialTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (DialogAnswer == System.Windows.Forms.DialogResult.Yes)
                        {
                            // simply start over again (recursive call!)
                            MniHelpVideoTutorial_Click(this, null);
                        }
                    }
                    else
                    {
                        DialogAnswer = MessageBox.Show(
                            StrVideoTutorialWebBRowserLaunchFailed + TUTORIAL_URL + TUTORIAL_NAME + "\r\n" + "\r\n" +
                            String.Format(StrVideoTutorialDownloadInstructions, TutorialPath),
                            StrDownloadVideoTutoriaManuallTitle,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                        if (DialogAnswer == System.Windows.Forms.DialogResult.Yes)
                        {
                            // simply start over again (recursive call!)
                            MniHelpVideoTutorial_Click(this, null);
                        }
                    }
                }
            }
        }

        private void MniMaintainAddresses_Click(System.Object sender, System.EventArgs e)
        {
            ucoPartnerTabSet.SelectTabPage(TPartnerEditTabPageEnum.petpAddresses);
        }

        private void MniMaintainBankAccounts_Click(System.Object sender, System.EventArgs e)
        {
            MessageBox.Show("Not implemented yet!");
        }

        private void MniMaintainContacts_Click(System.Object sender, System.EventArgs e)
        {
// TODO delete partner
#if TODO
            TCmdMPartner cmd;

            // MessageBox.Show('4GL call to ma1300b.w will happen here');
            if (IsNewPartner(FMainDS))
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorMaintainContacts2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
                return;
            }

            cmd = new TCmdMPartner();
            cmd.RunMaintainContacts(this, this.FMainDS.PPartner[0].PartnerKey);
#endif
        }

        private void MniMaintainDonorHistory_Click(System.Object sender, System.EventArgs e)
        {
// TODO delete partner
#if TODO
            Int32 LedgerNumber;
            String LedgerName;
            TCmdMFinance cmd;

            if (IsNewPartner(FMainDS))
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorMaintainDonorGiftHistory2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
                return;
            }
            else
            {
                cmd = new TCmdMFinance();
                LedgerNumber = cmd.GetLedgerNumber(this, out LedgerName);

                // MessageBox.Show('LedgerNumber: ' + LedgerNumber.ToString + '; LedgerName: ' + LedgerName);
                if (LedgerNumber != 0)
                {
                    cmd.OpenDonorGiftHistory(this, LedgerNumber, FPartnerKey);
                }
            }
#endif
        }

        private void MniMaintainRecipientHistory_Click(System.Object sender, System.EventArgs e)
        {
// TODO delete partner
#if TODO
            Int32 LedgerNumber;
            String LedgerName;
            TCmdMFinance cmd;

            if (IsNewPartner(FMainDS))
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 +
                    Resourcestrings.StrErrorMaintainRecipientGiftHistory2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
                return;
            }
            else
            {
                cmd = new TCmdMFinance();
                LedgerNumber = cmd.GetLedgerNumber(this, out LedgerName);

                // MessageBox.Show('LedgerNumber: ' + LedgerNumber.ToString + '; LedgerName: ' + LedgerName);
                if (LedgerNumber != 0)
                {
                    cmd.OpenRecipientGiftHistory(this, LedgerNumber, FPartnerKey);
                }
            }
#endif
        }

        private void MniMaintainFamilyMembers_Click(System.Object sender, System.EventArgs e)
        {
            if ((FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
                || (FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON)))
            {
                ucoPartnerTabSet.SelectTabPage(TPartnerEditTabPageEnum.petpFamilyMembers);
            }
            else
            {
                MessageBox.Show(CommonResourcestrings.StrErrorOnlyForFamilyOrPerson,
                    CommonResourcestrings.StrGenericFunctionalityNotAvailable);
            }
        }

        private void MniMaintainFinanceDetails_Click(System.Object sender, System.EventArgs e)
        {
// TODO delete partner
#if TODO
            TCmdMPartner cmd;

            // MessageBox.Show('4GL call to ma1003e.w will happen here');
            if (IsNewPartner(FMainDS))
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorMaintainFinanceDetails2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
                return;
            }

            cmd = new TCmdMPartner();
            cmd.RunMaintainFinanceDetails(this, this.FMainDS.PPartner[0].PartnerKey);
#endif
        }

        private void MniMaintainFinanceReports_Click(System.Object sender, System.EventArgs e)
        {
            MessageBox.Show("Not implemented yet!");
        }

        private void MniMaintainFoundationDetails_Click(System.Object sender, System.EventArgs e)
        {
            ucoPartnerTabSet.SelectTabPage(TPartnerEditTabPageEnum.petpFoundationDetails);
        }

        private void MniMaintainGiftReceipting_Click(System.Object sender, System.EventArgs e)
        {
            MessageBox.Show("Not implemented yet!");
        }

        private void MniMaintainNotes_Click(System.Object sender, System.EventArgs e)
        {
            ucoPartnerTabSet.SelectTabPage(TPartnerEditTabPageEnum.petpNotes);
        }

        private void MniMaintainOfficeSpecific_Click(System.Object sender, System.EventArgs e)
        {
            ucoPartnerTabSet.SelectTabPage(TPartnerEditTabPageEnum.petpOfficeSpecific);
        }

// TODO maintain screens in 4gl
#if TODO
        private void MniMaintainInterests_Click(System.Object sender, System.EventArgs e)
        {
            TCmdMPartner cmd;

            // $IFNDEF SHOWINTERESTSTAB
            if (IsNewPartner(FMainDS))
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorMaintainInterests2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
                return;
            }

            cmd = new TCmdMPartner();
            cmd.RunMaintainInterests(this, this.FMainDS.PPartner[0].PartnerKey);

            // $ELSE
            // ucoPartnerTabSet.SelectTabPage(TPartnerEditTabPageEnum.petpInterests);
            // $ENDIF
        }

        private void MaintainFieldCallback(TCommand cmd, ref Boolean bExpectAnother)
        {
            // PartnerKey: System.Int64;
            System.Int64 FieldKey;
            String FieldName;
            bool HasCurrentCommitment;

            if (cmd.commandName == "SUCCESS")
            {
                // PartnerKey := cmd.GetIntegerParam(0);
                FieldKey = cmd.GetIntegerParam(1);
                FieldName = cmd.GetStringParam(2);
                HasCurrentCommitment = cmd.GetBooleanParam(3);

                if (FieldKey == 0)
                {
                    // cancelled screen
                }
                else if (FieldKey < 0)
                {
                    // Something went wrong in the AppLink call, therefore remove field so we don't dispay something wrong
                    if ((HasPartnerType(MPartnerConstants.PARTNERTYPE_EX_WORKER) == true) && !HasCurrentCommitment)
                    {
                        ucoUpperPart.SetWorkerFieldText(MPartnerConstants.PARTNERTYPE_EX_WORKER);
                    }
                    else
                    {
                        ucoUpperPart.SetWorkerFieldText("");

                        // MessageBox.Show(this, 'The field was removed', '');
                    }
                }
                else
                {
                    // new field
                    if ((HasPartnerType(MPartnerConstants.PARTNERTYPE_EX_WORKER) == true) && !HasCurrentCommitment)
                    {
                        ucoUpperPart.SetWorkerFieldText(MPartnerConstants.PARTNERTYPE_EX_WORKER);
                    }
                    else if (FieldName != "")
                    {
                        ucoUpperPart.SetWorkerFieldText(FieldName);
                    }
                    else
                    {
                        ucoUpperPart.SetWorkerFieldText(StringHelper.FormatStrToPartnerKeyString(FieldKey.ToString()));
                    }

                    // MessageBox.Show(this, 'new field: ' + FieldKey.toString() + ' ' + FieldName, 'Not implemented yet');
                }

                bExpectAnother = false;
            }
            else
            {
                // error, no selection accepted
                bExpectAnother = false;
            }
        }

        private void MaintainWorkerField()
        {
            TCmdMPartner cmd;

            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorMaintainField2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
                return;
            }

            cmd = new TCmdMPartner();
            cmd.RunMaintainField(this, this.FMainDS.PPartner[0].PartnerKey, MaintainFieldCallback);
        }

        private void MniMaintainWorkerField_Click(System.Object sender, System.EventArgs e)
        {
            MaintainWorkerField();
        }

        private void MniMaintainPartnerDetails_Click(System.Object sender, System.EventArgs e)
        {
            ucoPartnerTabSet.SelectTabPage(TPartnerEditTabPageEnum.petpDetails);
        }

        private void MniMaintainPersonnelIndividualData_Click(System.Object sender, System.EventArgs e)
        {
// TODO delete partner
#if TODO
            TCmdMPartner cmd;

            // MessageBox.Show('4GL call to peindvs.w/peunits.w will happen here');
            if (IsNewPartner(FMainDS))
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorMaintainIndividualData2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
                return;
            }

            cmd = new TCmdMPartner();

            if ((this.FMainDS.PPartner[0].PartnerClass == "PERSON") || (this.FMainDS.PPartner[0].PartnerClass == "UNIT"))
            {
                cmd.RunMaintainPersonnel(this, this.FMainDS.PPartner[0].PartnerKey);
            }
            else
            {
                MessageBox.Show(CommonResourcestrings.StrErrorOnlyForPersonOrUnit,
                    CommonResourcestrings.StrGenericFunctionalityNotAvailable);
            }
#endif
        }

        private void MniMaintainRelationships_Click(System.Object sender, System.EventArgs e)
        {
// TODO delete partner
#if TODO
            TCmdMPartner cmd;

            // MessageBox.Show('4GL call to ma1100b.w will happen here');
            if (IsNewPartner(FMainDS))
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorMaintainRelationships2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
                return;
            }

            cmd = new TCmdMPartner();
            cmd.RunMaintainRelationships(this, this.FMainDS.PPartner[0].PartnerKey);
#endif
        }

        private void MniMaintainReminders_Click(System.Object sender, System.EventArgs e)
        {
// TODO delete partner
#if TODO
            TCmdMPartner cmd;

            // MessageBox.Show('4GL call to ma1600b.w will happen here');
            if (IsNewPartner(FMainDS))
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 +
                    Resourcestrings.StrErrorMaintainReminders2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
                return;
            }

            cmd = new TCmdMPartner();
            cmd.RunMaintainReminders(this, this.FMainDS.PPartner[0].PartnerKey);
#endif
        }

        private void MniMaintainSpecialTypes_Click(System.Object sender, System.EventArgs e)
        {
            ucoPartnerTabSet.SelectTabPage(TPartnerEditTabPageEnum.petpPartnerTypes);
        }

        private void MniMaintainSubscriptions_Click(System.Object sender, System.EventArgs e)
        {
            ucoPartnerTabSet.SelectTabPage(TPartnerEditTabPageEnum.petpSubscriptions);
        }
#endif



        #endregion

        /// <summary>
        /// This function is used to determine whether the partner has type EX-WORKER* when
        /// displaying the Worker Field
        /// </summary>
        /// <returns>void</returns>
        private bool HasPartnerType(String APartnerType)
        {
            /*
             * Need to load data here to be able to access the PartnerType table
             * in case DelayedDataLoading is switched on and the User hasn't switched
             * to the Special Types Tab yet.
             */
            if (FMainDS.PPartnerType == null)
            {
                FMainDS.Tables.Add(new PPartnerTypeTable());
                FMainDS.InitVars();
            }

            if (FMainDS.PPartnerType.Rows.Count == 0)
            {
                FMainDS.Merge(FPartnerEditUIConnector.GetDataPartnerTypes());
                FMainDS.PPartnerType.AcceptChanges();
            }

            return Ict.Petra.Shared.MPartner.Checks.HasPartnerType(APartnerType,
                FMainDS.PPartnerType, false);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public TFrmPetraEditUtils GetPetraUtilsObject()
        {
            return FPetraUtilsObject;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="o"></param>
        /// <param name="args"></param>
        public void OnDataSaved(Object o, TDataSavedEventArgs args)
        {
            // TODO event OnDataSaved
        }
    }
}