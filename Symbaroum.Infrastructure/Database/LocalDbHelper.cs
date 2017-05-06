using System;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Symbaroum.Infrastructure.Database
{
    public static class LocalDbHelper
    {
        public static void DropDatabase(string connectionString)
        {
            const string dropDatabaseSql =
                "if (select DB_ID('{0}')) is not null\r\n"
                + "begin\r\n"
                + "alter database [{0}] set offline with rollback immediate;\r\n"
                + "alter database [{0}] set online;\r\n"
                + "drop database [{0}];\r\n"
                + "end";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var sqlToExecute = string.Format(dropDatabaseSql, connection.Database);

                    var command = new SqlCommand(sqlToExecute, connection);

                    Console.WriteLine("Dropping database");
                    command.ExecuteNonQuery();
                    Console.WriteLine("Database is dropped");
                }
            }
            catch (SqlException sqlException)
            {
                if (sqlException.Message.StartsWith("Cannot open database"))
                {
                    Console.WriteLine("Database does not exist.");
                    return;
                }
                throw;
            }
        }

        //public static void ForceInitialization(DomainContext domainContext)
        //{
        //    var initializer = new MigrateDatabaseToLatestVersion<DomainContext, DomainContextMigrationsConfiguration>();

        //    System.Data.Entity.Database.SetInitializer(initializer);

        //    Console.WriteLine("Starting creating database");
        //    domainContext.Database.Initialize(true);
        //    Console.WriteLine("Database is created");
        //}
    }
}