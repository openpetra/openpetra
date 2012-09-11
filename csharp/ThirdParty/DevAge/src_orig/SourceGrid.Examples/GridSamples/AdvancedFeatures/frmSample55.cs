using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample55.
	/// </summary>
	[Sample("SourceGrid - Advanced features", 55, "RichText support")]
	public class frmSample55 : System.Windows.Forms.Form
    {
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButtonBold;
        private ToolStripButton toolStripButtonItalic;
        private ToolStripButton toolStripButtonUnderline;
        private ToolStripButton toolStripButtonSuperScript;
        private ToolStripButton toolStripButtonNormalScript;
        private ToolStripButton toolStripButtonSubScript;
        private ToolStripButton toolStripButtonLeft;
        private ToolStripButton toolStripButtonRight;
        private ToolStripButton toolStripButtonCenter;
        private ToolStripSplitButton toolStripSplitButtonForegroundColor;
        private ToolStripSplitButton toolStripSplitButtonBackgroundColor;
        private ColorDialog colorDialogForegroundColor;
        private SourceGrid.Grid grid1;
        private Color foregroundColor;
        private ColorDialog colorDialogBackgroundColor;
        private Color backgroundColor;

        public frmSample55()
        {
            foregroundColor = Color.Black;
            backgroundColor = Color.Blue;
            InitializeComponent();
        }

        #region create_Grid

        private void InitializeGrid(object sender, System.EventArgs e)
        {
            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.ColumnsCount = 3;

            // create header
            grid1.Rows.Insert(0);
            grid1[0, 0] = new SourceGrid.Cells.ColumnHeader("RichTextBox");
            grid1[0, 1] = new SourceGrid.Cells.ColumnHeader("RichTextBox");
            grid1[0, 2] = new SourceGrid.Cells.ColumnHeader("RichTextBox");

            // create 10 rows
            for (Int32 r = 1; r < 10; r++ )
            {
                grid1.Rows.Insert(r);

                // create three columns
                for (Int32 c = 0; c < 3; c++)
                {
                    // create richTextBox
                    SourceGrid.Cells.RichTextBox richTextBox = CreateRichTextBox();
                    SourceGrid.Cells.Views.RichTextBox richTextBoxView = CreateRichTextBoxView();
                    SourceGrid.Cells.Editors.RichTextBox editor = new SourceGrid.Cells.Editors.RichTextBox();
                    editor.Control.Multiline = true;
                    grid1[r, c] = richTextBox;
                    grid1[r, c].Editor = editor;
                    grid1[r, c].View = richTextBoxView;
                }
            }

            grid1.AutoSizeCells();
        }

        private SourceGrid.Cells.RichTextBox CreateRichTextBox()
        {
            DevAge.Windows.Forms.RichText richText = new DevAge.Windows.Forms.RichText(
                    "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0" +
                    "Microsoft Sans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17 Only a \\b " +
                    "Test\\b0.\\par\r\n}\r\n");
            return new SourceGrid.Cells.RichTextBox(richText);
        }

        private SourceGrid.Cells.Views.RichTextBox CreateRichTextBoxView()
        {
            SourceGrid.Cells.Views.RichTextBox richTextBoxView = new SourceGrid.Cells.Views.RichTextBox();
            richTextBoxView.BackColor = Color.AliceBlue;
            return richTextBoxView;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSample55));
            this.grid1 = new SourceGrid.Grid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonBold = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonItalic = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUnderline = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSuperScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNormalScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSubScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLeft = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCenter = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButtonForegroundColor = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSplitButtonBackgroundColor = new System.Windows.Forms.ToolStripSplitButton();
            this.colorDialogForegroundColor = new System.Windows.Forms.ColorDialog();
            this.colorDialogBackgroundColor = new System.Windows.Forms.ColorDialog();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid1
            // 
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.EnableSort = true;
            this.grid1.Location = new System.Drawing.Point(14, 29);
            this.grid1.Name = "grid1";
            this.grid1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grid1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grid1.Size = new System.Drawing.Size(574, 289);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = true;
            this.grid1.ToolTipText = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonBold,
            this.toolStripButtonItalic,
            this.toolStripButtonUnderline,
            this.toolStripButtonSuperScript,
            this.toolStripButtonNormalScript,
            this.toolStripButtonSubScript,
            this.toolStripButtonLeft,
            this.toolStripButtonCenter,
            this.toolStripButtonRight,
            this.toolStripSplitButtonForegroundColor,
            this.toolStripSplitButtonBackgroundColor});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(598, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonBold
            // 
            this.toolStripButtonBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBold.Image = global::WindowsFormsSample.Properties.Resources.text_bold;
            this.toolStripButtonBold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBold.Name = "toolStripButtonBold";
            this.toolStripButtonBold.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonBold.Text = "Bold";
            this.toolStripButtonBold.Click += new System.EventHandler(this.toolStripButtonBold_Click);
            // 
            // toolStripButtonItalic
            // 
            this.toolStripButtonItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonItalic.Image = global::WindowsFormsSample.Properties.Resources.text_italic;
            this.toolStripButtonItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonItalic.Name = "toolStripButtonItalic";
            this.toolStripButtonItalic.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonItalic.Text = "Italic";
            this.toolStripButtonItalic.Click += new System.EventHandler(this.toolStripButtonItalic_Click);
            // 
            // toolStripButtonUnderline
            // 
            this.toolStripButtonUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUnderline.Image = global::WindowsFormsSample.Properties.Resources.text_underline;
            this.toolStripButtonUnderline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUnderline.Name = "toolStripButtonUnderline";
            this.toolStripButtonUnderline.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonUnderline.Text = "Underline";
            this.toolStripButtonUnderline.Click += new System.EventHandler(this.toolStripButtonUnderline_Click);
            // 
            // toolStripButtonSuperScript
            // 
            this.toolStripButtonSuperScript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSuperScript.Image = global::WindowsFormsSample.Properties.Resources.text_superscript;
            this.toolStripButtonSuperScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSuperScript.Name = "toolStripButtonSuperScript";
            this.toolStripButtonSuperScript.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSuperScript.Text = "SuperScript";
            this.toolStripButtonSuperScript.Click += new System.EventHandler(this.toolStripButtonSuperScript_Click);
            // 
            // toolStripButtonNormalScript
            // 
            this.toolStripButtonNormalScript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNormalScript.Image = global::WindowsFormsSample.Properties.Resources.text_normalscript;
            this.toolStripButtonNormalScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNormalScript.Name = "toolStripButtonNormalScript";
            this.toolStripButtonNormalScript.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNormalScript.Text = "NormalScript";
            this.toolStripButtonNormalScript.Click += new System.EventHandler(this.toolStripButtonNormalScript_Click);
            // 
            // toolStripButtonSubScript
            // 
            this.toolStripButtonSubScript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSubScript.Image = global::WindowsFormsSample.Properties.Resources.text_subscript;
            this.toolStripButtonSubScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSubScript.Name = "toolStripButtonSubScript";
            this.toolStripButtonSubScript.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSubScript.Text = "SubScript";
            this.toolStripButtonSubScript.Click += new System.EventHandler(this.toolStripButtonSubScript_Click);
            // 
            // toolStripButtonLeft
            // 
            this.toolStripButtonLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLeft.Image = global::WindowsFormsSample.Properties.Resources.text_align_left;
            this.toolStripButtonLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLeft.Name = "toolStripButtonLeft";
            this.toolStripButtonLeft.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLeft.Text = "Left";
            this.toolStripButtonLeft.Click += new System.EventHandler(this.toolStripButtonLeft_Click);
            // 
            // toolStripButtonCenter
            // 
            this.toolStripButtonCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCenter.Image = global::WindowsFormsSample.Properties.Resources.text_align_center;
            this.toolStripButtonCenter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCenter.Name = "toolStripButtonCenter";
            this.toolStripButtonCenter.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCenter.Text = "Center";
            this.toolStripButtonCenter.Click += new System.EventHandler(this.toolStripButtonCenter_Click);
            // 
            // toolStripButtonRight
            // 
            this.toolStripButtonRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRight.Image = global::WindowsFormsSample.Properties.Resources.text_align_right;
            this.toolStripButtonRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRight.Name = "toolStripButtonRight";
            this.toolStripButtonRight.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRight.Text = "Right";
            this.toolStripButtonRight.Click += new System.EventHandler(this.toolStripButtonRight_Click);
            // 
            // toolStripButtonForegroundColor
            // 
            this.toolStripSplitButtonForegroundColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonForegroundColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonForegroundColor.Image = CreateForegroundImgPicture();
            this.toolStripSplitButtonForegroundColor.Name = "toolStripButtonForegroundColor";
            this.toolStripSplitButtonForegroundColor.Size = new System.Drawing.Size(23, 22);
            this.toolStripSplitButtonForegroundColor.Text = "ForegroundColor";
            this.toolStripSplitButtonForegroundColor.Click += new System.EventHandler(this.toolStripButtonForegroundColor_Click);
            this.toolStripSplitButtonForegroundColor.DropDownOpening += toolStripSplitButtonForegroundColorPicker_Show;
            // 
            // toolStripButtonBackgroundColor
            // 
            this.toolStripSplitButtonBackgroundColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonBackgroundColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonBackgroundColor.Image = CreateBackgroundImgPicture();
            this.toolStripSplitButtonBackgroundColor.Name = "toolStripButtonBackgroundColor";
            this.toolStripSplitButtonBackgroundColor.Size = new System.Drawing.Size(23, 22);
            this.toolStripSplitButtonBackgroundColor.Text = "BackgroundColor";
            this.toolStripSplitButtonBackgroundColor.Click += new System.EventHandler(this.toolStripButtonBackgroundColor_Click);
            this.toolStripSplitButtonBackgroundColor.DropDownOpening += toolStripSplitButtonBackgroundColorPicker_Show;
            // 
            // frmSample55
            // 
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.grid1);
            this.Text = "RichText support";
            this.Name = "frmSample55";
            this.Load += new System.EventHandler(this.InitializeGrid);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// cast cell on position pos to rich text box
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private SourceGrid.Cells.RichTextBox getRichTextBoxCell(SourceGrid.Grid grid, SourceGrid.Position pos)
        {
            return grid.GetCell(pos) as SourceGrid.Cells.RichTextBox;
        }

        private Image CreateForegroundImgPicture()
        {
            Image canvas = global::WindowsFormsSample.Properties.Resources.font;
            // create an object that will do the drawing operations
            Graphics artist = Graphics.FromImage(canvas);
            // draw a few shapes on the canvas picture
            artist.DrawBezier(new Pen(this.foregroundColor, 4), 3, 11, 3, 11, 13, 11, 13, 11);
            // now the drawing is done, we can discard the artist object
            artist.Dispose();
            //return the picture
            return canvas;
        }

        private Image CreateBackgroundImgPicture()
        {
            Image canvas = global::WindowsFormsSample.Properties.Resources.paintcan;
            // create an object that will do the drawing operations
            Graphics artist = Graphics.FromImage(canvas);
            // draw a few shapes on the canvas picture
            artist.DrawBezier(new Pen(this.backgroundColor, 3), 13, 11, 13, 8, 10, 5, 14, 10);
            // now the drawing is done, we can discard the artist object
            artist.Dispose();
            //return the picture
            return canvas;
        }

        #endregion

        #region Events

        /// <summary>
        /// Change selection to bold.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonBold_Click(object sender, EventArgs e)
        {
            SourceGrid.RangeRegion region = grid1.Selection.GetSelectionRegion();
            SourceGrid.PositionCollection poss = region.GetCellsPositions();
            for (int i = 0; i < poss.Count; ++i)
            {
                SourceGrid.Cells.Cell cell = grid1.GetCell(poss[i]) as SourceGrid.Cells.Cell;
                if (cell != null)
                {
                    getRichTextBoxCell(grid1, poss[i]).SelectionBold();
                }
            }
        }
        
        /// <summary>
        /// Change selection to italic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonItalic_Click(object sender, EventArgs e)
        {
            SourceGrid.RangeRegion region = grid1.Selection.GetSelectionRegion();
            SourceGrid.PositionCollection poss = region.GetCellsPositions();
            for (int i = 0; i < poss.Count; ++i)
            {
                SourceGrid.Cells.Cell cell = grid1.GetCell(poss[i]) as SourceGrid.Cells.Cell;
                if (cell != null)
                {
                    getRichTextBoxCell(grid1, poss[i]).SelectionItalic();
                }
            }
        }

        /// <summary>
        /// Underline current selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonUnderline_Click(object sender, EventArgs e)
        {
            SourceGrid.RangeRegion region = grid1.Selection.GetSelectionRegion();
            SourceGrid.PositionCollection poss = region.GetCellsPositions();
            for (int i = 0; i < poss.Count; ++i)
            {
                SourceGrid.Cells.Cell cell = grid1.GetCell(poss[i]) as SourceGrid.Cells.Cell;
                if (cell != null)
                {
                    getRichTextBoxCell(grid1, poss[i]).SelectionUnderline();
                }
            }
        }

        /// <summary>
        /// Change foreground color of current selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonForegroundColor_Click(object sender, EventArgs e)
        {
            SourceGrid.RangeRegion region = grid1.Selection.GetSelectionRegion();
            SourceGrid.PositionCollection poss = region.GetCellsPositions();
            for (int i = 0; i < poss.Count; ++i)
            {
                getRichTextBoxCell(grid1, poss[i]).SelectionColor = foregroundColor;
            }
        }

        /// <summary>
        /// Change brackround color of current selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonBackgroundColor_Click(object sender, EventArgs e)
        {
            
            SourceGrid.RangeRegion region = grid1.Selection.GetSelectionRegion();
            SourceGrid.PositionCollection poss = region.GetCellsPositions();
            for (int i = 0; i < poss.Count; ++i)
            {
                SourceGrid.Cells.Cell cell = grid1.GetCell(poss[i]) as SourceGrid.Cells.Cell;
                if (cell != null)
                {
                    cell.View.BackColor = backgroundColor;
                    grid1.PerformLayout();
                }
            }
        }

        /// <summary>
        /// Show forground color picker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripSplitButtonForegroundColorPicker_Show(object sender, EventArgs e)
        {
            colorDialogForegroundColor.ShowDialog();

            // get the selected color
            Color color = Color.FromArgb((int)colorDialogForegroundColor.Color.R,
                (int)colorDialogForegroundColor.Color.G, (int)colorDialogForegroundColor.Color.B);

            this.foregroundColor = color;
            toolStripSplitButtonForegroundColor.Image = CreateForegroundImgPicture();
        }

        /// <summary>
        /// Show background color picker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripSplitButtonBackgroundColorPicker_Show(object sender, EventArgs e)
        {
            colorDialogBackgroundColor.ShowDialog();

            // get the selected color
            Color color = Color.FromArgb((int)colorDialogBackgroundColor.Color.R,
                (int)colorDialogBackgroundColor.Color.G, (int)colorDialogBackgroundColor.Color.B);

            this.backgroundColor = color;
            toolStripSplitButtonBackgroundColor.Image = CreateBackgroundImgPicture();
        }

        /// <summary>
        /// Change current selection to super script
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonSuperScript_Click(object sender, EventArgs e)
        {
            SourceGrid.RangeRegion region = grid1.Selection.GetSelectionRegion();
            SourceGrid.PositionCollection poss = region.GetCellsPositions();
            for (int i = 0; i < poss.Count; ++i)
            {
                getRichTextBoxCell(grid1, poss[i]).SelectionSuperScript();
            }
        }

        /// <summary>
        /// Change current selection to normal script
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonNormalScript_Click(object sender, EventArgs e)
        {
            SourceGrid.RangeRegion region = grid1.Selection.GetSelectionRegion();
            SourceGrid.PositionCollection poss = region.GetCellsPositions();
            for (int i = 0; i < poss.Count; ++i)
            {
                getRichTextBoxCell(grid1, poss[i]).SelectionNormalScript();
            }
        }

        /// <summary>
        /// Change current selection to sub script
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonSubScript_Click(object sender, EventArgs e)
        {
            SourceGrid.RangeRegion region = grid1.Selection.GetSelectionRegion();
            SourceGrid.PositionCollection poss = region.GetCellsPositions();
            for (int i = 0; i < poss.Count; ++i)
            {
                getRichTextBoxCell(grid1, poss[i]).SelectionSubScript();
            }
        }

        /// <summary>
        /// Align current selection to the left
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonLeft_Click(object sender, EventArgs e)
        {
            SourceGrid.RangeRegion region = grid1.Selection.GetSelectionRegion();
            SourceGrid.PositionCollection poss = region.GetCellsPositions();
            for (int i = 0; i < poss.Count; ++i)
            {
                SourceGrid.Cells.Cell cell = grid1.GetCell(poss[i]) as SourceGrid.Cells.Cell;
                if (cell != null)
                {
                    cell.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
                    grid1.PerformLayout();
                }
            }
        }

        /// <summary>
        /// Align current selection to the center
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonCenter_Click(object sender, EventArgs e)
        {
            SourceGrid.RangeRegion region = grid1.Selection.GetSelectionRegion();
            SourceGrid.PositionCollection poss = region.GetCellsPositions();
            for (int i = 0; i < poss.Count; ++i)
            {
                SourceGrid.Cells.Cell cell = grid1.GetCell(poss[i]) as SourceGrid.Cells.Cell;
                if (cell != null)
                {
                    cell.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                    grid1.PerformLayout();
                }
            }
        }

        /// <summary>
        /// Align current selection to the right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonRight_Click(object sender, EventArgs e)
        {
            SourceGrid.RangeRegion region = grid1.Selection.GetSelectionRegion();
            SourceGrid.PositionCollection poss = region.GetCellsPositions();
            for (int i = 0; i < poss.Count; ++i)
            {
                SourceGrid.Cells.Cell cell = grid1.GetCell(poss[i]) as SourceGrid.Cells.Cell;
                if (cell != null)
                {
                    cell.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grid1.PerformLayout();
                }
            }
        }

        #endregion

    }
}
