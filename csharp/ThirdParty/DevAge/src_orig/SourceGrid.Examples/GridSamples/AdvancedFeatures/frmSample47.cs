using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsSample.GridSamples
{
    [Sample("SourceGrid - Advanced features", 47, "Multiple columns sort")]
    public partial class frmSample47 : Form
    {
        public frmSample47()
        {
            InitializeComponent();
        }

        private void frmSample47_Load(object sender, EventArgs e)
        {
            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.ColumnsCount = 3;
            grid1.FixedRows = 1;
            grid1.Rows.Insert(0);

            SourceGrid.Cells.ColumnHeader header1 = new SourceGrid.Cells.ColumnHeader("String");

            // here you can se the other column to sort when the current column is equal
            header1.SortComparer = new SourceGrid.MultiColumnsComparer(1, 2);

            grid1[0, 0] = header1;
            grid1[0, 1] = new SourceGrid.Cells.ColumnHeader("DateTime");
            grid1[0, 2] = new SourceGrid.Cells.ColumnHeader("CheckBox");
            for (int r = 1; r < 10; r++)
            {
                grid1.Rows.Insert(r);
                grid1[r, 0] = new SourceGrid.Cells.Cell("Hello " + r.ToString(), typeof(string));
                grid1[r, 1] = new SourceGrid.Cells.Cell(DateTime.Today.AddDays(7 * r), typeof(DateTime));
                grid1[r, 2] = new SourceGrid.Cells.CheckBox("", true);
            }
            grid1.AutoSizeCells();
        }
    }
}