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
using Ict.Petra.Shared.MReporting;
using Ict.Common;

namespace Ict.Petra.Server.MReporting
{
    /// <summary>
    /// This class is the base class for the user defined functions of the server part of the report generator.
    /// For each Petra Module, such user functions should be implemented.
    /// </summary>
    public class TRptUserFunctions
    {
        /// <summary>
        /// define the current depth and column, etc
        /// </summary>
        protected TRptSituation situation;

        /// <summary>
        /// define parameters and variables that can be used by the functions
        /// </summary>
        protected TParameterList parameters;

        /// <summary>
        /// this parameterless constructor is called when the System.Object is instantiated in TRptEvaluator
        ///
        /// </summary>
        /// <returns>void</returns>
        public TRptUserFunctions()
        {
            this.situation = null;
            parameters = null;
        }

        /// <summary>
        /// this constructor is used when the System.Object is instantiated in other situations
        ///
        /// </summary>
        /// <returns>void</returns>
        public TRptUserFunctions(TRptSituation ASituation)
        {
            this.situation = ASituation;
            parameters = situation.GetParameters();
        }

        /// <summary>
        /// this function should be overwritten; sets the environment for running the function
        /// </summary>
        public virtual Boolean FunctionSelector(TRptSituation ASituation, String f, TVariant[] ops, out TVariant AValue)
        {
            this.situation = ASituation;
            parameters = situation.GetParameters();
            AValue = null;
            return false;
        }
    }
}