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

namespace Ict.Petra.Client.CommonForms
{
    ///<summary>Imlements TPetraShepherdForm (and therefore becomes a WinForm)
    /// and handles the GUI behaviour of a Shepherd. Utilises
    /// TPetraShepherdFormLogic for the base Shepherd Logic.</summary>
    public class TPetraShepherdConcreteForm : TPetraShepherdForm, IPetraShepherdConcreteFormInterface
    {
        ///<summary>Name of the YAML file that contains the definition of the Shepherd Pages and the Shepherd overall</summary>
        private string FYamlFile;
        ///<summary>Instance of base Shepherd Logic.</summary>
        private TPetraShepherdFormLogic FLogic;
        ///<summary>Instance of helper Class for navigation purposes</summary>
        private TShepherdNavigationHelper FShepherdNavigationHelper;

        ///<summary>Constructor</summary>
        public TPetraShepherdConcreteForm()
        {
            // In implementing class: FYamlFile = "...";
        }

        protected override void BtnFinishClick(object sender, EventArgs e)
        {
			FLogic.HandleActionFinish();
        }

        protected override void BtnNextClick(object sender, EventArgs e)
        {
			FLogic.HandleActionNext();
        }

        protected override void BtnBackClick(object sender, EventArgs e)
        {
 		    FLogic.HandleActionBack();
        }

        protected override void BtnCancelClick(object sender, EventArgs e)
        {
			FLogic.HandleActionCancel();
        }

        protected override void BtnHelpClick(object sender, EventArgs e)
        {
			FLogic.HandleActionHelp();
        }
        
        ///<summary>Gets called when the Form is Show'n but before it's painted</summary>
        protected override void Form_Load(object sender, EventArgs e)
        { 
            FShepherdNavigationHelper = new TShepherdNavigationHelper(FLogic.ShepherdPages, pnlNavigation);

            FLogic = new TPetraShepherdFormLogic(FYamlFile, this);

            ShowCurrentPage(); 
        }

        ///<summary>Update navigation buttons and navigation panel</summary>
        public void UpdateNavigation()
        {
        }

        ///<summary>Displays the 'current' Shepherd Page and updates the navigation buttons and Navigation Panel</summary>
        public void ShowCurrentPage()
        {
            // ...
            UpdateNavigation();
        }

        ///<summary>Closes the Shepherd without any further ado and without saving</summary>
        public void CancelShepherd()
        {
            // Close the whole thing without any further ado!!!
        }
    }
}