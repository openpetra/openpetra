//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SourceGrid;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Views;
using DevAge.ComponentModel;
using DevAge.ComponentModel.Converter;
using DevAge.ComponentModel.Validator;
using DevAge.Drawing;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using System.Globalization;
using GNU.Gettext;

namespace Ict.Petra.Client.MCommon.Gui
{
    /// <summary>
    /// Contains User Control Office Specific Data Labels
    /// </summary>
    public partial class TUCOfficeSpecificDataLabels : System.Windows.Forms.UserControl
    {
        /// <summary>Object that holds the logic for this screen</summary>
        private TUCOfficeSpecificDataLabelsLogic FLogic;
        private Int64 FPartnerKey;
        private Int32 FApplicationKey;
        private Int64 FRegistrationOffice;
        private Boolean FUserControlInitialised;
        private TDelegateGetPartnerShortName FDelegateGetPartnerShortName;

        /// <summary>
        /// constructor
        /// </summary>
        public TUCOfficeSpecificDataLabels() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblInfo.Text = Catalog.GetString("Office Specific Data Labels");
            #endregion
        }

        private void GrdOfficeSpecificGrid_SizeChanged(System.Object sender, System.EventArgs e)
        {
            // inform the logic that the grid has been resized
            if (FLogic != null)
            {
                FLogic.ActUponGridSizeChanged();
            }
        }

        /// <summary>
        /// frees some static objects in FLogic.
        /// </summary>
        /// <param name="Disposing"></param>
        protected void SpecialDispose(Boolean Disposing)
        {
            FLogic.FreeStaticObjects();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADelegateFunction"></param>
        public void InitializeDelegateGetPartnerShortName(TDelegateGetPartnerShortName ADelegateFunction)
        {
            // set the delegate function from the calling System.Object
            FDelegateGetPartnerShortName = ADelegateFunction;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataTableValuePartner"></param>
        /// <param name="ADataTableValueApplication"></param>
        /// <param name="AOfficeSpecificDataLabelUse"></param>
        public void InitialiseUserControl(PDataLabelValuePartnerTable ADataTableValuePartner,
            PDataLabelValueApplicationTable ADataTableValueApplication,
            TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse)
        {
            FLogic = new TUCOfficeSpecificDataLabelsLogic();

            // Set the delegate function in the logic System.Object (so it can call back).
            // This can not be done in procedure InitializeDelegateGetPartnerShortName because FLogic
            // does not yet exist then.
            // Set up Data Sets and Tables
            FLogic.InitialiseDataStructures(ADataTableValuePartner, ADataTableValueApplication);

            // Set up screen logic
            FLogic.PartnerKey = FPartnerKey;
            FLogic.ApplicationKey = FApplicationKey;
            FLogic.RegistrationOffice = FRegistrationOffice;
            FLogic.OfficeSpecificDataLabelUse = AOfficeSpecificDataLabelUse;
            FLogic.OfficeSpecificGrid = grdOfficeSpecificGrid;

// TODO            FLogic.UCStatusBarTextProvider = sbtUCOfficeSpecificDataLabels;
            FLogic.PossiblySomethingToSave += new TTellGuiToEnableSaveButton(this.@PossiblySaveSomething);

            // Set up Grid
            FLogic.SetupGridColumnsAndRows();

            // Signalize that the initialisation is done
            FUserControlInitialised = true;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataTable"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AOfficeSpecificDataLabelUse"></param>
        public void PerformDataBinding(PDataLabelValuePartnerTable ADataTable,
            System.Int64 APartnerKey,
            TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse)
        {
            FPartnerKey = APartnerKey;

            if (!FUserControlInitialised)
            {
                InitialiseUserControl(ADataTable, null, AOfficeSpecificDataLabelUse);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataTable"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AApplicationKey"></param>
        /// <param name="ARegistrationOffice"></param>
        /// <param name="AOfficeSpecificDataLabelUse"></param>
        public void PerformDataBinding(PDataLabelValueApplicationTable ADataTable,
            Int64 APartnerKey,
            Int32 AApplicationKey,
            Int64 ARegistrationOffice,
            TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse)
        {
            FPartnerKey = APartnerKey;
            FApplicationKey = AApplicationKey;
            FRegistrationOffice = ARegistrationOffice;

            if (!FUserControlInitialised)
            {
                InitialiseUserControl(null, ADataTable, AOfficeSpecificDataLabelUse);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASuccess"></param>
        public void DataSavedEventFired(Boolean ASuccess)
        {
            FLogic.DataSavedEventFired(ASuccess);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void SaveAllChanges()
        {
            if (FLogic != null)
            {
                FLogic.SaveAllChanges();
            }
        }

        private void PossiblySaveSomething()
        {
            if (this.ParentForm != null)
            {
                ((TFrmPetraEditUtils)((IFrmPetraEdit) this.ParentForm).GetPetraUtilsObject()).SetChangedFlag();
            }
        }
    }
}