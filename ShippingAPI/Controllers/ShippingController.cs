using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingAPI.ApplicationCore.Contracts.Services;
using ShippingAPI.ApplicationCore.Entities;
using ShippingAPI.ApplicationCore.Models;

namespace ShippingAPI.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class ShipperController : ControllerBase
    {
        private const string AllShippersCacheKey = "cache:allShippers";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        private readonly IShipperService _shipperService;
        private readonly ICacheService   _cacheService;
        private readonly IHttpClientFactory   _httpClientFactory;

        public ShipperController(
            IShipperService shipperService,
            ICacheService   cacheService,
            IHttpClientFactory httpClientFactory)
        {
            _shipperService = shipperService;
            _cacheService   = cacheService;
            _httpClientFactory = httpClientFactory;
        }

        
        // GET /api/Shipper
        [HttpGet]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<IEnumerable<Shipper>>> GetAllShippers()
        {
            // try cache first
            var cached = await _cacheService.GetAsync<List<Shipper>>(AllShippersCacheKey);
            if (cached != null && cached.Count > 0)
                return Ok(cached);

            // fall back to database
            var shippersList = (await _shipperService.GetAllShippersAsync()).ToList();

            // store in cache
            await _cacheService.SetAsync(AllShippersCacheKey, shippersList, CacheDuration);

            return Ok(shippersList);
        }
        
        // GET /api/Shipper/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<Shipper>> GetShipperById(int id)
        {
            var shipper = await _shipperService.GetShipperByIdAsync(id);
            if (shipper == null)
            {
                return NotFound($"No shipper found with ID {id}");
            }
            return Ok(shipper);
        }

        
        // Gets shippers for a particular region.
        // GET /api/Shipper/region/{regionId}
        [HttpGet("region/{region}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<IEnumerable<Shipper>>> GetShippersByRegion(int regionId)
        {
            var shippers = await _shipperService.GetShippersByRegionAsync(regionId);
            return Ok(shippers);
        }

        
        // POST /api/Shipper
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateShipper([FromBody] ShipperRequestModel model)
        {
            var newShipper = new Shipper {
                Name          = model.Name,
                EmailId       = model.Email,
                Phone         = model.Phone,
                ContactPerson = model.ContactPerson
            };

            var result = await _shipperService.CreateShipperAsync(newShipper);
            if (result > 0)
            {
                // invalidate the cache so next GET hits the DB
                await _cacheService.RemoveAsync(AllShippersCacheKey);
                return Ok("Shipper created successfully.");
            }

            return BadRequest("Could not create the shipper.");
        }


        
        // PUT /api/Shipper
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateShipper([FromBody] ShipperRequestModel model)
        {
            var existing = await _shipperService.GetShipperByIdAsync(model.Id);
            if (existing == null)
                return NotFound($"No shipper found with ID {model.Id}");

            existing.Name          = model.Name;
            existing.EmailId       = model.Email;
            existing.Phone         = model.Phone;
            existing.ContactPerson = model.ContactPerson;

            var result = await _shipperService.UpdateShipperAsync(existing);
            if (result > 0)
            {
                await _cacheService.RemoveAsync(AllShippersCacheKey);
                return Ok("Shipper updated successfully.");
            }

            return BadRequest("Could not update the shipper.");
        }

        
        // DELETE /api/Shipper/delete/{id}
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteShipper(int id)
        {
            var result = await _shipperService.DeleteShipperAsync(id);
            if (result > 0)
            {
                await _cacheService.RemoveAsync(AllShippersCacheKey);
                return Ok($"Shipper with ID {id} deleted successfully.");
            }

            return NotFound($"No shipper found with ID {id}");
        }
        
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] ShippingStatusRequestModel model)
        {
            var ok = await _shipperService.UpdateShipmentStatusAsync(model.OrderId, model.Status);
            if (!ok)
                return StatusCode(500, "Failed to notify Order service.");

            return Ok("Shipping status updated.");
        }
    }