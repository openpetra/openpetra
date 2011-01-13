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
using System.Collections;

namespace Ict.Petra.Client.CommonForms.Logic
{
    ///<summary>Logic class for the internal behaviour of a Shepherd</summary>
    public class TPetraShepherdFormLogic : object
    {
        /// <summary>Holds a typed list of 0..n TPetraShepherdPage's</summary>
        private TPetraShepherdPagesList FShepherdPages;
        /// <summary>Holds the instance of the Shepherd Form management class</summary>
        private IPetraShepherdConcreteFormInterface FForm;
        /// <summary>Holds the instance of the current shepherd page</summary>
        private TPetraShepherdPage FCurrentPage;
        /// <summary>'Blackboard' for exchanging data between shepherd pages which isn't stored in the DB</summary>
        private SortedList FPagesDataHeap;

        ///<summary>List of Shepherd Pages</summary>
        public TPetraShepherdPagesList ShepherdPages {
            get
            {
                return FShepherdPages;
            }
            set
            {
            }
        }

        ///<summary>Currently displayed Shepherd Page</summary>
        public TPetraShepherdPage CurrentPage {
            get
            {
                return FCurrentPage;
            }
            set
            {
            }
        }

        
        ///<summary>Constructor</summary>
        public TPetraShepherdFormLogic(string AYamlFile, IPetraShepherdConcreteFormInterface APetraShepherdForm)
        {
            FForm = APetraShepherdForm;

            // Take AYamlFile and parse it into an XmlNode structure


            FShepherdPages = new TPetraShepherdPagesList(AYamlFile);

            // Iterate over all FPetraShepherdPages and add the VisibleOrEnabledChangedEventHandler

            // FShepherdPages needs to get added an auto-generated TPetraShepherdFinishPage
            // for the Finish Page (that is not specified in the YAML file!)
            // Note: That Finish Page (and only this) will have IsLastPage = true!!!
        }

        ///<summary>Returns an instance of a Page UserControl</summary>
        protected UserControl InstantiatePageUC(string AID)
        {
            return null;
        }

        ///<summary>Switches the current page</summary>
        private void SwitchToPage(string APage)
        {
            // ....

            FForm.ShowCurrentPage();
        }

        ///<summary>Switches the Finish page</summary>
        private void SwitchToFinishPage()
        {
        }

        ///<summary>Switches to the 'next' page (whatever page this is)</summary>
        public virtual void HandleActionNext()
        {
            // ....

            SwitchToPage(String.Empty);
        }

        ///<summary>Switches to the 'previous' page (whatever page this is)</summary>
        public virtual void HandleActionBack()
        {
            // ....

            SwitchToPage(String.Empty);
        }

        ///<summary>Causes to close the Shepherd without saving if the user chooses to do that</summary>
        public void HandleActionCancel()
        {
            // Show Message Box with Yes, No, Cancel options

            // If YES ->
            FForm.CancelShepherd();
        }

        ///<summary>Shows the context-sensitive help file content for the current page, if available, otherwise for the Shepherd as a whole</summary>
        public void HandleActionHelp()
        {
        }

        ///<summary>Switches to the auto-generated 'Finish' page.</summary>
        public void HandleActionFinish()
        {
            // .....

            SwitchToFinishPage();
        }

        ///<summary>Determines the 'First' page (whatever page this is).</summary>
        private void VisibleOrEnabledChangedEventHandler()
        {
            // re-enumerate FShepherdPages!

            FForm.UpdateNavigation();
        }


        /// <summary>Looks up a key in the PagesDataHeap and returns its value, or null if it doesn't exist</summary>
        public string PagesDataHeapPeek(string AKey)
        {
            return String.Empty;
        }

        /// <summary>Sets a key-value pair in the PagesDataHeap</summary>
        public void PagesDataHeapPoke(string AKey, string AValue)
        {
        }
    }
}