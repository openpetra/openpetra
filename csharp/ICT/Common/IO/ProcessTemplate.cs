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
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using Ict.Common;

namespace Ict.Common.IO
{
    /// <summary>
    /// this class helps with template scripts and other text files
    /// </summary>
    public class ProcessTemplate
    {
        /// <summary>
        /// the template
        /// </summary>
        public String FTemplateCode = "";

        /// <summary>
        /// the name of the file to write to
        /// </summary>
        public String FDestinationFile = "";

        /// <summary>
        /// temporary strings to store code into that will later each be inserted into a placeholder
        /// </summary>
        public SortedList FCodelets = new SortedList();

        /// <summary>
        /// snippets are smaller pieces of template code
        /// </summary>
        public SortedList FSnippets = new SortedList();

        /// <summary>
        /// constructor, open template from file
        /// </summary>
        /// <param name="AFullPath"></param>
        public ProcessTemplate(string AFullPath = null)
        {
            if ((AFullPath == null) || (AFullPath.Length == 0))
            {
                return;
            }

            if (!File.Exists(AFullPath))
            {
                throw new Exception("Cannot find template file " + AFullPath + "; please adjust the TemplateDir parameter");
            }

            StreamReader r;
            r = File.OpenText(AFullPath);
            FTemplateCode = string.Empty;

            while (!r.EndOfStream)
            {
                string line = r.ReadLine().TrimEnd(new char[] { '\r', '\t', ' ', '\n' }).Replace("\t", "    ");
                FTemplateCode += line + Environment.NewLine;
            }

            r.Close();

            // add other files, {#INCLUDE <filename>}
            while (FTemplateCode.Contains("{#INCLUDE "))
            {
                Int32 pos = FTemplateCode.IndexOf("{#INCLUDE ");
                Int32 newLinePos = FTemplateCode.IndexOf(Environment.NewLine, pos);
                string line = FTemplateCode.Substring(pos, newLinePos - pos);
                Int32 bracketClosePos = FTemplateCode.IndexOf("}", pos);
                string filename = FTemplateCode.Substring(pos + "{#INCLUDE ".Length, bracketClosePos - pos - "{#INCLUDE ".Length).Trim();

                // do this recursively, to get snippets and code in the right place, even from the include files
                ProcessTemplate includedTemplate = new ProcessTemplate(Path.GetDirectoryName(AFullPath) + Path.DirectorySeparatorChar + filename);

                FTemplateCode = FTemplateCode.Replace(line, includedTemplate.FTemplateCode);

                foreach (string key in includedTemplate.FSnippets.Keys)
                {
                    FSnippets.Add(key, includedTemplate.FSnippets[key]);
                }
            }

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

                    // remove all whitespaces from the end, but keep one line ending for ENDIF etc
                    snippetText = snippetText.TrimEnd(new char[] { '\n', '\r', ' ', '\t' }) + Environment.NewLine;
                    FSnippets.Add(snippetName, snippetText);
                }
            }

