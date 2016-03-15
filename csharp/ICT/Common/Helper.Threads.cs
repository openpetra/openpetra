//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Threading;

namespace Ict.Common
{
    /// <summary>
    /// Helper Class for things that have to do with Multithreading.
    /// </summary>
    public static class ThreadingHelper
    {
        /// <summary>
        /// Returns an identifier that either contains the current Threads' Name and its ManagedThreadId or -
        /// in case the current Threads' Name is String.Empty - only its ManagedThreadId.
        /// </summary>
        /// <returns>The current Threads' Name and its ManagedThreadId or -
        /// in case the current Threads' Name is String.Empty - only its ManagedThreadId.</returns>
        public static string GetCurrentThreadIdentifier()
        {
            return GetThreadIdentifier(Thread.CurrentThread);
        }

        /// <summary>
        /// Returns an identifier that either contains a Threads' Name and its ManagedThreadId or -
        /// in case a Threads' Name is String.Empty - only its ManagedThreadId.
        /// </summary>
        /// <param name="ATheThread">Thread to get the identifier for.</param>
        /// <returns>The Threads' Name and its ManagedThreadId or -
        /// in case a Threads' Name is String.Empty - only its ManagedThreadId.</returns>
        public static string GetThreadIdentifier(Thread ATheThread)
        {
            string ReturnValue = ATheThread.Name ?? String.Empty;

            if (ReturnValue.Length > 0)
            {
                // Ensure Thread Name starts with apostrophy ( ' ).
                if (!ReturnValue.StartsWith("'", StringComparison.InvariantCulture))
                {
                    ReturnValue = "'" + ReturnValue;
                }

                if (!ReturnValue.EndsWith("]", StringComparison.InvariantCulture))
                {
                    // Ensure Thread Name ends with apostrophy ( ' ).
                    if (!ReturnValue.EndsWith("'", StringComparison.InvariantCulture))
                    {
                        ReturnValue += "'";
                    }

                    ReturnValue += " [ThreadID: " + ATheThread.ManagedThreadId.ToString() + "]";
                }
            }
            else
            {
                ReturnValue += ATheThread.ManagedThreadId.ToString();
            }

            return ReturnValue;
        }
    }
}