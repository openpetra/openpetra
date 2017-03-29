// Auto generated with nant generateORMData
// From a template at inc\template\src\ORM\TableList.cs
//
// Do not modify this file manually!
//
{#GPLFILEHEADER}

using System.Collections.Generic;

namespace Ict.Petra.Shared
{
    /// <summary>
    /// this returns a list of all database tables, ordered by the most referenced tables,
    /// which need to be created first and can be deleted last
    /// </summary>
    public class TTableList
    {
        /// <summary>
        /// get the names of the tables, ordered by constraint dependancy.
        /// first the tables that other depend upon
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDBNames()
        {
            List<string> list = new List<string>();
            {#DBTableNames}
            return list;
        }

        /// <summary>
        /// get the names of the sequences of the whole database
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDBSequenceNames()
        {
            List<string> list = new List<string>();
            {#DBSequenceNames}
            return list;
        }
        
        /// <summary>
        /// get the names of the tables that are available for use in custom report
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDBNamesAvailableForCustomReport()
        {
            List<string> list = new List<string>();
            {#DBTableNamesAvailableForCustomReport}
            return list;
        }
    }
}
