//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//       sethb
//       Andrew Dillon
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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using GNU.Gettext;
using CustomControl.OrientAbleTextControls;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.IO;

namespace Ict.Common.Controls
{
    #region public data types

    /// <summary>
    /// Direction in which the Collapsible Panel collapses
    /// </summary>
    public enum TCollapseDirection
    {
        /// <summary>Vertical direction</summary>
        cdVertical,

        /// <summary>Horizontal direction (collapses to the left, i.e. Dock=<see cref="DockStyle.Left" />)</summary>
        cdHorizontal,

        /// <summary>Horizontal direction (collapses to the right, i.e. Dock=<see cref="DockStyle.Right" />)</summary>
        cdHorizontalRight
    }

    /// <summary>
    /// Kind of Control that is hosted inside the Collapsible Panel
    /// </summary>
    public enum THostedControlKind
    {
        /// <summary>UserControl (generic)</summary>
        hckUserControl,

        /// <summary>TaskList Control (<see cref="TTaskList" /></summary>
            hckTaskList,

        /// <summary>Collapsible Panel Hoster Control (<see cref="TPnlCollapsibleHoster" /></summary>
            hckCollapsiblePanelHoster
    }

    #endregion

    /// <summary>
    /// UserControl which acts as a 'Collapsible Panel'.
    /// </summary>
    public partial class TPnlCollapsible : System.Windows.Forms.UserControl
    {
        #region Private constants and fields

        private bool FControlProperlyInitialised = false;

        private bool FInitializeComponentRan = false;

        /// <summary>This will be the expanded size of panel. Where "size" is the dimension which adjusts during expansion</summary>
        private int FExpandedSize = 200;

        /// <summary>Hard-coded value of the collapsed height</summary>
        private const Int16 COLLAPSEDHEIGHT = 24;

        /// <summary>Hard-coded value of the expanded height</summary>
        private const Int16 EXPANDEDHEIGHT = 153;

        /// <summary>Hard-coded value of the collapsed width</summary>
        private const Int16 COLLAPSEDWIDTH = 20;

        /// <summary>Hard-coded value of the expanded width</summary>
        private const Int16 EXPANDEDWIDTH = 300;

        /// <summary>
        /// Given a direction, then a bool (indicating if panel is collapsed or not), then another bool (indicating whether or not the mouse is hovering)
        /// This will return the correct numerical index that btnToggle.ImageIndex should be.
        /// The values are defined in the static constructor.
        /// </summary>
        private readonly static Dictionary <TCollapseDirection, Dictionary <bool, Dictionary <bool, int>>>ArrowGraphicIndecies;

        /// <summary>Keeps track of the collapsed/expanded state</summary>
        private bool FIsCollapsed = false;

        /// <summary>Caches the translated text for several ToolTips</summary>
        private string FToolTipText;

        /// <summary> collapse direction </summary>
        private TCollapseDirection FCollapseDirection = TCollapseDirection.cdVertical;

        private string FInfoTextCollapseHorizontalLeft = "Navigation Pane";
        private string FInfoTextCollapseHorizontalLeftShepherd = "Shepherd Pages";
        private string FInfoTextCollapseHorizontalRight = "To-Do Bar";

        /// <summary>
        /// This is the operating definition of which styles are compatible with which directions.
        /// The values are defined in the static constructor.
        /// </summary>
        public readonly static Dictionary <TCollapseDirection, List <TVisualStylesEnum>>FDirStyleMapping;

        /// <summary>
        ///  If the direction changes without stating visual style,
        ///  then for fault tolerance, the direction will default to still changing
        ///  but will change style to a compatible one. This defines the default style
        ///  for each direction.
        /// </summary>
        public readonly static Dictionary <TCollapseDirection, TVisualStylesEnum>DEFAULT_STYLE;

        /// <summary></summary>
        public readonly static Dictionary <TCollapseDirection, TVisualStylesEnum>StoredStyles;

        /// <summary></summary>
        public readonly static Dictionary <THostedControlKind, TCollapseDirection>DEFAULT_COLLAPSEDIRECTION;

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
        private TPnlCollapsibleHoster FPnlCollapsibleHoster = null;

        /// <summary></summary>
        private TVisualStylesEnum FVisualStyleEnum;

