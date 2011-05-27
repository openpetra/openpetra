//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using GNU.Gettext;
using System.Xml;
using Ict.Common;
using Ict.Common.Controls;

namespace Ict.Common.Controls
{
    #region public data types

    //this pragma warning disables XML comment warnings
#pragma warning disable 1591
    public enum TCollapseDirection
    {
        cdVertical,
        cdHorizontal
    }

    public enum THostedControlKind
    {
        hckUserControl,
            hckTaskList
    }
#pragma warning restore 1591

    #endregion

    /// <summary>
    /// UserControl which acts as a 'Collapsible Panel'.
    /// </summary>
    public partial class TPnlCollapsible : System.Windows.Forms.UserControl
    {
        #region Private constants and fields

        /// <summary>Hard-coded value of the collapsed height</summary>
        private const Int16 COLLAPSEDHEIGHT = 24;

        /// <summary>Hard-coded value of the expanded height</summary>
        private const Int16 EXPANDEDHEIGHT = 153;

        /// <summary>Hard-coded value of the collapsed width</summary>
        private const Int16 COLLAPSEDWIDTH = 29;

        /// <summary>Hard-coded value of the expanded width</summary>
        private const Int16 EXPANDEDWIDTH = 300;

        /// <summary>Keeps track of the collapsed/expanded state</summary>
        private bool FIsCollapsed = false;

        /// <summary>Caches the translated text for several ToolTips</summary>
        private string FToolTipText;

        /// <summary> collapse direction </summary>
        private TCollapseDirection FCollapseDirection;

        /// <summary>default direction</summary>
        private const TCollapseDirection DEFAULT_DIRECTION = TCollapseDirection.cdVertical;

        /// <summary>default Title</summary>
        private const string DEFAULT_TITLE = "## Developer needs to change this ##";

        /// <summary></summary>
        private THostedControlKind FHostedControlKind;

        /// <summary></summary>
        private string FUserControlNamespace;

        /// <summary></summary>
        private string FUserControlClass;

        /// <summary></summary>
        private UserControl FUserControl = null;

        /// <summary></summary>
        private XmlNode FTaskListNode;

        /// <summary></summary>
        private TTaskList FTaskListInstance = null;

        /// <summary></summary>
        private TVisualStylesEnum FVisualStyleEnum;

        #endregion

        #region Events

        /// <summary>Event is fired after the panel has collapsed.</summary>
        public event System.EventHandler Collapsed;

        /// <summary>Event is fired after the panel has expanded.</summary>
        public event System.EventHandler Expanded;

        #endregion

        #region Properties

        /// <summary>
        /// Sets the Title Text.
        /// </summary>
        public override string Text
        {
            get
            {
                return FToolTipText;
            }

            set
            {
                string NewToolTipText;

                // Set Title Text
                lblDetailHeading.Text = value;
                NewToolTipText = String.Format(FToolTipText, value);

                // Update ToolTips
                this.tipCollapseExpandHints.SetToolTip(this.lblDetailHeading, NewToolTipText);
                this.tipCollapseExpandHints.SetToolTip(this.pnlTitleText, NewToolTipText);
                this.tipCollapseExpandHints.SetToolTip(this.btnToggle, NewToolTipText);
            }
        }

        /// <summary>True if the panel is collapsed, otherwise false.</summary>
        public bool IsCollapsed
        {
            get
            {
                return FIsCollapsed;
            }

            set
            {
                FIsCollapsed = value;
            }
        }

        /// <summary>
        /// Sets the Collapse Direction.
        /// </summary>
        public TCollapseDirection CollapseDirection
        {
            get
            {
                return FCollapseDirection;
            }
            set
            {
                FCollapseDirection = value;
            }
        }

        /// <summary>
        /// this gets/sets what kind of stuff is inside.
        /// </summary>
        public THostedControlKind HostedControlKind
        {
            get
            {
                return FHostedControlKind;
            }
            set
            {
                FHostedControlKind = value;
            }
        }

        /// <summary></summary>
        public string UserControlNamespace
        {
            get
            {
                //regex validation (no period at end)
                return FUserControlNamespace;
            }
            set
            {
                FUserControlNamespace = value;
            }
        }

        /// <summary></summary>
        public string UserControlClass
        {
            get
            {
                //regex validation
                return FUserControlClass;
            }
            set
            {
                FUserControlClass = value;
            }
        }

        /// <summary></summary>
        public UserControl UserControlInstance
        {
            get
            {
                return FUserControl;
            }
        }

        /// <summary></summary>
        public XmlNode TaskListNode
        {
            get
            {
                return FTaskListNode;
            }
            set
            {
                FTaskListNode = value;
            }
        }

        /// <summary></summary>
        public TTaskList TaskListInstance
        {
            get
            {
                return FTaskListInstance;
            }
            set
            {
                FTaskListInstance = value;
            }
        }

