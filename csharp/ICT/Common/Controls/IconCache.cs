//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using Ict.Common.Exceptions;

namespace Ict.Common.Controls
{
    /// <summary>
    /// Provides a Cache for storing Icons.
    /// </summary>
    /// <remarks>A single Icon file can hold an Icon in multiple sizes (e.g. 16x16 pixels
    /// and 32x32 pixels) and this Cache can return the Icon in the desired sizes from the Cache.</remarks>
    public class TIconCache : ConcurrentDictionary <string, Icon>
    {
        /// <summary>
        /// Size of Icon.
        /// </summary>
        public enum TIconSize
        {
            /// <summary>Icon with a size of 16x16 pixels</summary>
            is16by16,

            /// <summary>Icon with a size of 24x24 pixels</summary>
            is24by24,

            /// <summary>Icon with a size of 32x32 pixels</summary>
            is32by32,

            /// <summary>Icon with a size of 48x48 pixels</summary>
            is48by48
        }

        /// <summary>
        /// Single instance of TIconCache (created once on application startup).
        /// </summary>
        /// <remarks>Use this instance to access the Cache!</remarks>
        public static TIconCache IconCache;

        /// <summary>True if the last requested Icon was returned from the Cache.</summary>
        private static bool FLastIconRequestedWasReturnedFromCache = false;

        /// <summary>
        /// Constructor. Simply calls the base constructor.
        /// </summary>
        public TIconCache() : base()
        {
            IconCache = this;
        }

        /// <summary>
        /// Inquire this to find out if the last requested Icon was returned from the Cache.
        /// </summary>
        public bool LastIconRequestedWasReturnedFromCache
        {
            get
            {
                return FLastIconRequestedWasReturnedFromCache;
            }
        }

        /// <summary>
        /// Adds an Icon into the Cache.
        /// </summary>
        /// <param name="AFileName">File name of the Icon incl. full path</param>
        /// <param name="AIconSize">define which size the icon should have</param>
        public void AddIcon(string AFileName, TIconSize AIconSize)
        {
            if (AFileName == null)
            {
                return;
            }

            AFileName = Path.GetFullPath(AFileName.Replace('\\', '/'));
            Icon icon = new Icon(AFileName, GetIconSize(AIconSize));

            this.AddOrUpdate(AFileName + AIconSize.ToString(), icon, (AKey, AExistingValue) =>
                {
                    return AExistingValue;
                });
        }

        /// <summary>
        /// Adds an Icon into the Cache if it isn't yet in the Cache. Returns the
        /// Icon of the specified size, no matter if it was found in the Cache or
        /// whether it needed to be loaded from file first.
        /// </summary>
        /// <param name="AFileName">File name of the Icon incl. full path</param>
        /// <param name="AIconSize">Desired size of the Icon to be returned (should
        /// this size not be availabe then the closest match will be returned).</param>
        /// <returns>Icon of the specified size or the closest matching size.</returns>
        public Bitmap AddOrGetExistingIcon(string AFileName, TIconSize AIconSize)
        {
            Bitmap ReturnValue = null;

            AFileName = Path.GetFullPath(AFileName.Replace('\\', '/'));

            if (!ContainsIcon(AFileName, AIconSize))
            {
                AddIcon(AFileName, AIconSize);
                ReturnValue = GetIcon(AFileName, AIconSize);

                FLastIconRequestedWasReturnedFromCache = false;
            }
            else
            {
                ReturnValue = GetIcon(AFileName, AIconSize);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the Icon of the specified size from the Cache.
        /// </summary>
        /// <param name="AFileName">File name of the Icon incl. full path</param>
        /// <param name="AIconSize">Desired size of the Icon to be returned (should
        /// this size not be availabe then the closest match will be returned).</param>
        /// <returns>Icon of the specified size or the closest matching size.</returns>
        public Bitmap GetIcon(string AFileName, TIconSize AIconSize)
        {
            Icon TheItem;

            if (AFileName == null)
            {
                return null;
            }
            else
            {
                AFileName = Path.GetFullPath(AFileName.Replace('\\', '/'));

                try
                {
                    TheItem = (Icon) this[AFileName + AIconSize.ToString()];
                }
                catch (KeyNotFoundException)
                {
                    FLastIconRequestedWasReturnedFromCache = false;

                    throw new EIconNotInCacheException(String.Format(
                            "Icon with path {0} not yet loaded into cache; add it to the cache with AddIcon Method first", AFileName));
                }

                FLastIconRequestedWasReturnedFromCache = true;

                return TheItem.ToBitmap();
            }
        }

        /// <summary>
        /// Determines whether an Icon exists in the Cache.
        /// </summary>
        /// <param name="AFileName">File name of the Icon incl. full path</param>
        /// <param name="AIconSize">size of the icon</param>
        /// <returns>True if the icon exists in the Cache, otherwise false.</returns>
        public bool ContainsIcon(string AFileName, TIconSize AIconSize)
        {
            if (AFileName == null)
            {
                return false;
            }
            else
            {
                AFileName = Path.GetFullPath(AFileName.Replace('\\', '/'));

                return ContainsKey(AFileName + AIconSize.ToString());
            }
        }

        /// <summary>
        /// Returns the Size of an Icon in pixels that is specified with a <see cref="TIconSize" /> enum value.
        /// </summary>
        /// <param name="AIconSize">Icon size enum value</param>
        /// <returns>Size of an Icon in pixels for enum value specified with <paramref name="AIconSize"/></returns>
        private Size GetIconSize(TIconSize AIconSize)
        {
            switch (AIconSize)
            {
                case TIconSize.is16by16:
                    return new Size(16, 16);

                case TIconSize.is24by24:
                    return new Size(24, 24);

                case TIconSize.is32by32:
                    return new Size(32, 32);

                case TIconSize.is48by48:
                    return new Size(48, 48);

                default:
                    // Fallback
                    return new Size(16, 16);
            }
        }
    }

    /// <summary>
    /// Thrown if an attempt is made to access an Icon in the Cache, but the Icon
    /// does not exist in the Cache.
    /// </summary>
    public class EIconNotInCacheException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EIconNotInCacheException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param> 
        public EIconNotInCacheException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EIconNotInCacheException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }
}