/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Xml;
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// This control contains several sub panels;
    /// each panel has a heading;
    /// a click on the heading collapses or extends the panel
    /// several panels can be expanded at the same time
    /// </summary>
    public partial class TPnlAccordion : System.Windows.Forms.Panel
    {
        /// constructor
        public TPnlAccordion()
        {
            InitializeComponent();
        }

        /// <summary>
        /// this is the content panel which will host the task lists
        /// </summary>
        private TDashboard FDashboard;

        /// create an accordion with several items for the given folder, with all sub menus
        public TPnlAccordion(XmlNode AFolderNode, TDashboard ADashboard)
        {
            this.FDashboard = ADashboard;
            this.Name = "pnl" + AFolderNode.Name;
            this.Dock = DockStyle.Top;

            XmlNode ModuleNode = AFolderNode.LastChild;

            while (ModuleNode != null)
            {
                Panel pnlModule = new Panel();
                pnlModule.Dock = DockStyle.Top;

                Panel pnlModuleCaption = new Panel();
                pnlModuleCaption.Size = new System.Drawing.Size(this.Width, 27);
                pnlModuleCaption.Dock = DockStyle.Top;
                pnlModuleCaption.Click += new System.EventHandler(this.CollapseModuleMenu);
                pnlModule.Controls.Add(pnlModuleCaption);

                Label lblModule = new Label();
                lblModule.Font = new System.Drawing.Font("Microsoft Sans Serif",
                    12F,
                    System.Drawing.FontStyle.Bold,
                    System.Drawing.GraphicsUnit.Point,
                    ((byte)(0)));
                lblModule.ForeColor = System.Drawing.Color.Blue;
                lblModule.Location = new System.Drawing.Point(8, 2);
                lblModule.Name = "lbl" + ModuleNode.Name;
                lblModule.Size = new System.Drawing.Size(153, 23);
                lblModule.Text = TLstFolderNavigation.GetLabel(ModuleNode);
                lblModule.Click += new System.EventHandler(this.CollapseModuleMenu);

                Button btnCollapse = new Button();
                btnCollapse.Text = "^";
                btnCollapse.Tag = pnlModule;
                btnCollapse.Click += new System.EventHandler(this.CollapseModuleMenu);
                btnCollapse.Size = new System.Drawing.Size(20, 20);
                btnCollapse.Location = new System.Drawing.Point(163, 4);

                XmlNode SubmoduleNode = ModuleNode.FirstChild;

                Int32 CounterSubmodules = 0;

                while (SubmoduleNode != null)
                {
                    if (SubmoduleNode.Name == "SearchBoxes")
                    {
                        // todo Search Boxes for Submodule
                    }
                    else
                    {
                        LinkLabel lblSubmodule = new LinkLabel();
                        lblSubmodule.Name = SubmoduleNode.Name;
                        lblSubmodule.Text = TLstFolderNavigation.GetLabel(SubmoduleNode);
                        lblSubmodule.Location = new System.Drawing.Point(8, 25 + CounterSubmodules * 20);
                        lblSubmodule.Size = new System.Drawing.Size(153, 20);
                        lblSubmodule.Tag = SubmoduleNode;
                        lblSubmodule.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
                        pnlModule.Controls.Add(lblSubmodule);

                        CounterSubmodules++;
                    }

                    SubmoduleNode = SubmoduleNode.NextSibling;
                }

                pnlModule.Size = new System.Drawing.Size(this.Width, 5 + CounterSubmodules * 20 + 25);
                pnlModuleCaption.Controls.Add(lblModule);
                pnlModuleCaption.Controls.Add(btnCollapse);
                this.Controls.Add(pnlModule);
                this.Size = new System.Drawing.Size(this.Width, this.Height + pnlModule.Height);

                ModuleNode = ModuleNode.PreviousSibling;
            }
        }

        private void CollapseModuleMenu(object sender, EventArgs e)
        {
            Button btnModuleCollapse;
            Panel pnlModule;

            if (sender.GetType() == typeof(Panel))
            {
                // sender is the module caption panel
                pnlModule = (Panel)((Control)sender).Parent;
            }
            else
            {
                // either the button or label or icon
                pnlModule = (Panel)((Control)sender).Parent.Parent;
            }

            btnModuleCollapse = (Button)pnlModule.Controls[0].Controls[1];

            if (pnlModule.Height == pnlModule.Controls[0].Height)
            {
                // show the menu in full size again
                pnlModule.Height = (Int32)pnlModule.Tag;
                pnlModule.Parent.Height += (Int32)pnlModule.Tag - pnlModule.Controls[0].Height;
                btnModuleCollapse.Text = "^";
            }
            else
            {
                pnlModule.Tag = pnlModule.Height;
                pnlModule.Parent.Height -= (Int32)pnlModule.Tag - pnlModule.Controls[0].Height;
                pnlModule.Height = pnlModule.Controls[0].Height;
                btnModuleCollapse.Text = "v";
            }
        }

        private void LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Object tag = ((Control)sender).Tag;
            TLstTasks lstTasks = null;

            if (tag.GetType() == typeof(TLstTasks))
            {
                lstTasks = (TLstTasks)tag;
            }
            else
            {
                lstTasks = new TLstTasks((XmlNode)tag);
                lstTasks.Statusbar = FStatusbar;
                ((Control)sender).Tag = lstTasks;
            }

            FDashboard.ReplaceTaskList(lstTasks);
        }

        private TExtStatusBarHelp FStatusbar = null;

        /// <summary>
        /// set the statusbar so that error messages can be displayed
        /// </summary>
        public TExtStatusBarHelp Statusbar
        {
            set
            {
                FStatusbar = value;
            }
        }

        /// <summary>
        /// make sure that the content panel is populated with the contents of the first link;
        /// this might be called when selecting a folder
        /// </summary>
        public void SelectFirstLink()
        {
            bool validContent = false;

            if (this.Controls.Count > 0)
            {
                Panel pnlModule = (Panel) this.Controls[this.Controls.Count - 1];

                if (pnlModule.Controls.Count > 1)
                {
                    // first child is module caption
                    LinkLabel lbl = (LinkLabel)pnlModule.Controls[1];
                    LinkClicked(lbl, null);
                    validContent = true;
                }
            }

            if (!validContent)
            {
                FDashboard.ReplaceTaskList(null);
            }
        }
    }
}