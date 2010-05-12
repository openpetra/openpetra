//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// Custom Event used by UC_PartnerFind_Criteria to signal to the
    /// PartnerFind_Options screen that Critiera have been selected or rearranged
    ///
    /// </summary>
    public class TPartnerFindCriteriaSelectionChangedEventArgs : EventArgs
    {
        /// <summary>todoComment</summary>
        public String FSelectedCriteria;

        /// <summary>todoComment</summary>
        public TFindCriteriaColumn FCriteriaColumn;

        /// <summary>todoComment</summary>
        public Boolean FIsFirstInColumn;

        /// <summary>todoComment</summary>
        public Boolean FIsLastInColumn;

        /// <summary>todoComment</summary>
        public String SelectedCriteria
        {
            get
            {
                return FSelectedCriteria;
            }

            set
            {
                FSelectedCriteria = value;
            }
        }

        /// <summary>todoComment</summary>
        public TFindCriteriaColumn CriteriaColumn
        {
            get
            {
                return FCriteriaColumn;
            }

            set
            {
                FCriteriaColumn = value;
            }
        }

        /// <summary>todoComment</summary>
        public Boolean IsFirstInColumn
        {
            get
            {
                return FIsFirstInColumn;
            }

            set
            {
                FIsFirstInColumn = value;
            }
        }

        /// <summary>todoComment</summary>
        public Boolean IsLastInColumn
        {
            get
            {
                return FIsLastInColumn;
            }

            set
            {
                FIsLastInColumn = value;
            }
        }
    }
}