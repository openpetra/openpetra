#region PDFsharp - A .NET library for processing PDF
//
// Authors:
//   Stefan Lange (mailto:Stefan.Lange@pdfsharp.com)
//
// Copyright (c) 2005-2009 empira Software GmbH, Cologne (Germany)
//
// http://www.pdfsharp.com
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

using System;

namespace PdfSharp
{
  /// <summary>
  /// Version info base for all PDFsharp related assemblies.
  /// </summary>
  public static class ProductVersionInfo
  {
    /// <summary>
    /// The title of the product.
    /// </summary>
    public const string Title = "PDFsharp";

    /// <summary>
    /// A characteristic description of the product.
    /// </summary>
    public const string Description = "A .NET library for processing PDF.";

    /// <summary>
    /// The PDF producer information string.
    /// </summary>
    public const string Producer = Title + " " + VersionMajor + "." + VersionMinor + "." + VersionBuild + Technologie + " (" + Url + ")";

    /// <summary>
    /// The full version number.
    /// </summary>
    public const string Version = VersionMajor + "." + VersionMinor + "." + VersionBuild + "." + VersionPatch;

    /// <summary>
    /// The full version string.
    /// </summary>
    public const string Version2 = VersionMajor + "." + VersionMinor + "." + VersionBuild + "." + VersionPatch + Technologie;

    /// <summary>
    /// The home page of this product.
    /// </summary>
    public const string Url = "www.pdfsharp.com";

    /// <summary>
    /// 
    /// </summary>
    public const string Configuration = "";

    /// <summary>
    /// The company that created/owned the product.
    /// </summary>
    public const string Company = "empira Software GmbH, Cologne (Germany)";

    /// <summary>
    /// The name the product.
    /// </summary>
    public const string Product = "PDFsharp";

    /// <summary>
    /// The copyright information.
    /// </summary>
    public const string Copyright = "Copyright © 2005-2009 empira Software GmbH.";

    /// <summary>
    /// The trademark the product.
    /// </summary>
    public const string Trademark = "PDFsharp";

    /// <summary>
    /// Unused.
    /// </summary>
    public const string Culture = "";

    /// <summary>
    /// The major version number of the product.
    /// </summary>
    public const string VersionMajor = "1";

    /// <summary>
    /// The minor version number of the product.
    /// </summary>
    public const string VersionMinor = "31";

    /// <summary>
    /// The build number of the product.
    /// </summary>
    public const string VersionBuild = "2215";  // Build = days since 2005-01-01  -  change this values ONLY HERE

    /// <summary>
    /// The patch number of the product.
    /// </summary>
    public const string VersionPatch = "0";

#if DEBUG
    /// <summary>
    /// The calculated build number.
    /// </summary>
    public static int BuildNumber = (DateTime.Now - new DateTime(2005, 1, 1)).Days;
#endif

    /// <summary>
    /// The technology tag of the product:
    /// -g: GDI+,
    /// -w: WPF,
    /// -h: Both GDI+ and WPF (hybrid).
    /// </summary>
#if GDI && !WPF
    public const string Technologie = "-g";
#endif
#if WPF && !GDI && !SILVERLIGHT
    public const string Technologie = "-w";
#endif
#if WPF && GDI
    public const string Technologie = "-h";
#endif
#if SILVERLIGHT
    public const string Technologie = "-ag";
#endif
  }
}
