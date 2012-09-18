using System;
using System.Collections.Generic;
using System.Text;

namespace DevAge.Drawing.VisualElements
{
    public interface IDropDownButton : IVisualElement
    {
        ButtonStyle Style
        {
            get;
            set;
        }
    }

    [Serializable]
    public abstract class DropDownButtonBase : VisualElementBase, IDropDownButton
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public DropDownButtonBase()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public DropDownButtonBase(DropDownButtonBase other)
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
