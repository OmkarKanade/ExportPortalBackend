using ExportPortal.API.Data;
using ExportPortal.API.Models.Domain;
using ExportPortal.API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExportPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ExportPortalDbContext dbContext;
        public CurrencyController(ExportPortalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCertifications()
        {
            List<Currency> currencyDomain = await dbContext.Currencies.ToListAsync();

            return Ok(currencyDomain);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<Currency>> AddCurrency(CurrencyDTO currencyDTO)
        {
            var currencyDomain = new Currency
            {
                Name = currencyDTO.Name,
                Code = currencyDTO.Code
            };

            await dbContext.Currencies.AddAsync(currencyDomain);
            await dbContext.SaveChangesAsync();
            return Ok(currencyDomain);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            
            var currencyDomain = await dbContext.Currencies.FirstOrDefaultAsync(x => x.Id == id);

            if (currencyDomain == null)
            {
                return NotFound();
            }

            var currencyDTO = new CurrencyResponseDTO
            {
                Id = currencyDomain.Id,
                Name = currencyDomain.Name,
                Code = currencyDomain.Code
            };

            return Ok(currencyDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CurrencyDTO currencyDTO)
        {

            var currencyDomain = await dbContext.Currencies.FirstOrDefaultAsync(x => x.Id == id);

            if (currencyDomain == null)
            {
                return NotFound();
            }

            currencyDomain.Name = currencyDTO.Name;
            currencyDomain.Code = currencyDTO.Code;

            await dbContext.SaveChangesAsync();

            return Ok(currencyDTO);
        }
    }
}
