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
using Ict.Common.IO;
using Mono.Unix;

namespace Ict.Common.Controls
{
    /// <summary>
    /// This control has a list of folders;
    /// each folder is represented as a radiobutton (TRbtNavigationButton)
    /// </summary>
    public partial class TLstFolderNavigation : System.Windows.Forms.Panel
    {
        /// <summary>
        /// this is the path to the resource directory of the icons.
        /// this is public and static so that the TPnlAccordion can access it too.
        /// </summary>
        public static string ResourceDirectory = "";

        /// <summary>
        /// this will get the proper label for any navigation node;
        /// this is public and static so that the TPnlAccordion can access it too.
        /// </summary>
        /// <param name="ANode"></param>
        /// <returns></returns>
        public static string GetLabel(XmlNode ANode)
        {
            return Catalog.GetString(TYml2Xml.HasAttribute(ANode, "Label") ? TYml2Xml.GetAttribute(ANode,
                    "Label") : StringHelper.ReverseUpperCamelCase(ANode.Name));
        }

        /// constructor
        public TLstFolderNavigation()
        {
            ResourceDirectory = TAppSettingsManager.GetValueStatic("Resource.Dir");

            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblNavigationCaption.Text = Catalog.GetString("Caption");
            #endregion

            if (System.IO.File.Exists(ResourceDirectory + System.IO.Path.DirectorySeparatorChar + "2leftarrow.png"))
            {
                btnCollapseNavigation.Image = new System.Drawing.Bitmap(ResourceDirectory + System.IO.Path.DirectorySeparatorChar + "2leftarrow.png");
            }
            else
            {
                MessageBox.Show("cannot find file " + ResourceDirectory + System.IO.Path.DirectorySeparatorChar + "2leftarrow.png");
            }

            pnlNavigationCaption.GradientColorTop = System.Drawing.Color.FromArgb(0xF7, 0xFB, 0xFF);
            pnlNavigationCaption.GradientColorBottom = System.Drawing.Color.FromArgb(0xAD, 0xBE, 0xE7);
        }

        private TDashboard FDashboard;

        /// <summary>
        /// set the dashboard so that the task lists can be displayed in the right place
        /// </summary>
        public TDashboard Dashboard
        {
            set
            {
                FDashboard = value;
            }
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

        // avoid recursion of events on Mono
        private bool FMovingSplitter = false;

        private void SptNavigationSplitterMoved(object sender, EventArgs e)
        {
            // TODO: hide lowest folder radio button, add it to panel pnlMoreButtons
            if ((sptNavigation.Panel2.Controls.Count > 0) && !FMovingSplitter
                && (sptNavigation.Height > sptNavigation.Panel2.Controls[0].Height * sptNavigation.Panel2.Controls.Count))
            {
                FMovingSplitter = true;
                sptNavigation.SplitterDistance = sptNavigation.Height - sptNavigation.Panel2.Controls[0].Height * sptNavigation.Panel2.Controls.Count;
                FMovingSplitter = false;
            }
        }

        private void SptNavigationSplitterMoving(object sender, System.Windows.Forms.SplitterCancelEventArgs e)
        {
            // TODO: hide lowest folder radio button, add it to panel pnlMoreButtons
        }

        private TPnlAccordion GetOrCreatePanel(XmlNode AFolderNode)
        {
            if (this.sptNavigation.Panel1.Controls.ContainsKey("pnl" + AFolderNode.Name))
            {
                return (TPnlAccordion) this.sptNavigation.Panel1.Controls["pnl" + AFolderNode.Name];
            }
            else
            {
                TPnlAccordion pnlAccordion = new TPnlAccordion(AFolderNode, FDashboard);

                pnlAccordion.Statusbar = FStatusbar;

                this.sptNavigation.Panel1.Controls.Add(pnlAccordion);

                return pnlAccordion;
            }
        }

        private void FolderCheckedChanged(object sender, EventArgs e)
        {
            TRbtNavigationButton rbtFolder = (TRbtNavigationButton)sender;
            TPnlAccordion pnlAccordion = GetOrCreatePanel((XmlNode)rbtFolder.Tag);

            if (rbtFolder.Checked)
            {
                lblNavigationCaption.Text = rbtFolder.Text;
                pnlAccordion.Show();
                pnlAccordion.SelectFirstLink();
            }
            else
            {
                pnlAccordion.Hide();
            }
        }

        /// <summary>
        /// add a folder to the list
        /// </summary>
        public void AddFolder(XmlNode AFolderNode)
        {
            TRbtNavigationButton rbt = new TRbtNavigationButton();

            this.sptNavigation.Panel2.Controls.Add(rbt);
            rbt.Dock = System.Windows.Forms.DockStyle.Bottom;
            rbt.Tag = AFolderNode;
            rbt.Name = "rbt" + AFolderNode.Name;
            rbt.Text = GetLabel(AFolderNode);

            // TODO: pick up icon from within the resx file, if it is available?
            if (TYml2Xml.HasAttribute(AFolderNode,
                    "Icon")
                && System.IO.File.Exists(ResourceDirectory + System.IO.Path.DirectorySeparatorChar +
                    TYml2Xml.GetAttribute(AFolderNode, "Icon")))
            {
                rbt.Icon = ResourceDirectory + System.IO.Path.DirectorySeparatorChar + TYml2Xml.GetAttribute(AFolderNode, "Icon");
            }

            rbt.CheckedChanged += new System.EventHandler(this.FolderCheckedChanged);
        }

        /// <summary>
        /// Select the given folder
        /// </summary>
        /// <param name="AIndex"></param>
        public void SelectFolder(Int32 AIndex)
        {
            ((TRbtNavigationButton) this.sptNavigation.Panel2.Controls[AIndex]).Checked = true;

            // just make sure the splitter is positioned correctly
            SptNavigationSplitterMoved(null, null);
        }
    }
}