using System;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Cells.Views
{
	/// <summary>
	/// Summary description for a 3D themed Button.
	/// </summary>
	[Serializable]
	public class Button : Cell
	{
		/// <summary>
		/// Represents a Button with the ability to draw an Image. Disable also the selection border using the OwnerDrawSelectionBorder = true.
		/// </summary>
		public new readonly static Button Default;

		#region Constructors

		static Button()
		{
			Default = new Button();
		}

		/// <summary>
		/// Use default setting
		/// </summary>
		public Button()
		{
            Background = new DevAge.Drawing.VisualElements.ButtonThemed();
		}

		/// <summary>
		/// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <param name="p_Source"></param>
		public Button(Button p_Source):base(p_Source)
		{
            Background = (DevAge.Drawing.VisualElements.IButton)p_Source.Background.Clone();
		}
		#endregion

		#region Clone
		/// <summary>
		/// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			return new Button(this);
		}
		#endregion

        #region Visual Elements

        public new DevAge.Drawing.VisualElements.IButton Background
        {
            get { return (DevAge.Drawing.VisualElements.IButton)base.Background; }
            set { base.Background = value; }
        }

        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);

            if (context.CellRange.Contains(context.Grid.MouseDownPosition))
                Background.Style = DevAge.Drawing.ButtonStyle.Pressed;
            else if (context.CellRange.Contains(context.Grid.MouseCellPosition))
                Background.Style = DevAge.Drawing.ButtonStyle.Hot;
            else if (context.CellRange.Contains(context.Grid.Selection.ActivePosition))
                Background.Style = DevAge.Drawing.ButtonStyle.Focus;
            else
                Background.Style = DevAge.Drawing.ButtonStyle.Normal;
        }
        #endregion
	}
}
