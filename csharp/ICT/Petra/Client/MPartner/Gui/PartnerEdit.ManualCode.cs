//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2015 by OM International
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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Specialized;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Controls;
using Ict.Common.DB;
using Ict.Common.DB.Exceptions;
using Ict.Common.Data;
using Ict.Common.Data.Exceptions;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MPartner.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.MPersonnel.Gui.Setup;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TFrmPartnerEdit
    {
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

        #region Fields

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary>holds the DataSet that contains most data that is used on the screen</summary>
        private PartnerEditTDS FMainDS;


        /// <summary>used for sending information back from the PetraServer while saving</summary>
        private DataSet FResponseDS;

        /// <summary>holds the PartnerKey for which the screen is opened</summary>
        private System.Int64 FPartnerKey;

        /// <summary>SiteKey of a PartnerLocation record for which the screen is opened</summary>
        private Int64 FSiteKeyForSelectingPartnerLocation;

        /// <summary>LocationKey of a PartnerLocation record for which the screen is opened</summary>
        private Int32 FLocationKeyForSelectingPartnerLocation;

        /// <summary>The Class of the Partner that the screen is working with</summary>
        private string FPartnerClass;

        private Int32 FNewPartnerFamilyLocationKey;
        private Int64 FNewPartnerSiteKey;
        private Int64 FNewPartnerPartnerKey;
        private Int64 FNewPartnerFamilyPartnerKey;
        private Int64 FNewPartnerFamilySiteKey;
        private String FNewPartnerPartnerClass;
        private string FNewPartnerDefaultPartnerClass = SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY);
        private String FNewPartnerCountryCode;
        private String FNewPartnerSiteCountryCode;
        private String FNewPartnerAcquisitionCode;
        private Boolean FNewPartnerPrivatePartner;
        private Boolean FNewPartnerShowNewPartnerDialog;
        private String FCallerContext = "";
        private TPartnerEditTabPageEnum FShowTabPage;
        private TPartnerEditTabPageEnum FInitiallySelectedTabPage;
        private Boolean FUppperPartInitiallyCollapsed;
        private Boolean FFoundationDetailsEnabled;
        private TPartnerEditScreenLogic.TModuleTabGroupEnum FCurrentModuleTabGroup;

        /// <summary>Stores any Exception that occurs while the screen is loading</summary>
        public static Exception UExceptionAtLoad = null;

        #endregion

        #region ResourceStrings

        // TODO 2 Replace with String.Format(Catalog.GetString("Hello {0}"), myname);

        private static readonly string StrScreenCaption = Catalog.GetString("Partner Edit");
        private static readonly string StrQueryUnitParent = Catalog.GetString(
            "All 'Units' MUST be assigned a 'Parent'.\r\nDo you wish to assign one now?");
        private static readonly string StrQueryUnitParentTitle = Catalog.GetString("Assign Parent in Unit Hierarchy?");
// TODO        private static readonly string StrQueryOverwriteAddress = Catalog.GetString(
// TODO            "Are you sure you want to replace the current address with the address\r\n" +
// TODO            "that you are about to find?\r\n\r\nWARNING:\r\n" +
// TODO            "If you choose 'Yes', history for the currently displayed address will be lost!\r\n" +
// TODO            "If the displayed address was valid, you should instead add a new address\r\n" +
// TODO            "and then enter an end (to) date for the old address.");
// TODO        private static readonly string StrQueryOverwriteAddressTitle = Catalog.GetString("Replace Current Address?");
        private static readonly string StrCannotDeletePartner = Catalog.GetString(
            "Cannot delete Partner that has unsaved changes.\r\n\r\n" +
            "Either save the changes that you have made, or close this Partner Edit screen without saving the data " +
            "and then delete the Partner from the Partner Module screen.");
        private static readonly string StrCannotDeletePartnerTitle = Catalog.GetString("Cannot delete Partner that has unsaved changes");
        private static readonly string StrCannotPrintPartner = Catalog.GetString(
            "Cannot print a Partner with unsaved changes.\r\n\r\n" +
            "Either save the changes that you have made, or close this Partner Edit screen without saving the data " +
            "and reopen.");
        private static readonly string StrCannotPrintPartnerTitle = Catalog.GetString("Cannot print a Partner with unsaved changes");
