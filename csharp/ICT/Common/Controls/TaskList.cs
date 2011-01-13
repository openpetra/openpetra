/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 11:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Xml;
using Ict.Common;

namespace Ict.Common.Controls
{
	/// <summary>
	/// Description of TaskList.
	/// </summary>
	public partial class TaskList : UserControl
	{
		public XmlNode MasterXmlNode 
		{ 
			set
			{
				loadTaskItems(value);
				MasterXmlNode = value;
			}
		}
		
		//Private method to load taskItems of a masterXmlNode
		private void loadTaskItems(XmlNode masterXmlNode){
			//@TODO: Implement
			LinkLabel lblSubTaskItem = new LinkLabel();
//			lblSubTaskItem.Name = 
		}

		public TaskList()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	}
}
