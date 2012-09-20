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
/*using Ict.Petra.Client.App.PetraClient;
using Ict.Petra.Shared;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
 */

namespace ControlTestBench
{
    /// <summary>
    /// Description of MainForm3.
    /// </summary>
    public partial class MainForm3 : Form
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MainForm3()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        void TestShepherd(object sender, EventArgs e)
        {
            XmlDocument UINavigation = LoadYAMLTestFile();
            
            new ControlTestBench.ShepherdTest(UINavigation.FirstChild.NextSibling.FirstChild).Show();
        }


        XmlDocument LoadYAMLTestFile()
        {
            String yamlFile = txtYaml.Text.ToString();
            TYml2Xml parser = new TYml2Xml(yamlFile);
            XmlDocument UINavigation = parser.ParseYML2XML();
            return UINavigation;
        }
        
        void BtnCollapsibleTestClick(object sender, EventArgs e)
        {
            new CollapsibleTest().Show();
        }
        
        void HandlerTaskListTest(object sender, EventArgs e)
        {
            XmlDocument UINavigation = LoadYAMLTestFile();
            
            new TaskListTest(UINavigation.FirstChild.NextSibling.FirstChild).Show();
        }
    }
}
