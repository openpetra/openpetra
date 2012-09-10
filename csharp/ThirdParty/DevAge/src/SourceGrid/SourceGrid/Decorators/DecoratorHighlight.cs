using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGrid.Decorators
{
    public class DecoratorHighlight : DecoratorBase
    {
        private Range mRange = Range.Empty;
        /// <summary>
        /// Gets or sets the range to draw
        /// </summary>
        public Range Range
        {
            get { return mRange; }
            set { mRange = value; }
        }


        public override bool IntersectWith(Range range)
        {
            return Range.IntersectsWith(range);
        }

        public override void Draw(RangePaintEventArgs e)
        {
        }
    }
}
