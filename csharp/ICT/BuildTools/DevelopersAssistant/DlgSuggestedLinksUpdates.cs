//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2015 by OM International
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ict.Tools.DevelopersAssistant
{
    /// <summary>
    /// Dialog that displays suggested changes to the External Links for a particular item
    /// </summary>
    public partial class DlgSuggestedLinksUpdates : Form
    {
        private ExternalLinksDictionary _externalLinks = null;  // keep a reference to the external links dictionary
        private int _currentIndex = 0;                          // our initial index into the suggestions list
        private int _numChanges = 0;                            // will become the number of changes made

        /// <summary>
        /// Gets a value indicating if changes were made
        /// </summary>
        public int NumberOfChanges
        {
            get
            {
                return _numChanges;
            }
        }

        /// <summary>
        /// Standard constructor
        /// </summary>
        public DlgSuggestedLinksUpdates()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Call this to initialise the dialog
        /// </summary>
        /// <param name="DicExternalLinks">A reference to the external links dictionary</param>
        public void Initialise(ExternalLinksDictionary DicExternalLinks)
        {
            // keep our local reference
            _externalLinks = DicExternalLinks;
        }

        private void DlgSuggestedLinksUpdates_Load(object sender, EventArgs e)
        {
            if (_externalLinks == null)
            {
                throw new Exception("You must call the Initialise method before showing the dialog.");
            }

            // Load the first suggestion
            DisplaySuggestion();
        }

        private void DisplaySuggestion()
        {
            string name, currentUrl, suggestedUrl;

            if (_externalLinks.GetSuggestedUpdate(_currentIndex, out name, out currentUrl, out suggestedUrl))
            {
                lblUpdateName.Text = name;
                lblCurrentLink.Text = currentUrl;
                lblSuggestedLink.Text = suggestedUrl;
                _currentIndex++;
            }
            else
            {
                // This should not happen because the main screen should not call us if there are no suggestions
                linkApplySuggestion.Enabled = false;
            }

            btnNextSuggestion.Enabled = _externalLinks.GetSuggestedUpdate(_currentIndex, out name, out currentUrl, out suggestedUrl);
        }

        private void btnNextSuggestion_Click(object sender, EventArgs e)
        {
            DisplaySuggestion();
        }

        private void linkApplySuggestion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _externalLinks.SetToDefault(lblUpdateName.Text);
            _currentIndex--;
            _numChanges++;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}