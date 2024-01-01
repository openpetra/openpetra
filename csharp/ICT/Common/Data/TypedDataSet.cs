//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2024 by OM International
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

//using Ict.Common.Types;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;

using Ict.Common;

namespace Ict.Common.Data
{
    /// <summary>
    /// This is the base class for the typed datasets.
    /// It deals with some strange behaviour when Mono and MS.net have to talk to each other.
    /// </summary>
    public class TypedDataSet
    {
        /// <summary>
        /// removes the constraints from the schema; needed for Mono and .Net cooperation
        /// </summary>
        /// <param name="strSchema">the schema to be modified</param>
        /// <returns></returns>
        public static String RemoveConstraintsFromSchema(String strSchema)
        {
            String ReturnValue;

            ReturnValue = strSchema;

            // remove new line character
            ReturnValue = Regex.Replace(ReturnValue, "\\n", "");

            // unique (primary key) constraint
            ReturnValue = Regex.Replace(ReturnValue, "<xs:unique.*unique>", "");

            // foreign key constraint
            ReturnValue = Regex.Replace(ReturnValue, "<xs:keyref.*keyref>", "");

            // msdata:Expression, avoid problems with relationships (see bug 1825)
            ReturnValue = Regex.Replace(ReturnValue, "msdata:Expression=\".*?\"", "");

            return ReturnValue;
        }
    }

    /// <summary>
    /// this defines a constraint, foreign key
    /// </summary>
    [Serializable()]
    public class TTypedConstraint
    {
        /// <summary>
        /// name of the constraint
        /// </summary>
        public String FName;

        /// <summary>
        /// first table involved, the one having the foreign key
        /// </summary>
        public String FTable1;

        /// <summary>
        /// the other table involved, it is refered to by the first table
        /// </summary>
        public String FTable2;

        /// <summary>
        /// the foreign key in the first table
        /// </summary>
        public string[] FKey1;

        /// <summary>
        /// usually the primary key or another unique key of the second table
        /// </summary>
        public string[] FKey2;

        /// <summary>
        /// Create a typed constraint, that is reusable, and helps when deleting tables.
        /// Key2 points to table1 with Key1; e.g. key2 is the foreign key, key1 is a primary key
        /// the constraint is added to table2
        /// This follows the parameter order of System.Data.ConstraintCollection.Add( name, primaryKeyColumn, foreignKeyColumn)
        ///
        /// </summary>
        /// <returns>void</returns>
        public TTypedConstraint(String AName, String ATable1, string[] AKey1, String ATable2, string[] AKey2)
        {
            FName = AName;
            FTable1 = ATable1;
            FTable2 = ATable2;
            FKey1 = AKey1;
            FKey2 = AKey2;
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public TTypedConstraint()
        {
        }
    }

    /// <summary>
    /// this class is derived from TTypedConstraint and has additional settings
    /// whether this relation is enabled or not
    /// </summary>
    [Serializable()]
    public class TTypedRelation : TTypedConstraint
    {
        /// <summary>
        /// do we want constraints to be created for this relation
        /// </summary>
        public bool FCreateConstraints;

        /// <summary>
        /// is this relation enabled
        /// </summary>
        public bool FEnabled;

        /// <summary>
        /// Create a typed relation, that is reusable, and helps when deleting tables.
        /// Table1/Key1 is the parent, Table2/Key2 is the child in the relationship
        ///
        /// </summary>
        /// <returns>void</returns>
        public TTypedRelation(String AName, String ATable1, string[] AKey1, String ATable2, string[] AKey2, bool ACreateConstraints) : base(AName,
                                                                                                                                           ATable1,
                                                                                                                                           AKey1,
                                                                                                                                           ATable2,
                                                                                                                                           AKey2)
        {
            FCreateConstraints = ACreateConstraints;
            FEnabled = true;
        }
    }

