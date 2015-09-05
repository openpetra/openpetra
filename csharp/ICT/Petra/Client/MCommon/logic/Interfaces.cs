//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2015 by OM International
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
using System.Collections.Generic;
using System.Windows.Forms;

using Ict.Petra.Shared.MCommon;


namespace Ict.Petra.Client.MCommon
{
    /// <summary>
    /// Interface used by classes that implement label sorting
    /// </summary>
    public interface IFormDataSort
    {
        /// <summary>
        /// Initialize the sorting algorithm
        /// </summary>
        /// <param name="AParamList">An array of parameters specified by the interface implementation</param>
        /// <returns>False if an error occurred</returns>
        Boolean Initialize(object[] AParamList);

        /// <summary>
        /// Sort the specified form data list
        /// </summary>
        /// <param name="AFormDataList">A list to be sorted</param>
        /// <returns>True if successful</returns>
        Boolean Sort(List <TFormData>AFormDataList);
    }
}