        /// <summary></summary>
        private TVisualStyles FVisualStyle = new TVisualStyles(TVisualStylesEnum.vsAccordionPanel);

        /// <summary></summary>
        private bool FMouseHoveringOverCollapseToggle = false;

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

        /// <summary>Fired when a TaskLink got activated (by clicking on it or programmatically).</summary>
        [Description("Event is fired when a TaskLink got activated (by clicking on it or programmatically).")]
        [Category("Collapsible Panel")]
        public event TTaskList.TaskLinkClicked ItemActivation;

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
        [Description(
             "Set to true if the panel is to be shown collapsed, otherwise to false. NOTE: This is not persisted in the desginer-gernerated code on purpose as it could lead to Exceptions when the Form is run!")
        ]
        [Category("Collapsible Panel")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [Description(
             "Direction in which the panel collapses. Note: certain 'VisualStyleEnum' settings only work with certain CollapseDirection settings!")]
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
        [Description(
             "If 'HostedControlKind' is set to 'hckUserControl': the full namespace + classname of the UserControl that is to be hosted inside the panel.")
        ]
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
        [Description(
             "If 'HostedControlKind' is set to 'hckUserControl': the full namespace of the UserControl that is to be hosted inside the panel.")]
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

                UpdateTaskList();
            }
        }

        /// <summary></summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TPnlCollapsibleHoster CollapsiblePanelHosterInstance
        {
            get
            {
                return FPnlCollapsibleHoster;
            }
            set
            {
                FPnlCollapsibleHoster = value;

                UpdateCollapsiblePanelHoster();
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

        /// <summary>
        /// Active Task Item.
        /// </summary>
        /// <remarks>Setting this Property to null has the effect that any ActiveTaskItem
        /// will be un-set, i.e. there will be no ActiveTaskItem.</remarks>
        public XmlNode ActiveTaskItem
        {
            get
            {
                if ((FTaskListInstance != null)
                    && (TaskListNode != null))
                {
                    return FTaskListInstance.ActiveTaskItem;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if ((FTaskListInstance != null)
                    && (TaskListNode != null))
                {
                    FTaskListInstance.ActiveTaskItem = value;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor (esp. for WinForms Designer!)
        /// </summary>
        public TPnlCollapsible()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            FInitializeComponentRan = true;
        }

        /// <summary>
        /// static constructors are called once ever during runtime, and it is called the first time that the class
        /// is used in any way whatsoever (when followed after `new` keyword, when called staticly, etc).
        /// It is run before any dynamic constructors are called.
        /// </summary>
        static TPnlCollapsible()
        {
            //assign the variables that should be constant, but I couldn't figure out how to technically do that.
            FDirStyleMapping = new Dictionary <TCollapseDirection, List <TVisualStylesEnum>>();

            List <TVisualStylesEnum>tmp = new List <TVisualStylesEnum>();
            tmp.Add(TVisualStylesEnum.vsAccordionPanel);
            tmp.Add(TVisualStylesEnum.vsHorizontalCollapse);
            tmp.Add(TVisualStylesEnum.vsHorizontalCollapse_InfoPanelWithGradient);
            tmp.Add(TVisualStylesEnum.vsShepherd);
            FDirStyleMapping.Add(TCollapseDirection.cdHorizontal, tmp);

            List <TVisualStylesEnum>tmp2 = new List <TVisualStylesEnum>();
            tmp2.Add(TVisualStylesEnum.vsHorizontalCollapse);
            tmp.Add(TVisualStylesEnum.vsHorizontalCollapse_InfoPanelWithGradient);
            FDirStyleMapping.Add(TCollapseDirection.cdHorizontalRight, tmp2);

            List <TVisualStylesEnum>tmp3 = new List <TVisualStylesEnum>();
            tmp3.Add(TVisualStylesEnum.vsAccordionPanel);
            tmp3.Add(TVisualStylesEnum.vsDashboard);
            tmp3.Add(TVisualStylesEnum.vsTaskPanel);
            FDirStyleMapping.Add(TCollapseDirection.cdVertical, tmp3);

            DEFAULT_STYLE = new Dictionary <TCollapseDirection, TVisualStylesEnum>();
            DEFAULT_STYLE.Add(TCollapseDirection.cdHorizontal, TVisualStylesEnum.vsHorizontalCollapse);
            DEFAULT_STYLE.Add(TCollapseDirection.cdHorizontalRight, TVisualStylesEnum.vsHorizontalCollapse);
            DEFAULT_STYLE.Add(TCollapseDirection.cdVertical, TVisualStylesEnum.vsAccordionPanel);

            DEFAULT_COLLAPSEDIRECTION = new Dictionary <THostedControlKind, TCollapseDirection>();
            DEFAULT_COLLAPSEDIRECTION.Add(THostedControlKind.hckUserControl, TCollapseDirection.cdVertical);
            DEFAULT_COLLAPSEDIRECTION.Add(THostedControlKind.hckTaskList, TCollapseDirection.cdVertical);
            DEFAULT_COLLAPSEDIRECTION.Add(THostedControlKind.hckCollapsiblePanelHoster, TCollapseDirection.cdVertical);

            // Setup up Arrow Icons
            // Dictionary Elements: 1) TCollapseDirection: direction, 2) bool: indicating if panel is collapsed or not, 3) bool: indicating whether or not the mouse is hovering
            ArrowGraphicIndecies = new Dictionary <TCollapseDirection, Dictionary <bool, Dictionary <bool, int>>>();
            ArrowGraphicIndecies[TCollapseDirection.cdVertical] = new Dictionary <bool, Dictionary <bool, int>>();
            ArrowGraphicIndecies[TCollapseDirection.cdVertical].Add(true, new Dictionary <bool, int>());
            ArrowGraphicIndecies[TCollapseDirection.cdVertical].Add(false, new Dictionary <bool, int>());
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal] = new Dictionary <bool, Dictionary <bool, int>>();
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal].Add(true, new Dictionary <bool, int>());
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal].Add(false, new Dictionary <bool, int>());
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontalRight] = new Dictionary <bool, Dictionary <bool, int>>();
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontalRight].Add(true, new Dictionary <bool, int>());
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontalRight].Add(false, new Dictionary <bool, int>());

