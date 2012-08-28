//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Runtime.Caching;

namespace Ict.Common.Controls
{
    /// <summary>
    /// Provides a Cache for storing Icons.
    /// </summary>
    /// <remarks>A single Icon file can hold an Icon in multiple sizes (e.g. 16x16 pixels
    /// and 32x32 pixels) and this Cache can return the Icon in the desired sizes from the Cache.</remarks>
    public class TIconCache : MemoryCache
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

        /// <summary>
        /// Constructor. Simply calls the base constructor.
        /// </summary>
        /// <param name="AName"></param>
        /// <param name="AConfig"></param>
        public TIconCache(string AName, NameValueCollection AConfig) : base(AName, AConfig)
        {
            IconCache = this;
        }

        /// <summary>
        /// Adds an Icon into the Cache.
        /// </summary>
        /// <param name="AFileName">File name of the Icon incl. full path</param>
        public void AddIcon(string AFileName)
        {
            MemoryStream ms;

            if (AFileName == null)
            {
                return;
            }

            ms = new MemoryStream();

            using (FileStream
                   IconFile = new FileStream(AFileName, FileMode.Open))
            {
                IconFile.CopyTo(ms);
                this.Set(AFileName, ms, new CacheItemPolicy());
            }
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
            if (!ContainsIcon(AFileName))
            {
                AddIcon(AFileName);
            }

            return GetIcon(AFileName, AIconSize);
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
            MemoryStream TheItem;
            Size IconSize = GetIconSize(AIconSize);

            if (AFileName == null)
            {
                return null;
            }
            else
            {
                TheItem = (MemoryStream) this[AFileName];

                if (TheItem != null)
                {
                    TheItem.Position = 0;  // ALL IMPORTANT - without that, the creation of the Icon from the Stream fails!
                    return new System.Drawing.Icon(TheItem, IconSize).ToBitmap();
                }
                else
                {
                    throw new EIconNotInCacheException(String.Format(
                            "Icon with path {0} not yet loaded into cache; add it to the cache with AddIcon Method first", AFileName));
                }
            }
        }

        /// <summary>
        /// Determines whether an Icon exists in the Cache.
        /// </summary>
        /// <param name="AFileName">File name of the Icon incl. full path</param>
        /// <returns>True if the icon exists in the Cache, otherwise false.</returns>
        public bool ContainsIcon(string AFileName)
        {
            if (AFileName == null)
            {
                return false;
            }
            else
            {
                return Contains(AFileName, null);
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
    public class EIconNotInCacheException : Exception
    {
        /// <summary>
        /// Constructor with inner Exception
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="message"></param>
        public EIconNotInCacheException(Exception innerException, string message)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor without inner Exception
        /// </summary>
        /// <param name="message"></param>
        public EIconNotInCacheException(string message)
            : base(message)
        {
        }
    }
}