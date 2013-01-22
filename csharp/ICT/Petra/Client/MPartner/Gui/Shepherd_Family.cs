//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK
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
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonForms.Logic;
using Ict.Petra.Client.MPartner.Logic;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// Shepherd for a Partner of Class FAMILY.
    /// </summary>
    public partial class TShepherdFamilyForm : TPetraShepherdConcreteForm
    {
        ///<summary>Instance of this Shepherd's Logic.</summary>
        private TShepherdFamilyFormLogic FSpecificLogic;

        private bool FSkipLedgerSelectionPage = false;

        /// <summary>
        /// TODO Comment
        /// </summary>
        public bool SkipLedgerSelectionPage
        {
            get
            {
                return FSkipLedgerSelectionPage;
            }
            set
            {
                FSkipLedgerSelectionPage = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TShepherdFamilyForm(Form AParentForm) : base(AParentForm)
        {
            TLogging.Log("Entering TShepherdFamilyForm Constructor...");

            FYamlFile = Path.GetDirectoryName(TAppSettingsManager.GetValue("UINavigation.File")) +
                        Path.DirectorySeparatorChar + "Shepherd_Family_Definition.yaml";

            FLogic = new TShepherdFamilyFormLogic(FYamlFile, this);
            FSpecificLogic = (TShepherdFamilyFormLogic)FLogic;

            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();


            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //

            TLogging.Log("TShepherdFamilyForm Constructor ran.");
        }

        /// <summary>
        /// Load Event for the TShepherdFamilyForm.
        /// </summary>
        /// <param name="sender">Not evaluated.</param>
        /// <param name="e">Not evaluated.</param>
        protected override void Form_Load(object sender, EventArgs e)
        {
            TLogging.Log("Entering TShepherdFamilyForm Form_Load...");

            base.Form_Load(sender, e);

            this.Text = "Add New Family Shepherd";   // this should come out of the YAML file and should have been set in the TPetraShepherdConcreteForm.Form_Load Method!

            if (FSkipLedgerSelectionPage)
            {
                FSpecificLogic.SkipFirstShepherdPage();
            }

            TLogging.Log("TShepherdFamilyForm Form_Load ran.");
        }
    }
}