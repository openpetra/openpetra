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
using Ict.Common.Printing;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;

namespace Ict.Common.Printing
{
    /// <summary>
    /// The TGfxPrinter class helps to print to a System.Drawing.Printing.PrintDocument.
    ///
    /// This means you can use several fonts, etc.
    ///
    /// see also
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwinforms/html/printwinforms.asp
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnadvnet/html/vbnet01282003.asp
    /// </summary>
    public class TGfxPrinter : TPrinter
    {
        private System.Drawing.Printing.PrintDocument FDocument;

        /// todoComment
        public System.Drawing.Font FDefaultFont;

        /// todoComment
        public System.Drawing.Font FDefaultBoldFont;

        /// todoComment
        public System.Drawing.Font FHeadingFont;

        /// todoComment
        public System.Drawing.Font FSmallPrintFont;

        /// todoComment
        public System.Drawing.Font FBiggestLastUsedFont;
        private StringFormat FLeft;
        private StringFormat FRight;
        private StringFormat FCenter;
        private System.Drawing.Pen FBlackPen;

        /// <summary>these values are set by PrintPage</summary>
        protected float FLinesPerPage;

        /// <summary>todoComment</summary>
        protected PrintPageEventArgs FEv;

        /// <summary>the document to print into</summary>
        public PrintDocument Document
        {
            get
            {
                return FDocument;
            }
        }


