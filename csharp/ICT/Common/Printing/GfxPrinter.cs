//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.IO;
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
        /// <summary>
        /// the graphical document
        /// </summary>
        protected System.Drawing.Printing.PrintDocument FDocument;

        /// we have some different behaviour when printing the columns of a report and when printing a form letter
        public enum ePrinterBehaviour
        {
            /// printing the columns of a report, we can make text fit
            eReport,
            /// printing a form letter, we need to print everything, comma is not a whitespace
            eFormLetter
        };

        /// <summary>
        /// we have some different behaviour when printing the columns of a report (can make text fit) and when printing a form letter
        /// </summary>
        public ePrinterBehaviour FPrinterBehaviour = ePrinterBehaviour.eReport;

        /// printing should be started at this position for each line
        public float FPageXPos;

        /// space available for printing
        public float FPageWidthAvailable;

        /// todoComment
        public System.Drawing.Font FDefaultFont;

        /// todoComment
        public System.Drawing.Font FDefaultBoldFont;

        /// todoComment
        public System.Drawing.Font FHeadingFont;

        /// fonts for printing barcodes, using Code 128
        public System.Drawing.Font FBarCodeFont;

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
        public TGfxPrinter(System.Drawing.Printing.PrintDocument ADocument, ePrinterBehaviour APrinterBehaviour) : base()
        {
            FDocument = ADocument;
            FPrinterBehaviour = APrinterBehaviour;
            FSmallPrintFont = new System.Drawing.Font("Arial", 6);
            FDefaultFont = new System.Drawing.Font("Arial", 8);
            FDefaultBoldFont = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
            FHeadingFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);

            // using GPL Font Code 128 from Grand Zebu http://grandzebu.net/
            FBarCodeFont = new System.Drawing.Font("Code 128", 35, FontStyle.Regular);

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
        public override void Init(eOrientation AOrientation, TPrinterLayout APrinterLayout, eMarginType AMarginType)
        {
            base.Init(AOrientation, APrinterLayout, AMarginType);
            SetPageSize();

            if (AOrientation == eOrientation.ePortrait)
            {
                FDocument.DefaultPageSettings.Margins.Left = Convert.ToInt32(Cm2Inch(0.5f) * 100);
                FDocument.DefaultPageSettings.Margins.Right = Convert.ToInt32(Cm2Inch(0.5f) * 100);
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
            if ((FNumberOfPages == 0) && (CurrentPageNr != 0))
            {
                FNumberOfPages = CurrentPageNr;
            }

            if (AEv != null)
            {
                FprintAction = AEv.PrintAction;
            }
            CurrentPageNr = 0;
            CurrentDocumentNr = 1;
            FPrinterLayout.StartPrintDocument();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASender"></param>
        /// <param name="AEv"></param>
        private void EndPrint(object ASender, PrintEventArgs AEv)
        {
            if ((CurrentPageNr != 0) && (FNumberOfPages == 0))
            {
                FNumberOfPages = CurrentPageNr;
            }
        }

        private SortedList <string, Font>FFontCache = new SortedList <string, Font>();

        /// <summary>
        /// get the font that is associated with the enum value.
        /// this way we do not need to create a new font each time
        /// </summary>
        /// <param name="AFont"></param>
        /// <returns></returns>
        protected System.Drawing.Font GetFont(eFont AFont)
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

                case eFont.eBarCodeFont:
                    ReturnValue = FBarCodeFont;
                    break;

                case eFont.eSmallPrintFont:
                    ReturnValue = FSmallPrintFont;
                    break;
            }

            if (CurrentRelativeFontSize != 0)
            {
                float FontSize = ReturnValue.SizeInPoints + CurrentRelativeFontSize;

                if (FontSize <= 0.0f)
                {
                    FontSize = 0.5f;
                }

                string id = ReturnValue.FontFamily.ToString() + FontSize.ToString() + ReturnValue.Style.ToString();

                if (!FFontCache.ContainsKey(id))
                {
                    FFontCache.Add(id, new Font(ReturnValue.FontFamily, FontSize, ReturnValue.Style));
                }

                ReturnValue = FFontCache[id];
            }

            return ReturnValue;
        }

        /// <summary>
        /// update the biggest last used font for the next line feed
        /// </summary>
        /// <param name="AFont"></param>
        protected virtual bool UpdateBiggestLastUsedFont(eFont AFont)
        {
            if (GetFont(AFont).GetHeight(FEv.Graphics) > FBiggestLastUsedFont.GetHeight(FEv.Graphics))
            {
                FBiggestLastUsedFont = GetFont(AFont);
                return true;
            }

            return false;
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

            rect = new RectangleF(FLeftMargin, CurrentYPos, FWidth, GetFont(AFont).GetHeight(FEv.Graphics));

            if (PrintingMode == ePrintingMode.eDoPrint)
            {
                ATxt = GetFittedText(ATxt, AFont, rect.Width);
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
            if (PrintingMode == ePrintingMode.eDoPrint)
            {
                FEv.Graphics.DrawString(ATxt, GetFont(AFont), Brushes.Black, AXPos, CurrentYPos);
            }

            return (ATxt != null) && (ATxt.Length != 0);
        }

        /// <summary>
        /// prints into the current line, absolute x position with width and alignment
        /// </summary>
        /// <returns>true if something was printed</returns>
        public override Boolean PrintString(String ATxt, eFont AFont, float AXPos, float AWidth, eAlignment AAlign)
        {
            RectangleF rect = new RectangleF(AXPos, CurrentYPos, AWidth, GetFont(AFont).GetHeight(FEv.Graphics));

            if (PrintingMode == ePrintingMode.eDoPrint)
            {
                StringFormat f = GetStringFormat(AAlign);
                f.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

                if (FPrinterBehaviour == ePrinterBehaviour.eReport)
                {
                    ATxt = GetFittedText(ATxt, AFont, rect.Width);
                }

                FEv.Graphics.DrawString(ATxt, GetFont(AFont), Brushes.Black, rect, f);
            }

            return (ATxt != null) && (ATxt.Length != 0);
        }

        /// <summary>
        /// Check if the text will fit into the given width. If yes, the text will be returned.
        /// If no, the text will be shortened and a "..." will be added to indicate that some text is missing.
        /// </summary>
        /// <param name="ATxt">the original text</param>
        /// <param name="AFont">the font</param>
        /// <param name="AWidth">the space available for the text in cm</param>
        /// <returns>The input text. Either unmodified or shortened.</returns>
        protected String GetFittedText(String ATxt, eFont AFont, float AWidth)
        {
            String ReturnValue = "";

            if (GetWidthString(ATxt, AFont) <= AWidth)
            {
                // The whole text fits into the available space
                ReturnValue = ATxt;
            }
            else
            {
                // We have to cut the text
                float WidthDotDotDot = GetWidthString("...", AFont);
                float WidthForText = AWidth - WidthDotDotDot;

                if (WidthForText <= 0.0)
                {
                    // only space for ...
                    ReturnValue = "...";
                }
                else
                {
                    ReturnValue = CutTextToLength(ATxt, AFont, WidthForText) + "...";
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Cuts a given text so it will not extend the given width.
        /// </summary>
        /// <param name="ATxt">The text to cut</param>
        /// <param name="AFont">The font used</param>
        /// <param name="AWidth">The available length for the text in cm.</param>
        /// <returns>The maximum part of the text that will not extend the width.</returns>
        protected String CutTextToLength(String ATxt, eFont AFont, float AWidth)
        {
            String ReturnValue = "";

            for (int Counter = 1; Counter <= ATxt.Length; ++Counter)
            {
                float length = GetWidthString(ATxt.Substring(0, Counter), AFont);

                if (length <= AWidth)
                {
                    ReturnValue = ATxt.Substring(0, Counter);
                }
                else
                {
                    break;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// word wrap text, return the number of characters that fit the line width;
        /// if the first word does not fit the space available, wrap the word in itself
        /// </summary>
        /// <param name="ATxt"></param>
        /// <param name="AFont"></param>
        /// <param name="AWidth"></param>
        /// <returns>returns the length of the first word; this is needed if even the first word does not fit</returns>
        protected Int32 GetTextLengthThatWillFit(String ATxt, eFont AFont, float AWidth)
        {
            // see also http://www.codeguru.com/vb/gen/vb_misc/printing/article.php/c11233
            string buffer = ATxt;
            string fittingText = "";

            char[] whitespace = new char[] {
                ' ', '\t', '\r', '\n'
            };

            if (FPrinterBehaviour == ePrinterBehaviour.eReport)
            {
                // whitespace setting for ePrinterBehaviour.eReport
                whitespace = new char[] {
                    ' ', '\t', '\r', '\n', '-'
                };
            }

            string previousWhitespaces = "";
            string nextWhitespaces = "";
            Int32 result = 0;

            while (GetWidthString(fittingText, AFont) < AWidth)
            {
                result = fittingText.Length;

                if (buffer.Length == 0)
                {
                    return ATxt.Length;
                }

                Int32 indexWhitespace = buffer.IndexOfAny(whitespace);

                if (indexWhitespace > -1)
                {
                    string nextWord = buffer.Substring(0, indexWhitespace);
                    fittingText += previousWhitespaces + nextWhitespaces + nextWord;

                    nextWhitespaces = "";

                    // sometimes there are forced whitespaces (eg &nbsp;, already replaced by spaces etc)
                    // consider them as a word
                    previousWhitespaces = buffer[indexWhitespace].ToString();

                    while (indexWhitespace < buffer.Length - 1 && buffer.Substring(indexWhitespace + 1).IndexOfAny(whitespace) == 0)
                    {
                        indexWhitespace++;
                        nextWhitespaces += buffer[indexWhitespace];
                    }

                    buffer = buffer.Substring(indexWhitespace + 1);
                }
                else // no whitespace left
                {
                    // store length of result without the rest of the text, because it might not fit;
                    // but include whitespaces already, avoid printing them at start of new line
                    result = (fittingText + previousWhitespaces).Length;
                    fittingText += previousWhitespaces + buffer;
                    previousWhitespaces = "";
                    buffer = "";
                }
            }

            if (result > 0)
            {
                result += previousWhitespaces.Length;
            }

            if (result == 0)
            {
                // the first word is already too long for the assigned space
                // see how many characters would fit
                buffer = fittingText;
                fittingText = "";

                while (GetWidthString(fittingText, AFont) < AWidth && buffer.Length > 0)
                {
                    fittingText += buffer[0];
                    buffer = buffer.Substring(1);
                }

                return fittingText.Length - 1;
            }

            return result;
        }

        /// <summary>
        /// prints into the current line, absolute x position with width and alignment
        /// this method uses FCurrentXPos and FCurrentYPos to be able to continue a paragraph
        /// uses FCurrentXPos and FCurrentYPos to know where to start to print, and also sets
        /// valid values in those member variables
        /// </summary>
        /// <returns>true if any text was printed</returns>
        public override bool PrintStringWrap(String ATxt, eFont AFont, float AXPos, float AWidth, eAlignment AAlign)
        {
            int PreviousLength = -1;

            while (ATxt.Length > 0)
            {
                Int32 length = -1;

                PreviousLength = ATxt.Length;

                if (FPrinterBehaviour == ePrinterBehaviour.eFormLetter)
                {
                    length = GetTextLengthThatWillFit(ATxt, AFont, AXPos + AWidth - CurrentXPos);
                }
                else if (FPrinterBehaviour == ePrinterBehaviour.eReport)
                {
                    length = GetTextLengthThatWillFit(ATxt, AFont, AWidth);
                }

                if (FCurrentState.FNoWrap)
                {
                    length = ATxt.Length;
                }

                if (length > 0)
                {
                    string toPrint = ATxt.Substring(0, length);
                    ATxt = ATxt.Substring(length);

                    UpdateBiggestLastUsedFont(AFont);

                    if (FPrinterBehaviour == ePrinterBehaviour.eFormLetter)
                    {
                        PrintString(toPrint, AFont, CurrentXPos, AWidth, AAlign);
                    }
                    else if (FPrinterBehaviour == ePrinterBehaviour.eReport)
                    {
                        PrintString(toPrint, AFont, AXPos, AWidth, AAlign);
                    }

                    CurrentXPos += GetWidthString(toPrint, AFont);

                    if (ATxt.Length > 0)
                    {
                        // there is still more to come, we need a new line
                        CurrentXPos = AXPos;
                        LineFeed(); // will use the biggest used font, and reset it
                    }
                }
                else if ((ATxt.Length > 0) && (CurrentXPos != AXPos))
                {
                    // the first word did not fit the space; needs a new line
                    CurrentXPos = AXPos;
                    LineFeed();
                }

                if (ATxt.Length == PreviousLength)
                {
                    TLogging.Log("No space for " + ATxt);
                    return false;
                }
            }

            if (FPrinterBehaviour == ePrinterBehaviour.eReport)
            {
                CurrentXPos = 0;
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
            return FEv.Graphics.MeasureString(ATxt, GetFont(AFont)).Width;
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

            YPos = CurrentYPos;

            if (ALinePosition == eLinePosition.eBelow)
            {
                YPos = CurrentYPos + GetFont(AFont).GetHeight(FEv.Graphics);
            }
            else if (ALinePosition == eLinePosition.eAbove)
            {
                YPos = CurrentYPos;
            }

            if (AXPos1 != LeftMargin)
            {
                // lines above/below columns should not touch
                AXPos1 = AXPos1 + Cm(0.3f);
            }

            if (PrintingMode == ePrintingMode.eDoPrint)
            {
                FEv.Graphics.DrawLine(FBlackPen, AXPos1, YPos, AXPos2, YPos);
            }

            return true;
        }

        /// <summary>
        /// Draws a line, at specified position
        /// </summary>
        public override void DrawLine(Int32 APenPixels, float AXPos1, float AYPos1, float AXPos2, float AYPos2)
        {
            if (PrintingMode == ePrintingMode.eDoPrint)
            {
                FEv.Graphics.DrawLine(FBlackPen, AXPos1, AYPos1, AXPos2, AYPos2);
            }
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
            if (PrintingMode == ePrintingMode.eDoPrint)
            {
                FEv.Graphics.DrawRectangle(FBlackPen, AXPos, AYPos, AWidth, AHeight);
            }
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
            if (!File.Exists(APath))
            {
                TLogging.Log("cannot draw bitmap because file does not exist " + APath);
                return;
            }

            Bitmap img = new System.Drawing.Bitmap(APath);

            if (PrintingMode == ePrintingMode.eDoPrint)
            {
                FEv.Graphics.DrawImage(img, AXPos, AYPos);
            }

            // FEv.Graphics.PageUnit is inch; therefore need to convert pixel to inch
            // pixel/inch = dpi <=> inch = pixel/dpi
            CurrentYPos += img.Size.Height / img.VerticalResolution;
            CurrentXPos += img.Size.Width / img.HorizontalResolution;
        }

        /// <summary>
        /// draw a bitmap at the given position;
        /// the current position is moved
        ///
        /// Either Width or WidthPercentage should be unequals 0, but only one should have a value.
        /// Same applies to Height
        /// </summary>
        public override void DrawBitmap(string APath,
            float AXPos,
            float AYPos,
            float AWidth,
            float AHeight,
            float AWidthPercentage,
            float AHeightPercentage)
        {
            if (!System.IO.File.Exists(APath))
            {
                throw new Exception("TGfxPrinter.DrawBitmap: cannot find image file " + APath);
            }

            Bitmap img = new System.Drawing.Bitmap(APath);
            float Height = img.Size.Height;

            if (AHeightPercentage != 0.0f)
            {
                Height = Height / img.VerticalResolution * AHeightPercentage;
            }
            else
            {
                Height = AHeight;
            }

            float Width = img.Size.Width;

            if (AHeightPercentage != 0.0f)
            {
                Width = Width / img.HorizontalResolution * AWidthPercentage;
            }
            else
            {
                Width = AWidth;
            }

            if (PrintingMode == ePrintingMode.eDoPrint)
            {
                FEv.Graphics.DrawImage(img, AXPos, AYPos, Width, Height);
            }

            // FEv.Graphics.PageUnit is inch; therefore need to convert pixel to inch
            // pixel/inch = dpi <=> inch = pixel/dpi
            CurrentYPos += Height;
            CurrentXPos += Width;
        }

        /// <summary>
        /// Line Feed; increases the current y position by the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineFeed(eFont AFont)
        {
            CurrentYPos = CurrentYPos + GetFont(AFont).GetHeight(FEv.Graphics);
            return CurrentYPos;
        }

        /// <summary>
        /// Line Feed; increases the current y position by the height of the biggest last used font
        /// </summary>
        /// <returns>the new current line</returns>
        public override float LineFeed()
        {
            CurrentYPos = CurrentYPos + FBiggestLastUsedFont.GetHeight(FEv.Graphics);

            // reset the biggest last used font
            FBiggestLastUsedFont = FDefaultFont;
            return CurrentYPos;
        }

        /// <summary>
        /// Line Feed, but not full line; increases the current y position by half the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineSpaceFeed(eFont AFont)
        {
            CurrentYPos = CurrentYPos + GetFont(AFont).GetHeight(FEv.Graphics) / 2;
            return CurrentYPos;
        }

        /// <summary>
        /// Reverse Line Feed; decreases the current y position by the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineUnFeed(eFont AFont)
        {
            CurrentYPos = CurrentYPos - GetFont(AFont).GetHeight(FEv.Graphics);
            return CurrentYPos;
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
            CurrentYPos = FTopMargin + FHeight - FPageFooterSpace + FDefaultFont.GetHeight(FEv.Graphics);
            return CurrentYPos;
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
            return CurrentYPos < FTopMargin + FHeight - FPageFooterSpace;
        }

        /// <summary>
        /// Tell the printer, that there are more pages coming
        ///
        /// </summary>
        /// <returns>void</returns>
        public override void SetHasMorePages(bool AHasMorePages)
        {
            if (FEv != null)
            {
                FEv.HasMorePages = AHasMorePages;
            }
        }

        /// <summary>
        /// more pages are coming
        /// </summary>
        /// <returns></returns>
        public override bool HasMorePages()
        {
            if (FEv != null)
            {
                return FEv.HasMorePages;
            }

            return false;
        }

        /// <summary>
        /// Converts the given value in cm to the currently used measurement unit
        ///
        /// </summary>
        /// <returns>void</returns>
        public override float Cm(float AValueInCm)
        {
            float ReturnValue = 0;

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
        /// default printer resolution
        /// </summary>
        public static Int32 DEFAULTPRINTERRESOLUTION = 300;

        /// <summary>
        /// convert pixels to inches or other unit used for output
        /// </summary>
        /// <param name="AWidth"></param>
        /// <returns></returns>
        public override float PixelHorizontal(float AWidth)
        {
            if ((FEv != null) && (FEv.Graphics.PageUnit == GraphicsUnit.Inch))
            {
                // FEv.Graphics.PageUnit is inch; therefore need to convert pixel to inch
                // pixel/inch = dpi <=> inch = pixel/dpi
                // cannot use FEv.PageSettings.PrinterResolution.X since that only works if a printer is available.
                return AWidth / DEFAULTPRINTERRESOLUTION;
            }

            // TODO other units
            return AWidth;
        }

        /// <summary>
        /// convert pixels to inches or other unit used for output
        /// </summary>
        public override float PixelVertical(float AHeight)
        {
            if ((FEv != null) && (FEv.Graphics.PageUnit == GraphicsUnit.Inch))
            {
                // FEv.Graphics.PageUnit is inch; therefore need to convert pixel to inch
                // pixel/inch = dpi <=> inch = pixel/dpi
                // cannot use FEv.PageSettings.PrinterResolution.Y since that only works if a printer is available.
                return AHeight / DEFAULTPRINTERRESOLUTION;
            }

            // TODO other units
            return AHeight;
        }

        /// <summary>
        /// Converts the given value in Point/Pixel to the currently used measurement unit
        ///
        /// </summary>
        /// <returns>void</returns>
        public float Point(float AValueInPoint)
        {
            float ReturnValue = 0;

            if (FEv != null)
            {
                if (FEv.Graphics.PageUnit == GraphicsUnit.Millimeter)
                {
                    ReturnValue = AValueInPoint * 0.35277f;
                }
                else if (FEv.Graphics.PageUnit == GraphicsUnit.Point)
                {
                    ReturnValue = AValueInPoint;
                }
                else if (FEv.Graphics.PageUnit == GraphicsUnit.Inch)
                {
                    ReturnValue = AValueInPoint / 72.0f;
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

        private void SetPageSize()
        {
            PaperKind MyPaperKind;
            Margins MyMargins;
            float WidthInPoint;
            float HeightInPoint;

            if (FPrinterLayout.GetPageSize(out MyPaperKind, out MyMargins, out WidthInPoint, out HeightInPoint))
            {
                FDocument.DefaultPageSettings.Margins = MyMargins;

                if (MyPaperKind == PaperKind.Custom)
                {
                    // PaperSize: Height and Width in hundreds of an inch
                    FDocument.DefaultPageSettings.PaperSize =
                        new PaperSize("Custom", Convert.ToInt32(WidthInPoint / 72.0f * 100.0f), Convert.ToInt32(HeightInPoint / 72.0f * 100.0f));
                    FWidth = WidthInPoint / 72.0f * 100.0f;
                    FHeight = HeightInPoint / 72.0f * 100.0f;
                    FLeftMargin = MyMargins.Left;
                    FTopMargin = MyMargins.Top;
                    FRightMargin = MyMargins.Right;
                    FBottomMargin = MyMargins.Bottom;
                }
                else
                {
                    try
                    {
                        foreach (PaperSize pkSize in FDocument.PrinterSettings.PaperSizes)
                        {
                            if (pkSize.Kind == MyPaperKind)
                            {
                                FDocument.DefaultPageSettings.PaperSize = pkSize;

                                if (FOrientation == eOrientation.ePortrait)
                                {
                                    FWidth = pkSize.Width / 100.0f;
                                    FHeight = pkSize.Height / 100.0f;
                                }
                                else
                                {
                                    FWidth = pkSize.Height / 100.0f;
                                    FHeight = pkSize.Width / 100.0f;
                                }

                                FLeftMargin = MyMargins.Left / 100.0f;
                                FTopMargin = MyMargins.Top / 100.0f;
                                FRightMargin = MyMargins.Right / 100.0f;
                                FBottomMargin = MyMargins.Bottom / 100.0f;

                                FWidth -= FLeftMargin + FRightMargin;
                                FHeight -= FTopMargin + FBottomMargin;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // exception: no printers installed
                        FDocument.DefaultPageSettings.PaperSize =
                            new PaperSize("Custom", Convert.ToInt32(WidthInPoint / 72.0f * 100.0f), Convert.ToInt32(HeightInPoint / 72.0f * 100.0f));
                        FWidth = WidthInPoint / 72.0f * 100.0f;
                        FHeight = HeightInPoint / 72.0f * 100.0f;
                        FLeftMargin = MyMargins.Left;
                        FTopMargin = MyMargins.Top;
                        FRightMargin = MyMargins.Right;
                        FBottomMargin = MyMargins.Bottom;
                    }
                }

                FMarginType = eMarginType.eCalculatedMargins;
            }

            if (FHeight < 0)
            {
                throw new Exception("TGfxPrinter.SetPageSize: invalid paper size, height is negative");
            }
        }

        /// <summary>
        /// The PrintPage event is raised for each page to be printed.
        /// </summary>
        /// <returns>void</returns>
        protected virtual void PrintPage(Object ASender, PrintPageEventArgs AEv)
        {
            this.FEv = AEv;

            // ev.Graphics.PageUnit := GraphicsUnit.Point;  twips
            // ev.Graphics.PageUnit := GraphicsUnit.Millimeter;
            // u := ev.Graphics.PageUnit;  default world ???
            FEv.Graphics.PageUnit = GraphicsUnit.Inch;
            FEv.Graphics.TranslateTransform(0, 0);

            // first page? then we should store some settings
            if (CurrentPageNr == 0)
            {
                if (FMarginType == eMarginType.ePrintableArea)
                {
                    // if no printer is installed, use default values
                    FLeftMargin = 0;
                    FTopMargin = 0.1f;
                    FRightMargin = -0.1f;
                    FBottomMargin = 0.1f;
                    FWidth = 8.268333f;
                    FHeight = 11.69333f;

                    try
                    {
                        // margin is set by the printing program, eg. HTML Renderer
                        if (FEv.PageSettings.PrintableArea.Width != 0)
                        {
                            FLeftMargin = FEv.PageSettings.PrintableArea.Left / 100.0f;
                            FTopMargin = FEv.PageSettings.PrintableArea.Top / 100.0f;
                            FRightMargin = (FEv.PageSettings.PaperSize.Width - FEv.PageSettings.PrintableArea.Right) / 100.0f;
                            FBottomMargin = (FEv.PageSettings.PaperSize.Height - FEv.PageSettings.PrintableArea.Bottom) / 100.0f;
                            FWidth = FEv.PageSettings.PrintableArea.Width / 100.0f;
                            FHeight = FEv.PageSettings.PrintableArea.Height / 100.0f;
                        }
                    }
                    catch (Exception)
                    {
                        TLogging.Log("no printer");
                    }
                }
                else if (FMarginType == eMarginType.eDefaultMargins)
                {
                    // prepare the margins here, this is used for the reporting printing
                    FLeftMargin = FEv.MarginBounds.Left / 100.0f;
                    FTopMargin = FEv.MarginBounds.Top / 100.0f;
                    FRightMargin = FEv.MarginBounds.Right / 100.0f;
                    FBottomMargin = FEv.MarginBounds.Bottom / 100.0f;
                    FWidth = FEv.MarginBounds.Width / 100.0f;
                    FHeight = FEv.MarginBounds.Height / 100.0f;

                    if (FprintAction != PrintAction.PrintToPreview) // A "real printer" prints from the hard margin, but the preview has no hard margin.
                    {
                        FLeftMargin = 0;
                        FTopMargin = 0;
                    }
                }
                else if (FMarginType == eMarginType.eCalculatedMargins)
                {
                    // the margins have been set in SetPageSize
                }

                FBlackPen = new Pen(Color.Black, Cm(0.05f));

                // Calculate the number of lines per page.
                FLinesPerPage = (float)FHeight / (float)FDefaultFont.GetHeight(FEv.Graphics);

                if (FNumberOfPages == 0)
                {
                    // do a dry run without printing but calculate the number of pages

                    StartSimulatePrinting();

                    int pageCounter = 0;
                    FNumberOfPages = 1;

                    do
                    {
                        pageCounter++;
                        PrintPage(null, FEv);
                    } while (HasMorePages());

                    FinishSimulatePrinting();
                    BeginPrint(null, null);
                    FNumberOfPages = pageCounter;
                }
            }

            CurrentPageNr++;

            if (AEv.PageSettings.PrinterSettings.PrintRange == PrintRange.SomePages)
            {
                if (AEv.PageSettings.PrinterSettings.FromPage > CurrentPageNr)
                {
                    CurrentPageNr = AEv.PageSettings.PrinterSettings.FromPage;
                    CurrentDocumentNr = AEv.PageSettings.PrinterSettings.FromPage;
                }
            }

            CurrentYPos = FTopMargin;
            CurrentXPos = FLeftMargin;

            FPrinterLayout.PrintPageHeader();

            float CurrentYPosBefore = CurrentYPos;
            float CurrentXPosBefore = CurrentXPos;
            FPrinterLayout.PrintPageBody();

            if ((CurrentYPosBefore == CurrentYPos) && (CurrentXPosBefore == CurrentXPos) && FEv.HasMorePages)
            {
                throw new Exception("failure printing, does not fit the page");
            }

            FPrinterLayout.PrintPageFooter();

            if (AEv.PageSettings.PrinterSettings.PrintRange == PrintRange.SomePages)
            {
                if (CurrentPageNr == AEv.PageSettings.PrinterSettings.ToPage)
                {
                    SetHasMorePages(false);
                    return;
                }
            }
        }
    }
}