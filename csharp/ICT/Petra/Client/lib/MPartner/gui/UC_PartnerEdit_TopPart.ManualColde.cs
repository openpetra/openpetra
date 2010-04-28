/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2010 by OM International
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

using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_PartnerEdit_TopPart
    {
        #region Fields

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private String FPartnerClass;

        /// <summary>Used for keeping track of data verification errors</summary>
        private TVerificationResultCollection FVerificationResultCollection;

        #endregion

        #region Events

        /// <summary>
        /// This Event is thrown when the 'main data' of a DataTable for a certain
        /// PartnerClass has changed.
        ///
        /// </summary>
        public event TPartnerClassMainDataChangedHandler PartnerClassMainDataChanged;

        #endregion

        #region Properties

        /// <summary>UIConnector that the screen uses</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }

        /// <summary>Holds verification results.</summary>
        public TVerificationResultCollection VerificationResultCollection
        {
            get
            {
                return FVerificationResultCollection;
            }

            set
            {
                FVerificationResultCollection = value;
            }
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Shows the data that is in FMainDS
        /// </summary>
        public void ShowData()
        {
            FPartnerClass = FMainDS.PPartner[0].PartnerClass.ToString();

            ShowData(FMainDS.PPartner[0]);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AIncludePartnerClass"></param>
        /// <returns></returns>
        public String PartnerQuickInfo(Boolean AIncludePartnerClass)
        {
            String TmpString;

            TmpString = txtPartnerKey.Text + "   ";

            if (!FMainDS.PPartner[0].IsPartnerShortNameNull())
            {
                TmpString = TmpString + FMainDS.PPartner[0].PartnerShortName;
            }

            if (AIncludePartnerClass)
            {
                TmpString = TmpString + "   [" + FPartnerClass.ToString() + ']';
            }

            return TmpString;
        }

        #endregion

        #region Actions

        private void MaintainWorkerField(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}