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
    public class VendorQuotationController : ControllerBase
    {
        private readonly ExportPortalDbContext dbContext;
        public VendorQuotationController(ExportPortalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private decimal? GetVendorPrice(Product product, string vendorId)
        {
            if (vendorId == product.VendorId1)
                return product.Vendor1Price;
            else if (vendorId == product.VendorId2)
                return product.Vendor2Price;
            else if (vendorId == product.VendorId3)
                return product.Vendor3Price;
            else
                return null; 
        }

        [HttpGet]
        [Route("Vendor/{id:Guid}")]
        public async Task<IActionResult> GetAllAssignedQuotationsItemsByVendor([FromRoute] string id)
        {
            var quotationItemAssignmentsDomain = await dbContext.QuotationItemAssignments.Include(q => q.Vendor).
                Include(qi => qi.Item).ThenInclude(p => p.Product).Where(x => x.VendorId == id).ToListAsync();

            var groupedItems = quotationItemAssignmentsDomain.GroupBy(q => q.QuotationId)
                .Select(group => new
                {
                    QuotationId = group.Key,
                    Items = group.Select(item => new QuotationItemVendorDTO
                    {
                        Id = item.Id,
                        ProductId = item.Item.Product.Id,
                        ProductName = item.Item.Product.Name,
                        Quantity = item.Item.Quantity,
                        Price = GetVendorPrice(item.Item.Product, item.VendorId),
                    }).ToList()
                });

            List<QIAssignmentResponseDTO> allAssignItems = new List<QIAssignmentResponseDTO>();

            foreach (var group in groupedItems)
            {
                var assignItemsResponseDTO = new QIAssignmentResponseDTO
                {
                    QuotationId = group.QuotationId,
                    Items = group.Items
                };
                allAssignItems.Add(assignItemsResponseDTO);
            }

            return Ok(allAssignItems);
        }
    }
}