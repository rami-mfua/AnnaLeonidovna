using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using MySql.Data.MySqlClient;

namespace Bashirov_16.Model
{
    public class PostgreModel
    {
        public class UserCustom
        {
            public int id { get; set; }
            public string login { get; set; }
            public string user_password { get; set; }
            public string user_role { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string patronymic { get; set; }
        }

        public class DB
        {
            
            MySqlConnection connection = new MySqlConnection(
                    "server=bsu9hyxfkx49dwperkuz-mysql.services.clever-cloud.com;" +
                    "port=3306;" +
                    "username=uqzrpaehtziaf9b2;" +
                    "password=C1VWCMrtvGzOtOoRomCI;" +
                    "database=bsu9hyxfkx49dwperkuz"
                );

            public void openConnection()
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
            }

            public void closeConnection()
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            public MySqlConnection getConnection()
            {
                return connection;
            }
        }


        //public class ApplicationDatabaseContext : DbContext
        //{
        //    public DbSet<User> users { get; set; }


        //    private static readonly string host = "localhost:54321";
        //    private static readonly string database = "local_db_for_labs";
        //    private static readonly string username = "postgres";
        //    private static readonly string password = "";

        //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    {
        //        optionsBuilder.UseNpgsql(
        //            $"Database={database};" +
        //            $"Username={username};" +
        //            $"Password={password};" +
        //            $"Host={host}"
        //            );
        //    }
        //}
    }
}
