//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.CommonForms.Logic;

namespace Ict.Petra.Client.MPartner.Logic
{
    /// <summary>
    /// Logic for a Shepherd for a Partner of Class FAMILY.
    /// </summary>
    public class TShepherdFamilyFormLogic : TPetraShepherdFormLogic
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="AYamlFile">Full path to the Shepherd's YAML Definition file.</param>
        /// <param name="APetraShepherdForm">An instance of TPetraShepherdConcreteForm.</param>
        public TShepherdFamilyFormLogic(string AYamlFile, IPetraShepherdConcreteFormInterface APetraShepherdForm) : base(AYamlFile,
                                                                                                                        APetraShepherdForm)
        {
            TLogging.Log(
                "Entering TShepherdFamilyFormLogic Constructor. AYamlFile = " + AYamlFile + "; APetraShepherdForm = " +
                APetraShepherdForm.ToString() +
                "...");

            TLogging.Log("TShepherdFamilyFormLogic Constructor ran.");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Skips the first page of the Shepherd ('Ledger Selection')
        /// </summary>
        public void SkipFirstShepherdPage()
        {
            TLogging.Log("SkipLedgerSelectionPage got called --> skipping 'Ledger Selection' Shepherd Page!");

            base.SwitchToPage("Name");
        }

        #endregion
    }
}