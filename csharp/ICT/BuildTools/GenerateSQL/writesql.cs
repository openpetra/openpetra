//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2017 by OM International
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

// #define IMPORTFROMLEGACYDB

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ict.Common;
using Ict.Common.IO; // Implicit reference
using Ict.Tools.DBXML;

namespace GenerateSQL
{
    /// <summary>
    /// This class will write the SQL create table statements and other SQL statements
    /// using the datadefinitionn from the xml file
    /// </summary>
    public class TWriteSQL
    {
        /// <summary>
        /// list of all supported database types
        /// </summary>
        public enum eDatabaseType
        {
            /// <summary>
            /// PostgreSQL
            /// </summary>
            PostgreSQL,
            /// <summary>
            /// MySQL
            /// </summary>
            MySQL,
            /// <summary>
            /// SQLite
            /// </summary>
            Sqlite
        };

        private enum eInclude { eInCreateTable, eIncludeSeparate, eOnlyForeign, eOnlyLocal };

        /// <summary>
        /// parse the database type from a string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static eDatabaseType StringToDBMS(string name)
        {
            if (name.ToLower() == "postgresql")
            {
                return eDatabaseType.PostgreSQL;
            }

            if (name.ToLower() == "mysql")
            {
                return eDatabaseType.MySQL;
            }

            if (name.ToLower() == "sqlite")
            {
                return eDatabaseType.Sqlite;
            }

            // default and recommended
            return eDatabaseType.PostgreSQL;
        }

