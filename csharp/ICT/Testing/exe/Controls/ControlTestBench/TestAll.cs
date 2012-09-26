/*
 * Created by SharpDevelop.
 * User: sbird
 * Date: 7/1/2011
 * Time: 10:02 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Ict.Common.Controls;
using Ict.Common.IO;

namespace ControlTestBench
{
    /// <summary>
    /// </summary>
    public partial class TestAll : Form
    {
        XmlNode FTestYAMLNode = null;
        bool FFolderCollapsing = false;        
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public TestAll()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ATestYAMLNode"></param>
        public TestAll(XmlNode ATestYAMLNode)
        {
            FTestYAMLNode = ATestYAMLNode;
            
            InitializeComponent();
            
//            this.sptContent.Panel1.Controls.Clear();
//            
//            this.tPnlCollapsible1 = new TPnlCollapsible(THostedControlKind.hckTaskList, FTestYAMLNode, TCollapseDirection.cdHorizontal, 240, false, TVisualStylesEnum.vsHorizontalCollapse);
//            this.tPnlCollapsible1.BorderStyle = BorderStyle.FixedSingle;
//            this.tPnlCollapsible1.Text = "Finance";
//            this.sptContent.Panel1.Controls.Add(this.tPnlCollapsible1);
        }

        private XmlNode GetHardCodedXmlNodes_TaskList1()
        {
            string[] lines = new string[7];
            
            lines[0] = "TaskGroup:\n";
            lines[1] = "    Task1:\n";
            lines[2] = "        Label: First Item";
            lines[3] = "    Task2:\n";
            lines[4] = "        Label: Second Item";
            lines[5] = "    Task3:\n";
            lines[6] = "        Label: Third Item";
            
            return new TYml2Xml(lines).ParseYML2TaskListRoot();
        }

        private XmlNode GetHardCodedXmlNodes_TaskList2()
        {
            string[] lines = new string[9];
            
            lines[0] = "TaskGroup:\n";
            lines[1] = "    Task1:\n";
            lines[2] = "        Label: One Item";
            lines[3] = "    Task2:\n";
            lines[4] = "        Label: Another Item";
            lines[5] = "    Task3:\n";
            lines[6] = "        Label: Even another Item";
            lines[7] = "    Task4:\n";
            lines[8] = "        Label: The last Item";
            
            return new TYml2Xml(lines).ParseYML2TaskListRoot();
        }
        
        private XmlNode GetHardCodedXmlNodes_AccordionPanel1()
        {
            string[] lines = new string[11];
            
            lines[0] = "TaskGroup:\n";
            lines[1] = "    Task1:\n";
            lines[2] = "        Label: &Gift Processing";
            lines[3] = "    Task2:\n";
            lines[4] = "        Label: &Accounts Payable";
            lines[5] = "    Task3:\n";
            lines[6] = "        Label: &Budgets";
            lines[7] = "    Task4:\n";
            lines[8] = "        Label: General &Ledger";
            lines[9] = "    Task5:\n";
            lines[10] = "        Label: &Setup";
            
            return new TYml2Xml(lines).ParseYML2TaskListRoot();
        }

        private XmlNode GetHardCodedXmlNodes_AccordionPanel2()
        {
            string[] lines = new string[7];
            
            lines[0] = "TaskGroup:\n";
            lines[1] = "    Task1:\n";
            lines[2] = "        Label: First Item";
            lines[3] = "    Task2:\n";
            lines[4] = "        Label: Second Item";
            lines[5] = "    Task3:\n";
            lines[6] = "        Label: Third Item";
            
            return new TYml2Xml(lines).ParseYML2TaskListRoot();
        }
        
        
        private void TestEmptyConstructor(object sender, EventArgs e)
        {
//            this.Controls.Remove(this.FPnl);
//
//            this.FPnl = new TPnlCollapsible();
//            this.Controls.Add(this.FPnl);
        }
        private void TestTaskListVerticalConstructor(object sender, EventArgs e)
        {
//            this.Controls.Remove(this.FPnl);
//
//            this.FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, FTestYAMLNode, TCollapseDirection.cdVertical, 120, false, FEnumStyle);
//            this.Controls.Add(this.FPnl);
        }
        private void TestUserControlVerticalConstructor(object sender, EventArgs e)
        {
//            this.Controls.Remove(this.FPnl);
//
//            this.FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, "Ict.Petra.Client.MPartner.Gui.TUC_PartnerInfo", TCollapseDirection.cdVertical, false, FEnumStyle);
//            this.Controls.Add(this.FPnl);
        }
        private void TestTaskListHorizontalConstructor(object sender, EventArgs e)
        {
//            this.Controls.Remove(this.FPnl);
//
//            this.FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, FTestYAMLNode, TCollapseDirection.cdHorizontal, 183, false, FEnumStyle);
//            this.FPnl.BorderStyle = BorderStyle.FixedSingle;
//            this.FPnl.Text = "&Finance";
//            this.Controls.Add(this.FPnl);
        }
        private void TestTaskListExpandedConstructor(object sender, EventArgs e)
        {
//            this.Controls.Remove(this.FPnl);
//
//            this.FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, TCollapseDirection.cdVertical, false);
//            this.Controls.Add(this.FPnl);
        }
        private void TestFullConstructor(object sender, EventArgs e)
        {
//            this.Controls.Remove(this.FPnl);
//
//            this.FPnl = new TPnlCollapsible(FTestYAMLNode, THostedControlKind.hckUserControl, "Foo.Bar", FEnumStyle, TCollapseDirection.cdHorizontal, true);
//            this.Controls.Add(this.FPnl);
        }
        private void TestStacked(object sender, EventArgs e)
        {
//            this.Controls.Remove(this.FPnl);
//            this.Controls.Remove(this.FPnl2);
//
//            this.FPnl = new TPnlCollapsible();
//            this.FPnl2 = new TPnlCollapsible();
//            this.Controls.Add(this.FPnl);
//            this.Controls.Add(this.FPnl2);
        }

        
        private void ChangeWidth(object sender, EventArgs e)
        {
//            this.FPnl.ExpandedSize = int.Parse(txtChangeExpandedSize.Text);
        }
        private void ChangeText(object sender, EventArgs e)
        {
//            this.FPnl.Text = txtChangeText.Text;
        }
        
        private void ChangeCollapseDirection(object sender, EventArgs e)
        {
//            switch( cboChangeCollapseDirection.Text )
//            {
//                case "Vertical":
//                    this.FPnl.CollapseDirection = TCollapseDirection.cdVertical;
//                    break;
//                case "Horizontal":
//                    this.FPnl.CollapseDirection = TCollapseDirection.cdHorizontal;
//                    break;
//            }
        }
        
        private void ChangeHostedControlKind(object sender, EventArgs e)
        {
//            switch( cboChangeHostedControlKind.Text )
//            {
//                case "UserControl":
//                    this.FPnl.HostedControlKind = THostedControlKind.hckUserControl;
//                    break;
//                case "Task List":
//                    this.FPnl.HostedControlKind = THostedControlKind.hckTaskList;
//                    break;
//            }
        }
        
        private void ChangeUserControlString(object sender, EventArgs e)
        {
//            this.FPnl.UserControlString = txtChangeUserControlString.Text;

            //Won't test the Namespace and class properties because the are implicitely tested by this one.
        }
        
        private void ChangeTaskListNode(object sender, EventArgs e)
        {
//            TYml2Xml parser = new TYml2Xml( rtbChangeTaskListNode.Lines );
//            XmlDocument xmldoc = parser.ParseYML2XML();
//            this.FPnl.TaskListNode = xmldoc.FirstChild.NextSibling.FirstChild;
        }
        
        private void ChangeVisualStyle(object sender, EventArgs e)
        {
//            switch ( cboChangeVisualStyle.Text )
//            {
//                case "vsAccordionPanel":
//                    this.FPnl.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsAccordionPanel;
//                    break;
//                case "vsTaskPanel":
//                    this.FPnl.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsTaskPanel;
//                    break;
//                case "vsDashboard":
//                    this.FPnl.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsDashboard;
//                    break;
//                case "vsShepherd":
//                    this.FPnl.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsShepherd;
//                    break;
//                case "vsHorizontalCollapse":
//                    this.FPnl.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsHorizontalCollapse;
//                    break;
//            }
        }
        
        
        void CplFoldersCollapsed(object sender, EventArgs e)
        {
            FFolderCollapsing = true;
            sptContent.SplitterDistance = cplFolders.Width;
            FFolderCollapsing = false;
        }
        
        void CplFoldersExpanded(object sender, EventArgs e)
        {
            sptContent.SplitterDistance = cplFolders.Width;
        }        
        
        void SptContentSplitterMoved(object sender, SplitterEventArgs e)
        {
            cplFolders.Width = sptContent.SplitterDistance;
            
            if (!FFolderCollapsing) 
            {
                cplFolders.ExpandedSize = sptContent.SplitterDistance;    
            }            
        }
        
        void TestAllLoad(object sender, EventArgs e)
        {
            cplTasks1.TaskListNode = GetHardCodedXmlNodes_TaskList1();
            cplTasks2.TaskListNode = GetHardCodedXmlNodes_TaskList2();
            cplTasks3.TaskListNode = GetHardCodedXmlNodes_AccordionPanel1();
            cplTasks4.TaskListNode = GetHardCodedXmlNodes_AccordionPanel2();
            
            TTaskList tmp = cplTasks1.RealiseTaskListNow();
       }
    }
}
