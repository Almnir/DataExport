using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataExport
{
    class DatabaseHelper
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public static bool CheckConnection()
        {
            bool result = false;
            try
            {
                using (var connection = new SqlConnection(Globals.GetConnectionString()))
                {
                    var query = "select 1";
                    var command = new SqlCommand(query, connection);
                    connection.Open();
                    command.ExecuteScalar();
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public static List<string> GetTables(string connectionString)
        {
            List<string> tableNames = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DataTable schema = connection.GetSchema("Tables");
                    foreach (DataRow row in schema.Rows)
                    {
                        tableNames.Add(row[2].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
            return tableNames;
        }

        public static List<string> ExcludeEmptyTablesAndGetSize(List<string> tables, out long estimateSize)
        {
            estimateSize = 0;
            List<string> resultTables = new List<string>();
            try
            {
                foreach (var table in tables)
                {
                    using (var connection = new SqlConnection(Globals.GetConnectionString()))
                    {
                        var query = string.Format("select count(*) from {0}", table);
                        var command = new SqlCommand(query, connection);
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        if (count > 0)
                        {
                            long bytesPerFile = count * Globals.TABLES_SIZE_PER_ROW[table] / 6;
                            estimateSize += bytesPerFile;
                            resultTables.Add(table);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
            return resultTables;
        }
    }
}
