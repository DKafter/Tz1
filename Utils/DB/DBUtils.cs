using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Tz.Utils
{
    internal class DBUtils
    {
        public static DataTable Get(string commandText)
        {
            var customers = new DataTable();
            using (var dbConnection = DBUtils.GetDBConnection())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = dbConnection;
                    dbConnection.Open();
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    using (var sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(customers);
                    }
                    dbConnection.Close();
                }
            }
            return customers;
        }

        public static MySqlConnection GetDBConnection()
        {
            var connectionString = new Dictionary<String, String>();

            using (var stream = File.OpenRead(".env"))
            {
                DotNetEnv.Env.Load();

                var password = DotNetEnv.Env.GetString("PASSWORD");
                var userName = DotNetEnv.Env.GetString("USERNAME");
                var host = DotNetEnv.Env.GetString("HOST");
                var database = DotNetEnv.Env.GetString("DATABASE");

                connectionString.Add("password", password);
                connectionString.Add("userName", userName);
                connectionString.Add("host", host);
                connectionString.Add("database", database);
            }

            return DBSQLServerUtils.GetSqlConnection(connectionString);
        }

        public static void ExecuteToDb(string commandText)
        {
            using (var dbConnection = DBUtils.GetDBConnection())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = dbConnection;
                    dbConnection.Open();
                    cmd.CommandText = commandText;
                    try
                    {
                        cmd.ExecuteScalar();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    dbConnection.Close();
                }
            }
        }

        public static void ExecuteToDb(string commandText, string[] nameParams, object[] values)
        {
            using (var dbConnection = DBUtils.GetDBConnection())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = dbConnection;
                    dbConnection.Open();
                    cmd.CommandText = commandText;
                    for (int i = 0; i < values.Length; ++i)
                    {
                        if (values.GetValue(i) != DBNull.Value)
                        {
                            cmd.Parameters.AddWithValue($"@{nameParams[i]}", values[i] ?? DBNull.Value);
                        }
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    dbConnection.Close();
                }
            }
        }
    }
}
