/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 13:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Controls;
//using Ict.Petra.Client.App.PetraClient;
//using Ict.Petra.Shared;
//using Ict.Petra.Client.CommonForms;
//using Ict.Petra.Client.App.Core;
//using Ict.Petra.Client.App.Core.RemoteObjects;


namespace ControlTestBench
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
//			String yamlFile = txtYaml.
			
//			TAppSettingsManager opts = new TAppSettingsManager();
//            TYml2Xml parser = new TYml2Xml(opts.GetValue(yamlFile));
//            XmlDocument UINavigation = parser.ParseYML2XML();
//UINavigation.FirstChild
			
//			using ( TaskListCheck newForm = new TaskListCheck() ) newForm.ShowDialog();
			//newForm.Controls.Add(
		}
		
		
		void Button2Click(object sender, EventArgs e)
		{
		    new TestCollapsible.MainForm2().Show();
		}
	}
}
