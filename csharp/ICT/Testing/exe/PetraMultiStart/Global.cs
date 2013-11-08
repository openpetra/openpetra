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
    /// Variables for sharing data between the different Classes of PetraMultiStart.
    /// </summary>
    public class Global
    {
        /// <summary>
        /// File name of the Client Executable.
        /// </summary>
        public static String Filename;
        
        /// <summary>
        /// Client ID that the test should start with.
        /// </summary>
        public static Int32 StartClientID;
        
        /// <summary>
        /// File name of the Config file for the Client Executable.
        /// </summary>
        public static String Configfile;
    }
}