// TODO        private static readonly string StrDownloadVideoTutorialTitle = Catalog.GetString("Download Video Tutorial");
// TODO        private static readonly string StrDownloadVideoTutoriaManuallTitle = Catalog.GetString("Manual Download of Video Tutorial");
// TODO        private static readonly string StrVideoTutorialTitle = Catalog.GetString("Video Tutorial for Partner Edit Screen");
// TODO        private static readonly string StrVideoTutorialNotFound = Catalog.GetString(
// TODO            "The Video Tutorial for Partner Edit Screen cannot be found on your system.\r\n" +
// TODO            "(OpenPetra is looking in '{0}' for a file named '{1}').\r\n\r\n" +
// TODO            "The video can also be download from the Internet. Choose 'Yes' to download it and view it,\r\n" +
// TODO            "or choose 'No' to not download the video.");
// TODO        private static readonly string StrVideoTutorialLaunchFailed = Catalog.GetString(
// TODO            "There was a problem launching the Video Tutorial application. (Details can be found in the log file.)");
// TODO        private static readonly string StrVideoTutorialWebBRowserLaunched = Catalog.GetString(
// TODO            "The web browser should have been launched and offer the download of the file.");
// TODO        private static readonly string StrVideoTutorialWebBRowserLaunchFailed = Catalog.GetString(
// TODO            "There was a problem launching Internet Explorer. (Details can be found in the log file.)\r\n\r\n" +
// TODO            "To download the Video Tutorial, launch your web browser and enter the following address:\r\n" + "   ");
// TODO        private static readonly string StrVideoTutorialDownloadInstructions = Catalog.GetString(
// TODO            "Please save the file in the following folder:\r\n   {0}\r\n\r\n" +
// TODO            "After the download is finished: choose 'Yes' to start the Video Tutorial from that folder,\r\n" +
// TODO            "or choose 'No' to return to the Partner Edit screen.");
// TODO        private static readonly string StrDeactivatePartnerTitle = Catalog.GetString("Deactivate Partner");
// TODO        private static readonly string StrDeactivatePartnerActionCancelled = Catalog.GetString(
// TODO            "Deactivate Partner procedure cancelled - no data was changed.");
// TODO        private static readonly string StrDeactivatePartnerSuccess = Catalog.GetString("Deactivate Partner procedure finished sucessfully.");
// TODO        private static readonly string StrDeactivatePartnerStatusNotChanged = Catalog.GetString(
// TODO            "Partner Status wasn't changed - it was already set to '{0}'.");

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

        /// <summary>The Class of the Partner that the screen is working with (read-only!).</summary>
        /// <remarks>Available only after data has been loaded from the server!</remarks>
        public TPartnerClass PartnerClass
        {
            get
            {
                return SharedTypes.PartnerClassStringToEnum(FPartnerClass);
            }
        }

        /// <summary>
        /// set this property before showing the screen, or use SetParameters
        /// </summary>
        public TPartnerEditTabPageEnum ShowTabPage
        {
            get
            {
                return FShowTabPage;
            }
            set
            {
                FShowTabPage = value;
            }
        }

        /// <summary>Name of the currently active Tab Page in the lower part of the screen (in any of the TabGroups). Read-only.</summary>
        public string CurrentlySelectedTabPageName
        {
            get
            {
                return Enum.GetName(typeof(TPartnerEditTabPageEnum), ucoLowerPart.CurrentlySelectedTabPage);
            }
        }

        /// <summary>
        /// Used in the 'Form Messaging' implementation in <see cref="SaveChanges(ref PartnerEditTDS)"></see>.
        /// </summary>
        /// <description>
        /// Several running instances of the same screen (e.g. Partner Find screen)
        /// can distinguish between messages for certain instances of the screen through that.
        /// </description>
        public string CallerContext
        {
            get
            {
                return FCallerContext;
            }

            set
            {
                FCallerContext = value;
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
        /// <param name="ADefaultPartnerClass">The default Partner Class of the new partner (Family if not specified.)</param>
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
            TPartnerEditTabPageEnum AShowTabPage,
            string ADefaultPartnerClass = "FAMILY")
        {
            if (AScreenMode != TScreenMode.smNew)
            {
                throw new ArgumentException("AScreenMode parameter must be TScreenMode.smNew if this overload is used");
            }

            FPetraUtilsObject.ScreenMode = AScreenMode;
            FNewPartnerSiteKey = ASiteKey;
            FNewPartnerPartnerKey = APartnerKey;
            FNewPartnerPartnerClass = APartnerClass;
            FNewPartnerDefaultPartnerClass = ADefaultPartnerClass;
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
        /// <param name="ADefaultPartnerClass">The default Partner Class of the new partner (Family if not specified.)</param>
        public void SetParameters(TScreenMode AScreenMode,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String ACountryCode,
            String AAcquisitionCode,
            string ADefaultPartnerClass = "FAMILY")
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
                TPartnerEditTabPageEnum.petpDefault,
                ADefaultPartnerClass);
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
        /// Selects the given contact log.
        /// </summary>
        /// <param name="AContactLogID">Contact Log identifier.</param>
        public void SelectContactLog(string AContactLogID)
        {
            ucoLowerPart.SelectContactLog(AContactLogID);
        }

        /// <summary>
        /// Initializes a new instance of the TFrmPartnerEdit class (constructor)
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

        private void GetDataFromControlsManual(PPartnerRow ARow)
        {
            GetDataFromControls();
        }

        private void GetDataFromControls()
        {
            ucoUpperPart.GetDataFromControls();
            ucoLowerPart.GetDataFromControls();
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        private bool ValidateAllData()
        {
            bool ReturnValue = false;

            // Record a new Data Validation Run. (All TVerificationResults/TScreenVerificationResults that are created during this 'run' are associated with this 'run' through that.)
            FPetraUtilsObject.VerificationResultCollection.RecordNewDataValidationRun();

            // Perform validation in UserControls, too
            ucoUpperPart.ValidateAllData(TErrorProcessingMode.Epm_IgnoreNonCritical);
            ucoLowerPart.ValidateAllData(TErrorProcessingMode.Epm_IgnoreNonCritical);

            bool ignoreWarnings = !FPetraUtilsObject.VerificationResultCollection.HasCriticalErrors;
            ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(false, FPetraUtilsObject.VerificationResultCollection,
                this.GetType(), null, ignoreWarnings);

            if (ReturnValue)
            {
                // Remove a possibly shown Validation ToolTip as the data validation succeeded
                FPetraUtilsObject.ValidationToolTip.RemoveAll();
            }

            return ReturnValue;
        }

        /// <summary>
        /// Interface method to get the number of changed records and specify a message to incorporate into the 'Do you want to save?' message box
        /// </summary>
        /// <param name="AMessage">An optional message to display.  If the parameter is an empty string a default message will be used</param>
        /// <returns>The number of changed records.  Return -1 to imply 'unknown'.</returns>
        public int GetChangedRecordCount(out string AMessage)
        {
            GetDataFromControls();
            return FPetraUtilsObject.GetChangedRecordCount(FMainDS, out AMessage);
        }

        /// <summary>
        /// needed for the interface
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            return SaveChanges(ref FMainDS);
        }

        /// <summary>
        /// Determines the changes in the screen's dataset and submits them to the
        /// Server.
        /// </summary>
        /// <param name="AInspectDS">The screen's dataset
        /// </param>
        /// <returns>True if saving of data succeeded, otherwise false.</returns>
        private Boolean SaveChanges(ref PartnerEditTDS AInspectDS)
        {
            Boolean ReturnValue = false;
            PartnerEditTDS SubmitDS = null;
            TSubmitChangesResult SubmissionResult;

            TVerificationResultCollection VerificationResult;
            TVerificationResult VerificationResultItem;

            System.Windows.Forms.DialogResult UnitParentAssignment;
            int RowIndex;
            int NumRows;
            Int32 MaxColumn;
            Boolean SavedPartnerIsNewPartner = false;
            bool PartnerAttributesOrRelationsChanged = false;
            System.Int32 ChangedColumns;
#if SHOWCHANGES
            String DebugMessage;
#endif
#if DATASETDEBUGGING
            Int16 TmpRowCounter;
            string TmpDebugString = String.Empty;
#endif
            // Be sure to fire the OnLeave event on the active control of any user control
            FPetraUtilsObject.ForceOnLeaveForActiveControl();

            FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

            // Don't allow saving if user is still editing a Detail of a List
            if (FPetraUtilsObject.InDetailEditMode())
            {
                ReturnValue = false;
                return ReturnValue;
            }

            // Make sure that DataBinding writes the value of the active Control to the underlying DataSource!
            TDataBinding.EnsureDataChangesAreStored(this);

            GetDataFromControls();

            // Clear any validation errors so that the following call to ValidateAllData starts with a 'clean slate'.
            FPetraUtilsObject.VerificationResultCollection.Clear();

            if (ValidateAllData())
            {
                // Ask the user about non-critical warnings, if they are the only 'errors' in the collection
                if (FPetraUtilsObject.VerificationResultCollection.HasOnlyNonCriticalErrors
                    && (TDataValidation.ProcessAnyDataValidationWarnings(FPetraUtilsObject.VerificationResultCollection,
                            MCommonResourcestrings.StrFormSaveDataAnywayQuestion, this.GetType()) == false))
                {
                    return false;
                }

                // Fire the DataSavingValidated event, which is the last chance to cancel the save
                System.ComponentModel.CancelEventArgs eCancel = new System.ComponentModel.CancelEventArgs(false);
                FPetraUtilsObject.OnDataSavingValidated(this, eCancel);

                if (eCancel.Cancel == true)
                {
                    return false;
                }

                foreach (DataTable InspectDT in AInspectDS.Tables)
                {
                    foreach (DataRow InspectDR in InspectDT.Rows)
                    {
                        InspectDR.EndEdit();
                    }
                }

                if (FPetraUtilsObject.HasChanges)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataInProgress);
                    this.Cursor = Cursors.WaitCursor;

                    /* $IFDEF SHOWCHANGES MessageBox.Show('SaveChanges: AInspectDS.PLocation.Rows[0].HasVersion(DataRowVersion.Original): ' + AInspectDS.PLocation.Rows[0].HasVersion(DataRowVersion.Original).ToString + '; LocationKey: ' +
                     *AInspectDS.PLocation.Row[0].LocationKey.ToString); $ENDIF */

                    /* $IFDEF SHOWCHANGES MessageBox.Show('SaveChanges: AInspectDS.PLocation.Rows[1].HasVersion(DataRowVersion.Original): ' + AInspectDS.PLocation.Rows[1].HasVersion(DataRowVersion.Original).ToString + '; LocationKey: ' +
                     *AInspectDS.PLocation.Row[1].LocationKey.ToString); $ENDIF */
                    if (!FPetraUtilsObject.SubmitChangesContinue)
                    {
                        foreach (DataTable InspectDT in AInspectDS.Tables)
                        {
                            // MessageBox.Show('inspectDataTable: ' + InspectDT.ToString);
                            if ((InspectDT.TableName != PLocationTable.GetTableName()) && (InspectDT.TableName != PPartnerLocationTable.GetTableName()))
                            {
                                MaxColumn = InspectDT.Columns.Count;
                                ChangedColumns = DataUtilities.AcceptChangesForUnmodifiedRows(InspectDT, MaxColumn);

                                if (ChangedColumns != 0)
                                {
#if SHOWCHANGES
                                    MessageBox.Show(InspectDT.TableName + " - changed colums: " + ChangedColumns.ToString());
#endif
                                }
                            }
                            else if (InspectDT.TableName == PLocationTable.GetTableName())
                            {
                                MaxColumn = new PLocationTable().Columns.Count;

                                // MessageBox.Show('PLocation MaxColumn: ' + MaxColumn.ToString);
                                ChangedColumns = DataUtilities.AcceptChangesForUnmodifiedRows(AInspectDS.PLocation, MaxColumn, true);

                                if (ChangedColumns != 0)
                                {
#if SHOWCHANGES
                                    MessageBox.Show(PLocationTable.GetTableName() + " - changed colums: " + ChangedColumns.ToString());
#endif
                                }
                            }
                            else
                            {
                                MaxColumn = new PPartnerLocationTable().Columns.Count;

                                // MessageBox.Show('PPartnerLocation MaxColumn: ' + MaxColumn.ToString);
                                ChangedColumns = DataUtilities.AcceptChangesForUnmodifiedRows(AInspectDS.PPartnerLocation,
                                    MaxColumn,
                                    true);

                                if (ChangedColumns != 0)
                                {
#if SHOWCHANGES
                                    MessageBox.Show(PPartnerLocationTable.GetTableName() + " - changed colums: " + ChangedColumns.ToString());
#endif
                                }
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

                                    for (int Counter1 = 0; Counter1 <= MaxColumn - 1; Counter1 += 1)
                                    {
                                        if ((InspectDR.RowState == DataRowState.Added) || (InspectDR != InspectDR[Counter1, DataRowVersion.Current]))
                                        {
                                            ChangedColumns = ChangedColumns + 1;
                                            DebugMessage = DebugMessage + "  " + (InspectDT.Columns[Counter1].ColumnName).ToString() + ": " +
                                                           InspectDR[Counter1, DataRowVersion.Current].ToString() + Environment.NewLine;
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

                    // If changes have been made to pm_staff_data (commitments) then update the partner's family's Gift Destination records
                    TGiftDestination GiftDestination = new TGiftDestination();

                    if (GiftDestination.UpdateGiftDestination(ref AInspectDS))
                    {
                        // set the gift destination on this screen and all other open partner edit screens for the same family
                        SetGiftDestinationAllScreens();
                    }

                    SubmitDS = AInspectDS.GetChangesTyped(true);

                    if (SubmitDS == null)
                    {
                        // There is nothing to be saved.
                        // Update UI
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);
                        this.Cursor = Cursors.Default;

                        // We don't have unsaved changes anymore
                        FPetraUtilsObject.DisableSaveButton();

                        return true;
                    }

                    if ((SubmitDS.Tables.Contains(PPartnerAttributeTable.GetTableName()))
                        || (SubmitDS.Tables.Contains(PPartnerRelationshipTable.GetTableName())))
                    {
                        PartnerAttributesOrRelationsChanged = true;
                    }

#if DATASETDEBUGGING
                    TLogging.Log("Before submitting data to the Server. Client DataSet: " + SubmitDS.GetXml());
#endif
                    // Submit changes to the PETRAServer
                    try
                    {
                        if (!FPetraUtilsObject.SubmitChangesContinue)
                        {
                            FResponseDS = null;
                            SubmissionResult = FPartnerEditUIConnector.SubmitChanges(ref SubmitDS, ref FResponseDS, out VerificationResult);
                        }
                        else
                        {
                            SubmissionResult = FPartnerEditUIConnector.SubmitChangesContinue(out SubmitDS, ref FResponseDS, out VerificationResult);
                        }
                    }
                    catch (ESecurityDBTableAccessDeniedException Exp)
                    {
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                        this.Cursor = Cursors.Default;

                        TMessages.MsgSecurityException(Exp, this.GetType());

                        ReturnValue = false;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }
                    catch (EDBConcurrencyException Exp)
                    {
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                        this.Cursor = Cursors.Default;

                        TMessages.MsgDBConcurrencyException(Exp, this.GetType());

                        ReturnValue = false;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }
                    catch (Exception)
                    {
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                        this.Cursor = Cursors.Default;

                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        throw;
                    }

                    switch (SubmissionResult)
                    {
                        case TSubmitChangesResult.scrOK:

                            SavedPartnerIsNewPartner = IsNewPartner(AInspectDS);

                            // MessageBox.Show('DUMMY: ' + (SubmitDS.Tables['Locations'].Rows[0]['DUMMY']).ToString() );
                            if ((SharedTypes.PartnerClassStringToEnum(AInspectDS.PPartner[0].PartnerClass) == TPartnerClass.UNIT)
                                && SavedPartnerIsNewPartner)
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
                                    TFrmUnitHierarchy HierarchyForm = new TFrmUnitHierarchy(this.ParentForm);

                                    HierarchyForm.Show();
                                    HierarchyForm.ShowThisUnit(AInspectDS.PPartner[0].PartnerKey);
                                }
                            }

#if DATASETDEBUGGING
                            TLogging.Log("After submtting returned Server DataSet: " + SubmitDS.GetXml());
                            MessageBox.Show("Before CleanupAddressesBeforeMerge");
#endif
                            // Get rid of any new Addresses; they are returned back with different LocationKeys (based on a Sequence)
                            ucoLowerPart.CleanupAddressesBeforeMerge();
#if DATASETDEBUGGING
                            MessageBox.Show("After CleanupAddressesBeforeMerge");

                            if ((SubmitDS.PLocation != null)
                                && (SubmitDS.PLocation.Rows.Count > 0))
                            {
                                MessageBox.Show(
                                    "Location[0] LocationKey: " + SubmitDS.PLocation[0].LocationKey.ToString() +
                                    "; PartnerLocation[0] LocationKey: " +
                                    SubmitDS.PPartnerLocation[0].LocationKey.ToString());
                            }
#endif

                            // Delete all added Rows in the original dataset. They will automatically
                            // be put back in with the Merge. If added Rows are deleted they will
                            // be removed from the Row collection on the server. In that case the
                            // client will not be aware of that. For modified Rows: if they are
                            // deleted on the server they will not be taken out of the collection
                            // but come back to the client as being marked as 'Deleted'. Therefore
                            // with deleting the added Rows beforehand we are making sure that
                            // the dataset on the client is synchronized with the one on the server.
                            foreach (DataTable t in AInspectDS.Tables)
                            {
                                if ((t == AInspectDS.PBankingDetails)
                                    || (t == AInspectDS.PPartnerBankingDetails)
                                    || (t == AInspectDS.PDataLabelValuePartner))
                                {
                                    NumRows = t.Rows.Count;

                                    for (RowIndex = NumRows - 1; RowIndex >= 0; RowIndex -= 1)
                                    {
                                        DataRow InspectDR = t.Rows[RowIndex];

                                        // delete all added Rows.
                                        if (InspectDR.RowState == DataRowState.Added)
                                        {
                                            InspectDR.Delete();
                                        }
                                    }
                                }
                            }

                            // Update UserDefaults, if necessary
                            MaintainUserDefaults();

                            if (SubmitDS.Tables.Contains(PPartnerTaxDeductiblePctTable.GetTableName()))
                            {
                                UpdateTaxDeductiblePct();
                            }

                            // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                            AInspectDS.AcceptChanges();
#if DATASETDEBUGGING
                            TLogging.Log("After getting rid now of any deleted columns   Client DataSet: " + AInspectDS.GetXml());

                            if (AInspectDS.Tables.Contains(PLocationTable.GetTableName()))
                            {
                                for (TmpRowCounter = 0; TmpRowCounter < AInspectDS.Tables[PLocationTable.GetTableName()].Rows.Count; TmpRowCounter++)
                                {
                                    TmpDebugString = TmpDebugString + PLocationTable.GetTableName() + ".Row[" + TmpRowCounter.ToString() +
                                                     "]: PLocationKey: " +
                                                     AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpRowCounter][PLocationTable.
                                                                                                                          GetLocationKeyDBName()].
                                                     ToString() + "(); PSiteKey: " +
                                                     AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpRowCounter][PLocationTable.
                                                                                                                          GetSiteKeyDBName()].
                                                     ToString() + "(); Modification TimeStamp: " +
                                                     AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpRowCounter][PLocationTable.
                                                                                                                          GetModificationIdDBName()].
                                                     ToString() + "\r\n";
                                }
                            }

                            if (AInspectDS.Tables.Contains(PPartnerLocationTable.GetTableName()))
                            {
                                TmpDebugString = TmpDebugString + "\r\n";

                                for (TmpRowCounter = 0;
                                     TmpRowCounter < AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows.Count;
                                     TmpRowCounter++)
                                {
                                    TmpDebugString = TmpDebugString + PPartnerLocationTable.GetTableName() + ".Row[" + TmpRowCounter.ToString() +
                                                     "]: PLocationKey: " +
                                                     AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter][
                                        PPartnerLocationTable.GetLocationKeyDBName()].ToString() + "(); PSiteKey: " +
                                                     AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter][
                                        PPartnerLocationTable.GetSiteKeyDBName()].ToString() + "(); PPartnerKey: " +
                                                     AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter][
                                        PPartnerLocationTable.GetPartnerKeyDBName()].ToString() + "(); Modification TimeStamp: " +
                                                     AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter][
                                        PPartnerLocationTable.GetModificationIdDBName()].ToString() + "\r\n";
                                }

                                MessageBox.Show(TmpDebugString, "DEBUG: PLocation / PPartnerLocation local contents  [#1]");
                            }
