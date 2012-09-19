/*
 * Created by SharpDevelop.
 * User: sbird
 * Date: 8/16/2011
 * Time: 12:59 PM
 * 
 * created by sethb
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Xml;
using Ict.Common.Controls;
using Ict.Common.IO;

namespace ControlTestBench
{
    /// <summary>
    /// </summary>
    public partial class TaskListTest : Form
    {
        /// <summary>
        /// </summary>
        public TaskListTest()
        {
            InitializeComponent();
        }


        /// <summary>
        /// </summary>
        public void TestDefaultConstructor(object sender, EventArgs e)
        {
            XmlNode xmlnode = GetTestXmlNode();
            TaskList1 = new Ict.Common.Controls.TTaskList(xmlnode);
            TaskList1.Location = new System.Drawing.Point(10,10);
            TaskList1.Size = new System.Drawing.Size(300,200);
            this.Controls.Add(TaskList1);
        }
        /// <summary>
        /// </summary>
        public void TestFullConstructor(object sender, EventArgs e)
        {
            System.Xml.XmlNode xmlnode = GetTestXmlNode();

            TaskList1 = new Ict.Common.Controls.TTaskList(xmlnode, TVisualStylesEnum.vsShepherd);
            TaskList1.Location = new System.Drawing.Point(10,10);
            TaskList1.Size = new System.Drawing.Size(300,200);
            this.Controls.Add(TaskList1);

            throw new Exception(); // Reminder that this function has not been looked over yet.
        }
        /// <summary>
        /// This test is meant to directly manipulate xmlnode and circumnavigate any yml text at all.
        /// That way this is a direct test of the TTaskList functionality itself and not dependent on
        /// TYml2Xml working.
        ///
        /// Some remaining questions:
        ///
        ///  1) Where will the actual callback function be defined? in some .manual.cs file?
        /// </summary>
        public void TestCallbacks(object sender, EventArgs e)
        {
            this.Controls.Remove(TaskList1);

            TaskList1 = new TTaskList(GetTestXmlNode());
            TaskList1.Location = new System.Drawing.Point(10,10);
            TaskList1.Size = new System.Drawing.Size(300,200);
            TaskList1.ItemActivation += delegate
            {
                MessageBox.Show("The `ItemActivaion` event works (from inside TaskList GUI form directly)!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            this.Controls.Add(TaskList1);

            TaskList1.Click += delegate
            {
                MessageBox.Show("Click-ity-doo-da!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

//            throw new Exception(); // Reminder that this function has not been looked over yet.

            //MessageBox.Show("Now Click on the list and see a message box pop up.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //TODO: MessageBox.Show("Now Click on the first Item in the list, and you should see a message box pop up.", "ControlTestBench.TaskListTest.ExampleCallback", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        /// <summary>
        /// This is meant to be a bridge to the old tests written by Chadd and Ashley.
        /// </summary>
        public void TestDisableHide(object Sender, EventArgs e)
        {
            //TODO: setup a tasklist with the assumptions needed for Disable/Hide (see below funcitons).
        }

        /// <summary>
        /// This is copied from the older version of the test written by Chadd and Ashley.
        /// </summary>
        void DisableItemButtonClick(object sender, EventArgs e)
        {
            /*
            TODO: need to finish porting this test to this new form so that it's independent of other tests!

            XmlNode temp = this.taskList1.GetTaskByName("Task7");
            if(temp != null){
                if(this.taskList1.IsDisabled(temp)){
                    this.taskList1.EnableTaskItem(temp);
                }
                else{
                    this.taskList1.DisableTaskItem(temp);
                }
            }
            */
        }
        /// <summary>
        /// This is copied from the older version of the test written by Chadd and Ashley.
        /// </summary>
        void HideItemButtonClick(object sender, EventArgs e)
        {
            /*
            TODO: need to finish porting this test to this new form so that it's independent of other tests!

            XmlNode temp = this.taskList1.GetTaskByName("Task4c");
            if(temp != null){
                if(this.taskList1.IsHidden(temp)){
                    this.taskList1.ShowTaskItem(temp);
                }
                else{
                    this.taskList1.HideTaskItem(temp);
                }
            }
            //temp = this.taskList1.GetTaskByNumber("3");
            //if(temp != null){
                //this.taskList1.ShowTaskItem(temp);
            //}
            */
        }


        /// <summary>
        /// </summary>
        private XmlNode GetTestXmlNode()
        {
            string[] lines = new string[7];
            lines[0] = "TaskGroup:\n";
            lines[1] = "    Task1:\n";
            lines[2] = "        Label: First Item";
            lines[3] = "    Task2:\n";
            lines[4] = "        Label: Second Item";
            lines[5] = "    Task3:\n";
            lines[6] = "        Label: Third Item";
            TYml2Xml parser = new TYml2Xml(lines);
            XmlNode xmlnode = parser.ParseYML2TaskListRoot();
            return xmlnode;
        }

        /// <summary>
        /// </summary>
        public void ExampleCallback()
        {
            MessageBox.Show("The callback worked.", "Tests.Common.Controls.TTestTaskList.ExampleCallback", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
