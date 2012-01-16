//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
        private static XPathDocument doc;
        private static XPathNavigator nav;

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
            if (ASender is System.Windows.Forms.Form)
            {
                FormCursorAtTimeOfCall = ((System.Windows.Forms.Form)ASender).Cursor;
                ((System.Windows.Forms.Form)ASender).Cursor = Cursors.WaitCursor;
            }

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
            if (ASender is System.Windows.Forms.Form)
            {
                ((System.Windows.Forms.Form)ASender).Cursor = FormCursorAtTimeOfCall;
            }

            return ReturnValue;
        }

        private static string HelpTopicForHelpContextControl(Control AControl)
        {
            string ReturnValue = String.Empty;
            string HelpContext = AControl.FindForm().GetType().Name + "." + AControl.Name;

            // TODO: Look up help topic in an XML file instead of using the hard-coded if-statements, which are only for experimenting.
            if (HelpContext == "TFrmSetupCurrency.pnlDetails")
            {
                ReturnValue = "scroll-bookmark-14.html#scroll-bookmark-17";
            }

            if (HelpContext == "TFrmSetupCurrency.pnlButtons")
            {
                ReturnValue = "scroll-bookmark-3.html";
            }

            if (HelpContext == "TFrmGiftBatch.ucoTransactions")
            {
                ReturnValue = "scroll-bookmark-7.html#scroll-bookmark-9";
            }

            return ReturnValue;
        }

        private static string HelpTopicForHelpContextForm(string AFormTypeName)
        {
            string ReturnValue = String.Empty;

            // TODO: Look up help topic in an XML file instead of using the hard-coded if-statements, which are only for experimenting.
            if (AFormTypeName == "TLoginForm")
            {
                ReturnValue = "scroll-bookmark-14.html#scroll-bookmark-17";
            }

            if (AFormTypeName == "TFrmSetupCurrency")
            {
                ReturnValue = "scroll-bookmark-3.html#scroll-bookmark-5";
            }

            if (AFormTypeName == "TFrmGiftBatch")
            {
                ReturnValue = "scroll-bookmark-7.html#scroll-bookmark-8";
            }

            return ReturnValue;
        }

        private static void LoadHelpTopicsLookupTable()
        {
            FHelpTopicsLookupTableLoaded = true;

            MessageBox.Show("Loading the Form/Control and Help Topic associations from an XML file...");

            // TODO: Load the Form/Control and Help Topic associations from an XML file!

//            string fileName = "data.xml";
//            doc = new XPathDocument(fileName);
//            nav = doc.CreateNavigator();
//            ...
        }
    }
}