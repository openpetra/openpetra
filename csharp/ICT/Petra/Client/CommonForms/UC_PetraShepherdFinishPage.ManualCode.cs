//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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

namespace Ict.Petra.Client.CommonForms
{
    public partial class TUC_PetraShepherdFinishPage
    {
        #region Properties
        
        /// <summary>
        /// Summary Text #1.
        /// </summary>
        public string SummaryText1
        {
            get
            {
                return lblSummaryText1.Text;
            }

            set
            {
                lblSummaryText1.Text = value;
            }
        }

        /// <summary>
        /// Summary Text #2.
        /// </summary>
        public string SummaryText2
        {
            get
            {
                return lblSummaryText2.Text;
            }

            set
            {
                lblSummaryText2.Text = value;
            }
        }

        /// <summary>
        /// Determines whether the 'Further action on finish' CheckBox is shown (not shown by default).
        /// </summary>
        public bool FurtherActionOnFinishCheckBoxVisible
        {
            get
            {
                return chkFurtherActionOnFinish.Visible;
            }

            set
            {
                chkFurtherActionOnFinish.Visible = value;
            }
        }

        /// <summary>
        /// Text displayed for the 'Further action on finish' CheckBox.
        /// </summary>
        public string FurtherActionOnFinishText
        {
            get
            {
                return chkFurtherActionOnFinish.Text;
            }

            set
            {
                chkFurtherActionOnFinish.Text = value;
            }
        }

        /// <summary>
        /// Determines the Checked state of the 'Further action on finish' CheckBox (not Checked by default).
        /// </summary>
        public bool FurtherActionOnFinishChecked
        {
            get
            {
                return chkFurtherActionOnFinish.Checked;
            }

            set
            {
                chkFurtherActionOnFinish.Checked = value;
            }
        }

        #endregion
        
        #region Private Methods
        
        private void InitializeManualCode()
        {
            // TODO: REMOVE the following code once the Properties can be set from the Shepherds!
            SummaryText1 = "bla bla blah";
            SummaryText2 = "test only\r\ntest only too\r\ranother test.";
//            FurtherActionOnFinish = true;
            chkFurtherActionOnFinish.Visible = true;
        }
        
        #endregion
    }
}