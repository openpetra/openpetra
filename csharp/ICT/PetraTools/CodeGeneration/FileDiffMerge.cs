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
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Ict.Common.IO;

namespace Ict.Tools.CodeGeneration
{
    /// see http://en.wikibooks.org/wiki/Algorithm_implementation/Strings/Longest_common_subsequence
    /// and http://www.mycsharp.de/wbb2/thread.php?threadid=79055
    public class TFileDiffMerge
    {
        /// calculate md5hash for each line that is longer than 16 byte.
        /// store new hashes in a list, store index (int32 4 byte).
        /// store the unique lines, each line is at the same index as its hash in the hash list
        public static Int32[] CalculateHashes(string[] ASourceLines,
            Int32 AFirstDifferentLineNumber,
            ref List <string>AHashes,
            ref List <string>AUniqueLines,
            bool AMatchCommentedLines)
        {
            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();

            Int32[] indexes = new Int32[ASourceLines.Length - AFirstDifferentLineNumber];
            bool currentlyInManualCodeBlock = false;
            Int32 ManualCodeLastStartLine = -1;

            for (int count = AFirstDifferentLineNumber; count < ASourceLines.Length; count++)
            {
                string hash = ASourceLines[count];

                if (ASourceLines[count].Trim().StartsWith("#region ManualCode"))
                {
                    if (currentlyInManualCodeBlock)
                    {
                        throw new Exception(
                            "missing proper closing of ManualCode region: #endregion ManualCode. see line: " + ManualCodeLastStartLine.ToString());
                    }

                    ManualCodeLastStartLine = count;
                    currentlyInManualCodeBlock = true;
                }
                else if (ASourceLines[count].Trim().StartsWith("#endregion ManualCode"))
                {
                    currentlyInManualCodeBlock = false;
                }

                // allow to keep commented lines commented, but still recognize they are the same
                if (AMatchCommentedLines && !currentlyInManualCodeBlock)
                {
                    if (hash.Trim().StartsWith("//") && (hash.Trim().Length > 2))
                    {
                        hash = hash.Substring(0, hash.IndexOf("//")) + hash.Substring(hash.IndexOf("//") + 2).Trim();
                    }
                }

                if (currentlyInManualCodeBlock)
                {
                    // avoid mixing generated code and manual code
                    hash = "MANUALCODE" + hash;
                }

                if (hash.Length > 16)
                {
                    hash = System.BitConverter.ToString(hasher.ComputeHash(Encoding.Default.GetBytes(hash)));
                }

                int index;

                if (AHashes.Contains(hash))
                {
                    index = AHashes.IndexOf(hash);
                }
                else
                {
                    index = AHashes.Count;
                    AUniqueLines.Add(ASourceLines[count]);
                    AHashes.Add(hash);
                }

                indexes[count - AFirstDifferentLineNumber] = index;
            }

            return indexes;
        }

        /// <summary>
        /// calculate the longest common subsequence matrix. this will tell us which parts have been added or removed
        /// </summary>
        /// <param name="AList1">a list of integers</param>
        /// <param name="AList2">list of integers to be compared with the first list</param>
        /// <returns></returns>
        public static int[, ] GetLongestCommonSubsequenceMatrix(Int32[] AList1, Int32[] AList2)
        {
            int[, ] lcsMatrix = new int[AList1.Length + 1, AList2.Length + 1];
            Int32 element1, element2;

            for (int i = AList1.Length - 1; i >= 0; i--)
            {
                for (int j = AList2.Length - 1; j >= 0; j--)
                {
                    element1 = AList1[i];
                    element2 = AList2[j];

                    if (element1 == element2)
                    {
                        if ((i == AList1.Length - 1) || (j == AList2.Length - 1))
                        {
                            lcsMatrix[i, j] = 1;
                        }
                        else
                        {
                            lcsMatrix[i, j] = 1 + lcsMatrix[i + 1, j + 1];
                        }
                    }
                    else
                    {
                        if (i == AList1.Length - 1)
                        {
                            if (j == AList2.Length - 1)
                            {
                                lcsMatrix[i, j] = 0;
                            }
                            else
                            {
                                lcsMatrix[i, j] = Math.Max(0, lcsMatrix[i, j + 1]);
                            }
                        }
                        else
                        {
                            if (j == AList2.Length - 1)
                            {
                                lcsMatrix[i, j] = Math.Max(lcsMatrix[i + 1, j], 0);
                            }
                            else
                            {
                                lcsMatrix[i, j] = Math.Max(lcsMatrix[i + 1, j], lcsMatrix[i, j + 1]);
                            }
                        }
                    }
                }
            }

            return lcsMatrix;
        }

