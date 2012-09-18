using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevAge.TestApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            //Create Data Cells
            for (int i = 0; i < types.Length; i++)
            {
                object[] attributes = types[i].GetCustomAttributes(typeof(SampleAttribute), true);
                if (attributes != null && attributes.Length > 0)
                {
                    SampleAttribute sampleAttribute = (SampleAttribute)attributes[0];

                    ListViewItem item = new ListViewItem(sampleAttribute.SampleNumber.ToString() + " - " + sampleAttribute.Description);
                    item.Tag = types[i];
                    listSamples.Items.Add(item);
                }
            }
        }

        private void listSamples_DoubleClick(object sender, EventArgs e)
        {
            if (listSamples.SelectedItems.Count > 0)
            {
                Type formType = (Type)listSamples.SelectedItems[0].Tag;
                Form form = (Form)Activator.CreateInstance(formType);
                form.Owner = this;
                form.Show();
            }
        }

        private void btOpen_Click(object sender, EventArgs e)
        {
            listSamples_DoubleClick(sender, e);
        }
    }
}