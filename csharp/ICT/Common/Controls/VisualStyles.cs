/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 10:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

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
		private Font InternalTitleText;
		public Font TitleText {get{return InternalTitleText;}}
		
		private Color InternalTextColour;
		public Color TextColour {get{return InternalTextColour;}}
		
		private Color InternalPanelGradientStart;
		public Color PanelGradientStart {get{return InternalPanelGradientStart;}}
		
		private Color InternalPanelGradientEnd;
		public Color PanelGradientEnd {get{return InternalPanelGradientEnd;}}
		
		private LinearGradientMode InternalPanelGradientMode;
		public LinearGradientMode PanelGradientMode {get{return InternalPanelGradientMode;}}
		
		private int InternalTitleHeight;
		public int TitleHeight {get{return InternalTitleHeight;}}
		
		private Color InternalContentGradientStart;
		public Color ContentGradientStart {get{return InternalContentGradientStart;}}
		
		private Color InternalContentGradientEnd;
		public Color ContentGradientEnd {get{return InternalContentGradientEnd;}}
		
		private LinearGradientMode InternalContentGradientMode;
		public LinearGradientMode ContentGradientMode {get{return InternalContentGradientMode;}}
		
		//Constructor
		public TVisualStyles(TVisualStylesEnum style)
		{
			switch(style){
				case TVisualStylesEnum.vsAccordionPanel:
					InternalTitleText = new System.Drawing.Font("Sanserif",12);
					InternalTextColour = Color.FromArgb(61,92,135);
					InternalPanelGradientStart = Color.FromArgb(0,232,241);
					InternalPanelGradientEnd = Color.FromArgb(153,255,208);
					InternalPanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
					InternalTitleHeight = 100;
					InternalContentGradientStart = Color.FromArgb(255,255,255);
					InternalContentGradientEnd = Color.FromArgb(195,210,230);
					InternalContentGradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
					break;
					
				case TVisualStylesEnum.vsTaskPanel:
					InternalTitleText = new System.Drawing.Font("Serif",12);
					InternalTextColour = Color.FromArgb(61,92,0);
					InternalPanelGradientStart = Color.FromArgb(0,232,241);
					InternalPanelGradientEnd = Color.FromArgb(153,255,208);
					InternalPanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
					InternalTitleHeight = 100;
					InternalContentGradientStart = Color.FromArgb(255,255,255);
					InternalContentGradientEnd = Color.FromArgb(195,210,230);
					InternalContentGradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
					break;
					
				case TVisualStylesEnum.vsDashboard:
					InternalTitleText = new System.Drawing.Font("Sanserif",12);
					InternalTextColour = Color.FromArgb(255,100,135);
					InternalPanelGradientStart = Color.FromArgb(0,232,241);
					InternalPanelGradientEnd = Color.FromArgb(153,255,208);
					InternalPanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
					InternalTitleHeight = 100;
					InternalContentGradientStart = Color.FromArgb(255,255,255);
					InternalContentGradientEnd = Color.FromArgb(195,210,230);
					InternalContentGradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
					break;
					
				case TVisualStylesEnum.vsShepherd:
					InternalTitleText = new System.Drawing.Font("Sanserif",12);
					InternalTextColour = Color.FromArgb(42,242,135);
					InternalPanelGradientStart = Color.FromArgb(0,232,241);
					InternalPanelGradientEnd = Color.FromArgb(153,255,208);
					InternalPanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
					InternalTitleHeight = 100;
					InternalContentGradientStart = Color.FromArgb(255,255,255);
					InternalContentGradientEnd = Color.FromArgb(195,210,230);
					InternalContentGradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
					break;

				case TVisualStylesEnum.vsHorizontalCollapse:
				InternalTitleText = new System.Drawing.Font("Serif",12);
					InternalTextColour = Color.FromArgb(255,255,255);
					InternalPanelGradientStart = Color.FromArgb(0,232,241);
					InternalPanelGradientEnd = Color.FromArgb(153,255,208);
					InternalPanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
					InternalTitleHeight = 100;
					InternalContentGradientStart = Color.FromArgb(255,255,255);
					InternalContentGradientEnd = Color.FromArgb(195,210,230);
					InternalContentGradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
					break;			
			}
		}
	}
}