#endif
                            // Merge back with data from the Server (eg. for getting Sequence values)
                            AInspectDS.Merge(SubmitDS, false);
#if DATASETDEBUGGING
                            TLogging.Log("After Merge back with data from the Server  Server DataSet: " + SubmitDS.GetXml());
                            TLogging.Log("After Merge back with data from the Server  Client DataSet: " + AInspectDS.GetXml());

                            if (AInspectDS.Tables.Contains(PLocationTable.GetTableName()))
                            {
                                TmpDebugString = "";

                                for (TmpRowCounter = 0; TmpRowCounter < AInspectDS.Tables[PLocationTable.GetTableName()].Rows.Count; TmpRowCounter++)
                                {
                                    TmpDebugString = TmpDebugString + PLocationTable.GetTableName() + ".Row[" + TmpRowCounter.ToString() +
                                                     "]: PLocationKey: " +
                                                     AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpRowCounter][PLocationTable.
                                                                                                                          GetLocationKeyDBName()].
                                                     ToString() + "(); PSiteKey: " +
                                                     AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpRowCounter][PLocationTable.
                                                                                                                          GetSiteKeyDBName()].
                                                     ToString() + "(); Modification TimeStamp: " +
                                                     AInspectDS.Tables[PLocationTable.GetTableName()].Rows[TmpRowCounter][PLocationTable.
                                                                                                                          GetModificationIdDBName()].
                                                     ToString() + "\r\n";
                                }
                            }

                            if (AInspectDS.Tables.Contains(PPartnerLocationTable.GetTableName()))
                            {
                                TmpDebugString = TmpDebugString + "\r\n";

                                for (TmpRowCounter = 0;
                                     TmpRowCounter < AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows.Count;
                                     TmpRowCounter++)
                                {
                                    TmpDebugString = TmpDebugString + PPartnerLocationTable.GetTableName() + ".Row[" + TmpRowCounter.ToString() +
                                                     "]: PLocationKey: " +
                                                     AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter][
                                        PPartnerLocationTable.GetLocationKeyDBName()].ToString() + "(); PSiteKey: " +
                                                     AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter][
                                        PPartnerLocationTable.GetSiteKeyDBName()].ToString() + "(); PPartnerKey: " +
                                                     AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter][
                                        PPartnerLocationTable.GetPartnerKeyDBName()].ToString() + "(); Modification TimeStamp: " +
                                                     AInspectDS.Tables[PPartnerLocationTable.GetTableName()].Rows[TmpRowCounter][
                                        PPartnerLocationTable.GetModificationIdDBName()].ToString() + "\r\n";
                                }

                                MessageBox.Show(TmpDebugString, "DEBUG: PLocation / PPartnerLocation local contents  [#2]");
                            }
#endif

                            ucoUpperPart.UpdateStatusUpdatedDate();  // this is to refresh 'Status Updated' if it has been changed in the ComboBox and then saved...

                            ucoLowerPart.RefreshRecordsAfterMerge();
                            ucoLowerPart.RefreshPersonnelDataAfterMerge(PartnerAttributesOrRelationsChanged);

                            // Call AcceptChanges so that we don't have any changed data anymore!
                            AInspectDS.AcceptChanges();
#if DATASETDEBUGGING
                            TLogging.Log("After calling AcceptChanges on the Client DataSet: " + AInspectDS.GetXml());
#endif
                            // Update UI
                            FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataSuccessful);
                            this.Cursor = Cursors.Default;
                            EnableSave(false);

                            // If the screen was opened for a NEW Partner, remove the 'NEW:' indicator from the Window Title Bar and
                            // set this Partner to be the "Last Used Partner".
                            if (SavedPartnerIsNewPartner)
                            {
                                FPetraUtilsObject.HasNewData = false;
                                SetScreenCaption();

                                // Set Partner to be the "Last Used Partner"
                                TUserDefaults.NamedDefaults.SetLastPartnerWorkedWith(AInspectDS.PPartner[0].PartnerKey,
                                    TLastPartnerUse.lpuMailroomPartner, SharedTypes.PartnerClassStringToEnum(FPartnerClass));
                            }

                            // We don't have unsaved changes anymore
                            FPetraUtilsObject.DisableSaveButton();
                            FPetraUtilsObject.SubmitChangesContinue = false;

                            // Assign PartnerKey. This is needed in case this was a new Partner before saving!
                            FPartnerKey = AInspectDS.PPartner[0].PartnerKey;

                            ReturnValue = true;
                            FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));

                            if ((VerificationResult != null)
                                && (VerificationResult.HasCriticalOrNonCriticalErrors))
                            {
                                TDataValidation.ProcessAnyDataValidationErrors(false, VerificationResult,
                                    this.GetType(), null);
                            }

                            break;

                        case TSubmitChangesResult.scrError:
                            this.Cursor = Cursors.Default;

                            if (!(VerificationResult.Contains((System.Object) "Location Change Promotion: Information")))
                            {
                                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataErrorOccured);

                                TDataValidation.ProcessAnyDataValidationErrors(false, VerificationResult,
                                    this.GetType(), null);
                            }
                            else
                            {
                                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataCancelled);

                                VerificationResultItem = (TVerificationResult)VerificationResult.FindBy(
                                    (object)"Location Change Promotion: Information");

                                MessageBox.Show(VerificationResultItem.ResultText,
                                    VerificationResultItem.ResultTextCaption, MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            }

                            FPetraUtilsObject.SubmitChangesContinue = false;

                            ReturnValue = false;
                            FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                            break;

                        case TSubmitChangesResult.scrNothingToBeSaved:

                            // If there were no changes discovered then still need to call AcceptChanges to get rid now of
                            // any deleted columns
                            AInspectDS.AcceptChanges();

                            // Update UI
                            this.Cursor = Cursors.Default;
                            FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);

                            // We don't have unsaved changes anymore
                            EnableSave(false);
                            FPetraUtilsObject.HasChanges = false;

                            ReturnValue = true;
                            FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                            break;

                        case TSubmitChangesResult.scrInfoNeeded:
                            this.Cursor = Cursors.Default;
#if DATASETDEBUGGING
                            for (int Counter2 = 0; Counter2 < FResponseDS.Tables.Count; Counter2++)
                            {
                                MessageBox.Show(
                                    "Table #" + Counter2.ToString() + "'s name is " + FResponseDS.Tables[Counter2].TableName + ". It has " +
                                    FResponseDS.Tables[Counter2].Rows.Count.ToString() + " Rows. Type: " +
                                    FResponseDS.Tables[Counter2].GetType().ToString());
                            }

                            if (FResponseDS.Tables.Contains(MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME))
                            {
                                MessageBox.Show("FResponseDS Type: " + FResponseDS.GetType().ToString());
                                MessageBox.Show(MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME + " Type: " +
                                    FResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].GetType().ToString());
                                MessageBox.Show(
                                    "FResponseDS.Tables[" + MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME + "].Rows.Count: " +
                                    FResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].Rows.Count.ToString());
                                // Check if there is a Parameter Row that is not yet processed
                                DataView ExistingLocationParametersDV = new DataView(
                                    FResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME],
                                    PartnerAddressAggregateTDSSimilarLocationParametersTable.GetAnswerProcessedClientSideDBName() +
                                    " = false", "", DataViewRowState.CurrentRows);

                                if (ExistingLocationParametersDV.Count > 0)
                                {
                                    MessageBox.Show("ExistingLocationParametersDV.Count: " + ExistingLocationParametersDV.Count.ToString());
                                    MessageBox.Show(
                                        "Row[0].LocationKey: " +
                                        (ExistingLocationParametersDV[0].Row as PartnerAddressAggregateTDSSimilarLocationParametersRow).LocationKey.
                                        ToString());
                                }
                            }
#endif
                            ucoLowerPart.SimilarLocationsProcessing(
                            (PartnerAddressAggregateTDSSimilarLocationParametersTable)FResponseDS.Tables[MPartnerConstants.
                                                                                                         EXISTINGLOCATIONPARAMETERS_TABLENAME]);
#if DATASETDEBUGGING
                            if (FResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME].Rows.Count > 0)
                            {
                                MessageBox.Show("Reuse?: " + (FResponseDS.Tables[MPartnerConstants.EXISTINGLOCATIONPARAMETERS_TABLENAME]
                                                              as PartnerAddressAggregateTDSSimilarLocationParametersTable)[0].AnswerReuse.ToString());
                            }

                            if (FResponseDS.Tables.Contains(MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME))
                            {
                                MessageBox.Show(MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME + " Type: " +
                                    FResponseDS.Tables[MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME].GetType().ToString());
                                MessageBox.Show(
                                    "FResponseDS.Tables[" + MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME + "].Rows.Count: " +
                                    FResponseDS.Tables[MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME].Rows.Count.ToString());
                                //Check if there is a Parameter Row that is not yet processed
                                DataView AddressAddedOrChangedParametersDV = new DataView(
                                    FResponseDS.Tables[MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME],
                                    PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable.GetAnswerProcessedClientSideDBName() +
                                    " = false", "", DataViewRowState.CurrentRows);

                                if (AddressAddedOrChangedParametersDV.Count > 0)
                                {
                                    MessageBox.Show("AddressAddedOrChangedParametersDV.Count: " + AddressAddedOrChangedParametersDV.Count.ToString());
                                    MessageBox.Show(
                                        "Row[0].LocationKey: " +
                                        (AddressAddedOrChangedParametersDV[0].Row as PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow).
                                        LocationKey.
                                        ToString());
                                }
                            }
#endif
                            ucoLowerPart.AddressAddedOrChangedProcessing(
                            (PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable)
                            FResponseDS.Tables[MPartnerConstants.ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME],
                            (PartnerAddressAggregateTDSChangePromotionParametersTable)
                            FResponseDS.Tables[MPartnerConstants.ADDRESSCHANGEPROMOTIONPARAMETERS_TABLENAME]);
