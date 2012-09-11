using System;
using System.Data;
using System.Data.SqlClient;

namespace DevAge.Data.SqlClient
{
	/// <summary>
	/// This class is similar to the System.Data.SqlClient.SqlCommandBuilder, but use only the DataTable to create the required commands.
	/// Don't support identity (autoincrement) column and only the base data type. This class don't use the data source to explore the data but only the DataTable informations.
	/// So you must populate the PrimaryKeys of the DataSet. I suggest to use GUID column (uniqueidentifier) to replace the identity column.
	/// Remember to set the Connection of the generated command.
	/// </summary>
	public class SqlCommandBuilder
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dataTable">table used to create commands</param>
		public SqlCommandBuilder(DataTable dataTable)
		{
			mDataTable = dataTable;
			mPrimaryKeys = mDataTable.PrimaryKey;
			if (mPrimaryKeys == null || mPrimaryKeys.Length == 0)
				throw new ApplicationException("DataTable must have a primary key");
			
			mNormalColumns = new DataColumn[mDataTable.Columns.Count - mPrimaryKeys.Length];
			int foundNormalColumn = 0;
			for (int c = 0; c < mDataTable.Columns.Count; c++)
			{
				bool found = false;
				for (int cc = 0; cc < mPrimaryKeys.Length; cc++)
				{
					if (mDataTable.Columns[c].AutoIncrement)
						throw new ApplicationException("Autoincrement column not supported");

					if (mDataTable.Columns[c].ColumnName == mPrimaryKeys[cc].ColumnName)
					{
						found = true;
						break;
					}
				}

				if (found == false)
				{
					mNormalColumns[foundNormalColumn] = mDataTable.Columns[c];
					foundNormalColumn++;
				}
			}
		}

		private DataTable mDataTable;
		/// <summary>
		/// Table used to create commands
		/// </summary>
		public DataTable DataTable
		{
			get{return mDataTable;}
			set{mDataTable = value;}
		}

		private DataColumn[] mPrimaryKeys;
		public DataColumn[] PrimaryKeyColumns
		{
			get{return mPrimaryKeys;}
			set{mPrimaryKeys = value;}
		}
		private DataColumn[] mIdentityColumns;
		public DataColumn[] IdentityColumns
		{
			get{return mIdentityColumns;}
			set{mIdentityColumns = value;}
		}
		private DataColumn[] mNormalColumns;
		public DataColumn[] NormalColumns
		{
			get{return mNormalColumns;}
			set{mNormalColumns = value;}
		}

		#region Sql Method
		protected virtual string CreateSqlTableName()
		{
			return "[" + mDataTable.TableName + "]";
		}
		protected virtual string CreateSqlColumnName(DataColumn column)
		{
			return "[" + column.ColumnName + "]";
		}
		protected virtual string CreateSqlInsertColumnName()
		{
			System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

			//Primary Key Column
			for (int i = 0; i < mPrimaryKeys.Length; i++)
			{
				strBuilder.Append( CreateSqlColumnName(mPrimaryKeys[i]) );
				if ( i < (mPrimaryKeys.Length - 1)  || mNormalColumns.Length > 0)
					strBuilder.Append(",");
			}

			//Normal Column
			for (int i = 0; i < mNormalColumns.Length; i++)
			{
				strBuilder.Append( CreateSqlColumnName(mNormalColumns[i]) );
				if ( i < (mNormalColumns.Length - 1))
					strBuilder.Append(",");
			}

			return strBuilder.ToString();
		}
		protected virtual string CreateSqlInsertParameters(SqlParameterCollection parameters)
		{
			System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

			//Parameters
			for (int i = 0; i < parameters.Count; i++)
			{
				strBuilder.Append( parameters[i].ParameterName );
				if ( i < (parameters.Count - 1))
					strBuilder.Append(",");
			}

			return strBuilder.ToString();
		}

		protected virtual string CreateSqlPKWhereColumn()
		{
			System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

			int pIndex = 0;

			//Primary Key Column
			for (int i = 0; i < mPrimaryKeys.Length; i++)
			{
				strBuilder.Append( CreateSqlColumnName(mPrimaryKeys[i]) );
				strBuilder.Append( " = " );
				strBuilder.Append( "@p" + pIndex.ToString() );
				if ( i < (mPrimaryKeys.Length - 1) )
					strBuilder.Append(" AND ");

				pIndex++;
			}

			return strBuilder.ToString();
		}

