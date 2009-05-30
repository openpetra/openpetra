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
using System.Collections;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Tools.CodeGeneration
{
    /// <summary>
    /// Description of ProcessTemplate.
    /// </summary>
    public class ProcessTemplate
    {
        public String FTemplateCode = "";
        public String FDestinationFile = "";
        public SortedList FCodelets = new SortedList();

        public ProcessTemplate()
        {
            // need to set FTemplateCode manually
        }

        public ProcessTemplate(string AFullPath)
        {
            if (!File.Exists(AFullPath))
            {
                throw new Exception("Cannot find template file " + AFullPath + "; please adjust the TemplateDir parameter");
            }

            StreamReader r;
            r = File.OpenText(AFullPath);
            FTemplateCode = r.ReadToEnd();
            r.Close();
        }

        // check if all placeholders have been replaced in the template; ignore IFDEF
        public Boolean CheckTemplateCompletion(string s)
        {
            int posPlaceholder = s.IndexOf("{#");
            string remainingTemplatePlaceholders = "";

            while (posPlaceholder > -1)
            {
                s = s.Substring(posPlaceholder + 2);
                string latestPlaceholder = s.Substring(0, s.IndexOf("}"));
                remainingTemplatePlaceholders +=
                    latestPlaceholder + Environment.NewLine;
                s = s.Substring(s.IndexOf("}"));
                s = s.Replace("{#" + latestPlaceholder + "}", "todo");
                posPlaceholder = s.IndexOf("{#");
            }

            if (remainingTemplatePlaceholders.Length > 0)
            {
                StreamWriter FWriter;
                FWriter = File.CreateText(FDestinationFile + ".error");
                FWriter.Write(FTemplateCode);
                FWriter.Close();

                throw new Exception("The template has not completely been filled in. " +
                    Environment.NewLine + "You are missing: " + Environment.NewLine +
                    remainingTemplatePlaceholders + Environment.NewLine +
                    "Check file " + FDestinationFile + ".error");
            }

            return true;
        }

        public string RemoveUndefinedIFDEFs(string s)
        {
            int posPlaceholder = s.IndexOf("{#IFDEF ");

            while (posPlaceholder > -1)
            {
                string name = s.Substring(posPlaceholder + 8, s.IndexOf("}", posPlaceholder) - posPlaceholder - 8);
                int posPlaceholderAfter = s.IndexOf("{#ENDIF " + name + "}");

                if (posPlaceholderAfter == -1)
                {
                    throw new Exception("The template has a bug. " +
                        Environment.NewLine + "We are missing the ENDIF for: " + name);
                }

                s = s.Substring(0, posPlaceholder - 1) + s.Substring(s.IndexOf("}", posPlaceholderAfter) + 1);
                posPlaceholder = s.IndexOf("{#IFDEF ");
            }

            return s;
        }

        private int GetNextParameterisedTemplate(string APlaceholder, int posPlaceholder, ref ArrayList result)
        {
            int posEndPlaceholder = FTemplateCode.IndexOf("}", posPlaceholder);
            string parameterisedPlaceholder = FTemplateCode.Substring(posPlaceholder, posEndPlaceholder - posPlaceholder + 1);
            string line = parameterisedPlaceholder;

            // this will get the {
            StringHelper.GetNextToken(ref parameterisedPlaceholder);

            // this will get the #APlaceholder
            StringHelper.GetNextToken(ref parameterisedPlaceholder);

            // now get all the parameters
            while (parameterisedPlaceholder.Length > 0 && parameterisedPlaceholder != "}")
            {
                string paramId = StringHelper.GetNextToken(ref parameterisedPlaceholder);

                // = character
                StringHelper.GetNextToken(ref parameterisedPlaceholder);
                string paramValue = StringHelper.GetNextToken(ref parameterisedPlaceholder);

                // cut away the quotes
                paramValue = paramValue.Substring(1, paramValue.Length - 2);
                line = StringHelper.AddCSV(line, paramId);
                line = StringHelper.AddCSV(line, paramValue);
            }

            result.Add(line);
            posPlaceholder = FTemplateCode.IndexOf("{#" + APlaceholder + " ", posEndPlaceholder);
            return posPlaceholder;
        }

        // this can be used to fill in code that just depends on one or two parameters
        // e.g. {#COLUMNCALC text="Gift Reference"} in the report xml file should be extended to a full column calculation
        // the extending is done by the calling function (may involve procedures like UpperCamelCase etc)
        // @returns list of occurances, each a comma separated list of first the original (for later replacement),
        //          then the parameter ids and parameter values
        public ArrayList GetParameterisedTemplates(string APlaceholder)
        {
            int posPlaceholder = FTemplateCode.IndexOf("{#" + APlaceholder + " ");
            ArrayList result = new ArrayList();

            while (posPlaceholder > -1)
            {
                posPlaceholder = GetNextParameterisedTemplate(APlaceholder, posPlaceholder, ref result);
            }

            return result;
        }

        public string GetFirstParameterisedTemplate(string APlaceholder)
        {
            int posPlaceholder = FTemplateCode.IndexOf("{#" + APlaceholder + " ");
            ArrayList result = new ArrayList();

            if (posPlaceholder > -1)
            {
                posPlaceholder = GetNextParameterisedTemplate(APlaceholder, posPlaceholder, ref result);
            }

            if (result.Count > 0)
            {
                return result[0].ToString();
            }

            return null;
        }

        private Int32 FTempCodeletCounter = 0;
        protected String FCodeletPostfix = "";
        public String GetNewTempCodelet()
        {
            FTempCodeletCounter++;
            return FTempCodeletCounter.ToString();
        }

        public void SetCodeLetPostfix(string APostfix)
        {
            FCodeletPostfix = APostfix;
        }

        // add code to existing code that will be replaced later
        public string AddToCodelet(string APlaceholder, string ACodelet)
        {
            if (!FCodelets.ContainsKey(APlaceholder + FCodeletPostfix))
            {
                FCodelets.Add(APlaceholder + FCodeletPostfix, "");
            }

            int index = FCodelets.IndexOfKey(APlaceholder + FCodeletPostfix);
            FCodelets.SetByIndex(index, FCodelets.GetByIndex(index) + ACodelet);
            return FCodelets.GetByIndex(index).ToString();
        }

        protected void ReplaceCodelets()
        {
            Boolean somethingWasReplaced = true;

            // needs to be done several times, because the keys are ordered by name, which does not help here
            // protect from endless loop: only continue if a replacement still has happened.
            while (somethingWasReplaced)
            {
                somethingWasReplaced = false;

                for (int index = 0; index < FCodelets.Count; index++)
                {
                    string s = FCodelets.GetByIndex(index).ToString();

                    if (s.Length == 0)
                    {
                        s = "BLANK";
                    }

                    if (DoReplacePlaceHolder(FCodelets.GetKey(index).ToString(), s))
                    {
                        somethingWasReplaced = true;
                    }
                }
            }
        }

        /// <summary>
        /// add replacement to the list of codelets
        /// </summary>
        /// <param name="APlaceholder"></param>
        /// <param name="AValue"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public void ReplacePlaceHolder(string APlaceholder, string AValue, string ADefault)
        {
            if ((AValue.Length == 0) && (ADefault.Length != 0))
            {
                AValue = ADefault;
            }

            if (FCodelets.ContainsKey(APlaceholder + FCodeletPostfix))
            {
                // we don't want to replace a placeholder several times.
                // the first time it is replaced, and it cannot be replaced again
                return;

                //FCodelets.Remove(APlaceholder+FCodeletPostfix);
            }

            AddToCodelet(APlaceholder, AValue);
        }

        public void ReplacePlaceHolder(string APlaceholder, string AValue)
        {
            ReplacePlaceHolder(APlaceholder, AValue, "");
        }

        // returns true if at least one occurance was replaced
        private Boolean DoReplacePlaceHolder(string APlaceholder, string AValue)
        {
            if (AValue.Length != 0)
            {
                if (AValue.CompareTo("BLANK") == 0)
                {
                    AValue = "";
                }

                FTemplateCode = FTemplateCode.Replace("{#IFDEF " + APlaceholder + "}", "");
                FTemplateCode = FTemplateCode.Replace("{#ENDIF " + APlaceholder + "}", "");

                // automatically indent to the same indentation as the placeholder
                int posPlaceholder = FTemplateCode.IndexOf("{#" + APlaceholder + "}");

                if (posPlaceholder < 1)
                {
                    return false; // place holder cannot be found, so no replacement necessary
                }

                int posNewline = FTemplateCode.LastIndexOf(Environment.NewLine, posPlaceholder);
                string whitespaces = "";

                if (posNewline != -1)
                {
                    whitespaces = FTemplateCode.Substring(posNewline, posPlaceholder - posNewline);
                }

                // only indent if the placeholder is at the beginning of a line
                if (whitespaces.Trim().Length == 0)
                {
                    AValue = AValue.Replace(Environment.NewLine, whitespaces);
                }

                // if there is a newline after the place holder, make sure that there is no 2 newlines in the end
                // remove the newline from the value if there is one at the end of value
                posNewline = FTemplateCode.IndexOf(Environment.NewLine, posPlaceholder);
                int LengthBrackets = 3; // {# }

                if (posNewline == posPlaceholder + LengthBrackets + APlaceholder.Length)
                {
                    AValue = AValue.TrimEnd();
                }

                FTemplateCode = FTemplateCode.Replace("{#" + APlaceholder + "}", AValue);
                return true;
            }

            return false;
        }

        public Boolean ReplaceRegion(string regionName, string content)
        {
            // todo: if there is no region, create one, in the right place? before the brackets close of the last class?
            FTemplateCode = Regex.Replace(
                FTemplateCode,
                "#region " + regionName + ".*" + "#endregion",
                "#region " + regionName + Environment.NewLine + content + "#endregion",
                RegexOptions.Singleline);
            return true;
        }

        public Boolean processTemplateParameters(XmlNode curNode)
        {
            // add all attributes as template parameters
            IEnumerator enumerator = curNode.Attributes.GetEnumerator();

            while (enumerator.MoveNext())
            {
                XmlAttribute current = (XmlAttribute)enumerator.Current;

                // some placeholders should be replaced after all other processing, e.g. Initialise_Ledger
                AddToCodelet(current.Name, current.Value);
            }

            // there might be some sequences (e.g. XMLFILES)
            curNode = curNode.FirstChild;

            while (curNode != null)
            {
                string nodeValue = StringHelper.StrMerge(TYml2Xml.GetElements(curNode), ",");

                // some placeholders should be replaced after all other processing, e.g. Initialise_Ledger
                AddToCodelet(curNode.Name, nodeValue);
                curNode = TXMLParser.NextNotBlank(curNode.NextSibling);
            }

            return true;
        }

        public string BeautifyCode(string s)
        {
            // remove spaces at end of line
            while (s.IndexOf(" " + Environment.NewLine) != -1)
            {
                s = s.Replace(" " + Environment.NewLine, Environment.NewLine);
            }

            // remove 2 or more empty lines and replace with just one
            while (s.IndexOf(Environment.NewLine + Environment.NewLine + Environment.NewLine) != -1)
            {
                s = s.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine + Environment.NewLine);
            }

            return s;
        }

        public Boolean FinishWriting(string AXAMLFilename, string ADestFileExtension, Boolean ACheckTemplateCompletion)
        {
            ReplaceCodelets();
            FTemplateCode = RemoveUndefinedIFDEFs(FTemplateCode);
            FTemplateCode = BeautifyCode(FTemplateCode);
            FDestinationFile = System.IO.Path.GetDirectoryName(AXAMLFilename) +
                               System.IO.Path.DirectorySeparatorChar +
                               System.IO.Path.GetFileNameWithoutExtension(AXAMLFilename) +
                               ADestFileExtension;

            if (!ACheckTemplateCompletion || CheckTemplateCompletion(FTemplateCode))
            {
                StreamWriter FWriter = File.CreateText(FDestinationFile + ".new");
                FWriter.Write(FTemplateCode);
                FWriter.Close();

                if (TTextFile.UpdateFile(FDestinationFile))
                {
                    Console.WriteLine("Writing " + FDestinationFile);
                }

                return true;
            }

            return false;
        }
    }
}