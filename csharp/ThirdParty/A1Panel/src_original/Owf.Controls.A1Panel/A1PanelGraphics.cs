using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Owf.Controls
{
	internal class A1PanelGraphics
	{
		public static GraphicsPath GetRoundPath(Rectangle r, int depth)
		{
			GraphicsPath graphPath = new GraphicsPath();

			graphPath.AddArc(r.X, r.Y, depth, depth, 180, 90);
			graphPath.AddArc(r.X + r.Width - depth, r.Y, depth, depth, 270, 90);
			graphPath.AddArc(r.X + r.Width - depth, r.Y + r.Height - depth, depth, depth, 0, 90);
			graphPath.AddArc(r.X, r.Y + r.Height - depth, depth, depth, 90, 90);
			graphPath.AddLine(r.X, r.Y + r.Height - depth, r.X, r.Y + depth / 2);

			return graphPath;
		}
	}
}
