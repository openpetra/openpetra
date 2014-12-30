//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// A specialised RichTextBox that supports arbitrary 'Hyperlinks'. It supports multi-line text
    /// and a mix of plain text and 'hyperlinks'.
    /// </summary>
    /// <remarks>If one 'Hyperlink' is to be followed by another 'Hyperlink' without any
    /// plain text inbetween then valid separators are ',' (comma), ';' (semi-colon) and
    /// Environment.NewLine.
    /// </remarks>
    public partial class TRtbHyperlinks : UserControl
    {
        private const string FFontAppearanceNormalRTFCode = @"\b\f0\fs17";
        private const string FFontAppearanceSmallRTFCode = @"\f0\fs14";

        private List <string>SupportedLinkTypes = new List <string>();
        private SortedDictionary <int, string>FLinkPrefixes = new SortedDictionary <int, string>();
        private SortedDictionary <int, int>FLinkRanges = new SortedDictionary <int, int>();
        private DisplayHelper FDisplayHelper;

        private bool FUseSmallTextFont = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TRtbHyperlinks()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion

            FDisplayHelper = new DisplayHelper(this);

            SupportedLinkTypes.Add(THyperLinkHandling.HYPERLINK_PREFIX_EMAILLINK);
            SupportedLinkTypes.Add(THyperLinkHandling.HYPERLINK_PREFIX_URLLINK);
            SupportedLinkTypes.Add(THyperLinkHandling.HYPERLINK_PREFIX_URLWITHVALUELINK);
            SupportedLinkTypes.Add(THyperLinkHandling.HYPERLINK_PREFIX_SECUREDURL);
            SupportedLinkTypes.Add(THyperLinkHandling.HYPERLINK_PREFIX_FTPLINK);
            SupportedLinkTypes.Add(THyperLinkHandling.HYPERLINK_PREFIX_SKYPELINK);

            PlainRTFFormatting(rtbTextWithLinks);
            WriteLinkRTF(rtbTextWithLinks);

            rtbTextWithLinks.SelectionStart = 0;
            rtbTextWithLinks.DetectUrls = false;

            rtbTextWithLinks.Click += new System.EventHandler(rtbTextWithLinks_Click);
            rtbTextWithLinks.MouseMove += new System.Windows.Forms.MouseEventHandler(rtbTextWithLinks_MouseMove);
        }

        #region Delegates

        /// <summary>
        /// Delegate for building a Link where the Text of this Control is part of a hyperlink rather than it being the full hyperlink.
        /// </summary>
        public Func <string, int, string>BuildLinkWithValue;

        #endregion

        #region Properties

        /// <summary>
        /// Helper Class for handling the displaying, parsing and execution of certain types of HyperLinks
        /// </summary>
        public DisplayHelper Helper
        {
            get
            {
                return FDisplayHelper;
            }
        }

        /// <summary>
        /// Sets the Text of the RichTextBox. Include keywords that are reserved for the Link Types
        /// to mark up text passages that should be rendered as Hyperlinks.
        /// </summary>
        public new string Text
        {
            get
            {
                return rtbTextWithLinks.Text;
            }

            set
            {
                rtbTextWithLinks.Text = value;
                FLinkPrefixes.Clear();
                FLinkRanges.Clear();

                PlainRTFFormatting(rtbTextWithLinks);
                WriteLinkRTF(rtbTextWithLinks);
            }
        }

        /// <summary>
        /// Set to true to display the text not in the standard OpenPetra Text size, but smaller.
        /// </summary>
        /// <remarks>Set to true eg. for the Partner Info Panel (part of the Partner Find screen).</remarks>
        public bool UseSmallTextFont
        {
            get
            {
                return FUseSmallTextFont;
            }

            set
            {
                FUseSmallTextFont = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="BorderStyle"/> of this Control.
        /// </summary>
        public new BorderStyle BorderStyle
        {
            get
            {
                return rtbTextWithLinks.BorderStyle;
            }

            set
            {
                base.BorderStyle = value;
                rtbTextWithLinks.BorderStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="BackColor"/> of this Control.
        /// </summary>
        public new Color BackColor
        {
            get
            {
                return rtbTextWithLinks.BackColor;
            }

            set
            {
                base.BackColor = value;
                rtbTextWithLinks.BackColor = value;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Contains data about a Link that got clicked by the user.
        /// </summary>
        public delegate void THyperLinkClickedArgs(string ALinkText, string ALinkType, int ALinkEnd);

        /// <summary>Fired when a Link got clicked.</summary>
        public event THyperLinkClickedArgs LinkClicked;

        #endregion

        #region Private Methods

        private void PlainRTFFormatting(RichTextBox ARTFBox)
        {
            string text = ARTFBox.Text;
            string RTFInitStr = @"{\rtf1\ansi\deff0{\fonttbl{\f0\fnil\fcharset0 Verdana;}}" +
                                @"{\colortbl;\red0\green0\blue255;}" +
                                @"\viewkind4\uc1\pard\lang1033" +
                                (FUseSmallTextFont ? FFontAppearanceSmallRTFCode : FFontAppearanceNormalRTFCode) +
                                @"\par}";

            ARTFBox.Rtf = RTFInitStr;

//MessageBox.Show("ARTFBox.Text: " + text);
            string[] TextLines = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // Ensure proper CR+LF handling
            for (int Counter = 0; Counter < TextLines.Length; Counter++)
            {
                ARTFBox.AppendText(TextLines[Counter] + (Counter == TextLines.Length - 1 ? "" : "\u2028"));
            }
        }

        private void WriteLinkRTF(RichTextBox ARTFBox)
        {
            string RtfSelectionReplacementStart =
                @"{\rtf1\ansi\ansicpg1252\deff0{\fonttbl{\f0\fnil\fcharset0 Verdana;}}{\colortbl ;\red0\green0\blue255;}\uc1\pard\lang1033" +
                (FUseSmallTextFont ? FFontAppearanceSmallRTFCode : FFontAppearanceNormalRTFCode);
            const string RtfSelectionReplacementEnd = "}";

            int TextPos = 0;
            int FoundPos;
            int NextFoundPos = 0;
            string Tmp;
            string TmpRepl;

//            int NewlinePos;

//MessageBox.Show(ARTFBox.Text);
            foreach (string SupportedLinkType in SupportedLinkTypes)
            {
                FoundPos = ARTFBox.Find(SupportedLinkType);

                while (FoundPos > -1)
                {
                    TextPos = ARTFBox.Find(new char[] { ',', ';' }, FoundPos);

                    if (TextPos == -1)
                    {
                        for (int CounterForward = FoundPos; CounterForward < ARTFBox.Text.Length; CounterForward++)
                        {
//                            int AsciiCode = Convert.ToInt32(ARTFBox.Text[CounterForward]);
                            if (Convert.ToInt32(ARTFBox.Text[CounterForward]) == 10)
                            {
                                TextPos = CounterForward;
                                break;
                            }
                        }

                        if (TextPos == -1)
                        {
                            TextPos = ARTFBox.Text.Length;
                        }
                    }
                    else
                    {
                        for (int CounterBackwards = TextPos; CounterBackwards > FoundPos; CounterBackwards--)
                        {
                            char TmpChar = ARTFBox.Text[CounterBackwards];
//                            int TmpCharAscii = Convert.ToInt32(TmpChar);
//                            NewlinePos = ARTFBox.Text.IndexOf("\\n", 0);

                            if (Convert.ToInt32(ARTFBox.Text[CounterBackwards]) == 10)
                            {
                                TextPos = CounterBackwards;
                                break;
                            }
                        }
                    }

                    ARTFBox.SelectionStart = FoundPos;
                    ARTFBox.SelectionLength = TextPos - FoundPos;

                    Tmp = @"\cf1 \ul " + ARTFBox.Text.Substring(FoundPos + SupportedLinkType.Length,
                        ARTFBox.SelectionLength - SupportedLinkType.Length) + @"\cf0\ulnone ";
//                    MessageBox.Show("Tmp: " + Tmp);

                    TmpRepl = RtfSelectionReplacementStart + Tmp + RtfSelectionReplacementEnd;
                    ARTFBox.SelectedRtf = TmpRepl;

                    if ((NextFoundPos == 0)
                        || (NextFoundPos == -1))
                    {
                        NextFoundPos = FoundPos;
                    }

                    FLinkPrefixes.Add(ARTFBox.SelectionStart, SupportedLinkType);

                    if (TextPos != ARTFBox.Text.Length)
                    {
                        FoundPos = ARTFBox.Find(SupportedLinkType, TextPos - SupportedLinkType.Length, ARTFBox.Text.Length, RichTextBoxFinds.None);

                        FLinkRanges.Add(NextFoundPos, TextPos - SupportedLinkType.Length);
                        NextFoundPos = FoundPos;
                    }
                    else
                    {
                        // set abort condition
                        FoundPos = -1;

                        FLinkRanges.Add(NextFoundPos, ARTFBox.Text.Length);
                    }
                }
            }
        }

        private void OnLinkClicked(string ALinkText, string ALinkType, int ALinkEnd)
        {
            if (LinkClicked != null)
            {
                LinkClicked(ALinkText, ALinkType, ALinkEnd);
            }
        }

        #endregion

        #region Event Handlers

        void rtbTextWithLinks_Click(object sender, EventArgs e)
        {
            int SelectionStartAtBeginning = rtbTextWithLinks.SelectionStart;
            int LinkBegin = -1;
            int LinkEnd = 0;
            string LinkText = String.Empty;
            bool AbortSearch = false;
            string LinkType;
            int CheckedCharacters = 0;

            if (rtbTextWithLinks.SelectionLength != 0)
            {
                // If the user clicked to select text then we don't want to process a Link!
                return;
            }

            if (rtbTextWithLinks.SelectionStart == rtbTextWithLinks.Text.Length)
            {
                // If the user clicked after the text then we don't want to process a Link!
                return;
            }

//            MessageBox.Show(rtbTextWithLinks.SelectionStart.ToString());

            while (rtbTextWithLinks.SelectionFont.Underline
                   && !AbortSearch)
            {
                if (rtbTextWithLinks.SelectionStart > 0)
                {
                    if (Convert.ToInt32(rtbTextWithLinks.Text[rtbTextWithLinks.SelectionStart]) == 10)
                    {
                        if (CheckedCharacters == 0)
                        {
                            LinkBegin = -1;

                            break;
                        }

                        if (CheckedCharacters == 1)
                        {
                            rtbTextWithLinks.SelectionStart++;
                            LinkBegin = -1;

                            break;
                        }

                        rtbTextWithLinks.SelectionStart++;
                        LinkBegin = rtbTextWithLinks.SelectionStart;

                        AbortSearch = true;
                    }
                    else
                    {
                        rtbTextWithLinks.SelectionStart--;
                        LinkBegin = rtbTextWithLinks.SelectionStart;
                        CheckedCharacters++;
                    }
                }
                else
                {
                    AbortSearch = true;
                }
            }

            if (LinkBegin > -1)
            {
                rtbTextWithLinks.SelectionStart++;

                while (rtbTextWithLinks.SelectionFont.Underline && LinkEnd < rtbTextWithLinks.Text.Length)
                {
                    rtbTextWithLinks.SelectionStart++;
                    LinkEnd = rtbTextWithLinks.SelectionStart;

                    if ((LinkEnd == rtbTextWithLinks.Text.Length)
                        || (Convert.ToInt32(rtbTextWithLinks.Text[rtbTextWithLinks.SelectionStart]) == 10))
                    {
                        LinkEnd = rtbTextWithLinks.SelectionStart + 1;

                        break;
                    }
                }

                if (LinkEnd != 0)
                {
                    if (LinkEnd == rtbTextWithLinks.Text.Length)
                    {
                        LinkEnd++;
                    }

                    LinkText = rtbTextWithLinks.Text.Substring(LinkBegin, LinkEnd - LinkBegin - 1);

                    FLinkPrefixes.TryGetValue(rtbTextWithLinks.SelectionStart, out LinkType);

                    if (LinkType == null)
                    {
                        FLinkPrefixes.TryGetValue(rtbTextWithLinks.SelectionStart - 1, out LinkType);
                    }

                    rtbTextWithLinks.SelectionStart = SelectionStartAtBeginning;

                    OnLinkClicked(LinkText, LinkType, LinkEnd - 1);
                }
            }
            else
            {
                rtbTextWithLinks.SelectionStart = SelectionStartAtBeginning;
            }
        }

        void rtbTextWithLinks_MouseMove(object sender, MouseEventArgs e)
        {
            Point cursorLocation;
            int charIndex;
            bool CursorIsInALinkRange = false;
            int XPosCorrection = 0;

            // Get the index of the character under the cursor.
            cursorLocation = e.Location;
            charIndex = rtbTextWithLinks.GetCharIndexFromPosition(cursorLocation);

            foreach (var LinkRange in FLinkRanges)
            {
                // HACK: For some strange reason we need to do a minor correction for the last Link...
                if (LinkRange.Key == FLinkRanges.Last().Key)
                {
                    if (LinkRange.Value == rtbTextWithLinks.Text.Length)
                    {
                        XPosCorrection = 5;
                    }
                    else
                    {
                        XPosCorrection = 3;
                    }
                }

                if ((charIndex > LinkRange.Key)
                    && (charIndex <= LinkRange.Value))
                {
                    if (cursorLocation.X <= rtbTextWithLinks.GetPositionFromCharIndex(charIndex + 1).X - XPosCorrection)
                    {
                        CursorIsInALinkRange = true;
                        break;
                    }
                }
            }

            if (CursorIsInALinkRange)
            {
                this.ParentForm.Cursor = Cursors.Hand;
            }
            else
            {
                this.ParentForm.Cursor = Cursors.Default;
            }
        }

        #endregion

        /// <summary>
        /// Helper Class for handling the displaying, parsing and execution of certain types of HyperLinks
        /// </summary>
        public class DisplayHelper
        {
            private const string EMAILSEPARATOR = ";";

            private readonly TRtbHyperlinks FHyperLinksControl;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="AParent">The TRtbHyperlinks Control.</param>
            public DisplayHelper(TRtbHyperlinks AParent)
            {
                FHyperLinksControl = AParent;
            }

            /// <summary>
            /// Displays ordinary text that isn't any form of a hyperlink/other 'clickable' text.
            /// </summary>
            /// <param name="APlainText">Ordinary text that isn't any form of a hyperlink/other 'clickable' text.</param>
            /// <param name="AAddToExistingText">Set to true if Hyperlink (URL) should be added to
            /// already existing text (starting in a new line). (Default=false.)</param>
            /// <param name="AAddNoLineBreakBefore">Set to true to not add a line break before adding text.
            /// (Default=true.)</param>
            public void DisplayPlainText(string APlainText, bool AAddToExistingText = false,
                bool AAddNoLineBreakBefore = true)
            {
                if (!AAddToExistingText)
                {
                    FHyperLinksControl.Text = APlainText;
                }
                else
                {
                    FHyperLinksControl.Text +=
                        (((FHyperLinksControl.Text.Length == 0) || AAddNoLineBreakBefore) ? String.Empty : Environment.NewLine) +
                        APlainText;
                }
            }

            /// <summary>
            /// Displays E-mail Address(es).
            /// </summary>
            /// <param name="AEmailAddress">E-Mail Address or E-Mail Addresses.</param>
            /// <param name="AAddToExistingText">Set to true if E-Mail Address should be added to
            /// already existing text (starting in a new line). (Default=false.)</param>
            /// <param name="AAddNoLineBreakBefore">Set to true to not add a line break before adding text.
            /// (Default=true.)</param>
            public void DisplayEmailAddress(string AEmailAddress, bool AAddToExistingText = false,
                bool AAddNoLineBreakBefore = true)
            {
                if (!AAddToExistingText)
                {
                    FHyperLinksControl.Text = BuildEmailAddressString(AEmailAddress);
                }
                else
                {
                    FHyperLinksControl.Text +=
                        (((FHyperLinksControl.Text.Length == 0) || AAddNoLineBreakBefore) ? String.Empty : Environment.NewLine) +
                        BuildEmailAddressString(AEmailAddress);
                }
            }

            /// <summary>
            /// Builds a string for (an) E-mail Address(es).
            /// </summary>
            /// <param name="AEmailAddress">E-mail Address or E-mail Addresses.</param>
            private string BuildEmailAddressString(string AEmailAddress)
            {
                string ReturnValue = String.Empty;

                String[] EmailAddresses;

                if (AEmailAddress != String.Empty)
                {
                    EmailAddresses = StringHelper.SplitEmailAddresses(AEmailAddress);

                    for (int Counter = 0; Counter <= EmailAddresses.Length - 1; Counter += 1)
                    {
                        ReturnValue += THyperLinkHandling.HYPERLINK_PREFIX_EMAILLINK + EmailAddresses[Counter];

                        if (Counter != EmailAddresses.Length - 1)
                        {
                            ReturnValue += EMAILSEPARATOR + " ";
                        }
                    }
                }

                return ReturnValue;
            }

            /// <summary>
            /// Displays Internet Hyperlinks (URLs).
            /// </summary>
            /// <param name="AUrl">Hyperlink (URL).</param>
            /// <param name="AWithValue">Set to true if the Hyperlink is a Hyperlink that contains a value.
            /// (Default=false.)</param>
            /// <param name="AAddToExistingText">Set to true if Hyperlink (URL) should be added to
            /// already existing text (starting in a new line). (Default=false.)</param>
            /// <param name="AAddNoLineBreakBefore">Set to true to not add a line break before adding text.
            /// (Default=true.)</param>
            public int DisplayURL(string AUrl, bool AWithValue = false, bool AAddToExistingText = false,
                bool AAddNoLineBreakBefore = true)
            {
                if (!AAddToExistingText)
                {
                    FHyperLinksControl.Text = BuildURLString(AUrl, AWithValue);
                }
                else
                {
                    FHyperLinksControl.Text +=
                        (((FHyperLinksControl.Text.Length == 0) || AAddNoLineBreakBefore) ? String.Empty : Environment.NewLine) +
                        BuildURLString(AUrl, AWithValue);
                }

                // Return end of Link
                return FHyperLinksControl.Text.Length;
            }

            /// <summary>
            /// Builds a string for an Internet Hyperlink (URL).
            /// </summary>
            /// <param name="AUrl">Hyperlink (URL).</param>
            /// <param name="AWithValue">Set to true if the Hyperlink is a Hyperlink that contains a value.</param>
            private string BuildURLString(string AUrl, bool AWithValue = false)
            {
                if (AWithValue)
                {
                    return THyperLinkHandling.HYPERLINK_PREFIX_URLWITHVALUELINK + AUrl;
                }
                else
                {
                    return THyperLinkHandling.HYPERLINK_PREFIX_URLLINK + AUrl;
                }
            }

            /// <summary>
            /// Displays Skype IDs.
            /// </summary>
            /// <param name="ASkypeID">SkypeID.</param>
            /// <param name="AAddToExistingText">Set to true if the SkypeID should be added to
            /// already existing text (starting in a new line). (Default=false.)</param>
            /// <param name="AAddNoLineBreakBefore">Set to true to not add a line break before adding text.
            /// (Default=true.)</param>
            public void DisplaySkypeID(string ASkypeID, bool AAddToExistingText = false,
                bool AAddNoLineBreakBefore = true)
            {
                if (!AAddToExistingText)
                {
                    FHyperLinksControl.Text = BuildSkypeIDString(ASkypeID);
                }
                else
                {
                    FHyperLinksControl.Text +=
                        (((FHyperLinksControl.Text.Length == 0) || AAddNoLineBreakBefore) ? String.Empty : Environment.NewLine) +
                        BuildSkypeIDString(ASkypeID);
                }
            }

            /// <summary>
            /// Builds a string for a Skype ID.
            /// </summary>
            /// <param name="ASkypeID">SkypeID.</param>
            private string BuildSkypeIDString(string ASkypeID)
            {
                return THyperLinkHandling.HYPERLINK_PREFIX_SKYPELINK + ASkypeID;
            }

            /// <summary>
            /// Try to "execute" supplied link.
            /// </summary>
            public void LaunchHyperLink(string ALinkText, string ALinkType, int ALinkEnd = 0)
            {
                string TheLink;
                string LinkType = String.Empty;

                if (String.IsNullOrEmpty(ALinkText))
                {
                    throw new ArgumentNullException("ALinkText", "ALinkText must not be null or an empty string");
                }

                if (String.IsNullOrEmpty(ALinkType))
                {
                    throw new ArgumentException("The 'ALinkType' Argument must not be null or an empty string");
                }

                try
                {
                    if (ALinkText != String.Empty)
                    {
                        TheLink = ALinkText;

                        switch (THyperLinkHandling.ParseHyperLinkType(ALinkType))
                        {
                            case THyperLinkHandling.THyperLinkType.Http:

                                if ((ALinkText.ToLower().IndexOf(@"http://", StringComparison.InvariantCulture) < 0)
                                    && (ALinkText.ToLower().IndexOf(@"https://", StringComparison.InvariantCulture) < 0))
                                {
                                    LinkType = @"http://";
                                }

                                break;

                            case THyperLinkHandling.THyperLinkType.Http_With_Value_Replacement:

                                if (FHyperLinksControl.BuildLinkWithValue != null)
                                {
                                    TheLink = FHyperLinksControl.BuildLinkWithValue(TheLink, ALinkEnd);
                                }
                                else
                                {
                                    throw new EProblemLaunchingHyperlinkException(
                                        "Link is a Hyperlink that asks for a replacement of " +
                                        THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER +
                                        ", but the Delegate 'BuildLinkWithValue' has not been set up");
                                }

                                break;

                            case THyperLinkHandling.THyperLinkType.Ftp:

                                if (ALinkText.ToLower().IndexOf(@"ftp://", StringComparison.InvariantCulture) < 0)
                                {
                                    LinkType = @"ftp://";
                                }

                                break;

                            case THyperLinkHandling.THyperLinkType.Email:

                                if (ALinkText.ToLower().IndexOf("mailto:", StringComparison.InvariantCulture) < 0)
                                {
                                    LinkType = "mailto:";
                                }

                                break;

                            case THyperLinkHandling.THyperLinkType.Skype:

                                if (ALinkText.ToLower().IndexOf("skype:", StringComparison.InvariantCulture) < 0)
                                {
                                    LinkType = "skype:";
                                }

                                break;
                        }

                        System.Diagnostics.Process.Start(LinkType + TheLink);
                    }
                }
                catch (Exception ex)
                {
                    throw new EProblemLaunchingHyperlinkException("Hyperlink cannot be launched!", ex);
                }
            }
        }
    }
}