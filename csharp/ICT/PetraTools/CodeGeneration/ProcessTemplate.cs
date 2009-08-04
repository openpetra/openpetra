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
using System.Collections.Specialized;
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
        public SortedList FSnippets = new SortedList();

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

            // split off snippets (identified by "{##")
            if (FTemplateCode.Contains("{##"))
            {
                StringCollection snippets = StringHelper.StrSplit(FTemplateCode, "{##");

                // first part is the actual template code
                FTemplateCode = snippets[0];

                for (int counter = 1; counter < snippets.Count; counter++)
                {
                    string snippetName = snippets[counter].Substring(0, snippets[counter].IndexOf("}"));

                    // exclude first newline
                    string snippetText = snippets[counter].Substring(snippets[counter].IndexOf(Environment.NewLine) + Environment.NewLine.Length);

                    // remove all whitespaces from the end
                    snippetText = snippetText.TrimEnd(new char[] { '\n', '\r', ' ', '\t' });
                    FSnippets.Add(snippetName, snippetText);
                }
            }

            r.Close();
        }

        /// <summary>
        /// add snippets from another template file
        /// (eg. for writing datasets, we want to reuse the table template for custom tables)
        /// </summary>
        /// <param name="AFilePath"></param>
        public void AddSnippetsFromOtherFile(string AFilePath)
        {
            ProcessTemplate temp = new ProcessTemplate(AFilePath);

            foreach (string key in temp.FSnippets.Keys)
            {
                // don't overwrite own snippets with same name
                if (!this.FSnippets.ContainsKey(key))
                {
                    this.FSnippets.Add(key, (string)temp.FSnippets[key]);
                }
            }
        }

        /// <summary>
        /// get the specified snippet, in a new Template
        /// </summary>
        /// <param name="ASnippetName"></param>
        /// <returns></returns>
        public ProcessTemplate GetSnippet(string ASnippetName)
        {
            ProcessTemplate snippetTemplate = new ProcessTemplate();

            snippetTemplate.FTemplateCode = (string)FSnippets[ASnippetName];

            if (snippetTemplate.FTemplateCode == null)
            {
                throw new Exception("cannot find snippet with name " + ASnippetName);
            }

            snippetTemplate.FSnippets = this.FSnippets;
            return snippetTemplate;
        }

        /// <summary>
        /// insert the snippet into the current template, into the given codelet
        /// </summary>
        /// <param name="ACodeletName"></param>
        /// <param name="ASnippet"></param>
        public void InsertSnippet(string ACodeletName, ProcessTemplate ASnippet)
        {
            ASnippet.ReplaceCodelets();

            if (FCodelets.ContainsKey(ACodeletName))
            {
                AddToCodelet(ACodeletName, Environment.NewLine);
            }

            AddToCodelet(ACodeletName, ASnippet.FTemplateCode);
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

                string before = s.Substring(0, s.LastIndexOf(Environment.NewLine, posPlaceholder + 1));
                string after = s.Substring(s.IndexOf(Environment.NewLine, posPlaceholderAfter));

                s = before + after;

                posPlaceholder = s.IndexOf("{#IFDEF ");
            }

            return s;
        }

        public string RemoveDefinedIFNDEF(string s, string APlaceHolderName)
        {
            int posPlaceholder = s.IndexOf("{#IFNDEF " + APlaceHolderName + "}");

            while (posPlaceholder > -1)
            {
                int posPlaceholderAfter = s.IndexOf("{#ENDIFN " + APlaceHolderName + "}");

                if (posPlaceholderAfter == -1)
                {
                    Console.WriteLine("problem in area: " + Environment.NewLine +
                        s.Substring(posPlaceholder - 200, 500));
                    throw new Exception("The template has a bug. " +
                        Environment.NewLine + "We are missing the ENDIFN for: " + APlaceHolderName);
                }

                string before = s.Substring(0, s.LastIndexOf(Environment.NewLine, posPlaceholder + 1));
                string after = s.Substring(s.IndexOf(Environment.NewLine, posPlaceholderAfter));

                s = before + after;

                posPlaceholder = s.IndexOf("{#IFNDEF " + APlaceHolderName + "}");
            }

            return s;
        }

        public string ActivateDefinedIFDEF(string s, string APlaceholder)
        {
            s = s.Replace("{#IFDEF " + APlaceholder + "}" + Environment.NewLine, "");
            s = s.Replace("{#IFDEF " + APlaceholder + "}", "");
            s = s.Replace("{#ENDIF " + APlaceholder + "}" + Environment.NewLine, "");
            s = s.Replace("{#ENDIF " + APlaceholder + "}", "");
            return s;
        }

        public string ActivateUndefinedIFNDEFs(string s)
        {
            int posPlaceholder = s.IndexOf("{#IFNDEF ");

            while (posPlaceholder > -1)
            {
                string name = s.Substring(posPlaceholder + 9, s.IndexOf("}", posPlaceholder) - posPlaceholder - 9);

                s = s.Replace("{#IFNDEF " + name + "}" + Environment.NewLine, "");
                s = s.Replace("{#IFNDEF " + name + "}", "");
                s = s.Replace("{#ENDIFN " + name + "}" + Environment.NewLine, "");
                s = s.Replace("{#ENDIFN " + name + "}", "");

                posPlaceholder = s.IndexOf("{#IFNDEF ");
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

        /// <summary>
        /// add code to existing code that will be replaced later
        /// </summary>
        /// <param name="APlaceholder"></param>
        /// <param name="ACodelet"></param>
        /// <param name="AAddDuplicates"></param>
        /// <returns></returns>
        public string AddToCodelet(string APlaceholder, string ACodelet, bool AAddDuplicates)
        {
            if (!FCodelets.ContainsKey(APlaceholder + FCodeletPostfix))
            {
                FCodelets.Add(APlaceholder + FCodeletPostfix, "");
            }

            int index = FCodelets.IndexOfKey(APlaceholder + FCodeletPostfix);

            if (!AAddDuplicates && FCodelets.GetByIndex(index).ToString().Contains(ACodelet))
            {
                return FCodelets.GetByIndex(index).ToString();
            }

            FCodelets.SetByIndex(index, FCodelets.GetByIndex(index) + ACodelet);
            return FCodelets.GetByIndex(index).ToString();
        }

        // create a new codelet, overwrites existing one
        public string SetCodelet(string APlaceholder, string ACodelet)
        {
            if (!FCodelets.ContainsKey(APlaceholder + FCodeletPostfix))
            {
                FCodelets.Add(APlaceholder + FCodeletPostfix, "");
            }

            int index = FCodelets.IndexOfKey(APlaceholder + FCodeletPostfix);
            FCodelets.SetByIndex(index, ACodelet);
            return FCodelets.GetByIndex(index).ToString();
        }

        /// special way of splitting a multiline comment into several lines with comment slashes
        public void SetCodeletComment(string APlaceholder, string AMultiLineComment)
        {
            StringCollection descrLines = StringHelper.StrSplit(AMultiLineComment, Environment.NewLine);

            SetCodelet(APlaceholder, "");
            int countDescrLines = 0;

            foreach (string line in descrLines)
            {
                AddToCodelet(APlaceholder, "/// " + line.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;"));
                countDescrLines++;

                if (countDescrLines != descrLines.Count)
                {
                    AddToCodelet(APlaceholder, Environment.NewLine);
                }
            }
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

                if (AValue.Length > 0)
                {
                    FTemplateCode = ActivateDefinedIFDEF(FTemplateCode, APlaceholder);
                    FTemplateCode = RemoveDefinedIFNDEF(FTemplateCode, APlaceholder);
                }

                // automatically indent to the same indentation as the placeholder
                int posPlaceholder = FTemplateCode.IndexOf("{#" + APlaceholder + "}");

                if (posPlaceholder < 0)
                {
                    return false; // place holder cannot be found, so no replacement necessary
                }

                string placeHolderLine = FTemplateCode.Substring(FTemplateCode.LastIndexOf(Environment.NewLine,
                        posPlaceholder + 1) + Environment.NewLine.Length + 1);

                if (placeHolderLine.IndexOf(Environment.NewLine) != -1)
                {
                    placeHolderLine = placeHolderLine.Substring(0, placeHolderLine.IndexOf(Environment.NewLine));
                }

                if (placeHolderLine.Trim() == "{#" + APlaceholder + "}")
                {
                    // replace the whole line
                    int posNewline = FTemplateCode.Substring(0, posPlaceholder).LastIndexOf(Environment.NewLine);
                    string before = FTemplateCode.Substring(0, posNewline);
                    string after = FTemplateCode.Substring(FTemplateCode.IndexOf(Environment.NewLine, posPlaceholder + 1));

                    // indent the value by the given whitespaces
                    string whitespaces = FTemplateCode.Substring(posNewline, posPlaceholder - posNewline);
                    AValue = whitespaces + AValue.Replace(Environment.NewLine, whitespaces).TrimEnd(new char[] { '\n', '\r', ' ', '\t' });

                    // for debugging
//                    if (false && APlaceholder == "ODBCPARAMETERSFOREIGNKEY")
//                    {
//                        FTemplateCode = before + ">" + AValue + "<" + after;
//                        Console.WriteLine(FTemplateCode.Substring(before.Length - 20, AValue.Length + 60));
//                        Environment.Exit(-2);
//                    }

                    FTemplateCode = before + AValue + after;
                }
                else
                {
                    // replace just the placeholder
                    FTemplateCode = FTemplateCode.Replace("{#" + APlaceholder + "}", AValue);
                }

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
            FTemplateCode = ActivateUndefinedIFNDEFs(FTemplateCode);
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