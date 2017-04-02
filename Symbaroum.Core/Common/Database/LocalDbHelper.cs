using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace Symbaroum.Core.Common.Database
{
    public static class LocalDbHelper
    {
        public const string DbDirectory = "Data";
        public const string LocalDbInstanceName = @"SymbLocalDb";

        public static SqlConnection GetLocalDb(string dbName, bool deleteIfExists = false)
        {
            var outputFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), DbDirectory);
            var mdfFilename = dbName + ".mdf";
            var dbFileName = Path.Combine(outputFolder, mdfFilename);
            var logFileName = Path.Combine(outputFolder, $"{dbName}_log.ldf");
            // Create Data Directory If It Doesn't Already Exist.
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // If the file exists, and we want to delete old data, remove it here and create a new database.
            if (File.Exists(dbFileName) && deleteIfExists)
            {
                if (File.Exists(logFileName)) File.Delete(logFileName);
                File.Delete(dbFileName);
                CreateDatabase(dbName, dbFileName);
            }
            // If the database does not already exist, create it.
            else if (!File.Exists(dbFileName))
            {
                CreateDatabase(dbName, dbFileName);
            }

            // Open newly created, or old database.
            var connectionString = string.Format($@"Data Source=(LocalDB)\{LocalDbInstanceName};AttachDBFileName={dbFileName};Initial Catalog={dbName};Integrated Security=True;");
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public static bool CreateDatabase(string dbName, string dbFileName)
        {
            var connectionString = $@"Data Source=(LocalDB)\{LocalDbInstanceName};Initial Catalog=master;Integrated Security=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();

                DetachDatabase(dbName);

                cmd.CommandText = $"CREATE DATABASE {dbName} ON (NAME = N'{dbName}', FILENAME = '{dbFileName}')";
                cmd.ExecuteNonQuery();
            }

            if (File.Exists(dbFileName))
            {
                return true;
            }
            return false;
        }

        public static bool DetachDatabase(string dbName)
        {
            try
            {
                var connectionString = $@"Data Source=(LocalDB)\{LocalDbInstanceName};Initial Catalog=master;Integrated Security=True";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = $"exec sp_detach_db '{dbName}'";
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}