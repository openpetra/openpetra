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

namespace TestCollapsible
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm2 : Form
	{
		public MainForm2()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public void TestContent()
		{
		    this.tPnlCollapsible1.UserControlClass = "TUC_PartnerInfo";  // TUC_PartnerInfo
		    this.tPnlCollapsible1.UserControlNamespace = "Ict.Petra.Client.MPartner.Gui";

		    this.tPnlCollapsible1.RealiseUserControlNow();
		}
		
		public void TestTaskList()
		{
		    this.tPnlCollapsible1.HostedControlKind = THostedControlKind.hckTaskList;
		    this.tPnlCollapsible1.TaskListNode = ...;
		}
		
		void Button1Click(object sender, EventArgs e)
		{
		    TestTaskList();
		}
	}
}
