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

        [HttpGet]
        public async Task<IActionResult> GetAllAssignedQuotationsItems()
        {
            var quotationItemAssignmentsDomain = await dbContext.QuotationItemAssignments.Include(u => u.Customer)
                .Include(q => q.Vendor).Include(qi => qi.Item).ThenInclude(p => p.Product).ToListAsync();

            List<QIAssignmentResponseDTO> allAssignItems = new List<QIAssignmentResponseDTO>();

            foreach (var item in quotationItemAssignmentsDomain)
            {
                var assignItemsResponseDTO = new QIAssignmentResponseDTO
                {
                    Id = item.Id,
                    CustomerId = item.CustomerId,
                    CustomerName = item.Customer.Name,
                    VendorId = item.VendorId,
                    VendorName = item.Vendor.Name,
                    Item = new QuotationItemDTO
                    {
                        Id = item.Item.Id,
                        ProductId = item.Item.Product.Id,
                        ProductName = item.Item.Product.Name,
                        Quantity = item.Item.Quantity
                    }
                };
                allAssignItems.Add(assignItemsResponseDTO);
            }
            return Ok(allAssignItems);
        }

        [HttpGet]
        [Route("Vendor/{id:Guid}")]
        public async Task<IActionResult> GetAllAssignedQuotationsItemsByVendor([FromRoute] string id)
        {
            var quotationItemAssignmentsDomain = await dbContext.QuotationItemAssignments.Include(u => u.Customer)
                .Include(q => q.Vendor).Include(qi => qi.Item).ThenInclude(p => p.Product).Where(x => x.VendorId == id).ToListAsync();

            List<QIAssignmentResponseDTO> allAssignItems = new List<QIAssignmentResponseDTO>();

            foreach (var item in quotationItemAssignmentsDomain)
            {
                var assignItemsResponseDTO = new QIAssignmentResponseDTO
                {
                    Id = item.Id,
                    CustomerId = item.CustomerId,
                    CustomerName = item.Customer.Name,
                    VendorId = item.VendorId,
                    VendorName = item.Vendor.Name,
                    Item = new QuotationItemDTO
                    {
                        Id = item.Item.Id,
                        ProductId = item.Item.Product.Id,
                        ProductName = item.Item.Product.Name,
                        Quantity = item.Item.Quantity
                    }
                };
                allAssignItems.Add(assignItemsResponseDTO);
            }
            return Ok(allAssignItems);
        }
    }
}