using ATEA_TechnicalTask.Shared;
using ATEA_TechnicalTask.Tests.Fixtures;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using System.Diagnostics.CodeAnalysis;

namespace ATEA_TechnicalTask.Tests
{
    public class ArgumentsRepositoryTests : IDisposable, IClassFixture<ArgumentsRepositoryTestsFixture>
    {
        private IRepository<ArgumentsRecord> _repository;

        public ArgumentsRepositoryTests()
        {
            _repository = new ArgumentsRepository(new ConsoleLogger(), ArgumentsRepositoryTestsFixture.DatabaseFilename);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        [Fact]
        public async Task Insert_InsertTestRecord_RecordIsInserted()
        {
            ArgumentsRecord record = new("Insert", "record");

            ArgumentsRecord returnedRecord = await _repository.Insert(record);

            Assert.NotEqual(record.Id, returnedRecord.Id);
        }

        [Fact]
        public async Task GetAll_InsertAndGetTestRecords_AllRecordsAreReturned()
        {
            ArgumentsRecord[] records = new ArgumentsRecord[]
            {
                new("GetAll1", "GetAll2"),
                new("GetAll3", "GetAll4"),
                new("GetAll5", "GetAll6")
            };
            foreach (ArgumentsRecord record in records)
                await _repository.Insert(record);

            List<ArgumentsRecord> returnedRecords = await _repository.GetAll();

            Assert.True(records.All(e => returnedRecords.Contains(e, new ArgumentsComparer())));
        }

        [Fact]
        public async Task GetById_InsertAndGetTestRecordById_RecordIsReturned()
        {
            ArgumentsRecord record = new("GetById", "record");
            int id = (await _repository.Insert(record)).Id;

            ArgumentsRecord returnedRecord = await _repository.GetById(id);

            Assert.Equal(record, returnedRecord, new ArgumentsComparer());
        }

        [Fact]
        public async Task Delete_InsertAndDeleteTestRecord_RecordIsDeleted()
        {
            ArgumentsRecord record = new("Delete", "record");
            record.Id = (await _repository.Insert(record)).Id;

            await _repository.Delete(record);
            ArgumentsRecord emptyRecord = await _repository.GetById(record.Id);

            Assert.Equal(-1, emptyRecord.Id);
            Assert.Null(emptyRecord.Arg1);
            Assert.Null(emptyRecord.Arg2);
        }

        [Fact]
        public async Task Update_InsertAndUpdateTestRecord_RecordIsUpdated()
        {
            const string updatedArg1 = "Update_updated";
            const string updatedArg2 = "record_updated";
            ArgumentsRecord record = new("Update", "record");
            record.Id = (await _repository.Insert(record)).Id;
            record.Arg1 = updatedArg1;
            record.Arg2 = updatedArg2;

            await _repository.Update(record);
            ArgumentsRecord returnedRecord = await _repository.GetById(record.Id);

            Assert.Equal(record, returnedRecord, new ArgumentsComparer());
        }

        private class ArgumentsComparer : IEqualityComparer<ArgumentsRecord>
        {
            public bool Equals(ArgumentsRecord? x, ArgumentsRecord? y)
            {
                if (x == null || y == null) return false;
                return x.Arg1 == y.Arg1 && x.Arg2 == y.Arg2;
            }

            public int GetHashCode([DisallowNull] ArgumentsRecord obj)
            {
                return HashCode.Combine(obj.Arg1, obj.Arg2);
            }
        }
    }
}
