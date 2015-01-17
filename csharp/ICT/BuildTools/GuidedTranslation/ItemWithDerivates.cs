//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       jomammele
//
// Copyright 2004-2012 by OM International
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
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;

namespace GuidedTranslation
{
    /// <summary>
    /// contains the item string with and without additional characters and all its derivates
    /// </summary>
    public class ItemWithDerivates
    {
        /// <summary>
        /// item without Additional Characters
        /// </summary>
        public String StringWithoutAdditionalCharacters {
            get; set;
        }

        List <OriginalItem>AllDerivates = new List <OriginalItem>();

        /// <summary>
        /// Adds a new Derivate to an already existing item
        /// </summary>
        public void AddNewDerivate(OriginalItem MyOriginalItem)
        {
            String OriginalString = MyOriginalItem.OriginalString;

            // if(AllDerivates.Count != 0)
            bool Found = false;

            foreach (OriginalItem AllOriginalItems in AllDerivates)
            {
                if (AllOriginalItems.OriginalString.Equals(MyOriginalItem))
                {
                    Found = true;
                }
            }

            if (!Found)
            {
                AllDerivates.Add(MyOriginalItem);
            }
        }

        /// <summary>
        /// Returns the whole item as a string
        /// </summary>
        public string ReturnAsString()
        {
            //return item only if there is more than 1 derivate - as we want to find double items
            string MyString = "";

            if (AllDerivates.Count > 1)
            {
                MyString += "-----------------------\r\n" + StringWithoutAdditionalCharacters + "\r\n--> " + AllDerivates.Count + " derivates\r\n";

                foreach (OriginalItem MyOriginalItem in AllDerivates)
                {
                    MyString += "\"" + MyOriginalItem.OriginalString + "\"\r\n" + MyOriginalItem.SourceLocation + "\r\n-----\r\n";
                }
            }
            else
            {
                MyString += "-1";
            }

            return MyString;
        }

        /// <summary>
        /// returns the number of Derivates
        /// </summary>
        public int NumberOfDerivates()
        {
            return AllDerivates.Count;
        }
    }
/// <summary>
///contains one original String, string without additional characters and its source location
/// </summary>
    public class OriginalItem
    {
        /// <summary>
        /// the original item(derivate)
        /// </summary>
        public string OriginalString  {
            get; set;
        }

        /// <summary>
        /// the location of the derivate
        /// </summary>
        public string SourceLocation {
            get; set;
        }
    }
}