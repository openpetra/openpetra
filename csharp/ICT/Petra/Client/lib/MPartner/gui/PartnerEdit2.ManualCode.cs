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
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.DB;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.RemotedExceptions;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TFrmPartnerEdit2
    {
        #region Enums

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

        #endregion

        #region Fields

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

        private Boolean FPartnerTabSetInitialised;

//TODO        private Boolean FPersonnelTabSetInitialised;
//TODO        private Boolean FFinanceTabSetInitialised;
        private TModuleSwitchEnum FCurrentModuleTabGroup;

        /// <summary>Stores any Exception that occurs while the screen is loading</summary>
        public static Exception UExceptionAtLoad = null;

        #endregion

        #region ResourceStrings

        // TODO 2 Replace with String.Format(Catalog.GetString("Hello {0}"), myname);

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

        #endregion


        #region Public Methods

        /// <summary>
        /// to load a partner, set this property before showing the screen, or use SetParameters
        /// </summary>
        public Int64 PartnerKey
        {
            get
            {
                return FPartnerKey;
            }
            set
            {
                FPartnerKey = value;
                FPetraUtilsObject.ScreenMode = TScreenMode.smEdit;
            }
        }

        /// <summary>
        /// set this property before showing the screen, or use SetParameters
        /// </summary>
        public TPartnerEditTabPageEnum InitiallySelectedTabPage
        {
            get
            {
                return FInitiallySelectedTabPage;
            }
            set
            {
                FInitiallySelectedTabPage = value;
            }
        }

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

        /// <summary>
        /// Initializes a new instance of the TPartnerEditDSWinForm class (constructor)
        /// </summary>
        public void InitializeManualCode()
        {
            ucoLowerPart.DataLoadingStarted += new System.EventHandler(this.DataLoadOperationStarting);
            ucoLowerPart.DataLoadingFinished += new System.EventHandler(this.DataLoadOperationFinishing);

            FPetraUtilsObject.NoAutoHookupOfAllControls = true;

            // Initially, DON'T detect changes to the controls
            // and enable the save button. Wait until the data is loaded
            FPetraUtilsObject.SuppressChangeDetection = true;

            this.ucoUpperPart.PartnerClassMainDataChanged += new TPartnerClassMainDataChangedHandler(this.UcoUpperPart_PartnerClassMainDataChanged);
        }

        /// <summary>
        /// save the changes on the screen
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            // TODO

            return false;
        }

        #endregion


        #region Event Handlers

        private void TFrmPartnerEdit2_Load(System.Object sender, System.EventArgs e)
        {
            // Reduce Form height to fit the PartnerEdit screen fully only on 800x600 resolution
            if (System.Windows.Forms.Screen.GetBounds(ucoUpperPart).Height == 600)
            {
                this.Height = 600;
            }

            // Determine which tab page will be shown
            DetermineInitiallySelectedTabPage();

            /*
             * Load data for new Partner or existing Partner
             */
            LoadData();

            /*
             * From here on we have access to the Server Object and the DataSet is filled
             * with data.
             */
            FPartnerClass = FMainDS.PPartner[0].PartnerClass;

            // Determine whether Partner is of PartnerClass ORGANISATION and whether it is a Foundation
#if TODO
            DetermineOrganisationIsFoundation();
#endif

            // Setup Modulerelated Toggle Buttons in ToolBar
            SetupAvailableModuleDataItems(true, TModuleSwitchEnum.msNone);

            /*
             * Setup the bottom part of the screen - that is the TabSet that corresponds
             * with the initially selected TabPage
             */
            ucoLowerPart.MainDS = FMainDS;
            ucoLowerPart.PetraUtilsObject = FPetraUtilsObject;
            ucoLowerPart.PartnerEditUIConnector = FPartnerEditUIConnector;
            ucoLowerPart.InitiallySelectedTabPage = FInitiallySelectedTabPage;
            ucoLowerPart.InitialiseDelegateIsNewPartner(@IsNewPartner);
            ucoLowerPart.InitChildUserControl();

            switch (FCurrentModuleTabGroup)
            {
                case TModuleSwitchEnum.msPartner:

                    /*
                     * Set up ucoPartnerTabSet
                     */

                    // TODO 1

                    FPartnerTabSetInitialised = true;

                    break;

                case TModuleSwitchEnum.msPersonnel:

                    // TODO 2

                    break;

                case TModuleSwitchEnum.msFinance:

                    // TODO 2

                    break;
            }

            HookupPartnerEditDataChangeEvents(TPartnerEditTabPageEnum.petpDefault);

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.FormDataSavingStarted);

            // Hook up DataSaved Event to be able to run code after SaveChanges was run
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(this.FormDataSaved);

            /*
             * Set up top part of the Screen
             */

