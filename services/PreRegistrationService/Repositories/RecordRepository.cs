using DnsClient.Protocol;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using PreRegistrationService.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PreRegistrationService.Repositories
{
    public class RecordRepository
    {
        private readonly IMongoCollection<Record> _records;

        public RecordRepository(MongoDbService mongoDbService)
        {
            _records = mongoDbService.Database?.GetCollection<Record>("Record");
        }

        public async Task<HashSet<long>> GetAllCodesAsync()
        {
            var recordList = await _records.FindAsync(new BsonDocument());
            if (recordList == null)
            {
                return null;
            }
            List<long> codes = new List<long>();
            foreach (var record in recordList.ToList<Record>())
            {
                codes.Add(long.Parse(record.RecordCode));
            }
            return new HashSet<long>(codes);
        }

        public async Task<Record> GetRecordByIdAsync(string id) 
        {
            var filter = Builders<Record>.Filter.Eq(x => x.AccountID, id);
            var record = await _records.FindAsync(filter);
            if (record != null)
            {
                var recordList = await record.ToListAsync<Record>();
                return recordList[0];
            }
            return null;
        }

        public async Task AddRecordAsync(Record record)
        {
            await _records.InsertOneAsync(record);
        }

        public async Task<Record> GetRecordByCodeAsync(string code)
        {
            var filter = Builders<Record>.Filter.Eq(x => x.RecordCode, code);
            var record = await _records.FindAsync(filter);
            return record.ToList<Record>()[0];
        }
    }
}
