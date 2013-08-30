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
using System.Collections;
using Ict.Common.Printing;
using Ict.Common;
using System.Drawing.Printing;
using System.Drawing;
using System.Reflection;
using Ict.Petra.Shared;
//using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Shared.MReporting
{
    /// <summary>
    /// Here the specific layout of the report is defined.
    /// It is independent of the printer, so it works both for graphics and text.
    /// </summary>
    public class TReportPrinterLayout : TReportPrinterCommon
    {
        /// <summary>default value for position; they can be overwritten by the report definition</summary>
        public const Int32 HEADERPAGELEFT1_POS = 0;

        /// <summary>default value for position; they can be overwritten by the report definition</summary>
        public const Int32 HEADERPAGELEFT2_POS = 2;

        /// <summary>default value for position; they can be overwritten by the report definition</summary>
        public const Int32 COLUMNDATASTART_POS = 4;

        /// <summary>default value for position; they can be overwritten by the report definition</summary>
        public const Int32 COLUMN_WIDTH = 3;

        /// <summary>default value for position; they can be overwritten by the report definition</summary>
        public const float COLUMNLEFT1_POS = 0.5f;

        /// <summary>default value for position; they can be overwritten by the report definition</summary>
        public const float COLUMNLEFT2_POS = 2.5f;

        /// <summary>True: Wrap the text in a column if it is to long. Otherwise cut it </summary>
        private bool FWrapColumn;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AResult"></param>
        /// <param name="AParameters"></param>
        /// <param name="APrinter"></param>
        /// <param name="AWrapColumn">True: Wrap text in the column if it is to long. Otherwise cut it</param>
        public TReportPrinterLayout(TResultList AResult, TParameterList AParameters, TPrinter APrinter, bool AWrapColumn) : base(AResult, AParameters,
                                                                                                                                APrinter)
        {
            FWrapColumn = AWrapColumn;

            if (AParameters.Get("ReportWidth").ToDouble() > 20)
            {
                APrinter.Init(eOrientation.eLandscape, this, eMarginType.eDefaultMargins);
            }
            else
            {
                APrinter.Init(eOrientation.ePortrait, this, eMarginType.eDefaultMargins);
            }
        }

        /// <summary>
        /// The page size for reports defaults to A4 at the moment
        /// </summary>
        /// <param name="APaperKind"></param>
        /// <param name="AMargins"></param>
        /// <param name="AWidthInPoint"></param>
        /// <param name="AHeightInPoint"></param>
        /// <returns></returns>
        public override bool GetPageSize(out PaperKind APaperKind, out Margins AMargins, out float AWidthInPoint, out float AHeightInPoint)
        {
            APaperKind = PaperKind.A4;
            AMargins = new Margins(20, 20, 20, 39);
            AWidthInPoint = -1;
            AHeightInPoint = -1;

            return true;
        }

        /// <summary>
        /// Print a report, initialise the page numbers, print header, body and footer;
        /// At the moment, this is only used for the TxtPrinter.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void PrintReport()
        {
            StartPrintDocument();
            PrintPageHeader();
            PrintPageBody();
            PrintPageFooter();
        }

        /// <summary>
        /// print the page header
        /// </summary>
        public override void PrintPageHeader()
        {
            String pages;
            Version clientVersion;

            FPrinter.PrintString(Get("ControlSource", ReportingConsts.HEADERTITLE1), eFont.eHeadingFont, eAlignment.eCenter);
            FPrinter.PrintString(StringHelper.DateToLocalizedString(FTimePrinted), eFont.eDefaultFont, eAlignment.eRight);

            // todo:
            // should page, title, sitename, date be bold, as in Petra 2.1?
            // requested by, petra version, is not bold
            if (FPrinter.NumberOfPages == 0)
            {
                pages = "Page " + FPrinter.CurrentPageNr.ToString();
            }
            else
            {
                pages = "Page " + FPrinter.CurrentPageNr.ToString() + " of " + FPrinter.NumberOfPages.ToString();
            }

            FPrinter.PrintString(pages, eFont.eDefaultFont, eAlignment.eLeft);
            FPrinter.LineFeed(eFont.eHeadingFont);
            FPrinter.PrintString(Get("ControlSource", ReportingConsts.HEADERTITLE2), eFont.eDefaultBoldFont, eAlignment.eCenter);
            FPrinter.PrintString(FTimePrinted.ToString("T"), eFont.eDefaultFont, eAlignment.eRight);
            FPrinter.LineFeed(eFont.eDefaultBoldFont);
            FPrinter.PrintString("Report requested by: " + UserInfo.GUserInfo.UserID, eFont.eDefaultFont, eAlignment.eLeft);
            clientVersion = System.Reflection.Assembly.GetAssembly(typeof(TReportPrinterLayout)).GetName().Version;
            FPrinter.PrintString("Version: " + clientVersion.Major.ToString() + "." +
                clientVersion.Minor.ToString() + "." +
                clientVersion.Build.ToString() + "." +
                clientVersion.Revision.ToString(),
                eFont.eDefaultFont, eAlignment.eRight);
            FPrinter.LineFeed(eFont.eHeadingFont);
            FPrinter.LineFeed(eFont.eDefaultFont);
            FPrinter.PrintString(Get("ControlSource", ReportingConsts.HEADERDESCR1), eFont.eDefaultFont, eAlignment.eLeft);
            FPrinter.PrintString(Get("ControlSource", ReportingConsts.HEADERPERIOD), eFont.eDefaultFont, eAlignment.eRight);
            FPrinter.LineFeed(eFont.eDefaultFont);
            FPrinter.PrintString(Get("ControlSource", ReportingConsts.HEADERDESCR2), eFont.eDefaultFont, eAlignment.eLeft);
            FPrinter.PrintString(Get("ControlSource", ReportingConsts.HEADERPERIOD2), eFont.eDefaultFont, eAlignment.eRight);
            FPrinter.PrintString(Get("ControlSource", ReportingConsts.HEADERTYPE), eFont.eDefaultFont, eAlignment.eCenter);
            FPrinter.LineFeed(eFont.eDefaultFont);
            FPrinter.PrintString(Get("ControlSource", ReportingConsts.HEADERDESCR3), eFont.eDefaultFont, eAlignment.eLeft);
            FPrinter.PrintString(Get("ControlSource", ReportingConsts.HEADERPERIOD3), eFont.eDefaultFont, eAlignment.eRight);
            FPrinter.LineFeed(eFont.eDefaultFont);
            FPrinter.LineFeed(eFont.eDefaultFont);
            FPrinter.PrintString(Get("ControlSource",
                    ReportingConsts.HEADERPAGELEFT1), eFont.eDefaultBoldFont, GetPosition(ReportingConsts.HEADERPAGELEFT1, -1, HEADERPAGELEFT1_POS));
            FPrinter.PrintString(Get("ControlSource",
                    ReportingConsts.HEADERPAGELEFT2), eFont.eDefaultBoldFont, GetPosition(ReportingConsts.HEADERPAGELEFT2, -1, HEADERPAGELEFT2_POS));
            PrintColumnCaptions();
            FPrinter.DrawLine(FPrinter.LeftMargin, FPrinter.Width, eLinePosition.eAbove, eFont.eDefaultBoldFont);
            FPrinter.LineFeed(eFont.eDefaultFont);
        }

        /// <summary>
        /// prints the captions of the columns;
        /// prepare the footer lines for long captions;
        /// is called by PrintPageHeader
        ///
        /// </summary>
        /// <returns>void</returns>
        protected override void PrintColumnCaptions()
        {
            int columnNr;
            String Caption;

            System.Int32 NumberFooterLines;
            System.Int32 NumberFootNotes;
            float YPosStartCaptions;
            float LowestYPosCaptions;
            float FooterXPos;
            bool NeedCaptionInFooter;
            String CaptionParameter;
            float StringWidth;
            Int32 oldCurrentSubReport;
            eAlignment CaptionAlignment;
            oldCurrentSubReport = FParameters.Get("CurrentSubReport").ToInt();
            FParameters.Add("CurrentSubReport", -1);
            FNumberColumns = -1;

            for (columnNr = 0; columnNr <= Convert.ToInt32(Get("MaxDisplayColumns")) - 1; columnNr += 1)
            {
                if (Get("param_calculation", columnNr) != "")
                {
                    FNumberColumns = columnNr + 1;
                }
            }

            YPosStartCaptions = FPrinter.CurrentYPos;
            LowestYPosCaptions = FPrinter.CurrentYPos;
            NumberFooterLines = 0;
            NumberFootNotes = 0;
            FooterXPos = FPrinter.LeftMargin;

            for (columnNr = 0; columnNr <= FNumberColumns - 1; columnNr += 1)
            {
                // go back to top of the caption rows.
                FPrinter.CurrentYPos = YPosStartCaptions;
                NeedCaptionInFooter = false;

                // should we use the normal or the short version of the caption?
                // if a short version exists, it will be used, and the long version will be printed in the footer of the page
                CaptionParameter = "ColumnCaption";

                if (FParameters.Exists("ColumnShortCaption", columnNr))
                {
                    CaptionParameter = "ColumnShortCaption";
                    NeedCaptionInFooter = true;
                }

                CaptionAlignment = eAlignment.eCenter;

                if (Get("ColumnAlign", columnNr) == "left")
                {
                    CaptionAlignment = eAlignment.eLeft;
                }

                Caption = Get(CaptionParameter, columnNr);

                if ((!FPrinter.PrintStringAndFits(Caption, eFont.eDefaultBoldFont, GetPosition(columnNr, -1, 0),
                         GetWidth(columnNr, -1, 0), CaptionAlignment)))
                {
                    // (Columns[columnNr] as RectangleF).Left
                    // (Columns[columnNr] as RectangleF).Width
                    // if the text is still too long, add a line to the footer
                    NeedCaptionInFooter = true;
                }

                FPrinter.LineFeed(eFont.eDefaultFont);
                Caption = Get(CaptionParameter + '2', columnNr);

                if (Caption.Length > 0)
                {
                    if ((!FPrinter.PrintStringAndFits(Caption, eFont.eDefaultFont,
                             GetPosition(columnNr, -1, 0), GetWidth(columnNr, -1, 0), eAlignment.eCenter)))
                    {
                        // (Columns[columnNr] as RectangleF).Left
                        // (Columns[columnNr] as RectangleF).Width
                        // if the text is still too long, add a line to the footer
                        NeedCaptionInFooter = true;
                    }

                    FPrinter.LineFeed(eFont.eDefaultFont);
                }

                // 3rd header line
                Caption = Get(CaptionParameter + "3", columnNr);

                if (Caption.Length > 0)
                {
                    if ((!FPrinter.PrintStringAndFits(Caption, eFont.eDefaultFont,
                             GetPosition(columnNr, -1, 0),
                             GetWidth(columnNr, -1, 0),
                             eAlignment.eCenter)))
                    {
                        // (Columns[columnNr] as RectangleF).Left
                        // (Columns[columnNr] as RectangleF).Width
                        // if the text is still too long, add a line to the footer
                        NeedCaptionInFooter = true;
                    }

                    FPrinter.LineFeed(eFont.eDefaultFont);
                }

                if (NeedCaptionInFooter)
                {
                    NumberFootNotes = NumberFootNotes + 1;

                    // save current lowest point
                    if (FPrinter.CurrentYPos > LowestYPosCaptions)
                    {
                        LowestYPosCaptions = FPrinter.CurrentYPos;
                    }

                    // go back into the first line of the caption
                    FPrinter.CurrentYPos = YPosStartCaptions;
                    FPrinter.PrintString("(" + NumberFootNotes.ToString() + ")",
                        eFont.eSmallPrintFont,
                        GetPosition(columnNr, -1, 0),
                        GetWidth(columnNr, -1, 0),
                        eAlignment.eRight);

                    // (Columns[columnNr] as RectangleF).Left
                    // (Columns[columnNr] as RectangleF).Width
                    Caption = Convert.ToString(NumberFootNotes) + ": " +
                              Get("ColumnCaption", columnNr) + " " +
                              Get("ColumnCaption2", columnNr) + " " +
                              Get("ColumnCaption3", columnNr);
                    FParameters.Add("LongCaption", new TVariant(Caption), columnNr);

                    // simulate the printing of the footer, to know how many rows are needed in the footer
                    StringWidth = FPrinter.GetWidthString(Caption, eFont.eSmallPrintFont);

                    if (!FPrinter.ValidXPos(FooterXPos + StringWidth))
                    {
                        FooterXPos = FPrinter.LeftMargin;
                        NumberFooterLines = NumberFooterLines + 1;
                    }

                    FooterXPos = FooterXPos + StringWidth + FPrinter.GetWidthString("a", eFont.eSmallPrintFont) * 7;
                }

                // store the highest caption
                if (FPrinter.CurrentYPos > LowestYPosCaptions)
                {
                    LowestYPosCaptions = FPrinter.CurrentYPos;
                }
            }

            // feed forward to the lowest position after the captions
            FPrinter.CurrentYPos = LowestYPosCaptions;

            // if the line has not just begun, then add one more line
            if (FooterXPos != FPrinter.LeftMargin)
            {
                NumberFooterLines = NumberFooterLines + 1;
            }

            FPrinter.SetPageFooterSpace(NumberFooterLines, eFont.eSmallPrintFont);
            FParameters.Add("CurrentSubReport", oldCurrentSubReport);
        }

        /// <summary>
        /// print one value, into the given column
        /// </summary>
        /// <param name="columnNr"></param>
        /// <param name="level"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        protected override bool PrintColumn(Int32 columnNr, Int32 level, TVariant column)
        {
            String s;
            float position;
            float width;
            bool linePrinted;

            linePrinted = false;
            position = GetPosition(columnNr, level, 0);
            width = GetWidth(columnNr, level, 0);
            s = column.ToString();

            if (s.Length != 0)
            {
                linePrinted = true;

                if (FParameters.Get("LineAbove", columnNr, level, eParameterFit.eAllColumnFit).ToBool() == true)
                {
                    FPrinter.DrawLine(position, position + width, eLinePosition.eAbove, eFont.eDefaultFont);
                }

                if (FWrapColumn)
                {
                    FPrinter.PrintStringWrap(s, eFont.eDefaultFont, position, width, GetAlignment(columnNr, level, eAlignment.eRight));
                }
                else
                {
                    FPrinter.PrintString(s, eFont.eDefaultFont, position, width, GetAlignment(columnNr, level, eAlignment.eRight));
                }

                if (FParameters.Get("LineBelow", columnNr, level, eParameterFit.eAllColumnFit).ToBool() == true)
                {
                    FPrinter.DrawLine(position, position + width, eLinePosition.eBelow, eFont.eDefaultFont);
                }

                FPrinter.LineFeed(eFont.eDefaultFont);
            }

            return linePrinted;
        }

        /// <summary>
        /// print all columns of a given row
        /// </summary>
        /// <param name="ARow"></param>
        /// <returns></returns>
        protected bool PrintColumns(TResult ARow)
        {
            Int32 columnNr;
            float YPosBefore;
            float LowestYPos;

            YPosBefore = FPrinter.CurrentYPos;

            // we need at least one row, otherwise the strings on the left are written in the same row
            FPrinter.LineFeed(eFont.eDefaultFont);
            LowestYPos = FPrinter.CurrentYPos;

            for (columnNr = 0; columnNr <= FNumberColumns - 1; columnNr += 1)
            {
                FPrinter.CurrentYPos = YPosBefore;
                PrintColumn(columnNr, ARow.depth, ARow.column[columnNr]);

                // store the highest column
                if (FPrinter.CurrentYPos > LowestYPos)
                {
                    LowestYPos = FPrinter.CurrentYPos;
                }
            }

            FPrinter.CurrentYPos = LowestYPos;
            return true;
        }

        /// <summary>
        /// print the lowest level (has no child levels)
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override Boolean PrintLowestLevel(TResult row)
        {
            Boolean ReturnValue;
            String descr0 = "";
            String descr1 = "";

            if (!FPrinter.ValidYPos())
            {
                // not enoughSpace
                ReturnValue = false;
                FNextElementToPrint = row.childRow;
                FNextElementLineToPrint[row.depth] = eStageElementPrinting.eDetails;
            }
            else
            {
                // can either print descr or header
                if ((row.descr != null) && (row.descr[0].ToString().Length > 0))
                {
                    descr0 = row.descr[0].ToString();
                }
                else if ((row.header != null) && (row.header[0].ToString().Length > 0))
                {
                    descr0 = row.header[0].ToString();
                }

                if ((row.descr != null) && (row.descr[1].ToString().Length > 0))
                {
                    descr1 = row.descr[1].ToString();
                }
                else if ((row.header != null) && (row.header[1].ToString().Length > 0))
                {
                    descr1 = row.header[1].ToString();
                }

                FPrinter.PrintString(descr0, eFont.eDefaultFont, GetPosition(ReportingConsts.COLUMNLEFT + 1, row.depth,
                        FPrinter.Cm(COLUMNLEFT1_POS)),
                    GetWidth(ReportingConsts.COLUMNLEFT + 1,
                        row.depth,
                        FPrinter.Cm(1)),
                    GetAlignment(ReportingConsts.COLUMNLEFT + 1,
                        row.depth, eAlignment.eLeft));
                FPrinter.PrintString(descr1, eFont.eDefaultFont, GetPosition(ReportingConsts.COLUMNLEFT + 2,
                        row.depth,
                        FPrinter.Cm(COLUMNLEFT2_POS)),
                    GetWidth(ReportingConsts.COLUMNLEFT + 2, row.depth,
                        FPrinter.Cm(3)),
                    GetAlignment(ReportingConsts.COLUMNLEFT + 2,
                        row.depth, eAlignment.eLeft));
                PrintColumns(row);
                FNextElementLineToPrint[row.depth] = eStageElementPrinting.eFinished;
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// print the header of a normal level
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override Boolean PrintNormalLevelHeader(TResult row)
        {
            Boolean ReturnValue;
            Boolean linePrinted;

            if (!FPrinter.ValidYPos())
            {
                // not enoughSpace
                FNextElementToPrint = row.childRow;
                FNextElementLineToPrint[row.depth] = eStageElementPrinting.eHeader;
                ReturnValue = false;
            }
            else
            {
                linePrinted = FPrinter.PrintString(row.header[0].ToString(),
                    eFont.eDefaultFont,
                    GetPosition(ReportingConsts.HEADERCOLUMN + 1, row.depth,
                        FPrinter.Cm(COLUMNLEFT1_POS)),
                    GetWidth(ReportingConsts.HEADERCOLUMN + 1, row.depth,
                        FPrinter.Cm(3)),
                    GetAlignment(ReportingConsts.HEADERCOLUMN + 1, row.depth, eAlignment.eLeft));
                linePrinted = FPrinter.PrintString(row.header[1].ToString(),
                    eFont.eDefaultFont,
                    GetPosition(ReportingConsts.HEADERCOLUMN + 2, row.depth,
                        FPrinter.Cm(COLUMNLEFT2_POS)),
                    GetWidth(ReportingConsts.HEADERCOLUMN + 2, row.depth,
                        FPrinter.Cm(3)),
                    GetAlignment(ReportingConsts.HEADERCOLUMN + 1, row.depth, eAlignment.eLeft))
                              || linePrinted;

                if (linePrinted)
                {
                    FPrinter.LineFeed(eFont.eDefaultFont);
                }

                ReturnValue = true;
            }

            FNextElementLineToPrint[row.depth] = eStageElementPrinting.eDetails;
            return ReturnValue;
        }

        /// <summary>
        /// can either print or simulate
        /// </summary>
        /// <returns>s the current y position after printing or simulating
        /// </returns>
        protected override double PrintNormalLevelFooter(TResult row)
        {
            Boolean linePrinted;

            linePrinted = false;

            // footer
            if (FParameters.Get("SpaceLineAbove", -1, row.depth, eParameterFit.eExact).ToBool() == true)
            {
                FPrinter.LineSpaceFeed(eFont.eDefaultFont);
            }

            if (FParameters.Get("FullLineAbove", -1, row.depth, eParameterFit.eExact).ToBool() == true)
            {
                FPrinter.DrawLine(FPrinter.LeftMargin, FPrinter.Width, eLinePosition.eAbove, eFont.eDefaultFont);
                FPrinter.LineSpaceFeed(eFont.eDefaultFont);
            }

            FPrinter.PrintString(row.descr[0].ToString(), eFont.eDefaultFont,
                GetPosition(ReportingConsts.COLUMNLEFT + 1, row.depth,
                    FPrinter.Cm(COLUMNLEFT1_POS)),
                GetWidth(ReportingConsts.COLUMNLEFT + 1, row.depth, FPrinter.Cm(3)),
                GetAlignment(ReportingConsts.COLUMNLEFT + 1, row.depth, eAlignment.eLeft));
            PrintColumns(row);
            linePrinted = FPrinter.PrintString(row.descr[1].ToString(), eFont.eDefaultFont,
                GetPosition(ReportingConsts.COLUMNLEFT + 1, row.depth,
                    FPrinter.Cm(COLUMNLEFT1_POS)),
                GetWidth(ReportingConsts.COLUMNLEFT + 1, row.depth, FPrinter.Cm(3)),
                GetAlignment(ReportingConsts.COLUMNLEFT + 1, row.depth, eAlignment.eLeft));

            if (linePrinted)
            {
                FPrinter.LineFeed(eFont.eDefaultFont);
            }

            if (FParameters.Get("FullLineBelow", -1, row.depth, eParameterFit.eExact).ToBool() == true)
            {
                FPrinter.DrawLine(FPrinter.LeftMargin, FPrinter.Width, eLinePosition.eAbove, eFont.eDefaultFont);
                FPrinter.LineSpaceFeed(eFont.eDefaultFont);
            }

            if (FParameters.Get("SpaceLineBelow", -1, row.depth, eParameterFit.eExact).ToBool() == true)
            {
                FPrinter.LineSpaceFeed(eFont.eDefaultFont);
            }

            FNextElementLineToPrint[row.depth] = eStageElementPrinting.eFinished;
            return FPrinter.CurrentYPos;
        }

        /// <summary>
        /// print the footer of the page;
        /// can contain eg. long caption lines that did not fit at top of the columns
        /// </summary>
        public override void PrintPageFooter()
        {
            System.Int32 columnNr;
            String Caption;
            float StringWidth;
            float CurrentXPos;
            Int32 oldCurrentSubReport;

            if (FPrinter.PageFooterSpace == 0)
            {
                // this printer does not support page footers
                return;
            }

            FPrinter.LineFeedToPageFooter();
            FPrinter.DrawLine(FPrinter.LeftMargin, FPrinter.Width, eLinePosition.eAbove, eFont.eDefaultBoldFont);
            FPrinter.LineSpaceFeed(eFont.eSmallPrintFont);

            /*
             * TLogging.Log('TopMargin in cm: ' + Convert.ToString(TopMargin* 2.54));
             * TLogging.Log('BottomMargin in cm: ' + Convert.ToString(BottomMargin* 2.54));
             * TLogging.Log('Height in cm: ' + Convert.ToString(Height* 2.54));
             * TLogging.Log('Footerspace in cm: ' + Convert.ToString(FPageFooterSpace));
             * TLogging.Log('smallprintheight in cm: ' + Convert.ToString(SmallPrintFont.GetHeight(ev.Graphics)));
             * TLogging.Log('defaultheight in cm: ' + Convert.ToString(DefaultFont.GetHeight(ev.Graphics)));
             */
            oldCurrentSubReport = FParameters.Get("CurrentSubReport").ToInt();
            FParameters.Add("CurrentSubReport", -1);
            CurrentXPos = FPrinter.LeftMargin;

            for (columnNr = 0; columnNr <= FNumberColumns - 1; columnNr += 1)
            {
                if (FParameters.Exists("LongCaption", columnNr))
                {
                    Caption = Get("LongCaption", columnNr);
                    StringWidth = FPrinter.GetWidthString(Caption, eFont.eSmallPrintFont);

                    if (FPrinter.ValidXPos(CurrentXPos + StringWidth))
                    {
                        // can print in same line
                        FPrinter.PrintString(Caption, eFont.eSmallPrintFont, CurrentXPos);
                    }
                    else
                    {
                        FPrinter.LineFeed(eFont.eSmallPrintFont);
                        CurrentXPos = FPrinter.LeftMargin;
                        FPrinter.PrintString(Caption, eFont.eSmallPrintFont, CurrentXPos);
                    }

                    CurrentXPos = CurrentXPos + StringWidth + FPrinter.GetWidthString("a", eFont.eSmallPrintFont) * 7;
                }
            }

            FParameters.Add("CurrentSubReport", oldCurrentSubReport);
        }
    }
}