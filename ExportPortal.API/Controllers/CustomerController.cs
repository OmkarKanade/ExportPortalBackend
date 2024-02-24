﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExportPortal.API.Models.DTO;
using ExportPortal.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExportPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;

        public CustomerController(UserManager<UserProfile> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customersResult = await userManager.GetUsersInRoleAsync("Customer");

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

    }
}
