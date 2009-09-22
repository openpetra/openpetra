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
using System.Collections.Generic;
using Ict.Common.Printing;

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
        eSmallPrintFont
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
    /// The TPrinter class helps to print.
    ///
    /// This is a generic class for printing.
    /// Most of the functions are abstract and need to be implemented by a derived class.
    /// This is TxtPrinter and GfxPrinter, that are derived from TPrinter
    /// </summary>
    public abstract class TPrinter
    {
        /// <summary>todoComment</summary>
        protected ePrintingMode FPrintingMode;

        /// <summary>todoComment</summary>
        protected eOrientation FOrientation;

        /// <summary>todoComment</summary>
        protected Int32 FCurrentPageNr;

        /// <summary>todoComment</summary>
        protected Int32 FNumberOfPages;

        /// <summary>todoComment</summary>
        protected float FCurrentXPos;

        /// <summary>current y Position on page, in current display unit</summary>
        protected float FCurrentYPos;

        /// <summary>backup of current y position; needed for simulation</summary>
        protected float FCurrentYPosBackup;

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
        protected eFont FCurrentFont;

        /// <summary>todoComment</summary>
        protected eAlignment FCurrentAlignment = eAlignment.eLeft;

        /// <summary>todoComment</summary>
        protected TPrinterLayout FPrinterLayout;

        /// <summary>todoComment</summary>
        public System.Int32 CurrentPageNr
        {
            get
            {
                return FCurrentPageNr;
            }
        }

        /// <summary>this only has a valid value after the first rendering of the report</summary>
        public System.Int32 NumberOfPages
        {
            get
            {
                return FNumberOfPages;
            }
        }

        /// <summary>todoComment</summary>
        public float CurrentYPos
        {
            get
            {
                return FCurrentYPos;
            }

            set
            {
                FCurrentYPos = value;
            }
        }

        /// <summary>todoComment</summary>
        public float CurrentXPos
        {
            get
            {
                return FCurrentXPos;
            }

            set
            {
                FCurrentXPos = value;
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
                return FCurrentFont;
            }
            set
            {
                FCurrentFont = value;
            }
        }

        /// <summary>todoComment</summary>
        public eAlignment CurrentAlignment
        {
            get
            {
                return FCurrentAlignment;
            }
            set
            {
                FCurrentAlignment = value;
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
            return FCurrentYPos + 1;
        }

        /// <summary>
        /// Line Feed; increases the current y position by the given height
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public float LineFeed(float height)
        {
            FCurrentYPos += height;
            return FCurrentYPos;
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

        #region Calculate X position

        /// <summary>
        /// Converts the given value in cm to the currently used measurement unit
        ///
        /// </summary>
        /// <returns>void</returns>
        public abstract float Cm(float AValueInCm);

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
        /// todoComment
        /// </summary>
        public virtual void DrawBitmap(string APath,
            float AXPos,
            float AYPos,
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
            FPrintingMode = ePrintingMode.eDoPrint;
            FCurrentPageNr = 0;
            FNumberOfPages = 0;
            FCurrentFont = eFont.eDefaultFont;
            FPageFooterSpace = 0;
        }

        /// <summary>
        /// sets the orientation of the page
        /// </summary>
        /// <param name="AOrientation"></param>
        /// <param name="APrinterLayout"></param>
        public virtual void Init(eOrientation AOrientation, TPrinterLayout APrinterLayout)
        {
            FPrinterLayout = APrinterLayout;
            FOrientation = AOrientation;
        }

        /// <summary>
        /// start the simulation of printing; nothing is actually printed, but the CurrentYPos is increased
        ///
        /// </summary>
        /// <returns>void</returns>
        public void StartSimulatePrinting()
        {
            FCurrentYPosBackup = FCurrentYPos;
            FPrintingMode = ePrintingMode.eDoSimulate;
        }

        /// <summary>
        /// finish the simulation of printing; the actual CurrentYPos is restored
        ///
        /// </summary>
        /// <returns>void</returns>
        public void FinishSimulatePrinting()
        {
            FCurrentYPos = FCurrentYPosBackup;
            FPrintingMode = ePrintingMode.eDoPrint;
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
        /// renders a table at the current FCurrentYPos
        /// does not support rowspan at the moment
        /// colspan might be implemented in the generation of the TTableRowGfx structure
        /// does not care about fitting on page etc.
        /// </summary>
        /// <param name="AXPos">the X position to start the table</param>
        /// <param name="AWidthAvailable">AWidthAvailable</param>
        /// <param name="rows"></param>
        /// <returns>s height of table</returns>
        public virtual float PrintTable(float AXPos, float AWidthAvailable, List <TTableRowGfx>rows)
        {
            foreach (TTableRowGfx row in rows)
            {
                float currentYPos = FCurrentYPos;
                row.contentHeight = 0;

                // for each row, start again at the beginning of the line
                float currentXPos = AXPos;

                foreach (TTableCellGfx cell in row.cells)
                {
                    // for each cell, start again at the top of the table
                    FCurrentYPos = currentYPos;
                    FCurrentXPos = currentXPos;
                    cell.contentWidth =
                        (AWidthAvailable * cell.columnWidthInPercentage) / 100.0f;

                    eFont origFont = FCurrentFont;

                    if (cell.bold)
                    {
                        FCurrentFont = eFont.eDefaultBoldFont;
                    }

                    eAlignment origAlignment = FCurrentAlignment;
                    FCurrentAlignment = cell.align;
                    cell.contentHeight = FPrinterLayout.RenderContent(currentXPos, cell.contentWidth, ref cell.content);
                    LineFeed();
                    cell.contentHeight = FCurrentYPos - currentYPos;

                    if (cell.contentHeight > row.contentHeight)
                    {
                        row.contentHeight = cell.contentHeight;
                    }

                    FCurrentFont = origFont;
                    FCurrentAlignment = origAlignment;

                    currentXPos += cell.contentWidth;
                }

                // draw the border
                currentXPos = AXPos;

                foreach (TTableCellGfx cell in row.cells)
                {
                    FCurrentYPos = currentYPos;

                    if (cell.borderWidth > 0)
                    {
                        DrawRectangle(cell.borderWidth, currentXPos, FCurrentYPos, cell.contentWidth, row.contentHeight);
                    }

                    currentXPos += cell.contentWidth;
                }

                FCurrentYPos += row.contentHeight;
            }

            return -1;
        }
    }
}