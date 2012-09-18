using System;
using System.Data;

namespace DevAge.Data
{
    /// <summary>
    /// Utilities for DataSet class. Contains some methods to create expression string, for Select and Epression methods and methods for selecting data like SelectDistinct.
    /// </summary>
    public class DataSetHelper
    {
        #region Expression convert methods
        /// <summary>
        /// Create a string with this format: #MM/dd/yyyy# . This is the default format for DataSet expressions. Can be used for example with the DataTable.Select method.
        /// </summary>
        /// <param name="p_Date"></param>
        /// <returns></returns>
        public static string ExpressionFormat(DateTime p_Date)
        {
            return "#" + p_Date.ToString("MM/dd/yyyy") + "#";
        }
        /// <summary>
        /// Create a string with an InvariantCulture format.
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        public static string ExpressionFormat(int p_data)
        {
            return p_data.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Create a string with an InvariantCulture format.
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        public static string ExpressionFormat(long p_data)
        {
            return p_data.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        /// <summary>
        /// Create a string with an InvariantCulture format.
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        public static string ExpressionFormat(float p_data)
        {
            return p_data.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Create a string with an InvariantCulture format.
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        public static string ExpressionFormat(double p_data)
        {
            return p_data.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        /// <summary>
        /// Create a string with an InvariantCulture format.
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        public static string ExpressionFormat(decimal p_data)
        {
            return p_data.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        /// <summary>
        /// Create a string with an InvariantCulture format.
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        public static string ExpressionFormat(bool p_data)
        {
            return p_data.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Replace any special sql character (like single quote) and replace it with the valid sql equivalent. Then add the appropriate quote if the type require (string).
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        public static string ExpressionFormat(string p_data)
        {
            p_data = p_data.Replace("'", "''");
            return "'" + p_data + "'";
        }
        /// <summary>
        /// Replace any special sql character (like single quote) and replace it with the valid sql equivalent. Then add the appropriate quote if the type require (string).
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        public static string ExpressionFormat(char p_data)
        {
            return ExpressionFormat(p_data.ToString());
        }
        /// <summary>
        /// Return NULL
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        public static string ExpressionFormat(System.DBNull p_data)
        {
            return "NULL";
        }

        /// <summary>
        /// Format the specified value in a string that can be used inside an expression.
        /// </summary>
        /// <returns></returns>
        public static string ExpressionFormat(object p_data)
        {
            if (p_data is DateTime)
                return ExpressionFormat((DateTime)p_data);
            else if (p_data is int)
                return ExpressionFormat((int)p_data);
            else if (p_data is long)
                return ExpressionFormat((long)p_data);
            else if (p_data is float)
                return ExpressionFormat((float)p_data);
            else if (p_data is double)
                return ExpressionFormat((double)p_data);
            else if (p_data is decimal)
                return ExpressionFormat((decimal)p_data);
            else if (p_data is bool)
                return ExpressionFormat((bool)p_data);
            else if (p_data is string)
                return ExpressionFormat((string)p_data);
            else if (p_data is char)
                return ExpressionFormat((char)p_data);
            else if (p_data is System.DBNull)
                return ExpressionFormat((System.DBNull)p_data);
            else
                throw new DevAgeApplicationException("Type not supported for expression");
        }


        #endregion

        #region Operator methods
        /// <summary>
        /// Create a like expression for string values, automatically calls FormatValueForExpression
        /// </summary>
        /// <param name="pFieldName"></param>
        /// <param name="pFieldValue"></param>
        /// <returns></returns>
        public static string LikeExpression(string pFieldName, string pFieldValue)
        {
            //TODO Bisognerebbe gestire anche la ricerca di valori speciali. Si dovrebbe ad esempio sostituire le occorrenze di caratteri come * e % (entrambi supportati per la ricerca LIKE) con rispettivamente [*] e [%] e sostituire [ e ] con [[] e []] .
            string param = "%" + pFieldValue + "%";
            return pFieldName + " LIKE " + ExpressionFormat(param);
        }
        /// <summary>
        /// Create a start with expression for stirng value, automatically calls FormatValueForExpression
        /// </summary>
        /// <param name="pFieldName"></param>
        /// <param name="pFieldValue"></param>
        /// <returns></returns>
        public static string StartWithExpression(string pFieldName, string pFieldValue)
        {
            //TODO Bisognerebbe gestire anche la ricerca di valori speciali. Si dovrebbe ad esempio sostituire le occorrenze di caratteri come * e % (entrambi supportati per la ricerca LIKE) con rispettivamente [*] e [%] e sostituire [ e ] con [[] e []] .
            string param = pFieldValue + "%";
            return pFieldName + " LIKE " + ExpressionFormat(param);
        }
        /// <summary>
        /// Create an equal expression, automatically calls FormatValueForExpression
        /// </summary>
        /// <returns></returns>
        public static string EqualExpression(string pFieldName, object pFieldValue)
        {
            return pFieldName + " = " + ExpressionFormat(pFieldValue);
        }
        /// <summary>
        /// Create a Not equal expression, automatically calls FormatValueForExpression
        /// </summary>
        /// <returns></returns>
        public static string NotEqualExpression(string pFieldName, object pFieldValue)
        {
            return pFieldName + " <> " + ExpressionFormat(pFieldValue);
        }

        #endregion

        #region Helper methods
        /// <summary>
        /// Compares two values to see if they are equal. Also compares DBNULL.Value.
        /// Note: If your DataTable contains object fields, then you must extend this
        /// function to handle them in a meaningful way if you intend to group on them.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool ValEquals(object A, object B)
        {
            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }

        public static DataTable SelectDistinct(DataTable SourceTable, string FieldName, bool pAddEmptyValue, object EmptyValue)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

            if (pAddEmptyValue)
                dt.Rows.Add(new object[] { EmptyValue });

            object LastValue = EmptyValue;
            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ValEquals(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    dt.Rows.Add(new object[] { LastValue });
                }
            }

            return dt;
        }

        public static DataTable SampleDataByInterval(DataView sourceView, string[] columns, string samplingField, int intervalValue, DateTimeInterval intervalType)
        {
            DataTable newTable = new DataTable();
            for (int i = 0; i < columns.Length; i++)
                newTable.Columns.Add(new DataColumn(columns[i], sourceView.Table.Columns[columns[i]].DataType));

            DateTime lastValue;
            if (sourceView.Count > 0)
            {
                //Add the first row
                lastValue = (DateTime)sourceView[0][samplingField];
                DataRow newRow = newTable.NewRow();
                for (int i = 0; i < columns.Length; i++)
                    newRow[columns[i]] = sourceView[0][columns[i]];
                newTable.Rows.Add(newRow);

                //Add all rows
                for (int i = 1; i < sourceView.Count; i++)
                {
                    DataRow dr = sourceView[i].Row;
                    DateTime currentValue = (DateTime)dr[samplingField];
                    int currentInterval;

                    if (intervalType == DateTimeInterval.Months)
                        currentInterval = DateTimeHelper.MonthsDifference(currentValue, lastValue);
                    else
                        throw new ApplicationException("Interval not supported");

                    if (currentInterval >= intervalValue || (i + 1) == sourceView.Count)
                    {
                        if (intervalType == DateTimeInterval.Months)
                            lastValue = lastValue.AddMonths(intervalValue);
                        else
                            throw new ApplicationException("Interval not supported");

                        newRow = newTable.NewRow();
                        for (int j = 0; j < columns.Length; j++)
                            newRow[columns[j]] = dr[columns[j]];
                        newTable.Rows.Add(newRow);
                    }
                }
            }

            return newTable;
        }

        #endregion


        #region Evaluate methods
        private static DataTable mEmptyDataTable = new DataTable();
        /// <summary>
        /// Evaluate the given expression using the Compute method of a empty DataTable. Replace the parameters using the string.Format method ({0} syntax)
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object Eval(string expression, params object[] parameters)
        {
            object[] strParams = null;
            if (parameters != null)
            {
                strParams = new string[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    strParams[i] = ExpressionFormat(parameters[i]);
                }
            }

            string expToEvaluate;
            if (strParams != null)
                expToEvaluate = string.Format(expression, strParams);
            else
                expToEvaluate = expression;

            return mEmptyDataTable.Compute(expToEvaluate, string.Empty);
        }

        /// <summary>
        /// Evaluate the given expression using the Compute method of an empty DataTable replacing the values of the specified Row.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static object EvalRowExpression(DataRow row, string expression)
        {
            //DataTable table = new DataTable();
            //foreach (DataColumn c in row.Table.Columns)
            //    table.Columns.Add(c.ColumnName, c.DataType);

            //table.Rows.Add(row.ItemArray);
            //table.AcceptChanges();

            string expToEval = expression;
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                string matchString;

                // First search for [col1] syntax and then for the standard col1 syntax
                matchString = System.Text.RegularExpressions.Regex.Escape("[" + row.Table.Columns[i].ColumnName + "]");

                if (System.Text.RegularExpressions.Regex.IsMatch(expToEval, matchString, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    expToEval = System.Text.RegularExpressions.Regex.Replace(expToEval, matchString, ExpressionFormat(row[i]), System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // \b match only whole word
                matchString = @"\b" + System.Text.RegularExpressions.Regex.Escape(row.Table.Columns[i].ColumnName) + @"\b";

                if (System.Text.RegularExpressions.Regex.IsMatch(expToEval, matchString, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    expToEval = System.Text.RegularExpressions.Regex.Replace(expToEval, matchString, ExpressionFormat(row[i]), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }

            return Eval(expToEval, null);
        }

        #endregion
    }

    public enum DateTimeInterval
    {
        Months
    }
}
