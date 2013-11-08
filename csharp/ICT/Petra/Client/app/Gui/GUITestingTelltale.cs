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
//
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// pProvides a small 'telltale' window that shows details about the Test that a 
    /// particular instance of PetraClient is performing. It gets launched from the TTestWinForm Class.
    /// </summary>
    /// <remarks>These tests are to be driven by the PetraMultiStart program.</remarks>
    public partial class TGuiTestingTelltaleWinForm : Form
    {
        public TGuiTestingTelltaleWinForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //            
        }
        
        public string Client
        {
            set
            {
                lblClient.Text = value;
            }
        }
        
        public string ClientGroup
        {
            set
            {
                lblClientGroup.Text = value;
            }
        }
                
        public string TestingFile
        {
            set
            {
                lblTestingFile.Text = value;
            }
        }
        
        public string Repeats
        {
            set
            {
                lblRepeats.Text = value;
            }
        }
        
        public DateTime DisconnectTime
        {
            set
            {
                lblDisconnectTime.Text = value.ToLongTimeString();
            }
        }
    }
}