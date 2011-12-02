//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using System.Data;
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using System.Collections.Generic;
using Ict.Tools.CodeGeneration;
using Ict.Common.IO;

namespace Tests.CodeGeneration
{
    /// Test code generation
    [TestFixture]
    public class TMyTest
    {
        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [SetUp]
        public void Init()
        {
        }

        /// <summary>
        /// cleaning up everything that was set up for this test
        /// </summary>
        [TearDown]
        public void TearDown()
        {
        }

        /// <summary>
        /// each character gets its own line
        /// </summary>
        static string[] CharToLines(string s)
        {
            string[] result = new string[s.Length];

            for (int count = 0; count < s.Length; count++)
            {
                result[count] = s[count].ToString();
            }

            return result;
        }

        /// <summary>
        /// compare 2 strings.
        /// need to convert them to something resembling a file, so one character per line
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        static string TestHelperStrings(string s1, string s2)
        {
            Console.WriteLine(s1);
            Console.WriteLine(s2);

            string[] file1 = CharToLines(s1);
            string[] file2 = CharToLines(s2);

            List <string>hashes = new List <string>();
            List <string>origLines = new List <string>();

            Int32[] file1Indexes = TFileDiffMerge.CalculateHashes(file1, 0, ref hashes, ref origLines, true);
            Int32[] file2Indexes = TFileDiffMerge.CalculateHashes(file2, 0, ref hashes, ref origLines, false);

            // calculate matrix for the indexes of the hashes
            int[, ] matrix = TFileDiffMerge.GetLongestCommonSubsequenceMatrix(file1Indexes, file2Indexes);

            string result = TFileDiffMerge.GetDiffResult(matrix, file1Indexes, file2Indexes, origLines);
            Console.WriteLine(result);

            return result;
        }

        /// <summary>
        /// returns true if merging RewriteFile into OrigFile results in Resultfile
        /// </summary>
        /// <param name="AOrigFilename"></param>
        /// <param name="ARewriteFilename"></param>
        /// <param name="AResultFilename"></param>
        /// <returns></returns>
        private bool TestHelperMerge(string AOrigFilename, string ARewriteFilename, string AResultFilename)
        {
            const string path = "../../csharp/ICT/Testing/lib/CodeGeneration/SampleData/";

            TFileDiffMerge.Merge2Files(path + AOrigFilename, path + ARewriteFilename, path + AResultFilename + ".new");

            bool result = TTextFile.SameContent(path + AResultFilename + ".new", path + AResultFilename);

            if (result)
            {
                File.Delete(path + AResultFilename + ".new");
            }
            else
            {
                Assert.Fail("please compare " + path + AResultFilename + " and " + AResultFilename + ".new");
            }

            return result;
        }

        /// <summary>
        /// testing the diff and merge of simple strings
        /// </summary>
        [Test]
        public void TestDiffMergeStrings()
        {
            // see console output for readable diff
            Assert.AreEqual(TestHelperStrings("1ac", "abc"), "- 1\r\n  a\r\n+ b\r\n  c\r\n", "diff of 1ac and abc");
            Assert.AreEqual(TestHelperStrings("1ac", "abcd"), "- 1\r\n  a\r\n+ b\r\n  c\r\n+ d\r\n", "diff of 1ac and abcd");
            Assert.AreEqual(TestHelperStrings("HelloWorld",
                    "HelloWOrld"), "  H\r\n  e\r\n  l\r\n  l\r\n  o\r\n  W\r\n- o\r\n+ O\r\n  r\r\n  l\r\n  d\r\n",
                "diff of HelloWorld and HelloWOrld");
        }

        /// <summary>
        /// testing the diff and merge of text files
        /// </summary>
        [Test]
        public void TestDiffMergeTool1()
        {
            Assert.AreEqual(TestHelperMerge("orig.txt", "rewrite.txt", "result.txt"), true, "merging orig.txt and rewrite.txt");
        }

        /// <summary>
        /// testing the diff and merge of text files
        /// </summary>
        [Test]
        public void TestDiffMergeTool2()
        {
            Assert.AreEqual(TestHelperMerge("orig2.txt", "rewrite2.txt", "result2.txt"), true, "merging orig2.txt and rewrite2.txt");
        }

        /// <summary>
        /// testing the diff and merge of text files
        /// </summary>
        [Test]
        public void TestDiffMergeTool3()
        {
            Assert.AreEqual(TestHelperMerge("orig3.txt", "rewrite3.txt", "result3.txt"), true, "merging orig3.txt and rewrite3.txt");
        }
    }
}