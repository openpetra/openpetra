/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Xml;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Petra.Client.App.PetraClient
{

	public partial class TFrmMainWindowNew
	{
		private XmlDocument FUINavigation = null;
		
		private void InitializeManualCode()
		{
			rbtMyPetra.Checked = true;
			sptNavigation.Panel1.BackColor = sptNavigation.BackColor;
			sptNavigation.Panel2.BackColor = sptNavigation.BackColor;
			sptNavigation.BackColor = System.Drawing.Color.DarkGray;
        	sptNavigation.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.SptNavigationSplitterMoving);
		}
		
		private void SptNavigationSplitterMoving(object sender, System.Windows.Forms.SplitterCancelEventArgs e)
		{
			// TODO: hide lowest department radio button, add it to panel pnlMoreButtons
		}

		private ListView CreateNewTaskList(XmlNode ASubmoduleNode)
		{
        	ListView lstTasks = new System.Windows.Forms.ListView();
        	lstTasks.Dock = DockStyle.Fill;
        	lstTasks.View = System.Windows.Forms.View.Details;

        	ColumnHeader columnHeader = new System.Windows.Forms.ColumnHeader();
        	columnHeader.Text = "Task";
        	columnHeader.Width = 200;
			lstTasks.Columns.Add(columnHeader);
        	columnHeader = new System.Windows.Forms.ColumnHeader();
        	columnHeader.Text = "Description";
        	columnHeader.Width = 300;
			lstTasks.Columns.Add(columnHeader);
			
        	XmlNode TaskGroupNode = ASubmoduleNode.FirstChild;
        	
        	while (TaskGroupNode != null)
        	{
        		if (TaskGroupNode.Name == "SearchBoxes")
        		{
        			// TODO Search boxes
        		}
        		else
        		{
	        		System.Windows.Forms.ListViewGroup listViewGroup = new System.Windows.Forms.ListViewGroup(
	        			(TYml2Xml.HasAttribute(TaskGroupNode, "Label")?TYml2Xml.GetAttribute(TaskGroupNode, "Label"):TaskGroupNode.Name), 
	        			System.Windows.Forms.HorizontalAlignment.Left);
	        		listViewGroup.Name = TaskGroupNode.Name;
		        	lstTasks.Groups.Add(listViewGroup);
		        	
		        	XmlNode TaskNode = TaskGroupNode.FirstChild;
		        	while (TaskNode != null)
		        	{
	        			ListViewItem task = new ListViewItem(
		        			new string[] {
		        			TYml2Xml.HasAttribute(TaskNode, "Label")?TYml2Xml.GetAttribute(TaskNode, "Label"):TaskNode.Name,
		        			TYml2Xml.GetAttribute(TaskNode, "Description")}
		        		);
		        		task.Group = listViewGroup;
		        		lstTasks.Items.Add(task);
		        		TaskNode = TaskNode.NextSibling;
		        	}
        		}
        		TaskGroupNode = TaskGroupNode.NextSibling;
        	}
        	
        	return lstTasks;
		}
		
		private void LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			Object tag = ((Control)sender).Tag;
			ListView lstTasks = null;
			
			if (tag.GetType() == typeof(ListView))
			{
				lstTasks = (ListView) tag;
			}
			else
			{
				lstTasks = CreateNewTaskList((XmlNode)tag);
				((Control)sender).Tag = lstTasks;
			}
			
			if (pnlContent.Controls.Count > 0)
			{
				pnlContent.Controls.RemoveAt(0);
			}

			pnlContent.Controls.Add(lstTasks);
		}
		
		private XmlNode GetDepartmentFromNavigationFile(string ADepartmentName)
		{
			if (FUINavigation == null)
			{
				TAppSettingsManager opts = new TAppSettingsManager();
				TYml2Xml parser = new TYml2Xml(opts.GetValue("UINavigation.File"));
				FUINavigation = parser.ParseYML2XML();
			}

			XmlNode OpenPetraNode = FUINavigation.FirstChild.NextSibling.FirstChild;
            XmlNode SearchBoxesNode = OpenPetraNode.FirstChild;
            XmlNode MainMenuNode = SearchBoxesNode.NextSibling;
            XmlNode DepartmentNode = MainMenuNode.FirstChild;
			
            while (DepartmentNode != null)
            {
            	if (DepartmentNode.Name == ADepartmentName)
            	{
            		return DepartmentNode;	
            	}
            	DepartmentNode = DepartmentNode.NextSibling;
            }
            
           	throw new Exception("TFrmMainWindowNew::GetDepartmentFromNavigationFile cannot find department node " + ADepartmentName);
        }
		
		private Panel GetOrCreatePanel(string DepartmentName)
		{
			if (this.sptNavigation.Panel1.Controls.ContainsKey("pnl" + DepartmentName))
			{
				return (Panel) this.sptNavigation.Panel1.Controls["pnl" + DepartmentName];
			}
			else
			{
				XmlNode DepartmentNode = GetDepartmentFromNavigationFile(DepartmentName);

				Panel pnlDepartment = new Panel();
				pnlDepartment.Name = "pnl" + DepartmentName;
				pnlDepartment.Dock = DockStyle.Top;
				pnlDepartment.Size = new System.Drawing.Size(sptNavigation.Width, 1);

				XmlNode ModuleNode = DepartmentNode.LastChild;

				while (ModuleNode != null)
				{
					Panel pnlModule = new Panel();
					pnlModule.Dock = DockStyle.Top;
					
					Panel pnlModuleCaption = new Panel();
					pnlModuleCaption.Size = new System.Drawing.Size(sptNavigation.Width, 27);
					pnlModuleCaption.Dock = DockStyle.Top;
					pnlModuleCaption.Click += new System.EventHandler(this.CollapseModuleMenu);
					pnlModule.Controls.Add(pnlModuleCaption);
	
					Label lblModule = new Label();
					lblModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		        	lblModule.ForeColor = System.Drawing.Color.Blue;
		        	lblModule.Location = new System.Drawing.Point(8, 2);
		        	lblModule.Name = "lbl" + ModuleNode.Name;
		        	lblModule.Size = new System.Drawing.Size(153, 23);
		        	lblModule.Text = (TYml2Xml.HasAttribute(ModuleNode, "Label")?TYml2Xml.GetAttribute(ModuleNode, "Label"):ModuleNode.Name);
					lblModule.Click += new System.EventHandler(this.CollapseModuleMenu);
	
		        	Button btnCollapse = new Button();
		        	btnCollapse.Text = "^";
		        	btnCollapse.Tag = pnlModule;
		        	btnCollapse.Click += new System.EventHandler(this.CollapseModuleMenu);
		        	btnCollapse.Size = new System.Drawing.Size(20, 20);
		        	btnCollapse.Location = new System.Drawing.Point(163, 4);
		        	
					XmlNode SubmoduleNode = ModuleNode.FirstChild;
	
					Int32 CounterSubmodules = 0;
					while (SubmoduleNode != null)
					{
						if (SubmoduleNode.Name == "SearchBoxes")
						{
							// todo Search Boxes for Submodule
						}
						else
						{				
							LinkLabel lblSubmodule = new LinkLabel();
							lblSubmodule.Name = SubmoduleNode.Name;
							lblSubmodule.Text = (TYml2Xml.HasAttribute(SubmoduleNode, "Label")?TYml2Xml.GetAttribute(SubmoduleNode, "Label"):SubmoduleNode.Name);
							lblSubmodule.Location = new System.Drawing.Point(8, 25 + CounterSubmodules*20);
							lblSubmodule.Size = new System.Drawing.Size(153, 20);
							lblSubmodule.Tag = SubmoduleNode;
							lblSubmodule.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
							pnlModule.Controls.Add(lblSubmodule);
							
							CounterSubmodules++;
						}
						SubmoduleNode = SubmoduleNode.NextSibling;
					}
	
					pnlModule.Size = new System.Drawing.Size(sptNavigation.Width, 5 + CounterSubmodules*20 + 25);
		        	pnlModuleCaption.Controls.Add(lblModule);
		        	pnlModuleCaption.Controls.Add(btnCollapse);
					pnlDepartment.Controls.Add(pnlModule);
					pnlDepartment.Size = new System.Drawing.Size(sptNavigation.Width, pnlDepartment.Height + pnlModule.Height);
						
		        	ModuleNode = ModuleNode.PreviousSibling;
				}

				this.sptNavigation.Panel1.Controls.Add(pnlDepartment);

				return pnlDepartment;
			}
		}

		private void DepartmentCheckedChanged(object sender, EventArgs e)
		{
			RadioButton rbtDepartment = (RadioButton) sender;
			Panel pnlDepartment = GetOrCreatePanel(rbtDepartment.Name.Substring(3));

			if (rbtDepartment.Checked)
			{
				lblNavigationCaption.Text = rbtDepartment.Text;
				pnlDepartment.Show();
			}
			else
			{
				pnlDepartment.Hide();
			}
			
		}
		
		private void CollapseModuleMenu(object sender, EventArgs e)
		{
			Button btnModuleCollapse;
			Panel pnlModule;
			
			if (sender.GetType() == typeof(Panel))
			{
				// sender is the module caption panel
				pnlModule = (Panel)((Control)sender).Parent;
			}
			else
			{
				// either the button or label or icon
				pnlModule = (Panel)((Control)sender).Parent.Parent;
			}
			
			btnModuleCollapse = (Button)pnlModule.Controls[0].Controls[1];
			
			if (pnlModule.Height == pnlModule.Controls[0].Height)
			{
				// show the menu in full size again
				pnlModule.Height = (Int32)pnlModule.Tag;
				pnlModule.Parent.Height += (Int32)pnlModule.Tag - pnlModule.Controls[0].Height;
				btnModuleCollapse.Text = "^";
			}
			else
			{
				pnlModule.Tag = pnlModule.Height;
				pnlModule.Parent.Height -= (Int32)pnlModule.Tag - pnlModule.Controls[0].Height;
				pnlModule.Height = pnlModule.Controls[0].Height;
				btnModuleCollapse.Text = "v";
			}
		}
	}
	
}
