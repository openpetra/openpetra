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
using Mono.Unix;

namespace I18N
{
/// <summary>
/// see http://mono-project.com/I18N_with_Mono.Unix
/// c:\programme\poedit\bin\xgettext.exe --strict --no-location --from-code=UTF-8 u:\csharp\ICT\Common\Testing\I18N\Program.cs -o u:\csharp\ICT\Common\Testing\I18N\de.po
/// use -j for second time
/// --no-location, because if code changes, the lines will change, and -j will add a new position line
/// in Notepad++: change format to UTF-8 without BOM
/// in mono shell: msgfmt u:\csharp\Ict\Common\Testing\I18N\de.po -o u:\csharp\Ict\common\_bin\Debug\locale\de\LC_MESSAGES\i18n.mo
/// set LANGUAGE=de
/// i18n.exe (will need the Mono.Posix.dll, intl.dll, MonoPosixHelper.dll, Mono.Security.dll
/// </summary>
class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // does not change anything: Environment.SetEnvironmentVariable("LANGUAGE", "en");
            Catalog.Init("i18n", "locale");
            Console.WriteLine(Catalog.GetString("Hello World!"));
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
        Console.ReadKey(true);
    }
}
}