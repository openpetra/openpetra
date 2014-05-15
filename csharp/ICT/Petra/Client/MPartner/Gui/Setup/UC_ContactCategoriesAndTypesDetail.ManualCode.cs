//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, timop
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
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner.Mailroom.Validation;
using Ict.Petra.Shared.MPartner.Partner.Data;
using GNU.Gettext;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TUC_ContactCategoriesAndTypesDetail
    {
        private String FAttributeCategory;
        
        /// <summary>
        /// these values are for this AttributeCategory
        /// </summary>
        public String AttributeCategory
        {
            set
            {
                FAttributeCategory = value;
                                
                string rowFilter = String.Format("{0} = '{1}'",
                    PPartnerAttributeTypeTable.GetAttributeCategoryDBName(),
                    FAttributeCategory);
                FFilterPanelControls.SetBaseFilter(rowFilter, true);
                ApplyFilter();

                grdDetails.SelectRowWithoutFocus(FPrevRowChangedRow);                
            }
        }
        
        private void InitializeManualCode()
        {
//            lblLinkFormatTip.Text = "Enter the URL that should be launched\r\nfor the Contact Type ( e.g.\r\nhttp://www.facebook.com/{VALUE} ).";
            lblLinkFormatTip.Font = new System.Drawing.Font(lblLinkFormatTip.Font.FontFamily, 7, FontStyle.Regular);
            lblLinkFormatTip.Top -= 5;
        
            pnlDetails.MinimumSize = new Size(700, 145);              // To prevent shrinkage!
    
            LoadValues();
        }
        
        private void NewRecord(System.Object sender, EventArgs e)
        {
            AttributeCategory = ((TFrmContactCategoriesAndTypesSetup)ParentForm).FreezeCategoryCode();
            this.CreateNewPPartnerAttributeType();
        }

        private void NewRowManual(ref PPartnerAttributeTypeRow ARow)
        {
            string newName = Catalog.GetString("NewType");
            Int32 countNewDetail = 0;

            if (FMainDS.PPartnerAttributeType.Rows.Find(new object[] { FAttributeCategory, newName }) != null)
            {
                while (FMainDS.PPartnerAttributeType.Rows.Find(new object[] { FAttributeCategory, newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.Code = newName;
            ARow.SpecialLabel = "PLEASE ENTER LABEL"; 
            ARow.AttributeCategory = FAttributeCategory;
            ARow.AttributeTypeValueKind = "CONTACTDETAIL_GENERAL";
            ARow.Deletable = true;
            
            cmbDetailAttributeTypeValueKind.SelectedIndex = 1;
        }

        private void ShowDetailsManual(PPartnerAttributeTypeRow ARow)
        {
            if (ARow == null)
            {
				cmbDetailAttributeTypeValueKind.SelectedIndex = 0;
                txtDetailHyperlinkFormat.Enabled = false;

                return;
            }
            else
            {
    			switch (ARow.AttributeTypeValueKind) {
    				case "CONTACTDETAIL_GENERAL":
    					cmbDetailAttributeTypeValueKind.SelectedIndex = 0;
                        txtDetailHyperlinkFormat.Enabled = false;
    					break;
    				case "CONTACTDETAIL_HYPERLINK":
    					cmbDetailAttributeTypeValueKind.SelectedIndex = 2;
                        txtDetailHyperlinkFormat.Enabled = false;
    					break;
    				case "CONTACTDETAIL_HYPERLINK_WITHVALUE":
    					cmbDetailAttributeTypeValueKind.SelectedIndex = 3;
                        txtDetailHyperlinkFormat.Enabled = true;
    					break;
    				case "CONTACTDETAIL_EMAILADDRESS":
    					cmbDetailAttributeTypeValueKind.SelectedIndex = 1;
                        txtDetailHyperlinkFormat.Enabled = false;
    					break;
    				case "CONTACTDETAIL_SKYPEID":
    					cmbDetailAttributeTypeValueKind.SelectedIndex = 4;
                        txtDetailHyperlinkFormat.Enabled = false;
    					break;
                    default:
    					cmbDetailAttributeTypeValueKind.SelectedIndex = 0;
                        txtDetailHyperlinkFormat.Enabled = false;
    					break;                    
    			}       
            }
        }
        
        private void GetDetailDataFromControlsManual(PPartnerAttributeTypeRow ARow)
        {
            switch (cmbDetailAttributeTypeValueKind.SelectedIndex) 
            {
                case 0:
                    ARow.AttributeTypeValueKind = "CONTACTDETAIL_GENERAL";
                    break;
                case 1:
                    ARow.AttributeTypeValueKind = "CONTACTDETAIL_EMAILADDRESS";
                    break;
                case 2:
                    ARow.AttributeTypeValueKind = "CONTACTDETAIL_HYPERLINK";
                    break;
                case 3:
                    ARow.AttributeTypeValueKind = "CONTACTDETAIL_HYPERLINK_WITHVALUE";
                    break;
                case 4:
                    ARow.AttributeTypeValueKind = "CONTACTDETAIL_SKYPEID";
                    break;                    
                default:
                    ARow.AttributeTypeValueKind = "CONTACTDETAIL_GENERAL";
                    break;                    
            }            
        }

        /// <summary>
        /// load the values into the grid
        /// </summary>
        public void LoadValues()
        {
            Type DataTableType;
    
            // Load Data
            DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("ContactTypeList", "", null, 
                out DataTableType);
            FMainDS.PPartnerAttributeType.Merge(CacheDT);
        }

        /// <summary>
        /// The number of values in the grid for the current Type
        /// </summary>
        public int Count
        {
            get
            {
                return grdDetails.Rows.Count - 1;
            }
        }
        
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactTypeDemote(object sender, EventArgs e)
        {
            // TODO            
        }        
 
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void ContactTypePromote(object sender, EventArgs e)
        {
            // TODO
        }        
        
        private void EnableDisableUnassignableDate(Object sender, EventArgs e)
        {
            dtpDetailUnassignableDate.Enabled = chkDetailUnassignable.Checked;

            if (!chkDetailUnassignable.Checked)
            {
                dtpDetailUnassignableDate.Date = null;
            }
            else
            {
                dtpDetailUnassignableDate.Date = DateTime.Now.Date;
            }
        }        
    }
}