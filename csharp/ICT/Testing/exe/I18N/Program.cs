//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.IO;
using System.Globalization;
using GNU.Gettext;
using Ict.Common;

namespace Ict.Testing.I18N
{
    /// <summary>
    /// see http://www.gnu.org/software/gettext/manual/gettext.html#C_0023
    /// c:\programme\poedit\bin\xgettext.exe --strict --no-location --from-code=UTF-8 u:\csharp\ICT\Testing\Common\I18N\Program.cs -o u:\csharp\ICT\Testing\Testing\Common\I18N\de.po
    /// use -j for second time
    /// --no-location, because if code changes, the lines will change, and -j will add a new position line
    /// in Notepad++: change format to UTF-8 without BOM
    /// in mono shell, or with mono bin path in PATH: PATH=%PATH%;c:\Programme\Mono-2.4.3\bin;c:\Programme\Poedit\bin
    ///     msgfmt csharp\Ict\Testing\Common\I18N\de.po -d csharp\ICT\Testing\_bin\Debug --locale=de-DE --resource=OpenPetra --csharp
    /// to merge a custom language file (eg. organisation specific), use msgcat:
    ///     msgcat csharp\Ict\Testing\Common\I18N\de-custom.po csharp\Ict\Testing\Common\I18N\de.po --use-first -o csharp\Ict\Testing\Common\I18N\de-test.po
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                if (!File.Exists("de-DE/OpenPetra.resources.dll"))
                {
                    if (!Directory.Exists("de-DE"))
                    {
                        Directory.CreateDirectory("de-DE");
                    }

                    File.Copy("../../csharp/ICT/Testing/exe/I18N/Sample-de-DE/OpenPetra.resources.dll", "de-DE/OpenPetra.resources.dll");
                }

                Catalog.Init("de-DE", "de-DE");
                Console.WriteLine(Thread.CurrentThread.CurrentCulture.ToString());
                Console.WriteLine(Catalog.GetString("Hello World!"));
                Console.WriteLine(Catalog.GetString("Test for two lines\n" + "second line"));
                Catalog.Init("en-GB", "en-GB");
                Console.WriteLine(Catalog.GetString("Hello World!"));
                Console.WriteLine(Catalog.GetString("Test for two lines\n" + "second line"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                if (e.InnerException != null)
                {
                    Console.WriteLine(e.InnerException.Message);
                }

                Console.WriteLine(e.StackTrace);
            }

            Console.Write(Catalog.GetString("Press any key to continue . . . "));
            // Console.ReadKey(true);
        }
    }
}