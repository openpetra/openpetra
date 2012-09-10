using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DevAge.Drawing.VisualElements
{
    [Serializable]
    public class CheckBoxThemed : CheckBoxBase
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public CheckBoxThemed()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public CheckBoxThemed(CheckBoxThemed other)
            : base(other)
        {
        }
        #endregion
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new CheckBoxThemed(this);
        }

        #region Properties
        /// <summary>
        /// Standard checkbox used when the XP style are disabled.
        /// </summary>
        private CheckBox mStandardCheckBox = new CheckBox();

        public override ControlDrawStyle Style
        {
            get { return base.Style; }
            set { base.Style = value; mStandardCheckBox.Style = value; }
        }

        public override CheckBoxState CheckBoxState
        {
            get{return base.CheckBoxState;}
            set { base.CheckBoxState = value; mStandardCheckBox.CheckBoxState = value; }
        }
        #endregion

        #region Helper methods
        protected VisualStyleElement GetBackgroundElement()
        {
            if (Style == ControlDrawStyle.Hot)
            {
                if (CheckBoxState == CheckBoxState.Checked)
                    return VisualStyleElement.Button.CheckBox.CheckedHot;
                else if (CheckBoxState == CheckBoxState.Unchecked)
                    return VisualStyleElement.Button.CheckBox.UncheckedHot;
                else
                    return VisualStyleElement.Button.CheckBox.MixedHot;
            }
            else if (Style == ControlDrawStyle.Pressed)
            {
                if (CheckBoxState == CheckBoxState.Checked)
                    return VisualStyleElement.Button.CheckBox.CheckedPressed;
                else if (CheckBoxState == CheckBoxState.Unchecked)
                    return VisualStyleElement.Button.CheckBox.UncheckedPressed;
                else
                    return VisualStyleElement.Button.CheckBox.MixedPressed;
            }
            else if (Style == ControlDrawStyle.Disabled)
            {
                if (CheckBoxState == CheckBoxState.Checked)
                    return VisualStyleElement.Button.CheckBox.CheckedDisabled;
                else if (CheckBoxState == CheckBoxState.Unchecked)
                    return VisualStyleElement.Button.CheckBox.UncheckedDisabled;
                else
                    return VisualStyleElement.Button.CheckBox.MixedDisabled;
            }
            else
            {
                if (CheckBoxState == CheckBoxState.Checked)
                    return VisualStyleElement.Button.CheckBox.CheckedNormal;
                else if (CheckBoxState == CheckBoxState.Unchecked)
                    return VisualStyleElement.Button.CheckBox.UncheckedNormal;
                else
                    return VisualStyleElement.Button.CheckBox.MixedNormal;
            }
        }

        /// <summary>
        /// Gets the System.Windows.Forms.VisualStyles.VisualStyleRenderer to draw the specified element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected System.Windows.Forms.VisualStyles.VisualStyleRenderer GetRenderer(VisualStyleElement element)
        {
            return new System.Windows.Forms.VisualStyles.VisualStyleRenderer(element);
        }
        #endregion

        protected override void OnDraw(GraphicsCache graphics, RectangleF area)
        {
            if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(GetBackgroundElement()))
                GetRenderer(GetBackgroundElement()).DrawBackground(graphics.Graphics, Rectangle.Round(area));
            else
                mStandardCheckBox.Draw(graphics, area);
        }

        protected override SizeF OnMeasureContent(MeasureHelper measure, SizeF maxSize)
        {
            if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(GetBackgroundElement()))
                return GetRenderer(GetBackgroundElement()).GetPartSize(measure.Graphics, ThemeSizeType.True);
            else
                return mStandardCheckBox.Measure(measure, Size.Empty, maxSize);
        }
    }
}
