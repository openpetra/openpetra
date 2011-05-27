//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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

namespace Ict.Petra.Shared.MConference
{
    /// <summary>
    /// Defines the types of units that should be returned to the conference field reports.
    /// </summary>
    public enum TUnitTypeEnum
    {
        /// <summary>Fields that are charged for the conferencee</summary>
        utChargedFields,
        /// <summary>Fields that send people to the conference</summary>
        utSendingFields,
        /// <summary>Fields that receive people from the conference</summary>
        utReceivingFields,
        /// <summary>Fields where applicants from the conference are registered</summary>
        utRegisteringFields,
        /// <summary>All the outreach options of this conference</summary>
        utOutreachOptions,

        /// <summary>Undefined or unknown type </summary>
        utUnknown
    }
}