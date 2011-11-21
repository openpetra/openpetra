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
//using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Controls;
using ControlTestBench;

/*using Ict.Petra.Client.App.PetraClient;
 * using Ict.Petra.Shared;
 * using Ict.Petra.Client.CommonForms;
 * using Ict.Petra.Client.App.Core;
 * using Ict.Petra.Client.App.Core.RemoteObjects;
 */

namespace Ict.Testing.ControlTestBench
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// constructor
        /// </summary>
        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
        }

        void Button1Click(object sender, EventArgs e)
        {
            String yamlFile = txtYaml.Text.ToString();

            //	TAppSettingsManager opts = new TAppSettingsManager();
            //opts.GetValue(yamlFile)
            TYml2Xml parser = new TYml2Xml(yamlFile);
            XmlDocument UINavigation = parser.ParseYML2XML();

            String VisualStyle = cmbVisualStyle.Text.ToString();
            TVisualStylesEnum EnumStyle;

            switch (VisualStyle)
            {
                case "AccordionPanel": EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsAccordionPanel;
                    break;

                case "TaskPanel": EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsTaskPanel;
                    break;

                case "Dashboard":  EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsDashboard;
                    break;

                case "Shepherd": EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsShepherd;
                    break;

                case "HorizontalCollapse":
                default:
                    EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsHorizontalCollapse;
                    break;
            }

            //
            // taskList1
            //

            this.Controls.Remove(taskList1);
//			if(this.taskList1 == null){
            this.taskList1 = new Ict.Common.Controls.TTaskList(UINavigation.FirstChild.NextSibling.FirstChild, EnumStyle);
//			}
//			else{
//				this.taskList1 = new Ict.Common.Controls.TTaskList(this.taskList1.MasterXmlNode,EnumStyle);
//			}
            //	this.taskList1.AutomaticNumbering = false;
            this.taskList1.Location = new System.Drawing.Point(333, 29);
            //	this.taskList1.MasterXmlNode = null;
            this.taskList1.Name = "taskList1";
            this.taskList1.Size = new System.Drawing.Size(322, 213);
            this.taskList1.TabIndex = 6;
            this.Controls.Add(taskList1);
            //	this.taskList1.VisualStyle = null;
//            using ( TaskListCheck newForm = new TaskListCheck(UINavigation.FirstChild.NextSibling.FirstChild,EnumStyle) ) newForm.ShowDialog();
            //newForm.Controls.Add(
        }

        void Button2Click(object sender, EventArgs e)
        {
            String yamlFile = txtYaml.Text.ToString();
            TYml2Xml parser = new TYml2Xml(yamlFile);
            XmlDocument UINavigation = parser.ParseYML2XML();

            String VisualStyle = cmbVisualStyle.Text.ToString();
            TVisualStylesEnum EnumStyle;

            switch (VisualStyle)
            {
                case "AccordionPanel":
                    EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsAccordionPanel;
                    break;

                case "TaskPanel":
                    EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsTaskPanel;
                    break;

                case "Dashboard":
                    EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsDashboard;
                    break;

                case "Shepherd":
                    EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsShepherd;
                    break;

                case "HorizontalCollapse":
                default:
                    EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsHorizontalCollapse;
                    break;
            }

            new MainForm2(UINavigation.FirstChild.NextSibling.FirstChild, EnumStyle).Show();
        }

        void Button3Click(object sender, EventArgs e)
        {
            String yamlFile = txtYaml.Text.ToString();
            TYml2Xml parser = new TYml2Xml(yamlFile);
            XmlDocument UINavigation = parser.ParseYML2XML();

            new ShepherdTest(UINavigation.FirstChild.NextSibling.FirstChild).Show();
        }
    }
}