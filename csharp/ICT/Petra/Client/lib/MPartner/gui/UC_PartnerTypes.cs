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
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
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
            FLogic.InitialiseDelegatePartnerTypePropagationSelection(@OpenPartnerTypePropagationSelection);
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

        /// can be called from the hosting window to adjust the size of the control after resizing the window
        public void AdjustAfterResizing()
        {
            SetupDataGridVisualAppearance();
        }

        private void GrdPartnerTypes_DoubleClickCell(System.Object Sender, SourceGrid.CellContextEventArgs e)
        {
            FLogic.ChangeCheckedStateForRow(e.CellContext.Position.Row);
        }

        private void GrdPartnerTypes_EnterKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            FLogic.ChangeCheckedStateForRow(e.Row);
        }

        private void GrdPartnerTypes_SpaceKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            FLogic.ChangeCheckedStateForRow(e.Row);
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
    }
}