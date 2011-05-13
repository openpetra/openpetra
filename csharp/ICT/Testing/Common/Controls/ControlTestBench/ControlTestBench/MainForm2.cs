/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 14:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ict.Common.Controls;
using System.Xml;

namespace TestCollapsible
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm2 : Form
	{
	    private XmlNode FXmlNode = null;
	    private TVisualStylesEnum FVisualStyle;

		public MainForm2(XmlNode xmlNode, TVisualStylesEnum AVisualStyle)
		{
		    this.FVisualStyle = AVisualStyle;
		    this.FXmlNode = xmlNode;

		    InitializeComponent();
		    
		    this.tPnlCollapsible1.HostedControlKind = THostedControlKind.hckTaskList;
		    this.tPnlCollapsible1.TaskListNode = FXmlNode;
		    this.tPnlCollapsible1.Text = "Number 1";
//            this.tPnlCollapsible1.VisualStyle = AVisualStyle;
		    
		    this.tPnlCollapsible2.HostedControlKind = THostedControlKind.hckUserControl;
		    this.tPnlCollapsible2.UserControlClass = "TUC_PartnerInfo";  // TUC_PartnerInfo
		    this.tPnlCollapsible2.UserControlNamespace = "Ict.Petra.Client.MPartner.Gui";
		    this.tPnlCollapsible2.Text = "Number 2";
            
		    this.tPnlCollapsible3.HostedControlKind = THostedControlKind.hckTaskList;
            this.tPnlCollapsible3.TaskListNode = FXmlNode;
		    this.tPnlCollapsible3.Text = "Number 3";
//            this.tPnlCollapsible3.VisualStyle = AVisualStyle;
		    
		    this.tPnlCollapsible1.Collapse();
		    this.tPnlCollapsible1.Expand();
		    this.tPnlCollapsible2.Collapse();
		    this.tPnlCollapsible3.Collapse();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
		    this.tPnlCollapsible1.Toggle();
		    this.textBox1.Text = "IsCollapsed: " + this.tPnlCollapsible1.IsCollapsed;
		}
		
		void Button2Click(object sender, EventArgs e)
		{
		    this.tPnlCollapsible2.Toggle();
		    this.textBox2.Text = "IsCollapsed: " + this.tPnlCollapsible2.IsCollapsed;
		}
		
		void Button3Click(object sender, EventArgs e)
		{
		    this.tPnlCollapsible3.Toggle();
		    this.textBox3.Text = "IsCollapsed: " + this.tPnlCollapsible3.IsCollapsed;
		}
		
		void TextBox4TextChanged(object sender, EventArgs e)
		{
		    this.tPnlCollapsible1.Text = ((System.Windows.Forms.TextBox)sender).Text;
		}
		
		void TextBox6TextChanged(object sender, EventArgs e)
		{
			this.tPnlCollapsible3.Text = ((System.Windows.Forms.TextBox)sender).Text;
		}
		
		void TextBox5TextChanged(object sender, EventArgs e)
		{
			this.tPnlCollapsible2.Text = ((System.Windows.Forms.TextBox)sender).Text;
		}
	}
}
