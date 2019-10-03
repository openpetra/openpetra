//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2019 by OM International
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
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Fonts;

namespace Ict.Common.Printing
{
    /// <summary>
    /// this printer uses PdfSharp to print to PDF.
    /// </summary>
    public class TPdfPrinter : TGfxPrinter
    {
        private XGraphics FXGraphics;
        private PdfDocument FPdfDocument;

        /// todoComment
        public XFont FXDefaultFont;

        /// todoComment
        public XFont FXDefaultBoldFont;

        /// todoComment
        public XFont FXHeadingFont;

        /// todoComment
        public XFont FXSmallPrintFont;

        /// <summary>
        /// for printing bar codes
        /// </summary>
        public XFont FXBarCodeFont;

        /// todoComment
        public XFont FXBiggestLastUsedFont;
        private XStringFormat FXLeft;
        private XStringFormat FXRight;
        private XStringFormat FXCenter;
        private XPen FXBlackPen;

        /// reset the static variables for each Web Request call.
        public static void ResetStaticVariables()
        {
            MonoFontResolver.ResetStaticVariables();
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TPdfPrinter(ePrinterBehaviour APrinterBehaviour): base(APrinterBehaviour)
        {
            if (GlobalFontSettings.FontResolver == null)
            {
                GlobalFontSettings.FontResolver = new MonoFontResolver();
            }
        }

        /// the fonts need to be a little bit bigger so that they have the same size as the GfxPrinter?
        private const int XFONTSIZE = 3;

        /// <summary>
        /// initialise the fonts and pens.
        /// this can only happen when the Graphics and GraphicsUnit are known.
        /// </summary>
        private void InitFontsAndPens()
        {
            FXBlackPen = new XPen(XColor.FromKnownColor(XKnownColor.Black), Cm(0.05f));

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);

            // the fonts need to be a little bit bigger so that they have the same size as the GfxPrinter?
            FXSmallPrintFont = new XFont("Arial", 0.12, XFontStyle.Regular, options); // Point(6 + XFONTSIZE)
            FXDefaultFont = new XFont("Arial", 0.14, XFontStyle.Regular, options); // Point(8 + XFONTSIZE)
            FXDefaultBoldFont = new XFont("Arial", 0.14, XFontStyle.Bold, options); // Point(8 + XFONTSIZE)
            FXHeadingFont = new XFont("Arial", 0.16, XFontStyle.Bold, options); // Point(10 + XFONTSIZE)

            // using GPL Font Code 128 from Grand Zebu http://grandzebu.net/
            FXBarCodeFont = new XFont("Code 128", 0.45, XFontStyle.Regular, options); // Point(10 + XFONTSIZE)

            FXBiggestLastUsedFont = FXDefaultFont;
            FXRight = new XStringFormat();
            FXRight.Alignment = XStringAlignment.Far;
            FXLeft = new XStringFormat();
            FXLeft.Alignment = XStringAlignment.Near;
            FXCenter = new XStringFormat();
            FXCenter.Alignment = XStringAlignment.Center;
        }

        private SortedList <string, XFont>FXFontCache = new SortedList <string, XFont>();

        private XFont GetXFont(eFont AFont)
        {
            XFont ReturnValue = FXDefaultFont;

            switch (AFont)
            {
                case eFont.eDefaultFont:
                    ReturnValue = FXDefaultFont;
                    break;

                case eFont.eDefaultBoldFont:
                    ReturnValue = FXDefaultBoldFont;
                    break;

                case eFont.eHeadingFont:
                    ReturnValue = FXHeadingFont;
                    break;

                case eFont.eSmallPrintFont:
                    ReturnValue = FXSmallPrintFont;
                    break;

                case eFont.eBarCodeFont:
                    ReturnValue = FXBarCodeFont;
                    break;
            }

            string id = ReturnValue.FontFamily.Name + ReturnValue.Size.ToString() + ReturnValue.Style.ToString();

            if (!FXFontCache.ContainsKey(id))
            {
                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);
                FXFontCache.Add(id, new XFont(ReturnValue.FontFamily.Name, ReturnValue.Size /*+XFONTSIZE*/, ReturnValue.Style, options));
            }

            ReturnValue = FXFontCache[id];

            return ReturnValue;
        }

