﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExportPortal.API.Models.DTO;
using ExportPortal.API.Models.Domain;

namespace ExportPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;

        public VendorController(UserManager<UserProfile> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vendorsResult = await userManager.GetUsersInRoleAsync("Vendor");

            if (vendorsResult != null)
            {
                List<VendorResponseDTO> allVendors = new List<VendorResponseDTO>();

                foreach (var singleVendor in vendorsResult)
                {
                    var vendor = new VendorResponseDTO
                    {
                        Id = singleVendor.Id,
                        Name = singleVendor.Name,
                        OrganizationName = singleVendor.OrganizationName,
                        PhoneNumber = singleVendor.PhoneNumber,
                        Email = singleVendor.Email,
                        State = singleVendor.State,
                        City = singleVendor.City,
                        Address = singleVendor.Address,
                        Zipcode = singleVendor.Zipcode,
                        VendorCategory = singleVendor.VendorCategory,
                    };

                    allVendors.Add(vendor);
                }

                return Ok(allVendors);
            }

            return BadRequest("Something went wrong");
        }

        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] VendorDTO vendorDTO)
        {

            var vendorProfile = new UserProfile
            {
                UserName = vendorDTO.Email,
                Name = vendorDTO.Name,
                OrganizationName = vendorDTO.OrganizationName,
                PhoneNumber = vendorDTO.PhoneNumber,
                Email = vendorDTO.Email,
                State = vendorDTO.State,
                City = vendorDTO.City,
                Address = vendorDTO.Address,
                Zipcode = vendorDTO.Zipcode,
                VendorCategoryId = vendorDTO.VendorCategoryId,
            };

            var vendorResult = await userManager.CreateAsync(vendorProfile, "Pass@123");

            if (vendorResult.Succeeded)
            {
                List <string> roles = new List<string>();
                roles.Add("Vendor");
                var vendorRoleResult = await userManager.AddToRolesAsync(vendorProfile, roles);
                if (vendorRoleResult.Succeeded)
                {
                    return Ok("Vendor was registered! Please login.");
                }
            }

            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var vendorResult = await userManager.FindByIdAsync(id);
            if (vendorResult!= null)
            {
                var vendor = new VendorResponseDTO
                {
                    Id = vendorResult.Id,
                    Name = vendorResult.Name,
                    OrganizationName = vendorResult.OrganizationName,
                    PhoneNumber = vendorResult.PhoneNumber,
                    Email = vendorResult.Email,
                    State = vendorResult.State,
                    City = vendorResult.City,
                    Address = vendorResult.Address,
                    Zipcode = vendorResult.Zipcode,
                    VendorCategory = vendorResult.VendorCategory,
                };
                return Ok(vendor);
            }
           
            return BadRequest();
        }

    }
}
