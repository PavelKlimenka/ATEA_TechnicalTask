using DataAccess.Interfaces;
using DataAccess.Models;
using System.Data.Common;
using System.Data.SQLite;
using System.Reflection;

namespace DataAccess
{
    public class ArgumentsRepository : IRepository<ArgumentsRecord>
    {
        private const string DatabaseFileName = "ATEATechTask.db";
        private string _databaseFilePath;
        private SQLiteConnection _connection;

        public ArgumentsRepository()
        {
            string executablePath = Path.GetFullPath(Path.Combine(Assembly.GetExecutingAssembly().Location, ".."));
            _databaseFilePath = Path.Combine(executablePath, DatabaseFileName);
            if(!File.Exists(_databaseFilePath))
                CreateDatabase(_databaseFilePath);
            _connection = new SQLiteConnection($"Data Source={_databaseFilePath}; Version=3;");
            _connection.Open();
        }

        public async Task Delete(ArgumentsRecord entity)
        {
            SQLiteCommand command = new(
                "DELETE FROM Arguments" +
                $"WHERE id = {entity.Id}",
                _connection);
            await command.ExecuteNonQueryAsync();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task<List<ArgumentsRecord>> GetAll()
        {
            List<ArgumentsRecord> result = new();
            SQLiteCommand command = new("SELECT * FROM Arguments", _connection);
            using DbDataReader reader = await command.ExecuteReaderAsync();
            while(reader.Read())
            {
                result.Add(new ArgumentsRecord()
                {
                    Id = reader.GetInt32(0),
                    Arg1 = reader.GetString(1),
                    Arg2 = reader.GetString(2)
                });
            }
            return result;
        }

        public async Task<ArgumentsRecord> GetById(int id)
        {
            ArgumentsRecord result = new();
            SQLiteCommand command = new(
                "SELECT * FROM Arguments" +
                $"WHERE id = {id}",
                _connection);
            using DbDataReader reader = await command.ExecuteReaderAsync();
            if(reader.HasRows)
            {
                result.Id = reader.GetInt32(0);
                result.Arg1 = reader.GetString(1);
                result.Arg2 = reader.GetString(2);
            }
            return result;
        }

        public async Task Insert(ArgumentsRecord entity)
        {
            SQLiteCommand command = new(
                "INSERT INTO Arguments(arg1, arg2)" +
                $"VALUES ('{entity.Arg1}','{entity.Arg2}')", 
                _connection);
            await command.ExecuteNonQueryAsync();
        }

        public async Task Update(ArgumentsRecord entity)
        {
            SQLiteCommand command = new(
                "UPDATE Arguments" +
                $"SET arg1='{entity.Arg1}', arg2='{entity.Arg2}'" +
                $"WHERE id={entity.Id}", 
                _connection);
            await command.ExecuteNonQueryAsync();
        }

        private void CreateDatabase(string filePath)
        {
            SQLiteConnection.CreateFile(filePath);
            using SQLiteConnection connection = new SQLiteConnection($"Data Source={filePath}; Version=3;");
            
            SQLiteCommand command = new SQLiteCommand(
                $"CREATE TABLE IF NOT EXISTS Arguments (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                "arg1 NVARCHAR(50), arg2 NVARCHAR(50))",
                connection);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}