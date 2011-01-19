//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK, tomn, pauln
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
using System;
using System.Windows.Forms;
using Ict.Petra.Client.CommonForms.Logic;

using Ict.Common;

namespace Ict.Petra.Client.CommonForms
{
    ///<summary>Imlements TPetraShepherdForm (and therefore becomes a WinForm)
    /// and handles the GUI behaviour of a Shepherd. Utilises
    /// TPetraShepherdFormLogic for the base Shepherd Logic.</summary>
    public class TPetraShepherdConcreteForm : TPetraShepherdForm, IPetraShepherdConcreteFormInterface
    {
        ///<summary>Name of the YAML file that contains the definition of the Shepherd Pages and the Shepherd overall</summary>
        protected string FYamlFile = String.Empty;
        
        ///<summary>Instance of base Shepherd Logic.</summary>
        protected TPetraShepherdFormLogic FLogic;
        ///<summary>Instance of helper Class for navigation purposes</summary>
        private TShepherdNavigationHelper FShepherdNavigationHelper;

        
        ///<summary>Constructor</summary>
        public TPetraShepherdConcreteForm()
        {
            TLogging.Log("Entering TPetraShepherdConcreteForm Constructor...");
            
            // In implementing class: FYamlFile = "...";
            
            TLogging.Log("TPetraShepherdConcreteForm Constructor ran.");
        }
		
        /// <summary>
        /// Overwrites virtual function in PetraShepherdForm.cs,
        /// calls function in PetraShepherdConcreteForm.cs in logic namespace to handle action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void BtnFinishClick(object sender, EventArgs e)
        {
			FLogic.HandleActionFinish();
        }

        /// <summary>
        /// Overwrites virtual function in PetraShepherdForm.cs,
        /// calls function in PetraShepherdConcreteForm.cs in logic namespace to handle action 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void BtnNextClick(object sender, EventArgs e)
        {
			FLogic.HandleActionNext();
        }

        /// <summary>
        /// Overwrites virtual function in PetraShepherdForm.cs,
        /// calls function in PetraShepherdConcreteForm.cs in logic namespace to handle action 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void BtnBackClick(object sender, EventArgs e)
        {
 		    FLogic.HandleActionBack();
        }

        /// <summary>
        /// Overwrites virtual function in PetraShepherdForm.cs,
        /// calls function in PetraShepherdConcreteForm.cs in logic namespace to handle action 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void BtnCancelClick(object sender, EventArgs e)
        {
			FLogic.HandleActionCancel();
        }

        /// <summary>
        /// Overwrites virtual function in PetraShepherdForm.cs,
        /// calls function in PetraShepherdConcreteForm.cs in logic namespace to handle action 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void BtnHelpClick(object sender, EventArgs e)
        {
			FLogic.HandleActionHelp();
        }
        
        /// <summary>
        /// Gets called when the Form is shown but before it's painted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Load(object sender, EventArgs e)
        { 
            TLogging.Log("Entering TPetraShepherdConcreteForm (Base) Form_Load...");
            
            FShepherdNavigationHelper = new TShepherdNavigationHelper(FLogic.ShepherdPages, pnlNavigation);

            ShowCurrentPage(); 

            TLogging.Log("TPetraShepherdConcreteForm (Base) Form_Load ran.");                      
        }

        ///<summary>Update navigation buttons and navigation panel</summary>
        public void UpdateNavigation()
        {
            TLogging.Log("UpdateNavigation");
        }

        ///<summary>Displays the 'current' Shepherd Page and updates the navigation buttons and Navigation Panel</summary>
        public void ShowCurrentPage()
        {
            TLogging.Log("ShowCurrentPage");
            
//			  TODO: THIS IS THE NEXT STEP IN DEVELOPMENT 
//            pnlContent.Controls.Clear();
//            pnlContent.Controls.Add(FLogic.CurrentPage.UserControlInstance);

            UpdateNavigation();
        }

        ///<summary>Closes the Shepherd without any further ado and without saving</summary>
        public void CancelShepherd()
        {
            TLogging.Log("CancelShepherd");
        }
    }
}