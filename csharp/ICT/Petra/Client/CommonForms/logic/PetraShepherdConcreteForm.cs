//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK, tomn, pauln
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
using System;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.IO;
namespace Ict.Petra.Client.CommonForms.Logic
{
    ///<summary>Logic class for the internal behaviour of a Shepherd</summary>
    public class TPetraShepherdFormLogic : object
    {
        private const string STARTPAGE_NAME = "|||START PAGE|||";
        private const string FINISHPAGE_NAME = "FINISHPAGE_MASTER";

        /// <summary>Holds a typed list of 0..n TPetraShepherdPage's</summary>
        private TPetraShepherdPagesList FShepherdPages;
        /// <summary>Holds the instance of the Shepherd Form management class</summary>
        private IPetraShepherdConcreteFormInterface FForm;
        /// <summary>Holds the instance of the current shepherd page</summary>
        private TPetraShepherdPage FCurrentPage;
//        /// <summary>'Blackboard' for exchanging data between shepherd pages which isn't stored in the DB</summary>
//        private SortedList FPagesDataHeap;   // TODO

        ///<summary>List of Shepherd Pages</summary>
        public TPetraShepherdPagesList ShepherdPages
        {
            get
            {
                TLogging.Log("TPetraShepherdPagesList called get method in the attribute.");
                return FShepherdPages;
            }
        }

        ///<summary>Currently displayed Shepherd Page</summary>
        public TPetraShepherdPage CurrentPage
        {
            get
            {
                return FCurrentPage;
            }
            set
            {
                FCurrentPage = value;
            }
        }

        ///<summary>Constructor for the default logic behind all Shepherds.</summary>
        public TPetraShepherdFormLogic(string AYamlFile, IPetraShepherdConcreteFormInterface APetraShepherdForm)
        {
            TLogging.Log(
                "Entering TPetraShepherdFormLogic Constructor. AYamlFile = " + AYamlFile + "; APetraShepherdForm = " +
                APetraShepherdForm.ToString() +
                "...");

            FForm = APetraShepherdForm;

            // Take AYamlFile and parse it into an XmlNode structure

            ParseYAMLFileElements(AYamlFile);

            FShepherdPages = new TPetraShepherdPagesList(AYamlFile);

            SwitchToStartPage();

            TLogging.Log("The TPetraShepherdFormLogic constructor has switched to the first page.");

            // Iterate over all FPetraShepherdPages and add the VisibleOrEnabledChangedEventHandler

            // FShepherdPages needs to get added an auto-generated TPetraShepherdFinishPage
            // for the Finish Page (that is not specified in the YAML file!)
            // Note: That Finish Page (and only this) will have IsLastPage = true!!!


            TLogging.Log(
                "TPetraShepherdFormLogic Constructor ran and returned to the TPetraShepherdFormLogic constructor in PetraShepherdConcreteForm.");
        }

        /// <summary>
        /// Returns an XML node that defines a number of the properties of each Shepherd. Including size and title.
        /// </summary>
        /// <param name="AYamlFile"></param>
        /// <returns></returns>
        protected XmlNode ParseYAMLFileElements(string AYamlFile)
        {
            TLogging.Log("ParseYAMLFileElements method starting.");
            TYml2Xml parser = new TYml2Xml(AYamlFile);
            XmlDocument XmlPages = parser.ParseYML2XML();

            TLogging.Log("ParseYAMLFileElements currently has this many attributes: " + XmlPages.LastChild.LastChild.Attributes.Count);

            XmlNode FileElementData = XmlPages.DocumentElement;

            FileElementData = XmlPages.LastChild.LastChild;

            string ShepherdHeight = "";
            string ShepherdWidth = "";
            string ShepherdTitle = "";
            string TestElement = "";
            string FinishPageNote = "";

            #region YAML Attributes Input

            if (FileElementData.Attributes["FinishPageNote"] != null)
            {
                FinishPageNote = FileElementData.Attributes["FinishPageNote"].Value;
            }
            else
            {
                TLogging.Log("DID NOT FIND FINISH PAGE");
            }

            if (FileElementData.Attributes["Testelement"] != null)
            {
                TLogging.Log("FOUND TEST ELEMENT");
                TLogging.Log("Printing the value of test: " + FileElementData.Attributes["Testelement"].Value);
                TestElement = FileElementData.Attributes["Testelement"].Value;
            }
            else
            {
                TLogging.Log("Did not find a test element for this shepherd.");
            }

            if (FileElementData.Attributes["Width"] != null)
            {
                TLogging.Log("Printing the width of shepherd: " + FileElementData.Attributes["Width"].Value);
                ShepherdWidth = FileElementData.Attributes["Width"].Value;
            }
            else
            {
                TLogging.Log("Did not find a width for this shepherd.");
            }

            if (FileElementData.Attributes["Height"] != null)
            {
                TLogging.Log("Printing the height of shepherd: " + FileElementData.Attributes["Height"].Value);
                ShepherdHeight = FileElementData.Attributes["Height"].Value;
            }
            else
            {
                TLogging.Log("Did not find a height for this shepherd.");
            }

            if (FileElementData.Attributes["Title"] != null)
            {
                TLogging.Log("Printing the title of shepherd: " + FileElementData.Attributes["Title"].Value);
                ShepherdTitle = FileElementData.Attributes["Title"].Value;
            }
            else
            {
                TLogging.Log("Did not find a title for this shepherd.");
            }

            #endregion

            try
            {
                FForm.UpdateShepherdFormProperties(ShepherdTitle,
                    Convert.ToInt32(ShepherdWidth),
                    Convert.ToInt32(ShepherdHeight));
            }
            catch (FormatException)
            {
                TLogging.Log("An element (height or width) cannot be converted to integer. Check the datatype and try again.");
            }

            return FileElementData;
        }