        /// <summary>
        /// sets the orientation of the page
        ///
        /// </summary>
        /// <returns>void</returns>
        public TGfxPrinter(System.Drawing.Printing.PrintDocument ADocument) : base()
        {
            FDocument = ADocument;
            FSmallPrintFont = new System.Drawing.Font("Arial", 6);
            FDefaultFont = new System.Drawing.Font("Arial", 8);
            FDefaultBoldFont = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
            FHeadingFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            FBiggestLastUsedFont = FDefaultFont;
            FRight = new StringFormat(StringFormat.GenericDefault);
            FRight.Alignment = StringAlignment.Far;
            FLeft = new StringFormat(StringFormat.GenericDefault);
            FLeft.Alignment = StringAlignment.Near;
            FCenter = new StringFormat(StringFormat.GenericDefault);
            FCenter.Alignment = StringAlignment.Center;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AOrientation"></param>
        /// <param name="APrinterLayout"></param>
        public override void Init(eOrientation AOrientation, TPrinterLayout APrinterLayout)
        {
            base.Init(AOrientation, APrinterLayout);

            if (AOrientation == eOrientation.ePortrait)
            {
                FDocument.DefaultPageSettings.Margins.Left = Convert.ToInt32(Cm2Inch(0.5f) * 100);
                FDocument.DefaultPageSettings.Margins.Right = Convert.ToInt32(Cm2Inch(1) * 100);
                FDocument.DefaultPageSettings.Margins.Top = Convert.ToInt32(Cm2Inch(0.5f) * 100);
                FDocument.DefaultPageSettings.Margins.Bottom = Convert.ToInt32(Cm2Inch(1) * 100);
            }
            else if (AOrientation == eOrientation.eLandscape)
            {
                FDocument.DefaultPageSettings.Margins.Left = Convert.ToInt32(Cm2Inch(0.5f) * 100);
                FDocument.DefaultPageSettings.Margins.Right = Convert.ToInt32(Cm2Inch(1) * 100);
                FDocument.DefaultPageSettings.Margins.Top = Convert.ToInt32(Cm2Inch(0.5f) * 100);
                FDocument.DefaultPageSettings.Margins.Bottom = Convert.ToInt32(Cm2Inch(0.5f) * 100);
            }

            // Associate the eventhandling method with the
            // document's PrintPage event.
            FDocument.PrintPage += new PrintPageEventHandler(this.PrintPage);
            FDocument.BeginPrint += new PrintEventHandler(this.BeginPrint);
            FDocument.EndPrint += new PrintEventHandler(this.EndPrint);

            FDocument.DefaultPageSettings.Landscape = (FOrientation == eOrientation.eLandscape);
        }

        void BeginPrint(object ASender, PrintEventArgs AEv)
        {
            if (FCurrentPageNr != 0)
            {
                FNumberOfPages = FCurrentPageNr;
            }

            FCurrentPageNr = 0;
            FPrinterLayout.StartPrintDocument();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASender"></param>
        /// <param name="AEv"></param>
        private void EndPrint(object ASender, PrintEventArgs AEv)
        {
            if ((FCurrentPageNr != 0) && (FNumberOfPages == 0))
            {
                FNumberOfPages = FCurrentPageNr;
            }
        }

        private System.Drawing.Font GetFont(eFont AFont)
        {
            System.Drawing.Font ReturnValue;
            ReturnValue = FDefaultFont;

            switch (AFont)
            {
                case eFont.eDefaultFont:
                    ReturnValue = FDefaultFont;
                    break;

                case eFont.eDefaultBoldFont:
                    ReturnValue = FDefaultBoldFont;
                    break;

                case eFont.eHeadingFont:
                    ReturnValue = FHeadingFont;
                    break;

                case eFont.eSmallPrintFont:
                    ReturnValue = FSmallPrintFont;
                    break;
            }

            return ReturnValue;
        }

        private System.Drawing.StringFormat GetStringFormat(eAlignment AAlign)
        {
            System.Drawing.StringFormat ReturnValue;
            ReturnValue = FLeft;

            switch (AAlign)
            {
                case eAlignment.eDefault:
                    ReturnValue = FLeft;
                    break;

                case eAlignment.eLeft:
                    ReturnValue = FLeft;
                    break;

                case eAlignment.eRight:
                    ReturnValue = FRight;
                    break;

                case eAlignment.eCenter:
                    ReturnValue = FCenter;
                    break;
            }

            return ReturnValue;
        }

        /// <summary>
        /// prints into the current line, aligned x position
        ///
        /// </summary>
        public override Boolean PrintString(String ATxt, eFont AFont, eAlignment AAlign)
        {
            RectangleF rect;

            rect = new RectangleF(FLeftMargin, FCurrentYPos, FWidth, GetFont(AFont).GetHeight(FEv.Graphics));

            if (FPrintingMode == ePrintingMode.eDoPrint)
            {
                FEv.Graphics.DrawString(ATxt, GetFont(AFont), Brushes.Black, rect, GetStringFormat(AAlign));
            }

            return (ATxt != null) && (ATxt.Length != 0);
        }

        /// <summary>
        /// prints into the current line, absolute x position
        ///
        /// </summary>
        public override Boolean PrintString(String ATxt, eFont AFont, float AXPos)
        {
            if (FPrintingMode == ePrintingMode.eDoPrint)
            {
                FEv.Graphics.DrawString(ATxt, GetFont(AFont), Brushes.Black, AXPos, FCurrentYPos);
            }

            return (ATxt != null) && (ATxt.Length != 0);
        }

        /// <summary>
        /// prints into the current line, absolute x position with width and alignment
        /// </summary>
        /// <returns>true if something was printed</returns>
        public override Boolean PrintString(String ATxt, eFont AFont, float AXPos, float AWidth, eAlignment AAlign)
        {
            RectangleF rect = new RectangleF(AXPos, FCurrentYPos, AWidth, GetFont(AFont).GetHeight(FEv.Graphics));

            if (FPrintingMode == ePrintingMode.eDoPrint)
            {
                StringFormat f = GetStringFormat(AAlign);
                f.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                FEv.Graphics.DrawString(ATxt, GetFont(AFont), Brushes.Black, rect, f);
            }

            return (ATxt != null) && (ATxt.Length != 0);
        }

        /// <summary>
        /// word wrap text, return the number of characters that fit the line width
        /// </summary>
        /// <param name="ATxt"></param>
        /// <param name="AFont"></param>
        /// <param name="AWidth"></param>
        /// <param name="firstWordLength"></param>
        /// <returns>returns the length of the first word; this is needed if even the first word does not fit</returns>
        protected Int32 GetTextLengthThatWillFit(String ATxt, eFont AFont, float AWidth, out Int32 firstWordLength)
        {
            // see also http://www.codeguru.com/vb/gen/vb_misc/printing/article.php/c11233
            string buffer = ATxt;
            string fittingText = "";

            char[] whitespace = new char[] {
                ' ', '\t', '\r', '\n'
            };
            string previousWhitespaces = "";
            Int32 result = 0;
            firstWordLength = 0;

            while (GetWidthString(fittingText, AFont) < AWidth)
            {
                result = fittingText.Length + previousWhitespaces.Length;

                if (buffer.Length == 0)
                {
                    return result;
                }

                Int32 indexWhitespace = buffer.IndexOfAny(whitespace);

                if (indexWhitespace > 0)
                {
                    string nextWord = buffer.Substring(0, indexWhitespace);
                    fittingText += previousWhitespaces + nextWord;

                    // sometimes there are forced whitespaces (eg &nbsp; etc)
                    // consider them as a word
                    previousWhitespaces = buffer[indexWhitespace].ToString();

                    while (indexWhitespace < buffer.Length && buffer.IndexOfAny(whitespace, indexWhitespace + 1) == indexWhitespace + 1)
                    {
                        indexWhitespace++;
                        previousWhitespaces += buffer[indexWhitespace];
                    }

                    buffer = buffer.Substring(indexWhitespace + 1);
                }
                else // no whitespace left
                {
                    fittingText += previousWhitespaces + buffer;
                    previousWhitespaces = "";
                    buffer = "";
                }

                if (firstWordLength == 0)
                {
                    firstWordLength = fittingText.Length + previousWhitespaces.Length;
                }
            }

            return result + previousWhitespaces.Length;
        }

        /// <summary>
        /// prints into the current line, absolute x position with width and alignment
        /// this method uses FCurrentXPos and FCurrentYPos to be able to continue a paragraph
        /// uses FCurrentXPos and FCurrentYPos to know where to start to print, and also sets
        /// valid values in those member variables
        /// </summary>
        /// <returns>s bool true if any text was printed</returns>
        public override bool PrintStringWrap(String ATxt, eFont AFont, float AXPos, float AWidth, eAlignment AAlign)
        {
            while (ATxt.Length > 0)
            {
                Int32 firstWordLength;
                Int32 length = GetTextLengthThatWillFit(ATxt, AFont, AXPos + AWidth - FCurrentXPos, out firstWordLength);

                if ((length <= 0) && (FCurrentXPos == AXPos) && (firstWordLength > 0))
                {
                    // the word is too long, it will never fit;
                    // force to print the first word
                    // todo: this overwrites the text in the next cell; should we break the line inside the word, or not print the overlap?
                    // goal: the problem should be easy to notice by the user...
                    length = firstWordLength;
                    string toPrint = ATxt.Substring(0, length);
                    TLogging.Log("the text \"" + toPrint + "\" does not fit into the assigned space!");
                }

                if (length > 0)
                {
                    string toPrint = ATxt.Substring(0, length);
                    ATxt = ATxt.Substring(length);

                    if (GetFont(AFont).GetHeight(FEv.Graphics) > FBiggestLastUsedFont.GetHeight(FEv.Graphics))
                    {
                        FBiggestLastUsedFont = GetFont(AFont);
                    }

                    PrintString(toPrint, AFont, FCurrentXPos, AWidth, AAlign);

                    if (AAlign == eAlignment.eRight)
                    {
                        FCurrentXPos += GetWidthString(toPrint, AFont);
                    }

                    if (ATxt.Length > 0)
                    {
                        // there is still more to come, we need a new line
                        FCurrentXPos = AXPos;
                        LineFeed(); // will use the biggest used font, and reset it
                    }
                }
                else if ((ATxt.Length > 0) && (FCurrentXPos != AXPos))
                {
                    // the first word did not fit the space; needs a new line
                    FCurrentXPos = AXPos;
                    LineFeed();
                }
            }

            return true;
        }

        /// <summary>
        /// This function uses the normal DrawString function to print into a given space.
        /// It will return whether the text did fit that space or not.
        ///
        /// </summary>
        /// <param name="ATxt"></param>
        /// <param name="AFont"></param>
        /// <param name="AXPos"></param>
        /// <param name="AWidth"></param>
        /// <param name="AAlign"></param>
        /// <returns></returns>
        public override Boolean PrintStringAndFits(String ATxt, eFont AFont, float AXPos, float AWidth, eAlignment AAlign)
        {
            PrintString(ATxt, AFont, AXPos, AWidth, AAlign);
            return GetWidthString(ATxt, AFont) <= AWidth;
        }

        /// <summary>
        /// prints into the current line, into the given column
        /// </summary>
        /// <param name="ATxt"></param>
        /// <param name="AFont"></param>
        /// <returns>Return the width of the string, if it was printed in one line, using the given Font</returns>
        public override float GetWidthString(String ATxt, eFont AFont)
        {
            return FEv.Graphics.MeasureString(ATxt, GetFont(AFont), Convert.ToInt32(Cm(30))).Width;
        }

        /// <summary>
        /// Draws a line, either above or below the current text line
        /// the font is required to get the height of the row
        /// </summary>
        /// <param name="AXPos1"></param>
        /// <param name="AXPos2"></param>
        /// <param name="ALinePosition"></param>
        /// <param name="AFont"></param>
        /// <returns></returns>
        public override Boolean DrawLine(float AXPos1, float AXPos2, eLinePosition ALinePosition, eFont AFont)
        {
            float YPos;

            YPos = FCurrentYPos;

            if (ALinePosition == eLinePosition.eBelow)
            {
                YPos = FCurrentYPos + GetFont(AFont).GetHeight(FEv.Graphics);
            }
            else if (ALinePosition == eLinePosition.eAbove)
            {
                YPos = FCurrentYPos;
            }

            if (AXPos1 != LeftMargin)
            {
                // lines above/below columns should not touch
                AXPos1 = AXPos1 + Cm(0.3f);
            }

            if (FPrintingMode == ePrintingMode.eDoPrint)
            {
                FEv.Graphics.DrawLine(FBlackPen, AXPos1, YPos, AXPos2, YPos);
            }

            return true;
        }

        /// <summary>
        /// draw a rectangle
        /// </summary>
        /// <param name="APenPixels"></param>
        /// <param name="AXPos"></param>
        /// <param name="AYPos"></param>
        /// <param name="AWidth"></param>
        /// <param name="AHeight"></param>
        public override void DrawRectangle(Int32 APenPixels,
            float AXPos,
            float AYPos,
            float AWidth,
            float AHeight)
        {
            FEv.Graphics.DrawRectangle(FBlackPen, AXPos, AYPos, AWidth, AHeight);
        }

        /// <summary>
        /// draw a bitmap at the given position;
        /// the current position is moved
        /// </summary>
        /// <param name="APath"></param>
        /// <param name="AXPos"></param>
        /// <param name="AYPos"></param>
        public override void DrawBitmap(string APath,
            float AXPos,
            float AYPos)
        {
            Bitmap img = new System.Drawing.Bitmap(APath);

            FEv.Graphics.DrawImage(img, AXPos, AYPos);

            // FEv.Graphics.PageUnit is inch; therefore need to convert pixel to inch
            // pixel/inch = dpi <=> inch = pixel/dpi
            FCurrentYPos += img.Size.Height / img.VerticalResolution;
            FCurrentXPos += img.Size.Width / img.HorizontalResolution;
        }

        /// <summary>
        /// draw a bitmap at the given position;
        /// the current position is moved
        /// </summary>
        public override void DrawBitmap(string APath,
            float AXPos,
            float AYPos,
            float AWidthPercentage,
            float AHeightPercentage)
        {
            Bitmap img = new System.Drawing.Bitmap(APath);
            float Height = img.Size.Height / img.VerticalResolution * AHeightPercentage;
            float Width = img.Size.Width / img.HorizontalResolution * AWidthPercentage;

            FEv.Graphics.DrawImage(img, AXPos, AYPos, Width, Height);

            // FEv.Graphics.PageUnit is inch; therefore need to convert pixel to inch
            // pixel/inch = dpi <=> inch = pixel/dpi
            FCurrentYPos += Height;
            FCurrentXPos += Width;
        }

        /// <summary>
        /// Line Feed; increases the current y position by the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineFeed(eFont AFont)
        {
            FCurrentYPos = FCurrentYPos + GetFont(AFont).GetHeight(FEv.Graphics);
            return FCurrentYPos;
        }

        /// <summary>
        /// Line Feed; increases the current y position by the height of the biggest last used font
        /// </summary>
        /// <returns>the new current line</returns>
        public override float LineFeed()
        {
            FCurrentYPos = FCurrentYPos + FBiggestLastUsedFont.GetHeight(FEv.Graphics);

            // reset the biggest last used font
            FBiggestLastUsedFont = FDefaultFont;
            return FCurrentYPos;
        }

        /// <summary>
        /// Line Feed, but not full line; increases the current y position by half the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineSpaceFeed(eFont AFont)
        {
            FCurrentYPos = FCurrentYPos + GetFont(AFont).GetHeight(FEv.Graphics) / 2;
            return FCurrentYPos;
        }

        /// <summary>
        /// Reverse Line Feed; decreases the current y position by the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineUnFeed(eFont AFont)
        {
            FCurrentYPos = FCurrentYPos - GetFont(AFont).GetHeight(FEv.Graphics);
            return FCurrentYPos;
        }

        /// <summary>
        /// Set the space that is required by the page footer.
        /// ValidYPos will consider this value.
        ///
        /// </summary>
        /// <returns>void</returns>
        public override void SetPageFooterSpace(System.Int32 ANumberOfLines, eFont AFont)
        {
            // half a line for the drawn line, to separate the report body from the footer
            if (ANumberOfLines != 0)
            {
                FPageFooterSpace = ((float)Convert.ToDouble(ANumberOfLines) + 0.5f) * GetFont(AFont).GetHeight(FEv.Graphics) + FDefaultFont.GetHeight(
                    FEv.Graphics);
            }
        }

        /// <summary>
        /// Jump to the position where the page footer starts.
        /// SetPageFooterSpace is used to define the space reserved for the footer.
        ///
        /// </summary>
        /// <returns>void</returns>
        public override float LineFeedToPageFooter()
        {
            FCurrentYPos = FTopMargin + FHeight - FPageFooterSpace + FDefaultFont.GetHeight(FEv.Graphics);
            return FCurrentYPos;
        }

        /// <summary>
        /// Is the given position still on the page?
        ///
        /// </summary>
        /// <returns>void</returns>
        public override Boolean ValidXPos(float APosition)
        {
            return APosition < LeftMargin + Width;
        }

        /// <summary>
        /// Is the current line still on the page?
        ///
        /// </summary>
        /// <returns>void</returns>
        public override Boolean ValidYPos()
        {
            // MessageBox.Show(CurrentYPos.ToString() + ' ' + Height.ToString());
            return FCurrentYPos < FTopMargin + FHeight - FPageFooterSpace;
        }

        /// <summary>
        /// Tell the printer, that there are more pages coming
        ///
        /// </summary>
        /// <returns>void</returns>
        public override void SetHasMorePages(bool AHasMorePages)
        {
            FEv.HasMorePages = AHasMorePages;
        }

        /// <summary>
        /// Converts the given value in cm to the currently used measurement unit
        ///
        /// </summary>
        /// <returns>void</returns>
        public override float Cm(float AValueInCm)
        {
            float ReturnValue;

            ReturnValue = 0;

            if (FEv != null)
            {
                if (FEv.Graphics.PageUnit == GraphicsUnit.Millimeter)
                {
                    ReturnValue = Convert.ToInt32(AValueInCm * 10);
                }
                else if (FEv.Graphics.PageUnit == GraphicsUnit.Point)
                {
                    ReturnValue = Cm2Twips(AValueInCm);
                }
                else if (FEv.Graphics.PageUnit == GraphicsUnit.Inch)
                {
                    ReturnValue = AValueInCm / 2.54f;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// 1440 TWIPS = 1 inch = 2.54 cm
        ///
        /// </summary>
        /// <returns>void</returns>
        public static Int32 Cm2Twips(float ACm)
        {
            return Convert.ToInt32(ACm * 566.93);
        }

        /// <summary>
        /// 1440 TWIPS = 1 inch = 2.54 cm
        ///
        /// </summary>
        /// <returns>void</returns>
        public static Int32 Inch2Twips(float AInch)
        {
            return Convert.ToInt32(AInch * 1440);
        }

        /// <summary>
        /// The PrintPage event is raised for each page to be printed.
        /// </summary>
        /// <returns>void</returns>
        void PrintPage(object ASender, PrintPageEventArgs AEv)
        {
            this.FEv = AEv;

            // ev.Graphics.PageUnit := GraphicsUnit.Point;  twips
            // ev.Graphics.PageUnit := GraphicsUnit.Millimeter;
            // u := ev.Graphics.PageUnit;  default world ???
            FEv.Graphics.PageUnit = GraphicsUnit.Inch;
            FEv.Graphics.TranslateTransform(0, 0);

            // first page? then we should store some settings
            if (FCurrentPageNr == 0)
            {
                FLeftMargin = FEv.MarginBounds.Left / 100.0f;
                FTopMargin = FEv.MarginBounds.Top / 100.0f;
                FRightMargin = FEv.MarginBounds.Right / 100.0f;
                FBottomMargin = FEv.MarginBounds.Bottom / 100.0f;
                FWidth = FEv.MarginBounds.Width / 100.0f;
                FHeight = FEv.MarginBounds.Height / 100.0f;
                FBlackPen = new Pen(Color.Black, Cm(0.05f));

                // Calculate the number of lines per page.
                FLinesPerPage = (float)FHeight / (float)FDefaultFont.GetHeight(FEv.Graphics);

                // estimate the number of pages needed
                // todo: use the height of the header, the number of linesPerPage, and the expected number of result lines.
                // if the last page does not match the predicted number of pages, print again, with the correct number as a parameter
                // prepare the columns

                /*
                 * if (FColumns = nil) then
                 * begin
                 * FColumns := new ArrayList();
                 * x := FLeftMargin + Cm(4);
                 * colWidth := Cm(2);
                 * for countColumn := 1 to 20 do
                 * begin
                 * FColumns.Add(RectangleF.Create(x, 0, colWidth, 0));
                 * x := x + colWidth;
                 * end;
                 * end;
                 */
            }

            FCurrentPageNr++;

            if (AEv.PageSettings.PrinterSettings.PrintRange == PrintRange.SomePages)
            {
                if (AEv.PageSettings.PrinterSettings.FromPage > FCurrentPageNr)
                {
                    FCurrentPageNr = AEv.PageSettings.PrinterSettings.FromPage;
                }
            }

            FCurrentYPos = FTopMargin;
            FPrinterLayout.PrintPageHeader();
            FPrinterLayout.PrintPageBody();
            FPrinterLayout.PrintPageFooter();

            if (AEv.PageSettings.PrinterSettings.PrintRange == PrintRange.SomePages)
            {
                if (FCurrentPageNr == AEv.PageSettings.PrinterSettings.ToPage)
                {
                    SetHasMorePages(false);
                    return;
                }
            }
        }
    }
}