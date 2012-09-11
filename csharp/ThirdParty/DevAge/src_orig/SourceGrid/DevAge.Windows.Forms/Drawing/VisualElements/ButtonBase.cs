using System;
using System.Collections.Generic;
using System.Text;

namespace DevAge.Drawing.VisualElements
{
    public interface IButton : IBackground
    {
        ButtonStyle Style
        {
            get;
            set;
        }
    }

    [Serializable]
    public abstract class ButtonBase : BackgroundBase, IButton
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public ButtonBase()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public ButtonBase(ButtonBase other)
            : base(other)
        {
            Style = other.Style;
        }
        #endregion

        #region Properties
        private ButtonStyle mControlDrawStyle = ButtonStyle.Normal;
        public virtual ButtonStyle Style
        {
            get { return mControlDrawStyle; }
            set { mControlDrawStyle = value; }
        }
        protected virtual bool ShouldSerializeStyle()
        {
            return Style != ButtonStyle.Normal;
        }
        #endregion

    }
}
