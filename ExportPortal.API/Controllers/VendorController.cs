using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExportPortal.API.Models.DTO;
using ExportPortal.API.Models.Domain;
using ExportPortal.API.Mail;

namespace ExportPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly EmailService emailService;

        public VendorController(UserManager<UserProfile> userManager, EmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
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
                    SendWelcomeEmail(vendorProfile);
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

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UserUpdateDTO userUpdateDTO)
        {

            var updateResult = await userManager.FindByIdAsync(id);

            if (updateResult != null)
            {
                if (userUpdateDTO.NewPassword != null)
                {
                    var passResult = await userManager.ChangePasswordAsync(updateResult, userUpdateDTO.CurrentPassword, userUpdateDTO.NewPassword);
                    if (!passResult.Succeeded)
                    {
                        return BadRequest(passResult.Errors);
                    }
                }
                updateResult.Name = userUpdateDTO.Name;
                updateResult.OrganizationName = userUpdateDTO.OrganizationName;
                updateResult.PhoneNumber = userUpdateDTO.PhoneNumber;
                updateResult.State = userUpdateDTO.State;
                updateResult.City = userUpdateDTO.City;
                updateResult.Address = userUpdateDTO.Address;
                updateResult.Zipcode = userUpdateDTO.Zipcode;


                await userManager.UpdateAsync(updateResult);

                var update = new UserUpdateResponseDTO
                {
                    Id = updateResult.Id,
                    Name = updateResult.Name,
                    OrganizationName = updateResult.OrganizationName,
                    PhoneNumber = updateResult.PhoneNumber,
                    State = updateResult.State,
                    City = updateResult.City,
                    Address = updateResult.Address,
                    Zipcode = updateResult.Zipcode,
                };

                return Ok(update);
            }

            return BadRequest("Something went wrong");
        }

        private void SendWelcomeEmail(UserProfile user)
        {
            string subject = $"Welcome to Our Application Vendor ID {user.Id}";
            string body = $"Dear {user.Name},\n\nWelcome to our application! Your username is: {user.UserName} and your password is: Pass@123\n\nBest regards,\nYour Application Team";

            emailService.SendEmail(user.Email, subject, body);
        }

    }
}
