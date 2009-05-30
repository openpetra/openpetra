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
using System.Data;
using System.Data.Odbc;
using System.Runtime.Serialization;

namespace Ict.Common.Data
{
    /// <summary>
    /// This is the base class for the typed datatables.
    /// </summary>
    [Serializable()]
    public abstract class TTypedDataTable : DataTable
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">name of table</param>
        public TTypedDataTable(string name) : base(name)
        {
            this.InitClass();
            this.InitVars();
        }

        /// <summary>
        /// on purpose, this constructor does not call InitClass or InitVars;
        /// used for serialization
        /// </summary>
        /// <param name="tab">table for copying the table name</param>
        public TTypedDataTable(DataTable tab) : base(tab.TableName)
        {
            // System.Console.WriteLine('TTypedDataTable constructor tab:DataTable');
        }

        /// <summary>
        /// default constructor
        /// not needed, but for clarity
        /// </summary>
        public TTypedDataTable() : base()
        {
            this.InitClass();
            this.InitVars();
        }

        /// <summary>
        /// serialization constructor
        /// </summary>
        /// <param name="info">required for serialization</param>
        /// <param name="context">required for serialization</param>
        public TTypedDataTable(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
            // Console.WriteLine('TTypeddatatable.create serialization');
            this.InitVars();
        }

        /// <summary>
        /// abstract method to be implemented by generated code
        /// </summary>
        protected abstract void InitClass();

        /// <summary>
        /// abstract method to be implemented by generated code
        /// </summary>
        public abstract void InitVars();

        /// <summary>
        /// abstract method to be implemented by generated code
        /// </summary>
        public abstract OdbcParameter CreateOdbcParameter(DataColumn ACol);

        /// <summary>
        /// make sure that we use GetChangesType instead of GetChanges
        /// </summary>
        /// <returns>throws an exception</returns>
        public new DataTable GetChanges()
        {
            throw new Exception("Note to the developer: use GetChangesTyped instead of DataTable.GetChanges");

            // return null;
        }

        /// <summary>
        /// our own version of GetChanges
        /// </summary>
        /// <returns>returns a typed table with the changes</returns>
        public DataTable GetChangesTypedInternal()
        {
            DataTable ReturnValue;

            ReturnValue = base.GetChanges();

            if (ReturnValue != null)
            {
                // might not be necessary. The casting in the derived class might already call the contructor?
                ((TTypedDataTable)ReturnValue).InitVars();
            }

            return ReturnValue;
        }

        /// <summary>
        /// the number of rows in the current table
        /// </summary>
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        /// <summary>
        /// remove columns that are not needed
        /// </summary>
        /// <param name="ATableTemplate">this table only contains the columns that should be kept</param>
        public void RemoveColumnsNotInTableTemplate(DataTable ATableTemplate)
        {
            DataUtilities.RemoveColumnsNotInTableTemplate(this, ATableTemplate);
        }
    }
}