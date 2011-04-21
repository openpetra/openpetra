//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 chadds
//		 ashleyc
//
// Copyright 2004-2010 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Ict.Common.Controls
{

	   /// <summary>
	   /// All possible values that can be used to create a TVisualStyles Object
	   /// </summary>
       public enum TVisualStylesEnum
       {
               vsAccordionPanel,
               vsTaskPanel,
               vsDashboard,
               vsShepherd,
               vsHorizontalCollapse
       }
       /// <summary>
       /// Object that defines values for different visual styles in OpenPetra
       /// </summary>
       public class TVisualStyles
       {
               //Text Variables
               private Font InternalTitleText;
                /// <summary>The TitleText property gets the value of InternalTitleText</summary>
               /// <value>The TitleText Property represents the Font type of the Title Text of the user control.</value>
               public Font TitleText {get{return InternalTitleText;}}
               
               private Font InternalContentText;
               /// <summary>The ContentText property gets the value of InternalContentText</summary>
               /// <value>The ContentText property represents the Font specifications of the ContentText of the user control</value>               
               public Font ContentText{get{return InternalContentText;}}

               private Color InternalTitleTextColour;
                /// <summary>The TitleTextxColour gets the value of the InternalTitleTextColour </summary>
               /// <value> The TItleTextColour property represents the Colour specification of the TitleText of the user control</value>               
               public Color TitleTextColour {get{return InternalTitleTextColour;}}

               private Color InternalContentTextColour;
               /// <summary>The HoverTitleTextColour property gets the value of the InternalHoverTitleTextColour</summary>
               /// <value> The HoverTitleTextColour property represents the Colour specification of the title text when the mouse hovers over the title </value>               
               public Color ContentTextColour {get{return InternalContentTextColour;}}

               private Color InternalHoverTitleTextColour;
               /// <summary>The HoverTitleTextColour property gets the value of the InternalHoverTitleTextColour</summary>
               /// <value> The HoverTitleTextColour property represents the Colour specification of the title text when the mouse hovers over the title </value>               
               public Color HoverTitleTextColour {get{return InternalHoverTitleTextColour;}}

               private int InternalTitleHeight;
               /// <summary>The TitleHeight property gets the value of the InternalTitleHeight</summary>
               /// <value> The TitleHeight property represents the height of the TitleText</value>               
               public int TitleHeight {get{return InternalTitleHeight;}}

               private Color InternalContentActivatedTextColour;
               /// <summary>The ContentActivatedTextColour property gets the value of the InternalContentActivatedTextColour</summary>
               /// <value> The ContentActivatedTextColour property represents the colour of activated content text</value>               
               public Color ContentActivatedTextColour {get{return InternalContentActivatedTextColour;}}

               private Color InternalContentHoverTextColour;
               /// <summary>The ContentHoverTextColour property gets the value of the InternalContentHoverTextColour</summary>
               /// <value> The ContentHOverTextColour property represents the colour of the content text when the mouse hovers over it</value>               
               public Color ContentHoverTextColour {get{return InternalContentHoverTextColour;}}
               
               //Background Variables
               private Color InternalContentBackgroundColour;
               /// <summary>The ContentBackgroundColour property gets the value of the InternalContentBackgroundColour </summary>
               /// <value> The ContentBackgroundColour property represents the content's background color.</value>               
               public Color ContentBackgroundColour {get{return InternalContentBackgroundColour;}}

               private Color InternalTitleBackgroundColour;
               /// <summary>The TitleBackgroundColour property gets the value of the IntenralTitleBackgroundColour</summary>
               /// <value> The TItleBackgroundColour property represents the title's background colour</value>               
               public Color TitleBackgroundColour {get{return InternalTitleBackgroundColour;}}
               
               private Color InternalContentHoverBackgroundColour;
               /// <summary>The ContentHoverBackgroundColour property gets the value of the InternalContentHoverBackgroundColour</summary>
               /// <value> The ContentHOoverBackgroundColour property represent the content's background colour when the mouse hovers over it</value>               
               public Color ContentHoverBackgroundColour {get{return InternalContentHoverBackgroundColour;}}
               
               private Color InternalContentActivatedBackgroundColour;
               /// <summary>The ContentActivatedBackgroundColour property gets the value of the InternalContentAcivatedBackgroundColour </summary>
               /// <value> The ContentActivatedBackgroundCOlour property representf the content background colour when the content is activated </value>               
               public Color ContentActivatedBackgroundColour {get{return InternalContentActivatedBackgroundColour;}}
               
               //Gradient Variables
               private Color InternalPanelGradientStart;
               /// <summary>The PanelGradientStart property gets the value of the InternalPanelGradientStart</summary>
               /// <value> The PanelGradientStart property represents the beginning colour of the panel gradient</value>               
               public Color PanelGradientStart {get{return InternalPanelGradientStart;}}

               private Color InternalPanelGradientEnd;
               /// <summary>The PanelGradientEnd property gets the value of the InternalPanelGradientEnd</summary>
               /// <value> The PanelGradientEnd property represents the end colour of the title panel gradient.</value>               
               public Color PanelGradientEnd {get{return InternalPanelGradientEnd;}}

               private LinearGradientMode InternalPanelGradientMode;
               /// <summary>The PanelGradientMode property gets the value of the InternalPanelGradientMode</summary>
               /// <value> The PanelGradientMode property represents the direction of the gradient in the title panel gradient</value>               
               public LinearGradientMode PanelGradientMode {get{return InternalPanelGradientMode;}}

               private Color InternalContentGradientStart;
               /// <summary>The ContentGradientStart property gets the value of the InternalContentGradientstart</summary>
               /// <value> The ContentGradeientStart property represents the beginning colour of the content panel gradient</value>               
               public Color ContentGradientStart {get{return InternalContentGradientStart;}}

               private Color InternalContentGradientEnd;
               /// <summary>The ContentGradientEnd property gets the value of the InternalContentGradientEnd</summary>
               /// <value> THe ContentGradientEnd property represents the end colour of the content panel gradient</value>               
               public Color ContentGradientEnd {get{return InternalContentGradientEnd;}}

               private LinearGradientMode InternalContentGradientMode;
               /// <summary>The ContentGradientMode property gets the value of the InternalContentGradientMode</summary>
               /// <value> The ContentGradientMode property represents the direction of the gradient in the content panel gradient</value>               
               public LinearGradientMode ContentGradientMode {get{return InternalContentGradientMode;}}

               //Layout Variables
               private int InternalTaskIndentation;
               /// <summary> The TaskIndentation property gets the value of the InternalTaskIndentation</summary>
               /// <value> The TaskIndentation property represents the number of spaces the tasks are indented in the content panel</value>               
               public int TaskIndentation {get{return InternalTaskIndentation;}}

               private bool InternalAutomaticNumbering;
               /// <summary>The AutomaticNumbering property gets the value of the InternalAutomaticNumbering</summary>
               /// <value> The AutomaticNumbering property determines if automatic numbering is on</value>               
               public bool AutomaticNumbering {get{return InternalAutomaticNumbering;}}
               
               //bool variables for determining style in TaskList
               /// <summary>
               /// Boolean flag to specify whether the style uses a gradient background in the panel (title element)
               /// </summary>
               public bool UsePanelGradient;
               /// <summary>
               /// Boolean flag to specify whether certain controls use the background colors
               /// </summary>
               public bool UseContentBackgroundColours;
               /// <summary>
               /// Boolean flag to specify whether the style uses a gradient background in the content section
               /// </summary>
               public bool UseContentGradient;
               
               

               //Constructor
               
               /// <summary>
               /// Constructor to create a Visual Styles Object
               /// </summary>
               /// <param name="style">A TVisualStylesEnum Object</param>
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
                                       InternalContentGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;

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
                                       InternalContentHoverBackgroundColour = Color.FromArgb(210,210,210);
                                       InternalContentActivatedBackgroundColour = Color.FromArgb(0,80,160);
                                       

                                       //Layout Variables
                                       InternalTaskIndentation = 15;
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