        /// <summary>
        /// Calculate the diff result and return as string
        /// </summary>
        /// <param name="AMatrix">the matrix that was calculated by GetLongestCommonSubsequenceMatrix</param>
        /// <param name="AList1"></param>
        /// <param name="AList2"></param>
        /// <param name="AOrigLines">the indexes of the 2 lists refer to these strings</param>
        public static string GetDiffResult(int[, ] AMatrix, Int32[] AList1, Int32[] AList2, List <string>AOrigLines)
        {
            int x = 0, y = 0;
            string result = string.Empty;

            while (x < AList1.Length && y < AList2.Length)
            {
                if (AList1[x] == AList2[y])
                {
                    result += string.Format("  {0}", AOrigLines[AList1[x]]) + Environment.NewLine;
                    x++;
                    y++;
                }
                else if (AMatrix[x, y + 1] > AMatrix[x + 1, y])
                {
                    result += string.Format("+ {0}", AOrigLines[AList2[y]]) + Environment.NewLine;
                    y++;
                }
                else
                {
                    result += string.Format("- {0}", AOrigLines[AList1[x]]) + Environment.NewLine;
                    x++;
                }
            }

            if (x < AList1.Length)
            {
                for (int i = x; i < AList1.Length; i++)
                {
                    result += string.Format("- {0}", AOrigLines[AList1[i]]) + Environment.NewLine;
                }
            }
            else if (y < AList2.Length)
            {
                for (int j = y; j < AList2.Length; j++)
                {
                    result += string.Format("+ {0}", AOrigLines[AList2[j]]) + Environment.NewLine;
                }
            }

            return result;
        }

