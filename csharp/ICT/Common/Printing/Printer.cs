//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Xml;
using System.Collections.Generic;
using Ict.Common.Printing;
using System.Drawing.Printing;

namespace Ict.Common.Printing
{
    /// <summary>todoComment</summary>
    public enum eOrientation
    {
        /// <summary>todoComment</summary>
        eLandscape,

        /// <summary>todoComment</summary>
        ePortrait
    };

    /// <summary>which margins to use when printing</summary>
    public enum eMarginType
    {
        /// <summary>use the default margins of the printer</summary>
        eDefaultMargins,

        /// <summary>use the full printable area. the margins are managed by the rendering method, eg HTML renderer</summary>
        ePrintableArea,

        /// <summary>the margins have been set in SetPageSize</summary>
        eCalculatedMargins
    };

    /// <summary>todoComment</summary>
    public enum eAlignment
    {
        /// <summary>todoComment</summary>
        eDefault,

        /// <summary>todoComment</summary>
        eCenter,

        /// <summary>todoComment</summary>
        eLeft,

        /// <summary>todoComment</summary>
        eRight
    };

    /// <summary>todoComment</summary>
    public enum eLinePosition
    {
        /// <summary>todoComment</summary>
        eAbove,

        /// <summary>todoComment</summary>
        eBelow
    };

    /// <summary>todoComment</summary>
    public enum ePrintingMode
    {
        /// <summary>todoComment</summary>
        eDoPrint,

        /// <summary>todoComment</summary>
        eDoSimulate
    };

    /// <summary>todoComment</summary>
    public enum eFont
    {
        /// <summary>todoComment</summary>
        eDefaultFont,

        /// <summary>todoComment</summary>
        eDefaultBoldFont,

        /// <summary>todoComment</summary>
        eHeadingFont,

        /// <summary>todoComment</summary>
        eSmallPrintFont,

        /// <summary>useful for printing bar codes. usually code128.ttf</summary>
        eBarCodeFont
    };

    /// <summary>todoComment</summary>
    public enum eStageElementPrinting
    {
        /// <summary>todoComment</summary>
        eAnything,

        /// <summary>todoComment</summary>
        eHeader,

        /// <summary>todoComment</summary>
        eDetails,

        /// <summary>todoComment</summary>
        eFooter,

        /// <summary>todoComment</summary>
        eFinished
    };

    /// <summary>
    /// specify for which axis the value is intended. needed for taking resolution in account
    /// </summary>
    public enum eResolution
    {
        /// <summary>
        /// vertical
        /// </summary>
        eVertical,

        /// <summary>
        /// horizontal
        /// </summary>
        eHorizontal
    }

    /// definition for current state of printer; useful with the stack
    public class TPrinterState
    {
        /// <summary>todoComment</summary>
        public ePrintingMode FPrintingMode;

        /// <summary>todoComment</summary>
        public Int32 FCurrentPageNr;

        /// <summary>several documents are printed from one html file with different body elements</summary>
        public Int32 FCurrentDocumentNr = 1;

        /// <summary>todoComment</summary>
        public float FCurrentXPos;

        /// <summary>current y Position on page, in current display unit</summary>
        public float FCurrentYPos;

        /// <summary>other elements can be printed relative to this position</summary>
        public float FAnchorXPos;

        /// <summary>other elements can be printed relative to this position</summary>
        public float FAnchorYPos;

        /// <summary>todoComment</summary>
        public eFont FCurrentFont;

        /// relative number; 0 is normal size
        public float FCurrentRelativeFontSize = 0;

        /// <summary>todoComment</summary>
        public eAlignment FCurrentAlignment = eAlignment.eLeft;

        /// avoid wrapping, cut off text if it does not fit
        public bool FNoWrap = false;

        /// create a copy of this state
        public TPrinterState Copy()
        {
            TPrinterState newState = new TPrinterState();

            newState.FPrintingMode = FPrintingMode;
            newState.FCurrentPageNr = FCurrentPageNr;
            newState.FCurrentXPos = FCurrentXPos;
            newState.FCurrentYPos = FCurrentYPos;
            newState.FAnchorXPos = FAnchorXPos;
            newState.FAnchorYPos = FAnchorYPos;
            newState.FCurrentFont = FCurrentFont;
            newState.FCurrentRelativeFontSize = FCurrentRelativeFontSize;
            newState.FCurrentAlignment = FCurrentAlignment;
            newState.FNoWrap = FNoWrap;
            return newState;
        }
    }

