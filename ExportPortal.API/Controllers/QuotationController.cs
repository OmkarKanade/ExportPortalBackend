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

        [HttpGet]
        [Route("Admin")]
        public async Task<IActionResult> GetAllQuotationsForAdmin()
        {
            var quotationDomain = await dbContext.Quotations.Include(u => u.Customer)
                .Include(q => q.Items).ThenInclude(qi => qi.Product).Where(x => !x.Status).ToListAsync();

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
        [Route("AddItem")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<Quotation>> AddQuotationItem(QuotationDTO quotationDTO)
        {
            var customerId = quotationDTO.CustomerId;

            var activeQuotation = await dbContext.Quotations.Include(i => i.Items).FirstOrDefaultAsync(q => q.CustomerId == customerId && q.Status);

            if (activeQuotation == null)
            {
                var quotationDomain = new Quotation
                {
                    CustomerId = quotationDTO.CustomerId,
                    Status = true,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
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

            var existingItem = activeQuotation.Items.FirstOrDefault(qi => qi.ProductId == quotationDTO.ProductId);

            if (existingItem == null)
            {
                var quotationItem = new QuotationItem
                {
                    QuotationId = activeQuotation.Id,
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

            if (quotationDomain != null)
            {
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
            return BadRequest("Something went wrong");

        }

        [HttpGet]
        [Route("Customer/{id:Guid}")]
        public async Task<IActionResult> GetByCustomerId([FromRoute] string id)
        {
            var quotationDomains = await dbContext.Quotations.Include(u => u.Customer)
                .Include(q => q.Items).ThenInclude(qi => qi.Product).Where(x => x.CustomerId == id && !x.Status).ToListAsync();

            if (quotationDomains != null)
            {
                List<QuotationResponseDTO> quotationResponseDTO = new List<QuotationResponseDTO>();
                foreach (var quotationDomain in quotationDomains)
                {
                    var singleQuotationResponseDTO = new QuotationResponseDTO
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
                    quotationResponseDTO.Add(singleQuotationResponseDTO);
                }
                return Ok(quotationResponseDTO);
            }
            return BadRequest("Something went wrong");

        }

        [HttpGet]
        [Route("CustomerActive/{id:Guid}")]
        public async Task<IActionResult> GetByCustomerIdActive([FromRoute] string id)
        {
            var quotationDomains = await dbContext.Quotations.Include(u => u.Customer)
                .Include(q => q.Items).ThenInclude(qi => qi.Product).Where(x => x.CustomerId == id && x.Status).ToListAsync();

            if (quotationDomains != null)
            {
                List<QuotationResponseDTO> quotationResponseDTO = new List<QuotationResponseDTO>();
                foreach (var quotationDomain in quotationDomains)
                {
                    var singleQuotationResponseDTO = new QuotationResponseDTO
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
                    quotationResponseDTO.Add(singleQuotationResponseDTO);
                }
                return Ok(quotationResponseDTO);
            }
            return BadRequest("Something went wrong");
        }

        [HttpPut]
        [Route("UpdateItem/{itemId:Guid}")]
        public async Task<IActionResult> UpdateQuotationItem([FromRoute] Guid itemId, [FromBody] QuotationItemUpdateDTO updateDTO)
        {
            var quotationItemDomain = await dbContext.QuotationItems.Include(qi => qi.Quotation).Include(p => p.Product).FirstOrDefaultAsync(x => x.Id == itemId);
            
            if (quotationItemDomain != null && quotationItemDomain.Quotation.Status)
            {
                quotationItemDomain.Quantity = updateDTO.Quantity;
                quotationItemDomain.Quotation.LastModifiedOn = DateTime.Now;

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
            return BadRequest("Something went wrong");
        }


        [HttpDelete]
        [Route("DeleteItem/{itemId:Guid}")]
        public async Task<IActionResult> DeleteQuotationItem([FromRoute] Guid itemId)
        {
            var itemToDelete = await dbContext.QuotationItems.Include(qi => qi.Quotation).Include(p => p.Product).FirstOrDefaultAsync(qi => qi.Id == itemId);

            if (itemToDelete != null && itemToDelete.Quotation.Status)
            {
                itemToDelete.Quotation.LastModifiedOn = DateTime.Now;
                dbContext.QuotationItems.Remove(itemToDelete);
                await dbContext.SaveChangesAsync();

                var deletedItem = new QuotationItemDTO
                {
                    Id = itemToDelete.Id,
                    ProductId = itemToDelete.Product.Id,
                    ProductName = itemToDelete.Product.Name,
                    Quantity = itemToDelete.Quantity
                };
                return Ok(deletedItem);
            }
            return BadRequest("Something went wrong");
        }

        [HttpPut]
        [Route("SendQuotation/{id:Guid}")]
        public async Task<IActionResult> SetQuotationStatus([FromRoute] Guid id)
        {
            var quotation = await dbContext.Quotations.Include(u => u.Customer)
                .Include(q => q.Items).ThenInclude(qi => qi.Product).FirstOrDefaultAsync(q => q.Id == id);

            if (quotation != null)
            {
                quotation.Status = false;
                quotation.LastModifiedOn = DateTime.Now;

                await dbContext.SaveChangesAsync();

                var singleQuotationResponseDTO = new QuotationResponseDTO
                {
                    Id = quotation.Id,
                    CustomerId = quotation.CustomerId,
                    CustomerName = quotation.Customer.Name,
                    Status = quotation.Status,
                    Items = quotation.Items.Select(qi => new QuotationItemDTO
                    {
                        Id = qi.Id,
                        ProductId = qi.Product.Id,
                        ProductName = qi.Product.Name,
                        Quantity = qi.Quantity
                    }).ToList()
                };

                return Ok(singleQuotationResponseDTO);
            }
            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("AssignItemsToVendors/{id:Guid}")]
        public async Task<IActionResult> AssignQuotationItemsToVendors(Guid id)
        {
            var quotation = await dbContext.Quotations.Include(q => q.Items).ThenInclude(qi => qi.Product)
                .Include(q => q.Customer).FirstOrDefaultAsync(q => q.Id == id);

            if (quotation != null)
            {
                foreach (var item in quotation.Items)
                {
                    var vendorIds = new List<string?> { item.Product.VendorId1, item.Product.VendorId2, item.Product.VendorId3 };

                    foreach (var vendorId in vendorIds)
                    {
                        if (vendorId != null)
                        {
                            var assignment = new QuotationItemAssignment
                            {
                                CustomerId = quotation.CustomerId,
                                ItemId = item.Id,
                                VendorId = vendorId
                            };

                            dbContext.QuotationItemAssignments.Add(assignment);
                        }
                    }
                }

                await dbContext.SaveChangesAsync();

                return Ok("Quotation items assigned to vendors successfully");
            }
            return BadRequest("Something went wrong");
        }
    }
}
