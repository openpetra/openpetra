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

namespace Ict.Petra.Shared.MReporting
{
    /// <summary>
    /// some useful constants that are used throughout the reporting tool
    /// </summary>
    public class ReportingConsts
    {
        /// <summary>
        /// maximum number of function parameters
        /// </summary>
        public const Int32 MAX_FUNCTION_PARAMETER = 10;

        /// <summary>
        /// to identify parameters that are only important for this run
        /// </summary>
        public const Int32 APPLICATIONPARAMETERS = -2;

        /// <summary>
        /// to identify parameters that are only important during calculation of the report
        /// </summary>
        public const Int32 CALCULATIONPARAMETERS = -4;

        /// <summary>
        /// identifier for the header title1 field
        /// </summary>
        public const Int32 HEADERTITLE1 = 100;

        /// <summary>
        /// identifier for the header title2 field
        /// </summary>
        public const Int32 HEADERTITLE2 = 101;

        /// <summary>
        /// identifier for the header type field
        /// </summary>
        public const Int32 HEADERTYPE = 102;

        /// <summary>
        /// identifier for the header period field
        /// </summary>
        public const Int32 HEADERPERIOD = 103;

        /// <summary>
        /// identifier for the header period2 field
        /// </summary>
        public const Int32 HEADERPERIOD2 = 104;

        /// <summary>
        /// identifier for the header period3 field
        /// </summary>
        public const Int32 HEADERPERIOD3 = 105;

        /// <summary>
        /// identifier for the header descr1 field
        /// </summary>
        public const Int32 HEADERDESCR1 = 106;

        /// <summary>
        /// identifier for the header descr2 field
        /// </summary>
        public const Int32 HEADERDESCR2 = 107;

        /// <summary>
        /// identifier for the header descr3 field
        /// </summary>
        public const Int32 HEADERDESCR3 = 108;

        /// <summary>
        /// identifier for the header page number field
        /// </summary>
        public const Int32 HEADERPAGENR = 109;

        /// <summary>
        /// identifier for the header date field
        /// </summary>
        public const Int32 HEADERDATE = 110;

        /// <summary>
        /// identifier for the header line
        /// </summary>
        public const Int32 HEADERLINE = 111;

        /// <summary>
        /// identifier for the header pageleft1 field
        /// </summary>
        public const Int32 HEADERPAGELEFT1 = 112;

        /// <summary>
        /// identifier for the header pageleft2 field
        /// </summary>
        public const Int32 HEADERPAGELEFT2 = 113;

        /// <summary>
        /// identifier for the detail full line
        /// </summary>
        public const Int32 DETAILFULLLINE = 114;

        /// <summary>
        /// todoComment
        /// </summary>
        public const Int32 COLUMN_TEMP_LOWERLEVEL = 200;

        /// <summary>
        /// for parameters; if this is set, the parameter applies to all columns
        /// </summary>
        public const Int32 ALLCOLUMNS = 99;

        /// <summary>
        /// only applies to the header
        /// </summary>
        public const Int32 HEADERCOLUMN = -16;

        /// <summary>
        /// the column to the left, description
        /// </summary>
        public const Int32 COLUMNLEFT = -11;

        /// <summary>
        /// maximum number of columns
        /// </summary>
        public const Int32 MAX_COLUMNS = 90;
    }
}