//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Jayrock.Json;
using Jayrock.Json.Conversion;

namespace Ict.Common.IO
{
    /// <summary>
    /// some useful functions for dealing with json encoded strings
    /// </summary>
    public class TJsonTools
    {
        /// <summary>
        /// print all data from the submitted form into an HTML table;
        /// </summary>
        /// <param name="AJsonData"></param>
        /// <returns></returns>
        public static string DataToHTMLTable(string AJsonData)
        {
            if (AJsonData.Length == 0)
            {
                return String.Empty;
            }

            string RequiredCulture = CultureInfo.CurrentCulture.Name;
            AJsonData = RemoveContainerControls(AJsonData, ref RequiredCulture);

            string Result = "<table cellspacing=\"2\">";
            JsonObject list = (JsonObject)JsonConvert.Import(AJsonData);

            foreach (string key in list.Names)
            {
                string text = list[key].ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("&quot;", "-");
                Result += String.Format("<tr><td>{0}</td><td>{1}</td></tr>", key, text);
            }

            Result += "</table>";

            return Result;
        }

        /// <summary>
        /// insert the data into an Xml Node. This is useful for later converting the Xml to meaningful CSV with the same columns for Excel or Calc
        /// </summary>
        /// <param name="AJsonData"></param>
        /// <param name="ANode"></param>
        /// <param name="ADoc"></param>
        /// <param name="AOverwrite"></param>
        public static void DataToXml(string AJsonData, ref XmlNode ANode, XmlDocument ADoc, bool AOverwrite)
        {
            if (AJsonData.Length == 0)
            {
                return;
            }

            try
            {
                string RequiredCulture = CultureInfo.CurrentCulture.Name;
                AJsonData = RemoveContainerControls(AJsonData, ref RequiredCulture);

                JsonObject list = (JsonObject)JsonConvert.Import(AJsonData);

                foreach (string key in list.Names)
                {
                    if (AOverwrite || !TXMLParser.HasAttribute(ANode, key))
                    {
                        XmlAttribute attr = ADoc.CreateAttribute(StringHelper.UpperCamelCase(key));
                        string text = list[key].ToString().Replace("<br/>", "_");
                        attr.Value = text;

                        ANode.Attributes.Append(attr);
                    }
                }
            }
            catch (Exception)
            {
                TLogging.Log("Problem parsing: " + AJsonData);
                throw;
            }
        }

        /// <summary>
        /// replace all keywords (starting with #) with the values of the json variables with the same name
        /// </summary>
        /// <param name="AJsonData"></param>
        /// <param name="ATemplate"></param>
        /// <returns></returns>
        public static string ReplaceKeywordsWithData(string AJsonData, string ATemplate)
        {
            if (AJsonData.Length == 0)
            {
                return ATemplate;
            }

            string RequiredCulture = CultureInfo.CurrentCulture.Name;
            AJsonData = RemoveContainerControls(AJsonData, ref RequiredCulture);

            JsonObject list = (JsonObject)JsonConvert.Import(AJsonData);

            foreach (JsonMember entry in list)
            {
                string text = entry.Value.ToString().Replace("<", "&lt;").Replace(">", "&gt;");
                ATemplate = ATemplate.Replace("#" + entry.Name.ToString().ToUpper(), text);
            }

            return ATemplate;
        }

        /// <summary>
        /// internal function to be used by RemoveContainerControls.
        /// will give information for the required culture, so that the dates can be parsed correctly
        /// </summary>
        /// <returns></returns>
        private static string parseJSonValues(JsonObject ARoot, ref string ACulture)
        {
            string result = "";

            foreach (string key in ARoot.Names)
            {
                if (key.ToString().StartsWith("ext-comp") || key.ToString().StartsWith("card-"))
                {
                    string content = parseJSonValues((JsonObject)ARoot[key], ref ACulture);

                    if (content.Length > 0)
                    {
                        if (result.Length > 0)
                        {
                            result += ",";
                        }

                        result += content;
                    }
                }
                else
                {
                    if (result.Length > 0)
                    {
                        result += ",";
                    }

                    if (key.EndsWith("CountryCode") && (ARoot[key].ToString().Trim().Length > 0))
                    {
                        // we need this so that we can parse the dates correctly from json
                        ACulture = ARoot[key].ToString();
                    }

                    result += "\"" + key + "\":\"" + ARoot[key].ToString().Replace("\n", "<br/>").Replace("\"", "&quot;") + "\"";
                }
            }

            return result;
        }

