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

namespace Ict.Common.Printing
{
    /// <summary>
    /// collection of data that is entered on the web form
    /// </summary>
    public class TBarCode128
    {
        /// <summary>
        /// convert a string to a bar code that can be printed with code128.ttf font.
        /// This code is partly from http://grandzebu.net/informatique/codbar/code128_C%23.txt
        /// but also contains modifications from http://grandzebu.net/index.php?page=/informatique/codbar-en/code128.htm
        /// </summary>
        /// <param name="AText"></param>
        /// <returns></returns>
        public static string Encode(string AText)
        {
            string code128 = string.Empty;

            if (AText.Length == 0)
            {
                return string.Empty;
            }

            foreach (char c in AText)
            {
                if (((c < 32) || (c > 126)) && (c != 203))
                {
                    // invalid character
                    return "";
                }
            }

            bool tableB = true;
            int ind = 0;
            int length = AText.Length;
            int checksum = 0;
            int mini;
            int dummy;

            while (ind < length)
            {
                if (tableB == true)
                {
                    mini = (ind == 0 || ind + 3 == length - 1 ? 4 : 6);

                    mini--;

                    if ((ind + mini) <= length - 1)
                    {
                        while (mini >= 0)
                        {
                            if ((AText[ind + mini] < 48) || (AText[ind + mini] > 57))
                            {
                                break;
                            }

                            mini = mini - 1;
                        }
                    }

                    if (mini < 0)
                    {
                        if (ind == 0)
                        {
                            code128 = Char.ToString((char)210);
                        }
                        else
                        {
                            code128 = code128 + Char.ToString((char)204);
                        }

                        tableB = false;
                    }
                    else
                    {
                        if (ind == 0)
                        {
                            code128 = Char.ToString((char)209);
                        }
                    }
                }

                if (tableB == false)
                {
                    mini = 2;
                    mini = mini - 1;

                    if (ind + mini < length)
                    {
                        while (mini >= 0)
                        {
                            if (((AText[ind + mini]) < 48) || ((AText[ind]) > 57))
                            {
                                break;
                            }

                            mini = mini - 1;
                        }
                    }

                    if (mini < 0)
                    {
                        dummy = Int32.Parse(AText.Substring(ind, 2));

                        dummy = (dummy < 95 ? dummy + 32 : dummy + 105);

                        code128 = code128 + (char)(dummy);

                        ind += 2;
                    }
                    else
                    {
                        code128 = code128 + Char.ToString((char)205);
                        tableB = true;
                    }
                }

                if (tableB == true)
                {
                    code128 = code128 + AText[ind];
                    ind = ind + 1;
                }
            }

            for (ind = 0; ind <= code128.Length - 1; ind++)
            {
                dummy = code128[ind];

                dummy = dummy < 127 ? dummy - 32 : dummy - 105;

                if (ind == 0)
                {
                    checksum = dummy;
                }

                checksum = (checksum + (ind) * dummy) % 103;
            }

            checksum = checksum < 95 ? checksum + 32 : checksum + 105;

            code128 = code128 + Char.ToString((char)checksum) +
                      Char.ToString((char)211);

            return code128;
        }
    }
}