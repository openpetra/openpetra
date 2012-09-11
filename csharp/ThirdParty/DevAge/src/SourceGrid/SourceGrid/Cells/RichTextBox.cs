using System;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Cells.Virtual
{
    /// <summary>
    /// A Cell with a RichTextBox. This Cell is of type RichText.
    /// Abstract, you must override GetValue and SetValue.
    /// </summary>
    public class RichTextBox : CellVirtual
    {
        /// <summary>
        /// Constructor of a RichTextBox style cell. You must st a valid Model to use this type of cell with this constructor.
        /// </summary>
        public RichTextBox()
        {
            View = Views.RichTextBox.Default;
            Model.AddModel(new Models.RichTextBox());
            AddController(Controllers.RichTextBox.Default);
            Editor = new Editors.RichTextBox();
        }
    }
}

namespace SourceGrid.Cells
{
    /// <summary>
    /// A Cell with a RichTextBox. This Cell is of type string.
    /// View: Views.RichTextBox.Default 
    /// Model: Models.RichTextBox 
    /// Controllers: Controllers.RichTextBox.Default
    /// </summary>
    public class RichTextBox : Cell
    {
        #region Constructor

        /// <summary>
        /// Default constrcutor
        /// </summary>
        public RichTextBox()
            : this(null)
        {
        }

        /// <summary>
        /// Value constrcutor
        /// </summary>
        public RichTextBox(DevAge.Windows.Forms.RichText value)
            : base(value)
        {
            View = new Views.RichTextBox();
            Model.AddModel(new Models.RichTextBox());
            AddController(Controllers.RichTextBox.Default);
            Editor = new Editors.RichTextBox();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get RichTextBoxModel
        /// </summary>
        private Models.RichTextBox RichTextBoxModel
        {
            get { return (Models.RichTextBox)Model.FindModel(typeof(Models.RichTextBox)); }
        }


        /// <summary>
        /// Font of current selection
        /// </summary>
        public Font SelectionFont
        {
            get { return RichTextBoxModel.GetSelectionFont(GetContext()); }
            set { RichTextBoxModel.SetSelectionFont(GetContext(), value); }
        }

        /// <summary>
        /// Color of current selection
        /// </summary>
        public Color SelectionColor
        {
            get { return RichTextBoxModel.GetSelectionColor(GetContext()); }
            set { RichTextBoxModel.SetSelectionColor(GetContext(), value); }
        }

        /// <summary>
        /// CharOffset of current selection
        /// </summary>
        public int SelectionCharOffset
        {
            get { return RichTextBoxModel.GetSelectionCharOffset(GetContext()); }
            set { RichTextBoxModel.SetSelectionCharOffset(GetContext(), value); }
        }

        /// <summary>
        /// Alignment of current selection
        /// </summary>
        public HorizontalAlignment SelectionAlignment
        {
            get { return RichTextBoxModel.GetSelectionAlignment(GetContext()); }
            set { RichTextBoxModel.SetSelectionAlignment(GetContext(), value); }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Change SelectionFont to bold respectively not bold if already set.
        /// </summary>
        public void SelectionBold()
        {
            SelectionFont = new Font(SelectionFont, SelectionFont.Style ^ FontStyle.Bold);
        }

        /// <summary>
        /// Change SelectionFont to italic respectively not italic if already set.
        /// </summary>
        public void SelectionItalic()
        {
            SelectionFont = new Font(SelectionFont, SelectionFont.Style ^ FontStyle.Italic);
        }

        /// <summary>
        /// Change SelectionFont to underline respectively not underline if already set.
        /// </summary>
        public void SelectionUnderline()
        {
            SelectionFont = new Font(SelectionFont, SelectionFont.Style ^ FontStyle.Underline);
        }

        /// <summary>
        /// Insert a string at the selection
        /// </summary>
        /// <param name="s">String to insert</param>
        public void InsertString(string s)
        {
            RichTextBoxModel.InsertString(GetContext(), s);
        }

        /// <summary>
        /// Change SelectionCharOffset to OFFSET respectively 0 if already set.
        /// </summary>
        public void SelectionSuperScript()
        {
            //const int OFFSET = 2;
            //if (SelectionCharOffset != 0)
            //{
            //    SelectionCharOffset = 0;
            //    SelectionFont = new Font(SelectionFont.FontFamily, SelectionFont.Size + OFFSET,
            //        SelectionFont.Style);
            //}
            //else
            //{
            //    SelectionCharOffset = OFFSET;
            //    SelectionFont = new Font(SelectionFont.FontFamily, SelectionFont.Size - OFFSET,
            //        SelectionFont.Style);
            //}
            RichTextBoxModel.SetSelectionEffect(GetContext(), DevAge.Windows.Forms.EffectType.Superscript);
        }

        /// <summary>
        /// Change SelectionEffectType to normal.
        /// </summary>
        public void SelectionNormalScript()
        {
            RichTextBoxModel.SetSelectionEffect(GetContext(), DevAge.Windows.Forms.EffectType.Normal);
        }

        /// <summary>
        /// Change SelectionCharOffset to OFFSET respectively 0 if already set.
        /// </summary>
        public void SelectionSubScript()
        {
            //const int OFFSET = -2;
            //if (SelectionCharOffset != 0)
            //{
            //    SelectionCharOffset = 0;
            //    SelectionFont = new Font(SelectionFont.FontFamily, SelectionFont.Size - OFFSET,
            //        SelectionFont.Style);
            //}
            //else
            //{
            //    SelectionCharOffset = OFFSET;
            //    SelectionFont = new Font(SelectionFont.FontFamily, SelectionFont.Size + OFFSET,
            //        SelectionFont.Style);
            //}
            RichTextBoxModel.SetSelectionEffect(GetContext(), DevAge.Windows.Forms.EffectType.Subscript);
        }

        /// <summary>
        /// Change SelectionCharOffset to OFFSET respectively 0 if already set.
        /// </summary>
        public void SelectionNormal()
        {
            RichTextBoxModel.SetSelectionEffect(GetContext(), DevAge.Windows.Forms.EffectType.Normal);
        }

        #endregion
    }
}
