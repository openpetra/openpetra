/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 14:11
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace ControlTestBench
{
/// <summary>
/// Description of TaskListCheck.
/// </summary>
public partial class TaskListCheck : Form
{
    public TaskListCheck(XmlNode Node, Ict.Common.Controls.TVisualStylesEnum Style)
    {
        //
        // The InitializeComponent() call is required for Windows Forms designer support.
        //
        InitializeComponent(Node, Style);
        //
        // TODO: Add constructor code after the InitializeComponent() call.
        //
    }
}
}