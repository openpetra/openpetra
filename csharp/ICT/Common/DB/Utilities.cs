// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2019 by OM International
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
using System.Data.Common;
using System.Data.Odbc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ict.Common.DB
{
    #region TDataAdapterCanceller

    /// <summary>
    /// Provides a safe means to cancel the Fill operation of an associated <see cref="DbDataAdapter"/>.
    /// </summary>
    public sealed class TDataAdapterCanceller
    {
        readonly DbDataAdapter FDataAdapter;

        internal TDataAdapterCanceller(DbDataAdapter ADataAdapter)
        {
            FDataAdapter = ADataAdapter;
        }

        /// <summary>
        /// Call this Method to cancel the Fill operation of the associated <see cref="DbDataAdapter"/>.
        /// </summary>
        /// <remarks><em>IMPORTANT:</em> This Method <em>MUST</em> be called on a separate Thread as otherwise the cancellation
        /// will not work correctly (this is an implementation detail of ADO.NET!).</remarks>
        public void CancelFillOperation()
        {
            FDataAdapter.SelectCommand.Cancel();
        }
    }

    #endregion

    #region TSQLBatchStatementEntry

    /// <summary>
    /// Represents the Value of an entry in a HashTable for use in calls to one of the
    /// <c>TDataBase.ExecuteNonQueryBatch</c> Methods.
    /// </summary>
    /// <remarks>Once instantiated, Batch Statment Entry values can
    /// only be read!</remarks>
    public class TSQLBatchStatementEntry
    {
        /// <summary>Holds the SQL Statement for one Batch Statement Entry</summary>
        private string FSQLStatement;

        /// <summary>Holds the Parameters for a Batch Entry (optional)</summary>
        private DbParameter[] FParametersArray;

        /// <summary>
        /// SQL Statement for one Batch Entry.
        /// </summary>
        public String SQLStatement
        {
            get
            {
                return FSQLStatement;
            }
        }

        /// <summary>
        /// Parameters for a Batch Entry (optional).
        /// </summary>
        public DbParameter[] Parameters
        {
            get
            {
                return FParametersArray;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ASQLStatement">SQL Statement for one Batch Entry.</param>
        /// <param name="AParametersArray">Parameters for the SQL Statement (can be null).</param>
        /// <returns>void</returns>
        public TSQLBatchStatementEntry(String ASQLStatement, DbParameter[] AParametersArray)
        {
            FSQLStatement = ASQLStatement;
            FParametersArray = AParametersArray;
        }
    }

    #endregion

    /// <summary>
    /// A list of parameters which should be expanded into an `IN (?)' context.
    /// </summary>
    /// <example>
    /// Simply use the following style in your .sql file:
    /// <code>
    ///   SELECT * FROM table WHERE column IN (?)
    /// </code>
    ///
    /// Then, to test if <c>column</c> is the string <c>"First"</c>,
    /// <c>"Second"</c>, or <c>"Third"</c>, set the <c>OdbcParameter.Value</c>
    /// property to a <c>TDbListParameterValue</c> instance. You
    /// can use the
    /// <c>TDbListParameterValue.OdbcListParameterValue()</c>
    /// function to produce an <c>OdbcParameter</c> with an
    /// appropriate <c>Value</c> property.
    /// <code>
    /// OdbcParameter[] parameters = new OdbcParamter[]
    /// {
    ///     TDbListParameterValue(param_grdCommitmentStatusChoices", OdbcType.NChar,
    ///         new String[] { "First", "Second", "Third" }),
    /// };
    /// </code>
    /// </example>
    public class TDbListParameterValue : IEnumerable <OdbcParameter>
    {
        private IEnumerable SubValues;

        /// <summary>
        /// The OdbcParameter from which sub-parameters are Clone()d.
        /// </summary>
        public OdbcParameter OdbcParam;

        /// <summary>
        /// Create a list parameter, such as is used for 'column IN (?)' in
        /// SQL queries, from any IEnumerable object.
        /// </summary>
        /// <param name="name">The ParameterName to use when creating OdbcParameters.</param>
        /// <param name="type">The OdbcType of the produced OdbcParameters.</param>
        /// <param name="value">An enumerable collection of objects.
        /// If there are no objects in the enumeration, then the resulting
        /// query will look like <c>column IN (NULL)</c> because
        /// <c>column IN ()</c> is invalid. To avoid the case where
        /// the query should not match any rows and <c>column</c>
        /// may be NULL, use an expression like <c>(? AND column IN (?)</c>.
        /// Set the first parameter to FALSE if the list is empty and
        /// TRUE otherwise so that the prepared statement remains both
        /// syntactically and semantically valid.</param>
        public TDbListParameterValue(String name, OdbcType type, IEnumerable value)
        {
            OdbcParam = new OdbcParameter(name, type);
            SubValues = value;
        }

        IEnumerator <OdbcParameter>IEnumerable <OdbcParameter> .GetEnumerator()
        {
            UInt32 Counter = 0;

            foreach (Object value in SubValues)
            {
                OdbcParameter SubParameter = (OdbcParameter)((ICloneable)OdbcParam).Clone();
                SubParameter.Value = value;

                if (SubParameter.ParameterName != null)
                {
                    SubParameter.ParameterName += "_" + (Counter++);
                }

                yield return SubParameter;
            }
        }

        /// <summary>
        /// Get the generic IEnumerator over the sub-OdbcParameters.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable <OdbcParameter> ) this).GetEnumerator();
        }

        /// <summary>
        /// Represent this list of parameters as a string, using
        /// each value's <c>ToString()</c> method.
        /// </summary>
        public override String ToString()
        {
            return "[" + String.Join(",", SubValues.Cast <Object>()) + "]";
        }

        /// <summary>
        /// Convenience method for creating an OdbcParameter with an
        /// appropriate <c>TDbListParameterValue</c> as a value.
        /// </summary>
        public static OdbcParameter OdbcListParameterValue(String name, OdbcType type, IEnumerable value)
        {
            return new OdbcParameter(name, type) {
                       Value = new TDbListParameterValue(name, type, value)
            };
        }
    }
}
