//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MSysMan.Gui
{
    public partial class TFrmUserPreferences
    {
        private Boolean FViewMode = false;

        /// ViewMode is a special mode where the whole window with all tabs is in a readonly mode
        public bool ViewMode {
            get
            {
                return FViewMode;
            }
            set
            {
                FViewMode = value;
            }
        }
        
        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            ucoGeneral.SaveGeneralTab();
            
            if (ucoAppearance.SaveAppearanceTab())
            {
                Form MainWindow = FPetraUtilsObject.GetCallerForm();
                MethodInfo method = MainWindow.GetType().GetMethod("LoadNavigationUI");

                if (method != null)
                {
                    method.Invoke(MainWindow, new object[] { true });
                }

                method = MainWindow.GetType().GetMethod("SelectSettingsFolder");

                if (method != null)
                {
                    method.Invoke(MainWindow, null);
                }
            }
            
            Close();
        }

        private void InitializeManualCode()
        {
            this.AcceptButton = btnOK;
            
            //tabGiftBatch.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);
        }

        private void RunOnceOnActivationManual()
        {
            ucoGeneral.Focus();
            HookupAllInContainer(ucoGeneral);
            
            mnuMain.Enabled = false;
            mnuMain.Visible = false;
        }

        /*/// <summary>
        /// activate the transactions tab and load the gift transactions of the batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchStatus"></param>
        /// <param name="AFromTabClick">Indicates if called from a click on a tab or from grid doubleclick</param>
        public void LoadTransactions(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED,
            bool AFromTabClick = true)
        {*/
            
//            try
//            {
//                //this.tpgTransactions.Enabled = true;
//                FPetraUtilsObject.DisableDataChangedEvent();
//                this.ucoTransactions.LoadGifts(ALedgerNumber, ABatchNumber, ABatchStatus, AFromTabClick);
//            }
//            finally
//            {
//                FPetraUtilsObject.EnableDataChangedEvent();
//            }
        //}

        /*/// this window contains 2 tabs
        public enum eGiftTabs
        {
            /// list of batches
            Batches,

            /// list of transactions
            Transactions
        };

        void TabSelectionChanging(object sender, TabControlCancelEventArgs e)
        {
            FPetraUtilsObject.VerificationResultCollection.Clear();
        }

        bool FChangeTabEventHasRun = false;

        private void SelectTabManual(int ASelectedTabIndex)
        {
            if (ASelectedTabIndex == (int)eGiftTabs.Batches)
            {
                SelectTab(eGiftTabs.Batches);
            }
            else
            {
                SelectTab(eGiftTabs.Transactions);
            }
        }

        /// <summary>
        /// Switch to the given tab
        /// </summary>
        /// <param name="ATab"></param>
        /// <param name="AFromTabClick"></param>
        public void SelectTab(eGiftTabs ATab, bool AFromTabClick = true)
        {
            if (FChangeTabEventHasRun && AFromTabClick)
            {
                FChangeTabEventHasRun = false;
                return;
            }
            else
            {
                FChangeTabEventHasRun = !AFromTabClick;
            }

            if (ATab == eGiftTabs.Batches)
            {
                //If from grid double click then invoke tab changed event
                if (!AFromTabClick)
                {
                    this.tabGiftBatch.SelectedTab = this.tpgGeneral;
                }
            }
            else if (ATab == eGiftTabs.Transactions)
            {
                
            }
        }*/
    }
}