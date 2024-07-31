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

            if (orders == null )
            {
                return NotFound("Nenhuma Lista Encontrada");
            }

            foreach (var order in orders)
            {
                // Carregar o cliente relacionado
                if (order.ClientId != null)
                {
                    order.Client = await _client.Find(c => c.Id == order.ClientId).FirstOrDefaultAsync();
                }

                // Carregar os produtos relacionados
                if (order.ProductId != null)
                {
                    // Supondo que 'Products' é uma lista e que 'ProductId' pode ser uma lista de IDs
                    order.Products = await _product.Find(p => order.ProductId.Contains(p.Id)).ToListAsync();
                }

               
            }

            return Ok(orders);
        }

        [HttpPost]

        public async Task<ActionResult<Order>> Create ( OrderViewModel orderViewModel)
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
                    return NotFound("Cliente nao Encontrado ");
                }
                order.Client = client;

                await _order.InsertOneAsync(order);

                return StatusCode(201,order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> Edit(string id, OrderViewModel orderViewModel)
        {
            try
            {
               

                // Cria um filtro para encontrar o documento que deve ser atualizado
                var filter = Builders<Order>.Filter.Eq(x => x.Id, id);

                // Cria o objeto Order para substituir o documento existente
                var updatedOrder = new Order
                {
                    Id = orderViewModel.Id,
                    OrderDate = orderViewModel.OrderDate,
                    Status = orderViewModel.Status,
                    ClientId = orderViewModel.ClientId,
                    ProductId = orderViewModel.ProductId,
                    AdditionalAttributes = orderViewModel.AdditionalAttributes
                };

                // Substitui o documento existente com os dados atualizados
                var result = await _order.ReplaceOneAsync(filter, updatedOrder);

               

                return NoContent(); // Retorna 204 No Content em vez de 200 Ok para uma atualização bem-sucedida
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(x => x.Id, id);

                var result = await _order.DeleteOneAsync(filter);

                if (result.DeletedCount == 0)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(string id)
        {
            try
            {
                // Encontra a ordem com o ID fornecido
                var order = await _order.Find(x => x.Id == id).FirstOrDefaultAsync();

                if (order == null)
                {
                    return NotFound();
                }

                // Preenche o campo Client se o ClientId estiver presente
                if (!string.IsNullOrEmpty(order.ClientId))
                {
                    order.Client = await _client.Find(c => c.Id == order.ClientId).FirstOrDefaultAsync();
                }

                // Preenche o campo Products se ProductId estiver presente
                if (order.ProductId != null && order.ProductId.Any())
                {
                    order.Products = await _product.Find(p => order.ProductId.Contains(p.Id)).ToListAsync();
                }

                // Certifica-se de que AdditionalAttributes não seja nulo
                if (order.AdditionalAttributes == null)
                {
                    order.AdditionalAttributes = new Dictionary<string, string>();
                }

                return Ok(order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}

