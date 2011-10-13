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
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.Interfaces.MPartner;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_Relationships
    {

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private TUCRelationshipsLogic FLogic;
        
        #region Public Methods

//        /// <summary>
//        /// Gets the data from all controls on this UserControl.
//        /// The data is stored in the DataTables/DataColumns to which the Controls
//        /// are mapped.
//        /// </summary>
//        public void GetDataFromControls2()
//        {
//            GetDataFromControls(FMainDS.PBank[0]);
//        }

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
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;
        
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
        
        /// <summary>todoComment</summary>
        public void SpecialInitUserControl()
        {

            // Set up screen logic
            FLogic.MultiTableDS = (PartnerEditTDS)FMainDS;
            FLogic.PartnerEditUIConnector = FPartnerEditUIConnector;
            FLogic.LoadDataOnDemand();
            
            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Description", FMainDS.PPartnerRelationship.Columns["RelationDescription"]);
            grdDetails.AddTextColumn("Partner Key", FMainDS.PPartnerRelationship.Columns["OtherPartnerKey"]);
            grdDetails.AddTextColumn("Partner Name", FMainDS.PPartnerRelationship.Columns[PartnerEditTDSPPartnerRelationshipTable.GetPartnerShortNameDBName()]);
            grdDetails.AddTextColumn("Class", FMainDS.PPartnerRelationship.Columns[PartnerEditTDSPPartnerRelationshipTable.GetPartnerClassDBName()]);
            grdDetails.AddTextColumn("Comment", FMainDS.PPartnerRelationship.Columns[PPartnerRelationshipTable.GetCommentDBName()]);

            // Hook up RecalculateScreenParts Event that is fired by FLogic
            FLogic.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(this.RethrowRecalculateScreenParts);
            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpRelationships));

            if (grdDetails.Rows.Count > 1)
            {
                grdDetails.SelectRowInGrid(1);
            }
            else
            {
                btnDelete.Enabled = false;
                btnEditOtherPartner.Enabled = false;
            }
        }
        
        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }
        
        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
            FLogic = new TUCRelationshipsLogic();

            FMainDS.Tables.Add(new PartnerEditTDSPPartnerRelationshipTable());
            FMainDS.InitVars();
        }

        private void ShowDataManual()
        {
        }

        private void ShowDetailsManual(PPartnerRelationshipRow ARow)
        {
            long RelationPartnerKey;

            btnDelete.Enabled = false;
            btnEditOtherPartner.Enabled = false;

            if (ARow != null)
            {
                btnDelete.Enabled = true;
                
                // depending on the relation select other partner to be edited
                if (ARow.PartnerKey == ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey)
                {
                    RelationPartnerKey = GetSelectedDetailRow().RelationKey;
                }
                else
                {
                    RelationPartnerKey = GetSelectedDetailRow().PartnerKey;
                }
                
                if (RelationPartnerKey != 0)
                {
                    btnEditOtherPartner.Enabled = true;
                }
            }
        }
        
        private void GetDetailDataFromControlsManual(PPartnerRelationshipRow ARow)
        {
            txtPPartnerRelationshipPartnerKey.ValueChanged += new TDelegatePartnerChanged(txtPPartnerRelationshipPartnerKey_ValueChanged);
            txtPPartnerRelationshipRelationKey.ValueChanged += new TDelegatePartnerChanged(txtPPartnerRelationshipRelationKey_ValueChanged);
        }

        void txtPPartnerRelationshipPartnerKey_ValueChanged(long APartnerKey, string APartnerShortName, bool AValidSelection)
        {
            PartnerEditTDSPPartnerRelationshipRow CurrentRow;
            string PartnerShortName;
            TPartnerClass PartnerClass;

            if (AValidSelection) 
            {
                if (APartnerKey != ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey
                    && APartnerKey != 0)
                {
                    CurrentRow = GetSelectedDetailRow();
                    if (CurrentRow.PartnerKey != APartnerKey)
                    {
                        CurrentRow.PartnerKey       = APartnerKey;
                        FPartnerEditUIConnector.GetPartnerShortName (APartnerKey, out PartnerShortName, out PartnerClass);
                        CurrentRow.PartnerShortName = PartnerShortName;
                        CurrentRow.PartnerClass     = PartnerClass.ToString();
                    }
                }
            }
        }

        void txtPPartnerRelationshipRelationKey_ValueChanged(long APartnerKey, string APartnerShortName, bool AValidSelection)
        {
            PartnerEditTDSPPartnerRelationshipRow CurrentRow;
            string PartnerShortName;
            TPartnerClass PartnerClass;
            
            if (AValidSelection) 
            {
                if (APartnerKey != ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey
                    && APartnerKey != 0)
                {
                    CurrentRow = GetSelectedDetailRow();
                    if (CurrentRow.PartnerKey != APartnerKey)
                    {
                        CurrentRow.PartnerKey       = APartnerKey;
                        FPartnerEditUIConnector.GetPartnerShortName (APartnerKey, out PartnerShortName, out PartnerClass);
                        CurrentRow.PartnerShortName = PartnerShortName;
                        CurrentRow.PartnerClass     = PartnerClass.ToString();
                    }
                }
            }
        }
        
        private void NewRow(System.Object sender, EventArgs e)
        {
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            CreateNewPPartnerRelationship();
            
            // Fire OnRecalculateScreenParts event: reset counter in tab header
            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }

        private void NewRowManual(ref PartnerEditTDSPPartnerRelationshipRow ARow)
        {
            // Initialize relation with key of this partner on both sides, needs to be changed by user
            ARow.PartnerKey = ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey;
            ARow.RelationName = "";
            ARow.RelationKey = ARow.PartnerKey;
        }

        private int CurrentRowIndex()
        {
            //TODO WB: candidate for central method but not there yet?
            int rowIndex = -1;

            SourceGrid.RangeRegion selectedRegion = grdDetails.Selection.GetSelectionRegion();

            if ((selectedRegion != null) && (selectedRegion.GetRowsIndex().Length > 0))
            {
                rowIndex = selectedRegion.GetRowsIndex()[0];
            }

            return rowIndex;
        }

        private void SelectByIndex(int rowIndex)
        {
            //TODO WB: candidate for central method but not there yet?
            if (rowIndex >= grdDetails.Rows.Count)
            {
                rowIndex = grdDetails.Rows.Count - 1;
            }

            if ((rowIndex < 1) && (grdDetails.Rows.Count > 1))
            {
                rowIndex = 1;
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);
            }
            else
            {
                FPreviouslySelectedDetailRow = null;
            }
        }
        
        private void DeleteRow(System.Object sender, EventArgs e)
        {
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            //TODO WB: candidate for central method but not there yet?
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            int rowIndex = CurrentRowIndex();
            FPreviouslySelectedDetailRow.Delete();
            FPetraUtilsObject.SetChangedFlag();
            SelectByIndex(rowIndex);

            // Fire OnRecalculateScreenParts event: reset counter in tab header
            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }

        private void EditOtherPartner(System.Object sender, EventArgs e)
        {
            long RelationPartnerKey;
            
            if (GetSelectedDetailRow() == null)
            {
                return;
            }

            // depending on the relation select other partner to be edited
            if (GetSelectedDetailRow().PartnerKey == ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey)
            {
                RelationPartnerKey = GetSelectedDetailRow().RelationKey;
            }
            else
            {
                RelationPartnerKey = GetSelectedDetailRow().PartnerKey;
            }
                
    
            if (RelationPartnerKey == 0)
            {
                return;
            }
            
            this.Cursor = Cursors.WaitCursor;

            try
            {
                TFrmPartnerEdit frm = new TFrmPartnerEdit(this.Handle);

                frm.SetParameters(TScreenMode.smEdit, RelationPartnerKey);
                frm.Show();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        
        #endregion
        
    }
}