#if DATASETDEBUGGING
                            MessageBox.Show("After AddressAddedOrChangedProcessing");
#endif
                            // Recursive call!
                            FPetraUtilsObject.SubmitChangesContinue = true;
                            ReturnValue = SaveChanges(ref AInspectDS);

                            return ReturnValue;
                    }
                }
                else
                {
                    // Update UI
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);
                    this.Cursor = Cursors.Default;
                    EnableSave(false);

                    // We don't have unsaved changes anymore
                    FPetraUtilsObject.HasChanges = false;

                    ReturnValue = true;
                    FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                }
            }
            else
            {
                FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(false));
            }

            // if the partner has been saved then broadcast a message to any listening forms to inform them
            if (ReturnValue)
            {
                TFormsMessage BroadcastMessage;
                TFormsMessage BroadcastMessageGiftDestination;
                String PartnerShortNameForBroadcast;
                long FamilyPartnerKey;
                String FamilyShortName;

                if (SavedPartnerIsNewPartner)
                {
                    BroadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcNewPartnerSaved,
                        FCallerContext);
                }
                else
                {
                    BroadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcExistingPartnerSaved,
                        FCallerContext);
                }

                if (!FMainDS.PPartner[0].IsPartnerShortNameNull())
                {
                    PartnerShortNameForBroadcast = FMainDS.PPartner[0].PartnerShortName;
                }
                else
                {
                    PartnerShortNameForBroadcast = String.Empty;
                }

                BroadcastMessage.SetMessageDataPartner(
                    FPartnerKey,
                    SharedTypes.PartnerClassStringToEnum(FPartnerClass),
                    PartnerShortNameForBroadcast,
                    FMainDS.PPartner[0].StatusCode);

                TFormsList.GFormsList.BroadcastFormMessage(BroadcastMessage);

                if ((SubmitDS.PPartnerGiftDestination != null) && (SubmitDS.PPartnerGiftDestination.Rows.Count > 0))
                {
                    BroadcastMessageGiftDestination = new TFormsMessage(TFormsMessageClassEnum.mcPersonnelCommitmentChanged,
                        FCallerContext);

                    if (FPartnerClass == TPartnerClass.FAMILY.ToString())
                    {
                        FamilyPartnerKey = FPartnerKey;
                        FamilyShortName = PartnerShortNameForBroadcast;
                    }
                    else
                    {
                        FamilyPartnerKey = ((PPersonRow)FMainDS.PPerson.Rows[0]).FamilyKey;
                        FamilyShortName = String.Empty;
                    }

                    BroadcastMessageGiftDestination.SetMessageDataPartner(
                        FamilyPartnerKey,
                        TPartnerClass.FAMILY,
                        FamilyShortName,
                        FMainDS.PPartner[0].StatusCode);

                    TFormsList.GFormsList.BroadcastFormMessage(BroadcastMessageGiftDestination);
                }
            }

            return ReturnValue;
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

        private void UpdateTaxDeductiblePct()
        {
            decimal OriginalPct = 100;
            decimal NewPct = 100;
            DateTime OriginalValidFrom = DateTime.MinValue;
            DateTime NewValidFrom = DateTime.MinValue;

            // if a datarow already exists
            if (FMainDS.PPartnerTaxDeductiblePct.Rows[0].HasVersion(DataRowVersion.Original))
            {
                OriginalPct = Convert.ToDecimal(
                    FMainDS.PPartnerTaxDeductiblePct.Rows[0][PPartnerTaxDeductiblePctTable.GetPercentageTaxDeductibleDBName(),
                                                             DataRowVersion.Original]);
                OriginalValidFrom = Convert.ToDateTime(
                    FMainDS.PPartnerTaxDeductiblePct.Rows[0][PPartnerTaxDeductiblePctTable.GetDateValidFromDBName(), DataRowVersion.Original]);
            }

            int i = 0;

            // there will be either 0 or 1 rows that are not marked as deleted
            while (FMainDS.PPartnerTaxDeductiblePct.Rows[i].RowState == DataRowState.Deleted
                   && i < (FMainDS.PPartnerTaxDeductiblePct.Rows.Count - 1))
            {
                i++;
            }

            // if Tax Deductible Pct has not just been removed
            if (FMainDS.PPartnerTaxDeductiblePct.Rows[i].HasVersion(DataRowVersion.Current))
            {
                NewPct = Convert.ToDecimal(
                    FMainDS.PPartnerTaxDeductiblePct.Rows[i][PPartnerTaxDeductiblePctTable.GetPercentageTaxDeductibleDBName(),
                                                             DataRowVersion.Current]);
                NewValidFrom = Convert.ToDateTime(
                    FMainDS.PPartnerTaxDeductiblePct.Rows[i][PPartnerTaxDeductiblePctTable.GetDateValidFromDBName(), DataRowVersion.Current]);
            }

            // if either pct or date has changed
            if ((NewPct != OriginalPct) || (OriginalValidFrom != NewValidFrom))
            {
                DataTable GiftTotals;

                // find gifts that could be affected by this change
                if (TRemote.MFinance.Gift.WebConnectors.IsPartnerARecipient(FPartnerKey, out GiftTotals, NewPct, NewValidFrom))
                {
                    string StartMsg = string.Empty;
                    string PermissionGifts = string.Empty;
                    string NoPermissionGifts = string.Empty;
                    bool UnpostedGifts = false;
                    bool UnpostedGiftInLedgerWithNoPermission = false;
                    bool PostedGifts = false;
                    bool PostedGiftInLedgerWithNoPermission = false;

                    // check what gifts we have - posted/unposted - ledger permission/no ledger permission
                    foreach (DataRow Row in GiftTotals.Rows)
                    {
                        if (Convert.ToInt32(Row["Unposted"]) > 0)
                        {
                            if (UserInfo.GUserInfo.IsInLedger(Convert.ToInt32(Row["LedgerNumber"])))
                            {
                                UnpostedGifts = true;
                            }
                            else
                            {
                                UnpostedGiftInLedgerWithNoPermission = true;
                            }
                        }

                        if (Convert.ToInt32(Row["Posted"]) > 0)
                        {
                            if (UserInfo.GUserInfo.IsInLedger(Convert.ToInt32(Row["LedgerNumber"])))
                            {
                                PostedGifts = true;
                            }
                            else
                            {
                                PostedGiftInLedgerWithNoPermission = true;
                            }
                        }
                    }

                    // if pct (and date) has changed
                    if (NewPct != OriginalPct)
                    {
                        StartMsg = string.Format(Catalog.GetString("This partner's Tax Deductible Percentage has been changed from {0}% to {1}%."),
                            OriginalPct.ToString("0.##"), NewPct.ToString("0.##"));
                    }
                    // if only date has changed
                    else
                    {
                        StartMsg =
                            string.Format(Catalog.GetString(
                                    "This partner's Tax Deductible Percentage has been changed to {0}% for all gifts from {1}."),
                                NewPct.ToString("0.##"), NewValidFrom.Date.ToString("dd-MMM-yyyy"));
                    }

                    /* Unposted gifts */

                    if (UnpostedGifts || UnpostedGiftInLedgerWithNoPermission)
                    {
                        if (UnpostedGifts)
                        {
                            PermissionGifts = "\n\n" +
                                              Catalog.GetString("Do you want to update all unposted gifts with this new percentage?") + "\n";
                        }

                        if (UnpostedGiftInLedgerWithNoPermission)
                        {
                            NoPermissionGifts = "\n\n" + Catalog.GetString(
                                "Some unposted gifts cannot be auto updated with this new percentage as you do not have permission to access the ledger which they belong to."
                                ) + "\n";
                        }

                        // add info on how many gifts belonging to which ledger
                        foreach (DataRow Row in GiftTotals.Rows)
                        {
                            if ((Convert.ToInt32(Row["Unposted"]) > 0) && UserInfo.GUserInfo.IsInLedger(Convert.ToInt32(Row["LedgerNumber"])))
                            {
                                if (Convert.ToInt32(Row["Unposted"]) == 1)
                                {
                                    PermissionGifts += "\n" + "- " + string.Format(Catalog.GetString("1 unposted gift from Ledger {0}"),
                                        Convert.ToInt32(Row["LedgerNumber"]));
                                }
                                else
                                {
                                    PermissionGifts += "\n" + "- " + string.Format(Catalog.GetString("{0} unposted gifts from Ledger {1}"),
                                        Convert.ToInt32(Row["Unposted"]), Convert.ToInt32(Row["LedgerNumber"]));
                                }
                            }
                            else if (Convert.ToInt32(Row["Unposted"]) > 0)
                            {
                                if (Convert.ToInt32(Row["Unposted"]) == 1)
                                {
                                    NoPermissionGifts += "\n" + "- " + string.Format(Catalog.GetString("1 unposted gift from Ledger {0}"),
                                        Convert.ToInt32(Row["LedgerNumber"]));
                                }
                                else
                                {
                                    NoPermissionGifts += "\n" + "- " + string.Format(Catalog.GetString("{0} unposted gifts from Ledger {1}"),
                                        Convert.ToInt32(Row["Unposted"]), Convert.ToInt32(Row["LedgerNumber"]));
                                }
                            }
                        }

                        // display a message box
                        if (UnpostedGifts)
                        {
                            if (!string.IsNullOrEmpty(NoPermissionGifts))
                            {
                                NoPermissionGifts = "(" + NoPermissionGifts + ")";
                            }

                            if (MessageBox.Show(StartMsg + PermissionGifts + NoPermissionGifts,
                                    Catalog.GetString("Tax Deductible Percentage Changed"),
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                // update unposted gifts
                                TRemote.MFinance.Gift.WebConnectors.UpdateUnpostedGiftsTaxDeductiblePct(FPartnerKey, NewPct, NewValidFrom);
                            }
                        }
                        else
                        {
                            MessageBox.Show(StartMsg + NoPermissionGifts,
                                Catalog.GetString("Tax Deductible Percentage Changed"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    /* Posted gifts */

                    if (PostedGifts || PostedGiftInLedgerWithNoPermission)
                    {
                        if (PostedGifts)
                        {
                            PermissionGifts = Environment.NewLine + Environment.NewLine +
                                              string.Format(Catalog.GetString(
                                    "Do you want to update all posted gifts from {0} with this new percentage?"),
                                NewValidFrom.Date.ToString("dd-MMM-yyyy")) + Environment.NewLine;
                        }

                        if (PostedGiftInLedgerWithNoPermission)
                        {
                            NoPermissionGifts = Environment.NewLine + Environment.NewLine + Catalog.GetString(
                                "Some posted gifts cannot be auto updated with this new percentage as you do not have permission to access the ledger which they belong to."
                                ) + Environment.NewLine;
                        }

                        // add info on how many gifts belonging to which ledger
                        foreach (DataRow Row in GiftTotals.Rows)
                        {
                            if ((Convert.ToInt32(Row["Posted"]) > 0) && UserInfo.GUserInfo.IsInLedger(Convert.ToInt32(Row["LedgerNumber"])))
                            {
                                if (Convert.ToInt32(Row["Posted"]) == 1)
                                {
                                    PermissionGifts += Environment.NewLine + "- " + string.Format(Catalog.GetString("1 posted gift from Ledger {0}"),
                                        Convert.ToInt32(Row["LedgerNumber"]));
                                }
                                else
                                {
                                    PermissionGifts += Environment.NewLine + "- " +
                                                       string.Format(Catalog.GetString("{0} posted gifts from Ledger {1}"),
                                        Convert.ToInt32(Row["Posted"]), Convert.ToInt32(Row["LedgerNumber"]));
                                }
                            }
                            else if (Convert.ToInt32(Row["Posted"]) > 0)
                            {
                                if (Convert.ToInt32(Row["Posted"]) == 1)
                                {
                                    NoPermissionGifts += Environment.NewLine + "- " + string.Format(Catalog.GetString("1 posted gift from Ledger {0}"),
                                        Convert.ToInt32(Row["LedgerNumber"]));
                                }
                                else
                                {
                                    NoPermissionGifts += Environment.NewLine + "- " +
                                                         string.Format(Catalog.GetString("{0} posted gifts from Ledger {1}"),
                                        Convert.ToInt32(Row["Posted"]), Convert.ToInt32(Row["LedgerNumber"]));
                                }
                            }
                        }

                        // display a message box
                        if (PostedGifts)
                        {
                            if (!string.IsNullOrEmpty(NoPermissionGifts))
                            {
                                NoPermissionGifts = "(" + NoPermissionGifts + ")";
                            }

                            TFrmExtendedMessageBox ExtendedMessageBox = new TFrmExtendedMessageBox(FPetraUtilsObject.GetForm());

                            if (ExtendedMessageBox.ShowDialog(StartMsg + PermissionGifts + NoPermissionGifts,
                                    Catalog.GetString("Tax Deductible Percentage Changed"),
                                    Catalog.GetString("Do not print the adjusting gift transactions on periodic receipts"),
                                    TFrmExtendedMessageBox.TButtons.embbYesNo, TFrmExtendedMessageBox.TIcon.embiQuestion)
                                == TFrmExtendedMessageBox.TResult.embrYes)
                            {
                                bool NoLabel;
                                ExtendedMessageBox.GetResult(out NoLabel);

                                // update posted gifts
                                TCommonScreensForwarding.TaxDeductiblePctAdjust.Invoke(FPartnerKey, NewPct, NewValidFrom, NoLabel,
                                    FPetraUtilsObject.GetForm());
                            }
                        }
                        else
                        {
                            MessageBox.Show(StartMsg + NoPermissionGifts,
                                Catalog.GetString("Tax Deductible Percentage Changed"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        #endregion


        #region Event Handlers

        private void TFrmPartnerEdit2_Load(System.Object sender, System.EventArgs e)
        {
            FPetraUtilsObject.TFrmPetra_Load(sender, e);

            this.Shown += new EventHandler(FPetraUtilsObject.OnFormShown);
            this.Shown += new EventHandler(TFrmPartnerEdit_Shown);

            // Reduce Form height to fit the PartnerEdit screen fully only on 800x600 resolution
            if (System.Windows.Forms.Screen.GetBounds(ucoUpperPart).Height == 600)
            {
                this.Height = 600;
            }

            mniEditFind.Enabled = true;
            mniEditFind.Click += new EventHandler(EditFindPartner);

            /*
             * Load data for new Partner or existing Partner
             */
            LoadData();

            if ((FMainDS == null) || (FMainDS.PPartner == null) || (FMainDS.PPartner.Count == 0))
            {
                // screen has been cancelled, most probably the new partner dialog was cancelled
                return;
            }

            /*
             * From here on we have access to the Server Object and the DataSet is filled
             * with data.
             */
            FPartnerClass = FMainDS.PPartner[0].PartnerClass;

            // Determine which tab page will be shown
            DetermineInitiallySelectedTabPage();


            // Determine whether Partner is of PartnerClass ORGANISATION and whether it is a Foundation
            DetermineOrganisationIsFoundation();

            ArrangeMenuItemsAndToolBarButtons();

            // Setup Modulerelated Toggle Buttons in ToolBar
            SetupAvailableModuleDataItems(true, TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgNone);

            /*
             * Setup the bottom part of the screen - that is the TabSet that corresponds
             * with the initially selected TabPage
             */
            ucoLowerPart.MainDS = FMainDS;
            ucoLowerPart.PetraUtilsObject = FPetraUtilsObject;
            ucoLowerPart.PartnerEditUIConnector = FPartnerEditUIConnector;
            ucoLowerPart.CurrentModuleTabGroup = FCurrentModuleTabGroup;
            ucoLowerPart.InitiallySelectedTabPage = FInitiallySelectedTabPage;
            ucoLowerPart.InitialiseDelegateIsNewPartner(@IsNewPartner);


            // Hook up EnableDisableOtherScreenParts Event that is fired by ucoPartnerTabSet
            ucoLowerPart.EnableDisableOtherScreenParts += new TEnableDisableScreenPartsEventHandler(
                this.UcoPartnerTabSet_EnableDisableOtherScreenParts);

            // Hook up ShowTab Event that is fired by FUcoPartnerDetailsOrganisation
            ucoLowerPart.ShowTab += new TShowTabEventHandler(this.UcoPartnerTabSet_ShowTab);

            // Hook up HookUpDataChange Event that is fired by ucoPartnerTabSet
            ucoLowerPart.HookupDataChange += new THookupDataChangeEventHandler(this.UcoPartnerTabSet_HookupDataChange);
            ucoLowerPart.HookupPartnerEditDataChange += new THookupPartnerEditDataChangeEventHandler(
                this.UcoPartnerTabSet_HookupPartnerEditDataChange);

            switch (FCurrentModuleTabGroup)
            {
                case TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgPartner:

                    ViewPartnerData(null, null);

                    break;

                case TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgPersonnel:

                    // Only switch to Personnel Tab Group if Partner is of Partner Class PERSON
                    if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                    {
                        // TODO: this call to ViewPartnerData should not really be needed here but
                        // if it is not done then currently the Save button gets enabled when the user wants
                        // to access the Partner Data section (there is probably some initialisation happening
                        // in there that is necessary for the whole screen)
                        ViewPartnerData(null, null);

                        ViewPersonnelData(null, null);
                    }
                    else
                    {
                        ViewPartnerData(null, null);
                    }

                    break;

                case TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgFinance:

                    ViewFinanceData(null, null);

                    break;
            }

            HookupPartnerEditDataChangeEvents(TPartnerEditTabPageEnum.petpDefault);

            // Hook up DataSaved Event to be able to run code after SaveChanges was run
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(this.FormDataSaved);

            /*
             * Set up top part of the Screen
             */
            ucoUpperPart.InitialiseDelegateMaintainGiftDestination(new TDelegateMaintainGiftDestination(MaintainGiftDestination));
            ucoUpperPart.MainDS = FMainDS;
            ucoUpperPart.VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            ucoUpperPart.PartnerEditUIConnector = FPartnerEditUIConnector;
            ucoUpperPart.ShowData();
            ucoUpperPart.InitialiseUserControl();


            // Set up screen caption
            SetScreenCaption();

            // Collapse upper Part, if user had it so last time
            FUppperPartInitiallyCollapsed = TUserDefaults.GetBooleanDefault(TUserDefaults.PARTNER_EDIT_UPPERPARTCOLLAPSED, false);

            if (FUppperPartInitiallyCollapsed)
            {
// TODO                ViewUpperScreenPartCollapsed(this, null);
            }
            else
            {
// TODO                ViewUpperScreenPartExpanded(this, null);
            }

            // Disable 'Local Partner Data' MenuItem if there are no Office Specific Data Labels available
            if (!FMainDS.MiscellaneousData[0].OfficeSpecificDataLabelsAvailable)
            {
                mniMaintainLocalPartnerData.Enabled = false;
            }

            FPetraUtilsObject.SubmitChangesContinue = false;
            ApplySecurity();

            // Need to do this manually  we disabled the automatic hookup in the Base Form because
            // we remove some TabPages, therefore the Controls on it, but the Events hooked
            // up to them would still be around and prevent a GC of the Form!
            FPetraUtilsObject.HookupAllControls();

            ucoUpperPart.Select();
            this.Cursor = Cursors.Default;


            // Checks whether there any Tips to show to the User; if there are, they will be
            // shown.
// TODO            ucoUpperPart.CheckForUserTips();
        }

        /// <summary>
        /// We must switch to the selected TabPage only once the the 'Shown' Event of the Form has been run
        /// to make sure that the TabControl does not show the selected TabPage leftmost, but at its' correct
        /// place in the order of the Tabs. (See Bug https://tracker.openpetra.org/view.php?id=2392)
        /// </summary>
        /// <param name="sender">Not evaluated.</param>
        /// <param name="e">Not evaluated.</param>
        void TFrmPartnerEdit_Shown(object sender, EventArgs e)
        {
            ucoLowerPart.SelectTabPage(FInitiallySelectedTabPage);
        }

        private void UcoUpperPart_PartnerClassMainDataChanged(System.Object Sender, TPartnerClassMainDataChangedEventArgs e)
        {
            FPetraUtilsObject.HasChanges = true;

            SetScreenCaption();
        }

        private void UcoUpperPart_CollapsingEvent(System.Object sender, CollapsibleEventArgs args)
        {
#if TODO
            if (args.WillCollapse)
            {
                // is getting collapsed
                ucoUpperPart.Caption = "  " + ucoUpperPart.PartnerQuickInfo(false);
// TODO               ucoUpperPart.SubCaption = '[' + FPartnerClass + "] ";

                if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                {
// TODO                   ucoUpperPart.SubCaptionHighlighted = true;
                }
            }
            else
            {
                // is getting expanded
                ucoUpperPart.Caption = "";
                ucoUpperPart.SubCaption = "";
            }
#endif
        }

        private void UcoPartnerTabSet_EnableDisableOtherScreenParts(System.Object sender, TEnableDisableEventArgs e)
        {
            // MessageBox.Show('TFrmPartnerEdit.ucoPartnerTabEnableDisableOtherScreenParts = ' + e.Enable.ToString + ')';
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
                SetupAvailableModuleDataItems(e.Enable, TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgNone);
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
        }

        private void Form_Closed(object sender, EventArgs e)
        {
            // release the object so that the object can get GC'd on the PetraServer
            FPartnerEditUIConnector = null;
        }

        #endregion


        #region Action Handlers

        #region File Menu

        private void FileNewPartner(System.Object sender, System.EventArgs e)
        {
            Ict.Petra.Client.MPartner.Gui.TFrmPartnerEdit PartnerEditForm = new Ict.Petra.Client.MPartner.Gui.TFrmPartnerEdit(this);

            if (FPartnerClass != SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
            {
                PartnerEditForm.SetParameters(TScreenMode.smNew,
                    FPartnerClass, -1, -1, ucoLowerPart.PartnerLocationDataRowOfCurrentlySelectedAddress.LocationCountryCode);
            }
            else
            {
                PartnerEditForm.SetParameters(TScreenMode.smNew, FPartnerClass,
                    -1, -1, String.Empty, String.Empty, false, FPartnerKey,
                    ucoLowerPart.PartnerLocationDataRowOfCurrentlySelectedAddress.LocationKey,
                    ucoLowerPart.PartnerLocationDataRowOfCurrentlySelectedAddress.SiteKey
                    );
            }

            PartnerEditForm.Show();
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
            TFormsMessage BroadcastMessage;

            /* Check for new Partner that wasn't saved yet */
            if (IsNewPartner(FMainDS))
            {
                /* Tell user that he can't delete a new Partner that wasn't saved yet */
                MessageBox.Show(MPartnerResourcestrings.StrErrorNeedToSavePartner1 + MPartnerResourcestrings.StrErrorDeletePartner2,
                    MPartnerResourcestrings.StrErrorNeedToSavePartnerTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                /* Check for unsaved changes */
                if (CanClose())
                {
                    /* Delete Partner; if OK, broadcast a message to any listening forms to inform them and then close the screen */
                    if (TPartnerMain.DeletePartner(FPartnerKey, this))
                    {
                        BroadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcPartnerDeleted,
                            FCallerContext);

                        BroadcastMessage.SetMessageDataPartner(
                            FPartnerKey,
                            SharedTypes.PartnerClassStringToEnum(FPartnerClass),
                            "",
                            FMainDS.PPartner[0].StatusCode);

                        TFormsList.GFormsList.BroadcastFormMessage(BroadcastMessage);

                        this.Close();
                    }
                }
                else
                {
                    /* Tell user that he can't delete a Partner that has changes that weren't saved yet */
                    MessageBox.Show(StrCannotDeletePartner, StrCannotDeletePartnerTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }

        private void FileSendEmail(System.Object sender, System.EventArgs e)
        {
            string PrimaryEmailAddress;

            if (Calculations.GetPrimaryEmailAddress(
                    FMainDS.PPartnerAttribute, out PrimaryEmailAddress))
            {
                TRtbHyperlinks.DisplayHelper Launcher = new TRtbHyperlinks.DisplayHelper(new TRtbHyperlinks());

                Launcher.LaunchHyperLink(PrimaryEmailAddress, THyperLinkHandling.HYPERLINK_PREFIX_EMAILLINK);
            }
            else
            {
                MessageBox.Show(MPartnerResourcestrings.StrNoPrimaryEmailAvailableToSendEmailTo,
                    MPartnerResourcestrings.StrNoPrimaryEmailAvailableToSendEmailToTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FilePrintPartner(System.Object sender, System.EventArgs e)
        {
            /* Check for new Partner that wasn't saved yet */
            if (IsNewPartner(FMainDS))
            {
                /* Tell user that he can't delete a new Partner that wasn't saved yet */
                MessageBox.Show(MPartnerResourcestrings.StrErrorNeedToSavePartner1 + MPartnerResourcestrings.StrErrorPrintPartner2,
                    MPartnerResourcestrings.StrErrorNeedToSavePartnerTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                /* Check for unsaved changes */
                if (CanClose())
                {
                    Form MainWindow = FPetraUtilsObject.GetCallerForm();
                    TCommonScreensForwarding.OpenPrintPartnerDialog.Invoke(FPartnerKey, MainWindow);
                }
                else
                {
                    /* Tell user that he can't print a Partner that has changes that weren't saved yet */
                    MessageBox.Show(StrCannotPrintPartner, StrCannotPrintPartnerTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }

        private void FilePrintSection(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();

#if TODO
            Int32 SelectedTab = -1;

            switch (this.ucoLowerPart.CurrentlySelectedTabPage)
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

                    // TPartnerEditTabPageEnum.petpDocuments:     SelectedTab := 4;
                    // TPartnerEditTabPageEnum.petpOfficeSpecific:     SelectedTab := 5;
                    // TPartnerEditTabPageEnum.petpFoundationDetails:     SelectedTab := 6;
                    // TPartnerEditTabPageEnum.petpPartnerRelationshipsDetails:     SelectedTab := 7;
            }

            TPartnerPrintSectionDialog PrintSectionDialog = new TPartnerPrintSectionDialog();

            PrintSectionDialog.SetParameters(SelectedTab, @GetDataRowOfCurrentlySelectedRecord, FPetraUtilsObject.HasChanges, FPartnerKey);
            PrintSectionDialog.ShowDialog();
#endif
        }

        private void FileExportPartner(System.Object sender, System.EventArgs e)
        {
            TPartnerExportLogic.ExportSinglePartner(this.PartnerKey, 0, 0);
        }

        /// <summary>
        /// Opens the Partner Find screen (or activates it in case a non-modal instance was already open).
        /// </summary>
        private void EditFindPartner(System.Object sender, System.EventArgs e)
        {
            TPartnerMain.FindPartner(this);
        }

        #endregion

        #region Maintain Menu

        private void MaintainAddresses(System.Object sender, System.EventArgs e)
        {
            ucoLowerPart.SelectTabPage(TPartnerEditTabPageEnum.petpAddresses);
        }

        private void MaintainContactDetails(System.Object sender, System.EventArgs e)
        {
            ucoLowerPart.SelectTabPage(TPartnerEditTabPageEnum.petpContactDetails);
        }

        private void MaintainPartnerDetails(System.Object sender, System.EventArgs e)
        {
            ucoLowerPart.SelectTabPage(TPartnerEditTabPageEnum.petpDetails);
        }

        private void MaintainFoundationDetails(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainSubscriptions(System.Object sender, System.EventArgs e)
        {
            ucoLowerPart.SelectTabPage(TPartnerEditTabPageEnum.petpSubscriptions);
        }

        private void MaintainSpecialTypes(System.Object sender, System.EventArgs e)
        {
            ucoLowerPart.SelectTabPage(TPartnerEditTabPageEnum.petpPartnerTypes);
        }

        private void MaintainContacts(System.Object sender, System.EventArgs e)
        {
            ucoLowerPart.SelectTabPage(TPartnerEditTabPageEnum.petpContacts);
        }

        private void MaintainFamilyMembers(System.Object sender, System.EventArgs e)
        {
            if ((FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
                || (FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON)))
            {
                ucoLowerPart.SelectTabPage(TPartnerEditTabPageEnum.petpFamilyMembers);
            }
            else
            {
                MessageBox.Show(MCommonResourcestrings.StrErrorOnlyForFamilyOrPerson,
                    MCommonResourcestrings.StrGenericFunctionalityNotAvailable);
            }
        }

        private void MaintainRelationships(System.Object sender, System.EventArgs e)
        {
            ucoLowerPart.SelectTabPage(TPartnerEditTabPageEnum.petpPartnerRelationships);
        }

        private void MaintainInterests(System.Object sender, System.EventArgs e)
        {
            ucoLowerPart.SelectTabPage(TPartnerEditTabPageEnum.petpInterests);
        }

        private void MaintainReminders(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaintainNotes(System.Object sender, System.EventArgs e)
        {
            ucoLowerPart.SelectTabPage(TPartnerEditTabPageEnum.petpNotes);
        }

        private void MaintainLocalPartnerData(System.Object sender, System.EventArgs e)
        {
            ucoLowerPart.SelectTabPage(TPartnerEditTabPageEnum.petpOfficeSpecific);
        }

        /// <summary>
        /// This Method is called through a Delegate from UC_PartnerEdit_TopPart.ManualCode.cs!
        /// </summary>
        private void MaintainGiftDestination()
        {
            MaintainGiftDestination(this, null);
        }

        private void MaintainGiftDestination(System.Object sender, System.EventArgs e)
        {
            if (FPartnerClass == TPartnerClass.FAMILY.ToString())
            {
                // this will be 0 for a new and unsaved family partner
                // (new person partners can have gift destination added straight away as they are added to the person's family partner)
                if (FPartnerKey == 0)
                {
                    MessageBox.Show(Catalog.GetString("This new partner must be saved before a Gift Destination can be added."),
                        Catalog.GetString("Gift Destination"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                TFrmGiftDestination GiftDestinationForm = new TFrmGiftDestination(FPetraUtilsObject.GetForm(), FPartnerKey);

                GiftDestinationForm.Show();
            }
            else if (FPartnerClass == TPartnerClass.PERSON.ToString())
            {
                // open the Gift Destination screen for the person's family
                TFrmGiftDestination GiftDestinationForm = new TFrmGiftDestination(
                    FPetraUtilsObject.GetForm(), ((PPersonRow)FMainDS.PPerson.Rows[0]).FamilyKey);

                MessageBox.Show(string.Format(Catalog.GetString("Gift Destination records are held for Family partners only.{0}" +
                            "This screen will display all Gift Destination records for the Family of {1}."),
                        "\n\n", ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerShortName),
                    Catalog.GetString("Gift Destination"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                GiftDestinationForm.Show();
            }
        }

        private void MaintainIndividualData(System.Object sender, System.EventArgs e)
        {
            if (FPartnerClass != SharedTypes.PartnerClassEnumToString(TPartnerClass.UNIT))
            {
                ViewPersonnelData(null, null);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void MaintainDonorHistory(System.Object sender, System.EventArgs e)
        {
            TCommonScreensForwarding.OpenDonorRecipientHistoryScreen(true,
                PartnerKey,
                FPetraUtilsObject.GetForm());
        }

        private void MaintainRecipientHistory(System.Object sender, System.EventArgs e)
        {
            TCommonScreensForwarding.OpenDonorRecipientHistoryScreen(false,
                PartnerKey,
                FPetraUtilsObject.GetForm());
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
            ucoLowerPart.SelectTabPage(TPartnerEditTabPageEnum.petpFinanceDetails);
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
            // if tab group is about to be changed make sure that validation is ok
            if (ucoLowerPart.CurrentModuleTabGroup != TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgPartner)
            {
                if (!ucoLowerPart.ValidateCurrentModuleTabGroupData())
                {
                    return;
                }
            }

            tbbViewPartnerData.Checked = true;
            tbbViewPersonnelData.Checked = false;
            tbbViewFinanceData.Checked = false;
            mniViewPartnerData.Checked = true;
            mniViewPersonnelData.Checked = false;
            mniViewFinanceData.Checked = false;

            ucoLowerPart.ShowChildUserControl(TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgPartner);
        }

        private void ViewPersonnelData(System.Object sender, System.EventArgs e)
        {
            // if tab group is about to be changed make sure that validation is ok
            if (ucoLowerPart.CurrentModuleTabGroup != TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgPersonnel)
            {
                if (!ucoLowerPart.ValidateCurrentModuleTabGroupData())
                {
                    return;
                }
            }

            if (UserHasPersonnelAccess())
            {
                if (FPartnerClass != SharedTypes.PartnerClassEnumToString(TPartnerClass.UNIT))
                {
                    tbbViewPersonnelData.Checked = true;
                    tbbViewPartnerData.Checked = false;
                    tbbViewFinanceData.Checked = false;
                    mniViewPersonnelData.Checked = true;
                    mniViewPartnerData.Checked = false;
                    mniViewFinanceData.Checked = false;

                    ucoLowerPart.ShowChildUserControl(TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgPersonnel);
                }
                else
                {
                    // If editing a Partner of Partner Class UNIT, don't enable the Personnel Data TabGroup!
                }
            }
            else
            {
                MessageBox.Show(Catalog.GetString("You do not have access rights to the Personnel System of OpenPetra.\r\n\r\n" +
                        "For that reason the Pesonnel Data Tabs cannot be switched to on the\r\n" +
                        "Partner Edit screen. Showing the Partner Data Tabs instead."),
                    Catalog.GetString("Access Denied"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                tbbViewPersonnelData.Enabled = false;
                mniViewPersonnelData.Enabled = false;

                ViewPartnerData(null, null);
            }
        }

        private void ViewFinanceData(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private static bool UserHasPersonnelAccess()
        {
            return UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_PERSONNEL);
        }

        private static bool UserHasFinanceAccess()
        {
            return UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE1);
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

            FPetraUtilsObject.FormTitle = StrScreenCaption;

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
                    FPetraUtilsObject.HasNewData = true;

                    if (!GetPartnerEditUIConnector(TUIConnectorType.uictNewPartner))
                    {
                        MessageBox.Show(
                            String.Format(AppCoreResourcestrings.StrOpeningCancelledByUser,
                                StrScreenCaption),
                            AppCoreResourcestrings.StrOpeningCancelledByUserTitle,
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
                     * (initial address automatically created for PERSON if FNewPartnerFamilyLocationKey is populated)
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
                        if (FNewPartnerFamilyLocationKey == -1)
                        {
                            // Backstop: If FNewPartnerFamilyLocationKey was -1, the server will have returned the
                            // 'Best Address' of the FAMILY in the DataSet. Now use this to create the first address of the
                            // PERSON.
                            FNewPartnerFamilyLocationKey = FMainDS.PPartnerLocation[0].LocationKey;
                            FNewPartnerFamilySiteKey = FMainDS.PPartnerLocation[0].SiteKey;
                        }

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
                        //if (SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass) != TPartnerClass.PERSON)
                        // Create Address with default values
                        TAddressHandling.CreateNewAddress(FMainDS.PLocation,
                            FMainDS.PPartnerLocation,
                            FMainDS.PPartner[0].PartnerKey,
                            SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass),
                            FNewPartnerCountryCode,
                            -1);
                    }

                    // make sure that location specific fields in PartnerLocationDT get initialized
                    PartnerCodeHelper.SyncPartnerEditTDSPartnerLocation(FMainDS.PLocation, FMainDS.PPartnerLocation);

                    // Make this address a Current Address and also the 'Best' Address
                    NewPartnerLocationRow = (PartnerEditTDSPPartnerLocationRow)FMainDS.PPartnerLocation.Rows[0];
                    NewPartnerLocationRow.Icon = 1;
                    NewPartnerLocationRow.BestAddress = true;

                    /*
                     * Load Gift Destination
                     */
                    FMainDS.PPartnerGiftDestination.Merge(FPartnerEditUIConnector.GetCurrentAndFutureGiftDestinationData(FNewPartnerFamilyPartnerKey));
                    bool Changes = FPetraUtilsObject.HasChanges;

                    // update the gift destination
                    SetGiftDestination();

                    // revert to previous save status (SetGiftDestination unnecessarily enables save)
                    EnableSave(Changes);
                    FPetraUtilsObject.HasChanges = Changes;
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
                                String.Format(AppCoreResourcestrings.StrOpeningCancelledByUser,
                                    StrScreenCaption),
                                AppCoreResourcestrings.StrOpeningCancelledByUserTitle,
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
                                String.Format(AppCoreResourcestrings.StrOpeningCancelledByUser,
                                    StrScreenCaption),
                                AppCoreResourcestrings.StrOpeningCancelledByUserTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // to prevent strange error message, that would stop the form from closing
                            FPetraUtilsObject.FormActivatedForFirstTime = false;
                            Close();
                            return;
                        }

                        FPartnerKey = FMainDS.PPartner[0].PartnerKey;
                    }

                    SetGiftDestination();
                }
                catch (EPartnerNotExistantException)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(
                        "Partner with Partner Key " + FPartnerKey.ToString() + " does not exist.", "Non-existent Partner!", MessageBoxButtons.OK,
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
                        "Non-existent Location - Cannot Open Partner!",
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
                    TMessages.MsgSecurityException(Exp);

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

        /// <summary>
        /// set the gift destination on this screen and all other open partner edit screens for the same family
        /// </summary>
        private void SetGiftDestinationAllScreens()
        {
            if ((FMainDS.PPartnerGiftDestination != null) && (FMainDS.PPartnerGiftDestination.Rows.Count > 0))
            {
                // Broadcast message to update partners' Partner Edit screen if open
                TFormsMessage BroadcastMessage;

                BroadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcGiftDestinationChanged);

                if (FMainDS.PPartner[0].PartnerClass == TPartnerClass.FAMILY.ToString())
                {
                    BroadcastMessage.SetMessageDataGiftDestination(
                        FPartnerKey,
                        FMainDS.PPartnerGiftDestination);
                }
                else
                {
                    BroadcastMessage.SetMessageDataGiftDestination(
                        FMainDS.PPerson[0].FamilyKey,
                        FMainDS.PPartnerGiftDestination);
                }

                TFormsList.GFormsList.BroadcastFormMessage(BroadcastMessage);
            }
        }

        private void SetGiftDestination()
        {
            // get Field of Partner (if PERSON or FAMILY)
            System.Int64 FieldKey = 0;
            String FieldName = "";
            TPartnerClass FieldClass;
            bool HasCurrentCommitment = false;

            if ((FMainDS.PPartnerGiftDestination != null) && (FMainDS.PPartnerGiftDestination.Rows.Count > 0))
            {
                foreach (PPartnerGiftDestinationRow Row in FMainDS.PPartnerGiftDestination.Rows)
                {
                    // check if record is active for today
                    if ((Row.DateEffective <= DateTime.Today)
                        && ((Row.DateExpires >= DateTime.Today) || Row.IsDateExpiresNull())
                        && (Row.DateEffective != Row.DateExpires))
                    {
                        HasCurrentCommitment = true;
                        FieldKey = Row.FieldKey;
                        TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(FieldKey, out FieldName, out FieldClass);
                    }
                }

                // Check whether the Partner has "EX-WORKER*" Partner Type by looking this up in the MiscellaneusData Table
                if ((FMainDS.MiscellaneousData[0].HasEXWORKERPartnerType == true)
                    && !HasCurrentCommitment)
                {
                    ucoUpperPart.SetGiftDestinationText(MPartnerConstants.PARTNERTYPE_EX_WORKER);
                }
                else if (HasCurrentCommitment && (FieldName != ""))
                {
                    ucoUpperPart.SetGiftDestinationText(FieldName);
                }
                else if (HasCurrentCommitment && (FieldKey > 0))
                {
                    ucoUpperPart.SetGiftDestinationText(StringHelper.FormatStrToPartnerKeyString(FieldKey.ToString()));
                }
                else if (!HasCurrentCommitment)
                {
                    ucoUpperPart.SetGiftDestinationText(Catalog.GetString("Not set"));
                }
            }
            else
            {
                ucoUpperPart.SetGiftDestinationText(Catalog.GetString("Not set"));
            }
        }

        private void DataLoadOperationFinishing(System.Object sender, System.EventArgs e)
        {
//MessageBox.Show("DataLoadOperationFinishing");
            FPetraUtilsObject.SuppressChangeDetection = false;
        }

        private void DataLoadOperationStarting(System.Object sender, System.EventArgs e)
        {
//MessageBox.Show("DataLoadOperationStarting");
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
                case TPartnerEditTabPageEnum.petpContactDetails:
                case TPartnerEditTabPageEnum.petpSubscriptions:
                case TPartnerEditTabPageEnum.petpPartnerTypes:
                case TPartnerEditTabPageEnum.petpPartnerRelationships:
                case TPartnerEditTabPageEnum.petpNotes:
                case TPartnerEditTabPageEnum.petpFinanceDetails:
                case TPartnerEditTabPageEnum.petpInterests:
                    FInitiallySelectedTabPage = FShowTabPage;

                    break;

#if  SHOWUNFINISHEDTABS
                case TPartnerEditTabPageEnum.petpReminders:
                    FInitiallySelectedTabPage = FShowTabPage;

                    break;

#else
                case TPartnerEditTabPageEnum.petpContacts:
                    FInitiallySelectedTabPage = FShowTabPage;

                    break;

                case TPartnerEditTabPageEnum.petpReminders:
                    FShowTabPage = TPartnerEditTabPageEnum.petpAddresses;
                    FInitiallySelectedTabPage = FShowTabPage;

                    break;
#endif

                case TPartnerEditTabPageEnum.petpPersonnelIndividualData:
                case TPartnerEditTabPageEnum.petpPersonnelApplications:
                    FInitiallySelectedTabPage = FShowTabPage;

                    break;

                default:
                    FInitiallySelectedTabPage = TPartnerEditTabPageEnum.petpAddresses;

                    break;
            }

            FCurrentModuleTabGroup = TPartnerEditScreenLogic.DetermineTabGroup(FInitiallySelectedTabPage);
        }

        /// <summary>
        /// Determines whether Partner is of PartnerClass ORGANISATION and whether it is
        /// a Foundation.
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

        private void ArrangeMenuItemsAndToolBarButtons()
        {
            // Because of YAML inheritance there is one separator bar too many displayed: hide it for that reason
            mniSeparator0.Visible = false;
        }

        /// <summary>
        /// Sets Module-related Toggle Buttons in ToolBar up
        /// </summary>
        /// <returns>void</returns>
        private void SetupAvailableModuleDataItems(Boolean AEnable, TPartnerEditScreenLogic.TModuleTabGroupEnum ALockOnModule)
        {
            Boolean IsEnabled = false;

            // TODO 2 oChristianK cSecurity : Take security settings into consideration.
            // Partner Module Data
            if ((ALockOnModule == TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgNone)
                || (ALockOnModule == TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgPartner))
            {
                if (ALockOnModule == TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgPartner)
                {
                    IsEnabled = false;
                }
                else
                {
                    IsEnabled = AEnable;
                }

                if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                {
                    tbbViewPartnerData.Enabled = IsEnabled;
                    mniViewPartnerData.Enabled = IsEnabled;

                    mniMaintainFamilyMembers.Enabled = IsEnabled;
                    mniMaintainGiftDestination.Enabled = IsEnabled;
                    mniMaintainFamilyMembers.Text = MPartnerResourcestrings.StrFamilyMenuItemText.Substring(0,
                        MPartnerResourcestrings.StrFamilyMenuItemText.Length - 3);   // remove trailing "..."
                    // TODO
//                    // Exchange the 'Family Members' icon with the 'Family' icon!
//                    this.XPMenuItemExtender.SetMenuGlyph(this.mniMaintainFamilyMembers, imlMenuHelper.Images[0]);
                }
                else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
                {
                    tbbViewPartnerData.Enabled = false;
                    mniViewPartnerData.Enabled = false;

                    mniMaintainFamilyMembers.Enabled = IsEnabled;
                    mniMaintainGiftDestination.Enabled = IsEnabled;
                    mniMaintainFamilyMembers.Text = MPartnerResourcestrings.StrFamilyMembersMenuItemText.Substring(0,
                        MPartnerResourcestrings.StrFamilyMembersMenuItemText.Length - 3);   // remove trailing "...";
                }
                else
                {
                    tbbViewPartnerData.Enabled = false;
                    mniViewPartnerData.Enabled = false;

                    // Following functionality is available only for PERSON and FAMILY
                    mniMaintainFamilyMembers.Enabled = false;
                    mniMaintainGiftDestination.Enabled = false;
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
                mniMaintainLocalPartnerData.Enabled = IsEnabled;
                mniMaintainInterests.Enabled = IsEnabled;
                mniMaintainReminders.Enabled = IsEnabled;
                mniMaintainRelationships.Enabled = IsEnabled;
                mniMaintainContacts.Enabled = IsEnabled;
                mniMaintainNotes.Enabled = IsEnabled;
                mniMaintainFinanceDetails.Enabled = IsEnabled;
            }
            else
            {
                tbbViewPartnerData.Enabled = (!IsEnabled);
                tbbViewPartnerData.Enabled = (!IsEnabled);
                mniMaintainAddresses.Enabled = (!IsEnabled);
                mniEditFindNewAddress.Enabled = (!IsEnabled);
                mniMaintainPartnerDetails.Enabled = (!IsEnabled);

                if (!FFoundationDetailsEnabled)
                {
                    mniMaintainFoundationDetails.Enabled = (!IsEnabled);
                }

                mniMaintainSubscriptions.Enabled = (!IsEnabled);
                mniMaintainSpecialTypes.Enabled = (!IsEnabled);
                mniMaintainLocalPartnerData.Enabled = (!IsEnabled);
                mniMaintainInterests.Enabled = (!IsEnabled);
                mniMaintainReminders.Enabled = (!IsEnabled);
                mniMaintainFamilyMembers.Enabled = (!IsEnabled);
                mniMaintainRelationships.Enabled = (!IsEnabled);
                mniMaintainContacts.Enabled = (!IsEnabled);
                mniMaintainNotes.Enabled = (!IsEnabled);
                mniMaintainGiftDestination.Enabled = (!IsEnabled);
                mniMaintainFinanceDetails.Enabled = (!IsEnabled);
            }

            // Personnel Module Data
            if ((ALockOnModule == TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgNone)
                || (ALockOnModule == TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgPersonnel))
            {
                if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                {
                    if (UserHasPersonnelAccess())
                    {
                        tbbViewPersonnelData.Enabled = AEnable;
                        mniViewPersonnelData.Enabled = AEnable;
                        mniMaintainPersonnelData.Enabled = AEnable;
                    }
                    else
                    {
                        tbbViewPersonnelData.Enabled = false;
                        mniViewPersonnelData.Enabled = false;
                        mniMaintainPersonnelData.Enabled = false;
                    }

                    mniMaintainPersonnelData.Text = MPartnerResourcestrings.StrPersonnelPersonMenuItemText;
                }
                else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.UNIT))
                {
                    mniMaintainPersonnelData.Enabled = AEnable;
                    tbbViewPersonnelData.Enabled = false;
                    mniViewPersonnelData.Enabled = false;

                    mniMaintainPersonnelData.Text = MPartnerResourcestrings.StrPersonnelUnitMenuItemText;
                }
                else
                {
                    tbbViewPersonnelData.Enabled = (!AEnable);
                    mniViewPersonnelData.Enabled = (!AEnable);
                    mniMaintainPersonnelData.Enabled = (!AEnable);
                }
            }
            else
            {
                tbbViewPersonnelData.Enabled = (!AEnable);
                mniViewPersonnelData.Enabled = (!AEnable);
                mniMaintainPersonnelData.Enabled = (!AEnable);
            }

            // Finance Module Data
            if (((ALockOnModule == TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgNone)
                 || (ALockOnModule == TPartnerEditScreenLogic.TModuleTabGroupEnum.mtgFinance))
                && UserHasFinanceAccess())
            {
                //tbbViewFinanceData.Enabled = AEnable;   // This Tab Group is not functional yet
                //mniViewFinanceData.Enabled = AEnable;   // This Tab Group is not functional yet
                mniMaintainDonorHistory.Enabled = AEnable;
                mniMaintainRecipientHistory.Enabled = AEnable;

                // For the moment, we want all to stay disabled since they are not functional yet...
                // mniMaintainFinanceReports.Enabled := AEnable;
                // mniMaintainBankAccounts.Enabled := AEnable;
                // mniMaintainGiftReceipting.Enabled := AEnable;
            }
            else
            {
                //tbbViewFinanceData.Enabled = (!AEnable);    // This Tab Group is not functional yet
                //mniViewFinanceData.Enabled = (!AEnable);    // This Tab Group is not functional yet
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
            bool ServerCallSuccessful = false;

            TServerBusyHelper.CoordinatedAutoRetryCall("Partner Edit", ref ServerCallSuccessful,
                delegate
                {
                    switch (AUIConnectorType)
                    {
                        case TUIConnectorType.uictPartnerKey:
                            FPartnerEditUIConnector = TRemote.MPartner.Partner.UIConnectors.PartnerEdit(FPartnerKey);
                            FMainDS = FPartnerEditUIConnector.GetData(TClientSettings.DelayedDataLoading, FInitiallySelectedTabPage);
                            break;

                        case TUIConnectorType.uictLocationKey:

                            // MessageBox.Show('Passed in FLocationKeyForSelectingPartnerLocation: ' + FLocationKeyForSelectingPartnerLocation.toString);
                            FPartnerEditUIConnector = TRemote.MPartner.Partner.UIConnectors.PartnerEdit(FPartnerKey,
                            FSiteKeyForSelectingPartnerLocation,
                            FLocationKeyForSelectingPartnerLocation);
                            FMainDS = FPartnerEditUIConnector.GetData(TClientSettings.DelayedDataLoading, FInitiallySelectedTabPage);
                            break;

                        case TUIConnectorType.uictNewPartner:
                            FPartnerEditUIConnector = TRemote.MPartner.Partner.UIConnectors.PartnerEdit();
                            break;
                    }

                    ServerCallSuccessful = true;
                });

            if (ServerCallSuccessful)
            {
                // Ensure Object is Un-Registered at the time the Form got closed
                this.Closed += new System.EventHandler(this.Form_Closed);
            }
            else
            {
                // ServerCallRetries must be equal to MAX_RETRIES when we get here!
                if (TServerBusyHelperGui.ShowServerBusyDialogWhenOpeningForm(StrScreenCaption) == DialogResult.Retry)
                {
                    return GetPartnerEditUIConnector(AUIConnectorType);
                }
            }

            return ServerCallSuccessful;
        }

        private Boolean OpenNewPartnerDialog()
        {
            Boolean ReturnValue = false;

            TPartnerNewDialogWinForm NewPartnerDialog;

            NewPartnerDialog = new TPartnerNewDialogWinForm(this);
            NewPartnerDialog.SetParameters(FPartnerEditUIConnector,
                FNewPartnerPartnerClass,
                FNewPartnerSiteKey,
                FNewPartnerPartnerKey,
                FNewPartnerAcquisitionCode,
                FNewPartnerPrivatePartner,
                FNewPartnerFamilyPartnerKey,
                FNewPartnerFamilyLocationKey,
                FNewPartnerFamilySiteKey,
                FNewPartnerDefaultPartnerClass);

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
                FPetraUtilsObject.HasNewData = true;

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
                    FMainDS.PLocation.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging); //TODOWB
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
                    FMainDS.PPartnerInterest.RowDeleting += new DataRowChangeEventHandler(FPetraUtilsObject.OnAnyDataRowChanging);
                    break;

                case TPartnerEditTabPageEnum.petpOfficeSpecific:
                    FMainDS.PDataLabelValuePartner.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    break;

                case TPartnerEditTabPageEnum.petpPartnerRelationships:
                    FMainDS.PPartnerRelationship.ColumnChanging += new DataColumnChangeEventHandler(FPetraUtilsObject.OnAnyDataColumnChanging);
                    FMainDS.PPartnerRelationship.RowDeleting += new DataRowChangeEventHandler(FPetraUtilsObject.OnAnyDataRowChanging);
                    break;

                case TPartnerEditTabPageEnum.petpFinanceDetails:
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
            FPetraUtilsObject.SetScreenCaption(" - " + ucoUpperPart.PartnerQuickInfo(true));
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

        private void EnableSave(bool Enable)
        {
            if ((Enable) && (ucoUpperPart.Enabled))
            {
                FPetraUtilsObject.SetChangedFlag();
            }
            else
            {
                FPetraUtilsObject.DisableSaveButton();
            }
        }

        private void EnableDisableUpperPart(bool AEnable)
        {
            ucoUpperPart.Enabled = AEnable;
        }

        private void HookupDataChangeEvents()
        {
            HookupPartnerEditDataChangeEvents(TPartnerEditTabPageEnum.petpAddresses);
        }

        #endregion

        #region Menu and command key handlers for our user controls

        /// <summary>
        /// Handler for command key processing
        /// </summary>
        private bool ProcessCmdKeyManual(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.E | Keys.Control | Keys.Shift))
            {
                // special case where we focus the Top part
                if (this.ucoUpperPart.ProcessParentCmdKey(ref msg, keyData))
                {
                    return true;
                }
            }
            else if (this.ucoLowerPart.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Forms Messaging Interface Implementation

        /// <summary>
        /// Will be called by TFormsList to inform any Form that is registered in TFormsList
        /// about any 'Forms Messages' that are broadcasted.
        /// </summary>
        /// <remarks>The Partner Edit 'listens' to such 'Forms Message' broadcasts by
        /// implementing this virtual Method. This Method will be called each time a
        /// 'Forms Message' broadcast occurs.
        /// </remarks>
        /// <param name="AFormsMessage">An instance of a 'Forms Message'. This can be
        /// inspected for parameters in the Method Body and the Form can use those to choose
        /// to react on the Message, or not.</param>
        /// <returns>Returns True if the Form reacted on the specific Forms Message,
        /// otherwise false.</returns>
        public bool ProcessFormsMessage(TFormsMessage AFormsMessage)
        {
            bool MessageProcessed = false;

            if ((AFormsMessage.MessageClass == TFormsMessageClassEnum.mcNewPartnerSaved)
                || (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcExistingPartnerSaved)
                || (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcPartnerDeleted))
            {
                // Refreshes the Family Members list on the Family tab
                ucoLowerPart.RefreshFamilyMembersList(AFormsMessage);

                MessageProcessed = true;
            }
            else if (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcGiftDestinationChanged)
            {
                // check if partner keys are equal or that, if the partner is a Person, that they belong to a family with the same partner key
                if ((FPartnerKey == ((TFormsMessage.FormsMessageGiftDestination)AFormsMessage.MessageObject).PartnerKey)
                    || ((FPartnerClass == TPartnerClass.PERSON.ToString())
                        && (FMainDS.PPerson[0].FamilyKey == ((TFormsMessage.FormsMessageGiftDestination)AFormsMessage.MessageObject).PartnerKey)))
                {
                    if (FMainDS.PPartnerGiftDestination == null)
                    {
                        FMainDS.Tables.Add(new PPartnerGiftDestinationTable());
                        FMainDS.InitVars();
                    }

                    FMainDS.PPartnerGiftDestination.Merge(
                        ((TFormsMessage.FormsMessageGiftDestination)AFormsMessage.MessageObject).GiftDestinationTable);
                    FMainDS.MiscellaneousData[0].HasEXWORKERPartnerType = Checks.PartnerIsExWorker(FMainDS.PPartnerGiftDestination);
                    bool Changes = FPetraUtilsObject.HasChanges;

                    // update the gift destination
                    SetGiftDestination();

                    // revert to previous save status (SetGiftDestination unnecessarily enables save)
                    EnableSave(Changes);
                    FPetraUtilsObject.HasChanges = Changes;

                    MessageProcessed = true;
                }
            }
            else if ((AFormsMessage.MessageClass == TFormsMessageClassEnum.mcUnitHierarchyChanged)
                     && (FPartnerClass == TPartnerClass.UNIT.ToString()))
            {
                List <Tuple <string, Int64,
                             Int64>>UnitHierarchyChanges =
                    ((TFormsMessage.FormsMessageUnitHierarchy)AFormsMessage.MessageObject).UnitHierarchyChanges;

                // loop backwards as the most recent (and accurate) change will be at the end
                for (int i = UnitHierarchyChanges.Count - 1; i >= 0; i--)
                {
                    if (UnitHierarchyChanges[i].Item2 == FPartnerKey)
                    {
                        // refresh position in Uni Hierarchy
                        ucoLowerPart.RefreshUnitHierarchy(UnitHierarchyChanges[i]);
                        break;
                    }
                }

                MessageProcessed = true;
            }

            return MessageProcessed;
        }

        #endregion
    }

    /// <summary>
    /// Manages the opening of a Partner's Edit Screen
    /// </summary>
    public static class TPartnerEditScreenManager
    {
        /// <summary>
        /// Opens an instance of the Partner Edit Screen
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AParentForm"></param>
        /// <returns></returns>
        public static void OpenForm(long APartnerKey,
            Form AParentForm)
        {
            TFrmPartnerEdit frm = new TFrmPartnerEdit(AParentForm);

            frm.SetParameters(TScreenMode.smEdit, APartnerKey);
            frm.Show();
        }
    }
}