        ///<summary>Returns an instance of a Page UserControl</summary>
        protected UserControl InstantiatePageUC(string AID)
        {
            TLogging.Log("SwitchToPage (in TPetraShepherdFormLogic) was called for Page UserControl '" + AID + "'");

            return null;
        }

        ///<summary>Switches the current page</summary>
        protected void SwitchToPage(string APage)
        {
            TLogging.Log("SwitchToPage (in TPetraShepherdFormLogic) was called for Page '" + APage + "'");
            // ....
            CurrentPage = FShepherdPages.Pages[APage];
            TLogging.Log("PetraShepherdConcreteForm: SwitchToPage -- Page number = " + CurrentPage.ID);
            TLogging.Log("The current Total number of pages is = " + EnumeratePages());
            TLogging.Log("The percentage of pages = " + GetProgressBarPercentage());

            try
            {
                FForm.ShowCurrentPage();
            }
            catch (Exception e)
            {
                //This line always throws an exception during the first run of the code;
                //I don't know if that's okay, but we should probably resolve it.
                TLogging.Log("The exception for ShowCurrentPage() in PetraShepherdConcreteForm was caught.");
                TLogging.Log(e.Message);
            }
        }

        /// <summary>Returns the total number of pages in the Shepherd</summary>
        /// <returns>Total Number of Shepherd Pages</returns>
        public int EnumeratePages()
        {
            TLogging.Log("Enumerate Pages in TPetraShepherdFormLogic -- Counting the total number of pages.");
            int PagesCount = 0;

            foreach (KeyValuePair <string, TPetraShepherdPage>pair in FShepherdPages.Pages)
            {
                PagesCount++;
            }

            TLogging.Log("EnumeratePages in TPetraShepherdFormLogic -- Count of Pages = " + PagesCount);
            return PagesCount;
        }

        /// <summary>Iterates through the list of pages to find out which page number the current page is.</summary>
        /// <returns>Page Number</returns>
        public int GetCurrentPageNumber()
        {
            TLogging.Log("GetCurrentPageNumber() in TPetraShepherdConcreteForm.. ");
            int pageCounter = 1;

            foreach (KeyValuePair <string, TPetraShepherdPage>pair in FShepherdPages.Pages)
            {
                TLogging.Log("GetCurrentPageNumber loop. Pair.key: " + pair.Key);
                TLogging.Log("GetCurrentPageNumber loop. CurrentPage.ID: " + CurrentPage.ID);

                if (pair.Value.Visible && pair.Value.Enabled && (pair.Key == CurrentPage.ID))
                {
                    TLogging.Log("GetCurrentPageNumber Found the current page: " + pair.Key);
                    break;
                }

                pageCounter++;
            }

            TLogging.Log("GetCurrentPageNumber() -- Returning the following value." + pageCounter);
            return pageCounter;
        }

        /// <summary>
        /// Calculates the percentage of all of the pages that have been passed in the Shepherd.
        /// </summary>
        /// <returns>Percentage of Pages</returns>
        public float GetProgressBarPercentage()
        {
//            TPetraShepherdPage ProgressPage = null;
            float ProgressPercentage = 0;

            ProgressPercentage = ((float)GetCurrentPageNumber() / (float)EnumeratePages() * 100);
            TLogging.Log("GetProgredsBarPercentage returns the following: " + ProgressPercentage);
            return ProgressPercentage;
        }

