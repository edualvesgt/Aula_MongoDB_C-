using API_Minimal_Mongo.Domains;
using API_Minimal_Mongo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Formats.Asn1;

namespace API_Minimal_Mongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {

        private readonly IMongoCollection<User> _user;

        public UserController(MongoDbService mongoDbService)
        {
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {

                //Outro jeito para listar todos "(_ => true)" = regra de validacao para listar todos
                var users = await _user.Find(_ => true).ToListAsync();

                return Ok(users);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost]

        public async Task<ActionResult<User>> Create(User user)
        {
            try
            {
                await _user.InsertOneAsync(user);

                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut]

        public async Task<ActionResult<User>> Edit(String Id, User user)
        {

            try
            {
                var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);

                await _user.ReplaceOneAsync(filter, user);

                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        [HttpDelete]
        public async Task<ActionResult<User>> Delete(String id)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);

            if (filter is null)
            {
                return NotFound();
            }

            await _user.DeleteOneAsync(filter);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(String id)
        {
            var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();

            return user is not null ? Ok(user) : NotFound();
        }
    }
}