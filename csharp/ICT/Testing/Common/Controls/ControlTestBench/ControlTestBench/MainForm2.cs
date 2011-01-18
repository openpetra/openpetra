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
	    private XmlDocument Fxmldoc = null;

		public MainForm2(XmlDocument xmldoc)
		{
		    this.Fxmldoc = xmldoc;
			InitializeComponent();
		}
		
		public void TestContent()
		{
		    this.tPnlCollapsible1.UserControlClass = "TUC_PartnerInfo";  // TUC_PartnerInfo
		    this.tPnlCollapsible1.UserControlNamespace = "Ict.Petra.Client.MPartner.Gui";

		    this.tPnlCollapsible1.RealiseUserControlNow();
		}
		
		public void TestTaskList()
		{
		    this.tPnlCollapsible1.TaskListNode = Fxmldoc;
		}
		
		void Button1Click(object sender, EventArgs e)
		{
		    TestTaskList();
		}
	}
}
