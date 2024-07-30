using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace API_Minimal_Mongo.Domains
{
    public class Order
    {
        [BsonId]

        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string? Id { get; set; }

        [BsonElement("Date")]
        public DateTime? OrderDate { get; set; }

        [BsonElement("Status")]
        public string? Status { get; set; }

        [BsonElement("ClientId")]
        [JsonIgnore]
        public string? ClientId { get; set; }

        public Client? Client { get; set; }

        //Referecia para cadastrar o pedido 
        [JsonIgnore]
        [BsonElement("productId")]
        public List<string>? ProductId { get; set; }
        //Referencia para listar os pedidos podermos receber o dados dos produtos
        
        public List<Product>? Products { get; set; }

        public Dictionary<string, string> AdditionalAttributes { get; set; }


        public Order()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}