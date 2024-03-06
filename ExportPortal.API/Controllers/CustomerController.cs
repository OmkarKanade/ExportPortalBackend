using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExportPortal.API.Models.DTO;
using ExportPortal.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using ExportPortal.API.Mail;
using Microsoft.AspNetCore.Authorization;

namespace ExportPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly EmailService emailService;

        public CustomerController(UserManager<UserProfile> userManager, EmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? nameVal, [FromQuery] string? orgVal, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var dbCustomerResult = await userManager.GetUsersInRoleAsync("Customer");
            var customersResult = dbCustomerResult.AsQueryable();

            if (String.IsNullOrWhiteSpace(nameVal) == false)
            {
                customersResult = customersResult.Where(x => x.Name.ToLower().Contains(nameVal.ToLower()));
            }

            if (String.IsNullOrWhiteSpace(orgVal) == false)
            {
                customersResult = customersResult.Where(x => x.OrganizationName.ToLower().Contains(orgVal.ToLower()));
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            customersResult = customersResult.Skip(skipResults).Take(pageSize);

            if (customersResult != null)
            {
                List<CustomerResponseDTO> allCustomers = new List<CustomerResponseDTO>();

                foreach (var singleCustomer in customersResult)
                {
                    var customer = new CustomerResponseDTO
                    {
                        Id = singleCustomer.Id,
                        Name = singleCustomer.Name,
                        OrganizationName = singleCustomer.OrganizationName,
                        PhoneNumber = singleCustomer.PhoneNumber,
                        Email = singleCustomer.Email,
                        State = singleCustomer.State,
                        City = singleCustomer.City,
                        Address = singleCustomer.Address,
                        Zipcode = singleCustomer.Zipcode,
                    };

                    allCustomers.Add(customer);
                }

                return Ok(allCustomers);
            }

            return BadRequest("Something went wrong");
        }

        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] CustomerDTO customerDTO)
        {

            var customerProfile = new UserProfile
            {
                UserName = customerDTO.Email,
                Name = customerDTO.Name,
                OrganizationName = customerDTO.OrganizationName,
                PhoneNumber = customerDTO.PhoneNumber,
                Email = customerDTO.Email,
                State = customerDTO.State,
                City = customerDTO.City,
                Address = customerDTO.Address,
                Zipcode = customerDTO.Zipcode,
            };

            var customerResult = await userManager.CreateAsync(customerProfile, "Pass@123");

            if (customerResult.Succeeded)
            {
                List <string> roles = new List<string>();
                roles.Add("Customer");
                var customerRoleResult = await userManager.AddToRolesAsync(customerProfile, roles);
                if (customerRoleResult.Succeeded)
                {
                    SendWelcomeEmail(customerProfile);
                    return Ok("Customer was registered! Please login.");
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

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UserUpdateDTO userUpdateDTO)
        {

            var updateResult = await userManager.FindByIdAsync(id);

            if (updateResult != null)
            {
                if (userUpdateDTO.NewPassword != "")
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
            string subject = $"Welcome to Our Application Customer ID {user.Id}";
            string body = $"Dear {user.Name},\n\nWelcome to our application! Your username is: {user.UserName} and your password is: Pass@123\n\nBest regards,\nYour Application Team";

            emailService.SendEmail(user.Email, subject, body);
        }

    }
}
