//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       pauln
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
using System;

namespace Ict.Testing.Shepherds
{
    /// <summary>
    /// Logic Interface for Shepherd Unit Tests.
    /// </summary>
    /// TODO: implement this class in the dve
    public class TestLogicInterface : Ict.Petra.Client.CommonForms.Logic.IPetraShepherdConcreteFormInterface
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TestLogicInterface()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update navigation buttons and navigation panel.
        /// </summary>
        public void UpdateNavigation()
        {
            System.Console.WriteLine("Updating the navigation buttons in the GUI.. ");
        }

        /// <summary>
        /// Displays the 'current' Shepherd Page and updates the navigation buttons and Navigation Panel.
        /// </summary>
        public void ShowCurrentPage()
        {
            System.Console.WriteLine("Showing the current page in the GUI.. ");
        }

        /// <summary>
        /// Closes the Shepherd without any further ado and without saving.
        /// </summary>
        public void CancelShepherd()
        {
            System.Console.WriteLine("Canceling the shepherd's GUI and logic.. ");
        }

        /// <summary>
        /// Modifies the Form's layout according to the passed in Arguments.
        /// </summary>
        /// <param name="AString">Shepherd Title.</param>
        /// <param name="width">Width of the Shepherd Form.</param>
        /// <param name="height">Height of the Shepherd Form.</param>
        public void UpdateShepherdFormProperties(string AString, int width, int height)
        {
            System.Console.WriteLine("The ShepherdFormProperties would have been updated if this wasn't a dummy interface.");
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        /// <param name="ProgressPercent">TODO</param>
        public void UpdateProgressBar(int ProgressPercent)
        {
            System.Console.WriteLine("The Progress Bar would have been updated if this wasn't a dummy interface.");
        }

        #endregion
    }
}