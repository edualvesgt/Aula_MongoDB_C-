using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API_Minimal_Mongo.ViewModels
{
    public class OrderViewModel
    {
        [BsonId]

        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public string? ClientId { get; set; }

        public List<string>? ProductId { get; set; }
        public Dictionary<string, string>? AdditionalAttributes { get; set; }
    }
}
