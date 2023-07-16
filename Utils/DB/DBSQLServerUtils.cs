using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Tz.Utils
{
    internal class DBSQLServerUtils
    {
        public static MySqlConnection GetSqlConnection(Dictionary<String, String> connectionString)
        {
            var password = connectionString["password"];
            var userName = connectionString["userName"];
            var host = connectionString["host"];
            var database = connectionString["database"];

            if (password != String.Empty && userName != String.Empty)
            {
                var conn = $"server={host};uid={userName};" +
                           $"pwd={password};database={database}";
                if (conn != null) return new MySqlConnection(conn);
            }

            return null;
        }
    }
}
