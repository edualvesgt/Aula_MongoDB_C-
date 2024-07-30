using MongoDB.Driver;

namespace API_Minimal_Mongo.Services
{
    public class MongoDbService
    {
        /// <summary>
        /// Armazena a Configuracao da aplicacao 
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Armazena uma referencia ao MongoDb
        /// </summary>
        private readonly IMongoDatabase _database;


        /// <summary>
        /// Recebe a config da aplicacao como parametro 
        /// </summary>
        /// <param name="configuration">Objeto Configuration</param>
        public MongoDbService (IConfiguration configuration)
        {
            //Atribui a config em _configuration
            _configuration = configuration;

            //Obtem a  string de conecao que foi colnfigurada na appsettings
            var connectionString = _configuration.GetConnectionString("DbConnection");

            //Cria um Objeto que recebe o parametro a string
            var mongoUrl = MongoUrl.Create(connectionString);
            
            //Cria um client MongoClient para se connectar
            var mongoClient = new MongoClient(mongoUrl);

            //Obtem a referencia ao banco 
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        /// <summary>
        /// Propiedade para acessar o Banco de Dados 
        /// Retorna a referencia ao Banco 
        /// </summary>
        public IMongoDatabase GetDatabase => _database;

       

    }
}
