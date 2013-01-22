//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       pauln, AustinS
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
using System.Reflection;
using System;

using Ict.Common;
using Ict.Common.IO;

namespace Ict.Petra.Client.CommonForms.Logic
{
    ///<summary>Instance of a Shepherd Page (holds data from YAML file)</summary>
    public class TPetraShepherdPage
    {
        #region VariableInstantiation
        /// <summary> Unique ID for each individual page.</summary>
        string FID;

        /// <summary>Displayed Title for each page.</summary>
        protected string FTitle;

        ///<summary>Displayed note for each page.</summary>
        string FNote;

        ///<summary>Determines if the page is to be visible in the navigation bar.</summary>
        bool FVisible = true;

        ///<summary>Determines if the page is enabled (or selectable) in the navigation bar.</summary>
        bool FEnabled = true;

        ///<summary>Sets the user control to be displayed on the page.</summary>
        UserControl FUserControl = null;

        ///<summary>Namespace in which the user control can be found.</summary>
        protected string FUserControlNamespace;

        ///<summary>Class name for the user control.</summary>
        protected string FUserControlClassName;

        ///<summary>Information to be displayed in the help dialouge.</summary>
        protected string FHelpContext;

        ///<summary>TODO?</summary>
        string FUserControlType;

        ///<summary>Is the Shepherd Page the Finish page in the Shepherd?</summary>
        protected bool FIsLastPage = false;

        ///<summary>Is the Shepherd Page the First page in the Shepherd?</summary>
        bool FIsFirstPage = false;
        #endregion

        #region AttributeDefinitions
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

        ///<summary>Retuns an instance of the UserControl.</summary>
        public UserControl UserControlInstance
        {
            get
            {
                if (FUserControl == null)
                {
                    return RealiseUserControl();
                }
                else
                {
                    return FUserControl;
                }
            }
        }
        #endregion

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

        ///<summary>Default Constructor for TPetraShepherdPage</summary>
        public TPetraShepherdPage()
        {
        }

        /// <summary>
        /// Conrstructor for individual Shepherd pages. Each time a page is constructed, it
        /// recieves information from the ShepherdPageNode and stores it in each of the
        /// variables.
        /// </summary>
        /// <param name="ShepherdPageNode"></param>
        public TPetraShepherdPage(XmlNode ShepherdPageNode)
        {
            TLogging.Log("Constructor REACHED");

            FID = ShepherdPageNode.Attributes["ID"].Value;
            TLogging.Log("~~ID Assigned~~ " + FID);

            FTitle = ShepherdPageNode.Attributes["Title"].Value;
            TLogging.Log("~~Title Assigned~~ " + FTitle);

            FNote = ShepherdPageNode.Attributes["Note"].Value;
            TLogging.Log("~~Note Assigned~~ " + FNote);

            FVisible = System.Convert.ToBoolean(ShepherdPageNode.Attributes["Visible"].Value);
            TLogging.Log("~~Visible Assigned~~ " + System.Convert.ToString(FVisible));

            FEnabled = System.Convert.ToBoolean(ShepherdPageNode.Attributes["Enabled"].Value);
            TLogging.Log("~~Enabled Assigned~~ " + System.Convert.ToString(FEnabled));

            FUserControlNamespace = ShepherdPageNode.Attributes["UserControlNamespace"].Value;
            TLogging.Log("~~UserControlNamespace Assigned~~ " + FUserControlNamespace);

            FUserControlClassName = ShepherdPageNode.Attributes["UserControlClassName"].Value;
            TLogging.Log("~~UserControlClassName Assigned~~ " + FUserControlClassName);

            FHelpContext = ShepherdPageNode.Attributes["HelpContext"].Value;
            TLogging.Log("~~HelpContext Assigned~~ " + FHelpContext);

            //FUserControlType = ShepherdPageNode.Attributes["UserControlType"].Value;
            //TLogging.Log("~~UserControlType Assigned~~ ");

/*
 *          FIsLastPage=System.Convert.ToBoolean(ShepherdPageNode.Attributes["IsLastPage"].Value);
 *          TLogging.Log("~~IsLastPage Assigned~~ " + System.Convert.ToString(FIsLastPage));
 *
 *          FIsFirstPage=System.Convert.ToBoolean(ShepherdPageNode.Attributes["IsFirstPage"].Value);
 *          TLogging.Log("~~IsFirstPage Assigned~~ " + System.Convert.ToString(FIsFirstPage));
 */
        }

        /// <summary>
        /// TODO: Function that instantiates and returns the UserControl needed for each Shepherd Page
        /// That user control will then be displayed in the content of each page
        /// </summary>
        /// <returns>UserControl</returns>
        private UserControl RealiseUserControl()
        {
            Assembly asm = Assembly.LoadFrom(FUserControlNamespace + ".dll");

            System.Type classType = asm.GetType(FUserControlNamespace + "." + FUserControlClassName);

            if (classType == null)
            {
                MessageBox.Show("TPnlCollapsible.RealiseUserControl: Cannot find class " + FUserControlNamespace + "." + FUserControlClassName);
            }

            FUserControl = (UserControl)Activator.CreateInstance(classType);

            //FUserControl.Controls.Add();

            TLogging.Log("PetraShepherdPage: The user controls have been realised.");

            return FUserControl;
        }
    }


    ///<summary>List of 0..n Shepherd Pages (holds data from YAML file)</summary>
    public class TPetraShepherdPagesList
    {
        ///<summary>Dictionary containing a list of TPetraShepherdPage using the page's Unique ID as an identifier</summary>
        Dictionary <string, TPetraShepherdPage>FPagesList = new Dictionary <string, TPetraShepherdPage>();

