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
using Ict.Common;
using Ict.Common.Printing;
using System.Collections;

namespace Ict.Petra.Shared.MReporting
{
    /// <summary>
    /// Functions that deal with getting the right rows from the result;
    /// Accessing common parameters;
    /// keep track of the current row to be printed, and therefore cope with page breaks
    ///
    /// This class deals with the hierarchy of the results;
    /// it is able to walk through the master/child line,
    /// and to deal with page breaks.
    ///
    /// </summary>
    public abstract class TReportPrinterCommon : TPrinterLayout
    {
        /// <summary>the settings for this report</summary>
        protected TParameterList FParameters;

        /// <summary>the data for this report</summary>
        protected TResultList FResultList;

        /// <summary>direct access to the results (data for report)</summary>
        protected ArrayList FResults;

        /// <summary>where to print; can be graphics or text</summary>
        protected TPrinter FPrinter;

        /// <summary>this is the index of the Row that has to be printed on the next page;</summary>
        protected Int32 FNextElementToPrint;

        /// <summary>used for simulating</summary>
        protected Int32 FNextElementToPrintBackup;

        /// <summary>list of TStageElementPrinting for each level; this tells the next part of the element that needs to be printed</summary>
        protected ArrayList FNextElementLineToPrint;

        /// <summary>used for simulating</summary>
        protected ArrayList FNextElementLineToPrintBackup;

        /// <summary>what is the level of the deepest level</summary>
        protected Int32 FLowestLevel;

        /// <summary>number of columns for the report</summary>
        protected Int32 FNumberColumns;

        /// <summary>the time the document was printed</summary>
        protected System.DateTime FTimePrinted;

        /// <summary>
        /// </summary>
        /// <returns>s true if any value was printed
        /// </returns>
        protected virtual bool PrintColumn(Int32 columnNr, Int32 level, TVariant column)
        {
            return false;
        }

        /// <summary>
        /// print the lowest level (no children)
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected virtual Boolean PrintLowestLevel(TResult row)
        {
            return false;
        }

        /// <summary>
        /// print the header for a normal level (not deepest level)
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected virtual Boolean PrintNormalLevelHeader(TResult row)
        {
            return false;
        }

        /// <summary>
        /// can either print or simulate
        /// </summary>
        /// <returns>s the current y position after printing or simulating
        /// </returns>
        protected virtual double PrintNormalLevelFooter(TResult row)
        {
            return 0.0;
        }

        /// <summary>
        /// prints the captions of the columns;
        /// prepare the footer lines for long captions;
        /// is called by PrintPageHeader
        ///
        /// </summary>
        /// <returns>void</returns>
        protected virtual void PrintColumnCaptions()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AResult"></param>
        /// <param name="AParameters"></param>
        /// <param name="APrinter"></param>
        public TReportPrinterCommon(TResultList AResult, TParameterList AParameters, TPrinter APrinter)
        {
            // go through all results and parameters and replace the unformatted and encoded date
            // the whole point is to format the dates differently, depending on the output (printer vs. CSV)
            FParameters = AParameters.ConvertToFormattedStrings("Localized");
            FResultList = AResult.ConvertToFormattedStrings(FParameters, "Localized");
            FResults = FResultList.GetResults();
            FLowestLevel = FParameters.Get("lowestLevel").ToInt();
            FTimePrinted = DateTime.Now;
            FPrinter = APrinter;
        }

        /// <summary>
        /// prepare printing the document
        /// </summary>
        public override void StartPrintDocument()
        {
            Int32 i;
            TResult firstElement;

            FNextElementLineToPrint = new ArrayList();
            FNextElementLineToPrintBackup = new ArrayList();

            for (i = 0; i <= FLowestLevel; i += 1)
            {
                FNextElementLineToPrint.Add(eStageElementPrinting.eHeader);
            }

            FNextElementLineToPrint[FLowestLevel] = eStageElementPrinting.eDetails;
            FNextElementToPrint = 1;

            // it might be that the first row is not printable
            firstElement = FindNextSibling(null);

            if (firstElement != null)
            {
                FNextElementToPrint = firstElement.childRow;
            }

            FParameters.Add("CurrentSubReport", 0);
        }

        #region Walk through the report hierarchy (data side)