    /// <summary>
    /// our own Typed Dataset base class
    /// </summary>
    [Serializable()]
    public abstract class TTypedDataSet : DataSet
    {
        /// <summary>
        /// constraints in this dataset
        /// </summary>
        protected ArrayList FConstraints;

        /// <summary>
        /// relations in this dataset
        /// </summary>
        protected ArrayList FRelations;

        /// <summary>
        /// for initialising the tables
        /// implemented by generated code
        /// </summary>
        protected abstract void InitTables();

        /// <summary>
        /// for initialising the tables, of a specific dataset
        /// implemented by generated code
        /// </summary>
        /// <param name="ds">the dataset</param>
        protected abstract void InitTables(DataSet ds);

        /// <summary>
        /// for initialising the constraints
        /// implemented by generated code
        /// </summary>
        protected abstract void InitConstraints();

        /// <summary>
        /// for initialising the columns
        /// implemented by generated code
        /// </summary>
        public abstract void InitVars();

        private bool FThrowAwayAfterSubmitChanges = false;

        /// <summary>
        /// if you want the dataset to be cleared after submitchanges.
        /// This will increase the speed significantly: no updating of modificationID, no slow AcceptChanges.
        /// </summary>
        public bool ThrowAwayAfterSubmitChanges
        {
            set
            {
                FThrowAwayAfterSubmitChanges = value;

                foreach (DataTable table in this.Tables)
                {
                    if (table is TTypedDataTable)
                    {
                        ((TTypedDataTable)table).ThrowAwayAfterSubmitChanges = value;
                    }
                }
            }

            get
            {
                return FThrowAwayAfterSubmitChanges;
            }
        }

        private bool FDontThrowAwayAfterSubmitChanges = false;