// TODO            ucoUpperPart.InitialiseDelegateMaintainWorkerField(new TDelegateMaintainWorkerField(MaintainWorkerField));
            ucoUpperPart.MainDS = FMainDS;
            ucoUpperPart.VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            ucoUpperPart.PartnerEditUIConnector = FPartnerEditUIConnector;

            // Show data in the top part of the screen
            ucoUpperPart.ShowData();

            // Set up screen caption
            SetScreenCaption();

            // Collapse upper Part, if user had it so last time
            FUppperPartInitiallyCollapsed = TUserDefaults.GetBooleanDefault(TUserDefaults.PARTNER_EDIT_UPPERPARTCOLLAPSED, false);

            if (FUppperPartInitiallyCollapsed)
            {
                ViewUpperScreenPartCollapsed(this, null);
            }
            else
            {
                ViewUpperScreenPartExpanded(this, null);
            }

            // Disable 'Local Partner Data' MenuItem if there are no Office Specific Data Labels available
            if (!FMainDS.MiscellaneousData[0].OfficeSpecificDataLabelsAvailable)
            {
                mniMaintainLocalPartnerData.Enabled = false;
            }

            FSubmitChangesContinue = false;
            ApplySecurity();

            // Need to do this manually  we disabled the automatic hookup in the Base Form because
            // we remove some TabPages, therefore the Controls on it, but the Events hooked
            // up to them would still be around and prevent a GC of the Form!
            FPetraUtilsObject.HookupAllControls();


            ucoUpperPart.Focus();
            this.Cursor = Cursors.Default;

