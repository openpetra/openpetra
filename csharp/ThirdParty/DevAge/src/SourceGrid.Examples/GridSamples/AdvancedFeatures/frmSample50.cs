using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsSample.GridSamples
{
    [Sample("SourceGrid - Advanced features", 50, "Hide/Show columns")]
    public partial class frmSample50 : Form
    {
        public frmSample50()
        {
            InitializeComponent();
        }

        private void frmSample50_Load(object sender, EventArgs e)
        {
            grid1.BorderStyle = BorderStyle.FixedSingle;

            grid1.ColumnsCount = 3;
            grid1.FixedRows = 1;
            grid1.Rows.Insert(0);
            grid1[0, 0] = new SourceGrid.Cells.ColumnHeader("String");
            grid1[0, 1] = new SourceGrid.Cells.ColumnHeader("DateTime");
            grid1[0, 2] = new SourceGrid.Cells.ColumnHeader("CheckBox");
            grid1.Rows.SuspendLayout();
            for (int r = 1; r < 100; r++)
            {
                grid1.Rows.Insert(r);
                grid1[r, 0] = new SourceGrid.Cells.Cell("Hello " + r.ToString(), typeof(string));
                grid1[r, 1] = new SourceGrid.Cells.Cell(DateTime.Today, typeof(DateTime));
                grid1[r, 2] = new SourceGrid.Cells.CheckBox(null, true);
            }
            grid1.Rows.ResumeLayout();

            grid1.AutoSizeCells();
        }

        private void chkVisible_CheckedChange(object sender, EventArgs e)
        {
            grid1.Columns[0].Visible = chkCol0.Checked;
            grid1.Columns[1].Visible = chkCol1.Checked;
            grid1.Columns[2].Visible = chkCol2.Checked;
        }

        private void chkVisibleRow_CheckedChange(object sender, EventArgs e)
        {
        	grid1.Rows.ShowRow(1, chkRow1.Checked);
        	grid1.Rows.ShowRow(2, chkRow2.Checked);
        }

        private void chkFirst40Row_CheckedChanged(object sender, EventArgs e)
        {
        	grid1.Rows.SuspendLayout();
            for (int r = 1; r < 40; r++)
            {
            	grid1.Rows.ShowRow(r, chkFirst40Row.Checked);
            }
            this.grid1.Rows.ResumeLayout();
        }
    }
}