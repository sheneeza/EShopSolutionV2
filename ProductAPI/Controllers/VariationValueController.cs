using Microsoft.AspNetCore.Mvc;
using ProductAPI.ApplicationCore.Contracts.Services;
using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class VariationValueController : ControllerBase
    {
        private readonly IVariationValueService _variationValueService;

        public VariationValueController(IVariationValueService variationValueService)
        {
            _variationValueService = variationValueService;
        }

        // POST: /api/VariationValue/Save
        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] VariationValue variationValue)
        {
            if (variationValue == null)
                return BadRequest("VariationValue is null.");

            if (variationValue.Id == 0)
            {
                // Insert
                var rows = await _variationValueService.InsertAsync(variationValue);
                return Ok(new { Message = "VariationValue created", RowsAffected = rows, variationValue.Id });
            }
            else
            {
                // Update
                var rows = await _variationValueService.UpdateAsync(variationValue);
                return Ok(new { Message = "VariationValue updated", RowsAffected = rows });
            }
        }
        
        
        // GET: /api/VariationValue/GetByVariationId/{variationId}
        [HttpGet("GetByVariationId")]
        public async Task<IActionResult> GetByVariationId(int variationId)
        {
            var values = await _variationValueService.GetByVariationIdAsync(variationId);
            return Ok(values);
        }

       
    }