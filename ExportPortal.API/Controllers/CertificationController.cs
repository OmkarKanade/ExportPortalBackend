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
    public class CertificationController : ControllerBase
    {
        private readonly ExportPortalDbContext dbContext;
        public CertificationController(ExportPortalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCertifications()
        {
            List<Certification> certificationDomain = await dbContext.Certifications.ToListAsync();

            return Ok(certificationDomain);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<Certification>> AddCertificate(CertificationDTO certificateDTO)
        {
            var certificateDomain = new Certification
            {
                Name = certificateDTO.Name
            };

            await dbContext.Certifications.AddAsync(certificateDomain);
            await dbContext.SaveChangesAsync();
            return Ok(certificateDomain);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            
            var certificateDomain = await dbContext.Certifications.FirstOrDefaultAsync(x => x.Id == id);

            if (certificateDomain == null)
            {
                return NotFound();
            }

            var certificateDTO = new CertificationResponseDTO
            {
                Id = certificateDomain.Id,
                Name = certificateDomain.Name
            };

            return Ok(certificateDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CertificationDTO certificateDTO)
        {

            var certificateDomain = await dbContext.Certifications.FirstOrDefaultAsync(x => x.Id == id);

            if (certificateDomain == null)
            {
                return NotFound();
            }

            certificateDomain.Name = certificateDTO.Name;

            await dbContext.SaveChangesAsync();

            return Ok(certificateDTO);
        }
    }
}
