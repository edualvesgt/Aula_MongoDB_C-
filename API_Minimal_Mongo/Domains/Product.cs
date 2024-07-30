using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_Minimal_Mongo.Domains
{
    public class Product
    {
        [BsonId]
        //Define o nome do campo no MongoDb com _ide o tipo com ObjectId
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("name")]
        public string? Name { get; set; }
        [BsonElement("price")]
        public decimal Price { get; set; }

        //Adiciona um Dicionario para atributos 
        public Dictionary<string, string> AdditionalAttributes { get; set; }

        /// <summary>
        /// Ao ser instanciado um objeto da classe Product ,
        /// o atributo AdditionalAttributes ja vira com um novo dicionario e portanto habi;itado para adicionar mais atributos
        /// </summary>
        public Product()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }

    }
}
