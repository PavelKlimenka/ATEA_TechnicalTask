
using ATEA_TechnicalTask.Shared;
using System.Data.SQLite;

namespace ATEA_TechnicalTask.Tests.Fixtures
{
    /// <summary>
    /// Fixture used to create database file for testing
    /// </summary>
    public class ArgumentsRepositoryTestsFixture : IDisposable
    {
        public const string DatabaseFilename = "ATEA_TechnicalTask_Test.db";
        private readonly string _databaseFilePath;

        public ArgumentsRepositoryTestsFixture()
        {
            _databaseFilePath = Path.Combine(Utilities.GetExecutableDirectoryPath(), DatabaseFilename);
            if (File.Exists(_databaseFilePath)) File.Delete(_databaseFilePath);
            CreateDatabase(_databaseFilePath);
        }

        private static void CreateDatabase(string filePath)
        {
            SQLiteConnection.CreateFile(filePath);
            using SQLiteConnection connection = new SQLiteConnection($"Data Source={filePath}; Version=3;");

            using SQLiteCommand command = new SQLiteCommand(
                $"CREATE TABLE IF NOT EXISTS Arguments (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                "arg1 NVARCHAR(50), arg2 NVARCHAR(50))",
                connection);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Dispose()
        {
            File.Delete(_databaseFilePath);
        }
    }
}
