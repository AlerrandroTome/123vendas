using _123vendas.Domain.DTOs;
using _123vendas.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace _123vendas.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleByIdAsync(Guid id)
        {
            SaleDto? sale = await _saleService.GetSaleByIdAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSalesAsync()
        {
            IEnumerable<SaleDto> sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSaleAsync([FromBody] CreateSaleDto saleDto)
        {
            SaleDto createdSale = await _saleService.CreateSaleAsync(saleDto);
            return CreatedAtAction(nameof(GetSaleByIdAsync), new { id = createdSale.Id }, saleDto);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSaleAsync([FromBody] UpdateSaleDto saleDto)
        {
            SaleDto? result = await _saleService.UpdateSaleAsync(saleDto);
            if (result == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("Cancell/{id}")]
        public async Task<IActionResult> CancelSaleAsync(Guid id)
        {
            bool result = await _saleService.CancelSaleAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }        
    }
}