            // Note: Though the Icon Indexes are hard-coded here, they can get modified according to VisualStyleEnum when
            //       read through Method 'GetArrowGraphicIndex' (and they should only be read through that Method for
            //       that reason!)
            ArrowGraphicIndecies[TCollapseDirection.cdVertical][true].Add(true, 1);
            ArrowGraphicIndecies[TCollapseDirection.cdVertical][true].Add(false, 0);
            ArrowGraphicIndecies[TCollapseDirection.cdVertical][false].Add(true, 7);
            ArrowGraphicIndecies[TCollapseDirection.cdVertical][false].Add(false, 6);
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal][true].Add(true, 5);
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal][true].Add(false, 4);
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal][false].Add(true, 3);
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontal][false].Add(false, 2);
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontalRight][true].Add(true, 3);
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontalRight][true].Add(false, 2);
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontalRight][false].Add(true, 5);
            ArrowGraphicIndecies[TCollapseDirection.cdHorizontalRight][false].Add(false, 4);

            StoredStyles = new Dictionary <TCollapseDirection, TVisualStylesEnum>();
            StoredStyles[TCollapseDirection.cdVertical] = DEFAULT_STYLE[TCollapseDirection.cdVertical];
            StoredStyles[TCollapseDirection.cdHorizontal] = DEFAULT_STYLE[TCollapseDirection.cdHorizontal];
            StoredStyles[TCollapseDirection.cdHorizontalRight] = DEFAULT_STYLE[TCollapseDirection.cdHorizontalRight];
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
                if (!FInitializeComponentRan)
                {
                    InitializeComponent();

                    FInitializeComponentRan = true;
                }

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
            bool HostedControlKindSpecified = false;

            FControlProperlyInitialised = true;

            foreach (object arg in Args)
            {
                if (arg is Ict.Common.Controls.THostedControlKind)
                {
                    this.HostedControlKind = (THostedControlKind)arg;

                    HostedControlKindSpecified = true;
                }
                else if (arg is string)
                {
                    this.UserControlString = (string)arg;
                }
                else if (arg is Ict.Common.Controls.TCollapseDirection)
                {
                    if (this.CollapseDirection != (TCollapseDirection)arg)
                    {
                        this.CollapseDirection = (TCollapseDirection)arg;
                    }

                    CollapseDirectionSpecified = true;
                }
                else if (arg is Ict.Common.Controls.TVisualStylesEnum)
                {
                    this.VisualStyleEnum = (TVisualStylesEnum)arg;
                    VisualStyleSpecified = true;
                }
                else if (arg is bool)
                {
                    // We cannot have it Expand(), and thereby realise content before we're sure that the content is
                    // correctly defined and in place. And we do not guarantee order of parameters. See end of
                    // constructor for setting IsCollapsed Property properly.
                    this.FIsCollapsed = (bool)arg;

                    CollapseSpecified = true;
                }
                else if (arg is int)
                {
                    this.ExpandedSize = (int)arg;

                    ExpandedSizeSpecified = true;
                }
                else if (arg.GetType().IsSubclassOf(typeof(System.Xml.XmlNode)))
                {
                    this.TaskListNode = (XmlNode)arg;
                }
            }

            if (!CollapseDirectionSpecified)
            {
                if (HostedControlKindSpecified)
                {
                    this.CollapseDirection = DEFAULT_COLLAPSEDIRECTION[HostedControlKind];
                }
                else
                {
                    this.CollapseDirection = TCollapseDirection.cdVertical;
                }
            }

            if (!VisualStyleSpecified)
            {
                this.VisualStyleEnum = DEFAULT_STYLE[this.CollapseDirection];
            }

            AssertDirStyleMatch();

            if (!CollapseSpecified)
            {
                this.IsCollapsed = true;
            }

            if (ExpandedSizeSpecified)
            {
                // This is to make sure that the Panel is using updated width/height
                // since ExpandedSize may have been changed by another parameter
                this.IsCollapsed = this.FIsCollapsed;
            }

            // Start the image assuming that it is not being hovered.
            btnToggle.ImageIndex = GetArrowGraphicIndex(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Call this Method once all Properties are set up programmatically to render the Collapsible Panel correctly.
        /// </summary>
        public void InitUserControl()
        {
            FControlProperlyInitialised = true;

            this.IsCollapsed = FIsCollapsed;
        }

        /// <summary>
        /// checks that hosted control kind is set to something and that the appropriate data (like UsercontrolClass) is there.
        /// </summary>
        public bool HckDataMatch()
        {
            if ((FHostedControlKind == THostedControlKind.hckTaskList)
                || (FHostedControlKind == THostedControlKind.hckCollapsiblePanelHoster))
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
                throw new EInsufficientDataSetForHostedControlKindException();
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

        /// <summary/>
        public bool AssertDirStyleMatch()
        {
            return AssertDirStyleMatch(this.FCollapseDirection, this.FVisualStyleEnum);
        }

        /// <summary>
        /// Same as `AssertDirStyleMatch`, but direction and Style can be specified.
        /// </summary>
        public bool AssertDirStyleMatch(TCollapseDirection ADirection, TVisualStylesEnum AStyle)
        {
            if (!DirStyleMatch(ADirection, AStyle))
            {
                throw new EVisualStyleAndDirectionMismatchException();
            }

            return true;
        }

        /// <summary>
        /// Returns the styles available for a give direction
        /// </summary>
        public static List <TVisualStylesEnum>StylesForDirection(TCollapseDirection ADirection)
        {
            return FDirStyleMapping[ADirection];
        }

        /// <summary>
        /// Returns the Collapse Directions available for the given Visual Style.
        /// </summary>
        /// <returns>Collapse Directions available for the Visual Style specified in Argument <paramref name="AStyle" />.</returns>
        public List <TCollapseDirection>DirectionsForStyle(TVisualStylesEnum AStyle)
        {
            List <TCollapseDirection>returnDirections = new List <TCollapseDirection>();

            foreach (TCollapseDirection dir in FDirStyleMapping.Keys)
            {
                List <TVisualStylesEnum>styles = FDirStyleMapping[dir];

                foreach (TVisualStylesEnum style in styles)
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
        /// Causes the Collapsible Panel to collapse. Only the Title Panel will be visible after that.
        /// </summary>
        public void Collapse()
        {
            FIsCollapsed = true;
            btnToggle.ImageIndex = GetArrowGraphicIndex(FMouseHoveringOverCollapseToggle);

            switch (FHostedControlKind)
            {
                case THostedControlKind.hckUserControl:

                    if (FUserControl != null)
                    {
                        FUserControl.Visible = false;
                    }

                    break;

                case THostedControlKind.hckCollapsiblePanelHoster:

                    if (FPnlCollapsibleHoster != null)
                    {
                        FPnlCollapsibleHoster.Visible = false;
                    }

                    break;

                case THostedControlKind.hckTaskList:

                    if (FTaskListInstance != null)
                    {
                        FTaskListInstance.Visible = false;
                    }

                    break;
            }

            if (FCollapseDirection == TCollapseDirection.cdVertical)
            {
                TitleShow();
                this.Height = COLLAPSEDHEIGHT;
            }
            else
            {
                TitleHide();
                this.Width = COLLAPSEDWIDTH;

                ShowHideCollapsedInfoText(true);
            }

            OnCollapsed();
        }

        /// <summary>
        /// Causes the Collapsible Panel to expand. The Title Panel and the Content Panel will be visible after that.
        /// </summary>
        public void Expand()
        {
            FIsCollapsed = false;
            btnToggle.ImageIndex = GetArrowGraphicIndex(FMouseHoveringOverCollapseToggle);
            TitleShow();

            if (FCollapseDirection == TCollapseDirection.cdVertical)
            {
                this.Height = FExpandedSize;
            }
            else
            {
                this.Width = FExpandedSize;

                ShowHideCollapsedInfoText(false);
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

                    if (FPnlCollapsibleHoster != null)
                    {
                        FPnlCollapsibleHoster.Visible = false;
                    }

                    break;

                case THostedControlKind.hckCollapsiblePanelHoster:

                    if (FPnlCollapsibleHoster != null)
                    {
                        FPnlCollapsibleHoster.Visible = true;
                    }
                    else
                    {
                        if ((FControlProperlyInitialised)
                            || ((this.Site != null)
                                && (this.Site.DesignMode)))
                        {
                            InstantiateCollapsiblePanelHoster();
                        }
                    }

                    if (FTaskListInstance != null)
                    {
                        FTaskListInstance.Visible = false;
                    }

                    if (FUserControl != null)
                    {
                        FUserControl.Visible = false;
                    }

                    break;

                case THostedControlKind.hckTaskList:

                    if (FTaskListInstance != null)
                    {
                        FTaskListInstance.Visible = true;
                    }
                    else
                    {
                        if ((FControlProperlyInitialised)
                            || ((this.Site != null)
                                && (this.Site.DesignMode)))
                        {
                            InstantiateTaskList();
                        }
                    }

                    if (FUserControl != null)
                    {
                        FUserControl.Visible = false;
                    }

                    if (FPnlCollapsibleHoster != null)
                    {
                        FPnlCollapsibleHoster.Visible = false;
                    }

                    break;
            }

            OnExpanded();
        }

        void ShowHideCollapsedInfoText(bool AShow)
        {
            pnlCollapsedInfoText.Visible = AShow;

            if (AShow)
            {
                switch (FCollapseDirection)
                {
                    case TCollapseDirection.cdHorizontal:
                    {
                        if (FVisualStyleEnum == TVisualStylesEnum.vsShepherd)
                        {
                            otlCollapsedInfoText.Text = FInfoTextCollapseHorizontalLeftShepherd;
                        }
                        else
                        {
                            otlCollapsedInfoText.Text = FInfoTextCollapseHorizontalLeft;
                        }

                        otlCollapsedInfoText.ForeColor = FVisualStyle.CollapsedInfoTextFontColour;
                        otlCollapsedInfoText.RotationAngle = 270;

                        break;
                    }

                    case TCollapseDirection.cdHorizontalRight:
                    {
                        otlCollapsedInfoText.Text = FInfoTextCollapseHorizontalRight;
                        otlCollapsedInfoText.RotationAngle = 90;

                        break;
                    }
                }
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

        /// <summary>
        /// Allow the outside to force the CollapsiblePanelHoster to initialize.
        /// </summary>
        public TPnlCollapsibleHoster RealiseCollapsiblePanelHoster()
        {
            return InstantiateCollapsiblePanelHoster();
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
            else if (FCollapseDirection == TCollapseDirection.cdHorizontalRight)
            {
                this.Dock = System.Windows.Forms.DockStyle.Right;
            }
            else
            {
                this.Dock = System.Windows.Forms.DockStyle.Bottom;
            }

            if (!DirStyleMatch(newDirection, FVisualStyleEnum))
            {
                this.VisualStyleEnum = StoredStyles[newDirection];
            }
            else
            {
                ChangeVisualStyle(StoredStyles[newDirection]);
            }
        }

        /// <summary>
        /// Toggles the Collapse Direction of the Collapsible Panel.
        /// </summary>
        public void ToggleDirection()
        {
            if (FCollapseDirection == TCollapseDirection.cdVertical)
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
            Assembly asm = null;
            string dllName = TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + FUserControlNamespace + ".dll";

            try
            {
                asm = Assembly.LoadFrom(dllName);
            }
            catch (Exception)
            {
                if ((this.Site == null)
                    || (!this.Site.DesignMode))
                {
                    throw new EUserControlInvalidNamespaceSpecifiedException("Assembly '" + dllName + "' cannot be loaded.");
                }
            }

            System.Type classType = asm.GetType(this.UserControlString);

            if (classType == null)
            {
                if ((this.Site == null)
                    || (!this.Site.DesignMode))
                {
                    throw new EUserControlCantInstantiateClassException(
                        "Cannot find class '" + FUserControlNamespace + "." + FUserControlClass + " in DLL '" + dllName + "'");
                }
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

            UpdateTaskList();

            FTaskListInstance.ItemActivation += delegate(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked)
            {
                OnItemActivation(ATaskList, ATaskListNode, AItemClicked);
            };

            return FTaskListInstance;
        }

        void UpdateTaskList()
        {
            this.pnlContent.Controls.Add(FTaskListInstance);

            FTaskListInstance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExpandedSize = FTaskListInstance.TaskListMaxHeight + pnlTitleText.Height - 4;
            this.Height = ExpandedSize;
        }

        void UpdateCollapsiblePanelHoster()
        {
            FPnlCollapsibleHoster.Dock = System.Windows.Forms.DockStyle.Fill;

            TVisualStyles VisualStyle = new TVisualStyles(FVisualStyleEnum);
            this.pnlContent.Padding = new Padding(0, VisualStyle.ContentPaddingTop, 0, 0);

            this.pnlContent.Controls.Add(FPnlCollapsibleHoster);

            FPnlCollapsibleHoster.RealiseCollapsiblePanelsNow();
        }

        /// <summary></summary>
        private TPnlCollapsibleHoster InstantiateCollapsiblePanelHoster()
        {
            TVisualStylesEnum PnlCollapsibleVisualStyleEnum;

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

            if (FVisualStyleEnum == TVisualStylesEnum.vsHorizontalCollapse)
            {
                PnlCollapsibleVisualStyleEnum = TVisualStylesEnum.vsAccordionPanel;
            }
            else
            {
                PnlCollapsibleVisualStyleEnum = FVisualStyleEnum;
            }

            FPnlCollapsibleHoster = new Ict.Common.Controls.TPnlCollapsibleHoster(FTaskListNode, PnlCollapsibleVisualStyleEnum);

            UpdateCollapsiblePanelHoster();

            FPnlCollapsibleHoster.ItemActivation += delegate(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked)
            {
                OnItemActivation(ATaskList, ATaskListNode, AItemClicked);
            };

            return FPnlCollapsibleHoster;
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
            foreach (TCollapseDirection dir in DirectionsForStyle(AVisualStyle))
            {
                StoredStyles[dir] = AVisualStyle;
            }

            if (!DirStyleMatch(FCollapseDirection, AVisualStyle))
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

            this.BackColor = FVisualStyle.CollapsiblePanelBackgroundColour;

            lblDetailHeading.Font = FVisualStyle.TitleFont;
            lblDetailHeading.Height = FVisualStyle.TitleFont.Height + FVisualStyle.TitleHeightAdjustment;

            lblDetailHeading.ForeColor = FVisualStyle.TitleFontColour;
            tipCollapseExpandHints.ForeColor = FVisualStyle.TitleFontColourHover;

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
                pnlContent.GradientColorTop = FVisualStyle.ContentGradientStart;
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

            if (FVisualStyle.UseCollapsedInfoGradient)
            {
                SuspendLayout();
                pnlCollapsedInfoText.GradientColorBottom = FVisualStyle.CollapsedInfoGradientEnd;
                pnlCollapsedInfoText.GradientColorTop = FVisualStyle.CollapsedInfoGradientStart;
                pnlCollapsedInfoText.GradientMode = FVisualStyle.CollapsedInfoGradientMode;
                ResumeLayout();
            }
            else
            {
                SuspendLayout();
                pnlCollapsedInfoText.GradientColorBottom = FVisualStyle.CollapsedInfoBackgroundColour;
                pnlCollapsedInfoText.GradientColorTop = FVisualStyle.CollapsedInfoBackgroundColour;
                pnlCollapsedInfoText.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
                ResumeLayout();
            }

            return AVisualStyle;
        }

        /// <summary>
        /// Returns the Index of the Arrow Graphic that should be shown on the Toggle Button according to:
        /// * value of <see cref="FCollapseDirection" />;
        /// * value of <see cref="FIsCollapsed" />;
        /// * value of <see cref="FVisualStyleEnum" />;
        /// * value of <paramref name="AIsMouseHoveringOverToggleButton" />.
        /// /// </summary>
        /// <param name="AIsMouseHoveringOverToggleButton">Set to true if the mouse is hovering over
        /// the Toggle Button, otherwise to false.</param>
        /// <returns>Index of the Arrow Graphic that should be shown on the Toggle Button.</returns>
        private int GetArrowGraphicIndex(bool AIsMouseHoveringOverToggleButton)
        {
            int GraphicIndex = ArrowGraphicIndecies[FCollapseDirection][FIsCollapsed][AIsMouseHoveringOverToggleButton];

            if (FCollapseDirection == TCollapseDirection.cdVertical)
            {
                if (FVisualStyleEnum == TVisualStylesEnum.vsTaskPanel)
                {
                    if (FIsCollapsed)
                    {
                        GraphicIndex += 8;
                    }
                    else
                    {
                        GraphicIndex += 4;
                    }
                }
                else if (FVisualStyleEnum == TVisualStylesEnum.vsDashboard)
                {
                    if (FIsCollapsed)
                    {
                        GraphicIndex += 6;
                    }
                    else
                    {
                        GraphicIndex -= 6;
                    }
                }
            }

            return GraphicIndex;
        }

        private void OnItemActivation(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked)
        {
            // Re-fire Event
            if (ItemActivation != null)
            {
                ItemActivation(ATaskList, ATaskListNode, AItemClicked);
            }
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
            }
            else
            {
                Collapse();
            }
        }

        /// <summary>
        /// Event is raised when the mouse enters the Toggle Button.
        /// </summary>
        /// <param name="sender">The Toggle Button.</param>
        /// <param name="e">Not evaluated.</param>
        void BtnToggleMouseEnter(object sender, EventArgs e)
        {
//            TLogging.Log("BtnToggleMouseEnter: btnToggle.ImageIndex: " + btnToggle.ImageIndex.ToString());

            btnToggle.ImageIndex = GetArrowGraphicIndex(true);
            lblDetailHeading.ForeColor = FVisualStyle.TitleFontColourHover;
            FMouseHoveringOverCollapseToggle = true;

//            TLogging.Log("BtnToggleMouseEnter END: btnToggle.ImageIndex: " + btnToggle.ImageIndex.ToString());
        }

        /// <summary>
        /// Event is raised if the mouse leaves the Toggle Button.
        /// </summary>
        /// <param name="sender">The Toggle Button.</param>
        /// <param name="e">Not evaluated.</param>
        void BtnToggleMouseLeave(object sender, EventArgs e)
        {
//            TLogging.Log("BtnToggleMouseLeave: btnTogglePartnerDetails.ImageIndex: " + btnToggle.ImageIndex.ToString());

            btnToggle.ImageIndex = GetArrowGraphicIndex(false);
            lblDetailHeading.ForeColor = FVisualStyle.TitleFontColour;
            FMouseHoveringOverCollapseToggle = true;

//            TLogging.Log("BtnToggleMouseLeave END: btnToggle.ImageIndex: " + btnToggle.ImageIndex.ToString());
        }

        /// <summary>
        /// Event is raised when the mouse enters the rotated Collapsed Info Text Label.
        /// </summary>
        /// <param name="sender">The Toggle Button.</param>
        /// <param name="e">Not evaluated.</param>
        void BtnCollapsedInfoTextMouseEnter(object sender, EventArgs e)
        {
            otlCollapsedInfoText.ForeColor = FVisualStyle.CollapsedInfoTextFontColourHover;
            BtnToggleMouseEnter(this, null);
        }

        /// <summary>
        /// Event is raised if the mouse leaves the rotated Collapsed Info Text Label.
        /// </summary>
        /// <param name="sender">The Toggle Button.</param>
        /// <param name="e">Not evaluated.</param>
        void BtnCollapsedInfoTextMouseLeave(object sender, EventArgs e)
        {
            otlCollapsedInfoText.ForeColor = FVisualStyle.CollapsedInfoTextFontColour;
            BtnToggleMouseLeave(this, null);
        }

        /// <summary>
        /// Event is raised when the user clicks the rotated text in the Collapsed Info Panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CollapsedInfoTextClick(object sender, EventArgs e)
        {
            BtnToggleClick(this, null);
            BtnToggleMouseLeave(this, null);
        }

        #endregion
    }

    #region Custom Exceptions Defined

    /// <summary>
    /// This Exception is thrown whenever the hosted content is a TaskList, but
    /// a non-TaskList function is used. Or when the hosted content is a custom
    /// UserControl and a non-UserControl funciton is used.
    /// </summary>
    public class EInsufficientDataSetForHostedControlKindException : Exception
    {
        /// <summary>
        /// Constructor with no Arguments
        /// </summary>
        public EInsufficientDataSetForHostedControlKindException() : base()
        {
        }

        /// <summary>
        /// Constructor with inner Exception
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="message"></param>
        public EInsufficientDataSetForHostedControlKindException(Exception innerException, string message)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor without inner Exception
        /// </summary>
        /// <param name="message"></param>
        public EInsufficientDataSetForHostedControlKindException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Thrown when instantiating a tasklist, but tasklistnode property not set.
    /// </summary>
    public class ENoTaskListNodeSpecifiedException : Exception
    {
        /// <summary>
        /// Constructor with no Arguments
        /// </summary>
        public ENoTaskListNodeSpecifiedException() : base()
        {
        }

        /// <summary>
        /// Constructor with inner Exception
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="message"></param>
        public ENoTaskListNodeSpecifiedException(Exception innerException, string message)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor without inner Exception
        /// </summary>
        /// <param name="message"></param>
        public ENoTaskListNodeSpecifiedException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Thrown when an Assembly that contains the specified Namespace cannot be loaded.
    /// </summary>
    public class EUserControlInvalidNamespaceSpecifiedException : Exception
    {
        /// <summary>
        /// Constructor with no Arguments
        /// </summary>
        public EUserControlInvalidNamespaceSpecifiedException() : base()
        {
        }

        /// <summary>
        /// Constructor with inner Exception
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="message"></param>
        public EUserControlInvalidNamespaceSpecifiedException(Exception innerException, string message)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor without inner Exception
        /// </summary>
        /// <param name="message"></param>
        public EUserControlInvalidNamespaceSpecifiedException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Thrown when an Instance of the specified Class cannot be found in the Assembly of the specified Namespace.
    /// </summary>
    public class EUserControlCantInstantiateClassException : Exception
    {
        /// <summary>
        /// Constructor with no Arguments
        /// </summary>
        public EUserControlCantInstantiateClassException() : base()
        {
        }

        /// <summary>
        /// Constructor with inner Exception
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="message"></param>
        public EUserControlCantInstantiateClassException(Exception innerException, string message)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor without inner Exception
        /// </summary>
        /// <param name="message"></param>
        public EUserControlCantInstantiateClassException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// there is a mismatch of the visual style and the direction
    /// </summary>
    public class EVisualStyleAndDirectionMismatchException : Exception
    {
        /// <summary>
        /// Constructor with no Arguments
        /// </summary>
        public EVisualStyleAndDirectionMismatchException() : base()
        {
        }

        /// <summary>
        /// Constructor with inner Exception
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="message"></param>
        public EVisualStyleAndDirectionMismatchException(Exception innerException, string message)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor without inner Exception
        /// </summary>
        /// <param name="message"></param>
        public EVisualStyleAndDirectionMismatchException(string message)
            : base(message)
        {
        }
    }

    #endregion
}