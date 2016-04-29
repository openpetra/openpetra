//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Server.App.Core.Security;
using System.Data;

namespace Ict.Petra.Server.MCommon.WebConnectors
{
    /// <summary>
    /// this connector returns the next sequence value from the database
    /// </summary>
    public class TSequenceWebConnector
    {
        /// <summary>
        /// get the next sequence value
        /// </summary>
        /// <param name="ASequence"></param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static Int64 GetNextSequence(TSequenceNames ASequence)
        {
            return GetNextSequence(ASequence, null);
        }

        /// <summary>
        /// get the next sequence value
        /// </summary>
        /// <param name="ASequence"></param>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null. If null
        /// gets passed then the Method executes DB commands with the 'globally available'
        /// <see cref="DBAccess.GDBAccessObj" /> instance, otherwise with the instance that gets passed in with this
        /// Argument!</param>
        /// <returns></returns>
        [NoRemoting]
        public static Int64 GetNextSequence(TSequenceNames ASequence, TDataBase ADataBase)
        {
            Int64 NewSequenceValue = 0;

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GetDBAccessObj(ADataBase).GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                ref Transaction, ref SubmissionOK,
                delegate
                {
                    NewSequenceValue = DBAccess.GetDBAccessObj(ADataBase).GetNextSequenceValue(ASequence.ToString(),
                        Transaction);

                    SubmissionOK = true;
                });

            return NewSequenceValue;
        }
    }
}