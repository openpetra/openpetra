//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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

namespace Ict.Petra.Shared.MFinance
{
    /// <summary>
    /// enumeration for the filter of the batch screens.
    /// </summary>
    public enum TFinanceBatchFilterEnum
    {
        /// <summary>
        /// no batches displayed
        /// </summary>
        fbfNone = 0,

        /// <summary>
        /// show only batches that are ready for posting
        /// </summary>
        fbfReadyForPosting = 1,

        /// <summary>
        /// show batches that are unposted and not cancelled. includes ready for posting
        /// </summary>
        fbfEditing = 3,

        /// <summary>
        /// show all batches in current and forward posting periods. includes editing and batches ready for posting
        /// </summary>
        fbfAllCurrent = 7,

        /// <summary>
        /// show all batches, even previous periods (and years?). includes editing and batches ready for posting
        /// </summary>
        fbfAll = 15,
    };
}