//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using Ict.Common;
using System.Globalization;
using System.Security.Cryptography;
using System.Data;
using System.Threading;

namespace Ict.Common
{
    /// <summary>
    /// General String utility functions for ICT applications.
    /// </summary>
    public class StringHelper
    {
        private static string[] DefaultCSVSeparators = new string[] {
            ",", ";", "/", "|", "-", ":"
        };

        /// <summary>
        /// convert an array of strings into a StringCollection
        /// </summary>
        /// <param name="list">array of strings</param>
        /// <returns>a new StringCollection containing the strings</returns>
        public static StringCollection InitStrArr(string[] list)
        {
            StringCollection ReturnValue;

            ReturnValue = new StringCollection();

            foreach (string s in list)
            {
                ReturnValue.Add(s);
            }

            return ReturnValue;
        }

        /// <summary>
        /// split a string using a delimiter and return a StringCollection containing the pieces of the string
        /// </summary>
        /// <param name="s">the string to split</param>
        /// <param name="delim">the delimiter to use</param>
        /// <returns>a StringCollection with the pieces of the string</returns>
        public static StringCollection StrSplit(string s, string delim)
        {
            StringCollection ReturnValue;
            Boolean done;
            int p;
            int pos;
            string stemp;

            ReturnValue = new StringCollection();

            if (s.Length == 0)
            {
                return ReturnValue;
            }

            done = false;
            pos = 0;

            while (!done)
            {
                p = s.IndexOf(delim, pos);
                stemp = "";

                // escaped delimiter
                if ((p > 0) && (s.ToCharArray()[p - 1] == '\\'))
                {
                    pos = p + 1;
                }
                else
                {
                    if (p == -1)
                    {
                        done = true;
                        stemp = s;
                    }
                    else
                    {
                        stemp = s.Substring(0, p);
                        s = s.Substring(p + delim.Length);
                        pos = 0;
                    }

                    if (stemp.Length > 0)
                    {
                        ReturnValue.Add(stemp.Trim().Replace("\\\\", "\\").Replace("\\" + delim, delim));
                    }
                    else if (stemp.Length == 0)
                    {
                        ReturnValue.Add("");
                    }
                }
            }

            return ReturnValue;
        }

        private static void StrMergeHelper(ref StringBuilder builder, string element, char delim)
        {
            if (element.Contains("\\"))
            {
                element = element.Replace("\\", "\\\\");
            }

            // if the element already contains the delimiter, do something about it.
            // strsplit and getNextCSV have to revert it
            if (element.IndexOf(delim) != -1)
            {
                if (delim == '\t')
                {
                    builder.Append(element.Replace("\t", "\\t"));
                }
                else
                {
                    // replace a double quote with two double quotes inside the element
                    builder.Append("\"").Append(element.Replace("\"", "\"\"")).Append("\"");
                }
            }
            else
            {
                builder.Append(element);
            }
        }

        /// <summary>
        /// concatenate a string using the given delimiter
        /// </summary>
        /// <param name="l">the StringCollection containing the strings that should be concatenated</param>
        /// <param name="delim">the delimiter to be used between the strings</param>
        /// <returns>a string with the concatenated strings from the StringCollection</returns>
        public static string StrMerge(StringCollection l, char delim)
        {
            StringBuilder ReturnValue = new StringBuilder();

            for (int i = 0; i <= l.Count - 1; i += 1)
            {
                if (i != 0)
                {
                    ReturnValue.Append(delim);
                }

                StrMergeHelper(ref ReturnValue, l[i], delim);
            }

            return ReturnValue.ToString();
        }

        /// <summary>
        /// concatenate a string using the given delimiter
        /// </summary>
        /// <param name="l">the string array containing the strings that should be concatenated</param>
        /// <param name="delim">the delimiter to be used between the strings</param>
        /// <returns>a string with the concatenated strings from the string array</returns>
        public static string StrMerge(String[] l, char delim)
        {
            StringBuilder ReturnValue = new StringBuilder();

            for (int i = 0; i < l.Length; i++)
            {
                if (i != 0)
                {
                    ReturnValue.Append(delim);
                }

                StrMergeHelper(ref ReturnValue, l[i], delim);
            }

            return ReturnValue.ToString();
        }

        /// <summary>
        /// return a sorted version of the given StringCollection (not case sensitive)
        /// </summary>
        /// <param name="l">the StringCollection to be used to generate a sorted list</param>
        /// <returns>the sorted StringCollection</returns>
        public static StringCollection StrSort(StringCollection l)
        {
            StringCollection ReturnValue;
            ArrayList a;

            a = new ArrayList();

            foreach (string s in l)
            {
                a.Add(s);
            }

            a.Sort(new CaseInsensitiveComparer());
            ReturnValue = new StringCollection();

            foreach (string s in a)
            {
                ReturnValue.Add(s);
            }

            return ReturnValue;
        }