        /// <summary>
        /// This is not yet implemented.
        /// </summary>
        public TVisualStylesEnum VisualStyle
        {
            get
            {
                return FVisualStyleEnum;
            }
            set
            {
                ChangeVisualStyle(value);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TPnlCollapsible()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblDetailHeading.Text = Catalog.GetString("Collapsible Panel");
            #endregion

            Collapse();
            FHostedControlKind = THostedControlKind.hckTaskList;
            FUserControlClass = "";
            FUserControlNamespace = "";
            FCollapseDirection = DEFAULT_DIRECTION;
            FToolTipText = DEFAULT_TITLE;
            this.Text = DEFAULT_TITLE;
        }

        /// <summary>
        /// Custom Constructor
        /// </summary>
        public TPnlCollapsible(THostedControlKind AHCK, string ATitleText, TCollapseDirection ADirection)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            Collapse();
            FHostedControlKind = AHCK;
            FUserControlClass = "";
            FUserControlNamespace = "";
            FCollapseDirection = ADirection;
            FToolTipText = ATitleText;
            this.Text = ATitleText;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Causes the panel to collapse. Only the Title Panel will be visible after that.
        /// </summary>
        public void Collapse()
        {
            FIsCollapsed = true;

            btnToggle.ImageIndex = 1;

            if (FCollapseDirection == TCollapseDirection.cdVertical)
            {
                TitleShow();
                this.Height = COLLAPSEDHEIGHT;
            }
            else
            {
                TitleHide();
                this.Width = COLLAPSEDWIDTH;
            }
        }

        /// <summary>
        /// Causes the panel to expand. The Title Panel and the Content Panel will be visible after that.
        /// </summary>
        public void Expand()
        {
            FIsCollapsed = false;
            btnToggle.ImageIndex = 0;
            TitleShow();

            if (FCollapseDirection == TCollapseDirection.cdVertical)
            {
                this.Height = EXPANDEDHEIGHT;
            }
            else
            {
                this.Width = EXPANDEDWIDTH;
            }

            switch (FHostedControlKind)
            {
                case THostedControlKind.hckUserControl:
                    InstantiateUserControl();
                    break;

                case THostedControlKind.hckTaskList:
                    InstantiateTaskList();
                    break;
            }
        }

        /// <summary>
        /// Toggles between expanding and collapsing.
        /// </summary>
        public void Toggle()
        {
            if (IsCollapsed)
            {
                Expand();
            }
            else
            {
                Collapse();
            }
        }

        /// <summary>
        /// Allow the outside to force the usercontrol to initialize.
        /// </summary>
        public UserControl RealiseUserControlNow()
        {
            return RealiseUserControl();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Hides the Title text.
        /// </summary>
        private void TitleHide()
        {
            this.lblDetailHeading.Text = "";
        }

        /// <summary>
        /// shows title texzt.
        /// </summary>
        private void TitleShow()
        {
            this.lblDetailHeading.Text = this.Text;
        }

        /// <summary>
        /// Raises the 'Collapsed' Event if something subscribed to it.
        /// </summary>
        private void OnCollapsed()
        {
            if (Collapsed != null)
            {
                Collapsed(this, new EventArgs());
            }
        }

        /// <summary>
        /// Raises the 'Expanded' Event if something subscribed to it.
        /// </summary>
        private void OnExpanded()
        {
            if (Expanded != null)
            {
                Expanded(this, new EventArgs());
            }
        }

        private void ChangeDirection(TCollapseDirection ADirection)
        {
            FCollapseDirection = ADirection;
        }

        /// <summary>
        /// Allow the outside to force the usercontrol to initialize. See RealiseUserControlNow.
        /// </summary>
        private UserControl RealiseUserControl()
        {
            Assembly asm = Assembly.LoadFrom(FUserControlNamespace + ".dll");

            System.Type classType = asm.GetType(FUserControlNamespace + "." + FUserControlClass);

            if (classType == null)
            {
                MessageBox.Show("TPnlCollapsible.RealiseUserControl: Cannot find class " + FUserControlNamespace + "." + FUserControlClass);
            }

            FUserControl = (UserControl)Activator.CreateInstance(classType);

            pnlContent.Controls.Add(FUserControl);
            FUserControl.Dock = DockStyle.Fill;

            return FUserControl;
        }

        /// <summary></summary>
        private void InstantiateUserControl()
        {
            RealiseUserControl();
        }

        /// <summary></summary>
        private void InstantiateTaskList()
        {
            //needs to set correct visual style.
            if (FTaskListNode == null)
            {
                throw new ENoTaskListNodeSpecifiedException();
            }

            FTaskListInstance = new Ict.Common.Controls.TTaskList(FTaskListNode, FVisualStyleEnum);
            this.pnlContent.Controls.Add(FTaskListInstance);
            //FTaskListInstance.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            FTaskListInstance.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        private TVisualStylesEnum ChangeVisualStyle(TVisualStylesEnum AVisualStyle)
        {
            if ((FCollapseDirection != TCollapseDirection.cdHorizontal)
                && ((AVisualStyle == TVisualStylesEnum.vsHorizontalCollapse)
                    || (AVisualStyle == TVisualStylesEnum.vsShepherd)))
            {
                throw new EVisualStyleAndDirectionMismatchException();
            }
            else if ((FCollapseDirection != TCollapseDirection.cdVertical)
                     && (AVisualStyle.Equals(TVisualStylesEnum.vsAccordionPanel)
                         || AVisualStyle.Equals(TVisualStylesEnum.vsDashboard)
                         || AVisualStyle.Equals(TVisualStylesEnum.vsTaskPanel)))
            {
                throw new EVisualStyleAndDirectionMismatchException();
            }

            FVisualStyleEnum = AVisualStyle;

            if (this.TaskListInstance != null)
            {
                switch (AVisualStyle)
                {
                    case TVisualStylesEnum.vsHorizontalCollapse:
                    case TVisualStylesEnum.vsAccordionPanel:
                        this.TaskListInstance.VisualStyle = new TVisualStyles(TVisualStylesEnum.vsAccordionPanel);
                        break;

                    case TVisualStylesEnum.vsTaskPanel:
                    case TVisualStylesEnum.vsDashboard:
                        this.TaskListInstance.VisualStyle = new TVisualStyles(TVisualStylesEnum.vsTaskPanel);
                        break;

                    case TVisualStylesEnum.vsShepherd:
                        this.TaskListInstance.VisualStyle = new TVisualStyles(TVisualStylesEnum.vsShepherd);
                        break;
                }
            }

            //TODO: actually change the collapsible panel's visual properties.
            return AVisualStyle;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event is raised if the Toggle Button is clicked.
        /// </summary>
        /// <param name="sender">The Toggle Button.</param>
        /// <param name="e">Not evaluated.</param>
        private void BtnToggleClick(object sender, EventArgs e)
        {
            if (FIsCollapsed)
            {
                Expand();
                OnExpanded();
            }
            else
            {
                Collapse();
                OnCollapsed();
            }
        }

        /// <summary>
        /// Event is raised when the mouse enters the Toggle Button.
        /// </summary>
        /// <param name="sender">The Toggle Button.</param>
        /// <param name="e">Not evaluated.</param>
        void BtnToggleMouseEnter(object sender, EventArgs e)
        {
            TLogging.Log("BtnToggleMouseEnter: btnToggle.ImageIndex: " + btnToggle.ImageIndex.ToString());

            if (btnToggle.ImageIndex == 0)
            {
                btnToggle.ImageIndex = 3;
            }
            else
            {
                btnToggle.ImageIndex = 2;
            }

            lblDetailHeading.ForeColor = System.Drawing.Color.RoyalBlue;      // RoyalBlue;  SteelBlue     // Blue
            TLogging.Log("BtnToggleMouseEnter END: btnToggle.ImageIndex: " + btnToggle.ImageIndex.ToString());
        }

        /// <summary>
        /// Event is raised if the mouse leaves the Toggle Button.
        /// </summary>
        /// <param name="sender">The Toggle Button.</param>
        /// <param name="e">Not evaluated.</param>
        void BtnToggleMouseLeave(object sender, EventArgs e)
        {
            TLogging.Log("BtnToggleMouseLeave: btnTogglePartnerDetails.ImageIndex: " + btnToggle.ImageIndex.ToString());

            if (btnToggle.ImageIndex == 3)
            {
                btnToggle.ImageIndex = 0;
            }
            else if (btnToggle.ImageIndex != 0)
            {
                btnToggle.ImageIndex = 1;
            }

            lblDetailHeading.ForeColor = System.Drawing.Color.MediumBlue;  // Blue     // DarkBlue
            TLogging.Log("BtnToggleMouseLeave END: btnToggle.ImageIndex: " + btnToggle.ImageIndex.ToString());
        }

        #endregion
    }

    /// <summary>
    /// This Exception is thrown whenever the hosted content is a TaskList, but
    /// a non-TaskList function is used. Or when the hosted content is a custom
    /// UserControl and a non-UserControl funciton is used.
    /// </summary>
    public class EIncompatibleHostedControlKindException : ApplicationException
    {
    }

    /// <summary>
    /// Thrown when instantiating a tasklist, but tasklistnode property not set.
    /// </summary>
    public class ENoTaskListNodeSpecifiedException : ApplicationException
    {
    }

    /// <summary>
    /// there is a mismatch of the visual style and the direction
    /// </summary>
    public class EVisualStyleAndDirectionMismatchException : ApplicationException
    {
    }
}