//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2012 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MSysMan;
using SourceGrid.Selection;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// manual methods for the generated window
    public partial class TUC_AppearancePreferences
    {
        private bool AppearanceChanged = false;
        private string ViewTasks = "Tiles";
        private int TaskSize = 1;
        private bool SingleClickExecution = false;

        private ColorDialog BackgroundColorDialog = new ColorDialog();
        private ColorDialog CellBackgroundColorDialog = new ColorDialog();
        private ColorDialog AlternateColorDialog = new ColorDialog();
        private ColorDialog GridlinesColorDialog = new ColorDialog();
        private ColorDialog SelectionColorDialog = new ColorDialog();
        private ColorDialog FilterColorDialog = new ColorDialog();
        private ColorDialog FindColorDialog = new ColorDialog();

        // original preferences
        private System.Drawing.Color OriginalBackgroundColour;
        private System.Drawing.Color OriginalCellBackgroundColour;
        private System.Drawing.Color AlternateColour;
        private System.Drawing.Color GridlinesColour;
        private System.Drawing.Color SelectionColour;
        private int SelectionAlpha;
        private System.Drawing.Color FilterColour;
        private System.Drawing.Color FindColour;

        private void InitializeManualCode()
        {
            if (TUserDefaults.GetStringDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_VIEWTASKS, "Tiles") == "List")
            {
                rbtList.Checked = true;
                ViewTasks = "List";
            }

            TaskSize = TUserDefaults.GetInt16Default(TUserDefaults.MAINMENU_VIEWOPTIONS_TILESIZE, 2);

            if (TaskSize == 2)
            {
                rbtMedium.Checked = true;
            }
            else if (TaskSize == 3)
            {
                rbtSmall.Checked = true;
            }

            if (TUserDefaults.GetBooleanDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_SINGLECLICKEXECUTION, false) == true)
            {
                chkSingleClickExecution.Checked = true;
                SingleClickExecution = true;
            }

            // get user's current prefered grid colours
            OriginalBackgroundColour = System.Drawing.ColorTranslator.FromHtml(
                TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_BACKGROUND,
                    System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.White)));
            OriginalCellBackgroundColour = System.Drawing.ColorTranslator.FromHtml(
                TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_CELLBACKGROUND,
                    System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.White)));
            AlternateColour = System.Drawing.ColorTranslator.FromHtml(
                TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_ALTERNATE,
                    System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(230, 230, 230))));
            GridlinesColour = System.Drawing.ColorTranslator.FromHtml(
                TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_GRIDLINES,
                    System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(211, 211, 211))));
            FilterColour = System.Drawing.ColorTranslator.FromHtml(
                TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_FILTER_PANEL,
                    System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightBlue)));
            FindColour = System.Drawing.ColorTranslator.FromHtml(
                TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_FIND_PANEL,
                    System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.BurlyWood)));

            BackgroundColorDialog.Color = OriginalBackgroundColour;
            CellBackgroundColorDialog.Color = OriginalCellBackgroundColour;
            AlternateColorDialog.Color = AlternateColour;
            GridlinesColorDialog.Color = GridlinesColour;
            FilterColorDialog.Color = FilterColour;
            FindColorDialog.Color = FindColour;

            string SelectionColourUserDefault;

            // The UserDefault for the Selection colour stores a decimal Alpha value appended to the HTML representation of the colour
            // because the Selection needs to be transparent to a certain degree in order to let the data of a selected Grid Row shine through!
            // Example: "#00FFAA;50": A=140 (decimal 140), R=15 (hex 0F), G=255 (hex FF), B=170 (hex AA)
            SelectionColourUserDefault = TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_SELECTION, String.Empty);

            if (SelectionColourUserDefault.Length > 0)
            {
                SelectionColour = System.Drawing.ColorTranslator.FromHtml(SelectionColourUserDefault.Split(';')[0]);
                SelectionAlpha = Convert.ToInt32(SelectionColourUserDefault.Split(';')[1]);
            }
            else
            {
                // No UserDefault for the Selection in the DB; use a hard-coded default
                SelectionColour = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Highlight);
                SelectionAlpha = 120;
            }

            SelectionColorDialog.Color = SelectionColour;
            nudAlpha.Maximum = 255;
            nudAlpha.Value = 255 - SelectionAlpha;

            SetButtonColours();

            // create DataTable and DataRows for an example grid
            DataTable ExampleTable = new DataTable("Example");

            DataColumn Column1 = new DataColumn("Column1");
            DataColumn Column2 = new DataColumn("Column2");

            ExampleTable.Columns.Add(Column1);
            ExampleTable.Columns.Add(Column2);

            grdExample.AddTextColumn(Catalog.GetString("Column1"), Column1);
            grdExample.AddTextColumn(Catalog.GetString("Column2"), Column2);

            DataRow ExampleRow1 = ExampleTable.NewRow();
            DataRow ExampleRow2 = ExampleTable.NewRow();
            DataRow ExampleRow3 = ExampleTable.NewRow();
            DataRow ExampleRow4 = ExampleTable.NewRow();
            DataRow ExampleRow5 = ExampleTable.NewRow();
            ExampleRow1[0] = "Row 1";
            ExampleRow1[1] = "Row 1";
            ExampleRow2[0] = "Row 2";
            ExampleRow2[1] = "Row 2";
            ExampleRow3[0] = "Row 3";
            ExampleRow3[1] = "Row 3";
            ExampleRow4[0] = "Row 4";
            ExampleRow4[1] = "Row 4";
            ExampleRow5[0] = "Row 5";
            ExampleRow5[1] = "Row 5";
            ExampleTable.Rows.Add(ExampleRow1);
            ExampleTable.Rows.Add(ExampleRow2);
            ExampleTable.Rows.Add(ExampleRow3);
            ExampleTable.Rows.Add(ExampleRow4);
            ExampleTable.Rows.Add(ExampleRow5);

            DataView MyDataView = ExampleTable.DefaultView;
            MyDataView.AllowNew = false;
            grdExample.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);
        }

        // refresh button background colours
        private void SetButtonColours()
        {
            btnBackground.BackColor = BackgroundColorDialog.Color;
            btnCellBackground.BackColor = CellBackgroundColorDialog.Color;
            btnAlternate.BackColor = AlternateColorDialog.Color;
            btnGridlines.BackColor = GridlinesColorDialog.Color;
            btnSelection.BackColor = SelectionColorDialog.Color;
            btnFilter.BackColor = FilterColorDialog.Color;
            btnFind.BackColor = FindColorDialog.Color;
        }

        /// <summary>
        /// Gets the data from all UserControls on this TabControl.
        /// </summary>
        /// <returns>void</returns>
        public void GetDataFromControls()
        {
        }

        /// <summary>
        /// Saves any changed preferences to s_user_defaults
        /// </summary>
        /// <returns>void</returns>
        public bool SaveAppearanceTab()
        {
            // save any changes to task view
            if (rbtTiles.Checked && (ViewTasks != "Tiles"))
            {
                TUserDefaults.SetDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_VIEWTASKS, "Tiles");
                AppearanceChanged = true;
            }
            else if (rbtList.Checked && (ViewTasks != "List"))
            {
                TUserDefaults.SetDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_VIEWTASKS, "List");
                AppearanceChanged = true;
            }

            // change any changes to task size
            if (rbtLarge.Checked && (TaskSize != 1))
            {
                TUserDefaults.SetDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_TILESIZE, 1);
                AppearanceChanged = true;
            }
            else if (rbtMedium.Checked && (TaskSize != 2))
            {
                TUserDefaults.SetDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_TILESIZE, 2);
                AppearanceChanged = true;
            }
            else if (rbtSmall.Checked && (TaskSize != 3))
            {
                TUserDefaults.SetDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_TILESIZE, 3);
                AppearanceChanged = true;
            }

            // save any changes to single click execution
            if (chkSingleClickExecution.Checked && (SingleClickExecution != true))
            {
                TUserDefaults.SetDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_SINGLECLICKEXECUTION, true);
                AppearanceChanged = true;
            }
            else if ((chkSingleClickExecution.Checked == false) && (SingleClickExecution != false))
            {
                TUserDefaults.SetDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_SINGLECLICKEXECUTION, false);
                AppearanceChanged = true;
            }

            // save any changes to grid colours
            if (OriginalBackgroundColour != BackgroundColorDialog.Color)
            {
                String HtmlColor = System.Drawing.ColorTranslator.ToHtml(BackgroundColorDialog.Color);
                TUserDefaults.SetDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_BACKGROUND, HtmlColor);

                TSgrdDataGrid.ColourInfoSetup = false;
            }

            if (OriginalCellBackgroundColour != CellBackgroundColorDialog.Color)
            {
                String HtmlColor = System.Drawing.ColorTranslator.ToHtml(CellBackgroundColorDialog.Color);
                TUserDefaults.SetDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_CELLBACKGROUND, HtmlColor);

                TSgrdDataGrid.ColourInfoSetup = false;
            }

            if (AlternateColour != AlternateColorDialog.Color)
            {
                String HtmlColor = System.Drawing.ColorTranslator.ToHtml(AlternateColorDialog.Color);
                TUserDefaults.SetDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_ALTERNATE, HtmlColor);

                TSgrdDataGrid.ColourInfoSetup = false;
            }

            if (GridlinesColour != GridlinesColorDialog.Color)
            {
                String HtmlColor = System.Drawing.ColorTranslator.ToHtml(GridlinesColorDialog.Color);
                TUserDefaults.SetDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_GRIDLINES, HtmlColor);

                TSgrdDataGrid.ColourInfoSetup = false;
            }

            if ((SelectionColour != SelectionColorDialog.Color) || (SelectionAlpha != nudAlpha.Value))
            {
                String HtmlColorAndAlpha = System.Drawing.ColorTranslator.ToHtml(SelectionColorDialog.Color) + ";" +
                                           (255 - Convert.ToInt32(nudAlpha.Value));
                TUserDefaults.SetDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_SELECTION, HtmlColorAndAlpha);

                TSgrdDataGrid.ColourInfoSetup = false;
            }

            if (FilterColour != FilterColorDialog.Color)
            {
                String HtmlColor = System.Drawing.ColorTranslator.ToHtml(FilterColorDialog.Color);
                TUserDefaults.SetDefault(TUserDefaults.NamedDefaults.COLOUR_FILTER_PANEL, HtmlColor);

                TUcoFilterAndFind.ColourInfoSetup = false;
            }

            if (FindColour != FindColorDialog.Color)
            {
                String HtmlColor = System.Drawing.ColorTranslator.ToHtml(FindColorDialog.Color);
                TUserDefaults.SetDefault(TUserDefaults.NamedDefaults.COLOUR_FIND_PANEL, HtmlColor);

                TUcoFilterAndFind.ColourInfoSetup = false;
            }

            return AppearanceChanged;
        }

        private void OnBtnBackground(Object sender, EventArgs e)
        {
            // Show the color dialog.
            DialogResult Result = BackgroundColorDialog.ShowDialog();

            // See if user pressed ok.
            if (Result == DialogResult.OK)
            {
                SetButtonColours();
                grdExample.BackColor = BackgroundColorDialog.Color;
            }
        }

        private void OnBtnCellBackground(Object sender, EventArgs e)
        {
            // Show the color dialog.
            DialogResult Result = CellBackgroundColorDialog.ShowDialog();

            // See if user pressed ok.
            if (Result == DialogResult.OK)
            {
                SetButtonColours();
                grdExample.CellBackgroundColour = CellBackgroundColorDialog.Color;
            }
        }

        private void OnBtnAlternate(Object sender, EventArgs e)
        {
            // Show the color dialog.
            DialogResult Result = AlternateColorDialog.ShowDialog();

            // See if user pressed ok.
            if (Result == DialogResult.OK)
            {
                SetButtonColours();
                grdExample.AlternatingBackgroundColour = AlternateColorDialog.Color;
            }
        }

        private void OnBtnGridlines(Object sender, EventArgs e)
        {
            // Show the color dialog.
            DialogResult Result = GridlinesColorDialog.ShowDialog();

            // See if user pressed ok.
            if (Result == DialogResult.OK)
            {
                SetButtonColours();
                grdExample.GridLinesColour = GridlinesColorDialog.Color;
            }
        }

        private void OnBtnSelection(Object sender, EventArgs e)
        {
            // Show the color dialog.
            DialogResult Result = SelectionColorDialog.ShowDialog();

            // See if user pressed ok.
            if (Result == DialogResult.OK)
            {
                SetButtonColours();
                ((SelectionBase)grdExample.Selection).BackColor = System.Drawing.Color.FromArgb(255 - Convert.ToInt32(
                        nudAlpha.Value), SelectionColorDialog.Color);
                ((SelectionBase)grdExample.Selection).FocusBackColor = ((SelectionBase)grdExample.Selection).BackColor;
            }
        }

        private void OnNudAlpha(Object sender, EventArgs e)
        {
            ((SelectionBase)grdExample.Selection).BackColor = System.Drawing.Color.FromArgb(255 - Convert.ToInt32(
                    nudAlpha.Value), SelectionColorDialog.Color);
            ((SelectionBase)grdExample.Selection).FocusBackColor = ((SelectionBase)grdExample.Selection).BackColor;
        }

        // restore grid colours to default values
        private void OnBtnRestore(Object sender, EventArgs e)
        {
            BackgroundColorDialog.Color = System.Drawing.Color.White;
            CellBackgroundColorDialog.Color = System.Drawing.Color.White;
            AlternateColorDialog.Color = System.Drawing.Color.FromArgb(230, 230, 230);
            GridlinesColorDialog.Color = System.Drawing.Color.FromArgb(211, 211, 211);
            SelectionColorDialog.Color = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Highlight);
            nudAlpha.Value = 120;

            grdExample.BackColor = BackgroundColorDialog.Color;
            grdExample.CellBackgroundColour = CellBackgroundColorDialog.Color;
            grdExample.AlternatingBackgroundColour = AlternateColorDialog.Color;
            grdExample.GridLinesColour = GridlinesColorDialog.Color;
            ((SelectionBase)grdExample.Selection).BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(
                    nudAlpha.Value), SelectionColorDialog.Color);
            ((SelectionBase)grdExample.Selection).FocusBackColor = ((SelectionBase)grdExample.Selection).BackColor;

            SetButtonColours();
        }

        private void OnBtnFilter(Object sender, EventArgs e)
        {
            // Show the color dialog.
            DialogResult Result = FilterColorDialog.ShowDialog();

            // See if user pressed ok.
            if (Result == DialogResult.OK)
            {
                SetButtonColours();
            }
        }

        private void OnBtnFind(Object sender, EventArgs e)
        {
            // Show the color dialog.
            DialogResult Result = FindColorDialog.ShowDialog();

            // See if user pressed ok.
            if (Result == DialogResult.OK)
            {
                SetButtonColours();
            }
        }

        // restore Filter and Find colours to default values
        private void OnBtnRestoreFilterFind(Object sender, EventArgs e)
        {
            FilterColorDialog.Color = System.Drawing.Color.LightBlue;
            FindColorDialog.Color = System.Drawing.Color.BurlyWood;

            SetButtonColours();
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing
        /// to another record, otherwise set it to false.</param>
        /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
        /// user, otherwise set it to false.</param>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        public bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors)
        {
            return true;
        }
    }
}