        /// <summary>
        /// the rows have a unique child number, even if they have different master rows
        ///
        /// </summary>
        /// <returns>void</returns>
        protected TResult FindRow(Int32 childNr)
        {
            foreach (TResult row in FResults)
            {
                if (row.childRow == childNr)
                {
                    return row;
                }
            }

            throw new Exception("row " + childNr.ToString() + " could not be found.");
        }

        /// <summary>
        /// find first row that is in the hierarchy below the current row
        /// </summary>
        /// <param name="currentRow"></param>
        /// <returns></returns>
        protected TResult FindFirstChild(TResult currentRow)
        {
            TResult ReturnValue;

            ReturnValue = null;

            foreach (TResult row in FResults)
            {
                if ((row.masterRow == currentRow.childRow) && (row.depth <= FLowestLevel))
                {
                    if ((ReturnValue == null) || (row.childRow < ReturnValue.childRow))
                    {
                        ReturnValue = row;
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// find the next row on the same level
        /// </summary>
        /// <param name="currentRow"></param>
        /// <returns></returns>
        protected TResult FindNextSibling(TResult currentRow)
        {
            TResult ReturnValue;
            Int32 i;
            Int32 masterRow;
            Int32 childRow;

            // if currentRow is nil assume the root (needed to find first printable element)
            if (currentRow != null)
            {
                masterRow = currentRow.masterRow;
                childRow = currentRow.childRow;
            }
            else
            {
                masterRow = 0;
                childRow = 0;
            }

            ReturnValue = null;

            foreach (TResult row in FResults)
            {
                if (row.masterRow == masterRow)
                {
                    if ((row.childRow > childRow) && ((ReturnValue == null) || (row.childRow < ReturnValue.childRow)))
                    {
                        if ((row.depth == 1) && (FLowestLevel != 1)
                            && (FParameters.GetOrDefault("HasSubReports", -1, new TVariant(false)).ToBool() == true))
                        {
                            // reset the FLowestLevel, because this is basically a new report (several lowerlevelreports in main level)
                            // todo: be careful: some reports have several rows in the main level, I just assumed one total for the finance reports
                            // it works now for reports with just one row depth, for others this still needs to be sorted properly. another parameter?
                            FLowestLevel = FResultList.GetDeepestVisibleLevel(row.childRow);
                            FPrinter.LineSpaceFeed(eFont.eDefaultFont);
                            FPrinter.DrawLine(FPrinter.LeftMargin, FPrinter.RightMargin, eLinePosition.eAbove, eFont.eDefaultBoldFont);
                            FPrinter.LineSpaceFeed(eFont.eDefaultFont);
                            FParameters.Add("CurrentSubReport", FParameters.Get("CurrentSubReport").ToInt() + 1);

                            for (i = 0; i <= FLowestLevel; i += 1)
                            {
                                FNextElementLineToPrint.Add(eStageElementPrinting.eHeader);
                            }

                            FNextElementLineToPrint[FLowestLevel] = eStageElementPrinting.eDetails;
                        }

                        ReturnValue = row;
                        FNextElementLineToPrint[ReturnValue.depth] = eStageElementPrinting.eHeader;
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// find the next sibling of the parent row
        /// </summary>
        /// <param name="currentRow"></param>
        /// <returns></returns>
        protected TResult FindNextUncle(TResult currentRow)
        {
            TResult ReturnValue;

            ReturnValue = null;

            if (currentRow.masterRow > 0)
            {
                ReturnValue = FindNextRow(FindRow(currentRow.masterRow));
            }

            if (ReturnValue != null)
            {
                FNextElementLineToPrint[ReturnValue.depth] = eStageElementPrinting.eHeader;
            }

            return ReturnValue;
        }

        /// <summary>
        /// find the next sibling of currentRow.
        /// if there is none, then try to find the next row up one hierarchy
        /// if currentRow is the last row, return nil
        ///
        /// </summary>
        /// <returns>void</returns>
        protected TResult FindNextRow(TResult currentRow)
        {
            TResult ReturnValue;

            ReturnValue = FindNextSibling(currentRow);

            if ((ReturnValue == null) && (currentRow.masterRow > 0))
            {
                // see if the father is already fully printed
                ReturnValue = FindRow(currentRow.masterRow);

                if (ReturnValue != null)
                {
                    if ((eStageElementPrinting)FNextElementLineToPrint[ReturnValue.depth] == eStageElementPrinting.eFinished)
                    {
                        ReturnValue = null;
                    }
                    else
                    {
                        FNextElementLineToPrint[ReturnValue.depth] = eStageElementPrinting.eFooter;
                    }
                }
            }

            if (ReturnValue == null)
            {
                ReturnValue = FindNextUncle(currentRow);
            }

            return ReturnValue;
        }

        #endregion

        #region Parameter related functions

        /// <summary>
        /// Get the value of a parameter
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="column"></param>
        /// <param name="depth"></param>
        /// <param name="exact"></param>
        /// <returns></returns>
        protected string Get(String parameterId, int column, int depth, eParameterFit exact)
        {
            string ReturnValue;

            ReturnValue = FParameters.Get(parameterId, column, depth, exact).ToString();

            if (ReturnValue == "NOTFOUND")
            {
                ReturnValue = "";
            }

            return ReturnValue;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="column"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        protected string Get(String parameterId, int column, int depth)
        {
            return Get(parameterId, column, depth, eParameterFit.eBestFit);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        protected string Get(String parameterId, int column)
        {
            return Get(parameterId, column, -1, eParameterFit.eBestFit);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        protected string Get(String parameterId)
        {
            return Get(parameterId, -1, -1, eParameterFit.eBestFit);
        }

        /// <summary>
        /// This function returns the position of the column in the current measurement unit
        /// (ie. letter for textmode, or inch for graphics; see implementation of function FPrinter.Cm);
        /// using ColumnPosition and ColumnPositionIndented from the FParameters
        ///
        /// </summary>
        /// <param name="columnNr"></param>
        /// <param name="level"></param>
        /// <param name="ADefault">in cm</param>
        /// <returns>the position in cm
        /// </returns>
        protected float GetPosition(Int32 columnNr, Int32 level, float ADefault)
        {
            float ReturnValue;
            TVariant value;
            float indented;

            indented = 0.0f;

            if ((columnNr > -1) && (columnNr < ReportingConsts.MAX_COLUMNS))
            {
                // only for data columns

                if (FParameters.Get("indented", columnNr, level, eParameterFit.eAllColumnFit).ToBool() == true)
                {
                    value = FParameters.Get("ColumnPositionIndented", columnNr, level);

                    if (!value.IsZeroOrNull())
                    {
                        indented = (float)value.ToDouble();
                    }
                }
            }

            value = FParameters.Get("ColumnPosition", columnNr, level);

            if (value.IsNil())
            {
                ReturnValue = ADefault + indented;
            }
            else
            {
                ReturnValue = (float)value.ToDouble() + indented;
            }

            return FPrinter.LeftMargin + FPrinter.Cm(ReturnValue);
        }

        /// <summary>
        /// This function returns the width of the column in in the current measurement unit;
        /// (ie. letter for textmode, or inch for graphics; see implementation of function FPrinter.Cm);
        /// using ColumnWidth and ColumnPositionIndented from the FParameters
        ///
        /// </summary>
        /// <param name="columnNr"></param>
        /// <param name="level"></param>
        /// <param name="ADefault">in cm</param>
        /// <returns>the width in cm
        /// </returns>
        protected float GetWidth(Int32 columnNr, Int32 level, float ADefault)
        {
            float ReturnValue;
            TVariant value;

            value = FParameters.Get("ColumnWidth", columnNr, level);

            if (value.IsZeroOrNull())
            {
                ReturnValue = ADefault;
            }
            else
            {
                ReturnValue = (float)value.ToDouble();
            }

            if (level == -1)
            {
                // page header, width of column
                value = FParameters.Get("ColumnPositionIndented", columnNr, level);

                if (!value.IsZeroOrNull())
                {
                    ReturnValue = ReturnValue + (float)value.ToDouble();
                }
            }

            return FPrinter.Cm(ReturnValue);
        }

        /// <summary>
        /// get the alignment for the current level (left, centered, right)
        /// </summary>
        /// <param name="columnNr"></param>
        /// <param name="level"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        protected eAlignment GetAlignment(Int32 columnNr, Int32 level, eAlignment ADefault)
        {
            eAlignment ReturnValue;
            TVariant value;

            value = FParameters.Get("ColumnAlign", columnNr, level);
            ReturnValue = ADefault;

            if (value.IsZeroOrNull())
            {
                ReturnValue = ADefault;
            }
            else if (value.ToString().CompareTo("left") == 0)
            {
                ReturnValue = eAlignment.eLeft;
            }
            else if (value.ToString().CompareTo("right") == 0)
            {
                ReturnValue = eAlignment.eRight;
            }
            else if (value.ToString().CompareTo("center") == 0)
            {
                ReturnValue = eAlignment.eCenter;
            }

            return ReturnValue;
        }

        #endregion

        #region Walk through the report hierarchy (output side)

        /// <summary>
        /// print the details of a normal level (not lowest level)
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected Boolean PrintNormalLevelDetails(TResult row)
        {
            Boolean ReturnValue;
            TResult childRow;

            FNextElementLineToPrint[row.depth] = eStageElementPrinting.eDetails;
            childRow = FindFirstChild(row);

            while (childRow != null)
            {
                FNextElementLineToPrint[childRow.depth] = eStageElementPrinting.eHeader;

                if (!PrintRow(childRow))
                {
                    return false;
                }

                childRow = FindNextSibling(childRow);
            }

            ReturnValue = true;
            FNextElementLineToPrint[row.depth] = eStageElementPrinting.eFooter;
            return ReturnValue;
        }

        /// <summary>
        /// print a normal level
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected Boolean PrintNormalLevel(TResult row)
        {
            if ((eStageElementPrinting)FNextElementLineToPrint[row.depth] == eStageElementPrinting.eHeader)
            {
                if (!PrintNormalLevelHeader(row))
                {
                    return false;
                }
            }

            if ((eStageElementPrinting)FNextElementLineToPrint[row.depth] == eStageElementPrinting.eDetails)
            {
                if (!PrintNormalLevelDetails(row))
                {
                    return false;
                }
            }

            if ((eStageElementPrinting)FNextElementLineToPrint[row.depth] == eStageElementPrinting.eFooter)
            {
                // try if footer will still fit on the page; it can consist of 2 lines, and there is no page length check in there
                FPrinter.StartSimulatePrinting();
                PrintNormalLevelFooter(row);

                if (!FPrinter.ValidYPos())
                {
                    // not enoughSpace
                    FPrinter.FinishSimulatePrinting();
                    FNextElementLineToPrint[row.depth] = eStageElementPrinting.eFooter;
                    FNextElementToPrint = row.childRow;
                    return false;
                }

                FPrinter.FinishSimulatePrinting();
                PrintNormalLevelFooter(row);
            }

            return true;
        }

        /// <summary>
        /// prints the current Row
        /// </summary>
        /// <returns>s true if all were printed; false if page was full
        /// side effects: will set the NextElementToPrint
        /// </returns>
        protected Boolean PrintRow(TResult row)
        {
            Boolean ReturnValue;
            bool LowestLevel;

            LowestLevel = false;

            if (FindFirstChild(row) == null)
            {
                // if both descr and header are set, don't use lowestlevel, but normal level
                // to print both.
                // situations to test:
                // a) Account Detail, acc/cc with no transaction, but a balance
                // b) some situation with analysis attributes on account detail
                if ((row.descr == null) || (row.header == null))
                {
                    LowestLevel = true;
                }
            }

            if (row.depth == FLowestLevel)
            {
                LowestLevel = true;
            }

            if (LowestLevel)
            {
                if (!PrintLowestLevel(row))
                {
                    return false;
                }
            }
            else
            {
                if (!PrintNormalLevel(row))
                {
                    return false;
                }
            }

            ReturnValue = true;
            FNextElementToPrint = -1;
            return ReturnValue;
        }

        /// <summary>
        /// print the main part of the page
        /// </summary>
        public override void PrintPageBody()
        {
            if (FResults.Count == 0)
            {
                // nothing to be printed
                return;
            }

            try
            {
                TResult currentRow = null;

                if (FNextElementToPrint > 0)
                {
                    currentRow = FindRow(FNextElementToPrint);
                }

                while ((currentRow != null) && PrintRow(currentRow))
                {
                    currentRow = FindNextRow(currentRow);
                }

                FPrinter.SetHasMorePages(currentRow != null);
            }
            catch (Exception E)
            {
                TLogging.Log(E.Message);

                // MessageBox.Show(E.StackTrace);
                System.Console.WriteLine(E.StackTrace);
            }
        }

        #endregion
    }
}