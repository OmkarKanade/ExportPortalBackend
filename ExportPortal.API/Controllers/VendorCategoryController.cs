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
    public class VendorCategoryController : ControllerBase
    {
        private readonly ExportPortalDbContext dbContext;

        public VendorCategoryController(ExportPortalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            List<VendorCategory> vendorCategoryDomain = await dbContext.VendorCategories.ToListAsync();
            
            return Ok(vendorCategoryDomain);
        }


        [HttpPost]
        public async Task<ActionResult<VendorCategory>> AddCategory(VendorCategoryDTO categoryDTO)
        {
            var categoryDomain = new VendorCategory
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
            };

            await dbContext.VendorCategories.AddAsync(categoryDomain);
            await dbContext.SaveChangesAsync();
            return Ok(categoryDomain);
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {

            var vendorCategoryDomain = await dbContext.VendorCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (vendorCategoryDomain == null)
            {
                return NotFound();
            }

            var vendorCategoryDTO = new VendorCategoryDTO
            {
                Name = vendorCategoryDomain.Name,
                Description = vendorCategoryDomain.Description
            };

            return Ok(vendorCategoryDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] VendorCategoryDTO vendorCategoryDTO)
        {

            var vendorCategoryDomain = await dbContext.VendorCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (vendorCategoryDomain == null)
            {
                return NotFound();
            }

            vendorCategoryDomain.Name = vendorCategoryDTO.Name;
            vendorCategoryDomain.Description = vendorCategoryDTO.Description;

            await dbContext.SaveChangesAsync();

            return Ok(vendorCategoryDTO);
        }

    }
}
