/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.IO;
using Ict.Common;
using Ict.Common.Printing;
using System.Collections;

namespace Ict.Common.Printing
{
    /// <summary>
    /// The TTxtPrinter class allows to freely position text in a file.
    ///
    /// The printing is done in memory, and in the end the result is written to a file.
    /// That way the centering and aligning is possible,
    /// and jumping between lines is possible as well.
    ///
    /// @todo addline, which tests if page break is necessary
    ///       callback function printPageHeader(pagenumber: Integer);
    /// </summary>
    public class TTxtPrinter : TPrinter
    {
        // settings for courier new 8, 1 cm edge both sides
        // to allow really long reports for multiperiod

        /// <summary>todoComment</summary>
        public const Int16 NumberOfCharactersPerLineLandscape = 163;

        /// <summary>todoComment</summary>
        public const Int16 NumberOfCharactersPerLinePortrait = 112;

        /// <summary>todoComment</summary>
        public const Int16 DEFAULT_LENGTH_LINE = 800;

        /// <summary>todoComment</summary>
        public const float FACTOR_CM_2_LETTER = 2.0f;

        private ArrayList FText;

        /// <summary>
        /// constructor
        ///
        /// </summary>
        public TTxtPrinter() : base()
        {
            FText = new ArrayList();
        }

        /// <summary>
        /// set the orientation of the page
        /// </summary>
        /// <param name="AOrientation"></param>
        /// <param name="APrinterLayout"></param>
        public override void Init(eOrientation AOrientation, TPrinterLayout APrinterLayout)
        {
            base.Init(AOrientation, APrinterLayout);

            // FWidth: don't try to fit on a page at the moment
            FWidth = DEFAULT_LENGTH_LINE;
            FLeftMargin = 0;
            FRightMargin = FWidth - FLeftMargin;
            FCurrentPageNr = 1;
        }

        #region Manage the Y Position

        /// <summary>
        /// Line Feed; increases the current y position by the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineFeed(eFont AFont)
        {
            String s;

            System.Int32 counter;
            bool HasDash;
            bool HasOtherCharacters;
            FCurrentYPos = FCurrentYPos + 1;

            // check if line is only a marking line; in that case, jump over it
            s = GetLine(Convert.ToInt32(FCurrentYPos)).Trim();
            HasDash = false;
            HasOtherCharacters = false;

            for (counter = 0; counter <= s.Length - 1; counter += 1)
            {
                if ((s[counter] != '-') && (s[counter] != ' '))
                {
                    HasOtherCharacters = true;
                }

                if (s[counter] == '-')
                {
                    HasDash = true;
                }
            }

            if (HasDash && (!HasOtherCharacters))
            {
                FCurrentYPos = FCurrentYPos + 1;
            }

            return FCurrentYPos;
        }

        /// <summary>
        /// Line Feed, but not full line; increases the current y position by half the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineSpaceFeed(eFont AFont)
        {
            FCurrentYPos = FCurrentYPos + 1;
            return FCurrentYPos;
        }

        /// <summary>
        /// Reverse Line Feed; decreases the current y position by the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineUnFeed(eFont AFont)
        {
            FCurrentYPos = FCurrentYPos - 1;
            return FCurrentYPos;
        }

