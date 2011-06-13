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
using System.Collections;
using System.Runtime.Remoting;

namespace Ict.Common.Remoting.Client
{
    /// <summary>
    /// Remoting helper class for ICT applications.
    /// Allows Interfaces instead of Objects to be used for calls to remote objects
    /// that are defined in .NET (Remoting) Configuration files.
    /// see also the book 'Advanced .NET Remoting' (Chapter 4, page 100)
    /// </summary>
    public class TRemotingHelper
    {
        private static bool IsInit;
        private static IDictionary WellKnownTypes;

        /// <summary>
        /// Call this function to use interfaces of remote objects that are defined in
        /// .NET (Remoting) Configuration files.
        ///
        /// </summary>
        /// <param name="MyType">Name of the type as it appears in the .NET (Remoting)
        /// Configuration file</param>
        /// <returns>Interface on which calls to the remoted object can be made
        /// </returns>
        public static object GetObject(System.Type MyType)
        {
            WellKnownClientTypeEntry Entr;

            if (IsInit == false)
            {
                InitTypeCache();
            }

            Entr = (WellKnownClientTypeEntry)(WellKnownTypes[MyType]);

            if (Entr == null)
            {
                throw new RemotingException("Type not found!");
            }

            return Activator.GetObject(Entr.ObjectType, Entr.ObjectUrl);
        }

        /// <summary>
        /// Initialises a type cache that is used by GetObject.
        /// </summary>
        /// <returns>void</returns>
        public static void InitTypeCache()
        {
            IsInit = true;
            WellKnownTypes = new Hashtable();

            foreach (WellKnownClientTypeEntry Entr in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
            {
                if (Entr.ObjectType == null)
                {
                    throw new RemotingException(("A configured remote type could not " +
                                                 "be found on the server. Please check spelling in your configuration file!"));
                }

                WellKnownTypes.Add(Entr.ObjectType, Entr);
            }
        }
    }
}