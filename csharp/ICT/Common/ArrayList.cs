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
using System.Diagnostics.CodeAnalysis;
using System.Collections;

namespace Ict.Common
{
    /// <summary>
    /// A wrapper around System.Collections.ArrayList,
    /// that allows to use the index property even beyond the existing count of objects
    /// </summary>
    /// <example>
    ///  TSelfExpandingArrayList myArray = new TSelfExpandingArrayList();
    ///  myArray[5] = "test5";
    ///  myArray[3] = "test3";
    ///  myArray[7] = "test7";
    ///  for (Int32 i = 0; i &lt; myArray.Count; i++)
    ///  {
    ///     if (myArray[i] != null)
    ///     {
    ///         System.Console.Writeline(i.ToString() + " " + myArray[i].ToString());
    ///     }
    ///  }
    ///
    ///  myArray.Compact();
    ///
    ///  for (Int32 i = 0; i &lt; myArray.Count; i++)
    ///  {
    ///     if (myArray[i] != null)
    ///     {
    ///         System.Console.Writeline(i.ToString() + " " + myArray[i].ToString());
    ///     }
    ///  }
    ///</example>
    [SuppressMessage("Gendarme.Rules.Design", "ListsAreStronglyTypedRule",
         Justification = "Gendarme identifies this Type as a Generic, which is wrong, hence we want to surpress the Gendarme Warning.")]
    [SuppressMessage("Gendarme.Rules.Design", "StronglyTypeICollectionMembersRule",
         Justification = "Gendarme identifies this Type as a Generic, which is wrong, hence we want to surpress the Gendarme Warning.")]
    public class TSelfExpandingArrayList : System.Collections.ArrayList
    {
        /// <summary>
        /// property for the elements;
        /// you can assign elements to any position: the array will be increased
        /// and filled with null values in between the assigned values
        /// </summary>
        public override Object this[int i]
        {
            get
            {
                while (i >= Count)
                {
                    this.Add(null);
                }

                return base[i];
            }

            set
            {
                while (i >= this.Count)
                {
                    this.Add(null);
                }

                base[i] = value;
            }
        }

        /// <summary>
        /// remove elements that are not needed anymore
        /// </summary>
        public void Compact()
        {
            System.Int32 counter;
            counter = 0;

            while (counter < this.Count)
            {
                if (this[counter] == null)
                {
                    this.RemoveAt(counter);
                }
                else
                {
                    counter = counter + 1;
                }
            }
        }
    }
}