//            MessageBox.Show("Data loaded successfully (might not look like it, but this screen isn't finished yet...!");
        }

        private void UcoUpperPart_PartnerClassMainDataChanged(System.Object Sender, TPartnerClassMainDataChangedEventArgs e)
        {
            FPetraUtilsObject.HasChanges = true;
#if TODO
            SetScreenCaption();
#endif
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

        #endregion


        #region Action Handlers

        #region File Menu

        private void FileNewPartner(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FileNewPartnerWithShepherdPerson(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FileNewPartnerWithShepherdFamily(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FileNewPartnerWithShepherdChurch(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FileNewPartnerWithShepherdOrganisation(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FileNewPartnerWithShepherdUnit(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FileDeactivatePartner(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FileDeletePartner(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FileSendEmail(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FilePrintPartner(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FilePrintSection(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FileExportPartner(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Maintain Menu

        private void MaintainAddresses(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainPartnerDetails(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainFoundationDetails(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainSubscriptions(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainSpecialTypes(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainContacts(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainFamilyMembers(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainRelationships(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainInterests(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainReminders(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainNotes(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainLocalPartnerData(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainWorkerField(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainIndividualData(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainDonorHistory(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainRecipientHistory(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainFinanceReports(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainBankAccounts(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainGiftReceipting(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainFinanceDetails(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region View Menu

        private void ViewUpperScreenPartExpanded(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ViewUpperScreenPartCollapsed(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ViewPartnerData(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ViewPersonnelData(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ViewFinanceData(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Help Menu

        private void HelpVideoTutorial(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion


        #region Private Methods

        private void LoadData()
        {
            PartnerEditTDSPPartnerLocationRow NewPartnerLocationRow;

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
        /// Determines which TabPage to show when the screen is loaded and which
        /// TabSet to initialise.
        /// </summary>
        /// <remarks>
        /// Verifies that TapPages are only opened for the Partners which use them.
        /// </remarks>
        private void DetermineInitiallySelectedTabPage()
        {
            switch (FShowTabPage)
            {
                case TPartnerEditTabPageEnum.petpDefault:

                    // TODO 2 oChristianK cPartner Edit / Tabs : Introduce a User Default that can specify which TabPage is the one the User wants to see as default.
                    FShowTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    FInitiallySelectedTabPage = FShowTabPage;
                    FCurrentModuleTabGroup = TModuleSwitchEnum.msPartner;
                    break;

                case TPartnerEditTabPageEnum.petpFoundationDetails:

                    if (!FFoundationDetailsEnabled)
                    {
                        FShowTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    }

                    FInitiallySelectedTabPage = FShowTabPage;
                    FCurrentModuleTabGroup = TModuleSwitchEnum.msPartner;
                    break;

                case TPartnerEditTabPageEnum.petpFamilyMembers:

                    if (!((FPartnerClass == "PERSON")
                          || (FPartnerClass == "FAMILY")))
                    {
                        FShowTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    }

                    FInitiallySelectedTabPage = FShowTabPage;
                    FCurrentModuleTabGroup = TModuleSwitchEnum.msPartner;
                    break;

                case TPartnerEditTabPageEnum.petpOfficeSpecific:

                    if (!FMainDS.MiscellaneousData[0].OfficeSpecificDataLabelsAvailable)
                    {
                        FShowTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    }

                    FInitiallySelectedTabPage = FShowTabPage;
                    FCurrentModuleTabGroup = TModuleSwitchEnum.msPartner;
                    break;

                case TPartnerEditTabPageEnum.petpAddresses:
                case TPartnerEditTabPageEnum.petpDetails:
                case TPartnerEditTabPageEnum.petpSubscriptions:
                case TPartnerEditTabPageEnum.petpPartnerTypes:
                case TPartnerEditTabPageEnum.petpNotes:
                    FInitiallySelectedTabPage = FShowTabPage;
                    FCurrentModuleTabGroup = TModuleSwitchEnum.msPartner;
                    break;

#if  SHOWUNFINISHEDTABS
                case TPartnerEditTabPageEnum.petpRelationships:
                case TPartnerEditTabPageEnum.petpContacts:
                case TPartnerEditTabPageEnum.petpReminders:
                case TPartnerEditTabPageEnum.petpInterests:
                    FInitiallySelectedTabPage = FShowTabPage;
                    FCurrentModuleTabGroup = TModuleSwitchEnum.msPartner;
                    break;

#else
                case TPartnerEditTabPageEnum.petpRelationships:
                case TPartnerEditTabPageEnum.petpContacts:
                case TPartnerEditTabPageEnum.petpReminders:
                case TPartnerEditTabPageEnum.petpInterests:
                    FShowTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    FInitiallySelectedTabPage = FShowTabPage;
                    FCurrentModuleTabGroup = TModuleSwitchEnum.msPartner;
                    break;
#endif
                default:

                    // Fallback
#if TODO
                    ucoPartnerTabSet.InitiallySelectedTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    FCurrentModuleTabGroup = TModuleSwitchEnum.msPartner;
#endif
                    break;
            }
        }

        /// <summary>
        /// Sets Module-related Toggle Buttons in ToolBar up
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

        private bool CheckSecurityOKToCreateNewPartner(Boolean AShowMessage)
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

        /// <summary>
        /// Hook up Events that enable the 'Save' ToolBarButton and File/Save menu entry.
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
        /// Sets the caption (title) of the screen.
        /// </summary>
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
                mniFileNewPartner.Enabled = false;
                tbbNewPartner.Enabled = false;
            }
        }

        /// <summary>
        /// This Procedure will get called from the SaveChanges procedure whenever a
        /// Save operation is finished (successful or unsuccesful).
        /// </summary>
        /// <param name="sender">The Object that throws this Event</param>
        /// <param name="e">Event Arguments. Success property is true if saving was successful,
        /// otherwise false.
        /// </param>
        /// <returns>void</returns>
        private void FormDataSaved(System.Object sender, TDataSavedEventArgs e)
        {
// TODO            ucoPartnerTabSet.DataSavedEventFired(e.Success);

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
        /// </summary>
        /// <param name="sender">The Object that throws this Event</param>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        private void FormDataSavingStarted(System.Object sender, System.EventArgs e)
        {
// TODO            ucoPartnerTabSet.DataSavingStartedEventFired();
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

        #endregion
    }
}