        /// <summary>
        /// create a database file or the scripts for creating a database.
        /// for SQLite that is directly the file,
        /// for the other database systems the sql create table scripts etc are written
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="ATargetDatabase"></param>
        /// <param name="AOutputFile"></param>
        public static void WriteSQL(TDataDefinitionStore AStore, eDatabaseType ATargetDatabase, String AOutputFile)
        {
            System.Console.WriteLine("Writing file to {0}...", AOutputFile);

            FileStream outPutFileStream = new FileStream(AOutputFile, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(outPutFileStream);

            if (ATargetDatabase == eDatabaseType.MySQL)
            {
                sw.WriteLine("SET AUTOCOMMIT=0;");
                sw.WriteLine("SET FOREIGN_KEY_CHECKS=0;");
            }

            List <TTable>Tables = AStore.GetTables();

            Tables = TTableSort.TopologicalSort(AStore, Tables);

            foreach (TTable Table in Tables)
            {
                if (!WriteTable(ATargetDatabase, sw, Table, true))
                {
                    Environment.Exit(1);
                }
            }

            if (ATargetDatabase == eDatabaseType.PostgreSQL)
            {
                foreach (TTable Table in Tables)
                {
                    DumpConstraints(sw, Table, eInclude.eOnlyForeign, true);
                }
            }

            WriteSequences(sw, ATargetDatabase, AStore, true);

            sw.Close();
            System.Console.WriteLine("Success: file written: {0}", AOutputFile);

            string createTablesWithoutConstraints =
                Path.GetDirectoryName(AOutputFile) + Path.DirectorySeparatorChar +
                Path.GetFileNameWithoutExtension(AOutputFile) + "_withoutConstraints.sql";
            outPutFileStream = new FileStream(
                createTablesWithoutConstraints,
                FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(outPutFileStream);

            if (ATargetDatabase == eDatabaseType.PostgreSQL)
            {
                sw.WriteLine("SET client_min_messages TO WARNING;");
            }
            else if (ATargetDatabase == eDatabaseType.MySQL)
            {
                sw.WriteLine("SET AUTOCOMMIT=0;");
                sw.WriteLine("SET FOREIGN_KEY_CHECKS=0;");
            }

            foreach (TTable Table in Tables)
            {
                if (!WriteTable(ATargetDatabase, sw, Table))
                {
                    Environment.Exit(1);
                }
            }

            WriteSequences(sw, ATargetDatabase, AStore, true);
            sw.Close();
            System.Console.WriteLine("Success: file written: {0}", createTablesWithoutConstraints);

            string removeAllTablesFile =
                Path.GetDirectoryName(AOutputFile) + Path.DirectorySeparatorChar +
                Path.GetFileNameWithoutExtension(AOutputFile) + "_remove.sql";
            outPutFileStream = new FileStream(
                removeAllTablesFile,
                FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(outPutFileStream);

            Tables = TTableSort.TopologicalSort(AStore, Tables);
            Tables.Reverse();

            if (ATargetDatabase == eDatabaseType.MySQL)
            {
                sw.WriteLine("SET foreign_key_checks = 0;");
            }

            foreach (TTable Table in Tables)
            {
                sw.WriteLine("DROP TABLE IF EXISTS " + Table.strName + " CASCADE;");
            }

            if (ATargetDatabase == eDatabaseType.Sqlite)
            {
                // see http://www.mail-archive.com/sqlite-users@sqlite.org/msg05123.html
                // see http://www.sqlite.org/faq.html#q1
                // no sequences in sqlite
            }
            else if (ATargetDatabase == eDatabaseType.MySQL)
            {
                // also no sequences in Mysql
                // see http://dev.mysql.com/doc/refman/5.0/en/information-functions.html for a workaround
                // look for CREATE TABLE sequence and LAST_INSERT_ID
                List <TSequence>Sequences = AStore.GetSequences();

                foreach (TSequence Sequence in Sequences)
                {
                    sw.WriteLine("DROP TABLE IF EXISTS " + Sequence.strName + ";");
                }
            }
            else
            {
                List <TSequence>Sequences = AStore.GetSequences();

                foreach (TSequence Sequence in Sequences)
                {
                    sw.WriteLine("DROP SEQUENCE IF EXISTS " + Sequence.strName + ";");
                }
            }

            sw.Close();
            System.Console.WriteLine("Success: file written: {0}", removeAllTablesFile);

            string LoadDataFile = System.IO.Path.GetDirectoryName(AOutputFile) +
                                  System.IO.Path.DirectorySeparatorChar +
                                  "loaddata.sh";
            outPutFileStream = new FileStream(LoadDataFile,
                FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(outPutFileStream);

            // todo: sw.WriteLine("echo \"load tables\"; psql petra < petra0203_tables.sql");
            foreach (TTable Table in Tables)
            {
                // Dump indexes
                sw.WriteLine("echo " + Table.strName + "; psql petra < " + Table.strName + ".sql");
            }

            // todo: sw.WriteLine("echo \"load constraints\"; psql petra < petra0203_constraints.sql");
            // todo: sw.WriteLine("echo \"load indexes\"; psql petra < petra0203_indexes.sql");
            sw.Close();
            outPutFileStream.Close();
            System.Console.WriteLine("Success: file written: {0}", LoadDataFile);

            string CreateTablesFile = System.IO.Path.GetDirectoryName(AOutputFile) +
                                      System.IO.Path.DirectorySeparatorChar +
                                      "createtables-" + ATargetDatabase.ToString() + ".sql";
            outPutFileStream = new FileStream(CreateTablesFile,
                FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(outPutFileStream);

            if (ATargetDatabase == eDatabaseType.PostgreSQL)
            {
                sw.WriteLine("SET client_min_messages TO WARNING;");
            }
            else if (ATargetDatabase == eDatabaseType.MySQL)
            {
                sw.WriteLine("SET AUTOCOMMIT=0;");
                sw.WriteLine("SET FOREIGN_KEY_CHECKS=0;");
            }

            foreach (TTable Table in Tables)
            {
                if (!WriteTable(ATargetDatabase, sw, Table))
                {
                    Environment.Exit(1);
                }
            }

            WriteSequences(sw, ATargetDatabase, AStore, false);

            sw.Close();
            outPutFileStream.Close();
            System.Console.WriteLine("Success: file written: {0}", CreateTablesFile);

            string CreateConstraintsAndIndexesFile = System.IO.Path.GetDirectoryName(AOutputFile) +
                                                     System.IO.Path.DirectorySeparatorChar +
                                                     "createconstraints-" + ATargetDatabase.ToString() + ".sql";
            outPutFileStream = new FileStream(CreateConstraintsAndIndexesFile,
                FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(outPutFileStream);

            foreach (TTable Table in Tables)
            {
                if (ATargetDatabase == eDatabaseType.Sqlite)
                {
                    // see http://www.sqlite.org/omitted.html:
                    // sqlite does not support Alter table add constraint
                }
                else
                {
                    DumpConstraints(sw, Table, eInclude.eOnlyForeign, true);
                }
            }

            foreach (TTable Table in Tables)
            {
                // Dump indexes
                DumpIndexes(sw, Table, eInclude.eIncludeSeparate, true);
            }

            sw.Close();
            outPutFileStream.Close();
            System.Console.WriteLine("Success: file written: {0}", CreateConstraintsAndIndexesFile);
        }

        private static Boolean WriteSequences(StreamWriter ASw,
            eDatabaseType ATargetDatabase,
            TDataDefinitionStore AStore,
            bool AWithSequenceInitialisation)
        {
            if (ATargetDatabase == eDatabaseType.Sqlite)
            {
                // see http://www.mail-archive.com/sqlite-users@sqlite.org/msg05123.html
                // see http://www.sqlite.org/faq.html#q1
                // no sequences in sqlite
            }
            else if (ATargetDatabase == eDatabaseType.MySQL)
            {
                // also no sequences in Mysql
                // see http://dev.mysql.com/doc/refman/5.0/en/information-functions.html for a workaround
                // look for CREATE TABLE sequence and LAST_INSERT_ID
                List <TSequence>Sequences = AStore.GetSequences();

                foreach (TSequence seq in Sequences)
                {
                    string createStmt = "CREATE TABLE " + seq.strName + " (sequence INTEGER AUTO_INCREMENT, dummy INTEGER, PRIMARY KEY(sequence));";
                    ASw.WriteLine(createStmt);

                    if (AWithSequenceInitialisation)
                    {
                        // the following line would cause trouble later when loading the demo/base data
                        createStmt = "INSERT INTO " + seq.strName + " VALUES(NULL, -1);";
                        ASw.WriteLine(createStmt);
                    }
                }
            }
            else
            {
                List <TSequence>Sequences = AStore.GetSequences();

                foreach (TSequence Sequence in Sequences)
                {
                    if (!WriteSequence(ASw, Sequence))
                    {
                        Environment.Exit(1);
                    }
                }
            }

            return true;
        }

        private static Boolean WriteSequence(StreamWriter ASw, TSequence ASequence)
        {
            ASw.WriteLine("CREATE SEQUENCE \"{0}\"", ASequence.strName);
            ASw.WriteLine("  INCREMENT BY {0}", ASequence.iIncrement.ToString());
            ASw.WriteLine("  MINVALUE {0}", ASequence.iMinVal.ToString());
            ASw.WriteLine("  MAXVALUE {0}", ASequence.iMaxVal.ToString());
            ASw.Write("  START WITH {0}", ASequence.iInitial.ToString());

            if (ASequence.bCycleOnLimit)
            {
                ASw.WriteLine();
                ASw.Write("  CYCLE");
            }

            ASw.WriteLine(";");
            return true;
        }

        private static Boolean WriteTable(eDatabaseType ATargetDatabase, StreamWriter ASw, TTable ATable, bool AWriteConstraintsAndIndexes = false)
        {
            // Show the table info
            ASw.WriteLine("-- {0}", ATable.strDescription.Replace("\r", " ").Replace("\n", " "));
            ASw.WriteLine("-- GROUP: {0}", ATable.strGroup);
            ASw.WriteLine("CREATE TABLE {0} (", ATable.strName);

            // Dump fields
            DumpFields(ATargetDatabase, ASw, ATable);

            if (AWriteConstraintsAndIndexes)
            {
                if (ATargetDatabase == eDatabaseType.PostgreSQL)
                {
                    DumpConstraints(ASw, ATable, eInclude.eOnlyLocal, true);
                }
                else
                {
                    DumpConstraints(ASw, ATable, eInclude.eInCreateTable, true);
                    DumpIndexes(ASw, ATable, eInclude.eInCreateTable, true);
                }
            }
            else
            {
                // always add the primary key
                DumpConstraints(ASw, ATable, eInclude.eOnlyLocal, true);
            }

            ASw.WriteLine();
            ASw.Write(")");

            if (ATargetDatabase == eDatabaseType.MySQL)
            {
                // use InnoDB, otherwise there are no constraints
                ASw.WriteLine(" ENGINE=InnoDB");
                // make sure we are using the character set for UTF8
                ASw.WriteLine(" CHARACTER SET UTF8");
            }

            ASw.WriteLine(";");

            if (AWriteConstraintsAndIndexes && (ATargetDatabase == eDatabaseType.PostgreSQL))
            {
                DumpIndexes(ASw, ATable, eInclude.eIncludeSeparate, true);
            }

            ASw.WriteLine();
            return true;
        }

        private static void DumpFields(eDatabaseType ATargetDatabase, StreamWriter ASw, TTable ATable)
        {
            // Run over all fields
            bool first = true;

            foreach (TTableField field in ATable.grpTableField)
            {
                ASw.Write(WriteField(ATargetDatabase, ATable, field, first, true));
                first = false;
            }
        }

        /// <summary>
        /// write the part of the sql creation script for a specific column
        /// </summary>
        /// <param name="ATargetDatabase"></param>
        /// <param name="ATable"></param>
        /// <param name="field"></param>
        /// <param name="first"></param>
        /// <param name="AWithComments"></param>
        /// <returns></returns>
        public static string WriteField(eDatabaseType ATargetDatabase, TTable ATable, TTableField field, bool first, bool AWithComments)
        {
            string result = "";

            if (!first)
            {
                result += ",";
            }

            if (AWithComments)
            {
                result += Environment.NewLine;

                if (field.strDescription.Length != 0)
                {
                    result += String.Format("    -- {0}", field.strDescription.Replace("\r", " ").Replace("\n", " ")) + Environment.NewLine;
                }
            }

#if IMPORTFROMLEGACYDB
            // this is useful when converting from legacy database, with columns that contain too long strings
            if ((field.strType == "varchar") && (field.iLength >= 20) && (!field.strName.Contains("_code_")))
            {
                field.strType = "text";
            }
#endif

            if (field.strType == "bit")
            {
                result += String.Format("  {0} boolean", field.strName);
            }
            else if ((field.strType == "number") && (field.iLength == 10) && (field.iDecimals == -1))
            {
                result += String.Format("  {0} bigint", field.strName);
            }
            else if (field.strType == "number")
            {
                result += String.Format("  {0} numeric", field.strName);
            }
            else if (field.bAutoIncrement)
            {
                if (ATargetDatabase == eDatabaseType.PostgreSQL)
                {
                    result += String.Format("  {0} SERIAL", field.strName);
                }
                else if (ATargetDatabase == eDatabaseType.Sqlite)
                {
                    result += String.Format("  {0} INTEGER PRIMARY KEY AUTOINCREMENT ", field.strName);
                }
                else if (ATargetDatabase == eDatabaseType.MySQL)
                {
                    result += String.Format("  {0} INTEGER AUTO_INCREMENT UNIQUE ", field.strName);
                }
            }
            else
            {
                result += String.Format("  {0} {1}", field.strName, field.strType);
            }

            // According to the type we will add parameters
            if ((field.strType == "number") && (field.iLength == 10) && (field.iDecimals == -1))
            {
                // no parameter for bigints
            }
            else if ((field.strType == "varchar") || (field.strType == "number"))
            {
                if (field.iLength >= 0)
                {
                    result += String.Format("({0}", field.iLength.ToString());
                }

                if (field.strType == "number")
                {
                    result += String.Format(", {0}", (field.iDecimals > 0 ? field.iDecimals.ToString() : "0"));
                }

                result += String.Format(")");
            }

            if (field.strDefault.Length > 0)
            {
                if (field.strDefault == "NULL")
                {
                    result += String.Format(" DEFAULT {0}", field.strDefault);
                }
                else if ((field.strType == "varchar") || (field.strType == "text"))
                {
                    result += String.Format(" DEFAULT '{0}'", field.strDefault);
                }
                else if (field.strType == "bit")
                {
                    if (ATargetDatabase == eDatabaseType.MySQL)
                    {
                        result += String.Format(" DEFAULT {0}", field.strDefault);
                    }
                    else
                    {
                        result += String.Format(" DEFAULT '{0}'", field.strDefault);
                    }
                }
                else if (field.strDefault == "SYSDATE")
                {
                    // MySql cannot have a function for the default value
                    // see http://dev.mysql.com/doc/refman/5.0/en/data-type-defaults.html
                    if (ATargetDatabase != eDatabaseType.MySQL)
                    {
                        result += String.Format(" DEFAULT CURRENT_DATE");
                    }
                }
                else
                {
                    result += String.Format(" DEFAULT {0}", field.strDefault);
                }
            }

#if (!IMPORTFROMLEGACYDB)
            if (field.bNotNull)
            {
                result += String.Format(" NOT NULL");
            }

            if ((field.strCheck != null) && (field.strCheck.Length != 0))
            {
                result += String.Format(" CHECK {0}", field.strCheck);
            }
#endif

            return result;
        }

        private static void DumpConstraints(StreamWriter ASw, TTable ATable, eInclude onlyForeign, Boolean AAdd)
        {
            foreach (TConstraint constr in ATable.grpConstraint)
            {
                WriteConstraint(ASw, ATable, constr, onlyForeign, AAdd);
            }
        }

        private static void WriteConstraint(StreamWriter ASw, TTable ATable, TConstraint constr, eInclude onlyForeign, Boolean AAdd)
        {
            if (onlyForeign != eInclude.eOnlyForeign && (constr.strType == "primarykey"))
            {
                ASw.WriteLine(",");
                ASw.WriteLine("  CONSTRAINT {0}", constr.strName);
                ASw.Write("    PRIMARY KEY ({0})", StringHelper.StrMerge(constr.strThisFields, ','));
            }

            if (onlyForeign != eInclude.eOnlyForeign && (constr.strType == "uniquekey"))
            {
                ASw.WriteLine(",");
                ASw.WriteLine("  CONSTRAINT {0}", constr.strName);
                ASw.Write("    UNIQUE ({0})", StringHelper.StrMerge(constr.strThisFields, ','));
            }

            if (onlyForeign == eInclude.eOnlyForeign && (constr.strType == "foreignkey"))
            {
                ASw.WriteLine("ALTER TABLE {0}", ATable.strName);

                if (AAdd)
                {
                    ASw.WriteLine("  ADD CONSTRAINT {0}", constr.strName);
                    ASw.WriteLine("    FOREIGN KEY ({0})", StringHelper.StrMerge(constr.strThisFields, ','));
                    ASw.WriteLine("    REFERENCES {0}({1});", constr.strOtherTable, StringHelper.StrMerge(constr.strOtherFields, ','));
                }
                else
                {
                    ASw.WriteLine("  DROP CONSTRAINT IF EXISTS {0};", constr.strName);
                }
            }
            else if (onlyForeign == eInclude.eInCreateTable && (constr.strType == "foreignkey"))
            {
                ASw.WriteLine(",");
                ASw.WriteLine("  CONSTRAINT {0}", constr.strName);
                ASw.WriteLine("    FOREIGN KEY ({0})", StringHelper.StrMerge(constr.strThisFields, ','));
                ASw.Write("    REFERENCES {0}({1})", constr.strOtherTable, StringHelper.StrMerge(constr.strOtherFields, ','));
            }
        }

        static Int32 countGeneratedIndex = 0;
        private static void DumpIndexes(StreamWriter ASw, TTable ATable, eInclude includeIndexes, Boolean AAdd)
        {
            for (System.Int32 implicit_ = 0; implicit_ <= 1; implicit_ += 1)
            {
                countGeneratedIndex = 0;

                foreach (TIndex index in ATable.grpIndex)
                {
                    // first the automatically generated Indexes
                    if (index.bImplicit == (implicit_ != 1))
                    {
                        string indexName = index.strName;

                        if ((indexName.IndexOf("_fkcr_key") != 0)
                            || (indexName.IndexOf("_fkmd_key") != 0)
                            || (indexName.IndexOf("inx_s_group_gift") != 0))
                        {
                            indexName += countGeneratedIndex++;
                        }

                        if (AAdd)
                        {
                            if (includeIndexes == eInclude.eInCreateTable)
                            {
                                ASw.WriteLine(",");
                            }
                            else
                            {
                                ASw.Write("CREATE ");
                            }

                            if (index.bUnique)
                            {
                                ASw.Write("UNIQUE ");
                            }

                            if (includeIndexes == eInclude.eInCreateTable)
                            {
                                ASw.WriteLine("KEY {0} ", indexName);
                            }
                            else
                            {
                                ASw.WriteLine("INDEX {0} ", indexName);
                                ASw.WriteLine("   ON {0}", ATable.strName);
                            }

                            string fields = "";

                            foreach (TIndexField indfield in index.grpIndexField)
                            {
                                if (fields.Length > 0)
                                {
                                    fields += ",";
                                }

                                fields += indfield.strName;
                            }

                            if (includeIndexes == eInclude.eInCreateTable)
                            {
                                ASw.Write("   ({0})", fields);
                            }
                            else
                            {
                                ASw.WriteLine("   ({0});", fields);
                            }
                        }
                        else
                        {
                            ASw.WriteLine("DROP INDEX IF EXISTS {0} CASCADE;", indexName);
                        }
                    }
                }
            }
        }
    }
}
