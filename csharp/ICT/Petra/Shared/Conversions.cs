//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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

namespace Ict.Petra.Shared
{
    /// functions for various Petra-related conversions.
    public class Conversions
    {
        /// <summary>
        /// encode the time value of DateTime in an Int32
        /// </summary>
        /// <param name="ADateTime"></param>
        /// <returns></returns>
        public static Int32 DateTimeToInt32Time(DateTime ADateTime)
        {
            return (ADateTime.Hour * 3600) + (ADateTime.Minute * 60) + ADateTime.Second;
        }

        /// <summary>
        /// create a DateTime object for today with the given time encoded in the Int32
        /// </summary>
        /// <param name="AInt32Time"></param>
        /// <returns></returns>
        public static DateTime Int32TimeToDateTime(Int32 AInt32Time)
        {
            TimeSpan TimeSpanFromSecs = TimeSpan.FromSeconds(AInt32Time);

            return new DateTime(DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                TimeSpanFromSecs.Hours,
                TimeSpanFromSecs.Minutes,
                TimeSpanFromSecs.Seconds);
        }
    }
}