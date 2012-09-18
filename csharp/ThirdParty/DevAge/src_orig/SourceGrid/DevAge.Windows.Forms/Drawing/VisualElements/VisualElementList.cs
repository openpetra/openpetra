using System;
using System.Collections.Generic;
using System.Text;

namespace DevAge.Drawing.VisualElements
{
    [Serializable]
    public class VisualElementList : List<IVisualElement>, ICloneable
    {
        #region ICloneable Members
        public object Clone()
        {
            VisualElementList elements = new VisualElementList();
            foreach (IVisualElement element in this)
            {
                elements.Add((IVisualElement)element.Clone());
            }

            return elements;
        }
        #endregion
    }
}
