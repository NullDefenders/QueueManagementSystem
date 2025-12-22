using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Domain.Entities;

using PreRegistrationService.Services;
using Infrastructure.EntityFrameWork;
using PreRegistrationService.Repositories;

namespace PreRegistrationService.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly UniqueStringGenerator _uniqueStringGenerator;
        private readonly RecordRepository _recordRepository;

        public RecordController(UniqueStringGenerator stringGenerator, RecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
            _uniqueStringGenerator = stringGenerator;
        }

        [HttpGet("{id}")]
        public async Task<Record> GetRecordByIdAccount(string id)
        {
            var filter = Builders<Record>.Filter.Eq(x => x.AccountID, id);
            var record = await _recordRepository.GetRecordByIdAsync(id);
            return record;
        }

        [HttpPost("record")]
        public async Task<ActionResult> Post(RecordForService customer)
        {
            Record record = new Record(null, customer.accountId, customer.name, customer.surname, customer.recordTime.AddHours(3), _uniqueStringGenerator.Next(), customer.serviceId, customer.categoryPrefix, customer.serviceName);
            await _recordRepository.AddRecordAsync(record);
            return CreatedAtAction(nameof(GetRecordByIdAccount), new { id = record.AccountID }, record);
        }

        [HttpGet("record/{code}")]
        public async Task<Record> GetRecordByCode(string code)
        {
            var record = await _recordRepository.GetRecordByCodeAsync(code);
            return record;
        }
        /// <summary>
        /// data в формате serviceID-YYYY-MM-DD
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet("allrecordtimeinday/{date}/{serviceId}")]
        public async Task<List<string>> GetAllRecordingTimesInDay(string date, string serviceId)
        {
            var times = await _recordRepository.GetAllRecordingTimesInDay(serviceId, date);
            return times;
        }
    }
}
