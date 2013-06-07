/*
 * Created by SharpDevelop.
 * User: Pete
 * Date: 30/05/2013
 * Time: 12:41
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Testing.NUnitPetraServer;
using Ict.Tools.DBXML;
using System.Collections.Generic;
using Ict.Petra.Server.App.Core;

namespace ICT.Tools.DataMigrateStatistics
{
    class Program
    {
        public static void Main(string[] args)
        {
            new TLogging("delivery/bin/Ict.Tools.DataMigrateStatistics.log");
            new TAppSettingsManager(false);
            
            string row_count_location = TAppSettingsManager.GetValue("fulldumpPath", "delivery/bin/fulldump") +
                Path.DirectorySeparatorChar + "_row_count.txt";
            
            if (File.Exists(row_count_location))
            {
                DBAccess.GDBAccessObj = new TDataBase();

                TCmdOpts cmdLine = new TCmdOpts();
                string xmlfile = cmdLine.GetOptValue("petraxml");

                DBAccess.GDBAccessObj.EstablishDBConnection(CommonTypes.ParseDBType(cmdLine.GetOptValue("type")), cmdLine.GetOptValue("host"),
                    cmdLine.GetOptValue("port"), cmdLine.GetOptValue("database"), cmdLine.GetOptValue("username"), cmdLine.GetOptValue("password"), "");

                TDataDefinitionParser parserNew = new TDataDefinitionParser(xmlfile, false);
                TDataDefinitionStore storeNew = new TDataDefinitionStore();
                parserNew.ParseDocument(ref storeNew, false, true);
                List <TTable>newTables = storeNew.GetTables();

                // table names and row numbers on alternate lines
                string[] checkRows = File.ReadAllLines(row_count_location);

                Console.WriteLine();
                Console.WriteLine("--- Testing all rows have loaded successfully ---");

                int totalRows = 0; // total number of rows in database
                int totalCheckRows = 0; // total number of rows there should be in the database
                bool rowsMissing = false;

                foreach (TTable newTable in newTables)
                {
                    if (newTable.strName != "s_login") // ignore this table as the row count changes everytime a connection is made to the database
                    {
                        int i = 0;
                        int rowCount; // number of rows actually in table
                        int rowCountCheck = 0; // number of rows there should be in table
                    
                        // count rows in table
                        string sql = "SELECT count(*) from " + newTable.strName + ";";
                        rowCount = Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(sql, IsolationLevel.ReadUncommitted));

                        // read how many rows there should be in the same table
                        while (i < checkRows.Length)
                        {
                            if (checkRows[i] == newTable.strName)
                            {
                                rowCountCheck = Convert.ToInt32(checkRows[i + 1]);
                                break;
                            }

                            i += 2;
                        }

                        totalRows += rowCount;
                        totalCheckRows += rowCountCheck;

                        // if there are rows missing
                        if (rowCount != rowCountCheck)
                        {
                            Console.Write(newTable.strName + " is incomplete. ");
                            Console.WriteLine((rowCountCheck - rowCount) + " out of " + rowCountCheck + " rows did not load!");
                            rowsMissing = true;
                        }
                    }
                }

                // if all rows are present
                if (!rowsMissing)
                {
                    Console.WriteLine("All rows successfully loaded.");
                }

                // write a summary of the number of rows successfully loaded
                Console.WriteLine();
                Console.WriteLine("--- Total number of rows loaded ---");
                Console.WriteLine(totalRows + " out of " + totalCheckRows + " rows loaded successfully.");
                string percentage = (((double)totalRows / totalCheckRows) * 100).ToString("#.##");
                Console.WriteLine(percentage + "%");
                Console.WriteLine();

                DBAccess.GDBAccessObj.CloseDBConnection();
            }
            else
            {
                TLogging.Log("Warning: unable to perform a row count test. _row_count.txt is missing from the folder .../delivery/bin/fulldump.");
                TLogging.Log("");
            }
        }
    }
}