        /// <summary>
        /// Merge the second file into the first file, and return the result.
        /// special treatment of region ManualCode, this will always be included in the result
        /// </summary>
        /// <param name="AMatrix">the matrix that was calculated by GetLongestCommonSubsequenceMatrix</param>
        /// <param name="AList1"></param>
        /// <param name="AList2"></param>
        /// <param name="AOrigLinesBeforeHashing">the indexes of the 2 lists refer to these strings</param>
        /// <param name="AOrigFileLines">the original file which should be used for identical initial part of the files</param>
        /// <param name="AFirstDifferentLine">number of lines that should be copied from AOrigFileLines</param>
        public static string[] MergeSecondFileIntoFirst(int[, ] AMatrix,
            Int32[] AList1,
            Int32[] AList2,
            List <string>AOrigLinesBeforeHashing,
            string[] AOrigFileLines,
            Int32 AFirstDifferentLine)
        {
            int x = 0, y = 0;

            List <string>result = new List <string>();

            for (Int32 counter = 0; counter < AFirstDifferentLine; counter++)
            {
                result.Add(AOrigFileLines[counter]);
            }

            bool currentlyInManualCodeBlock = false;

            while (x < AList1.Length && y < AList2.Length)
            {
                if (AList1[x] == AList2[y])
                {
                    // lines are equal in both files
                    result.Add(AOrigLinesBeforeHashing[AList1[x]]);
                    x++;
                    y++;
                }
                else if (AMatrix[x, y + 1] > AMatrix[x + 1, y])
                {
                    // the second file has new lines
                    result.Add(AOrigLinesBeforeHashing[AList2[y]]);
                    y++;
                }
                else
                {
                    // there are lines from the first file missing in the second file
                    // only add old lines from first file if manual code
                    if (AOrigLinesBeforeHashing[AList1[x]].Trim().StartsWith("#region ManualCode"))
                    {
                        currentlyInManualCodeBlock = true;
                    }

                    if (currentlyInManualCodeBlock)
                    {
                        result.Add(AOrigLinesBeforeHashing[AList1[x]]);
                    }

                    if (AOrigLinesBeforeHashing[AList1[x]].Trim().StartsWith("#endregion ManualCode"))
                    {
                        currentlyInManualCodeBlock = false;
                    }

                    x++;
                }
            }

            if (x < AList1.Length)
            {
                // there are more lines in the first file than in the second file
                for (int i = x; i < AList1.Length; i++)
                {
                    if (AOrigLinesBeforeHashing[AList1[i]].Trim().StartsWith("#region ManualCode"))
                    {
                        currentlyInManualCodeBlock = true;
                    }

                    if (currentlyInManualCodeBlock)
                    {
                        result.Add(AOrigLinesBeforeHashing[AList1[i]]);
                    }

                    if (AOrigLinesBeforeHashing[AList1[i]].Trim().StartsWith("#endregion ManualCode"))
                    {
                        currentlyInManualCodeBlock = false;
                    }
                }
            }
            else if (y < AList2.Length)
            {
                // there are more lines in the second file than in the first file
                for (int j = y; j < AList2.Length; j++)
                {
                    result.Add(AOrigLinesBeforeHashing[AList2[j]]);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Merge 2 files, using special treatment for ManualCode and commented line.
        /// only touch the original file if it is changed
        /// </summary>
        /// <param name="AOrigFilename"></param>
        /// <param name="ANewLines"></param>
        /// <param name="ADestinationFilename"></param>
        /// <returns>returns true if the file was changed</returns>
        public static bool Merge2Files(string AOrigFilename, string[] ANewLines, string ADestinationFilename)
        {
            AOrigFilename = AOrigFilename.Replace('\\', Path.DirectorySeparatorChar);
            ADestinationFilename = ADestinationFilename.Replace('\\', Path.DirectorySeparatorChar);

            if (!File.Exists(AOrigFilename))
            {
                File.WriteAllLines(ADestinationFilename, ANewLines);
                return true;
            }

            string[] sourceLines = null;

            try
            {
                sourceLines = File.ReadAllLines(AOrigFilename);

                List <string>hashes = new List <string>();
                List <string>origLines = new List <string>();

                int FirstDifferentLineNumber = 0;

                while (FirstDifferentLineNumber < sourceLines.Length
                       && FirstDifferentLineNumber < ANewLines.Length
                       && sourceLines[FirstDifferentLineNumber] == ANewLines[FirstDifferentLineNumber])
                {
                    FirstDifferentLineNumber++;
                }

                // check if there is an occurance of manual code, otherwise just overwrite the file
                int SearchManualCode = FirstDifferentLineNumber;

                while (SearchManualCode < sourceLines.Length && !sourceLines[SearchManualCode].Trim().StartsWith("#region ManualCode"))
                {
                    SearchManualCode++;
                }

                if (SearchManualCode == sourceLines.Length)
                {
                    File.WriteAllLines(ADestinationFilename + ".new", ANewLines);
                    TTextFile.UpdateFile(ADestinationFilename, true);
                    return true;
                }

                // the order of calculation hashes is important to keep commmented lines commented
                Int32[] file1Indexes = TFileDiffMerge.CalculateHashes(sourceLines, FirstDifferentLineNumber, ref hashes, ref origLines, true);
                Int32[] file2Indexes = TFileDiffMerge.CalculateHashes(ANewLines, FirstDifferentLineNumber, ref hashes, ref origLines, false);

                // calculate matrix for the indexes of the hashes
                int[, ] matrix = TFileDiffMerge.GetLongestCommonSubsequenceMatrix(file1Indexes, file2Indexes);

                string[] mergedText = TFileDiffMerge.MergeSecondFileIntoFirst(matrix,
                    file1Indexes,
                    file2Indexes,
                    origLines,
                    sourceLines,
                    FirstDifferentLineNumber);

                bool changed = false;

                if (mergedText.Length != sourceLines.Length)
                {
                    changed = true;
                }
                else
                {
                    for (int counter = 0; counter < mergedText.Length && !changed; counter++)
                    {
                        if (mergedText[counter].CompareTo(sourceLines[counter]) != 0)
                        {
                            changed = true;
                        }
                    }
                }

                if (changed)
                {
                    // for debugging merge bugs: store the original and the generated file:
                    // File.WriteAllLines(ADestinationFilename + ".orig", sourceLines);
                    // File.WriteAllLines(ADestinationFilename + ".generated", ANewLines);
                    File.WriteAllLines(ADestinationFilename + ".new", mergedText);
                    TTextFile.UpdateFile(ADestinationFilename);
                }

                return changed;
            }
            catch (Exception e)
            {
                if (sourceLines != null)
                {
                    File.WriteAllLines(ADestinationFilename + ".orig", sourceLines);
                }

                File.WriteAllLines(ADestinationFilename + ".generated", ANewLines);
                throw new Exception(String.Format("Problems merging file {0}: {1}" +
                        Environment.NewLine + e.StackTrace, AOrigFilename, e.Message));
            }
        }

        /// <summary>
        /// overload that writes the result back to the original file
        /// </summary>
        /// <param name="AOrigFilename"></param>
        /// <param name="ANewLines"></param>
        /// <returns></returns>
        public static bool Merge2Files(string AOrigFilename, string[] ANewLines)
        {
            return Merge2Files(AOrigFilename, ANewLines, AOrigFilename);
        }

        /// <summary>
        /// overload for just the filenames, files will be loaded by this functions
        /// </summary>
        /// <param name="AOrigFilename"></param>
        /// <param name="ANewFilename"></param>
        /// <param name="ADestinationFilename"></param>
        /// <returns></returns>
        public static bool Merge2Files(string AOrigFilename, string ANewFilename, string ADestinationFilename)
        {
            return Merge2Files(AOrigFilename, File.ReadAllLines(ANewFilename), ADestinationFilename);
        }
    }
}