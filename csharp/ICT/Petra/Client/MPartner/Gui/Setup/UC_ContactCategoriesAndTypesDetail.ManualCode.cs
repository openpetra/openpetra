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
using Ict.Petra.Client.App.Core.RemoteObjects;
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
                
                //save the position of the actual row
                int rowIndex = grdDetails.GetFirstHighlightedRowIndex();
                
                FMainDS.PPartnerAttributeType.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                    PPartnerAttributeTypeTable.GetAttributeCategoryDBName(),
                    FAttributeCategory);
                SelectRowInGrid(rowIndex);
            }
        }
        
        private void InitializeManualCode()
        {
//            lblLinkFormatTip.Text = "Enter the URL that should be launched\r\nfor the Contact Type ( e.g.\r\nhttp://www.facebook.com/{VALUE} ).";
            lblLinkFormatTip.Font = new System.Drawing.Font(lblLinkFormatTip.Font.FontFamily, 7, FontStyle.Regular);
            lblLinkFormatTip.Top -= 5;
        }
        
        private void NewRecord(System.Object sender, EventArgs e)
        {
            AttributeCategory = ((TFrmContactCategoriesAndTypesSetup)ParentForm).FreezeCategoryCode();
            this.CreateNewPPartnerAttributeType();
        }

        private void NewRowManual(ref PPartnerAttributeTypeRow ARow)
        {
            string newName = Catalog.GetString("NEWVALUE");
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
            ARow.AttributeCategory = FAttributeCategory;
            ARow.Deletable = true;
        }

        private void GetDetailDataFromControlsManual(PPartnerAttributeTypeRow ARow)
        {
            // TODO
        }

        /// <summary>
        /// load the values into the grid
        /// </summary>
        public void LoadValues()
        {
            PPartnerAttributeTypeTable AT = new PPartnerAttributeTypeTable();
            Ict.Common.Data.TTypedDataTable TypedTable;
            
            TRemote.MCommon.DataReader.WebConnectors.GetData(PPartnerAttributeTypeTable.GetTableDBName(), 
                new TSearchCriteria[] { new TSearchCriteria(PPartnerAttributeTypeTable.GetAttributeCategoryDBName(),
                FAttributeCategory) }, out TypedTable);
            
            AT.Merge(TypedTable);
            PPartnerAttributeTypeTable myAT = FMainDS.PPartnerAttributeType;
            myAT.Merge(AT);
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