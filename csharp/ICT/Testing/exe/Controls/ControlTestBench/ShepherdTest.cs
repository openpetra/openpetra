/*
 * >>>> Describe the functionality of this file. <<<<
 *
 * Comment: >>>> Optional comment. <<<<
 *
 * Author:  >>>> Put your full name here <<<<
 *
 * Version: $Revision: 1.3 $ / $Date: 2008/11/27 11:47:02 $
 */

using System;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;

using Ict.Common.Controls;

namespace ControlTestBench
{
/// <summary>
/// Description of ShepherdTest.
/// </summary>
public partial class ShepherdTest : Form
{
    XmlNode FTestYAMNode = null;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public ShepherdTest()
    {
        //
        // The InitializeComponent() call is required for Windows Forms designer support.
        //
        InitializeComponent();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="AXmlNode"></param>
    public ShepherdTest(XmlNode AXmlNode) : this()
    {
        FTestYAMNode = AXmlNode;
        
        this.Controls.Clear();
        
        this.tPnlCollapsible1 = new TPnlCollapsible(THostedControlKind.hckTaskList, FTestYAMNode, TCollapseDirection.cdHorizontal, 240, false, TVisualStylesEnum.vsShepherd);
        this.tPnlCollapsible1.BorderStyle = BorderStyle.FixedSingle;
        this.tPnlCollapsible1.Text = "My Shepherd Test";
        this.Controls.Add(this.tPnlCollapsible1);
        
//        tPnlCollapsible1.VisualStyleEnum = TVisualStylesEnum.vsShepherd;
//        tPnlCollapsible1.TaskListNode = AXmlNode;
//        tPnlCollapsible1.Collapse();
//        tPnlCollapsible1.Expand();                
    }
}
}