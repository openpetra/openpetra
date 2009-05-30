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
using Ict.Common;

namespace GenerateGTK
{
/// this tool will generate GTK User Interfaces from yaml files
class Program
{
    private static String sampleCall = "GenerateGTK -ymlfile:myscreen.yml -petraxml:petra.xml -outputdir:gtk";

    public static void Main(string[] args)
    {
        try
        {
            TAppSettingsManager settings = new TAppSettingsManager(false);

            if (!settings.HasValue("ymlfile")
                || !settings.HasValue("petraxml")
                || !settings.HasValue("outputdir"))
            {
                Console.WriteLine("call: " + sampleCall);
                return;
            }

            TGenerator.GenerateGui(settings.GetValue("ymlfile"),
                settings.GetValue("petraxml"),
                settings.GetValue("outputdir"));
        }
        catch (Exception e)
        {
            System.Console.WriteLine("error: " + e.Message);
            System.Console.WriteLine(e.StackTrace);
            Environment.Exit(-1);
        }
    }
}
}