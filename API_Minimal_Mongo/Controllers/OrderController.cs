using API_Minimal_Mongo.Domains;
using API_Minimal_Mongo.Services;
using API_Minimal_Mongo.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace API_Minimal_Mongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {

        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;



        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get ()
        {
           var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();

            return orders is not null ? Ok(orders) : NotFound();
        }

        [HttpPost]

        public async Task<ActionResult<Order>> Create ([FromBody] OrderViewModel orderViewModel)
        {
            try
            {
                var order = new Order
                {
                    Id = orderViewModel.Id,
                    OrderDate = orderViewModel.OrderDate,
                    Status = orderViewModel.Status,
                    ClientId = orderViewModel.ClientId,
                    ProductId = orderViewModel.ProductId, 
                    AdditionalAttributes = orderViewModel.AdditionalAttributes!
                    
                };
                var client = await _client.Find(c => c.Id == orderViewModel.ClientId).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound();
                }
                order.Client = client;

                await _order.InsertOneAsync(order);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


            
        }

        [HttpPut]

        public async Task<ActionResult<Order>> Edit (String Id , Order order)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(x => x.Id, order.Id);

                await _order.ReplaceOneAsync(filter, order);

                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Order>> Delete(String id)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.Id, id);

            if (filter is null)
            {
                return NotFound();
            }

            await _order.DeleteOneAsync(filter);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(String id)
        {
            var user = await _order.Find(x => x.Id == id).FirstOrDefaultAsync();

            return user is not null ? Ok(user) : NotFound();
        }
    }
}
