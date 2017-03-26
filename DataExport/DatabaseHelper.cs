using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataExport
{
    class DatabaseHelper
    {
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DataTable schema = connection.GetSchema("Tables");
                List<string> TableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    TableNames.Add(row[2].ToString());
                }
                return TableNames;
            }
        }
    }
}