        /// <summary>
        /// update the biggest last used font for the next line feed
        /// </summary>
        /// <param name="AFont"></param>
        protected override bool UpdateBiggestLastUsedFont(eFont AFont)
        {
            if ((FXBiggestLastUsedFont == null) || (GetXFont(AFont).Height > FXBiggestLastUsedFont.Height))
            {
                FXBiggestLastUsedFont = GetXFont(AFont);
                return true;
            }

            return false;
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
                FPageFooterSpace = ((float)Convert.ToDouble(ANumberOfLines) + 0.5f) * GetXFont(AFont).Height * CurrentLineHeight +
                                   FXDefaultFont.Height;
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
            CurrentYPos = FTopMargin + FHeight - FPageFooterSpace + FXDefaultFont.Height * CurrentLineHeight;
            return CurrentYPos;
        }

        private XStringFormat GetXStringFormat(eAlignment AAlign)
        {
            XStringFormat ReturnValue;

            ReturnValue = FXLeft;

            switch (AAlign)
            {
                case eAlignment.eDefault:
                    ReturnValue = FXLeft;
                    break;

                case eAlignment.eLeft:
                    ReturnValue = FXLeft;
                    break;

                case eAlignment.eRight:
                    ReturnValue = FXRight;
                    break;

                case eAlignment.eCenter:
                    ReturnValue = FXCenter;
                    break;
            }

            return ReturnValue;
        }

        private XRect CalculatePrintStringRectangle(float ALeft, float ATop, float AWidth, eFont AFont)
        {
            float MyYPos = ATop;

            if (Environment.OSVersion.ToString().StartsWith("Unix"))
            {
                // on Mono/Unix, we have problems with the y position being too high on the page
                MyYPos += GetFontHeight(AFont);
            }

            return new XRect(ALeft, MyYPos, AWidth, GetFontHeight(AFont) + 0.2f);
        }

        /// <summary>
        /// prints into the current line, aligned x position
        ///
        /// </summary>
        public override Boolean PrintString(String ATxt, eFont AFont, eAlignment AAlign)
        {
            XRect rect = CalculatePrintStringRectangle(FLeftMargin, CurrentYPos, FWidth, AFont);

            if (PrintingMode == ePrintingMode.eDoPrint)
            {
                ATxt = GetFittedText(ATxt, AFont, (float)rect.Width);
                FXGraphics.DrawString(ATxt, GetXFont(AFont), new XSolidBrush(XColors.Black), rect, GetXStringFormat(AAlign));
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
                FXGraphics.DrawString(ATxt, GetXFont(AFont), new XSolidBrush(XColors.Black), AXPos, CurrentYPos);
            }

            return (ATxt != null) && (ATxt.Length != 0);
        }

