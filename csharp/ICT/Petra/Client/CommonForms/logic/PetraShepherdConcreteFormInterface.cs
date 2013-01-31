//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK, tomn, pauln
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
using System.Windows.Forms;

namespace Ict.Petra.Client.CommonForms.Logic
{
    /// <summary>
    /// Interface for TPetraShepherdConcreteForm.
    /// </summary>
    public interface IPetraShepherdConcreteFormInterface
    {
        /// <summary>Update navigation buttons and navigation panel.</summary>
        void UpdateNavigation();

        /// <summary>Displays the 'current' Shepherd Page and updates the navigation buttons and Navigation Panel.</summary>
        void ShowCurrentPage();

        /// <summary>Closes the Shepherd without any further ado and without saving.</summary>
        void CancelShepherd();

        /// <summary>Modifies the Form's layout according to the passed in Arguments.</summary>
        void UpdateShepherdFormProperties(string ATitle, int AShepherdWidth, int AShepherdHeight);
    }
}