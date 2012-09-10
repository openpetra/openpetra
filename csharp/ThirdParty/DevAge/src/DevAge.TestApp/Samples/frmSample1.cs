using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestApp.Samples
{
    [Sample("Other controls", 1, "Send Char (SendKeys)")]
    public partial class frmSample1 : Form
    {
        public frmSample1()
        {
            InitializeComponent();
        }

        private void txtOutput_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F2)
            //{
            //    foreach (char c in txtInput.Text)
            //        DevAge.Windows.Forms.SendCharExact.Send(c);
            //}
        }

        private void txtOutput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                foreach (char c in txtInput.Text)
                    DevAge.Windows.Forms.SendCharExact.Send(c);
            }
        }
    }
}