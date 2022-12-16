using ATEA_TechnicalTask.Shared;
using ATEA_TechnicalTask.Shared.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;
using System.Data.Common;
using System.Data.SQLite;
using System.Reflection;

namespace DataAccess
{
    public class ArgumentsRepository : IRepository<ArgumentsRecord>
    {
        private readonly string _databaseFileName = "ATEATechTask.db";
        private string _databaseFilePath;
        private SQLiteConnection _connection;
        private ILogger _logger;

        public ArgumentsRepository(ILogger logger, string? databaseFilename = null)
        {
            _logger = logger;
            if(databaseFilename != null) _databaseFileName = databaseFilename;
            _databaseFilePath = Path.Combine(Utils.GetExecutableDirectoryPath(), _databaseFileName);

            if(!File.Exists(_databaseFilePath)) CreateDatabase(_databaseFilePath);

            try
            {
                _connection = new SQLiteConnection($"Data Source={_databaseFilePath}; Version=3;");
                _connection.Open();
            }
            catch(Exception e)
            {
                _logger.LogError($"Wasn't able to open database connection. Message: {e.Message}");
            }
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public async Task<ArgumentsRecord> Delete(ArgumentsRecord entity)
        {
            ArgumentsRecord result = new(entity);

            try
            {
                using SQLiteCommand command = new(
                    "DELETE FROM Arguments " +
                    $"WHERE id={entity.Id}",
                    _connection);
                await command.ExecuteNonQueryAsync();
            }
            catch(Exception e)
            {
                _logger.LogError($"Wasn't able to delete record with id '{entity.Id}'. Message: {e.Message}");
            }

            return result;
        }

        public async Task<List<ArgumentsRecord>> GetAll()
        {
            List<ArgumentsRecord> result = new();
            try
            {
                using SQLiteCommand command = new("SELECT * FROM Arguments", _connection);
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
            }
            catch(Exception e)
            {
                _logger.LogError($"Wasn't able to get all records. Message: {e.Message}");
            }

            return result;
        }

        public async Task<ArgumentsRecord> GetById(int id)
        {
            ArgumentsRecord result = new();

            try
            {
                using SQLiteCommand command = new(
                    "SELECT * FROM Arguments " +
                    $"WHERE id={id}",
                    _connection);
                using DbDataReader reader = await command.ExecuteReaderAsync();
                if(reader.HasRows)
                {
                    reader.Read();
                    result.Id = reader.GetInt32(0);
                    result.Arg1 = reader.GetString(1);
                    result.Arg2 = reader.GetString(2);
                }
            }
            catch(Exception e)
            {
                _logger.LogError($"Wasn't able to get record with id '{id}'. Message: {e.Message}");
            }

            return result;
        }

        public async Task<ArgumentsRecord> Insert(ArgumentsRecord entity)
        {
            ArgumentsRecord result = new(entity);

            try
            {
                using SQLiteCommand command = new(
                    "INSERT INTO Arguments(arg1, arg2) " +
                    $"VALUES ('{entity.Arg1}','{entity.Arg2}')", 
                    _connection);
                await command.ExecuteNonQueryAsync();
                result.Id = (int)_connection.LastInsertRowId;
            }
            catch(Exception e)
            {
                _logger.LogError($"Wasn't able to insert record. Message: {e.Message}");
            }

            return result;
        }

        public async Task<ArgumentsRecord> Update(ArgumentsRecord entity)
        {
            ArgumentsRecord result = new(entity);

            try
            {
                using SQLiteCommand command = new(
                    "UPDATE Arguments " +
                    $"SET arg1='{entity.Arg1}', arg2='{entity.Arg2}' " +
                    $"WHERE id={entity.Id}", 
                    _connection);
                await command.ExecuteNonQueryAsync();
            }
            catch(Exception e)
            {
                _logger.LogError($"Wasn't able to update record with id '{entity.Id}'. Message: {e.Message}");
            }

            return result;
        }

        private void CreateDatabase(string filePath)
        {
            try
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
            catch(Exception e)
            {
                _logger.LogError($"Wasn't able to create database. Message: {e.Message}");
            }
        }
    }
}