        /// <summary>
        /// Switches to the first page
        /// Iterates through FShepeherdPages.Pages to find the first page that is both visible and enabled.
        /// </summary>
        public void SwitchToStartPage()
        {
            TLogging.Log("SwitchToStartPage (in TPetrashepherdFormLogic)");

            string startPage = "";             //temporary string to hold the key of the StartPage

            foreach (KeyValuePair <string, TPetraShepherdPage>pair in FShepherdPages.Pages)
            {
                if (pair.Value.Visible && pair.Value.Enabled)
                {
                    TLogging.Log("SwitchToStartPage foreach loop returned the following value that was both visible and enabled: " + pair.Key);
                    startPage = pair.Key;
                    pair.Value.IsFirstPage = true;
                    CurrentPage = pair.Value;
                    break;
                }
            }

            TLogging.Log("temporary page was assigned to " + startPage + " in SwitchToStartPage.");
            try
            {
                SwitchToPage(startPage);
            }
            catch (KeyNotFoundException)
            {
                TLogging.Log("KeyNotFoundException Thrown in SwitchToStartPage when SwitchToPage(startPage) was called.");
            }
        }

        ///<summary>Switches the Finish page</summary>
        public void SwitchToFinishPage()
        {
            TLogging.Log("SwitchToFinishPage (in TPetraShepherdFormLogic) called to: " + FINISHPAGE_NAME);

            SwitchToPage(FINISHPAGE_NAME);
        }

        ///<summary>Switches to the 'next' page (whatever page this is)</summary>
        public virtual void HandleActionNext()
        {
            TLogging.Log("HandleActionNext (in TPetraShepherdFormLogic)");

            string nextPage = "";     //temporary string to hold the key of the StartPage
            bool hasPassedCurrentPage = false;     // used to tell if the iteration has already checked to see if you have passed the current page.
            bool hasPassedItAgain = false;

            foreach (KeyValuePair <string, TPetraShepherdPage>pair in FShepherdPages.Pages)
            {
                if (hasPassedCurrentPage)         //TODO: there has to be a better way to handle iterating through the loop one more time; it works now, but is ugly.
                {
                    hasPassedItAgain = true;
                }

                if (pair.Key == CurrentPage.ID)
                {
                    TLogging.Log("Found the equivilance.");
                    hasPassedCurrentPage = true;
                    TLogging.Log("Set the hasPassedCurrentPage bool to true.");
                }

                if (pair.Value.Visible && pair.Value.Enabled && hasPassedItAgain)
                {
                    TLogging.Log("Switchtonextpage foreach loop returned the following value that was both visible and enabled: " + pair.Key);
                    nextPage = pair.Key;
                    CurrentPage = pair.Value;
                    break;
                }
            }

            TLogging.Log("temporary next page was assigned to " + nextPage + " in HandleActionNext().");

            try // rather than a try/catch statement, the next button should instead be greyed out
            {
                SwitchToPage(nextPage);
            }
            catch (KeyNotFoundException)
            {
                TLogging.Log("KeyNotFoundException Thrown at HandleActionNext when SwitchToPage(nextPage) was called.");
            }
        }

        ///<summary>Switches to the 'previous' page (whatever page this is)</summary>
        public virtual void HandleActionBack() //TODO: The handleActionBack method has an edge case that I can't figure out quite yet -- when only two pages are visible and enabled, hitting the back button repeatedly cycles through the two pages..
                                               // :-/
        {
            TLogging.Log("HandleActionBack (in TPetraShepherdFormLogic)");

            string backPage = ""; //temporary string to hold the key of the StartPage
            TPetraShepherdPage temporaryPage = CurrentPage;
            int counter = 0;

            if (CurrentPage.IsFirstPage)
            {
                backPage = CurrentPage.ID;
            }
            else
            {
                foreach (KeyValuePair <string, TPetraShepherdPage>pair in FShepherdPages.Pages)
                {
                    if ((pair.Value == CurrentPage) && pair.Value.Enabled && pair.Value.Visible)
                    {
                        backPage = temporaryPage.ID;
                        
                        TLogging.Log("Set the backpage to the following: " + temporaryPage.ID);
                        
                        break;
                    }

                    temporaryPage = pair.Value;
                    counter++;
                }
            }

            backPage = temporaryPage.ID;
            SwitchToPage(backPage);
        }

        ///<summary>Causes to close the Shepherd without saving if the user chooses to do that</summary>
        public void HandleActionCancel()
        {
            TLogging.Log("HandleActionCancel");
            // Show Message Box with Yes, No, Cancel options

            // If YES ->
            FForm.CancelShepherd();
        }

        ///<summary>Shows the context-sensitive help file content for the current page, if available, otherwise for the Shepherd as a whole</summary>
        public void HandleActionHelp()
        {
            TLogging.Log("HandleActionHelp");
        }

        ///<summary>Switches to the auto-generated 'Finish' page.</summary>
        public void HandleActionFinish()
        {
            TLogging.Log("HandleActionFinish");
            // .....

            SwitchToFinishPage();
        }

