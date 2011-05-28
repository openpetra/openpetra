//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.IO;

namespace Ict.Common.Printing
{
    /// <summary>
    /// helper functions for form letters, which can be used for printing to paper or preparing emails etc
    /// </summary>
    public class TFormLettersTools
    {
        /// <summary>
        /// for form letter files, we need to check if there is a template specific for the country or form.
        /// Otherwise the next best fitting template is used
        /// </summary>
        /// <param name="APath"></param>
        /// <param name="AFileID"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="AFormsID">several classifications be separated by a dot, eg. adult.serve. If there is no template for adult.serve, use the template for adult</param>
        /// <param name="AExtension"></param>
        /// <returns></returns>
        public static string GetRoleSpecificFile(string APath,
            string AFileID,
            string ACountryCode,
            string AFormsID,
            string AExtension)
        {
            if (!AFileID.EndsWith("."))
            {
                AFileID += ".";
            }

            if (!AExtension.StartsWith("."))
            {
                AExtension = "." + AExtension;
            }

            string FileName = Path.Combine(APath, AFileID);

            if (File.Exists(FileName + ACountryCode + "." + AFormsID + AExtension))
            {
                return FileName + ACountryCode + "." + AFormsID + AExtension;
            }

            while (AFormsID != null && AFormsID.Contains("."))
            {
                AFormsID = AFormsID.Substring(0, AFormsID.LastIndexOf('.'));

                if (File.Exists(FileName + ACountryCode + "." + AFormsID + AExtension))
                {
                    return FileName + ACountryCode + "." + AFormsID + AExtension;
                }
            }

            return FileName + ACountryCode + AExtension;
        }
    }
}