		protected virtual string CreateSqlUpdateColumn()
		{
			System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

			int pIndex = mPrimaryKeys.Length; //start the parameter count from the number of primary keys because I first populate the primary key columns

			//Normal Column
			for (int i = 0; i < mNormalColumns.Length; i++)
			{
				strBuilder.Append( CreateSqlColumnName(mNormalColumns[i]) );
				strBuilder.Append( " = " );
				strBuilder.Append( "@p" + pIndex.ToString() );
				if ( i < (mNormalColumns.Length - 1))
					strBuilder.Append(",");

				pIndex++;
			}

			return strBuilder.ToString();
		}

		protected virtual void PopulateAllParameters(SqlParameterCollection parameters)
		{
			int pIndex = 0;

			//Primary Key Column
			for (int i = 0; i < mPrimaryKeys.Length; i++)
			{
				parameters.Add("@p" + pIndex.ToString(), SqlDbTypeFromDataType(mPrimaryKeys[i].DataType), 0, mPrimaryKeys[i].ColumnName);
				pIndex++;
			}

			//Normal Column
			for (int i = 0; i < mNormalColumns.Length; i++)
			{
				parameters.Add("@p" + pIndex.ToString(), SqlDbTypeFromDataType(mNormalColumns[i].DataType), 0, mNormalColumns[i].ColumnName);
				pIndex++;
			}
		}

		protected virtual SqlDbType SqlDbTypeFromDataType(Type type)
		{
			if (type == typeof(string))
				return SqlDbType.VarChar;
			else if (type == typeof(int))
				return SqlDbType.Int;
			else if (type == typeof(long))
				return SqlDbType.BigInt;
			else if (type == typeof(double))
				return SqlDbType.Float;
			else if (type == typeof(float))
				return SqlDbType.Float;
			else if (type == typeof(bool))
				return SqlDbType.Bit;
			else if (type == typeof(DateTime))
				return SqlDbType.DateTime;
			else if (type == typeof(decimal))
				return SqlDbType.Decimal;
			else if (type == typeof(Guid))
				return SqlDbType.UniqueIdentifier;
			else if (type == typeof(byte[]))
				return SqlDbType.Image;
			else
				throw new ApplicationException("Type " + type.Name + " not supported");
		}
		#endregion

		public virtual SqlCommand GetInsertCommand()
		{
			//INSERT INTO Region( RegionID , RegionDescription ) VALUES ( @p1 , @p2 )
			const string cmdFormat = "INSERT INTO {0} ({1}) VALUES ({2})";

			SqlCommand command = new SqlCommand();
			command.CommandType = CommandType.Text;
			PopulateAllParameters(command.Parameters);
			string sql = string.Format(cmdFormat, CreateSqlTableName(), CreateSqlInsertColumnName(), CreateSqlInsertParameters(command.Parameters) );
			command.CommandText = sql;

			return command;
		}
		public virtual SqlCommand GetUpdateCommand()
		{
			//UPDATE [FundPrice] SET [IDImport]=..., [Price]=.... WHERE [IDFundPrice]=...
			const string cmdFormat = "UPDATE {0} SET {1} WHERE {2}";

			SqlCommand command = new SqlCommand();
			command.CommandType = CommandType.Text;
			PopulateAllParameters(command.Parameters);
			string sql = string.Format(cmdFormat, CreateSqlTableName(), CreateSqlUpdateColumn(), CreateSqlPKWhereColumn() );
			command.CommandText = sql;

			return command;
		}

		public virtual SqlCommand GetDeleteCommand()
		{
			//DELETE [FundPrice] WHERE [IDFundPrice]=...
			const string cmdFormat = "DELETE {0} WHERE {1}";

			SqlCommand command = new SqlCommand();
			command.CommandType = CommandType.Text;
			PopulateAllParameters(command.Parameters);
			string sql = string.Format(cmdFormat, CreateSqlTableName(), CreateSqlPKWhereColumn() );
			command.CommandText = sql;

			return command;
		}
	}
}
