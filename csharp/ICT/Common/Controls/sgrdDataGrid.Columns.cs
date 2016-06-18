//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevAge.Drawing;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.Models;
using SourceGrid.Cells.Views;

namespace Ict.Common.Controls
{
    /// <summary>Delegate forward declaration</summary>
    public delegate System.Drawing.Image DelegateGetImageForRow(int ARow);

    #region TextColumn

    /// <summary>
    /// This is a custom DataGridColumn for Text, for use with TSgrdDataGrid.
    /// </summary>
    public class TSgrdTextColumn : SourceGrid.DataGridColumn
    {
        /// <summary>
        /// the grid that this column belongs to
        /// </summary>
        protected SourceGrid.DataGrid FGrid;

        /// <summary>
        /// the currently selected cell
        /// </summary>
        protected SourceGrid.Cells.ICellVirtual FDataCellSelected;

        private bool FBoldRightBorder;

        /// <summary>
        /// Set the right border of a cell to bold
        /// </summary>
        public bool BoldRightBorder
        {
            set
            {
                FBoldRightBorder = value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ADataGrid"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="ACaption"></param>
        /// <param name="ADataCell"></param>
        public TSgrdTextColumn(SourceGrid.DataGrid ADataGrid,
            System.Data.DataColumn ADataColumn,
            string ACaption,
            SourceGrid.Cells.ICellVirtual ADataCell) :
            this(ADataGrid, ADataColumn, ACaption, ADataCell, -1, true)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ADataGrid"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="ACaption"></param>
        /// <param name="ADataCell"></param>
        /// <param name="ASortableHeader"></param>
        public TSgrdTextColumn(SourceGrid.DataGrid ADataGrid,
            System.Data.DataColumn ADataColumn,
            string ACaption,
            SourceGrid.Cells.ICellVirtual ADataCell,
            Boolean ASortableHeader) :
            this(ADataGrid, ADataColumn, ACaption, ADataCell, -1, ASortableHeader)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ADataGrid"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="ACaption"></param>
        /// <param name="ADataCell"></param>
        /// <param name="AColumnWidth"></param>
        /// <param name="ASortableHeader"></param>
        public TSgrdTextColumn(SourceGrid.DataGrid ADataGrid,
            System.Data.DataColumn ADataColumn,
            string ACaption,
            SourceGrid.Cells.ICellVirtual ADataCell,
            Int16 AColumnWidth,
            Boolean ASortableHeader) :
            base(ADataGrid)
        {
            HeaderCell = new SourceGrid.Cells.ColumnHeader(ACaption);

            if (!ASortableHeader)
            {
                ((SourceGrid.Cells.ColumnHeader)HeaderCell).AutomaticSortEnabled = false;
            }

            HeaderCell.View = ((TSgrdDataGrid)ADataGrid).ColumnHeaderView;

            if (ADataColumn != null)
            {
                PropertyName = ADataColumn.ColumnName;
            }

            DataCell = ADataCell;

            if (AColumnWidth != -1)
            {
                Width = AColumnWidth;
                AutoSizeMode = SourceGrid.AutoSizeMode.MinimumSize;
            }

            FGrid = ADataGrid;
        }

        /// <summary>
        /// get the data cell of a row
        /// </summary>
        /// <param name="AGridRow"></param>
        /// <returns></returns>
        public override SourceGrid.Cells.ICellVirtual GetDataCell(int AGridRow)
        {
            SourceGrid.Cells.ICellVirtual ReturnValue;
            SourceGrid.Cells.ICellVirtual BaseDataCell = base.GetDataCell(AGridRow);
            SourceGrid.Cells.ICellVirtual AlternatingDataCellSelected;
            SourceGrid.Cells.ICellVirtual BoldBorderDataCellSelected;
            TSgrdDataGrid GridWrapper = (TSgrdDataGrid)FGrid;

            int Reminder;

            HeaderCell.View = ((TSgrdDataGrid)FGrid).ColumnHeaderView;
            FDataCellSelected = BaseDataCell.Copy();

            // Create a ToolTip
            FDataCellSelected.AddController(SourceGrid.Cells.Controllers.ToolTipText.Default);
            FDataCellSelected.Model.AddModel(TToolTipModel.myDefault);

            // Alternating BackColor (banding effect)
            Math.DivRem(AGridRow, 2, out Reminder);

            if (Reminder == 0)
            {
                FDataCellSelected.View.BackColor = ((TSgrdDataGrid)FGrid).CellBackgroundColour;
                FDataCellSelected.View.Border = new DevAge.Drawing.RectangleBorder(new BorderLine(GridWrapper.GridLinesColour, 0.5f));
                ReturnValue = FDataCellSelected;
            }
            else
            {
                if (((TSgrdDataGrid)FGrid).AlternatingBackgroundColour != Color.Empty)
                {
                    AlternatingDataCellSelected = FDataCellSelected.Copy();
                    AlternatingDataCellSelected.View = (SourceGrid.Cells.Views.IView)FDataCellSelected.View.Clone();
                    AlternatingDataCellSelected.View.BackColor = ((TSgrdDataGrid)FGrid).AlternatingBackgroundColour;
                    AlternatingDataCellSelected.View.Border = new DevAge.Drawing.RectangleBorder(new BorderLine(GridWrapper.GridLinesColour, 0.5f));
                    ReturnValue = AlternatingDataCellSelected;
                }
                else
                {
                    ReturnValue = FDataCellSelected;
                }
            }

            if (FBoldRightBorder)
            {
                BoldBorderDataCellSelected = ReturnValue.Copy();

                BoldBorderDataCellSelected.View.Border = new DevAge.Drawing.RectangleBorder(new BorderLine(GridWrapper.GridLinesColour, 0.5f),
                    new BorderLine(GridWrapper.GridLinesColour, 0.5f),
                    new BorderLine(GridWrapper.GridLinesColour, 0.5f),
                    new BorderLine(GridWrapper.GridLinesColour, 5f));

                ReturnValue = BoldBorderDataCellSelected;
            }

            return ReturnValue;
        }
    }
    #endregion

    #region ImageColumn

    /// <summary>
    /// This is a custom DataGridColumn for Images, for use with TSgrdDataGrid.
    /// </summary>
    public class TSgrdImageColumn : TSgrdTextColumn
    {
        private const Int32 IMAGECOLUMN_HORIZONTAL_PADDING = 4;
        private DelegateGetImageForRow FGetImage;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ADataGrid"></param>
        /// <param name="ACaption"></param>
        /// <param name="AGetImage"></param>
        public TSgrdImageColumn(SourceGrid.DataGrid ADataGrid, string ACaption, DelegateGetImageForRow AGetImage)
            : base(ADataGrid, null, ACaption, null, -1, false)
        {
            HeaderCell = new SourceGrid.Cells.ColumnHeader(ACaption);
            ((SourceGrid.Cells.ColumnHeader)HeaderCell).AutomaticSortEnabled = false;
            HeaderCell.View = ((TSgrdDataGrid)FGrid).ColumnHeaderView;
            PropertyName = null;
            DataCell = null;
            FGrid = ADataGrid;
            this.AutoSizeMode = SourceGrid.AutoSizeMode.MinimumSize;
            FGetImage = AGetImage;
        }

        /// <summary>
        /// get the data cell of a row
        /// </summary>
        /// <param name="AGridRow"></param>
        /// <returns></returns>
        public override SourceGrid.Cells.ICellVirtual GetDataCell(int AGridRow)
        {
            int Reminder;

            System.Drawing.Image TheImage;
            FDataCellSelected = new SourceGrid.Cells.Virtual.CellVirtual();

            // Create Icon
            TheImage = FGetImage(AGridRow - 1);
            FDataCellSelected.Model.AddModel(new SourceGrid.Cells.Models.Image(TheImage));
            ((ViewBase)FDataCellSelected.View).ImageAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            // Automatic calculation of column width
            Width = TheImage.Width + IMAGECOLUMN_HORIZONTAL_PADDING;

            // Create a ToolTip
            FDataCellSelected.AddController(SourceGrid.Cells.Controllers.ToolTipText.Default);
            FDataCellSelected.Model.AddModel(TToolTipModel.myDefault);

            // Alternating BackColor (banding effect)
            Math.DivRem(AGridRow, 2, out Reminder);

            if (Reminder != 0)
            {
                if (((TSgrdDataGrid)FGrid).AlternatingBackgroundColour != Color.Empty)
                {
                    FDataCellSelected.View = (SourceGrid.Cells.Views.IView)FDataCellSelected.View.Clone();
                    FDataCellSelected.View.BackColor = ((TSgrdDataGrid)FGrid).AlternatingBackgroundColour;
                }
            }

            return FDataCellSelected;
        }
    }
    #endregion

    #region BoundImage

    /// <summary>
    /// An interface for dealing with SourceGrid images in columns
    /// </summary>
    public interface IBoundImageEvaluator
    {
        /// <summary>
        /// This method gets called to determine whether the grid cell should have an image displayed or not
        /// </summary>
        /// <param name="AContext">The invocation context that was used in the <see cref="BoundGridImage"/> constructor</param>
        /// <param name="ADataRowView">The row containing the cell that will get the image</param>
        /// <returns>True if an image should be displayed</returns>
        bool EvaluateBoundImage(BoundGridImage.AnnotationContextEnum AContext, DataRowView ADataRowView);
    }

    /// <summary>
    /// Class that encapsulates code dealing with data bound images in grid cells
    /// </summary>
    public class BoundGridImage : IImage
    {
        /// <summary>
        /// Available bound images
        /// </summary>
        public enum DisplayImageEnum
        {
            /// <summary>
            /// The image for an inactive item
            /// </summary>
            Inactive
        };

        /// <summary>
        /// Available annotation contexts
        /// </summary>
        public enum AnnotationContextEnum
        {
            /// <summary> The Account Code context </summary>
            AccountCode,

            /// <summary> The Coste Centre context </summary>
            CostCentreCode,

            /// <summary> The Analaysis Type context </summary>
            AnalysisTypeCode,

            /// <summary> The Ananlysis Attribute Value context </summary>
            AnalysisAttributeValue
        };

        // Local copies of constructor parameters
        private IBoundImageEvaluator FCallerForm;
        private AnnotationContextEnum FCallerContext;
        private DisplayImageEnum FDisplayImageEnum;

        // The actual image to display
        private System.Drawing.Image FDisplayImage = null;

        /// <summary>
        /// Main Constructor
        /// </summary>
        /// <param name="ACallerForm">A reference to the caller form.  Usually use 'this'.  It must implement the <see cref="IBoundImageEvaluator"/> interface.</param>
        /// <param name="AContext">A string that identifies the column context.  Can be empty text if the grid only has one column with an image.</param>
        /// <param name="AImageEnum">One of the available images that can be displayed.</param>
        public BoundGridImage(IBoundImageEvaluator ACallerForm, AnnotationContextEnum AContext, DisplayImageEnum AImageEnum)
        {
            FCallerForm = ACallerForm;
            FCallerContext = AContext;
            FDisplayImageEnum = AImageEnum;

            string path = TAppSettingsManager.GetValue("Resource.Dir", string.Empty, true);

            if (path.Length > 0)
            {
                string filename = string.Empty;

                try
                {
                    switch (FDisplayImageEnum)
                    {
                        // At present we only have the Inactive triangle but if there are more images later add them to this switch statement

                        case DisplayImageEnum.Inactive:
                            filename = "RedTriangle.png";
                            break;
                    }

                    FDisplayImage = System.Drawing.Image.FromFile(Path.Combine(path, filename));
                }
                catch (FileNotFoundException)
                {
                    TLogging.Log(string.Format("Could not find the file {0} in folder {1}", filename, path));
                }
                catch (Exception ex)
                {
                    TLogging.Log(string.Format("Error opening resource file {0}: {1}", filename, ex.Message));
                }
            }
        }

        /// <summary>
        /// Implementation of the required IImage method
        /// </summary>
        /// <param name="cellContext">The cell for which an image is desired</param>
        /// <returns>The image, or null for no image</returns>
        public System.Drawing.Image GetImage(CellContext cellContext)
        {
            SourceGrid.DataGrid grid = (SourceGrid.DataGrid)cellContext.Grid;
            DataRowView row = (DataRowView)grid.Rows.IndexToDataSourceRow(cellContext.Position.Row);

            if (FCallerForm.EvaluateBoundImage(FCallerContext, row))
            {
                // The caller wants the image to be displayed
                return FDisplayImage;
            }

            // No image displayed
            return null;
        }
    }

    #endregion

    #region TToolTipModel

    /// <summary>
    /// tooltips for grid
    /// </summary>
    public class TToolTipModel : System.Object, SourceGrid.Cells.Models.IToolTipText
    {
        /// default tooltip
        public static TToolTipModel myDefault;

        /// <summary>
        /// this needs to be called once for the whole application
        /// </summary>
        public static void InitializeUnit()
        {
            // Initialisation of Unit needs to be done once.
            if (TToolTipModel.myDefault == null)
            {
                TToolTipModel.myDefault = new TToolTipModel();
            }
        }

        /// <summary>
        /// get the correct tooltip for the current cell
        /// </summary>
        /// <param name="ACellContext"></param>
        /// <returns>tooltip text</returns>
        public string GetToolTipText(SourceGrid.CellContext ACellContext)
        {
            string ReturnValue = "";
            DataRowView TheRow;
            TSgrdDataGrid TheGrid;

            TheGrid = (TSgrdDataGrid)(SourceGrid.DataGrid)ACellContext.Grid;
            TheRow = (DataRowView)TheGrid.Rows.IndexToDataSourceRow(ACellContext.Position.Row);

            // MessageBox.Show('TToolTipModel.GetToolTipText.  Row: ' + Convert.ToString(ACellContext.Position.Row  1) );
            if (TheRow != null)
            {
                try
                {
                    // Create a ToolTip
                    if (TheGrid.ToolTipTextDelegate != null)
                    {
                        // MessageBox.Show('TToolTipModel.GetToolTipText.  Inquiring ToolTip Text...');
                        ReturnValue = TheGrid.ToolTipTextDelegate((short)ACellContext.Position.Column, (short)(ACellContext.Position.Row - 1));

//                      MessageBox.Show("TToolTipModel.GetToolTipText.  ToolTip Text: " + ReturnValue);
                        return ReturnValue;
                    }
                }
                catch (Exception E)
                {
                    MessageBox.Show("Exception in TToolTipModel.GetToolTipText: " + E.ToString());
                }
            }
            else
            {
                ReturnValue = "";
            }

            return ReturnValue;
        }
    }

    /// <summary>
    /// Class that handles tool tips for cells with a bound annotation image
    /// </summary>
    public class BoundImageToolTipModel : SourceGrid.Cells.Models.IToolTipText
    {
        // Local copies of constructor parameters
        private IBoundImageEvaluator FCallerForm;
        private BoundGridImage.AnnotationContextEnum FCallerContext;
        private BoundGridImage.DisplayImageEnum FImageEnum;

        /// <summary>
        /// Main Constructor
        /// </summary>
        /// <param name="ACallerForm">A reference to the caller form.  Usually use 'this'.  It must implement the <see cref="IBoundImageEvaluator"/> interface.</param>
        /// <param name="AContext">A string that identifies the column context.  Can be empty text if the grid only has one column with an image.</param>
        /// <param name="AImageEnum">One of the available images that can be displayed.</param>
        public BoundImageToolTipModel(IBoundImageEvaluator ACallerForm,
            BoundGridImage.AnnotationContextEnum AContext,
            BoundGridImage.DisplayImageEnum AImageEnum)
        {
            FCallerForm = ACallerForm;
            FCallerContext = AContext;
            FImageEnum = AImageEnum;
        }

        /// <summary>
        /// Implementation of the required IToolTipText method
        /// </summary>
        /// <param name="cellContext">The cell for which an tool tip is desired</param>
        /// <returns>The text, which may be an empty string</returns>
        public string GetToolTipText(SourceGrid.CellContext cellContext)
        {
            SourceGrid.DataGrid grid = (SourceGrid.DataGrid)cellContext.Grid;
            DataRowView row = (DataRowView)grid.Rows.IndexToDataSourceRow(cellContext.Position.Row);

            if (row != null)
            {
                if (FCallerForm.EvaluateBoundImage(FCallerContext, row))
                {
                    // There is an image in this row/column
                    if (FImageEnum == BoundGridImage.DisplayImageEnum.Inactive)
                    {
                        if (FCallerContext == BoundGridImage.AnnotationContextEnum.CostCentreCode)
                        {
                            return Catalog.GetString("This Cost Centre code is no longer active");
                        }
                        else if (FCallerContext == BoundGridImage.AnnotationContextEnum.AccountCode)
                        {
                            return Catalog.GetString("This Bank Account code is no longer active");
                        }
                        else if (FCallerContext == BoundGridImage.AnnotationContextEnum.AnalysisTypeCode)
                        {
                            return Catalog.GetString("This Analysis Attribute type is no longer active");
                        }
                        else if (FCallerContext == BoundGridImage.AnnotationContextEnum.AnalysisAttributeValue)
                        {
                            return Catalog.GetString("This Analysis Attribute value is no longer active");
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
    #endregion
}