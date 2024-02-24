using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExportPortal.API.Models.DTO;
using ExportPortal.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExportPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;

        public AdminController(UserManager<UserProfile> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var adminsResult = await userManager.GetUsersInRoleAsync("Admin");

            if (adminsResult != null)
            {
                List<AdminResponseDTO> allAdmins = new List<AdminResponseDTO>();

                foreach (var singleAdmin in adminsResult)
                {
                    var admin = new AdminResponseDTO
                    {
                        Id = singleAdmin.Id,
                        Name = singleAdmin.Name,
                        OrganizationName = singleAdmin.OrganizationName,
                        PhoneNumber = singleAdmin.PhoneNumber,
                        Email = singleAdmin.Email,
                        State = singleAdmin.State,
                        City = singleAdmin.City,
                        Address = singleAdmin.Address,
                        Zipcode = singleAdmin.Zipcode,
                    };

                    allAdmins.Add(admin);
                }

                return Ok(allAdmins);
            }

            return BadRequest("Something went wrong");
        }

        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AdminDTO adminDTO)
        {

            var adminProfile = new UserProfile
            {
                UserName = adminDTO.Email,
                Name = adminDTO.Name,
                OrganizationName = adminDTO.OrganizationName,
                PhoneNumber = adminDTO.PhoneNumber,
                Email = adminDTO.Email,
                State = adminDTO.State,
                City = adminDTO.City,
                Address = adminDTO.Address,
                Zipcode = adminDTO.Zipcode,
            };

            var adminResult = await userManager.CreateAsync(adminProfile, "Pass@123");

            if (adminResult.Succeeded)
            {
                List <string> roles = new List<string>();
                roles.Add("Admin");
                var adminRoleResult = await userManager.AddToRolesAsync(adminProfile, roles);
                if (adminRoleResult.Succeeded)
                {
                    return Ok("Admin was registered! Please login.");
                }
            }

            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var customerResult = await userManager.FindByIdAsync(id);
            if (customerResult != null)
            {
                var customer = new CustomerResponseDTO
                {
                    Id = customerResult.Id,
                    Name = customerResult.Name,
                    OrganizationName = customerResult.OrganizationName,
                    PhoneNumber = customerResult.PhoneNumber,
                    Email = customerResult.Email,
                    State = customerResult.State,
                    City = customerResult.City,
                    Address = customerResult.Address,
                    Zipcode = customerResult.Zipcode,
                };
                return Ok(customer);
            }
           
            return BadRequest();
        }

    }
}
