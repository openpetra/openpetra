//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2015 by OM International
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
using System.Windows.Forms;

namespace Ict.Common.IO
{
    /// <summary>
    /// This stupid static class contains one static method whose sole purpose is to get round a bug in Windows 7
    /// that did not exist in XP and is fixed in Windows 8.
    /// There is an issue with Win7 displaying filenames longer than about 11 characters - see
    /// https://social.msdn.microsoft.com/Forums/vstudio/en-US/fdef2a08-d90d-4c87-adb1-9963f5bf41e3/openfiledialog-file-name-not-correctly-justified?forum=wpf
    /// Basically the VISUAL display of the filename is clipped on the left so you only see the last 11 characters
    /// But the filename is correct and can be seen in full by clicking in the text area.
    ///
    /// The idea for this solution came from
    /// http://stackoverflow.com/questions/17163784/default-name-with-openfiledialog-c
    /// </summary>
    public static class TWin7FileOpenSaveDialog
    {
        /// <summary>
        /// Call this immediately before the line that shows the OpenFile or SaveFile dialog.  It is only necessary if you have
        /// set the dialog.FileName property. When this is set there is a real possibility that the full file name will be
        /// clipped on the left so you only visually see the right-most 10 or 11 characters. (The full name is still in the property though).
        /// </summary>
        /// <param name="AFilenameWithoutPath">Specify the filename.  The method only does anything for names longer than 10 characters</param>
        public static void PrepareDialog(String AFilenameWithoutPath)
        {
            Version ver = Environment.OSVersion.Version;

            // Check for a 'long' name in Windows Vista (6.0) and Win7 (6.1).  The bug is fixed in Win8 (6.2)
            if ((AFilenameWithoutPath.Length > 10)
                && (Environment.OSVersion.Platform == PlatformID.Win32NT)
                && (ver.Major == 6)
                && (ver.Minor < 2))
            {
                // Set up a timer, interval and event handler
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 500;
                timer.Elapsed += delegate
                {
                    timer.Stop();

                    // SHIFT + (HOME then END) will select the complete name
                    SendKeys.SendWait("+({HOME}{END})");
                };

                // Start the timer that will in 500 ms fire off our SendKeys once the dialog is shown
                timer.Start();
            }
        }
    }
}