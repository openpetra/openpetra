/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 10:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing.Text;

namespace Ict.Common.Controls
{
	
	public enum TVisualStylesEnum
	{
		vsAccordionPanel,
		vsTaskPanel,
		vsDashboard,
		vsShepherd,
		vsHorizontalCollapse
	}
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class TVisualStyles
	{
		public Font titleText {get;}
		public Color textColour {get;}
		public Color panelGradientStart {get;}
		public Color panelGradientEnd {get;}
		public LinearGradientMode panelGradientMode {get;}
		public int height {get;}
		public Color contentGradientStart {get;}
		public Color contentGradientEnd {get;}
		public LinearGradientMode contentGradientMode {get;}
		
		//Constructor
		public TVisualStyles(TVisualStylesEnum style)
		{
			switch(style){
				case TVisualStylesEnum.vsAccordionPanel:
					titleText = 
					textColour = Color.FromArgb(61,92,135);
					panelGradientStart = Color.FromArgb(224,232,241);
					panelGradientEnd = Color.FromArgb(153,176,208);
					panelGradientMode = System.Drawing.Drawing2D.Vertical;
					height = 100;
					contentGradientStart = Color.FromArgb(255,255,255);
					contentGradientEnd = Color.FromArgb(195,210,230);
					contentGradientMode = System.Drawing.Drawing2D.Vertical;
					break;
					
				case TVisualStylesEnum.vsTaskPanel:
					titleText = System.Drawing.Text.GenericFontFamilies.SansSerif;
					textColour = Color.FromArgb(63,150,75);
					panelGradientStart = #96CDCD;
					panelGradientEnd = #D1EEEE;
					panelGradientMode = System.Drawing.Drawing2D.ForwardDiagonal;
					height = 100;
					contentGradientStart = #96CDCD;
					contentGradientEnd = #D1EEEE;
					contentGradientMode = System.Drawing.Drawing2D.ForwardDiagonal;
					break;
					
				case TVisualStylesEnum.vsDashboard:
					titleText = System.Drawing.Text.GenericFontFamilies.SansSerif;
					textColour = #000000;
					panelGradientStart = #96CDCD;
					panelGradientEnd = #D1EEEE;
					panelGradientMode = System.Drawing.Drawing2D.ForwardDiagonal;
					height = 100;
					contentGradientStart = #96CDCD;
					contentGradientEnd = #D1EEEE;
					contentGradientMode = System.Drawing.Drawing2D.ForwardDiagonal;
					break;
					
				case TVisualStylesEnum.vsShepherd:
					titleText = System.Drawing.Text.GenericFontFamilies.SansSerif;
					textColour = #000000;
					panelGradientStart = #96CDCD;
					panelGradientEnd = #D1EEEE;
					panelGradientMode = System.Drawing.Drawing2D.ForwardDiagonal;
					height = 100;
					contentGradientStart = #96CDCD;
					contentGradientEnd = #D1EEEE;
					contentGradientMode = System.Drawing.Drawing2D.ForwardDiagonal;
					break;

				case TVisualStylesEnum.vsHorizontalCollapse:
					titleText = System.Drawing.Text.GenericFontFamilies.SansSerif;
					textColour = #000000;
					panelGradientStart = #96CDCD;
					panelGradientEnd = #D1EEEE;
					panelGradientMode = System.Drawing.Drawing2D.ForwardDiagonal;
					height = 100;
					contentGradientStart = #96CDCD;
					contentGradientEnd = #D1EEEE;
					contentGradientMode = System.Drawing.Drawing2D.ForwardDiagonal;
					break;					
			}
		}
	}
}
