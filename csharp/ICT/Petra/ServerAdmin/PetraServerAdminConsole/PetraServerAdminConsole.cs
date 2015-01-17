//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Description:
//          PetraServerConsole.exe (Command line application) is the Middle Tier of Petra.NET.
//
//          Its purpose is to accept Petra Client connections and perform actions that
//          the Clients request. Business Objects in the Server read and write data to
//          the DB, the Client itself has no connection to the DB at all.
//
// @Comment This application can run both on Windows and Linux.
//          It uses only a rather small unit (PetraServerConsole.Main.pas) to start
//          the PetraServer as a Command line application. The 'big' rest of
//          the Server logic and the Petra Business Objects are located in
//          numeruous libraries (.dll's) that can be used by a Command line
//          application or any other form of .NET application (eg. WinForms) to
//          provide Petra Server functionality!
// @Authors:
//       christiank
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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PetraServerAdminConsole
{
    class PetraServerAdminConsole
    {
        void run()
        {
            TAdminConsole.Start();
        }

        public static void Main(string[] args)
        {
            PetraServerAdminConsole myApp = new PetraServerAdminConsole();

            myApp.run();
        }
    }
}