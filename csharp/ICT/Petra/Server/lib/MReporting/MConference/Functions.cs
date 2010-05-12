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
using Ict.Petra.Server.MReporting;
using Ict.Common;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Server.MReporting.MConference
{
    /// <summary>
    /// This contains specific functions for the Conference module,
    /// that are needed for report generation.
    /// </summary>
    public class TRptUserFunctionsConference : TRptUserFunctions
    {
        /// <summary>
        /// all functions for reports in the conference module need to be registered here
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="f"></param>
        /// <param name="ops"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override Boolean FunctionSelector(TRptSituation ASituation, String f, TVariant[] ops, out TVariant value)
        {
            if (base.FunctionSelector(ASituation, f, ops, out value))
            {
                return true;
            }

            /*
             * if (isSame(f, 'doSomething')) then
             * begin
             * value := new TVariant();
             * doSomething(ops[1].ToInt(), ops[2].ToString(), ops[3].ToString());
             * exit;
             * end;
             */
            value = new TVariant();
            return false;
        }
    }
}