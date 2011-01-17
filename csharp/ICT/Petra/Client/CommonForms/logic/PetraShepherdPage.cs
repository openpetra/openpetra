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

using Ict.Common;

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
            
            // Add to FPagesList
            
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