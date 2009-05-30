/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Description:
 *         PetraServerAdminConsole.exe (Command line application) is a 'remote control'
 *         application for the PetraServer.
 *
 *         Its purpose is to connect to a running PetraServer and provide information
 *         about and control over the Server just as the Server application does when
 *         it is run interactively. However, the Server will usually run as a
 *         background process, so the Server .exe's menu will not be available. This
 *         is where this 'remote control' application comes in.
 *
 * @Comment This application can run both on Windows and Linux.
 *         It uses only a rather small unit (PetraServerAdminConsole.Main.pas)
 *         to run as a Command line application. The rest of the application
 *         logic is located in a library (.dll) that can be used by a Command
 *         line application or any other form of .NET application (eg. WinForms)
 *         to provide PetraServer 'remote control' functionality!
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Petra ServerAdmin (Console) Application")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("ICT")]
[assembly: AssemblyProduct("Petra ServerAdmin")]
[assembly: AssemblyCopyright("(c) OM International 2004-2009")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// This sets the default COM visibility of types in the assembly to invisible.
// If you need to expose a type to COM, use [ComVisible(true)] on that type.
[assembly: ComVisible(false)]

// The assembly version has following format :
//
// Major.Minor.Build.Revision
//
// You can specify all the values or you can use the default the Revision and
// Build Numbers by using the '*' as shown below:
[assembly: AssemblyVersion("0.0.9.0")]