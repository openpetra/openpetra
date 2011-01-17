//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       >>>> Put your full name or just a shortname here <<<<
//
// Copyright 2004-2011 by OM International
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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml; 

using Ict.Common;
using Ict.Common.IO; 

namespace Ict.Petra.Client.CommonForms.Logic
{
    ///<summary>Instance of a Shepherd Page (holds data from YAML file)</summary>
    public class TPetraShepherdPage
    {
        string FID;
        
        string FTitle;
        
        string FNote;
        
        bool FVisible = true;
        
        bool FEnabled = true;
        
        UserControl FUserControl = null;
        
        string FUserControlNamespace;
        
        string FUserControlClassName;
        
        string FHelpContext;
        
        string FUserControlType;
        
        ///<summary>Is the Shepherd Page the Finish page in the Shepherd?</summary>
        protected bool FIsLastPage = false;
        
        bool FIsFirstPage = false;
        
        /// <summary>Return/Set Shepherd Page ID</summary>
        public string ID
        {
        	get
        	{
        		return FID;
        	}
        	set
        	{
        		FID = value;
        	}
        }
        
        /// <summary>Return Shepherd Page Title</summary>
        public string Title
        {
        	get
        	{
        		return FTitle;
        	}
        }
        
        /// <summary>Return/Set Shepherd Page Note</summary>
        public string Note
        {
        	get
        	{
        		return FNote;
        	}
        	set
        	{
        		FNote = value;
        	}
        }
    
        ///<summary>Is the Shepherd Page visible?</summary>
        public bool Visible 
        {
            get
            {
                return FVisible;
            }
            set
            {
                if (FVisible != value)
                {
                    FVisible = value;
                    OnVisibleOrEnabledChangedEvent();
                }
            }
        }
    
        ///<summary>Is the Shepherd Page enabled?</summary>
        public bool Enabled 
        {
            get
            {
                return FEnabled;
            }
            set
            {
                if (FEnabled != value)
                {
                    OnVisibleOrEnabledChangedEvent();
                    FEnabled = value;
                }
            }
        }
    

                /// <summary>Return Shepherd Page Namespace</summary>
        public string UserControlNamespace
        {
        	get
        	{
        		return FUserControlNamespace;
        	}
        }
        
        /// <summary>Return Shepherd Page Class Name</summary>
        public string UserControlClassName
        {
        	get
        	{
        		return FUserControlClassName;
        	}
        }
        
        /// <summary>Return/Set Shepherd Help Context Name</summary>
        public string HelpContext
        {
        	get
        	{
        		return FHelpContext;
        	}
        	set
        	{
        		FHelpContext = value;
        	}
        }
        
        /// <summary>Return/Set Shepherd Control Type</summary>
        public string UserControlClassType
        {
        	get
        	{
        		return FUserControlType;
        	}
        	set
        	{
        		FUserControlType = value;
        	}
        }
        
        ///<summary>Is the Shepherd Page the first reachable page in the Shepherd?</summary>
        public bool IsFirstPage 
        {
            get
            {
                return FIsFirstPage;
            }
            set
            {
                FIsFirstPage = value;
            }
        }
    
        ///<summary>Is the Shepherd Page the Finish page in the Shepherd?</summary>
        public bool IsLastPage 
        {
            get
            {
                return FIsLastPage;
            }
            set
            {
            	FIsLastPage = value;
            }
        }
    
        ///<summary>Raised if value of Visible or Enabled Property changes</summary>
        public event System.EventHandler VisibleOrEnabledChangedEvent;
    
    
        ///<summary>Fires the VisibleOrEnabledChangedEvent if something subscribed to it</summary>
        public void OnVisibleOrEnabledChangedEvent()
        {
            if (VisibleOrEnabledChangedEvent != null)
            {
                VisibleOrEnabledChangedEvent(this, new System.EventArgs());
            }
        }
        
        public TPetraShepherdPage()
        {
        }
        
        public TPetraShepherdPage(XmlNode ShepherdPageNode)
        {
        	//Comment
            FID=ShepherdPageNode["ID"].Value;
            TLogging.Log("~~ID Assigned~~ " + FID);
            
            FTitle=ShepherdPageNode["Title"].Value;
            TLogging.Log("~~Title Assigned~~ " + FTitle);
            
            FNote=ShepherdPageNode["Note"].Value;
            TLogging.Log("~~Note Assigned~~ " + FNote);
            
            FVisible=System.Convert.ToBoolean(ShepherdPageNode["Visible"].Value);
            TLogging.Log("~~Visible Assigned~~ " + System.Convert.ToString(FVisible));
            
            FEnabled=System.Convert.ToBoolean(ShepherdPageNode["Enabled"].Value);
            TLogging.Log("~~Enabled Assigned~~ " + System.Convert.ToString(FEnabled));
            
            FUserControlNamespace=ShepherdPageNode["UserControlNamespace"].Value;
            TLogging.Log("~~UserControlNamespace Assigned~~ " + FUserControlNamespace);
            
            FUserControlClassName=ShepherdPageNode["UserControlClassName"].Value;
            TLogging.Log("~~UserControlClassName Assigned~~ " + FUserControlClassName);
            
            FHelpContext=ShepherdPageNode["HelpContext"].Value;
            TLogging.Log("~~HelpContext Assigned~~ " + FHelpContext);
            
            FUserControlType=ShepherdPageNode["UserControlType"].Value;
            TLogging.Log("~~UserControlType Assigned~~ ");
            
            FIsLastPage=System.Convert.ToBoolean(ShepherdPageNode["IsLastPage"].Value);
            TLogging.Log("~~IsLastPage Assigned~~ " + System.Convert.ToString(FIsLastPage));
            
            FIsFirstPage=System.Convert.ToBoolean(ShepherdPageNode["IsFirstPage"].Value);
            TLogging.Log("~~IsFirstPage Assigned~~ " + System.Convert.ToString(FIsFirstPage));
        }
    }
    
    
    ///<summary>List of 0..n Shepherd Pages (holds data from YAML file)</summary>
    public class TPetraShepherdPagesList
    {
        Dictionary<string, TPetraShepherdPage> FPagesList = new Dictionary<string, TPetraShepherdPage>();
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AYamlFile"></param>
        public TPetraShepherdPagesList(string AYamlFile)
        {
            TLogging.Log("Entering TPetraShepherdPagesList Constructor. AYamlFile = " + AYamlFile + "...");
            
            TYml2Xml parser = new TYml2Xml(AYamlFile);
            XmlDocument XmlPages = parser.ParseYML2XML();
            TLogging.Log("TPetraShepherdPagesList has " + XmlPages.ChildNodes.Count + "Nodes.");    
        	XmlNode temporaryXmlNode = XmlPages.FirstChild; 
            for(int i = 0; i < XmlPages.ChildNodes.Count; i++)
        	{
            	TPetraShepherdPage temporaryPetraShepherdPage = new TPetraShepherdPage(temporaryXmlNode); 
            	FPagesList.Add(temporaryPetraShepherdPage.Title,temporaryPetraShepherdPage); 
        		temporaryXmlNode = XmlPages.NextSibling; 
            }
            TLogging.Log("TPetraShepherdPagesList Constructor ran.");    
        }
    }
    
    ///<summary>Specialisation of a Shepherd Page: Finish Page</summary>
    public class TPetraShepherdFinishPage : TPetraShepherdPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TPetraShepherdFinishPage()
        {
            base.FIsLastPage = true;
        }
    }
}