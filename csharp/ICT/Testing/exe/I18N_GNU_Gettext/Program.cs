//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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
using GNU.Gettext; //Required namespace for i18n

namespace Ict.Testing.I18N_GNU.Gettext
{
    class Example
    {
        //This variable will allow us to set which strings are translatable
        //"my_class" is a catalog identifier, used in the next section
        private static GettextResourceManager _catalog = new GettextResourceManager("my_class");

        public static void Main(string[] args)
        {
            //This string is modified adding the method GetString() from the catalog variable
            System.Console.WriteLine(_catalog.GetString("Hello world!"));
        }
    }
}