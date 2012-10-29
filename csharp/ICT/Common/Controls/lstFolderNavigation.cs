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
using Ict.Common.IO;
using GNU.Gettext;

namespace Ict.Common.Controls
{
    /// <summary>
    /// This control has a list of folders;
    /// each folder is represented as a radiobutton (TRbtNavigationButton)
    /// </summary>
    public partial class TLstFolderNavigation : System.Windows.Forms.Panel
    {
        private TDashboard FDashboard;
        private TExtStatusBarHelp FStatusbar = null;
        private bool FMovingSplitter = false;       // avoid recursion of events on Mono
        private bool FMultiLedgerSite = false;
        private int FCurrentLedger = -1;

        #region Public Static

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

        #endregion

        /// <summary>Ledger Number value that signalises that the user hasn't got access to any Ledger in the Site.</summary>
        public const int LEDGERNUMBER_NO_ACCESS_TO_ANY_LEDGER = -2;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TLstFolderNavigation()
        {
            ResourceDirectory = TAppSettingsManager.GetValue("Resource.Dir");

            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblNavigationCaption.Text = Catalog.GetString("Caption");
            #endregion

#if disabled
            if (System.IO.File.Exists(ResourceDirectory + System.IO.Path.DirectorySeparatorChar + "2leftarrow.png"))
            {
                btnCollapseNavigation.Image = new System.Drawing.Bitmap(ResourceDirectory + System.IO.Path.DirectorySeparatorChar + "2leftarrow.png");
            }
            else
            {
                MessageBox.Show("cannot find file " + ResourceDirectory + System.IO.Path.DirectorySeparatorChar + "2leftarrow.png");
            }
#endif
        }

        #region Delegates

        /// <summary>
        /// this function checks if the user has access to the navigation node
        /// </summary>
        public delegate bool CheckAccessPermissionDelegate(XmlNode ANode, string AUserId, bool ACheckLedgerPermissions);

        #endregion

        #region Properties

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
        /// True if the Site that OpenPetra is running on uses multiple Ledgers, otherwise false.
        /// </summary>
        public bool MultiLedgerSite
        {
            get
            {
                return FMultiLedgerSite;
            }

            set
            {
                FMultiLedgerSite = value;
            }
        }