        ///<summary>Determines the 'First' page (whatever page this is).</summary>
        private void VisibleOrEnabledChangedEventHandler()
        {
            TLogging.Log("VisibleOrEnabledChangedEventHandler");
            // re-enumerate FShepherdPages!

            FForm.UpdateNavigation();
        }

        /// <summary>Looks up a key in the PagesDataHeap and returns its value, or null if it doesn't exist</summary>
        public string PagesDataHeapPeek(string AKey)
        {
            TLogging.Log("VisibleOrEnabledChangedEventHandler");

            return String.Empty;
        }

        /// <summary>Sets a key-value pair in the PagesDataHeap</summary>
        public void PagesDataHeapPoke(string AKey, string AValue)
        {
            TLogging.Log("PagesDataHeapPoke");
        }

        /// <summary>
        /// Creates XmlNodes for the Task List.
        /// </summary>
        /// <returns></returns>
        public XmlNode CreateTaskList()
        {
//              TLogging.Log("Starting method CreateTaskList!");

            // Create the xml document container
            XmlDocument XMLDocumentOfActivePages = TYml2Xml.CreateXmlDocument();
            XmlNode root = XMLDocumentOfActivePages.FirstChild.NextSibling;

            // Create 'ShepherdPages' element (which serves as 'our' root element)
            XmlElement ShepherdPages = root.OwnerDocument.CreateElement("ShepherdPages");             //<ShepherdPages>
            XmlNode ShepherdPagesNode = root.AppendChild(ShepherdPages);

            int PageCounter = 1;

            // TODO: Sub-Shepherds
            foreach (KeyValuePair <string, TPetraShepherdPage>pair in FShepherdPages.Pages)
            {
                XmlElement ID = ShepherdPagesNode.OwnerDocument.CreateElement("Page" + PageCounter.ToString());                 //<ID>
                XmlNode IDNode = ShepherdPagesNode.AppendChild(ID);

                // Label Attribute
                XmlAttribute LabelAttribute = ShepherdPagesNode.OwnerDocument.CreateAttribute("Label");
                IDNode.Attributes.Append(LabelAttribute);
                IDNode.Attributes["Label"].Value = pair.Value.Title;

                // Visible Attribute
                if (!(pair.Value.Visible))
                {
                    XmlAttribute VisibleAttribute = ShepherdPagesNode.OwnerDocument.CreateAttribute("Visible");
                    IDNode.Attributes.Append(VisibleAttribute);
                    IDNode.Attributes["Visible"].Value = "False";
                }

                // Enabled Attribute
                if (!(pair.Value.Enabled))
                {
                    XmlAttribute EnabledAttribute = ShepherdPagesNode.OwnerDocument.CreateAttribute("Enabled");
                    IDNode.Attributes.Append(EnabledAttribute);
                    IDNode.Attributes["Enabled"].Value = "False";
                }

                PageCounter++;
            }

            XmlNode firstPage = root.FirstChild;

// For debugging only
//			TLogging.Log("Count of child nodes: " + firstPage.ChildNodes.Count);
//			XmlNodeList PageAttributes = firstPage.ChildNodes;
//			int counter = 0;
//			foreach(XmlNode node in PageAttributes)
//			{
//				foreach(XmlNode attributeNode in node.ChildNodes)
//				{
//
//					TLogging.Log("Foreach Node Value: " + attributeNode.InnerText);
//
//				}
//				TLogging.Log("Inner foreach: " + counter);
//				counter++;
//			}
//			TLogging.Log("FIRST CHILD NAME: " + root.FirstChild.FirstChild.FirstChild.NextSibling.InnerText);
//			ChildDisplay(firstPage,0);

            return firstPage;
        }

        /// <summary>
        /// This Method is only for debugging the TaskList nodes.
        /// </summary>
        /// <param name="xnod"></param>
        /// <param name="level"></param>
        private static void ChildDisplay(XmlNode xnod, int level)
        {
            XmlNode xnodWorking;
            String pad = new String(' ', level * 2);

            TLogging.Log(pad + xnod.Name + "(" + xnod.NodeType.ToString() + ": <" + xnod.Value + ">)");

            if (xnod.NodeType == XmlNodeType.Element)
            {
                XmlNamedNodeMap mapAttributes = xnod.Attributes;

                for (int i = 0; i < mapAttributes.Count; i++)
                {
                    TLogging.Log(pad + " " + mapAttributes.Item(i).Name + " = " + mapAttributes.Item(i).Value);
                }
            }

            if (xnod.HasChildNodes)
            {
                xnodWorking = xnod.FirstChild;

                while (xnodWorking != null)
                {
                    ChildDisplay(xnodWorking, level + 1);
                    xnodWorking = xnodWorking.NextSibling;
                }
            }
        }
    }
}