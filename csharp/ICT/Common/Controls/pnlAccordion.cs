//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion
        }

        /// <summary>
        /// This is the content panel which will host the task lists
        /// </summary>
        private TDashboard FDashboard;

        /// <summary>
        /// This is the currently displayed Task List
        /// </summary>
        private TLstTasks FCurrentTaskList = null;

        private int FMaxTaskWidth;
        private TExtStatusBarHelp FStatusbar = null;

#if disabled
        static System.Drawing.Bitmap UpArrow = null;
        static System.Drawing.Bitmap DownArrow = null;
#endif

        /// create an accordion with several items for the given folder, with all sub menus
        public TPnlAccordion(XmlNode AFolderNode, TDashboard ADashboard, string APanelName)
        {
            this.FDashboard = ADashboard;
            this.Name = APanelName;
            this.Dock = DockStyle.Top;
            this.Height = 0;

#if disabled
            if (UpArrow == null)
            {
                UpArrow = new System.Drawing.Bitmap(TLstFolderNavigation.ResourceDirectory + System.IO.Path.DirectorySeparatorChar + "2uparrow.png");
            }

            if (DownArrow == null)
            {
                DownArrow = new System.Drawing.Bitmap(
                    TLstFolderNavigation.ResourceDirectory + System.IO.Path.DirectorySeparatorChar + "2downarrow.png");
            }
#endif

            XmlNode ModuleNode = AFolderNode.LastChild;

            while (ModuleNode != null)
            {
                Panel pnlModule = new Panel();
                pnlModule.Dock = DockStyle.Top;

                TPnlGradient pnlModuleCaption = new TPnlGradient();
                pnlModuleCaption.GradientColorTop = System.Drawing.Color.FromArgb(0xE7, 0xEF, 0xFF);
                pnlModuleCaption.GradientColorBottom = System.Drawing.Color.FromArgb(0xD6, 0xE3, 0xFF);
                pnlModuleCaption.Size = new System.Drawing.Size(this.Width, 27);
                pnlModuleCaption.Dock = DockStyle.Top;
                pnlModuleCaption.Click += new System.EventHandler(this.CollapseModuleMenu);
                pnlModule.Controls.Add(pnlModuleCaption);

                Label lblModule = new Label();
                lblModule.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
                lblModule.ForeColor = System.Drawing.Color.FromArgb(0x10, 0x41, 0x8c);
                lblModule.BackColor = System.Drawing.Color.Transparent;
                lblModule.Location = new System.Drawing.Point(11, 6);
                lblModule.Name = "lbl" + ModuleNode.Name;
                lblModule.Size = new System.Drawing.Size(153, 23);
                lblModule.Text = TLstFolderNavigation.GetLabel(ModuleNode);
                lblModule.Click += new System.EventHandler(this.CollapseModuleMenu);

#if disabled
                Button btnCollapse = new Button();
                btnCollapse.Tag = pnlModule;
                btnCollapse.Location = new System.Drawing.Point(163, 1);
                btnCollapse.Size = new System.Drawing.Size(27, 27);
                btnCollapse.Image = UpArrow;
                btnCollapse.UseVisualStyleBackColor = true;
                btnCollapse.Text = "";
                btnCollapse.Click += new System.EventHandler(this.CollapseModuleMenu);
#endif

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
                        lblSubmodule.Font = new System.Drawing.Font("Tahoma",
                            8.25F,
                            System.Drawing.FontStyle.Bold,
                            System.Drawing.GraphicsUnit.Point,
                            0);
                        lblSubmodule.Text = TLstFolderNavigation.GetLabel(SubmoduleNode);
                        lblSubmodule.Location = new System.Drawing.Point(30, pnlModuleCaption.Height + 5 + CounterSubmodules * 20);
                        lblSubmodule.Size = new System.Drawing.Size(153, 20);
                        lblSubmodule.LinkColor = System.Drawing.Color.FromArgb(0x10, 0x41, 0x8c);
                        lblSubmodule.LinkBehavior = LinkBehavior.HoverUnderline;
                        lblSubmodule.Tag = SubmoduleNode;

                        lblSubmodule.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
                        pnlModule.Controls.Add(lblSubmodule);

                        CounterSubmodules++;
                    }

                    SubmoduleNode = SubmoduleNode.NextSibling;
                }

                pnlModule.Size = new System.Drawing.Size(this.Width, pnlModuleCaption.Height + CounterSubmodules * 20);
                pnlModule.BackColor = System.Drawing.Color.FromArgb(0xCE, 0xDB, 0xFF);
                pnlModuleCaption.Controls.Add(lblModule);
#if disabled
                pnlModuleCaption.Controls.Add(btnCollapse);
#endif
                this.Controls.Add(pnlModule);
                this.Size = new System.Drawing.Size(this.Width, this.Height + pnlModule.Height);

                ModuleNode = ModuleNode.PreviousSibling;
            }
        }

        #region Properties

        /// <summary>
        /// Maximum Task Width
        /// </summary>
        public int MaxTaskWidth
        {
            get
            {
                return FMaxTaskWidth;
            }

            set
            {
                if (FMaxTaskWidth != value)
                {
                    FMaxTaskWidth = value;

                    FCurrentTaskList.MaxTaskWidth = FMaxTaskWidth;
                }
            }
        }

        #endregion

        #region Public Methods

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
                FDashboard.ShowTaskList(null);
            }
        }

        #endregion

        #region Private Methods

        private void CollapseModuleMenu(object sender, EventArgs e)
        {
#if disabled
            Button btnModuleCollapse;
            Panel pnlModule;

            if (sender.GetType() == typeof(TPnlGradient))
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

            if (btnModuleCollapse.Image == DownArrow)
            {
                // show the menu in full size again
                pnlModule.Height = (Int32)pnlModule.Tag;
                pnlModule.Parent.Height += (Int32)pnlModule.Tag - pnlModule.Controls[0].Height;
                btnModuleCollapse.Image = UpArrow;
            }
            else
            {
                pnlModule.Tag = pnlModule.Height;
                pnlModule.Parent.Height -= (Int32)pnlModule.Tag - pnlModule.Controls[0].Height;
                pnlModule.Height = pnlModule.Controls[0].Height;
                btnModuleCollapse.Image = DownArrow;
            }
#endif
        }

        private void LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Object tag = ((Control)sender).Tag;

            if (tag.GetType() == typeof(TLstTasks))
            {
                FCurrentTaskList = (TLstTasks)tag;
//TLogging.Log("LinkClicked for existing " + FCurrentTaskList.Name);
            }
            else
            {
                FCurrentTaskList = new TLstTasks((XmlNode)tag);
//TLogging.Log("LinkClicked for NEW " + FCurrentTaskList.Name);
                ((Control)sender).Tag = FCurrentTaskList;
            }

            FCurrentTaskList.Statusbar = FStatusbar;
            FCurrentTaskList.Dock = DockStyle.Fill;

            FDashboard.ShowTaskList(FCurrentTaskList);
//            Invalidate();
        }

        #endregion
    }
}