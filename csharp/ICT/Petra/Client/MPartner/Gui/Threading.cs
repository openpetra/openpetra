//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared.MPartner;
using System.Windows.Forms;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// Allows opening the Partner Edit screen from within a Thread (can be a
    /// background Thread or an UI Thread).
    ///
    /// </summary>
    public class TThreadedOpenPartnerEditScreen
    {
        private TScreenMode FScreenMode;
        private System.Int64 FPartnerKey;
        private TPartnerEditTabPageEnum FShowTabPage;
        private Int64 FSiteKeyForSelectingPartnerLocation;
        private Int64 FNewPartnerSiteKey;
        private Int64 FNewPartnerPartnerKey;
        private String FNewPartnerPartnerClass;
        private String FNewPartnerCountryCode;
        private String FNewPartnerAcquisitionCode;
        private Boolean FNewPartnerPrivatePartner;
        private Boolean FNewPartnerShowNewPartnerDialog;
        private Int64 FNewPartnerFamilyPartnerKey;
        private Int32 FNewPartnerFamilyLocationKey;
        private Int64 FNewPartnerFamilySiteKey;
        private Int32 FLocationKeyForSelectingPartnerLocation;

        #region TThreadedOpenPartnerEditScreen'

        /// <summary>
        /// See Ict.Petra.Client.MPartner.PartnerEdit for descriptions of the
        /// SetParameters procedures.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void SetParameters(TScreenMode AScreenMode, System.Int64 APartnerKey)
        {
            SetParameters(AScreenMode, APartnerKey, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AShowTabPage"></param>
        public void SetParameters(TScreenMode AScreenMode, System.Int64 APartnerKey, TPartnerEditTabPageEnum AShowTabPage)
        {
            FScreenMode = AScreenMode;
            FPartnerKey = APartnerKey;
            FShowTabPage = AShowTabPage;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="ALocationKey"></param>
        public void SetParameters(TScreenMode AScreenMode, Int64 APartnerKey, Int64 ASiteKey, Int32 ALocationKey)
        {
            SetParameters(AScreenMode, APartnerKey, ASiteKey, ALocationKey, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="ALocationKey"></param>
        /// <param name="AShowTabPage"></param>
        public void SetParameters(TScreenMode AScreenMode,
            Int64 APartnerKey,
            Int64 ASiteKey,
            Int32 ALocationKey,
            TPartnerEditTabPageEnum AShowTabPage)
        {
            FScreenMode = AScreenMode;
            FPartnerKey = APartnerKey;
            FSiteKeyForSelectingPartnerLocation = ASiteKey;
            FLocationKeyForSelectingPartnerLocation = ALocationKey;
            FShowTabPage = AShowTabPage;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="ANewPartnerFamilyPartnerKey"></param>
        /// <param name="ANewPartnerFamilyLocationKey"></param>
        /// <param name="ANewPartnerFamilySiteKey"></param>
        /// <param name="AShowNewPartnerDialog"></param>
        /// <param name="AShowTabPage"></param>
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
            FScreenMode = AScreenMode;
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
        /// overload
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="ANewPartnerFamilyPartnerKey"></param>
        /// <param name="ANewPartnerFamilyLocationKey"></param>
        /// <param name="ANewPartnerFamilySiteKey"></param>
        /// <param name="AShowNewPartnerDialog"></param>
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
        /// overload
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="ANewPartnerFamilyPartnerKey"></param>
        /// <param name="ANewPartnerFamilyLocationKey"></param>
        /// <param name="ANewPartnerFamilySiteKey"></param>
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
        /// overload
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="ANewPartnerFamilyPartnerKey"></param>
        /// <param name="ANewPartnerFamilyLocationKey"></param>
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
        /// overload
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="ANewPartnerFamilyPartnerKey"></param>
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
        /// overload
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
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
        /// overload
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="AAcquisitionCode"></param>
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
        /// overload
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ACountryCode"></param>
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
        /// overload
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        public void SetParameters(TScreenMode AScreenMode, String APartnerClass, System.Int64 ASiteKey, System.Int64 APartnerKey)
        {
            SetParameters(AScreenMode, APartnerClass, ASiteKey, APartnerKey, "", "", false, -1, -1, -1, true, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        public void SetParameters(TScreenMode AScreenMode, String APartnerClass, System.Int64 ASiteKey)
        {
            SetParameters(AScreenMode, APartnerClass, ASiteKey, -1, "", "", false, -1, -1, -1, true, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AScreenMode"></param>
        /// <param name="APartnerClass"></param>
        public void SetParameters(TScreenMode AScreenMode, String APartnerClass)
        {
            SetParameters(AScreenMode, APartnerClass, -1, -1, "", "", false, -1, -1, -1, true, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AScreenMode"></param>
        public void SetParameters(TScreenMode AScreenMode)
        {
            SetParameters(AScreenMode, "FAMILY", -1, -1, "", "", false, -1, -1, -1, true, TPartnerEditTabPageEnum.petpDefault);
        }

        /// <summary>
        /// Shows the Partner Edit screen for a new Partner.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void ShowForExistingPartner(Form AParentForm)
        {
            TFrmPartnerEdit frmPEDS;

            // MessageBox.Show('From Thread: about to show PartnerEdit screen for existing Partner...');
            try
            {
                frmPEDS = new TFrmPartnerEdit(AParentForm);
                frmPEDS.SetParameters(FScreenMode,
                    FPartnerKey,
                    FSiteKeyForSelectingPartnerLocation,
                    FLocationKeyForSelectingPartnerLocation,
                    FShowTabPage);

                // MessageBox.Show('From Thread: now showing PartnerEdit screen!');
                frmPEDS.Show();
            }
            catch (Exception Exp)
            {
                MessageBox.Show("Exception in TThreadedOpenPartnerEditScreen.ShowForExistingPartner:" + Exp.ToString());
            }
        }

        /// <summary>
        /// Shows the Partner Edit screen for a new Partner.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void ShowForNewPartner(Form AParentForm)
        {
            TFrmPartnerEdit frmPEDS;

            // MessageBox.Show('From Thread: about to show PartnerEdit screen for new Partner...');
            try
            {
                frmPEDS = new TFrmPartnerEdit(AParentForm);
                frmPEDS.SetParameters(FScreenMode,
                    FNewPartnerPartnerClass,
                    FNewPartnerSiteKey,
                    FNewPartnerPartnerKey,
                    FNewPartnerCountryCode,
                    FNewPartnerAcquisitionCode,
                    FNewPartnerPrivatePartner,
                    FNewPartnerFamilyPartnerKey,
                    FNewPartnerFamilyLocationKey,
                    FNewPartnerFamilySiteKey,
                    FNewPartnerShowNewPartnerDialog);

                // MessageBox.Show('From Thread: now showing PartnerEdit screen!');
                frmPEDS.Show();
            }
            catch (Exception Exp)
            {
                MessageBox.Show("Exception in TThreadedOpenPartnerEditScreen.ShowForNewPartner:" + Exp.ToString());
            }
        }

        #endregion
    }
}