        /// <summary>
        /// prints into the current line, absolute x position with width and alignment
        /// </summary>
        /// <returns>true if something was printed</returns>
        public override Boolean PrintString(String ATxt, eFont AFont, float AXPos, float AWidth, eAlignment AAlign)
        {
            XRect rect = CalculatePrintStringRectangle(AXPos, CurrentYPos, AWidth, AFont);

            if (PrintingMode == ePrintingMode.eDoPrint)
            {
                if (FPrinterBehaviour == ePrinterBehaviour.eReport)
                {
                    ATxt = GetFittedText(ATxt, AFont, (float)rect.Width);
                }

                if ((AAlign == eAlignment.eCenter) && Environment.OSVersion.ToString().StartsWith("Unix"))
                {
                    // it seems on Mono/Unix, the string aligning to the center does not work properly. so we do it manually
                    rect = new XRect(rect.Left + (AWidth - GetWidthString(ATxt, AFont)) / 2.0f, rect.Top, rect.Height, rect.Width);
                    FXGraphics.DrawString(ATxt, GetXFont(AFont), new XSolidBrush(XColors.Black), rect, GetXStringFormat(eAlignment.eLeft));
                }
                else
                {
                    XStringFormat f = GetXStringFormat(AAlign);

                    //TLogging.Log("curr ypos " + CurrentYPos.ToString() + " " + AXPos.ToString() + " " + ATxt + AWidth.ToString());
                    FXGraphics.DrawString(ATxt, GetXFont(AFont), new XSolidBrush(XColors.Black), rect, f);
                }
            }

            return (ATxt != null) && (ATxt.Length != 0);
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
                YPos = CurrentYPos + GetFontHeight(AFont);
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
                FXGraphics.DrawLine(FXBlackPen, AXPos1, YPos, AXPos2, YPos);
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
                FXGraphics.DrawLine(FXBlackPen, AXPos1, AYPos1, AXPos2, AYPos2);
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
                FXGraphics.DrawRectangle(FXBlackPen, AXPos, AYPos, AWidth, AHeight);
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

            Bitmap img;

            try
            {
                img = new System.Drawing.Bitmap(APath);
            }
            catch (Exception e)
            {
                TLogging.Log("Problem reading image for printing to PDF: " + APath);
                TLogging.Log(e.ToString());
                throw new Exception("Problem reading image for printing to PDF: " + APath);
            }

            if (img != null)
            {
                DrawBitmapInternal(img, AXPos, AYPos, img.Size.Width, img.Size.Height);
            }
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

            Bitmap img;

            try
            {
                img = new System.Drawing.Bitmap(APath);
            }
            catch (Exception e)
            {
                TLogging.Log("Problem reading image for printing to PDF: " + APath);
                TLogging.Log(e.ToString());
                throw new Exception("Problem reading image for printing to PDF: " + APath);
            }

            float Height = img.Size.Height;

            if (AHeightPercentage != 0.0f)
            {
                Height = Height * AHeightPercentage;
            }
            else
            {
                Height = AHeight;
            }

            float Width = img.Size.Width;

            if (AHeightPercentage != 0.0f)
            {
                Width = Width * AWidthPercentage;
            }
            else
            {
                Width = AWidth;
            }

// there seem to be too many problems with this on Linux. On Linux, the size of the PDF is quite small anyway
#if DEACTIVATED
            if (false && (Width / img.Size.Width * 100 < 80))
            {
                // we should scale down the picture to make the result pdf smaller
                try
                {
                    Bitmap SmallerImg = new Bitmap(Convert.ToInt32(Width), Convert.ToInt32(Height));
                    using (Graphics g = Graphics.FromImage((Image)SmallerImg))
                    {
                        g.DrawImage(img, 0, 0, Convert.ToInt32(Width), Convert.ToInt32(Height));
                    }

                    // saving as PNG file because I get GDI+ status: InvalidParameter in Mono/XSP when trying to save to jpg
                    string ThumbPath = APath.Replace(Path.GetExtension(APath), "thumb.png");

                    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
                    ImageCodecInfo jpegCodecInfo = codecs[0];

                    foreach (ImageCodecInfo codec in codecs)
                    {
                        if (codec.FormatID == ImageFormat.Png.Guid)
                        {
                            jpegCodecInfo = codec;
                        }
                    }

                    EncoderParameters codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = new EncoderParameter(Encoder.Quality, 75);
                    SmallerImg.Save(ThumbPath, jpegCodecInfo, codecParams);
                    SmallerImg.Dispose();
                    img.Dispose();

                    File.Delete(ThumbPath);

                    img = new System.Drawing.Bitmap(ThumbPath);
                }
                catch (Exception)
                {
                }
            }
#endif

            DrawBitmapInternal(img, AXPos, AYPos, Width, Height);

            img.Dispose();
        }

        /// <summary>
        /// Draw the bitmap and move the cursor position
        /// </summary>
        /// <param name="img"></param>
        /// <param name="AXPos">in page units</param>
        /// <param name="AYPos">in page units</param>
        /// <param name="AWidth">in pixel</param>
        /// <param name="AHeight">in pixel</param>
        private void DrawBitmapInternal(Bitmap img, float AXPos, float AYPos, float AWidth, float AHeight)
        {
            AWidth = PixelHorizontal(AWidth);
            AHeight = PixelVertical(AHeight);

            if (PrintingMode == ePrintingMode.eDoPrint)
            {
// TODO                FXGraphics.DrawImage(XImage.FromGdiPlusImage(img), AXPos, AYPos, AWidth, AHeight);
            }

            CurrentYPos += AHeight;
            CurrentXPos += AWidth;
        }

        private float GetFontHeight(eFont AFont)
        {
            return GetFontHeight(GetXFont(AFont));
        }

        private float GetFontHeight(XFont AFont)
        {
            return (float)Math.Round(AFont.Size, 2);     //  .GetHeight(FXGraphics)
        }

        /// <summary>
        /// prints into the current line, into the given column
        /// </summary>
        /// <param name="ATxt"></param>
        /// <param name="AFont"></param>
        /// <returns>Return the width of the string, if it was printed in one line, using the given Font</returns>
        public override float GetWidthString(String ATxt, eFont AFont)
        {
            return (float)FXGraphics.MeasureString(ATxt, GetXFont(AFont), XStringFormats.Default).Width;
        }

        /// remember the rotation and transformation
        protected Stack <XGraphicsState>FGraphicsStateStack = new Stack <XGraphicsState>();


        /// <summary>
        /// save the state
        /// </summary>
        public override void SaveState()
        {
            base.SaveState();

            FGraphicsStateStack.Push(FXGraphics.Save());
        }

        /// <summary>
        /// restore the state
        /// </summary>
        public override void RestoreState()
        {
            base.RestoreState();

            FXGraphics.Restore(FGraphicsStateStack.Pop());
        }

