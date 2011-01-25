https://sourceforge.net/projects/pdfsharp/
based on version: 1.31.1789.0
my own version: 1.31.2215.0 (25.01.2011)
License: MIT (GPL compatible)

PDFsharp is a .NET library for creating and modifying Adobe PDF documents 
programmatically from any .NET language like C# or VB.NET. 
PDFsharp defines classes for the objects found in PDF files, 
so you never have to deal with IDs or references directly.

Changes in my own version to make it work with Mono, and on the server without a graphical screen:
based on the work of Michael Scott, which he has published in this thread http://forum.pdfsharp.net/viewtopic.php?f=4&t=1206#p3830
see the files in this directory:
PDFsharp\code\PdfSharp\PdfSharp\ProductVersionInfo.cs
PDFsharp\code\PdfSharp\PdfSharp.Drawing\XGraphics.cs
PDFsharp\code\PdfSharp\PdfSharp.Internal\NativeMethods.cs
PDFsharp\code\PdfSharp\PdfSharp.Fonts.OpenType\FontData.cs