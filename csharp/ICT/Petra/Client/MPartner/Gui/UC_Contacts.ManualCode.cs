//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       andreww
//
// Copyright 2004-2014 by OM International
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
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MCommon;
using Ict.Common.Data;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_Contacts
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
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
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
            if (!FMainDS.Tables.Contains(PContactLogTable.GetTableName()))
            {
                FMainDS.Merge(TRemote.MPartner.Partner.WebConnectors.FindContactLogsForPartner(FMainDS.PPartner[0].PartnerKey));
                FMainDS.Merge(new PPartnerContactTable());//TRemote.MPartner.Partner.WebConnectors.GetPartnerContacts(FMainDS.PPartner[0].PartnerKey));
                FMainDS.PContactLog.DefaultView.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.PContactLog.DefaultView);
            }

            FMainDS.InitVars();
            
        }

        private Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue = false;

            // Load Partner Types, if not already loaded
            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PPartnerContact == null)
                {
                    FMainDS.Tables.Add(new PPartnerContactTable());
                }

                if (FMainDS.PContactLog == null)
                {
                    FMainDS.Tables.Add(new PContactLogTable());
                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading)
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataContacts());

                    // Make DataRows unchanged
                    if (FMainDS.PContactLog.Rows.Count > 0)
                    {
                        FMainDS.PContactLog.AcceptChanges();
                    }
                }

                if (FMainDS.PContactLog.Rows.Count != 0)
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

        private void ShowDataManual()
        {
        }

        private void NewRowManual(ref PContactLogRow ARow)
        {
            ARow.ContactLogId = TRemote.MCommon.WebConnectors.GetNextSequence(TSequenceNames.seq_contact);
            
            PPartnerContactRow PartnerContact = FMainDS.PPartnerContact.NewRowTyped(true);
            PartnerContact.ContactLogId = ARow.ContactLogId;
            PartnerContact.PartnerKey = ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey;
            FMainDS.PPartnerContact.Rows.Add(PartnerContact);
        }

        private void ShowDetailsManual(PContactLogRow ARow)
        {
            if (ARow != null)
            {
                ucoContact.ShowDetails(ARow);
            }
        }

        private bool PreDeleteManual(PContactLogRow pContactLogRow, ref string ADeletionQuestion)
        {
            ADeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");
            ADeletionQuestion += String.Format("{0}{0}({1} {2})",
                Environment.NewLine, ucoContact.ContactDate, ucoContact.ContactCode);
            return true;
        }

        private void GetDetailDataFromControlsManual(PContactLogRow ARow)
        {
            ucoContact.GetDetails(ARow);   
        }

        private void PostDeleteManual(PContactLogRow pContactLogRow, bool AAllowDeletion, bool ADeletionPerformed, string ACompletionMessage)
        {
            if (ADeletionPerformed)
            {
                DoRecalculateScreenParts();
            }
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs()
            {
                ScreenPart = TScreenPartEnum.spCounters
            });
        }

        private void ValidateDataDetailsManual(PContactLogRow FPreviouslySelectedDetailRow)
        {
        }

        private void NewContactLog(object sender, EventArgs e)
        {
            if (CreateNewPContactLog())
            {
                ucoContact.Focus();
            }

            // reset counter in tab header
            RecalculateTabHeaderCounter();
        }

        private void DeleteContactLog(object sender, EventArgs e)
        {
            
            if(!TRemote.MPartner.Partner.WebConnectors.IsContactLogAssociatedWithMoreThanOnePartner(100)){ // TODOOOOOOOOOOOO
                // filter this before passing it so that only the selected row(s) get nuked
                TRemote.MPartner.Partner.WebConnectors.DeleteContacts(FMainDS.PContactLog); // TODOOOOOOOOOOOOOO 
                
                TDeleteGridRows.DeleteRows(this, grdDetails, FPetraUtilsObject, this);
            }

        }

        private void RecalculateTabHeaderCounter()
        {
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;
            /* Fire OnRecalculateScreenParts event to update the Tab Counters */
            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }
    
        #endregion
    }
}