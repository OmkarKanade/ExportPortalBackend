using ExportPortal.API.Data;
using ExportPortal.API.Models.Domain;
using ExportPortal.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExportPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationController : ControllerBase
    {
        private readonly ExportPortalDbContext dbContext;
        public QuotationController(ExportPortalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuotations()
        {
            var quotationDomain = await dbContext.Quotations.Include(u => u.Customer)
                .Include(q => q.Items).ThenInclude(qi => qi.Product).ToListAsync();

            List<QuotationResponseDTO> allQuotations = new List<QuotationResponseDTO>();

            foreach (var item in quotationDomain)
            {
                var quotationResponseDTO = new QuotationResponseDTO
                {
                    Id = item.Id,
                    CustomerId = item.CustomerId,
                    CustomerName = item.Customer.Name,
                    Status = item.Status,
                    Items = item.Items.Select(qi => new QuotationItemDTO
                    {
                        Id = qi.Id,
                        ProductId = qi.Product.Id,
                        ProductName = qi.Product.Name,
                        Quantity = qi.Quantity
                    }).ToList()
                };
                allQuotations.Add(quotationResponseDTO);
            }
            return Ok(allQuotations);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<Quotation>> AddQuotationItem(QuotationDTO quotationDTO)
        {
            var customerId = quotationDTO.CustomerId;

            var quotation = await dbContext.Quotations.Include(i => i.Items).FirstOrDefaultAsync(x => x.CustomerId == customerId);

            if (quotation == null)
            {
                var quotationDomain = new Quotation
                {
                    CustomerId = quotationDTO.CustomerId,
                    Status = false
                };
                await dbContext.Quotations.AddAsync(quotationDomain);
                await dbContext.SaveChangesAsync();
                var quotationItemInitial = new QuotationItem
                {
                    QuotationId = quotationDomain.Id,
                    ProductId = quotationDTO.ProductId,
                    Quantity = quotationDTO.Quantity
                };
                await dbContext.QuotationItems.AddAsync(quotationItemInitial);
                await dbContext.SaveChangesAsync();
                return Ok("Product added to Quotation");
            }

            var existingItem = quotation.Items.FirstOrDefault(qi => qi.ProductId == quotationDTO.ProductId);

            if (existingItem == null)
            {
                var quotationItem = new QuotationItem
                {
                    QuotationId = quotation.Id,
                    ProductId = quotationDTO.ProductId,
                    Quantity = quotationDTO.Quantity
                };
                await dbContext.QuotationItems.AddAsync(quotationItem);
                await dbContext.SaveChangesAsync();
                return Ok("Product added to Quotation");
            }
            else
            {
                existingItem.Quantity = quotationDTO.Quantity;
                await dbContext.SaveChangesAsync();
                return Ok("Quantity updated in Quotation");
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var quotationDomain = await dbContext.Quotations.Include(u => u.Customer)
                .Include(q => q.Items).ThenInclude(qi => qi.Product).FirstOrDefaultAsync(x => x.Id == id);

            if (quotationDomain == null)
            {
                return NotFound();
            }

            var quotationResponseDTO = new QuotationResponseDTO
            {
                Id = quotationDomain.Id,
                CustomerId = quotationDomain.CustomerId,
                CustomerName = quotationDomain.Customer.Name,
                Status = quotationDomain.Status,
                Items = quotationDomain.Items.Select(qi => new QuotationItemDTO
                {
                    Id = qi.Id,
                    ProductId = qi.Product.Id,
                    ProductName = qi.Product.Name,
                    Quantity = qi.Quantity
                }).ToList()
            };

            return Ok(quotationResponseDTO);
        }

        [HttpPost]
        [Route("UpdateItem")]
        public async Task<IActionResult> UpdateQuotationItem([FromBody] QuotationItemUpdateDTO updateDTO)
        {
            var quotationItemDomain = await dbContext.QuotationItems.Include(p => p.Product).FirstOrDefaultAsync(x => x.Id == updateDTO.Id);

            if (quotationItemDomain == null)
            {
                return NotFound();
            }

            quotationItemDomain.Quantity = updateDTO.Quantity;

            await dbContext.SaveChangesAsync();

            var updatedItem = new QuotationItemDTO
            {
                Id = quotationItemDomain.Id,
                ProductId = quotationItemDomain.Product.Id,
                ProductName = quotationItemDomain.Product.Name,
                Quantity = quotationItemDomain.Quantity
            };

            return Ok(updatedItem);
        }

    }
}
