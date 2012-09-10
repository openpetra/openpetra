using System;

namespace DevAge.Drawing
{

    /// <summary>
    /// Specifies alignment of content on the drawing surface.
    /// The same enum as System.Drawing.ContentAlignment. Rewritten for compatibility with the Compact Framework.
    /// </summary>
    public enum ContentAlignment
    {
        /// <summary>
        /// Content is vertically aligned at the bottom, and horizontally aligned at the center.  
        /// </summary>
        BottomCenter = 512,
        /// <summary>
        /// Content is vertically aligned at the bottom, and horizontally aligned on the left.  
        /// </summary>
        BottomLeft = 256,
        /// <summary>
        /// Content is vertically aligned at the bottom, and horizontally aligned on the right.
        /// </summary>
        BottomRight = 1024,
        /// <summary>
        /// Content is vertically aligned in the middle, and horizontally aligned at the center.  
        /// </summary>
        MiddleCenter = 32,
        /// <summary>
        /// Content is vertically aligned in the middle, and horizontally aligned on the left.  
        /// </summary>
        MiddleLeft = 16,
        /// <summary>
        /// Content is vertically aligned in the middle, and horizontally aligned on the right. 
        /// </summary>
        MiddleRight = 64,
        /// <summary>
        /// Content is vertically aligned at the top, and horizontally aligned at the center.  
        /// </summary>
        TopCenter = 2,
        /// <summary>
        /// Content is vertically aligned at the top, and horizontally aligned on the left. 
        /// </summary>
        TopLeft = 1,
        /// <summary>
        /// Content is vertically aligned at the top, and horizontally aligned on the right.
        /// </summary>
        TopRight = 4
    }

    public enum RectanglePartType
    {
        None = 0,
        ContentArea = 1,
        LeftBorder = 2,
        TopBorder = 3,
        RightBorder = 4,
        BottomBorder = 5
    }

    public enum Gradient3DBorderStyle
    {
        Raised = 1,
        Sunken = 2
    }

    public enum ElementsDrawMode
    {
        /// <summary>
        /// Draw each element over the previous
        /// </summary>
        Covering = 1,
        /// <summary>
        /// Align each element with the previous if an alignment is specified.
        /// </summary>
        Align = 2
    }


    public enum ControlDrawStyle
    {
        Normal = 1,
        Pressed = 2,
        Hot = 3,
        Disabled = 4
    }

    public enum ButtonStyle
    {
        Normal = 1,
        Pressed = 2,
        Hot = 3,
        Disabled = 4,
        NormalDefault = 5,
        Focus = 6
    }

    public enum HeaderSortStyle
    {
        None = 0,
        Ascending = 1,
        Descending = 2
    }

    public enum CheckBoxState
    {
        Undefined = 0,
        Checked = 1,
        Unchecked = 2
    }

    public enum BorderStyle
    {
        None,
        System
    }

    public enum BackgroundColorStyle
    {
        None = 0,
        Linear = 1,
        Solid = 2
    }
}
