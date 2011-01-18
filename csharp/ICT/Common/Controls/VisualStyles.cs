/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 10:51
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 * //Comments: Activated underline is currently not enabled. 
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
               //Text Variables
               private Font InternalTitleText;
               public Font TitleText {get{return InternalTitleText;}}
               
               private Font InternalContentText;
               public Font ContentText{get{return InternalContentText;}}

               private Color InternalTitleTextColour;
               public Color TitleTextColour {get{return InternalTitleTextColour;}}

               private Color InternalContentTextColour;
               public Color ContentTextColour {get{return InternalContentTextColour;}}

               private Color InternalHoverTitleTextColour;
               public Color HoverTitleTextColour {get{return InternalHoverTitleTextColour;}}

               private int InternalTitleHeight;
               public int TitleHeight {get{return InternalTitleHeight;}}

               private Color InternalContentActivatedTextColour;
               public Color ContentActivatedTextColour {get{return InternalContentActivatedTextColour;}}

               private Color InternalContentHoverTextColour;
               public Color ContentHoverTextColour {get{return InternalContentHoverTextColour;}}
               
               //Background Variables
               private Color InternalContentBackgroundColour;
               public Color ContentBackgroundColour {get{return InternalContentBackgroundColour;}}

               private Color InternalTitleBackgroundColour;
               public Color TitleBackgroundColour {get{return InternalTitleBackgroundColour;}}
               
               private Color InternalContentHoverBackgroundColour;
               public Color ContentHoverBackgroundColour {get{return InternalContentHoverBackgroundColour;}}
               
               private Color InternalContentActivatedBackgroundColour;
               public Color ContentActivatedBackgroundColour {get{return InternalContentActivatedBackgroundColour;}}
               
               //Gradient Variables
               private Color InternalPanelGradientStart;
               public Color PanelGradientStart {get{return InternalPanelGradientStart;}}

               private Color InternalPanelGradientEnd;
               public Color PanelGradientEnd {get{return InternalPanelGradientEnd;}}

               private LinearGradientMode InternalPanelGradientMode;
               public LinearGradientMode PanelGradientMode {get{return InternalPanelGradientMode;}}

               private Color InternalContentGradientStart;
               public Color ContentGradientStart {get{return InternalContentGradientStart;}}

               private Color InternalContentGradientEnd;
               public Color ContentGradientEnd {get{return InternalContentGradientEnd;}}

               private LinearGradientMode InternalContentGradientMode;
               public LinearGradientMode ContentGradientMode {get{return InternalContentGradientMode;}}

               //Layout Variables
               private int InternalTaskIndentation;
               public int TaskIndentation {get{return InternalTaskIndentation;}}

               private bool InternalAutomaticNumbering;
               public bool AutomaticNumbering {get{return InternalAutomaticNumbering;}}
               
               //bool variables for determining style in TaskList
               public bool UsePanelGradient;
               public bool UseContentBackgroundColours;
               public bool UseContentGradient;
               
               

               //Constructor
               public TVisualStyles(TVisualStylesEnum style)
               {
                       switch(style){
                               case TVisualStylesEnum.vsAccordionPanel:
                                       //TitleText Variables
                                       InternalTitleText = new System.Drawing.Font("Verdana",8, System.Drawing.FontStyle.Bold);
                                       InternalTitleTextColour = Color.FromArgb(89,101,165);
                                       InternalHoverTitleTextColour = Color.FromArgb(20,65,142);                                       
                                       InternalTitleHeight = 100;
                                       
                                       //ContentText Variables
                                       //underline hover is default
                                       InternalContentText = new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Bold);                                  
                                       InternalContentTextColour = Color.FromArgb(89,101,165);
                                       InternalContentHoverTextColour = Color.FromArgb(37,101,212);
                                       InternalContentActivatedTextColour = InternalContentHoverTextColour;
                                       
                                       //Gradient Variables
                                       InternalPanelGradientStart = Color.FromArgb(255,255,255);
                                       InternalPanelGradientEnd = Color.FromArgb(190,212,254);
                                       InternalPanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                                       InternalContentGradientStart = Color.FromArgb(212,228,254);
                                       InternalContentGradientEnd = Color.FromArgb(205,218,254);
                                       InternalContentGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;

                                       //Layout Variables
                                       InternalTaskIndentation = 50;
                                       InternalAutomaticNumbering = false;
                                       
                                       //bool variables
                                       UsePanelGradient = true;
                                       UseContentGradient = true;
                                       UseContentBackgroundColours = false;
                                       break;

                               case TVisualStylesEnum.vsTaskPanel:
                                       //TitleText Variables
                                       InternalTitleText = new System.Drawing.Font("Verdana",8, System.Drawing.FontStyle.Bold);
                                       InternalTitleTextColour = Color.FromArgb(30,92,196);                                       
                                       InternalHoverTitleTextColour = Color.FromArgb(102,134,181);
                                       InternalTitleHeight = 100;
                                       
                                       //ContentText Variables
                                       //underline hover is default
                                       InternalContentText = new System.Drawing.Font("Verdana", 7);                                                                                     
                                       InternalContentTextColour = Color.FromArgb(62,117,225);
                                       InternalContentHoverTextColour = Color.FromArgb(90,155,252);
                                       InternalContentActivatedTextColour = InternalContentTextColour;
                                       
                                       //BackgroundVariables
                                       InternalContentBackgroundColour = Color.FromArgb(212,221,249);

                                       //Gradient Variables
                                       InternalPanelGradientStart = Color.FromArgb(255,255,255);
                                       InternalPanelGradientEnd = Color.FromArgb(190,212,254);
                                       InternalPanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                                       
                                       //Layout Variables
                                       InternalTaskIndentation = 15;
                                       InternalAutomaticNumbering = false;
                                       
                                       //bool variables
                                       UsePanelGradient = true;
                                       UseContentGradient = false;
                                       UseContentBackgroundColours = false;
                                       break;

                               case TVisualStylesEnum.vsDashboard:
                                       //TitleText Variables
                                       InternalTitleText = new System.Drawing.Font("Verdana",8, System.Drawing.FontStyle.Bold);
                                       InternalTitleTextColour = Color.FromArgb(30,92,196);
                                       InternalHoverTitleTextColour = Color.FromArgb(102,134,181);
                                       InternalTitleHeight = 100; 
                                       
                                       //ContentText Variables
                                       
                                       
                                       //BackgroundVariables
                                       InternalContentBackgroundColour = Color.FromArgb(212,221,249);

                                       //Gradient Variables
                                       InternalPanelGradientStart = Color.FromArgb(255,255,255);
                                       InternalPanelGradientEnd = Color.FromArgb(190,212,254);
                                       InternalPanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;

                                       //Layout Variables
                                       InternalTaskIndentation = 45;
                                       InternalAutomaticNumbering = false;
                                       
                                       //bool variables
                                       UsePanelGradient = true;
                                       UseContentGradient = false;
                                       UseContentBackgroundColours = false;

                                       break;

                               case TVisualStylesEnum.vsShepherd:
                                       //TitleText Variables
                                       InternalTitleText = new System.Drawing.Font("Verdana",8, System.Drawing.FontStyle.Bold);
                                       InternalTitleTextColour = Color.FromArgb(0,0,0);                                      
                                       InternalHoverTitleTextColour = Color.FromArgb(60,60,60);
                                       InternalTitleHeight = 100;
                                       
                                       //ContentText Variables
                                       //underline hover is default
                                       InternalContentText = new System.Drawing.Font("Verdana", 8);
                                       InternalContentTextColour = Color.FromArgb(0,0,0);
                                       InternalContentHoverTextColour = Color.FromArgb(60,60,60);
                                       InternalContentActivatedTextColour = Color.FromArgb(255,255,255);
                                       
                                       //Gradient Variables
                                       InternalPanelGradientStart = Color.FromArgb(255,255,255);
                                       InternalPanelGradientEnd = Color.FromArgb(255,255,255);
                                       InternalPanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                                       
                                       //Background variables
                                       InternalContentBackgroundColour =Color.FromArgb(255,255,255);
                                       InternalContentHoverTextColour = Color.FromArgb(210,210,210);
                                       InternalContentActivatedTextColour = Color.FromArgb(0,40,80);
                                       

                                       //Layout Variables
                                       InternalTaskIndentation = 0;
                                       InternalAutomaticNumbering = true;
                                       
                                       //bool variables
                                       UsePanelGradient = false;
                                       UseContentGradient = false;
                                       UseContentBackgroundColours = true;
                                       break;

                               case TVisualStylesEnum.vsHorizontalCollapse:
                                       //TitleText Variables
                                       InternalTitleText = new System.Drawing.Font("Tahoma",13,System.Drawing.FontStyle.Bold);
                                       InternalTitleTextColour = Color.FromArgb(20,65,142);
                                       InternalHoverTitleTextColour = InternalTitleTextColour;
                                       InternalTitleHeight = 100;
                                       
                                       //ContentText Variables
                                       
                                       //BackgroundVariables
                                       InternalContentBackgroundColour = Color.FromArgb(212,221,249);

                                       //Gradient Variables
                                       InternalPanelGradientStart = Color.FromArgb(255,255,255);
                                       InternalPanelGradientEnd = Color.FromArgb(190,212,254);
                                       InternalPanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;

                                       //Layout Variables
                                       InternalTaskIndentation = 30;
                                       InternalAutomaticNumbering = false;
                                       
                                       //bool variables
                                       UsePanelGradient = true;
                                       UseContentGradient = false;
                                       UseContentBackgroundColours = false;
                                       break;		
			}
		}
	}
}
