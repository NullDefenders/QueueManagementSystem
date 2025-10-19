using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class Record
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("accontID"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string? AccountID { get; set; }
        [BsonElement("name"), BsonRepresentation(MongoDB.Bson.BsonType.String)]

        public string? Name { get; set; }
        [BsonElement("surname"), BsonRepresentation(MongoDB.Bson.BsonType.String)]

        public string? Surname { get; set; }

        [BsonElement("recordTime"), BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTime RecordTime { get; set; }
        [BsonElement("recordCode"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string? RecordCode { get; set; }
        [BsonElement("serviceId"), BsonRepresentation(MongoDB.Bson.BsonType.String)]

        public string? ServiceID { get; set; }
        [BsonElement("categoryPrefix"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string? CategoryPrefix { get; set; }
        [BsonElement("serviceName"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string? ServiceName { get; set; }


        public Record(string id, string accountID, string name, string surname, DateTime recordTime, string recordCode, string serviceID, string categoryPrefix, string serviceName)
        {
            Id = null;
            AccountID = accountID;
            Name = name;
            Surname = surname;
            RecordTime = recordTime;
            RecordCode = recordCode;
            ServiceID = serviceID;
            CategoryPrefix = categoryPrefix;
            ServiceName = serviceName;
        }
    }
}