    /// <summary>
    /// The TPrinter class helps to print.
    ///
    /// This is a generic class for printing.
    /// Most of the functions are abstract and need to be implemented by a derived class.
    /// This is TxtPrinter and GfxPrinter, that are derived from TPrinter
    /// </summary>
    public abstract class TPrinter
    {
        /// <summary>todoComment</summary>
        protected eOrientation FOrientation;

        /// <summary>use printable area or default margins</summary>
        protected eMarginType FMarginType;

        /// <summary>todoComment</summary>
        protected Int32 FNumberOfPages;

        /// <summary>how much space is needed for the footer lines; footerspace = font.height  number of lines</summary>
        protected float FPageFooterSpace;

        /// <summary>todoComment</summary>
        protected float FLeftMargin;

        /// <summary>todoComment</summary>
        protected float FTopMargin;

        /// <summary>todoComment</summary>
        protected float FRightMargin;

        /// <summary>todoComment</summary>
        protected float FBottomMargin;

        /// <summary>todoComment</summary>
        protected float FWidth;

        /// <summary>todoComment</summary>
        protected float FHeight;

        /// <summary>todoComment</summary>
        protected TPrinterLayout FPrinterLayout;

        /// current state of printer
        protected TPrinterState FCurrentState = new TPrinterState();

        /// I can check whether I'm printing to a preview or a real printer.
        protected PrintAction FprintAction;

        /// <summary>todoComment</summary>
        public System.Int32 CurrentPageNr
        {
            get
            {
                return FCurrentState.FCurrentPageNr;
            }
            set
            {
                FCurrentState.FCurrentPageNr = value;
            }
        }

        /// <summary>a document can consist of several pages;
        /// a document is one body element</summary>
        public System.Int32 CurrentDocumentNr
        {
            get
            {
                return FCurrentState.FCurrentDocumentNr;
            }
            set
            {
                FCurrentState.FCurrentDocumentNr = value;
            }
        }

        /// <summary>this only has a valid value after the first rendering of the report</summary>
        public System.Int32 NumberOfPages
        {
            get
            {
                return FNumberOfPages;
            }
            set
            {
                FNumberOfPages = value;
            }
        }

        /// <summary>todoComment</summary>
        public float CurrentYPos
        {
            get
            {
                return FCurrentState.FCurrentYPos;
            }

            set
            {
                FCurrentState.FCurrentYPos = value;
            }
        }

        /// <summary>todoComment</summary>
        public float CurrentXPos
        {
            get
            {
                return FCurrentState.FCurrentXPos;
            }

            set
            {
                FCurrentState.FCurrentXPos = value;
            }
        }

        /// <summary>other elements can be positioned relative to this position</summary>
        public float AnchorXPos
        {
            get
            {
                return FCurrentState.FAnchorXPos;
            }

            set
            {
                FCurrentState.FAnchorXPos = value;
            }
        }

        /// <summary>other elements can be positioned relative to this position</summary>
        public float AnchorYPos
        {
            get
            {
                return FCurrentState.FAnchorYPos;
            }

            set
            {
                FCurrentState.FAnchorYPos = value;
            }
        }

        /// <summary>todoComment</summary>
        public float LeftMargin
        {
            get
            {
                return FLeftMargin;
            }
        }

        /// <summary>todoComment</summary>
        public float RightMargin
        {
            get
            {
                return FRightMargin;
            }
        }

        /// <summary>todoComment</summary>
        public eFont CurrentFont
        {
            get
            {
                return FCurrentState.FCurrentFont;
            }
            set
            {
                FCurrentState.FCurrentFont = value;
            }
        }

        /// <summary>
        /// the relative font size; 0 is default size
        /// </summary>
        public float CurrentRelativeFontSize
        {
            get
            {
                return FCurrentState.FCurrentRelativeFontSize;
            }
            set
            {
                FCurrentState.FCurrentRelativeFontSize = value;
            }
        }

        /// <summary>todoComment</summary>
        public eAlignment CurrentAlignment
        {
            get
            {
                return FCurrentState.FCurrentAlignment;
            }
            set
            {
                FCurrentState.FCurrentAlignment = value;
            }
        }

        /// this is about simulation or printing
        public ePrintingMode PrintingMode
        {
            get
            {
                return FCurrentState.FPrintingMode;
            }
            set
            {
                FCurrentState.FPrintingMode = value;
            }
        }

        /// <summary>todoComment</summary>
        public float Width
        {
            get
            {
                return FWidth;
            }
        }

        /// <summary>todoComment</summary>
        public float PageFooterSpace
        {
            get
            {
                return FPageFooterSpace;
            }
        }