        /// <summary>
        /// rotate the following output by some degrees, at the given position
        /// </summary>
        public override void RotateAtTransform(double ADegrees, double XPos, double YPos)
        {
            FXGraphics.RotateAtTransform(ADegrees, new XPoint(XPos, YPos));
        }

        /// <summary>
        /// Line Feed; increases the current y position by the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineFeed(eFont AFont)
        {
            CurrentYPos = CurrentYPos + GetFontHeight(AFont);
            return CurrentYPos;
        }

        /// <summary>
        /// Line Feed; increases the current y position by the height of the biggest last used font
        /// </summary>
        /// <returns>the new current line</returns>
        public override float LineFeed()
        {
            CurrentYPos += GetFontHeight(FXBiggestLastUsedFont);

            // reset the biggest last used font
            FXBiggestLastUsedFont = FXDefaultFont;
            return CurrentYPos;
        }

        /// <summary>
        /// Line Feed, but not full line; increases the current y position by half the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineSpaceFeed(eFont AFont)
        {
            CurrentYPos = CurrentYPos + GetFontHeight(AFont) / 2;
            return CurrentYPos;
        }

        /// <summary>
        /// Reverse Line Feed; decreases the current y position by the height of the given font
        /// </summary>
        /// <returns>the new current line
        /// </returns>
        public override float LineUnFeed(eFont AFont)
        {
            CurrentYPos = CurrentYPos - GetFontHeight(AFont);
            return CurrentYPos;
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
        /// print the page to PDF
        /// </summary>
        protected void PrintPage()
        {
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
                }

                // Calculate the number of lines per page.
                FLinesPerPage = (float)FHeight / (float)FXDefaultFont.GetHeight() * CurrentLineHeight;

                if (FNumberOfPages == 0)
                {
                    // do a dry run without printing but calculate the number of pages

                    StartSimulatePrinting();

                    int pageCounter = 0;
                    FNumberOfPages = 1;

                    do
                    {
                        pageCounter++;
                        PrintPage();
                    } while (HasMorePages());

                    FinishSimulatePrinting();
                    BeginPrint(null, null);
                    FNumberOfPages = pageCounter;
                }
            }

            CurrentPageNr++;

            CurrentYPos = FTopMargin;
            CurrentXPos = FLeftMargin;

            FPrinterLayout.PrintPageHeader();

            float CurrentYPosBefore = CurrentYPos;
            float CurrentXPosBefore = CurrentXPos;
            FPrinterLayout.PrintPageBody();

            if ((CurrentYPosBefore == CurrentYPos) && (CurrentXPosBefore == CurrentXPos) && HasMorePages())
            {
                throw new Exception("failure printing, does not fit the page");
            }

