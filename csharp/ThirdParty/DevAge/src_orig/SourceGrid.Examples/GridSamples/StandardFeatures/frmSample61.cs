using System;
using System.Windows.Forms;

using SourceGrid;

namespace WindowsFormsSample.GridSamples.StandardFeatures
{
    /// <summary>
    /// A sample with linked controls
    /// </summary>
    [Sample("SourceGrid - Standard features", 61, "Linked Controls")]
    public class frmSample61 : Form
    {
        /// <summary>
        /// The <see cref="Grid"/> control showing some content
        /// </summary>
        private SourceGrid.Grid grid;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Gets or sets a value indicating whether the selection is changing.
        /// </summary>
        private bool IsRechangingSelection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="frmSample61"/> class.
        /// </summary>
        public frmSample61()
        {
            IsRechangingSelection = false;

            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// If managed resources should be disposed <see langword="true"/>; <see langword="false"/> otherwise.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Occurs when loading this control.
        /// </summary>
        /// <param name="eventArgs">
        /// The <see cref="EventArgs"/>.
        /// </param>
        protected override void OnLoad(EventArgs eventArgs)
        {
            const int numberOfRows = 6;
            const int numberOfCols = 4;
            grid.Redim(numberOfRows, numberOfCols);
            grid.FixedRows = 1;
            grid.FixedColumns = 1;
            grid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            grid.ToolTipText = "Linked Controls";

            grid[0, 0] = new SourceGrid.Cells.Header(null);

            // Create column header
            for (int i = 0; i < numberOfCols; i++)
            {
                string label;

                switch (i)
                {
                    case 1:
                        label = "Description";
                        break;
                    case 2:
                        label = "Value";
                        break;
                    case 3:
                        label = "Unit";
                        break;
                    default:
                        label = null;
                        break;
                }

                var header = new SourceGrid.Cells.ColumnHeader(label) { AutomaticSortEnabled = false };

                grid[0, i] = header;
            }

            // Create some entries
            SourceGrid.Cells.Views.Cell visualAspect = new SourceGrid.Cells.Views.Cell();

            for (int i = 1; i < numberOfRows; i++)
            {
                string label;
                string unit;
                Control usedControl;
                SourceGrid.Cells.Controllers.ToolTipText toolTipController =
                    new SourceGrid.Cells.Controllers.ToolTipText();

                toolTipController.IsBalloon = true;

                switch (i)
                {
                    case 1:
                        label = "Numeric Up Down [3...15]";
                        unit = "µs";
                        usedControl = new NumericUpDown();
                        (usedControl as NumericUpDown).Minimum = 3;
                        (usedControl as NumericUpDown).Maximum = 15;
                        (usedControl as NumericUpDown).Increment = 2;
                        (usedControl as NumericUpDown).Value = 7;
                        break;
                    case 2:
                        label = "Text box with max. length of 15 chars.";
                        unit = ":o)";
                        usedControl = new TextBox();
                        (usedControl as TextBox).MaxLength = 15;
                        (usedControl as TextBox).Text = "Some Text";
                        break;
                    case 3:
                        label = "Disabled text box";
                        unit = "^_^";
                        usedControl = new TextBox();
                        (usedControl as TextBox).Enabled = false;
                        (usedControl as TextBox).Text = "Some disabled Text";
                        break;
                    case 4:
                        label = "Combo box with auto complete";
                        unit = "Prio.";
                        usedControl = new ComboBox();
                        (usedControl as ComboBox).Items.AddRange(
                            new object[] { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" });
                        (usedControl as ComboBox).AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        (usedControl as ComboBox).AutoCompleteSource = AutoCompleteSource.ListItems;
                        break;
                    default:
                        label = "Progress bar";
                        unit = "";
                        Random randNmbr = new Random();
                        usedControl = new ProgressBar();
                        (usedControl as ProgressBar).Value = randNmbr.Next(0, 100);
                        break;
                }

                grid[i, 1] = new SourceGrid.Cells.Cell
                    {
                        View = visualAspect, Value = label, ToolTipText = label + "..." + unit 
                    };
                grid[i, 1].AddController(toolTipController);

                grid[i, 2] = new SourceGrid.Cells.Cell();

                usedControl.Enter += delegate(object sender, EventArgs e)
                {
                    IsRechangingSelection = true;

                    foreach (RowInfo rowInfo in grid.Rows)
                    {
                        grid.Selection.SelectRow(rowInfo.Index, false);
                    }

                    foreach (LinkedControlValue lcv in grid.LinkedControls)
                    {
                        if (lcv.Control == usedControl)
                        {
                            grid.Selection.SelectRow(lcv.Position.Row, true);
                            break;
                        }
                    }

                    IsRechangingSelection = false;
                };

                grid.LinkedControls.Add(new SourceGrid.LinkedControlValue(usedControl, new SourceGrid.Position(i, 2)));

                grid[i, 3] = new SourceGrid.Cells.Cell { View = visualAspect, Value = unit };
            }

            if (grid.Columns[2].MinimalWidth < 127)
            {
                grid.Columns[2].MinimalWidth = 127;
            }

            grid.VScrollBar.ValueChanged += delegate(object sender, EventArgs valueChangedEventArgs)
            {
                // Hide all linked controls above 'new value'
                // Show all linked controls beyond 'new value'
                foreach (LinkedControlValue lcv in grid.LinkedControls)
                {
                    lcv.Control.Visible = lcv.Position.Row > grid.VScrollBar.Value;
                }

                // Reselecting works more or less when scrolling down. But what when scrolling up?
                if (grid.Selection.ActivePosition.Row <= grid.VScrollBar.Value)
                {
                    IsRechangingSelection = false;

                    foreach (LinkedControlValue lcv in grid.LinkedControls)
                    {
                        grid.Selection.SelectRow(lcv.Position.Row, false);
                    }

                    IsRechangingSelection = true;

                    grid.Selection.SelectRow(grid.VScrollBar.Value + 1, true);
                }
            };

            // Focus the custom control when changing selection
            grid.Selection.SelectionChanged += delegate(object sender, RangeRegionChangedEventArgs e)
            {
                if (!IsRechangingSelection && e.AddedRange != null && e.RemovedRange == null)
                {
                    bool isFound = false;
                    int selectedRow = -1;
                    int selectedCol = -1;
                    int[] selectedRows = e.AddedRange.GetRowsIndex();

                    if (sender is SourceGrid.Selection.SelectionBase)
                    {
                        selectedRow = (sender as SourceGrid.Selection.SelectionBase).ActivePosition.Row;
                        selectedCol = (sender as SourceGrid.Selection.SelectionBase).ActivePosition.Column;
                    }

                    if (selectedRows[0] != -1)
                    {
                        selectedRow = selectedRows[0];
                    }

                    foreach (LinkedControlValue lcv in grid.LinkedControls)
                    {
                        if (lcv.Position.Row == selectedRow)
                        {
                            // Remove focus from control
                            isFound = true;
                            lcv.Control.Focus();
                            break;
                        }
                    }

                    if (!isFound)
                    {
                        IsRechangingSelection = true;
                        grid.Selection.Focus(new Position(selectedRow, selectedCol), true);
                        IsRechangingSelection = false;
                    }
                }
            };
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grid = new SourceGrid.Grid();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grid.EnableSort = true;
            this.grid.Location = new System.Drawing.Point(13, 13);
            this.grid.Name = "grid";
            this.grid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grid.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grid.Size = new System.Drawing.Size(259, 250);
            this.grid.TabIndex = 0;
            this.grid.TabStop = true;
            this.grid.ToolTipText = "";
            // 
            // frmLinkedControlSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.grid);
            this.Name = "frmLinkedControlSample";
            this.Text = "frmLinkedControlSample";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