        /// <summary>
        /// save the state before a rotation etc
        /// </summary>
        public virtual void SaveState()
        {
        }

        /// <summary>
        /// restore the state after a rotation etc
        /// </summary>
        public virtual void RestoreState()
        {
        }

        /// <summary>
        /// rotate the following output by some degrees, at the given position
        /// </summary>
        public virtual void RotateAtTransform(double ADegrees, double XPos, double YPos)
        {
        }

        /// <summary>
        /// Line Feed; increases the current y position by the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public abstract float LineFeed(eFont AFont);

        /// <summary>
        /// Line Feed; increases the current y position by the height of the biggest last used font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public virtual float LineFeed()
        {
            return CurrentYPos + 1;
        }

        /// <summary>
        /// Line Feed; increases the current y position by the given height
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public float LineFeed(float height)
        {
            CurrentYPos += height;
            return CurrentYPos;
        }

        /// <summary>
        /// Line Feed, but not full line; increases the current y position by half the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public abstract float LineSpaceFeed(eFont AFont);

        /// <summary>
        /// Reverse Line Feed; decreases the current y position by the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public abstract float LineUnFeed(eFont AFont);

        /// <summary>
        /// Is the given position still on the page?
        ///
        /// </summary>
        /// <returns>void</returns>
        public abstract Boolean ValidXPos(float APosition);

        /// <summary>
        /// Is the current line still on the page?
        ///
        /// </summary>
        /// <returns>void</returns>
        public abstract Boolean ValidYPos();

        /// <summary>
        /// Jump to the position where the page footer starts.
        /// SetPageFooterSpace is used to define the space reserved for the footer.
        ///
        /// </summary>
        /// <returns>void</returns>
        public abstract float LineFeedToPageFooter();

        /// <summary>
        /// Set the space that is required by the page footer.
        /// ValidYPos will consider this value.
        ///
        /// </summary>
        /// <returns>void</returns>
        public abstract void SetPageFooterSpace(System.Int32 ANumberOfLines, eFont AFont);

        /// <summary>
        /// Tell the printer, that there are more pages coming
        ///
        /// </summary>
        /// <returns>void</returns>
        public abstract void SetHasMorePages(bool AHasMorePages);

        /// <summary>
        /// more pages are coming
        /// </summary>
        /// <returns></returns>
        public abstract bool HasMorePages();

        #region Calculate X position

        /// <summary>
        /// Converts the given value in cm to the currently used measurement unit
        /// </summary>
        public abstract float Cm(float AValueInCm);

        /// <summary>
        /// Converts the given value in pixel to the currently used measurement unit, using the horizontal resolution
        /// </summary>
        public abstract float PixelHorizontal(float AValueInPixel);

        /// <summary>
        /// Converts the given value in pixel to the currently used measurement unit, using the vertical resolution
        /// </summary>
        public abstract float PixelVertical(float AValueInPixel);

        #endregion
        #region Print String

        /// <summary>
        /// prints into the current line, aligned x position
        /// </summary>
        /// <returns>true if something was printed
        /// </returns>
        public abstract Boolean PrintString(String ATxt, eFont AFont, eAlignment AAlign);

        /// <summary>
        /// prints into the current line, absolute x position
        /// </summary>
        /// <returns>true if something was printed
        /// </returns>
        public abstract Boolean PrintString(String ATxt, eFont AFont, float AXPos);

        /// <summary>
        /// prints into the current line, absolute x position with width and alignment
        /// </summary>
        /// <returns>true if something was printed
        /// </returns>
        public abstract bool PrintString(String ATxt, eFont AFont, float AXPos, float AWidth, eAlignment AAlign);

        /// <summary>
        /// prints into the current line, absolute x position with width and alignment
        /// this method uses FCurrentXPos and FCurrentYPos to be able to continue a paragraph
        /// uses FCurrentXPos and FCurrentYPos to know where to start to print, and also sets
        /// valid values in those member variables
        /// </summary>
        /// <returns>s bool true if any text was printed</returns>
        public virtual bool PrintStringWrap(String ATxt, eFont AFont, float AXPos, float AWidth, eAlignment AAlign)
        {
            return false;
        }

        /// <summary>
        /// This function uses the normal DrawString function to print into a given space.
        /// </summary>
        /// <returns>whether the text did fit that space or not.
        /// </returns>
        public abstract Boolean PrintStringAndFits(String ATxt, eFont AFont, float AXPos, float AWidth, eAlignment AAlign);

        #endregion

        /// <summary>
        /// Return the width of the string, if it was printed in one line, using the given Font
        ///
        /// </summary>
        /// <returns>void</returns>
        public abstract float GetWidthString(String ATxt, eFont AFont);

