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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MPartner;
using SourceGrid;
using Ict.Common.Controls;
using System.Globalization;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// A UserControl that holds the same information as the Petra 4GL Partner Types
    /// section in the Petra 4GL Partner Edit screen.
    ///
    /// Since this UserControl can be re-used, it is no longer necessary to recreate
    /// all the layout and fields and verification rules in other places than the
    /// Partner Edit screen.
    /// </summary>
    public partial class TUCPartnerTypes : TPetraUserControl
    {
        private TFrmPetraEditUtils FPetraUtilsObject;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private TUCPartnerTypesLogic FLogic;

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

        /// helper object for the whole screen
        public TFrmPetraEditUtils PetraUtilsObject
        {
            set
            {
                FPetraUtilsObject = value;
            }
        }

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        /// <summary>
        /// constructor
        /// </summary>
        public TUCPartnerTypes() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion

            // define the screen's logic
            FLogic = new TUCPartnerTypesLogic();

            // TODO: add PetraUtilsObject from parent form
            //this.SetStatusBarText(this.grdPartnerTypes, "Special Types List. Tick/untick selected Special Type with <Enter> or <Space> key or by double-clicking desired Special Type.");
        }

        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
        }

        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        private PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable OpenPartnerTypePropagationSelection(String APartnerTypeCode,
            String AAction)
        {
            PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable ReturnValue = null;

// TODO OpenPartnerTypePropagationSelection
#if TODO
            TPartnerTypeFamilyMembersPropagationSelectionWinForm PartnerTypeFamilyMembersPropagationSelectionWinForm;
            PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable FamilyMembersPromotionTable;

            PartnerTypeFamilyMembersPropagationSelectionWinForm = new TPartnerTypeFamilyMembersPropagationSelectionWinForm();
            PartnerTypeFamilyMembersPropagationSelectionWinForm.SetParameters(FMainDS.PFamily[0].PartnerKey,
                FPartnerEditUIConnector,
                APartnerTypeCode,
                AAction);

            if (PartnerTypeFamilyMembersPropagationSelectionWinForm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                ReturnValue = new PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable();
            }
            else
            {
                if (PartnerTypeFamilyMembersPropagationSelectionWinForm.GetReturnedParameters(out FamilyMembersPromotionTable))
                {
                    ReturnValue = FamilyMembersPromotionTable;
                }
                else
                {
                    ReturnValue = new PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable();
                }
            }
#endif
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitUserControl()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void SpecialInitUserControl()
        {
            // Set up screen logic
            FLogic.MultiTableDS = (PartnerEditTDS)FMainDS;
            FLogic.PartnerEditUIConnector = FPartnerEditUIConnector;
            FLogic.PetraUtilsObject = FPetraUtilsObject;
// TODO PartnerTypeFamilyMembersPropagationSelectionWinForm Dialog still missing
#if TODO
            FLogic.InitialiseDelegatePartnerTypePropagationSelection(@OpenPartnerTypePropagationSelection);
#endif
            FLogic.DataGrid = grdPartnerTypes;
            FLogic.LoadTypes();
            FLogic.LoadDataOnDemand();

            // Create temp table for grid display
            FLogic.CreateTempPartnerTypesTable();
            FLogic.FillTempPartnerTypesTable();

            // Create SourceDataGrid columns
            FLogic.CreateColumns();

            // DataBindingrelated stuff
            FLogic.DataBindGrid();

            // Setup the DataGrid's visual appearance
            SetupDataGridVisualAppearance();

            // Hook up RecalculateScreenParts Event that is fired by FLogic
            FLogic.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(this.RethrowRecalculateScreenParts);
            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpPartnerTypes));
        }

        /// <summary>
        /// Rebuild internal DataTable that is used for the Grid.
        /// </summary>
        public void RefreshDataGrid()
        {
            /*
             * This is needed to reflect the re-ordering of Partner Types that took place
             * due to the adding/removing of a Partner Type.
             */
            FLogic.FillTempPartnerTypesTable();

            // scroll to top of grid
            grdPartnerTypes.ShowCell(0);
        }

        /// can be called from the hosting window to adjust the size of the control after resizing the window
        public void AdjustAfterResizing()
        {
            SetupDataGridVisualAppearance();
        }

        private void GrdPartnerTypes_EnterKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            FLogic.ChangeCheckedStateForRow(e.Row);
        }

        private void GrdPartnerTypes_SpaceKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            FLogic.ChangeCheckedStateForRow(e.Row);

            // I'm not entirely sure why...
            // but without this line a checkbox is not updated when it is first clicked with the mouse and then the spacebar is pressed
            grdPartnerTypes.Refresh();
        }

        /// <summary>
        /// Sets up the visual appearance of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupDataGridVisualAppearance()
        {
            grdPartnerTypes.AutoSizeCells();

            // it is necessary to reassign the width because the columns don't take up the maximum width
            grdPartnerTypes.Width = 510;
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <remarks>May be called by the Form that hosts this UserControl to invoke the data validation of
        /// the UserControl.</remarks>
        /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing
        /// to another record, otherwise set it to false.</param>
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
        public bool ValidateAllData(bool ARecordChangeVerification,
            TErrorProcessingMode ADataValidationProcessingMode,
            Control AValidateSpecificControl = null)
        {
            bool ReturnValue = true;

// TODO
//            bool ReturnValue = false;
//            Control ControlToValidate;
//            PSubscriptionRow CurrentRow;
//
//            CurrentRow = GetSelectedDetailRow();
//
//            if (CurrentRow != null)
//            {
//                if (AValidateSpecificControl != null)
//                {
//                    ControlToValidate = AValidateSpecificControl;
//                }
//                else
//                {
//                    ControlToValidate = this.ActiveControl;
//                }
//
//                GetDetailsFromControls(CurrentRow);
//
//                // TODO Generate automatic validation of data, based on the DB Table specifications (e.g. 'not null' checks)
//                ValidateDataDetailsManual(CurrentRow);
//
//                if (ADataValidationProcessingMode != TErrorProcessingMode.Epm_None)
//                {
//                    // Only process the Data Validations here if ControlToValidate is not null.
//                    // It can be null if this.ActiveControl yields null - this would happen if no Control
//                    // on this UserControl has got the Focus.
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
//            else
//            {
//                ReturnValue = true;
//            }
//
//            if(ReturnValue)
//            {
//                // Remove a possibly shown Validation ToolTip as the data validation succeeded
//                FPetraUtilsObject.ValidationToolTip.RemoveAll();
//            }

            return ReturnValue;
        }
    }
}