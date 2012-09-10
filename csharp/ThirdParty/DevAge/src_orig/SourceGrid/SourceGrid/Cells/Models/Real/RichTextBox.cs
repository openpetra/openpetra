using System;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Cells.Models
{


    public class RichTextBox : IRichTextBox
    {
        /// <summary>
        /// Insert a string at the selection
        /// </summary>
        /// <param name="cellContext"></param>
        /// <param name="s">String to insert</param>
        public void InsertString(CellContext cellContext, string s)
        {
            GetRichTextBoxControl(cellContext).SelectedText = s;
        }

        #region IRichTextBox Members

        /// <summary>
        /// Sets the effect of the selected text.
        /// </summary>
        public void SetSelectionEffect(CellContext cellContext, DevAge.Windows.Forms.EffectType effect)
        {
            ValueChangeEventArgs valArgs = new ValueChangeEventArgs(null, effect);

            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnValueChanging(cellContext, valArgs);
            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnValueChanged(cellContext, EventArgs.Empty);
        }

        /// <summary>
        /// Change the font of the selected text.
        /// </summary>
        /// <param name="cellContext"></param>
        /// <param name="font"></param>
        public void SetSelectionFont(CellContext cellContext, Font font)
        {
            ValueChangeEventArgs valArgs = new ValueChangeEventArgs(null, font);

            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnValueChanging(cellContext, valArgs);
            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnValueChanged(cellContext, EventArgs.Empty);
        }

        /// <summary>
        /// Get font of current selection
        /// </summary>
        /// <param name="cellContext"></param>
        /// <returns></returns>
        public Font GetSelectionFont(CellContext cellContext)
        {
            return GetRichTextBoxControl(cellContext).SelectionFont;
        }

        /// <summary>
        /// Change the font of the selected text.
        /// </summary>
        /// <param name="cellContext"></param>
        /// <param name="color"></param>
        public void SetSelectionColor(CellContext cellContext, Color color)
        {
            ValueChangeEventArgs valArgs = new ValueChangeEventArgs(null, color);

            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnValueChanging(cellContext, valArgs);
            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnValueChanged(cellContext, EventArgs.Empty);
        }

        /// <summary>
        /// Get font of current selection
        /// </summary>
        /// <param name="cellContext"></param>
        /// <returns></returns>
        public Color GetSelectionColor(CellContext cellContext)
        {
            return GetRichTextBoxControl(cellContext).SelectionColor;
        }

        /// <summary>
        /// Get char offset of current selection
        /// </summary>
        /// <param name="cellContext"></param>
        /// <returns></returns>
        public int GetSelectionCharOffset(CellContext cellContext)
        {
            return GetRichTextBoxControl(cellContext).SelectionCharOffset;
        }

        /// <summary>
        /// Change char offset of the selected text.
        /// </summary>
        /// <param name="cellContext"></param>
        /// <param name="charoffset"></param>
        public void SetSelectionCharOffset(CellContext cellContext, int charoffset)
        {
            ValueChangeEventArgs valArgs = new ValueChangeEventArgs(null, charoffset);

            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnValueChanging(cellContext, valArgs);
            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnValueChanged(cellContext, EventArgs.Empty);
        }

        /// <summary>
        /// Set horizontal alignment of selected text.
        /// </summary>
        /// <param name="cellContext"></param>
        /// <param name="horAlignment"></param>
        public void SetSelectionAlignment(CellContext cellContext, HorizontalAlignment horAlignment)
        {
            ValueChangeEventArgs valArgs = new ValueChangeEventArgs(null, horAlignment);

            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnValueChanging(cellContext, valArgs);
            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnValueChanged(cellContext, EventArgs.Empty);
        }

        /// <summary>
        /// Get horizontal alignment of current selection
        /// </summary>
        /// <param name="cellContext"></param>
        /// <returns></returns>
        public HorizontalAlignment GetSelectionAlignment(CellContext cellContext)
        {
            return GetRichTextBoxControl(cellContext).SelectionAlignment;
        }

        /// <summary>
        /// Get real RichTextBox Control
        /// </summary>
        /// <param name="cellContext"></param>
        /// <returns></returns>
        private DevAge.Windows.Forms.DevAgeRichTextBox GetRichTextBoxControl(CellContext cellContext)
        {
            if (cellContext.Cell != null)
            {
                Editors.RichTextBox editorRichTextBox = cellContext.Cell.Editor as Editors.RichTextBox;

                // if editor is not active, value needs to be assigned to prevent
                // using an old editor of another cell
                // as an editor can be used for more than one cell
                if (editorRichTextBox.EditCell == null)
                {
                    editorRichTextBox.Control.Value = cellContext.Value as DevAge.Windows.Forms.RichText;
                }

                return editorRichTextBox.Control;
            }

            return null;
        }

        #endregion
    }
}