        /// <summary>
        /// Attribute of TPetraShepherdPage that allows for read only access to Dictionary of Pages.
        /// </summary>
        public Dictionary <string, TPetraShepherdPage>Pages
        {
            get
            {
                return FPagesList;
            }
        }

        /// <summary>
        /// Constructor for TPetraShepherdPagesList. This function reads in a yaml file from the appropriate
        /// namespace, parses it into xmlNodes, and adds them to the list of pages so that they can be read
        /// by the individual ShepherdPage constructor, which it calls.
        /// </summary>
        /// <param name="AYamlFile"></param>
        public TPetraShepherdPagesList(string AYamlFile)
        {
            TLogging.Log("Entering TPetraShepherdPagesList Constructor. AYamlFile = " + AYamlFile + "...");

            TYml2Xml parser = new TYml2Xml(AYamlFile);
            XmlDocument XmlPages = parser.ParseYML2XML();

            TLogging.Log("TPetraShepherdPagesList currently has this many nodes: " + XmlPages.LastChild.LastChild.LastChild.ChildNodes.Count);

            XmlNode temporaryXmlNode = XmlPages.LastChild.LastChild.LastChild.FirstChild;
            //...Required LastChild.LastChild.FirstChild because of the structure of the XML File after parsing.

            int counter = 0;
            //while(temporaryXmlNode != null) //temporarily commented while I try a foreach instead for this loop..
            //i've found that this loop only iterates through one element in XmlPages, and is therefore
            //useless
            //ckatpetra: In the constructor of TPetraShepherdPagesList you get data into the XmlPages variable. XmlPages is of Type XmlDocument.
            //ckatpetra: This can easily be saved to a file, see e.g. the code example here:
            // to read the YAML into an XML file, check out http://msdn.microsoft.com/en-us/library/dw229a22(v=VS.80).aspx

            XmlNodeList nodeList;
            XmlNode root = XmlPages.DocumentElement;
            nodeList = XmlPages.LastChild.LastChild.LastChild.ChildNodes;
            TLogging.Log("The amount of nodes in the nodeList in the TPetraShepherdPagesList constructor is as follows: " + nodeList.Count);

            foreach (XmlNode node in nodeList)
            {
                if (node.Name.Contains("SubShepherd."))
                {
                    TLogging.Log("TPetraSHepherdPagesList Contsructor loop: Found a sub shepherd.. Skipping.. ");
                }
                else
                {
                    TPetraShepherdPage temporaryPetraShepherdPage = new TPetraShepherdPage(node);
                    //Conrstuctor call for each page built off an XML node.

                    TLogging.Log("TPetraShepherdPagesList Constructor loop: THE TITLE OF THE CURRENT PAGE IS: " + temporaryPetraShepherdPage.Title);

                    FPagesList.Add(temporaryPetraShepherdPage.ID, temporaryPetraShepherdPage);
                }

                counter++;
            }

            TPetraShepherdFinishPage shepherdFinishPage = new TPetraShepherdFinishPage(XmlPages);
            TLogging.Log("Adding a shepherd finish page: " + shepherdFinishPage.ID);
            FPagesList.Add(shepherdFinishPage.ID, shepherdFinishPage);

            //Temporary Statement to add a subshepherd finish page in addition to the Finish page above
            TPetraShepherdFinishPage shepherdSubFinishPage = new TPetraShepherdFinishPage(XmlPages, "SubShepherd");
            TLogging.Log("Adding a shepherd sub-finish page: " + shepherdSubFinishPage.ID);
            FPagesList.Add(shepherdSubFinishPage.ID, shepherdSubFinishPage);


            TLogging.Log("TPetraShepherdPagesList Constructor ran successfully.");
        }
    }

    ///<summary>Specialisation of a Shepherd Page: Finish Page</summary>
    public class TPetraShepherdFinishPage : TPetraShepherdPage
    {
        /// <summary>
        /// Constructor for TPetraShepherdFinish page creation.
        /// </summary>
        public TPetraShepherdFinishPage(XmlDocument XmlPages)
        {
            base.ID = "FINISHPAGE_MASTER";
            Init(XmlPages);
        }

        public TPetraShepherdFinishPage(XmlDocument XmlPages, string SubShepherdName)
        {
            base.ID = "FINISHPAGE_CHILD_" + SubShepherdName.ToUpper();
            Init(XmlPages);
        }

        private void Init(XmlDocument XmlPages)
        {
            base.Enabled = true;
            base.Visible = true;
            base.FIsLastPage = false;
            base.FUserControlClassName = "TUC_PetraShepherdFinishPage";
            base.FUserControlNamespace = "Ict.Petra.Client.CommonForms";
            base.FTitle = "Here is a summary of the information you have provided:" + base.ID;
            base.Note = GetFinishPageNote(XmlPages);
        }

        protected string GetFinishPageNote(XmlDocument XmlPages)
        {
            XmlNode FileElementData = XmlPages.DocumentElement;

            FileElementData = XmlPages.LastChild.LastChild;

            if (FileElementData.Attributes["FinishPageNote"] != null)
            {
                TLogging.Log("VALUE OF FINISH PAGE NOTE: " + FileElementData.Attributes["FinishPageNote"].Value);
                return FileElementData.Attributes["FinishPageNote"].Value;
            }
            else
            {
                return "Choose \'Finish\' to commit the data.";
            }
        }
    }
}