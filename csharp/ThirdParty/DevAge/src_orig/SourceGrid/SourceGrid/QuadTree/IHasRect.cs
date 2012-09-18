using SourceGrid;
using System;
using System.Drawing;

namespace QuadTreeLib
{
    /// <summary>
    /// An interface that defines and object with a rectangle
    /// </summary>
    public interface IHasRect
    {
        Range Rectangle { get; }
    }
}
