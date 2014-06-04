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
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
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
        public StringBuilder FTemplateCode = new StringBuilder();

        /// <summary>
        /// the name of the file to write to
        /// </summary>
        public String FDestinationFile = "";

        /// <summary>
        /// temporary strings to store code into that will later each be inserted into a placeholder
        /// </summary>
        public SortedList <string, StringBuilder>FCodelets = new SortedList <string, StringBuilder>();

        /// <summary>
        /// snippets are smaller pieces of template code
        /// </summary>
        public SortedList <string, string>FSnippets = new SortedList <string, string>();

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
            FTemplateCode = new StringBuilder();

            while (!r.EndOfStream)
            {
                string line = r.ReadLine().TrimEnd(new char[] { '\r', '\t', ' ', '\n' }).Replace("\t", "    ");
                FTemplateCode.Append(line).Append(Environment.NewLine);
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

                FTemplateCode = FTemplateCode.Replace(line, includedTemplate.FTemplateCode.ToString());

                foreach (string key in includedTemplate.FSnippets.Keys)
                {
                    FSnippets.Add(key, includedTemplate.FSnippets[key]);
                }
            }

            // split off snippets (identified by "{##")
            if (FTemplateCode.Contains("{##"))
            {
                StringCollection snippets = StringHelper.StrSplit(FTemplateCode.ToString(), "{##");

                // first part is the actual template code
                FTemplateCode = new StringBuilder(snippets[0]);

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
            FTemplateCode.Append(Environment.NewLine);
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

            snippetTemplate.FTemplateCode = new StringBuilder(FSnippets[ASnippetName]);

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
            ASnippet.ProcessIFDEFs(ref ASnippet.FTemplateCode);
            ASnippet.ProcessIFNDEFs(ref ASnippet.FTemplateCode);

            if (FCodelets.ContainsKey(ACodeletName)
                && !FCodelets[ACodeletName].ToString().EndsWith(Environment.NewLine)
                && (FCodelets[ACodeletName].Length > 0))
            {
                AddToCodelet(ACodeletName, Environment.NewLine);
            }

            AddToCodelet(ACodeletName, ASnippet.FTemplateCode.ToString());
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
            ASnippet.ProcessIFDEFs(ref ASnippet.FTemplateCode);
            ASnippet.ProcessIFNDEFs(ref ASnippet.FTemplateCode);

            if (FCodelets.ContainsKey(ACodeletName)
                && (FCodelets[ACodeletName].Length > 0))
            {
                AddSeparatorToCodelet(ACodeletName, ASeparator);
            }

            AddToCodelet(ACodeletName, ASnippet.FTemplateCode.ToString());
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
                AddToCodeletPrepend(ACodeletName, ASnippet.FTemplateCode.ToString() + Environment.NewLine);
            }
            else
            {
                AddToCodeletPrepend(ACodeletName, ASnippet.FTemplateCode.ToString());
            }
        }

        /// check if all placeholders have been replaced in the template; ignore IFDEF
        public Boolean CheckTemplateCompletion(StringBuilder s)
        {
            int posPlaceholder = s.IndexOf("{#");
            string remainingTemplatePlaceholders = "";

            while (posPlaceholder > -1)
            {
                string latestPlaceholder = s.Substring(posPlaceholder + 2, s.IndexOf("}", posPlaceholder) - posPlaceholder - 2);
                remainingTemplatePlaceholders +=
                    latestPlaceholder + Environment.NewLine;
                posPlaceholder = s.IndexOf("{#", posPlaceholder + 2);
            }

            if (remainingTemplatePlaceholders.Length > 0)
            {
                if (FDestinationFile.Length > 0)
                {
                    StreamWriter FWriter;
                    FWriter = File.CreateText(FDestinationFile + ".error");
                    FWriter.Write(s.ToString());
                    FWriter.Close();

                    throw new Exception("The template has not completely been filled in. " +
                        Environment.NewLine + "You are missing: " + Environment.NewLine +
                        remainingTemplatePlaceholders + Environment.NewLine +
                        "Check file " + FDestinationFile + ".error");
                }
                else
                {
                    TLogging.Log("Failure in this code: ");
                    TLogging.Log(s.ToString());

                    throw new Exception("The template has not completely been filled in. " +
                        Environment.NewLine + "You are missing: " + Environment.NewLine +
                        remainingTemplatePlaceholders);
                }
            }

            return true;
        }

        /// <summary>
        /// remove all ifndefs that are defined
        /// </summary>
        private void RemoveDefinedIFNDEF(ref StringBuilder sb, List <string>APlaceHolderName)
        {
            string s = sb.ToString();
            int posPlaceholder = s.IndexOf("{#IFNDEF ");

            while (posPlaceholder > -1)
            {
                string name = s.Substring(posPlaceholder + 9, s.IndexOf("}", posPlaceholder) - posPlaceholder - 9);
                int posPlaceholderAfter = s.IndexOf("{#ENDIFN " + name + "}", posPlaceholder);

                if (posPlaceholderAfter == -1)
                {
                    Console.WriteLine("problem in area: " + Environment.NewLine +
                        s.Substring(posPlaceholder - 200, 500));
                    throw new Exception("The template has a bug. " +
                        Environment.NewLine + "We are missing the ENDIFN for: " + name);
                }

                StringBuilder newName = new StringBuilder(name);

                foreach (string placeholder in APlaceHolderName)
                {
                    newName.Replace(placeholder, "TRUE");
                }

                sb.Replace("{#IFNDEF " + name + "}",
                    "{#IFNDEF " + newName.ToString() + "}");
                sb.Replace("{#ENDIFN " + name + "}",
                    "{#ENDIFN " + newName.ToString() + "}");

                s = sb.ToString();
                posPlaceholder = s.IndexOf("{#IFNDEF ", posPlaceholder + 1);
            }
        }

        /// <summary>
        /// activate all ifdefs that are defined
        /// </summary>
        private void ActivateDefinedIFDEF(ref StringBuilder sb, List <string>APlaceholder)
        {
            // get all ifdefs, and replace the APlaceHolder with TRUE
            string s = sb.ToString();

            int posPlaceholder = s.IndexOf("{#IFDEF ");

            while (posPlaceholder > -1)
            {
                string name = s.Substring(posPlaceholder + 8, s.IndexOf("}", posPlaceholder) - posPlaceholder - 8);

                int posPlaceholderAfter = s.IndexOf("{#ENDIF " + name + "}", posPlaceholder);

                if (posPlaceholderAfter == -1)
                {
                    throw new Exception("The template has a bug. " +
                        Environment.NewLine + "We are missing the ENDIF for: " + name);
                }

                StringBuilder newName = new StringBuilder(name);

                foreach (string placeholder in APlaceholder)
                {
                    newName.Replace(placeholder, "TRUE");
                }

                sb.Replace("{#IFDEF " + name + "}",
                    "{#IFDEF " + newName + "}");
                sb.Replace("{#ENDIF " + name + "}",
                    "{#ENDIF " + newName + "}");

                s = sb.ToString();
                posPlaceholder = s.IndexOf("{#IFDEF ", posPlaceholder + 1);
            }
        }

        private int FindMatchingEndTag(string s, int posStartTag, string AStartTag, string AEndTag)
        {
            int pos = posStartTag + 1;
            int counterStartTag = 1;

            while (pos < s.Length)
            {
                int startPos = s.IndexOf(AStartTag, pos);
                int endPos = s.IndexOf(AEndTag, pos);

                if ((startPos != -1) && (startPos < endPos))
                {
                    counterStartTag++;
                    pos = startPos + 1;
                }
                else if (endPos != -1)
                {
                    counterStartTag--;

                    if (counterStartTag == 0)
                    {
                        return endPos;
                    }

                    pos = endPos + 1;
                }
                else
                {
                    return -1;
                }
            }

            // cannot find
            return -1;
        }

        /// <summary>
        /// remove all ifdefs that are not defined, or activate them
        /// </summary>
        public void ProcessIFDEFs(ref StringBuilder sb)
        {
            string s = sb.ToString();
            int posPlaceholder = s.IndexOf("{#IFDEF ");

            while (posPlaceholder > -1)
            {
                string name = s.Substring(posPlaceholder + 8, s.IndexOf("}", posPlaceholder) - posPlaceholder - 8);

                // find the matching closing ENDIF
                int posPlaceholderAfter = FindMatchingEndTag(s.ToString(), posPlaceholder, "{#IFDEF " + name + "}", "{#ENDIF " + name + "}");

                if (posPlaceholderAfter == -1)
                {
                    throw new Exception("The template has a bug. " +
                        Environment.NewLine + "We are missing the ENDIF for: " + name);
                }

                sb.Remove(posPlaceholder, 8 + name.Length + 1 + Environment.NewLine.Length);
                posPlaceholderAfter -= 8 + name.Length + 1 + Environment.NewLine.Length;
                int LengthLineBreakAfter = 0;

                if (posPlaceholderAfter + 8 + name.Length + 1 + Environment.NewLine.Length < sb.Length)
                {
                    LengthLineBreakAfter = Environment.NewLine.Length;
                }

                sb.Remove(posPlaceholderAfter, 8 + name.Length + 1 + LengthLineBreakAfter);

                if (!((name == "TRUE") || (name == "TRUE AND TRUE") || name.StartsWith("TRUE OR") || name.EndsWith("OR TRUE")))
                {
                    sb.Remove(posPlaceholder, posPlaceholderAfter - posPlaceholder);
                }

                s = sb.ToString();

                posPlaceholder = s.IndexOf("{#IFDEF ");
            }
        }

        /// <summary>
        /// activate all ifndefs that are not defined
        /// </summary>
        private void ProcessIFNDEFs(ref StringBuilder sb)
        {
            string s = sb.ToString();
            int posPlaceholder = s.IndexOf("{#IFNDEF ");

            while (posPlaceholder > -1)
            {
                string name = s.Substring(posPlaceholder + 9, s.IndexOf("}", posPlaceholder) - posPlaceholder - 9);

                // find the matching closing ENDIFN
                int posPlaceholderAfter = FindMatchingEndTag(s.ToString(), posPlaceholder, "{#IFNDEF " + name + "}", "{#ENDIFN " + name + "}");

                if (posPlaceholderAfter == -1)
                {
                    throw new Exception("The template has a bug. " +
                        Environment.NewLine + "We are missing the ENDIFN for: " + name);
                }

                sb.Remove(posPlaceholder, 9 + name.Length + 1 + Environment.NewLine.Length);
                posPlaceholderAfter -= 9 + name.Length + 1 + Environment.NewLine.Length;
                int LengthLineBreakAfter = 0;

                if (posPlaceholderAfter + 9 + name.Length + 1 + Environment.NewLine.Length < sb.Length)
                {
                    LengthLineBreakAfter = Environment.NewLine.Length;
                }

                sb.Remove(posPlaceholderAfter, 9 + name.Length + 1 + LengthLineBreakAfter);

                if ((name == "TRUE") || (name == "TRUE AND TRUE") || name.StartsWith("TRUE OR") || name.EndsWith("OR TRUE"))
                {
                    sb.Remove(posPlaceholder, posPlaceholderAfter - posPlaceholder);
                }

                s = sb.ToString();

                posPlaceholder = s.IndexOf("{#IFNDEF ");
            }
        }

        /// <summary>
        /// this helps to distinguish codelets when nesting codelets
        /// </summary>
        public String FCodeletPostfix = "";

        /// <summary>
        /// set the postfix for codelets. this helps to distinguish codelets when nesting codelets
        /// </summary>
        /// <param name="APostfix"></param>
        public void SetCodeLetPostfix(string APostfix)
        {
            FCodeletPostfix = APostfix;
        }

        /// add code to existing code that will be replaced later
        public void AddToCodelet(string APlaceholder, string ACodelet)
        {
            if (!FCodelets.ContainsKey(APlaceholder + FCodeletPostfix))
            {
                FCodelets.Add(APlaceholder + FCodeletPostfix, new StringBuilder());
            }

            FCodelets[APlaceholder + FCodeletPostfix].Append(ACodelet);
        }

        /// add separator to codelet at the end of the previous row
        public string AddSeparatorToCodelet(string APlaceholder, string ASeparator)
        {
            if (!FCodelets.ContainsKey(APlaceholder + FCodeletPostfix))
            {
                return String.Empty;
            }

            string Codelet = FCodelets[APlaceholder + FCodeletPostfix].ToString();

            if (Codelet.Trim().EndsWith(";"))
            {
                // we do not need a separator for such situations
                ASeparator = string.Empty;
            }

            if (Codelet.EndsWith(Environment.NewLine))
            {
                Codelet = Codelet.Substring(0, Codelet.Length - Environment.NewLine.Length) + ASeparator + Environment.NewLine;
            }
            else
            {
                Codelet += ASeparator;
            }

            FCodelets[APlaceholder + FCodeletPostfix] = new StringBuilder(Codelet);
            return Codelet;
        }

        /// add code to existing code that will be replaced later.
        /// the new code is added before the existing code.
        /// this overload allows duplicates to be added
        public void AddToCodeletPrepend(string APlaceholder, string ACodelet)
        {
            AddToCodeletPrepend(APlaceholder, ACodelet, true);
        }

        /// add code to existing code that will be replaced later.
        /// the new code is added before the existing code
        public void AddToCodeletPrepend(string APlaceholder, string ACodelet, bool AAllowDuplicates)
        {
            if (!FCodelets.ContainsKey(APlaceholder + FCodeletPostfix))
            {
                FCodelets.Add(APlaceholder + FCodeletPostfix, new StringBuilder());
            }

            string previousValue = FCodelets[APlaceholder + FCodeletPostfix].ToString();

            if (AAllowDuplicates || !previousValue.Contains(ACodelet))
            {
                FCodelets[APlaceholder + FCodeletPostfix] = new StringBuilder(ACodelet);
                FCodelets[APlaceholder + FCodeletPostfix].Append(previousValue);
            }
        }

        /// <summary>
        /// add code to existing code that will be replaced later
        /// </summary>
        /// <param name="APlaceholder"></param>
        /// <param name="ACodelet"></param>
        /// <param name="AAddDuplicates"></param>
        /// <returns></returns>
        public void AddToCodelet(string APlaceholder, string ACodelet, bool AAddDuplicates)
        {
            if (!FCodelets.ContainsKey(APlaceholder + FCodeletPostfix))
            {
                FCodelets.Add(APlaceholder + FCodeletPostfix, new StringBuilder());
            }

            if (!AAddDuplicates && FCodelets[APlaceholder + FCodeletPostfix].ToString().Contains(ACodelet))
            {
                return;
            }

            FCodelets[APlaceholder + FCodeletPostfix].Append(ACodelet);
        }

        /// create a new codelet, overwrites existing one
        public void SetCodelet(string APlaceholder, string ACodelet)
        {
            if (ACodelet == null)
            {
                ACodelet = string.Empty;
            }

            if (!FCodelets.ContainsKey(APlaceholder + FCodeletPostfix))
            {
                FCodelets.Add(APlaceholder + FCodeletPostfix, new StringBuilder(ACodelet));
            }
            else
            {
                FCodelets[APlaceholder + FCodeletPostfix] = new StringBuilder(ACodelet);
            }
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

            List <string>Placeholders = new List <string>();

            foreach (string key in FCodelets.Keys)
            {
                if (FCodelets[key].Length > 0)
                {
                    Placeholders.Add(key);
                }
            }

            // needs to be done several times, because the keys are ordered by name, which does not help here
            // protect from endless loop: only continue if a replacement still has happened.
            while (somethingWasReplaced)
            {
                somethingWasReplaced = false;

                ActivateDefinedIFDEF(ref FTemplateCode, Placeholders);
                RemoveDefinedIFNDEF(ref FTemplateCode, Placeholders);

                for (int index = 0; index < FCodelets.Count; index++)
                {
                    string s = FCodelets.Values[index].ToString();

                    if (s.Length == 0)
                    {
                        s = "BLANK";
                    }

                    if (DoReplacePlaceHolder(FCodelets.Keys[index], s))
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

                // automatically indent to the same indentation as the placeholder
                int posPlaceholder = FTemplateCode.IndexOf("{#" + APlaceholder + "}");

                if (posPlaceholder < 0)
                {
                    return false; // place holder cannot be found, so no replacement necessary
                }

                string placeHolderLine = FTemplateCode.Substring(FTemplateCode.ToString().LastIndexOf(Environment.NewLine,
                        posPlaceholder + 1) + Environment.NewLine.Length + 1);

                if (placeHolderLine.IndexOf(Environment.NewLine) != -1)
                {
                    placeHolderLine = placeHolderLine.Substring(0, placeHolderLine.IndexOf(Environment.NewLine));
                }

                if (placeHolderLine.Trim() == "{#" + APlaceholder + "}")
                {
                    // replace the whole line
                    int posNewline = FTemplateCode.Substring(0, posPlaceholder).LastIndexOf(Environment.NewLine);

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

                    FTemplateCode.Remove(posNewline, FTemplateCode.IndexOf(Environment.NewLine, posPlaceholder + 1) - posNewline);

                    FTemplateCode.Insert(posNewline, AValue);
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
            FTemplateCode = new StringBuilder(Regex.Replace(
                    FTemplateCode.ToString(),
                    "#region " + regionName + ".*" + "#endregion",
                    "#region " + regionName + Environment.NewLine + content + "#endregion",
                    RegexOptions.Singleline));
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
                string nodeValue = StringHelper.StrMerge(TYml2Xml.GetElements(child), ',');

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
        public void BeautifyCode(ref StringBuilder s)
        {
            // remove spaces at end of line
            while (s.IndexOf(" " + Environment.NewLine) != -1)
            {
                s.Replace(" " + Environment.NewLine, Environment.NewLine);
            }

            // remove 2 or more empty lines and replace with just one
            while (s.IndexOf(Environment.NewLine + Environment.NewLine + Environment.NewLine) != -1)
            {
                s.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine + Environment.NewLine);
            }
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
//            TLogging.Log("1");
            ReplaceCodelets();
//            TLogging.Log("2");
            ProcessIFDEFs(ref FTemplateCode);
            ProcessIFNDEFs(ref FTemplateCode);
            BeautifyCode(ref FTemplateCode);

            AXAMLFilename = AXAMLFilename.Replace('\\', Path.DirectorySeparatorChar);
            AXAMLFilename = AXAMLFilename.Replace('/', Path.DirectorySeparatorChar);

            FDestinationFile = System.IO.Path.GetDirectoryName(AXAMLFilename) +
                               System.IO.Path.DirectorySeparatorChar +
                               System.IO.Path.GetFileNameWithoutExtension(AXAMLFilename) +
                               ADestFileExtension;

            if (!ACheckTemplateCompletion || CheckTemplateCompletion(FTemplateCode))
            {
                Console.WriteLine("Writing " + Path.GetFileName(FDestinationFile));

                // just one line break at the end
                string code = FTemplateCode.ToString().TrimEnd(new char[] { ' ', '\t', '\r', '\n' });

                StreamWriter sw = new StreamWriter(FDestinationFile + ".new", false, System.Text.Encoding.UTF8);
                sw.Write(code);
                sw.Close();

                TTextFile.UpdateFile(FDestinationFile, true);

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
            ProcessIFDEFs(ref FTemplateCode);
            ProcessIFNDEFs(ref FTemplateCode);
            BeautifyCode(ref FTemplateCode);

            if (!ACheckTemplateCompletion || CheckTemplateCompletion(FTemplateCode))
            {
                // just one line break at the end
                return FTemplateCode.ToString().TrimEnd(new char[] { ' ', '\t', '\r', '\n' }) + Environment.NewLine;
            }

            // an exception is thrown anyways by CheckTemplateCompletion
            return "";
        }
    }

    /// <summary>
    /// helpful extensions for the StringBuilder
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// useful extension for StringBuilder
        public static bool Contains(this StringBuilder sb, string s)
        {
            return sb.ToString().Contains(s);
        }

        /// useful extension for StringBuilder
        public static int IndexOf(this StringBuilder sb, string s)
        {
            return sb.ToString().IndexOf(s);
        }

        /// useful extension for StringBuilder
        public static int IndexOf(this StringBuilder sb, string s, int pos)
        {
            return sb.ToString().IndexOf(s, pos);
        }

        /// useful extension for StringBuilder
        public static string Substring(this StringBuilder sb, int pos, int length)
        {
            return sb.ToString().Substring(pos, length);
        }

        /// useful extension for StringBuilder
        public static string Substring(this StringBuilder sb, int pos)
        {
            return sb.ToString().Substring(pos);
        }
    }
}