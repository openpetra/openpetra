https://sourceforge.net/projects/pdfsharp/
based on version: 1.31.1789.0
my own version: 1.31.2600.0 (14.02.2012)
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


also including patch for faster rendering, from thread:
http://forum.pdfsharp.net/viewtopic.php?f=2&t=679&start=0
see also http://www.pakeha_by.my-webs.org/MigraDocFastTableRender.html


Steps for creating the dll:

* download http://gnuwin32.sourceforge.net/packages/diffutils.htm and http://gnuwin32.sourceforge.net/packages/patch.htm and install
* create diff:
cd C:\Users\tpokorra\Downloads
"c:\Program Files (x86)\GnuWin32\bin\diff.exe" -Naur PDFSharp-MigraDocFoundation-1_31.orig PDFSharp-MigraDocFoundation-1_31.patched > my.patch

* apply diff:
cd C:\Users\tpokorra\Downloads\PDFSharp-MigraDocFoundation-1_31.ToPatch
"c:\Program Files (x86)\GnuWin32\bin\patch.exe" -p1 < ..\my.patch

apply the TableRendering.patch:
cd C:\Users\tpokorra\Downloads\PDFSharp-MigraDocFoundation-1_31.ToPatch\MigraDoc\code
"c:\Program Files (x86)\GnuWin32\bin\patch.exe" -p0 < ..\..\..\FastTableRender-patch\TableRendering.patch

in SharpDevelop 4.1:
load C:\Users\tpokorra\Downloads\PDFSharp-MigraDocFoundation-1_31.patched\PDFsharp\dev\PDFsharp-VS2008.sln
change PDFsharp/code/PdfSharp/PdfSharp/ProductVersionInfo.cs VersionBuild to a number fitting today
eg. http://www.wolframalpha.com/input/?i=days+since+%222005-01-01%22
and compile PDFSharp project in Release configuration.
find the dll in C:\Users\tpokorra\Downloads\PDFSharp-MigraDocFoundation-1_31.patched\PDFsharp\code\PdfSharp\bin\Release