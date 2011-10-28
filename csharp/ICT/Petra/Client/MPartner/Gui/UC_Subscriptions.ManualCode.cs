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
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Gui;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_Subscriptions
    {

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
        }

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
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// Loads Partner Types Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        public Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            // Load Partner Types, if not already loaded
            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PSubscription == null)
                {
                    FMainDS.Tables.Add(new PSubscriptionTable());
                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading)
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataSubscriptions());

                    // Make DataRows unchanged
                    if (FMainDS.PSubscription.Rows.Count > 0)
                    {
                        FMainDS.PSubscription.AcceptChanges();
                    }
                }

                if (FMainDS.PSubscription.Rows.Count != 0)
                {
                    ReturnValue = true;
                }
                else
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

        /// <summary>
        /// 
        /// </summary>
        public void SpecialInitUserControl()
        {

            // Set up screen logic
            //MultiTableDS = (PartnerEditTDS)FMainDS;
            PartnerEditUIConnector = FPartnerEditUIConnector;
            LoadDataOnDemand();
            
            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpSubscriptions));

            // enable grid to react to insert and delete keyboard keys
            grdDetails.InsertKeyPressed += new TKeyPressedEventHandler(grdDetails_InsertKeyPressed);
            grdDetails.DeleteKeyPressed += new TKeyPressedEventHandler(grdDetails_DeleteKeyPressed);
            
            if (grdDetails.Rows.Count > 1)
            {
                grdDetails.SelectRowInGrid(1);
            }
            else
            {
                btnDelete.Enabled = false;
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

        /// <summary>
        /// 
        /// </summary>
        private void InitializeManualCode()
        {
            FMainDS.Tables.Add(new PSubscriptionTable());
            FMainDS.InitVars();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowDataManual()
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ARow"></param>
        private void ShowDetailsManual(PSubscriptionRow ARow)
        {
            if (ARow != null)
            {
                ucoDetails.AllowEditIssues = true;
                ucoDetails.ShowDetails (ARow);
            }
            
            btnDelete.Enabled = false;

            if (ARow != null)
            {
                btnDelete.Enabled = true;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ARow"></param>
        private void GetDetailDataFromControlsManual(PSubscriptionRow ARow)
        {
            ucoDetails.GetDetails (ARow);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_InsertKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            NewRow(this, null);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_DeleteKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            if (e.Row != -1)
            {
                this.DeleteRow(this, null);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            CreateNewPSubscription();
            
            // Fire OnRecalculateScreenParts event: reset counter in tab header
            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ARow"></param>
        private void NewRowManual(ref PSubscriptionRow ARow)
        {
            // Initialize subscription
            ARow.PartnerKey = ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey;
            ARow.PublicationCode = "";
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void EditIssues(System.Object sender, EventArgs e)
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void CancelAllSubscriptions(System.Object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        
        #endregion
        
    }
}