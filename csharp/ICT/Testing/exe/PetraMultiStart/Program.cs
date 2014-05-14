//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timotheusp, ChristianK (C# translation, adaption to OpenPetra)
//
// Copyright 2004-2013 by OM International
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

namespace PetraMultiStart
{
/// <summary>
/// Program for the simulation of many connected Clients.
/// <para>
/// Each client is run in a separate thread.
/// </para>
/// <para>
/// The program runs endlessly, and needs to be stopped with CTRL-C!!!
/// </para>
/// </summary>
/// <remarks>
/// See 'README.txt' file for basic documentation!
/// </remarks>
class Program
{
    public static void Main(string[] args)
    {
        main.RunTest();
    }
}
}