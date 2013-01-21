//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2012 by OM International
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
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

using Ict.Common;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Determines the Application Help Topic of a Form or of a Control on a Form.
    /// </summary>
    public static class THelpContext
    {
        private static bool FHelpTopicsLookupTableLoaded = false;
        private static XPathNavigator FXMLFileNavigation;

        /// <summary>
        /// Delegate that retrieves the help topic.
        /// </summary>
        /// <param name="ASender"></param>
        /// <param name="AEventArgs"></param>
        /// <returns></returns>
        public static string DetermineHelpTopic(System.Object ASender, Ict.Common.HelpLauncher.TDetermineHelpTopicEventArgs AEventArgs)
        {
            string ReturnValue = String.Empty;
            Form HelpContextForm;
            Control HelpContextControl;
            Cursor FormCursorAtTimeOfCall = null;
            string HelpContext = String.Empty;
            string HelpContextForControl = String.Empty;
            bool HelpTopicFoundForControl = false;

            if (AEventArgs == null)
            {
                throw new ArgumentException("AEventArgs must not be null");
            }

            if (AEventArgs.HelpContextForm == null)
            {
                throw new ArgumentException("The HelpContextForm needs to be specified in the AEventArgs, but it isn't.");
            }
            else
            {
                HelpContextForm = AEventArgs.HelpContextForm;
            }

            // Show Hourglass Cursor
            FormCursorAtTimeOfCall = ((System.Windows.Forms.Form)HelpContextForm).Cursor;
            ((System.Windows.Forms.Form)HelpContextForm).Cursor = Cursors.WaitCursor;

            // Load help topics once and keep it cached from then on.
            if (!FHelpTopicsLookupTableLoaded)
            {
                LoadHelpTopicsLookupTable();
            }

            HelpContextControl = AEventArgs.HelpContextControl;

            HelpContext = HelpContextForm.GetType().Name;

            if (HelpContextControl != null)
            {
                while (!HelpTopicFoundForControl)
                {
                    // We don't support help on TableLayoutPanels as this is too granular, rather use the Parent control of a TableLayoutPanel
                    if (HelpContextControl is TableLayoutPanel)
                    {
                        HelpContextControl = HelpContextControl.Parent;
                    }

                    HelpContextForControl = HelpTopicForHelpContextControl(HelpContextControl);

                    if (HelpContextForControl != String.Empty)
                    {
                        HelpTopicFoundForControl = true;
                    }
                    else
                    {
                        // No help topic found for Control --> go one up in the Control hierarachy and repeat the loop
                        HelpContextControl = HelpContextControl.Parent;

                        if (HelpContextControl is System.Windows.Forms.Form)
                        {
                            HelpContextForControl = HelpTopicForHelpContextForm(HelpContextControl.GetType().Name);
                            break;
                        }
                    }
                }

                ReturnValue = HelpContextForControl;
            }
            else
            {
                // We have only got a Form and no Control --> Return the Form's Help Topic.
                ReturnValue = HelpTopicForHelpContextForm(HelpContext);
            }

            // Reset to originally shown Cursor
            ((System.Windows.Forms.Form)HelpContextForm).Cursor = FormCursorAtTimeOfCall;

            return ReturnValue;
        }

        /// <summary>
        /// Retrieves the Help Topic associated with a certain Control on a Form.
        /// </summary>
        /// <param name="AControl">Control.</param>
        /// <returns>Help Topic URL part if there is a Help Topic associated with the Control, otherwise String.Empty.</returns>
        private static string HelpTopicForHelpContextControl(Control AControl)
        {
            string ReturnValue = String.Empty;
            string HelpContext = AControl.FindForm().GetType().Name + "." + AControl.Name;

            XPathExpression expr = FXMLFileNavigation.Compile("/pageGuide/page[@context='" + HelpContext + "']/link");
            XPathNodeIterator iterator = FXMLFileNavigation.Select(expr);

            while (iterator.MoveNext())
            {
                XPathNavigator nav2 = iterator.Current.Clone();
                ReturnValue = nav2.Value;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Retrieves the Help Topic associated with a Form.
        /// </summary>
        /// <param name="AFormTypeName">Type of the Form as a string.</param>
        /// <returns>Help Topic URL part if there is a Help Topic associated with the Form, otherwise String.Empty.</returns>
        private static string HelpTopicForHelpContextForm(string AFormTypeName)
        {
            string ReturnValue = String.Empty;

            XPathExpression expr = FXMLFileNavigation.Compile("/pageGuide/page[@context='" + AFormTypeName + "']/link");
            XPathNodeIterator iterator = FXMLFileNavigation.Select(expr);

            while (iterator.MoveNext())
            {
                XPathNavigator nav2 = iterator.Current.Clone();
                ReturnValue = nav2.Value;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Loads the Form/Control and Help Topic associations from an XML file.
        /// </summary>
        /// <remarks>XML file is loaded only once and then remains cached.</remarks>
        private static void LoadHelpTopicsLookupTable()
        {
            const string HELPTOPICS_XML_FILENAME = "HelpTopics.xml";

            FHelpTopicsLookupTableLoaded = true;

            // The XML File that holds the Form/Control and Help Topic associations is found where
            // the UINavigation.yml file is found, too.
            string Tmp = TAppSettingsManager.GetValue("UINavigation.File");
            Tmp = Tmp.Substring(0, Tmp.LastIndexOf("/") + 1);
            string HelpTopicsXMLFileName = Tmp + HELPTOPICS_XML_FILENAME;

            // Load the XML File and prepare it for XPath Queries
            FXMLFileNavigation = new XPathDocument(HelpTopicsXMLFileName).CreateNavigator();
        }
    }
}