        /// <summary>
        /// remove ext-comp controls, for multi-page forms.
        /// this overload does not require a ref string for the culture
        /// </summary>
        /// <returns></returns>
        public static string RemoveContainerControls(string AJSONFormData)
        {
            string dummy = string.Empty;

            return RemoveContainerControls(AJSONFormData, ref dummy);
        }

        /// <summary>
        /// remove ext-comp controls, for multi-page forms.
        /// will give information for the required culture, so that the dates can be parsed correctly
        /// </summary>
        /// <returns></returns>
        public static string RemoveContainerControls(string AJSONFormData, ref string ARequiredCulture)
        {
            // to avoid strange error messages during testing
            if (AJSONFormData.Length == 0)
            {
                return String.Empty;
            }

            AJSONFormData = AJSONFormData.Replace(Environment.NewLine, "<br/>");
            AJSONFormData = AJSONFormData.Replace("\\\\\"", "&quot;");
            AJSONFormData = AJSONFormData.Replace("\"\"\"", "\"\"");

            if (!AJSONFormData.StartsWith("{"))
            {
                // at the moment, we cannot fix arrays
                return AJSONFormData;
            }

            try
            {
                JsonObject root = (JsonObject)Jayrock.Json.Conversion.JsonConvert.Import(AJSONFormData);

                string result = "{" + parseJSonValues(root, ref ARequiredCulture) + "}";

                return result;
            }
            catch (Exception e)
            {
                // we have some json strings which do include unescaped quotes, which causes confusion

                string copy = AJSONFormData;

                // simple fix for flat list. advantage over the replace method: only quote colon quote are searched, quote comma quote is handled correctly inside a value
                if (!AJSONFormData.Substring(1).Contains("{"))
                {
                    // find the names first, by looking for quote colon quote. the values must not contain those 3 characters in that order!!!
                    int posColon = AJSONFormData.IndexOf("\":\"");

                    List <string>names = new List <string>();

                    while (posColon != -1)
                    {
                        string before = AJSONFormData.Substring(0, posColon);
                        int posBeginName = before.LastIndexOf("\"");
                        string name = before.Substring(posBeginName + 1);
                        names.Add(name);
                        posColon = AJSONFormData.IndexOf("\":\"", posColon + 1);
                    }

                    SortedList <string, string>values = new SortedList <string, string>();

                    names.Reverse();
                    int posNextName = AJSONFormData.Length - 1;

                    foreach (string name in names)
                    {
                        int indexOfName = AJSONFormData.IndexOf(",\"" + name + "\":\"");

                        if (indexOfName == -1)
                        {
                            // first value
                            indexOfName = AJSONFormData.IndexOf("{\"" + name + "\":\"");
                        }

                        int indexOfValue = indexOfName + name.Length + 5;
                        string value = AJSONFormData.Substring(indexOfValue, posNextName - indexOfValue - 1);
                        values.Add(name, value);
                        posNextName = indexOfName;
                    }

                    names.Reverse();
                    StringBuilder s = new StringBuilder("{");

                    foreach (string name in names)
                    {
                        s.Append("\"");
                        s.Append(name);
                        s.Append("\":\"");
                        s.Append(values[name].Replace("\"", "&quot;"));
                        s.Append("\",");
                    }

                    s.Remove(s.Length - 1, 1);
                    s.Append("}");
                    copy = s.ToString();
                }
                else
                {
                    // fix also more complex strings, with several {} lists
                    // disadvantage over first method: more string combinations are disallowed in the values, eg. ","
                    copy = copy.Replace("{\"", "{'");
                    copy = copy.Replace("\":\"", "':'");
                    copy = copy.Replace("\":{", "':{");
                    copy = copy.Replace("\",\"", "','");
                    copy = copy.Replace("\"},\"", "'},'");
                    copy = copy.Replace("\"}", "'}");

                    copy = copy.Replace("\"", "&quot;");
                }

                // try again
                try
                {
                    JsonObject root = (JsonObject)Jayrock.Json.Conversion.JsonConvert.Import(copy);

                    string result = "{" + parseJSonValues(root, ref ARequiredCulture) + "}";

                    return result;
                }
                catch (Exception)
                {
                    TLogging.Log("problem parsing: " + AJSONFormData);
                    TLogging.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// import JSON code into a typed structure.
        /// </summary>
        /// <param name="ATypeOfStructure"></param>
        /// <param name="AJSONFormData"></param>
        /// <returns>an object of the given type, you just have to cast it to your type</returns>
        public static Object ImportIntoTypedStructure(System.Type ATypeOfStructure, string AJSONFormData)
        {
            if (AJSONFormData.Length == 0)
            {
                return new Jayrock.Json.JsonObject();
            }

            // set the current culture, so that the dates can be parsed correctly
            string RequiredCulture = CultureInfo.CurrentCulture.Name;
            string withoutContainers = RemoveContainerControls(AJSONFormData, ref RequiredCulture);
            CultureInfo OrigCulture = Catalog.SetCulture(RequiredCulture);

            try
            {
                return Jayrock.Json.Conversion.JsonConvert.Import(ATypeOfStructure,
                    withoutContainers);
            }
            catch (Exception)
            {
                TLogging.Log("Problem parsing JSON object: " + AJSONFormData);
                throw;
            }
            finally
            {
                Catalog.SetCulture(OrigCulture);
            }
        }

        /// <summary>
        /// parse the string to a JsonObject which can be iterated like this:
        ///     foreach (string key in MyJsonObject.Names)
        ///     {
        ///             Console.WriteLine(key + " " + MyJsonObject[key].ToString());
        ///     }
        /// </summary>
        /// <param name="AJSONFormData">it is recommended to run RemoveContainerControls on that parameter first</param>
        /// <returns></returns>
        public static JsonObject ParseValues(string AJSONFormData)
        {
            if (AJSONFormData.Length == 0)
            {
                return new Jayrock.Json.JsonObject();
            }

            // set the current culture, so that the dates can be parsed correctly
            string RequiredCulture = CultureInfo.CurrentCulture.Name;
            AJSONFormData = RemoveContainerControls(AJSONFormData, ref RequiredCulture);
            CultureInfo OrigCulture = Catalog.SetCulture(RequiredCulture);

            try
            {
                return (JsonObject)Jayrock.Json.Conversion.JsonConvert.Import(AJSONFormData);
            }
            catch (Exception)
            {
                TLogging.Log("Problem parsing JSON object: " + AJSONFormData);
                throw;
            }
            finally
            {
                Catalog.SetCulture(OrigCulture);
            }
        }

        /// <summary>
        /// reverse of ParseValues
        /// </summary>
        /// <param name="AParsedObject"></param>
        /// <returns></returns>
        public static string ToJsonString(JsonObject AParsedObject)
        {
            // look for country id
            string RequiredCulture = CultureInfo.CurrentCulture.Name;

            foreach (string key in AParsedObject.Names)
            {
                if (key.EndsWith("CountryCode") && (AParsedObject[key].ToString().Trim().Length > 0))
                {
                    // we need this so that we can format the dates correctly into the json string
                    RequiredCulture = AParsedObject[key].ToString();
                }
            }

            CultureInfo OrigCulture = Catalog.SetCulture(RequiredCulture);

            string Result = Jayrock.Json.Conversion.JsonConvert.ExportToString(AParsedObject);

            Catalog.SetCulture(OrigCulture);

            return Result;
        }
    }
}