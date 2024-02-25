using ExportPortal.API.Data;
using ExportPortal.API.Models.Domain;
using ExportPortal.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExportPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ExportPortalDbContext dbContext;
        public ProductController(ExportPortalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            List<Product> products = await dbContext.Products.Include("Certification")
                .Include("VendorCategory").Include(u => u.UserProfile1)
                .Include(u => u.UserProfile2).Include(u => u.UserProfile3).ToListAsync();
            if (products != null)
            {
                List<ProductResponseDTO> productDTO = new List<ProductResponseDTO>();
                foreach (var productDomain in products) {
                    var productResponseDTO = new ProductResponseDTO
                    {
                        Id = productDomain.Id,
                        Name = productDomain.Name,
                        ScientificName = productDomain.ScientificName,
                        VendorCategory = productDomain.VendorCategory,
                        VendorId1 = productDomain.UserProfile1?.Id,
                        VendorName1 = productDomain.UserProfile1?.Name,
                        VendorId2 = productDomain.UserProfile2?.Id,
                        VendorName2 = productDomain.UserProfile2?.Name,
                        VendorId3 = productDomain.UserProfile3?.Id,
                        VendorName3 = productDomain.UserProfile3?.Name,
                        HSNCode = productDomain.HSNCode,
                        ToPuneFreight = productDomain.ToPuneFreight,
                        InnerPackageMaterial = productDomain.InnerPackageMaterial,
                        OuterPackageMaterial = productDomain.OuterPackageMaterial,
                        ManualPackage = productDomain.ManualPackage,
                        MachinePackage = productDomain.MachinePackage,
                        LocalTransport = productDomain.LocalTransport,
                        Fumigation = productDomain.Fumigation,
                        TotalRate = productDomain.TotalRate,
                        GrossWeight = productDomain.GrossWeight,
                        PouchType = productDomain.PouchType,
                        BumperisPouches = productDomain.BumperisPouches,
                        BagOrBox = productDomain.BagOrBox,
                        BagOrBoxBumpers = productDomain.BagOrBoxBumpers,
                        Ingredients = productDomain.Ingredients,
                        ManufacturingProcess = productDomain.ManufacturingProcess,
                        DairyDeclarationRequired = productDomain.DairyDeclarationRequired,
                        IsForHumanConsumption = productDomain.IsForHumanConsumption,
                        Certification = productDomain.Certification,
                    };

                    productDTO.Add(productResponseDTO);
                };
                return Ok(productDTO);
            }
            return BadRequest();
        }


        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(ProductDTO productDto)
        {
            var productDomain = new Product
            {
                Name = productDto.Name,
                ScientificName = productDto.ScientificName,
                VendorCategoryId = productDto.VendorCategoryId,
                VendorId1 = productDto?.VendorId1,
                VendorId2 = productDto?.VendorId2,
                VendorId3 = productDto?.VendorId3,
                HSNCode = productDto.HSNCode,
                ToPuneFreight = productDto.ToPuneFreight,
                InnerPackageMaterial = productDto.InnerPackageMaterial,
                OuterPackageMaterial = productDto.OuterPackageMaterial,
                ManualPackage = productDto.ManualPackage,
                MachinePackage = productDto.MachinePackage,
                LocalTransport = productDto.LocalTransport,
                Fumigation = productDto.Fumigation,
                TotalRate = productDto.TotalRate,
                GrossWeight = productDto.GrossWeight,
                PouchType = productDto.PouchType,
                BumperisPouches = productDto.BumperisPouches,
                BagOrBox = productDto.BagOrBox,
                BagOrBoxBumpers = productDto.BagOrBoxBumpers,
                Ingredients = productDto.Ingredients,
                ManufacturingProcess = productDto.ManufacturingProcess,
                DairyDeclarationRequired = productDto.DairyDeclarationRequired,
                IsForHumanConsumption = productDto.IsForHumanConsumption,
                CertificationId = productDto.CertificationId,
            };

            await dbContext.Products.AddAsync(productDomain);
            await dbContext.SaveChangesAsync();
            return Ok(productDto);
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var productDomain = await dbContext.Products.Include("Certification").Include("VendorCategory")
                .Include(u => u.UserProfile1).Include(u => u.UserProfile2)
                .Include(u => u.UserProfile3).FirstOrDefaultAsync(x => x.Id == id);
            if (productDomain != null)
            {
                var productResponseDto = new ProductResponseDTO
                {
                    Name = productDomain.Name,
                    ScientificName = productDomain.ScientificName,
                    VendorCategory = productDomain.VendorCategory,
                    VendorId1 = productDomain.UserProfile1?.Id,
                    VendorName1 = productDomain.UserProfile1?.Name,
                    VendorId2 = productDomain.UserProfile2?.Id,
                    VendorName2 = productDomain.UserProfile2?.Name,
                    VendorId3 = productDomain.UserProfile3?.Id,
                    VendorName3 = productDomain.UserProfile3?.Name,
                    HSNCode = productDomain.HSNCode,
                    ToPuneFreight = productDomain.ToPuneFreight,
                    InnerPackageMaterial = productDomain.InnerPackageMaterial,
                    OuterPackageMaterial = productDomain.OuterPackageMaterial,
                    ManualPackage = productDomain.ManualPackage,
                    MachinePackage = productDomain.MachinePackage,
                    LocalTransport = productDomain.LocalTransport,
                    Fumigation = productDomain.Fumigation,
                    TotalRate = productDomain.TotalRate,
                    GrossWeight = productDomain.GrossWeight,
                    PouchType = productDomain.PouchType,
                    BumperisPouches = productDomain.BumperisPouches,
                    BagOrBox = productDomain.BagOrBox,
                    BagOrBoxBumpers = productDomain.BagOrBoxBumpers,
                    Ingredients = productDomain.Ingredients,
                    ManufacturingProcess = productDomain.ManufacturingProcess,
                    DairyDeclarationRequired = productDomain.DairyDeclarationRequired,
                    IsForHumanConsumption = productDomain.IsForHumanConsumption,
                    Certification = productDomain.Certification,
                };
                return Ok(productResponseDto);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("Vendor/{id:Guid}")]
        public async Task<IActionResult> GetAllProductsAssigned([FromRoute] string id)
        {
            List<Product> products = await dbContext.Products.Include("Certification")
                .Include("VendorCategory").Include(u => u.UserProfile1)
                .Include(u => u.UserProfile2).Include(u => u.UserProfile3)
                .Where(x => x.VendorId1 == id || x.VendorId2 == id || x.VendorId3 == id)
                .ToListAsync();

            if (products != null)
            {
                List<ProductResponseDTO> productDTO = new List<ProductResponseDTO>();
                foreach (var productDomain in products)
                {
                    var productResponseDTO = new ProductResponseDTO
                    {
                        Id = productDomain.Id,
                        Name = productDomain.Name,
                        ScientificName = productDomain.ScientificName,
                        VendorCategory = productDomain.VendorCategory,
                        VendorId1 = productDomain.UserProfile1?.Id,
                        VendorName1 = productDomain.UserProfile1?.Name,
                        VendorId2 = productDomain.UserProfile2?.Id,
                        VendorName2 = productDomain.UserProfile2?.Name,
                        VendorId3 = productDomain.UserProfile3?.Id,
                        VendorName3 = productDomain.UserProfile3?.Name,
                        HSNCode = productDomain.HSNCode,
                        ToPuneFreight = productDomain.ToPuneFreight,
                        InnerPackageMaterial = productDomain.InnerPackageMaterial,
                        OuterPackageMaterial = productDomain.OuterPackageMaterial,
                        ManualPackage = productDomain.ManualPackage,
                        MachinePackage = productDomain.MachinePackage,
                        LocalTransport = productDomain.LocalTransport,
                        Fumigation = productDomain.Fumigation,
                        TotalRate = productDomain.TotalRate,
                        GrossWeight = productDomain.GrossWeight,
                        PouchType = productDomain.PouchType,
                        BumperisPouches = productDomain.BumperisPouches,
                        BagOrBox = productDomain.BagOrBox,
                        BagOrBoxBumpers = productDomain.BagOrBoxBumpers,
                        Ingredients = productDomain.Ingredients,
                        ManufacturingProcess = productDomain.ManufacturingProcess,
                        DairyDeclarationRequired = productDomain.DairyDeclarationRequired,
                        IsForHumanConsumption = productDomain.IsForHumanConsumption,
                        Certification = productDomain.Certification,
                    };

                    productDTO.Add(productResponseDTO);
                };
                return Ok(productDTO);
            }
            return BadRequest("Something went wrong");
        }

    }
}