            FPrinterLayout.PrintPageFooter();
        }

        /// <summary>
        /// insert another pdf into the current PDF
        /// </summary>
        /// <param name="AFilename"></param>
        public override void InsertDocument(string AFilename)
        {
            if (PrintingMode == ePrintingMode.eDoPrint)
            {
                PdfDocument inputDocument = PdfReader.Open(AFilename, PdfDocumentOpenMode.Import);

                foreach (PdfPage pageToInsert in inputDocument.Pages)
                {
                    FPdfDocument.AddPage(pageToInsert);
                }
            }
        }

        /// <summary>
        /// store a pdf to file. will check for paper size using the PrinterLayout. By default A4 is chosen
        /// </summary>
        /// <param name="AFilename"></param>
        public void SavePDF(string AFilename)
        {
            PaperKind MyPaperKind;
            Margins MyMargins;
            float WidthInPoint;
            float HeightInPoint;

            if (FPrinterLayout.GetPageSize(out MyPaperKind, out MyMargins, out WidthInPoint, out HeightInPoint))
            {
                SavePDF(AFilename, MyPaperKind, MyMargins, WidthInPoint, HeightInPoint);
            }
            else
            {
                SavePDF(AFilename, PaperKind.A4, new Margins(20, 20, 20, 39), -1, -1);
            }
        }

        /// <summary>
        /// store a pdf to a file. will call PrintPage automatically
        /// </summary>
        public void SavePDF(string AFilename, PaperKind APaperKind, Margins AMargins, float AWidthInPoint, float AHeightInPoint)
        {
            if (RegionInfo.CurrentRegion == null)
            {
                // https://bugzilla.novell.com/show_bug.cgi?id=588708
                // RegionInfo.CurrentRegion is null
                throw new Exception("Mono bug: CurrentRegion is still null, invariant culture. Please set LANG environment variable");
            }

            FPdfDocument = new PdfDocument();
            bool firstPage = true;

            do
            {
                PdfPage page = FPdfDocument.AddPage();

                if (APaperKind != PaperKind.Custom)
                {
                    // see if we can match PaperKind to PageSize by name
                    foreach (PageSize MyPageSize in Enum.GetValues(typeof(PageSize)))
                    {
                        if (MyPageSize.ToString() == APaperKind.ToString())
                        {
                            page.Size = MyPageSize;
                            break;
                        }
                    }
                }

                if ((AWidthInPoint != -1.0) && (AHeightInPoint != -1.0))
                {
                    // to get the points, eg. inch * 72
                    page.Width = AWidthInPoint;
                    page.Height = AHeightInPoint;
                }

                FXGraphics = XGraphics.FromPdfPage(page, XGraphicsUnit.Inch);

                // it seems for Linux we better reset the FEv, otherwise the positions are only correct on the first page, but wrong on the following pages
                // was: if (FEv == null)
                if (true)
                {
                    PrinterSettings myPrinterSettings = new PrinterSettings();
                    PageSettings myPageSettings = new PageSettings(myPrinterSettings);
                    myPageSettings.Color = true;
                    myPageSettings.Landscape = false;
                    myPageSettings.Margins = AMargins;
                    myPageSettings.PaperSize =
                        new PaperSize(page.Size.ToString(), Convert.ToInt32(page.Width.Point / 72.0f * 100.0f),
                            Convert.ToInt32(page.Height.Point / 72.0f * 100.0f));

                    InitFontsAndPens();
                }

                if (firstPage)
                {
                    PrintPage();
                    firstPage = false;
                }
                else
                {
                    PrintPage();
                }
            } while (HasMorePages());

            // should we catch an exception if document cannot be written?
            FPdfDocument.Save(AFilename);
            FPdfDocument.Close();

            // Console.WriteLine("PDF has been written to " + Path.GetFullPath(AFilename));
        }
    }

    class MonoFontResolver: PdfSharp.Fonts.IFontResolver
    {
        static string _FontDirectory = ""; // STATIC_OK: will be set for each request
        
        static List<string> _FontDirectories = new List<string>(); // STATIC_OK: will be set for each request
        static List<string> _FontFiles = new List<string>(); // STATIC_OK: will be set for each request

        /// reset the static variables for each Web Request call.
        public static void ResetStaticVariables()
        {
            _FontDirectory = "";
            _FontDirectories = new List<string>();
            _FontFiles = new List<string>();
        }

        static string FontDirectory
        {
            set
            {
                _FontDirectory = value;
                _FontDirectories.Add(value);
                RecursiveFontLookup(value);
            }
        }
        static List<string> FontDirectories
        {
          get { return _FontDirectories; }
        }

        static void RecursiveFontLookup(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                if (file.EndsWith(".ttf"))
                {
                    _FontFiles.Add(file);
                }
            }

            string[] directories = Directory.GetDirectories(dir);

            foreach (string directory in directories)
            {
                _FontDirectories.Add(directory);
                RecursiveFontLookup(directory);
            }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // see https://github.com/empira/PDFsharp-samples/blob/master/samples/core/FontResolver/SegoeWpFontResolver.cs
            return new FontResolverInfo(familyName, isBold, isItalic);
        }

        public byte[] GetFont(string faceName)
        {
            if (_FontDirectory == "")
            {
                // yum install liberation*fonts*
                FontDirectory = "/usr/share/fonts/";
            }

            // Console.WriteLine("looking for " + faceName);

            // see https://ask.libreoffice.org/en/question/87444/replacing-fonts-by-similar-ones/
            if (faceName.ToLower() == "arial")
            {
                faceName = "LiberationSans";
            }
            else if (faceName.ToLower() == "code 128")
            {
                faceName = "code128";
                // wget https://github.com/Holger-Will/code-128-font/raw/master/fonts/code128.ttf
                // see https://github.com/Holger-Will/code-128-font
            }
            else if (faceName.ToLower() == "times new roman")
            {
                faceName = "LiberationSerif";
            }

            foreach (string fontFile in _FontFiles)
            {
                try
                {
                    // Console.WriteLine(fontFile);
                    BinaryReader br = new BinaryReader(File.OpenRead(fontFile));
                    byte[] buffer = br.ReadBytes((int)br.BaseStream.Length);
                    br.Close();

                    if (Path.GetFileNameWithoutExtension(fontFile).Contains(faceName))
                    {
                        return buffer;
                    }
                }
                catch // (Exception exc)
                {
                    //System.Console.WriteLine(exc.ToString());
                }
            }

            return null;
        }
    }
}
