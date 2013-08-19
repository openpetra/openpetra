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
using System.Windows.Forms;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.CommonControls.Logic
{
    /// <summary>Delegate for a call to open a Modal Partner Find screen</summary>
    public delegate bool TDelegateOpenPartnerFindScreen(String ARestrictToPartnerClass,
        out Int64 APartnerKey,
        out String AShortName,
        out TLocationPK ALocationPK,
        Form AParentForm);

    /// <summary>Delegate for a call to open a Modal Conference Find screen</summary>
    public delegate bool TDelegateOpenConferenceFindScreen(String AConferenceNamePattern,
        String AOutreachCodePattern,
        out Int64 AConferenceKey,
        out String AConferenceName,
        Form AParentForm);

    /// <summary>Delegate for a call to open a Modal Event Find screen</summary>
    public delegate bool TDelegateOpenEventFindScreen(String AEventNamePattern,
        out Int64 AEventKey,
        out String AEventName,
        out String AOutreachCode,
        Form AParentForm);

    /// <summary>Delegate for a call to open a Modal Extract Find screen</summary>
    public delegate bool TDelegateOpenExtractFindScreen(out int AExtractId,
        out String AExtractName,
        out String AExtractDesc,
        Form AParentForm);

    /// <summary>Delegate for a call to open a Modal Range Find screen</summary>
    public delegate bool TDelegateOpenRangeFindScreen(String ARegionName,
        out String[] ARangeName,
        out String[] RangeFrom,
        out String[] RangeTo,
        Form AParentForm);
}