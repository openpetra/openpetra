/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop, christiank
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
using System;
using System.Runtime.InteropServices;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// Contains Windows API (WinAPI) calls that are necessary for getting window positions right
    /// </summary>
    public class WindowHandling
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
        public static extern Boolean ShowWindow(IntPtr hwnd, int nCmdShow);

        /// This function returns the handle to the foreground window - the window
        /// with which the user is currently working.
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        ///  The SetForegroundWindow function puts the thread that created the
        /// specified window into the foreground and activates the window
        [DllImport("user32.dll")]
        public static extern Boolean SetForegroundWindow(IntPtr hwnd);

        /// The SetFocus function sets the keyboard focus to the specified window.
        /// The window must be attached to the calling thread's message queue.
        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hwnd);
    }
}