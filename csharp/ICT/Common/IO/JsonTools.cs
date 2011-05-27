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
using System.Xml;
using System.Collections;
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

            JsonObject list = (JsonObject)JsonConvert.Import(AJsonData);

            foreach (DictionaryEntry entry in list)
            {
                string text = entry.Value.ToString().Replace("<", "&lt;").Replace(">", "&gt;");
                ATemplate = ATemplate.Replace("#" + entry.Key.ToString().ToUpper(), text);
            }

            return ATemplate;
        }

        /// <summary>
        /// internal function to be used by RemoveContainerControls.
        /// important side effect: will set the current culture, so that the dates can be parsed correctly
        /// </summary>
        /// <param name="ARoot"></param>
        /// <returns></returns>
        private static string parseJSonValues(JsonObject ARoot)
        {
            string result = "";

            foreach (string key in ARoot.Names)
            {
                if (key.ToString().StartsWith("ext-comp"))
                {
                    string content = parseJSonValues((JsonObject)ARoot[key]);

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

                    if (key.EndsWith("CountryCode"))
                    {
                        // we need this so that we can parse the dates correctly from json
                        Ict.Common.Catalog.Init(ARoot[key].ToString(), ARoot[key].ToString());
                    }

                    result += "\"" + key + "\":\"" + ARoot[key].ToString().Replace("\n", "<br/>").Replace("\"", "&quot;") + "\"";
                }
            }

            return result;
        }

        /// <summary>
        /// remove ext-comp controls, for multi-page forms.
        /// important side effect: will set the current culture, so that the dates can be parsed correctly
        /// </summary>
        /// <param name="AJSONFormData"></param>
        /// <returns></returns>
        public static string RemoveContainerControls(string AJSONFormData)
        {
            // to avoid strange error messages during testing
            if (AJSONFormData.Length == 0)
            {
                return String.Empty;
            }

            JsonObject root = (JsonObject)Jayrock.Json.Conversion.JsonConvert.Import(AJSONFormData);

            string result = "{" + parseJSonValues(root) + "}";

            return result;
        }

        /// <summary>
        /// import JSON code into a typed structure.
        /// important side effect: will set the current culture, so that the dates can be parsed correctly
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

            return Jayrock.Json.Conversion.JsonConvert.Import(ATypeOfStructure,
                RemoveContainerControls(AJSONFormData));
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

            return (JsonObject)Jayrock.Json.Conversion.JsonConvert.Import(AJSONFormData);
        }

        /// <summary>
        /// reverse of ParseValues
        /// </summary>
        /// <param name="AParsedObject"></param>
        /// <returns></returns>
        public static string ToJsonString(JsonObject AParsedObject)
        {
            return Jayrock.Json.Conversion.JsonConvert.ExportToString(AParsedObject);
        }
    }
}