        /// <summary>
        /// if you want no warning about that the datatable should be cleared after submitchanges.
        /// in some cases you must keep the data, eg when you need the new keys after INSERT
        /// </summary>
        public bool DontThrowAwayAfterSubmitChanges
        {
            set
            {
                FDontThrowAwayAfterSubmitChanges = value;

                foreach (DataTable table in this.Tables)
                {
                    if (table is TTypedDataTable)
                    {
                        ((TTypedDataTable)table).DontThrowAwayAfterSubmitChanges = value;
                    }
                }
            }

            get
            {
                return FDontThrowAwayAfterSubmitChanges;
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public TTypedDataSet()
        {
            this.InitClass();
        }

        /// <summary>
        /// constructor with dataset name
        /// </summary>
        /// <param name="ADatasetName">name for the new typed dataset</param>
        public TTypedDataSet(string ADatasetName)
        {
            base.DataSetName = ADatasetName;
            this.InitClass();
            this.EnableRelations();
        }

        /// <summary>
        /// also copy the constraints and relations arrays
        ///
        /// </summary>
        /// <returns>cloned Dataset</returns>
        public override DataSet Clone()
        {
            TTypedDataSet res;
            Int32 i;
            DataTable table;

            // Console.WriteLine('Call TTypedDataSet.Clone ' + Enum(DetermineExecutingCLR()).ToString());
            res = (TTypedDataSet) base.Clone();

            // res.FConstraints := ArrayList.Create(this.FConstraints);
            // res.FRelations := ArrayList.Create(this.FRelations);
            InitConstraints();

            // TODO: need to disable disabled relations?
            // need to remove the tables that are not in the original dataset
            i = 0;

            while (i < res.Tables.Count)
            {
                table = (DataTable)res.Tables[i];

                if (this.Tables.IndexOf(table.TableName) == -1)
                {
                    res.RemoveTable(table.TableName);
                }
                else
                {
                    i = i + 1;
                }
            }

            return res;
        }

        /// <summary>
        /// Initialise the class, calling abstract routines in the right order
        /// </summary>
        public void InitClass()
        {
            // Console.WriteLine('Call TTypedDataSet.InitClass');
            this.EnforceConstraints = true;
            FConstraints = new ArrayList();
            FRelations = new ArrayList();
            this.InitTables();
            this.InitVars();
            this.InitConstraints();
        }

        /// <summary>
        /// make sure that the typed variables are all referencing to the dataset
        /// </summary>
        protected virtual void MapTables()
        {
            InitVars();
            InitConstraints();
        }

        /// <summary>
        /// overload that makes sure that the typed tables are mapped again
        /// </summary>
        /// <param name="ATable"></param>
        public new void Merge(DataTable ATable)
        {
            base.Merge(ATable);
            MapTables();
        }

        /// <summary>
        /// overload that makes sure that the typed tables are mapped again
        /// </summary>
        /// <param name="ADataSet"></param>
        public new void Merge(DataSet ADataSet)
        {
            base.Merge(ADataSet);
            MapTables();
        }

        /// <summary>
        /// overload that makes sure that the typed tables are mapped again
        /// </summary>
        public new void Merge(DataSet ADataSet, bool APreserveChanges)
        {
            base.Merge(ADataSet, APreserveChanges);
            MapTables();
        }

        /// <summary>
        /// This returns an array of DataColumns, based on a Array of strings of column names of a given table
        ///
        /// </summary>
        /// <returns>void</returns>
        public DataColumn[] GetDataColumnArrayFromString(DataTable ATable, string[] AKeys)
        {
            DataColumn[] ReturnValue = new DataColumn[AKeys.Length];
            System.Int32 Counter;

            Counter = 0;

            foreach (String s in AKeys)
            {
                ReturnValue[Counter] = ATable.Columns[s];
                Counter = Counter + 1;
            }

            return ReturnValue;
        }

        /// <summary>
        /// enable a specific constraint
        ///
        /// </summary>
        /// <returns>void</returns>
        public void EnableConstraint(TTypedConstraint AConstraint)
        {
            DataTable Table1;
            DataTable Table2;

            Table1 = Tables[AConstraint.FTable1];
            Table2 = Tables[AConstraint.FTable2];

            if ((Table1 != null) && (Table2 != null))
            {
                if ((!Table2.Constraints.Contains(AConstraint.FName)))
                {
                    if ((Table1.Rows.Count != 0) && (Table2.Rows.Count != 0) || (Table1.Rows.Count == 0) && (Table2.Rows.Count == 0))
                    {
                    }

                    // ADDING OF CONSTRAINTS TEMPORARILY DISABLED TO BE ABLE TO TRANSFER PCHURCH AND PORGANISTAION TYPED TABLES...
                    // Table2.Constraints.Add(AConstraint.FName,
                    // GetDataColumnArrayFromString(Table1, AConstraint.FKey1),
                    // GetDataColumnArrayFromString(Table2, AConstraint.FKey2));
                }
                else
                {
                    // the constraint exists; does it need to be disabled?
                    if (!((Table1.Rows.Count != 0) && (Table2.Rows.Count != 0) || (Table1.Rows.Count == 0) && (Table2.Rows.Count == 0)))
                    {
                        Table2.Constraints.Remove(AConstraint.FName);
                    }
                }
            }
        }

        /// <summary>
        /// Will add the constraints to the tables.
        /// The constraints should have been before added to FConstraints.
        /// Constraints will only be added if they don't exist already
        ///
        /// </summary>
        /// <returns>void</returns>
        public void EnableConstraints()
        {
            foreach (TTypedConstraint constr in FConstraints)
            {
                EnableConstraint(constr);
            }
        }

        /// <summary>
        /// disable all the constraints
        /// </summary>
        public void DisableConstraints()
        {
            if (FConstraints != null)
            {
                foreach (TTypedConstraint constr in FConstraints)
                {
                    DisableConstraint(constr.FName);
                }
            }
        }

        /// <summary>
        /// Enable a specific constraint
        ///
        /// </summary>
        /// <returns>void</returns>
        public void EnableConstraint(String AName)
        {
            foreach (TTypedConstraint constr in FConstraints)
            {
                if (constr.FName == AName)
                {
                    EnableConstraint(constr);
                }
            }
        }

        /// <summary>
        /// Disable a specific constraint
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DisableConstraint(String AName)
        {
            DataTable Table2;

            foreach (TTypedConstraint constr in FConstraints)
            {
                if (constr.FName == AName)
                {
                    Table2 = Tables[constr.FTable2];

                    if (Table2 != null)
                    {
                        if (Table2.Constraints.IndexOf(AName) != -1)
                        {
                            Table2.Constraints.Remove(AName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Disable all constraints created because of a relation
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DisableRelationConstraints()
        {
            DataTable Table2;

            foreach (TTypedRelation Relation in FRelations)
            {
                Table2 = Tables[Relation.FTable2];

                if (Table2 != null)
                {
                    if (Table2.Constraints.IndexOf(Relation.FName) != -1)
                    {
                        Table2.Constraints.Remove(Relation.FName);
                    }
                }
            }
        }

        /// <summary>
        /// enable a specific relation
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void EnableRelation(TTypedRelation ARelation)
        {
            DataTable Table1;
            DataTable Table2;

            Table1 = Tables[ARelation.FTable1];
            Table2 = Tables[ARelation.FTable2];

            if ((Table1 != null) && (Table2 != null))
            {
//MessageBox.Show("Enabling Relation: " + ARelation.FName + " (" + ARelation.FTable1.ToString() + "; " + ARelation.FTable2.ToString() + ")...");
//MessageBox.Show("Relation Keys: " + ARelation.FName + " (" + ARelation.FKey1[0].ToString() + "; " + ARelation.FKey2[0].ToString() + ")");
                if ((!Relations.Contains(ARelation.FName)))
                {
                    Relations.Add(ARelation.FName,
                        GetDataColumnArrayFromString(Table1, ARelation.FKey1),           // parentcolumn
                        GetDataColumnArrayFromString(Table2, ARelation.FKey2),           // childcolumn
                        ARelation.FCreateConstraints);

//                    MessageBox.Show("Enabled Relation: " + ARelation.FName + " (" + ARelation.FKey1.ToString() + "; " + ARelation.FKey2.ToString());
                }
            }
        }

        /// <summary>
        /// Will add the relations to the tables.
        /// The relations should have been before added to FRelations.
        /// Relations will only be added if they don't exist already
        ///
        /// </summary>
        /// <returns>void</returns>
        public void EnableRelations()
        {
            foreach (TTypedRelation Relation in FRelations)
            {
                if (Relation.FEnabled)
                {
//                  MessageBox.Show("Enabling Relation: " + Relation.FName);
                    EnableRelation(Relation);
                }
            }
        }

        /// <summary>
        /// Enable a specific relation
        ///
        /// </summary>
        /// <returns>void</returns>
        public void EnableRelation(String AName)
        {
            foreach (TTypedRelation Relation in FRelations)
            {
                if (Relation.FName == AName)
                {
                    Relation.FEnabled = true;
                    EnableRelation(Relation);
                }
            }
        }

        /// <summary>
        /// Disable a specific relation
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DisableRelation(String AName)
        {
            DataTable Table2;

            foreach (TTypedRelation Relation in FRelations)
            {
                if (Relation.FName == AName)
                {
                    Relation.FEnabled = false;
                    Table2 = Tables[Relation.FTable2];

                    if (Table2 != null)
                    {
                        if (Relations.IndexOf(AName) != -1)
                        {
                            Relations.Remove(AName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// returns a dataset with only necessary data
        /// depending on the parameter removeEmptyTables, the empty tables are not returned
        /// all rows that are not changed are not returned
        /// </summary>
        /// <returns>a new dataset with only modified data
        /// </returns>
        public virtual TTypedDataSet GetChangesTyped(Boolean removeEmptyTables)
        {
            TTypedDataSet ds = (TTypedDataSet) base.GetChanges();

            if (ds == null)
            {
                return null;
            }

            ds.InitVars();
            ds.MapTables();

            ds.ThrowAwayAfterSubmitChanges = ThrowAwayAfterSubmitChanges;
            ds.DontThrowAwayAfterSubmitChanges = DontThrowAwayAfterSubmitChanges;

            // need to copy over the enabled/disabled status of relations
            foreach (TTypedRelation relNew in ds.FRelations)
            {
                foreach (TTypedRelation relOrig in this.FRelations)
                {
                    if (relOrig.FName == relNew.FName)
                    {
                        relNew.FEnabled = relOrig.FEnabled;
                    }
                }
            }

            if (removeEmptyTables == true)
            {
                ds.RemoveEmptyTables();
            }

            // TODO: REMOVING OF EMPTY FIELDS NOT DONE BECAUSE WE CURRENTLY CAN'T HANDLE THIS PROPERLY ON THE SERVER SIDE...

            ds.EnableConstraints();
            ds.EnableRelations();
            ds.EnforceConstraints = true;
            return ds;
        }

        /// <summary>
        /// Remove a table fromt the dataset, and all constraints and references refering to it
        /// </summary>
        /// <param name="ATableName">table to remove</param>
        public void RemoveTable(String ATableName)
        {
            DataTable TableToDelete;

            // remove all constraints referencing the table that should be deleted
            foreach (TTypedConstraint constr in FConstraints)
            {
                if ((constr.FTable1 == ATableName) || (constr.FTable2 == ATableName))
                {
                    TableToDelete = Tables[constr.FTable2];

                    if (TableToDelete != null)
                    {
                        if (TableToDelete.Constraints.IndexOf(constr.FName) != -1)
                        {
                            TableToDelete.Constraints.Remove(constr.FName);
                        }
                    }
                }
            }

            // remove all relations referencing the table that should be deleted
            foreach (TTypedRelation relation in FRelations)
            {
                if ((relation.FTable1 == ATableName) || (relation.FTable2 == ATableName))
                {
                    TableToDelete = Tables[relation.FTable2];

                    if (TableToDelete != null)
                    {
                        if (Relations.IndexOf(relation.FName) != -1)
                        {
                            Relations.Remove(relation.FName);
                        }

                        if (TableToDelete.Constraints.IndexOf(relation.FName) != -1)
                        {
                            TableToDelete.Constraints.Remove(relation.FName);
                        }
                    }
                }
            }

            TableToDelete = Tables[ATableName];

            if (TableToDelete != null)
            {
                // remove the table itself
                Tables.Remove(TableToDelete);
            }
        }

        /// <summary>
        /// remove a list of tables from the dataset (StringCollection)
        /// </summary>
        /// <param name="ATableNames">names of tables to be removed</param>
        public void RemoveTables(StringCollection ATableNames)
        {
            foreach (String TableName in ATableNames)
            {
                RemoveTable(TableName);
            }
        }

        /// <summary>
        /// remove a list of tables from the dataset (array of string)
        /// </summary>
        /// <param name="ATableNames">names of tables to be removed</param>
        public void RemoveTables(string[] ATableNames)
        {
            foreach (String TableName in ATableNames)
            {
                RemoveTable(TableName);
            }
        }

        /// <summary>
        /// remove all tables from the dataset that are empty
        /// </summary>
        public void RemoveEmptyTables()
        {
            System.Data.DataTable CurrentTable;
            System.Int32 TableCount = 0;

            while (TableCount != Tables.Count)
            {
                CurrentTable = (System.Data.DataTable)(Tables[TableCount]);

                if (CurrentTable.Rows.Count == 0)
                {
                    RemoveTable(CurrentTable.TableName);
                    TableCount = 0;
                }
                else
                {
                    TableCount++;
                }
            }
        }
    }
}
