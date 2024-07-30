using API_Minimal_Mongo.Domains;
using API_Minimal_Mongo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;

namespace API_Minimal_Mongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]

    public class ProductController : ControllerBase
    {
        private readonly IMongoCollection<Product> _product;


        /// <summary>
        /// Construtor que recebe como dependencia o objeto da classe MongoDbService
        /// </summary>
        /// <param name="mongoDbService">Objeto da classe MongoDbService</param>
        public ProductController(MongoDbService mongoDbService)
        {

            //Obtem a collection "product"
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }


        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
                return Ok(products);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);

            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            try
            {
                await _product.InsertOneAsync(product);

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut]

        public async Task<ActionResult<Product>> Edit(String Id, Product product)
        {
            try
            {

                var filter = Builders<Product>.Filter.Eq(x => x.Id, product.Id);

                await _product.ReplaceOneAsync(filter, product);

                return Ok();

            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpGet("{id}")]

        public async Task<ActionResult<Product>> GetById(String id)
        {
            try
            {
                var product = await _product.Find(x => x.Id == id).FirstOrDefaultAsync();

                return product is not null ? Ok(product) : NotFound();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpDelete]

        public async Task<ActionResult<Product>> Delete(String id)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);

            if (filter is null)
            {
                return NotFound();
            }

            await _product.DeleteOneAsync(filter);

            return Ok();

        }
    }
}