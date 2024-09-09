using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Vou.Services.VoucherAPI.Data;
using Vou.Services.VoucherAPI.Models;

namespace Vou.Services.VoucherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : Controller
    {
        private readonly IMongoCollection<Voucher>? _voucher;
        public VoucherController(MongoDbService mongoDbService)
        {
            _voucher = mongoDbService.Database?.GetCollection<Voucher>("voucher");
        }
        [HttpGet]
        public async Task<IEnumerable<Voucher>> Get()
        {
            return await _voucher.Find(FilterDefinition<Voucher>.Empty).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Voucher?>> GetById(int id)
        {
            var filter = Builders<Voucher>.Filter.Eq(x => x.Id, id);
            var voucher = _voucher.Find(filter).FirstOrDefault();
            return voucher is not null ? Ok(voucher) : NotFound();
        }
        [HttpPost] 
        public async Task<ActionResult> Post (Voucher voucher)
        {
            await _voucher.InsertOneAsync(voucher);
            return CreatedAtAction(nameof(GetById), new {id = voucher.Id},voucher);
        }
        [HttpPut]
        public async Task<ActionResult> Put(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter.Eq(x => x.Id, voucher.Id);
            await _voucher.ReplaceOneAsync(filter, voucher);
            return Ok(voucher);
        }

        [HttpGet("event/{eventId:int}")]
        public async Task<IEnumerable<Voucher>> GetByEventId(int eventId)
        {
            var filter = Builders<Voucher>.Filter.Eq(x => x.EventId, eventId);
            return await _voucher.Find(filter).ToListAsync();
        }
        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            var filter = Builders<Voucher>.Filter.Eq(x => x.Id, id);
            _voucher.DeleteOneAsync(filter);
            return Ok();
        }
    }
}
