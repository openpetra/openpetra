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
using Ict.Common.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Common;

namespace Ict.Tools.DBXML
{
    /// <summary>
    /// all information about the structure of a database table
    /// </summary>
    public class TTable
    {
        /// <summary>
        /// the columns of the table
        /// </summary>
        public List <TTableField>grpTableField;

        /// <summary>
        /// the indexes in the table
        /// </summary>
        public List <TIndex>grpIndex;

        /// <summary>
        /// the constraints (foreign, unique, primary) of the table
        /// </summary>
        public List <TConstraint>grpConstraint;

        /// <summary>
        /// the name of the table (as it is in the database)
        /// </summary>
        public string strName;

        /// <summary>
        /// order of the table
        /// </summary>
        public int iOrder;

        /// <summary>
        /// the name of the table as it should be used in dotnet
        /// </summary>
        public string strDotNetName;

        /// the name of the table when it is used in a dataset; empty otherwise.
        public string strVariableNameInDataset = null;

        /// <summary>
        /// the name used for dumping the table
        /// </summary>
        public string strDumpName;

        /// <summary>
        /// a description of the table
        /// </summary>
        public string strDescription;

        /// <summary>
        /// define which area in the db this table belongs to
        /// </summary>
        public string strArea;

        /// <summary>
        /// even shorter description than strDescription
        /// </summary>
        public string strLabel;

        /// <summary>
        /// is there a label at all?
        /// </summary>
        public Boolean ExistsStrLabel;

        /// <summary>
        /// each table belongs to a certain group, which helps with the HTML documentation of the database structure
        /// </summary>
        public string strGroup;

        /// created / modified fields
        public bool bWithoutCRMDFields;

        /// attempt an insert instead of failed update in SubmitChanges in DataAccess
        public bool bCatchUpdateException;

        /// references to this table from other tables list of TConstraints
        public ArrayList FReferenced;

        /// <summary>
        /// this for debugging; it helps to find columns that are missing
        /// </summary>
        public static Boolean GEnabledLoggingMissingFields = true;

        /// <summary>
        /// constructor
        /// </summary>
        public TTable()
        {
            FReferenced = new ArrayList();
            grpTableField = new List <TTableField>();
            grpConstraint = new List <TConstraint>();
            grpIndex = new List <TIndex>();
        }

        /// <summary>
        /// copy the values from another table;
        /// used for generating datasets by deriving from database tables
        /// </summary>
        /// <param name="AOtherTable"></param>
        public void Assign(TTable AOtherTable)
        {
            foreach (TTableField f in AOtherTable.grpTableField)
            {
                this.grpTableField.Add(f);
            }

            foreach (TIndex i in AOtherTable.grpIndex)
            {
                this.grpIndex.Add(i);
            }

            foreach (TConstraint c in AOtherTable.grpConstraint)
            {
                this.grpConstraint.Add(c);
            }

            this.iOrder = AOtherTable.iOrder;
            this.strName = AOtherTable.strName;
            this.strDotNetName = AOtherTable.strDotNetName;
            this.strVariableNameInDataset = AOtherTable.strVariableNameInDataset;
            this.strDumpName = AOtherTable.strDumpName;
            this.strDescription = AOtherTable.strDescription;
            this.strArea = AOtherTable.strArea;
            this.strLabel = AOtherTable.strLabel;
            this.ExistsStrLabel = AOtherTable.ExistsStrLabel;
            this.strGroup = AOtherTable.strGroup;
            this.bWithoutCRMDFields = AOtherTable.bWithoutCRMDFields;
            this.bCatchUpdateException = AOtherTable.bCatchUpdateException;
        }

        /// <summary>
        /// change the name from the sql name to a Delphi class name
        /// remove underscores, use capitalised letters
        /// </summary>
        /// <param name="s">turn the table name into CamelCase</param>
        /// <returns></returns>
        public static string NiceTableName(string s)
        {
            return StringHelper.UpperCamelCase(s, false, false);
        }

        /// <summary>
        /// change the name from the sql name to a Delphi variable name
        /// remove underscores, remove prefixes and type identifiers,
        /// use capitalised letters
        /// </summary>
        /// <param name="tableField">the reference to the column which you want the nice name of</param>
        /// <returns>the column name without prefix and postfix</returns>
        public static string NiceFieldName(TTableField tableField)
        {
            string ReturnValue;

            // jump over prefix (p_, a_, ...)
            if (tableField.strNameDotNet.Length > 0)
            {
                ReturnValue = tableField.strNameDotNet;
            }
            else
            {
                ReturnValue = NiceFieldName(tableField.strName);
            }

            return ReturnValue;
        }

        /// <summary>
        /// format the name of a column using Camelcase and dropping prefix/postfix
        /// </summary>
        /// <param name="tableField">the name to format</param>
        /// <returns>nice name</returns>
        public static string NiceFieldName(String tableField)
        {
            return StringHelper.UpperCamelCase(tableField, true, true);
        }

