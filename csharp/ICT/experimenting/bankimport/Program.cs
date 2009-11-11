/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
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
using System.Windows.Forms;
using Mono.Unix;
using Ict.Petra.Client.MFinance.Gui.BankImport;

namespace bankimport
{
/// <summary>
/// Class with program entry point.
/// </summary>
internal sealed class Program
{
    /// <summary>
    /// Program entry point.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
        // seems not to work to load culture from config file etc
        // need to set environment variable before starting PetraClient?
        // ie if you want to force english: set LANGUAGE=en; PetraClient.exe -C:[..]PetraClient.exe.config
        // see also http://www.mail-archive.com/mono-devel-list@lists.ximian.com/msg16275.html
        // CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("de");
        // Thread.CurrentThread.CurrentCulture = culture;
        // Thread.CurrentThread.CurrentUICulture = culture;
        // Environment.SetEnvironmentVariable ("LANGUAGE", "de");
        Catalog.Init("i18n", "./locale");
    	
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new TFrmMainForm(System.IntPtr.Zero));
    }
}
}