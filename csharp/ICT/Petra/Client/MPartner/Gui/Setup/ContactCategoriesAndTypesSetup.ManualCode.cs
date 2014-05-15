//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
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
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Validation;
using GNU.Gettext;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmContactCategoriesAndTypesSetup
    {
        private void InitializeManualCode()
        {
            LoadDataAndFinishScreenSetup();            
        }
        
        /// <summary>Loads the data for the screen and finishes the setting up of the screen.</summary>
        /// <returns>void</returns>
        private void LoadDataAndFinishScreenSetup()
        {
          Type DataTableType;
    
          // Load Data
          DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("ContactCategoryList", 
                PPartnerAttributeCategoryTable.GetSystemCategoryDBName() + " = false", null, out DataTableType);
          FMainDS.PPartnerAttributeCategory.Merge(CacheDT);
    
//          FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
//          grdDetails.CancelEditingWithEscapeKey = false;
//          grdDetails.Columns.Clear();
//          grdDetails.AddTextColumn("Acquisition Code", FMainDS.PAcquisition.ColumnAcquisitionCode);
//          grdDetails.AddTextColumn("Description", FMainDS.PAcquisition.ColumnAcquisitionDescription);
//          grdDetails.AddCheckBoxColumn("Assignable", FMainDS.PAcquisition.ColumnValidAcquisition);
//          grdDetails.AddCheckBoxColumn("Deletable", FMainDS.PAcquisition.ColumnDeletable);
//          grdDetails.AddCheckBoxColumn("Recruiting Mission", FMainDS.PAcquisition.ColumnRecruitingEffort);
//          grdDetails.Enter += new EventHandler(grdDetails_Enter);
//          grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(grdDetails_FocusRowLeaving);
//          grdDetails.Selection.SelectionChanged += new RangeRegionChangedEventHandler(grdDetails_RowSelected);
//          grdDetails.Selection.EnableMultiSelection = true;
    
          DataView myDataView = FMainDS.PPartnerAttributeCategory.DefaultView;
          myDataView.Sort = PPartnerAttributeCategoryTable.GetDeletableDBName() + " ASC, " + 
                PPartnerAttributeCategoryTable.GetIndexDBName() + " ASC";
          myDataView.AllowNew = false;
          grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
    
          FPetraUtilsObject.InitActionState();
//          chkToggleFilterCheckedChanged(null, null);
//          chkDetailValidAcquisitionCheckedChanged(null, null);
//          chkDetailRecruitingEffortCheckedChanged(null, null);
//          chkDetailDeletableCheckedChanged(null, null);
          UpdateRecordNumberDisplay();
        }
        
        private void NewRecord(System.Object sender, EventArgs e)
        {
            this.CreateNewPPartnerAttributeCategory();

            ucoValues.Enabled = true;
            txtDetailCategoryCode.Enabled = true;
        }

        private void NewRowManual(ref PPartnerAttributeCategoryRow ARow)
        {
            string newName = Catalog.GetString("NewCategory");
            Int32 countNewDetail = 0;

            if (FMainDS.PPartnerAttributeCategory.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PPartnerAttributeCategory.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.CategoryCode = newName;
            ARow.Deletable = true;
        }

        private TSubmitChangesResult StoreManualCode(ref PartnerContactSetupTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
//            return TRemote.MFinance.Setup.WebConnectors.SaveGLSetupTDS(FLedgerNumber, ref ASubmitChanges, out AVerificationResult);
            // TODO
            
            AVerificationResult = null;
            return TSubmitChangesResult.scrNothingToBeSaved;
        }

        private void DeleteRecord(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

//            DataView view = new DataView(FMainDS.AFreeformAnalysis);
//            view.RowStateFilter = DataViewRowState.CurrentRows;
//            view.RowFilter = String.Format("{0} = '{1}'",
//                AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
//                FPreviouslySelectedDetailRow.AnalysisTypeCode);

//            if (view.Count > 0)
//            {
//                MessageBox.Show(Catalog.GetString(
//                        "Please delete the unused values first!\n\nNote:Used Contact Categories and Contact Categories with used Contact Types cannot be deleted."));
//                return;
//            }
//
            DeletePPartnerAttributeCategory();
        }

        private void GetDetailDataFromControlsManual(PPartnerAttributeCategoryRow ARow)
        {
            ucoValues.GetDataFromControls();
        }

        private void GetDataFromControlsManual()
        {
            // TODO
        }

        private void ShowDetailsManual(PPartnerAttributeCategoryRow ARow)
        {
            if (ARow == null)
            {
                pnlDetails.Enabled = false;
                ucoValues.Enabled = false;
            }
            else
            {
                pnlDetails.Enabled = true;
                ucoValues.Enabled = true;
                ucoValues.AttributeCategory = ARow.CategoryCode;
                txtDetailCategoryCode.Enabled = ucoValues.Count == 0;
            }
        }

        /// <summary>
        /// freeze the category code after before adding a new value in the user control
        /// </summary>
        public String FreezeCategoryCode()
        {
            txtDetailCategoryCode.Enabled = false;
            return txtDetailCategoryCode.Text;
        }
        
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactCategoryDemote(object sender, EventArgs e)
        {
            // TODO            
        }        
        
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactCategoryPromote(object sender, EventArgs e)
        {
            // TODO
        }        
    }
}