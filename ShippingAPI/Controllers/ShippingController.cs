using Microsoft.AspNetCore.Mvc;
using ShippingAPI.ApplicationCore.Contracts.Services;
using ShippingAPI.ApplicationCore.Entities;
using ShippingAPI.ApplicationCore.Models;

namespace ShippingAPI.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperService _shipperService;

        public ShipperController(IShipperService shipperService)
        {
            _shipperService = shipperService;
        }

        
        //Gets all shippers.
        // GET /api/Shipper
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shipper>>> GetAllShippers()
        {
            var shippers = await _shipperService.GetAllShippersAsync();
            return Ok(shippers);
        }
        
        // GET /api/Shipper/{id}
        [HttpGet("{id}")]
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
        public async Task<ActionResult<IEnumerable<Shipper>>> GetShippersByRegion(int regionId)
        {
            var shippers = await _shipperService.GetShippersByRegionAsync(regionId);
            return Ok(shippers);
        }

        
        // Creates a new shipper.
        // POST /api/Shipper
        [HttpPost]
        public async Task<ActionResult> CreateShipper([FromBody] ShipperRequestModel model)
        {
            var newShipper = new Shipper
            {
                Name = model.Name,
                EmailId = model.Email,
                Phone = model.Phone,
                ContactPerson = model.ContactPerson
            };

            var result = await _shipperService.CreateShipperAsync(newShipper);
            if (result > 0)
                return Ok("Shipper created successfully.");
            else
                return BadRequest("Could not create the shipper.");
        }

        
        // Updates an existing shipper.
        // PUT /api/Shipper
        [HttpPut]
        public async Task<ActionResult> UpdateShipper([FromBody] ShipperRequestModel model)
        {
            // Typically you might retrieve the existing entity first, but here's a direct approach:
            var existingShipper = await _shipperService.GetShipperByIdAsync(model.Id);
            if (existingShipper == null)
                return NotFound($"No shipper found with ID {model.Id}");

            existingShipper.Name = model.Name;
            existingShipper.EmailId = model.Email;
            existingShipper.Phone = model.Phone;
            existingShipper.ContactPerson = model.ContactPerson;

            var result = await _shipperService.UpdateShipperAsync(existingShipper);
            if (result > 0)
                return Ok("Shipper updated successfully.");
            else
                return BadRequest("Could not update the shipper.");
        }

        
        // Deletes an existing shipper by ID.
        // DELETE /api/Shipper/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteShipper(int id)
        {
            var result = await _shipperService.DeleteShipperAsync(id);
            if (result > 0)
                return Ok($"Shipper with ID {id} deleted successfully.");
            else
                return NotFound($"No shipper found with ID {id}");
        }
    }