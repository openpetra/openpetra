//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2012 by OM International
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

using Ict.Common.Controls;

namespace Ict.Petra.Client.App.PetraClient
{
    /// <summary>
    /// Creates and manges a 'Breadcrumb Trail' that is shown in the Main Menu above the Tasks.
    /// </summary>
    public class TBreadcrumbTrail
    {
        Panel FBreadcrumbTrailPanel;
        Label FBreadcrumbTrailModuleLabel;
        Label FBreadcrumbTrailDetailLabel;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ATopPanel">The Panel to which the Breadcrumb Trail should be added to.</param>
        public TBreadcrumbTrail(Panel ATopPanel)
        {
            TVisualStyles VisualStyle = new TVisualStyles(TVisualStylesEnum.vsHorizontalCollapse);
                        
            FBreadcrumbTrailPanel = new Panel();
            FBreadcrumbTrailPanel.Name = "BreadcrumbTrail";
            FBreadcrumbTrailPanel.BackColor = Color.Transparent;
            FBreadcrumbTrailPanel.Dock = DockStyle.Fill;           
            
            ATopPanel.Controls.Add(FBreadcrumbTrailPanel);
            
            Panel BreadcrumbTrailModulePanel = new Panel();
            BreadcrumbTrailModulePanel.Name = "BreadcrumbTrailMainPanel";
            BreadcrumbTrailModulePanel.BackColor = Color.Transparent;
            BreadcrumbTrailModulePanel.Dock = DockStyle.Left;           
            BreadcrumbTrailModulePanel.AutoSize = true;
            BreadcrumbTrailModulePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            
            FBreadcrumbTrailPanel.Controls.Add(BreadcrumbTrailModulePanel);
            
            FBreadcrumbTrailModuleLabel = new Label();
            FBreadcrumbTrailModuleLabel.Name = "BreadcrumbTrailModuleLabel";
            FBreadcrumbTrailModuleLabel.Font = VisualStyle.TitleFont;
            FBreadcrumbTrailModuleLabel.ForeColor = VisualStyle.TitleFontColour;
            FBreadcrumbTrailModuleLabel.BackColor = Color.Transparent;
            FBreadcrumbTrailModuleLabel.AutoSize = true;
            FBreadcrumbTrailModuleLabel.Dock = DockStyle.Left;
            
            BreadcrumbTrailModulePanel.Controls.Add(FBreadcrumbTrailModuleLabel);
            
            FBreadcrumbTrailDetailLabel = new Label();
            FBreadcrumbTrailDetailLabel.Name = "BreadcrumbTrailDetailLabel";
            FBreadcrumbTrailDetailLabel.Font = new System.Drawing.Font(VisualStyle.TitleFont.FontFamily, VisualStyle.TitleFont.SizeInPoints - 2, 
                FontStyle.Bold);
            FBreadcrumbTrailDetailLabel.ForeColor = VisualStyle.TitleFontColour;
            FBreadcrumbTrailDetailLabel.BackColor = Color.Transparent;
            FBreadcrumbTrailDetailLabel.AutoEllipsis = true;
            FBreadcrumbTrailDetailLabel.Dock = DockStyle.Fill;
            FBreadcrumbTrailDetailLabel.Padding = new Padding(0, 2, 0, 0);
            
            FBreadcrumbTrailPanel.Controls.Add(FBreadcrumbTrailDetailLabel);
            
            FBreadcrumbTrailDetailLabel.BringToFront();
            
            FBreadcrumbTrailModuleLabel.Text = "Ledger 43";
            FBreadcrumbTrailDetailLabel.Text = "-> Gift";           
        }

        /// <summary>
        /// Module Text of the Breadcrumb Trail
        /// </summary>
        public string ModuleText
        {
            get
            {
                return FBreadcrumbTrailModuleLabel.Text;
            }
            
            set
            {
                FBreadcrumbTrailModuleLabel.Text = value;
            }
        }
                /// <summary>
        /// Detail Text of the Breadcrumb Trail
        /// </summary>
        public string DetailText
        {
            get
            {
                return FBreadcrumbTrailDetailLabel.Text;
            }
            
            set
            {
                FBreadcrumbTrailDetailLabel.Text = value;
            }
        }
        
    }
}