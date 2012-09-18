using System;
using System.Collections.Generic;
using System.Text;

namespace DevAge.Drawing.VisualElements
{
    public interface ICheckBox : IVisualElement
    {
        ControlDrawStyle Style
        {
            get;
            set;
        }

        CheckBoxState CheckBoxState
        {
            get;
            set;
        }
    }

    [Serializable]
    public abstract class CheckBoxBase : VisualElementBase, ICheckBox
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public CheckBoxBase()
        {
            AnchorArea = new AnchorArea(float.NaN, float.NaN, float.NaN, float.NaN, true, true);
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public CheckBoxBase(CheckBoxBase other)
            : base(other)
        {
            Style = other.Style;
            CheckBoxState = other.CheckBoxState;
        }
        #endregion

        #region Properties
        private ControlDrawStyle mControlDrawStyle = ControlDrawStyle.Normal;
        public virtual ControlDrawStyle Style
        {
            get { return mControlDrawStyle; }
            set { mControlDrawStyle = value; }
        }
        protected virtual bool ShouldSerializeStyle()
        {
            return Style != ControlDrawStyle.Normal;
        }

        private CheckBoxState mCheckBoxState = CheckBoxState.Undefined;

        public virtual CheckBoxState CheckBoxState
        {
            get { return mCheckBoxState; }
            set { mCheckBoxState = value; }
        }
        protected virtual bool ShouldSerializeCheckBoxState()
        {
            return CheckBoxState != CheckBoxState.Undefined;
        }
        #endregion
    }
}