        /// <summary>
        /// change the name of the constraint from the sql name to a .net name
        /// remove underscores, use capitalised letters, prepend FK or call it PrimaryKey
        /// </summary>
        /// <param name="c">the constraint</param>
        /// <returns>nice name for the key</returns>
        public static string NiceKeyName(TConstraint c)
        {
            string ReturnValue = "";
            int underscore;
            string s;

            if (c.strType == "primarykey")
            {
                return "PrimaryKey";
            }

            // e.g. a_account_hierarchy_fk1
            if (c.strType == "foreignkey")
            {
                ReturnValue = "FK";
            }
            else if (c.strType == "uniquekey")
            {
                ReturnValue = "UK";
            }

            s = c.strName;

            // jump over prefix (p_, a_, ...)
            underscore = s.IndexOf('_');
            s = s.Substring(underscore + 1);
            underscore = s.IndexOf('_');

            while (underscore != -1)
            {
                ReturnValue = ReturnValue + s.Substring(0, 1).ToUpper() + s.Substring(1, underscore - 1);
                s = s.Substring(underscore + 1);
                underscore = s.IndexOf('_');
            }

            // drop last part of the name, only take the number, without the fk
            if (c.strType == "foreignkey")
            {
                underscore = s.IndexOf("fk");
            }
            else if (c.strType == "uniquekey")
            {
                underscore = s.IndexOf("uc");
            }

            return ReturnValue + s.Substring(underscore + 2);
        }

        /// <summary>
        /// get the namespace for this table, eg. MPersonnel.Units
        /// </summary>
        public static string GetNamespace(string AStrGroup)
        {
            String NamespaceTable;

            if (AStrGroup == "partner")
            {
                NamespaceTable = "MPartner.Partner";
            }
            else if (AStrGroup == "mailroom")
            {
                NamespaceTable = "MPartner.Mailroom";
            }
            else if (AStrGroup == "account")
            {
                NamespaceTable = "MFinance.Account";
            }
            else if (AStrGroup == "gift")
            {
                NamespaceTable = "MFinance.Gift";
            }
            else if (AStrGroup == "ap")
            {
                NamespaceTable = "MFinance.AP";
            }
            else if (AStrGroup == "ar")
            {
                NamespaceTable = "MFinance.AR";
            }
            else if (AStrGroup == "personnel")
            {
                NamespaceTable = "MPersonnel.Personnel";
            }
            else if (AStrGroup == "units")
            {
                NamespaceTable = "MPersonnel.Units";
            }
            else if (AStrGroup == "common")
            {
                NamespaceTable = "MCommon";
            }
            else
            {
                NamespaceTable = 'M' + AStrGroup;
            }

            NamespaceTable = NamespaceTable.Replace("Msysman", "MSysMan").
                             Replace("Mconference", "MConference").
                             Replace("Mhospitality", "MHospitality");

            return NamespaceTable;
        }

        /// <summary>
        /// overload, do warn about missing fields (if GEnabledLoggingMissingFields is set)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public TTableField GetField(string s)
        {
            return GetField(s, true);
        }