            // just make sure that there is a newline at the end, for ENDIF etc
            FTemplateCode += Environment.NewLine;
        }

        /// <summary>
        /// load the license and copyright text for the file header
        /// </summary>
        /// <param name="ATemplatePath"></param>
        /// <returns></returns>
        public static string LoadEmptyFileComment(string ATemplatePath)
        {
            StreamReader sr = new StreamReader(ATemplatePath + Path.DirectorySeparatorChar +
                "EmptyFileComment.txt");
            string fileheader = sr.ReadToEnd();

            sr.Close();
            fileheader = fileheader.Replace(">>>> Put your full name or just a shortname here <<<<", "auto generated");
            return fileheader;
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
            ASnippet.FTemplateCode = ASnippet.RemoveUndefinedIFDEFs(ASnippet.FTemplateCode);
            ASnippet.FTemplateCode = ASnippet.ActivateUndefinedIFNDEFs(ASnippet.FTemplateCode);

            if (FCodelets.ContainsKey(ACodeletName)
                && !((string)FCodelets[ACodeletName]).EndsWith(Environment.NewLine)
                && (((string)FCodelets[ACodeletName]).Length > 0))
            {
                AddToCodelet(ACodeletName, Environment.NewLine);
            }

            AddToCodelet(ACodeletName, ASnippet.FTemplateCode);
        }

        /// <summary>
        /// insert the snippet into the current template, into the given codelet.
        /// use separator to separate from previous items inserted into that codelet
        /// </summary>
        /// <param name="ACodeletName"></param>
        /// <param name="ASnippet"></param>
        /// <param name="ASeparator"></param>
        public void InsertSnippet(string ACodeletName, ProcessTemplate ASnippet, string ASeparator)
        {
            ASnippet.ReplaceCodelets();
            ASnippet.FTemplateCode = ASnippet.RemoveUndefinedIFDEFs(ASnippet.FTemplateCode);
            ASnippet.FTemplateCode = ASnippet.ActivateUndefinedIFNDEFs(ASnippet.FTemplateCode);

            if (FCodelets.ContainsKey(ACodeletName)
                && (((string)FCodelets[ACodeletName]).Length > 0))
            {
                AddSeparatorToCodelet(ACodeletName, ASeparator);
            }

            AddToCodelet(ACodeletName, ASnippet.FTemplateCode);
        }

        /// <summary>
        /// insert the snippet into the current template, into the given codelet.
        /// add new text in front of the text that has already been added to the codelet
        /// </summary>
        /// <param name="ACodeletName"></param>
        /// <param name="ASnippet"></param>
        public void InsertSnippetPrepend(string ACodeletName, ProcessTemplate ASnippet)
        {
            ASnippet.ReplaceCodelets();

            if (FCodelets.ContainsKey(ACodeletName))
            {
                AddToCodeletPrepend(ACodeletName, ASnippet.FTemplateCode + Environment.NewLine);
            }
            else
            {
                AddToCodeletPrepend(ACodeletName, ASnippet.FTemplateCode);
            }
        }

        /// check if all placeholders have been replaced in the template; ignore IFDEF
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

        /// <summary>
        /// remove all ifdefs that are not defined
        /// </summary>
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

                string before = s.Substring(0, posPlaceholder);
                string after = s.Substring(s.IndexOf(Environment.NewLine, posPlaceholderAfter) + Environment.NewLine.Length);

                s = before + after;

                posPlaceholder = s.IndexOf("{#IFDEF ");
            }

            return s;
        }

        /// <summary>
        /// remove all ifndefs that are defined
        /// </summary>
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

                string before = s.Substring(0, posPlaceholder);
                string after = s.Substring(s.IndexOf(Environment.NewLine, posPlaceholderAfter) + Environment.NewLine.Length);

                s = before + after;

                posPlaceholder = s.IndexOf("{#IFNDEF " + APlaceHolderName + "}");
            }

            return s;
        }

        /// <summary>
        /// activate all ifdefs that are defined
        /// </summary>
        public string ActivateDefinedIFDEF(string s, string APlaceholder)
        {
            s = s.Replace("{#IFDEF " + APlaceholder + "}" + Environment.NewLine, "");
            s = s.Replace("{#ENDIF " + APlaceholder + "}" + Environment.NewLine, "");
            return s;
        }

        /// <summary>
        /// activate all ifndefs that are not defined
        /// </summary>
        private string ActivateUndefinedIFNDEFs(string s)
        {
            int posPlaceholder = s.IndexOf("{#IFNDEF ");

            while (posPlaceholder > -1)
            {
                string name = s.Substring(posPlaceholder + 9, s.IndexOf("}", posPlaceholder) - posPlaceholder - 9);

                s = s.Replace("{#IFNDEF " + name + "}" + Environment.NewLine, "");
                s = s.Replace("{#ENDIFN " + name + "}" + Environment.NewLine, "");

                posPlaceholder = s.IndexOf("{#IFNDEF ");
            }

            return s;
        }

        /// <summary>
        /// this helps to distinguish codelets when nesting codelets
        /// </summary>
        protected String FCodeletPostfix = "";

        /// <summary>
        /// set the postfix for codelets. this helps to distinguish codelets when nesting codelets
        /// </summary>
        /// <param name="APostfix"></param>
        public void SetCodeLetPostfix(string APostfix)
        {
            FCodeletPostfix = APostfix;
        }

        /// add code to existing code that will be replaced later
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

        /// add separator to codelet at the end of the previous row
        public string AddSeparatorToCodelet(string APlaceholder, string ASeparator)
        {
            if (!FCodelets.ContainsKey(APlaceholder + FCodeletPostfix))
            {
                return String.Empty;
            }

            int index = FCodelets.IndexOfKey(APlaceholder + FCodeletPostfix);
            string Codelet = FCodelets.GetByIndex(index).ToString();

            if (Codelet.EndsWith(Environment.NewLine))
            {
                Codelet = Codelet.Substring(0, Codelet.Length - Environment.NewLine.Length) + ASeparator + Environment.NewLine;
            }
            else
            {
                Codelet += ASeparator;
            }

            FCodelets.SetByIndex(index, Codelet);
            return Codelet;
        }

        /// add code to existing code that will be replaced later.
        /// the new code is added before the existing code.
        /// this overload allows duplicates to be added
        public string AddToCodeletPrepend(string APlaceholder, string ACodelet)
        {
            return AddToCodeletPrepend(APlaceholder, ACodelet, true);
        }

        /// add code to existing code that will be replaced later.
        /// the new code is added before the existing code
        public string AddToCodeletPrepend(string APlaceholder, string ACodelet, bool AAllowDuplicates)
        {
            if (!FCodelets.ContainsKey(APlaceholder + FCodeletPostfix))
            {
                FCodelets.Add(APlaceholder + FCodeletPostfix, "");
            }

            int index = FCodelets.IndexOfKey(APlaceholder + FCodeletPostfix);
            string previousValue = FCodelets.GetByIndex(index).ToString();

            if (AAllowDuplicates || !previousValue.Contains(ACodelet))
            {
                FCodelets.SetByIndex(index, ACodelet + previousValue);
            }

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

        /// create a new codelet, overwrites existing one
        public string SetCodelet(string APlaceholder, string ACodelet)
        {
            if (ACodelet == null)
            {
                ACodelet = "";
            }

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

            if (countDescrLines == 0)
            {
                AddToCodelet(APlaceholder, "///");
            }
        }

        /// <summary>
        /// insert the codelets into the template that have been collected until now
        /// </summary>
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

        /// <summary>
        /// replace a placeholder with an actual value.
        /// overload with empty default.
        /// </summary>
        /// <param name="APlaceholder"></param>
        /// <param name="AValue"></param>
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

                if (AValue.Contains("{#" + APlaceholder + "}"))
                {
                    throw new Exception("DoReplacePlaceHolder() Problem: Placeholder recursion, " + APlaceholder);
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

                    if (AValue.Length > 0)
                    {
                        // indent the value by the given whitespaces
                        string whitespaces = FTemplateCode.Substring(posNewline, posPlaceholder - posNewline);
                        AValue = whitespaces + AValue.Replace(Environment.NewLine, whitespaces).TrimEnd(new char[] { '\n', '\r', ' ', '\t' });

                        // no trailing spaces for IFDEF and IFNDEF and ENDIFN and ENDIF
                        AValue = AValue.Replace(whitespaces + "{#IFDEF ", Environment.NewLine + "{#IFDEF ");
                        AValue = AValue.Replace(whitespaces + "{#IFNDEF ", Environment.NewLine + "{#IFNDEF ");
                        AValue = AValue.Replace(whitespaces + "{#ENDIF ", Environment.NewLine + "{#ENDIF ");
                        AValue = AValue.Replace(whitespaces + "{#ENDIFN ", Environment.NewLine + "{#ENDIFN ");
                    }

                    FTemplateCode = before + AValue + after;
                }
                else
                {
                    // replace just the placeholder

                    // insert indenting whitespaces if AValue has several lines
                    if (AValue.Contains(Environment.NewLine))
                    {
                        int posNewline = FTemplateCode.Substring(0, posPlaceholder).LastIndexOf(Environment.NewLine);

                        if (posNewline >= 0)
                        {
                            int countWhitespaces =
                                FTemplateCode.Substring(posNewline, posPlaceholder - posNewline).Replace("\t",
                                    "    ").Length - Environment.NewLine.Length;
                            string Whitespaces = Environment.NewLine + "".PadLeft(countWhitespaces);
                            AValue = AValue.Replace(Environment.NewLine, Whitespaces);
                        }
                    }

                    FTemplateCode = FTemplateCode.Replace("{#" + APlaceholder + "}", AValue);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// replace a region with new content
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
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

        /// <summary>
        /// insert parameters into placeholders
        /// </summary>
        /// <param name="curNode"></param>
        /// <returns></returns>
        public Boolean processTemplateParameters(XmlNode curNode)
        {
            // add all attributes as template parameters
            SortedList <string, string>attrList = TYml2Xml.GetAttributes(curNode);

            foreach (string key in attrList.Keys)
            {
                // some placeholders should be replaced after all other processing, e.g. Initialise_Ledger
                AddToCodelet(key, attrList[key]);
            }

            // there might be some sequences (e.g. XMLFILES)
            List <XmlNode>children = TYml2Xml.GetChildren(curNode, true);

            foreach (XmlNode child in children)
            {
                string nodeValue = StringHelper.StrMerge(TYml2Xml.GetElements(child), ",");

                // some placeholders should be replaced after all other processing, e.g. Initialise_Ledger
                AddToCodelet(child.Name, nodeValue);
            }

            return true;
        }

        /// <summary>
        /// clean up the code, remove spaces, too many empty lines
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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

        /// <summary>
        /// write the content to a file.
        /// deal with conditional defines etc.
        /// </summary>
        /// <param name="AXAMLFilename"></param>
        /// <param name="ADestFileExtension"></param>
        /// <param name="ACheckTemplateCompletion"></param>
        /// <returns></returns>
        public Boolean FinishWriting(string AXAMLFilename, string ADestFileExtension, Boolean ACheckTemplateCompletion)
        {
            ReplaceCodelets();
            FTemplateCode = RemoveUndefinedIFDEFs(FTemplateCode);
            FTemplateCode = ActivateUndefinedIFNDEFs(FTemplateCode);
            FTemplateCode = BeautifyCode(FTemplateCode);

            // just one line break at the end
            FTemplateCode = FTemplateCode.TrimEnd(new char[] { ' ', '\t', '\r', '\n' });

            FDestinationFile = System.IO.Path.GetDirectoryName(AXAMLFilename) +
                               System.IO.Path.DirectorySeparatorChar +
                               System.IO.Path.GetFileNameWithoutExtension(AXAMLFilename) +
                               ADestFileExtension;

            if (!ACheckTemplateCompletion || CheckTemplateCompletion(FTemplateCode))
            {
                if (TFileDiffMerge.Merge2Files(FDestinationFile, FTemplateCode.Replace("\r", "").Split(new char[] { '\n' })))
                {
                    Console.WriteLine("Writing " + Path.GetFileName(FDestinationFile));
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// return the snippet as a string, instead of writing to file
        /// </summary>
        public string FinishWriting(Boolean ACheckTemplateCompletion)
        {
            ReplaceCodelets();
            FTemplateCode = RemoveUndefinedIFDEFs(FTemplateCode);
            FTemplateCode = ActivateUndefinedIFNDEFs(FTemplateCode);
            FTemplateCode = BeautifyCode(FTemplateCode);

            // just one line break at the end
            FTemplateCode = FTemplateCode.TrimEnd(new char[] { ' ', '\t', '\r', '\n' }) + Environment.NewLine;

            if (!ACheckTemplateCompletion || CheckTemplateCompletion(FTemplateCode))
            {
                return FTemplateCode;
            }

            // an exception is thrown anyways by CheckTemplateCompletion
            return "";
        }
    }
}