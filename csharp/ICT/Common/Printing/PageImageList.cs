//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Bernardo Castilho
//
// Copyright 2009-2014 by Bernardo Castilho
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


using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;

namespace Ict.Common.Printing
{
#if false

    // This version of the PageImageList stores images as byte arrays. It is a little
    // more complex and slower than a simple list, but doesn't consume GDI resources.
    // This is important when the list contains lots of images (Windows only supports
    // 10,000 simultaneous GDI objects!)
    class PageImageList
    {
        // ** fields
        List<byte[]> _list = new List<byte[]>();

        // ** object model
        public void Clear()
        {
            _list.Clear();
        }
        public int Count
        {
            get { return _list.Count; }
        }
        public void Add(Image img)
        {
            _list.Add(GetBytes(img));

            // stored image data, now dispose of original
            img.Dispose();
        }
        public Image this[int index]
        {
            get { return GetImage(_list[index]); }
            set { _list[index] = GetBytes(value); }
        }

        // implementation
        byte[] GetBytes(Image img)
        {
            // use interop to get the metafile bits
            Metafile mf = img as Metafile;
            var enhMetafileHandle = mf.GetHenhmetafile().ToInt32();
            var bufferSize = GetEnhMetaFileBits(enhMetafileHandle, 0, null);
            var buffer = new byte[bufferSize];
            GetEnhMetaFileBits(enhMetafileHandle, bufferSize, buffer);

            // return bits
            return buffer;
        }
        Image GetImage(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            return Image.FromStream(ms);
        }

        [System.Runtime.InteropServices.DllImport("gdi32")]
        static extern int GetEnhMetaFileBits(int hemf, int cbBuffer, byte[] lpbBuffer);
    }

#else

    // This version of the PageImageList is a simple List<Image>. It is simple,
    // but caches one image (GDI object) per preview page.
    class PageImageList : List<Image>
    {
    }

#endif
}