        /// <summary>
        /// return the reference to a column, given by name
        /// </summary>
        /// <param name="s">name of the column</param>
        /// <param name="AShowWarningNonExistingField">show warning if there is no field with that name;
        /// only takes effect if GEnabledLoggingMissingFields is true</param>
        /// <returns>reference to the column</returns>
        public TTableField GetField(string s, bool AShowWarningNonExistingField)
        {
            TTableField ReturnValue;

            ReturnValue = null;

            foreach (TTableField t in grpTableField)
            {
                if ((t.strName == s) || (t.strNameDotNet == s))
                {
                    return t;
                }
            }

            if (ReturnValue == null)
            {
                // should be disabled for creating the diff between two Petra versions.
                if ((GEnabledLoggingMissingFields) && (s != "s_modification_id_t") && AShowWarningNonExistingField)
                {
                    System.Console.WriteLine("Warning: TTable.GetField: cannot find field " + strName + '.' + s);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// get the reference to the constraint which is based on the columns that are passed as parameter
        /// </summary>
        /// <param name="fields">the fields that are part of the constraint that we look for</param>
        /// <returns>the reference to the constraint</returns>
        public TConstraint GetConstraint(StringCollection fields)
        {
            Boolean same;

            foreach (TConstraint c in grpConstraint)
            {
                same = true;

                if (fields.Count != c.strThisFields.Count)
                {
                    same = false;
                }

                foreach (string s in fields)
                {
                    if (c.strThisFields.IndexOf(s) == -1)
                    {
                        same = false;
                    }
                }

                if (same)
                {
                    return c;
                }
            }

            return null;
        }

        /// <summary>
        /// determine if a given field is part of a certain type of constraint
        /// </summary>
        /// <param name="AFieldname">the field in question</param>
        /// <param name="AKeyType">a type of constraint, eg. unique, primary, foreign key</param>
        /// <returns>true if the field is part of a key of the given type</returns>
        public Boolean IsKey(String AFieldname, String AKeyType)
        {
            foreach (TConstraint c in grpConstraint)
            {
                if ((c.strType == AKeyType) && (c.strThisFields.Contains(AFieldname)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// determine whether the table has a primary key
        /// </summary>
        /// <returns>true if the table has a primary key constraint</returns>
        public bool HasPrimaryKey()
        {
            foreach (TConstraint constr in grpConstraint)
            {
                if (constr.strType == "primarykey")
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// determine whether the table has a unique key
        /// </summary>
        /// <returns>true if the table has a unique key constraint</returns>
        public bool HasUniqueKey()
        {
            int NumberOfUniqueKeys = 0;

            foreach (TConstraint constr in grpConstraint)
            {
                if (constr.strType == "uniquekey")
                {
                    NumberOfUniqueKeys++;
                }
            }

            if (NumberOfUniqueKeys > 1)
            {
                throw new Exception("Currently we only support one unique key per DB Table!");
            }

            return NumberOfUniqueKeys > 0;
        }

        /// <summary>
        /// determine whether the table has any foreign key constraints
        /// </summary>
        /// <returns>true if the table has at least one foreign key constraint</returns>
        public bool HasForeignKey()
        {
            foreach (TConstraint constr in grpConstraint)
            {
                if (constr.strType == "foreignkey")
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// get a reference to the primary key
        /// </summary>
        /// <returns>the primary key of the table</returns>
        public TConstraint GetPrimaryKey()
        {
            foreach (TConstraint c in grpConstraint)
            {
                if (c.strType == "primarykey")
                {
                    return c;
                }
            }

            throw new Exception("cannot find primary key of table " + this.strName);
        }

        /// <summary>
        /// get a reference to the first unique key
        /// </summary>
        /// <returns>the first unique key of the table</returns>
        public TConstraint GetFirstUniqueKey()
        {
            foreach (TConstraint c in grpConstraint)
            {
                if (c.strType == "uniquekey")
                {
                    return c;
                }
            }

            throw new Exception("cannot find a unique key of table " + this.strName);
        }

        /// <summary>
        /// get the list of constraints of other tables, that reference this table
        /// </summary>
        /// <returns>the list of constraints of other tables, that reference this table</returns>
        public ArrayList GetReferences()
        {
            return FReferenced;
        }

        /// <summary>
        /// add a foreign key from another table, that is referencing this table
        /// </summary>
        /// <param name="AForeignKey">foreign key to add</param>
        public void AddReference(TConstraint AForeignKey)
        {
            FReferenced.Add(AForeignKey);
        }

        /// <summary>
        /// set the references in the tables referenced by the constraints of this table
        /// </summary>
        /// <param name="db"></param>
        public void PrepareLinks(TDataDefinitionStore db)
        {
            TIndex index;
            TIndexField indexfield;

            grpConstraint.Sort(new ConstraintComparer());

            foreach (TConstraint constr in grpConstraint)
            {
                if (constr.strType == "foreignkey")
                {
                    constr.strThisTable = strName;
                    db.GetTable(constr.strOtherTable).AddReference(constr);
                }

                // indexes
                if (constr.strThisFields.Count <= 0)
                {
                    Console.WriteLine("*** Field list contains no fields in key " + constr.strName + " of table " + strName);
                    Environment.Exit(1);
                }

                if (constr.strType == "primarykey")
                {
                    index = new TIndex();
                    index.strName = TIndex.MakeIndexName(constr.strName, "Table " + strName + ", Primary key constraint " + constr.strName, "");
                    index.bUnique = true;
                    index.bPrimary = true;
                    index.bImplicit = true;

                    foreach (string s in constr.strThisFields)
                    {
                        indexfield = new TIndexField();
                        indexfield.strName = s;
                        indexfield.strOrder = "ascending";
                        index.grpIndexField.Add(indexfield);
                    }

                    AddIndex(index);
                }

                if (constr.strType == "foreignkey")
                {
                    if (constr.strOtherFields.Count <= 0)
                    {
                        Console.WriteLine("*** Field list contains no remote fields in key " + constr.strName + " of table " + strName);
                        Environment.Exit(1);
                    }

                    // Register the Indexes that we need
                    // For the refering table (for the prohitchangeofprimarykey!)
                    index = new TIndex();
                    index.strName = TIndex.MakeIndexName(constr.strName, "Table " + strName + ", Foreign key constraint " + constr.strName, "_key");
                    index.bUnique = false;
                    index.bPrimary = false;
                    index.bImplicit = true;

                    foreach (string s in constr.strThisFields)
                    {
                        indexfield = new TIndexField();
                        indexfield.strName = s;
                        indexfield.strOrder = "ascending";
                        index.grpIndexField.Add(indexfield);
                    }

                    AddIndex(index);

                    // For the refered table (for the write triggers)
                    index = new TIndex();
                    index.strName = TIndex.MakeIndexName(constr.strName, "Table " + strName + ", Foreign key constraint " + constr.strName, "_ref");
                    index.bUnique = false;
                    index.bPrimary = false;
                    index.bImplicit = true;

                    foreach (string s in constr.strOtherFields)
                    {
                        indexfield = new TIndexField();
                        indexfield.strName = s;
                        indexfield.strOrder = "ascending";
                        index.grpIndexField.Add(indexfield);
                    }

                    db.GetTable(constr.strOtherTable).AddIndex(index);
                }

                if (constr.strType == "uniquekey")
                {
                    index = new TIndex();
                    index.strName = TIndex.MakeIndexName(constr.strName, "Table " + strName + ", Unique key constraint " + constr.strName, "");
                    index.bUnique = true;

                    // only true, if there is no primary key; normally the primary key should give the primary index
                    index.bPrimary = false;
                    index.bImplicit = true;

                    foreach (string s in constr.strThisFields)
                    {
                        indexfield = new TIndexField();
                        indexfield.strName = s;
                        indexfield.strOrder = "ascending";
                        index.grpIndexField.Add(indexfield);
                    }

                    AddIndex(index);
                }
            }
        }

        /// <summary>
        /// create the auto generated fields (eg. createdby, datemodified)
        /// </summary>
        public void PrepareAutoGeneratedFields()
        {
            TTableField field;
            TConstraint constr;

            if ((!this.bWithoutCRMDFields) == true)
            {
                // created_by etc are not anymore in the petra.xml, therefore they can be all added here
                field = new TTableField();
                field.strName = "s_date_created_d";
                field.strTableName = this.strName;
                field.strType = "date";
                field.strDescription = "The date the record was created.";
                field.strFormat = "99/99/9999";
                field.strDefault = "SYSDATE";
                field.ExistsStrInitialValue = true;
                field.strInitialValue = "TODAY";
                field.strLabel = "Created Date";
                field.ExistsStrLabel = true;

                // bNotNull: row required for historic data
                field.bNotNull = false;
                field.iOrder = grpTableField.Count;
                grpTableField.Add(field);
                field = new TTableField();
                field.strName = "s_created_by_c";
                field.strTableName = this.strName;
                field.strType = "varchar";
                field.strDescription = "User ID of who created this record.";
                field.strFormat = "X(10)";
                field.iLength = 20;
                field.iCharLength = 10;

                // bNotNull: row required for historic data
                field.bNotNull = false;
                field.strLabel = "Created By";
                field.ExistsStrLabel = true;
                field.strInitialValue = "";
                field.ExistsStrInitialValue = true;
                field.iOrder = grpTableField.Count;
                grpTableField.Add(field);

                if (this.strName != "s_user")
                {
                    // avoid circular reference inside table. MySQL does not like that
                    constr = new TConstraint();
                    constr.strName = this.strName + "_fkcr";
                    constr.strType = "foreignkey";
                    constr.strThisFields = StringHelper.StrSplit("s_created_by_c", ",");
                    constr.strOtherTable = "s_user";
                    constr.strOtherFields = StringHelper.StrSplit("s_user_id_c", ",");
                    grpConstraint.Add(constr);
                }

                field = new TTableField();
                field.strName = "s_date_modified_d";
                field.strTableName = this.strName;
                field.strType = "date";
                field.strDescription = "The date the record was modified.";
                field.strFormat = "99/99/9999";
                field.bNotNull = false;

                // no default date, because when the record is created, the date modified should be NULL
                field.strLabel = "Modified Date";
                field.ExistsStrLabel = true;
                field.iOrder = grpTableField.Count;
                grpTableField.Add(field);
                field = new TTableField();
                field.strName = "s_modified_by_c";
                field.strTableName = this.strName;
                field.strType = "varchar";
                field.strDescription = "User ID of who last modified this record.";
                field.strFormat = "X(10)";
                field.iLength = 20;
                field.iCharLength = 10;
                field.bNotNull = false;
                field.strLabel = "Modified By";
                field.ExistsStrLabel = true;
                field.strInitialValue = "";
                field.ExistsStrInitialValue = true;
                field.iOrder = grpTableField.Count;
                grpTableField.Add(field);

                if (this.strName != "s_user")
                {
                    // avoid circular reference inside table. MySQL does not like that
                    constr = new TConstraint();
                    constr.strName = this.strName + "_fkmd";
                    constr.strType = "foreignkey";
                    constr.strThisFields = StringHelper.StrSplit("s_modified_by_c", ",");
                    constr.strOtherTable = "s_user";
                    constr.strOtherFields = StringHelper.StrSplit("s_user_id_c", ",");
                    grpConstraint.Add(constr);
                }
            }

            field = new TTableField();
            field.strName = "s_modification_id_t";
            field.strTableName = this.strName;
            field.strType = "timestamp";
            field.strDescription = "This identifies the current version of the record.";
            field.bNotNull = false;
            field.iOrder = grpTableField.Count;
            grpTableField.Add(field);
        }

        /** checks if all the fields of the index exist in the table
         */
        public Boolean TestIndex(TIndex AIndex)
        {
            foreach (TIndexField indexfield in AIndex.grpIndexField)
            {
                if (GetField(indexfield.strName).strName != indexfield.strName)
                {
                    Console.WriteLine(
                        "ERROR: Index " + AIndex.strName + " in table " + strName + " refers to a non existing field " + indexfield.strName);
                    Environment.Exit(1);
                }
            }

            return true;
        }

        /** only add the index if it does not exist yet
         */
        public Boolean AddIndex(TIndex AIndex)
        {
            foreach (TIndex i in grpIndex)
            {
                if (i.Similar(AIndex) == true)
                {
                    // Found a similar index
                    i.aliases.Add(AIndex.strName);
                    return false;
                }
            }

            // We might still not want to add this index if a tighter index already exists...
            if (!AIndex.bUnique)
            {
                AIndex.bUnique = true;

                foreach (TIndex i in grpIndex)
                {
                    if (i.Similar(AIndex) == true)
                    {
                        // Found a similar index
                        AIndex.bUnique = false;
                        i.aliases.Add(AIndex.strName);
                        return false;
                    }
                }

                AIndex.bUnique = false;
            }

            // We might still not want to add this index if an index already exists that just has other sorting orders
            foreach (TIndex i in grpIndex)
            {
                if ((i.SimilarFields(AIndex, false) == true) && (i.bUnique == AIndex.bUnique))
                {
                    // Found a similar index
                    i.aliases.Add(AIndex.strName);
                    return false;
                }
            }

            // test if all the fields of the index exist
            if (!TestIndex(AIndex))
            {
                return false;
            }

            grpIndex.Add(AIndex);
            return true;
        }
    }

    /// <summary>
    /// sort the tables by dependency of the constraints, using topological sort.
    /// in the end, we get a list of tables, in the order that you need when you populate the database with constraints enabled.
    /// first the tables that depend on nothing, and then the tables that depend on them.
    /// deleting the database can be done the other way round
    /// </summary>
    public class TTableSort
    {
        private static TTable GetTableWithoutUnsatisfiedDependancies(List <TTable>ASortedList, List <TTable>AUnsortedTables)
        {
            foreach (TTable t in AUnsortedTables)
            {
                bool unsatisfiedDependancy = false;

                // find a table that has no dependancies on Tables that are still in ATables
                foreach (TConstraint c in t.grpConstraint)
                {
                    if ((c.strType == "foreignkey") && (c.strOtherTable != t.strName))
                    {
                        foreach (TTable t2 in AUnsortedTables)
                        {
                            if (t2.strName == c.strOtherTable)
                            {
                                unsatisfiedDependancy = true;
                            }
                        }
                    }
                }

                if (!unsatisfiedDependancy)
                {
                    return t;
                }
            }

            return null;
        }

        private static Int32 GetTableIndex(List <TTable>ATables, string ADBName)
        {
            Int32 i = 0;

            foreach (TTable t in ATables)
            {
                if (t.strName == ADBName)
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        /// <summary>
        /// sort by dependancies
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="AOrigTables"></param>
        /// <returns></returns>
        public static List <TTable>TopologicalSort(TDataDefinitionStore AStore, List <TTable>AOrigTables)
        {
            List <TTable>Result = new List <TTable>();
            List <TTable>Tables = new List <TTable>(AOrigTables);

            // manually put s_user first. it does depend on p_partner, but we are not using that at the moment
            Result.Add(Tables[GetTableIndex(Tables, "s_user")]);
            Tables.RemoveAt(GetTableIndex(Tables, "s_user"));

            while (Tables.Count > 0)
            {
                TTable t = GetTableWithoutUnsatisfiedDependancies(Result, Tables);

                if (t == null)
                {
                    string msg = string.Empty;

                    foreach (TTable t2 in Tables)
                    {
                        msg += t2.strName + ";";
                    }

                    throw new Exception("Problem with TopologicalSort, tables " + msg);
                }

                Result.Add(t);
                Tables.Remove(t);
            }

            return Result;
        }
    }

    /// <summary>
    /// this describes a column of a table
    /// </summary>
    public class TTableField
    {
        /// <summary>
        /// the name of the column
        /// </summary>
        public string strName;

        /// <summary>
        /// the name of the table, that this column belongs to
        /// </summary>
        public string strTableName;

        /// <summary>
        /// SQL type of the column
        /// </summary>
        public string strType;

        /// <summary>
        /// .net type that should be used for this column
        /// </summary>
        public string strTypeDotNet;

        /// <summary>
        /// name that should be used in generated code for this column
        /// </summary>
        private string FNameDotNet;

        /// <summary>
        /// property for dynamically calculating the .net name for the column
        /// </summary>
        public string strNameDotNet
        {
            set
            {
                FNameDotNet = value;
            }
            get
            {
                if ((FNameDotNet == null) || (FNameDotNet.Length == 0))
                {
                    return TTable.NiceFieldName(strName);
                }

                return FNameDotNet;
            }
        }

        /// <summary>
        /// help for this column for the user
        /// </summary>
        public string strHelp;

        /// <summary>
        /// label for this column; could be used by GUI generator
        /// </summary>
        public string strLabel;

        /// <summary>
        /// description of this column
        /// </summary>
        public string strDescription;

        /// <summary>
        /// special check for enforcing certain conditions; not used at the moment
        /// </summary>
        public String strCheck;

        /// the order of this field in the table
        public Int32 iOrder;

        /// is this field part of the primary key of the table
        public bool bPartOfPrimKey = false;

        /// is this field part of the first unique key of the table
        public bool bPartOfFirstUniqueKey = false;

        /// name of the sequence that is used to fill this field
        public string strSequence;

        /// this is calculated from strInitialValue and strFormat
        public string strDefault;

        /// this is taken from petra.xml
        public string strInitialValue;

        /// <summary>
        /// whether there exists an initial value
        /// </summary>
        public Boolean ExistsStrInitialValue;

        /// <summary>
        /// whether there exists a label for the column when it is displayed in a grid
        /// </summary>
        public Boolean ExistsStrColLabel;

        /// <summary>
        /// whether there exists a label for this column (eg. in front of a text field)
        /// </summary>
        public Boolean ExistsStrLabel;

        /// <summary>
        /// is there a help text?
        /// </summary>
        public Boolean ExistsStrHelp;

        /// <summary>
        /// has a check been defined?
        /// </summary>
        public Boolean ExistsStrCheck;

        /// <summary>
        /// a format; used to be in Progress notation
        /// </summary>
        public string strFormat;

        /// number of characters; if it does not exists, it is calculated from strFormat
        public Int32 iCharLength;

        /// <summary>
        /// length in byte?
        /// </summary>
        public int iLength;

        /// <summary>
        /// how many decimals does the number have (allowed places after the decimal point)
        /// </summary>
        public int iDecimals;

        /// <summary>
        /// will this field automatically have a unique value
        /// </summary>
        public Boolean bAutoIncrement;

        /// <summary>
        /// can this field be null?
        /// </summary>
        public Boolean bNotNull;

        /// <summary>
        /// a label that is used when this column is displayed in a grid; can be used for GUI generation
        /// </summary>
        public string strColLabel;

        /// <summary>
        /// can be used to define input validation (not used at the moment)
        /// </summary>
        public string strValExp;

        /// <summary>
        /// can be used to define input validation, and show an error message (not used at the moment)
        /// </summary>
        public string strValMsg;

        /// <summary>
        /// constructor
        /// </summary>
        public TTableField()
        {
            strNameDotNet = "";
            strTypeDotNet = "";
            strValExp = "";
            strValMsg = "";
            strSequence = "";
            strDefault = "";
            strHelp = "";
            strLabel = "";
            ExistsStrInitialValue = false;
            ExistsStrColLabel = false;
            ExistsStrLabel = false;
            ExistsStrHelp = false;
            iLength = -1;
            iDecimals = -1;
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="t"></param>
        public TTableField(TTableField t)
        {
            strName = t.strName;
            strTableName = t.strTableName;
            strType = t.strType;
            strTypeDotNet = t.strTypeDotNet;
            strNameDotNet = t.strNameDotNet;
            strHelp = t.strHelp;
            strLabel = t.strLabel;
            strDescription = t.strDescription;
            strCheck = t.strCheck;
            iOrder = t.iOrder;
            strSequence = t.strSequence;
            strDefault = t.strDefault;
            strInitialValue = t.strInitialValue;
            ExistsStrInitialValue = t.ExistsStrInitialValue;
            ExistsStrColLabel = t.ExistsStrColLabel;
            ExistsStrLabel = t.ExistsStrLabel;
            ExistsStrHelp = t.ExistsStrHelp;
            ExistsStrCheck = t.ExistsStrCheck;
            strFormat = t.strFormat;
            iCharLength = t.iCharLength;
            iLength = t.iLength;
            iDecimals = t.iDecimals;
            bNotNull = t.bNotNull;
            strColLabel = t.strColLabel;
            strValExp = t.strValExp;
            strValMsg = t.strValMsg;
        }

        /// <summary>
        /// get the fitting type in dot net for the field
        /// </summary>
        /// <returns></returns>
        public string GetDotNetType()
        {
            if ((strTypeDotNet != null) && (strTypeDotNet.Length > 0))
            {
                return strTypeDotNet;
            }

            if (strType.ToLower() == "integer")
            {
                return "Int32";
            }
            else if (strType.ToLower() == "rowid")
            {
                return "String";
            }
            else if (strType.ToLower() == "varchar")
            {
                return "String";
            }
            else if (strType.ToLower() == "bit")
            {
                return "Boolean";
            }
            else if ((strType.ToLower() == "number") && (iLength == 24))
            {
                return "Decimal";  // 'currency'
            }
            else if ((strType.ToLower() == "number") && (iLength == 10))
            {
                return "Int64";
            }
            else if (strType.ToLower() == "number")
            {
                return "Decimal";
            }
            else if (strType.ToLower() == "date")
            {
                if (!bNotNull)
                {
                    return "System.DateTime?";
                }

                return "System.DateTime";
            }
            else if (strType.ToLower() == "boolean")
            {
                return "Boolean";
            }
            else if (strType.ToLower() == "int32")
            {
                return "Int32";
            }
            else if (strType.ToLower() == "int64")
            {
                return "Int64";
            }
            else if (strType.ToLower() == "datetime")
            {
                return "DateTime";
            }
            else if (strType.ToLower() == "timestamp")
            {
                return "DateTime";
            }
            else if (strType.ToLower() == "string")
            {
                return "String";
            }
            else
            {
                return strType;
            }
        }
    }

    /// <summary>
    /// constraints can be foreign keys or primary keys
    /// </summary>
    public class TConstraint
    {
        /// <summary>
        /// name of the constraint
        /// </summary>
        public string strName;

        /// <summary>
        /// kind of constraint: foreign or unique
        /// </summary>
        public string strType;

        /// <summary>
        /// name of the table that this constraint belongs to
        /// </summary>
        public string strThisTable;

        /// <summary>
        /// the fields in this table that are part of the foreign or unique key
        /// </summary>
        public StringCollection strThisFields;

        /// <summary>
        /// the other table that is refered to by this foreign key
        /// </summary>
        public string strOtherTable;

        /// <summary>
        /// the list of fields that are refered to in the other table
        /// </summary>
        public StringCollection strOtherFields;

        /// <summary>
        /// constructor
        /// </summary>
        public TConstraint()
        {
            strThisFields = new StringCollection();
            strOtherFields = new StringCollection();
        }
    }

    /// <summary>
    /// data of an index
    /// </summary>
    public class TIndex
    {
        /// <summary>
        /// name of the index
        /// </summary>
        public String strName;

        /// <summary>
        /// description
        /// </summary>
        public String strDescr;

        /// <summary>
        /// which area this index belongs to
        /// </summary>
        public String strArea;

        /// <summary>
        /// used for primary key index in some DBMS
        /// </summary>
        public bool bPrimary;

        /// <summary>
        /// unique index, no duplicates allowed
        /// </summary>
        public bool bUnique;

        /// implicit = true means: this index can be derived from a key
        public bool bImplicit;

        /// <summary>
        /// the fields that are part of this index
        /// </summary>
        public List <TIndexField>grpIndexField;

        /// list of strings, names of similar indexes (not added)
        public ArrayList aliases;

        /// <summary>
        /// constructor
        /// </summary>
        public TIndex()
        {
            strArea = "";
            strDescr = "";
            grpIndexField = new List <TIndexField>();
            aliases = new ArrayList();
        }

        /// <summary>
        /// compare if two indexes have the same fields, optional checking the sorting order
        /// </summary>
        /// <param name="i">the index to compare with</param>
        /// <param name="checkSortingOrder">should the sorting order be checked as well</param>
        /// <returns>true if same fields are part of each index</returns>
        public bool SimilarFields(TIndex i, bool checkSortingOrder)
        {
            if (this.grpIndexField.Count != i.grpIndexField.Count)
            {
                return false;
            }
            else
            {
                for (Int32 c = 0; c <= this.grpIndexField.Count - 1; c += 1)
                {
                    if ((this.grpIndexField[c].strName != i.grpIndexField[c].strName)
                        || ((checkSortingOrder == true)
                            && (this.grpIndexField[c].strOrder != i.grpIndexField[c].strOrder)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// compare two indexes whether they are the same
        /// </summary>
        /// <param name="i">the index to compare</param>
        /// <returns>true if same fields and uniqueness</returns>
        public bool Similar(TIndex i)
        {
            if (this.bUnique != i.bUnique)
            {
                return false;
            }
            else if (!SimilarFields(i, true))
            {
                return false;
            }

            return true;
        }

        private const Int32 MAX_INDEX_NAME_LEN = 32;
        private const Int32 INDEX_PREFIX_LEN = 4;
        private static SortedList FUsedIndexNames = new SortedList();

        /// <summary>
        /// Progress has a maximum length for the index name
        /// </summary>
        /// <param name="AKeyName"></param>
        /// <param name="AMsg"></param>
        /// <param name="APostfix"></param>
        /// <returns></returns>
        public static String MakeIndexName(String AKeyName, String AMsg, string APostfix)
        {
            String ReturnValue = "inx_" + AKeyName + APostfix;

            if ((ReturnValue.Length) > MAX_INDEX_NAME_LEN)
            {
                // Console.WriteLine(AMsg);
                // Console.Write('    Index name too long: ' + result + ', truncate to ');
                // problem: index names are not unique across the whole database?
                // this seems to be a problem for Postgresql: solution: use full length name, using constraint name
                Int32 RightLen = MAX_INDEX_NAME_LEN - INDEX_PREFIX_LEN;
                Int32 Offset = ReturnValue.Length - RightLen;
                ReturnValue = "inx_" + ReturnValue.Substring(Offset);
            }

            if (FUsedIndexNames.IndexOfKey(ReturnValue) != -1)
            {
                int count = 1;
                string baseName = ReturnValue;

                do
                {
                    count++;
                    ReturnValue = baseName.Substring(0, baseName.Length - count.ToString().Length) + count.ToString();
                } while (FUsedIndexNames.IndexOfKey(ReturnValue) != -1);
            }

            FUsedIndexNames.Add(ReturnValue, ReturnValue);
            return ReturnValue;
        }
    }

    /// <summary>
    /// data of an index
    /// </summary>
    public class TIndexField
    {
        /// <summary>
        /// name of the index
        /// </summary>
        public String strName;

        /// <summary>
        /// ascending or descending order
        /// </summary>
        public String strOrder;
    }

    /// <summary>
    /// all necessary data of a sequence
    /// </summary>
    public class TSequence
    {
        /// <summary>
        /// name of the sequence
        /// </summary>
        public String strName;

        /// <summary>
        /// description for the sequence
        /// </summary>
        public String strDescription;

        /// <summary>
        /// area in the database that this sequence applies to
        /// </summary>
        public String strArea;

        /// <summary>
        /// should the sequence start with 0 once it goes beyond the maximum value
        /// </summary>
        public bool bCycleOnLimit;

        /// <summary>
        ///  minimum value
        /// </summary>
        public System.Int32 iMinVal;

        /// <summary>
        /// maximum value
        /// </summary>
        public System.Int32 iMaxVal;

        /// <summary>
        /// initial value when database is created
        /// </summary>
        public System.Int32 iInitial;

        /// <summary>
        /// increment by this integer each time
        /// </summary>
        public System.Int32 iIncrement;
    }

    /// <summary>
    /// This holds the tables and the sequences of the database structure;
    /// this is an objectorientated representation in memory
    /// of the database structure that was originally defined in XML
    /// </summary>
    public class TDataDefinitionStore
    {
        private List <TTable>tables;
        private List <TSequence>sequences;

        /// <summary>
        /// constructor
        /// </summary>
        public TDataDefinitionStore() : base()
        {
            tables = new List <TTable>();
            sequences = new List <TSequence>();
        }

        /// <summary>
        /// clear all tables and sequences
        /// </summary>
        public void Clear()
        {
            tables.Clear();
            sequences.Clear();
        }

        /// <summary>
        /// get a specific table by name
        /// </summary>
        /// <param name="tableName">the name of the table</param>
        /// <returns>the table object</returns>
        public TTable GetTable(string tableName)
        {
            int counter = 0;

            while (counter < tables.Count)
            {
                TTable element = tables[counter];

                if ((element.strName == tableName.Trim()) || (TTable.NiceTableName(element.strName) == tableName.Trim()))
                {
                    return element;
                }

                counter++;
            }

            return null;
        }

        /// <summary>
        /// get the tables
        /// </summary>
        /// <returns></returns>
        public List <TTable>GetTables()
        {
            return tables;
        }

        /// <summary>
        /// add a table
        /// </summary>
        /// <param name="table"></param>
        public void AddTable(TTable table)
        {
            table.iOrder = tables.Count;
            tables.Add(table);
        }

        /// <summary>
        /// get the sequences
        /// </summary>
        /// <returns></returns>
        public List <TSequence>GetSequences()
        {
            return sequences;
        }

        /// <summary>
        /// add a sequence
        /// </summary>
        /// <param name="sequence"></param>
        public void AddSequence(TSequence sequence)
        {
            sequences.Add(sequence);
        }

        /** set all the references between the tables (table should know which tables reference them)
         */
        public void PrepareLinks()
        {
            foreach (TTable table in tables)
            {
                table.PrepareLinks(this);
            }
        }

        /// <summary>
        /// Prepare the fields that are auto generated (eg. createdby, lastmodified)
        /// </summary>
        public void PrepareAutoGeneratedFields()
        {
            foreach (TTable table in tables)
            {
                table.PrepareAutoGeneratedFields();
            }
        }
    }

    /** this describes a table that is derived from a database table,
     * has additional custom fields, or is completely customised
     */
    public class TDataSetTable : TTable
    {
        /// <summary>
        /// the table name from the sql database
        /// </summary>
        public string tableorig;

        /// <summary>
        /// the CamelCase name of the table
        /// </summary>
        public string tablename;

        /// <summary>
        /// an alias for the table
        /// </summary>
        public string tablealias;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tableorig">the sql name of the table</param>
        /// <param name="tablename">the name of the table in CamelCase</param>
        /// <param name="tablealias">which alias to use for the table</param>
        /// <param name="origtable">this is an instance of a table that should be used as a base</param>
        public TDataSetTable(string tableorig, string tablename, string tablealias, TTable origtable)
        {
            if (origtable != null)
            {
                Assign(origtable);
            }

            this.tableorig = tableorig;
            this.tablename = tablename;
            this.tablealias = tablealias;
        }
    }

    /// <summary>
    /// for comparing constraints for sorting and to test if they are the same
    /// </summary>
    public class ConstraintComparer : IComparer <TConstraint>
    {
        /** Calls CaseInsensitiveComparer.Compare with the parameters
         */
        public System.Int32 Compare(TConstraint x, TConstraint y)
        {
            // order: primarykey, uniquekey, foreignkey

            if (x.strType == y.strType)
            {
                string name1 = x.strName;
                string name2 = y.strName;

                if (name1 == name2)
                {
                    return 0;
                }
                else if ((name1.IndexOf("_fk") != -1) && (name2.IndexOf("_fk") != -1))
                {
                    try
                    {
                        // use the number of the foreign key to compare the keys
                        // _fk1_key
                        StringCollection strCollection = StringHelper.StrSplit(name1, "_");

                        if (strCollection[strCollection.Count - 1] == "key")
                        {
                            name1 = strCollection[strCollection.Count - 2];
                        }
                        else
                        {
                            name1 = strCollection[strCollection.Count - 1];
                        }

                        Int32 nr1 = Convert.ToInt32(name1.Substring(2));
                        strCollection = StringHelper.StrSplit(name2, "_");

                        if (strCollection[strCollection.Count - 1] == "key")
                        {
                            name2 = strCollection[strCollection.Count - 2];
                        }
                        else
                        {
                            name2 = strCollection[strCollection.Count - 1];
                        }

                        Int32 nr2 = Convert.ToInt32(name2.Substring(2));

                        if ((nr1 == -1) || (nr2 == -1) || (nr1 == nr2))
                        {
                            return new CaseInsensitiveComparer().Compare(x.strName, y.strName);
                        }
                        else
                        {
                            return nr1 - nr2;
                        }
                    }
                    catch (Exception)
                    {
                        // Console.WriteLine('One of these Constraint names is strange: ' + (x as TConstraint).strName + ' ' + (y as TConstraint).strName);
                        // is used for the fkcr and fkmd (created and modified foreign key)
                        return new CaseInsensitiveComparer().Compare(x.strName, y.strName);
                    }
                }
                else
                {
                    return new CaseInsensitiveComparer().Compare(x.strName, y.strName);
                }
            }
            else if (x.strType == "primarykey")
            {
                return -1;
            }
            else if (y.strType == "primarykey")
            {
                return 1;
            }
            else if (x.strType == "uniquekey")
            {
                return -1;
            }
            else if (y.strType == "uniquekey")
            {
                return 1;
            }

            return -1;
        }
    }
}