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

namespace Ict.Petra.Client.CommonControls.Logic
{
    /// <summary>
    /// Allows calls to Screens that are defined in various Assemblies
    /// from within various Assemblies. This resolves the problem of
    /// circular references between Assemblies that would come up with
    /// conventional calls to Screens.
    /// </summary>
    public static class TCommonScreensForwarding
    {
        static TDelegateOpenPartnerFindScreen FOpenPartnerFindScreen;
        static TDelegateOpenPartnerFindByBankDetailsScreen FOpenPartnerFindByBankDetailsScreen;
        static TDelegateOpenBankFindDialog FOpenBankFindDialog;
        static TDelegateOpenConferenceFindScreen FOpenConferenceFindScreen;
        static TDelegateOpenEventFindScreen FOpenEventFindScreen;
        static TDelegateOpenExtractFindScreen FOpenExtractFindScreen;
        static TDelegateOpenExtractMasterScreen FOpenExtractMasterScreen;
        static TDelegateOpenDonorRecipientHistoryScreen FOpenDonorRecipientHistoryScreen;
        static TDelegateOpenPartnerEditScreen FOpenPartnerEditScreen;
        static TDelegateOpenExtractMasterScreenHidden FOpenExtractMasterScreenHidden;
        static TDelegateOpenRangeFindScreen FOpenRangeFindScreen;
        static TDelegateOpenOccupationCodeFindScreen FOpenOccupationCodeFindScreen;
        static TDelegateOpenGetMergeDataDialog FOpenGetMergeDataDialog;
        static TDelegateOpenPrintPartnerDialog FOpenPrintPartnerDialog;
        static TDelegateTaxDeductiblePctAdjust FTaxDeductiblePctAdjust;
        static TDelegateOpenPrintUnitHierarchy FOpenPrintUnitHierarchy;

        /// <summary>
        /// This property is used to provide a function which opens a modal Partner Find screen.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenPartnerFindScreen OpenPartnerFindScreen
        {
            get
            {
                return FOpenPartnerFindScreen;
            }

            set
            {
                FOpenPartnerFindScreen = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens a modal Partner Find screen with only the
        /// Find By Bank Details tab enabled.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenPartnerFindByBankDetailsScreen OpenPartnerFindByBankDetailsScreen
        {
            get
            {
                return FOpenPartnerFindByBankDetailsScreen;
            }

            set
            {
                FOpenPartnerFindByBankDetailsScreen = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens a modal Partner Find screen with only the
        /// Find By Bank Details tab enabled.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenBankFindDialog OpenBankFindDialog
        {
            get
            {
                return FOpenBankFindDialog;
            }

            set
            {
                FOpenBankFindDialog = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens the conference find screen.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenConferenceFindScreen OpenConferenceFindScreen
        {
            get
            {
                return FOpenConferenceFindScreen;
            }

            set
            {
                FOpenConferenceFindScreen = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens the event find screen.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenEventFindScreen OpenEventFindScreen
        {
            get
            {
                return FOpenEventFindScreen;
            }

            set
            {
                FOpenEventFindScreen = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens the extract find screen.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenExtractFindScreen OpenExtractFindScreen
        {
            get
            {
                return FOpenExtractFindScreen;
            }

            set
            {
                FOpenExtractFindScreen = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens the Extract Master screen.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenExtractMasterScreen OpenExtractMasterScreen
        {
            get
            {
                return FOpenExtractMasterScreen;
            }

            set
            {
                FOpenExtractMasterScreen = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens the Donor Recipient History screen.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenDonorRecipientHistoryScreen OpenDonorRecipientHistoryScreen
        {
            get
            {
                return FOpenDonorRecipientHistoryScreen;
            }

            set
            {
                FOpenDonorRecipientHistoryScreen = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens the Donor Recipient History screen.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenPartnerEditScreen OpenPartnerEditScreen
        {
            get
            {
                return FOpenPartnerEditScreen;
            }

            set
            {
                FOpenPartnerEditScreen = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens the Extract Master screen but does not show it.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenExtractMasterScreenHidden OpenExtractMasterScreenHidden
        {
            get
            {
                return FOpenExtractMasterScreenHidden;
            }

            set
            {
                FOpenExtractMasterScreenHidden = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens the Range find screen.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenRangeFindScreen OpenRangeFindScreen
        {
            get
            {
                return FOpenRangeFindScreen;
            }

            set
            {
                FOpenRangeFindScreen = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens the Occupation Code find screen.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenOccupationCodeFindScreen OpenOccupationCodeFindScreen
        {
            get
            {
                return FOpenOccupationCodeFindScreen;
            }

            set
            {
                FOpenOccupationCodeFindScreen = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens the Merge Select dialog.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenGetMergeDataDialog OpenGetMergeDataDialog
        {
            get
            {
                return FOpenGetMergeDataDialog;
            }

            set
            {
                FOpenGetMergeDataDialog = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens the Print Partner report dialog.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenPrintPartnerDialog OpenPrintPartnerDialog
        {
            get
            {
                return FOpenPrintPartnerDialog;
            }

            set
            {
                FOpenPrintPartnerDialog = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which opens the Print Partner report dialog.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenPrintUnitHierarchy OpenPrintUnitHierarchy
        {
            get
            {
                return FOpenPrintUnitHierarchy;
            }

            set
            {
                FOpenPrintUnitHierarchy = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which does a TaxDeductiblePct adjustment.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateTaxDeductiblePctAdjust TaxDeductiblePctAdjust
        {
            get
            {
                return FTaxDeductiblePctAdjust;
            }

            set
            {
                FTaxDeductiblePctAdjust = value;
            }
        }
    }
}