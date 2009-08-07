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
using System.Data;
using Ict.Common;

namespace anniversaries
{
    class Program
    {
        /// <summary>
        /// convert a CSV list into an int array
        /// </summary>
        /// <param name="ACSVList"></param>
        /// <returns></returns>
        private static int[] ConvertCSVToIntArray(string ACSVList)
        {
            string[] elements = ACSVList.Split(',');
            int[] intList = new int[elements.Length];
            int Counter = 0;
            foreach (string s in elements)
            {
                intList[Counter++] = Convert.ToInt32(s);
            }
            return intList;
        }
        
        /// <summary>
        /// sample call: mono anniversaries.exe -username:*** -password:*** -startdate:01/06/2009 -enddate:30/06/2009 -specialbirthdays:50,60,70,75,80,85,90,95,100,105,110,115 -specialanniversary:20,25,30,35,40,45,50,55 -Server.RDBMSType:Progress -Server.ODBC_DSN:petra2_2
        /// /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            TAppSettingsManager settings = new TAppSettingsManager(false);
            
            try
            {
                TDataAnniversaries.InitDBConnection(
                    settings.GetValue("username"),
                    settings.GetValue("password"));

                DataSet dataBirthdays = TDataAnniversaries.GetBirthdays(
                    Convert.ToInt64(settings.GetValue("fieldkey")),
                    Convert.ToDateTime(settings.GetValue("startdate")),
                    Convert.ToDateTime(settings.GetValue("enddate")),
                    ConvertCSVToIntArray(settings.GetValue("specialbirthdays")));

                DataSet dataAnniversaries = TDataAnniversaries.GetAnniversaries(
                    Convert.ToInt64(settings.GetValue("fieldkey")),
                    Convert.ToDateTime(settings.GetValue("startdate")),
                    Convert.ToDateTime(settings.GetValue("enddate")),
                    ConvertCSVToIntArray(settings.GetValue("specialanniversary")));
                
                TSendEmail.SendEmailToPersonnel(
                    dataBirthdays, 
                    dataAnniversaries, 
                    settings.GetValue("email", ""),
                    Convert.ToDateTime(settings.GetValue("startdate")),
                    Convert.ToDateTime(settings.GetValue("enddate")),
                    settings.GetValue("specialbirthdays"),
                    settings.GetValue("specialanniversary")
                    );
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
    }
}