        /// <summary>
        /// check if a StringCollection haystack contains all of the needles
        /// </summary>
        /// <param name="haystack">the StringCollection to be searched</param>
        /// <param name="needles">the StringCollection containing the strings that are to be found</param>
        /// <returns>true if all strings can be found, false otherwise</returns>
        public static Boolean Contains(StringCollection haystack, StringCollection needles)
        {
            Boolean ReturnValue = true;

            foreach (string s in needles)
            {
                if ((!haystack.Contains(s)))
                {
                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// check if a StringCollection haystack contains at least one of the needles
        /// </summary>
        /// <param name="haystack">the StringCollection to be searched</param>
        /// <param name="needles">the StringCollection containing the strings that are to be found</param>
        /// <returns>true if at least one string can be found, false if none can be found</returns>
        public static Boolean ContainsSome(StringCollection haystack, StringCollection needles)
        {
            Boolean ReturnValue = false;

            foreach (string s in needles)
            {
                if (haystack.Contains(s))
                {
                    return true;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Tests if two strings are equal, case insensitive
        /// </summary>
        /// <param name="s1">the first string to be compared</param>
        /// <param name="s2">the other string to be compared</param>
        /// <returns>true if the strings are equal, not considering case sensitivity</returns>
        public static Boolean IsSame(string s1, string s2)
        {
            return s1.ToLower() == s2.ToLower();
        }

        /// <summary>
        /// removes line breaks and tabulators and trims spaces, even inside the string
        /// </summary>
        /// <param name="s">the string that should be cleaned up</param>
        /// <returns>the clean string, without line breaks, tabulators and groups of spaces</returns>
        public static string CleanString(string s)
        {
            string ReturnValue = "";

            // removes \n and \t and trims spaces, even inside the string
            int p;
            string s2;
            char ch;
            char prev_ch;

            s2 = s.Trim();
            ReturnValue = "";
            p = 0;
            prev_ch = (Char)0;

            while (p < s2.Length)
            {
                ch = s2[p];

                if (ch == (Char)9)                       //(ch == chr(9))
                {
                    ch = ' ';
                }

                if (ch == (Char)10)                  //ch == chr(10))
                {
                    ch = ' ';
                }

                if ((!((prev_ch == ' ') && (ch == ' '))))
                {
                    ReturnValue = ReturnValue + ch;
                }

                prev_ch = ch;
                p++;
            }

            return ReturnValue;
        }

        /// <summary>
        /// return the string without the quotes at the start and the end of the string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimQuotes(string s)
        {
            string result = s;

            if (result.Length > 0)
            {
                if (result[0] == '"')
                {
                    result = result.Substring(1);
                }
            }

            if (result.Length > 0)
            {
                if (result[result.Length - 1] == '"')
                {
                    result = result.Substring(0, result.Length - 1);
                }
            }

            return result;
        }

        /// <summary>
        /// calcuate the md5 hash sum of a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string MD5Sum(string s)
        {
            MD5CryptoServiceProvider cr = new MD5CryptoServiceProvider();

            return BitConverter.ToString(cr.ComputeHash(System.Text.Encoding.Default.GetBytes(s))).Replace("-", "").ToLower();
        }

        /// <summary>
        /// need to find the matching quotes
        /// </summary>
        /// <param name="s">String to the find the matching quote</param>
        /// <param name="position">position of the first quote</param>
        /// <returns>index of the end matching quote in the string</returns>
        public static int FindMatchingQuote(String s, int position)
        {
            int counter = position + 1;

            while (counter < s.Length)
            {
                if (s[counter] == '"')
                {
                    if ((counter + 1 == s.Length) || (s[counter + 1] != '"'))
                    {
                        return counter - 1;
                    }
                    else
                    {
                        // two double quotes mean one escaped double quote.
                        counter++;
                    }
                }

                counter++;
            }

            // cannot find
            throw new System.IndexOutOfRangeException();
        }

        /// <summary>
        /// need to find the matching quotes
        /// </summary>
        /// <param name="s">String to the find the matching quote</param>
        /// <param name="position">position of the first quote</param>
        /// <returns>index of the end matching quote in the string</returns>
        public static int FindMatchingQuote(StringBuilder s, int position)
        {
            int counter = position + 1;

            while (counter < s.Length)
            {
                if (s[counter] == '"')
                {
                    if ((counter + 1 == s.Length) || (s[counter + 1] != '"'))
                    {
                        return counter - 1;
                    }
                    else
                    {
                        // two double quotes mean one escaped double quote.
                        counter++;
                    }
                }

                counter++;
            }

            // cannot find
            throw new System.IndexOutOfRangeException();
        }

        /// <summary>
        /// get the separator from the first line of CSV Data.
        /// first test for tab, then for semicolon. otherwise default to comma
        /// </summary>
        /// <param name="ACSVData"></param>
        /// <returns></returns>
        public static string GetCSVSeparator(string ACSVData)
        {
            string InputSeparator = ",";

            string FirstLine = ACSVData;

            if (ACSVData.IndexOf("\n") > 0)
            {
                FirstLine = ACSVData.Substring(0, ACSVData.IndexOf("\n"));
            }

            if (FirstLine.Contains("\t"))
            {
                InputSeparator = "\t";
            }
            else if (FirstLine.Contains(";"))
            {
                InputSeparator = ";";
            }

            return InputSeparator;
        }

        /// <summary>
        /// retrieves the first value of the comma separated list, and removes the value from the list
        /// </summary>
        /// <param name="list">the comma separated list that will get the first value removed</param>
        /// <param name="separator">the delimiter/separator of the list</param>
        /// <param name="ATryAllSeparators">if this is true, a number of default separators (slash, comma, etc) will be used</param>
        /// <param name="ARemoveLeadingAndTrailingSpaces">if this is true, leading and trailing spaces will be discarded (useful for file imports)</param>
        /// <returns>the first value of the list</returns>
        public static string GetNextCSV(ref string list,
            string separator,
            Boolean ATryAllSeparators = false,
            Boolean ARemoveLeadingAndTrailingSpaces = false)
        {
            if (list.Length == 0)
            {
                return "";
            }

            if (ATryAllSeparators == true)
            {
                // find which is the separator used in the file
                int commaPosition = list.IndexOf(separator);
                int alternativeSeparatorCounter = 0;

                while ((commaPosition == -1) && (alternativeSeparatorCounter < DefaultCSVSeparators.Length))
                {
                    separator = DefaultCSVSeparators[alternativeSeparatorCounter];
                    alternativeSeparatorCounter = alternativeSeparatorCounter + 1;
                    commaPosition = list.IndexOf(separator);
                }
            }

            int position = 0;
            bool escape = false;
            bool isFinalisedQuotedText = false;
            StringBuilder value = new StringBuilder();

            if (!list.StartsWith(separator))
            {
                while (position < list.Length)
                {
                    if (escape)
                    {
                        escape = false;
                    }
                    else
                    {
                        if (list[position] == '\\')
                        {
                            escape = true;
                            position++;
                        }
                    }

                    if ((list[position] == ' ') && (value.Length == 0) && ARemoveLeadingAndTrailingSpaces)
                    {
                        // leading spaces are ignored
                        position++;
                    }
                    else if (list[position] == '"')
                    {
                        // TODO: no substring???
                        string quotedstring = list.Substring(position + 1, FindMatchingQuote(list, position) - position);

                        if (value.Length == 0)
                        {
                            value.Append(quotedstring);
                        }
                        else
                        {
                            value.Append("\"").Append(quotedstring).Append("\"");
                        }

                        position += quotedstring.Length + 2;

                        // If we are not to add trailing spaces we set isFinalisedQuotedText = true
                        if (ARemoveLeadingAndTrailingSpaces)
                        {
                            isFinalisedQuotedText = true;
                        }
                    }
                    else
                    {
                        if (!isFinalisedQuotedText)
                        {
                            // Do not append anything (eg trailing spaces) to already finalised quoted text
                            value.Append(list[position]);
                        }

                        position++;
                    }

                    if (!escape && (position + separator.Length - 1 < list.Length) && (list.Substring(position, separator.Length) == separator))
                    {
                        // found the next separator
                        break;
                    }
                }
            }

            value = value.Replace("\"\"", "\"");

            if (position == list.Length)
            {
                list = "";
            }
            else
            {
                list = list.Substring(position + separator.Length, list.Length - position - separator.Length);

                // there still was a separator, so if the list is now empty, we need to provide an empty value
                if (list.Length == 0)
                {
                    list = "\"\"";
                }
            }

            if (isFinalisedQuotedText || !ARemoveLeadingAndTrailingSpaces)
            {
                return value.ToString();
            }
            else
            {
                return value.ToString().TrimEnd(new char[] { ' ' });
            }
        }

        /// <summary>
        /// retrieves the first value of the comma separated list, and removes the value from the list.
        /// This version of GetNextCSV is quite optimized, but less flexible than the other version.
        /// only supports single character separators, and works only with the specified separator
        /// </summary>
        /// <param name="list">the comma separated list that will get the first value removed</param>
        /// <param name="separator">the delimiter/separator of the list</param>
        /// <param name="ARemoveLeadingAndTrailingSpaces">if this is true, leading and trailing spaces will be discarded (useful for file imports)</param>
        /// <returns>the first value of the list</returns>
        public static string GetNextCSV(ref StringBuilder list, char separator, Boolean ARemoveLeadingAndTrailingSpaces = false)
        {
            if (list.Length == 0)
            {
                return "";
            }

            int position = 0;
            bool escape = false;
            bool isFinalisedQuotedText = false;
            StringBuilder value = new StringBuilder();

            if (list[0] != separator)
            {
                while (position < list.Length)
                {
                    if (escape)
                    {
                        escape = false;
                    }
                    else
                    {
                        if (list[position] == '\\')
                        {
                            escape = true;
                            position++;
                        }
                    }

                    if ((list[position] == ' ') && (separator != ' ') && (value.Length == 0) && ARemoveLeadingAndTrailingSpaces)
                    {
                        // leading spaces are ignored
                        position++;
                    }
                    else if (list[position] == '"')
                    {
                        // TODO: no substring???
                        char[] quotedstring = new char[FindMatchingQuote(list, position) - position];
                        list.CopyTo(position + 1, quotedstring, 0, quotedstring.Length);

                        if (value.Length == 0)
                        {
                            value.Append(quotedstring);
                        }
                        else
                        {
                            value.Append("\"").Append(quotedstring).Append("\"");
                        }

                        position += quotedstring.Length + 2;

                        // If we are not to add trailing spaces we set isFinalisedQuotedText = true
                        if (ARemoveLeadingAndTrailingSpaces)
                        {
                            isFinalisedQuotedText = true;
                        }
                    }
                    else
                    {
                        if (!isFinalisedQuotedText)
                        {
                            // Do not append anything (eg trailing spaces) to already finalised quoted text
                            value.Append(list[position]);
                        }

                        position++;
                    }

                    if (!escape && (position < list.Length) && (list[position] == separator))
                    {
                        // found the next separator
                        break;
                    }
                }
            }

            value.Replace("\"\"", "\"");

            if (position == list.Length)
            {
                list.Remove(0, list.Length);
            }
            else
            {
                list.Remove(0, position + 1);

                // there still was a separator, so if the list is now empty, we need to provide an empty value
                if (list.Length == 0)
                {
                    list.Append("\"\"");
                }
            }

            if (isFinalisedQuotedText || !ARemoveLeadingAndTrailingSpaces)
            {
                return value.ToString();
            }
            else
            {
                return value.ToString().TrimEnd(new char[] { ' ' });
            }
        }

        /// <summary>
        /// overload for GetNextCSV.
        /// if the value is empty, the default value will be used
        /// </summary>
        /// <param name="list">separated values; the first value will be removed</param>
        /// <param name="separator">delimiter to be used</param>
        /// <param name="ADefaultValue">to be used if the csv value is empty</param>
        /// <returns>the first value of the string</returns>
        public static string GetNextCSV(ref string list, string separator, string ADefaultValue)
        {
            string result = GetNextCSV(ref list, separator, false, false);

            if (result.Length == 0)
            {
                result = ADefaultValue;
            }

            return result;
        }

        /// <summary>
        /// try to use different separators; first fitting separator is used
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separators"></param>
        /// <returns></returns>
        public static string GetNextCSV(ref string list, string[] separators)
        {
            string result = list;
            string origList = list;

            foreach (string separator in separators)
            {
                result = GetNextCSV(ref list, separator, false, false);

                if (result != origList)
                {
                    return result;
                }

                list = origList;
            }

            return origList;
        }

        /// <summary>
        /// overload for GetNextCSV
        /// this will use the comma as default separator
        /// </summary>
        /// <param name="list">separated values; the first value will be removed</param>
        /// <returns>the first value of the string</returns>
        public static string GetNextCSV(ref string list)
        {
            return GetNextCSV(ref list, ",", false, false);
        }

        /// <summary>
        /// retrieves a specific value from a comma separated list, the list stays unchanged
        /// </summary>
        /// <param name="list">the comma separated list</param>
        /// <param name="index">index of the value that should be returned, starting counts with 0</param>
        /// <returns>the value at the given position in the list</returns>
        public static String GetCSVValue(string list, int index)
        {
            int counter;
            String element;
            String listcsv;

            counter = 0;
            listcsv = list;
            element = GetNextCSV(ref listcsv);

            while ((counter < index) && (listcsv.Length != 0))
            {
                element = GetNextCSV(ref listcsv);
                counter = counter + 1;
            }

            return element;
        }

        /// <summary>
        /// parse a line of CSV values, and return a StringCollection with the values
        /// </summary>
        public static StringCollection GetCSVList(string list, string delimiter, bool trimmedValues = false)
        {
            string listcsv = list;
            StringCollection Result = new StringCollection();

            string value = GetNextCSV(ref listcsv, delimiter);

            if (trimmedValues)
            {
                Result.Add(value.Trim());
            }
            else
            {
                Result.Add(value);
            }

            while ((listcsv.Length != 0))
            {
                value = GetNextCSV(ref listcsv, delimiter);

                if (trimmedValues)
                {
                    Result.Add(value.Trim());
                }
                else
                {
                    Result.Add(value);
                }
            }

            return Result;
        }

        /// <summary>
        /// checks if the list contains the given value
        /// </summary>
        /// <param name="list">separated values</param>
        /// <param name="AElement">string to look for in the list</param>
        /// <returns>true if the value is an element of the list</returns>
        public static Boolean ContainsCSV(string list, String AElement)
        {
            string listcsv = list;
            string element = GetNextCSV(ref listcsv);

            while ((listcsv.Length != 0) && (element != AElement))
            {
                element = GetNextCSV(ref listcsv);
            }

            return element == AElement;
        }

        /// <summary>
        /// adds a new value to a comma separated list, adding a comma if necessary
        /// </summary>
        /// <param name="line">the existing line, could be empty or hold already values</param>
        /// <param name="value">the new value</param>
        /// <returns>the new list, consisting of the old list plus the new value</returns>
        public static string AddCSV(string line, string value)
        {
            return AddCSV(line, value, ",");
        }

        /// <summary>
        /// adds a new value to a comma separated list, adding a delimiter if necessary
        /// </summary>
        /// <param name="line">existing list</param>
        /// <param name="value">value to be added</param>
        /// <param name="separator">delimiter to use</param>
        /// <returns>the new list containing the old list and the new value</returns>
        public static string AddCSV(string line, string value, string separator)
        {
            string ReturnValue = "";
            Boolean containsSeparator;

            if (value == null)
            {
                value = "";
            }

            if (line.Length > 0)
            {
                ReturnValue = line + separator;
            }
            else if (value.Length == 0)
            {
                // if the first value is empty, it should not be discarded; use a space instead. so that a delimiter will be added for the next value
                value = " ";
            }

            value = value.Replace("\"", "\"\"");
            containsSeparator = (value.IndexOf(separator) != -1);

            // force quotes for integers that have leading 0; this is needed for account codes etc
            // unfortunately, Excel still does not treat the column as text
            bool forceQuotes = value.StartsWith("0") && value.Length > 1 && StringHelper.IsStringPositiveInteger(value);

            // only use quotes if the value contains the separator or it contains double quotes
            // TODO: allow option to always use quotes, or define to use single or double quotes
            if ((containsSeparator == false) && (value.IndexOf('"') == -1) && !forceQuotes)
            {
                ReturnValue = ReturnValue + value;
            }
            else
            {
                ReturnValue = ReturnValue + '"' + value + '"';
            }

            return ReturnValue;
        }

        /// <summary>
        /// concatenates two comma separated lists, adding a comma if necessary
        /// </summary>
        /// <param name="line1">first line, could be empty or hold already values</param>
        /// <param name="line2">second line</param>
        /// <param name="separator">defaults to comma</param>
        /// <returns>the new list, consisting of the values of the 2 lists</returns>
        public static string ConcatCSV(string line1, string line2, string separator)
        {
            string ReturnValue;

            ReturnValue = line1;

            if ((line1.Length > 0) && (line2.Length > 0))
            {
                ReturnValue = ReturnValue + separator;
            }

            return ReturnValue + line2;

            //	return ReturnValue;
        }

        /// <summary>
        /// concatenate two string using the comma as delimiter
        /// </summary>
        /// <param name="line1">the first string</param>
        /// <param name="line2">the second string</param>
        /// <returns>a list of the 2 strings</returns>
        public static string ConcatCSV(string line1, string line2)
        {
            return ConcatCSV(line1, line2, ",");
        }

        /// <summary>
        /// simple parser function
        /// a token is defined to be a group of characters separated by given separator characters
        /// eg. a word etc
        /// </summary>
        /// <param name="AStringToParse">the string to be parsed; the first token will be removed</param>
        /// <returns>the first token; skips the separators (eg. spaces)</returns>
        public static string GetNextToken(ref string AStringToParse)
        {
            int currentPos = 0;
            string token = "";
            const string separatorTokens = "(),{}=";

            if (separatorTokens.IndexOf(AStringToParse[0]) != -1)
            {
                token = AStringToParse.Substring(0, 1);
                AStringToParse = AStringToParse.Substring(1);
                return token;
            }

            while (currentPos < AStringToParse.Length
                   && (AStringToParse[currentPos] == ' ' || AStringToParse[currentPos] == '\t'))
            {
                currentPos++;
            }

            while (currentPos < AStringToParse.Length
                   && AStringToParse[currentPos] != ' '
                   && separatorTokens.IndexOf(AStringToParse[currentPos]) == -1)
            {
                if (AStringToParse[currentPos] == '"')
                {
                    // expect a closing quote
                    if (AStringToParse.IndexOf('"', currentPos + 1) == -1)
                    {
                        throw new Exception("Cannot find closing quote, in: " + AStringToParse);
                    }

                    token = token + AStringToParse[currentPos];
                    currentPos++;

                    while (currentPos < AStringToParse.Length && AStringToParse[currentPos] != '"')
                    {
                        token = token + AStringToParse[currentPos];
                        currentPos++;
                    }

                    token += AStringToParse[currentPos];
                    currentPos++;
                }
                else
                {
                    token += AStringToParse[currentPos];
                    currentPos++;
                }
            }

            if ((currentPos < AStringToParse.Length)
                && (separatorTokens.IndexOf(AStringToParse[currentPos]) == -1))
            {
                currentPos++;
            }

            AStringToParse = AStringToParse.Substring(currentPos);
            return token;
        }

        /// <summary>
        /// returns the directory portion of pathname, using the last / or \ character
        /// </summary>
        /// <param name="path">the complete path</param>
        /// <returns>the directory portion of the path</returns>
        public static string DirName(string path)
        {
            int posSlash;

            posSlash = 0;

            while (path.IndexOf('/', posSlash + 1) >= 0)
            {
                posSlash = path.IndexOf('/', posSlash + 1);
            }

            while (path.IndexOf("\\", posSlash + 1) >= 0)
            {
                posSlash = path.IndexOf("\\", posSlash + 1);
            }

            return path.Substring(0, posSlash);
        }

        /// <summary>
        /// returns the filename portion of pathname, using the last / or \ character
        /// </summary>
        /// <param name="path">the complete path</param>
        /// <returns>the filename portion of the path</returns>
        public static string BaseName(string path)
        {
            int posSlash;

            posSlash = 0;

            while (path.IndexOf('/', posSlash + 1) >= 0)
            {
                posSlash = path.IndexOf('/', posSlash + 1);
            }

            while (path.IndexOf("\\", posSlash + 1) >= 0)
            {
                posSlash = path.IndexOf("\\", posSlash + 1);
            }

            return path.Substring(posSlash + 1, path.Length - posSlash);
        }

        /// <summary>
        /// Capitalizes the first letter of a string.
        /// InitialCaps returns a string in which the first character is capitalized and
        /// all the remaining characters are lower-case.
        ///
        /// </summary>
        /// <param name="str">String to be converted.</param>
        /// <returns>String formatted with an initial capital.</returns>
        public static string InitialCaps(string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length - 1).ToLower();
        }

        /// <summary>
        /// return a string where all underscores are removed,
        /// and instead the character following the underscore
        /// has been converted to Uppercase, also the first character of the string
        /// </summary>
        /// <param name="AStr">the string to be transformed</param>
        /// <param name="AIgnorePrefix">strip any prefix</param>
        /// <param name="AIgnorePostfix">strip any postfix</param>
        /// <returns>the string in new convention</returns>
        public static string UpperCamelCase(String AStr, bool AIgnorePrefix, bool AIgnorePostfix)
        {
            return UpperCamelCase(AStr, '_', AIgnorePrefix, AIgnorePostfix);
        }

        /// <summary>
        /// General function for transforming a string from old style naming convention to new (e.g. a_account_hierarchy to AccountHierarchy)
        /// </summary>
        /// <param name="AStr">string to be transformed</param>
        /// <param name="ASeparator">separator that will mark the next character for uppercase</param>
        /// <param name="AIgnorePrefix">should prefixes be ignored</param>
        /// <param name="AIgnorePostfix">should postfixes be ignored</param>
        /// <returns>the string in new convention</returns>
        public static string UpperCamelCase(String AStr, char ASeparator, bool AIgnorePrefix, bool AIgnorePostfix)
        {
            string[] parts = AStr.Split(new char[] { ASeparator }, StringSplitOptions.None);

            if (parts.Length <= 1)       // Handle string without seperator
            {
                return AStr.Length > 1 ? char.ToUpper(AStr[0]).ToString() + AStr.Substring(1) : AStr;
            }

            int start = (AIgnorePrefix ? 1 : 0);     // ignore the first part
            int last = (AIgnorePostfix ? 1 : 0);     // ignore the last part

            for (int idx = start; idx < parts.Length - last; ++idx)
            {
                if (parts[idx].Length > 0)
                {
                    parts[idx] = char.ToUpper(parts[idx][0]).ToString() + parts[idx].Substring(1);
                }
            }

            return string.Join("", parts, start, parts.Length - start - last);
        }

        /// <summary>
        /// overload of UpperCamelCase; will always use the postfix and not drop it
        /// </summary>
        /// <param name="AStr">string to be modified</param>
        /// <param name="AIgnorePrefix">should prefix be ignored</param>
        /// <returns>converted string</returns>
        public static string UpperCamelCase(String AStr, bool AIgnorePrefix)
        {
            return UpperCamelCase(AStr, AIgnorePrefix, false);
        }

        /// <summary>
        /// overload for UpperCamelCase; will not drop postfix or prefix
        /// </summary>
        /// <param name="AStr">string to be changed</param>
        /// <returns>converted string</returns>
        public static string UpperCamelCase(String AStr)
        {
            return UpperCamelCase(AStr, false, false);
        }

        /// <summary>
        /// converts a string from UpperCamelCase back to a string with spaces; keeps the capital letters
        /// </summary>
        public static string ReverseUpperCamelCase(String AStr)
        {
            if (AStr.Length == 0)
            {
                return "";
            }

            string newString = AStr.Substring(0, 1);

            for (Int32 counter = 1; counter < AStr.Length; counter++)
            {
                if (Char.IsUpper(AStr[counter]) && (counter < AStr.Length - 1) && !Char.IsUpper(AStr[counter + 1]))
                {
                    newString += " ";
                }

                newString += AStr[counter];
            }

            return newString;
        }

        /// <summary>
        /// A Quote character is inserted at the beginning and end of the string, and each Quote character in the string is doubled.
        /// </summary>
        /// <param name="s">string to be quoted</param>
        /// <param name="delim">the quote character to be used (could be single or double quote, etc)</param>
        /// <returns>the quoted string</returns>
        public static string AnsiQuotedStr(string s, string delim)
        {
            return delim + s.Replace(delim, delim + delim) + delim;
        }

        /// <summary>
        /// reverse function for AnsiQuotedStr
        /// this will remove the quotes and deal with quotes contained in the string
        /// </summary>
        /// <param name="s">string to be stripped of quotes</param>
        /// <param name="delim">which quote character to look for and remove/replace</param>
        /// <returns>the string in non quoted form</returns>
        public static string AnsiDeQuotedStr(string s, string delim)
        {
            // A Quote character is inserted at the beginning and end of S, and each Quote character in the string is doubled.
            return s.Substring(1, s.Length - 2).Replace(delim + delim, delim);
        }

        /// <summary>
        /// this method attempts to convert a string to a decimal value;
        /// if it fails, no exception is thrown, but a default value is used instead
        /// </summary>
        /// <param name="s">string that should contain a float</param>
        /// <param name="ADefault">value to be used if there is no float in the string</param>
        /// <returns>the decimal value or the default value</returns>
        public static decimal TryStrToDecimal(string s, decimal ADefault)
        {
            decimal ReturnValue;

            if (!decimal.TryParse(s, out ReturnValue))
            {
                ReturnValue = ADefault;
            }

            return ReturnValue;
        }

        /// <summary>
        /// attempt to parse a string for an Integer; if it fails, return a default value
        /// </summary>
        /// <param name="s">the string containing an Integer</param>
        /// <param name="ADefault">alternative default value</param>
        /// <returns>the Integer value or the default value</returns>
        public static Int64 TryStrToInt(string s, Int64 ADefault)
        {
            Int64 ReturnValue;

            if (!Int64.TryParse(s, out ReturnValue))
            {
                ReturnValue = ADefault;
            }

            return ReturnValue;
        }

        /// <summary>
        /// attempt to parse a string for an Integer; if it fails, return a default value
        /// </summary>
        /// <param name="s">the string containing an Integer</param>
        /// <param name="ADefault">alternative default value</param>
        /// <returns>the Integer value or the default value</returns>
        public static Int32 TryStrToInt32(string s, Int32 ADefault)
        {
            Int32 ReturnValue;

            if (!Int32.TryParse(s, out ReturnValue))
            {
                ReturnValue = ADefault;
            }

            return ReturnValue;
        }

        /// <summary>
        /// attempt to parse a string for an decimal
        /// this is a little special for currencies, which was more important in Delphi than it is now in C#
        /// </summary>
        /// <param name="s">the string containing a currency value</param>
        /// <returns>the decimal value</returns>
        public static decimal TryStrToCurr(string s)
        {
            return Convert.ToDecimal(s);
        }

        /// <summary>
        /// print an integer into a string
        /// </summary>
        /// <param name="i">the integer value</param>
        /// <returns>a string containing the integer value</returns>
        public static string IntToStr(int i)
        {
            return i.ToString();
        }

        /// <summary>
        /// parse a string and return the Integer value
        /// </summary>
        /// <param name="s">the string containing the integer value</param>
        /// <returns>the integer value</returns>
        public static System.Int64 StrToInt(string s)
        {
            return Convert.ToInt64(s);
        }

        /// <summary>
        /// print a date into a string using a given format
        /// this will make sure that the correct separators are used (problem with dash and hyphen in original String.Format)
        /// </summary>
        /// <param name="date">the date to be printed</param>
        /// <param name="format">format string, eg. dd/MM/yyyy; see help for String.Format</param>
        /// <returns></returns>
        public static string DateToStr(System.DateTime date, string format)
        {
            string ReturnValue;

            // format e.g.: 'dd/MM/yyyy'
            ReturnValue = String.Format("{0:" + format + '}', date);

            if (format.IndexOf('/') != -1)
            {
                // even if the format is dd/MM/yyyy, on some configuration it will return ddMMyyyy
                ReturnValue = ReturnValue.Replace('-', '/');
            }

            return ReturnValue;
        }

        /// <summary>
        /// print a partner key with all leading zeros to a string
        /// a partner key has the form: 0027123456
        /// </summary>
        /// <param name="APartnerKey">partner key to be printed</param>
        /// <returns>the string containing a formatted version of the partner key</returns>
        public static String PartnerKeyToStr(System.Int64 APartnerKey)
        {
            String ReturnValue;
            String mNumber;

            if (APartnerKey < 0)
            {
                throw new EPartnerKeyOutOfRangeException("Partner Key number less than 0!");
            }

            if (APartnerKey > 9999999999)
            {
                throw new EPartnerKeyOutOfRangeException("Partner Key number more than 9999999999!");
            }

            try
            {
                mNumber = APartnerKey.ToString("0000000000");
            }
            catch (Exception)
            {
                throw new EPartnerKeyOutOfRangeException("Partner Key number conversion failure!");
            }

            if (mNumber.Length > 10)
            {
                throw new EPartnerKeyOutOfRangeException("Partner Key too long!");
            }
            else
            {
                ReturnValue = mNumber;
            }

            return ReturnValue;
        }

        /// <summary>
        /// parse a string and return the partner key
        /// </summary>
        /// <param name="APartnerKeyString">string containing the partner key</param>
        /// <returns>the Integer value of the partner key</returns>
        public static System.Int64 StrToPartnerKey(String APartnerKeyString)
        {
            System.Int64 ReturnValue;

            if (APartnerKeyString.Length > 10)
            {
                throw new EPartnerKeyOutOfRangeException("Partner Key to long!");
            }

            if ((APartnerKeyString == null) || (APartnerKeyString == ""))
            {
                throw new EPartnerKeyOutOfRangeException("No Data provided!");
            }

            try
            {
                ReturnValue = System.Convert.ToInt64(APartnerKeyString);
            }
            catch (Exception)
            {
                throw new EPartnerKeyOutOfRangeException("Partner Key number conversion failure!");
            }
            return ReturnValue;
        }

        /// <summary>
        /// format a partner key given as a string
        /// will add leading zeros etc
        /// will rotate the string, so drop a zero at the front in case there is an additional digit at the end
        /// this is used to help entering the partner key and always display leading zeros
        /// </summary>
        /// <param name="APartnerKeyString">the string to be formatted</param>
        /// <returns>the formatted string containing the partner key</returns>
        public static String FormatStrToPartnerKeyString(String APartnerKeyString)
        {
            String ReturnValue;

            System.Int32 mStringLength;
            System.Int32 mNumberOfZeros;
            System.String mPaddingString;

            // Initialisation
            ReturnValue = APartnerKeyString;
            mStringLength = APartnerKeyString.Length;

            // Test whether the string is empty or not
            if ((APartnerKeyString == null) || (APartnerKeyString == ""))
            {
                throw new EPartnerKeyOutOfRangeException("No Data provided!");
            }

            // Test whether the string consists out of digits
            try
            {
                System.Convert.ToInt64(APartnerKeyString);
            }
            catch (Exception)
            {
                throw new EPartnerKeyOutOfRangeException("Partner Key number conversion failure!");
            }

            // Formatting the string:
            switch (mStringLength)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:

                    // If there are not enough characters add zeros to the front.
                    mNumberOfZeros = 10 - APartnerKeyString.Length;
                    mPaddingString = new System.String('0', mNumberOfZeros);
                    ReturnValue = mPaddingString + APartnerKeyString;
                    break;

                case 10:

                    // Do nothing
                    ReturnValue = APartnerKeyString;
                    break;

                case 11:

                    // If a character was added lately
                    if (APartnerKeyString.StartsWith("0") == true)
                    {
                        ReturnValue = APartnerKeyString.Substring(1);
                    }
                    else
                    {
                        ReturnValue = APartnerKeyString.Substring(0, 10);
                    }

                    break;

                default:
                    ReturnValue = APartnerKeyString.Substring(0, 10);
                    break;
            }

            return ReturnValue;
        }

        /// <summary>
        /// checks if there is positive Integer in the string
        /// </summary>
        /// <param name="APositiveInteger">string to check</param>
        /// <returns>true if greater or equals 0</returns>
        public static bool IsStringPositiveInteger(String APositiveInteger)
        {
            Int64 Res64;
            bool ReturnValue = Int64.TryParse(APositiveInteger, out Res64);

            if (ReturnValue)
            {
                ReturnValue = (Res64 >= 0);
            }

            return ReturnValue;
        }

        /// <summary>
        ///  print a boolean value to string
        /// </summary>
        /// <param name="b">boolean value</param>
        /// <returns>a string containing the boolean value</returns>
        public static string BoolToStr(Boolean b)
        {
            return b.ToString();
        }

        /// <summary>
        /// reverse function for BoolToStr
        /// will parse a string for a boolean
        /// </summary>
        /// <param name="s">string containing the bool value</param>
        /// <returns>boolean value</returns>
        public static Boolean StrToBool(string s)
        {
            return Convert.ToBoolean(s);
        }

        /// <summary>
        /// makes sure that a null value is converted to an empty string, otherwise the string is returned.
        /// </summary>
        /// <param name="s">the string which can be null or have a proper string value</param>
        /// <returns>empty string or the contents of s</returns>
        public static string NullToEmptyString(string s)
        {
            if (s == null)
            {
                return "";
            }
            else
            {
                return s;
            }
        }

        /// <summary>
        /// This method formats a currency value, using an MS Access Style format string.
        /// examples: "(#,##0.00)" "#,##0.00 CR" etc.
        /// It returns a string with the written value according to the format.
        /// This function is only used by FormatCurrency,
        /// which is the function that should be called from outside.
        /// </summary>
        /// <param name="d">the double value to be formatted</param>
        /// <param name="format">format to be used to print the double</param>
        /// <returns>the formatted currency string</returns>
        public static String FormatCurrencyInternal(decimal d, String format)
        {
            String ReturnValue;
            Int32 counter;
            Int16 decimalPlaces;
            Int64 thousands;
            String group;

            System.Int32 startZero;
            System.Int32 endZero;
            System.Int32 NumberZeros;
            System.Int32 currentNumberDigits;
            String DecimalSeparator;
            String ThousandsSeparator;

            if (format.Trim().Length == 0)
            {
                return "";
            }

            // need to replace the decimal point with the correct decimal separator
            // assume the decimal point is used for describing the format in the first place
            DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            ThousandsSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            format = format.Replace(".", "<TOBETHEDECIMALOPERATOR>");
            format = format.Replace(",", "<TOBETHEGROUPOPERATOR>");
            format = format.Replace("<TOBETHEDECIMALOPERATOR>", DecimalSeparator);
            format = format.Replace("<TOBETHEGROUPOPERATOR>", ThousandsSeparator);

            if (d == 0)
            {
                return format;
            }

            decimalPlaces = 0;

            // count the zeros after the dot
            if (format.IndexOf(DecimalSeparator) > 0)
            {
                for (counter = format.IndexOf(DecimalSeparator) + 1; counter <= format.Length - 1; counter += 1)
                {
                    if ((format[counter] == '0') || (format[counter] == '9'))
                    {
                        decimalPlaces = (short)(decimalPlaces + 1);
                    }
                    else if (format[counter] != '0')
                    {
                        break;
                    }
                }
            }

            // display only the thousands?
            if ((format.IndexOf(">>") == -1) && (format.IndexOf('0') == -1))
            {
                d = d / 1000.0M;
            }

            d = Math.Round(d, decimalPlaces);
            ReturnValue = "";

            if (decimalPlaces > 0)
            {
                group = Convert.ToString(Convert.ToInt64((d - Math.Floor(d)) * (decimal)Math.Pow(10, decimalPlaces)));
                ReturnValue = ReturnValue + group;
                ReturnValue = new String('0', decimalPlaces - group.Length).ToString() + ReturnValue;
                ReturnValue = DecimalSeparator + ReturnValue;
            }

            thousands = Convert.ToInt64(Math.Pow(10, CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSizes[0]));

            if (decimalPlaces > 0)
            {
                if (d == 0)
                {
                    ReturnValue = '0' + ReturnValue;
                }
            }

            while (d != 0)
            {
                group = Convert.ToString((Convert.ToInt64(Math.Floor(d)) % thousands));
                ReturnValue = group + ReturnValue;
                d = Math.Floor(d / (decimal)Math.Pow(10, CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSizes[0]));

                if (d != 0)
                {
                    ReturnValue =
                        new String('0', CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSizes[0] - group.Length).ToString() + ReturnValue;

                    // only use group separators, if the format string also uses them (e.g. a comma)
                    if (format.IndexOf(ThousandsSeparator) != -1)
                    {
                        ReturnValue = ThousandsSeparator + ReturnValue;
                    }
                }
            }

            // get the number of leading zeros
            NumberZeros = 0;
            startZero = format.IndexOf('0');

            if (startZero == -1)
            {
                startZero = format.IndexOf('9');
            }

            if (startZero != -1)
            {
                endZero = format.IndexOf(DecimalSeparator, startZero) - 1;

                if (endZero < 0)
                {
                    endZero = startZero;

                    // count all the digits
                    while ((endZero + 1 < format.Length) && ((format[endZero + 1] == '0') || (format[endZero + 1] == '9')))
                    {
                        endZero++;
                    }
                }

                NumberZeros = endZero - startZero + 1;
            }

            // fill up with leading zeros
            // don't worry about group separators at the moment
            currentNumberDigits = ReturnValue.IndexOf(DecimalSeparator);

            if (currentNumberDigits < 0)
            {
                currentNumberDigits = ReturnValue.Length;
            }

            if (currentNumberDigits < NumberZeros)
            {
                ReturnValue = new String('0', NumberZeros - currentNumberDigits).ToString() + ReturnValue;
            }

            // add anything before # or 0
            for (counter = 0; counter <= format.Length - 1; counter += 1)
            {
                if ((format[counter] == '#') || (format[counter] == '>') || (format[counter] == '0') || (format[counter] == '9'))
                {
                    ReturnValue = format.Substring(0, counter) + ReturnValue;
                    break;
                }
            }

            for (counter = (short)(format.Length - 1); counter >= 0; counter -= 1)
            {
                if ((format[counter] == '#') || (format[counter] == '>') || (format[counter] == '0') || (format[counter] == '9'))
                {
                    ReturnValue = ReturnValue + format.Substring(counter + 1);
                    break;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// find the appropriate format string, using general format (for the whole column) and the type of the current value
        /// </summary>
        /// <param name="AVariantFormatString">what kind of value are we talking about, eg. currency</param>
        /// <param name="AGeneralFormatString">defining the format on a higher level, e.g. CurrencyWithoutDecimals or CurrencyThousands etc</param>
        /// <returns>a string format similar to Access string format (format for positive, negative, zero, null)</returns>
        public static String GetFormatString(String AVariantFormatString, String AGeneralFormatString)
        {
            String ReturnValue;
            String formatZero;
            String format;

            // sort out the format parameter which could be modifiying the format of the variant (e.g. CurrencyThousands on a specific currency format)
            if (AGeneralFormatString.Length != 0)
            {
                // try to reshape the currency format, to fit either CurrencyWithoutDecimals or CurrencyThousands
                if (IsSame(AGeneralFormatString, "CurrencyWithoutDecimals"))
                {
                    // recursive call, to e.g. convert the name currency to the format string
                    format = GetFormatString(AVariantFormatString, "");

                    // "#,##0.00;#,##0.00; ; "
                    while (format.IndexOf(".00") != -1)
                    {
                        format = format.Substring(0, format.IndexOf(".00")) + format.Substring(format.IndexOf(".00") + 3);
                    }

                    // ">>>,>>>,>>>,>>9.99"
                    while (format.IndexOf(".99") != -1)
                    {
                        format = format.Substring(0, format.IndexOf(".99")) + format.Substring(format.IndexOf(".99") + 3);
                    }
                }
                else if (IsSame(AGeneralFormatString, "CurrencyThousands"))
                {
                    format = GetFormatString(AVariantFormatString, "");

                    // '#,##0.00;#,##0.00; ; '
                    while (format.IndexOf("#0.00") != -1)
                    {
                        format = format.Substring(0, format.IndexOf("#0.00")) + format.Substring(format.IndexOf("#0.00") + 5);
                    }

                    // for 0.00, replace with 0
                    while (format.IndexOf(".00") != -1)
                    {
                        format = format.Substring(0, format.IndexOf(".00")) + format.Substring(format.IndexOf(".00") + 3);
                    }

                    // ">>>,>>>,>>>,>>9.99"
                    while (format.IndexOf("9.99") != -1)
                    {
                        format = format.Substring(0, format.IndexOf("9.99")) + format.Substring(format.IndexOf("9.99") + 4);
                    }
                }
                else if (IsSame(AGeneralFormatString, "CurrencyComplete"))
                {
                    format = "#,##0.00;(#,##0.00);0.00;0";
                }
                else if (IsSame(AVariantFormatString, "currency"))
                {
                    // this is needed to format the values for the debit/credit columns, when applying e.g. #,##0.00; ; ; ;
                    format = AGeneralFormatString;
                }
                else if (AVariantFormatString.Length == 0)
                {
                    format = AGeneralFormatString;
                }
                else
                {
                    format = AVariantFormatString;
                }
            }
            else
            {
                format = AVariantFormatString;
            }

            if (IsSame(format, "Currency"))
            {
                ReturnValue = "#,##0.00;(#,##0.00);0.00;0";
            }
            else if (IsSame(format, "CurrencyWithoutDecimals"))
            {
                ReturnValue = "#,##0;(#,##0);0;";
            }
            else if (IsSame(format, "CurrencyThousands"))
            {
                ReturnValue = "#;(#);0;";
            }
            else if (IsSame(format, "percentage"))
            {
                ReturnValue = "0%;-0%;0;";
            }
            else if (IsSame(format, "percentage2decimals"))
            {
                ReturnValue = "0.00%;-0.00%;0.00%;";
            }
            else if (IsSame(format, "currencycreditdebit"))
            {
                ReturnValue = "#,##0.00;#,##0.00; ; ";
            }
            else if (IsSame(format, "currencyCRDR"))
            {
                ReturnValue = "#,##0.00 CR;#,##0.00 DR;0.00; ";
            }
            else if (IsSame(format, "partnerkey"))
            {
                ReturnValue = "0000999999;;;";
            }
            else if (IsSame(format, "nothing"))
            {
                ReturnValue = " ; ; ; ";
            }
            else if (format.IndexOf(">>") != -1)
            {
                // To convert the Progress format (e.g. >>>,>>>,>>>,>>9.99, stored in a_currency.a_display_format_c) to the Access format
                // for 0.00, get the format of 0.01 and replace the number 1 with 0); need to replace the decimal operator again.
                formatZero = FormatCurrencyInternal(0.01M, format.Substring(1)).Replace('1', '0').Replace(
                    CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator,
                    ".");
                ReturnValue = format.Substring(1) + ';' + format + ';' + formatZero + ';' + formatZero;
            }
            else if (format.IndexOf(';') != -1)
            {
                // Access Format, just pass it through
                ReturnValue = format;
            }
            else
            {
                // text, row, etc.
                ReturnValue = "";
            }

            return ReturnValue;
        }

        /// <summary>
        /// check if the given format is for currencies
        /// </summary>
        /// <param name="AFormatString">format string to check</param>
        /// <returns>true if the format contains the name currency or characters that are indicating a currency format</returns>
        public static Boolean IsCurrencyFormatString(String AFormatString)
        {
            return (AFormatString.ToLower().IndexOf("currency") == 0) || (AFormatString.IndexOf("#,##") != -1)
                   || (AFormatString.IndexOf(">,>>") != -1);
        }

        /// <summary>
        /// print a currency value (actually all sorts of values, e.g. dates) into a string
        /// </summary>
        /// <param name="value">the value to be printed</param>
        /// <param name="format">the format to be used; can be dayofyear for birthdays, currency, etc</param>
        /// <returns>the formatted string</returns>
        public static String FormatCurrency(TVariant value, String format)
        {
            String ReturnValue;
            decimal d;
            DateTime ThisYearDate;

            // for partnerkey
            String OrigFormat;

            ReturnValue = "";

            if (format.ToLower() == "dayofyear")
            {
                if (value.IsZeroOrNull())
                {
                    ReturnValue = "N/A";
                }
                else
                {
                    ThisYearDate = new DateTime(DateTime.Today.Year, value.ToDate().Month, value.ToDate().Day);
                    ReturnValue = DateToStr(ThisYearDate, "dd-MMM").ToUpper();
                }

                return ReturnValue;
            }

            if (format == null)
            {
                format = "";
            }

            OrigFormat = format;

            if (value.TypeVariant == eVariantTypes.eString)
            {
                format = "";
            }
            else
            {
                format = GetFormatString(value.FormatString, format);
            }

            if (value != null)
            {
                if ((format == null) || (format.Length == 0))
                {
                    return value.ToString();
                }

                String formatPositive = GetNextCSV(ref format, ";");
                String formatNegative = GetNextCSV(ref format, ";");
                String formatZero = GetNextCSV(ref format, ";");
                String formatNil = GetNextCSV(ref format, ";");

                if ((OrigFormat.ToLower() == "partnerkey") || (value.FormatString == "partnerkey"))
                {
                    if (value.ToInt64() <= 0)
                    {
                        ReturnValue = FormatCurrencyInternal(0, "0000000000");
                    }
                    else
                    {
                        ReturnValue = FormatCurrencyInternal(value.ToDecimal(), formatPositive);
                    }
                }
                else if ((value.TypeVariant == eVariantTypes.eDecimal)
                         || (value.TypeVariant == eVariantTypes.eCurrency)
                         || (value.TypeVariant == eVariantTypes.eInteger))
                {
                    d = value.ToDecimal();

                    if (d > 0)
                    {
                        ReturnValue = FormatCurrencyInternal(d, formatPositive);
                    }
                    else if (d < 0)
                    {
                        ReturnValue = FormatCurrencyInternal(Math.Abs(d), formatNegative);
                    }
                    else
                    {
                        // (d == 0)
                        ReturnValue = FormatCurrencyInternal(d, formatZero);
                    }
                }
                else if (value.IsZeroOrNull())
                {
                    ReturnValue = FormatCurrencyInternal(0, formatNil);
                }
                else
                {
                    ReturnValue = value.ToString();
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// overload for FormatCurrency, using a decimal value
        /// </summary>
        /// <param name="value">value to be formatted</param>
        /// <param name="format">format to be used</param>
        /// <returns>the formatted string</returns>
        public static String FormatCurrency(decimal value, String format)
        {
            return FormatCurrency(new TVariant(value), format);
        }

        private static DataTable CurrencyFormats = null;

        /// <summary>
        /// Use this for displaying currency-sensitive amounts.
        /// </summary>
        /// <param name="AValue"></param>
        /// <param name="ACurrencyCode"></param>
        /// <returns></returns>
        public static String FormatUsingCurrencyCode(decimal AValue, String ACurrencyCode)
        {
            String format = "->>>,>>>,>>>,>>9.99";

            if (CurrencyFormats != null)
            {
                CurrencyFormats.DefaultView.RowFilter = String.Format("a_currency_code_c='{0}'", ACurrencyCode);

                if (CurrencyFormats.DefaultView.Count > 0)
                {
                    format = CurrencyFormats.DefaultView[0].Row["a_display_format_c"].ToString();
                }
            }

            return StringHelper.FormatCurrency(new TVariant(AValue), format);
        }

        /// <summary></summary>
        /// <param name="ACurrencyCode"></param>
        /// <returns></returns>
        public static int DecimalPlacesForCurrency(String ACurrencyCode)
        {
            int Ret = 2;

            if (CurrencyFormats != null)
            {
                CurrencyFormats.DefaultView.RowFilter = String.Format("a_currency_code_c='{0}'", ACurrencyCode);

                if (CurrencyFormats.DefaultView.Count > 0)
                {
                    String format = CurrencyFormats.DefaultView[0].Row["a_display_format_c"].ToString();
                    int dotPos = format.LastIndexOf('.');

                    if (dotPos > 0)
                    {
                        Ret = format.Length - 1 - dotPos;
                    }
                    else
                    {
                        Ret = 0;
                    }
                }
            }

            return Ret;
        }

        /// <summary>
        /// If this is not given (during initialisation), a default format will be used.
        /// </summary>
        public static DataTable CurrencyFormatTable
        {
            set
            {
                CurrencyFormats = value;
            }
        }

        /// <summary>
        /// returns the full name of the month, using .net localised information
        /// </summary>
        /// <param name="monthNr">the number of the month (starting January = 1)</param>
        /// <returns>the printed name of the month</returns>
        public static String GetLongMonthName(Int32 monthNr)
        {
            return DateTimeFormatInfo.CurrentInfo.GetMonthName(monthNr);
        }

        /// <summary>
        /// return a string with just the date, no time information
        /// </summary>
        /// <param name="ADateTime">the date to print</param>
        /// <returns>the date printed into a string</returns>
        public static String DateToLocalizedString(DateTime ADateTime)
        {
            return DateToLocalizedString(ADateTime, false, false);
        }

        /// <summary>
        /// overload for nullable DateTime
        /// </summary>
        /// <param name="ADateTime"></param>
        /// <returns></returns>
        public static String DateToLocalizedString(DateTime ? ADateTime)
        {
            if (ADateTime == null)
            {
                return "";
            }

            return DateToLocalizedString(ADateTime.Value, false, false);
        }

        /// <summary>
        /// return a string with the date and optionally with the time
        /// </summary>
        /// <param name="ADateTime">the date to print</param>
        /// <param name="AIncludeTime">if true then the time is printed as well</param>
        /// <returns>the printed date</returns>
        public static String DateToLocalizedString(DateTime ADateTime, Boolean AIncludeTime)
        {
            return DateToLocalizedString(ADateTime, true, false);
        }

        /// <summary>
        /// print a date to string, optionally with time and even seconds
        /// </summary>
        /// <param name="ADateTime">the date to print</param>
        /// <param name="AIncludeTime">want to print time</param>
        /// <param name="ATimeWithSeconds">want to print seconds</param>
        /// <returns>the formatted date string</returns>
        public static String DateToLocalizedString(DateTime ADateTime, Boolean AIncludeTime, Boolean ATimeWithSeconds)
        {
            String ReturnValue = "";

            ReturnValue = ADateTime.ToString("dd-MMM-yyyy").ToUpper();

            // need to do something about german, Month March, should be rather M&Auml;R than MRZ?
            // see also http:www.codeproject.com/csharp/csdatetimelibrary.asp?df=100&forumid=157955&exp=0&select=1387944
            // and http:www.nntp.perl.org/group/perl.datetime/2003/05/msg2250.html
            // better solution has been implemented: for export to CSV/Excel, the date should not be formatted as text, but formatted by the export/print program...

            // Mono and .Net return different strings for month of March in german culture
            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de")
            {
                if (ADateTime.Month == 3)
                {
                    ReturnValue = ReturnValue.Replace("MRZ", "MÄR");
                }
            }

            // todo use short month names from local array, similar to GetLongMonthName
            if (ATimeWithSeconds)
            {
                if (ATimeWithSeconds)
                {
                    ReturnValue = ReturnValue + ' ' + ADateTime.ToLongTimeString();
                }
                else
                {
                    ReturnValue = ReturnValue + ' ' + ADateTime.ToShortTimeString();
                }
            }

            return ReturnValue;
        }

/*
 *      private static ArrayList months = new ArrayList();
 *      /// <summary>
 *      /// initialize the localized month names
 *      /// not used at the moment; using .net localisation instead
 *      /// </summary>
 *      /// <param name="monthNames">an array of the month names</param>
 *      public static void SetLocalizedMonthNames(ArrayList monthNames)
 *      {
 *          months = new ArrayList();
 *
 *          foreach (string s in monthNames)
 *          {
 *              months.Add(s);
 *          }
 *      }
 */
        /// <summary>
        /// Finds a matching closing bracket in a String.
        /// </summary>
        /// <param name="AInspectString">The String to inspect.</param>
        /// <param name="AStartPos">Start position from which to search (must be >= 0).</param>
        /// <param name="ABracketChar">Opening bracket character. Supported are '(', '[' and '{'.</param>
        /// <returns>If return value is positive: The position in string of the matching closing bracket. If return
        /// value is negative and not -9999: uneven opening and closing of bracktes. The number is the of missing
        /// closing brackets. If return value is -9999: AStartPos was greater than the string has characters!</returns>
        public static int FindMatchingEndBracket(string AInspectString, int AStartPos, char ABracketChar)
        {
            int ReturnValue = -9999;
            char CurrentChar;
            char ClosingBracketChar;
            int ExtraBracketOccurances = 0;

            #region Working on Arguments

            if (AStartPos <= 0)
            {
                throw new ArgumentException("AStartPos Argument must greater than 0");
            }

            if (ABracketChar == '(')
            {
                ClosingBracketChar = ')';
            }
            else if (ABracketChar == '[')
            {
                ClosingBracketChar = ']';
            }
            else if (ABracketChar == '{')
            {
                ClosingBracketChar = '}';
            }
            else
            {
                throw new ArgumentException(
                    "Character submitted in ABracketChar is not supported. Supported bracket characters are: '(', '[' and '{'.");
            }

            #endregion

            for (int Counter = AStartPos; Counter < AInspectString.Length; Counter++)
            {
                CurrentChar = AInspectString[Counter];

                if (CurrentChar == ABracketChar)
                {
                    ExtraBracketOccurances++;
                    ReturnValue = (ExtraBracketOccurances * -1);  // set ReturnValue in case no matching number of closing Brackets is ever found!
                }
                else if (CurrentChar == ClosingBracketChar)
                {
                    if (ExtraBracketOccurances > 0)
                    {
                        ExtraBracketOccurances--;
                        ReturnValue = (ExtraBracketOccurances * -1);  // set ReturnValue in case no matching number of closing Brackets is ever found!
                    }
                    else
                    {
                        // We have found the matching closing bracket!
                        ReturnValue = Counter;
                        break;
                    }
                }
            }

/*
 *          Console.WriteLine(String.Format(
 *                  "FindMatchingEndBracketPos for AInspectString '{0}', AStartPos: {1}; ABracketChar: '{2}':   Closing bracket '{3}' found at position {4}.",
 *                  AInspectString, AStartPos, ABracketChar, ClosingBracketChar, ReturnValue));
 *
 */
            return ReturnValue;
        }

        /// check if the needle occurs in s, ignoring case sensitivity
        static public Boolean ContainsI(string s, string needle)
        {
            return s.IndexOf(needle, StringComparison.OrdinalIgnoreCase) > -1;
        }

        /// <summary>
        /// Count the occurences of a certain character in a string
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int CountOccurencesOfChar(string instance, char c)
        {
            int result = 0;

            foreach (char curChar in instance)
            {
                if (c == curChar)
                {
                    ++result;
                }
            }

            return result;
        }
    }
}