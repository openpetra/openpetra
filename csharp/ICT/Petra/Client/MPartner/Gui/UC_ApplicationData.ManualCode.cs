//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2012 by OM International
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
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;


namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_ApplicationData
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private IndividualDataTDS FMainDS;          // FMainDS is NOT of Type 'PartnerEditTDS' in this UserControl!!!
        private PartnerEditTDS FPartnerEditTDS;

        #region Events

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #endregion

        #region Properties

        /// <summary>used for passing through the Clientside Proxy for the UIConnector</summary>
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

        /// dataset for the whole screen
        public PartnerEditTDS MainDS
        {
            get
            {
                return FPartnerEditTDS;
            }

            set
            {
                FPartnerEditTDS = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets up the screen logic, retrieves data, databinds the Grid and the Detail
        /// UserControl.
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseUserControl()
        {
            FMainDS = new IndividualDataTDS();

            // Merge DataTables which are held only in PartnerEditTDS into IndividualDataTDS so that we can access that data in here!
            FMainDS.Merge(FPartnerEditTDS);

            ucoApplications.PetraUtilsObject = FPetraUtilsObject;
            ucoApplications.MainDS = FMainDS;
            ucoApplications.PartnerEditUIConnector = FPartnerEditUIConnector;
            ucoApplications.SpecialInitUserControl(FMainDS);

            // Hook up RecalculateScreenParts Event
            ucoApplications.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(DoRecalculateScreenParts);
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <remarks>May be called by the Form that hosts this UserControl to invoke the data validation of
        /// the UserControl.</remarks>
        /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
        /// user, otherwise set it to false.</param>
        /// <param name="AValidateSpecificControl">Pass in a Control to restrict Data Validation error checking to a
        /// specific Control for which Data Validation errors might have been recorded. (Default=null).
        /// <para>
        /// This is useful for restricting Data Validation error checking to the current TabPage of a TabControl in order
        /// to only display Data Validation errors that pertain to the current TabPage. To do this, pass in a TabControl in
        /// this Argument.
        /// </para>
        /// </param>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        public bool ValidateAllData(bool AProcessAnyDataValidationErrors, Control AValidateSpecificControl = null)
        {
            bool ReturnValue = true;

            ReturnValue = ucoApplications.ValidateAllData(true, AProcessAnyDataValidationErrors, AValidateSpecificControl);

            return ReturnValue;
        }

        /// <summary>
        /// Gets the data from all UserControls on this TabControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        /// <returns>void</returns>
        public void GetDataFromControls()
        {
            ucoApplications.GetDataFromControls2();

            if (!FPartnerEditTDS.Tables.Contains(PmGeneralApplicationTable.GetTableName()))
            {
                FPartnerEditTDS.Tables.Add(new PmGeneralApplicationTable());
            }

            if (!FPartnerEditTDS.Tables.Contains(PmShortTermApplicationTable.GetTableName()))
            {
                FPartnerEditTDS.Tables.Add(new PmShortTermApplicationTable());
            }

            if (!FPartnerEditTDS.Tables.Contains(PmYearProgramApplicationTable.GetTableName()))
            {
                FPartnerEditTDS.Tables.Add(new PmYearProgramApplicationTable());
            }

            FPartnerEditTDS.Tables[PmShortTermApplicationTable.GetTableName()].Rows.Clear();
            FPartnerEditTDS.Tables[PmShortTermApplicationTable.GetTableName()].Merge(FMainDS.PmShortTermApplication);
            FPartnerEditTDS.Tables[PmYearProgramApplicationTable.GetTableName()].Rows.Clear();
            FPartnerEditTDS.Tables[PmYearProgramApplicationTable.GetTableName()].Merge(FMainDS.PmYearProgramApplication);
            FPartnerEditTDS.Tables[PmGeneralApplicationTable.GetTableName()].Rows.Clear();
            FPartnerEditTDS.Tables[PmGeneralApplicationTable.GetTableName()].Merge(FMainDS.PmGeneralApplication);
        }

        /// <summary>
        /// Called when data got saved in the screen. This Method takes over changed data
        /// into FMainDS, which is different than the Partner Edit screen's FMainDS, in
        /// order to have current data on which decisions on whether to refresh certain
        /// parts of the 'Overview' need to be updated.
        /// </summary>
        /// <param name="APartnerAttributesOrRelationsChanged">NOT USED IN THIS CONTEXT!  (Set to true by the SaveChanges Method
        /// of the Partner Edit screen if PartnerAttributes or Relationships have changed.)</param>
        public void RefreshPersonnelDataAfterMerge(bool APartnerAttributesOrRelationsChanged)
        {
            //
            // Need to merge Tables from PartnerEditTDS into IndividualDataTDS so the updated s_modification_id_t of modififed Rows is held correctly in IndividualDataTDS, too!
            //

            // ...but first empty relevant DataTables to ensure that DataRows that got deleted in FPartnerEditTDS are reflected in FMainDS (just performing a Merge wouldn't remove them!)
            if (FMainDS.Tables.Contains(PPartnerLocationTable.GetTableName()))
            {
                FMainDS.Tables[PPartnerLocationTable.GetTableName()].Rows.Clear();
            }

            if (FMainDS.Tables.Contains(PLocationTable.GetTableName()))
            {
                FMainDS.Tables[PLocationTable.GetTableName()].Rows.Clear();
            }

            if (FMainDS.Tables.Contains(PPartnerRelationshipTable.GetTableName()))
            {
                FMainDS.Tables[PPartnerRelationshipTable.GetTableName()].Rows.Clear();
            }

            // Now perform the Merge operation
            FMainDS.Merge(FPartnerEditTDS);

            // Call AcceptChanges on IndividualDataTDS so that we don't have any changed data anymore (this is done to PartnerEditTDS, too, after this Method returns)!
            FMainDS.AcceptChanges();
        }

        /// <summary>
        /// returns number of application records existing for current person record
        /// </summary>
        /// <returns>int</returns>
        public int CountApplications()
        {
            return ucoApplications.CountApplications();
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        /// <returns>void</returns>
        public void AdjustAfterResizing()
        {
        }

        #endregion


        #region Private Methods

        private void DoRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            // trigger event so outer controls can react
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(sender, e);
            }
        }

        #endregion


        #region Event Handlers

        #endregion

        #region Menu and command key handlers for our user controls

        /// <summary>
        /// Handler for command key processing
        /// </summary>
        private bool ProcessCmdKeyManual(ref Message msg, Keys keyData)
        {
            if (this.ucoApplications.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}