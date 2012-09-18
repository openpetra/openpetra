using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DevAge.Drawing.VisualElements
{
    [Serializable]
    public class EditablePanelThemed : EditablePanelBase
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public EditablePanelThemed()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public EditablePanelThemed(EditablePanelThemed other)
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
            return new EditablePanelThemed(this);
        }

        /// <summary>
        /// Standard button used when the XP style are disabled.
        /// </summary>
        private EditablePanel mStandard = new EditablePanel();

        #region Helper methods
        protected VisualStyleElement GetBackgroundElement()
        {
            return VisualStyleElement.TextBox.TextEdit.Normal;
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

        public override BorderStyle BorderStyle
        {
            get{return base.BorderStyle;}
            set
            {
                base.BorderStyle = value;
                mStandard.BorderStyle = value;
            }
        }

        public override void Draw(GraphicsCache graphics, RectangleF area)
        {
            if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(GetBackgroundElement()))
            {
                if (BorderStyle == BorderStyle.System)
                    GetRenderer(GetBackgroundElement()).DrawBackground(graphics.Graphics, Rectangle.Round(area));
            }
            else
                mStandard.Draw(graphics, area);
        }

        private RectangleBorder mEquivalentPadding = new RectangleBorder(new BorderLine(Color.Empty, 2));

        public override System.Drawing.RectangleF GetContentRectangle(System.Drawing.RectangleF backGroundArea)
        {
            if (BorderStyle == BorderStyle.System)
                return mEquivalentPadding.GetContentRectangle(backGroundArea);
            else
                return backGroundArea;
        }

        public override System.Drawing.SizeF GetExtent(System.Drawing.SizeF contentSize)
        {
            if (BorderStyle == BorderStyle.System)
                return mEquivalentPadding.GetExtent(contentSize);
            else
                return contentSize;
        }

        public override RectanglePartType GetPointPartType(RectangleF area, PointF point, out float distanceFromBorder)
        {
            if (BorderStyle == BorderStyle.System)
                return mEquivalentPadding.GetPointPartType(area, point, out distanceFromBorder);
            else
            {
                distanceFromBorder = 0;
                return RectanglePartType.ContentArea;
            }
        }
    }
}
