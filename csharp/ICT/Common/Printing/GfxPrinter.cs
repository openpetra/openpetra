//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2018 by OM International
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
    /// The TGfxPrinter class is the base class for TPdfPrinter
    /// </summary>
    public class TGfxPrinter : TPrinter
    {
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

        private StringFormat FLeft;
        private StringFormat FRight;
        private StringFormat FCenter;

        /// <summary>these values are set by PrintPage</summary>
        protected float FLinesPerPage;

        /// <summary>
        /// sets the orientation of the page
        ///
        /// </summary>
        /// <returns>void</returns>
        public TGfxPrinter(ePrinterBehaviour APrinterBehaviour) : base()
        {
            FPrinterBehaviour = APrinterBehaviour;
            
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
//            SetPageSize();
        }

        void BeginPrint(object ASender, PrintEventArgs AEv)
        {
            if ((FNumberOfPages == 0) && (CurrentPageNr != 0))
            {
                FNumberOfPages = CurrentPageNr;
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

        /// <summary>
        /// update the biggest last used font for the next line feed
        /// </summary>
        /// <param name="AFont"></param>
        protected virtual bool UpdateBiggestLastUsedFont(eFont AFont)
        {
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
            return false;
        }

        /// <summary>
        /// prints into the current line, absolute x position
        ///
        /// </summary>
        public override Boolean PrintString(String ATxt, eFont AFont, float AXPos)
        {
            return false;
        }

        /// <summary>
        /// prints into the current line, absolute x position with width and alignment
        /// </summary>
        /// <returns>true if something was printed</returns>
        public override Boolean PrintString(String ATxt, eFont AFont, float AXPos, float AWidth, eAlignment AAlign)
        {
            return false;
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
            return 0.0f;
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
            return false;
        }

        /// <summary>
        /// Draws a line, at specified position
        /// </summary>
        public override void DrawLine(Int32 APenPixels, float AXPos1, float AYPos1, float AXPos2, float AYPos2)
        {
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
        }

        /// <summary>
        /// Line Feed; increases the current y position by the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineFeed(eFont AFont)
        {
            return CurrentYPos;
        }

        /// <summary>
        /// Line Feed; increases the current y position by the height of the biggest last used font
        /// </summary>
        /// <returns>the new current line</returns>
        public override float LineFeed()
        {
            return CurrentYPos;
        }

        /// <summary>
        /// Line Feed, but not full line; increases the current y position by half the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineSpaceFeed(eFont AFont)
        {
            return CurrentYPos;
        }

        /// <summary>
        /// Reverse Line Feed; decreases the current y position by the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineUnFeed(eFont AFont)
        {
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
        }

        /// <summary>
        /// Jump to the position where the page footer starts.
        /// SetPageFooterSpace is used to define the space reserved for the footer.
        ///
        /// </summary>
        /// <returns>void</returns>
        public override float LineFeedToPageFooter()
        {
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

        bool FHasMorePages = false;

        /// <summary>
        /// Tell the printer, that there are more pages coming
        ///
        /// </summary>
        /// <returns>void</returns>
        public override void SetHasMorePages(bool AHasMorePages)
        {
            FHasMorePages = AHasMorePages;
        }

        /// <summary>
        /// more pages are coming
        /// </summary>
        /// <returns></returns>
        public override bool HasMorePages()
        {
            return FHasMorePages;
        }

        /// <summary>
        /// Converts the given value in cm to the currently used measurement unit
        ///
        /// </summary>
        /// <returns>void</returns>
        public override float Cm(float AValueInCm)
        {
            float ReturnValue = 0;

            if (true)
            // if (FEv.Graphics.PageUnit == GraphicsUnit.Inch)
            {
                ReturnValue = AValueInCm / 2.54f;
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
            if (true)
            //if ((FEv != null) && (FEv.Graphics.PageUnit == GraphicsUnit.Inch))
            {
                // FEv.Graphics.PageUnit is inch; therefore need to convert pixel to inch
                // pixel/inch = dpi <=> inch = pixel/dpi
                // cannot use FEv.PageSettings.PrinterResolution.X since that only works if a printer is available.
                return AWidth / DEFAULTPRINTERRESOLUTION;
            }

            // TODO other units
//            return AWidth;
        }

        /// <summary>
        /// convert pixels to inches or other unit used for output
        /// </summary>
        public override float PixelVertical(float AHeight)
        {
            if (true)
            //if ((FEv != null) && (FEv.Graphics.PageUnit == GraphicsUnit.Inch))
            {
                // FEv.Graphics.PageUnit is inch; therefore need to convert pixel to inch
                // pixel/inch = dpi <=> inch = pixel/dpi
                // cannot use FEv.PageSettings.PrinterResolution.Y since that only works if a printer is available.
                return AHeight / DEFAULTPRINTERRESOLUTION;
            }

            // TODO other units
//            return AHeight;
        }
/*
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
            if ((CurrentPageNr == 0) && (ASender != null))
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
                FLinesPerPage = (float)FHeight / (float)FDefaultFont.GetHeight(FEv.Graphics) * CurrentLineHeight;

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

            if (AEv.PageSettings.PrinterSettings.PrintRange == PrintRange.SomePages)
            {
                // Do we need to skip some pages at the start??
                if ((ASender != null) && (CurrentPageNr < AEv.PageSettings.PrinterSettings.FromPage - 1))
                {
                    // We are going to make recursive calls to this method to 'print' the pages we need to skip.
                    // But we don't want to call this paragraph of code again when we come back.
                    // We coming in here the first time because ASender is not null and there are pages to skip
                    // While skipping, ASender will be null so we won't come back
                    // After skipping, while printing real pages again, ASender will be non-null but the page number will be 'too high'
                    PrintingMode = ePrintingMode.eDoSimulate;

                    while (CurrentPageNr < AEv.PageSettings.PrinterSettings.FromPage - 1)
                    {
                        PrintPage(null, FEv);
                    }

                    // Now restore the real print mode. Now we are ready to continue with the first piece of paper and with real printing.
                    // All the TPrinterState properties will be correct because we haven't pushed/popped anything onto the stack.
                    PrintingMode = ePrintingMode.eDoPrint;
                }
            }

            CurrentPageNr++;

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
        */
    }
}
