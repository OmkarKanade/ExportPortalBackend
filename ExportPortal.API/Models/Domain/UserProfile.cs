﻿using Microsoft.AspNetCore.Identity;

namespace ExportPortal.API.Models.Domain
{
    public class UserProfile : IdentityUser
    {
        public string Name { get; set; }
        public string OrganizationName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int Zipcode { get; set; }
        public Guid? VendorCategoryId { get; set; }
        public bool IsVerified { get; set; }


        // Navigation properties
        public VendorCategory VendorCategory { get; set; }

    }
}
