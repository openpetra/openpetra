using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using DevAge.ComponentModel;
using DevAge;

namespace SourceGrid.Cells.Editors
{
    /// <summary>
    /// An editor that use a RichTextBoxTyped for editing support.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class RichTextBox : EditorControlBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public RichTextBox()
            : base(typeof(DevAge.Windows.Forms.RichText))
        {
            TypeConverter = new DevAge.ComponentModel.Converter.RichTextTypeConverter();
        }

        #endregion

        #region Edit Control
        /// <summary>
        /// Create the editor control
        /// </summary>
        /// <returns></returns>
        protected override Control CreateControl()
        {
            DevAge.Windows.Forms.DevAgeRichTextBox editor = new DevAge.Windows.Forms.DevAgeRichTextBox();
            editor.BorderStyle = BorderStyle.None;
            editor.AutoSize = false;
            editor.Validator = this;
            return editor;
        }

        /// <summary>
        /// Gets the control used for editing the cell.
        /// </summary>
        public new DevAge.Windows.Forms.DevAgeRichTextBox Control
        {
            get
            {
                return (DevAge.Windows.Forms.DevAgeRichTextBox)base.Control;
            }
        }

        /// <summary>
        /// This method is called just before the edit start.
        /// You can use this method to customize the editor with the cell informations.
        /// </summary>
        /// <param name="cellContext"></param>
        /// <param name="editorControl"></param>
        protected override void OnStartingEdit(CellContext cellContext, Control editorControl)
        {
            base.OnStartingEdit(cellContext, editorControl);

            DevAge.Windows.Forms.DevAgeRichTextBox l_RchTxtBox = (DevAge.Windows.Forms.DevAgeRichTextBox)editorControl;
            l_RchTxtBox.WordWrap = cellContext.Cell.View.WordWrap;

            // to set the scroll of the textbox to the initial position
            //(otherwise the richtextbox uses the previous scroll position)
            l_RchTxtBox.SelectionStart = 0;
            l_RchTxtBox.SelectionLength = 0;
        }

        /// <summary>
        /// Set the specified value in the current editor control.
        /// </summary>
        /// <param name="editValue"></param>
        public override void SetEditValue(object editValue)
        {
            Control.Value = editValue as DevAge.Windows.Forms.RichText;
            Control.SelectAll();
        }

        /// <summary>
        /// Returns the value inserted with the current editor control
        /// </summary>
        /// <returns></returns>
        public override object GetEditedValue()
        {
            return Control.Value;
        }

        /// <summary>
        /// Override content of cell with sent character
        /// </summary>
        /// <param name="key"></param>
        protected override void OnSendCharToEditor(char key)
        {
            Control.Text = key.ToString();
            if (Control.Text != null)
                Control.SelectionStart = Control.Text.Length;
        }

        #endregion
    }
}

