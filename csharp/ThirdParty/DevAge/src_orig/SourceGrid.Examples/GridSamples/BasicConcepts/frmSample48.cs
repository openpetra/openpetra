using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsSample.GridSamples
{
    [Sample("SourceGrid - Basic concepts", 48, "Real Grid basic (flat headers, linear backcolor)")]
    public partial class frmSample48 : Form
    {
        public frmSample48()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            grid1.BorderStyle = BorderStyle.FixedSingle;

            grid1.ColumnsCount = 3;
            grid1.FixedRows = 1;
            grid1.Rows.Insert(0);

            DevAge.Drawing.RectangleBorder border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.DarkGreen), new DevAge.Drawing.BorderLine(Color.DarkGreen));

            DevAge.Drawing.VisualElements.ColumnHeader flatHeader = new DevAge.Drawing.VisualElements.ColumnHeader();
            flatHeader.Border = border;
            flatHeader.BackColor = Color.ForestGreen;
            flatHeader.BackgroundColorStyle = DevAge.Drawing.BackgroundColorStyle.Solid;
            SourceGrid.Cells.Views.ColumnHeader headerView = new SourceGrid.Cells.Views.ColumnHeader();
            headerView.Font = new Font(grid1.Font, FontStyle.Bold | FontStyle.Underline);
            headerView.Background = flatHeader;
            headerView.ForeColor = Color.White;

            SourceGrid.Cells.Views.Cell cellView = new SourceGrid.Cells.Views.Cell();
            cellView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.ForestGreen, Color.White, 45);
            cellView.Border = border;
            SourceGrid.Cells.Views.CheckBox checkView = new SourceGrid.Cells.Views.CheckBox();
            checkView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.ForestGreen, Color.White, 45);
            checkView.Border = border;


            grid1[0, 0] = new SourceGrid.Cells.ColumnHeader("String");
            grid1[0, 0].View = headerView;
            
            grid1[0, 1] = new SourceGrid.Cells.ColumnHeader("DateTime");
            grid1[0, 1].View = headerView;

            grid1[0, 2] = new SourceGrid.Cells.ColumnHeader("CheckBox");
            grid1[0, 2].View = headerView;
            for (int r = 1; r < 10; r++)
            {
                grid1.Rows.Insert(r);

                grid1[r, 0] = new SourceGrid.Cells.Cell("Hello " + r.ToString(), typeof(string));
                grid1[r, 0].View = cellView;

                grid1[r, 1] = new SourceGrid.Cells.Cell(DateTime.Today, typeof(DateTime));
                grid1[r, 1].View = cellView;

                grid1[r, 2] = new SourceGrid.Cells.CheckBox(null, true);
                grid1[r, 2].View = checkView;
            }

            grid1.AutoSizeCells();
        }
    }
}