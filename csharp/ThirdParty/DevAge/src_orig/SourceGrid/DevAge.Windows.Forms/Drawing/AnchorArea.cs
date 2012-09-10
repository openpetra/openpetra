using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DevAge.Drawing
{
    //[TypeConverter(typeof(AnchorAreaConverter))]
    //[ODL.SerializationMode(ODL.DefinitionType.ComplexTypeFields)]
    /// <summary>
    /// The AnchorArea class is used to specify the anchor properties of an object.
    /// You can set to align the content to the left, right, top or bottom using the relative properties (Left, Right, Top, Bottom).
    /// You can also set more than one properties to allign the content to more than one side.
    /// Use float.NaN to set a null value for one of the properties.
    /// </summary>
    [Serializable]
    public class AnchorArea : ICloneable, IComparable
    {
        /// <summary>
        /// Default is constructor
        /// </summary>
        public AnchorArea()
        {
        }

        /// <summary>
        /// Construct an anchor area object
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="center"></param>
        /// <param name="middle"></param>
        public AnchorArea(float left, float top, float right, float bottom, bool center, bool middle)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
            Center = center;
            Middle = middle;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public AnchorArea(AnchorArea other)
        {
            Right = other.Right;
            Left = other.Left;
            Bottom = other.Bottom;
            Top = other.Top;
            Center = other.Center;
            Middle = other.Middle;
        }

        /// <summary>
        /// Constructo an anchorarea class based on the aligment and the stretch parameters.
        /// </summary>
        /// <param name="aligment"></param>
        /// <param name="stretch"></param>
        public AnchorArea(ContentAlignment aligment, bool stretch)
        {
            if (Utilities.IsBottom(aligment) || stretch)
                Bottom = 0;
            if (Utilities.IsLeft(aligment) || stretch)
                Left = 0;
            if (Utilities.IsRight(aligment) || stretch)
                Right = 0;
            if (Utilities.IsTop(aligment) || stretch)
                Top = 0;

            if (Utilities.IsCenter(aligment) && stretch == false)
                Center = true;
            if (Utilities.IsMiddle(aligment) && stretch == false)
                Middle = true;
        }

        public static AnchorArea Empty
        {
            get { return new AnchorArea(); }
        }

        public bool IsEmpty
        {
            get { return HasRight == false && HasLeft == false && 
                        HasTop == false && HasBottom == false && 
                        Middle == false && Center == false; }
        }

        [DefaultValue(float.NaN)]
        public float Right = float.NaN;

        [DefaultValue(float.NaN)]
        public float Left = float.NaN;

        [DefaultValue(false)]
        public bool Center;

        [DefaultValue(float.NaN)]
        public float Top = float.NaN;

        [DefaultValue(float.NaN)]
        public float Bottom = float.NaN;

        [DefaultValue(false)]
        public bool Middle;

        public bool HasRight
        {
            get { return float.IsNaN(Right) == false; }
        }
        public bool HasLeft
        {
            get { return float.IsNaN(Left) == false; }
        }
        public bool HasTop
        {
            get { return float.IsNaN(Top) == false; }
        }
        public bool HasBottom
        {
            get { return float.IsNaN(Bottom) == false; }
        }

        public override string ToString()
        {
            return "Top " + Top.ToString() + ", " +
                "Bottom " + Bottom.ToString() + ", " +
                "Right " + Right.ToString() + ", " +
                "Left " + Left.ToString();
        }

        public override int GetHashCode()
        {
            return (int)(Top + Bottom + Left + Right);
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        #region ICloneable Members

        public object Clone()
        {
            return new AnchorArea(this);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            
            if (!(obj is AnchorArea))
                throw new ArgumentException("Invalid object, AnchorArea expected");

            AnchorArea other = (AnchorArea)obj;

            int topCompare = Top.CompareTo(other.Top);
            int bottomCompare = Bottom.CompareTo(other.Bottom);
            int leftCompare = Left.CompareTo(other.Left);
            int rightCompare = Right.CompareTo(other.Right);
            int centerCompare = Center.CompareTo(other.Center);
            int middleCompare = Middle.CompareTo(other.Middle);

            if (topCompare > 1)
                return 1;
            else if (topCompare < 1)
                return -1;
            else
            {
                if (bottomCompare > 1)
                    return 1;
                else if (bottomCompare < 1)
                    return -1;
                else
                {
                    if (rightCompare > 1)
                        return 1;
                    else if (rightCompare < 1)
                        return -1;
                    else
                    {
                        if (leftCompare > 1)
                            return 1;
                        else if (leftCompare < 1)
                            return -1;
                        else
                        {
                            if (centerCompare > 1)
                                return 1;
                            else if (centerCompare < 1)
                                return -1;
                            else
                            {
                                if (middleCompare > 1)
                                    return 1;
                                else if (middleCompare < 1)
                                    return -1;
                                else
                                {
                                    return 0;
                                }
                            }
                        }
                    }
                }
            }

        }

        #endregion

        public static bool operator ==(AnchorArea a, AnchorArea b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(AnchorArea a, AnchorArea b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Calculate the destination area of 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="content"></param>
        /// <param name="anchor"></param>
        /// <returns></returns>
        public static RectangleF CalculateArea(RectangleF area, SizeF content, AnchorArea anchor)
        {
            if (anchor.IsEmpty)
                return area;
            else
            {
                RectangleF destination = new RectangleF();

                //Left and width
                if (anchor.Center)
                {
                    //TODO Try in the future to use also Left and Right property when align the content.
                    destination.X = (area.X + area.Width / 2.0F) - content.Width / 2.0F;
                    destination.Width = content.Width;
                }
                else if (anchor.HasLeft && anchor.HasRight)
                {
                    destination.X = area.Left + anchor.Left;
                    destination.Width = area.Width - (anchor.Left + anchor.Right);
                }
                else if (anchor.HasLeft)
                {
                    destination.X = area.Left + anchor.Left;
                    destination.Width = content.Width;
                }
                else if (anchor.HasRight)
                {
                    destination.X = (area.Right - content.Width) - anchor.Right;
                    destination.Width = content.Width;
                }
                else
                {
                    destination.X = area.Left;
                    destination.Width = content.Width;
                }

                //Top and height
                if (anchor.Middle)
                {
                    //TODO Try in the future to use also Left and Right property when align the content.
                    destination.Y = (area.Y + area.Height / 2.0F) - content.Height / 2.0F;
                    destination.Height = content.Height;
                }
                else if (anchor.HasTop && anchor.HasBottom)
                {
                    destination.Y = area.Top + anchor.Top;
                    destination.Height = area.Height - (anchor.Top + anchor.Bottom);
                }
                else if (anchor.HasTop)
                {
                    destination.Y = area.Top + anchor.Top;
                    destination.Height = content.Height;
                }
                else if (anchor.HasBottom)
                {
                    destination.Y = (area.Bottom - content.Height) - anchor.Bottom;
                    destination.Height = content.Height;
                }
                else
                {
                    destination.Y = area.Top;
                    destination.Height = content.Height;
                }

                destination.Intersect(area);
                return destination;
            }
        }
    }

    ///// <summary>
    ///// TypeConverter for the Border structure. Use the ODL definition language for string conversion.
    ///// </summary>
    //public class AnchorAreaConverter : ODL.Converters.PlainTextConverter
    //{
    //    public AnchorAreaConverter()
    //        : base(typeof(AnchorArea))
    //    {
    //        Configuration.TypeSynonyms.Add(new ODL.TypeSynonym(ConverterType.Name, ConverterType));
    //    }
    //}


}