        /// <summary>
        /// Draws a line, either above or below the current text line
        /// the font is required to get the height of the row
        ///
        /// </summary>
        /// <returns>void</returns>
        public abstract Boolean DrawLine(float AXPos1, float AXPos2, eLinePosition ALinePosition, eFont AFont);

        /// <summary>
        /// Draws a line, at specified position
        /// </summary>
        public virtual void DrawLine(Int32 APenPixels, float AXPos1, float AYPos1, float AXPos2, float AYPos2)
        {
            // TTxtPrinter does not need this; so don't force implementation
        }

        /// <summary>
        /// draws a rectangle
        /// </summary>
        /// <returns>void</returns>
        public virtual void DrawRectangle(Int32 APenPixels,
            float AXPos,
            float AYPos,
            float AWidth,
            float AHeight)
        {
            // TTxtPrinter does not need this; so don't force implementation
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APath"></param>
        /// <param name="AXPos"></param>
        /// <param name="AYPos"></param>
        public virtual void DrawBitmap(string APath,
            float AXPos,
            float AYPos)
        {
            // TTxtPrinter does not need this; so don't force implementation
        }

        /// <summary>
        /// Draw a bitmap.
        ///
        /// Either Width or WidthPercentage should be unequals 0, but only one should have a value.
        /// Same applies to Height
        /// </summary>
        public virtual void DrawBitmap(string APath,
            float AXPos,
            float AYPos,
            float AWidth,
            float AHeight,
            float AWidthPercentage,
            float AHeightPercentage)
        {
            // TTxtPrinter does not need this; so don't force implementation
        }

        /// <summary>
        /// constructor
        ///
        /// </summary>
        public TPrinter() : base()
        {
            FOrientation = eOrientation.ePortrait;
            PrintingMode = ePrintingMode.eDoPrint;
            FMarginType = eMarginType.eDefaultMargins;
            CurrentPageNr = 0;
            FNumberOfPages = 0;
            CurrentFont = eFont.eDefaultFont;
            FPageFooterSpace = 0;
        }

        /// <summary>
        /// sets the orientation of the page
        /// </summary>
        public virtual void Init(eOrientation AOrientation, TPrinterLayout APrinterLayout, eMarginType AMarginType)
        {
            FPrinterLayout = APrinterLayout;
            FOrientation = AOrientation;
            FMarginType = AMarginType;
        }

        Stack <TPrinterState>FPrinterStateStack = new Stack <TPrinterState>();

        /// <summary>
        /// start the simulation of printing; nothing is actually printed, but the CurrentYPos is increased
        ///
        /// </summary>
        /// <returns>void</returns>
        public void StartSimulatePrinting()
        {
            PushCurrentState();
            PrintingMode = ePrintingMode.eDoSimulate;
        }

        /// <summary>
        /// finish the simulation of printing; the actual CurrentYPos is restored
        ///
        /// </summary>
        /// <returns>void</returns>
        public void FinishSimulatePrinting()
        {
            PopCurrentState();
        }

        /// <summary>
        /// store the current printer state (font size etc)
        /// </summary>
        public void PushCurrentState()
        {
            FPrinterStateStack.Push(FCurrentState);
            FCurrentState = FCurrentState.Copy();
        }

        /// <summary>
        /// return to previous printer state;
        /// this is used for printing table cells
        /// </summary>
        public void PopCurrentState()
        {
            FCurrentState = FPrinterStateStack.Pop();
        }

        /// <summary>
        /// return to previous printer state; but keep the new y position (used eg. for printing the page header)
        /// </summary>
        public void PopCurrentStateApartFromYPosition()
        {
            float YPos = this.CurrentYPos;

            FCurrentState = FPrinterStateStack.Pop();
            CurrentYPos = YPos;
        }

        /// <summary>
        /// Converts the given value in cm to the equivalent value in inches
        ///
        /// </summary>
        /// <returns>void</returns>
        static public float Cm2Inch(float AValueInCm)
        {
            return AValueInCm / 2.54f;
        }

        /// <summary>
        /// Converts the given value in inches to the equivalent value in centimeters
        ///
        /// </summary>
        /// <returns>void</returns>
        static public float Inch2Cm(float AValueInInch)
        {
            return AValueInInch * 2.54f;
        }

        /// <summary>
        /// Converts the given value in inch to the currently used measurement unit;
        /// uses Inch2Cm and Cm
        ///
        /// </summary>
        /// <returns>void</returns>
        public float Inch(float AValueInInch)
        {
            return Cm(Inch2Cm(AValueInInch));
        }

        /// <summary>
        /// insert another document into the current document. At the moment only used for PDF
        /// </summary>
        /// <param name="AFilename"></param>
        public virtual void InsertDocument(string AFilename)
        {
            throw new Exception("cannot insert document " + AFilename + ", this currently only works for printing to PDF");
        }

        /// <summary>
        /// renders a table at the current FCurrentYPos
        /// does not support rowspan at the moment
        /// colspan might be implemented in the generation of the TTableRowGfx structure
        /// does not care about fitting on page etc.
        /// </summary>
        /// <param name="AXPos">the X position to start the table</param>
        /// <param name="AWidthAvailable">AWidthAvailable</param>
        /// <param name="rows"></param>
        /// <param name="ARowsFittingOnPage">number of rows that fitted on the page</param>
        /// <returns>height of table</returns>
        public virtual float PrintTable(float AXPos, float AWidthAvailable, List <TTableRowGfx>rows, out Int32 ARowsFittingOnPage)
        {
            float origYPos = CurrentYPos;

            ARowsFittingOnPage = 0;

            foreach (TTableRowGfx row in rows)
            {
                float RowYPos = CurrentYPos;
                row.contentHeight = 0;

                // for each row, start again at the beginning of the line
                float currentXPos = AXPos;

                foreach (TTableCellGfx cell in row.cells)
                {
                    // for each cell, start again at the top of the table
                    CurrentYPos = RowYPos;
                    CurrentXPos = currentXPos + Cm(0.1f);

                    PushCurrentState();

                    if (cell.bold)
                    {
                        CurrentFont = eFont.eDefaultBoldFont;
                    }

                    FCurrentState.FNoWrap = cell.nowrap;
                    CurrentAlignment = cell.align;
                    XmlNode LocalNode = cell.content;
                    cell.contentHeight = FPrinterLayout.RenderContent(CurrentXPos, cell.contentWidth - Cm(0.2f), ref LocalNode);
                    LineFeed();
                    cell.contentHeight = CurrentYPos - RowYPos + Cm(0.1f);

                    if (cell.contentHeight > row.contentHeight)
                    {
                        row.contentHeight = cell.contentHeight;
                    }

                    PopCurrentState();

                    currentXPos += cell.contentWidth;
                }

                // draw the border
                currentXPos = AXPos;

                foreach (TTableCellGfx cell in row.cells)
                {
                    CurrentYPos = RowYPos;

                    if (cell.borderWidth > 0)
                    {
                        float horizontalDiff = Cm(0.1f);

                        if (cell.borderBitField == (TTableCellGfx.BOTTOM | TTableCellGfx.TOP | TTableCellGfx.LEFT | TTableCellGfx.RIGHT))
                        {
                            DrawRectangle(cell.borderWidth, currentXPos, CurrentYPos - horizontalDiff, cell.contentWidth, row.contentHeight);
                        }
                        else
                        {
                            if ((cell.borderBitField & TTableCellGfx.TOP) != 0)
                            {
                                DrawLine(cell.borderWidth,
                                    currentXPos,
                                    CurrentYPos - horizontalDiff,
                                    currentXPos + cell.contentWidth,
                                    CurrentYPos - horizontalDiff);
                            }

                            if ((cell.borderBitField & TTableCellGfx.BOTTOM) != 0)
                            {
                                DrawLine(cell.borderWidth,
                                    currentXPos,
                                    CurrentYPos + row.contentHeight - horizontalDiff,
                                    currentXPos + cell.contentWidth,
                                    CurrentYPos + row.contentHeight - horizontalDiff);
                            }

                            if ((cell.borderBitField & TTableCellGfx.LEFT) != 0)
                            {
                                DrawLine(cell.borderWidth,
                                    currentXPos,
                                    CurrentYPos - 2,
                                    currentXPos,
                                    CurrentYPos + row.contentHeight - horizontalDiff);
                            }

                            if ((cell.borderBitField & TTableCellGfx.RIGHT) != 0)
                            {
                                DrawLine(cell.borderWidth,
                                    currentXPos + cell.contentWidth,
                                    CurrentYPos - horizontalDiff,
                                    currentXPos + cell.contentWidth,
                                    CurrentYPos + row.contentHeight - horizontalDiff);
                            }
                        }
                    }

                    currentXPos += cell.contentWidth;
                }

                CurrentYPos += row.contentHeight;

                // do we need a page break?
                if (!ValidYPos())
                {
                    break;
                }

                ARowsFittingOnPage++;
            }

            return CurrentYPos - origYPos;
        }
    }
}