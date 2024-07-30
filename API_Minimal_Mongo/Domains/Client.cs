using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_Minimal_Mongo.Domains
{
    public class Client
    {
        [BsonId]

        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("UserId")]
        public string? UserId { get; set; }

        [BsonElement("Cpf")]
        public double? Cpf { get; set; }

        [BsonElement("Phone")]
        public double? Phone { get; set; }

        [BsonElement("Address")]
        public string? Address { get; set; }

        public Dictionary<string, string> AdditionalAttributes { get; set; }


        public Client()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}