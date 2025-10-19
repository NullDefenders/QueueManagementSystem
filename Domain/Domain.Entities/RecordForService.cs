using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RecordForService
    {
        public string accountId { get; set; }
        public string serviceId { get; set; }
        public DateTime recordTime { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string categoryPrefix { get; set; }
        public string serviceName { get; set; }

    }
}
