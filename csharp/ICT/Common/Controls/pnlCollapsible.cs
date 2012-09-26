//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//       Seth Bird
//       Andrew Dillon
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
//
using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using GNU.Gettext;
using System.Xml;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.IO;

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

        /// <summary>This will be the expanded size of panel. Where "size" is the dimension which adjusts during expansion</summary>
        private int FExpandedSize = 200;

        /// <summary>Hard-coded value of the collapsed height</summary>
        private const Int16 COLLAPSEDHEIGHT = 24;

        /// <summary>Hard-coded value of the expanded height</summary>
        private const Int16 EXPANDEDHEIGHT = 153;

        /// <summary>Hard-coded value of the collapsed width</summary>
        private const Int16 COLLAPSEDWIDTH = 33;

        /// <summary>Hard-coded value of the expanded width</summary>
        private const Int16 EXPANDEDWIDTH = 300;

        /// <summary>
        /// Given a direction, then a bool (indicating if panel is collapsed or not), then another bool (indicating whether or not the mouse is hovering)
        /// This will return the correct numerical index that btnToggle.ImageIndex should be.
        /// The values are defined in the static constructor.
        /// </summary>
        private readonly static Dictionary<TCollapseDirection, Dictionary<bool, Dictionary<bool, int>>> ArrowGraphicIndecies;

        /// <summary>Keeps track of the collapsed/expanded state</summary>
        private bool FIsCollapsed = false;

        /// <summary>Caches the translated text for several ToolTips</summary>
        private string FToolTipText;

        /// <summary> collapse direction </summary>
        private TCollapseDirection FCollapseDirection = TCollapseDirection.cdVertical;

        /// <summary>
        /// This is the operating definition of which styles are compatible with which directions.
        /// The values are defined in the static constructor.
        /// </summary>
        public readonly static Dictionary<TCollapseDirection, List<TVisualStylesEnum>> FDirStyleMapping;

        /// <summary>
        ///  If the direction changes without stating visual style,
        ///  then for fault tolerance, the direction will default to still changing
        ///  but will change style to a compatible one. This defines the default style
        ///  for each direction.
        /// </summary>
        public readonly static Dictionary<TCollapseDirection, TVisualStylesEnum> DEFAULT_STYLE;

        /// <summary></summary>
        public readonly static Dictionary<TCollapseDirection, TVisualStylesEnum> StoredStyles;

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

        /// <summary></summary>
        private TVisualStyles FVisualStyle;

        #endregion

        #region Events

        /// <summary>Event is fired after the panel has collapsed.</summary>
        [Description("Event is fired after the panel has collapsed.")]
        [Category("Collapsible Panel")]
        public event System.EventHandler Collapsed;

        /// <summary>Event is fired after the panel has expanded.</summary>
        [Description("Event is fired after the panel has expanded.")]
        [Category("Collapsible Panel")]
        public event System.EventHandler Expanded;

        #endregion

        #region Properties

        /// <summary>
        /// Sets the Title and ToolTip Text. (They are always the same).
        /// </summary>
        [Description("Title and ToolTip Text. (They are always the same).")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return FToolTipText;
            }

            set
            {
                ChangeText(value);
            }
        }

        /// <summary>True if the panel is collapsed, otherwise false.</summary>
        [Description("Set to true if the panel is to be shown collapsed, otherwise to false.")]
        [Category("Collapsible Panel")]
        public bool IsCollapsed
        {
            get
            {
                return FIsCollapsed;
            }

            set
            {
                if (value == true)
                {
                    Collapse();
                }
                else
                {
                    Expand();
                }
            }
        }

        /// <summary>
        /// The size (width <em>or</em>height, depending on <see cref="FCollapseDirection" />) the Control expands to when it gets expanded.
        /// </summary>
        [Description("The size (width *or* height, depending on the the 'CollapseDirection') the Control expands to when it gets expanded.")]
        [Category("Collapsible Panel")]
        public int ExpandedSize
        {
            get
            {
                return FExpandedSize;
            }
            set
            {
                FExpandedSize = value;
                
                // Make this change visible in the WinForms Designer
                if ((this.Site != null)
                    && (this.Site.DesignMode))
                {
                    Toggle();
                    Toggle();
                }                
            }
        }

        /// <summary>
        /// Sets the Collapse Direction.
        /// </summary>
        [Description("Direction in which the panel collapses. Note: certain 'VisualStyleEnum' settings only work with certain CollapseDirection settings!")]
        [Category("Collapsible Panel")]
        public TCollapseDirection CollapseDirection
        {
            get
            {
                return FCollapseDirection;
            }
            set
            {
                ChangeDirection(value);
                
                // Make this change visible in the WinForms Designer
                if ((this.Site != null)
                    && (this.Site.DesignMode))
                {
                    Toggle();
                    Toggle();
                }                                
            }
        }

        /// <summary>
        /// this gets/sets what kind of stuff is inside.
        /// NOTE: this always allow switching of HostedControlKind without checking if appropriate
        /// data is initialized already because 1) tasklistnode initialised to empty list in constructor,
        /// and 2) nothing ever checks if hostedcontrolnamespace and hostedcontrolclass is set. You the
        /// programmer must make sure you set them or else a runtime error will occur!!
        /// </summary>
        [Description("Defines what kind of Control the panel hosts inside itself.")]
        [Category("Collapsible Panel")]
        public THostedControlKind HostedControlKind
        {
            get
            {
                return FHostedControlKind;
            }
            set
            {
                FHostedControlKind = value;
                
                // Make this change visible in the WinForms Designer
                if ((this.Site != null)
                    && (this.Site.DesignMode))
                {
                    Expand();
                }
            }
        }

        /// <summary>There is no real Field called FUserControlString, but this is a convienence for the user.</summary>
        [Category("Collapsible Panel")]
        [Description("If 'HostedControlKind' is set to 'hckUserControl': the full namespace + classname of the UserControl that is to be hosted inside the panel.")]
        public string UserControlString
        {
            get
            {
                return FUserControlNamespace + "." + FUserControlClass;
            }
            set
            {
                Match m = Regex.Match(value, @"^(?<namespace>.*)\.(?<class>[^.]*)$");
                FUserControlClass = m.Groups["class"].Value;
                FUserControlNamespace = m.Groups["namespace"].Value;
            }
        }

        /// <summary></summary>
        [Category("Collapsible Panel")]
        [Description("If 'HostedControlKind' is set to 'hckUserControl': the full namespace of the UserControl that is to be hosted inside the panel.")]
        public string UserControlNamespace
        {
            get
            {
                return FUserControlNamespace;
            }
            set
            {
                FUserControlNamespace = value;
            }
        }

        /// <summary></summary>
        [Category("Collapsible Panel")]
        [Description("If 'HostedControlKind' is set to 'hckUserControl': the classname of the UserControl that is to be hosted inside the panel.")]
        public string UserControlClass
        {
            get
            {
                return FUserControlClass;
            }
            set
            {
                FUserControlClass = value;
            }
        }

        /// <summary></summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UserControl UserControlInstance
        {
            get
            {
                return FUserControl;
            }
        }

        /// <summary></summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public XmlNode TaskListNode
        {
            get
            {
                return FTaskListNode;
            }
            set
            {
                //Note: we do not check to see if the tasklistnode is valid or not.
                FTaskListNode = value;
            }
        }

        /// <summary></summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        /// Visual Style.
        /// </summary>
        [Description("The Style in which the panel will be displayed. Note: certain Styles only work with certain 'CollapseDirection' settings!")]
        [Category("Collapsible Panel")]
        public TVisualStylesEnum VisualStyleEnum
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
        /// Constructor (esp. for WinForms Designer!)
        /// </summary>
        public TPnlCollapsible()
        {
            if (TCommonControlsHelper.IsInDesignMode())
            {
                InitializeComponent();
            }
            else
            {
                SetupControl(new object[] {});
            }
        } 
        
        /// <summary>
        /// static constructors are called once ever during runtime, and it is called the first time that the class
        /// is used in any way whatsoever (when followed after `new` keyword, when called staticly, etc).
        /// It is run before any dynamic constructors are called.
        /// </summary>
        static TPnlCollapsible()
        {
            //assign the variables that should be constant, but I couldn't figure out how to technically do that.
            FDirStyleMapping = new Dictionary<TCollapseDirection, List<TVisualStylesEnum>>();

            List<TVisualStylesEnum> tmp =  new List<TVisualStylesEnum>();
            tmp.Add(TVisualStylesEnum.vsAccordionPanel);
            tmp.Add(TVisualStylesEnum.vsHorizontalCollapse);
            tmp.Add(TVisualStylesEnum.vsShepherd);
            FDirStyleMapping.Add(TCollapseDirection.cdHorizontal, tmp);

            List<TVisualStylesEnum> tmp2 =  new List<TVisualStylesEnum>();
            tmp2.Add(TVisualStylesEnum.vsAccordionPanel);
            tmp2.Add(TVisualStylesEnum.vsDashboard);
            tmp2.Add(TVisualStylesEnum.vsTaskPanel);
            FDirStyleMapping.Add(TCollapseDirection.cdVertical, tmp2);

            DEFAULT_STYLE = new Dictionary<TCollapseDirection, TVisualStylesEnum>();
            DEFAULT_STYLE.Add(TCollapseDirection.cdHorizontal, TVisualStylesEnum.vsHorizontalCollapse);
            DEFAULT_STYLE.Add(TCollapseDirection.cdVertical, TVisualStylesEnum.vsAccordionPanel);

            ArrowGraphicIndecies = new Dictionary<TCollapseDirection, Dictionary<bool, Dictionary<bool, int>>>();
            ArrowGraphicIndecies[TCollapseDirection.cdVertical] = new Dictionary<bool, Dictionary<bool, int>>();
            ArrowGraphicIndecies[TCollapseDirection.cdVertical].Add(true, new Dictionary<bool, int>());
            ArrowGraphicIndecies[TCollapseDirection.cdVertical].Add(false, new Dictionary<bool, int>());
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal] = new Dictionary<bool, Dictionary<bool, int>>();
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal].Add(true, new Dictionary<bool, int>());
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal].Add(false, new Dictionary<bool, int>());

            ArrowGraphicIndecies[TCollapseDirection.cdVertical][true].Add(true, 7);
            ArrowGraphicIndecies[TCollapseDirection.cdVertical][true].Add(false, 6);
            ArrowGraphicIndecies[TCollapseDirection.cdVertical][false].Add(true, 1);
            ArrowGraphicIndecies[TCollapseDirection.cdVertical][false].Add(false, 0);
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal][true].Add(true, 5);
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal][true].Add(false, 4);
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal][false].Add(true, 3);
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal][false].Add(false, 2);            
            
            StoredStyles = new Dictionary<TCollapseDirection, TVisualStylesEnum>();
            StoredStyles[TCollapseDirection.cdVertical] = DEFAULT_STYLE[TCollapseDirection.cdVertical];
            StoredStyles[TCollapseDirection.cdHorizontal] = DEFAULT_STYLE[TCollapseDirection.cdHorizontal];            
        }

        /// <summary>
        /// The constructor allows different initial settings to be passed in in any order.
        /// Below is the mapping of input Types to which setting it affects:
        ///     Ict.Common.Controls.THostedControlKind  = The last parameter of this Type will be set as panel's HostedControlKind
        ///     string || System.String                 = The last parameter of this Type will be set as panel's UserControlString. It should be something like "Some.Namespace.SomeUserControlClass"
        ///     Ict.Common.Controls.TCollapseDirection  = The last parameter of this Type will be set as panel's CollapseDirection
        ///     Ict.Common.Controls.TVisualStylesEnum   = The last parameter of this Type will be set as panel's VisualStyleEnum
        ///     bool || System.Boolean                  = The last parameter of this Type will be set as panel's IsCollapsed property value.
        ///     System.Xml.XmlNode                      = The last parameter of this Type will be set as panel's TaskListNode
        ///     int || System.Int32                     = The last parameter of this Type will be set as panel's ExpandedSize
        /// </summary>        
        public TPnlCollapsible(params object[] Args)
        {
            if (!TCommonControlsHelper.IsInDesignMode())
            {
                SetupControl(Args);
            }
        }

        /// <summary>
        /// Allows different initial settings to be passed in in any order.
        /// Below is the mapping of input Types to which setting it affects:
        ///     Ict.Common.Controls.THostedControlKind  = The last parameter of this Type will be set as panel's HostedControlKind
        ///     string || System.String                 = The last parameter of this Type will be set as panel's UserControlString. It should be something like "Some.Namespace.SomeUserControlClass"
        ///     Ict.Common.Controls.TCollapseDirection  = The last parameter of this Type will be set as panel's CollapseDirection
        ///     Ict.Common.Controls.TVisualStylesEnum   = The last parameter of this Type will be set as panel's VisualStyleEnum
        ///     bool || System.Boolean                  = The last parameter of this Type will be set as panel's IsCollapsed property value.
        ///     System.Xml.XmlNode                      = The last parameter of this Type will be set as panel's TaskListNode
        ///     int || System.Int32                     = The last parameter of this Type will be set as panel's ExpandedSize
        /// </summary>                
        private void SetupControl(params object[] Args)
        {
            bool VisualStyleSpecified = false;
            bool CollapseDirectionSpecified = false;
            bool CollapseSpecified = false;
            bool ExpandedSizeSpecified = false;
            
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            this.lblDetailHeading.Text = "Collapsible Panel";

            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            
            #region defaults
            
            this.Text = "## Developer needs to change this ##";
            this.HostedControlKind = THostedControlKind.hckTaskList;
            this.UserControlNamespace = "";
            this.UserControlClass = "";
            //this.VisualStyleEnum = new TVisualStyles(this.StoredStyles[CollapseDirection]);
            this.FTaskListNode = TYml2Xml.CreateXmlDocument();
            this.ExpandedSize = 153;
            //this.UserControlInstance -- intentionally left undefined at first
            //this.TaskListInstance -- intentionally left undefined at first
            
            #endregion


            foreach(object arg in Args)
            {
                if (arg is Ict.Common.Controls.THostedControlKind)
                {
                    this.HostedControlKind = (THostedControlKind) arg;
                }
                else if(arg is string)
                {
                    this.UserControlString = (string) arg;
                }
                else if(arg is Ict.Common.Controls.TCollapseDirection)
                {
                    if (this.CollapseDirection != (TCollapseDirection) arg) 
                    {
                        this.CollapseDirection = (TCollapseDirection) arg;    
                    }                   
                    
                    CollapseDirectionSpecified = true;
                }
                else if(arg is Ict.Common.Controls.TVisualStylesEnum)
                {
                    this.VisualStyleEnum = (TVisualStylesEnum) arg;
                    VisualStyleSpecified = true;
                }
                else if(arg is bool)
                {
                    //We cannot have it Expand(), and thereby realise content before we're sure that the content is
                    // correctly defined and in place. And we do not guarantee order of parameters. see end of
                    // constructor for setting isCollapse properly.
                    this.FIsCollapsed = (bool) arg;
                    
                    CollapseSpecified = true;
                }
                //else if(arg.GetType().ToString().Equals("System.Integer"))
                else if(arg is int)
                {
                    this.ExpandedSize = (int) arg;
                    
                    ExpandedSizeSpecified = true;
                }
                else if(arg.GetType().IsSubclassOf(typeof(System.Xml.XmlNode)))
                {
                    this.TaskListNode = (XmlNode) arg;
                }
            }

            if (!VisualStyleSpecified) 
            {
                this.VisualStyleEnum = DEFAULT_STYLE[this.CollapseDirection];
            }

            if (!CollapseDirectionSpecified) 
            {
                this.CollapseDirection = TCollapseDirection.cdVertical;                
            }
            
            if (!CollapseSpecified) 
            {
                this.IsCollapsed = true;
            }
                       
            if (ExpandedSizeSpecified) 
            {
                // this is to make sure that panel is using updated width/height
                // since ExpandedSize may have been changed by another parameter
                this.IsCollapsed = this.FIsCollapsed;                
            }

            //start the image assuming that it is not being hovered.
            btnToggle.ImageIndex = ArrowGraphicIndecies[FCollapseDirection][IsCollapsed][false];
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// checks that hosted control kind is set to something and that the appropriate data (like UsercontrolClass) is there.
        /// </summary>
        public bool HckDataMatch()
        {
            if(FHostedControlKind == THostedControlKind.hckTaskList)
            {
                if (FTaskListNode == null)
                {
                    return false;
                }
            }
            else
            {
                // the getter for UserControlString will always return "." when neither class nor namespace is defined.
                if (UserControlString == ".")
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// checks that hosted control kind is set to something and that the appropriate data (like UsercontrolClass) is there.
        /// </summary>
        public bool AssertHckDataMatch()
        {
            if (!HckDataMatch())
            {
                throw new EIncompatibleHostedControlKindException();
            }
            return true;
        }

            #region Direction and Style mapping functions

            /// <summary>
            /// Determines if a given Direction and Style are compatible
            /// </summary>
            public bool DirStyleMatch(TCollapseDirection ADirection, TVisualStylesEnum AStyle)
            {
                return FDirStyleMapping[ADirection].Contains(AStyle);
            }

            /// <summary>
            /// same as `DirStyleMatch`, but will throw an error.
            /// </summary>
            public bool AssertDirStyleMatch(TCollapseDirection ADirection, TVisualStylesEnum AStyle)
            {
                if (!DirStyleMatch(ADirection, AStyle))
                {
                    throw new EVisualStyleAndDirectionMismatchException();
                }
                return true;
            }

            /// <summary/>
            public bool AssertDirStyleMatch()
            {
                return AssertDirStyleMatch(this.FCollapseDirection, this.FVisualStyleEnum);
            }

            /// <summary>
            /// Returns the styles available for a give direction
            /// </summary>
            public List<TVisualStylesEnum> StylesForDirection(TCollapseDirection ADirection)
            {
                return FDirStyleMapping[ADirection];
            }

            /// <summary>
            /// Returns the directions available for a give style
            /// </summary>
            public List<TCollapseDirection> DirectionsForStyle(TVisualStylesEnum AStyle)
            {
                List<TCollapseDirection> returnDirections = new List<TCollapseDirection>();

                foreach(TCollapseDirection dir in FDirStyleMapping.Keys)
                {
                    List<TVisualStylesEnum> styles = FDirStyleMapping[dir];
                    foreach(TVisualStylesEnum style in styles)
                    {
                        if (style == AStyle)
                        {
                            returnDirections.Add(dir);
                        }
                    }
                }

                return returnDirections;
            }

            #endregion

        /// <summary>
        /// Causes the panel to collapse. Only the Title Panel will be visible after that.
        /// </summary>
        public void Collapse()
        {
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

            btnToggle.ImageIndex = 1;
            FIsCollapsed = true;
        
            switch (FHostedControlKind)
            {
                case THostedControlKind.hckUserControl:
                    if (FUserControl != null) 
                    {
                        FUserControl.Visible = false;    
                    }
                    
                    break;

                case THostedControlKind.hckTaskList:
                    if (FTaskListInstance != null) 
                    {
                        FTaskListInstance.Visible = false;    
                    }
                    
                    break;
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
                this.Height = FExpandedSize;
            }
            else
            {
                this.Width = FExpandedSize;
            }

            switch (FHostedControlKind)
            {
                case THostedControlKind.hckUserControl:
                    if (FUserControl != null) 
                    {
                        FUserControl.Visible = true;                        
                    }
                    else
                    {
                        InstantiateUserControl();
                    }
                    
                    if (FTaskListInstance != null) 
                    {
                        FTaskListInstance.Visible = false;    
                    }                    
                    
                    break;

                case THostedControlKind.hckTaskList:
                    if (FTaskListInstance != null) 
                    {
                        FTaskListInstance.Visible = true;                            
                    }
                    else
                    {
                        InstantiateTaskList();
                    }
                    
                    if (FUserControl != null) 
                    {
                        FUserControl.Visible = false;    
                    }                    
                    
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
        /// Allow the outside to force the UserControl to initialize.
        /// </summary>
        public UserControl RealiseUserControlNow()
        {
            return RealiseUserControl();
        }

        /// <summary>
        /// Allow the outside to force the TaskList to initialize.
        /// </summary>
        public TTaskList RealiseTaskListNow()        
        {
            return InstantiateTaskList();
        }
        
        #endregion

        #region Helper Methods

        /// <summary>
        /// Hides the Title text. This is used only internally because collapsed horizontal panels should not be showing title.
        /// </summary>
        private void TitleHide()
        {
            this.lblDetailHeading.Text = "";
        }

        /// <summary>
        /// shows title text. This exists only to pair with TitleHide().
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

        /// <summary>
        /// Changes both the tooltip text and the title text.
        /// </summary>
        private void ChangeText(string ANewText)
        {
            string NewText;
            string NewToolTipText;
            NewText = ANewText;

            // Set Title Text
            this.lblDetailHeading.Text = ANewText;

            // Update ToolTips
            //NewToolTipText = String.Format(FToolTipText, ANewText); --What the heck was this line supposed to do??  ~~sbird
            NewToolTipText = NewText;
            this.FToolTipText = NewToolTipText;
            this.tipCollapseExpandHints.SetToolTip(this.lblDetailHeading, NewToolTipText);
            this.tipCollapseExpandHints.SetToolTip(this.pnlTitleText, NewToolTipText);
            this.tipCollapseExpandHints.SetToolTip(this.btnToggle, NewToolTipText);
        }

        /// <summary></summary>
        private void ChangeDirection(TCollapseDirection ADirection)
        {
            TCollapseDirection oldDirection = FCollapseDirection;
            TCollapseDirection newDirection = ADirection;

            FCollapseDirection = newDirection;

            if (FCollapseDirection == TCollapseDirection.cdHorizontal)
            {
                this.Dock = System.Windows.Forms.DockStyle.Left;
            }
            else
            {
                this.Dock = System.Windows.Forms.DockStyle.Bottom;
            }

            if (! DirStyleMatch(newDirection, FVisualStyleEnum)  )
            {
                this.VisualStyleEnum = StoredStyles[newDirection];                
            }
            else
            {
                ChangeVisualStyle(StoredStyles[newDirection]);
            }
        }
        
        private void ToggleDirection()
        {
            if(FCollapseDirection == TCollapseDirection.cdVertical)
            {
                ChangeDirection(TCollapseDirection.cdHorizontal);
            }
            else
            {
                ChangeDirection(TCollapseDirection.cdVertical);
            }
        }

        /// <summary>
        /// Allow the outside to force the usercontrol to initialize. See RealiseUserControlNow.
        /// </summary>
        private UserControl RealiseUserControl()
        {
            if (pnlContent.Controls.Count == 1)
            {
                // reuse the existing control, otherwise we will overload more and more user controls
                // and only the first user control will be visible
                return (UserControl)pnlContent.Controls[0];
            }

            Assembly asm;
            string dllName = TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + FUserControlNamespace + ".dll";

            try
            {
                asm = Assembly.LoadFrom(dllName);
            }
            catch (Exception)
            {
                TLogging.Log("TPnlCollapsible.RealiseUserControl: cannot load file " + dllName);
                return null;
            }

            System.Type classType = asm.GetType(this.UserControlString);

            if (classType == null)
            {
                MessageBox.Show("Apologies, but an error occured.     Details: TPnlCollapsible.RealiseUserControl: Cannot find class " + FUserControlNamespace + "." + FUserControlClass);
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
        private TTaskList InstantiateTaskList()
        {
            if (FTaskListNode == null)
            {
                if ((this.Site == null) 
                    || (!this.Site.DesignMode))
                {
                    throw new ENoTaskListNodeSpecifiedException();    
                }
                else
                {
                    // Show some hard-coded Tasks in WinForm Designer
                    FTaskListNode = GetHardCodedXmlNodes_ForDesignerOnly();
                }
            }

            FTaskListInstance = new Ict.Common.Controls.TTaskList(FTaskListNode, FVisualStyleEnum);
            this.pnlContent.Controls.Add(FTaskListInstance);
            
            FTaskListInstance.Dock = System.Windows.Forms.DockStyle.Fill;
            
            return FTaskListInstance;
        }

        /// <summary>
        /// Used ONLY for showing some sensible data in the Designer if FHostedControlKind == <see cref="THostedControlKind.hckTaskList" />.
        /// </summary>
        private XmlNode GetHardCodedXmlNodes_ForDesignerOnly()
        {
            string[] lines = new string[7];
            
            lines[0] = "TaskGroup:\n";
            lines[1] = "    Task1:\n";
            lines[2] = "        Label: First Item";
            lines[3] = "    Task2:\n";
            lines[4] = "        Label: Second Item";
            lines[5] = "    Task3:\n";
            lines[6] = "        Label: Third Item";
            
            return new TYml2Xml(lines).ParseYML2TaskListRoot();
        }
        
        /// <summary>
        /// Changes the Visual Style. If the given style is not compatible with the current
        /// direction, then it is simply ignored and the current style is kept and the
        /// function returns current style without throwing any exception.
        /// </summary>
        private TVisualStylesEnum ChangeVisualStyle(TVisualStylesEnum AVisualStyle)
        {
            //This is to support future support where styles can work in multiple directions.
            foreach (TCollapseDirection dir in DirectionsForStyle(AVisualStyle))
            {
                StoredStyles[dir] = AVisualStyle;
            }
 
            if (! DirStyleMatch(FCollapseDirection, AVisualStyle)  )
            {
                return FVisualStyleEnum;
            }

            FVisualStyleEnum = AVisualStyle;
            FVisualStyle = new TVisualStyles(AVisualStyle);

            pnlTitleText.Padding = new Padding(FVisualStyle.TitlePaddingLeft, FVisualStyle.TitlePaddingTop, 
                FVisualStyle.TitlePaddingRight, FVisualStyle.TitlePaddingBottom);                

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
                                   
            lblDetailHeading.Font = FVisualStyle.TitleFont;
            lblDetailHeading.Height = FVisualStyle.TitleFont.Height + FVisualStyle.TitleHeightAdjustment;
            
            lblDetailHeading.ForeColor = FVisualStyle.TitleFontColour;
            tipCollapseExpandHints.ForeColor = FVisualStyle.TitleFontColourHover;

            //TODO: border

            //Gradient
            if (FVisualStyle.UseTitleGradient)
            {
                SuspendLayout();
                pnlTitle.GradientColorBottom = FVisualStyle.TitleGradientEnd;
                pnlTitle.GradientColorTop = FVisualStyle.TitleGradientStart;
                pnlTitle.GradientMode = FVisualStyle.TitleGradientMode;
                pnlTitleText.GradientColorBottom = FVisualStyle.TitleGradientEnd;
                pnlTitleText.GradientColorTop = FVisualStyle.TitleGradientStart;
                pnlTitleText.GradientMode = FVisualStyle.TitleGradientMode;
                ResumeLayout();
            }
            else
            {
                SuspendLayout();
                pnlTitle.GradientColorBottom = FVisualStyle.TitleBackgroundColour;
                pnlTitle.GradientColorTop = FVisualStyle.TitleBackgroundColour;
                pnlTitle.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
                pnlTitleText.GradientColorBottom = FVisualStyle.TitleBackgroundColour;
                pnlTitleText.GradientColorTop = FVisualStyle.TitleBackgroundColour;
                pnlTitleText.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;                
                ResumeLayout();
            }                       

            if (FVisualStyle.UseContentGradient)
            {
                SuspendLayout();
                pnlContent.GradientColorBottom = FVisualStyle.ContentGradientEnd;
                pnlContent.GradientColorTop = FVisualStyle.ContentGradientEnd;
                pnlContent.GradientMode = FVisualStyle.ContentGradientMode;
                ResumeLayout();
            }
            else
            {
                SuspendLayout();
                pnlContent.GradientColorBottom = FVisualStyle.ContentBackgroundColour;
                pnlContent.GradientColorTop = FVisualStyle.ContentBackgroundColour;
                pnlContent.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
                ResumeLayout();
            }                       

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

            btnToggle.ImageIndex = ArrowGraphicIndecies[FCollapseDirection][IsCollapsed][true];
            lblDetailHeading.ForeColor = FVisualStyle.TitleFontColourHover;

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

            btnToggle.ImageIndex = ArrowGraphicIndecies[FCollapseDirection][IsCollapsed][false];
            lblDetailHeading.ForeColor = FVisualStyle.TitleFontColour;

            TLogging.Log("BtnToggleMouseLeave END: btnToggle.ImageIndex: " + btnToggle.ImageIndex.ToString());
        }

        #endregion
    }

    #region Custom Exceptions Defined

    /// <summary>
    /// This Exception is thrown whenever the hosted content is a TaskList, but
    /// a non-TaskList function is used. Or when the hosted content is a custom
    /// UserControl and a non-UserControl funciton is used.
    /// </summary>
    public class EIncompatibleHostedControlKindException : ApplicationException
    {
    }

    /// <summary>
    /// Thrown when instantiating HostedControlKind is hckUserControl, but UserControlString property not set.
    /// </summary>
    public class ENoUserControlSpecifiedException : ApplicationException
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

    #endregion
}
