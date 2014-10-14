//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
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
using System.Runtime.InteropServices;

namespace Ict.Common
{
    /// <summary>
    /// Contains Windows API (WinAPI) calls that are necessary for getting window positions right
    /// </summary>
    public static class TWindowHandling
    {
        /// <summary>todoComment</summary>
        public const Int32 SW_HIDE = 0;

        /// <summary>todoComment</summary>
        public const Int32 SW_SHOWNORMAL = 1;

        /// <summary>todoComment</summary>
        public const Int32 SW_SHOW = 5;

        /// <summary>todoComment</summary>
        public const Int32 SW_RESTORE = 9;

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary>
        /// this is a wrapper around the win32 function ShowWindow
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        public static Boolean ShowWindowWrapper(IntPtr hwnd, int nCmdShow)
        {
            try
            {
                return ShowWindow(hwnd, nCmdShow);
            }
            catch (Exception)
            {
                // avoid crash on Linux
            }
            return false;
        }

        ///  The SetForegroundWindow function puts the thread that created the
        /// specified window into the foreground and activates the window
        [DllImport("user32.dll")]
        private static extern Boolean SetForegroundWindow(IntPtr hwnd);

        /// <summary>
        /// this is a wrapper around the win32 function SetForegroundWindow
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static Boolean SetForegroundWindowWrapper(IntPtr hwnd)
        {
            try
            {
                return SetForegroundWindow(hwnd);
            }
            catch (Exception)
            {
                // avoid crash on Linux
            }
            return false;
        }
    }
}