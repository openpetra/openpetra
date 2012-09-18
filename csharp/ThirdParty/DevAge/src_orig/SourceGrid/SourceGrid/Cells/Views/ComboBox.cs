using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGrid.Cells.Views
{
    public class ComboBox : Cell
    {
        /// <summary>
        /// Represents a default CheckBox with the CheckBox image align to the Middle Center of the cell. You must use this VisualModel with a Cell of type ICellCheckBox.
        /// </summary>
        public new readonly static ComboBox Default = new ComboBox();

        #region Constructors

        static ComboBox()
        {
        }

        /// <summary>
        /// Use default setting and construct a read and write VisualProperties
        /// </summary>
        public ComboBox()
        {
            ElementDropDown.AnchorArea = new DevAge.Drawing.AnchorArea(float.NaN, 0, 0, 0, false, false);
        }

        /// <summary>
        /// Copy constructor. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
        /// </summary>
        /// <param name="p_Source"></param>
        public ComboBox(ComboBox p_Source)
            : base(p_Source)
        {
            ElementDropDown = (DevAge.Drawing.VisualElements.IDropDownButton)p_Source.ElementDropDown.Clone();
        }
        #endregion

        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);

            PrepareVisualElementDropDown(context);
        }

        protected override IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetElements()
        {
            if (ElementDropDown != null)
                yield return ElementDropDown;

            foreach (DevAge.Drawing.VisualElements.IVisualElement v in GetBaseElements())
                yield return v;
        }
        private IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetBaseElements()
        {
            return base.GetElements();
        }

        private DevAge.Drawing.VisualElements.IDropDownButton mElementDropDown = new DevAge.Drawing.VisualElements.DropDownButtonThemed();
        /// <summary>
        /// Gets or sets the visual element used to draw the checkbox. Default is DevAge.Drawing.VisualElements.CheckBoxThemed.
        /// </summary>
        public DevAge.Drawing.VisualElements.IDropDownButton ElementDropDown
        {
            get { return mElementDropDown; }
            set { mElementDropDown = value; }
        }


        protected virtual void PrepareVisualElementDropDown(CellContext context)
        {
            if (context.CellRange.Contains(context.Grid.MouseCellPosition))
            {
                ElementDropDown.Style = DevAge.Drawing.ButtonStyle.Hot;
            }
            else
            {
                ElementDropDown.Style = DevAge.Drawing.ButtonStyle.Normal;
            }
        }

        #region Clone
        /// <summary>
        /// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new ComboBox(this);
        }
        #endregion
    }
}
