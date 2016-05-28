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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_LocalPartnerData
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        #region Public Methods

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

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
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
        private void DataSavingStarted(System.Object sender, System.EventArgs e)
        {
            ucoGrid.GetDataFromControlsManual();
        }

        /// <summary>
        /// Gets the data from all controls on this UserControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls2()
        {
//            GetDataFromControls(FMainDS.PBank[0]);
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamically loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public void SpecialInitUserControl()
        {
            LoadDataOnDemand();

            ucoGrid.InitialiseUserControlAndShowData(FMainDS.PDataLabelValuePartner,
                ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey,
                MCommonTypes.PartnerClassStringToOfficeSpecificDataLabelUseEnum(((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerClass));

            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpOfficeSpecific));

            this.SizeChanged += new System.EventHandler(ucoGrid.GrdLocalDataLabelValues_SizeChanged);


            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <remarks>May be called by the Form that hosts this UserControl to invoke the data validation of
        /// the UserControl.</remarks>
        /// <param name="ADataValidationProcessingMode">Set to TErrorProcessingMode.Epm_All if data validation errors should be shown to the
        /// user, otherwise set it to TErrorProcessingMode.Epm_None.</param>
        /// <param name="AValidateSpecificControl">Pass in a Control to restrict Data Validation error checking to a
        /// specific Control for which Data Validation errors might have been recorded. (Default=this.ActiveControl).
        /// <para>
        /// This is useful for restricting Data Validation error checking to the current TabPage of a TabControl in order
        /// to only display Data Validation errors that pertain to the current TabPage. To do this, pass in a TabControl in
        /// this Argument.
        /// </para>
        /// </param>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        public bool ValidateAllData(TErrorProcessingMode ADataValidationProcessingMode, Control AValidateSpecificControl = null)
        {
            bool ReturnValue = true;

// TODO
//        bool ReturnValue = false;
//        Control ControlToValidate;
//        if (AValidateSpecificControl != null)
//        {
//            ControlToValidate = AValidateSpecificControl;
//        }
//        else
//        {
//            ControlToValidate = this.ActiveControl;
//        }
//
//        GetDataFromControls(FMainDS.PSubscription[0]);
//
//            // TODO Generate automatic validation of data, based on the DB Table specifications (e.g. 'not null' checks)
//            ValidateDataManual(FMainDS.PSubscription[0]);
//
//            if (ADataValidationProcessingMode != TErrorProcessingMode.Epm_None)
//            {
//                // Only process the Data Validations here if ControlToValidate is not null.
//                // It can be null if this.ActiveControl yields null - this would happen if no Control
//                // on this UserControl has got the Focus.
//                if (ControlToValidate != null)
//                {
//                    if(ControlToValidate.FindUserControlOrForm(true) == this)
//                    {
//                        ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(false, FPetraUtilsObject.VerificationResultCollection,
//                            this.GetType(), ControlToValidate.FindUserControlOrForm(true).GetType());
//                    }
//                    else
//                    {
//                        ReturnValue = true;
//                    }
//                }
//            }
//
//        if(ReturnValue)
//        {
//            // Remove a possibly shown Validation ToolTip as the data validation succeeded
//            FPetraUtilsObject.ValidationToolTip.RemoveAll();
//        }

            return ReturnValue;
        }

        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
        }

        /// <summary>
        /// Loads Partner Types Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue = true;

            // Load Partner Types, if not already loaded
            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PDataLabelValuePartner == null)
                {
                    FMainDS.Tables.Add(new PDataLabelValuePartnerTable());
                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading)
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataLocalPartnerDataValues());

                    // Make DataRows unchanged
                    if (FMainDS.PDataLabelValuePartner.Rows.Count > 0)
                    {
                        FMainDS.PDataLabelValuePartner.AcceptChanges();
                    }
                }

                if (FMainDS.PDataLabelValuePartner == null)
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            return ReturnValue;
        }

        #endregion
    }
}