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
using System.IO;
using System.Text;
using System.Collections.Generic;
using Ict.Tools.DBXML;
using Ict.Common;
using ICSharpCode.SharpZipLib.GZip;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// manage the sequences
    /// </summary>
    public class TSequenceWriter
    {
        private static SortedList <string, long>Sequences = null;

        /// <summary>
        /// init the sequences file
        /// </summary>
        public static void InitSequences(List <TSequence>ASequencesToBeMigrated)
        {
            string filename = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "_Sequences.txt";

            if (File.Exists(filename))
            {
                LoadSequences();
            }
            else
            {
                // parse _seqvals.d
                string filenameseqvals = filename.Replace("_Sequences.txt", "_seqvals.d");

                if (!File.Exists(filenameseqvals))
                {
                    TLogging.Log("Warning: cannot find file " + filenameseqvals);

                    return;
                }

                StreamReader sr = new StreamReader(filenameseqvals);
                Sequences = new SortedList <string, long>();

                foreach (TSequence newSequence in ASequencesToBeMigrated)
                {
                    Sequences.Add(newSequence.strName, newSequence.iInitial);
                }

                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(new char[] { ' ' });

                    if (line[0] == ".")
                    {
                        break;
                    }

                    string name = line[1].Trim(new char[] { '"' });

                    if (Sequences.ContainsKey(name))
                    {
                        Sequences[name] = Convert.ToInt64(line[2]);
                    }
                }

                sr.Close();
            }
        }

        /// <summary>
        /// load the sequences from the temp file
        /// </summary>
        public static void LoadSequences()
        {
            if (Sequences == null)
            {
                Sequences = new SortedList <string, long>();

                string filename = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "_Sequences.txt";

                if (File.Exists(filename))
                {
                    StreamReader sr = new StreamReader(filename);
                    string[] line;

                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine().Split(new char[] { ',' });

                        Sequences.Add(line[0], Convert.ToInt64(line[1]));
                    }

                    sr.Close();
                }
                else
                {
                    throw new Exception("please call InitSequences first");
                }
            }
        }

        /// <summary>
        /// write the sequences to file
        /// </summary>
        public static void WriteSequences()
        {
            if (Sequences != null)
            {
                string filename = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "_Sequences.txt";

                StreamWriter sw = new StreamWriter(filename);

                foreach (string s in Sequences.Keys)
                {
                    sw.WriteLine(s + "," + Sequences[s].ToString());
                }

                sw.Close();

                // write _Sequences.sql.gz
                FileStream outStream = File.Create(filename.Replace(".txt", ".sql.gz"));
                Stream gzoStream = new GZipOutputStream(outStream);
                sw = new StreamWriter(gzoStream, Encoding.UTF8);

                foreach (string sequenceName in Sequences.Keys)
                {
                    sw.WriteLine("SELECT pg_catalog.setval('" + sequenceName + "', " +
                        Sequences[sequenceName].ToString() +
                        ", false);");
                }

                sw.Close();
            }
        }

        /// <summary>
        /// get the next value
        /// </summary>
        public static long GetNextSequenceValue(string ASequenceName)
        {
            Sequences[ASequenceName]++;
            return Sequences[ASequenceName] - 1;
        }
    }
}