        /// <summary>
        /// The currently selected Ledger
        /// </summary>
        public int CurrentLedger
        {
            get
            {
                return FCurrentLedger;
            }

            set
            {
                FCurrentLedger = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// add a folder to the list
        /// </summary>
        public void AddFolder(XmlNode AFolderNode, string AUserId, CheckAccessPermissionDelegate AHasAccessPermission)
        {
            TRbtNavigationButton rbt = new TRbtNavigationButton(FolderCheckChanging);

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

            if ((TYml2Xml.HasAttribute(AFolderNode, "Enabled"))
                && (TYml2Xml.GetAttribute(AFolderNode, "Enabled").ToLower() == "false"))
            {
                rbt.Enabled = false;
            }
            else
            {
                rbt.Enabled = AHasAccessPermission(AFolderNode, AUserId, false);
            }
        }

        /// <summary>
        /// For reloading all Module's navigation Controls (TPnlModuleNavigation), eg. after creating/deleting Ledgers
        /// </summary>
        public void ClearFolders()
        {
            this.sptNavigation.Panel1.Controls.Clear();
            this.sptNavigation.Panel2.Controls.Clear();
        }

        /// <summary>
        /// Select the given folder, if it is enabled
        /// </summary>
        /// <param name="AIndex"></param>
        public bool SelectFolder(Int32 AIndex)
        {
            TRbtNavigationButton btn = (TRbtNavigationButton) this.sptNavigation.Panel2.Controls[AIndex];

            if (btn.Enabled)
            {
                btn.Checked = true;

                // just make sure the splitter is positioned correctly
                SptNavigationSplitterMoved(null, null);

                return true;
            }

            return false;
        }

        /// <summary>
        /// select the first folder that is available (ie enabled)
        /// </summary>
        /// <returns></returns>
        public void SelectFirstAvailableFolder()
        {
            Int32 Index = 0;

            while (Index < this.sptNavigation.Panel2.Controls.Count && !SelectFolder(Index))
            {
                Index++;
            }
        }

        #endregion

        #region Private Methods

        private TPnlModuleNavigation GetOrCreatePanel(XmlNode AFolderNode, out bool APanelCreated)
        {
            TPnlModuleNavigation CollPanelHoster;
            string pnlName = "pnl" + AFolderNode.Name;

            if (AFolderNode.Attributes["Label"] != null)
            {
                pnlName = AFolderNode.Attributes["Label"].Value.Replace(" ", "");
            }

            if (this.sptNavigation.Panel1.Controls.ContainsKey(pnlName))
            {
                APanelCreated = false;

                CollPanelHoster = (TPnlModuleNavigation) this.sptNavigation.Panel1.Controls[pnlName];
                CollPanelHoster.CurrentLedger = FCurrentLedger;

                return CollPanelHoster;
            }
            else
            {
                APanelCreated = true;

                CollPanelHoster = new TPnlModuleNavigation(AFolderNode, FDashboard, this.Width, FMultiLedgerSite);
                CollPanelHoster.Name = pnlName;
                CollPanelHoster.Statusbar = FStatusbar;
                CollPanelHoster.Dock = DockStyle.Left;
                CollPanelHoster.CurrentLedger = FCurrentLedger;
                CollPanelHoster.Collapsed += delegate(object sender, EventArgs e)
                {
                    CollapsibleNavigationCollapsed(sender, e);
                };
                CollPanelHoster.Expanded += delegate(object sender, EventArgs e)
                {
                    CollapsibleNavigationExpanded(sender, e);
                };
                CollPanelHoster.ItemActivation += delegate(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked)
                {
                    OnItemActivation(ATaskList, ATaskListNode, AItemClicked);
                };
                CollPanelHoster.LedgerChanged += delegate(int ALedgerNr, string ALedgerName)
                {
                    OnLedgerChanged(ALedgerNr, ALedgerName);
                };

                this.sptNavigation.Panel1.Controls.Add(CollPanelHoster);

                return CollPanelHoster;
            }
        }

        #endregion

        #region Event Handling

        private bool FolderCheckChanging(TRbtNavigationButton ANavigationButton)
        {
            bool ReturnValue = true;

            XmlNode ModuleXmlNode = (XmlNode)ANavigationButton.Tag;

            if (TXMLParser.GetAttribute(ModuleXmlNode, "DependsOnLedger").ToLower() == "true")
            {
                if (FCurrentLedger == LEDGERNUMBER_NO_ACCESS_TO_ANY_LEDGER)
                {
                    MessageBox.Show(String.Format("Access to OpenPetra Module '{0}' is denied as you don't have access rights to any Ledger!" +
                            "\r\n\r\n" +
                            "Someone with OpenPetra System Administrator rights needs to grant you access rights to at least one Ledger " +
                            "for you to be able to work with this Module.", TXMLParser.GetAttribute(ModuleXmlNode, "Label")),
                        "Access to OpenPetra Module Denied",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }

        private void FolderCheckedChanged(object sender, EventArgs e)
        {
            bool PanelCreated;
            TRbtNavigationButton rbtFolder = (TRbtNavigationButton)sender;
            TPnlModuleNavigation CollPanelHoster = GetOrCreatePanel((XmlNode)rbtFolder.Tag, out PanelCreated);

            if (rbtFolder.Checked)
            {
                CollPanelHoster.Text = rbtFolder.Text;
                CollPanelHoster.Show();

                if (PanelCreated)
                {
                    CollPanelHoster.SelectFirstLink();
                }
                else
                {
                    CollPanelHoster.FireSelectedLinkEvent();
                }
            }
            else
            {
                CollPanelHoster.Hide();
            }
        }

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

        void CollapsibleNavigationCollapsed(object sender, EventArgs e)
        {
            this.Width = ((TPnlModuleNavigation)sender).Width;
//            FFolderCollapsing = true;
//            sptContent.SplitterDistance = cplFolders.Width;
//            FFolderCollapsing = false;
        }

        void CollapsibleNavigationExpanded(object sender, EventArgs e)
        {
            this.Width = ((TPnlModuleNavigation)sender).Width;
//            sptContent.SplitterDistance = cplFolders.Width;
        }

        private void OnItemActivation(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked)
        {
            // TODO
        }

        private void OnLedgerChanged(int ALedgerNr, string ALedgerName)
        {
            string LedgerChangeTitle = Catalog.GetString("Ledger Change");

            FCurrentLedger = ALedgerNr;

            if (ALedgerName != String.Empty)
            {
                MessageBox.Show(String.Format("You have changed the Ledger to\r\n\r\n    Ledger {0} (#{1}).",
                        ALedgerName, ALedgerNr),
                    LedgerChangeTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(String.Format("You have changed the Ledger to\r\n\r\n    Ledger #{0}.", ALedgerNr), LedgerChangeTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion
    }
}