        /// <summary>
        /// Is the given position still on the page?
        ///
        /// </summary>
        /// <returns>void</returns>
        public override Boolean ValidXPos(float APosition)
        {
            Boolean ReturnValue;

            if (FOrientation == eOrientation.ePortrait)
            {
                ReturnValue = APosition <= NumberOfCharactersPerLinePortrait;
            }
            else
            {
                ReturnValue = APosition <= NumberOfCharactersPerLineLandscape;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Is the current line still on the page?
        ///
        /// </summary>
        /// <returns>void</returns>
        public override Boolean ValidYPos()
        {
            // todo: no page processing for text printing yet
            return true;
        }

        /// <summary>
        /// Jump to the position where the page footer starts.
        /// SetPageFooterSpace is used to define the space reserved for the footer.
        ///
        /// </summary>
        /// <returns>void</returns>
        public override float LineFeedToPageFooter()
        {
            return FCurrentYPos + 1;
        }

        /// <summary>
        /// Set the space that is required by the page footer.
        /// ValidYPos will consider this value.
        ///
        /// </summary>
        /// <returns>void</returns>
        public override void SetPageFooterSpace(System.Int32 ANumberOfLines, eFont AFont)
        {
            // just to tell that we accept a page footer; not fully implemented (regarding page breaks etc)
            if (ANumberOfLines != 0)
            {
                FPageFooterSpace = 1;
            }
        }

        /// <summary>
        /// Tell the printer, that there are more pages coming
        ///
        /// </summary>
        /// <returns>void</returns>
        public override void SetHasMorePages(bool AHasMorePages)
        {
            // not implemented
        }

        #endregion

        #region Calculate X position

        /// <summary>
        /// Converts the given value in cm to the currently used measurement unit (here only letters)
        ///
        /// </summary>
        /// <returns>void</returns>
        public override float Cm(float AValueInCm)
        {
            float ReturnValue;

            // valueInCm / 29.7 = valueInLetters / NumberOfCharactersPerLinePortrait
            ReturnValue = (AValueInCm / 29.7f) * NumberOfCharactersPerLinePortrait;

            // the cm in the xml file are targeted towards graphics output, the text output needs more space
            return ReturnValue * FACTOR_CM_2_LETTER;
        }

        #endregion

        #region Text printing specific

        /// <summary>
        /// retrieve a complete line from the text in memory
        ///
        /// </summary>
        /// <returns>void</returns>
        public string GetLine(Int32 y)
        {
            while (FText.Count <= y)
            {
                FText.Add(new String(' ', DEFAULT_LENGTH_LINE));
            }

            return (string)FText[y];
        }

        /// <summary>
        /// write a complete line; overwrites any existing line
        ///
        /// </summary>
        /// <returns>void</returns>
        public void SetLine(Int32 y, string line)
        {
            FText[y] = line;
        }

        /// <summary>
        /// Insert a line that can be used for lines, ie. ======
        ///
        /// If a line has already been inserted in that position, don't do anything.
        /// Otherwise move the text down, and create a new empty line in the given place AYPos
        /// </summary>
        /// <returns>true if a line has been inserted
        /// </returns>
        public bool InsertLineForMarkingLine(float AYPos)
        {
            String s;
            bool insert;

            System.Int32 counter;
            insert = false;
            s = GetLine(Convert.ToInt32(AYPos)).Trim();

            for (counter = 0; counter <= s.Length - 1; counter += 1)
            {
                if ((s[counter] != '-') && (s[counter] != ' '))
                {
                    insert = true;
                }
            }

            if (insert)
            {
                FText.Insert(Convert.ToInt32(AYPos), new String(' ', DEFAULT_LENGTH_LINE));
            }

            return insert;
        }

        /// <summary>
        /// write text to a specified position
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Print(Int32 x, Int32 y, string s)
        {
            string line;
            string head;
            string tail;

            if ((s == null) || (s.Length == 0))
            {
                return;
            }

            line = GetLine(y);
            tail = "";

            if ((line.Length - x - s.Length > 0) && (x + s.Length < line.Length))
            {
                tail = line.Substring(x + s.Length, line.Length - x - s.Length);
            }

            head = "";

            if ((x > 0) && (x < line.Length))
            {
                head = line.Substring(0, x);
            }
            else if (x >= line.Length)
            {
                head = line;
            }

            if (FPrintingMode == ePrintingMode.eDoPrint)
            {
                line = head + s + tail;
            }

            SetLine(y, line);
        }

        #endregion

        #region Print String

        /// <summary>
        /// prints into the current line, aligned x position
        /// </summary>
        /// <returns>true if something was printed
        /// </returns>
        public override Boolean PrintString(String ATxt, eFont AFont, eAlignment AAlign)
        {
            Boolean ReturnValue;
            Int32 XPos;

            if ((ATxt == null) || (ATxt.Length == 0))
            {
                return false;
            }

            ReturnValue = true;

            // eDefault
            XPos = 0;

            switch (AAlign)
            {
                case eAlignment.eCenter:

                    if (FOrientation == eOrientation.ePortrait)
                    {
                        XPos = (NumberOfCharactersPerLinePortrait - ATxt.Length) / 2;
                    }
                    else
                    {
                        XPos = (NumberOfCharactersPerLineLandscape - ATxt.Length) / 2;
                    }

                    break;

                case eAlignment.eLeft:
                    XPos = 0;
                    break;

                case eAlignment.eRight:

                    if (FOrientation == eOrientation.ePortrait)
                    {
                        XPos = (Int16)(NumberOfCharactersPerLinePortrait - ATxt.Length);
                    }
                    else
                    {
                        XPos = (Int16)(NumberOfCharactersPerLineLandscape - ATxt.Length);
                    }

                    break;
            }

            Print(XPos, Convert.ToInt32(FCurrentYPos), ATxt);
            return ReturnValue;
        }

        /// <summary>
        /// prints into the current line, absolute x position with width and alignment
        /// </summary>
        /// <returns>true if something was printed
        /// </returns>
        public override bool PrintString(String ATxt, eFont AFont, float AXPos, float AWidth, eAlignment AAlign)
        {
            String ToPrint;

            System.Int32 XPos;

            if ((ATxt == null) || (ATxt.Length == 0))
            {
                return false;
            }

            // eDefault
            XPos = Convert.ToInt32(AXPos);

            switch (AAlign)
            {
                case eAlignment.eCenter:
                    XPos = Convert.ToInt32(AXPos) + (Convert.ToInt32(AWidth) - ATxt.Length) / 2;
                    break;

                case eAlignment.eLeft:
                    XPos = Convert.ToInt32(AXPos);
                    break;

                case eAlignment.eRight:

                    if (Convert.ToInt32(AWidth) > ATxt.Length)
                    {
                        XPos = Convert.ToInt32(AXPos) + Convert.ToInt32(AWidth) - ATxt.Length;
                    }
                    else
                    {
                        XPos = Convert.ToInt32(AXPos);
                    }

                    break;
            }

            ToPrint = ATxt;

            if (ATxt.Length > Convert.ToInt32(AWidth))
            {
                ToPrint = ATxt.Substring(0, Convert.ToInt32(AWidth));
            }

            Print(XPos, Convert.ToInt32(FCurrentYPos), ToPrint);
            return true;
        }

        /// <summary>
        /// prints into the current line, absolute x position
        /// </summary>
        /// <returns>true if something was printed
        /// </returns>
        public override Boolean PrintString(String ATxt, eFont AFont, float AXPos)
        {
            Boolean ReturnValue;

            ReturnValue = (ATxt.Length != 0);
            Print(Convert.ToInt32(AXPos), Convert.ToInt32(FCurrentYPos), ATxt);
            return ReturnValue;
        }

        /// <summary>
        /// This function uses the normal PrintString function to print into a given space.
        /// </summary>
        /// <returns>whether the text did fit that space or not.
        /// </returns>
        public override Boolean PrintStringAndFits(String ATxt, eFont AFont, float AXPos, float AWidth, eAlignment AAlign)
        {
            PrintString(ATxt, AFont, AXPos, AWidth, AAlign);
            return ATxt.Length <= AWidth;
        }

        #endregion

        /// <summary>
        /// Return the width of the string, if it was printed in one line, using the given Font
        ///
        /// </summary>
        /// <returns>void</returns>
        public override float GetWidthString(String ATxt, eFont AFont)
        {
            return ATxt.Length;
        }

        /// <summary>
        /// Draws a line, either above or below the current text line
        /// the font is required to get the height of the row
        ///
        /// </summary>
        /// <returns>void</returns>
        public override Boolean DrawLine(float AXPos1, float AXPos2, eLinePosition ALinePosition, eFont AFont)
        {
            if (ALinePosition == eLinePosition.eAbove)
            {
                // to deal with things already printed in the beginning of the line (e.g. descr)
                InsertLineForMarkingLine(FCurrentYPos);
            }
            else if (ALinePosition == eLinePosition.eBelow)
            {
                FCurrentYPos = FCurrentYPos + 1;
            }

            if (AXPos1 != 0)
            {
                // lines above/below columns should not touch
                AXPos1 = AXPos1 + 1;
                AXPos2 = AXPos2 - 1;
            }

            if (FPrintingMode == ePrintingMode.eDoPrint)
            {
                PrintString(new String('-', Convert.ToInt32(AXPos2 - AXPos1 + 1)), eFont.eDefaultFont, Convert.ToInt32(AXPos1));
            }

            FCurrentYPos = FCurrentYPos + 1;
            return true;
        }

        #region Use the resulting Text

        /// <summary>
        /// This will cut the long separator lines, e.g. ----
        ///
        /// </summary>
        /// <returns>void</returns>
        private void FinishText()
        {
            System.Int32 maxLength;
            System.Int32 currentLineLength;
            System.Int32 y;

            // go through all lines, and get the maximum length, apart from the separating lines
            maxLength = 1;

            foreach (String line in FText)
            {
                currentLineLength = line.Trim().Length;

                if ((currentLineLength < DEFAULT_LENGTH_LINE) && (currentLineLength > maxLength))
                {
                    maxLength = currentLineLength;
                }
            }

            // shorten the lines to that maximum length
            for (y = 0; y <= FText.Count - 1; y += 1)
            {
                String line = GetLine(y);
                currentLineLength = line.Trim().Length;

                if (currentLineLength >= DEFAULT_LENGTH_LINE)
                {
                    SetLine(y, line.Substring(0, maxLength));
                }
            }
        }

        /// <summary>
        /// finally write the text from memory into a file
        ///
        /// </summary>
        /// <returns>void</returns>
        public void WriteToFile(string filename)
        {
            string rightTrim;

            FinishText();
            StreamWriter txtfile = new StreamWriter(filename, false, System.Text.Encoding.Default);

            foreach (String line in FText)
            {
                rightTrim = line.Substring(0, line.Trim().Length + line.IndexOf(line.Trim()));
                txtfile.WriteLine(rightTrim);
            }

            txtfile.Close();
        }

        /// <summary>
        /// copies the generated report into an array of strings.
        /// </summary>
        /// <returns>the array of strings
        /// </returns>
        public String[] GetArrayOfString()
        {
            String rightTrim;

            System.Int32 Counter;
            FinishText();
            String[] ReturnValue = new String[FText.Count];
            Counter = 0;

            foreach (String line in FText)
            {
                rightTrim = line.Substring(0, line.Trim().Length + line.IndexOf(line.Trim()));
                ReturnValue[Counter] = rightTrim;
                Counter = Counter + 1;
            }

            return ReturnValue;
        }

        #endregion
    }
}